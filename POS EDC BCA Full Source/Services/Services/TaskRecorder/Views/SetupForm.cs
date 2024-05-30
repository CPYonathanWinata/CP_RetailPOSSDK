/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using DevExpress.XtraEditors;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views
{
    /// <summary>
    /// Form to accept initial parameters
    /// </summary>
    public partial class SetupForm : XtraForm
    {
        #region property backend fields

        private RecorderViewModel viewModel;

        #endregion

        #region private fields

        private string initialRecordingFolder;
        private string initialTemplatePath;

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupForm"/> class.
        /// </summary>
        protected SetupForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupForm"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public SetupForm(RecorderViewModel viewModel)
            : this()
        {
            this.ViewModel = viewModel;

            if (this.ViewModel != null)
            {
                this.initialRecordingFolder = this.ViewModel.PosRecorderFolderPath;
                this.initialTemplatePath = this.ViewModel.PosTemplateFilePath;
                this.TemplateFileDialog.Filter = this.ViewModel.TemplateFilter;
            }

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
        public RecorderViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
            private set
            {
                viewModel = value;
            }
        }

        #endregion

        #region private/protected methods

        private void RecordingFolderBrowseButton_Click(object sender, EventArgs e)
        {
            if (this.ViewModel != null)
            {
                var result = this.RecorderFolderDialog.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.ViewModel.PosRecorderFolderPath = this.RecorderFolderDialog.SelectedPath;
                }
            }
        }

        private void TemplateFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (this.ViewModel != null)
            {
                var result = this.TemplateFileDialog.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.ViewModel.PosTemplateFilePath = this.TemplateFileDialog.FileName;
                }
            }
        }

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
            this.RecordingParametersHeader.Text = ApplicationLocalizer.Language.Translate(58600); // Recorder prameters
            this.DocumentDirectoryHeader.Text = ApplicationLocalizer.Language.Translate(58601); // Document directory
            this.RecordingFilePathLabel.Text = ApplicationLocalizer.Language.Translate(58602); // Recording file path
            this.TemplateFilePathLabel.Text = ApplicationLocalizer.Language.Translate(58603); // Template file path
            this.SaveNodeButton.Text = ApplicationLocalizer.Language.Translate(58604); // OK
            this.CancelNodeButton.Text = ApplicationLocalizer.Language.Translate(58448); // Cancel
            this.RecordingFolderBrowseButton.Text = ApplicationLocalizer.Language.Translate(58605); // Browse
        }

        private void SaveNodeButton_Click(object sender, EventArgs e)
        {
            this.ViewModel.ExecuteSetParameters();
        }

        private void CancelNodeButton_Click(object sender, EventArgs e)
        {
            this.ViewModel.PosRecorderFolderPath = this.initialRecordingFolder;
            this.ViewModel.PosTemplateFilePath = this.initialTemplatePath;
        }

        #endregion

    }
}
