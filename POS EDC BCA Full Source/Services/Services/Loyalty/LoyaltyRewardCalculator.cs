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
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.SaleItem;

    internal class LoyaltyRewardCalculator
    {
        private const int NoneSalesLineSalesId = -1;

        private LoyaltyRewardCalculator()
        {
        }

        /// <summary>
        /// Calculates reward point lines for redemption.
        /// </summary>
        /// <param name="retailTransaction">The transaction.</param>
        /// <param name="cardInfo">The card infomation including card number and payment amount.</param>
        /// <returns>The reward point lines.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public static List<ILoyaltyRewardPointLine> CalculateRewardPointLinesForRedeem(RetailTransaction retailTransaction, ICardInfo cardInfo)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (cardInfo == null)
            {
                throw new ArgumentNullException("cardInfo");
            }

            if (cardInfo.Amount <= 0m)
            {
                throw new ArgumentException("Payment amount must be greater than zero.");
            }

            IEnumerable<RewardPoint> rewardPoints = LoyaltyTransactionService.GetLoyaltyRewardsPoints(cardInfo.CardNumber);

            List<ILoyaltyRewardPointLine> rewardPointLines = new List<ILoyaltyRewardPointLine>();

            if (rewardPoints != null && rewardPoints.Any())
            {
                decimal redeemAmountLeft = cardInfo.Amount;
                int lineNumber = 0;
                if (retailTransaction.LoyaltyItem != null
                    && retailTransaction.LoyaltyItem.RewardPointLines != null
                    && retailTransaction.LoyaltyItem.RewardPointLines.Any())
                {
                    lineNumber = retailTransaction.LoyaltyItem.RewardPointLines.Count();
                }

                Dictionary<long, decimal> payablePerSalesLine = new Dictionary<long, decimal>();

                // Prepare the loyalty payable amount per sales line
                foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(item => !item.Voided))
                {
                    decimal salesLineTotalCharges = (from cl in saleItem.MiscellaneousCharges select cl.Amount).Sum();
                    payablePerSalesLine.Add(saleItem.LineId, saleItem.NetAmountWithTax + salesLineTotalCharges);
                }

                // Add transaction-level charges to the list
                if (retailTransaction.MiscellaneousCharges.Any())
                {
                    decimal transactionLevelTotalCharges = (from cl in retailTransaction.MiscellaneousCharges select cl.Amount).Sum();
                    payablePerSalesLine.Add(NoneSalesLineSalesId, transactionLevelTotalCharges);
                }

                foreach (RewardPoint activePoint in rewardPoints)
                {
                    IEnumerable<RedeemSchemeLine> redeemLines = LoyaltyHelper.Instance.LoyaltyData.GetRedeemSchemeLines(cardInfo.CardNumber, activePoint.RewardPointId)
                        .Where(rsl => (rsl.ToRewardType == LoyaltyRewardType.PaymentByAmount || rsl.ToRewardType == LoyaltyRewardType.PaymentByQuantity) 
                                       && rsl.FromRewardPointAmountQuantity > decimal.Zero 
                                       && rsl.ToRewardAmountQuantity > decimal.Zero);

                    decimal activePoints = activePoint.Active;
                    RedeemSchemeLine firstBestRedeemLine = null;

                    // Use the reward points to pay towards the transaction, product by product (because points may have restrictions on products)
                    // For each sales line, we can only redeem at most the min of redeemAmountLeft and payablePerSalesLine.
                    foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(i => !i.Voided && payablePerSalesLine[i.LineId] > 0m))
                    {
                        decimal salesLineMaxRedeemAmount = Math.Min(payablePerSalesLine[saleItem.LineId], redeemAmountLeft);

                        RedeemSchemeLine bestRedeemLine = LoyaltyRewardCalculator.FindBestRedeemLine(redeemLines, saleItem);

                        if (bestRedeemLine != null)
                        {
                            if (firstBestRedeemLine == null)
                            {
                                firstBestRedeemLine = bestRedeemLine;
                            }
                            
                            decimal redeemedPoints = CalculateRedeemPointsPerSalesLine(ref redeemAmountLeft, payablePerSalesLine, activePoints, saleItem, salesLineMaxRedeemAmount, bestRedeemLine);
                            activePoints -= redeemedPoints;

                            if (redeemAmountLeft <= 0m || activePoints <= 0m)
                            {
                                break;
                            }
                        }
                    }

                    // Use the reward points to pay towards the transaction-level charge lines
                    // We can only redeem at most the min of redeemAmountLeft and transactionLevelTotalCharges.
                    if (redeemAmountLeft > 0m && activePoints > 0m && payablePerSalesLine.ContainsKey(NoneSalesLineSalesId) && payablePerSalesLine[NoneSalesLineSalesId] > 0m)
                    {
                        decimal maxRedeemAmount = Math.Min(payablePerSalesLine[NoneSalesLineSalesId], redeemAmountLeft);

                        RedeemSchemeLine bestRedeemLine = LoyaltyRewardCalculator.FindBestRedeemLine(redeemLines, null);

                        if (bestRedeemLine != null && bestRedeemLine.FromRewardPointAmountQuantity > 0 && bestRedeemLine.ToRewardAmountQuantity > 0)
                        {
                            if (firstBestRedeemLine == null)
                            {
                                firstBestRedeemLine = bestRedeemLine;
                            }

                            decimal redeemedPoints = CalculateRedeemPointsPerSalesLine(ref redeemAmountLeft, payablePerSalesLine, activePoints, null, maxRedeemAmount, bestRedeemLine);
                            activePoints -= redeemedPoints;
                        }
                    }

                    decimal totalRedeemedPoints = activePoint.Active - activePoints;

                    if (totalRedeemedPoints > 0m)
                    {
                        // Round redeem points
                        totalRedeemedPoints = totalRedeemedPoints * -1m;
                        totalRedeemedPoints = RoundLoyaltyPointsForPayment(totalRedeemedPoints, activePoint.RewardPointType, activePoint.RewardPointCurrency);

                        ILoyaltyRewardPointLine rewardPointLine = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateLoyaltyRewardPointLine();
                        rewardPointLine.EntryType = LoyaltyRewardPointEntryType.Redeem;
                        rewardPointLine.EntryDate = DateTime.Now;
                        rewardPointLine.EntryTime = (int)rewardPointLine.EntryDate.TimeOfDay.TotalSeconds;
                        rewardPointLine.LoyaltyGroupRecordId = firstBestRedeemLine.LoyaltyGroupRecordId;
                        rewardPointLine.LoyaltyTierRecordId = firstBestRedeemLine.LoyaltyTierRecordId;
                        rewardPointLine.RewardPointRecordId = redeemLines.First().FromRewardPointRecordId;
                        rewardPointLine.RewardPointType = firstBestRedeemLine.FromRewardPointType;
                        rewardPointLine.RewardPointId = activePoint.RewardPointId;
                        rewardPointLine.RewardPointRedeemable = true;
                        rewardPointLine.RewardPointAmountQuantity = totalRedeemedPoints;
                        rewardPointLine.RewardPointCurrency = activePoint.RewardPointCurrency;
                        rewardPointLine.LoyaltyCardNumber = cardInfo.CardNumber;
                        rewardPointLine.LineNumber = ++lineNumber;

                        rewardPointLines.Add(rewardPointLine);
                    }

                    if (redeemAmountLeft <= 0m)
                    {
                        break;
                    }
                }

                if (redeemAmountLeft > 0m)
                {
                    LoyaltyGUI.PromptError(50057);
                    rewardPointLines = null;
                }
            }
            else
            {
                LoyaltyGUI.PromptError(50057);
                rewardPointLines = null;
            }

            return rewardPointLines;
        }

        private static decimal CalculateRedeemPointsPerSalesLine(ref decimal redeemAmountLeft, Dictionary<long, decimal> payablePerSalesLine, decimal activePoints, SaleLineItem saleItem, decimal maxRedeemAmount, RedeemSchemeLine bestRedeemLine)
        {
            decimal redeemedPoints = 0m;
            decimal coveredAmount = 0m;
            decimal pointPrice = 0m;

            // Payment by amount type reward rule defines that X points cost Y money
            // Payment by quantity type reward rule defines that X points cost Y products
            // Here the price of a one point (in money) is calculated
            switch (bestRedeemLine.ToRewardType)
            {
                case LoyaltyRewardType.PaymentByAmount:
                    {
                        pointPrice = bestRedeemLine.ToRewardAmountQuantity / bestRedeemLine.FromRewardPointAmountQuantity;
                        break;
                    }
                case LoyaltyRewardType.PaymentByQuantity:
                    {
                        decimal productPrice = saleItem.NetAmountWithTax / saleItem.Quantity;
                        pointPrice = bestRedeemLine.ToRewardAmountQuantity * productPrice / bestRedeemLine.FromRewardPointAmountQuantity;
                        break;
                    }
                default:
                    throw new InvalidOperationException("Redeem rule line has invalid reward type");
            }

            // Redeem points
            redeemedPoints = Math.Min(
                activePoints,
                maxRedeemAmount / pointPrice);

            // The covered price is calculated based on spent amount of points. 
            coveredAmount = LoyaltyHelper.Instance.Application.Services.Rounding.Round(
                redeemedPoints * pointPrice,
                ApplicationSettings.Terminal.StoreCurrency);

            if (saleItem != null)
            {
                payablePerSalesLine[saleItem.LineId] -= coveredAmount;
            }
            else
            {
                payablePerSalesLine[NoneSalesLineSalesId] -= coveredAmount;
            }
            redeemAmountLeft -= coveredAmount;

            return redeemedPoints;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public static bool CalculateRewardPointLinesForDiscount(ILoyaltyItem loyaltyItem, decimal? amount)
        {
            if (loyaltyItem == null)
            {
                throw new ArgumentNullException("loyaltyItem");
            }
            
            if (string.IsNullOrWhiteSpace(loyaltyItem.LoyaltyCardNumber))
            {
                throw new ArgumentException("loyaltyItem.LoyaltyCardNumber should not be empty");
            }

            // Get all redeemable loyalty reward points
            IEnumerable<RewardPoint> rewardPoints = LoyaltyTransactionService.GetLoyaltyRewardsPoints(loyaltyItem.LoyaltyCardNumber);
            if (rewardPoints == null || !rewardPoints.Any())
            {
                LoyaltyGUI.PromptError(50057); // The loyalty card does not have enough points.
                return false;
            }

            int lineNumber = 0;
            if (loyaltyItem.RewardPointLines != null
                && loyaltyItem.RewardPointLines.Any())
            {
                lineNumber = loyaltyItem.RewardPointLines.Count();
            }

            List<ILoyaltyRewardPointLine> rewardPointLines = new List<ILoyaltyRewardPointLine>(rewardPoints.Count());
            decimal redeemAmountLeft = amount.HasValue ? amount.Value : decimal.Zero;
            // Iterate through all loyalty reward points, get all redeem scheme line for each point and try to find best redeem line by rate,
            // if best redeen line was found add new loyalty reward point line to the incoming loyalty item 
            foreach (RewardPoint activePoint in rewardPoints)
            {
                IEnumerable<RedeemSchemeLine> redeemLines = LoyaltyHelper.Instance.LoyaltyData.GetRedeemSchemeLines(loyaltyItem.LoyaltyCardNumber, activePoint.RewardPointId)
                    .Where(rsl => rsl.ToRewardType == LoyaltyRewardType.Discount && rsl.FromRewardPointAmountQuantity > decimal.Zero && rsl.ToRewardAmountQuantity > decimal.Zero);

                RedeemSchemeLine bestRedeemLine = LoyaltyRewardCalculator.FindBestRedeemLine(redeemLines, null);
                if (bestRedeemLine == null)
                {
                    continue;
                }

                decimal redeemedPoints = amount.HasValue
                    ? Math.Min(activePoint.Active, redeemAmountLeft * (bestRedeemLine.FromRewardPointAmountQuantity / bestRedeemLine.ToRewardAmountQuantity))
                    : activePoint.Active;
                if (redeemedPoints == decimal.Zero)
                {
                    continue;
                }

                decimal coveredAmount = LoyaltyHelper.Instance.Application.Services.Rounding.Round(
                    redeemedPoints * bestRedeemLine.ToRewardAmountQuantity / bestRedeemLine.FromRewardPointAmountQuantity,
                    ApplicationSettings.Terminal.StoreCurrency);

                redeemAmountLeft -= coveredAmount;

                ILoyaltyRewardPointLine rewardPointLine = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateLoyaltyRewardPointLine();
                rewardPointLine.EntryType = coveredAmount >= decimal.Zero ? LoyaltyRewardPointEntryType.Redeem : LoyaltyRewardPointEntryType.Refund;
                rewardPointLine.LoyaltyGroupRecordId = bestRedeemLine.LoyaltyGroupRecordId;
                rewardPointLine.LoyaltyTierRecordId = bestRedeemLine.LoyaltyTierRecordId;
                rewardPointLine.RewardPointRecordId = bestRedeemLine.FromRewardPointRecordId;
                rewardPointLine.RewardPointType = bestRedeemLine.FromRewardPointType;
                rewardPointLine.RewardPointId = bestRedeemLine.FromRewardPointId;
                rewardPointLine.RewardPointRedeemable = bestRedeemLine.FromRewardPointRedeemable;
                rewardPointLine.RewardPointCurrency = activePoint.RewardPointCurrency;
                rewardPointLine.LoyaltyCardNumber = loyaltyItem.LoyaltyCardNumber;
                rewardPointLine.RewardPointAmountQuantity = RoundLoyaltyPointsForPayment(decimal.Negate(redeemedPoints), activePoint.RewardPointType, activePoint.RewardPointCurrency);

                if (rewardPointLine.EntryType == LoyaltyRewardPointEntryType.Refund)
                {
                    LoyaltyRewardCalculator.SetExpirationDate(rewardPointLine, bestRedeemLine.FromRewardPointExpirationTimeValue,bestRedeemLine.FromRewardPointExpirationTimeUnit);
                }

                rewardPointLine.LineNumber = ++lineNumber;

                rewardPointLines.Add(rewardPointLine);

                // If whole amount was covered then break the cycle
                if (amount.HasValue && redeemAmountLeft <= decimal.Zero)
                {
                    break;
                }
            }

            if (!rewardPointLines.Any())
            {
                LoyaltyGUI.PromptError(55605); //Loyalty point discounts are not configured for the card. Enter another loyalty card number.
                return false;
            }

            // If not the whole amount was covered throw the error
            if (amount.HasValue && redeemAmountLeft > decimal.Zero)
            {
                LoyaltyGUI.PromptError(50057); // The loyalty card does not have enough points.
                return false;
            }

            // Save reward point lines and covered amount in the loyalty item
            if (loyaltyItem.RewardPointLines != null)
            {
                rewardPointLines.AddRange(loyaltyItem.RewardPointLines);
            }
            loyaltyItem.RewardPointLines = rewardPointLines;
            loyaltyItem.Amount = amount.HasValue ? decimal.Negate(amount.Value) : redeemAmountLeft;
            return true;
        }

        /// <summary>
        /// Calculates reward point lines for refund.
        /// </summary>
        /// <param name="retailTransaction">The transaction.</param>
        /// <param name="cardInfo">The card infomation including card number and payment amount.</param>
        /// <returns>The reward point lines.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public static List<ILoyaltyRewardPointLine> CalculateRewardPointLinesForRefund(RetailTransaction retailTransaction, ICardInfo cardInfo)
        {
            // Validate inputs
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            if (cardInfo == null)
            {
                throw new ArgumentNullException("cardInfo");
            }

            if (cardInfo.Amount >= 0m)
            {
                throw new ArgumentException("Refund amount must be greater than zero.");
            }

            // Get reward point status
            IEnumerable<RewardPoint> rewardPoints = LoyaltyTransactionService.GetLoyaltyRewardsPoints(cardInfo.CardNumber);

            List<ILoyaltyRewardPointLine> rewardPointLines = new List<ILoyaltyRewardPointLine>();
            if (rewardPoints != null && rewardPoints.Any())
            {
                decimal refundAmountLeft = decimal.Negate(cardInfo.Amount);
                int lineNumber = 0;
                if (retailTransaction.LoyaltyItem != null
                    && retailTransaction.LoyaltyItem.RewardPointLines != null
                    && retailTransaction.LoyaltyItem.RewardPointLines.Any())
                {
                    lineNumber = retailTransaction.LoyaltyItem.RewardPointLines.Count();
                }

                CustomerOrderTransaction customerOrderTransaction = retailTransaction as CustomerOrderTransaction;
                Dictionary<int, decimal> payablePerSalesLine = new Dictionary<int, decimal>();

                // Prepare payable amount per sales line
                 if (customerOrderTransaction != null && customerOrderTransaction.Mode == LSRetailPosis.Transaction.CustomerOrderMode.Cancel)
                 {
                     payablePerSalesLine = retailTransaction.SaleItems.Where(item => !item.Voided && item.NetAmountWithTax > decimal.Zero).ToDictionary(item => item.LineId, item => item.NetAmountWithTax);
                 }
                 else
                 {
                     payablePerSalesLine = retailTransaction.SaleItems.Where(item => !item.Voided && item.NetAmountWithTax < decimal.Zero).ToDictionary(item => item.LineId, item => decimal.Negate(item.NetAmountWithTax));
                 }

                // Convert refund amount to reward points based on redeem ranking
                foreach (RewardPoint refundPoint in rewardPoints)
                {
                    // Find the redemption rules that apply
                    IEnumerable<RedeemSchemeLine> redeemLines = LoyaltyHelper.Instance.LoyaltyData.GetRedeemSchemeLines(cardInfo.CardNumber, refundPoint.RewardPointId)
                        .Where(rsl => (rsl.ToRewardType == LoyaltyRewardType.PaymentByAmount || rsl.ToRewardType == LoyaltyRewardType.PaymentByQuantity)
                                       && rsl.FromRewardPointAmountQuantity > decimal.Zero
                                       && rsl.ToRewardAmountQuantity > decimal.Zero);

                    RedeemSchemeLine firstBestRedeemLine = null;
                    decimal totalRefundPoints = 0m;
                    foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(i => !i.Voided && payablePerSalesLine[i.LineId] > 0m))
                    {
                        decimal salesLineMaxRefundAmount = Math.Min(payablePerSalesLine[saleItem.LineId], refundAmountLeft);

                        RedeemSchemeLine bestRedeemLine = LoyaltyRewardCalculator.FindBestRedeemLine(redeemLines, saleItem);

                        if (bestRedeemLine != null)
                        {
                            if (firstBestRedeemLine == null)
                            {
                                firstBestRedeemLine = bestRedeemLine;
                            }

                            // Payment by amount type reward rule defines that X points cost Y money
                            // Payment by quantity type reward rule defines that X points cost Y products
                            // Here the price of a one point (in money) is calculated
                            decimal pointPrice = 0m;
                            switch (bestRedeemLine.ToRewardType)
                            {
                                case LoyaltyRewardType.PaymentByAmount:
                                    {
                                        pointPrice = bestRedeemLine.ToRewardAmountQuantity / bestRedeemLine.FromRewardPointAmountQuantity;
                                        break;
                                    }
                                case LoyaltyRewardType.PaymentByQuantity:
                                    {
                                        decimal productPrice = saleItem.NetAmountWithTax / saleItem.Quantity;
                                        pointPrice = bestRedeemLine.ToRewardAmountQuantity * productPrice / bestRedeemLine.FromRewardPointAmountQuantity;
                                        break;
                                    }
                                default:
                                    throw new InvalidOperationException("Redeem rule line has invalid reward type");
                            }

                            // Calculate refund points
                            decimal refundPoints = salesLineMaxRefundAmount / pointPrice;

                            totalRefundPoints += refundPoints;
                            payablePerSalesLine[saleItem.LineId] -= salesLineMaxRefundAmount;
                            refundAmountLeft -= salesLineMaxRefundAmount;

                            if (refundAmountLeft <= 0m)
                            {
                                break;
                            }
                        }
                    }

                    if (totalRefundPoints > 0m)
                    {
                        // Round refund points
                        totalRefundPoints = RoundLoyaltyPointsForPayment(totalRefundPoints, refundPoint.RewardPointType, refundPoint.RewardPointCurrency);

                        // Prepare a reward point line for refund
                        ILoyaltyRewardPointLine rewardPointLine = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateLoyaltyRewardPointLine();
                        rewardPointLine.EntryType = LoyaltyRewardPointEntryType.Refund;
                        rewardPointLine.EntryDate = DateTime.Now;
                        rewardPointLine.EntryTime = (int)rewardPointLine.EntryDate.TimeOfDay.TotalSeconds;
                        rewardPointLine.LoyaltyGroupRecordId = firstBestRedeemLine.LoyaltyGroupRecordId;
                        rewardPointLine.LoyaltyTierRecordId = firstBestRedeemLine.LoyaltyTierRecordId;
                        rewardPointLine.RewardPointRecordId = redeemLines.First().FromRewardPointRecordId;
                        rewardPointLine.RewardPointType = firstBestRedeemLine.FromRewardPointType;
                        rewardPointLine.RewardPointId = refundPoint.RewardPointId;
                        rewardPointLine.RewardPointRedeemable = true;
                        rewardPointLine.RewardPointAmountQuantity = totalRefundPoints;
                        rewardPointLine.RewardPointCurrency = refundPoint.RewardPointCurrency;
                        rewardPointLine.LoyaltyCardNumber = cardInfo.CardNumber;
                        rewardPointLine.LineNumber = ++lineNumber;

                        LoyaltyRewardCalculator.SetExpirationDate(rewardPointLine, firstBestRedeemLine.FromRewardPointExpirationTimeValue, firstBestRedeemLine.FromRewardPointExpirationTimeUnit);

                        rewardPointLines.Add(rewardPointLine);
                    }

                    if (refundAmountLeft <= 0m)
                    {
                        break;
                    }
                }

                if (refundAmountLeft > 0m)
                {
                    LoyaltyGUI.PromptError(50165);
                    rewardPointLines = null;
                }
            }
            else
            {
                LoyaltyGUI.PromptError(50165);
                rewardPointLines = null;
            }

            return rewardPointLines;
        }

        /// <summary>
        /// Rounds loyalty points for payment. It's payment when points are negative. It's refund when points are positive.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="rewardPointType">The type fo the reward point. Amount or Quantity.</param>
        /// <param name="rewardPointCurrency">The currency of the reward point if it's Amount type.</param>
        /// <returns>The rounding result.</returns>
        private static decimal RoundLoyaltyPointsForPayment(decimal points, LoyaltyRewardPointType rewardPointType, string rewardPointCurrency)
        {
            switch (rewardPointType)
            {
                case LoyaltyRewardPointType.Quantity:
                    if (points <= 0)
                    {
                        points = Math.Floor(points);
                    }
                    else
                    {
                        points = Math.Ceiling(points);
                    }
                    break;

                case LoyaltyRewardPointType.Amount:
                    points = LoyaltyHelper.Instance.Application.Services.Rounding.Round(points, rewardPointCurrency);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Invalid reward point type: {0}.", rewardPointType));
            }
            return points;
        }

        /// <summary>
        /// Calculates reward point lines for earning points or return points.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The reward point lines.</returns>
        public static List<ILoyaltyRewardPointLine> CalculateRewardPointLinesForEarnOrDeduct(RetailTransaction transaction)
        {
            List<ILoyaltyRewardPointLine> rewardPointLines = new List<ILoyaltyRewardPointLine>();
            List<LoyaltyRewardPointEntryType> rewardPointEntryTypes = new List<LoyaltyRewardPointEntryType>();

            // Check if the transaction should return points
            bool isReturnByReceipt = transaction.SaleItems.Any(saleItem => saleItem.ReceiptReturnItem && !saleItem.Voided);
            if (!isReturnByReceipt || !transaction.ReturnTransactionHasLoyaltyPayment)
            {
                rewardPointEntryTypes.Add(LoyaltyRewardPointEntryType.ReturnEarned);
            }

            // Check if the transaction should earn points
            if ((!transaction.TenderLines.Any(tenderLine => tenderLine is ILoyaltyTenderLineItem && !tenderLine.Voided) && transaction.LoyaltyDiscount == decimal.Zero)
                || (transaction.LoyaltyDiscount != decimal.Zero && ApplicationSettings.Terminal.AwardPointsForPartialRedemption))
            {
                rewardPointEntryTypes.Add(LoyaltyRewardPointEntryType.Earn);
            }

            int lineNumber = 0;
            if (transaction.LoyaltyItem.RewardPointLines != null
                && transaction.LoyaltyItem.RewardPointLines.Any())
            {
                lineNumber = transaction.LoyaltyItem.RewardPointLines.Count();
            }

            // Calculate points
            if (rewardPointEntryTypes.Count > 0)
            {
                IEnumerable<EarnSchemeLine> earnSchemeLines = LoyaltyHelper.Instance.LoyaltyData.GetEarnSchemeLines(transaction.LoyaltyItem.LoyaltyCardNumber);

                IEnumerable<SaleLineItem> saleLines = transaction.SaleItems.Where(item => !item.Voided);

                foreach (LoyaltyRewardPointEntryType rewardPointEntryType in rewardPointEntryTypes)
                {
                    foreach (EarnSchemeLine earnSchemeLine in earnSchemeLines)
                    {
                        ILoyaltyRewardPointLine rewardPointLine = LoyaltyRewardCalculator.CalculateEarnedLoyaltyPointsByLine(earnSchemeLine, saleLines, rewardPointEntryType);

                        if (rewardPointLine != null)
                        {
                            rewardPointLine.LineNumber = ++lineNumber;
                            rewardPointLine.LoyaltyCardNumber = transaction.LoyaltyItem.LoyaltyCardNumber;
                            rewardPointLines.Add(rewardPointLine);
                        }
                    }
                }
            }

            return rewardPointLines;
        }

        private static ILoyaltyRewardPointLine GetRewardPointLine(EarnSchemeLine earnSchemeLine, decimal earnedPoints)
        {
            ILoyaltyRewardPointLine rewardPointLine = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateLoyaltyRewardPointLine();
            rewardPointLine.LoyaltyGroupRecordId = earnSchemeLine.LoyaltyGroupRecordId;
            rewardPointLine.EntryDate = DateTime.Now;
            rewardPointLine.EntryTime = (int)rewardPointLine.EntryDate.TimeOfDay.TotalSeconds;
            rewardPointLine.LoyaltyTierRecordId = earnSchemeLine.LoyaltyTierRecordId;
            rewardPointLine.RewardPointRecordId = earnSchemeLine.ToRewardPointRecordId;
            rewardPointLine.RewardPointAmountQuantity = earnedPoints;
            rewardPointLine.RewardPointType = earnSchemeLine.ToRewardPointType;
            rewardPointLine.RewardPointId = earnSchemeLine.ToRewardPointId;
            rewardPointLine.RewardPointRedeemable = earnSchemeLine.ToRewardPointRedeemable;
            rewardPointLine.RewardPointCurrency = earnSchemeLine.ToRewardPointCurrency;

            if (earnedPoints > 0m)
            {
                SetExpirationDate(rewardPointLine, earnSchemeLine.ToRewardPointExpirationTimeValue, earnSchemeLine.ToRewardPointExpirationTimeUnit);
            }

            return rewardPointLine;
        }

        private static void SetExpirationDate(ILoyaltyRewardPointLine rewardPointLine, int expirationTimeValue, DayMonthYear expirationTimeUnit)
        {
            if (expirationTimeValue == 0)
            {
                rewardPointLine.ExpirationDate = DateTime.MaxValue;
            }
            else
            {
                switch (expirationTimeUnit)
                {
                    case DayMonthYear.Day:
                        rewardPointLine.ExpirationDate = rewardPointLine.EntryDate.AddDays(expirationTimeValue);
                        break;

                    case DayMonthYear.Month:
                        rewardPointLine.ExpirationDate = rewardPointLine.EntryDate.AddMonths(expirationTimeValue);
                        break;

                    case DayMonthYear.Year:
                        rewardPointLine.ExpirationDate = rewardPointLine.EntryDate.AddYears(expirationTimeValue);
                        break;

                    default:
                        rewardPointLine.ExpirationDate = DateTime.MaxValue;
                        break;
                }
            }
        }

        private static ILoyaltyRewardPointLine CalculateEarnedLoyaltyPointsByLine(EarnSchemeLine earnSchemeLine, IEnumerable<SaleLineItem> saleLines, LoyaltyRewardPointEntryType rewardPointType)
        {
            decimal totalQuantity = 0;
            decimal totalAmount = 0;

            if (earnSchemeLine.FromActivityType == LoyaltyActivityType.PurchaseProductByAmount
                || earnSchemeLine.FromActivityType == LoyaltyActivityType.PurchaseProductByQuantity
                || earnSchemeLine.FromActivityType == LoyaltyActivityType.SalesTransactionCount)
            {
                bool productIndependent = earnSchemeLine.FromCategory == 0 && earnSchemeLine.FromProduct == 0 && earnSchemeLine.FromVariant == 0;
                LSRetailPosis.DataAccess.ItemData itemData = new LSRetailPosis.DataAccess.ItemData(
                                ApplicationSettings.Database.LocalConnection,
                                ApplicationSettings.Database.DATAAREAID,
                                ApplicationSettings.Terminal.StorePrimaryId);

                foreach (SaleLineItem saleLine in saleLines)
                {
                    if ((
                            (rewardPointType == LoyaltyRewardPointEntryType.Earn && saleLine.Quantity > 0)
                            || (rewardPointType == LoyaltyRewardPointEntryType.ReturnEarned && saleLine.Quantity < 0)
                        )
                        && !saleLine.Voided)
                    {
                        bool accept = LoyaltyRewardCalculator.IsSalesLineEligible(saleLine, earnSchemeLine.FromCategory, earnSchemeLine.FromProduct, earnSchemeLine.FromVariant);

                        if (accept)
                        {
                            totalQuantity += saleLine.UnitQtyConversion.Convert(saleLine.Quantity);
                            totalAmount += saleLine.NetAmount;
                        }
                    }
                }
            }

            decimal earnedPoints = 0;

            switch (earnSchemeLine.FromActivityType)
            {
                case LoyaltyActivityType.PurchaseProductByAmount:
                    totalAmount = LoyaltyHelper.Instance.Application.Services.Currency.CurrencyToCurrency(ApplicationSettings.Terminal.StoreCurrency, earnSchemeLine.FromActivityAmountCurrency, totalAmount);
                    earnedPoints = LoyaltyRewardCalculator.CheckThresholdAndCalculateEarnedPoints(earnSchemeLine, totalAmount);
                    break;
                case LoyaltyActivityType.PurchaseProductByQuantity:
                    earnedPoints = LoyaltyRewardCalculator.CheckThresholdAndCalculateEarnedPoints(earnSchemeLine, totalQuantity);
                    break;
                case LoyaltyActivityType.SalesTransactionCount:
                    if (rewardPointType == LoyaltyRewardPointEntryType.Earn && totalAmount > decimal.Zero)
                    {
                        earnedPoints = LoyaltyRewardCalculator.CheckThresholdAndCalculateEarnedPoints(earnSchemeLine, 1.0m);
                    }
                    break;
                default:
                    throw new InvalidOperationException("earnSchemeLine.FromActivityType");
            }

            ILoyaltyRewardPointLine rewardPointLine = null;

            if (earnedPoints != 0m)
            {
                rewardPointLine = LoyaltyRewardCalculator.GetRewardPointLine(earnSchemeLine, earnedPoints);
                rewardPointLine.EntryType = rewardPointType;
            }

            return rewardPointLine;
        }

        private static decimal CheckThresholdAndCalculateEarnedPoints(EarnSchemeLine earnSchemeLine, decimal activityAmountQuantity)
        {
            decimal earnedPoints = 0;

            if (Math.Abs(activityAmountQuantity) >= earnSchemeLine.FromActivityAmountQuantity)
            {
                if (earnSchemeLine.ToRewardPointType == LoyaltyRewardPointType.Quantity)
                {
                    decimal div = activityAmountQuantity / earnSchemeLine.FromActivityAmountQuantity;
                    if (div > 0m)
                    {
                        div = Math.Floor(div);
                    }
                    else
                    {
                        div = Math.Ceiling(div);
                    }

                    earnedPoints = div * earnSchemeLine.ToRewardPointAmountQuantity;
                }
                else if (earnSchemeLine.ToRewardPointType == LoyaltyRewardPointType.Amount)
                {
                    earnedPoints = (activityAmountQuantity / earnSchemeLine.FromActivityAmountQuantity) * earnSchemeLine.ToRewardPointAmountQuantity;
                    earnedPoints = LoyaltyHelper.Instance.Application.Services.Rounding.Round(earnedPoints, ApplicationSettings.Terminal.StoreCurrency);
                }
            }

            return earnedPoints;
        }

        private static RedeemSchemeLine FindBestRedeemLine(IEnumerable<RedeemSchemeLine> redeemLines, SaleLineItem saleItem)
        {
            decimal bestRedeemRate = 0;
            RedeemSchemeLine bestRedeemLine = null;
            string redeemCurrency = ApplicationSettings.Terminal.StoreCurrency;

            foreach (var redeemLine in redeemLines)
            {
                if ((saleItem == null && (redeemLine.ToRewardType == LoyaltyRewardType.PaymentByAmount || redeemLine.ToRewardType == LoyaltyRewardType.Discount))
                    || LoyaltyRewardCalculator.IsSalesLineEligible(saleItem, redeemLine.ToCategory, redeemLine.ToProduct, redeemLine.ToVariant))
                {
                    decimal redeemRateInRedeemCurrency = 0m;

                    switch (redeemLine.ToRewardType)
                    {
                        case LoyaltyRewardType.Discount:
                        case LoyaltyRewardType.PaymentByAmount:
                            {
                                // Convert the reward amount to redeem currency if it's different.
                                if (!redeemLine.ToRewardAmountCurrency.Equals(redeemCurrency, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Currency conversion
                                    redeemLine.ToRewardAmountQuantity = LoyaltyHelper.Instance.Application.Services.Currency.CurrencyToCurrency(redeemLine.ToRewardAmountCurrency, redeemCurrency, redeemLine.ToRewardAmountQuantity);
                                    redeemLine.ToRewardAmountCurrency = redeemCurrency;
                                }

                                redeemRateInRedeemCurrency = redeemLine.FromRewardPointAmountQuantity / redeemLine.ToRewardAmountQuantity;
                                break;
                            }
                        case LoyaltyRewardType.PaymentByQuantity:
                            {
                                decimal amountPerUnit = saleItem.NetAmountWithTax / saleItem.Quantity;
                                redeemRateInRedeemCurrency = redeemLine.FromRewardPointAmountQuantity / (redeemLine.ToRewardAmountQuantity * amountPerUnit);
                                break;
                            }
                    }

                    if (bestRedeemRate == 0 || redeemRateInRedeemCurrency < bestRedeemRate)
                    {
                        bestRedeemRate = redeemRateInRedeemCurrency;
                        bestRedeemLine = redeemLine;
                    }
                }
            }

            return bestRedeemLine;
        }

        private static bool IsSalesLineEligible(SaleLineItem saleItem, long category, long product, long variant)
        {
            bool productIndependent = category == 0L && product == 0L && variant == 0L;
            LSRetailPosis.DataAccess.ItemData itemData = new LSRetailPosis.DataAccess.ItemData(
                            ApplicationSettings.Database.LocalConnection,
                            ApplicationSettings.Database.DATAAREAID,
                            ApplicationSettings.Terminal.StorePrimaryId);

            bool accept = false;

            if (saleItem == null)
            {
                accept = false;
            }
            else if (productIndependent)
            {
                accept = true;
            }
            else if (variant != 0L)
            {
                accept = (saleItem.Dimension != null && variant == saleItem.Dimension.DistinctProductVariantId);
            }
            else if (product != 0L)
            {
                accept = (product == saleItem.ProductId);
            }
            else if (category != 0L)
            {
                accept = itemData.ProductInCategory(saleItem.ProductId, saleItem.Dimension == null ? 0 : saleItem.Dimension.DistinctProductVariantId, category);
            }

            return accept;
        }

        public static void CalculateLoyaltyCardBalance(ILoyaltyItem loyaltyItem, ILoyaltyCardData loyaltyCardData)
        {

            if (loyaltyItem == null)
            {
                throw new ArgumentNullException("loyaltyItem");
            }

            if (string.IsNullOrWhiteSpace(loyaltyItem.LoyaltyCardNumber))
            {
                throw new ArgumentException("loyaltyItem.LoyaltyCardNumber should not be empty");
            }
            
            loyaltyCardData.IssuedPoints = decimal.Zero;
            loyaltyCardData.UsedPoints = decimal.Zero;
            loyaltyCardData.ExpiredPoints = decimal.Zero;
            loyaltyCardData.BalancePoints = decimal.Zero;

            // Get all redeemable loyalty reward points
            IEnumerable<RewardPoint> rewardPoints = LoyaltyTransactionService.GetLoyaltyRewardsPoints(loyaltyItem.LoyaltyCardNumber, false);

            if (rewardPoints != null)
            {
                foreach (RewardPoint activePoint in rewardPoints)
                {
                    loyaltyCardData.IssuedPoints += activePoint.Issued;
                    loyaltyCardData.UsedPoints += activePoint.Used;
                    loyaltyCardData.ExpiredPoints += activePoint.Expired;
                    loyaltyCardData.BalancePoints += activePoint.Active;
                }
            }
        }
    }
}
