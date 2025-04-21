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
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

using System;
using System.ServiceModel;
using System.Net;

using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using LSRetailPosis.Settings.FunctionalityProfiles;

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

using BlankOperations = Microsoft.Dynamics.Retail.Pos.BlankOperations;


using System.ComponentModel;


using System.Diagnostics;
using System.Drawing;
using System.Linq;

using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LSRetailPosis.Transaction.Line.InfocodeItem;
using LSRetailPosis.Transaction.Line.Discount;


namespace Microsoft.Dynamics.Retail.Pos.ItemTriggers
{
    /// <summary>
    /// <example><code>
    /// // In order to get a copy of the last item added to the transaction, use the following code:
    /// LinkedListNode<SaleLineItem> saleItem = ((RetailTransaction)posTransaction).SaleItems.Last;
    /// // To remove the last line use:
    /// ((RetailTransaction)posTransaction).SaleItems.RemoveLast();
    /// </code></example>
    /// </summary>
    [Export(typeof(IItemTrigger))]
    public class ItemTriggers : IItemTrigger
    {
        
        [Import]
        public IApplication Application { get; set; }
        #region Constructor - Destructor
        //add by Yonathan for parm 
        public string itemIdRemove;
        public string activityTrigger;
        public decimal qtyBeforeAdded;
        string taxGroupId ;
        public class parmRequest
        {
            public string company { get; set; }
            public string site { get; set; }
            public string warehouse { get; set; }
            public string itemId { get; set; }
            public string maxQty { get; set; }
            public string barcodeSetupId { get; set; }
            public string configId { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class parmRequestStockSO
        {
            public string ITEMID { get; set; }
            public string DATAAREAID { get; set; }
            public string WAREHOUSE { get; set; }
            public string TRANSACTIONID { get; set; }
            public string QUANTITYAX { get; set; }
            public string QUANTITYINPUT { get; set; }
            public string ORIGIN { get; set; }
            public string RETAILVARIANTID { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class parmResponseStockSO
        {
            public bool error { get; set; }
            public int message_code { get; set; }
            public string message_description { get; set; }
            public string response_data { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Data
        {
            public List<ListstockItem> liststockItemsPFMPOC { get; set; }
        }

        public class ListstockItem
        {
            public string ItemId { get; set; }
            public string ItemBarcode { get; set; }
            public double AvailPhyQty { get; set; }
            public double PhyInventQty { get; set; }
        }

        public class parmResponse
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string messsage { get; set; }
            public Data data { get; set; }
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
        //end add
        public ItemTriggers()
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for ItemTriggers are reserved at 50350 - 50399
        }

        #endregion

        #region IItemTriggersV1 Members

        public void PreSale(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction)
        {
            bool existTaxTable = true;
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreSale", "Prior to the sale of an item...", LSRetailPosis.LogTraceLevel.Trace);


            //check TaxTable - yonathan 17/07/2024
            existTaxTable = checkTaxTable(LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId, Application.Settings.Database.DataAreaID, saleLineItem.ItemId);
            //
            if (existTaxTable == false)
            {
                posTransaction.OperationCancelled = true;
                preTriggerResult.ContinueOperation = false;
                return;
            }
            //last before adding the item.

            

             
            /*string customer = ((RetailTransaction)posTransaction).Customer.CustomerId;

            if (customer.ToString() == "C000004125")
            {
                //add customization by Yonathan 10/04/2023 for validation grabmart order to prevent adding a new item
                if (BlankOperations.BlankOperations.grabMartProperties.grabMartStatus == true)
                {
                    MessageBox.Show("Tidak bisa add item terkait transaksi Grabmart", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    preTriggerResult.ContinueOperation = false;
                }
            }*/
        }

        public void PostSale(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostSale", "After the sale of an item...", LSRetailPosis.LogTraceLevel.Trace);
            activityTrigger = "AddSaleItemPOS";
            string ppnValidation = APIAccess.APIAccessClass.ppnValidation;
            var retailTransaction = posTransaction as RetailTransaction;
            bool foundTaxDiff = false;
            taxGroupId = "";
            int countLoop = 0;
            
            if (APIAccess.APIAccessClass.isB2b == "1" || APIAccess.APIAccessClass.isB2b == "2") //check if customer is either B2B or Canvas 
            {
                foreach (var saleItems in retailTransaction.SaleItems) //loop to check if the item is the same tax group 
                {
                    //taxGroupId = saleItems.TaxGroupId;
                    if (countLoop == 0)
                    {
                        taxGroupId = saleItems.TaxGroupId;
                        countLoop++;
                        continue;
                    }
                    else
                    {
                        //if (taxGroupId != saleItems.TaxGroupId)
                        if(!taxGroupId.Equals(saleItems.TaxGroupId, StringComparison.OrdinalIgnoreCase))
                        {
                            using (frmMessage dialog = new frmMessage(string.Format("Item '{0}' memiliki TaxGroup '{1}'.\nTidak boleh add item dengan TaxGroup selain '{2}'", saleItems.ItemId, saleItems.TaxGroupId, taxGroupId), MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                posTransaction.OperationCancelled = true;
                                return;
                                //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                                //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                            }
                        }
                        else
                        {
                            countLoop++;
                            continue;
                        }
                    }


                }
            }
            
            //recalculate the discount
            //if (retailTransaction.Comment == "PAYMENTDISCOUNT" || retailTransaction.Comment == "PROMOED" || retailTransaction.Comment == "PROMORCPT") //if (transaction.Comment == "PAYMENTDISCOUNT"  || transaction.Comment == "PROMOPDI" || transaction.Comment == "PROMOPDIS")          
            //{ 
                calculatePromoDiscount(retailTransaction); 
            //}
            //posTransaction.OperationCancelled = true;
        }//

        public void PreReturnItem(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreReturnItem", "Prior to entering return state...", LSRetailPosis.LogTraceLevel.Trace);                
        }

        public void PostReturnItem(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostReturnItem", "After entering return state", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PreVoidItem(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, int lineId)
        {

            if (posTransaction.ToString() == "LSRetailPosis.Transaction.CustomerOrderTransaction" && (APIAccess.APIAccessClass.isB2b == "1" || APIAccess.APIAccessClass.isB2b == "2")) //check if customer is either B2B or Canvas 
            {
                string custType = APIAccess.APIAccessClass.isB2b == "1" ? "Canvas" : "B2B";
                using (frmMessage dialog = new frmMessage(string.Format("Tidak bisa Void Product apabila sudah terbentuk Customer Order. Apabila ada item yang ingin ditambahkan, silakan void transaksi ini dan input ulang."), MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    posTransaction.OperationCancelled = true;
                    return;
                    //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                    //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                }
            }
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreVoidItem", "Before voiding an item", LSRetailPosis.LogTraceLevel.Trace);
            /*
            string customer = ((RetailTransaction)posTransaction).Customer.CustomerId;

            if (customer.ToString() == "C000004125")
            {
                //add customization by Yonathan 10/04/2023 for validation grabmart order to prevent adding a new item
                if (BlankOperations.BlankOperations.grabMartProperties.grabMartStatus == true)
                {
                    MessageBox.Show("Tidak bisa void terkait transaksi Grabmart", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    preTriggerResult.ContinueOperation = false;
                }
            }*/
            //
            /*SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {

                string queryString = @"SELECT TOP 1 ITEMID FROM CPEXTVOUCHERCODETMP";

                using (SqlCommand cmd = new SqlCommand(queryString, connectionLocal))
                {

                    if (connectionLocal.State != ConnectionState.Open)
                    {
                        connectionLocal.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string itemID = reader["ITEMID"].ToString();

                            if(itemID == ((RetailTransaction)posTransaction).GetItem(lineId).ItemId)
                            {
                                MessageBox.Show("Unable to void product related to voucher code ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                preTriggerResult.ContinueOperation = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
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
            }*/
        }

        public void PostVoidItem(IPosTransaction posTransaction, int lineId)
        {
            string source = "IItemTriggersV1.PostVoidItem";
            string value = "After voiding an item";
            LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Trace);
            LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);

            var retailTransaction = posTransaction as RetailTransaction;
            //add a code to delete the ppn if the itemline is zero - yonathan 16/07/2024
            if (retailTransaction.SaleItems.Count == 0)
            {
                taxGroupId = "";
            }
            //

            //add to check discount payment from transaction.comment
            //var retailTransaction = posTransaction as RetailTransaction;
            //if (checkDiscPayment(posTransaction) == true)
            //{
            //    Application.Services.Discount.AddTotalDiscountAmount(retailTransaction, 0);
            //    Application.Services.Discount.AddTotalDiscountPercent(retailTransaction, 0);
            //    //application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction); 
            //    Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);

            //    //globalTransaction.CalcTotals();
            //    retailTransaction.Comment = "";
            //    //retailTransaction.CalcTotals();
            //    //POSFormsManager.ShowPOSStatusPanelText("Discount payment removed");
            //    MessageBox.Show("Discount payment removed");
            //    //Application.RunOperation(PosisOperations.DisplayTotal, "");
            //    //retailTransaction.CalcTotals();
            //}


        }

        public void PreSetQty(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreSetQty", "Before setting the qty for an item", LSRetailPosis.LogTraceLevel.Trace);
            int countLines = 0;
            bool isPaymDisc = false;
            bool existTaxTable = true;
            string userIDFromClass = APIAccess.APIAccessClass.userID;

            ////qtyBeforeAdded = posTransaction.
            //var retailTransaction = posTransaction as RetailTransaction;
            ////must fix here to get the real qty of the selected item

            //if(retailTransaction.CalculableSalesLines.Count != 0 && saleLineItem != null)
            //{
            //    foreach (var lines in retailTransaction.CalculableSalesLines)
            //    {
            //        if (retailTransaction.CalculableSalesLines[countLines].ItemId == saleLineItem.ItemId)
            //        {
            //            qtyBeforeAdded += retailTransaction.CalculableSalesLines[countLines].PriceQty;
                        
            //        }
            //        countLines++;
            //    }
                
            //}
            //check TaxTable - yonathan 17/07/2024
            if (saleLineItem != null)
            {
                existTaxTable = checkTaxTable(LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId, Application.Settings.Database.DataAreaID, saleLineItem.ItemId);
            }
            //
            if (existTaxTable == false)
            {
                posTransaction.OperationCancelled = true;
                preTriggerResult.ContinueOperation = false;
                return;
            }
            

        }

        private bool checkTaxTable(string storeId, string dataAreaId, string itemId)
        {
            SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            bool result = false;
            //CHECK THE TAX TABLE DATA
            try
            {
                string queryString = @"SELECT * FROM TAXTABLE JOIN TAXDATA
                            ON TAXTABLE.TAXCODE = TAXDATA.TAXCODE
                            WHERE TAXTABLE.DATAAREAID = @DATAAREAID";

                using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                {

                    command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);


                    if (connectionStore.State != ConnectionState.Open)
                    {
                        connectionStore.Open(); 
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {

                            using (frmMessage dialog = new frmMessage(string.Format("Tidak bisa melanjutkan transaksi karena tidak ada data TAXTABLE/TAXDATA pada toko {0}.\nHubungi Tim IT untuk melakukan Sync Data.", storeId), MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

                                result= false;
                            }
                        }
                        else
                        {
                            result= true;
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
                if (connectionStore.State != ConnectionState.Closed)
                {
                    connectionStore.Close();
                }
            }

            //CHECK THE INVENTITEMMODULE TAX GROUP DATA
            try
            {
                string queryString = @"SELECT ITEMID, MODULETYPE, TAXITEMGROUPID      
                                        FROM  [ax].[INVENTTABLEMODULE]
                                        WHERE MODULETYPE = 2
                                        AND ITEMID = @ITEMID
                                        AND TAXITEMGROUPID = ''";

                using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                {

                    command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);
                    command.Parameters.AddWithValue("@ITEMID", itemId);

                    if (connectionStore.State != ConnectionState.Open)
                    {
                        connectionStore.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            using (frmMessage dialog = new frmMessage(string.Format("Tidak bisa melanjutkan transaksi karena tidak ada data TAXGROUP INVENTTABLEMODULE untuk Item {0} pada toko {1}.\nHubungi Tim IT untuk melakukan Sync Data.",itemId, storeId), MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

                                result=  false;
                            }
                        }
                        else
                        {
                            result=  true;
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
                if (connectionStore.State != ConnectionState.Closed)
                {
                    connectionStore.Close();
                }
            }
            return result;

        }

        private bool checkDiscPayment(IPosTransaction posTransaction)
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            bool isPaymDisc = false;
            try
            {
                string queryString = @"SELECT [PROMOID]  FROM [ax].[CPPROMOCASHBACK] WHERE PROMOID = @PromoId";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@PromoId", retailTransaction.Comment);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                isPaymDisc = true;
                            }
                        }
                    }                     
                }
            }

            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }

            return isPaymDisc;
        }

        public void PostSetQty(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostSetQty", "After setting the qty from an item", LSRetailPosis.LogTraceLevel.Trace);
            string positiveStatus = "";
            string resultItemId = "";
            var retailTransaction = posTransaction as RetailTransaction;

            //add to check discount payment from transaction.comment
            //if (checkDiscPayment(posTransaction) == true)
            //{
            //    Application.Services.Discount.AddTotalDiscountAmount(retailTransaction, 0);
            //    Application.Services.Discount.AddTotalDiscountPercent(retailTransaction, 0);
            //    //application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction); 
            //    Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);

            //    //globalTransaction.CalcTotals();
            //    retailTransaction.Comment = "";
            //    //retailTransaction.CalcTotals();
            //    //POSFormsManager.ShowPOSStatusPanelText("Discount payment removed");
            //    MessageBox.Show("Discount payment removed");
            //    //Application.RunOperation(PosisOperations.DisplayTotal, "");
            //    //retailTransaction.CalcTotals();
            //}


            ////add infocode lines
            //InfoCodeLineItem infocodeLines = new InfoCodeLineItem()
            //{
            //    InfocodeId = "DISCOUNT",
            //    Information = "TESTDSC",

            //    Transaction = retailTransaction,

            //    // Set other properties as needed
            //};
            //transaction.InfoCodeLines.AddLast(infocodeLines);

            if (saleLineItem.Quantity < 0)
            {
                using (frmMessage dialog = new frmMessage("Tidak boleh input quantity negatif", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    posTransaction.OperationCancelled = true;
                    return;
                    //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                    //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                }
            }

            //Disable karena check stock positive terjadi saat pembayaran - Yonathan 19/01/2024
            /*
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                //get the itemID when scanning the item barcode.
                //if (saleLineItem.BarcodeId.ToString() != "")
               // { }
                if (saleLineItem.BarcodeId != "" && saleLineItem.BarcodeId != null)
                {
                    string queryStringBarcode = @"SELECT ITEMBARCODE,ITEMID,DATAAREAID  FROM ax.INVENTITEMBARCODE where ITEMBARCODE=@ITEMBARCODE";
                    using (SqlCommand command = new SqlCommand(queryStringBarcode, connection))
                    {
                        command.Parameters.AddWithValue("@ITEMBARCODE", saleLineItem.BarcodeId);

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();

                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                resultItemId = reader["ITEMID"].ToString();

                            }


                        }
                    }
                }
                else
                {
                    resultItemId = saleLineItem.ItemId;
                }


                //before checking the stock, check first whether this item type is service
                //string queryString = @" SELECT DISPLAYPRODUCTNUMBER,PRODUCTTYPE  FROM ax.ECORESPRODUCT where DISPLAYPRODUCTNUMBER =@ITEMID";
                string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@ITEMID", resultItemId);

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

            if (positiveStatus == "1")
            {
                //checkStock(posTransaction, saleLineItem); 
            }*/

            //if(activityTrigger == "" || activityTrigger == null)
                activityTrigger = "SetQtyItemPOS" ;
           
        }

        public void PrePriceOverride(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PrePriceOverride", "Before overriding the price on an item", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void  PostPriceOverride(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostPriceOverride", "After overriding the price of an item", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

        #region IItemTriggersV2 Members

        public void PreClearQty(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV2.PreClearQty", "Triggered before clear the quantity of an item.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostClearQty(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV2.PostClearQty", "Triggered after clear the quantity of an item.", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion


        #region customization Yonathan for stock positive on hand checking


        public string getFolderPath(string ProcessingDirectory, string typeFolder)
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

        public string checkStockSO(string url, string parmItemId, string parmDataAreaId, string parmWareHouse, string parmTransId, string parmQuantityAx, string parmQuantityInput, string parmOrigin, string configId)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            string result = "";
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", "PFM");
            var pack = new parmRequestStockSO()
            {
                DATAAREAID = parmDataAreaId,
                ITEMID = parmItemId,                
                WAREHOUSE = parmWareHouse,
                TRANSACTIONID = parmTransId,
                QUANTITYAX = parmQuantityAx,
                QUANTITYINPUT = parmQuantityInput,
                ORIGIN = parmOrigin,
                RETAILVARIANTID = configId
            };


            var data = MyJsonConverter.Serialize(pack);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }             
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            

            return result;
        }


        public ReadOnlyCollection<object>  checkStockOnHand(string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "")
        {
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            string itemId, siteId, wareHouse, maxQty, barCode, company = "";
            bool status = false;
            string message = "";
            //object[] array = Array.Empty<object>();


            ReadOnlyCollection<object> containerArray = new ReadOnlyCollection<object>(new object[0]);

            try
            {
                object[] parameterList = new object[] 
							{
								url,
								parmSiteId,
								parmWareHouse,
								parmItemId,
								"",
								"",
								
							};

                
                 containerArray = Application.TransactionServices.InvokeExtension("getStockOnHand", parameterList);

                

                return containerArray;
                //txtBox.AppendText("Time elapsed : " + timeTaken + "\r\n");

                ////txtBox.AppendText("Service reference : " + url + "\r\n");
                //txtBox.AppendText("Item ID : " + containerArray[2] + "\r\n");
                //txtBox.AppendText("Barcode : " + containerArray[3] + "\r\n");
                //txtBox.AppendText("Avail Phy QTY : " + containerArray[4] + "\r\n");
                //txtBox.AppendText("Phy Invent Qty : " + containerArray[5] + "\r\n" + "\r\n");

                //Console.ReadLine();

                //containerArray = Application.TransactionServices.InvokeExtension("getStockOnHandList", "SvcRefGetStockOnhandPFMPOC");
                /*returnValue = InternalApplication.TransactionServices.Invoke(
                 "GetCustomerBalance",
                 new object[] { customerId, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreCurrency, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId }
                 );*/
                //status = (bool)containerArray[1];
                // message = containerArray[2].ToString();
                //tbName.Text = containerArray[4].ToString();
                //tbPoin.Text = containerArray[5].ToString();

            }
            catch (Exception ex)
            {
                return containerArray;
                //MessageBox.Show(ex.Message);
               
                
            }


            
            /*
            switch (url)
            {
                case "SvcRefGetStockOnhandPFMPOC":
				   
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUri);  

                    var cc = new CPGetStockOnHandGroupPFMPOC.CallContext();// { Company = "6000", Language = "EN-US", LogonAsUser = @"MAHADASHA\business.system" };

                    using (CPGetStockOnHandGroupPFMPOC.CP_GetStockOnHandClient client1 = new  CPGetStockOnHandGroupPFMPOC.CP_GetStockOnHandClient())
                    {
                        //callContext.Company
                        //ServiceReference1.CallContext cc = new ServiceReference1.CallContext();
                        //cc.Company = parmCompanyCode;
                        cc.Company = parmCompanyCode;
                        CP_GetStockOnHandParm GetStockOnHandParm_ = new CP_GetStockOnHandParm();
                        GetStockOnHandParm_.parmItemId = parmItemId;
                        GetStockOnHandParm_.parmSiteId = parmSiteId;
                        GetStockOnHandParm_.parmWarehouse = parmWareHouse;
                        GetStockOnHandParm_.parmMaxQtyCheck = "";
                        GetStockOnHandParm_.parmBarcodeSetupId = "";
                        // Process the result
                        //Console.WriteLine(result);
                        timer.Start();
                        CP_GetStockOnHandResponse[] str = client1.getStockOnHandList(cc, GetStockOnHandParm_);
                        timer.Stop();
                        TimeSpan timeTaken = timer.Elapsed;
                        txtBox.AppendText("Time elapsed : " + timeTaken + "\r\n");
                        foreach (var a in str)
                        {
                            //txtBox.AppendText("Service reference : " + url + "\r\n");
                            txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
                            txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
                            txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
                            txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

                            //Console.ReadLine();
                        }
                    }



                    break;

                case "SvcRefGetStockOnhandPFM12":
                    var cc2 = new CPGetStockOnHandGroupPFMJKT12.CallContext();
                    using (CPGetStockOnHandSvcPFMJKT12Client client2 = new CPGetStockOnHandSvcPFMJKT12Client())
                    //using (var client2 = new ServiceReference2.CPGetStockOnHandSvcPFMJKT12Client())
                    {
                        //ServiceReference2.CallContext cc2 = new ServiceReference2.CallContext();
                        //cc2.Company = parmCompanyCode;
                        cc2.Company = parmCompanyCode;

                        CP_GetStockOnHandParmPFMJKT12 GetStockOnHandParm2_ = new CP_GetStockOnHandParmPFMJKT12();
                        GetStockOnHandParm2_.parmItemId = parmItemId;
                        GetStockOnHandParm2_.parmSiteId = parmSiteId;
                        GetStockOnHandParm2_.parmWarehouse = parmWareHouse;
                        GetStockOnHandParm2_.parmMaxQtyCheck = "";
                        GetStockOnHandParm2_.parmBarcodeSetupId = "";


                        timer.Start();
                        CP_GetStockOnHandResponsePFMJKT12[] str2 = client2.getStockOnHandList(cc2, GetStockOnHandParm2_);
                        timer.Stop();
                        TimeSpan timeTaken2 = timer.Elapsed;
                        txtBox.AppendText("Time elapsed : " + timeTaken2 + "\r\n");
                        foreach (var a in str2)
                        {

                            txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
                            txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
                            txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
                            txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

                            //Console.ReadLine();
                        }
                    }

                    break;*/



        }

        public string checkStockOnHandOld(string url, string parmCompanyCode, string parmSiteId, string parmWareHouse, string parmItemId, string parmMaxQty, string parmBarcodeSetupId, string parmConfigId)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            string result = "";
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";

            var pack = new parmRequest()
            {
                company = parmCompanyCode,
                site = parmSiteId,
                warehouse = parmWareHouse,
                itemId = parmItemId,
                maxQty = parmMaxQty,
                barcodeSetupId = parmBarcodeSetupId,
                configId = parmConfigId
            };


            var data = MyJsonConverter.Serialize(pack);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
                        

            return result;
        }

        /*
         public IHttpActionResult getStock(WebAPIModels model)
        {
            OutputModels.outputMessage outMsg1 = new OutputModels.outputMessage();

            try
            {
                OutputModels.bracket_list_stock_item liststockItem = new OutputModels.bracket_list_stock_item();

                liststockItem.liststockItems = new List<Models.OutputModels.liststockItem>();

                SvcRefGetStockOnHand.CallContext cc = new SvcRefGetStockOnHand.CallContext();
                cc.Company = model.company;

                SvcRefGetStockOnHand.CP_GetStockOnHandClient client = new SvcRefGetStockOnHand.CP_GetStockOnHandClient();

                SvcRefGetStockOnHand.CP_GetStockOnHandParm GetStockOnHandParm_ = new SvcRefGetStockOnHand.CP_GetStockOnHandParm();
                GetStockOnHandParm_.parmItemId = model.itemId;
                GetStockOnHandParm_.parmSiteId = model.site;
                GetStockOnHandParm_.parmWarehouse = model.warehouse;
                GetStockOnHandParm_.parmMaxQtyCheck = model.maxQty;
                GetStockOnHandParm_.parmBarcodeSetupId = model.barcodeSetupId;

                SvcRefGetStockOnHand.CP_GetStockOnHandResponse[] str = client.getStockOnHandList(cc, GetStockOnHandParm_);

                foreach (var a in str)
                {
                    OutputModels.liststockItem tmp = new OutputModels.liststockItem();
                    tmp.ItemId = a.parmItemId;
                    tmp.ItemBarcode = a.parmItemBarcode;
                    tmp.AvailPhyQty = a.parmAvailPhyQty;
                    tmp.PhyInventQty = a.parmPhyInventQty;
                    liststockItem.liststockItems.Add(tmp);
                }

                outMsg1.status = Convert.ToInt32(HttpStatusCode.OK);
                outMsg1.messsage = "Success";
                outMsg1.data = liststockItem;

                return Ok(outMsg1);
            }
            catch (Exception ee)
            {
                outMsg1.status = Convert.ToInt32(HttpStatusCode.Forbidden);
                outMsg1.error = true;
                outMsg1.messsage = ee.Message;

                return Content(HttpStatusCode.OK, outMsg1);
            }
        }
         
         */

        public void checkStock(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            string companyCode = Application.Settings.Database.DataAreaID;
            string siteId = "";
            string warehouseId = "";
            string configId="";
            decimal availQtyStock = 0;
            decimal availQtyStockSO = 0;
            decimal availQty = 0;
            decimal remainQty = 0;
            //decimal qtyOrdered = 0;
            string qtyOrderedConverted = "";
            var url = "";// "http://10.1.5.18/AXAIF/AIF/getStockOnHand";
            var url2 = "";// "http://10.204.3.174:80/api/stockOnHand/getItem";
            var url3 = "";
            //var result = "";
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");

            //url = getFolderPath(PathDirectory, "urlAXAIF") + "AXAIF/AIF/getStockOnHand"; //10.1.5.18/AXAIF/AIF/getStockOnhandPFMPOC
            //url =  getFolderPath(PathDirectory, "urlAXAIF") + "AXAIF/AIF/getStockOnhandPFMPOC"; 
            //url2 = getFolderPath(PathDirectory, "urlAPI") + "api/stockOnHand/getItem";
            string functionName = "GetStockAX%"; // "GetStockAXPFMPOC"; //change to GetStockAX
            string functionName2 = "GetItemAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            //url3 = APIClass.getSvcRefByName(functionName);
            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            url2 = APIClass.getURLAPIByFuncName(functionName2);

            if (url2 == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName2));
            }

            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpRequest.Method = "POST";

            //httpRequest.ContentType = "application/json";
            //get all the needed parameters
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

                //string queryString2 = @"SELECT ID.INVENTDIMID, ITEMID, CONFIGID FROM INVENTDIM ID JOIN INVENTITEMBARCODE IB ON ID.INVENTDIMID = IB.INVENTDIMID
                //                         WHERE ITEMID = @ITEMID";

                string queryString2 = @"SELECT ITEMID, RETAILVARIANTID FROM [ax].[INVENTITEMBARCODE]
                                         WHERE ITEMID =  @ITEMID";
                using (SqlCommand command = new SqlCommand(queryString2, connection))
                {
                    command.Parameters.AddWithValue("@ITEMID", saleLineItem.ItemId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //configId = reader["CONFIGID"].ToString();
                            configId = reader["RETAILVARIANTID"].ToString();
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

            if (url.StartsWith("http"))
            {
                var resultCheckStock = checkStockOnHandOld(url, companyCode, siteId, warehouseId, saleLineItem.ItemId, "", "", configId);
                parmResponse responseCheckStock = MyJsonConverter.Deserialize<parmResponse>(resultCheckStock);
                if (responseCheckStock.status.ToString() == "403") // || responseCheckStockSO.error != true)
                //if (conResult.Count == 1)
                {


                    using (frmMessage dialog = new frmMessage("Terjadi gangguan koneksi, tidak bisa menjual item ini", MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                        posTransaction.OperationCancelled = true;
                        return;
                        //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                        //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                    } 
                }
                else
                {
                    if (responseCheckStock.data.liststockItemsPFMPOC.Count != 0)
                    //if (conResult[5] != "0")
                    {
                        availQtyStock = Convert.ToDecimal( responseCheckStock.data.liststockItemsPFMPOC[0].AvailPhyQty);
                        //availQtyStock = Convert.ToDecimal(conResult[5]);
                    }
                    else
                    {
                        using (frmMessage dialog = new frmMessage("Tidak ada stock di sistem, \nSilakan Receive PO/TO terlebih dahulu\nItemid: " + saleLineItem.ItemId + " - " + saleLineItem.Description, MessageBoxButtons.OK, MessageBoxIcon.Error))
                        {
                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                            itemIdRemove = saleLineItem.ItemId;
                            posTransaction.OperationCancelled = true;
                            return;

                        }
                    }
                    availQtyStock = Convert.ToDecimal(responseCheckStock.data.liststockItemsPFMPOC.Count != 0 ? responseCheckStock.data.liststockItemsPFMPOC[0].AvailPhyQty : 0);//responseCheckStock.data.liststockItems[0].AvailPhyQty);
                    //availQtyStock = Convert.ToDecimal(conResult[5] != "" ? conResult[5] : 0);
                }
            }
            else
            {
                ReadOnlyCollection<object> conResult = checkStockOnHand(url, companyCode, siteId, warehouseId, saleLineItem.ItemId, "", "", configId);
                if (conResult.Count == 1)
                {


                    using (frmMessage dialog = new frmMessage("Terjadi gangguan koneksi, tidak bisa menjual item ini", MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                        posTransaction.OperationCancelled = true;
                        return;
                        //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                        //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                    }  
                } 
                else
                {
                    //if (responseCheckStock.data.liststockItemsPFMPOC.Count != 0)
                    if (conResult[5] != "0")
                    {
                        //availQtyStock = Convert.ToDecimal( responseCheckStock.data.liststockItemsPFMPOC[0].AvailPhyQty);
                        availQtyStock = Convert.ToDecimal(conResult[5]);
                    }
                    else
                    {
                        using (frmMessage dialog = new frmMessage("Tidak ada stock di sistem, \nSilakan Receive PO/TO terlebih dahulu\nItemid: " + saleLineItem.ItemId + " - " + saleLineItem.Description, MessageBoxButtons.OK, MessageBoxIcon.Error))
                        {
                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                            itemIdRemove = saleLineItem.ItemId;
                            posTransaction.OperationCancelled = true;
                            return;

                        }
                    }
                    //availQtyStock = Convert.ToDecimal(responseCheckStock.data.liststockItemsPFMPOC.Count != 0 ? responseCheckStock.data.liststockItemsPFMPOC[0].AvailPhyQty : 0);//responseCheckStock.data.liststockItems[0].AvailPhyQty);
                    availQtyStock = Convert.ToDecimal(conResult[5] != "" ? conResult[5] : 0);
                }
            }
            //var resultCheckStock = checkStockOnHand(url, companyCode, siteId, warehouseId, saleLineItem.ItemId, "", "", configId);
            

            //parmResponse responseCheckStock = MyJsonConverter.Deserialize<parmResponse>(resultCheckStock);
            //parmResponse responseCheckStock = new parmResponse()
            //{
            //    status = conResult.Count > 0 ? 0 : 403,
            //    messsage = conResult[1],
            //    data = conResult[4] !="0" ?
                
 
            //}

            
            //if (responseCheckStock.status.ToString() == "403") // || responseCheckStockSO.error != true)
            
            //MessageBox.Show(String.Format("AIF Config Id : {0}, Available Qty {1}" , configId.ToString(),availQtyStock));
            //check stock from mysql table
            qtyOrderedConverted =   saleLineItem.QuantityOrdered.ToString().Replace(",",".");
            var resultCheckStockSO = checkStockSO(url2, saleLineItem.ItemId, companyCode, warehouseId, posTransaction.TransactionId, availQtyStock.ToString().Replace(",", "."), qtyOrderedConverted, activityTrigger, configId);// retailTransaction.LastRunOperation.ToString());
            parmResponseStockSO responseCheckStockSO = MyJsonConverter.Deserialize<parmResponseStockSO>(resultCheckStockSO);
            availQtyStockSO = decimal.Parse(responseCheckStockSO.response_data,CultureInfo.InvariantCulture);
            //MessageBox.Show(String.Format("API Config Id : {0}, Available Qty {1}", configId.ToString(), availQtyStockSO));
            //compare the available stock with sales transaction
            availQty = availQtyStock + availQtyStockSO; //
            //MessageBox.Show(String.Format("Config Id : {0}, ({1} + {2}) = {3}" , configId.ToString(), availQtyStock, availQtyStockSO, availQty));
            remainQty = availQty - (saleLineItem.QuantityOrdered + qtyBeforeAdded);
            //qtyOrdered = 0;
            //check if remains zero


            if (remainQty < 0) 
            {
                using (frmMessage dialog = new frmMessage("ItemId: " + saleLineItem.ItemId + " - " + saleLineItem.Description + "\nTidak Bisa Dijual Karena melebihi Qty Stock\nQty Tersedia: " + (Math.Truncate(Convert.ToDecimal(availQty) * 1000m) / 1000m), MessageBoxButtons.OK, MessageBoxIcon.Error))  //String.Format(availQty % 1 == 0 ? "{0:0}" : "{0:0.00}", availQty), MessageBoxButtons.OK, MessageBoxIcon.Error)) 
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    itemIdRemove = saleLineItem.ItemId;
                    posTransaction.OperationCancelled = true;
                    qtyBeforeAdded = 0;
                    return;
                }
                               
            }
            else
            {
                //MessageBox.Show("You can sale this item", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            qtyBeforeAdded = 0;

        }
        #endregion  



        private void calculatePromoDiscount(RetailTransaction transaction)
        {

            var matchingLines = transaction.CalculableSalesLines.Where(line => line.Comment != "").ToList();
            decimal pctDisc = 0;
            decimal amtDisc = 0;
            string _promoName = "";
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;
            foreach (var line in matchingLines)
            {
                pctDisc = 0;
                amtDisc = 0;
                _promoName = "";
                line.IsInfoCodeItem = false;
                LSRetailPosis.Transaction.Line.Discount.LineDiscountItem lineDisc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                selectPromoItem(line.ItemId, line.Comment, out pctDisc, out amtDisc, out fromDate, out toDate, out _promoName);
                if (_promoName == "")
                {
                    selectPromoItemReceipt(line.ItemId, line.Comment, out pctDisc, out amtDisc, out fromDate, out toDate, out _promoName);
                }

                //check if this line already has a discount with the same promo ID

                foreach (var discountLine in line.DiscountLines.ToList())
                {
                    line.DiscountLines.Remove(discountLine);
                }

                var existingDiscount = line.DiscountLines
                                        .OfType<PeriodicDiscountItem>()
                                        .FirstOrDefault(d => d.OfferId == line.Comment);

                if (existingDiscount == null)
                {
                    DiscountItem discItem = new PeriodicDiscountItem();
                    if (pctDisc == 0)
                    {
                        discItem.Amount = amtDisc;
                    }
                    else if (amtDisc == 0)
                    {
                        discItem.Percentage = pctDisc;
                    }

                    PeriodicDiscountItem periodDiscItem = discItem as PeriodicDiscountItem;
                    periodDiscItem.OfferId = line.Comment;
                    periodDiscItem.OfferName = _promoName;
                    periodDiscItem.QuantityDiscounted = 1;
                    periodDiscItem.BeginDateTime = fromDate;
                    periodDiscItem.EndDateTime = toDate;

                    line.DiscountLines.AddFirst(discItem);


                }


                //DiscountItem discItem = (DiscountItem)new PeriodicDiscountItem();
                //if (pctDisc == 0)
                //{
                //    discItem.Amount = amtDisc;
                //}
                //else if (amtDisc == 0)
                //{
                //    discItem.Percentage = pctDisc;
                //}


                //PeriodicDiscountItem periodDiscItem = discItem as PeriodicDiscountItem;
                //periodDiscItem.OfferId = _promoId;
                //periodDiscItem.OfferName = _promoName;
                //periodDiscItem.QuantityDiscounted = 1;
                //periodDiscItem.BeginDateTime = fromDate; //Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["From Date"].Value.ToString());
                //periodDiscItem.EndDateTime = toDate; //Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["To Date"].Value.ToString());


                //line.DiscountLines.AddFirst(discItem);
            }

            //PeriodicDiscountItem periodDiscItem = discItem as PeriodicDiscountItem;
            //periodDiscItem.OfferId = dataGridResult.Rows[e.RowIndex].Cells["Promo ID"].Value.ToString();
            //periodDiscItem.OfferName = dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString();
            //periodDiscItem.QuantityDiscounted = 1;
            //periodDiscItem.BeginDateTime = Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["From Date"].Value.ToString());
            //periodDiscItem.EndDateTime = Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["To Date"].Value.ToString());

            //transaction.CurrentSaleLineItem.DiscountLines.AddFirst(discItem);

        }

        private void selectPromoItem(string itemId, string promoId, out decimal pctDisc, out decimal amtDisc, out DateTime fromDate, out DateTime toDate, out string promoName)
        {
            amtDisc = 0;
            pctDisc = 0;
            promoName = "";
            fromDate = DateTime.Now;
            toDate = DateTime.Now;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = "";
                queryString = @"SELECT LINES.[PROMOID]       
							      ,[ITEMID]     
							      ,[DISCAMOUNT]
							      ,[DISCPERCENTAGE]      
							      ,[RETAILSTOREID]
							      ,[FROMDATE]
							      ,[TODATE]
                                  ,[DESCRIPTION]   
						      FROM [ax].[CPPROMOEDQTYDETAIL] LINES
						      LEFT JOIN [AX].[CPPROMOEDQTY] HEADER
						      ON LINES.PROMOID =  HEADER.PROMOID
						      WHERE RETAILSTOREID = @STOREID
						      AND LINES.PROMOID = @PROMOID
						      AND LINES.ITEMID = @ITEMID";

                //SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@STOREID", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);
                    command.Parameters.AddWithValue("@PROMOID", promoId);
                    command.Parameters.AddWithValue("@ITEMID", itemId);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                pctDisc = Convert.ToDecimal(reader["DISCPERCENTAGE"]);
                                amtDisc = Convert.ToDecimal(reader["DISCAMOUNT"]);
                                fromDate = Convert.ToDateTime(reader["FROMDATE"].ToString());
                                toDate = Convert.ToDateTime(reader["TODATE"].ToString());
                                promoName = reader["DESCRIPTION"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
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
        private void selectPromoItemReceipt(string itemId, string promoId, out decimal pctDisc, out decimal amtDisc, out DateTime fromDate, out DateTime toDate, out string promoName)
        {
            amtDisc = 0;
            pctDisc = 0;
            promoName = "";
            fromDate = DateTime.Now;
            toDate = DateTime.Now;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = "";
                queryString = @"SELECT LINES.[PROMOID]       
							  ,[ITEMID]     
							  ,[DISCAMOUNT]
							  ,[DISCPERCENTAGE]      
							  ,[RETAILSTOREID]
							  ,[FROMDATE]
							  ,[TODATE]
                              ,[DESCRIPTION]
						  FROM [ax].[CPPROMOEDQTYPERSTRUKDETAIL] LINES
						  LEFT JOIN [AX].[CPPROMOEDQTYPERSTRUK] HEADER
						  ON LINES.PROMOID =  HEADER.PROMOID
						  WHERE RETAILSTOREID = @STOREID
						  AND LINES.PROMOID = @PROMOID
						  AND LINES.ITEMID = @ITEMID";


                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@STOREID", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);
                    command.Parameters.AddWithValue("@PROMOID", promoId);
                    command.Parameters.AddWithValue("@ITEMID", itemId);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                pctDisc = Convert.ToDecimal(reader["DISCPERCENTAGE"]);
                                amtDisc = Convert.ToDecimal(reader["DISCAMOUNT"]);
                                fromDate = Convert.ToDateTime(reader["FROMDATE"].ToString());
                                toDate = Convert.ToDateTime(reader["TODATE"].ToString());
                                promoName = reader["DESCRIPTION"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
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
