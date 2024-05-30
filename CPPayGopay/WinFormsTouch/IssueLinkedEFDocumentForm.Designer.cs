/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class IssueLinkedEFDocumentForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tableLayoutInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblFiscalPrinterTerminalNumber = new System.Windows.Forms.Label();
            this.lblFiscalPrinterTerminalNumberLabel = new System.Windows.Forms.Label();
            this.lblFiscalReceiptNumber = new System.Windows.Forms.Label();
            this.lblFiscalReceiptNumberLabel = new System.Windows.Forms.Label();
            this.lblReceiptNumberLabel = new System.Windows.Forms.Label();
            this.lblReceiptNumber = new System.Windows.Forms.Label();
            this.lblCustomerAddressLabel = new System.Windows.Forms.Label();
            this.lblCustomerReceiptEmailLabel = new System.Windows.Forms.Label();
            this.checkSendByEmailLabel = new System.Windows.Forms.Label();
            this.lblCustomerCNPJCPFLabel = new System.Windows.Forms.Label();
            this.lblCustomerNameLabel = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblCustomerCNPJCPF = new System.Windows.Forms.Label();
            this.lblCustomerAddress = new System.Windows.Forms.Label();
            this.lblCustomerReceiptEmail = new System.Windows.Forms.Label();
            this.checkSendByEmail = new System.Windows.Forms.CheckBox();
            this.tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectCustomer = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutMain.SuspendLayout();
            this.tableLayoutInfo.SuspendLayout();
            this.tableLayoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 3;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutMain.Controls.Add(this.lblTitle, 1, 1);
            this.tableLayoutMain.Controls.Add(this.tableLayoutButtons, 1, 4);
            this.tableLayoutMain.Controls.Add(this.tableLayoutInfo, 1, 2);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 6;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutMain.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblTitle.Location = new System.Drawing.Point(163, 25);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3, 10, 3, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(697, 81);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Tag = "";
            this.lblTitle.Text = "Issue referenced NF-e";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutInfo
            // 
            this.tableLayoutInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutInfo.AutoSize = true;
            this.tableLayoutInfo.ColumnCount = 2;
            this.tableLayoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutInfo.Controls.Add(this.lblFiscalPrinterTerminalNumber, 1, 2);
            this.tableLayoutInfo.Controls.Add(this.lblFiscalPrinterTerminalNumberLabel, 0, 2);
            this.tableLayoutInfo.Controls.Add(this.lblFiscalReceiptNumber, 1, 1);
            this.tableLayoutInfo.Controls.Add(this.lblFiscalReceiptNumberLabel, 0, 1);
            this.tableLayoutInfo.Controls.Add(this.lblReceiptNumberLabel, 0, 0);
            this.tableLayoutInfo.Controls.Add(this.lblReceiptNumber, 1, 0);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerAddressLabel, 0, 5);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerReceiptEmailLabel, 0, 6);
            this.tableLayoutInfo.Controls.Add(this.checkSendByEmailLabel, 0, 7);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerCNPJCPFLabel, 0, 4);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerNameLabel, 0, 3);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerName, 1, 3);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerCNPJCPF, 1, 4);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerAddress, 1, 5);
            this.tableLayoutInfo.Controls.Add(this.lblCustomerReceiptEmail, 1, 6);
            this.tableLayoutInfo.Controls.Add(this.checkSendByEmail, 1, 7);
            this.tableLayoutInfo.Location = new System.Drawing.Point(33, 136);
            this.tableLayoutInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.tableLayoutInfo.Name = "tableLayoutInfo";
            this.tableLayoutInfo.RowCount = 6;
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutInfo.Size = new System.Drawing.Size(958, 400);
            this.tableLayoutInfo.TabIndex = 29;
            // 
            // lblFiscalPrinterTerminalNumber
            // 
            this.lblFiscalPrinterTerminalNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiscalPrinterTerminalNumber.AutoSize = true;
            this.lblFiscalPrinterTerminalNumber.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalPrinterTerminalNumber.Location = new System.Drawing.Point(482, 105);
            this.lblFiscalPrinterTerminalNumber.Name = "lblFiscalPrinterTerminalNumber";
            this.lblFiscalPrinterTerminalNumber.Size = new System.Drawing.Size(0, 40);
            this.lblFiscalPrinterTerminalNumber.TabIndex = 33;
            // 
            // lblFiscalPrinterTerminalNumberLabel
            // 
            this.lblFiscalPrinterTerminalNumberLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFiscalPrinterTerminalNumberLabel.AutoSize = true;
            this.lblFiscalPrinterTerminalNumberLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalPrinterTerminalNumberLabel.Location = new System.Drawing.Point(78, 105);
            this.lblFiscalPrinterTerminalNumberLabel.Name = "lblFiscalPrinterTerminalNumberLabel";
            this.lblFiscalPrinterTerminalNumberLabel.Size = new System.Drawing.Size(398, 40);
            this.lblFiscalPrinterTerminalNumberLabel.TabIndex = 31;
            this.lblFiscalPrinterTerminalNumberLabel.Text = "Fiscal printer terminal number";
            // 
            // lblFiscalReceiptNumber
            // 
            this.lblFiscalReceiptNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiscalReceiptNumber.AutoSize = true;
            this.lblFiscalReceiptNumber.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalReceiptNumber.Location = new System.Drawing.Point(482, 55);
            this.lblFiscalReceiptNumber.Name = "lblFiscalReceiptNumber";
            this.lblFiscalReceiptNumber.Size = new System.Drawing.Size(0, 40);
            this.lblFiscalReceiptNumber.TabIndex = 32;
            // 
            // lblFiscalReceiptNumberLabel
            // 
            this.lblFiscalReceiptNumberLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFiscalReceiptNumberLabel.AutoSize = true;
            this.lblFiscalReceiptNumberLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalReceiptNumberLabel.Location = new System.Drawing.Point(189, 55);
            this.lblFiscalReceiptNumberLabel.Name = "lblFiscalReceiptNumberLabel";
            this.lblFiscalReceiptNumberLabel.Size = new System.Drawing.Size(287, 40);
            this.lblFiscalReceiptNumberLabel.TabIndex = 31;
            this.lblFiscalReceiptNumberLabel.Text = "Fiscal receipt number";
            // 
            // lblReceiptNumberLabel
            // 
            this.lblReceiptNumberLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReceiptNumberLabel.AutoSize = true;
            this.lblReceiptNumberLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNumberLabel.Location = new System.Drawing.Point(252, 5);
            this.lblReceiptNumberLabel.Name = "lblReceiptNumberLabel";
            this.lblReceiptNumberLabel.Size = new System.Drawing.Size(224, 40);
            this.lblReceiptNumberLabel.TabIndex = 5;
            this.lblReceiptNumberLabel.Text = "Receipt Number";
            // 
            // lblReceiptNumber
            // 
            this.lblReceiptNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblReceiptNumber.AutoSize = true;
            this.lblReceiptNumber.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNumber.Location = new System.Drawing.Point(482, 5);
            this.lblReceiptNumber.Name = "lblReceiptNumber";
            this.lblReceiptNumber.Size = new System.Drawing.Size(0, 40);
            this.lblReceiptNumber.TabIndex = 12;
            // 
            // lblCustomerAddressLabel
            // 
            this.lblCustomerAddressLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCustomerAddressLabel.AutoSize = true;
            this.lblCustomerAddressLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerAddressLabel.Location = new System.Drawing.Point(230, 255);
            this.lblCustomerAddressLabel.Name = "lblCustomerAddressLabel";
            this.lblCustomerAddressLabel.Size = new System.Drawing.Size(246, 40);
            this.lblCustomerAddressLabel.TabIndex = 8;
            this.lblCustomerAddressLabel.Text = "Customer address";
            // 
            // lblCustomerReceiptEmailLabel
            // 
            this.lblCustomerReceiptEmailLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCustomerReceiptEmailLabel.AutoSize = true;
            this.lblCustomerReceiptEmailLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerReceiptEmailLabel.Location = new System.Drawing.Point(164, 305);
            this.lblCustomerReceiptEmailLabel.Name = "lblCustomerReceiptEmailLabel";
            this.lblCustomerReceiptEmailLabel.Size = new System.Drawing.Size(312, 40);
            this.lblCustomerReceiptEmailLabel.TabIndex = 9;
            this.lblCustomerReceiptEmailLabel.Text = "Customer receipt email";
            // 
            // checkSendByEmailLabel
            // 
            this.checkSendByEmailLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.checkSendByEmailLabel.AutoSize = true;
            this.checkSendByEmailLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkSendByEmailLabel.Location = new System.Drawing.Point(280, 355);
            this.checkSendByEmailLabel.Name = "checkSendByEmailLabel";
            this.checkSendByEmailLabel.Size = new System.Drawing.Size(196, 40);
            this.checkSendByEmailLabel.TabIndex = 9;
            this.checkSendByEmailLabel.Text = "Send by email";
            // 
            // lblCustomerCNPJCPFLabel
            // 
            this.lblCustomerCNPJCPFLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCustomerCNPJCPFLabel.AutoSize = true;
            this.lblCustomerCNPJCPFLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerCNPJCPFLabel.Location = new System.Drawing.Point(203, 205);
            this.lblCustomerCNPJCPFLabel.Name = "lblCustomerCNPJCPFLabel";
            this.lblCustomerCNPJCPFLabel.Size = new System.Drawing.Size(273, 40);
            this.lblCustomerCNPJCPFLabel.TabIndex = 6;
            this.lblCustomerCNPJCPFLabel.Text = "Customer CNPJ/CPF";
            // 
            // lblCustomerNameLabel
            // 
            this.lblCustomerNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCustomerNameLabel.AutoSize = true;
            this.lblCustomerNameLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNameLabel.Location = new System.Drawing.Point(257, 155);
            this.lblCustomerNameLabel.Name = "lblCustomerNameLabel";
            this.lblCustomerNameLabel.Size = new System.Drawing.Size(219, 40);
            this.lblCustomerNameLabel.TabIndex = 4;
            this.lblCustomerNameLabel.Text = "Customer name";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(482, 155);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(0, 40);
            this.lblCustomerName.TabIndex = 9;
            // 
            // lblCustomerCNPJCPF
            // 
            this.lblCustomerCNPJCPF.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerCNPJCPF.AutoSize = true;
            this.lblCustomerCNPJCPF.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerCNPJCPF.Location = new System.Drawing.Point(482, 205);
            this.lblCustomerCNPJCPF.Name = "lblCustomerCNPJCPF";
            this.lblCustomerCNPJCPF.Size = new System.Drawing.Size(0, 40);
            this.lblCustomerCNPJCPF.TabIndex = 10;
            // 
            // lblCustomerAddress
            // 
            this.lblCustomerAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerAddress.AutoSize = true;
            this.lblCustomerAddress.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerAddress.Location = new System.Drawing.Point(482, 255);
            this.lblCustomerAddress.Name = "lblCustomerAddress";
            this.lblCustomerAddress.Size = new System.Drawing.Size(0, 40);
            this.lblCustomerAddress.TabIndex = 11;
            // 
            // lblCustomerReceiptEmail
            // 
            this.lblCustomerReceiptEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerReceiptEmail.AutoSize = true;
            this.lblCustomerReceiptEmail.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerReceiptEmail.Location = new System.Drawing.Point(482, 305);
            this.lblCustomerReceiptEmail.Name = "lblCustomerReceiptEmail";
            this.lblCustomerReceiptEmail.Size = new System.Drawing.Size(0, 40);
            this.lblCustomerReceiptEmail.TabIndex = 11;
            // 
            // checkSendByEmail
            // 
            this.checkSendByEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkSendByEmail.AutoSize = true;
            this.checkSendByEmail.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkSendByEmail.Location = new System.Drawing.Point(482, 364);
            this.checkSendByEmail.Name = "checkSendByEmail";
            this.checkSendByEmail.Size = new System.Drawing.Size(22, 21);
            this.checkSendByEmail.TabIndex = 11;
            // 
            // tableLayoutButtons
            // 
            this.tableLayoutButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutButtons.ColumnCount = 3;
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.Controls.Add(this.btnOk, 1, 0);
            this.tableLayoutButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutButtons.Controls.Add(this.btnSelectCustomer, 0, 0);
            this.tableLayoutButtons.Location = new System.Drawing.Point(33, 557);
            this.tableLayoutButtons.Name = "tableLayoutButtons";
            this.tableLayoutButtons.RowCount = 1;
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutButtons.Size = new System.Drawing.Size(958, 100);
            this.tableLayoutButtons.TabIndex = 30;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(406, 24);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(146, 52);
            this.btnOk.TabIndex = 27;
            this.btnOk.Tag = "btnOk";
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(558, 24);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 52);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Tag = "btnCancel";
            this.btnCancel.Text = "Cancel";
            // 
            // btnSelectCustomer
            // 
            this.btnSelectCustomer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSelectCustomer.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectCustomer.Appearance.Options.UseFont = true;
            this.btnSelectCustomer.Location = new System.Drawing.Point(225, 24);
            this.btnSelectCustomer.Name = "btnSelectCustomer";
            this.btnSelectCustomer.Size = new System.Drawing.Size(175, 52);
            this.btnSelectCustomer.TabIndex = 26;
            this.btnSelectCustomer.Tag = "btnSelectCustomer";
            this.btnSelectCustomer.Text = "Select Customer";
            this.btnSelectCustomer.Click += new System.EventHandler(this.btnSelectCustomer_Click);
            // 
            // IssueLinkedEFDocumentForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutMain);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "IssueLinkedEFDocumentForm";
            this.Controls.SetChildIndex(this.tableLayoutMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutMain.ResumeLayout(false);
            this.tableLayoutMain.PerformLayout();
            this.tableLayoutInfo.ResumeLayout(false);
            this.tableLayoutInfo.PerformLayout();
            this.tableLayoutButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private DevExpress.XtraEditors.SimpleButton btnSelectCustomer;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutButtons;
        private System.Windows.Forms.Label lblCustomerNameLabel;
        private System.Windows.Forms.Label lblReceiptNumberLabel;
        private System.Windows.Forms.Label lblCustomerCNPJCPFLabel;
        private System.Windows.Forms.Label lblCustomerAddressLabel;
        private System.Windows.Forms.Label lblCustomerReceiptEmailLabel;
        private System.Windows.Forms.Label checkSendByEmailLabel;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblCustomerCNPJCPF;
        private System.Windows.Forms.Label lblReceiptNumber;
        private System.Windows.Forms.Label lblCustomerAddress;
        private System.Windows.Forms.Label lblCustomerReceiptEmail;
        private System.Windows.Forms.CheckBox checkSendByEmail;
        private System.Windows.Forms.Label lblFiscalPrinterTerminalNumber;
        private System.Windows.Forms.Label lblFiscalPrinterTerminalNumberLabel;
        private System.Windows.Forms.Label lblFiscalReceiptNumber;
        private System.Windows.Forms.Label lblFiscalReceiptNumberLabel;                  
    }
}