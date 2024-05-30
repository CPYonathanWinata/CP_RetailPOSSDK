namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPPayGoPay
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelAmountDue = new System.Windows.Forms.Label();
            this.labelAmountDueValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numPad1 = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtHP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.txtReff = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.amtCustAmounts = new LSRetailPosis.POSProcesses.WinControls.AmountViewer();
            this.label4 = new System.Windows.Forms.Label();
            this.labelPaymentAmount = new System.Windows.Forms.Label();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1004, 726);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(30, 649);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(944, 66);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(342, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 57);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(475, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 57);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.labelAmountDue);
            this.flowLayoutPanel1.Controls.Add(this.labelAmountDueValue);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(233, 43);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(537, 95);
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
            this.labelAmountDue.Size = new System.Drawing.Size(398, 95);
            this.labelAmountDue.TabIndex = 0;
            this.labelAmountDue.Tag = "";
            this.labelAmountDue.Text = "Total amount due:";
            // 
            // labelAmountDueValue
            // 
            this.labelAmountDueValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAmountDueValue.AutoSize = true;
            this.labelAmountDueValue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelAmountDueValue.Location = new System.Drawing.Point(398, 0);
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.numPad1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 3, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(100, 175);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 429);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numPad1
            // 
            this.numPad1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numPad1.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPad1.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.numPad1.Appearance.Options.UseFont = true;
            this.numPad1.Appearance.Options.UseForeColor = true;
            this.numPad1.AutoSize = true;
            this.numPad1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numPad1.CurrencyCode = null;
            this.numPad1.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPad1.EnteredValue = "";
            this.numPad1.Location = new System.Drawing.Point(251, 30);
            this.numPad1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.numPad1.MaskChar = "";
            this.numPad1.MaskInterval = 0;
            this.numPad1.MaxNumberOfDigits = 20;
            this.numPad1.MinimumSize = new System.Drawing.Size(300, 330);
            this.numPad1.Name = "numPad1";
            this.numPad1.NegativeMode = false;
            this.numPad1.NoOfTries = 0;
            this.numPad1.NumberOfDecimals = 2;
            this.numPad1.PromptText = "Enter Amount";
            this.numPad1.ShortcutKeysActive = false;
            this.numPad1.Size = new System.Drawing.Size(300, 330);
            this.numPad1.TabIndex = 2;
            this.numPad1.TimerEnabled = true;
            this.numPad1.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad1_EnterButtonPressed);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtHP);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtCustomerName);
            this.panel1.Controls.Add(this.txtReff);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtAmount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(43, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 393);
            this.panel1.TabIndex = 4;
            // 
            // txtHP
            // 
            this.txtHP.Location = new System.Drawing.Point(3, 79);
            this.txtHP.Name = "txtHP";
            this.txtHP.Size = new System.Drawing.Size(194, 20);
            this.txtHP.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "No Handphone";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(3, 178);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(194, 20);
            this.txtCustomerName.TabIndex = 5;
            // 
            // txtReff
            // 
            this.txtReff.Location = new System.Drawing.Point(3, 126);
            this.txtReff.Name = "txtReff";
            this.txtReff.Size = new System.Drawing.Size(194, 20);
            this.txtReff.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Customer Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "No Reff Transaksi";
            // 
            // txtAmount
            // 
            this.txtAmount.Enabled = false;
            this.txtAmount.Location = new System.Drawing.Point(3, 31);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(194, 20);
            this.txtAmount.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Amount";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.amtCustAmounts);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(559, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 393);
            this.panel2.TabIndex = 5;
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
            this.amtCustAmounts.Location = new System.Drawing.Point(0, 27);
            this.amtCustAmounts.LowesetOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCustAmounts.Name = "amtCustAmounts";
            this.amtCustAmounts.OptionsLimit = 5;
            this.amtCustAmounts.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.amtCustAmounts.ShowBorder = false;
            this.amtCustAmounts.Size = new System.Drawing.Size(200, 366);
            this.amtCustAmounts.TabIndex = 2;
            this.amtCustAmounts.UsedCurrencyCode = "";
            this.amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 27);
            this.label4.TabIndex = 1;
            this.label4.Text = "Payment amount";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelPaymentAmount
            // 
            this.labelPaymentAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.labelPaymentAmount.Location = new System.Drawing.Point(2, 2);
            this.labelPaymentAmount.Name = "labelPaymentAmount";
            this.labelPaymentAmount.Size = new System.Drawing.Size(196, 27);
            this.labelPaymentAmount.TabIndex = 0;
            this.labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CPPayGoPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(1004, 726);
            this.Controls.Add(this.tableLayoutPanel4);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "CPPayGoPay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.CPPayGoPay_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelAmountDue;
        private System.Windows.Forms.Label labelAmountDueValue;
        private System.Windows.Forms.Label labelPaymentAmount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numPad1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.TextBox txtReff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private LSRetailPosis.POSProcesses.WinControls.AmountViewer amtCustAmounts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtHP;
        private System.Windows.Forms.Label label5;
    }
}