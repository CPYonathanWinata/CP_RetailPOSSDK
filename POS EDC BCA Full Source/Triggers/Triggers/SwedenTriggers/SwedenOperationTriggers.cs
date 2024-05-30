/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Localization.Sweden.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Settings.FunctionalityProfiles;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;

    /// <summary>
    /// Sweden operation triggers.
    /// </summary>
    [Export(typeof(IOperationTrigger))]
    class SwedenOperationTriggers : IOperationTrigger, IGlobalization
    {
        #region Globalization
        private readonly ReadOnlyCollection<string> supportedCountryRegions = new ReadOnlyCollection<string>(new string[] { SupportedCountryRegion.SE.ToString() });

        /// <summary>
        /// Defines ISO country region codes this functionality is applicable for.
        /// </summary>
        public ReadOnlyCollection<string> SupportedCountryRegions
        {
            get { return supportedCountryRegions; }
        }
        #endregion

        #region Operation Triggers
        public void PreProcessOperation(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, PosisOperations posisOperation)
        {
            if (preTriggerResult == null)
            {
                throw new ArgumentNullException("preTriggerResult");
            }

            if (Array.IndexOf(new[] {PosisOperations.ConvertCustomerOrder, PosisOperations.SalesOrder, PosisOperations.CustomerAccountDeposit, PosisOperations.CustomerOrderDetails }, posisOperation) > -1)
            {
                // This operation is not supported for Sweden.
                var errorMessage = ApplicationLocalizer.Language.Translate(107217);
                POSFormsManager.ShowPOSErrorDialog(new PosisException(errorMessage));
                LSRetailPosis.ApplicationLog.Log("SwedenOperationTriggers.PreProcessOperation", errorMessage, LSRetailPosis.LogTraceLevel.Error);
                preTriggerResult.ContinueOperation = false;
            }
        }

        public void PostProcessOperation(IPosTransaction posTransaction, PosisOperations posisOperation)
        {
            // Left empty on purpose.
        }
        #endregion
    }
}
