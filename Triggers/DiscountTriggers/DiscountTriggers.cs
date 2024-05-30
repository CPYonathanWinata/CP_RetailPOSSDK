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
using LSRetailPosis.Transaction.Line.InfocodeItem;
using LSRetailPosis.Transaction;
using System.Data.SqlClient;
using System.Data;
using System;

namespace Microsoft.Dynamics.Retail.Pos.DiscountTriggers
{

    [Export(typeof(IDiscountTrigger))]
    public class DiscountTriggers : IDiscountTrigger
    {

        #region Constructor - Destructor

        public DiscountTriggers()
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for DiscountTriggers are reserved at 53000 - 53999

        }

        #endregion

        #region IDiscountTriggersV1 Members

        public void PreLineDiscountAmount(IPreTriggerResult preTriggerResult, IPosTransaction transaction, int LineId)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV1.PreLineDiscountAmount", "Triggered before adding line discount amount.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PreLineDiscountPercent(IPreTriggerResult preTriggerResult, IPosTransaction transaction, int LineId)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV1.PreLineDiscountPercent", "Triggered before adding line discount percentange.", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

        #region IDiscountTriggersV2 Members

        public void PostLineDiscountAmount(IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PostLineDiscountAmount", "Triggered after adding line discount amount.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostLineDiscountPercent(IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PostLineDiscountPercent", "Triggered after adding line discount percentange.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PreTotalDiscountAmount(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PreTotalDiscountAmount", "Triggered before total discount amount.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostTotalDiscountAmount(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PostTotalDiscountAmount", "Triggered after total discount amount.", LSRetailPosis.LogTraceLevel.Trace);

            //Custom to add payment discount
            if(checkDiscPayment(posTransaction) == true)
            {
                addDiscountInfoCode(posTransaction);
            }
            
        }

        
        public void PreTotalDiscountPercent(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PreTotalDiscountPercent", "Triggered before total discount percent.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostTotalDiscountPercent(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PostTotalDiscountPercent", "Triggered after total discount percent.", LSRetailPosis.LogTraceLevel.Trace);
            //Custom to add payment discount
            if (checkDiscPayment(posTransaction) == true)
            {
                addDiscountInfoCode(posTransaction);
            }
        }
        
        #endregion


        //Custom to add payment discount
        private void addDiscountInfoCode(IPosTransaction posTransaction)
        {
            //add infocode lines
            var retailTransaction = posTransaction as RetailTransaction;
            InfoCodeLineItem infocodeLines = new InfoCodeLineItem()
            {
                InfocodeId = "DISCOUNT",
                Information = retailTransaction.Comment,
                RefRelation = LSRetailPosis.Settings.ApplicationSettings.Terminal.FunctionalityProfile,
                Transaction = retailTransaction,

                // Set other properties as needed
            };
            retailTransaction.InfoCodeLines.AddLast(infocodeLines);
            retailTransaction.Comment = "PAYMENTDISCOUNT";
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
        //end

    }
}
