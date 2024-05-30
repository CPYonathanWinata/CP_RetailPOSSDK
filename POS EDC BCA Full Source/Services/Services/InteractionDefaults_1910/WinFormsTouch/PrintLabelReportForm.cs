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
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export("PrintLabelReportForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PrintLabelReportForm : frmTouchBase, IInteractionView
    {
        #region Private fields

        private PrintLabelReportViewModel viewModel;

        #endregion

        #region Construction

        /// <summary>
        /// Default ctor.
        /// </summary>
        protected PrintLabelReportForm()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public PrintLabelReportForm(PrintLabelReportNotification printLabelReportNotification)
            : this()
        {
            if (printLabelReportNotification == null)
            {
                throw new ArgumentNullException("printLabelReportNotification");
            }

            viewModel = new PrintLabelReportViewModel();
            this.viewModel.ReportDate = DateTime.Today;
            this.viewModel.LabelType = (RetailLabelTypeBase)printLabelReportNotification.LabelType;
            this.viewModel.processingDialog = new frmMessage(100029, MessageBoxButtons.OK, MessageBoxIcon.Information); // Click OK to run another operation before the completion of printing.
            this.viewModel.processingDialog.FormClosed += processingDialog_FormClosed;
            this.bindingSource.Add(this.viewModel);

            this.SetNumPadEntryType(Contracts.UI.NumpadEntryTypes.Barcode);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //
                // Get all text through the Translation function in the ApplicationLocalizer
                // TextID's for PrintLabelRreport are reserved at 100000-100099
                //
                TranslateAndSetupControls();
                this.btnSetupPrinter.Visible = viewModel.IsUserAllowedToSetupLabelPrinter();
                viewModel.CheckPrintLabelReportTaskResult();
            }

            base.OnLoad(e);
        }

        private void TranslateAndSetupControls()
        {
            btnPrint.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1181); // Print
            btnSetupPrinter.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(100035); // Set up printer
            btnSearch.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1262); // Search 
            btnClose.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1221); // Close
            btnRemove.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(103105); // Remove 
            btnEnterQuantity.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(103106); // Enter quantity 
            if (this.viewModel.LabelType == RetailLabelTypeBase.ShelfLabel)
            {
                labelHeading.Text = ApplicationLocalizer.Language.Translate(100000); // Print shelf label
            }
            else if (this.viewModel.LabelType == RetailLabelTypeBase.ItemLabel)
            {
                labelHeading.Text = ApplicationLocalizer.Language.Translate(100001); // Print item label
            }
            this.colItemId.Caption = ApplicationLocalizer.Language.Translate(103101); // Item number
            this.colItemName.Caption = ApplicationLocalizer.Language.Translate(103102); // Description
            this.colQuantity.Caption = ApplicationLocalizer.Language.Translate(103103); // Quantity
            this.colStatus.Caption = ApplicationLocalizer.Language.Translate(103122); // Status
        }

        #endregion

        #region Event handlers

        private void OnPrint_Click(object sender, EventArgs e)
        {
            this.viewModel.PrintLabelReport();
        }

        private void OnSetupPrinter_Click(object sender, EventArgs e)
        {
            this.viewModel.SetupPrinterSettings();
        }

        private void processingDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.viewModel.processingDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

        private void OnNumPadEnter_Pressed()
        {
            if (this.numPad1.EntryType == Contracts.UI.NumpadEntryTypes.Barcode)
            {
                string enteredValue = numPad1.EnteredValue;
                numPad1.ClearValue();
                this.viewModel.AddOrUpdateItem(enteredValue);
            }
            else if (this.numPad1.EntryType == Contracts.UI.NumpadEntryTypes.IntegerPositive)
            {
                int quantity = 0;
                if (!Int32.TryParse(numPad1.EnteredValue, out quantity))
                {
                    return;
                }
                numPad1.ClearValue();
                var selectedItem = grdView.GetFocusedRow() as LabelReportItem;
                this.viewModel.EnterItemQuantity(selectedItem, quantity);

                this.SetNumPadEntryType(Contracts.UI.NumpadEntryTypes.Barcode);
            }
        }

        private void OnSearch_Click(object sender, EventArgs e)
        {
            this.viewModel.OpenItemSearchDialog();
        }

        private void OnRemove_Click(object sender, EventArgs e)
        {
            var selectedItem = grdView.GetFocusedRow() as LabelReportItem;
            this.viewModel.RemoveItem(selectedItem);
        }

        private void OnEnterQuantity_Click(object sender, EventArgs e)
        {
            this.SetNumPadEntryType(Contracts.UI.NumpadEntryTypes.IntegerPositive);
        }

        private void OnPageUp_Click(object sender, EventArgs e)
        {
            grdView.MovePrevPage();
        }

        private void OnUp_Click(object sender, EventArgs e)
        {
            grdView.MovePrev();
        }

        private void OnPageDown_Click(object sender, EventArgs e)
        {
            grdView.MoveNextPage();
        }

        private void OnDown_Click(object sender, EventArgs e)
        {
            grdView.MoveNext();
        }

        #endregion

        #region Private methods

        private void SetNumPadEntryType(Contracts.UI.NumpadEntryTypes entryType)
        {
            if (entryType == Contracts.UI.NumpadEntryTypes.Barcode)
            {
                PosApplication.Instance.Services.Peripherals.Scanner.ReEnableForScan();
                numPad1.EntryType = Contracts.UI.NumpadEntryTypes.Barcode;
                numPad1.PromptText = ApplicationLocalizer.Language.Translate(103110);               // Scan or enter barcode
                numPad1.ClearValue();
            }
            else
            {
                PosApplication.Instance.Services.Peripherals.Scanner.DisableForScan();
                numPad1.EntryType = Contracts.UI.NumpadEntryTypes.IntegerPositive;
                numPad1.PromptText = ApplicationLocalizer.Language.Translate(103111);               // Enter quantity
                numPad1.ClearValue();
            }
        }

        #endregion

        #region IInteractionView

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
        /// <typeparam name="TResults">Type of result notification object.</typeparam>
        /// <returns>Result notification object.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new PrintLabelReportNotification
            {
            } as TResults;
        }

        #endregion
    }
}
