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
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using Microsoft.Dynamics.Commerce.Runtime.TransactionService.Serialization;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
 

    internal static class LoyaltyTransactionService
    {
        public static bool PostLoyaltyCardRewardPointTrans(RetailTransaction retailTransaction, IEnumerable<ILoyaltyRewardPointLine> rewardLines)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

             if (rewardLines == null)
            {
                throw new ArgumentNullException("rewardLines");
            }

            try
            {
                bool rewardLinesArePosted = false;

                if (rewardLines.Any())
                {
                    XDocument rewardsDoc = LoyaltyTransactionService.GetRewardPointTransAsXml(retailTransaction, rewardLines);

                    ReadOnlyCollection<object> serviceResponse = LoyaltyHelper.Instance.Application.TransactionServices.Invoke(
                        "PostLoyaltyCardRewardPointTrans",
                        new object[]
                    {
                        rewardsDoc.ToString()
                    });

                    rewardLinesArePosted = (bool)serviceResponse[1];

                    if (!rewardLinesArePosted)
                    {
                        string message = serviceResponse[2].ToString();
                        LoyaltyGUI.PromptError(message);
                    }
                }

                return rewardLinesArePosted;
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException("LSRetailPosis.TransactionServices.PostLoyaltyCardRewardPointTrans", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates loyalty card.
        /// </summary>
        /// <param name="loyaltyCard">Loyalty card.</param>
        /// <param name="partyId">The record identifier of the party.</param>
        /// <param name="channelId">The record identifier of the channel.</param>
        /// <returns>True if loyalty card was issued.</returns>
        public static bool IssueLoyaltyCard(LoyaltyCard loyaltyCard, long partyId, long channelId)
        {
            if (loyaltyCard == null || string.IsNullOrEmpty(loyaltyCard.Number))
            {
                throw new ArgumentException("loyaltyCard");
            }

            ReadOnlyCollection<object> serviceResponse = LoyaltyHelper.Instance.Application.TransactionServices.Invoke(
                "IssueLoyaltyCard",
                new object[] { 
                    loyaltyCard.Number,
                    (int)loyaltyCard.TenderType,
                    partyId,
                    channelId
                });
            
            try
            {
                bool success = (bool)serviceResponse[1];
                object[] data = new object[serviceResponse.Count - 3];
                Array.Copy(serviceResponse.ToArray(), 3, data, 0, data.Length);

                if (success)
                {
                    // igsugak: TS API may change to return xml instead of array of objects. Remove this call and method after implementation.
                    string cardInfoXml = LoyaltyTransactionService.GetLoyaltyCardAsXml(data);
                    LoyaltyHelper.Instance.LoyaltyData.SaveLoyaltyCard(cardInfoXml);
                }
                else
                {
                    string message = serviceResponse[2].ToString();
                    LoyaltyGUI.PromptError(message);
                }
                LoyaltyHelper.CPupdateUplineProgram(loyaltyCard.Number);
                return success;
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException("LSRetailPosis.TransactionServices.IssueLoyaltyCard", ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets loyalty reward points by the loyalty card number.
        /// </summary>
        /// <param name="loyaltyCardNumber">The Loyalty card number.</param>
	/// <returns>Reward points</returns>        
	/// <returns>The loyalty card's reward points.</returns>
        public static IEnumerable<RewardPoint> GetLoyaltyRewardsPoints(string loyaltyCardNumber, bool includeActivePointsOnly = true)
        {
            if (string.IsNullOrWhiteSpace(loyaltyCardNumber))
            {
                throw new ArgumentException("loyaltyItem");
            }

            try
            {
                ReadOnlyCollection<object> serviceResponse = LoyaltyHelper.Instance.Application.TransactionServices.Invoke(
                    "GetLoyaltyCardRewardPointsStatus",
                    new object[] {
                        SerializationHelper.ConvertDateTimeToAXDateString(DateTime.Today, 213),
                        loyaltyCardNumber,
                        true,
                        true,
                        true,
                        false,
                        includeActivePointsOnly
                });

                bool success = (bool)serviceResponse[1];

                IEnumerable<RewardPoint> rewardPoints = null;

                if (success)
                {
                    rewardPoints = LoyaltyTransactionService.GetRewardPointsFromXml(serviceResponse[3].ToString());
                }
                else
                {
                    string message = serviceResponse[2].ToString();
                    LoyaltyGUI.PromptError(message);
                }

                return rewardPoints;
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException("Loyalty.GetLoyaltyRewardsPoints", ex);
                throw;
            }
        }

        private static IEnumerable<RewardPoint> GetRewardPointsFromXml(string rewardPointsXml)
        {
            XDocument rewardPointsDoc = XDocument.Parse(rewardPointsXml);

            var rewardPointsParsed = (from rewardPointInfo in rewardPointsDoc.Descendants("RewardPointStatus")
                                      select new
                                      {
                                          RewardPointId = rewardPointInfo.Attribute("RewardPointId").Value,
                                          Issued = decimal.Parse(rewardPointInfo.Attribute("Issued").Value, CultureInfo.InvariantCulture),
                                          Expired = decimal.Parse(rewardPointInfo.Attribute("Expired").Value, CultureInfo.InvariantCulture),
                                          Used = decimal.Parse(rewardPointInfo.Attribute("Used").Value, CultureInfo.InvariantCulture),
                                          Active = decimal.Parse(rewardPointInfo.Attribute("Active").Value, CultureInfo.InvariantCulture),
                                          RewardPointCurrency = rewardPointInfo.Attribute("Currency").Value,
                                          RewardPointType = (LoyaltyRewardPointType)Enum.Parse(typeof(LoyaltyRewardPointType), rewardPointInfo.Attribute("RewardPointType").Value),
                                          RedeemRanking = rewardPointInfo.Attribute("RedeemRanking").Value
                                      });

            IEnumerable<RewardPoint> rewardPoints = (from rewardPoint in rewardPointsParsed
                                                    group rewardPoint by new { rewardPoint.RewardPointId, rewardPoint.RewardPointCurrency, rewardPoint.RedeemRanking, rewardPoint.RewardPointType }
                                                    into groupedPoint
                                                    orderby groupedPoint.Key.RedeemRanking
                                                    select new RewardPoint
                                                    {
                                                        RewardPointId = groupedPoint.Key.RewardPointId,
                                                        Issued = groupedPoint.Sum(l => l.Issued),
                                                        Expired = groupedPoint.Sum(l => l.Expired),
                                                        Used = groupedPoint.Sum(l => l.Used),
                                                        Active = groupedPoint.Sum(l => l.Active),
                                                        RewardPointCurrency = groupedPoint.Key.RewardPointCurrency,
                                                        RewardPointType = groupedPoint.Key.RewardPointType
                                                    }).AsEnumerable();

            return rewardPoints;
        }

        private static string GetLoyaltyCardAsXml(object[] data)
        {
            LoyaltyCard loyaltyCard = LoyaltyCard.GetInstance();

            XElement loyaltyCardTierListElmt = new XElement("GroupTierList");
            foreach (object cardTierRowData in (object[])data[1])
            {
                object[] cardTierData = (object[])cardTierRowData;
                loyaltyCardTierListElmt.Add(new XElement("CardGroupTier",
                    new XElement("CardTierRecId", (long)cardTierData[0]),
                    new XElement("GroupRecId", (long)cardTierData[1]),
                    new XElement("TierRecId", (long)cardTierData[3]),
                    new XElement("ValidFrom", cardTierData[4].ToString()),
                    new XElement("ValidTo", cardTierData[5].ToString())
                    ));
            }

            object[] cardData = (object[])data[0];
            XElement loyaltyCardElmt = new XElement("LoyaltyCard",
                new XElement("RecId", (long)cardData[0]),
                new XElement("Number", (string)cardData[1]),
                new XElement("TenderType", (int)cardData[2]),
                new XElement("PartyRecId", (long)cardData[3]),
                loyaltyCardTierListElmt
                );

            XDocument loyaltyDoc = new XDocument();
            loyaltyDoc.Add(loyaltyCardElmt);
            return loyaltyDoc.ToString();
        }

        private static XDocument GetRewardPointTransAsXml(RetailTransaction retailTransaction, IEnumerable<ILoyaltyRewardPointLine> rewardLines)
        {
            XElement rewardsElmt = new XElement("RetailLoyaltyCardRewardPointTransList");

            LoyaltyTransactionService.CreatedXmlElementsFromRewardLines(rewardsElmt, rewardLines);
            LoyaltyTransactionService.SetXmlElementsTransactionProperties(rewardsElmt, retailTransaction);

            XDocument rewardsDoc = new XDocument(rewardsElmt);
            return rewardsDoc;
        }

        private static void SetXmlElementsTransactionProperties(XElement rewardsElmt, RetailTransaction retailTransaction)
        {
            ICustomerOrderTransaction customerOrderTransaction = retailTransaction as ICustomerOrderTransaction;
            string salesId = customerOrderTransaction != null ? customerOrderTransaction.OrderId : string.Empty;
            string transactionId = string.IsNullOrEmpty(salesId) ? retailTransaction.TransactionId : string.Empty;
            string dataAreaId = ApplicationSettings.Database.DATAAREAID ?? string.Empty;
            LoyaltyTransactionType loyaltyTransactionType = string.IsNullOrEmpty(salesId) ? LoyaltyTransactionType.RetailTransaction : LoyaltyTransactionType.SalesOrder;
            foreach (XElement rewardLine in rewardsElmt.Elements())
            {
                rewardLine.Add(
                    new XElement("Channel", retailTransaction.ChannelId),
                    new XElement("CustAccount", retailTransaction.Customer.CustomerId ?? string.Empty),
                    new XElement("CustAccountDataAreaId", dataAreaId),
                    new XElement("LoyaltyTransactionType", (int)loyaltyTransactionType),
                    new XElement("LoyaltyTransDataAreaId", dataAreaId),
                    new XElement("ReceiptId", retailTransaction.ReceiptId ?? string.Empty),
                    new XElement("SalesId", salesId),
                    new XElement("StaffId", retailTransaction.OperatorId),
                    new XElement("StoreId", retailTransaction.StoreId ?? string.Empty),
                    new XElement("TerminalId", retailTransaction.TerminalId ?? string.Empty),
                    new XElement("TransactionId", transactionId ?? string.Empty));
            }
        }

        private static void CreatedXmlElementsFromRewardLines(XElement rewardsElmt, IEnumerable<ILoyaltyRewardPointLine> rewardLines)
        {
            foreach (ILoyaltyRewardPointLine rewardLine in rewardLines)
            {
                TimeSpan t = TimeSpan.FromSeconds(rewardLine.EntryTime);

                XElement rewardElmt = new XElement(
                    "RetailLoyaltyCardRewardPointTrans",
                    new XElement("Affiliation", rewardLine.LoyaltyGroupRecordId),
                    new XElement("CardNumber", rewardLine.LoyaltyCardNumber),
                    new XElement("EntryDate", string.Format("{0:MM/dd/yyyy}", rewardLine.EntryDate)),
                    new XElement("EntryTime", string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds)),
                    new XElement("EntryType", (int)rewardLine.EntryType),
                    new XElement("ExpirationDate", string.Format("{0:MM/dd/yyyy}", rewardLine.ExpirationDate)),
                    new XElement("LoyaltyTier", rewardLine.LoyaltyTierRecordId),
                    new XElement("LoyaltyTransLineNum", rewardLine.LineNumber),
                    new XElement("RewardPoint", rewardLine.RewardPointRecordId),
                    new XElement("RewardPointAmountQty", rewardLine.RewardPointAmountQuantity));

                rewardsElmt.Add(rewardElmt);
            }
        }
    }
}
