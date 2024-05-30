/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Dialog.ViewModels;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch
{
    /// <summary>
    /// Smart search form class
    /// </summary>
    partial class frmSmartSearch : frmTouchBase
    {
        private SearchViewModel viewModel;
        private int gridVisibleRows;
        private bool itemSearchResultsFromAX = false;

        /// <summary>
        /// Prevents a default instance of the <see cref="frmSmartSearch"/> class from being created.
        /// </summary>
        private frmSmartSearch()
        {
            InitializeComponent();

            // Set control data bindings
            labelResults.DataBindings["Text"].Format            += new ConvertEventHandler(OnSearchTerms_Convert);
            btnTransactions.DataBindings["Enabled"].Format      += new ConvertEventHandler(OnCustomerResultTypeToBool_Convert);
            btnProductDetails.DataBindings["Enabled"].Format    += new ConvertEventHandler(OnItemResultTypeToBool_Convert);
            btnShowPrice.DataBindings["Enabled"].Format         += new ConvertEventHandler(OnItemResultTypeToBool_Convert);
            btnSelect.DataBindings["Enabled"].Format            += new ConvertEventHandler(OnResultToBool_Convert);
            btnPgUp.DataBindings["Enabled"].Format              += new ConvertEventHandler(OnFirstResultToBool_Convert);            
            btnPgDown.DataBindings["Enabled"].Format            += new ConvertEventHandler(OnLastResultToBool_Convert);            
            labelName.DataBindings["Visible"].Format            += new ConvertEventHandler(OnCustomerResultTypeToBool_Convert);
            labelName.DataBindings["Text"].Format               += new ConvertEventHandler(OnCustomerResultName_Convert);
            labelPhone.DataBindings["Visible"].Format           += new ConvertEventHandler(OnCustomerResultTypeToBool_Convert);
            labelPhone.DataBindings["Text"].Format              += new ConvertEventHandler(OnCustomerResultPhone_Convert);
            labelEmail.DataBindings["Visible"].Format           += new ConvertEventHandler(OnCustomerResultTypeToBool_Convert);
            labelEmail.DataBindings["Text"].Format              += new ConvertEventHandler(OnCustomerResultEmail_Convert);
            labelAddress.DataBindings["Visible"].Format         += new ConvertEventHandler(OnCustomerResultTypeToBool_Convert);
            labelAddress.DataBindings["Text"].Format            += new ConvertEventHandler(OnCustomerResultAddress_Convert);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmSmartSearch"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public frmSmartSearch(SearchViewModel viewModel)
            : this()
        {
            this.viewModel = viewModel;

            // Set and bind the view model
            bindingSource.Add(viewModel);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //Set the size of the form the same as the main form
                this.Bounds = new Rectangle(
                    ApplicationSettings.MainWindowLeft,
                    ApplicationSettings.MainWindowTop,
                    ApplicationSettings.MainWindowWidth,
                    ApplicationSettings.MainWindowHeight);

                TranslateLabels();

                SetFilterSelection();

                viewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnViewModel_PropertyChanged);
            }
            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            this.lblHeading.Text        = ApplicationLocalizer.Language.Translate(61517); // "Search"
            this.Text                   = ApplicationLocalizer.Language.Translate(61517); // "Search"
            btnAllResults.Text          = ApplicationLocalizer.Language.Translate(51132); // "All results"
            btnCustomersOnly.Text       = ApplicationLocalizer.Language.Translate(51115); // "Customers only"
            btnItemsOnly.Text           = ApplicationLocalizer.Language.Translate(51116); // "Items only"
            btnTransactions.Text        = ApplicationLocalizer.Language.Translate(51126); // "Customer transactions"
            btnProductDetails.Text      = ApplicationLocalizer.Language.Translate(99800); // "Product details"
            btnSelect.Text              = ApplicationLocalizer.Language.Translate(51105); // "OK"
            btnCancel.Text              = ApplicationLocalizer.Language.Translate(51101); // "Cancel"
            btnShowPrice.Text           = ApplicationLocalizer.Language.Translate(61511); // "Show price"
            colName.Caption             = ApplicationLocalizer.Language.Translate(2731);  // "Name"
            colNumber.Caption           = ApplicationLocalizer.Language.Translate(2730);  // "Number"
            colPrice.Caption            = ApplicationLocalizer.Language.Translate(1488);  // "Price"
            colType.Caption             = ApplicationLocalizer.Language.Translate(51045); // "Type"
            radioCurrentStore.Text      = ApplicationLocalizer.Language.Translate(99888); // "Current store products"
            radioAllProducts.Text       = ApplicationLocalizer.Language.Translate(99891); // "All products"
        }

        #region Methods

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

        /// <summary>
        /// Sets the price visibility.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        private void SetPriceVisibility(bool visible)
        {
            colPrice.Visible = visible;

            // Make price column next to name column.
            if (colPrice.Visible)
                colPrice.VisibleIndex = colName.VisibleIndex + 1;

            // Update button text
            btnShowPrice.Text = ApplicationLocalizer.Language.Translate(colPrice.Visible
                                                                            ? 61512 /* "Hide price" */
                                                                            : 61511 /* "Show price" */);

            viewModel.ItemPriceVisible = visible;
        }

        private void SetFilterSelection()
        {
            btnAllResults.Checked = (viewModel.SearchType == SearchType.All);
            btnItemsOnly.Checked = (viewModel.SearchType == SearchType.Item);
            btnCustomersOnly.Checked = (viewModel.SearchType == SearchType.Customer);
        }

        private void SelectResult()
        {
            if (viewModel.SelectedResult != null)
            {
                // If selected row is of type category then re process it on selection to expaned items.
                if (viewModel.SelectedResult.SearchType == SearchType.Category)
                {
                    viewModel.ExecuteFilterCategory();
                }
                else // It is final selection.
                {
                    if (itemSearchResultsFromAX) // Download the selected product from AX and download
                    {
                        CreateProductData(viewModel.SelectedResult.Number);
                    }
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void Search()
        {
            if (radioAllProducts.Checked)
            {
                itemSearchResultsFromAX = true;
            }
            else
            {
                itemSearchResultsFromAX = false;
            }
            // Get the results
            viewModel.ExecuteSearch();

            // Select the top grid row
            gridView.MoveFirst();

            // Move focus back to the seach TextBox
            textBoxSearch.Select();
        }

        #endregion

        #region Events

        private void OnTimer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            viewModel.ExecuteLoadRowDetail();
        }

        private void OnBtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void OnBtnClear_Click(object sender, EventArgs e)
        {
            // Get the results
            viewModel.ExecuteClear();

            // Move focus back to the seach TextBox
            textBoxSearch.Select();
        }

        private void OnBtnShowPrice_Click(object sender, EventArgs e)
        {
            // Toggle visibility
            SetPriceVisibility(!colPrice.Visible);
        }

        private void OnBtnTransactions_Click(object sender, EventArgs e)
        {
            viewModel.ExecuteCustomerTransactions();
        }

        private void OnBtnProductDetails_Click(object sender, EventArgs e)
        {
            if (itemSearchResultsFromAX) // Download the selected product from AX and download
            {
                CreateProductData(viewModel.SelectedResult.Number);
            }
            
            this.viewModel.ExecuteProductDetails();
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

        private void tabButtons_TextChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckButton button = (DevExpress.XtraEditors.CheckButton)sender;
            Size size = button.CalcBestSize();
            size.Width = Math.Max(size.Width, button.Width);
            size.Height = button.Height;
            button.Size = size;
        }

        private void OnGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            viewModel.ExecuteSelect(e.FocusedRowHandle);

            LoadNextPageIfNeeded();            

            // First stop the timer to clear the last event and then enable it.
            timer.Stop();
            timer.Start();

            // Focus back to search box
            textBoxSearch.Select();
        }

        private void OnFilterButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == btnAllResults)
            {
                tableLayoutSearchProducts.Enabled = true;
                viewModel.ExecuteFilterAll();
            }
            else if (sender == btnItemsOnly)
            {
                tableLayoutSearchProducts.Enabled = true;
                viewModel.ExecuteFilterItemsOnly();
            }
            else if (sender == btnCustomersOnly)
            {
                SetPriceVisibility(false);
                tableLayoutSearchProducts.Enabled = false;
                radioCurrentStore.Select();
                viewModel.ResetSearchAcrossAX();
                viewModel.ExecuteFilterCustomersOnly();

            }
        }

        private void OnTextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:

                    // Pressing enter button with no searh term change will select the current result.
                    if (viewModel.SearchTerms.Equals(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        SelectResult();
                    }
                    else // Else perform search with new term.
                    {
                        viewModel.SearchTerms = textBoxSearch.Text;
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

        void OnViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case SearchViewModel.PROPERTY_SEARCH_TYPE:
                    SetFilterSelection();
                    break;
                case SearchViewModel.PROPERTY_ADD_TO_SALE:
                    SelectResult();
                    break;
                default:
                    break;
            }
        }

        private void OnBtnSelect_Click(object sender, EventArgs e)
        {
            SelectResult();
        }

        private void OnGridResult_DoubleClick(object sender, EventArgs e)
        {
            Point p = gridView.GridControl.PointToClient(MousePosition);
            GridHitInfo info = gridView.CalcHitInfo(p);

            if (info.HitTest != GridHitTest.Column)
            {
                SelectResult();
            }
        }

        #endregion

        #region Binding Converters

        private void OnSearchTerms_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                string val = (string)e.Value;

                // Format the string is search terms exist
                if (string.IsNullOrWhiteSpace(val))
                {
                    e.Value = string.Empty;
                }
                else if (val.Length < viewModel.MinimumSearchTermLengh)
                {
                    e.Value = ApplicationLocalizer.Language.Translate(61519); // Search requires a minimum of two characters.
                }
                else if (viewModel.SelectedResult == null)
                {
                    e.Value = ApplicationLocalizer.Language.Translate(61518); // No search results were found.
                }
                else
                {
                    e.Value = ApplicationLocalizer.Language.Translate(51122, val); // "Search results for "{0}""
                }
            }
        }

        private void OnCustomerResultTypeToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = viewModel.SelectedResult != null 
                    && viewModel.SelectedResult.SearchType == SearchType.Customer
                    && !string.IsNullOrWhiteSpace(viewModel.SelectedResult.Number);
            }
        }

        private void OnItemResultTypeToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = viewModel.SelectedResult != null && viewModel.SelectedResult.SearchType == SearchType.Item;
            }
        }

        private void OnResultToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = viewModel.SelectedResult != null;
            }
        }

        private void OnFirstResultToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = viewModel.SelectedResult != null && !gridView.IsFirstRow;
            }
        }

        private void OnLastResultToBool_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = viewModel.SelectedResult != null && !gridView.IsLastRow;
            }
        }

        private void OnCustomerResultName_Convert(object sender, ConvertEventArgs e)
        {
            string name = null;

            if ((viewModel.SelectedResult != null) && (viewModel.SelectedResult.SearchType == SearchType.Customer))
            {
                var selectedCustomer = viewModel.SelectedResult.GetCustomer();

                if (selectedCustomer != null)
                {
                    name = selectedCustomer.Name;
                }
            }

            e.Value = name;
        }

        private void OnCustomerResultPhone_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                string phone = null;

                if (viewModel.SelectedResult != null && viewModel.SelectedResult.SearchType == SearchType.Customer)
                {
                    var selectedCustomer = viewModel.SelectedResult.GetCustomer();

                    if (selectedCustomer != null)
                        phone = selectedCustomer.Telephone;
                }

                e.Value = phone;
            }
        }

        private void OnCustomerResultEmail_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                string email = null;

                if (viewModel.SelectedResult != null && viewModel.SelectedResult.SearchType == SearchType.Customer)
                {
                    var selectedCustomer = viewModel.SelectedResult.GetCustomer();

                    if (selectedCustomer != null)
                        email = selectedCustomer.Email;
                }

                e.Value = email;
            }
        }

        private void OnCustomerResultAddress_Convert(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                string address = null;

                if (viewModel.SelectedResult != null && viewModel.SelectedResult.SearchType == SearchType.Customer)
                {
                    var selectedCustomer = viewModel.SelectedResult.GetCustomer();

                    if (selectedCustomer != null)
                        address = selectedCustomer.Address.Replace("\n", " ");
                }

                e.Value = address;
            }
        }

        #endregion

        private void radioAllProducts_CheckedChanged(object sender, EventArgs e)
        {
                viewModel.SetSearchAcrossAX(true);
                textBoxSearch.Select();
        }

        private void radioCurrentStore_CheckedChanged(object sender, EventArgs e)
        {
            viewModel.SetSearchAcrossAX(false);
            textBoxSearch.Select();
        }

        // Saves the product data in POS tables for the item selected
        private void CreateProductData(string selectedItem)
        {
            NetTracer.Information("frmItemSearch::CreateProductData Started with item selected: {0}", selectedItem);
            bool createdLocal = false;
            XDocument tmpstring;

            try
            {

                //Retrieve the Product
                tmpstring = Dialog.InternalApplication.Services.Item.GetProductData(selectedItem);

                DM.ItemDataManager itemdataManager = new DM.ItemDataManager(
                    PosApplication.Instance.Settings.Database.Connection,
                    PosApplication.Instance.Settings.Database.DataAreaID);

                ItemData itemData = new ItemData(
                ApplicationSettings.Database.LocalConnection,
                ApplicationSettings.Database.DATAAREAID,
                ApplicationSettings.Terminal.StorePrimaryId);

                createdLocal = itemData.SaveProductData(tmpstring);

                if (!createdLocal)
                {
                    Dialog.InternalApplication.Services.Dialog.ShowMessage(99890, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                Dialog.InternalApplication.Services.Dialog.ShowMessage(99890, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NetTracer.Information("frmItemSearch::CreateProductData completed with Search String: {0}", selectedItem);
        }
    }
}
