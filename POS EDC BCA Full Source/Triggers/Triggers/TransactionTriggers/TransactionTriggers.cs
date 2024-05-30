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
using System.Data.SqlClient;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;
using LSRetailPosis.Transaction.Line.TenderItem;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Collections.Generic;
using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
using System.Data;
using GME_Custom.GME_EngageFALWSServices;
using System;

namespace Microsoft.Dynamics.Retail.Pos.TransactionTriggers
{
    [Export(typeof(ITransactionTrigger))]
    public class TransactionTriggers : ITransactionTrigger
    {
        #region Constructor - Destructor
        string receiptId = string.Empty;

        public TransactionTriggers()
        {

            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for TransactionTriggers are reserved at 50300 - 50349
        }

        #endregion

        public void BeginTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.BeginTransaction", "When starting the transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        /// <summary>
        /// Triggered during save of a transaction to database.
        /// </summary>
        /// <param name="posTransaction">PosTransaction object.</param>
        /// <param name="sqlTransaction">SqlTransaction object.</param>
        /// <remarks>
        /// Use provided sqlTransaction to write to the DB. Don't commit, rollback transaction or close the connection.
        /// Any exception thrown from this trigger will rollback the saving of pos transaction.
        /// </remarks>
        public void SaveTransaction(IPosTransaction posTransaction, SqlTransaction sqlTransaction)
        {
            ApplicationLog.Log("TransactionTriggers.SaveTransaction", "Saving a transaction.", LogTraceLevel.Trace);

            //if (posTransaction.GetType().Name == "RetailTransaction")
            //{
            //    queryData data = new queryData();

            //    if (((LSRetailPosis.Transaction.PosTransaction)(posTransaction)).EntryStatus != LSRetailPosis.Transaction.PosTransaction.TransactionStatus.Voided)
            //    {
            //        if (GME_Var.isBonManual)
            //        {
            //            string channel;
            //            string bonManualNumber;
            //            string reason;
            //            decimal totalAmount = 0;

            //            bonManualNumber = GME_Var.bonManualNumber;
            //            reason = GME_Var.reasonBonManual;

            //            channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

            //            if (bonManualNumber != string.Empty)
            //            {
            //                LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
            //                foreach (SaleLineItem lineItem in tmp)
            //                {
            //                    totalAmount = totalAmount + lineItem.NetAmount;
            //                }

            //                data.insertBonManualDataPull(bonManualNumber, posTransaction.StoreId, reason, channel, posTransaction.TerminalId,
            //                    posTransaction.TransactionId, posTransaction.ReceiptId, totalAmount, Connection.applicationLoc.Settings.Database.DataAreaID,
            //                    Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
            //            }

            //            GME_Var.bonManualNumber = string.Empty;
            //            GME_Var.reasonBonManual = string.Empty;
            //            GME_Var.isBonManual = false;
            //        }
            //    }
            //}
        }

        public void PreEndTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreEndTransaction", "When concluding the transaction, prior to printing and saving...", LSRetailPosis.LogTraceLevel.Trace);

            if (GME_Var.isPayTransaction)
            {
                //Jacksonsoft.WaitWindow.Show(DoProgressPreEndTransaction, "Please Wait ...");

                queryData data = new queryData();
                RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                IntegrationService integrationService = new IntegrationService();

                if (retailTransaction != null)
                {
                    if (retailTransaction.TransactionType == PosTransaction.TypeOfTransaction.Sales)
                    {
                        receiptId = GME_Method.generateReceiptNumberSequence(retailTransaction.TransactionType.ToString(), retailTransaction.AmountDue);
                        GME_Var.receiptID = receiptId;

                        if (GME_Var.isEDCBCA && GME_Var.respCodeBCA != "00") { preTriggerResult.ContinueOperation = false; GME_Var.isEDCBCA = false; }
                        if (GME_Var.isEDCMandiri && GME_Var.respCodeMandiri != "00") { preTriggerResult.ContinueOperation = false; GME_Var.isEDCMandiri = false; }

                        List<lineItem> listLineItem = new List<lineItem>();
                        string result9100 = string.Empty;
                        GME_Var.totalDPP = 0;

                        int index = 1;
                        LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                        foreach (SaleLineItem saleLineItem in tmp)
                        {
                            string familyCode = data.getFamilyCodeItem(saleLineItem.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            bool isSkipInsert = false;
                            if (familyCode != string.Empty)
                            {
                                switch (familyCode.Substring(0, 2))
                                {
                                    case "87":
                                        isSkipInsert = true;
                                        break;
                                }
                            }
                            else
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Item "+ saleLineItem.ItemId  + " tidak bisa digunakan, harap hubungi help desk, silahkan void line");
                            }

                            if (!isSkipInsert)
                            {
                                if (!saleLineItem.Voided)
                                {
                                    long amount = 0;
                                    if (!GME_Var.isReedem)
                                    {
                                        amount = (Convert.ToInt64(saleLineItem.NetAmount) / Convert.ToInt64(saleLineItem.Quantity));
                                    }

                                    listLineItem.Add(setlineItem(index, saleLineItem.ItemId, familyCode, Convert.ToInt32(saleLineItem.Quantity), amount));

                                    string itemdonation = data.getItemDonasi("DONASI", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    if (saleLineItem.ItemId != itemdonation)
                                    {
                                        GME_Var.totalDPP += saleLineItem.NetAmountWithNoTax;
                                    }

                                }

                            }
                            index++;

                        }

                        //reedem
                        if (GME_Var.isReedem)
                        {
                            string kodepengikat = data.getKodePengikatRedeem(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            listLineItem.Add(setlineItem(index, kodepengikat, "", 1, 0));

                        }

                        lineItems lineItems = setLineItems(listLineItem);
                        GME_Var.lineItems = new lineItems();
                        GME_Var.lineItems = lineItems;

                        string trxNumber = receiptId.Substring(10);

                        DateTime dateNow = DateTime.Now;
                        ticket ticket = new ticket();

                        if (GME_Var.listVouchers.Count > 0 && GME_Var.listVouchers != null)
                        {
                            vouchersDto vouchersDto = setVouchers(GME_Var.listVouchers);
                            //ticket = GME_Method.setTicket("000" + trxNumber, dateNow.ToString("yyyyMMdd"), lineItems, 9100, vouchersDto);
                            ticket = GME_Method.setTicket(trxNumber, dateNow.ToString("yyyyMMdd"), lineItems, 9100, vouchersDto);
                        }
                        else
                        {
                            // ticket = GME_Method.setTicket("000" + trxNumber, dateNow.ToString("yyyyMMdd"), lineItems);
                            ticket = GME_Method.setTicket(trxNumber, dateNow.ToString("yyyyMMdd"), lineItems);
                        }

                        //valid -> online do requestRewards 9100 and 9220
                        //if (!GME_Var.isSNFTransaction)
                        //{
                            result9100 = integrationService.requestReward9100(GME_Var.identifierCode, posTransaction.StoreId, trxNumber, retailTransaction.Shift.StaffId, retailTransaction.Shift.TerminalId, dateNow.ToString("yyyyMMdd"), dateNow.ToString("HHmmss"), ticket);

                            if (result9100 != "Success")
                            {
                                GME_Var.SetSNF("PROCESSSTOPPED", "RequestReward9100");

                                if (ticket.vouchers != null)
                                {
                                    for (int i = 0; i < ticket.vouchers.voucher.Length; i++)
                                    {
                                        data.insertOfflineSNFVoucher(ticket.vouchers.voucher[i].voucherCode, posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }

                                    for (int j = 0; j < ticket.lineItems.lineItem.Length; j++)
                                    {
                                        data.insertOfflineSNFLines(ticket.lineItems.lineItem[j].productCode, ticket.lineItems.lineItem[j].familyCode, Convert.ToDecimal(ticket.lineItems.lineItem[j].quantity),
                                            Convert.ToDecimal(ticket.lineItems.lineItem[j].productPrice), string.Empty, ticket.ticketNumber, ticket.ticketDate, ticket.lineItems.lineItem[j].index,
                                            posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < ticket.lineItems.lineItem.Length; j++)
                                    {
                                        data.insertOfflineSNFLines(ticket.lineItems.lineItem[j].productCode, ticket.lineItems.lineItem[j].familyCode, Convert.ToDecimal(ticket.lineItems.lineItem[j].quantity),
                                            Convert.ToDecimal(ticket.lineItems.lineItem[j].productPrice), string.Empty, ticket.ticketNumber, ticket.ticketDate, ticket.lineItems.lineItem[j].index,
                                            posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }
                                }
                            }
                        //}
                        //else
                        //{
                        //    if (ticket.vouchers != null)
                        //    {
                        //        for (int i = 0; i < ticket.vouchers.voucher.Length; i++)
                        //        {
                        //            data.insertOfflineSNFVoucher(ticket.vouchers.voucher[i].voucherCode, posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //        }

                        //        for (int j = 0; j < ticket.lineItems.lineItem.Length; j++)
                        //        {
                        //            data.insertOfflineSNFLines(ticket.lineItems.lineItem[j].productCode, ticket.lineItems.lineItem[j].familyCode, Convert.ToDecimal(ticket.lineItems.lineItem[j].quantity),
                        //                Convert.ToDecimal(ticket.lineItems.lineItem[j].productPrice), string.Empty, ticket.ticketNumber, ticket.ticketDate, ticket.lineItems.lineItem[j].index,
                        //                posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        for (int j = 0; j < ticket.lineItems.lineItem.Length; j++)
                        //        {
                        //            data.insertOfflineSNFLines(ticket.lineItems.lineItem[j].productCode, ticket.lineItems.lineItem[j].familyCode, Convert.ToDecimal(ticket.lineItems.lineItem[j].quantity),
                        //                Convert.ToDecimal(ticket.lineItems.lineItem[j].productPrice), string.Empty, ticket.ticketNumber, ticket.ticketDate, ticket.lineItems.lineItem[j].index,
                        //                posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //        }
                        //    }                            
                        //}

                        //will do it later in loyalty.cs
                        GME_Var.trxNumber = trxNumber;
                        GME_Var.storeId = posTransaction.StoreId;
                        GME_Var.result9100 = result9100;

                        //untuk ENGAGE
                        GME_Var.tenderList.Clear();

                        List<decimal> listCash = new List<decimal>();
                        List<decimal> listCashVoucher = new List<decimal>();
                        List<decimal> listEDCBCA = new List<decimal>();
                        List<decimal> listEDCMDR = new List<decimal>();                        

                        foreach (TenderLineItem tenderLineItem in retailTransaction.TenderLines)
                        {

                            switch (tenderLineItem.TenderTypeId)
                            {
                                case "1": //CASH   
                                    listCash.Add(tenderLineItem.Amount);
                                    break;
                                case "2": //CASH VOUCHER
                                    listCashVoucher.Add(tenderLineItem.Amount);
                                    break;
                                case "16"://BCA
                                    listEDCBCA.Add(tenderLineItem.Amount);                                    
                                    break;
                                case "17": //Mandiri
                                    listEDCMDR.Add(tenderLineItem.Amount);                                    
                                    break;
                            }
                        }

                        if (listCash.Count > 0) { GME_Var.tenderList.Add("CASH", listCash); }
                        if (listCashVoucher.Count > 0) { GME_Var.tenderList.Add("CASH VOUCHER", listCashVoucher); }
                        if (listEDCBCA.Count > 0) { GME_Var.tenderList.Add("EDC BCA", listEDCBCA); }
                        if (listEDCMDR.Count > 0) { GME_Var.tenderList.Add("EDC MANDIRI", listEDCMDR); }
                        GME_Var.amountDue = retailTransaction.AmountDue.ToString();
                    }
                }
            }

            #region comment please wait
            /*void DoProgressPreEndTransaction(object sender, Jacksonsoft.WaitWindowEventArgs e)
            {
                queryData data = new queryData();
                RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                IntegrationService integrationService = new IntegrationService();

                if (retailTransaction != null)
                {
                    if (retailTransaction.TransactionType == PosTransaction.TypeOfTransaction.Sales)
                    {
                        receiptId = GME_Method.generateReceiptNumberSequence(retailTransaction.TransactionType.ToString(), retailTransaction.AmountDue);
                        GME_Var.receiptID = receiptId;

                        if (GME_Var.isEDCBCA && GME_Var.respCodeBCA != "00") { preTriggerResult.ContinueOperation = false; GME_Var.isEDCBCA = false; }
                        if (GME_Var.isEDCMandiri && GME_Var.respCodeMandiri != "00") { preTriggerResult.ContinueOperation = false; GME_Var.isEDCMandiri = false; }

                        List<lineItem> listLineItem = new List<lineItem>();
                        string result9100 = string.Empty;
                        GME_Var.totalDPP = 0;

                        int index = 1;
                        LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                        foreach (SaleLineItem saleLineItem in tmp)
                        {
                            string familyCode = data.getFamilyCodeItem(saleLineItem.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            bool isSkipInsert = false;
                            switch (familyCode.Substring(0, 2))
                            {
                                case "87":
                                    isSkipInsert = true;
                                    break;
                            }

                            if (!isSkipInsert)
                            {
                                if (!saleLineItem.Voided)
                                {
                                    int amount = 0;
                                    if (!GME_Var.isReedem)
                                    {
                                        amount = (Convert.ToInt32(saleLineItem.NetAmount) / Convert.ToInt32(saleLineItem.Quantity));
                                    }

                                    listLineItem.Add(setlineItem(index, saleLineItem.ItemId, familyCode, Convert.ToInt32(saleLineItem.Quantity), amount));

                                    string itemdonation = data.getItemDonasi("DONASI", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    if (saleLineItem.ItemId != itemdonation)
                                    {
                                        GME_Var.totalDPP += saleLineItem.NetAmountWithNoTax;
                                    }

                                }

                            }
                            index++;

                        }

                        //reedem
                        if (GME_Var.isReedem)
                        {
                            string kodepengikat = data.getKodePengikatRedeem(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            listLineItem.Add(setlineItem(index, kodepengikat, "", 1, 0));

                        }

                        lineItems lineItems = setLineItems(listLineItem);
                        GME_Var.lineItems = new lineItems();
                        GME_Var.lineItems = lineItems;

                        string trxNumber = receiptId.Substring(10);

                        DateTime dateNow = DateTime.Now;
                        ticket ticket = new ticket();

                        if (GME_Var.listVouchers.Count > 0 && GME_Var.listVouchers != null)
                        {
                            vouchersDto vouchersDto = setVouchers(GME_Var.listVouchers);
                            //ticket = GME_Method.setTicket("000" + trxNumber, dateNow.ToString("yyyyMMdd"), lineItems, 9100, vouchersDto);
                            ticket = GME_Method.setTicket(trxNumber, dateNow.ToString("yyyyMMdd"), lineItems, 9100, vouchersDto);
                        }
                        else
                        {
                            // ticket = GME_Method.setTicket("000" + trxNumber, dateNow.ToString("yyyyMMdd"), lineItems);
                            ticket = GME_Method.setTicket(trxNumber, dateNow.ToString("yyyyMMdd"), lineItems);
                        }

                        //valid -> online do requestRewards 9100 and 9220                                     
                        result9100 = integrationService.requestReward9100(GME_Var.identifierCode, posTransaction.StoreId, trxNumber, retailTransaction.Shift.StaffId, retailTransaction.Shift.TerminalId, dateNow.ToString("yyyyMMdd"), dateNow.ToString("HHmmss"), ticket);

                        //will do it later in loyalty.cs
                        GME_Var.trxNumber = trxNumber;
                        GME_Var.storeId = posTransaction.StoreId;
                        GME_Var.result9100 = result9100;

                        //untuk ENGAGE
                        GME_Var.tenderList.Clear();

                        foreach (TenderLineItem tenderLineItem in retailTransaction.TenderLines)
                        {

                            switch (tenderLineItem.TenderTypeId)
                            {

                                case "1": //CASH                         
                                    if (!GME_Var.tenderList.ContainsKey("CASH"))
                                    {
                                        GME_Var.tenderList.Add("CASH", tenderLineItem.Amount);
                                    }
                                    else
                                    {

                                        bool isInserted = false;
                                        for (int i = 1; i <= 10; i++)
                                        {
                                            if (!GME_Var.tenderList.ContainsKey("CASH_" + i))
                                            {
                                                if (!isInserted)
                                                {
                                                    GME_Var.tenderList.Add("CASH_" + i, tenderLineItem.Amount);
                                                    isInserted = true;
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                    break;
                                case "2": //CASH VOUCHER
                                    if (!GME_Var.tenderList.ContainsKey("CASH VOUCHER"))
                                    {
                                        GME_Var.tenderList.Add("CASH VOUCHER", tenderLineItem.Amount);
                                    }
                                    else
                                    {
                                        bool isInserted = false;
                                        for (int i = 1; i <= 10; i++)
                                        {
                                            if (!GME_Var.tenderList.ContainsKey("CASH VOUCHER_" + i))
                                            {
                                                if (!isInserted)
                                                {
                                                    GME_Var.tenderList.Add("CASH VOUCHER_" + i, tenderLineItem.Amount);
                                                    isInserted = true;
                                                    break;
                                                }

                                            }
                                        }
                                    }

                                    break;
                                case "16"://BCA
                                    if (!GME_Var.tenderList.ContainsKey("EDC BCA"))
                                    {
                                        GME_Var.tenderList.Add("EDC BCA", tenderLineItem.Amount);
                                    }
                                    else
                                    {
                                        bool isInserted = false;
                                        for (int i = 1; i <= 10; i++)
                                        {
                                            if (!GME_Var.tenderList.ContainsKey("EDC BCA_" + i)) ;
                                            {
                                                if (!isInserted)
                                                {
                                                    GME_Var.tenderList.Add("EDC BCA_" + i, tenderLineItem.Amount);
                                                    isInserted = true;
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                    break;
                                case "17": //Mandiri
                                    if (!GME_Var.tenderList.ContainsKey("EDC MANDIRI"))
                                    {
                                        GME_Var.tenderList.Add("EDC MANDIRI", tenderLineItem.Amount);
                                    }
                                    else
                                    {
                                        bool isInserted = false;
                                        for (int i = 1; i <= 10; i++)
                                        {
                                            if (!GME_Var.tenderList.ContainsKey("EDC MANDIRI_" + i)) ;
                                            {
                                                if (!isInserted)
                                                {
                                                    GME_Var.tenderList.Add("EDC MANDIRI_" + i, tenderLineItem.Amount);
                                                    isInserted = true;
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                    break;


                                    //loyalty tidak dikirim ke ENGAGE
                                    //case "LOYALTY CARD": //LOYALTY
                                    //    GME_Var.tenderList.Add("LOYALTI CARD", tenderLineItem.Amount);
                                    //    break;
                            }
                        }
                        GME_Var.amountDue = retailTransaction.AmountDue.ToString();
                    }
                }
            }*/
            #endregion
        }

        public void PostEndTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PostEndTransaction", "When concluding the transaction, after printing and saving", LSRetailPosis.LogTraceLevel.Trace);

            if (GME_Var.isPayTransaction)
            {
                //Jacksonsoft.WaitWindow.Show(DoProgressPostEndTransaction, "Please Wait ...");
                queryData data = new queryData();
                RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                IntegrationService integrationService = new IntegrationService();

                if (retailTransaction != null)
                {
                    if (retailTransaction.TransactionType == PosTransaction.TypeOfTransaction.Sales)
                    {
                        if (GME_Var.isBonManual)
                        {
                            string channel;
                            string bonManualNumber;
                            string reason;
                            decimal totalAmount = 0;

                            bonManualNumber = GME_Var.bonManualNumber;
                            reason = GME_Var.reasonBonManual;

                            channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            if (bonManualNumber != string.Empty)
                            {
                                LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                                foreach (SaleLineItem lineItem in tmp)
                                {
                                    totalAmount = totalAmount + lineItem.NetAmount;
                                }

                                data.insertBonManualDataPull(bonManualNumber, posTransaction.StoreId, reason, channel, posTransaction.TerminalId,
                                    posTransaction.TransactionId, posTransaction.ReceiptId, totalAmount, Connection.applicationLoc.Settings.Database.DataAreaID,
                                    Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }

                            GME_Var.bonManualNumber = string.Empty;
                            GME_Var.reasonBonManual = string.Empty;
                            GME_Var.isBonManual = false;
                        }

                        receiptId = GME_Method.generateReceiptNumberSequence(retailTransaction.TransactionType.ToString(), retailTransaction.AmountDue);

                        //ADD BY RIZKI for testing active voucher
                        if (GME_Var.isActiveVoucher)
                        {
                            bool isActive = false;

                            DataTable dt = data.getVoucherDataTemp(posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dt.Rows[i];

                                //Active Voucher 
                                try
                                {
                                    integrationService.updateVoucherActive(Connection.applicationLoc.Settings.Database.DataAreaID, dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId);
                                    isActive = true;
                                }
                                catch (Exception er)
                                {
                                    isActive = false;
                                }
                            }

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dt.Rows[i];
                                data.deleteVoucherDataTemp(dr["VOUCHERID"].ToString(), posTransaction.Shift.TerminalId, posTransaction.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }

                            //ADD BY RIZKI for testing active voucher
                            if (isActive)
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher anda sudah aktif, dan dapat digunakan.");
                            }
                        }

                        ////Added by Adhi(TBSI Voucher Baru)
                        if (GME_Var.isTBSIVoucherBaru)
                        {
                            vouchersList voucherList = new vouchersList();
                            DataTable redeemVoucherDT = new DataTable();

                            redeemVoucherDT = data.getRedeemVoucher(posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                voucherList = integrationService.updateVoucherRedeem(Connection.applicationLoc.Settings.Database.DataAreaID, dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId);
                            }

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                data.deleteRedeemVoucher(dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }

                            data.updateVoucher(GME_Var.voucherTBSIBaru, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId);
                        }

                        if (GME_Var.isCash || GME_Var.isDisc)
                        {
                            vouchersList voucherList = new vouchersList();
                            DataTable redeemVoucherDT = new DataTable();

                            redeemVoucherDT = data.getRedeemVoucher(posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                voucherList = integrationService.updateVoucherRedeem(Connection.applicationLoc.Settings.Database.DataAreaID, dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId);
                            }

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                data.deleteRedeemVoucher(dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }
                        }

                        //proses stoped  at req reward 9100
                        GME_Custom.GME_Data.queryDataOffline queryDataOffline = new GME_Custom.GME_Data.queryDataOffline();

                        //tambahan buat reprint struk
                        Dictionary<string, string> value = new Dictionary<string, string>();

                        int pointVisit = (GME_Custom.GME_Propesties.GME_Var.tempPointBalance - Convert.ToInt32(GME_Custom.GME_Propesties.GME_Var.memberPoint)) - GME_Custom.GME_Propesties.GME_Var.pointUsed;
                        string approvalCode = "";
                        string PanNumber = string.Empty;
                        if (GME_Var.approvalCodeBCA != string.Empty)
                        {
                            approvalCode = GME_Var.approvalCodeBCA;
                        }
                        else if (GME_Var.approvalCodeMandiri != string.Empty)
                        {
                            approvalCode = GME_Var.approvalCodeMandiri;
                        }
                        else if (GME_Var.payCardApprovalCodeOffline != string.Empty)
                        {
                            approvalCode = GME_Var.payCardApprovalCodeOffline;
                        }
                        //PAN
                        if (GME_Var.panBCA != string.Empty && GME_Var.panBCA.Length > 0)
                        {
                            PanNumber = "**" + GME_Var.panBCA.Substring(6);
                        }
                        else if (GME_Var.panMandiri != string.Empty && GME_Var.panMandiri.Length > 0)
                        {
                            PanNumber = "**" + GME_Var.panMandiri.Substring(6);
                        }
                        //string totalPoint = string.Empty;
                        //if (!GME_Var.isReedem)
                        //{
                        //    totalPoint = GME_Var.tempPointBalance.ToString();
                        //}
                        //else
                        //{
                        //    if (pointVisit >= 0)
                        //    {
                        //        pointVisit = pointVisit * -1;
                        //    }
                        //    int temp = Convert.ToInt32(GME_Var.tempPointBalance + pointVisit);
                        //    totalPoint = temp.ToString();
                        //}

                        //negate reeedem point
                        //negate the value
                        string reedemPoint = string.Empty;
                        if (GME_Var.pointUsed > 0)
                        {
                            int tempReedem = GME_Var.pointUsed * -1;
                            reedemPoint = tempReedem.ToString();
                        }
                        else
                        {
                            reedemPoint = GME_Var.pointUsed.ToString();
                        }

                        value.Add("POINT", Convert.ToInt32(GME_Custom.GME_Propesties.GME_Var.memberPoint.ToString()).ToString() + ";" + GME_Var.PointVisit.ToString() + ";" + GME_Var.totalPoint.ToString() + ";" + reedemPoint);
                        value.Add("DPP", GME_Custom.GME_Propesties.GME_Var.totalDPP.ToString());
                        value.Add("ApprovalCode", approvalCode);
                        value.Add("PAN", PanNumber);

                        //check connection to dbchannell                      
                        if (GME_Var.isSNFTransaction)
                            queryDataOffline.insertOfflineSNF(posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                        if (GME_Var.customerIdOffline != string.Empty)
                            data.updateReceiptCustomerTransTable(GME_Var.receiptID, GME_Var.customerIdOffline, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        else if (GME_Var.customerData != null)
                            data.updateReceiptCustomerTransTable(GME_Var.receiptID, GME_Var.customerData.CustomerId, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        else
                            data.updateReceiptCustomerTransTable(GME_Var.receiptID, "9999999999999999999", posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                        data.updateReceiptIdSalesTrans(GME_Var.receiptID, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                        data.updateRetailTransactionTable(value, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                        //if (GME_Var.isPayCard)
                        //{                            
                        //    if (GME_Var.payCardBCA)
                        //    {                                
                        //        data.updatePaymentTypeCard(GME_Var.invoiceNumberBCA, GME_Var.approvalCodeBCA,
                        //            posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC BCA", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //    }
                        //    else if (GME_Var.payCardBCAOffline)
                        //    {
                        //        data.updateApprovalCode(GME_Var.payCardApprovalCodeOffline, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId);

                        //        data.updatePaymentTypeCard(string.Empty, GME_Var.payCardApprovalCodeOffline,
                        //            posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC BCA", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //    }
                        //    else if (GME_Var.payCardMandiriOffline)
                        //    {
                        //        data.updateApprovalCode(GME_Var.payCardApprovalCodeOffline, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId);

                        //        data.updatePaymentTypeCard(string.Empty, GME_Var.payCardApprovalCodeOffline,
                        //            posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC MDR", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //    }
                        //    else if (GME_Var.payCardMandiri)
                        //    {
                        //        data.updatePaymentTypeCard(GME_Var.invoiceNumberMandiri, GME_Var.approvalCodeMandiri,
                        //                                    posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC MDR", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        //    }
                        //}      

                        string tender = string.Empty;

                        foreach (TenderLineItem tenderLineItem in retailTransaction.TenderLines)
                        {

                            switch (tenderLineItem.TenderTypeId)
                            {                                
                                case "16"://BCA                                    
                                    if (GME_Var.payCardBCA)
                                    {
                                        data.updatePaymentTypeCardBCA(GME_Var.invoiceNumberBCA, GME_Var.approvalCodeBCA,
                                            posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC BCA", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }
                                    else if (GME_Var.payCardBCAOffline)
                                    {
                                        if (GME_Var.approvalOfflineBCA == string.Empty)
                                            GME_Var.approvalOfflineBCA = GME_Var.payCardApprovalCodeOffline;

                                        data.updatePaymentTypeCardBCA(string.Empty, GME_Var.approvalOfflineBCA,
                                            posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC BCA", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }
                                    break;
                                case "17": //Mandiri        
                                    if (GME_Var.payCardMandiriOffline)
                                    {
                                        if (GME_Var.approvalOfflineMandiri == string.Empty)
                                            GME_Var.approvalOfflineMandiri = GME_Var.payCardApprovalCodeOffline;

                                        data.updatePaymentTypeCardMandiri(string.Empty, GME_Var.approvalOfflineMandiri,
                                            posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC MDR", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }
                                    else if (GME_Var.payCardMandiri)
                                    {
                                        data.updatePaymentTypeCardMandiri(GME_Var.invoiceNumberMandiri, GME_Var.approvalCodeMandiri,
                                                                    posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, "EDC MDR", Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                    }
                                    break;
                            }
                        }
                    }
                }

                if (GME_Var.custEnrollType != string.Empty)
                {

                }

                if (GME_Var.DiscVoucher != 0)
                {
                    int cashVoucher = 0;
                    int discVoucher = 1;
                    int discountOrginType = 3; //Manual Discount
                    string channel;
                    channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                    if (GME_Var.iswelcomevoucher == true)
                    {
                        var kodepromoID = data.getKodePromoIDWelcome(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        data.updateKodePromoID(kodepromoID, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId, discountOrginType);
                    }
                    if (GME_Var.isbirthdayvoucher == true)
                    {
                        if (GME_Var.isbirthdayvoucherCLUB == true)
                        {
                            var kodepromoIDCLUB = data.getKodePromoIDBirthdayCLUB(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            data.updateKodePromoID(kodepromoIDCLUB, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId, discountOrginType);
                        }
                        if (GME_Var.isbirthdayvoucherFAN == true)
                        {
                            var kodepromoIDFAN = data.getKodePromoIDBirthdayCLUB(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            data.updateKodePromoID(kodepromoIDFAN, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId, discountOrginType);
                        }
                    }

                    data.insertVoucherTransactionPull(cashVoucher, discVoucher, posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                }

                GME_Method.setAllVariableToDefault();
            }

            GME_Var.isPayTransaction = false;

            #region comment please wait
            /*void DoProgressPostEndTransaction(object sender, Jacksonsoft.WaitWindowEventArgs e)
            {
                queryData data = new queryData();
                RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                IntegrationService integrationService = new IntegrationService();

                if (retailTransaction != null)
                {
                    if (retailTransaction.TransactionType == PosTransaction.TypeOfTransaction.Sales)
                    {
                        if (GME_Var.isBonManual)
                        {
                            string channel;
                            string bonManualNumber;
                            string reason;
                            decimal totalAmount = 0;

                            bonManualNumber = GME_Var.bonManualNumber;
                            reason = GME_Var.reasonBonManual;

                            channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            if (bonManualNumber != string.Empty)
                            {
                                LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                                foreach (SaleLineItem lineItem in tmp)
                                {
                                    totalAmount = totalAmount + lineItem.NetAmount;
                                }

                                data.insertBonManualDataPull(bonManualNumber, posTransaction.StoreId, reason, channel, posTransaction.TerminalId,
                                    posTransaction.TransactionId, posTransaction.ReceiptId, totalAmount, Connection.applicationLoc.Settings.Database.DataAreaID,
                                    Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }

                            GME_Var.bonManualNumber = string.Empty;
                            GME_Var.reasonBonManual = string.Empty;
                            GME_Var.isBonManual = false;
                        }

                        receiptId = GME_Method.generateReceiptNumberSequence(retailTransaction.TransactionType.ToString(), retailTransaction.AmountDue);

                        //ADD BY RIZKI for testing active voucher
                        if (GME_Var.isActiveVoucher)
                        {
                            bool isActive = false;

                            DataTable dt = data.getVoucherDataTemp(posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dt.Rows[i];

                                //Active Voucher 
                                try
                                {
                                    integrationService.updateVoucherActive(Connection.applicationLoc.Settings.Database.DataAreaID, dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId);
                                    isActive = true;
                                }
                                catch (Exception er)
                                {
                                    isActive = false;
                                }
                            }

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dt.Rows[i];
                                data.deleteVoucherDataTemp(dr["VOUCHERID"].ToString(), posTransaction.Shift.TerminalId, posTransaction.Shift.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }

                            //ADD BY RIZKI for testing active voucher
                            if (isActive)
                            {
                                BlankOperations.BlankOperations.ShowMsgBoxInformation("Voucher anda sudah aktif, dan dapat digunakan.");
                            }
                        }

                        ////Added by Adhi(TBSI Voucher Baru)
                        if (GME_Var.isTBSIVoucherBaru)
                        {
                            vouchersList voucherList = new vouchersList();
                            DataTable redeemVoucherDT = new DataTable();

                            redeemVoucherDT = data.getRedeemVoucher(posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                voucherList = integrationService.updateVoucherRedeem(Connection.applicationLoc.Settings.Database.DataAreaID, dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId);
                            }

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                data.deleteRedeemVoucher(dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }

                            data.updateVoucher(GME_Var.voucherTBSIBaru, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId);
                        }

                        if (GME_Var.isCash || GME_Var.isDisc)
                        {
                            vouchersList voucherList = new vouchersList();
                            DataTable redeemVoucherDT = new DataTable();

                            redeemVoucherDT = data.getRedeemVoucher(posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                voucherList = integrationService.updateVoucherRedeem(Connection.applicationLoc.Settings.Database.DataAreaID, dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId);
                            }

                            for (int i = 0; i < redeemVoucherDT.Rows.Count; i++)
                            {
                                DataRow dr = redeemVoucherDT.Rows[i];

                                data.deleteRedeemVoucher(dr["VOUCHERID"].ToString(), posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            }
                        }

                        //proses stoped  at req reward 9100
                        GME_Custom.GME_Data.queryDataOffline queryDataOffline = new GME_Custom.GME_Data.queryDataOffline();

                        //tambahan buat reprint struk
                        Dictionary<string, string> value = new Dictionary<string, string>();

                        int pointVisit = (GME_Custom.GME_Propesties.GME_Var.tempPointBalance - Convert.ToInt32(GME_Custom.GME_Propesties.GME_Var.memberPoint)) - GME_Custom.GME_Propesties.GME_Var.pointUsed;
                        string approvalCode = "";
                        string PanNumber = string.Empty;
                        if (GME_Var.approvalCodeBCA != string.Empty)
                        {
                            approvalCode = GME_Var.approvalCodeBCA;
                        }
                        else if (GME_Var.approvalCodeMandiri != string.Empty)
                        {
                            approvalCode = GME_Var.approvalCodeMandiri;
                        }
                        else if (GME_Var.payCardApprovalCodeOffline != string.Empty)
                        {
                            approvalCode = GME_Var.payCardApprovalCodeOffline;
                        }
                        //PAN
                        if (GME_Var.panBCA != string.Empty && GME_Var.panBCA.Length > 0)
                        {
                            PanNumber = "**" + GME_Var.panBCA.Substring(6);
                        }
                        else if (GME_Var.panMandiri != string.Empty && GME_Var.panMandiri.Length > 0)
                        {
                            PanNumber = "**" + GME_Var.panMandiri.Substring(6);
                        }
                        string totalPoint = string.Empty;
                        if (!GME_Var.isReedem)
                        {
                            totalPoint = GME_Var.tempPointBalance.ToString();
                        }
                        else
                        {
                            if (pointVisit >= 0)
                            {
                                pointVisit = pointVisit * -1;
                            }
                            int temp = Convert.ToInt32(GME_Var.tempPointBalance + pointVisit);
                            totalPoint = temp.ToString();
                        }
                        value.Add("POINT", Convert.ToInt32(GME_Custom.GME_Propesties.GME_Var.memberPoint.ToString()).ToString() + ";" + pointVisit.ToString() + ";" + totalPoint);
                        value.Add("DPP", GME_Custom.GME_Propesties.GME_Var.totalDPP.ToString());
                        value.Add("ApprovalCode", approvalCode);
                        value.Add("PAN", PanNumber);

                        //check connection to dbchannell
                        if (!GME_Var.pingStatus) //offline
                        {
                            queryDataOffline.insertOfflineSNF(posTransaction, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);

                            if (GME_Var.customerIdOffline != string.Empty)
                                data.updateReceiptCustomerTransTable(GME_Var.receiptID, GME_Var.customerIdOffline, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                            else if (GME_Var.customerData != null)
                                data.updateReceiptCustomerTransTable(GME_Var.receiptID, GME_Var.customerData.CustomerId, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                            else
                                data.updateReceiptCustomerTransTable(GME_Var.receiptID, "9999999999999999999", posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);

                            data.updateReceiptIdSalesTrans(GME_Var.receiptID, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);

                            data.updateRetailTransactionTable(value, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);

                            if (GME_Var.isPayCard)
                            {
                                if (GME_Var.isEDCBCA)
                                {
                                    data.updatePaymentTypeCard(GME_Var.invoiceNumberBCA, GME_Var.approvalCodeBCA, GME_Var.merchantIdBCA, string.Empty,
                                        posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                                }
                                else if (GME_Var.isEDCBCAOffline)
                                {
                                    data.updateApprovalCode(GME_Var.payCardApprovalCodeOffline, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString, posTransaction.TransactionId);

                                    data.updatePaymentTypeCard(string.Empty, GME_Var.payCardApprovalCodeOffline, string.Empty, string.Empty,
                                        posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                                }
                                else if (GME_Var.isEDCMandiriOffline)
                                {
                                    data.updateApprovalCode(GME_Var.payCardApprovalCodeOffline, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString, posTransaction.TransactionId);

                                    data.updatePaymentTypeCard(string.Empty, GME_Var.payCardApprovalCodeOffline, string.Empty, string.Empty,
                                        posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                                }
                                else if (GME_Var.isEDCMandiri)
                                {
                                    data.updatePaymentTypeCard(GME_Var.invoiceNumberMandiri, GME_Var.approvalCodeMandiri, GME_Var.merchantIdMandiri, string.Empty,
                                                                posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                                }
                            }
                        }
                        else
                        {
                            queryDataOffline.insertOfflineSNF(posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            if (GME_Var.customerIdOffline != string.Empty)
                                data.updateReceiptCustomerTransTable(GME_Var.receiptID, GME_Var.customerIdOffline, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            else if (GME_Var.customerData != null)
                                data.updateReceiptCustomerTransTable(GME_Var.receiptID, GME_Var.customerData.CustomerId, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            else
                                data.updateReceiptCustomerTransTable(GME_Var.receiptID, "9999999999999999999", posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            data.updateReceiptIdSalesTrans(GME_Var.receiptID, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                            data.updateRetailTransactionTable(value, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, posTransaction.TransactionId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);


                            if (GME_Var.isPayCard)
                            {
                                if (GME_Var.isEDCBCA)
                                {
                                    data.updatePaymentTypeCard(GME_Var.invoiceNumberBCA, GME_Var.approvalCodeBCA, GME_Var.merchantIdBCA, string.Empty,
                                        posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                }
                                else if (GME_Var.isEDCBCAOffline)
                                {
                                    data.updateApprovalCode(GME_Var.payCardApprovalCodeOffline, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId);

                                    data.updatePaymentTypeCard(string.Empty, GME_Var.payCardApprovalCodeOffline, string.Empty, string.Empty,
                                        posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                }
                                else if (GME_Var.isEDCMandiriOffline)
                                {
                                    data.updateApprovalCode(GME_Var.payCardApprovalCodeOffline, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId);

                                    data.updatePaymentTypeCard(string.Empty, GME_Var.payCardApprovalCodeOffline, string.Empty, string.Empty,
                                        posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                }
                                else if (GME_Var.isEDCMandiri)
                                {
                                    data.updatePaymentTypeCard(GME_Var.invoiceNumberMandiri, GME_Var.approvalCodeMandiri, GME_Var.merchantIdMandiri, string.Empty,
                                                                posTransaction.TransactionId, posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                                }
                            }
                        }
                    }
                }

                if (GME_Var.custEnrollType != string.Empty)
                {

                }

                if (GME_Var.DiscVoucher != 0)
                {
                    int cashVoucher = 0;
                    int discVoucher = 1;
                    int discountOrginType = 3; //Manual Discount
                    string channel;
                    channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                    if (GME_Var.iswelcomevoucher == true)
                    {
                        var kodepromoID = data.getKodePromoIDWelcome(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                        data.updateKodePromoID(kodepromoID, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId, discountOrginType);
                    }
                    if (GME_Var.isbirthdayvoucher == true)
                    {
                        if (GME_Var.isbirthdayvoucherCLUB == true)
                        {
                            var kodepromoIDCLUB = data.getKodePromoIDBirthdayCLUB(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            data.updateKodePromoID(kodepromoIDCLUB, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId, discountOrginType);
                        }
                        if (GME_Var.isbirthdayvoucherFAN == true)
                        {
                            var kodepromoIDFAN = data.getKodePromoIDBirthdayCLUB(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                            data.updateKodePromoID(kodepromoIDFAN, posTransaction.Shift.StoreId, posTransaction.Shift.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString, posTransaction.TransactionId, discountOrginType);
                        }
                    }

                    data.insertVoucherTransactionPull(cashVoucher, discVoucher, posTransaction, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                }

                GME_Method.setAllVariableToDefault();
            }*/
            #endregion
        }

        private lineItem setlineItem(int index, string productCode, string familyCode, int quantity, long productPrice)
        {
            lineItem lineItem = new lineItem();

            lineItem.index = index;
            lineItem.productCode = productCode;
            lineItem.familyCode = familyCode;
            lineItem.quantity = quantity;
            lineItem.productPrice = productPrice;
            lineItem.nonDiscountable = false;
            lineItem.rewardsExcluded = false;          

            return lineItem;
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

        private lineItems setLineItems(List<lineItem> listLineItem)
        {
            lineItems lineItems = new lineItems();
            lineItems.lineItem = new lineItem[listLineItem.Count];

            for (int i = 0; i < listLineItem.Count; i++)
            {               
                lineItems.lineItem[i] = listLineItem[i];                
            }

            return lineItems;
        }

        public void PreVoidTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreVoidTransaction", "Before voiding the transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostVoidTransaction(IPosTransaction posTransaction)
        {
            string source = "TransactionTriggers.PostVoidTransaction";
            string value = "After voiding the transaction...";
            LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Trace);
            LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);

            GME_Method.setAllVariableToDefault();
        }

        public void PreReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreReturnTransaction", "Before returning the transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostReturnTransaction(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PostReturnTransaction", "After returning the transaction...", LSRetailPosis.LogTraceLevel.Trace);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public void PreConfirmReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("TransactionTriggers.PreConfirmReturnTransaction", "Before confirming return transaction...", LogTraceLevel.Trace);
        }
    }
}
