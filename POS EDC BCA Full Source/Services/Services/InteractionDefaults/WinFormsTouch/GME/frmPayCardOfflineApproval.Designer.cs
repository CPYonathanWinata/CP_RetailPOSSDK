namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class frmPayCardOfflineApproval
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayCardOfflineApproval));
            //this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.Numpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CustTypeLbl = new System.Windows.Forms.Label();
            this.txtApprovalCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Numpad
            // 
            this.Numpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Numpad.Appearance.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Numpad.Appearance.Options.UseFont = true;
            this.Numpad.AutoSize = true;
            this.Numpad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Numpad.CurrencyCode = null;
            this.Numpad.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.Numpad.EnteredValue = "";
            this.Numpad.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Price;
            this.Numpad.Location = new System.Drawing.Point(366, 251);
            this.Numpad.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Numpad.MaskChar = "";
            this.Numpad.MaskInterval = 2;
            this.Numpad.MaxNumberOfDigits = 9;
            this.Numpad.MinimumSize = new System.Drawing.Size(300, 330);
            this.Numpad.Name = "Numpad";
            this.Numpad.NegativeMode = false;
            this.Numpad.NoOfTries = 0;
            this.Numpad.NumberOfDecimals = 0;
            this.Numpad.PromptText = " ";
            this.Numpad.ShortcutKeysActive = false;
            this.Numpad.Size = new System.Drawing.Size(300, 330);
            this.Numpad.TabIndex = 28;
            this.Numpad.TimerEnabled = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(361, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 29);
            this.label1.TabIndex = 29;
            this.label1.Text = "Amount";
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.BtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnCancel.BackgroundImage")));
            this.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.ForeColor = System.Drawing.Color.Transparent;
            this.BtnCancel.Location = new System.Drawing.Point(528, 620);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(138, 74);
            this.BtnCancel.TabIndex = 52;
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.Color.Transparent;
            this.BtnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnOk.BackgroundImage")));
            this.BtnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOk.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.ForeColor = System.Drawing.Color.Transparent;
            this.BtnOk.Location = new System.Drawing.Point(366, 620);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(138, 74);
            this.BtnOk.TabIndex = 51;
            this.BtnOk.UseVisualStyleBackColor = false;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 139);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 55;
            this.pictureBox1.TabStop = false;
            // 
            // CustTypeLbl
            // 
            this.CustTypeLbl.AutoSize = true;
            this.CustTypeLbl.Font = new System.Drawing.Font("Comic Sans MS", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustTypeLbl.ForeColor = System.Drawing.Color.Black;
            this.CustTypeLbl.Location = new System.Drawing.Point(390, 64);
            this.CustTypeLbl.Name = "CustTypeLbl";
            this.CustTypeLbl.Size = new System.Drawing.Size(259, 74);
            this.CustTypeLbl.TabIndex = 56;
            this.CustTypeLbl.Text = "Pay Card";
            // 
            // txtApprovalCode
            // 
            this.txtApprovalCode.BackColor = System.Drawing.Color.Ivory;
            this.txtApprovalCode.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApprovalCode.Location = new System.Drawing.Point(366, 206);
            this.txtApprovalCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtApprovalCode.Name = "txtApprovalCode";
            this.txtApprovalCode.Size = new System.Drawing.Size(300, 34);
            this.txtApprovalCode.TabIndex = 57;
            this.txtApprovalCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(361, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 29);
            this.label2.TabIndex = 58;
            this.label2.Text = "Approval code";
            // 
            // frmPayCardOfflineApproval
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtApprovalCode);
            this.Controls.Add(this.CustTypeLbl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Numpad);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmPayCardOfflineApproval";
            this.Text = "frmPayCardOfflineApproval";
            this.Controls.SetChildIndex(this.Numpad, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.BtnOk, 0);
            this.Controls.SetChildIndex(this.BtnCancel, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.CustTypeLbl, 0);
            this.Controls.SetChildIndex(this.txtApprovalCode, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private DevExpress.XtraEditors.StyleController styleController;
        private LSRetailPosis.POSProcesses.WinControls.NumPad Numpad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.PictureBox pictureBox1;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Label CustTypeLbl;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.TextBox txtApprovalCode;
        private System.Windows.Forms.Label label2;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;

        #endregion

        //private DevExpress.XtraEditors.StyleController styleController;
    }
}