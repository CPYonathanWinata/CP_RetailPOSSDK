/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

//Microsoft Dynamics AX for Retail POS Plug-ins 
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
using LSRetailPosis.Transaction;
using System.Data;
using System;
using System.Collections.Generic;
using GME_Custom.GME_EngageFALWSServices;
using LSRetailPosis.Transaction.Line.TenderItem;
using Jacksonsoft;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.PaymentTriggers
{
    [Export(typeof(IPaymentTrigger))]
    public class PaymentTriggers : IPaymentTrigger
    {

        #region Constructor - Destructor
        [Import]
        public IApplication Application { get; set; }

        public PaymentTriggers()
        {

            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for PaymentTriggers are reserved at 50400 - 50449

        }

        #endregion

        #region IPaymentTriggers Members

        public void PrePayCustomerAccount(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, decimal amount)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PrePayCustomerAccount", "Before charging to a customer account", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PrePayCardAuthorization(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, ICardInfo cardInfo, decimal amount)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PrePayCardAuthorization", "Before the EFT authorization", LSRetailPosis.LogTraceLevel.Trace);
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            queryData data = new queryData();

            if (GME_Var.isEDCBCA)
            {
                cardInfo.CardNumber = "52";
                cardInfo.CardTypeId = "EDC BCA";
                cardInfo.TenderTypeId = "16";

                this.checkPaymetToEngage("10");

                GME_Var.payCardBCA = true;
            }
            else if (GME_Var.isEDCOffline)
            {
                if (GME_Var.isEDCBCAOffline)
                {
                    cardInfo.CardNumber = "52";
                    cardInfo.CardTypeId = "EDC BCA";
                    cardInfo.TenderTypeId = "16";

                    this.checkPaymetToEngage("10");
                    GME_Var.payCardBCAOffline = true;                    
                }

                if (GME_Var.isEDCMandiriOffline)
                {
                    cardInfo.CardNumber = "44";
                    cardInfo.CardTypeId = "EDC MDR";
                    cardInfo.TenderTypeId = "17";

                    this.checkPaymetToEngage("12");
                    GME_Var.payCardMandiriOffline = true;                    
                }

                GME_FormCaller.GME_FormPayCardOfflineApproval(Connection.applicationLoc, preTriggerResult, posTransaction);

                if (GME_Var.paycardofflineamount > 0)
                {
                    if (GME_Var.paycardofflineamount < retailTransaction.TransSalePmtDiff)
                    {
                        preTriggerResult.ContinueOperation = false;
                        Application.RunOperation(PosisOperations.BlankOperation, "PartialPaymentEDC");
                    }
                }
            }
            else if (GME_Var.isEDCMandiri)
            {
                cardInfo.CardNumber = "44";
                cardInfo.CardTypeId = "EDC MDR";
                cardInfo.TenderTypeId = "17";

                this.checkPaymetToEngage("12");

                GME_Var.payCardMandiri = true;
            }
            else
            {
                preTriggerResult.ContinueOperation = false;
            }
        }

        /// <summary>
        /// <example><code>
        /// // In order to delete the already-added payment you use the following code:
        /// if (retailTransaction.TenderLines.Count > 0)
        /// {
        ///     retailTransaction.TenderLines.RemoveLast();
        ///     retailTransaction.LastRunOpererationIsValidPayment = false;
        /// }
        /// </code></example>
        /// </summary>
        public void OnPayment(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.OnPayment", "On the addition of a tender...", LSRetailPosis.LogTraceLevel.Trace);
        }


        /// <summary>
        /// added this method for add vouchers
        /// </summary>
        /// <param name="listVoucher"></param>
        /// <returns></returns>
        private vouchersDto setVouchers(System.Collections.Generic.List<voucherDto> listVoucher)
        {
            vouchersDto vouchers = new vouchersDto();
            vouchers.voucher = new voucherDto[listVoucher.Count];

            for (int i = 0; i < listVoucher.Count; i++)
            {
                vouchers.voucher[i] = listVoucher[i];
            }


            return vouchers;
        }

        /// <summary>
        /// added this method for add ticket
        /// </summary>
        /// <param name="ticketNumber"></param>
        /// <param name="ticketDate"></param>
        /// <param name="vouchers"></param>
        /// <returns></returns>
        private ticket setTicket(string ticketNumber, string ticketDate, vouchersDto vouchers = null)
        {
            ticket ticket = new ticket();
            ticket.ticketNumber = ticketNumber;
            ticket.ticketDate = ticketDate;

            if (vouchers != null)
            {
                ticket.vouchers = vouchers;
            }

            return ticket;
        }

        public void PrePayment(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, object posOperation, string tenderId)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PrePayment", "On the start of a payment operation...", LSRetailPosis.LogTraceLevel.Trace);

            queryData data = new queryData();
            IntegrationService integrationService = new IntegrationService();
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            GME_Var.isReprint = false;

            DateTime dateNow = DateTime.Now;

            if (GME_Var.identifierCode != null && GME_Var.identifierCode != "")
            {                
                Jacksonsoft.WaitWindow.Show(DoProgressGetPoint, "Please Wait ...");                
            }

            if (!preTriggerResult.ContinueOperation) { goto outtoendmethod; }

            outtoendmethod:;

            void DoProgressGetPoint(object sender, Jacksonsoft.WaitWindowEventArgs e)
            {
                if (posOperation.ToString() != "PayCard")
                {
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

                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Point customer saat ini : " + GME_Var.memberPoint);

                            }
                        }
                        else if (resultGetPoint == "0")
                        {
                            if (GME_Var.identifierCode != "9999999999999999999" && !GME_Var.isShowInfoPoint)
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Point customer saat ini : " + GME_Var.memberPoint);

                            }
                        }
                        else
                        {
                            if (GME_Var.identifierCode != "9999999999999999999" && !GME_Var.pingStatus && !GME_Var.isShowInfoPoint)
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Untuk saat ini tidak bisa melakukan pembayaran menggunakan point");
                            }
                        }
                    }
                }

                if (!GME_Var._EngageHOStatus && GME_Var.DiscVoucher == 1 || GME_Var.CashVoucher == 1)
                {
                    BlankOperations.BlankOperations.ShowMsgBox("Koneksi terputus. Transaksi menggunakan voucher tidak dapat dilakukan dalam kondisi offline. Mohon lakukan void transaksi");
                    preTriggerResult.ContinueOperation = false;
                }

                switch ((PosisOperations)posOperation)
                {
                    case PosisOperations.PayCash:
                        // Insert code here... 

                        if (retailTransaction.AmountDue < 0)
                        {
                            BlankOperations.BlankOperations.ShowMsgBoxInformation("Total amount transaksi tidak boleh minus !");
                            preTriggerResult.ContinueOperation = false;
                        }
                        else
                        {
                            GME_Var.isShowInfoPoint = true;
                            if (GME_Var.isCashVoucher || tenderId == "16" || tenderId == "17") { goto outtoend; }
                            if (data.isStorePointPay(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                            {
                                if (GME_Var._EngageHOStatus)
                                {
                                    if (!GME_Var.isPointUsed)
                                    {
                                        if (GME_Var.identifierCode != null && GME_Var.identifierCode != "9999999999999999999" && GME_Var.identifierCode != "")
                                        {
                                            if (BlankOperations.BlankOperations.ShowMsgDialog(GME_Var.msgBoxStorePointPay) == "OK")
                                            {

                                                if (GME_Var.customerData == null)
                                                {
                                                    BlankOperations.BlankOperations.ShowMsgBox("Harap masukan customer terlebih dahulu");
                                                    preTriggerResult.ContinueOperation = false;
                                                }
                                                else
                                                {
                                                    this.checkPaymetToEngage("PTS");
                                                    GME_Var.isPoint = true;
                                                    GME_Var.isPointUsed = true;
                                                    Connection.applicationLoc.RunOperation(PosisOperations.RedeemLoyaltyPoints, "");
                                                    preTriggerResult.ContinueOperation = false;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (data.isStoreDonasi(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                                {                                        
                                    if (!GME_Var.isDonasi)
                                    {
                                        if (BlankOperations.BlankOperations.ShowMsgDialog(GME_Var.msgBoxStoreDonasi) == "OK")
                                        {
                                            GME_Var.isDonasi = true;
                                            GME_FormCaller.GME_FormDonasi(Connection.applicationLoc);
                                            preTriggerResult.ContinueOperation = false;
                                        }
                                    }
                                }                                
                                /// ADD BY MARIA -- FORM DONASI
                                else
                                {
                                    if (data.isStoreDonasi(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                                    {

                                        if (!GME_Var.isDonasi)
                                        {
                                            if (BlankOperations.BlankOperations.ShowMsgDialog(GME_Var.msgBoxStoreDonasi) == "OK")
                                            {
                                                GME_Var.isDonasi = true;
                                                GME_FormCaller.GME_FormDonasi(Connection.applicationLoc);
                                                preTriggerResult.ContinueOperation = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (data.isStoreDonasi(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                                {

                                    if (!GME_Var.isDonasi)
                                    {
                                        if (BlankOperations.BlankOperations.ShowMsgDialog(GME_Var.msgBoxStoreDonasi) == "OK")
                                        {
                                            GME_Var.isDonasi = true;
                                            GME_FormCaller.GME_FormDonasi(Connection.applicationLoc);
                                            preTriggerResult.ContinueOperation = false;
                                        }
                                    }
                                }
                            }
                            outtoend:; //skip if cash voucher cause already do it in the front
                        }
                        GME_Var.retailTransGlobal = retailTransaction;

                        GME_Var.isPayTransaction = true;

                        if (tenderId == "2") //cash voucher
                        {
                            this.checkPaymetToEngage("35");

                        }
                        else if (tenderId == "1") //tunai
                        {
                            this.checkPaymetToEngage("18");
                        }                        
                        else if (tenderId == "16" && GME_Var.payCardBCAOffline)
                        {
                            GME_Var.approvalOfflineBCA = GME_Var.payCardApprovalCodeOffline;
                        }
                        else if (tenderId == "17" && GME_Var.payCardMandiriOffline)
                        {
                            GME_Var.approvalOfflineMandiri = GME_Var.payCardApprovalCodeOffline;
                        }

                        break;
                    case PosisOperations.PayCard:
                        // Insert code here...
                        GME_Var.isShowInfoPoint = true;
                        GME_Var.isPayTransaction = true;
                        GME_Var.isPayCard = true;
                        ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                        ElectronicDataCaptureMandiri MandiriOnline = new ElectronicDataCaptureMandiri();

                        if (GME_Var.isEDCBCA)
                        {
                            this.checkPaymetToEngage("10");
                        }

                        if (GME_Var.isEDCMandiri)
                        {
                            this.checkPaymetToEngage("12");
                        }

                        break;
                    case PosisOperations.PayCheque:
                        // Insert code here...
                        break;
                    case PosisOperations.PayCorporateCard:
                        // Insert code here...
                        break;
                    case PosisOperations.PayCreditMemo:
                        // Insert code here...
                        break;
                    case PosisOperations.PayCurrency:
                        // Insert code here...
                        break;
                    case PosisOperations.PayCustomerAccount:
                        // Insert code here...
                        break;
                    case PosisOperations.PayGiftCertificate:
                        // Insert code here...
                        break;
                    case PosisOperations.PayLoyalty:
                        // Insert code here...
                        //loyalty here
                        if (!GME_Var._EngageHOStatus)
                        {
                            BlankOperations.BlankOperations.ShowMsgBox("Transaksi menggunakan redeemtion tidak dapat dilakukan dalam kondisi offline. Mohon lakukan void transaksi");
                        }
                        else
                        {
                            DataTable dtItems = data.getLoyaltyItems(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            bool isValidReedemItems = false;
                            bool isPointValid = false;
                            List<LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem> ItemsFailed = new List<LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem>();
                            if (dtItems != null)
                            {
                                if (dtItems.Rows.Count > 0)
                                {
                                    if (retailTransaction.SaleItems.Count > 0)
                                    {
                                        foreach (var lineItem in retailTransaction.SaleItems)
                                        {
                                            isValidReedemItems = false;
                                            foreach (DataRow row in dtItems.Rows)
                                            {
                                                if (row.ItemArray[0].ToString().Equals(lineItem.ItemId))
                                                {
                                                    string point = data.getItemPointPrice(lineItem.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                                    if (point != string.Empty && point.Length > 0)
                                                    {
                                                        if (Convert.ToDouble(GME_Var.memberPoint) > Convert.ToDouble(point))
                                                        {
                                                            isPointValid = true;
                                                        }
                                                    }
                                                    isValidReedemItems = true;
                                                }
                                            }

                                            if (!isValidReedemItems)
                                            {
                                                ItemsFailed.Add(lineItem);
                                            }

                                        }
                                    }

                                }
                            }


                            if (ItemsFailed.Count == 0)
                            {
                                if (isPointValid)
                                {
                                    GME_Var.isShowInfoPoint = true;
                                    GME_Var.isReedem = true;
                                    GME_Var.isPoint = true;
                                    this.checkPaymetToEngage("PTS");
                                    GME_Var.isPayTransaction = true;
                                }
                                else
                                {
                                    BlankOperations.BlankOperations.ShowMsgBoxInformation("Point anda tidak mencukupi untuk melakukan reedem katalog");
                                    preTriggerResult.ContinueOperation = false;
                                }

                            }
                            else
                            {
                                string itemidmsg = string.Empty;
                                for (int j = 0; j < ItemsFailed.Count; j++)
                                {
                                    LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem saleItem = ItemsFailed[j];
                                    itemidmsg += saleItem.ItemId + " ";
                                }
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Item " + itemidmsg + "tidak terdaftar di dalam reedem katalog. Harap lakukan void all");

                                for (int j = 0; j < ItemsFailed.Count; j++)
                                {
                                    LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem saleItem = ItemsFailed[j];
                                }

                                preTriggerResult.ContinueOperation = false;
                            }                            
                        }
                        break;
                }
            }

            void DoProgressRedeemPoint(object sender, Jacksonsoft.WaitWindowEventArgs e)
            {
                this.checkPaymetToEngage("PTS");
                GME_Var.isPoint = true;
                Connection.applicationLoc.RunOperation(PosisOperations.RedeemLoyaltyPoints, "");
                preTriggerResult.ContinueOperation = false;
            }
        }

        /// <summary>
        /// Triggered before voiding of a payment.
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"> </param>
        public void PreVoidPayment(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PreVoidPayment", "Before the void payment operation...", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered after voiding of a payment.
        /// </summary>
        /// <param name="posTransaction"></param>
        /// <param name="lineId"> </param>
        public void PostVoidPayment(IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PostVoidPayment", "After the void payment operation...", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered before registering cash payment.
        /// </summary>
        /// <param name="preTriggerResult"></param>
        /// <param name="posTransaction"></param>
        /// <param name="posOperation"></param>
        /// <param name="tenderId"></param>
        /// <param name="currencyCode"></param>
        /// <param name="amount"></param>
        public void PreRegisterPayment(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, object posOperation, string tenderId, string currencyCode, decimal amount)
        {
            LSRetailPosis.ApplicationLog.Log("PaymentTriggers.PreRegisterPayment", "Before registering the payment...", LSRetailPosis.LogTraceLevel.Trace);
        }

        private bool checkPaymetToEngage(string payMethod)
        {
            bool isInsertedCard = false;
            foreach (string val in GME_Var.paymentMethods)
            {
                if (val == payMethod)
                {
                    isInsertedCard = true;
                }
            }

            if (!isInsertedCard)
            {
                GME_Var.paymentMethods.Add(payMethod);
            }

            return isInsertedCard;
        }

        #endregion
    }
}
