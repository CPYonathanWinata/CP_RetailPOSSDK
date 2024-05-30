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
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export("DisbursementSlipCreationForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DisbursementSlipCreationForm : frmTouchBase, IInteractionView
    {

        #region Members

        private DisbursementSlipCreationViewModel viewModel;

        #endregion

        #region Construction

        /// <summary>
        /// Default ctor.
        /// </summary>
        public DisbursementSlipCreationForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                viewModel = new DisbursementSlipCreationViewModel();

                bindingSource.Add(viewModel);

                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                TranslateAndSetupControls();
            }

            base.OnLoad(e);
        }

        #endregion

        #region Private methods

        private void TranslateAndSetupControls()
        {
            this.Text = lblHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106029);
            lblPersonName.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106039);
            lblCustomerIdentityCard.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106030);
            lblReasonOfReturn.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106031);
            lblSuplpement.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106032);
            lblDocumentNum.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106033);
            lblDocumentDate.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(106034);

            this.btnOk.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(55403);
            this.btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(55404);
            this.btnClear.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1280);
        }

        #endregion
        
        #region Event handlers

        private void numPad_EnterButtonPressed()
        {
            btnOk_Click(this, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (viewModel.Validate())
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            viewModel.Clear();
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
            return new DisbursementSlipCreationConfirmation
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK,
                DisbursementSlipInfo = viewModel
            } as TResults;
        }
    }
}
