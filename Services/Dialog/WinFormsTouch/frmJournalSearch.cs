/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch
{
    using System;
    using System.Windows.Forms;

    using Contracts.DataEntity;
    using Contracts.Services;
    using DataEntity;
    using LSRetailPosis.POSProcesses;

    public partial class frmJournalSearch : frmTouchBase, ISalesOrderSearchCriteriaHolder
    {
        private const int InitialLoadedRows = 500;
        
        /// <summary>
        /// Search for the last 30 days by default 
        /// </summary>
        private const int DefaultJournalSearchInterval = 30;

        /// <summary>
        /// Gets or sets the search criteria to be used.
        /// </summary>
        public SalesOrderSearchCriteria SearchCriteria { get; set; }

        /// <summary>
        /// Gives all details from database for search.
        /// </summary>
        public frmJournalSearch()
        {

            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmJournalSearch are reserved at 2450 - 2499 
            // The last id in use is: 2469
            //

            InitializeComponent();
            this.dtpFromDate.Value = System.DateTime.Today.Subtract(TimeSpan.FromDays(DefaultJournalSearchInterval));

            // Translate all components...

            btnClose.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2452);                        // Cancel

            lblTransactionIdHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2454);         // Receipt Id
            btnClear.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1280);                        // Clear
            btnItemSearch.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1262);                   // Search
            btnTransactionId.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1262);                // Search

            lblHeader.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2457);                       // Search journal

            lblCustomerIdHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2459);            // Customer account
            lblCustomerFirstNameHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2460);   // Customer First Name
            lblCustomerLastNameHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2461);    // Customer Last Name
            lblStoreIdHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2462);             // Store
            lblEmailHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51138);              // Email
            lblStaffIdHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2464);             // Staff
            lblItemIdHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2465);              // Product Id
            lblBarcodeHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2466);             // Bar code
            lblSerialNumberHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2467);        // Serial Number
            lblStartDateHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2468);           // Start Date
            lblEndDateHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(2469);             // End Date
        }

        private void btnTransactionId_Click(object sender, EventArgs e)
        {
            this.SearchCriteria.ReceiptId = this.txtTransactionId.Text.Trim();
            this.SearchCriteria.CustomerAccountNumber = this.txtCustomerId.Text.Trim();
            this.SearchCriteria.CustomerFirstName = this.txtCustomerFirstName.Text.Trim();
            this.SearchCriteria.CustomerLastName = this.txtCustomerLastName.Text.Trim();
            this.SearchCriteria.StoreId = this.txtStoreId.Text.Trim();
            this.SearchCriteria.ReceiptEmailAddress = this.txtEmail.Text.Trim();
            this.SearchCriteria.StaffId = this.txtStaffId.Text.Trim();
            this.SearchCriteria.ItemId = this.txtItemId.Text.Trim();
            this.SearchCriteria.Barcode = this.txtBarcode.Text.Trim();
            this.SearchCriteria.SerialNumber = this.txtSerialNumber.Text.Trim();
            this.SearchCriteria.StartDateTime = this.dtpFromDate.Value.Date.ToUniversalTime();
            // Set date to end of the day. Since the selected time is 12 AM, we add 1 day and subtract 1 millisecond to get 11:59:59.999 PM
            this.SearchCriteria.EndDateTime = this.dtpToDate.Value.Date.AddDays(1).AddMilliseconds(-1).ToUniversalTime();

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Hook up Scanner
                Dialog.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(OnScannerEvent);
                Dialog.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent += new ScannerMessageEventHandler(OnScannerEvent);
                Dialog.InternalApplication.Services.Peripherals.Scanner.ReEnableForScan();

                SetFormFocus();
            }

            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            Dialog.InternalApplication.Services.Peripherals.Scanner.DisableForScan();
            Dialog.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(OnScannerEvent);

            base.OnClosed(e);
        }

        private void SetFormFocus()
        {
            txtTransactionId.Focus();
        }

        private void OnScannerEvent(IScanInfo scanInfo)
        {
            if (scanInfo.ScanData.Length > 0)
            {
                this.txtTransactionId.Text = scanInfo.ScanData;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtTransactionId.Text = string.Empty;
            this.txtCustomerId.Text = string.Empty;
            this.txtCustomerFirstName.Text = string.Empty;
            this.txtCustomerLastName.Text = string.Empty;
            this.txtStoreId.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtStaffId.Text = string.Empty;
            this.txtItemId.Text = string.Empty;
            this.txtBarcode.Text = string.Empty;
            this.txtSerialNumber.Text = string.Empty;
            this.txtTransactionId.Text = string.Empty;
            this.dtpFromDate.Value = System.DateTime.Now.Subtract(TimeSpan.FromDays(DefaultJournalSearchInterval));
            this.dtpToDate.Value = System.DateTime.Now;
            
            SetFormFocus();
        }

        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            string itemId = string.Empty;

            DialogResult dialogResult = Dialog.InternalApplication.Services.Dialog.ItemSearch(InitialLoadedRows, ref itemId);
            if (dialogResult == DialogResult.OK)
            {
                this.txtItemId.Text = itemId;
            }
        }
    }
}
