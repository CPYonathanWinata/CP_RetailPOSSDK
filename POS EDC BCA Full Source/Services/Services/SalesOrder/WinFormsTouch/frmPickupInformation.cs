/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
	sealed partial class formPickupInformation : frmTouchBase
    {
        private const string DATABINDING_VALUEPROPERTY = "Value";
        private const string DATAENTITY_IDPROPERTY     = "Id";

        private formPickupInformation()
        {
            InitializeComponent();
        }

        public formPickupInformation(PickupInformationViewModel viewModel) : this()
        {
            this.bindingSource.Add(viewModel);         
        }
        
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            if (hevent == null)
                throw new ArgumentNullException("hevent");
            else
            {
                LSRetailPosis.POSControls.POSFormsManager.ShowHelp(System.Windows.Forms.Form.ActiveForm);
                hevent.Handled = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                this.dateTimePicker.MinDate = DateTime.Today;                

                // Handle the formatting and parsing for binding to the DateTimePicker so it knows how to handle nulls from a DateTime? value.
                this.dateTimePicker.DataBindings[DATABINDING_VALUEPROPERTY].Format += new ConvertEventHandler(OnDateTimePicker_Format);
                this.dateTimePicker.DataBindings[DATABINDING_VALUEPROPERTY].Parse  += new ConvertEventHandler(OnDateTimePicker_Parse);

                LoadData();
            }
            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            labelTitle.Text    = LSRetailPosis.ApplicationLocalizer.Language.Translate(56383); // Pickup information
            labelDate.Text     = LSRetailPosis.ApplicationLocalizer.Language.Translate(56326); // Requested delivery date:
            colNumber.Caption  = LSRetailPosis.ApplicationLocalizer.Language.Translate(56384); // Number
            colName.Caption    = LSRetailPosis.ApplicationLocalizer.Language.Translate(56388); // Name
            colAddress.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(56389); // Address
            btnSave.Text       = LSRetailPosis.ApplicationLocalizer.Language.Translate(56318); // Save
            btnCancel.Text     = LSRetailPosis.ApplicationLocalizer.Language.Translate(56319); // Cancel
        }

        private void LoadData()
        {          
            gridAddresses.DataSource = this.ViewModel.Stores;

            // Wire up row changed event after setting data source so SelectedStore isn't set prematurely
            gridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(OnGridFocusedRowChanged);

            if (this.ViewModel.SelectedStore == null)
            {
                if (this.ViewModel.Stores.Count > 0)
                    this.ViewModel.SelectedStore = this.ViewModel.Stores[0];                
            }

            gridView.FocusedRowHandle = this.ViewModel.Stores.IndexOf(this.ViewModel.SelectedStore);

            CheckRowPosition();
        }

        [Browsable(false)]
        public PickupInformationViewModel ViewModel
        {
            get
            {
                PickupInformationViewModel viewModel = null;

                if (bindingSource != null && bindingSource.Current != null)
                    viewModel = (PickupInformationViewModel)bindingSource.Current;

                return viewModel;
            }
        }        

        private void OnDateTimePicker_Format(object sender, ConvertEventArgs e)
        {
            // Object => Control
            // e.Value is the object's value, if that is null we want to show the checkbox unchecked
            if (e.Value == null)
            {
                dateTimePicker.ShowCheckBox = true;
                dateTimePicker.Checked = false;
            }
            else
            {
                if ((DateTime)e.Value >= this.dateTimePicker.MinDate)
                {
                    dateTimePicker.ShowCheckBox = true;
                    dateTimePicker.Checked = true;
                }
            }
        }

        private void OnDateTimePicker_Parse(object sender, ConvertEventArgs e)
        {
            // CONTROL => OBJECT
            // e.Value is the formatted value coming from the control, we change it to want we want to set on the object

            if (this.dateTimePicker.Checked)
            {
                e.Value = new Nullable<DateTime>(Convert.ToDateTime(e.Value));
            }
            else
            {
                e.Value = null;
            }
        }        

        private void OnGridFocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle < gridView.DataRowCount)
            {                
                this.ViewModel.SelectedStore = gridView.GetRow(e.FocusedRowHandle) as DataEntity.Store;

                CheckRowPosition();
            }
        }

        private void OnBtnPgUp_Click(object sender, EventArgs e)
        {
            this.gridView.MovePrevPage();

            CheckRowPosition();
        }

        private void OnBtnUp_Click(object sender, EventArgs e)
        {
            this.gridView.MovePrev();

            CheckRowPosition();
        }

        private void OnBtnPgDone_Click(object sender, EventArgs e)
        {
            this.gridView.MoveNextPage();

            CheckRowPosition();
        }

        private void OnBtnDown_Click(object sender, EventArgs e)
        {
            this.gridView.MoveNext();

            CheckRowPosition();
        }

        private void CheckRowPosition()
        {
            btnPgDown.Enabled = !gridView.IsLastRow;
            btnDown.Enabled   = btnPgDown.Enabled;
            btnPgUp.Enabled   = !gridView.IsFirstRow;
            btnUp.Enabled     = btnPgUp.Enabled;
        }

        private void OnBtnSearch_Click(object sender, EventArgs e)
        {
            string filter = textBoxSearch.Text.Trim();

            // If the filter is empty, just reset the data source to all stores
            if (string.IsNullOrEmpty(filter) && gridView.RowCount != this.ViewModel.Stores.Count)
            {
                gridAddresses.DataSource = this.ViewModel.Stores;
            }
            else
            {
                var filteredList = this.ViewModel.Stores.Where(s =>
                    s.Number.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Name.IndexOf(filter,   StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Phone.IndexOf(filter,  StringComparison.OrdinalIgnoreCase) >= 0);

                gridAddresses.DataSource = filteredList.ToList().AsReadOnly();
            }

            // Move focus to the grid to be keyboard friendly
            gridAddresses.Select();

            // Default to first item in grid if exists
            ReadOnlyCollection<DataEntity.Store> dataSource = (ReadOnlyCollection<DataEntity.Store>)gridAddresses.DataSource;
            if (dataSource.Count > 0)
            {
                this.ViewModel.SelectedStore = dataSource[0];
            }
        }

        private void OnBtnClear_Click(object sender, EventArgs e)
        {
            // Clear search filter
            textBoxSearch.Text = string.Empty;
            textBoxSearch.Select();

            // Perform filter
            btnSearch.PerformClick();
        }

        private void OnTextBoxSearch_Enter(object sender, EventArgs e)
        {
            // Make the search button the default button if the user presses Enter
            this.AcceptButton = btnSearch;
        }

        private void OnTextBoxSearch_Leave(object sender, EventArgs e)
        {
            // Reset the default button to save after leaving the search textbox
            this.AcceptButton = btnSave;
        }

        private void OnGridAddresses_DoubleClick(object sender, EventArgs e)
        {
            // Get the selected row handle and use to look up data row
            int selectedRowHandle = gridView.GetSelectedRows()[0];

            if (selectedRowHandle >= 0)
            {
                this.ViewModel.SelectedStore = gridView.GetRow(selectedRowHandle) as DataEntity.Store;

                btnSave.PerformClick();
            }
        }
    }
}