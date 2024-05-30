/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class frmGetSalesOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private DevExpress.XtraEditors.StyleController styleController1;

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
            this.styleController1 = new DevExpress.XtraEditors.StyleController(this.components);
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.basePanel = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grSalesOrders = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSalesOrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrderType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSalesStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDocumentStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreationDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustomerAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnEdit = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPickUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancelOrder = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnReturn = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.textBoxSearch = new DevExpress.XtraEditors.TextEdit();
            this.btnCreatePickList = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCreatePackSlip = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPrintPackSlip = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblHeading = new System.Windows.Forms.Label();
            this.btnCreateInvoice = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grSalesOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxSearch.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(102, 536);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.ShowToolTips = false;
            this.btnUp.Size = new System.Drawing.Size(65, 61);
            this.btnUp.TabIndex = 11;
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(841, 536);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.ShowToolTips = false;
            this.btnDown.Size = new System.Drawing.Size(65, 61);
            this.btnDown.TabIndex = 12;
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(29, 536);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(3, 11, 4, 4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.ShowToolTips = false;
            this.btnPgUp.Size = new System.Drawing.Size(65, 61);
            this.btnPgUp.TabIndex = 10;
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(914, 536);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4, 11, 3, 3);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.ShowToolTips = false;
            this.btnPgDown.Size = new System.Drawing.Size(65, 61);
            this.btnPgDown.TabIndex = 13;
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.tableLayoutPanel);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Margin = new System.Windows.Forms.Padding(0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(1012, 681);
            this.basePanel.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 14;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.grSalesOrders, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.btnEdit, 3, 4);
            this.tableLayoutPanel.Controls.Add(this.btnPickUp, 5, 4);
            this.tableLayoutPanel.Controls.Add(this.btnCancelOrder, 7, 4);
            this.tableLayoutPanel.Controls.Add(this.btnReturn, 9, 4);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnCreatePickList, 3, 5);
            this.tableLayoutPanel.Controls.Add(this.btnCreatePackSlip, 5, 5);
            this.tableLayoutPanel.Controls.Add(this.btnPrintPackSlip, 7, 5);
            this.tableLayoutPanel.Controls.Add(this.btnClose, 12, 5);
            this.tableLayoutPanel.Controls.Add(this.btnPgUp, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.btnUp, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.btnDown, 12, 4);
            this.tableLayoutPanel.Controls.Add(this.btnPgDown, 13, 4);
            this.tableLayoutPanel.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btnCreateInvoice, 9, 5);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1008, 677);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // grSalesOrders
            // 
            this.grSalesOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grSalesOrders.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel.SetColumnSpan(this.grSalesOrders, 14);
            this.grSalesOrders.Location = new System.Drawing.Point(29, 194);
            this.grSalesOrders.MainView = this.gridView1;
            this.grSalesOrders.Name = "grSalesOrders";
            this.grSalesOrders.Size = new System.Drawing.Size(950, 328);
            this.grSalesOrders.TabIndex = 1;
            this.grSalesOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.ColumnPanelRowHeight = 40;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSalesOrderID,
            this.colOrderType,
            this.colSalesStatus,
            this.colDocumentStatus,
            this.colCreationDate,
            this.colCustomerAccount,
            this.colCustomerName,
            this.colTotalAmount,
            this.colEmail,
            this.gridColumn1});
            this.gridView1.CustomizationFormBounds = new System.Drawing.Rectangle(749, 376, 216, 323);
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grSalesOrders;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowHeight = 40;
            this.gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colCreationDate, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colSalesOrderID
            // 
            this.colSalesOrderID.Caption = "Sales order";
            this.colSalesOrderID.FieldName = "SALESID";
            this.colSalesOrderID.Name = "colSalesOrderID";
            this.colSalesOrderID.Visible = true;
            this.colSalesOrderID.VisibleIndex = 0;
            this.colSalesOrderID.Width = 150;
            // 
            // colOrderType
            // 
            this.colOrderType.AppearanceCell.Options.UseTextOptions = true;
            this.colOrderType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colOrderType.Caption = "Journal Num"; //this.colOrderType.Caption = "Order type";
            this.colOrderType.FieldName = "JOURNALID"; //this.colOrderType.FieldName = "ORDERTYPE"; //change to JournalID by Yonathan 26/06/2023
            this.colOrderType.Name = "colOrderType";
            this.colOrderType.Visible = true;
            this.colOrderType.VisibleIndex = 5;
            this.colOrderType.Width = 101;
            // 
            // colSalesStatus
            // 
            this.colSalesStatus.AppearanceCell.Options.UseTextOptions = true;
            this.colSalesStatus.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colSalesStatus.Caption = "Order Status";
            this.colSalesStatus.FieldName = "SALESSTATUS";
            this.colSalesStatus.Name = "colSalesStatus";
            this.colSalesStatus.Visible = true;
            this.colSalesStatus.VisibleIndex = 1;
            this.colSalesStatus.Width = 102;
            // 
            // colDocumentStatus
            // 
            this.colDocumentStatus.AppearanceCell.Options.UseTextOptions = true;
            this.colDocumentStatus.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colDocumentStatus.Caption = "Document Status";
            this.colDocumentStatus.FieldName = "DOCUMENTSTATUS";
            this.colDocumentStatus.Name = "colDocumentStatus";
            this.colDocumentStatus.Visible = true;
            this.colDocumentStatus.VisibleIndex = 2;
            this.colDocumentStatus.Width = 102;
            // 
            // colCreationDate
            // 
            this.colCreationDate.Caption = "Created date";
            this.colCreationDate.DisplayFormat.FormatString = "d";
            this.colCreationDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colCreationDate.FieldName = "DATE";
            this.colCreationDate.Name = "colCreationDate";
            this.colCreationDate.OptionsColumn.AllowEdit = false;
            this.colCreationDate.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.colCreationDate.Visible = true;
            this.colCreationDate.VisibleIndex = 3;
            this.colCreationDate.Width = 97;
            // 
            // colCustomerAccount
            // 
            this.colCustomerAccount.Caption = "Customer";
            this.colCustomerAccount.FieldName = "CUSTOMERACCOUNT";
            this.colCustomerAccount.Name = "colCustomerAccount";
            this.colCustomerAccount.Width = 140;
            // 
            // colCustomerName
            // 
            this.colCustomerName.Caption = "Customer Name";
            this.colCustomerName.FieldName = "CUSTOMERNAME";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.Visible = true;
            this.colCustomerName.VisibleIndex = 4;
            this.colCustomerName.Width = 220;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.AppearanceCell.Options.UseTextOptions = true;
            this.colTotalAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTotalAmount.Caption = "Total";
            this.colTotalAmount.DisplayFormat.FormatString = "n2";
            this.colTotalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalAmount.FieldName = "TOTALAMOUNT";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.Visible = true;
            this.colTotalAmount.VisibleIndex = 6;
            this.colTotalAmount.Width = 176;
            // 
            // colEmail
            // 
            this.colEmail.Caption = "E-mail";
            this.colEmail.FieldName = "EMAIL";
            this.colEmail.Name = "colEmail";
            this.colEmail.Width = 120;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Name = "gridColumn1";

            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEdit.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnEdit, 2);
            this.btnEdit.Location = new System.Drawing.Point(232, 538);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.ShowToolTips = false;
            this.btnEdit.Size = new System.Drawing.Size(130, 57);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit order";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPickUp
            // 
            this.btnPickUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickUp.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPickUp.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnPickUp, 2);
            this.btnPickUp.Location = new System.Drawing.Point(370, 538);
            this.btnPickUp.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnPickUp.Name = "btnPickUp";
            this.btnPickUp.ShowToolTips = false;
            this.btnPickUp.Size = new System.Drawing.Size(136, 57);
            this.btnPickUp.TabIndex = 3;
            this.btnPickUp.Text = "Pick up order";
            this.btnPickUp.Click += new System.EventHandler(this.btnPickUp_Click);
            // 
            // btnCancelOrder
            // 
            this.btnCancelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelOrder.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelOrder.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnCancelOrder, 2);
            this.btnCancelOrder.Location = new System.Drawing.Point(514, 538);
            this.btnCancelOrder.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.ShowToolTips = false;
            this.btnCancelOrder.Size = new System.Drawing.Size(127, 57);
            this.btnCancelOrder.TabIndex = 4;
            this.btnCancelOrder.Text = "Cancel order";
            this.btnCancelOrder.Click += new System.EventHandler(this.btnCancelOrder_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturn.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReturn.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnReturn, 2);
            this.btnReturn.Location = new System.Drawing.Point(649, 538);
            this.btnReturn.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.ShowToolTips = false;
            this.btnReturn.Size = new System.Drawing.Size(127, 57);
            this.btnReturn.TabIndex = 5;
            this.btnReturn.Text = "Return order";
            this.btnReturn.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel.SetColumnSpan(this.tableLayoutPanel1, 16);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnSearch, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClear, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSearch, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(29, 138);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(950, 50);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.search;
            this.btnSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(830, 9);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 32);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClear.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.remove;
            this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(890, 9);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(57, 32);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "û";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch.Location = new System.Drawing.Point(3, 9);
            this.textBoxSearch.Name = "textBoxSearch";
            // 
            // 
            // 
            this.textBoxSearch.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxSearch.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSearch.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxSearch.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxSearch.Properties.Appearance.Options.UseFont = true;
            this.textBoxSearch.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxSearch.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxSearch.Size = new System.Drawing.Size(821, 32);
            this.textBoxSearch.TabIndex = 0;
            this.textBoxSearch.Enter += new System.EventHandler(this.textBoxSearch_Enter);
            this.textBoxSearch.Leave += new System.EventHandler(this.textBoxSearch_Leave);
            // 
            // btnCreatePickList
            // 
            this.btnCreatePickList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatePickList.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreatePickList.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnCreatePickList, 2);
            this.btnCreatePickList.Location = new System.Drawing.Point(232, 605);
            this.btnCreatePickList.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreatePickList.Name = "btnCreatePickList";
            this.btnCreatePickList.ShowToolTips = false;
            this.btnCreatePickList.Size = new System.Drawing.Size(130, 57);
            this.btnCreatePickList.TabIndex = 6;
            this.btnCreatePickList.Tag = "BtnNormal";
            this.btnCreatePickList.Text = "Create picking list";
            this.btnCreatePickList.Click += new System.EventHandler(this.btnCreatePickList_Click);
            // 
            // btnCreatePackSlip
            // 
            this.btnCreatePackSlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatePackSlip.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreatePackSlip.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnCreatePackSlip, 2);
            this.btnCreatePackSlip.Location = new System.Drawing.Point(370, 605);
            this.btnCreatePackSlip.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreatePackSlip.Name = "btnCreatePackSlip";
            this.btnCreatePackSlip.ShowToolTips = false;
            this.btnCreatePackSlip.Size = new System.Drawing.Size(136, 57);
            this.btnCreatePackSlip.TabIndex = 7;
            this.btnCreatePackSlip.Tag = "BtnNormal";
            this.btnCreatePackSlip.Text = "Create packing slip";
            this.btnCreatePackSlip.Click += new System.EventHandler(this.btnCreatePackSlip_Click);
            // 
            // btnPrintPackSlip
            // 
            this.btnPrintPackSlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintPackSlip.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrintPackSlip.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnPrintPackSlip, 2);
            this.btnPrintPackSlip.Location = new System.Drawing.Point(514, 605);
            this.btnPrintPackSlip.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintPackSlip.Name = "btnPrintPackSlip";
            this.btnPrintPackSlip.ShowToolTips = false;
            this.btnPrintPackSlip.Size = new System.Drawing.Size(127, 57);
            this.btnPrintPackSlip.TabIndex = 8;
            this.btnPrintPackSlip.Tag = "BtnNormal";
            this.btnPrintPackSlip.Text = "Print packing slip";
            this.btnPrintPackSlip.Click += new System.EventHandler(this.btnPrintPackSlip_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnClose, 2);
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(841, 605);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(137, 57);
            this.btnClose.TabIndex = 9;
            this.btnClose.Tag = "BtnNormal";
            this.btnClose.Text = "Close";
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblHeading, 14);
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(369, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(270, 95);
            this.lblHeading.TabIndex = 14;
            this.lblHeading.Tag = "";
            this.lblHeading.Text = "Sales orders";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateInvoice.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateInvoice.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.btnCreateInvoice, 2);
            this.btnCreateInvoice.Location = new System.Drawing.Point(649, 605);
            this.btnCreateInvoice.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.ShowToolTips = false;
            this.btnCreateInvoice.Size = new System.Drawing.Size(127, 57);
            this.btnCreateInvoice.TabIndex = 15;
            this.btnCreateInvoice.Tag = "BtnNormal";
            this.btnCreateInvoice.Text = "Create Invoice";
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // frmGetSalesOrder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1012, 681);
            this.Controls.Add(this.basePanel);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmGetSalesOrder";
            this.Controls.SetChildIndex(this.basePanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grSalesOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxSearch.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private DevExpress.XtraEditors.PanelControl basePanel;
        private DevExpress.XtraGrid.GridControl grSalesOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colSalesOrderID;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerAccount;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerName;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colCreationDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderType;
        private DevExpress.XtraGrid.Columns.GridColumn colSalesStatus;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCreatePackSlip;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPrintPackSlip;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCreatePickList;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnReturn;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPickUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnEdit;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancelOrder;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.TextEdit textBoxSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.Columns.GridColumn colEmail;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private System.Windows.Forms.Label lblHeading;
        private DevExpress.XtraGrid.Columns.GridColumn colDocumentStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCreateInvoice;
        private DevExpress.XtraEditors.StyleController styleController;     
    }
}
