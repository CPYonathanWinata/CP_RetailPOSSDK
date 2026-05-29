using LSRetailPosis.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace PaymentTriggers
{
    public partial class Infolog : Form
    {
        private string itemId;
        private decimal remainQty;
        private string statusItem;
        private string itemName;
        public bool findStockEmpty { get; private set; }
        string labelInfo;
        private string itemIdMulti;

        public Infolog(XmlNodeList _itemNodes = null, RetailTransaction transaction = null, string _itemIdString = "", string _infoLabel = "1")
        {
            findStockEmpty = false;
            labelInfo = _infoLabel;

            if (_itemIdString != "")
            {
                itemId = _itemIdString;
            }
            
            InitializeComponent();
            InitializeGrid(_itemNodes, transaction);

            
        }

        public class ItemSummary
        {
            public string ItemId { get; set; }
            public decimal Qty { get; set; }
        }

        public static List<ItemSummary> ParseAndSummarize(string hex)
        {
            
            if (hex.StartsWith("0x"))
                hex = hex.Substring(2);

           
            byte[] bytes = Enumerable.Range(0, hex.Length / 2)
                .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                .ToArray();
 
            string xmlString = Encoding.UTF8.GetString(bytes);

            
            XDocument doc = XDocument.Parse(xmlString);

            
            XNamespace ns = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.Commerce.Runtime.DataModel";

            // 5. ambil SalesLines
            var items = doc
                .Descendants(ns + "SalesLine")
                .Select(x => new
                {
                    ItemId = (string)x.Element(ns + "ItemId"),
                    Qty = (decimal?)x.Element(ns + "Quantity") ?? 0
                })
                .Where(x => !string.IsNullOrEmpty(x.ItemId))
                .ToList();

            // 6. grouping
            var result = items
                .GroupBy(x => x.ItemId)
                .Select(g => new ItemSummary
                {
                    ItemId = g.Key,
                    Qty = g.Sum(x => x.Qty)
                })
                .ToList();

            return result;
        }

        public decimal getDataSuspend()
        {
            return 1;
        }
       
        private void InitializeGrid(XmlNodeList _itemNodes, RetailTransaction transaction)
        {
            if (labelInfo == "1")
            {
                infoLbl.Text = "Stok barang di bawah ini tidak mencukupi untuk ditransaksikan.\nSilakan hapus atau kurangi jumlahnya agar tidak melebihi stok tersedia";

                var groupedSaleItems = transaction.SaleItems
              .Where(item => !item.Voided)
              .GroupBy(item => item.ItemId)
              .Select(group => new
              {
                  ItemId = group.Key,
                  TotalQuantity = group.Sum(item => item.Quantity),
                  Description = group.First().Description
              });



                foreach (XmlNode node in _itemNodes)
                {
                    itemId = node.Attributes["ItemId"].Value;


                    remainQty = Convert.ToDecimal(node.Attributes["QtyAvail"].Value.Replace(",", "."), CultureInfo.InvariantCulture);



                    var selectedSaleItem = groupedSaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value);
                    statusItem = remainQty - selectedSaleItem.TotalQuantity < 0 ? "Tidak" : "Ya";

                    //var selectedSaleItem = transaction.SaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value && item.Voided != true);
                    //statusItem = remainQty - selectedSaleItem.Quantity < 0 ? "Tidak" : "Ya";


                    ////for testing purpose
                    //MessageBox.Show(remainQty + " - " + selectedSaleItem.Quantity);

                    ////add for test

                    if (statusItem == "Tidak") 
                    {
                        itemName = selectedSaleItem.Description.PadRight(35); // Adjust the width as needed
                        gridViewItem.Rows.Add(itemId, itemName, selectedSaleItem.TotalQuantity, remainQty);
                        //messageBoxString += itemId + " | " + itemName + " | " + remainQty + "\n";
                        foreach (var item in transaction.SaleItems.Where(item => item.ItemId == itemId))
                        {
                            item.ShouldBeManuallyRemoved = true;
                        }
                        //selectedSaleItem.ShouldBeManuallyRemoved = true;
                        findStockEmpty = true;
                    }
                }
            }
            else if (labelInfo == "2")
            {
                infoLbl.Text = "Barang di bawah ini tidak dapat ditransaksikan karena tidak ada di tabel\nCPITEMONHANDSTATUS. Silakan hubungi IT Support untuk sync";

                var groupedSaleItems = transaction.SaleItems
                                        .Where(item => !item.Voided)
                                        .GroupBy(item => item.ItemId)
                                        .Select(group => new
                                        {
                                            ItemId = group.Key,
                                            TotalQuantity = group.Sum(item => item.Quantity),
                                            Description = group.First().Description
                                        });

                //itemIdNotList = string.Join(";", notListItemIds);
                //breakdown the itemid
                //node.Attributes["ItemId"].Value;

                string[] items = itemId.Split(';');
                this.gridViewItem.Columns["RemainQty"].Visible = false;
                foreach (string itemSingle in items)
                {

                    var selectedSaleItem = groupedSaleItems.FirstOrDefault(item => item.ItemId == itemSingle.ToString());
                    itemName = selectedSaleItem.Description.PadRight(35);
                    if (!string.IsNullOrWhiteSpace(itemSingle))
                    {
                        gridViewItem.Rows.Add(itemSingle.Trim(), itemName, selectedSaleItem.TotalQuantity);

                        foreach (var item in transaction.SaleItems.Where(item => item.ItemId == itemSingle))
                        {
                            item.ShouldBeManuallyRemoved = true;
                        }
                        findStockEmpty = true;
                    }
                }
                                
                //remainQty = Convert.ToDecimal(node.Attributes["QtyAvail"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
                //var selectedSaleItem = groupedSaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value);
                //statusItem = remainQty - selectedSaleItem.TotalQuantity < 0 ? "Tidak" : "Ya";

                ////var selectedSaleItem = transaction.SaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value && item.Voided != true);
                ////statusItem = remainQty - selectedSaleItem.Quantity < 0 ? "Tidak" : "Ya";


                //////for testing purpose
                ////MessageBox.Show(remainQty + " - " + selectedSaleItem.Quantity);

                //////add for test

                //if (statusItem == "Tidak")
                //{
                //    itemName = selectedSaleItem.Description.PadRight(35); // Adjust the width as needed
                //    gridViewItem.Rows.Add(itemId, itemName, selectedSaleItem.TotalQuantity, remainQty);
                //    //messageBoxString += itemId + " | " + itemName + " | " + remainQty + "\n";
                //    foreach (var item in transaction.SaleItems.Where(item => item.ItemId == itemId))
                //    {
                //        item.ShouldBeManuallyRemoved = true;
                //    }
                //    //selectedSaleItem.ShouldBeManuallyRemoved = true;
                //    findStockEmpty = true;
                //}


            }

           

            if (findStockEmpty == true)
            {
                Controls.Add(gridViewItem);
            }
           

            

            
        }
        // Method to set the value of findFalse
        public void setFindStockEmpty(bool value)
        {
            findStockEmpty = value;
        }
        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
