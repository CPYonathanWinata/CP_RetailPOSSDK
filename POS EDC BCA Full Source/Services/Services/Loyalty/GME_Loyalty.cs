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

namespace Microsoft.Dynamics.Retail.Pos.Loyalty
{
    public class GME_Loyalty
    {
        public bool createLoyalty(string loyaltyCardNum, IRetailTransaction retailTransaction)
        {
            bool loyaltyCardIssued = false;
            RetailTransaction transaction = retailTransaction as RetailTransaction;
            LoyaltyCard loyaltyCard = null;            

            loyaltyCard = LoyaltyCard.GetInstance();
            loyaltyCard.Number = loyaltyCardNum;
            loyaltyCard.TenderType = LoyaltyCardTenderType.AsCardTender;
            if (transaction.Customer != null && !transaction.Customer.IsEmptyCustomer())
            {
                loyaltyCard.PartyNumber = transaction.Customer.PartyNumber;
            }

            if ((loyaltyCard != null) && LoyaltyHelper.ValidateLoyaltyCardNumber(loyaltyCard.Number))
            {
                loyaltyCardIssued = LoyaltyTransactionService.IssueLoyaltyCard(loyaltyCard, transaction.Customer.IsEmptyCustomer() ? 0L : transaction.Customer.PartyId, retailTransaction.ChannelId);

                if (loyaltyCardIssued && LoyaltyHelper.CheckTransactionChangeLoyalty(retailTransaction))
                {
                    this.AddLoyaltyRequestCard(retailTransaction, loyaltyCard.Number);
                }
            }

            return loyaltyCardIssued;
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

        public bool AddLoyaltyRequestCard(IRetailTransaction retailTransaction, string cardNumber)
        {
            ICardInfo cardInfo = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateCardInfo();
            cardInfo.CardNumber = cardNumber;
            
            return this.AddLoyaltyRequest(retailTransaction, cardInfo);
        }
    }
}
