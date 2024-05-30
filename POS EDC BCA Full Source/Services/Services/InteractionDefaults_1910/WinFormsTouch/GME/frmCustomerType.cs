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
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormCustType", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmCustomerType : frmTouchBase, IInteractionView
    {
        public IPosTransaction posTransaction;
        public IApplication application;

        public frmCustomerType()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmCustomerType(GME_Custom.GME_Propesties.frmCustTypeConfirm parm)
            : this()
        {
            this.posTransaction = parm.posTransaction;
            this.application = parm.application;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmCustTypeConfirm
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

        private void publicBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormPublic(this.application, posTransaction);
            this.Close();
        }

        private void lybcMemberBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormLYBCMember(this.application, posTransaction);
            this.Close();
        }       

        private void newEnrollBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormEnrollment(this.application, posTransaction);
            this.Close();
        }

        private void logOffBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EmpPurchBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormEmployeePurchase(application, posTransaction);
            this.Close();
        }

        private void FamPurchBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormFamilyPurchase(application, posTransaction);
            this.Close();
        }
    }
}
