using System.Drawing;
using System.Windows.Forms;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CPIBLIBLIORDERS
{
    partial class BlibliOrderList
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
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.header = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.pickerTanggal = new System.Windows.Forms.DateTimePicker();
            this.lblTanggal = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.gridBlibliOrderList = new System.Windows.Forms.DataGridView();
            this.order_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.order_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDetail = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStore = new System.Windows.Forms.Label();
            this.lblCurDate = new System.Windows.Forms.Label();
            this.comBoxStatus = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBlibliOrderList)).BeginInit();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(12, 19);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1000, 65);
            this.header.TabIndex = 10;
            this.header.Text = "Blibli Order List";
            this.header.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.White;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSearch.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.textBoxSearch.Location = new System.Drawing.Point(351, 138);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(322, 29);
            this.textBoxSearch.TabIndex = 13;
            this.textBoxSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pickerTanggal
            // 
            this.pickerTanggal.Font = new System.Drawing.Font("Tahoma", 10F);
            this.pickerTanggal.Location = new System.Drawing.Point(249, 183);
            this.pickerTanggal.Name = "pickerTanggal";
            this.pickerTanggal.Size = new System.Drawing.Size(216, 24);
            this.pickerTanggal.TabIndex = 14;
            // 
            // lblTanggal
            // 
            this.lblTanggal.AutoSize = true;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblTanggal.Location = new System.Drawing.Point(178, 184);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Size = new System.Drawing.Size(65, 17);
            this.lblTanggal.TabIndex = 15;
            this.lblTanggal.Text = "Tanggal :";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblStatus.Location = new System.Drawing.Point(481, 187);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 17);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Status :";
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApply.Location = new System.Drawing.Point(832, 184);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(180, 27);
            this.btnApply.TabIndex = 18;
            this.btnApply.Text = "Apply filter";
            this.btnApply.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // gridBlibliOrderList
            // 
            this.gridBlibliOrderList.AllowUserToAddRows = false;
            this.gridBlibliOrderList.AllowUserToDeleteRows = false;
            this.gridBlibliOrderList.AllowUserToResizeColumns = false;
            this.gridBlibliOrderList.AllowUserToResizeRows = false;
            this.gridBlibliOrderList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.gridBlibliOrderList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 16F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBlibliOrderList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBlibliOrderList.ColumnHeadersHeight = 50;
            this.gridBlibliOrderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridBlibliOrderList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.order_id,
            this.status,
            this.order_time,
            this.orderDetail});
            this.gridBlibliOrderList.EnableHeadersVisualStyles = false;
            this.gridBlibliOrderList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.gridBlibliOrderList.Location = new System.Drawing.Point(12, 225);
            this.gridBlibliOrderList.Name = "gridBlibliOrderList";
            this.gridBlibliOrderList.ReadOnly = true;
            this.gridBlibliOrderList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridBlibliOrderList.RowTemplate.Height = 50;
            this.gridBlibliOrderList.RowTemplate.ReadOnly = true;
            this.gridBlibliOrderList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridBlibliOrderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBlibliOrderList.Size = new System.Drawing.Size(1000, 459);
            this.gridBlibliOrderList.TabIndex = 25;
            // 
            // order_id
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.order_id.DefaultCellStyle = dataGridViewCellStyle2;
            this.order_id.HeaderText = "Nomor Order";
            this.order_id.Name = "order_id";
            this.order_id.ReadOnly = true;
            this.order_id.Width = 250;
            // 
            // status
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.DefaultCellStyle = dataGridViewCellStyle3;
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 250;
            // 
            // order_time
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.order_time.DefaultCellStyle = dataGridViewCellStyle4;
            this.order_time.HeaderText = "Tanggal Order";
            this.order_time.Name = "order_time";
            this.order_time.ReadOnly = true;
            this.order_time.Width = 250;
            // 
            // orderDetail
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(170)))), ((int)(((byte)(190)))));
            this.orderDetail.DefaultCellStyle = dataGridViewCellStyle5;
            this.orderDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.orderDetail.HeaderText = "";
            this.orderDetail.Name = "orderDetail";
            this.orderDetail.ReadOnly = true;
            this.orderDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.orderDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.orderDetail.Text = "Detail";
            this.orderDetail.UseColumnTextForButtonValue = true;
            this.orderDetail.Width = 205;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(885, 700);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 53);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblStore
            // 
            this.lblStore.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblStore.Location = new System.Drawing.Point(337, 84);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(315, 21);
            this.lblStore.TabIndex = 21;
            this.lblStore.Text = "Toko";
            this.lblStore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurDate
            // 
            this.lblCurDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblCurDate.Location = new System.Drawing.Point(337, 105);
            this.lblCurDate.Name = "lblCurDate";
            this.lblCurDate.Size = new System.Drawing.Size(315, 21);
            this.lblCurDate.TabIndex = 22;
            this.lblCurDate.Text = "Tanggal";
            this.lblCurDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comBoxStatus
            // 
            this.comBoxStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comBoxStatus.FormattingEnabled = true;
            this.comBoxStatus.Location = new System.Drawing.Point(543, 184);
            this.comBoxStatus.Name = "comBoxStatus";
            this.comBoxStatus.Size = new System.Drawing.Size(195, 24);
            this.comBoxStatus.TabIndex = 23;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(12, 700);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(127, 53);
            this.btnRefresh.TabIndex = 24;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // BlibliOrderList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.comBoxStatus);
            this.Controls.Add(this.lblCurDate);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridBlibliOrderList);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTanggal);
            this.Controls.Add(this.pickerTanggal);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlibliOrderList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBlibliOrderList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

       

        #endregion

        private DevExpress.XtraEditors.StyleController styleController;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.DateTimePicker pickerTanggal;
        private System.Windows.Forms.Label lblTanggal;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.DataGridView gridBlibliOrderList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.Label lblCurDate;
        private System.Windows.Forms.ComboBox comBoxStatus;
        private System.Windows.Forms.Button btnRefresh;
        Panel overlayPanel = new Panel();
        private DataGridViewTextBoxColumn order_id;
        private DataGridViewTextBoxColumn status;
        private DataGridViewTextBoxColumn order_time;
        private DataGridViewButtonColumn orderDetail;
    }
}