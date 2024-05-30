using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using LSRetailPosis.Transaction;
using LSRetailPosis.POSControls;
using System.ComponentModel.Composition;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
	public partial class CPWeightBarcode : Form
	{
		

		IPosTransaction posTransaction;
		char cforKeyDown = '\0';
		int _lastKeystroke = DateTime.Now.Millisecond;
		List<char> _barcode = new List<char>(1);
		bool UseKeyboard = false;
		string skuId = "";

		public CPWeightBarcode(IPosTransaction _posTransaction)
		{
			InitializeComponent();
			posTransaction = _posTransaction;
			txtQty.Text = "";
			txtSKU.Text = "";
			skuId = "";
			txtScanBarcode.Text = "";
			lblSKUName.Text = "";
			btnOk.Text = "OK";
			lblScanBarcode.Text = "Scan SKU ID Barcode";
			txtScanBarcode.Enabled = true;
			btnReset.Enabled = true;
			btnOk.Enabled = false;
			btnCancel.Enabled = true;

			_barcode = new List<char>(1);
			_lastKeystroke = DateTime.Now.Millisecond;
			cforKeyDown = '\0';
			txtScanBarcode.Focus();
		}

		private void validateBarcode()
		{
			//determine how many digit to be cut from last
			int checkDigit = 1;

			//set minimum barcode length
			if (txtScanBarcode.Text.Length < 6)
			{
				callMessage("Barcode length too short");
				return;
			}

			string scanValue = txtScanBarcode.Text.Remove(txtScanBarcode.Text.Length - checkDigit);

			if (txtSKU.Text == "")
			{
				//Search local DB for product name
				SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

				try
				{
					string queryString = @"SELECT 
												COALESCE(b.NAME, a.SEARCHNAME) AS PRODUCT_NAME
												, d.ITEMID AS ITEMID
												, d.UNITID AS UNITID
												, d.INVENTDIMID AS INVENTDIMID
											FROM
												ECORESPRODUCT a
												LEFT JOIN ECORESPRODUCTTRANSLATION b ON a.RECID = b.PRODUCT
												INNER JOIN INVENTITEMBARCODE d ON a.DISPLAYPRODUCTNUMBER = d.ITEMID
											WHERE 
												d.ITEMBARCODE = @BARCODE";

					using (SqlCommand cmd = new SqlCommand(queryString, connectionLocal))
					{
						cmd.Parameters.AddWithValue("@BARCODE", scanValue);
						//cmd.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

						if (connectionLocal.State != ConnectionState.Open)
						{
							connectionLocal.Open();
						}

						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								skuId = reader["ITEMID"] + "";
								string unitId = reader["UNITID"] + "";
								string inventDimId = reader["INVENTDIMID"] + "";
								IApplication application = PosApplication.Instance as IApplication;
								decimal checkPrice = application.Services.Price.GetItemPrice(skuId, inventDimId, unitId);

								if (checkPrice <= 0)
								{
									callMessage("Zero price is not allowed");
									return;
								}

								//decimal defaultPrice = this.Application.Services.Price.GetItemPrice(saleLineItem.ItemId, saleLineItem.SalesOrderUnitOfMeasure);
								txtSKU.Text = scanValue;
								lblSKUName.Text = "SKU ID: " + skuId + "\n" + reader["PRODUCT_NAME"] + "";

								lblScanBarcode.Text = "Scan Weight Barcode";
							}
							else
							{
								callMessage("Product Not Found");
								return;
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
					if (connectionLocal.State != ConnectionState.Closed)
					{
						connectionLocal.Close();
					}
				}
			}
			else if (txtQty.Text == "")
			{
				Decimal qty = 0;
				//Decimal fullQty = 0;
				try
				{
					qty = Decimal.Parse(scanValue);
					//fullQty = Decimal.Parse(numPad1.EnteredValue);
				}
				catch
				{
					callMessage("Barcode must be numeric");
					return;
				}

				if(qty < 0)
				{
					callMessage("Negative Qty is not allowed");
					return;
				}

				/*if (fullQty % 1 == 0)
				{
					qty = qty / 1000;
				}
				else
				{
					qty = fullQty;
				}*/

				qty = qty / 1000;
				txtQty.Text = qty + "";

				btnOk.Enabled = true;
				lblScanBarcode.Text = "Click OK to Confirm or Reset to retry scan Barcode";
				txtScanBarcode.Enabled = false;
			}
			else
			{
				callMessage("Press Reset before Scan New Barcode");
				return;
			}

			txtScanBarcode.Text = "";
			txtScanBarcode.Focus();

			_barcode = new List<char>(1);
			_lastKeystroke = DateTime.Now.Millisecond;
			cforKeyDown = '\0';
			btnReset.Enabled = true;
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			txtQty.Text = "";
			txtSKU.Text = "";
			skuId = "";
			lblSKUName.Text = "";
			btnReset.Enabled = true;
			btnOk.Enabled = false;
			lblScanBarcode.Text = "Scan SKU ID Barcode";
			txtScanBarcode.Enabled = true;
			txtScanBarcode.Text = "";
			txtScanBarcode.Focus();

			_barcode = new List<char>(1);
			_lastKeystroke = DateTime.Now.Millisecond;
			cforKeyDown = '\0';
		}

		private void callMessage(string message)
		{
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
			}
			txtScanBarcode.Text = "";
			txtScanBarcode.Focus();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			decimal qty;
			if (txtQty.Text == "")
			{
				callMessage("Please scan Weight Barcode");
			}
			else if (txtSKU.Text == "")
			{
				callMessage("Please scan SKU Barcode");
			}
			else if (skuId == "")
			{
				callMessage("Please scan SKU Barcode");
			}
			else if (!Decimal.TryParse(txtQty.Text, out qty))
			{
				callMessage("Weight must be decimal");
				return;
			}
			else if (lblSKUName.Text == "")
			{
				callMessage("Product not found");
			}
			else
			{
				//add item to transaction here
				bool itemExist = false;

				try
				{
					btnOk.Text = "Loading...";
					btnOk.Enabled = false;
					//btnCancel.Enabled = false;
					btnReset.Enabled = false;

					//validate not allowing same item added twice
					for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
					{
						if (((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId == skuId)
						{
							IApplication application = PosApplication.Instance as IApplication;
							/*callMessage("Item already exist");
							btnOk.Text = "OK";
							btnOk.Enabled = true;
							btnCancel.Enabled = true;
							btnReset.Enabled = true;
							return;*/
							//LSRetailPosis.POSProcesses.WinControls.NumPad numpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
							//numpad.NumberOfDecimals = 3;
							
							
							//LSRetailPosis.POSProcesses.ItemSale uSale = ;
							RetailTransaction retailTransaction = posTransaction as RetailTransaction;
							//int iLastLine = Decimal.ToInt32(retailTransaction.NoOfItemLines);
							LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem sl = retailTransaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);

							try
							{
								PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
								PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PreSetQty(preTriggerResult,sl, posTransaction, i)));


							}
							catch (Exception ex)
							{
								LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
							}

							//sl.Quantity += qty;
							sl.QuantityOrdered = qty;
							

							try
							{
								PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
								PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PostSetQty(posTransaction, sl)));
								sl.Quantity += sl.QuantityOrdered;

							}
							catch (Exception ex)
							{
								LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
							}

							
							//sl.CalculateLine();
							
							retailTransaction.CalcTotals();

							




							//application.RunOperation(PosisOperations.SetQty, , posTransaction);
							//retailTransaction.Add(sl);
							//application.RunOperation(PosisOperations.ItemSale, skuId);//application.RunOperation(PosisOperations.ItemSale, skuId);
							//Application.RunOperation(PosisOperations.SetQty, skuId, posTransaction);
							itemExist = true;
						}
					}

					if(!itemExist)
					{
						LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
						iSale.OperationID = PosisOperations.ItemSale;
						iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
						//iSale.Barcode = skuId; disable by Yonathan 21/10/2022
						iSale.Barcode = txtSKU.Text;  // change to this by yonathan 21/10/2022
						//iSale.BarcodeInfo.ItemId = txtSKU.Text;
						iSale.OperationInfo.NumpadQuantity = qty;
						iSale.POSTransaction = (PosTransaction)posTransaction;

						iSale.RunOperation();
					}
					
				}
				catch (Exception ex)
				{
					btnOk.Text = "OK";
					btnOk.Enabled = true;
					btnCancel.Enabled = true;
					btnReset.Enabled = true;
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}

				this.Close();
			}
		}

		private void CPWeightBarcode_Load(object sender, EventArgs e)
		{
			this.KeyDown += new KeyEventHandler(txtScanBarcode_KeyDown);
			this.KeyUp += new KeyEventHandler(txtScanBarcode_KeyUp);
		}

		private void txtScanBarcode_KeyUp(object sender, KeyEventArgs e)
		{
			/* check if keydown and keyup is not different
			 * and keydown event is not fired again before the keyup event fired for the same key
			 * and keydown is not null
			 * Barcode never fired keydown event more than 1 time before the same key fired keyup event
			 * Barcode generally finishes all events (like keydown > keypress > keyup) of single key at a time, 
			 * if two different keys are pressed then it is with keyboard
			 */
			if (cforKeyDown != (char)e.KeyCode || cforKeyDown == '\0')
			{
				cforKeyDown = '\0';
				_barcode.Clear();
				return;
			}

			// getting the time difference between 2 keys
			int elapsed = (DateTime.Now.Millisecond - _lastKeystroke);

			/*
			 * Barcode scanner usually takes less than 17 milliseconds as per my Barcode reader to read , 
			 * increase this if neccessary of your barcode scanner is slower
			 * also assuming human can not type faster than 17 milliseconds
			 */
			if (elapsed > 17)
				_barcode.Clear();

			// Do not push in array if Enter/Return is pressed, since it is not any Character that need to be read
			if (e.KeyCode != Keys.Return)
			{
				_barcode.Add((char)e.KeyData);
			}

			// Barcode scanner hits Enter/Return after reading barcode
			if (e.KeyCode == Keys.Return)
			{
				if(txtScanBarcode.Text != "")
				{
					if (_barcode.Count > 0)// || UseKeyboard)
					{
						_barcode.Clear();
						validateBarcode();
					}
					else
					{
						callMessage("Please input using Barcode Scanner");
						_barcode.Clear();
					}
				}
			}

			// update the last key press strock time
			_lastKeystroke = DateTime.Now.Millisecond;
		}

		private void txtScanBarcode_KeyDown(object sender, KeyEventArgs e)
		{
			cforKeyDown = (char)e.KeyCode;
		}
	}
}
