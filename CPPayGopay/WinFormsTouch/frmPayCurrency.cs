/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.Common;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System.Data;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Summary description for frmCurrency.
    /// </summary>
    [Export("PayCurrencyView", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class frmPayCurrency : frmTouchBase, IInteractionView
    {
        private PanelControl panelBase;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numCurrNumpad;

        private TableLayoutPanel tableLayoutPanel1;
        private Label layAmount;
        private TableLayoutPanel tableLayoutPanelCurrency;
        private PanelControl panelCurrency;
        private PanelControl panelAmount;
        private TableLayoutPanel tableLayoutPanelAmount;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPaymentAmount;
        private FlowLayoutPanel flowLayoutPanel;
        private Label labelAmountDue;
        private Label labelAmountDueValue;
        private Tender tenderInfo;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private TableLayoutPanel tableLayoutPanel2;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private TableLayoutPanel tableLayoutPanel3;
        private LSRetailPosis.POSProcesses.WinControls.AmountViewer amtCurrAmounts;
        private Label labelCurrencyOptions;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnViewAll;
        private IContainer components;
        private PayCurrencyViewModel viewModel;

        #region Properties

        public decimal ExchangeRate
        {
            get
            {
                return viewModel.ExchangeRate;
            }
        }

        public string CurrentCurrencyCode
        {
            get
            {
                return viewModel.SelectedCurrency;
            }
        }

        public decimal RegisteredAmount
        {
            get
            {
                return viewModel.CurrencyAmountTendered;
            }
        }

        #endregion

        protected frmPayCurrency()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        [ImportingConstructor]
        public frmPayCurrency(PayCurrencyConfirmation payCurrencyConfirmation)
            : this()
        {
            if (payCurrencyConfirmation == null)
            {
                throw new ArgumentNullException("payCurrencyConfirmation");
            }
            this.tenderInfo = (Tender)payCurrencyConfirmation.TenderInfo;

            viewModel = new PayCurrencyViewModel(payCurrencyConfirmation.BalanceAmount, payCurrencyConfirmation.ProposedPaymentAmount);
            viewModel.PropertyChanged += new PropertyChangedEventHandler(OnViewModel_PropertyChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmPayCurrency are reserved at 1420 - 1439
                // In use now are ID's 1420 - 1425
                //
                btnOk.Text = ApplicationLocalizer.Language.Translate(1201); //OK
                btnCancel.Text = ApplicationLocalizer.Language.Translate(1420); //Cancel
                this.Text = ApplicationLocalizer.Language.Translate(1421); //Currency payment
                layAmount.Text = ApplicationLocalizer.Language.Translate(1422); //Payment amount            
                labelCurrencyOptions.Text = ApplicationLocalizer.Language.Translate(1427);//Currency options
                labelAmountDue.Text = ApplicationLocalizer.Language.Translate(1428);//Amount due
                btnViewAll.Text = ApplicationLocalizer.Language.Translate(1429);//View All
                UpdateDisplayAmounts();

                UpdateControlState(false);
            }

            base.OnLoad(e);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.panelBase = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelAmountDue = new System.Windows.Forms.Label();
            this.labelAmountDueValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelAmount = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanelAmount = new System.Windows.Forms.TableLayoutPanel();
            this.btnPaymentAmount = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.layAmount = new System.Windows.Forms.Label();
            this.panelCurrency = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanelCurrency = new System.Windows.Forms.TableLayoutPanel();
            this.amtCurrAmounts = new LSRetailPosis.POSProcesses.WinControls.AmountViewer();
            this.btnViewAll = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelCurrencyOptions = new System.Windows.Forms.Label();
            this.numCurrNumpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).BeginInit();
            this.tableLayoutPanelAmount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelCurrency)).BeginInit();
            this.tableLayoutPanelCurrency.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.tableLayoutPanel3);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Margin = new System.Windows.Forms.Padding(0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Size = new System.Drawing.Size(983, 760);
            this.panelBase.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(979, 756);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(66, 680);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(847, 65);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(427, 4);
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
            this.btnOk.Location = new System.Drawing.Point(292, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Controls.Add(this.labelAmountDue);
            this.flowLayoutPanel.Controls.Add(this.labelAmountDueValue);
            this.flowLayoutPanel.Location = new System.Drawing.Point(225, 43);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(529, 95);
            this.flowLayoutPanel.TabIndex = 0;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelAmount, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelCurrency, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numCurrNumpad, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(87, 163);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 483);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelAmount
            // 
            this.panelAmount.Controls.Add(this.tableLayoutPanelAmount);
            this.panelAmount.Enabled = false;
            this.panelAmount.Location = new System.Drawing.Point(557, 30);
            this.panelAmount.Margin = new System.Windows.Forms.Padding(0);
            this.panelAmount.Name = "panelAmount";
            this.panelAmount.Size = new System.Drawing.Size(200, 133);
            this.panelAmount.TabIndex = 3;
            // 
            // tableLayoutPanelAmount
            // 
            this.tableLayoutPanelAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanelAmount.ColumnCount = 1;
            this.tableLayoutPanelAmount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAmount.Controls.Add(this.btnPaymentAmount, 0, 1);
            this.tableLayoutPanelAmount.Controls.Add(this.layAmount, 0, 0);
            this.tableLayoutPanelAmount.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanelAmount.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.tableLayoutPanelAmount.Name = "tableLayoutPanelAmount";
            this.tableLayoutPanelAmount.RowCount = 1;
            this.tableLayoutPanelAmount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAmount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAmount.Size = new System.Drawing.Size(196, 129);
            this.tableLayoutPanelAmount.TabIndex = 0;
            // 
            // btnPaymentAmount
            // 
            this.btnPaymentAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPaymentAmount.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPaymentAmount.Appearance.Options.UseFont = true;
            this.btnPaymentAmount.Location = new System.Drawing.Point(33, 47);
            this.btnPaymentAmount.Name = "btnPaymentAmount";
            this.btnPaymentAmount.Size = new System.Drawing.Size(130, 60);
            this.btnPaymentAmount.TabIndex = 7;
            this.btnPaymentAmount.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // layAmount
            // 
            this.layAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.layAmount.AutoSize = true;
            this.layAmount.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.layAmount.Location = new System.Drawing.Point(20, 0);
            this.layAmount.Margin = new System.Windows.Forms.Padding(0);
            this.layAmount.Name = "layAmount";
            this.layAmount.Size = new System.Drawing.Size(155, 25);
            this.layAmount.TabIndex = 6;
            this.layAmount.Text = "Payment amount";
            this.layAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCurrency
            // 
            this.panelCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCurrency.Controls.Add(this.tableLayoutPanelCurrency);
            this.panelCurrency.Location = new System.Drawing.Point(47, 30);
            this.panelCurrency.Margin = new System.Windows.Forms.Padding(0);
            this.panelCurrency.Name = "panelCurrency";
            this.panelCurrency.Size = new System.Drawing.Size(200, 453);
            this.panelCurrency.TabIndex = 1;
            // 
            // tableLayoutPanelCurrency
            // 
            this.tableLayoutPanelCurrency.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanelCurrency.ColumnCount = 1;
            this.tableLayoutPanelCurrency.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCurrency.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCurrency.Controls.Add(this.amtCurrAmounts, 0, 2);
            this.tableLayoutPanelCurrency.Controls.Add(this.btnViewAll, 0, 3);
            this.tableLayoutPanelCurrency.Controls.Add(this.labelCurrencyOptions, 0, 1);
            this.tableLayoutPanelCurrency.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanelCurrency.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.tableLayoutPanelCurrency.Name = "tableLayoutPanelCurrency";
            this.tableLayoutPanelCurrency.RowCount = 4;
            this.tableLayoutPanelCurrency.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCurrency.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCurrency.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCurrency.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCurrency.Size = new System.Drawing.Size(196, 441);
            this.tableLayoutPanelCurrency.TabIndex = 1;
            // 
            // amtCurrAmounts
            // 
            this.amtCurrAmounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.amtCurrAmounts.CurrencyRate = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCurrAmounts.ForeignCurrencyMode = true;
            this.amtCurrAmounts.HighestOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCurrAmounts.IncludeRefAmount = true;
            this.amtCurrAmounts.LocalCurrencyCode = "";
            this.amtCurrAmounts.Location = new System.Drawing.Point(3, 24);
            this.amtCurrAmounts.LowesetOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCurrAmounts.Name = "amtCurrAmounts";
            this.amtCurrAmounts.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.amtCurrAmounts.ShowBorder = false;
            this.amtCurrAmounts.Size = new System.Drawing.Size(190, 349);
            this.amtCurrAmounts.TabIndex = 2;
            this.amtCurrAmounts.UsedCurrencyCode = "";
            this.amtCurrAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.HeigherAndLowerAmounts;
            this.amtCurrAmounts.AmountChanged += new LSRetailPosis.POSProcesses.WinControls.AmountViewer.OutputChanged(this.amtCurrAmounts_AmountChanged);
            // 
            // btnViewAll
            // 
            this.btnViewAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewAll.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewAll.Appearance.Options.UseFont = true;
            this.btnViewAll.Location = new System.Drawing.Point(4, 380);
            this.btnViewAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(188, 57);
            this.btnViewAll.TabIndex = 2;
            this.btnViewAll.Text = "View all";
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // labelCurrencyOptions
            // 
            this.labelCurrencyOptions.AutoSize = true;
            this.labelCurrencyOptions.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.labelCurrencyOptions.Location = new System.Drawing.Point(0, 0);
            this.labelCurrencyOptions.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrencyOptions.Name = "labelCurrencyOptions";
            this.labelCurrencyOptions.Size = new System.Drawing.Size(135, 21);
            this.labelCurrencyOptions.TabIndex = 3;
            this.labelCurrencyOptions.Text = "Currency options";
            // 
            // numCurrNumpad
            // 
            this.numCurrNumpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numCurrNumpad.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCurrNumpad.Appearance.Options.UseFont = true;
            this.numCurrNumpad.AutoSize = true;
            this.numCurrNumpad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numCurrNumpad.CurrencyCode = null;
            this.numCurrNumpad.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCurrNumpad.EnteredValue = "";
            this.numCurrNumpad.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Price;
            this.numCurrNumpad.Location = new System.Drawing.Point(252, 30);
            this.numCurrNumpad.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.numCurrNumpad.MaskChar = "";
            this.numCurrNumpad.MaskInterval = 0;
            this.numCurrNumpad.MaxNumberOfDigits = 9;
            this.numCurrNumpad.MinimumSize = new System.Drawing.Size(300, 330);
            this.numCurrNumpad.Name = "numCurrNumpad";
            this.numCurrNumpad.NegativeMode = false;
            this.numCurrNumpad.NoOfTries = 0;
            this.numCurrNumpad.NumberOfDecimals = 2;
            this.numCurrNumpad.PromptText = "";
            this.numCurrNumpad.ShortcutKeysActive = false;
            this.numCurrNumpad.Size = new System.Drawing.Size(300, 330);
            this.numCurrNumpad.TabIndex = 2;
            this.numCurrNumpad.TimerEnabled = true;
            this.numCurrNumpad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numCurrNumpad_EnterButtonPressed);
            // 
            // frmPayCurrency
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(983, 760);
            this.Controls.Add(this.panelBase);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmPayCurrency";
            this.Controls.SetChildIndex(this.panelBase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).EndInit();
            this.tableLayoutPanelAmount.ResumeLayout(false);
            this.tableLayoutPanelAmount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelCurrency)).EndInit();
            this.tableLayoutPanelCurrency.ResumeLayout(false);
            this.tableLayoutPanelCurrency.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Listens for changes on the view model
        /// </summary>        
        private void OnViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrencyAmountDue":
                    UpdateDisplayAmounts();
                    break;

                case "SelectedCurrency":
                    UpdateDisplayAmounts();

                    UpdateControlState(true);
                    break;

                case "CurrencyAmountTendered":
                    ValidateAmount(viewModel.CurrencyAmountTendered);
                    break;

                default:
                    break;
            }
        }

        private void UpdateDisplayAmounts()
        {
            if (viewModel.SelectedCurrency == ApplicationSettings.Terminal.StoreCurrency)
            {
                btnPaymentAmount.Text = PosApplication.Instance.Services.Rounding.Round(viewModel.CurrencyAmountDue, viewModel.SelectedCurrency, true);
            }
            else
            {
                btnPaymentAmount.Text = PosApplication.Instance.Services.Rounding.RoundAmount(
                    viewModel.CurrencyAmountDue, ApplicationSettings.Terminal.StoreId, tenderInfo.TenderID, viewModel.SelectedCurrency, true);
            }
            labelAmountDueValue.Text = string.Format(ApplicationLocalizer.Language.Translate(1426), viewModel.SelectedCurrency, btnPaymentAmount.Text);
        }

        private void UpdateControlState(bool currencySelected)
        {
            if (currencySelected)
            {
                panelAmount.Enabled = true;
                numCurrNumpad.NegativeMode = viewModel.Balance < 0;
                numCurrNumpad.Enabled = true;
                numCurrNumpad.EntryType = NumpadEntryTypes.Price;
                numCurrNumpad.CurrencyCode = viewModel.SelectedCurrency;
                numCurrNumpad.PromptText = string.Format(ApplicationLocalizer.Language.Translate(1424), viewModel.SelectedCurrency); //Enter Amount in

                decimal minimumCurrencyUnit = PosApplication.Instance.Services.Rounding.GetMinimumCurrencyUnit(viewModel.SelectedCurrency);
                numCurrNumpad.NumberOfDecimals = PosApplication.Instance.BusinessLogic.Utility.GetNumberOfDecimals(minimumCurrencyUnit);

                numCurrNumpad.ClearValue();
                numCurrNumpad.Focus();
            }
            else
            {
                panelAmount.Enabled = false;
                amtCurrAmounts.LocalCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
                amtCurrAmounts.UsedCurrencyCode = string.Empty;
                amtCurrAmounts.SoldLocalAmount = viewModel.ProposedPaymentAmount;
                amtCurrAmounts.SetButtons();

                numCurrNumpad.NegativeMode = false;
                numCurrNumpad.PromptText = ApplicationLocalizer.Language.Translate(1423); //Choose currency code
                numCurrNumpad.Enabled = false;
            }
        }

        /// <summary>
        /// Handler for number pad
        /// </summary>
        private void numCurrNumpad_EnterButtonPressed()
        {
            // User entered manual amount   
            if (numCurrNumpad.EnteredDecimalValue == PosApplication.Instance.Services.Rounding.RoundAmount(
                  viewModel.CurrencyAmountDue, ApplicationSettings.Terminal.StoreId, tenderInfo.TenderID))
            {
                viewModel.CurrencyAmountTendered = viewModel.CurrencyAmountDue;
            }
            else
            {
                viewModel.CurrencyAmountTendered = numCurrNumpad.EnteredDecimalValue;
            }
        }

        /// <summary>
        /// Handler for currency selector
        /// </summary>        
        private void amtCurrAmounts_AmountChanged(decimal outAmount, string currCode)
        {
            viewModel.SelectedCurrency = currCode;
        }

        /// <summary>
        /// Handler for full amount button
        /// </summary>        
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            viewModel.CurrencyAmountTendered = viewModel.CurrencyAmountDue;
        }

        private void ValidateAmount(decimal amount)
        {
            TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo,
                PosApplication.Instance.Services.Rounding.Round(amount), true, viewModel.Balance);

            if (string.IsNullOrEmpty(tenderReq.ErrorText))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop)) //The amount entered is higher than the maximum amount allowed
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }

                numCurrNumpad.TryAgain();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.numCurrNumpad_EnterButtonPressed();
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
            return new PayCurrencyConfirmation
            {
                RegisteredAmount = this.RegisteredAmount,
                ExchangeRate = this.ExchangeRate,
                CurrentCurrencyCode = this.CurrentCurrencyCode,
                Confirmed = this.DialogResult == DialogResult.OK
            } as TResults;
        }

        #endregion


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "rows and columns are added to the table, cannot dispose")]
        private void btnViewAll_Click(object sender, EventArgs e)
        {
            //Open Generic Search

            const string Currency = "Currency";           

            using (DataTable dataTable = new DataTable())
            {               

                DataColumn column = new DataColumn(Currency);
                column.Caption = ApplicationLocalizer.Language.Translate(51125);//Currency
                dataTable.Columns.Add(column);

                DataRow selectedRow = null;
                foreach (string currencyType in amtCurrAmounts.AmountStrings)
                {
                    DataRow row = dataTable.NewRow();                 
                    row[Currency] = currencyType;
                    dataTable.Rows.Add(row);

                }
                DialogResult result = PosApplication.Instance.Services.Dialog.GenericSearch(dataTable, ref selectedRow, ApplicationLocalizer.Language.Translate(51125));//Add a param for currency from loaclizer as 3rd param(Get latest dialog project)

                if (result == DialogResult.OK)
                {
                    string selectedCurrency = (string)selectedRow[Currency]; 
                    amtCurrAmounts_AmountChanged(viewModel.Balance, selectedCurrency);
                }
            }
        }
    }
}