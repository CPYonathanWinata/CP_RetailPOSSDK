/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class frmDimensions
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.parentPanel = new System.Windows.Forms.Panel();            
            this.colorButton = new DevExpress.XtraEditors.CheckButton();
            this.configButton = new DevExpress.XtraEditors.CheckButton();            
            this.sizeButton = new DevExpress.XtraEditors.CheckButton();
            this.styleButton = new DevExpress.XtraEditors.CheckButton();            
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPageUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPageDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.dimensionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelbase = new System.Windows.Forms.Panel();            
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.parentPanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.panelbase.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.panelControl1, 5);
            this.panelControl1.Controls.Add(this.parentPanel);
            this.panelControl1.Location = new System.Drawing.Point(219, 138);
            this.panelControl1.Name = "panelControl1";
            this.tableLayoutPanel.SetRowSpan(this.panelControl1, 4);
            this.panelControl1.Size = new System.Drawing.Size(881, 490);
            this.panelControl1.TabIndex = 0;
            // 
            // parentPanel
            // 
            this.parentPanel.Controls.Add(this.dimensionsPanel);
            this.parentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentPanel.Location = new System.Drawing.Point(2, 2);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(877, 486);
            this.parentPanel.TabIndex = 10;
            this.parentPanel.Resize += new System.EventHandler(this.ParentPanel_Resize);
            // 
            // dimensionsPanel
            // 
            this.dimensionsPanel.AutoScroll = true;
            this.dimensionsPanel.Location = new System.Drawing.Point(0, 0);
            this.dimensionsPanel.Name = "dimensionsPanel";
            this.dimensionsPanel.Size = new System.Drawing.Size(115, 51);
            this.dimensionsPanel.TabIndex = 10;
            // 
            // configButton
            // 
            this.configButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.configButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configButton.Appearance.Options.UseFont = true;
            this.configButton.Location = new System.Drawing.Point(33, 579);
            this.configButton.Name = "configButton";
            this.configButton.Size = new System.Drawing.Size(130, 57);
            this.configButton.TabIndex = 9;
            this.configButton.Text = "Configuration";
            this.configButton.Visible = false;
            this.configButton.CheckedChanged += new System.EventHandler(this.OnConfigButton_CheckedChanged);
            this.configButton.Click += new System.EventHandler(this.OnConfigButton_Click);
            // 
            // styleButton
            // 
            this.styleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.styleButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.styleButton.Appearance.Options.UseFont = true;
            this.styleButton.Location = new System.Drawing.Point(33, 444);
            this.styleButton.Name = "styleButton";
            this.styleButton.Size = new System.Drawing.Size(130, 57);
            this.styleButton.TabIndex = 8;
            this.styleButton.Text = "Style";
            this.styleButton.Visible = false;
            this.styleButton.CheckedChanged += new System.EventHandler(this.OnStyleButton_CheckedChanged);
            this.styleButton.Click += new System.EventHandler(this.OnStyleButton_Click);
            // 
            // sizeButton
            // 
            this.sizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeButton.Appearance.Options.UseFont = true;
            this.sizeButton.Location = new System.Drawing.Point(33, 309);
            this.sizeButton.Name = "sizeButton";
            this.sizeButton.Size = new System.Drawing.Size(130, 57);
            this.sizeButton.TabIndex = 7;
            this.sizeButton.Text = "Size";
            this.sizeButton.Visible = false;
            this.sizeButton.CheckedChanged += new System.EventHandler(this.OnSizeButton_CheckedChanged);
            this.sizeButton.Click += new System.EventHandler(this.OnSizeButton_Click);
            // 
            // colorButton
            // 
            this.colorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.colorButton.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton.Appearance.Options.UseFont = true;
            this.colorButton.Location = new System.Drawing.Point(33, 174);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(130, 57);
            this.colorButton.TabIndex = 5;
            this.colorButton.Text = "Color";
            this.colorButton.Visible = false;
            this.colorButton.CheckedChanged += new System.EventHandler(this.OnColorButton_CheckedChanged);
            this.colorButton.Click += new System.EventHandler(this.OnColorButton_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 8;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel.Controls.Add(this.panelControl1, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.colorButton, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.configButton, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.sizeButton, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.styleButton, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btnCancel, 4, 5);
            this.tableLayoutPanel.Controls.Add(this.btnPageUp, 2, 5);
            this.tableLayoutPanel.Controls.Add(this.btnUp, 3, 5);
            this.tableLayoutPanel.Controls.Add(this.btnDown, 5, 5);
            this.tableLayoutPanel.Controls.Add(this.btnPageDown, 6, 5);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(30, 40, 30, 15);
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));            
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1018, 766);
            this.tableLayoutPanel.TabIndex = 10;
            this.tableLayoutPanel.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.OnTableLayoutPanel_CellPaint);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblHeader, 9);
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(33, 40);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(3, 0, 3, 30);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(952, 65);
            this.lblHeader.TabIndex = 10;
            this.lblHeader.Text = "Select product options";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPageUp
            // 
            this.btnPageUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageUp.Appearance.Options.UseFont = true;
            this.btnPageUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPageUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageUp.Location = new System.Drawing.Point(86, 646);
            this.btnPageUp.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnPageUp.Name = "btnPageUp";
            this.btnPageUp.Size = new System.Drawing.Size(65, 58);
            this.btnPageUp.TabIndex = 14;
            this.btnPageUp.Text = "Ç";
            this.btnPageUp.Click += new System.EventHandler(this.btnPageUp_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(159, 646);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 58);
            this.btnUp.TabIndex = 13;
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;            
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(462, 646);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";            
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(827, 646);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 58);
            this.btnDown.TabIndex = 12;
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnPageDown
            // 
            this.btnPageDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageDown.Appearance.Options.UseFont = true;
            this.btnPageDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnPageDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageDown.Location = new System.Drawing.Point(900, 646);
            this.btnPageDown.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnPageDown.Name = "btnPageDown";
            this.btnPageDown.Size = new System.Drawing.Size(65, 58);
            this.btnPageDown.TabIndex = 15;
            this.btnPageDown.Text = "Ê";
            this.btnPageDown.Click += new System.EventHandler(this.btnPageDown_Click);
            // 
            // panelbase
            // 
            this.panelbase.Controls.Add(this.tableLayoutPanel);
            this.panelbase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelbase.Location = new System.Drawing.Point(0, 0);
            this.panelbase.Name = "panelbase";
            this.panelbase.Size = new System.Drawing.Size(1018, 766);
            this.panelbase.TabIndex = 11;
            // 
            // frmDimensions
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1018, 766);
            this.Controls.Add(this.panelbase);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmDimensions";
            this.Controls.SetChildIndex(this.panelbase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.parentPanel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.panelbase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;        
        private DevExpress.XtraEditors.CheckButton colorButton;
        private DevExpress.XtraEditors.CheckButton sizeButton;
        private DevExpress.XtraEditors.CheckButton configButton;
        private DevExpress.XtraEditors.CheckButton styleButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label lblHeader;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
		private System.Windows.Forms.Panel panelbase;        
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageDown;
        private System.Windows.Forms.Panel parentPanel;
        private System.Windows.Forms.FlowLayoutPanel dimensionsPanel;
    }
}