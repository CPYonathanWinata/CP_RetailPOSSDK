namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class CP_frmPackingSlipDetail
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.postBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtSalesOrder = new System.Windows.Forms.TextBox();
            this.header = new System.Windows.Forms.Label();
            this.lblSalesOrder = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // postBtn
            // 
            this.postBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.postBtn.FlatAppearance.BorderSize = 0;
            this.postBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.postBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.postBtn.Location = new System.Drawing.Point(347, 637);
            this.postBtn.Name = "postBtn";
            this.postBtn.Size = new System.Drawing.Size(165, 68);
            this.postBtn.TabIndex = 14;
            this.postBtn.Text = "Post Packing\r\nSlip";
            this.postBtn.UseVisualStyleBackColor = false;
            this.postBtn.Click += new System.EventHandler(this.postBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.closeBtn.Location = new System.Drawing.Point(528, 637);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(165, 68);
            this.closeBtn.TabIndex = 13;
            this.closeBtn.Text = "Cancel";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(207, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 21);
            this.button1.TabIndex = 11;
            this.button1.Text = "Load SO";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.dataGridView1.Location = new System.Drawing.Point(31, 166);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(962, 454);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // txtSalesOrder
            // 
            this.txtSalesOrder.Location = new System.Drawing.Point(31, 139);
            this.txtSalesOrder.Name = "txtSalesOrder";
            this.txtSalesOrder.Size = new System.Drawing.Size(170, 21);
            this.txtSalesOrder.TabIndex = 15;
            this.txtSalesOrder.Visible = false;
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(313, 19);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(399, 65);
            this.header.TabIndex = 16;
            this.header.Text = "Packing Slip Form";
            this.header.Click += new System.EventHandler(this.header_Click);
            // 
            // lblSalesOrder
            // 
            this.lblSalesOrder.AutoSize = true;
            this.lblSalesOrder.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblSalesOrder.Location = new System.Drawing.Point(433, 88);
            this.lblSalesOrder.Name = "lblSalesOrder";
            this.lblSalesOrder.Size = new System.Drawing.Size(108, 23);
            this.lblSalesOrder.TabIndex = 17;
            this.lblSalesOrder.Text = "SO Number";
            this.lblSalesOrder.Click += new System.EventHandler(this.lblSalesOrder_Click);
            // 
            // CP_frmPackingSlipDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.lblSalesOrder);
            this.Controls.Add(this.header);
            this.Controls.Add(this.txtSalesOrder);
            this.Controls.Add(this.postBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CP_frmPackingSlipDetail";
            this.Controls.SetChildIndex(this.dataGridView1, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.closeBtn, 0);
            this.Controls.SetChildIndex(this.postBtn, 0);
            this.Controls.SetChildIndex(this.txtSalesOrder, 0);
            this.Controls.SetChildIndex(this.header, 0);
            this.Controls.SetChildIndex(this.lblSalesOrder, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Button postBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtSalesOrder;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Label lblSalesOrder;
    }
}
