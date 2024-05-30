using System.Drawing;
using System.Windows.Forms;
namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class CP_frmPaymentInvoice
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.styleController2 = new DevExpress.XtraEditors.StyleController(this.components);
            this.lblInvNumber = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.Label();
            this.postBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.VoucherId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.removeBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblVoucher = new System.Windows.Forms.Label();
            this.voucherTxtBox = new System.Windows.Forms.TextBox();
            this.useBtn = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtVouchTotal = new System.Windows.Forms.TextBox();
            this.txtInvTotal = new System.Windows.Forms.TextBox();
            this.lblInvTotal = new System.Windows.Forms.Label();
            this.txtRemain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInvNumber
            // 
            this.lblInvNumber.AutoSize = true;
            this.lblInvNumber.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblInvNumber.Location = new System.Drawing.Point(425, 90);
            this.lblInvNumber.Name = "lblInvNumber";
            this.lblInvNumber.Size = new System.Drawing.Size(144, 23);
            this.lblInvNumber.TabIndex = 22;
            this.lblInvNumber.Text = "Invoice Number";
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(255, 15);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(496, 65);
            this.header.TabIndex = 21;
            this.header.Text = "Payment Journal Form";
            // 
            // postBtn
            // 
            this.postBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.postBtn.FlatAppearance.BorderSize = 0;
            this.postBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.postBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.postBtn.Location = new System.Drawing.Point(339, 639);
            this.postBtn.Name = "postBtn";
            this.postBtn.Size = new System.Drawing.Size(165, 68);
            this.postBtn.TabIndex = 20;
            this.postBtn.Text = "Post Payment Journal";
            this.postBtn.UseVisualStyleBackColor = false;
            this.postBtn.Click += new System.EventHandler(this.postBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.closeBtn.Location = new System.Drawing.Point(520, 639);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(165, 68);
            this.closeBtn.TabIndex = 19;
            this.closeBtn.Text = "Cancel";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VoucherId,
            this.txtAmount,
            this.removeBtn});
            this.dataGridView1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.dataGridView1.Location = new System.Drawing.Point(23, 199);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(973, 317);
            this.dataGridView1.TabIndex = 18;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // VoucherId
            // 
            this.VoucherId.HeaderText = "Voucher No";
            this.VoucherId.Name = "VoucherId";
            this.VoucherId.ReadOnly = true;
            this.VoucherId.Width = 500;
            // 
            // txtAmount
            // 
            this.txtAmount.HeaderText = "Amount";
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Width = 270;
            // 
            // removeBtn
            // 
            this.removeBtn.HeaderText = "";
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseColumnTextForButtonValue = true;
            this.removeBtn.Width = 200;
            // 
            // lblVoucher
            // 
            this.lblVoucher.AutoSize = true;
            this.lblVoucher.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblVoucher.Location = new System.Drawing.Point(20, 165);
            this.lblVoucher.Name = "lblVoucher";
            this.lblVoucher.Size = new System.Drawing.Size(114, 18);
            this.lblVoucher.TabIndex = 23;
            this.lblVoucher.Text = "Add Voucher : ";
            // 
            // voucherTxtBox
            // 
            this.voucherTxtBox.Font = new System.Drawing.Font("Tahoma", 12F);
            this.voucherTxtBox.Location = new System.Drawing.Point(131, 161);
            this.voucherTxtBox.Name = "voucherTxtBox";
            this.voucherTxtBox.Size = new System.Drawing.Size(293, 27);
            this.voucherTxtBox.TabIndex = 24;
            // 
            // useBtn
            // 
            this.useBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.useBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.useBtn.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.useBtn.Location = new System.Drawing.Point(430, 161);
            this.useBtn.Name = "useBtn";
            this.useBtn.Size = new System.Drawing.Size(140, 27);
            this.useBtn.TabIndex = 26;
            this.useBtn.Text = "Use Voucher";
            this.useBtn.UseVisualStyleBackColor = false;
            this.useBtn.Click += new System.EventHandler(this.useBtn_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(21, 561);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(180, 18);
            this.lblTotal.TabIndex = 27;
            this.lblTotal.Text = "Total Voucher Amount :";
            // 
            // txtVouchTotal
            // 
            this.txtVouchTotal.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtVouchTotal.Location = new System.Drawing.Point(207, 557);
            this.txtVouchTotal.Name = "txtVouchTotal";
            this.txtVouchTotal.ReadOnly = true;
            this.txtVouchTotal.Size = new System.Drawing.Size(280, 27);
            this.txtVouchTotal.TabIndex = 28;
            this.txtVouchTotal.TextChanged += new System.EventHandler(this.txtTotal_TextChanged);
            // 
            // txtInvTotal
            // 
            this.txtInvTotal.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtInvTotal.Location = new System.Drawing.Point(207, 524);
            this.txtInvTotal.Name = "txtInvTotal";
            this.txtInvTotal.ReadOnly = true;
            this.txtInvTotal.Size = new System.Drawing.Size(280, 27);
            this.txtInvTotal.TabIndex = 30;
            // 
            // lblInvTotal
            // 
            this.lblInvTotal.AutoSize = true;
            this.lblInvTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblInvTotal.Location = new System.Drawing.Point(21, 528);
            this.lblInvTotal.Name = "lblInvTotal";
            this.lblInvTotal.Size = new System.Drawing.Size(175, 18);
            this.lblInvTotal.TabIndex = 29;
            this.lblInvTotal.Text = "Total Invoice Amount :";
            // 
            // txtRemain
            // 
            this.txtRemain.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtRemain.Location = new System.Drawing.Point(207, 590);
            this.txtRemain.Name = "txtRemain";
            this.txtRemain.ReadOnly = true;
            this.txtRemain.Size = new System.Drawing.Size(280, 27);
            this.txtRemain.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(21, 594);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 18);
            this.label1.TabIndex = 31;
            this.label1.Text = "Remaining Amount :";
            // 
            // CP_frmPaymentInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.txtRemain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInvTotal);
            this.Controls.Add(this.lblInvTotal);
            this.Controls.Add(this.txtVouchTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.useBtn);
            this.Controls.Add(this.voucherTxtBox);
            this.Controls.Add(this.lblVoucher);
            this.Controls.Add(this.lblInvNumber);
            this.Controls.Add(this.header);
            this.Controls.Add(this.postBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.dataGridView1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CP_frmPaymentInvoice";
            this.Text = "CP_frmPaymentInvoice";
            this.Controls.SetChildIndex(this.dataGridView1, 0);
            this.Controls.SetChildIndex(this.closeBtn, 0);
            this.Controls.SetChildIndex(this.postBtn, 0);
            this.Controls.SetChildIndex(this.header, 0);
            this.Controls.SetChildIndex(this.lblInvNumber, 0);
            this.Controls.SetChildIndex(this.lblVoucher, 0);
            this.Controls.SetChildIndex(this.voucherTxtBox, 0);
            this.Controls.SetChildIndex(this.useBtn, 0);
            this.Controls.SetChildIndex(this.lblTotal, 0);
            this.Controls.SetChildIndex(this.txtVouchTotal, 0);
            this.Controls.SetChildIndex(this.lblInvTotal, 0);
            this.Controls.SetChildIndex(this.txtInvTotal, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtRemain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.StyleController styleController2;
        private System.Windows.Forms.Label lblInvNumber;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Button postBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblVoucher;
        private System.Windows.Forms.TextBox voucherTxtBox;
        private System.Windows.Forms.Button useBtn;
        private DevExpress.XtraEditors.StyleController styleController;
        public Label lblTotal;
        private TextBox txtVouchTotal;
        private DataGridViewTextBoxColumn VoucherId;
        private DataGridViewTextBoxColumn txtAmount;
        private DataGridViewButtonColumn removeBtn;
        private TextBox txtInvTotal;
        public Label lblInvTotal;
        private TextBox txtRemain;
        public Label label1;
    }
}