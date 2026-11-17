namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_PROMODISCNEW
{
    partial class CP_PromoDiscNew
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle headerStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle headerStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle numberStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();


            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlPageHolder = new System.Windows.Forms.Panel();
            this.pnlSearchHolder = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitMain = new System.Windows.Forms.SplitContainer();

            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblDiscType = new System.Windows.Forms.Label();
            this.cbDiscType = new System.Windows.Forms.ComboBox();
            this.pnlSubtotal = new System.Windows.Forms.Panel();
            this.lblSubtotalCaption = new System.Windows.Forms.Label();
            this.lblSubtotalValue = new System.Windows.Forms.Label();

            this.pnlJournal = new System.Windows.Forms.Panel();
            this.pnlJournalHead = new System.Windows.Forms.Panel();
            this.lblJournalTitle = new System.Windows.Forms.Label();
            this.lblJournalSub = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.pnlJournalSearchHolder = new System.Windows.Forms.Panel();
            this.txtJournalSearch = new System.Windows.Forms.TextBox();
            this.dataGridHeader = new System.Windows.Forms.DataGridView();
            this.colJPilih = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colJId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJMinPay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJCategory = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colJItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJMetode = new System.Windows.Forms.DataGridViewButtonColumn();

            this.pnlDetails = new System.Windows.Forms.Panel();
            this.pnlDetailsHead = new System.Windows.Forms.Panel();
            this.lblDetailsTitle = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridDetails = new System.Windows.Forms.DataGridView();
            this.colDId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDPct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDDeduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlItemFooter = new System.Windows.Forms.Panel();
            this.lblSelectedCount = new System.Windows.Forms.Label();
            this.flpItemActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveSelection = new System.Windows.Forms.Button();
            this.btnClearSelection = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlSubtotal.SuspendLayout();
            this.pnlJournal.SuspendLayout();
            this.pnlJournalHead.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.pnlDetailsHead.SuspendLayout();
            this.pnlItemFooter.SuspendLayout();
            this.flpItemActions.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitle
            //
            this.pnlTitle.BackColor = System.Drawing.Color.FromArgb(109, 145, 191);
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(1024, 40);
            //
            // lblTitle
            //
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTitle.Text = "Pengaturan Diskon";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlBottom
            //
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1024, 72);
            //
            // btnClose  (Anchor = None keeps it centred when the form resizes)
            //
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(172, 193, 213);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(20, 20, 50);
            this.btnClose.Location = new System.Drawing.Point(448, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(128, 56);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // tblMain  (fixed filter row + a splitter holding the two groups)
            //
            this.tblMain.BackColor = System.Drawing.Color.FromArgb(213, 222, 229);
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.pnlFilter, 0, 0);
            this.tblMain.Controls.Add(this.splitMain, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Name = "tblMain";
            this.tblMain.Padding = new System.Windows.Forms.Padding(8);
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // splitMain  (drag the bar to resize the journal / details grids;
            //             the start position is set to 34% in OnShown)
            //
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(172, 193, 213);   // splitter bar colour
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Margin = new System.Windows.Forms.Padding(0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitMain.Panel1.BackColor = System.Drawing.Color.FromArgb(213, 222, 229);
            this.splitMain.Panel1.Controls.Add(this.pnlJournal);
            this.splitMain.Panel1MinSize = 120;
            this.splitMain.Panel2.BackColor = System.Drawing.Color.FromArgb(213, 222, 229);
            this.splitMain.Panel2.Controls.Add(this.pnlDetails);
            this.splitMain.Panel2MinSize = 160;
            this.splitMain.Size = new System.Drawing.Size(1008, 560);
            this.splitMain.SplitterDistance = 190;
            this.splitMain.SplitterWidth = 8;
            this.splitMain.TabStop = false;
            //
            // pnlFilter
            //
            this.pnlFilter.Controls.Add(this.lblDiscType);
            this.pnlFilter.Controls.Add(this.cbDiscType);
            this.pnlFilter.Controls.Add(this.pnlSubtotal);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.pnlFilter.Name = "pnlFilter";
            //
            // lblDiscType
            //
            this.lblDiscType.AutoSize = true;
            this.lblDiscType.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDiscType.Location = new System.Drawing.Point(4, 16);
            this.lblDiscType.Name = "lblDiscType";
            this.lblDiscType.Text = "Jenis Diskon";
            //
            // cbDiscType
            //
            this.cbDiscType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiscType.Font = new System.Drawing.Font("Tahoma", 11F);
            this.cbDiscType.FormattingEnabled = true;
            this.cbDiscType.Location = new System.Drawing.Point(88, 10);
            this.cbDiscType.Name = "cbDiscType";
            this.cbDiscType.Size = new System.Drawing.Size(180, 26);
            this.cbDiscType.TabIndex = 0;
            this.cbDiscType.SelectedIndexChanged += new System.EventHandler(this.cbDiscType_SelectedIndexChanged);
            //
            // pnlSubtotal
            //
            this.pnlSubtotal.BackColor = System.Drawing.Color.White;
            this.pnlSubtotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSubtotal.Controls.Add(this.lblSubtotalValue);     // Fill first, then Left
            this.pnlSubtotal.Controls.Add(this.lblSubtotalCaption);
            this.pnlSubtotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSubtotal.Name = "pnlSubtotal";
            this.pnlSubtotal.Size = new System.Drawing.Size(260, 44);
            //
            // lblSubtotalCaption
            //
            this.lblSubtotalCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSubtotalCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblSubtotalCaption.Name = "lblSubtotalCaption";
            this.lblSubtotalCaption.Size = new System.Drawing.Size(150, 34);
            this.lblSubtotalCaption.Text = "Subtotal Keranjang";
            this.lblSubtotalCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblSubtotalValue
            //
            this.lblSubtotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtotalValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubtotalValue.ForeColor = System.Drawing.Color.FromArgb(0, 168, 168);
            this.lblSubtotalValue.Name = "lblSubtotalValue";
            this.lblSubtotalValue.Text = "Rp 0";
            this.lblSubtotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlJournal
            //
            this.pnlJournal.BackColor = System.Drawing.Color.FromArgb(228, 235, 241);
            this.pnlJournal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlJournal.Controls.Add(this.dataGridHeader);   // Fill first, then Top
            this.pnlJournal.Controls.Add(this.pnlJournalHead);
            this.pnlJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlJournal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.pnlJournal.Name = "pnlJournal";
            this.pnlJournal.Padding = new System.Windows.Forms.Padding(4);
            //
            // pnlJournalHead
            //
            this.pnlJournalHead.Controls.Add(this.pnlJournalSearchHolder);   // added first = docked last = left of the page badge
            this.pnlJournalHead.Controls.Add(this.pnlPageHolder);
            this.pnlJournalHead.Controls.Add(this.lblJournalSub);
            this.pnlJournalHead.Controls.Add(this.lblJournalTitle);
            this.pnlJournalHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlJournalHead.Name = "pnlJournalHead";
            this.pnlJournalHead.Size = new System.Drawing.Size(100, 46);
            //
            // lblJournalTitle
            //
            this.lblJournalTitle.AutoSize = true;
            this.lblJournalTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblJournalTitle.Location = new System.Drawing.Point(6, 4);
            this.lblJournalTitle.Name = "lblJournalTitle";
            this.lblJournalTitle.Text = "Jurnal Promo Pembayaran";
            //
            // lblJournalSub
            //
            this.lblJournalSub.AutoSize = true;
            this.lblJournalSub.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblJournalSub.ForeColor = System.Drawing.Color.DimGray;
            this.lblJournalSub.Location = new System.Drawing.Point(8, 26);
            this.lblJournalSub.Name = "lblJournalSub";
            this.lblJournalSub.Text = "Penawaran, periode, dan penerapan promo";
            //
            // pnlJournalSearchHolder
            //
            this.pnlJournalSearchHolder.Controls.Add(this.txtJournalSearch);
            this.pnlJournalSearchHolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlJournalSearchHolder.Name = "pnlJournalSearchHolder";
            this.pnlJournalSearchHolder.Size = new System.Drawing.Size(240, 46);
            //
            // txtJournalSearch
            //
            this.txtJournalSearch.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtJournalSearch.Location = new System.Drawing.Point(6, 8);
            this.txtJournalSearch.Name = "txtJournalSearch";
            this.txtJournalSearch.Size = new System.Drawing.Size(224, 22);
            this.txtJournalSearch.TabIndex = 1;
            //
            // pnlPageHolder
            //
            this.pnlPageHolder.Controls.Add(this.lblPage);
            this.pnlPageHolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPageHolder.Name = "pnlPageHolder";
            this.pnlPageHolder.Size = new System.Drawing.Size(44, 46);
            //
            // lblPage
            //
            this.lblPage.BackColor = System.Drawing.Color.FromArgb(109, 145, 191);
            this.lblPage.ForeColor = System.Drawing.Color.White;
            this.lblPage.Location = new System.Drawing.Point(6, 6);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(28, 22);
            this.lblPage.Text = "1";
            this.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // dataGridHeader
            //
            this.dataGridHeader.AllowUserToAddRows = false;
            this.dataGridHeader.AllowUserToDeleteRows = false;
            this.dataGridHeader.AllowUserToResizeRows = false;
            this.dataGridHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridHeader.BackgroundColor = System.Drawing.Color.White;
            this.dataGridHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle1.BackColor = System.Drawing.Color.FromArgb(109, 145, 191);
            headerStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            headerStyle1.ForeColor = System.Drawing.Color.White;
            headerStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(109, 145, 191);
            headerStyle1.SelectionForeColor = System.Drawing.Color.White;
            headerStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridHeader.ColumnHeadersDefaultCellStyle = headerStyle1;
            this.dataGridHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colJPilih, this.colJId, this.colJName, this.colJStart,
                this.colJEnd, this.colJMinPay, this.colJCategory, this.colJItem, this.colJMetode});
            cellStyle1.Font = new System.Drawing.Font("Tahoma", 11F);
            this.dataGridHeader.DefaultCellStyle = cellStyle1;
            this.dataGridHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridHeader.EnableHeadersVisualStyles = false;
            this.dataGridHeader.GridColor = System.Drawing.Color.FromArgb(226, 234, 253);
            this.dataGridHeader.MultiSelect = false;
            this.dataGridHeader.Name = "dataGridHeader";
            this.dataGridHeader.ReadOnly = true;
            this.dataGridHeader.RowHeadersVisible = false;
            this.dataGridHeader.RowTemplate.Height = 40;
            this.dataGridHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridHeader.TabIndex = 1;
            //
            // Journal columns
            //
            this.colJPilih.FillWeight = 9F;
            this.colJPilih.HeaderText = "Pilih";
            this.colJPilih.Name = "colJPilih";
            this.colJPilih.Text = "Pilih";
            this.colJPilih.UseColumnTextForButtonValue = true;
            this.colJId.FillWeight = 11F;
            this.colJId.HeaderText = "ID";
            this.colJId.Name = "colJId";
            this.colJName.FillWeight = 20F;
            this.colJName.HeaderText = "Nama Promo";
            this.colJName.Name = "colJName";
            this.colJStart.FillWeight = 12F;
            this.colJStart.HeaderText = "Mulai";
            this.colJStart.Name = "colJStart";
            this.colJEnd.FillWeight = 12F;
            this.colJEnd.HeaderText = "Berakhir";
            this.colJEnd.Name = "colJEnd";
            this.colJMinPay.FillWeight = 12F;
            this.colJMinPay.HeaderText = "Min. Bayar";
            this.colJMinPay.Name = "colJMinPay";
            this.colJCategory.FillWeight = 14F;
            this.colJCategory.HeaderText = "Kategori";
            this.colJCategory.Name = "colJCategory";
            this.colJCategory.Text = "Lihat Kategori";
            this.colJCategory.UseColumnTextForButtonValue = true;
            this.colJItem.FillWeight = 10F;
            this.colJItem.HeaderText = "Item";
            this.colJItem.Name = "colJItem";
            this.colJMetode.FillWeight = 16F;
            this.colJMetode.HeaderText = "Metode Bayar";
            this.colJMetode.Name = "colJMetode";
            this.colJMetode.Visible = false;   // shown only for Jenis Diskon = Item
            //
            // pnlDetails
            //
            this.pnlDetails.BackColor = System.Drawing.Color.FromArgb(228, 235, 241);
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.dataGridDetails);   // Fill first, then Top
            this.pnlDetails.Controls.Add(this.pnlDetailsHead);
            this.pnlDetails.Controls.Add(this.pnlItemFooter);     // then Bottom
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(4);
            //
            // pnlDetailsHead
            //
            this.pnlDetailsHead.Controls.Add(this.pnlSearchHolder);
            this.pnlDetailsHead.Controls.Add(this.lblDetailsTitle);
            this.pnlDetailsHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetailsHead.Name = "pnlDetailsHead";
            this.pnlDetailsHead.Size = new System.Drawing.Size(100, 40);
            //
            // lblDetailsTitle
            //
            this.lblDetailsTitle.AutoSize = true;
            this.lblDetailsTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetailsTitle.Location = new System.Drawing.Point(6, 10);
            this.lblDetailsTitle.Name = "lblDetailsTitle";
            this.lblDetailsTitle.Text = "Daftar Pembayaran";
            //
            // pnlSearchHolder
            //
            this.pnlSearchHolder.Controls.Add(this.txtSearch);
            this.pnlSearchHolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchHolder.Name = "pnlSearchHolder";
            this.pnlSearchHolder.Size = new System.Drawing.Size(240, 40);
            //
            // txtSearch
            //
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtSearch.Location = new System.Drawing.Point(6, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(224, 22);
            this.txtSearch.TabIndex = 2;
            //
            // pnlItemFooter  (item mode only: "n dipilih" + Hapus Pilihan / Simpan Pilihan)
            //
            this.pnlItemFooter.Controls.Add(this.lblSelectedCount);   // Fill first, then Right
            this.pnlItemFooter.Controls.Add(this.flpItemActions);
            this.pnlItemFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlItemFooter.Name = "pnlItemFooter";
            this.pnlItemFooter.Padding = new System.Windows.Forms.Padding(4, 6, 4, 2);
            this.pnlItemFooter.Size = new System.Drawing.Size(100, 46);
            this.pnlItemFooter.Visible = false;
            //
            // lblSelectedCount
            //
            this.lblSelectedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectedCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblSelectedCount.Name = "lblSelectedCount";
            this.lblSelectedCount.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblSelectedCount.Text = "0 dipilih";
            this.lblSelectedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // flpItemActions  (right-to-left: first control added sits at the far right)
            //
            this.flpItemActions.AutoSize = true;
            this.flpItemActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpItemActions.Controls.Add(this.btnSaveSelection);
            this.flpItemActions.Controls.Add(this.btnClearSelection);
            this.flpItemActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpItemActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpItemActions.Name = "flpItemActions";
            this.flpItemActions.WrapContents = false;
            //
            // btnSaveSelection
            //
            this.btnSaveSelection.BackColor = System.Drawing.Color.FromArgb(0, 184, 169);
            this.btnSaveSelection.FlatAppearance.BorderSize = 0;
            this.btnSaveSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSelection.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveSelection.ForeColor = System.Drawing.Color.White;
            this.btnSaveSelection.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSaveSelection.Name = "btnSaveSelection";
            this.btnSaveSelection.Size = new System.Drawing.Size(130, 34);
            this.btnSaveSelection.Text = "Simpan Pilihan";
            this.btnSaveSelection.UseVisualStyleBackColor = false;
            this.btnSaveSelection.Click += new System.EventHandler(this.btnSaveSelection_Click);
            //
            // btnClearSelection
            //
            this.btnClearSelection.BackColor = System.Drawing.Color.FromArgb(172, 193, 213);
            this.btnClearSelection.FlatAppearance.BorderSize = 0;
            this.btnClearSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSelection.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearSelection.ForeColor = System.Drawing.Color.FromArgb(20, 20, 50);
            this.btnClearSelection.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(130, 34);
            this.btnClearSelection.Text = "Hapus Pilihan";
            this.btnClearSelection.UseVisualStyleBackColor = false;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            //
            // dataGridDetails
            //
            this.dataGridDetails.AllowUserToAddRows = false;
            this.dataGridDetails.AllowUserToDeleteRows = false;
            this.dataGridDetails.AllowUserToResizeRows = false;
            this.dataGridDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridDetails.BackgroundColor = System.Drawing.Color.White;
            this.dataGridDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            headerStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle2.BackColor = System.Drawing.Color.FromArgb(109, 145, 191);
            headerStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            headerStyle2.ForeColor = System.Drawing.Color.White;
            headerStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(109, 145, 191);
            headerStyle2.SelectionForeColor = System.Drawing.Color.White;
            headerStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridDetails.ColumnHeadersDefaultCellStyle = headerStyle2;
            this.dataGridDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDId, this.colDMethod, this.colDPct, this.colDDeduct, this.colDMax, this.colDSelect});
            cellStyle2.Font = new System.Drawing.Font("Tahoma", 11F);
            this.dataGridDetails.DefaultCellStyle = cellStyle2;
            this.dataGridDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridDetails.EnableHeadersVisualStyles = false;
            this.dataGridDetails.GridColor = System.Drawing.Color.FromArgb(226, 234, 253);
            this.dataGridDetails.MultiSelect = false;
            this.dataGridDetails.Name = "dataGridDetails";
            this.dataGridDetails.RowHeadersVisible = false;
            this.dataGridDetails.RowTemplate.Height = 40;
            this.dataGridDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDetails.TabIndex = 3;
            //
            // Details columns (Diskon %, Potongan, Mak. Diskon are editable)
            //
            numberStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            numberStyle.Font = new System.Drawing.Font("Tahoma", 11F);
            this.colDId.FillWeight = 18F;
            this.colDId.HeaderText = "ID";
            this.colDId.Name = "colDId";
            this.colDId.ReadOnly = true;
            this.colDMethod.FillWeight = 24F;
            this.colDMethod.HeaderText = "Metode";
            this.colDMethod.Name = "colDMethod";
            this.colDMethod.ReadOnly = true;
            this.colDPct.DefaultCellStyle = numberStyle;
            this.colDPct.FillWeight = 14F;
            this.colDPct.HeaderText = "Diskon %";
            this.colDPct.Name = "colDPct";
            this.colDDeduct.DefaultCellStyle = numberStyle;
            this.colDDeduct.FillWeight = 14F;
            this.colDDeduct.HeaderText = "Potongan";
            this.colDDeduct.Name = "colDDeduct";
            this.colDMax.DefaultCellStyle = numberStyle;
            this.colDMax.FillWeight = 16F;
            this.colDMax.HeaderText = "Mak. Diskon";
            this.colDMax.Name = "colDMax";
            this.colDSelect.FillWeight = 14F;
            this.colDSelect.HeaderText = "Pilih";
            this.colDSelect.Name = "colDSelect";
            this.colDSelect.Text = "Pilih";
            this.colDSelect.UseColumnTextForButtonValue = true;
            //
            // CP_PromoDiscNew
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tblMain);    // Fill first, then Bottom, then Top
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTitle);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CP_PromoDiscNew";
            this.Text = "CP_PromoDiscNew";
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.Controls.SetChildIndex(this.tblMain, 0);   // Fill at front so Top/Bottom are docked first

            ((System.ComponentModel.ISupportInitialize)(this.dataGridHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetails)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlSubtotal.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlJournalHead.ResumeLayout(false);
            this.pnlJournalHead.PerformLayout();
            this.pnlJournal.ResumeLayout(false);
            this.pnlDetailsHead.ResumeLayout(false);
            this.pnlDetailsHead.PerformLayout();
            this.pnlItemFooter.ResumeLayout(false);
            this.flpItemActions.ResumeLayout(false);
            this.flpItemActions.PerformLayout();
            this.pnlDetails.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlPageHolder;
        private System.Windows.Forms.Panel pnlSearchHolder;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.SplitContainer splitMain;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblDiscType;
        private System.Windows.Forms.ComboBox cbDiscType;
        private System.Windows.Forms.Panel pnlSubtotal;
        private System.Windows.Forms.Label lblSubtotalCaption;
        private System.Windows.Forms.Label lblSubtotalValue;

        private System.Windows.Forms.Panel pnlJournal;
        private System.Windows.Forms.Panel pnlJournalHead;
        private System.Windows.Forms.Label lblJournalTitle;
        private System.Windows.Forms.Label lblJournalSub;
        private System.Windows.Forms.Panel pnlJournalSearchHolder;
        private System.Windows.Forms.TextBox txtJournalSearch;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.DataGridView dataGridHeader;
        private System.Windows.Forms.DataGridViewButtonColumn colJPilih;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJMinPay;
        private System.Windows.Forms.DataGridViewButtonColumn colJCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJItem;
        private System.Windows.Forms.DataGridViewButtonColumn colJMetode;

        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Panel pnlDetailsHead;
        private System.Windows.Forms.Label lblDetailsTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dataGridDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDPct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDDeduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDMax;
        private System.Windows.Forms.DataGridViewButtonColumn colDSelect;
        private System.Windows.Forms.Panel pnlItemFooter;
        private System.Windows.Forms.Label lblSelectedCount;
        private System.Windows.Forms.FlowLayoutPanel flpItemActions;
        private System.Windows.Forms.Button btnSaveSelection;
        private System.Windows.Forms.Button btnClearSelection;
    }
}