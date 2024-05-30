/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis.POSProcesses.WinControls;
namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views
{
    partial class NewNodeForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblNewNodeHeader = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.OperationGroupComboBox = new DevExpress.XtraEditors.LookUpEdit();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.ModuleComboBox = new DevExpress.XtraEditors.LookUpEdit();
            this.NodeNameLabel = new DevExpress.XtraEditors.LabelControl();
            this.NodeDescriptionLabel = new DevExpress.XtraEditors.LabelControl();
            this.ModuleLabel = new DevExpress.XtraEditors.LabelControl();
            this.OperationGroupLabel = new DevExpress.XtraEditors.LabelControl();
            this.NodeNameTextBox = new DevExpress.XtraEditors.TextEdit();
            this.NodeDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.DialogButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SaveNodeButton = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.CancelNodeButton = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGroupComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModuleComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NodeNameTextBox.Properties)).BeginInit();
            this.DialogButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblNewNodeHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.DialogButtonsPanel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 323);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblNewNodeHeader
            // 
            this.lblNewNodeHeader.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewNodeHeader.Appearance.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblNewNodeHeader.Location = new System.Drawing.Point(3, 3);
            this.lblNewNodeHeader.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.lblNewNodeHeader.Name = "lblNewNodeHeader";
            this.lblNewNodeHeader.Size = new System.Drawing.Size(86, 25);
            this.lblNewNodeHeader.TabIndex = 0;
            this.lblNewNodeHeader.Text = "New node";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.OperationGroupComboBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.ModuleComboBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.NodeNameLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.NodeDescriptionLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.ModuleLabel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.OperationGroupLabel, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.NodeNameTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.NodeDescriptionTextBox, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 44);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(478, 161);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // OperationGroupComboBox
            // 
            this.OperationGroupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.OperationGroupComboBox.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSource, "SelectedOperationGroup", true));
            this.OperationGroupComboBox.Location = new System.Drawing.Point(128, 130);
            this.OperationGroupComboBox.Name = "OperationGroupComboBox";
            this.OperationGroupComboBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperationGroupComboBox.Properties.Appearance.Options.UseFont = true;
            this.OperationGroupComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OperationGroupComboBox.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DisplayName", "DisplayName")});
            this.OperationGroupComboBox.Properties.DisplayMember = "DisplayName";
            this.OperationGroupComboBox.Properties.NullText = "";
            this.OperationGroupComboBox.Properties.ShowHeader = false;
            this.OperationGroupComboBox.Properties.ValueMember = "EnumValue";
            this.OperationGroupComboBox.Size = new System.Drawing.Size(347, 28);
            this.OperationGroupComboBox.TabIndex = 7;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels.NewNodeViewModel);
            // 
            // ModuleComboBox
            // 
            this.ModuleComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ModuleComboBox.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSource, "SelectedModule", true));
            this.ModuleComboBox.Location = new System.Drawing.Point(128, 96);
            this.ModuleComboBox.Name = "ModuleComboBox";
            this.ModuleComboBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModuleComboBox.Properties.Appearance.Options.UseFont = true;
            this.ModuleComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ModuleComboBox.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DisplayName", "DisplayName")});
            this.ModuleComboBox.Properties.DisplayMember = "DisplayName";
            this.ModuleComboBox.Properties.NullText = "";
            this.ModuleComboBox.Properties.ShowHeader = false;
            this.ModuleComboBox.Properties.ValueMember = "EnumValue";
            this.ModuleComboBox.Size = new System.Drawing.Size(347, 28);
            this.ModuleComboBox.TabIndex = 5;
            // 
            // NodeNameLabel
            // 
            this.NodeNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NodeNameLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NodeNameLabel.Location = new System.Drawing.Point(3, 6);
            this.NodeNameLabel.Name = "NodeNameLabel";
            this.NodeNameLabel.Size = new System.Drawing.Size(81, 21);
            this.NodeNameLabel.TabIndex = 0;
            this.NodeNameLabel.Text = "Node name";
            // 
            // NodeDescriptionLabel
            // 
            this.NodeDescriptionLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NodeDescriptionLabel.Location = new System.Drawing.Point(3, 37);
            this.NodeDescriptionLabel.Name = "NodeDescriptionLabel";
            this.NodeDescriptionLabel.Size = new System.Drawing.Size(119, 21);
            this.NodeDescriptionLabel.TabIndex = 2;
            this.NodeDescriptionLabel.Text = "Node description";
            // 
            // ModuleLabel
            // 
            this.ModuleLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ModuleLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModuleLabel.Location = new System.Drawing.Point(3, 99);
            this.ModuleLabel.Name = "ModuleLabel";
            this.ModuleLabel.Size = new System.Drawing.Size(53, 21);
            this.ModuleLabel.TabIndex = 4;
            this.ModuleLabel.Text = "Module";
            // 
            // OperationGroupLabel
            // 
            this.OperationGroupLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.OperationGroupLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperationGroupLabel.Location = new System.Drawing.Point(3, 133);
            this.OperationGroupLabel.Name = "OperationGroupLabel";
            this.OperationGroupLabel.Size = new System.Drawing.Size(116, 21);
            this.OperationGroupLabel.TabIndex = 6;
            this.OperationGroupLabel.Text = "Operation group";
            // 
            // NodeNameTextBox
            // 
            this.NodeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NodeNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NodeNameTextBox.Location = new System.Drawing.Point(128, 3);
            this.NodeNameTextBox.Name = "NodeNameTextBox";
            this.NodeNameTextBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NodeNameTextBox.Properties.Appearance.Options.UseFont = true;
            this.NodeNameTextBox.Size = new System.Drawing.Size(347, 28);
            this.NodeNameTextBox.TabIndex = 1;
            // 
            // NodeDescriptionTextBox
            // 
            this.NodeDescriptionTextBox.AcceptsReturn = true;
            this.NodeDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NodeDescriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Description", true));
            this.NodeDescriptionTextBox.Location = new System.Drawing.Point(128, 37);
            this.NodeDescriptionTextBox.Multiline = true;
            this.NodeDescriptionTextBox.Name = "NodeDescriptionTextBox";
            this.NodeDescriptionTextBox.Size = new System.Drawing.Size(347, 53);
            this.NodeDescriptionTextBox.TabIndex = 3;
            // 
            // DialogButtonsPanel
            // 
            this.DialogButtonsPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DialogButtonsPanel.AutoSize = true;
            this.DialogButtonsPanel.Controls.Add(this.SaveNodeButton);
            this.DialogButtonsPanel.Controls.Add(this.CancelNodeButton);
            this.DialogButtonsPanel.Location = new System.Drawing.Point(101, 264);
            this.DialogButtonsPanel.Name = "DialogButtonsPanel";
            this.DialogButtonsPanel.Size = new System.Drawing.Size(282, 56);
            this.DialogButtonsPanel.TabIndex = 2;
            // 
            // SaveNodeButton
            // 
            this.SaveNodeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SaveNodeButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveNodeButton.Appearance.Options.UseFont = true;
            this.SaveNodeButton.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanSaveNode", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.SaveNodeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveNodeButton.Location = new System.Drawing.Point(3, 3);
            this.SaveNodeButton.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.SaveNodeButton.Name = "SaveNodeButton";
            this.SaveNodeButton.Size = new System.Drawing.Size(130, 50);
            this.SaveNodeButton.TabIndex = 0;
            this.SaveNodeButton.Text = "Save";
            this.SaveNodeButton.Click += new System.EventHandler(this.SaveNodeButton_Click);
            // 
            // CancelNodeButton
            // 
            this.CancelNodeButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelNodeButton.Appearance.Options.UseFont = true;
            this.CancelNodeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelNodeButton.Location = new System.Drawing.Point(149, 3);
            this.CancelNodeButton.Name = "CancelNodeButton";
            this.CancelNodeButton.Size = new System.Drawing.Size(130, 50);
            this.CancelNodeButton.TabIndex = 1;
            this.CancelNodeButton.Text = "Cancel";
            // 
            // NewNodeForm
            // 
            this.AcceptButton = this.SaveNodeButton;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.CancelNodeButton;
            this.ClientSize = new System.Drawing.Size(508, 347);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimizeBox = false;
            this.Name = "NewNodeForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "New node - Task Recorder";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGroupComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModuleComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NodeNameTextBox.Properties)).EndInit();
            this.DialogButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl lblNewNodeHeader;
        private System.Windows.Forms.FlowLayoutPanel DialogButtonsPanel;
        private SimpleButtonEx SaveNodeButton;
        private SimpleButtonEx CancelNodeButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.LabelControl NodeNameLabel;
        private DevExpress.XtraEditors.LabelControl NodeDescriptionLabel;
        private DevExpress.XtraEditors.LabelControl ModuleLabel;
        private DevExpress.XtraEditors.LabelControl OperationGroupLabel;
        private DevExpress.XtraEditors.TextEdit NodeNameTextBox;
        private DevExpress.XtraEditors.LookUpEdit ModuleComboBox;
        private DevExpress.XtraEditors.LookUpEdit OperationGroupComboBox;
        private System.Windows.Forms.TextBox NodeDescriptionTextBox;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}