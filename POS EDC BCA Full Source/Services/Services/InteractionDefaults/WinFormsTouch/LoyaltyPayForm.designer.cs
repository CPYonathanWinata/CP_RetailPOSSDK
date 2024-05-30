/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using DevExpress.XtraEditors;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
    using System.ComponentModel.Composition;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using LSRetailPosis.Transaction;


    partial class LoyaltyPayForm
    {
        #region Form Components
        private PanelControl panelControl1;
        private TableLayoutPanel tableLayoutPanel;
        private PanelControl panelAmount;
        private TableLayoutPanel tableLayoutPanelAmount;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPaymentAmount;
        private Label labelPaymentAmount;
        private FlowLayoutPanel flowLayoutPanel;
        private Label labelAmountDue;
        private Label labelAmountDueValue;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numCurrNumpad;
        private Label labelAmount;
        private Label labelCardNumber;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private TableLayoutPanel tableLayoutPanelOkCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private TableLayoutPanel tableLayoutPanel1;
        private IContainer components;
        private DevExpress.XtraEditors.TextEdit textBoxAmount;
        private DevExpress.XtraEditors.TextEdit textBoxCardNumber;
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelOkCancel = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelAmountDue = new System.Windows.Forms.Label();
            this.labelAmountDueValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelAmount = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanelAmount = new System.Windows.Forms.TableLayoutPanel();
            this.btnPaymentAmount = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelPaymentAmount = new System.Windows.Forms.Label();
            this.numCurrNumpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.labelAmount = new System.Windows.Forms.Label();
            this.labelCardNumber = new System.Windows.Forms.Label();
            this.textBoxAmount = new DevExpress.XtraEditors.TextEdit();
            this.textBoxCardNumber = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelOkCancel.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).BeginInit();
            this.tableLayoutPanelAmount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxAmount.Properties)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel, 0, 2);
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
            this.tableLayoutPanelOkCancel.TabIndex = 0;
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
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(346, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Controls.Add(this.labelAmountDue);
            this.flowLayoutPanel.Controls.Add(this.labelAmountDueValue);
            this.flowLayoutPanel.Location = new System.Drawing.Point(245, 43);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
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
            this.labelAmountDue.Size = new System.Drawing.Size(390, 65);
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
            this.labelAmountDueValue.Size = new System.Drawing.Size(139, 65);
            this.labelAmountDueValue.TabIndex = 9;
            this.labelAmountDueValue.Text = "$0.00";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.panelAmount, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.numCurrNumpad, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelAmount, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCardNumber, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxAmount, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxCardNumber, 1, 3);
            this.tableLayoutPanel.Location = new System.Drawing.Point(52, 200);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(916, 423);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // panelAmount
            // 
            this.panelAmount.Controls.Add(this.tableLayoutPanelAmount);
            this.panelAmount.Location = new System.Drawing.Point(684, 37);
            this.panelAmount.Margin = new System.Windows.Forms.Padding(3, 37, 3, 3);
            this.panelAmount.Name = "panelAmount";
            this.tableLayoutPanel.SetRowSpan(this.panelAmount, 5);
            this.panelAmount.Size = new System.Drawing.Size(200, 133);
            this.panelAmount.TabIndex = 0;
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
            this.btnPaymentAmount.Location = new System.Drawing.Point(33, 50);
            this.btnPaymentAmount.Name = "btnPaymentAmount";
            this.btnPaymentAmount.Size = new System.Drawing.Size(130, 60);
            this.btnPaymentAmount.TabIndex = 5;
            this.btnPaymentAmount.Click += new System.EventHandler(this.btnPaymentAmount_Click);
            // 
            // labelPaymentAmount
            // 
            this.labelPaymentAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelPaymentAmount.AutoSize = true;
            this.labelPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.labelPaymentAmount.Location = new System.Drawing.Point(21, 3);
            this.labelPaymentAmount.Margin = new System.Windows.Forms.Padding(3);
            this.labelPaymentAmount.Name = "labelPaymentAmount";
            this.labelPaymentAmount.Size = new System.Drawing.Size(154, 25);
            this.labelPaymentAmount.TabIndex = 0;
            this.labelPaymentAmount.Text = "Payment amount";
            this.labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.numCurrNumpad.Enabled = false;
            this.numCurrNumpad.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCurrNumpad.EnteredValue = "";
            this.numCurrNumpad.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.None;
            this.numCurrNumpad.Location = new System.Drawing.Point(361, 3);
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
            this.tableLayoutPanel.SetRowSpan(this.numCurrNumpad, 5);
            this.numCurrNumpad.ShortcutKeysActive = false;
            this.numCurrNumpad.Size = new System.Drawing.Size(300, 330);
            this.numCurrNumpad.TabIndex = 0;
            this.numCurrNumpad.TimerEnabled = true;
            this.numCurrNumpad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numCurrNumpad_EnterButtonPressed);
            // 
            // labelAmount
            // 
            this.labelAmount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelAmount.AutoSize = true;
            this.labelAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmount.Location = new System.Drawing.Point(32, 19);
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
            this.labelCardNumber.Location = new System.Drawing.Point(32, 80);
            this.labelCardNumber.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelCardNumber.Name = "labelCardNumber";
            this.labelCardNumber.Size = new System.Drawing.Size(88, 17);
            this.labelCardNumber.TabIndex = 0;
            this.labelCardNumber.Text = "Card number";
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxAmount.Enabled = false;
            this.textBoxAmount.Location = new System.Drawing.Point(32, 39);
            this.textBoxAmount.Name = "textBoxAmount";
            // 
            // 
            // 
            this.textBoxAmount.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxAmount.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAmount.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxAmount.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxAmount.Properties.Appearance.Options.UseFont = true;
            this.textBoxAmount.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxAmount.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxAmount.Size = new System.Drawing.Size(306, 28);
            this.textBoxAmount.TabIndex = 1;
            this.textBoxAmount.Enter += new System.EventHandler(this.AmountTextBox_Enter);
            // 
            // textBoxCardNumber
            // 
            this.textBoxCardNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxCardNumber.Enabled = false;
            this.textBoxCardNumber.Location = new System.Drawing.Point(32, 100);
            this.textBoxCardNumber.Name = "textBoxCardNumber";
            // 
            // 
            // 
            this.textBoxCardNumber.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxCardNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCardNumber.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxCardNumber.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxCardNumber.Properties.Appearance.Options.UseFont = true;
            this.textBoxCardNumber.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxCardNumber.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxCardNumber.Properties.MaxLength = 19;
            this.textBoxCardNumber.Size = new System.Drawing.Size(306, 28);
            this.textBoxCardNumber.TabIndex = 2;
            this.textBoxCardNumber.Enter += new System.EventHandler(this.CardNumberTextBox_Enter);
            // 
            // LoyaltyPayForm
            // 
            this.Appearance.Options.UseFont = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "LoyaltyPayForm";
            this.Text = "Pay card";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanelOkCancel.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).EndInit();
            this.tableLayoutPanelAmount.ResumeLayout(false);
            this.tableLayoutPanelAmount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxAmount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCardNumber.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private StyleController styleController;
    }
}
