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
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ICustomer;
using LSRetailPosis.Transaction;
 
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.Contracts;

using APIAccess;
using LSRetailPosis.POSProcesses;
namespace Microsoft.Dynamics.Retail.Pos.CustomerTriggers
{
    [Export(typeof(ICustomerTrigger))]
    public class CustomerTriggers : ICustomerTrigger
    {
        [Import]
        public IApplication Application { get; set; }
        #region Constructor - Destructor
        
        //add customization to compare old vs new customer by Yonathan 18/01/2024
        string oldCustId = "";


        public CustomerTriggers()
        {

            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for CustomerTriggers are reserved at 65000 - 65999
        }

        #endregion

        #region ICustomerTriggersV1 Members

        public void PreCustomerClear(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV1.PreCustomerClear", "Prior to clearing a customer from the transaction.", LSRetailPosis.LogTraceLevel.Trace);
            if (validateCustomer(posTransaction))
            {
                MessageBox.Show("Unable to change/clear customer when voucher is applied ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                preTriggerResult.ContinueOperation = false;
            }
        }

        public void PostCustomerClear(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV1.PostCustomerClear", "After clearing a customer from the transaction.", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

        #region ICustomerTriggersV2 Members
        /// <summary>
        /// Triggered at the beginning
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        public void PreCustomer(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV2.PreCustomer", "Triggered at the beginning.", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered at the end
        /// </summary>
        /// <param name="posTransaction"></param>
        public void PostCustomer(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV2.PostCustomer", "Triggered at the end.", LSRetailPosis.LogTraceLevel.Trace);

            RetailTransaction   transaction = posTransaction as RetailTransaction;
            if (Application.TransactionServices.CheckConnection())
			{ 
			    try
			    {
                    ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getB2bRetailParam", transaction.Customer.CustomerId.ToString());
                    //APIAccess.APIAccessClass.userID = "";
                    APIAccess.APIAccessClass.custId = transaction.Customer.CustomerId.ToString();
                    APIAccess.APIAccessClass.isB2b = containerArray[3].ToString();
                    APIAccess.APIAccessClass.priceGroup = containerArray[4].ToString();
                    APIAccess.APIAccessClass.lineDiscGroup = containerArray[5].ToString();
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    throw;
                }
            }  
            //containerArray[2].ToString(); //3,4,5
            //string userIDFromClass = APIAccess.APIAccessClass.userID;


            //compare old vs new customer to clear the disc total payment - yonathan 18/01/2024
           
            //if (oldCustId != "" && transaction.Customer.CustomerId != oldCustId)
            //{
            //    if (checkDiscPayment(posTransaction) == true)
            //    {
            //        Application.Services.Discount.AddTotalDiscountAmount(transaction, 0);
            //        Application.Services.Discount.AddTotalDiscountPercent(transaction, 0);
            //        //application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction); 
            //        Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);

            //        //globalTransaction.CalcTotals();
            //        transaction.Comment = "";
            //        //retailTransaction.CalcTotals();
            //        //POSFormsManager.ShowPOSStatusPanelText("Discount payment removed");
            //        MessageBox.Show("Discount payment removed");
            //        //Application.RunOperation(PosisOperations.DisplayTotal, "");
            //        //retailTransaction.CalcTotals();
            //    }
            //}
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

        /// <summary>
        /// Triggered just before the customer is set
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="customerId"></param>
        public void PreCustomerSet(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, string customerId)
        {
            
            if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
            {
                DE customer = ((RetailTransaction)posTransaction).Customer;
                if(customer.CustomerId != null)
                oldCustId = customer.CustomerId.ToString();
            }


            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV2.PreCustomerSet", "Triggered just before the customer is set.", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered before customer search
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        public void PreCustomerSearch(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV2.PreCustomerSearch", "Triggered before customer search.", LSRetailPosis.LogTraceLevel.Trace);

            if(validateCustomer(posTransaction))
            {
                MessageBox.Show("Unable to change/clear customer when voucher is applied ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                preTriggerResult.ContinueOperation = false;
            }
        }

        /// <summary>
        /// Triggered after customer search
        /// </summary>
        /// <param name="posTransaction"></param>
        public void PostCustomerSearch(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV2.PostCustomerSearch", "Triggered after customer search.", LSRetailPosis.LogTraceLevel.Trace);
        }


        #endregion


        #region customize
        private bool validateCustomer(IPosTransaction posTransaction)
        {
            if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
            {
                DE customer = ((RetailTransaction)posTransaction).Customer;

                if (!String.IsNullOrEmpty(customer.CustomerId))
                {
                    SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

                    try
                    {

                        string queryString = @"SELECT TOP 1 VOUCHERCODE FROM CPEXTVOUCHERCODETMP";

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
                                    return true;
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
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
