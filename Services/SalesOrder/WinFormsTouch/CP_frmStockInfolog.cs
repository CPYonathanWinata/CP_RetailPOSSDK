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

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    public partial class CP_frmStockInfolog : Form
    {
        private string itemId;
        private decimal remainQty;
        private string statusItem;
        private string itemName;
        public bool findStockEmpty { get; private set; }

        public CP_frmStockInfolog(string _itemNodes)//, RetailTransaction transaction)
        {
            findStockEmpty = false;
            InitializeComponent();
            InitializeGrid(_itemNodes);//, transaction);
        }
        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void InitializeGrid(string _itemNodes)//, RetailTransaction transaction)
        {
             // Split by '{}' and ignore empty strings
            string[] items = _itemNodes.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            string itemId = "";
            string itemName = "";
            decimal remainQty = 0;
            decimal qtyDo = 0;
            foreach (var item in items)
            {
                // Split each item by ';' to extract itemId, itemName, and availQty
                string[] parts = item.Split(';');
                if (parts.Length == 3)
                {
                    itemId = parts[0];
                    itemName = parts[1];
                    var culture = new CultureInfo("id-ID");
                    qtyDo = decimal.Parse(parts[2], culture);
                    //remainQty = int.Parse(parts[2]);
                    remainQty = decimal.Parse(parts[3], culture);
                    //remainQty = int.Parse(parts[2]);

                     
                }
            

                //var selectedSaleItem = transaction.SaleItems.FirstOrDefault(item => item.ItemId == node.Attributes["ItemId"].Value && item.Voided != true);
                //statusItem = remainQty - selectedSaleItem.Quantity < 0 ? "Tidak" : "Ya";


                ////for testing purpose
                //MessageBox.Show(remainQty + " - " + selectedSaleItem.Quantity);
                
                ////add for test


                gridViewItem.Rows.Add(itemId, itemName, qtyDo, remainQty);
                     
            }
                         
                Controls.Add(gridViewItem);
        }
           
        
    }
}
