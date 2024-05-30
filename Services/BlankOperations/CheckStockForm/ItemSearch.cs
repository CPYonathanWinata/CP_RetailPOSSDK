using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CheckStockForm
{
    public partial class ItemSearch : Form
    {
        public event EventHandler<ItemSelectedEventArgs> ItemSelected;
        private System.Windows.Forms.Timer timer;
        private string currentText = string.Empty;
        public class ItemSelectedEventArgs : EventArgs
        {
            public string SelectedSKU { get; set;}
            public string SelectedBarang { get; set;}

            public ItemSelectedEventArgs(string selectedSKU, string selectedBarang)
            {
                SelectedSKU = selectedSKU;
                SelectedBarang = selectedBarang;
            }
        }
        public ItemSearch()
        {
            InitializeComponent();

            // Initialize the timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 500; // Set the interval to 1000 milliseconds (1 second)
            timer.Tick += Timer_Tick;

            //add Event
            itemGrid.CellDoubleClick += dataGridView1_CellDoubleClick;
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the double-clicked cell is in a valid row
            if (e.RowIndex >= 0 && e.RowIndex < itemGrid.Rows.Count)
            {
                // Call the desired method, e.g., submitBtn_Click
                submitBtn_Click(sender, e);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Get the text from the searchBox and call the queryData method
            string itemString = searchBox.Text;
            queryData(itemString);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Update the currentText variable
            currentText = searchBox.Text;

            // Restart the timer every time the text changes
            timer.Stop();
            timer.Start();
        }

        private void queryData(string itemString)
        {
            //string connectionString = "Data Source=DYNAMICS01\\DEVPRISQLSVR;Initial Catalog=JTJDRN1StoreDev;Integrated Security=True;Persist Security Info=False;Pooling=True;Encrypt=True;TrustServerCertificate=True;Application Name=\"Microsoft Dynamics AX for Retail POS\"";
            itemString = "%" + itemString + "%";
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @"SELECT	 
                                            [DISPLAYPRODUCTNUMBER] as [SKU]
                                            ,[NAME] as [Nama Barang]
                                       FROM [ax].[ECORESPRODUCT] ERP
                                       INNER JOIN [AX].[ECORESPRODUCTTRANSLATION] EPT ON ERP.RECID = EPT.PRODUCT
                                       WHERE [DISPLAYPRODUCTNUMBER] LIKE @ProductNumber OR [SEARCHNAME] LIKE @SearchName";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@ProductNumber", itemString);
                    command.Parameters.AddWithValue("@SearchName", itemString);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Clear existing data and columns in the DataGridView
                        itemGrid.Rows.Clear();
                        itemGrid.Columns.Clear();

                        // Add columns to DataGridView dynamically
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            itemGrid.Columns.Add(reader.GetName(i), reader.GetName(i));
                            
                        }

                        // Check if there are rows returned from the query
                        while (reader.Read())
                        {
                            // Create an array to hold values for each column
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);

                            // Add a new row to the DataGridView with the retrieved values
                            itemGrid.Rows.Add(values);
                        }
                    }
                }

                itemGrid.Columns[1].Width = 360;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            // Get the selected SKU from the selected row
            if (itemGrid.SelectedRows.Count > 0)
            {
                string selectedSKU = itemGrid.SelectedRows[0].Cells["SKU"].Value.ToString();
                string selectedBarang = itemGrid.SelectedRows[0].Cells["Nama Barang"].Value.ToString();

                if (ItemSelected != null)
                {
                    ItemSelected(this, new ItemSelectedEventArgs(selectedSKU, selectedBarang));
                }
                
            }

            // Close the form or perform any other necessary actions
           // this.Close();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    
    }
}
