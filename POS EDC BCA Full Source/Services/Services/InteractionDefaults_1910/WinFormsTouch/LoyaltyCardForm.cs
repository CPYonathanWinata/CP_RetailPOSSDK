/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export("LoyaltyCardForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LoyaltyCardForm : frmTouchBase, IInteractionView
    {

        #region Members

        /// <summary>
        /// Gets or sets the view model of this form.
        /// </summary>
        private LoyaltyCardFormViewModel ViewModel { get; set; }

        private readonly LoyaltyCardFormContext context;

        #endregion

        #region Construction

        /// <summary>
        /// Default ctor.
        /// </summary>
        protected LoyaltyCardForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        [ImportingConstructor]
        public LoyaltyCardForm(LoyaltyCardConfirmation loyaltyCardConfirmation) : this()
        {
            if (loyaltyCardConfirmation == null)
            {
                throw new ArgumentNullException("loyaltyCardConfirmation");
            }

            context = loyaltyCardConfirmation.Context;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                ViewModel = new LoyaltyCardFormViewModel(context);

                bindingSource.Add(ViewModel);

                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmLoyaltyCard are reserved at 55500 - 55599
                //
                TranslateAndSetupControls();

                // Hook up MSR/Scanner
                ViewModel.HookUpPeripherals();
                LoyaltyCardFormViewModel.EnablePosDevices();
                ViewModel.ClearViewModelProperties();
            }

            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            LoyaltyCardFormViewModel.DisablePosDevices();
        }

        private void TranslateAndSetupControls()
        {
            this.Text = lblHeading.Text = ViewModel.GetFormTitle();
            padLoyaltyCardNumber.PromptText = GetLoyaltyCardNumberLabelText();
            lblLoyaltyCardBalanceTitle.Text = GetLoyaltyCardBalancePointsLabelText();
            lblLoyaltyCardCustomerNameTitle.Text = GetLoyaltyCardCustomerNameLabelText();
            lblLoyaltyCardStatusTitle.Text = GetLoyaltyCardStatusLabelText();
            lblLoyaltyCardTypeTitle.Text = GetLoyaltyCardTypeLabelText();
            lblLoyaltyCardIssuedPointsTitle.Text = GetLoyaltyCardIssuedPointsLabelText();
            lblLoyaltyCardUsedPointsTitle.Text = GetLoyaltyCardUsedPointsLabelText();
            lblLoyaltyCardExpiredPointsTitle.Text = GetLoyaltyCardExpiredPointsLabelText();

            this.btnOk.Text = GetPrintReceiptButtonLabelText();
            btnCancel.Text = GetCancelButtonLabelText();
            btnGet.Text = ViewModel.GetGetButtonLabelText();
        }

        #endregion

        #region Methods

        private void PreExecute()
        {
            ViewModel.CardNumber = padLoyaltyCardNumber.EnteredValue;

            LoyaltyCardFormViewModel.DisablePosDevices();

            try
            {
                ViewModel.PreExecute();
            }
            catch (PosisException px)
            {
                HandlePosIsException(px);
            }
            catch (LoyaltyCardException gex)
            {
                HandleLoyaltyCardException(gex);
            }

            LoyaltyCardFormViewModel.EnablePosDevices();
        }

        private bool Execute()
        {
            bool result = false;

            LoyaltyCardFormViewModel.DisablePosDevices();

            try
            {
                result = ViewModel.Execute();
                padLoyaltyCardNumber.SelectAll();
            }
            catch (PosisException px)
            {
                HandlePosIsException(px);
            }
            catch (LoyaltyCardException lex)
            {
                HandleLoyaltyCardException(lex);
            }

            LoyaltyCardFormViewModel.EnablePosDevices();

            return result;
        }

        private void HandleLoyaltyCardException(LoyaltyCardException ex)
        {
            ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            ShowErrorMessage(ex.Message);
        }

        private void HandlePosIsException(PosisException ex)
        {
            string message = string.Format(ApplicationLocalizer.Language.Translate(55512),
                    LSRetailPosis.Settings.ApplicationSettings.ShortApplicationTitle);

            ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            ShowErrorMessage(message);
        }

        private void ShowErrorMessage(string message)
        {
            LoyaltyCardFormViewModel.ShowErrorMessage(message);
            padLoyaltyCardNumber.SelectAll();
        }

        private static string GetLoyaltyCardNumberLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardNumber);
        }

        private static string GetLoyaltyCardCustomerNameLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardCustomerName);
        }

        private static string GetLoyaltyCardStatusLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardStatus);
        }

        private static string GetLoyaltyCardTypeLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardType);
        }

        private static string GetLoyaltyCardIssuedPointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardIssuedPoints);
        }

        private static string GetLoyaltyCardUsedPointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardUsedPoints);
        }

        private static string GetLoyaltyCardExpiredPointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardExpiredPoints);
        }

        private static string GetLoyaltyCardBalancePointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardBalance);
        }

        private static string GetPrintReceiptButtonLabelText()
        {
            return ApplicationLocalizer.Language.Translate(55410); // Print receipt
        }

        private static string GetCancelButtonLabelText()
        {
            return ApplicationLocalizer.Language.Translate(55404); // Cancel
        }

        #endregion

        #region Event handlers

        private void padLoyaltyCardId_EnterButtonPressed()
        {
            if (padLoyaltyCardNumber.EnteredValue.Length > 0)
            {
                PreExecute();
            }
        }

        private void padLoyaltyCardId_CardSwept(string[] trackParts)
        {
            if ((trackParts != null) && (trackParts.Length > 0))
            {
                padLoyaltyCardNumber.EnteredValue = trackParts[0];
                padLoyaltyCardNumber.Refresh();
                PreExecute();
            }
        }

        private void btnValidateLoyaltyCard_Click(object sender, EventArgs e)
        {
            PreExecute();
        }

        private void ProcessScannedItem(IScanInfo scanInfo)
        {
            if (scanInfo.ScanData.Length > 0)
            {
                padLoyaltyCardNumber.EnteredValue = scanInfo.ScanDataLabel;
                padLoyaltyCardNumber.Refresh();
                PreExecute();
            }
        }

        private void ProcessSwipedCard(ICardInfo cardInfo)
        {
            padLoyaltyCardId_CardSwept(cardInfo.Track2Parts);
        }

        private void numPad_EnterButtonPressed()
        {
            btnOk_Click(this, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (padLoyaltyCardNumber.EnteredValue.Length > 0
                && Execute())
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        #endregion

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type.</typeparam>
        /// <param name="args">Notification object.</param>
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Returns the results of the interaction call.
        /// </summary>
        /// <typeparam name="TResults">Type of result confirmation object.</typeparam>
        /// <returns>Result confirmation object.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new LoyaltyCardConfirmation
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK
            } as TResults;
        }
    }
}
