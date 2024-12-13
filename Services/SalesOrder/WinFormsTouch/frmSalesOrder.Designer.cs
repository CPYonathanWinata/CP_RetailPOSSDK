/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class frmSalesOrder
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
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFromDate = new DevExpress.XtraEditors.LabelControl();
            this.lblHeading = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblCustomerIdHeading = new DevExpress.XtraEditors.LabelControl();
            this.txtCustomerId = new DevExpress.XtraEditors.TextEdit();
            this.lblSalesOrderIdHeading = new DevExpress.XtraEditors.LabelControl();
            this.txtSalesOrderId = new DevExpress.XtraEditors.TextEdit();
            this.lblToDate = new DevExpress.XtraEditors.LabelControl();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblOrder = new DevExpress.XtraEditors.LabelControl();
            this.orderBox = new System.Windows.Forms.ComboBox();
            this.btnCustomerSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.chkOnlineOrder = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesOrderId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.tableLayoutPanel1);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Padding = new System.Windows.Forms.Padding(5);
            this.panelControl.Size = new System.Drawing.Size(1024, 768);
            this.panelControl.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lblFromDate, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomerIdHeading, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtCustomerId, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSalesOrderIdHeading, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtSalesOrderId, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblToDate, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.dtpFromDate, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.dtpToDate, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblOrder, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.orderBox, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.btnCustomerSearch, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 5);
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1010, 754);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFromDate.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(33, 309);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(85, 25);
            this.lblFromDate.TabIndex = 6;
            this.lblFromDate.Text = "Start date:";
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblHeading, 2);
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(362, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(285, 95);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Tag = "";
            this.lblHeading.Text = "Recall orders";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(33, 681);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(944, 65);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(476, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Tag = "BtnNormal";
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Appearance.Options.UseTextOptions = true;
            this.btnOk.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnOk.Location = new System.Drawing.Point(341, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 0;
            this.btnOk.Tag = "BtnNormal";
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblCustomerIdHeading
            // 
            this.lblCustomerIdHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCustomerIdHeading.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerIdHeading.Location = new System.Drawing.Point(33, 139);
            this.lblCustomerIdHeading.Name = "lblCustomerIdHeading";
            this.lblCustomerIdHeading.Size = new System.Drawing.Size(108, 25);
            this.lblCustomerIdHeading.TabIndex = 1;
            this.lblCustomerIdHeading.Text = "Customer ID:";
            // 
            // txtCustomerId
            // 
            this.txtCustomerId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomerId.Location = new System.Drawing.Point(33, 170);
            this.txtCustomerId.MinimumSize = new System.Drawing.Size(4, 22);
            this.txtCustomerId.Name = "txtCustomerId";
            // 
            // 
            // 
            this.txtCustomerId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCustomerId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerId.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtCustomerId.Properties.Appearance.Options.UseBackColor = true;
            this.txtCustomerId.Properties.Appearance.Options.UseFont = true;
            this.txtCustomerId.Properties.Appearance.Options.UseForeColor = true;
            this.txtCustomerId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCustomerId.Size = new System.Drawing.Size(881, 28);
            this.txtCustomerId.TabIndex = 2;
            this.txtCustomerId.TextChanged += new System.EventHandler(this.OnFieldChanged);
            // 
            // lblSalesOrderIdHeading
            // 
            this.lblSalesOrderIdHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSalesOrderIdHeading.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesOrderIdHeading.Location = new System.Drawing.Point(33, 224);
            this.lblSalesOrderIdHeading.Name = "lblSalesOrderIdHeading";
            this.lblSalesOrderIdHeading.Size = new System.Drawing.Size(76, 25);
            this.lblSalesOrderIdHeading.TabIndex = 4;
            this.lblSalesOrderIdHeading.Text = "Order ID:";
            // 
            // txtSalesOrderId
            // 
            this.txtSalesOrderId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSalesOrderId.Location = new System.Drawing.Point(33, 255);
            this.txtSalesOrderId.MinimumSize = new System.Drawing.Size(4, 22);
            this.txtSalesOrderId.Name = "txtSalesOrderId";
            // 
            // 
            // 
            this.txtSalesOrderId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtSalesOrderId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalesOrderId.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtSalesOrderId.Properties.Appearance.Options.UseBackColor = true;
            this.txtSalesOrderId.Properties.Appearance.Options.UseFont = true;
            this.txtSalesOrderId.Properties.Appearance.Options.UseForeColor = true;
            this.txtSalesOrderId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSalesOrderId.Size = new System.Drawing.Size(881, 28);
            this.txtSalesOrderId.TabIndex = 5;
            this.txtSalesOrderId.TextChanged += new System.EventHandler(this.OnFieldChanged);
            // 
            // lblToDate
            // 
            this.lblToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblToDate.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(33, 395);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(78, 25);
            this.lblToDate.TabIndex = 8;
            this.lblToDate.Text = "End date:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Checked = false;
            this.dtpFromDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Location = new System.Drawing.Point(33, 340);
            this.dtpFromDate.MinimumSize = new System.Drawing.Size(944, 22);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ShowCheckBox = true;
            this.dtpFromDate.Size = new System.Drawing.Size(944, 29);
            this.dtpFromDate.TabIndex = 7;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.OnFieldChanged);
            // 
            // dtpToDate
            // 
            this.dtpToDate.Checked = false;
            this.dtpToDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Location = new System.Drawing.Point(33, 426);
            this.dtpToDate.MinimumSize = new System.Drawing.Size(944, 22);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.ShowCheckBox = true;
            this.dtpToDate.Size = new System.Drawing.Size(944, 29);
            this.dtpToDate.TabIndex = 9;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.OnFieldChanged);
            // 
            // lblOrder
            // 
            this.lblOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOrder.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrder.Location = new System.Drawing.Point(33, 461);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(94, 25);
            this.lblOrder.TabIndex = 4;
            this.lblOrder.Text = "Order type:";
            this.lblOrder.Visible = true;
            // 
            // orderBox
            // 
            this.orderBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderBox.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderBox.FormattingEnabled = true;
            this.orderBox.Items.AddRange(new object[] {
            "POS Order",
            "Online Order"});
            this.orderBox.Location = new System.Drawing.Point(33, 461);
            this.orderBox.Name = "orderBox";
            this.orderBox.Size = new System.Drawing.Size(271, 33);
            this.orderBox.TabIndex = 6;
            this.orderBox.Visible = true;
            // 
            // btnCustomerSearch
            // 
            this.btnCustomerSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCustomerSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerSearch.Appearance.Options.UseFont = true;
            this.btnCustomerSearch.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.search;
            this.btnCustomerSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCustomerSearch.Location = new System.Drawing.Point(920, 135);
            this.btnCustomerSearch.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.Padding = new System.Windows.Forms.Padding(0);
            this.btnCustomerSearch.Size = new System.Drawing.Size(57, 32);
            this.btnCustomerSearch.TabIndex = 3;
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // chkOnlineOrder
            // 
            this.chkOnlineOrder.AutoSize = true;
            this.chkOnlineOrder.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOnlineOrder.Location = new System.Drawing.Point(33, 461);
            this.chkOnlineOrder.Name = "chkOnlineOrder";
            this.chkOnlineOrder.Size = new System.Drawing.Size(188, 29);
            this.chkOnlineOrder.TabIndex = 5;
            this.chkOnlineOrder.Text = "Online order only";
            this.chkOnlineOrder.UseVisualStyleBackColor = true;
            // 
            // frmSalesOrder
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmSalesOrder";
            this.Controls.SetChildIndex(this.panelControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesOrderId.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl;
        private DevExpress.XtraEditors.TextEdit txtSalesOrderId;
        private DevExpress.XtraEditors.LabelControl lblSalesOrderIdHeading;
        private System.Windows.Forms.Label lblHeading;
        private DevExpress.XtraEditors.TextEdit txtCustomerId;
        private DevExpress.XtraEditors.LabelControl lblCustomerIdHeading;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCustomerSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private DevExpress.XtraEditors.LabelControl lblFromDate;
        private DevExpress.XtraEditors.LabelControl lblToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private DevExpress.XtraEditors.StyleController styleController;

        //add checkbox - yonathan 23092024
        private System.Windows.Forms.CheckBox chkOnlineOrder;
        private System.Windows.Forms.ComboBox orderBox;
        private DevExpress.XtraEditors.LabelControl lblOrder;
        //private System.Windows.Forms.Label lblOrder;
        //end
    }
}
