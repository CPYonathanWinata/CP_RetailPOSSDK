/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;

    using LSRetailPosis;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.SaleItem;

    using CustomerOrderType = LSRetailPosis.Transaction.CustomerOrderType;
    using SalesStatus = LSRetailPosis.Transaction.SalesStatus;

    public partial class frmGetSalesOrder : LSRetailPosis.POSProcesses.frmTouchBase
    {
        private const string DeliveryModeString = "DELIVERYMODE";
        private const string SalesIdString = "SALESID";
        private const string DocumentStatusString = "DOCUMENTSTATUS";
        private const string SalesStatusString = "SALESSTATUS";
        private const string OrderTypeString = "ORDERTYPE";
        private const string CustomerAccountString = "CUSTOMERACCOUNT";
        private const string CustomerNameString = "CUSTOMERNAME";
        private const string EmailString = "EMAIL";
        private const string ReferenceIdString = "CHANNELREFERENCEID";

        private const string FilterFormat =
            "[SALESID] LIKE '%{0}%' OR [CUSTOMERACCOUNT] LIKE '%{0}%' OR [CUSTOMERNAME] LIKE '%{0}%' OR [EMAIL] LIKE '%{0}%' OR [DATE] LIKE '%{0}%' OR [TOTALAMOUNT] LIKE '%{0}%'";

        private SalesStatus selectedOrderDocumentStatus;
        private SalesStatus selectedOrderSalesStatus;
        private string selectedOrderPickupDeliveryMode;
        private LSRetailPosis.POSProcesses.frmMessage refreshDialog;
        private BackgroundWorker refreshWorker;
        private OrderListModel dataModel;

        /// <summary>
        /// Returns selected sales order id as string.
        /// </summary>
        public string SelectedSalesOrderId { get; private set; }

        /// <summary>
        /// Returns the order type of the selected order
        /// </summary>
        public CustomerOrderType SelectedOrderType { get; private set; }

        /// <summary>
        /// Get the selected (and instantiated) order
        /// </summary>
        public CustomerOrderTransaction SelectedOrder { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmGetSalesOrder"/> class. 
        /// </summary>
        public frmGetSalesOrder()
        {
            InitializeComponent();
        }

        internal frmGetSalesOrder(OrderListModel data)
            : this()
        {
            this.dataModel = data;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.colSalesStatus.DisplayFormat.Format = new CustomerOrderStatusFormatter();
                this.colDocumentStatus.DisplayFormat.Format = new CustomerOrderStatusFormatter();
                this.colOrderType.DisplayFormat.Format = new CustomerOrderTypeFormatter();

                this.TranslateLabels();

                //For improved UX, delay refreshing the grid until the OnShown event so that the form is completely drawn.
            }

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            RefreshGrid();
            base.OnShown(e);
        }

        // See PS#3312 - Appears this should be invoked but is not.
        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's are reserved at 56200 - 56299
            // 
            // The last Text ID in use is:  56211
            //

            // Translate everything
            btnCreatePackSlip.Text = ApplicationLocalizer.Language.Translate(56218);   //Create packing slip
            btnPrintPackSlip.Text = ApplicationLocalizer.Language.Translate(56219);   //Print packing slip
            btnCreatePickList.Text = ApplicationLocalizer.Language.Translate(56104);   //Create picking list
            btnReturn.Text = ApplicationLocalizer.Language.Translate(56398);   //Return Order
            btnCancelOrder.Text = ApplicationLocalizer.Language.Translate(56215);   //Cancel order
            btnEdit.Text = ApplicationLocalizer.Language.Translate(56212);   //View details
            btnPickUp.Text = ApplicationLocalizer.Language.Translate(56213);   //Pickup order
            btnClose.Text = ApplicationLocalizer.Language.Translate(56205);   //Close

            colOrderType.Caption = ApplicationLocalizer.Language.Translate(56216); //Order type
            colSalesStatus.Caption = ApplicationLocalizer.Language.Translate(56217); // Order status
            colDocumentStatus.Caption = ApplicationLocalizer.Language.Translate(56265); // Document status
            colSalesOrderID.Caption = ApplicationLocalizer.Language.Translate(56206); //Sales order
            colCreationDate.Caption = ApplicationLocalizer.Language.Translate(56207); //Created date and time
            colTotalAmount.Caption = ApplicationLocalizer.Language.Translate(56210); //Total
            colCustomerAccount.Caption = ApplicationLocalizer.Language.Translate(56224); //Customer Account
            colCustomerName.Caption = ApplicationLocalizer.Language.Translate(56225); //Customer
            colEmail.Caption = ApplicationLocalizer.Language.Translate(56236); //E-mail

            //title
            this.Text = ApplicationLocalizer.Language.Translate(56106); //Sales orders
            lblHeading.Text = ApplicationLocalizer.Language.Translate(56106); //Sales orders

            //Do not allow filtering from the grid UI
            gridView1.OptionsCustomization.AllowFilter = false;
            gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            gridView1.MovePrevPage();
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            gridView1.MoveNextPage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gridView1.MovePrev();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gridView1.MoveNext();
        }

        private void textBoxSearch_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = btnEdit;
        }

        private void textBoxSearch_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = btnSearch;
        }

        private void RefreshGrid()
        {
            // Pop a "loading..." dialog and refresh the list contents in the background.
            using (refreshDialog = new LSRetailPosis.POSProcesses.frmMessage(56141, MessageBoxIcon.Information, true))  //"Searching for orders..."
            using (refreshWorker = new BackgroundWorker())
            {
                //Create a background worker to fetch the data
                refreshWorker.DoWork += refreshWorker_DoWork;
                refreshWorker.RunWorkerCompleted += refreshWorker_RunWorkerCompleted;

                //listen to th OnShow event of the dialog so we can kick-off the thread AFTER the dialog is visible
                refreshDialog.Shown += refreshDialog_Shown;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(refreshDialog);

                //Worker thread terminates, which causes the dialog to close, and both can safely dispose at the end of the 'using' scope.
            }
        }

        void refreshDialog_Shown(object sender, EventArgs e)
        {
            // Set the wait cursor and then kick-off the async worker thread.
            refreshDialog.UseWaitCursor = true;
            refreshWorker.RunWorkerAsync();
        }

        void refreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //If there were any exceptions during the Refresh work, then throw them now.
                if (e.Error != null)
                {
                    throw e.Error;
                }

                // Otherwise, reset the grid datasource using the newly refreshed data model.
                grSalesOrders.DataSource = dataModel.OrderList;

                if (dataModel.OrderList != null && dataModel.OrderList.Rows.Count == 0)
                {
                    // There are no sales orders in the database for this customer....
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56123, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);

                // "An error occurred while refreshing the list."
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56232, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // close the "Loading..." dilaog
                refreshDialog.UseWaitCursor = false;
                refreshDialog.Close();

                // Update the buttons.
                this.EnableButtons();
            }
        }

        void refreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            dataModel.Refresh();
        }

        private void GetSelectedRow()
        {
            DataRow row = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);

            this.SelectedSalesOrderId = row.Field<string>(SalesIdString);

            this.selectedOrderSalesStatus = (SalesStatus)row[SalesStatusString];
            this.selectedOrderDocumentStatus = (SalesStatus)row[DocumentStatusString];

            // CustomerOrderType does not have default, failing if something else.
            this.SelectedOrderType = (CustomerOrderType)Enum.Parse(typeof(CustomerOrderType), row[OrderTypeString].ToString());

            this.selectedOrderPickupDeliveryMode = row.Field<string>(DeliveryModeString);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (dataModel.OrderList != null && e != null && e.FocusedRowHandle >= 0 && e.FocusedRowHandle < dataModel.OrderList.Rows.Count)
            {
                GetSelectedRow();
                this.EnableButtons();
            }
        }

        /// <summary>
        /// Disable buttons in case no order is selected (like after searching with no results)
        /// </summary>
        private void DisableButtons()
        {
            this.btnEdit.Enabled = false;
            this.btnPickUp.Enabled = false;
            this.btnReturn.Enabled = false;
            this.btnCancelOrder.Enabled = false;
            this.btnCreatePickList.Enabled = false;
            this.btnCreatePackSlip.Enabled = false;
            this.btnPrintPackSlip.Enabled = false;
        }

        private void EnableButtons()
        {
            bool isSalesOrder = this.SelectedOrderType == CustomerOrderType.SalesOrder;
            bool isShipping = !ApplicationSettings.Terminal.PickupDeliveryModeCode.Equals(this.selectedOrderPickupDeliveryMode, StringComparison.OrdinalIgnoreCase);

            // invoiced (shipped/picked up at store) > delivered (packed) > processing (picked) > created
            // document status -> highest line status
            // sales status -> lowest line status

            // always allow view details button
            bool enableEdit = true;

            // can return if at least one line is invoiced
            bool enableReturn = isSalesOrder && this.selectedOrderDocumentStatus == SalesStatus.Invoiced;

            // can cancel if no line is more than created (order cannot have any changes)
            bool enableCancel = isSalesOrder && this.selectedOrderDocumentStatus == SalesStatus.Created;

            // can pick if at least one line is not fully invoiced
            bool enablePickup = isSalesOrder && this.selectedOrderSalesStatus != SalesStatus.Invoiced;

            // there must be at least one line not picked
            bool enablePickList = isSalesOrder && this.selectedOrderSalesStatus == SalesStatus.Created;

            // only pack shipped orders - there must be at least one line created or picked
            bool enablePackSlip = isSalesOrder && isShipping &&
                (this.selectedOrderSalesStatus == SalesStatus.Created || this.selectedOrderSalesStatus == SalesStatus.Processing);

            // can print pack slip if pack slip has been created - at least one line has been packed or invoiced
            bool enablePrintPackSlip = isSalesOrder && isShipping &&
                (this.selectedOrderDocumentStatus == SalesStatus.Delivered || this.selectedOrderDocumentStatus == SalesStatus.Invoiced);

            // If the list is only for PackSlip creation, disable everything else
            if (this.dataModel is PackslipOrderListModel)
            {
                enableEdit = false;
                enablePickup = false;
                enableReturn = false;
                enableCancel = false;

                // Pick/Pack operations are unchanged (enablePickList, enablePackSlip);
            }

            this.btnEdit.Enabled = enableEdit;
            this.btnPickUp.Enabled = enablePickup;
            this.btnReturn.Enabled = enableReturn;
            this.btnCancelOrder.Enabled = enableCancel;
            this.btnCreatePickList.Enabled = enablePickList;
            this.btnCreatePackSlip.Enabled = enablePackSlip;
            this.btnPrintPackSlip.Enabled = enablePrintPackSlip;
        }

        private void SetSelectedOrderAndClose(CustomerOrderTransaction transaction)
        {
            if (transaction == null)
            {
                return;
            }

            this.UpdateSerialNumbers(transaction);
            this.SelectedOrder = transaction;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Updates serial numbers for items in transaction.
        /// </summary>
        /// <remarks>This function will ask user to confirm serial numbers for every sales line to be picked up in transaction.</remarks>
        /// <param name="transaction">The customer order transaction.</param>
        private void UpdateSerialNumbers(CustomerOrderTransaction transaction)
        {
            if (transaction == null)
            {
                return;
            }

            if (transaction.Mode == CustomerOrderMode.Pickup)
            {
                IEnumerable<SaleLineItem> pickupSaleItems = transaction.SaleItems.Where(
                    item => item.Quantity != 0 &&
                            ApplicationSettings.Terminal.PickupDeliveryModeCode.Equals(item.DeliveryMode.Code));

                foreach (var saleLineItem in pickupSaleItems)
                {
                    SalesOrder.InternalApplication.Services.Item.UpdateSerialNumberInfo(saleLineItem);
                }
            }
        }

        private void SetSearchFilter(string p)
        {
            string filter = string.Empty;

            if (!string.IsNullOrWhiteSpace(p))
            {
                filter = string.Format(FilterFormat, p);
            }

            gridView1.ActiveFilterString = filter;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetSearchFilter(textBoxSearch.Text.Trim());
            
            // Check if we have results after searching
            if (gridView1.DataRowCount > 0)
            {
                GetSelectedRow();
                this.EnableButtons();
            }
            else
            {
                // Disable buttons as no order matches the search criteria
                this.DisableButtons();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetSearchFilter(string.Empty);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get order
            CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Edit);
            
            if (cot != null)
            {
                if (SalesOrderActions.ShowOrderDetails(cot, OrderDetailsSelection.ViewDetails))
                {
                    SetSelectedOrderAndClose(cot);
                }                
            }

        }

        private void btnPickUp_Click(object sender, EventArgs e)
        {
            // Get order
            // set Mode = Pickup
            CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Pickup);
            
            if (cot != null)
            {
                if (SalesOrderActions.ShowOrderDetails(cot, OrderDetailsSelection.PickupOrder))
                {
                    SetSelectedOrderAndClose(cot);
                }
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            //Get order
            //set Mode = Cancel
            CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Cancel);
            if (cot != null)
            {
                if (cot.OrderStatus == SalesStatus.Processing)
                {
                    //Order cannot be cancelled at this time from POS
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56237, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (SalesOrderActions.ShowOrderDetails(cot, OrderDetailsSelection.CancelOrder))
                {
                    SetSelectedOrderAndClose(cot);
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Get the order from the grid
            CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Edit);
            if (cot != null)
            {
                //Now get an invoice from the order
                cot = SalesOrderActions.ReturnOrderInvoices(cot);
                SetSelectedOrderAndClose(cot);
            }
        }

        private void btnCreatePickList_Click(object sender, EventArgs e)
        {
            SalesOrderActions.TryCreatePickListForOrder(this.selectedOrderSalesStatus, this.SelectedSalesOrderId);
            RefreshGrid();
        }

        private void btnCreatePackSlip_Click(object sender, EventArgs e)
        {
            SalesOrderActions.TryCreatePackSlip(this.selectedOrderSalesStatus, this.SelectedSalesOrderId);
            RefreshGrid();
            GetSelectedRow();  // to reload "selectedOrderStatus" object.
            this.EnableButtons();
        }

        private void btnPrintPackSlip_Click(object sender, EventArgs e)
        {
            //to call pack Slip Method
            SalesOrderActions.TryPrintPackSlip(this.selectedOrderDocumentStatus, this.SelectedSalesOrderId);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal abstract class OrderListModel
    {
        public abstract void Refresh();

        public DataTable OrderList
        {
            get;
            protected set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PackslipOrderListModel : OrderListModel
    {
        private string customerId = string.Empty;

        public PackslipOrderListModel(string customerId)
        {
            this.customerId = customerId;
        }

        public override void Refresh()
        {
            bool retVal;
            string comment;
            DataTable salesOrders = null;

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (SalesOrder.InternalApplication.TransactionServices.CheckConnection())
                {
                    // Publish the Sales order to the Head Office through the Transaction Services...
                    SalesOrder.InternalApplication.Services.SalesOrder.GetCustomerOrdersForPackSlip(out retVal, out comment, ref salesOrders, customerId);

                    this.OrderList = salesOrders;
                }
            }
            catch (LSRetailPosis.PosisException px)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), px);
                throw;
            }
            catch (Exception x)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                throw new LSRetailPosis.PosisException(52300, x);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class SearchOrderListModel : OrderListModel
    {
        private string customerId = string.Empty;
        private string orderId = string.Empty;
        DateTime? startDate = null;
        DateTime? endDate = null;
        int? resultMaxCount;

        public SearchOrderListModel(string customerSearchTerm, string orderSearchTerm, DateTime? startDateTerm, DateTime? endDateTerm, int? resultMaxCount)
        {
            this.customerId = customerSearchTerm;
            this.orderId = orderSearchTerm;
            this.startDate = startDateTerm;
            this.endDate = endDateTerm;
            this.resultMaxCount = resultMaxCount;
        }

        public override void Refresh()
        {
            this.OrderList = SalesOrderActions.GetOrdersList(this.customerId, this.orderId, this.startDate, this.endDate, this.resultMaxCount);
        }
    }
}
