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
using System.ComponentModel;
using System.Data;
using System.Linq;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// View model for sale refund details form.
    /// </summary>
    internal sealed class SaleRefundDetailsViewModel : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets the payment refunds list.
        /// </summary>
        public ReadOnlyCollection<PaymentRefundItem> PaymentRefunds
        {
            get { return paymentRefundCollection; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Retrieves payment refund amounts for the retail transaction.
        /// </summary>
        /// <param name="retailTransaction">Retail transaction</param>
        public void RetrievePaymentRefunds(IRetailTransaction retailTransaction)
        {
            if (retailTransaction == null)
                throw new ArgumentNullException("retailTransaction");

            var paymentRefunds = new PaymentRefundCollection();

            IRetailTransaction originalTransaction, returnTransaction = null;
            TransactionTenderData transactionTenderData = null;

            var transactionData = new TransactionData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, PosApplication.Instance);

            //In case the income retailTransaction is a return sale we should retreive the originalTransaction,
            //otherwise consider retailTransaction as an originalTransaction
            if (retailTransaction.SaleIsReturnSale)
            {
                originalTransaction = transactionData.GetOriginalRetailTransaction((RetailTransaction)retailTransaction);
                if (originalTransaction == null)
                    throw new InvalidOperationException("Original transaction is not found.");

                returnTransaction = retailTransaction;
            }
            else
            {
                originalTransaction = retailTransaction;
            }

            if (transactionTenderData == null)
                transactionTenderData = new TransactionTenderData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            //Retreive payments for the originalTransaction and add them to paymentRefunds collection or update in case they are already exist
            using (DataTable paymentTransactions = transactionTenderData.GetRetailTransPayments(originalTransaction.TransactionId, originalTransaction.StoreId, 
                originalTransaction.TerminalId))
            {
                ConsiderOriginalPayments(paymentRefunds, retailTransaction.StoreId, paymentTransactions); 
            }

            //Retreive loyaltyDiscount for the originalTransaction and add it to paymentRefunds collection or update in case it is already exist
            originalTransaction.LoyaltyDiscount = transactionData.GetLoyaltyDiscountAmount(originalTransaction.TransactionId, originalTransaction.StoreId, originalTransaction.TerminalId);
            ConsiderOriginalDiscount(paymentRefunds, originalTransaction);

            //Retreive earlier returned payments/discounts for the originalTransaction and add/update them to paymentRefunds collection
            using (DataTable returnedEarlierEntryAmounts = transactionTenderData.GetTransactionReturnedEntryAmounts(originalTransaction))
            {
                ConsiderReturnedEarlierEntryAmounts(paymentRefunds, originalTransaction.StoreId, returnedEarlierEntryAmounts);
            }

            //In case retailTransaction is a return sale get the current payments/discounts and add/update them to paymentRefunds collection
            if (returnTransaction != null)
            {
                ConsiderCurrentRefunds(paymentRefunds, returnTransaction);
                ConsiderCurrentDiscountRefunds(paymentRefunds, returnTransaction);
            }

            CalculateAvailableAmountsAndTotals(paymentRefunds, retailTransaction);

            paymentRefundCollection = new ReadOnlyCollection<PaymentRefundItem>(paymentRefunds);

            OnPropertyChanged("PaymentRefunds");
        }

        #endregion

        #region Private members

        private ReadOnlyCollection<PaymentRefundItem> paymentRefundCollection;

        /// <summary>
        /// Considers payments of the original transaction in the summary amounts of a payment refund collection.
        /// </summary>
        /// <param name="paymentRefunds">Keyed collection of <see cref="PaymentRefundItem"/></param>
        /// <param name="store">Retail store ID</param>
        /// <param name="paymentTransactions">A <see cref="DataTable"/> holding payments of the original transaction.</param>
        private void ConsiderOriginalPayments(KeyedCollection<string, PaymentRefundItem> paymentRefunds, string store, DataTable paymentTransactions)
        {
            foreach (DataRow transaction in paymentTransactions.Rows)
            {
                string tenderType = (string)transaction["TENDERTYPE"];
                AddPaymentAmountToCollection(paymentRefunds, RefundAmountType.PaidAmount, store, RefundAmountEntryType.Payment, tenderType, (decimal)transaction["AMOUNTTENDERED"]);
            }
        }

        /// <summary>
        /// Considers discount of the original transaction in the summary amounts of a payment refund collection.
        /// </summary>
        /// <param name="paymentRefunds">Keyed collection of <see cref="PaymentRefundItem"/></param>
        /// <param name="originalTransaction">Original retail transaction</param>
        private void ConsiderOriginalDiscount(KeyedCollection<string, PaymentRefundItem> paymentRefunds, IRetailTransaction originalTransaction)
        {
            if (originalTransaction.LoyaltyDiscount != decimal.Zero)
            {
                AddPaymentAmountToCollection(paymentRefunds, RefundAmountType.PaidAmount, originalTransaction.StoreId, RefundAmountEntryType.Discount,
                    ((int)TypeOfDiscount.Loyalty).ToString(), originalTransaction.LoyaltyDiscount);
            }
        }

        /// <summary>
        /// Considers refunds of the earlier returned transactions in the summary amounts of a payment refund collection
        /// </summary>
        /// <param name="paymentRefunds">Keyed collection of payment refund items</param>
        /// <param name="storeId">Store Id</param>
        /// <param name="returnedEarlierEntryAmounts">Earlier returned entry amounts</param>
        private void ConsiderReturnedEarlierEntryAmounts(PaymentRefundCollection paymentRefunds, string storeId, DataTable returnedEarlierEntryAmounts)
        {
            foreach (DataRow earlierRefund in returnedEarlierEntryAmounts.Rows)
            {
                AddPaymentAmountToCollection(paymentRefunds, RefundAmountType.ReturnedEarlierAmount, storeId, 
                    DBUtil.ToEnum<RefundAmountEntryType>(earlierRefund["ENTRYTYPE"]), DBUtil.ToStr(earlierRefund["ENTRYID"]), DBUtil.ToDecimal(earlierRefund["AMOUNT"]));
            }
        }

        /// <summary>
        /// Considers refunds of the current return transaction in the summary amounts of a payment refund collection.
        /// </summary>
        /// <param name="paymentRefunds">Keyed collection of <see cref="PaymentRefundItem"/></param>
        /// <param name="retailTransaction">Current return transaction</param>
        private void ConsiderCurrentRefunds(KeyedCollection<string, PaymentRefundItem> paymentRefunds, IRetailTransaction retailTransaction)
        {
            RetailTransaction returnTransaction = (RetailTransaction)retailTransaction;
            if (returnTransaction.TenderLines != null)
            {
                foreach (var tenderLine in (from line in returnTransaction.TenderLines where !line.Voided select line))
                {
                    AddPaymentAmountToCollection(paymentRefunds, RefundAmountType.ReturnedAmount, returnTransaction.StoreId, RefundAmountEntryType.Payment, tenderLine.TenderTypeId, tenderLine.Amount);
                }
            }
        }

        /// <summary>
        /// Considers discount refunds of the current return transaction in the summary amounts of a payment refund collection
        /// </summary>
        /// <param name="paymentRefunds">Keyed collection of <see cref="PaymentRefundItem"/></param>
        /// <param name="returnTransaction">Current return transaction</param>
        private void ConsiderCurrentDiscountRefunds(PaymentRefundCollection paymentRefunds, IRetailTransaction returnTransaction)
        {
            if (returnTransaction.LoyaltyDiscount != decimal.Zero)
            {
                AddPaymentAmountToCollection(paymentRefunds, RefundAmountType.ReturnedAmount, returnTransaction.StoreId, RefundAmountEntryType.Discount,
                    ((int)TypeOfDiscount.Loyalty).ToString(), returnTransaction.LoyaltyDiscount);
            }
        }

        /// <summary>
        /// Adds payment amount to the collection.
        /// </summary>
        /// <param name="paymentRefunds">Keyed collection of <see cref="PaymentRefundItem"/></param>
        /// <param name="amountType">Type of the amount</param>
        /// <param name="store">Retail store ID</param>
        /// <param name="entryType">Entry type</param>
        /// <param name="entryId">Entry ID</param>
        /// <param name="amount">Amount to consider</param>
        private void AddPaymentAmountToCollection(KeyedCollection<string, PaymentRefundItem> paymentRefunds, RefundAmountType amountType, string store, RefundAmountEntryType entryType, string entryId, decimal amount)
        {
            PaymentRefundItem paymentRefund;
            entryId = entryId.Trim();
            var key = PaymentRefundItem.GetKey(entryType, entryId);
            if (paymentRefunds.Contains(key))
            {
                paymentRefund = paymentRefunds[key];
                AddPaymentAmountOnSum(paymentRefund, amount, amountType, true);
            }
            else
            {
                paymentRefund = new PaymentRefundItem();
                paymentRefund.EntryId = entryId;
                paymentRefund.EntryType = entryType;
                if (tenderData == null)
                    tenderData = new TenderData(PosApplication.Instance.Settings.Database.Connection, PosApplication.Instance.Settings.Database.DataAreaID);
                if (entryType == RefundAmountEntryType.Payment)
                {
                    paymentRefund.EntryName = tenderData.GetTender(entryId, store).TenderName; 
                }
                else if ((entryType == RefundAmountEntryType.Discount) && (entryId.Equals(((int)TypeOfDiscount.Loyalty).ToString())))
                {
                    paymentRefund.EntryName = ApplicationLocalizer.Language.Translate(55606);
                }
                AddPaymentAmountOnSum(paymentRefund, amount, amountType);
                paymentRefunds.Add(paymentRefund);
            }
        }

        /// <summary>
        /// Adds payment amount to the appropriate payment refund column.
        /// </summary>
        /// <param name="paymentRefund">Payment refund item</param>
        /// <param name="amount">Amount to consider</param>
        /// <param name="amountType">Type of the amount</param>
        /// <param name="add">Denotes whether to add the value to sum or to set it as initial value; optional.</param>
        private static void AddPaymentAmountOnSum(PaymentRefundItem paymentRefund, decimal amount, RefundAmountType amountType, bool add = false)
        {
            switch (amountType)
            {
                case RefundAmountType.PaidAmount:
                    if (add)
                        paymentRefund.OriginalAmount += amount;
                    else
                        paymentRefund.OriginalAmount = amount;
                    break;
                case RefundAmountType.ReturnedAmount:
                    if (add)
                        paymentRefund.ReturnedAmount += -amount;
                    else
                        paymentRefund.ReturnedAmount = -amount;
                    break;
                case RefundAmountType.ReturnedEarlierAmount:
                    if (add)
                        paymentRefund.ReturnedEarlierAmount += -amount;
                    else
                        paymentRefund.ReturnedEarlierAmount = -amount;
                    break;
                case RefundAmountType.AvailableAmount:
                    if (add)
                        paymentRefund.AvailableAmount += amount;
                    else
                        paymentRefund.AvailableAmount = amount;
                    break;
            }
        }

        /// <summary>
        /// Calculates available amounts values and totals for the payment refund collection.
        /// </summary>
        /// <param name="paymentRefunds">Payment refund collection</param>
        /// <param name="retailTransaction">Retail transaction</param>
        private static void CalculateAvailableAmountsAndTotals(ICollection<PaymentRefundItem> paymentRefunds, IRetailTransaction retailTransaction)
        {
            decimal totalAmountAvailableForRefund =
                retailTransaction.SaleIsReturnSale ?
                Math.Abs(retailTransaction.TransSalePmtDiff) :
                (from pr in paymentRefunds let availableAmount = pr.OriginalAmount - pr.ReturnedEarlierAmount select availableAmount).Sum();

            decimal paidAmountTotal = decimal.Zero;
            decimal returnedEarlierAmountTotal = decimal.Zero;
            decimal returnedAmountTotal = decimal.Zero;
            decimal availableAmountTotal = decimal.Zero;

            foreach (var paymentRefund in paymentRefunds)
            {
                decimal availableForPaymentMethod = paymentRefund.OriginalAmount - paymentRefund.ReturnedAmount - paymentRefund.ReturnedEarlierAmount;
                if (availableForPaymentMethod < decimal.Zero)
                    availableForPaymentMethod = decimal.Zero;

                paymentRefund.AvailableAmount = Math.Min(availableForPaymentMethod, totalAmountAvailableForRefund);

                paidAmountTotal += paymentRefund.OriginalAmount;
                returnedEarlierAmountTotal += paymentRefund.ReturnedEarlierAmount;
                returnedAmountTotal += paymentRefund.ReturnedAmount;
            }

            availableAmountTotal = paidAmountTotal - returnedEarlierAmountTotal - returnedAmountTotal;
            if (availableAmountTotal > totalAmountAvailableForRefund)
            {
                availableAmountTotal = totalAmountAvailableForRefund;
            }

            // Add 'Total' line item.
            paymentRefunds.Add(
                new PaymentRefundItem()
                {
                    EntryName = ApplicationLocalizer.Language.Translate(54),
                    OriginalAmount = paidAmountTotal,
                    ReturnedEarlierAmount = returnedEarlierAmountTotal,
                    ReturnedAmount = returnedAmountTotal,
                    AvailableAmount = availableAmountTotal
                });
        }

        private TenderData tenderData;

        /// <summary>
        /// Refund amount type
        /// </summary>
        private enum RefundAmountType
        {
            /// <summary>
            /// Paid amount
            /// </summary>
            PaidAmount,
            /// <summary>
            /// Returned amount
            /// </summary>
            ReturnedAmount,
            /// <summary>
            /// Amount returned earlier
            /// </summary>
            ReturnedEarlierAmount,
            /// <summary>
            /// Available amount
            /// </summary>
            AvailableAmount
        }
        #endregion

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
    }

    /// <summary>
    /// Defines sale payment refund structure.
    /// </summary>
    internal sealed class PaymentRefundItem
    {
        /// <summary>
        /// Gets or sets entry ID.
        /// </summary>
        public string EntryId { get; set; }

        /// <summary>
        /// Gets or sets entry type.
        /// </summary>
        public RefundAmountEntryType EntryType { get; set; }

        /// <summary>
        /// Gets or sets the entry name.
        /// </summary>
        public string EntryName { get; set; }

        /// <summary>
        /// Gets or sets original amount.
        /// </summary>
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// Gets or sets returned amount.
        /// </summary>
        public decimal ReturnedAmount { get; set; }

        /// <summary>
        /// Gets or sets amount returned in earlier transactions.
        /// </summary>
        public decimal ReturnedEarlierAmount { get; set; }

        /// <summary>
        /// Gets or sets available amount for return for the current tender type ID.
        /// </summary>
        public decimal AvailableAmount { get; set; }

        /// <summary>
        /// Returns Refund item key based on its entry id and type
        /// </summary>
        /// <param name="entryType">Entry type</param>
        /// <param name="entryId">Entry ID</param>
        /// <returns>Refund item key</returns>
        public static string GetKey(RefundAmountEntryType entryType, string entryId)
        {
            return (int)entryType + entryId;
        }
    }

    /// <summary>
    /// Payment refund collection keyed by entry type + entry ID.
    /// </summary>
    internal sealed class PaymentRefundCollection : KeyedCollection<string, PaymentRefundItem>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="PaymentRefundItem"/>
        /// </summary>
        public PaymentRefundCollection() : base() { }

        protected override string GetKeyForItem(PaymentRefundItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return PaymentRefundItem.GetKey(item.EntryType, item.EntryId);
        }
    }
}
