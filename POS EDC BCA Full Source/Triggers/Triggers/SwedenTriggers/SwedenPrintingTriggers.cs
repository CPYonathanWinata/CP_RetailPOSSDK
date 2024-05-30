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
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using LSRetailPosis.Settings.FunctionalityProfiles;
    using LSRetailPosis.Transaction;
    using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// Sweden printing trigger.
    /// </summary>
    [Export(typeof(IPrintingTrigger))]
    public class SwedenPrintingTriggers : IPrintingTrigger, IGlobalization
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

        [CLSCompliant(false)]
        public void PrePrintReceiptCopy(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {            
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction != null && PosApplication.Instance.Services.FiscalRegister.FiscalRegisterEnabled())
            {
                try
                {
                    RegisterCopyOfReceiptUtility.RegisterCopyOfReceipt(retailTransaction);
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationLog.Log("SwedishFiscalRegister.RegisterReceiptCopy", ex.Message, LSRetailPosis.LogTraceLevel.Error);

                    if (preTriggerResult != null)
                    {
                        preTriggerResult.ContinueOperation = false;
                        preTriggerResult.Message = ex.Message;
                    }
                }
            }
        }
    }
}
