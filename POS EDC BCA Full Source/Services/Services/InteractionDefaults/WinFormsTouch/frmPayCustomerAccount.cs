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
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.Common;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Summary description for frmPayCustomerAccount.
    /// </summary>
    [Export("PayCustomerAccountForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class frmPayCustomerAccount : frmTouchBase, IInteractionView
    {
        #region Class variables
        private decimal registeredAmount;
        private decimal amount;
        private string customerID;
        private decimal balanceAmount;        
        private PosTransaction posTransaction;
        private int controlIndex;
        private decimal? proposedPaymentAmount;
        #endregion

        #region Form Components

        private Tender tenderInfo;
        private PanelControl panelControl1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelAmountDue;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label labelAmountDueValue;
        private PanelControl panelCustomer;
        private TableLayoutPanel tableLayoutPanel2;
        private Label labelAmount;
        private Label labelAccountInfo;
        private Label labelCustomerId;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCustomerSearch;
        private Label labelCustomerIdValue;
        private Label labelCustomerName;
        private Label labelCustomerNameValue;
        private Label labelAmountValue;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numPad1;
        private PanelControl panelControl2;
        private LSRetailPosis.POSProcesses.WinControls.AmountViewer amtCustAmounts;
        private Label labelPaymentAmount;
        private TableLayoutPanel tableLayoutPanel3;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private TableLayoutPanel tableLayoutPanel4;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion
        
        

        #region Properties

        public string CustomerID
        {
            get
            {
                return customerID;
            }
            set
            {
                customerID = (value == null) ? string.Empty : value;
            }
        }

        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                try
                {
                    amount = value;
                    labelAmountValue.Text = PosApplication.Instance.Services.Rounding.Round(amount, false);
                }
                catch (Exception)
                {
                }
            }
        }

        public decimal RegisteredAmount
        {
            get
            {
                return registeredAmount;
            }
            set
            {
                if (registeredAmount == value)
                {
                    return;
                }

                registeredAmount = value;
            }
        }

        public PosTransaction PosTransaction
        {
            set
            {
                posTransaction = value;
                RetailTransaction retailPosTransaction = (RetailTransaction)posTransaction;

                if (!retailPosTransaction.InvoicedCustomer.IsEmptyCustomer())
                {
                    this.CustomerID = retailPosTransaction.InvoicedCustomer.CustomerId;
                }
                else if (!retailPosTransaction.Customer.IsEmptyCustomer())
                {
                    this.CustomerID = retailPosTransaction.Customer.CustomerId;
                }

                SetCustomerDisplayText(retailPosTransaction);
            }
        }

        #endregion

        #region Constructor and Destructor

        protected frmPayCustomerAccount()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        [ImportingConstructor]
        public frmPayCustomerAccount(PayCustomerAccountConfirmation payCustomerAccountConfirmation)
            : this()
        {
            if (payCustomerAccountConfirmation == null)
            {
                throw new ArgumentNullException("payCustomerAccountConfirmation");
            }
            this.tenderInfo = (Tender)payCustomerAccountConfirmation.TenderInfo;
            this.proposedPaymentAmount = payCustomerAccountConfirmation.ProposedPaymentAmount;
            amtCustAmounts.SoldLocalAmount = 
                payCustomerAccountConfirmation.ProposedPaymentAmount != null 
                ? (decimal)payCustomerAccountConfirmation.ProposedPaymentAmount 
                : payCustomerAccountConfirmation.BalanceAmount;
            this.balanceAmount = payCustomerAccountConfirmation.BalanceAmount;
            this.PosTransaction = (PosTransaction)payCustomerAccountConfirmation.PosTransaction;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                amtCustAmounts.LocalCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
                amtCustAmounts.UsedCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;

                if (tenderInfo.OverTenderAllowed)
                {
                    amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.HigherAmounts;
                }
                else
                {
                    amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;
                }

                amtCustAmounts.HighestOptionAmount = tenderInfo.MaximumAmountAllowed;
                amtCustAmounts.LowesetOptionAmount = tenderInfo.MinimumAmountAllowed;
                amtCustAmounts.SetButtons();

                amount = 0;
                registeredAmount = 0;
                controlIndex = 1;
                SetButtonStatus();

                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmPayCustomerAccount are reserved at 1440 - 1459
                // In use now are ID's 1440 - 1451
                //                

                btnCancel.Text = ApplicationLocalizer.Language.Translate(1440); //Cancel
                btnOk.Text = ApplicationLocalizer.Language.Translate(1201);//Ok
                labelAmount.Text = ApplicationLocalizer.Language.Translate(1441); //Amount
                labelPaymentAmount.Text = ApplicationLocalizer.Language.Translate(1442); //Payment amount

                this.Text = ApplicationLocalizer.Language.Translate(1445); //Customer account payment
                labelAmountDue.Text = ApplicationLocalizer.Language.Translate(1450); //Total amount due:
                labelAmountDueValue.Text = PosApplication.Instance.Services.Rounding.Round(this.balanceAmount, true);
                btnCustomerSearch.Text = ApplicationLocalizer.Language.Translate(1447); //Change customer
                labelCustomerId.Text = ApplicationLocalizer.Language.Translate(1448); //Customer id
                labelCustomerName.Text = ApplicationLocalizer.Language.Translate(1449); //Customer Name
                labelAccountInfo.Text = ApplicationLocalizer.Language.Translate(1451); //Account information

                numPad1.SetEnteredValueFocus();

                if (labelAmountValue.Text.Length != 0)
                {
                    controlIndex = 2;
                }
                else if ((labelCustomerIdValue.Text.Length != 0) && (labelCustomerNameValue.Text.Length != 0))
                {
                    controlIndex = 1;
                }
                else
                {
                    controlIndex = 0;
                }

                SetButtonStatus();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelCustomer = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAmount = new System.Windows.Forms.Label();
            this.labelAccountInfo = new System.Windows.Forms.Label();
            this.labelCustomerId = new System.Windows.Forms.Label();
            this.btnCustomerSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelCustomerIdValue = new System.Windows.Forms.Label();
            this.labelCustomerName = new System.Windows.Forms.Label();
            this.labelCustomerNameValue = new System.Windows.Forms.Label();
            this.labelAmountValue = new System.Windows.Forms.Label();
            this.numPad1 = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.amtCustAmounts = new LSRetailPosis.POSProcesses.WinControls.AmountViewer();
            this.labelPaymentAmount = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelAmountDue = new System.Windows.Forms.Label();
            this.labelAmountDueValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelCustomer)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1024, 768);
            this.panelControl1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(30, 687);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(960, 66);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(484, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(349, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 2;
            this.btnOk.Tag = "";
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelCustomer, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numPad1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 3, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(108, 194);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 429);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelCustomer
            // 
            this.panelCustomer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelCustomer.Controls.Add(this.tableLayoutPanel2);
            this.panelCustomer.Location = new System.Drawing.Point(44, 30);
            this.panelCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.panelCustomer.Name = "panelCustomer";
            this.panelCustomer.Size = new System.Drawing.Size(200, 328);
            this.panelCustomer.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelAmount, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.labelAccountInfo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelCustomerId, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnCustomerSearch, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.labelCustomerIdValue, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelCustomerName, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.labelCustomerNameValue, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelAmountValue, 0, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(196, 324);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelAmount
            // 
            this.labelAmount.AutoSize = true;
            this.labelAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmount.Location = new System.Drawing.Point(3, 179);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(61, 17);
            this.labelAmount.TabIndex = 5;
            this.labelAmount.Text = "Amount:";
            // 
            // labelAccountInfo
            // 
            this.labelAccountInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAccountInfo.AutoSize = true;
            this.labelAccountInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountInfo.Location = new System.Drawing.Point(31, 5);
            this.labelAccountInfo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 10);
            this.labelAccountInfo.Name = "labelAccountInfo";
            this.labelAccountInfo.Size = new System.Drawing.Size(133, 17);
            this.labelAccountInfo.TabIndex = 0;
            this.labelAccountInfo.Text = "Account information";
            // 
            // labelCustomerId
            // 
            this.labelCustomerId.AutoSize = true;
            this.labelCustomerId.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerId.Location = new System.Drawing.Point(3, 32);
            this.labelCustomerId.Name = "labelCustomerId";
            this.labelCustomerId.Size = new System.Drawing.Size(87, 17);
            this.labelCustomerId.TabIndex = 1;
            this.labelCustomerId.Text = "Customer ID:";
            // 
            // btnCustomerSearch
            // 
            this.btnCustomerSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCustomerSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerSearch.Appearance.Options.UseFont = true;
            this.btnCustomerSearch.Location = new System.Drawing.Point(31, 263);
            this.btnCustomerSearch.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.Size = new System.Drawing.Size(133, 50);
            this.btnCustomerSearch.TabIndex = 7;
            this.btnCustomerSearch.Text = "Change customer";
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // labelCustomerIdValue
            // 
            this.labelCustomerIdValue.AutoSize = true;
            this.labelCustomerIdValue.Location = new System.Drawing.Point(3, 49);
            this.labelCustomerIdValue.Name = "labelCustomerIdValue";
            this.labelCustomerIdValue.Size = new System.Drawing.Size(0, 21);
            this.labelCustomerIdValue.TabIndex = 2;
            // 
            // labelCustomerName
            // 
            this.labelCustomerName.AutoSize = true;
            this.labelCustomerName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerName.Location = new System.Drawing.Point(3, 105);
            this.labelCustomerName.Name = "labelCustomerName";
            this.labelCustomerName.Size = new System.Drawing.Size(108, 17);
            this.labelCustomerName.TabIndex = 3;
            this.labelCustomerName.Text = "Customer name:";
            // 
            // labelCustomerNameValue
            // 
            this.labelCustomerNameValue.AutoSize = true;
            this.labelCustomerNameValue.Location = new System.Drawing.Point(3, 122);
            this.labelCustomerNameValue.Name = "labelCustomerNameValue";
            this.labelCustomerNameValue.Size = new System.Drawing.Size(0, 21);
            this.labelCustomerNameValue.TabIndex = 4;
            // 
            // labelAmountValue
            // 
            this.labelAmountValue.AutoSize = true;
            this.labelAmountValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelAmountValue.Location = new System.Drawing.Point(3, 196);
            this.labelAmountValue.Name = "labelAmountValue";
            this.labelAmountValue.Size = new System.Drawing.Size(0, 21);
            this.labelAmountValue.TabIndex = 6;
            // 
            // numPad1
            // 
            this.numPad1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numPad1.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPad1.Appearance.Options.UseFont = true;
            this.numPad1.AutoSize = true;
            this.numPad1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numPad1.CurrencyCode = null;
            this.numPad1.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPad1.EnteredValue = "";
            this.numPad1.Location = new System.Drawing.Point(249, 30);
            this.numPad1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.numPad1.MaskChar = "";
            this.numPad1.MaskInterval = 0;
            this.numPad1.MaxNumberOfDigits = 20;
            this.numPad1.MinimumSize = new System.Drawing.Size(300, 330);
            this.numPad1.Name = "numPad1";
            this.numPad1.NegativeMode = false;
            this.numPad1.NoOfTries = 0;
            this.numPad1.NumberOfDecimals = 2;
            this.numPad1.PromptText = "Bar code";
            this.numPad1.ShortcutKeysActive = false;
            this.numPad1.Size = new System.Drawing.Size(300, 330);
            this.numPad1.TabIndex = 2;
            this.numPad1.TimerEnabled = true;
            this.numPad1.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad1_EnterButtonPressed);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.amtCustAmounts);
            this.panelControl2.Controls.Add(this.labelPaymentAmount);
            this.panelControl2.Location = new System.Drawing.Point(559, 30);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(200, 330);
            this.panelControl2.TabIndex = 3;
            // 
            // amtCustAmounts
            // 
            this.amtCustAmounts.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amtCustAmounts.Appearance.Options.UseFont = true;
            this.amtCustAmounts.Appearance.Options.UseForeColor = true;
            this.amtCustAmounts.CurrencyRate = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCustAmounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.amtCustAmounts.ForeignCurrencyMode = false;
            this.amtCustAmounts.HighestOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCustAmounts.IncludeRefAmount = true;
            this.amtCustAmounts.LocalCurrencyCode = "";
            this.amtCustAmounts.Location = new System.Drawing.Point(2, 29);
            this.amtCustAmounts.LowesetOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCustAmounts.Name = "amtCustAmounts";
            this.amtCustAmounts.OptionsLimit = 5;
            this.amtCustAmounts.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.amtCustAmounts.ShowBorder = false;
            this.amtCustAmounts.Size = new System.Drawing.Size(196, 299);
            this.amtCustAmounts.TabIndex = 1;
            this.amtCustAmounts.UsedCurrencyCode = "";
            this.amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;
            this.amtCustAmounts.AmountChanged += new LSRetailPosis.POSProcesses.WinControls.AmountViewer.OutputChanged(this.amtCustAmounts_AmountChanged);
            // 
            // labelPaymentAmount
            // 
            this.labelPaymentAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.labelPaymentAmount.Location = new System.Drawing.Point(2, 2);
            this.labelPaymentAmount.Name = "labelPaymentAmount";
            this.labelPaymentAmount.Size = new System.Drawing.Size(196, 27);
            this.labelPaymentAmount.TabIndex = 0;
            this.labelPaymentAmount.Text = "Payment amount";
            this.labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.labelAmountDue);
            this.flowLayoutPanel1.Controls.Add(this.labelAmountDueValue);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(245, 43);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(529, 95);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // labelAmountDue
            // 
            this.labelAmountDue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAmountDue.AutoSize = true;
            this.labelAmountDue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelAmountDue.Location = new System.Drawing.Point(0, 0);
            this.labelAmountDue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAmountDue.Name = "labelAmountDue";
            this.labelAmountDue.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelAmountDue.Size = new System.Drawing.Size(390, 95);
            this.labelAmountDue.TabIndex = 0;
            this.labelAmountDue.Tag = "";
            this.labelAmountDue.Text = "Total amount due:";
            // 
            // labelAmountDueValue
            // 
            this.labelAmountDueValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAmountDueValue.AutoSize = true;
            this.labelAmountDueValue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelAmountDueValue.Location = new System.Drawing.Point(390, 0);
            this.labelAmountDueValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAmountDueValue.Name = "labelAmountDueValue";
            this.labelAmountDueValue.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelAmountDueValue.Size = new System.Drawing.Size(139, 95);
            this.labelAmountDueValue.TabIndex = 1;
            this.labelAmountDueValue.Text = "$0.00";
            // 
            // frmPayCustomerAccount
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmPayCustomerAccount";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelCustomer)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Events

        private void numPad1_EnterButtonPressed()
        {
            TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, numPad1.EnteredDecimalValue, true, balanceAmount);
            if (string.IsNullOrEmpty(tenderReq.ErrorText))
            {
                controlIndex = 1;
                Action(numPad1.EnteredValue);
            }
            else
            {
                using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    // The amount entered is higher than the maximum amount allowed
                    POSFormsManager.ShowPOSForm(dialog);
                    numPad1.TryAgain();
                }
            }
        }

        private void amtCustAmounts_AmountChanged(decimal outAmount, string currCode)
        {
            TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, outAmount, true, balanceAmount);
            if (string.IsNullOrEmpty(tenderReq.ErrorText))
            {
                controlIndex = 1;
                Action(outAmount.ToString());
            }
            else
            {
                using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    // The amount entered is higher than the maximum amount allowed
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            CustomerSearch custSearch = new CustomerSearch();
            custSearch.OperationID = PosisOperations.CustomerSearch;
            custSearch.POSTransaction = this.posTransaction;
            custSearch.RunOperation();

            RetailTransaction retailPosTransaction = (RetailTransaction)posTransaction;

            if (!(retailPosTransaction.Customer.IsEmptyCustomer())
                && retailPosTransaction.Customer.Blocked == BlockedEnum.No)
            {
                //Get the customer information
                if (!retailPosTransaction.InvoicedCustomer.IsEmptyCustomer())
                {
                    this.CustomerID = retailPosTransaction.InvoicedCustomer.CustomerId;
                }
                else if (!retailPosTransaction.Customer.IsEmptyCustomer())
                {
                    this.CustomerID = retailPosTransaction.Customer.CustomerId;
                }

                SetCustomerDisplayText(retailPosTransaction);

                //The customer might have a discount so the lines need to be calculated again
                retailPosTransaction.CalcTotals();

                //Change the Amountviewer if needed
                if (retailPosTransaction.TransSalePmtDiff != balanceAmount)
                {
                    balanceAmount = retailPosTransaction.TransSalePmtDiff;
                    if (proposedPaymentAmount == null || Math.Abs(balanceAmount) < Math.Abs((decimal)proposedPaymentAmount))
                    {
                        amtCustAmounts.SoldLocalAmount = balanceAmount;
                        amtCustAmounts.SetButtons();
                    }

                    //If an amount has already been selected and it's higher than the new balance of the transaction (with the new discount)
                    //then we need to adjust it to reflect the new balance.
                    if (this.Amount > balanceAmount)
                    {
                        this.Amount = balanceAmount;
                    }

                }

                controlIndex = 2;
                Action(string.Empty);
            }
            /*The control itself displays information about weather a customer is found
             *else
            {
                frmMessage dialog = new frmMessage(1446, MessageBoxButtons.OK, MessageBoxIcon.Information);
                POSFormsManager.ShowPOSForm(dialog);
            }*/
        }
        
        private void SetCustomerDisplayText(RetailTransaction transaction)
        {
            LSRetailPosis.Transaction.Customer cust;

            if (transaction.InvoicedCustomer != null 
                && !string.IsNullOrWhiteSpace(transaction.InvoicedCustomer.CustomerId))
            {
                cust = transaction.InvoicedCustomer;
            }
            else
            {
                cust = transaction.Customer;
            }

            labelCustomerIdValue.Text = cust.CustomerId;
            labelCustomerNameValue.Text = cust.Name;
        }

        #endregion

        #region Private Procedures

        private void resetAllColors()
        {
            Color color = Color.Transparent;
            labelCustomerIdValue.BackColor = color;
            labelCustomerNameValue.BackColor = color;
            labelAmountValue.BackColor = color;
        }

        private void SetButtonStatus()
        {
            Color selectColor = Color.White;

            resetAllColors();

            switch (controlIndex)
            {
                case 1:
                    labelAmountValue.BackColor = selectColor;
                    labelAmountValue.Text = string.Empty;
                    numPad1.Enabled = true;
                    numPad1.EntryType = NumpadEntryTypes.Price;
                    numPad1.PromptText = ApplicationLocalizer.Language.Translate(1443); //Enter Amount in
                    numPad1.NegativeMode = this.balanceAmount < 0;
                    break;

                case 2:
                    labelCustomerIdValue.BackColor = selectColor;
                    labelCustomerIdValue.Text = string.Empty;
                    labelCustomerNameValue.BackColor = selectColor;
                    labelCustomerNameValue.Text = string.Empty;
                    amtCustAmounts.SoldLocalAmount = proposedPaymentAmount != null ? (decimal)proposedPaymentAmount : balanceAmount;
                    amtCustAmounts.SetButtons();
                    numPad1.NegativeMode = false;
                    numPad1.PromptText = ApplicationLocalizer.Language.Translate(1444); //Choose currency code
                    numPad1.Enabled = false;
                    break;
            }
        }

        private bool SetValue(string value)
        {
            System.Diagnostics.Debug.Assert(value != null, "value may not be null.");

            Color selectColor = Color.White;
            bool valueOK = false;
            try
            {
                switch (controlIndex)
                {
                    case 1:
                        if (string.IsNullOrEmpty(value))
                        {
                            value = "0";
                        }

                        Amount = Convert.ToDecimal(value);
                        valueOK = Math.Abs(Amount) > 0;
                        break;
                    case 2:
                        valueOK = !string.IsNullOrWhiteSpace(labelCustomerIdValue.Text)
                            && !string.IsNullOrWhiteSpace(labelCustomerNameValue.Text);
                        break;
                }
            }
            catch (FormatException)
            {
            }

            return valueOK;
        }

        private void Action(string value)
        {
            // Check if the amount is valid
            if (SetValue(value))
            {
                // if an amount and customer id have both been entered then close the dlg
                if (!string.IsNullOrWhiteSpace(labelAmountValue.Text) && !string.IsNullOrWhiteSpace(labelCustomerIdValue.Text) && !string.IsNullOrWhiteSpace(labelCustomerNameValue.Text))
                {
                    registeredAmount  = amount;                    
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    numPad1.TryAgain();
                }
            }
            else
            {
                numPad1.TryAgain();
            }

            controlIndex++;
            if (controlIndex > 2)
            {
                controlIndex = 1;
            }

            numPad1.Clear();
            SetButtonStatus();
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.numPad1_EnterButtonPressed();
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
            return new PayCustomerAccountConfirmation
            {
                Confirmed        = this.DialogResult == DialogResult.OK,
                RegisteredAmount = this.RegisteredAmount,
                CustomerId       = this.CustomerID,
            } as TResults;
        }

        #endregion
    }
}