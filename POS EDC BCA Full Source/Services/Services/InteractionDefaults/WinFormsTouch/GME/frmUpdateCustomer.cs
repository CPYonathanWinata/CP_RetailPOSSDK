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
    [Export("FormUpdateCustomer", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmUpdateCustomer : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;        
        public DataTable skinTypeDT = new DataTable();
        public DataTable customerInfoDT = new DataTable();        
        queryData data = new queryData();
        public string gender = string.Empty;
        public string channel = string.Empty;
        public string birthDateStr = string.Empty;
        public DateTime birthDate = DateTime.MinValue;
        public string accountNum = string.Empty;


        public frmUpdateCustomer()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmUpdateCustomer(frmUpdateCustomerConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;

            this.skinTypeDT = data.getSkinType(this.application.Settings.Database.Connection.ConnectionString);
            this.skinTypeCmb.DataSource = this.skinTypeDT;
            this.skinTypeCmb.DisplayMember = "DESKRIPSI";
            //this.skinTypeCmb.ValueMember = "NOMOR";
            this.phoneNumTxt.Text = "628";
            this.smsCb.Checked = true;
            this.emailCb.Checked = true;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmUpdateCustomerConfirm
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

        private void updateBtn_Click(object sender, EventArgs e)
        {
            bool readyToInsert = true;

            if (this.cardMemberTxt.TextLength < 14)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jumlah nomor kartu member harus 14 digit.");
                cardMemberTxt.Focus();
                readyToInsert = false;
            }
            else if (dayDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tanggal harus diisi!");
                dayDateTxt.Focus();
                readyToInsert = false;
            }
            else if (monthDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Bulan harus diisi!");
                monthDateTxt.Focus();
                readyToInsert = false;
            }
            else if (yearDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tahun harus diisi!");
                yearDateTxt.Focus();
                readyToInsert = false;
            }
            else if (cardMemberTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu member harus diisi!");
                cardMemberTxt.Focus();
                readyToInsert = false;
            }
            else if (firstNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama depan harus diisi!");
                firstNameTxt.Focus();
                readyToInsert = false;
            }
            else if (lastNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama belakang harus diisi!");
                lastNameTxt.Focus();
                readyToInsert = false;
            }
            else if (rdBtnMale.Checked == false && rdBtnFemale.Checked == false)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jenis kelamin harus dipilih!");
                readyToInsert = false;
            }
            else if (emailCb.Checked && emailTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Email harus diisi!");
                emailTxt.Focus();
                readyToInsert = false;
            }
            else if (this.emailCb.Checked && !this.emailTxt.Text.Contains("@"))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format email salah.");
                readyToInsert = false;
            }
            else if (smsCb.Checked && (phoneNumTxt.Text == "628" || string.IsNullOrEmpty(this.phoneNumTxt.Text) || this.phoneNumTxt.Text.Length <= 3))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor handphone harus diisi");
                phoneNumTxt.Focus();
                readyToInsert = false;
            }
            else if (this.phoneNumTxt.Text.Length <= 10 || this.phoneNumTxt.Text.Substring(0, 4) == "6280")
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format nomor handphone salah.");
                readyToInsert = false;
            }
            else if (!GME_Method.CheckForInternetConnection())
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Sedang Offline, tidak dapat melakukan update member");
                readyToInsert = false;
            }
            else
            {
                if (rdBtnMale.Checked)
                    gender = "M";
                else if (rdBtnFemale.Checked)
                    gender = "F";

                IntegrationService integration = new IntegrationService();
                updateCustomer updateCustomer = new updateCustomer();

                bool isLookupCard = false;
                bool isUpdatePerson = false;
                bool isUpdateHouseHold = false;
                bool isUpdateContactabilitySMS = false;
                bool isUpdateContactabilityEMAIL = false;
                bool isSNF = false;

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
                    if (integration.lookupCard(cardMemberTxt.Text) == "VALID")
                    {
                        integration.requestReward0302(cardMemberTxt.Text, application.Shift.StoreId);
                        isLookupCard = true;
                        if (integration.updatePersonUpdateCustomer(GME_Var.lookupCardPersonId, phoneNumTxt.Text, emailTxt.Text, firstNameTxt.Text, birthDate, gender, lastNameTxt.Text, skinTypeCmb.Text) == "Success")
                        {
                            isUpdatePerson = true;
                            if (integration.updateHousehold(GME_Var.UpdateCustHouseholdId, application.Shift.StoreId, emailTxt.Text, phoneNumTxt.Text) == "Success")
                            {
                                isUpdateHouseHold = true;

                                if (smsCb.Checked && phoneNumTxt.Text != string.Empty)
                                {
                                    if (integration.updateContactabilitySMS(GME_Var.lookupCardPersonId) == "Success")
                                    {
                                        isUpdateContactabilitySMS = true;
                                    }
                                }

                                if (emailCb.Checked && emailTxt.Text != string.Empty)
                                {
                                    if (integration.updateContactabilityEmail(GME_Var.lookupCardPersonId) == "Success")
                                    {
                                        isUpdateContactabilityEMAIL = true;
                                    }
                                }
                            }

                            channel = data.getChannelStore(application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            if (data.insertUpdateCustomerDataPull(firstNameTxt.Text, lastNameTxt.Text, gender, emailTxt.Text, birthDate.ToString(), phoneNumTxt.Text, skinTypeCmb.Text, accountNum,
                                                      application.Settings.Database.Connection.ConnectionString, channel, application.Shift.TerminalId, application.Shift.StoreId, posTransaction.TransactionId))
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Update customer success");
                            }

                            integration.requestReward0302(cardMemberTxt.Text, application.Shift.StoreId);
                            System.Threading.Thread.Sleep(5000);
                        }
                        else
                        {
                            BlankOperations.BlankOperations.ShowMsgBoxInformation("Untuk saat ini tidak dapat melakukan update member");
                        }
                    }
                    else
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Untuk saat ini tidak dapat melakukan update member");
                    }
                    
                    //else //SNF
                    //{
                    //    data.insertUpdateCustomerDataPull(firstNameTxt.Text, lastNameTxt.Text, gender, emailTxt.Text, birthDate.ToString(), phoneNumTxt.Text, skinTypeCmb.Text, accountNum,
                    //                          application.Settings.Database.OfflineConnection.ConnectionString, channel, application.Shift.TerminalId, application.Shift.StoreId, posTransaction.TransactionId);

                    //    BlankOperations.BlankOperations.ShowMsgBoxInformation("Member akan diupdate 1 X 24 Jam");

                    //    GME_Var.SetSNF("HOUSEHOLDID", GME_Var.personHouseHoldId.ToString());
                    //    GME_Var.SetSNF("IDENTIFIERID", cardMemberTxt.Text);
                    //    GME_Var.SetSNF("CUSTOMERID", GME_Var.customerData.CustomerId);
                    //    GME_Var.SetSNF("GENDER", gender);
                    //    GME_Var.SetSNF("SKINTYPE", skinTypeCmb.Text);
                    //    GME_Var.SetSNF("BIRTHDAY", birthDate.ToString("yyyy-MM-dd"));
                    //    GME_Var.SetSNF("STATUS", "1");
                    //    GME_Var.SetSNF("EMAIL", gender);
                    //    GME_Var.SetSNF("FIRSTNAME", firstNameTxt.Text);
                    //    GME_Var.SetSNF("LASTNAME", lastNameTxt.Text);
                    //    GME_Var.SetSNF("PHONENUMBER", phoneNumTxt.Text);
                    //    GME_Var.SetSNF("PERSONID", GME_Var.personId.ToString());
                    //    GME_Var.SetSNF("CHANNEL", channel);
                    //    //GME_Var.SetSNF("ENROLLTYPE", "");

                    //    isSNF = true;

                    //}

                    ////check salah satu proses fail?
                    //if (!isLookupCard || !isUpdatePerson || !isUpdateHouseHold || !isUpdateContactabilitySMS || !isUpdateContactabilityEMAIL)
                    //{
                    //    if (!isLookupCard) { GME_Var.SetSNF("PROCESSSTOPPED", "LOOKUPCARD"); goto outThisFunc; }
                    //    if (!isUpdatePerson) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEPERSON"); goto outThisFunc; }
                    //    if (!isUpdateHouseHold) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEHOUSEHOLD"); goto outThisFunc; }
                    //    if (!isUpdateContactabilitySMS) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYSMS"); goto outThisFunc; }
                    //    if (!isUpdateContactabilityEMAIL) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYEMAIL"); goto outThisFunc; }
                    //    outThisFunc:;
                    //}

                    //if (isSNF)
                    //{
                    //    queryDataOffline queryDataOffline = new queryDataOffline();
                    //    if (!GME_Var.pingStatus)
                    //    {
                    //        queryDataOffline.insertOfflineSNF(posTransaction, application.Settings.Database.OfflineConnection.ConnectionString);

                    //        //GME_Method.setAllVariableToDefault();
                    //    }
                    //    else
                    //    {
                    //        queryDataOffline.insertOfflineSNF(posTransaction, application.Settings.Database.Connection.ConnectionString);

                    //        // GME_Method.setAllVariableToDefault();
                    //    }
                    //    GME_Var.clearSNF();

                    //}
                    this.Close();
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {                        
            if (cardMemberTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu member harus diisi!");
            }
            else if (this.cardMemberTxt.TextLength < 14)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jumlah nomor kartu member harus 14 digit.");
            }            
            else
            {
                Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");
            }
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            IntegrationService integration = new IntegrationService();
            bool isValid = false;
            string birtdateStr = string.Empty;

            if (integration.lookupCard(this.cardMemberTxt.Text) == "VALID")
            {
                isValid = true;

                if (integration.getPerson(GME_Var.lookupCardPersonId) == "Success")
                {
                    isValid = true;
                }
            }

            if (isValid)
            {
                integration.getContactability(GME_Var.lookupCardPersonId);

                firstNameTxt.Text = GME_Var.personFirstName;
                lastNameTxt.Text = GME_Var.personLastName;

                if (GME_Var.personGender == "M")
                    rdBtnMale.Checked = true;

                if (GME_Var.personGender == "F")
                    rdBtnFemale.Checked = true;

                emailTxt.Text = GME_Var.personEmail;
                phoneNumTxt.Text = GME_Var.personPhone;

                if (!GME_Var.contactAbilitysubscriptionEmail)
                    emailCb.Checked = false;

                if (!GME_Var.contactAbilitysubscriptionSMS)
                    smsCb.Checked = false;

                if (GME_Var.personBirthdate != string.Empty)
                {
                    birtdateStr = GME_Var.personBirthdate.Substring(0, 10);

                    if (birtdateStr.Length == 9)
                    {
                        dayDateTxt.Text = birtdateStr.Substring(2, 2);
                        monthDateTxt.Text = birtdateStr.Substring(0, 2);
                        yearDateTxt.Text = birtdateStr.Substring(6, 4);
                    }
                    else
                    {
                        dayDateTxt.Text = birtdateStr.Substring(3, 2);
                        monthDateTxt.Text = birtdateStr.Substring(0, 2);
                        yearDateTxt.Text = birtdateStr.Substring(6, 4);
                    }
                }
                skinTypeCmb.Text = GME_Var.personSkinType;
            }
            else
            {
                this.Close();
            }
        }

        private void dayDateTxt_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void monthDateTxt_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void yearDateTxt_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void phoneNumTxt_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            bool readyToInsert = true;

            if (this.cardMemberTxt.TextLength < 14)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jumlah nomor kartu member harus 14 digit.");
                cardMemberTxt.Focus();
                readyToInsert = false;
            }
            else if (dayDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tanggal harus diisi!");
                dayDateTxt.Focus();
                readyToInsert = false;
            }
            else if (monthDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Bulan harus diisi!");
                monthDateTxt.Focus();
                readyToInsert = false;
            }
            else if (yearDateTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tahun harus diisi!");
                yearDateTxt.Focus();
                readyToInsert = false;
            }
            else if (cardMemberTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor kartu member harus diisi!");
                cardMemberTxt.Focus();
                readyToInsert = false;
            }
            else if (firstNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama depan harus diisi!");
                firstNameTxt.Focus();
                readyToInsert = false;
            }
            else if (lastNameTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nama belakang harus diisi!");
                lastNameTxt.Focus();
                readyToInsert = false;
            }
            else if (rdBtnMale.Checked == false && rdBtnFemale.Checked == false)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jenis kelamin harus dipilih!");
                readyToInsert = false;
            }
            else if (emailCb.Checked && emailTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Email harus diisi!");
                emailTxt.Focus();
                readyToInsert = false;
            }
            else if (this.emailCb.Checked && !this.emailTxt.Text.Contains("@"))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format email salah.");
                readyToInsert = false;
            }
            else if (smsCb.Checked && (phoneNumTxt.Text == "628" || string.IsNullOrEmpty(this.phoneNumTxt.Text) || this.phoneNumTxt.Text.Length <= 3))
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor handphone harus diisi");
                phoneNumTxt.Focus();
                readyToInsert = false;
            }
            else if (this.phoneNumTxt.Text.Length <= 10 || this.phoneNumTxt.Text.Substring(0, 4) == "6280")
            {
                BlankOperations.BlankOperations.ShowMsgBox("Format nomor handphone salah.");
                readyToInsert = false;
            }
            else
            {
                if (rdBtnMale.Checked)
                    gender = "M";
                else if (rdBtnFemale.Checked)
                    gender = "F";

                IntegrationService integration = new IntegrationService();
                updateCustomer updateCustomer = new updateCustomer();

                bool isLookupCard = false;
                bool isUpdatePerson = false;
                bool isUpdateHouseHold = false;
                bool isUpdateContactabilitySMS = false;
                bool isUpdateContactabilityEMAIL = false;
                bool isSNF = false;

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
                    if (integration.lookupCard(cardMemberTxt.Text) == "VALID")
                    {
                        integration.requestReward0302(cardMemberTxt.Text, application.Shift.StoreId);
                        isLookupCard = true;
                        if (integration.updatePersonUpdateCustomer(GME_Var.lookupCardPersonId, phoneNumTxt.Text, emailTxt.Text, firstNameTxt.Text, birthDate, gender, lastNameTxt.Text, skinTypeCmb.Text) == "Success")
                        {
                            isUpdatePerson = true;
                            if (integration.updateHousehold(GME_Var.UpdateCustHouseholdId, application.Shift.StoreId, emailTxt.Text, phoneNumTxt.Text) == "Success")
                            {
                                isUpdateHouseHold = true;

                                if (smsCb.Checked && phoneNumTxt.Text != string.Empty)
                                {
                                    if (integration.updateContactabilitySMS(GME_Var.lookupCardPersonId) == "Success")
                                    {
                                        isUpdateContactabilitySMS = true;
                                    }
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
                    }

                    channel = data.getChannelStore(application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                    if (data.insertUpdateCustomerDataPull(firstNameTxt.Text, lastNameTxt.Text, gender, emailTxt.Text, birthDate.ToString(), phoneNumTxt.Text, skinTypeCmb.Text, accountNum,
                                              application.Settings.Database.Connection.ConnectionString, channel, application.Shift.TerminalId, application.Shift.StoreId, posTransaction.TransactionId))
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Update customer success");
                    }
                }
            }

            this.Close();
        }
    }
}
