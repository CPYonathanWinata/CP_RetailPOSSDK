/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Loyalty
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LSRetailPosis.DataAccess;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.AffiliationItem;
    using LSRetailPosis;
    using LSRetailPosis.Transaction.Line.TenderItem;
    using LSRetailPosis.POSProcesses.Common;
    using LSRetailPosis.POSControls;
    using LSRetailPosis.POSProcesses;

    /// <summary>
    /// Loyalty absctract factory class
    /// </summary>
    internal class LoyaltyHelper
    {
        private static LoyaltyHelper instance = new LoyaltyHelper();

        private LoyaltyHelper() { }

        public LoyaltyData LoyaltyData { get; set; }

        private IApplication application;

        /// <summary>
        /// Gets LoyaltyAbstractFactory instance
        /// </summary>
        internal static LoyaltyHelper Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
        public IApplication Application {
            get
            {
                return application; 
            }
            set
            {
                if (value != null && this.application != value)
                {
                    this.application = value;
                    this.LoyaltyData = LoyaltyData.GetInstance(value);
                }
            }
        }

        /// <summary>
        /// Creates loyalty card.
        /// </summary>
        /// <param name="cardInfo">Card info.</param>
        /// <returns>LoyaltyCard if card number is valid, else null.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="cardInfo"/> is null.</exception>
        public LoyaltyCard GetLoyaltyCard(ICardInfo cardInfo)
        {
            if (cardInfo == null)
            {
                throw new ArgumentNullException("cardInfo");
            }

            string cardNumber = cardInfo.CardEntryType == CardEntryTypes.MAGNETIC_STRIPE_READ
                        ? cardInfo.Track2Parts[0]
                        : cardInfo.CardNumber;

            LoyaltyCard loyaltyCard = null;

            if (!string.IsNullOrEmpty(cardNumber))
            {
                loyaltyCard = this.LoyaltyData.GetLoyaltyCard(cardNumber);
            }

            return loyaltyCard;
        }

        public ICardInfo GetCardForTransactionTender(IRetailTransaction retailTransaction, string tenderID)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (string.IsNullOrEmpty(tenderID))
            {
                throw new ArgumentNullException("tenderID");
            }
            
            ICardInfo cardInfo = this.Application.BusinessLogic.Utility.CreateCardInfo();

            if (LoyaltyHelper.CheckTransactionHasLoyalty(retailTransaction) && retailTransaction.LoyaltyItem.TenderType != LoyaltyCardTenderType.NoTender)
            {
                cardInfo.CardNumber = retailTransaction.LoyaltyItem.LoyaltyCardNumber;
            }

            cardInfo.CardEntryType = CardEntryTypes.MANUALLY_ENTERED;
            cardInfo.CardType = CardTypes.LoyaltyCard;
            cardInfo.TenderTypeId = tenderID;
            decimal proposedAmount = retailTransaction.TransSalePmtDiff;
            TenderOperation.AdjustProposedRefundAmount(ref proposedAmount, cardInfo.TenderTypeId, (RetailTransaction)retailTransaction);
            cardInfo.Amount = proposedAmount;

            return LoyaltyGUI.PayByLoyalty(cardInfo);
        }

        /// <summary>
        /// Creates Loyalty item.
        /// </summary>
        /// <param name="loyaltyCard">Loyalty card.</param>
        /// <returns>Loyalty item.</returns>
        /// <exception cref="ArgumentNullException">Throws exception if <paramref name="loyaltyCard"/> is null.</exception>
        public ILoyaltyItem GetLoyaltyItem(LoyaltyCard loyaltyCard)
        {
            if (loyaltyCard == null)
            {
                throw new ArgumentNullException("loyaltyCard");
            }

            ILoyaltyItem loyaltyItem = this.Application.BusinessLogic.Utility.CreateLoyaltyItem();

            loyaltyItem.LoyaltyCardNumber = loyaltyCard.Number;
            loyaltyItem.TenderType = loyaltyCard.TenderType;
            loyaltyItem.CustomerPartyNumber = loyaltyCard.PartyNumber;

            return loyaltyItem;
        }
    
        /// <summary>
        /// Creates customer entity of the loyalty.
        /// </summary>
        /// <param name="partyNumber">Customer party number.</param>
        /// <returns>Customer if it exists, else null.</returns>
        public Contracts.DataEntity.ICustomer GetCustomer(string partyNumber)
        {
            Contracts.DataEntity.ICustomer customer = null;

            if (!string.IsNullOrEmpty(partyNumber))
            {
                // first we check if a customer already exists for that party.
                customer = this.Application.BusinessLogic.CustomerSystem.GetCustomerByPartyNumber(partyNumber);

                // if none is found we create one
                if ((customer == null) || customer.IsEmptyCustomer())
                {
                    customer = this.Application.BusinessLogic.CustomerSystem.CreateCustomerFromParty(partyNumber);
                }
            }

            return customer;
        }

        /// <summary>
        /// Gets active loyalty card number by customer.
        /// </summary>
        /// <param name="customerPartyNumber">Customer party number.</param>
        /// <returns>Loyalty card number.</returns>
        public string GetActiveLoyaltyCardNumber(string customerPartyNumber)
        {
            if (string.IsNullOrEmpty("customerPartyNumber"))
            {
                throw new InvalidOperationException("customerPartyNumber");
            }

            IEnumerable<LoyaltyCard> loyaltyCards = this.LoyaltyData.GetLoyaltyCards(customerPartyNumber);

            int count = (from card in loyaltyCards
                             where card.TenderType != LoyaltyCardTenderType.Blocked
                             select card).Count();
            string cardNumber = null;

            if (count == 1)
            {
                cardNumber = (from card in loyaltyCards
                              where card.TenderType != LoyaltyCardTenderType.Blocked
                              select card).SingleOrDefault().Number;
            }

            return cardNumber;
        }

        public static void AddLoyaltyRewardPointLines(RetailTransaction transaction, List<ILoyaltyRewardPointLine> rewardPointLines)
        {
            if (transaction.LoyaltyItem.RewardPointLines == null)
            {
                transaction.LoyaltyItem.RewardPointLines = rewardPointLines;
            }
            else
            {
                rewardPointLines.AddRange(transaction.LoyaltyItem.RewardPointLines);
                transaction.LoyaltyItem.RewardPointLines = rewardPointLines;
            }
        }

        public void AddLoyaltyTenderLine(RetailTransaction retailTransaction, ICardInfo cardInfo)
        {
            // Gathering tender information
            TenderData tenderData = new TenderData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            ITender tenderInfo = tenderData.GetTender(cardInfo.TenderTypeId, ApplicationSettings.Terminal.StoreId);

            TenderRequirement tenderRequirement = new TenderRequirement((Tender)tenderInfo, cardInfo.Amount, true, retailTransaction.TransSalePmtDiff);
            if (!string.IsNullOrWhiteSpace(tenderRequirement.ErrorText))
            {
                LoyaltyGUI.PromptError(tenderRequirement.ErrorText);
            }

            //Add a loyalty tender item to transaction.
            LoyaltyTenderLineItem loyaltyTenderItem = (LoyaltyTenderLineItem)this.Application.BusinessLogic.Utility.CreateLoyaltyTenderLineItem();
            loyaltyTenderItem.CardNumber = cardInfo.CardNumber;
            loyaltyTenderItem.CardTypeId = cardInfo.CardTypeId;
            loyaltyTenderItem.Amount = cardInfo.Amount;

            //tenderInfo.
            loyaltyTenderItem.Description = tenderInfo.TenderName;
            loyaltyTenderItem.TenderTypeId = cardInfo.TenderTypeId;
            loyaltyTenderItem.CompanyCurrencyAmount = cardInfo.Amount;

            // the exchange rate between the store amount(not the paid amount) and the company currency
            loyaltyTenderItem.ExchrateMST = this.Application.Services.Currency.ExchangeRate(
                ApplicationSettings.Terminal.StoreCurrency) * 100;

            // card tender processing and printing require an EFTInfo object to be attached. 
            // however, we don't want loyalty info to show up where other EFT card info would on the receipt 
            //  because loyalty has its own receipt template fields, so we just assign empty EFTInfo object
            loyaltyTenderItem.EFTInfo = this.Application.BusinessLogic.Utility.CreateEFTInfo();
            // we don't want Loyalty to be 'captured' by payment service, so explicitly set not to capture to be safe
            loyaltyTenderItem.EFTInfo.IsPendingCapture = false;

            loyaltyTenderItem.SignatureData = LSRetailPosis.POSProcesses.TenderOperation.ProcessSignatureCapture(tenderInfo, loyaltyTenderItem);

            retailTransaction.Add(loyaltyTenderItem);

            retailTransaction.LastRunOperationIsValidPayment = true;
        }



        /// <summary>
        /// Checks new loyalty and customer for update.
        /// </summary>
        /// <param name="retailTransaction">Retail transaction.</param>
        /// <param name="loyaltyCard">Loyalty card.</param>
        /// <param name="customer">Loyalty card customer.</param>
        /// <returns>True if the transaction can be updated with loyalty.</returns>
        public static bool AcceptLoyalty(RetailTransaction retailTransaction, LoyaltyCard loyaltyCard, Contracts.DataEntity.ICustomer customer)
        {
            ApplicationLog.Log("Loyalty.AcceptTransactionLoyalty", "Verifying loyalty request...", LSRetailPosis.LogTraceLevel.Trace);
            bool acceptLoyalty = true;

            if (loyaltyCard == null || String.IsNullOrEmpty(loyaltyCard.Number))
            {
                LoyaltyGUI.PromptError(50103);
                acceptLoyalty = false;
            }
            else if (loyaltyCard.TenderType == LoyaltyCardTenderType.Blocked)
            {
                LoyaltyGUI.PromptError(1030555); // The loyalty card can't be added to the transaction because it is blocked.
                acceptLoyalty = false;
            }
            else if (customer != null && !customer.IsEmptyCustomer())
            {
                if (customer.Blocked == BlockedEnum.All || customer.PartyNumber != loyaltyCard.PartyNumber)
                {
                    LoyaltyGUI.PromptInfo(3063);
                    acceptLoyalty = false;
                }
                else if (!retailTransaction.Customer.IsEmptyCustomer() && retailTransaction.Customer.CustomerId != customer.CustomerId)
                {
                    LoyaltyGUI.PromptInfo(3222);
                    acceptLoyalty = false;
                }
            }

            return acceptLoyalty;
        }

        /// <summary>
        /// Updates retail transaction with loyalty and customer.
        /// </summary>
        /// <param name="retailTransaction">Retail transaction.</param>
        /// <param name="loyaltyCard">Loyalty card.</param>
        /// <param name="customer">Loyalty card customer.</param>
        public void UpdateTransactionLoyalty(RetailTransaction retailTransaction, LoyaltyCard loyaltyCard, Contracts.DataEntity.ICustomer customer)
        {
            bool recalcTotals = false;

            if (LoyaltyHelper.CheckTransactionHasLoyalty(retailTransaction))
            {
                foreach (ITenderLineItem tenderLine in retailTransaction.TenderLines)
                {
                    ILoyaltyTenderLineItem loyaltyTenderLine = tenderLine as ILoyaltyTenderLineItem;

                    if (loyaltyTenderLine != null && !loyaltyTenderLine.Voided && loyaltyTenderLine.CardNumber != loyaltyCard.Number)
                    {
                        retailTransaction.VoidPaymentLine(loyaltyTenderLine.LineId);
                    }
                }

                IEnumerable<AffiliationItem> previousLoyaltyAffiliations = this.LoyaltyData.GetAffiliations(retailTransaction.LoyaltyItem.LoyaltyCardNumber);
                if (previousLoyaltyAffiliations.Any())
                {
                    recalcTotals = true;
                    foreach (AffiliationItem affiliation in previousLoyaltyAffiliations)
                    {
                        AffiliationItem item = retailTransaction.AffiliationLines.Where(l => l.RecId == affiliation.RecId && l.LoyaltyTier == affiliation.LoyaltyTier).SingleOrDefault();

                        if (item != null)
                        {
                            retailTransaction.AffiliationLines.Remove(item);
                        }
                        
                    }
                }
            }

            recalcTotals |= this.UpdateTransactionAffiliations(retailTransaction, customer);

            retailTransaction.LoyaltyItem = this.GetLoyaltyItem(loyaltyCard);

            IEnumerable<AffiliationItem> newLoyaltyAffiliations = this.LoyaltyData.GetAffiliations(loyaltyCard.Number);
            if (newLoyaltyAffiliations.Any())
            {
                recalcTotals = true;
                foreach (AffiliationItem affiliation in newLoyaltyAffiliations)
                {
                    retailTransaction.AffiliationLines.AddLast(affiliation);
                }
            }

            if (recalcTotals)
            {
                this.Application.BusinessLogic.ItemSystem.RecalcPriceTaxDiscount(retailTransaction, true);
                retailTransaction.CalcTotals();
            }

            LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(ApplicationLocalizer.Language.Translate(32211)); //The loyalty card was applied to the transaction.
        }

        private bool UpdateTransactionAffiliations(RetailTransaction retailTransaction, Contracts.DataEntity.ICustomer customer)
        {
            bool recalcTotals;

            if (customer != null && !customer.IsEmptyCustomer() && (retailTransaction.Customer.IsEmptyCustomer() || retailTransaction.Customer.CustomerId != customer.CustomerId))
            {
                recalcTotals = true;
                this.Application.BusinessLogic.CustomerSystem.SetCustomer(retailTransaction, customer, customer);

                // Add related affiliations from customer to transaction.
                var affiliations = retailTransaction.Customer.CustomerAffiliations.Where(c => !retailTransaction.AffiliationLines.Any(a => a.RecId == c.AffiliationId)).ToList();

                AffiliationAdd addAffiliationOperation = new AffiliationAdd(affiliations);
                addAffiliationOperation.OperationID = PosisOperations.AffiliationAdd;
                addAffiliationOperation.POSTransaction = retailTransaction;
                addAffiliationOperation.RunOperation();
            }
            else
            {
                recalcTotals = false;
            }

            return recalcTotals;
        }

        public static bool CheckTransactionChangeLoyalty(IRetailTransaction retailTransaction)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            bool canChange = true;

            if (LoyaltyHelper.CheckTransactionHasLoyalty(retailTransaction))
            {
                canChange = LoyaltyGUI.PromptQuestion(50055);
            }

            return canChange;
        }

        public static bool CheckTransactionHasLoyalty(IRetailTransaction retailTransaction)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            return !String.IsNullOrEmpty(retailTransaction.LoyaltyItem.LoyaltyCardNumber);
        }

        public static bool CheckLoyaltyPointsCalculated(IRetailTransaction retailTransaction)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            return retailTransaction.LoyaltyItem.RewardPointLines != null && 
                retailTransaction.LoyaltyItem.RewardPointLines.Any(rpl => rpl.EntryType == LoyaltyRewardPointEntryType.Earn || rpl.EntryType == LoyaltyRewardPointEntryType.ReturnEarned);
        }

        /// <summary>
        /// Prompts a user with the error message if the card number is null or empty.
        /// </summary>
        /// <param name="loyaltyCardNumber">The loyalty card number.</param>
        /// <returns>True if the card number is not null or empty, false otherwise.</returns>
        public static bool ValidateLoyaltyCardNumber(string loyaltyCardNumber)
        {
            bool loyaltyCardNumberIsInvalid = string.IsNullOrEmpty(loyaltyCardNumber);

            if (loyaltyCardNumberIsInvalid)
            {
                LoyaltyGUI.PromptError(50103); //The card number is not valid.
            }

            return !loyaltyCardNumberIsInvalid;
        }
    }
}
