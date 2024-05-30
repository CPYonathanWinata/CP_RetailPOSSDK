/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class formOrderDetails
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPageUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSave = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPageDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClear = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.orderInformationPage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.OrderInformationPage();
            this.customerInformationPage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.CustomerInformationPage();
            this.itemDetailsPage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.ItemDetailsPage();
            this.deliveryInformationPage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.DeliveryInformationPage();
            this.paymentHistoryPage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.PaymentHistoryPage();
            this.cancellationChargePage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.CancellationChargePage();
            this.summaryPage = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages.OrderSummaryPage();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.orderDetailsNavBar = new Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsNavigationBar();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 245F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.panel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelTitle, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.orderDetailsNavBar, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 15, 10, 6);
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.orderInformationPage);
            this.panel.Controls.Add(this.customerInformationPage);
            this.panel.Controls.Add(this.itemDetailsPage);
            this.panel.Controls.Add(this.deliveryInformationPage);
            this.panel.Controls.Add(this.paymentHistoryPage);
            this.panel.Controls.Add(this.cancellationChargePage);
            this.panel.Controls.Add(this.summaryPage);
            this.panel.Location = new System.Drawing.Point(258, 82);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.panel.Size = new System.Drawing.Size(749, 601);
            this.panel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel.SetColumnSpan(this.tableLayoutPanel1, 2);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPageUp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPageDown, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDown, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClear, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(120, 686);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(779, 72);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(461, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // btnPageUp
            // 
            this.btnPageUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnPageUp.Appearance.Options.UseFont = true;
            this.btnPageUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.top;
            this.btnPageUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageUp.Location = new System.Drawing.Point(4, 11);
            this.btnPageUp.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnPageUp.Name = "btnPageUp";
            this.btnPageUp.Size = new System.Drawing.Size(57, 57);
            this.btnPageUp.TabIndex = 2;
            this.btnPageUp.Text = "Ç";
            this.btnPageUp.Click += new System.EventHandler(this.OnPageUp_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(326, 11);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(127, 57);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            // 
            // btnPageDown
            // 
            this.btnPageDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageDown.Appearance.Options.UseFont = true;
            this.btnPageDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.bottom;
            this.btnPageDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageDown.Location = new System.Drawing.Point(718, 11);
            this.btnPageDown.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnPageDown.Name = "btnPageDown";
            this.btnPageDown.Size = new System.Drawing.Size(57, 57);
            this.btnPageDown.TabIndex = 5;
            this.btnPageDown.Text = "Ê";
            this.btnPageDown.Click += new System.EventHandler(this.OnPageDown_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(653, 11);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.OnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(69, 11);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.OnUp_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(191, 11);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(127, 57);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.OnClear_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.labelTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SelectedPageTitle", true));
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Light", 30F);
            this.labelTitle.Location = new System.Drawing.Point(471, 15);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.labelTitle.Size = new System.Drawing.Size(323, 64);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Order information";
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.tableLayoutPanel);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(1024, 768);
            this.panelControl.TabIndex = 0;
            // 
            // orderInformationPage
            // 
            this.orderInformationPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderInformationPage.Enabled = false;
            this.orderInformationPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderInformationPage.Location = new System.Drawing.Point(30, 0);
            this.orderInformationPage.Name = "orderInformationPage";
            this.orderInformationPage.Size = new System.Drawing.Size(689, 601);
            this.orderInformationPage.TabIndex = 0;
            this.orderInformationPage.Text = "Order information";
            // 
            // customerInformationPage
            // 
            this.customerInformationPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerInformationPage.Enabled = false;
            this.customerInformationPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customerInformationPage.Location = new System.Drawing.Point(30, 0);
            this.customerInformationPage.Name = "customerInformationPage";
            this.customerInformationPage.Size = new System.Drawing.Size(689, 601);
            this.customerInformationPage.TabIndex = 1;
            this.customerInformationPage.Text = "Customer information";
            // 
            // itemDetailsPage
            // 
            this.itemDetailsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemDetailsPage.Enabled = false;
            this.itemDetailsPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemDetailsPage.Location = new System.Drawing.Point(30, 0);
            this.itemDetailsPage.Name = "itemDetailsPage";
            this.itemDetailsPage.Size = new System.Drawing.Size(689, 601);
            this.itemDetailsPage.TabIndex = 1;
            this.itemDetailsPage.Text = "Item details";
            // 
            // deliveryInformationPage
            // 
            this.deliveryInformationPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deliveryInformationPage.Enabled = false;
            this.deliveryInformationPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deliveryInformationPage.Location = new System.Drawing.Point(30, 0);
            this.deliveryInformationPage.Name = "deliveryInformationPage";
            this.deliveryInformationPage.Size = new System.Drawing.Size(689, 601);
            this.deliveryInformationPage.TabIndex = 2;
            this.deliveryInformationPage.Text = "Delivery information";
            // 
            // paymentHistoryPage
            // 
            this.paymentHistoryPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentHistoryPage.Enabled = false;
            this.paymentHistoryPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paymentHistoryPage.Location = new System.Drawing.Point(30, 0);
            this.paymentHistoryPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.paymentHistoryPage.Name = "paymentHistoryPage";
            this.paymentHistoryPage.Size = new System.Drawing.Size(689, 601);
            this.paymentHistoryPage.TabIndex = 3;
            this.paymentHistoryPage.Text = "Payment history";
            // 
            // cancellationChargePage
            // 
            this.cancellationChargePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancellationChargePage.Enabled = false;
            this.cancellationChargePage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancellationChargePage.Location = new System.Drawing.Point(30, 0);
            this.cancellationChargePage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancellationChargePage.Name = "cancellationChargePage";
            this.cancellationChargePage.Size = new System.Drawing.Size(689, 601);
            this.cancellationChargePage.TabIndex = 3;
            this.cancellationChargePage.Text = "Cancellation charge";
            // 
            // summaryPage
            // 
            this.summaryPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryPage.Enabled = false;
            this.summaryPage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summaryPage.Location = new System.Drawing.Point(30, 0);
            this.summaryPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.summaryPage.Name = "summaryPage";
            this.summaryPage.Size = new System.Drawing.Size(689, 601);
            this.summaryPage.TabIndex = 3;
            this.summaryPage.Text = "Order summary";
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.SalesOrder.OrderDetailsViewModel);
            // 
            // orderDetailsNavBar
            // 
            this.orderDetailsNavBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orderDetailsNavBar.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", this.bindingSource, "SelectedPageIndex", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.orderDetailsNavBar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderDetailsNavBar.Location = new System.Drawing.Point(10, 33);
            this.orderDetailsNavBar.Margin = new System.Windows.Forms.Padding(0, 18, 0, 0);
            this.orderDetailsNavBar.Name = "orderDetailsNavBar";
            this.tableLayoutPanel.SetRowSpan(this.orderDetailsNavBar, 2);
            this.orderDetailsNavBar.SelectedIndex = 0;
            this.orderDetailsNavBar.Size = new System.Drawing.Size(245, 653);
            this.orderDetailsNavBar.TabIndex = 7;
            // 
            // formOrderDetails
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "formOrderDetails";
            this.Text = "Order Details";
            this.Controls.SetChildIndex(this.panelControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.panel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label labelTitle;
        private OrderDetailsNavigationBar orderDetailsNavBar;
        private OrderDetailsPages.OrderInformationPage orderInformationPage;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private OrderDetailsPages.CustomerInformationPage customerInformationPage;
        private OrderDetailsPages.ItemDetailsPage itemDetailsPage;
        private OrderDetailsPages.DeliveryInformationPage deliveryInformationPage;
        private OrderDetailsPages.PaymentHistoryPage paymentHistoryPage;
        private OrderDetailsPages.CancellationChargePage cancellationChargePage;
        private OrderDetailsPages.OrderSummaryPage summaryPage;
        private System.Windows.Forms.BindingSource bindingSource;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSave;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClear;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}