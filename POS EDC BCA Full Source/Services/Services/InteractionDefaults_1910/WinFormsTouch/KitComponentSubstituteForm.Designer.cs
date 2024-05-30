/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    partial class KitComponentSubstituteForm
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
            this.basePanel = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblHeading = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSelect = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnGetprice = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lblProducts = new DevExpress.XtraEditors.LabelControl();
            this.grKitProductSubstitute = new DevExpress.XtraGrid.GridControl();
            this.grdKitProductSubstituteView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemUnitOfMeasure = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).BeginInit();
            this.basePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grKitProductSubstitute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdKitProductSubstituteView)).BeginInit();
            this.SuspendLayout();
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.panelControl1);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(1024, 768);
            this.basePanel.TabIndex = 4;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1020, 764);
            this.panelControl1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1016, 760);
            this.tableLayoutPanel2.TabIndex = 22;
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(244, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(527, 95);
            this.lblHeading.TabIndex = 24;
            this.lblHeading.Tag = "";
            this.lblHeading.Text = "Item number: item name";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 9;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnPgDown, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnPgUp, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnUp, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDown, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSelect, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnGetprice, 4, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(26, 684);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(964, 65);
            this.tableLayoutPanel3.TabIndex = 23;
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(895, 4);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(65, 57);
            this.btnPgDown.TabIndex = 6;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(4, 4);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(65, 57);
            this.btnPgUp.TabIndex = 0;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(77, 4);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 57);
            this.btnUp.TabIndex = 1;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(822, 4);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 57);
            this.btnDown.TabIndex = 5;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSelect.Appearance.Options.UseFont = true;
            this.btnSelect.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSelect.Location = new System.Drawing.Point(282, 4);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(127, 57);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Tag = "";
            this.btnSelect.Text = "OK";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCancel.Location = new System.Drawing.Point(552, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 57);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Tag = "";
            this.btnCancel.Text = "Cancel";
            // 
            // btnGetprice
            // 
            this.btnGetprice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetprice.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetprice.Appearance.Options.UseFont = true;
            this.btnGetprice.Location = new System.Drawing.Point(417, 4);
            this.btnGetprice.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetprice.Name = "btnGetprice";
            this.btnGetprice.Size = new System.Drawing.Size(127, 57);
            this.btnGetprice.TabIndex = 3;
            this.btnGetprice.Tag = "";
            this.btnGetprice.Text = "Show price";
            this.btnGetprice.Click += new System.EventHandler(this.btnGetprice_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 8;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Controls.Add(this.lblProducts, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.grKitProductSubstitute, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(30, 139);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(956, 530);
            this.tableLayoutPanel6.TabIndex = 21;
            // 
            // lblProducts
            // 
            this.lblProducts.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProducts.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblProducts.Location = new System.Drawing.Point(0, 3);
            this.lblProducts.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblProducts.Name = "lblProducts";
            this.lblProducts.Size = new System.Drawing.Size(65, 21);
            this.lblProducts.TabIndex = 21;
            this.lblProducts.Text = "Products";
            // 
            // grKitProductSubstitute
            // 
            this.grKitProductSubstitute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel6.SetColumnSpan(this.grKitProductSubstitute, 8);
            this.grKitProductSubstitute.Cursor = System.Windows.Forms.Cursors.Default;
            this.grKitProductSubstitute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grKitProductSubstitute.Location = new System.Drawing.Point(0, 45);
            this.grKitProductSubstitute.MainView = this.grdKitProductSubstituteView;
            this.grKitProductSubstitute.Margin = new System.Windows.Forms.Padding(0);
            this.grKitProductSubstitute.Name = "grKitProductSubstitute";
            this.grKitProductSubstitute.Size = new System.Drawing.Size(956, 485);
            this.grKitProductSubstitute.TabIndex = 20;
            this.grKitProductSubstitute.TabStop = false;
            this.grKitProductSubstitute.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdKitProductSubstituteView});
            // 
            // grdKitProductSubstituteView
            // 
            this.grdKitProductSubstituteView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grdKitProductSubstituteView.Appearance.HeaderPanel.Options.UseFont = true;
            this.grdKitProductSubstituteView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grdKitProductSubstituteView.Appearance.Row.Options.UseFont = true;
            this.grdKitProductSubstituteView.ColumnPanelRowHeight = 40;
            this.grdKitProductSubstituteView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemId,
            this.colItemName,
            this.colQuantity,
            this.colItemUnitOfMeasure,
            this.colItemPrice});
            this.grdKitProductSubstituteView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.grdKitProductSubstituteView.GridControl = this.grKitProductSubstitute;
            this.grdKitProductSubstituteView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.grdKitProductSubstituteView.Name = "grdKitProductSubstituteView";
            this.grdKitProductSubstituteView.OptionsBehavior.Editable = false;
            this.grdKitProductSubstituteView.OptionsCustomization.AllowFilter = false;
            this.grdKitProductSubstituteView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdKitProductSubstituteView.OptionsView.ShowGroupPanel = false;
            this.grdKitProductSubstituteView.OptionsView.ShowIndicator = false;
            this.grdKitProductSubstituteView.RowHeight = 30;
            this.grdKitProductSubstituteView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.grdKitProductSubstituteView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.grdKitProductSubstituteView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.grdKitProductSubstituteView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.grdKitProductSubstituteView_FocusedRowChanged);
            this.grdKitProductSubstituteView.DoubleClick += new System.EventHandler(this.grdKitProductSubstituteView_DoubleClick);
            // 
            // colItemId
            // 
            this.colItemId.Caption = "Number";
            this.colItemId.FieldName = "Number";
            this.colItemId.Name = "colItemId";
            this.colItemId.OptionsColumn.AllowEdit = false;
            this.colItemId.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colItemId.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colItemId.Visible = true;
            this.colItemId.VisibleIndex = 0;
            this.colItemId.Width = 215;
            // 
            // colItemName
            // 
            this.colItemName.Caption = "Name";
            this.colItemName.FieldName = "Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.OptionsColumn.AllowEdit = false;
            this.colItemName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colItemName.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colItemName.Visible = true;
            this.colItemName.VisibleIndex = 1;
            this.colItemName.Width = 804;
            // 
            // colQuantity
            // 
            this.colQuantity.Caption = "Quantity";
            this.colQuantity.DisplayFormat.FormatString = "n2";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "QTY";
            this.colQuantity.MinWidth = 40;
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.OptionsColumn.AllowEdit = false;
            this.colQuantity.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 2;
            this.colQuantity.Width = 124;
            // 
            // colItemUnitOfMeasure
            // 
            this.colItemUnitOfMeasure.Caption = "Unit of measure";
            this.colItemUnitOfMeasure.FieldName = "UnitOfMeasure";
            this.colItemUnitOfMeasure.MinWidth = 40;
            this.colItemUnitOfMeasure.Name = "colItemUnitOfMeasure";
            this.colItemUnitOfMeasure.OptionsColumn.AllowEdit = false;
            this.colItemUnitOfMeasure.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colItemUnitOfMeasure.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colItemUnitOfMeasure.Visible = true;
            this.colItemUnitOfMeasure.VisibleIndex = 3;
            this.colItemUnitOfMeasure.Width = 239;
            // 
            // colItemPrice
            // 
            this.colItemPrice.AppearanceCell.Options.UseTextOptions = true;
            this.colItemPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colItemPrice.AppearanceHeader.Options.UseTextOptions = true;
            this.colItemPrice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colItemPrice.Caption = "Charge";
            this.colItemPrice.DisplayFormat.FormatString = "c2";
            this.colItemPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colItemPrice.FieldName = "Price";
            this.colItemPrice.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
            this.colItemPrice.Name = "colItemPrice";
            this.colItemPrice.OptionsColumn.AllowEdit = false;
            this.colItemPrice.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colItemPrice.Width = 69;
            // 
            // KitComponentSubstituteForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.basePanel);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "KitComponentSubstituteForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Kit Component Substitute ";
            this.Controls.SetChildIndex(this.basePanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).EndInit();
            this.basePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grKitProductSubstitute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdKitProductSubstituteView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl basePanel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnGetprice;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private DevExpress.XtraGrid.GridControl grKitProductSubstitute;
        private DevExpress.XtraGrid.Views.Grid.GridView grdKitProductSubstituteView;
        private DevExpress.XtraGrid.Columns.GridColumn colItemName;
        private DevExpress.XtraGrid.Columns.GridColumn colItemId;
        private DevExpress.XtraGrid.Columns.GridColumn colItemPrice;
        private DevExpress.XtraGrid.Columns.GridColumn colItemUnitOfMeasure;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraEditors.LabelControl lblProducts;
    }
}