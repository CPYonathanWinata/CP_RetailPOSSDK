using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using LSRetailPosis.POSControls;
using LSRetailPosis.POSControls.Touch;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
	public partial class CPSettlementEDC : Form
	{
		static AutoResetEvent autoEventBCA = new AutoResetEvent(false);
		private bool _isReceived { get; set; }
		private bool _paymentSuccess { get; set; }
		static string _LastResponBCA { get; set; }
		static bool isEDCSettlementBCA { get; set; }

		public string transType;
		IPosTransaction posTransaction;

		public CPSettlementEDC(IPosTransaction _posTransaction, IApplication _application, string _transType)
		{
			InitializeComponent();
			posTransaction = _posTransaction;
			transType = _transType;
			if(transType == "10")
			{
				this.Text = "EDC Settlement";
						
			}
			else if (transType == "19")
			{
				this.Text = "Reprint last trx receipt";
			}

		}

		private void CPSettlementEDC_Load(object sender, EventArgs e)
		{
			string queryString = "";
			SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{

					queryString = @"SELECT BANK, PORT FROM AX.CPPILIHBANK WHERE BANK NOT LIKE 'QRIS%'";


					using (SqlCommand command = new SqlCommand(queryString, connectionStore))
					{
						if (connectionStore.State != ConnectionState.Open)
						{
							connectionStore.Open();
						}

						//Assigned Result to Dropdown
						SqlDataAdapter d = new SqlDataAdapter(queryString, connectionStore);
						DataTable dt = new DataTable();
						d.Fill(dt);

						cmbPilihBank.DataSource = dt;
						cmbPilihBank.DisplayMember = "BANK";
						cmbPilihBank.ValueMember = "PORT";
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connectionStore.State != ConnectionState.Closed)
					{
						connectionStore.Close();
					}
				}

		}
		private void flushCOMPort()
		{
			GME_Var.serialBCA.DiscardInBuffer();
			GME_Var.serialBCA.DiscardOutBuffer();
			GME_Var.serialBCA.BaseStream.Flush();

		}
		private void settlementBCA(string bankName)
		{
			//string transType = "";


			
			try
			{
				/*
				 * Set cancel button disabled during ECR Process
				 * to prevent user click Cancel during transaction in EDC
				 */


				GME_Var.transTypeBCA = transType;
				//Create BCA Object using GMECustom library
				ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
				GME_Var.approvalCodeBCA = string.Empty;
				GME_Var.respCodeBCA = string.Empty;
				RetailTransaction retailTransaction = posTransaction as RetailTransaction;

				//Set totalamount based on registered Amount
				int totalAmount = 0;//decimal.ToInt32(this.registeredAmount);

				//Try Open Port based on CPPILIHBANK data
				try
				{
					BCAOnline.openPort(cmbPilihBank.SelectedValue.ToString());
				}
				catch (Exception ex)
				{
					MessageBox.Show("error open port");
				}

				if (GME_Var.serialBCA.IsOpen)
				{
					flushCOMPort();
					//Connection open
					_isReceived = false;
					autoEventBCA.Reset();
					autoEventBCA = new AutoResetEvent(false);

					/*
					 * Send Data to ECR in here
					 * Noticed function WaitCallBank(receiveThreadBCA) is threading
					 * Means that it will wait for receiveThreadBCA to return value for continue process
					 */

					try
					{
						if(transType == "10")
						{
							BCAOnline.sendData(totalAmount.ToString(), transType, Connection.applicationLoc, posTransaction, "");
						
						}
						else if(transType == "19")
						{

							//if (GME_Var.invoiceNumberBCA != "")
							//{
								BCAOnline.sendData(totalAmount.ToString(), transType, Connection.applicationLoc, posTransaction, GME_Var.invoiceNumberBCA);
							//}
							/*else
							{
								MessageBox.Show("Belum ada transaksi yang bisa reprint");
								return;
							}*/
							
						}
						Thread.Sleep(500);
						lblWaitingRespond.Visible = true;
						//waitingForm.Show();
						ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
						autoEventBCA.WaitOne();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}


					if (GME_Var.transTypeBCA == transType)
					{

						lblWaitingRespond.Visible = false;
						//validate response from BCA here
						switch (GME_Var.respCodeBCA)
						{

							case "00":
							case "*0":
								#region ECRSuccess
								if (transType == "10")
								{
									//int transAmount = int.Parse(GME_Var.amountBCA.Substring(0, GME_Var.amountBCA.Length - 2));
									string transAmount = "";
									transAmount = GME_Var.amountBCA.ToString();
									//if(transAmount != "000000000000")
//                                    if (transAmount != 0)
									//{
										showError("Settlement success");

										string reffNum = GME_Var.dateBCA.ToString() + GME_Var.timeBCA.ToString();
										//reffnum, bankName,transdate,storeid
										//insert into AX.CPAMBILTUNAI
										SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

										try
										{
											string queryString = @"
											INSERT INTO AX.CPSETTLEMENT (
																		REFFNUM, 
																		BANKNAME,
																		STOREID, 
																		TRANSDATE, 
																			
																		[DATAAREAID],
																		[PARTITION]
																		)
											VALUES (
													@REFFNUM, 
													@BANKNAME,
													@STOREID, 
													@TRANSDATE,																			
													@DATAAREAID,
													@PARTITION
													)";


											using (SqlCommand command = new SqlCommand(queryString, connection))
											{
												command.Parameters.AddWithValue("@REFFNUM", reffNum);
												command.Parameters.AddWithValue("@BANKNAME", bankName);
												command.Parameters.AddWithValue("@STOREID", this.posTransaction.StoreId);
												command.Parameters.AddWithValue("@TRANSDATE", DateTime.Now);
												command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);
												command.Parameters.AddWithValue("@PARTITION", 1);

												if (connection.State != ConnectionState.Open)
												{
													connection.Open();
												}

												command.ExecuteNonQuery();
											}
										}
										catch (Exception ex)
										{
											LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
											MessageBox.Show(ex.ToString());
											throw;
										}
										finally
										{
											if (connection.State != ConnectionState.Closed)
											{
												connection.Close();
											}
										}

									/*}
									else
									{
										//MessageBox.Show("No Transaction,\nalready settled");
										showError("No Transaction,\nalready settled");
									}*/
								}
								else
								{
									showError("Reprint success");
								}
								
								
								
								break;
								#endregion
							case "54":
								showError("Decline Expired Card");
								break;
							case "55":
								showError("Decline Incorrect PIN");
								break;
							case "P2":
								showError("Read Card Error");
								break;
							case "P3":
								showError("User press Cancel on EDC");

								break;
							case "Z3":
								showError("EMV Card Decline");
								break;
							case "CE":
								showError("Connection Error/Line Busy");
								break;
							case "TO":
								showError("Connection Timeout");

								break;
							case "PT":
								showError("EDC Problem / EDC perlu di settlement");

								break;
							case "PS":
								showError("Settlement Failed");
								break;
							case "aa":
								showError("Decline (aa represent two digit alphanumeric value from EDC)");
								break;
							case "S2":
								showError("TRANSAKSI GAGAL");
								break;
							case "S3":
								showError("TXN BLM DIPROSES MINTA SCAN QR");
								break;
							case "S4":
								showError("TXN EXPIRED ULANGI TRANSAKSI");
								break;
							case "TN":
								showError("Topup Tunai Not Ready");
								break;
							case "NAK":
								showError("NAK");

								break;
							default:
								showError("Unknown Error: " + GME_Var.respCodeBCA);
								break;
						}
					}
					_isReceived = true;
					autoEventBCA.Reset();
				}
				else
				{

					showError("EDC Not Connected");
				}

				GME_Var.isPoint = false;
				//Close port when transaction done
				BCAOnline.closePort();
			}
			catch (Exception ex)
			{
				showError("Failed to send data");
				;   
			}
		}

		private void showError(string message)
		{
			/*using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
			{
				POSFormsManager.ShowPOSForm(dialog);
				//this.DialogResult = DialogResult.None;
			}*/
			MessageBox.Show(message);
			this.Close();

		}
		/*
		 * Threading function
		 * call and wait for response from EDC
		 */
		public void receiveThreadBCA(object stateInfo)
		{
			using (GME_Var.serialBCA)
			{
				// get start time
				DateTime start = DateTime.Now;
				// buffer for pushing received string data into
				StringBuilder indata = new StringBuilder();
				_LastResponBCA = "";
				//looptime is used to determined how many seconds has passed
				int loopTime = 0;
				GME_Var.serialBCA.DiscardInBuffer();
				GME_Var.serialBCA.DiscardOutBuffer();
				GME_Var.serialBCA.BaseStream.Flush();
				while (!_isReceived)
				{
					//set timeout when there is no response from EDC after some time
					/*if (loopTime > 60)
					{
						btnCancel.Enabled = true;
						GME_Var.serialBCA.Dispose();
						GME_Var.transTypeBCA = "01";
						GME_Var.respCodeBCA = "TO";
						_LastResponBCA = "";
						isEDCSettlementBCA = false;
						((AutoResetEvent)stateInfo).Set();
						_isReceived = true;
						break;
					}
					*/
					//add loopTime after each loop
					loopTime++;
					//mod by Yonathan to check whether port is open -  Custom QRIS
					if (GME_Var.serialBCA.IsOpen)
					{
						if (GME_Var.serialBCA.BytesToRead > 0)
						{
							//Add data to string when there is a response from EDC
							byte[] buffer = new byte[GME_Var.serialBCA.BytesToRead];
							int bytesRead = GME_Var.serialBCA.Read(buffer, 0, buffer.Length);
							if (bytesRead <= 0) return;

							buffer.Last();
							indata.Clear();
							foreach (byte b in buffer)
							{
								indata.Append(string.Format("{0:x2}", b));
							}
						}
						else
							//else wait for 1 second
							System.Threading.Thread.Sleep(1000);

						//if response found (indata string > 0 character)
						if (indata.Length > 0)
						{
							/*
							if (GME_Var.serialBCA.IsOpen) //send ACK that informs EDC that message is received.
							{
								byte[] _hexval = new byte[1]; // need to convert byte to byte[] to write
								_hexval[0] = 0x06;
								GME_Var.serialBCA.Write(_hexval, 0, 1);
								//Thread.Sleep(1);
							}*/
							//check if current indata string is the same with last response
							if (indata.ToString() != _LastResponBCA)
							{
								string hex = indata.ToString();
								ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
								//if length > 2 and not 06 (ACK)
								if (hex.Length > 2 && hex != "06")
								{
									//mapping response to BCADataCapture (GMECustom library)
									//ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
									BCAOnline.MappingText(hex);
									GME_Var.serialBCA.Dispose();
									_LastResponBCA = hex;
									isEDCSettlementBCA = false;
									((AutoResetEvent)stateInfo).Set();
									_isReceived = true;

									//send ACK
									ElectronicDataCaptureBCA BCAOnlineAck = new ElectronicDataCaptureBCA();
									try
									{
										BCAOnlineAck.openPort(cmbPilihBank.SelectedValue.ToString());
									}
									catch (Exception ex) { }

									if (GME_Var.serialBCA.IsOpen)
									{
										byte[] _hexval = new byte[1]; // need to convert byte to byte[] to write
										_hexval[0] = 0x06;
										GME_Var.serialBCA.Write(_hexval, 0, 1);
										//Thread.Sleep(1);
									}
									else
									{
										showError("EDC Not Connected");
									}
									//end send ACK
									break;
								}
								else if (hex == "15") //if NAK, then re-send the command
								{

									//btnCancel.Enabled = true;
									GME_Var.serialBCA.Dispose();
									GME_Var.transTypeBCA = "01";
									GME_Var.respCodeBCA = "NAK";
									_LastResponBCA = "";
									isEDCSettlementBCA = false;
									//((AutoResetEvent)stateInfo).Set();
									_isReceived = true;
									break;
									//string textReq = "P01000000000100000000000000                       000000000000002020N                                                                                   ";
									//byte[] hexstring = Encoding.ASCII.GetBytes(textReq);
									//ElectronicDataCaptureBCA BCAOnlineNAK = new ElectronicDataCaptureBCA();


									/*try
									{
										BCAOnlineNAK.openPort(cmbPilihBank.SelectedValue.ToString());
									}
									catch (Exception ex) { }
									if (GME_Var.serialBCA.IsOpen)
									{
									   

										foreach (byte hexval in hexstring)
										{
											byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
											GME_Var.serialBCA.Write(_hexval, 0, 1);
											//Thread.Sleep(1);
										}
									}
									else
									{
										showError("EDC Not Connected");
									}
									*/

									//ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
									//BCAOnline.sendData("1", "01", Connection.applicationLoc, posTransaction, refNumber);

									//send ACK
									/*
									ElectronicDataCaptureBCA BCAOnlineNAK = new ElectronicDataCaptureBCA();

									string textReq = "P01000000000100000000000000                       000000000000002020N                                                                                   "; 
									byte[] hexstring = Encoding.ASCII.GetBytes(textReq);
									try
									{
										BCAOnlineNAK.openPort(cmbPilihBank.SelectedValue.ToString());
									}
									catch (Exception ex) { }

									if (GME_Var.serialBCA.IsOpen)
									{
									   

										foreach (byte hexval in hexstring)
										{
											byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
											GME_Var.serialBCA.Write(_hexval, 0, 1);
											//Thread.Sleep(1);
										}
									}
									else
									{
										showError("EDC Not Connected");
									}*/
									//break;
								}
							}
							else
							{
								_isReceived = true;
								//send ACK
								ElectronicDataCaptureBCA BCAOnlineAck = new ElectronicDataCaptureBCA();
								try
								{
									BCAOnlineAck.openPort(cmbPilihBank.SelectedValue.ToString());
								}
								catch (Exception ex) { }

								if (GME_Var.serialBCA.IsOpen)
								{
									byte[] _hexval = new byte[1]; // need to convert byte to byte[] to write
									_hexval[0] = 0x06;
									GME_Var.serialBCA.Write(_hexval, 0, 1);
									//Thread.Sleep(1);
								}
								else
								{
									showError("EDC Not Connected");
								}
								//end send ACK
								break;
							}
						}
					}
					else
					{
						btnCancel.Enabled = true;
						GME_Var.serialBCA.Dispose();
						GME_Var.transTypeBCA = "01";
						GME_Var.respCodeBCA = "TO";
						_LastResponBCA = "";
						isEDCSettlementBCA = false;
						((AutoResetEvent)stateInfo).Set();
						_isReceived = true;
						break;
					}
					//

				}
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string bankName = cmbPilihBank.GetItemText(cmbPilihBank.SelectedItem);
			settlementBCA(bankName);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
	}
}
