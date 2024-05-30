using System.Drawing;
namespace PaymentTriggers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Infolog));
            this.okBtn = new System.Windows.Forms.Button();
            this.gridViewItem = new System.Windows.Forms.DataGridView();
            this.ItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemainQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderQty = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.infoLbl = new System.Windows.Forms.Label();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.OrderQty,
            this.RemainQty
            });
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
            // RemainQty
            // 
            this.RemainQty.HeaderText = "Stok Tersedia";
            this.RemainQty.Name = "RemainQty";
            this.RemainQty.ReadOnly = true;
            this.RemainQty.Width = 80;
            //
            // OrderQty
            //
            this.OrderQty.HeaderText = "Qty Beli";
            this.OrderQty.Name = "QtyOrder";
            this.OrderQty.ReadOnly = true;
            this.OrderQty.Width = 80;
            // 
            // infoLbl
            // 
            this.infoLbl.AutoSize = true;
            this.infoLbl.BackColor = System.Drawing.Color.OrangeRed;
            this.infoLbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.infoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLbl.Location = new System.Drawing.Point(81, 9);
            this.infoLbl.Name = "infoLbl";
            this.infoLbl.Size = new System.Drawing.Size(471, 38);
            this.infoLbl.TabIndex = 2;
            this.infoLbl.Text = "Stok barang di bawah ini tidak mencukupi untuk ditransaksikan.\nSilakan hapus atau" +
    " kurangi jumlahnya agar tidak melebihi stok tersedia";
            this.infoLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // iconBox
            // 
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            this.iconBox.Location = new System.Drawing.Point(44, 9);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(37, 40);
            this.iconBox.TabIndex = 3;
            this.iconBox.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(558, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 40);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // Infolog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(631, 372);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.iconBox);
            this.Controls.Add(this.infoLbl);
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
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.DataGridView gridViewItem;
        private System.Windows.Forms.Label infoLbl;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemainQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderQty;
    }
}