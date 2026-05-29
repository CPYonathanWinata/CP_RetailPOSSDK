using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccess
{
    public class APIParameter
    {

        //public class parmRequestAddTransaction
        //{
        //    public string ITEMID { get; set; }
        //    public string QTY { get; set; }
        //    public string UNITID { get; set; }
        //    public string DATAAREAID { get; set; }
        //    public string WAREHOUSE { get; set; }
        //    public string TYPE { get; set; }
        //    public string REFERENCESNUMBER { get; set; }
        //    public string RETAILVARIANTID { get; set; }
        //}

        //public class parmMultiRequestAddTrans
        //{
        //    public List<parmRequestAddTransaction> parmRequest { get; set; }
        //}

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
            public decimal discAmt { get; set; }
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
            public List<Campaign> campaigns { get; set; }
            public Price price { get; set; }
            public Currency currency { get; set; }
            public List<Item> items { get; set; }
            public Receiver receiver { get; set; }
        }

        public class Campaign
        {
            public string id { get; set; }
            public string name { get; set; }
            public int deductedAmount { get; set; }
            public string deductedPart { get; set; }
            public List<string> appliedItemIDs { get; set; }
            public string level { get; set; }
            public int mexFundedRatio { get; set; }
            public string type { get; set; }
            public int usageCount { get; set; }
            public string campaignNameForMex { get; set; }
        }

        //public class Campaigns
        //{
        //    public List<Campaign> CampaignList { get; set; }
        //}


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


        //for rounding
        public class RoundingRule 
        {
            public int FromAmount { get; set; }
            public int ToAmount { get; set; }
            public int Rounding { get; set; }
        }
        //

        public static class RoundingDataStore
        {
            private static readonly List<RoundingData> _items = new List<RoundingData>();

            public static List<RoundingData> Items
            {
                get { return _items; }
            }
        }

        public class RoundingData
        {
            public string TransId { get; set; }
            public int LineNum { get; set; }
            public string ItemId { get; set; }
            public decimal Rounding { get; set; }
        }

        //for calling PO receive via PDT 11092025 Yonathan
        public class parmRequestPOStatus
        {
            public string DATAAREAID { get; set; }
            public string DOCUMENT_NUMBER { get; set; }
        }

        public class parmResponsePOStatus
        {
            public bool error { get; set; }
            public int message_code { get; set; }
            public string message_description { get; set; }
            public string response_data { get; set; }
        }

        public class responsePOData
        {
            public string DATAAREAID { get; set; }
            public string DOCUMENT_NUMBER { get; set; }
            public string WAREHOUSE_FROM { get; set; }
            public string WAREHOUSE_TO { get; set; }
            public int STAT_PRINT { get; set; }
            public string RECEIVED_USER { get; set; }
            public string RECEIVED_AT { get; set; }
        }
        //end

        //public static MySql.Data.MySqlClient.MySqlConnection mySqlConnString;

        //add mysql subtitue for getting URL API - Yonathan 18092025
        public class parmRequestAPIConfig
        {
            public string DATAAREAID { get; set; }
            public string STOREID { get; set; }

        }
        public class parmResponseAPIConfig
        {
            public bool error { get; set; }
            public int message_code { get; set; }
            public string message_description { get; set; }
            public ResponseData response_data { get; set; }
        }

      
        public class ResponseData
        {
            public List<CpApiUrlQris> CPAPIURLQRIS { get; set; }
            public List<CpUrlConfig> CPURLCONFIG { get; set; }
        }

        public class CpApiUrlQris
        {
            public int PAYMENTMETHOD { get; set; }
            public string PAYMENTMETHODNAME { get; set; }
            public string URLCREATE { get; set; }
            public string URLINVALIDATE { get; set; }
            public string URLNOTIFY { get; set; }
        }

        public class CpUrlConfig
        {
            public string FUNCNAME { get; set; }
            public string URL { get; set; }
            public int ISUSINGSERVICEREFERENCE { get; set; }
            public string SERVICEREFERENCENAME { get; set; }
        }

        //

        //for izone PLN API
        public class ApiRequestCheckBalanceIzone
        {
            public string legal { get; set; }
            public string storeId { get; set; }
            public string terminalId { get; set; }
        }

        public class ApiResponseCheckBalanceIzone
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class CheckBalanceData
        {
            public string ResponseCode { get; set; }
            public string GroupBalance { get; set; }
            public string Message { get; set; }
            public string TerminalBalance { get; set; }
            public string TerminalID { get; set; }
        }


        public class APIRequestInquiryTransactionIzone
        {
            
            public string legal { get; set; }
            public string storeId { get; set; }
            public string terminalId { get; set; }
            public requestDataIzone requestData { get; set; }
        }

        public class requestDataIzone
        {
            public string productCode { get; set; }  //prepaid -> token
            public decimal amount { get; set; }
            public string customerId { get; set; }
            public string meterNumber { get; set; }
        }


        public class APIResponseInquiryTransactionIzone
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }           
            public string data { get; set; }
        }


        public class InquiryTransactionDataIzone
        {
            public string AdditionalData { get; set; }
            public string ResponseCode { get; set; }
            public string ProductCode { get; set; }
            public string ResponseData { get; set; }
            public string Amount { get; set; }
            public string TransactionDateTime { get; set; }
            public string CustomerID { get; set; }
            public int Admin { get; set; }
            public string ProcessingCode { get; set; }
            public string TransactionID { get; set; }
            public string TerminalID { get; set; }
            public string ReferenceNo { get; set; }
            public string TraceNo { get; set; }


            public Dictionary<string, string> GetParsedResponseData()
            {
                var result = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(ResponseData))
                    return result;

                // Split by pipe
                string[] parts = ResponseData.Split('|');
                foreach (var part in parts)
                {
                    // Split key-value by colon
                    int colonIndex = part.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        string key = part.Substring(0, colonIndex).Trim();
                        string value = part.Substring(colonIndex + 1).Trim();
                        result[key] = value;
                    }
                    else
                    {
                        // Optional: store items without colon as "Info" or ignore
                        string info = part.Trim();
                        if (!string.IsNullOrEmpty(info))
                            result["Info_" + result.Count] = info;
                    }
                }

                return result;
            }

        }


        public class ApiRequestPaymentIzone
        {
            public string legal { get; set; }
            public string storeId { get; set; }
            public string terminalId { get; set; }
            public string traceNumber { get; set; }
        }

        public class ApiResponsePaymentIzone
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class PaymentResponseDataIzone
        {
            public string AdditionalData { get; set; }
            public string BillAmount { get; set; }
            public string SerialNo { get; set; }
            public string ResponseCode { get; set; }
            public string Receipt { get; set; }
            public string TerminalBalance { get; set; }
            public string ProductCode { get; set; }
            public string Amount { get; set; }
            public string TransactionDateTime { get; set; }
            public string CustomerID { get; set; }
            public string Admin { get; set; }
            public string ProcessingCode { get; set; }
            public string TerminalID { get; set; }
            public string ReferenceNo { get; set; }
            public string TraceNo { get; set; }
        }

        public class ApiRequestCheckTransIzone
        {
            public string legal { get; set; }
            public string storeId { get; set; }
            public string terminalId { get; set; }
            public string traceNumber { get; set; }
        }


        public class ApiResponseCheckTransIzone
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class CheckTransIzoneResponseData
        {
            public string AdditionalData { get; set; }
            public string BillAmount { get; set; }
            public string SerialNo { get; set; }
            public string ResponseCode { get; set; }
            public string ProductCode { get; set; }
            public string Amount { get; set; }
            public string TransactionDateTime { get; set; }
            public string CustomerID { get; set; }
            public string ProcessingCode { get; set; }
            public int Collection { get; set; }
            public string TerminalID { get; set; }
            public string ReferenceNo { get; set; }
            public string TraceNo { get; set; }
            public string Receipt { get; set; }
        }

        public class ApiRequestCheckTransAltIzone
        {
            public string legal { get; set; }
            public string storeId { get; set; }
            public string terminalId { get; set; }
            public string traceNumber { get; set; }
        }

        public class ApiResponseCheckTransAltIzone
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class CheckTransAltIzoneData
        {
            public string SerialNo { get; set; }
            public string ReqTime { get; set; }
            public string Receipt { get; set; }
            public string ProductCode { get; set; }
            public string Amount { get; set; }
            public string TrxDate { get; set; }
            public string RespTime { get; set; }
            public string TerminalID { get; set; }
            public string CustomerID { get; set; }
            public string BillingAmount { get; set; }
            public string ReferenceNo { get; set; }
            public string TransactionID { get; set; }
        }


        //end


        // blibli otfrt
        /*{
    "warehouse": "WH_JDELIMA",
    "dateFrom": "2026-02-18 00:00:00",
    "dateTo": "2026-02-18 23:59:59"
}*/
        public class ApiRequestBliBliListOrder
        {
            public string warehouse { get; set; }
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }

        public class ApiResponseBliBliListOrderRaw
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; } 
        }

        public class ApiResponseBliBliListOrder
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public List<OrderData> data { get; set; }
        }

        public class OrderData
        {
            public string order_id { get; set; }
            public string store_code { get; set; }
            public string pickup_point_code { get; set; }
            public string pickup_point_name { get; set; }
            public string status { get; set; }
            public string reason { get; set; }
            public string package_id { get; set; }
            public int status_receipt { get; set; }
            public string no_receipt_pos { get; set; }
            public string order_time { get; set; }

            public List<Product> product { get; set; }
            public List<Amount> amount { get; set; }
            //public List<object> adjustment { get; set; }
            //public List<Adjustment> adjustment { get; set; }
            public List<List<Adjustment>> adjustment { get; set; }

            public Recipient recipient { get; set; }
            public List<Shipment> shipment { get; set; }
            public List<Flags> flags { get; set; }
            public List<OrderItem> order_item { get; set; }

            //public DateTime created_at { get; set; }
            //public DateTime updated_at { get; set; }

            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Adjustment
        {
            public int amount { get; set; }
            public string code { get; set; }
            public string description { get; set; }
            public int merchantMargin { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }


        public class Product
        {
            public string blibliSku { get; set; }
            public string name { get; set; }
            public string sellerSku { get; set; }
            public int quantity { get; set; }
            public int initialQuantity { get; set; }
            public decimal itemPrice { get; set; }
            public string type { get; set; }
            public string notes { get; set; }
        }

        public class Amount
        {
            public decimal itemAmount { get; set; }
            public decimal itemTotalAmount { get; set; }
            public decimal sellerAmount { get; set; }
            public decimal shippingCost { get; set; }
            public decimal shippingInsuranceCost { get; set; }
            public decimal paymentFee { get; set; }
        }

        public class Recipient
        {
            public string name { get; set; }
            public string streetAddress { get; set; }
            public string country { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string district { get; set; }
            public string subDistrict { get; set; }
            public string zipCode { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
        }

        public class Shipment
        {
            public string logisticProductCode { get; set; }
            public string logisticProductName { get; set; }
            public string logisticOptionCode { get; set; }
            public string logisticOptionName { get; set; }
            public string notes { get; set; }
            public long shippingEtdMin { get; set; }
            public long shippingEtdMax { get; set; }
            public int totalWeight { get; set; }
        }

        public class Flags
        {
            public bool instantPickup { get; set; }
            public bool fulfilledByBlibli { get; set; }
            public bool cashOnDelivery { get; set; }
            public bool fasOrder { get; set; }
        }

        public class OrderItem
        {
            public string id { get; set; }
            public string packageId { get; set; }
            public bool packageCreated { get; set; }
            public int? itemSellerCount { get; set; }
        }


        //change status to PF
        public class ApiRequestBlibliCreatePacakge
        {
            public string orderId { get; set; }
            public string orderItemIds { get; set; }
        }

        public class ApiResponseBlibliCreatePackage
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public DataPackage data { get; set; }
        }

        public class DataPackage
        {
            public string requestId { get; set; }
            public string errorMessage { get; set; }
            public string errorCode { get; set; }
            public bool success { get; set; }
            public Value value { get; set; }
        }

        public class Value
        {
            public string id { get; set; }
            public string storeId { get; set; }
            public string createdDate { get; set; }
            public string createdBy { get; set; }
            public string updatedDate { get; set; }
            public string updatedBy { get; set; }
            public string version { get; set; }
            public string packageId { get; set; }
        }
        //end


        //change status to PU
        public class ApiRequestBlibliFulfillOrder
        {
            public string orderId { get; set; }
        }

        public class ApiResponseBlibliFulfillOrder
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }


        //end

        //get current status
        public class ApiRequestBlibliGetCurrentStatus
        {
            public string id { get; set; }
        }

        public class ApiResponseBlibliGetCurrentStatus
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }

        public class BlibliCurrentStatusData
        {
            public string pickup_point_code { get; set; }
            public string pickup_point_name { get; set; }
            public string order_id { get; set; }
            public string status { get; set; }
            public string reason { get; set; }
        }
        //end


        //update transaction after receipt
        public class ApiRequestBlibliUpdateTransStatus
        {
            public string orderId { get; set; }
            public string receiptID { get; set; }
            
        }

        public class ApiResponseBlibliUpdateTransStatus
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }
        //end

        //cancel order 
        public class ApiRequestBlibliCancelOrder
        {
            public string orderId { get; set; }
            public string reasonCode { get; set; }
        }

        public class ApiResponseBlibliCancelOrder
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public List<CancelOrderItem> data { get; set; }
        }

        public class CancelOrderItem
        {
            public int status { get; set; }
            public bool error { get; set; }
            public string message { get; set; }
            public object data { get; set; } // karena null, bisa pakai object atau string
        }
        //end
    }
}