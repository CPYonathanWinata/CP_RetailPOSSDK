namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class frmPaymentCash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaymentCash));
            //this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.LblTotalAmountDue = new System.Windows.Forms.Label();
            this.LblAmountDue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnAmount400K = new System.Windows.Forms.Button();
            this.BtnAmount350K = new System.Windows.Forms.Button();
            this.BtnAmount300K = new System.Windows.Forms.Button();
            this.BtnAmount250K = new System.Windows.Forms.Button();
            this.BtnAmount200K = new System.Windows.Forms.Button();
            this.BtnAmount150K = new System.Windows.Forms.Button();
            this.BtnAmount100K = new System.Windows.Forms.Button();
            this.BtnAmountDue = new System.Windows.Forms.Button();
            this.Numpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnAmount500K = new System.Windows.Forms.Button();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // LblTotalAmountDue
            // 
            this.LblTotalAmountDue.AutoSize = true;
            this.LblTotalAmountDue.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTotalAmountDue.Location = new System.Drawing.Point(312, 12);
            this.LblTotalAmountDue.Name = "LblTotalAmountDue";
            this.LblTotalAmountDue.Size = new System.Drawing.Size(469, 59);
            this.LblTotalAmountDue.TabIndex = 29;
            this.LblTotalAmountDue.Text = "Total amount due : IDR";
            // 
            // LblAmountDue
            // 
            this.LblAmountDue.AutoSize = true;
            this.LblAmountDue.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAmountDue.Location = new System.Drawing.Point(767, 12);
            this.LblAmountDue.Name = "LblAmountDue";
            this.LblAmountDue.Size = new System.Drawing.Size(0, 59);
            this.LblAmountDue.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 26);
            this.label1.TabIndex = 48;
            this.label1.Text = "Payment amount";
            // 
            // BtnAmount400K
            // 
            this.BtnAmount400K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount400K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount400K.Location = new System.Drawing.Point(193, 345);
            this.BtnAmount400K.Name = "BtnAmount400K";
            this.BtnAmount400K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount400K.TabIndex = 47;
            this.BtnAmount400K.Text = "400,000";
            this.BtnAmount400K.UseVisualStyleBackColor = false;
            this.BtnAmount400K.Click += new System.EventHandler(this.BtnAmount400K_Click);
            // 
            // BtnAmount350K
            // 
            this.BtnAmount350K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount350K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount350K.Location = new System.Drawing.Point(193, 264);
            this.BtnAmount350K.Name = "BtnAmount350K";
            this.BtnAmount350K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount350K.TabIndex = 46;
            this.BtnAmount350K.Text = "350,000";
            this.BtnAmount350K.UseVisualStyleBackColor = false;
            this.BtnAmount350K.Click += new System.EventHandler(this.BtnAmount350K_Click);
            // 
            // BtnAmount300K
            // 
            this.BtnAmount300K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount300K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount300K.Location = new System.Drawing.Point(193, 183);
            this.BtnAmount300K.Name = "BtnAmount300K";
            this.BtnAmount300K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount300K.TabIndex = 45;
            this.BtnAmount300K.Text = "300,000";
            this.BtnAmount300K.UseVisualStyleBackColor = false;
            this.BtnAmount300K.Click += new System.EventHandler(this.BtnAmount300K_Click);
            // 
            // BtnAmount250K
            // 
            this.BtnAmount250K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount250K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount250K.Location = new System.Drawing.Point(390, 426);
            this.BtnAmount250K.Name = "BtnAmount250K";
            this.BtnAmount250K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount250K.TabIndex = 44;
            this.BtnAmount250K.Text = "250,000";
            this.BtnAmount250K.UseVisualStyleBackColor = false;
            this.BtnAmount250K.Click += new System.EventHandler(this.BtnAmount250K_Click);
            // 
            // BtnAmount200K
            // 
            this.BtnAmount200K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount200K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount200K.Location = new System.Drawing.Point(390, 345);
            this.BtnAmount200K.Name = "BtnAmount200K";
            this.BtnAmount200K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount200K.TabIndex = 43;
            this.BtnAmount200K.Text = "200,000";
            this.BtnAmount200K.UseVisualStyleBackColor = false;
            this.BtnAmount200K.Click += new System.EventHandler(this.BtnAmount200K_Click);
            // 
            // BtnAmount150K
            // 
            this.BtnAmount150K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount150K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount150K.Location = new System.Drawing.Point(390, 264);
            this.BtnAmount150K.Name = "BtnAmount150K";
            this.BtnAmount150K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount150K.TabIndex = 42;
            this.BtnAmount150K.Text = "150,000";
            this.BtnAmount150K.UseVisualStyleBackColor = false;
            this.BtnAmount150K.Click += new System.EventHandler(this.BtnAmount150K_Click);
            // 
            // BtnAmount100K
            // 
            this.BtnAmount100K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount100K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount100K.Location = new System.Drawing.Point(390, 183);
            this.BtnAmount100K.Name = "BtnAmount100K";
            this.BtnAmount100K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount100K.TabIndex = 41;
            this.BtnAmount100K.Text = "100,000";
            this.BtnAmount100K.UseVisualStyleBackColor = false;
            this.BtnAmount100K.Click += new System.EventHandler(this.BtnAmount100K_Click);
            // 
            // BtnAmountDue
            // 
            this.BtnAmountDue.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmountDue.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmountDue.Location = new System.Drawing.Point(647, 183);
            this.BtnAmountDue.Name = "BtnAmountDue";
            this.BtnAmountDue.Size = new System.Drawing.Size(156, 70);
            this.BtnAmountDue.TabIndex = 40;
            this.BtnAmountDue.UseVisualStyleBackColor = false;
            this.BtnAmountDue.Click += new System.EventHandler(this.BtnAmountDue_Click);
            // 
            // Numpad
            // 
            this.Numpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Numpad.Appearance.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.Numpad.Location = new System.Drawing.Point(880, 166);
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
            this.Numpad.TabIndex = 27;
            this.Numpad.TimerEnabled = true;
            this.Numpad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad1_EnterButtonPressed);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnCancel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Location = new System.Drawing.Point(758, 539);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(127, 52);
            this.BtnCancel.TabIndex = 50;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnOk.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.Location = new System.Drawing.Point(547, 539);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(127, 52);
            this.BtnOk.TabIndex = 49;
            this.BtnOk.Text = "OK";
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
            this.pictureBox1.TabIndex = 51;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1217, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(147, 139);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 52;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(12, 621);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(1352, 147);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 53;
            this.pictureBox4.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(963, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 26);
            this.label2.TabIndex = 54;
            this.label2.Text = "Input amount";
            // 
            // BtnAmount500K
            // 
            this.BtnAmount500K.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnAmount500K.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAmount500K.Location = new System.Drawing.Point(193, 426);
            this.BtnAmount500K.Name = "BtnAmount500K";
            this.BtnAmount500K.Size = new System.Drawing.Size(156, 70);
            this.BtnAmount500K.TabIndex = 55;
            this.BtnAmount500K.Text = "500,000";
            this.BtnAmount500K.UseVisualStyleBackColor = false;
            this.BtnAmount500K.Click += new System.EventHandler(this.BtnAmount500K_Click);
            // 
            // frmPaymentCash
            // 
            this.Appearance.BackColor = System.Drawing.Color.LemonChiffon;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 780);
            this.Controls.Add(this.BtnAmount500K);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.Numpad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnAmount400K);
            this.Controls.Add(this.BtnAmount350K);
            this.Controls.Add(this.BtnAmount300K);
            this.Controls.Add(this.BtnAmount250K);
            this.Controls.Add(this.BtnAmount200K);
            this.Controls.Add(this.BtnAmount150K);
            this.Controls.Add(this.BtnAmount100K);
            this.Controls.Add(this.BtnAmountDue);
            this.Controls.Add(this.LblAmountDue);
            this.Controls.Add(this.LblTotalAmountDue);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmPaymentCash";
            this.Text = "frmPaymentCash";
            this.Controls.SetChildIndex(this.LblTotalAmountDue, 0);
            this.Controls.SetChildIndex(this.LblAmountDue, 0);
            this.Controls.SetChildIndex(this.BtnAmountDue, 0);
            this.Controls.SetChildIndex(this.BtnAmount100K, 0);
            this.Controls.SetChildIndex(this.BtnAmount150K, 0);
            this.Controls.SetChildIndex(this.BtnAmount200K, 0);
            this.Controls.SetChildIndex(this.BtnAmount250K, 0);
            this.Controls.SetChildIndex(this.BtnAmount300K, 0);
            this.Controls.SetChildIndex(this.BtnAmount350K, 0);
            this.Controls.SetChildIndex(this.BtnAmount400K, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.Numpad, 0);
            this.Controls.SetChildIndex(this.BtnOk, 0);
            this.Controls.SetChildIndex(this.BtnCancel, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.pictureBox2, 0);
            this.Controls.SetChildIndex(this.pictureBox4, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.BtnAmount500K, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Label LblTotalAmountDue;
        private System.Windows.Forms.Label LblAmountDue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnAmount400K;
        private System.Windows.Forms.Button BtnAmount350K;
        private System.Windows.Forms.Button BtnAmount300K;
        private System.Windows.Forms.Button BtnAmount250K;
        private System.Windows.Forms.Button BtnAmount200K;
        private System.Windows.Forms.Button BtnAmount150K;
        private System.Windows.Forms.Button BtnAmount100K;
        private System.Windows.Forms.Button BtnAmountDue;
        private LSRetailPosis.POSProcesses.WinControls.NumPad Numpad;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label2;
        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Button BtnAmount500K;
        //private DevExpress.XtraEditors.StyleController styleController;
        //private DevExpress.XtraEditors.StyleController styleController;
    }
}