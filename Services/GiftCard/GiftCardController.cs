/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.Common;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.HardwareProfiles;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using LSRetailPosis.Transaction.Line.TenderItem;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.TransactionServices;
using LSRetailPosis.Settings.FunctionalityProfiles;

namespace Microsoft.Dynamics.Retail.Pos.GiftCard
{
    // Get all text through the Translation function in the ApplicationLocalizer
    // TextID's for gift cards validation meessages are reserved at 107000 - 107100
    // In use now are ID's: 107000 - 107011
    sealed class GiftCardController : INotifyPropertyChanged
    {       
        #region Types

        internal enum ContextType
        {
            GiftCardIssue,
            GiftCardPayment,
            GiftCardAddTo,
            GiftCardBalance
        }

        #endregion

        #region Fields

        private string validatedCardNumber;
        private decimal validatedCardBalance;
        private string validatedCardCurrency;
        private bool isCardBalanceChecked;
        private string policyAccountingCurrency;
        private bool isGiftCardPoliciesChecked;        
        
        private PosTransaction Transaction;
        private Tender tenderInfo;

        private InputBoxEnum activeField = InputBoxEnum.Amount;
        private decimal amount;
        private decimal balance;
        private GiftCardStatus policyGiftCardStatus;
        private decimal policyMinReloadAmountAllowed;
        private decimal policyMaxBalanceAllowed;
        private bool policyNonReloadable;
        private bool policyOneTimeRedemption;
        private DateTime policyExpirationDate;
        private DateTime policyActiveFrom;
        private string cardNumber;

        private decimal? proposedPaymentAmount;
                
        #endregion

        #region Properties

        /// <summary>
        /// Get controller context.
        /// </summary>
        public ContextType Context { get; private set; }

        /// <summary>
        /// Get or set GiftCard number.
        /// </summary>
        public string CardNumber
        {
            get { return this.cardNumber; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // Save the number
                    this.cardNumber = value;
                    OnPropertyChanged("CardNumber");
                }
            }
        }

        /// <summary>
        /// Get GiftCard amount captured from UI.
        /// </summary>
        public decimal Amount
        {
            get { return this.amount; }
            set
            {
                if (this.amount != value)
                {
                    this.amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        /// <summary>
        /// Get GiftCard balance.
        /// </summary>
        public decimal Balance
        {
            get { return this.balance; }
            private set
            {
                if (this.balance != value)
                {
                    this.balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        /// <summary>
        /// Gets the proposed payment amount.
        /// </summary>
        public decimal ProposedPaymentAmount 
        {
            get
            {
                return proposedPaymentAmount ?? TransactionAmount;
            }

            private set
            {
                proposedPaymentAmount = value;
            }
        }

        /// <summary>
        /// Get GiftCard currency.
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        ///  Policy face value.
        /// </summary>
        public decimal FaceValue { get; private set; }
        /// <summary>
        /// Get amount of the transaction.
        /// </summary>
        public decimal TransactionAmount { get; private set; }

        /// <summary>
        /// Get maximum transaction amount allowed (up to gift card balance if the balance has already been checked).
        /// </summary>
        public decimal MaxTransactionAmountAllowed
        {
            get 
            {
                if (!isCardBalanceChecked)
                {
                    return this.TransactionAmount;
                }
                else
                {
                    return Math.Min(this.TransactionAmount, this.Balance);
                }
            }
        }

        /// <summary>
        /// Gets the tender info.
        /// </summary>
        internal Tender TenderInfo
        {
            get { return this.tenderInfo; }
        }

        /// <summary>
        /// Gets or sets the currently active field on the form
        /// </summary>
        public InputBoxEnum ActiveField
        {
            get { return this.activeField; }
            set
            {
                if (this.activeField != value)
                {
                    this.activeField = value;
                    OnPropertyChanged("ActiveField");
                }
            }
        }

        /// <summary>
        /// Gets the bool representing if the OK button should be enabled
        /// </summary>
        public bool Validated
        {
            get
            {
                bool result = false;

                switch (this.Context)
                {
                    case GiftCardController.ContextType.GiftCardPayment:
                    case GiftCardController.ContextType.GiftCardAddTo:
                        result = !string.IsNullOrWhiteSpace(CardNumber) && (Amount != 0);
                        break;

                    case GiftCardController.ContextType.GiftCardIssue:
                        result = !string.IsNullOrWhiteSpace(CardNumber) && (Amount != 0
                            // Zero amount is allowed to issue new gift card in the return sale transaction and refund to it in the same transaction
                            || (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments && CurrentTransacationIsReturnSale()));
                        break;

                    case GiftCardController.ContextType.GiftCardBalance:
                        result = !string.IsNullOrWhiteSpace(CardNumber);
                        break;

                    default:
                        throw new InvalidEnumArgumentException(this.Context.ToString());
                }

                return result;
            }
        }

        /// <summary>
        /// Gets gift card Status policy.
        /// </summary>
        public GiftCardStatus PolicyGiftCardStatus
        {
            get { return this.policyGiftCardStatus; }
            set
            {
                if (this.policyGiftCardStatus != value)
                {
                    this.policyGiftCardStatus = value;
                    OnPropertyChanged("PolicyGiftCardStatus");
                }
            }
        }
          /// <summary>
        /// Gets Active From gift card policy.
        /// </summary>
        public DateTime PolicyActiveFrom
        {
            get { return this.policyActiveFrom; }
            set
            {
                if (this.policyActiveFrom != value)
                {
                    this.policyActiveFrom = value;
                    OnPropertyChanged("PolicyActiveFrom");
                }
            }
        }
       	/// <summary>
        /// Gets Expiration date gift card policy.
        /// </summary>
        public DateTime PolicyExpirationDate
        {
            get { return this.policyExpirationDate; }
            set
            {
                if (this.policyExpirationDate != value)
                {
                    this.policyExpirationDate = value;
                    OnPropertyChanged("PolicyExpirationDate");
                }
            }
        }
        /// <summary>
        /// Gets One-time redemption gift card policy.
        /// </summary>
        public bool PolicyOneTimeRedemption
        {
            get { return this.policyOneTimeRedemption; }
            set
            {
                if (this.policyOneTimeRedemption != value)
                {
                    this.policyOneTimeRedemption = value;
                    OnPropertyChanged("PolicyOneTimeRedemption");
                }
            }
        }
        /// <summary>
        /// Gets Non-reloadable gift card policy.
        /// </summary>
        public bool PolicyNonReloadable
        {
            get { return this.policyNonReloadable; }
            set
            {
                if (this.policyNonReloadable != value)
                {
                    this.policyNonReloadable = value;
                    OnPropertyChanged("PolicyNonReloadable");
                }
            }
        }
        /// <summary>
        /// Gets minimum reload amount allowed gift card policy.
        /// </summary>
        public decimal PolicyMinReloadAmountAllowed
        {
            get { return this.policyMinReloadAmountAllowed; }
            set
            {
                if (this.policyMinReloadAmountAllowed != value)
                {
                    this.policyMinReloadAmountAllowed = value;
                    OnPropertyChanged("PolicyMinReloadAmountAllowed");
                }
            }
        }
        /// <summary>
        /// Gets maximum balance allowed gift card policy.
        /// </summary>
        public decimal PolicyMaxBalanceAllowed
        {
            get { return this.policyMaxBalanceAllowed; }
            set
            {
                if (this.policyMaxBalanceAllowed != value)
                {
                    this.policyMaxBalanceAllowed = value;
                    OnPropertyChanged("PolicyMaxBalanceAllowed");
                }
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="context">Context of the gift card form</param>
        /// <param name="posTransaction">Transaction object.</param>
        public GiftCardController(ContextType context, PosTransaction posTransaction)
            : this(context, posTransaction, null)
        {            
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context of the gift card form</param>
        /// <param name="posTransaction">Transaction object.</param>
        /// <param name="tenderInfo">Tender information about GC (Required for Payment Context) </param>
        public GiftCardController(ContextType context, PosTransaction posTransaction, Tender tenderInfo)
        {
            this.Context = context;
            this.Transaction = posTransaction;
            this.tenderInfo = tenderInfo;
            this.CardNumber = string.Empty;

            // Get the balance of the transaction.
            IRetailTransaction retailTransaction = Transaction as IRetailTransaction;
            CustomerPaymentTransaction customerPaymentTransaction = Transaction as CustomerPaymentTransaction;

            if (retailTransaction != null)
            {
                TransactionAmount = retailTransaction.TransSalePmtDiff;

                decimal paymentAmount = TransactionAmount;
                if (this.Context != ContextType.GiftCardBalance && TenderOperation.AdjustProposedRefundAmount(ref paymentAmount, tenderInfo.TenderID, (RetailTransaction)retailTransaction))
                {
                    ProposedPaymentAmount = paymentAmount;
                }
            }
            else if (customerPaymentTransaction != null)
            {
                TransactionAmount = customerPaymentTransaction.TransSalePmtDiff;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the title of the form based on current context.
        /// </summary>
        /// <returns></returns>
        public string GetFormTitle()
        {
            string result = string.Empty;

            switch (Context)
            {
                case ContextType.GiftCardIssue:
                    result = ApplicationLocalizer.Language.Translate(55400);
                    break;

                case ContextType.GiftCardAddTo:
                    result = ApplicationLocalizer.Language.Translate(55407);
                    break;

                case ContextType.GiftCardBalance:
                    result = ApplicationLocalizer.Language.Translate(55408);
                    break;

                case ContextType.GiftCardPayment:
                    result = ApplicationLocalizer.Language.Translate(55409);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Validate and lock a gift card.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        public void ValidateGiftCardPayment()
        {
            if (!ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments || !(Transaction is RetailTransaction))
            {
                AssertNotAlreadyAdded();
            }

            // Called this method on already validated number. Save TS call.
            if (!cardNumber.Equals(validatedCardNumber, StringComparison.OrdinalIgnoreCase))
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (GiftCard.InternalApplication.TransactionServices.CheckConnection())
                {
                    // If used another card, release currenty locked gift card from HQ.
                    if (!string.IsNullOrEmpty(validatedCardNumber))
                    {
                        VoidGiftCardPayment();
                    }

                    string comment = string.Empty;
                    bool success = false;

                    validatedCardCurrency = string.Empty;
                    validatedCardBalance = 0M;

                    bool skipReserveValidation = GiftCardIsIssuedInCurrentTransacation();

                    // Validate gift card
                    // Gift card is locked at HQ if transaction is successful
                    GiftCard.InternalApplication.TransactionServices.ValidateGiftCard(ref success, ref comment, ref validatedCardCurrency, ref validatedCardBalance,
                        cardNumber, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, skipReserveValidation);

                    if (success)
                    {
                        validatedCardNumber = cardNumber;
                        Currency = validatedCardCurrency;
                        validatedCardBalance = this.FromGiftCardCurrency(validatedCardBalance);
                    }
                    else
                    {
                        throw new GiftCardException(comment);
                    }
                }
            }
        }

        /// <summary>
        /// Void/unlock the existing validate/locked card.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        public void VoidGiftCardPayment()
        {
            // There is no card to void.
            if (string.IsNullOrEmpty(validatedCardNumber))
            {
                return;
            }

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (GiftCard.InternalApplication.TransactionServices.CheckConnection())
                {
                    bool success = false;
                    string comment = string.Empty;

                    GiftCard.InternalApplication.TransactionServices.VoidGiftCardPayment(ref success, ref comment, validatedCardNumber, ApplicationSettings.Terminal.StoreId,
                        ApplicationSettings.Terminal.TerminalId);

                    if (success)
                    {
                        validatedCardNumber = string.Empty;
                    }
                    else
                    {
                        throw new GiftCardException(comment);
                    }
                }
            }
            catch (PosisException ex)
            {
                // Throw custom exception if TS call failed.
                string message = ApplicationLocalizer.Language.Translate(55415);

                throw new GiftCardException(message, ex);
            }
        }

        /// <summary>
        /// Validate amount with tender rules.
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="GiftCardException"></exception>
        public void ValidateTenderAmount()
        {
            if (!string.IsNullOrEmpty(validatedCardNumber))
            {
                try
                {
                    if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments && (Transaction is RetailTransaction))
                    {
                        ValidatePayByGiftCard();
                    }
                    CheckGiftCardPolicies();

                    ValidatePolicyExpirationDate();
                    ValidatePolicyGiftCardStatus();
                    ValidatePolicyActiveFrom();
                    if (amount < 0)
                    {
                        ValidatePolicyNonReloadable();
                        ValidatePolicyMaxBalanceAllowed(validatedCardBalance - amount);
                    }
                }
                catch (GiftCardException)
                {
                    VoidGiftCardPayment();
                    throw;
                }

                // First check fo gift card balance.
                if (amount > validatedCardBalance)
                {
                    // Release the currently locked gift card from HQ since payment is not validated
                    VoidGiftCardPayment();
                    //change to this by Yonathan 28/12/2022 to localize the language
                    throw new GiftCardException("Nominal transaksi yang diinput salah, silakan coba transaksi lagi dengan penginputan nominal sesuai nilai voucher");
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(55411));
                }
                else
                {
                    TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, amount, true, TransactionAmount);
                    if (!string.IsNullOrEmpty(tenderReq.ErrorText))
                    {
                        // Release the currently locked gift card from HQ since payment is not validated
                        VoidGiftCardPayment();

                        throw new GiftCardException(tenderReq.ErrorText);
                    }

                    CardNumber = validatedCardNumber;
                    Balance = validatedCardBalance;
                    Amount = amount;
                    Currency = validatedCardCurrency;
                }
            }
        }

        /// <summary>
        /// Query balance of a gift card.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        public void CheckGiftCardBalance()
        {
            if (GiftCard.InternalApplication.TransactionServices.CheckConnection())
            {
                bool succeeded = false;
                string comment = string.Empty;
                string currencyCode = string.Empty;
                decimal giftCardBalance = 0M;

                Balance = 0;

                GiftCard.InternalApplication.TransactionServices.GetGiftCardBalance(ref succeeded, ref comment, ref currencyCode, ref giftCardBalance, CardNumber);
                
                if (succeeded)
                {
                    Currency = currencyCode;
                    Balance = this.FromGiftCardCurrency(giftCardBalance);
                    isCardBalanceChecked = true;
                    Amount = Balance; //add by Yonathan to set the value of amount same as the balance.
                }
                else
                {
                    isCardBalanceChecked = false;
                    throw new GiftCardException(comment);
                }
            }
        }
              

        /// <summary>
        /// Query policies of a gift card.
        /// </summary>
        /// <param name="skipCheckConnection">Skip connection check sign.</param>
        /// <exception cref="GiftCardException"></exception>
        public void CheckGiftCardPolicies(bool skipCheckConnection = false)
        {
            // Gift card policies shouldn't be checked if UseGiftCardPolicies parameter is no set.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies &&  (skipCheckConnection || GiftCard.InternalApplication.TransactionServices.CheckConnection()))
            {
                bool succeeded = false;
                string comment = string.Empty;
                GiftCardStatus status = GiftCardStatus.Active;
                DateTime validFrom = DateTime.MinValue;
                DateTime validTo = DateTime.MinValue;
                bool oneTimeRedemption = false;
                bool nonReloadable = false;
                decimal minReloadAmountAllowed = 0m;
                decimal maxBalanceAllowed = 0m;

                // Get the default gift card policies through the transaction services.
                GiftCard.InternalApplication.TransactionServices.GetGiftCardPolicies(
                    ref succeeded,
                    ref comment,
                    cardNumber,
                    ref status,
                    ref validFrom,
                    ref validTo,
                    ref oneTimeRedemption,
                    ref nonReloadable,
                    ref minReloadAmountAllowed,
                    ref maxBalanceAllowed,
                    ref policyAccountingCurrency);

                if (succeeded)
                {
		            isGiftCardPoliciesChecked = true;
                    PolicyGiftCardStatus = status;
                    PolicyActiveFrom = validFrom;
                    PolicyExpirationDate = validTo;
                    PolicyOneTimeRedemption = oneTimeRedemption;
                    PolicyNonReloadable = nonReloadable;
                    PolicyMinReloadAmountAllowed = this.FromAccountingCurrency(minReloadAmountAllowed);
                    PolicyMaxBalanceAllowed = this.FromAccountingCurrency(maxBalanceAllowed);
                }
                else
                {
                    isGiftCardPoliciesChecked = false;
                    throw new GiftCardException(comment);
                }
            }
        }

        /// <summary>
        /// Query default policies of a gift card.
        /// </summary>
        /// <param name="skipCheckConnection">Skip connection check sign.</param>
        /// <exception cref="GiftCardException"></exception>
        public void CheckGiftCardDefaultPolicies(bool skipCheckConnection = false)
        {
            // Gift card policies shouldn't be checked if UseGiftCardPolicies parameter is no set.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && (skipCheckConnection || GiftCard.InternalApplication.TransactionServices.CheckConnection()))
            {
                bool succeeded = false;
                string comment = string.Empty;
                decimal faceValue = 0m;
                decimal activationPeriod = 0m;
                int validityPeriod  = 0;
                DateTime fixedExpirationDate = DateTime.MinValue;
                bool oneTimeRedemption = false;
                bool nonReloadable = false;
                decimal minReloadAmountAllowed = 0m;
                decimal maxBalanceAllowed = 0m;

                // Get the default gift card policies through the transaction services.
                GiftCard.InternalApplication.TransactionServices.GetGiftCardPolicySet(
                    ref succeeded,
                    ref comment,
                    cardNumber,
                    ref faceValue,
                    ref activationPeriod,
                    ref validityPeriod,
                    ref fixedExpirationDate,
                    ref oneTimeRedemption,
                    ref nonReloadable,
                    ref minReloadAmountAllowed,
                    ref maxBalanceAllowed,
                    ref policyAccountingCurrency);

                if (succeeded)
                {                    
                    this.isGiftCardPoliciesChecked = true;
                    FaceValue = faceValue;
                    if (FaceValue != 0)
                    {
                        Amount = this.FromAccountingCurrency(FaceValue);
                    }                    
                    PolicyGiftCardStatus = GiftCardStatus.Active;
                    PolicyActiveFrom = (activationPeriod != 0m) ? DateTime.Now.AddHours((double)activationPeriod) : DateTime.Now;
                    PolicyExpirationDate = (fixedExpirationDate != DateTime.MinValue && fixedExpirationDate != LSRetailPosis.DevUtilities.Utility.POSNODATE) 
                        ? fixedExpirationDate 
                        : ((validityPeriod != 0m) ? DateTime.Now.AddDays((double)validityPeriod) : LSRetailPosis.DevUtilities.Utility.POSNODATE);
                    PolicyOneTimeRedemption = oneTimeRedemption;
                    PolicyNonReloadable = nonReloadable;
                    PolicyMinReloadAmountAllowed = this.FromAccountingCurrency(minReloadAmountAllowed);
                    PolicyMaxBalanceAllowed = this.FromAccountingCurrency(maxBalanceAllowed);                   
                }
                else
                {                    
                    this.isGiftCardPoliciesChecked = false;
                    throw new GiftCardException(comment);
                }
            }
        }

        /// <summary>
        /// Print the gift card balance receipt.
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <exception cref="GiftCardException"></exception>
        public void PrintGiftCardBalance()
        {
            if (Printer.DeviceType == DeviceTypes.None && (Functions.CountryRegion == SupportedCountryRegion.RU && !GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled()))
            {
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(10060));
            }

            // If balance already queried skip TS call.
            if ((string.IsNullOrEmpty(CardNumber)) || (!cardNumber.Equals(CardNumber, StringComparison.OrdinalIgnoreCase)))
            {
                this.CheckGiftCardBalance();
                this.CheckGiftCardPolicies();
            }

            IGiftCardLineItem gcItem = this.CreateGiftCardLineItemWithPolicies();
            
            // Required for receipt header
            ((IPosTransactionV2)Transaction).EndDateTime = DateTime.Now;

            if (Functions.CountryRegion == SupportedCountryRegion.RU && GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            {
                var giftCardsList = new List<IGiftCardLineItem>();
                giftCardsList.Capacity = 1;
                giftCardsList.Add(gcItem);
                GiftCard.InternalApplication.Services.Peripherals.FiscalPrinter.PrintGiftCardsBalance(giftCardsList);
            }
            else
            {
                GiftCard.InternalApplication.Services.Printing.PrintGiftCertificate(FormType.GiftCertificate, Transaction, gcItem, false);
            }
        }

        /// <summary>
        /// Issue a gift card
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="amount"></param>
        /// <exception cref="GiftCardException"></exception>
        public void IssueGiftCard()
        {
            if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments && (Transaction is RetailTransaction))
            {
                ValidateGiftCardIssue();
            }
            else
            {
                AssertNotAlreadyAdded();
            }

            // Begin by checking if there is a connection to the Transaction Service
            if (GiftCard.InternalApplication.TransactionServices.CheckConnection())
            {
                CheckGiftCardDefaultPolicies();

                ValidatePolicyMinReloadAmountAllowed();
                ValidatePolicyMaxBalanceAllowed(amount);

                bool succeeded = false;
                string comment = string.Empty;
                
                // Issue the gift card through the transaction services and add it to the transaction
                // Gift card is locked at HQ if transaction is successful
                GiftCard.InternalApplication.TransactionServices.IssueGiftCard(ref succeeded, ref comment, ref cardNumber,
                        ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
                        Transaction.TransactionId, Transaction.ReceiptId, ApplicationSettings.Terminal.StoreCurrency,
                        amount, DateTime.Now);

                if (!succeeded || cardNumber.Length == 0)
                {
                    throw new GiftCardException(comment);
                }
                else
                {
                    CardNumber = cardNumber;
                    Amount = amount;
                    Balance = amount;
                    Currency = ApplicationSettings.Terminal.StoreCurrency;
                }
            }
        }

        /// <summary>
        /// Add money to gift card.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="amount"></param>
        /// <exception cref="GiftCardException"></exception>
        public void AddToGiftCard()
        {
            if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments && (Transaction is RetailTransaction))
            {
                PreValidateAddToGiftCard(); // run once more just in case method could be called form some other places
                ValidateAddToGiftCard();
            }
            else
            {
                AssertNotAlreadyAdded();
            }

            // Begin by checking if there is a connection to the Transaction Service
            if (GiftCard.InternalApplication.TransactionServices.CheckConnection())
            {
                bool succeeded = false;
                string comment = string.Empty;
                decimal currentBalance = 0;

                // Check gift card balance always before adding to the transaction.
                // This is done to recalculate the balance and currency even if the gift card is changed without user clicking on Check Balance.
                this.CheckGiftCardBalance();
                CheckGiftCardPolicies(true);

                ValidatePolicyExpirationDate();
                ValidatePolicyGiftCardStatus(); 
                ValidatePolicyNonReloadable();
                ValidatePolicyActiveFrom();
                ValidatePolicyMinReloadAmountAllowed();
                ValidatePolicyMaxBalanceAllowed(Balance + amount);

                // Deposit money to gift card through the transaction services and add it to the transaction
                // Gift card is locked at HQ if transaction is successful
                GiftCard.InternalApplication.TransactionServices.AddToGiftCard(ref succeeded, ref comment, ref currentBalance, cardNumber,
                    ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
                    Transaction.TransactionId, Transaction.ReceiptId, this.Currency, this.ToGiftCardCurrency(amount), DateTime.Now);

                if (!succeeded)
                {
                    throw new GiftCardException(comment);
                }
                else
                {
                    CardNumber = cardNumber;
                    Amount = amount;
                    Balance = this.FromGiftCardCurrency(currentBalance);
                }
            }
        }

        /// <summary>
        /// Pre validate current transaction before add to gift card operation.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        public void PreValidateAddToGiftCard()
        {
            RetailTransaction retailTransaction = Transaction as RetailTransaction;

            if (retailTransaction != null)
            {
                if (retailTransaction.SaleIsReturnSale)
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107004));
                }

                if (retailTransaction.SaleItems != null &&
                    retailTransaction.SaleItems.OfType<ISaleLineItem>().Any(l => (!l.Voided && l.Quantity < 0)))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107004));
                }

                // Add to gift card operation is not allowed, if transaction already contains any gift card payment.
                if (retailTransaction.TenderLines != null &&
                    retailTransaction.TenderLines.OfType<IGiftCardTenderLineItem>().Any(l => !l.Voided))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107000));
                }
            }
        }
        /// <summary>
        /// Get result of last gift card policy check operation.
        /// </summary>
        /// <returns>True - if last operation was succeed, otherwise returns false.</returns>
        public bool IsGiftCardPoliciesChecked()
        {
            return ApplicationSettings.Terminal.UseGiftCardPolicies && isGiftCardPoliciesChecked;
        }

        /// <summary>
        /// Indicates is gift card amount text field should be editable or not.
        /// </summary>
        /// <returns>True - if gift car amount field control should be editable, otherwise returns false.</returns>
        public bool IsGiftCardAmountEditable()
        {
            return !ApplicationSettings.Terminal.UseGiftCardPolicies || !Convert.ToBoolean(this.FaceValue);
        }


        /// <summary>
        /// Return IGiftCardLineItem with filled in policies values from GiftCardController
        /// </summary>
        /// <returns></returns>
        public IGiftCardLineItem CreateGiftCardLineItemWithPolicies()
        {
            var gcItem = GiftCard.InternalApplication.BusinessLogic.Utility.CreateGiftCardLineItem(ApplicationSettings.Terminal.StoreCurrency, GiftCard.InternalApplication.Services.Rounding, (IRetailTransaction)Transaction);
            gcItem.SerialNumber = CardNumber;            
            gcItem.Balance = Balance;
            if (ApplicationSettings.Terminal.UseGiftCardPolicies)
            {
                gcItem.PolicyGiftCardStatus = PolicyGiftCardStatus;
                gcItem.PolicyActivationDate = PolicyActiveFrom;
                gcItem.PolicyExpirationDate = PolicyExpirationDate;
                gcItem.PolicyMaxBalanceAllowed = PolicyMaxBalanceAllowed;
                gcItem.PolicyMinReloadAmountAllowed = PolicyMinReloadAmountAllowed;
                gcItem.PolicyNonReloadable = PolicyNonReloadable;
                gcItem.PolicyOneTimeRedemption = PolicyOneTimeRedemption;
            }

            return gcItem;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Check if Gift card is already added to transaction either as sale or payment.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <exception cref="GiftCardException"></exception>
        private void AssertNotAlreadyAdded()
        {
            // Once a Gift card is added to the transaction for any operation (Issue, AddTo, Payment), then
            // the same card cannot be added to the transaction again for any other operation.

            RetailTransaction retailTransaction = Transaction as RetailTransaction;
            CustomerPaymentTransaction customerPaymentTransaction = Transaction as CustomerPaymentTransaction;
            LinkedList<SaleLineItem> salesLines = null;
            LinkedList<TenderLineItem> tenderLines = null;

            if (retailTransaction != null)
            {
                tenderLines = retailTransaction.TenderLines;
                salesLines = retailTransaction.SaleItems;
            }
            else if (customerPaymentTransaction != null)
            {
                tenderLines = customerPaymentTransaction.TenderLines;
            }

            // Check for all sales lines of type GiftCertificateItem where they are not voided and the serial number matches the card number.
            if (salesLines != null
                && salesLines.OfType<IGiftCardLineItem>().Any(l => (!l.Voided) && string.Equals(l.SerialNumber, cardNumber, StringComparison.Ordinal)))
            {
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(4323));
            }

            // Check for all tender lines of type GiftCertificateTenderLineItem where they are not voided and the serial number matches the card number.
            if (tenderLines != null
                && tenderLines.OfType<IGiftCardTenderLineItem>().Any(l => (!l.Voided) && string.Equals(l.SerialNumber, cardNumber, StringComparison.Ordinal)))
            {
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(4323));
            }
        }

        /// <summary>
        /// Validates gift card issue operation.
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <param name="amount">Issue amount</param>
        /// <exception cref="GiftCardException"></exception>
        private void ValidateGiftCardIssue()
        {
            RetailTransaction retailTransaction = Transaction as RetailTransaction;

            if (retailTransaction != null)
            {
                if (amount != decimal.Zero &&
                    retailTransaction.SaleIsReturnSale)
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107003));
                }

                if (amount != decimal.Zero &&
                    retailTransaction.SaleItems != null &&
                    retailTransaction.SaleItems.OfType<ISaleLineItem>().Any(l => (!l.Voided && l.Quantity < 0)))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107003));
                }

                // Gift card issue operation is not allowed, if transaction already contains any gift card payment. 
                // Exception: zero amount card issue is allowed in the same transaction with with gift card refund oeprations.
                if (retailTransaction.TenderLines != null &&
                    retailTransaction.TenderLines.OfType<IGiftCardTenderLineItem>().Any(l => !l.Voided && !(amount == decimal.Zero && l.Amount < decimal.Zero)))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107000));
                }

                // Gift card issue operation is not allowed, if transaction already contains replanishment with the same gift card number (core behavior)
                if (retailTransaction.SaleItems != null &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => !l.Voided && string.Equals(l.SerialNumber, cardNumber, StringComparison.Ordinal)))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(4323));
                }
            }
        }

        #region Gift card policies validation Methods

        /// <summary>
        /// Validates gift card Expiration date policy.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePolicyExpirationDate()
        {
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && PolicyExpirationDate != LSRetailPosis.DevUtilities.Utility.POSNODATE && PolicyExpirationDate.Date < DateTime.Now.Date)
            {
                // The gift card is expired.
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(107007));
            }
        }
        
        /// <summary>
        /// Validates gift card status policy.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePolicyGiftCardStatus()
        {
            // Skip validation if UseGiftCardPolicies parameter is no set, otherwise validate if gift card is expired.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && PolicyGiftCardStatus != GiftCardStatus.Active)
            {
                // The gift card is expired.
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(107007));
            }
        }

        /// <summary>
        /// Validates gift card Active From policy.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePolicyActiveFrom()
        {
            // Skip validation if UseGiftCardPolicies parameter is no set, otherwise validate if gift active.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && PolicyActiveFrom > DateTime.Now && (amount > 0 || !GiftCardIsIssuedInCurrentTransacation()))
            {
                // The gift card is not activated.
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(107008));
            }
        }

        /// <summary>
        /// Validates non-reloadable gift card policy.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePolicyNonReloadable()
        {
            // Skip validation if UseGiftCardPolicies parameter is no set, otherwise validate Not-reloadable policy restriction.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && PolicyNonReloadable && (amount > 0 || !GiftCardIsIssuedInCurrentTransacation()))
            {
                // It is not allowed to add to this gift card.                
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(107009));
            }
        }

        /// <summary>
        /// Validates minimum reload amount gift card policy.
        /// </summary>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePolicyMinReloadAmountAllowed()
        {
            // Skip validation if UseGiftCardPolicies parameter is no set, otherwise validate Min. Reload Amount policy restriction.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && PolicyMinReloadAmountAllowed != 0m && amount != 0m && amount < PolicyMinReloadAmountAllowed)
            {
                // The amount to add to the gift card must not be less than {0}.
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(107010, PolicyMinReloadAmountAllowed.ToString("N")));
            }
        }

        /// <summary>
        /// Validates maximum balance gift card policy.
        /// </summary>
        /// <param name="currentBalace">Result balance.</param>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePolicyMaxBalanceAllowed(decimal resultBalace)
        {
            // Skip validation if UseGiftCardPolicies parameter is no set, otherwise validate Max Balance policy restriction.
            if (ApplicationSettings.Terminal.UseGiftCardPolicies && PolicyMaxBalanceAllowed != 0m && resultBalace > PolicyMaxBalanceAllowed)
            {
                // The gift card balance must not be greater than {0}.
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(107011, PolicyMaxBalanceAllowed.ToString("N")));
            }
        }

        #endregion

        /// <summary>
        /// Determines if current transacation is the return sale.
        /// </summary>
        /// <returns>True if current transacation is the return sale; otherwise false.</returns>
        private bool CurrentTransacationIsReturnSale()
        {
            var retailTransaction = Transaction as RetailTransaction;

            return (retailTransaction != null && retailTransaction.SaleIsReturnSale);
        }

        /// <summary>
        /// Determines is gift card issued in the current transacation.
        /// </summary>
        /// <returns>True if gift card is issued in the current transacation; otherwise false.</returns>
        private bool GiftCardIsIssuedInCurrentTransacation()
        {
            bool result = false;

            if (ApplicationSettings.Terminal.ProcessGiftCardsAsPrepayments && (Transaction is RetailTransaction))
            {
                RetailTransaction retailTransaction = (RetailTransaction)Transaction;

                // Gift card reserve validation should be skipped in case gift card is issued in the current transaction
                if (retailTransaction.SaleItems != null &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => !l.Voided && string.Equals(l.SerialNumber, cardNumber, StringComparison.Ordinal) && !l.AddTo))
                {
                    result = true;
                }
            }

            return result;
        }


        /// <summary>
        /// Validates add to gift card operation.
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <param name="amount">Replenishment amount</param>
        /// <exception cref="GiftCardException"></exception>
        private void ValidateAddToGiftCard()
        {
            RetailTransaction retailTransaction = Transaction as RetailTransaction;

            // Add to gift card operation is not allowed, if transaction already contains replanishment with the same gift card number (core behavior)
            if (retailTransaction != null &&
                retailTransaction.SaleItems != null &&
                retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => (!l.Voided) && string.Equals(l.SerialNumber, cardNumber, StringComparison.Ordinal)))
            {
                throw new GiftCardException(ApplicationLocalizer.Language.Translate(4323));
            }
        }

        /// <summary>
        /// Validates pay by gift card operation.
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <param name="amount">Issue amount</param>
        /// <exception cref="GiftCardException"></exception>
        private void ValidatePayByGiftCard()
        {
            RetailTransaction retailTransaction = Transaction as RetailTransaction;

            if (retailTransaction != null)
            {
                // Gift card payment is not allowed, if transaction contains any Gift card issue or Add to gift card operation. 
                // Exception: gift card refund is allowed in the same transaction with with the zero gift card issue operation.
                if (retailTransaction.SaleItems != null &&
                    retailTransaction.SaleItems.OfType<IGiftCardLineItem>().Any(l => !l.Voided && !(amount < decimal.Zero && l.Amount == decimal.Zero && !l.AddTo)))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(107001));
                }

                // Gift card payment is not allowed, if transaction contains any payment with the same gift card number (core behavior).
                if (retailTransaction.TenderLines != null &&
                    retailTransaction.TenderLines.OfType<IGiftCardTenderLineItem>().Any(l => (!l.Voided) && string.Equals(l.SerialNumber, cardNumber, StringComparison.Ordinal)))
                {
                    throw new GiftCardException(ApplicationLocalizer.Language.Translate(4323));
                }
            }
        }

        /// <summary>
        /// Converts the value from store currency to the gift card currency.
        /// </summary>
        /// <param name="value">The value in store currency.</param>
        /// <returns>The converted value in the gift card currency.</returns>
        private decimal ToGiftCardCurrency(decimal value)
        {
            return GiftCard.InternalApplication.Services.Currency.CurrencyToCurrency(
                ApplicationSettings.Terminal.StoreCurrency,
                Currency,
                value);
        }

        /// <summary>
        /// Converts the value from the gift card currency to store currency.
        /// </summary>
        /// <param name="value">The value in the gift card currency.</param>
        /// <returns>The converted value in store currency.</returns>
        private decimal FromGiftCardCurrency(decimal value)
        {
            return GiftCard.InternalApplication.Services.Currency.CurrencyToCurrency(
                Currency,
                ApplicationSettings.Terminal.StoreCurrency,
                value);
        }

        /// <summary>
        /// Converts the value from the gift card company accounting currency to store currency.
        /// </summary>
        /// <param name="value">The value in the gift card company accounting currency.</param>
        /// <returns>The converted value in store currency.</returns>
        private decimal FromAccountingCurrency(decimal value)
        {
            return GiftCard.InternalApplication.Services.Currency.CurrencyToCurrency(
                policyAccountingCurrency,
                ApplicationSettings.Terminal.StoreCurrency,
                value);
        }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
