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
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{

    /// <summary>
    /// Extended logon assignment UI
    /// </summary>
    [Export("ExtendedLogOnForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ExtendedLogOnForm : frmTouchBase, IInteractionView
    {

        #region Fields

        private int gridVisibleRows;
        private readonly ExtendedLogOnViewModel viewModel;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLogOnForm"/> class.
        /// </summary>
        public ExtendedLogOnForm()
        {
            InitializeComponent();

            btnAssign.DataBindings["Enabled"].Format += new ConvertEventHandler(OnResultTypeToBool_Convert);
            btnUnassign.DataBindings["Enabled"].Format += new ConvertEventHandler(OnResultTypeToBool_Convert);

            viewModel = new ExtendedLogOnViewModel();

            // Set and bind the view model
            bindingSource.Add(viewModel);
        }

        /// <summary>
        /// Overloads <see cref="E:Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.TranslateLabels();
            }

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Search();
        }

        #endregion

        #region Private Methods

        private void TranslateLabels()
        {
            //
            // TextID's for frmIncomeExpenseAccounts are reserved at 99400 - 99924
            //

            this.Text = ApplicationLocalizer.Language.Translate(99400);
            this.lblHeader.Text = ApplicationLocalizer.Language.Translate(99400);
            this.colOperatorID.Caption = ApplicationLocalizer.Language.Translate(99402);
            this.colName.Caption = ApplicationLocalizer.Language.Translate(99403);
            this.btnAssign.Text = ApplicationLocalizer.Language.Translate(99404);
            this.btnClose.Text = ApplicationLocalizer.Language.Translate(99405);
            this.btnUnassign.Text = ApplicationLocalizer.Language.Translate(99406);
        }

        private void LoadNextPageIfNeeded()
        {
            // If we are less than one page from the end of the currently loaded rows, load the next page
            int currentRowIndex = gridView.GetDataSourceRowIndex(gridView.FocusedRowHandle);

            // Get the number of visible rows to pre-fetch the data pages.
            if (gridVisibleRows == 0)
            {
                gridVisibleRows = (gridView.ViewRect.Height / gridView.RowHeight);
            }

            if (currentRowIndex + gridVisibleRows > gridView.DataRowCount - 1)
            {
                viewModel.ExecuteNextPage();
            }
        }

        private void AssignOperator()
        {
            using (ExtendedLogOnScanForm scanForm = new ExtendedLogOnScanForm())
            {
                POSFormsManager.ShowPOSForm(scanForm);

                if (scanForm.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        viewModel.ExecuteAssign(scanForm.ExtendedLogOnInfo);
                    }
                    catch (PosisException pex)
                    {
                        NetTracer.Error(pex, "ExtendedLogOnForm::AssignOperator: Failed.");

                        POSFormsManager.ShowPOSMessageDialog(pex.ErrorMessageNumber);
                    }
                    catch (Exception ex)
                    {
                        NetTracer.Error(ex, "ExtendedLogOnForm::AssignOperator: Failed.");

                        POSFormsManager.ShowPOSMessageDialog(99412); // Generic failure.
                    }
                }
            }
        }

        /// <summary>
        /// Unassign extend logon for the operator.
        /// </summary>
        private void UnassignOperator()
        {
            DialogResult dialogResult = PosApplication.Instance.Services.Dialog.ShowMessage(ApplicationLocalizer.Language.Translate(99407, viewModel.SelectedResult.OperatorName),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    viewModel.ExecuteUnassign();
                }
                catch (Exception ex)
                {
                    NetTracer.Error(ex, "RevokeExtendedLogon::RevokeOperator: Failed.");

                    POSFormsManager.ShowPOSMessageDialog(99412); // Generic failure.
                }
            }
        }

        private void Search()
        {
            // Get the results
            viewModel.ExecuteSearch();

            // Select the top grid row
            gridView.MoveFirst();

            // Move focus back to the seach TextBox
            txtSearchFilter.Select();

            UpdateNavigationButtons();
        }

        #endregion

        #region Event Handlers

        private void OnBtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void OnBtnClear_Click(object sender, EventArgs e)
        {
            // Get the results
            viewModel.ExecuteClear();

            // Move focus back to the seach TextBox
            txtSearchFilter.Select();
        }

        private void OnBtnPgUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrevPage();
        }

        private void OnBtnUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrev();
        }

        private void OnBtnPgDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNextPage();
        }

        private void OnBtnDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNext();
        }

        private void UpdateNavigationButtons()
        {
            btnDown.Enabled = btnPgDown.Enabled = !gridView.IsLastRow;
            btnUp.Enabled = btnPgUp.Enabled = !gridView.IsFirstRow;
        }

        private void OnResultTypeToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = viewModel.SelectedResult != null;
            }
        }

        private void OnGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            viewModel.ExecuteSelect(e.FocusedRowHandle);

            LoadNextPageIfNeeded();

            UpdateNavigationButtons();

            // Focus back to search box
            txtSearchFilter.Select();
        }

        private void OntxtSearchFilter_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:

                    // Pressing enter button with no searh term change will select the current result.
                    if (txtSearchFilter.Text.Equals(viewModel.SearchFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        AssignOperator();
                    }
                    else // Else perform search with new term.
                    {
                        viewModel.SearchFilter = txtSearchFilter.Text;
                        Search();
                    }

                    e.Handled = true;
                    break;
                case Keys.Up:
                    gridView.MovePrev();
                    e.Handled = true;
                    break;
                case Keys.Down:
                    gridView.MoveNext();
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    gridView.MovePrevPage();
                    e.Handled = true;
                    break;
                case Keys.PageDown:
                    gridView.MoveNextPage();
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void OnBtnAssign_Click(object sender, EventArgs e)
        {
            AssignOperator();
        }

        private void OnGridOperators_DoubleClick(object sender, EventArgs e)
        {
            Point p = gridView.GridControl.PointToClient(MousePosition);
            GridHitInfo info = gridView.CalcHitInfo(p);

            if (info.HitTest != GridHitTest.Column)
            {
                AssignOperator();
            }
        }

        private void OnBtnUnassign_Click(object sender, EventArgs e)
        {
            UnassignOperator();
        }

        #endregion

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
            return new ExtendedLogOnConfirmation
            {
                Confirmed = this.DialogResult == DialogResult.OK,
            } as TResults;
        }

        #endregion

    }
}
