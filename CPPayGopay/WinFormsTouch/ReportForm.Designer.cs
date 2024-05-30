/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Drawing;
/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/
namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    partial class ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tblReportParameters = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutReportBtns = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRunReport = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSave = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutReportList = new System.Windows.Forms.TableLayoutPanel();
            this.gridReportList = new DevExpress.XtraGrid.GridControl();
            this.gridReportListView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblReportHeader = new System.Windows.Forms.Label();
            this.tblReportResult = new System.Windows.Forms.TableLayoutPanel();
            this.btnGrid = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnChart = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chartContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblParametersHeader = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutReportBtns.SuspendLayout();
            this.tableLayoutReportList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReportList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridReportListView)).BeginInit();
            this.tblReportResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.tblReportParameters, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutReportBtns, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutReportList, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblReportHeader, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tblReportResult, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblParametersHeader, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 12);
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tblReportParameters
            // 
            this.tblReportParameters.AutoScroll = true;
            this.tblReportParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblReportParameters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.tblReportParameters.Location = new System.Drawing.Point(30, 399);
            this.tblReportParameters.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.tblReportParameters.Name = "tblReportParameters";
            this.tblReportParameters.Size = new System.Drawing.Size(229, 267);
            this.tblReportParameters.TabIndex = 8;
            // 
            // flowLayoutReportBtns
            // 
            this.flowLayoutReportBtns.AutoScroll = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutReportBtns, 2);
            this.flowLayoutReportBtns.Controls.Add(this.btnRunReport);
            this.flowLayoutReportBtns.Controls.Add(this.btnSave);
            this.flowLayoutReportBtns.Controls.Add(this.btnClose);
            this.flowLayoutReportBtns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutReportBtns.Location = new System.Drawing.Point(33, 669);
            this.flowLayoutReportBtns.Name = "flowLayoutReportBtns";
            this.flowLayoutReportBtns.Padding = new System.Windows.Forms.Padding(300, 0, 300, 0);
            this.flowLayoutReportBtns.Size = new System.Drawing.Size(958, 44);
            this.flowLayoutReportBtns.TabIndex = 2;
            // 
            // btnRunReport
            // 
            this.btnRunReport.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunReport.Appearance.Options.UseFont = true;
            this.btnRunReport.Location = new System.Drawing.Point(304, 4);
            this.btnRunReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnRunReport.Name = "btnRunReport";
            this.btnRunReport.Size = new System.Drawing.Size(100, 36);
            this.btnRunReport.TabIndex = 0;
            this.btnRunReport.Text = "Run";
            this.btnRunReport.Click += new System.EventHandler(this.runReport_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(412, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 36);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save As";
            this.btnSave.Click += new System.EventHandler(this.save_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(520, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tableLayoutReportList
            // 
            this.tableLayoutReportList.AutoScroll = true;
            this.tableLayoutReportList.ColumnCount = 2;
            this.tableLayoutReportList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutReportList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutReportList.Controls.Add(this.gridReportList, 0, 0);
            this.tableLayoutReportList.Controls.Add(this.btnUp, 0, 1);
            this.tableLayoutReportList.Controls.Add(this.btnDown, 1, 1);
            this.tableLayoutReportList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutReportList.Location = new System.Drawing.Point(30, 92);
            this.tableLayoutReportList.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.tableLayoutReportList.Name = "tableLayoutReportList";
            this.tableLayoutReportList.RowCount = 2;
            this.tableLayoutReportList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutReportList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutReportList.Size = new System.Drawing.Size(229, 267);
            this.tableLayoutReportList.TabIndex = 20;
            // 
            // gridReportList
            // 
            this.gridReportList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutReportList.SetColumnSpan(this.gridReportList, 2);
            this.gridReportList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReportList.Location = new System.Drawing.Point(3, 3);
            this.gridReportList.MainView = this.gridReportListView;
            this.gridReportList.Name = "gridReportList";
            this.gridReportList.Size = new System.Drawing.Size(223, 211);
            this.gridReportList.TabIndex = 0;
            this.gridReportList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridReportListView});
            // 
            // gridReportListView
            // 
            this.gridReportListView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gridReportListView.Appearance.Row.Options.UseFont = true;
            this.gridReportListView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName,
            this.colValue});
            this.gridReportListView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridReportListView.GridControl = this.gridReportList;
            this.gridReportListView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridReportListView.Name = "gridReportListView";
            this.gridReportListView.OptionsBehavior.Editable = false;
            this.gridReportListView.OptionsCustomization.AllowColumnMoving = false;
            this.gridReportListView.OptionsCustomization.AllowColumnResizing = false;
            this.gridReportListView.OptionsCustomization.AllowFilter = false;
            this.gridReportListView.OptionsCustomization.AllowGroup = false;
            this.gridReportListView.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridReportListView.OptionsCustomization.AllowSort = false;
            this.gridReportListView.OptionsMenu.EnableColumnMenu = false;
            this.gridReportListView.OptionsMenu.EnableFooterMenu = false;
            this.gridReportListView.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridReportListView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridReportListView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridReportListView.OptionsView.ShowColumnHeaders = false;
            this.gridReportListView.OptionsView.ShowGroupPanel = false;
            this.gridReportListView.OptionsView.ShowIndicator = false;
            this.gridReportListView.RowHeight = 40;
            this.gridReportListView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridReportListView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridReportListView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridReportListView_RowClick);
            this.gridReportListView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.OnGridView_FocusedRowChanged);
            // 
            // colName
            // 
            this.colName.AppearanceCell.Options.UseTextOptions = true;
            this.colName.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colName.FieldName = "ReportId";
            this.colName.MinWidth = 125;
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            this.colName.Width = 125;
            // 
            // colValue
            // 
            this.colValue.AppearanceCell.Options.UseTextOptions = true;
            this.colValue.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colValue.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colValue.FieldName = "ReportTitle";
            this.colValue.MinWidth = 125;
            this.colValue.Name = "colValue";
            this.colValue.Visible = true;
            this.colValue.VisibleIndex = 1;
            this.colValue.Width = 125;
            // 
            // btnUp
            // 
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(3, 220);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 44);
            this.btnUp.TabIndex = 2;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.OnBtnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(161, 220);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 44);
            this.btnDown.TabIndex = 3;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.OnBtnDown_Click);
            // 
            // lblReportHeader
            // 
            this.lblReportHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblReportHeader.AutoSize = true;
            this.lblReportHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportHeader.Location = new System.Drawing.Point(33, 54);
            this.lblReportHeader.Name = "lblReportHeader";
            this.lblReportHeader.Size = new System.Drawing.Size(81, 25);
            this.lblReportHeader.TabIndex = 11;
            this.lblReportHeader.Text = "Reports";
            // 
            // tblReportResult
            // 
            this.tblReportResult.AutoScroll = true;
            this.tblReportResult.BackColor = System.Drawing.Color.White;
            this.tblReportResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblReportResult.ColumnCount = 3;
            this.tblReportResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblReportResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tblReportResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tblReportResult.Controls.Add(this.btnGrid, 1, 0);
            this.tblReportResult.Controls.Add(this.btnChart, 2, 0);
            this.tblReportResult.Controls.Add(this.gridControl1, 0, 1);
            this.tblReportResult.Controls.Add(this.chartContainer, 0, 1);
            this.tblReportResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblReportResult.Location = new System.Drawing.Point(271, 57);
            this.tblReportResult.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.tblReportResult.Name = "tblReportResult";
            this.tblReportResult.RowCount = 2;
            this.tableLayoutPanel1.SetRowSpan(this.tblReportResult, 4);
            this.tblReportResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tblReportResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblReportResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblReportResult.Size = new System.Drawing.Size(723, 609);
            this.tblReportResult.TabIndex = 20;
            // 
            // btnGrid
            // 
            this.btnGrid.Image = ((System.Drawing.Image)(resources.GetObject("btnGrid.Image")));
            this.btnGrid.Location = new System.Drawing.Point(624, 3);
            this.btnGrid.Name = "btnGrid";
            this.btnGrid.Size = new System.Drawing.Size(44, 39);
            this.btnGrid.TabIndex = 0;
            this.btnGrid.Click += new System.EventHandler(this.OnGridButtonClick);
            // 
            // btnChart
            // 
            this.btnChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChart.Image = ((System.Drawing.Image)(resources.GetObject("btnChart.Image")));
            this.btnChart.Location = new System.Drawing.Point(674, 3);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(44, 39);
            this.btnChart.TabIndex = 0;
            this.btnChart.Click += new System.EventHandler(this.OnChartButtonClick);
            // 
            // gridControl1
            // 
            this.tblReportResult.SetColumnSpan(this.gridControl1, 3);
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 590);           
            this.gridControl1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(715, 14);
            this.gridControl1.TabIndex = 21;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;            
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView1.ViewCaptionHeight = 20;
            this.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView1_CustomColumnDisplayText);
            // 
            // chartContainer
            // 
            this.tblReportResult.SetColumnSpan(this.chartContainer, 3);
            this.chartContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartContainer.Location = new System.Drawing.Point(3, 48);
            this.chartContainer.Name = "chartContainer";
            this.chartContainer.Size = new System.Drawing.Size(715, 536);
            this.chartContainer.TabIndex = 23;
            // 
            // lblParametersHeader
            // 
            this.lblParametersHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblParametersHeader.AutoSize = true;
            this.lblParametersHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParametersHeader.Location = new System.Drawing.Point(33, 368);
            this.lblParametersHeader.Name = "lblParametersHeader";
            this.lblParametersHeader.Size = new System.Drawing.Size(96, 21);
            this.lblParametersHeader.TabIndex = 12;
            this.lblParametersHeader.Text = "Parameters";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModel.ReportViewModel);
            // 
            // ReportForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "ReportForm";
            this.Text = "ReportForm";
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutReportBtns.ResumeLayout(false);
            this.tableLayoutReportList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReportList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridReportListView)).EndInit();
            this.tblReportResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        } 

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.FlowLayoutPanel tblReportParameters;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.TableLayoutPanel tblReportResult;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSave;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutReportBtns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutReportList;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private System.Windows.Forms.Label lblReportHeader;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnGrid;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnChart;        
        private System.Windows.Forms.FlowLayoutPanel chartContainer;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnRunReport;
        private System.Windows.Forms.Label lblParametersHeader;
        private DevExpress.XtraGrid.GridControl gridReportList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridReportListView;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colValue;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
    }
}