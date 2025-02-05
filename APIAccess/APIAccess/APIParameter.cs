using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccess
{
    public class APIParameter
    {
        public class parmRequestStockSO
        {
            public string ITEMID { get; set; }
            public string DATAAREAID { get; set; }
            public string WAREHOUSE { get; set; }
            public string TRANSACTIONID { get; set; }
            public string QUANTITYAX { get; set; }
            public string QUANTITYINPUT { get; set; }
            public string ORIGIN { get; set; }
            public string RETAILVARIANTID { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class parmResponseStockSO
        {
            public bool error { get; set; }
            public int message_code { get; set; }
            public string message_description { get; set; }
            public string response_data { get; set; }
        }

        public class parmRequestAddItemMultiple
        {
            public string ITEMID { get; set; }
            public string QTY { get; set; }
            public string UNITID { get; set; }
            public string DATAAREAID { get; set; }
            public string WAREHOUSE { get; set; }
            public string TYPE { get; set; }
            public string REFERENCESNUMBER { get; set; }
            public string RETAILVARIANTID { get; set; }
        }

        public class parmMultiRequest
        {
            public List<parmRequest> parmRequest { get; set; }
        }

        public class parmRequestShopeePay
        {
            public string storeId { get; set; }
            public decimal amount { get; set; }
            public string terminalId { get; set; }
            public string transactionId { get; set; }
        }

        public class parmInvalidateShopeePay
        {
            public string storeId { get; set; }            
            public string terminalId { get; set; }
            public string transactionId { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class parmResponseShopeePay
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class ListResultData
        {
            public string request_id { get; set; }
            public int errcode { get; set; }
            public string debug_msg { get; set; }
            public string qr_content { get; set; }
            public string qr_url { get; set; }
            public string store_name { get; set; }
            public int qrValidityPeriod { get; set; }
            public string nmid { get; set; }
        }

        public class ListResultDataInquiry
        {
            public string request_id { get; set; }
            public int errcode { get; set; }
            public string debug_msg { get; set; }
            public int payment_method { get; set; }
            public TransactionData transaction { get; set; }
        }

        public class TransactionData
        {
            public string reference_id { get; set; }
            public long amount { get; set; }
            public long create_time { get; set; }
            public long update_time { get; set; }
            public string transaction_sn { get; set; }
            public int status { get; set; }
            public int transaction_type { get; set; }
            public string merchant_ext_id { get; set; }
            public string terminal_id { get; set; }
            public string user_id_hash { get; set; }
            public string store_ext_id { get; set; }
            public string promo_id_applied { get; set; }
        }



        //for Grabmart API
        public class parmOrderListGrabmart
        {
            public string warehouse { get; set; }
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }

        public class parmUpdateStatusListGrabmart
        {
            public string merchantID { get; set; }
            public string orderID { get; set; }
            public string receiptID { get; set; }
        }

        public class parmGetCurrentOrderStateGrabmart
        {
            public string merchantID { get; set; }
            public string orderID { get; set; }

        }

        public class parmCancelOrderGrabmart
        {
            public string merchantID { get; set; }
            public string orderID { get; set; }
            public string cancelReasons { get; set; }

        }

        public class Price
        {
            public int subtotal { get; set; }
            public int tax { get; set; }
            public int deliveryFee { get; set; }
            public int eaterPayment { get; set; }
            public int grabFundPromo { get; set; }
            public int merchantFundPromo { get; set; }
            public int merchantChargeFee { get; set; }
            public int basketPromo { get; set; }
            public int smallOrderFee { get; set; }
        }

        public class Currency
        {
            public string code { get; set; }
            public string symbol { get; set; }
            public int exponent { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public int quantity { get; set; }
            public string specifications { get; set; }
            public int price { get; set; }
            public List<object> modifiers { get; set; }
            public string grabItemID { get; set; }
            public int tax { get; set; }
            public string status { get; set; }
        }

        public class Data
        {
            public string merchantID { get; set; }
            public string orderID { get; set; }
            public string shortOrderNumber { get; set; }
            public string state { get; set; }
            public string partnerMerchantID { get; set; }
            public string paymentType { get; set; }
            public string orderTime { get; set; }
            public Price price { get; set; }
            public Currency currency { get; set; }
            public List<Item> items { get; set; }
            public Receiver receiver { get; set; }
        }

        public class Address
        {
            public string address { get; set; }
            public string unitNumber { get; set; }
            public string deliveryInstruction { get; set; }
            public string postcode { get; set; }
            public Coordinates coordinates { get; set; }
            public string poiSource { get; set; }
            public string poiID { get; set; }
            public string keywords { get; set; }
        }

        public class Coordinates
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public class Receiver
        {
            public string name { get; set; }
            public string phones { get; set; }
            public Address address { get; set; }
            public string email { get; set; }
        }


        public class ApiResponseGrabmart
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class DataStatusOrder
        {
            public string merchantID { get; set; }
            public string orderID { get; set; }
            public string shortOrderNumber { get; set; }
            public string state { get; set; }
            public string partnerMerchantID { get; set; }
        }

        public class parmRequestOnlineOrder
        {
            public string legal { get; set; }
            public string warehouse { get; set; }
        }

        public class parmResponseOnlineOrder
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public dataOnlineOrder data { get; set; }
        }

        public class dataOnlineOrder
        {
            public int total_order { get; set; }
        } 
        public class SaleLineItemData
        {
            public string ItemId { get; set; }
            public string UnitId { get; set; }
            public decimal Price { get; set; }
            public int LineId { get; set; }

        }

        

        //
        public static MySql.Data.MySqlClient.MySqlConnection mySqlConnString;
    }
}
