// CREATED BY MARIA - WCS    
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

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmFanBOBB", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmFanBOBB : frmTouchBase, IInteractionView
    {
        IApplication application = null;
        string getInput = string.Empty;
        string qty = string.Empty;
        string itemId = string.Empty;


        public frmFanBOBB()
        {
            InitializeComponent();
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmFanBOBBconfirm
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

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmFanBOBB(GME_Custom.GME_Propesties.frmFanBOBBconfirm parm)
            : this()
        {

            application = parm.application;
            itemId = parm.item;
            qty = parm.quantity;


        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            
        }
    }
}
