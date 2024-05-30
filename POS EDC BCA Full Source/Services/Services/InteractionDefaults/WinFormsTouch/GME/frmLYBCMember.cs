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
using GME_Custom.GME_Propesties;
using Jacksonsoft;
using System.Threading.Tasks;


namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormLYBCMember", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmLYBCMember : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;


        GME_Custom.GME_Data.IntegrationService integration = new GME_Custom.GME_Data.IntegrationService();
        GME_Custom.GME_Data.queryData queryData = new GME_Custom.GME_Data.queryData();

        public frmLYBCMember()
        {
            InitializeComponent();

        }


        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmLYBCMember(frmLYBCMemberConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmLYBCMemberConfirm
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
            this.Close();
            GME_FormCaller.GME_FormCustomerType(Connection.applicationLoc, posTransaction);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Customer.Customer cust = new Customer.Customer();
            string selectedCustomerId = string.Empty;

            //check email?
            Regex regEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?
                              \^_`{|}~]+)*"
                               + "@"
                               + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");

            //check only number?
            Regex regNumber = new Regex(@"^\d$");

            if (regNumber.IsMatch(CardNumberTxt.Text))
            {
                if (this.CardNumberTxt.Text.Substring(0,3) != "628")
                {
                    if (this.CardNumberTxt.TextLength != 14)
                    {
                        BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu member harus 14 digit.");
                    }
                }
            }
            else
            {                
                bool isCardReplacement = false;

                if (this.CardNumberTxt.Text.StartsWith("2") || this.CardNumberTxt.Text.StartsWith("3") || this.CardNumberTxt.Text.StartsWith("4") || this.CardNumberTxt.Text.StartsWith("9"))
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Nomor kartu member sudah tidak berlaku, harap lakukan card replacement");

                    GME_Var.searchTerm = string.Empty;
                    GME_Var.isCustomerTypeSearch = true;

                    GME_FormCaller.GME_FormCardReplacement(this.application, this.posTransaction, this.CardNumberTxt.Text);

                    this.CardNumberTxt.Text = GME_Var.cardReplacementNumber;

                    isCardReplacement = true;
                }

                if ((isCardReplacement && this.CardNumberTxt.Text != string.Empty) || !isCardReplacement)
                {
                    GME_Var.isLYBCCustomer = true;
                    
                    Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");
                }
            }
        }


        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            Customer.Customer cust = new Customer.Customer();

            bool isCardNumAx = false;
            bool isPhoneNumAx = false;
            bool isEmailAx = false;            


            isCardNumAx = queryData.isCardNumberInAX(CardNumberTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

            if (!isCardNumAx)             
            {
                isPhoneNumAx = queryData.checkExistPublicByPhone(CardNumberTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                if (!isPhoneNumAx)
                {
                    isEmailAx = queryData.checkExistPublicByEmail(CardNumberTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                }
            }                       

            //AX Found
            if (isCardNumAx || isPhoneNumAx || isEmailAx)
            {
                queryData.getCardNumberFromCustId(GME_Var.customerId, GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                integration.requestReward0302(GME_Var.identifierCode, application.Shift.StoreId);
                ////lookup

                //bool islookupValid = false;

                //if (isCardNumAx)
                //{
                //    if (integration.lookupCard(this.CardNumberTxt.Text) == "VALID")
                //    {
                //        islookupValid = true;
                //    }
                //}
                //else if (isPhoneNumAx)
                //{
                //    if (integration.lookupCardByPhone(this.CardNumberTxt.Text) == "VALID")
                //    {
                //        islookupValid = true;
                //    }
                //}
                //else
                //{
                //    if (integration.lookupCardByEmail(this.CardNumberTxt.Text) == "VALID")
                //    {
                //        islookupValid = true;
                //    }
                //}

                //if (islookupValid)
                //{
                //Transaksi
                GME_Var.isCustomerTypeSearch = true;                
                GME_Var.searchTerm = CardNumberTxt.Text;                              

                cust.Search(posTransaction);                    

                this.Close();
                //}
            }
            else
            {
                //AX Not Found 
                //check Engage lookup by Card/Email/Phone

                bool islookupValid = false;
                bool lookupCardCardNumber = false;
                bool lookupCardEmail = false;
                bool lookupCardPhone = false;

                if (CardNumberTxt.Text.Length == 14 && !CardNumberTxt.Text.Contains("@"))
                {                    
                    if (integration.lookupCard(this.CardNumberTxt.Text) == "VALID")
                    {
                        islookupValid = true;

                        integration.requestReward0302(GME_Var.lookupCardNumber, application.Shift.StoreId);
                    }                    

                    lookupCardCardNumber = true;
                }

                if (CardNumberTxt.Text.Substring(0,3) == "628")
                {
                    if (integration.lookupCardByPhone(this.CardNumberTxt.Text) == "VALID")
                    {
                        islookupValid = true;

                        integration.requestReward0302(GME_Var.lookupCardNumber, application.Shift.StoreId);
                    }

                    lookupCardPhone = true;
                }

                if (CardNumberTxt.Text.Contains("@"))
                {
                    if (integration.lookupCardByEmail(this.CardNumberTxt.Text) == "VALID")
                    {
                        islookupValid = true;

                        integration.requestReward0302(GME_Var.lookupCardNumber, application.Shift.StoreId);
                    }

                    lookupCardEmail = true;
                }

                if (islookupValid)
                {

                    GME_Var.lookupCardLYBC = true;

                    if (queryData.isCardNumberInAX(GME_Var.lookupCardNumber, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                    {
                        GME_Var.isCustomerTypeSearch = true;
                        GME_Var.searchTerm = GME_Var.lookupCardNumber;
                        cust.Search(posTransaction);

                        this.Close();
                    }
                    else
                    {
                        frmCustomerSearch custSearch = new frmCustomerSearch();
                        custSearch.addNewCust(GME_Var.lookupCardFirstName, GME_Var.lookupCardLastName, GME_Var.lookupCardEmail, GME_Var.lookupCardPhoneNumber, "Trade", posTransaction);

                        GME_Var.searchTerm = string.Empty;
                        this.Close();
                    }
                }
                else
                {
                    //tidak ditemukan di AX / Engage
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Data tidak ditemukan");

                    //SNF
                    GME_Var.isSNFTransaction = true;
                    
                    if (lookupCardCardNumber)
                    {
                        GME_Var.SetSNF("IDENTIFIERID", CardNumberTxt.Text);
                        GME_Var.SetSNF("PROCESSSTOPPED", "10");
                        GME_Var.SetSNF("STATUS", "3");
                        this.Close();
                    }
                    else if (lookupCardEmail)
                    {
                        GME_Var.SetSNF("EMAIL", CardNumberTxt.Text);
                        GME_Var.SetSNF("PROCESSSTOPPED", "11");
                        GME_Var.SetSNF("STATUS", "3");
                        this.Close();
                    }
                    else if (lookupCardPhone)
                    {
                        GME_Var.SetSNF("PHONENUMBER", CardNumberTxt.Text);
                        GME_Var.SetSNF("PROCESSSTOPPED", "12");
                        GME_Var.SetSNF("STATUS", "3");
                        this.Close();
                    }                                            

                    //this.Close();
                    //GME_Var.SetSNF("ENROLLTYPE", "LYBC");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.CardNumberTxt.Text = string.Empty;
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
