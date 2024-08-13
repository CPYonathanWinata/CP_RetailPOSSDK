/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
//Begin Add NEC-Hamzah
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Drawing;
using System.Xml;
//End Add NEC-Hamzah
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Printing;
using System.Management;

namespace Microsoft.Dynamics.Retail.Pos.PurchaseOrderReceiving
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Too many form framework types.")]
    public partial class frmPurchaseOrderReceiving : LSRetailPosis.POSProcesses.frmTouchBase
    {
        [Import]
        public IApplication Application { get; set; }
        private PosTransaction posTransaction;
        private DataTable headerTable;
        private DataTable entryTable;
        private SaleLineItem saleLineItem;
        private NumPadMode inputMode;
        private PurchaseOrderReceiptData receiptData;
        private bool isEdit; // = false;
        private PRCountingType prType;
        private bool isMixedDeliveryMode = false;
        decimal quantityTotal;

        string errorPrinter = "Tidak bisa melakukan print !!!\nSebelum hubungi Tim IT, pastikan :\n- Printer sudah on/menyala\n- Default Printer \"EPSON LX 310 ESC/P\"\n- Posisi kertas struk terpasang dengan benar\n- Semua kabel USB tidak kendor\n\nKemudian coba print ulang ";//dengan tekan tombol \"Reprint\"";
        //Begin add line NEC
        int Offset = 0;
        /// <summary>
        /// Mode of the numberpad:  Barcode entry, or Quantity entry
        /// </summary>
        private enum NumPadMode
        {
            Barcode = 0,
            Quantity = 1
        }

        /// <summary>
        /// Simple container for PO Receipt line details
        /// </summary>
        private struct EntryItem
        {
            public string ItemNumber;
            public string ItemName;
            public decimal QuantityReceived;
            public string Unit;
        }

        /// <summary>
        /// Get/Set the receipt number for the receipt to be shown
        /// </summary>
        public string ReceiptNumber { get; set; }

        /// <summary>
        /// Get/set the receipt type for the receipt to b shown
        /// </summary>
        public PRCountingType PRType { get; set; }

        /// <summary>
        /// Get/Set the purchase Id for the receipt to be shown
        /// </summary>
        public string PONumber { get; set; }

        /// <summary>
        /// Sets Pos transaction to value.
        /// </summary>
        public PosTransaction PosTransaction
        {
            set { posTransaction = value; }
        }

        /// <summary>
        /// Displays purchase order receiving form.
        /// </summary>
        public frmPurchaseOrderReceiving()
        {

            InitializeComponent();
            
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                receiptData = new PurchaseOrderReceiptData(
                    ApplicationSettings.Database.LocalConnection,
                    ApplicationSettings.Database.DATAAREAID,
                    ApplicationSettings.Terminal.StorePrimaryId);

                posTransaction = (PosTransaction)PurchaseOrderReceiving.InternalApplication.BusinessLogic.Utility.CreateSalesOrderTransaction(
                    ApplicationSettings.Terminal.StoreId,
                    ApplicationSettings.Terminal.StoreCurrency,
                    ApplicationSettings.Terminal.TaxIncludedInPrice,
                    PurchaseOrderReceiving.InternalApplication.Services.Rounding);

                ClearForm();

                PurchaseOrderReceiving.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(OnBarcodeScan);
                PurchaseOrderReceiving.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent += new ScannerMessageEventHandler(OnBarcodeScan);
                PurchaseOrderReceiving.InternalApplication.Services.Peripherals.Scanner.ReEnableForScan();

            }
            DeleteTmpLine();
            LoadReceiptLines();
            TranslateLabels();
            base.OnLoad(e);
            //<CP_POTOCancel>
            if (!string.IsNullOrEmpty(this.cpGetDataDocumentBuffer()))
            {
                btnCommit.Visible = false;
                numPad1.Visible = false;
                txtDelivery.Visible = false; //temp disable 22072024
                txtDriver.Visible = false;

                //additional by Yonathan to enable the reprint button 23072024
                btnReprint.Visible = true;
                //end
            }
            else
            {
                btnReprint.Visible = false;
            }

            btnSave.Visible = false;
            btnSearch.Visible = false;
            if (prType == PRCountingType.PurchaseOrder)
            {
                btnUom.Visible = false;
            }
            else
            {
                btnEdit.Visible = false;
            }
            //</CP_POTOCancel>
            this.gvInventory.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GridView_RowClickEventHandler);
            
            
            //Add by Erwin 15 July 2019
            SetInputMode(NumPadMode.Quantity);
            btnEdit.Visible = false;
            //End Add by Erwin 15 July 2019


            //add by Yonathan 13/07/2023
            txtSearchBox.Text = "Search Item number or Description";
            txtSearchBox.ForeColor = Color.Gray;
            //end
            if (btnReprint.Visible == true)
            {
                btnCheckRcv.Visible = false;
            }
            else
            {
                btnCheckRcv.Visible = true;
            }

        }

        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmPurchaseOrderReceiving are reserved at 103140 - 103159
            // In use now are ID's: 103140 - 103157
            //
            tableLayoutPanelMain.Size = new System.Drawing.Size(1017, 707);
            colItemNumber.Caption = ApplicationLocalizer.Language.Translate(103146); //Item
            colOrdered.Caption = ApplicationLocalizer.Language.Translate(103147); //Ordered
            colReceived.Caption = ApplicationLocalizer.Language.Translate(103148); //Received
            colReceivedNow.Caption = ApplicationLocalizer.Language.Translate(1031482); //Received Now
            this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(1031202); //Picking/Receiving
            switch (prType)
            {
                case PRCountingType.PurchaseOrder:
                    colReceived.Caption = ApplicationLocalizer.Language.Translate(103148); //Received
                    colReceivedNow.Caption = ApplicationLocalizer.Language.Translate(1031482); //Received Now
                    this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(103140); //Receiving 
                    btnReceiveAll.Text = ApplicationLocalizer.Language.Translate(103118); //Receive All
                    //start - add modification by Yonathan to disable Receive All button 10/10/2022
                    btnReceiveAll.Visible = false;
                    numPad1.NumberOfDecimals = 3;

                    //end 
                    break;
                case PRCountingType.TransferIn:
                    colReceived.Caption = ApplicationLocalizer.Language.Translate(103148); //Received
                    colReceivedNow.Caption = ApplicationLocalizer.Language.Translate(1031482); //Received Now
                    this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(1031402); //Transfer Order Receiving 
                    btnReceiveAll.Text = ApplicationLocalizer.Language.Translate(103118); //Receive All
                    //start - add modification by Yonathan 10/10/2022 to disable numpad and entering quantity
                    //numPad1.Enabled = false;
                    //numPad1.Enabled = false;
                    //add modification by Yonathan 20/05/2024 to enable numpad 
                    //btnReceiveAll.Visible = false;

                    //btnReceiveAll.Visible = false;

                    //13/08/2024 disable numpad, enable btnReceiveAll by Yonathan
                    btnReceiveAll.Visible = true;
                    numPad1.Enabled = false;
                    //end
                    numPad1.NumberOfDecimals = 3;
                    //end
                    break;
                case PRCountingType.TransferOut:
                    colReceived.Caption = ApplicationLocalizer.Language.Translate(1031481); //Picked
                    colReceivedNow.Caption = ApplicationLocalizer.Language.Translate(1031483); //Picked Now
                    this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(1031401); //Transfer Order Picking 
                    this.btnReceiveAll.Text = ApplicationLocalizer.Language.Translate(1031181); // Pick All
                    break;
                default:
                    break;
            }

            colUnit.Caption = ApplicationLocalizer.Language.Translate(103149); //UoM
            numPad1.PromptText = ApplicationLocalizer.Language.Translate(103154); //Scan or enter barcode
            lblReceiptNumberHeading.Text = ApplicationLocalizer.Language.Translate(103141); //Picking/Receiving no.
            lblPoNumberHeading.Text = ApplicationLocalizer.Language.Translate(103142); //Order number
            lblDriverHeading.Text = ApplicationLocalizer.Language.Translate(103143); //Driver details
            lblDeliveryHeading.Text = ApplicationLocalizer.Language.Translate(103144); //Delivery note number
            lblDeliveryMethod.Text = "Delivery note cannot be duplicate, if PO has been canceled and you want to receive again,  the number must be new"; //ApplicationLocalizer.Language.Translate(56362); //Delivery method
            btnClose.Text = ApplicationLocalizer.Language.Translate(103153); //Close
            btnSearch.Text = ApplicationLocalizer.Language.Translate(103152); //Search
            btnEdit.Text = ApplicationLocalizer.Language.Translate(103106); //Edit quantity
            btnSave.Text = ApplicationLocalizer.Language.Translate(103117); //Save
            btnRefresh.Text = ApplicationLocalizer.Language.Translate(103116); //Refresh
            btnUom.Text = ApplicationLocalizer.Language.Translate(103113); //Edit unit of measure
            btnCommit.Text = ApplicationLocalizer.Language.Translate(103107); //Commit
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            PurchaseOrderReceiving.InternalApplication.Services.Peripherals.Scanner.DisableForScan();
        }

        void OnBarcodeScan(IScanInfo scanInfo)
        {
            InventoryLookup(scanInfo.ScanDataLabel);
        }

        private void ClearForm()
        {
            this.saleLineItem = null;

            this.txtReceiptNumber.Text = string.Empty;

            if (this.entryTable != null)
            {
                this.entryTable.Clear();
            }
            this.grInventory.DataSource = this.entryTable;

            this.numPad1.ClearValue();
        }

        private void LoadReceiptLinesFromDB()
        {
            try
            {
                // Get all items from purchase order which are not assorted.
                ReadOnlyCollection<string> unassortedItems = receiptData.GetUnassortedItemsFromPurchaseOrder(this.PONumber, this.ReceiptNumber);

                bool createdLocal;
                // Get unassorted item's data from AX and create it in local database.
                if (unassortedItems != null && unassortedItems.Count > 0)
                {
                    string itemIds = string.Join<string>(",", unassortedItems);
                    XDocument tmpstring = PurchaseOrderReceiving.InternalApplication.Services.Item.GetProductData(itemIds);
                    ItemData itemData = new ItemData(
                        ApplicationSettings.Database.LocalConnection,
                        ApplicationSettings.Database.DATAAREAID,
                        ApplicationSettings.Terminal.StorePrimaryId);
                    createdLocal = itemData.SaveProductData(tmpstring);
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                PurchaseOrderReceiving.InternalApplication.Services.Dialog.ShowMessage(99890, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.entryTable = receiptData.GetPurchaseOrderReceiptLines(this.PONumber, this.ReceiptNumber);
            this.entryTable = AddVariantInfo(this.entryTable);
        }

        private void LoadReceiptHeadersFromDB()
        {
            this.headerTable = receiptData.GetPurchaseOrderReceipt(this.PONumber, this.ReceiptNumber);
        }

        private void LoadReceiptLines()
        {
            //string receiptNumber = "";
            LoadReceiptHeadersFromDB();
            LoadReceiptLinesFromDB();

            if (entryTable != null && entryTable.Rows.Count == 0)
            {
                GetReceiptLinesFromAX();
                SaveReceipt();
                LoadReceiptHeadersFromDB();
                //receiptNumber = entryTable.Rows[0].Field<string>(DataAccessConstants.ReceiptNumber);
                LoadReceiptLinesFromDB();
            }

            // Reset RowState for the first time of bringing lines from AX into an empty grid, 
            // or from DB, therefore it will NOT trigger the prompt of saving changes before exiting the form.
            this.entryTable.AcceptChanges();

            this.txtReceiptNumber.Text = headerTable.Rows[0].Field<string>(DataAccessConstants.ReceiptNumber); //receiptNumber; //change to receipt number from ax by Yonathan 23072024 //
            this.txtPoNumber.Text = headerTable.Rows[0].Field<string>(DataAccessConstants.PoNumber);
            this.txtDriver.Text = headerTable.Rows[0].Field<string>(DataAccessConstants.DriverDetails);
            //this.txtDelivery.Text = headerTable.Rows[0].Field<string>(DataAccessConstants.DeliveryNoteNumber);
            // this.txtDeliveryMethod.Text = headerTable.Rows[0].Field<string>(DataAccessConstants.DeliveryMethod);
            //this.isMixedDeliveryMode = string.IsNullOrEmpty(this.txtDeliveryMethod.Text);
            this.prType = headerTable.Rows[0].Field<PRCountingType>(DataAccessConstants.OrderType);

            this.grInventory.DataSource = this.entryTable;
            this.gvInventory.FocusedRowHandle = 0; //mod by Yonathan 12/10/2022            
            this.gvInventory.SelectRow(0);
        }

        private void SetInputMode(NumPadMode mode)
        {
            if (mode == NumPadMode.Barcode)
            {
                this.isEdit = false;
                PurchaseOrderReceiving.InternalApplication.Services.Peripherals.Scanner.ReEnableForScan();
                numPad1.EntryType = NumpadEntryTypes.Barcode;
                numPad1.PromptText = ApplicationLocalizer.Language.Translate(103154);               //Scan or enter barcode
            }
            else if (mode == NumPadMode.Quantity)
            {
                PurchaseOrderReceiving.InternalApplication.Services.Peripherals.Scanner.DisableForScan();
                numPad1.EntryType = NumpadEntryTypes.Quantity;
                numPad1.PromptText = ApplicationLocalizer.Language.Translate(103155);               //Scan or enter barcode
            }
            inputMode = mode;
        }

        private void ResetNumpad()
        {
            //Edit by Erwin 15 July 2019
            //this.saleLineItem = null;
            //End Edit by Erwin 15 July 2019
            numPad1.ClearValue();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (entryTable != null && entryTable.GetChanges() != null && btnReprint.Visible != true)
            {
                POSFormsManager.ShowPOSMessageDialog(103119);
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void frmPurchaseOrderReceiving_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void SaveReceipt()
        {
            string driver = headerTable.Rows[0].Field<string>(DataAccessConstants.DriverDetails);
            string delivery = headerTable.Rows[0].Field<string>(DataAccessConstants.DeliveryNoteNumber);

            if (!txtDriver.Text.Equals(driver))
            {
                headerTable.Rows[0][DataAccessConstants.DriverDetails] = txtDriver.Text;
            }
            if (!txtDelivery.Text.Equals(delivery))
            {
                headerTable.Rows[0][DataAccessConstants.DeliveryNoteNumber] = txtDelivery.Text;
            }

            try
            {
                this.UseWaitCursor = true;
                receiptData.SaveReceipt(headerTable, entryTable);
            }
            finally
            {
                this.UseWaitCursor = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string selectedItemId = string.Empty;

                //Edit by Erwin 15 July 2019
                //SetInputMode(NumPadMode.Barcode);
                SetInputMode(NumPadMode.Quantity);
                //End Edit by Erwin 15 July 2019

                // Show the search dialog through the item service
                if (!PurchaseOrderReceiving.InternalApplication.Services.Item.ItemSearch(ref selectedItemId, 500))
                {
                    return;
                }

                ItemData itemData = new ItemData(
                    ApplicationSettings.Database.LocalConnection,
                    ApplicationSettings.Database.DATAAREAID,
                    ApplicationSettings.Terminal.StorePrimaryId);

                numPad1.EnteredValue = itemData.GetBarcodeForItem(selectedItemId);

                if (string.IsNullOrEmpty(numPad1.EnteredValue))
                {
                    numPad1.EnteredValue = selectedItemId;
                }
                InventoryLookup(numPad1.EnteredValue);
                numPad1.Focus();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        void numPad1_EnterButtonPressed()
        {
            //Add by Erwin 15 July 2019

            DataRow row = GetCurrentRow();
            numPad1.Focus();

            if (row != null)
            {
                this.isEdit = true;
                string itemNumber = row.Field<string>(DataAccessConstants.ItemNumber);
                this.InventoryLookup(itemNumber);
            }

            //End Add by Erwin 15 July 2019
            //InventoryLookup(numPad1.EnteredValue);
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            // set quantity on an existing line
            DataRow row = GetCurrentRow();
            numPad1.Focus();

            if (row != null)
            {
                this.isEdit = true;
                string itemNumber = row.Field<string>(DataAccessConstants.ItemNumber);
                this.InventoryLookup(itemNumber);
            }
        }
        //add by Yonathan for reprint PO 09/07/2024
        /*private void btnReprint_Click(object sender, EventArgs s)
        {
            string tempDriverDetails;
            LoadReceiptLinesFromDB();         

            try
            {
               
                LoadReceiptLinesFromDB();
                //Begin add NEC hmz to manipulate PO Id with driver Details
                if (this.prType == PRCountingType.PurchaseOrder)// || this.prType == PRCountingType.TransferIn)
                    tempDriverDetails = this.PONumber + "-" + txtDelivery.Text;
                else
                    tempDriverDetails = this.PONumber;
                //End add NEC hmz

                // Commit receipt to AX via webservice
                // Begin modify line NEC - to pass tempDriverDetails
                //IPRDocument prDoc = PurchaseOrderReceiving.InternalApplication.Services.StoreInventoryServices.CommitOrderReceipt(tempDriverDetails, this.ReceiptNumber, this.prType);

                tempDriverDetails = this.PONumber;
                // Remove rows that are successfully submitted
                List<DataRow> removeRows = new List<DataRow>();

               
                string sHeader = "     -------------- " + Environment.NewLine +
                                   "         REPRINT " + Environment.NewLine +
                                   "     --------------" + Environment.NewLine;
                //Offset = Offset + 260;

                string sPrint = this.ReceiveDocumentFormat("REPRINT");

                PrintDocument p = new PrintDocument();
                PrintDialog pd = new PrintDialog();
                PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
                Margins margins = new Margins(0, 0, 0, 0);

                Font normalFont = new Font("Courier New", 8);
                Font biggerFont = new Font("Courier New", 20);

                pd.Document = p;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.Document.DefaultPageSettings.Margins = margins;
                p.DefaultPageSettings.PaperSize.Width = 600;

                p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                {
                    SolidBrush brush = new SolidBrush(Color.Black);
                    float leftMargin = p.DefaultPageSettings.PrintableArea.Left;
                    float yPos = 0;
 
                    string[] headerLines = sHeader.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in headerLines)
                    {
                        e1.Graphics.DrawString(line, biggerFont, brush, leftMargin, yPos);
                        yPos += normalFont.GetHeight(e1.Graphics);
                    }
                    
                   
                    string[] printLines = sPrint.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in printLines)
                    {
                        e1.Graphics.DrawString(line, normalFont, brush, leftMargin, yPos);
                        yPos += normalFont.GetHeight(e1.Graphics);
                    }
                     
                };

                try
                {
                    p.Print();
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception Occured While Printing", ex);
                }
                   
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }//end
         * */


        private void btnCheckRcv_Click(object sender, EventArgs s)
        {
            string tempDriverDetails;
            
            if (txtDelivery.Text == "")
            {
                // Show commit failure message                  
                throw new Exception("Please Fill the Delivery note number");
            }
            //check item qty
            this.checkQtyItem();
            //End add NEC
            //<CPPOTOCancel>
            if (this.prType == PRCountingType.TransferIn)
            {
                this.checkQtyReceived();
            }
            Offset = 0;
            string itemName = "";
            try
            {
               
                
                // Remove rows that are successfully submitted
                List<DataRow> removeRows = new List<DataRow>();

                string sHeader =   "   ----------------" + Environment.NewLine +
                                   "     CEK  RECEIVE  " + Environment.NewLine +
                                   "   ----------------" + Environment.NewLine;
                int a = 34;
                int b = 10;
                string sPrint = "";
                //sPrint += "             -----------------------------------" + Environment.NewLine;
                sPrint += "        SKU      Nama Item".PadRight(a) + "   Terima".PadLeft(b) + Environment.NewLine;
                sPrint += "        ---------------------------------------" + Environment.NewLine;
                //loop each itemName
                foreach (DataRow row in entryTable.Rows)
                {
                    //int unitDecimals = unitOfMeasureData.GetUnitDecimals(line.PurchUnit);
                    if (row.Field<string>(DataAccessConstants.ItemName).Length > 22)
                    {
                        itemName = row.Field<string>(DataAccessConstants.ItemName).Substring(0,22).PadRight(24);
                    }
                    else
                    {
                        itemName = row.Field<string>(DataAccessConstants.ItemName).PadRight(24);
                    }
                    
                    
                    sPrint += "        " + row.Field<string>(DataAccessConstants.ItemNumber) + "-" + itemName + PurchaseOrderReceiving.InternalApplication.Services.Rounding.Round(row.Field<decimal>(DataAccessConstants.QuantityReceivedNow),3) + Environment.NewLine; 
                }
                
                //

                sPrint += "        ---------------------------------------" + Environment.NewLine;
                sPrint += "        JIKA SUDAH BENAR, TEKAN TOMBOL COMMIT!" + Environment.NewLine;
                Offset += 136;

                //this.ReceiveDocumentFormat("PREVIEW");

                PrintDocument p = new PrintDocument();
                PrintDialog pd = new PrintDialog();
                PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
                Margins margins = new Margins(0, 0, 0, 0);

                Font normalFont = new Font("Courier New", 8);
                Font biggerFont = new Font("Courier New", 20);

                pd.Document = p;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.Document.DefaultPageSettings.Margins = margins;
                p.DefaultPageSettings.PaperSize.Width = 600;

                bool isPrinterOffline = checkPrinterStatus(p);                

                

                if (isPrinterOffline == false)
                {
                    p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                    {
                        SolidBrush brush = new SolidBrush(Color.Black);
                        float leftMargin = p.DefaultPageSettings.PrintableArea.Left;
                        float yPos = 0;

                        string[] headerLines = sHeader.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string line in headerLines)
                        {
                            e1.Graphics.DrawString(line, biggerFont, brush, leftMargin, yPos);
                            yPos += normalFont.GetHeight(e1.Graphics);
                        }

                        string[] printLines = sPrint.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string line in printLines)
                        {
                            e1.Graphics.DrawString(line, normalFont, brush, leftMargin, yPos);
                            yPos += normalFont.GetHeight(e1.Graphics);
                        }
                    };

                    try
                    {
                        p.Print();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception Occurred While Printing", ex);
                    }
                }
                else
                {
                    using (frmMessage frm = new frmMessage(errorPrinter, MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        POSFormsManager.ShowPOSForm(frm);
                    }
                }
                

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnReprint_Click(object sender, EventArgs s)
        {
            string tempDriverDetails;
            LoadReceiptLinesFromDB();
            Offset = 0;
            
            try
            {
                LoadReceiptLinesFromDB();
                //Begin add NEC hmz to manipulate PO Id with driver Details
                if (this.prType == PRCountingType.PurchaseOrder)// || this.prType == PRCountingType.TransferIn)
                    tempDriverDetails = this.PONumber + "-" + txtDelivery.Text;
                else
                    tempDriverDetails = this.PONumber;
                //End add NEC hmz

                // Commit receipt to AX via webservice
                // Begin modify line NEC - to pass tempDriverDetails
                //IPRDocument prDoc = PurchaseOrderReceiving.InternalApplication.Services.StoreInventoryServices.CommitOrderReceipt(tempDriverDetails, this.ReceiptNumber, this.prType);

                tempDriverDetails = this.PONumber;
                // Remove rows that are successfully submitted
                List<DataRow> removeRows = new List<DataRow>();

                string sHeader =   "     -------------- " + Environment.NewLine +
                                   "         REPRINT " + Environment.NewLine +
                                   "     --------------" + Environment.NewLine;

                string sPrint = this.ReceiveDocumentFormat("REPRINT");

                PrintDocument p = new PrintDocument();
                PrintDialog pd = new PrintDialog();
                PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
                Margins margins = new Margins(0, 0, 0, 0);

                Font normalFont = new Font("Courier New", 8);
                Font biggerFont = new Font("Courier New", 20);

                pd.Document = p;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.Document.DefaultPageSettings.Margins = margins;
                p.DefaultPageSettings.PaperSize.Width = 600;

                bool isPrinterOffline = checkPrinterStatus(p);                

                

                if (isPrinterOffline == false)
                {
                    p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                    {
                        SolidBrush brush = new SolidBrush(Color.Black);
                        float leftMargin = p.DefaultPageSettings.PrintableArea.Left;
                        float yPos = 0;

                        string[] headerLines = sHeader.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string line in headerLines)
                        {
                            e1.Graphics.DrawString(line, biggerFont, brush, leftMargin, yPos);
                            yPos += normalFont.GetHeight(e1.Graphics);
                        }

                        string[] printLines = sPrint.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string line in printLines)
                        {
                            e1.Graphics.DrawString(line, normalFont, brush, leftMargin, yPos);
                            yPos += normalFont.GetHeight(e1.Graphics);
                        }
                    };

                    try
                    {
                        p.Print();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception Occurred While Printing", ex);
                    }
                }
                else
                {
                    using (frmMessage frm = new frmMessage(errorPrinter, MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        POSFormsManager.ShowPOSForm(frm);
                    }
                }
                

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool checkPrinterStatus(PrintDocument p)
        {
            string printerName = p.PrinterSettings.PrinterName;
            bool isPrinterOffline = false;
            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher(string.Format("SELECT * FROM Win32_Printer WHERE Name LIKE '%{0}'", printerName));

            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = p.PrinterSettings.PrinterName;
                if (printerName.Equals(printerName))
                {
                    Console.WriteLine("Printer = " + printer["Name"]);
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        //MessageBox.Show("Printer is offline or not available.");
                        isPrinterOffline = true;

                    }
                    else
                    {
                        // printer is not offline
                        //MessageBox.Show("Printer is is connected.");
                        isPrinterOffline = false;

                    }
                }
            }

            return isPrinterOffline;
        }

        private void btnSave_Click(object sender, EventArgs s)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SaveReceipt();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void GetReceiptLinesFromAX()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                UnitOfMeasureData unitOfMeasureData = new UnitOfMeasureData(
                    PurchaseOrderReceiving.InternalApplication.Settings.Database.Connection,
                    PurchaseOrderReceiving.InternalApplication.Settings.Database.DataAreaID,
                    LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId,
                    PurchaseOrderReceiving.InternalApplication);

                // Purge all rows on grid
                entryTable.Rows.Clear();

                IList<IPRDocumentLine> poLines = PurchaseOrderReceiving.InternalApplication.Services.StoreInventoryServices.GetOrderReceiptLines(this.PONumber, this.PRType);

                // Append lines to grid control
                foreach (IPRDocumentLine newLine in poLines)
                {
                    DataRow row = entryTable.NewRow();
                    entryTable.Rows.Add(row);
                    UpdateRowWithPurchLine(row, newLine, unitOfMeasureData, this.PRType);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs s)
        {
            DeleteTmpLine();
            GetReceiptLinesFromAX();
            entryTable.AcceptChanges();

            this.SaveReceipt();
            LoadReceiptLines();
        }

        private void btnReceiveAll_Click(object sender, EventArgs e)
        {
            numPad1.ClearValue();

            foreach (DataRow row in entryTable.Rows)
            {
                string recId = row.Field<string>(DataAccessConstants.LineReceiptNumber);

                if (!string.IsNullOrWhiteSpace(recId))
                {
                    decimal quantity = row.Field<decimal>(DataAccessConstants.QuantityOrdered) - row.Field<decimal>(DataAccessConstants.QuantityReceived);

                    if (quantity < 0)
                    {
                        quantity = 0;
                    }

                    row[DataAccessConstants.QuantityReceivedNow] = quantity;
                }
            }

            grInventory.DataSource = entryTable;
        }

        private static void UpdateRowWithPurchLine(DataRow row, IPRDocumentLine line, UnitOfMeasureData unitOfMeasureData, PRCountingType PRTemp = PRCountingType.PickingList)
        {
            int unitDecimals = unitOfMeasureData.GetUnitDecimals(line.PurchUnit);

            // Update columns that are included in IPRDocumentLine
            row[DataAccessConstants.LineReceiptNumber] = line.RecId;
            row[DataAccessConstants.ItemNumber] = line.ItemId;
            row[DataAccessConstants.ItemName] = line.ItemName;
            // If the item is a service, qtyOrdered is always 0.
            //decimal quantity = line.QtyOrdered == 0 ? line.PurchQty : line.QtyOrdered;

            decimal quantity = line.PurchQty;
            if ((PRTemp == PRCountingType.TransferIn || PRTemp == PRCountingType.TransferOut) && line.QtyOrdered != 0)
            {
                quantity = line.QtyOrdered;
            }
            else 
            {
                quantity = line.PurchQty;
            }

            row[DataAccessConstants.QuantityOrdered] = PurchaseOrderReceiving.InternalApplication.Services.Rounding.Round(quantity, unitDecimals);
            row[DataAccessConstants.QuantityReceived] = PurchaseOrderReceiving.InternalApplication.Services.Rounding.Round(line.PurchReceived, unitDecimals);

            if (row.RowState == DataRowState.Added)
            {
                // Always set to zero because only delta will be shown and submitted
                row[DataAccessConstants.QuantityReceivedNow] = PurchaseOrderReceiving.InternalApplication.Services.Rounding.Round(line.PurchReceivedNow, unitDecimals);
            }

            row[DataAccessConstants.UnitOfMeasure] = line.PurchUnit;
            row[DataAccessConstants.ConfigId] = line.ConfigId;
            row[DataAccessConstants.InventSizeId] = line.InventSizeId;
            row[DataAccessConstants.InventColorId] = line.InventColorId;
            row[DataAccessConstants.InventStyleId] = line.InventStyleId;
            row[DataAccessConstants.DeliveryMethod] = line.DeliveryMethod;

            // Does not assign GUID. GUID cannot be used as PK because it will change every time on AX side

            //add by Yonathan to get receiptnumber from message line 22/07/2024
            //row[DataAccessConstants.ReceiptNumber] = line.Message;
        }

        private void btnUom_Click(object sender, EventArgs e)
        {
            OpenUnitOfMeasureDialog();
        }

        private bool GetItemInfo(string barcode, bool skipDimensionDialog)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return false;
            }

            this.saleLineItem = (SaleLineItem)PurchaseOrderReceiving.InternalApplication.BusinessLogic.Utility.CreateSaleLineItem(
                ApplicationSettings.Terminal.StoreCurrency,
                PurchaseOrderReceiving.InternalApplication.Services.Rounding,
                posTransaction);

            IScanInfo scanInfo = PurchaseOrderReceiving.InternalApplication.BusinessLogic.Utility.CreateScanInfo();
            scanInfo.ScanDataLabel = barcode;
            scanInfo.EntryType = BarcodeEntryType.ManuallyEntered;

            IBarcodeInfo barcodeInfo = PurchaseOrderReceiving.InternalApplication.Services.Barcode.ProcessBarcode(scanInfo);

            if ((barcodeInfo.InternalType == BarcodeInternalType.Item) && (barcodeInfo.ItemId != null))
            {
                // The entry was a barcode which was found and now we have the item id...
                this.saleLineItem.ItemId = barcodeInfo.ItemId;
                this.saleLineItem.BarcodeId = barcodeInfo.BarcodeId;
                this.saleLineItem.SalesOrderUnitOfMeasure = barcodeInfo.UnitId;
                this.saleLineItem.EntryType = barcodeInfo.EntryType;
                this.saleLineItem.Dimension.ColorId = barcodeInfo.InventColorId;
                this.saleLineItem.Dimension.SizeId = barcodeInfo.InventSizeId;
                this.saleLineItem.Dimension.StyleId = barcodeInfo.InventStyleId;
                this.saleLineItem.Dimension.VariantId = barcodeInfo.VariantId;
            }
            else
            {
                // It could be an ItemId
                this.saleLineItem.ItemId = barcodeInfo.BarcodeId;
                this.saleLineItem.EntryType = barcodeInfo.EntryType;
            }

            // fetch all the addtional item properties
            PurchaseOrderReceiving.InternalApplication.Services.Item.ProcessItem(saleLineItem, true);

            if (saleLineItem.Found == false)
            {
                POSFormsManager.ShowPOSMessageDialog(2611);             // Item not found.
                return false;
            }
            else if ((saleLineItem.Dimension != null)
                && (saleLineItem.Dimension.EnterDimensions || !string.IsNullOrEmpty(saleLineItem.Dimension.VariantId))
                && !skipDimensionDialog)
            {
                if (!barcodeInfo.Found)
                {
                    return OpenItemDimensionsDialog(barcodeInfo);
                }
            }

            return true; 
        }

        private void InventoryLookup(string barcode)
        {
            if (inputMode == NumPadMode.Barcode)
            {
                if (GetItemInfo(barcode, this.isEdit) && (this.isEdit || this.ValidateItemIdAndVariantsForPicking(this.saleLineItem)))
                {
                    this.SetQuantitiesFromRow(this.saleLineItem);

                    // Set mode to quantity
                    SetInputMode(NumPadMode.Quantity);
                }
                else
                {
                    // Clear the current item so the user can try again
                    ResetNumpad();
                }
            }
            else if ((inputMode == NumPadMode.Quantity) && (saleLineItem != null) && !string.IsNullOrEmpty(numPad1.EnteredValue))
            {
                // Add to list
                EntryItem item = new EntryItem()
                {
                    ItemNumber = this.saleLineItem.ItemId,
                    Unit = this.saleLineItem.InventOrderUnitOfMeasure,
                    ItemName = this.saleLineItem.Description,
                    QuantityReceived = decimal.Parse(numPad1.EnteredValue)
                };

                if (this.ValidateData(item, this.saleLineItem))
                {
                    AddItem(item);
                    //Edit by Erwin 15 July 2019
                    //SetInputMode(NumPadMode.Barcode);
                    SetInputMode(NumPadMode.Quantity);
                    //End Edit by Erwin 15 July 2019
                }
                else
                {
                    // Clear the current quantity so the user can try again
                    numPad1.ClearValue();
                    SetInputMode(NumPadMode.Quantity);
                }
            }
            numPad1.Select();
        }

        private void AddItem(EntryItem item)
        {
            DataRow rowInEdit = this.GetRowInEdit(this.saleLineItem);

            if (this.isEdit && rowInEdit != null)
            {
                // Edit QuantityReceived of an existing row
                rowInEdit[DataAccessConstants.QuantityReceivedNow] = item.QuantityReceived;
            }
            else
            {
                if (rowInEdit != null)
                {
                    // increment an existing row
                    decimal oldQuantity = rowInEdit.Field<decimal>(DataAccessConstants.QuantityReceivedNow);
                    rowInEdit[DataAccessConstants.QuantityReceivedNow] = oldQuantity + item.QuantityReceived;
                    rowInEdit[DataAccessConstants.ReceiptDate] = DateTime.Now;
                    rowInEdit[DataAccessConstants.UserId] = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
                    rowInEdit[DataAccessConstants.TerminalId] = ApplicationSettings.Terminal.TerminalId;
                    rowInEdit[DataAccessConstants.InventSizeId] = this.saleLineItem.Dimension.SizeId ?? string.Empty;
                    rowInEdit[DataAccessConstants.InventColorId] = this.saleLineItem.Dimension.ColorId ?? string.Empty;
                    rowInEdit[DataAccessConstants.InventStyleId] = this.saleLineItem.Dimension.StyleId ?? string.Empty;
                    rowInEdit[DataAccessConstants.ConfigId] = this.saleLineItem.Dimension.ConfigId ?? string.Empty;
                }
                else
                {
                    AddNewRow(item);
                }
            }

            ResetNumpad();
        }

        private static bool IsSameVariants(DataRow row, Dimensions dimension)
        {
            return row[DataAccessConstants.InventSizeId].ToString() == (dimension.SizeId ?? string.Empty)
                && row[DataAccessConstants.InventColorId].ToString() == (dimension.ColorId ?? string.Empty)
                && row[DataAccessConstants.InventStyleId].ToString() == (dimension.StyleId ?? string.Empty)
                && row[DataAccessConstants.ConfigId].ToString() == (dimension.ConfigId ?? string.Empty);
        }

        /// <summary>
        /// Return the data-row for the currently selected/focused row in the grid
        /// </summary>
        /// <returns>DataRow if it exists, null otherwise</returns>
        private DataRow GetCurrentRow()
        {
            ColumnView view = (ColumnView)grInventory.MainView;
            if (view != null)
            {
                return view.GetFocusedDataRow();
            }
            return null;
        }

        private DataRow GetRowInEdit(ISaleLineItem lineItem)
        {
            DataRow row = null;

            if (this.isEdit)
            {
                row = this.GetCurrentRow();
            }
            else
            {
                row = this.GetRowByItemIdAndVariants(lineItem);
            }

            return row;
        }

        private DataRow GetRowByItemIdAndVariants(ISaleLineItem lineItem)
        {
            DataRow[] rows = this.entryTable.Select(string.Format("ITEMNUMBER = '{0}'", lineItem.ItemId));

            DataRow ret = null;
            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    if (IsSameVariants(row, ((SaleLineItem)lineItem).Dimension))
                    {
                        ret = row;
                        break;
                    }
                }
            }

            return ret;
        }

        private void SetQuantitiesFromRow(ISaleLineItem lineItem)
        {
            UnitOfMeasureData unitOfMeasureData = new UnitOfMeasureData(
                PurchaseOrderReceiving.InternalApplication.Settings.Database.Connection,
                PurchaseOrderReceiving.InternalApplication.Settings.Database.DataAreaID,
                LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId,
                PurchaseOrderReceiving.InternalApplication);

            DataRow row = this.GetRowInEdit(lineItem);

            if (row != null)
            {
                int unitDecimals = unitOfMeasureData.GetUnitDecimals(row.Field<string>(DataAccessConstants.UnitOfMeasure));

                lineItem.QuantityOrdered = PurchaseOrderReceiving.InternalApplication.Services.Rounding.Round(row.Field<decimal>(DataAccessConstants.QuantityOrdered), unitDecimals);
                lineItem.QuantityPickedUp = PurchaseOrderReceiving.InternalApplication.Services.Rounding.Round(row.Field<decimal>(DataAccessConstants.QuantityReceived), unitDecimals);
                //txtDelivery.Text = row.Field<string>(DataAccessConstants.ReceiptNumber);
            }
        }

        //Begin Add NEC-Hmz to print Receipt after receive item  

        private static Configuration appConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private string GetSettingFromConfigFile()
        {
            string ConnectionString = "";

            XmlDocument xdoc = new XmlDocument();
            // NEC Begin modify 19 Apr 2016 Error in Path reference
            string strXmlFilePath = Path.Combine(ApplicationSettings.GetAppPath(), "POS.exe.config");
            //xdoc.Load(@"C:\Program Files (x86)\Microsoft Dynamics AX\60\Retail POS\POS.exe.config");
            xdoc.Load(strXmlFilePath);
            // NEC end modify
            XmlNode xnodes = xdoc.SelectSingleNode("configuration");
            XmlNodeList xmlList = xnodes.SelectNodes("AxRetailPOS");

            foreach (XmlNode xmlNodeS in xmlList)
            {
                ConnectionString += "," + xmlNodeS.Attributes.GetNamedItem("StoreDatabaseConnectionString").Value;
            }
            return ConnectionString.Substring(1);
        }

        private string GetStoreId()
        {
            string StoreId = "";

            XmlDocument xdoc = new XmlDocument();
            // NEC Begin modify 19 Apr 2016 Error in Path reference
            string strXmlFilePath = Path.Combine(ApplicationSettings.GetAppPath(), "POS.exe.config");
            //xdoc.Load(@"C:\Program Files (x86)\Microsoft Dynamics AX\60\Retail POS\POS.exe.config");
            xdoc.Load(strXmlFilePath);
            // NEC end modify
            XmlNode xnodes = xdoc.SelectSingleNode("configuration");
            XmlNodeList xmlList = xnodes.SelectNodes("AxRetailPOS");

            foreach (XmlNode xmlNodeS in xmlList)
            {
                StoreId += "," + xmlNodeS.Attributes.GetNamedItem("StoreId").Value;
            }
            return StoreId.Substring(1);
        }

        private string ReceiveDocumentFormat(string statusReceipt)
        {
            int totalQty = 0;
            decimal totalQtyDec = 0;
            string s = "";
            string namatoko = "";
            string deliverynote = "";
            string receiptDate = "";
            string qtyString = "";
            string qtyStringMod = "";
            string itemName = "";
            string itemNumber = "";
            decimal qty = 0;

            //string sumUnit, ItemName, ItemNumber, QtyReceivedNow = "";
            string connectionString = GetSettingFromConfigFile();
            string storeid = GetStoreId();
            //start modification by Yonathan 11/10/2022 to ease when debugging the PO/TO Receipt format 
            int a, b, c, d, e, f = 0;
            a = 34;
            b = 10;
            c = 18;
            d = 12;
            e = 8;
            f = 0;
            // end 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader, reader2, reader3 = null;
                    SqlCommand command = new SqlCommand("Select ItemNumber, ItemName, Unit,QuantityReceived, QuantityReceivedNow from POSPURCHASEORDERRECEIPTLINE where PONumber = '" + this.PONumber + "'", connection);
                    SqlCommand command2 = new SqlCommand("Select RETAILSTORETABLE.StoreNumber, DIRADDRESSBOOK.Description from RETAILSTORETABLE inner join DIRADDRESSBOOK ON RETAILSTORETABLE.STORENUMBER = DIRADDRESSBOOK.name where RETAILSTORETABLE.StoreNumber = '" + storeid + "'", connection);
                    SqlCommand command3;

                    command3 = new SqlCommand(@"SELECT TOP (1) LINE.PONUMBER, LINE.RECEIPTDATE, HEADER.DELIVERYNOTENUMBER  FROM POSPURCHASEORDERRECEIPT HEADER
JOIN POSPURCHASEORDERRECEIPTLINE LINE ON HEADER.PONUMBER = LINE.PONUMBER
where HEADER.PONumber = '" + this.PONumber + "'", connection);
                     
                          //command3 = new SqlCommand("Select DeliveryNoteNumber from POSPURCHASEORDERRECEIPT where PONumber = '" + this.PONumber + "'", connection);
                    
                        
                    using (reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            namatoko = reader2["Description"].ToString();
                        }
                    }

                    using (reader3 = command3.ExecuteReader())
                    {
                        while (reader3.Read())
                        {
                            deliverynote = reader3["DeliveryNoteNumber"].ToString();
                            receiptDate = reader3["RECEIPTDATE"].ToString();
                        }
                    }

                    if (deliverynote == string.Empty || receiptDate == string.Empty)
                    {
                        object[] parameterList = new object[] 
							{
								this.PONumber,
                                ApplicationSettings.Database.DATAAREAID.ToString()
								
								
							};

                        try
                        {
                            ReadOnlyCollection<object> containerArray = PurchaseOrderReceiving.InternalApplication.TransactionServices.InvokeExtension("getPackingSlipInfoPO", parameterList);


                            if (containerArray[2].ToString() == "Success")
                            {
                                deliverynote = containerArray[3].ToString();
                                receiptDate = containerArray[4].ToString();
                            }

                        }
                        catch (Exception ex)
                        {
                            LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                            throw;
                        }
             
                    }
                    if (statusReceipt != "REPRINT") //additional code for reprint by Yonathan 16/07/2024
                    {
                        s += "             -----------------------------------" + Environment.NewLine;
                        s += "                          " + statusReceipt + Environment.NewLine;
                        s += "             -----------------------------------" + Environment.NewLine;
                    }
                    //s += "             -----------------------------------" + Environment.NewLine;
                    //s += "                          " + statusReceipt + Environment.NewLine;
                    //s += "             -----------------------------------" + Environment.NewLine;
                    if (statusReceipt == "REPRINT") //additional code for reprint by Yonathan 16/07/2024
                    {
                        s += "             Tgl Reprint  : " + DateTime.Now.ToString() + Environment.NewLine;
                    }
                    s += "             Keterangan   : " + namatoko + Environment.NewLine +
                         "             No. Terima   : " + this.ReceiptNumber + Environment.NewLine +
                         "             Tgl Terima   : " + receiptDate + Environment.NewLine + //DateTime.Now.ToString() 
                         "             No. PO/TO    : " + this.PONumber + Environment.NewLine +
                        //   "Tgl PO       :" + "Tanggal PO" + Environment.NewLine + //Custom field
                        //  "Supplier     :" + "Supplier" + Environment.NewLine + //Custom Field
                         "             DO No        : " + deliverynote + Environment.NewLine; //delivery note number
                   

                    /*s += "-------------------------------------------------------" + Environment.NewLine +

                         "Kode Barang                       Unit              Qty " + Environment.NewLine +
                         "-------------------------------------------------------" + Environment.NewLine;*/

                    //       s += "------------------------------------------------" + Environment.NewLine;
                    s += "             -----------------------------------" + Environment.NewLine; // modif by Julius 14 07 2017
                    //s += "Kode Barang".PadRight(25) + "Unit".PadRight(5) + "Qty".PadLeft(4);
                    //s += "             Kode Barang".PadRight(32) + "Qty".PadLeft(10); //+ "Unit".PadLeft(5); modif by Yonathan 11/10/2022 disable Unit column
                    s += "             Kode Barang".PadRight(a) + "Qty".PadLeft(b); //+ "Unit".PadLeft(5); modif by Yonathan 11/10/2022 disable Unit column
                    //"Kode Barang                       Unit              Qty " 
                    s += Environment.NewLine;

                    reader = command.ExecuteReader();
                    //Offset = Offset + 160;
                    while (reader.Read())
                    {
                        //modif by Yonathan 18/10/2022
                        itemName = reader["ItemName"].ToString();
                        itemNumber = reader["ItemNumber"].ToString();

                        if (statusReceipt == "REPRINT")
                        {
                            qty = (Math.Truncate(Convert.ToDecimal(reader["QuantityReceived"]) * 1000m) / 1000m);
                        }
                        else
                        {
                            qty = (Math.Truncate(Convert.ToDecimal(reader["QuantityReceivedNow"]) * 1000m) / 1000m);
                        }
                        //mod by Yonathan 25/07/2023 to prevent item 0 qty receive to appear on the receipt.
                        if (qty != 0)
                        {
                            if (itemName.Length >= 18)
                            {
                                itemName = itemName.Substring(0, 18).PadRight(18, ' ');
                            }
                            else
                            {
                                itemName = itemName;
                                int countItemName = itemName.Length;
                                int addSpace = 18 - countItemName;
                                for (int i = 0; i < addSpace; i++)
                                {
                                    itemName += " ";
                                }
                            }
                            s += "             " + itemNumber + "-" + itemName;
                            //end add
                            /*
                            if (reader["ItemName"].ToString().Length > c)
                                s += "             " + reader["ItemNumber"].ToString() + "-" + reader["ItemName"].ToString().Substring(0, c).PadRight(d);
                            else
                                s += "             " + reader["ItemNumber"].ToString() + "-" + reader["ItemName"].ToString().PadRight(d);*/
                            //Modify by heron 140817- change the sequence

                            /*s += reader["Unit"].ToString().PadRight(5);
                            s += Convert.ToInt32(reader["QuantityReceivedNow"]).ToString().PadLeft(4) + Environment.NewLine;*/
                            /*decimal m = 199.123000000000m;
                            m = Math.Truncate(m * 1000m) / 1000m;
                            Console.WriteLine(m);*/
                            //add by Yonathan 17/10/2022

                            qtyStringMod = qty.ToString();
                            if (qty.ToString().Length == 7)
                            {
                                qtyString = qty.ToString();
                            }
                            else
                            {
                                qtyString = qty.ToString();
                                int stringCount = qty.ToString().Length;
                                int spaceToBeAdded = 7 - stringCount;
                                for (int i = 0; i < spaceToBeAdded; i++)
                                {
                                    qtyString += " ";
                                }
                            }
                            //end

                            //s += (Math.Truncate(Convert.ToDecimal(reader["QuantityReceivedNow"]) * 1000m) / 1000m).ToString().PadLeft(e) + Environment.NewLine;
                            s += qtyString.PadLeft(e) + Environment.NewLine;
                            //s += reader["QuantityReceivedNow"].ToString().PadLeft(4); //Convert.ToInt32(reader["QuantityReceivedNow"]).ToString().PadLeft(4);  disable by Yonathan 10/10/2022 because qty supports decimal
                            //s += reader["Unit"].ToString().PadLeft(4) + Environment.NewLine; modif by Yonathan 11/10/2022 disable Unit column

                            //End Modify by heron 140817- change the sequence



                            // Convert.ToInt32(reader["QuantityReceivedNow"]); disable by Yonathan 10/10/2022 because qty supports decimal
                            totalQtyDec += qty; //(Math.Truncate(Convert.ToDecimal(reader["QuantityReceivedNow"]) * 1000m) / 1000m);
                            Offset = Offset + 13;
                        }
                        
                    }

                    //     s += "------------------------------------------------" + Environment.NewLine;
                    s += "             -----------------------------------" + Environment.NewLine; // modif by Julius 14 07 2017
                    //s += "Total Qty    :".PadRight(27) + totalQty.ToString().PadLeft(7) + Environment.NewLine;
                    //s += "             Total Qty    :".PadRight(22) + totalQty.ToString().PadLeft(7) + Environment.NewLine; //disable by Yonathan to add support for decimal
                    s += "             Total Qty    :".PadRight(22) + totalQtyDec.ToString().PadLeft(15) + Environment.NewLine; //newly modified by yonathan 10/10/2022
                    //     s += "------------------------------------------------" + Environment.NewLine;
                    s += "             -----------------------------------" + Environment.NewLine; // modif by Julius 14 07 2017
                    s += Environment.NewLine + Environment.NewLine + Environment.NewLine;

                    s += "             Diterima Oleh     Diserahkan Oleh";
                    s += Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                    s += "             (_____________)   (_____________)";
                    //s += Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    //     Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                    //     Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    Offset = Offset + 260;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Format Error", ex);
                }
            }
            return s;
        }


        //To check qty receive
        private void checkQtyItem()
        {
            decimal totalQty = 0;
            foreach (DataRow row in entryTable.Rows)
            {
                
                decimal quantity = row.Field<decimal>(DataAccessConstants.QuantityOrdered) - row.Field<decimal>(DataAccessConstants.QuantityReceivedNow);

                if (quantity < 0)
                {
                    throw new Exception("Quantity receive greater than quantity ordered");
                }

                totalQty += row.Field<decimal>(DataAccessConstants.QuantityReceivedNow);

            }

            //add by yonathan 10/07/2024
            if (totalQty == 0)
            {
                throw new Exception("PO harus ada barang yang di-receive.\nTidak boleh kosong sama sekali.");
            }
        }

        //Add custom by Yonathan 23-09-2022
        public class parmRequest
        {
            public string ITEMID { get; set; }
            public string QTY { get; set; }
            public string UNITID { get; set; }
            public string DATAAREAID { get; set; }
            public string WAREHOUSE { get; set; }
            public string TYPE { get; set; }
            public string REFERENCESNUMBER { get; set; }
            public string RETAILVARIANTID { get; set; }
        }

        public class parmMultiRequest
        {
            public List<parmRequest> parmRequest { get; set; }
        }

        public static class MyJsonConverter
        {
            /// <summary>
            /// Deserialize an from json string
            /// </summary>
            public static T Deserialize<T>(string body)
            {
                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(body);
                    writer.Flush();
                    stream.Position = 0;
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
                }
            }

            /// <summary>
            /// Serialize an object to json
            /// </summary>
            public static string Serialize<T>(T item)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new DataContractJsonSerializer(typeof(T)).WriteObject(ms, item);
                    return Encoding.Default.GetString(ms.ToArray());
                }
            }
        }

        public string getFolderPath(string ProcessingDirectory, string typeFolder) //custom by YNWA
        {
            string Folder = "";

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(ProcessingDirectory);
            XmlNode xnodes = xdoc.SelectSingleNode("configuration");
            XmlNodeList xmlList = xnodes.SelectNodes("folderpath");

            foreach (XmlNode xmlNodeS in xmlList)
            {
                Folder += "," + xmlNodeS.Attributes.GetNamedItem(typeFolder).Value;
            }
            return Folder.Substring(1);
        }

        private void addCPInventoryOnHand()
        {

            //RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            //string itemId = "";
            //string qtyItem = "";
            //string unitId = "";
            string dataAreaId = "";
            string warehouseId = "";
            string configId = "";
            //string transType = "";
            //string refNumber = "";
            string siteId = "";
            string url = "";// "http://10.204.3.174:80/api/stockOnHand/addItemMultiple";
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            string functionName = "AddItemMultipleAPI";
            //url = getFolderPath(PathDirectory, "urlAPI") + "api/stockOnHand/addItemMultiple";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);
            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            string result = "";



            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
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
                            dataAreaId = reader["INVENTLOCATIONDATAAREAID"].ToString();
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

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", "PFM");
            List<string> multiPack = new List<string>();
            var packList = new List<parmRequest>();


            foreach (DataRow row in entryTable.Rows)
            {
                try
                {

                    string queryString2 = @"SELECT ITEMID, RETAILVARIANTID FROM [ax].[INVENTITEMBARCODE]
                                         WHERE ITEMID =  @ITEMID";


                    using (SqlCommand command = new SqlCommand(queryString2, connection))
                    {
                        command.Parameters.AddWithValue("@ITEMID", row.Field<string>(DataAccessConstants.ItemNumber));

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();

                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //configId = reader["CONFIGID"].ToString();
                                configId = reader["RETAILVARIANTID"].ToString();
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


                decimal quantity = row.Field<decimal>(DataAccessConstants.QuantityOrdered) - row.Field<decimal>(DataAccessConstants.QuantityReceivedNow);

                var pack = new parmRequest
                {
                    ITEMID = row.Field<string>(DataAccessConstants.ItemNumber),
                    QTY = row.Field<decimal>(DataAccessConstants.QuantityReceivedNow).ToString().Replace(",", "."), //lines.QuantityOrdered.ToString(),
                    UNITID = row.Field<string>(DataAccessConstants.UnitOfMeasure),
                    DATAAREAID = dataAreaId,
                    WAREHOUSE = warehouseId,
                    TYPE = this.prType == PRCountingType.TransferIn ? "0" : "1",
                    REFERENCESNUMBER = this.prType == PRCountingType.TransferIn ? row.Field<string>(2) : row.Field<string>(DataAccessConstants.PoNumber),
                    RETAILVARIANTID = configId//this.saleLineItem.Dimension.VariantId
                };
                //var data = MyJsonConverter.Serialize(pack);
                packList.Add(pack);
            }
            var multiData = MyJsonConverter.Serialize(packList);


            using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(multiData);
                streamWriter.Flush();
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }


        }
        //end custom


        //End Begin Add NEC-Hmz
        //<CPPOTOCancel>
        private void checkQtyReceived()
        {
            quantityTotal = 0;

            foreach (DataRow row in entryTable.Rows)
            {
                decimal quantity = row.Field<decimal>(DataAccessConstants.QuantityReceivedNow);
                quantityTotal = quantityTotal + quantity;


            }
            if (quantityTotal == 0)
            { 
                throw new Exception("There is no quantity to received, please click receive all");
            }
        }


        private void btnCommit_Click(object sender, EventArgs e)
        {
            string tempDriverDetails;
            /*LoadReceiptLinesFromDB();
            //Begin add NEC hmz to manipulate PO Id with driver Details
            if (this.prType == PRCountingType.PurchaseOrder)// || this.prType == PRCountingType.TransferIn)
                tempDriverDetails = this.PONumber + "-" + txtDelivery.Text;
            else
                tempDriverDetails = this.PONumber;
            */
            try
            {
                //Begin add NEC
                if (txtDelivery.Text == "")
                {
                    // Show commit failure message                  
                    throw new Exception("Please Fill the Delivery note number");
                }
                //check item qty
                this.checkQtyItem();
                //End add NEC
                //<CPPOTOCancel>
                if (this.prType == PRCountingType.TransferIn)
                {
                    this.checkQtyReceived();
                }
                //</CPPOTOCancel>
                // Save lines to local database
                SaveReceipt();

                LoadReceiptLinesFromDB();
                //Begin add NEC hmz to manipulate PO Id with driver Details
                if (this.prType == PRCountingType.PurchaseOrder)// || this.prType == PRCountingType.TransferIn)
                    tempDriverDetails = this.PONumber + "-" + txtDelivery.Text;
                else
                    tempDriverDetails = this.PONumber;
                //End add NEC hmz

                // Commit receipt to AX via webservice
                // Begin modify line NEC - to pass tempDriverDetails
                IPRDocument prDoc = PurchaseOrderReceiving.InternalApplication.Services.StoreInventoryServices.CommitOrderReceipt(tempDriverDetails, this.ReceiptNumber, this.prType);

                tempDriverDetails = this.PONumber;
                // Remove rows that are successfully submitted
                List<DataRow> removeRows = new List<DataRow>();

                // Success commmit
                if (prDoc != null)
                {

                    // print here
                    // Begin add NEC Hmz to custom print
                    string s = this.ReceiveDocumentFormat("ORIGINAL");

                    PrintDocument p = new PrintDocument();
                    PrintDialog pd = new PrintDialog();
                    PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
                    Margins margins = new Margins(0, 0, 0, 0);

                    pd.Document = p;
                    pd.Document.DefaultPageSettings.PaperSize = psize;
                    pd.Document.DefaultPageSettings.Margins = margins;
                    p.DefaultPageSettings.PaperSize.Width = 600;

                    //add by Yonathan to CHeck whether the printer is online 06082024
                    bool isPrinterOffline = checkPrinterStatus(p);
                    if (isPrinterOffline == false)
                    {
                        p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                        {
                            //e1.Graphics.DrawString(s, new Font("Courier New", 9), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left + 100, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                            //modif by Julius 14 07 2017
                            e1.Graphics.DrawString(s, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

                        };
                        try
                        {
                            //Edit by Erwin 23 October 2019
                            //for (int i = 1; i <= 2; i++)
                            //{
                            //    p.Print();
                            //}

                            p.Print();

                            //End Edit by Erwin 23 October 2019
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Exception Occured While Printing", ex);
                        }

                        //add by Erwin 23 October 2019 print copy receipt
                        string sCopy = this.ReceiveDocumentFormat("COPY");

                        PrintDocument pCopy = new PrintDocument();
                        PrintDialog pdCopy = new PrintDialog();
                        PaperSize psizeCopy = new PaperSize("Custom", 100, Offset + 236);
                        Margins marginsCopy = new Margins(0, 0, 0, 0);

                        pdCopy.Document = pCopy;
                        pdCopy.Document.DefaultPageSettings.PaperSize = psizeCopy;
                        pdCopy.Document.DefaultPageSettings.Margins = marginsCopy;
                        pCopy.DefaultPageSettings.PaperSize.Width = 600;
                        pCopy.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                        {
                            //e1.Graphics.DrawString(s, new Font("Courier New", 9), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left + 100, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                            //modif by Julius 14 07 2017
                            e1.Graphics.DrawString(sCopy, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(pCopy.DefaultPageSettings.PrintableArea.Left, 0, pCopy.DefaultPageSettings.PrintableArea.Width, pCopy.DefaultPageSettings.PrintableArea.Height));

                        };
                        try
                        {

                            pCopy.Print();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Exception Occured While Printing", ex);
                        }
                    }                    
                    else
                    {
                        using (frmMessage frm = new frmMessage(errorPrinter, MessageBoxButtons.OK, MessageBoxIcon.Error))
                        {
                            POSFormsManager.ShowPOSForm(frm);
                        }
                    }

                   

                    //END add by Erwin 23 October 2019 print copy receipt

                    // End add NEC Hmz

                    //Add customization by Yonathan 23 Sept 2022 to input the transaction to CPINVENTORYONHAND table for tracking daily on-hand

                    this.addCPInventoryOnHand();

                    //end customization


                    foreach (DataRow row in entryTable.Rows)
                    {
                        IPRDocumentLine updatedLine = prDoc.PRDocumentLines.Where(line => string.Equals(line.Guid, row.Field<string>(DataAccessConstants.Guid), StringComparison.OrdinalIgnoreCase)
                            && line.UpdatedInAx == true).FirstOrDefault();

                        if (updatedLine != null)
                        {
                            removeRows.Add(row);
                        }
                    }
                }

                if (removeRows.Count > 0)
                {
                    foreach (DataRow row in removeRows)
                    {
                        // Remove line from local DB
                        receiptData.DeleteReceiptLine(row.Field<string>(DataAccessConstants.Guid));

                        // Remove line from form
                        entryTable.Rows.Remove(row);
                        entryTable.AcceptChanges();
                    }

                    if (removeRows.Count == prDoc.PRDocumentLines.Count)
                    {
                        // Delete header if all lines are removed
                        receiptData.DeleteReceipt(prDoc.RecId);

                        // Show commit succeeded message
                        using (frmMessage dialog = new frmMessage(10314011, MessageBoxButtons.OK, MessageBoxIcon.Information))
                        {
                            POSFormsManager.ShowPOSForm(dialog);
                            if (dialog.DialogResult == DialogResult.OK)
                            {
                                this.DialogResult = DialogResult.OK;
                                Close();
                            }
                        }
                    }
                    else
                    {
                        // Show partial commit success message
                        using (frmMessage dialog = new frmMessage(10314012, MessageBoxButtons.OK, MessageBoxIcon.Information))
                        {
                            POSFormsManager.ShowPOSForm(dialog);
                        }

                        grInventory.DataSource = entryTable;
                        entryTable.AcceptChanges();
                    }
                }
                else
                {
                    // Show commit failure message
                    using (frmMessage dialog = new frmMessage(10314013, MessageBoxButtons.OK, MessageBoxIcon.Information))
                    {
                        POSFormsManager.ShowPOSForm(dialog);
                    }

                    grInventory.DataSource = entryTable;
                    entryTable.AcceptChanges();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            gvInventory.MoveNextPage();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gvInventory.MoveNext();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gvInventory.MovePrev();
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            gvInventory.MovePrevPage();
        }

        /// <summary>
        /// This method Opens Dimesions Screen.
        /// </summary>
        /// <param name="barcodeInfo"></param>
        private bool OpenItemDimensionsDialog(IBarcodeInfo barcodeInfo)
        {
            bool returnValue = false;
            // Get the dimensions
            DataTable inventDimCombination = PurchaseOrderReceiving.InternalApplication.Services.Dimension.GetDimensions(saleLineItem.ItemId);

            // Get the dimensions
            DimensionConfirmation dimensionConfirmation = new DimensionConfirmation()
            {
                InventDimCombination = inventDimCombination,
                DimensionData = saleLineItem.Dimension,
                DisplayDialog = !barcodeInfo.Found
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(dimensionConfirmation, () =>
            {
                if (dimensionConfirmation.Confirmed)
                {
                    if (dimensionConfirmation.SelectDimCombination != null)
                    {
                        DataRow dr = dimensionConfirmation.SelectDimCombination;
                        saleLineItem.Dimension.VariantId = dr.Field<string>("VARIANTID");
                        saleLineItem.Dimension.ColorId = dr.Field<string>("COLORID");
                        saleLineItem.Dimension.ColorName = dr.Field<string>("COLOR");
                        saleLineItem.Dimension.SizeId = dr.Field<string>("SIZEID");
                        saleLineItem.Dimension.SizeName = dr.Field<string>("SIZE");
                        saleLineItem.Dimension.StyleId = dr.Field<string>("STYLEID");
                        saleLineItem.Dimension.StyleName = dr.Field<string>("STYLE");
                        saleLineItem.Dimension.ConfigId = dr.Field<string>(DataAccessConstants.ConfigId);
                        saleLineItem.Dimension.ConfigName = dr.Field<string>("CONFIG");
                        saleLineItem.Dimension.DistinctProductVariantId = (Int64)dr["DISTINCTPRODUCTVARIANT"];

                        if (string.IsNullOrEmpty(saleLineItem.BarcodeId))
                        {   // Pick up if not previously set
                            saleLineItem.BarcodeId = dr.Field<string>("ITEMBARCODE");
                        }

                        string unitId = dr.Field<string>("UNITID");
                        if (!String.IsNullOrEmpty(unitId))
                        {
                            saleLineItem.SalesOrderUnitOfMeasure = unitId;
                        }
                    }
                    returnValue = true;
                }
            }
            );

            PurchaseOrderReceiving.InternalApplication.Services.Interaction.InteractionRequest(request);

            return returnValue;
        }

        private void OpenUnitOfMeasureDialog()
        {
            try
            {
                DataRow selectedRow = GetCurrentRow();
                string itemNumber = (string)selectedRow[DataAccessConstants.ItemNumber];
                string uom = (string)selectedRow[DataAccessConstants.UnitOfMeasure];

                SaleLineItem selectedItem = (SaleLineItem)PurchaseOrderReceiving.InternalApplication.BusinessLogic.Utility.CreateSaleLineItem();

                selectedItem.ItemId = itemNumber;
                selectedItem.InventOrderUnitOfMeasure = uom;
                selectedItem.SalesOrderUnitOfMeasure = uom;

                if (frmUOMList.PromptAndChangeSalesUnitOfMeasure(selectedItem))
                {
                    selectedRow[DataAccessConstants.UnitOfMeasure] = selectedItem.SalesOrderUnitOfMeasure;
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// This method Adds the variant information.
        /// </summary>
        /// <param name="countTableComment"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private DataTable AddVariantInfo(DataTable countTableComment)
        {
            string commentColName = "COMMENT";

            DataColumn col = new DataColumn(commentColName, typeof(string));

            if (!countTableComment.Columns.Contains(commentColName))
            {
                countTableComment.Columns.Add(col);
            }

            this.saleLineItem = (SaleLineItem)PurchaseOrderReceiving.InternalApplication.BusinessLogic.Utility.CreateSaleLineItem(
                    ApplicationSettings.Terminal.StoreCurrency,
                    PurchaseOrderReceiving.InternalApplication.Services.Rounding,
                    posTransaction);
            foreach (DataRow row in countTableComment.Rows)
            {
                this.saleLineItem.Dimension.ColorName = row.Field<string>("COLOR");
                this.saleLineItem.Dimension.SizeName = row.Field<string>("SIZE");
                this.saleLineItem.Dimension.StyleName = row.Field<string>("STYLE");
                this.saleLineItem.Dimension.ConfigName = row.Field<string>("CONFIG");

                this.saleLineItem.Dimension.ColorId = row[DataAccessConstants.InventColorId] as string;
                this.saleLineItem.Dimension.SizeId = row[DataAccessConstants.InventSizeId] as string;
                this.saleLineItem.Dimension.StyleId = row[DataAccessConstants.InventStyleId] as string;
                this.saleLineItem.Dimension.ConfigId = row[DataAccessConstants.ConfigId] as string;

                row[commentColName] = ColorSizeStyleConfig(saleLineItem.Dimension);
            }

            return countTableComment;
        }

        /// <summary>
        /// This method formats the Variant information 
        /// </summary>
        /// <param name="saleItem"></param>
        /// <returns></returns>
        private static string ColorSizeStyleConfig(Dimensions dimension)
        {
            string dash = " - ";
            string color = Dimensions.DimensionValue(dimension.ColorName, dimension.ColorId);
            string size = Dimensions.DimensionValue(dimension.SizeName, dimension.SizeId);
            string style = Dimensions.DimensionValue(dimension.StyleName, dimension.StyleId);
            string config = Dimensions.DimensionValue(dimension.ConfigName, dimension.ConfigId);
            string[] dimensionValues = { color, size, style, config };

            StringBuilder colorSizeStyleConfig = new StringBuilder(String.Empty);
            for (int i = 0; i < dimensionValues.Length; i++)
            {
                if (dimensionValues[i].Length > 0)
                {
                    if (colorSizeStyleConfig.Length > 0)
                    {
                        colorSizeStyleConfig.Append(dash);
                    }
                    colorSizeStyleConfig.Append(dimensionValues[i]);
                }
            }

            return colorSizeStyleConfig.ToString();
        }

        private bool ValidateData(EntryItem item, ISaleLineItem lineItem)
        {
            bool valid = true;

            // Check if the unit allows any decimals
            UnitOfMeasureData uomData = new UnitOfMeasureData(
                ApplicationSettings.Database.LocalConnection,
                ApplicationSettings.Database.DATAAREAID,
                ApplicationSettings.Terminal.StorePrimaryId,
                PurchaseOrderReceiving.InternalApplication);
            //add by Yonathan 14 Oct 2022
            int unitDecimals;
            if (item.Unit != "")
            {
                unitDecimals = uomData.GetUnitDecimals(item.Unit);
            }
            else
            {
                DataRow row = GetCurrentRow();
                unitDecimals = uomData.GetUnitDecimals(row.Field<string>(DataAccessConstants.UnitOfMeasure));


            }
            //end add
            
            //int unitDecimals = uomData.GetUnitDecimals(item.Unit); //add by Yonathan 14 Oct 2022

            if ((unitDecimals == 0) && ((item.QuantityReceived - (int)item.QuantityReceived) != 0))
            {
                string msg = ApplicationLocalizer.Language.Translate(103157, (object)item.ItemNumber);

                using (frmMessage frm = new frmMessage(msg, MessageBoxButtons.OK, MessageBoxIcon.Warning))
                {
                    POSFormsManager.ShowPOSForm(frm);
                }
                valid = false;
            }

            if (this.prType == PRCountingType.TransferOut || this.prType == PRCountingType.PickingList)
            {
                decimal qtyReceived = item.QuantityReceived;
                if (!this.isEdit)
                {
                    DataRow row = this.GetRowByItemIdAndVariants(saleLineItem);
                    if (row != null)
                    {
                        qtyReceived += row.Field<decimal>(DataAccessConstants.QuantityReceivedNow);
                    }
                }

                if (qtyReceived + lineItem.QuantityPickedUp > lineItem.QuantityOrdered)
                {
                    using (frmMessage frm = new frmMessage(99521, MessageBoxButtons.OK, MessageBoxIcon.Warning))
                    {
                        POSFormsManager.ShowPOSForm(frm);
                    }
                    valid = false;
                }
            }

            return valid;
        }

        private bool ValidateItemIdAndVariantsForPicking(ISaleLineItem lineItem)
        {
            bool isValid = true;
            if (this.prType == PRCountingType.TransferOut || this.prType == PRCountingType.PickingList)
            {
                DataRow row = this.GetRowByItemIdAndVariants(lineItem);

                isValid = (row != null);

                if (!isValid)
                {
                    using (frmMessage frm = new frmMessage(99523, MessageBoxButtons.OK, MessageBoxIcon.Warning))
                    {
                        POSFormsManager.ShowPOSForm(frm);
                    }
                }
            }

            return isValid;
        }

        private void AddNewRow(EntryItem item)
        {
            // Add a new row
            DataRow row = entryTable.NewRow();

            row[DataAccessConstants.ItemNumber] = item.ItemNumber;
            row[DataAccessConstants.ItemName] = item.ItemName;
            row[DataAccessConstants.QuantityOrdered] = 0;
            row[DataAccessConstants.QuantityReceived] = 0;
            row[DataAccessConstants.QuantityReceivedNow] = item.QuantityReceived;
            row[DataAccessConstants.UnitOfMeasure] = item.Unit;
            row[DataAccessConstants.ReceiptDate] = DateTime.Now;
            row[DataAccessConstants.UserId] = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
            row[DataAccessConstants.TerminalId] = ApplicationSettings.Terminal.TerminalId;
            row[DataAccessConstants.InventSizeId] = this.saleLineItem.Dimension.SizeId ?? string.Empty;
            row[DataAccessConstants.InventColorId] = this.saleLineItem.Dimension.ColorId ?? string.Empty;
            row[DataAccessConstants.InventStyleId] = this.saleLineItem.Dimension.StyleId ?? string.Empty;
            row[DataAccessConstants.ConfigId] = this.saleLineItem.Dimension.ConfigId ?? string.Empty;
            row["COMMENT"] = ColorSizeStyleConfig(saleLineItem.Dimension);

            try
            {
                entryTable.Rows.Add(row);
            }
            catch (ArgumentException ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            catch (NoNullAllowedException ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            catch (ConstraintException ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
        }

        private void GridView_RowClickEventHandler(object sender, RowClickEventArgs e)
        {
            DataRow row = GetCurrentRow();
            numPad1.Focus();

            if (row != null)
            {
                string itemNumber = row.Field<string>(DataAccessConstants.ItemNumber);
                if (this.GetItemInfo(itemNumber, true))
                {
                    this.SetQuantitiesFromRow(this.saleLineItem);
                }
            }
        }

        private void gvInventory_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.isMixedDeliveryMode)
            {
                this.txtDelivery.Text = string.Empty;
                DataRow row = this.gvInventory.GetDataRow(e.FocusedRowHandle);

                if (row != null)
                {
                    string lineItemDeliveryMethod = row[DataAccessConstants.DeliveryMethod] as string;

                    if (lineItemDeliveryMethod != null)
                    {
                        this.txtDelivery.Text = lineItemDeliveryMethod;
                    }
                }
            }
        }

        private void DeleteTmpLine()
        {

            string connectionString = GetSettingFromConfigFile();
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        connection.Open();
                        cmd.CommandText = "DELETE FROM POSPURCHASEORDERRECEIPTLINE WHERE PONUMBER = @po";
                        cmd.Parameters.AddWithValue("@po", this.PONumber);
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("delete successful");
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
        }

        private string cpGetDataDocumentBuffer()//using real-time service
        {
            string deliveryNote = "";

            //string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            //string connectionString = @"Data Source= DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=DevDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";//Persist Security Info=False;User ID=USER_NAME;Password=USER_PASS;
            //string connectionString = @"Data Source= DYNAMICS16\SQLAXDB1 ;Initial Catalog=PRDDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";

            //SqlConnection connection = new SqlConnection(connectionString); //LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            object[] parameterList = new object[] 
							{
								txtPoNumber.Text.ToString(),
                                ApplicationSettings.Database.DATAAREAID.ToString()
								
								
							};


            //ReadOnlyCollection<object> containerArray2 = PurchaseOrderReceiving.InternalApplication.TransactionServices.InvokeExtension("getStockOnHand", parameterList);

            
            try
            {
                ReadOnlyCollection<object> containerArray = PurchaseOrderReceiving.InternalApplication.TransactionServices.InvokeExtension("getDataDocumentBuffer", parameterList);


                if (containerArray[2].ToString() == "true")
                {
                    deliveryNote = containerArray[3].ToString();
                }
                    
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
             
            

            return deliveryNote;
        }

        private string cpGetDataDocumentBufferOld() //using CPCONNECTION
        {
            string deliveryNote = "";

            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            //string connectionString = @"Data Source= DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=DevDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";//Persist Security Info=False;User ID=USER_NAME;Password=USER_PASS;
            //string connectionString = @"Data Source= DYNAMICS16\SQLAXDB1 ;Initial Catalog=PRDDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";

            SqlConnection connection = new SqlConnection(connectionString); //LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            
            try
            {
                string queryString = @" SELECT DOCUMENTID FROM NECI_POSDocumentBuffer
                                        WHERE DOCUMENTID = @PURCHID AND DATAAREAID = @DATAAREA ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@PURCHID", txtPoNumber.Text);
                    command.Parameters.AddWithValue("@DATAAREA", ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deliveryNote = reader.GetString(0);
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

            return deliveryNote;
        }

        private void lblDeliveryMethod_Click(object sender, EventArgs e)
        {

        }

        private void txtDelivery_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9 ]*$");

            if (!regexItem.IsMatch(txtDelivery.Text))
            {
                MessageBox.Show("Delivery Note input must not use symbol");
                e.Cancel = true;
            }
            else
                if (txtDelivery.Text.Length > 20)
                {
                    MessageBox.Show("Delivery Note maximum character is 20");
                    e.Cancel = true;
                }
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            //add by Yonathan 13/07/2023 for select row based on text filter
            if (txtSearchBox.Text != "Search Item number or Description")
            {
                int foundItemId = 0;
                int foundItemName = 0;
                string searchText = txtSearchBox.Text.Trim().ToLower(); // Get the text from the TextBox and remove leading/trailing spaces

                // Loop through the rows of the GridView and select the ones that match the search text
                for (int i = 0; i < gvInventory.RowCount && foundItemId == 0; i++)
                {
                    // Assuming the value you want to match is in a specific column (e.g., column with FieldName "ColumnName")
                    string cellValue = gvInventory.GetRowCellValue(i, "ITEMNUMBER").ToString();

                    if (cellValue.Contains(searchText))
                    {
                        foundItemId = 1;
                        int rowHandle = gvInventory.LocateByValue("ITEMNUMBER", cellValue);
                        if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                            gvInventory.FocusedRowHandle = rowHandle;
                    }
                    else
                    {
                        // Deselect rows that don't match the search text
                        gvInventory.UnselectRow(i);
                    }
                }

                //if not found itemid, search for the item description
                for (int i = 0; i < gvInventory.RowCount && foundItemName == 0; i++)
                {
                    // Assuming the value you want to match is in a specific column (e.g., column with FieldName "ColumnName")
                    string cellValueDesc = gvInventory.GetRowCellValue(i, "ITEMNAME").ToString();
                    string cellValueDescToLower = cellValueDesc.ToLower();

                    if (cellValueDescToLower.Contains(searchText))
                    {
                        foundItemName = 1;
                        int rowHandle = gvInventory.LocateByValue("ITEMNAME", cellValueDesc);
                        if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                            gvInventory.FocusedRowHandle = rowHandle;
                    }
                    else
                    {
                        // Deselect rows that don't match the search text
                        gvInventory.UnselectRow(i);
                    }
                }
            }
            

           
        }
        //add by Yonathan 13/07/2023
        private void searchBox_Enter(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == "Search Item number or Description")
            {
                txtSearchBox.Text = "";
                txtSearchBox.ForeColor = Color.Black;
            }
        }

        private void searchBox_Leave(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == "")
            {
                txtSearchBox.Text = "Search Item number or Description";
                txtSearchBox.ForeColor = Color.Gray;
            }
        }
    }
}
