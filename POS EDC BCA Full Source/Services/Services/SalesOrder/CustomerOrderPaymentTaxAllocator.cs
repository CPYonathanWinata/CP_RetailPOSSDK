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
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SalesOrder.CustomerOrderParameters;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    /// <summary>
    /// Class for allocating customer order prepayment taxes to order payments.
    /// </summary>
    internal class CustomerOrderPrepaymentTaxAllocator
    {
        private CustomerOrderTransaction customerOrderTransaction;
        private ICollection<ITaxItem> prepaymentTaxLines;
        private Queue<Tuple<ITenderLineItem, PaymentInfo>> payments = new Queue<Tuple<ITenderLineItem, PaymentInfo>>();
        private bool hasPrepaymentTaxes;

        /// <summary>
        /// Constructs instance of CustomerOrderPrepaymentTaxAllocator.
        /// </summary>
        /// <param name="customerOrderTransaction">Customer order transaction.</param>
        public CustomerOrderPrepaymentTaxAllocator(CustomerOrderTransaction customerOrderTransaction)
        {
            this.customerOrderTransaction = customerOrderTransaction;
            var custOrder = customerOrderTransaction as ICustomerOrderTransactionV4;

            hasPrepaymentTaxes = custOrder != null && custOrder.PrepaymentTaxLines != null
                && custOrder.PrepaymentTaxLines.Count > 0 && customerOrderTransaction.AmountDue != 0;

            if (hasPrepaymentTaxes)
            {
                prepaymentTaxLines = custOrder.PrepaymentTaxLines;
            }
        }

        /// <summary>
        /// Adds payment.
        /// </summary>
        /// <param name="tender">Tender line item.</param>
        /// <param name="paymentInfo">Serialized payment.</param>
        public void AddPayment(ITenderLineItem tender, PaymentInfo paymentInfo)
        {
            if (hasPrepaymentTaxes)
            {
                payments.Enqueue(new Tuple<ITenderLineItem, PaymentInfo>(tender, paymentInfo));
            }
        }

        /// <summary>
        /// Processes order prepayment.
        /// </summary>
        public void ProcessOrderPrepayment()
        {
            if (prepaymentTaxLines != null)
            {
                var accumulatedTaxes = new Dictionary<string, decimal>();

                foreach (var taxLine in prepaymentTaxLines)
                {
                    accumulatedTaxes[taxLine.TaxCode] = 0m;
                }

                while (payments.Count > 0)
                {
                    var payment = payments.Dequeue();
                    AddTaxLinesToPayment(payment.Item1, payment.Item2, accumulatedTaxes, payments.Count == 0);
                }
            }
        }

        private void AddTaxLinesToPayment(ITenderLineItem tender, PaymentInfo paymentInfo, Dictionary<string, decimal> accumulatedTaxes, bool lastPayment)
        {
            if (prepaymentTaxLines != null)
            {
                decimal paymentTaxAmount;

                foreach (var taxLine in prepaymentTaxLines)
                {
                    if (!lastPayment)
                    {
                        paymentTaxAmount = tender.Amount * taxLine.Amount / customerOrderTransaction.AmountDue;
                        paymentTaxAmount = SalesOrder.InternalApplication.Services.Rounding.Round(paymentTaxAmount);
                        accumulatedTaxes[taxLine.TaxCode] = accumulatedTaxes[taxLine.TaxCode] + paymentTaxAmount;
                    }
                    else
                    {
                        paymentTaxAmount = taxLine.Amount - accumulatedTaxes[taxLine.TaxCode];
                    }

                    if (!string.IsNullOrEmpty(tender.CurrencyCode))
                    {
                        paymentTaxAmount = SalesOrder.InternalApplication.Services.Currency.CurrencyToCurrency(ApplicationSettings.Terminal.StoreCurrency, tender.CurrencyCode, paymentTaxAmount);
                    }

                    if (paymentInfo.PrepaymentTaxLines == null)
                    {
                        paymentInfo.PrepaymentTaxLines = new Collection<TaxLineInfo>();
                    }
                    paymentInfo.PrepaymentTaxLines.Add(new TaxLineInfo() { TaxCode = taxLine.TaxCode, Amount = paymentTaxAmount });
                }
            }
        }
    }
}
