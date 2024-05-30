using System.Windows.Forms;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CheckStockForm
{
    partial class MainFormCheckStock
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEMNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(19, 29);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(64, 20);
            this.addBtn.TabIndex = 1;
            this.addBtn.Text = "Add Item";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SKU,
            this.ITEMNAME,
            this.Stock,
            this.deleteBtn});
            this.dataGridView1.Location = new System.Drawing.Point(19, 54);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(656, 263);
            this.dataGridView1.TabIndex = 2;
            // 
            // checkBtn
            // 
            this.checkBtn.Location = new System.Drawing.Point(19, 335);
            this.checkBtn.Name = "checkBtn";
            this.checkBtn.Size = new System.Drawing.Size(110, 20);
            this.checkBtn.TabIndex = 3;
            this.checkBtn.Text = "Check Stock";
            this.checkBtn.UseVisualStyleBackColor = true;
            this.checkBtn.Click += new System.EventHandler(this.checkBtn_Click);
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(592, 29);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(75, 23);
            this.clearBtn.TabIndex = 4;
            this.clearBtn.Text = "Clear all";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // SKU
            // 
            this.SKU.HeaderText = "SKU";
            this.SKU.Name = "SKU";
            this.SKU.ReadOnly = true;
            // 
            // ITEMNAME
            // 
            this.ITEMNAME.HeaderText = "Nama Barang";
            this.ITEMNAME.Name = "ITEMNAME";
            this.ITEMNAME.ReadOnly = true;
            this.ITEMNAME.Width = 310;
            // 
            // Stock
            // 
            this.Stock.HeaderText = "Stok Tersedia";
            this.Stock.Name = "Stock";
            this.Stock.ReadOnly = true;
            // 
            // deleteBtn
            // 
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.deleteBtn.HeaderText = "";
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.ReadOnly = true;
            this.deleteBtn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseColumnTextForButtonValue = true;
            // 
            // MainFormCheckStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 368);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.checkBtn);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.addBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainFormCheckStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Check Stock Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormCheckStock_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button addBtn;
        private DataGridView dataGridView1;
        private Button checkBtn;
        private Button clearBtn;
        private DataGridViewTextBoxColumn SKU;
        private DataGridViewTextBoxColumn ITEMNAME;
        private DataGridViewTextBoxColumn Stock;
        private DataGridViewButtonColumn deleteBtn;
    
    }
}