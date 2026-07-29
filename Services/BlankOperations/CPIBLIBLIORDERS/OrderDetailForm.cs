using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CPIBLIBLIORDERS
{
    public partial class OrderDetailForm : Form
    {
        List<APIAccess.APIParameter.OrderData> orderList;
        string packageId;
        public event EventHandler DataProcessed;
        int exponent = 0;
        int findFalse;
        IPosTransaction posTransaction;
        IApplication application;
        decimal totalAdjustment = 0m;      
        public OrderDetailForm(string orderId, string status, List<APIAccess.APIParameter.OrderData> _orderList, IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent();
            
            posTransaction = _posTransaction;
            // Set label info
            lblOrderNo.Text = orderId;
            lblStatus.Text = status;
            orderList = _orderList;
            bool isFirstIteration = true;
            decimal priceAfterExponent = 0;
            string priceAfterExponentString = "";
            decimal discAfterExponent = 0;
            string discAfterExponentString = "";
            bool isAvail = true;
            string isAvailable = "Ya";
            decimal subTotal = 0;
            decimal grandTotal = 0;
            string siteId = "";
            string warehouseId = "";
            string itemIdMulti = "";
            //string qtyMulti = "";
            string barcodeMulti = "";
            string configIdMulti = "";
            string qtyMulti = ""; //add Qty by Yonathan 11092024
            string xmlResponse;
            string itemName = "";
            bool noNameDetected = false;
            application = _application;
            btnCancel.Visible = false ;                                                                           
            // Setup DataGridView untuk produk
            //dgvOrderItems.Columns.Clear();
            //dgvOrderItems.Columns.Add("itemid", "ItemId");
            //dgvOrderItems.Columns.Add("product", "Product");
            //dgvOrderItems.Columns.Add("qty", "Qty");
            //dgvOrderItems.Columns.Add("price", "Price");
            //dgvOrderItems.Columns.Add("discount", "Discount");
            //dgvOrderItems.Columns.Add("stock", "Stock");
            //dgvOrderItems.Columns.Add("subtotal", "Subtotal");

            decimal total = 0m;




            //check stock
            string functionNameAX = "GetStockAX%"; // "GetStockAXPFMPOC"; //change to GetStockAX
            string functionNameAPI = "GetItemAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            string urlRTS = APIClass.getURLAPIByFuncName(functionNameAX);
            string urlAPI = APIClass.getURLAPIByFuncName(functionNameAPI);

            APIAccess.APIFunction apiFunction = new APIAccess.APIFunction();

 
            //itemDetails = item;

            int indexRow = 0;
            foreach (var orderItem in orderList)
            {
                //check positive stock first
                if (orderItem.product == null)
                    continue;

                
                APIAccess.APIAccessClass.blibliCustName = orderItem.recipient.name;
            }
            
            //APIAccess.APIAccessClass.blibliCustName = item.receiver.name;
            //APIAccess.APIAccessClass.blibliCustPhone = item.receiver.phones;
            APIAccess.APIAccessClass.blibliOrderIdLong = orderId;
            APIAccess.APIAccessClass.blibliOrderState = status;
            

            //foreach (var order in orderList)
            //{
            //    if (order.product == null)
            //        continue;

            //    foreach (var item in order.product)
            //    {

            //    }
            //}


            foreach (var orderItem in orderList)
            {
                //check positive stock first
                if (orderItem.product == null)
                    continue;

                foreach (var item in orderItem.product)
                {

                    if (APIAccess.APIFunction.checkPositiveStatus(item.sellerSku, LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection) == true)
                    {
                        if (isFirstIteration)
                        {


                            APIAccess.APIAccessClass.merchantId = orderItem.store_code;
                            isFirstIteration = false;
                        }

                        //loop through the items in the cart
                        itemIdMulti += item.sellerSku;
                        qtyMulti += item.quantity; //add Qty by Yonathan 11092024
                     

                        // Add the separator (;) if it's not the last item
                        if (item.sellerSku != orderItem.product[orderItem.product.Count - 1].sellerSku)
                        {
                            itemIdMulti += ";";
                            qtyMulti += ";"; 
                        }
                    }
                }




            }

            //LSRetailPosis.Settings.ApplicationSettings.Terminal.InventLocationId
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
            XmlNodeList itemNodes = null;
            if (itemIdMulti != "")
            {
                result = apiFunction.checkStockOnHandMultiNew(_application, urlRTS, _application.Settings.Database.DataAreaID, siteId, ApplicationSettings.Terminal.InventLocationId, itemIdMulti, "", "", "", qtyMulti, posTransaction.StoreId + "-" + orderId.ToString()); // mod by Yonathan to add 2 parameters qty and trans id 11092024
                xmlResponse = result[3].ToString();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlResponse);

                itemNodes = xmlDoc.SelectNodes("//StockListResult");
            }


          

            //if (item.campaigns != null)
            //{
            //    foreach (var campaign in item.campaigns)
            //    {

            //        var applicableItems = item.items.Where(i => campaign.appliedItemIDs.Contains(i.id)).ToList();

            //        int totalQty = applicableItems.Sum(i => i.quantity);

            //        if (totalQty > 0)
            //        {
            //            foreach (var orderItem in applicableItems)
            //            {
            //                decimal itemDiscount = (orderItem.quantity / (decimal)totalQty) * campaign.deductedAmount;
            //                orderItem.discAmt += itemDiscount / orderItem.quantity;
            //            }
            //        }
            //    }
            //}

            //end check stock
            string itemType = "Stock";
            foreach (var order in orderList)
            {
                if (order.product == null)
                    continue;

                foreach (var item in order.product)
                {

                    decimal remainQty = 0;//availQty;// -(orderItem.quantity);// + qtyBeforeAdded);
                    string itemId = "";
                    bool found = false;
                    //remainQty = Convert.ToDecimal(itemNodes[indexRow].Attributes["QtyAvail"].Value);
                    if (itemNodes != null)
                    {
                        foreach (XmlNode node in itemNodes)
                        {
                            if (node.Attributes["ItemId"].Value == item.sellerSku)
                            {
                                remainQty = Convert.ToDecimal(node.Attributes["QtyAvail"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                                //remainQty = decimal.Parse(node.Attributes["QtyAvail"].Value, NumberStyles.Number, CultureInfo.CurrentCulture); //.Replace(",", ".")
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

                    var firstAdjustment = order.adjustment.FirstOrDefault();

                    

                    if (order.adjustment != null)
                    {
                        totalAdjustment = order.adjustment
                            .Where(adjList => adjList != null)        
                            .SelectMany(adjList => adjList)           
                            .Where(a => a != null)                   
                            .Sum(a => a.amount);                    
                    }

                    totalAdjustment = Math.Abs(totalAdjustment);
                    priceAfterExponent = item.itemPrice / (decimal)Math.Pow(10, exponent);


                    //if (orderItem.discAmt != null && orderItem.discAmt != 0)
                    //discAfterExponent = item.pr.discAmt / (decimal)Math.Pow(10, exponent);

                    //discAfterExponentString = orderItem.discAmt.ToString().Substring(0, orderItem.discAmt.ToString().Length - exponent);
                    //decimal.TryParse(discAfterExponentString, out discAfterExponent);

                    subTotal = (priceAfterExponent * item.quantity);
                    grandTotal += subTotal - ( item.quantity); //(orderItem.discAmt*orderItem.quantity);


                    //if(itemNodes != null)
                    if (found == true) //found in XML
                    {
                        isAvailable = remainQty - item.quantity < 0 ? "Tidak" : "Ya";
                        itemType = "Stock";
                    }
                    else //not Found in XML -> Non Stock
                    {
                        itemType = "Non";
                        isAvailable = "Ya";
                    }
                    //add to row

                    decimal subtotal = (item.itemPrice * item.quantity);  // minus discount per line - Math.Abs(totalAdjustment); //(firstAdjustment.amount);
                    total += subtotal;

                    dgvOrderItems.Rows.Add(
                        item.sellerSku,
                        item.name,
                        item.quantity,
                        string.Format("Rp {0:N0}", item.itemPrice),
                        string.Format("Rp {0:N0}", 0), //discount per line per item,
                        isAvailable,
                        string.Format("Rp {0:N0}", subtotal)
                    );




                    if (item.name == "")
                    {
                        findFalse = 2;
                    }
               

                    //itemNodes[indexRow].Attributes["ItemId"].Value;
                    DataGridViewRow row = dgvOrderItems.Rows[indexRow];
                    DataGridViewCell availableCell = row.Cells["Stock"];
                    indexRow++;



                    if (isAvailable == "Tidak")//Stock
                    {
                        findFalse = 1;
                        availableCell.Style.ForeColor = Color.Red;
                        //this.Stock.DefaultCellStyle.ForeColor = Color.Red;
                        //this.Stock.DefaultCellStyle.Font = new Font(itemDetailsGrid.Font, FontStyle.Bold);




                    }
                    else if (isAvailable == "Ya")
                    {
                        availableCell.Style.ForeColor = Color.Green;
                        //this.Stock.DefaultCellStyle.ForeColor = Color.Green;
                        //this.Stock.DefaultCellStyle.Font = new Font(itemDetailsGrid.Font, FontStyle.Bold);
                    }
                }
            }
            //totalAdjustment = 5000;

            lblSubtotal.Text = string.Format("Subtotal: Rp {0:N0}", total);
            total = total - Math.Abs(totalAdjustment);
            lblVoucher.Text = string.Format("Potongan: Rp {0:N0}", totalAdjustment); 
            lblTotal.Text = string.Format("Total: Rp {0:N0}", total);
            this.Load += OrderDetailForm_Load;
            //Check status to change the button label of process/ready to deliver

            //end
        }

        private void OrderDetailForm_Load(object sender, EventArgs e)
        {
              if (string.Equals(lblStatus.Text.ToString(), "Pesanan siap dikirim", StringComparison.OrdinalIgnoreCase) )
              {
                  btnProcess.Text = "Finalisasi Pesanan";
                  btnCancel.Visible = false;
              }                  
              else if(string.Equals(lblStatus.Text.ToString(), "Pesanan dalam pengiriman", StringComparison.OrdinalIgnoreCase)  )
              {
                  btnProcess.Text = "Finalisasi Pesanan";
                  btnCancel.Visible = false;
              }
              else if (string.Equals(lblStatus.Text.ToString(), "Pesanan terkirim", StringComparison.OrdinalIgnoreCase))
              {
                  btnProcess.Text = "Finalisasi Pesanan";
                  btnCancel.Visible = false;
              }
              else if (string.Equals(lblStatus.Text.ToString(), "Pesanan dibatalkan", StringComparison.OrdinalIgnoreCase))
              {
                  btnProcess.Visible = false;
                  btnCancel.Visible = false;
              }

             
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
        
        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            
        }

        private void btnProcessOrder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Order diproses!");
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            IApplication applicationLoc = PosApplication.Instance as IApplication;
            //update to PF (Create Package)
            APIAccess.APIParameter.Receiver receiverParm;
            string functionName = "UpdateStatusTransBlibli";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            string url = APIClass.getURLAPIByFuncName(functionName);
            bool error, error2 = false;
            IPosTransaction suspendTrans = null;
            if (btnProcess.Text == "Finalisasi Pesanan")
            { 
                 DialogResult results = MessageBox.Show(
                "Proses pesanan jadi struk?\nPastikan driver sudah mengambil pesanan.",  
                "Konfirmasi",                        
                MessageBoxButtons.YesNo,              
                MessageBoxIcon.Question               
                );

             
                 if (results == DialogResult.Yes)
                 {
                   
                     string suspendedTransId = string.Empty;

                     using (SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection)
                     {
                         if (connection.State != ConnectionState.Open)
                             connection.Open();

                         string query = @"SELECT TRANSACTIONID 
                     FROM [crt].[SALESTRANSACTION]
                     WHERE COMMENT = @Comment";

                         using (SqlCommand command = new SqlCommand(query, connection))
                         {
                             command.Parameters.AddWithValue("@Comment", lblOrderNo.Text);

                             object result = command.ExecuteScalar();

                             if (result != null && result != DBNull.Value)
                             {
                                 suspendedTransId = result.ToString();
                             }
                         }
                     }
                     //if exist, then recall
                     if (suspendedTransId != "")
                     {

                         applicationLoc.RunOperation(PosisOperations.RecallTransaction, suspendedTransId);//, posTransaction);

                         RetailTransaction transaction = APIAccess.APIAccessClass.posTransaction as RetailTransaction;
                         //transaction.CalcTotals();
                         //applydiscount voucher blibli (if any)
                         if (totalAdjustment != 0)
                         {
                             //transaction.SetLoyaltyDiscAmount()
                             transaction.SetTotalDiscAmount(totalAdjustment);
                             transaction.Comment = "BliBliDiscount";
                             try
                             {
                                 PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                                 PosApplication.Instance.Triggers.Invoke<IDiscountTrigger>((Action<IDiscountTrigger>)(t => t.PostTotalDiscountAmount(APIAccess.APIAccessClass.posTransaction)));
                                 //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 

                             }
                             catch (Exception ex)
                             {
                                 LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                             }

                             application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);

                             //application.Services.Tax.CalculateTax( transaction);


                             transaction.CalcTotals();
                             //end

                         }
                        
                         foreach (var itemSale in transaction.SaleItems)
                         {
                             IApplication applicationLocal = PosApplication.Instance as IApplication;

                             var foundItem = orderList
                                            .Where(o => o.product != null)
                                            .SelectMany(o => o.product)
                                            .FirstOrDefault(p => p.sellerSku == itemSale.ItemId);


                             itemSale.CustomerPrice = foundItem.itemPrice;
                             itemSale.GrossAmount = foundItem.itemPrice;
                             itemSale.OriginalPrice = foundItem.itemPrice;
                             itemSale.Price = foundItem.itemPrice;
                             //itemSale.TaxAmount
                             //salesLine.TradeAgreementPriceGroup = result[1];
                             itemSale.TradeAgreementPrice = foundItem.itemPrice;

                             itemSale.ClearPeriodicDiscounts();
                             itemSale.ClearCustomerDiscountLines(true);

                             //LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem custDiscountManual = new LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem();

                             //custDiscountManual.Amount = 0; 

                             //applicationLoc.Services.Discount.AddDiscountLine(itemSale, custDiscountManual);

                             applicationLoc.Services.Tax.CalculateTax(itemSale, transaction);

 
                         }
                         //applicationLoc.Services.Discount.AddTotalDiscountAmount(transaction, 10000);
                         ////applicationLoc.Services.Discount.AddTotalDiscountPercent(retailTransaction, 0);
                         ////application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction);
                         ////applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);

                         //transaction.CalcTotals();
                         //transaction.Save();


                        

                        
                        

                         //transaction = APIAccess.APIAccessClass.posTransaction as RetailTransaction; 
                         this.DialogResult = DialogResult.OK;
                         this.Close();

                         //remove discount
                         foreach (var salesLine in transaction.CalculableSalesLines)
                         {

                             foreach (var lineDiscount in salesLine.DiscountLines.ToList())
                             {

                                 if (lineDiscount.ToString() != "LSRetailPosis.Transaction.Line.Discount.TotalDiscountItem")
                                 {

                                     salesLine.DiscountLines.Remove(lineDiscount);


                                 }

                             }

                         }
                         //end

                        




                         transaction.CalcTotals();
                         transaction.Save();

                         applicationLoc.RunOperation(PosisOperations.PayCustomerAccount, "42", transaction); //42 PROD 37 DEV 


                         //APIAccess.APIParameter.ApiResponseBlibliUpdateTransStatus response = APIAccess.APIFunction.BlibliOrderAPI.updateTransStatus(url, lblOrderNo.Text.ToString(), APIAccess.APIAccessClass.posTransaction.TransactionId);
                         //this.DialogResult = DialogResult.OK;
                         //this.Close();
                     }
                     else //if not exists, then create one and then pay customer
                     {

                        LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
                        
                        
                        foreach (DataGridViewRow row in dgvOrderItems.Rows)
                        {
                            if (!row.IsNewRow) // hindari baris kosong terakhir
                            {
                                // Contoh ambil value dari kolom "ItemId" dan "Quantity"
                                string itemId = row.Cells["itemid"].Value.ToString();
                                decimal quantity = Convert.ToDecimal(row.Cells["qty"].Value);


                                RetailTransaction blibliPosTransactionLocal = BlankOperations.blibliPosTransaction as RetailTransaction;
                                BlankOperations.itemIdToAdd = itemId;
                                BlankOperations.quantityToAdd = quantity;


                                applicationLoc.RunOperation(PosisOperations.BlankOperation, "BliBliTransaction", blibliPosTransactionLocal);
                            }                          
                                
                        }

                         RetailTransaction blibliPosTransaction = BlankOperations.blibliPosTransaction as RetailTransaction;
                         //transaction.CalcTotals();
                         //applydiscount voucher blibli (if any)
                         if (totalAdjustment != 0)
                         {
                             //transaction.SetLoyaltyDiscAmount()
                             blibliPosTransaction.SetTotalDiscAmount(totalAdjustment);
                             blibliPosTransaction.Comment = "BliBliDiscount";
                             try
                             {
                                 PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                                 PosApplication.Instance.Triggers.Invoke<IDiscountTrigger>((Action<IDiscountTrigger>)(t => t.PostTotalDiscountAmount(APIAccess.APIAccessClass.posTransaction)));
                                 //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 

                             }
                             catch (Exception ex)
                             {
                                 LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                             }
                             application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(blibliPosTransaction);
                             blibliPosTransaction.CalcTotals();
                             //end

                         }
                        
                         foreach (var itemSale in blibliPosTransaction.SaleItems)
                         {
                             IApplication applicationLocal = PosApplication.Instance as IApplication;

                             var foundItem = orderList
                                            .Where(o => o.product != null)
                                            .SelectMany(o => o.product)
                                            .FirstOrDefault(p => p.sellerSku == itemSale.ItemId);


                             itemSale.CustomerPrice = foundItem.itemPrice;
                             itemSale.GrossAmount = foundItem.itemPrice;
                             itemSale.OriginalPrice = foundItem.itemPrice;
                             itemSale.Price = foundItem.itemPrice;
                             //itemSale.TaxAmount
                             //salesLine.TradeAgreementPriceGroup = result[1];
                             itemSale.TradeAgreementPrice = foundItem.itemPrice;

                             itemSale.ClearPeriodicDiscounts();
                             itemSale.ClearCustomerDiscountLines(true);                              
                             applicationLoc.Services.Tax.CalculateTax(itemSale, blibliPosTransaction);

 
                         }
                         //applicationLoc.Services.Discount.AddTotalDiscountAmount(transaction, 10000);
                     
                         this.DialogResult = DialogResult.OK;
                         this.Close();

                         //remove discount
                         foreach (var salesLine in blibliPosTransaction.CalculableSalesLines)
                         {

                             foreach (var lineDiscount in salesLine.DiscountLines.ToList())
                             {

                                 if (lineDiscount.ToString() != "LSRetailPosis.Transaction.Line.Discount.TotalDiscountItem")
                                 {

                                     salesLine.DiscountLines.Remove(lineDiscount);


                                 }

                             }

                         }
                        
                         blibliPosTransaction.CalcTotals();
                         blibliPosTransaction.Save();

                         applicationLoc.RunOperation(PosisOperations.PayCustomerAccount, "42", blibliPosTransaction); 

                         

                     }
                 }

               
                
            }
            else
            {

                //tidak bisa lanjut apabila tidak punya stock salah satu itemnya.
                bool adaStockKosong = false;

                foreach (DataGridViewRow row in dgvOrderItems.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var stockValue = row.Cells["Stock"].Value;

                        if (stockValue != null && stockValue.ToString() == "Tidak")
                        {
                            adaStockKosong = true;
                            break;
                        }
                    }
                }

                if (adaStockKosong)
                {
                    MessageBox.Show(
                                    "Tidak bisa melanjutkan order, ada item dengan stok kosong.\nSilakan batalkan pesanan.",
                                    "Peringatan",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );

                    btnCancel.Visible = true;
                }
                else
                {
                    DialogResult result = MessageBox.Show(
                "Konfirmasi proses pesanan?\nPastikan stock fisik mencukupi.", // pesan
                "Konfirmasi",                        // judul
                MessageBoxButtons.YesNo,             // tombol Yes / No
                MessageBoxIcon.Question              // ikon tanda tanya
            );

                    // Cek jawaban user
                    if (result == DialogResult.Yes)
                    {
                        LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
                        //iSale.OperationID = PosisOperations.ItemSale;
                        //iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                        //iSale.Barcode = skuId; disable by Yonathan 21/10/2022

                        //use blank operation to store the items.
                        try
                        {
                            foreach (DataGridViewRow row in dgvOrderItems.Rows)
                            {
                                if (!row.IsNewRow) // hindari baris kosong terakhir
                                {
                                    // Contoh ambil value dari kolom "ItemId" dan "Quantity"
                                    string itemId = row.Cells["itemid"].Value.ToString();
                                    decimal quantity = Convert.ToDecimal(row.Cells["qty"].Value);


                                    RetailTransaction blibliPosTransactionLocal = BlankOperations.blibliPosTransaction as RetailTransaction;
                                    BlankOperations.itemIdToAdd = itemId;
                                    BlankOperations.quantityToAdd = quantity;


                                    applicationLoc.RunOperation(PosisOperations.BlankOperation, "BliBliTransaction", blibliPosTransactionLocal);
                                }




                                RetailTransaction blibliPosTransaction = BlankOperations.blibliPosTransaction as RetailTransaction;


                                //Disable Cust Disc 06082025 - Yonathan

                                int indexLines = 0;

                                string isB2bCust = APIAccess.APIAccessClass.isB2b;
                                string priceGroup = APIAccess.APIAccessClass.priceGroup;//.ToString();
                                string lineDiscGroup = APIAccess.APIAccessClass.lineDiscGroup;//.ToString();





                                blibliPosTransaction.CalcTotals();
                                blibliPosTransaction.Save();

                                BlankOperations.blibliPosTransactionDisc = blibliPosTransaction;


                                RetailTransaction transaction = posTransaction as RetailTransaction;
                                var application = PosApplication.Instance as IApplication;



                                transaction = blibliPosTransaction; // (RetailTransaction)BlankOperations.grabPosTransactionDisc; 
                                transaction.Comment = lblOrderNo.Text.ToString();
                                //applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);
                                transaction.CalcTotals();
                                transaction.Save();
                                suspendTrans =  application.RunOperation(PosisOperations.SuspendTransaction, 1, transaction);
                                
                            }

                            
                            RetailTransaction child = (RetailTransaction)suspendTrans;

                            
                            
                             //if exist, then proceed to API blibli
                            if (child.EntryStatus == PosTransaction.TransactionStatus.OnHold)
                            {

                                //application.RunOperation(PosisOperations.DisplayTotal, "");
                                int tryCount = 0;

                                do
                                {
                                    error = createPackage();

                                    if (error == false)
                                    {
                                        error2 = fulfillOrder(packageId);
                                        //this.Close();//continue to suspend


                                        MessageBox.Show(
                                            "Order sudah diambil.\nSilakan siapkan barang dan tunggu driver, baru lanjutkan finalisasi pesanan",
                                            "Info",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                        );
                                        //MessageBox.Show("Order sudah diambil.\nSilakan siapkan barang dan tunggu driver, baru lanjutkan finalisasi pesanan");
                                        POSFormsManager.ShowPOSStatusPanelText("Blibli order telah diambil. Siapkan pesanan.");

                                        APIAccess.APIAccessClass.blibliOrderIdLong = "";
                                        APIAccess.APIAccessClass.blibliOrderState = "";

                                        this.DialogResult = DialogResult.OK;
                                        this.Close();

                                        break;
                                    }
                                    else
                                    {
                                        tryCount++;
                                    }

                                }
                                while (tryCount < 3);
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Gagal proses transaksi Blibli.\nHubungi IT untuk cek permission suspend transaksi.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                               
                            }
                               
                        }
                        catch (Exception ex)
                        {
                            LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                            throw;
                        }
                        //TESTING

                        //error = createPackage();
                        //if (error == false)
                        //{
                        //    error2 = fulfillOrder(packageId);
                        //}

                        //if (error2 == false)
                        //{
                        //    LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
                        //    //iSale.OperationID = PosisOperations.ItemSale;
                        //    //iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                        //    //iSale.Barcode = skuId; disable by Yonathan 21/10/2022

                        //    //use blank operation to store the items.
                        //    try
                        //    {
                        //        foreach (DataGridViewRow row in dgvOrderItems.Rows)
                        //        {
                        //            if (!row.IsNewRow) // hindari baris kosong terakhir
                        //            {
                        //                // Contoh ambil value dari kolom "ItemId" dan "Quantity"
                        //                string itemId = row.Cells["itemid"].Value.ToString();
                        //                decimal quantity = Convert.ToDecimal(row.Cells["qty"].Value);


                        //                RetailTransaction blibliPosTransactionLocal = BlankOperations.blibliPosTransaction as RetailTransaction;
                        //                BlankOperations.itemIdToAdd = itemId;
                        //                BlankOperations.quantityToAdd = quantity;


                        //                applicationLoc.RunOperation(PosisOperations.BlankOperation, "BliBliTransaction", blibliPosTransactionLocal);
                        //            }




                        //            RetailTransaction blibliPosTransaction = BlankOperations.blibliPosTransaction as RetailTransaction;


                        //            //Disable Cust Disc 06082025 - Yonathan

                        //            int indexLines = 0;

                        //            string isB2bCust = APIAccess.APIAccessClass.isB2b;
                        //            string priceGroup = APIAccess.APIAccessClass.priceGroup;//.ToString();
                        //            string lineDiscGroup = APIAccess.APIAccessClass.lineDiscGroup;//.ToString();





                        //            blibliPosTransaction.CalcTotals();
                        //            blibliPosTransaction.Save();

                        //            BlankOperations.blibliPosTransactionDisc = blibliPosTransaction;


                        //            RetailTransaction transaction = posTransaction as RetailTransaction;
                        //            var application = PosApplication.Instance as IApplication;



                        //            transaction = blibliPosTransaction; // (RetailTransaction)BlankOperations.grabPosTransactionDisc; 
                        //            transaction.Comment = lblOrderNo.Text.ToString();
                        //            //applicationLoc.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);
                        //            transaction.CalcTotals();
                        //            transaction.Save();
                        //            application.RunOperation(PosisOperations.SuspendTransaction, 1, transaction);
                        //        }

                        //        //this.Close();//continue to suspend



                        //        MessageBox.Show("Order sudah diambil.\nSilakan siapkan barang dan tunggu driver, baru lanjutkan finalisasi pesanan");
                        //        POSFormsManager.ShowPOSStatusPanelText("Blibli order telah diambil. Siapkan pesanan.");


                        //        //application.RunOperation(PosisOperations.DisplayTotal, "");
                        //        this.DialogResult = DialogResult.OK;
                        //        this.Close();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        //        throw;
                        //    }
                            
                        //}
                    }
                }
            }
            //response.data.value.packageId;
        }

        private bool createPackage()
        {
            string url = "";
            string itemIds = "";
            APIAccess.APIParameter.Receiver receiverParm;
            string functionName = "CreatePackageBlibliAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);


            foreach (var order in orderList)
            {
                if (order.order_item == null)
                    continue;

                foreach (var item in order.order_item)
                {
                    if (!string.IsNullOrEmpty(itemIds))
                    {
                        itemIds += ",";
                    }
                    itemIds += item.id;
                }
            }

            APIAccess.APIParameter.ApiResponseBlibliCreatePackage response = APIAccess.APIFunction.BlibliOrderAPI.createPackage(url, lblOrderNo.Text.ToString(), itemIds);


            packageId = response.data.value.packageId;

            return response.error;
        }

        private bool fulfillOrder(string _packageId)
        {
            string url = "";
            string itemIds = "";
            APIAccess.APIParameter.Receiver receiverParm;
            string functionName = "FulfillOrderBlibliAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);



            APIAccess.APIParameter.ApiResponseBlibliFulfillOrder response = APIAccess.APIFunction.BlibliOrderAPI.fulfillOrder(url, lblOrderNo.Text.ToString());

            return response.error;
        }

        

        private void lblOrderNoTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            if (lblStatus.Text.ToString() == "Pesanan diterima")
            {
                // Tampilkan konfirmasi
                DialogResult result = MessageBox.Show(
                    "Apakah ingin membatalkan pesanan?", // pesan
                    "Konfirmasi",                        // judul
                    MessageBoxButtons.YesNo,             // tombol Yes / No
                    MessageBoxIcon.Question              // ikon tanda tanya
                );

                // Cek jawaban user
                if (result == DialogResult.Yes)
                {
                    string url = "";
                    string functionName = "CancelOrderBlibliAPI";
                    APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
                    url = APIClass.getURLAPIByFuncName(functionName);
                    APIAccess.APIParameter.ApiResponseBlibliCancelOrder response = APIAccess.APIFunction.BlibliOrderAPI.cancelOrder(url, lblOrderNo.Text.ToString());

                    if (response.error == false)
                        MessageBox.Show("Order dibatalkan");
                    //this.DialogResult = DialogResult.OK;
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Tidak bisa batalkan pesanan yang sudah diproses");
            }
        }

        private void lblStatusDetail_Click(object sender, EventArgs e)
        {

        }

        private void lblStatusTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void lblOrderNo_Click(object sender, EventArgs e)
        {

        }
    }
}
