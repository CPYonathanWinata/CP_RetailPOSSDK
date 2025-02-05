/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

//Microsoft Dynamics AX for Retail POS Plug-ins 
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System.ComponentModel.Composition;
using System.Data.SqlClient;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using System.Windows.Forms;
using System;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Linq;
using LSRetailPosis.Transaction;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
//using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;
using GME_Custom.GME_EngageFALWSServices;
using LSRetailPosis.Transaction.Line.TenderItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Xml;
using LSRetailPosis.POSProcesses;
using System.Collections.ObjectModel;
using System.Globalization;
using LSRetailPosis.Transaction.Line.SaleItem;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.TransactionTriggers
{
	[Export(typeof(ITransactionTrigger))]
	public class TransactionTriggers : ITransactionTrigger
	{
		[Import]
		public IApplication Application { get; set; }
        //APIAccess.APIParameter.RetailTransactionExtended retailExtended ;//= new APIAccess.APIParameter.RetailTransactionExtended();

        
		static SerialPort mySerialPort;
		//Generate Hex For Request, starts with 4 static bytes
		byte[] bytestosend = { 0x02, 0x42, 0x4E, 0x49 };
		int countRetry = 0;
		Dictionary<string, string> dict = new Dictionary<string, string>();
		string topupName = "";

		// CPAPIEZEELINK CUSTOMIZATION

		//DEVELOPMENT
		//const int DEVICE_ID = 57;
		//const string DEVICE_KEY = "b41c4c2da562a283fbf0dc94184ad5792a865be1";
		//const string MERCHANT_ID = "MD190805";
		//const string MERCHANT_KEY = "9df983ej9fgu0w3rm89ug08r9ty8390m5nar8ytm";
/*
		//PRODUCTION
		const int DEVICE_ID = 143;
		const string DEVICE_KEY = "12905b460f97465880de6e7da58c20b8";
		const string MERCHANT_ID = "65496501";
		const string MERCHANT_KEY = "10F926CB69ED4D0893A218EBD92E4576";

		string token = "";
		decimal amountEarn = 0;
		// END CPAPIEZEELINK CUSTOMIZATION
*/
		//CPTOPUPBNI
		string comPort = "COM12";
		//END CPTOPUPBNI

		#region Constructor - Destructor

		string receiptId = string.Empty;
		string PanNumber = string.Empty;
		Dictionary<string, string> value = new Dictionary<string, string>();
		string approvalCode = "";

		public TransactionTriggers()
		{
			mySerialPort = new SerialPort(comPort);

			mySerialPort.BaudRate = 9600;
			mySerialPort.Parity = Parity.None;
			mySerialPort.StopBits = StopBits.One;
			mySerialPort.DataBits = 8;
			mySerialPort.Handshake = Handshake.None;
			mySerialPort.RtsEnable = true;

			mySerialPort.ReadTimeout = 50000;
			mySerialPort.WriteTimeout = 50000;



			// Get all text through the Translation function in the ApplicationLocalizer
			// TextID's for TransactionTriggers are reserved at 50300 - 50349
		}

		//added customization by Yonathan for add item to stock
		// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
		public class parmRequest
		{
			public string ITEMID { get; set; }
			public string QTY { get; set; }
			public string UNITID { get; set; }
			public string DATAAREAID { get; set; }
			public string WAREHOUSE { get; set; }
			public string TYPE { get; set; }
			public string REFERENCESNUMBER { get; set; }
			public string RETAILVARIANTID { get; set; }
		}

		public class parmMultiRequest
		{
			public List<parmRequest> parmRequest { get; set; }
		}

		public static class MyJsonConverter
		{
			/// <summary>
			/// Deserialize an from json string
			/// </summary>
			public static T Deserialize<T>(string body)
			{
				using (var stream = new MemoryStream())
				using (var writer = new StreamWriter(stream))
				{
					writer.Write(body);
					writer.Flush();
					stream.Position = 0;
					return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
				}
			}

			/// <summary>
			/// Serialize an object to json
			/// </summary>
			public static string Serialize<T>(T item)
			{
				using (MemoryStream ms = new MemoryStream())
				{
					new DataContractJsonSerializer(typeof(T)).WriteObject(ms, item);
					return Encoding.Default.GetString(ms.ToArray());
				}
			}
		}

		//

		~TransactionTriggers()
		{
			mySerialPort.Close();
		}

		#endregion

		public void BeginTransaction(IPosTransaction posTransaction)
		{
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.BeginTransaction", "When starting the transaction...", LSRetailPosis.LogTraceLevel.Trace);
		}

		/// <summary>
		/// Triggered during save of a transaction to database.
		/// </summary>
		/// <param name="posTransaction">PosTransaction object.</param>
		/// <param name="sqlTransaction">SqlTransaction object.</param>
		/// <remarks>
		/// Use provided sqlTransaction to write to the DB. Don't commit, rollback transaction or close the connection.
		/// Any exception thrown from this trigger will rollback the saving of pos transaction.
		/// </remarks>
		public void SaveTransaction(IPosTransaction posTransaction, SqlTransaction sqlTransaction)
		{
			ApplicationLog.Log("TransactionTriggers.SaveTransaction", "Saving a transaction.", LogTraceLevel.Trace);
			CPupdateNULLOrder(sqlTransaction);

			if (posTransaction is IRetailTransaction)
			{
				string commandString = @" UPDATE [ax].RETAILTRANSACTIONTABLE  
									SET CPNoOrder = B.NOORDER ,
									CPDateOrder = B.DATEORDER ,
									CPNameOrder = B.NAMEORDER
									from [ax].RETAILTRANSACTIONTABLE A
									JOIN CPOrderTable B
									on A.TRANSACTIONID = B.TRANSACTIONID
									WHERE A.TRANSACTIONID = (@TRANSACTIONID)";

				using (SqlCommand sqlCmd = new SqlCommand(commandString, sqlTransaction.Connection, sqlTransaction))
				{
					sqlCmd.Parameters.Add(new SqlParameter("@TRANSACTIONID", posTransaction.TransactionId));
					sqlCmd.ExecuteNonQuery();
				}
			}

			//Example:
			//if (posTransaction is IRetailTransaction)
			//{
			//    string commandString = "INSERT INTO PARTNER_CUSTOMTRANSACTIONTABLE VALUES (@VAL1)";

			//    using (SqlCommand sqlCmd = new SqlCommand(commandString, sqlTransaction.Connection, sqlTransaction))
			//    {
			//        sqlCmd.Parameters.Add(new SqlParameter("@VAL1", posTransaction.PartnerData.TestData));
			//        sqlCmd.ExecuteNonQuery();
			//    }
			//}
		}

		public void PreEndTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
		{
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreEndTransaction", "When concluding the transaction, prior to printing and saving...", LSRetailPosis.LogTraceLevel.Trace);

			// UPDATE TABLE CPEZSELECTEDCUSTOMER
	 /*       if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{
				SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

				try
				{
					string queryString = "SELECT TOP 1 TRANSACTIONID, CARDNUMBER FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

					using (SqlCommand cmd = new SqlCommand(queryString, connection))
					{
						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								if (reader["TRANSACTIONID"].ToString() == posTransaction.TransactionId)
								{
									string cardNumber = reader["CARDNUMBER"].ToString();
									reader.Close();
									reader.Dispose();
									cmd.Dispose();
									connection.Close();
									connection.Dispose();

									EarnPoint(cardNumber, posTransaction);
								}
							}
							reader.Close();
							reader.Dispose();
						}
					}
				}
				catch (SqlException ex)
				{
					throw new Exception("Format Error", ex);
				}
				finally
				{
					if (connection != null)
						connection.Dispose();
				}
			}*/
			// END UPDATE TABLE CPEZSELECTEDCUSTOMER

			// ECR BCA CUSTOMIZE

			if (GME_Var.isPayTransaction)
			{
				//Jacksonsoft.WaitWindow.Show(DoProgressPreEndTransaction, "Please Wait ...");

				queryData data = new queryData();
				RetailTransaction retailTransaction = posTransaction as RetailTransaction;
				IntegrationService integrationService = new IntegrationService();

				if (retailTransaction != null)
				{
					if (retailTransaction.TransactionType == PosTransaction.TypeOfTransaction.Sales)
					{
				   //     receiptId = GME_Method.generateReceiptNumberSequence(retailTransaction.TransactionType.ToString(), retailTransaction.AmountDue);
					//    GME_Var.receiptID = receiptId;

						if (GME_Var.isEDCBCA && GME_Var.respCodeBCA != "00") { preTriggerResult.ContinueOperation = false; GME_Var.isEDCBCA = false; }
						//   if (GME_Var.isEDCMandiri && GME_Var.respCodeMandiri != "00") { preTriggerResult.ContinueOperation = false; GME_Var.isEDCMandiri = false; }

						List<lineItem> listLineItem = new List<lineItem>();
						string result9100 = string.Empty;
						GME_Var.totalDPP = 0;

						int index = 1;
				  

						List<decimal> listCash = new List<decimal>();
						List<decimal> listCashVoucher = new List<decimal>();
						//            List<decimal> listEDCBCA = new List<decimal>();
						//           List<decimal> listEDCMDR = new List<decimal>();

				   
					}
				}
			}
		}

		public void PostEndTransaction(IPosTransaction posTransaction)
		{
            APIAccess.APIFunction APIFunction = new APIAccess.APIFunction();
            //MessageBox.Show(APIAccess.APIAccessClass.idText.ToString(), "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information); ;
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PostEndTransaction", "When concluding the transaction, after printing and saving", LSRetailPosis.LogTraceLevel.Trace);
            bool foundGiftCardPayment = false; //add by Yoanthan 10092024
			if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{


				// CPNEWPAYMENTPOS CUSTOMIZATION
                CPNEWPAYMENTPOS(posTransaction);
				// END CPNEWPAYMENTPOS CUSTOMIZATION

				//Top Up BNI Tapcash CUSTOMIZATION - ES 09092019
				CPTOPUPBNI(posTransaction);
				//END Top Up BNI Tapcash CUSTOMIZATION - ES 09092019

				//CPAPIEZEELINK CUSTOMIZATION - ES 25 OCTOBER 2019
		//        CPAPIEZEELINK(posTransaction);
				//END CPAPIEZEELINK CUSTOMIZATION - ES 25 OCTOBER 2019

				// CPVOUCHERCODE CUSTOMIZATION
				CPVOUCHERCODE(posTransaction); //enable again 07/03/2023 by Yonathan
				// END CPVOUCHERCODE CUSTOMIZATION


				//added customization by yonathan 08/09/2022
				//loop through all of the item transaction
				AddItemTransaction(posTransaction); //temporarily disable for testing because not implemented yet.
				//end added

                //add by yonathan to update receiptid on giftcard 10092024
                RetailTransaction retail = posTransaction as RetailTransaction;
                foreach (var tenderLines in retail.TenderLines)
                {
                    if(tenderLines.ToString() == "LSRetailPosis.Transaction.Line.TenderItem.GiftCertificateTenderLineItem")
                    {
                        foundGiftCardPayment = true;
                    }
                    
                }
                //[LSRetailPosis.Transaction.Line.TenderItem.GiftCertificateTenderLineItem] = {LSRetailPosis.Transaction.Line.TenderItem.GiftCertificateTenderLineItem}
                if(foundGiftCardPayment == true)
                {
                    UpdateGiftCardReceiptId(posTransaction);
                }
                
                //end
                

			}
            //clear the value - 16012025
            APIFunction.clearRetailTransExtended();
            
		 
		}

        private void UpdateGiftCardReceiptId(IPosTransaction posTransaction)
        {
            
            if (Application.TransactionServices.CheckConnection())
            {
                ReadOnlyCollection<object> containerArray = new ReadOnlyCollection<object>(new object[0]);
                
                try
                {
                    object[] parameterList = new object[] 
							{
								posTransaction.StoreId,
								posTransaction.ReceiptId,
                                posTransaction.TerminalId,
								posTransaction.TransactionId								
							};


                    containerArray = Application.TransactionServices.InvokeExtension("updateGiftCardReceiptId", parameterList);
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    throw;
                }
            }
           
        }

		public static string ConvertStringToHex(string asciiString)
		{
			string hex = "";
			foreach (char c in asciiString)
			{
				int tmp = c;
				hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
			}
			return hex;
		}

		public static byte[] StringToByteArray(String hex)
		{
			int NumberChars = hex.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			return bytes;
		}

		private void mySerialPort_DataReceived(
					object sender,
					SerialDataReceivedEventArgs e)
		{
			SerialPort sp = (SerialPort)sender;
			string indata = sp.ReadExisting();
			//data received on serial port is asssigned to "indata" string

			if (indata.Length > 1)
			{
				if (indata[149] != 0x30 && indata[150] != 0x30)
				{
					if (countRetry < 3)
					{
						countRetry++;
						mySerialPort.Write(bytestosend, 0, bytestosend.Length);
					}
					else
					{
						MessageBox.Show(topupName + " failed");

						mySerialPort.Close();
						bytestosend = new byte[4];
						bytestosend[0] = 0x02;
						bytestosend[1] = 0x42;
						bytestosend[2] = 0x4E;
						bytestosend[3] = 0x49;
					}
				}
				else
				{
					MessageBox.Show(topupName + " success");

					mySerialPort.Close();
					bytestosend = new byte[4];
					bytestosend[0] = 0x02;
					bytestosend[1] = 0x42;
					bytestosend[2] = 0x4E;
					bytestosend[3] = 0x49;
				}
			}

		}

		public void PreVoidTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
		{
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreVoidTransaction", "Before voiding the transaction...", LSRetailPosis.LogTraceLevel.Trace);

			SqlConnection connectionEZ = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

			try
			{
				string queryString = @"DELETE FROM CPEZSELECTEDCUSTOMER";

				using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
				{
					if (connectionEZ.State != ConnectionState.Open)
					{
						connectionEZ.Open();
					}

					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				throw new Exception("Format Error", ex);
			}
			finally
			{
				connectionEZ.Close();
			}

			try
			{
				string queryString = @"DELETE FROM CPEXTVOUCHERCODETMP";

				using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
				{
					if (connectionEZ.State != ConnectionState.Open)
					{
						connectionEZ.Open();
					}

					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				throw new Exception("Format Error", ex);
			}
			finally
			{
				connectionEZ.Close();
			}

		}

		public void PostVoidTransaction(IPosTransaction posTransaction)
		{
			string source = "TransactionTriggers.PostVoidTransaction";
			string value = "After voiding the transaction...";
			LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Trace);
			LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);
		}

		public void PreReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction, IPosTransaction posTransaction)
		{
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreReturnTransaction", "Before returning the transaction...", LSRetailPosis.LogTraceLevel.Trace);
		}

		public void PostReturnTransaction(IPosTransaction posTransaction)
		{
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PostReturnTransaction", "After returning the transaction...", LSRetailPosis.LogTraceLevel.Trace);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
		public void PreConfirmReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction)
		{
			LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreConfirmReturnTransaction", "Before confirming return transaction...", LogTraceLevel.Trace);
		}

		public void CPupdateNULLOrder(SqlTransaction sqlTransaction)
		{
			string commandString = @" UPDATE [ax].RETAILTRANSACTIONTABLE  
									SET CPNoOrder = '' ,
									CPDateOrder = '1900-01-01' ,
									CPNameOrder = ''
									WHERE CPNoOrder IS NULL";

			using (SqlCommand sqlCmd = new SqlCommand(commandString, sqlTransaction.Connection, sqlTransaction))
			{

				sqlCmd.ExecuteNonQuery();
			}

		}

		public void PostCancelTransaction(IPosTransaction posTransaction)
		{
			throw new System.NotImplementedException();
		}

		public void PreCancelTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction, IPosTransaction posTransaction)
		{
			throw new System.NotImplementedException();
		}

		public void PreRollbackTransaction(IPreTriggerResult preTriggerResult, IPosTransaction originalTransaction)
		{
			throw new System.NotImplementedException();
		}

		//CUSTOMIZE METHOD
		#region customize
 /*       private int getToken()
		{
			string newToken = "";

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
									   SecurityProtocolType.Tls11 |
									   SecurityProtocolType.Tls12;
			string reqUrl = "https://pfm.cp.co.id/api/ezeelink/token/production/get";
			int return_code = 0;
			string message = "";

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqUrl);
			req.Method = "POST";
			req.Credentials = CredentialCache.DefaultCredentials;
			req.Accept = "text/json";
			req.Headers["Authorization"] = "retailstore01*";

			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				string content = string.Empty;
				using (var stream = resp.GetResponseStream())
				{
					using (var sr = new StreamReader(stream))
					{
						content = sr.ReadToEnd();
					}
				}
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				dynamic item = serializer.Deserialize<object>(content);

				return_code = item["return_code"];
				message = (string)item["message"];

				if (return_code == 200)
				{
					newToken = (string)item["result"];

					if (resp != null)
						resp.Close();

					token = newToken;
					return 1;
				}
				else
				{
					MessageBox.Show(message);
					return 0;
				}
			}
		}
		 */
		/*
		private void getRedeemValue(ref decimal amount, string cardNumber, IPosTransaction posTransaction)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			
			try
			{
				string queryString = "SELECT AMOUNT FROM AX.CPEZREDEEM WHERE TRANSACTIONID = @TRANSACTIONID AND CARDNUMBER = @CARDNUMBER";

				using (SqlCommand cmd = new SqlCommand(queryString, connection))
				{
					cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
					cmd.Parameters.AddWithValue("@CARDNUMBER", cardNumber);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							amount = int.Parse(reader[0].ToString());
						}
					}
				}
			}
			catch (SqlException ex)
			{
				MessageBox.Show("No Internet Connection, Current transaction will not earn point. (10)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				//LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
			}
			finally
			{
				connection.Close();
			}
		}
		 * */

	 /*   private void addToCPEZEARN(IPosTransaction posTransaction)
		{
			SqlConnection connectionEZ = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

			try
			{
				string queryCheck = @"SELECT TOP 1 TRANSACTIONID 
										FROM CPEZSELECTEDCUSTOMER 
										WHERE 
											POINTISSUE IS NOT NULL AND 
											POINTBALANCE IS NOT NULL AND
											TRANSDATETIMESTR IS NOT NULL AND
											M_TRAN_ID IS NOT NULL";

				using (SqlCommand cmdCheck = new SqlCommand(queryCheck, connectionEZ))
				{
					if (connectionEZ.State != ConnectionState.Open)
					{
						connectionEZ.Open();
					}

					using (SqlDataReader reader = cmdCheck.ExecuteReader())
					{
						if (reader.Read())
						{
							reader.Close();
							reader.Dispose();
							cmdCheck.Dispose();

							string queryString = @"INSERT INTO AX.CPEZEARN
															(
																RECEIPTID,
																TRANSACTIONID,
																CARDNUMBER,
																CHANNEL,
																AMOUNT,
																EARNPOINTS,
																CUSTOMERNAME,
																M_TRAN_ID,
																LASTBALANCE,
																TRANSDATETIMESTR,
																ACCOUNTSTATUS,
																MEMBERSHIP,
																STORE,
																TERMINAL,
																DATAAREAID,
																PARTITION,
																RECID
															)
														SELECT 
															'' AS RECEIPTID, 
															TRANSACTIONID,
															CARDNUMBER,
															1 AS CHANNEL,
															@AMOUNT AS AMOUNT,
															POINTISSUE,
															NAME,
															M_TRAN_ID,
															POINTBALANCE,
															TRANSDATETIMESTR,
															ACCOUNTSTATUS,
															MEMBERSHIP,
															@STORE AS STORE,
															@TERMINAL AS TERMINAL,
															@DATAAREAID AS DATAAREAID,
															1 AS PARTITION,
															1 AS RECID
														FROM CPEZSELECTEDCUSTOMER";

							using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
							{
								//cmd.Parameters.AddWithValue("@RECEIPTID", ((RetailTransaction)posTransaction).ReceiptId);
								cmd.Parameters.AddWithValue("@AMOUNT", amountEarn);
								cmd.Parameters.AddWithValue("@STORE", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);
								cmd.Parameters.AddWithValue("@TERMINAL", LSRetailPosis.Settings.ApplicationSettings.Database.TerminalID);
								cmd.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

								if (connectionEZ.State != ConnectionState.Open)
								{
									connectionEZ.Open();
								}

								cmd.ExecuteNonQuery();
							}
						}
					}
				}
			}
			catch (SqlException ex)
			{
				//throw new Exception("Format Error", ex);
				MessageBox.Show("No Internet Connection, Current transaction will not earn point. (4)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
			}
			finally
			{
				string queryString = @"DELETE FROM CPEZSELECTEDCUSTOMER";

				using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
				{
					if (connectionEZ.State != ConnectionState.Open)
					{
						connectionEZ.Open();
					}

					cmd.ExecuteNonQuery();
				}

				connectionEZ.Close();
			}
		} */
		/*
		private void EarnPoint(string cardNumber, IPosTransaction posTransaction)
		{
			try
			{
		  //      if (getToken() == 1)
		   //     {
					decimal amount = 0;
					getRedeemValue(ref amount, cardNumber, posTransaction);

					Random rand = new Random();
					int responseCode = 0;
					string error_id = "";
					string M_TRANS_ID = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-" + rand.Next(1, 10000);

					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
											   SecurityProtocolType.Tls11 |
											   SecurityProtocolType.Tls12;
					//string reqCheckUrl = "https://api.ezeelink.co.id/v2/staging/EZ_IssueCardPoint"; // Development
		//            string reqCheckUrl = "https://api.ezeelink.co.id/v2/apiezee/EZ_IssueCardPoint"; // Production
/*
					ASCIIEncoding encoder = new ASCIIEncoding();
					byte[] data = encoder.GetBytes("");

					HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
					reqCheck.Method = "POST";
					reqCheck.Credentials = CredentialCache.DefaultCredentials;
					reqCheck.Accept = "text/json";
					reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
					reqCheck.Headers["device_key"] = DEVICE_KEY;
					reqCheck.Headers["access_token"] = token;
					reqCheck.Headers["merchant_id"] = MERCHANT_ID;
					reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
					reqCheck.Headers["m_trans_id"] = M_TRANS_ID;
					reqCheck.Headers["card_number"] = cardNumber;
					reqCheck.Headers["value"] = ((int)((RetailTransaction)posTransaction).NetAmountWithTax - (int) amount).ToString();
					reqCheck.ContentType = "application/json";
					reqCheck.ContentLength = data.Length;

					if (((int)((RetailTransaction)posTransaction).NetAmountWithTax - (int)amount) <= 0)
					{
						return;
					}

					using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
					{
						string content = string.Empty;
						using (var stream = resp.GetResponseStream())
						{
							using (var sr = new StreamReader(stream))
							{
								content = sr.ReadToEnd();
							}
						}
						JavaScriptSerializer serializer = new JavaScriptSerializer();
						dynamic item = serializer.Deserialize<object>(content);

						responseCode = item["response_code"];

						if (responseCode == 200)
						{
							error_id = item["response_data"]["error_id"];

							if (error_id == "0000")
							{
								string pointIssue = item["response_data"]["results"]["point_issue"];
								string pointBal = item["response_data"]["results"]["point_bal"];
								string transDateTime = item["response_data"]["results"]["process_date_yyyyMMdd"];
								//string m_tran_id = item["response_data"]["results"]["m_tran_id"];
								string m_tran_id = item["response_data"]["results"]["ezee_tran_id"];

								amountEarn = ((RetailTransaction)posTransaction).NetAmountWithTax - amount;
				*/
			  /*                  SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

								try
								{
									string queryString = @"UPDATE CPEZSELECTEDCUSTOMER SET
															POINTISSUE = @POINTISSUE,
															POINTBALANCE = @POINTBALANCE,
															TRANSDATETIMESTR = @TRANSDATETIME,
															M_TRAN_ID = @M_TRAN_ID
														WHERE
															TRANSACTIONID = @TRANSACTIONID AND
															CARDNUMBER = @CARDNUMBER
														";
				
									using (SqlCommand cmd = new SqlCommand(queryString, connection))
									{
										cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
										cmd.Parameters.AddWithValue("@CARDNUMBER", cardNumber);
										cmd.Parameters.AddWithValue("@POINTISSUE", pointIssue);
										cmd.Parameters.AddWithValue("@POINTBALANCE", pointBal);
										cmd.Parameters.AddWithValue("@TRANSDATETIME", transDateTime);
										cmd.Parameters.AddWithValue("@M_TRAN_ID", m_tran_id);

										if (connection.State != ConnectionState.Open)
										{
											connection.Open();
										}

										cmd.ExecuteNonQuery();
									}
						/*        }
								catch (SqlException ex)
								{
									MessageBox.Show("No Internet Connection, Current transaction will not earn point. (1)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

									//LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
								}
								finally
								{
									connection.Close();
								}

								//Add to CPEZEARN
					//            addToCPEZEARN(posTransaction);
							}
							else
							{
								MessageBox.Show("No Internet Connection, Current transaction will not earn point. (2)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								/*using (EventLog eventLog = new EventLog("Application"))
								{
									eventLog.Source = "Application";
									eventLog.WriteEntry(item["response_data"]["error_text"], EventLogEntryType.Error);
								}*/
			  //              }
			//            }
		/*                else
						{
							MessageBox.Show("No Internet Connection, Current transaction will not earn point. (3)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							/*using (EventLog eventLog = new EventLog("Application"))
							{
								eventLog.Source = "Application";
								eventLog.WriteEntry(item["response_message"], EventLogEntryType.Error);
							}*/
	  //                  }
	//                }
	  //          }
	 /*       }
			catch
			{

			}
			
		}*/

        //CPIADMFEE
        public decimal GetAdmFee(int _tenderId)
        {
            string admFee = "";
            decimal amountAdmFee = 0;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @"SELECT ADMFEE 
                                        FROM ax.CPBANKADM 
                                        WHERE TENDERTYPEID = @TENDERID 
                                        AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date)";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TENDERID", _tenderId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            admFee = reader[0].ToString();
                             
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            amountAdmFee = (Math.Truncate(Convert.ToDecimal(admFee) * 1000m) / 1000m);
            return amountAdmFee;
        }


        private void CPNEWPAYMENTPOS(IPosTransaction posTransaction)
		{
            //add new field to this //16012025 
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			int id = 0;
			string transactionID = "";
			string receiptID = "";
			string PaymentAmount = "";
			string NoReffTransaction = "";
			int TenderID = 0;
			string TenderName = "";
			string PurchaserName = "";
			string PurchaserPhone = "";
			string store = "";
			string transDate = "";
			string dataareaID = "";
			string partition = "";
			int tenderQRIS = 30; //tender QRIS PROD 30, Dev 31
			try
			{
				string queryString = @"
											SELECT 
												tmp.ID,
												tmp.TransactionID,
												rt.ReceiptID,
												tmp.PaymentAmount,
												tmp.NoReffTransaction,
												tmp.TenderID,
												tmp.TenderName,
												tmp.PurchaserName,
												tmp.PurchaserPhone,
												tmp.Store,
												tmp.TransDate,
												tmp.[DATAAREAID],
												tmp.[PARTITION]
											FROM AX.[CPNEWPAYMENTPOSTMP] tmp
											INNER JOIN AX.[RETAILTRANSACTIONTABLE] rt
											ON tmp.TRANSACTIONID = rt.TRANSACTIONID AND tmp.STORE = rt.STORE
											ORDER BY ID DESC
											";

				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							id = (int)reader[0];
							transactionID = reader[1] + "";
							receiptID = reader[2] + "";
							PaymentAmount = reader[3] + "";
							NoReffTransaction = reader[4] + "";
							TenderID = (int)reader[5];
							TenderName = reader[6] + "";
							PurchaserName = reader[7] + "";
							PurchaserPhone = reader[8] + "";
							store = reader[9] + "";
							//transDate = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}", reader[10]) + ""; // reader[10].ToString("yyyy-mm-dd hh:mm:ss:ms") + ""; //2022/09/20 16:10:27
                            transDate = ((DateTime)reader[10]).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture); //change to this to match the globalization format - yonathan 14102024

							dataareaID = reader[11] + "";
							partition = reader[12] + "";
						}
					}
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
			finally
			{
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}

			#region CPECRBCA



			/*
			 * Move data from AX.CPAMBILTUNAITEMP to AX.CPAMBILTUNAI
			 * after that, delete data from AX.CPAMBILTUNAITEMP
			 */
			if (TenderID == 32 || TenderID == 33 || TenderID == tenderQRIS)  //additional tender ID for QRIS by Yonathan 17/01/2023 QRIS DEV 31 PROD 30
			{
				string amountECR = "";
				string bankECR = "";
				string cardECR = "";
				string storeECR = "";
				string tenderECR = "";
				string transactionIdECR = "";
				string transactionStatusECR = "";
				string transactionTypeECR = "";
				string transDateECR = "";
				string trxIdECR = "";
				string dataAreaIdECR = "";
				string partitionECR = "";
                string admFeeECR = ""; //add admfee for fee tarik tunai by yonathan //CPIADMFEE
				try 
				{
					string queryString = @"
											SELECT
												AMOUNT,
												BANK,
												CARDNUMBER,
												STORE,
												TENDER,
												TRANSACTIONID,
												TRANSACTIONSTATUS,
												TRANSACTIONTYPE,
												TRANSDATE,
												TRXIDEDC,
												DATAAREAID,
                                                ADMFEE,
												PARTITION
											FROM
												AX.CPAMBILTUNAITEMP
											WHERE TRANSACTIONID = @TRANSACTIONID
											ORDER BY ID DESC
											";

					using (SqlCommand command = new SqlCommand(queryString, connection))
					{
						command.Parameters.AddWithValue("@TRANSACTIONID", transactionID);

						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								amountECR = reader[0] + "";
								bankECR = reader[1] + "";
								cardECR = reader[2] + "";
								storeECR = reader[3] + "";
								tenderECR = reader[4] + "";
								transactionIdECR = reader[5] + "";
								transactionStatusECR = reader[6] + "";
								transactionTypeECR = reader[7] + "";
								transDateECR = reader[8] + "";
								trxIdECR = reader[9] + "";
								dataAreaIdECR = reader[10] + "";
                                admFeeECR = reader[11] + "";
							}
						}
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}

				if (id != 0)
				{
					try
					{
                        //decimal admFee = 0;
                        //admFee = GetAdmFee(TenderID);
                        //ADD ADMFEE FOR INSERT BY YONATHAN 13/06/2024 //CPIADMFEE


						string queryString = @"
											INSERT INTO AX.CPAMBILTUNAI (
																			TRANSACTIONID, 
																			RECEIPTID,
																			TRXIDEDC,
																			CARDNUMBER, 
																			AMOUNT, 
																			TRANSACTIONTYPE, 
																			BANK, 
																			TENDER, 
																			STORE,
																			TRANSDATE,
																			TRANSACTIONSTATUS,
                                                                            ADMFEE,
                                                                            FEE,
																			[DATAAREAID],
																			[PARTITION]
																			)
											VALUES (
													@TRANSACTIONID, 
													@RECEIPTID,
													@TRXIDEDC,
													@CARDNUMBER, 
													@AMOUNT, 
													@TRANSACTIONTYPE, 
													@BANK, 
													@TENDER, 
													@STORE, 
													@TRANSDATE,
													@TRANSACTIONSTATUS,
                                                    @ADMFEE,
                                                    @FEE,
													@DATAAREAID,
													@PARTITION
													)";

						using (SqlCommand command = new SqlCommand(queryString, connection))
						{
							command.Parameters.AddWithValue("@TRANSACTIONID", transactionIdECR);
							command.Parameters.AddWithValue("@RECEIPTID", receiptID);
							command.Parameters.AddWithValue("@TRXIDEDC", trxIdECR);
							command.Parameters.AddWithValue("@CARDNUMBER", cardECR);
							command.Parameters.AddWithValue("@AMOUNT", (Math.Truncate(Convert.ToDecimal(amountECR) * 1000m) / 1000m)); //convert amount ,0000
							command.Parameters.AddWithValue("@TRANSACTIONTYPE", transactionTypeECR);
							command.Parameters.AddWithValue("@BANK", bankECR);
							command.Parameters.AddWithValue("@TENDER", tenderECR);
							command.Parameters.AddWithValue("@Store", storeECR);
							command.Parameters.AddWithValue("@TRANSDATE", Convert.ToDateTime(transDateECR).ToString("yyyy-MM-dd")); //convertdate YYYY-MM-DD 
							command.Parameters.AddWithValue("@TRANSACTIONSTATUS", transactionStatusECR);
                            command.Parameters.AddWithValue("@ADMFEE", admFeeECR);
                            command.Parameters.AddWithValue("@FEE", admFeeECR);
							command.Parameters.AddWithValue("@DATAAREAID", dataAreaIdECR);
							command.Parameters.AddWithValue("@PARTITION", partitionECR);
                            //command.Parameters.AddWithValue("@PARTITION", partitionECR);

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
						throw;
					}
					finally
					{
						try
						{
							string queryStringDelete = @"DELETE FROM AX.[CPAMBILTUNAITEMP]";

							SqlCommand commandDelete = new SqlCommand(queryStringDelete, connection);

							if (connection.State != ConnectionState.Open)
							{
								connection.Open();
							}

							commandDelete.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
							throw;
						}
						finally
						{
							if (connection.State != ConnectionState.Closed)
							{
								connection.Close();
							}
						}
					}
				}
				   
				/*try
				{
					string queryString = @"UPDATE ax.CPAMBILTUNAI
											SET RECEIPTID = @ReceiptID
											WHERE TRANSACTIONID = @TransactionID AND DATAAREAID = @DataAreaID";

					using (SqlCommand command = new SqlCommand(queryString, connection))
					{
						command.Parameters.AddWithValue("@ReceiptID", receiptID);
						command.Parameters.AddWithValue("@TransDate", transDate);
						command.Parameters.AddWithValue("@STORE", store);
						command.Parameters.AddWithValue("@TransactionID", transactionID);
						command.Parameters.AddWithValue("@DataAreaID", dataareaID);

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
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}*/
			}
			#endregion

            if (id != 0)
            {
                try
                {
                    //add type, number, gender, country - yonathan 17012025
                    string queryString = @"
											INSERT INTO AX.CPNEWPAYMENTPOS (
																			ID,
																			TransactionID, 
																			ReceiptID, 
																			PaymentAmount, 
																			NoReffTransaction, 
																			TenderID, 
																			TenderName, 
																			PurchaserName, 
																			PurchaserPhone, 
																			Store, 
																			TransDate,                                                                             
																			[DATAAREAID],
																			[PARTITION]
																			)
											VALUES (
													@ID,
													@TransactionID,
													@ReceiptID, 
													@PaymentAmount, 
													@NoReffTransaction, 
													@TenderID, 
													@TenderName, 
													@PurchaserName, 
													@PurchaserPhone, 
													@Store, 
													@TransDate,                                                    
													@DataAreaID,
													@Partition
													)";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@TransactionID", transactionID); 
                        command.Parameters.AddWithValue("@ReceiptID", receiptID);
                        command.Parameters.AddWithValue("@PaymentAmount", PaymentAmount);
                        command.Parameters.AddWithValue("@NoReffTransaction", NoReffTransaction);
                        command.Parameters.AddWithValue("@TenderID", TenderID);
                        command.Parameters.AddWithValue("@TenderName", TenderName);
                        command.Parameters.AddWithValue("@PurchaserName", PurchaserName);
                        command.Parameters.AddWithValue("@PurchaserPhone", PurchaserPhone);
                        command.Parameters.AddWithValue("@Store", store);
                        command.Parameters.AddWithValue("@TransDate", transDate);
                        //CPPOSPAYMENT 
                        //command.Parameters.AddWithValue("@TYPEID", APIAccess.APIAccessClass.idTypeBox);
                        //command.Parameters.AddWithValue("@NUMBERID", APIAccess.APIAccessClass.idText.ToString());
                        //command.Parameters.AddWithValue("@GENDER", APIAccess.APIAccessClass.genderBox);
                        //command.Parameters.AddWithValue("@COUNTRY", APIAccess.APIAccessClass.nationality.ToString());
                        //command.Parameters.AddWithValue("@AGE", APIAccess.APIAccessClass.ageText);
                        command.Parameters.AddWithValue("@DataAreaID", dataareaID);
                        command.Parameters.AddWithValue("@Partition", partition);

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
                    throw;
                }
                finally
                {
                    try
                    {
                        string queryStringDelete = @"DELETE FROM AX.[CPNEWPAYMENTPOSTMP]";

                        SqlCommand commandDelete = new SqlCommand(queryStringDelete, connection);

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        commandDelete.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        throw;
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }


            //add write to field DPPNLNONPPN  & DPPNLDIBEBASKAN & DPPNILAILAIN 
            writeDPPTransExtended(posTransaction);
            //disable CPPOSPAYMENT
//            else //if cash payment method
//            {


//                SqlConnection connectionID = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
//                try
//                {
//                    string queryStringID = @"
//											SELECT TOP 1 REPLICATIONCOUNTERFROMORIGIN FROM AX.[CPNEWPAYMENTPOS] ORDER BY REPLICATIONCOUNTERFROMORIGIN DESC";

                     

//                    using (SqlCommand command = new SqlCommand(queryStringID, connectionID))
//                    {

//                        if (connectionID.State != ConnectionState.Open)
//                        {
//                            connectionID.Open();
//                        }
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            if (reader.Read())
//                            {
//                                id = (int)reader[0];
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
//                    throw;
//                }
//                finally
//                {
//                    if (connectionID.State != ConnectionState.Closed)
//                    {
//                        connectionID.Close();
//                    }
//                }

//                // input the insert query here
                
//                RetailTransaction retailTrans = posTransaction as RetailTransaction;
//                transactionID = posTransaction.TransactionId;
//                receiptID = posTransaction.ReceiptId;
//                //PaymentAmount = retailTrans.Payment.ToString() ;
//                NoReffTransaction = "";
//                TenderID = 1;
//                TenderName =    "CASH";
//                partition = "1";
                 
//                //PurchaserName = reader[7] + "";
//                PurchaserPhone = "";
//                store = posTransaction.StoreId;
                
//                try
//                {
//                    //add type, number, gender, country - yonathan 17012025
//                    string queryString = @"
//											INSERT INTO AX.CPNEWPAYMENTPOS (
//																			ID,
//																			TransactionID, 
//																			ReceiptID, 
//																			PaymentAmount, 
//																			NoReffTransaction, 
//																			TenderID, 
//																			TenderName, 
//																			PurchaserName, 
//																			PurchaserPhone, 
//																			Store, 
//																			TransDate, 
//                                                                            TypeId,
//                                                                            NumberId,
//                                                                            Gender,
//                                                                            Country,
//                                                                            Age,
//																			[DATAAREAID],
//																			[PARTITION]
//																			)
//											VALUES (
//													@ID,
//													@TransactionID,
//													@ReceiptID, 
//													@PaymentAmount, 
//													@NoReffTransaction, 
//													@TenderID, 
//													@TenderName, 
//													@PurchaserName, 
//													@PurchaserPhone, 
//													@Store, 
//													@TransDate,
//                                                    @TYPEID,
//                                                    @NUMBERID,
//                                                    @GENDER,
//                                                    @COUNTRY,
//                                                    @AGE,
//													@DataAreaID,
//													@Partition
//													)";

//                    using (SqlCommand command = new SqlCommand(queryString, connection))
//                    {
//                        command.Parameters.AddWithValue("@ID", ++id);
//                        command.Parameters.AddWithValue("@TransactionID", transactionID);
//                        command.Parameters.AddWithValue("@ReceiptID", receiptID);
//                        command.Parameters.AddWithValue("@PaymentAmount", retailTrans.Payment);
//                        command.Parameters.AddWithValue("@NoReffTransaction", NoReffTransaction);
//                        command.Parameters.AddWithValue("@TenderID", TenderID);
//                        command.Parameters.AddWithValue("@TenderName", TenderName);
//                        command.Parameters.AddWithValue("@PurchaserName", APIAccess.APIAccessClass.custText.ToString());
//                        command.Parameters.AddWithValue("@PurchaserPhone", PurchaserPhone);
//                        command.Parameters.AddWithValue("@Store", store);
//                        command.Parameters.AddWithValue("@TransDate", DateTime.Now);
//                        command.Parameters.AddWithValue("@TYPEID", APIAccess.APIAccessClass.idTypeBox);
//                        command.Parameters.AddWithValue("@NUMBERID", APIAccess.APIAccessClass.idText.ToString());
//                        command.Parameters.AddWithValue("@GENDER", APIAccess.APIAccessClass.genderBox);
//                        command.Parameters.AddWithValue("@COUNTRY", APIAccess.APIAccessClass.nationality.ToString());
//                        command.Parameters.AddWithValue("@AGE", APIAccess.APIAccessClass.ageText);
//                        command.Parameters.AddWithValue("@DataAreaID", Application.Settings.Database.DataAreaID);
//                        command.Parameters.AddWithValue("@Partition", partition);

//                        if (connection.State != ConnectionState.Open)
//                        {
//                            connection.Open();
//                        }

//                        command.ExecuteNonQuery();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
//                    throw;
//                }
//                finally
//                {
//                    try
//                    {
//                        string queryStringDelete = @"DELETE FROM AX.[CPNEWPAYMENTPOSTMP]";

//                        SqlCommand commandDelete = new SqlCommand(queryStringDelete, connection);

//                        if (connection.State != ConnectionState.Open)
//                        {
//                            connection.Open();
//                        }

//                        commandDelete.ExecuteNonQuery();
//                    }
//                    catch (Exception ex)
//                    {
//                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
//                        throw;
//                    }
//                    finally
//                    {
//                        if (connection.State != ConnectionState.Closed)
//                        {
//                            connection.Close();
//                        }
//                    }
//                }

//                //++id
//            }
		}

        private void writeDPPTransExtended(IPosTransaction posTransaction)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            decimal dppNilaiLain = getDPPLain(posTransaction);  
            string receiptId = posTransaction.ReceiptId;
            string store = posTransaction.StoreId;
            string terminal = posTransaction.TerminalId;
            string transactionId = posTransaction.TransactionId;
            DateTime transDate = DateTime.Now.Date;   
            decimal dppNLDibebaskan = getDPPBebas(posTransaction);//500.00m;   
            decimal dppNLNonPpn = getDPPNLNon(posTransaction);  
            string dataAreaId = Application.Settings.Database.DataAreaID;
            long partition = 1; 

            try
            {
                string queryString = @" INSERT INTO 
                        ax.CPRETAILTRANSACTIONTABLEEXT (
                            DPPNILAILAIN,
                            RECEIPTID,
                            STORE,
                            TERMINAL,
                            TRANSACTIONID,
                            TRANSDATE,
                            DPPNLDIBEBASKAN,
                            DPPNLNONPPN,
                            [DATAAREAID],
                            [PARTITION])

                        VALUES (
                            @DPPNILAILAIN,
                            @RECEIPTID,
                            @STORE,
                            @TERMINAL,
                            @TRANSACTIONID,
                            @TRANSDATE,
                            @DPPNLDIBEBASKAN,
                            @DPPNLNONPPN,
                            @DATAAREAID,
                            @PARTITION
                        )";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@DPPNILAILAIN", dppNilaiLain);
                    command.Parameters.AddWithValue("@RECEIPTID", receiptId);
                    command.Parameters.AddWithValue("@STORE", store);
                    command.Parameters.AddWithValue("@TERMINAL", terminal);
                    command.Parameters.AddWithValue("@TRANSACTIONID", transactionId);
                    command.Parameters.AddWithValue("@TRANSDATE", transDate);
                    command.Parameters.AddWithValue("@DPPNLDIBEBASKAN", dppNLDibebaskan);
                    command.Parameters.AddWithValue("@DPPNLNONPPN", dppNLNonPpn);
                    command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);
                    command.Parameters.AddWithValue("@PARTITION", partition);

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
                throw;
            }
            finally
            {                   
                if (connection.State != ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            
        }

        private decimal getDPPNLNon(IPosTransaction posTransaction)
        {
            RetailTransaction theTransaction = posTransaction as RetailTransaction;
           
            CustomerOrderTransaction cot;
            decimal subtotal = 0;

            try
            {

                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                    if (s.TaxGroupId == "NonPPN" && !s.Voided)
                        subtotal += s.NetAmountWithNoTax;

                }


                return subtotal * 11 / 12; 
            }
            catch
            {
                 
                return 0;
                
            }
        }

        private decimal getDPPLain(IPosTransaction posTransaction)
        {
             
            RetailTransaction theTransaction = posTransaction as RetailTransaction;
            
            CustomerOrderTransaction cot;
            decimal subtotal = 0;

            if (ApplicationSettings.Terminal.StoreTaxGroup == "TAX")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        if (s.TaxGroupId == "PPN" && !s.Voided)
                            subtotal += s.NetAmountWithNoTax;

                         
                    }
                    subtotal = subtotal * 11 / 12;
                    
                }
                catch
                { 
                    subtotal = 0;
                   
                }

            }
            else if (ApplicationSettings.Terminal.StoreTaxGroup == "NonPKP")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        if (s.TaxGroupId == "PPN" && !s.Voided)
                            subtotal += s.NetAmount;
 
                    }
                    subtotal = subtotal * 11 / 12;
                }
                catch
                { 
                    subtotal = 0; 
                }
            }
            return subtotal;
        }

        private decimal getDPPBebas(IPosTransaction posTransaction)
        {
            RetailTransaction theTransaction = posTransaction as RetailTransaction;
            
            CustomerOrderTransaction cot;
            decimal subtotal = 0;

            try
            {

                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                    if (s.TaxGroupId == "Dibebaskan" && !s.Voided)
                        subtotal += s.NetAmountWithNoTax;
                     
                }


                return subtotal = subtotal * 11 / 12;
                
            }
            catch
            {
                 
                return 0;
                
            }
        
        }

		private void CPTOPUPBNI(IPosTransaction posTransaction)
		{
			mySerialPort.Close();
			mySerialPort = new SerialPort(comPort);

			mySerialPort.BaudRate = 9600;
			mySerialPort.Parity = Parity.None;
			mySerialPort.StopBits = StopBits.One;
			mySerialPort.DataBits = 8;
			mySerialPort.Handshake = Handshake.None;
			mySerialPort.RtsEnable = true;

			mySerialPort.ReadTimeout = 50000;
			mySerialPort.WriteTimeout = 50000;

			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			

			if (posTransaction.ReceiptId != "")
			{
				string listItem = "";
				int totalItem = 0;
				listItem += "(";

				for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
				{
					if (totalItem != 0 && !((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Voided && ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId != "")
						listItem += ", ";

					if (!((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Voided && ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId != "")
					{
						listItem += "'" + ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId + "'";
						dict[((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId] = (string)((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Description;
						totalItem++;
					}
				}

				listItem += ")";

				if (totalItem != 0)
				{
					try
					{

						string queryString = @" SELECT CAST(AMOUNT AS INT) AS AMOUNT, ITEMID FROM [AX].[CPITEMTOPUP]
											WHERE ITEMID IN " + listItem + @"
										";

						using (SqlCommand command = new SqlCommand(queryString, connection))
						{
							if (connection.State != ConnectionState.Open)
							{
								connection.Open();
							}
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									try
									{
										mySerialPort.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);

										mySerialPort.Open();
									}
									// This is a better way to handle the exceptions.  
									// You don't need to set isOpen[i] = false, since it defaults to that value  
									catch (IOException ex)
									{
										Console.WriteLine(ex);
									}

									try
									{
										bytestosend = new byte[4];
										bytestosend[0] = 0x02;
										bytestosend[1] = 0x42;
										bytestosend[2] = 0x4E;
										bytestosend[3] = 0x49;

										//Transaction Type
										Array.Resize(ref bytestosend, bytestosend.Length + 1);
										bytestosend[bytestosend.GetUpperBound(0)] = 0x05;

										//Amount
										//0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x30,
										int amount = ((int)reader[0]) * 100;
										string paddingValue = (amount + "").PadLeft(12, '0');
										string hexString = ConvertStringToHex(paddingValue);
										byte[] valueByte = StringToByteArray(hexString);

										for (int i = 0; i < valueByte.Length; i++)
										{
											Array.Resize(ref bytestosend, bytestosend.Length + 1);
											bytestosend[bytestosend.GetUpperBound(0)] = valueByte[i];
										}

										//Invoice Number
										int invoiceNumber = 2;
										paddingValue = (invoiceNumber + "").PadLeft(6, '0');
										hexString = ConvertStringToHex(paddingValue);
										Array.Clear(valueByte, 0, valueByte.Length);
										valueByte = StringToByteArray(hexString);

										for (int i = 0; i < valueByte.Length; i++)
										{
											Array.Resize(ref bytestosend, bytestosend.Length + 1);
											bytestosend[bytestosend.GetUpperBound(0)] = valueByte[i];
										}

										//Billing Number
										int billingNumber = 3;
										paddingValue = (billingNumber + "").PadLeft(16, '0');
										hexString = ConvertStringToHex(paddingValue);
										Array.Clear(valueByte, 0, valueByte.Length);
										valueByte = StringToByteArray(hexString);

										for (int i = 0; i < valueByte.Length; i++)
										{
											Array.Resize(ref bytestosend, bytestosend.Length + 1);
											bytestosend[bytestosend.GetUpperBound(0)] = valueByte[i];
										}

										//Account Type
										Array.Resize(ref bytestosend, bytestosend.Length + 1);
										bytestosend[bytestosend.GetUpperBound(0)] = 0x30;

										//Bank Filler
										for (int i = 0; i < 164; i++)
										{
											Array.Resize(ref bytestosend, bytestosend.Length + 1);
											bytestosend[bytestosend.GetUpperBound(0)] = 0x20;
										}

										//ETX
										Array.Resize(ref bytestosend, bytestosend.Length + 1);
										bytestosend[bytestosend.GetUpperBound(0)] = 0x03;

										//LRC
										int lrcValue = bytestosend[1] ^ bytestosend[2];

										for (int i = 3; i < bytestosend.Length; i++)
										{
											lrcValue ^= bytestosend[i];
										}

										//hexString = ConvertStringToHex(paddingValue);
										int tmp = lrcValue;
										hexString = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
										Array.Clear(valueByte, 0, valueByte.Length);
										valueByte = StringToByteArray(hexString);

										for (int i = 0; i < valueByte.Length; i++)
										{
											Array.Resize(ref bytestosend, bytestosend.Length + 1);
											bytestosend[bytestosend.GetUpperBound(0)] = valueByte[i];
										}

										countRetry = 0;

										topupName = dict[reader[1].ToString()];
										mySerialPort.Write(bytestosend, 0, bytestosend.Length);

									}
									// This is a better way to handle the exceptions.  
									// You don't need to set isOpen[i] = false, since it defaults to that value  
									catch (IOException ex)
									{
										Console.WriteLine(ex);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
						throw;
					}
					finally
					{
						if (connection.State != ConnectionState.Closed)
						{
							connection.Close();
						}
					}
				}
			}
		}


		public string getFolderPath(string ProcessingDirectory, string typeFolder) //custom by YNWA
		{
			string Folder = "";

			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(ProcessingDirectory);
			XmlNode xnodes = xdoc.SelectSingleNode("configuration");
			XmlNodeList xmlList = xnodes.SelectNodes("folderpath");

			foreach (XmlNode xmlNodeS in xmlList)
			{
				Folder += "," + xmlNodeS.Attributes.GetNamedItem(typeFolder).Value;
			}
			return Folder.Substring(1);
		}

		public void AddItemTransaction(IPosTransaction posTransaction) //custom by yonathan
		{
			RetailTransaction retailTransaction = posTransaction as RetailTransaction;
			//string itemId = "";
			//string qtyItem = "";
			//string unitId = "";
			//string dataAreaId = "";
			string warehouseId = "";
			//string transType = "";
			//string refNumber = "";
			string siteId = "";
			string url = "";// "http://10.204.3.174:80/api/stockOnHand/addItemMultiple";
			//string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");

			//url = getFolderPath(PathDirectory, "urlAPI") + "api/stockOnHand/addItemMultiple";
			string functionName = "AddItemMultipleAPI";
			APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
			url = APIClass.getURLAPIByFuncName(functionName);
			if (url == "")
			{
				throw new Exception(string.Format("Function not found : {0},\nPlease contact your IT Admin", functionName));
			}

			string result = "";


			if (posTransaction.ReceiptId != "")
			{
				SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{

					string queryString = @" SELECT A.INVENTLOCATION, A.INVENTLOCATIONDATAAREAID, C.INVENTSITEID 
							FROM ax.RETAILCHANNELTABLE A, ax.RETAILSTORETABLE B, ax.INVENTLOCATION C
							WHERE A.RECID=B.RECID AND C.INVENTLOCATIONID=A.INVENTLOCATION AND B.STORENUMBER=@STOREID";

					using (SqlCommand command = new SqlCommand(queryString, connection))
					{
						command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);

						if (connection.State != ConnectionState.Open)
						{
							connection.Open();

						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								siteId = reader["INVENTSITEID"].ToString();
								warehouseId = reader["INVENTLOCATION"].ToString();
							}

						}
					}


				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

				HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
				httpRequest.Method = "POST";
				httpRequest.ContentType = "application/json";
				httpRequest.Headers.Add("Authorization", "PFM");
				List<string> multiPack = new List<string>();
				var packList = new List<parmRequest>();


				if (retailTransaction.CalculableSalesLines.Count != 0)
				{
					foreach (var lines in retailTransaction.CalculableSalesLines)
					{
						//parmRequest pack = new parmRequest()
						var pack = new parmRequest
						{
							ITEMID = lines.ItemId,
							QTY = (lines.PriceQty * -1).ToString().Replace(",","."), //lines.QuantityOrdered.ToString(),
							UNITID = lines.SalesOrderUnitOfMeasure,
							DATAAREAID = Application.Settings.Database.DataAreaID,
							WAREHOUSE = warehouseId,
							TYPE = "2",
							REFERENCESNUMBER = retailTransaction.ReceiptId,
							RETAILVARIANTID = lines.Dimension.VariantId
						};
						//var data = MyJsonConverter.Serialize(pack);
						packList.Add(pack);
					}
					var multiData = MyJsonConverter.Serialize(packList);
					/*                 
					var resList = new List<res>();

					foreach (var reservation in _context.ReservationModel)
					{
						var start = reservation.StartOfReservation.ToString("yyyy.MM.dd");
						var end = reservation.EndOfReservation.ToString("yyyy.MM.dd");


						var res = new Reservation
						{
							start = start,
							end = end,
						};

						resList.Add(res);

					}
					var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(resList);
					return jsonString;
					 */
					using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
					{
						streamWriter.Write(multiData);
						streamWriter.Flush();
					}

					try
					{
						HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
						using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
						{
							result = streamReader.ReadToEnd();
						}
					}
					catch (Exception ex)
					{
						LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
						throw;
					}

					//DialogResult res = MessageBox.Show(result.ToString(), "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

				}
			}           
		}

		/*
		private void CPAPIEZEELINK(IPosTransaction posTransaction)
		{
			SqlConnection connectionEZ = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			//UPDATE CPEZREDEEM TABLE
			if (posTransaction.ReceiptId != "")
			{
				try
				{
					string queryString = @"UPDATE 
												AX.CPEZEARN 
											SET 
												RECEIPTID = @RECEIPTID
											WHERE 
												TRANSACTIONID = @TRANSACTIONID";

					using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
					{
						cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
						cmd.Parameters.AddWithValue("@RECEIPTID", posTransaction.ReceiptId);

						if (connectionEZ.State != ConnectionState.Open)
						{
							connectionEZ.Open();
						}

						cmd.ExecuteNonQuery();
					}
				}
				catch (SqlException ex)
				{
					throw new Exception("Format Error", ex);
				}
				finally
				{
					connectionEZ.Close();
				}

				try
				{
					string queryString = @"UPDATE 
												AX.CPEZREDEEM 
											SET 
												RECEIPTID = @RECEIPTID
											WHERE 
												TRANSACTIONID = @TRANSACTIONID";

					using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
					{
						cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
						cmd.Parameters.AddWithValue("@RECEIPTID", posTransaction.ReceiptId);

						if (connectionEZ.State != ConnectionState.Open)
						{
							connectionEZ.Open();
						}

						cmd.ExecuteNonQuery();
					}
				}
				catch (SqlException ex)
				{
					throw new Exception("Format Error", ex);
				}
				finally
				{
					connectionEZ.Close();
				}

				try
				{
					string queryString = @"DELETE FROM CPEZSELECTEDCUSTOMER";

					using (SqlCommand cmd = new SqlCommand(queryString, connectionEZ))
					{
						if (connectionEZ.State != ConnectionState.Open)
						{
							connectionEZ.Open();
						}

						cmd.ExecuteNonQuery();
					}
				}
				catch (SqlException ex)
				{
					throw new Exception("Format Error", ex);
				}
				finally
				{
					connectionEZ.Close();
				}
			}
		}
		 * */
/*
		private lineItem setlineItem(int index, string productCode, string familyCode, int quantity, long productPrice)
		{
			lineItem lineItem = new lineItem();

			lineItem.index = index;
			lineItem.productCode = productCode;
			lineItem.familyCode = familyCode;
			lineItem.quantity = quantity;
			lineItem.productPrice = productPrice;
			lineItem.nonDiscountable = false;
			lineItem.rewardsExcluded = false;

			return lineItem;
		}

		private lineItems setLineItems(List<lineItem> listLineItem)
		{
			lineItems lineItems = new lineItems();
			lineItems.lineItem = new lineItem[listLineItem.Count];

			for (int i = 0; i < listLineItem.Count; i++)
			{
				lineItems.lineItem[i] = listLineItem[i];
			}

			return lineItems;
		}*/


		private void CPVOUCHERCODE(IPosTransaction posTransaction)
		{
			string journalID = "";
			string dataAreaID = "";
			string voucherCode = "";

			// FETCH DATA FOR USE
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

			try
			{
				string querySelect = @"SELECT TOP 1 PRICEDISCJOURNALID, DATAAREAID, VOUCHERCODE FROM CPEXTVOUCHERCODETMP";

				using (SqlCommand cmd = new SqlCommand(querySelect, connection))
				{
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							journalID = reader["PRICEDISCJOURNALID"].ToString();
							dataAreaID = reader["DATAAREAID"].ToString();
							voucherCode = reader["VOUCHERCODE"].ToString();

							reader.Close();
							reader.Dispose();
							cmd.Dispose();

							string queryDelete = "DELETE FROM CPEXTVOUCHERCODETMP";

							using (SqlCommand cmdDelete = new SqlCommand(queryDelete, connection))
							{
								if (connection.State != ConnectionState.Open)
								{
									connection.Open();
								}

								cmdDelete.ExecuteNonQuery();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
			finally
			{
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}
			
			if(journalID != "")
			{
				// INSERT INTO LOCAL DB FOR BACKUP IF THERE IS NO CONNECTION
				try
				{
					string queryString = @"INSERT INTO [AX].[CPRETAILTRANSACTIONVOUCHER]
										(
											VOUCHERCODE,
											TRANSACTIONID,
											RECEIPTID,
											STORE,
											DATAAREAID,
											[PARTITION]
										)
										VALUES
										(
											@VOUCHERCODE,
											@TRANSACTIONID,
											@RECEIPTID,
											@STOREID,
											@DATAAREAID,
											@PARTITION
										)
										";

					using (SqlCommand cmd = new SqlCommand(queryString, connection))
					{
						cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
						cmd.Parameters.AddWithValue("@RECEIPTID", posTransaction.ReceiptId);
						cmd.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
						cmd.Parameters.AddWithValue("@VOUCHERCODE", voucherCode);
						cmd.Parameters.AddWithValue("@DATAAREAID", dataAreaID);
						cmd.Parameters.AddWithValue("@PARTITION", 1);

						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						cmd.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}

                //PROCEED UPDATE SERVER DB --NEW via RTS-- yonathan 08/01/2024
                ReadOnlyCollection<object> containerArray = new ReadOnlyCollection<object>(new object[0]);

                try
                {
                    object[] parameterList = new object[] 
							{
								posTransaction.StoreId,
								voucherCode,
								posTransaction.TransactionId								
							};


                    containerArray = Application.TransactionServices.InvokeExtension("updateStatusVoucher", parameterList);
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    throw;
                }

				//PROCEED UPDATE SERVER DB --OLD--
				/*
                string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;

				connection = new SqlConnection(connectionString);
				try
				{

					string queryString = @"UPDATE CPEXTVOUCHERCODETABLE
										SET REDEEMSTATUS = 1, TRANSSTORE = @STOREID, CREATEDTRANSID = @TRANSACTIONID
										WHERE VOUCHERCODE = @VOUCHERCODE
										";

					using (SqlCommand cmd = new SqlCommand(queryString, connection))
					{
						cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
						cmd.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
						cmd.Parameters.AddWithValue("@VOUCHERCODE", voucherCode);

						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						cmd.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}*/
			}            
		}

		#endregion
	}
}
