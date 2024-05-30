/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel.Composition;
using System.Drawing;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    [Export("SaleRefundDetailsForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SaleRefundDetailForm : frmTouchBase, IInteractionView
	{
        private readonly IRetailTransaction retailTransaction;
        private readonly SaleRefundDetailsViewModel viewModel;

		/// <summary>
		/// Display refund amounts form.
		/// </summary>
		private SaleRefundDetailForm()
		{
			InitializeComponent();
		}

        [ImportingConstructor]
        public SaleRefundDetailForm(SaleRefundDisplayNotification notification)
            : this()
        {
            if (notification == null)
                throw new ArgumentNullException("notification");

            retailTransaction = (IRetailTransaction)notification.RetailTransaction;
            if (retailTransaction == null)
                throw new ArgumentException("Notification does not contain return transaction", "notification");

            viewModel = new SaleRefundDetailsViewModel();
        }

		protected override void OnLoad(EventArgs e)
		{
			if (!this.DesignMode)
			{
				this.Bounds = new Rectangle(
					ApplicationSettings.MainWindowLeft,
					ApplicationSettings.MainWindowTop,
					ApplicationSettings.MainWindowWidth,
					ApplicationSettings.MainWindowHeight
					);

				TranslateLabels();

                viewModel.RetrievePaymentRefunds(retailTransaction);
                saleRefundDetailsViewModelBindingSource.Add(viewModel);
			}

			base.OnLoad(e);
		}

		private void TranslateLabels()
		{
			// Translate the buttons...
            this.Text = ApplicationLocalizer.Language.Translate(TitleLabelId);
            btnClose.Text = ApplicationLocalizer.Language.Translate(CloseLabelId);
            colTenderType.Caption = ApplicationLocalizer.Language.Translate(TenderTypeLabelId);
            colOriginalAmount.Caption = ApplicationLocalizer.Language.Translate(OriginalAmountLabelId);
            colReturnedAmount.Caption = ApplicationLocalizer.Language.Translate(ReturnedAmountLabelId);
            colReturnedEarlierAmount.Caption = ApplicationLocalizer.Language.Translate(ReturnedEarlierAmountLabelId);
            colAvailableAmount.Caption = ApplicationLocalizer.Language.Translate(AvailableAmountLabelId);
            lblHeading.Text = ApplicationLocalizer.Language.Translate(TitleLabelId);
		}

        #region IInteractionView implementation
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
            return new SaleRefundDisplayNotification() as TResults;
        }

        #endregion

        #region Private members
        private const int TitleLabelId = 106014; // Display refund amounts
        private const int CloseLabelId = 2738; // Cancel
        private const int TenderTypeLabelId = 106024; // Payment method
        private const int OriginalAmountLabelId = 106016; // Paid amount
        private const int ReturnedAmountLabelId = 106021; // Returned amount
        private const int ReturnedEarlierAmountLabelId = 106017; // Amount returned earlier
        private const int AvailableAmountLabelId = 106018; // Available amount
        #endregion
    }
}