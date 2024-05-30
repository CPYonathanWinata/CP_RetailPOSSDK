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
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;


namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormPublic", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmPublic : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;        

        public frmPublic()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmPublic(GME_Custom.GME_Propesties.frmPublicConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmPublicConfirm
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK
                //Confirmed = confirm,
                //custEmail = this.emailTxt.Text,
                //custName = this.nameTxt.Text,
                //custPhoneNum = this.phoneNumTxt.Text,
                //custGroup = "Trade"
            } as TResults;
            //return null;
        }
        /*Interactionview Implementation for Argument send (current Null)*/
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            //throw new System.NotImplementedException();
        }
        #endregion

        private void skipBtn_Click(object sender, EventArgs e)
        {
            GME_Custom.GME_Propesties.GME_Var.isSkipCustomerType = true;            
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            //GME_Custom.GME_Propesties.GME_Var.isCustomerType = true;
            if (this.emailTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Email harus diisi.");
            }
            else if (this.phoneNumTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor telepon harus diisi");
            }
            else
            {
                frmCustomerSearch custSearch = new frmCustomerSearch();
                custSearch.addNewCust(this.nameTxt.Text, string.Empty, this.emailTxt.Text, this.phoneNumTxt.Text, "Trade", posTransaction, string.Empty);

                this.Close();
            }
        }
    }
}
