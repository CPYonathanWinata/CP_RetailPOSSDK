/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.CustomerOrderParameters
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using LSRetailPosis.Transaction;

    /// <summary>
    /// Collected Customer Order info, for use in serializing and transmitting via TS call.
    /// </summary>
    [Serializable]
    [XmlRoot("CustomerOrder")]
    public class CustomerOrderInfo
    {
        /// <summary>
        /// Whether or not to Automatically create a pick list for the order
        /// </summary>
        [XmlElement("AutoPickOrder")]
        public bool AutoPickOrder;

        /// <summary>
        /// Id of the order/quote
        /// </summary>
        [XmlElement("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Transaction id.
        /// </summary>
        [XmlElement("TransactionId")]
        public string TransactionId;

        /// <summary>
        /// Type of order
        /// </summary>
        [XmlElement("OrderType")]
        public CustomerOrderType OrderType;

        /// <summary>
        /// Status of the order
        /// </summary>
        [XmlElement("Status")]
        public int Status;

        /// <summary>
        /// Document status of the order
        /// </summary>
        [XmlElement("DocumentStatus")]
        public int DocumentStatus;

        /// <summary>
        /// Customer account id
        /// </summary>
        [XmlElement("CustomerAccount")]
        public string CustomerAccount;

        /// <summary>
        /// Address id
        /// </summary>
        [XmlElement("AddressRecord")]
        public string AddressRecordId;

        /// <summary>
        /// Id of the Site
        /// </summary>
        [XmlElement("InventSiteId")]
        public string SiteId;

        /// <summary>
        /// Id of the Warehouse location
        /// </summary>
        [XmlElement("InventLocationId")]
        public string WarehouseId;

        /// <summary>
        /// Id of the current store
        /// </summary>
        [XmlElement("StoreId")]
        public string StoreId;

        /// <summary>
        /// Retail terminal Id
        /// </summary>
        [XmlElement("TerminalId")]
        public string TerminalId;

        /// <summary>
        /// Total manual discount amount.
        /// </summary>
        [XmlElement("TotalManualDiscountAmount")]
        public decimal TotalManualDiscountAmount;

        /// <summary>
        /// Total manual discount percentage.
        /// </summary>
        [XmlElement("TotalManualDiscountPercentage")]
        public decimal TotalManualDiscountPercentage;

        /// <summary>
        /// Expiry Date in string format
        /// </summary>
        [XmlElement("ExpiryDate")]
        public string ExpiryDateString;

        /// <summary>
        /// Creation Date in string format
        /// </summary>
        [XmlElement("CreationDate")]
        public string CreationDateString;

        /// <summary>
        /// Local hour of day when order is created
        /// </summary>
        [XmlElement("HourOfDay")]
        public int LocalHourOfDay;

        /// <summary>
        /// Delivery mode
        /// </summary>
        [XmlElement("DeliveryMode")]
        public string DeliveryMode;

        /// <summary>
        /// Expiry Date in string format
        /// </summary>
        [XmlElement("RequestedDeliveryDate")]
        public string RequestedDeliveryDateString;

        /// <summary>
        /// Comment
        /// </summary>
        [XmlElement("Comment")]
        public string Comment;

        /// <summary>
        /// Set true if Prepayment (Deposit) Amount was Overridden.
        /// </summary>
        [XmlElement("PrepaymentAmountOverridden")]
        public bool PrepaymentAmountOverridden;

        /// <summary>
        /// Amount of prepayment that is currently applied to this order.
        /// </summary>
        [XmlElement("PrepaymentAmountApplied")]
        public decimal? PrepaymentAmountApplied;

        /// <summary>
        /// Amount that has been previously invoiced (picked-up)
        /// </summary>
        [XmlElement("PreviouslyInvoicedAmount")]
        public decimal PreviouslyInvoicedAmount;

        /// <summary>
        /// Information to void a prior authorization
        /// </summary>
        [XmlElement("PreAuthorization")]
        public Preauthorization Preauthorization;

        /// <summary>
        /// Worker ID for sales person.
        /// </summary>
        /// <remarks>Note this is not the same as operator ID</remarks>
        [XmlElement("SalespersonStaffId")]
        public string SalespersonStaffId;

        /// <summary>
        /// Worker name for sales person.
        /// </summary>
        [XmlElement("SalespersonName")]
        public string SalespersonName;

    

        /// <summary>
        /// Worker name for sales person.
        /// </summary>
        [XmlElement("SalesTakerName")]
        public string SalesTakerName;
        //end

        /// <summary>
        /// Currency code for the order (usually the Store currency code)
        /// </summary>
        [XmlElement("CurrencyCode")]
        public string CurrencyCode;

        /// <summary>
        /// Quotataion Id
        /// </summary>
        [XmlElement("QuotationId")]
        public string QuotationId;

        /// <summary>
        /// Return reason code id.
        /// </summary>
        [XmlElement("ReturnReasonCodeId")]
        public string ReturnReasonCodeId;

        /// <summary>
        /// Loyalty card id.
        /// </summary>
        [XmlElement("LoyaltyCardId")]
        public string LoyaltyCardId;

        /// <summary>
        /// Indicates whether the customer order has any posted loyalty tender line. 
        /// </summary>
        [XmlElement("HasLoyaltyPayment")]
        public bool HasLoyaltyPayment;
        
        /// <summary>
        /// Return Channel reference id
        /// </summary>
        [XmlElement("ChannelReferenceId")]
        public string ChannelReferenceId;

        /// <summary>
        /// Credit card token for customer order
        /// </summary>
        [XmlElement("CreditCardToken")]
        public string CreditCardToken;

        /// <summary>
        /// Email for the order
        /// </summary>
        [XmlElement("Email")]
        public string Email;

        //Custom by Yonathan 06/02/2024
        /// <summary>
        /// Worker ID for sales person.
        /// </summary>
        /// <remarks>Note this is not the same as operator ID</remarks>
        [XmlElement("SalesTakerStaffId")]
        public string SalesTakerStaffId;

        //Custom by Yonathan 25/07/2024
        /// <summary>
        /// Worker ID for sales person.
        /// </summary>
        /// <remarks>Note this is not the same as operator ID</remarks>
        [XmlElement("CPOrderNumber")]
        public string CPOrderNumber;

        /// <summary> 
        /// Items
        /// </summary>
        public Collection<ItemInfo> Items;

        /// <summary>
        /// Charges
        /// </summary>
        public Collection<ChargeInfo> Charges;

        /// <summary>
        /// Payments
        /// </summary>
        public Collection<PaymentInfo> Payments;

        /// <summary>
        /// Discount codes on the transaction
        /// </summary>
        [XmlElement("DiscountCodes")]
        public Collection<string> DiscountCodes;

        /// <summary>
        /// Affiliations
        /// </summary>
        [XmlArray("Affiliations")]
        public Collection<AffiliationInfo> Affiliations;

        /// <summary>
        /// Convert CustomerOrderInfo into XML
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            string xmlString = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerOrderInfo));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                writer.Flush();
                xmlString = writer.ToString();
            }
            return xmlString;
        }

        /// <summary>
        /// Create CustomerOrderInfo from XML
        /// </summary>
        /// <param name="orderXml"></param>
        /// <returns></returns>
        public static CustomerOrderInfo FromXml(string orderXml)
        {
            CustomerOrderInfo orderInfo;
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerOrderInfo));
            using (StringReader reader = new StringReader(orderXml))
            {
                serializer = new XmlSerializer(typeof(CustomerOrderInfo));
                orderInfo = (CustomerOrderInfo)serializer.Deserialize(reader);
                return orderInfo;
            }
        }
    }

    /// <summary>
    /// Item info, for use in transmitting via TS call
    /// </summary>
    [Serializable]
    [XmlType("Item")]
    public class ItemInfo
    {
        /// <summary>
        /// Item id
        /// </summary>
        [XmlAttribute]
        public string ItemId;

        /// <summary>
        /// AX RecId
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rec", Justification = "Follows naming in AX.")]
        [XmlAttribute("RecId")]
        public Int64 RecId;

        /// <summary>
        /// Quantity
        /// </summary>
        [XmlAttribute]
        public decimal Quantity;

        /// <summary>
        /// Quantity picked
        /// </summary>
        [XmlAttribute]
        public decimal QuantityPicked;

        /// <summary>
        /// Unit of measure
        /// </summary>
        [XmlAttribute]
        public string Unit;

        /// <summary>
        /// Price
        /// </summary>
        [XmlAttribute]
        public decimal Price;

        /// <summary>
        /// Discount amount
        /// </summary>
        [XmlAttribute]
        public decimal Discount;

        /// <summary>
        /// Discount percent
        /// </summary>
        [XmlAttribute]
        public decimal DiscountPercent;

        /// <summary>
        /// Net amount
        /// </summary>
        [XmlAttribute]
        public decimal NetAmount;

        /// <summary>
        /// Item Comment
        /// </summary>
        [XmlAttribute]
        public string Comment;

        /// <summary>
        /// Sales Tax Group Id
        /// </summary>
        [XmlAttribute("TaxGroup")]
        public string SalesTaxGroup;

        /// <summary>
        /// Item Tax Group Id
        /// </summary>
        [XmlAttribute("TaxItemGroup")]
        public string ItemTaxGroup;

        /// <summary>
        /// SalesMarkup - used for price charges.
        /// </summary>
        [XmlAttribute("SalesMarkup")]
        public decimal SalesMarkup;

        /// <summary>
        /// Id of the Site
        /// </summary>
        [XmlAttribute]
        public string SiteId;

        /// <summary>
        /// Id of the Store
        /// </summary>
        [XmlAttribute]
        public string StoreId;

        /// <summary>
        /// Sales status.
        /// </summary>
        [XmlAttribute]
        public int Status;

        /// <summary>
        /// Id of the Warehouse location
        /// </summary>
        [XmlAttribute("InventLocationId")]
        public string WarehouseId;

        /// <summary>
        /// Id of the color.
        /// </summary>
        [XmlAttribute("InventColorId")]
        public string ColorId;

        /// <summary>
        /// Name of the color.
        /// </summary>
        [XmlAttribute("InventColorName")]
        public string ColorName;

        /// <summary>
        /// Id of the size.
        /// </summary>
        [XmlAttribute("InventSizeId")]
        public string SizeId;

        /// <summary>
        /// Name of the size.
        /// </summary>
        [XmlAttribute("InventSizeName")]
        public string SizeName;

        /// <summary>
        /// Id of the style.
        /// </summary>
        [XmlAttribute("InventStyleId")]
        public string StyleId;

        /// <summary>
        /// Name of the style.
        /// </summary>
        [XmlAttribute("InventStyleName")]
        public string StyleName;

        /// <summary>
        /// Id of the config.
        /// </summary>
        [XmlAttribute("ConfigId")]
        public string ConfigId;

        /// <summary>
        /// Name of the config.
        /// </summary>
        [XmlAttribute("ConfigName")]
        public string ConfigName;

        /// <summary>
        /// Delivery mode
        /// </summary>
        [XmlAttribute("DeliveryMode")]
        public string DeliveryMode;

        /// <summary>
        /// Expiry Date in string format
        /// </summary>
        [XmlAttribute("RequestedDeliveryDate")]
        public string RequestedDeliveryDateString;

        /// <summary>
        /// Address id
        /// </summary>
        [XmlAttribute("AddressRecord")]
        public string AddressRecordId;

        /// <summary>
        /// Id of the Batch
        /// </summary>
        [XmlAttribute]
        public string BatchId;

        /// <summary>
        /// Serial Id
        /// </summary>
        [XmlAttribute]
        public string SerialId;

        /// <summary>
        /// Id of the variant
        /// </summary>
        [XmlAttribute]
        public string VariantId;

        [XmlAttribute("InventTransId")]
        public string InventTransId;

        [XmlAttribute("InvoiceId")]
        public string InvoiceId;

        [XmlAttribute("LineDscAmount")]
        public decimal LineDscAmount;

        [XmlAttribute("PeriodicDiscount")]
        public decimal PeriodicDiscount;

        [XmlAttribute("PeriodicPercentageDiscount")]
        public decimal PeriodicPercentageDiscount;

        [XmlAttribute("LineManualDiscountAmount")]
        public decimal LineManualDiscountAmount;

        [XmlAttribute("LineManualDiscountPercentage")]
        public decimal LineManualDiscountPercentage;

        [XmlAttribute("TotalDiscount")]
        public decimal TotalDiscount;

        [XmlAttribute("TotalPctDiscount")]
        public decimal TotalPctDiscount;

        /// <summary>
        /// Line Level Charges(for Mixed Delivery).
        /// </summary>
        public Collection<ChargeInfo> Charges;

        public Collection<DiscountInfo> Discounts;
    }

    /// <summary>
    /// Discount info, for use in transmitting via TS call
    /// </summary>
    [Serializable]
    [XmlType("Discount")]
    public class DiscountInfo
    {       
        [XmlAttribute("Amount")]
        public decimal EffectiveAmount;      

        [XmlAttribute("CustomerDiscountType")]
        public int CustomerDiscountType;

        [XmlAttribute("DealPrice")]
        public decimal DealPrice;

        [XmlAttribute("DiscountAmount")]
        public decimal DiscountAmount;

        [XmlAttribute("DiscountCode")]
        public string DiscountCode;

        [XmlAttribute("DiscountOriginType")]
        public int DiscountOriginType;

        [XmlAttribute("ManualDiscountType")]
        public int ManualDiscountType;

        [XmlAttribute("Percentage")]
        public decimal Percentage;   

        [XmlAttribute("PeriodicDiscountOfferId")]
        public string PeriodicDiscountOfferId;

        [XmlAttribute("OfferName")]
        public string OfferName;
    }

    /// <summary>
    /// Misc. Charge, info for use in transmitting via TS call
    /// </summary>
    [Serializable]
    [XmlType("Charge")]
    public class ChargeInfo
    {
        /// <summary>
        /// Charge code
        /// </summary>
        [XmlAttribute("Code")]
        public string Code;

        /// <summary>
        /// Charge amount
        /// </summary>
        [XmlAttribute("Amount")]
        public decimal Amount;

        /// <summary>
        /// Sales Tax Group Id
        /// </summary>
        [XmlAttribute("TaxGroup")]
        public string SalesTaxGroup;

        /// <summary>
        /// Item Tax Group Id
        /// </summary>
        [XmlAttribute("TaxItemGroup")]
        public string TaxGroup;
    }

    /// <summary>
    /// Payment info, for use in transmitting via TS call
    /// </summary>
    [Serializable]
    [XmlType("Payment")]
    public class PaymentInfo
    {
        /// <summary>
        /// Payment type
        /// </summary>
        [XmlAttribute("PaymentType")]
        public string PaymentType;

        /// <summary>
        /// Payment amount collected
        /// </summary>
        [XmlAttribute("Amount")]
        public decimal Amount;

        /// <summary>
        /// Currency of the payment
        /// </summary>
        [XmlAttribute("Currency")]
        public string Currency;

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("Date")]
        public string DateString;

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("Prepayment")]
        public bool Prepayment;
    }

    /// <summary>
    /// Affiliation info, for use in transmitting via TS call.
    /// </summary>
    [Serializable]
    [XmlType("Affiliation")]
    public class AffiliationInfo
    {
        /// <summary>
        /// Affiliation id.
        /// </summary>
        [XmlAttribute("RetailAffiliationId")]
        public long AffiliationId;

        /// <summary>
        /// Gets or sets Loyalty tier id.
        /// </summary>
        [XmlAttribute("RetailLoyaltyTierId")]
        public long LoyaltyTierId;

        /// <summary>
        /// Gets or sets the affiliation type.
        /// </summary>
        [XmlAttribute("RetailAffiliationType")]
        public Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.RetailAffiliationType AffiliationType;
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType("PreAuthorization")]
    public class Preauthorization
    {
        /// <summary>
        /// Payment SDK properties XML
        /// </summary>
        [XmlAttribute("PaymentPropertiesBlob")]
        public string PaymentPropertiesBlob;
    }

    #region Return Invoices
    [XmlRoot("CustInvoiceJours", Namespace = "")]
    public class InvoiceJournalList
    {
        [XmlElement("CustInvoiceJour")]
        public Collection<InvoiceJournal> Invoices;

        /// <summary>
        /// Create Invoice Journal from XML
        /// </summary>
        /// <param name="orderXml"></param>
        /// <returns></returns>
        public static InvoiceJournalList FromXml(string orderXml)
        {
            InvoiceJournalList list;
            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceJournalList));
            using (StringReader reader = new StringReader(orderXml))
            {
                serializer = new XmlSerializer(typeof(InvoiceJournalList));
                list = (InvoiceJournalList)serializer.Deserialize(reader);
                return list;
            }
        }
    }

    [XmlType("CustInvoiceJour")]
    public class InvoiceJournal
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rec", Justification = "Follows naming in AX.")]
        [XmlAttribute("RecId")]
        public string RecId;

        [XmlAttribute("InvoiceId")]
        public string InvoiceId;

        [XmlAttribute("SalesId")]
        public string SalesId;

        [XmlAttribute("InvoiceDate")]
        public string InvoiceDate;

        [XmlAttribute("SalesType")]
        public string SalesType;

        [XmlAttribute("CurrencyCode")]
        public string CurrencyCode;

        [XmlAttribute("InvoiceAmount")]
        public string InvoiceAmount;

        [XmlAttribute("InvoiceAccount")]
        public string InvoiceAccount;

        [XmlAttribute("InvoicingName")]
        public string InvoicingName;

        /// <summary>
        /// Total manual discount amount.
        /// </summary>
        [XmlAttribute("TotalManualDiscountAmount")]
        public decimal TotalManualDiscountAmount;

        /// <summary>
        /// Total manual discount percentage.
        /// </summary>
        [XmlAttribute("TotalManualDiscountPercentage")]
        public decimal TotalManualDiscountPercentage;

        [XmlElement("CustInvoiceTrans")]
        public Collection<InvoiceItem> Items;

        /// <summary>
        /// Create Invoice Journal from XML
        /// </summary>
        /// <param name="orderXml"></param>
        /// <returns></returns>
        public static InvoiceJournal FromXml(string orderXml)
        {
            InvoiceJournal orderInfo;
            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceJournal));
            using (StringReader reader = new StringReader(orderXml))
            {
                serializer = new XmlSerializer(typeof(InvoiceJournal));
                orderInfo = (InvoiceJournal)serializer.Deserialize(reader);
                return orderInfo;
            }
        }
    }

    [XmlType("CustInvoiceTrans")]
    public class InvoiceItem
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rec", Justification = "Follows naming in AX.")]
        [XmlAttribute("RecId")]
        public string RecId;

        [XmlAttribute("ItemId")]
        public string ItemId;

        [XmlAttribute("EcoResProductName")]
        public string ProductName;

        [XmlAttribute("InventDimId")]
        public string InventDimId;

        [XmlAttribute("InventTransId")]
        public string InventTransId;

        [XmlAttribute("Qty")]
        public decimal Quantity;

        [XmlAttribute("SalesPrice")]
        public decimal Price;

        [XmlAttribute("DiscPercent")]
        public decimal DiscountPercent;

        [XmlAttribute("DiscAmount")]
        public decimal DiscountAmount;

        [XmlAttribute("InventBatchId")]
        public string BatchId;

        [XmlAttribute("LineAmount")]
        public decimal NetAmount;

        [XmlAttribute("InventSiteId")]
        public string Site;

        [XmlAttribute("InventLocationId")]
        public string Warehouse;

        [XmlAttribute("InventSerialId")]
        public string SerialId;

        [XmlAttribute("InventColorId")]
        public string ColorId;

        [XmlAttribute("InventSizeId")]
        public string SizeId;

        [XmlAttribute("InventStyleId")]
        public string StyleId;

        [XmlAttribute("ConfigId")]
        public string ConfigId;

        [XmlAttribute("InventColorName")]
        public string ColorName;

        [XmlAttribute("InventSizeName")]
        public string SizeName;

        [XmlAttribute("InventStyleName")]
        public string StyleName;

        [XmlAttribute("ConfigName")]
        public string ConfigName;

        [XmlAttribute("TaxGroup")]
        public string SalesTaxGroup;

        [XmlAttribute("TaxItemGroup")]
        public string ItemTaxGroup;

        [XmlAttribute("SalesMarkup")]
        public decimal SalesMarkup;

        [XmlAttribute("SaleUnit")]
        public string Unit;

        [XmlAttribute()]
        public string DeliveryMode;

        [XmlAttribute("LineDscAmount")]
        public decimal LineDscAmount;

        [XmlAttribute("PeriodicDiscount")]
        public decimal PeriodicDiscount;

        [XmlAttribute("PeriodicPercentageDiscount")]
        public decimal PeriodicPercentageDiscount;

        [XmlAttribute("LineManualDiscountAmount")]
        public decimal LineManualDiscountAmount;

        [XmlAttribute("LineManualDiscountPercentage")]
        public decimal LineManualDiscountPercentage;

        [XmlAttribute("TotalDiscount")]
        public decimal TotalDiscount;

        [XmlAttribute("TotalPctDiscount")]
        public decimal TotalPctDiscount;

        public Collection<InvoiceItemDiscount> Discounts;

        public InvoiceItem()
        {
            this.Discounts = new Collection<InvoiceItemDiscount>();
        }
    }


    /// <summary>
    /// Invoice item discount info, for use in transmitting via TS call
    /// </summary>
    [Serializable]
    [XmlType("Discount")]
    public class InvoiceItemDiscount
    {
        [XmlAttribute("Amount")]
        public decimal EffectiveAmount;

        [XmlAttribute("CustomerDiscountType")]
        public int CustomerDiscountType;

        [XmlAttribute("DealPrice")]
        public decimal DealPrice;

        [XmlAttribute("DiscountAmount")]
        public decimal DiscountAmount;

        [XmlAttribute("DiscountCode")]
        public string DiscountCode;

        [XmlAttribute("DiscountOriginType")]
        public int DiscountOriginType;

        [XmlAttribute("ManualDiscountType")]
        public int ManualDiscountType;

        [XmlAttribute("Percentage")]
        public decimal Percentage;

        [XmlAttribute("PeriodicDiscountOfferId")]
        public string PeriodicDiscountOfferId;

        [XmlAttribute("OfferName")]
        public string OfferName;
    }

    #endregion

    #region GetCustomer

    [XmlRoot("Customer", Namespace = "")]
    public class GetCustomerResponse
    {
        [XmlElement("CustTable")]
        public CustTableRow CustomerInfo;

        [XmlElement("RetailCustTable")]
        public RetailCustTableRow RetailCustomerInfo;

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
        [XmlElement("DirPartyTable")]
        public DirPartyTableRow DirPartyInfo;

        [XmlElement("PostalAddress")]
        public Collection<PostalAddress> PostalAddresses;

        [XmlElement("ElectronicAddress")]
        public Collection<ElectronicAddress> ElectronicAddresses;

        /// <summary>
        /// Create GetCustomerResponse from XML
        /// </summary>
        /// <param name="getCustomerXmlResponse">The xml response for the GetCustomer AX call.</param>
        /// <returns>The GetCustomerResponse object.</returns>
        public static GetCustomerResponse FromXml(string getCustomerXmlResponse)
        {
            GetCustomerResponse orderInfo;
            XmlSerializer serializer = new XmlSerializer(typeof(GetCustomerResponse));
            using (StringReader reader = new StringReader(getCustomerXmlResponse))
            {
                serializer = new XmlSerializer(typeof(GetCustomerResponse));
                orderInfo = (GetCustomerResponse)serializer.Deserialize(reader);
                return orderInfo;
            }
        }
    }

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cust", Justification = "Follows naming in AX.")]
    [XmlType("CustTable")]
    public class CustTableRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("AccountNum")]
        public string AccountNumber;

        [XmlAttribute("Party")]
        public long Party;

        [XmlAttribute("CustGroup")]
        public string CustomerGroup;

        [XmlAttribute("InvoiceAccount")]
        public string InvoiceAccount;

        [XmlAttribute("NameAlias")]
        public string NameAlias;

        [XmlAttribute("Currency")]
        public string Currency;

        [XmlAttribute("LanguageId")]
        public string LanguageId;

        [XmlAttribute("MultiLineDisc")]
        public string MultilineDiscount;

        [XmlAttribute("EndDisc")]
        public string EndDiscount;

        [XmlAttribute("LineDisc")]
        public string LineDiscount;

        [XmlAttribute("PriceGroup")]
        public string PriceGroup;

        [XmlAttribute("CreditMax")]
        public decimal CreditMax;

        [XmlAttribute("Blocked")]
        public int Blocked;

        [XmlAttribute("TaxGroup")]
        public string TaxGroup;

        [XmlAttribute("VATNum")]
        public string VATNumber;

        [XmlAttribute("OrgId")]
        public string OrgId;

        [XmlAttribute("UsePurchRequest")]
        public int UsePurchaseRequest;

        [XmlAttribute("InclTax")]
        public int IncludeTax;

        [XmlAttribute("IdentificationNumber")]
        public string IdentificationNumber;

        [XmlAttribute("CNPJCPFNum")]
        public string CNPJCPFNumber;
    }

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cust", Justification = "Follows naming in AX.")]
    [XmlType("RetailCustTable")]
    public class RetailCustTableRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("AccountNum")]
        public string AccountNumber;

        [XmlAttribute("NonChargableAccount")]
        public int NonChargeableAccount;

        [XmlAttribute("PostAsShipment")]
        public int PostAsShipment;

        [XmlAttribute("ReceiptEmail")]
        public string ReceiptEmail;

        [XmlAttribute("ReceiptOption")]
        public int ReceiptOption;

        [XmlAttribute("RequiresApproval")]
        public int RequiresApproval;

        [XmlAttribute("UseOrderNumberReference")]
        public int UseOrderNumberReference;
    }

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
    [XmlType("DirPartyTable")]
    public class DirPartyTableRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("PartyNumber")]
        public string PartyNumber;

        [XmlAttribute("RelationType")]
        public long RelationType;
    }

    [XmlType("PostalAddress")]
    public class PostalAddress
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
        public DirPartyLocationRow DirPartyLocation;

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
        public DirPartyLocationRoleRow DirPartyLocationRole;

        public LogisticsLocationRow LogisticsLocation;

        public LogisticsLocationRoleRow LogisticsLocationRole;

        public LogisticsPostalAddressRow LogisticsPostalAddress;
    }

    [XmlType("ElectronicAddress")]
    public class ElectronicAddress
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
        public DirPartyLocationRow DirPartyLocation;

        public LogisticsLocationRow LogisticsLocation;

        public LogisticsElectronicAddressRow LogisticsElectronicAddress;
    }

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
    [XmlType("DirPartyLocation")]
    public class DirPartyLocationRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("Location")]
        public long Location;

        [XmlAttribute("Party")]
        public long Party;

        [XmlAttribute("IsPostalAddress")]
        public int IsPostalAddress;

        [XmlAttribute("IsPrimary")]
        public int IsPrimary;

        [XmlAttribute("IsPrivate")]
        public int IsPrivate;

        [XmlAttribute("IsLocationOwner")]
        public int IsLocationOwner;

        [XmlAttribute("ValidFrom")]
        public string ValidFrom;

        [XmlAttribute("ValidTo")]
        public string ValidTo;
    }

    [XmlType("DirPartyLocationRole")]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir", Justification = "Follows naming in AX.")]
    public class DirPartyLocationRoleRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("LocationRole")]
        public long LocationRole;

        [XmlAttribute("PartyLocation")]
        public long PartyLocation;
    }

    [XmlType("LogisticsLocation")]
    public class LogisticsLocationRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("LocationId")]
        public string LocationId;

        [XmlAttribute("ParentLocation")]
        public long ParentLocation;

        [XmlAttribute("Description")]
        public string Description;

        [XmlAttribute("IsPostalAddress")]
        public int IsPostalAddress;
    }

    [XmlType("LogisticsLocationRole")]
    public class LogisticsLocationRoleRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("Type")]
        public int Type;

        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("IsPostalAddress")]
        public int IsPostalAddress;

        [XmlAttribute("IsContactInfo")]
        public int IsContactInfo;
    }

    [XmlType("LogisticsPostalAddress")]
    public class LogisticsPostalAddressRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("Location")]
        public long Location;

        [XmlAttribute("Address")]
        public string Address;

        [XmlAttribute("ZipCode")]
        public string ZipCode;

        [XmlAttribute("State")]
        public string State;

        [XmlAttribute("City")]
        public string City;

        [XmlAttribute("CountryRegionId")]
        public string CountryRegionId;

        [XmlAttribute("County")]
        public string County;

        [XmlAttribute("Street")]
        public string Street;

        [XmlAttribute("StreetNumber")]
        public string StreetNumber;

        [XmlAttribute("TimeZone")]
        public int TimeZone;

        [XmlAttribute("BuildingCompliment")]
        public string BuildingCompliment;

        [XmlAttribute("DistrictName")]
        public string DistrictName;

        [XmlAttribute("ValidFrom")]
        public string ValidFrom;

        [XmlAttribute("ValidTo")]
        public string ValidTo;
    }

    [XmlType("LogisticsElectronicAddress")]
    public class LogisticsElectronicAddressRow
    {
        [XmlAttribute("RecId")]
        public long RecordId;

        [XmlAttribute("Location")]
        public long Location;

        [XmlAttribute("Locator")]
        public string Locator;

        [XmlAttribute("Type")]
        public int Type;

        [XmlAttribute("CountryRegionCode")]
        public string CountryRegionCode;

        [XmlAttribute("ValidFrom")]
        public string ValidFrom;

        [XmlAttribute("ValidTo")]
        public string ValidTo;
    }

    #endregion
}
