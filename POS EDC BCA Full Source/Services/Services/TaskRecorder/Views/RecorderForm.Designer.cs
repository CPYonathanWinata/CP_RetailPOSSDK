/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views
{
    partial class RecorderForm
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (ViewModel != null)
                {
                    ViewModel.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.ContentPanel = new DevExpress.XtraEditors.PanelControl();
            this.ModuleUsageProfilePanel = new System.Windows.Forms.TableLayoutPanel();
            this.UsageProfileComboBox = new DevExpress.XtraEditors.LookUpEdit();
            this.ModuleComboBox = new DevExpress.XtraEditors.LookUpEdit();
            this.ModuleLabel = new DevExpress.XtraEditors.LabelControl();
            this.UsageProfileLabel = new DevExpress.XtraEditors.LabelControl();
            this.TreeViewContainer = new DevExpress.XtraEditors.PanelControl();
            this.FrameworkIndustryPanel = new System.Windows.Forms.TableLayoutPanel();
            this.FrameworkLabel = new DevExpress.XtraEditors.LabelControl();
            this.IndustryLabel = new DevExpress.XtraEditors.LabelControl();
            this.IndustryComboBox = new DevExpress.XtraEditors.LookUpEdit();
            this.FrameworkComboBox = new DevExpress.XtraEditors.LookUpEdit();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnStart = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnStop = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnNew = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDelete = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClear = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSetup = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnHelp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.stopRecordingWaitInfoTimer = new System.Windows.Forms.Timer(this.components);
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ContentPanel)).BeginInit();
            this.ContentPanel.SuspendLayout();
            this.ModuleUsageProfilePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UsageProfileComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModuleComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TreeViewContainer)).BeginInit();
            this.FrameworkIndustryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IndustryComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameworkComboBox.Properties)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoSize = true;
            this.ContentPanel.Controls.Add(this.ModuleUsageProfilePanel);
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ContentPanel.Location = new System.Drawing.Point(0, 637);
            this.ContentPanel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(527, 79);
            this.ContentPanel.TabIndex = 3;
            // 
            // ModuleUsageProfilePanel
            // 
            this.ModuleUsageProfilePanel.AutoSize = true;
            this.ModuleUsageProfilePanel.ColumnCount = 3;
            this.ModuleUsageProfilePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ModuleUsageProfilePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ModuleUsageProfilePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ModuleUsageProfilePanel.Controls.Add(this.UsageProfileComboBox, 2, 1);
            this.ModuleUsageProfilePanel.Controls.Add(this.ModuleComboBox, 0, 1);
            this.ModuleUsageProfilePanel.Controls.Add(this.ModuleLabel, 0, 0);
            this.ModuleUsageProfilePanel.Controls.Add(this.UsageProfileLabel, 2, 0);
            this.ModuleUsageProfilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModuleUsageProfilePanel.Location = new System.Drawing.Point(2, 2);
            this.ModuleUsageProfilePanel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.ModuleUsageProfilePanel.Name = "ModuleUsageProfilePanel";
            this.ModuleUsageProfilePanel.RowCount = 2;
            this.ModuleUsageProfilePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ModuleUsageProfilePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ModuleUsageProfilePanel.Size = new System.Drawing.Size(523, 75);
            this.ModuleUsageProfilePanel.TabIndex = 0;
            // 
            // UsageProfileComboBox
            // 
            this.UsageProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.UsageProfileComboBox.Location = new System.Drawing.Point(316, 37);
            this.UsageProfileComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.UsageProfileComboBox.Name = "UsageProfileComboBox";
            this.UsageProfileComboBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.UsageProfileComboBox.Properties.Appearance.Options.UseFont = true;
            this.UsageProfileComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.UsageProfileComboBox.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DisplayName", "DisplayName")});
            this.UsageProfileComboBox.Properties.DisplayMember = "DisplayName";
            this.UsageProfileComboBox.Properties.NullText = "";
            this.UsageProfileComboBox.Properties.ReadOnly = true;
            this.UsageProfileComboBox.Properties.ShowHeader = false;
            this.UsageProfileComboBox.Properties.ValueMember = "EnumValue";
            this.UsageProfileComboBox.Size = new System.Drawing.Size(204, 28);
            this.UsageProfileComboBox.TabIndex = 3;
            // 
            // ModuleComboBox
            // 
            this.ModuleComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ModuleComboBox.Location = new System.Drawing.Point(3, 37);
            this.ModuleComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ModuleComboBox.Name = "ModuleComboBox";
            this.ModuleComboBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ModuleComboBox.Properties.Appearance.Options.UseFont = true;
            this.ModuleComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ModuleComboBox.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DisplayName", "DisplayName")});
            this.ModuleComboBox.Properties.DisplayMember = "DisplayName";
            this.ModuleComboBox.Properties.NullText = "";
            this.ModuleComboBox.Properties.ReadOnly = true;
            this.ModuleComboBox.Properties.ShowHeader = false;
            this.ModuleComboBox.Properties.ValueMember = "EnumValue";
            this.ModuleComboBox.Size = new System.Drawing.Size(203, 28);
            this.ModuleComboBox.TabIndex = 1;
            // 
            // ModuleLabel
            // 
            this.ModuleLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ModuleLabel.Location = new System.Drawing.Point(3, 10);
            this.ModuleLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.ModuleLabel.Name = "ModuleLabel";
            this.ModuleLabel.Size = new System.Drawing.Size(53, 21);
            this.ModuleLabel.TabIndex = 0;
            this.ModuleLabel.Text = "Module";
            // 
            // UsageProfileLabel
            // 
            this.UsageProfileLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.UsageProfileLabel.Location = new System.Drawing.Point(316, 10);
            this.UsageProfileLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.UsageProfileLabel.Name = "UsageProfileLabel";
            this.UsageProfileLabel.Size = new System.Drawing.Size(92, 21);
            this.UsageProfileLabel.TabIndex = 2;
            this.UsageProfileLabel.Text = "Usage profile";
            // 
            // TreeViewContainer
            // 
            this.TreeViewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeViewContainer.Location = new System.Drawing.Point(0, 175);
            this.TreeViewContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TreeViewContainer.Name = "TreeViewContainer";
            this.TreeViewContainer.Size = new System.Drawing.Size(527, 462);
            this.TreeViewContainer.TabIndex = 2;
            // 
            // FrameworkIndustryPanel
            // 
            this.FrameworkIndustryPanel.AutoSize = true;
            this.FrameworkIndustryPanel.ColumnCount = 3;
            this.FrameworkIndustryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.FrameworkIndustryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FrameworkIndustryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.FrameworkIndustryPanel.Controls.Add(this.FrameworkLabel, 0, 0);
            this.FrameworkIndustryPanel.Controls.Add(this.IndustryLabel, 2, 0);
            this.FrameworkIndustryPanel.Controls.Add(this.IndustryComboBox, 2, 1);
            this.FrameworkIndustryPanel.Controls.Add(this.FrameworkComboBox, 0, 1);
            this.FrameworkIndustryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FrameworkIndustryPanel.Location = new System.Drawing.Point(0, 100);
            this.FrameworkIndustryPanel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.FrameworkIndustryPanel.Name = "FrameworkIndustryPanel";
            this.FrameworkIndustryPanel.RowCount = 2;
            this.FrameworkIndustryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.FrameworkIndustryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.FrameworkIndustryPanel.Size = new System.Drawing.Size(527, 75);
            this.FrameworkIndustryPanel.TabIndex = 1;
            // 
            // FrameworkLabel
            // 
            this.FrameworkLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FrameworkLabel.Location = new System.Drawing.Point(3, 10);
            this.FrameworkLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.FrameworkLabel.Name = "FrameworkLabel";
            this.FrameworkLabel.Size = new System.Drawing.Size(79, 21);
            this.FrameworkLabel.TabIndex = 0;
            this.FrameworkLabel.Text = "Framework";
            // 
            // IndustryLabel
            // 
            this.IndustryLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.IndustryLabel.Location = new System.Drawing.Point(318, 10);
            this.IndustryLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.IndustryLabel.Name = "IndustryLabel";
            this.IndustryLabel.Size = new System.Drawing.Size(57, 21);
            this.IndustryLabel.TabIndex = 2;
            this.IndustryLabel.Text = "Industry";
            // 
            // IndustryComboBox
            // 
            this.IndustryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.IndustryComboBox.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSource, "SelectedIndustry", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IndustryComboBox.Location = new System.Drawing.Point(318, 37);
            this.IndustryComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.IndustryComboBox.Name = "IndustryComboBox";
            this.IndustryComboBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.IndustryComboBox.Properties.Appearance.Options.UseFont = true;
            this.IndustryComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.IndustryComboBox.Properties.DisplayMember = "IndustryCode";
            this.IndustryComboBox.Properties.NullText = "";
            this.IndustryComboBox.Size = new System.Drawing.Size(206, 28);
            this.IndustryComboBox.TabIndex = 3;
            // 
            // FrameworkComboBox
            // 
            this.FrameworkComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameworkComboBox.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSource, "SelectedFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FrameworkComboBox.Location = new System.Drawing.Point(3, 37);
            this.FrameworkComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.FrameworkComboBox.Name = "FrameworkComboBox";
            this.FrameworkComboBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FrameworkComboBox.Properties.Appearance.Options.UseFont = true;
            this.FrameworkComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FrameworkComboBox.Properties.DisplayMember = "FrameworkId";
            this.FrameworkComboBox.Properties.NullText = "";
            this.FrameworkComboBox.Size = new System.Drawing.Size(204, 28);
            this.FrameworkComboBox.TabIndex = 1;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel.Controls.Add(this.btnStart, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btnStop, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.btnNew, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.btnDelete, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.btnClear, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.btnSetup, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.btnHelp, 6, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(527, 100);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Appearance.Options.UseFont = true;
            this.btnStart.Appearance.Options.UseTextOptions = true;
            this.btnStart.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnStart.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanStartRecording", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnStart.Location = new System.Drawing.Point(3, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(69, 94);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.StartRecordingButton_ItemClick);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Appearance.Options.UseFont = true;
            this.btnStop.Appearance.Options.UseTextOptions = true;
            this.btnStop.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnStop.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanStopRecording", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnStop.Location = new System.Drawing.Point(78, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(69, 94);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.StopRecordingButton_ItemClick);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Appearance.Options.UseFont = true;
            this.btnNew.Appearance.Options.UseTextOptions = true;
            this.btnNew.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnNew.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanCreateNewNode", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnNew.Location = new System.Drawing.Point(153, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(69, 94);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.NewNodeButton_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Appearance.Options.UseFont = true;
            this.btnDelete.Appearance.Options.UseTextOptions = true;
            this.btnDelete.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnDelete.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanDeleteNode", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnDelete.Location = new System.Drawing.Point(228, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 94);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.DeleteNodeButton_ItemClick);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.Appearance.Options.UseTextOptions = true;
            this.btnClear.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnClear.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanClearNode", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnClear.Location = new System.Drawing.Point(303, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(69, 94);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.ClearNodeButton_ItemClick);
            // 
            // btnSetup
            // 
            this.btnSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetup.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetup.Appearance.Options.UseFont = true;
            this.btnSetup.Appearance.Options.UseTextOptions = true;
            this.btnSetup.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnSetup.Location = new System.Drawing.Point(378, 3);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(69, 94);
            this.btnSetup.TabIndex = 5;
            this.btnSetup.Text = "Setup";
            this.btnSetup.Click += new System.EventHandler(this.SetupButton_ItemClick);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Appearance.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.Appearance.Options.UseFont = true;
            this.btnHelp.Appearance.Options.UseTextOptions = true;
            this.btnHelp.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.btnHelp.Location = new System.Drawing.Point(453, 3);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(71, 94);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.HelpButton_ItemClick);
            // 
            // stopRecordingWaitInfoTimer
            // 
            this.stopRecordingWaitInfoTimer.Interval = 1000;
            this.stopRecordingWaitInfoTimer.Tick += new System.EventHandler(this.StopRecordingWaitInfoTimer_Tick);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels.RecorderViewModel);
            // 
            // RecorderForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(527, 716);
            this.Controls.Add(this.TreeViewContainer);
            this.Controls.Add(this.FrameworkIndustryPanel);
            this.Controls.Add(this.ContentPanel);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.MaximizeBox = false;
            this.Name = "RecorderForm";
            this.TopMost = true;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.RecorderForm_HelpRequested);
            ((System.ComponentModel.ISupportInitialize)(this.ContentPanel)).EndInit();
            this.ContentPanel.ResumeLayout(false);
            this.ContentPanel.PerformLayout();
            this.ModuleUsageProfilePanel.ResumeLayout(false);
            this.ModuleUsageProfilePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UsageProfileComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModuleComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TreeViewContainer)).EndInit();
            this.FrameworkIndustryPanel.ResumeLayout(false);
            this.FrameworkIndustryPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IndustryComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameworkComboBox.Properties)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl ContentPanel;
        private System.Windows.Forms.TableLayoutPanel FrameworkIndustryPanel;
        private DevExpress.XtraEditors.LabelControl FrameworkLabel;
        private DevExpress.XtraEditors.LabelControl IndustryLabel;
        private System.Windows.Forms.TableLayoutPanel ModuleUsageProfilePanel;
        private DevExpress.XtraEditors.LabelControl ModuleLabel;
        private DevExpress.XtraEditors.LabelControl UsageProfileLabel;
        private DevExpress.XtraEditors.PanelControl TreeViewContainer;
        private DevExpress.XtraEditors.LookUpEdit UsageProfileComboBox;
        private DevExpress.XtraEditors.LookUpEdit ModuleComboBox;
        private DevExpress.XtraEditors.LookUpEdit IndustryComboBox;
        private DevExpress.XtraEditors.LookUpEdit FrameworkComboBox;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnStart;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnStop;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnNew;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDelete;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClear;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSetup;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnHelp;
        private System.Windows.Forms.Timer stopRecordingWaitInfoTimer;
    }
}