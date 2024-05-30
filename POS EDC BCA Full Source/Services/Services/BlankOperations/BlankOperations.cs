 /*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME;
using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.Interaction;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using LSRetailPosis.Transaction;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Printing;
using OnBarcode.Barcode;
using System;
using System.Text.RegularExpressions;
using LSRetailPosis.POSProcesses;
using GME_Custom.GME_EngageFALWSServices;
using System.Collections.Generic;
using Jacksonsoft;
using System.Threading.Tasks;
using LSRetailPosis.Transaction.Line.SaleItem;
using System.Threading;
using LSRetailPosis.DataAccess;
using System.Linq;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{

    [Export(typeof(IBlankOperations))]
    public sealed class BlankOperations : IBlankOperations
    {
        [Import]
        public IApplication Application { get; set; }
        //private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        static AutoResetEvent autoEventBCA = new AutoResetEvent(false);
        static AutoResetEvent autoEventMDR = new AutoResetEvent(false);
        private bool _isReceived { get; set; }
        static string _LastResponBCA { get; set; }
        static string _LastResponMDR { get; set; }
        static bool isEDCSettlementBCA { get; set; }
        static bool isEDCSettlementMDR { get; set; }
        // Get all text through the Translation function in the ApplicationLocalizer
        // TextID's for BlankOperations are reserved at 50700 - 50999

        #region IBlankOperations Members
        /// <summary>
        /// Displays an alert message according operation id passed.
        /// </summary>
        /// <param name="operationInfo"></param>
        /// <param name="posTransaction"></param>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public void BlankOperation(IBlankOperationInfo operationInfo, IPosTransaction posTransaction)
        {
            IntegrationService integrationService = new IntegrationService();
            #region comment
            // This country check can be removed when customizing the BlankOperations service.
            //if (Functions.CountryRegion == SupportedCountryRegion.BR ||
            //    Functions.CountryRegion == SupportedCountryRegion.HU ||
            //    Functions.CountryRegion == SupportedCountryRegion.RU)
            //{
            //    if (Application.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            //    {
            //        Application.Services.Peripherals.FiscalPrinter.BlankOperations(operationInfo, posTransaction);
            //    }
            //    return;
            //}

            //StringBuilder comment = new StringBuilder(128);
            //comment.AppendFormat(ApplicationLocalizer.Language.Translate(50700), operationInfo.OperationId);
            //comment.AppendLine();
            //comment.AppendFormat(ApplicationLocalizer.Language.Translate(50701), operationInfo.Parameter);
            //comment.AppendLine();
            //comment.Append(ApplicationLocalizer.Language.Translate(50702));

            //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(comment.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error))
            //{
            //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            //}

            //// Set this property to true when your operation is handled
            //operationInfo.OperationHandled = true;

            // Other examples:

            // Add an item to the transaction
            // Application.RunOperation(PosisOperations.ItemSale, "<ItemId>");

            // Logoff
            // Application.RunOperation(PosisOperations.LogOff, null);

            //switch ((operationInfo.OperationId).ToUpperInvariant().Replace(" ", string.Empty))
            //{
            //    #region Test
            //    case "FormTest":  //Create Planned Order                    
            //        GME_FormCaller.GME_FormTest(this.Application);
            //        break;
            //    default:
            //        //default, just echo the operation number and parameter value 
            //        break;
            //        #endregion
            //}
            #endregion

            if (operationInfo.OperationId == "FormBonManual")
            {
                string statusWorker;

                GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();
                statusWorker = data.getWorkerStatus(ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalOperator.OperatorId, Application.Settings.Database.Connection.ConnectionString);

                if (statusWorker == "Manager")
                    GME_FormCaller.GME_FormBonManual(this.Application, posTransaction.StoreId);
                else
                    ShowMsgBoxInformation("Menu bon manual hanya dapat digunakan oleh store leader");
            }          
            //Add by Adhi (Form Sales Person)
            else if (operationInfo.OperationId == "formSalesPerson")
            {
                GME_FormCaller.GME_FormSalesPerson(this.Application);
            }
            else if (operationInfo.OperationId == "MallVoucherPay")
            {
                GME_Var.isMallVoucherPay = true;

                this.Application.RunOperation(PosisOperations.PayCash, "10000000");
            }
            //// ADD by Maria  -- member info
            else if (operationInfo.OperationId == "MemberInfo")
            {
                System.Diagnostics.Process.Start(GME_Custom.GME_Propesties.GME_Var.memberInfoLink);
            }
            //// ADD by Maria  -- Unprinted Voucher
            else if (operationInfo.OperationId == "FrmUnprintVoucher")
            {
                if (!GME_Var._EngageHOStatus)
                {
                    BlankOperations.ShowMsgBox("Sedang offline, tidak dapat melakukan unprint");
                }
                else
                {
                    GME_FormCaller.GME_FormUnprintedVoucher(this.Application);
                }
            }
            else if (operationInfo.OperationId == "FrmUpdateCustomer")
            {
                if (!GME_Var._EngageHOStatus)
                {
                    BlankOperations.ShowMsgBox("Sedang offline, tidak dapat melakukan update member");
                }
                else
                {
                    GME_FormCaller.GME_FormUpdateCustomer(this.Application, posTransaction);
                }
            }
            else if (operationInfo.OperationId == "FrmCardReplacement")
            {
                GME_FormCaller.GME_FormCardReplacement(this.Application, GME_Var.customerPosTransaction, string.Empty);
            }
            else if (operationInfo.OperationId == "PayBCAOnline")
            {
                if (GME_Var.retailTransGlobal != null)
                {
                    getpoint();

                    PointAndDonation();

                    if (GME_Var.isPoint)
                    {
                        { goto outtoendmethod; }
                    }

                    GME_Var.transTypeBCA = "01";
                    GME_Var.isEDCBCA = true;
                    ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                    GME_Var.approvalCodeBCA = string.Empty;
                    RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                    int totalAmount = decimal.ToInt32(retailTransaction.TransSalePmtDiff);

                    try
                    {
                        BCAOnline.openPort("COM4");
                    }
                    catch (Exception e)
                    {

                    }

                    if (GME_Var.serialBCA.IsOpen)
                    {
                        //autoEventBCA = new AutoResetEvent(false);
                        BCAOnline.sendData(totalAmount.ToString(), "01", Connection.applicationLoc, posTransaction);
                        _isReceived = false;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
                        autoEventBCA.WaitOne();

                        if (GME_Var.transTypeBCA == "01")
                        {
                            if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeBCA == "*0")
                            {
                                BlankOperations.ShowMsgBoxInformation("Pembayaran EDC BCA berhasil");

                            }
                            else if (GME_Var.respCodeBCA == "54")
                            {
                                BlankOperations.ShowMsgBox("Decline Expired Card");
                                GME_Var.isEDCBCA = false;

                            }
                            else if (GME_Var.respCodeBCA == "55")
                            {
                                BlankOperations.ShowMsgBox("Decline Incorrect PIN");
                                GME_Var.isEDCBCA = false;

                            }
                            else if (GME_Var.respCodeBCA == "P2")
                            {
                                BlankOperations.ShowMsgBox("Read Card Error");
                                GME_Var.isEDCBCA = false;

                            }
                            else if (GME_Var.respCodeBCA == "P3")
                            {
                                BlankOperations.ShowMsgBox("User press Cancel on EDC");
                                GME_Var.isEDCBCA = false;

                            }
                            else if (GME_Var.respCodeBCA == "Z3")
                            {
                                BlankOperations.ShowMsgBox("EMV Card Decline");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "CE")
                            {
                                BlankOperations.ShowMsgBox("Connection Error/Line Busy");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "TO")
                            {
                                BlankOperations.ShowMsgBox("Connection Timeout");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "PT")
                            {
                                BlankOperations.ShowMsgBox("EDC Problem / EDC perlu di settlement");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "PS")
                            {
                                BlankOperations.ShowMsgBox("Settlement Failed");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "aa")
                            {
                                BlankOperations.ShowMsgBox("Decline (aa represent two digit alphanumeric value from EDC)");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "S2")
                            {
                                BlankOperations.ShowMsgBox("TRANSAKSI GAGAL");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "S3")
                            {
                                BlankOperations.ShowMsgBox("TXN BLM DIPROSES MINTA SCAN QR");
                                GME_Var.isEDCBCA = false;


                            }
                            else if (GME_Var.respCodeBCA == "S4")
                            {
                                BlankOperations.ShowMsgBox("TXN EXPIRED ULANGI TRANSAKSI");
                                GME_Var.isEDCBCA = false;

                            }
                            else if (GME_Var.respCodeBCA == "TN")
                            {
                                BlankOperations.ShowMsgBox("Topup Tunai Not Ready");
                                GME_Var.isEDCBCA = false;

                            }
                        }

                        autoEventBCA.Reset();


                        //ThreadPool.QueueUserWorkItem(new WaitCallback(DataReceived), autoEventBCA);
                        //autoEventBCA.WaitOne();
                        // ThreadPool.QueueUserWorkItem(_ => receiveThreadFunc());
                        // ThreadPool.QueueUserWorkItem(_ => DataReceived());
                        if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeBCA == "*0")
                            Application.RunOperation(PosisOperations.PayCard, string.Empty);                        
                    }
                    else
                    {
                        BlankOperations.ShowMsgBoxInformation("Koneksi ke EDC BCA terputus. Silahkan Hubungi Help Desk dan lakukan pembayaran dengan EDC offline");
                    }

                    outtoendmethod:;
                    GME_Var.isPoint = false;
                }
            }
            else if (operationInfo.OperationId == "SettlementBCAOnline")
            {
                if (isEDCSettlementBCA) { BlankOperations.ShowMsgBox("Settlement menunggu respon dari EDC BCA"); goto outthisfunc; }

                if (BlankOperations.ShowMsgDialog("Apakah anda mau melakukan Settlement EDC BCA ?") == "OK")
                {
                    isEDCSettlementBCA = true;
                    ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                    GME_Var.transTypeBCA = "10";
                    GME_Var.settlementBCAOnline = true;
                    try
                    {
                        BCAOnline.openPort("COM4");
                    }
                    catch (Exception e)
                    {

                    }

                    if (GME_Var.serialBCA.IsOpen)
                    {
                        //autoEventBCA = new AutoResetEvent(false);
                        BCAOnline.sendData("000000000000", "10", this.Application, posTransaction);

                        _isReceived = false;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
                        autoEventBCA.WaitOne();

                        if (GME_Var.transTypeBCA == "10")
                        {
                            if (GME_Var.respCodeBCA == "00")
                            {
                                BlankOperations.ShowMsgBoxInformation("Settlement berhasil");

                            }
                            else if (GME_Var.respCodeBCA == "54")
                            {
                                BlankOperations.ShowMsgBox("Decline Expired Card");

                            }
                            else if (GME_Var.respCodeBCA == "55")
                            {
                                BlankOperations.ShowMsgBox("Decline Incorrect PIN");

                            }
                            else if (GME_Var.respCodeBCA == "P2")
                            {
                                BlankOperations.ShowMsgBox("Read Card Error");

                            }
                            else if (GME_Var.respCodeBCA == "P3")
                            {
                                BlankOperations.ShowMsgBox("User press Cancel on EDC");

                            }
                            else if (GME_Var.respCodeBCA == "Z3")
                            {
                                BlankOperations.ShowMsgBox("EMV Card Decline");

                            }
                            else if (GME_Var.respCodeBCA == "CE")
                            {
                                BlankOperations.ShowMsgBox("Connection Error/Line Busy");

                            }
                            else if (GME_Var.respCodeBCA == "TO")
                            {
                                BlankOperations.ShowMsgBox("Connection Timeout");

                            }
                            else if (GME_Var.respCodeBCA == "PT")
                            {
                                BlankOperations.ShowMsgBox("TIdak ada transaksi yang harus di settlement");

                            }
                            else if (GME_Var.respCodeBCA == "PS")
                            {
                                BlankOperations.ShowMsgBox("Settlement Failed");

                            }
                            else if (GME_Var.respCodeBCA == "aa")
                            {
                                BlankOperations.ShowMsgBox("Decline (aa represent two digit alphanumeric value from EDC)");

                            }
                            else if (GME_Var.respCodeBCA == "S2")
                            {
                                BlankOperations.ShowMsgBox("TRANSAKSI GAGAL");

                            }
                            else if (GME_Var.respCodeBCA == "S3")
                            {
                                BlankOperations.ShowMsgBox("TXN BLM DIPROSES MINTA SCAN QR");

                            }
                            else if (GME_Var.respCodeBCA == "S4")
                            {
                                BlankOperations.ShowMsgBox("TXN EXPIRED ULANGI TRANSAKSI");

                            }
                            else if (GME_Var.respCodeBCA == "TN")
                            {
                                BlankOperations.ShowMsgBox("Topup Tunai Not Ready");

                            }
                        }

                        autoEventBCA.Reset();

                    }
                    else
                    {
                        ShowMsgBoxInformation("Koneksi port EDC BCA terputus, Silahkan Hubungi Help Desk " + System.Environment.NewLine +
                                "Lakukan settlement di EDC BCA secara manual");
                        isEDCSettlementBCA = false;
                    }
                }
                outthisfunc:;


            }
            else if (operationInfo.OperationId == "PayEDCOffline")
            {
                getpoint();

                PointAndDonation();

                GME_Var.isEDCOffline = true;

                if (operationInfo.Parameter == "EDC BCA")
                {
                    GME_Var.isEDCBCAOffline = true;
                }
                else if (operationInfo.Parameter == "EDC Mandiri")
                {
                    GME_Var.isEDCMandiriOffline = true;
                }

                Application.RunOperation(PosisOperations.PayCard, string.Empty);
            }
            else if (operationInfo.OperationId == "PayMandiriOffline")
            {
                getpoint();

                PointAndDonation();

                GME_Var.isEDCMandiriOffline = true;
                GME_Var.isEDCOffline = true;
                Application.RunOperation(PosisOperations.PayCard, string.Empty);
            }
            else if (operationInfo.OperationId == "SettlementMandiriOnline")
            {
                if (isEDCSettlementMDR) { BlankOperations.ShowMsgBox("Settlement menunggu respon dari EDC Mandiri"); goto outthisfunc; }
                if (BlankOperations.ShowMsgDialog("Apakah anda mau melakukan Settlement EDC Mandiri ?") == "OK")
                {
                    isEDCSettlementMDR = true;
                    ElectronicDataCaptureMandiri MandiriOnline = new ElectronicDataCaptureMandiri();

                    GME_Var.transTypeMandiri = "30";
                    GME_Var.settlementMandiriOnline = true;

                    try
                    {
                        MandiriOnline.openPort("COM5");
                    }
                    catch (Exception e)
                    {

                    }
                    if (GME_Var.serialMandiri.IsOpen)
                    {
                        //autoEventMDR = new AutoResetEvent(false);
                        MandiriOnline.Send_Data("000000000000", "30", this.Application);

                        _isReceived = false;
                        //ThreadPool.QueueUserWorkItem(_ => receiveThreadMandiri());
                        ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadMandiri), autoEventMDR);
                        autoEventMDR.WaitOne();

                        if (GME_Var.transTypeMandiri == "30")
                        {
                            if (GME_Var.respCodeMandiri == "00")
                            {
                                BlankOperations.ShowMsgBoxInformation("Settlement berhasil");
                            }
                            else if (GME_Var.respCodeMandiri == "ER")
                            {
                                BlankOperations.ShowMsgBoxInformation("Settlement Gagal ");
                                GME_Var.isEDCMandiri = false;
                            }
                        }

                        autoEventMDR.Reset();

                    }
                    else
                    {
                        BlankOperations.ShowMsgBoxInformation("Koneksi port EDC Mandiri terputus, Silahkan Hubungi Help Desk " + System.Environment.NewLine +
                                "Lakukan settlement di EDC Mandiri secara manual");
                        isEDCSettlementMDR = false;
                    }
                }
                outthisfunc:;
            }
            else if (operationInfo.OperationId == "PayMandiriOnline")
            {

                if (GME_Var.retailTransGlobal != null)
                {
                    getpoint();

                    PointAndDonation();

                    if (GME_Var.isPoint)
                    {
                        { goto outtoendmethod; }
                    }

                    GME_Var.isEDCMandiri = true;
                    GME_Var.transTypeMandiri = "10";
                    ElectronicDataCaptureMandiri MandiriOnline = new ElectronicDataCaptureMandiri();
                    GME_Var.approvalCodeMandiri = string.Empty;
                    RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                    int totalAmount = decimal.ToInt32(retailTransaction.TransSalePmtDiff);

                    try
                    {
                        MandiriOnline.openPort("COM5");
                    }
                    catch (Exception e)
                    {

                    }
                    if (GME_Var.serialMandiri.IsOpen)
                    {
                        //autoEventMDR = new AutoResetEvent(false);
                        MandiriOnline.Send_Data(totalAmount.ToString(), "10", Connection.applicationLoc);

                        _isReceived = false;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadMandiri), autoEventMDR);
                        autoEventMDR.WaitOne();

                        if (GME_Var.transTypeMandiri == "10" || GME_Var.transTypeMandiri == "SALE")
                        {
                            if (GME_Var.respCodeMandiri == "00")
                            {
                                BlankOperations.ShowMsgBoxInformation("Pembayaran EDC Mandiri berhasil");
                                Application.RunOperation(PosisOperations.PayCard, string.Empty);
                            }
                            else if (GME_Var.respCodeMandiri == "ER")
                            {
                                BlankOperations.ShowMsgBoxInformation("Terdapat masalah pembayaran menggunakan EDC Mandiri");
                                GME_Var.isEDCMandiri = false;
                            }
                        }

                        autoEventMDR.Reset();
                    }
                    else
                    {
                        BlankOperations.ShowMsgBoxInformation("Koneksi ke EDC Mandiri terputus. Silahkan Hubungi Help Desk dan lakukan pembayaran dengan EDC offline");
                    }

                    outtoendmethod:;
                    GME_Var.isPoint = false;
                }

            }
            else if (operationInfo.OperationId == "CheckEDCBCAConnection")
            {
                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                BCAOnline.checkConnectionPortBCA("COM4");
            }
            else if (operationInfo.OperationId == "CheckEDCMandiriConnection")
            {
                ElectronicDataCaptureMandiri MandiriOnline = new ElectronicDataCaptureMandiri();

                MandiriOnline.checkConnectionPortMandiri("COM5");
            }
            else if (operationInfo.OperationId == "payCashVoucher")
            {
                if (GME_Var.retailTransGlobal != null)
                {
                    GME_Var.isTBSIVoucherBaru = false;
                    GME_Var.isCash = true;
                    GME_Var.isDisc = false;
                    GME_Var.isAktifVoucher = false;
                    RetailTransaction retailTrans = posTransaction as RetailTransaction;

                    if (!GME_Var._EngageHOStatus)
                    {
                        BlankOperations.ShowMsgBox("Sedang offline, voucher tidak dapat digunakan");
                    }
                    else
                    {
                        PointAndDonation();

                        if (GME_Var.isPoint)
                        {
                            { goto outtoendmethod; }
                        }

                        GME_Var.isCashVoucher = true;
                        GME_FormCaller.GME_FormTBSIVoucherBaru(this.Application, string.Empty);

                        if (GME_Var.isCashVoucher == false)
                        {
                            BlankOperations.ShowMsgBoxInformation("Voucher yang anda masukan adalah Diskon Voucher");
                        }
                        else
                        {
                            if (GME_Var.isContinueVoucherCasPayment)
                            {
                                PayCash paycash = new PayCash(false, "2");
                                paycash.OperationID = PosisOperations.PayCash;
                                paycash.OperationInfo = new OperationInfo();
                                paycash.POSTransaction = (PosTransaction)posTransaction;

                                if (GME_Var.amountTBSI > retailTrans.AmountDue)
                                {
                                    BlankOperations.ShowMsgBox("Total belanja harus lebih besar atau sama dengan total voucher");
                                }
                                else
                                {
                                    paycash.RunOperation();
                                }
                            }
                        }

                        outtoendmethod:;
                        GME_Var.isPoint = false;
                    }
                }
                
            }
            else if (operationInfo.OperationId == "PartialPaymentEDC")
            {

                PayCash paycash = null;
                if (GME_Var.isEDCBCAOffline || GME_Var.isEDCBCA)
                {
                    paycash = new PayCash(false, "16");
                }
                else if (GME_Var.isEDCMandiriOffline || GME_Var.isEDCMandiri)
                {
                    paycash = new PayCash(false, "17");
                }

                if (paycash != null)
                {
                    paycash.OperationID = PosisOperations.PayCash;
                    paycash.OperationInfo = new OperationInfo();
                    paycash.POSTransaction = (PosTransaction)posTransaction;
                    //pay.Amount = ((RetailTransaction)posTransaction).NetAmountWithTax;
                    // paycash.Amount = GME_Var.paycardofflineamount;
                    GME_Var.isEDCBCAOffline = false;
                    GME_Var.isEDCMandiriOffline = false;
                    GME_Var.isEDCBCA = false;
                    GME_Var.isEDCMandiri = false;
                    GME_Var.partialPaymentEDC = true;
                    paycash.RunOperation();
                }

            }
            else if (operationInfo.OperationId == "testSuspend")
            {
                SuspendRetrieveData suspendRetrieveData = new SuspendRetrieveData(Connection.applicationLoc.Settings.Database.Connection, ApplicationSettings.Database.DATAAREAID);
                queryData queryData = new queryData();

                String channelid = queryData.getChannelStore(ApplicationSettings.Terminal.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                DataTable dataTable = suspendRetrieveData.GetSuspendedTransactionList(Connection.applicationLoc.Settings.Database.Connection, Convert.ToInt64(channelid));

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        BlankOperations.ShowMsgBoxInformation("Tidak dapat melakukan closing karena terdapat " + dataTable.Rows.Count + " transaksi suspend ");
                    }
                    else
                    {
                        Application.RunOperation(PosisOperations.BlindCloseShift, string.Empty);
                    }
                }

            }
            else if (operationInfo.OperationId == "functionPayCard")
            {
                if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeMandiri == "00")
                    Application.RunOperation(PosisOperations.PayCard, string.Empty);
            }
            //Add by Adhi (Form TBSI Voucher Baru)
            else if (operationInfo.OperationId == "FrmTBSIVoucherBaru")
            {
                GME_Var.isDisc = true;
                GME_Var.isCash = false;
                GME_Var.isAktifVoucher = false;
                RetailTransaction retailTrans = posTransaction as RetailTransaction;

                if (!GME_Var._EngageHOStatus)
                {
                    BlankOperations.ShowMsgBox("Sedang offline, voucher tidak dapat digunakan");
                }
                else if (retailTrans == null)
                {
                    BlankOperations.ShowMsgBox("Masukkan product terlebih dahulu");
                }
                else
                {
                    Point();

                    if (GME_Var.isPoint)
                    {
                        { goto outtoendmethod; }
                    }

                    GME_Var.isDiscVoucher = true;
                    GME_FormCaller.GME_FormTBSIVoucherBaru(this.Application, string.Empty);

                    if (GME_Var.isDiscVoucher == false)
                    {
                        BlankOperations.ShowMsgBoxInformation("Voucher yang anda masukan adalah Cash Voucher");
                    }
                    else
                    {
                        if (GME_Custom.GME_Propesties.GME_Var.isTBSIVoucherBaru == true)
                        {
                            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();
                            decimal amountdue = 0;
                            foreach (var lineItem in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (lineItem.ItemId == GME_Var.itemVouchTBSI[i] && lineItem.Voided == false)
                                    {
                                        amountdue = amountdue + lineItem.NetAmount;
                                    }
                                }
                            }

                            foreach (var lineItem in retailTrans.SaleItems)
                            {
                                LSRetailPosis.Transaction.Line.Discount.DiscountItem disc;
                                disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (lineItem.ItemId == GME_Var.itemVouchTBSI[i] && lineItem.Voided == false)
                                    {
                                        decimal prorate = 0;
                                        prorate = (lineItem.NetAmount / amountdue) * GME_Var.amountVouchTBSI;

                                        decimal potongan = prorate / lineItem.Quantity;
                                        disc.Amount = potongan;
                                        lineItem.DiscountLines.AddFirst(disc);
                                        lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                    }
                                }
                                lineItem.CalculateLine();
                                retailTrans.CalcTotals();
                                this.Application.Services.Tax.CalculateTax(retailTrans);
                                GME_Var.isVoucherApplied = true;
                                GME_Var.isVMSused = true;
                                GME_Var.DiscVoucher = 1;
                            }
                        }
                    }
                    outtoendmethod:;
                    GME_Var.isPoint = false;
                }
            }
            //Add by Adhi (Form TBSI Voucher Lama)
            else if (operationInfo.OperationId == "FrmTBSIVoucherLama")
            {
                GME_Var.isAktifVoucher = false;
                RetailTransaction retailTrans = posTransaction as RetailTransaction;

                if (retailTrans == null)
                {
                    BlankOperations.ShowMsgBox("Masukkan product terlebih dahulu");
                }
                else
                {
                    Point();

                    if (GME_Var.isPoint)
                    {
                        { goto outtoendmethod; }
                    }

                    GME_FormCaller.GME_FormTBSIVoucherLama(this.Application, string.Empty);

                    if (GME_Custom.GME_Propesties.GME_Var.isTBSIVoucherLama == true)
                    {
                        GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

                        decimal amountdue = 0;
                        foreach (var lineItem in retailTrans.SaleItems)
                        {
                            for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                            {
                                if (lineItem.ItemId == GME_Var.itemVouchTBSI[i] && lineItem.Voided == false)
                                {
                                    amountdue = amountdue + lineItem.NetAmount;
                                }
                            }
                        }

                        foreach (var lineItem in retailTrans.SaleItems)
                        {
                            LSRetailPosis.Transaction.Line.Discount.DiscountItem disc;
                            disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                            for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                            {
                                if (lineItem.ItemId == GME_Var.itemVouchTBSI[i] && lineItem.Voided == false)
                                {
                                    decimal prorate = 0;
                                    prorate = (lineItem.NetAmount / amountdue) * GME_Var.amountVouchTBSI;

                                    decimal potongan = prorate / lineItem.Quantity;
                                    disc.Amount = potongan;
                                    lineItem.DiscountLines.AddFirst(disc);
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                }
                            }
                            lineItem.CalculateLine();
                            retailTrans.CalcTotals();
                            this.Application.Services.Tax.CalculateTax(retailTrans);
                            GME_Var.isVoucherApplied = true;
                            GME_Var.DiscVoucher = 1;
                        }
                    }
                    outtoendmethod:;
                    GME_Var.isPoint = false;
                }
            }
            //Add by Adhi (Form Welcome Voucher 50)
            else if (operationInfo.OperationId == "FrmWelcomeVoucher50")
            {
                GME_Var.wlcmvouchamount = 0;
                RetailTransaction retailTrans = posTransaction as RetailTransaction;

                if (!GME_Var._EngageHOStatus)
                {
                    BlankOperations.ShowMsgBox("Sedang offline, voucher tidak dapat digunakan");
                }
                else if(GME_Var.iswelcomevoucher100 == true)
                {
                    BlankOperations.ShowMsgBox("Hanya dapat menggunakan 1 jenis welcome voucher");
                }
                else
                {
                    if (retailTrans == null)
                    {
                        BlankOperations.ShowMsgBox("Masukkan product terlebih dahulu");
                    }
                    else
                    {
                        if (GME_Var.personId == 0)
                        {
                            BlankOperations.ShowMsgBox("Masukan customer terlebih dahulu");
                        }
                        else
                        {
                            Point();

                            if (GME_Var.isPoint)
                            {
                                { goto outtoendmethod; }
                            }

                            GME_Var.wlcmvouchamount = 0;
                            GME_Custom.GME_Propesties.GME_Var.iswelcomevoucher50 = true;
                            GME_FormCaller.GME_FormWelcomeVoucher(this.Application, string.Empty);

                            GME_Custom.GME_Data.queryData data = new GME_Custom.GME_Data.queryData();

                            if (GME_Var.iswelcomevoucher50 == true)
                            {
                                foreach (var lineItem in retailTrans.SaleItems)
                                {
                                    LSRetailPosis.Transaction.Line.Discount.DiscountItem disc;
                                    disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                                    var displaynumberwlcmvouch = GME_Var.displaynumberWelcomeVouch;

                                    for (int i = 0; i < displaynumberwlcmvouch.Length; i++)
                                    {
                                        if (lineItem.ItemId == displaynumberwlcmvouch[i])
                                        {
                                            if (lineItem.NetAmount != 0 && lineItem.Voided == false && GME_Var.wlcmvouchamount != 0)
                                            {
                                                decimal prorate = 0;
                                                prorate = (lineItem.NetAmount / GME_Custom.GME_Propesties.GME_Var.totalbelanjakategori) * GME_Var.wlcmvouchamount;

                                                decimal potongan = prorate / lineItem.Quantity;
                                                disc.Amount = potongan;
                                                lineItem.DiscountLines.AddFirst(disc);
                                                lineItem.Comment = lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                            }
                                        }
                                    }
                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    this.Application.Services.Tax.CalculateTax(retailTrans);
                                }
                                GME_Custom.GME_Propesties.GME_Var.totalbelanjakategori = 0;
                                GME_Var.isVoucherApplied = true;
                                GME_Var.DiscVoucher = 1;

                                //UPDATE ENGAGE
                                List<voucherDto> listVoucher = new List<voucherDto>();

                                listVoucher.Add(setVoucher(GME_Var.voucherdipake));
                                vouchersDto vouchers = setVouchers(listVoucher);

                                foreach (voucherDto vc in listVoucher)
                                {
                                    GME_Var.listVouchers.Add(vc);
                                }
                            }
                            outtoendmethod:;
                            GME_Var.isPoint = false;
                        }
                    }
                }
            }
            //Add by Adhi (Form Welcome Voucher 100)
            else if (operationInfo.OperationId == "FrmWelcomeVoucher100")
            {
                GME_Var.wlcmvouchamount = 0;
                RetailTransaction retailTrans = posTransaction as RetailTransaction;

                if (!GME_Var._EngageHOStatus)
                {
                    BlankOperations.ShowMsgBox("Sedang offline, voucher tidak dapat digunakan");
                }
                else if (GME_Var.iswelcomevoucher50 == true)
                {
                    BlankOperations.ShowMsgBox("Hanya dapat menggunakan 1 jenis welcome voucher");
                }
                else
                {
                    if (retailTrans == null)
                    {
                        BlankOperations.ShowMsgBox("Masukkan product terlebih dahulu");
                    }
                    else
                    {
                        if (GME_Var.personId == 0)
                        {
                            BlankOperations.ShowMsgBox("Masukan customer terlebih dahulu");
                        }
                        else
                        {
                            Point();
                            GME_FormCaller.GME_FormWelcomeVoucher(this.Application, string.Empty);

                            if (GME_Var.iswelcomevoucher100 == true)
                            {
                                foreach (var lineItem in retailTrans.SaleItems)
                                {
                                    LSRetailPosis.Transaction.Line.Discount.DiscountItem disc;
                                    disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                                    var displaynumberwlcmvouch = GME_Var.displaynumberWelcomeVouch;

                                    for (int i = 0; i < displaynumberwlcmvouch.Length; i++)
                                    {
                                        if (lineItem.ItemId == displaynumberwlcmvouch[i])
                                        {
                                            if (lineItem.NetAmount != 0 && lineItem.Voided == false && GME_Var.wlcmvouchamount != 0)
                                            {
                                                decimal prorate = 0;
                                                prorate = (lineItem.NetAmount / GME_Custom.GME_Propesties.GME_Var.totalbelanjakategori) * GME_Var.wlcmvouchamount;

                                                decimal potongan = prorate / lineItem.Quantity;
                                                disc.Amount = potongan;
                                                lineItem.DiscountLines.AddFirst(disc);
                                                lineItem.Comment = lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                            }
                                        }
                                    }
                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    this.Application.Services.Tax.CalculateTax(retailTrans);
                                }
                                GME_Custom.GME_Propesties.GME_Var.totalbelanjakategori = 0;
                                GME_Var.isVoucherApplied = true;
                                GME_Var.DiscVoucher = 1;

                                //UPDATE ENGAGE
                                List<voucherDto> listVoucher = new List<voucherDto>();

                                listVoucher.Add(setVoucher(GME_Var.voucherdipake));
                                vouchersDto vouchers = setVouchers(listVoucher);

                                foreach (voucherDto vc in listVoucher)
                                {
                                    GME_Var.listVouchers.Add(vc);
                                }
                            }
                        }
                    }
                }
            }
            else if (operationInfo.OperationId == "FrmBirthdayVoucher")
            {
                LSRetailPosis.Transaction.RetailTransaction retailTrans = posTransaction as LSRetailPosis.Transaction.RetailTransaction;

                if (!GME_Var._EngageHOStatus)
                {
                    BlankOperations.ShowMsgBox("Sedang offline, voucher tidak dapat digunakan");
                }
                else if(retailTrans == null)
                {
                    BlankOperations.ShowMsgBox("Masukkan product terlebih dahulu");
                }
                else
                {
                    if (GME_Var.personId == 0)
                    {
                        BlankOperations.ShowMsgBox("Masukan customer terlebih dahulu");
                    }
                    else
                    {
                        Point();

                        if (GME_Var.isPoint)
                        {
                            { goto outtoendmethod; }
                        }

                        GME_FormCaller.GME_FormBirthdayVoucher(this.Application, string.Empty);

                        if (GME_Var.isBirthdayNotProrate == true)
                        {
                            LSRetailPosis.Transaction.Line.Discount.DiscountItem disc;
                            disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                            foreach (var val in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (val.ItemId == GME_Var.itemVouchTBSI[i] && val.Voided == false)
                                    {
                                        disc.Percentage = GME_Var.percentageDisc;

                                        val.DiscountLines.AddFirst(disc);
                                        val.Comment = posTransaction.OperatorId + System.Environment.NewLine;

                                        val.CalculateLine();
                                        retailTrans.CalcTotals();
                                        this.Application.Services.Tax.CalculateTax(retailTrans);
                                    }
                                }
                            }
                            GME_Var.isbirthdayvoucher = true;
                            GME_Var.isVoucherAppliedforBirthday = true;
                            GME_Var.DiscVoucher = 1;
                        }

                        //check var global jika ada maka prorate?
                        if (GME_Var.isBirthdayProrate == true && GME_Var.birthdayAmountDisc != 0)
                        {
                            decimal potongan = 0;
                            LSRetailPosis.Transaction.Line.Discount.DiscountItem disc;
                            disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                            foreach (var val in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (val.ItemId == GME_Var.itemVouchTBSI[i] && val.Voided == false)
                                    {
                                        potongan = (val.NetAmount * GME_Var.birthdayAmountDisc) / GME_Var.netAmountBirthday;
                                        disc.Amount = decimal.Round(potongan) / val.Quantity;

                                        val.DiscountLines.AddFirst(disc);
                                        val.Comment = posTransaction.OperatorId + System.Environment.NewLine;

                                        val.CalculateLine();
                                        retailTrans.CalcTotals();
                                        this.Application.Services.Tax.CalculateTax(retailTrans);
                                    }
                                }
                            }
                            GME_Var.isbirthdayvoucher = true;
                            GME_Var.isVoucherAppliedforBirthday = true;
                            GME_Var.DiscVoucher = 1;
                        }
                        outtoendmethod:;
                        GME_Var.isPoint = false;
                    }
                }
            }
            else if (operationInfo.OperationId == "TenderDeclaration")
            {
                Application.RunOperation(PosisOperations.TenderDeclaration, string.Empty);
            }

            void DoProgressSettlementBCA(object sender, Jacksonsoft.WaitWindowEventArgs e)
            {
                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                GME_Var.transTypeBCA = "10";
                GME_Var.settlementBCAOnline = true;

                BCAOnline.sendData("000000000000", "10", this.Application, posTransaction);
                System.Threading.Thread.Sleep(35000);
            }

            void DoProgressSettlementMandiri(object sender, Jacksonsoft.WaitWindowEventArgs e)
            {
                ElectronicDataCaptureMandiri MandiriOnline = new ElectronicDataCaptureMandiri();

                GME_Var.transTypeMandiri = "30";
                GME_Var.settlementMandiriOnline = true;

                MandiriOnline.Send_Data("000000000000", "30", this.Application);
                System.Threading.Thread.Sleep(35000);
            }

        }
        #endregion
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

        public static void ShowMsgBox(string text)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            }

        }

        public static void SplashScreen()
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Plese wait... \n Harap tunggu hingga notifikasi (Selesai) muncul.", MessageBoxButtons.OK, MessageBoxIcon.Information))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            }
        }

        public static void ShowMsgBoxInformation(string text)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            }
        }

        void ShowMsgBoxInternal(string text)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            }

        }

        public static string ShowMsgDialog(string message)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
               LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

                if(dialog.DialogResult == DialogResult.Yes)
                {
                    return "OK";
                }
                else
                {
                    return "NO";
                }
            }
        }

        public void GetItemUndiscounted()
        {
            int count = 0;
            var itemVouchTBSI = new string[0];
            var itemVouchTBSIOld = new string[0];
            var posTransaction = GME_Var.welcomePosTransaction;
            LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;

            foreach (SaleLineItem lineItem in tmp)
            {
                int counter = 0;
                for (int i = 0; i < GME_Var.displaynumberUndiscounted.Length; i++)
                {
                    if (lineItem.ItemId != GME_Var.displaynumberUndiscounted[i])
                    {
                        counter++;
                    }
                }

                if (counter == GME_Var.displaynumberUndiscounted.Length)
                {
                    count++;
                    itemVouchTBSI = new string[count];
                    itemVouchTBSI[count - 1] = lineItem.ItemId;

                    if (itemVouchTBSI[0] == null)
                    {
                        for (int i = 0; i < itemVouchTBSIOld.Length; i++)
                        {
                            itemVouchTBSI[i] = itemVouchTBSIOld[i];
                        }
                    }
                    itemVouchTBSIOld = itemVouchTBSI;
                }
            }
            GME_Var.itemVouchTBSI = itemVouchTBSI;
        }

        public void Point()
        {
            if (!GME_Var.isPointUsed)
            {
                queryData data = new queryData();
                if (data.isStorePointPay(Application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                {
                    //POINT
                    if (GME_Var._EngageHOStatus)
                    {
                        if (GME_Var.identifierCode != null && GME_Var.identifierCode != "9999999999999999999" && GME_Var.identifierCode != "")
                        {
                            if (ShowMsgDialog(GME_Var.msgBoxStorePointPay) == "OK")
                            {
                                if (GME_Var.customerData == null)
                                {
                                    ShowMsgBox("Harap masukan customer terlebih dahulu");
                                }
                                else
                                {
                                    Jacksonsoft.WaitWindow.Show(this.DoProgressRedeemPoint, "Please Wait ...");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PointAndDonation()
        {
            queryData data = new queryData();

            if (data.isStorePointPay(Application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
            {
                if (!GME_Var.isPointUsed)
                {
                    if (GME_Var._EngageHOStatus)
                    {
                        if (GME_Var.DiscVoucher != 1)
                        {
                            if (GME_Var.identifierCode != null && GME_Var.identifierCode != "9999999999999999999" && GME_Var.identifierCode != "")
                            {
                                if (ShowMsgDialog(GME_Var.msgBoxStorePointPay) == "OK")
                                {

                                    if (GME_Var.customerData == null)
                                    {
                                        ShowMsgBox("Harap masukan customer terlebih dahulu");
                                    }
                                    else
                                    {
                                        Jacksonsoft.WaitWindow.Show(this.DoProgressRedeemPoint, "Please Wait ...");
                                    }
                                }
                            }
                        }
                    }

                    if (data.isStoreDonasi(Application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                    {
                        if (!GME_Var.isDonasi)
                        {
                            if (ShowMsgDialog(GME_Var.msgBoxStoreDonasi) == "OK")
                            {
                                GME_Var.isDonasi = true;
                                GME_FormCaller.GME_FormDonasi(Connection.applicationLoc);
                            }
                        }
                    }
                    
                }
                /// ADD BY MARIA -- FORM DONASI
                else
                {
                    if (data.isStoreDonasi(Application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                    {

                        if (!GME_Var.isDonasi)
                        {
                            if (ShowMsgDialog(GME_Var.msgBoxStoreDonasi) == "OK")
                            {
                                GME_Var.isDonasi = true;
                                GME_FormCaller.GME_FormDonasi(Connection.applicationLoc);
                            }
                        }
                    }
                }
            }
            else
            {
                if (data.isStoreDonasi(Application.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                {

                    if (!GME_Var.isDonasi)
                    {
                        if (ShowMsgDialog(GME_Var.msgBoxStoreDonasi) == "OK")
                        {
                            GME_Var.isDonasi = true;
                            GME_FormCaller.GME_FormDonasi(Connection.applicationLoc);
                        }
                    }
                }
            }
        }

        private void DoProgressRedeemPoint(object sender, Jacksonsoft.WaitWindowEventArgs e)
        {
            GME_Var.paymentMethods.Add("PTS");
            GME_Var.isPoint = true;
            GME_Var.isPointUsed = true;
            Application.RunOperation(PosisOperations.RedeemLoyaltyPoints, "");  
        }

        public void receiveThreadBCA(object stateInfo)
        {
            using (GME_Var.serialBCA)
            {
                // get start time
                DateTime start = DateTime.Now;
                // buffer for pushing received string data into
                StringBuilder indata = new StringBuilder();

                // loop until at most 10 seconds have passed 
                while (!_isReceived)
                {
                    if (GME_Var.serialBCA.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[GME_Var.serialBCA.BytesToRead];
                        int bytesRead = GME_Var.serialBCA.Read(buffer, 0, buffer.Length);
                        if (bytesRead <= 0) return;
                        //serialBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        buffer.Last();
                        indata.Clear();
                        foreach (byte b in buffer)
                        {
                            indata.Append(string.Format("{0:x2}", b));
                        }
                    }
                    else
                        System.Threading.Thread.Sleep(25);

                    if (indata.Length > 0)
                    {
                        if (indata.ToString() != _LastResponBCA)
                        {
                            string hex = indata.ToString();
                            if (hex.Length > 2 && hex != "06")
                            {
                                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                                BCAOnline.MappingText(hex);
                                GME_Var.serialBCA.Dispose();
                                _LastResponBCA = hex;
                                isEDCSettlementBCA = false;
                                ((AutoResetEvent)stateInfo).Set();
                                break;
                                // isReceived = true;
                            }
                        } else if (GME_Var.transTypeBCA == "10")
                        {
                            string hex = indata.ToString();
                            if (hex.Length > 2 && hex != "06")
                            {
                                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                                BCAOnline.MappingText(hex);
                                GME_Var.serialBCA.Dispose();
                                _LastResponBCA = hex;
                                isEDCSettlementBCA = false;
                                ((AutoResetEvent)stateInfo).Set();
                                break;
                                // isReceived = true;
                            }
                        }
                    }
                }


            }
        }

        public void receiveThreadMandiri(object stateInfo)
        {
            using (GME_Var.serialMandiri)
            {
                // get start time
                DateTime start = DateTime.Now;
                // buffer for pushing received string data into
                StringBuilder indata = new StringBuilder();

                // loop until at most 10 seconds have passed 
                while (!_isReceived)
                {
                    if (GME_Var.serialMandiri.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[GME_Var.serialMandiri.BytesToRead];
                        int bytesRead = GME_Var.serialMandiri.Read(buffer, 0, buffer.Length);
                        if (bytesRead <= 0) return;
                        //serialBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        buffer.Last();
                        indata.Clear();
                        foreach (byte b in buffer)
                        {
                            indata.Append(string.Format("{0:x2}", b));
                        }
                    }
                    else
                        System.Threading.Thread.Sleep(25);

                    if (indata.Length > 0)
                    {
                        if (indata.ToString() != _LastResponMDR)
                        {
                            string hex = indata.ToString();
                            if (hex.Length > 2 && hex != "06")
                            {
                                ElectronicDataCaptureMandiri MDROnline = new ElectronicDataCaptureMandiri();
                                MDROnline.MappingTextMandiri(hex);
                                GME_Var.serialMandiri.Dispose();
                                _LastResponMDR = hex;
                                isEDCSettlementMDR = false;
                                ((AutoResetEvent)stateInfo).Set();
                                break;
                                // isReceived = true;
                            }
                        }else if (GME_Var.transTypeMandiri == "30")
                        {
                            string hex = indata.ToString();
                            if (hex.Length > 2 && hex != "06")
                            {
                                ElectronicDataCaptureMandiri MDROnline = new ElectronicDataCaptureMandiri();
                                MDROnline.MappingTextMandiri(hex);
                                GME_Var.serialMandiri.Dispose();
                                _LastResponMDR = hex;
                                isEDCSettlementMDR = false;
                                ((AutoResetEvent)stateInfo).Set();
                                break;
                                // isReceived = true;
                            }
                        }
                    }
                }


            }
        }

        public void getpoint()
        {
            IntegrationService integrationService = new IntegrationService();

            if (GME_Var._EngageHOStatus)
            {
                integrationService.getIdentifier(GME_Var.identifierCode);
                if (GME_Var.personId > 0)
                {
                    integrationService.getPerson(GME_Var.personId);
                }

                string resultGetPoint = string.Empty;
                //get point balances
                if (GME_Var.personHouseHoldId > 0)
                {
                    resultGetPoint = integrationService.getBalances(GME_Var.personHouseHoldId);
                }

                if (resultGetPoint == "Success")
                {
                    //force update to AX
                    if (GME_Var.identifierCode != "9999999999999999999")
                    {
                        integrationService.updatePoint(GME_Var.identifierCode, Convert.ToDecimal(GME_Var.memberPoint), Connection.applicationLoc.Settings.Database.DataAreaID);
                    }

                    if (!GME_Var.isShowInfoPoint && GME_Var.identifierCode != "9999999999999999999")
                    {
                        ShowMsgBoxInformation("Point customer saat ini : " + GME_Var.memberPoint);
                    }
                }
                else if (resultGetPoint == "0")
                {
                    if (GME_Var.identifierCode != "9999999999999999999" && !GME_Var.isShowInfoPoint)
                    {
                        ShowMsgBoxInformation("Point customer saat ini : " + GME_Var.memberPoint);
                    }
                }
                else
                {
                    if (GME_Var.identifierCode != "9999999999999999999" && !GME_Var._EngageHOStatus && !GME_Var.isShowInfoPoint)
                    {
                        ShowMsgBoxInformation("Untuk saat ini tidak bisa melakukan pembayaran menggunakan point");
                    }
                }
            }
        }
    }
}
