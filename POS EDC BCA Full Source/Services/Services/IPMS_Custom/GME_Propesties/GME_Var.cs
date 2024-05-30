using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using System.ComponentModel.Composition;
using GME_Custom;
using System.Threading;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest; 
using System.ComponentModel;
using System.IO.Ports;
using GME_Custom.GME_EngageFALWSServices;
using GME_Custom.GME_EngageServices;

namespace GME_Custom.GME_Propesties
{
    public class GME_Var
    {
        public static bool isLogOff;
        public static bool moreShift = false;
        public static bool isPayTransaction = false;
        public static string bonManualNumber = string.Empty;
        public static string dataAreaId = "usrt";
        public static bool isBonManual = false;
        public static string reasonBonManual = string.Empty;
        public static string msgBoxCustomerInfo = "Do you want back to customer type ?";
        public static string msgBoxClosingBonManual = "Input apabila terdapat transaksi bon manual";
        public static string cashierMngId = string.Empty;
        public static string cashierMngName = string.Empty;
        public static string custName = string.Empty;
        public static string custEmail = string.Empty;
        public static string custPhone = string.Empty;
        public static string custGroup = string.Empty;
        public static bool isCustomerType = false;
        public static string searchTerm = string.Empty;
        public static string msgBoxEmployePurchase = "Nomor induk karyawan tidak ditemukan";
        public static string msgBoxStorePointPay = "Apakah anda ingin membayar dengan point ?";
        public static string msgBoxStoreDonasi = "Apakah anda ingin melakukan donasi ?";
        public static bool isPoint = false;
        public static bool isDonasi = false;
        public static ICustomer customerData = null;
        public static IPosTransaction customerPosTransaction = null;
        public static bool isRefreshCustomer = false;
        public static string msgBoxRefreshCustomer = "Tidak ada data pelanggan.";
        public static bool isSkipCustomerType = false;
        public static string msgBoxFamilyPurchase = "Nomor keluarga tidak ditemukan";
        public static string salesPerson = string.Empty;
        public static bool isChangeSalesPerson = false;
        public static bool isVoidTransaction = false;
        public static decimal amountDonasi = 0;
        public static string itemDonasi = "3610000";
        public static bool isMallVoucherPay = false;
        public static bool hasSuspendTransaction = false;
        public static bool hasRecallTransaction = false;
        public static string msgBoxSuspendBlindCloseShift = "Masih ada transaksi yang di suspend." + System.Environment.NewLine
                                                                + "Tolong selesaikan terlebih dahulu transaksi tersebut";
        public static string msgUnprintVoucher = "Voucher berhasil di print"; // add by mar
        public static string msgUnprintVoucherError = "Data voucher sudah tidak ada. Semua voucher telah digunakan atau di print"; // add by mar
        public static string memberInfoLink = "http://aloe.thebodyshop.co.id/MemberActivities/";  // add by mar 
        public static bool isVoucherPrinted = false; // add by mar unprint voucher
        public static bool isCardReplacement = false;
        public static bool isEmployeeNotMember = false;
        public static bool isFamilyNotMember = false;
        public static bool isPublicCustomer = false;
        public static bool isEnrollCustomer = false;
        public static bool isLYBCCustomer = false;
        public static bool isSearchCustomer = false;
        public static decimal totalAmountEDC = 0;
        public static bool isEDCBCA = false;
        public static bool isEDCMandiri = false;
        public static DataTable responseEDCTemp = null;
        public static bool isPayCard = false;
        public static bool settlementBCAOnline = false;
        public static bool settlementMandiriOnline = false;
        public static bool isEDCBCAOffline = false;
        public static bool isEDCMandiriOffline = false;
        public static string payCardApprovalCodeOffline = string.Empty;
        public static string[] unPrintVouch = null;  // add by mar unprint voucher
        public static string getVoucher = string.Empty;  // add by mar unprint voucher        
        public static bool isActiveVoucher = false; //ADD BY RIZKI for testing active voucher
        public static string payVoucherId = string.Empty; //ADD BY RIZKI for testing active voucher
        public static decimal totalAmountBelanja = 0;
        public static bool isApplyDiscount = false;
        public static bool isTBSIVoucherBaru = false;
        public static string[] getvoucherTBSIBaru = null;
        public static string voucherTBSIBaru = string.Empty;
        public static string enrollCardMemberNum = string.Empty;
        public static string enrollLYBCCardMemberNum = string.Empty;
        public static string enrollGender = string.Empty;
        public static DateTime enrollBirthDate = DateTime.MinValue;
        public static string enrollskinType = string.Empty;
        public static int enrollType = 0;
        public static IRetailTransaction retailTransGlobal = null;
        public static string customerId = string.Empty;
        public static string custEnrollType = string.Empty;
        public static bool pingStatus = true; //for testing not have databse offline set true
        public static bool isEnrollCustomerLYBC = false;
        public static bool custDetailReq0302 = false;
        public static bool req0302Response = false;
        public static int pingFailedCount = 0;
        public static string customerIdOffline = string.Empty;
        public static bool preTriggerResultPayCard = false;
        public static string server = string.Empty;
        public static bool isEDCOffline = false;
        public static string unprintVoucher = string.Empty;
        public static string printType = string.Empty;
        public static bool isEmployeePurch = false;
        public static bool isGrantedCoupon = false;
        public static List<string> GratedCoupons = new List<string>();
        public static int pointUsed = 0;
        public static decimal cardReplacePersonIdOld = 0;
        public static short cardReplaceCardTypeOld = 0;
        public static DateTime cardReplaceCreateDateOld = DateTime.MinValue;
        public static bool contactAbilitysubscriptionSMS = false;
        public static bool contactAbilitysubscriptionEmail = false;
        public static bool isEnrollGuest = false;
        public static bool isCustomerTypeSearch = false;
        public static bool isTenderCounted = false;
        public static bool isSNFTransaction = false;
        public static string updateMemberResp = string.Empty;

        //EDC BCA
        public static SerialPort serialBCA = new SerialPort();
        public static string transTypeBCA = string.Empty;
        public static string amountBCA = string.Empty;
        public static string otherAmountBCA = string.Empty;
    //    public static string panBCA = string.Empty;   
      //  public static string expiryBCA = string.Empty;
        public static string panBCA = "5409120012345684".PadRight(19, ' ');  
        public static string expiryBCA = "2510";

        public static string respCodeBCA = string.Empty;
        public static string approvalCodeBCA = string.Empty;
        public static string dateBCA = string.Empty;
        public static string timeBCA = string.Empty;
        public static string merchantIdBCA = string.Empty;
        public static string terminalIdBCA = string.Empty;
        public static string invoiceNumberBCA = string.Empty;
        public static int loopRespBCA = 1;
        public static bool payCardBCA = false;
        public static bool payCardBCAOffline = false;
        public static string approvalOfflineBCA = string.Empty;
        public static string refNumberBCA = string.Empty;//add by Yonathan 2 Dec 2022
        //EDC BCA

        //EDC Manidiri
        public static SerialPort serialMandiri = new SerialPort();
        public static string transTypeMandiri = string.Empty;
        public static string panMandiri = string.Empty;
        public static string cardTypeMandiri = string.Empty;
        public static string respCodeMandiri = string.Empty;
        public static string approvalCodeMandiri = string.Empty;
        public static string dateMandiri = string.Empty;
        public static string timeMandiri = string.Empty;
        public static string merchantIdMandiri = string.Empty;
        public static string terminalIdMandiri = string.Empty;
        public static string invoiceNumberMandiri = string.Empty;
        public static int loopRespMandiri = 1;
        public static string amounMandiri = string.Empty;
        public static bool payCardMandiri = false;
        public static bool payCardMandiriOffline = false;
        public static string approvalOfflineMandiri = string.Empty;
        //EDC Mandiri

        public static decimal publicPersonId { get; set; }
        public static string publicIdentifier { get; set; }
        public static decimal publicHouseholdId { get; set; }
        public static decimal enrollPersonId { get; set; }
        public static string enrollIdentiferId { get; set; }
        public static decimal enrollHouseholdId { get; set; }
        public static decimal UpdateCustHouseholdId { get; set; }
        public static bool isSuccessCreatedCust = false;
        public static string msgCardReplacementNewCardValidation = "New card number harus diisi"; // add by mar
        public static string cardReplacementNumber { get; set; }
        public static string enrollPersonFirstName { get; set; }
        public static string enrollPersonLastName { get; set; }
        public static string enrollPersonPhoneNumber { get; set; }

        /// <person>
        public static decimal personId { get; set; }
        public static string personFirstName { get; set; }
        public static string personLastName { get; set; }
        public static string personEmail { get; set; }
        public static string personPhone { get; set; }
        public static string personBirthdate { get; set; }
        public static string personGender { get; set; }
        public static string personSkinType { get; set; }
        public static decimal personHouseHoldId { get; set; }
        /// </person>      

        /// <lookupCard>
        public static decimal lookupCardPersonId { get; set; }
        public static string lookupCardNumber { get; set; }
        public static string lookupCardTier { get; set; }
        public static string lookupCardFirstName { get; set; }
        public static string lookupCardEmail { get; set; }
        public static string lookupCardPhoneNumber { get; set; }
        public static bool lookupCardLYBC = false;
        public static bool lookupCardOnlineMode = false;
        public static string lookupCardStatus = string.Empty;
        public static string lookupCardLastName { get; set; }
        public static string lookupCardStatusCard = string.Empty;
        /// </lookupCard>

        /// <getBalance>
        public static double memberPoint { get; set; }
        /// </getBalance>     
        /// 

        //birthday voucher
        public static decimal totalAmount = 0;
        public static voucherCheckDTO[] vouchersResp { get; set; }
        public static string identifierCardType { get; set; }
        public static bool isBirthdayProrate = false;
        public static bool isBirthdayNotProrate = false;
        public static decimal birthdayAmountDisc { get; set; }
        public static decimal percentageDisc = 0;
        public static decimal netAmountBirthday = 0;


        //paycard offline
        public static decimal paycardofflineamount = 0;

        //point
        public static lineItems lineItems = new lineItems();
        public static string amountDue = string.Empty;
        public static string trxNumber = string.Empty;
        public static string storeId = string.Empty;
        public static string result9100 = string.Empty;


        public static bool isCardReplacementSuccess = false;

        public static string identifierCode { get; set; }
        public static string approvalCode { get; set; }

        public static bool isReedem = false;
        public static decimal totalDPP = 0;
        public static decimal amountTBSI = 0;
        public static int totalPoint = 0;        
        public static bool isContinueVoucherCasPayment = false;
        public static string receiptID = string.Empty;
        public static bool partialPaymentEDC = false;
        public static bool isShowInfoPoint = false;

        public static Dictionary<string, string> RespEDCMandiri = new Dictionary<string, string>();
        public static Dictionary<string, string> RespEDCBCA = new Dictionary<string, string>();
        public static Dictionary<string, List<decimal>> tenderList = new Dictionary<string, List<decimal>>();


        //request reward
        private static List<string> paymentMethod = new List<string>();


        private static List<voucherDto> listVoucher = new List<voucherDto>();

        public static List<voucherDto> listVouchers
        {
            get { return listVoucher; }
            set { listVoucher = value; }
        }

        public static string[] fieldSNF = { "BIRTHDAY", "CASHIERID", "CHANNEL", "CURRENCYCODE", "CUSTOMERID", "EMAIL", "FAMILYCODE", "FIRSTNAME", "GENDER", "HOUSEHOLDID", "IDENTIFIERID", "LASTNAME", "PERSONID", "PHONENUMBER", "PROCESSSTOPPED", "PRODUCTCODE", "QTY", "REPLICATIONCOUNTER", "DATAAREAID", "SKINTYPE", "STORE", "TERMINAL", "TRANSACTIONDATE", "TRANSACTIONID", "TRANSACTIONTIME", "UNITPRICE", "VOUCHERNUMBER", "ROWVERSION", "STATUS", "ENROLLTYPE", "IDENTIFIERIDREPLACEMENT" };

        public static string auditNumber { get; set; }
        public static int pointBalance { get; set; }
        public static int tempPointBalance { get; set; }
        public static int PointVisit { get; set; }
        public static List<string> paymentMethods
        {
            get { return paymentMethod; }
            set { paymentMethod = value; }
        } //paymentMethod 18 = TUNAI 10 = BCA 12 = MANDIRI PTS = POINTS 14 = TITIPAN CUSTOMER 


        //REPRINT
        public static Dictionary<string, string> reprintValue = new Dictionary<string, string>();
        public static bool isReprint = false;
        


        /// <summary>
        /// clear list listvouchers, paymentmethods
        /// </summary>
        public static void clearList()
        {
            listVoucher.Clear();
            paymentMethod.Clear();
        }


        private static Dictionary<string, string> datSNF = new Dictionary<string, string>();

        public static void SetSNF(string key, string value)
        {
            if (datSNF.ContainsKey(key))
            {
                datSNF[key] = value;
            }
            else
            {
                datSNF.Add(key, value);
            }
        }

        public static string GetSNF(string key)
        {
            string result = null;

            if (datSNF.ContainsKey(key))
            {
                result = datSNF[key];
            }

            return result;
        }

        public static void clearSNF()
        {
            datSNF.Clear();
        }

       //DISKON & PROMO
        public static decimal itemDiscAmount = 0;// add by maria price discount
        public static int getDiscountType = 0; // add  by maria price discount
        public static int getOperator = 0; // add by maria price discount
        public static string itemDiscount = string.Empty; // add by maria price discount       
        public static bool doneCheckItemDiscount = false;
        public static long[] parent;
        public static long[] child;
        public static string[] displaynumber;
        public static decimal getamountdue = 0;
        public static string[] itemdiscount;
        public static decimal[] itemdiscamount;
        public static int[] itemdisctype;
        public static bool isDiscountORApplied = false;
        public static string[] offerIdsFound;  // add by mar price discount
        public static decimal[] amountIdsFound;
        public static int[] currencyIdsFound;
        public static bool iscekitemLine = false;
        public static decimal lastDiscountAmount = 0;
        public static decimal lastDiscountDealPrice = 0;
        public static string exclusiveOfferid;
        public static decimal exclusiveAmountid = 0;
        public static string[] compoundedOfferid;
        public static decimal[] compoundedAmountid;
        public static int countcomp = 0;
        public static string bestOfferid;
        public static decimal bestPercentage = 0;
        public static decimal bestAmountid = 0;
        public static decimal compoundpercentage = 0;
        public static bool isExclusive = false;
        public static bool isCompounded = false;
        public static decimal quantity = 0;

        //Added by Adhi (Welcome Voucher)              
        public static string[] pilihvoucher;
        public static string getPilihVoucher = string.Empty;
        public static string[] amount;
        public static string getAmount = string.Empty;
        public static bool isWelcomeVoucherApplied = false;
        public static bool isgetidentifiersukses = true;
        public static string voucherdipake;
        public static IPosTransaction welcomePosTransaction = null;
        public static decimal wlcmvouchamount = 0;
        public static bool isApplyDisc;
        public static decimal totalbelanjakategori = 0;
        public static decimal minbelanja = 0;
        public static bool iswelcomevoucher50 = false;
        public static bool iswelcomevoucher100 = false;
        public static bool iswelcomevoucher = false;
        public static int DiscVoucher = 0;
        public static int CashVoucher = 0;
        public static string[] displaynumberWelcomeVouch;
        public static bool isgetitemWelcomeVoucher = false;
        public static bool isVoucherApplied = false;

        //VMS
        public static string[] displaynumberUndiscounted;
        public static bool isgetitemUndiscounted = false;
        public static decimal amountVouchTBSI = 0;
        public static string[] itemVouchTBSI;
        public static bool isVouchTBSIApplied = false;
        public static bool isTBSIVoucherLama = false;
        public static bool isVMSused = false;
        public static bool isCashVoucher = false;
        public static bool isDiscVoucher = false;
        public static bool isCash = false;
        public static bool isDisc = false;
        public static bool isAktifVoucher = false;

        //BIRTHDAY VOUCHER
        public static bool isbirthdayvoucher = false;
        public static bool isbirthdayvoucherFAN = false;
        public static bool isbirthdayvoucherCLUB = false;
        public static bool isVoucherAppliedforBirthday = false;

        public static bool isUpdatePointAX = false;
        public static bool isCheckForInternetConnection = true;

        //POINT PLUS PAY
        public static bool isPointUsed = false;

        //GWP       
        public static decimal qtyBefore = 0;
        public static int lineNumberGlobal;
        public static bool isSetQty = false;

        //itemvoucher
        public static List<string> listItemVoucher = new List<string>();

        //login setting
        public static List<string> ListAttLoginConfig = new List<string>();
        public static bool _EngageHOStatus { get; set; }
        public static string _EngageHOServer { get; set; }
        public static int _pingFailedEngage = 0;

        public static string _defaultCustomerPublic = "9999999999999999999";
        public static bool isShowInfoAfterEnroll = false;
        public static string _custInfoName = string.Empty;
    }
}
