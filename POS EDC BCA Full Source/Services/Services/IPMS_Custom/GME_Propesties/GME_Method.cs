using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSRetailPosis;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Loyalty;
using System.IO.Ports;
using GME_Custom;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_Data;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using System.Net.NetworkInformation;
using System.Threading;
using Microsoft.Dynamics.Retail.Pos.BlankOperations;
using GME_Custom.GME_EngageFALWSServices;
using Microsoft.PointOfService;
using System.Net;

namespace GME_Custom.GME_Propesties
{
    public class GME_Method
    {        
        public static string getAmountDecimal(string Amount)
        {
            decimal DecimalParse = 0;
            decimal.TryParse(Amount, out DecimalParse);
            return DecimalParse.ToString("#,###.00");
        }

        public static string generateReceiptNumberSequence(string transType, decimal amount)
        {
            string receiptId = string.Empty;
            GME_Method method = new GME_Method();

            if (transType == "Sales" && amount >= 0)
            {
                receiptId = method.receiptNumberSequenceTransSales();
            }
            else if (transType == "Sales" && amount < 0)
            {
                receiptId = method.receiptNumberSequenceReturnSales();
            }
            return receiptId;
        }

        private string receiptNumberSequenceTransSales()
        {
            string formatSequence;
            string sequence = "0";
            IApplication application = Connection.applicationLoc;
            string receiptId = string.Empty;
            int nextSequence = 0;
            int year = 0;

            queryData data = new queryData();

            int.TryParse(data.checkValidReceiptYear(application.Shift.StoreId, application.Shift.TerminalId, "1", application.Settings.Database.Connection.ConnectionString), out year);

            sequence = data.getNextReceiptNumberSequence(application.Shift.StoreId, application.Shift.TerminalId, "1", year.ToString(), application.Settings.Database.Connection.ConnectionString);

            if (year != DateTime.Now.Year)
            {
                sequence = "0";
            }
            year = DateTime.Now.Year;            

            if (sequence.Length == 1 || sequence.Length == 0)
            {
                formatSequence = "1" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "00000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 2)
            {
                formatSequence = "1" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "0000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 3)
            {
                formatSequence = "1" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 4)
            {
                formatSequence = "1" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "00";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 5)
            {
                formatSequence = "1" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "0";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 6)
            {
                formatSequence = "1" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "");
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }

            data.insertReceiptNumberSequence(application.Shift.StoreId, application.Shift.TerminalId, "1", "Sales", year, nextSequence, application.Settings.Database.Connection.ConnectionString);

            return receiptId;
        }

        private string receiptNumberSequenceReturnSales()
        {
            string formatSequence;
            string sequence = "0";
            IApplication application = Connection.applicationLoc;
            string receiptId = string.Empty;
            int nextSequence = 0;
            int year = 0;

            GME_Data.queryData data = new GME_Data.queryData();

            int.TryParse(data.checkValidReceiptYear(application.Shift.StoreId, application.Shift.TerminalId, "2", application.Settings.Database.Connection.ConnectionString), out year);

            sequence = data.getNextReceiptNumberSequence(application.Shift.StoreId, application.Shift.TerminalId, "2", year.ToString(), application.Settings.Database.Connection.ConnectionString);

            if (year != DateTime.Now.Year)
            {
                sequence = "0";
            }
            year = DateTime.Now.Year;                        

            if (sequence.Length == 1 || sequence.Length == 0)
            {
                formatSequence = "2" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "00000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 2)
            {
                formatSequence = "2" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "0000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 3)
            {
                formatSequence = "2" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 4)
            {
                formatSequence = "2" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "00";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 5)
            {
                formatSequence = "2" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "") + "0";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 6)
            {
                formatSequence = "2" + application.Shift.StoreId + application.Shift.TerminalId.Replace(application.Shift.StoreId + "-", "") + year.ToString().Replace("20", "");
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                receiptId = formatSequence + nextSequence.ToString();
            }

            data.insertReceiptNumberSequence(application.Shift.StoreId, application.Shift.TerminalId, "2", "Sales", year, nextSequence, application.Settings.Database.Connection.ConnectionString);

            return receiptId;
        }

        public static void setAllVariableToDefault()
        {
            GME_Var.isLogOff = false;            
            GME_Var.isPayTransaction = false;
            GME_Var.bonManualNumber = string.Empty;            
            GME_Var.isBonManual = false;
            GME_Var.reasonBonManual = string.Empty;
            GME_Var.cashierMngId = string.Empty;
            GME_Var.cashierMngName = string.Empty;
            GME_Var.custName = string.Empty;
            GME_Var.custEmail = string.Empty;
            GME_Var.custPhone = string.Empty;
            GME_Var.custGroup = string.Empty;
            GME_Var.isCustomerType = false;
            GME_Var.searchTerm = string.Empty;
            GME_Var.isPoint = false;
            GME_Var.isDonasi = false;
            GME_Var.customerData = null;
            GME_Var.customerPosTransaction = null;
            GME_Var.isRefreshCustomer = false;
            GME_Var.isSkipCustomerType = false;
            GME_Var.salesPerson = string.Empty;
            GME_Var.isChangeSalesPerson = false;
            GME_Var.isVoidTransaction = false;
            GME_Var.amountDonasi = 0;            
            GME_Var.isMallVoucherPay = false;
            GME_Var.hasSuspendTransaction = false;
            GME_Var.hasRecallTransaction = false;
            GME_Var.isVoucherPrinted = false; // add by mar unprint voucher
            GME_Var.isCardReplacement = false;
            GME_Var.isEmployeeNotMember = false;
            GME_Var.isFamilyNotMember = false;
            GME_Var.isPublicCustomer = false;
            GME_Var.isEnrollCustomer = false;
            GME_Var.isLYBCCustomer = false;
            GME_Var.isSearchCustomer = false;
            GME_Var.totalAmountEDC = 0;
            GME_Var.isEDCBCA = false;
            GME_Var.isEDCMandiri = false;
            GME_Var.responseEDCTemp = null;
            GME_Var.isPayCard = false;
            GME_Var.settlementBCAOnline = false;
            GME_Var.settlementMandiriOnline = false;
            GME_Var.isEDCBCAOffline = false;
            GME_Var.isEDCMandiriOffline = false;
            GME_Var.payCardApprovalCodeOffline = string.Empty;
            GME_Var.unPrintVouch = null;
            GME_Var.getVoucher = string.Empty;
            GME_Var.isActiveVoucher = false; //ADD BY RIZKI for testing active voucher
            GME_Var.payVoucherId = string.Empty; //ADD BY RIZKI for testing active voucher
            GME_Var.totalAmountBelanja = 0;
            GME_Var.isTBSIVoucherBaru = false;
            GME_Var.getvoucherTBSIBaru = null;
            GME_Var.voucherTBSIBaru = string.Empty;
            GME_Var.enrollCardMemberNum = string.Empty;
            GME_Var.enrollLYBCCardMemberNum = string.Empty;
            GME_Var.enrollGender = string.Empty;
            GME_Var.enrollBirthDate = DateTime.MinValue;
            GME_Var.enrollskinType = string.Empty;
            GME_Var.enrollType = 0;
            GME_Var.retailTransGlobal = null;
            GME_Var.customerId = string.Empty;
            GME_Var.custEnrollType = string.Empty;
            //GME_Var.pingStatus = false;
            GME_Var.isEnrollCustomerLYBC = false;
            GME_Var.custDetailReq0302 = false;
            GME_Var.req0302Response = false;
            GME_Var.pingFailedCount = 0;
            GME_Var.customerIdOffline = string.Empty;
            GME_Var.preTriggerResultPayCard = false;

            GME_Var.server = string.Empty;
            GME_Var.totalAmount = 0;
            GME_Var.isBirthdayProrate = false;
            GME_Var.isBirthdayNotProrate = false;
            GME_Var.percentageDisc = 0;
            GME_Var.netAmountBirthday = 0;

            GME_Var.paycardofflineamount = 0;
            GME_Var.isEDCOffline = false;
            GME_Var.printType = string.Empty;
            GME_Var.isGrantedCoupon = false;
            GME_Var.GratedCoupons.Clear();
            GME_Var.pointUsed = 0;
            GME_Var.cardReplacePersonIdOld = 0;
            GME_Var.cardReplaceCardTypeOld = 0;
            GME_Var.cardReplaceCreateDateOld = DateTime.MinValue;
            GME_Var.contactAbilitysubscriptionSMS = false;
            GME_Var.contactAbilitysubscriptionEmail = false;
            GME_Var.isEnrollGuest = false;
            GME_Var.isCustomerTypeSearch = false;
            GME_Var.isTenderCounted = false;
            GME_Var.isSNFTransaction = false;
            GME_Var.updateMemberResp = string.Empty;

            //EDC BCA               
            GME_Var.transTypeBCA = string.Empty;
            GME_Var.amountBCA = string.Empty;
            GME_Var.otherAmountBCA = string.Empty;
            GME_Var.panBCA = string.Empty;
            GME_Var.expiryBCA = string.Empty;
            GME_Var.respCodeBCA = string.Empty;
            GME_Var.approvalCodeBCA = string.Empty;
            GME_Var.dateBCA = string.Empty;

            GME_Var.timeBCA = string.Empty;
            GME_Var.merchantIdBCA = string.Empty;
            GME_Var.terminalIdBCA = string.Empty;
            GME_Var.invoiceNumberBCA = string.Empty;
            GME_Var.loopRespBCA = 1;
            GME_Var.payCardBCA = false;
            GME_Var.payCardBCAOffline = false;
            GME_Var.approvalOfflineBCA = string.Empty;
            //EDC BCA

            //EDC Mandiri
            GME_Var.transTypeMandiri = string.Empty;
            GME_Var.panMandiri = string.Empty;
            GME_Var.cardTypeMandiri = string.Empty;
            GME_Var.respCodeMandiri = string.Empty;
            GME_Var.approvalCodeMandiri = string.Empty;
            GME_Var.dateMandiri = string.Empty;
            GME_Var.timeMandiri = string.Empty;
            GME_Var.merchantIdMandiri = string.Empty;
            GME_Var.terminalIdMandiri = string.Empty;
            GME_Var.invoiceNumberMandiri = string.Empty;
            GME_Var.loopRespMandiri = 1;
            GME_Var.amounMandiri = string.Empty;
            GME_Var.payCardMandiri = false;
            GME_Var.payCardMandiriOffline = false;
            GME_Var.approvalOfflineMandiri = string.Empty;
            //EDC Mandiri

            GME_Var.publicPersonId = 0;
            GME_Var.publicIdentifier = string.Empty;
            GME_Var.publicHouseholdId = 0;
            GME_Var.enrollPersonId = 0;
            GME_Var.enrollIdentiferId = string.Empty;
            GME_Var.enrollHouseholdId = 0;
            GME_Var.UpdateCustHouseholdId = 0;
            GME_Var.isSuccessCreatedCust = false;
            GME_Var.cardReplacementNumber = string.Empty;

            /// <person>
            GME_Var.personId = 0;
            GME_Var.personFirstName = string.Empty;
            GME_Var.personLastName = string.Empty;
            GME_Var.personEmail = string.Empty;
            GME_Var.personPhone = string.Empty;
            GME_Var.personBirthdate = string.Empty;
            GME_Var.personGender = string.Empty;
            GME_Var.personSkinType = string.Empty;
            GME_Var.personHouseHoldId = 0;
            /// </person>      

            /// <lookupCard>
            GME_Var.lookupCardPersonId = 0;
            GME_Var.lookupCardNumber = string.Empty;
            GME_Var.lookupCardTier = string.Empty;
            GME_Var.lookupCardFirstName = string.Empty;
            GME_Var.lookupCardEmail = string.Empty;
            GME_Var.lookupCardPhoneNumber = string.Empty;
            GME_Var.lookupCardLYBC = false;
            GME_Var.lookupCardOnlineMode = false;
            GME_Var.lookupCardStatus = string.Empty;
            GME_Var.lookupCardLastName = string.Empty;
            GME_Var.lookupCardStatusCard = string.Empty;
            /// </lookupCard>

            /// <getBalance>
            GME_Var.memberPoint = 0;
            GME_Var.tempPointBalance = 0;
            /// </getBalance>
            /// 

            //identifier
            GME_Var.identifierCode = string.Empty;
            GME_Var.approvalCode = string.Empty;
            GME_Var.auditNumber = string.Empty;
            
            GME_Var.clearList();
            GME_Var.clearSNF();

            //DISKON & PROMO
            GME_Var.isExclusive = false;
            GME_Var.isCompounded = false;
            GME_Var.doneCheckItemDiscount = false;
            GME_Var.quantity = 0;

            //WELCOME VOUCHER
            GME_Var.isgetidentifiersukses = false;
            GME_Var.iswelcomevoucher = false;
            GME_Var.isgetitemWelcomeVoucher = false;
            GME_Var.iswelcomevoucher = false;
            GME_Var.DiscVoucher = 0;
            GME_Var.CashVoucher = 0;
            GME_Var.amountVouchTBSI = 0;
            GME_Var.isVoucherApplied = false;
            GME_Var.isgetitemUndiscounted = false;
            GME_Var.iswelcomevoucher50 = false;
            GME_Var.iswelcomevoucher100 = false;
            GME_Var.welcomePosTransaction = null;

            //VMS
            GME_Var.isgetitemUndiscounted = false;
            GME_Var.amountVouchTBSI = 0;
            GME_Var.isVouchTBSIApplied = false;
            GME_Var.amountDue = string.Empty;
            GME_Var.trxNumber = string.Empty;
            GME_Var.storeId = string.Empty;
            GME_Var.result9100 = string.Empty;
            GME_Var.isReedem = false;
            GME_Var.totalDPP = 0;
            GME_Var.amountTBSI = 0;
            GME_Var.totalPoint = 0;
            GME_Var.isCashVoucher = false;
            GME_Var.receiptID = string.Empty;
            GME_Var.partialPaymentEDC = false;
            GME_Var.isShowInfoPoint = false;
            GME_Var.isCashVoucher = false;
            GME_Var.isDiscVoucher = false;
            GME_Var.isCash = false;
            GME_Var.isDisc = false;
            GME_Var.isAktifVoucher = false;

            //BIRTHDAY VOUCHER
            GME_Var.isbirthdayvoucher = false;
            GME_Var.isbirthdayvoucherCLUB = false;
            GME_Var.isbirthdayvoucherFAN = false;
            GME_Var.isVoucherAppliedforBirthday = false;
            GME_Var.reprintValue.Clear();
            GME_Var.isReprint = false;
            GME_Var.isUpdatePointAX = false;

            //POINT PLUS PAY
            GME_Var.isPointUsed = false;

            //GWP
            GME_Var.iscekitemLine = false;
            GME_Var.isDiscountORApplied = false;

            GME_Var.listItemVoucher.Clear();
            GME_Var.ListAttLoginConfig.Clear();
        }

        public static void publicCustomerEngage(string name, string lastName, string email, string phoneNumber, IApplication application, IPosTransaction posTransaction)
        {
            IntegrationService integration = new IntegrationService();
            queryData data = new queryData();
            string channel = string.Empty;

            bool isUpdateIdentifier = false;
            bool isUpdatePerson = false;
            bool isUpdateHouseHold = false;
            bool isUpdateContactabilitySMS = false;
            bool isUpdateContactabilityEMAIL = false;

            if (integration.updateIdentifier(GME_Var.publicIdentifier, 5) == "Success")
            {
                isUpdateIdentifier = true;

                if (integration.updatePersonPublic(GME_Var.publicPersonId, phoneNumber, email, name, lastName) == "Success")                    
                {
                    isUpdatePerson = true;

                    if (integration.updateHousehold(GME_Var.publicHouseholdId, application.Shift.StoreId, email, phoneNumber) == "Success")
                    {
                        isUpdateHouseHold = true;

                        if (phoneNumber != string.Empty)
                        {
                            if (integration.updateContactabilitySMS(GME_Var.publicPersonId) == "Success")
                            {
                                isUpdateContactabilitySMS = true;
                            }
                        }

                        if (email != string.Empty)
                        {
                            if (integration.updateContactabilityEmail(GME_Var.publicPersonId) == "Success")
                            {
                                isUpdateContactabilityEMAIL = true;
                            }
                        }
                    }                    
                }                
            }

            channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);            

            string customerId = string.Empty;
            if (GME_Var.customerData != null)
                customerId = GME_Var.customerData.CustomerId;
            else
                customerId = GME_Var.customerIdOffline;

            if (channel == string.Empty)
            {
                channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);
                //SNF
                GME_Var.SetSNF("HOUSEHOLDID", GME_Var.personHouseHoldId.ToString());
                GME_Var.SetSNF("IDENTIFIERID", GME_Var.identifierCode);
                GME_Var.SetSNF("CUSTOMERID", GME_Var.customerData.CustomerId);
                GME_Var.SetSNF("STATUS", "1");
                GME_Var.SetSNF("EMAIL", email);
                GME_Var.SetSNF("FIRSTNAME", GME_Var.enrollPersonFirstName);
                GME_Var.SetSNF("LASTNAME", GME_Var.enrollPersonLastName);
                GME_Var.SetSNF("PHONENUMBER", GME_Var.enrollPersonPhoneNumber);
                GME_Var.SetSNF("PERSONID", GME_Var.personId.ToString());
                GME_Var.SetSNF("CHANNEL", channel);

                if (!isUpdateIdentifier || !isUpdatePerson || !isUpdateHouseHold || !isUpdateContactabilitySMS || !isUpdateContactabilityEMAIL)
                {
                    if (!isUpdateIdentifier) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEIDENTIFIER"); goto outThisFunc; }
                    if (!isUpdatePerson) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEPERSON"); goto outThisFunc; }
                    if (!isUpdateHouseHold) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEHOUSEHOLD"); goto outThisFunc; }

                    if (phoneNumber != string.Empty)
                    {
                        if (!isUpdateContactabilitySMS) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYSMS"); goto outThisFunc; }
                    }

                    if (email != string.Empty)
                    {
                        if (!isUpdateContactabilityEMAIL) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYEMAIL"); goto outThisFunc; }
                    }

                    outThisFunc:;
                }
            }
        }

        public static void enrollmentCustomerEngage(string cardMemberNum, string firstName, string lastName, string gender, string email, string phoneNumber,
            DateTime birthDate, string skinType, IApplication application, IPosTransaction posTransaction)
        {
            IntegrationService integration = new IntegrationService();
            queryData data = new queryData();
            string channel = string.Empty;

            bool isCreateIdentifier = false;
            bool isUpdateIdentifier = false;
            bool isUpdatePerson = false;
            bool isUpdateHouseHold = false;
            bool isUpdateContactabilitySMS = false;
            bool isUpdateContactabilityEMAIL = false;

            if (integration.createIdentifier(10, cardMemberNum) == "Success")
            {
                isCreateIdentifier = true;
                if (integration.updateIdentifier(GME_Var.enrollIdentiferId, 10) == "Success")
                {
                    isUpdateIdentifier = true;
                    if (integration.updatePersonEnrollment(GME_Var.enrollPersonId, phoneNumber, email, firstName, birthDate, gender, lastName, skinType) == "Success")
                    {
                        isUpdatePerson = true;
                        if (integration.updateHousehold(GME_Var.enrollHouseholdId, application.Shift.StoreId, email, phoneNumber) == "Success")
                        {
                            isUpdateHouseHold = true;

                            if (phoneNumber != string.Empty)
                            {
                                if (integration.updateContactabilitySMS(GME_Var.enrollPersonId) == "Success")
                                {
                                    isUpdateContactabilitySMS = true;
                                }
                            }

                            if (email != string.Empty)
                            {
                                if (integration.updateContactabilityEmail(GME_Var.enrollPersonId) == "Success")
                                {
                                    isUpdateContactabilityEMAIL = true;
                                }
                            }
                        }
                    }
                }
            }

            channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

            if (channel == string.Empty)
                channel = data.getChannelStore(posTransaction.StoreId, Connection.applicationLoc.Settings.Database.OfflineConnection.ConnectionString);

            string customerId = string.Empty;
            if (GME_Var.customerData != null)
                customerId = GME_Var.customerData.CustomerId;
            else
                customerId = GME_Var.customerIdOffline;

            if (!data.insertNewCustomerDataExtend(customerId, gender, skinType, birthDate, posTransaction.StoreId,
            channel, posTransaction.TransactionId, posTransaction.TerminalId, application.Settings.Database.Connection.ConnectionString))
            {
                ///DB offline
                data.insertNewCustomerDataExtend(customerId, gender, skinType, birthDate, posTransaction.StoreId,
                channel, posTransaction.TransactionId, posTransaction.TerminalId, application.Settings.Database.OfflineConnection.ConnectionString);

                //SNF
                GME_Var.SetSNF("HOUSEHOLDID", GME_Var.personHouseHoldId.ToString());
                GME_Var.SetSNF("IDENTIFIERID", GME_Var.identifierCode);
                GME_Var.SetSNF("CUSTOMERID", GME_Var.customerData.CustomerId);
                GME_Var.SetSNF("GENDER", gender);
                GME_Var.SetSNF("SKINTYPE", skinType);
                GME_Var.SetSNF("BIRTHDAY", birthDate.ToString("yyyyMMdd"));
                GME_Var.SetSNF("STATUS", "1");
                GME_Var.SetSNF("EMAIL", email);
                GME_Var.SetSNF("FIRSTNAME", GME_Var.enrollPersonFirstName);
                GME_Var.SetSNF("LASTNAME", GME_Var.enrollPersonLastName);
                GME_Var.SetSNF("PHONENUMBER", GME_Var.enrollPersonPhoneNumber);
                GME_Var.SetSNF("PERSONID", GME_Var.personId.ToString());
                GME_Var.SetSNF("CHANNEL", channel);
            }

            if (!isCreateIdentifier || !isUpdateIdentifier || !isUpdatePerson || !isUpdateHouseHold || !isUpdateContactabilitySMS || !isUpdateContactabilityEMAIL)
            {
                if (!isCreateIdentifier) { GME_Var.SetSNF("PROCESSSTOPPED", "CREATEIDENTIFIER"); goto outThisFunc; }
                if (!isUpdateIdentifier) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEIDENTIFIER"); goto outThisFunc; }
                if (!isUpdatePerson) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEPERSON"); goto outThisFunc; }
                if (!isUpdateHouseHold) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATEHOUSEHOLD"); goto outThisFunc; }

                if (phoneNumber != string.Empty)
                {
                    if (!isUpdateContactabilitySMS) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYSMS"); goto outThisFunc; }
                }

                if (email != string.Empty)
                {
                    if (!isUpdateContactabilityEMAIL) { GME_Var.SetSNF("PROCESSSTOPPED", "UPDATECONTACTBILITYEMAIL"); goto outThisFunc; }
                }

                outThisFunc:;
            }
            
        }

        private static System.Timers.Timer _timer;
        private static System.Timers.Timer _timerHO;

        public static void startTimer()
        {
            _timer = new System.Timers.Timer(5000);            
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(pingToServer);
            _timer.Start();
            pingToServer(_timer, null);
        }

        public static void startTimerHO()
        {
            _timerHO = new System.Timers.Timer(5000);
            _timerHO.Elapsed += new System.Timers.ElapsedEventHandler(pingToServerHO);
            _timerHO.Start();
            pingToServerHO(_timerHO, null);
        }

        public static void pingToServer(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Ping p = new Ping();
                string str = "t";
                if (GME_Var.server != string.Empty && GME_Var.server != null)
                {
                    var pingResponce = p.Send(GME_Var.server, 1000, Encoding.ASCII.GetBytes(str));
                    DateTime value = DateTime.Now;

                    if (pingResponce.Status == IPStatus.Success)
                    {
                        Console.WriteLine("Address:{0}, RoundTrip:{1}, TTL:{2}, Status:{3}", pingResponce.Address, pingResponce.RoundtripTime, pingResponce.Options.Ttl, pingResponce.Status);
                        GME_Var.pingFailedCount = 0;
                        GME_Var.pingStatus = true;
                    }
                    else if (pingResponce.Status == IPStatus.TimedOut)
                    {
                        GME_Var.pingFailedCount += 1;

                        if (GME_Var.pingFailedCount > 9)
                            GME_Var.pingStatus = false;
                    }
                    else
                    {
                        Console.WriteLine("Error Occurred ...!!!");

                        GME_Var.pingFailedCount += 1;

                        if (GME_Var.pingFailedCount > 9)
                            GME_Var.pingStatus = false;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occurred ...!!!");
                GME_Var.pingStatus = false;
            }
        }

        public static void pingToServerHO(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Ping p = new Ping();
                string str = "t";
                if (GME_Var._EngageHOServer != string.Empty && GME_Var._EngageHOServer != null)
                {
                    var pingResponce = p.Send(GME_Var._EngageHOServer, 1000, Encoding.ASCII.GetBytes(str));
                    DateTime value = DateTime.Now;

                    if (pingResponce.Status == IPStatus.Success)
                    {
                        Console.WriteLine("Address:{0}, RoundTrip:{1}, TTL:{2}, Status:{3}", pingResponce.Address, pingResponce.RoundtripTime, pingResponce.Options.Ttl, pingResponce.Status);
                        GME_Var._pingFailedEngage = 0;
                        GME_Var._EngageHOStatus = true;
                    }
                    else if (pingResponce.Status == IPStatus.TimedOut)
                    {
                        GME_Var._pingFailedEngage += 1;

                        if (GME_Var._pingFailedEngage > 4)
                            GME_Var._EngageHOStatus = false;
                    }
                    else
                    {
                        Console.WriteLine("Error Occurred ...!!!");

                        GME_Var._pingFailedEngage += 1;

                        if (GME_Var._pingFailedEngage > 4)
                            GME_Var._EngageHOStatus = false;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occurred ...!!!");
                GME_Var._EngageHOStatus = false;
            }
        }


        public static void readConfig()
        {
            // Create an XML reader for this file.
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server.cfg")))
            {
                while (reader.Read())
                {
                    string attribute = null;
                    switch (reader.Name)
                    {
                        case "hostStore":
                             attribute = reader["address"];
                            if (attribute != null)
                            {
                                GME_Var.server = attribute;
                            }
                            break;
                        case "hostEngageHO":
                            attribute = reader["address"];
                            if (attribute != null)
                            {
                                GME_Var._EngageHOServer = attribute;
                            }
                            break;
                        case "AifLoginSetting":                         
                            attribute = reader["domainname"];
                            if (attribute != null)
                            {
                                GME_Var.ListAttLoginConfig.Add(attribute);
                            }
                            attribute = reader["username"];
                            if (attribute != null)
                            {
                                GME_Var.ListAttLoginConfig.Add(attribute);
                            }
                            attribute = reader["password"];
                            if (attribute != null)
                            {
                                GME_Var.ListAttLoginConfig.Add(attribute);
                            }
                            break;

                    }
                }
            }

        }

        public static ticket setTicket(string ticketNumber, string ticketDate, lineItems lineItems, int type = 9100, vouchersDto vouchers = null)
        {
            ticket ticket = new ticket();
            ticket.ticketNumber = ticketNumber;
            ticket.ticketDate = ticketDate;

            if (vouchers != null)
            {
                ticket.vouchers = vouchers;
            }
            if (type == 9100)
            {
                ticket.lineItems = lineItems;
            }


            return ticket;
        }


        public static paymentMethod setPaymentMethod(string paymentMode, string amount)
        {
            //jika bukan point ?
            if (paymentMode != "PTS")
            {
                //amount = String.Format("{0:0.00}", amount);
                amount = amount.Replace(",", string.Empty);
                amount = amount.Replace(".", string.Empty);
            }

            paymentMethod paymentMethod = new paymentMethod();
            paymentMethod.paymentMode = paymentMode;
            paymentMethod.amount = Convert.ToInt64(amount);


            return paymentMethod;
        }

        public static paymentMethods setPaymentMethods(string auditNumber, int newPointBalance, List<paymentMethod> listPaymentMethod)
        {
            paymentMethods paymentMethods = new paymentMethods();
            paymentMethods.paymentMethod = new paymentMethod[listPaymentMethod.Count];

            paymentMethods.auditNumber = auditNumber;
            paymentMethods.newPointsBalance = newPointBalance;
            paymentMethods.newNumberIssuedCoupons = 0;
            paymentMethods.numberPrintedCoupons = 0;
            for (int i = 0; i < listPaymentMethod.Count; i++)
            {
                paymentMethods.paymentMethod[i] = listPaymentMethod[i];
            }


            return paymentMethods;
        }

        public static string customerIdNumberSequence()
        {
            string formatSequence;
            string sequence = "0";
            IApplication application = Connection.applicationLoc;
            string customerId = string.Empty;
            int nextSequence = 0;
            

            queryData dataOffline = new queryData();            

            if (GME_Var.pingStatus)
                sequence = dataOffline.getNextCustomerIdSequence(application.Shift.StoreId, application.Shift.TerminalId, application.Settings.Database.Connection.ConnectionString);
            else
                sequence = dataOffline.getNextCustomerIdSequence(application.Shift.StoreId, application.Shift.TerminalId, application.Settings.Database.OfflineConnection.ConnectionString);

            if (sequence.Length == 1 || sequence.Length == 0)
            {
                formatSequence = "000";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                customerId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 2)
            {
                formatSequence = "00";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                customerId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 3)
            {
                formatSequence = "0";
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                customerId = formatSequence + nextSequence.ToString();
            }
            else if (sequence.Length == 4)
            {                
                int.TryParse(sequence, out nextSequence);
                nextSequence = nextSequence + 1;
                customerId = nextSequence.ToString();
            }

            if (GME_Var.pingStatus)
                dataOffline.insertCustomerOfflineSequence(application.Shift.StoreId, application.Shift.TerminalId, nextSequence, customerId, application.Settings.Database.Connection.ConnectionString);
            else
                dataOffline.insertCustomerOfflineSequence(application.Shift.StoreId, application.Shift.TerminalId, nextSequence, customerId, application.Settings.Database.OfflineConnection.ConnectionString);

            return customerId;
        }

        #region print voucher


        private static void ConnectToPrinter(PosPrinter printer)
        {
            printer.Open();
            printer.Claim(10000);
            printer.DeviceEnabled = true;
        }

        private static void DisconnectFromPrinter(PosPrinter printer)
        {
            printer.Release();
            printer.Close();
        }

        private static PosPrinter GetReceiptPrinter()
        {

            PosExplorer posExplorer = new PosExplorer();
            DeviceCollection devices = posExplorer.GetDevices((DeviceCompatibilities)Enum.Parse(typeof(DeviceCompatibilities), "Opos", false));
            DeviceInfo receiptPrinterDevice = null;
            foreach (DeviceInfo deviceInfo in devices)
            {
                receiptPrinterDevice = deviceInfo;
            }

            return (PosPrinter)posExplorer.CreateInstance(receiptPrinterDevice);
        }


        private static void PrintTextLine(PosPrinter printer, string text)
        {
            printer.PrintNormal(PrinterStation.Receipt, text + Environment.NewLine);
        }



        /// <summary>
        /// PRINT VOUCHER
        /// </summary>
        /// <param name="HTMLCode"></param>
        /// <returns></returns>
        private static string HTMLToText(string HTMLCode)
        {
            string getHeader = string.Empty;
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);

            //Decode html specific characters
            HTMLCode = System.Net.WebUtility.HtmlDecode(HTMLCode);

            //Replace <br /> with line breaks
            HTMLCode = lineBreakRegex.Replace(HTMLCode, Environment.NewLine);

            return HTMLCode;
        }


        private static string HTMLToTextperLine(string HTMLCode)
        {
            string getHeader = string.Empty;


            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
            var removeLineSpace = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            // var textloc = html;

            //Decode html specific characters
            HTMLCode = System.Net.WebUtility.HtmlDecode(HTMLCode);
            //Remove tag whitespace/line breaks
            HTMLCode = tagWhiteSpaceRegex.Replace(HTMLCode, "><");
            //Replace <br /> with line breaks
            HTMLCode = lineBreakRegex.Replace(HTMLCode, Environment.NewLine);
            //Strip formatting
            HTMLCode = stripFormattingRegex.Replace(HTMLCode, string.Empty);

            HTMLCode = removeLineSpace.Replace(HTMLCode, string.Empty);

            // Finally, remove all HTML tags and return plain text
            return HTMLCode;

        }

        /// <summary>
        /// voucher in html
        /// </summary>
        /// <param name="voucherTxt"></param>
        /// <returns></returns>
        /// 


        private static List<string> listContain = new List<string>();
        private static List<string> listAlignment = new List<string>();
        private static List<int> listBoldStyle = new List<int>();


        public static bool PrintReceiptPage(string voucherTxt)
        {
            bool isSuccess = false;

            String Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
            String Underline = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'2', (byte)'u', (byte)'C' });
            String Italic = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'i', (byte)'C' });
            String CenterAlign = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' });
            String RightAlign = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'r', (byte)'A' });
            String DoubleWideCharacters = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'2', (byte)'C' });
            String DoubleHightCharacters = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'3', (byte)'C' });
            String DoubleWideAndHightCharacters = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'4', (byte)'C' });

            stripHTML(HTMLToText(voucherTxt));

            PosPrinter printer = GetReceiptPrinter();
            try
            {

                ConnectToPrinter(printer);
                for (int i = 1; i < listContain.Count; i++)
                {
                    bool isBoldStyle = false;
                    switch (listAlignment[i])
                    {
                        case "center":
                            for (int j = 0; j < listBoldStyle.Count; j++)
                            {
                                if (listBoldStyle[j] == i)
                                {
                                    PrintTextLine(printer, CenterAlign + Bold + listContain[i]);
                                    isBoldStyle = true;
                                    break;
                                }

                            }
                            if (!isBoldStyle)
                            {
                                PrintTextLine(printer, CenterAlign + listContain[i]);
                            }
                            break;
                        case "left":
                            for (int j = 0; j < listBoldStyle.Count; j++)
                            {
                                if (listBoldStyle[j] == i)
                                {
                                    PrintTextLine(printer, Bold + listContain[i]);
                                    isBoldStyle = true;
                                    break;
                                }
                            }
                            if (!isBoldStyle)
                            {
                                PrintTextLine(printer, listContain[i]);
                            }
                            break;
                    }


                }

                printer.PrintBarCode(PrinterStation.Receipt,
              listContain[0].Substring(listContain[0].IndexOf(':') + 1).Trim(), BarCodeSymbology.Code128,
              80, 80, PosPrinter.PrinterBarCodeCenter, BarCodeTextPosition.None);
                PrintTextLine(printer, CenterAlign + listContain[0].Substring(listContain[0].IndexOf(':') + 1).Trim());

                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);

                isSuccess = true;
                printer.CutPaper(100);
                return isSuccess;

            }
            finally
            {
                DisconnectFromPrinter(printer);
            }            
        }
        
        private static void stripHTML(string voucherTxt)
        {
            listContain.Clear();
            listAlignment.Clear();
            listBoldStyle.Clear();

            string mainHeader = string.Empty;
            string compName = string.Empty;
            string substrings = string.Empty;// = value.Split(delimiter);
            string getBarcode = string.Empty;
            string perLines = string.Empty;
            int countNot = 0;

            string getAlignment = string.Empty;
            string prevAlignment = string.Empty;
            bool printSpace = false;
            bool nameFound = false;
            int getNumLines = voucherTxt.Split('\n').Length - 1;

            for (int countLines = 1; countLines <= getNumLines; countLines++)
            {
                perLines = voucherTxt.Split('\n').GetValue(countLines).ToString();

                if (perLines != string.Empty)
                {

                    if (perLines.Contains("BARCODE"))
                    {
                        perLines = HTMLToTextperLine(perLines);
                        listContain.Add(perLines);
                        listAlignment.Add("center");
                    }

                    else
                    {

                        if (perLines.Contains("align") || prevAlignment != string.Empty)
                        {

                            if (perLines.Contains("/div"))
                                printSpace = false;

                            else
                                printSpace = true;

                            if (perLines.Contains("Name:"))
                            {
                                nameFound = true;
                            }

                            if (perLines.Contains("center") || (prevAlignment == "center" && !perLines.Contains("left")))
                            {
                                prevAlignment = ("center");
                                if (perLines.Contains("<b>"))
                                {
                                    listBoldStyle.Add(listContain.Count);
                                }
                                perLines = HTMLToTextperLine(perLines);

                                if (printSpace)
                                {
                                    if (nameFound)
                                    {
                                        listContain.Add(perLines);
                                        listAlignment.Add("center");
                                    }
                                    else
                                    {
                                        if (perLines != string.Empty)
                                        {
                                            countNot++;
                                        }
                                        if (countNot == 5)
                                        {
                                            listContain.Add(perLines);
                                            listAlignment.Add("center");
                                        }
                                        else if (countNot <= 2)
                                        {

                                            listContain.Add(perLines);
                                            listAlignment.Add("center");
                                        }
                                        else if (countNot == 3)
                                        {
                                            listContain.Add(perLines);
                                            listAlignment.Add("center");
                                        }
                                        else if (countNot == 4)
                                        {
                                            listContain.Add(perLines);
                                            listAlignment.Add("center");
                                        }
                                        else
                                        {
                                            listContain.Add(perLines);
                                            listAlignment.Add("center");
                                        }
                                    }
                                }
                            }
                            else if (perLines.Contains("left") || (prevAlignment == "left" && !perLines.Contains("center")))
                            {
                                prevAlignment = ("left");
                                if (perLines.Contains("<b>"))
                                {
                                    listBoldStyle.Add(listContain.Count);
                                }
                                perLines = HTMLToTextperLine(perLines);

                                if (printSpace)
                                {
                                    listContain.Add(perLines);
                                    listAlignment.Add("left");
                                }
                            }

                        }


                    }
                }
            }
        }

        public static void printSuspendTransaction(string transactionId, string register, string salesID, string salesName)
        {            

            String Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
            String Underline = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'2', (byte)'u', (byte)'C' });
            String Italic = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'i', (byte)'C' });
            String CenterAlign = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' });
            String RightAlign = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'r', (byte)'A' });
            String DoubleWideCharacters = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'2', (byte)'C' });
            String DoubleHightCharacters = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'3', (byte)'C' });
            String DoubleWideAndHightCharacters = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'4', (byte)'C' });            

            PosPrinter printer = GetReceiptPrinter();

            try
            {
                ConnectToPrinter(printer);

                PrintTextLine(printer, CenterAlign + Bold + "SUSPEND TRANSACTION");

                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);

                PrintTextLine(printer, "Transaction ID : " + transactionId);
                PrintTextLine(printer, "POS Number     : " + register);
                PrintTextLine(printer, "Sales ID       : " + salesID);
                PrintTextLine(printer, "Sales Name     : " + salesName);

                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);

                printer.PrintBarCode(PrinterStation.Receipt,
                transactionId.Trim(), BarCodeSymbology.Code128,
                80, 80, PosPrinter.PrinterBarCodeCenter, BarCodeTextPosition.None);

                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);

                printer.CutPaper(100);
            }
            finally
            {
                DisconnectFromPrinter(printer);
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
    #endregion
}
