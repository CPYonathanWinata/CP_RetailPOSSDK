/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class frmGetSalesInvoices
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
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSelect = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.basePanel = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grInvoices = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colInvoiceAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceCurrency = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).BeginInit();
            this.basePanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(514, 692);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowToolTips = false;
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 6;
            this.btnClose.Tag = "BtnNormal";
            this.btnClose.Text = "Close";
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(95, 692);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.ShowToolTips = false;
            this.btnUp.Size = new System.Drawing.Size(57, 57);
            this.btnUp.TabIndex = 2;
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(868, 692);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.ShowToolTips = false;
            this.btnDown.Size = new System.Drawing.Size(57, 57);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(30, 692);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.ShowToolTips = false;
            this.btnPgUp.Size = new System.Drawing.Size(57, 57);
            this.btnPgUp.TabIndex = 1;
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.SalesOrder.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(933, 692);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.ShowToolTips = false;
            this.btnPgDown.Size = new System.Drawing.Size(57, 57);
            this.btnPgDown.TabIndex = 8;
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSelect.Appearance.Options.UseFont = true;
            this.btnSelect.Location = new System.Drawing.Point(379, 692);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4, 11, 4, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.ShowToolTips = false;
            this.btnSelect.Size = new System.Drawing.Size(127, 57);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Tag = "BtnNormal";
            this.btnSelect.Text = "Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.tableLayoutPanel);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Margin = new System.Windows.Forms.Padding(0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(1024, 768);
            this.basePanel.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 10;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.grInvoices, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnPgUp, 0, 9);
            this.tableLayoutPanel.Controls.Add(this.btnDown, 8, 9);
            this.tableLayoutPanel.Controls.Add(this.btnPgDown, 9, 9);
            this.tableLayoutPanel.Controls.Add(this.btnUp, 1, 9);
            this.tableLayoutPanel.Controls.Add(this.btnSelect, 4, 9);
            this.tableLayoutPanel.Controls.Add(this.btnClose, 5, 9);
            this.tableLayoutPanel.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(26, 40, 26, 11);
            this.tableLayoutPanel.RowCount = 10;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // grInvoices
            // 
            this.grInvoices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grInvoices.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel.SetColumnSpan(this.grInvoices, 10);
            this.grInvoices.Location = new System.Drawing.Point(30, 139);
            this.grInvoices.MainView = this.gridView1;
            this.grInvoices.Margin = new System.Windows.Forms.Padding(4);
            this.grInvoices.Name = "grInvoices";
            this.tableLayoutPanel.SetRowSpan(this.grInvoices, 8);
            this.grInvoices.Size = new System.Drawing.Size(960, 538);
            this.grInvoices.TabIndex = 0;
            this.grInvoices.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.grInvoices.DoubleClick += new System.EventHandler(this.grInvoices_DoubleClick);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.ColumnPanelRowHeight = 40;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colInvoiceAccount,
            this.colInvoiceNumber,
            this.colInvoiceDate,
            this.colInvoiceAmount,
            this.colInvoiceCurrency,
            this.colInvoiceId});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView1.GridControl = this.grInvoices;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowHeight = 40;
            this.gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colInvoiceAccount
            // 
            this.colInvoiceAccount.Caption = "Invoice Account";
            this.colInvoiceAccount.FieldName = "INVOICEACCOUNT";
            this.colInvoiceAccount.Name = "colInvoiceAccount";
            this.colInvoiceAccount.Width = 100;
            // 
            // colInvoiceNumber
            // 
            this.colInvoiceNumber.AppearanceCell.Options.UseTextOptions = true;
            this.colInvoiceNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colInvoiceNumber.Caption = "Invoice Number";
            this.colInvoiceNumber.FieldName = "INVOICEID";
            this.colInvoiceNumber.Name = "colInvoiceNumber";
            this.colInvoiceNumber.Visible = true;
            this.colInvoiceNumber.VisibleIndex = 0;
            this.colInvoiceNumber.Width = 100;
            // 
            // colInvoiceDate
            // 
            this.colInvoiceDate.AppearanceCell.Options.UseTextOptions = true;
            this.colInvoiceDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colInvoiceDate.Caption = "Invoice Date";
            this.colInvoiceDate.FieldName = "INVOICEDATE";
            this.colInvoiceDate.Name = "colInvoiceDate";
            this.colInvoiceDate.Visible = true;
            this.colInvoiceDate.VisibleIndex = 1;
            this.colInvoiceDate.Width = 99;
            // 
            // colInvoiceAmount
            // 
            this.colInvoiceAmount.AppearanceCell.Options.UseTextOptions = true;
            this.colInvoiceAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colInvoiceAmount.Caption = "Invoice Amount";
            this.colInvoiceAmount.DisplayFormat.FormatString = "N2";
            this.colInvoiceAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colInvoiceAmount.FieldName = "INVOICEAMOUNT";
            this.colInvoiceAmount.Name = "colInvoiceAmount";
            this.colInvoiceAmount.Visible = true;
            this.colInvoiceAmount.VisibleIndex = 2;
            this.colInvoiceAmount.Width = 140;
            // 
            // colInvoiceCurrency
            // 
            this.colInvoiceCurrency.AppearanceCell.Options.UseTextOptions = true;
            this.colInvoiceCurrency.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colInvoiceCurrency.Caption = "Invoice Currency";
            this.colInvoiceCurrency.FieldName = "CURRENCYCODE";
            this.colInvoiceCurrency.Name = "colInvoiceCurrency";
            this.colInvoiceCurrency.Visible = true;
            this.colInvoiceCurrency.VisibleIndex = 3;
            this.colInvoiceCurrency.Width = 220;
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.Caption = "InvoiceId";
            this.colInvoiceId.FieldName = "INVOICEID";
            this.colInvoiceId.Name = "colInvoiceId";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitle.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblTitle, 10);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblTitle.Location = new System.Drawing.Point(320, 40);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblTitle.Size = new System.Drawing.Size(379, 95);
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "Get sales invoices";
            // 
            // frmGetSalesInvoices
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.basePanel);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmGetSalesInvoices";
            this.Controls.SetChildIndex(this.basePanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePanel)).EndInit();
            this.basePanel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grInvoices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSelect;
        private DevExpress.XtraEditors.PanelControl basePanel;
        private DevExpress.XtraGrid.GridControl grInvoices;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceDate;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceCurrency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceAccount;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceId;
        private System.Windows.Forms.Label lblTitle;
    }
}
