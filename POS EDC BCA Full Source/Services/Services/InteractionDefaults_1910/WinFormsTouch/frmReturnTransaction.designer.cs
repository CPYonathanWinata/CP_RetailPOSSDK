/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class frmReturnTransaction
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
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.panelBase = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnReturn = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSelectLine = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDeselectAll = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSelectAll = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDisplayRefundAmounts = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.receiptItems1 = new LSRetailPosis.POSProcesses.WinControls.ReceiptItems();
            this.lblHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(797, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowToolTips = false;
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Tag = "BtnNormal";
            this.btnCancel.Text = "Cancel";
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.tableLayoutPanel2);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Margin = new System.Windows.Forms.Padding(0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Size = new System.Drawing.Size(1024, 768);
            this.panelBase.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.receiptItems1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel2.TabIndex = 43;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 10;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnReturn, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSelectLine, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDeselectAll, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSelectAll, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDisplayRefundAmounts, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(26, 687);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(968, 66);
            this.tableLayoutPanel3.TabIndex = 44;
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturn.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.Appearance.Options.UseFont = true;
            this.btnReturn.Location = new System.Drawing.Point(662, 4);
            this.btnReturn.Margin = new System.Windows.Forms.Padding(4);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.ShowToolTips = false;
            this.btnReturn.Size = new System.Drawing.Size(127, 57);
            this.btnReturn.TabIndex = 2;
            this.btnReturn.Tag = "BtnNormal";
            this.btnReturn.Text = "Return items";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnSelectLine
            // 
            this.btnSelectLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectLine.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectLine.Appearance.Options.UseFont = true;
            this.btnSelectLine.Location = new System.Drawing.Point(253, 4);
            this.btnSelectLine.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectLine.Name = "btnSelectLine";
            this.btnSelectLine.ShowToolTips = false;
            this.btnSelectLine.Size = new System.Drawing.Size(127, 57);
            this.btnSelectLine.TabIndex = 39;
            this.btnSelectLine.Tag = "BtnNormal";
            this.btnSelectLine.Text = "Select line";
            this.btnSelectLine.Click += new System.EventHandler(this.btnSelectLine_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeselectAll.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeselectAll.Appearance.Options.UseFont = true;
            this.btnDeselectAll.Location = new System.Drawing.Point(523, 4);
            this.btnDeselectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.ShowToolTips = false;
            this.btnDeselectAll.Size = new System.Drawing.Size(131, 57);
            this.btnDeselectAll.TabIndex = 38;
            this.btnDeselectAll.Tag = "BtnNormal";
            this.btnDeselectAll.Text = "Clear selection";
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAll.Appearance.Options.UseFont = true;
            this.btnSelectAll.Location = new System.Drawing.Point(388, 4);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.ShowToolTips = false;
            this.btnSelectAll.Size = new System.Drawing.Size(127, 57);
            this.btnSelectAll.TabIndex = 37;
            this.btnSelectAll.Tag = "BtnNormal";
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDisplayRefundAmounts
            // 
            this.btnDisplayRefundAmounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayRefundAmounts.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayRefundAmounts.Appearance.Options.UseFont = true;
            this.btnDisplayRefundAmounts.Location = new System.Drawing.Point(43, 4);
            this.btnDisplayRefundAmounts.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisplayRefundAmounts.Name = "btnDisplayRefundAmounts";
            this.btnDisplayRefundAmounts.ShowToolTips = false;
            this.btnDisplayRefundAmounts.Size = new System.Drawing.Size(202, 57);
            this.btnDisplayRefundAmounts.TabIndex = 39;
            this.btnDisplayRefundAmounts.Tag = "BtnNormal";
            this.btnDisplayRefundAmounts.Text = "Display refund amounts";
            this.btnDisplayRefundAmounts.Click += new System.EventHandler(this.btnDisplayRefundAmounts_Click);
            // 
            // receiptItems1
            // 
            this.receiptItems1.Appearance.Options.UseTextOptions = true;
            this.receiptItems1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.receiptItems1.DesignAllowedOnPos = false;
            this.receiptItems1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.receiptItems1.Location = new System.Drawing.Point(26, 135);
            this.receiptItems1.Margin = new System.Windows.Forms.Padding(0);
            this.receiptItems1.Name = "receiptItems1";
            this.receiptItems1.ReturnItems = false;
            this.receiptItems1.SelectedItemIndex = 0;
            this.receiptItems1.Size = new System.Drawing.Size(968, 552);
            this.receiptItems1.TabIndex = 41;
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(327, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(365, 95);
            this.lblHeading.TabIndex = 40;
            this.lblHeading.Text = "Returnable items";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmReturnTransaction
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelBase);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmReturnTransaction";
            this.Controls.SetChildIndex(this.panelBase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private DevExpress.XtraEditors.PanelControl panelBase;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnReturn;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSelectAll;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDeselectAll;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSelectLine;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDisplayRefundAmounts;
        private System.Windows.Forms.Label lblHeading;
        private LSRetailPosis.POSProcesses.WinControls.ReceiptItems receiptItems1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}