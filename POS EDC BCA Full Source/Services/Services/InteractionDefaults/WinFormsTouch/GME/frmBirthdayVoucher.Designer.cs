namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class frmBirthdayVoucher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBirthdayVoucher));
            //this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.VoucherIdTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(12, 12);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(147, 139);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 67;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(270, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(578, 90);
            this.label1.TabIndex = 68;
            this.label1.Text = "Birthday Voucher";
            // 
            // OKBtn
            // 
            this.OKBtn.BackColor = System.Drawing.Color.Transparent;
            this.OKBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OKBtn.BackgroundImage")));
            this.OKBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKBtn.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKBtn.ForeColor = System.Drawing.Color.Transparent;
            this.OKBtn.Location = new System.Drawing.Point(369, 500);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(134, 69);
            this.OKBtn.TabIndex = 76;
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // VoucherIdTxt
            // 
            this.VoucherIdTxt.BackColor = System.Drawing.Color.Ivory;
            this.VoucherIdTxt.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VoucherIdTxt.Location = new System.Drawing.Point(368, 305);
            this.VoucherIdTxt.Name = "VoucherIdTxt";
            this.VoucherIdTxt.Size = new System.Drawing.Size(383, 34);
            this.VoucherIdTxt.TabIndex = 75;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(364, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 29);
            this.label2.TabIndex = 74;
            this.label2.Text = "Voucher ID";
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CancelBtn.BackgroundImage")));
            this.CancelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.ForeColor = System.Drawing.Color.Transparent;
            this.CancelBtn.Location = new System.Drawing.Point(617, 500);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(134, 69);
            this.CancelBtn.TabIndex = 77;
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // frmBirthdayVoucher
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.VoucherIdTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmBirthdayVoucher";
            this.Text = "frmBirthdayVoucher";
            this.Load += new System.EventHandler(this.frmBirthdayVoucher_Load);
            this.Controls.SetChildIndex(this.pictureBox3, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.VoucherIdTxt, 0);
            this.Controls.SetChildIndex(this.OKBtn, 0);
            this.Controls.SetChildIndex(this.CancelBtn, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.PictureBox pictureBox3;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TextBox VoucherIdTxt;
        private System.Windows.Forms.Label label2;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Button CancelBtn;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
    }
}