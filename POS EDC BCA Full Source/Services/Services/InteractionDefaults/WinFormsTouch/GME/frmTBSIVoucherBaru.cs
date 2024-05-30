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
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Linq;
using LSRetailPosis.Transaction.Line.SaleItem;
using System.Globalization;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    [Export("FrmTBSIVoucherBaru", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmTBSIVoucherBaru : frmTouchBase, IInteractionView
    {
        string tbsiVoucherId = string.Empty;
        decimal total = 0;

        public IApplication application;
        public IPosTransaction posTransaction;

        public frmTBSIVoucherBaru()
        {
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmTBSIVoucherBaru(frmTBSIVoucherBaruConfirm parm)
           : this()
        {
            tbsiVoucherId = parm.tbsiVoucherId;

            this.application = parm.application;
        }

        #region Interface
        /*Interactionview Implementation for Return Result : not implemented*/
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new frmTBSIVoucherBaruConfirm
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

        private void OKBtn_Click(object sender, EventArgs e)
        {           
            if (VoucherIdTxt.Text == "" && DiscountAmountTxt.Text == "")
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Anda belum memasukan kode voucher");
                GME_Var.isContinueVoucherCasPayment = false;
            }
            else
            {
                if (!GME_Method.CheckForInternetConnection())
                {
                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Sedang Offline, Voucher tidak dapat digunakan");
                }
                else
                {
                    Jacksonsoft.WaitWindow.Show(this.DoProgressOK, "Please Wait ...");
                }
            }            
        }

        private void DoProgressOK(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            if (GME_Var.isCashVoucher)
            {
                GME_Var.amountTBSI = Convert.ToDecimal(DiscountAmountTxt.Text);
            }
            else
            {
                posTransaction = GME_Var.welcomePosTransaction;
                LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                queryData data = new queryData();

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
                GME_Var.amountVouchTBSI = Convert.ToDecimal(DiscountAmountTxt.Text);
            }
            GME_Var.isTBSIVoucherBaru = true;
            if (GME_Var.isCashVoucher)
            {
                GME_Var.isContinueVoucherCasPayment = true;
            }

            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VoucherIdTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Jacksonsoft.WaitWindow.Show(this.DoProgressSearch, "Please Wait ...");
            }
        }

        private void DoProgressSearch(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            IntegrationService voucherInt = new IntegrationService();
            vouchersList voucherList = new vouchersList();
            voucherList = voucherInt.lookup(application.Settings.Database.DataAreaID, VoucherIdTxt.Text, application.Shift.StoreId);
            queryData data = new queryData();

            if (voucherList == null)
            {
                BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher id tidak ditemukan");
                if (GME_Var.isCashVoucher)
                {
                    GME_Var.isContinueVoucherCasPayment = false;
                }
            }
            else
            {
                GME_Var.getvoucherTBSIBaru = new string[voucherList.VoucherItems.Count];
                for (int i = 0; i < voucherList.VoucherItems.Count; i++)
                {
                    if (GME_Var.isCash == true)
                    {
                        if (GME_Var.isCashVoucher)
                        {
                            if (voucherList.VoucherItems[i].responseCode == 1)
                            {
                                NumberFormatInfo MyNFI = new NumberFormatInfo();
                                MyNFI.NegativeSign = "-";
                                MyNFI.CurrencyDecimalSeparator = ".";
                                MyNFI.CurrencyGroupSeparator = ",";

                                decimal convertDecimal = decimal.Parse(voucherList.VoucherItems[i].responseMessage, NumberStyles.Currency, MyNFI);
                                total = convertDecimal + total;

                                DiscountAmountTxt.Text = total.ToString();
                                data.insertRedeemVoucher(application.Shift.StoreId, application.Shift.TerminalId, VoucherIdTxt.Text, application.Settings.Database.Connection.ConnectionString);


                                if (GME_Var.voucherTBSIBaru == "" && GME_Var.voucherTBSIBaru.Length > 0)
                                {
                                    GME_Var.voucherTBSIBaru = DiscountAmountTxt.Text;
                                }
                                else
                                {
                                    GME_Var.voucherTBSIBaru = GME_Var.voucherTBSIBaru + ";" + VoucherIdTxt.Text;
                                }
                                VoucherIdTxt.Text = string.Empty;
                                GME_Var.isContinueVoucherCasPayment = true;
                            }
                            else
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation(voucherList.VoucherItems[i].responseMessage);
                                if (GME_Var.isCashVoucher)
                                {
                                    GME_Var.isContinueVoucherCasPayment = false;
                                }
                                this.Close();
                            }
                        }
                        else
                        {
                            this.Close();
                        }
                    }

                    if (GME_Var.isDisc == true)
                    {
                        if (GME_Var.isDiscVoucher == true)
                        {
                            if (voucherList.VoucherItems[i].responseCode == 1)
                            {
                                decimal convertDecimal = Convert.ToDecimal(voucherList.VoucherItems[i].responseMessage);
                                total = convertDecimal + total;

                                DiscountAmountTxt.Text = total.ToString("#,##0.00");
                                data.insertRedeemVoucher(application.Shift.StoreId, application.Shift.TerminalId, VoucherIdTxt.Text, application.Settings.Database.Connection.ConnectionString);

                                if (GME_Var.voucherTBSIBaru == "" && GME_Var.voucherTBSIBaru.Length > 0)
                                {
                                    GME_Var.voucherTBSIBaru = DiscountAmountTxt.Text;
                                }
                                else
                                {
                                    GME_Var.voucherTBSIBaru = GME_Var.voucherTBSIBaru + ";" + VoucherIdTxt.Text;
                                }
                                VoucherIdTxt.Text = string.Empty;
                            }
                            else
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation(voucherList.VoucherItems[i].responseMessage);
                                this.Close();
                            }
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
