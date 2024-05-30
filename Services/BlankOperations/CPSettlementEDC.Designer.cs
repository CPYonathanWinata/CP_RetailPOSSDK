namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPSettlementEDC
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
            this.cmbPilihBank = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblWaitingRespond = new System.Windows.Forms.Label();
            this.lblBank = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbPilihBank
            // 
            this.cmbPilihBank.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbPilihBank.FormattingEnabled = true;
            this.cmbPilihBank.Location = new System.Drawing.Point(12, 23);
            this.cmbPilihBank.Name = "cmbPilihBank";
            this.cmbPilihBank.Size = new System.Drawing.Size(203, 28);
            this.cmbPilihBank.TabIndex = 5;
            this.cmbPilihBank.Text = "Pilih Bank";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.btnOk.Location = new System.Drawing.Point(12, 90);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 37);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.btnCancel.Location = new System.Drawing.Point(140, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 37);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblWaitingRespond
            // 
            this.lblWaitingRespond.AutoSize = true;
            this.lblWaitingRespond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaitingRespond.ForeColor = System.Drawing.Color.Red;
            this.lblWaitingRespond.Location = new System.Drawing.Point(12, 66);
            this.lblWaitingRespond.Name = "lblWaitingRespond";
            this.lblWaitingRespond.Size = new System.Drawing.Size(203, 13);
            this.lblWaitingRespond.TabIndex = 8;
            this.lblWaitingRespond.Text = "Waiting for response...Please check EDC";
            this.lblWaitingRespond.Visible = false;
            // 
            // lblBank
            // 
            this.lblBank.AutoSize = true;
            this.lblBank.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBank.Location = new System.Drawing.Point(12, 4);
            this.lblBank.Name = "lblBank";
            this.lblBank.Size = new System.Drawing.Size(64, 13);
            this.lblBank.TabIndex = 9;
            this.lblBank.Text = "Pilih Bank";
            // 
            // CPSettlementEDC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 136);
            this.ControlBox = false;
            this.Controls.Add(this.lblBank);
            this.Controls.Add(this.lblWaitingRespond);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbPilihBank);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CPSettlementEDC";
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.CPSettlementEDC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPilihBank;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblWaitingRespond;
        private System.Windows.Forms.Label lblBank;
    }
}