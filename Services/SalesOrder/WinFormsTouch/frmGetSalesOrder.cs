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
    using System.Data.SqlClient;
    using System.Configuration;

    using LSRetailPosis;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.SaleItem;

    using CustomerOrderType = LSRetailPosis.Transaction.CustomerOrderType;
    using SalesStatus = LSRetailPosis.Transaction.SalesStatus;
    using System.Drawing;
    using Microsoft.Dynamics.BusinessConnectorNet;
    using System.Collections.ObjectModel;
    using System.Xml;
    using Microsoft.Dynamics.Retail.Pos.SalesOrder.CustomerOrderParameters;
    using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
   

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
        //additional field by Yonathan 22/06/2023
        private const string InvoiceIdString = "INVOICEID";
        private const string IsOpenString = "ISOPEN";
        private const string JournalIdString = "JOURNALID";
        //private const string TotalAmountDecimal = "TOTALAMOUNT";
        //end
        private const string FilterFormat =
            "[SALESID] LIKE '%{0}%' OR [CUSTOMERACCOUNT] LIKE '%{0}%' OR [CUSTOMERNAME] LIKE '%{0}%' OR [EMAIL] LIKE '%{0}%' OR [DATE] LIKE '%{0}%' OR [TOTALAMOUNT] LIKE '%{0}%'";

        private SalesStatus selectedOrderDocumentStatus;
        private SalesStatus selectedOrderSalesStatus;
        private string selectedOrderPickupDeliveryMode;
        private LSRetailPosis.POSProcesses.frmMessage refreshDialog;
        private BackgroundWorker refreshWorker;
        private OrderListModel dataModel;

        //add by yonathan 21/06/2023
        private string invoiceId;
        private string isOpen;
        //add by yonathan 21/06/2024
        string isB2bCust = "0";

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
        /// 

        //add by Yonathan to get total amount 30/06/2023
        public decimal selectedInvoiceAmount { get; private set; }
        //end

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
            btnReturn.Text = "Payment Invoice"; //ApplicationLocalizer.Language.Translate(56398);   //Return Order changed by Yonathan 26/06/2023
            btnCancelOrder.Text = ApplicationLocalizer.Language.Translate(56215);   //Cancel order
            btnEdit.Text = ApplicationLocalizer.Language.Translate(56212);   //View details
            btnPickUp.Text = "Print Invoice"; //ApplicationLocalizer.Language.Translate(56213);   //Pickup order changed by Yonathan 09092024
            btnClose.Text = ApplicationLocalizer.Language.Translate(56205);   //Close

            colOrderType.Caption = "Journal Payment"; //ApplicationLocalizer.Language.Translate(56216); //Order type  changed by Yonathan 26/06/2023
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
            if (textBoxSearch.Text == "") //custom by Yonathan 5/JAN/2023
            {
                textBoxSearch.Text = "Input No Sales Order or Customer Account or Customer Name";
                textBoxSearch.ForeColor = Color.Gray;
            }
        }

        private void textBoxSearch_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = btnSearch;
            if (textBoxSearch.Text == "Input No Sales Order or Customer Account or Customer Name") //custom by Yonathan 5/JAN/2023
            {
                textBoxSearch.Text = "";
                textBoxSearch.ForeColor = Color.Black;
            }
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

                // Start Edit By Erwin 12 March 2019
                /*
                // get active sales id
                //string param_order_id = "(";
                int count_param = 0;

                //foreach (DataRow row in dataModel.OrderList.Rows)
                //{
                //    if (count_param != 0)
                //    {
                //        param_order_id += ",";
                //    }

                //    param_order_id += ("'" + row[SalesIdString].ToString() + "'");
                //    count_param++;
                //}
                //param_order_id += ")";

                // get sales id based on warehouse
                //string connectionString = @"Data Source= DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=DevDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";//Persist Security Info=False;User ID=USER_NAME;Password=USER_PASS;
                //string connectionString = @"Data Source= DYNAMICS16\SQLAXDB1 ;Initial Catalog=PRDDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";
                string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;

                SqlConnection connection = new SqlConnection(connectionString); //LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                
                string param_sales_id = "(";

                try
                {
                    string queryString = "SELECT DISTINCT ST.SALESID FROM SALESTABLE ST ";
                    queryString += "WHERE ST.INVENTLOCATIONID LIKE '%" + ApplicationSettings.Database.StoreID + "%' ";
                    queryString += "AND ST.SHIPPINGDATEREQUESTED >= (GETDATE() - 60) ";
                    queryString += "AND ST.SALESSTATUS = 2 ";
                    //queryString += "AND ST.SALESID IN " + param_order_id;
                    //queryString += @" AND ST.SALESID NOT IN (
                    //                    SELECT DISTINCT SALESID
                    //                    FROM SALESLINE SL
                    //                    INNER JOIN INVENTDIM ID ON SL.INVENTDIMID = ID.INVENTDIMID AND SL.DATAAREAID = ID.DATAAREAID
                    //                    WHERE ID.INVENTLOCATIONID NOT LIKE '%" + ApplicationSettings.Database.StoreID + @"%' 
                    //                )";
                    
                    using(SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        if(connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        count_param = 0;

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                if (count_param != 0)
                                {
                                    param_sales_id += ",";
                                }

                                param_sales_id += ("'" + reader.GetString(reader.GetOrdinal("SALESID")) + "'");
                                count_param++;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Format Error", ex);
                }

                param_sales_id += ")";

                // filter data
                DataView dv = new DataView(dataModel.OrderList);
                if (param_sales_id == "()")
                {
                    //dv.RowFilter = "SALESID IN " + param_sales_id; //"SALESSTATUS = 3 AND SALESID IN " + param_sales_id +" AND SALESID IN " + param_order_id;
                    grSalesOrders.DataSource = dv;
                
                }
                else
                {
                    dv.RowFilter = "SALESID IN " + param_sales_id; //"SALESSTATUS = 3 AND SALESID IN " + param_sales_id +" AND SALESID IN " + param_order_id;
                    grSalesOrders.DataSource = dv;
                
                }
                //dv.RowFilter = "SALESID IN " + param_sales_id; //"SALESSTATUS = 3 AND SALESID IN " + param_sales_id +" AND SALESID IN " + param_order_id;
                //grSalesOrders.DataSource = dv;
                */

                // End Edit By Erwin 12 March 2019
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
            //this.SelectedSalesTotal = row.Field<decimal>(TotalAmountDecimal);
            this.selectedOrderSalesStatus = (SalesStatus)row[SalesStatusString];
            this.selectedOrderDocumentStatus = (SalesStatus)row[DocumentStatusString];
            this.selectedInvoiceAmount = row.Field<decimal>("INVOICEAMOUNT");
            // CustomerOrderType does not have default, failing if something else.
            this.SelectedOrderType = (CustomerOrderType)Enum.Parse(typeof(CustomerOrderType), row[OrderTypeString].ToString());

            this.selectedOrderPickupDeliveryMode = row.Field<string>(DeliveryModeString);

            //add by Yonathan to check whether this invoice has been settle, so the payment invoice button will be disabled 21/06/2023
            //ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getCustTrans", this.SelectedSalesOrderId.ToString());
            invoiceId = row.Field<string>(InvoiceIdString); //containerArray[2].ToString(); //getInvoiceId(containerArray[2].ToString());
            //ReadOnlyCollection<object> containerArrayInvoice = SalesOrder.InternalApplication.TransactionServices.Invoke("getSalesInvoice", invoiceId);
            isOpen = row.Field<string>(IsOpenString);
            //end
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (dataModel.OrderList != null && e != null && e.FocusedRowHandle >= 0 && e.FocusedRowHandle < dataModel.OrderList.Rows.Count)
            {
                GetSelectedRow();

                //get customer classification type
                ReadOnlyCollection<object> containerArray;
                var custId = gridView1.GetRowCellValue(gridView1.GetFocusedDataSourceRowIndex(), "CUSTOMERACCOUNT");
                if (SalesOrder.InternalApplication.TransactionServices.CheckConnection())
                {
                    try
                    {
                        containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getB2bRetailParam", custId);
                        isB2bCust = containerArray[6].ToString();


                    }
                    catch (Exception ex)
                    {
                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        throw;
                    }
                }
                
                //
                this.EnableButtons();
            }
        }

        /// <summary>
        /// Disable buttons in case no order is selected (like after searching with no results)
        /// </summary>
        private void DisableButtons()
        {
            this.btnEdit.Enabled = false;
            //this.btnPickUp.Enabled = false;
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
                (this.selectedOrderDocumentStatus == SalesStatus.Delivered);//|| this.selectedOrderDocumentStatus == SalesStatus.Invoiced);

            bool enableInvoice = false; //add by Yonathan to create invoice indise POS 13/01/2023

            // If the list is only for PackSlip creation, disable everything else
            if (this.dataModel is PackslipOrderListModel)
            {
                enableEdit = false;
                enablePickup = false;
                enableReturn = false;
                enableCancel = false;

                // Pick/Pack operations are unchanged (enablePickList, enablePackSlip);
            }
            
            // Add By Erwin 13 March 2019 - Start
            //modified by Yonathan 5/JAN/2023
            if(this.selectedOrderDocumentStatus == SalesStatus.Created)
            {
                enableEdit = true;
                enablePickup = false;
                enableReturn = false;
                enableCancel = false;
                enablePickList = false;
                enablePackSlip = true;
                enableInvoice = false;
            }
            else if (this.selectedOrderDocumentStatus == SalesStatus.Delivered)
            {
                enableEdit = false;
                enablePickup = false;
                enableReturn = false;
                enableCancel = false;
                enablePickList = false;
                enablePackSlip = false;
                enableInvoice = true;

                //disable invoice & payment feature right now
                enableInvoice = true;
                //enableReturn = false;
            }
            else if (this.selectedOrderDocumentStatus == SalesStatus.Invoiced)
            {
                enableInvoice = false;
                
               
                if (invoiceId != "" )//&& isOpen == "false" )
                {
                    enableReturn = false;
                    enablePickup = true;
                }
                else
                {
                    if (isB2bCust == "1")
                    {
                        enableReturn = true; //payment if canvas customer
                    }
                    else
                    {
                        enableReturn = false; //cannot payment if B2B
                    }

                    
                }
                //mod by Yonathan 13/06/2023 true because change function to payment invoice
                //enablePickup = false;

                //disable invoice & payment feature right now
                //enableInvoice = false;
                //enableReturn = false;
            }

            //add by Yonathan 26/06/2023 to prevent pickup when there is no result SO
            if (dataModel.OrderList.Rows.Count == 0)
            {
                enablePickup = false;
                enableEdit = false;
            }
            // Add By Erwin 13 March 2019 - End


            this.btnEdit.Enabled = enableEdit;
            this.btnPickUp.Enabled = enablePickup; //enablePickup; //disable by Yonathan as not useed 15/08/2023 
            this.btnReturn.Enabled = enableReturn;
            this.btnCancelOrder.Enabled = enableCancel;
            this.btnCreatePickList.Enabled = false; //enablePickList; //disable by Yonathan as not useed 15/08/2023 
            this.btnCreatePackSlip.Enabled = enablePackSlip;
            this.btnPrintPackSlip.Enabled = enablePrintPackSlip;
            this.btnCreateInvoice.Enabled = enableInvoice; //add by  Yonathan 13/01/2023

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
                    //edit to check whether b2b or not
                    
                    ReadOnlyCollection<object> containerArray;
                    //if (SalesOrder.InternalApplication.TransactionServices.CheckConnection())
                    //{
                    //    try
                    //    {
                    //        containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getB2bRetailParam", cot.Customer.CustomerId.ToString());
                    //        isB2bCust = containerArray[3].ToString();
                          
                            
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //        throw;
                    //    }
                    //}
                    if (isB2bCust == "1" || isB2bCust == "2")
                    {
                        SetSelectedOrderAndClose(cot);
                    }
                    else
                    {
                        CustomerOrderInfo parameters = SerializationHelper.GetInfoFromTransaction(cot);

                        // Serialize the Customer Order info into an XML doc
                        string xmlString = parameters.ToXml();

                        containerArray = SalesOrder.InternalApplication.TransactionServices.Invoke("updateCustomerOrder", xmlString);
                    }
                    //original
                    //SetSelectedOrderAndClose(cot);

                   

                }                
            }

        }

        private void btnPrintInvoice_Click(object sender, System.EventArgs e)
        {
            //CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Pickup);
            CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(this.SelectedSalesOrderId, this.SelectedOrderType, LSRetailPosis.Transaction.CustomerOrderMode.Edit);
            
            ITransactionSystem transSys = SalesOrder.InternalApplication.BusinessLogic.TransactionSystem;

            //SalesOrder.InternalApplication.RunOperation(Contracts.PosisOperations.PrintPreviousInvoice,cot);


            transSys.PrintTransaction(cot, true, false);
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

        //mod by Yonathan -> change the function to payment invoice
        private void btnPayment_Click(object sender, EventArgs e)
        {
            //string invoiceId = "";
            string journalNum = "";
            GetSelectedRow();
            string custId = gridView1.GetRowCellValue(gridView1.GetFocusedDataSourceRowIndex(), "CUSTOMERACCOUNT").ToString();
            decimal amountInvoice = 0;

            //ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.Invoke("getSalesInvoicesBySalesId", this.SelectedSalesOrderId.ToString());

            //invoiceId = getInvoiceId(containerArray[3].ToString());
            //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Do you want to use voucher for payment?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //{
            
            //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            //    if (dialog.DialogResult == DialogResult.Yes)
            //    {
            //        CP_frmPaymentInvoice invoicePayment = new CP_frmPaymentInvoice(SalesOrder.InternalApplication, this.SelectedSalesOrderId.ToString(), invoiceId, custId, this.selectedInvoiceAmount );
            //        invoicePayment.ShowDialog();
            //    }
            //    else if(dialog.DialogResult == DialogResult.No)
            //    {
                //if (dialog.DialogResult == DialogResult.Yes)
                //{
                    PostPayment(invoiceId, this.selectedInvoiceAmount, out journalNum);
                    if (journalNum != "")
                    {
                        SettlePayment(journalNum, invoiceId);
                    }
            //    }
                    
            //    //}
            //}

            
            RefreshGrid();
            GetSelectedRow();  // to reload "selectedOrderStatus" object.
            this.EnableButtons();
        }

        public string getInvoiceId(string _xmlString)
        {
            string invoiceId = "";

            string xmlString = _xmlString;//"<?xml version=\"1.0\" encoding=\"utf-8\"?><CustInvoiceJours><CustInvoiceJour RecId=\"5637653827\" InvoiceId=\"INV/23/00000234\" SalesId=\"SO/23/0000000469\" SalesType=\"3\" InvoiceDate=\"2023-06-12\" CurrencyCode=\"IDR\" InvoiceAmount=\"2200.000\" InvoiceAccount=\"C000000003\" InvoicingName=\"CUST KANVAS\" /></CustInvoiceJours>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            XmlNodeList invoiceJourNodes = xmlDoc.SelectNodes("//CustInvoiceJour");
            if (invoiceJourNodes.Count > 0)
            {
                XmlNode invoiceJourNode = invoiceJourNodes[0];
                //string recId = invoiceJourNode.Attributes["RecId"].Value;
                invoiceId = invoiceJourNode.Attributes["InvoiceId"].Value;
                //string salesId = invoiceJourNode.Attributes["SalesId"].Value;
                //string salesType = invoiceJourNode.Attributes["SalesType"].Value;
                //string invoiceDate = invoiceJourNode.Attributes["InvoiceDate"].Value;
                //string currencyCode = invoiceJourNode.Attributes["CurrencyCode"].Value;
                //string invoiceAmount = invoiceJourNode.Attributes["InvoiceAmount"].Value;
                //string invoiceAccount = invoiceJourNode.Attributes["InvoiceAccount"].Value;
                //string invoicingName = invoiceJourNode.Attributes["InvoicingName"].Value;


            }


            return invoiceId;
        }

        //end mod


        //old
        
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

        //private void btnCreatePackSlip_Click(object sender, EventArgs e)
        //{
        //    GetSelectedRow();

        //    CP_frmPackSlipDetails packingSlipDetails = new CP_frmPackSlipDetails(this.SelectedSalesOrderId.ToString());
        //    packingSlipDetails.ShowDialog();
        //    /*
        //    SalesOrderActions.TryCreatePackSlip(this.selectedOrderSalesStatus, this.SelectedSalesOrderId);
        //    RefreshGrid();
        //    GetSelectedRow();  // to reload "selectedOrderStatus" object.
        //    this.EnableButtons();*/
        //}

        //Mod by Yonathan 12/05/2023 for Custom Form Packing Slip
        private void btnCreatePackSlip_Click(object sender, EventArgs e)
        {
            GetSelectedRow();

            CP_frmPackingSlipDetail packingSlipDetails = new CP_frmPackingSlipDetail(SalesOrder.InternalApplication, this.SelectedSalesOrderId.ToString());
            packingSlipDetails.ShowDialog();
            RefreshGrid();
            GetSelectedRow();  // to reload "selectedOrderStatus" object.
            this.EnableButtons();
            /*
            SalesOrderActions.TryCreatePackSlip(this.selectedOrderSalesStatus, this.SelectedSalesOrderId);
            RefreshGrid();
            GetSelectedRow();  // to reload "selectedOrderStatus" object.
            this.EnableButtons();*/
        }

        private void btnPrintPackSlip_Click(object sender, EventArgs e)
        {
            //to call pack Slip Method
            SalesOrderActions.TryPrintPackSlip(this.selectedOrderDocumentStatus, this.SelectedSalesOrderId);
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            bool retValue;
            string comment;
            string invoiceAx;
            string journalNum;
            //Axapta ax;
            //Object[] parmInvoice = new object[2];
            //Object[] parmPayment = new object[11];
            //Object[] parmSettlement = new object[3];
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Create and Post Invoice for this Sales Order?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                /*if (dialog.DialogResult == DialogResult.Yes)
                {
                    CP_frmPaymentInvoice invoicePayment = new CP_frmPaymentInvoice(SalesOrder.InternalApplication, this.SelectedSalesOrderId.ToString(), invoiceId, custId);
                    invoicePayment.ShowDialog();
                }*/
                //else if(dialog.DialogResult == DialogResult.No)
                //{
                if (dialog.DialogResult == DialogResult.Yes)
                {
                    CreateInvoice(out invoiceAx);
                }
                
            }
            /*if (invoiceAx != "")
            {
                PostPayment(invoiceAx, out journalNum);
                if (journalNum != "")
                {
                    SettlePayment(journalNum, invoiceAx);
                }
            }*/
            
            
            /*
            ax = new Axapta();
            ax.Logon(ApplicationSettings.Database.DATAAREAID, null, null, null);
            object invoiceAX = "";
            object paymentAX = "";
            //MessageBox.Show("You click create invoice, but no function in it");
            parmInvoice[0] = this.SelectedSalesOrderId.ToString();
            parmInvoice[1] = DateTime.Now;
            var custId = gridView1.GetRowCellValue(gridView1.GetFocusedDataSourceRowIndex(), "CUSTOMERACCOUNT");  


            //invoice
            try
            {
                //call Static AX Class for Sales Order lines                                                 
               
                invoiceAX = ax.CallStaticClassMethod("NECI_SettleSOPayment", "SalesOrderInvoiceCreate", parmInvoice);
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage("Invoice has been created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //oDeleteSO = ax.CallStaticClassMethod("NECI_SettleSOPayment", "deleteSalesOrder", SalesId);
                //throw new Exception(string.Format("Rollback Sales Order Transaction... error : {0} ", ex.ToString()));
            }

            //Payment journal
            try
            {
                //call Static AX Class for Sales Order lines    
                parmPayment[0] = 0 ; //amountCash
                parmPayment[1] = 0 ; //amountDebit
                parmPayment[2] = 0; //amountCredit
                parmPayment[3] = 0; //amountZero
                parmPayment[4] = 0; //amountZero
                parmPayment[5] = this.SelectedSalesOrderId.ToString(); //SO
                parmPayment[6] = custId.ToString(); //row["CUSTOMERACCOUNT"] = customerAccount;
                parmPayment[7] = invoiceAX; //invoiceId
                parmPayment[8] = DateTime.Now; //transdate
                parmPayment[9] = ""; //costCenter
                parmPayment[10] = ApplicationSettings.Database.StoreID.ToString();         //StoreId            

                paymentAX = ax.CallStaticClassMethod("NECI_SettleSOPayment", "SalesOrderPaymentPOS", parmPayment);
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Payment has been created  \n {0}", paymentAX), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Payment error : {0}",ex.ToString()), MessageBoxButtons.OK, MessageBoxIcon.Information);
                //oDeleteSO = ax.CallStaticClassMethod("NECI_SettleSOPayment", "deleteSalesOrder", SalesId);
                //throw new Exception(string.Format("Rollback Sales Order Transaction... error : {0} ", ex.ToString()));
            }


            //Settlement
            if (paymentAX != "")
            {
                try
                {
                    //call Static AX Class for Sales Order lines    
                    parmSettlement[0] = custId.ToString();  //accountnum
                    parmSettlement[1] = paymentAX;
                    parmSettlement[2] = invoiceAX;//invoicenum
                    //[2] = 0; //voucher

                    //parmSettlement[10] = ApplicationSettings.Database.StoreID.ToString();         //StoreId            

                    paymentAX = ax.CallStaticClassMethod("NECI_SettleSOPayment", "SettlePaymentPOS", parmSettlement);
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Payment has been settled \n {0}", parmSettlement), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Payment error : {0}", ex.ToString()), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //oDeleteSO = ax.CallStaticClassMethod("NECI_SettleSOPayment", "deleteSalesOrder", SalesId);
                    //throw new Exception(string.Format("Rollback Sales Order Transaction... error : {0} ", ex.ToString()));
                }
            }
            */
            
            
            RefreshGrid();
            GetSelectedRow();  // to reload "selectedOrderStatus" object.
            this.EnableButtons();
        }

        private void SettlePayment(string _journalNum, string _invoiceNum)
        {
            var custId = gridView1.GetRowCellValue(gridView1.GetFocusedDataSourceRowIndex(), "CUSTOMERACCOUNT");
            try
            {
                object[] parameterList = new object[] 
							{
                                custId,
                                _journalNum,
                                _invoiceNum,                                
							};
                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("postSettlePayment", parameterList);
                
                if (containerArray[2].ToString() == "1")
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Payment has been settled \n "), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Settle error, please settle payment on AX");
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }

        private void PostPayment(string _invoiceAx, decimal _invoiceAmount, out string _journalNum)
        {
            _journalNum = "";
            var custId = gridView1.GetRowCellValue(gridView1.GetFocusedDataSourceRowIndex(), "CUSTOMERACCOUNT");              
            try
            {
                object[] parameterList = new object[] 
							{
                                _invoiceAmount,0,0,0,0,
								this.SelectedSalesOrderId.ToString(),
                                custId,
                                _invoiceAx,
                                DateTime.Now,
                                "",
                                ApplicationSettings.Database.StoreID.ToString()
							};

                /*object[] parameterList = new object[] 
							    {
								    amountCash,0,0,totalVoucher,0, //cash,debit,credit,voucher,other not implemented
								    salesId,
								    custId,
								    _invoiceAx,
								    DateTime.Now,
								    "",
								    ApplicationSettings.Database.StoreID.ToString()
							    };*/

                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("postSalesPayment", parameterList);
                _journalNum = containerArray[3].ToString();
                if (containerArray[2].ToString() == "Success")
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(String.Format("Payment journal {0} has been created", _journalNum), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Payment error, please post Payment on AX");
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }

        }
        private void CreateInvoice(out string _invoiceAx)
        {
            _invoiceAx = "";
            string statusInvoice;
            try
            {
                object[] parameterList = new object[] 
							{
								this.SelectedSalesOrderId.ToString(),
								DateTime.Now
								
							};

                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("postSalesInvoice", parameterList);
                _invoiceAx = containerArray[3].ToString();
                statusInvoice=    containerArray[2].ToString();
                if(statusInvoice == "Success")
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(String.Format("Invoice {0} has been created", _invoiceAx), MessageBoxButtons.OK, MessageBoxIcon.Information); 

                    ////print

                    //ITransactionSystem transSys = SalesOrder.InternalApplication.BusinessLogic.TransactionSystem;
                    //transSys.PrintTransaction(SelectedOrder, true, true);
                    //update invoice id yonathan 06092024
                    updateInvoiceId(_invoiceAx, this.SelectedSalesOrderId.ToString());
                    ITransactionSystem transSys = SalesOrder.InternalApplication.BusinessLogic.TransactionSystem;
                    SelectedOrder.InvoiceComment = _invoiceAx;
                    SelectedOrder.Save();

                    transSys.PrintTransaction(SelectedOrder, false, false);
                }
                else
                {
	                throw new Exception("Invoice error, please post invoice on AX");
                }                
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }
        //update invoice id yonathan 06092024
        private void updateInvoiceId(string _invoiceAx, string _salesId)
        {
            string storeId = "";
            SqlConnection connection = ApplicationSettings.Database.LocalConnection;
            storeId = ApplicationSettings.Terminal.StoreId;
            //var retailTransaction = posTransaction as RetailTransaction;
            try
            {
                string queryString = @" UPDATE RETAILTRANSACTIONTABLE
                                        SET INVOICECOMMENT = @INVOICECOMMENT
                                        WHERE STORE = @STOREID
                                        AND  SALESORDERID =@SALESID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STOREID", storeId);
                    command.Parameters.AddWithValue("@SALESID", _salesId);
                    command.Parameters.AddWithValue("@INVOICECOMMENT", _invoiceAx);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
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
                    //MessageBox.Show("In");
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
