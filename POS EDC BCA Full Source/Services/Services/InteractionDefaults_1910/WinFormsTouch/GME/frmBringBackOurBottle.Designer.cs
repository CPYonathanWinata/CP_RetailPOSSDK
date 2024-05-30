namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    partial class frmBringBackOurBottle
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
            this.Numpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.SuspendLayout();
            // 
            // Numpad
            // 
            this.Numpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Numpad.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.Numpad.Location = new System.Drawing.Point(120, 314);
            this.Numpad.MaskChar = "";
            this.Numpad.MaskInterval = 0;
            this.Numpad.MaxNumberOfDigits = 9;
            this.Numpad.MinimumSize = new System.Drawing.Size(300, 300);
            this.Numpad.Name = "Numpad";
            this.Numpad.NegativeMode = false;
            this.Numpad.NoOfTries = 0;
            this.Numpad.NumberOfDecimals = 2;
            this.Numpad.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Numpad.PromptText = null;
            this.Numpad.ShortcutKeysActive = false;
            this.Numpad.Size = new System.Drawing.Size(300, 300);
            this.Numpad.TabIndex = 8;
            this.Numpad.TimerEnabled = true;
            // 
            // frmBringBackOurBottle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.Numpad);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmBringBackOurBottle";
            this.Text = "frmBringBackOurBottle";
            this.Controls.SetChildIndex(this.Numpad, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LSRetailPosis.POSProcesses.WinControls.NumPad Numpad;
        //private DevExpress.XtraEditors.StyleController styleController;
    }
}