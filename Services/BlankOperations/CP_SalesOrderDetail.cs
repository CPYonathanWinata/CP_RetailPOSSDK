using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSRetailPosis.POSControls.Touch;
using System.Xml;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using LSRetailPosis.Transaction;


namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CP_SalesOrderDetail : frmTouchBase
    {
        public IApplication Application;
        public CP_SalesOrderDetail(IApplication _application)
        {
            InitializeComponent();
            txtSalesOrder.Text = "SO/23/0000000286";
            
            Application = _application;
        }
        

        //private void buttin1_Click(object sender, EventArgs e)
        //{
        //    public ItemDetailsViewModel(CustomerOrderTransaction customerOrderTransaction)
        //    {
        //        this.SetTransaction(customerOrderTransaction);

        //        // Create a collection of LineItemViewModels from each SaleLineItem
        //        viewModels = (from lineItem in this.Transaction.SaleItems.Where(i => !i.Voided)
        //                      select new LineItemViewModel(lineItem)).ToList<LineItemViewModel>();

        //        this.lineItems = new ReadOnlyCollection<LineItemViewModel>(viewModels);
        //    }

        //    public ReadOnlyCollection<LineItemViewModel> Items
        //    {
        //        get { return lineItems; }
        //    }
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            // The XML string returned from the API
            ReadOnlyCollection<object> containerArray = Application.TransactionServices.Invoke("getSalesOrderDetail", txtSalesOrder.Text, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId);
            string xmlString = containerArray[3].ToString(); 
            //"<?xml version=\"1.0\" encoding=\"utf-8\"?><SalesTable SalesId=\"SO/23/0000000286\" RecId=\"5637666603\" SalesName=\"CUST KANVAS\" CustAccount=\"C000000003\"><SalesLine RecId=\"5647976870\" ItemId=\"11310034\" InventDimId=\"D/22/000005241\" SalesQty=\"3.00\" SalesDeliverNow=\"0.00\" InventBatchId=\"\" wmsLocationId=\"\" wmsPalletId=\"\" InventSiteId=\"JKT\" InventLocationId=\"WH_KUJHUB5\" ConfigId=\"\" InventSizeId=\"\" InventColorId=\"\" InventStyleId=\"\" InventSerialId=\"\" Guid=\"{AE2D6F5A-68A4-4E53-AAF4-98D4CB8CD092}\" UpdatedInAx=\"false\" Message=\"\" /><SalesLine RecId=\"5647976871\" ItemId=\"13030502\" InventDimId=\"D/22/000005241\" SalesQty=\"1.00\" SalesDeliverNow=\"0.00\" InventBatchId=\"\" wmsLocationId=\"\" wmsPalletId=\"\" InventSiteId=\"JKT\" InventLocationId=\"WH_KUJHUB5\" ConfigId=\"\" InventSizeId=\"\" InventColorId=\"\" InventStyleId=\"\" InventSerialId=\"\" Guid=\"{8E4788A6-39DE-4EA5-9AAD-19B08200B629}\" UpdatedInAx=\"false\" Message=\"\" /><SalesLine RecId=\"5647976869\" ItemId=\"11310014\" InventDimId=\"D/22/000005241\" SalesQty=\"2.00\" SalesDeliverNow=\"0.00\" InventBatchId=\"\" wmsLocationId=\"\" wmsPalletId=\"\" InventSiteId=\"JKT\" InventLocationId=\"WH_KUJHUB5\" ConfigId=\"\" InventSizeId=\"\" InventColorId=\"\" InventStyleId=\"\" InventSerialId=\"\" Guid=\"{52F58EED-F517-4B88-842E-1DE3A5FC3963}\" UpdatedInAx=\"false\" Message=\"\" /></SalesTable>";
            int numberLines = 0;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            // Create a new DataTable with two columns: ItemId and SalesQty
            DataTable table = new DataTable();

            table.Columns.Add("NO.");
            table.Columns.Add("ITEM ID");
            table.Columns.Add("QUANTITY SO");
            table.Columns.Add("QUANTITY RCV");

            // Loop through the SalesLine elements and extract the values of ItemId and SalesQty
            XmlNodeList salesLines = doc.GetElementsByTagName("SalesLine");
            foreach (XmlNode salesLine in salesLines)
            {
                numberLines++;
                string itemId = salesLine.Attributes["ItemId"].Value;
                string salesQty = salesLine.Attributes["SalesQty"].Value;

                // Add the pair of values to the DataTable
                table.Rows.Add(numberLines, itemId, salesQty);
            }

            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = table; 
            changeGridStyles();

            lblSalesOrder.Text = txtSalesOrder.Text;
            
        }
        private void changeGridStyles()
        {
            
            dataGridView1.Columns[0].Width = 55;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 600;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            this.dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.dataGridView1.Columns["STOCKCOUNTINGADJUST"].HeaderText = "Stock Adjusted (+/-)";

            //dataGridView1.Columns[0].Width = 90;
        }


        private void getSalesOrderDetails()
        {
            string salesId = txtSalesOrder.Text;
            ReadOnlyCollection<object> containerArray = Application.TransactionServices.Invoke("getSalesOrderDetail", salesId, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string valueDeliverNow = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            decimal resultValue;
            if (dataGridView1.Columns[e.ColumnIndex].Name == "DeliverNow")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && decimal.TryParse(valueDeliverNow, out resultValue))
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = resultValue.ToString("F2");
                }
            }
        }

        private void postBtn_Click(object sender, EventArgs e)
        {
            //List<string> values = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            // Create the XML declaration with version and encoding
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            // Create a new root element called "SalesTable" and add it to the XmlDocument object
            XmlElement root = xmlDoc.CreateElement("SalesTable");
            root.SetAttribute("SalesId", txtSalesOrder.Text);
            xmlDoc.AppendChild(root);

            // Loop through the rows of the DataGridView and add a new SalesLine element for each row
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Get the ItemId, SalesQty, and DeliverNow values from the DataGridView row
                string itemId = row.Cells["ItemId"].Value.ToString();
                string salesQty = row.Cells["SalesQty"].Value.ToString();
                string deliverNow = row.Cells["DeliverNow"].Value.ToString();

                // Create a new SalesLine element and add it to the root element of the XML document
                XmlElement salesLine = xmlDoc.CreateElement("SalesLine");
                salesLine.SetAttribute("ItemId", itemId);
                salesLine.SetAttribute("SalesQty", salesQty);
                salesLine.SetAttribute("SalesDeliverNow", deliverNow);
                root.AppendChild(salesLine);
            }

            // Save the modified XML document to a string
            string updatedXmlString = xmlDoc.OuterXml;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
