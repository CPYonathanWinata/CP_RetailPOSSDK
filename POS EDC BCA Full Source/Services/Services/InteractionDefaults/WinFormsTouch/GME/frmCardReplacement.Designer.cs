namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class FrmCardReplacement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCardReplacement));
            //this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.newCardTypeCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.newCardNumTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.oldCardTypeCombo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.oldCardNumTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // newCardTypeCombo
            // 
            this.newCardTypeCombo.BackColor = System.Drawing.Color.Ivory;
            this.newCardTypeCombo.Enabled = false;
            this.newCardTypeCombo.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newCardTypeCombo.FormattingEnabled = true;
            this.newCardTypeCombo.Items.AddRange(new object[] {
            "As Card Tender",
            "As Contact Tender",
            "No Tender",
            "Blocked"});
            this.newCardTypeCombo.Location = new System.Drawing.Point(349, 460);
            this.newCardTypeCombo.Name = "newCardTypeCombo";
            this.newCardTypeCombo.Size = new System.Drawing.Size(383, 34);
            this.newCardTypeCombo.TabIndex = 81;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(344, 431);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 26);
            this.label4.TabIndex = 80;
            this.label4.Text = "New card type";
            // 
            // newCardNumTxt
            // 
            this.newCardNumTxt.BackColor = System.Drawing.Color.Ivory;
            this.newCardNumTxt.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newCardNumTxt.Location = new System.Drawing.Point(349, 392);
            this.newCardNumTxt.Name = "newCardNumTxt";
            this.newCardNumTxt.Size = new System.Drawing.Size(383, 34);
            this.newCardNumTxt.TabIndex = 79;
            this.newCardNumTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.newCardNumTxt_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(344, 363);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 26);
            this.label5.TabIndex = 78;
            this.label5.Text = "New card number";
            // 
            // oldCardTypeCombo
            // 
            this.oldCardTypeCombo.BackColor = System.Drawing.Color.Ivory;
            this.oldCardTypeCombo.Enabled = false;
            this.oldCardTypeCombo.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oldCardTypeCombo.FormattingEnabled = true;
            this.oldCardTypeCombo.Items.AddRange(new object[] {
            "Blocked",
            "As Card Tender",
            "As Contact Tender",
            "No Tender"});
            this.oldCardTypeCombo.Location = new System.Drawing.Point(349, 324);
            this.oldCardTypeCombo.Name = "oldCardTypeCombo";
            this.oldCardTypeCombo.Size = new System.Drawing.Size(383, 34);
            this.oldCardTypeCombo.TabIndex = 77;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(344, 295);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 26);
            this.label3.TabIndex = 76;
            this.label3.Text = "Old card type";
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CancelBtn.BackgroundImage")));
            this.CancelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.ForeColor = System.Drawing.Color.Transparent;
            this.CancelBtn.Location = new System.Drawing.Point(598, 554);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(134, 69);
            this.CancelBtn.TabIndex = 75;
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.BackColor = System.Drawing.Color.Transparent;
            this.OKBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OKBtn.BackgroundImage")));
            this.OKBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKBtn.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKBtn.ForeColor = System.Drawing.Color.Transparent;
            this.OKBtn.Location = new System.Drawing.Point(349, 554);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(134, 69);
            this.OKBtn.TabIndex = 74;
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // oldCardNumTxt
            // 
            this.oldCardNumTxt.BackColor = System.Drawing.Color.Ivory;
            this.oldCardNumTxt.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oldCardNumTxt.Location = new System.Drawing.Point(349, 256);
            this.oldCardNumTxt.Name = "oldCardNumTxt";
            this.oldCardNumTxt.Size = new System.Drawing.Size(383, 34);
            this.oldCardNumTxt.TabIndex = 73;
            this.oldCardNumTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.oldCardNumTxt_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(344, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 26);
            this.label2.TabIndex = 72;
            this.label2.Text = "Old card number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(296, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(490, 74);
            this.label1.TabIndex = 71;
            this.label1.Text = "Card Replacement";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 145);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 83;
            this.pictureBox1.TabStop = false;
            // 
            // FrmCardReplacement
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.newCardTypeCombo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.newCardNumTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.oldCardTypeCombo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.oldCardNumTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "FrmCardReplacement";
            this.Text = "frmCardReplacement";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.oldCardNumTxt, 0);
            this.Controls.SetChildIndex(this.OKBtn, 0);
            this.Controls.SetChildIndex(this.CancelBtn, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.oldCardTypeCombo, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.newCardNumTxt, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.newCardTypeCombo, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox newCardTypeCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox newCardNumTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox oldCardTypeCombo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TextBox oldCardNumTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.PictureBox pictureBox1;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
    }
}