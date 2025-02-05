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
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;

using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

using System.Text;
using System.Net;

using System;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

using PaymentTriggers;

using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_EngageFALWSServices;

using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ICustomer;
using LSRetailPosis.Settings;
using System.Xml;
using System.Drawing;

namespace Microsoft.Dynamics.Retail.Pos.PaymentTriggers
{
    [Export(typeof(IPaymentTrigger))]
    public class PaymentTriggers : IPaymentTrigger
    {

        [Import]
        public IApplication Application { get; set; } //add by yonathan 02/01/2024


        int custNoOrder;
        string NoOrder;
        private IEFTInfo eftInfo;
        
   //     private ICardInfo cardInfo;
   //     private IEFTV2 eftV2;
           
        int amount;

        #region Constructor - Destructor

        public PaymentTriggers()
        {

            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for PaymentTriggers are reserved at 50400 - 50449
              
        }
              
        public PaymentTriggers(IEFTInfo eftInfo)
        {
            this.eftInfo = eftInfo;
        }

        #endregion

        #region IPaymentTriggers Members

        public void PrePayCustomerAccount(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, decimal amount)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PrePayCustomerAccount", "Before charging to a customer account", LSRetailPosis.LogTraceLevel.Trace);
       //     MessageBox.Show(amount + ":  prepaycustomeraccount");
        }

        public void PrePayCardAuthorization(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, ICardInfo cardInfo, decimal amount)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PrePayCardAuthorization", "Before the EFT authorization", LSRetailPosis.LogTraceLevel.Trace);
       
            //add by Julius for EDC BCA 09.08.2021

            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            queryData data = new queryData();

          /*  if (GME_Var.isEDCBCA)
            {
                cardInfo.CardNumber = "52";
                cardInfo.CardTypeId = "EDC BCA";
                cardInfo.TenderTypeId = "16";

         
                this.checkPaymetToEngage("10");

                GME_Var.payCardBCA = true;

                
            }
            else
            {
                preTriggerResult.ContinueOperation = false;
            }*/
            preTriggerResult.ContinueOperation = false;
        }
 

        /// <summary>
        /// <example><code>
        /// // In order to delete the already-added payment you use the following code:
        /// if (retailTransaction.TenderLines.Count > 0)
        /// {
        ///     retailTransaction.TenderLines.RemoveLast();
        ///     retailTransaction.LastRunOpererationIsValidPayment = false;
        /// }
        /// </code></example>
        /// </summary>
        public void OnPayment(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.OnPayment", "On the addition of a tender...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PrePayment(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, object posOperation, string tenderId)
        {
            string messageBoxString = "";

            //add by Yonathan 13/05/2024 to check B2B Cust cannot pay unless creat a customer order first
            if ((APIAccess.APIAccessClass.isB2b == "1" || APIAccess.APIAccessClass.isB2b == "2") && posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
            {
                string custType = APIAccess.APIAccessClass.isB2b == "1" ? "Canvas" : "B2B";
                using (frmMessage dialog = new frmMessage(string.Format("Customer {0} diharuskan membuat Customer Order\nterlebih dahulu untuk bisa melakukan pembayaran.",custType), MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
              
                preTriggerResult.ContinueOperation = false;                     
                
                return;
            }
            //add if to check whether the stock is enough by yonathan 02/01/2023
            if (checkStock(posTransaction, out messageBoxString) == true)
            {
               
                preTriggerResult.ContinueOperation = false;
                return;
 
            }
            else
            {
                //add new form to ask customer detail //13012025 CPPOSPAYMENT
                //using (CP_CustomerDetail customerDetail = new CP_CustomerDetail())
                //{                   
                //    customerDetail.ShowDialog();
                
                //}
                //end
                //   MessageBox.Show("pre payment");
                LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PrePayment", "On the start of a payment operation...", LSRetailPosis.LogTraceLevel.Trace);
                if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
                {
                    DE customer = ((RetailTransaction)posTransaction).Customer;
                    int amount = (int)((RetailTransaction)posTransaction).NetAmountWithTax;

                    //string HexRespon = "0202000130313030303030303031303030303030303030303030303030303136383838382A2A2A2A2A2A383232352020202A2A2A2A3030202020202020313030303933363937303138323031383031303231373031323530303030303530303030313539393943443035303936314E5445535420494E444F4D415245542044474E20464C415A5A20202020202020202020202020202020202031303030303530303030363930374E4E4E3030303030303030303030302020202020202020202020204E2020202020202020032D";

                    //  this.getBinaryHex(posTransaction.TransactionId);

                    if (!String.IsNullOrEmpty(customer.CustomerId))
                    {
                        this.getCustAddNoOrder(customer.CustomerId);
                        this.getNoOrder(posTransaction.TransactionId);

                        //Validation customer External Voucher (SAMSUNG GIFT) - ES 14 November 2019
                        if (checkCustVoucherCode(customer.CustomerId) && !checkVoucherCode())
                        {
                            MessageBox.Show("For this customer you have to 'Add Voucher Code' , select from Additional task ");
                            preTriggerResult.ContinueOperation = false;
                        }
                        //END Validation customer External Voucher (SAMSUNG GIFT) - ES 14 November 2019


                        // Validate lock tender - ES 15 May 2022
                        #region CPLOCKTENDER
                        if (!validateCustomer(customer.CustomerId, tenderId))
                        {
                            using (frmMessage dialog = new frmMessage("Please Choose Correct Customer for this Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            {
                                POSFormsManager.ShowPOSForm(dialog);
                            }
                            preTriggerResult.ContinueOperation = false;
                        }
                        #endregion
                        // End Validate lock tender - ES 15 May 2022
                    }
                    else
                    {
                        // Validate lock tender - ES 15 May 2022
                        #region CPLOCKTENDER
                        if ((PosisOperations)posOperation != PosisOperations.PayCash && (PosisOperations)posOperation != PosisOperations.PayGiftCertificate)
                        {
                            using (frmMessage dialog = new frmMessage("Please Choose Correct Customer for this Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            {
                                POSFormsManager.ShowPOSForm(dialog);
                            }
                            preTriggerResult.ContinueOperation = false;
                        }
                        #endregion
                        // End Validate lock tender - ES 15 May 2022
                    }

                    if (custNoOrder == 1 && String.IsNullOrEmpty(NoOrder))
                    {
                        MessageBox.Show("For this customer you have to 'Add no Order' , select from Additional task ");
                        preTriggerResult.ContinueOperation = false;
                    }

                    //Add Validation for Top Up BNI Tapcash - ES 09092019
                    #region topUpBNI
                    string listItem = "";
                    int totalItem = 0;
                    var dict = new Dictionary<string, int>();

                    listItem += "(";

                    for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
                    {
                        if (totalItem != 0 && !((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Voided && ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId != "")
                            listItem += ", ";

                        if (!((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Voided && ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId != "")
                        {
                            listItem += "'" + ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId + "'";
                            dict[((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId] = (int)((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Quantity;
                            totalItem++;
                        }
                    }

                    listItem += ")";

                    if (totalItem != 0)
                    {
                        SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                        try
                        {

                            string queryString = @" SELECT MAX(ID1) AS ID1, MAX(ID2) AS ID2, SUM(TOTAL_ITEM1) AS TOTAL_ITEM1, SUM(TOTAL_ITEM2) AS TOTAL_ITEM2, SUM(EDC1) AS EDC1, SUM(EDC2) AS EDC2
                                        FROM
                                        (
	                                        SELECT ITEMID ID1, '' ID2, SUM(1) TOTAL_ITEM1, 0 TOTAL_ITEM2, SUM(EDCTYPE) EDC1, 0 EDC2
	                                        FROM [AX].[CPITEMTOPUP] WHERE ITEMID IN " + listItem + @"
	                                        GROUP BY ITEMID
	
	                                        UNION

	                                        SELECT '' ID1, ITEMID ID2, 0 TOTAL_ITEM1, SUM(1) TOTAL_ITEM2, 0 EDC1, SUM(EDCTYPE) EDC2
	                                        FROM [AX].[CPITEMTOPUPFEE] WHERE ITEMID IN " + listItem + @"
	                                        GROUP BY ITEMID
                                        ) AS TOPUP
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
                                        if (reader[2].ToString() != "" && reader[3].ToString() != "")
                                        {
                                            if ((PosisOperations)posOperation != PosisOperations.PayCash)
                                            {
                                                MessageBox.Show("PAYMENT METHOD MUST BE CASH FOR TOP UP");
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                            else if ((int)reader[2] < 1)
                                            {
                                                MessageBox.Show("Please choose top up item");
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                            else if ((int)reader[2] > 1)
                                            {
                                                MessageBox.Show("Only 1 top up item allowed");
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                            else if (dict[reader[0].ToString()] > 1)
                                            {
                                                MessageBox.Show("Qty top up item must not be more than 1");
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                            else if (reader[1].ToString() != "" && dict[reader[1].ToString()] > 1)
                                            {
                                                MessageBox.Show("Qty top up fee must not be more than 1");
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                            else if ((int)reader[3] != 1)
                                            {
                                                MessageBox.Show("Please choose top up fee");
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                            else if ((int)reader[4] != (int)reader[5])
                                            {
                                                MessageBox.Show("Top up item and top up fee does not match");
                                                preTriggerResult.ContinueOperation = false;
                                            }
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
            
                #endregion
                //END Add Validation for Top Up BNI Tapcash - ES 09092019

                // CPAPIEZEELINK CHECK FOR INTERNET CONNECTION - ES 07112019
                //       CPAPIEZEELINKCheckConnection();
                // END CPAPIEZEELINK CHECK FOR INTERNET CONNECTION - ES 07112019
            }

            switch ((PosisOperations)posOperation)
            {
                case PosisOperations.PayCash:
                    //   code here...
                    //      int amount = (int)((RetailTransaction)posTransaction).NetAmountWithTax;
                    //     CP_CheckPayment();
                    break;
                case PosisOperations.PayCard:
                    // Insert code here...
                    GME_Var.isPayTransaction = true;
                    GME_Var.isPayCard = true;
                    ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                    if (GME_Var.isEDCBCA)
                    {
                        this.checkPaymetToEngage("10");
                    }



                    break;
                case PosisOperations.PayCheque:
                    // Insert code here...
                    break;
                case PosisOperations.PayCorporateCard:
                    // Insert code here...
                    break;
                case PosisOperations.PayCreditMemo:
                    // Insert code here...
                    break;
                case PosisOperations.PayCurrency:
                    // Insert code here...
                    break;
                case PosisOperations.PayCustomerAccount:
                    // Insert code here...
                    break;
                case PosisOperations.PayGiftCertificate:
                    // Insert code here...
                    break;
                case PosisOperations.PayLoyalty:
                    // Insert code here...
                    break;

                // etc.....
            }
        }
        //add by yonathan 02/01/2023
        static string TruncateString(string input, int desiredLength)
        {
            // Check if the string is shorter than the desired length
            if (input.Length < desiredLength)
            {
                // Pad the string with spaces to reach the desired length
                return input.PadRight(desiredLength + 9);
            }

            // Check if the string is longer than the desired length
            if (input.Length > desiredLength)
            {
                // Truncate the string to the desired length
                return input.Substring(0, desiredLength);
            }

            // If the string is already of the desired length, return the original string
            return input;
        }

        private bool checkPositiveStatus(string _itemId)
        {
            //before checking the stock, check first whether this item type is service
            //string queryString = @" SELECT DISPLAYPRODUCTNUMBER,PRODUCTTYPE  FROM ax.ECORESPRODUCT where DISPLAYPRODUCTNUMBER =@ITEMID";
            string positiveStatus = "0";
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";
            try
            {
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
                            ////for testing purpose
                            //MessageBox.Show(positiveStatus);
                            ////
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
            

            return positiveStatus == "1" ? true : false;
        }

        private string getVariantId(string _itemId, SqlConnection connection)
        {
            string configId = "";
            try
                {
                    string queryString2 = @"SELECT ITEMID, RETAILVARIANTID FROM [ax].[INVENTITEMBARCODE]
                                             WHERE ITEMID =  @ITEMID";
                    using (SqlCommand command = new SqlCommand(queryString2, connection))
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

                                configId = reader["RETAILVARIANTID"].ToString();

                                //configIdMulti = reader["RETAILVARIANTID"].ToString();
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
            return configId;
        }

        private bool checkStock(IPosTransaction posTransaction, out string messageBoxString)
        {
            string itemIdMulti = "";

            bool findStockEmpty = false;
            decimal remainQty = 0;
            int indexRow = 0;
            string itemId = "";

            string itemName = "";
            var urlRTS = "";
            var urlAPI = "";
            string statusItem = "";
            string xmlResponse;
            bool positiveStatus = false;
            string siteId = "";
            string warehouseId = "", configId = "", configIdMulti = "";
            string functionNameAX = "GetStockAX%"; // "GetStockAXPFMPOC"; //change to GetStockAX
            string functionNameAPI = "GetItemAPI";
             messageBoxString = "";
            //var retailTransaction = posTransaction as RetailTransaction;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            //APIAccess.APIAccessClass.itemToRemove = new List<string>();
            APIAccess.APIFunction apiFunction = new APIAccess.APIFunction();
            RetailTransaction transaction = posTransaction as RetailTransaction;
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            urlRTS = APIClass.getURLAPIByFuncName(functionNameAX);


            //var positiveItemIds = transaction.SaleItems
            //.Where(item => !item.Voided && checkPositiveStatus(item.ItemId))
            //.GroupBy(item => item.ItemId)
            //.Select(group => group.Key.ToString());

            //itemIdMulti = string.Join(";", positiveItemIds);


            //// Split the string by semicolon (;) delimiter
            //string[] itemIdVal = itemIdMulti.Split(';');

            //new to get item quantity
            //change by Yonathan to add QTY Input 15/08/2024
            var positiveItems = transaction.SaleItems
            .Where(item => !item.Voided && checkPositiveStatus(item.ItemId))
            .GroupBy(item => item.ItemId)
            .Select(group => new
            {
                ItemId = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            });

            var positiveItemIds = positiveItems
                .Select(item => item.ItemId.ToString());

            itemIdMulti = string.Join(";", positiveItemIds);

            var positiveQuantities = positiveItems
                .Select(item => item.Quantity);

            string quantityItems = string.Join(";", positiveQuantities);
            //end
            // Split the string by semicolon (;) delimiter
            string[] itemIdVal = itemIdMulti.Split(';');

            // Loop through each substring
            for (int indexLoop = 0; indexLoop < itemIdVal.Length; indexLoop++)
            {
                string itemIdSingle;

                itemIdSingle = itemIdVal.ElementAt(indexLoop);

                configId = getVariantId(itemIdSingle, connection);
                //check if this is not the last index
                if (indexLoop != (itemIdVal.Length -1))
                {
                    configIdMulti += configId + ";";
                }
                else
                {
                    configIdMulti += configId;
                }
            }
 

            

            /*
            foreach (var orderItem in transaction.SaleItems.Where(s => s.Voided == false))
            {
                positiveStatus = checkPositiveStatus(orderItem.ItemId);
                
                // Add the separator (;) if it's not the last item
                if (orderItem.ItemId != transaction.SaleItems.ElementAt(transaction.SaleItems.Count - 1).ItemId && positiveStatus == true)
                {
                    itemIdMulti += ";";
                }
                else if (positiveStatus == true)
                {
                    itemIdMulti += orderItem.ItemId;
                }


                ////chec if positivestatus
                
                ////loop through the items in the cart

                //if(positiveStatus == true)
                //{
                //    itemIdMulti += orderItem.ItemId;
                //    // Add the separator (;) if it's not the last item
                //    if (orderItem.ItemId != transaction.SaleItems.ElementAt(transaction.SaleItems.Count - 1).ItemId)
                //    {
                //        itemIdMulti += ";";
                //    }
                //}
                 
                
            }*/

            

            
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

            ////for testing purpose
            //MessageBox.Show(itemIdMulti);
            ////
           
            if (itemIdMulti != "")
            {
                //change by Yonathan to add QTY Input 15/08/2024
                var result = apiFunction.checkStockOnHandMultiNew(Application, urlRTS, Application.Settings.Database.DataAreaID, siteId, ApplicationSettings.Terminal.InventLocationId, itemIdMulti, "", "", configIdMulti, quantityItems, transaction.TransactionId);

                //only for testing with old method
                //var result = apiFunction.checkStockOnHandMulti(Application, urlRTS, Application.Settings.Database.DataAreaID, siteId, ApplicationSettings.Terminal.InventLocationId, itemIdMulti, "", "", configIdMulti);//, quantityItems, transaction.TransactionId);
                //end
                xmlResponse = result[3].ToString();
                
                XmlDocument xmlDoc = new XmlDocument(); 
                xmlDoc.LoadXml(xmlResponse);

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//StockListResult");
                //messageBoxString = "Stock barang di bawah ini kurang atau tidak cukup untuk ditransaksikan. Silakan hapus atau kurangi quantity-nya.\n\n";//ITEMID   | QTY TERSEDIA \n";
                //messageBoxString += "ITEMID | NAMA ITEM                | QTY TERSEDIA \n";

                //string[] itemIdsArray = itemIdMulti.Split(';');

                //foreach (XmlNode node in itemNodes)
                //{
                //    itemId = node.Attributes["ItemId"].Value;
                //    remainQty = Convert.ToDecimal(node.Attributes["QtyAvail"].Value);
                //    var selectedSaleItem = transaction.SaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value && item.Voided != true);
                //    statusItem = remainQty - selectedSaleItem.Quantity < 0 ? "Tidak" : "Ya";

                //    if (statusItem == "Tidak")
                //    {
                //        itemName = selectedSaleItem.Description.PadRight(35); // Adjust the width as needed
                         
                //        selectedSaleItem.ShouldBeManuallyRemoved = true;
                //        findStockEmpty = true;
                //    }
                //}


                //Yonathan for test purposes
                
                //end  
                // Show custom dialog form
                using (Infolog customDialog = new Infolog(itemNodes,transaction))
                {
                    findStockEmpty = customDialog.findStockEmpty;
                    if (findStockEmpty == true)
                    {
                        customDialog.ShowDialog();

                    }
                     
                    


                }
                //customDialogForm.Controls.Add(dataGridView);

                //// Show the custom dialog form
                //customDialogForm.ShowDialog();
            }

          


            /*
            // Iterate through the array using a foreach loop
            //foreach (string orderItem in itemIdsArray)
            foreach (var orderItem in transaction.SaleItems.Where(s => s.Voided == false))
            //&& !excludedItemIds.Split(';').Contains(item.ItemId.ToString())
            //foreach (XmlNode node in itemNodes)
            //foreach (var orderItem in transaction.SaleItems.Where(s => s.Voided == false && !itemIdMulti.Split(';').Contains(s.ItemId.ToString())))
            {//availQty;// -(orderItem.quantity);// + qtyBeforeAdded);

                //remainQty = Convert.ToDecimal(node.Attributes["QtyAvail"].Value); 
                remainQty = Convert.ToDecimal(itemNodes[indexRow].Attributes["QtyAvail"].Value);
                statusItem = remainQty - orderItem.Quantity < 0 ? "Tidak" : "Ya";
                itemId = itemNodes[indexRow].Attributes["ItemId"].Value;
                

                if (statusItem == "Tidak")
                {
                    itemName = orderItem.Description; //TruncateString(orderItem.Description, 33);
                    messageBoxString += itemId + " | " + itemName + " | " + remainQty + "\n";// +" | " + statusItem + "\n";
                    orderItem.ShouldBeManuallyRemoved = true;
                    findStockEmpty = true;
                }
                //APIAccess.APIAccessClass.itemToRemove.Add(itemId);
                    //+ "\n";
                indexRow++;
            }
             * */

            //MessageBox.Show(messageBoxString);
            return findStockEmpty;
            //itemNodes[indexRow].Attributes["ItemId"].Value
            //itemNodes[indexRow].Attributes["QtyAvail"].Value
            //throw new NotImplementedException();
        }



        /// <summary>
        /// Triggered before voiding of a payment.
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"> </param>
        public void PreVoidPayment(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PreVoidPayment", "Before the void payment operation...", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered after voiding of a payment.
        /// </summary>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"> </param>
        public void PostVoidPayment(IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PostVoidPayment", "After the void payment operation...", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered before registering cash payment.
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="posOperation"></param>
        /// <param name="tenderId"></param>
        /// <param name="currencyCode"></param>
        /// <param name="amount"></param>
        public void PreRegisterPayment(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, object posOperation, string tenderId, string currencyCode, decimal amount)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PreRegisterPayment", "Before registering the payment...", LSRetailPosis.LogTraceLevel.Trace);
        }
        #endregion

        #region Custom
        private void getCustAddNoOrder(string custId)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string queryString = @" SELECT CPNoOrder FROM [ax].[CUSTTABLE]
                                        WHERE ACCOUNTNUM =  @CUSTID
                                        AND CPNoOrder = 1";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@CUSTID", custId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            custNoOrder = reader.GetInt32(0);
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

        private void getNoOrder(string transactionId)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @" SELECT NOORDER FROM [dbo].[CPOrderTable]
                                        WHERE TRANSACTIONID =  @TRANSACTIONID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", transactionId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NoOrder = reader.GetString(0);
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

        public static char ReturnLRC(string RequestMessage)
        {
            int lrcAnswer = 0;
            for (int i = 0; i < RequestMessage.Length; i++)
            {
                lrcAnswer = lrcAnswer ^ (Byte)(Encoding.UTF7.GetBytes(RequestMessage.Substring(i, 1))[0]);
            }
            return (Char)lrcAnswer;
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

        public static string ConvertHexToString(string HexValue)
        {
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("https://pfm.cp.co.id/api/connection/check"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        /*
        private void CPAPIEZEELINKCheckConnection()
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = "SELECT TOP 1 TRANSACTIONID FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

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
                            if(!CheckForInternetConnection())
                            {
                                MessageBox.Show("No Internet Connection, Current transaction will not earn point.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
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
        }
        */
        private bool checkCustVoucherCode(string custID)
        {
            bool returnValue = false;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @" SELECT ISVOUCHERCODEREQUIRED FROM [ax].[CPCUSTTABLEVOUCHER]
                                        WHERE ACCOUNTNUM =  @CUSTID
                                        AND ISVOUCHERCODEREQUIRED = 1";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@CUSTID", custID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            returnValue = true;
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

            return returnValue;
        }

        private bool checkVoucherCode()
        {
            bool returnValue = false;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {

                string queryString = @"SELECT TOP 1 VOUCHERCODE FROM CPEXTVOUCHERCODETMP";

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
                            returnValue = true;
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

            return returnValue;
        }

        private void CP_PaymentForm(decimal amount)
        {
            CP_FormPayment cpFormPayment = new CP_FormPayment(amount);
            cpFormPayment.ShowDialog();
        //    break;
        }
        
        /*
        private void CP_ECRBCA(decimal amount)
        {
          //  int amount = (int)((RetailTransaction)posTransaction).NetAmountWithTax;
        //    MessageBox.Show("masuk ke CPECRBCA"); 
            
            SerialPort port = new SerialPort();
            port.PortName = "COM6";
       //     port.PortName = "COM7";
            port.Parity = Parity.None;
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
           // port.Handshake = Handshake.RequestToSend;
           // port.ReceivedBytesThreshold = 8;
            if (port.IsOpen)
            {
                MessageBox.Show("port is open");
                port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
                port.Close();
                port.Dispose();
            }
            else 
            {
         //       MessageBox.Show("port is not open");
            }
          //   string hexString = this.CPgetHexData(amount);
       
             byte[] bytesToSend = StringToByteArray(CPgetHexData(amount));

             int lrcValue = bytesToSend[1] ^ bytesToSend[2];

             for (int i = 3; i < bytesToSend.Length; i++)
            {
                lrcValue ^= bytesToSend[i];
            }

            //hexString = ConvertStringToHex(paddingValue);
            int tmp = lrcValue;
            string hexString = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            //Array.Clear(valueByte, 0, valueByte.Length);
            byte[] valueByte = StringToByteArray(hexString);

            for (int i = 0; i < valueByte.Length; i++)
            {
                Array.Resize(ref bytesToSend, bytesToSend.Length + 1);
                bytesToSend[bytesToSend.GetUpperBound(0)] = valueByte[i];
            }

            Console.WriteLine("Request Hex : ");
            Console.WriteLine(bytesToSend);
            
            try
            {
                port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
                port.Open();
       //         MessageBox.Show("open port");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
   //         MessageBox.Show("before write port ");
            port.Write(bytesToSend,0,bytesToSend.Length);
     //       MessageBox.Show("write port");
       //     port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);

         //   Console.ReadKey();

        //    cardInfo.AuthCode = "12345";
         //   cardInfo.CardNumber = "012345678";
            
            port.Close();
            port.Dispose();
            
        }
        
        private void mySerialPort_DataReceived(
                   object sender,
                   SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //data received on serial port is asssigned to "indata" string
        //    Console.WriteLine("Received data:");
            MessageBox.Show("Received data:");
            Console.WriteLine(indata.Length);
            MessageBox.Show(""+indata.Length);

         //   int count = 0;
         //   string response = "";

         //   string hex = BitConverter.ToString(data);

            if (indata.Length > 1)
            {
                MessageBox.Show(indata);
            //    while ((indata[149] != 0x30 && indata[150] != 0x30) && (indata[149] != 0x47 && indata[150] != 0x45))
             //   {
         //           mySerialPort.Write(bytestosend, 0, bytestosend.Length);
                //    MessageBox.Show("a");
              //  }
                //    port.Close();
                //   port.Dispose();
            }
     //       port.Close();
      //      port.Dispose();
          } 

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //    MessageBox.Show("string to byte ");
            return bytes;
        }
        
        /*
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
         * */
         
        /*
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;

            // Insert code here...
            MessageBox.Show("response port");

            int bytes = port.BytesToRead;
            byte[] buffer = new byte[bytes];

            if (port.BytesToRead > 1)
            {

                port.Read(buffer, 0, bytes);
            }

            foreach (byte item in buffer)
            {
                Console.WriteLine(item);
                Console.ReadKey();
                MessageBox.Show(""+item, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        
            }

            //string indata = port.ReadExisting();
            //Console.WriteLine("Data received");
            //Console.WriteLine(indata);
            //Console.ReadKey();
        }
        */
        
        private bool checkPaymetToEngage(string payMethod)
        {
            bool isInsertedCard = false;
            foreach (string val in GME_Var.paymentMethods)
            {
                if (val == payMethod)
                {
                    isInsertedCard = true;
                }
            }

            if (!isInsertedCard)
            {
                GME_Var.paymentMethods.Add(payMethod);
            }

            return isInsertedCard;
        }

        #region CPLOCKTENDER
        //function to validate customer based on tender
        private bool validateCustomer(string customerId, string tenderId)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryStringID = @"SELECT 
	                                        CUSTACCOUNT 
                                        FROM ax.CPCUSTTENDERMAPTABLE
                                        WHERE 
	                                        CUSTACCOUNT = @CUSTOMERID
	                                        AND TENDERTYPEID = @TENDERTYPEID
	                                        AND DATAAREAID = @DATAAREAID";

                using (SqlCommand command = new SqlCommand(queryStringID, connection))
                {
                    command.Parameters.AddWithValue("@CUSTOMERID", customerId);
                    command.Parameters.AddWithValue("@TENDERTYPEID", tenderId);
                    command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
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

            return false;
        }

        #endregion
         
        #endregion
    }
}
