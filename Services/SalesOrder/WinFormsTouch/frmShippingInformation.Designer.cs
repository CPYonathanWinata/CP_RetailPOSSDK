/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class formShippingInformation
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
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelContact = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelDeliveryDate = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnEditMethod = new LSRetailPosis.POSProcesses.WinControls.SimplePopupButton();
            this.labelShippingMethod = new System.Windows.Forms.Label();
            this.labelShipMethodValue = new System.Windows.Forms.Label();
            this.btnChargeChange = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelCharge = new System.Windows.Forms.Label();
            this.labelShipChargeValue = new System.Windows.Forms.Label();
            this.tableLayoutPanelLower = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.viewAddressUserControl1 = new Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch.ViewAddressUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tableLayoutPanelLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.tableLayoutPanel);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panelControl.Size = new System.Drawing.Size(1024, 768);
            this.panelControl.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.btnClear, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelContact, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelDeliveryDate, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.dateTimePicker, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.btnSearch, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.btnEditMethod, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.labelShippingMethod, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.labelShipMethodValue, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.btnChargeChange, 4, 5);
            this.tableLayoutPanel.Controls.Add(this.labelCharge, 3, 5);
            this.tableLayoutPanel.Controls.Add(this.labelShipChargeValue, 3, 6);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelLower, 0, 14);
            this.tableLayoutPanel.Controls.Add(this.viewAddressUserControl1, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(30, 40, 30, 0);
            this.tableLayoutPanel.RowCount = 15;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1010, 759);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.Location = new System.Drawing.Point(306, 221);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(127, 57);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.OnClear_Click);
            // 
            // labelContact
            // 
            this.labelContact.AutoSize = true;
            this.labelContact.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelContact.Location = new System.Drawing.Point(33, 155);
            this.labelContact.Name = "labelContact";
            this.labelContact.Size = new System.Drawing.Size(193, 25);
            this.labelContact.TabIndex = 1;
            this.labelContact.Text = "SHIPPING ADDRESS";
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelTitle, 5);
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelTitle.Location = new System.Drawing.Point(280, 40);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelTitle.Size = new System.Drawing.Size(450, 95);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Shipping information";
            // 
            // labelDeliveryDate
            // 
            this.labelDeliveryDate.AutoSize = true;
            this.labelDeliveryDate.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeliveryDate.Location = new System.Drawing.Point(33, 344);
            this.labelDeliveryDate.Name = "labelDeliveryDate";
            this.labelDeliveryDate.Size = new System.Drawing.Size(152, 25);
            this.labelDeliveryDate.TabIndex = 5;
            this.labelDeliveryDate.Text = "DELIVERY DATE";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Checked = false;
            this.tableLayoutPanel.SetColumnSpan(this.dateTimePicker, 2);
            this.dateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "DeliveryDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePicker.Location = new System.Drawing.Point(33, 404);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowCheckBox = true;
            this.dateTimePicker.Size = new System.Drawing.Size(400, 29);
            this.dateTimePicker.TabIndex = 6;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.SalesOrder.ShippingInformationViewModel);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.Location = new System.Drawing.Point(306, 158);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(127, 57);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.OnSearch_Click);
            // 
            // btnEditMethod
            // 
            this.btnEditMethod.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEditMethod.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditMethod.Appearance.Options.UseFont = true;
            this.btnEditMethod.Location = new System.Drawing.Point(849, 158);
            this.btnEditMethod.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnEditMethod.Name = "btnEditMethod";
            this.btnEditMethod.PopupContent = null;
            this.btnEditMethod.Size = new System.Drawing.Size(127, 57);
            this.btnEditMethod.TabIndex = 8;
            this.btnEditMethod.Text = "Change...";
            this.btnEditMethod.Click += new System.EventHandler(this.OnBtnShippingMethod_Click);
            // 
            // labelShippingMethod
            // 
            this.labelShippingMethod.AutoSize = true;
            this.labelShippingMethod.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelShippingMethod.Location = new System.Drawing.Point(575, 155);
            this.labelShippingMethod.Name = "labelShippingMethod";
            this.labelShippingMethod.Size = new System.Drawing.Size(190, 25);
            this.labelShippingMethod.TabIndex = 7;
            this.labelShippingMethod.Text = "SHIPPING METHOD";
            // 
            // labelShipMethodValue
            // 
            this.labelShipMethodValue.AutoSize = true;
            this.labelShipMethodValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ShippingMethod.DescriptionText", true));
            this.labelShipMethodValue.Location = new System.Drawing.Point(575, 218);
            this.labelShipMethodValue.Name = "labelShipMethodValue";
            this.labelShipMethodValue.Size = new System.Drawing.Size(97, 21);
            this.labelShipMethodValue.TabIndex = 9;
            this.labelShipMethodValue.Text = "Next Day Air";
            // 
            // btnChargeChange
            // 
            this.btnChargeChange.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChargeChange.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChargeChange.Appearance.Options.UseFont = true;
            this.btnChargeChange.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ShippingCharge", true));
            this.btnChargeChange.Location = new System.Drawing.Point(849, 344);
            this.btnChargeChange.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnChargeChange.Name = "btnChargeChange";
            this.btnChargeChange.Size = new System.Drawing.Size(127, 57);
            this.btnChargeChange.TabIndex = 11;
            this.btnChargeChange.Text = "Add";
            this.btnChargeChange.Click += new System.EventHandler(this.OnChargeChange_Click);
            // 
            // labelCharge
            // 
            this.labelCharge.AutoSize = true;
            this.labelCharge.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelCharge.Location = new System.Drawing.Point(575, 344);
            this.labelCharge.Name = "labelCharge";
            this.labelCharge.Size = new System.Drawing.Size(162, 25);
            this.labelCharge.TabIndex = 10;
            this.labelCharge.Text = "Shipping charge:";
            // 
            // labelShipChargeValue
            // 
            this.labelShipChargeValue.AutoSize = true;
            this.labelShipChargeValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedShippingCharge", true));
            this.labelShipChargeValue.Location = new System.Drawing.Point(575, 401);
            this.labelShipChargeValue.Name = "labelShipChargeValue";
            this.labelShipChargeValue.Size = new System.Drawing.Size(49, 21);
            this.labelShipChargeValue.TabIndex = 12;
            this.labelShipChargeValue.Text = "$0.00";
            // 
            // tableLayoutPanelLower
            // 
            this.tableLayoutPanelLower.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tableLayoutPanelLower.ColumnCount = 2;
            this.tableLayoutPanel.SetColumnSpan(this.tableLayoutPanelLower, 5);
            this.tableLayoutPanelLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLower.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanelLower.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanelLower.Location = new System.Drawing.Point(91, 603);
            this.tableLayoutPanelLower.Name = "tableLayoutPanelLower";
            this.tableLayoutPanelLower.RowCount = 1;
            this.tableLayoutPanelLower.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLower.Size = new System.Drawing.Size(828, 153);
            this.tableLayoutPanelLower.TabIndex = 13;
            // 
            // btnSaveF
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "Validated", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(283, 80);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 15, 4, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(3, 15, 3, 15);
            this.btnSave.Size = new System.Drawing.Size(127, 58);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(418, 81);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 15, 4, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // viewAddressUserControl1
            // 
            this.viewAddressUserControl1.AutoSize = true;
            this.viewAddressUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewAddressUserControl1.Location = new System.Drawing.Point(33, 221);
            this.viewAddressUserControl1.MinimumSize = new System.Drawing.Size(200, 120);
            this.viewAddressUserControl1.Name = "viewAddressUserControl1";
            this.tableLayoutPanel.SetRowSpan(this.viewAddressUserControl1, 2);
            this.viewAddressUserControl1.Size = new System.Drawing.Size(267, 120);
            this.viewAddressUserControl1.TabIndex = 3;
            this.viewAddressUserControl1.TabStop = false;
            // 
            // formShippingInformation
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "formShippingInformation";
            this.Controls.SetChildIndex(this.panelControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tableLayoutPanelLower.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelShippingMethod;
        private System.Windows.Forms.Label labelContact;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelShipMethodValue;
        private System.Windows.Forms.Label labelShipChargeValue;
        private System.Windows.Forms.Label labelCharge;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label labelDeliveryDate;
        private LSRetailPosis.POSProcesses.WinControls.SimplePopupButton btnEditMethod;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnChargeChange;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClear;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLower;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSave;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
		private Customer.WinFormsTouch.ViewAddressUserControl viewAddressUserControl1;
    }
}