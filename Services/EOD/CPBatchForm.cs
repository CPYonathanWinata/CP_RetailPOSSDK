using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.EOD
{
    public partial class CPBatchForm : Form
    {
         public string SelectedBatchId { get; private set; }
        public string SelectedTerminalId { get; private set; }

        private string connectionString = ApplicationSettings.Database.LocalConnectionString; // Add your SQL connection string here
        private string storeId; // Store ID passed from the main form

        public CPBatchForm(string storeId)
        {
            this.InitializeComponent();
            this.storeId = storeId;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ControlBox = false;
            // Disable window resizing
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            LoadData();  // Load data into the DataGridView when form is initialized
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT BATCHID, STARTDATE, CLOSEDATE, TERMINALID, STOREID, STAFFID FROM [ax].[RETAILPOSBATCHTABLE] WHERE STOREID = @StoreId ORDER BY BATCHID DESC";
                    query = @" SELECT 
                                    BATCH.BATCHID AS BATCHID, 
                                    DATEADD(HOUR, 7, STARTDATETIMEUTC) AS STARTDATE,  -- Convert to UTC+7
                                    DATEADD(HOUR, 7, CLOSEDATETIMEUTC) AS CLOSEDATE,  -- Convert to UTC+7
                                    BATCH.TERMINALID AS TERMINALID, 
                                    BATCH.STOREID AS STOREID, 
                                    ISNULL(OPENBY, '') AS OPENBY, 
                                    ISNULL(CLOSEBY, '') AS CLOSEBY
                                FROM 
                                    [ax].[RETAILPOSBATCHTABLE] BATCH
                                LEFT JOIN 
                                    [ax].[CPRETAILPOSBATCHTABLEEXTEND] EXT ON BATCH.BATCHID = EXT.BATCHID
                                WHERE 
                                    BATCH.STOREID = @StoreId
                                    AND CLOSEDATETIMEUTC >= DATEADD(DAY, -7, GETDATE()) 
                                ORDER BY 
                                    BATCHID DESC;
                                ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StoreId", storeId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;   
                    dataGridView1.Columns["BATCHID"].HeaderText  = "Batch";
                    dataGridView1.Columns["BATCHID"].Visible = false;// = "Batch";
                    dataGridView1.Columns["STARTDATE"].HeaderText = "Mulai Shift";
                    dataGridView1.Columns["CLOSEDATE"].HeaderText = "Tutup Shift";
                    dataGridView1.Columns["TERMINALID"].HeaderText = "Terminal ID";
                    dataGridView1.Columns["STOREID"].HeaderText = "Store ID";
                    dataGridView1.Columns["OPENBY"].HeaderText = "Open By";
                    dataGridView1.Columns["CLOSEBY"].HeaderText = "Close By";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get selected BatchId and TerminalId
                SelectedBatchId = dataGridView1.SelectedRows[0].Cells["BATCHID"].Value.ToString();
                SelectedTerminalId = dataGridView1.SelectedRows[0].Cells["TERMINALID"].Value.ToString();

                // Set DialogResult to OK to close the form and indicate success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a row.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close the form without selecting anything
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    
    }
}
