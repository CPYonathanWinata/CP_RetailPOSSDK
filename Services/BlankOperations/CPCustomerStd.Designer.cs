namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPCustomerStd
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
            this.lblCustomerStd = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCustomerStd
            // 
            this.lblCustomerStd.AutoSize = true;
            this.lblCustomerStd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerStd.Location = new System.Drawing.Point(3, 0);
            this.lblCustomerStd.Name = "lblCustomerStd";
            this.lblCustomerStd.Size = new System.Drawing.Size(105, 15);
            this.lblCustomerStd.TabIndex = 0;
            this.lblCustomerStd.Text = "lblCustomerStd";
            // 
            // CPCustomerStd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCustomerStd);
            this.Name = "CPCustomerStd";
            this.Size = new System.Drawing.Size(337, 125);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCustomerStd;
    }
}
