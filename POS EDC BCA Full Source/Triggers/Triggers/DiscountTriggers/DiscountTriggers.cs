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
        }

        public void PreTotalDiscountPercent(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PreTotalDiscountPercent", "Triggered before total discount percent.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostTotalDiscountPercent(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IDiscountTriggersV2.PostTotalDiscountPercent", "Triggered after total discount percent.", LSRetailPosis.LogTraceLevel.Trace);
        }
        
        #endregion

    }
}
