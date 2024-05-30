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
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormEnrollment", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmEnrollment : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;
        public DataTable skinTypeDT = new DataTable();
        GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();
        public string gender = string.Empty;
        public string channel = string.Empty;

        public frmEnrollment()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmEnrollment(GME_Custom.GME_Propesties.frmEnrollmentConfirm parm)
            : this()
        {
            this.application = parm.application;
            this.posTransaction = parm.posTransaction;

            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

            this.skinTypeDT = data.getSkinType(this.application.Settings.Database.Connection.ConnectionString);
            this.skinTypeCmb.DataSource = this.skinTypeDT;
            this.skinTypeCmb.DisplayMember = "DESKRIPSI";
            this.skinTypeCmb.ValueMember = "NOMOR";

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
            else if (emailCb.Checked && emailTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Email harus diisi!");
                emailTxt.Focus();
            }
            else if (smsCb.Checked && phoneNumTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor telepon harus diisi!");
                phoneNumTxt.Focus();
            }
            else if (dateOfBitrh.Value == DateTime.Today)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Tanggal lahir harus dipilih!");
                dateOfBitrh.Focus();
            }
            else if (skinTypeCmb.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Jenis kulit harus diisi!");
                skinTypeCmb.Focus();
            }
            else
            {
                frmCustomerSearch custSearch = new frmCustomerSearch();
                custSearch.addNewCust(this.firstNameTxt.Text, this.lastNameTxt.Text, this.emailTxt.Text, this.phoneNumTxt.Text, "Trade", posTransaction, this.cardMemberTxt.Text);

                if (rdBtnMale.Checked)
                    gender = rdBtnMale.Text;
                if (rdBtnFemale.Checked)
                    gender = rdBtnFemale.Text;

                channel = data.getChannelStore(posTransaction.StoreId, GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                data.insertNewCustomerDataExtend(GME_Custom.GME_Propesties.GME_Var.customerData.CustomerId, gender, skinTypeCmb.Text, dateOfBitrh.Value, this.posTransaction.StoreId, 
                    channel, this.posTransaction.TransactionId, this.posTransaction.TerminalId, this.application.Settings.Database.Connection.ConnectionString);

                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
