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
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Controls;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels;
using Microsoft.Win32;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views
{
    /// <summary>
    /// Form for the recorder
    /// </summary>
    public partial class RecorderForm : XtraForm
    {
        #region Symbols

        private const string START_SYMBOL = "\xE1F5";
        private const string STOP_SYMBOL = "\xE15B";
        private const string NEW_SYMBOL = "\xE109";
        private const string DELETE_SYMBOL = "\xE106";
        private const string CLEAR_SYMBOL = "\xE10E";
        private const string SETUP_SYMBOL = "\xE15E";
        private const string HELP_SYMBOL = "\xE11B";

        #endregion

        #region private fields

        private Binding muduleEditValueBinding;
        private Binding moduleEnabledBinding;
        private Binding usageProfileEditValueBinding;
        private Binding usageProfileEnabledBinding;
        private bool areLineChildBindingsInitialized;
        private BindableTreeView<FrameworkLineViewModel> treeControl;
        private Form stopRecordingWaitInfoDialog;

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderForm"/> class.
        /// </summary>
        public RecorderForm()
        {
            MessageFilter messageFilter = new MessageFilter(this);
            this.ViewModel = new RecorderViewModel(messageFilter);

            InitializeComponent();
            this.bindingSource.Add(this.ViewModel);
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public RecorderViewModel ViewModel
        {
            get;
            private set;
        }

        #endregion

        #region private/protected methods
        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                // Post the message, instead of executing directly so that the task recorder form is shown to user before getting busy with initializing data
                WindowsFormsSynchronizationContext.Current.Post(st =>
                {
                    this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                    this.ViewModel.OnError += ViewModel_OnError;

                    this.ViewModel.Initialize();
                    InitializeTreeView();

                    this.FrameworkComboBox.Properties.DataSource = this.ViewModel.FrameworkList;
                    this.IndustryComboBox.Properties.DataSource = this.ViewModel.IndustryList;

                    this.ModuleComboBox.Properties.DataSource = this.ViewModel.LocalizedAvailableModules;
                    this.ModuleComboBox.Refresh();

                    this.UsageProfileComboBox.Properties.DataSource = this.ViewModel.LocalizedUsageProfiles;
                    this.UsageProfileComboBox.Refresh();
                }, null);
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            btnStart.Text = string.Format("{0}{1}{2}{3}", START_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58417)); // Start
            btnStop.Text = string.Format("{0}{1}{2}{3}", STOP_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58418)); // Stop
            btnNew.Text = string.Format("{0}{1}{2}{3}", NEW_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58412)); // New
            btnDelete.Text = string.Format("{0}{1}{2}{3}", DELETE_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58407)); // Delete
            btnClear.Text = string.Format("{0}{1}{2}{3}", CLEAR_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58406)); // Clear
            btnSetup.Text = string.Format("{0}{1}{2}{3}", SETUP_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58416)); // Setup
            btnHelp.Text = string.Format("{0}{1}{2}{3}", HELP_SYMBOL, Environment.NewLine, Environment.NewLine, ApplicationLocalizer.Language.Translate(58413)); // Help        

            this.Text = ApplicationLocalizer.Language.Translate(58405); // Task Recorder
            this.FrameworkLabel.Text = ApplicationLocalizer.Language.Translate(58408); // Framework
            this.IndustryLabel.Text = ApplicationLocalizer.Language.Translate(58409); // Industry
            this.ModuleLabel.Text = ApplicationLocalizer.Language.Translate(58411); // Module
            this.UsageProfileLabel.Text = ApplicationLocalizer.Language.Translate(58419); // Usage profile
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Control will be disposed by parent container when window is disposed.")]
        private void InitializeTreeView()
        {
            treeControl = new BindableTreeView<FrameworkLineViewModel>()
            {
                Dock = DockStyle.Fill,
                DisplayMember = line => line.Name,
                HierarchyMember = per => per.ChildLines,
                AltStyleNodeMember = line => line.HasRecording,
                TopLevelStyleNodeMember = line => line.IsFrameworkIndustryLine,
                ItemSorter = FrameworkLineViewModel.Comparer,
                Sorted = true
            };

            this.TreeViewContainer.Controls.Add(treeControl);

            imageList.Images.Add(BindableTreeViewImagesKey.NormalNodeImage.ToString(), Microsoft.Dynamics.Retail.Pos.TaskRecorder.Properties.Resources.EmptyNode);
            imageList.Images.Add(BindableTreeViewImagesKey.AltStyleNodeImage.ToString(), Microsoft.Dynamics.Retail.Pos.TaskRecorder.Properties.Resources.FilledNode);
            treeControl.ImageList = imageList;

            treeControl.AddBinding("DataSource", this.ViewModel, "FrameworkLines");
            treeControl.AddBinding("SelectedItem", this.ViewModel, "SelectedFrameworkLine");

        }

        private void SetupButton_ItemClick(object sender, EventArgs e)
        {
            using (SetupForm setupForm = new SetupForm(this.ViewModel))
            {
                setupForm.ShowDialog(this);
            }
        }

        private void NewNodeButton_ItemClick(object sender, EventArgs e)
        {
            NewNodeViewModel newNodeViewModel = new NewNodeViewModel(this.ViewModel.SelectedFrameworkLine, this.ViewModel.SelectedFramework.FrameworkId, this.ViewModel.SelectedIndustry.IndustryCode);
            using (NewNodeForm newNodeForm = new NewNodeForm(newNodeViewModel))
            {
                var result = newNodeForm.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.ViewModel.ExecuteCreateNewNode(newNodeViewModel.CreatedLine);
                }
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "AreFrameworksIndustriesEmpty":
                    {
                        NotifyUserForEmptyLists();
                        break;
                    }
                case "SelectedFrameworkLine":
                    {
                        SetLineChildBindings();
                        break;
                    }
                case "RecorderStatus":
                    {
                        ManageWaitDialog();
                        break;
                    }
                default:
                    break;
            }
        }

        private void ManageWaitDialog()
        {
            if (this.ViewModel.RecorderStatus == RecorderStatus.Stopping)
            {
                using (this.stopRecordingWaitInfoDialog = new frmMessage(58456, MessageBoxIcon.Information, true)) // Stopping... Generating documents...
                {
                    this.components.Add(this.stopRecordingWaitInfoDialog);
                    this.stopRecordingWaitInfoTimer.Enabled = true;
                    this.stopRecordingWaitInfoDialog.ShowDialog(this);
                }
            }
            else if (this.ViewModel.RecorderStatus == RecorderStatus.Stopped)
            {
                this.CloseStopRecordingWaitDialog(false);
            }
        }

        private void StopRecordingWaitInfoTimer_Tick(object sender, EventArgs args)
        {
            this.stopRecordingWaitInfoTimer.Stop();
            this.stopRecordingWaitInfoTimer.Enabled = false;
            this.CloseStopRecordingWaitDialog(true);
        }

        private void NotifyUserForEmptyLists()
        {
            if (this.ViewModel.AreFrameworksIndustriesEmpty)
            {
                using (frmMessage message = new frmMessage(58455, MessageBoxIcon.Information, false))
                {
                    message.ShowDialog(this);
                }
            }
        }

        private void SetLineChildBindings()
        {
            if (!this.areLineChildBindingsInitialized && this.ViewModel.SelectedFrameworkLine != null)
            {
                Console.WriteLine("Bindings initialized");
                muduleEditValueBinding = this.ModuleComboBox.AddBinding("EditValue", this.ViewModel, "SelectedFrameworkLine.Module");
                moduleEnabledBinding = this.ModuleComboBox.AddBinding("Enabled", this.ViewModel, "SelectedFrameworkLine.IsBasic");

                usageProfileEditValueBinding = this.UsageProfileComboBox.AddBinding("EditValue", this.ViewModel, "SelectedFrameworkLine.UsageProfile");
                usageProfileEnabledBinding = this.UsageProfileComboBox.AddBinding("Enabled", this.ViewModel, "SelectedFrameworkLine.IsBasic");

                this.areLineChildBindingsInitialized = true;
            }
            else if (this.areLineChildBindingsInitialized && this.ViewModel.SelectedFrameworkLine == null)
            {
                Console.WriteLine("Bindings removed");
                this.ModuleComboBox.DataBindings.Remove(muduleEditValueBinding);
                this.ModuleComboBox.DataBindings.Remove(moduleEnabledBinding);

                this.UsageProfileComboBox.DataBindings.Remove(usageProfileEditValueBinding);
                this.UsageProfileComboBox.DataBindings.Remove(usageProfileEnabledBinding);

                this.areLineChildBindingsInitialized = false;
            }
        }

        private void ViewModel_OnError(object sender, MVVM.ViewModelErrorEventArgs e)
        {
            this.CloseStopRecordingWaitDialog(false, true);
            using (frmError message = new frmError(e.Message, e.Exception))
            {
                message.ShowDialog(this);
            }
        }

        private void StartRecordingButton_ItemClick(object sender, EventArgs e)
        {
            string message = null;
            bool generateWordDocument = true;
            bool proceedWithRecording = true;

            try
            {
                // Verify if word is intalled
                using (var wordClassRegistry = Registry.ClassesRoot.OpenSubKey("Word.Application"))
                {
                    if (wordClassRegistry == null)
                    {
                        message = (58402).Translate(); // Microsoft Word is not installed and so a word document will not be generated at the end of the recording. Do you want to proceed?
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                message = (58401).Translate(); // Unable to verify if Microsoft Word is installed. A word document may not be generated at the end of the recording. Do you want to proceed?
            }

            // if there is an error verifying word installation, then confirm with user if want to proceed.
            if (message != null)
            {
                generateWordDocument = false;
                string title = string.Format((58403).Translate(), // Confirm - {0}
                    (58404).Translate()); // Task Recorder
                var dialogResult = MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                proceedWithRecording = dialogResult == System.Windows.Forms.DialogResult.Yes;
            }

            // Proceed with recording either if no errors or user says continue.
            if (proceedWithRecording)
            {
                this.ViewModel.ExecuteStartRecording(generateWordDocument);
                if (this.ViewModel.RecorderStatus == RecorderStatus.Recording)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            StopRecording();
            base.OnActivated(e);
        }

        private void StopRecording()
        {
            if ((this.ViewModel != null) && (this.ViewModel.RecorderStatus == RecorderStatus.Recording))
            {
                this.WindowState = FormWindowState.Normal;
                this.ViewModel.ExecuteStopRecording();
            }
        }

        private void CloseStopRecordingWaitDialog(bool isFromTimer, bool immediate = false)
        {
            // If it has to close dialog immediately (later to show another dialog, like error, stop timer and close dialog immediately.
            if (immediate)
            {
                this.stopRecordingWaitInfoTimer.Enabled = false;
                ExecuteCloseWaitDialog();
            }
            else
            {
                // If the call is from timer, then stop timer. And if recording has stopped, then the wait dialog has to be closed.
                if (isFromTimer)
                {
                    if (this.ViewModel.RecorderStatus == RecorderStatus.Stopped)
                    {
                        this.ExecuteCloseWaitDialog();
                    }
                }
                else if (!this.stopRecordingWaitInfoTimer.Enabled)
                {
                    // If the timer is not enabled, then it took more time than the minimum wait time, so the dialog can be closed now.
                    this.ExecuteCloseWaitDialog();
                }
            }
        }

        private void ExecuteCloseWaitDialog()
        {
            if (this.stopRecordingWaitInfoDialog != null)
            {
                this.stopRecordingWaitInfoDialog.Close();
                this.stopRecordingWaitInfoDialog = null;
            }
        }

        private void StopRecordingButton_ItemClick(object sender, EventArgs e)
        {
            this.StopRecording();
        }

        private void DeleteNodeButton_ItemClick(object sender, EventArgs e)
        {
            this.ViewModel.ExecuteDeleteNode();
        }

        private void ClearNodeButton_ItemClick(object sender, EventArgs e)
        {
            this.ViewModel.ExecuteClearNode();
        }

        private void HelpButton_ItemClick(object sender, EventArgs e)
        {
            LSRetailPosis.POSControls.POSFormsManager.ShowHelp(this);
        }

        private void RecorderForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            btnHelp.PerformClick();
        }

        #endregion
    }
}
