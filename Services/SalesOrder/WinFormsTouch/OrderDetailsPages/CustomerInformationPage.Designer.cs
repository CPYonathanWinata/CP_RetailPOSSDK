/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class CustomerInformationPage
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelContact = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new DevExpress.XtraEditors.TextEdit();
            this.labelPhoneContact = new System.Windows.Forms.Label();
            this.textBoxPhoneContact = new DevExpress.XtraEditors.TextEdit();
            this.viewAddressUserControl1 = new Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch.ViewAddressUserControl();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelEmailId = new System.Windows.Forms.Label();
            this.textBoxEmailId = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPhoneContact.Properties)).BeginInit();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxEmailId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.SalesOrder.CustomerInformationViewModel);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelContact, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.viewAddressUserControl1, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelButtons, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(701, 565);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelContact
            // 
            this.labelContact.AutoSize = true;
            this.labelContact.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContact.Location = new System.Drawing.Point(3, 0);
            this.labelContact.Name = "labelContact";
            this.labelContact.Size = new System.Drawing.Size(157, 17);
            this.labelContact.TabIndex = 1;
            this.labelContact.Text = "CONTACT INFORMATION";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelPhoneContact, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxPhoneContact, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(695, 61);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labelName
            // 
            this.labelName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelName.AutoSize = true;
            this.labelName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelName.Location = new System.Drawing.Point(3, 5);
            this.labelName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(55, 21);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(3, 29);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxName.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxName.Properties.Appearance.Options.UseFont = true;
            this.textBoxName.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxName.Properties.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(341, 28);
            this.textBoxName.TabIndex = 4;
            // 
            // labelPhoneContact
            // 
            this.labelPhoneContact.AutoSize = true;
            this.labelPhoneContact.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelPhoneContact.Location = new System.Drawing.Point(350, 5);
            this.labelPhoneContact.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelPhoneContact.Name = "labelPhoneContact";
            this.labelPhoneContact.Size = new System.Drawing.Size(116, 21);
            this.labelPhoneContact.TabIndex = 5;
            this.labelPhoneContact.Text = "Phone number:";
            // 
            // textBoxPhoneContact
            // 
            this.textBoxPhoneContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPhoneContact.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Phone", true));
            this.textBoxPhoneContact.Location = new System.Drawing.Point(350, 29);
            this.textBoxPhoneContact.Name = "textBoxPhoneContact";
            this.textBoxPhoneContact.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxPhoneContact.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPhoneContact.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxPhoneContact.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxPhoneContact.Properties.Appearance.Options.UseFont = true;
            this.textBoxPhoneContact.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxPhoneContact.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxPhoneContact.Properties.ReadOnly = true;
            this.textBoxPhoneContact.Size = new System.Drawing.Size(342, 28);
            this.textBoxPhoneContact.TabIndex = 6;
            // 
            // viewAddressUserControl1
            // 
            this.viewAddressUserControl1.Location = new System.Drawing.Point(3, 87);
            this.viewAddressUserControl1.MinimumSize = new System.Drawing.Size(285, 148);
            this.viewAddressUserControl1.Name = "viewAddressUserControl1";
            this.viewAddressUserControl1.Size = new System.Drawing.Size(695, 148);
            this.viewAddressUserControl1.TabIndex = 8;
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelButtons.ColumnCount = 2;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnSearch, 1, 0);
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(3, 315);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 1;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(695, 247);
            this.tableLayoutPanelButtons.TabIndex = 16;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Appearance.Options.UseFont = true;
            this.btnAdd.Location = new System.Drawing.Point(216, 11);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(127, 57);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.OnBtnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.Location = new System.Drawing.Point(351, 11);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(127, 57);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.OnBtnSearch_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Controls.Add(this.labelEmailId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxEmailId, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 241);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(695, 68);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // labelEmailId
            // 
            this.labelEmailId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelEmailId.AutoSize = true;
            this.labelEmailId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelEmailId.Location = new System.Drawing.Point(3, 5);
            this.labelEmailId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelEmailId.Name = "labelEmailId";
            this.labelEmailId.Size = new System.Drawing.Size(51, 21);
            this.labelEmailId.TabIndex = 14;
            this.labelEmailId.Text = "Email:";
            // 
            // textBoxEmailId
            // 
            this.textBoxEmailId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Email", true));
            this.textBoxEmailId.Location = new System.Drawing.Point(3, 29);
            this.textBoxEmailId.Name = "textBoxEmailId";
            this.textBoxEmailId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxEmailId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEmailId.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxEmailId.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxEmailId.Properties.Appearance.Options.UseFont = true;
            this.textBoxEmailId.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxEmailId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxEmailId.Properties.ReadOnly = true;
            this.textBoxEmailId.Size = new System.Drawing.Size(342, 28);
            this.textBoxEmailId.TabIndex = 15;
            // 
            // CustomerInformationPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CustomerInformationPage";
            this.Text = "Customer information";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPhoneContact.Properties)).EndInit();
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxEmailId.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelContact;
        private System.Windows.Forms.Label labelName;
        private DevExpress.XtraEditors.TextEdit textBoxName;
        private DevExpress.XtraEditors.TextEdit textBoxPhoneContact;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnAdd;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSearch;
        private System.Windows.Forms.Label labelPhoneContact;
        private Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch.ViewAddressUserControl viewAddressUserControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelEmailId;
        private DevExpress.XtraEditors.TextEdit textBoxEmailId;
    }
}
