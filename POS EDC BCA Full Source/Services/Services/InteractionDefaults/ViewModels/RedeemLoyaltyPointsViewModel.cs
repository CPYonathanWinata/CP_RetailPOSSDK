/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.ComponentModel;
using System.Linq;
using LSRetailPosis.POSProcesses.ViewModels;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    class RedeemLoyaltyPointsViewModel : INotifyPropertyChanged
    {
        #region ctor

        internal RedeemLoyaltyPointsViewModel(string cardNumber, decimal amountAvailableForDiscount)
        {
            CardNumber = cardNumber;
            AmountAvailableForDiscount = PosApplication.Instance.Services.Rounding.Round(amountAvailableForDiscount, true);

            // Hook up MSR/Scanner
            HookUpPeripherals();
            EnablePosDevices();

            RetrieveLoyaltyCardBalance();
        }

        #endregion

        #region Properties

        private string cardNumber;
        /// <summary>
        /// Gets or sets the card number
        /// </summary>
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                if (!string.Equals(cardNumber, value, System.StringComparison.Ordinal))
                {
                    cardNumber = value;
                    ClearCardBalanceProperties();
                    OnPropertyChanged(PropertyCardNumber);
                }
            }
        }

        /// <summary>
        /// Gets or sets the discount amount
        /// </summary>
        public decimal DiscountAmount { get; set; }

        private string pointsBalance;
        /// <summary>
        /// Gets or sets loyalty points balance
        /// </summary>
        public string PointsBalance
        {
            get
            {
                return pointsBalance;
            }
            set
            {
                if (pointsBalance != value)
                {
                    pointsBalance = value;
                    OnPropertyChanged(PropertyPointsBalance);
                }
            }
        }

        private string maxDiscountAmount;
        /// <summary>
        /// Gets or sets maximum discount amount
        /// </summary>
        public string MaxDiscountAmount
        {
            get
            {
                return maxDiscountAmount;
            }
            set
            {
                if (maxDiscountAmount != value)
                {
                    maxDiscountAmount = value;
                    OnPropertyChanged(PropertyMaxDiscountAmount);
                }
            }
        }

        private string amountAvailableForDiscount;
        /// <summary>
        /// Gets or sets Amount available for discount
        /// </summary>
        public string AmountAvailableForDiscount
        {
            get
            {
                return amountAvailableForDiscount;
            }
            set
            {
                if (amountAvailableForDiscount != value)
                {
                    amountAvailableForDiscount = value;
                    OnPropertyChanged(PropertyAmountAvailableForDiscount);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Retrieves loyalty card balance if the card is valid
        /// </summary>
        /// <returns></returns>
        public bool RetrieveLoyaltyCardBalance()
        {
            if (!string.IsNullOrWhiteSpace(CardNumber))
            {
                ILoyaltyItem loyaltyItem = PosApplication.Instance.Services.Loyalty.GetLoyaltyItem(CardNumber);
                if (loyaltyItem != null)
                {
                    PointsBalance = PosApplication.Instance.Services.Rounding.Round(decimal.Negate(loyaltyItem.RewardPointLines.Sum(l => l.RewardPointAmountQuantity)), false);
                    MaxDiscountAmount = PosApplication.Instance.Services.Rounding.Round(decimal.Negate(loyaltyItem.Amount), true);
                    return true;
                }
                else
                {
                    CardNumber = string.Empty;
                }
            }
            return false;
        }

        /// <summary>
        /// Clears all view model property values.
        /// </summary>
        public void ClearCardBalanceProperties()
        {
            PointsBalance = string.Empty;
            MaxDiscountAmount = string.Empty;
        }

        public void HookUpPeripherals()
        {
            // Hook up MSR/Scanner
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(ProcessScannedItem);
            PosApplication.Instance.Services.Peripherals.MSR.MSRMessageEvent -= new MSRMessageEventHandler(ProcessSwipedCard);
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent += new ScannerMessageEventHandler(ProcessScannedItem);
            PosApplication.Instance.Services.Peripherals.MSR.MSRMessageEvent += new MSRMessageEventHandler(ProcessSwipedCard);
        }

        public void UnHookPeripherals()
        {
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(ProcessScannedItem);
            PosApplication.Instance.Services.Peripherals.MSR.MSRMessageEvent -= new MSRMessageEventHandler(ProcessSwipedCard);
        }

        public void ProcessScannedItem(IScanInfo scanInfo)
        {
            if (scanInfo.ScanData.Length > 0)
            {
                CardNumber = scanInfo.ScanData;
                RetrieveLoyaltyCardBalance();
            }
        }

        public void ProcessSwipedCard(ICardInfo cardInfo)
        {
            if ((cardInfo.Track2Parts != null) && (cardInfo.Track2Parts.Length > 0))
            {
                CardNumber = cardInfo.Track2Parts[0];
                RetrieveLoyaltyCardBalance();
            }
        }

        public static void EnablePosDevices()
        {
            PosApplication.Instance.Services.Peripherals.Scanner.ReEnableForScan();
            PosApplication.Instance.Services.Peripherals.MSR.EnableForSwipe();
        }

        public static void DisablePosDevices()
        {
            PosApplication.Instance.Services.Peripherals.Scanner.DisableForScan();
            PosApplication.Instance.Services.Peripherals.MSR.DisableForSwipe();
        }

        #endregion

        #region INotifyPropertyChanged interface
        /// <summary>
        /// Raised when a property on this view model changes it's value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constants defining the property names.
        /// </summary>
        private const string PropertyCardNumber = "CardNumber";
        private const string PropertyPointsBalance = "PointsBalance";
        private const string PropertyMaxDiscountAmount = "MaxDiscountAmount";
        private const string PropertyAmountAvailableForDiscount = "AmountAvailableForDiscount";

        /// <summary>
        /// Raises the object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property which value was changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
    }
}