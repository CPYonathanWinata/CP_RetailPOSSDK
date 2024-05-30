/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class ViewTimeClockEntriesForm
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
                if (storeTable != null)
                {
                    storeTable.Dispose();
                    storeTable = null;
                }
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
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.tlpOptions = new System.Windows.Forms.TableLayoutPanel();
            this.btnClockIn = new DevExpress.XtraEditors.CheckButton();
            this.btnClockOut = new DevExpress.XtraEditors.CheckButton();
            this.btnBreakForLunch = new DevExpress.XtraEditors.CheckButton();
            this.btnBreakFromWork = new DevExpress.XtraEditors.CheckButton();
            this.btnSelectStore = new DevExpress.XtraEditors.CheckButton();
            this.btnSelectAllStore = new DevExpress.XtraEditors.CheckButton();
            this.gridViewTimeClockEntries = new DevExpress.XtraGrid.GridControl();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRegistrationType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWorker = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPersonal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProfile = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClockInTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClockOutTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tlpContent.SuspendLayout();
            this.tlpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTimeClockEntries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 5;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.Controls.Add(this.labelTitle, 0, 0);
            this.tlpContent.Controls.Add(this.tlpOptions, 0, 1);
            this.tlpContent.Controls.Add(this.gridViewTimeClockEntries, 0, 2);
            this.tlpContent.Controls.Add(this.btnPgUp, 0, 4);
            this.tlpContent.Controls.Add(this.btnUp, 1, 4);
            this.tlpContent.Controls.Add(this.btnClose, 2, 4);
            this.tlpContent.Controls.Add(this.btnDown, 3, 4);
            this.tlpContent.Controls.Add(this.btnPgDown, 4, 4);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(0, 0);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tlpContent.RowCount = 5;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.Size = new System.Drawing.Size(1024, 753);
            this.tlpContent.TabIndex = 10;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.tlpContent.SetColumnSpan(this.labelTitle, 5);
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelTitle.Location = new System.Drawing.Point(271, 40);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(482, 65);
            this.labelTitle.TabIndex = 12;
            this.labelTitle.Text = "View time clock entries";
            // 
            // tlpOptions
            // 
            this.tlpOptions.ColumnCount = 6;
            this.tlpContent.SetColumnSpan(this.tlpOptions, 6);
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpOptions.Controls.Add(this.btnClockIn, 0, 0);
            this.tlpOptions.Controls.Add(this.btnClockOut, 1, 0);
            this.tlpOptions.Controls.Add(this.btnBreakForLunch, 2, 0);
            this.tlpOptions.Controls.Add(this.btnBreakFromWork, 3, 0);
            this.tlpOptions.Controls.Add(this.btnSelectStore, 4, 0);
            this.tlpOptions.Controls.Add(this.btnSelectAllStore, 5, 0);
            this.tlpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOptions.Location = new System.Drawing.Point(26, 155);
            this.tlpOptions.Margin = new System.Windows.Forms.Padding(0, 30, 0, 10);
            this.tlpOptions.Name = "tlpOptions";
            this.tlpOptions.RowCount = 1;
            this.tlpOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpOptions.Size = new System.Drawing.Size(972, 66);
            this.tlpOptions.TabIndex = 11;
            // 
            // btnClockIn
            // 
            this.btnClockIn.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClockIn.Appearance.Options.UseFont = true;
            this.btnClockIn.Checked = true;
            this.btnClockIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClockIn.GroupIndex = 0;
            this.btnClockIn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClockIn.Location = new System.Drawing.Point(4, 4);
            this.btnClockIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnClockIn.Name = "btnClockIn";
            this.btnClockIn.Padding = new System.Windows.Forms.Padding(4);
            this.btnClockIn.Size = new System.Drawing.Size(153, 58);
            this.btnClockIn.TabIndex = 0;
            this.btnClockIn.Text = "Clock in";
            this.btnClockIn.Click += new System.EventHandler(this.OnButtonClockIn_Click);
            // 
            // btnClockOut
            // 
            this.btnClockOut.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClockOut.Appearance.Options.UseFont = true;
            this.btnClockOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClockOut.GroupIndex = 0;
            this.btnClockOut.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClockOut.Location = new System.Drawing.Point(165, 4);
            this.btnClockOut.Margin = new System.Windows.Forms.Padding(4);
            this.btnClockOut.Name = "btnClockOut";
            this.btnClockOut.Padding = new System.Windows.Forms.Padding(4);
            this.btnClockOut.Size = new System.Drawing.Size(153, 58);
            this.btnClockOut.TabIndex = 1;
            this.btnClockOut.TabStop = false;
            this.btnClockOut.Text = "Clock out";
            this.btnClockOut.Click += new System.EventHandler(this.OnButtonClockOut_Click);
            // 
            // btnBreakForLunch
            // 
            this.btnBreakForLunch.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBreakForLunch.Appearance.Options.UseFont = true;
            this.btnBreakForLunch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBreakForLunch.GroupIndex = 0;
            this.btnBreakForLunch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnBreakForLunch.Location = new System.Drawing.Point(326, 4);
            this.btnBreakForLunch.Margin = new System.Windows.Forms.Padding(4);
            this.btnBreakForLunch.Name = "btnBreakForLunch";
            this.btnBreakForLunch.Padding = new System.Windows.Forms.Padding(4);
            this.btnBreakForLunch.Size = new System.Drawing.Size(153, 58);
            this.btnBreakForLunch.TabIndex = 2;
            this.btnBreakForLunch.TabStop = false;
            this.btnBreakForLunch.Text = "Break for lunch";
            this.btnBreakForLunch.Click += new System.EventHandler(this.OnButtonBreakForLunch_Click);
            // 
            // btnBreakFromWork
            // 
            this.btnBreakFromWork.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBreakFromWork.Appearance.Options.UseFont = true;
            this.btnBreakFromWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBreakFromWork.GroupIndex = 0;
            this.btnBreakFromWork.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnBreakFromWork.Location = new System.Drawing.Point(487, 4);
            this.btnBreakFromWork.Margin = new System.Windows.Forms.Padding(4);
            this.btnBreakFromWork.Name = "btnBreakFromWork";
            this.btnBreakFromWork.Padding = new System.Windows.Forms.Padding(4);
            this.btnBreakFromWork.Size = new System.Drawing.Size(153, 58);
            this.btnBreakFromWork.TabIndex = 3;
            this.btnBreakFromWork.TabStop = false;
            this.btnBreakFromWork.Text = "Break from work";
            this.btnBreakFromWork.Click += new System.EventHandler(this.OnButtonBreakFromWork_Click);
            // 
            // btnSelectStore
            // 
            this.btnSelectStore.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSelectStore.Appearance.Options.UseFont = true;
            this.btnSelectStore.Checked = true;
            this.btnSelectStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectStore.GroupIndex = 1;
            this.btnSelectStore.Location = new System.Drawing.Point(648, 4);
            this.btnSelectStore.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectStore.Name = "btnSelectStore";
            this.btnSelectStore.Padding = new System.Windows.Forms.Padding(4);
            this.btnSelectStore.Size = new System.Drawing.Size(153, 58);
            this.btnSelectStore.TabIndex = 4;
            this.btnSelectStore.Text = "Select store";
            this.btnSelectStore.Click += new System.EventHandler(this.OnButtonSelectStore_Click);
            // 
            // btnSelectAllStore
            // 
            this.btnSelectAllStore.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSelectAllStore.Appearance.Options.UseFont = true;
            this.btnSelectAllStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectAllStore.GroupIndex = 1;
            this.btnSelectAllStore.Location = new System.Drawing.Point(809, 4);
            this.btnSelectAllStore.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAllStore.Name = "btnSelectAllStore";
            this.btnSelectAllStore.Padding = new System.Windows.Forms.Padding(4);
            this.btnSelectAllStore.Size = new System.Drawing.Size(159, 58);
            this.btnSelectAllStore.TabIndex = 5;
            this.btnSelectAllStore.TabStop = false;
            this.btnSelectAllStore.Text = "Select all stores";
            this.btnSelectAllStore.Click += new System.EventHandler(this.OnButtonSelectAllStore_Click);
            // 
            // gridViewTimeClockEntries
            // 
            this.gridViewTimeClockEntries.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tlpContent.SetColumnSpan(this.gridViewTimeClockEntries, 5);
            this.gridViewTimeClockEntries.DataBindings.Add(new System.Windows.Forms.Binding("DataSource", this.bindingSource, "TimeClockEntries", true));
            this.gridViewTimeClockEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewTimeClockEntries.Location = new System.Drawing.Point(29, 234);
            this.gridViewTimeClockEntries.MainView = this.gridView;
            this.gridViewTimeClockEntries.Name = "gridViewTimeClockEntries";
            this.gridViewTimeClockEntries.Size = new System.Drawing.Size(966, 424);
            this.gridViewTimeClockEntries.TabIndex = 10;
            this.gridViewTimeClockEntries.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.ViewTimeClockEntriesViewModel);
            // 
            // gridView
            // 
            this.gridView.Appearance.FooterPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.FooterPanel.Options.UseFont = true;
            this.gridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.gridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.ColumnPanelRowHeight = 40;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRegistrationType,
            this.colWorker,
            this.colPersonal,
            this.colProfile,
            this.colClockInTime,
            this.colClockOutTime,
            this.colActivity,
            this.colStore});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.GridControl = this.gridViewTimeClockEntries;
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
            // colRegistrationType
            // 
            this.colRegistrationType.Name = "colRegistrationType";
            // 
            // colWorker
            // 
            this.colWorker.AppearanceCell.Options.UseTextOptions = true;
            this.colWorker.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colWorker.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWorker.Caption = "Worker";
            this.colWorker.FieldName = "WORKER";
            this.colWorker.MinWidth = 125;
            this.colWorker.Name = "colWorker";
            this.colWorker.Visible = true;
            this.colWorker.VisibleIndex = 0;
            this.colWorker.Width = 125;
            // 
            // colPersonal
            // 
            this.colPersonal.AppearanceCell.Options.UseTextOptions = true;
            this.colPersonal.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colPersonal.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colPersonal.Caption = "Personnel";
            this.colPersonal.FieldName = "PERSONNEL";
            this.colPersonal.MinWidth = 125;
            this.colPersonal.Name = "colPersonal";
            this.colPersonal.Visible = true;
            this.colPersonal.VisibleIndex = 1;
            this.colPersonal.Width = 125;
            // 
            // colProfile
            // 
            this.colProfile.AppearanceCell.Options.UseTextOptions = true;
            this.colProfile.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colProfile.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProfile.Caption = "Profile";
            this.colProfile.FieldName = "PROFILE";
            this.colProfile.MinWidth = 125;
            this.colProfile.Name = "colProfile";
            this.colProfile.Visible = true;
            this.colProfile.VisibleIndex = 2;
            this.colProfile.Width = 125;
            // 
            // colClockInTime
            // 
            this.colClockInTime.AppearanceCell.Options.UseTextOptions = true;
            this.colClockInTime.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colClockInTime.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colClockInTime.Caption = "Clock-in time";
            this.colClockInTime.FieldName = "CLOCKINTIME";
            this.colClockInTime.MinWidth = 175;
            this.colClockInTime.Name = "colClockInTime";
            this.colClockInTime.Visible = true;
            this.colClockInTime.VisibleIndex = 3;
            this.colClockInTime.Width = 175;
            // 
            // colClockOutTime
            // 
            this.colClockOutTime.AppearanceCell.Options.UseTextOptions = true;
            this.colClockOutTime.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colClockOutTime.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colClockOutTime.Caption = "Clock-out time";
            this.colClockOutTime.FieldName = "CLOCKOUTTIME";
            this.colClockOutTime.MinWidth = 175;
            this.colClockOutTime.Name = "colClockOutTime";
            this.colClockOutTime.Visible = true;
            this.colClockOutTime.VisibleIndex = 4;
            this.colClockOutTime.Width = 175;
            // 
            // colActivity
            // 
            this.colActivity.AppearanceCell.Options.UseTextOptions = true;
            this.colActivity.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colActivity.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colActivity.Caption = "Activity";
            this.colActivity.FieldName = "ACTIVITY";
            this.colActivity.MinWidth = 125;
            this.colActivity.Name = "colActivity";
            this.colActivity.Visible = true;
            this.colActivity.VisibleIndex = 5;
            this.colActivity.Width = 125;
            // 
            // colStore
            // 
            this.colStore.AppearanceCell.Options.UseTextOptions = true;
            this.colStore.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colStore.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colStore.Caption = "Store";
            this.colStore.FieldName = "STORE";
            this.colStore.MinWidth = 175;
            this.colStore.Name = "colStore";
            this.colStore.Visible = true;
            this.colStore.VisibleIndex = 6;
            this.colStore.Width = 175;
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(30, 680);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(57, 57);
            this.btnPgUp.TabIndex = 4;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.OnButtonPgUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(95, 680);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 5;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.OnButtonUp_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.CausesValidation = false;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(447, 679);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 60);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(872, 680);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 7;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.OnButtonDown_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(937, 680);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(57, 57);
            this.btnPgDown.TabIndex = 8;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.OnButtonPgDown_Click);
            // 
            // ViewTimeClockEntriesForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1024, 753);
            this.Controls.Add(this.tlpContent);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ViewTimeClockEntriesForm";
            this.Text = "ViewTimeClockEntrieForm";
            this.Controls.SetChildIndex(this.tlpContent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.tlpOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTimeClockEntries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TableLayoutPanel tlpOptions;
        private DevExpress.XtraEditors.CheckButton btnClockIn;
        private DevExpress.XtraEditors.CheckButton btnClockOut;
        private DevExpress.XtraEditors.CheckButton btnBreakForLunch;
        private DevExpress.XtraEditors.CheckButton btnBreakFromWork;
        private DevExpress.XtraEditors.CheckButton btnSelectStore;
        private DevExpress.XtraEditors.CheckButton btnSelectAllStore;
        private DevExpress.XtraGrid.GridControl gridViewTimeClockEntries;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colRegistrationType;
        private DevExpress.XtraGrid.Columns.GridColumn colStore;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private System.Windows.Forms.BindingSource bindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colWorker;
        private DevExpress.XtraGrid.Columns.GridColumn colPersonal;
        private DevExpress.XtraGrid.Columns.GridColumn colProfile;
        private DevExpress.XtraGrid.Columns.GridColumn colClockInTime;
        private DevExpress.XtraGrid.Columns.GridColumn colClockOutTime;
        private DevExpress.XtraGrid.Columns.GridColumn colActivity;
    }
}

