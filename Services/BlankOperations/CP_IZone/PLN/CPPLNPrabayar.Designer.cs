using System.Drawing;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.IZone.PLN
{
    partial class CPPLNPrabayar
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
            this.header = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.parentPanel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.errLbl = new System.Windows.Forms.Label();
            this.inputBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIDMeter = new System.Windows.Forms.TextBox();
            this.lblIDMeter = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.nominalBox = new System.Windows.Forms.ComboBox();
            this.lblNominal = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtTotalAmt = new System.Windows.Forms.TextBox();
            this.txtAdminAmt = new System.Windows.Forms.TextBox();
            this.txtStroom = new System.Windows.Forms.TextBox();
            this.txtTarifDaya = new System.Windows.Forms.TextBox();
            this.txtNamaPelanggan = new System.Windows.Forms.TextBox();
            this.txtIdPelanggan = new System.Windows.Forms.TextBox();
            this.txtNoMeter = new System.Windows.Forms.TextBox();
            this.lblNoMeter = new System.Windows.Forms.Label();
            this.lblTarif = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblAdmin = new System.Windows.Forms.Label();
            this.lblStroom = new System.Windows.Forms.Label();
            this.lblIdPelanggan = new System.Windows.Forms.Label();
            this.lblNama = new System.Windows.Forms.Label();
            this.lblHeader3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.dropDownButton1 = new DevExpress.XtraEditors.DropDownButton();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.parentPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(359, 38);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(314, 65);
            this.header.TabIndex = 16;
            this.header.Text = "PLN Prabayar";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(370, 684);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(127, 57);
            this.btnBack.TabIndex = 18;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnNext.Location = new System.Drawing.Point(566, 684);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(127, 57);
            this.btnNext.TabIndex = 19;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // parentPanel
            // 
            this.parentPanel.Controls.Add(this.tabControl);
            this.parentPanel.Location = new System.Drawing.Point(36, 151);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(959, 527);
            this.parentPanel.TabIndex = 21;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Location = new System.Drawing.Point(0, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(959, 517);
            this.tabControl.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.errLbl);
            this.tabPage1.Controls.Add(this.inputBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtIDMeter);
            this.tabPage1.Controls.Add(this.lblIDMeter);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(951, 491);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // errLbl
            // 
            this.errLbl.AutoSize = true;
            this.errLbl.ForeColor = System.Drawing.Color.Red;
            this.errLbl.Location = new System.Drawing.Point(299, 259);
            this.errLbl.Name = "errLbl";
            this.errLbl.Size = new System.Drawing.Size(139, 13);
            this.errLbl.TabIndex = 26;
            this.errLbl.Text = "Input dengan format angka";
            this.errLbl.Visible = false;
            // 
            // inputBox
            // 
            this.inputBox.Font = new System.Drawing.Font("Tahoma", 12F);
            this.inputBox.FormattingEnabled = true;
            this.inputBox.Items.AddRange(new object[] {
            "ID Pelanggan",
            "No. Meter"});
            this.inputBox.Location = new System.Drawing.Point(159, 192);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(121, 27);
            this.inputBox.TabIndex = 25;
            this.inputBox.SelectedIndexChanged += new System.EventHandler(this.inputBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F);
            this.label3.Location = new System.Drawing.Point(155, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 24;
            this.label3.Text = "Input by :";
            // 
            // txtIDMeter
            // 
            this.txtIDMeter.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtIDMeter.Location = new System.Drawing.Point(159, 276);
            this.txtIDMeter.MaxLength = 12;
            this.txtIDMeter.Name = "txtIDMeter";
            this.txtIDMeter.Size = new System.Drawing.Size(499, 30);
            this.txtIDMeter.TabIndex = 2;
            this.txtIDMeter.TextChanged += new System.EventHandler(this.txtIDMeter_TextChanged);
            // 
            // lblIDMeter
            // 
            this.lblIDMeter.AutoSize = true;
            this.lblIDMeter.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblIDMeter.Location = new System.Drawing.Point(155, 250);
            this.lblIDMeter.Name = "lblIDMeter";
            this.lblIDMeter.Size = new System.Drawing.Size(137, 23);
            this.lblIDMeter.TabIndex = 1;
            this.lblIDMeter.Text = "ID Pelanggan :";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.nominalBox);
            this.tabPage2.Controls.Add(this.lblNominal);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(951, 491);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // nominalBox
            // 
            this.nominalBox.BackColor = System.Drawing.SystemColors.Window;
            this.nominalBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nominalBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.nominalBox.FormattingEnabled = true;
            this.nominalBox.ItemHeight = 24;
            this.nominalBox.Location = new System.Drawing.Point(218, 151);
            this.nominalBox.Name = "nominalBox";
            this.nominalBox.Size = new System.Drawing.Size(499, 32);
            this.nominalBox.TabIndex = 6;
            // 
            // lblNominal
            // 
            this.lblNominal.AutoSize = true;
            this.lblNominal.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblNominal.Location = new System.Drawing.Point(214, 113);
            this.lblNominal.Name = "lblNominal";
            this.lblNominal.Size = new System.Drawing.Size(130, 23);
            this.lblNominal.TabIndex = 3;
            this.lblNominal.Text = "Pilih Nominal :";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Controls.Add(this.lblHeader3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(951, 491);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.89957F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.10044F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tableLayoutPanel1.Controls.Add(this.txtTotalAmt, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtAdminAmt, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtStroom, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtTarifDaya, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtNamaPelanggan, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtIdPelanggan, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNoMeter, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNoMeter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTarif, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTotal, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblAdmin, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblStroom, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblIdPelanggan, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblNama, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(211, 112);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 268);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // txtTotalAmt
            // 
            this.txtTotalAmt.Enabled = false;
            this.txtTotalAmt.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtTotalAmt.Location = new System.Drawing.Point(213, 231);
            this.txtTotalAmt.Name = "txtTotalAmt";
            this.txtTotalAmt.Size = new System.Drawing.Size(227, 30);
            this.txtTotalAmt.TabIndex = 14;
            // 
            // txtAdminAmt
            // 
            this.txtAdminAmt.Enabled = false;
            this.txtAdminAmt.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtAdminAmt.Location = new System.Drawing.Point(213, 193);
            this.txtAdminAmt.Name = "txtAdminAmt";
            this.txtAdminAmt.Size = new System.Drawing.Size(227, 30);
            this.txtAdminAmt.TabIndex = 14;
            // 
            // txtStroom
            // 
            this.txtStroom.Enabled = false;
            this.txtStroom.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtStroom.Location = new System.Drawing.Point(213, 155);
            this.txtStroom.Name = "txtStroom";
            this.txtStroom.Size = new System.Drawing.Size(227, 30);
            this.txtStroom.TabIndex = 14;
            // 
            // txtTarifDaya
            // 
            this.txtTarifDaya.Enabled = false;
            this.txtTarifDaya.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtTarifDaya.Location = new System.Drawing.Point(213, 117);
            this.txtTarifDaya.Name = "txtTarifDaya";
            this.txtTarifDaya.Size = new System.Drawing.Size(227, 30);
            this.txtTarifDaya.TabIndex = 14;
            // 
            // txtNamaPelanggan
            // 
            this.txtNamaPelanggan.Enabled = false;
            this.txtNamaPelanggan.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtNamaPelanggan.Location = new System.Drawing.Point(213, 79);
            this.txtNamaPelanggan.Name = "txtNamaPelanggan";
            this.txtNamaPelanggan.Size = new System.Drawing.Size(227, 30);
            this.txtNamaPelanggan.TabIndex = 14;
            // 
            // txtIdPelanggan
            // 
            this.txtIdPelanggan.Enabled = false;
            this.txtIdPelanggan.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtIdPelanggan.Location = new System.Drawing.Point(213, 41);
            this.txtIdPelanggan.Name = "txtIdPelanggan";
            this.txtIdPelanggan.Size = new System.Drawing.Size(227, 30);
            this.txtIdPelanggan.TabIndex = 22;
            // 
            // txtNoMeter
            // 
            this.txtNoMeter.Enabled = false;
            this.txtNoMeter.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtNoMeter.Location = new System.Drawing.Point(213, 3);
            this.txtNoMeter.Name = "txtNoMeter";
            this.txtNoMeter.Size = new System.Drawing.Size(227, 30);
            this.txtNoMeter.TabIndex = 13;
            // 
            // lblNoMeter
            // 
            this.lblNoMeter.AutoSize = true;
            this.lblNoMeter.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblNoMeter.Location = new System.Drawing.Point(3, 0);
            this.lblNoMeter.Name = "lblNoMeter";
            this.lblNoMeter.Size = new System.Drawing.Size(102, 23);
            this.lblNoMeter.TabIndex = 5;
            this.lblNoMeter.Text = "NO METER";
            // 
            // lblTarif
            // 
            this.lblTarif.AutoSize = true;
            this.lblTarif.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblTarif.Location = new System.Drawing.Point(3, 114);
            this.lblTarif.Name = "lblTarif";
            this.lblTarif.Size = new System.Drawing.Size(114, 23);
            this.lblTarif.TabIndex = 8;
            this.lblTarif.Text = "TARIF/DAYA";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblTotal.Location = new System.Drawing.Point(3, 228);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(146, 23);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "TOTAL AMOUNT";
            // 
            // lblAdmin
            // 
            this.lblAdmin.AutoSize = true;
            this.lblAdmin.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblAdmin.Location = new System.Drawing.Point(3, 190);
            this.lblAdmin.Name = "lblAdmin";
            this.lblAdmin.Size = new System.Drawing.Size(69, 23);
            this.lblAdmin.TabIndex = 10;
            this.lblAdmin.Text = "ADMIN";
            // 
            // lblStroom
            // 
            this.lblStroom.AutoSize = true;
            this.lblStroom.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblStroom.Location = new System.Drawing.Point(3, 152);
            this.lblStroom.Name = "lblStroom";
            this.lblStroom.Size = new System.Drawing.Size(151, 38);
            this.lblStroom.TabIndex = 11;
            this.lblStroom.Text = "RP STROOM/TOKEN";
            // 
            // lblIdPelanggan
            // 
            this.lblIdPelanggan.AutoSize = true;
            this.lblIdPelanggan.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblIdPelanggan.Location = new System.Drawing.Point(3, 38);
            this.lblIdPelanggan.Name = "lblIdPelanggan";
            this.lblIdPelanggan.Size = new System.Drawing.Size(140, 23);
            this.lblIdPelanggan.TabIndex = 6;
            this.lblIdPelanggan.Text = "ID PELANGGAN";
            // 
            // lblNama
            // 
            this.lblNama.AutoSize = true;
            this.lblNama.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblNama.Location = new System.Drawing.Point(3, 76);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(60, 23);
            this.lblNama.TabIndex = 7;
            this.lblNama.Text = "NAMA";
            // 
            // lblHeader3
            // 
            this.lblHeader3.AutoSize = true;
            this.lblHeader3.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblHeader3.Location = new System.Drawing.Point(326, 42);
            this.lblHeader3.Name = "lblHeader3";
            this.lblHeader3.Size = new System.Drawing.Size(277, 23);
            this.lblHeader3.TabIndex = 4;
            this.lblHeader3.Text = "PEMBELIAN LISTRIK PRABAYAR";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(95, 684);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnFinish.FlatAppearance.BorderSize = 0;
            this.btnFinish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinish.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFinish.Location = new System.Drawing.Point(808, 684);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(127, 57);
            this.btnFinish.TabIndex = 23;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Location = new System.Drawing.Point(0, 0);
            this.dropDownButton1.Name = "dropDownButton1";
            this.dropDownButton1.TabIndex = 0;
            // 
            // CPPLNPrabayar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.parentPanel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.header);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CPPLNPrabayar";
            this.Text = "CPPLNPrabayar";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPPLNPrabayar_FormClosed);
            this.Controls.SetChildIndex(this.header, 0);
            this.Controls.SetChildIndex(this.btnBack, 0);
            this.Controls.SetChildIndex(this.btnNext, 0);
            this.Controls.SetChildIndex(this.parentPanel, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnFinish, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.parentPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      
        
      

        #endregion

        //private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel parentPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtIDMeter;
        private System.Windows.Forms.Label lblIDMeter;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label lblNominal;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblStroom;
        private System.Windows.Forms.Label lblAdmin;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTarif;
        private System.Windows.Forms.Label lblHeader3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblNoMeter;
        private System.Windows.Forms.Label lblIdPelanggan;
        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.TextBox txtTotalAmt;
        private System.Windows.Forms.TextBox txtAdminAmt;
        private System.Windows.Forms.TextBox txtStroom;
        private System.Windows.Forms.TextBox txtTarifDaya;
        private System.Windows.Forms.TextBox txtNamaPelanggan;
        private System.Windows.Forms.TextBox txtIdPelanggan;
        private System.Windows.Forms.TextBox txtNoMeter;
        private DevExpress.XtraEditors.DropDownButton dropDownButton1;
        private System.Windows.Forms.ComboBox nominalBox;
        private System.Windows.Forms.ComboBox inputBox;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Label errLbl;
    }
}