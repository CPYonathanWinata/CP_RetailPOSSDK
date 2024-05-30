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
using GME_Custom.GME_Propesties;
using LSRetailPosis.Transaction.Line.SaleItem;
using LSRetailPosis.Transaction.Line.Discount;
using GME_Custom.GME_EngageFALWSServices;
using Jacksonsoft;
using System.Threading.Tasks;
using System.Linq;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmWelcomeVoucher", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmWelcomeVoucher : frmTouchBase, IInteractionView
    {
        string voucherId = string.Empty;
        string memberId = string.Empty;

        public IApplication application;
        public IPosTransaction posTransaction;

        public frmWelcomeVoucher()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmWelcomeVoucher(GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm parm)
            : this()
        {
            voucherId = parm.voucherId;
            this.application = parm.application;
            this.VoucherIdTxt.Focus();
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK
            } as TResults;
        }
        /*Interactionview Implementation for Argument send (current Null)*/
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            //throw new System.NotImplementedException();
        }

        #endregion      
        
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (!GME_Var.isVoucherApplied)
            {
                GME_Var.iswelcomevoucher50 = false;
                GME_Var.iswelcomevoucher100 = false;
            }
            GME_Var.wlcmvouchamount = 0;           
            GME_Var.iswelcomevoucher = false;
            GME_Var.isgetidentifiersukses = false;
            GME_Var.isWelcomeVoucherApplied = false;
            this.Close();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (VoucherIdTxt.Text == "" && totalamountTxt.Text == "")
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Anda belum memasukan kode voucher");
            }
            else
            {
                if (!GME_Method.CheckForInternetConnection())
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Sedang Offline, Voucher tidak dapat digunakan");
                }
                else
                {
                    Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");
                }
            }
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            posTransaction = GME_Var.welcomePosTransaction;
            LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();
            //GET ITEM WITHOUT VOUCHER EXCLUDE
            int j = 0;
            var categoryAX = new long[data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
            foreach (DataRow row in data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
            {
                categoryAX[j] = row.Field<long>("PARENTCATEGORY");
                j++;
            }

            int count = 0;
            var itemVouchTBSI = new string[0];
            var itemVouchTBSIold = new string[0];
            foreach (SaleLineItem lineItem in tmp)
            {
                foreach (DataRow row in data.getCategoryItem(lineItem.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CATEGORY", typeof(string));
                    dt.Rows.Add(row.Field<long>("LEVEL1"));
                    dt.Rows.Add(row.Field<long>("LEVEL2"));
                    dt.Rows.Add(row.Field<long>("LEVEL3"));
                    dt.Rows.Add(row.Field<long>("LEVEL4"));
                    dt.Rows.Add(row.Field<long>("LEVEL5"));
                    dt.Rows.Add(row.Field<long>("LEVEL6"));
                    string[] allcategoryItem = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                    for (j = 0; j < categoryAX.Length; j++)
                    {
                        for (int i = 0; i < allcategoryItem.Length; i++)
                        {
                            if (categoryAX[j] == Convert.ToInt64(allcategoryItem[i]))
                            {
                                { goto outtoendmethod; }
                            }

                            if (i == allcategoryItem.Length - 1 && j == categoryAX.Length - 1)
                            {
                                count++;
                                itemVouchTBSI = new string[count];
                                itemVouchTBSI[count - 1] = lineItem.ItemId;

                                if (itemVouchTBSI[0] == null)
                                {
                                    for (int z = 0; z < itemVouchTBSIold.Length; z++)
                                    {
                                        itemVouchTBSI[z] = itemVouchTBSIold[z];
                                    }
                                }
                                itemVouchTBSIold = itemVouchTBSI;
                                { goto outtoendmethod; }
                            }
                        }
                    }
                }
                outtoendmethod:;
            }
            GME_Var.itemVouchTBSI = itemVouchTBSI;
            RetailTransaction retailTrans = posTransaction as RetailTransaction;
            decimal minbelanja = 0;

            if (retailTrans == null)
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Lakukan transaksi terlebih dahulu sebelum menggunakan voucher");
            }
            else
            {
                //WELCOME VOUCHER 50RB
                if (GME_Var.iswelcomevoucher50 == true)
                {
                    string verifikasi = data.getVerification50Rb(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                    if (verifikasi == "1")
                    {
                        int countwlcm = 0;
                        minbelanja = Convert.ToDecimal(data.getMinTransaksi50(Connection.applicationLoc.Settings.Database.Connection.ConnectionString));

                        var categoryWelcome50AX = new long[data.getkategori50(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
                        foreach (DataRow row in data.getkategori50(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                        {
                            categoryWelcome50AX[countwlcm] = row.Field<long>("RECID");
                            countwlcm++;
                        }

                        var itemVouchWLCM50 = new string[0];
                        var itemVouchWLCM50old = new string[0];
                        decimal stash50k = 0;
                        int counts = 0;

                        foreach (SaleLineItem lineItem in tmp)
                        {
                            for (int tbsi = 0; tbsi < itemVouchTBSIold.Length; tbsi++)
                            {
                                if (lineItem.ItemId == itemVouchTBSIold[tbsi] && lineItem.Voided == false)
                                {
                                    foreach (DataRow row in data.getCategoryItem(lineItem.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                                    {
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add("KATEGORI", typeof(string));
                                        dt.Rows.Add(row.Field<long>("LEVEL1"));
                                        dt.Rows.Add(row.Field<long>("LEVEL2"));
                                        dt.Rows.Add(row.Field<long>("LEVEL3"));
                                        dt.Rows.Add(row.Field<long>("LEVEL4"));
                                        dt.Rows.Add(row.Field<long>("LEVEL5"));
                                        dt.Rows.Add(row.Field<long>("LEVEL6"));
                                        string[] allcategory = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                                        for (int k = 0; k < categoryWelcome50AX.Length; k++)
                                        {
                                            for (int i = 0; i < allcategory.Length; i++)
                                            {
                                                if (categoryWelcome50AX[k] == Convert.ToInt64(allcategory[i]))
                                                {
                                                    counts++;
                                                    itemVouchWLCM50 = new string[counts];
                                                    itemVouchWLCM50[counts - 1] = lineItem.ItemId;

                                                    if (itemVouchWLCM50[0] == null)
                                                    {
                                                        for (int iterasi = 0; iterasi < itemVouchWLCM50old.Length; iterasi++)
                                                        {
                                                            itemVouchWLCM50[iterasi] = itemVouchWLCM50old[iterasi];
                                                        }
                                                    }
                                                    itemVouchWLCM50old = itemVouchWLCM50;

                                                    stash50k = stash50k + lineItem.NetAmount;
                                                    { goto outtoendmethod; }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            outtoendmethod:;
                            GME_Var.displaynumberWelcomeVouch = itemVouchWLCM50old;
                        }

                        if (stash50k >= minbelanja)
                        {
                            GME_Var.totalbelanjakategori = stash50k;
                            GME_Var.iswelcomevoucher50 = true;
                            GME_Var.isWelcomeVoucherApplied = true;
                            GME_Var.DiscVoucher = 1;
                        }
                        else
                        {
                            BlankOperations.BlankOperations.ShowMsgBoxInformation("Belanja minimal Rp " + decimal.ToInt32(minbelanja));
                            GME_Var.iswelcomevoucher50 = false;
                            GME_Var.wlcmvouchamount = 0;
                            GME_Var.isWelcomeVoucherApplied = false;
                        }
                    }
                }

                //WELCOME VOUCHER 100RB
                else
                {
                    string verifikasi = data.getVerification100Rb(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                    if (verifikasi == "1")
                    {
                        int countwlcm = 0;
                        minbelanja = Convert.ToDecimal(data.getMinTransaksi100(GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString));

                        var categoryWelcome100AX = new long[data.getkategori100(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
                        foreach (DataRow row in data.getkategori100(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                        {
                            categoryWelcome100AX[countwlcm] = row.Field<long>("RECID");
                            countwlcm++;
                        }

                        var itemVouchWLCM100 = new string[0];
                        var itemVouchWLCM100old = new string[0];
                        decimal stash100k = 0;
                        int counts = 0;

                        foreach (SaleLineItem lineItem in tmp)
                        {
                            for (int tbsi = 0; tbsi < itemVouchTBSIold.Length; tbsi++)
                            {
                                if (lineItem.ItemId == itemVouchTBSIold[tbsi] && lineItem.Voided == false)
                                {
                                    foreach (DataRow row in data.getCategoryItem(lineItem.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                                    {
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add("KATEGORI", typeof(string));
                                        dt.Rows.Add(row.Field<long>("LEVEL1"));
                                        dt.Rows.Add(row.Field<long>("LEVEL2"));
                                        dt.Rows.Add(row.Field<long>("LEVEL3"));
                                        dt.Rows.Add(row.Field<long>("LEVEL4"));
                                        dt.Rows.Add(row.Field<long>("LEVEL5"));
                                        dt.Rows.Add(row.Field<long>("LEVEL6"));
                                        string[] allcategory = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                                        for (int k = 0; k < categoryWelcome100AX.Length; k++)
                                        {
                                            for (int i = 0; i < allcategory.Length; i++)
                                            {
                                                if (categoryWelcome100AX[k] == Convert.ToInt64(allcategory[i]))
                                                {
                                                    counts++;
                                                    itemVouchWLCM100 = new string[counts];
                                                    itemVouchWLCM100[counts - 1] = lineItem.ItemId;

                                                    if (itemVouchWLCM100[0] == null)
                                                    {
                                                        for (int iterasi = 0; iterasi < itemVouchWLCM100old.Length; iterasi++)
                                                        {
                                                            itemVouchWLCM100[iterasi] = itemVouchWLCM100old[iterasi];
                                                        }
                                                    }
                                                    itemVouchWLCM100old = itemVouchWLCM100;

                                                    stash100k = stash100k + lineItem.NetAmount;
                                                    { goto outtoendmethod; }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            outtoendmethod:;
                            GME_Var.displaynumberWelcomeVouch = itemVouchWLCM100old;
                        }

                        if (stash100k >= minbelanja)
                        {
                            GME_Var.totalbelanjakategori = stash100k;
                            GME_Var.iswelcomevoucher100 = true;
                            GME_Var.isWelcomeVoucherApplied = true;
                            GME_Var.DiscVoucher = 1;
                        }
                        else
                        {
                            BlankOperations.BlankOperations.ShowMsgBoxInformation("Belanja minimal Rp " + decimal.ToInt32(minbelanja));
                            GME_Var.iswelcomevoucher100 = false;
                            GME_Var.wlcmvouchamount = 0;
                            GME_Var.isWelcomeVoucherApplied = false;
                        }
                    }
                }
                GME_Var.isgetidentifiersukses = false;
                this.Close();
            }
        }

        private void DoProgressGetVoucher(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            GME_Custom.GME_Data.IntegrationService integration = new GME_Custom.GME_Data.IntegrationService();
            GME_Var.voucherdipake = VoucherIdTxt.Text;
            memberId = GME_Var.identifierCode;

            integration.getIdentifier(memberId);

            string text = string.Empty;
            string perLines = string.Empty;

            if (GME_Custom.GME_Propesties.GME_Var.personId != 0)
            {
                GME_Var.isgetidentifiersukses = true;
                GME_Var.iswelcomevoucher = true;
                integration.getValidVouchersForCard(memberId);
                if (GME_Var.isWelcomeVoucherApplied == true)
                {
                    var b = GME_Var.wlcmvouchamount.ToString("#,##0.00");
                    totalamountTxt.Text = b;
                }
                else
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Kode voucher yang anda masukan tidak ditemukan");
                    this.Close();
                }
                VoucherIdTxt.Text = string.Empty;
            }
            else
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Masukan customer terlebih dahulu");
                this.Close();
            }
        }

        private void VoucherIdTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Jacksonsoft.WaitWindow.Show(this.DoProgressGetVoucher, "Please Wait ...");
            }                   
        }
    }
}
