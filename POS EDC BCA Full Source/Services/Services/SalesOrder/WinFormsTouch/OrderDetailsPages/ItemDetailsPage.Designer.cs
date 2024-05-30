/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class ItemDetailsPage
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
            this.gridItems = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQtyOrdered = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrev = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPickupQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPickAll = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.SalesOrder.ItemDetailsViewModel);
            // 
            // gridItems
            // 
            this.gridItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridItems.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridItems.Location = new System.Drawing.Point(3, 5);
            this.gridItems.MainView = this.gridView;
            this.gridItems.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.gridItems.Name = "gridItems";
            this.gridItems.Size = new System.Drawing.Size(697, 430);
            this.gridItems.TabIndex = 2;
            this.gridItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridItems.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridView_KeyUp);
            // 
            // gridView
            // 
            this.gridView.Appearance.FooterPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.FooterPanel.Options.UseFont = true;
            this.gridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.gridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Appearance.Preview.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Preview.Options.UseFont = true;
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.ColumnPanelRowHeight = 40;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemId,
            this.colDescription,
            this.colQtyOrdered,
            this.colPrev,
            this.colPickupQty});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.FooterPanelHeight = 40;
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
            this.gridView.PreviewFieldName = "MoreInfo";
            this.gridView.PreviewIndent = 20;
            this.gridView.PreviewLineCount = 1;
            this.gridView.RowHeight = 40;
            this.gridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView_CustomDrawCell);
            this.gridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView_CustomColumnDisplayText);
            this.gridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseDown);
            this.gridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseUp);
            this.gridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseMove);
            // 
            // colItemId
            // 
            this.colItemId.Caption = "Item number";
            this.colItemId.FieldName = "ItemId";
            this.colItemId.Name = "colItemId";
            this.colItemId.OptionsColumn.AllowEdit = false;
            this.colItemId.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colItemId.OptionsColumn.ShowInCustomizationForm = false;
            this.colItemId.Visible = true;
            this.colItemId.VisibleIndex = 0;
            this.colItemId.Width = 100;
            // 
            // colDescription
            // 
            this.colDescription.AppearanceCell.Options.UseTextOptions = true;
            this.colDescription.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colDescription.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDescription.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.colDescription.Caption = "Item name";
            this.colDescription.FieldName = "Description";
            this.colDescription.MinWidth = 180;
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.OptionsColumn.AllowMove = false;
            this.colDescription.OptionsColumn.AllowShowHide = false;
            this.colDescription.OptionsColumn.AllowSize = false;
            this.colDescription.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colDescription.OptionsFilter.AllowAutoFilter = false;
            this.colDescription.OptionsFilter.AllowFilter = false;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 1;
            this.colDescription.Width = 194;
            // 
            // colQtyOrdered
            // 
            this.colQtyOrdered.AppearanceCell.Options.UseTextOptions = true;
            this.colQtyOrdered.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQtyOrdered.AppearanceHeader.Options.UseTextOptions = true;
            this.colQtyOrdered.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQtyOrdered.Caption = "Quantity ordered";
            this.colQtyOrdered.FieldName = "QuantityOrdered";
            this.colQtyOrdered.Name = "colQtyOrdered";
            this.colQtyOrdered.Visible = true;
            this.colQtyOrdered.VisibleIndex = 2;
            this.colQtyOrdered.Width = 119;
            // 
            // colPrev
            // 
            this.colPrev.AppearanceCell.Options.UseTextOptions = true;
            this.colPrev.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPrev.AppearanceHeader.Options.UseTextOptions = true;
            this.colPrev.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPrev.Caption = "Picked up quantity";
            this.colPrev.FieldName = "QuantityPreviouslyPickedUp";
            this.colPrev.MinWidth = 130;
            this.colPrev.Name = "colPrev";
            this.colPrev.Visible = true;
            this.colPrev.VisibleIndex = 3;
            this.colPrev.Width = 135;
            // 
            // colPickupQty
            // 
            this.colPickupQty.AppearanceCell.Options.UseTextOptions = true;
            this.colPickupQty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPickupQty.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colPickupQty.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPickupQty.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.colPickupQty.AppearanceHeader.Options.UseTextOptions = true;
            this.colPickupQty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPickupQty.Caption = "Pickup quantity";
            this.colPickupQty.FieldName = "PickupQuantity";
            this.colPickupQty.MaxWidth = 150;
            this.colPickupQty.MinWidth = 130;
            this.colPickupQty.Name = "colPickupQty";
            this.colPickupQty.OptionsColumn.AllowEdit = false;
            this.colPickupQty.OptionsColumn.AllowMove = false;
            this.colPickupQty.OptionsColumn.AllowShowHide = false;
            this.colPickupQty.OptionsColumn.AllowSize = false;
            this.colPickupQty.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPickupQty.OptionsFilter.AllowAutoFilter = false;
            this.colPickupQty.OptionsFilter.AllowFilter = false;
            this.colPickupQty.Visible = true;
            this.colPickupQty.VisibleIndex = 4;
            this.colPickupQty.Width = 145;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnPickAll, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gridItems, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(703, 501);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnPickAll
            // 
            this.btnPickAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPickAll.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPickAll.Appearance.Options.UseFont = true;
            this.btnPickAll.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "AnyItemsForCurrentStore", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnPickAll.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPickAll.Location = new System.Drawing.Point(556, 441);
            this.btnPickAll.Name = "btnPickAll";
            this.btnPickAll.Size = new System.Drawing.Size(144, 57);
            this.btnPickAll.TabIndex = 5;
            this.btnPickAll.Text = "Pick up all items";
            this.btnPickAll.Click += new System.EventHandler(this.OnPickAll_Click);
            // 
            // ItemDetailsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ItemDetailsPage";
            this.Size = new System.Drawing.Size(703, 501);
            this.Text = "Item details";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridItems;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colItemId;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colPickupQty;
        private DevExpress.XtraGrid.Columns.GridColumn colQtyOrdered;
        private DevExpress.XtraGrid.Columns.GridColumn colPrev;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPickAll;
    }
}
