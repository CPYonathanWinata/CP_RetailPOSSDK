//Added by Adhi 
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
    //Added by Adhi     
    [Export("formEmployeePurchase", typeof(IInteractionView))]//Copas dari frmPublic(21) untuk interaction
    [PartCreationPolicy(CreationPolicy.NonShared)]//Copas dari frmPublic(22)
    public partial class frmEmployeePurchase : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;

        public frmEmployeePurchase()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmEmployeePurchase(GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
        }

        //Added by Adhi 
        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm //Memanggil frmEmployeePurchaseConfirm
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

        //Added by Adhi 
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Customer.Customer cust = new Customer.Customer();

            GME_Custom.GME_Propesties.GME_Var.searchTerm = this.nikTxt.Text;
            cust.Search(posTransaction);

            GME_Custom.GME_Propesties.GME_Var.searchTerm = string.Empty;
            this.Close();
        }

        private void nikTxt_Leave(object sender, EventArgs e)
        {
            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

            if (data.getCustomerId(nikTxt.Text, GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString) == "")
            {
                BlankOperations.BlankOperations.ShowMsgBox(GME_Custom.GME_Propesties.GME_Var.msgBoxEmployePurchase);
            }
            else
            {
                NamaTxt.Text = data.getCustomerName(nikTxt.Text, GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
            }
        }
    }
}
