namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPIZone
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
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.closeBtn = new System.Windows.Forms.Button();
            this.header = new System.Windows.Forms.Label();
            this.btnPLNPra = new System.Windows.Forms.Button();
            this.btnPLNPasca = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.closeBtn.Location = new System.Drawing.Point(450, 691);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(127, 57);
            this.closeBtn.TabIndex = 15;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(424, 30);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(166, 65);
            this.header.TabIndex = 14;
            this.header.Text = "IZONE";
            // 
            // btnPLNPra
            // 
            this.btnPLNPra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnPLNPra.FlatAppearance.BorderSize = 0;
            this.btnPLNPra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPLNPra.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPLNPra.Location = new System.Drawing.Point(322, 269);
            this.btnPLNPra.Name = "btnPLNPra";
            this.btnPLNPra.Size = new System.Drawing.Size(380, 57);
            this.btnPLNPra.TabIndex = 16;
            this.btnPLNPra.Text = "PLN Prabayar";
            this.btnPLNPra.UseVisualStyleBackColor = false;
            this.btnPLNPra.Click += new System.EventHandler(this.btnPLNPra_Click);
            // 
            // btnPLNPasca
            // 
            this.btnPLNPasca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnPLNPasca.FlatAppearance.BorderSize = 0;
            this.btnPLNPasca.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPLNPasca.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPLNPasca.Location = new System.Drawing.Point(322, 356);
            this.btnPLNPasca.Name = "btnPLNPasca";
            this.btnPLNPasca.Size = new System.Drawing.Size(380, 57);
            this.btnPLNPasca.TabIndex = 17;
            this.btnPLNPasca.Text = "PLN Pascabayar";
            this.btnPLNPasca.UseVisualStyleBackColor = false;
            this.btnPLNPasca.Click += new System.EventHandler(this.btnPLNPasca_Click);
            // 
            // CPIZone
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.btnPLNPasca);
            this.Controls.Add(this.btnPLNPra);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.header);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CPIZone";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CPIZone";
            this.Controls.SetChildIndex(this.header, 0);
            this.Controls.SetChildIndex(this.closeBtn, 0);
            this.Controls.SetChildIndex(this.btnPLNPra, 0);
            this.Controls.SetChildIndex(this.btnPLNPasca, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Button btnPLNPra;
        private System.Windows.Forms.Button btnPLNPasca;
    }
}