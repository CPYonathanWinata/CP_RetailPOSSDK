/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.Common;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "frm")]
    public partial class frmGetSalesInvoices : LSRetailPosis.POSProcesses.frmTouchBase
    {

        /// <summary>
        /// Returns sales order as datatable.
        /// </summary>
        /// <remarks>
        ///     [InvoiceId]
        ///     [InvoiceNumber]
        ///     [Date]
        ///     [Amount]
        ///     [Currency]
        /// </remarks>
        public DataTable Invoices { get; set; }

        /// <summary>
        /// Returns selected sales order id as string.
        /// </summary>
        public string SelectedInvoiceId { get; private set; }

        /// <summary>
        /// Transaction returned items will be added to
        /// </summary>
        public CustomerOrderTransaction Transaction { get; private set; }

        /// <summary>
        /// Keeps in sales order form.
        /// </summary>
        public frmGetSalesInvoices()
        {
            InitializeComponent();
        }

        public frmGetSalesInvoices(DataTable salesInvoiceRecords, CustomerOrderTransaction originalTransaction)
            : this()
        {
            if (salesInvoiceRecords == null) throw new ArgumentNullException("salesInvoiceRecords");
            if (originalTransaction == null) throw new ArgumentNullException("originalTransaction");

            Invoices = salesInvoiceRecords;

            // Copy the original order and clear all the items/charges.
            CustomerOrderTransaction returnTransaction = new CustomerOrderTransaction(originalTransaction, originalTransaction.ISalesOrder);
            SalesOrderActions.SetCustomerOrderDefaults(returnTransaction);
            returnTransaction.DeliveryMode = originalTransaction.DeliveryMode;
            returnTransaction.RequestedDeliveryDate = originalTransaction.RequestedDeliveryDate;
            returnTransaction.SaleItems.Clear();
            returnTransaction.MiscellaneousCharges.Clear();
            returnTransaction.Mode = CustomerOrderMode.Return;
            returnTransaction.LockPrices = true;

            // Clear deposit details from the Return because we simply return the full invoiced amount (deposit + payments) for the items,
            // and there is no way to allocate the actual deposit paid for individual lines on the transaction in the case of a partial return.
            returnTransaction.PrepaymentAmountOverridden = true;
            returnTransaction.PrepaymentAmountRequired = 0.0m;

            this.Transaction = returnTransaction;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
                grInvoices.DataSource = Invoices;
            }

            base.OnLoad(e);
        }

        // See PS#3312 - Appears this should be invoked but is not.
        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's are reserved at 56247 - 56251
            // 
            // The last Text ID in use is:  56247
            //

            // Translate everything
            btnSelect.Text = ApplicationLocalizer.Language.Translate(57104);     //Select
            btnClose.Text = ApplicationLocalizer.Language.Translate(56205);     //Close
            colInvoiceAccount.Caption = ApplicationLocalizer.Language.Translate(56247);     //Account
            colInvoiceNumber.Caption = ApplicationLocalizer.Language.Translate(56248);      //Number
            colInvoiceDate.Caption = ApplicationLocalizer.Language.Translate(56249);        //Date
            colInvoiceAmount.Caption = ApplicationLocalizer.Language.Translate(56250);      //Amount
            colInvoiceCurrency.Caption = ApplicationLocalizer.Language.Translate(56251);    //Currency

            //title
            this.Text = ApplicationLocalizer.Language.Translate(2401); //Sales Invoices
        }

        private void ReturnItemsFromInvoice()
        {
            bool retValue = false;
            string comment = string.Empty;
            string invoiceId = SelectedInvoiceId;
            CustomerOrderTransaction invoiceTransaction;
            DialogResult results = DialogResult.None;

            try
            {
                // Construct a transaction from the given Invoice
                SalesOrder.GetSalesInvoiceTransaction(ref retValue, ref comment, invoiceId, out invoiceTransaction);

                if (retValue)
                {
                    ReturnOperationHelper.RemoveNonReturnableItems(invoiceTransaction);

                    // Check if there are any items left in the transaction to return...
                    if (invoiceTransaction.SaleItems.Count == 0)
                    {
                        // This transaction does not include any items that can be returned.
                        using (frmMessage msgDlg = new frmMessage(3684, MessageBoxButtons.OK, MessageBoxIcon.Exclamation))
                        {
                            POSFormsManager.ShowPOSForm(msgDlg);
                        }

                        this.DialogResult = results;
                        return;
                    }

                    invoiceTransaction.Mode = CustomerOrderMode.Return;
                    ReturnTransactionConfirmation returnTransactionConfirmation = new ReturnTransactionConfirmation()
                    {
                        PosTransaction = invoiceTransaction,
                    };

                    InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(returnTransactionConfirmation, () =>
                    {
                        if (returnTransactionConfirmation.Confirmed)
                        {
                            // The user selected to return items...
                            // Transfer the items to the posTranscation for the return
                            // Updates customer and calculates taxes based upon items and customer
                            SalesOrderActions.InsertReturnedItemsIntoTransaction(returnTransactionConfirmation.ReturnedLineNumbers, invoiceTransaction, this.Transaction);

                            // We only support returning a single-invoice at a time, so Close the form after processing the first one.
                            results = System.Windows.Forms.DialogResult.OK;
                            this.Close();
                        }
                    }
                    );

                    SalesOrder.InternalApplication.Services.Interaction.InteractionRequest(request);
                }
                else
                {
                    //An error occurred in the operation...
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(10002, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.HandleException(this.ToString(), ex);

            }

            this.DialogResult = results;
        }


        private void GetSelectedRow()
        {
            System.Data.DataRow Row = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
            SelectedInvoiceId = (string)Row["INVOICEID"];
        }

        #region events

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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetSelectedRow();
            ReturnItemsFromInvoice();
        }

        private void grInvoices_DoubleClick(object sender, EventArgs e)
        {
            GetSelectedRow();
            ReturnItemsFromInvoice();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if ((this.Invoices != null) && (e != null)
                && (e.FocusedRowHandle >= 0) && (e.FocusedRowHandle < this.Invoices.Rows.Count))
            {
                this.GetSelectedRow();
            }
        }

        #endregion
    }
}
