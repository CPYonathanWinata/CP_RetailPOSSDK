/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class DisbursementSlipCreationForm
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblHeading = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSuplpement = new System.Windows.Forms.Label();
            this.lblCustomerIdentityCard = new System.Windows.Forms.Label();
            this.panelAmount = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDocumentDate = new System.Windows.Forms.Label();
            this.lblPersonName = new System.Windows.Forms.Label();
            this.lblReasonOfReturn = new System.Windows.Forms.Label();
            this.lblDocumentNum = new System.Windows.Forms.Label();
            this.tbPersonName = new System.Windows.Forms.TextBox();
            this.tbCustomerIdentityCard = new System.Windows.Forms.TextBox();
            this.tbReasonOfReturn = new System.Windows.Forms.TextBox();
            this.tbDocumentNum = new System.Windows.Forms.TextBox();
            this.dtDocumentDate = new System.Windows.Forms.DateTimePicker();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelControl1.AutoSize = true;
            this.panelControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Location = new System.Drawing.Point(217, 194);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(3);
            this.panelControl1.Size = new System.Drawing.Size(6, 6);
            this.panelControl1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblHeading, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel6, 3);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnClear, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnOk, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(30, 692);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(964, 65);
            this.tableLayoutPanel6.TabIndex = 9;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.Location = new System.Drawing.Point(283, 4);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(127, 57);
            this.btnClear.TabIndex = 0;
            this.btnClear.Tag = "BtnExtraLong";
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "IsValid", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnOk.Location = new System.Drawing.Point(418, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 1;
            this.btnOk.Tag = "BtnExtraLong";
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(553, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Tag = "BtnExtraLong";
            this.btnCancel.Text = "Cancel";
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeading.Location = new System.Drawing.Point(250, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeading.Size = new System.Drawing.Size(524, 95);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Tag = "";
            this.lblHeading.Text = "Disbursement slip details";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblSuplpement, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.lblCustomerIdentityCard, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.panelAmount, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblDocumentDate, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.lblPersonName, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblReasonOfReturn, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblDocumentNum, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.tbPersonName, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.tbCustomerIdentityCard, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.tbReasonOfReturn, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.tbDocumentNum, 2, 6);
            this.tableLayoutPanel4.Controls.Add(this.dtDocumentDate, 2, 7);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(50, 238);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 10;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(923, 350);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // lblSuplpement
            // 
            this.lblSuplpement.AutoSize = true;
            this.lblSuplpement.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSuplpement.Location = new System.Drawing.Point(5, 244);
            this.lblSuplpement.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblSuplpement.Name = "lblSuplpement";
            this.lblSuplpement.Size = new System.Drawing.Size(120, 25);
            this.lblSuplpement.TabIndex = 41;
            this.lblSuplpement.Text = "Supplement";
            // 
            // lblCustomerIdentityCard
            // 
            this.lblCustomerIdentityCard.AutoSize = true;
            this.lblCustomerIdentityCard.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblCustomerIdentityCard.Location = new System.Drawing.Point(5, 42);
            this.lblCustomerIdentityCard.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblCustomerIdentityCard.Name = "lblCustomerIdentityCard";
            this.lblCustomerIdentityCard.Size = new System.Drawing.Size(207, 25);
            this.lblCustomerIdentityCard.TabIndex = 21;
            this.lblCustomerIdentityCard.Text = "Customer identity card:";
            // 
            // panelAmount
            // 
            this.panelAmount.Controls.Add(this.tableLayoutPanel1);
            this.panelAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAmount.Location = new System.Drawing.Point(259, 104);
            this.panelAmount.Margin = new System.Windows.Forms.Padding(5);
            this.panelAmount.Name = "panelAmount";
            this.panelAmount.Size = new System.Drawing.Size(181, 312);
            this.panelAmount.TabIndex = 18;
            this.panelAmount.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(177, 308);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lblDocumentDate
            // 
            this.lblDocumentDate.AutoSize = true;
            this.lblDocumentDate.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblDocumentDate.Location = new System.Drawing.Point(5, 314);
            this.lblDocumentDate.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblDocumentDate.Name = "lblDocumentDate";
            this.lblDocumentDate.Size = new System.Drawing.Size(145, 25);
            this.lblDocumentDate.TabIndex = 6;
            this.lblDocumentDate.Text = "Document date:";
            // 
            // lblPersonName
            // 
            this.lblPersonName.AutoSize = true;
            this.lblPersonName.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblPersonName.Location = new System.Drawing.Point(5, 3);
            this.lblPersonName.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblPersonName.Name = "lblPersonName";
            this.lblPersonName.Size = new System.Drawing.Size(126, 25);
            this.lblPersonName.TabIndex = 20;
            this.lblPersonName.Text = "Person name:";
            // 
            // lblReasonOfReturn
            // 
            this.lblReasonOfReturn.AutoSize = true;
            this.lblReasonOfReturn.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblReasonOfReturn.Location = new System.Drawing.Point(5, 144);
            this.lblReasonOfReturn.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblReasonOfReturn.Name = "lblReasonOfReturn";
            this.lblReasonOfReturn.Size = new System.Drawing.Size(199, 25);
            this.lblReasonOfReturn.TabIndex = 23;
            this.lblReasonOfReturn.Text = "Reason of cash return:";
            // 
            // lblDocumentNum
            // 
            this.lblDocumentNum.AutoSize = true;
            this.lblDocumentNum.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblDocumentNum.Location = new System.Drawing.Point(5, 275);
            this.lblDocumentNum.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.lblDocumentNum.Name = "lblDocumentNum";
            this.lblDocumentNum.Size = new System.Drawing.Size(103, 25);
            this.lblDocumentNum.TabIndex = 24;
            this.lblDocumentNum.Text = "Document:";
            // 
            // tbPersonName
            // 
            this.tbPersonName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "PersonName", true));
            this.tbPersonName.Location = new System.Drawing.Point(218, 3);
            this.tbPersonName.Name = "tbPersonName";
            this.tbPersonName.Size = new System.Drawing.Size(702, 33);
            this.tbPersonName.TabIndex = 36;
            // 
            // tbCustomerIdentityCard
            // 
            this.tbCustomerIdentityCard.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "CardInfo", true));
            this.tbCustomerIdentityCard.Location = new System.Drawing.Point(218, 42);
            this.tbCustomerIdentityCard.Multiline = true;
            this.tbCustomerIdentityCard.Name = "tbCustomerIdentityCard";
            this.tbCustomerIdentityCard.Size = new System.Drawing.Size(702, 96);
            this.tbCustomerIdentityCard.TabIndex = 37;
            // 
            // tbReasonOfReturn
            // 
            this.tbReasonOfReturn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ReasonOfReturn", true));
            this.tbReasonOfReturn.Location = new System.Drawing.Point(218, 144);
            this.tbReasonOfReturn.Name = "tbReasonOfReturn";
            this.tbReasonOfReturn.Size = new System.Drawing.Size(702, 33);
            this.tbReasonOfReturn.TabIndex = 38;
            // 
            // tbDocumentNum
            // 
            this.tbDocumentNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "DocumentNum", true));
            this.tbDocumentNum.Location = new System.Drawing.Point(218, 275);
            this.tbDocumentNum.Name = "tbDocumentNum";
            this.tbDocumentNum.Size = new System.Drawing.Size(702, 33);
            this.tbDocumentNum.TabIndex = 39;
            // 
            // dtDocumentDate
            // 
            this.dtDocumentDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "DocumentDate", true));
            this.dtDocumentDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "DocumentDate", true));
            this.dtDocumentDate.Location = new System.Drawing.Point(218, 314);
            this.dtDocumentDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtDocumentDate.Name = "dtDocumentDate";
            this.dtDocumentDate.Size = new System.Drawing.Size(212, 33);
            this.dtDocumentDate.TabIndex = 40;
            this.dtDocumentDate.Value = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels.DisbursementSlipCreationViewModel);
            // 
            // DisbursementSlipCreationForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "DisbursementSlipCreationForm";
            this.Text = "DisbursementSlipCreationForm";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClear;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblDocumentDate;
        private DevExpress.XtraEditors.PanelControl panelAmount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCustomerIdentityCard;
        private System.Windows.Forms.Label lblPersonName;
        private System.Windows.Forms.Label lblReasonOfReturn;
        private System.Windows.Forms.Label lblDocumentNum;
        private System.Windows.Forms.TextBox tbPersonName;
        private System.Windows.Forms.TextBox tbCustomerIdentityCard;
        private System.Windows.Forms.TextBox tbReasonOfReturn;
        private System.Windows.Forms.TextBox tbDocumentNum;
        private System.Windows.Forms.DateTimePicker dtDocumentDate;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.Label lblSuplpement;
    }
}
