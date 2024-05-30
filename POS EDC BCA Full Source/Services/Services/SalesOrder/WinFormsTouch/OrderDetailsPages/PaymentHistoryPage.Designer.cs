/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class PaymentHistoryPage
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
			this.gridPayments = new DevExpress.XtraGrid.GridControl();
			this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colDate = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colAmount = new DevExpress.XtraGrid.Columns.GridColumn();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridPayments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
			this.SuspendLayout();
			// 
			// bindingSource
			// 
			this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.SalesOrder.PaymentHistoryViewModel);
			// 
			// gridPayments
			// 
			this.gridPayments.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.gridPayments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridPayments.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.gridPayments.Location = new System.Drawing.Point(0, 0);
			this.gridPayments.MainView = this.gridView;
			this.gridPayments.Name = "gridPayments";
			this.gridPayments.Size = new System.Drawing.Size(703, 501);
			this.gridPayments.TabIndex = 4;
			this.gridPayments.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
			// 
			// gridView
			// 
			this.gridView.Appearance.FooterPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gridView.Appearance.FooterPanel.Options.UseFont = true;
			this.gridView.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.gridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
			this.gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
			this.gridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.gridView.Appearance.Row.Options.UseFont = true;
			this.gridView.Appearance.Row.Options.UseTextOptions = true;
			this.gridView.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.gridView.ColumnPanelRowHeight = 40;
			this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDate,
            this.colName,
            this.colAmount});
			this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
			this.gridView.FooterPanelHeight = 40;
			this.gridView.GridControl = this.gridPayments;
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
			this.gridView.OptionsView.ShowGroupPanel = false;
			this.gridView.OptionsView.ShowIndicator = false;
			this.gridView.RowHeight = 40;
			this.gridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
			this.gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
			// 
			// colDate
			// 
			this.colDate.AppearanceCell.Options.UseTextOptions = true;
			this.colDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colDate.AppearanceHeader.Options.UseTextOptions = true;
			this.colDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colDate.Caption = "Date";
			this.colDate.FieldName = "Date";
			this.colDate.MaxWidth = 100;
			this.colDate.MinWidth = 100;
			this.colDate.Name = "colDate";
			this.colDate.OptionsColumn.AllowEdit = false;
			this.colDate.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colDate.OptionsColumn.ShowInCustomizationForm = false;
			this.colDate.Visible = true;
			this.colDate.VisibleIndex = 0;
			this.colDate.Width = 100;
			// 
			// colName
			// 
			this.colName.AppearanceCell.Options.UseTextOptions = true;
			this.colName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.colName.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
			this.colName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.colName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.colName.AppearanceHeader.Options.UseTextOptions = true;
			this.colName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.colName.Caption = "Order number";
			this.colName.FieldName = "OrderNumber";
			this.colName.MaxWidth = 300;
			this.colName.MinWidth = 200;
			this.colName.Name = "colName";
			this.colName.OptionsColumn.AllowEdit = false;
			this.colName.OptionsColumn.AllowMove = false;
			this.colName.OptionsColumn.AllowShowHide = false;
			this.colName.OptionsColumn.AllowSize = false;
			this.colName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colName.OptionsFilter.AllowAutoFilter = false;
			this.colName.OptionsFilter.AllowFilter = false;
			this.colName.Visible = true;
			this.colName.VisibleIndex = 1;
			this.colName.Width = 200;
			// 
			// colAmount
			// 
			this.colAmount.AppearanceCell.Options.UseTextOptions = true;
			this.colAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.colAmount.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
			this.colAmount.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.colAmount.Caption = "Amount";
			this.colAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.colAmount.FieldName = "Amount";
			this.colAmount.MinWidth = 200;
			this.colAmount.Name = "colAmount";
			this.colAmount.Visible = true;
			this.colAmount.VisibleIndex = 2;
			this.colAmount.Width = 200;
			// 
			// PaymentHistoryPage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.Controls.Add(this.gridPayments);
			this.Name = "PaymentHistoryPage";
			this.Size = new System.Drawing.Size(703, 501);
			this.Text = "Payment history";
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridPayments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridPayments;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colAmount;
    }
}
