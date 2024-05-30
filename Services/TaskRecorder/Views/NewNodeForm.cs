/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views
{
    /// <summary>
    /// Form to create new node/frameworkline
    /// </summary>
    public partial class NewNodeForm : XtraForm
    {
        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="NewNodeForm"/> class.
        /// </summary>
        protected NewNodeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewNodeForm"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public NewNodeForm(NewNodeViewModel viewModel)
            : this()
        {
            this.ViewModel = viewModel;
            this.bindingSource.Add(this.ViewModel);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public NewNodeViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
            private set
            {
                viewModel = value;
                this.Initialize();
            }
        }private NewNodeViewModel viewModel;
        #endregion

        #region private/protected methods

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            this.lblNewNodeHeader.Text      = ApplicationLocalizer.Language.Translate(58441); // New node
            this.NodeNameLabel.Text         = ApplicationLocalizer.Language.Translate(58443); // Node name
            this.NodeDescriptionLabel.Text  = ApplicationLocalizer.Language.Translate(58444); // Node description
            this.ModuleLabel.Text           = ApplicationLocalizer.Language.Translate(58445); // Module
            this.OperationGroupLabel.Text   = ApplicationLocalizer.Language.Translate(58446); // Operation group
            this.SaveNodeButton.Text        = ApplicationLocalizer.Language.Translate(58447); // Save 
            this.CancelNodeButton.Text      = ApplicationLocalizer.Language.Translate(58448); // Cancel
            this.Text                       = ApplicationLocalizer.Language.Translate(58442); // New node - Task Recorder
        }

        private void Initialize()
        {
            if (this.ViewModel != null)
            {
                this.ModuleComboBox.Properties.DataSource = this.ViewModel.LocalizedAvailableModules;                
                this.ModuleComboBox.Refresh();

                this.OperationGroupComboBox.Properties.DataSource = this.ViewModel.LocalizedAvailableOperationGroups;               
                this.OperationGroupComboBox.Refresh();
            }
        }

        private void SaveNodeButton_Click(object sender, EventArgs e)
        {
            this.ViewModel.ExecuteSaveNode();
        }

        #endregion       
    }
}
