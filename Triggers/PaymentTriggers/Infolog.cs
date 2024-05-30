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

namespace PaymentTriggers
{
    public partial class Infolog : Form
    {
        private string itemId;
        private decimal remainQty;
        private string statusItem;
        private string itemName;
        public bool findStockEmpty { get; private set; }

        public Infolog(XmlNodeList _itemNodes, RetailTransaction transaction)
        {
            findStockEmpty = false;
            InitializeComponent();
            InitializeGrid(_itemNodes, transaction);
        }

        private void InitializeGrid(XmlNodeList _itemNodes, RetailTransaction transaction)
        {
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
              

               
                var selectedSaleItem =  groupedSaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value);
                statusItem = remainQty - selectedSaleItem.TotalQuantity < 0 ? "Tidak" : "Ya";

                //var selectedSaleItem = transaction.SaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value && item.Voided != true);
                //statusItem = remainQty - selectedSaleItem.Quantity < 0 ? "Tidak" : "Ya";


                ////for testing purpose
                //MessageBox.Show(remainQty + " - " + selectedSaleItem.Quantity);
                
                ////add for test
                
                if (statusItem == "Tidak")
                {
                    itemName = selectedSaleItem.Description.PadRight(35); // Adjust the width as needed
                    gridViewItem.Rows.Add(itemId, itemName,selectedSaleItem.TotalQuantity, remainQty );
                    //messageBoxString += itemId + " | " + itemName + " | " + remainQty + "\n";
                    foreach (var item in transaction.SaleItems.Where(item => item.ItemId == itemId))
                    {
                        item.ShouldBeManuallyRemoved = true;
                    }
                    //selectedSaleItem.ShouldBeManuallyRemoved = true;
                    findStockEmpty = true;
                }
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
