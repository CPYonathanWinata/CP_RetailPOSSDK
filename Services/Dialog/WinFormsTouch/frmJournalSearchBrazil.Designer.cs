/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis.POSProcesses;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch
{
    partial class frmJournalSearchBrazil : frmTouchBase
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
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnMoreOptions = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClear = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.searchPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblFiscalReceiptNumber = new DevExpress.XtraEditors.LabelControl();
            this.txtFiscalReceiptNumber = new DevExpress.XtraEditors.TextEdit();
            this.lblReceiptNumber = new DevExpress.XtraEditors.LabelControl();
            this.txtReceiptNumber = new DevExpress.XtraEditors.TextEdit();
            this.lblCPFCNPJ = new DevExpress.XtraEditors.LabelControl();
            this.lblForeignerId = new DevExpress.XtraEditors.LabelControl();
            this.txtForeignerId = new DevExpress.XtraEditors.TextEdit();
            this.lblFiscalDocumentSeries = new DevExpress.XtraEditors.LabelControl();
            this.txtFiscalDocumentSeries = new DevExpress.XtraEditors.TextEdit();
            this.lblFiscalDocumentNumber = new DevExpress.XtraEditors.LabelControl();
            this.txtFiscalDocumentNumber = new DevExpress.XtraEditors.TextEdit();
            this.txtCNPJCPF = new DevExpress.XtraEditors.TextEdit();
            this.lblHeader = new System.Windows.Forms.Label();
            this.basePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiscalReceiptNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtForeignerId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiscalDocumentSeries.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiscalDocumentNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCNPJCPF.Properties)).BeginInit();
            this.basePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelBottom, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.searchPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1016, 741);
            this.tableLayoutPanel1.TabIndex = 40;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelBottom.AutoSize = true;
            this.tableLayoutPanelBottom.ColumnCount = 4;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Controls.Add(this.btnMoreOptions, 0, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnClear, 1, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnClose, 2, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnSearch, 0, 0);
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(33, 641);
            this.tableLayoutPanelBottom.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(950, 66);
            this.tableLayoutPanelBottom.TabIndex = 51;
            // 
            // btnMoreOptions
            // 
            this.btnMoreOptions.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMoreOptions.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoreOptions.Appearance.Options.UseFont = true;
            this.btnMoreOptions.AutoWidthInLayoutControl = true;
            this.btnMoreOptions.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnMoreOptions.Location = new System.Drawing.Point(342, 3);
            this.btnMoreOptions.Name = "btnMoreOptions";
            this.btnMoreOptions.Padding = new System.Windows.Forms.Padding(0);
            this.btnMoreOptions.Size = new System.Drawing.Size(130, 60);
            this.btnMoreOptions.TabIndex = 53;
            this.btnMoreOptions.Tag = "";
            this.btnMoreOptions.Text = "More Options";
            this.btnMoreOptions.Click += new System.EventHandler(this.btnMoreOptions_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClear.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(478, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Padding = new System.Windows.Forms.Padding(0);
            this.btnClear.Size = new System.Drawing.Size(130, 60);
            this.btnClear.TabIndex = 54;
            this.btnClear.Tag = "";
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.AutoWidthInLayoutControl = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClose.Location = new System.Drawing.Point(614, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(0);
            this.btnClose.Size = new System.Drawing.Size(130, 60);
            this.btnClose.TabIndex = 55;
            this.btnClose.Tag = "";
            this.btnClose.Text = "Cancel";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.AutoWidthInLayoutControl = true;
            this.btnSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(206, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Padding = new System.Windows.Forms.Padding(0);
            this.btnSearch.Size = new System.Drawing.Size(130, 60);
            this.btnSearch.TabIndex = 52;
            this.btnSearch.Tag = "";
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // searchPanel
            // 
            this.searchPanel.AllowDrop = true;
            this.searchPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.searchPanel.ColumnCount = 2;
            this.searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.searchPanel.Controls.Add(this.lblFiscalReceiptNumber, 0, 0);
            this.searchPanel.Controls.Add(this.txtFiscalReceiptNumber, 0, 1);
            this.searchPanel.Controls.Add(this.lblReceiptNumber, 1, 0);
            this.searchPanel.Controls.Add(this.txtReceiptNumber, 1, 1);
            this.searchPanel.Controls.Add(this.lblCPFCNPJ, 0, 2);
            this.searchPanel.Controls.Add(this.lblForeignerId, 1, 2);
            this.searchPanel.Controls.Add(this.txtForeignerId, 1, 3);
            this.searchPanel.Controls.Add(this.txtCNPJCPF, 0, 3);
            this.searchPanel.Controls.Add(this.txtFiscalDocumentNumber, 0, 5);
            this.searchPanel.Controls.Add(this.lblFiscalDocumentNumber, 0, 4);
            this.searchPanel.Controls.Add(this.lblFiscalDocumentSeries, 1, 4);
            this.searchPanel.Controls.Add(this.txtFiscalDocumentSeries, 1, 5);
            this.searchPanel.Location = new System.Drawing.Point(30, 157);
            this.searchPanel.Margin = new System.Windows.Forms.Padding(0);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(30, 10, 30, 11);
            this.searchPanel.RowCount = 7;
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.searchPanel.Size = new System.Drawing.Size(950, 446);
            this.searchPanel.TabIndex = 41;
            // 
            // lblFiscalReceiptNumber
            // 
            this.lblFiscalReceiptNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFiscalReceiptNumber.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalReceiptNumber.Location = new System.Drawing.Point(33, 42);
            this.lblFiscalReceiptNumber.Name = "lblFiscalReceiptNumber";
            this.lblFiscalReceiptNumber.Size = new System.Drawing.Size(180, 25);
            this.lblFiscalReceiptNumber.TabIndex = 33;
            this.lblFiscalReceiptNumber.Text = "Fiscal receipt number";
            // 
            // txtFiscalReceiptNumber
            // 
            this.txtFiscalReceiptNumber.Location = new System.Drawing.Point(33, 73);
            this.txtFiscalReceiptNumber.Name = "txtFiscalReceiptNumber";
            this.txtFiscalReceiptNumber.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtFiscalReceiptNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtFiscalReceiptNumber.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtFiscalReceiptNumber.Properties.Appearance.Options.UseBackColor = true;
            this.txtFiscalReceiptNumber.Properties.Appearance.Options.UseFont = true;
            this.txtFiscalReceiptNumber.Properties.Appearance.Options.UseForeColor = true;
            this.txtFiscalReceiptNumber.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFiscalReceiptNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtFiscalReceiptNumber.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFiscalReceiptNumber.Properties.Mask.EditMask = "######";
            this.txtFiscalReceiptNumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtFiscalReceiptNumber.Size = new System.Drawing.Size(288, 32);
            this.txtFiscalReceiptNumber.TabIndex = 0;
            // 
            // lblReceiptNumber
            // 
            this.lblReceiptNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblReceiptNumber.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNumber.Location = new System.Drawing.Point(477, 42);
            this.lblReceiptNumber.Name = "lblReceiptNumber";
            this.lblReceiptNumber.Size = new System.Drawing.Size(133, 25);
            this.lblReceiptNumber.TabIndex = 42;
            this.lblReceiptNumber.Text = "Receipt number";
            // 
            // txtReceiptNumber
            // 
            this.txtReceiptNumber.Location = new System.Drawing.Point(477, 73);
            this.txtReceiptNumber.Name = "txtReceiptNumber";
            this.txtReceiptNumber.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtReceiptNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtReceiptNumber.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtReceiptNumber.Properties.Appearance.Options.UseBackColor = true;
            this.txtReceiptNumber.Properties.Appearance.Options.UseFont = true;
            this.txtReceiptNumber.Properties.Appearance.Options.UseForeColor = true;
            this.txtReceiptNumber.Properties.Appearance.Options.UseTextOptions = true;
            this.txtReceiptNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtReceiptNumber.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtReceiptNumber.Properties.Mask.EditMask = "######";
            this.txtReceiptNumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtReceiptNumber.Size = new System.Drawing.Size(288, 32);
            this.txtReceiptNumber.TabIndex = 43;
            // 
            // lblCPFCNPJ
            // 
            this.lblCPFCNPJ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCPFCNPJ.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPFCNPJ.Location = new System.Drawing.Point(33, 162);
            this.lblCPFCNPJ.Name = "lblCPFCNPJ";
            this.lblCPFCNPJ.Size = new System.Drawing.Size(83, 25);
            this.lblCPFCNPJ.TabIndex = 44;
            this.lblCPFCNPJ.Text = "CNPJ/CPF";
            // 
            // lblForeignerId
            // 
            this.lblForeignerId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblForeignerId.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForeignerId.Location = new System.Drawing.Point(477, 162);
            this.lblForeignerId.Name = "lblForeignerId";
            this.lblForeignerId.Size = new System.Drawing.Size(102, 25);
            this.lblForeignerId.TabIndex = 45;
            this.lblForeignerId.Text = "Foreigner Id";
            // 
            // txtForeignerId
            // 
            this.txtForeignerId.Location = new System.Drawing.Point(477, 193);
            this.txtForeignerId.Name = "txtForeignerId";
            this.txtForeignerId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtForeignerId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtForeignerId.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtForeignerId.Properties.Appearance.Options.UseBackColor = true;
            this.txtForeignerId.Properties.Appearance.Options.UseFont = true;
            this.txtForeignerId.Properties.Appearance.Options.UseForeColor = true;
            this.txtForeignerId.Properties.Appearance.Options.UseTextOptions = true;
            this.txtForeignerId.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtForeignerId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtForeignerId.Properties.Mask.EditMask = "####################";
            this.txtForeignerId.Properties.MaxLength = 20;
            this.txtForeignerId.Size = new System.Drawing.Size(288, 32);
            this.txtForeignerId.TabIndex = 46;
            // 
            // lblFiscalDocumentSeries
            // 
            this.lblFiscalDocumentSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFiscalDocumentSeries.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalDocumentSeries.Location = new System.Drawing.Point(477, 282);
            this.lblFiscalDocumentSeries.Name = "lblFiscalDocumentSeries";
            this.lblFiscalDocumentSeries.Size = new System.Drawing.Size(189, 25);
            this.lblFiscalDocumentSeries.TabIndex = 49;
            this.lblFiscalDocumentSeries.Text = "Fiscal document series";
            // 
            // txtFiscalDocumentSeries
            // 
            this.txtFiscalDocumentSeries.Location = new System.Drawing.Point(477, 313);
            this.txtFiscalDocumentSeries.Name = "txtFiscalDocumentSeries";
            this.txtFiscalDocumentSeries.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtFiscalDocumentSeries.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtFiscalDocumentSeries.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtFiscalDocumentSeries.Properties.Appearance.Options.UseBackColor = true;
            this.txtFiscalDocumentSeries.Properties.Appearance.Options.UseFont = true;
            this.txtFiscalDocumentSeries.Properties.Appearance.Options.UseForeColor = true;
            this.txtFiscalDocumentSeries.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFiscalDocumentSeries.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtFiscalDocumentSeries.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFiscalDocumentSeries.Properties.Mask.EditMask = "###";
            this.txtFiscalDocumentSeries.Properties.MaxLength = 3;
            this.txtFiscalDocumentSeries.Size = new System.Drawing.Size(288, 32);
            this.txtFiscalDocumentSeries.TabIndex = 50;
            // 
            // lblFiscalDocumentNumber
            // 
            this.lblFiscalDocumentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFiscalDocumentNumber.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiscalDocumentNumber.Location = new System.Drawing.Point(33, 282);
            this.lblFiscalDocumentNumber.Name = "lblFiscalDocumentNumber";
            this.lblFiscalDocumentNumber.Size = new System.Drawing.Size(207, 25);
            this.lblFiscalDocumentNumber.TabIndex = 47;
            this.lblFiscalDocumentNumber.Text = "Fiscal document number";
            // 
            // txtFiscalDocumentNumber
            // 
            this.txtFiscalDocumentNumber.Location = new System.Drawing.Point(33, 313);
            this.txtFiscalDocumentNumber.Name = "txtFiscalDocumentNumber";
            this.txtFiscalDocumentNumber.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtFiscalDocumentNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtFiscalDocumentNumber.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtFiscalDocumentNumber.Properties.Appearance.Options.UseBackColor = true;
            this.txtFiscalDocumentNumber.Properties.Appearance.Options.UseFont = true;
            this.txtFiscalDocumentNumber.Properties.Appearance.Options.UseForeColor = true;
            this.txtFiscalDocumentNumber.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFiscalDocumentNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtFiscalDocumentNumber.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFiscalDocumentNumber.Properties.Mask.EditMask = "####################";
            this.txtFiscalDocumentNumber.Properties.MaxLength = 20;
            this.txtFiscalDocumentNumber.Size = new System.Drawing.Size(288, 32);
            this.txtFiscalDocumentNumber.TabIndex = 48;
            // 
            // txtCNPJCPF
            // 
            this.txtCNPJCPF.Location = new System.Drawing.Point(33, 193);
            this.txtCNPJCPF.Name = "txtCNPJCPF";
            this.txtCNPJCPF.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCNPJCPF.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtCNPJCPF.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtCNPJCPF.Properties.Appearance.Options.UseBackColor = true;
            this.txtCNPJCPF.Properties.Appearance.Options.UseFont = true;
            this.txtCNPJCPF.Properties.Appearance.Options.UseForeColor = true;
            this.txtCNPJCPF.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCNPJCPF.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCNPJCPF.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCNPJCPF.Properties.Mask.EditMask = "####################";
            this.txtCNPJCPF.Properties.MaxLength = 20;
            this.txtCNPJCPF.Size = new System.Drawing.Size(288, 32);
            this.txtCNPJCPF.TabIndex = 45;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(351, 40);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(314, 65);
            this.lblHeader.TabIndex = 40;
            this.lblHeader.Text = "Search journal";
            // 
            // basePanel
            // 
            this.basePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.basePanel.Controls.Add(this.tableLayoutPanel1);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(1018, 743);
            this.basePanel.TabIndex = 0;
            // 
            // frmJournalSearchBrazil
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AcceptButton = btnSearch;
            this.ClientSize = new System.Drawing.Size(1018, 743);
            this.Controls.Add(this.basePanel);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmJournalSearchBrazil";
            this.Controls.SetChildIndex(this.basePanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiscalReceiptNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtForeignerId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiscalDocumentSeries.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFiscalDocumentNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCNPJCPF.Properties)).EndInit();
            this.basePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel basePanel;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClear;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSearch;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnMoreOptions;
        private System.Windows.Forms.TableLayoutPanel searchPanel;
        private DevExpress.XtraEditors.LabelControl lblFiscalReceiptNumber;
        private DevExpress.XtraEditors.TextEdit txtFiscalReceiptNumber;
        private DevExpress.XtraEditors.LabelControl lblReceiptNumber;
        private DevExpress.XtraEditors.TextEdit txtReceiptNumber;
        private DevExpress.XtraEditors.LabelControl lblCPFCNPJ;
        private DevExpress.XtraEditors.LabelControl lblForeignerId;
        private DevExpress.XtraEditors.TextEdit txtForeignerId;
        private DevExpress.XtraEditors.LabelControl lblFiscalDocumentSeries;
        private DevExpress.XtraEditors.TextEdit txtFiscalDocumentSeries;
        private DevExpress.XtraEditors.LabelControl lblFiscalDocumentNumber;
        private DevExpress.XtraEditors.TextEdit txtFiscalDocumentNumber;
        private DevExpress.XtraEditors.TextEdit txtCNPJCPF;
    }
}
