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
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.AffiliationItem;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
    using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;
    using GME_Custom.GME_Propesties;
    using GME_Custom.GME_Data;
    using Microsoft.Dynamics.Retail.Pos.Interaction; ////
    using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;//

    using GME_Custom.GME_EngageFALWSServices;

    /// <summary>
    /// Loyalty implementation class.
    /// </summary>
    [Export(typeof(ILoyalty))]
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Loyalty : ILoyalty
    {
        private IApplication application;

        [Import]
        public IApplication Application
        {
            set
            {
                this.application = value;
                LoyaltyHelper.Instance.Application = this.application;
            }
        }

        public bool AddLoyaltyRequest(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            bool acceptLoyalty = false;

            if (LoyaltyHelper.CheckTransactionChangeLoyalty(transaction))
            {
                ICardInfo cardInfo = LoyaltyGUI.GetCardInfo();

                if (cardInfo != null)
                {
                    acceptLoyalty = this.AddLoyaltyRequest(transaction, cardInfo);
                }
            }

            return acceptLoyalty;
        }

        public bool AddLoyaltyRequest(IRetailTransaction retailTransaction, ICardInfo cardInfo)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (cardInfo == null)
            {
                throw new ArgumentNullException("cardInfo");
            }

            bool acceptLoyalty = false;

            LoyaltyCard loyalty = LoyaltyHelper.Instance.GetLoyaltyCard(cardInfo);
            Contracts.DataEntity.ICustomer customer = null;

            if (loyalty != null)
            {
                customer = (!transaction.Customer.IsEmptyCustomer() && loyalty.PartyNumber == transaction.Customer.PartyNumber)
                        ? transaction.Customer
                        : LoyaltyHelper.Instance.GetCustomer(loyalty.PartyNumber);
            }

            if (transaction.ChannelId == 0)
            {
                transaction.ChannelId = ApplicationSettings.Terminal.StorePrimaryId;
            }

            acceptLoyalty = LoyaltyHelper.AcceptLoyalty(transaction, loyalty, customer);

            if (acceptLoyalty)
            {
                LoyaltyHelper.Instance.UpdateTransactionLoyalty(transaction, loyalty, customer);
            }

            return acceptLoyalty;
        }

        public bool AddLoyaltyRequest(IRetailTransaction retailTransaction, string cardNumber)
        {
            ICardInfo cardInfo = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateCardInfo();
            cardInfo.CardNumber = cardNumber;

            return this.AddLoyaltyRequest(retailTransaction, cardInfo);
        }

        public bool TryToAddLoyaltyRequest(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            bool added = false;

            if (!transaction.Customer.IsEmptyCustomer() && !LoyaltyHelper.CheckTransactionHasLoyalty(transaction))
            {
                string loyaltyCardNumber = LoyaltyHelper.Instance.GetActiveLoyaltyCardNumber(transaction.Customer.PartyNumber);

                if (!string.IsNullOrEmpty(loyaltyCardNumber))
                {
                    added = this.AddLoyaltyRequest(retailTransaction, loyaltyCardNumber);
                }
            }

            return added;
        }

        public void CalculateLoyaltyPoints(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            ApplicationLog.Log("Loyalty.CalculateLoyaltyPoints", string.Format("Calculating earned transaction loyalty points..."), LSRetailPosis.LogTraceLevel.Trace);

            if (LoyaltyHelper.CheckTransactionHasLoyalty(transaction) &&
                transaction.LoyaltyItem.TenderType != LoyaltyCardTenderType.Blocked && !(transaction is CustomerOrderTransaction)
                && !LoyaltyHelper.CheckLoyaltyPointsCalculated(transaction))
            {
                List<ILoyaltyRewardPointLine> rewardLines = LoyaltyRewardCalculator.CalculateRewardPointLinesForEarnOrDeduct(transaction);
                LoyaltyHelper.AddLoyaltyRewardPointLines(transaction, rewardLines);
            }
        }

        public void UpdateLoyaltyPoints(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (!ApplicationSettings.Terminal.EarnLoyaltyOffline
                && LoyaltyHelper.CheckTransactionHasLoyalty(transaction)
                && transaction.LoyaltyItem.RewardPointLines != null)
            {
                ApplicationLog.Log("Loyalty.UpdateLoyaltyPoints", string.Format("Updating loyalty points..."), LSRetailPosis.LogTraceLevel.Trace);

                LoyaltyGUI.PromptInfoAndExecute(50054, () =>
                {
                    IEnumerable<ILoyaltyRewardPointLine> lines = transaction.LoyaltyItem.RewardPointLines.Where(line => line.EntryType != LoyaltyRewardPointEntryType.Redeem && line.EntryType != LoyaltyRewardPointEntryType.Refund);
                    LoyaltyTransactionService.PostLoyaltyCardRewardPointTrans(transaction, lines);
                });
            }

            List<paymentMethod> listPaymentMethod = new List<paymentMethod>();
            IntegrationService integrationService = new IntegrationService();

            DateTime dateNow = DateTime.Now;

            int pointAx = 0;
            int selisih = 0;
            if (GME_Var.isPoint)
            {

                pointAx = integrationService.getPoint(GME_Var.identifierCode, Connection.applicationLoc.Settings.Database.DataAreaID);

                //selisih = GME_Var.pointBalance - pointAx;
                selisih = Convert.ToInt32(GME_Var.memberPoint) - pointAx;

            }



            for (int i = 0; i < GME_Var.paymentMethods.Count; i++)
            {

                if (GME_Var.paymentMethods[i] == "PTS") //point
                {
                    listPaymentMethod.Add(GME_Method.setPaymentMethod(GME_Var.paymentMethods[i], selisih.ToString()));
                    GME_Var.pointUsed = selisih;
                }
                else if (GME_Var.paymentMethods[i] == "18") //tunai
                {
                    decimal amount = 0;
                    if (GME_Var.tenderList.ContainsKey("CASH"))
                    {
                        List<decimal> listAmount = GME_Var.tenderList["CASH"];
                        foreach (decimal val in listAmount)
                        {
                            amount += val;
                        }
                    }

                    if (amount > 0)
                        listPaymentMethod.Add(GME_Method.setPaymentMethod(GME_Var.paymentMethods[i], amount.ToString()));
                }
                else if (GME_Var.paymentMethods[i] == "35") //cash voucher 
                {
                    decimal amount = 0;
                    if (GME_Var.tenderList.ContainsKey("CASH VOUCHER"))
                    {
                        List<decimal> listAmount = GME_Var.tenderList["CASH VOUCHER"];
                        foreach (decimal val in listAmount)
                        {
                            amount += val;
                        }
                    }
                    if (amount > 0)
                        listPaymentMethod.Add(GME_Method.setPaymentMethod(GME_Var.paymentMethods[i], amount.ToString()));

                }
                else if (GME_Var.paymentMethods[i] == "10") //edc bca 
                {
                    decimal amount = 0;
                    if (GME_Var.tenderList.ContainsKey("EDC BCA"))
                    {
                        List<decimal> listAmount = GME_Var.tenderList["EDC BCA"];
                        foreach (decimal val in listAmount)
                        {
                            amount += val;
                        }
                    }

                    if (amount > 0)
                        listPaymentMethod.Add(GME_Method.setPaymentMethod(GME_Var.paymentMethods[i], amount.ToString()));
                }
                else if (GME_Var.paymentMethods[i] == "12") //edc mandiri
                {
                    decimal amount = 0;
                    if (GME_Var.tenderList.ContainsKey("EDC MANDIRI"))
                    {
                        List<decimal> listAmount = GME_Var.tenderList["EDC MANDIRI"];
                        foreach (decimal val in listAmount)
                        {
                            amount += val;
                        }
                    }

                    if (amount > 0)
                        listPaymentMethod.Add(GME_Method.setPaymentMethod(GME_Var.paymentMethods[i], amount.ToString()));

                }
            }

            //update 9220 disini
            if (GME_Var.result9100 == "Success")
            {
                paymentMethods paymentMethods = GME_Method.setPaymentMethods(GME_Var.auditNumber, (Convert.ToInt32(GME_Var.memberPoint) + (GME_Var.PointVisit - selisih)), listPaymentMethod);
                GME_Var.totalPoint = (GME_Var.tempPointBalance - selisih);                
                ticket tick9220 = GME_Method.setTicket(GME_Var.trxNumber, dateNow.ToString("yyyyMMdd"), GME_Var.lineItems, 9220);

                //if (!GME_Var.isSNFTransaction)
                //{
                    if (integrationService.requestReward9220(GME_Var.identifierCode, GME_Var.storeId, GME_Var.trxNumber, retailTransaction.Shift.StaffId, retailTransaction.Shift.TerminalId, dateNow.ToString("yyyyMMdd"), dateNow.ToString("HHmmss"), paymentMethods, tick9220) != "Success")
                    {
                        GME_Var.SetSNF("PROCESSSTOPPED", "RequestReward9220");
                    }
                //}
                //else
                //{
                //    //lanjut nanti
                //}

                if (!GME_Var.isPoint && GME_Var.identifierCode != "9999999999999999999")
                {
                    integrationService.updatePoint(GME_Var.identifierCode, (GME_Var.tempPointBalance - selisih), Connection.applicationLoc.Settings.Database.DataAreaID);
                }

                if (GME_Var.isGrantedCoupon)
                {
                    for (int i = 0; i < GME_Var.GratedCoupons.Count; i++)
                    {
                        GME_Method.PrintReceiptPage(GME_Var.GratedCoupons[i]);
                    }
                }

            }

            GME_Var.amountDue = string.Empty;
            GME_Var.trxNumber = string.Empty;
            GME_Var.storeId = string.Empty;
            GME_Var.result9100 = string.Empty;

        }

        public bool IssueLoyaltyCard(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            ApplicationLog.Log("Loyalty.IssueLoyaltyCard", string.Format("Issuing loyalty card..."), LSRetailPosis.LogTraceLevel.Trace);

            LoyaltyCard loyaltyCard = LoyaltyGUI.IssueLoyaltyCard(transaction);

            bool loyaltyCardIssued = false;

            if ((loyaltyCard != null) && LoyaltyHelper.ValidateLoyaltyCardNumber(loyaltyCard.Number))
            {
                LoyaltyGUI.PromptInfoAndExecute(50102, () =>
                {
                    loyaltyCardIssued = LoyaltyTransactionService.IssueLoyaltyCard(loyaltyCard, transaction.Customer.IsEmptyCustomer() ? 0L : transaction.Customer.PartyId, retailTransaction.ChannelId);
                });

                if (loyaltyCardIssued && LoyaltyHelper.CheckTransactionChangeLoyalty(retailTransaction))
                {
                    this.AddLoyaltyRequest(retailTransaction, loyaltyCard.Number);
                   //GME_FormCaller.GME_FormCardReplacement(application, loyaltyCard.Number);//////// ADD BY MARIA - CARD REPLACEMENT
                }
            }

            return loyaltyCardIssued;
        }

        public void AddLoyaltyPayment(IRetailTransaction retailTransaction, string tenderID)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            ICardInfo cardInfo = LoyaltyHelper.Instance.GetCardForTransactionTender(retailTransaction, tenderID);

            if (cardInfo != null)
            {
                LoyaltyCard loyaltyCard = LoyaltyHelper.Instance.GetLoyaltyCard(cardInfo);

                if (loyaltyCard == null || string.IsNullOrWhiteSpace(loyaltyCard.Number))
                {
                    LoyaltyGUI.PromptError(50103);
                }
                else if (loyaltyCard.TenderType == LoyaltyCardTenderType.Blocked || loyaltyCard.TenderType == LoyaltyCardTenderType.NoTender)
                {
                    LoyaltyGUI.PromptError(99901);
                }
                else
                {
                    if (cardInfo != null)
                    {
                        // Trigger: PreRegisterPayment trigger for the operation
                        PreTriggerResult preRegisterPaymentResult = new PreTriggerResult();

                        PosApplication.Instance.Triggers.Invoke<IPaymentTriggerV3>(
                            t => t.PreRegisterPayment(
                                preRegisterPaymentResult,
                                retailTransaction,
                                (object)PosisOperations.PayLoyalty,
                                cardInfo.TenderTypeId,
                                cardInfo.CurrencyCode,
                                cardInfo.Amount));

                        if (TriggerHelpers.ProcessPreTriggerResults(preRegisterPaymentResult))
                        {
                            this.AddLoyaltyPayment(retailTransaction, cardInfo, cardInfo.Amount);
                        }
                    }
                }
            }
        }

        public void AddLoyaltyPayment(IRetailTransaction retailTransaction, ICardInfo cardInfo, decimal amount)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (cardInfo == null)
            {
                throw new ArgumentException("cardInfo");
            }

            cardInfo.Amount = amount;

            LoyaltyCard loyaltyCard = LoyaltyHelper.Instance.GetLoyaltyCard(cardInfo);

            if (loyaltyCard.TenderType == LoyaltyCardTenderType.NoTender)
            {
                LoyaltyGUI.PromptError(99901);
            }
            else
            {
                List<ILoyaltyRewardPointLine> rewardPointLines;
                if (cardInfo.Amount >= 0)
                {
                    // Prepare point lines to redeem points
                    rewardPointLines = LoyaltyRewardCalculator.CalculateRewardPointLinesForRedeem(transaction, cardInfo);
                }
                else
                {
                    // Prepare point lines to refund points
                    rewardPointLines = LoyaltyRewardCalculator.CalculateRewardPointLinesForRefund(transaction, cardInfo);
                }

                if (rewardPointLines != null)
                {
                    LoyaltyHelper.Instance.AddLoyaltyTenderLine(transaction, cardInfo);

                    LoyaltyHelper.AddLoyaltyRewardPointLines(transaction, rewardPointLines);
                }
            }
        }

        public bool AddLoyaltyDiscount(IRetailTransaction retailTransaction, string cardNumber, decimal discountAmount)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;
            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (String.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException("cardNumber");
            }

            // If a previous loyalty item exists on the transaction, the system should prompt the user whether to 
            // overwrite the existing loyalty item or cancel the operation
            if ((transaction.LoyaltyItem != null) && (!string.IsNullOrWhiteSpace(transaction.LoyaltyItem.LoyaltyCardNumber)))
            {
                if (!transaction.LoyaltyItem.LoyaltyCardNumber.Equals(cardNumber, StringComparison.Ordinal))
                {
                    if (!LoyaltyGUI.PromptQuestion(50055)) // Do you want to overwrite the loyalty record?
                    {
                        return false;
                    }
                }
            }

            if (!AddLoyaltyRequest(transaction, cardNumber))
            {
                return false;
            }

            // if the amount is higher than the "new" amount available for discount, then it is acceptable to lower the amount
            decimal amountAvailableForDiscount = transaction.SaleItems.Where(si => si.EligibleForDiscount || si.LoyaltyDiscountWasApplied).Sum(si => si.NetAmountWithTax + si.LoyaltyDiscount);
            if (Math.Abs(discountAmount) > Math.Abs(amountAvailableForDiscount))
            {
                discountAmount = amountAvailableForDiscount;
            }

            if (discountAmount == decimal.Zero)
            {
                transaction.LoyaltyItem.RewardPointLines = null;
            }
            else if (!LoyaltyRewardCalculator.CalculateRewardPointLinesForDiscount(transaction.LoyaltyItem, discountAmount))
            {
                return false;
            }

            return true;
        }

        public ILoyaltyItem GetLoyaltyItem(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentNullException("cardNumber");
            }

            ICardInfo cardInfo = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateCardInfo();
            cardInfo.CardNumber = cardNumber;
            LoyaltyCard loyaltyCard = LoyaltyHelper.Instance.GetLoyaltyCard(cardInfo);

            if (!LoyaltyHelper.AcceptLoyalty(null, loyaltyCard, null))
            {
                return null;
            }

            ILoyaltyItem loyaltyItem = LoyaltyHelper.Instance.GetLoyaltyItem(loyaltyCard);
            if (!LoyaltyRewardCalculator.CalculateRewardPointLinesForDiscount(loyaltyItem, null))
            {
                return null;
            }

            return loyaltyItem;
        }

        private static string GetLoyaltyCardTypeString(LoyaltyCardTenderType cardType)
        {
            const int loyaltyCardTypeAsCardTender = 55513; //As card tender
            const int loyaltyCardTypeAsContactTender = 55514; //As contact tender
            const int loyaltyCardTypeNoTender = 55515; //No tender

            string cardTypeString = string.Empty;

            if (cardType == LoyaltyCardTenderType.AsCardTender)
            {
                cardTypeString = ApplicationLocalizer.Language.Translate(loyaltyCardTypeAsCardTender);
            }
            else if (cardType == LoyaltyCardTenderType.AsContactTender)
            {
                cardTypeString = ApplicationLocalizer.Language.Translate(loyaltyCardTypeAsContactTender);
            }
            else if (cardType == LoyaltyCardTenderType.NoTender)
            {
                cardTypeString = ApplicationLocalizer.Language.Translate(loyaltyCardTypeNoTender);
            }

            return cardTypeString;
        }

        public ILoyaltyCardData GetLoyaltyBalanceInfo(string cardNumber)
        {
            const int loyaltyCardStatusActive = 55509; //Active
            const int loyaltyCardStatusBlocked = 55510; //Blocked
            const int cardNumberIsNotValid = 50103; //The card number is not valid.

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                throw new ArgumentException("cardNumber");
            }

            var cardInfo = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateCardInfo();
            cardInfo.CardNumber = cardNumber;

            var loyaltyCard = LoyaltyHelper.Instance.GetLoyaltyCard(cardInfo);
            if (loyaltyCard == null)
            {
                LoyaltyGUI.PromptError(cardNumberIsNotValid);
                return null;
            }

            var loyaltyCardData = new LSRetailPosis.BusinessLogic.PropertyClasses.LoyaltyCardData();
            loyaltyCardData.CardNumber = cardNumber;

            var customer = LoyaltyHelper.Instance.GetCustomer(loyaltyCard.PartyNumber);

            loyaltyCardData.CustomerName = (customer != null) ? customer.Name : string.Empty;
            loyaltyCardData.CardType = (customer != null && customer.Blocked == BlockedEnum.All) ? LoyaltyCardTenderType.Blocked : loyaltyCard.TenderType;
            loyaltyCardData.CardTypeString = GetLoyaltyCardTypeString(loyaltyCardData.CardType);
            loyaltyCardData.StatusString = ApplicationLocalizer.Language.Translate((loyaltyCardData.CardType == LoyaltyCardTenderType.Blocked)
                                                ? loyaltyCardStatusBlocked : loyaltyCardStatusActive);

            if (loyaltyCardData.CardType != LoyaltyCardTenderType.Blocked)
            {
                var loyaltyItem = LoyaltyHelper.Instance.GetLoyaltyItem(loyaltyCard);
                LoyaltyRewardCalculator.CalculateLoyaltyCardBalance(loyaltyItem, loyaltyCardData);
            }

            return loyaltyCardData;
        }

        /// <summary>
        /// Submits the request to print loyalty card balance to the printer.
        /// </summary>
        /// <param name="loyaltyCardData">An instance of the <see cref="ILoyaltyCardData"/> holding the information about the loyalty card.</param>
        public void PrintLoyaltyBalance(ILoyaltyCardData loyaltyCardData)
        {
            if (LoyaltyHelper.Instance.Application.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            {
                LoyaltyHelper.Instance.Application.Services.Peripherals.FiscalPrinter.PrintLoyaltyCardBalance(loyaltyCardData);
            }
            else
            {
                LoyaltyGUI.PromptError(86501);
            }
        }

        public bool VoidLoyaltyPayment(IRetailTransaction retailTransaction, ILoyaltyTenderLineItem loyaltyTenderItem)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (loyaltyTenderItem == null)
            {
                throw new ArgumentNullException("loyaltyTenderItem");
            }

            transaction.VoidPaymentLine(loyaltyTenderItem.LineId);

            if (transaction.LoyaltyItem != null && transaction.LoyaltyItem.RewardPointLines != null)
            {
                transaction.LoyaltyItem.RewardPointLines = transaction.LoyaltyItem.RewardPointLines.Where(line => !(line.LoyaltyCardNumber == loyaltyTenderItem.CardNumber
                                                         && (line.EntryType == LoyaltyRewardPointEntryType.Redeem || line.EntryType == LoyaltyRewardPointEntryType.Refund)));
            }

            return true;
        }

        public decimal GetLoyaltyBalance(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;
            if (transaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            decimal rewardPointBalance = decimal.Zero;
            IEnumerable<RewardPoint> rewardPoints = LoyaltyTransactionService.GetLoyaltyRewardsPoints(transaction.LoyaltyItem.LoyaltyCardNumber);
            if (rewardPoints != null)
            {
                rewardPointBalance = rewardPoints.Sum(rp => rp.Active);
            }

            return rewardPointBalance;
        }

        public bool CaptureLoyaltyRedeemOrRefundLines(IRetailTransaction retailTransaction)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (transaction == null)
            {
                throw new ArgumentException(string.Format("The argument is not of type {0}", typeof(RetailTransaction).Name), "retailTransaction");
            }

            ApplicationLog.Log("Loyalty.CaptureLoyaltyRedeemOrRefundLines", string.Format("Capturing loyalty reward lines..."), LSRetailPosis.LogTraceLevel.Trace);

            bool loyaltyRedeemOrRefundLinesCaptured = false;

            LoyaltyGUI.PromptInfoAndExecute(50053, () =>
            {
                //need to recalculate loyalty points in case new quantity of returned item was entered
                if (transaction.LoyaltyDiscount < decimal.Zero)
                {
                    retailTransaction.LoyaltyItem.RewardPointLines = null;
                    LoyaltyRewardCalculator.CalculateRewardPointLinesForDiscount(retailTransaction.LoyaltyItem, retailTransaction.LoyaltyDiscount);
                }
                
                IEnumerable<ILoyaltyRewardPointLine> lines = transaction.LoyaltyItem.RewardPointLines.Where(line => line.EntryType == LoyaltyRewardPointEntryType.Redeem || line.EntryType == LoyaltyRewardPointEntryType.Refund);
                if (transaction.LoyaltyDiscount != decimal.Zero && lines != null)
                {
                    foreach (var rewardPointLine in lines.Where(line => line.EntryDate == null || line.EntryDate == DateTime.MinValue))
                    {
                        rewardPointLine.EntryDate = DateTime.Now;
                        rewardPointLine.EntryTime = (int)rewardPointLine.EntryDate.TimeOfDay.TotalSeconds;

                        // For redemption loyalty points as a discount operation ExpirationDate value here is a delta 
                        // between reward point expiration time value (in years, months or days) and default EntryDate value (DateTime.MinValue);
                        // this value needs to be adjusted with the current EntryDate value actualized for the moment of posting;
                        // if ExpirationDate is not set up or equals to DateTime.MaxValue (reward point never expires) adjustment is not needed
                        if (rewardPointLine.ExpirationDate.HasValue && rewardPointLine.ExpirationDate != DateTime.MaxValue)
                        {
                            rewardPointLine.ExpirationDate = new DateTime(rewardPointLine.ExpirationDate.Value.Ticks - DateTime.MinValue.Ticks + rewardPointLine.EntryDate.Ticks); 
                        }
                    }
                }
                loyaltyRedeemOrRefundLinesCaptured = LoyaltyTransactionService.PostLoyaltyCardRewardPointTrans(transaction, lines);
            });

            return loyaltyRedeemOrRefundLinesCaptured;
        }
    }
}
