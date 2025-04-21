using System.Drawing;
using System.Windows.Forms;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPGrabOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.styleControllerDailyOnHand = new DevExpress.XtraEditors.StyleController(this.components);
            this.header = new System.Windows.Forms.Label();
            this.closeBtn = new System.Windows.Forms.Button();
            this.dateTimeBox = new System.Windows.Forms.TextBox();
            this.storeBox = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.parentPanel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grabMartList = new System.Windows.Forms.DataGridView();
            this.OrderIDLong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Details = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabpage2 = new System.Windows.Forms.TabPage();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.itemDetailsGrid = new System.Windows.Forms.DataGridView();
            this.lblOrder = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.ItemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Specifications = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleControllerDailyOnHand)).BeginInit();
            this.parentPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grabMartList)).BeginInit();
            this.tabpage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemDetailsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(318, 15);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(388, 65);
            this.header.TabIndex = 5;
            this.header.Text = "Grabmart Orders";
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.closeBtn.Location = new System.Drawing.Point(448, 687);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(127, 57);
            this.closeBtn.TabIndex = 8;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // dateTimeBox
            // 
            this.dateTimeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.dateTimeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dateTimeBox.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.dateTimeBox.Location = new System.Drawing.Point(342, 105);
            this.dateTimeBox.Name = "dateTimeBox";
            this.dateTimeBox.ReadOnly = true;
            this.dateTimeBox.Size = new System.Drawing.Size(312, 29);
            this.dateTimeBox.TabIndex = 9;
            this.dateTimeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // storeBox
            // 
            this.storeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.storeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.storeBox.Font = new System.Drawing.Font("Tahoma", 16F);
            this.storeBox.Location = new System.Drawing.Point(329, 83);
            this.storeBox.Name = "storeBox";
            this.storeBox.ReadOnly = true;
            this.storeBox.Size = new System.Drawing.Size(345, 26);
            this.storeBox.TabIndex = 10;
            this.storeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnOK.Location = new System.Drawing.Point(184, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(205, 40);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "Terima pesanan";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // parentPanel
            // 
            this.parentPanel.Controls.Add(this.tabControl);
            this.parentPanel.Location = new System.Drawing.Point(36, 140);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(959, 529);
            this.parentPanel.TabIndex = 24;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabpage2);
            this.tabControl.Location = new System.Drawing.Point(3, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(956, 526);
            this.tabControl.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.btnRefresh);
            this.tabPage1.Controls.Add(this.grabMartList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(948, 500);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(41, 451);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(127, 40);
            this.btnRefresh.TabIndex = 29;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grabMartList
            // 
            this.grabMartList.AllowUserToAddRows = false;
            this.grabMartList.AllowUserToResizeColumns = false;
            this.grabMartList.AllowUserToResizeRows = false;
            this.grabMartList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.grabMartList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grabMartList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grabMartList.ColumnHeadersHeight = 50;
            this.grabMartList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderIDLong,
            this.OrderID,
            this.state,
            this.OrderTime,
            this.Details});
            this.grabMartList.EnableHeadersVisualStyles = false;
            this.grabMartList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.grabMartList.Location = new System.Drawing.Point(41, 29);
            this.grabMartList.Name = "grabMartList";
            this.grabMartList.RowTemplate.Height = 50;
            this.grabMartList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grabMartList.Size = new System.Drawing.Size(864, 400);
            this.grabMartList.TabIndex = 28;
            this.grabMartList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grabMartList_CellFormatting);
            // 
            // OrderIDLong
            // 
            this.OrderIDLong.HeaderText = "Order Id Long";
            this.OrderIDLong.Name = "OrderIDLong";
            this.OrderIDLong.ReadOnly = true;
            this.OrderIDLong.Visible = false;
            this.OrderIDLong.Width = 250;
            // 
            // OrderID
            // 
            this.OrderID.HeaderText = "Nomor Order";
            this.OrderID.Name = "OrderID";
            this.OrderID.ReadOnly = true;
            this.OrderID.Width = 250;
            // 
            // state
            // 
            this.state.HeaderText = "Status";
            this.state.Name = "state";
            this.state.ReadOnly = true;
            // 
            // OrderTime
            // 
            this.OrderTime.HeaderText = "Tanggal Order";
            this.OrderTime.Name = "OrderTime";
            this.OrderTime.ReadOnly = true;
            this.OrderTime.Width = 160;
            // 
            // Details
            // 
            this.Details.HeaderText = "";
            this.Details.Name = "Details";
            this.Details.ReadOnly = true;
            this.Details.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Details.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Details.Text = "Detail Pesanan";
            this.Details.UseColumnTextForButtonValue = true;
            // 
            // tabpage2
            // 
            this.tabpage2.BackColor = System.Drawing.Color.White;
            this.tabpage2.Controls.Add(this.txtGrandTotal);
            this.tabpage2.Controls.Add(this.btnCancelOrder);
            this.tabpage2.Controls.Add(this.btnOK);
            this.tabpage2.Controls.Add(this.btnBack);
            this.tabpage2.Controls.Add(this.itemDetailsGrid);
            this.tabpage2.Controls.Add(this.lblOrder);
            this.tabpage2.Location = new System.Drawing.Point(4, 22);
            this.tabpage2.Name = "tabpage2";
            this.tabpage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabpage2.Size = new System.Drawing.Size(948, 500);
            this.tabpage2.TabIndex = 1;
            this.tabpage2.Text = "tabPage2";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.txtGrandTotal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGrandTotal.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtGrandTotal.Location = new System.Drawing.Point(624, 462);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.ReadOnly = true;
            this.txtGrandTotal.Size = new System.Drawing.Size(280, 26);
            this.txtGrandTotal.TabIndex = 31;
            this.txtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCancelOrder
            // 
            this.btnCancelOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnCancelOrder.FlatAppearance.BorderSize = 0;
            this.btnCancelOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOrder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancelOrder.Location = new System.Drawing.Point(406, 451);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.Size = new System.Drawing.Size(205, 40);
            this.btnCancelOrder.TabIndex = 30;
            this.btnCancelOrder.Text = "Batalkan pesanan";
            this.btnCancelOrder.UseVisualStyleBackColor = false;
            this.btnCancelOrder.Click += new System.EventHandler(this.btnCancelOrder_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(41, 451);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(127, 40);
            this.btnBack.TabIndex = 28;
            this.btnBack.Text = "Kembali";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // itemDetailsGrid
            // 
            this.itemDetailsGrid.AllowUserToAddRows = false;
            this.itemDetailsGrid.AllowUserToResizeRows = false;
            this.itemDetailsGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.itemDetailsGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.itemDetailsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.itemDetailsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itemDetailsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemID,
            this.colItem,
            this.Specifications,
            this.Quantity,
            this.colPrice,
            this.colDisc,
            this.Subtotal,
            this.ItemStock,
            this.Stock});
            this.itemDetailsGrid.EnableHeadersVisualStyles = false;
            this.itemDetailsGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.itemDetailsGrid.Location = new System.Drawing.Point(41, 29);
            this.itemDetailsGrid.Name = "itemDetailsGrid";
            this.itemDetailsGrid.RowHeadersVisible = false;
            this.itemDetailsGrid.RowTemplate.Height = 40;
            this.itemDetailsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.itemDetailsGrid.Size = new System.Drawing.Size(863, 401);
            this.itemDetailsGrid.TabIndex = 16;
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblOrder.Location = new System.Drawing.Point(37, 3);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(165, 23);
            this.lblOrder.TabIndex = 15;
            this.lblOrder.Text = "Order Number : ";
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnCheck.FlatAppearance.BorderSize = 0;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCheck.Location = new System.Drawing.Point(868, 704);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(127, 40);
            this.btnCheck.TabIndex = 29;
            this.btnCheck.Text = "Check Stock";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Visible = false;
            // 
            // ItemID
            // 
            this.ItemID.HeaderText = "SKU";
            this.ItemID.Name = "ItemID";
            this.ItemID.ReadOnly = true;
            this.ItemID.Width = 70;
            // 
            // colItem
            // 
            this.colItem.HeaderText = "Item Name";
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.Width = 240;
            // 
            // Specifications
            // 
            this.Specifications.HeaderText = "Catatan";
            this.Specifications.Name = "Specifications";
            this.Specifications.ReadOnly = true;
            this.Specifications.Width = 100;
            // 
            // Quantity
            // 
            this.Quantity.HeaderText = "QTY";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 40;
            // 
            // colPrice
            // 
            dataGridViewCellStyle3.Format = "N2";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPrice.HeaderText = "Harga (Per Qty)";
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            this.colPrice.Width = 85;
            // 
            // colDisc
            // 
            dataGridViewCellStyle6.Format = "N2";
            this.colDisc.DefaultCellStyle = dataGridViewCellStyle6;
            this.colDisc.HeaderText = "Diskon";
            this.colDisc.Name = "colDisc";
            this.colDisc.ReadOnly = true;
            this.colDisc.Width = 80;
            // 
            // Subtotal
            // 
            dataGridViewCellStyle4.Format = "N2";
            this.Subtotal.DefaultCellStyle = dataGridViewCellStyle4;
            this.Subtotal.HeaderText = "Subtotal";
            this.Subtotal.Name = "Subtotal";
            this.Subtotal.ReadOnly = true;
            this.Subtotal.Width = 110;
            // 
            // ItemStock
            // 
            dataGridViewCellStyle5.Format = "N";
            this.ItemStock.DefaultCellStyle = dataGridViewCellStyle5;
            this.ItemStock.HeaderText = "Stock Toko";
            this.ItemStock.Name = "ItemStock";
            this.ItemStock.ReadOnly = true;
            this.ItemStock.Width = 70;
            // 
            // Stock
            // 
            this.Stock.HeaderText = "Stock cukup?";
            this.Stock.Name = "Stock";
            this.Stock.ReadOnly = true;
            this.Stock.Width = 65;
            // 
            // CPGrabOrder
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.parentPanel);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.storeBox);
            this.Controls.Add(this.dateTimeBox);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.header);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CPGrabOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "CPGrabmartOrder";
            this.Load += new System.EventHandler(this.CPGrabOrder_Load);
            this.Controls.SetChildIndex(this.header, 0);
            this.Controls.SetChildIndex(this.closeBtn, 0);
            this.Controls.SetChildIndex(this.dateTimeBox, 0);
            this.Controls.SetChildIndex(this.storeBox, 0);
            this.Controls.SetChildIndex(this.btnCheck, 0);
            this.Controls.SetChildIndex(this.parentPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleControllerDailyOnHand)).EndInit();
            this.parentPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grabMartList)).EndInit();
            this.tabpage2.ResumeLayout(false);
            this.tabpage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemDetailsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private DevExpress.XtraEditors.StyleController styleControllerDailyOnHand;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Button closeBtn;
        private DevExpress.XtraEditors.StyleController styleController;
        private TextBox dateTimeBox;
        private TextBox storeBox;
        private Button btnOK;
        private Panel parentPanel;
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabpage2;
        private DataGridView itemDetailsGrid;
        private DataGridViewTextBoxColumn colSKU;
        private DataGridViewTextBoxColumn colQty;
        private Label lblOrder;
        private DataGridView grabMartList;
        private Button btnBack;
        private Button btnCheck;
        private Button btnCancelOrder;
        private DataGridViewTextBoxColumn OrderID;
        private DataGridViewTextBoxColumn state;
        private DataGridViewTextBoxColumn OrderTime;
        private DataGridViewButtonColumn Details;
        private DataGridViewTextBoxColumn OrderIDLong;
        private Button btnRefresh;
        private TextBox txtGrandTotal;
        private DataGridViewTextBoxColumn ItemID;
        private DataGridViewTextBoxColumn colItem;
        private DataGridViewTextBoxColumn Specifications;
        private DataGridViewTextBoxColumn Quantity;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colDisc;
        private DataGridViewTextBoxColumn Subtotal;
        private DataGridViewTextBoxColumn ItemStock;
        private DataGridViewTextBoxColumn Stock;
    }
}