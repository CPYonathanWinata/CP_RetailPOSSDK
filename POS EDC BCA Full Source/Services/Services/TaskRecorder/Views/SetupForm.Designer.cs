/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views
{
    partial class SetupForm
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
            this.MainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DialogButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SaveNodeButton = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.CancelNodeButton = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.RecordingParametersHeader = new DevExpress.XtraEditors.LabelControl();
            this.DocumentDirectoryHeader = new DevExpress.XtraEditors.LabelControl();
            this.RecordingFilePathLabel = new DevExpress.XtraEditors.LabelControl();
            this.TemplateFilePathLabel = new DevExpress.XtraEditors.LabelControl();
            this.RecorderFilePathTextBox = new DevExpress.XtraEditors.TextEdit();
            this.TemplateFilePathTextBox = new DevExpress.XtraEditors.TextEdit();
            this.RecordingFolderBrowseButton = new DevExpress.XtraEditors.SimpleButton();
            this.TemplateFileBrowseButton = new DevExpress.XtraEditors.SimpleButton();
            this.RecorderFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.TemplateFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainPanel.SuspendLayout();
            this.DialogButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecorderFilePathTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplateFilePathTextBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.AutoSize = true;
            this.MainPanel.BackColor = System.Drawing.Color.Transparent;
            this.MainPanel.ColumnCount = 3;
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainPanel.Controls.Add(this.DialogButtonsPanel, 0, 4);
            this.MainPanel.Controls.Add(this.RecordingParametersHeader, 0, 0);
            this.MainPanel.Controls.Add(this.DocumentDirectoryHeader, 0, 1);
            this.MainPanel.Controls.Add(this.RecordingFilePathLabel, 0, 2);
            this.MainPanel.Controls.Add(this.TemplateFilePathLabel, 0, 3);
            this.MainPanel.Controls.Add(this.RecorderFilePathTextBox, 1, 2);
            this.MainPanel.Controls.Add(this.TemplateFilePathTextBox, 1, 3);
            this.MainPanel.Controls.Add(this.RecordingFolderBrowseButton, 2, 2);
            this.MainPanel.Controls.Add(this.TemplateFileBrowseButton, 2, 3);
            this.MainPanel.Location = new System.Drawing.Point(18, 19);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.RowCount = 5;
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.Size = new System.Drawing.Size(648, 271);
            this.MainPanel.TabIndex = 0;
            // 
            // DialogButtonsPanel
            // 
            this.DialogButtonsPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DialogButtonsPanel.AutoSize = true;
            this.MainPanel.SetColumnSpan(this.DialogButtonsPanel, 3);
            this.DialogButtonsPanel.Controls.Add(this.SaveNodeButton);
            this.DialogButtonsPanel.Controls.Add(this.CancelNodeButton);
            this.DialogButtonsPanel.Location = new System.Drawing.Point(183, 198);
            this.DialogButtonsPanel.Name = "DialogButtonsPanel";
            this.DialogButtonsPanel.Size = new System.Drawing.Size(282, 56);
            this.DialogButtonsPanel.TabIndex = 9;
            // 
            // SaveNodeButton
            // 
            this.SaveNodeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SaveNodeButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveNodeButton.Appearance.Options.UseFont = true;
            this.SaveNodeButton.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "CanSetParameters", true));
            this.SaveNodeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveNodeButton.Location = new System.Drawing.Point(3, 3);
            this.SaveNodeButton.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.SaveNodeButton.Name = "SaveNodeButton";
            this.SaveNodeButton.Size = new System.Drawing.Size(130, 50);
            this.SaveNodeButton.TabIndex = 0;
            this.SaveNodeButton.Text = "OK";
            this.SaveNodeButton.Click += new System.EventHandler(this.SaveNodeButton_Click);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels.RecorderViewModel);
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
            this.CancelNodeButton.Click += new System.EventHandler(this.CancelNodeButton_Click);
            // 
            // RecordingParametersHeader
            // 
            this.RecordingParametersHeader.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.RecordingParametersHeader.Appearance.ForeColor = System.Drawing.Color.DarkBlue;
            this.MainPanel.SetColumnSpan(this.RecordingParametersHeader, 3);
            this.RecordingParametersHeader.Location = new System.Drawing.Point(4, 5);
            this.RecordingParametersHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 21);
            this.RecordingParametersHeader.Name = "RecordingParametersHeader";
            this.RecordingParametersHeader.Size = new System.Drawing.Size(177, 23);
            this.RecordingParametersHeader.TabIndex = 0;
            this.RecordingParametersHeader.Text = "Recorder parameters";
            // 
            // DocumentDirectoryHeader
            // 
            this.DocumentDirectoryHeader.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.DocumentDirectoryHeader.Location = new System.Drawing.Point(4, 54);
            this.DocumentDirectoryHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 13);
            this.DocumentDirectoryHeader.Name = "DocumentDirectoryHeader";
            this.DocumentDirectoryHeader.Size = new System.Drawing.Size(138, 21);
            this.DocumentDirectoryHeader.TabIndex = 1;
            this.DocumentDirectoryHeader.Text = "Document directory";
            // 
            // RecordingFilePathLabel
            // 
            this.RecordingFilePathLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.RecordingFilePathLabel.Location = new System.Drawing.Point(4, 93);
            this.RecordingFilePathLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RecordingFilePathLabel.Name = "RecordingFilePathLabel";
            this.RecordingFilePathLabel.Size = new System.Drawing.Size(131, 21);
            this.RecordingFilePathLabel.TabIndex = 2;
            this.RecordingFilePathLabel.Text = "Recording file path";
            // 
            // TemplateFilePathLabel
            // 
            this.TemplateFilePathLabel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TemplateFilePathLabel.Location = new System.Drawing.Point(4, 140);
            this.TemplateFilePathLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TemplateFilePathLabel.Name = "TemplateFilePathLabel";
            this.TemplateFilePathLabel.Size = new System.Drawing.Size(124, 21);
            this.TemplateFilePathLabel.TabIndex = 3;
            this.TemplateFilePathLabel.Text = "Template file path";
            // 
            // RecorderFilePathTextBox
            // 
            this.RecorderFilePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RecorderFilePathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "PosRecorderFolderPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RecorderFilePathTextBox.Location = new System.Drawing.Point(150, 93);
            this.RecorderFilePathTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 20, 5);
            this.RecorderFilePathTextBox.Name = "RecorderFilePathTextBox";
            this.RecorderFilePathTextBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.RecorderFilePathTextBox.Properties.Appearance.Options.UseFont = true;
            this.RecorderFilePathTextBox.Size = new System.Drawing.Size(358, 28);
            this.RecorderFilePathTextBox.TabIndex = 4;
            // 
            // TemplateFilePathTextBox
            // 
            this.TemplateFilePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TemplateFilePathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "PosTemplateFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TemplateFilePathTextBox.Location = new System.Drawing.Point(150, 140);
            this.TemplateFilePathTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 20, 5);
            this.TemplateFilePathTextBox.Name = "TemplateFilePathTextBox";
            this.TemplateFilePathTextBox.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TemplateFilePathTextBox.Properties.Appearance.Options.UseFont = true;
            this.TemplateFilePathTextBox.Size = new System.Drawing.Size(358, 28);
            this.TemplateFilePathTextBox.TabIndex = 5;
            // 
            // RecordingFolderBrowseButton
            // 
            this.RecordingFolderBrowseButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.RecordingFolderBrowseButton.Appearance.Options.UseFont = true;
            this.RecordingFolderBrowseButton.Location = new System.Drawing.Point(532, 93);
            this.RecordingFolderBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RecordingFolderBrowseButton.Name = "RecordingFolderBrowseButton";
            this.RecordingFolderBrowseButton.Size = new System.Drawing.Size(112, 37);
            this.RecordingFolderBrowseButton.TabIndex = 6;
            this.RecordingFolderBrowseButton.Text = "Browse";
            this.RecordingFolderBrowseButton.Click += new System.EventHandler(this.RecordingFolderBrowseButton_Click);
            // 
            // TemplateFileBrowseButton
            // 
            this.TemplateFileBrowseButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TemplateFileBrowseButton.Appearance.Options.UseFont = true;
            this.TemplateFileBrowseButton.Location = new System.Drawing.Point(532, 140);
            this.TemplateFileBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TemplateFileBrowseButton.Name = "TemplateFileBrowseButton";
            this.TemplateFileBrowseButton.Size = new System.Drawing.Size(112, 37);
            this.TemplateFileBrowseButton.TabIndex = 7;
            this.TemplateFileBrowseButton.Text = "Browse";
            this.TemplateFileBrowseButton.Click += new System.EventHandler(this.TemplateFileBrowseButton_Click);
            // 
            // SetupForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(684, 310);
            this.Controls.Add(this.MainPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(1312, 719);
            this.Name = "SetupForm";
            this.Text = "Setup - POS Task Recorder ";
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.DialogButtonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecorderFilePathTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemplateFilePathTextBox.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainPanel;
        private DevExpress.XtraEditors.LabelControl RecordingParametersHeader;
        private DevExpress.XtraEditors.LabelControl DocumentDirectoryHeader;
        private DevExpress.XtraEditors.LabelControl RecordingFilePathLabel;
        private DevExpress.XtraEditors.LabelControl TemplateFilePathLabel;
        private DevExpress.XtraEditors.TextEdit RecorderFilePathTextBox;
        private DevExpress.XtraEditors.TextEdit TemplateFilePathTextBox;
        private System.Windows.Forms.FolderBrowserDialog RecorderFolderDialog;
        private System.Windows.Forms.OpenFileDialog TemplateFileDialog;
        private DevExpress.XtraEditors.SimpleButton RecordingFolderBrowseButton;
        private DevExpress.XtraEditors.SimpleButton TemplateFileBrowseButton;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.FlowLayoutPanel DialogButtonsPanel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx SaveNodeButton;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx CancelNodeButton;
    }
}