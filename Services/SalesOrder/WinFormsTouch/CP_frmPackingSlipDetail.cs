using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSRetailPosis.POSControls.Touch;
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch;

using LSRetailPosis;

using APIAccess;
using LSRetailPosis.Settings;
using System.Globalization;
using System.Data.SqlClient;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
	public partial class CP_frmPackingSlipDetail : frmTouchBase
	{
		private static string LogSource = typeof(CP_frmPackingSlipDetail).ToString();
		public IApplication application;
		public IPosTransaction posTransaction;
		string salesID;
		CustomerOrderTransaction transaction;

		private List<LineItemViewModel> viewModels;
		private ReadOnlyCollection<LineItemViewModel> lineItems;
        //add order type - Yonathan 04102024
        int orderType = 0;
        //end
		protected void SetTransaction(CustomerOrderTransaction custTransaction)
		{
			this.transaction = custTransaction;
		}
		public CustomerOrderTransaction Transaction
		{
			get { return this.transaction; }
			set
			{
				if (this.transaction != value)
				{
					SetTransaction(value);
					//OnPropertyChanged("Transaction");
				}
			}
		}
		 
		public CP_frmPackingSlipDetail(IApplication _application, string _salesId, int _orderType = 0)
		{
			InitializeComponent();
			txtSalesOrder.Text = _salesId;
			salesID = _salesId;
			application = _application;
            orderType = _orderType;
			//posTransaction = _posTransaction;
			transaction = SalesOrderActions.GetCustomerOrder(salesID, LSRetailPosis.Transaction.CustomerOrderType.SalesOrder, LSRetailPosis.Transaction.CustomerOrderMode.Edit);
			ItemDetailsViewModel(transaction);
			loadTableData();
			//Application = _application;
		}
		private void buttio1_Click(object sender, EventArgs e)
		{
			//CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Edit);

		}
		public void ItemDetailsViewModel(CustomerOrderTransaction customerOrderTransaction)
		{

			this.SetTransaction(customerOrderTransaction);
			// Create a collection of LineItemViewModels from each SaleLineItem
			viewModels = (from lineItem in this.Transaction.SaleItems.Where(i => !i.Voided)
							select new LineItemViewModel(lineItem)).ToList<LineItemViewModel>();

			this.lineItems = new ReadOnlyCollection<LineItemViewModel>(viewModels);
		}

		//public ReadOnlyCollection<LineItemViewModel> Items
		//{
		//    get { return lineItems; }
		//}
		//private void buttin1_Click(object sender, EventArgs e)
		//{
		//    public ItemDetailsViewModel(CustomerOrderTransaction customerOrderTransaction)
		//    {
		//        this.SetTransaction(customerOrderTransaction);

		//        // Create a collection of LineItemViewModels from each SaleLineItem
		//        viewModels = (from lineItem in this.Transaction.SaleItems.Where(i => !i.Voided)
		//                      select new LineItemViewModel(lineItem)).ToList<LineItemViewModel>();

		//        this.lineItems = new ReadOnlyCollection<LineItemViewModel>(viewModels);
		//    }

		//    public ReadOnlyCollection<LineItemViewModel> Items
		//    {
		//        get { return lineItems; }
		//    }
		//}
		private void button1_Click(object sender, EventArgs e)
		{
            // Loop through each row in the dataGridView1
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Make sure the row is not the new row
                if (!row.IsNewRow)
                {
                    // Get the value from the QtySO column and assign it to QtyDO column
                    row.Cells["QtyDO"].Value = row.Cells["QtySO"].Value;
                }
            }
			
		}

		private void loadTableData()
		{
		   int numberLines = 0;
			XmlDocument doc = new XmlDocument();
			
			DataTable table = new DataTable();

			table.Columns.Add("NO.");
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("Unit");
			table.Columns.Add("QtySO");
			table.Columns.Add("QtyDO");

			
			foreach (var salesLine in this.lineItems)
			{
				
				//RetailTransaction retailTransaction = posTransaction as RetailTransaction;
				
				numberLines++;
				string itemId = salesLine.ItemId.ToString(); //salesLine.Attributes["ItemId"].Value;
				string salesQty = salesLine.QuantityOrdered.ToString();//salesLine.Attributes["SalesQty"].Value;
				string itemName = salesLine.Description.ToString();
				string unitItem = salesLine.lineItem.BackofficeSalesOrderUnitOfMeasure.ToString();
				// Add the pair of values to the DataTable
				table.Rows.Add(numberLines, itemId, itemName, unitItem, salesQty);
			}
		   

			// Bind the DataTable to the DataGridView
			dataGridView1.DataSource = table;
			changeGridStyles();

			lblSalesOrder.Text = txtSalesOrder.Text;
		}
		private void changeGridStyles()
		{
			dataGridView1.AllowUserToAddRows = false;
		   
			dataGridView1.Columns[0].Width = 55;
			dataGridView1.Columns[0].ReadOnly = true;
			dataGridView1.Columns[1].Width = 100;
			dataGridView1.Columns[1].ReadOnly = true;
			dataGridView1.Columns[2].Width = 550;
			dataGridView1.Columns[2].ReadOnly = true;
			dataGridView1.Columns[3].Width = 50;
			dataGridView1.Columns[3].ReadOnly = true;
			dataGridView1.Columns[4].Width = 100;
			dataGridView1.Columns[4].ReadOnly = true;
			dataGridView1.Columns[5].Width = 100;
			//dataGridView1.Columns[4].ReadOnly = true;
			this.dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12);
			this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			//this.dataGridView1.Columns["STOCKCOUNTINGADJUST"].HeaderText = "Stock Adjusted (+/-)";

			//dataGridView1.Columns[0].Width = 90;
		}


		//private void getSalesOrderDetails()
		//{
		//    string salesId = txtSalesOrder.Text;
		//    ReadOnlyCollection<object> containerArray = Application.TransactionServices.Invoke("getSalesOrderDetail", salesId, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId);
		//}

		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			string valueDeliverNow = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
			decimal resultValue;
			if (dataGridView1.Columns[e.ColumnIndex].Name == "QtyDO")
			{
				if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && decimal.TryParse(valueDeliverNow, out resultValue))
				{
					dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = resultValue.ToString("F4");
				}
			}
		}


		private string checkPositiveStatus(string _itemId)
		{
			string positiveStatus = "0";
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{
				//before checking the stock, check first whether this item type is service
				//string queryString = @" SELECT DISPLAYPRODUCTNUMBER,PRODUCTTYPE  FROM ax.ECORESPRODUCT where DISPLAYPRODUCTNUMBER =@ITEMID";
				string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";
				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@ITEMID", _itemId);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();

					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							positiveStatus = reader["POSITIVESTATUS"].ToString();

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

			return positiveStatus;
		}

		
		private void postBtn_Click(object sender, EventArgs e)
		{
			bool retValue;
			string comment;
            string splitInvoice;
			int flagError = 0;
			string functionNameAX = "GetStockAX%";
			string functionNameAPI = "GetItemAPI";
			string functionNameAPIAddItem = "AddItemMultipleAPI";
			string urlAX = "";
			string urlAPI = "";
			string itemIdStock = "";
			decimal availQtyStock = 0;
			decimal availQtyStockSO = 0;
			decimal availQty = 0;
			decimal remainQty = 0;
            decimal qtyReceive = 0;
            decimal qtySO = 0;
			//decimal qtyReceive = 0;
			string positiveStatus = "";
			
			string inventLocationId = ApplicationSettings.Terminal.InventLocationId;
			string inventSiteId = getInventSite(inventLocationId); 

			APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
			APIAccess.APIFunction APIFunction = new APIAccess.APIFunction();
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Apakah yakin akan melakukan posting?\nHarap pastikan qty barang sudah sesuai.", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
				if (dialog.DialogResult == DialogResult.Yes)
				{
					List<string> values = new List<string>();

					XmlDocument xmlDoc = new XmlDocument();
					// Create the XML declaration with version and encoding
					XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
					xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
					// Create a new root element called "SalesTable" and add it to the XmlDocument object
					XmlElement root = xmlDoc.CreateElement("SalesTable");
					root.SetAttribute("SalesId", txtSalesOrder.Text);
					xmlDoc.AppendChild(root);                    
					// Loop through the rows of the DataGridView and add a new SalesLine element for each row
					foreach (DataGridViewRow row in dataGridView1.Rows)
					{
                        
						//if (row.Cells["QtyDO"].Value == null)
						if (row.Cells["QtyDO"].Value == null || row.Cells["QtyDO"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["QtyDO"].Value.ToString())) 
						{
							flagError = 2;
							break;
						}
						else
						{
                            qtyReceive = Convert.ToDecimal(row.Cells["QtyDO"].Value);
                            qtySO = Convert.ToDecimal(row.Cells["QtySO"].Value);
                            if (qtyReceive < 0)
                            {
                                flagError = 6;
                            }
                            
                            //if (Convert.ToDecimal(row.Cells["QtyDO"].Value) <= Convert.ToDecimal(row.Cells["QtySO"].Value))
                            else
                            {
                                if (qtyReceive <= qtySO)
                                {
                                    // Get the ItemId, SalesQty, and DeliverNow values from the DataGridView row
                                    string itemId = row.Cells["ItemId"].Value.ToString();
                                    string itemName = row.Cells["ItemName"].Value.ToString();
                                    string deliverNow = row.Cells["QtyDO"].Value.ToString();

                                    //check if this is stocked item

                                    if (orderType == 1 && qtyReceive != qtySO)   //if receive qty is different than the qty SO - add by Yonathan 04102024                               
                                    {
                                        flagError = 7;
                                        break;
                                    }
                                    else if (orderType == 1 && qtyReceive == 0)
                                    {
                                        flagError = 8;
                                        break;
                                    }
                                    positiveStatus = checkPositiveStatus(itemId);
                                    if (positiveStatus == "1")
                                    {
                                        //check stock before posting packing slip                                    
                                        urlAX = APIClass.getURLAPIByFuncName(functionNameAX);
                                        urlAPI = APIClass.getURLAPIByFuncName(functionNameAPI);

                                        //check stock from AX
                                        ReadOnlyCollection<object> conResult = APIFunction.checkStockOnHand(SalesOrder.InternalApplication, urlAX, SalesOrder.InternalApplication.Settings.Database.DataAreaID, inventSiteId, inventLocationId, itemId, "", "", "");
                                        if (conResult.Count == 1)
                                        {
                                            flagError = 3;
                                            break;
                                        }
                                        else
                                        {
                                            if (conResult[5] != "0")
                                            {
                                                availQtyStock = Convert.ToDecimal(conResult[5]);
                                            }
                                            else
                                            {
                                                flagError = 4;
                                                break;
                                            }
                                            availQtyStock = Convert.ToDecimal(conResult[5] != "" ? conResult[5] : 0);
                                        }
                                        //check stock from daily transaction through API
                                        string resultCheckStockSO = APIFunction.checkStockSO(urlAPI, itemId, SalesOrder.InternalApplication.Settings.Database.DataAreaID, ApplicationSettings.Terminal.InventLocationId, "", "0", "0", "", "");
                                        APIParameter.parmResponseStockSO responseCheckStockSO = APIFunction.MyJsonConverter.Deserialize<APIParameter.parmResponseStockSO>(resultCheckStockSO);
                                        availQtyStockSO = decimal.Parse(responseCheckStockSO.response_data, CultureInfo.InvariantCulture);

                                        availQty = availQtyStock + availQtyStockSO; //
                                        remainQty = availQty - Convert.ToDecimal(row.Cells["QtyDO"].Value);



                                     


                                        if (remainQty < 0)
                                        {
                                            flagError = 5;
                                            //itemIdStock += itemIdStock == "" ? string.Format("Itemid-NamaBarang-Stok\n{0} - {1} - (Qty {2})\n", itemId, itemName, availQty.ToString()) : string.Format("{0} - {1} - Qty {2}\n", itemId, itemName, availQty.ToString());

                                            itemIdStock += "{" + itemId + ";" + itemName + ";" + row.Cells["QtyDO"].Value + ";" + availQty.ToString() + "}";

                                        }
                                        else
                                        {

                                            XmlElement salesLine = xmlDoc.CreateElement("SalesLine");
                                            salesLine.SetAttribute("ItemId", itemId);
                                            salesLine.SetAttribute("QtyRcv", deliverNow);
                                            root.AppendChild(salesLine);

                                        }
                                    }
                                    else
                                    {
                                        XmlElement salesLine = xmlDoc.CreateElement("SalesLine");
                                        salesLine.SetAttribute("ItemId", itemId);
                                        salesLine.SetAttribute("QtyRcv", deliverNow);
                                        root.AppendChild(salesLine);
                                    }
                                }
                                
                                else //if receive qty is bigger
                                {
                                    flagError = 1;
                                    break;
                                }
                            } 
                            
                            
						}
					}

					if (flagError == 0)
					{
						// Save the modified XML document to a string
						string updatedXmlString = xmlDoc.OuterXml;

						try
						{                            
							CreatePackingSlip(out retValue, out comment, out splitInvoice, salesID, updatedXmlString);
							if (retValue == false)
							{
								SalesOrder.InternalApplication.Services.Dialog.ShowMessage(comment, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							else
							{
                                if (comment == "4") //if sales order cancelled
                                {
                                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage("Sales Order dibatalkan.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage("Posting berhasil.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    SalesOrderActions.TryPrintPackSlip(LSRetailPosis.Transaction.SalesStatus.Delivered, salesID, "1");

                                    string invoiceAx = "";
                                    //string comboInvoice = "0";
                                    //validate if automatically post invoice after packing slip DO (based on customer master) - Yonathan 17092024
                                    if(splitInvoice == "0")
                                    {
                                        CreateInvoice(out invoiceAx);
                                    }
                                    
                                }
								
								//add to trigger
                                /* disable adding to API
								var packList = new List<APIParameter.parmRequestAddItemMultiple>();                                 
								foreach (DataGridViewRow row in dataGridView1.Rows)
								{
									//parmRequest pack = new parmRequest()
									var pack = new APIParameter.parmRequestAddItemMultiple()
									{
										ITEMID = row.Cells["ItemId"].Value.ToString(),
										QTY = (Convert.ToDecimal(row.Cells["QtyDO"].Value) * -1).ToString().Replace(",", "."), //lines.QuantityOrdered.ToString(),
										UNITID = row.Cells["Unit"].Value.ToString(),
										DATAAREAID = SalesOrder.InternalApplication.Settings.Database.DataAreaID,
										WAREHOUSE = ApplicationSettings.Terminal.InventLocationId,
										TYPE = "2",
										REFERENCESNUMBER = salesID,
										RETAILVARIANTID = "" 
									};
									//var data = MyJsonConverter.Serialize(pack);
									packList.Add(pack);
								}
								urlAPI = APIClass.getURLAPIByFuncName(functionNameAPIAddItem);
								string resultAPI = APIFunction.addItemMultiple(urlAPI, packList);
                                 * */
								//application.Services.Printing.PrintReceipt(Microsoft.Dynamics.Retail.Pos.Contracts.Services.FormType.SalesOrderReceipt, posTransaction, true);
								
                                
							}
						
							this.Close();
                           

						}
						catch (Exception x)
						{
							ApplicationExceptionHandler.HandleException(CP_frmPackingSlipDetail.LogSource, x);
							// "Error creating the packing slip."
							SalesOrder.InternalApplication.Services.Dialog.ShowMessage(x.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
						}

					}
					else if( flagError == 1)
					{
						SalesOrder.InternalApplication.Services.Dialog.ShowMessage("QtyDO tidak boleh lebih besar dari QtySo.", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else if(flagError == 2)
					{
						SalesOrder.InternalApplication.Services.Dialog.ShowMessage("Belum input QtyDO.\nApabila tidak di-receive, ketik 0 pada QtyDO.", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
                    else if (flagError == 3 || flagError == 4 || flagError == 5)
                    {
                        FlexibleMessageBox.FONT = new Font("Segoe", 16, FontStyle.Regular);
                        //itemIdStock += "Itemid-ProductName-Stok\n";


                        //itemIdStock += string.Format("\nStok item diatas no SO: {0} tidak mencukupi", salesID);
                        //var result = FlexibleMessageBox.Show(itemIdStock, "Infolog Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        //add new check stock UI - Yonathan 04102024
                        using (CP_frmStockInfolog customDialog = new CP_frmStockInfolog(itemIdStock))//, transaction))
                        {
                             
                                customDialog.ShowDialog(this);

                            




                        }
                        //end

                        //SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Stock item tidak cukup\nTidak bisa melakukan packing slip\nItem Id : {0}",itemIdStock), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (flagError == 6)
                    {
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage("QtyDO tidak boleh minus / negatif.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (flagError == 7)
                    {
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage("QtyDO dengan QtySO untuk SO Online tidak boleh berbeda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (flagError == 8)
                    {
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage("QtyDO untuk SO Online tidak boleh 0", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

				}
			}
		}
        private void CreateInvoice(out string _invoiceAx)
        {
            _invoiceAx = "";
            string statusInvoice;
            try
            {
                object[] parameterList = new object[] 
							{
								salesID.ToString(),
								DateTime.Now
								
							};

                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("postSalesInvoice", parameterList);
                _invoiceAx = containerArray[3].ToString();
                statusInvoice = containerArray[2].ToString();
                if (statusInvoice == "Success")
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(String.Format("Invoice {0} sudah terbentuk.", _invoiceAx), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //print
                    //update invoice id yonathan 06092024
                    updateInvoiceId(_invoiceAx, salesID.ToString()); 
                    ITransactionSystem transSys = SalesOrder.InternalApplication.BusinessLogic.TransactionSystem;
                    transaction.InvoiceComment = _invoiceAx;
                    transaction.Save();
                   
                    transSys.PrintTransaction(transaction, false, false); //print original
                    transSys.PrintTransaction(transaction, true, false); //print for copy
                    
                }
                else
                {
                    throw new Exception(string.Format("Invoice error, please post invoice on AX"));
                    //throw new Exception(string.Format("Invoice error, please post invoice on AX\n{0}", statusInvoice));
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }
        //update invoice id yonathan 06092024
        private void updateInvoiceId(string _invoiceAx, string _salesId)
        {
            string storeId = "";
            bool update = false;

            SqlConnection connection = ApplicationSettings.Database.LocalConnection;
            storeId = ApplicationSettings.Terminal.StoreId;
            //var retailTransaction = posTransaction as RetailTransaction;
            try
            {
                string queryString = @" UPDATE AX.RETAILTRANSACTIONTABLE
                                        SET 
                                        INVOICECOMMENT = @INVOICECOMMENT,
                                        MODIFIEDDATETIME = DATEADD(HOUR, -(DATEPART(TZOFFSET, SYSDATETIMEOFFSET()) / 60), SYSDATETIME())  
                                        WHERE STORE = @STOREID
                                        AND  SALESORDERID =@SALESID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STOREID", storeId);
                    command.Parameters.AddWithValue("@SALESID", _salesId);
                    command.Parameters.AddWithValue("@INVOICECOMMENT", _invoiceAx);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    // Execute the query and check affected rows 
                    int rowsAffected = command.ExecuteNonQuery();
                    update = rowsAffected > 0; // If rows are affected, update was successful, if not then it's online order = Yonathan 20112024

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

            if (!update)
            {

                //treat is as online pos order and insert the data - Yonathan 20112024

                try
                {
                    string queryString = @" INSERT INTO  AX.CPPOSONLINEORDER
                                        (RETAILSTOREID,SALESID,STAFFID,TRANSDATETIME,DATAAREAID,PARTITION)
                                        VALUES
                                        (@STOREID,@SALESID,@STAFFID,DATEADD(HOUR, -(DATEPART(TZOFFSET, SYSDATETIMEOFFSET()) / 60), SYSDATETIME()),@DATAAREAID,@PARTITION)"
                                        ;

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@STOREID", storeId);
                        command.Parameters.AddWithValue("@SALESID", _salesId);
                        command.Parameters.AddWithValue("@STAFFID", ApplicationSettings.Terminal.TerminalOperator.OperatorId);
                        command.Parameters.AddWithValue("@DATAAREAID", SalesOrder.InternalApplication.Settings.Database.DataAreaID);
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
		private string getInventSite(string inventLocationId)
		{
			string inventSite = "";
			string storeId = "";
			SqlConnection connection = ApplicationSettings.Database.LocalConnection;
			storeId = ApplicationSettings.Terminal.StoreId;
			//var retailTransaction = posTransaction as RetailTransaction;
			try
			{
				string queryString = @" SELECT A.INVENTLOCATION, A.INVENTLOCATIONDATAAREAID, C.INVENTSITEID 
							FROM ax.RETAILCHANNELTABLE A, ax.RETAILSTORETABLE B, ax.INVENTLOCATION C
							WHERE A.RECID=B.RECID AND C.INVENTLOCATIONID=A.INVENTLOCATION AND B.STORENUMBER=@STOREID";

				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@STOREID", storeId);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{

							inventSite = reader["INVENTSITEID"].ToString();
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
			return inventSite;
		}

		public void CreatePackingSlip(
			out bool retValue,
			out string comment,
            out string splitInvoice,
			string salesId, string _xmlString)
		{
            splitInvoice = "0";
			try
			{
				object[] parameterList = new object[] 
							{
								txtSalesOrder.Text.ToString(),
								_xmlString
								
							};


				ReadOnlyCollection<object> containerArray = application.TransactionServices.InvokeExtension("postPackingSlip", parameterList);
				retValue = (bool)containerArray[1];
				comment = containerArray[2].ToString();
                
				if (containerArray.Count > 3)
				{
					string detailComment = containerArray[3].ToString();
                    splitInvoice = containerArray[3].ToString();
                    if (!string.IsNullOrWhiteSpace(detailComment))
					{
                        comment += detailComment;
					}

                    if (!string.IsNullOrWhiteSpace(splitInvoice))
                    {
                        splitInvoice = splitInvoice;
                    }
                    else
                    {
                        splitInvoice = "0";
                    }

                    
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void lblSalesOrder_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void header_Click(object sender, EventArgs e)
        {

        }
	}
}