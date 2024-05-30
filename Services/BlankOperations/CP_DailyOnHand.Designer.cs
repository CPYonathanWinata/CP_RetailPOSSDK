using System.Drawing;
using System.Windows.Forms;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CP_DailyOnHand
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
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.styleControllerDailyOnHand = new DevExpress.XtraEditors.StyleController(this.components);
            this.dataGridResult = new System.Windows.Forms.DataGridView();
            this.header = new System.Windows.Forms.Label();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.dateTimeBox = new System.Windows.Forms.TextBox();
            this.storeBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleControllerDailyOnHand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridResult
            // 
            this.dataGridResult.AllowUserToResizeRows = false;
            this.dataGridResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResult.EnableHeadersVisualStyles = false;
            this.dataGridResult.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.dataGridResult.Location = new System.Drawing.Point(29, 176);
            this.dataGridResult.Name = "dataGridResult";
            this.dataGridResult.RowHeadersVisible = false;
            this.dataGridResult.RowTemplate.Height = 40;
            this.dataGridResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridResult.Size = new System.Drawing.Size(962, 492);
            this.dataGridResult.TabIndex = 4;
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(331, 19);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(343, 65);
            this.header.TabIndex = 5;
            this.header.Text = "Daily On-Hand";
            // 
            // searchBox
            // 
            this.searchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBox.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.searchBox.Location = new System.Drawing.Point(29, 140);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(962, 32);
            this.searchBox.TabIndex = 0;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            this.searchBox.Enter += new System.EventHandler(this.searchBox_Enter);
            this.searchBox.Leave += new System.EventHandler(this.searchBox_Leave);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.closeBtn.Location = new System.Drawing.Point(448, 689);
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
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.textBox1.Location = new System.Drawing.Point(29, 114);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(239, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "*Qty dalam 3 desimal (.000)";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // CP_DailyOnHand
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.storeBox);
            this.Controls.Add(this.dateTimeBox);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.header);
            this.Controls.Add(this.dataGridResult);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CP_DailyOnHand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "CP_DailyOnHand";
            this.Load += new System.EventHandler(this.CP_DailyOnHand_Load);
            this.Controls.SetChildIndex(this.dataGridResult, 0);
            this.Controls.SetChildIndex(this.header, 0);
            this.Controls.SetChildIndex(this.searchBox, 0);
            this.Controls.SetChildIndex(this.closeBtn, 0);
            this.Controls.SetChildIndex(this.dateTimeBox, 0);
            this.Controls.SetChildIndex(this.storeBox, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleControllerDailyOnHand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.StyleController styleControllerDailyOnHand;
        private System.Windows.Forms.DataGridView dataGridResult;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button closeBtn;
        private DevExpress.XtraEditors.StyleController styleController;
        private TextBox dateTimeBox;
        private TextBox storeBox;
        private TextBox textBox1;
        internal Label header;
        private ContextMenuStrip contextMenuStrip1;
    }
}