namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class frmCustomerInfo
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
            //this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.CustInfoDG = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustInfoDG)).BeginInit();
            this.SuspendLayout();
            // 
            // CustInfoDG
            // 
            this.CustInfoDG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustInfoDG.Location = new System.Drawing.Point(155, 217);
            this.CustInfoDG.Name = "CustInfoDG";
            this.CustInfoDG.Size = new System.Drawing.Size(718, 367);
            this.CustInfoDG.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(343, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 65);
            this.label1.TabIndex = 5;
            this.label1.Text = "Customer Info";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.CancelBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.ForeColor = System.Drawing.Color.Black;
            this.CancelBtn.Location = new System.Drawing.Point(759, 613);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CancelBtn.Size = new System.Drawing.Size(114, 50);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // frmCustomerInfo
            // 
            this.Appearance.BackColor = System.Drawing.Color.SeaGreen;
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CustInfoDG);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmCustomerInfo";
            this.Text = "frmCustomerInfo";
            this.Load += new System.EventHandler(this.frmCustomerInfo_Load);
            this.Controls.SetChildIndex(this.CustInfoDG, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.CancelBtn, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustInfoDG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.DataGridView CustInfoDG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelBtn;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
    }
}