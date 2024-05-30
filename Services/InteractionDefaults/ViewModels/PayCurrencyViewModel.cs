/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using LSRetailPosis.POSProcesses.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    class PayCurrencyViewModel : INotifyPropertyChanged
    {
        private decimal exchangeRate = 1M;
        private string selectedCurrency;
        private decimal currencyAmountTendered;
        private decimal? proposedPaymentAmount;

        public PayCurrencyViewModel(decimal balance)
        {
            this.Balance = balance;

            // Initialize the selected currency to the store currency
            this.SelectedCurrency = ApplicationSettings.Terminal.StoreCurrency;
        }

        public PayCurrencyViewModel(decimal balance, decimal? proposedPaymentAmount) : this(balance)
        {
            this.proposedPaymentAmount = proposedPaymentAmount;
        }

        /// <summary>
        /// Gets the balance amount in the store currency
        /// </summary>
        public decimal Balance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the proposed payment amount.
        /// </summary>
        public decimal ProposedPaymentAmount
        {
            get
            {
                return proposedPaymentAmount != null ? (decimal)proposedPaymentAmount : Balance;
            }
        }

        /// <summary>
        /// Gets the total amount due in the foreign currency
        /// </summary>
        public decimal CurrencyAmountDue
        {
            // Use proposed payment amount as the basis for calculating amount due.
            get { return this.ProposedPaymentAmount * this.ExchangeRate; }
        }

        /// <summary>
        /// Gets or sets the amount being tendered in the foreign currrency
        /// </summary>
        public decimal CurrencyAmountTendered
        {
            get { return currencyAmountTendered; }
            set
            {
                if (this.currencyAmountTendered != value)
                {
                    this.currencyAmountTendered = value;

                    OnPropertyChanged("CurrencyAmountTendered");
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected currency code
        /// </summary>
        public string SelectedCurrency
        {
            get
            {
                return this.selectedCurrency;
            }
            set
            {
                if (selectedCurrency != value)
                {
                    this.selectedCurrency = value;

                    // The exchange rate between the store currency and the selected foreign currency.
                    decimal currencyRate = PosApplication.Instance.Services.Currency.GetExchangeRate(ApplicationSettings.Terminal.StoreCurrency, selectedCurrency);
                    if (currencyRate > 0)
                    {
                        this.ExchangeRate = currencyRate;
                    }

                    OnPropertyChanged("SelectedCurrency");
                }
            }
        }

        /// <summary>
        /// Gets or sets the exchange rate
        /// </summary>
        public decimal ExchangeRate
        {
            get
            {
                return this.exchangeRate;
            }
            private set
            {
                if (this.exchangeRate != value)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Exchange rate must be greater than 0");
                    }

                    this.exchangeRate = value;

                    OnPropertyChanged("ExchangeRate");
                    OnPropertyChanged("CurrencyAmountDue");
                }
            }
        }

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
}