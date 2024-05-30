namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPShowJournal
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
            this.jBENHL1StoresDataSet = new Microsoft.Dynamics.Retail.Pos.BlankOperations.JBENHL1StoresDataSet();
            this.jBENHL1StoresDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTransactionid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcpNoOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcpNameOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcpDateOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            ((System.ComponentModel.ISupportInitialize)(this.jBENHL1StoresDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jBENHL1StoresDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // jBENHL1StoresDataSet
            // 
            this.jBENHL1StoresDataSet.DataSetName = "JBENHL1StoresDataSet";
            this.jBENHL1StoresDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // jBENHL1StoresDataSetBindingSource
            // 
            this.jBENHL1StoresDataSetBindingSource.DataSource = this.jBENHL1StoresDataSet;
            this.jBENHL1StoresDataSetBindingSource.Position = 0;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTransactionid,
            this.colcpNoOrder,
            this.colcpNameOrder,
            this.colcpDateOrder});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colTransactionid
            // 
            this.colTransactionid.FieldName = "Transactionid";
            this.colTransactionid.Name = "colTransactionid";
            this.colTransactionid.Visible = true;
            this.colTransactionid.VisibleIndex = 0;
            // 
            // colcpNoOrder
            // 
            this.colcpNoOrder.FieldName = "cpNoOrder";
            this.colcpNoOrder.Name = "colcpNoOrder";
            this.colcpNoOrder.Visible = true;
            this.colcpNoOrder.VisibleIndex = 1;
            // 
            // colcpNameOrder
            // 
            this.colcpNameOrder.FieldName = "cpNameOrder";
            this.colcpNameOrder.Name = "colcpNameOrder";
            this.colcpNameOrder.Visible = true;
            this.colcpNameOrder.VisibleIndex = 2;
            // 
            // colcpDateOrder
            // 
            this.colcpDateOrder.FieldName = "cpDateOrder";
            this.colcpDateOrder.Name = "colcpDateOrder";
            this.colcpDateOrder.Visible = true;
            this.colcpDateOrder.VisibleIndex = 3;
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView2.GridControl = this.gridControl1;
            this.gridView2.Name = "gridView2";
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "Transactionid";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.FieldName = "cpNoOrder";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.FieldName = "cpNameOrder";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "cpDateOrder";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridControl1
            // 
            this.gridControl1.DataMember = "Query";
            this.gridControl1.Location = new System.Drawing.Point(41, 50);
            this.gridControl1.MainView = this.gridView2;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(451, 200);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2,
            this.gridView1});
            // 
            // CPShowJournal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 262);
            this.Controls.Add(this.gridControl1);
            this.Name = "CPShowJournal";
            this.Text = "CPShowJournal";
            this.Load += new System.EventHandler(this.CPShowJournal_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.jBENHL1StoresDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jBENHL1StoresDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private JBENHL1StoresDataSet jBENHL1StoresDataSet;
        private System.Windows.Forms.BindingSource jBENHL1StoresDataSetBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colTransactionid;
        private DevExpress.XtraGrid.Columns.GridColumn colcpNoOrder;
        private DevExpress.XtraGrid.Columns.GridColumn colcpNameOrder;
        private DevExpress.XtraGrid.Columns.GridColumn colcpDateOrder;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;

    }
}