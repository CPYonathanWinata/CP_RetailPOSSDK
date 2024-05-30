/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis.POSProcesses.WinControls;
namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class formOrderDetailsSelection
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnCloseOrder = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnRecalculate = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnViewDetails = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblHeader = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.btnCloseOrder, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.btnRecalculate, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.btnViewDetails, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btnClose, 1, 6);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // btnCloseOrder
            // 
            this.btnCloseOrder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCloseOrder.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseOrder.Appearance.Options.UseFont = true;
            this.btnCloseOrder.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCloseOrder.Location = new System.Drawing.Point(446, 371);
            this.btnCloseOrder.Margin = new System.Windows.Forms.Padding(14);
            this.btnCloseOrder.Name = "btnCloseOrder";
            this.btnCloseOrder.Size = new System.Drawing.Size(127, 57);
            this.btnCloseOrder.TabIndex = 2;
            this.btnCloseOrder.Text = "Close order";
            this.btnCloseOrder.Click += new System.EventHandler(this.OnBtnCloseOrder_Click);
            // 
            // btnRecalculate
            // 
            this.btnRecalculate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRecalculate.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecalculate.Appearance.Options.UseFont = true;
            this.btnRecalculate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRecalculate.Location = new System.Drawing.Point(446, 456);
            this.btnRecalculate.Margin = new System.Windows.Forms.Padding(14);
            this.btnRecalculate.Name = "btnRecalculate";
            this.btnRecalculate.Size = new System.Drawing.Size(127, 57);
            this.btnRecalculate.TabIndex = 4;
            this.btnRecalculate.Text = "Recalculate";
            this.btnRecalculate.Click += new System.EventHandler(this.OnBtnReturnOrder_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnViewDetails.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewDetails.Appearance.Options.UseFont = true;
            this.btnViewDetails.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnViewDetails.Location = new System.Drawing.Point(446, 286);
            this.btnViewDetails.Margin = new System.Windows.Forms.Padding(14);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(127, 57);
            this.btnViewDetails.TabIndex = 0;
            this.btnViewDetails.Text = "View details";
            this.btnViewDetails.Click += new System.EventHandler(this.OnBtnViewDetails_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeader.Appearance.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel.SetColumnSpan(this.lblHeader, 3);
            this.lblHeader.Location = new System.Drawing.Point(269, 43);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(481, 65);
            this.lblHeader.TabIndex = 5;
            this.lblHeader.Text = "Customer order options";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(446, 692);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            // 
            // panelControl
            // 
            this.panelControl.AutoSize = true;
            this.panelControl.Controls.Add(this.tableLayoutPanel);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(1024, 768);
            this.panelControl.TabIndex = 1;
            // 
            // formOrderDetailsSelection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "formOrderDetailsSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.panelControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private SimpleButtonEx btnViewDetails;
        private SimpleButtonEx btnCloseOrder;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private SimpleButtonEx btnRecalculate;
        private DevExpress.XtraEditors.LabelControl lblHeader;
        private SimpleButtonEx btnClose;
        private DevExpress.XtraEditors.StyleController styleController;
    }
}