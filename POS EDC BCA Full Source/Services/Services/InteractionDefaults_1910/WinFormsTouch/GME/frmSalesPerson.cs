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
    [Export("formSalesPerson", typeof(IInteractionView))]//Copas dari frmPublic(21) untuk interaction
    [PartCreationPolicy(CreationPolicy.NonShared)]//Copas dari frmPublic(22)
    public partial class frmSalesPerson : frmTouchBase, IInteractionView //Copas dari frmPublic(23) edited (frmBelajar)
    {
        string salesPersonId = string.Empty;
        string salesPersonName = string.Empty;

        public IApplication application;
        //public IPosTransaction posTransaction;

        public frmSalesPerson()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmSalesPerson(GME_Custom.GME_Propesties.frmSalesPersonConfirm parm)
            : this()
        {        
            this.application = parm.application;
            //this.posTransaction = parm.posTransaction;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmSalesPersonConfirm
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

        private void OKBtn_Click(object sender, EventArgs e)
        {
            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

            if (data.getSalesPerson(application.Shift.StoreId, application.Shift.TerminalId, application.Settings.Database.Connection.ConnectionString) == string.Empty)
            {
                data.insertSalesPerson(SalesPersonIdTxt.Text, application.Shift.StoreId, application.Shift.TerminalId, application.Settings.Database.Connection.ConnectionString);
            }
            else
            {
                data.updateSalesPerson(SalesPersonIdTxt.Text, application.Shift.StoreId, application.Shift.TerminalId, application.Settings.Database.Connection.ConnectionString);
            }

            GME_Custom.GME_Propesties.GME_Var.isChangeSalesPerson = true;

            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}        