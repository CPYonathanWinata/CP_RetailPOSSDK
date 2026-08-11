using LSRetailPosis.POSControls;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CPPricingSimulator
{
    public partial class PricingSimulator : Form
    {
        IPosTransaction localPosTransaction;
        IApplication localApplication;
        IPosTransaction posTransaction;
        RetailTransaction dummyRetailTransaction;

        int wItemId = 12;
        int wDescription = 40; // increased to fit longer descriptions
        int wVariantId = 12;
        int wQty = 10;
        int wOriginalPrice = 14;
        int wNetAmount = 14;
        int wDiscount = 14;


        public PricingSimulator(IPosTransaction _posTransaction, IApplication _application)
        {
            localApplication = _application;
            posTransaction = _posTransaction;

            //Create a list for item´s to be removed
            LinkedList<SaleLineItem> newSaleLinesList = new LinkedList<SaleLineItem>();
            //PosTransaction posTransDummy = new PosTransaction();

            dummyRetailTransaction = new RetailTransaction(ApplicationSettings.Terminal.StoreId, "IDR", ApplicationSettings.Terminal.TaxIncludedInPrice, localApplication.Services.Rounding);

            //dummyRetailTransaction = localPosTransaction as RetailTransaction;
            dummyRetailTransaction.OperatorId = posTransaction.OperatorId;
            dummyRetailTransaction.OperatorName = posTransaction.OperatorName;
            dummyRetailTransaction.OperatorNameOnReceipt = posTransaction.OperatorNameOnReceipt;
            localPosTransaction = dummyRetailTransaction as PosTransaction;

            InitializeComponent();
            SetupGridColumns(); 
           
        }

        private void SetupGridColumns()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "ItemId", HeaderText = "ItemId", DataPropertyName = "ItemId" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Description", DataPropertyName = "Description" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "VariantId", HeaderText = "VariantId", DataPropertyName = "VariantId" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Qty", HeaderText = "Qty", DataPropertyName = "QuantityOrdered" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "OrigPrice", HeaderText = "OrigPrice", DataPropertyName = "OriginalPrice" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NetAmount", HeaderText = "NetAmount", DataPropertyName = "NetAmount" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Discount", HeaderText = "Discount", DataPropertyName = "PeriodicDiscount" });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "EditQty",
                HeaderText = "",
                Text = "Edit Qty",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Width = 70
            });
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // header row click, ignore

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            string itemId = dataGridView1.Rows[e.RowIndex].Cells["ItemId"].Value.ToString();

            if (columnName == "EditQty")
            {
                EditQtyForItem(itemId);
            }
            else if (columnName == "Delete")
            {
                DeleteItem(itemId);
            }
        }

        private void DeleteItem(string itemId)
        {
            var confirm = MessageBox.Show("Remove item {itemId}?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            var item = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemId);
            if (item == null) return;

            var node = dummyRetailTransaction.SaleItems.Find(item);
            dummyRetailTransaction.SaleItems.Remove(node);

            POSFormsManager.ShowPOSStatusPanelText("");
            localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);

            RefreshGrid();
        }

        private void EditQtyForItem(string itemId)
        {
            var item = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemId);
            if (item == null) return;

            decimal currentQty = item.Quantity;
            decimal newQty;
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter new quantity for {itemId}:", "Edit Quantity", currentQty.ToString());

            if (string.IsNullOrWhiteSpace(input)) return;


            if (!decimal.TryParse(input, out newQty) || newQty <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than 0.");
                return;
            }
            var firstItem = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemId);

            if (firstItem != null)
            {
                var firstNode = dummyRetailTransaction.SaleItems.Find(firstItem);
                var node = firstNode.Next;

                while (node != null)
                {
                    var next = node.Next; // save reference before removing

                    if (node.Value.ItemId == itemId)
                    {
                        decimal qtyToMove = node.Value.Quantity;
                        dummyRetailTransaction.SaleItems.Remove(node);
                        firstNode.Value.Quantity += qtyToMove;
                    }

                    node = next;
                }

                firstNode.Value.Quantity = newQty;
                //firstNode.Value.QuantityOrdered = newQty; // keep QuantityOrdered in sync too, since your grid displays this field
            }

            POSFormsManager.ShowPOSStatusPanelText("");
            localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);

            //foreach
            // Refresh the grid
          

            RefreshGrid();
        }

        //private void EditQtyForItem(string itemId)
        //{
        //    var item = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemId);
        //    if (item == null) return;

        //    decimal currentQty = item.Quantity;

        //    NumericKeypadForm keypad = new NumericKeypadForm(
        //        "Enter the quantity for " + itemId, "Edit Quantity", currentQty.ToString());

        //    if (keypad.ShowDialog() != DialogResult.OK)
        //    {
        //        return; // cancelled
        //    }

        //    string input = keypad.ResultValue;

        //    if (string.IsNullOrWhiteSpace(input))
        //    {
        //        return;
        //    }

        //    decimal newQty;
        //    if (!decimal.TryParse(input.Replace(",", "."), out newQty) || newQty <= 0)
        //    {
        //        MessageBox.Show("Please enter a valid quantity greater than 0.");
        //        return;
        //    }

        //    var node = dummyRetailTransaction.SaleItems.Find(item);
        //    node.Value.Quantity = newQty;

        //    POSFormsManager.ShowPOSStatusPanelText("");
        //    localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);

        //    RefreshGrid();
        //}

        private void RefreshGrid()
        {
            string selectedItemId = null;

            if (dataGridView1.CurrentRow != null)
            {
                selectedItemId = dataGridView1.CurrentRow.Cells["ItemId"].Value.ToString();
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dummyRetailTransaction.SaleItems.ToList();

            if (selectedItemId != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    if (row.Cells["ItemId"].Value != null && row.Cells["ItemId"].Value.ToString() == selectedItemId)
                    {
                        dataGridView1.CurrentCell = row.Cells["ItemId"];
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int quantityToAdd = 1; //random.Next(1, 11);

            string itemId = "11310014";

            var matchingLines = dummyRetailTransaction.SaleItems.Where(line => line.ItemId == itemId).ToList();
            LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();

            if (matchingLines.Count == 0)
            {
                iSale.OperationID = PosisOperations.ItemSale;
                iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                iSale.Barcode = itemId;
                iSale.OperationInfo.NumpadQuantity = quantityToAdd;
                iSale.POSTransaction = (PosTransaction)localPosTransaction;

                iSale.RunOperation();
                POSFormsManager.ShowPOSStatusPanelText("");
                localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);
            }
            else
            {
                for (int i = 0; i < ((RetailTransaction)localPosTransaction).SaleItems.Count; i++)
                {
                    LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = dummyRetailTransaction.GetItem(((RetailTransaction)localPosTransaction).SaleItems.ElementAt(i).LineId);
                    int lineId = ((RetailTransaction)localPosTransaction).SaleItems.ElementAt(i).LineId;
                    if (currentLine.ItemId == itemId)
                    {
                        currentLine.QuantityOrdered = currentLine.QuantityOrdered + 1;
                        currentLine.Quantity = currentLine.QuantityOrdered;
                    }
                }
                POSFormsManager.ShowPOSStatusPanelText("");
                localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);
            }

            // Refresh the grid with the latest data
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dummyRetailTransaction.SaleItems.ToList();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{

        //    textBox1.Text = "";
          



        //    Random random = new Random();
        //    int quantityToAdd = 1; //random.Next(1, 11);
           
        //    //localPosTransaction.OperatorId = posTransaction.OperatorId;
        //    //localPosTransaction.OperatorName = posTransaction.OperatorName;
        //    //localPosTransaction.OperatorNameOnReceipt = posTransaction.OperatorNameOnReceipt;
          


        //    //LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();

        //    string itemId = "11310014";


        //    //var searchedItem = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemid);
        //    var matchingLines = dummyRetailTransaction.SaleItems.Where(line => line.ItemId == itemId).ToList(); 
        //    LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
        //    if (matchingLines.Count == 0)               
        //    {
        //        iSale.OperationID = PosisOperations.ItemSale;
        //        iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
        //        //iSale.Barcode = skuId; disable by Yonathan 21/10/2022
        //        iSale.Barcode = itemId;  // change to this by yonathan 21/10/2022
        //        //iSale.BarcodeInfo.ItemId = txtSKU.Text;
        //        iSale.OperationInfo.NumpadQuantity = quantityToAdd;
        //        iSale.POSTransaction = (PosTransaction)localPosTransaction;

        //        iSale.RunOperation();
        //        POSFormsManager.ShowPOSStatusPanelText("");
        //        localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);


        //        textBox1.Text += new string('-', wItemId + wDescription + wVariantId + wQty + wOriginalPrice + wNetAmount + wDiscount + 14) + "\r\n";

        //    }
        //    else
        //    {


        //        for (int i = 0; i < ((RetailTransaction)localPosTransaction).SaleItems.Count; i++)
        //        {
        //            LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = dummyRetailTransaction.GetItem(((RetailTransaction)localPosTransaction).SaleItems.ElementAt(i).LineId);
        //            int lineId = ((RetailTransaction)localPosTransaction).SaleItems.ElementAt(i).LineId;
        //            if (currentLine.ItemId == itemId)
        //            {
        //                currentLine.QuantityOrdered =  currentLine.QuantityOrdered + 1;
        //                currentLine.Quantity = currentLine.QuantityOrdered;// +qtySelected;
        //            }

        //        }
        //        POSFormsManager.ShowPOSStatusPanelText("");
        //        localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);


        //        //textBox1.Text += new string('-', wItemId + wQty + wDiscount + wTotal + 8) + "\r\n";
        //    }


        //    textBox1.Text = "";
        //    textBox1.Text = "ItemId".PadRight(wItemId) + "| " +
        //        "Description".PadRight(wDescription) + "| " +
        //        "VariantId".PadRight(wVariantId) + "| " +
        //        "Qty".PadRight(wQty) + "| " +
        //        "OrigPrice".PadRight(wOriginalPrice) + "| " +
        //        "NetAmount".PadRight(wNetAmount) + "| " +
        //        "Discount".PadRight(wDiscount) + "\r\n";


        //    foreach (var item in dummyRetailTransaction.SaleItems)
        //    {
        //        string desc = item.Description.Length > wDescription - 1
        //                    ? item.Description.Substring(0, wDescription - 1)
        //                    : item.Description;

        //        textBox1.Text += item.ItemId.PadRight(wItemId) + "| " +
        //                          item.Description.PadRight(wDescription) + "| " +
        //                          item.BarcodeId.PadRight(wVariantId) + "| " +
        //                          item.QuantityOrdered.ToString().PadRight(wQty) + "| " +
        //                          item.OriginalPrice.ToString("N2").PadRight(wOriginalPrice) + "| " +
        //                          item.NetAmount.ToString("N2").PadRight(wNetAmount) + "| " +
        //                          item.PeriodicDiscount.ToString("N2").PadRight(wDiscount) + "\r\n";
        //    }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }


        //add QTY
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return; // no row selected
            }

            // Get the ItemId from the selected row
            string itemId = dataGridView1.CurrentRow.Cells["ItemId"].Value.ToString();
            decimal qtyToAdd = 1;

            // Find the first occurrence (this is "Line 1" - the one we keep)
            var firstItem = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemId);

            if (firstItem != null)
            {
                var firstNode = dummyRetailTransaction.SaleItems.Find(firstItem);
                var node = firstNode.Next;

                while (node != null)
                {
                    var next = node.Next; // save reference before removing

                    if (node.Value.ItemId == itemId)
                    {
                        decimal qtyToMove = node.Value.Quantity;
                        dummyRetailTransaction.SaleItems.Remove(node);
                        firstNode.Value.Quantity += qtyToMove;
                    }

                    node = next;
                }

                firstNode.Value.Quantity += qtyToAdd;
                firstNode.Value.QuantityOrdered += qtyToAdd; // keep QuantityOrdered in sync too, since your grid displays this field
            }

            POSFormsManager.ShowPOSStatusPanelText("");
            localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);

            // Refresh the grid
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dummyRetailTransaction.SaleItems.ToList();
        }
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    string itemId = "11310014";
        //    decimal qtyToAdd = 1; // the new quantity being added

        //    // Find the first occurrence (this is "Line 1" - the one we keep)
        //    var firstItem = dummyRetailTransaction.SaleItems.FirstOrDefault(x => x.ItemId == itemId);

        //    if (firstItem != null)
        //    {
        //        var firstNode = dummyRetailTransaction.SaleItems.Find(firstItem);
        //        var node = firstNode.Next;

        //        while (node != null)
        //        {
        //            var next = node.Next; // save reference before removing

        //            if (node.Value.ItemId == itemId)
        //            {
        //                // 1. Store qty of the duplicate line
        //                decimal qtyToMove = node.Value.Quantity;

        //                // 2. Remove the duplicate line
        //                dummyRetailTransaction.SaleItems.Remove(node);

        //                // 3. Merge qty into Line 1
        //                firstNode.Value.Quantity += qtyToMove;
        //            }

        //            node = next;
        //        }

        //        // 4. Add the new incoming qty on top
        //        firstNode.Value.Quantity += qtyToAdd;
        //    }
        //    POSFormsManager.ShowPOSStatusPanelText("");
        //    localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);

        //    textBox1.Text = "";
        //    textBox1.Text = "ItemId".PadRight(wItemId) + "| " +
        //        "Description".PadRight(wDescription) + "| " +
        //        "VariantId".PadRight(wVariantId) + "| " +
        //        "Qty".PadRight(wQty) + "| " +
        //        "OrigPrice".PadRight(wOriginalPrice) + "| " +
        //        "NetAmount".PadRight(wNetAmount) + "| " +
        //        "Discount".PadRight(wDiscount) + "\r\n";


        //    foreach (var item in dummyRetailTransaction.SaleItems)
        //    {
        //        string desc = item.Description.Length > wDescription - 1
        //                    ? item.Description.Substring(0, wDescription - 1)
        //                    : item.Description;

        //        textBox1.Text += item.ItemId.PadRight(wItemId) + "| " +
        //              desc.PadRight(wDescription) + "| " +
        //              item.BarcodeId.PadRight(wVariantId) + "| " +
        //              item.QuantityOrdered.ToString().PadRight(wQty) + "| " +
        //              item.OriginalPrice.ToString("N2").PadRight(wOriginalPrice) + "| " +
        //              item.NetAmount.ToString("N2").PadRight(wNetAmount) + "| " +
        //              item.PeriodicDiscount.ToString("N2").PadRight(wDiscount) + "\r\n";
        //    }
        //}

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            // Open ItemSearch form
            ItemSearch itemSearchForm = new ItemSearch(localApplication);
            itemSearchForm.ItemSelected += ItemSearchForm_ItemSelected;
            //itemSearchForm.ShowDialog();


            itemSearchForm.StartPosition = FormStartPosition.Manual;


            itemSearchForm.Location = new Point(
                this.Location.X + (this.Width - itemSearchForm.Width) / 2, // center horizontal
                this.Location.Y + 300
            );

            // Tampilkan dialog
            itemSearchForm.ShowDialog(this);
        }

        private void ItemSearchForm_ItemSelected(object sender, ItemSearch.ItemSelectedEventArgs e)
        {
            // Handle the selected SKU, e.g., add it to the DataGridView
            string selectedSKU = e.SelectedSKU;
            string selectedBarang = e.SelectedBarang;
            string selectedBarcode = e.SelectedBarcode;
            string selectedUnitId = e.SelectedUnitId;
            decimal selectedPrice = e.SelectedPrice;
            decimal selectedDisc = e.SelectedDisc;
            //add period 04122025
            string selectedDiscFrom = e.SelectedDiscFrom;
            string selectedDiscTo = e.SelectedDiscTo;
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
              
                int quantityToAdd = 1; //random.Next(1, 11);



                var matchingLines = dummyRetailTransaction.SaleItems.Where(line => line.ItemId == selectedSKU).ToList();
                LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();

                if (matchingLines.Count == 0)
                {
                    iSale.OperationID = PosisOperations.ItemSale;
                    iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                    iSale.Barcode = selectedSKU;
                    iSale.OperationInfo.NumpadQuantity = quantityToAdd;
                    iSale.POSTransaction = (PosTransaction)localPosTransaction;

                    iSale.RunOperation();
                    POSFormsManager.ShowPOSStatusPanelText("");
                    localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);
                }
                else
                {
                    for (int i = 0; i < ((RetailTransaction)localPosTransaction).SaleItems.Count; i++)
                    {
                        LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = dummyRetailTransaction.GetItem(((RetailTransaction)localPosTransaction).SaleItems.ElementAt(i).LineId);
                        int lineId = ((RetailTransaction)localPosTransaction).SaleItems.ElementAt(i).LineId;
                        if (currentLine.ItemId == selectedSKU)
                        {
                            currentLine.QuantityOrdered = currentLine.QuantityOrdered + 1;
                            currentLine.Quantity = currentLine.QuantityOrdered;
                        }
                    }
                    POSFormsManager.ShowPOSStatusPanelText("");
                    localApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(dummyRetailTransaction);
                }

                // Refresh the grid with the latest data
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dummyRetailTransaction.SaleItems.ToList();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        
    }
}
