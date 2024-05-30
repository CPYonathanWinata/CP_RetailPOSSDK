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

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormPaymentCash", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmPaymentCash : frmTouchBase, IInteractionView
    {
        private bool confirmVar = false;        
        private decimal payAmountVar = 0;
        private bool operationDoneVar = false;

        public frmPaymentCash()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmPaymentCash(PayCashConfirmation payCashConfirmation)
            : this()
        {
            this.LblAmountDue.Text += GME_Custom.GME_Propesties.GME_Method.getAmountDecimal(payCashConfirmation.BalanceAmount.ToString());
            this.BtnAmountDue.Text += GME_Custom.GME_Propesties.GME_Method.getAmountDecimal(payCashConfirmation.BalanceAmount.ToString());

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

        private void BtnOk_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = Numpad.EnteredDecimalValue;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.confirmVar = false;
            this.operationDoneVar = false;
            this.Close();
        }

        private void BtnAmountDue_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmountDue.Text);
            this.Close();
        }

        private void BtnAmount100K_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount100K.Text);
            this.Close();
        }

        private void BtnAmount150K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount150K.Text);
            this.Close();
        }

        private void BtnAmount200K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount200K.Text);
            this.Close();
        }

        private void BtnAmount250K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount250K.Text);
            this.Close();
        }

        private void BtnAmount300K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount300K.Text);
            this.Close();
        }

        private void BtnAmount350K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount350K.Text);
            this.Close();
        }

        private void BtnAmount400K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount400K.Text);
            this.Close();
        }

        private void numPad1_EnterButtonPressed()
        {
            payAmountVar = Numpad.EnteredDecimalValue;                                
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.Close();            
        }

        private void BtnAmount500K_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.confirmVar = true;
            this.operationDoneVar = true;
            this.payAmountVar = decimal.Parse(BtnAmount500K.Text);
            this.Close();
        }
    }
}
