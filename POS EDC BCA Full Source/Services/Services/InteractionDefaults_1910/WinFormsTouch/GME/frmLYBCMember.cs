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

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormLYBCMember", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmLYBCMember : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;

        public frmLYBCMember()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmLYBCMember(GME_Custom.GME_Propesties.frmLYBCMemberConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmLYBCMemberConfirm
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK
            } as TResults;
            //return null;
        }
        /*Interactionview Implementation for Argument send (current Null)*/
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            //throw new System.NotImplementedException();
        }
        #endregion

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormCustomerType(application, posTransaction);
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Customer.Customer cust = new Customer.Customer();            
            string selectedCustomerId = string.Empty;

            if (this.custNumTxt.TextLength > 14)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jumlah nomor kartu member harus 14 digit.");
            }
            else if (this.custNumTxt.Text.StartsWith("1"))
            {
                MessageBox.Show("Kartu anda sudah lama apakah anda ingin mengganti dengan kartu baru?");
            }
            else
            {
                GME_Custom.GME_Propesties.GME_Var.searchTerm = this.custNumTxt.Text;
                cust.Search(posTransaction);                               
            }

            GME_Custom.GME_Propesties.GME_Var.searchTerm = string.Empty;
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.custNumTxt.Text = string.Empty;
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            
        }
    }
}
