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
    [Export("FormCustInfo", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public partial class frmCustomerInfo : frmTouchBase, IInteractionView
    {
        #region Variable
        public DataTable custInfoTable;
        public IApplication apps;
        #endregion

        public frmCustomerInfo()
        {
            InitializeComponent();
        }
       

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmCustInfoConfirm
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

        private void frmCustomerInfo_Load(object sender, EventArgs e)
        {
            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

            //MessageBox.Show(GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

            custInfoTable = data.getCustomerInfo(GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

            CustInfoDG.DataSource = custInfoTable;
            CustInfoDG.AllowUserToAddRows = false;
            //CustInfoDG.AutoGenerateColumns = true;
            //CustInfoDG.Columns[GME_Custom.GME_Propesties.frmCustInfoConfirm.accountNum].DisplayIndex = 0;
            //CustInfoDG.Columns[GME_Custom.GME_Propesties.frmCustInfoConfirm.custName].DisplayIndex = 1;
            //CustInfoDG.Columns[GME_Custom.GME_Propesties.frmCustInfoConfirm.custGroup].DisplayIndex = 2;
            //CustInfoDG.Columns[GME_Custom.GME_Propesties.frmCustInfoConfirm.currency].DisplayIndex = 4;

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
