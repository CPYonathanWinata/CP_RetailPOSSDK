/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Data;
using LSRetailPosis.POSProcesses;

namespace LSRetailPosis.ButtonGrid
{
    public partial class frmBarcodeSelect : frmTouchBase
    {
        private DataTable barcodeTable = new DataTable();
        private String selectedBarcodeId;

        public String SelectedBarcodeId
        {
            get { return selectedBarcodeId; }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        public frmBarcodeSelect(DataTable barcodeTable)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            this.barcodeTable = barcodeTable;
          
        }

        private void frmBarcodeSelect_Load(object sender, EventArgs e)
        {
            this.lblHeader.Text = ApplicationLocalizer.Language.Translate(1348); //Select barcode
            grBarcodes.DataSource = barcodeTable;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectBarcode();
        }

        private void SelectBarcode()
        {
            if (gridView1.RowCount > 0)
            {
                System.Data.DataRow Row = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
                selectedBarcodeId = (string)Row["ITEMBARCODE"];
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void grBarcodes_Click(object sender, EventArgs e)
        {
            SelectBarcode();
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            gridView1.MovePrevPage();
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            gridView1.MoveNextPage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gridView1.MovePrev();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gridView1.MoveNext();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}