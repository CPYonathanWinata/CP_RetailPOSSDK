/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    sealed partial class OrderInformationPage : OrderDetailsPage
    {
        public OrderInformationPage()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                // Create the popup and listen to it being closed
                this.btnEditType.PopupContent = new OrderTypePopup();
                this.btnEditType.DropDown.Closed += new ToolStripDropDownClosedEventHandler(OnEditTypeDropDown_Closed);
            }

            disableElement(); //custom by Yonathan 3/Jan/2023
            base.OnLoad(e);
        }

        public void disableElement() //custom by Yonathan 3/Jan/2023
        {
            this.textBoxChannelOrder.Enabled = false;
            this.textBoxComment.Enabled = true;
            this.textBoxLoyaltyCard.Enabled = false;
            this.textBoxOrderId.Enabled = false;
            //this.textBoxPerson.Enabled = false;
            this.textBoxStatus.Enabled = false;
            this.textBoxType.Enabled = false;
            this.btnEditType.Visible = false;
            //this.btnPersonEdit.Visible = false;
        }

        public override void OnActivate()
        {
            UpdateExpirationVisibility();
            UpdateCommentReadOnlyState();
        }

        private OrderDetailsViewModel ViewModel
        {
            get
            {
                return base.GetViewModel<OrderDetailsViewModel>();
            }
        }

        private void TranslateLabels()
        {
            this.Text               = ApplicationLocalizer.Language.Translate(56328); // "Order information:"
            labelOrderType.Text     = ApplicationLocalizer.Language.Translate(56335); // "Order type:"
            labelStatus.Text        = ApplicationLocalizer.Language.Translate(56336); // "Order status:"
            labelSalesPerson.Text   = ApplicationLocalizer.Language.Translate(56337); // "Sales person:"
            labelExpiration.Text    = ApplicationLocalizer.Language.Translate(56338); // "Expiration date:"
            labelComment.Text       = ApplicationLocalizer.Language.Translate(56339); // "Comments:"
            labelLoyaltyCard.Text   = ApplicationLocalizer.Language.Translate(56406); // "Loyalty card:"
            labelChannelOrder.Text = ApplicationLocalizer.Language.Translate(56413); // "Channel order:"
            btnEditType.Text        = ApplicationLocalizer.Language.Translate(56222); // "Change..."
            btnPersonEdit.Text      = btnEditType.Text;                               // "Change..."
            labelOrderId.Text       = ApplicationLocalizer.Language.Translate(56401); // "Order number:"
        }

        private void OnEditTypeDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                OrderTypePopup popup = this.btnEditType.PopupContent as OrderTypePopup;
                if (popup != null)
                {
                    int orderType = popup.SelectedIndex;

                    if (Enum.IsDefined(typeof(CustomerOrderType), orderType))
                    {
                        this.ViewModel.OrderType = (CustomerOrderType)orderType;
                    }
                    else
                    {
                        throw new InvalidEnumArgumentException(string.Format("{0} is not a valid index for enum CustomerOrderType", orderType));
                    }
                }
            }
        }

        private void OnBtnPersonTaskEdit_Click(object sender, System.EventArgs e)
        {
            const string staffName = "NameOnReceipt";
            const string staffId = "StaffId";

            using (DataTable dataTable = new DataTable())
            {
                DataColumn column = new DataColumn(staffId);
                column.Caption = ApplicationLocalizer.Language.Translate(1509);
                dataTable.Columns.Add(column);

                column = new DataColumn(staffName);
                column.Caption = ApplicationLocalizer.Language.Translate(1505);
                dataTable.Columns.Add(column);

                DataRow selectedRow = null;
                foreach (Employee employee in this.ViewModel.Employees)
                {
                    DataRow row = dataTable.NewRow();
                    row[staffId] = employee.StaffId;
                    row[staffName] = employee.NameOnReceipt;
                    dataTable.Rows.Add(row);

                    if (selectedRow != null && string.Equals(this.ViewModel.SalesTakerId, employee.StaffId, StringComparison.OrdinalIgnoreCase))
                        selectedRow = row;
                }

                DialogResult result = SalesOrder.InternalApplication.Services.Dialog.GenericSearch(dataTable, ref selectedRow, "Sales taker search");

                if (result == DialogResult.OK)
                {
                    this.ViewModel.SalesTakerId = (string)selectedRow[staffId]; // First column is StaffId.
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification="rows and columns are added to the table, cannot dispose")]
        private void OnBtnPersonEdit_Click(object sender, EventArgs e)
        {
            const string staffName = "NameOnReceipt";
            const string staffId = "StaffId";

            using (DataTable dataTable = new DataTable())
            {
                DataColumn column = new DataColumn(staffId);
                column.Caption = ApplicationLocalizer.Language.Translate(1509);
                dataTable.Columns.Add(column);

                column = new DataColumn(staffName);
                column.Caption = ApplicationLocalizer.Language.Translate(1505);
                dataTable.Columns.Add(column);

                DataRow selectedRow = null;
                foreach (Employee employee in this.ViewModel.Employees)
                {
                    DataRow row = dataTable.NewRow();
                    row[staffId]   = employee.StaffId;
                    row[staffName] = employee.NameOnReceipt;
                    dataTable.Rows.Add(row);

                    if (selectedRow != null && string.Equals(this.ViewModel.SalesPersonId, employee.StaffId, StringComparison.OrdinalIgnoreCase))
                        selectedRow = row;
                }

                DialogResult result = SalesOrder.InternalApplication.Services.Dialog.GenericSearch(dataTable, ref selectedRow, ApplicationLocalizer.Language.Translate(56414));

                if (result == DialogResult.OK)
                {
                    this.ViewModel.SalesPersonId = (string)selectedRow[staffId]; // First column is StaffId.
                }
            }
        }

        
        protected override void OnViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "OrderType":
                    UpdateExpirationVisibility();
                    break;

                default:
                    break;
            }
        }

        public override void OnClear()
        {
            this.ViewModel.OrderComment = string.Empty;
            this.ViewModel.SalesPersonId = string.Empty;
            this.ViewModel.OrderExpirationDate = this.ViewModel.MinimumOrderExpirationDate;

            //custom by Yonathan 12/02/2024
            this.ViewModel.SalesTakerId = string.Empty;
        }

        private void UpdateExpirationVisibility()
        {
            // Expiration Date field (and caption) are only visible for Quotes.
            bool expirationVisible = (this.ViewModel.OrderType == CustomerOrderType.Quote);
            this.labelExpiration.Visible = expirationVisible;
            this.dateTimePicker.Visible = expirationVisible;
        }

        private void UpdateCommentReadOnlyState()
        {
            if (this.ViewModel != null)
            {
                this.textBoxComment.Properties.ReadOnly = !this.ViewModel.IsCommentChangeAllowed;
            }
        }

    }
};