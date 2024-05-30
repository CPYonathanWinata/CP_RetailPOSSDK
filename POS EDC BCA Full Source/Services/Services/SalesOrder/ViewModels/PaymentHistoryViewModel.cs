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
using System.Linq;
using LSRetailPosis.Transaction;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class PaymentHistoryViewModel : PageViewModel
    {
        private ReadOnlyCollection<PaymentHistoryLineViewModel> payments;        
                
        public PaymentHistoryViewModel(CustomerOrderTransaction customerOrderTransaction)
        {
            this.SetTransaction(customerOrderTransaction);

            // Create a collection of PaymentHistoryLineViewModels from each CustomerOrderPaymentHistory
            List<PaymentHistoryLineViewModel> viewModels = (from lineItem in this.Transaction.PaymentHistory
                                                            select new PaymentHistoryLineViewModel(lineItem, this.Transaction.OrderId)).ToList<PaymentHistoryLineViewModel>();

            payments = new ReadOnlyCollection<PaymentHistoryLineViewModel>(viewModels);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification="Used by databinding")]
        public ReadOnlyCollection<PaymentHistoryLineViewModel> Payments
        {
            get { return payments; }
        }        
    }

    class PaymentHistoryLineViewModel
    {
        public PaymentHistoryLineViewModel(CustomerOrderPaymentHistory line, string orderId)
        {
            this.Date        = line.Date.ToShortDateString();
            this.Amount      = SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(line.Amount, true, false);
            this.OrderNumber = orderId;
            this.Balance = SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(line.Balance, true, false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification="Used by databinding")]
        public string Date
        {
            get;
            private set;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by databinding")]
        public string Amount
        {
            get;
            private set;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by databinding")]
        public string OrderNumber
        {
            get;
            private set;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by databinding")]
        public string Balance
        {
            get;
            private set;
        }
    }
}
