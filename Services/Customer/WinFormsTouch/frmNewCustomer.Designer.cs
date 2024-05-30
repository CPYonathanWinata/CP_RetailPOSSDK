/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch
{
    partial class frmNewCustomer
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
            this.themedPanel = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelCenter = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnSave = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClear = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tabControlParent = new DevExpress.XtraTab.XtraTabControl();
            this.tabPageContact = new DevExpress.XtraTab.XtraTabPage();
            this.panelDetailsTab = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxLastName = new DevExpress.XtraEditors.TextEdit();
            this.labelLastName = new System.Windows.Forms.Label();
            this.textBoxMiddleName = new DevExpress.XtraEditors.TextEdit();
            this.labelMiddleName = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new DevExpress.XtraEditors.TextEdit();
            this.labelType = new System.Windows.Forms.Label();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.textBoxFirstName = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanelType = new System.Windows.Forms.TableLayoutPanel();
            this.radioPerson = new System.Windows.Forms.RadioButton();
            this.radioOrg = new System.Windows.Forms.RadioButton();
            this.labelPhone = new System.Windows.Forms.Label();
            this.textBoxPhone = new DevExpress.XtraEditors.TextEdit();
            this.viewAddressUserControl1 = new Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch.ViewAddressUserControl();
            this.labelCpfCnpjNumber = new System.Windows.Forms.Label();
            this.textBoxCpfCnpjNumber = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAffiliations = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblAffiliationCount = new System.Windows.Forms.Label();
            this.labelAffiliations = new System.Windows.Forms.Label();
            this.textBoxLanguage = new DevExpress.XtraEditors.TextEdit();
            this.btnLanguage = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.btnCurrency = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelGroup = new System.Windows.Forms.Label();
            this.textBoxCurrency = new DevExpress.XtraEditors.TextEdit();
            this.labelReceiptEmail = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelCurrency = new System.Windows.Forms.Label();
            this.labelWebSite = new System.Windows.Forms.Label();
            this.btnGroup = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.textBoxEmail = new DevExpress.XtraEditors.TextEdit();
            this.textBoxGroup = new DevExpress.XtraEditors.TextEdit();
            this.textBoxReceiptEmail = new DevExpress.XtraEditors.TextEdit();
            this.textBoxWebSite = new DevExpress.XtraEditors.TextEdit();
            this.textIDNumber = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBoxGender = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateBirth = new System.Windows.Forms.DateTimePicker();
            this.cmbBoxCustType = new System.Windows.Forms.ComboBox();
            this.tabPageHistory = new DevExpress.XtraTab.XtraTabPage();
            this.panelHistoryTab = new System.Windows.Forms.TableLayoutPanel();
            this.labelDateCreated = new System.Windows.Forms.Label();
            this.labelTotalVisits = new System.Windows.Forms.Label();
            this.gridOrders = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.columnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnOrderNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnOrderStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnStore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textDateCreated = new DevExpress.XtraEditors.TextEdit();
            this.textTotalVisits = new DevExpress.XtraEditors.TextEdit();
            this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelDateLastVisit = new System.Windows.Forms.Label();
            this.textDateLastVisit = new DevExpress.XtraEditors.TextEdit();
            this.labelStoreLastVisited = new System.Windows.Forms.Label();
            this.textStoreLastVisited = new DevExpress.XtraEditors.TextEdit();
            this.labelTotalSales = new System.Windows.Forms.Label();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textSearch = new DevExpress.XtraEditors.TextEdit();
            this.buttonSearch = new DevExpress.XtraEditors.SimpleButton();
            this.buttonClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.textTotalSales = new DevExpress.XtraEditors.TextEdit();
            this.labelBalance = new System.Windows.Forms.Label();
            this.textBalance = new DevExpress.XtraEditors.TextEdit();
            this.layoutPanelInvoiceAccount = new System.Windows.Forms.TableLayoutPanel();
            this.labelInvAccountId = new DevExpress.XtraEditors.LabelControl();
            this.labelInvAccountName = new DevExpress.XtraEditors.LabelControl();
            this.textInvAccountName = new DevExpress.XtraEditors.TextEdit();
            this.textInvAccountId = new DevExpress.XtraEditors.TextEdit();
            this.labelInvBalance = new DevExpress.XtraEditors.LabelControl();
            this.textInvBalance = new DevExpress.XtraEditors.TextEdit();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.themedPanel)).BeginInit();
            this.themedPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelCenter.SuspendLayout();
            this.tableLayoutPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlParent)).BeginInit();
            this.tabControlParent.SuspendLayout();
            this.tabPageContact.SuspendLayout();
            this.panelDetailsTab.SuspendLayout();
            this.tableLayoutPanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxLastName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMiddleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxFirstName.Properties)).BeginInit();
            this.tableLayoutPanelType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCpfCnpjNumber.Properties)).BeginInit();
            this.tableLayoutPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxLanguage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCurrency.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxReceiptEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWebSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textIDNumber.Properties)).BeginInit();
            this.tabPageHistory.SuspendLayout();
            this.panelHistoryTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDateCreated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTotalVisits.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDateLastVisit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textStoreLastVisited.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTotalSales.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBalance.Properties)).BeginInit();
            this.layoutPanelInvoiceAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textInvAccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textInvAccountId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textInvBalance.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // themedPanel
            // 
            this.themedPanel.Controls.Add(this.tableLayoutPanel1);
            this.themedPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themedPanel.Location = new System.Drawing.Point(0, 0);
            this.themedPanel.Margin = new System.Windows.Forms.Padding(0);
            this.themedPanel.Name = "themedPanel";
            this.themedPanel.Size = new System.Drawing.Size(1024, 768);
            this.themedPanel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelCenter, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 10, 30, 0);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanelCenter
            // 
            this.tableLayoutPanelCenter.ColumnCount = 1;
            this.tableLayoutPanelCenter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCenter.Controls.Add(this.tableLayoutPanelBottom, 0, 1);
            this.tableLayoutPanelCenter.Controls.Add(this.tabControlParent, 0, 0);
            this.tableLayoutPanelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCenter.Location = new System.Drawing.Point(33, 78);
            this.tableLayoutPanelCenter.Name = "tableLayoutPanelCenter";
            this.tableLayoutPanelCenter.RowCount = 2;
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCenter.Size = new System.Drawing.Size(954, 683);
            this.tableLayoutPanelCenter.TabIndex = 1;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelBottom.AutoSize = true;
            this.tableLayoutPanelBottom.ColumnCount = 3;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnSave, 1, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnClear, 0, 0);
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(3, 607);
            this.tableLayoutPanelBottom.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(948, 73);
            this.tableLayoutPanelBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(549, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 3, 3, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 60);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "Validated", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(409, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 3, 5, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 60);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Customer.ViewModels.CustomerViewModel);
            this.bindingSource.CurrentChanged += new System.EventHandler(this.bindingSource_CurrentChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(269, 3);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 3, 5, 10);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(130, 60);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tabControlParent
            // 
            this.tabControlParent.AppearancePage.Header.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlParent.AppearancePage.Header.Options.UseFont = true;
            this.tabControlParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlParent.Location = new System.Drawing.Point(3, 3);
            this.tabControlParent.Name = "tabControlParent";
            this.tabControlParent.SelectedTabPage = this.tabPageContact;
            this.tabControlParent.Size = new System.Drawing.Size(948, 586);
            this.tabControlParent.TabIndex = 0;
            this.tabControlParent.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPageContact,
            this.tabPageHistory});
            this.tabControlParent.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabControlParent_SelectedPageChanged);
            // 
            // tabPageContact
            // 
            this.tabPageContact.Controls.Add(this.panelDetailsTab);
            this.tabPageContact.Name = "tabPageContact";
            this.tabPageContact.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageContact.Size = new System.Drawing.Size(942, 550);
            this.tabPageContact.Text = "Contact details";
            // 
            // panelDetailsTab
            // 
            this.panelDetailsTab.ColumnCount = 2;
            this.panelDetailsTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelDetailsTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelDetailsTab.Controls.Add(this.tableLayoutPanelLeft, 0, 0);
            this.panelDetailsTab.Controls.Add(this.tableLayoutPanelRight, 1, 0);
            this.panelDetailsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailsTab.Location = new System.Drawing.Point(10, 10);
            this.panelDetailsTab.Name = "panelDetailsTab";
            this.panelDetailsTab.RowCount = 1;
            this.panelDetailsTab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelDetailsTab.Size = new System.Drawing.Size(922, 530);
            this.panelDetailsTab.TabIndex = 0;
            // 
            // tableLayoutPanelLeft
            // 
            this.tableLayoutPanelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.ColumnCount = 3;
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanelLeft.Controls.Add(this.textBoxLastName, 0, 7);
            this.tableLayoutPanelLeft.Controls.Add(this.labelLastName, 0, 6);
            this.tableLayoutPanelLeft.Controls.Add(this.textBoxMiddleName, 0, 5);
            this.tableLayoutPanelLeft.Controls.Add(this.labelMiddleName, 0, 4);
            this.tableLayoutPanelLeft.Controls.Add(this.labelName, 0, 8);
            this.tableLayoutPanelLeft.Controls.Add(this.textBoxName, 0, 9);
            this.tableLayoutPanelLeft.Controls.Add(this.labelType, 0, 0);
            this.tableLayoutPanelLeft.Controls.Add(this.labelFirstName, 0, 2);
            this.tableLayoutPanelLeft.Controls.Add(this.textBoxFirstName, 0, 3);
            this.tableLayoutPanelLeft.Controls.Add(this.tableLayoutPanelType, 0, 1);
            this.tableLayoutPanelLeft.Controls.Add(this.labelPhone, 0, 10);
            this.tableLayoutPanelLeft.Controls.Add(this.textBoxPhone, 0, 11);
            this.tableLayoutPanelLeft.Controls.Add(this.viewAddressUserControl1, 0, 12);
            this.tableLayoutPanelLeft.Controls.Add(this.labelCpfCnpjNumber, 0, 13);
            this.tableLayoutPanelLeft.Controls.Add(this.textBoxCpfCnpjNumber, 0, 14);
            this.tableLayoutPanelLeft.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelLeft.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelLeft.Name = "tableLayoutPanelLeft";
            this.tableLayoutPanelLeft.RowCount = 23;
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLeft.Size = new System.Drawing.Size(455, 524);
            this.tableLayoutPanelLeft.TabIndex = 0;
            this.tableLayoutPanelLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanelLeft_Paint);
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.SetColumnSpan(this.textBoxLastName, 2);
            this.textBoxLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "LastName", true));
            this.textBoxLastName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "AreSplitNameFieldsVisible", true));
            this.textBoxLastName.Location = new System.Drawing.Point(3, 171);
            this.textBoxLastName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxLastName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxLastName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxLastName.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxLastName.Properties.Appearance.Options.UseFont = true;
            this.textBoxLastName.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxLastName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxLastName.Properties.MaxLength = 20;
            this.textBoxLastName.Size = new System.Drawing.Size(382, 24);
            this.textBoxLastName.TabIndex = 8;
            // 
            // labelLastName
            // 
            this.labelLastName.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.labelLastName, 2);
            this.labelLastName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "AreSplitNameFieldsVisible", true));
            this.labelLastName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastName.Location = new System.Drawing.Point(3, 149);
            this.labelLastName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(75, 17);
            this.labelLastName.TabIndex = 7;
            this.labelLastName.Text = "Last name:";
            // 
            // textBoxMiddleName
            // 
            this.textBoxMiddleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.SetColumnSpan(this.textBoxMiddleName, 2);
            this.textBoxMiddleName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "MiddleName", true));
            this.textBoxMiddleName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "AreSplitNameFieldsVisible", true));
            this.textBoxMiddleName.Location = new System.Drawing.Point(3, 120);
            this.textBoxMiddleName.Name = "textBoxMiddleName";
            this.textBoxMiddleName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxMiddleName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxMiddleName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxMiddleName.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxMiddleName.Properties.Appearance.Options.UseFont = true;
            this.textBoxMiddleName.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxMiddleName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxMiddleName.Properties.MaxLength = 20;
            this.textBoxMiddleName.Size = new System.Drawing.Size(382, 24);
            this.textBoxMiddleName.TabIndex = 6;
            // 
            // labelMiddleName
            // 
            this.labelMiddleName.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.labelMiddleName, 2);
            this.labelMiddleName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "AreSplitNameFieldsVisible", true));
            this.labelMiddleName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMiddleName.Location = new System.Drawing.Point(3, 100);
            this.labelMiddleName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelMiddleName.Name = "labelMiddleName";
            this.labelMiddleName.Size = new System.Drawing.Size(93, 17);
            this.labelMiddleName.TabIndex = 5;
            this.labelMiddleName.Text = "Middle name:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.labelName, 2);
            this.labelName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "IsNameVisible", true));
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(3, 203);
            this.labelName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(48, 17);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.SetColumnSpan(this.textBoxName, 2);
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true));
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "IsNameVisible", true));
            this.textBoxName.Location = new System.Drawing.Point(3, 225);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxName.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxName.Properties.Appearance.Options.UseFont = true;
            this.textBoxName.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxName.Properties.MaxLength = 100;
            this.textBoxName.Size = new System.Drawing.Size(382, 24);
            this.textBoxName.TabIndex = 10;
            // 
            // labelType
            // 
            this.labelType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelType.AutoSize = true;
            this.labelType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelType.Location = new System.Drawing.Point(3, 2);
            this.labelType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(41, 17);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type:";
            // 
            // labelFirstName
            // 
            this.labelFirstName.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.labelFirstName, 2);
            this.labelFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "AreSplitNameFieldsVisible", true));
            this.labelFirstName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFirstName.Location = new System.Drawing.Point(3, 51);
            this.labelFirstName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(77, 17);
            this.labelFirstName.TabIndex = 3;
            this.labelFirstName.Text = "First name:";
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.SetColumnSpan(this.textBoxFirstName, 2);
            this.textBoxFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FirstName", true));
            this.textBoxFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "AreSplitNameFieldsVisible", true));
            this.textBoxFirstName.Location = new System.Drawing.Point(3, 71);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxFirstName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxFirstName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxFirstName.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxFirstName.Properties.Appearance.Options.UseFont = true;
            this.textBoxFirstName.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxFirstName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxFirstName.Properties.MaxLength = 20;
            this.textBoxFirstName.Size = new System.Drawing.Size(382, 24);
            this.textBoxFirstName.TabIndex = 4;
            this.textBoxFirstName.EditValueChanged += new System.EventHandler(this.textBoxFirstName_EditValueChanged);
            // 
            // tableLayoutPanelType
            // 
            this.tableLayoutPanelType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelType.AutoSize = true;
            this.tableLayoutPanelType.ColumnCount = 2;
            this.tableLayoutPanelLeft.SetColumnSpan(this.tableLayoutPanelType, 2);
            this.tableLayoutPanelType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelType.Controls.Add(this.radioPerson, 0, 0);
            this.tableLayoutPanelType.Controls.Add(this.radioOrg, 1, 0);
            this.tableLayoutPanelType.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanelType.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanelType.Name = "tableLayoutPanelType";
            this.tableLayoutPanelType.RowCount = 1;
            this.tableLayoutPanelType.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelType.Size = new System.Drawing.Size(382, 27);
            this.tableLayoutPanelType.TabIndex = 2;
            // 
            // radioPerson
            // 
            this.radioPerson.AutoSize = true;
            this.radioPerson.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "IsPerson", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioPerson.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.radioPerson.Location = new System.Drawing.Point(3, 3);
            this.radioPerson.Name = "radioPerson";
            this.radioPerson.Size = new System.Drawing.Size(66, 21);
            this.radioPerson.TabIndex = 0;
            this.radioPerson.TabStop = true;
            this.radioPerson.Text = "Person";
            this.radioPerson.UseVisualStyleBackColor = true;
            // 
            // radioOrg
            // 
            this.radioOrg.AutoSize = true;
            this.radioOrg.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "IsOrganization", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioOrg.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.radioOrg.Location = new System.Drawing.Point(75, 3);
            this.radioOrg.Name = "radioOrg";
            this.radioOrg.Size = new System.Drawing.Size(101, 21);
            this.radioOrg.TabIndex = 1;
            this.radioOrg.TabStop = true;
            this.radioOrg.Text = "Organization";
            this.radioOrg.UseVisualStyleBackColor = true;
            this.radioOrg.Visible = false;
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.labelPhone, 2);
            this.labelPhone.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPhone.Location = new System.Drawing.Point(3, 257);
            this.labelPhone.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(104, 17);
            this.labelPhone.TabIndex = 11;
            this.labelPhone.Text = "Phone number:";
            // 
            // textBoxPhone
            // 
            this.textBoxPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.SetColumnSpan(this.textBoxPhone, 2);
            this.textBoxPhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Phone", true));
            this.textBoxPhone.Location = new System.Drawing.Point(3, 279);
            this.textBoxPhone.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxPhone.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxPhone.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxPhone.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxPhone.Properties.Appearance.Options.UseFont = true;
            this.textBoxPhone.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxPhone.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxPhone.Properties.MaxLength = 20;
            this.textBoxPhone.Size = new System.Drawing.Size(382, 24);
            this.textBoxPhone.TabIndex = 12;
            // 
            // viewAddressUserControl1
            // 
            this.viewAddressUserControl1.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.viewAddressUserControl1, 2);
            this.viewAddressUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewAddressUserControl1.Location = new System.Drawing.Point(3, 312);
            this.viewAddressUserControl1.MinimumSize = new System.Drawing.Size(285, 148);
            this.viewAddressUserControl1.Name = "viewAddressUserControl1";
            this.viewAddressUserControl1.Size = new System.Drawing.Size(382, 148);
            this.viewAddressUserControl1.TabIndex = 13;
            this.viewAddressUserControl1.Load += new System.EventHandler(this.viewAddressUserControl1_Load);
            // 
            // labelCpfCnpjNumber
            // 
            this.labelCpfCnpjNumber.AutoSize = true;
            this.tableLayoutPanelLeft.SetColumnSpan(this.labelCpfCnpjNumber, 2);
            this.labelCpfCnpjNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCpfCnpjNumber.Location = new System.Drawing.Point(3, 468);
            this.labelCpfCnpjNumber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelCpfCnpjNumber.Name = "labelCpfCnpjNumber";
            this.labelCpfCnpjNumber.Size = new System.Drawing.Size(61, 17);
            this.labelCpfCnpjNumber.TabIndex = 14;
            this.labelCpfCnpjNumber.Text = "CpfCnpj:";
            // 
            // textBoxCpfCnpjNumber
            // 
            this.textBoxCpfCnpjNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelLeft.SetColumnSpan(this.textBoxCpfCnpjNumber, 2);
            this.textBoxCpfCnpjNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "CNPJCPFNumber", true));
            this.textBoxCpfCnpjNumber.Location = new System.Drawing.Point(3, 488);
            this.textBoxCpfCnpjNumber.Name = "textBoxCpfCnpjNumber";
            this.textBoxCpfCnpjNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.textBoxCpfCnpjNumber.Properties.Appearance.Options.UseFont = true;
            this.textBoxCpfCnpjNumber.Properties.MaxLength = 255;
            this.textBoxCpfCnpjNumber.Size = new System.Drawing.Size(382, 24);
            this.textBoxCpfCnpjNumber.TabIndex = 0;
            // 
            // tableLayoutPanelRight
            // 
            this.tableLayoutPanelRight.ColumnCount = 3;
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.60212F));
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.39788F));
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRight.Controls.Add(this.label1, 0, 14);
            this.tableLayoutPanelRight.Controls.Add(this.btnAffiliations, 2, 13);
            this.tableLayoutPanelRight.Controls.Add(this.lblAffiliationCount, 0, 13);
            this.tableLayoutPanelRight.Controls.Add(this.labelAffiliations, 0, 12);
            this.tableLayoutPanelRight.Controls.Add(this.textBoxLanguage, 0, 11);
            this.tableLayoutPanelRight.Controls.Add(this.btnLanguage, 2, 11);
            this.tableLayoutPanelRight.Controls.Add(this.labelLanguage, 0, 10);
            this.tableLayoutPanelRight.Controls.Add(this.btnCurrency, 2, 9);
            this.tableLayoutPanelRight.Controls.Add(this.labelGroup, 0, 6);
            this.tableLayoutPanelRight.Controls.Add(this.textBoxCurrency, 0, 9);
            this.tableLayoutPanelRight.Controls.Add(this.labelReceiptEmail, 0, 2);
            this.tableLayoutPanelRight.Controls.Add(this.labelEmail, 0, 0);
            this.tableLayoutPanelRight.Controls.Add(this.labelCurrency, 0, 8);
            this.tableLayoutPanelRight.Controls.Add(this.labelWebSite, 0, 4);
            this.tableLayoutPanelRight.Controls.Add(this.btnGroup, 2, 7);
            this.tableLayoutPanelRight.Controls.Add(this.textBoxEmail, 0, 1);
            this.tableLayoutPanelRight.Controls.Add(this.textBoxGroup, 0, 7);
            this.tableLayoutPanelRight.Controls.Add(this.textBoxReceiptEmail, 0, 3);
            this.tableLayoutPanelRight.Controls.Add(this.textBoxWebSite, 0, 5);
            this.tableLayoutPanelRight.Controls.Add(this.textIDNumber, 0, 15);
            this.tableLayoutPanelRight.Controls.Add(this.label2, 0, 16);
            this.tableLayoutPanelRight.Controls.Add(this.cmbBoxGender, 0, 17);
            this.tableLayoutPanelRight.Controls.Add(this.label3, 0, 18);
            this.tableLayoutPanelRight.Controls.Add(this.label4, 0, 20);
            this.tableLayoutPanelRight.Controls.Add(this.dateBirth, 0, 19);
            this.tableLayoutPanelRight.Controls.Add(this.cmbBoxCustType, 0, 21);
            this.tableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRight.Location = new System.Drawing.Point(464, 3);
            this.tableLayoutPanelRight.Name = "tableLayoutPanelRight";
            this.tableLayoutPanelRight.RowCount = 24;
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.Size = new System.Drawing.Size(455, 524);
            this.tableLayoutPanelRight.TabIndex = 1;
            this.tableLayoutPanelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanelRight_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 345);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "ID Number:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnAffiliations
            // 
            this.btnAffiliations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAffiliations.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.Edit;
            this.btnAffiliations.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAffiliations.Location = new System.Drawing.Point(312, 313);
            this.btnAffiliations.Margin = new System.Windows.Forms.Padding(0);
            this.btnAffiliations.Name = "btnAffiliations";
            this.btnAffiliations.Padding = new System.Windows.Forms.Padding(3);
            this.btnAffiliations.Size = new System.Drawing.Size(143, 30);
            this.btnAffiliations.TabIndex = 17;
            this.btnAffiliations.Click += new System.EventHandler(this.OnAffiliations_Click);
            // 
            // lblAffiliationCount
            // 
            this.lblAffiliationCount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "AffiliationCount", true));
            this.lblAffiliationCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAffiliationCount.Location = new System.Drawing.Point(3, 313);
            this.lblAffiliationCount.MaximumSize = new System.Drawing.Size(228, 0);
            this.lblAffiliationCount.Name = "lblAffiliationCount";
            this.lblAffiliationCount.Size = new System.Drawing.Size(149, 30);
            this.lblAffiliationCount.TabIndex = 18;
            this.lblAffiliationCount.Text = "Count (0)";
            // 
            // labelAffiliations
            // 
            this.labelAffiliations.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelAffiliations, 3);
            this.labelAffiliations.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAffiliations.Location = new System.Drawing.Point(3, 296);
            this.labelAffiliations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelAffiliations.Name = "labelAffiliations";
            this.labelAffiliations.Size = new System.Drawing.Size(81, 17);
            this.labelAffiliations.TabIndex = 15;
            this.labelAffiliations.Text = "Affiliations:";
            // 
            // textBoxLanguage
            // 
            this.textBoxLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textBoxLanguage, 2);
            this.textBoxLanguage.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Language.DisplayName", true));
            this.textBoxLanguage.Location = new System.Drawing.Point(3, 267);
            this.textBoxLanguage.Name = "textBoxLanguage";
            this.textBoxLanguage.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxLanguage.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxLanguage.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxLanguage.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxLanguage.Properties.Appearance.Options.UseFont = true;
            this.textBoxLanguage.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxLanguage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxLanguage.Properties.ReadOnly = true;
            this.textBoxLanguage.Size = new System.Drawing.Size(306, 24);
            this.textBoxLanguage.TabIndex = 13;
            // 
            // btnLanguage
            // 
            this.btnLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLanguage.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.search;
            this.btnLanguage.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnLanguage.Location = new System.Drawing.Point(312, 264);
            this.btnLanguage.Margin = new System.Windows.Forms.Padding(0);
            this.btnLanguage.Name = "btnLanguage";
            this.btnLanguage.Padding = new System.Windows.Forms.Padding(3);
            this.btnLanguage.Size = new System.Drawing.Size(143, 30);
            this.btnLanguage.TabIndex = 14;
            this.btnLanguage.Click += new System.EventHandler(this.OnLanguage_Click);
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelLanguage, 3);
            this.labelLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLanguage.Location = new System.Drawing.Point(3, 247);
            this.labelLanguage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(72, 17);
            this.labelLanguage.TabIndex = 12;
            this.labelLanguage.Text = "Language:";
            // 
            // btnCurrency
            // 
            this.btnCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCurrency.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.search;
            this.btnCurrency.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCurrency.Location = new System.Drawing.Point(312, 215);
            this.btnCurrency.Margin = new System.Windows.Forms.Padding(0);
            this.btnCurrency.Name = "btnCurrency";
            this.btnCurrency.Padding = new System.Windows.Forms.Padding(3);
            this.btnCurrency.Size = new System.Drawing.Size(143, 30);
            this.btnCurrency.TabIndex = 11;
            this.btnCurrency.Click += new System.EventHandler(this.OnCurrency_Click);
            // 
            // labelGroup
            // 
            this.labelGroup.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelGroup, 3);
            this.labelGroup.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGroup.Location = new System.Drawing.Point(3, 149);
            this.labelGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(112, 17);
            this.labelGroup.TabIndex = 6;
            this.labelGroup.Text = "Customer group:";
            // 
            // textBoxCurrency
            // 
            this.textBoxCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textBoxCurrency, 2);
            this.textBoxCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Currency", true));
            this.textBoxCurrency.Location = new System.Drawing.Point(3, 218);
            this.textBoxCurrency.Name = "textBoxCurrency";
            this.textBoxCurrency.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxCurrency.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxCurrency.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxCurrency.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxCurrency.Properties.Appearance.Options.UseFont = true;
            this.textBoxCurrency.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxCurrency.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxCurrency.Properties.ReadOnly = true;
            this.textBoxCurrency.Size = new System.Drawing.Size(306, 24);
            this.textBoxCurrency.TabIndex = 10;
            // 
            // labelReceiptEmail
            // 
            this.labelReceiptEmail.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelReceiptEmail, 3);
            this.labelReceiptEmail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReceiptEmail.Location = new System.Drawing.Point(3, 51);
            this.labelReceiptEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelReceiptEmail.Name = "labelReceiptEmail";
            this.labelReceiptEmail.Size = new System.Drawing.Size(100, 17);
            this.labelReceiptEmail.TabIndex = 2;
            this.labelReceiptEmail.Text = "Receipt e-mail:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelEmail, 3);
            this.labelEmail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEmail.Location = new System.Drawing.Point(3, 2);
            this.labelEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(51, 17);
            this.labelEmail.TabIndex = 0;
            this.labelEmail.Text = "E-mail:";
            // 
            // labelCurrency
            // 
            this.labelCurrency.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelCurrency, 3);
            this.labelCurrency.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrency.Location = new System.Drawing.Point(3, 198);
            this.labelCurrency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(71, 17);
            this.labelCurrency.TabIndex = 9;
            this.labelCurrency.Text = "Currrency:";
            // 
            // labelWebSite
            // 
            this.labelWebSite.AutoSize = true;
            this.tableLayoutPanelRight.SetColumnSpan(this.labelWebSite, 3);
            this.labelWebSite.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWebSite.Location = new System.Drawing.Point(3, 100);
            this.labelWebSite.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelWebSite.Name = "labelWebSite";
            this.labelWebSite.Size = new System.Drawing.Size(66, 17);
            this.labelWebSite.TabIndex = 4;
            this.labelWebSite.Text = "Web site:";
            // 
            // btnGroup
            // 
            this.btnGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroup.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.search;
            this.btnGroup.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnGroup.Location = new System.Drawing.Point(312, 166);
            this.btnGroup.Margin = new System.Windows.Forms.Padding(0);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Padding = new System.Windows.Forms.Padding(3);
            this.btnGroup.Size = new System.Drawing.Size(143, 30);
            this.btnGroup.TabIndex = 8;
            this.btnGroup.Click += new System.EventHandler(this.OnCustomerGroup_Click);
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textBoxEmail, 2);
            this.textBoxEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Email", true));
            this.textBoxEmail.Location = new System.Drawing.Point(3, 22);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxEmail.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxEmail.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxEmail.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxEmail.Properties.Appearance.Options.UseFont = true;
            this.textBoxEmail.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxEmail.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxEmail.Properties.MaxLength = 80;
            this.textBoxEmail.Size = new System.Drawing.Size(306, 24);
            this.textBoxEmail.TabIndex = 1;
            // 
            // textBoxGroup
            // 
            this.textBoxGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textBoxGroup, 2);
            this.textBoxGroup.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "CustomerGroup", true));
            this.textBoxGroup.Location = new System.Drawing.Point(3, 169);
            this.textBoxGroup.Name = "textBoxGroup";
            this.textBoxGroup.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxGroup.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxGroup.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxGroup.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxGroup.Properties.Appearance.Options.UseFont = true;
            this.textBoxGroup.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxGroup.Properties.ReadOnly = true;
            this.textBoxGroup.Size = new System.Drawing.Size(306, 24);
            this.textBoxGroup.TabIndex = 7;
            // 
            // textBoxReceiptEmail
            // 
            this.textBoxReceiptEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textBoxReceiptEmail, 2);
            this.textBoxReceiptEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ReceiptEmail", true));
            this.textBoxReceiptEmail.Location = new System.Drawing.Point(3, 71);
            this.textBoxReceiptEmail.Name = "textBoxReceiptEmail";
            this.textBoxReceiptEmail.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxReceiptEmail.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxReceiptEmail.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxReceiptEmail.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxReceiptEmail.Properties.Appearance.Options.UseFont = true;
            this.textBoxReceiptEmail.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxReceiptEmail.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxReceiptEmail.Properties.MaxLength = 80;
            this.textBoxReceiptEmail.Size = new System.Drawing.Size(306, 24);
            this.textBoxReceiptEmail.TabIndex = 3;
            // 
            // textBoxWebSite
            // 
            this.textBoxWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textBoxWebSite, 2);
            this.textBoxWebSite.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "WebSite", true));
            this.textBoxWebSite.Location = new System.Drawing.Point(3, 120);
            this.textBoxWebSite.Name = "textBoxWebSite";
            this.textBoxWebSite.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxWebSite.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxWebSite.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxWebSite.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxWebSite.Properties.Appearance.Options.UseFont = true;
            this.textBoxWebSite.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxWebSite.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxWebSite.Properties.MaxLength = 255;
            this.textBoxWebSite.Size = new System.Drawing.Size(306, 24);
            this.textBoxWebSite.TabIndex = 5;
            // 
            // textIDNumber
            // 
            this.textIDNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRight.SetColumnSpan(this.textIDNumber, 2);
            this.textIDNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "IdentificationNumber", true));
            this.textIDNumber.Location = new System.Drawing.Point(3, 364);
            this.textIDNumber.Name = "textIDNumber";
            this.textIDNumber.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textIDNumber.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textIDNumber.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textIDNumber.Properties.Appearance.Options.UseBackColor = true;
            this.textIDNumber.Properties.Appearance.Options.UseFont = true;
            this.textIDNumber.Properties.Appearance.Options.UseForeColor = true;
            this.textIDNumber.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textIDNumber.Properties.MaxLength = 255;
            this.textIDNumber.Size = new System.Drawing.Size(306, 24);
            this.textIDNumber.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 389);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "Gender:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cmbBoxGender
            // 
            this.cmbBoxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxGender.FormattingEnabled = true;
            this.cmbBoxGender.Items.AddRange(new object[] {
            "Male",
            "Female",
            "Non-Specific"});
            this.cmbBoxGender.Location = new System.Drawing.Point(3, 410);
            this.cmbBoxGender.Name = "cmbBoxGender";
            this.cmbBoxGender.Size = new System.Drawing.Size(137, 21);
            this.cmbBoxGender.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 435);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 22;
            this.label3.Text = "BirthDate :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 479);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 17);
            this.label4.TabIndex = 23;
            this.label4.Text = "Customer Type :";
            // 
            // dateBirth
            // 
            this.dateBirth.CustomFormat = "dd.MM.yyyy";
            this.dateBirth.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "CPBirthDate", true));
            this.dateBirth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateBirth.Location = new System.Drawing.Point(3, 453);
            this.dateBirth.Name = "dateBirth";
            this.dateBirth.Size = new System.Drawing.Size(149, 21);
            this.dateBirth.TabIndex = 24;
            this.dateBirth.ValueChanged += new System.EventHandler(this.dateBirth_ValueChanged);
            // 
            // cmbBoxCustType
            // 
            this.cmbBoxCustType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxCustType.FormattingEnabled = true;
            this.cmbBoxCustType.Items.AddRange(new object[] {
            "",
            "Loyalty"});
            this.cmbBoxCustType.Location = new System.Drawing.Point(3, 499);
            this.cmbBoxCustType.Name = "cmbBoxCustType";
            this.cmbBoxCustType.Size = new System.Drawing.Size(137, 21);
            this.cmbBoxCustType.TabIndex = 25;
            // 
            // tabPageHistory
            // 
            this.tabPageHistory.Controls.Add(this.panelHistoryTab);
            this.tabPageHistory.Name = "tabPageHistory";
            this.tabPageHistory.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageHistory.Size = new System.Drawing.Size(942, 550);
            this.tabPageHistory.Text = "History";
            // 
            // panelHistoryTab
            // 
            this.panelHistoryTab.ColumnCount = 8;
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelHistoryTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panelHistoryTab.Controls.Add(this.labelDateCreated, 0, 0);
            this.panelHistoryTab.Controls.Add(this.labelTotalVisits, 0, 2);
            this.panelHistoryTab.Controls.Add(this.gridOrders, 2, 3);
            this.panelHistoryTab.Controls.Add(this.textDateCreated, 0, 1);
            this.panelHistoryTab.Controls.Add(this.textTotalVisits, 0, 3);
            this.panelHistoryTab.Controls.Add(this.btnPgDown, 7, 19);
            this.panelHistoryTab.Controls.Add(this.btnDown, 6, 19);
            this.panelHistoryTab.Controls.Add(this.labelDateLastVisit, 0, 4);
            this.panelHistoryTab.Controls.Add(this.textDateLastVisit, 0, 5);
            this.panelHistoryTab.Controls.Add(this.labelStoreLastVisited, 0, 6);
            this.panelHistoryTab.Controls.Add(this.textStoreLastVisited, 0, 7);
            this.panelHistoryTab.Controls.Add(this.labelTotalSales, 0, 8);
            this.panelHistoryTab.Controls.Add(this.labelSearch, 2, 0);
            this.panelHistoryTab.Controls.Add(this.textSearch, 2, 1);
            this.panelHistoryTab.Controls.Add(this.buttonSearch, 6, 1);
            this.panelHistoryTab.Controls.Add(this.buttonClear, 7, 1);
            this.panelHistoryTab.Controls.Add(this.btnUp, 3, 19);
            this.panelHistoryTab.Controls.Add(this.btnPgUp, 2, 19);
            this.panelHistoryTab.Controls.Add(this.textTotalSales, 0, 9);
            this.panelHistoryTab.Controls.Add(this.labelBalance, 0, 10);
            this.panelHistoryTab.Controls.Add(this.textBalance, 0, 11);
            this.panelHistoryTab.Controls.Add(this.layoutPanelInvoiceAccount, 0, 12);
            this.panelHistoryTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryTab.Location = new System.Drawing.Point(10, 10);
            this.panelHistoryTab.Name = "panelHistoryTab";
            this.panelHistoryTab.RowCount = 20;
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.panelHistoryTab.Size = new System.Drawing.Size(922, 530);
            this.panelHistoryTab.TabIndex = 0;
            // 
            // labelDateCreated
            // 
            this.labelDateCreated.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelDateCreated, 2);
            this.labelDateCreated.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDateCreated.Location = new System.Drawing.Point(3, 2);
            this.labelDateCreated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelDateCreated.Name = "labelDateCreated";
            this.labelDateCreated.Size = new System.Drawing.Size(90, 17);
            this.labelDateCreated.TabIndex = 9;
            this.labelDateCreated.Text = "Date created:";
            // 
            // labelTotalVisits
            // 
            this.labelTotalVisits.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelTotalVisits, 2);
            this.labelTotalVisits.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalVisits.Location = new System.Drawing.Point(3, 53);
            this.labelTotalVisits.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelTotalVisits.Name = "labelTotalVisits";
            this.labelTotalVisits.Size = new System.Drawing.Size(79, 17);
            this.labelTotalVisits.TabIndex = 11;
            this.labelTotalVisits.Text = "Total visits:";
            // 
            // gridOrders
            // 
            this.panelHistoryTab.SetColumnSpan(this.gridOrders, 6);
            this.gridOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOrders.Location = new System.Drawing.Point(197, 73);
            this.gridOrders.MainView = this.gridView;
            this.gridOrders.Name = "gridOrders";
            this.panelHistoryTab.SetRowSpan(this.gridOrders, 15);
            this.gridOrders.Size = new System.Drawing.Size(722, 386);
            this.gridOrders.TabIndex = 4;
            this.gridOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.columnDate,
            this.columnOrderNumber,
            this.columnOrderStatus,
            this.columnStore,
            this.columnItem,
            this.columnQuantity,
            this.columnAmount});
            this.gridView.GridControl = this.gridOrders;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsBehavior.ReadOnly = true;
            this.gridView.OptionsCustomization.AllowColumnMoving = false;
            this.gridView.OptionsCustomization.AllowGroup = false;
            this.gridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView.OptionsCustomization.AllowRowSizing = true;
            this.gridView.OptionsMenu.EnableColumnMenu = false;
            this.gridView.OptionsMenu.EnableFooterMenu = false;
            this.gridView.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView.OptionsNavigation.UseTabKey = false;
            this.gridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.OptionsView.ShowIndicator = false;
            // 
            // columnDate
            // 
            this.columnDate.Caption = "Date";
            this.columnDate.DisplayFormat.FormatString = "d";
            this.columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.columnDate.FieldName = "OrderDate";
            this.columnDate.Name = "columnDate";
            this.columnDate.Visible = true;
            this.columnDate.VisibleIndex = 0;
            // 
            // columnOrderNumber
            // 
            this.columnOrderNumber.Caption = "Order number";
            this.columnOrderNumber.FieldName = "OrderNumber";
            this.columnOrderNumber.Name = "columnOrderNumber";
            this.columnOrderNumber.Visible = true;
            this.columnOrderNumber.VisibleIndex = 1;
            // 
            // columnOrderStatus
            // 
            this.columnOrderStatus.AppearanceCell.Options.UseTextOptions = true;
            this.columnOrderStatus.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.columnOrderStatus.Caption = "Order status";
            this.columnOrderStatus.FieldName = "OrderStatus";
            this.columnOrderStatus.Name = "columnOrderStatus";
            this.columnOrderStatus.Visible = true;
            this.columnOrderStatus.VisibleIndex = 2;
            // 
            // columnStore
            // 
            this.columnStore.Caption = "Store";
            this.columnStore.FieldName = "StoreName";
            this.columnStore.Name = "columnStore";
            this.columnStore.Visible = true;
            this.columnStore.VisibleIndex = 3;
            // 
            // columnItem
            // 
            this.columnItem.Caption = "Item";
            this.columnItem.FieldName = "ItemName";
            this.columnItem.Name = "columnItem";
            this.columnItem.Visible = true;
            this.columnItem.VisibleIndex = 4;
            // 
            // columnQuantity
            // 
            this.columnQuantity.Caption = "Quantity";
            this.columnQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.columnQuantity.FieldName = "ItemQuantity";
            this.columnQuantity.Name = "columnQuantity";
            this.columnQuantity.Visible = true;
            this.columnQuantity.VisibleIndex = 5;
            // 
            // columnAmount
            // 
            this.columnAmount.Caption = "Amount";
            this.columnAmount.DisplayFormat.FormatString = "C";
            this.columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.columnAmount.FieldName = "ItemAmount";
            this.columnAmount.Name = "columnAmount";
            this.columnAmount.Visible = true;
            this.columnAmount.VisibleIndex = 6;
            // 
            // textDateCreated
            // 
            this.textDateCreated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textDateCreated.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "DateCreated", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "d"));
            this.textDateCreated.Location = new System.Drawing.Point(3, 26);
            this.textDateCreated.Name = "textDateCreated";
            this.textDateCreated.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textDateCreated.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textDateCreated.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textDateCreated.Properties.Appearance.Options.UseBackColor = true;
            this.textDateCreated.Properties.Appearance.Options.UseFont = true;
            this.textDateCreated.Properties.Appearance.Options.UseForeColor = true;
            this.textDateCreated.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textDateCreated.Properties.MaxLength = 20;
            this.textDateCreated.Properties.ReadOnly = true;
            this.textDateCreated.Size = new System.Drawing.Size(149, 22);
            this.textDateCreated.TabIndex = 10;
            // 
            // textTotalVisits
            // 
            this.textTotalVisits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTotalVisits.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "TotalVisitsCount", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.textTotalVisits.Location = new System.Drawing.Point(3, 73);
            this.textTotalVisits.Name = "textTotalVisits";
            this.textTotalVisits.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textTotalVisits.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textTotalVisits.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textTotalVisits.Properties.Appearance.Options.UseBackColor = true;
            this.textTotalVisits.Properties.Appearance.Options.UseFont = true;
            this.textTotalVisits.Properties.Appearance.Options.UseForeColor = true;
            this.textTotalVisits.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textTotalVisits.Properties.MaxLength = 20;
            this.textTotalVisits.Properties.ReadOnly = true;
            this.textTotalVisits.Size = new System.Drawing.Size(149, 22);
            this.textTotalVisits.TabIndex = 12;
            // 
            // btnPgDown
            // 
            this.btnPgDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgDown.Appearance.Options.UseFont = true;
            this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.bottom;
            this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgDown.Location = new System.Drawing.Point(853, 466);
            this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgDown.Name = "btnPgDown";
            this.btnPgDown.Size = new System.Drawing.Size(65, 61);
            this.btnPgDown.TabIndex = 8;
            this.btnPgDown.Tag = "";
            this.btnPgDown.Text = "Ê";
            this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Appearance.Options.UseFont = true;
            this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.down;
            this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(779, 466);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(65, 61);
            this.btnDown.TabIndex = 7;
            this.btnDown.Tag = "";
            this.btnDown.Text = "ò";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // labelDateLastVisit
            // 
            this.labelDateLastVisit.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelDateLastVisit, 2);
            this.labelDateLastVisit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDateLastVisit.Location = new System.Drawing.Point(3, 100);
            this.labelDateLastVisit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelDateLastVisit.Name = "labelDateLastVisit";
            this.labelDateLastVisit.Size = new System.Drawing.Size(114, 17);
            this.labelDateLastVisit.TabIndex = 13;
            this.labelDateLastVisit.Text = "Date of last visit:";
            // 
            // textDateLastVisit
            // 
            this.textDateLastVisit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDateLastVisit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "LastVisitedDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "d"));
            this.textDateLastVisit.Location = new System.Drawing.Point(3, 120);
            this.textDateLastVisit.Name = "textDateLastVisit";
            this.textDateLastVisit.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textDateLastVisit.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textDateLastVisit.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textDateLastVisit.Properties.Appearance.Options.UseBackColor = true;
            this.textDateLastVisit.Properties.Appearance.Options.UseFont = true;
            this.textDateLastVisit.Properties.Appearance.Options.UseForeColor = true;
            this.textDateLastVisit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textDateLastVisit.Properties.MaxLength = 20;
            this.textDateLastVisit.Properties.ReadOnly = true;
            this.textDateLastVisit.Size = new System.Drawing.Size(149, 22);
            this.textDateLastVisit.TabIndex = 14;
            // 
            // labelStoreLastVisited
            // 
            this.labelStoreLastVisited.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelStoreLastVisited, 2);
            this.labelStoreLastVisited.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStoreLastVisited.Location = new System.Drawing.Point(3, 147);
            this.labelStoreLastVisited.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelStoreLastVisited.Name = "labelStoreLastVisited";
            this.labelStoreLastVisited.Size = new System.Drawing.Size(115, 17);
            this.labelStoreLastVisited.TabIndex = 15;
            this.labelStoreLastVisited.Text = "Store last visited:";
            // 
            // textStoreLastVisited
            // 
            this.textStoreLastVisited.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textStoreLastVisited.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "LastVisitedStore", true));
            this.textStoreLastVisited.Location = new System.Drawing.Point(3, 167);
            this.textStoreLastVisited.Name = "textStoreLastVisited";
            this.textStoreLastVisited.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textStoreLastVisited.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textStoreLastVisited.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textStoreLastVisited.Properties.Appearance.Options.UseBackColor = true;
            this.textStoreLastVisited.Properties.Appearance.Options.UseFont = true;
            this.textStoreLastVisited.Properties.Appearance.Options.UseForeColor = true;
            this.textStoreLastVisited.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textStoreLastVisited.Properties.MaxLength = 20;
            this.textStoreLastVisited.Properties.ReadOnly = true;
            this.textStoreLastVisited.Size = new System.Drawing.Size(149, 22);
            this.textStoreLastVisited.TabIndex = 16;
            // 
            // labelTotalSales
            // 
            this.labelTotalSales.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelTotalSales, 2);
            this.labelTotalSales.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalSales.Location = new System.Drawing.Point(3, 194);
            this.labelTotalSales.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelTotalSales.Name = "labelTotalSales";
            this.labelTotalSales.Size = new System.Drawing.Size(77, 17);
            this.labelTotalSales.TabIndex = 17;
            this.labelTotalSales.Text = "Total sales:";
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelSearch, 2);
            this.labelSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSearch.Location = new System.Drawing.Point(197, 3);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(3);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(82, 17);
            this.labelSearch.TabIndex = 0;
            this.labelSearch.Text = "Sales orders";
            // 
            // textSearch
            // 
            this.textSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHistoryTab.SetColumnSpan(this.textSearch, 4);
            this.textSearch.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FirstName", true));
            this.textSearch.Location = new System.Drawing.Point(197, 34);
            this.textSearch.Name = "textSearch";
            this.textSearch.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textSearch.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textSearch.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textSearch.Properties.Appearance.Options.UseBackColor = true;
            this.textSearch.Properties.Appearance.Options.UseFont = true;
            this.textSearch.Properties.Appearance.Options.UseForeColor = true;
            this.textSearch.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textSearch.Properties.MaxLength = 20;
            this.panelHistoryTab.SetRowSpan(this.textSearch, 2);
            this.textSearch.Size = new System.Drawing.Size(575, 24);
            this.textSearch.TabIndex = 1;
            this.textSearch.Enter += new System.EventHandler(this.textSearch_Enter);
            this.textSearch.Leave += new System.EventHandler(this.textSearch_Leave);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSearch.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.search;
            this.buttonSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.buttonSearch.Location = new System.Drawing.Point(783, 31);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Padding = new System.Windows.Forms.Padding(3);
            this.panelHistoryTab.SetRowSpan(this.buttonSearch, 2);
            this.buttonSearch.Size = new System.Drawing.Size(57, 30);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonClear.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
            this.buttonClear.Appearance.Options.UseFont = true;
            this.buttonClear.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.remove;
            this.buttonClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.buttonClear.Location = new System.Drawing.Point(851, 31);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Padding = new System.Windows.Forms.Padding(3);
            this.panelHistoryTab.SetRowSpan(this.buttonClear, 2);
            this.buttonClear.Size = new System.Drawing.Size(57, 30);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "û";
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Appearance.Options.UseFont = true;
            this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.up;
            this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(271, 466);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(65, 61);
            this.btnUp.TabIndex = 6;
            this.btnUp.Tag = "";
            this.btnUp.Text = "ñ";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnPgUp
            // 
            this.btnPgUp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPgUp.Appearance.Options.UseFont = true;
            this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Customer.Properties.Resources.top;
            this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnPgUp.Location = new System.Drawing.Point(198, 466);
            this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPgUp.Name = "btnPgUp";
            this.btnPgUp.Size = new System.Drawing.Size(65, 61);
            this.btnPgUp.TabIndex = 5;
            this.btnPgUp.Tag = "";
            this.btnPgUp.Text = "Ç";
            this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
            // 
            // textTotalSales
            // 
            this.textTotalSales.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTotalSales.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "TotalSalesAmount", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.textTotalSales.Location = new System.Drawing.Point(3, 214);
            this.textTotalSales.Name = "textTotalSales";
            this.textTotalSales.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textTotalSales.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textTotalSales.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textTotalSales.Properties.Appearance.Options.UseBackColor = true;
            this.textTotalSales.Properties.Appearance.Options.UseFont = true;
            this.textTotalSales.Properties.Appearance.Options.UseForeColor = true;
            this.textTotalSales.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textTotalSales.Properties.MaxLength = 20;
            this.textTotalSales.Properties.ReadOnly = true;
            this.textTotalSales.Size = new System.Drawing.Size(149, 22);
            this.textTotalSales.TabIndex = 18;
            // 
            // labelBalance
            // 
            this.labelBalance.AutoSize = true;
            this.panelHistoryTab.SetColumnSpan(this.labelBalance, 2);
            this.labelBalance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBalance.Location = new System.Drawing.Point(3, 241);
            this.labelBalance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(59, 17);
            this.labelBalance.TabIndex = 19;
            this.labelBalance.Text = "Balance:";
            // 
            // textBalance
            // 
            this.textBalance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBalance.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Balance", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.textBalance.Location = new System.Drawing.Point(3, 261);
            this.textBalance.Name = "textBalance";
            this.textBalance.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBalance.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBalance.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBalance.Properties.Appearance.Options.UseBackColor = true;
            this.textBalance.Properties.Appearance.Options.UseFont = true;
            this.textBalance.Properties.Appearance.Options.UseForeColor = true;
            this.textBalance.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textBalance.Properties.MaxLength = 20;
            this.textBalance.Properties.ReadOnly = true;
            this.textBalance.Size = new System.Drawing.Size(149, 22);
            this.textBalance.TabIndex = 20;
            // 
            // layoutPanelInvoiceAccount
            // 
            this.layoutPanelInvoiceAccount.ColumnCount = 2;
            this.panelHistoryTab.SetColumnSpan(this.layoutPanelInvoiceAccount, 2);
            this.layoutPanelInvoiceAccount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutPanelInvoiceAccount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutPanelInvoiceAccount.Controls.Add(this.labelInvAccountId, 0, 2);
            this.layoutPanelInvoiceAccount.Controls.Add(this.labelInvAccountName, 0, 0);
            this.layoutPanelInvoiceAccount.Controls.Add(this.textInvAccountName, 0, 1);
            this.layoutPanelInvoiceAccount.Controls.Add(this.textInvAccountId, 0, 3);
            this.layoutPanelInvoiceAccount.Controls.Add(this.labelInvBalance, 0, 4);
            this.layoutPanelInvoiceAccount.Controls.Add(this.textInvBalance, 0, 5);
            this.layoutPanelInvoiceAccount.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "HasInvoiceAccount", true));
            this.layoutPanelInvoiceAccount.Location = new System.Drawing.Point(3, 289);
            this.layoutPanelInvoiceAccount.Name = "layoutPanelInvoiceAccount";
            this.layoutPanelInvoiceAccount.RowCount = 6;
            this.panelHistoryTab.SetRowSpan(this.layoutPanelInvoiceAccount, 6);
            this.layoutPanelInvoiceAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInvoiceAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInvoiceAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInvoiceAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInvoiceAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInvoiceAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInvoiceAccount.Size = new System.Drawing.Size(188, 152);
            this.layoutPanelInvoiceAccount.TabIndex = 21;
            // 
            // labelInvAccountId
            // 
            this.labelInvAccountId.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.layoutPanelInvoiceAccount.SetColumnSpan(this.labelInvAccountId, 2);
            this.labelInvAccountId.Location = new System.Drawing.Point(3, 53);
            this.labelInvAccountId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.labelInvAccountId.Name = "labelInvAccountId";
            this.labelInvAccountId.Size = new System.Drawing.Size(118, 17);
            this.labelInvAccountId.TabIndex = 2;
            this.labelInvAccountId.Text = "Invoice account ID:";
            // 
            // labelInvAccountName
            // 
            this.labelInvAccountName.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.layoutPanelInvoiceAccount.SetColumnSpan(this.labelInvAccountName, 2);
            this.labelInvAccountName.Location = new System.Drawing.Point(3, 3);
            this.labelInvAccountName.Name = "labelInvAccountName";
            this.labelInvAccountName.Size = new System.Drawing.Size(138, 17);
            this.labelInvAccountName.TabIndex = 0;
            this.labelInvAccountName.Text = "Invoice account name:";
            // 
            // textInvAccountName
            // 
            this.textInvAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textInvAccountName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "InvoiceName", true));
            this.textInvAccountName.Location = new System.Drawing.Point(3, 26);
            this.textInvAccountName.Name = "textInvAccountName";
            this.textInvAccountName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textInvAccountName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textInvAccountName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textInvAccountName.Properties.Appearance.Options.UseBackColor = true;
            this.textInvAccountName.Properties.Appearance.Options.UseFont = true;
            this.textInvAccountName.Properties.Appearance.Options.UseForeColor = true;
            this.textInvAccountName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textInvAccountName.Properties.ReadOnly = true;
            this.textInvAccountName.Size = new System.Drawing.Size(153, 22);
            this.textInvAccountName.TabIndex = 1;
            // 
            // textInvAccountId
            // 
            this.textInvAccountId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textInvAccountId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "InvoiceAccountId", true));
            this.textInvAccountId.Location = new System.Drawing.Point(3, 73);
            this.textInvAccountId.Name = "textInvAccountId";
            this.textInvAccountId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textInvAccountId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textInvAccountId.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textInvAccountId.Properties.Appearance.Options.UseBackColor = true;
            this.textInvAccountId.Properties.Appearance.Options.UseFont = true;
            this.textInvAccountId.Properties.Appearance.Options.UseForeColor = true;
            this.textInvAccountId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textInvAccountId.Properties.ReadOnly = true;
            this.textInvAccountId.Size = new System.Drawing.Size(153, 22);
            this.textInvAccountId.TabIndex = 3;
            // 
            // labelInvBalance
            // 
            this.labelInvBalance.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelInvBalance.Location = new System.Drawing.Point(3, 101);
            this.labelInvBalance.Name = "labelInvBalance";
            this.labelInvBalance.Size = new System.Drawing.Size(153, 17);
            this.labelInvBalance.TabIndex = 4;
            this.labelInvBalance.Text = "Invoice Account Balance:";
            // 
            // textInvBalance
            // 
            this.textInvBalance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textInvBalance.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "InvoiceBalance", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.textInvBalance.Location = new System.Drawing.Point(3, 124);
            this.textInvBalance.Name = "textInvBalance";
            this.textInvBalance.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textInvBalance.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textInvBalance.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textInvBalance.Properties.Appearance.Options.UseBackColor = true;
            this.textInvBalance.Properties.Appearance.Options.UseFont = true;
            this.textInvBalance.Properties.Appearance.Options.UseForeColor = true;
            this.textInvBalance.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textInvBalance.Properties.ReadOnly = true;
            this.textInvBalance.Size = new System.Drawing.Size(153, 22);
            this.textInvBalance.TabIndex = 5;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblHeader.Location = new System.Drawing.Point(328, 10);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(364, 65);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Customer details";
            // 
            // frmNewCustomer
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.themedPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmNewCustomer";
            this.Text = "Customer";
            this.Controls.SetChildIndex(this.themedPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.themedPanel)).EndInit();
            this.themedPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanelCenter.ResumeLayout(false);
            this.tableLayoutPanelCenter.PerformLayout();
            this.tableLayoutPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlParent)).EndInit();
            this.tabControlParent.ResumeLayout(false);
            this.tabPageContact.ResumeLayout(false);
            this.panelDetailsTab.ResumeLayout(false);
            this.tableLayoutPanelLeft.ResumeLayout(false);
            this.tableLayoutPanelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxLastName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMiddleName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxFirstName.Properties)).EndInit();
            this.tableLayoutPanelType.ResumeLayout(false);
            this.tableLayoutPanelType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCpfCnpjNumber.Properties)).EndInit();
            this.tableLayoutPanelRight.ResumeLayout(false);
            this.tableLayoutPanelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxLanguage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxCurrency.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxReceiptEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWebSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textIDNumber.Properties)).EndInit();
            this.tabPageHistory.ResumeLayout(false);
            this.panelHistoryTab.ResumeLayout(false);
            this.panelHistoryTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDateCreated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTotalVisits.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDateLastVisit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textStoreLastVisited.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTotalSales.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBalance.Properties)).EndInit();
            this.layoutPanelInvoiceAccount.ResumeLayout(false);
            this.layoutPanelInvoiceAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textInvAccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textInvAccountId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textInvBalance.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl themedPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCenter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLeft;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.Label labelCurrency;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelReceiptEmail;
        private System.Windows.Forms.Label labelWebSite;
		private System.Windows.Forms.Label labelCpfCnpjNumber;
        private System.Windows.Forms.Label labelAffiliations;
		private DevExpress.XtraEditors.TextEdit textBoxName;
        private DevExpress.XtraEditors.TextEdit textBoxGroup;
        private DevExpress.XtraEditors.TextEdit textBoxCurrency;
        private DevExpress.XtraEditors.TextEdit textBoxLanguage;
        private DevExpress.XtraEditors.TextEdit textBoxPhone;
        private DevExpress.XtraEditors.TextEdit textBoxEmail;
        private DevExpress.XtraEditors.TextEdit textBoxReceiptEmail;
        private DevExpress.XtraEditors.TextEdit textBoxWebSite;
        private DevExpress.XtraEditors.TextEdit textBoxCpfCnpjNumber;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnGroup;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCurrency;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnLanguage;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnAffiliations;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelType;
        private System.Windows.Forms.RadioButton radioPerson;
        private System.Windows.Forms.RadioButton radioOrg;
        private DevExpress.XtraEditors.TextEdit textBoxLastName;
        private System.Windows.Forms.Label labelLastName;
        private DevExpress.XtraEditors.TextEdit textBoxMiddleName;
        private System.Windows.Forms.Label labelMiddleName;
        private System.Windows.Forms.Label labelFirstName;
		private DevExpress.XtraEditors.TextEdit textBoxFirstName;
		private ViewAddressUserControl viewAddressUserControl1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSave;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClear;
        private DevExpress.XtraTab.XtraTabControl tabControlParent;
        private DevExpress.XtraTab.XtraTabPage tabPageContact;
        private System.Windows.Forms.TableLayoutPanel panelDetailsTab;
        private DevExpress.XtraTab.XtraTabPage tabPageHistory;
        private System.Windows.Forms.TableLayoutPanel panelHistoryTab;
        private System.Windows.Forms.Label labelDateCreated;
        private System.Windows.Forms.Label labelTotalVisits;
        private System.Windows.Forms.Label labelSearch;
        private DevExpress.XtraGrid.GridControl gridOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.TextEdit textDateCreated;
        private DevExpress.XtraEditors.TextEdit textTotalVisits;
        private DevExpress.XtraEditors.TextEdit textSearch;
        private DevExpress.XtraEditors.TextEdit textStoreLastVisited;
        private DevExpress.XtraEditors.TextEdit textDateLastVisit;
        private DevExpress.XtraEditors.TextEdit textTotalSales;
        private DevExpress.XtraEditors.TextEdit textBalance;
        private System.Windows.Forms.Label labelStoreLastVisited;
        private System.Windows.Forms.Label labelTotalSales;
        private System.Windows.Forms.Label labelBalance;
        private System.Windows.Forms.Label labelDateLastVisit;
        private DevExpress.XtraGrid.Columns.GridColumn columnDate;
        private DevExpress.XtraGrid.Columns.GridColumn columnOrderNumber;
        private DevExpress.XtraGrid.Columns.GridColumn columnOrderStatus;
        private DevExpress.XtraGrid.Columns.GridColumn columnStore;
        private DevExpress.XtraGrid.Columns.GridColumn columnItem;
        private DevExpress.XtraGrid.Columns.GridColumn columnQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn columnAmount;
        private DevExpress.XtraEditors.SimpleButton buttonSearch;
        private DevExpress.XtraEditors.SimpleButton buttonClear;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
        private DevExpress.XtraEditors.LabelControl labelInvAccountId;
        private System.Windows.Forms.TableLayoutPanel layoutPanelInvoiceAccount;
        private DevExpress.XtraEditors.LabelControl labelInvAccountName;
        private DevExpress.XtraEditors.TextEdit textInvAccountName;
        private DevExpress.XtraEditors.TextEdit textInvAccountId;
        private DevExpress.XtraEditors.LabelControl labelInvBalance;
        private DevExpress.XtraEditors.TextEdit textInvBalance;
        private System.Windows.Forms.Label lblAffiliationCount;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit textIDNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBoxGender;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateBirth;
        private System.Windows.Forms.ComboBox cmbBoxCustType;
    }
}