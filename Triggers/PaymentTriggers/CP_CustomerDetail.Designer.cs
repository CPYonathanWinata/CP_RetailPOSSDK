namespace PaymentTriggers
{
    partial class CP_CustomerDetail
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
            this.okBtn = new System.Windows.Forms.Button();
            this.headerLbl = new System.Windows.Forms.Label();
            this.idTypeBox = new System.Windows.Forms.ComboBox();
            this.idTypeLbl = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.idText = new System.Windows.Forms.TextBox();
            this.custText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.genderLbl = new System.Windows.Forms.Label();
            this.genderBox = new System.Windows.Forms.ComboBox();
            this.ageText = new System.Windows.Forms.TextBox();
            this.ageLbl = new System.Windows.Forms.Label();
            this.errLbl1 = new System.Windows.Forms.Label();
            this.errLbl2 = new System.Windows.Forms.Label();
            this.errLbl3 = new System.Windows.Forms.Label();
            this.errLbl4 = new System.Windows.Forms.Label();
            this.errLbl5 = new System.Windows.Forms.Label();
            this.errLbl6 = new System.Windows.Forms.Label();
            this.countryLabel = new System.Windows.Forms.Label();
            this.natComboBox = new System.Windows.Forms.ComboBox();
            this.tahunLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okBtn.Location = new System.Drawing.Point(100, 473);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(438, 37);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // headerLbl
            // 
            this.headerLbl.AutoSize = true;
            this.headerLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLbl.Location = new System.Drawing.Point(186, 20);
            this.headerLbl.Name = "headerLbl";
            this.headerLbl.Size = new System.Drawing.Size(275, 31);
            this.headerLbl.TabIndex = 2;
            this.headerLbl.Text = "Customer Information";
            // 
            // idTypeBox
            // 
            this.idTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.idTypeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idTypeBox.FormattingEnabled = true;
            this.idTypeBox.Items.AddRange(new object[] {
            "None",
            "NPWP",
            "KTP",
            "PASPOR"});
            this.idTypeBox.Location = new System.Drawing.Point(96, 122);
            this.idTypeBox.Name = "idTypeBox";
            this.idTypeBox.Size = new System.Drawing.Size(121, 32);
            this.idTypeBox.TabIndex = 3;
            // 
            // idTypeLbl
            // 
            this.idTypeLbl.AutoSize = true;
            this.idTypeLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.idTypeLbl.Location = new System.Drawing.Point(96, 97);
            this.idTypeLbl.Name = "idTypeLbl";
            this.idTypeLbl.Size = new System.Drawing.Size(78, 22);
            this.idTypeLbl.TabIndex = 4;
            this.idTypeLbl.Text = "Tipe ID :";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.idLabel.Location = new System.Drawing.Point(96, 170);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(95, 22);
            this.idLabel.TabIndex = 5;
            this.idLabel.Text = "Nomor ID :";
            // 
            // idText
            // 
            this.idText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idText.Location = new System.Drawing.Point(96, 195);
            this.idText.MaxLength = 16;
            this.idText.Name = "idText";
            this.idText.Size = new System.Drawing.Size(442, 29);
            this.idText.TabIndex = 6;
            // 
            // custText
            // 
            this.custText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custText.Location = new System.Drawing.Point(96, 264);
            this.custText.Name = "custText";
            this.custText.Size = new System.Drawing.Size(442, 29);
            this.custText.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(96, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 22);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nama Customer :";
            // 
            // genderLbl
            // 
            this.genderLbl.AutoSize = true;
            this.genderLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.genderLbl.Location = new System.Drawing.Point(96, 382);
            this.genderLbl.Name = "genderLbl";
            this.genderLbl.Size = new System.Drawing.Size(131, 22);
            this.genderLbl.TabIndex = 11;
            this.genderLbl.Text = "Jenis Kelamin :";
            // 
            // genderBox
            // 
            this.genderBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.genderBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genderBox.FormattingEnabled = true;
            this.genderBox.Items.AddRange(new object[] {
            "Pria",
            "Wanita"});
            this.genderBox.Location = new System.Drawing.Point(100, 416);
            this.genderBox.Name = "genderBox";
            this.genderBox.Size = new System.Drawing.Size(161, 32);
            this.genderBox.TabIndex = 12;
            // 
            // ageText
            // 
            this.ageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ageText.Location = new System.Drawing.Point(329, 416);
            this.ageText.MaxLength = 3;
            this.ageText.Name = "ageText";
            this.ageText.Size = new System.Drawing.Size(102, 29);
            this.ageText.TabIndex = 14;
            this.ageText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ageText_KeyPress);
            // 
            // ageLbl
            // 
            this.ageLbl.AutoSize = true;
            this.ageLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ageLbl.Location = new System.Drawing.Point(325, 382);
            this.ageLbl.Name = "ageLbl";
            this.ageLbl.Size = new System.Drawing.Size(56, 22);
            this.ageLbl.TabIndex = 13;
            this.ageLbl.Text = "Usia :";
            // 
            // errLbl1
            // 
            this.errLbl1.AutoSize = true;
            this.errLbl1.Location = new System.Drawing.Point(172, 104);
            this.errLbl1.Name = "errLbl1";
            this.errLbl1.Size = new System.Drawing.Size(19, 13);
            this.errLbl1.TabIndex = 13;
            this.errLbl1.Text = "err";
            // 
            // errLbl2
            // 
            this.errLbl2.AutoSize = true;
            this.errLbl2.Location = new System.Drawing.Point(189, 177);
            this.errLbl2.Name = "errLbl2";
            this.errLbl2.Size = new System.Drawing.Size(19, 13);
            this.errLbl2.TabIndex = 14;
            this.errLbl2.Text = "err";
            // 
            // errLbl3
            // 
            this.errLbl3.AutoSize = true;
            this.errLbl3.Location = new System.Drawing.Point(242, 246);
            this.errLbl3.Name = "errLbl3";
            this.errLbl3.Size = new System.Drawing.Size(19, 13);
            this.errLbl3.TabIndex = 15;
            this.errLbl3.Text = "err";
            // 
            // errLbl4
            // 
            this.errLbl4.AutoSize = true;
            this.errLbl4.Location = new System.Drawing.Point(227, 389);
            this.errLbl4.Name = "errLbl4";
            this.errLbl4.Size = new System.Drawing.Size(19, 13);
            this.errLbl4.TabIndex = 16;
            this.errLbl4.Text = "err";
            // 
            // errLbl5
            // 
            this.errLbl5.AutoSize = true;
            this.errLbl5.Location = new System.Drawing.Point(377, 389);
            this.errLbl5.Name = "errLbl5";
            this.errLbl5.Size = new System.Drawing.Size(19, 13);
            this.errLbl5.TabIndex = 17;
            this.errLbl5.Text = "err";
            // 
            // errLbl6
            // 
            this.errLbl6.AutoSize = true;
            this.errLbl6.Location = new System.Drawing.Point(265, 313);
            this.errLbl6.Name = "errLbl6";
            this.errLbl6.Size = new System.Drawing.Size(19, 13);
            this.errLbl6.TabIndex = 21;
            this.errLbl6.Text = "err";
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.countryLabel.Location = new System.Drawing.Point(96, 306);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(167, 22);
            this.countryLabel.TabIndex = 9;
            this.countryLabel.Text = "Kewarganegaraan :";
            // 
            // natComboBox
            // 
            this.natComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.natComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.natComboBox.FormattingEnabled = true;
            this.natComboBox.Items.AddRange(new object[] {
            "Pria",
            "Wanita"});
            this.natComboBox.Location = new System.Drawing.Point(100, 333);
            this.natComboBox.Name = "natComboBox";
            this.natComboBox.Size = new System.Drawing.Size(438, 32);
            this.natComboBox.TabIndex = 10;
            // 
            // tahunLabel
            // 
            this.tahunLabel.AutoSize = true;
            this.tahunLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tahunLabel.Location = new System.Drawing.Point(437, 423);
            this.tahunLabel.Name = "tahunLabel";
            this.tahunLabel.Size = new System.Drawing.Size(62, 22);
            this.tahunLabel.TabIndex = 23;
            this.tahunLabel.Text = "Tahun";
            // 
            // CP_CustomerDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 530);
            this.Controls.Add(this.tahunLabel);
            this.Controls.Add(this.natComboBox);
            this.Controls.Add(this.errLbl6);
            this.Controls.Add(this.countryLabel);
            this.Controls.Add(this.errLbl5);
            this.Controls.Add(this.errLbl4);
            this.Controls.Add(this.errLbl3);
            this.Controls.Add(this.errLbl2);
            this.Controls.Add(this.errLbl1);
            this.Controls.Add(this.ageText);
            this.Controls.Add(this.ageLbl);
            this.Controls.Add(this.genderBox);
            this.Controls.Add(this.genderLbl);
            this.Controls.Add(this.custText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idText);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.idTypeLbl);
            this.Controls.Add(this.idTypeBox);
            this.Controls.Add(this.headerLbl);
            this.Controls.Add(this.okBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CP_CustomerDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CP_CustomerDetail";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label headerLbl;
        private System.Windows.Forms.ComboBox idTypeBox;
        private System.Windows.Forms.Label idTypeLbl;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.TextBox idText;
        private System.Windows.Forms.TextBox custText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label genderLbl;
        private System.Windows.Forms.ComboBox genderBox;
        private System.Windows.Forms.TextBox ageText;
        private System.Windows.Forms.Label ageLbl;
        private System.Windows.Forms.Label errLbl1;
        private System.Windows.Forms.Label errLbl2;
        private System.Windows.Forms.Label errLbl3;
        private System.Windows.Forms.Label errLbl4;
        private System.Windows.Forms.Label errLbl5;
        private System.Windows.Forms.Label errLbl6;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.ComboBox natComboBox;
        private System.Windows.Forms.Label tahunLabel;
    }
}