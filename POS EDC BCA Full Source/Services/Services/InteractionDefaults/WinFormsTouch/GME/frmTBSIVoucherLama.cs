using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.Transaction;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using GME_Custom;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction.Line.SaleItem;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmTBSIVoucherLama", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmTBSIVoucherLama : frmTouchBase, IInteractionView
    {
        string tbsiVoucherId = string.Empty;

        public IApplication application;
        public IPosTransaction posTransaction;

        public frmTBSIVoucherLama()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmTBSIVoucherLama(frmTBSIVoucherLamaConfirm parm)
            : this()
        {
            tbsiVoucherId = parm.tbsiVoucherId;

            this.application = parm.application;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmTBSIVoucherLamaConfirm
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK
            } as TResults;
        }
        /*Interactionview Implementation for Argument send (current Null)*/
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            //throw new System.NotImplementedException();
        }
        #endregion

        private void BtnOk_Click(object sender, EventArgs e)
        {
            //application.RunOperation(PosisOperations.TotalDiscountAmount, Numpad.EnteredDecimalValue);
            int count = 0;
            var itemVouchTBSI = new string[0];
            var itemVouchTBSIOld = new string[0];
            posTransaction = GME_Var.welcomePosTransaction;
            LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
            foreach (SaleLineItem lineItem in tmp)
            {
                int counter = 0;
                for (int i = 0; i < GME_Var.displaynumberUndiscounted.Length; i++)
                {
                    if (lineItem.ItemId != GME_Var.displaynumberUndiscounted[i])
                    {
                        counter++;
                    }
                }

                if (counter == GME_Var.displaynumberUndiscounted.Length)
                {
                    count++;
                    itemVouchTBSI = new string[count];
                    itemVouchTBSI[count - 1] = lineItem.ItemId;

                    if (itemVouchTBSI[0] == null)
                    {
                        for (int i = 0; i < itemVouchTBSIOld.Length; i++)
                        {
                            itemVouchTBSI[i] = itemVouchTBSIOld[i];
                        }
                    }
                    itemVouchTBSIOld = itemVouchTBSI;
                }
            }
            GME_Var.itemVouchTBSI = itemVouchTBSI;
            GME_Var.amountVouchTBSI = Numpad.EnteredDecimalValue;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
