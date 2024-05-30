/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;

namespace Microsoft.Dynamics.Retail.Pos.OperationTriggers
{
    [Export(typeof(IOperationTrigger))]
    public class OperationTriggers : IOperationTrigger
    {
       #region Constructor - Destructor

        public OperationTriggers()
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for InfocodeTriggers are reserved at 59000 - 59999
        }

        #endregion

        #region IOperationTriggersV1 Members

        /// <summary>
        /// Before the operation is processed this trigger is called.
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="posisOperation"></param>
        public void PreProcessOperation(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, PosisOperations posisOperation)
        {
            LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV1.PreProcessOperation", "Before the operation is processed this trigger is called.", LSRetailPosis.LogTraceLevel.Trace);

            BlankOperations.BlankOperations.ShowMsgBoxInformation("test");
        }

        /// <summary>
        /// After the operation has been processed this trigger is called.
        /// </summary>
        /// <param name="posTransaction"></param>
        /// <param name="posisOperation"></param>
        public void PostProcessOperation(IPosTransaction posTransaction, PosisOperations posisOperation)
        {
            LSRetailPosis.ApplicationLog.Log("IOperationTriggersV1.PostProcessOperation", "After the operation has been processed this trigger is called.", LSRetailPosis.LogTraceLevel.Trace);            
        }

        #endregion

    }
}
