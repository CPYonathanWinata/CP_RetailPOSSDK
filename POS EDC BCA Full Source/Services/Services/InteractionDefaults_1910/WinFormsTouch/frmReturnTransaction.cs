/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [SuppressMessage("Microsoft.Naming", "CA1704", Justification = "Unfortunately this is how the form was originally named")]
    [Export("ReturnTransactionForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmReturnTransaction : frmTouchBase, IInteractionView
    {

        #region Member variables

        private RetailTransaction posTransaction;
        private LinkedList<int> returnedLineNumbers = new LinkedList<int>();

        #endregion

        #region Properties

        public LinkedList<int> ReturnedLineNumbers
        {
            get { return returnedLineNumbers; }
        }

        #endregion

        protected frmReturnTransaction()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public frmReturnTransaction(ReturnTransactionConfirmation returnTransactionConfirmation)
            : this()
        {
            if (returnTransactionConfirmation == null)
            {
                throw new ArgumentNullException("returnTransactionConfirmation");
            }

            this.posTransaction = (RetailTransaction)returnTransactionConfirmation.PosTransaction;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmReturnTransaction are reserved at 2650 - 2699
                // In use now are ID's 2650 - 2661
                //
                this.receiptItems1.SetRowItemHeight(40);
                this.receiptItems1.DesignAllowedOnPos = LSRetailPosis.Settings.VisualProfiles.VisualProfile.DesignAllowedOnPos;

                this.Text = ApplicationLocalizer.Language.Translate(1663); // Return Transaction
                lblHeading.Text = ApplicationLocalizer.Language.Translate(2651); // Returnable items
                btnSelectLine.Text = ApplicationLocalizer.Language.Translate(2655);  // Select line
                btnSelectAll.Text = ApplicationLocalizer.Language.Translate(2656);  // Select all
                btnDeselectAll.Text = ApplicationLocalizer.Language.Translate(2657);  // Clear selection
                btnReturn.Text = ApplicationLocalizer.Language.Translate(2658);  // Return items
                btnCancel.Text = ApplicationLocalizer.Language.Translate(2659);  // Cancel

                if (Functions.CountryRegion == SupportedCountryRegion.RU)
                {
                    btnDisplayRefundAmounts.Text = ApplicationLocalizer.Language.Translate(106022); // Display refund
                }
                else
                {
                    btnDisplayRefundAmounts.Visible = false;
                }

                receiptItems1.SetMode(LSRetailPosis.POSProcesses.WinControls.ReceiptItemsViewMode.ItemsSelect);
                receiptItems1.DisplayItems(posTransaction);
                receiptItems1.ClearSelection();
            }

            base.OnLoad(e);
        }

        private void btnDisplayRefundAmounts_Click(object sender, System.EventArgs e)
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            if (retailTransaction != null)
            {
                RetailTransaction clonedTransaction = (RetailTransaction)retailTransaction.CloneTransaction();
                // Clear SaleIsReturnSale flag as we do not want to treat this transaction as a return transaction, as on this stage it is just a stub for the return transaction.
                clonedTransaction.SaleIsReturnSale = false;
                SaleRefundDisplayNotification notification = new SaleRefundDisplayNotification()
                {
                    RetailTransaction = clonedTransaction
                };

                InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(notification, () => { });

                PosApplication.Instance.Services.Interaction.InteractionRequest(request);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            // Populate the return list if some items are selected. Otherwise report with an error....            
            foreach (DataRow row in receiptItems1.ItemTable.Rows)
            {
                if ((bool)row["SELECTED"])
                {
                    returnedLineNumbers.AddLast(Convert.ToInt16(row["LINEID"].ToString()));
                }
            }

            // Check if any items are selected...
            if (returnedLineNumbers.Count == 0)
            {
                using (frmMessage msgDlg = new frmMessage(2660, MessageBoxButtons.OK, MessageBoxIcon.Information))  // No items are selected
                {
                    POSFormsManager.ShowPOSForm(msgDlg);
                }
                return;
            }

            if (SupportedCountryRegion.RU == Functions.CountryRegion)
            {
                // Check if any non-returnable items are selected...
                bool showWarning = false;
                foreach (DataRow row in receiptItems1.ItemTable.Rows)
                {
                    if ((bool)row["SELECTED"] && (bool)row["PROHIBITRETURN_RU"])
                    {
                        showWarning = true;
                        break;
                    }
                }

                // Show warning message... 
                if (showWarning)
                {
                    using (frmMessage msgDlg = new frmMessage(106003, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))  // Some of selected items are non-returnable. Continue?
                    {
                        POSFormsManager.ShowPOSForm(msgDlg);
                        if (msgDlg.DialogResult == System.Windows.Forms.DialogResult.No)
                        {
                            returnedLineNumbers.Clear();
                            return;
                        }
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSelectLine_Click(object sender, EventArgs e)
        {
            receiptItems1.ToggleSelect();

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            receiptItems1.SelectAll();
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            receiptItems1.Deselect_All();
        }

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
            return new ReturnTransactionConfirmation
            {
                ReturnedLineNumbers = this.ReturnedLineNumbers,
                Confirmed = this.DialogResult == DialogResult.OK
            } as TResults;
        }

        #endregion
    }
}