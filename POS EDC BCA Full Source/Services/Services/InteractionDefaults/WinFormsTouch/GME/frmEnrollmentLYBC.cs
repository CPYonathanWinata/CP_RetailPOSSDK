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
using System.Globalization;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormEnrollmentLYBC", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmEnrollmentLYBC : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;
        public DataTable skinTypeDT = new DataTable();
        queryData data = new queryData();
        public string gender = string.Empty;
        public string channel = string.Empty;
        public string birthDateStr = string.Empty;
        public DateTime birthDate = DateTime.MinValue;
        Customer.Customer cust = new Customer.Customer();

        public frmEnrollmentLYBC()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmEnrollmentLYBC(frmEnrollmentLYBCConfirm parm)
            : this()
        {
            if (GME_Var.lookupCardLYBC)
            {
                this.application = parm.application;
                this.posTransaction = parm.posTransaction;

                this.cardMemberTxt.Text = GME_Var.lookupCardNumber;
                this.firstNameTxt.Text = GME_Var.personFirstName;
                this.lastNameTxt.Text = GME_Var.personLastName;

                if (GME_Var.personGender == "M")
                    this.rdBtnMale.Checked = true;
                else if (GME_Var.personGender == "F")
                    this.rdBtnFemale.Checked = true;

                this.emailTxt.Text = GME_Var.personEmail;
                this.phoneNumTxt.Text = GME_Var.personPhone;
                this.dayDateTxt.Text = GME_Var.personBirthdate.Substring(2, 2);
                this.monthDateTxt.Text = GME_Var.personBirthdate.Substring(0, 1);
                this.yearDateTxt.Text = GME_Var.personBirthdate.Substring(5, 4);
                this.skinTypeCmb.Text = GME_Var.personSkinType;
            }
            else
            {
                this.rdBtnFemale.Enabled = false;
                this.rdBtnMale.Enabled = false;
                this.emailTxt.Enabled = false;
                this.phoneNumTxt.Enabled = false;
                this.dayDateTxt.Enabled = false;
                this.monthDateTxt.Enabled = false;
                this.yearDateTxt.Enabled = false;
                this.skinTypeCmb.Enabled = false;
            }
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmEnrollmentLYBCConfirm
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

        private void frmEnrollment_Load(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click_1(object sender, EventArgs e)
        {
            if (this.cardMemberTxt.TextLength < 14)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jumlah nomor kartu member harus 14 digit.");
            }
            else if (dayDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tanggal harus diisi!");
                dayDateTxt.Focus();
            }
            else if (monthDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Bulan harus diisi!");
                monthDateTxt.Focus();
            }
            else if (yearDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tahun harus diisi!");
                yearDateTxt.Focus();
            }
            else
            {
                birthDateStr = dayDateTxt.Text + "/" + monthDateTxt.Text + "/" + yearDateTxt.Text;
                birthDate = DateTime.ParseExact(birthDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (cardMemberTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu LYBC harus diisi!");
                cardMemberTxt.Focus();
            }
            else if (firstNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama depan harus diisi!");
                firstNameTxt.Focus();
            }
            else if (lastNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama belakang harus diisi!");
                lastNameTxt.Focus();
            }
            else if (rdBtnMale.Checked == false && rdBtnFemale.Checked == false)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jenis kelamin harus dipilih!");
            }                
            else if (skinTypeCmb.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jenis kulit harus diisi!");
                skinTypeCmb.Focus();
            }
            else
            {
                IntegrationService integration = new IntegrationService();
                queryData queryData = new queryData();

                if (rdBtnMale.Checked)
                    gender = "M";
                else if (rdBtnFemale.Checked)
                    gender = "F";

                if (skinTypeCmb.Text == "Normal")
                    skinTypeCmb.Text = "1";
                else if (skinTypeCmb.Text == "Berminyak")
                    skinTypeCmb.Text = "2";
                else if (skinTypeCmb.Text == "Kering")
                    skinTypeCmb.Text = "3";
                else if (skinTypeCmb.Text == "Kombinasi")
                    skinTypeCmb.Text = "4";

                if (EnrollTypeCmb.Text == "CASH")
                    EnrollTypeCmb.Text = "0";
                else if (EnrollTypeCmb.Text == "MANDIRI")
                    EnrollTypeCmb.Text = "1";
                else if (EnrollTypeCmb.Text == "BCA")
                    EnrollTypeCmb.Text = "2";
                else if (EnrollTypeCmb.Text == "BNI")
                    EnrollTypeCmb.Text = "3";
                else if (EnrollTypeCmb.Text == "CITIBANK")
                    EnrollTypeCmb.Text = "4";
                else if (EnrollTypeCmb.Text == "HSBC")
                    EnrollTypeCmb.Text = "5";
                else if (EnrollTypeCmb.Text == "TELKOMSEL")
                    EnrollTypeCmb.Text = "6";
                else if (EnrollTypeCmb.Text == "OTHERBANK")
                    EnrollTypeCmb.Text = "7";

                //check AX CARD/PHONE/EMAIL
                bool isCardNumAx = false;
                bool isPhoneNumAx = false;
                bool isEmailAx = false;

                GME_Var.enrollLYBCCardMemberNum = cardMemberTxt.Text;


                if (!queryData.isCardNumberInAX(cardMemberTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                {
                    if (!queryData.checkExistPublicByPhone(phoneNumTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                    {
                        if (queryData.checkExistPublicByEmail(emailTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                        {
                            isEmailAx = true;
                        }
                    }
                    else
                    {
                        isPhoneNumAx = true;
                    }
                }
                else
                {
                    isCardNumAx = true;
                }

                integration.requestReward0302(cardMemberTxt.Text, application.Shift.StoreId);

                if (!GME_Var.req0302Response)
                {
                    GME_Var.SetSNF("PROCESSSTOPPED", "RequestReward0302");
                }


                //lookupCard
                bool islookupValid = false;


                if (integration.lookupCard(this.cardMemberTxt.Text) == "VALID")
                {
                    islookupValid = true;
                    application.Services.Dialog.ShowMessage("Nomor kartu member sudah digunakan, silahkan masukkan nomor yang lain", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (integration.lookupCardByPhone(this.phoneNumTxt.Text) == "VALID")
                {
                    islookupValid = true;
                }

                if (integration.lookupCardByEmail(this.emailTxt.Text) == "VALID")
                {
                    islookupValid = true;
                }



                frmCustomerSearch custSearch = new frmCustomerSearch();
                custSearch.addNewCust(this.firstNameTxt.Text, this.lastNameTxt.Text, this.emailTxt.Text, this.phoneNumTxt.Text, "Trade", posTransaction);

                if (GME_Var.isSuccessCreatedCust)
                {
                    channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                    if (!data.insertNewCustomerDataExtend(GME_Var.customerData.CustomerId, gender, skinTypeCmb.Text, birthDate, this.posTransaction.StoreId,
                    channel, this.posTransaction.TransactionId, this.posTransaction.TerminalId, this.application.Settings.Database.Connection.ConnectionString))
                    {
                        ///DB offline
                    }
                }
                else
                {
                    ///Offline mode
                }

                this.Close();
            }
        }

        private void CancelBtn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }       
    }
}
