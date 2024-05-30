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
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export("RedeemLoyaltyPointsForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RedeemLoyaltyPointsForm : frmTouchBase, IInteractionView
    {
        #region Members

        /// <summary>
        /// Gets or sets the view model of this form
        /// </summary>
        private RedeemLoyaltyPointsViewModel ViewModel { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default ctor.
        /// </summary>
        protected RedeemLoyaltyPointsForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        [ImportingConstructor]
        public RedeemLoyaltyPointsForm(RedeemLoyaltyPointsConfirmation redeemLoyaltyPointsConfirmation)
            : this()
        {
            if (redeemLoyaltyPointsConfirmation == null)
            {
                throw new ArgumentNullException("redeemLoyaltyPointsConfirmation");
            }
            ViewModel = new RedeemLoyaltyPointsViewModel(redeemLoyaltyPointsConfirmation.CardNumber, redeemLoyaltyPointsConfirmation.AmountAvailableForDiscount);
            padLoyaltyDiscountAmount.NegativeMode = redeemLoyaltyPointsConfirmation.AmountAvailableForDiscount < decimal.Zero;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                bindingSource.Add(ViewModel);

                //
                // Get all text through the Translation function in the ApplicationLocalizer
                // TextID's for RedeemLoyaltyPoints are reserved at 55600-55649
                //
                TranslateAndSetupControls();
            }

            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            RedeemLoyaltyPointsViewModel.DisablePosDevices();
        }

        private void TranslateAndSetupControls()
        {
            this.Text = lblHeading.Text = ApplicationLocalizer.Language.Translate(55600); //Redeem loyalty points
            padLoyaltyCardNumber.PromptText = ApplicationLocalizer.Language.Translate(103235); //Loyalty card number
            lblLoyaltyCardBalanceTitle.Text = ApplicationLocalizer.Language.Translate(55601); //Balance points
            lblMaxLoyaltyDiscountAmountTitle.Text = ApplicationLocalizer.Language.Translate(55602); //Available loyalty points discount amount
            lblAmountAvailableForDiscountTitle.Text = ApplicationLocalizer.Language.Translate(55612); //Maximum discount amount for the receipt
            padLoyaltyDiscountAmount.PromptText = ApplicationLocalizer.Language.Translate(55603); //Discount amount
            btnOk.Text = ApplicationLocalizer.Language.Translate(1240); //OK
            btnCancel.Text = ApplicationLocalizer.Language.Translate(1241); //Cancel
        }

        #endregion

        #region Event handlers

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (padLoyaltyCardNumber.EnteredValue.Length > 0)
            {
                ViewModel.RetrieveLoyaltyCardBalance();
            }
        }

        private void padLoyaltyCardNumber_EnterButtonPressed()
        {
            if (padLoyaltyCardNumber.EnteredValue.Length > 0)
            {
                btnGet.Select();
                ViewModel.RetrieveLoyaltyCardBalance();
            }
        }

        private void padLoyaltyDiscountAmount_EnterButtonPressed()
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void padLoyaltyCardNumber_CardSwept(ICardInfo cardInfo)
        {
            this.ViewModel.ProcessSwipedCard(cardInfo);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                decimal amount = 0;
                if (ViewModel.RetrieveLoyaltyCardBalance()
                    && (string.IsNullOrWhiteSpace(padLoyaltyDiscountAmount.EnteredValue) || padLoyaltyDiscountAmount.EnteredValue.Equals("-") 
                    || Decimal.TryParse(padLoyaltyDiscountAmount.EnteredValue, out amount)))
                {
                    ViewModel.DiscountAmount = amount;
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }

            base.OnClosing(e);
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
            return new RedeemLoyaltyPointsConfirmation
            {
                DiscountAmount = ViewModel.DiscountAmount,
                CardNumber = ViewModel.CardNumber,
                Confirmed = this.DialogResult == DialogResult.OK
            } as TResults;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}