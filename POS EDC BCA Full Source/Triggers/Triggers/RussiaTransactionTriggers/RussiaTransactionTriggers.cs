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
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.TransactionServices;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;

namespace Microsoft.Dynamics.Retail.Localization.Russia.TransactionTriggers
{
    [Export(typeof(ITransactionTrigger))]
    public class TransactionTriggers : ITransactionTrigger, IGlobalization
    {
        #region Globalization
        private readonly ReadOnlyCollection<string> supportedCountryRegions = new ReadOnlyCollection<string>(new string[] { SupportedCountryRegion.RU.ToString() });

        /// <summary>
        /// Defines ISO country region codes this functionality is applicable for.
        /// </summary>
        public ReadOnlyCollection<string> SupportedCountryRegions
        {
            get { return supportedCountryRegions; }
        }
        #endregion

        #region Private declarations
        private const int timeFrameAllowedForReturn = 1;
        #endregion

        #region Properties
        /// <summary>
        /// Application instance.
        /// </summary>
        [Import]
        public IApplication Application { get; set; }
        #endregion

        #region Triggers

        public void BeginTransaction(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void SaveTransaction(IPosTransaction posTransaction, System.Data.SqlClient.SqlTransaction sqlTransaction)
        {
            // Left empty on purpose.
        }

        public void PreEndTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("RussianTransactionTriggers.PreEndTransaction", "Before concluding transaction...", LSRetailPosis.LogTraceLevel.Trace);

            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction != null)
            {
                if (!LSRetailPosis.Settings.ApplicationSettings.Terminal.ProcessReturnsAsInOriginalSaleShift_RU)
                {
                    if (retailTransaction.SaleIsReturnSale)
                    {
                        var originalTransaction = new TransactionData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, Application).GetOriginalRetailTransaction(retailTransaction);
                        if (originalTransaction == null)
                            throw new InvalidOperationException("Original retail transaction is not found.");

                        bool? hasSameShiftId =
                            Application.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled() ?
                            Application.Services.Peripherals.FiscalPrinter.HasSameShiftId(originalTransaction) :
                            (originalTransaction.Shift != null && originalTransaction.Shift.BatchId == Application.Shift.BatchId);
                        // Skip aggregation if we have a return transaction returning an item from the original transaction that was registered in a previous (not current) shift.
                        if (hasSameShiftId.HasValue && !hasSameShiftId.Value)
                        {
                            retailTransaction.SkipAggregation = true;
                        }
                    }
                    else if (retailTransaction.SaleItems != null &&
                        (from sl in retailTransaction.SaleItems
                         where sl.Quantity < decimal.Zero && !sl.Voided
                         select sl).Any())
                    {
                        retailTransaction.SkipAggregation = true;
                    }
                }
            }
        }

        public void PostEndTransaction(IPosTransaction posTransaction)
        {
            var retailTransaction = posTransaction as IRetailTransaction;
            if (retailTransaction != null)
            {
                PrintGiftCardsBalance(retailTransaction);
            }
        }

        public void PreVoidTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PostVoidTransaction(Pos.Contracts.DataEntity.IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public void PreReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction, IPosTransaction posTransaction)
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                throw new InvalidOperationException("posTransaction as RetailTransaction is null");
            }

            if (retailTransaction.SaleItems == null)
            {
                throw new InvalidOperationException("(posTransaction as RetailTransaction).SaleItems is null");
            }

            if (preTriggerResult == null)
            {
                throw new ArgumentNullException("preTriggerResult");
            }

            if (LSRetailPosis.Settings.ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
            {
                if (preTriggerResult.ContinueOperation &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => (!l.Voided && l.Amount != decimal.Zero && !l.AddTo)))
                {
                    TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 107005);
                    preTriggerResult.ContinueOperation = false;
                }

                if (preTriggerResult.ContinueOperation &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => (!l.Voided && l.AddTo)))
                {
                    TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 107006);
                    preTriggerResult.ContinueOperation = false;
                }
            }

            if (preTriggerResult.ContinueOperation &&
                !LSRetailPosis.Settings.ApplicationSettings.Terminal.ProcessReturnsAsInOriginalSaleShift_RU)
            {
                if (retailTransaction.SaleItems.Any(l => (!l.Voided && l.Quantity < decimal.Zero && !l.ReceiptReturnItem)))
                {
                    TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 106040);
                    preTriggerResult.ContinueOperation = false;
                }

                if (retailTransaction.SaleItems.Any(
                    l => (!l.Voided && l.ReceiptReturnItem && 
                   (l.ReturnStoreId != originalTransaction.StoreId || 
                    l.ReturnTerminalId != originalTransaction.TerminalId || 
                    l.ReturnTransId != originalTransaction.TransactionId))))
                {
                    TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, 106040);
                    preTriggerResult.ContinueOperation = false;
                }
            }
        }

        public void PostReturnTransaction(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreConfirmReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction)
        {
            if (preTriggerResult == null)
            {
                throw new ArgumentNullException("preTriggerResult");
            }

            if (originalTransaction == null)
            {
                throw new ArgumentNullException("originalTransaction");
            }

            bool dialogWasShown = false;

            if (preTriggerResult.ContinueOperation &&
                originalTransaction.TerminalId != ApplicationSettings.Terminal.TerminalId)
            {
                ShowNotSameDateReturnDialog(ref dialogWasShown, preTriggerResult, ApplicationSettings.Terminal.TerminalOperator.AllowNotSameDateReturn_RU, 106006, 106007);
            }

            if (!dialogWasShown)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(((IPosTransactionV1)originalTransaction).BeginDateTime.Date);
                if (preTriggerResult.ContinueOperation &&
                    timeSpan.TotalDays > timeFrameAllowedForReturn)
                {
                    ShowNotSameDateReturnDialog(ref dialogWasShown, preTriggerResult, ApplicationSettings.Terminal.TerminalOperator.AllowNotSameDateReturn_RU, 106000, 106001, ((IPosTransactionV1)originalTransaction).BeginDateTime.ToShortDateString());
                }
            }

            if (!dialogWasShown && preTriggerResult.ContinueOperation)
            {
                bool? sameFiscalPrinterShiftId = null;
                if (Application.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
                {
                    sameFiscalPrinterShiftId = Application.Services.Peripherals.FiscalPrinter.HasSameShiftId(originalTransaction);
                    if (sameFiscalPrinterShiftId.HasValue && !sameFiscalPrinterShiftId.Value)
                    {
                        ShowNotSameDateReturnDialog(ref dialogWasShown, preTriggerResult, ApplicationSettings.Terminal.TerminalOperator.AllowNotSameDateReturn_RU, 106008, 106009);
                    }
                }
                if (!dialogWasShown && !sameFiscalPrinterShiftId.HasValue && originalTransaction.Shift != null && originalTransaction.Shift.BatchId != Application.Shift.BatchId)
                {
                    ShowNotSameDateReturnDialog(ref dialogWasShown, preTriggerResult, ApplicationSettings.Terminal.TerminalOperator.AllowNotSameDateReturn_RU, 106008, 106009);
                }
            }
        }
        #endregion

        #region Private methods
        private static void ShowNotSameDateReturnDialog(ref bool dialogWasShown, IPreTriggerResult preTriggerResult, EmployeeNotSameDateReturn allowNotSameDateReturn, int errorMessageId, int warnMessageId, params string[] messageArgs)
        {
            // Early return if the dialog was already shown to the user.
            if (dialogWasShown)
                return;

            if (allowNotSameDateReturn == EmployeeNotSameDateReturn.Reject)
            {
                TriggerHelpers.ShowDialog(MessageBoxButtons.OK, MessageBoxIcon.Error, errorMessageId, messageArgs);
                preTriggerResult.ContinueOperation = false;
                dialogWasShown = true;
            }
            else if (allowNotSameDateReturn == EmployeeNotSameDateReturn.Warn && preTriggerResult.ContinueOperation)
            {
                preTriggerResult.ContinueOperation = TriggerHelpers.ShowDialog(MessageBoxButtons.YesNo, MessageBoxIcon.Warning, warnMessageId, messageArgs) == DialogResult.Yes;
                dialogWasShown = true;
            }
        }


        private void PrintGiftCardsBalance(IRetailTransaction transaction)
        {
            RetailTransaction retailTransaction = transaction as RetailTransaction;
            if (retailTransaction != null && Application.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            {
                var giftCardList = this.GetGiftCardLineItemList(retailTransaction);
                Application.Services.Peripherals.FiscalPrinter.PrintGiftCardsBalance(giftCardList, retailTransaction);
            }            
        }
        private IList<IGiftCardLineItem> GetGiftCardLineItemList(IRetailTransaction transaction)
        {
            var retailTransaction = transaction as RetailTransaction;
            var giftCardsList = new List<IGiftCardLineItem>();
            giftCardsList.AddRange(retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Where(t => !t.Voided && t.Amount != 0));
            giftCardsList.AddRange(this.GetGiftCardsFromPayments(retailTransaction));            
            this.RetrieveGiftCardProperties(giftCardsList);
            return giftCardsList;           
        }
              
        private IList<IGiftCardLineItem> GetGiftCardsFromPayments(IRetailTransaction transaction)
        {
            var retailTransaction = transaction as RetailTransaction;
            var giftCardsList = new List<IGiftCardLineItem>();
            foreach (var giftCardTender in retailTransaction.TenderLines.OfType<IGiftCardTenderLineItem>().Where(t => !t.Voided))
            {                
                IGiftCardLineItem gcItem = Application.BusinessLogic.Utility.CreateGiftCardLineItem(
                       ApplicationSettings.Terminal.StoreCurrency, Application.Services.Rounding, (IRetailTransaction)retailTransaction);
                gcItem.SerialNumber = giftCardTender.SerialNumber;
                gcItem.AddTo = false;                
                gcItem.Amount = giftCardTender.Amount;
                gcItem.Description = giftCardTender.Description;
                giftCardsList.Add(gcItem);
            }
            return giftCardsList;
        }
               
        private void RetrieveGiftCardProperties(IList<IGiftCardLineItem> giftCardsList)
        {
            bool succeeded = false;
            string comment = string.Empty;
            string currencyCode = string.Empty;
            decimal giftCardBalance = 0M;
            GiftCardStatus status = GiftCardStatus.Active;
            DateTime AcriveFrom = DateTime.MinValue;
            DateTime ExpiryDate = DateTime.MinValue;
            bool oneTimeRedemption = false;
            bool nonReloadable = false;
            decimal minReloadAmountAllowed = 0m;
            decimal maxBalanceAllowed = 0m;
            string policyAccountingCurrency = String.Empty;
           
            foreach (var gcItem in giftCardsList)
            {
                Application.TransactionServices.GetGiftCardBalance(ref succeeded, ref comment, ref currencyCode, ref giftCardBalance, gcItem.SerialNumber);
                gcItem.Balance = giftCardBalance;                

                Application.TransactionServices.GetGiftCardPolicies(
                            ref succeeded,
                            ref comment,
                            gcItem.SerialNumber,
                            ref status,
                            ref AcriveFrom,
                            ref ExpiryDate,
                            ref oneTimeRedemption,
                            ref nonReloadable,
                            ref minReloadAmountAllowed,
                            ref maxBalanceAllowed,
                            ref policyAccountingCurrency);
                gcItem.PolicyGiftCardStatus = status;
                gcItem.PolicyActivationDate = AcriveFrom;
                gcItem.PolicyExpirationDate = ExpiryDate;
                gcItem.PolicyMaxBalanceAllowed = Application.Services.Currency.CurrencyToCurrency(policyAccountingCurrency,
                                                                                                    ApplicationSettings.Terminal.StoreCurrency,
                                                                                                    maxBalanceAllowed);
                gcItem.PolicyMinReloadAmountAllowed = Application.Services.Currency.CurrencyToCurrency(policyAccountingCurrency,
                                                                                                    ApplicationSettings.Terminal.StoreCurrency,
                                                                                                    minReloadAmountAllowed);
                gcItem.PolicyNonReloadable = nonReloadable;
                gcItem.PolicyOneTimeRedemption = oneTimeRedemption;               
            }                                
        }

        #endregion
    }
}
