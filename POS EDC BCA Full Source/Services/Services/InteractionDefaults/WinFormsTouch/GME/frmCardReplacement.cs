 /// ADD BY MARIA   -- FORM Card Replacement
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
using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
using LSRetailPosis.Transaction;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;
using Microsoft.Dynamics.Retail.Pos.Loyalty;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmCardReplacement", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FrmCardReplacement : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;
        public DataTable oldCardTypeDT = new DataTable();
        public DataTable newCardTypeDT = new DataTable();
        public string oldCardNum = string.Empty;
        public string channel = string.Empty;
        public string transIdtest = string.Empty;        

        public FrmCardReplacement()
        {
            InitializeComponent();
        }


        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public FrmCardReplacement(FrmCardReplacementConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
            this.oldCardNum = parm.oldCardNumber;

            if (this.oldCardNum != string.Empty)
                oldCardNumTxt.Text = this.oldCardNum;
            else
                oldCardNumTxt.Text = GME_Var.identifierCode;

            newCardTypeCombo.Text = "As Card Tender";
            oldCardTypeCombo.Text = "Blocked";

        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.frmEnrollmentConfirm
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

        private ICustomerSystem CustomerSystem
        {
            //get { return this.Application.BusinessLogic.CustomerSystem; }
            get { return GME_Custom.GME_Propesties.Connection.applicationLoc.BusinessLogic.CustomerSystem; }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (newCardNumTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox(GME_Var.msgCardReplacementNewCardValidation);
            }
            else
            {                                
                Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");                
            }
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            queryData data = new queryData();
            queryDataOffline dataOffline = new queryDataOffline();
            IntegrationService integration = new IntegrationService();
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            DE.ICustomer customer;
            Customer.Customer customerLoc = new Customer.Customer();
            string customerId = string.Empty;

            customerId = data.getCustomerByCardNumber(this.oldCardNumTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
            customer = this.CustomerSystem.GetCustomerInfo(customerId);
            if (retailTransaction != null)
            {
                this.CustomerSystem.SetCustomer(retailTransaction, customer, customer);

                integration.requestReward0302(this.oldCardNumTxt.Text, application.Shift.StoreId);
                integration.requestReward0302(this.newCardNumTxt.Text, application.Shift.StoreId);
                System.Threading.Thread.Sleep(6000);

                if (oldCardNumTxt.Text.Length != 14)
                {
                    BlankOperations.BlankOperations.ShowMsgBox("Old card number harus 14 digit");
                }
                else if (newCardNumTxt.Text.Length != 14)
                {
                    BlankOperations.BlankOperations.ShowMsgBox("New card number harus 14 digit");
                }
                else if (integration.lookupCard(this.newCardNumTxt.Text) == "VALID")
                {
                    BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu member sudah digunakan, silahkan masukkan nomor yang lain");
                }
                else
                {
                    if (integration.lookupCard(this.oldCardNumTxt.Text) == "VALID")
                    {
                        if (integration.updateIdentifierNewCard(this.newCardNumTxt.Text, this.oldCardNumTxt.Text) == "Success")
                        {
                            if (integration.updateIdentifierOldCard(this.oldCardNumTxt.Text) == "Success")
                            {
                                integration.requestReward0302(this.oldCardNumTxt.Text, application.Shift.StoreId);
                                integration.requestReward0302(this.newCardNumTxt.Text, application.Shift.StoreId);
                                System.Threading.Thread.Sleep(6000);

                                if (integration.getPerson(GME_Var.personId) == "Success")
                                {
                                    if (integration.getIdentifier(this.newCardNumTxt.Text) == "Success")
                                    {
                                        if (!data.isCardNumberInAX(oldCardNumTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                                        {
                                            GME_Var.cardReplacementNumber = this.newCardNumTxt.Text;
                                            GME_Var.isCardReplacement = true;

                                            frmCustomerSearch custSearch = new frmCustomerSearch();
                                            custSearch.addNewCust(GME_Var.lookupCardFirstName, GME_Var.lookupCardLastName, GME_Var.lookupCardEmail, GME_Var.lookupCardPhoneNumber, "Trade", posTransaction);

                                            this.Close();
                                        }
                                        else
                                        {
                                            GME_Loyalty loyalty = new GME_Loyalty();
                                            Customer.Customer cust = new Customer.Customer();

                                            if (loyalty.createLoyalty(this.newCardNumTxt.Text, retailTransaction))
                                            {
                                                //loyalty success
                                                GME_Var.cardReplacementNumber = this.newCardNumTxt.Text;

                                                channel = data.getChannelStore(application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                                                if (data.insertCardReplacement(newCardNumTxt.Text, oldCardNumTxt.Text, oldCardTypeCombo.Text, newCardTypeCombo.Text, application.Shift.StoreId,
                                                            channel, retailTransaction.TransactionId, application.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                                                {
                                                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Nomor kartu member '" + this.newCardNumTxt.Text + "' berhasil didaftarkan di POS.");
                                                    GME_Var.isCardReplacementSuccess = true;
                                                    GME_Var.identifierCode = this.newCardNumTxt.Text;

                                                    GME_Var.isCustomerTypeSearch = true;
                                                    GME_Var.searchTerm = this.newCardNumTxt.Text;

                                                    cust.Search(posTransaction);
                                                }                                               

                                                GME_Var.isCardReplacement = true;
                                                this.Close();
                                            }
                                            else
                                            {
                                                GME_Var.cardReplacementNumber = this.oldCardNumTxt.Text;

                                                channel = data.getChannelStore(application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                                                data.insertCardReplacement(newCardNumTxt.Text, oldCardNumTxt.Text, oldCardTypeCombo.Text, newCardTypeCombo.Text, application.Shift.StoreId,
                                                            channel, retailTransaction.TransactionId, application.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);                                             

                                                GME_Var.isCardReplacementSuccess = false;
                                                ///Offline mode
                                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Nomor kartu member '" + this.newCardNumTxt.Text + "' gagal didaftarkan di POS.");

                                                this.Close();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        BlankOperations.BlankOperations.ShowMsgBox("Gagal melakukan card replacement");
                                    }
                                }
                            }
                        }
                        else
                        {                            
                            BlankOperations.BlankOperations.ShowMsgBox("Gagal melakukan card replacement");
                        }
                    }
                    else
                    {
                        if (GME_Var.lookupCardStatusCard == "REPLACED")
                        {
                            BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu member" + this.oldCardNumTxt.Text + " sudah di replace");
                        }
                        else
                        {
                            BlankOperations.BlankOperations.ShowMsgBox("Mohon maaf untuk saat ini tidak bisa melakukan card replacement");
                        }
                    }
                }
            }
            else
            {
                BlankOperations.BlankOperations.ShowMsgBox("Silahkan masukkan customer member terlebih dahulu");
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void oldCardNumTxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void newCardNumTxt_KeyPress(object sender, KeyPressEventArgs e)
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
