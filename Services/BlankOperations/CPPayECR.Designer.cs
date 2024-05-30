namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPPayECR
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
            this.btn_Pay = new System.Windows.Forms.Button();
            this.btn_Settlment = new System.Windows.Forms.Button();
            this.btn_connection = new System.Windows.Forms.Button();
            this.bank_cb = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btn_Pay
            // 
            this.btn_Pay.Location = new System.Drawing.Point(52, 70);
            this.btn_Pay.Name = "btn_Pay";
            this.btn_Pay.Size = new System.Drawing.Size(110, 23);
            this.btn_Pay.TabIndex = 0;
            this.btn_Pay.Text = "Pay ECR";
            this.btn_Pay.UseVisualStyleBackColor = true;
            this.btn_Pay.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Settlment
            // 
            this.btn_Settlment.Location = new System.Drawing.Point(52, 108);
            this.btn_Settlment.Name = "btn_Settlment";
            this.btn_Settlment.Size = new System.Drawing.Size(110, 23);
            this.btn_Settlment.TabIndex = 1;
            this.btn_Settlment.Text = "Settelmen";
            this.btn_Settlment.UseVisualStyleBackColor = true;
            this.btn_Settlment.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_connection
            // 
            this.btn_connection.Location = new System.Drawing.Point(52, 147);
            this.btn_connection.Name = "btn_connection";
            this.btn_connection.Size = new System.Drawing.Size(110, 23);
            this.btn_connection.TabIndex = 2;
            this.btn_connection.Text = "Cek Connection";
            this.btn_connection.UseVisualStyleBackColor = true;
            this.btn_connection.Click += new System.EventHandler(this.button3_Click);
            // 
            // bank_cb
            // 
            this.bank_cb.FormattingEnabled = true;
            this.bank_cb.Items.AddRange(new object[] {
            "BCA"});
            this.bank_cb.Location = new System.Drawing.Point(52, 24);
            this.bank_cb.Name = "bank_cb";
            this.bank_cb.Size = new System.Drawing.Size(123, 21);
            this.bank_cb.TabIndex = 3;
            // 
            // CPPayECR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.bank_cb);
            this.Controls.Add(this.btn_connection);
            this.Controls.Add(this.btn_Settlment);
            this.Controls.Add(this.btn_Pay);
            this.Name = "CPPayECR";
            this.Text = "CPPayECR";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Pay;
        private System.Windows.Forms.Button btn_Settlment;
        private System.Windows.Forms.Button btn_connection;
        private System.Windows.Forms.ComboBox bank_cb;
    }
}