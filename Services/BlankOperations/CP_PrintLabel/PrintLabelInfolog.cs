using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
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

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_PrintLabel
{
    public partial class Infolog : Form
    {
        private string itemId;
        private decimal remainQty;
        private string statusItem;
        private string itemName;
        public bool findStockEmpty { get; private set; }
        public IApplication Application { get; set; }
        //public event EventHandler<ItemSelectedEventArgs> ItemSelected;
        public event EventHandler<List<ItemData>> ItemsReady;

        public class ItemData
        {
            public string SKU { get; set; }
            public string Barang { get; set; }
            public string UnitId { get; set; }
            public string Barcode { get; set; }
            public decimal Price { get; set; }
            public decimal Disc { get; set; }
        }

        public Infolog(XmlDocument _xmlDoc, IApplication _application) 
        {
            //txtInfo.Text = "";
            XmlNodeList _itemNodes = _xmlDoc.SelectNodes("//Item");
            
            Application = _application;
            findStockEmpty = false;
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;

            this.ShowInTaskbar = false;
            gridViewItem.CellContentClick += gridViewItem_CellContentClick;

            InitializeGrid(_itemNodes);
        }

        private void gridViewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridViewItem.Columns["chkSelect"].Index && e.RowIndex >= 0)
            {
               
                bool currentValue = Convert.ToBoolean(gridViewItem.Rows[e.RowIndex].Cells["chkSelect"].Value ?? false);
                gridViewItem.Rows[e.RowIndex].Cells["chkSelect"].Value = !currentValue;
            }
        }

        private void InitializeGrid(XmlNodeList _itemNodes)
        {


            foreach (XmlNode node in _itemNodes)
            {
                string itemId = node["ItemRelation"] != null ? node["ItemRelation"].InnerText : "";
                string name = node["Name"] != null ? node["Name"].InnerText : "";
                string unit = node["UnitId"] != null ? node["UnitId"].InnerText : "";
                string amount = node["Amount"] != null ? node["Amount"].InnerText : "";
                string itemBarcode = node["ItemBarcode"] != null ? node["ItemBarcode"].InnerText : "";
                string discount = "0"; //node["Discount"] != null ? node["Discount"].InnerText : "";

                // Parse ke decimal
                decimal amountDec = decimal.Parse(amount);
                decimal discDec = decimal.Parse(discount);

                amount = amountDec.ToString("N0"); // "9500"
                discount = getDiscount(itemId, amountDec).ToString("N0");


                gridViewItem.Rows.Add(false, itemId, name, amount, discount, unit, itemBarcode);

                
            }

            Controls.Add(gridViewItem);


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

        // Method to set the value of findFalse
        public void setFindStockEmpty(bool value)
        {
            findStockEmpty = value;
        }
        private void okBtn_Click(object sender, EventArgs e)
        {

             var items = new List<ItemData>();
             foreach (DataGridViewRow row in gridViewItem.Rows)
             {
                 bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value ?? false);
                 if (isChecked)
                 {
                     // Misal ambil kolom "ItemID"
                     string itemId = row.Cells["ItemID"].Value.ToString();
                     Console.WriteLine("Checked: " + itemId);
                 }
             }
         
            foreach (DataGridViewRow row in gridViewItem.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value ?? false);
                if (isChecked)
                {
                    if (!row.IsNewRow)
                    {
                        var item = new ItemData
                        { //sku,barang,harga,unit
                            SKU = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : string.Empty,
                            Barang = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : string.Empty,
                            Price = row.Cells[3].Value != null ? Convert.ToDecimal(row.Cells[3].Value.ToString()) : 0m,
                            Disc = row.Cells[4].Value != null ? Convert.ToDecimal(row.Cells[4].Value.ToString()) : 0m,
                            UnitId = row.Cells[5].Value != null ? row.Cells[5].Value.ToString() : string.Empty,
                            Barcode = row.Cells[6].Value != null ? row.Cells[6].Value.ToString() : string.Empty,

                            //Barcode = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : string.Empty,

                            //Disc = row.Cells[5].Value != null ? Convert.ToDecimal(row.Cells[5].Value.ToString()) : 0m
                        };
                        items.Add(item);
                    }

                }
               
            
            }

            if (ItemsReady != null)
            {
                ItemsReady(this, items);
            }

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectAll_Click(object sender, System.EventArgs e)
        {
           
            bool allChecked = true;

            foreach (DataGridViewRow row in gridViewItem.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value ?? false);
                if (!isChecked)
                {
                    allChecked = false;
                    break;
                }
            }

         
            bool newValue = !allChecked;

            foreach (DataGridViewRow row in gridViewItem.Rows)
            {
                row.Cells["chkSelect"].Value = newValue;
            }
        }
        
    }
}
