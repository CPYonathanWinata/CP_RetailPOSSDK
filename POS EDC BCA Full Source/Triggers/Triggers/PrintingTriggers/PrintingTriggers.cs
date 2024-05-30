/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.PrintingTriggers
{
    using System.ComponentModel.Composition;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;

    /// <summary>
    /// Printing trigger.
    /// </summary>
    [Export(typeof(IPrintingTrigger))]
    public class PrintingTriggers : IPrintingTrigger
    {
        #region Constructor - Destructor

        public PrintingTriggers()
        {
            // Get all text through the Translation function in the ApplicationLocalizer           
        }

        #endregion
       
        public void PrePrintReceiptCopy(IPreTriggerResult preTriggerResult,  IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IPrintingTriggersV1.PrePrintReceiptCopy", "Before printing copy of receipt", LSRetailPosis.LogTraceLevel.Trace);
        }
    }
}
