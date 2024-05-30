/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class LogbookForm
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
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.gridLogbook = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colActivityType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tlpOptions = new System.Windows.Forms.TableLayoutPanel();
            this.btnLastDay = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnLastWeek = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnLastMonth = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSelectStore = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelTitle = new System.Windows.Forms.Label();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogbook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tlpOptions.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(911, 656);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(57, 57);
            this.btnPgDown.TabIndex = 8;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(846, 656);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 7;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.CausesValidation = false;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(422, 656);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(4, 656);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(57, 57);
            this.btnPgUp.TabIndex = 4;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(69, 656);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 5;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // gridLogbook
            // 
            this.gridLogbook.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tlpContent.SetColumnSpan(this.gridLogbook, 5);
            this.gridLogbook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLogbook.Location = new System.Drawing.Point(3, 164);
            this.gridLogbook.MainView = this.gridView;
            this.gridLogbook.Name = "gridLogbook";
            this.gridLogbook.Size = new System.Drawing.Size(966, 485);
            this.gridLogbook.TabIndex = 10;
            this.gridLogbook.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView,
            this.gridView1});
            // 
            // gridView
            // 
            this.gridView.Appearance.FooterPanel.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.FooterPanel.Options.UseFont = true;
            this.gridView.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.gridView.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.GridControl = this.gridLogbook;
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
            this.gridView.RowHeight = 50;
            this.gridView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridLogbook;
            this.gridView1.Name = "gridView1";
            // 
            // colActivityType
            // 
            this.colActivityType.AppearanceCell.Options.UseTextOptions = true;
            this.colActivityType.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colActivityType.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colActivityType.Caption = "Activity type";
            this.colActivityType.FieldName = "ACTIVITYTYPE";
            this.colActivityType.MinWidth = 125;
            this.colActivityType.Name = "colActivityType";
            this.colActivityType.Visible = true;
            this.colActivityType.VisibleIndex = 0;
            this.colActivityType.Width = 125;
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
            this.colActivity.VisibleIndex = 1;
            this.colActivity.Width = 125;
            // 
            // colDateTime
            // 
            this.colDateTime.AppearanceCell.Options.UseTextOptions = true;
            this.colDateTime.Caption = "DateTime";
            this.colDateTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colDateTime.FieldName = "DATETIME";
            this.colDateTime.MinWidth = 125;
            this.colDateTime.Name = "colDateTime";
            this.colDateTime.Visible = true;
            this.colDateTime.VisibleIndex = 2;
            this.colDateTime.Width = 175;
            // 
            // colStore
            // 
            this.colStore.AppearanceCell.Options.UseTextOptions = true;
            this.colStore.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            this.colStore.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colStore.Caption = "Store";
            this.colStore.FieldName = "STORE";
            this.colStore.MinWidth = 125;
            this.colStore.Name = "colStore";
            this.colStore.Visible = true;
            this.colStore.VisibleIndex = 3;
            this.colStore.Width = 125;
            // 
            // tlpOptions
            // 
            this.tlpOptions.ColumnCount = 4;
            this.tlpContent.SetColumnSpan(this.tlpOptions, 5);
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpOptions.Controls.Add(this.btnLastDay, 0, 0);
            this.tlpOptions.Controls.Add(this.btnLastWeek, 1, 0);
            this.tlpOptions.Controls.Add(this.btnLastMonth, 2, 0);
            this.tlpOptions.Controls.Add(this.btnSelectStore, 3, 0);
            this.tlpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOptions.Location = new System.Drawing.Point(0, 95);
            this.tlpOptions.Margin = new System.Windows.Forms.Padding(0);
            this.tlpOptions.Name = "tlpOptions";
            this.tlpOptions.RowCount = 1;
            this.tlpOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpOptions.Size = new System.Drawing.Size(972, 66);
            this.tlpOptions.TabIndex = 11;
            // 
            // btnLastDay
            // 
            this.btnLastDay.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastDay.Appearance.Options.UseFont = true;
            this.btnLastDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLastDay.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnLastDay.Location = new System.Drawing.Point(4, 4);
            this.btnLastDay.Margin = new System.Windows.Forms.Padding(4);
            this.btnLastDay.Name = "btnLastDay";
            this.btnLastDay.Size = new System.Drawing.Size(235, 58);
            this.btnLastDay.TabIndex = 0;
            this.btnLastDay.Text = "Last 24 hours";
            this.btnLastDay.Click += new System.EventHandler(this.btnLastDay_Click);
            // 
            // btnLastWeek
            // 
            this.btnLastWeek.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastWeek.Appearance.Options.UseFont = true;
            this.btnLastWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLastWeek.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnLastWeek.Location = new System.Drawing.Point(247, 4);
            this.btnLastWeek.Margin = new System.Windows.Forms.Padding(4);
            this.btnLastWeek.Name = "btnLastWeek";
            this.btnLastWeek.Size = new System.Drawing.Size(235, 58);
            this.btnLastWeek.TabIndex = 1;
            this.btnLastWeek.Text = "Last 7 days";
            this.btnLastWeek.Click += new System.EventHandler(this.btnLastWeek_Click);
            // 
            // btnLastMonth
            // 
            this.btnLastMonth.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastMonth.Appearance.Options.UseFont = true;
            this.btnLastMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLastMonth.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnLastMonth.Location = new System.Drawing.Point(490, 4);
            this.btnLastMonth.Margin = new System.Windows.Forms.Padding(4);
            this.btnLastMonth.Name = "btnLastMonth";
            this.btnLastMonth.Size = new System.Drawing.Size(235, 58);
            this.btnLastMonth.TabIndex = 2;
            this.btnLastMonth.Text = "Last 30 days";
            this.btnLastMonth.Click += new System.EventHandler(this.btnLastMonth_Click);
            // 
            // btnSelectStore
            // 
            this.btnSelectStore.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectStore.Appearance.Options.UseFont = true;
            this.btnSelectStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectStore.Location = new System.Drawing.Point(733, 4);
            this.btnSelectStore.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectStore.Name = "btnSelectStore";
            this.btnSelectStore.Size = new System.Drawing.Size(235, 58);
            this.btnSelectStore.TabIndex = 3;
            this.btnSelectStore.Text = "Select store";
            this.btnSelectStore.Click += new System.EventHandler(this.btnSelectStore_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.tlpContent.SetColumnSpan(this.labelTitle, 5);
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(383, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(206, 65);
            this.labelTitle.TabIndex = 12;
            this.labelTitle.Text = "Logbook";
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
            this.tlpContent.Controls.Add(this.gridLogbook, 0, 2);
            this.tlpContent.Controls.Add(this.btnPgUp, 0, 3);
            this.tlpContent.Controls.Add(this.btnUp, 1, 3);
            this.tlpContent.Controls.Add(this.btnClose, 2, 3);
            this.tlpContent.Controls.Add(this.btnDown, 3, 3);
            this.tlpContent.Controls.Add(this.btnPgDown, 4, 3);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(26, 40);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 4;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.Size = new System.Drawing.Size(972, 717);
            this.tlpContent.TabIndex = 9;
            // 
            // LogbookForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tlpContent);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LogbookForm";
            this.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.Text = "LogbookForm";
            this.Controls.SetChildIndex(this.tlpContent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogbook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tlpOptions.ResumeLayout(false);
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bindingSource;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;        
        private DevExpress.XtraGrid.GridControl gridLogbook;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TableLayoutPanel tlpOptions;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnLastDay;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnLastWeek;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnLastMonth;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSelectStore;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colActivityType;
        private DevExpress.XtraGrid.Columns.GridColumn colActivity;
        private DevExpress.XtraGrid.Columns.GridColumn colDateTime;
        private DevExpress.XtraGrid.Columns.GridColumn colStore;
    }
}