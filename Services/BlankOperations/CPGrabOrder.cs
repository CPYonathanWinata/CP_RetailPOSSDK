using LSRetailPosis.POSControls.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

using System.Reflection;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Xml;
using System.Collections.ObjectModel;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using System.Globalization;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using LSRetailPosis;
using LSRetailPosis.Transaction.Line;
using LSRetailPosis.Transaction.Line.Discount;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPGrabOrder : LSRetailPosis.POSControls.Touch.frmTouchBase
    {

        DataTable dataTableList;
        IPosTransaction posTransaction;
        IApplication application;
        string salesID;
        int currentPanelIndex;
        APIAccess.APIParameter.Data itemDetails;
        int findFalse  ;
        string driverState;
        string orderId = "";
        string orderIdLong = "";
        string merchantId = "";
        string nameCust, phoneCust = "";
        int exponent = 0;
        //string gmTID = "19"; //DEV
        string gmTID = "16"; //Prod
        public CPGrabOrder(IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent(); 
            posTransaction = _posTransaction;
            //posTransaction = _posTransaction; 
            application = _application;
            dateTimeBox.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000; //1 seconds
            timer.Start();
            

            storeBox.Text = ApplicationSettings.Terminal.StoreId + " - " + ApplicationSettings.Terminal.StoreName; 
            this.BringToFront();

            parentPanel.Controls.Remove(tabControl);
            currentPanelIndex = -1;
            SetCurrentTab(0);
            generateList();
            // Assuming you have a DataGridView named dataGridView1 on your form
            this.grabMartList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set the font size for the headers
            this.grabMartList.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 14, FontStyle.Bold);

            // Set the font size for the rows
            this.grabMartList.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);

            // Set the font size for a specific column (e.g., the first column)
            //grabMartList.Columns[0].DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Italic);
            this.grabMartList.CellContentClick+=grabMartList_CellContentClick;
            this.grabMartList.RowTemplate.Height = 50; 
        }

        

        private void btnOk_Click(object sender, EventArgs e)
        {
            
        }
   
        void timer_Tick(object sender, EventArgs e)
        {
            dateTimeBox.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
        }

        private string updateStatus(string _merchantId, string _orderId, string _receiptId)
        {
            //int rowIndex;
            string result = "";
            //string amountCashout = "";
            string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            var url = "";// "https://devpfm.cp.co.id/api/grab/updateStatusReceipt";
            string functionName = "UpdateOrderGrabAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");

                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new APIAccess.APIParameter.parmUpdateStatusListGrabmart()
                {
                    merchantID = _merchantId,
                    orderID = _orderId,                
                    receiptID = _receiptId
                };

                try
                {
                    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //return "0";
                    throw;

                }
            }
            return result;
        }

         
        private string checkOrderStatus(string _merchantId, string _orderIdLong)
        {
            string result = "";
            //int rowIndex;
            //string amountCashout = "";
            string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            var url = "";// "https://devpfm.cp.co.id/api/grab/getCurrentState";
           
            string functionName = "GetOrderStatusGRABMART";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");

                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new APIAccess.APIParameter.parmGetCurrentOrderStateGrabmart()
                {
                    merchantID = _merchantId,
                    orderID = _orderIdLong
                   
                };

                try
                {
                    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //return "0";
                    throw;

                }
            }

            return result;
        }
        private void generateList()
        {
            //int rowIndex;
            //string amountCashout = "";
            APIAccess.APIParameter.ApiResponseGrabmart responseAPI;
            string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            //var url = "https://devpfm.cp.co.id/api/grab/listOrder"
            string url = "";
            APIAccess.APIParameter.Receiver receiverParm;
            string functionName = "GetGRABMARTAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {


                

                //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                //string result = "";
                //httpRequest.Method = "POST";
                //httpRequest.ContentType = "application/json";
                //httpRequest.Headers.Add("Authorization", "PFM");

                ////initialize the required parameter before post to API

                ////parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                ////parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                ////parmRequestCashOut.MODULETYPE = "CO";
                ////parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                //var pack = new APIAccess.APIParameter.parmOrderListGrabmart()
                //{
                //    warehouse = storeId,
                //    dateFrom = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),//"2023-11-10 00:00:00", //DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),                    
                //    dateTo = DateTime.Now.ToString("yyyy-MM-dd 23:59:59") //"2023-11-10 23:59:59"
                //};




                //try
                //{
                //    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                //    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                //    {
                //        streamWriter.Write(data);
                //    }

                //    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        result = streamReader.ReadToEnd();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                //    //return "0";
                //    throw;

                //}

                //APIAccess.APIParameter.ApiResponseGrabmart responseData = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(result); //MyJsonConverter.Deserialize<parmResponse>(result);

                ////Data order = MyJsonConverter.Deserialize<Data>(responseData.data);
                //APIAccess.APIParameter.Data[] order = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.Data[]>(responseData.data);

                

                responseAPI = APIAccess.APIFunction.GrabMartAPI.getOrderList(storeId, url);
                if (responseAPI.data != "")
                {
                    APIAccess.APIParameter.Data[] order = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.Data[]>(responseAPI.data);
                    // Open status emptystring agar bis ditransaksikan selama belum distrukin - Yonathan 11112024
                    var groupedData = order.GroupBy(item => item.orderID).Select(group => new
                    {
                        OrderID = group.Key,
                        Items = group.ToList()
                        
                    });



                   

                    // Add data to grabMartList
                    foreach (var group in groupedData)
                    {
                    
                        foreach (var item in group.Items)
                        {
                            merchantId = item.merchantID;

                            int rowIndex = grabMartList.Rows.Add(
                                item.orderID,
                                item.shortOrderNumber,
                                item.state,
                               item.orderTime


                            );

                            // Store the original item data in the row tag for later use
                            grabMartList.Rows[rowIndex].Tag = item;


                        }
                    }

                }
                else
                {
                    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(responseAPI.message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                        return;
                    }
                    
                }
                
                


               
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (BackButtonClicked())
            {
                int currentIndex = currentPanelIndex;
                currentIndex--;
                if (currentIndex >= 0)
                {
                    SetCurrentTab(currentIndex);
                    grabMartList.Rows.Clear();
                    generateList();
                    if (currentIndex == 0)
                    {
                        //btnBack.Enabled = false;
                        //btnBack.BackColor = Color.DarkGray;
                    }
                }
            }
        }

        private void grabMartList_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            orderId = "";
            
            string reasonMessage = string.Empty;
            driverState = "";

            

            if (e.ColumnIndex == grabMartList.Columns["Details"].Index && e.RowIndex >= 0)
            {
                APIAccess.APIParameter.Data item = (APIAccess.APIParameter.Data)grabMartList.Rows[e.RowIndex].Tag;
                orderId = grabMartList.Rows[e.RowIndex].Cells["OrderID"].Value.ToString();
                driverState = grabMartList.Rows[e.RowIndex].Cells["state"].Value.ToString();
                orderIdLong = grabMartList.Rows[e.RowIndex].Cells["OrderIDLong"].Value.ToString();

                //if (driverState == "DELIVERED") //add by Yonathan because of delivered status 09082024
                //{
                //    using (LSRetailPosis.POSProcesses.frmMessage dialog3 = new LSRetailPosis.POSProcesses.frmMessage("Pesanan ini tidak bisa diproses karena status pesanan telah DELIVERED dari sisi DRIVER", MessageBoxButtons.OK, MessageBoxIcon.Error))
                //    {
                //        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog3);
                //        return;
                //    }
                //}

                if (NextButtonClicked(out reasonMessage))
                {
                    int currentIndex = currentPanelIndex;
                    currentIndex++;
                    if (!(currentIndex >= tabControl.TabCount))
                    {
                        SetCurrentTab(currentIndex);
                        btnOK.Visible = true;
                        //btnBack.BackColor = Color.FromArgb(171, 194, 215);

                        if (currentIndex == tabControl.TabCount - 1)
                        {
                            //btnNext.Enabled = false;
                            //btnNext.BackColor = Color.DarkGray;

                            //btnFinish.Enabled = true;
                            //btnFinish.BackColor = Color.FromArgb(171, 194, 215);
                        }
                    }
                    

                    // Show order details in a new form
                    
                    ShowOrderDetailsForm(item, orderId, orderIdLong, driverState);
                }
                else
                {
                    if (reasonMessage.Length > 0)
                        MessageBox.Show(reasonMessage, "Error", MessageBoxButtons.OK);
                }
                // Get the original item data from the row tag
               
                    
            }
        }

        private void grabMartList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grabMartList.RowCount > 0)
            {
                grabMartList.Columns["Details"].Visible = true;
            }
            else
            {
                grabMartList.Columns["Details"].Visible = false;
            }
        }
        private void ShowOrderDetailsForm(APIAccess.APIParameter.Data item, string _orderId, string _orderIdLong, string _driverStatus)
        {
            findFalse = 0;
            itemDetailsGrid.Rows.Clear();
            
            bool isFirstIteration = true;
            decimal priceAfterExponent = 0;
            string priceAfterExponentString = "";
            string discAfterExponentString = "";
            decimal discAfterExponent = 0;
            bool isAvail = true;
            string isAvailable = "Ya";
            decimal subTotal = 0;
            decimal grandTotal = 0;
            string siteId = "";
            string warehouseId = "";
            string itemIdMulti = "";
            //string qtyMulti = "";
            string barcodeMulti = "";
            string configIdMulti= "";
            string qtyMulti = ""; //add Qty by Yonathan 11092024
            string xmlResponse;
            // Set the font size for the headers
            //this.itemDetailsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 14, FontStyle.Bold);

            //// Set the font size for the rows
            //this.itemDetailsGrid.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            var urlRTS="";
            var urlAPI="";
            string functionNameAX = "GetStockAX%"; // "GetStockAXPFMPOC"; //change to GetStockAX
            string functionNameAPI = "GetItemAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            urlRTS = APIClass.getURLAPIByFuncName(functionNameAX);
            urlAPI = APIClass.getURLAPIByFuncName(functionNameAPI);

            APIAccess.APIFunction apiFunction = new APIAccess.APIFunction();


            //Form detailsForm = new Form();
            //detailsForm.StartPosition = FormStartPosition.CenterParent;
            //DataGridView detailsDataGridView = new DataGridView();

            // Set DataGridView properties
            //itemDetailsGrid.Dock = DockStyle.Fill;
            //detailsForm.Controls.Add(detailsDataGridView);

            // Set up columns for DataGridView
            //itemDetailsGrid.Columns.Add("ItemID", "Item ID");
            //itemDetailsGrid.Columns.Add("Quantity", "Quantity");
            
            // Add order details to DataGridView
            itemDetails = item;
            
            int indexRow=0;

            if (item.receiver != null)//|| item.receiver != "")
            {
                APIAccess.APIAccessClass.grabCustName = item.receiver.name;
                APIAccess.APIAccessClass.grabCustPhone = item.receiver.phones;

                nameCust = item.receiver.name;
                phoneCust = item.receiver.phones;
            }
            foreach (var orderItem in item.items)
            {
                //check positive stock first

                if (APIAccess.APIFunction.checkPositiveStatus(orderItem.id, LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection) == true)
                {
                    if (isFirstIteration)
                    {

                        exponent = item.currency.exponent;
                        BlankOperations.exponent = exponent;
                        APIAccess.APIAccessClass.merchantId = item.merchantID;
                        isFirstIteration = false;
                    }

                    //loop through the items in the cart
                    itemIdMulti += orderItem.id;
                    qtyMulti += orderItem.quantity; //add Qty by Yonathan 11092024
                    //barcodeMulti +=
                    //configIdMulti +=
                    //qtyMulti +=
                    // Add the itemid to the result string
                    //result += itemId.ToString();

                    // Add the separator (;) if it's not the last item
                    if (orderItem.id != item.items[item.items.Count - 1].id)
                    {
                        itemIdMulti += ";";
                        qtyMulti += ";"; //add Qty by Yonathan 11092024
                    }
                }

                
            }

            //get the inventSiteId
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            var retailTransaction = posTransaction as RetailTransaction;
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

                //                //string queryString2 = @"SELECT ID.INVENTDIMID, ITEMID, CONFIGID FROM INVENTDIM ID JOIN INVENTITEMBARCODE IB ON ID.INVENTDIMID = IB.INVENTDIMID
                //                //                         WHERE ITEMID = @ITEMID";

                //                string queryString2 = @"SELECT ITEMID, RETAILVARIANTID FROM [ax].[INVENTITEMBARCODE]
                //                                         WHERE ITEMID =  @ITEMID";
                //                using (SqlCommand command = new SqlCommand(queryString2, connection))
                //                {
                //                    command.Parameters.AddWithValue("@ITEMID", saleLineItem.ItemId);

                //                    if (connection.State != ConnectionState.Open)
                //                    {
                //                        connection.Open();

                //                    }
                //                    using (SqlDataReader reader = command.ExecuteReader())
                //                    {
                //                        if (reader.Read())
                //                        {
                //                            //configId = reader["CONFIGID"].ToString();
                //                            configId = reader["RETAILVARIANTID"].ToString();
                //                        }

                //                    }
                //                }
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

            //check whether itemIdMulti is empty. if empry, don't check the RTS
            /*<?xml version="1.0" encoding="utf-8"?><GetStockList><StockListResult ItemId="70000004" Barcode="" QtyAvail="45,000" QtyPhy="45,000" /></GetStockList>*/
            ReadOnlyCollection<object> result = null;
            XmlNodeList itemNodes = null ;
            if (itemIdMulti != "")
            {
                 result = apiFunction.checkStockOnHandMultiNew(application, urlRTS, application.Settings.Database.DataAreaID, siteId, ApplicationSettings.Terminal.InventLocationId, itemIdMulti, "", "", "", qtyMulti, posTransaction.StoreId + "-" + _orderId.ToString()); // mod by Yonathan to add 2 parameters qty and trans id 11092024
                 xmlResponse = result[3].ToString();

                 XmlDocument xmlDoc = new XmlDocument();
                 xmlDoc.LoadXml(xmlResponse);

                 itemNodes = xmlDoc.SelectNodes("//StockListResult");
            }
            
            
            //loop for each node
            //foreach (XmlNode node in itemNodes)
            //{
            //    string itemId = node.Attributes["ItemId"].Value;

            //    decimal qtyAvail = decimal.Parse(node.Attributes["QtyAvail"].Value, CultureInfo.InvariantCulture);
            //    decimal qtyPhy = decimal.Parse(node.Attributes["QtyPhy"].Value, CultureInfo.InvariantCulture);// decimal.Parse(node.Attributes["SalesPrice"].Value).ToString("0");
                


            //}

            if (item.campaigns != null)
            {
                foreach (var campaign in item.campaigns)
                {

                    var applicableItems = item.items.Where(i => campaign.appliedItemIDs.Contains(i.id)).ToList();

                    int totalQty = applicableItems.Sum(i => i.quantity);

                    if (totalQty > 0)
                    {
                        foreach (var orderItem in applicableItems)
                        {
                            decimal itemDiscount = (orderItem.quantity / (decimal)totalQty) * campaign.deductedAmount;
                            orderItem.discAmt += itemDiscount / orderItem.quantity;

                            
                        }
                    }
                }
            }



            string itemType = "Stock";
            foreach (var orderItem in item.items)
            {
                
                
                if (isFirstIteration)
                {

                    exponent = item.currency.exponent;

                    APIAccess.APIAccessClass.merchantId = item.merchantID;
                }
                //ApplicationSettings.Terminal.InventLocationId;
                
                lblOrder.Text = "Order Number : " + _orderId.ToString();

                //check Stock
                //var resultRTS = apiFunction.checkStockOnHand(application, urlRTS, application.Settings.Database.DataAreaID, "JKT", ApplicationSettings.Terminal.InventLocationId, orderItem.id, "", "", "");

                //decimal availQtyStock = Convert.ToDecimal(resultRTS[5] != "" ? resultRTS[5] : 0);
                //var resultAPI = apiFunction.checkStockSO(urlAPI, orderItem.id, application.Settings.Database.DataAreaID, ApplicationSettings.Terminal.InventLocationId, "", availQtyStock.ToString().Replace(",", "."), orderItem.quantity.ToString(), "GrabMartCheckStock", "");
                //end
                //APIAccess.APIParameter.parmResponseStockSO responseCheckStockSO = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.parmResponseStockSO>(resultAPI);
                //decimal availQtyStockSO = decimal.Parse(responseCheckStockSO.response_data, CultureInfo.InvariantCulture);
                //MessageBox.Show(String.Format("API Config Id : {0}, Available Qty {1}", configId.ToString(), availQtyStockSO));
                //compare the available stock with sales transaction
                //decimal availQty = availQtyStock + availQtyStockSO; //
                //MessageBox.Show(String.Format("Config Id : {0}, ({1} + {2}) = {3}" , configId.ToString(), availQtyStock, availQtyStockSO, availQty));
                decimal remainQty = 0;//availQty;// -(orderItem.quantity);// + qtyBeforeAdded);
                string itemId = "";
                bool found = false;
                //remainQty = Convert.ToDecimal(itemNodes[indexRow].Attributes["QtyAvail"].Value);
                //add for nonstock items 17062025 - Yonathan
                if(itemNodes != null)
                {
                    foreach (XmlNode node in itemNodes)
                    {
                        if (node.Attributes["ItemId"].Value == orderItem.id)
                        {
                            remainQty = decimal.Parse(node.Attributes["QtyAvail"].Value, NumberStyles.Number, CultureInfo.CurrentCulture); //.Replace(",", ".")
                            itemId = node.Attributes["ItemId"].Value.ToString();
                            found = true;
                            break;
                        }
                        //else
                        //{
                        //    itemType = "Non";
                        //}
                    }
                }
                //end
                
                //remainQty = itemNodes == null ? 0 : Convert.ToDecimal(itemNodes[indexRow].Attributes["QtyAvail"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                priceAfterExponentString = orderItem.price.ToString().Substring(0, orderItem.price.ToString().Length - exponent);
                decimal.TryParse(priceAfterExponentString, out priceAfterExponent);

                discAfterExponentString = orderItem.discAmt.ToString().Substring(0, orderItem.discAmt.ToString().Length - exponent);
                decimal.TryParse(discAfterExponentString, out discAfterExponent);

                subTotal = (priceAfterExponent * orderItem.quantity);
                grandTotal += subTotal - (discAfterExponent * orderItem.quantity); //(orderItem.discAmt*orderItem.quantity);

                //if(itemNodes != null)
                //add for nonstock items 17062025 - Yonathan CP_MDFPOSGRABORDER
                if (found == true) //found in XML
                {
                    isAvailable = remainQty - orderItem.quantity < 0 ? "Tidak" : "Ya";
                    itemType = "Stock";
                }
                else //not Found in XML -> Non Stock
                {
                    itemType = "Non";
                    isAvailable = "Ya";
                }
                //add to row
                itemDetailsGrid.Rows.Add(
                    orderItem.id,//itemNodes == null ? orderItem.id : itemId,
                    getItemName(orderItem.id),
                    orderItem.specifications,
                    orderItem.quantity,
                    priceAfterExponent,
                    discAfterExponent * orderItem.quantity,//orderItem.discAmt*orderItem.quantity,
                    subTotal,
                    itemType == "Non" ? "Non Stock" : remainQty.ToString("N2",CultureInfo.CurrentCulture), //remainQty.ToString(CultureInfo.InvariantCulture),
                    isAvailable// = remainQty - orderItem.quantity < 0 ? "Tidak" : "Ya"

                ); 

                //itemDetailsGrid.Rows.Add(s
                //    orderItem.id,
                //    getItemName(orderItem.id),                    
                //    orderItem.specifications,
                //    orderItem.quantity,
                //    priceAfterExponent,
                //    subTotal,
                //    remainQty,
                //    isAvail =  remainQty - orderItem.quantity <0 ? false : true
                    
                //);

                //itemNodes[indexRow].Attributes["ItemId"].Value;
                DataGridViewRow row = itemDetailsGrid.Rows[indexRow];
                DataGridViewCell availableCell = row.Cells["Stock"];
                indexRow++;

                

                if (isAvailable == "Tidak")//Stock
                {
                    findFalse = 1;
                    availableCell.Style.ForeColor = Color.Red; 
                    //this.Stock.DefaultCellStyle.ForeColor = Color.Red;
                    this.Stock.DefaultCellStyle.Font = new Font(itemDetailsGrid.Font, FontStyle.Bold);
                    
                    
                     

                }
                else if (isAvailable == "Ya")
                {
                    availableCell.Style.ForeColor = Color.Green; 
                    //this.Stock.DefaultCellStyle.ForeColor = Color.Green;
                    this.Stock.DefaultCellStyle.Font = new Font(itemDetailsGrid.Font, FontStyle.Bold);
                }
                
            }
            txtGrandTotal.Text = "Total : " + grandTotal.ToString("N2");
            //this.grabMartList.Columns["colPrice"].DefaultCellStyle.Format = "C";
            // Show the order details form
            //detailsForm.ShowDialog();
        }


        public string getItemName(string _itemId)
        {
            string itemName = "" ;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @"SELECT ERT.NAME, ERP.DISPLAYPRODUCTNUMBER, ERP.RECID, ERT.PRODUCT FROM  [ax].[ECORESPRODUCTTRANSLATION] ERT join [ax].[ECORESPRODUCT] ERP ON ERT.PRODUCT = ERP.RECID WHERE DISPLAYPRODUCTNUMBER = @itemId";

               
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@itemId", _itemId);
                    
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            itemName = reader[0].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return itemName;
        }

        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
        }


        private void CPGrabOrder_Load(object sender, EventArgs e)
        {
           

        }

        private void changeGridStyles()
        {
            
        }

        private void SetCurrentTab(int currentIndex)
        {


            if (currentPanelIndex > -1) //remove all old controls back to tabcontrol
            {
                int count = parentPanel.Controls.Count;

                for (int i = count - 1; i >= 0; i--)
                {
                    Control control = parentPanel.Controls[i];
                    parentPanel.Controls.Remove(control);
                    tabControl.TabPages[currentPanelIndex].Controls.Add(control);
                }
            }

            {
                int count = tabControl.TabPages[currentIndex].Controls.Count;
                //m_labelSubtitle.Text = (currentIndex + 1) + " : " + tabControl.TabPages[currentIndex].Text;
                for (int i = count - 1; i >= 0; i--)
                {
                    Control control = tabControl.TabPages[currentIndex].Controls[i];
                    tabControl.TabPages[currentIndex].Controls.Remove(control);
                    parentPanel.Controls.Add(control);
                }

                currentPanelIndex = currentIndex;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            BlankOperations.grabMartProperties.grabMartStatus = false;
            this.Close();
        }

        private void btnOK_Click_1(object sender, EventArgs e)//, string _merchantId, string _orderId)
        {
            BlankOperations.grabPosTransaction = null;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            IApplication applicationLoc = PosApplication.Instance as IApplication;
            string result = "";
            SaleLineItem saleLineItem;
            decimal priceAfterExponent = 0;
            string priceAfterExponentString = "";
            if (findFalse == 0)
            {

                result = checkOrderStatus(merchantId, orderIdLong);

                APIAccess.APIParameter.ApiResponseGrabmart responseData = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(result); //MyJsonConverter.Deserialize<parmResponse>(result);

                //Data order = MyJsonConverter.Deserialize<Data>(responseData.data);
                APIAccess.APIParameter.DataStatusOrder resultStatus = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.DataStatusOrder>(responseData.data);
                driverState = resultStatus.state;

                if (driverState == "DRIVER_ARRIVED" || driverState == "COLLECTED" || driverState == "DELIVERED") // tambah status delivered agar bis ditransaksikan selama belum distrukin - Yonathan 11112024
                {
                    //add to cart & pay
                    
                    LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
                    //iSale.OperationID = PosisOperations.ItemSale;
                    //iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                    //iSale.Barcode = skuId; disable by Yonathan 21/10/2022

                    //use blank operation to store the items.
                    foreach (var orderItem in itemDetails.items)
                    {
                        RetailTransaction grabPosTransactionLocal = BlankOperations.grabPosTransaction as RetailTransaction;
                        BlankOperations.itemIdToAdd = orderItem.id;
                        BlankOperations.quantityToAdd = orderItem.quantity;
                        applicationLoc.RunOperation(PosisOperations.BlankOperation, "94", grabPosTransactionLocal);
                    }

                    //foreach (var orderItem in itemDetails.items)
                    //{
                    //    iSale = new LSRetailPosis.POSProcesses.ItemSale();
                    //    iSale.OperationID = PosisOperations.ItemSale;
                    //    iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                    //    iSale.Barcode = orderItem.id;  // change to this by yonathan 21/10/2022
                    //    //iSale.BarcodeInfo.ItemId = txtSKU.Text;
                    //    iSale.OperationInfo.NumpadQuantity = orderItem.quantity;//orderItem.id
                    //    iSale.POSTransaction = (LSRetailPosis.Transaction.PosTransaction)posTransaction;
                        
                    //    iSale.RunOperation();

                    //}
                    //BlankOperations.globalposTransaction
                    //Microsoft.Dynamics.Retail.Pos.BlankOperations.BlankOperations.globalposTransaction
                    RetailTransaction grabPosTransaction = BlankOperations.grabPosTransaction as RetailTransaction;

                    foreach (var itemSale in grabPosTransaction.SaleItems)
                    {
                        IApplication applicationLocal = PosApplication.Instance as IApplication;
                        APIAccess.APIParameter.Item foundItem = itemDetails.items.FirstOrDefault(item => item.id == itemSale.ItemId);
                        var orderItem = itemDetails.items.FirstOrDefault(item => item.id == itemSale.ItemId);


                        priceAfterExponentString = foundItem.price.ToString().Substring(0, foundItem.price.ToString().Length - exponent);

                        decimal.TryParse(priceAfterExponentString, out priceAfterExponent);





                        itemSale.CustomerPrice = priceAfterExponent;
                        itemSale.GrossAmount = priceAfterExponent;
                        itemSale.OriginalPrice = priceAfterExponent;
                        itemSale.Price = priceAfterExponent;
                        //salesLine.TradeAgreementPriceGroup = result[1];
                        itemSale.TradeAgreementPrice = priceAfterExponent;

                        LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem custDiscountManual = new LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem();

                        custDiscountManual.Amount = orderItem.discAmt;
                        //custDiscountManual.Percentage = Convert.ToDecimal(resultDisc[2]);
                        applicationLoc.Services.Discount.AddDiscountLine(itemSale, custDiscountManual);

                        applicationLoc.Services.Tax.CalculateTax(itemSale, grabPosTransaction);
                       
                       

                         
                        //saleLineItem = RetailTransaction.PriceOverride(itemSale, foundItem.price);//transaction.SaleItems.Last.Value, priceToOverride);
                        //saleLineItem = RetailTransaction.PriceOverride(itemSale, priceAfterExponent);//transaction.SaleItems.Last.Value, priceToOverride);







                        //decimal priceToOverride = priceAfterExponent;
                        //saleLineItem = RetailTransaction.SetCostPrice(itemSale, priceToOverride);
                        //saleLineItem.PriceOverridden = false;
                        //applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);
                        //grabPosTransaction.CalcTotals();

                        ////applicationLoc.RunOperation(PosisOperations.BlankOperation, "95", grabPosTransaction);

                        

                        //applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(grabPosTransaction);


                        

                        //string str = ((IServicesV1)PosApplication.Instance.Services).Rounding.Round(((BaseSaleItem)saleLineItem).OriginalPrice, true);
                        //POSFormsManager.ShowPOSStatusPanelText(ApplicationLocalizer.Language.Translate(3352, new object[3]
                        //        {
                        //            (object) ((LineItem) saleLineItem).Description,
                        //            (object) ((BaseSaleItem) saleLineItem).BarcodeId,
                        //            (object) str
                        //        }));
                    }
                    grabPosTransaction.CalcTotals();
                    grabPosTransaction.Save();
                    //applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(grabPosTransaction);
                    //grabPosTransaction.CalcTotals();
                    //grabPosTransaction.Save();

//                    foreach (var itemSale in grabPosTransaction.SaleItems)
//                    {
//                        //#GRABDISCOUNT 
//                        var orderItem = itemDetails.items.FirstOrDefault(item => item.id == itemSale.ItemId);
                        


//                        //LSRetailPosis.Transaction.Line.Discount.LineDiscountItem lineDisc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
//                        DiscountItem lineDisc = new LineDiscountItem();
//                        lineDisc.Amount = orderItem.discAmt;// *itemSale.qu
//;
//                        //applicationLoc.Services.Discount.AddLineDiscountAmount(grabPosTransaction.CurrentSaleLineItem, lineDisc);


//                        LineDiscountItem itemDisc = lineDisc as LineDiscountItem;
//                        itemDisc.Amount = lineDisc.Amount;
//                        itemDisc.LineDiscountType = LineDiscountItem.DiscountTypes.Manual;


//                        itemSale.DiscountLines.AddFirst(itemDisc);

//                        //END
//                    }
//                    grabPosTransaction.CalcTotals();
//                    grabPosTransaction.Save();

                    BlankOperations.grabPosTransactionDisc = grabPosTransaction;
                    //applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(grabPosTransaction);
                    //grabPosTransaction.CalcTotals();


                    //foreach (var orderItem in itemDetails.items)
                    //{

                    //}

                    //iSale.Barcode = "11320006";  // change to this by yonathan 21/10/2022
                    ////iSale.BarcodeInfo.ItemId = txtSKU.Text;
                    //iSale.OperationInfo.NumpadQuantity = 2;
                    //iSale.POSTransaction = (LSRetailPosis.Transaction.PosTransaction)posTransaction;

                    //iSale.RunOperation();
                    ////BlankOperations.grabMartProperties.grabMartStatus = true;


                    ////LSRetailPosis.POSProcesses.ItemSale iSale2 = new LSRetailPosis.POSProcesses.ItemSale();
                    //iSale = new LSRetailPosis.POSProcesses.ItemSale();
                    //iSale.OperationID = PosisOperations.ItemSale;
                    //iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                    ////iSale.Barcode = skuId; disable by Yonathan 21/10/2022
                    //iSale.Barcode = "11310014";  // change to this by yonathan 21/10/2022
                    ////iSale.BarcodeInfo.ItemId = txtSKU.Text;
                    //iSale.OperationInfo.NumpadQuantity = 3;
                    //iSale.POSTransaction = (LSRetailPosis.Transaction.PosTransaction)posTransaction;

                    //iSale.RunOperation();



                    RetailTransaction transaction = posTransaction as RetailTransaction;
                    var application = PosApplication.Instance as IApplication;

                    /*
            
                    PayCustomerAccountConfirmation paycust = new PayCustomerAccountConfirmation();
                    paycust.PosTransaction = posTransaction;
                    paycust.BalanceAmount = transaction.AmountDue;
                    //paycust.TenderInfo = transaction;
                    //Interaction.frmPayCustomerAccount pay = new Interaction.frmPayCustomerAccount(paycust);
                    Tender newTender = new Tender();
                    newTender.PosisOperation = PosisOperations.PayCustomerAccount;
                    newTender.TenderID = gmTID;
                    newTender.TenderName = transaction.Customer.Name;
                    paycust.TenderInfo = newTender;
                    Interaction.frmPayCustomerAccount pay = new Interaction.frmPayCustomerAccount(paycust);
                    pay.ShowDialog();
                    pay.command();
                     * */

                    
                    try
                    {

                        //string queryString = @"  INSERT INTO CPOrderTable (TRANSACTIONID,NOORDER,DATEORDER,NAMEORDER)
                        //                        VALUES ( @TRANSACTIONID  , @NOORDER , @DATEORDER,@NAMEORDER )";
                        string queryString = @"exec CP_UpdateInsertCPOrder @TRANSACTIONID  , @NOORDER , @DATEORDER,@NAMEORDER";

                        using (SqlCommand command = new SqlCommand(queryString, connection))
                        {
                            command.Parameters.AddWithValue("@TRANSACTIONID", grabPosTransaction.TransactionId.ToString());
                            command.Parameters.AddWithValue("@NOORDER", orderId.ToString());
                            command.Parameters.AddWithValue("@DATEORDER", DateTime.Now);
                            command.Parameters.AddWithValue("@NAMEORDER", "GRABMART");

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
                    this.Close();


                    //RetailTransaction grabPosTransactionLocal = BlankOperations.grabPosTransaction as RetailTransaction;
                     

                    APIAccess.APIAccessClass.grabOrderState = driverState;
                    APIAccess.APIAccessClass.grabOrderIdLong = orderIdLong;
                    //applicationLoc.RunOperation(PosisOperations.BlankOperation, "102", BlankOperations.grabPosTransactionDisc);
                    transaction = grabPosTransaction; // (RetailTransaction)BlankOperations.grabPosTransactionDisc; 
                    transaction.CalcTotals();
                    transaction.Save();
                    //BlankOperations.grabPosTransaction = grabPosTransaction;
                    //applicationLoc.RunOperation(PosisOperations.BlankOperation, "95", grabPosTransaction);
                    //applicationLoc.RunOperation(PosisOperations.BlankOperation, "102", grabPosTransaction);
                    //grabPosTransaction.CalcTotals();
                    //grabPosTransaction.Save();
                    //BlankOperations.grabPosTransaction = grabPosTransaction;
                    //application.RunOperation(PosisOperations.PayCustomerAccount, gmTID, BlankOperations.grabPosTransaction);


                    application.RunOperation(PosisOperations.PayCustomerAccount, gmTID, transaction); //tenderId - is ID payment method, transaction - your transaction object
                    //application.RunOperation(PosisOperations.PayCashQuick, gmTID, transaction); 
                    /*PayCash pay = new PayCash(false, gmTID);  // 1 is the number of Payement method

                    pay.OperationID = PosisOperations.PayCustomerAccount; // choose ure payment method
                    pay.OperationInfo = new OperationInfo();
                    pay.POSTransaction = (PosTransaction)posTransaction;
                    pay.Amount = transaction.NetAmountWithTax;
            
                    pay.RunOperation();*/

                    
                    this.Close();
                }
                else //if (driverState == "")
                {
                    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Driver belum sampai.\nJika driver sudah sampai, pastikan :\n1. Driver sudah update status 'Sudah Sampai Toko'\n2. Kemudian lakukan refresh list dan coba proses kembali.", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                        return;
                    } //"Driver belum sampai, jika driver sudah sampai pastikan driver sudah update status sampai toko lalu kemudian lakukan refresh list!"
                }
                //else
                //{
                //    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Driver belum sampai, tidak bisa melanjutkan order", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                //    {
                //        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                //        return;
                //    }
                //}
               
            }
            else if (findFalse == 1) 
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Salah satu stock item tidak cukup.\nTidak bisa melanjutkan order", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }
            


        }

        bool NextButtonClicked(out string reasonMessage)
        {
            reasonMessage = "OK";
            return true;
        }
        bool BackButtonClicked()
        {
            return true;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string reasonMessage = string.Empty;
            if (NextButtonClicked(out reasonMessage))
            {
                int currentIndex = currentPanelIndex;
                currentIndex++;
                if (!(currentIndex >= tabControl.TabCount))
                {
                    SetCurrentTab(currentIndex);
                    btnOK.Visible = true;
                    //btnBack.BackColor = Color.FromArgb(171, 194, 215);

                    if (currentIndex == tabControl.TabCount - 1)
                    {
                        //btnNext.Enabled = false;
                        //btnNext.BackColor = Color.DarkGray;

                        //btnFinish.Enabled = true;
                        //btnFinish.BackColor = Color.FromArgb(171, 194, 215);
                    }
                }
            }
            else
            {
                if (reasonMessage.Length > 0)
                    MessageBox.Show(reasonMessage, "Error", MessageBoxButtons.OK);
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            string resultRequest = "";
            string cancelRequest = "";
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Batalkan pesanan ini?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                if (dialog.DialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    //do the API request cancel
                    resultRequest = requestCancel(merchantId, orderIdLong);

                    APIAccess.APIParameter.ApiResponseGrabmart responseDataReq = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(resultRequest); // 
                    if (responseDataReq.error == true && responseDataReq.status == 500)
                    {
                        using (LSRetailPosis.POSProcesses.frmMessage dialog2 = new LSRetailPosis.POSProcesses.frmMessage("Pesanan tidak bisa dibatalkan karena limit.\nUntuk membatalkan pesanan ini,\nsilakan hubungi Grab CS (021)80648787 .\n\nNote :\nMerchant sudah tidak bisa melakukan cancel\nsetelah status order di-pickup oleh driver.", MessageBoxButtons.OK, MessageBoxIcon.Error))
                        {
                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog2);
                        }
                        //order cannot be cancelled because reaching the daily limit
                    }
                    else
                    {
                        //order can be cancelled
                        //do the API cancel with reasons 1001
                        cancelRequest = cancelOrder(merchantId, orderIdLong, "1001");

                        APIAccess.APIParameter.ApiResponseGrabmart responseDataCancel = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(cancelRequest); // 
                        if (responseDataCancel.error == true && responseDataCancel.status == 500)
                        {
                            using (LSRetailPosis.POSProcesses.frmMessage dialog3 = new LSRetailPosis.POSProcesses.frmMessage("Pesanan tidak bisa dibatalkan karena limit.\nUntuk membatalkan pesanan ini,\nsilakan hubungi Grab CS (021)80648787 .\n\nNote :\nMerchant sudah tidak bisa melakukan cancel\nsetelah status order di-pickup oleh driver.", MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog3);
                            }
                        }
                        else if(responseDataCancel.error == false && responseDataCancel.status == 200) 
                        {
                            using (LSRetailPosis.POSProcesses.frmMessage dialog3 = new LSRetailPosis.POSProcesses.frmMessage("Pesanan berhasil dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Information))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog3);
                            }
                            this.Close();
                        }
                        // close the form
                        
                    }
                }
                
                
                return;

            }


        }

        private string requestCancel(string _merchantId, string _orderIdLong)
        {
            string result = "";
            //int rowIndex;
            //string amountCashout = "";
            string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            var url = "";// "https://devpfm.cp.co.id/api/grab/requestCancel";
            string functionName = "ReqCancelGrabAPI";
             
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");

                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new APIAccess.APIParameter.parmGetCurrentOrderStateGrabmart()
                {
                    merchantID = _merchantId,
                    orderID = _orderIdLong

                };

                try
                {
                    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //return "0";
                    throw;

                }
            }

            return result;
        }

        private string cancelOrder(string _merchantId, string _orderIdLong, string _reasonCode)
        {
            string result = "";
            //int rowIndex;
            //string amountCashout = "";
            string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            var url = "";// "https://devpfm.cp.co.id/api/grab/cancel";
            string functionName = "CancelOrderGrabAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");

                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new APIAccess.APIParameter.parmCancelOrderGrabmart()
                {
                    merchantID = _merchantId,
                    orderID = _orderIdLong,
                    cancelReasons = _reasonCode

                };

                try
                {
                    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //return "0";
                    throw;

                }
            }

            return result;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grabMartList.Rows.Clear();
            generateList();
        }

         
    }
}
