/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class ExtendedLogOnForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtendedLogOnForm));
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.colAssigned = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnAssign = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutFilter = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.txtSearchFilter = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnUnassign = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.gridOperators = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colOperatorID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchFilter.Properties)).BeginInit();
            this.tableLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOperators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // colAssigned
            // 
            this.colAssigned.Caption = "Assigned";
            this.colAssigned.FieldName = "ExtendedLogOnAssigned";
            this.colAssigned.Name = "colAssigned";
            this.colAssigned.OptionsColumn.AllowEdit = false;
            this.colAssigned.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // btnAssign
            // 
            this.btnAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssign.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssign.Appearance.Options.UseFont = true;
            this.btnAssign.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "SelectedResult", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnAssign.Location = new System.Drawing.Point(287, 4);
            this.btnAssign.Margin = new System.Windows.Forms.Padding(4);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(127, 57);
            this.btnAssign.TabIndex = 2;
            this.btnAssign.Text = "Assign";
            this.btnAssign.Click += new System.EventHandler(this.OnBtnAssign_Click);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.ExtendedLogOnViewModel);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(557, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.AutoSize = true;
            this.tableLayoutPanelMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutFilter, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelButtons, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.gridOperators, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // tableLayoutFilter
            // 
            this.tableLayoutFilter.AutoSize = true;
            this.tableLayoutFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutFilter.ColumnCount = 3;
            this.tableLayoutFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutFilter.Controls.Add(this.btnSearch, 1, 0);
            this.tableLayoutFilter.Controls.Add(this.btnClear, 2, 0);
            this.tableLayoutFilter.Controls.Add(this.txtSearchFilter, 0, 0);
            this.tableLayoutFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutFilter.Location = new System.Drawing.Point(29, 138);
            this.tableLayoutFilter.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.tableLayoutFilter.Name = "tableLayoutFilter";
            this.tableLayoutFilter.RowCount = 1;
            this.tableLayoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutFilter.Size = new System.Drawing.Size(964, 38);
            this.tableLayoutFilter.TabIndex = 10;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearch.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.search;
            this.btnSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(837, 3);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 32);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.OnBtnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClear.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.remove;
            this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(904, 3);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(57, 32);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.OnBtnClear_Click);
            // 
            // txtSearchFilter
            // 
            this.txtSearchFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchFilter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SearchFilter", true));
            this.txtSearchFilter.Location = new System.Drawing.Point(3, 3);
            this.txtSearchFilter.Name = "txtSearchFilter";
            this.txtSearchFilter.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtSearchFilter.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtSearchFilter.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtSearchFilter.Properties.Appearance.Options.UseBackColor = true;
            this.txtSearchFilter.Properties.Appearance.Options.UseFont = true;
            this.txtSearchFilter.Properties.Appearance.Options.UseForeColor = true;
            this.txtSearchFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSearchFilter.Size = new System.Drawing.Size(828, 32);
            this.txtSearchFilter.TabIndex = 0;
            this.txtSearchFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OntxtSearchFilter_KeyDown);
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.AutoSize = true;
            this.tableLayoutPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelButtons.ColumnCount = 9;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelButtons.Controls.Add(this.btnAssign, 3, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnClose, 5, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnUnassign, 4, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnDown, 1, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnPgDown, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnPgUp, 8, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnUp, 7, 0);
            this.tableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(26, 692);
            this.tableLayoutPanelButtons.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 1;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(972, 65);
            this.tableLayoutPanelButtons.TabIndex = 7;
            // 
            // btnUnassign
            // 
            this.btnUnassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnassign.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnassign.Appearance.Options.UseFont = true;
            this.btnUnassign.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "SelectedResult", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnUnassign.Location = new System.Drawing.Point(422, 4);
            this.btnUnassign.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnassign.Name = "btnUnassign";
            this.btnUnassign.Size = new System.Drawing.Size(127, 57);
            this.btnUnassign.TabIndex = 3;
            this.btnUnassign.Text = "Unassign";
            this.btnUnassign.Click += new System.EventHandler(this.OnBtnUnassign_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(69, 4);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 5;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.OnBtnDown_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(4, 4);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(57, 57);
            this.btnPgDown.TabIndex = 6;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.OnBtnPgDown_Click);
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(910, 4);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(58, 57);
            this.btnPgUp.TabIndex = 0;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.OnBtnPgUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(845, 4);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 1;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.OnBtnUp_Click);
            // 
            // gridOperators
            // 
            this.gridOperators.DataMember = "Results";
            this.gridOperators.DataSource = this.bindingSource;
            this.gridOperators.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOperators.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gridOperators.Location = new System.Drawing.Point(30, 183);
            this.gridOperators.LookAndFeel.SkinName = "Money Twins";
            this.gridOperators.MainView = this.gridView;
            this.gridOperators.Margin = new System.Windows.Forms.Padding(4);
            this.gridOperators.Name = "gridOperators";
            this.gridOperators.Size = new System.Drawing.Size(964, 494);
            this.gridOperators.TabIndex = 8;
            this.gridOperators.TabStop = false;
            this.gridOperators.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridOperators.DoubleClick += new System.EventHandler(this.OnGridOperators_DoubleClick);
            // 
            // gridView
            // 
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.ColumnPanelRowHeight = 40;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOperatorID,
            this.colName,
            this.colAssigned});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            styleFormatCondition1.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            styleFormatCondition1.Appearance.Options.UseFont = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Column = this.colAssigned;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = true;
            this.gridView.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.gridView.GridControl = this.gridOperators;
            this.gridView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsCustomization.AllowFilter = false;
            this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.OptionsView.ShowIndicator = false;
            this.gridView.PaintStyleName = "Skin";
            this.gridView.RowHeight = 40;
            this.gridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.OnGridView_FocusedRowChanged);
            // 
            // colOperatorID
            // 
            this.colOperatorID.Caption = "OperatorID";
            this.colOperatorID.FieldName = "OperatorID";
            this.colOperatorID.Name = "colOperatorID";
            this.colOperatorID.OptionsColumn.AllowEdit = false;
            this.colOperatorID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colOperatorID.Visible = true;
            this.colOperatorID.VisibleIndex = 0;
            this.colOperatorID.Width = 125;
            // 
            // colName
            // 
            this.colName.Caption = "Name";
            this.colName.FieldName = "OperatorName";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            this.colName.Width = 371;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(334, 40);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeader.Size = new System.Drawing.Size(356, 95);
            this.lblHeader.TabIndex = 11;
            this.lblHeader.Text = "Extended Logon";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExtendedLogOnForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "ExtendedLogOnForm";
            this.Text = "Extended log on";
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tableLayoutFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchFilter.Properties)).EndInit();
            this.tableLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOperators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnAssign;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private DevExpress.XtraGrid.GridControl gridOperators;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colOperatorID;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutFilter;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.TextEdit txtSearchFilter;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private System.Windows.Forms.BindingSource bindingSource;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUnassign;
        private DevExpress.XtraGrid.Columns.GridColumn colAssigned;
        private System.Windows.Forms.Label lblHeader;
    }
}
