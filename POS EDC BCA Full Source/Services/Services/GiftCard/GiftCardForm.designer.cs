/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;

namespace Microsoft.Dynamics.Retail.Pos.GiftCard
{
    partial class GiftCardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                GiftCard.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(ProcessScannedItem);
                GiftCard.InternalApplication.Services.Peripherals.MSR.MSRMessageEvent -= new MSRMessageEventHandler(ProcessSwipedCard);
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelOkCancel = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.payGiftCardViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelHeading = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.numCurrNumpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.panelAmount = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanelAmount = new System.Windows.Forms.TableLayoutPanel();
            this.btnPaymentAmount = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelPaymentAmount = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelAmount = new System.Windows.Forms.Label();
            this.labelCardNumber = new System.Windows.Forms.Label();
            this.textBoxAmount = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.labelCardStatusTitle = new System.Windows.Forms.Label();
            this.labelCardStatus = new System.Windows.Forms.Label();
            this.labelCardActiveFromTitle = new System.Windows.Forms.Label();
            this.labelCardExpirationDate = new System.Windows.Forms.Label();
            this.labelCardExpirationDateTitle = new System.Windows.Forms.Label();
            this.labelReloadableTitle = new System.Windows.Forms.Label();
            this.labelCardReloadble = new System.Windows.Forms.Label();
            this.labelCardRedemption = new System.Windows.Forms.Label();
            this.labelCheckBalanceTitle = new System.Windows.Forms.Label();
            this.labelCardActiveFrom = new System.Windows.Forms.Label();
            this.labelCheckBalanceAmount = new System.Windows.Forms.Label();
            this.labelRedemptionTitle = new System.Windows.Forms.Label();
            this.labelCardMaxBalanceTitle = new System.Windows.Forms.Label();
            this.labelCardMinReloadAmountTitle = new System.Windows.Forms.Label();
            this.labelCardMaxBalance = new System.Windows.Forms.Label();
            this.labelCardMinReloadAmount = new System.Windows.Forms.Label();
            this.textBoxCardNumber = new DevExpress.XtraEditors.TextEdit();
            this.btnCheckBalance = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelOkCancel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.payGiftCardViewModelBindingSource)).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).BeginInit();
            this.panelAmount.SuspendLayout();
            this.tableLayoutPanelAmount.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxAmount.Properties)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCardNumber.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1024, 768);
            this.panelControl1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelOkCancel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanelOkCancel
            // 
            this.tableLayoutPanelOkCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanelOkCancel.ColumnCount = 2;
            this.tableLayoutPanelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOkCancel.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanelOkCancel.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanelOkCancel.Location = new System.Drawing.Point(33, 685);
            this.tableLayoutPanelOkCancel.Name = "tableLayoutPanelOkCancel";
            this.tableLayoutPanelOkCancel.RowCount = 1;
            this.tableLayoutPanelOkCancel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOkCancel.Size = new System.Drawing.Size(954, 65);
            this.tableLayoutPanelOkCancel.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(481, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.payGiftCardViewModelBindingSource, "Validated", true));
            this.btnOk.Location = new System.Drawing.Point(346, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.OnBtnOk_Click);
            // 
            // payGiftCardViewModelBindingSource
            // 
            this.payGiftCardViewModelBindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.GiftCard.GiftCardController);
            this.payGiftCardViewModelBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.OnPayGiftCardViewModelBindingSource_BindingComplete);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Controls.Add(this.labelHeading);
            this.flowLayoutPanel.Location = new System.Drawing.Point(315, 43);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.flowLayoutPanel.Size = new System.Drawing.Size(390, 95);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // labelHeading
            // 
            this.labelHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelHeading.AutoSize = true;
            this.labelHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelHeading.Location = new System.Drawing.Point(0, 0);
            this.labelHeading.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeading.Name = "labelHeading";
            this.labelHeading.Size = new System.Drawing.Size(390, 65);
            this.labelHeading.TabIndex = 0;
            this.labelHeading.Tag = "";
            this.labelHeading.Text = "Total amount due:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.numCurrNumpad, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelAmount, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(52, 177);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(916, 469);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // numCurrNumpad
            // 
            this.numCurrNumpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numCurrNumpad.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCurrNumpad.Appearance.ForeColor = System.Drawing.Color.Transparent;
            this.numCurrNumpad.Appearance.Options.UseFont = true;
            this.numCurrNumpad.Appearance.Options.UseForeColor = true;
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
            this.numCurrNumpad.Location = new System.Drawing.Point(374, 3);
            this.numCurrNumpad.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
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
            this.numCurrNumpad.TabIndex = 1;
            this.numCurrNumpad.TimerEnabled = true;
            this.numCurrNumpad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.OnNumpad_EnterButtonPressed);
            this.numCurrNumpad.CardSwept += new LSRetailPosis.POSProcesses.WinControls.NumPad.cardSwipedDelegate(this.ProcessSwipedCard);
            // 
            // panelAmount
            // 
            this.panelAmount.Controls.Add(this.tableLayoutPanelAmount);
            this.panelAmount.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "MaxTransactionAmountAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.panelAmount.Location = new System.Drawing.Point(697, 37);
            this.panelAmount.Margin = new System.Windows.Forms.Padding(3, 37, 3, 3);
            this.panelAmount.Name = "panelAmount";
            this.panelAmount.Size = new System.Drawing.Size(200, 133);
            this.panelAmount.TabIndex = 2;
            // 
            // tableLayoutPanelAmount
            // 
            this.tableLayoutPanelAmount.ColumnCount = 1;
            this.tableLayoutPanelAmount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAmount.Controls.Add(this.btnPaymentAmount, 0, 1);
            this.tableLayoutPanelAmount.Controls.Add(this.labelPaymentAmount, 0, 0);
            this.tableLayoutPanelAmount.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanelAmount.Name = "tableLayoutPanelAmount";
            this.tableLayoutPanelAmount.RowCount = 2;
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
            this.btnPaymentAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "MaxTransactionAmountAllowed", true));
            this.btnPaymentAmount.Location = new System.Drawing.Point(33, 50);
            this.btnPaymentAmount.Name = "btnPaymentAmount";
            this.btnPaymentAmount.Size = new System.Drawing.Size(130, 60);
            this.btnPaymentAmount.TabIndex = 1;
            this.btnPaymentAmount.Click += new System.EventHandler(this.OnPaymentAmount_Click);
            // 
            // labelPaymentAmount
            // 
            this.labelPaymentAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelPaymentAmount.AutoSize = true;
            this.labelPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.labelPaymentAmount.Location = new System.Drawing.Point(20, 3);
            this.labelPaymentAmount.Margin = new System.Windows.Forms.Padding(3);
            this.labelPaymentAmount.Name = "labelPaymentAmount";
            this.labelPaymentAmount.Size = new System.Drawing.Size(155, 25);
            this.labelPaymentAmount.TabIndex = 0;
            this.labelPaymentAmount.Text = "Payment amount";
            this.labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelAmount, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCardNumber, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxAmount, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel3, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.textBoxCardNumber, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.btnCheckBalance, 0, 4);
            this.tableLayoutPanel.Location = new System.Drawing.Point(16, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(338, 471);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelAmount
            // 
            this.labelAmount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelAmount.AutoSize = true;
            this.labelAmount.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "Amount", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmount.Location = new System.Drawing.Point(3, 19);
            this.labelAmount.Margin = new System.Windows.Forms.Padding(3, 19, 3, 0);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(58, 17);
            this.labelAmount.TabIndex = 0;
            this.labelAmount.Text = "Amount";
            // 
            // labelCardNumber
            // 
            this.labelCardNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCardNumber.AutoSize = true;
            this.labelCardNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCardNumber.Location = new System.Drawing.Point(3, 80);
            this.labelCardNumber.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelCardNumber.Name = "labelCardNumber";
            this.labelCardNumber.Size = new System.Drawing.Size(111, 17);
            this.labelCardNumber.TabIndex = 2;
            this.labelCardNumber.Text = "Gift card number";
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "Amount", true));
            this.textBoxAmount.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "Amount", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.textBoxAmount.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.payGiftCardViewModelBindingSource, "CardNumber", true));
            this.textBoxAmount.Location = new System.Drawing.Point(3, 39);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxAmount.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAmount.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxAmount.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxAmount.Properties.Appearance.Options.UseFont = true;
            this.textBoxAmount.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxAmount.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxAmount.Size = new System.Drawing.Size(332, 28);
            this.textBoxAmount.TabIndex = 1;
            this.textBoxAmount.Enter += new System.EventHandler(this.OnAmountTextBox_Enter);
            this.textBoxAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextbox_KeyPress);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.labelCardStatusTitle, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelCardStatus, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelCardActiveFromTitle, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.labelCardExpirationDate, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.labelCardExpirationDateTitle, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.labelReloadableTitle, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.labelCardReloadble, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.labelCardRedemption, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.labelCheckBalanceTitle, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelCardActiveFrom, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.labelCheckBalanceAmount, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelRedemptionTitle, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.labelCardMaxBalanceTitle, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.labelCardMinReloadAmountTitle, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.labelCardMaxBalance, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.labelCardMinReloadAmount, 1, 6);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 215);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 9;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(332, 253);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // labelCardStatusTitle
            // 
            this.labelCardStatusTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardStatusTitle.AutoSize = true;
            this.labelCardStatusTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "Balance", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardStatusTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelCardStatusTitle.Location = new System.Drawing.Point(3, 45);
            this.labelCardStatusTitle.Name = "labelCardStatusTitle";
            this.labelCardStatusTitle.Size = new System.Drawing.Size(46, 17);
            this.labelCardStatusTitle.TabIndex = 2;
            this.labelCardStatusTitle.Text = "Status";
            // 
            // labelCardStatus
            // 
            this.labelCardStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardStatus.AutoSize = true;
            this.labelCardStatus.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyGiftCardStatus", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyGiftCardStatus", true));
            this.labelCardStatus.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelCardStatus.Location = new System.Drawing.Point(169, 37);
            this.labelCardStatus.Name = "labelCardStatus";
            this.labelCardStatus.Size = new System.Drawing.Size(108, 25);
            this.labelCardStatus.TabIndex = 3;
            this.labelCardStatus.Text = "(CardStatus)";
            // 
            // labelCardActiveFromTitle
            // 
            this.labelCardActiveFromTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardActiveFromTitle.AutoSize = true;
            this.labelCardActiveFromTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyActiveFrom", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardActiveFromTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelCardActiveFromTitle.Location = new System.Drawing.Point(3, 78);
            this.labelCardActiveFromTitle.Name = "labelCardActiveFromTitle";
            this.labelCardActiveFromTitle.Size = new System.Drawing.Size(78, 17);
            this.labelCardActiveFromTitle.TabIndex = 4;
            this.labelCardActiveFromTitle.Text = "Active from";
            // 
            // labelCardExpirationDate
            // 
            this.labelCardExpirationDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardExpirationDate.AutoSize = true;
            this.labelCardExpirationDate.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyExpirationDate", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardExpirationDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyExpirationDate", true));
            this.labelCardExpirationDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelCardExpirationDate.Location = new System.Drawing.Point(169, 95);
            this.labelCardExpirationDate.Name = "labelCardExpirationDate";
            this.labelCardExpirationDate.Size = new System.Drawing.Size(110, 20);
            this.labelCardExpirationDate.TabIndex = 7;
            this.labelCardExpirationDate.Text = "(Valid To Date)";
            // 
            // labelCardExpirationDateTitle
            // 
            this.labelCardExpirationDateTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardExpirationDateTitle.AutoSize = true;
            this.labelCardExpirationDateTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyExpirationDate", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardExpirationDateTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelCardExpirationDateTitle.Location = new System.Drawing.Point(3, 98);
            this.labelCardExpirationDateTitle.Name = "labelCardExpirationDateTitle";
            this.labelCardExpirationDateTitle.Size = new System.Drawing.Size(100, 17);
            this.labelCardExpirationDateTitle.TabIndex = 6;
            this.labelCardExpirationDateTitle.Text = "Expiration date";
            // 
            // labelReloadableTitle
            // 
            this.labelReloadableTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelReloadableTitle.AutoSize = true;
            this.labelReloadableTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyNonReloadable", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelReloadableTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelReloadableTitle.Location = new System.Drawing.Point(3, 131);
            this.labelReloadableTitle.Name = "labelReloadableTitle";
            this.labelReloadableTitle.Size = new System.Drawing.Size(74, 17);
            this.labelReloadableTitle.TabIndex = 8;
            this.labelReloadableTitle.Text = "Reloadable";
            // 
            // labelCardReloadble
            // 
            this.labelCardReloadble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardReloadble.AutoSize = true;
            this.labelCardReloadble.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyNonReloadable", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardReloadble.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyNonReloadable", true));
            this.labelCardReloadble.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelCardReloadble.Location = new System.Drawing.Point(169, 128);
            this.labelCardReloadble.Name = "labelCardReloadble";
            this.labelCardReloadble.Size = new System.Drawing.Size(119, 20);
            this.labelCardReloadble.TabIndex = 9;
            this.labelCardReloadble.Text = "(ReloadbleType)";
            // 
            // labelCardRedemption
            // 
            this.labelCardRedemption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardRedemption.AutoSize = true;
            this.labelCardRedemption.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyOneTimeRedemption", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardRedemption.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyOneTimeRedemption", true));
            this.labelCardRedemption.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelCardRedemption.Location = new System.Drawing.Point(169, 148);
            this.labelCardRedemption.Name = "labelCardRedemption";
            this.labelCardRedemption.Size = new System.Drawing.Size(133, 20);
            this.labelCardRedemption.TabIndex = 11;
            this.labelCardRedemption.Text = "(RedemptionType)";
            // 
            // labelCheckBalanceTitle
            // 
            this.labelCheckBalanceTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCheckBalanceTitle.AutoSize = true;
            this.labelCheckBalanceTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "Balance", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCheckBalanceTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCheckBalanceTitle.Location = new System.Drawing.Point(3, 20);
            this.labelCheckBalanceTitle.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.labelCheckBalanceTitle.Name = "labelCheckBalanceTitle";
            this.labelCheckBalanceTitle.Size = new System.Drawing.Size(112, 17);
            this.labelCheckBalanceTitle.TabIndex = 0;
            this.labelCheckBalanceTitle.Text = "Available balance";
            // 
            // labelCardActiveFrom
            // 
            this.labelCardActiveFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardActiveFrom.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyActiveFrom", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardActiveFrom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyActiveFrom", true));
            this.labelCardActiveFrom.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelCardActiveFrom.Location = new System.Drawing.Point(169, 75);
            this.labelCardActiveFrom.Name = "labelCardActiveFrom";
            this.labelCardActiveFrom.Size = new System.Drawing.Size(160, 20);
            this.labelCardActiveFrom.TabIndex = 5;
            this.labelCardActiveFrom.Text = "31/12/2014 15:15:45";
            // 
            // labelCheckBalanceAmount
            // 
            this.labelCheckBalanceAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCheckBalanceAmount.AutoSize = true;
            this.labelCheckBalanceAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "Balance", true));
            this.labelCheckBalanceAmount.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "Balance", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCheckBalanceAmount.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelCheckBalanceAmount.Location = new System.Drawing.Point(169, 12);
            this.labelCheckBalanceAmount.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelCheckBalanceAmount.Name = "labelCheckBalanceAmount";
            this.labelCheckBalanceAmount.Size = new System.Drawing.Size(148, 25);
            this.labelCheckBalanceAmount.TabIndex = 1;
            this.labelCheckBalanceAmount.Text = "(Balance amount)";
            // 
            // labelRedemptionTitle
            // 
            this.labelRedemptionTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelRedemptionTitle.AutoSize = true;
            this.labelRedemptionTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyOneTimeRedemption", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelRedemptionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelRedemptionTitle.Location = new System.Drawing.Point(3, 151);
            this.labelRedemptionTitle.Name = "labelRedemptionTitle";
            this.labelRedemptionTitle.Size = new System.Drawing.Size(82, 17);
            this.labelRedemptionTitle.TabIndex = 10;
            this.labelRedemptionTitle.Text = "Redemption";
            // 
            // labelCardMaxBalanceTitle
            // 
            this.labelCardMaxBalanceTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardMaxBalanceTitle.AutoSize = true;
            this.labelCardMaxBalanceTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyMaxBalanceAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardMaxBalanceTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelCardMaxBalanceTitle.Location = new System.Drawing.Point(3, 204);
            this.labelCardMaxBalanceTitle.Name = "labelCardMaxBalanceTitle";
            this.labelCardMaxBalanceTitle.Size = new System.Drawing.Size(119, 17);
            this.labelCardMaxBalanceTitle.TabIndex = 14;
            this.labelCardMaxBalanceTitle.Text = "Maximum balance";
            // 
            // labelCardMinReloadAmountTitle
            // 
            this.labelCardMinReloadAmountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardMinReloadAmountTitle.AutoSize = true;
            this.labelCardMinReloadAmountTitle.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyMinReloadAmountAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardMinReloadAmountTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelCardMinReloadAmountTitle.Location = new System.Drawing.Point(3, 184);
            this.labelCardMinReloadAmountTitle.Name = "labelCardMinReloadAmountTitle";
            this.labelCardMinReloadAmountTitle.Size = new System.Drawing.Size(160, 17);
            this.labelCardMinReloadAmountTitle.TabIndex = 12;
            this.labelCardMinReloadAmountTitle.Text = "Minimum reload amount";
            // 
            // labelCardMaxBalance
            // 
            this.labelCardMaxBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardMaxBalance.AutoSize = true;
            this.labelCardMaxBalance.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyMaxBalanceAllowed", true));
            this.labelCardMaxBalance.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyMaxBalanceAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardMaxBalance.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelCardMaxBalance.Location = new System.Drawing.Point(169, 201);
            this.labelCardMaxBalance.Name = "labelCardMaxBalance";
            this.labelCardMaxBalance.Size = new System.Drawing.Size(99, 20);
            this.labelCardMaxBalance.TabIndex = 15;
            this.labelCardMaxBalance.Text = "(MaxBalance)";
            // 
            // labelCardMinReloadAmount
            // 
            this.labelCardMinReloadAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCardMinReloadAmount.AutoSize = true;
            this.labelCardMinReloadAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "PolicyMinReloadAmountAllowed", true));
            this.labelCardMinReloadAmount.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "PolicyMinReloadAmountAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelCardMinReloadAmount.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelCardMinReloadAmount.Location = new System.Drawing.Point(169, 181);
            this.labelCardMinReloadAmount.Name = "labelCardMinReloadAmount";
            this.labelCardMinReloadAmount.Size = new System.Drawing.Size(144, 20);
            this.labelCardMinReloadAmount.TabIndex = 13;
            this.labelCardMinReloadAmount.Text = "(MinReloadAmount)";
            // 
            // textBoxCardNumber
            // 
            this.textBoxCardNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCardNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payGiftCardViewModelBindingSource, "CardNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCardNumber.Location = new System.Drawing.Point(3, 100);
            this.textBoxCardNumber.Name = "textBoxCardNumber";
            this.textBoxCardNumber.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxCardNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCardNumber.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxCardNumber.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxCardNumber.Properties.Appearance.Options.UseFont = true;
            this.textBoxCardNumber.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxCardNumber.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxCardNumber.Properties.MaxLength = 19;
            this.textBoxCardNumber.Size = new System.Drawing.Size(332, 28);
            this.textBoxCardNumber.TabIndex = 3;
            this.textBoxCardNumber.Enter += new System.EventHandler(this.OnCardNumberTextBox_Enter);
            this.textBoxCardNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextbox_KeyPress);
            // 
            // btnCheckBalance
            // 
            this.btnCheckBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckBalance.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckBalance.Appearance.Options.UseFont = true;
            this.btnCheckBalance.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.payGiftCardViewModelBindingSource, "Balance", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnCheckBalance.Location = new System.Drawing.Point(4, 151);
            this.btnCheckBalance.Margin = new System.Windows.Forms.Padding(4, 20, 4, 4);
            this.btnCheckBalance.Name = "btnCheckBalance";
            this.btnCheckBalance.Size = new System.Drawing.Size(129, 57);
            this.btnCheckBalance.TabIndex = 4;
            this.btnCheckBalance.Text = "Check balance";
            this.btnCheckBalance.Click += new System.EventHandler(this.OnBtnCheckBalance_Click);
            // 
            // GiftCardForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "GiftCardForm";
            this.Text = "Pay card";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanelOkCancel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.payGiftCardViewModelBindingSource)).EndInit();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).EndInit();
            this.panelAmount.ResumeLayout(false);
            this.tableLayoutPanelAmount.ResumeLayout(false);
            this.tableLayoutPanelAmount.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxAmount.Properties)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCardNumber.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DevExpress.XtraEditors.PanelControl panelAmount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAmount;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPaymentAmount;
        private System.Windows.Forms.Label labelPaymentAmount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Label labelHeading;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numCurrNumpad;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.Label labelCardNumber;
        private DevExpress.XtraEditors.TextEdit textBoxAmount;
        private DevExpress.XtraEditors.TextEdit textBoxCardNumber;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private System.Windows.Forms.BindingSource payGiftCardViewModelBindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOkCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private const int swipeSuppressionTimeout = 1000;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCheckBalance;
        private System.Windows.Forms.Label labelCheckBalanceTitle;
        private System.Windows.Forms.Label labelCheckBalanceAmount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelCardStatusTitle;
        private System.Windows.Forms.Label labelCardStatus;
        private System.Windows.Forms.Label labelCardActiveFrom;
        private System.Windows.Forms.Label labelCardActiveFromTitle;
        private System.Windows.Forms.Label labelCardExpirationDate;
        private System.Windows.Forms.Label labelCardExpirationDateTitle;
        private System.Windows.Forms.Label labelReloadableTitle;
        private System.Windows.Forms.Label labelCardReloadble;
        private System.Windows.Forms.Label labelCardRedemption;
        private System.Windows.Forms.Label labelCardMaxBalanceTitle;
        private System.Windows.Forms.Label labelCardMinReloadAmountTitle;
        private System.Windows.Forms.Label labelCardMinReloadAmount;
        private System.Windows.Forms.Label labelCardMaxBalance;
        private System.Windows.Forms.Label labelRedemptionTitle; // 1 second

    }
}
