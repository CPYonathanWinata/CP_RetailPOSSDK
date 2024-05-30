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

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPGrabmartOrder : Form
    {
        IPosTransaction posTransaction;

        public CPGrabmartOrder(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
            iSale.OperationID = PosisOperations.ItemSale;
            iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
            //iSale.Barcode = skuId; disable by Yonathan 21/10/2022
            iSale.Barcode = "10130000";  // change to this by yonathan 21/10/2022
            //iSale.BarcodeInfo.ItemId = txtSKU.Text;
            iSale.OperationInfo.NumpadQuantity = 2;
            iSale.POSTransaction = (LSRetailPosis.Transaction.PosTransaction)posTransaction;

            iSale.RunOperation();
        }
    }
}
