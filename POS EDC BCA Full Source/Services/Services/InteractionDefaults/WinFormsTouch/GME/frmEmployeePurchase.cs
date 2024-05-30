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
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Jacksonsoft;
using System.Threading.Tasks;


namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    //Added by Adhi     
    [Export("formEmployeePurchase", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmEmployeePurchase : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;
        public string cardMember = string.Empty;

        private ICustomerSystem CustomerSystem
        {
            //get { return this.Application.BusinessLogic.CustomerSystem; }
            get { return Connection.applicationLoc.BusinessLogic.CustomerSystem; }
        }

        public frmEmployeePurchase()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmEmployeePurchase(frmEmployeePurchaseConfirm parm)
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
            return new frmEmployeePurchaseConfirm //Memanggil frmEmployeePurchaseConfirm
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

        private void button1_Click(object sender, EventArgs e)
        {
            DE.ICustomer customer = null;
            Customer.Customer cust = new Customer.Customer();            

            customer = this.CustomerSystem.GetCustomerInfo(nikTxt.Text);
            //this.application.Services.Customer.AddCustomerToTransaction(customer, posTransaction);

            GME_Var.customerData = customer;
            GME_Var.isEmployeePurch = true;
            GME_Var.isCustomerTypeSearch = true;

            GME_Var.searchTerm = this.nikTxt.Text;
            cust.Search(posTransaction);

            //GME_Var.searchTerm = string.Empty;
            GME_Var.isEmployeePurch = false;
            this.Close();
        }

        private void nikTxt_Leave(object sender, EventArgs e)
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

            if (data.getCustomerId(nikTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString) == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox(GME_Var.msgBoxEmployePurchase);
                NamaTxt.Text = string.Empty;
            }
            else
            {
                NamaTxt.Text = data.getCustomerName(nikTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                cardMember = data.checkEmployeeIsMember(nikTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                if (cardMember != string.Empty)
                {
                    if (integration.lookupCard(cardMember) == "VALID")
                    {
                        integration.requestReward0302(cardMember, application.Shift.StoreId);
                        integration.getPerson(GME_Var.lookupCardPersonId);

                        GME_Var.identifierCode = GME_Var.lookupCardNumber;
                    }
                }
                else
                {
                    GME_Var.isEmployeeNotMember = true;
                    GME_Var.identifierCode = "9999999999999999999";
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            GME_FormCaller.GME_FormCustomerType(application, posTransaction);
            this.Close();
        }
    }
}
