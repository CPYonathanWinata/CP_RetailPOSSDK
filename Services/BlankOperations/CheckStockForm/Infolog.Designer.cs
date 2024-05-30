namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CheckStockForm
{
    partial class Infolog
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
            this.okBtn = new System.Windows.Forms.Button();
            this.gridViewItem = new System.Windows.Forms.DataGridView();
            this.ItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemainQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewItem)).BeginInit();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okBtn.Location = new System.Drawing.Point(1, 334);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(629, 37);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "Close";
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // gridViewItem
            // 
            this.gridViewItem.AllowUserToAddRows = false;
            this.gridViewItem.AllowUserToDeleteRows = false;
            this.gridViewItem.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.gridViewItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemId,
            this.ItemName,
            this.QtyOrder,
            this.RemainQty});
            this.gridViewItem.Location = new System.Drawing.Point(1, 70);
            this.gridViewItem.Name = "gridViewItem";
            this.gridViewItem.ReadOnly = true;
            this.gridViewItem.Size = new System.Drawing.Size(729, 258);
            this.gridViewItem.TabIndex = 1;
            // 
            // ItemId
            // 
            this.ItemId.HeaderText = "SKU";
            this.ItemId.Name = "ItemId";
            this.ItemId.ReadOnly = true;
            this.ItemId.Width = 80;
            // 
            // ItemName
            // 
            this.ItemName.HeaderText = "Nama Barang";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.Width = 350;
            // 
            // QtyOrder
            // 
            this.QtyOrder.HeaderText = "Qty Beli";
            this.QtyOrder.Name = "QtyOrder";
            this.QtyOrder.ReadOnly = true;
            this.QtyOrder.Width = 80;
            // 
            // RemainQty
            // 
            this.RemainQty.HeaderText = "Stok Tersedia";
            this.RemainQty.Name = "RemainQty";
            this.RemainQty.ReadOnly = true;
            this.RemainQty.Width = 80;
            // 
            // Infolog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(631, 372);
            this.ControlBox = false;
            this.Controls.Add(this.gridViewItem);
            this.Controls.Add(this.okBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Infolog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "INFORMASI STOK";
            ((System.ComponentModel.ISupportInitialize)(this.gridViewItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.DataGridView gridViewItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemainQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtyOrder;
    
    }
}