/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Affiliation form class
    /// </summary>
    [Export("AffiliationForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AffiliationForm : frmTouchBase, IInteractionView
    {
        private const string COLSELECT = "IsSelected";

        /// <summary>
        /// Gets or sets the confirmation object.
        /// </summary>
        private AffiliationConfirmation ConfirmationResult;
        private AffiliationViewModel viewModel;

        private AffiliationForm()
        {
            InitializeComponent();

            this.btnOK.Enabled = false;
            this.ConfirmationResult = new AffiliationConfirmation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliationForm"/> class.
        /// </summary>
        /// <param name="affiliationConfirmation">Affiliation confirmation.</param>
        [ImportingConstructor]
        public AffiliationForm(AffiliationConfirmation affiliationConfirmation)
            : this()
        {
            if (affiliationConfirmation == null)
            {
                throw new ArgumentNullException("affiliationConfirmation");
            }

            this.Initialize(affiliationConfirmation);
        }

        private void TranslateLabels()
        {
            btnOK.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1201); // OK
            btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51114); // Cancel
            

            lblHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(103372); // Affiliations

            colCheckEdit.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(61503); // Select
            colName.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(103373); // Name
            colDescription.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(103374); // Description
        }

        private void Initialize(AffiliationConfirmation affiliationConfirmation)
        {
            this.ConfirmationResult = new AffiliationConfirmation();
            this.viewModel = new AffiliationViewModel(affiliationConfirmation.SelectedAffiliations);

            this.bindingSource.Add(this.viewModel);
            this.grAffiliation.DataBindings.Add("DataSource", this.bindingSource, "DisplayAffiliations", true, DataSourceUpdateMode.OnPropertyChanged);

            this.lblHeadDescription.Text = (affiliationConfirmation.FormType == FormType.Customer ?
                LSRetailPosis.ApplicationLocalizer.Language.Translate(103375) : // Customer
                LSRetailPosis.ApplicationLocalizer.Language.Translate(103382)); // Apply
        }

        protected override void OnLoad(System.EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();   // handle localization texts
            }

            base.OnLoad(e);
        }

        private void btnSelect_Click(object sender, System.EventArgs e)
        {
            this.SelectAffiliation();
        }

        private void btnPgUp_Click(object sender, System.EventArgs e)
        {
            gvAffiliation.MovePrevPage();
            CheckRowPosition();
        }

        private void btnUp_Click(object sender, System.EventArgs e)
        {
            gvAffiliation.MovePrev();
            CheckRowPosition();
        }

        private void btnPgDown_Click(object sender, System.EventArgs e)
        {
            gvAffiliation.MoveNextPage();
            CheckRowPosition();
        }

        private void btnDown_Click(object sender, System.EventArgs e)
        {
            gvAffiliation.MoveNext();
            CheckRowPosition();
        }

        private void grItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                btnSelect_Click(null, null);
            }
            CheckRowPosition();
        }

        private void colCheckEdit_ValueChange(object sender, System.EventArgs e)
        {
            this.btnOK.Enabled = true;
        }

        private void CheckRowPosition()
        {
            btnPgDown.Enabled = !gvAffiliation.IsLastRow;
            btnDown.Enabled = btnPgDown.Enabled;
            btnPgUp.Enabled = !gvAffiliation.IsFirstRow;
            btnUp.Enabled = btnPgUp.Enabled;
        }

        private void SelectAffiliation()
        {
            Collection<DE.IAffiliation> selectedAffiliations = this.GetSelectedAffiliations();

            this.ConfirmationResult.AffiliationResults = new Collection<object>(selectedAffiliations.Cast<object>().ToList());

            this.DialogResult = DialogResult.OK;
        }

        private Collection<DE.IAffiliation> GetSelectedAffiliations()
        {
            Collection<DE.IAffiliation> selectedAffiliations = new Collection<DE.IAffiliation>();

            foreach (var item in this.viewModel.DisplayAffiliations)
            {
                if (item.IsSelected)
                {
                    selectedAffiliations.Add(item.Affiliation);
                }
            }

            return selectedAffiliations;
        }

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type</typeparam>
        /// <param name="args">Notification</param>
        public void Initialize<TArgs>(TArgs args)
             where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call
        /// </summary>
        /// <typeparam name="TResults"></typeparam>
        /// <returns>Returns the TResults object</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            this.ConfirmationResult.Confirmed = (this.DialogResult == DialogResult.OK);
            return this.ConfirmationResult as TResults;
        }

        #endregion
    }
}
