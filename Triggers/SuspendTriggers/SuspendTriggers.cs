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

namespace Microsoft.Dynamics.Retail.Pos.SuspendTriggers
{
    [Export(typeof(ISuspendTrigger))]
    public class SuspendTriggers : ISuspendTrigger
    {

        #region Constructor - Destructor

        public SuspendTriggers()
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for SuspendTriggers are reserved at 62000 - 62999

        }

        #endregion

        #region ISuspendTriggers Members

        public void OnSuspendTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("SuspendTriggers.OnSuspendTransaction", "On the suspension of a transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void OnRecallTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("SuspendTriggers.OnRecallTransaction", "On the recall of a suspended transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

        #region ISuspendTriggers Members

        public void PreSuspendTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("SuspendTriggers.PreSuspendTransaction", "Prior to the suspension of a transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostSuspendTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("SuspendTriggers.PostSuspendTransaction", "After the suspension of a transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PreRecallTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("SuspendTriggers.PreRecallTransaction", "Prior to the recall of a transaction from suspension...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostRecallTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("SuspendTriggers.PostRecallTransaction", "After the recall of a transaction from suspension...", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

    }
}
