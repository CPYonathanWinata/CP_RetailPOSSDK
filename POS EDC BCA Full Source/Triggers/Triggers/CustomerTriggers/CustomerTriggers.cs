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
using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Customer;


namespace Microsoft.Dynamics.Retail.Pos.CustomerTriggers
{
    [Export(typeof(ICustomerTrigger))]
    public class CustomerTriggers : ICustomerTrigger
    {

        #region Constructor - Destructor

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
            
            if (GME_Var.isRefreshCustomer)
            {
                if (GME_Var.customerData != null)
                {
                    Customer.Customer cust = new Customer.Customer();

                    cust.AddCustomerToTransaction(GME_Var.customerData, posTransaction);
                }
            }
            else
            {
                GME_Var.identifierCode = null;
                GME_FormCaller.GME_FormCustomerType(Connection.applicationLoc, posTransaction);
            }            
        }

        /// <summary>
        /// Triggered at the end
        /// </summary>
        /// <param name="posTransaction"></param>
        public void PostCustomer(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV2.PostCustomer", "Triggered at the end.", LSRetailPosis.LogTraceLevel.Trace);            
        }

        /// <summary>
        /// Triggered just before the customer is set
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="customerId"></param>
        public void PreCustomerSet(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, string customerId)
        {
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
    }
}
