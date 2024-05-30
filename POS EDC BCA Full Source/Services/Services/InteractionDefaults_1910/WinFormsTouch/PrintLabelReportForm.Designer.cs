/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class PrintLabelReportForm
    {
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

            viewModel.UnHookPeripherals();

            if (viewModel.processingDialog != null)
            {
                viewModel.processingDialog.FormClosed -= processingDialog_FormClosed;
                viewModel.processingDialog.Dispose();
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
            this.basePanel = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeading = new System.Windows.Forms.Label();
            this.grItems = new DevExpress.XtraGrid.GridControl();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grdView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnRemove = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnEnterQuantity = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPrint = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dtReportDate = new System.Windows.Forms.DateTimePicker();
            this.numPad1 = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.btnSetupPrinter = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.tableLayoutPanel1);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(869, 639);
            this.basePanel.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelHeading, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grItems, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(865, 635);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelHeading
            // 
            this.labelHeading.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelHeading.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelHeading, 2);
            this.labelHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelHeading.Location = new System.Drawing.Point(249, 40);
            this.labelHeading.Name = "labelHeading";
            this.labelHeading.Size = new System.Drawing.Size(366, 65);
            this.labelHeading.TabIndex = 0;
            this.labelHeading.Text = "Print label report";
            this.labelHeading.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // grItems
            // 
            this.grItems.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grItems.DataMember = "Items";
            this.grItems.DataSource = this.bindingSource;
            this.grItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grItems.Location = new System.Drawing.Point(29, 108);
            this.grItems.MainView = this.grdView;
            this.grItems.Name = "grItems";
            this.grItems.Size = new System.Drawing.Size(553, 371);
            this.grItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdView});
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.PrintLabelReportViewModel);
            // 
            // grdView
            // 
            this.grdView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdView.Appearance.HeaderPanel.Options.UseFont = true;
            this.grdView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grdView.Appearance.Row.Options.UseFont = true;
            this.grdView.ColumnPanelRowHeight = 40;
            this.grdView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItemName,
            this.colItemId,
            this.colQuantity,
            this.colStatus});
            this.grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.grdView.GridControl = this.grItems;
            this.grdView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.grdView.Name = "grdView";
            this.grdView.OptionsBehavior.Editable = false;
            this.grdView.OptionsCustomization.AllowFilter = false;
            this.grdView.OptionsMenu.EnableColumnMenu = false;
            this.grdView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdView.OptionsView.AutoCalcPreviewLineCount = true;
            this.grdView.OptionsView.ShowGroupPanel = false;
            this.grdView.OptionsView.ShowIndicator = false;
            this.grdView.OptionsView.ShowPreview = true;
            this.grdView.PreviewFieldName = "Comment";
            this.grdView.PreviewLineCount = 5;
            this.grdView.RowHeight = 40;
            this.grdView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.grdView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.grdView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // colItemName
            // 
            this.colItemName.Caption = "Description";
            this.colItemName.FieldName = "ItemName";
            this.colItemName.Name = "colItemName";
            this.colItemName.OptionsColumn.AllowEdit = false;
            this.colItemName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colItemName.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colItemName.Visible = true;
            this.colItemName.VisibleIndex = 1;
            this.colItemName.Width = 161;
            // 
            // colItemId
            // 
            this.colItemId.Caption = "Item number";
            this.colItemId.FieldName = "ItemId";
            this.colItemId.Name = "colItemId";
            this.colItemId.OptionsColumn.AllowEdit = false;
            this.colItemId.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colItemId.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colItemId.Visible = true;
            this.colItemId.VisibleIndex = 0;
            this.colItemId.Width = 161;
            // 
            // colQuantity
            // 
            this.colQuantity.Caption = "Quantity";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 2;
            this.colQuantity.Width = 98;
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Reason";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 3;
            this.colStatus.Width = 251;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnPgUp, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnUp, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRemove, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnEnterQuantity, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDown, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnPgDown, 7, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(29, 485);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(807, 65);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(3, 5);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Padding = new System.Windows.Forms.Padding(0);
            this.btnPgUp.Size = new System.Drawing.Size(57, 57);
            this.btnPgUp.TabIndex = 3;
            this.btnPgUp.Text = "З";
            this.btnPgUp.Click += new System.EventHandler(this.OnPageUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(66, 5);
            this.btnUp.Name = "btnUp";
            this.btnUp.Padding = new System.Windows.Forms.Padding(0);
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "с";
            this.btnUp.Click += new System.EventHandler(this.OnUp_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Appearance.Options.UseFont = true;
            this.btnRemove.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnRemove.Location = new System.Drawing.Point(273, 5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Padding = new System.Windows.Forms.Padding(0);
            this.btnRemove.Size = new System.Drawing.Size(127, 57);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.OnRemove_Click);
            // 
            // btnEnterQuantity
            // 
            this.btnEnterQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnterQuantity.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnterQuantity.Appearance.Options.UseFont = true;
            this.btnEnterQuantity.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnEnterQuantity.Location = new System.Drawing.Point(406, 5);
            this.btnEnterQuantity.Name = "btnEnterQuantity";
            this.btnEnterQuantity.Padding = new System.Windows.Forms.Padding(0);
            this.btnEnterQuantity.Size = new System.Drawing.Size(127, 57);
            this.btnEnterQuantity.TabIndex = 6;
            this.btnEnterQuantity.Text = "Enter quantity";
            this.btnEnterQuantity.Click += new System.EventHandler(this.OnEnterQuantity_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(683, 5);
            this.btnDown.Name = "btnDown";
            this.btnDown.Padding = new System.Windows.Forms.Padding(0);
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 10;
            this.btnDown.Text = "т";
            this.btnDown.Click += new System.EventHandler(this.OnDown_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(747, 5);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Padding = new System.Windows.Forms.Padding(0);
            this.btnPgDown.Size = new System.Drawing.Size(57, 57);
            this.btnPgDown.TabIndex = 11;
            this.btnPgDown.Text = "К";
            this.btnPgDown.Click += new System.EventHandler(this.OnPageDown_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnPrint, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSearch, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(29, 556);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(807, 65);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Appearance.Options.UseFont = true;
            this.btnPrint.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPrint.Location = new System.Drawing.Point(207, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Padding = new System.Windows.Forms.Padding(0);
            this.btnPrint.Size = new System.Drawing.Size(127, 57);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.OnPrint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(340, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Padding = new System.Windows.Forms.Padding(0);
            this.btnSearch.Size = new System.Drawing.Size(127, 57);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.OnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClose.Location = new System.Drawing.Point(473, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(0);
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.dtReportDate, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.numPad1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnSetupPrinter, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(588, 108);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(248, 371);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // dtReportDate
            // 
            this.dtReportDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtReportDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "ReportDate", true));
            this.dtReportDate.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.dtReportDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtReportDate.Location = new System.Drawing.Point(3, 3);
            this.dtReportDate.Name = "dtReportDate";
            this.dtReportDate.Size = new System.Drawing.Size(242, 33);
            this.dtReportDate.TabIndex = 0;
            // 
            // numPad1
            // 
            this.numPad1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numPad1.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPad1.Appearance.Options.UseFont = true;
            this.numPad1.AutoSize = true;
            this.numPad1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numPad1.CurrencyCode = null;
            this.numPad1.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPad1.EnteredValue = "";
            this.numPad1.Location = new System.Drawing.Point(4, 43);
            this.numPad1.Margin = new System.Windows.Forms.Padding(4);
            this.numPad1.MaskChar = "";
            this.numPad1.MaskInterval = 0;
            this.numPad1.MaxNumberOfDigits = 13;
            this.numPad1.MinimumSize = new System.Drawing.Size(248, 314);
            this.numPad1.Name = "numPad1";
            this.numPad1.NegativeMode = false;
            this.numPad1.NoOfTries = 0;
            this.numPad1.NumberOfDecimals = 2;
            this.numPad1.Padding = new System.Windows.Forms.Padding(0, 10, 3, 0);
            this.numPad1.PromptText = null;
            this.numPad1.ShortcutKeysActive = false;
            this.numPad1.Size = new System.Drawing.Size(248, 314);
            this.numPad1.TabIndex = 1;
            this.numPad1.TimerEnabled = true;
            this.numPad1.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.OnNumPadEnter_Pressed);
            // 
            // btnSetupPrinter
            // 
            this.btnSetupPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetupPrinter.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetupPrinter.Appearance.Options.UseFont = true;
            this.btnSetupPrinter.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSetupPrinter.Location = new System.Drawing.Point(3, 364);
            this.btnSetupPrinter.Name = "btnSetupPrinter";
            this.btnSetupPrinter.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.btnSetupPrinter.Size = new System.Drawing.Size(242, 4);
            this.btnSetupPrinter.TabIndex = 12;
            this.btnSetupPrinter.Text = "Setup printer";
            this.btnSetupPrinter.Click += new System.EventHandler(this.OnSetupPrinter_Click);
            // 
            // PrintLabelReportForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(869, 639);
            this.Controls.Add(this.basePanel);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "PrintLabelReportForm";
            this.Text = "PrintLabelReportForm";
            this.Controls.SetChildIndex(this.basePanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private PanelControl basePanel;
        private System.ComponentModel.IContainer components = null;
        private DevExpress.XtraGrid.GridControl grItems;
        private DevExpress.XtraGrid.Views.Grid.GridView grdView;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPrint;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSetupPrinter;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private Label labelHeading;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSearch;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnRemove;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnEnterQuantity;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numPad1;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colItemId;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private System.Windows.Forms.BindingSource bindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colItemName;
        private System.Windows.Forms.DateTimePicker dtReportDate;
    }
}