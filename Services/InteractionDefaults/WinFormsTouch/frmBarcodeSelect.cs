/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
   [Export("BarcodeForm", typeof(IInteractionView))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmBarcodeSelect : frmTouchBase, IInteractionView
    {
        private DataTable barcodeTable = new DataTable();
        private String selectedBarcodeId;

        public String SelectedBarcodeId
        {
            get { return selectedBarcodeId; }
        }

        public frmBarcodeSelect()
        {
            // Required for Windows Form Designer support            
            InitializeForm();
        }
        
       
       /// <summary>
        /// Required designer variable.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmBarcodeSelect(BarcodeConfirmation barcodeTableConfirmation)
            : this()
        {

            int count = barcodeTableConfirmation.BarcodeTable.Rows.Count;
            this.barcodeTable = barcodeTableConfirmation.BarcodeTable;
          
        }

        private void frmBarcodeSelect_Load(object sender, EventArgs e)
        {
            this.Text = ApplicationLocalizer.Language.Translate(1348); //Select barcode
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


        private void InitializeForm()
        {
            InitializeComponent();
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmMessage are reserved at 1340 - 1359
            // In use now are ID's 1340 - 1346
            //
            //buttons 
            btnClose.Text = ApplicationLocalizer.Language.Translate(2360); //Close
            btnSelect.Text = ApplicationLocalizer.Language.Translate(2361); //Select
            btnUp.Text = ApplicationLocalizer.Language.Translate(2362); //Up
            btnDown.Text = ApplicationLocalizer.Language.Translate(2363); //Down
            btnPgUp.Text = ApplicationLocalizer.Language.Translate(2364); //Pg Up
            btnPgDown.Text = ApplicationLocalizer.Language.Translate(2365); //Pg Down
            //Labels
            colDescription.Caption  = ApplicationLocalizer.Language.Translate(2366); //Description
            colItemBarcode.Caption = ApplicationLocalizer.Language.Translate(2367); //Item Barcode
            colQty.Caption = ApplicationLocalizer.Language.Translate(2368); //Qantity 
            colUnitId.Caption = ApplicationLocalizer.Language.Translate(2369); //UnitId

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

        private void frmBarcodeSelect_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void frmBarcodeSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SelectBarcode();
            }
        }

        private void grBarcodes_DoubleClick(object sender, EventArgs e)
        {
            SelectBarcode();
        }

        #region IInteractionView implementation

       /// <summary>
       /// Initialize the form
       /// </summary>
       /// <typeparam name="TArgs">Prism Notification type</typeparam>
       /// <param name="args">Notification</param>
       public void Initialize<TArgs>(TArgs args)
            where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }


       /// <summary>
       /// Return the results of the interation call
       /// </summary>
       /// <typeparam name="TResults"></typeparam>
       /// <returns>Returns the TResults object</returns>
       public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new BarcodeConfirmation
            {
                SelectedBarcodeId = this.SelectedBarcodeId,
                Confirmed = this.DialogResult == DialogResult.OK
            } as TResults;
        }

        #endregion
    }
}