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
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormEnrollment", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmEnrollment : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;
        public DataTable skinTypeDT = new DataTable();
        public DataTable enrollTypeDT = new DataTable();
        queryData data = new queryData();
        public string gender = string.Empty;
        public string channel = string.Empty;
        public string birthDateStr = string.Empty;
        public DateTime birthDate = DateTime.MinValue;
        Customer.Customer cust = new Customer.Customer();

        public frmEnrollment()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmEnrollment(frmEnrollmentConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;

            this.skinTypeDT = data.getSkinType(this.application.Settings.Database.Connection.ConnectionString);
            this.skinTypeCmb.DataSource = this.skinTypeDT;
            this.skinTypeCmb.DisplayMember = "DESKRIPSI";                        
            this.phoneNumTxt.Text = "628";
            this.smsCb.Checked = true;
            this.emailCb.Checked = true;
            this.enrollTypeDT = data.getEnrollmentType(this.application.Settings.Database.Connection.ConnectionString);
            this.EnrollTypeCmb.DataSource = this.enrollTypeDT;
            this.EnrollTypeCmb.DisplayMember = "TYPE";            
            this.EnrollTypeCmb.Text = "CASH";
            this.skinTypeCmb.Text = "NORMAL";
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

        private void frmEnrollment_Load(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            bool readyToInsert = true;

            if (this.cardMemberTxt.TextLength != 14)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jumlah nomor kartu member harus 14 digit.");
                readyToInsert = false;
                cardMemberTxt.Focus();
            }            
            else if (dayDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tanggal harus diisi!");
                readyToInsert = false;
                dayDateTxt.Focus();
            }
            else if (monthDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Bulan harus diisi!");
                readyToInsert = false;
                monthDateTxt.Focus();
            }
            else if (yearDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tahun harus diisi!");
                readyToInsert = false;
                yearDateTxt.Focus();
            } 
            else if (cardMemberTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu LYBC harus diisi!");
                readyToInsert = false;
                cardMemberTxt.Focus();
            }
            else if (firstNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama depan harus diisi!");
                readyToInsert = false;
                firstNameTxt.Focus();
            }
            else if (lastNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama belakang harus diisi!");
                readyToInsert = false;
                lastNameTxt.Focus();
            }
            else if (rdBtnMale.Checked == false && rdBtnFemale.Checked == false)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jenis kelamin harus dipilih!");
                readyToInsert = false;
            }
            else if (emailCb.Checked && emailTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Email harus diisi!");
                readyToInsert = false;
                emailTxt.Focus();
            }
            else if (this.emailCb.Checked && !this.emailTxt.Text.Contains("@"))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format email salah.");
            }
            else if (smsCb.Checked && (phoneNumTxt.Text == "628" || string.IsNullOrEmpty(this.phoneNumTxt.Text) || this.phoneNumTxt.Text.Length <= 3))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor handphone harus diisi");
                readyToInsert = false;
                phoneNumTxt.Focus();
            }
            else if (this.phoneNumTxt.Text.Length <= 10 || this.phoneNumTxt.Text.Substring(0,4) == "6280"  || this.phoneNumTxt.Text.Substring(0, 3) == "081")
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format nomor handphone salah.");
                readyToInsert = false;
            }            
            else
            {
                birthDateStr = dayDateTxt.Text + "/" + monthDateTxt.Text + "/" + yearDateTxt.Text;
                try
                {
                    birthDate = DateTime.ParseExact(birthDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    int result = DateTime.Compare(birthDate, DateTime.Now);
                    if (result > 0)
                    {
                        BlankOperations.BlankOperations.ShowMsgBox("Isi tanggal lahir dengan format tanggal yang benar");
                        readyToInsert = false;
                    }
                }
                catch (Exception er)
                {
                    BlankOperations.BlankOperations.ShowMsgBox("Isi tanggal lahir dengan format tanggal yang benar");
                    readyToInsert = false;
                }                

                if (readyToInsert)
                {
                    Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");
                }
            }
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            IntegrationService integration = new IntegrationService();
            queryData queryData = new queryData();
            string enrolltype = string.Empty;
            string skinType = string.Empty;

            if (rdBtnMale.Checked)
                gender = "M";
            else if (rdBtnFemale.Checked)
                gender = "F";

            if (skinTypeCmb.Text == "NORMAL")
                skinType = "1";
            else if (skinTypeCmb.Text == "OILY")
                skinType = "2";
            else if (skinTypeCmb.Text == "DRY")
                skinType = "3";
            else if (skinTypeCmb.Text == "COMBINATION")
                skinType = "4";

            if (EnrollTypeCmb.Text == "CASH")
                enrolltype = "0";
            else if (EnrollTypeCmb.Text == "MANDIRI")
                enrolltype = "1";
            else if (EnrollTypeCmb.Text == "BCA")
                enrolltype = "2";
            else if (EnrollTypeCmb.Text == "BNI")
                enrolltype = "3";
            else if (EnrollTypeCmb.Text == "CITIBANK")
                enrolltype = "4";
            else if (EnrollTypeCmb.Text == "HSBC")
                enrolltype = "5";
            else if (EnrollTypeCmb.Text == "TELKOMSEL")
                enrolltype = "6";
            else if (EnrollTypeCmb.Text == "OTHERBANK")
                enrolltype = "7";

            GME_Var.identifierCode = cardMemberTxt.Text;
            GME_Var.enrollCardMemberNum = cardMemberTxt.Text;
            GME_Var.enrollGender = gender;
            GME_Var.enrollBirthDate = birthDate;
            GME_Var.enrollskinType = skinType;
            GME_Var.enrollType = int.Parse(enrolltype);
            GME_Var.enrollPersonFirstName = firstNameTxt.Text;
            GME_Var.enrollPersonLastName = lastNameTxt.Text;
            GME_Var.enrollPersonPhoneNumber = phoneNumTxt.Text;

            //check AX CARD/PHONE/EMAIL
            bool isCardNumAx = false;
            bool isPhoneNumAx = false;
            bool isEmailAx = false;

            if (queryData.isCardNumberInAX(cardMemberTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
            {
                isCardNumAx = true;
            }

            if (smsCb.Checked)
            {
                if (queryData.checkExistPublicByPhone(phoneNumTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                {
                    isPhoneNumAx = true;
                }
            }

            if (emailCb.Checked)
            {
                if (queryData.checkExistPublicByEmail(emailTxt.Text.Trim(), GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                {
                    isEmailAx = true;
                }
            }

            //lookupCard
            bool islookupValid = false;

            if (GME_Var._EngageHOStatus)
            {
                integration.requestReward0302(cardMemberTxt.Text, application.Shift.StoreId);

                if (!GME_Var.req0302Response)
                {
                    GME_Var.SetSNF("PROCESSSTOPPED", "RequestReward0302");
                }



                if (integration.lookupCard(this.cardMemberTxt.Text) == "VALID")
                {
                    islookupValid = true;
                    application.Services.Dialog.ShowMessage("Nomor kartu member sudah digunakan, silahkan masukkan nomor yang lain", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (emailCb.Checked && !islookupValid)
                {
                    if (integration.lookupCardByEmail(this.emailTxt.Text) == "VALID")
                    {
                        islookupValid = true;
                    }
                }

                if (smsCb.Checked && !islookupValid)
                {
                    if (integration.lookupCardByPhone(this.phoneNumTxt.Text) == "VALID")
                    {
                        islookupValid = true;
                    }
                }
            }
            else
            {
                islookupValid = false;
            }

            //selain cardnumber
            //AX NOT FOUND, ENGAGE FOUND
            if (islookupValid && !isCardNumAx)
            {
                //check if guest?
                if (GME_Var.lookupCardTier.ToUpper() == "GUEST")
                {
                    if (BlankOperations.BlankOperations.ShowMsgDialog("Sebelumnya customer sudah terdaftar menjadi guest, apakah ingin melakukan card replacement?") == "OK")
                    {
                        GME_FormCaller.GME_FormCardReplacement(this.application, this.posTransaction, GME_Var.lookupCardNumber);

                        if (GME_Var.isCardReplacement)
                        {
                            bool isUpdatePerson = false;
                            bool isUpdateHouseHold = false;
                            bool isUpdateContactabilitySMS = false;
                            bool isUpdateContactabilityEMAIL = false;

                            if (integration.updatePersonEnrollment(GME_Var.lookupCardPersonId, phoneNumTxt.Text, emailTxt.Text, firstNameTxt.Text, birthDate, gender, lastNameTxt.Text, skinType) == "Success")
                            {
                                isUpdatePerson = true;
                                if (integration.updateHousehold(GME_Var.enrollHouseholdId, application.Shift.StoreId, this.emailTxt.Text, this.phoneNumTxt.Text) == "Success")
                                {
                                    isUpdateHouseHold = true;

                                    if (smsCb.Checked && phoneNumTxt.Text != string.Empty)
                                        if (integration.updateContactabilitySMS(GME_Var.lookupCardPersonId) == "Success")
                                        {
                                            isUpdateContactabilitySMS = true;
                                        }

                                    if (emailCb.Checked && emailTxt.Text != string.Empty)
                                    {
                                        if (integration.updateContactabilityEmail(GME_Var.lookupCardPersonId) == "Success")
                                        {
                                            isUpdateContactabilityEMAIL = true;
                                        }
                                    }
                                }
                            }

                            GME_Var.searchTerm = GME_Var.cardReplacementNumber;
                            cust.Search(posTransaction);

                            channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            data.insertNewCustomerDataExtend(GME_Var.customerData.CustomerId, gender, skinType, birthDate, this.posTransaction.StoreId,
                                channel, this.posTransaction.TransactionId, this.posTransaction.TerminalId, this.application.Settings.Database.Connection.ConnectionString);

                            //check salah satu proses fail?
                            if (!isUpdatePerson || !isUpdateHouseHold || !isUpdateContactabilitySMS || !isUpdateContactabilityEMAIL)
                            {
                                GME_Var.isSNFTransaction = true;
                                GME_Var.SetSNF("HOUSEHOLDID", GME_Var.personHouseHoldId.ToString());
                                GME_Var.SetSNF("IDENTIFIERID", cardMemberTxt.Text);
                                GME_Var.SetSNF("CUSTOMERID", GME_Var.customerData.CustomerId);
                                GME_Var.SetSNF("GENDER", gender);
                                GME_Var.SetSNF("SKINTYPE", skinType);
                                GME_Var.SetSNF("BIRTHDAY", birthDate.ToString("yyyy-MM-dd"));
                                GME_Var.SetSNF("STATUS", "1");
                                GME_Var.SetSNF("EMAIL", emailTxt.Text);
                                GME_Var.SetSNF("FIRSTNAME", firstNameTxt.Text);
                                GME_Var.SetSNF("LASTNAME", lastNameTxt.Text);
                                GME_Var.SetSNF("PHONENUMBER", phoneNumTxt.Text);
                                GME_Var.SetSNF("PERSONID", GME_Var.personId.ToString());
                                GME_Var.SetSNF("CHANNEL", channel);
                                GME_Var.SetSNF("ENROLLTYPE", enrolltype);

                                if (!isUpdatePerson) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEPERSON"); goto outThisFunc; }
                                if (!isUpdateHouseHold) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEHOUSEHOLD"); goto outThisFunc; }
                                if (!isUpdateContactabilitySMS) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYSMS"); goto outThisFunc; }
                                if (!isUpdateContactabilityEMAIL) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYEMAIL"); goto outThisFunc; }
                                outThisFunc:;
                            }
                            this.Close();
                        }
                        else
                        {
                            GME_Var.isEnrollGuest = true;

                            if (isPhoneNumAx || isEmailAx)
                            {
                                GME_Var.isCustomerTypeSearch = true;
                                cust.Search(posTransaction);
                            }
                            else //ENGAGE FOUND
                            {
                                frmCustomerSearch custSearch = new frmCustomerSearch();
                                custSearch.addNewCust(this.firstNameTxt.Text, this.lastNameTxt.Text, this.emailTxt.Text, this.phoneNumTxt.Text, "Trade", posTransaction);
                                this.Close();
                            }

                            this.Close();
                        }
                    }
                    else
                    {
                        //AX FOUND
                        //true
                        GME_Var.isEnrollGuest = true;

                        if (isPhoneNumAx || isEmailAx)
                        {
                            GME_Var.isCustomerTypeSearch = true;
                            cust.Search(posTransaction);
                        }
                        else //ENGAGE FOUND
                        {
                            frmCustomerSearch custSearch = new frmCustomerSearch();
                            custSearch.addNewCust(this.firstNameTxt.Text, this.lastNameTxt.Text, this.emailTxt.Text, this.phoneNumTxt.Text, "Trade", posTransaction);
                            this.Close();
                        }

                        this.Close();
                    }
                }
                else
                {
                    BlankOperations.BlankOperations.ShowMsgBox("customer sudah terdaftar menjadi member dengan tier '" + GME_Var.lookupCardTier + "' dan nomor member '" + GME_Var.lookupCardNumber + "'");
                    GME_Var.searchTerm = GME_Var.lookupCardNumber;
                    cust.Search(posTransaction);
                    this.Close();
                }
            }
            else
            {
                //jika tidak found di ax lanjut add customer
                if (!isCardNumAx)
                {
                    //add enroll
                    GME_Var.isEnrollCustomer = true;
                    frmCustomerSearch custSearch = new frmCustomerSearch();
                    custSearch.addNewCust(this.firstNameTxt.Text, this.lastNameTxt.Text, this.emailTxt.Text, this.phoneNumTxt.Text, "Trade", posTransaction);
                    this.Close();
                }
                else if (isCardNumAx && !islookupValid) //ax found engage not found
                {
                    BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu tidak valid");
                }
            }
        }


        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            GME_FormCaller.GME_FormCustomerType(Connection.applicationLoc, posTransaction);
        }

        private void dayDateTxt_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dayDateTxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void monthDateTxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void yearDateTxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void phoneNumTxt_TextChanged(object sender, EventArgs e)
        {

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

        private void cardMemberTxt_KeyPress(object sender, KeyPressEventArgs e)
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
