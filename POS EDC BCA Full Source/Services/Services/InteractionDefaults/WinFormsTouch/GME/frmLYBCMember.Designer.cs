namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class frmLYBCMember
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLYBCMember));
            //this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.CustTypeLbl = new System.Windows.Forms.Label();
            this.CardNumberTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CustTypeLbl
            // 
            this.CustTypeLbl.AutoSize = true;
            this.CustTypeLbl.Font = new System.Drawing.Font("Comic Sans MS", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustTypeLbl.ForeColor = System.Drawing.Color.Black;
            this.CustTypeLbl.Location = new System.Drawing.Point(313, 77);
            this.CustTypeLbl.Name = "CustTypeLbl";
            this.CustTypeLbl.Size = new System.Drawing.Size(411, 74);
            this.CustTypeLbl.TabIndex = 19;
            this.CustTypeLbl.Text = "LYBC MEMBER";
            // 
            // CardNumberTxt
            // 
            this.CardNumberTxt.BackColor = System.Drawing.Color.Ivory;
            this.CardNumberTxt.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CardNumberTxt.Location = new System.Drawing.Point(360, 312);
            this.CardNumberTxt.Name = "CardNumberTxt";
            this.CardNumberTxt.Size = new System.Drawing.Size(310, 34);
            this.CardNumberTxt.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(355, 276);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 23);
            this.label1.TabIndex = 25;
            this.label1.Text = "Card number / Mobile phone number / Email";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.BackgroundImage = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.search;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(676, 307);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 42);
            this.btnSearch.TabIndex = 26;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.BackgroundImage = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.remove;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.Transparent;
            this.btnClear.Location = new System.Drawing.Point(728, 307);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 42);
            this.btnClear.TabIndex = 27;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.cancelBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelBtn.BackgroundImage")));
            this.cancelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.ForeColor = System.Drawing.Color.Transparent;
            this.cancelBtn.Location = new System.Drawing.Point(454, 492);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(134, 69);
            this.cancelBtn.TabIndex = 31;
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 139);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 46;
            this.pictureBox1.TabStop = false;
            // 
            // frmLYBCMember
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.CardNumberTxt);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CustTypeLbl);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmLYBCMember";
            this.Text = "frmLYBCMember";
            this.Controls.SetChildIndex(this.CustTypeLbl, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnSearch, 0);
            this.Controls.SetChildIndex(this.btnClear, 0);
            this.Controls.SetChildIndex(this.cancelBtn, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.CardNumberTxt, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label CustTypeLbl;
        private System.Windows.Forms.TextBox CardNumberTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button cancelBtn;
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
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
    }
}