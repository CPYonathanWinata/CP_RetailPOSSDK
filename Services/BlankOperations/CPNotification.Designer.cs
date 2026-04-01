namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPNotification
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSales = new System.Windows.Forms.Button();
            this.lblRTS = new System.Windows.Forms.Label();
            this.lblAPI = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(96, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(82, 41);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSales
            // 
            this.btnSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSales.Location = new System.Drawing.Point(3, 3);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(87, 42);
            this.btnSales.TabIndex = 1;
            this.btnSales.Text = "Sales";
            this.btnSales.UseVisualStyleBackColor = true;
            this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
            // 
            // lblRTS
            // 
            this.lblRTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRTS.Location = new System.Drawing.Point(132, 43);
            this.lblRTS.Name = "lblRTS";
            this.lblRTS.Size = new System.Drawing.Size(74, 14);
            this.lblRTS.TabIndex = 2;
            this.lblRTS.Text = "RTS Status";
            this.lblRTS.Visible = false;
            // 
            // lblAPI
            // 
            this.lblAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAPI.Location = new System.Drawing.Point(58, 47);
            this.lblAPI.Name = "lblAPI";
            this.lblAPI.Size = new System.Drawing.Size(74, 10);
            this.lblAPI.TabIndex = 3;
            this.lblAPI.Text = "API Status";
            this.lblAPI.Visible = false;
            // 
            // CPNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAPI);
            this.Controls.Add(this.lblRTS);
            this.Controls.Add(this.btnSales);
            this.Controls.Add(this.btnRefresh);
            this.Name = "CPNotification";
            this.Size = new System.Drawing.Size(197, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Label lblRTS;
        private System.Windows.Forms.Label lblAPI;
    }
}
