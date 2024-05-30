using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using GME_Custom;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormPaymentCash", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmPaymentCash : frmTouchBase, IInteractionView
    {
        private bool confirmVar = false;        
        private decimal payAmountVar = 0;
        private bool operationDoneVar = false;
	    private decimal payCashAmount = 0;

        public frmPaymentCash()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmPaymentCash(PayCashConfirmation payCashConfirmation)
            : this()
        {

            if (GME_Custom.GME_Propesties.GME_Var.isCashVoucher || GME_Custom.GME_Propesties.GME_Var.partialPaymentEDC)
            {
                this.Hide();
            }
            else
            {
                this.LblAmountDue.Text += GME_Custom.GME_Propesties.GME_Method.getAmountDecimal(payCashConfirmation.BalanceAmount.ToString());
                this.BtnAmountDue.Text += GME_Custom.GME_Propesties.GME_Method.getAmountDecimal(payCashConfirmation.BalanceAmount.ToString());
                this.label3.Text += GME_Custom.GME_Propesties.GME_Method.getAmountDecimal(payCashConfirmation.BalanceAmount.ToString()).Replace(".00","");
		        payCashAmount += payCashConfirmation.BalanceAmount;
            }


            Numpad.NumberOfDecimals = 0;
            Numpad.EntryStartsInDecimals = true;            
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new PayCashConfirmation
            {
                OperationDone = this.operationDoneVar,
                RegisteredAmount = this.payAmountVar,
                Confirmed = this.confirmVar
            } as TResults;
            //return null;
        }
        /*Interactionview Implementation for Argument send (current Null)*/
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            //throw new System.NotImplementedException();
        }
        #endregion

        //private void BtnOk_Click(object sender, EventArgs e)
        //{            
        //    this.DialogResult = DialogResult.OK;
        //    this.confirmVar = true;
        //    this.operationDoneVar = true;
        //    this.payAmountVar = Numpad.EnteredDecimalValue;
        //    this.Close();
        //}

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.confirmVar = false;
            this.operationDoneVar = false;
            this.Close();
        }

        private void BtnAmountDue_Click(object sender, EventArgs e)
        {
            decimal amount = payCashAmount;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount100K_Click(object sender, EventArgs e)
        {
            decimal amount = 100000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount150K_Click(object sender, EventArgs e)
        {
            decimal amount = 150000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount200K_Click(object sender, EventArgs e)
        {
            decimal amount = 200000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount250K_Click(object sender, EventArgs e)
        {
            decimal amount = 250000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount300K_Click(object sender, EventArgs e)
        {
            decimal amount = 300000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount350K_Click(object sender, EventArgs e)
        {
            decimal amount = 350000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void BtnAmount400K_Click(object sender, EventArgs e)
        {
            decimal amount = 400000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void numPad1_EnterButtonPressed()
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = Numpad.EnteredDecimalValue;
            this.Close();
        }

        private void BtnAmount500K_Click(object sender, EventArgs e)
        {
            decimal amount = 500000;
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = amount;
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = payCashAmount;
            this.Close();
        }


        private void frmPaymentCash_Load_1(object sender, EventArgs e)
        {
            if (GME_Custom.GME_Propesties.GME_Var.isCashVoucher || GME_Custom.GME_Propesties.GME_Var.partialPaymentEDC)
            {
                this.DialogResult = DialogResult.OK;
                this.confirmVar = true;
                this.operationDoneVar = true;
                if (GME_Custom.GME_Propesties.GME_Var.isCashVoucher)
                {
                    this.payAmountVar = GME_Custom.GME_Propesties.GME_Var.amountTBSI;
                    GME_Custom.GME_Propesties.GME_Var.isCashVoucher = false; //matikan setelah masuk
                }
                else if (GME_Custom.GME_Propesties.GME_Var.partialPaymentEDC)
                {
                    this.payAmountVar = GME_Custom.GME_Propesties.GME_Var.paycardofflineamount;
                    GME_Custom.GME_Propesties.GME_Var.partialPaymentEDC = false; //matikan setelah masuk
                }

                this.Close();
            }
        }
    }
}
