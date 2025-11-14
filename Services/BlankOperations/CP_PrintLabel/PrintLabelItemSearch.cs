using Microsoft.Dynamics.Retail.Pos.Contracts;
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

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_PrintLabel
{
    public partial class ItemSearch : Form
    {
        

        public IApplication Application { get; set; }
        public event EventHandler<ItemSelectedEventArgs> ItemSelected;
        private System.Windows.Forms.Timer timer;
        private string currentText = string.Empty;
        public class ItemSelectedEventArgs : EventArgs
        {
            public string SelectedSKU { get; set;}
            public string SelectedBarang { get; set;}
            public string SelectedBarcode { get; set; }
            public decimal SelectedPrice { get; set; }
            public string SelectedUnitId { get; set; }
            public decimal SelectedDisc { get; set; }
            
            public ItemSelectedEventArgs(string selectedSKU, string selectedBarang, string selectedBarcode, string selectedUnitId, decimal selectedPrice, decimal selectedDisc)
            {
                SelectedSKU = selectedSKU;
                SelectedBarang = selectedBarang;
                SelectedBarcode = selectedBarcode;
                SelectedUnitId = selectedUnitId;
                SelectedPrice = selectedPrice;
                SelectedDisc = selectedDisc;

            }
        }
        public ItemSearch(IApplication application)
        
        
        {
            this.Application = application;
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.ControlBox = false;

            //this.ShowInTaskbar = false;
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
//                string queryString = @"SELECT	 
//                                            [DISPLAYPRODUCTNUMBER] as [SKU]
//                                            ,[NAME] as [Nama Barang]
//                                            ,[UNITID] as [Unit Id]
//                                       FROM [ax].[ECORESPRODUCT] ERP
//                                       INNER JOIN [AX].[ECORESPRODUCTTRANSLATION] EPT ON ERP.RECID = EPT.PRODUCT
//                                       INNER JOIN [AX].[INVENTTABLEMODULE] ITM ON ITM.ITEMID = ERP.[DISPLAYPRODUCTNUMBER]
//                                       WHERE  ITM.MODULETYPE = 2 AND [DISPLAYPRODUCTNUMBER] LIKE @ProductNumber OR [SEARCHNAME] LIKE @SearchName";

                string queryString = @"SELECT	 
                                            ERP.[DISPLAYPRODUCTNUMBER] AS [SKU],
                                            EPT.[NAME] AS [Nama Barang],
	                                        ISNULL(IIB.[ITEMBARCODE],'') AS [Barcode],
                                            ISNULL(ITM.[UNITID], '') AS [Unit Id]
                                        FROM [ax].[ECORESPRODUCT] ERP
                                        INNER JOIN [AX].[ECORESPRODUCTTRANSLATION] EPT 
                                            ON ERP.RECID = EPT.PRODUCT
                                        LEFT JOIN [AX].[INVENTTABLEMODULE] ITM 
                                            ON ITM.ITEMID = ERP.[DISPLAYPRODUCTNUMBER]
                                            AND ITM.MODULETYPE = 2
                                        LEFT JOIN [AX].[INVENTITEMBARCODE] IIB
	                                        ON IIB.ITEMID = ERP.[DISPLAYPRODUCTNUMBER]
                                        WHERE (ERP.[DISPLAYPRODUCTNUMBER] LIKE @ProductNumber OR [SEARCHNAME] LIKE @SearchName)
                                          AND EPT.LANGUAGEID = 'en-us'
                                        ORDER BY SKU ASC;";


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
                string selectedBarcode = itemGrid.SelectedRows[0].Cells["Barcode"].Value.ToString();
                string selectedUnitId = itemGrid.SelectedRows[0].Cells["Unit Id"].Value.ToString();
                decimal selectedPrice = Application.Services.Price.GetItemPrice(selectedSKU, selectedUnitId);
                decimal selectedDisc = getDiscount(selectedSKU, selectedPrice);
                if (ItemSelected != null)
                {
                    ItemSelected(this, new ItemSelectedEventArgs(selectedSKU, selectedBarang, selectedBarcode, selectedUnitId, selectedPrice, selectedDisc));
                }
                
            }

            // Close the form or perform any other necessary actions
           // this.Close();
        }

        public decimal getDiscount(string selectedSKU, decimal selectedPrice)
        {

            decimal pctDisc = 0;
            decimal amtDisc = 0;
            bool foundData = false;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string query = @"SELECT TOP 1 
                                    RP.ISDISCOUNTCODEREQUIRED,
                                    RP.STATUS,
                                    RPL.OFFERID,
                                    ERP.DISPLAYPRODUCTNUMBER,
                                    RPL.NAME,
                                    RDLO.DISCPCT,
                                    RDLO.DISCAMOUNT,
                                    RDLO.OFFERPRICE,
                                    RP.VALIDFROM,
                                    RP.VALIDTO
                                FROM RETAILPERIODICDISCOUNT RP
                                INNER JOIN RETAILPERIODICDISCOUNTLINE RPL
                                    ON RP.OFFERID = RPL.OFFERID
                                INNER JOIN RETAILGROUPMEMBERLINE RGM
                                    ON RPL.RETAILGROUPMEMBERLINE = RGM.RECID
                                INNER JOIN ECORESPRODUCT ERP
                                    ON RGM.PRODUCT = ERP.RECID
                                INNER JOIN RETAILDISCOUNTLINEOFFER RDLO
                                    ON RDLO.RECID = RPL.RECID
                                WHERE RP.STATUS = 1
                                  AND RP.ISDISCOUNTCODEREQUIRED = 0
                                  AND GETDATE() BETWEEN RP.VALIDFROM AND RP.VALIDTO
                                  AND ERP.DISPLAYPRODUCTNUMBER = @ItemId
                                ORDER BY RP.VALIDFROM DESC;

                                ";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", selectedSKU);
                    //command.Parameters.AddWithValue("@SearchName", itemString);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            pctDisc = reader["DISCPCT"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DISCPCT"]);
                            amtDisc = reader["DISCAMOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DISCAMOUNT"]);

                            foundData = true;
                        }
                    }
                }

                
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

            if (foundData == true)
            {
                return CalculateDiscountedPrice(selectedPrice, pctDisc, amtDisc);

            }
            else
            { 
                return 0; 
            }
        }

        decimal CalculateDiscountedPrice(decimal price, decimal pctDisc, decimal amtDisc)
        {
            decimal totalDisc = 0m;

            if (pctDisc > 0)
                totalDisc += price * (pctDisc / 100);

            if (amtDisc > 0)
                totalDisc += amtDisc;

            if (totalDisc > price)
                totalDisc = price;

            return price - totalDisc;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    
    }
}
