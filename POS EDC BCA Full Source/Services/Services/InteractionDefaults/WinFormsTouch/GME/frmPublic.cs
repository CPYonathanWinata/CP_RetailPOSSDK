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
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Customer;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormPublic", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmPublic : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;

        private ICustomerSystem CustomerSystem
        {
            //get { return this.Application.BusinessLogic.CustomerSystem; }
            get { return Connection.applicationLoc.BusinessLogic.CustomerSystem; }
        }

        public frmPublic()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmPublic(frmPublicConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
            this.phoneNumTxt.Text = "628";
            this.smsCb.Checked = true;
            this.emailCb.Checked = true;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmPublicConfirm
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

        private void skipBtn_Click(object sender, EventArgs e)
        {
            GME_Var.isSkipCustomerType = true;
            GME_Var.customerPosTransaction = posTransaction;///// add by maria
            GME_Var.identifierCode = "9999999999999999999";           
            this.Close();
        }        

        private void saveBtn_Click(object sender, EventArgs e)
        {            
            if (this.emailCb.Checked && this.emailTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Email harus diisi");
            }
            else if (this.emailCb.Checked && !this.emailTxt.Text.Contains("@"))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format email salah");
            }
            else if (smsCb.Checked && (phoneNumTxt.Text == "628" || string.IsNullOrEmpty(this.phoneNumTxt.Text) || this.phoneNumTxt.Text.Length <= 3))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor handphone harus diisi");                
                phoneNumTxt.Focus();
            }
            else if (this.phoneNumTxt.Text.Length <= 10 || this.phoneNumTxt.Text.Substring(0, 4) == "6280" || this.phoneNumTxt.Text.Substring(0, 3) == "081")
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format nomor handphone salah");                
            }
            else if (nameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("First name harus diisi");
            }
            else if (lastNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Last name harus diisi");
            }
            else
            {
                Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");                
            }
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            DE.ICustomer customer = null;
            queryData data = new queryData();
            IntegrationService integration = new IntegrationService();
            queryDataOffline dataOffline = new queryDataOffline();
            string channel = string.Empty;
            bool lookupValidEN = false;
            bool lookupValidAX = false;


            if (smsCb.Checked)
            {
                if (data.checkExistPublicByPhone(phoneNumTxt.Text, application.Settings.Database.Connection.ConnectionString) == true)
                {
                    lookupValidAX = true;
                }
            }
            else if (emailCb.Checked)
            {
                if (data.checkExistPublicByEmail(emailTxt.Text, application.Settings.Database.Connection.ConnectionString) == true)
                {
                    lookupValidAX = true;
                }
            }

            if (lookupValidAX)
            {
                customer = this.CustomerSystem.GetCustomerInfo(GME_Var.customerId);
                this.application.Services.Customer.AddCustomerToTransaction(customer, posTransaction);

                GME_Var.customerData = customer;
            }
            else
            {
                channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                if (channel == string.Empty)
                {
                    channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                }

                if (integration.lookupCardByPhone(phoneNumTxt.Text) == "VALID")
                {
                    lookupValidEN = true;
                }
                else if (integration.lookupCardByEmail(emailTxt.Text) == "VALID")
                {
                    lookupValidEN = true;
                }

                if (lookupValidEN == true)
                {
                    string customerId = string.Empty;
                    Customer.Customer cust = new Customer.Customer();

                    integration.requestReward0302(GME_Var.lookupCardNumber, application.Shift.StoreId);

                    customerId = data.getCustomerIdByCardNumber(GME_Var.lookupCardNumber, application.Settings.Database.Connection.ConnectionString);

                    customer = this.CustomerSystem.GetCustomerInfo(customerId);

                    GME_Var.customerData = customer;
                    GME_Var.isEmployeePurch = true;

                    GME_Var.searchTerm = customerId;
                    cust.Search(posTransaction);
                }
                else
                {
                    if (GME_Var.lookupCardStatus == "INVALID")
                    {
                        GME_Var.isPublicCustomer = true;
                        //temp cod, after UAT must be remove
                        if (!data.checkExistPublicByCardNum(GME_Var.publicIdentifier, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                        {//temp code                                                                   
                            integration.requestReward0302(GME_Var.publicIdentifier, application.Shift.StoreId);

                            frmCustomerSearch custSearch = new frmCustomerSearch();
                            custSearch.addNewCust(nameTxt.Text, lastNameTxt.Text, emailTxt.Text, phoneNumTxt.Text, "Trade", posTransaction);
                        }
                    }
                }
            }

            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            GME_FormCaller.GME_FormCustomerType(Connection.applicationLoc, posTransaction);            
        }

        private void phoneNumTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }    
}
