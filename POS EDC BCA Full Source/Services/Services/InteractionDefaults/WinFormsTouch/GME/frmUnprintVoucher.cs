/// ADD BY MARIA   -- FORM Unprint Voucher
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
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Drawing.Printing; // maria
using System.Drawing;//maria
using System.Net; // maria
using GME_Custom.GME_Data;
using System.Text;//
using System.Resources.Tools;
using DevExpress.XtraPrinting;
using OnBarcode.Barcode;
using PQScan.BarcodeCreator;
using System.Drawing.Imaging;
using System.IO;
using LSRetailPosis.Settings.HardwareProfiles;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmUnprintVoucher", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FrmUnprintVoucher : frmTouchBase, IInteractionView
    {
        IApplication application = null;
        string memberId = string.Empty;
        string printerInstalled = string.Empty; /// test for printing

        public FrmUnprintVoucher()
        {
            InitializeComponent();
        }


        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new FrmUnprintVoucherConfirm
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
        public FrmUnprintVoucher(FrmUnprintVoucherConfirm parm)
            : this()
        {
            application = parm.application;

        }

        private void OkBtn_Click(object sender, EventArgs e)
        {

            GME_Var.isVoucherPrinted = false;
            GME_Var.personId = 0;
            GME_Var.getVoucher = "";

            memberId = memberIdTxt.Text;

            IntegrationService integration = new IntegrationService();

            if (!GME_Method.CheckForInternetConnection())
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Sedang Offline, tidak dapat melakukan unprint");
            }
            else
            {
                integration.getIdentifier(memberId);

                if (GME_Var.personId != 0)
                {
                    GME_Var.isVoucherPrinted = true;
                    integration.getValidVouchersForCard(memberId);

                    bool isSuccess = false;

                    if (GME_Var.unPrintVouch != null)
                    {
                        for (int i = 0; i < GME_Var.unPrintVouch.Length; i++) /// gantiin
                        {
                            GME_Var.getVoucher = GME_Var.unPrintVouch[i].ToString();
                            isSuccess = GME_Method.PrintReceiptPage(GME_Var.getVoucher);
                        }
                    }

                    //// SHOW MSG BOX IF PRINTED
                    if (isSuccess)
                    {
                        BlankOperations.BlankOperations.ShowMsgBoxInformation(GME_Var.msgUnprintVoucher);
                        this.Close();
                    }
                    else
                    {
                        BlankOperations.BlankOperations.ShowMsgBox(GME_Var.msgUnprintVoucherError);
                    }
                }
            }
        }


        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}