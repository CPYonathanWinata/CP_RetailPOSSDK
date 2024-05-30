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
using LSRetailPosis.Transaction;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FormBonManual", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmBonManual : frmTouchBase, IInteractionView
    {
        public IApplication application;
        public string store;

        public frmBonManual()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmBonManual(frmBonManualConfirm parm)
            : this()
        {
            this.application = parm.apps;
            this.store = parm.storeId;
            this.BonManualNumTxt.Focus();
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmBonManualConfirm
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

        private void button2_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (!data.isBonManualNumber(this.store, BonManualNumTxt.Text, this.application.Settings.Database.Connection.ConnectionString))
            //    {
            //        BlankOperations.BlankOperations.ShowMsgBox("The number has been used or the number does not exists");
            //    }
            //    textBox2.Focus();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BonManualNumTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Nomor bon manual harus diisi");
            }
            else if (reasonTxt.Text == string.Empty)
            {
                BlankOperations.BlankOperations.ShowMsgBox("Reason harus diisi");
            }
            else
            {
                GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

                if (data.isBonManualNumber(this.store, BonManualNumTxt.Text, this.application.Settings.Database.Connection.ConnectionString) == "1")
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Nomor bon manual sudah digunakan");
                }
                else if (data.isBonManualNumber(this.store, BonManualNumTxt.Text, this.application.Settings.Database.Connection.ConnectionString) == string.Empty)
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Nomor bon manual tidak ada / data belum terupdate pada store server");
                }
                else
                {
                    List<string> nextBonManualNum = new List<string>();
                    string bonManualNum = string.Empty;

                    GME_Var.bonManualNumber = BonManualNumTxt.Text;
                    GME_Var.reasonBonManual = reasonTxt.Text;

                    nextBonManualNum = data.checkBonManualLongkap(this.store, this.BonManualNumTxt.Text, this.application.Settings.Database.Connection.ConnectionString);

                    for (int i = 0; i < nextBonManualNum.Count; i++)
                    {
                        if (i + 1 != nextBonManualNum.Count)
                            bonManualNum += nextBonManualNum[i] + ", ";
                        else
                            bonManualNum += nextBonManualNum[i];
                    }

                    if (nextBonManualNum.Count > 0)
                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Ada nomor bon manual yang longkap terdiri dari : '" + bonManualNum + "'");

                    data.updateBonManualDetails(BonManualNumTxt.Text, this.store, this.application.Settings.Database.Connection.ConnectionString);

                    GME_Var.isBonManual = true;

                    this.Close();
                }
            }
        }
    }
}
