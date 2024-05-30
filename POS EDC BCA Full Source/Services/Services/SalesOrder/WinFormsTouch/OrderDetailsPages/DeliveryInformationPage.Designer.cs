/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class DeliveryInformationPage
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
                WingdingsFont.Dispose();
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
            this.gridItems = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItemOverview = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipAll = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPickupAll = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnPickupAll = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnShipAll = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.pnlTotals = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalShippingChargeValue = new System.Windows.Forms.Label();
            this.lblTotalShippingCharge = new System.Windows.Forms.Label();
            this.lblOrderShippingChargeValue = new System.Windows.Forms.Label();
            this.lblOrderShippingCharge = new System.Windows.Forms.Label();
            this.lblLinesShippingCharge = new System.Windows.Forms.Label();
            this.lblLinesShippingChargeValue = new System.Windows.Forms.Label();
            this.btnPageDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPageUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.pnlTotals.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 8;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.gridItems, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnPickupAll, 4, 3);
            this.tableLayoutPanel.Controls.Add(this.btnShipAll, 3, 3);
            this.tableLayoutPanel.Controls.Add(this.pnlTotals, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.btnPageDown, 7, 3);
            this.tableLayoutPanel.Controls.Add(this.btnDown, 6, 3);
            this.tableLayoutPanel.Controls.Add(this.btnUp, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.btnPageUp, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(701, 565);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // gridItems
            // 
            this.gridItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridItems.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel.SetColumnSpan(this.gridItems, 8);
            this.gridItems.Location = new System.Drawing.Point(3, 3);
            this.gridItems.MainView = this.gridView;
            this.gridItems.Name = "gridItems";
            this.gridItems.Size = new System.Drawing.Size(695, 392);
            this.gridItems.TabIndex = 1;
            this.gridItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.gridView.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.Transparent;
            this.gridView.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.Transparent;
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Appearance.Preview.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Preview.Options.UseFont = true;
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.ColumnPanelRowHeight = 50;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemOverview,
            this.colQuantity,
            this.colLineStatus,
            this.colShipAll,
            this.colPickupAll});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.GridControl = this.gridItems;
            this.gridView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsCustomization.AllowColumnMoving = false;
            this.gridView.OptionsCustomization.AllowColumnResizing = false;
            this.gridView.OptionsCustomization.AllowFilter = false;
            this.gridView.OptionsCustomization.AllowGroup = false;
            this.gridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView.OptionsCustomization.AllowSort = false;
            this.gridView.OptionsMenu.EnableColumnMenu = false;
            this.gridView.OptionsMenu.EnableFooterMenu = false;
            this.gridView.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridView.OptionsView.AutoCalcPreviewLineCount = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.OptionsView.ShowIndicator = false;
            this.gridView.OptionsView.ShowPreview = true;
            this.gridView.PreviewFieldName = "FormattedDeliveryMethod";
            this.gridView.PreviewLineCount = 1;
            this.gridView.RowHeight = 50;
            this.gridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView_CustomDrawCell);
            this.gridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView_CustomColumnDisplayText);
            this.gridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridView_KeyUp);
            this.gridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseDown);
            this.gridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseUp);
            this.gridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseMove);
            // 
            // colItemOverview
            // 
            this.colItemOverview.AppearanceCell.Options.UseTextOptions = true;
            this.colItemOverview.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colItemOverview.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colItemOverview.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colItemOverview.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.colItemOverview.AppearanceHeader.Options.UseTextOptions = true;
            this.colItemOverview.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colItemOverview.Caption = "Item overview";
            this.colItemOverview.FieldName = "ItemOverview";
            this.colItemOverview.MinWidth = 100;
            this.colItemOverview.Name = "colItemOverview";
            this.colItemOverview.OptionsColumn.AllowEdit = false;
            this.colItemOverview.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colItemOverview.Visible = true;
            this.colItemOverview.VisibleIndex = 0;
            this.colItemOverview.Width = 225;
            // 
            // colQuantity
            // 
            this.colQuantity.AppearanceCell.Options.UseTextOptions = true;
            this.colQuantity.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQuantity.AppearanceHeader.Options.UseTextOptions = true;
            this.colQuantity.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQuantity.Caption = "Quantity";
            this.colQuantity.FieldName = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.OptionsColumn.AllowEdit = false;
            this.colQuantity.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 1;
            // 
            // colLineStatus
            // 
            this.colLineStatus.AppearanceCell.Options.UseTextOptions = true;
            this.colLineStatus.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLineStatus.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colLineStatus.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLineStatus.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.colLineStatus.AppearanceHeader.Options.UseTextOptions = true;
            this.colLineStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLineStatus.Caption = "Status";
            this.colLineStatus.FieldName = "FormattedLineStatus";
            this.colLineStatus.MinWidth = 100;
            this.colLineStatus.Name = "colLineStatus";
            this.colLineStatus.OptionsColumn.AllowEdit = false;
            this.colLineStatus.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colLineStatus.Visible = true;
            this.colLineStatus.VisibleIndex = 2;
            this.colLineStatus.Width = 100;
            // 
            // colShipAll
            // 
            this.colShipAll.AppearanceCell.Options.UseTextOptions = true;
            this.colShipAll.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colShipAll.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colShipAll.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colShipAll.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.colShipAll.AppearanceHeader.Options.UseTextOptions = true;
            this.colShipAll.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colShipAll.Caption = "Ship";
            this.colShipAll.FieldName = "Ship";
            this.colShipAll.Name = "colShipAll";
            this.colShipAll.OptionsColumn.AllowEdit = false;
            this.colShipAll.OptionsColumn.AllowMove = false;
            this.colShipAll.OptionsColumn.AllowShowHide = false;
            this.colShipAll.OptionsColumn.AllowSize = false;
            this.colShipAll.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colShipAll.OptionsFilter.AllowAutoFilter = false;
            this.colShipAll.OptionsFilter.AllowFilter = false;
            this.colShipAll.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.colShipAll.Visible = true;
            this.colShipAll.VisibleIndex = 3;
            // 
            // colPickupAll
            // 
            this.colPickupAll.AppearanceCell.Options.UseTextOptions = true;
            this.colPickupAll.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPickupAll.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colPickupAll.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPickupAll.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.colPickupAll.AppearanceHeader.Options.UseTextOptions = true;
            this.colPickupAll.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPickupAll.Caption = "Pickup";
            this.colPickupAll.FieldName = "Pickup";
            this.colPickupAll.Name = "colPickupAll";
            this.colPickupAll.OptionsColumn.AllowEdit = false;
            this.colPickupAll.OptionsColumn.AllowMove = false;
            this.colPickupAll.OptionsColumn.AllowShowHide = false;
            this.colPickupAll.OptionsColumn.AllowSize = false;
            this.colPickupAll.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPickupAll.OptionsFilter.AllowAutoFilter = false;
            this.colPickupAll.OptionsFilter.AllowFilter = false;
            this.colPickupAll.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.colPickupAll.Visible = true;
            this.colPickupAll.VisibleIndex = 4;
            // 
            // btnPickupAll
            // 
            this.btnPickupAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPickupAll.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPickupAll.Appearance.Options.UseFont = true;
            this.btnPickupAll.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "IsDeliveryChangeAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnPickupAll.Location = new System.Drawing.Point(353, 508);
            this.btnPickupAll.Margin = new System.Windows.Forms.Padding(3, 10, 5, 0);
            this.btnPickupAll.Name = "btnPickupAll";
            this.btnPickupAll.Size = new System.Drawing.Size(127, 57);
            this.btnPickupAll.TabIndex = 10;
            this.btnPickupAll.Text = "Pickup all";
            this.btnPickupAll.Click += new System.EventHandler(this.OnPickupAll_Click);
            // 
            // btnShipAll
            // 
            this.btnShipAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShipAll.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShipAll.Appearance.Options.UseFont = true;
            this.btnShipAll.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "IsDeliveryChangeAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnShipAll.Location = new System.Drawing.Point(220, 508);
            this.btnShipAll.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.btnShipAll.Name = "btnShipAll";
            this.btnShipAll.Size = new System.Drawing.Size(127, 57);
            this.btnShipAll.TabIndex = 11;
            this.btnShipAll.Text = "Ship all";
            this.btnShipAll.Click += new System.EventHandler(this.OnShipAll_Click);
            // 
            // pnlTotals
            // 
            this.pnlTotals.ColumnCount = 2;
            this.tableLayoutPanel.SetColumnSpan(this.pnlTotals, 8);
            this.pnlTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlTotals.Controls.Add(this.lblTotalShippingChargeValue, 1, 2);
            this.pnlTotals.Controls.Add(this.lblTotalShippingCharge, 0, 2);
            this.pnlTotals.Controls.Add(this.lblOrderShippingChargeValue, 1, 1);
            this.pnlTotals.Controls.Add(this.lblOrderShippingCharge, 0, 1);
            this.pnlTotals.Controls.Add(this.lblLinesShippingCharge, 0, 0);
            this.pnlTotals.Controls.Add(this.lblLinesShippingChargeValue, 1, 0);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTotals.Location = new System.Drawing.Point(3, 401);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.RowCount = 3;
            this.pnlTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pnlTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pnlTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pnlTotals.Size = new System.Drawing.Size(695, 94);
            this.pnlTotals.TabIndex = 14;
            // 
            // lblTotalShippingChargeValue
            // 
            this.lblTotalShippingChargeValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedTotalShippingCharge", true));
            this.lblTotalShippingChargeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalShippingChargeValue.Location = new System.Drawing.Point(592, 62);
            this.lblTotalShippingChargeValue.Name = "lblTotalShippingChargeValue";
            this.lblTotalShippingChargeValue.Size = new System.Drawing.Size(100, 32);
            this.lblTotalShippingChargeValue.TabIndex = 5;
            this.lblTotalShippingChargeValue.Text = "$0";
            this.lblTotalShippingChargeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalShippingCharge
            // 
            this.lblTotalShippingCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalShippingCharge.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalShippingCharge.Location = new System.Drawing.Point(3, 62);
            this.lblTotalShippingCharge.Name = "lblTotalShippingCharge";
            this.lblTotalShippingCharge.Size = new System.Drawing.Size(583, 32);
            this.lblTotalShippingCharge.TabIndex = 4;
            this.lblTotalShippingCharge.Text = "Total shipping";
            this.lblTotalShippingCharge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOrderShippingChargeValue
            // 
            this.lblOrderShippingChargeValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedOrderShippingCharge", true));
            this.lblOrderShippingChargeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrderShippingChargeValue.Location = new System.Drawing.Point(592, 31);
            this.lblOrderShippingChargeValue.Name = "lblOrderShippingChargeValue";
            this.lblOrderShippingChargeValue.Size = new System.Drawing.Size(100, 31);
            this.lblOrderShippingChargeValue.TabIndex = 3;
            this.lblOrderShippingChargeValue.Text = "$0";
            this.lblOrderShippingChargeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOrderShippingCharge
            // 
            this.lblOrderShippingCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrderShippingCharge.Location = new System.Drawing.Point(3, 31);
            this.lblOrderShippingCharge.Name = "lblOrderShippingCharge";
            this.lblOrderShippingCharge.Size = new System.Drawing.Size(583, 31);
            this.lblOrderShippingCharge.TabIndex = 2;
            this.lblOrderShippingCharge.Text = "Order level shipping";
            this.lblOrderShippingCharge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLinesShippingCharge
            // 
            this.lblLinesShippingCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLinesShippingCharge.Location = new System.Drawing.Point(3, 0);
            this.lblLinesShippingCharge.Name = "lblLinesShippingCharge";
            this.lblLinesShippingCharge.Size = new System.Drawing.Size(583, 31);
            this.lblLinesShippingCharge.TabIndex = 1;
            this.lblLinesShippingCharge.Text = "Shipping lines total";
            this.lblLinesShippingCharge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLinesShippingChargeValue
            // 
            this.lblLinesShippingChargeValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedLinesShippingCharge", true));
            this.lblLinesShippingChargeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLinesShippingChargeValue.Location = new System.Drawing.Point(592, 0);
            this.lblLinesShippingChargeValue.Name = "lblLinesShippingChargeValue";
            this.lblLinesShippingChargeValue.Size = new System.Drawing.Size(100, 31);
            this.lblLinesShippingChargeValue.TabIndex = 0;
            this.lblLinesShippingChargeValue.Text = "$0";
            this.lblLinesShippingChargeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPageDown
            // 
            this.btnPageDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageDown.Appearance.Options.UseFont = true;
            this.btnPageDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.bottom;
            this.btnPageDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageDown.Location = new System.Drawing.Point(640, 508);
            this.btnPageDown.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.btnPageDown.Name = "btnPageDown";
            this.btnPageDown.Size = new System.Drawing.Size(57, 57);
            this.btnPageDown.TabIndex = 13;
            this.btnPageDown.Text = "Ê";
            this.btnPageDown.Click += new System.EventHandler(this.btnPageDown_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(577, 508);
            this.btnDown.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 12;
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(66, 508);
            this.btnUp.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnPageUp
            // 
            this.btnPageUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageUp.Appearance.Options.UseFont = true;
            this.btnPageUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.top;
            this.btnPageUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageUp.Location = new System.Drawing.Point(3, 508);
            this.btnPageUp.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.btnPageUp.Name = "btnPageUp";
            this.btnPageUp.Size = new System.Drawing.Size(57, 57);
            this.btnPageUp.TabIndex = 8;
            this.btnPageUp.Text = "Ç";
            this.btnPageUp.Click += new System.EventHandler(this.btnPageUp_Click);
            // 
            // DeliveryInformationPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.tableLayoutPanel);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.bindingSource, "HasDeliveryInformation", true));
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DeliveryInformationPage";
            this.Text = "Shipping and delivery";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.pnlTotals.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPickupAll;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnShipAll;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colItemOverview;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn colShipAll;
        private DevExpress.XtraGrid.Columns.GridColumn colPickupAll;
        private DevExpress.XtraGrid.Columns.GridColumn colLineStatus;
        private DevExpress.XtraGrid.GridControl gridItems;
        private System.Windows.Forms.TableLayoutPanel pnlTotals;
        private System.Windows.Forms.Label lblTotalShippingChargeValue;
        private System.Windows.Forms.Label lblTotalShippingCharge;
        private System.Windows.Forms.Label lblOrderShippingChargeValue;
        private System.Windows.Forms.Label lblOrderShippingCharge;
        private System.Windows.Forms.Label lblLinesShippingCharge;
        private System.Windows.Forms.Label lblLinesShippingChargeValue;
    }
}
