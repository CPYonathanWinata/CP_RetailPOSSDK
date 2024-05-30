/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class RedeemLoyaltyPointsForm
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

                ViewModel.UnHookPeripherals();
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblHeading = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.padLoyaltyCardNumber = new LSRetailPosis.POSProcesses.WinControls.StringPad();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnGet = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblLoyaltyCardBalanceTitle = new System.Windows.Forms.Label();
            this.padLoyaltyDiscountAmount = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.lblLoyaltyCardBalance = new System.Windows.Forms.Label();
            this.lblMaxLoyaltyDiscountAmountTitle = new System.Windows.Forms.Label();
            this.lblMaxLoyaltyDiscountAmount = new System.Windows.Forms.Label();
            this.lblAmountAvailableForDiscountTitle = new System.Windows.Forms.Label();
            this.lblAmountAvailableForDiscount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelControl1.AutoSize = true;
            this.panelControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Location = new System.Drawing.Point(217, 194);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(3);
            this.panelControl1.Size = new System.Drawing.Size(6, 6);
            this.panelControl1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel6, 3);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(30, 692);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(964, 65);
            this.tableLayoutPanel6.TabIndex = 9;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Location = new System.Drawing.Point(351, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 1;
            this.btnOk.Tag = "BtnExtraLong";
            this.btnOk.Text = "OK";
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(486, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Tag = "BtnExtraLong";
            this.btnCancel.Text = "Cancel";
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(276, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(471, 95);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Tag = "";
            this.lblHeading.Text = "Redeem loyalty points";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.padLoyaltyCardNumber, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnGet, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardBalanceTitle, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.padLoyaltyDiscountAmount, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardBalance, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblMaxLoyaltyDiscountAmountTitle, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblMaxLoyaltyDiscountAmount, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblAmountAvailableForDiscountTitle, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblAmountAvailableForDiscount, 1, 3);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(150, 253);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(723, 320);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // padLoyaltyCardNumber
            // 
            this.padLoyaltyCardNumber.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.padLoyaltyCardNumber.Appearance.Options.UseFont = true;
            this.padLoyaltyCardNumber.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.padLoyaltyCardNumber.DataBindings.Add(new System.Windows.Forms.Binding("EnteredValue", this.bindingSource, "CardNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.padLoyaltyCardNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.padLoyaltyCardNumber.EnteredValue = "";
            this.padLoyaltyCardNumber.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.StringPadEntryTypes.AlphaNumeric;
            this.padLoyaltyCardNumber.Location = new System.Drawing.Point(3, 3);
            this.padLoyaltyCardNumber.MaskChar = "";
            this.padLoyaltyCardNumber.MaskInterval = 0;
            this.padLoyaltyCardNumber.MaxNumberOfDigits = 80;
            this.padLoyaltyCardNumber.Name = "padLoyaltyCardNumber";
            this.padLoyaltyCardNumber.NegativeMode = false;
            this.padLoyaltyCardNumber.NumberOfDecimals = 0;
            this.padLoyaltyCardNumber.ShortcutKeysActive = false;
            this.padLoyaltyCardNumber.Size = new System.Drawing.Size(248, 62);
            this.padLoyaltyCardNumber.TabIndex = 0;
            this.padLoyaltyCardNumber.TimerEnabled = true;
            this.padLoyaltyCardNumber.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.StringPad.enterbuttonDelegate(this.padLoyaltyCardNumber_EnterButtonPressed);
            this.padLoyaltyCardNumber.CardSwept += new LSRetailPosis.POSProcesses.WinControls.StringPad.cardSwipedDelegate(this.padLoyaltyCardNumber_CardSwept);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.RedeemLoyaltyPointsViewModel);
            // 
            // btnGet
            // 
            this.btnGet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGet.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGet.Appearance.Options.UseFont = true;
            this.btnGet.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.search;
            this.btnGet.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnGet.Location = new System.Drawing.Point(260, 30);
            this.btnGet.Margin = new System.Windows.Forms.Padding(6);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(57, 32);
            this.btnGet.TabIndex = 1;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // lblLoyaltyCardBalanceTitle
            // 
            this.lblLoyaltyCardBalanceTitle.AutoSize = true;
            this.lblLoyaltyCardBalanceTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardBalanceTitle.Location = new System.Drawing.Point(5, 71);
            this.lblLoyaltyCardBalanceTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardBalanceTitle.Name = "lblLoyaltyCardBalanceTitle";
            this.lblLoyaltyCardBalanceTitle.Size = new System.Drawing.Size(131, 25);
            this.lblLoyaltyCardBalanceTitle.TabIndex = 3;
            this.lblLoyaltyCardBalanceTitle.Text = "Point balance:";
            // 
            // padLoyaltyDiscountAmount
            // 
            this.padLoyaltyDiscountAmount.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.padLoyaltyDiscountAmount.Appearance.Options.UseFont = true;
            this.padLoyaltyDiscountAmount.AutoSize = true;
            this.padLoyaltyDiscountAmount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.padLoyaltyDiscountAmount.CurrencyCode = null;
            this.padLoyaltyDiscountAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.padLoyaltyDiscountAmount.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.padLoyaltyDiscountAmount.EnteredValue = "";
            this.padLoyaltyDiscountAmount.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Price;
            this.padLoyaltyDiscountAmount.Location = new System.Drawing.Point(472, 3);
            this.padLoyaltyDiscountAmount.MaskChar = "";
            this.padLoyaltyDiscountAmount.MaskInterval = 0;
            this.padLoyaltyDiscountAmount.MaxNumberOfDigits = 80;
            this.padLoyaltyDiscountAmount.MinimumSize = new System.Drawing.Size(248, 314);
            this.padLoyaltyDiscountAmount.Name = "padLoyaltyDiscountAmount";
            this.padLoyaltyDiscountAmount.NegativeMode = false;
            this.padLoyaltyDiscountAmount.NoOfTries = 0;
            this.padLoyaltyDiscountAmount.NumberOfDecimals = 2;
            this.padLoyaltyDiscountAmount.PromptText = null;
            this.tableLayoutPanel4.SetRowSpan(this.padLoyaltyDiscountAmount, 4);
            this.padLoyaltyDiscountAmount.ShortcutKeysActive = false;
            this.padLoyaltyDiscountAmount.Size = new System.Drawing.Size(248, 314);
            this.padLoyaltyDiscountAmount.TabIndex = 2;
            this.padLoyaltyDiscountAmount.TimerEnabled = true;
            this.padLoyaltyDiscountAmount.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.padLoyaltyDiscountAmount_EnterButtonPressed);
            // 
            // lblLoyaltyCardBalance
            // 
            this.lblLoyaltyCardBalance.AutoSize = true;
            this.lblLoyaltyCardBalance.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "PointsBalance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblLoyaltyCardBalance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardBalance.Location = new System.Drawing.Point(259, 71);
            this.lblLoyaltyCardBalance.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardBalance.Name = "lblLoyaltyCardBalance";
            this.lblLoyaltyCardBalance.Size = new System.Drawing.Size(139, 25);
            this.lblLoyaltyCardBalance.TabIndex = 4;
            this.lblLoyaltyCardBalance.Text = "(Point balance)";
            // 
            // lblMaxLoyaltyDiscountAmountTitle
            // 
            this.lblMaxLoyaltyDiscountAmountTitle.AutoSize = true;
            this.lblMaxLoyaltyDiscountAmountTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblMaxLoyaltyDiscountAmountTitle.Location = new System.Drawing.Point(5, 102);
            this.lblMaxLoyaltyDiscountAmountTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblMaxLoyaltyDiscountAmountTitle.Name = "lblMaxLoyaltyDiscountAmountTitle";
            this.lblMaxLoyaltyDiscountAmountTitle.Size = new System.Drawing.Size(199, 25);
            this.lblMaxLoyaltyDiscountAmountTitle.TabIndex = 5;
            this.lblMaxLoyaltyDiscountAmountTitle.Text = "Max discount amount:";
            // 
            // lblMaxLoyaltyDiscountAmount
            // 
            this.lblMaxLoyaltyDiscountAmount.AutoSize = true;
            this.lblMaxLoyaltyDiscountAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "MaxDiscountAmount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblMaxLoyaltyDiscountAmount.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblMaxLoyaltyDiscountAmount.Location = new System.Drawing.Point(259, 102);
            this.lblMaxLoyaltyDiscountAmount.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblMaxLoyaltyDiscountAmount.Name = "lblMaxLoyaltyDiscountAmount";
            this.lblMaxLoyaltyDiscountAmount.Size = new System.Drawing.Size(207, 25);
            this.lblMaxLoyaltyDiscountAmount.TabIndex = 6;
            this.lblMaxLoyaltyDiscountAmount.Text = "(Max discount amount)";
            // 
            // lblTotalAmountDueTitle
            // 
            this.lblAmountAvailableForDiscountTitle.AutoSize = true;
            this.lblAmountAvailableForDiscountTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblAmountAvailableForDiscountTitle.Location = new System.Drawing.Point(5, 133);
            this.lblAmountAvailableForDiscountTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblAmountAvailableForDiscountTitle.Name = "lblAmountAvailableForDiscountTitle";
            this.lblAmountAvailableForDiscountTitle.Size = new System.Drawing.Size(165, 25);
            this.lblAmountAvailableForDiscountTitle.TabIndex = 7;
            this.lblAmountAvailableForDiscountTitle.Text = "Amount available for discount:";
            // 
            // lblTotalAmountDue
            // 
            this.lblAmountAvailableForDiscount.AutoSize = true;
            this.lblAmountAvailableForDiscount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "AmountAvailableForDiscount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblAmountAvailableForDiscount.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblAmountAvailableForDiscount.Location = new System.Drawing.Point(259, 133);
            this.lblAmountAvailableForDiscount.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblAmountAvailableForDiscount.Name = "lblAmountAvailableForDiscount";
            this.lblAmountAvailableForDiscount.Size = new System.Drawing.Size(173, 25);
            this.lblAmountAvailableForDiscount.TabIndex = 8;
            this.lblAmountAvailableForDiscount.Text = "(Amount available for discount)";
            // 
            // RedeemLoyaltyPointsForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "RedeemLoyaltyPointsForm";
            this.Text = "RedeemLoyaltyPointsForm";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblLoyaltyCardBalanceTitle;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnGet;
        private LSRetailPosis.POSProcesses.WinControls.StringPad padLoyaltyCardNumber;
        private System.Windows.Forms.Label lblLoyaltyCardBalance;
        private LSRetailPosis.POSProcesses.WinControls.NumPad padLoyaltyDiscountAmount;
        private System.Windows.Forms.Label lblAmountAvailableForDiscount;
        private System.Windows.Forms.Label lblMaxLoyaltyDiscountAmount;
        private System.Windows.Forms.Label lblAmountAvailableForDiscountTitle;
        private System.Windows.Forms.Label lblMaxLoyaltyDiscountAmountTitle;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}