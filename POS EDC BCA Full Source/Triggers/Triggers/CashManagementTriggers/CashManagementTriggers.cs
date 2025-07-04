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


namespace Microsoft.Dynamics.Retail.Pos.CashManagementTriggers
{
    [Export(typeof(ICashManagementTrigger))]
    public class CashManagementTriggers : ICashManagementTrigger
    {

        #region Constructor - Destructor

        public CashManagementTriggers()
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for CashManagementTriggers are reserved at 63000 - 63999
        }

        #endregion

        #region ICashManagementTriggers Members

        public void PreTenderDeclaration(IPreTriggerResult preTriggerResult, IPosTransaction transaction)
        {
            LSRetailPosis.ApplicationLog.Log("CashManagementTriggers.PreTenderDeclaration", "Before running the Tender Declaration operation...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostTenderDeclaration(IPosTransaction transaction)
        {
            LSRetailPosis.ApplicationLog.Log("CashManagementTriggers.PostTenderDeclaration", "After running the Tender Declaration operation...", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

    }
}
