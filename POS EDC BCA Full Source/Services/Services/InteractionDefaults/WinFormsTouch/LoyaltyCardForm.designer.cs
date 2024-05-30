/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class LoyaltyCardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltyCardForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblHeading = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoyaltyCardExpiredPoints = new System.Windows.Forms.Label();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblLoyaltyCardUsedPoints = new System.Windows.Forms.Label();
            this.lblLoyaltyCardIssuedPoints = new System.Windows.Forms.Label();
            this.lblLoyaltyCardType = new System.Windows.Forms.Label();
            this.lblLoyaltyCardStatus = new System.Windows.Forms.Label();
            this.lblLoyaltyCardCustomerName = new System.Windows.Forms.Label();
            this.lblLoyaltyCardExpiredPointsTitle = new System.Windows.Forms.Label();
            this.lblLoyaltyCardUsedPointsTitle = new System.Windows.Forms.Label();
            this.lblLoyaltyCardStatusTitle = new System.Windows.Forms.Label();
            this.panelAmount = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoyaltyCardBalanceTitle = new System.Windows.Forms.Label();
            this.btnGet = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.padLoyaltyCardNumber = new LSRetailPosis.POSProcesses.WinControls.StringPad();
            this.lblLoyaltyCardBalance = new System.Windows.Forms.Label();
            this.lblLoyaltyCardCustomerNameTitle = new System.Windows.Forms.Label();
            this.lblLoyaltyCardTypeTitle = new System.Windows.Forms.Label();
            this.lblLoyaltyCardIssuedPointsTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).BeginInit();
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
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            this.lblHeading.Location = new System.Drawing.Point(373, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(278, 95);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Tag = "";
            this.lblHeading.Text = "Loyalty Card";
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
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardExpiredPoints, 2, 8);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardUsedPoints, 2, 7);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardIssuedPoints, 2, 6);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardType, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardStatus, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardCustomerName, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardExpiredPointsTitle, 1, 8);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardUsedPointsTitle, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardStatusTitle, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.panelAmount, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardBalanceTitle, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.btnGet, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.padLoyaltyCardNumber, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardBalance, 2, 5);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardCustomerNameTitle, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardTypeTitle, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblLoyaltyCardIssuedPointsTitle, 1, 6);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(302, 271);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 10;
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
            this.tableLayoutPanel4.Size = new System.Drawing.Size(419, 285);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // lblLoyaltyCardExpiredPoints
            // 
            this.lblLoyaltyCardExpiredPoints.AutoSize = true;
            this.lblLoyaltyCardExpiredPoints.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ExpiredPoints", true));
            this.lblLoyaltyCardExpiredPoints.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardExpiredPoints.Location = new System.Drawing.Point(259, 257);
            this.lblLoyaltyCardExpiredPoints.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardExpiredPoints.Name = "lblLoyaltyCardExpiredPoints";
            this.lblLoyaltyCardExpiredPoints.Size = new System.Drawing.Size(144, 25);
            this.lblLoyaltyCardExpiredPoints.TabIndex = 35;
            this.lblLoyaltyCardExpiredPoints.Text = "(Expired points)";
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.LoyaltyCardFormViewModel);
            // 
            // lblLoyaltyCardUsedPoints
            // 
            this.lblLoyaltyCardUsedPoints.AutoSize = true;
            this.lblLoyaltyCardUsedPoints.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "UsedPoints", true));
            this.lblLoyaltyCardUsedPoints.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardUsedPoints.Location = new System.Drawing.Point(259, 226);
            this.lblLoyaltyCardUsedPoints.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardUsedPoints.Name = "lblLoyaltyCardUsedPoints";
            this.lblLoyaltyCardUsedPoints.Size = new System.Drawing.Size(123, 25);
            this.lblLoyaltyCardUsedPoints.TabIndex = 34;
            this.lblLoyaltyCardUsedPoints.Text = "(Used points)";
            // 
            // lblLoyaltyCardIssuedPoints
            // 
            this.lblLoyaltyCardIssuedPoints.AutoSize = true;
            this.lblLoyaltyCardIssuedPoints.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "IssuedPoints", true));
            this.lblLoyaltyCardIssuedPoints.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardIssuedPoints.Location = new System.Drawing.Point(259, 195);
            this.lblLoyaltyCardIssuedPoints.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardIssuedPoints.Name = "lblLoyaltyCardIssuedPoints";
            this.lblLoyaltyCardIssuedPoints.Size = new System.Drawing.Size(134, 25);
            this.lblLoyaltyCardIssuedPoints.TabIndex = 33;
            this.lblLoyaltyCardIssuedPoints.Text = "(Issued points)";
            // 
            // lblLoyaltyCardType
            // 
            this.lblLoyaltyCardType.AutoSize = true;
            this.lblLoyaltyCardType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "CardTypeString", true));
            this.lblLoyaltyCardType.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardType.Location = new System.Drawing.Point(259, 133);
            this.lblLoyaltyCardType.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardType.Name = "lblLoyaltyCardType";
            this.lblLoyaltyCardType.Size = new System.Drawing.Size(105, 25);
            this.lblLoyaltyCardType.TabIndex = 31;
            this.lblLoyaltyCardType.Text = "(Card type)";
            // 
            // lblLoyaltyCardStatus
            // 
            this.lblLoyaltyCardStatus.AutoSize = true;
            this.lblLoyaltyCardStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "StatusString", true));
            this.lblLoyaltyCardStatus.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardStatus.Location = new System.Drawing.Point(259, 102);
            this.lblLoyaltyCardStatus.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardStatus.Name = "lblLoyaltyCardStatus";
            this.lblLoyaltyCardStatus.Size = new System.Drawing.Size(118, 25);
            this.lblLoyaltyCardStatus.TabIndex = 30;
            this.lblLoyaltyCardStatus.Text = "(Card status)";
            // 
            // lblLoyaltyCardCustomerName
            // 
            this.lblLoyaltyCardCustomerName.AutoSize = true;
            this.lblLoyaltyCardCustomerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "CustomerName", true));
            this.lblLoyaltyCardCustomerName.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardCustomerName.Location = new System.Drawing.Point(259, 71);
            this.lblLoyaltyCardCustomerName.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardCustomerName.Name = "lblLoyaltyCardCustomerName";
            this.lblLoyaltyCardCustomerName.Size = new System.Drawing.Size(157, 25);
            this.lblLoyaltyCardCustomerName.TabIndex = 29;
            this.lblLoyaltyCardCustomerName.Text = "(Customer name)";
            // 
            // lblLoyaltyCardExpiredPointsTitle
            // 
            this.lblLoyaltyCardExpiredPointsTitle.AutoSize = true;
            this.lblLoyaltyCardExpiredPointsTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardExpiredPointsTitle.Location = new System.Drawing.Point(5, 257);
            this.lblLoyaltyCardExpiredPointsTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardExpiredPointsTitle.Name = "lblLoyaltyCardExpiredPointsTitle";
            this.lblLoyaltyCardExpiredPointsTitle.Size = new System.Drawing.Size(136, 25);
            this.lblLoyaltyCardExpiredPointsTitle.TabIndex = 28;
            this.lblLoyaltyCardExpiredPointsTitle.Text = "Expired points:";
            // 
            // lblLoyaltyCardUsedPointsTitle
            // 
            this.lblLoyaltyCardUsedPointsTitle.AutoSize = true;
            this.lblLoyaltyCardUsedPointsTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardUsedPointsTitle.Location = new System.Drawing.Point(5, 226);
            this.lblLoyaltyCardUsedPointsTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardUsedPointsTitle.Name = "lblLoyaltyCardUsedPointsTitle";
            this.lblLoyaltyCardUsedPointsTitle.Size = new System.Drawing.Size(115, 25);
            this.lblLoyaltyCardUsedPointsTitle.TabIndex = 27;
            this.lblLoyaltyCardUsedPointsTitle.Text = "Used points:";
            // 
            // lblLoyaltyCardStatusTitle
            // 
            this.lblLoyaltyCardStatusTitle.AutoSize = true;
            this.lblLoyaltyCardStatusTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardStatusTitle.Location = new System.Drawing.Point(5, 102);
            this.lblLoyaltyCardStatusTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardStatusTitle.Name = "lblLoyaltyCardStatusTitle";
            this.lblLoyaltyCardStatusTitle.Size = new System.Drawing.Size(110, 25);
            this.lblLoyaltyCardStatusTitle.TabIndex = 21;
            this.lblLoyaltyCardStatusTitle.Text = "Card status:";
            // 
            // panelAmount
            // 
            this.panelAmount.Controls.Add(this.tableLayoutPanel1);
            this.panelAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAmount.Location = new System.Drawing.Point(259, 104);
            this.panelAmount.Margin = new System.Windows.Forms.Padding(5);
            this.panelAmount.Name = "panelAmount";
            this.panelAmount.Size = new System.Drawing.Size(181, 312);
            this.panelAmount.TabIndex = 18;
            this.panelAmount.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(177, 308);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lblLoyaltyCardBalanceTitle
            // 
            this.lblLoyaltyCardBalanceTitle.AutoSize = true;
            this.lblLoyaltyCardBalanceTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardBalanceTitle.Location = new System.Drawing.Point(5, 164);
            this.lblLoyaltyCardBalanceTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardBalanceTitle.Name = "lblLoyaltyCardBalanceTitle";
            this.lblLoyaltyCardBalanceTitle.Size = new System.Drawing.Size(131, 25);
            this.lblLoyaltyCardBalanceTitle.TabIndex = 6;
            this.lblLoyaltyCardBalanceTitle.Text = "Point balance:";
            // 
            // btnGet
            // 
            this.btnGet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGet.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGet.Appearance.Options.UseFont = true;
            this.btnGet.Image = ((System.Drawing.Image)(resources.GetObject("btnGet.Image")));
            this.btnGet.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnGet.Location = new System.Drawing.Point(260, 30);
            this.btnGet.Margin = new System.Windows.Forms.Padding(6);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(57, 32);
            this.btnGet.TabIndex = 2;
            this.btnGet.Click += new System.EventHandler(this.btnValidateLoyaltyCard_Click);
            // 
            // padLoyaltyCardNumber
            // 
            this.padLoyaltyCardNumber.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.padLoyaltyCardNumber.Appearance.Options.UseFont = true;
            this.padLoyaltyCardNumber.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.padLoyaltyCardNumber.DataBindings.Add(new System.Windows.Forms.Binding("EnteredValue", this.bindingSource, "CardNumber", true));
            this.padLoyaltyCardNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.padLoyaltyCardNumber.EnteredValue = "";
            this.padLoyaltyCardNumber.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.StringPadEntryTypes.AlphaNumeric;
            this.padLoyaltyCardNumber.Location = new System.Drawing.Point(3, 3);
            this.padLoyaltyCardNumber.MaskChar = "";
            this.padLoyaltyCardNumber.MaskInterval = 0;
            this.padLoyaltyCardNumber.MaxNumberOfDigits = 80;
            this.padLoyaltyCardNumber.Name = "padLoyaltyCardNumber";
            this.padLoyaltyCardNumber.NegativeMode = false;
            this.padLoyaltyCardNumber.ShortcutKeysActive = false;
            this.padLoyaltyCardNumber.Size = new System.Drawing.Size(248, 62);
            this.padLoyaltyCardNumber.TabIndex = 0;
            this.padLoyaltyCardNumber.TimerEnabled = true;
            this.padLoyaltyCardNumber.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.StringPad.enterbuttonDelegate(this.padLoyaltyCardId_EnterButtonPressed);
            this.padLoyaltyCardNumber.CardSwept += new LSRetailPosis.POSProcesses.WinControls.StringPad.cardSwipedDelegate(this.ProcessSwipedCard);
            // 
            // lblLoyaltyCardBalance
            // 
            this.lblLoyaltyCardBalance.AutoSize = true;
            this.lblLoyaltyCardBalance.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "BalancePoints", true));
            this.lblLoyaltyCardBalance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardBalance.Location = new System.Drawing.Point(259, 164);
            this.lblLoyaltyCardBalance.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardBalance.Name = "lblLoyaltyCardBalance";
            this.lblLoyaltyCardBalance.Size = new System.Drawing.Size(139, 25);
            this.lblLoyaltyCardBalance.TabIndex = 7;
            this.lblLoyaltyCardBalance.Text = "(Point balance)";
            // 
            // lblLoyaltyCardCustomerNameTitle
            // 
            this.lblLoyaltyCardCustomerNameTitle.AutoSize = true;
            this.lblLoyaltyCardCustomerNameTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardCustomerNameTitle.Location = new System.Drawing.Point(5, 71);
            this.lblLoyaltyCardCustomerNameTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardCustomerNameTitle.Name = "lblLoyaltyCardCustomerNameTitle";
            this.lblLoyaltyCardCustomerNameTitle.Size = new System.Drawing.Size(149, 25);
            this.lblLoyaltyCardCustomerNameTitle.TabIndex = 20;
            this.lblLoyaltyCardCustomerNameTitle.Text = "Customer name:";
            // 
            // lblLoyaltyCardTypeTitle
            // 
            this.lblLoyaltyCardTypeTitle.AutoSize = true;
            this.lblLoyaltyCardTypeTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardTypeTitle.Location = new System.Drawing.Point(5, 133);
            this.lblLoyaltyCardTypeTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardTypeTitle.Name = "lblLoyaltyCardTypeTitle";
            this.lblLoyaltyCardTypeTitle.Size = new System.Drawing.Size(97, 25);
            this.lblLoyaltyCardTypeTitle.TabIndex = 23;
            this.lblLoyaltyCardTypeTitle.Text = "Card type:";
            // 
            // lblLoyaltyCardIssuedPointsTitle
            // 
            this.lblLoyaltyCardIssuedPointsTitle.AutoSize = true;
            this.lblLoyaltyCardIssuedPointsTitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblLoyaltyCardIssuedPointsTitle.Location = new System.Drawing.Point(5, 195);
            this.lblLoyaltyCardIssuedPointsTitle.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblLoyaltyCardIssuedPointsTitle.Name = "lblLoyaltyCardIssuedPointsTitle";
            this.lblLoyaltyCardIssuedPointsTitle.Size = new System.Drawing.Size(126, 25);
            this.lblLoyaltyCardIssuedPointsTitle.TabIndex = 26;
            this.lblLoyaltyCardIssuedPointsTitle.Text = "Issued points:";
            // 
            // LoyaltyCardForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "LoyaltyCardForm";
            this.Text = "LoyaltyCardForm";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).EndInit();
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
        private DevExpress.XtraEditors.PanelControl panelAmount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblLoyaltyCardExpiredPoints;
        private System.Windows.Forms.Label lblLoyaltyCardUsedPoints;
        private System.Windows.Forms.Label lblLoyaltyCardIssuedPoints;
        private System.Windows.Forms.Label lblLoyaltyCardType;
        private System.Windows.Forms.Label lblLoyaltyCardStatus;
        private System.Windows.Forms.Label lblLoyaltyCardCustomerName;
        private System.Windows.Forms.Label lblLoyaltyCardExpiredPointsTitle;
        private System.Windows.Forms.Label lblLoyaltyCardUsedPointsTitle;
        private System.Windows.Forms.Label lblLoyaltyCardStatusTitle;
        private System.Windows.Forms.Label lblLoyaltyCardCustomerNameTitle;
        private System.Windows.Forms.Label lblLoyaltyCardTypeTitle;
        private System.Windows.Forms.Label lblLoyaltyCardIssuedPointsTitle;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}
