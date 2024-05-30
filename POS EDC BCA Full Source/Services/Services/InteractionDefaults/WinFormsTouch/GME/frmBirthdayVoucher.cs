using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
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
using GME_Custom.GME_EngageFALWSServices;
using GME_Custom.GME_Propesties;
using LSRetailPosis.Transaction.Line.SaleItem;
using Jacksonsoft;
using System.Threading.Tasks;
using System.Linq;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmBirthdayVoucher", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmBirthdayVoucher : frmTouchBase, IInteractionView
    {
        string voucherId = string.Empty;

        string memberId = string.Empty;

        public IApplication application;
        public IPosTransaction posTransaction;

        public frmBirthdayVoucher()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmBirthdayVoucher(GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm parm)
            : this()
        {
            voucherId = parm.voucherId;

            this.application = parm.application;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm
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

        private void OKBtn_Click(object sender, EventArgs e)
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

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            posTransaction = GME_Var.welcomePosTransaction;
            LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

            int j = 0;
            var categoryAX = new long[data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
            foreach (DataRow row in data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
            {
                categoryAX[j] = row.Field<long>("PARENTCATEGORY");
                j++;
            }

            int count = 0;
            var itemVouchBirthday = new string[0];
            var itemVouchBirthdayOld = new string[0];
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
                                itemVouchBirthday = new string[count];
                                itemVouchBirthday[count - 1] = lineItem.ItemId;

                                if (itemVouchBirthday[0] == null)
                                {
                                    for (int z = 0; z < itemVouchBirthdayOld.Length; z++)
                                    {
                                        itemVouchBirthday[z] = itemVouchBirthdayOld[z];
                                    }
                                }
                                itemVouchBirthdayOld = itemVouchBirthday;
                                { goto outtoendmethod; }
                            }
                        }
                    }
                }
                outtoendmethod:;
            }
            GME_Var.itemVouchTBSI = itemVouchBirthday;

            //GET NET AMOUNT
            decimal netamountBirthday = 0;
            foreach (SaleLineItem lineItem in tmp)
            {
                for (int i = 0; i < itemVouchBirthdayOld.Length; i++)
                {
                    if (lineItem.ItemId == itemVouchBirthday[i] && lineItem.Voided == false)
                    {
                        netamountBirthday = netamountBirthday + lineItem.NetAmount;
                    }
                }
                GME_Var.netAmountBirthday = netamountBirthday;
            }

            bool isFailed = false;
            if (VoucherIdTxt.Text.Length < 1 && VoucherIdTxt.Text == string.Empty) { BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher ID masih kosong !"); isFailed = true; }
            
            DataTable parameterVoucherDT = new DataTable();

            GME_Custom.GME_Data.IntegrationService integrationService = new GME_Custom.GME_Data.IntegrationService();
            int percentageDisc = 0;

            string memberid = GME_Var.identifierCode;
            integrationService.getIdentifier(memberid);

            if (memberid != string.Empty && memberid != null && VoucherIdTxt.Text != string.Empty)
            {
                if (integrationService.getValidVouchersForCard(memberid) != "Get voucher for card success")
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher tidak ditemukan");
                    isFailed = true;
                }
            }

            if (GME_Custom.GME_Propesties.GME_Var.vouchersResp != null)
            {

                voucherCheckDTO[] vcResp = GME_Custom.GME_Propesties.GME_Var.vouchersResp;
                string trxNumber = GME_Custom.GME_Propesties.GME_Var.customerPosTransaction.TransactionId;
                //loop list check voucher
                int counters = 0;
                for (int i = 0; i < vcResp.Length; i++)
                {
                    counters++;
                    if (vcResp[i].voucherId == VoucherIdTxt.Text)
                    {
                        counters--;
                        //check status voucher ?
                        if (vcResp[i].voucherStatus.ToString() == "1")
                        {
                            //check tanggal?
                            int result = DateTime.Compare(DateTime.Now, vcResp[i].validityDate);
                            if (result > 0)
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher tidak valid / sudah Expired");
                                isFailed = true;
                            }
                            else
                            {
                                GME_Custom.GME_Data.queryData que = new GME_Custom.GME_Data.queryData();

                                Dictionary<string, string> dt = que.getBirthdayVoucherData(Connection.applicationLoc.Settings.Database.DataAreaID, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                string[] tempTrxNumber = trxNumber.Split('-');
                                trxNumber = tempTrxNumber[tempTrxNumber.Length - 1];
                                if (trxNumber.Length < 3)
                                {
                                    int loop = 3 - trxNumber.Length;
                                    for (int k = 0; k < loop; k++)
                                        trxNumber = "0" + trxNumber;
                                }

                                //datetime now
                                DateTime dateNow = DateTime.Now;

                                List<voucherDto> listVoucher = new List<voucherDto>();

                                //check min and maximum belanja berdasarkan voucher club tier
                                if (GME_Custom.GME_Propesties.GME_Var.identifierCardType == "LYBC Club")
                                {

                                    decimal minClub = 0;
                                    decimal maxClub = 0;
                                    //CLUB

                                    minClub = Convert.ToDecimal(dt["minclub"]);
                                    maxClub = Convert.ToDecimal(dt["maxclub"]);
                                    percentageDisc = Convert.ToInt32(dt["DiscTierClub"]);

                                    int min = Decimal.Compare(GME_Var.netAmountBirthday, minClub);
                                    int max = Decimal.Compare(GME_Var.netAmountBirthday, maxClub);

                                    if (min < 0)
                                    {
                                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Minimal pembelajaan adalah Rp " + decimal.ToInt32(minClub));
                                        isFailed = true;
                                        this.Close();
                                    }

                                    listVoucher.Add(setVoucher(VoucherIdTxt.Text));
                                    vouchersDto vouchers = setVouchers(listVoucher);
                                    ticket ticket = setTicket("000" + trxNumber, dateNow.ToString("yyyyMMdd"), vouchers);
                                    try
                                    {
                                        string result900017 = integrationService.requestReward900017(GME_Custom.GME_Propesties.GME_Var.identifierCode, application.Shift.StoreId, trxNumber, application.Shift.StaffId, application.Shift.TerminalId, dateNow.ToString("yyyyMMdd"), dateNow.ToString("HHmmss"), ticket);

                                        if (result900017 == "Success")
                                        {
                                            foreach (voucherDto vc in listVoucher)
                                            {
                                                GME_Var.listVouchers.Add(vc);
                                            }

                                            if (min >= 0 && max <= 0)
                                            {
                                                //apply voucher not prorate
                                                //application.RunOperation(PosisOperations.TotalDiscountPercent, percentageDisc);
                                                //BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher berhasil di apply");
                                                GME_Var.isBirthdayNotProrate = true;
                                                GME_Var.isBirthdayProrate = false;
                                                GME_Var.percentageDisc = percentageDisc;
                                            }
                                            else if (max > 0)
                                            {
                                                //jika total belanja > max total belanja voucher
                                                //apply voucher prorate
                                                //hitung akan dilakukan di blank operation
                                                GME_Var.isBirthdayProrate = true;
                                                GME_Var.isBirthdayNotProrate = false;
                                                GME_Var.birthdayAmountDisc = (maxClub * percentageDisc) / 100;
                                            }
                                            GME_Var.isbirthdayvoucherCLUB = true;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Failed to connect to Engage Server");
                                        isFailed = true;
                                    }
                                }
                                else if (GME_Custom.GME_Propesties.GME_Var.identifierCardType == "LYBC Fan")
                                {
                                    decimal minFan = 0;
                                    decimal maxFan = 0;
                                    //FAN

                                    minFan = Convert.ToDecimal(dt["minfan"]);
                                    maxFan = Convert.ToDecimal(dt["maxfan"]);
                                    percentageDisc = Convert.ToInt32(dt["DiscTierFan"]);

                                    int min = Decimal.Compare(GME_Var.netAmountBirthday, minFan);
                                    int max = Decimal.Compare(GME_Var.netAmountBirthday, maxFan);

                                    if (min < 0)
                                    {
                                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Minimal pembelanjaan adalah Rp " + decimal.ToInt32(minFan));
                                        isFailed = true;
                                        this.Close();
                                    }

                                    listVoucher.Add(setVoucher(VoucherIdTxt.Text));
                                    vouchersDto vouchers = setVouchers(listVoucher);
                                    ticket ticket = setTicket("000" + trxNumber, dateNow.ToString("yyyyMMdd"), vouchers);

                                    try
                                    {
                                        string result900017 = integrationService.requestReward900017(GME_Custom.GME_Propesties.GME_Var.identifierCode, application.Shift.StoreId, trxNumber, application.Shift.StaffId, application.Shift.TerminalId, dateNow.ToString("yyyyMMdd"), dateNow.ToString("HHmmss"), ticket);

                                        if (result900017 == "Success")
                                        {
                                            foreach (voucherDto vc in listVoucher)
                                            {
                                                GME_Var.listVouchers.Add(vc);
                                            }
                                            if (min >= 0 && max <= 0)
                                            {
                                                //apply voucher tidak di prorate
                                                //application.RunOperation(PosisOperations.TotalDiscountPercent, percentageDisc);
                                                //BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher berhasil di apply");
                                                GME_Var.isBirthdayNotProrate = true;
                                                GME_Var.isBirthdayProrate = false;
                                                GME_Var.percentageDisc = percentageDisc;
                                            }
                                            else if (max > 0)
                                            {
                                                //jika total belanja > max total belanja voucher
                                                //apply voucher prorate
                                                //hitung akan dilakukan di blank operation
                                                GME_Var.isBirthdayProrate = true;
                                                GME_Var.isBirthdayNotProrate = false;
                                                GME_Var.birthdayAmountDisc = (maxFan * percentageDisc) / 100;


                                            }
                                            GME_Var.isbirthdayvoucherFAN = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        BlankOperations.BlankOperations.ShowMsgBoxInformation("Failed to connect to Engage Server");
                                        isFailed = true;
                                        //this.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher tidak valid / sudah Expired");
                            isFailed = true;
                            //this.Close();
                        }
                    }
                    if (counters == vcResp.Length)
                    {
                        BlankOperations.BlankOperations.ShowMsgBox("Kode voucher yang anda masukan tidak ada");
                        this.Close();
                    }
                }
            }

            if (!isFailed)
            {
                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VoucherIdTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                OKBtn_Click(sender, e);
            }
        }

        private void frmBirthdayVoucher_Load(object sender, EventArgs e)
        {

        }

        private voucherDto setVoucher(string voucherCode)
        {
            voucherDto voucher = new voucherDto();
            voucher.voucherCode = voucherCode;
            return voucher;
        }

        private vouchersDto setVouchers(List<voucherDto> listVoucher)
        {
            vouchersDto vouchers = new vouchersDto();
            vouchers.voucher = new voucherDto[listVoucher.Count];

            for (int i = 0; i < listVoucher.Count; i++)
            {
                vouchers.voucher[i] = listVoucher[i];
            }


            return vouchers;
        }

        private ticket setTicket(string ticketNumber, string ticketDate, vouchersDto vouchers)
        {
            ticket ticket = new ticket();
            ticket.ticketNumber = ticketNumber;
            ticket.ticketDate = ticketDate;
            ticket.vouchers = vouchers;

            return ticket;
        }
    }
}
