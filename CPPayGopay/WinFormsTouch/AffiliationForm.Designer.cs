/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class AffiliationForm
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
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOK = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grAffiliation = new DevExpress.XtraGrid.GridControl();
            this.gvAffiliation = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCheckEdit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanelHeading = new System.Windows.Forms.TableLayoutPanel();
            this.lblHeadDescription = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.repositoryItemCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grAffiliation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAffiliation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit)).BeginInit();
            this.tableLayoutPanelHeading.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanelHeading, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 9;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnCancel, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnPgUp, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnUp, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnPgDown, 8, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnDown, 7, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnOK, 4, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Font = new System.Drawing.Font("Webdings", 8.25F);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(26, 691);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(972, 66);
            this.tableLayoutPanel4.TabIndex = 17;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCancel.Location = new System.Drawing.Point(693, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Tag = "";
            this.btnCancel.Text = "Cancel";
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(4, 4);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(65, 58);
            this.btnPgUp.TabIndex = 10;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(77, 4);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 58);
            this.btnUp.TabIndex = 11;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(902, 4);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(66, 58);
            this.btnPgDown.TabIndex = 16;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(829, 4);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 58);
            this.btnDown.TabIndex = 15;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnOK.Location = new System.Drawing.Point(422, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(127, 57);
            this.btnOK.TabIndex = 13;
            this.btnOK.Tag = "";
            this.btnOK.Text = "OK";
            this.btnOK.Enabled = false;
            this.btnOK.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.AffiliationViewModel);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.grAffiliation, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(29, 149);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(966, 528);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grAffiliation
            // 
            this.grAffiliation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.SetColumnSpan(this.grAffiliation, 3);
            this.grAffiliation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grAffiliation.Location = new System.Drawing.Point(0, 0);
            this.grAffiliation.MainView = this.gvAffiliation;
            this.grAffiliation.Margin = new System.Windows.Forms.Padding(0);
            this.grAffiliation.Name = "grAffiliation";
            this.grAffiliation.Size = new System.Drawing.Size(966, 528);
            this.grAffiliation.TabIndex = 0;
            this.grAffiliation.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit,
            });
            this.grAffiliation.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAffiliation});
            this.grAffiliation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grItems_KeyDown);
            // 
            // gvAffiliation
            // 
            this.gvAffiliation.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.gvAffiliation.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvAffiliation.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gvAffiliation.Appearance.Row.Options.UseFont = true;
            this.gvAffiliation.ColumnPanelRowHeight = 40;
            this.gvAffiliation.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCheckEdit,
            this.colName,
            this.colDescription});
            this.gvAffiliation.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvAffiliation.GridControl = this.grAffiliation;
            this.gvAffiliation.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gvAffiliation.Name = "gvAffiliation";
            this.gvAffiliation.OptionsCustomization.AllowFilter = false;
            this.gvAffiliation.OptionsCustomization.AllowSort = false;
            this.gvAffiliation.OptionsDetail.EnableMasterViewMode = false;
            this.gvAffiliation.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvAffiliation.OptionsSelection.MultiSelect = false;
            this.gvAffiliation.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.gvAffiliation.OptionsView.ShowGroupPanel = false;
            this.gvAffiliation.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gvAffiliation.OptionsView.ShowIndicator = false;
            this.gvAffiliation.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gvAffiliation.RowHeight = 40;
            this.gvAffiliation.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;            
            this.gvAffiliation.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gvAffiliation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grItems_KeyDown);
            // 
            // colCheckEdit
            // 
            this.colCheckEdit.Caption = "Select";
            this.colCheckEdit.Name = "colCheckEdit";
            this.colCheckEdit.FieldName = "IsSelected";
            this.colCheckEdit.ShowUnboundExpressionMenu = true;
            this.colCheckEdit.Visible = true;
            this.colCheckEdit.VisibleIndex = 0;
            this.colCheckEdit.ColumnEdit = this.repositoryItemCheckEdit;
            this.colCheckEdit.Width = 313;
            this.colCheckEdit.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.colCheckEdit.ColumnEdit.EditValueChanged += new System.EventHandler(this.colCheckEdit_ValueChange);
            // 
            // colName
            // 
            this.colName.Caption = "Name";
            this.colName.FieldName = "Affiliation.Name";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colName.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            this.colName.Width = 213;
            // 
            // colDescription
            // 
            this.colDescription.Caption = "Description";
            this.colDescription.FieldName = "Affiliation.Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.OptionsColumn.AllowIncrementalSearch = false;
            this.colDescription.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDescription.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colDescription.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 2;
            this.colDescription.Width = 386;
            // 
            // tableLayoutPanelHeading
            // 
            this.tableLayoutPanelHeading.ColumnCount = 4;
            this.tableLayoutPanelHeading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeading.Controls.Add(this.lblHeadDescription, 1, 0);
            this.tableLayoutPanelHeading.Controls.Add(this.lblHeading, 2, 0);
            this.tableLayoutPanelHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHeading.Location = new System.Drawing.Point(29, 43);
            this.tableLayoutPanelHeading.Name = "tableLayoutPanelHeading";
            this.tableLayoutPanelHeading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelHeading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelHeading.Size = new System.Drawing.Size(966, 100);
            this.tableLayoutPanelHeading.TabIndex = 0;
            // 
            // lblHeadDescription
            // 
            this.lblHeadDescription.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeadDescription.AutoSize = true;
            this.lblHeadDescription.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold);
            this.lblHeadDescription.Location = new System.Drawing.Point(250, 2);
            this.lblHeadDescription.Margin = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeadDescription.Name = "lblHeadDescription";
            this.lblHeadDescription.Size = new System.Drawing.Size(231, 65);
            this.lblHeadDescription.TabIndex = 41;
            this.lblHeadDescription.Text = "Customer";
            this.lblHeadDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeadDescription.Visible = true;
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(481, 2);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(234, 95);
            this.lblHeading.TabIndex = 41;
            this.lblHeading.Text = "Affiliations";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // repositoryItemCheckEdit
            // 
            this.repositoryItemCheckEdit.AllowFocused = false;
            this.repositoryItemCheckEdit.AutoHeight = false;
            this.repositoryItemCheckEdit.Name = "repositoryItemCheckEdit";
            this.repositoryItemCheckEdit.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit.Tag = true;
            // 
            // AffiliationForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel2);
            this.ImeMode = System.Windows.Forms.ImeMode.Inherit;
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "AffiliationForm";
            this.Text = "frmAffiliation";
            this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grAffiliation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAffiliation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit)).EndInit();
            this.tableLayoutPanelHeading.ResumeLayout(false);
            this.tableLayoutPanelHeading.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl grAffiliation;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAffiliation;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colCheckEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeading;
        private System.Windows.Forms.Label lblHeadDescription;
        private System.Windows.Forms.BindingSource bindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit;
    }
}