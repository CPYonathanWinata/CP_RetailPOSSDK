/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.TransactionServices;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Dynamics.Retail.Pos.GiftCard.Properties;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Dynamics.Retail.Pos.GiftCard
{
    /// <summary>
    /// Summary description for GiftCardForm.
    /// </summary>
    partial class GiftCardForm : frmTouchBase
    {
        //start custom disable
        private Stopwatch stopwatch;
        private const int MaxKeyPressInterval = 20;
        char cforKeyDown = '\0';
        int _lastKeystroke = DateTime.Now.Millisecond;
        List<char> _barcode = new List<char>(1);
        bool UseKeyboard = false;

        //end
        protected GiftCardForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        public GiftCardForm(GiftCardController giftCardController)
            : this()
        {
            // Create the view model bindings and listen to changes
            this.payGiftCardViewModelBindingSource.Add(giftCardController);
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(OnViewModel_PropertyChanged);

            //
            // Coallesce format from bindings
            //

            // Amounts
            //this.textBoxAmount.DataBindings["Text"].Format += new ConvertEventHandler(OnAmount_Convert);            // Bound to GiftCardViewModel.Amount
            this.textBoxAmount.DataBindings["Text"].Format += new ConvertEventHandler(OnBalance_Convert);          
            this.btnPaymentAmount.DataBindings["Text"].Format += new ConvertEventHandler(OnBalance_Convert);        // Bound to GiftCardViewModel.TransactionAmount
            this.textBoxAmount.DataBindings["Visible"].Format += new ConvertEventHandler(OnAmountToBool_Convert);   // Bound to GiftCardViewModel.Amount
            this.textBoxAmount.DataBindings["Enabled"].Format += new ConvertEventHandler(OnFaceValueToBool_Convert);// Bound to GiftCardViewModel.Amount
            

            this.labelAmount.DataBindings["Visible"].Format += new ConvertEventHandler(OnAmountToBool_Convert);     // Bound to GiftCardViewModel.Context

            // Card number
            this.textBoxCardNumber.DataBindings["Text"].Format += new ConvertEventHandler(OnNegateBoolean_Convert); // Bound to GiftCardViewModel.CardNumber

            // Balance
            this.btnCheckBalance.DataBindings["Visible"].Format += new ConvertEventHandler(OnBtnCheckBalanceToBool_Convert);        //Bound to GiftCardViewModel.Balance
            this.labelCheckBalanceTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnBalanceToBool_Convert);         //Bound to GiftCardViewModel.Balance
            this.labelCheckBalanceAmount.DataBindings["Visible"].Format += new ConvertEventHandler(OnBalanceToBool_Convert);        //Bound to GiftCardViewModel.Balance            
            this.labelCheckBalanceAmount.DataBindings["Text"].Format += new ConvertEventHandler(OnBalance_Convert);                 //Bound to GiftCardViewModel.Balance
            this.labelCardStatusTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);            //Bound to GiftCardViewModel.PolicyGiftCardStatus
            this.labelCardStatus.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);           //Bound to GiftCardViewModel.PolicyGiftCardStatus
            this.labelCardStatus.DataBindings["Text"].Format += new ConvertEventHandler(OnStatusToText_Convert);                    //Bound to GiftCardViewModel.PolicyGiftCardStatus
            this.labelCardActiveFromTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);        //Bound to GiftCardViewModel.PolicyActiveFrom
            this.labelCardActiveFrom.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);       //Bound to GiftCardViewModel.PolicyActiveFrom
            this.labelCardActiveFrom.DataBindings["Text"].Format += new ConvertEventHandler(OnActivationDateText_Convert);          //Bound to GiftCardViewModel.PolicyActiveFrom
            this.labelCardExpirationDateTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);    //Bound to GiftCardViewModel.PolicyExpirationDate
            this.labelCardExpirationDate.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);   //Bound to GiftCardViewModel.PolicyExpirationDate
            this.labelCardExpirationDate.DataBindings["Text"].Format += new ConvertEventHandler(OnExpirationDate_Convert);          //Bound to GiftCardViewModel.PolicyExpirationDate
            this.labelReloadableTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);            //Bound to GiftCardViewModel.PolicyNonReloadable
            this.labelRedemptionTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);            //Bound to GiftCardViewModel.PolicyOneTimeRedemption
            this.labelCardRedemption.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);       //Bound to GiftCardViewModel.PolicyOneTimeRedemption
            this.labelCardRedemption.DataBindings["Text"].Format += new ConvertEventHandler(OnRedemptionTypeToText_Convert);        //Bound to GiftCardViewModel.PolicyOneTimeRedemption
            this.labelCardReloadble.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);        //Bound to GiftCardViewModel.PolicyNonReloadable
            this.labelCardReloadble.DataBindings["Text"].Format += new ConvertEventHandler(OnNonReloadTypeToBool_Convert);          //Bound to GiftCardViewModel.PolicyNonReloadable
            this.labelCardMaxBalanceTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);        //Bound to GiftCardViewModel.PolicyMaxBalanceAllowed
            this.labelCardMaxBalance.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);       //Bound to GiftCardViewModel.PolicyMaxBalanceAllowed
            this.labelCardMaxBalance.DataBindings["Text"].Format += new ConvertEventHandler(OnMaxBalance_Convert);                  //Bound to GiftCardViewModel.PolicyMaxBalanceAllowed
            this.labelCardMinReloadAmountTitle.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyToBool_Convert);   //Bound to GiftCardViewModel.PolicyMinReloadAmountAllowed
            this.labelCardMinReloadAmount.DataBindings["Visible"].Format += new ConvertEventHandler(OnPolicyValuesToBool_Convert);  //Bound to GiftCardViewModel.PolicyMinReloadAmountAllowed
            this.labelCardMinReloadAmount.DataBindings["Text"].Format += new ConvertEventHandler(OnMinReloadAmount_Convert);        //Bound to GiftCardViewModel.PolicyMinReloadAmountAllowed

            this.panelAmount.DataBindings["Visible"].Format += new ConvertEventHandler(OnPaymentAmountToBool_Convert);  //Bound to GiftCardViewModel.MaxTransactionAmountAllowed
        }

        protected override void OnLoad(EventArgs e)
        {
            this.textBoxAmount.ReadOnly = true;
            string statusScan = "";
            if (!this.DesignMode)
            {
                TranslateLabels();
                
                //textBoxAmount.Enabled = false; 
                // Initialize payment amount on number pad for the pay by gift card form
                if (this.ViewModel.Context == GiftCardController.ContextType.GiftCardPayment)
                {
                    numCurrNumpad.EnteredValue = GiftCard.InternalApplication.Services.Rounding.Round(this.ViewModel.ProposedPaymentAmount, false);
                    this.textBoxAmount.Text = numCurrNumpad.EnteredValue;
                }

                // Hook up MSR/Scanner
                GiftCard.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(ProcessScannedItem);
                GiftCard.InternalApplication.Services.Peripherals.MSR.MSRMessageEvent -= new MSRMessageEventHandler(ProcessSwipedCard);
                GiftCard.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent += new ScannerMessageEventHandler(ProcessScannedItem);
                GiftCard.InternalApplication.Services.Peripherals.MSR.MSRMessageEvent += new MSRMessageEventHandler(ProcessSwipedCard);
                ////new
                //stopwatch = new Stopwatch();
                //stopwatch.Start();
                //this.KeyDown += new KeyEventHandler(BarcodeReader_KeyDown);
                //this.KeyUp += new KeyEventHandler(BarcodeReader_KeyUp);


                //add new scan only - yonathan 26092024
                if (GiftCard.InternalApplication.TransactionServices.CheckConnection())
                {
                    ReadOnlyCollection<object>  containerArray = GiftCard.InternalApplication.TransactionServices.InvokeExtension("getInputStatus", ApplicationSettings.Database.DATAAREAID);
                    statusScan = containerArray[3].ToString();
                    if ( statusScan == "True")
                    {
                        this.textBoxCardNumber.ReadOnly = true;
                        labelCardNumber.Text = labelCardNumber.Text + " (HANYA SCAN BARCODE)";
                        this.textBoxCardNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BarcodeReader_KeyDown);
                        this.textBoxCardNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BarcodeReader_KeyUp);
                    }
                    else
                    {
                        this.textBoxCardNumber.ReadOnly = false;
                    }
                    
                }
                

                
                //end
                EnablePosDevices();
            }
            

            base.OnLoad(e);
            
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DisablePosDevices();

            base.OnFormClosed(e);
        }

       
        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmPayGiftCard are reserved at 55400 - 55499
            // In use now are ID's: 55400 - 55431
            //
            this.Text = labelHeading.Text           = this.ViewModel.GetFormTitle();
            this.numCurrNumpad.PromptText           = ApplicationLocalizer.Language.Translate(50017);   // Enter amount
            this.labelCardNumber.Text               = ApplicationLocalizer.Language.Translate(55401);   // Gift card number
            this.labelAmount.Text                   = "Amount Voucher";//ApplicationLocalizer.Language.Translate(50008);   // Amount //add by yonathan 28/12/2022 to change label name
            this.labelPaymentAmount.Text            = ApplicationLocalizer.Language.Translate(50083);   // Payment amount
            this.btnOk.Text                         = ApplicationLocalizer.Language.Translate(55403);   // OK
            this.btnCancel.Text                     = ApplicationLocalizer.Language.Translate(55404);   // Cancel
            this.labelCheckBalanceTitle.Text        = ApplicationLocalizer.Language.Translate(55402);   // Available balance
            this.labelCheckBalanceAmount.Text       = string.Empty;
            this.btnCheckBalance.Text               = ApplicationLocalizer.Language.Translate(55417);   // Check balance
            this.labelCardStatusTitle.Text          = ApplicationLocalizer.Language.Translate(55418);   // Status
            this.labelCardStatus.Text               = String.Empty;
            this.labelCardActiveFromTitle.Text      = ApplicationLocalizer.Language.Translate(55422);   // Activation date and time
            this.labelCardActiveFrom.Text           = String.Empty;
            this.labelCardExpirationDateTitle.Text  = ApplicationLocalizer.Language.Translate(55423);   //Expiration date
            this.labelCardExpirationDate.Text       = String.Empty;
            this.labelRedemptionTitle.Text          = ApplicationLocalizer.Language.Translate(55425);   // Redemption
            this.labelCardRedemption.Text           = String.Empty;
            this.labelReloadableTitle.Text          = ApplicationLocalizer.Language.Translate(55424);   // Reloadable
            this.labelCardReloadble.Text            = String.Empty;
            this.labelCardMaxBalanceTitle.Text      = ApplicationLocalizer.Language.Translate(55429);    // Maximum balance
            this.labelCardMaxBalance.Text           = String.Empty;
            this.labelCardMinReloadAmountTitle.Text = ApplicationLocalizer.Language.Translate(55428);   // Minimum reload amount
            this.labelCardMinReloadAmount.Text      = String.Empty; 
            
            // Set context-specific labels based on which gift card UI is displaying
            switch (this.ViewModel.Context)
            {
                case GiftCardController.ContextType.GiftCardBalance:
                    this.btnOk.Text = ApplicationLocalizer.Language.Translate(55410); // Print receipt
                    break;

                case GiftCardController.ContextType.GiftCardPayment:
                    this.labelHeading.Text = ApplicationLocalizer.Language.Translate(55416,
                        GiftCard.InternalApplication.Services.Rounding.Round(this.ViewModel.ProposedPaymentAmount, true));
                    break;

                case GiftCardController.ContextType.GiftCardIssue:
                case GiftCardController.ContextType.GiftCardAddTo:
                    break;
            }
        }

        private bool Execute()
        {
            bool result = false;

            DisablePosDevices();

            try
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardPayment:
                        this.ViewModel.ValidateGiftCardPayment();
                        this.ViewModel.ValidateTenderAmount();
                        result = true;
                        break;

                    case GiftCardController.ContextType.GiftCardIssue:
                        this.ViewModel.IssueGiftCard();
                        result = true;
                        break;

                    case GiftCardController.ContextType.GiftCardAddTo:
                        this.ViewModel.AddToGiftCard();
                        result = true;
                        break;

                    case GiftCardController.ContextType.GiftCardBalance:
                        this.ViewModel.PrintGiftCardBalance();
                        result = true;
                        break;
                }
            }
            catch (PosisException px)
            {
                HandlePosIsException(px);
            }
            catch (GiftCardException gex)
            {
                HandleGiftCardException(gex);
            }

            EnablePosDevices();

            return result;
        }

        #region Properties

        private GiftCardController ViewModel
        {
            get { return (GiftCardController)payGiftCardViewModelBindingSource.Current; }
        }

        #endregion

        #region Binding Converters

        private void OnAmount_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                decimal val = (decimal)e.Value;

                // Display an empty string when amount is zero
                e.Value = (val == 0) ? string.Empty : GiftCard.InternalApplication.Services.Rounding.Round(val, false);
            }
        }

        private void OnAmountToBool_Convert(object sender, ConvertEventArgs e)
        {
            // Make visible the amount text box and label in the correct contexts
            if (e.DesiredType == typeof(bool))
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardBalance:
                        e.Value = false;
                        break;

                    case GiftCardController.ContextType.GiftCardIssue:
                    case GiftCardController.ContextType.GiftCardAddTo:
                    case GiftCardController.ContextType.GiftCardPayment:
                        e.Value = true;
                        break;
                }
            }
        }

        private void OnBalance_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                decimal val = (decimal)e.Value;

                // Display an empty string when amount is zero
                e.Value = GiftCard.InternalApplication.Services.Rounding.Round(val, true);
            }
        }

        private void OnMaxBalance_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyMaxBalanceAllowed, e.Value, null);
            }
        }

        private void OnMinReloadAmount_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyMinReloadAmountAllowed, e.Value, null);
            }
        }

        private void OnRedemptionTypeToText_Convert(object sender, ConvertEventArgs e)
        {            
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyOneTimeRedemption, e.Value, null);
            }
        }

        private void OnActivationDateText_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyActiveFrom, e.Value, null);                
            }
        }

        private void OnExpirationDate_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyExpirationDate, e.Value, null);                
            }
        }

        private void OnNonReloadTypeToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyNonReloadable, e.Value, null);
            }
        }

        private void OnStatusToText_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = GiftCard.InternalApplication.Services.GiftCard.GetGiftCardPolicyAsDisplayText(GiftCardPolicyType.PolicyCardStatus, e.Value, this.ViewModel.CreateGiftCardLineItemWithPolicies());
            }
        }

        private void OnFaceValueToBool_Convert(object sender, ConvertEventArgs e)
        {
            switch (this.ViewModel.Context)
            {
                case GiftCardController.ContextType.GiftCardIssue:
                    e.Value = this.ViewModel.IsGiftCardAmountEditable();
                    break;

                default:
                    e.Value = true;
                    break;
            }
        }
        private void OnBalanceToBool_Convert(object sender, ConvertEventArgs e)
        {
            //Make visible the check balance title label and balance label in correct contexts
            if (e.DesiredType == typeof(bool))
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardBalance:
                    case GiftCardController.ContextType.GiftCardAddTo:
                        e.Value = true;
                        break;

                    case GiftCardController.ContextType.GiftCardIssue:
                        e.Value = false;
                        break;
                        
                    case GiftCardController.ContextType.GiftCardPayment:
                    default:
                        // Only display the balance if a cardnumber has been entered
                        if (string.IsNullOrWhiteSpace(this.ViewModel.CardNumber))
                        {
                            e.Value = false;
                        }
                        else
                        {
                            e.Value = true;
                        }
                        break;
                }
            }
        }

        private void OnPolicyToBool_Convert(object sender, ConvertEventArgs e)
        {
            //Make visible policies titles in correct contexts.
            if (e.DesiredType == typeof(bool))
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardBalance:
                    case GiftCardController.ContextType.GiftCardAddTo:
                    case GiftCardController.ContextType.GiftCardIssue:
                        e.Value = ApplicationSettings.Terminal.UseGiftCardPolicies;
                        break;

                    case GiftCardController.ContextType.GiftCardPayment:
                    default:
                        // Only display policies titles if a cardnumber has been entered
                        if (string.IsNullOrWhiteSpace(this.ViewModel.CardNumber))
                        {
                            e.Value = false;
                        }
                        else
                        {
                            e.Value = this.ViewModel.IsGiftCardPoliciesChecked();
                        }
                        break;
                }
            }
        }

        private void OnPolicyValuesToBool_Convert(object sender, ConvertEventArgs e)
        {
            //Make visible the policies values in correct contexts.
            if (e.DesiredType == typeof(bool))
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardBalance:
                    case GiftCardController.ContextType.GiftCardAddTo:
                    case GiftCardController.ContextType.GiftCardIssue:
                        e.Value = this.ViewModel.IsGiftCardPoliciesChecked();                        
                        break;

                    case GiftCardController.ContextType.GiftCardPayment:
                    default:
                        // Only display the balance if a cardnumber has been entered.
                        if (string.IsNullOrWhiteSpace(this.ViewModel.CardNumber))
                        {
                            e.Value = false;
                        }
                        else
                        {
                            e.Value = this.ViewModel.IsGiftCardPoliciesChecked(); 
                        }
                        break;
                }
            }
        }
      
        private void OnBtnCheckBalanceToBool_Convert(object sender, ConvertEventArgs e)
        {
            //Make visible the check balance button in correct contexts
            if (e.DesiredType == typeof(bool))
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardBalance:
                    case GiftCardController.ContextType.GiftCardAddTo:
                        e.Value = true;
                        break;

                    case GiftCardController.ContextType.GiftCardIssue:
                        e.Value = false;
                        break;

                    case GiftCardController.ContextType.GiftCardPayment:
                    default:
                        e.Value = true;
                        break;
                }
            }
        }

        private void OnNegateBoolean_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
                e.Value = !(bool)e.Value;
        }

        private void OnPaymentAmountToBool_Convert(object sender, ConvertEventArgs e)
        {
            //Make visible the payment amount in the correct contexts
            if (e.DesiredType == typeof(bool))
            {
                switch (this.ViewModel.Context)
                {
                    case GiftCardController.ContextType.GiftCardPayment:
                        e.Value = true;
                        break;

                    case GiftCardController.ContextType.GiftCardBalance:
                    case GiftCardController.ContextType.GiftCardAddTo:
                    case GiftCardController.ContextType.GiftCardIssue:
                    default:
                        e.Value = false;
                        break;
                }
            }
        }

        #endregion

        private static void EnablePosDevices()
        {
            GiftCard.InternalApplication.Services.Peripherals.Scanner.ReEnableForScan();
            GiftCard.InternalApplication.Services.Peripherals.MSR.EnableForSwipe();
        }

        private static void DisablePosDevices()
        {
            GiftCard.InternalApplication.Services.Peripherals.Scanner.DisableForScan();
            GiftCard.InternalApplication.Services.Peripherals.MSR.DisableForSwipe();
        }

        /// <summary>
        /// Move input to next field in tab order
        /// </summary>        
        private void NextField(Control currentField)
        {
            // Clear the just entered value
            numCurrNumpad.ClearValue();

            // Dynamics 894206. DevExpress TextEdit's controls do not work with Control.SelectNextControl()
            // Obtain inner TextBox as workaround. https://www.devexpress.com/Support/Center/Question/Details/B132761
            TextEdit textEdit = currentField as TextEdit;
            if (textEdit != null)
                currentField = textEdit.MaskBox;

            // Move to the next field
            this.SelectNextControl(currentField, true, true, true, true);
        }

        private void HandleGiftCardException(GiftCardException ex)
        {
            ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            ShowErrorMessage(ex.Message);
        }

        private void HandlePosIsException(PosisException ex)
        {
            string message = string.Format(ApplicationLocalizer.Language.Translate(55002),
                    LSRetailPosis.Settings.ApplicationSettings.ShortApplicationTitle);

            ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            ShowErrorMessage(message);
        }

        private void ShowErrorMessage(string message)
        {
            using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                dialog.ShowDialog(this);
            }

            textBoxAmount.SelectAll();
        }

        #region Event handlers

        private void PadGiftCardId_CardSwept(string[] trackParts)
        {
            if ((trackParts != null) && (trackParts.Length > 0))
            {
                // Update card number field with scanned number
                UpdateCardNumberField(trackParts[0]);
            }
        }

        private void ProcessScannedItem(IScanInfo scanInfo)
        {
            if (scanInfo.ScanData.Length > 0)
            {
                // Update card number field with scanned number
                UpdateCardNumberField(scanInfo.ScanDataLabel);                
            }
        }

        private void UpdateCardNumberField(string cardNumber)
        {
            this.ViewModel.CardNumber = cardNumber;
            var currentActiveField = this.ViewModel.ActiveField;
            var currentEneteredValue = this.numCurrNumpad.EnteredValue;            

            this.ViewModel.ActiveField = InputBoxEnum.CardNumber;
            this.OnNumpad_EnterButtonPressed();

            // Do not set amount field as active if it was active before scanning and now this field became read only.
            if (!(currentActiveField == InputBoxEnum.Amount && !this.ViewModel.IsGiftCardAmountEditable()))
            {
                this.ViewModel.ActiveField = currentActiveField;

                if (currentActiveField == InputBoxEnum.Amount)
                {
                    numCurrNumpad.EnteredValue = currentEneteredValue;

                    if (this.textBoxAmount.Text.Length == 0 && numCurrNumpad.EnteredValue.Length != 0)
                    {
                        this.textBoxAmount.Text = numCurrNumpad.EnteredValue;
                    }
                }
            }         
        }

        private void ProcessSwipedCard(ICardInfo cardInfo)
        {
            PadGiftCardId_CardSwept(cardInfo.Track2Parts);
        }

        /// <summary>
        /// Called when properties on the ViewModel change
        /// </summary>        
        private void OnViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActiveField":
                    switch (this.ViewModel.ActiveField)
                    {
                        case InputBoxEnum.Amount:
                            this.numCurrNumpad.EntryType = NumpadEntryTypes.Price;
                            this.numCurrNumpad.PromptText = ApplicationLocalizer.Language.Translate(50017);
                            break;
                        case InputBoxEnum.CardNumber:
                            this.numCurrNumpad.MaxNumberOfDigits = 19;
                            this.numCurrNumpad.EntryType = NumpadEntryTypes.None;   // Gift card numbers can be generated with any characters, not just numbers
                            this.numCurrNumpad.PromptText = ApplicationLocalizer.Language.Translate(50012);
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Updates the ViewModel based on the active field when the user uses the numpad for input
        /// </summary>
        private void OnNumpad_EnterButtonPressed()
        {
            switch (this.ViewModel.ActiveField)
            {
                case InputBoxEnum.Amount:
                    // Update the view model
                    this.ViewModel.Amount = numCurrNumpad.EnteredDecimalValue;

                    // Move to the next field
                    NextField(this.textBoxAmount);
                    break;

                case InputBoxEnum.CardNumber:
                    // Update the view model
                    this.ViewModel.CardNumber = numCurrNumpad.EnteredValue;

                    try
                    {
                        if (this.ViewModel.Context == GiftCardController.ContextType.GiftCardIssue)
                        {
                            // Check the default gift card policies if the card is not created yet
                            this.ViewModel.CheckGiftCardDefaultPolicies();
                        }
                        else
                        {
                            // Check the balance of the card if the card should have been created already
                            this.ViewModel.CheckGiftCardBalance();
                            // Check the gift card policies if the card exists
                            this.ViewModel.CheckGiftCardPolicies();
                        }
                    }
                    catch (GiftCardException gex)
                    {
                        HandleGiftCardException(gex);
                    }

                    // Clear the just entered value but don't move to next field - allows continuous checking of multiple card balances
                    numCurrNumpad.ClearValue();
                    break;
                default:
                    break;
            }
        }

        private void OnAmountTextBox_Enter(object sender, EventArgs e)
        {
            if (this.ViewModel.IsGiftCardAmountEditable())
            {
                this.ViewModel.ActiveField = InputBoxEnum.Amount;
            }
        }

        private void OnCardNumberTextBox_Enter(object sender, EventArgs e)
        {
            this.ViewModel.ActiveField = InputBoxEnum.CardNumber;
        }

        private void OnPaymentAmount_Click(object sender, EventArgs e)
        {
            this.ViewModel.Amount = this.ViewModel.MaxTransactionAmountAllowed;

            // Move to the next field (do not do any other processing since we have already directly set the ViewModel amount)
            NextField(this.textBoxAmount);
        }

        private void OnBtnOk_Click(object sender, EventArgs e)
        {
            if (Execute())
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void OnBtnCheckBalance_Click(object sender, EventArgs e)
        {
            try
            {
                this.ViewModel.CheckGiftCardBalance();
                this.ViewModel.CheckGiftCardPolicies();
            }
            catch (GiftCardException gex)
            {
                HandleGiftCardException(gex);
            }
        }

        private void OnTextbox_KeyPress(Object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                // Stop numeric pad value overwritting the current text box value
                this.numCurrNumpad.EnteredValue = string.Empty;

                // Jump to next object
                this.OnNumpad_EnterButtonPressed();
            }

            /*
            //new custom
            stopwatch.Stop();

            // Check the interval between key presses
            if (stopwatch.ElapsedMilliseconds > MaxKeyPressInterval)
            {
                e.Handled = true; // Block input from keyboard
            }
            else
            {
                e.Handled = false; // Allow fast input (likely from a barcode scanner)
            }

            // Restart the stopwatch for the next key press
            stopwatch.Restart();*/
        }


       
        #endregion

        /// <summary>
        /// Process input result.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnPayGiftCardViewModelBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            // Validation locks other controls in the form.
            // This will clear the offending field and allow to keep the form in working condition.
            if (e.BindingCompleteState != BindingCompleteState.Success)
            {
                e.Cancel = false;
            }
        }     


        //custom method for disabling input via keyboard
        
        private void BarcodeReader_KeyUp(object sender, KeyEventArgs e)
        {
            // if keyboard input is allowed to read

            Debug.WriteLine("Key Up - Key Code: " + e.KeyCode.ToString());
            Debug.WriteLine("Key Up - Key Data: " + e.KeyData.ToString());
            /* check if keydown and keyup is not different
             * and keydown event is not fired again before the keyup event fired for the same key
             * and keydown is not null
             * Barcode never fired keydown event more than 1 time before the same key fired keyup event
             * Barcode generally finishes all events (like keydown > keypress > keyup) of single key at a time, if two different keys are pressed then it is with keyboard
             */
            if ((cforKeyDown != (char)e.KeyCode || cforKeyDown == '\0') && e.KeyCode!=Keys.ShiftKey)
            
            {
                cforKeyDown = '\0';
                _barcode.Clear();
                return;
            }

            // getting the time difference between 2 keys
            int elapsed = (DateTime.Now.Millisecond - _lastKeystroke);

            /*
             * Barcode scanner usually takes less than 17 milliseconds to read, increase this if neccessary of your barcode scanner is slower
             * also assuming human can not type faster than 17 milliseconds
             */
            if (elapsed > 20)
                _barcode.Clear();

            // Do not push in array if Enter/Return is pressed, since it is not any Character that need to be read
            //if (e.KeyCode != Keys.Return)
            //{
            //    _barcode.Add((char)e.KeyData);
            //}
            if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Return && e.KeyCode != Keys.ShiftKey)
            {
                char keyPressed = (char)e.KeyCode;
                _barcode.Add(keyPressed);
                Debug.WriteLine("Barcode Data Added: " + keyPressed); // Check each added character
            }

            // Barcode scanner hits Enter/Return after reading barcode
            if (e.KeyCode == Keys.Return && _barcode.Count > 0)
            {
                string BarCodeData = new String(_barcode.ToArray());
                if (!UseKeyboard)
                { 
                    textBoxCardNumber.Text = String.Format("{0}", BarCodeData);


                    if (e.KeyCode == Keys.Return)
                    {
                        // Stop numeric pad value overwritting the current text box value
                        this.numCurrNumpad.EnteredValue = string.Empty;

                        // Jump to next object
                        this.OnNumpad_EnterButtonPressed();
                    }

                    //string inputValue = textBoxCardNumber.Text;

                    //if (inputValue.Length > 4)
                    //{
                    //    string maskedValue = new string('*', inputValue.Length - 4) + inputValue.Substring(inputValue.Length - 4);
                    //    //textBoxCardNumber.TextChanged -= MaskInput;  // Temporarily unsubscribe to avoid infinite loop
                    //    textBoxCardNumber.Text = maskedValue;
                    //    textBoxCardNumber.SelectionStart = maskedValue.Length; // Keep cursor at the end
                    //    //textBoxCardNumber.TextChanged += MaskInput;  // Re-subscribe
                    //}

                }//MessageBox.Show(String.Format("{0}", BarCodeData));
                _barcode.Clear();
            }
            else
            {
                textBoxCardNumber.Text = "";
            }


            // update the last key press strock time
            _lastKeystroke = DateTime.Now.Millisecond;
        }

        private void BarcodeReader_KeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine("BarcodeReader_KeyDown : " + (char)e.KeyCode);
            //_barcode.Clear();
            if (e.KeyCode != Keys.ShiftKey)
            cforKeyDown = (char)e.KeyCode;
        }
    }

    // States for input boxes in form
    enum InputBoxEnum
    {
        Amount = 0,
        CardNumber = 1,
    }
}
