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
using GME_Custom.GME_EngageServices;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmItemVoucher", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmItemVoucher : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public IPosTransaction posTransaction;

        public frmItemVoucher()
        {
            InitializeComponent();
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmItemVoucherConfirm
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

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmItemVoucher(frmItemVoucherConfirm parm)
            : this()
        {
            application = parm.application;
        }

        private void VoucherIdTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OKBtn_Click_1(sender, e);
            }
        }

        private void OKBtn_Click_1(object sender, EventArgs e)
        {
            Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            GME_Var.isAktifVoucher = true;
            queryData data = new queryData();
            vouchersList vouchersList = new vouchersList();
            GME_Var.isDisc = false;
            GME_Var.isCash = false;

            IntegrationService integrationService = new IntegrationService();

            if (VoucherIdTxt.Text.Length < 1 && VoucherIdTxt.Text == string.Empty) { BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher ID masih kosong !"); VoucherIdTxt.Focus(); }
            else if (VoucherIdTxt.Text.Length > 0 && VoucherIdTxt.Text != string.Empty)
            {

                if(GME_Var.listItemVoucher.Count > 0)
                {
                    foreach (var itemVouch in GME_Var.listItemVoucher)
                    {
                        if (itemVouch.Equals(VoucherIdTxt.Text.Trim()))
                        {
                            BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher ID sudah pernah di input. Harap masukkan voucher id yang lain ");
                            goto exitthisfunc;
                        }
                    }
                }

                vouchersList = integrationService.lookup(Connection.applicationLoc.Settings.Database.DataAreaID, VoucherIdTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                for (int i = 0; i < vouchersList.VoucherItems.Count; i++)
                {
                    if (vouchersList.VoucherItems[i].responseCode == 0)
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher ID tidak ada, tolong scan kembali Voucher ID");
                        VoucherIdTxt.Clear();
                        VoucherIdTxt.Focus();
                    }
                    else if (vouchersList.VoucherItems[i].responseCode == 2 || vouchersList.VoucherItems[i].responseCode == 4)
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("No Voucher sudah di redeem, silahkan pilih no Voucher ID yang lain");
                        VoucherIdTxt.Clear();
                        VoucherIdTxt.Focus();
                    }
                    else if (vouchersList.VoucherItems[i].responseCode == 3)
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("No Voucher sudah di blocked");
                        VoucherIdTxt.Clear();
                        VoucherIdTxt.Focus();
                    }
                    else if (vouchersList.VoucherItems[i].responseCode == 6 || vouchersList.VoucherItems[i].responseCode == 1)
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("No Voucher sudah di activasi tidak dapat digunakan");
                        VoucherIdTxt.Clear();
                        VoucherIdTxt.Focus();
                    }
                    else
                    {
                        if (VoucherIdTxt.Text.Length > 0 && VoucherIdTxt.Text != string.Empty)
                        {
                            //insert to temp
                            data.insertToActiveVoucherTemp(application.Shift.StoreId, application.Shift.TerminalId, VoucherIdTxt.Text, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            GME_Var.isActiveVoucher = true;
                            GME_Var.payVoucherId = VoucherIdTxt.Text;
                            GME_Var.listItemVoucher.Add(VoucherIdTxt.Text);
                            this.Close();
                        }
                    }
                }

                exitthisfunc:;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
