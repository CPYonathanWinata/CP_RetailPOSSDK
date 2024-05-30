/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class frmProductSearchAttributesFilter
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
            this.panelbase = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.parentAttributesPanel = new System.Windows.Forms.Panel();
            this.attributesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.panelSeachFilterAttributesValues = new DevExpress.XtraEditors.PanelControl();
            this.parentAttributesValuesPanel = new System.Windows.Forms.Panel();
            this.attributesValuesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPageDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAttributesListUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnAttributesListDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPageUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOK = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.panelbase.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.parentAttributesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSeachFilterAttributesValues)).BeginInit();
            this.panelSeachFilterAttributesValues.SuspendLayout();
            this.parentAttributesValuesPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 14;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.parentAttributesPanel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.lblHeader, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.btnDown, 11, 2);
            this.tableLayoutPanel.Controls.Add(this.panelSeachFilterAttributesValues, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.btnPageDown, 10, 2);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.btnPageUp, 5, 2);
            this.tableLayoutPanel.Controls.Add(this.btnUp, 4, 2);
            this.tableLayoutPanel.Controls.Add(this.btnCancel, 8, 2);
            this.tableLayoutPanel.Controls.Add(this.btnOK, 7, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(30, 40, 30, 15);
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1018, 766);
            this.tableLayoutPanel.TabIndex = 10;
            // 
            // parentAttributesPanel
            // 
            this.parentAttributesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parentAttributesPanel.Controls.Add(this.attributesPanel);
            this.parentAttributesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentAttributesPanel.Location = new System.Drawing.Point(53, 138);
            this.parentAttributesPanel.Name = "parentAttributesPanel";
            this.parentAttributesPanel.Size = new System.Drawing.Size(150, 504);
            this.parentAttributesPanel.TabIndex = 11;
            this.parentAttributesPanel.Resize += new System.EventHandler(this.attributesPanel_Resize);
            // 
            // attributesPanel
            // 
            this.attributesPanel.AutoScroll = true;
            this.attributesPanel.Location = new System.Drawing.Point(0, 0);
            this.attributesPanel.Name = "attributesPanel";
            this.attributesPanel.Size = new System.Drawing.Size(115, 51);
            this.attributesPanel.TabIndex = 10;
            this.attributesPanel.Resize += new System.EventHandler(this.parentAttributesPanel_Resize);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblHeader, 13);
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(53, 40);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(3, 0, 3, 30);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(932, 65);
            this.lblHeader.TabIndex = 10;
            this.lblHeader.Text = "Select product options";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(879, 663);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 61);
            this.btnDown.TabIndex = 15;
            this.btnDown.Text = "Ê";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // panelSeachFilterAttributesValues
            // 
            this.panelSeachFilterAttributesValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.panelSeachFilterAttributesValues, 8);
            this.panelSeachFilterAttributesValues.Controls.Add(this.parentAttributesValuesPanel);
            this.panelSeachFilterAttributesValues.Location = new System.Drawing.Point(265, 138);
            this.panelSeachFilterAttributesValues.Name = "panelSeachFilterAttributesValues";
            this.panelSeachFilterAttributesValues.Size = new System.Drawing.Size(680, 504);
            this.panelSeachFilterAttributesValues.TabIndex = 0;
            // 
            // parentAttributesValuesPanel
            // 
            this.parentAttributesValuesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parentAttributesValuesPanel.Controls.Add(this.attributesValuesPanel);
            this.parentAttributesValuesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentAttributesValuesPanel.Location = new System.Drawing.Point(2, 2);
            this.parentAttributesValuesPanel.Name = "parentAttributesValuesPanel";
            this.parentAttributesValuesPanel.Size = new System.Drawing.Size(676, 500);
            this.parentAttributesValuesPanel.TabIndex = 10;
            this.parentAttributesValuesPanel.Resize += new System.EventHandler(this.ParentPanel_Resize);
            // 
            // attributesValuesPanel
            // 
            this.attributesValuesPanel.AutoScroll = true;
            this.attributesValuesPanel.Location = new System.Drawing.Point(0, 0);
            this.attributesValuesPanel.Name = "attributesValuesPanel";
            this.attributesValuesPanel.Size = new System.Drawing.Size(115, 51);
            this.attributesValuesPanel.TabIndex = 10;
            this.attributesValuesPanel.Resize += new System.EventHandler(this.attributesValuesPanel_Resize);
            // 
            // btnPageDown
            // 
            this.btnPageDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageDown.Appearance.Options.UseFont = true;
            this.btnPageDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.bottom;
            this.btnPageDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageDown.Location = new System.Drawing.Point(806, 663);
            this.btnPageDown.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnPageDown.Name = "btnPageDown";
            this.btnPageDown.Size = new System.Drawing.Size(65, 61);
            this.btnPageDown.TabIndex = 12;
            this.btnPageDown.Text = "ò";
            this.btnPageDown.Click += new System.EventHandler(this.btnPageDown_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnAttributesListUp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAttributesListDown, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(53, 648);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(146, 80);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // btnAttributesListUp
            // 
            this.btnAttributesListUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAttributesListUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnAttributesListUp.Appearance.Options.UseFont = true;
            this.btnAttributesListUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.up;
            this.btnAttributesListUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAttributesListUp.Location = new System.Drawing.Point(4, 15);
            this.btnAttributesListUp.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnAttributesListUp.Name = "btnAttributesListUp";
            this.btnAttributesListUp.Size = new System.Drawing.Size(65, 61);
            this.btnAttributesListUp.TabIndex = 16;
            this.btnAttributesListUp.Text = "ñ";
            this.btnAttributesListUp.Click += new System.EventHandler(this.btnAttributesListUp_Click);
            // 
            // btnAttributesListDown
            // 
            this.btnAttributesListDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAttributesListDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnAttributesListDown.Appearance.Options.UseFont = true;
            this.btnAttributesListDown.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.down;
            this.btnAttributesListDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAttributesListDown.Location = new System.Drawing.Point(77, 15);
            this.btnAttributesListDown.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnAttributesListDown.Name = "btnAttributesListDown";
            this.btnAttributesListDown.Size = new System.Drawing.Size(65, 61);
            this.btnAttributesListDown.TabIndex = 17;
            this.btnAttributesListDown.Text = "Ê";
            this.btnAttributesListDown.Click += new System.EventHandler(this.btnAttributesListDown_Click);
            // 
            // btnPageUp
            // 
            this.btnPageUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPageUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPageUp.Appearance.Options.UseFont = true;
            this.btnPageUp.Image = global::Microsoft.Dynamics.Retail.Pos.Interaction.Properties.Resources.top;
            this.btnPageUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPageUp.Location = new System.Drawing.Point(339, 663);
            this.btnPageUp.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnPageUp.Name = "btnPageUp";
            this.btnPageUp.Size = new System.Drawing.Size(65, 61);
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
            this.btnUp.Location = new System.Drawing.Point(266, 663);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 15, 4, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 61);
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
            this.btnCancel.Location = new System.Drawing.Point(608, 665);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(475, 665);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(127, 57);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmProductSearchAttributesFilter
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1018, 766);
            this.Controls.Add(this.panelbase);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmProductSearchAttributesFilter";
            this.Controls.SetChildIndex(this.panelbase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.panelbase.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.parentAttributesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSeachFilterAttributesValues)).EndInit();
            this.panelSeachFilterAttributesValues.ResumeLayout(false);
            this.parentAttributesValuesPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelbase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblHeader;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnAttributesListDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnAttributesListUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPageDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelSeachFilterAttributesValues;
        private System.Windows.Forms.Panel parentAttributesValuesPanel;
        private System.Windows.Forms.FlowLayoutPanel attributesValuesPanel;
        private System.Windows.Forms.Panel parentAttributesPanel;
        private System.Windows.Forms.FlowLayoutPanel attributesPanel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOK;
    }
}