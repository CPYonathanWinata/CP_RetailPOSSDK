/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    partial class SaleRefundDetailForm
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
            this.lblHeading = new System.Windows.Forms.Label();
            this.refundAmountsGrid = new DevExpress.XtraGrid.GridControl();
            this.saleRefundDetailsViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTenderType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOriginalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReturnedEarlierAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReturnedAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAvailableAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tablePanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refundAmountsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saleRefundDetailsViewModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tablePanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.refundAmountsGrid, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tablePanelButtons, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(260, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(504, 95);
            this.lblHeading.TabIndex = 1;
            this.lblHeading.Text = "Display refund amounts";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // refundAmountsGrid
            // 
            this.refundAmountsGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.refundAmountsGrid.DataMember = "PaymentRefunds";
            this.refundAmountsGrid.DataSource = this.saleRefundDetailsViewModelBindingSource;
            this.refundAmountsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.refundAmountsGrid.Location = new System.Drawing.Point(30, 139);
            this.refundAmountsGrid.MainView = this.gridView1;
            this.refundAmountsGrid.Margin = new System.Windows.Forms.Padding(4);
            this.refundAmountsGrid.Name = "grIncomeExpenseAccounts";
            this.refundAmountsGrid.Size = new System.Drawing.Size(964, 538);
            this.refundAmountsGrid.TabIndex = 6;
            this.refundAmountsGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // saleRefundDetailsViewModelBindingSource
            // 
            this.saleRefundDetailsViewModelBindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.SaleRefundDetailsViewModel);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.ColumnPanelRowHeight = 40;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTenderType,
            this.colOriginalAmount,
            this.colReturnedEarlierAmount,
            this.colReturnedAmount,
            this.colAvailableAmount});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.FooterPanelHeight = 40;
            this.gridView1.GridControl = this.refundAmountsGrid;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowHeight = 40;
            this.gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // colTenderType
            // 
            this.colTenderType.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.colTenderType.AppearanceCell.Options.UseFont = true;
            this.colTenderType.Caption = "Payment method";
            this.colTenderType.FieldName = "EntryName";
            this.colTenderType.Name = "colTenderType";
            this.colTenderType.Visible = true;
            this.colTenderType.VisibleIndex = 0;
            this.colTenderType.Width = 100;
            // 
            // colPaidAmount
            // 
            this.colOriginalAmount.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.colOriginalAmount.AppearanceCell.Options.UseFont = true;
            this.colOriginalAmount.Caption = "Original amount";
            this.colOriginalAmount.DisplayFormat.FormatString = "0.00";
            this.colOriginalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOriginalAmount.FieldName = "OriginalAmount";
            this.colOriginalAmount.Name = "colOriginalAmount";
            this.colOriginalAmount.Visible = true;
            this.colOriginalAmount.VisibleIndex = 1;
            this.colOriginalAmount.Width = 100;
            // 
            // colReturnedEarlierAmount
            // 
            this.colReturnedEarlierAmount.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.colReturnedEarlierAmount.AppearanceCell.Options.UseFont = true;
            this.colReturnedEarlierAmount.Caption = "Amount returned earlier";
            this.colReturnedEarlierAmount.DisplayFormat.FormatString = "0.00";
            this.colReturnedEarlierAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colReturnedEarlierAmount.FieldName = "ReturnedEarlierAmount";
            this.colReturnedEarlierAmount.Name = "colReturnedEarlierAmount";
            this.colReturnedEarlierAmount.Visible = true;
            this.colReturnedEarlierAmount.VisibleIndex = 2;
            this.colReturnedEarlierAmount.Width = 100;
            // 
            // colReturnedAmount
            // 
            this.colReturnedAmount.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.colReturnedAmount.AppearanceCell.Options.UseFont = true;
            this.colReturnedAmount.Caption = "Returned amount";
            this.colReturnedAmount.DisplayFormat.FormatString = "0.00";
            this.colReturnedAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colReturnedAmount.FieldName = "ReturnedAmount";
            this.colReturnedAmount.Name = "colReturnedAmount";
            this.colReturnedAmount.Visible = true;
            this.colReturnedAmount.VisibleIndex = 3;
            this.colReturnedAmount.Width = 100;
            // 
            // colAvailableAmount
            // 
            this.colAvailableAmount.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.colAvailableAmount.AppearanceCell.Options.UseFont = true;
            this.colAvailableAmount.Caption = "Available amount";
            this.colAvailableAmount.DisplayFormat.FormatString = "0.00";
            this.colAvailableAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAvailableAmount.FieldName = "AvailableAmount";
            this.colAvailableAmount.Name = "colAvailableAmount";
            this.colAvailableAmount.Visible = true;
            this.colAvailableAmount.VisibleIndex = 4;
            this.colAvailableAmount.Width = 100;
            // 
            // tablePanelButtons
            // 
            this.tablePanelButtons.AutoSize = true;
            this.tablePanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tablePanelButtons.ColumnCount = 8;
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelButtons.Controls.Add(this.btnClose, 4, 0);
            this.tablePanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanelButtons.Location = new System.Drawing.Point(26, 692);
            this.tablePanelButtons.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tablePanelButtons.Name = "tablePanelButtons";
            this.tablePanelButtons.RowCount = 1;
            this.tablePanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelButtons.Size = new System.Drawing.Size(972, 65);
            this.tablePanelButtons.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(422, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            // 
            // SaleRefundDetailForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "SaleRefundDetailForm";
            this.Text = "Display refund amounts";
            this.Controls.SetChildIndex(this.tableLayoutPanel, 0);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refundAmountsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saleRefundDetailsViewModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tablePanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl refundAmountsGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colTenderType;
        private DevExpress.XtraGrid.Columns.GridColumn colOriginalAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colReturnedEarlierAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colReturnedAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colAvailableAmount;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tablePanelButtons;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.BindingSource saleRefundDetailsViewModelBindingSource;
    }
}