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
    using System.Linq;
    using LSRetailPosis.Settings.FunctionalityProfiles;
    using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    [Export(typeof(IItemTrigger))]
    public class SwedenItemTriggers : IItemTrigger, IGlobalization
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

        public void PreSale(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction)
        {
            if (preTriggerResult == null)
            {
                throw new ArgumentNullException("preTriggerResult");
            }

            if (saleLineItem == null)
            {
                throw new ArgumentNullException("saleLineItem");
            }

            //Transactions with both return and sale operations are not allowed if fiscal register is connected
            var retailTransaction = posTransaction as LSRetailPosis.Transaction.RetailTransaction;
            if (retailTransaction != null)
            {
                if (PosApplication.Instance.Services.FiscalRegister.FiscalRegisterEnabled())
                {
                    bool quantityPositive = false, quantityNegative = false;
                    foreach (var saleItem in retailTransaction.SaleItems.Where(si => !si.Voided && si.Quantity != 0))
                    {
                        if (saleItem.Quantity > 0) quantityPositive = true;
                        else if (saleItem.Quantity < 0) quantityNegative = true;
                    }

                    if ((quantityNegative && !quantityPositive && saleLineItem.Quantity > decimal.Zero) ||
                        (quantityPositive && !quantityNegative && saleLineItem.Quantity < decimal.Zero) ||
                        (quantityPositive && quantityNegative))
                    {
                        preTriggerResult.ContinueOperation = false;
                        //"You cannot return and sale items in the same fiscal receipt. Register the sale of items as a separate operation."
                        preTriggerResult.MessageId = 86469;
                        return;
                    }
                }
            }
        }

        public void PostSale(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreReturnItem(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PostReturnItem(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreVoidItem(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostVoidItem(IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PreSetQty(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostSetQty(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            // Left empty on purpose.
        }

        public void PrePriceOverride(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostPriceOverride(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            // Left empty on purpose.
        }

        public void PreClearQty(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostClearQty(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            // Left empty on purpose.
        }
    }
}
