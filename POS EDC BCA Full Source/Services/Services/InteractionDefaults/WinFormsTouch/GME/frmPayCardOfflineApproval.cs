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
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormPayCardOfflineApproval", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmPayCardOfflineApproval : frmTouchBase, IInteractionView
    {
        string approvalCodeVar = string.Empty;
        bool confirmOk = false;
        IPosTransaction posTransaction;       

        public frmPayCardOfflineApproval()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmPayCardOfflineApproval(frmPayCardOfflineApprovalCode parm)
            : this()
        {
            posTransaction = parm.posTransaction;

            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

            Numpad.EnteredValue = GME_Method.getAmountDecimal(retailTransaction.TransSalePmtDiff.ToString());

            Numpad.NumberOfDecimals = 0;
            Numpad.EntryStartsInDecimals = true;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmPayCardOfflineApprovalCode
            {
                Confirmed = confirmOk,
                approvalCode = approvalCodeVar
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
            if (txtApprovalCode.Text.Length != 6)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Approval code harus 6 digit");
            }else if (Numpad.EnteredValue == null)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Amount harus diisi");
            }
            else
            {
                RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                if (Convert.ToDecimal(Numpad.EnteredValue) > retailTransaction.TransSalePmtDiff)
                {
                    BlankOperations.BlankOperations.ShowMsgBox("Amount yang diinput tidak boleh lebih dari " + retailTransaction.TransSalePmtDiff.ToString());
                }
                else
                {                   
                    approvalCodeVar = txtApprovalCode.Text;
                    GME_Var.paycardofflineamount = Numpad.EnteredDecimalValue;
                    GME_Var.payCardApprovalCodeOffline = approvalCodeVar;
                    confirmOk = true;

                    this.Close();
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            confirmOk = false;
            this.Close();
        }        
    }
}
