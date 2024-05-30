/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace RussiaItemTriggers
{
    [Export(typeof(IItemTrigger))]
    public class RussiaItemTriggers : IItemTrigger, IGlobalization
    {
        #region Globalization
        private readonly ReadOnlyCollection<string> supportedCountryRegions = new ReadOnlyCollection<string>(new string[] { SupportedCountryRegion.RU.ToString() });

        public System.Collections.ObjectModel.ReadOnlyCollection<string> SupportedCountryRegions
        {
            get { return supportedCountryRegions; }
        }
        #endregion

        #region Item Triggers
        public void PreSale(IPreTriggerResult preTriggerResult, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PostSale(Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction)
        {
            if (posTransaction == null)
            {
                throw new ArgumentNullException("posTransaction is null", "posTransaction");
            }

            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("posTransaction as RetailTransaction is null", "posTransaction");
            }

            var notValidatedSaleLineItems = retailTransaction.SaleItems.Where(si => (si.PriceToBeChecked));
            var saleLineItemsToBeRemoved = new List<SaleLineItem>();
            foreach (SaleLineItem saleItem in notValidatedSaleLineItems)
            {
                if (CheckLabelChangeStatusForStore() &&
                    CheckLabelChangeStatusForCustPriceGroup(saleItem.TradeAgreementPriceGroup) &&
                    saleItem.Price != GetConfirmedPriceFromLabelChangeJournalTrans(saleItem))
                {
                    if (PosApplication.Instance.BusinessLogic.ItemSystem.PriceOverrideRestrictions(retailTransaction.OperatorId) == EmployeePriceOverride.NotAllowed)
                    {
                        TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 100028); //Label change is not confirmed for the sales price. Please contact the store administrator.
                        saleLineItemsToBeRemoved.Add(saleItem);
                    }
                    else
                    {
                        TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Warning, 100028); //Label change is not confirmed for the sales price. Please contact the store administrator.
                        saleItem.PriceValid = true;
                    }
                }
                else
                {
                    saleItem.PriceValid = true;
                }
            }

            // Remove items w/o confirmed price if any
            foreach (var saleLineItem in saleLineItemsToBeRemoved)
            {
                retailTransaction.SaleItems.Remove(saleLineItem);
            }

            if (saleLineItemsToBeRemoved != null && saleLineItemsToBeRemoved.Count > 0)
            {
                retailTransaction.CalcTotals();
            }
        }

        public void PreReturnItem(IPreTriggerResult preTriggerResult, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PostReturnItem(Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreVoidItem(IPreTriggerResult preTriggerResult, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostVoidItem(Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PreSetQty(IPreTriggerResult preTriggerResult, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostSetQty(Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem)
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                throw new ArgumentException("posTransaction as RetailTransaction is null", "posTransaction");
            }

            if (retailTransaction.SaleItems == null)
            {
                throw new ArgumentException("(posTransaction as RetailTransaction).SaleItems is null", "posTransaction");
            }

            if (saleLineItem == null)
            {
                throw new ArgumentNullException("saleLineItem");
            }

            if (LSRetailPosis.Settings.ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments &&
                saleLineItem.Quantity < decimal.Zero)
            {
                if (!retailTransaction.OperationCancelled &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => (!l.Voided && l.Amount != decimal.Zero && !l.AddTo)))
                {
                    TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 107005);
                    retailTransaction.OperationCancelled = true;
                }

                if (!retailTransaction.OperationCancelled &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => (!l.Voided && l.AddTo)))
                {
                    TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 107006);
                    retailTransaction.OperationCancelled = true;
                }
            }

            if (!retailTransaction.OperationCancelled &&
                !LSRetailPosis.Settings.ApplicationSettings.Terminal.ProcessReturnsAsInOriginalSaleShift_RU &&
                saleLineItem.Quantity < decimal.Zero &&
                !saleLineItem.ReceiptReturnItem &&
                retailTransaction.SaleItems.Any(l => (!l.Voided && l.ReceiptReturnItem)))
            {
                TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 106041);
                retailTransaction.OperationCancelled = true;
            }
        }

        public void PrePriceOverride(IPreTriggerResult preTriggerResult, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostPriceOverride(Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem)
        {
            // Left empty on purpose.
        }

        public void PreClearQty(IPreTriggerResult preTriggerResult, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, int lineId)
        {
            // Left empty on purpose.
        }

        public void PostClearQty(Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.IPosTransaction posTransaction, Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ISaleLineItem saleLineItem)
        {
            // Left empty on purpose.
        }

        #endregion

        #region Private methods
        private bool CheckLabelChangeStatusForStore()
        {
            var storeDataManager = new StoreDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            Store store = storeDataManager.GetStore(LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId);

            return (store.PrintProductLabels || store.PrintShelfLabels);
        }

        private bool CheckLabelChangeStatusForCustPriceGroup(string custPriceGroup)
        {
            bool retailCheckPriceInCustPriceGroup = false;

            if (!string.IsNullOrEmpty(custPriceGroup))
            {
                retailCheckPriceInCustPriceGroup = new DiscountDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID).GetCheckSalesPriceStatus(custPriceGroup);
            }

            return retailCheckPriceInCustPriceGroup;
        }

        private decimal GetConfirmedPriceFromLabelChangeJournalTrans(SaleLineItem saleItem)
        {
            decimal confirmedPrice = decimal.Zero;
            var selectStr = "SELECT top 1 PRICE "
                            + "FROM [ax].[RETAILLABELCHANGEJOURNALTRANS] "
                            + "WHERE CONFIRMED = 1 and ITEMID = @ITEMID and VARIANTID = @VARIANTID and STOREID = @STOREID and PRICEVALIDONDATE <= @PRICEVALIDONDATE and DATAAREAID = @DATAAREAID "
                            + "ORDER BY PRICEVALIDONDATE desc, JOURNALNUM desc";

            using (DataTable labelChangePriceTransTable = new DBUtil(ApplicationSettings.Database.LocalConnection).GetTable(selectStr,
                new SqlParameter("@ITEMID", saleItem.ItemId),
                new SqlParameter("@VARIANTID", saleItem.Dimension != null ? saleItem.Dimension.VariantId : string.Empty),
                new SqlParameter("@STOREID", ApplicationSettings.Terminal.StoreId),
                new SqlParameter("@PRICEVALIDONDATE", DateTime.Today),
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID)))
            {
                if (labelChangePriceTransTable.Rows.Count > 0)
                {
                    confirmedPrice = (decimal)labelChangePriceTransTable.Rows[0]["PRICE"];
                }
            }

            return confirmedPrice;
        }
        #endregion
    }
}
