/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.GiftCertificateItem;
using LSRetailPosis.Transaction.Line.TenderItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.TransactionServices;

namespace Microsoft.Dynamics.Retail.Pos.GiftCard
{
    // Get all text through the Translation function in the ApplicationLocalizer
    //
    // TextID's for the GiftCard service are reserved at 55000 - 55999
    // TextID's for the following modules are as follows:
    //
    // GiftCard.cs:             55000 - 55399. The last in use: 55001
    // GiftCardForm.cs:         55400 - 55499  The last in use: 55434

    [Export(typeof(IGiftCard))]
    public class GiftCard : IGiftCard
    {

        #region IGiftCard Members
        /// <summary>
        /// IApplication instance.
        /// </summary>
        private IApplication application;

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
        [Import]
        public IApplication Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
                InternalApplication = value;
            }
        }

        /// <summary>
        /// Gets or sets the static IApplication instance.
        /// </summary>
        internal static IApplication InternalApplication { get; private set; }

        /// <summary>
        /// Issues gift card.
        /// </summary>
        /// <param name="posTransaction"></param>
        /// <param name="gcTenderInfo"></param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public void IssueGiftCard(IPosTransaction posTransaction, ITender gcTenderInfo)
        {
            LogMessage("Issuing a gift card",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.IssueGiftCard");

            if (GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            {
                //The operation should proceed after the fiscal printer handles IssueGiftCard
                GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.IssueGiftCard(posTransaction, gcTenderInfo);
            }

            // Determines if a gift card item can be added in the Brazilian NFC-e
            if (Functions.CountryRegion == SupportedCountryRegion.BR && !CanIssueGiftCardInConsumerEfd(posTransaction))
            {
                var message = ApplicationLocalizer.Language.Translate(86047);

                Application.Services.Dialog.ShowMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            GiftCardController controller = new GiftCardController(GiftCardController.ContextType.GiftCardIssue, (PosTransaction)posTransaction, (Tender)gcTenderInfo);

            using (GiftCardForm giftCardForm = new GiftCardForm(controller))
            {
                POSFormsManager.ShowPOSForm(giftCardForm);

                if (giftCardForm.DialogResult == DialogResult.OK)
                {
                    // Add the gift card to the transaction.
                    RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                    GiftCertificateItem giftCardItem = (GiftCertificateItem)this.Application.BusinessLogic.Utility.CreateGiftCardLineItem(
                        ApplicationSettings.Terminal.StoreCurrency,
                        this.Application.Services.Rounding, retailTransaction);

                    if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
                    {
                        InitGiftCardItem(giftCardItem);
                    }

                    giftCardItem.SerialNumber = controller.CardNumber;
                    giftCardItem.StoreId = posTransaction.StoreId;
                    giftCardItem.TerminalId = posTransaction.TerminalId;
                    giftCardItem.StaffId = posTransaction.OperatorId;
                    giftCardItem.TransactionId = posTransaction.TransactionId;
                    giftCardItem.ReceiptId = posTransaction.ReceiptId;
                    giftCardItem.Amount = controller.Amount;
                    giftCardItem.Balance = controller.Balance;
                    giftCardItem.Date = DateTime.Now;

                    // Necessary property settings for the the gift certificate "item"...
                    giftCardItem.Price = giftCardItem.Amount;
                    giftCardItem.NetAmount = giftCardItem.Amount;
                    giftCardItem.StandardRetailPrice = giftCardItem.Amount;
                    giftCardItem.Quantity = 1;
                    giftCardItem.TaxRatePct = 0;
                    if (!ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
                    {
                        giftCardItem.Description = ApplicationLocalizer.Language.Translate(55001);  // Gift Card
                    }
                    giftCardItem.Comment = controller.CardNumber;
                    giftCardItem.NoDiscountAllowed = true;
                    giftCardItem.Found = true;

                    retailTransaction.Add(giftCardItem);

                    if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
                    {
                        // Calculate the tax for gift card item
                        this.Application.Services.Tax.CalculateTax(retailTransaction);
                    }
                }
            }
        }

        /// <summary>
        /// Updates gift card.
        /// </summary>
        /// <param name="voided"></param>
        /// <param name="comment"></param>
        /// <param name="gcLineItem"></param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Grandfather")]
        public void VoidGiftCard(ref bool voided, ref string comment, IGiftCardLineItem gcLineItem)
        {
            LogMessage("Voiding a gift card",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.VoidGiftCard");

            if (this.Application.TransactionServices.CheckConnection())
            {
                this.Application.TransactionServices.VoidGiftCard(ref voided, ref comment, gcLineItem.SerialNumber);
            }
        }

        /// <summary>
        /// Unlock/Releases the gift card.
        /// </summary>
        /// <param name="released">if set to <c>true</c> [released].</param>
        /// <param name="comment">The comment.</param>
        /// <param name="gcLineItem">The gift card line item.</param>
        public void UnlockGiftCard(ref bool released, ref string comment, IGiftCardLineItem gcLineItem)
        {
            if (gcLineItem != null)
            {
                LogMessage("Unlock a gift card",
                    LSRetailPosis.LogTraceLevel.Trace,
                    "GiftCard.UnlockGiftCard");

                if (this.Application.TransactionServices.CheckConnection())
                {
                    this.Application.TransactionServices.GiftCardRelease(ref released, ref comment, gcLineItem.SerialNumber);
                }
            }
        }

        /// <summary>
        /// Gift card payment related methods
        /// </summary>
        /// <param name="valid"></param>
        /// <param name="comment"></param>
        /// <param name="posTransaction"></param>
        /// <param name="cardInfo"></param>
        /// <param name="gcTenderInfo"></param>
        public void AuthorizeGiftCardPayment(ref bool valid, ref string comment, IPosTransaction posTransaction, ICardInfo cardInfo, ITender gcTenderInfo)
        {
            if (cardInfo == null)
                throw new ArgumentNullException("cardInfo");

            LogMessage("Authorizing a gift card payment", LogTraceLevel.Trace);

            valid = false;

            GiftCardController controller = new GiftCardController(GiftCardController.ContextType.GiftCardPayment, (PosTransaction)posTransaction, (Tender)gcTenderInfo);

            controller.CardNumber = cardInfo.CardNumber;
            using (GiftCardForm giftCardForm = new GiftCardForm(controller))
            {
                POSFormsManager.ShowPOSForm(giftCardForm);

                if (giftCardForm.DialogResult == DialogResult.OK)
                {
                    valid = true;
                    cardInfo.CardNumber = controller.CardNumber;
                    cardInfo.Amount = controller.Amount;
                    cardInfo.CurrencyCode = controller.Currency;
                }
            }
        }

        /// <summary>
        /// Void payment of gift card.
        /// </summary>
        /// <param name="voided"></param>
        /// <param name="comment"></param>
        /// <param name="gcTenderLineItem"></param>
        public void VoidGiftCardPayment(ref bool voided, ref string comment, IGiftCardTenderLineItem gcTenderLineItem)
        {
            LogMessage("Cancelling the used marking of the gift card.",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.VoidGiftCardPayment");

            GiftCertificateTenderLineItem giftCardTenderLineItem = gcTenderLineItem as GiftCertificateTenderLineItem;
            if (giftCardTenderLineItem == null)
            {
                throw new ArgumentNullException("gcTenderLineItem");
            }

            if (this.Application.TransactionServices.CheckConnection())
            {
                this.Application.TransactionServices.VoidGiftCardPayment(ref voided, ref comment, giftCardTenderLineItem.SerialNumber,
                    giftCardTenderLineItem.Transaction.StoreId, giftCardTenderLineItem.Transaction.TerminalId);
            }
        }

        /// <summary>
        /// Updates gift card details.
        /// </summary>
        /// <param name="updated"></param>
        /// <param name="comment"></param>
        /// <param name="gcTenderLineItem"></param>
        public void UpdateGiftCard(ref bool updated, ref string comment, IGiftCardTenderLineItem gcTenderLineItem)
        {
            LogMessage("Reedming money from gift card.",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.UpdateGiftCard");

            GiftCertificateTenderLineItem giftCardTenderLineItem = gcTenderLineItem as GiftCertificateTenderLineItem;
            if (giftCardTenderLineItem == null)
            {
                throw new ArgumentNullException("gcTenderLineItem");
            }

            decimal balance = 0;

            // Begin by checking if there is a connection to the Transaction Service
            if (this.Application.TransactionServices.CheckConnection())
            {
                this.Application.TransactionServices.GiftCardPayment(ref updated, ref comment, ref balance,
                    giftCardTenderLineItem.SerialNumber, giftCardTenderLineItem.Transaction.StoreId,
                    giftCardTenderLineItem.Transaction.TerminalId, giftCardTenderLineItem.Transaction.OperatorId,
                    giftCardTenderLineItem.Transaction.TransactionId, giftCardTenderLineItem.Transaction.ReceiptId,
                    ApplicationSettings.Terminal.StoreCurrency, giftCardTenderLineItem.ForeignCurrencyAmount, DateTime.Now);

                // Update the balance in Tender line item.
                giftCardTenderLineItem.Balance = balance;
            }
        }

        /// <summary>
        /// Handles Gift Card Balance operation.
        /// </summary>
        public void GiftCardBalance(IPosTransaction posTransaction)
        {
            LogMessage("Inquiring gift card balance.",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.GiftCardBalance");

            GiftCardController controller = new GiftCardController(GiftCardController.ContextType.GiftCardBalance, (PosTransaction)posTransaction);

            using (GiftCardForm giftCardForm = new GiftCardForm(controller))
            {
                POSFormsManager.ShowPOSForm(giftCardForm);
            }
        }

        /// <summary>
        /// Handles AddTo Gift Card operation.
        /// </summary>
        /// <param name="retailTransaction"></param>
        /// <param name="gcTenderInfo"></param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "P0 Integration")]
        public void AddToGiftCard(IRetailTransaction retailTransaction, ITender gcTenderInfo)
        {
            LogMessage("Adding money to gift card.",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.AddToGiftCard");

            if (GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            {
                //The operation should proceed after the fiscal printer handles AddToGiftCard
                GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.AddToGiftCard(retailTransaction, gcTenderInfo);
            }

            GiftCardController controller = new GiftCardController(GiftCardController.ContextType.GiftCardAddTo, (PosTransaction)retailTransaction, (Tender)gcTenderInfo);

            if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
            {
                try
                {
                    controller.PreValidateAddToGiftCard();
                }
                catch (GiftCardException ex)
                {
                    ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    Application.Services.Dialog.ShowMessage(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            using (GiftCardForm giftCardForm = new GiftCardForm(controller))
            {
                POSFormsManager.ShowPOSForm(giftCardForm);

                if (giftCardForm.DialogResult == DialogResult.OK)
                {
                    // Add the gift card to the transaction.
                    GiftCertificateItem giftCardItem = (GiftCertificateItem)this.Application.BusinessLogic.Utility.CreateGiftCardLineItem(
                        ApplicationSettings.Terminal.StoreCurrency, this.Application.Services.Rounding, retailTransaction);

                    if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
                    {
                        InitGiftCardItem(giftCardItem);
                    }

                    giftCardItem.SerialNumber = controller.CardNumber;
                    giftCardItem.StoreId = retailTransaction.StoreId;
                    giftCardItem.TerminalId = retailTransaction.TerminalId;
                    giftCardItem.StaffId = retailTransaction.OperatorId;
                    giftCardItem.TransactionId = retailTransaction.TransactionId;
                    giftCardItem.ReceiptId = retailTransaction.ReceiptId;
                    giftCardItem.Amount = controller.Amount;
                    giftCardItem.Balance = controller.Balance;
                    giftCardItem.Date = DateTime.Now;
                    giftCardItem.AddTo = true;

                    giftCardItem.Price = giftCardItem.Amount;
                    giftCardItem.StandardRetailPrice = giftCardItem.Amount;
                    giftCardItem.Quantity = 1;
                    giftCardItem.TaxRatePct = 0;
                    if (!ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
                    {
                        giftCardItem.Description = ApplicationLocalizer.Language.Translate(55000);  // Add to Gift Card
                    }
                    giftCardItem.Comment = controller.CardNumber;
                    giftCardItem.NoDiscountAllowed = true;
                    giftCardItem.Found = true;

                    ((RetailTransaction)retailTransaction).Add(giftCardItem);

                    if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments)
                    {
                        // Calculate the tax for gift card item
                        this.Application.Services.Tax.CalculateTax(retailTransaction);
                    }
                }
            }
        }

        /// <summary>
        /// Void a gift card deposit line item.
        /// </summary>
        /// <param name="voided">Return true if sucessful, false otherwise.</param>
        /// <param name="comment">Error text if failed.</param>
        /// <param name="gcLineItem">Gift card line item.</param>
        public void VoidAddToGiftCard(ref bool voided, ref string comment, IGiftCardLineItem gcLineItem)
        {
            LogMessage("Voiding money addition to gift card.",
                LSRetailPosis.LogTraceLevel.Trace,
                "GiftCard.VoidGiftCardDeposit");

            if (gcLineItem == null)
                throw new ArgumentNullException("gcLineItem");

            decimal balance = 0;

            if (this.Application.TransactionServices.CheckConnection())
            {
                this.Application.TransactionServices.AddToGiftCard(ref voided, ref comment, ref balance, gcLineItem.SerialNumber,
                    gcLineItem.StoreId, gcLineItem.TerminalId, gcLineItem.StaffId, gcLineItem.TransactionId, gcLineItem.ReceiptId,
                    ApplicationSettings.Terminal.StoreCurrency, decimal.Negate(gcLineItem.Amount), DateTime.Now);
            }

            // Unlock the gift card
            bool released = false;
            UnlockGiftCard(ref released, ref comment, gcLineItem);
        }

        /// <summary>
        /// Get text to display value of the policy to user.
        /// </summary>        
        /// <param name="giftCardPolicyType">Type of the policy.</param>
        /// <param name="value">Value of the policy.</param>
        /// <param name="giftCardLineItem">This parameter should contain policies values to display card status.</param>
        /// <returns>Text to display.</returns>
        public string GetGiftCardPolicyAsDisplayText(GiftCardPolicyType giftCardPolicyType, object value, IGiftCardLineItem giftCardLineItem)
        {
            switch (giftCardPolicyType)
            {
                case GiftCardPolicyType.PolicyCardStatus:
                    return this.GetGiftCardPolicyGiftCardStatusAsText((GiftCardStatus)value, giftCardLineItem);
                case GiftCardPolicyType.PolicyMaxBalanceAllowed:
                    return this.GetGiftCardPolicyAmountRestrictionsAsText((decimal)value);
                case GiftCardPolicyType.PolicyMinReloadAmountAllowed:
                    return this.GetGiftCardPolicyAmountRestrictionsAsText((decimal)value);
                case GiftCardPolicyType.PolicyNonReloadable:
                    return this.GetGiftCardPolicyReloadableAsText((bool)value);
                case GiftCardPolicyType.PolicyOneTimeRedemption:
                    return this.GetGiftCardPolicyOneTimeRedemptionAsText((bool)value);
                case GiftCardPolicyType.PolicyActiveFrom:
                    return this.GetGiftCardPolicyActiveFromAsText((DateTime)value);
                case GiftCardPolicyType.PolicyExpirationDate:
                    return this.GetGiftCardPolicyExpirationDateAsText((DateTime)value);
                default:
                    return value == null ? null : value.ToString();
            }

        }
        /// <summary>
        /// Returns status of the gift card as text.
        /// </summary>
        /// <param name="policyGiftCardStatus">Status of the gift card.</param>
        /// <param name="giftCardLineItem">This parameter should contain policies values to display card status.</param>
        /// <returns>Text to display</returns>
        private string GetGiftCardPolicyGiftCardStatusAsText(GiftCardStatus policyGiftCardStatus, IGiftCardLineItem giftCardLineItem)
        {
            switch (policyGiftCardStatus)
            {
                case GiftCardStatus.Active:
                    {
                        string retVal = ApplicationLocalizer.Language.Translate(55419); // Active
                        if (DateTime.Now < giftCardLineItem.PolicyActivationDate)
                        {
                            retVal = ApplicationLocalizer.Language.Translate(55431); // Not active
                        }
                        if (giftCardLineItem.PolicyExpirationDate != DateTime.MinValue && giftCardLineItem.PolicyExpirationDate != LSRetailPosis.DevUtilities.Utility.POSNODATE && DateTime.Now > giftCardLineItem.PolicyExpirationDate)
                        {
                            retVal = ApplicationLocalizer.Language.Translate(55432); // Expired
                        }

                        return retVal;
                    }
                case GiftCardStatus.Expired:
                    {
                        string retVal = ApplicationLocalizer.Language.Translate(55420); // Redeemed
                        if (giftCardLineItem.PolicyExpirationDate != DateTime.MinValue && giftCardLineItem.PolicyExpirationDate != LSRetailPosis.DevUtilities.Utility.POSNODATE && DateTime.Now > giftCardLineItem.PolicyExpirationDate)
                        {
                            retVal = ApplicationLocalizer.Language.Translate(55432); // Expired
                        }

                        return retVal;
                    }
                case GiftCardStatus.Closed: return ApplicationLocalizer.Language.Translate(55421); // Closed
                default:
                    return String.Empty;
            }
        }


        /// <summary>
        /// Returns active from gift card policy value as text.
        /// </summary>
        /// <param name="policyActiveFrom">Date and time when card would be activated.</param>
        /// <returns>Text to display.</returns>
        public string GetGiftCardPolicyActiveFromAsText(DateTime policyActiveFrom)
        {
            return (policyActiveFrom == DateTime.MinValue || policyActiveFrom == LSRetailPosis.DevUtilities.Utility.POSNODATE) ? String.Empty : policyActiveFrom.ToString();
        }

        /// <summary>
        /// Returns expiration date gift card policy value as text.
        /// </summary>
        /// <param name="policyExpirationDate">Date when card will expire.</param>
        /// <returns>Text to display.</returns>
        public string GetGiftCardPolicyExpirationDateAsText(DateTime policyExpirationDate)
        {
            var retValue = policyExpirationDate.ToLocalTime().ToShortDateString();
            if (policyExpirationDate == DateTime.MinValue)
            {
                retValue = String.Empty;
            }
            if (policyExpirationDate == LSRetailPosis.DevUtilities.Utility.POSNODATE)
            {
                retValue = ApplicationLocalizer.Language.Translate(55430); /*Unlimited*/
            }
            return retValue;
        }

        /// <summary>
        /// Displays if gift card is reloadable or not.
        /// </summary>
        /// <param name="policyNonReloadable">Indicates if gift card is Non-reloadable.</param>
        /// <returns>Text to display: "yes" if gift card is reloadable and "no" if not.</returns>
        public string GetGiftCardPolicyReloadableAsText(bool policyNonReloadable)
        {
            return policyNonReloadable ? LSRetailPosis.ApplicationLocalizer.Language.Translate(55434) /*Disallowed*/ : LSRetailPosis.ApplicationLocalizer.Language.Translate(55433) /*Allowed*/;
        }

        /// <summary>
        /// Returns OneTimeRedemption gift card policy value as text.
        /// </summary>
        /// <param name="policyOneTimeRedemption">Indicates if gift card is one-time redemtion only.</param>
        /// <returns>Text to display.</returns>
        public string GetGiftCardPolicyOneTimeRedemptionAsText(bool policyOneTimeRedemption)
        {
            return policyOneTimeRedemption ? ApplicationLocalizer.Language.Translate(55426) /*One-time*/ : ApplicationLocalizer.Language.Translate(55427)  /*Many times*/;
        }

        /// <summary>
        /// Returns amount restrictions gift card policy value as text.
        /// </summary>
        /// <param name="policyMinReloadAmountAllowed"></param>
        /// <returns>Text to display.</returns>
        public string GetGiftCardPolicyAmountRestrictionsAsText(decimal policyMinReloadAmountAllowed)
        {
            return policyMinReloadAmountAllowed == Decimal.Zero ? ApplicationLocalizer.Language.Translate(55430) /*Unlimited*/
                                                                : GiftCard.InternalApplication.Services.Rounding.Round(policyMinReloadAmountAllowed, true);
        }


        /// <summary>
        /// Log a message to the file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="traceLevel"></param>
        /// <param name="args"></param>
        private void LogMessage(string message, LogTraceLevel traceLevel, params object[] args)
        {
            ApplicationLog.Log(this.GetType().Name, string.Format(message, args), traceLevel);
        }

        /// <summary>
        /// Inits GiftCertificateItem from the default gift card item specified in the RetailParameters.GiftCardItem field.
        /// </summary>
        /// <param name="giftCardItem">Gift card item.</param>
        private void InitGiftCardItem(GiftCertificateItem giftCardItem)
        {
            giftCardItem.ItemId = ApplicationSettings.Terminal.ItemToGiftCard;
            this.Application.Services.Item.ProcessItem(giftCardItem, true);
            giftCardItem.ItemId = string.Empty; // by design
        }

        #endregion

        private static bool CanIssueGiftCardInConsumerEfd(IPosTransaction posTransaction)
        {
            var retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction != null && ApplicationSettings.Terminal.OperationMode == Terminal.RetailTerminalOperationMode.EFDocConsumer)
            {
                return !retailTransaction.SaleItems.Any(i => !(i is GiftCertificateItem) && !i.Voided);
            }

            return true;
        }
    }
}
