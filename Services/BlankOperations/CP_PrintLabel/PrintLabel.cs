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
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ZXing;
using ZXing.Common;
using ItemSearch = Microsoft.Dynamics.Retail.Pos.BlankOperations.CheckStockForm.ItemSearch;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_PrintLabel
{
    public partial class MainFormPrintLabel : Form
    {
        IPosTransaction posTransaction;
        IApplication application;
        List<string> listItemId = new List<string>();
        List<string> listItemName = new List<string>();
        List<string> listItemBarcode = new List<string>();
        List<string> listUnitId = new List<string>();
        List<decimal> listPrice = new List<decimal>();
        List<decimal> listDisc = new List<decimal>();
        string xmlLayout = "";
        string barcode = "";
        int offset = 0;
        List<XElement> lines;
        bool findNewPrice;
        int currentPrintIndex = 0;
        bool feedLines = false;
        public MainFormPrintLabel(IPosTransaction _posTransaction, IApplication _application)
        {
            //findNewPrice = true;
            InitializeComponent();
            posTransaction = _posTransaction;
            application = _application;
             
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
 
            this.ShowInTaskbar = false;
            //add event
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            checkNewPrice();
            
        }

        

        private void checkNewPrice()
        {
            //application.Services.Price.GetItemPrice().
            
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            //DateTime dateParam = DateTime.ParseExact("2023-08-30", "yyyy-MM-dd", CultureInfo.InvariantCulture); DateTime.Now.ToShortDateString(); //DateTime.ParseExact("2023-08-30", "yyyy-MM-dd", CultureInfo.InvariantCulture);  //DateTime.Now;
                //DateTime.ParseExact("2025-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dateString = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dateParam = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Step 2: Convert to the format your database expects
            string formattedDate = dateParam.ToString("yyyy-MM-dd");

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("Items");
            xmlDoc.AppendChild(root);

            try
            {

                string queryString = @"SELECT 
                                        GROUPID, 
                                        INVENTLOCATION, 
                                        ITEMRELATION, 
                                        UNITID, 
                                        ITEMBARCODE,
                                        FROMDATE, 
                                        AMOUNT,  
                                        NAME
                                    FROM (
                                        SELECT 
                                            PDG.GROUPID,
                                            RCT.INVENTLOCATION,
                                            PDT.ITEMRELATION,
                                            PDT.UNITID,
                                            IIB.ITEMBARCODE, 
                                            PDT.FROMDATE,
                                            PDT.AMOUNT,        
                                            ERP.SEARCHNAME AS NAME,
		
                                            ROW_NUMBER() OVER (
                                                PARTITION BY PDT.ITEMRELATION
                                                ORDER BY PDT.FROMDATE DESC
                                            ) AS rn
                                        FROM RETAILCHANNELTABLE RCT
                                        INNER JOIN RETAILCHANNELPRICEGROUP RCP
                                            ON RCP.RETAILCHANNEL = RCT.RECID
                                        INNER JOIN PRICEDISCGROUP PDG
                                            ON RCP.PRICEGROUP = PDG.RECID
                                        INNER JOIN PRICEDISCTABLE PDT
                                            ON PDT.ACCOUNTRELATION = PDG.GROUPID
                                        LEFT JOIN INVENTITEMBARCODE IIB
                                            ON IIB.ITEMID = PDT.ITEMRELATION
                                            AND IIB.UNITID = PDT.UNITID
                                        LEFT JOIN ECORESPRODUCT ERP
                                            ON ERP.DISPLAYPRODUCTNUMBER = PDT.ITEMRELATION
                                        WHERE RCT.INVENTLOCATION = @INVENTLOCATION
                                    ) t
                                    WHERE rn = 1
                                    AND FROMDATE = @DATE
                                    ORDER BY FROMDATE DESC;
                                    ";

                //System.Diagnostics.Debug.WriteLine("=== DEBUG INFO ===");
                //System.Diagnostics.Debug.WriteLine(string.Format("@INVENTLOCATION = '{0}'",
                //    ApplicationSettings.Terminal.InventLocationId));
                //System.Diagnostics.Debug.WriteLine(string.Format("dateString (string) = '{0}' (type: {1})",
                //    dateString, dateString.GetType().Name));
                //System.Diagnostics.Debug.WriteLine(string.Format("dateParam (parsed) = '{0}' (type: {1})",
                //    dateParam, dateParam.GetType().Name));
                //System.Diagnostics.Debug.WriteLine(string.Format("DateTime.Today = '{0}' (type: {1})",
                //    DateTime.Today, DateTime.Today.GetType().Name));
                //System.Diagnostics.Debug.WriteLine("===================");
                //// Build a testable SQL string with ACTUAL values (for SSMS)
                //string testQuery = queryString
                //    .Replace("@INVENTLOCATION", "'" + ApplicationSettings.Terminal.InventLocationId.Replace("'", "''") + "'")
                //    .Replace("@DATE", "'" + DateTime.Today.ToString("yyyy-MM-dd") + "'");

                //// Output it for debugging
                //System.Diagnostics.Debug.WriteLine("=== TESTABLE QUERY (paste into SSMS) ===");
                //System.Diagnostics.Debug.WriteLine(testQuery);
                //System.Diagnostics.Debug.WriteLine("=========================================");
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    //command.Parameters.AddWithValue("@DATE", dateString);
                    //command.Parameters.AddWithValue("@INVENTLOCATION", ApplicationSettings.Terminal.InventLocationId);

                    command.Parameters.Add("@INVENTLOCATION", SqlDbType.NVarChar, 20).Value = ApplicationSettings.Terminal.InventLocationId;                  
                    //command.Parameters.Add("@DATE", SqlDbType.Date).Value = DateTime.Today;
                    command.Parameters.Add("@DATE", SqlDbType.Date).Value = new DateTime(2025, 11, 18);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        
                        {
                            findNewPrice = true;
                          
                            XmlElement itemNode = xmlDoc.CreateElement("Item");

                          
                            XmlElement itemIdNode = xmlDoc.CreateElement("ItemRelation");
                            itemIdNode.InnerText = reader["ITEMRELATION"].ToString();
                            itemNode.AppendChild(itemIdNode);

                            XmlElement itemNameNode = xmlDoc.CreateElement("Name");
                            itemNameNode.InnerText = reader["NAME"] == DBNull.Value ? "" : reader["NAME"].ToString();
                            itemNode.AppendChild(itemNameNode);

                            XmlElement barcodeNode = xmlDoc.CreateElement("ItemBarcode");
                            barcodeNode.InnerText = reader["ITEMBARCODE"] == DBNull.Value ? "" : reader["ITEMBARCODE"].ToString();
                            itemNode.AppendChild(barcodeNode);

                            XmlElement unitNode = xmlDoc.CreateElement("UnitId");
                            unitNode.InnerText = reader["UNITID"].ToString();
                            itemNode.AppendChild(unitNode);

                            XmlElement amountNode = xmlDoc.CreateElement("Amount");
                            amountNode.InnerText = reader["AMOUNT"].ToString();
                            itemNode.AppendChild(amountNode);

                            XmlElement discNode = xmlDoc.CreateElement("Discount");
                            discNode.InnerText = "0"; //getDiscount(itemIdNode.InnerText.ToString(), Convert.ToDecimal( amountNode.InnerText)).ToString();
                            itemNode.AppendChild(discNode);
 

                            // Tambahkan ke root
                            root.AppendChild(itemNode);
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

            if (findNewPrice == true)
            {

                Infolog infologItem = new Infolog(xmlDoc, application);
                infologItem.ItemsReady += Infolog_ItemsReady;
                infologItem.ShowDialog();
            }
        }

        private void Infolog_ItemsReady(object sender, List<Infolog.ItemData> e)
        {
            foreach (Infolog.ItemData item in e)
            {
                bool exists = false;

                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (r.Cells[0].Value != null && r.Cells[0].Value.ToString() == item.SKU)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    dataGridView1.Rows.Add(
                        item.SKU,
                        item.Barang,
                        item.UnitId,
                        item.Barcode,
                        item.Price.ToString("N0"),
                        item.Disc.ToString("N0")
                    );
                }
            }
        }



        

        private void addBtn_Click(object sender, EventArgs e)
        {
            // Open ItemSearch form
            ItemSearch itemSearchForm = new ItemSearch(application);
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
                dataGridView1.Rows.Add(selectedSKU, selectedBarang, selectedUnitId, selectedBarcode, selectedPrice.ToString("N0"), selectedDisc.ToString("N0"));
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
            xmlLayout = "";
            string itemIdMulti = "";
            string siteId = "";
            string qtyMulti = "";

           


            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Silakan add barang terlebih dahulu");
            }
            else
            {
                DialogResult result = MessageBox.Show(
               "Klik 'Yes' untuk cetak label",
               "Konfirmasi",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                { 

                
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                       
                        if (!row.IsNewRow)
                        {
                            
                            string sku = row.Cells["SKU"].Value.ToString();
 
                            itemIdMulti += sku + ";";
                            qtyMulti += "0" + ";";
                        }
                    }

                    
                    if (!string.IsNullOrEmpty(itemIdMulti))
                    {
                        itemIdMulti = itemIdMulti.TrimEnd(';');
                        qtyMulti = qtyMulti.TrimEnd(';');
                    }

                  
                    //MessageBox.Show(itemIdMulti);
                    //getSiteWH(out  siteId);
                    //checkStockItem(itemIdMulti, qtyMulti, siteId);
                    getPrintLayout();
                    CP_PrintLabel(xmlLayout);
                }
            }           
            
        }
        
        private void CP_PrintLabel(string xmlLayout)
        {
            string printerName = LSRetailPosis.Settings.HardwareProfiles.Printer.DeviceName;
            int heightPaper = 170; //default 152
            byte[] xmlBytes = new byte[xmlLayout.Length / 2];
            for (int i = 0; i < xmlLayout.Length; i += 2)
            {
                xmlBytes[i / 2] = Convert.ToByte(xmlLayout.Substring(i, 2), 16);
            }

            string xmlString = Encoding.UTF8.GetString(xmlBytes);
            xmlString.Replace("\0", string.Empty).Trim();
            string cleanXML = Regex.Replace(xmlString, @"[^\u0020-\u007E\u000A\u000D]", string.Empty).Trim();
           

            XDocument xmlDoc = XDocument.Parse(cleanXML);
            lines = xmlDoc.Descendants("line")
                              .OrderBy(x => int.Parse((string)x.Attribute("nr") ?? "0"))
                              .ToList();

            Console.ReadLine();

            PrintDocument p = new PrintDocument();
            

            if (printerName.Contains("LX-310"))
            {
                // Dot matrix (tractor feed)
                p.DefaultPageSettings.PaperSize = new PaperSize("Custom", 320, heightPaper); // per item
                p.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            }
            else
            {
                // Thermal printer (roll)
                p.DefaultPageSettings.PaperSize = new PaperSize("Custom", 280, 10000); 
                p.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            }

            listItemId.Clear();
            listItemName.Clear();
            listItemBarcode.Clear();
            listUnitId.Clear();
            listPrice.Clear();
            listDisc.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                 
                string sku = Convert.ToString(row.Cells[0].Value);
                string barang = Convert.ToString(row.Cells[1].Value);
                string unitId  = Convert.ToString(row.Cells[2].Value);
                string barcode = Convert.ToString(row.Cells[3].Value);
                string priceStr = Convert.ToString(row.Cells[4].Value);
                string discStr = Convert.ToString(row.Cells[5].Value);

                decimal price = 0;
                decimal disc = 0;

                decimal.TryParse(priceStr, out price);
                decimal.TryParse(discStr, out disc);

                listItemId.Add(sku);
                listItemName.Add(barang);
                listItemBarcode.Add(barcode); 
                listUnitId.Add(unitId);
                listPrice.Add(price);
                listDisc.Add(disc);
            }



            p.PrintPage += p_PrintPage;
                
            //    (sender, e) =>
            //{
                

            try
            {
                feedLines = false;
                p.Print();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Print error: " + ex.Message);
            }
        }
        

        private void p_PrintPage(object sender, PrintPageEventArgs e)
        {
            float y = 0;
            float leftMargin = 0;
            float pageHeight = 152; // e.MarginBounds.Height; 
            float lineHeight = 18;
            int spaceHeader = 6;
            int spaceLine = 8;
            string printerName = LSRetailPosis.Settings.HardwareProfiles.Printer.DeviceName;

            if (printerName == "EPSON LX-310 ESC/P")//"Microsoft XPS Document Writer")
            {
                spaceHeader = 95;
                spaceLine = 95;
            }
            else //for thermal printer
            {
                spaceHeader = 0;
                spaceLine = 0;
            }
            //"".PadLeft(spaceHeader) 
            float baseXOffset = spaceHeader; 
            //for (int i = 0; i < listItemId.Count; i++)
            //{
            while (currentPrintIndex < listItemId.Count && feedLines == false)
            {

                if (printerName.Contains("LX-310"))
                {
                    if (y + lineHeight >= pageHeight)
                    { 
                        e.HasMorePages = true;
                        return;
                    }
                }


                //PrintDocument p = new PrintDocument();
                //p.DefaultPageSettings.PaperSize = new PaperSize("Custom", 280, 250);

                //p.PrintPage += (sender, e) =>
                //{
                string itemId = listItemId[currentPrintIndex];
                string itemName = listItemName[currentPrintIndex];
                string barCode = listItemBarcode[currentPrintIndex];
                string unitId = listUnitId[currentPrintIndex];
                decimal price = listPrice[currentPrintIndex];
                decimal disc = listDisc[currentPrintIndex];

                foreach (var line in lines)
                {
                    var charposList = line.Elements("charpos")
                        .OrderBy(c => int.Parse((string)c.Attribute("nr") ?? "0"))
                        .ToList();

                    float currentY = y;
                    foreach (var cp in charposList)
                    {
                        int nr = int.Parse((string)cp.Attribute("nr") ?? "0");
                        string rawVal = (string)cp.Attribute("value") ?? "";
                        string value = rawVal;
                        int fontStyle = int.Parse((string)cp.Attribute("FontStyle") ?? "0");
                        int fontSize = 9; //int.Parse((string)cp.Attribute("FontSize") ?? "9");
                         

                        FontStyle fs = FontStyle.Regular;
                        if (fontStyle == 1) fs = FontStyle.Bold;


                        //leftMargin = -3;
                        leftMargin = -3 + baseXOffset;
                        float x = leftMargin + nr * 2.2f;



                        switch (rawVal.ToUpperInvariant())
                        {
                            case "STORE":
                                value = "PRIMA FRESH MART";
                                e.Graphics.DrawString(value, new Font("Calibri", 8, fs),
                                  Brushes.Black, new PointF(x, currentY));
                                y = y - 10;
                                break;
                            case "UNITID":
                                value = "Unit: " + unitId;
                                e.Graphics.DrawString(value, new Font("Calibri", 8, fs),
                                  Brushes.Black, new PointF(x, currentY + 10));
                                break;
                            case "BARCODE":
                                //value = itemId;
                                try
                                {

                                    using (Bitmap barcodeBmp = GenerateAnyBarcode(barCode, width: 200, height: 20)) // GenerateAnyBarcode
                                    {

                                        float barcodeX = leftMargin;// +(280 - barcodeBmp.Width) / 2;
                                        e.Graphics.DrawImage(barcodeBmp, barcodeX, currentY);
                                    }
                                    currentY = currentY + 20;
                                    e.Graphics.DrawString(barCode, new Font("Calibri", 9), Brushes.Black, new PointF(x + 60, currentY));
                                    //y = y = 4;

                                }
                                catch (Exception ex)
                                {

                                    e.Graphics.DrawString(itemId, new Font("Calibri", 9), Brushes.Black, new PointF(x, currentY));

                                }
                                break;
                            case "ITEMNAME":
                                value = itemName;
                                e.Graphics.DrawString(value, new Font("Calibri", fontSize, fs),
                              Brushes.Black, new PointF(x, currentY));
                                break;
                            case "PRICE":
                                //value = "Rp. " + price.ToString("N0");
                                if (disc > 0 && disc < price)
                                {
                                    using (Bitmap bmp = new Bitmap(150, 25))
                                    using (Graphics g = Graphics.FromImage(bmp))
                                    {
                                        g.Clear(Color.White);
                                        Font f = new Font("Calibri", 9);
                                        string oldPrice = price.ToString("N0");
                                        SizeF ts = g.MeasureString(oldPrice, f);
                                        g.DrawString("Rp. " + oldPrice, f, Brushes.Black, 0, 0);
                                        g.DrawLine(Pens.Black, 0, ts.Height / 2, ts.Width, ts.Height / 2);
                                        e.Graphics.DrawImage(bmp, x + 12, currentY + 5);
                                    }
                                }

                                break;
                            case "DISCOUNT":
                            case "DISC":
                                //value = (disc > 0 && disc < price) ? disc.ToString("N0") : "";
                                if (disc > 0 && disc < price)
                                {
                                    e.Graphics.DrawString("Rp. " + disc.ToString("N0"), new Font("Calibri", 13, fs),
                                    Brushes.Black, new PointF(x + 10, currentY));
                                }
                                else if (disc == 0)
                                {
                                    e.Graphics.DrawString("Rp. " + price.ToString("N0"), new Font("Calibri", 13, fs),
                                        Brushes.Black, new PointF(x, currentY));

                                }
                                break;
                            case "PRINTED":
                                value = "Printed : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                e.Graphics.DrawString(value, new Font("Calibri", fontSize, fs),
                              Brushes.Black, new PointF(x, currentY));
                                break;
                            default:
                                e.Graphics.DrawString(value, new Font("Calibri", fontSize, fs),
                               Brushes.Black, new PointF(x, currentY));
                                break;
                        }


                    }

                    y += lineHeight; // line spacing


                }

                //};

                //p.EndPrint += (s, e) =>
                //{
                //    string printerName = p.PrinterSettings.PrinterName;

                //    // hilangkan extra feed
                //    Thread.Sleep(200);
                //    RawPrinterHelper.SendStringToPrinter(printerName, "\x1D\x56\x42\x00"); // cut tanpa feed
                //};

                //try
                //{

                //    p.Print();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Print error: " + ex.Message);
                //}
                //RawPrinterHelper.SendStringToPrinter(p.PrinterSettings.PrinterName, "\x1B\x64\x02"); // feed
                //    RawPrinterHelper.SendStringToPrinter(p.PrinterSettings.PrinterName, "\x1D\x56\x01"); // partial cut
                //y += 8;
                //e.Graphics.DrawString("------------------------------------------",
                //    new Font("Consolas", 8), Brushes.Black, new PointF(leftMargin, y));
                //y += 20;
                //if (printerName.Contains("LX-310") && currentPrintIndex ==  listItemId.Count)
                //{
                //    // reset y to the last safe drawing position before page end
                //    if (y + (lineHeight * 10) > pageHeight)
                //        y = pageHeight - (lineHeight * 10);

                //    y += lineHeight * 10;

                //    e.Graphics.DrawString("LINE", new Font("Calibri", 9, FontStyle.Regular),
                //              Brushes.Black, new PointF(3, y));
                //}


                
                currentPrintIndex++;

            }

            //This won't print unless for the last time e.hasmorepage and feedlines == true
            if (feedLines == true)
            {
                if (printerName.Contains("LX-310"))
                {
              
                    if (y + (lineHeight * 10) > pageHeight)
                        y = pageHeight - (lineHeight * 10);

                    y += lineHeight * 15;

                    e.Graphics.DrawString("--", new Font("Calibri", 8, FontStyle.Regular),
                        Brushes.Black, new PointF(3, y));
                }
            }

          
            //this won't trigger twice, cause of feedlines becoming true after this.
            if (currentPrintIndex >= listItemId.Count && feedLines == false)
            {
                feedLines = true;
                e.HasMorePages = true;
                return;
            }


            e.HasMorePages = false;
            currentPrintIndex = 0;  
            //};
        }

        /*private void p_PrintPage(object sender, PrintPageEventArgs e)
        {
            float y = 0;
            float leftMargin = 0;
            float pageHeight = e.MarginBounds.Height;
            float lineHeight = 18;
            float spaceHeader = 6f;
            float spaceLine = 8f;
            string printerName = LSRetailPosis.Settings.HardwareProfiles.Printer.DeviceName;

        
            if (printerName == "EPSON LX-310 ESC/P") // dot matrix
            {
                spaceHeader = 6f;
                spaceLine = 8f;
            }
            else // thermal printer
            {
                spaceHeader = 0f;
                spaceLine = 0f;
            }

          
            float baseXOffset = spaceHeader; // horizontal  
            float extraLineSpacing = spaceLine; // vertical spacing per line

            while (currentPrintIndex < listItemId.Count)
            {
                if (y + lineHeight > pageHeight)
                {
                    e.HasMorePages = true;
                    return;
                }

                string itemId = listItemId[currentPrintIndex];
                string itemName = listItemName[currentPrintIndex];
                string barCode = listItemBarcode[currentPrintIndex];
                string unitId = listUnitId[currentPrintIndex];
                decimal price = listPrice[currentPrintIndex];
                decimal disc = listDisc[currentPrintIndex];

                foreach (var line in lines)
                {
                    var charposList = line.Elements("charpos")
                        .OrderBy(c => int.Parse((string)c.Attribute("nr") ?? "0"))
                        .ToList();

                    float currentY = y + extraLineSpacing; // apply printer spacing
                    foreach (var cp in charposList)
                    {
                        int nr = int.Parse((string)cp.Attribute("nr") ?? "0");
                        string rawVal = (string)cp.Attribute("value") ?? "";
                        string value = rawVal;
                        int fontStyle = int.Parse((string)cp.Attribute("FontStyle") ?? "0");
                        int fontSize = 9;

                        FontStyle fs = (fontStyle == 1) ? FontStyle.Bold : FontStyle.Regular;

                        // adjust X position based on character position + printer offset
                        leftMargin = -3 + baseXOffset;
                        float x = leftMargin + nr * 2.2f;

                        switch (rawVal.ToUpperInvariant())
                        {
                            case "STORE":
                                value = "PRIMA FRESH MART";
                                e.Graphics.DrawString(value, new Font("Calibri", 8, fs),
                                    Brushes.Black, new PointF(x, currentY - 10));
                                break;

                            case "UNITID":
                                value = "Unit: " + unitId;
                                e.Graphics.DrawString(value, new Font("Calibri", 8, fs),
                                    Brushes.Black, new PointF(x, currentY + 10));
                                break;

                            case "BARCODE":
                                try
                                {
                                    using (Bitmap barcodeBmp = GenerateGS1_128(barCode, 200, 20))
                                    {
                                        float barcodeX = leftMargin;
                                        e.Graphics.DrawImage(barcodeBmp, barcodeX, currentY);
                                    }

                                    currentY += 20;
                                    e.Graphics.DrawString(barCode, new Font("Calibri", 9),
                                        Brushes.Black, new PointF(x + 60, currentY));
                                }
                                catch
                                {
                                    e.Graphics.DrawString(itemId, new Font("Calibri", 9),
                                        Brushes.Black, new PointF(x, currentY));
                                }
                                break;

                            case "ITEMNAME":
                                e.Graphics.DrawString(itemName, new Font("Calibri", fontSize, fs),
                                    Brushes.Black, new PointF(x, currentY));
                                break;

                            case "PRICE":
                                if (disc > 0 && disc < price)
                                {
                                    using (Bitmap bmp = new Bitmap(150, 25))
                                    using (Graphics g = Graphics.FromImage(bmp))
                                    {
                                        g.Clear(Color.White);
                                        Font f = new Font("Calibri", 9);
                                        string oldPrice = price.ToString("N0");
                                        SizeF ts = g.MeasureString(oldPrice, f);
                                        g.DrawString("Rp. " + oldPrice, f, Brushes.Black, 0, 0);
                                        g.DrawLine(Pens.Black, 0, ts.Height / 2, ts.Width, ts.Height / 2);
                                        e.Graphics.DrawImage(bmp, x + 12, currentY + 5);
                                    }
                                }
                                break;

                            case "DISC":
                            case "DISCOUNT":
                                if (disc > 0 && disc < price)
                                {
                                    e.Graphics.DrawString("Rp. " + disc.ToString("N0"),
                                        new Font("Calibri", 13, fs),
                                        Brushes.Black, new PointF(x + 10, currentY));
                                }
                                else if (disc == 0)
                                {
                                    e.Graphics.DrawString("Rp. " + price.ToString("N0"),
                                        new Font("Calibri", 13, fs),
                                        Brushes.Black, new PointF(x, currentY));
                                }
                                break;

                            case "PRINTED":
                                value = "Printed : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                e.Graphics.DrawString(value, new Font("Calibri", fontSize, fs),
                                    Brushes.Black, new PointF(x, currentY));
                                break;

                            default:
                                e.Graphics.DrawString(value, new Font("Calibri", fontSize, fs),
                                    Brushes.Black, new PointF(x, currentY));
                                break;
                        }
                    }

                    y += lineHeight + extraLineSpacing; // apply printer-specific line spacing
                }

                currentPrintIndex++;
            }

            e.HasMorePages = false;
        }
        */


        private void getPrintLayout()
        {
            //GET THE DESIGN
           
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = "";
                queryString = @"SELECT DESCRIPTION, LINESXML FROM AX.RETAILFORMLAYOUT WHERE FORMLAYOUTID = '37'";

                //SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {


                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                xmlLayout = reader["LINESXML"].ToString();
                            }
                        }
                    }
                }
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

        public static class RawPrinterHelper
        {
            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
            public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);
            [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true)]
            public static extern bool ClosePrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true)]
            public static extern bool StartDocPrinter(IntPtr hPrinter, int Level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);
            [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true)]
            public static extern bool EndDocPrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true)]
            public static extern bool StartPagePrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true)]
            public static extern bool EndPagePrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true)]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public class DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)]
                public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)]
                public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)]
                public string pDataType;
            }

            public static bool SendStringToPrinter(string printerName, string data)
            {
                IntPtr pBytes;
                IntPtr hPrinter;
                int dwWritten;
                DOCINFOA di = new DOCINFOA
                {
                    pDocName = "Raw ESC/POS Command",
                    pDataType = "RAW"
                };

                if (!OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
                    return false;

                StartDocPrinter(hPrinter, 1, di);
                StartPagePrinter(hPrinter);

                byte[] bytes = Encoding.ASCII.GetBytes(data);
                pBytes = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, pBytes, bytes.Length);
                WritePrinter(hPrinter, pBytes, bytes.Length, out dwWritten);
                Marshal.FreeCoTaskMem(pBytes);

                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);
                ClosePrinter(hPrinter);
                return true;
            }
        }

        public static Bitmap GenerateAnyBarcode(string digits, int width = 250, int height = 120)
        {
            var options = new EncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 10,
                PureBarcode = true
            };

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128, // <-- bukan GS1
                Options = options
            };

            var pixelData = writer.Write(digits);

            Bitmap bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                          ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
        }


        public static Bitmap GenerateGS1_128(string barcodeDigits, int width = 250, int height = 120)
        {
            if (string.IsNullOrWhiteSpace(barcodeDigits))
                throw new ArgumentException("Barcode digits cannot be empty.");

            // Ensure GTIN-14 format (prepend 0 if GTIN-13)
            string gtin14 = barcodeDigits.Length == 13 ? "0" + barcodeDigits : barcodeDigits;
            if (gtin14.Length != 14)
                throw new ArgumentException("GS1-128 (AI 01) requires 13 or 14 digit GTIN.");

            // GS1-128 uses FNC1 prefix
            string gs1Data = "\u001D" + "01" + gtin14; // FNC1 + AI(01) + GTIN-14

            // Configure ZXing encoder
            var options = new EncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 10,
                PureBarcode = true
            };

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = options
            };

            var pixelData = writer.Write(gs1Data);

            // Convert to bitmap
            Bitmap bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                          ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
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
                        var result = apiFunction.checkStockOnHandMultiNew(application, urlRTS, application.Settings.Database.DataAreaID, _siteId, ApplicationSettings.Terminal.InventLocationId, _itemIdMulti, "", "", "", _qtyMulti, posTransaction.StoreId + "-FORMCHECKSTOCK"); //add 2 new parameter by Yonathan 11092024
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
