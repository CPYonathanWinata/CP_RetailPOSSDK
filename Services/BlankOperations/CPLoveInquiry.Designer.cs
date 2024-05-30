namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPLoveInquiry
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
            this.colCreatedDateTime = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.colStaff = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.colTerminal = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.colReceiptId = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.colType = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.colAmount = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.colCPNoOrder = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_colCPNoOrder = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colCPNameOrder = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_colCPNameOrder = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colCPDateOrder = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_colCPDateOrder = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.timer_location = new System.Windows.Forms.Timer(this.components);
            this.lblSummary2 = new System.Windows.Forms.Label();
            this.lblSummary3 = new System.Windows.Forms.Label();
            this.btn_printZ = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblSummary4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colCPNoOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colCPNameOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colCPDateOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // colCreatedDateTime
            // 
            this.colCreatedDateTime.Caption = "Date";
            this.colCreatedDateTime.FieldName = "CREATEDDATETIME";
            this.colCreatedDateTime.LayoutViewField = null;
            this.colCreatedDateTime.Name = "colCreatedDateTime";
            // 
            // colStaff
            // 
            this.colStaff.Caption = "Operator ID";
            this.colStaff.FieldName = "STAFF";
            this.colStaff.LayoutViewField = null;
            this.colStaff.Name = "colStaff";
            // 
            // colTerminal
            // 
            this.colTerminal.Caption = "Register";
            this.colTerminal.FieldName = "TERMINAL";
            this.colTerminal.LayoutViewField = null;
            this.colTerminal.Name = "colTerminal";
            // 
            // colReceiptId
            // 
            this.colReceiptId.Caption = "Receipt";
            this.colReceiptId.FieldName = "RECEIPTID";
            this.colReceiptId.LayoutViewField = null;
            this.colReceiptId.Name = "colReceiptId";
            // 
            // colType
            // 
            this.colType.Caption = "Type";
            this.colType.FieldName = "TYPE";
            this.colType.LayoutViewField = null;
            this.colType.Name = "colType";
            // 
            // colAmount
            // 
            this.colAmount.Caption = "Amount";
            this.colAmount.FieldName = "GROSSAMOUNT";
            this.colAmount.LayoutViewField = null;
            this.colAmount.Name = "colAmount";
            // 
            // colCPNoOrder
            // 
            this.colCPNoOrder.Caption = "No Order";
            this.colCPNoOrder.FieldName = "CPNoOrder";
            this.colCPNoOrder.LayoutViewField = this.layoutViewField_colCPNoOrder;
            this.colCPNoOrder.Name = "colCPNoOrder";
            this.colCPNoOrder.Width = 77;
            // 
            // layoutViewField_colCPNoOrder
            // 
            this.layoutViewField_colCPNoOrder.EditorPreferredWidth = 152;
            this.layoutViewField_colCPNoOrder.Location = new System.Drawing.Point(0, 144);
            this.layoutViewField_colCPNoOrder.Name = "layoutViewField_colCPNoOrder";
            this.layoutViewField_colCPNoOrder.Size = new System.Drawing.Size(222, 24);
            this.layoutViewField_colCPNoOrder.TextSize = new System.Drawing.Size(62, 13);
            // 
            // colCPNameOrder
            // 
            this.colCPNameOrder.Caption = "Name Order";
            this.colCPNameOrder.FieldName = "CPNameOrder";
            this.colCPNameOrder.LayoutViewField = this.layoutViewField_colCPNameOrder;
            this.colCPNameOrder.Name = "colCPNameOrder";
            this.colCPNameOrder.Width = 94;
            // 
            // layoutViewField_colCPNameOrder
            // 
            this.layoutViewField_colCPNameOrder.EditorPreferredWidth = 152;
            this.layoutViewField_colCPNameOrder.Location = new System.Drawing.Point(0, 168);
            this.layoutViewField_colCPNameOrder.Name = "layoutViewField_colCPNameOrder";
            this.layoutViewField_colCPNameOrder.Size = new System.Drawing.Size(222, 24);
            this.layoutViewField_colCPNameOrder.TextSize = new System.Drawing.Size(62, 13);
            // 
            // colCPDateOrder
            // 
            this.colCPDateOrder.Caption = "Date Order";
            this.colCPDateOrder.FieldName = "CPDateOrder";
            this.colCPDateOrder.LayoutViewField = this.layoutViewField_colCPDateOrder;
            this.colCPDateOrder.Name = "colCPDateOrder";
            this.colCPDateOrder.Width = 90;
            // 
            // layoutViewField_colCPDateOrder
            // 
            this.layoutViewField_colCPDateOrder.EditorPreferredWidth = 152;
            this.layoutViewField_colCPDateOrder.Location = new System.Drawing.Point(0, 192);
            this.layoutViewField_colCPDateOrder.Name = "layoutViewField_colCPDateOrder";
            this.layoutViewField_colCPDateOrder.Size = new System.Drawing.Size(222, 24);
            this.layoutViewField_colCPDateOrder.TextSize = new System.Drawing.Size(62, 13);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(13, 13);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(86, 16);
            this.lblPeriod.TabIndex = 2;
            this.lblPeriod.Text = "Period From";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Location = new System.Drawing.Point(113, 8);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(278, 23);
            this.dtpFrom.TabIndex = 1;
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Location = new System.Drawing.Point(431, 8);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(278, 23);
            this.dtpTo.TabIndex = 2;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(397, 13);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 16);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To";
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Location = new System.Drawing.Point(725, 7);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(92, 29);
            this.btnFilter.TabIndex = 3;
            this.btnFilter.Text = "Search";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(11, 451);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(88, 14);
            this.lblSummary.TabIndex = 5;
            this.lblSummary.Text = "Summary 1";
            // 
            // timer_location
            // 
            this.timer_location.Enabled = true;
            this.timer_location.Tick += new System.EventHandler(this.timer_location_Tick);
            // 
            // lblSummary2
            // 
            this.lblSummary2.AutoSize = true;
            this.lblSummary2.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary2.Location = new System.Drawing.Point(11, 469);
            this.lblSummary2.Name = "lblSummary2";
            this.lblSummary2.Size = new System.Drawing.Size(88, 14);
            this.lblSummary2.TabIndex = 6;
            this.lblSummary2.Text = "Summary 2";
            // 
            // lblSummary3
            // 
            this.lblSummary3.AutoSize = true;
            this.lblSummary3.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary3.Location = new System.Drawing.Point(11, 486);
            this.lblSummary3.Name = "lblSummary3";
            this.lblSummary3.Size = new System.Drawing.Size(88, 14);
            this.lblSummary3.TabIndex = 7;
            this.lblSummary3.Text = "Summary 3";
            // 
            // btn_printZ
            // 
            this.btn_printZ.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_printZ.Location = new System.Drawing.Point(431, 413);
            this.btn_printZ.Name = "btn_printZ";
            this.btn_printZ.Size = new System.Drawing.Size(92, 29);
            this.btn_printZ.TabIndex = 9;
            this.btn_printZ.Text = "Z Report Love";
            this.btn_printZ.UseVisualStyleBackColor = true;
            this.btn_printZ.Click += new System.EventHandler(this.btn_printZ_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 57);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(978, 341);
            this.dataGridView1.TabIndex = 10;
            // 
            // lblSummary4
            // 
            this.lblSummary4.AutoSize = true;
            this.lblSummary4.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary4.Location = new System.Drawing.Point(11, 506);
            this.lblSummary4.Name = "lblSummary4";
            this.lblSummary4.Size = new System.Drawing.Size(88, 14);
            this.lblSummary4.TabIndex = 11;
            this.lblSummary4.Text = "Summary 4";
            // 
            // CPLoveInquiry
            // 
            this.AcceptButton = this.btnFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 560);
            this.Controls.Add(this.lblSummary4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_printZ);
            this.Controls.Add(this.lblSummary3);
            this.Controls.Add(this.lblSummary2);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblPeriod);
            this.Name = "CPLoveInquiry";
            this.Text = "Love Inquiry";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colCPNoOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colCPNameOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colCPDateOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn colTRANSACTIONID;
      

        private DevExpress.XtraGrid.Columns.LayoutViewColumn colCreatedDateTime;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colStaff;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colTerminal;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colReceiptId;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colType;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colAmount;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colCPNoOrder;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colCPNoOrder;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colCPNameOrder;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colCPNameOrder;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colCPDateOrder;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colCPDateOrder;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Timer timer_location;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblSummary2;
        private System.Windows.Forms.Label lblSummary3;
        private System.Windows.Forms.Button btn_printZ;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblSummary4;
    }
}