using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSRetailPosis.Transaction;
using LSRetailPosis.Settings;

using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_EngageFALWSServices;
using System.Threading;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPPayGoPay : Form
    {
        public CPPayGoPay()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtAmount.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Amount must be filled", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                }
                //MessageBox.Show("Amount must be filled");
                txtAmount.Focus();
            }
            else if(txtHP.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Handphone No. must be filled", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                }

                txtHP.Focus();
            }
            else if (txtReff.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Reference No. must be filled", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                }
                //MessageBox.Show("Reference No. must be filled");
                txtReff.Focus();
            }
            else if (txtCustomerName.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Customer Name must be filled", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                }
                //MessageBox.Show("Customer Name must be filled");
                txtCustomerName.Focus();
            }
            
        }

        private void numPad1_EnterButtonPressed()
        {
            txtAmount.Text = numPad1.EnteredValue;
        }

        private void CPPayGoPay_Load(object sender, EventArgs e)
        {
            numPad1.EnteredValue = Math.Round(((RetailTransaction)BlankOperations.globalposTransaction).NetAmountWithTax, 2).ToString();
            txtAmount.Text = numPad1.EnteredValue;
            labelAmountDueValue.Text = "IDR " + Math.Round(((RetailTransaction)BlankOperations.globalposTransaction).NetAmountWithTax, 2).ToString();

            amtCustAmounts.LocalCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
            amtCustAmounts.UsedCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
            amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;

            amtCustAmounts.HighestOptionAmount = Math.Round(((RetailTransaction)BlankOperations.globalposTransaction).NetAmountWithTax, 0);
            amtCustAmounts.LowesetOptionAmount = Math.Round(((RetailTransaction)BlankOperations.globalposTransaction).NetAmountWithTax, 0);

            amtCustAmounts.SoldLocalAmount = Math.Round(((RetailTransaction)BlankOperations.globalposTransaction).NetAmountWithTax, 0);
            amtCustAmounts.SetButtons();
        }
    }
}
