using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_PROMODISCNEW
{
    public partial class CP_PromoDiscNew : LSRetailPosis.POSControls.Touch.frmTouchBase
    {
        // ASSUMPTIONS - verify against your AX enums
        private const int DISCTYPE_PAYMENT = 2;   // CPPROMODISCOUNTHEADER.DISCOUNTTYPE for payment promos
        private const int DISCTYPE_ITEM = 1;      // CPPROMODISCOUNTHEADER.DISCOUNTTYPE for item promos
        private const int STATUS_ACTIVE = 1;      // CPPROMODISCOUNTHEADER.STATUS for enabled promos

        // ---- grid look: change these three numbers to resize text / rows ----
        private const float GridHeaderFontSize = 8.25F;   // 75% of the previous 11F
        //GridHeaderFontSize = 15.25F;  // column header titles (bold) 
        private const float GridDataFontSize = 11F;    // data cells, numeric columns and buttons
        private const int GridRowHeight = 48;             // px
        private const int GridHeaderHeight = 24;          // px, column header row (75% of the previous 32)
        private static readonly System.Drawing.Color GridHeaderBackColor = System.Drawing.Color.FromArgb(109, 145, 191);
        private static readonly System.Drawing.Color GridHeaderTextColor = System.Drawing.Color.White;

        // Item column of a payment promo: when the promo lists specific items, the cell is painted as this button
        private const string ItemButtonText = "Lihat Item";
        private const string CategoryButtonText = "Lihat Kategori";   // other value of CATSCOPE: "Semua kategori" = plain label

        private const int EM_SETCUEBANNER = 0x1501;
        private static readonly CultureInfo IdCulture = new CultureInfo("id-ID");

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private System.Drawing.Font _headerFont;
        private DataTable _journalTable;
        private DataTable _detailTable;
        private string _selectedPromoId;
        private bool _allSelected;
        private bool _loadingJournal;   // blocks SelectionChanged while the journal is being (re)bound
        private System.Windows.Forms.Timer _journalSearchTimer;   // 1 s typing delay for the journal search box
        private System.Windows.Forms.Timer _searchTimer;   // 1 s typing delay for the search box

        // Item-mode columns (built in code so the designer never has to serialize unattached columns)
        private DataGridViewCheckBoxColumn colIChk;
        private DataGridViewTextBoxColumn colIId, colIName, colIUnit, colICategory, colIPrice, colIDisc,
                                          colIPct, colIInclTax, colIMax, colIMaxStruk, colISold, colIQty;

        // Results, read by the caller after DialogResult.OK
        public int SelectedDiscType { get; private set; }
        public string SelectedPromoId { get; private set; }
        public string SelectedDetailId { get; private set; }
        public decimal SelectedDiscPercentage { get; private set; }
        public int SelectedDiscAmount { get; private set; }
        public int SelectedMaxValue { get; private set; }
        public DataTable SelectedItems { get; private set; }   // item mode: checked rows (ID, NAME, QTY, DISCPERCENTAGE, DISCAMOUNT, ...)

        // Passed in by the caller (the blank operation)
        private readonly IPosTransaction posTransaction;
        private readonly IApplication application;
        private readonly string initialPromoId;      // _promoId argument (not used yet)
        private readonly string callerOperationId;   // _operationId argument (not used yet)

        public CP_PromoDiscNew(IPosTransaction _posTransaction, IApplication _application, string _promoId, string _operationId)
        {
            InitializeComponent();

            posTransaction = _posTransaction;
            application = _application;
            initialPromoId = _promoId;
            callerOperationId = _operationId;

            lblSubtotalValue.Text = "Rp " + GetCartTotal().ToString("N0", IdCulture);
            SendMessage(txtSearch.Handle, EM_SETCUEBANNER, (IntPtr)1, "Cari pembayaran...");
            SendMessage(txtJournalSearch.Handle, EM_SETCUEBANNER, (IntPtr)1, "Cari promo...");

            BindColumns();
            CreateItemColumns();
            ApplyGridFonts();

            dataGridHeader.CellPainting += Grid_CellPainting;
            dataGridDetails.CellPainting += Grid_CellPainting;
            dataGridHeader.CellContentClick += dataGridHeader_CellContentClick;
            dataGridHeader.CellClick += dataGridHeader_CellClick;
            dataGridHeader.SelectionChanged += dataGridHeader_SelectionChanged;
            dataGridHeader.CellFormatting += dataGridHeader_CellFormatting;
            dataGridDetails.CellContentClick += dataGridDetails_CellContentClick;
            dataGridDetails.DataError += (s, e) => { e.ThrowException = false; e.Cancel = false; };
            dataGridDetails.CurrentCellDirtyStateChanged += dataGridDetails_CurrentCellDirtyStateChanged;
            dataGridDetails.CellValueChanged += dataGridDetails_CellValueChanged;
            dataGridDetails.ColumnHeaderMouseClick += dataGridDetails_ColumnHeaderMouseClick;
            _journalSearchTimer = new System.Windows.Forms.Timer(components);
            _journalSearchTimer.Interval = 1000;
            _journalSearchTimer.Tick += journalSearchTimer_Tick;
            txtJournalSearch.TextChanged += txtJournalSearch_TextChanged;

            _searchTimer = new System.Windows.Forms.Timer(components);
            _searchTimer.Interval = 1000;
            _searchTimer.Tick += searchTimer_Tick;
            txtSearch.TextChanged += txtSearch_TextChanged;

            cbDiscType.Items.AddRange(new object[] { "Pembayaran", "Item" });
            cbDiscType.SelectedIndex = 0;   // triggers cbDiscType_SelectedIndexChanged -> loads data
        }

        // ---------- setup ----------

        private void BindColumns()
        {
            dataGridHeader.AutoGenerateColumns = false;
            colJId.DataPropertyName = "PROMOID";
            colJName.DataPropertyName = "DISCNAME";
            colJStart.DataPropertyName = "VALIDFROM";
            colJEnd.DataPropertyName = "VALIDTO";
            colJMinPay.DataPropertyName = "MINIMUMAMOUNT";
            colJItem.DataPropertyName = "ITEMSCOPE";
            colJStart.DefaultCellStyle.Format = "yyyy-MM-dd";
            colJEnd.DefaultCellStyle.Format = "yyyy-MM-dd";
            colJPilih.UseColumnTextForButtonValue = false;   // text is set per row in CellFormatting
            colJPilih.FlatStyle = FlatStyle.Flat;            // so the teal "Dipilih" colour shows
            colJMetode.UseColumnTextForButtonValue = false;  // "Lihat Metode" / "Semua Metode" set per row in CellFormatting
            colJMetode.FlatStyle = FlatStyle.Flat;

            dataGridDetails.AutoGenerateColumns = false;
            colDId.DataPropertyName = "ID";
            colDMethod.DataPropertyName = "NAME";
            colDPct.DataPropertyName = "DISCPERCENTAGE";
            colDDeduct.DataPropertyName = "DISCAMOUNT";
            colDMax.DataPropertyName = "MAXVALUE";
            colDPct.DefaultCellStyle.Format = "0.##";
            colDPct.ReadOnly = true;      // payment discount values are display only
            colDDeduct.ReadOnly = true;
            colDMax.ReadOnly = true;
        }

        private DataGridViewTextBoxColumn NewTextColumn(string name, string header, string prop, float weight, bool numeric, bool readOnly)
        {
            DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
            c.Name = name;
            c.HeaderText = header;
            c.DataPropertyName = prop;
            c.FillWeight = weight;
            c.ReadOnly = readOnly;
            if (numeric) c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (numeric && readOnly) c.DefaultCellStyle.BackColor = Color.FromArgb(240, 243, 246);   // grey "locked" boxes
            return c;
        }

        // Columns of the "Daftar Item" grid (Jenis Diskon = Item)
        private void CreateItemColumns()
        {
            colIChk = new DataGridViewCheckBoxColumn();
            colIChk.Name = "colIChk";
            colIChk.HeaderText = string.Empty;          // header checkbox is painted in Grid_CellPainting
            colIChk.DataPropertyName = "SEL";
            colIChk.FillWeight = 4F;

            colIId = NewTextColumn("colIId", "ID", "ID", 9F, false, true);
            colIName = NewTextColumn("colIName", "Nama Item", "NAME", 18F, false, true);
            colIUnit = NewTextColumn("colIUnit", "Unit", "UNITID", 5F, false, true);
            colICategory = NewTextColumn("colICategory", "Kategori", "CATEGORYNAME", 12F, false, true);
            colIPrice = NewTextColumn("colIPrice", "Harga", "PRICE", 8F, true, true);
            colIPrice.DefaultCellStyle.Format = "N0";
            colIPrice.DefaultCellStyle.FormatProvider = IdCulture;
            colIDisc = NewTextColumn("colIDisc", "Potongan", "DISCAMOUNT", 9F, true, true);
            colIPct = NewTextColumn("colIPct", "Diskon %", "DISCPERCENTAGE", 8F, true, true);
            colIPct.DefaultCellStyle.Format = "0.##";
            colIInclTax = NewTextColumn("colIInclTax", "Disc Incl Tax", "DISCINCLTAX", 11F, true, true);
            colIMax = NewTextColumn("colIMax", "Max", "MAXQTY", 6F, true, true);
            colIMaxStruk = NewTextColumn("colIMaxStruk", "Max/Struk", "MAXQTYPERSTRUK", 8F, true, true);
            colISold = NewTextColumn("colISold", "Terjual", "SOLD", 7F, true, true);
            colIQty = NewTextColumn("colIQty", "Jumlah", "QTY", 8F, true, false);   // the only editable box
        }

        // Swap the details grid between the payment layout and the item layout.
        private void SetDetailColumns(bool isPayment)
        {
            dataGridDetails.DataSource = null;
            dataGridDetails.Columns.Clear();

            if (isPayment)
                dataGridDetails.Columns.AddRange(new DataGridViewColumn[] { colDId, colDMethod, colDPct, colDDeduct, colDMax, colDSelect });
            else
                dataGridDetails.Columns.AddRange(new DataGridViewColumn[] {
                    colIChk, colIId, colIName, colIUnit, colICategory, colIPrice,
                    colIDisc, colIPct, colIInclTax, colIMax, colIMaxStruk, colISold, colIQty });

            pnlItemFooter.Visible = !isPayment;   // "0 dipilih" + Hapus/Simpan Pilihan
            ApplyGridFonts();
        }

        // Fonts are applied in code (constructor + OnShown) so nothing in the base form
        // or the designer can override them.
        private void ApplyGridFonts()
        {
            foreach (DataGridView grid in new[] { dataGridHeader, dataGridDetails })
            {
                grid.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", GridDataFontSize);
                grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", GridHeaderFontSize, System.Drawing.FontStyle.Bold);
                grid.RowTemplate.Height = GridRowHeight;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid.ColumnHeadersHeight = GridHeaderHeight;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (DataGridViewColumn col in grid.Columns)
                    col.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", GridDataFontSize);
            }

            if (_headerFont != null) _headerFont.Dispose();
            _headerFont = new System.Drawing.Font("Tahoma", GridHeaderFontSize, System.Drawing.FontStyle.Bold);
            dataGridHeader.Invalidate();
            dataGridDetails.Invalidate();

            foreach (DataGridViewColumn col in new DataGridViewColumn[] { colDPct, colDDeduct, colDMax })
            {
                col.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", GridDataFontSize);
            }

            foreach (DataGridView grid in new[] { dataGridHeader, dataGridDetails })
            {
                foreach (DataGridViewRow row in grid.Rows) row.Height = GridRowHeight;
            }
        }

        // Column header titles are painted manually so the header font size can't be ignored.
        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // journal Kategori cell of a promo without category: plain label, not a button
            if (e.RowIndex >= 0 && sender == dataGridHeader && e.ColumnIndex == colJCategory.Index)
            {
                DataRowView catRow = dataGridHeader.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (catRow != null && Convert.ToString(catRow["CATSCOPE"]) != CategoryButtonText)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Border);
                    bool selected = (e.State & DataGridViewElementStates.Selected) != 0;
                    TextRenderer.DrawText(e.Graphics, Convert.ToString(catRow["CATSCOPE"]), e.CellStyle.Font,
                        new Rectangle(e.CellBounds.X + 3, e.CellBounds.Y, e.CellBounds.Width - 6, e.CellBounds.Height),
                        selected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                    e.Handled = true;
                    return;
                }
            }

            // journal Item cell of a promo with specific items: draw a button (a text cell cannot be a button)
            if (e.RowIndex >= 0 && sender == dataGridHeader && e.ColumnIndex == colJItem.Index
                && Convert.ToString(e.Value) == ItemButtonText)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Border);
                Rectangle btn = Rectangle.Inflate(e.CellBounds, -4, -4);
                e.Graphics.FillRectangle(Brushes.White, btn);
                using (Pen border = new Pen(Color.FromArgb(120, 140, 170)))
                    e.Graphics.DrawRectangle(border, btn.X, btn.Y, btn.Width - 1, btn.Height - 1);
                TextRenderer.DrawText(e.Graphics, ItemButtonText, e.CellStyle.Font, btn, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                e.Handled = true;
                return;
            }

            if (e.RowIndex != -1 || e.ColumnIndex < 0 || _headerFont == null) return;

            using (System.Drawing.SolidBrush back = new System.Drawing.SolidBrush(GridHeaderBackColor))
                e.Graphics.FillRectangle(back, e.CellBounds);

            using (System.Drawing.Pen line = new System.Drawing.Pen(System.Drawing.Color.FromArgb(226, 234, 253)))
                e.Graphics.DrawLine(line, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

            DataGridView grid = (DataGridView)sender;
            if (colIChk != null && grid == dataGridDetails && grid.Columns[e.ColumnIndex] == colIChk)
            {
                CheckBoxState state = _allSelected ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                Size glyph = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);
                CheckBoxRenderer.DrawCheckBox(e.Graphics,
                    new Point(e.CellBounds.X + (e.CellBounds.Width - glyph.Width) / 2,
                              e.CellBounds.Y + (e.CellBounds.Height - glyph.Height) / 2), state);
                e.Handled = true;
                return;
            }

            System.Drawing.Rectangle textRect = new System.Drawing.Rectangle(
                e.CellBounds.X + 6, e.CellBounds.Y, e.CellBounds.Width - 12, e.CellBounds.Height);

            TextRenderer.DrawText(e.Graphics, Convert.ToString(e.FormattedValue), _headerFont, textRect,
                GridHeaderTextColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            e.Handled = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ApplyGridFonts();

            // start with the journal at ~34% of the height; the user can drag the splitter from there
            splitMain.SplitterDistance = Math.Max(splitMain.Panel1MinSize,
                Math.Min((int)(splitMain.Height * 0.34), splitMain.Height - splitMain.Panel2MinSize - splitMain.SplitterWidth));
        }

        // ---------- data access ----------

        private DataTable Query(string sql, params SqlParameter[] parameters)
        {
            // The POS shares ONE local connection and other POS code (e.g. Rounding.CheckTableRounding)
            // calls Open() on it without checking, so it must be CLOSED again after every use.
            SqlConnection conn = ApplicationSettings.Database.LocalConnection;
            if (conn.State != ConnectionState.Open) conn.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                    return dt;
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed) conn.Close();   // always, like CPDiscountPayment
            }
        }

        private int CurrentDiscType
        {
            get { return cbDiscType.SelectedIndex == 1 ? DISCTYPE_ITEM : DISCTYPE_PAYMENT; }
        }

        private void LoadJournal()
        {
            const string sql = @"
SELECT h.PROMOID, h.DISCNAME, h.VALIDFROM, h.VALIDTO, h.MINIMUMAMOUNT,
       CASE WHEN EXISTS (SELECT 1 FROM ax.CPPROMODISCOUNTLINE l
                         WHERE l.PROMOID = h.PROMOID AND l.DATAAREAID = h.DATAAREAID
                           AND ISNULL(l.ITEMID, '') NOT IN ('', '0'))
            THEN 'Lihat Item'                -- must equal ItemButtonText: painted as a button
            WHEN EXISTS (SELECT 1 FROM ax.CPPROMODISCOUNTLINE l
                         WHERE l.PROMOID = h.PROMOID AND l.DATAAREAID = h.DATAAREAID
                           AND ISNULL(l.CATEGORY, 0) <> 0)
            THEN 'Sesuai kategori'
            ELSE 'Semua item' END AS ITEMSCOPE,
       CASE WHEN EXISTS (SELECT 1 FROM ax.CPPROMODISCOUNTLINE l
                         WHERE l.PROMOID = h.PROMOID AND l.DATAAREAID = h.DATAAREAID
                           AND ISNULL(l.CATEGORY, 0) <> 0)
            THEN 'Lihat Kategori'            -- must equal CategoryButtonText: real button
            ELSE 'Semua kategori' END AS CATSCOPE,
       CASE WHEN EXISTS (SELECT 1 FROM ax.CPPROMODISCOUNTPAYMENTMETHOD pm
                         WHERE pm.PROMOID = h.PROMOID AND pm.DATAAREAID = h.DATAAREAID)
            THEN 'Lihat Metode' ELSE 'Semua Metode' END AS PAYSCOPE
FROM ax.CPPROMODISCOUNTHEADER h
WHERE h.DATAAREAID   = @DATAAREAID
  AND h.DISCOUNTTYPE = @DISCOUNTTYPE
  AND h.STATUS       = @STATUS
  AND @TODAY BETWEEN h.VALIDFROM AND h.VALIDTO
ORDER BY h.VALIDFROM, h.PROMOID";

            _journalTable = Query(sql,
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
                new SqlParameter("@DISCOUNTTYPE", CurrentDiscType),
                new SqlParameter("@STATUS", STATUS_ACTIVE),
                new SqlParameter("@TODAY", SqlDbType.Date) { Value = DateTime.Today });

            txtJournalSearch.Text = string.Empty;
            _journalSearchTimer.Stop();   // a freshly loaded journal starts unfiltered
            _loadingJournal = true;
            _selectedPromoId = null;
            try { dataGridHeader.DataSource = _journalTable; }
            finally { _loadingJournal = false; }
            lblPage.Text = _journalTable.Rows.Count.ToString();   // total promos of the selected discount type

            // auto-select the first promo
            SelectPromo(_journalTable.Rows.Count > 0 ? Convert.ToString(_journalTable.Rows[0]["PROMOID"]) : null);
        }

        // Tender type name for a payment method. The name of the CURRENT channel wins
        // (RETAILCHANNELTABLE.RECID where INVENTLOCATION = this terminal's InventLocationId);
        // if that channel has no such tender type, the name from any other channel is used.
        // Used after "ax.CPPROMODISCOUNTPAYMENTMETHOD pm" in a query that has the @INVENTLOCATION parameter.
        private const string TenderNameApplySql = @"
OUTER APPLY (SELECT TOP 1 tt.NAME
             FROM ax.RETAILSTORETENDERTYPETABLE tt
             WHERE tt.TENDERTYPEID = pm.PAYMENTMETHODID
               AND tt.DATAAREAID = pm.DATAAREAID
             ORDER BY CASE WHEN tt.CHANNEL = (SELECT TOP 1 ch.RECID
                                              FROM ax.RETAILCHANNELTABLE ch
                                              WHERE ch.INVENTLOCATION = @INVENTLOCATION)
                           THEN 0 ELSE 1 END,
                      tt.CHANNEL) t
";

        private void LoadDetails()
        {
            txtSearch.Text = string.Empty;
            _searchTimer.Stop();   // clearing the box must not trigger a delayed search

            if (string.IsNullOrEmpty(_selectedPromoId))
            {
                dataGridDetails.DataSource = null;
                _detailTable = null;
                UpdateSelectedCount();
                return;
            }

            string sql;
            if (CurrentDiscType == DISCTYPE_PAYMENT)
            {
                // Method name = tender type name of this channel; falls back to the id if not found.
                sql = @"
SELECT pm.PAYMENTMETHODID AS ID,
       ISNULL(t.NAME, pm.PAYMENTMETHODID) AS NAME,
       pm.DISCPERCENTAGE, pm.DISCAMOUNT, pm.MAXDISCAMOUNT AS MAXVALUE
FROM ax.CPPROMODISCOUNTPAYMENTMETHOD pm
" + TenderNameApplySql + @"
WHERE pm.PROMOID = @PROMOID AND pm.DATAAREAID = @DATAAREAID
ORDER BY pm.PAYMENTMETHODID";
            }
            else
            {
                sql = @"
SELECT CAST(0 AS bit) AS SEL,
       l.ITEMID AS ID, ISNULL(pn.NAME, l.DESCRIPTION) AS NAME, l.UNITID,
       ISNULL(cat.NAME, '') AS CATEGORYNAME,
       CAST(0 AS decimal(18, 0)) AS PRICE,      -- TODO: Harga (price lookup not in these tables)
       l.DISCAMOUNT, l.DISCPERCENTAGE, l.DISCINCLTAX,
       l.MAXQTY, l.MAXQTYPERSTRUK,
       CAST(0 AS int) AS SOLD,                  -- TODO: Terjual (qty already sold under this promo)
       CAST(1 AS int) AS QTY
FROM ax.CPPROMODISCOUNTLINE l
OUTER APPLY (
    -- category-based line: use l.CATEGORY; item line: the item's category (first hierarchy)
    SELECT TOP 1 c.NAME
    FROM ax.ECORESCATEGORY c
    WHERE c.RECID = CASE WHEN ISNULL(l.CATEGORY, 0) <> 0 THEN l.CATEGORY
                         ELSE (SELECT TOP 1 pc.CATEGORY
                               FROM ax.ECORESPRODUCT p
                               JOIN ax.ECORESPRODUCTCATEGORY pc ON pc.PRODUCT = p.RECID
                               WHERE p.DISPLAYPRODUCTNUMBER = l.ITEMID
                               ORDER BY pc.CATEGORYHIERARCHY) END
) cat
OUTER APPLY (
    -- item name: ECORESPRODUCTTRANSLATION.PRODUCT = ECORESPRODUCT.RECID (current language first)
    SELECT TOP 1 t.NAME
    FROM ax.ECORESPRODUCT p
    JOIN ax.ECORESPRODUCTTRANSLATION t ON t.PRODUCT = p.RECID
    WHERE p.DISPLAYPRODUCTNUMBER = l.ITEMID
    ORDER BY CASE WHEN t.LANGUAGEID = @LANGUAGEID THEN 0 ELSE 1 END
) pn
WHERE l.PROMOID = @PROMOID AND l.DATAAREAID = @DATAAREAID
ORDER BY l.ITEMID";
            }

            _detailTable = Query(sql,
                new SqlParameter("@PROMOID", _selectedPromoId),
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
                new SqlParameter("@LANGUAGEID", CultureInfo.CurrentUICulture.Name),
                new SqlParameter("@INVENTLOCATION", ApplicationSettings.Terminal.InventLocationId ?? string.Empty));

            dataGridDetails.DataSource = _detailTable.DefaultView;
            UpdateSelectedCount();
        }

        // Cart total used for the subtotal label and for the minimum purchase check
        // (same value CPDiscountPayment compares with its minimum: NetAmountWithTax).
        private decimal GetCartTotal()
        {
            RetailTransaction transaction = posTransaction as RetailTransaction;
            return transaction == null ? 0m : transaction.NetAmountWithTax;
        }

        // Minimum purchase validation. Returns true when the cart total reaches the selected journal's
        // "Min. Bayar"; otherwise shows the "Pembayaran minimum belum terpenuhi" popup and returns false.
        private bool CheckMinimumPurchase()
        {
            decimal minimum = 0m;
            if (_journalTable != null)
            {
                foreach (DataRow r in _journalTable.Rows)
                {
                    if (Convert.ToString(r["PROMOID"]) == _selectedPromoId)
                    {
                        minimum = r["MINIMUMAMOUNT"] == DBNull.Value ? 0m : Convert.ToDecimal(r["MINIMUMAMOUNT"]);
                        break;
                    }
                }
            }

            decimal total = GetCartTotal();
            if (total >= minimum) return true;

            using (CP_PromoWarningPopup dlg = new CP_PromoWarningPopup(
                "Pembayaran minimum belum terpenuhi",
                "Keranjang tidak memenuhi pembayaran minimum untuk jurnal ini.",
                "Total keranjang " + total.ToString("N0", IdCulture)
                    + " kurang dari pembayaran minimum sebesar " + minimum.ToString("N0", IdCulture) + ".",
                "Kembali ke item"))
            {
                dlg.ShowDialog(this);
            }
            return false;
        }

        // Category validation (payment promos): at least one non-voided cart item must belong to a category
        // of the promo (CPPROMODISCOUNTLINE.CATEGORY) or to one of its sub-categories (ECORESCATEGORY.PARENTCATEGORY).
        // A promo without any category is valid for every item. Returns false after showing the warning popup.
        private bool CheckCartCategory()
        {
            List<string> categories = LoadCategories(_selectedPromoId);
            if (categories.Count == 0) return true;

            // distinct item ids in the cart
            List<string> itemIds = new List<string>();
            RetailTransaction transaction = posTransaction as RetailTransaction;
            if (transaction != null)
            {
                foreach (var line in transaction.SaleItems)
                {
                    if (!line.Voided && !string.IsNullOrEmpty(line.ItemId) && !itemIds.Contains(line.ItemId))
                        itemIds.Add(line.ItemId);
                }
            }

            bool matched = false;
            if (itemIds.Count > 0)
            {
                List<SqlParameter> prms = new List<SqlParameter>();
                prms.Add(new SqlParameter("@PROMOID", _selectedPromoId));
                prms.Add(new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID));

                string[] names = new string[itemIds.Count];
                for (int i = 0; i < itemIds.Count; i++)
                {
                    names[i] = "@I" + i;
                    prms.Add(new SqlParameter(names[i], itemIds[i]));
                }

                string sql = @"
WITH PromoCat AS (
    SELECT l.CATEGORY AS RECID
    FROM ax.CPPROMODISCOUNTLINE l
    WHERE l.PROMOID = @PROMOID AND l.DATAAREAID = @DATAAREAID AND ISNULL(l.CATEGORY, 0) <> 0
    UNION ALL
    SELECT c.RECID
    FROM ax.ECORESCATEGORY c
    JOIN PromoCat pcat ON c.PARENTCATEGORY = pcat.RECID
)
SELECT TOP 1 1 AS FOUND
FROM ax.ECORESPRODUCT p
JOIN ax.ECORESPRODUCTCATEGORY pc ON pc.PRODUCT = p.RECID
WHERE p.DISPLAYPRODUCTNUMBER IN (" + string.Join(", ", names) + @")
  AND pc.CATEGORY IN (SELECT RECID FROM PromoCat)";

                matched = Query(sql, prms.ToArray()).Rows.Count > 0;
            }

            if (matched) return true;

            using (CP_PromoWarningPopup dlg = new CP_PromoWarningPopup(
                "Kategori item belum terpenuhi",
                "Keranjang tidak memiliki item dari kategori yang berlaku untuk promo ini.",
                "Kategori item di cart belum memenuhi syarat diskon. Kategori yang berlaku: "
                    + string.Join(", ", categories.ToArray()) + ".",
                "Kembali ke item"))
            {
                dlg.ShowDialog(this);
            }
            return false;
        }

        private void SelectPromo(string promoId)
        {
            _selectedPromoId = promoId;
            dataGridHeader.Invalidate();
            LoadDetails();
        }

        // ---------- events ----------

        private void cbDiscType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isPayment = CurrentDiscType == DISCTYPE_PAYMENT;
            lblJournalTitle.Text = isPayment ? "Jurnal Promo Pembayaran" : "Jurnal Promo Item";
            lblDetailsTitle.Text = isPayment ? "Daftar Pembayaran" : "Daftar Item";
            lblJournalSub.Text = isPayment ? "Penawaran, periode, dan penerapan promo" : "Penawaran, periode, dan metode pembayaran";
            colJCategory.Visible = isPayment;     // payment promos: Kategori + Item
            colJItem.Visible = isPayment;
            colJMetode.Visible = !isPayment;      // item promos: Metode Bayar
            SetDetailColumns(isPayment);
            SendMessage(txtSearch.Handle, EM_SETCUEBANNER, (IntPtr)1, isPayment ? "Cari pembayaran..." : "Cari item...");

            LoadJournal();
        }

        // Item cell button: a text cell raises CellClick, not CellContentClick
        private void dataGridHeader_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colJItem.Index) return;
            DataRowView row = dataGridHeader.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null || Convert.ToString(row["ITEMSCOPE"]) != ItemButtonText) return;

            using (CP_PromoCategoryPopup dlg = new CP_PromoCategoryPopup(
                "Item Dibutuhkan", Convert.ToString(row["DISCNAME"]), LoadRequiredItems(Convert.ToString(row["PROMOID"]))))
            {
                dlg.ShowDialog(this);
            }
        }

        // Items listed on the promo lines (CPPROMODISCOUNTLINE.ITEMID), as "ITEMID - name"
        private List<string> LoadRequiredItems(string promoId)
        {
            DataTable dt = Query(@"
SELECT DISTINCT l.ITEMID, ISNULL(pn.NAME, ISNULL(l.DESCRIPTION, '')) AS NAME
FROM ax.CPPROMODISCOUNTLINE l
OUTER APPLY (
    SELECT TOP 1 t.NAME
    FROM ax.ECORESPRODUCT p
    JOIN ax.ECORESPRODUCTTRANSLATION t ON t.PRODUCT = p.RECID
    WHERE p.DISPLAYPRODUCTNUMBER = l.ITEMID
    ORDER BY CASE WHEN t.LANGUAGEID = @LANGUAGEID THEN 0 ELSE 1 END
) pn
WHERE l.PROMOID = @PROMOID AND l.DATAAREAID = @DATAAREAID AND ISNULL(l.ITEMID, '') NOT IN ('', '0')
ORDER BY l.ITEMID",
                new SqlParameter("@PROMOID", promoId),
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
                new SqlParameter("@LANGUAGEID", CultureInfo.CurrentUICulture.Name));

            List<string> items = new List<string>();
            foreach (DataRow r in dt.Rows)
            {
                string name = Convert.ToString(r["NAME"]);
                items.Add(name.Length == 0 ? Convert.ToString(r["ITEMID"]) : Convert.ToString(r["ITEMID"]) + " - " + name);
            }
            return items;
        }

        private void dataGridHeader_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataRowView row = dataGridHeader.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null) return;

            if (e.ColumnIndex == colJPilih.Index)
            {
                bool selected = Convert.ToString(row["PROMOID"]) == _selectedPromoId;
                e.Value = selected ? "Dipilih" : "Pilih";
                e.CellStyle.BackColor = selected ? System.Drawing.Color.FromArgb(0, 184, 169) : System.Drawing.SystemColors.Control;
                e.CellStyle.ForeColor = selected ? System.Drawing.Color.White : System.Drawing.SystemColors.ControlText;
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == colJMetode.Index)
            {
                e.Value = Convert.ToString(row["PAYSCOPE"]);
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == colJMinPay.Index && e.Value != null && e.Value != DBNull.Value)
            {
                e.Value = "Rp " + Convert.ToDecimal(e.Value).ToString("N0", IdCulture);
                e.FormattingApplied = true;
            }
        }

        // Selecting a journal row (mouse or keyboard) loads its details automatically.
        private void dataGridHeader_SelectionChanged(object sender, EventArgs e)
        {
            if (_loadingJournal || dataGridHeader.CurrentRow == null) return;

            DataRowView row = dataGridHeader.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return;

            string promoId = Convert.ToString(row["PROMOID"]);
            if (promoId != _selectedPromoId)
                SelectPromo(promoId);
        }

        private void dataGridHeader_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataRowView row = dataGridHeader.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null) return;

            // Pilih/Dipilih is only an indicator now: clicking the row (anywhere) selects the promo,
            // see dataGridHeader_SelectionChanged.
            if (e.ColumnIndex == colJMetode.Index)
            {
                ShowPaymentMethods(Convert.ToString(row["PROMOID"]));
            }
            else if (e.ColumnIndex == colJCategory.Index && Convert.ToString(row["CATSCOPE"]) == CategoryButtonText)
            {
                using (CP_PromoCategoryPopup dlg = new CP_PromoCategoryPopup(
                    Convert.ToString(row["DISCNAME"]), LoadCategories(Convert.ToString(row["PROMOID"]))))
                {
                    dlg.ShowDialog(this);
                }
            }
        }

        private void dataGridDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colDSelect.Index) return;

            dataGridDetails.EndEdit();
            DataRowView row = dataGridDetails.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null) return;

            if (!CheckMinimumPurchase()) return;   // cart below the promo's minimum: show the warning, stay here
            if (!CheckCartCategory()) return;      // no cart item in the promo's categories: show the warning, stay here

            SelectedDiscType = CurrentDiscType;
            SelectedPromoId = _selectedPromoId;
            SelectedDetailId = Convert.ToString(row["ID"]);
            SelectedDiscPercentage = row["DISCPERCENTAGE"] == DBNull.Value ? 0m : Convert.ToDecimal(row["DISCPERCENTAGE"]);
            SelectedDiscAmount = row["DISCAMOUNT"] == DBNull.Value ? 0 : Convert.ToInt32(row["DISCAMOUNT"]);
            SelectedMaxValue = row["MAXVALUE"] == DBNull.Value ? 0 : Convert.ToInt32(row["MAXVALUE"]);

            // PAYMENTMETHODID of CPPROMODISCOUNTPAYMENTMETHOD = the tender id for "pay customer account"
            string tenderId = SelectedDetailId;

            // customer account mapped to this tender (CPCUSTTENDERMAPTABLE); no mapping = nothing to pay to
            string custAccount = LoadMappedCustomerAccount(tenderId);
            if (string.IsNullOrEmpty(custAccount))
            {
                using (CP_PromoWarningPopup dlg = new CP_PromoWarningPopup(
                    "Customer belum dipetakan",
                    "Metode pembayaran ini belum memiliki customer account.",
                    "Metode pembayaran " + tenderId + " tidak ada di pemetaan customer (CPCUSTTENDERMAPTABLE).",
                    "Kembali"))
                {
                    dlg.ShowDialog(this);
                }
                return;
            }

            DialogResult = DialogResult.OK;
            Close();   // close first (same order as CPDiscountPayment), then run the operation

            try
            {
                AddMappedCustomerToTransaction(custAccount);

                RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                int tenderCountBefore = retailTransaction != null ? retailTransaction.TenderLines.Count : 0;

                application.RunOperation(PosisOperations.PayCustomerAccount, tenderId, posTransaction);

                // No tender line was added = the cashier cancelled the payment.
                // A promo payment must not stay open, so void the whole transaction (back to the start screen).
                if (retailTransaction != null && retailTransaction.TenderLines.Count == tenderCountBefore)
                    application.RunOperation(PosisOperations.VoidTransaction, "", posTransaction);
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
        }

        // Categories shown by the "Lihat Kategori" popup:
        // CPPROMODISCOUNTLINE.CATEGORY -> ECORESCATEGORY.RECID, shown as ECORESCATEGORY.NAME
        private List<string> LoadCategories(string promoId)
        {
            DataTable dt = Query(@"
SELECT DISTINCT c.NAME
FROM ax.CPPROMODISCOUNTLINE l
JOIN ax.ECORESCATEGORY c ON c.RECID = l.CATEGORY
WHERE l.PROMOID = @PROMOID
  AND l.DATAAREAID = @DATAAREAID
  AND ISNULL(l.CATEGORY, 0) <> 0
ORDER BY c.NAME",
                new SqlParameter("@PROMOID", promoId),
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID));

            List<string> names = new List<string>();
            foreach (DataRow r in dt.Rows) names.Add(Convert.ToString(r["NAME"]));
            return names;
        }

        // ---------- journal search (same idea as the details search) ----------

        private void txtJournalSearch_TextChanged(object sender, EventArgs e)
        {
            _journalSearchTimer.Stop();
            _journalSearchTimer.Start();
        }

        private void journalSearchTimer_Tick(object sender, EventArgs e)
        {
            _journalSearchTimer.Stop();
            ApplyJournalFilter();
        }

        // Filters the journal rows by promo id / promo name containing the text.
        private void ApplyJournalFilter()
        {
            if (_journalTable == null) return;

            string text = txtJournalSearch.Text.Trim();
            if (text.Length == 0)
            {
                _journalTable.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                text = text.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
                _journalTable.DefaultView.RowFilter =
                    string.Format("PROMOID LIKE '%{0}%' OR DISCNAME LIKE '%{0}%'", text);
            }

            if (dataGridHeader.Rows.Count == 0)
            {
                // nothing matches: no promo selected, so clear the details grid
                _selectedPromoId = null;
                dataGridDetails.DataSource = null;
                _detailTable = null;
                UpdateSelectedCount();
            }
            // otherwise the grid's selection changes and dataGridHeader_SelectionChanged loads the details
        }

        // Customer account mapped to a tender type: CPCUSTTENDERMAPTABLE (TENDERTYPEID -> CUSTACCOUNT)
        private string LoadMappedCustomerAccount(string tenderId)
        {
            DataTable dt = Query(@"
SELECT TOP 1 CUSTACCOUNT
FROM ax.CPCUSTTENDERMAPTABLE
WHERE TENDERTYPEID = @TENDERTYPEID AND DATAAREAID = @DATAAREAID",
                new SqlParameter("@TENDERTYPEID", tenderId),
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID));

            return dt.Rows.Count == 0 ? null : Convert.ToString(dt.Rows[0]["CUSTACCOUNT"]);
        }

        // Puts the customer on the current transaction (skipped when it is already the transaction's customer)
        private void AddMappedCustomerToTransaction(string custAccount)
        {
            RetailTransaction transaction = posTransaction as RetailTransaction;
            if (transaction != null && transaction.Customer != null && transaction.Customer.CustomerId == custAccount)
                return;

            var customer = application.BusinessLogic.CustomerSystem.GetCustomerInfo(custAccount);
            if (customer == null)
                throw new InvalidOperationException("Customer " + custAccount + " tidak ditemukan.");

            application.Services.Customer.AddCustomerToTransaction(customer, posTransaction);
        }

        // ---------- item list: checkbox selection ----------

        private void dataGridDetails_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // commit the checkbox immediately so "n dipilih" updates on click
            if (dataGridDetails.IsCurrentCellDirty && dataGridDetails.CurrentCell != null
                && dataGridDetails.CurrentCell.OwningColumn == colIChk)
                dataGridDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridDetails.Columns[e.ColumnIndex] == colIChk)
                UpdateSelectedCount();
        }

        private void dataGridDetails_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridDetails.Columns[e.ColumnIndex] == colIChk)
                ToggleAll(!_allSelected);
        }

        private void ToggleAll(bool value)
        {
            dataGridDetails.EndEdit();
            if (_detailTable == null || !_detailTable.Columns.Contains("SEL")) return;

            DataView view = _detailTable.DefaultView;          // respects the search filter
            DataRowView[] rows = new DataRowView[view.Count];
            view.CopyTo(rows, 0);
            foreach (DataRowView drv in rows) drv["SEL"] = value;

            UpdateSelectedCount();
        }

        private void UpdateSelectedCount()
        {
            int count = 0;
            bool all = false;

            if (_detailTable != null && _detailTable.Columns.Contains("SEL"))
            {
                count = _detailTable.Select("SEL = true").Length;

                DataView view = _detailTable.DefaultView;
                all = view.Count > 0;
                foreach (DataRowView drv in view)
                {
                    if (!(drv["SEL"] is bool) || !(bool)drv["SEL"]) { all = false; break; }
                }
            }

            _allSelected = all;
            lblSelectedCount.Text = count + " dipilih";
            dataGridDetails.Invalidate();   // repaints the header checkbox
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            ToggleAll(false);
        }

        private void btnSaveSelection_Click(object sender, EventArgs e)
        {
            dataGridDetails.EndEdit();
            if (_detailTable == null || !_detailTable.Columns.Contains("SEL")) return;

            DataTable selected = _detailTable.Clone();
            foreach (DataRow r in _detailTable.Select("SEL = true")) selected.ImportRow(r);

            if (selected.Rows.Count == 0)
            {
                MessageBox.Show("Belum ada item yang dipilih.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!CheckMinimumPurchase()) return;

            SelectedDiscType = CurrentDiscType;
            SelectedPromoId = _selectedPromoId;
            SelectedItems = selected;

            // TODO: apply the discount for each row of SelectedItems (QTY = Jumlah) to the cart,
            // or let the caller do it using SelectedItems.
            DialogResult = DialogResult.OK;
            Close();
        }

        // "Lihat Metode" button of an item promo: list the payment methods it is limited to
        private void ShowPaymentMethods(string promoId)
        {
            DataTable dt = Query(
                "SELECT ISNULL(t.NAME, pm.PAYMENTMETHODID) AS NAME FROM ax.CPPROMODISCOUNTPAYMENTMETHOD pm " +
                TenderNameApplySql +
                "WHERE pm.PROMOID = @PROMOID AND pm.DATAAREAID = @DATAAREAID ORDER BY pm.PAYMENTMETHODID",
                new SqlParameter("@PROMOID", promoId),
                new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
                new SqlParameter("@INVENTLOCATION", ApplicationSettings.Terminal.InventLocationId ?? string.Empty));

            string text;
            if (dt.Rows.Count == 0)
            {
                text = "Semua metode pembayaran berlaku.";
            }
            else
            {
                string[] ids = new string[dt.Rows.Count];
                for (int i = 0; i < ids.Length; i++) ids[i] = Convert.ToString(dt.Rows[i][0]);
                text = "Metode pembayaran: " + string.Join(", ", ids);
            }
            MessageBox.Show(text, promoId, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Realtime search: every keystroke restarts a 1 second timer; when the cashier stops typing
        // the loaded rows are filtered by item id / name containing the text.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void searchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            if (_detailTable == null) return;

            dataGridDetails.EndEdit();
            string text = txtSearch.Text.Trim();
            if (text.Length == 0)
            {
                _detailTable.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                // escape quotes and LIKE wildcards
                text = text.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
                _detailTable.DefaultView.RowFilter =
                    string.Format("ID LIKE '%{0}%' OR NAME LIKE '%{0}%'", text);
            }
            UpdateSelectedCount();   // header checkbox reflects the visible rows
        }
    }

    // Warning popup (title, subtitle, one red wrapped message, one teal button). Built in code, no designer file.
    internal class CP_PromoWarningPopup : Form
    {
        public CP_PromoWarningPopup(string title, string subtitle, string message, string buttonText)
        {
            Font = new System.Drawing.Font("Tahoma", 9F);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            BackColor = System.Drawing.Color.FromArgb(172, 193, 213);   // 1px frame
            Padding = new Padding(1);

            // header
            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 62;
            header.BackColor = System.Drawing.Color.FromArgb(228, 235, 241);
            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new System.Drawing.Point(16, 10);
            Label lblSub = new Label();
            lblSub.Text = subtitle;
            lblSub.AutoSize = true;
            lblSub.Location = new System.Drawing.Point(16, 36);
            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSub);

            // footer with the button
            Panel footer = new Panel();
            footer.Dock = DockStyle.Bottom;
            footer.Height = 62;
            footer.BackColor = System.Drawing.Color.FromArgb(228, 235, 241);
            Button button = new Button();
            button.Text = buttonText;
            button.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            button.BackColor = System.Drawing.Color.FromArgb(0, 184, 169);
            button.ForeColor = System.Drawing.Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Size = new System.Drawing.Size(150, 38);
            button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button.Click += delegate { Close(); };
            footer.Controls.Add(button);

            // body: red bullet message, wraps over several lines when long
            Panel body = new Panel();
            body.Dock = DockStyle.Fill;
            body.BackColor = System.Drawing.Color.FromArgb(247, 249, 251);
            Label lblMessage = new Label();
            lblMessage.Text = "\u2022   " + message;
            lblMessage.ForeColor = System.Drawing.Color.FromArgb(176, 0, 32);
            lblMessage.Font = new System.Drawing.Font("Tahoma", 10F);
            lblMessage.AutoSize = true;
            lblMessage.MaximumSize = new System.Drawing.Size(430, 0);
            lblMessage.Location = new System.Drawing.Point(16, 18);
            body.Controls.Add(lblMessage);

            // Fill first, then Bottom, then Top (docking order)
            Controls.Add(body);
            Controls.Add(footer);
            Controls.Add(header);

            ClientSize = new System.Drawing.Size(466, header.Height + footer.Height + lblMessage.PreferredSize.Height + 36);
            button.Location = new System.Drawing.Point(footer.Width - button.Width - 18, 12);
            AcceptButton = button;   // Enter closes
            CancelButton = button;   // Esc closes
        }
    }

    // "Kategori Berlaku" popup (opened by the Lihat Kategori button). Built in code, no designer file.
    internal class CP_PromoCategoryPopup : Form
    {
        private const int RowHeight = 26;

        public CP_PromoCategoryPopup(string promoName, List<string> categories)
            : this("Kategori Berlaku", promoName, categories) { }

        public CP_PromoCategoryPopup(string popupTitle, string promoName, List<string> categories)
        {
            Font = new System.Drawing.Font("Tahoma", 9F);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            BackColor = System.Drawing.Color.FromArgb(172, 193, 213);   // 1px frame
            Padding = new Padding(1);

            // header
            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 62;
            header.BackColor = System.Drawing.Color.FromArgb(228, 235, 241);
            Label title = new Label();
            title.Text = popupTitle;
            title.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new System.Drawing.Point(16, 10);
            Label sub = new Label();
            sub.Text = promoName;
            sub.AutoSize = true;
            sub.Location = new System.Drawing.Point(16, 36);
            header.Controls.Add(title);
            header.Controls.Add(sub);

            // footer
            Panel footer = new Panel();
            footer.Dock = DockStyle.Bottom;
            footer.Height = 62;
            footer.BackColor = System.Drawing.Color.FromArgb(228, 235, 241);
            Button close = new Button();
            close.Text = "Tutup";
            close.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            close.BackColor = System.Drawing.Color.FromArgb(0, 184, 169);
            close.ForeColor = System.Drawing.Color.White;
            close.FlatStyle = FlatStyle.Flat;
            close.FlatAppearance.BorderSize = 2;
            close.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            close.Size = new System.Drawing.Size(66, 38);
            close.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            close.Click += delegate { Close(); };
            footer.Controls.Add(close);

            // body: bulleted list (blank when there are no categories)
            ListBox list = new ListBox();
            list.Dock = DockStyle.Fill;
            list.BorderStyle = BorderStyle.None;
            list.BackColor = System.Drawing.Color.FromArgb(247, 249, 251);
            list.Font = new System.Drawing.Font("Tahoma", 10F);
            list.ItemHeight = RowHeight;
            list.DrawMode = DrawMode.OwnerDrawFixed;
            list.SelectionMode = SelectionMode.None;
            list.IntegralHeight = false;
            list.DrawItem += delegate(object s, DrawItemEventArgs e)
            {
                e.DrawBackground();
                if (e.Index < 0) return;
                TextRenderer.DrawText(e.Graphics, "\u2022   " + list.Items[e.Index], list.Font,
                    new System.Drawing.Rectangle(e.Bounds.X + 16, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height),
                    System.Drawing.Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
            foreach (string c in categories) list.Items.Add(c);

            // Fill first, then Bottom, then Top (docking order)
            Controls.Add(list);
            Controls.Add(footer);
            Controls.Add(header);

            int rows = Math.Max(2, Math.Min(categories.Count, 8));
            ClientSize = new System.Drawing.Size(466, header.Height + footer.Height + rows * RowHeight + 16);
            close.Location = new System.Drawing.Point(footer.Width - close.Width - 18, 12);
            CancelButton = close;   // Esc closes
        }
    }
}