/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using LSRetailPosis;
using LSRetailPosis.BusinessLogic.PropertyClasses;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings.HardwareProfiles;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// View model for the LoyaltyCardForm.
    /// </summary>
    internal class LoyaltyCardFormViewModel : INotifyPropertyChanged
    {
        #region Private properties

        /// <summary>
        /// Gets or sets the <see cref="ILoyaltyCardData"/> object.
        /// </summary>
        private ILoyaltyCardData LoyaltyCardData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IOperation"/> to be executed.
        /// </summary>
        private IOperation Operation { get; set; }

        /// <summary>
        /// Gets or sets the last requested card number.
        /// </summary>
        private string LastRequestedCardNumber { get; set; }

        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets loyalty card number.
        /// </summary>
        public string CardNumber
        {
            get { return LoyaltyCardData.CardNumber; }
            set { LoyaltyCardData.CardNumber = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card customer name.
        /// </summary>
        public string CustomerName
        {
            get { return LoyaltyCardData.CustomerName; }
            set { LoyaltyCardData.CustomerName = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card tender type.
        /// </summary>
        public LoyaltyCardTenderType CardType
        {
            get { return LoyaltyCardData.CardType; }
            set { LoyaltyCardData.CardType = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card tender type string.
        /// </summary>
        public string CardTypeString
        {
            get { return LoyaltyCardData.CardTypeString; }
            set { LoyaltyCardData.CardTypeString = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card status string.
        /// </summary>
        public string StatusString
        {
            get { return LoyaltyCardData.StatusString; }
            set { LoyaltyCardData.StatusString = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card issued points.
        /// </summary>
        public decimal IssuedPoints
        {
            get { return LoyaltyCardData.IssuedPoints; }
            set { LoyaltyCardData.IssuedPoints = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card used points.
        /// </summary>
        public decimal UsedPoints
        {
            get { return LoyaltyCardData.UsedPoints; }
            set { LoyaltyCardData.UsedPoints = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card expired points.
        /// </summary>
        public decimal ExpiredPoints
        {
            get { return LoyaltyCardData.ExpiredPoints; }
            set { LoyaltyCardData.ExpiredPoints = value; }
        }

        /// <summary>
        /// Gets or sets the loyalty card balance points.
        /// </summary>
        public decimal BalancePoints
        {
            get { return LoyaltyCardData.BalancePoints; }
            set { LoyaltyCardData.BalancePoints = value; }
        }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructs <see cref="LoyaltyCardFormViewModel"/> based on the <param name="context"/> provided.
        /// </summary>
        /// <param name="context">The context of the view model.</param>
        internal LoyaltyCardFormViewModel(LoyaltyCardFormContext context)
        {
            LoyaltyCardData = new LoyaltyCardData();
            Operation = OperationFactory.Create(context, this);
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Clears all view model property values.
        /// </summary>
        public void ClearViewModelProperties()
        {
            LoyaltyCardData = new LoyaltyCardData();
            NotifyAllPropertiesChanged();
        }

        /// <summary>
        /// Gets the title of the form based on current context.
        /// </summary>
        /// <returns>Form title string.</returns>
        public string GetFormTitle()
        {
            return ApplicationLocalizer.Language.Translate(Operation.GetDescriptionResourceId());
        }

        /// <summary>
        /// Gets the Get button label text.
        /// </summary>
        /// <returns>Get button label string.</returns>
        public string GetGetButtonLabelText()
        {
            return ApplicationLocalizer.Language.Translate(Operation.GetButtonResourceId());
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
                LoyaltyCardData.CardNumber = scanInfo.ScanData;
                Operation.PreExecute();
            }
        }

        public void ProcessSwipedCard(ICardInfo cardInfo)
        {
            if ((cardInfo.Track2Parts != null) && (cardInfo.Track2Parts.Length > 0))
            {
                LoyaltyCardData.CardNumber = cardInfo.Track2Parts[0];
                Operation.PreExecute();
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

        public bool PreExecute()
        {
            return Operation.PreExecute();
        }

        public bool Execute()
        {
            return Operation.Execute();
        }

        public static void ShowErrorMessage(string message)
        {
            PosApplication.Instance.Services.Dialog.ShowMessage(message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Holds the constants referencing the string resource identifiers related to Loyalty.
        /// </summary>
        public static class Resources
        {
            public const int LoyaltyCardNumber = 55500; //Loyalty card number:
            public const int LoyaltyCardBalance = 55501; //Loyalty points balance:
            public const int LoyaltyCardCustomerName = 55502; //Customer name:
            public const int LoyaltyCardScheme = 55503; //Loyalty scheme:
            public const int LoyaltyCardStatus = 55504; //Card status:
            public const int LoyaltyCardType = 55505; //Card type:
            public const int LoyaltyCardIssuedPoints = 55506; //Issued points:
            public const int LoyaltyCardUsedPoints = 55507; //Used points:
            public const int LoyaltyCardExpiredPoints = 55508; //Expired points:
            public const int LoyaltyCardBalanceTitle = 55511; //Loyalty card balance
        }

        /// <summary>
        /// Interface for the operation performed in the POS.
        /// </summary>
        private interface IOperation
        {
            /// <summary>
            /// Executes perliminary part of the operation.
            /// </summary>
            /// <returns>true if the operation succeeded; false otherwise.</returns>
            bool PreExecute();
            /// <summary>
            /// Executes the operation.
            /// </summary>
            /// <returns>true if the operation succeeded; false otherwise.</returns>
            bool Execute();
            /// <summary>
            /// Gets an integer ID of the string resource displayed on the button calling the operation.
            /// </summary>
            /// <returns>An integer ID of the string resource.</returns>
            int GetButtonResourceId();
            /// <summary>
            /// Gets an integer ID of the string resource describing the operation.
            /// </summary>
            /// <returns>An integer ID of the string resource.</returns>
            /// <remarks>This value is used as the resource ID of the form title.</remarks>
            int GetDescriptionResourceId();
        }

        /// <summary>
        /// A base class for the Loyalty form view model operation.
        /// </summary>
        private abstract class LoyaltyCardFormViewModelOperation : IOperation
        {
            #region Properties
            /// <summary>
            /// Gets or sets the <see cref="LoyaltyCardFormViewModel"/>.
            /// </summary>
            protected LoyaltyCardFormViewModel ViewModel { get; set; }
            #endregion

            #region IOperation members
            /// <summary>
            /// Executes preliminary part of the operation.
            /// </summary>
            /// <returns>true if the operation succeeded; false otherwise.</returns>
            public abstract bool PreExecute();

            /// <summary>
            /// Executes the operation.
            /// </summary>
            /// <returns>true if the operation succeeded; false otherwise.</returns>
            public abstract bool Execute();

            /// <summary>
            /// Gets an integer ID of the string resource displayed on the button calling the operation.
            /// </summary>
            /// <returns>An integer ID of the string resource.</returns>
            public abstract int GetButtonResourceId();

            /// <summary>
            /// Gets an integer ID of the string resource describing the operation.
            /// </summary>
            /// <returns>An integer ID of the string resource.</returns>
            /// <remarks>This value is used as the resource ID of the form title.</remarks>
            public abstract int GetDescriptionResourceId();
            #endregion

            #region Ctor
            /// <summary>
            /// Constructs an instance of the <see cref="LoyaltyCardFormViewModelOperation"/> based on the <paramref name="viewModel"/> object provided.
            /// </summary>
            /// <param name="viewModel">An instance of the <see cref="LoyaltyCardFormViewModel"/>.</param>
            public LoyaltyCardFormViewModelOperation(LoyaltyCardFormViewModel viewModel)
            {
                ViewModel = viewModel;
            }
            #endregion
        }

        private class LoyaltyCardBalance : LoyaltyCardFormViewModelOperation
        {
            #region Ctor
            /// <summary>
            /// Constructs an insnacne of the <see cref="LoyaltyCardBalance"/> class.
            /// </summary>
            /// <param name="viewModel">The view model providing the context for the operation.</param>
            public LoyaltyCardBalance(LoyaltyCardFormViewModel viewModel) : base(viewModel) { }
            #endregion

            #region IOperation members
            /// <summary>
            /// Submits a request for loyalty card balance through TS.
            /// </summary>
            /// <returns>true if request succeeded; false otherwise.</returns>
            public override bool PreExecute()
            {
                if (string.IsNullOrWhiteSpace(ViewModel.LoyaltyCardData.CardNumber))
                {
                    ViewModel.ClearViewModelProperties();
                    return false;
                }

                if (PosApplication.Instance.TransactionServices.CheckConnection())
                {
                    ViewModel.LastRequestedCardNumber = ViewModel.CardNumber;

                    ViewModel.LoyaltyCardData = PosApplication.Instance.Services.Loyalty.GetLoyaltyBalanceInfo(ViewModel.LoyaltyCardData.CardNumber);

                    if (ViewModel.LoyaltyCardData != null)
                    {
                        ViewModel.NotifyAllPropertiesChanged();
                    }
                    else
                    {
                        ViewModel.ClearViewModelProperties();
                        ViewModel.CardNumber = ViewModel.LastRequestedCardNumber;
                        ViewModel.OnPropertyChanged(LoyaltyCardFormViewModel.PropertyCardNumber);
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Prints the loyalty card balance on the fiscal printer.
            /// </summary>
            /// <returns>true if the operation succeeded; false otherwise.</returns>
            public override bool Execute()
            {
                if (FiscalPrinter.DriverType == FiscalPrinterDriver.None)
                {
                    throw new LoyaltyCardException(ApplicationLocalizer.Language.Translate(86501));
                }

                if (string.IsNullOrEmpty(ViewModel.LoyaltyCardData.CardNumber))
                {
                    return false;
                }

                // If balance already queried skip TS call.
                if (string.IsNullOrEmpty(ViewModel.LastRequestedCardNumber) && !string.Equals(ViewModel.LoyaltyCardData.CardNumber, ViewModel.LastRequestedCardNumber, StringComparison.OrdinalIgnoreCase))
                {
                    bool preExecutionSuceeded = this.PreExecute();
                    if (!preExecutionSuceeded)
                    {
                        return false;
                    }
                }

                PosApplication.Instance.Services.Loyalty.PrintLoyaltyBalance(ViewModel.LoyaltyCardData);

                return true;
            }

            public override int GetButtonResourceId()
            {
                return 55405; // Check balance
            }

            public override int GetDescriptionResourceId()
            {
                return Resources.LoyaltyCardBalanceTitle;
            }
            #endregion
        }

        /// <summary>
        /// A static factory creating <see cref="IOperation"/> interface implementers.
        /// </summary>
        private static class OperationFactory
        {
            /// <summary>
            /// Creates an instance of the <see cref="IOperation"/> interface based on the <paramref name="context"/> and <paramref name="viewModel"/> provided.
            /// </summary>
            /// <param name="context">Defines the type of the operation.</param>
            /// <param name="viewModel">An instance of the <see cref="LoyaltyCardFormViewModel"/> providing the context.</param>
            /// <returns><see cref="IOperation"/> interface implementer.</returns>
            public static IOperation Create(LoyaltyCardFormContext context, LoyaltyCardFormViewModel viewModel)
            { 
                switch (context)
                {
                    case LoyaltyCardFormContext.LoyaltyCardBalance:
                        return new LoyaltyCardBalance(viewModel);
                    default:
                        throw new NotImplementedException(); // Other contexts are not implemented yet, but we will never get here as we do not pass any other context values.
                }
            }
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
        public const string PropertyCardNumber = "CardNumber";
        public const string PropertyCustomerName = "CustomerName";
        public const string PropertyCardTypeString = "CardTypeString";
        public const string PropertyStatusString = "StatusString";
        public const string PropertyIssuedPoints = "IssuedPoints";
        public const string PropertyUsedPoints = "UsedPoints";
        public const string PropertyExpiredPoints = "ExpiredPoints";
        public const string PropertyBalancePoints = "BalancePoints";


        /// <summary>
        /// Raises the object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property which value was changed.</param>
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

        /// <summary>
        /// Notifies all the subscribers that all the properties were changed.
        /// </summary>
        private void NotifyAllPropertiesChanged()
        {
            OnPropertyChanged(PropertyCardNumber);
            OnPropertyChanged(PropertyCustomerName);
            OnPropertyChanged(PropertyCardTypeString);
            OnPropertyChanged(PropertyStatusString);
            OnPropertyChanged(PropertyIssuedPoints);
            OnPropertyChanged(PropertyUsedPoints);
            OnPropertyChanged(PropertyExpiredPoints);
            OnPropertyChanged(PropertyBalancePoints);
        }
        #endregion
    }

    /// <summary>The exception that is thrown when something goes wrong in the secure remoting channel.</summary>
    [Serializable]
    public class LoyaltyCardException : Exception
    {
        /// <summary>Initializes a new instance of the LoyaltyCardException class with default properties.</summary>
        public LoyaltyCardException()
        {
        }

        /// <summary>Initializes a new instance of the LoyaltyCardException class with the given message.</summary>
        /// <param name="message">The error message that explains why the exception occurred.</param>
        public LoyaltyCardException(string message) :
            base(message)
        {
        }

        /// <summary>Initializes a new instance of the LoyaltyCardException class with the specified properties.</summary>
        /// <param name="message">The error message that explains why the exception occurred.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public LoyaltyCardException(string message, System.Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>Initializes the exception with serialized information.</summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected LoyaltyCardException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
