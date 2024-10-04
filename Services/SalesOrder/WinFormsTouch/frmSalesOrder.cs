/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Data;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using CustomerOrderMode = LSRetailPosis.Transaction.CustomerOrderMode;
using CustomerOrderType = LSRetailPosis.Transaction.CustomerOrderType;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    public partial class frmSalesOrder : LSRetailPosis.POSProcesses.frmTouchBase
    {
        private RetailTransaction transaction;

        /// <summary>
        /// Set transaction 
        /// </summary>
        public RetailTransaction Transaction
        {
            get { return transaction; }
            set { transaction = value; }
        }

        /// <summary>
        /// Leads to sales order form.
        /// </summary>
        public frmSalesOrder()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                ClearSalesOrderInput();
                ClearCustomerInput();
                LoadCustomer(this.Transaction);
            }

            // Add By Erwin 20 March 2019
            dtpFromDate.Text = DateTime.Now.ToString(); //DateTime.Now.AddDays(-14).ToString(); //yonathan 23/05/2023 change to today as requested
            dtpToDate.Text = DateTime.Now.ToString();
            btnCustomerSearch.Visible = false;
            btnCustomerSearch.Enabled = false;
            // End Add By Erwin 20 March 2019

            this.orderBox.SelectedIndex = 0;

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's are reserved at 56100 - 56199
            // 
            // The last Text ID in use is:  56131
            //

            // Translate all texts.....

            this.Text = lblHeading.Text = ApplicationLocalizer.Language.Translate(56114); //Recall orders

            lblSalesOrderIdHeading.Text = ApplicationLocalizer.Language.Translate(56125); //Search for order id...
            lblCustomerIdHeading.Text = ApplicationLocalizer.Language.Translate(56113); //Search for customer...
            lblFromDate.Text = ApplicationLocalizer.Language.Translate(56136); //Start date:
            lblToDate.Text = ApplicationLocalizer.Language.Translate(56137); //End date:

            btnOk.Text = ApplicationLocalizer.Language.Translate(56204); //Get sales order
            btnCustomerSearch.Text = ApplicationLocalizer.Language.Translate(56115); //Search
            btnCancel.Text = ApplicationLocalizer.Language.Translate(56134); //Cancel

            EnableButtons();
        }

        private void SetFormFocus()
        {
            txtSalesOrderId.Focus();
        }

        private void ClearSalesOrderInput()
        {
            txtSalesOrderId.Text = string.Empty;
        }

        private void ClearCustomerInput()
        {
            txtCustomerId.Text = string.Empty;
        }

        private void EnableButtons()
        {
            // only enabled if a text field is not blank or date fields are set
            btnOk.Enabled =
                !(string.IsNullOrWhiteSpace(txtCustomerId.Text)
                    && string.IsNullOrWhiteSpace(txtSalesOrderId.Text)
                    && (!dtpFromDate.Checked)
                    && (!dtpToDate.Checked)
                  );
        }

        private void LoadCustomer(RetailTransaction fromTransaction)
        {
            if (fromTransaction != null && fromTransaction.Customer != null)
            {
                txtCustomerId.Text = fromTransaction.Customer.CustomerId;
            }
            else
            {
                txtCustomerId.Text = string.Empty;
            }
        }

        private void LookupCustomer()
        {
            RetailTransaction tempTrans = (RetailTransaction)SalesOrder.InternalApplication.BusinessLogic.Utility.CreateRetailTransaction(
                    ApplicationSettings.Terminal.StoreId,
                    ApplicationSettings.Terminal.StoreCurrency,
                    ApplicationSettings.Terminal.TaxIncludedInPrice,
                    SalesOrder.InternalApplication.Services.Rounding);

            tempTrans.OperatorId = this.Transaction.OperatorId;

            SalesOrder.InternalApplication.Services.Customer.Search(tempTrans);

            LoadCustomer(tempTrans);
        }

        private void SearchForOrders()
        {
            // Max. no of record to be fetched as when Customer Order Search is performed.
            // Required for a good response time.
            //const int RESULT_MAX_COUNT = 100;
            const int RESULT_MAX_COUNT = 300;

            try
            {
                // Show the available sales orders for selection...
                DateTime? fromDate = null;
                DateTime? toDate = null;
                string customer = (txtCustomerId.Text ?? string.Empty).Trim();
                string order = (txtSalesOrderId.Text ?? string.Empty).Trim();

                if (dtpFromDate.Checked) { fromDate = dtpFromDate.Value; }
                if (dtpToDate.Checked) { toDate = dtpToDate.Value; }

                if (fromDate.HasValue && toDate.HasValue && dtpFromDate.Value > dtpToDate.Value)
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(2470, MessageBoxButtons.OK, MessageBoxIcon.Error); // The start date must be greater than or equal to the end date.
                    return;
                }

                //Note : Restricting the result to only 100 records for a good response time.
                SearchOrderListModel list = new SearchOrderListModel(customer, order, fromDate, toDate, RESULT_MAX_COUNT, this.orderBox.SelectedIndex);

                using (frmGetSalesOrder listDialog = new frmGetSalesOrder(list))
                {
                    SalesOrder.InternalApplication.ApplicationFramework.POSShowForm(listDialog);
                    if (listDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this.Transaction = listDialog.SelectedOrder;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
            catch (PosisException px)
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSErrorDialog(px);
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), px);
            }
            catch (Exception x)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                throw;
            }
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            LookupCustomer();
            SetFormFocus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SearchForOrders();
        }

        private void OnFieldChanged(object sender, EventArgs e)
        {
            EnableButtons();
        }
    }
}
