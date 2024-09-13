using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ItemSearch = Microsoft.Dynamics.Retail.Pos.BlankOperations.CheckStockForm.ItemSearch;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CheckStockForm
{
    public partial class MainFormCheckStock : Form
    {
        IPosTransaction posTransaction;
        IApplication application;
        public MainFormCheckStock(IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
            application = _application;

            //add event
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }

        

        private void addBtn_Click(object sender, EventArgs e)
        {
            // Open ItemSearch form
            ItemSearch itemSearchForm = new ItemSearch();
            itemSearchForm.ItemSelected += ItemSearchForm_ItemSelected;
            itemSearchForm.ShowDialog();
        }

        private void ItemSearchForm_ItemSelected(object sender, ItemSearch.ItemSelectedEventArgs e)
        {
            // Handle the selected SKU, e.g., add it to the DataGridView
            string selectedSKU = e.SelectedSKU;
            string selectedBarang = e.SelectedBarang;

            // Check if the SKU already exists in the DataGridView
            bool skuExists = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            { 
                // Assuming the SKU column is at index 0 (replace with the actual index)
                string existingSKU = row.Cells[0].Value.ToString();

                if (string.Equals(existingSKU, selectedSKU))
                {
                    // SKU already exists, set the flag and break out of the loop
                    skuExists = true;
                    break;
                }
            }

            // If SKU doesn't exist, add it to the DataGridView
            if (!skuExists)
            {
                dataGridView1.Rows.Add(selectedSKU, selectedBarang);
            }
             
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the delete button column
            if (e.ColumnIndex == dataGridView1.Columns["deleteBtn"].Index && e.RowIndex >= 0)
            {
                // Handle the delete button click for the specific row
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                dataGridView1.Rows.Remove(selectedRow);
                // Perform additional delete logic if needed
            }
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            string itemIdMulti = "";
            string siteId = "";
            string qtyMulti = "";

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Silakan add barang terlebih dahulu");
            }
            else
            {
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Check if the row is not the new row (if AllowUserToAddRows is true)
                    if (!row.IsNewRow)
                    {
                        // Assuming the SKU column index is 0, replace it with the actual index
                        string sku = row.Cells["SKU"].Value.ToString();

                        // Add the SKU to the itemIdMulti string with a semicolon separator
                        itemIdMulti += sku + ";";
                        qtyMulti += "0" + ";";
                    }
                }

                // Remove the trailing semicolon, if any
                if (!string.IsNullOrEmpty(itemIdMulti))
                {
                    itemIdMulti = itemIdMulti.TrimEnd(';');
                    qtyMulti = qtyMulti.TrimEnd(';');
                }

                // Now, itemIdMulti contains all SKUs separated by semicolons
                // You can use itemIdMulti as needed (e.g., display, process, etc.)
                //MessageBox.Show(itemIdMulti);
                getSiteWH(out  siteId);
                checkStockItem(itemIdMulti, qtyMulti, siteId);
            }


           
            
        }

        private void getSiteWH(out string siteId)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            siteId = "";
            string warehouseId = "";
            try
            {

                string queryString = @" SELECT A.INVENTLOCATION, A.INVENTLOCATIONDATAAREAID, C.INVENTSITEID 
                            FROM ax.RETAILCHANNELTABLE A, ax.RETAILSTORETABLE B, ax.INVENTLOCATION C
                            WHERE A.RECID=B.RECID AND C.INVENTLOCATIONID=A.INVENTLOCATION AND B.STORENUMBER=@STOREID";


                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            siteId = reader["INVENTSITEID"].ToString();
                            warehouseId = reader["INVENTLOCATION"].ToString();
                        }

                    }
                }

                //string queryString2 = @"SELECT ID.INVENTDIMID, ITEMID, CONFIGID FROM INVENTDIM ID JOIN INVENTITEMBARCODE IB ON ID.INVENTDIMID = IB.INVENTDIMID
                //                         WHERE ITEMID = @ITEMID";

            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        private void checkStockItem(string _itemIdMulti, string _qtyMulti, string _siteId)
        {
            string functionNameAX = "GetStockAX%"; // "GetStockAXPFMPOC"; //change to GetStockAX
            string functionNameAPI = "GetItemAPI";
            string messageBoxString = "";
            string xmlResponse;
            decimal remainQty = 0;
            int rowNumber = 0;
            bool findStockEmpty = false;
            var urlRTS = "";
            APIAccess.APIFunction apiFunction = new APIAccess.APIFunction();
            RetailTransaction transaction = posTransaction as RetailTransaction;
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            urlRTS = APIClass.getURLAPIByFuncName(functionNameAX);

            if (_itemIdMulti != "")
            {
                try
                {

                    bool statusTrans;
                    statusTrans = application.TransactionServices.CheckConnection();

                    if (statusTrans == true)
                    {
                        var result = apiFunction.checkStockOnHandMulti(application, urlRTS, application.Settings.Database.DataAreaID, _siteId, ApplicationSettings.Terminal.InventLocationId, _itemIdMulti, "", "", "", _qtyMulti, posTransaction.StoreId+"-FORMCHECKSTOCK"); //add 2 new parameter by Yonathan 11092024
                      //var result = apiFunction.checkStockOnHandMulti(Application, urlRTS, Application.Settings.Database.DataAreaID, siteId, ApplicationSettings.Terminal.InventLocationId, itemIdMulti, "", "", configIdMulti, quantityItems, transaction.TransactionId);
                        xmlResponse = result[3].ToString();

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlResponse);

                        XmlNodeList itemNodes = xmlDoc.SelectNodes("//StockListResult");



                        foreach (XmlNode node in itemNodes)
                        {
                            string qtyAvailString = node.Attributes["QtyAvail"].Value;
                            remainQty = Convert.ToDecimal(qtyAvailString.Replace(",", "."), CultureInfo.InvariantCulture);


                            dataGridView1.Rows[rowNumber].Cells["Stock"].Value = remainQty;

                            rowNumber++;


                        }
                    }

                    //ShowMsgBoxInformation(statusTrans.ToString());
                }
                catch(Exception ex)
                {
                    ShowMsgBoxInformation(ex.Message);
                }


                

            }
        }

        public static void ShowMsgBoxInformation(string text)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                // Assuming the column name is "Stock"
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Stock")
                {
                    DataGridViewCell cell = dataGridView1[e.ColumnIndex, e.RowIndex];

                    if (cell.Value != null && cell.Value != DBNull.Value)
                    {
                        decimal value = (decimal)cell.Value;

                        // Check if the value has decimal places
                        if (value == decimal.Truncate(value))
                        {
                            // No decimal places, format as whole number
                            e.Value = value.ToString("N0");
                        }
                        else
                        {
                            // Has decimal places, format as decimal
                            e.Value = value.ToString("N2"); // Adjust the format as needed
                        }
                    }
                }
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            // Clear all rows in the DataGridView
            dataGridView1.Rows.Clear();
        }

        private void MainFormCheckStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            clearBtn_Click(sender, e);
        }


    }
}
