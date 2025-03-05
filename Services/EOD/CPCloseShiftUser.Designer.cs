using System.Windows.Forms;
namespace Microsoft.Dynamics.Retail.Pos.EOD
{
    partial class CPCloseShiftUser
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
            //this.FormBorderStyle = FormBorderStyle.FixedDialog; // Prevent resizing
            this.FormBorderStyle = FormBorderStyle.None;

            this.MinimizeBox = false;  // Hide Minimize button
            this.MaximizeBox = false;  // Hide Maximize button
            
            this.confirmBtn = new System.Windows.Forms.Button();
            this.closedBox = new System.Windows.Forms.ComboBox();
            this.closeLbl = new System.Windows.Forms.Label();
            this.storedLbl = new System.Windows.Forms.Label();
            this.storedBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // confirmBtn
            // 
            this.confirmBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmBtn.Location = new System.Drawing.Point(137, 238);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(107, 43);
            this.confirmBtn.TabIndex = 0;
            this.confirmBtn.Text = "Confirm";
            this.confirmBtn.UseVisualStyleBackColor = true;
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // closedBox
            // 
            this.closedBox.Font = new System.Drawing.Font("Calibri", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closedBox.FormattingEnabled = true;
            this.closedBox.Location = new System.Drawing.Point(12, 70);
            this.closedBox.Name = "closedBox";
            this.closedBox.Size = new System.Drawing.Size(364, 32);
            this.closedBox.TabIndex = 1;
            this.closedBox.SelectedIndexChanged += new System.EventHandler(this.ClosedBox_SelectedIndexChanged);
            this.closedBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // 
            // closeLbl
            // 
            this.closeLbl.AutoSize = true;
            this.closeLbl.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeLbl.Location = new System.Drawing.Point(12, 38);
            this.closeLbl.Name = "closeLbl";
            this.closeLbl.Size = new System.Drawing.Size(182, 29);
            this.closeLbl.TabIndex = 2;
            this.closeLbl.Text = "Closed Shift Oleh";
            // 
            // storedLbl
            // 
            this.storedLbl.AutoSize = true;
            this.storedLbl.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storedLbl.Location = new System.Drawing.Point(12, 128);
            this.storedLbl.Name = "storedLbl";
            this.storedLbl.Size = new System.Drawing.Size(171, 29);
            this.storedLbl.TabIndex = 4;
            this.storedLbl.Text = "Setor Cash Oleh";
            // 
            // storedBox
            // 
            this.storedBox.Font = new System.Drawing.Font("Calibri", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storedBox.FormattingEnabled = true;
            this.storedBox.Location = new System.Drawing.Point(12, 160);
            this.storedBox.Name = "storedBox";
            this.storedBox.Size = new System.Drawing.Size(364, 32);
            this.storedBox.TabIndex = 3;
            this.storedBox.SelectedIndexChanged += new System.EventHandler(this.storedBox_SelectedIndexChanged);
            this.storedBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // CPCloseShiftUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 297);
            this.Controls.Add(this.storedLbl);
            this.Controls.Add(this.storedBox);
            this.Controls.Add(this.closeLbl);
            this.Controls.Add(this.closedBox);
            this.Controls.Add(this.confirmBtn);
            this.Name = "CPCloseShiftUser";
            this.Text = "Close Shift";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.ComboBox closedBox;
        private System.Windows.Forms.Label closeLbl;
        private System.Windows.Forms.Label storedLbl;
        private System.Windows.Forms.ComboBox storedBox;
    }
}