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
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    //Added by Adhi
    [Export("formFamilyPurchase", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmFamilyPurchase : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;
        public string cardMember = string.Empty;

        public frmFamilyPurchase()
        {
            InitializeComponent();
        }

        //Added by Adhi (fungsi form)
        //copas dari frmpublic (37)
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmFamilyPurchase(frmFamilyPurchaseConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
        }

        //Adedd by Adhi
        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmFamilyPurchaseConfirm
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
            Customer.Customer cust = new Customer.Customer();

            GME_Var.searchTerm = this.custNumberTxt.Text;
            GME_Var.isCustomerTypeSearch = true;
            cust.Search(posTransaction);

            GME_Var.searchTerm = string.Empty;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormCustomerType(application, posTransaction);
            this.Close();
        }

        private void NoFamilyMember_Leave(object sender, EventArgs e)
        {
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");            
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            queryData data = new queryData();
            IntegrationService integration = new IntegrationService();

            if (data.getFamilyMember(NoFamilyMember.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString) == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox(GME_Var.msgBoxFamilyPurchase);
                custNumberTxt.Text = string.Empty;
                NameTxt.Text = string.Empty;
            }
            else
            {
                custNumberTxt.Text = data.getFamilyCustomerId(NoFamilyMember.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                NameTxt.Text = data.getFamilyMemberName(NoFamilyMember.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                cardMember = data.checkFamilyIsMember(custNumberTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                if (cardMember != string.Empty)
                {
                    if (integration.lookupCard(cardMember) == "VALID")
                    {
                        integration.requestReward0302(cardMember, application.Shift.StoreId);
                        integration.getPerson(GME_Var.lookupCardPersonId);
                    }
                }
                else
                {
                    GME_Var.isFamilyNotMember = true;
                }
            }
        }
    }
}
