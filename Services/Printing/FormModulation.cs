/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Data;

using System.Configuration;

using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Settings.HardwareProfiles;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using LSRetailPosis.Transaction.Line.Tax;
using LSRetailPosis.Transaction.Line.TaxItems;
using LSRetailPosis.Transaction.Line.TenderItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;
using APIAccess;
 

namespace Microsoft.Dynamics.Retail.Pos.Printing
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class FormModulation
    {
        #region Member variables

        private SqlConnection connection;
        private string copyText = string.Empty;
        private readonly char esc = Convert.ToChar(27);
        private const string LEGACY_LOGO_MESSAGE = "<L>";
        private const string logoRegEx = @"<L:\s*(?<imageId>\d+)\s*>";
        private const string barCodeRegEx = "<B: (.+?)>";
        private const string qrCodeRegEx = "<Q: (.+?)>";
        private const string LOGO_MESSAGE = "<L:{0}>";
        private const string NORMAL_TEXT_MARKER = "|1C";
        private const string DOUBLESIZE_TEXT_MARKER = "|3C";
        private readonly ReceiptData receiptData;
        //private readonly FormModulationEfd formModulationEfd;
        //Begin add NEC hmz
        private decimal totalCustom;
        private decimal totalTaxCustom;
        // End add NEC hmz
        string OrderNo, OrderName, npwpAddress, npwpArea; //add by Heron
        string purchaserName, purchaserPhone; //add by Erwin
        string lblCardNumber = string.Empty; //add by Erwin
        string lblRedeemPoints = string.Empty; //add by Erwin
        string lblLastBalance = string.Empty; //add by Erwin
        string lblEarnPoints = string.Empty; //add by Erwin
        string lblTotalBalance = string.Empty; //add by Erwin
        string lblTransID = string.Empty; //add by Erwin
        string cardNumber = string.Empty; //add by Erwin
        string transID = string.Empty; //add by Erwin
        string ezeelinkCustomerName = string.Empty; //add by Erwin
        float redeemPoints, lastBalance; //add by Erwin
        float earnPoints, totalBalance; //add by Erwin

        string loyaltyFullName = string.Empty;
        string loyaltyPhone = string.Empty; // add by Julius S


        string lblSalesId = string.Empty; //add by Yonathan 25/05/2023
        #region CPECRBCA
        //decimal totalDibebaskan = 0; //declare variable to store amount
        decimal amountAmbilTunai = 0; //declare ambilTunai variable
        decimal amountAdmFee = 0;//add adm fee Yonathan 13/06/2024 //CPIADMFEE
        string invoiceId = ""; //add for invoice id Yonathan 10092024
        decimal amountPerLine = 0;
        decimal qtyPerLine = 0;
        decimal grandTotal = 0;

        decimal DPPPKP = 0;
        decimal DPPBebas = 0;
        decimal DPPNonPajak = 0;
        bool DPPNilaiLainStatus = true;
        string statusDPP2025Print = "False";

        ReadOnlyCollection<object> containerArray; //add for invoice detail - Yonathan 07102024
        XDocument xdoc;
        XElement xml;
        int countInfo = 0;
        #endregion

        #endregion

        #region Properties

        public SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        private static IUtility Utility
        {
            get { return Printing.InternalApplication.BusinessLogic.Utility; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        public FormModulation(SqlConnection connection)
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for FormModulation are reserved at 13020 - 13049
            // In use now are ID's: 13020 - 13040
            //
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.connection = connection;
            receiptData = new ReceiptData(connection, string.Empty);
            //formModulationEfd = new FormModulationEfd(connection.ConnectionString);
        }

        private static string CreateWhitespace(int stringLength, char seperator)
        {
            StringBuilder whiteString = new StringBuilder();
            for (int i = 1; i <= stringLength; i++)
            {
                whiteString.Append(seperator);
            }

            return whiteString.ToString();
        }

        //Begin add NEC
        private string GetTotalQuantity(RetailTransaction theTransaction)
        {
            try
            {
                decimal qty = 0.0m;
                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                    qty += s.Quantity;
                }
                return qty.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        // End add NEC
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Grandfather")]
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Grand Father PS6015")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "Grandfather")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfather")]
        private string GetInfoFromTransaction(FormItemInfo itemInfo, IEFTInfo eftInfo, ITenderLineItem tenderItem, RetailTransaction theTransaction)
        {
            CustomerOrderTransaction cot;

            //start by Yonathan 29102024
            decimal lineAmountIncTaxDecimal = 0;
            string lineAmountIncTax;
            decimal subtotal = 0;
            string lineTaxAmountMST;
            decimal lineTaxAmountMSTDecimal = 0;
            string taxItemGroup = "";
            //end
			try
			{
                if (theTransaction != null)
                {
                    string registerId = string.Empty;
                    string controlCode = string.Empty;
                    if (theTransaction.FiscalLineItems.Count > 0)
                    {
                        // Take fiscal line for the current operation, that is a line with the maximum line id
                        var fiscalLine = theTransaction.FiscalLineItems.OrderByDescending(fl => fl.LineId).FirstOrDefault();
                        if (fiscalLine != null)
                        {
                            registerId = fiscalLine.RegisterId;
                            controlCode = string.Concat(fiscalLine.ControlCode, "\n");
                        }
                    }

                    getPurchaser(theTransaction); //add by Erwin
  //                  getEzeelink(theTransaction); //add by Erwin
                    getNoOrder(theTransaction);// added by Heron
               //     getNPWPDetail(theTransaction);// added by Heron

                    //getMemberName(theTransaction);// add by Julius S   //temporary disable for development purpose (Belum dipakai) 10/06/2022 by Yonathan because receipt ID is empty when called this

                    #region CPECRBCA
                    getAmbilTunai(theTransaction); //call function to get AmbilTunai Amount
                    //getAdmTunai(theTransaction);
                    #endregion

                    /*case "CUSTORDERSALES" :
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && cot.OrderType == LSRetailPosis.Transaction.CustomerOrderType.SalesOrder)
                            {
                            

                                return cot.OrderId;
                            }*/
                    //add by Yonathan 10092024 for invoice of cust order
                    if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId == "" && countInfo  == 0)
                    {
                        getInvoiceId(cot.OrderId);
                        string returnString = containerArray[3].ToString();
                        // Load XML into XDocument
                        if (containerArray[1].ToString() != "False")
                        {
                            xdoc = XDocument.Parse(returnString);
                            xml = XElement.Parse(returnString);

                            if (invoiceId == "")
                            {
                                foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                {
                                    var matchingNode = xml.Elements("CustInvoiceTrans")
                                                        .FirstOrDefault(x => x.Attribute("ItemLines")
                                                        .Value.Split(';')[0] == saleLine.ItemId);

                                    if (matchingNode != null)
                                    {
                                        // Split the ItemLines attribute by semicolons
                                        string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                        
                                        invoiceId = itemDetails[8].ToString();

                                       

                                    }
                                    else
                                    {
                                        invoiceId = "";
                                    }
                                }
                                
                            }
                            /*
                                var cultureInfo = new CultureInfo("id-ID");
                                var groupedData = xdoc.Descendants("CustInvoiceTrans")
                                    .Where(e => e.Attribute("ItemLines") != null)
                                    .Select(e => e.Attribute("ItemLines").Value.Split(';'))
                                    .GroupBy(
                                        fields => new { ItemId = fields[0], ItemName = fields[1] }, // Group by ItemId and ItemName
                                        fields => new
                                        {
                                            Quantity = decimal.Parse(fields[2], NumberStyles.Number, cultureInfo),
                                            LineAmount = decimal.Parse(fields[3], NumberStyles.Number, cultureInfo),
                                            SalesId = fields[4]
                                        }
                                    )
                                    .Select(group => new
                                    {
                                        group.Key.ItemId,
                                        group.Key.ItemName,
                                    
                                        //SalesIdCount = group.Select(x => x.SalesId).Distinct().Count()
                                    });

                                            // Output the results
                                            foreach (var data in groupedData)
                                            {

                                                reportLayout.AppendLine(string.Format("{0} - {1}", data.ItemId.ToString(), data.ItemName.ToString()));

                                                reportLayout.AppendLine(string.Format("{0} - {1}", data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));
                                                totalAmount += data.TotalLineAmount;
                                                //totalSales = data.SalesIdCount;
                                            }
                            }*/
                            
                        }
                        countInfo++;

                    }

                    //end
                      
                     
                    switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty))
                    {
                        

                        case "DATE":
                            return ((IPosTransactionV1)theTransaction).BeginDateTime.ToShortDateString(); 
                        case "TIME24H":
                            return ((IPosTransactionV1)theTransaction).BeginDateTime.ToString("HH:mm");
                        case "EFTDATE":
                            return eftInfo.TransactionDateTime.ToShortDateString();
                        case "EFTTIME24H":
                            return eftInfo.TransactionDateTime.ToString("HH:mm");
                        case "EFTACCOUNTTYPE":
                            return eftInfo.AccountType;
                        case "EFTAPPLICATIONID":
                            return eftInfo.ApplicationIdentifier;
                        case "TIME12H":
                            return ((IPosTransactionV1)theTransaction).BeginDateTime.ToString("hh:mm tt");
                        case "TRANSNO":
                            return theTransaction.TransactionId ?? string.Empty;
                        case "TRANSACTIONNUMBER":
                            return theTransaction.TransactionId ?? string.Empty;
                        case "RECEIPTNUMBER":
                            return theTransaction.ReceiptId;
                        case "STAFF_ID":
                            return theTransaction.OperatorId ?? string.Empty;
                        case "EMPLOYEENAME":
                            return theTransaction.OperatorName;
                        case "OPERATORID":
                            return theTransaction.OperatorId ?? string.Empty;
                        case "OPERATORNAME":
                            return theTransaction.OperatorName;
                        case "OPERATORNAMEONRECEIPT":
                            return theTransaction.OperatorNameOnReceipt;
                        case "EMPLOYEEID":
                            return theTransaction.OperatorId;
                        case "SALESPERSONID":
                            return theTransaction.SalesPersonId;
                        case "SALESPERSONNAME":
                            return theTransaction.SalesPersonName;
                        case "SALESPERSONNAMEONRECEIPT":
                            return theTransaction.SalesPersonNameOnReceipt;
                        case "TOTALWITHTAX":
                            //for Customer order - Yonathan 22102024
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "") 
                            {

                                var totalNode = xdoc.Descendants("Total").FirstOrDefault();
                                if (totalNode != null)
                                {
                                    string totalAmount = totalNode.Attribute("TotalAmount").Value;
                                    decimal totalAmountDecimal = decimal.Parse(totalAmount, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));

                                    return Printing.InternalApplication.Services.Rounding.Round(totalAmountDecimal,theTransaction.StoreCurrencyCode, true);
                                }
                                else
                                {
                                    return Printing.InternalApplication.Services.Rounding.Round(theTransaction.NetAmountWithTaxAndCharges, theTransaction.StoreCurrencyCode, true);
                                }
                            }
                            else
                            {
                                return Printing.InternalApplication.Services.Rounding.Round(theTransaction.NetAmountWithTaxAndCharges, theTransaction.StoreCurrencyCode, true);
                            }
                            //return Printing.InternalApplication.Services.Rounding.Round(theTransaction.NetAmountWithTaxAndCharges, theTransaction.StoreCurrencyCode, true);
                        case "REMAININGBALANCE":
                            return Printing.InternalApplication.Services.Rounding.Round(eftInfo.RemainingBalance, theTransaction.StoreCurrencyCode, true);
                        case "TOTAL":
                            // Begin add line NEC
                            {
                            totalCustom = theTransaction.NetAmountWithNoTax;
                            return Printing.InternalApplication.Services.Rounding.Round(theTransaction.NetAmountWithNoTax, theTransaction.StoreCurrencyCode, true);
                            }
                        case "TAXTOTAL":
                            // Begin add line NEC
                            {
                            totalTaxCustom = theTransaction.TaxAmount;
                            return Printing.InternalApplication.Services.Rounding.Round(theTransaction.TaxAmount, theTransaction.StoreCurrencyCode, true);
                            }
                        case "SUMTOTALDISCOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(theTransaction.TotalDiscount, theTransaction.StoreCurrencyCode, true) == "0" ? "" : Printing.InternalApplication.Services.Rounding.Round(theTransaction.TotalDiscount, theTransaction.StoreCurrencyCode, true);
                        case "SUMLINEDISCOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(theTransaction.LineDiscount, theTransaction.StoreCurrencyCode, true);
                        case "SUMALLDISCOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(theTransaction.OverallTotalDiscount, theTransaction.StoreCurrencyCode, true);
                        case "TERMINALID":
                            return theTransaction.TerminalId;
                        case "CASHIER":
                            return LSRetailPosis.Settings.ApplicationSettings.Terminal.TerminalOperator.Name;
                        case "CUSTOMERNAME":
                            getMemberName(theTransaction);
                            if (loyaltyFullName != string.Empty)
                            {
                                return loyaltyFullName;
                            }
                            else
                            {
                                return theTransaction.Customer.Name;
                            }
                        case "CUSTOMERACCOUNTNUMBER":
                            return theTransaction.Customer.CustomerId ?? string.Empty;
                        case "CUSTOMERADDRESS":
                            return theTransaction.Customer.Address;
                        case "CUSTOMERAMOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(theTransaction.AmountToAccount, theTransaction.StoreCurrencyCode, true);
                        case "CUSTOMERVAT":
                            return theTransaction.Customer.VatNum;
                        case "CUSTOMERTAXOFFICE":
                            return theTransaction.Customer.TaxOffice;
                        case "CARDEXPIREDATE":
                            return eftInfo.ExpDate.Substring(0, 2) + "/" + eftInfo.ExpDate.Substring(2, 2);
                        case "CARDNUMBER":
                            return Utility.MaskCardNumber(eftInfo.CardNumber);
                        case "CARDNUMBERPARTLYHIDDEN":
                            return "************-" + eftInfo.CardNumber.Substring(eftInfo.CardNumber.Length - 4, 4);
                        case "CARDTYPE":
                            return eftInfo.CardName;
                        case "CARDISSUERNAME":
                            return eftInfo.IssuerName;
                        case "CARDAMOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(Math.Abs(eftInfo.Amount), theTransaction.StoreCurrencyCode, true);
                        case "CARDAUTHNUMBER":
                            return eftInfo.AuthCode;
                        case "BATCHCODE":
                            return eftInfo.BatchCode;
                        case "ACQUIRERNAME":
                            return eftInfo.AcquirerName;
                        case "VISAAUTHCODE":
                            return eftInfo.VisaAuthCode;
                        case "EUROAUTHCODE":
                            return eftInfo.EuropayAuthCode;
                        case "EFTSTORECODE":
                            return eftInfo.StoreNumber.PadRight(4, '0').Substring(eftInfo.StoreNumber.Length - 4, 4);
                        case "EFTTERMINALNUMBER":
                            return eftInfo.TerminalNumber.PadLeft(4, '0');
                        case "EFTINFOMESSAGE":
                            if (eftInfo.Authorized)
                            {
                                return copyText + eftInfo.AuthorizedText;
                            }
                            else
                            {
                                return eftInfo.NotAuthorizedText;
                            }
                        case "EFTTERMINALID":
                            return eftInfo.TerminalId.PadLeft(8, '0');
                        case "EFTMERCHANTID":
                            return EFT.MerchantId;
                        case "ENTRYSOURCECODE":
                            return eftInfo.EntrySourceCode;
                        case "AUTHSOURCECODE":
                            return eftInfo.AuthSourceCode;
                        case "AUTHORIZATIONCODE":
                            return eftInfo.AuthCode;
                        case "SEQUENCECODE":
                            return eftInfo.SequenceCode;
                        case "EFTMESSAGE":
                            return eftInfo.Message;
                        case "EFTRETRIEVALREFERENCENUMBER":
                            return eftInfo.RetrievalReferenceNumber;
                        case "CUSTOMERTENDERAMOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(tenderItem.Amount, theTransaction.StoreCurrencyCode, true);
                        case "TENDERROUNDING":
                            return Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(theTransaction.TransSalePmtDiff), theTransaction.StoreCurrencyCode, true);
                        case "INVOICECOMMENT":
                            return theTransaction.InvoiceComment;
                        case "TRANSACTIONCOMMENT":
                            return theTransaction.Comment;
                        case "LOGO":
                            //
                            // if there is no specific image for the logo just return <L> as it is used to do before
                            // if there is an image for this logo specified in the designer, then return <L:ImageId> so that 
                            // the image with that id can be retrived and printed
                            //
                            return itemInfo.ImageId == 0 ? LEGACY_LOGO_MESSAGE : string.Format(LOGO_MESSAGE, itemInfo.ImageId.ToString());
                        case "RECEIPTNUMBERBARCODE":
                            return "<B: " + theTransaction.ReceiptId + ">";
                        case "CUSTOMERORDERBARCODE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return "<B: " + cot.OrderId + ">";
                            }
                            return string.Empty;
                        case "REPRINTMESSAGE":
                            return copyText;
                        case "OFFLINEINDICATOR":
                            if (theTransaction.CreatedOffline)
                                return ApplicationLocalizer.Language.Translate(13043);
                            else
                                return string.Empty;
                        case "STOREID":
                            return theTransaction.StoreId;
                        case "STORENAME":
                            return theTransaction.StoreName;
                        case "STOREADDRESS":
                            return theTransaction.StoreAddress;
                        case "STORECITY":
                            return ApplicationSettings.Terminal.StoreCity;
                        case "STORESTATE":
                            return ApplicationSettings.Terminal.StoreState;
                        case "STORECOUNTY":
                            return ApplicationSettings.Terminal.StoreCounty;
                        case "STOREDISTRICT":
                            return ApplicationSettings.Terminal.StoreDistrict;
                        case "STORECOUNTRYREGION":
                            return ApplicationSettings.Terminal.StoreCountry;
                        case "STOREZIP":
                            return ApplicationSettings.Terminal.StoreZip;
                        case "STORESTREET":
                            return ApplicationSettings.Terminal.StoreStreet;
                        case "STOREBUILDINGCOMPLEMENT":
                            return ApplicationSettings.Terminal.StoreBuildingCompliment;
                        case "STOREPOSTBOX":
                            return ApplicationSettings.Terminal.StorePostBox;
                        case "STOREPHONE":
                            return theTransaction.StorePhone;
                        case "STORETAXIDENTIFICATIONNUMBER":
                            return ApplicationSettings.Terminal.TaxIdNumber;
                        case "TENDERAMOUNT":
                            return Printing.InternalApplication.Services.Rounding.Round(Math.Abs(tenderItem.Amount), theTransaction.StoreCurrencyCode, true);
                        case "LOYALTYCARDNUMBER":
                          
                            return theTransaction.LoyaltyItem != null ? "************" + theTransaction.LoyaltyItem.LoyaltyCardNumber.Substring(theTransaction.LoyaltyItem.LoyaltyCardNumber.Length - 4, 4) : string.Empty;

                        case "CREDITMEMONUMBER":
                            {
                                if ((theTransaction.CreditMemoItem == null)
                                    || (theTransaction.CreditMemoItem.CreditMemoNumber == null))
                                {
                                    return string.Empty;
                                }

                                return theTransaction.CreditMemoItem.CreditMemoNumber;
                            }
                        case "CREDITMEMOAMOUNT":
                            {
                                if (theTransaction.CreditMemoItem == null)
                                {
                                    return string.Empty;
                                }

                                return Printing.InternalApplication.Services.Rounding.Round(
                                    theTransaction.CreditMemoItem.Amount, theTransaction.StoreCurrencyCode, true);
                            }
                        case "ALLTENDERCOMMENTS":
                            {
                                string comments = string.Empty;
                                foreach (ITenderLineItem tenderLineItem in theTransaction.TenderLines)
                                {
                                    if (tenderLineItem.Comment != null)
                                    {
                                        comments += tenderLineItem.Comment;
                                    }
                                }

                                return comments;
                            }
                        case "ALLITEMCOMMENTS":
                            {
                                string comments = string.Empty;
                                foreach (ISaleLineItem saleLineItem in theTransaction.SaleItems)
                                {
                                    if (saleLineItem.Comment != null)
                                    {
                                        comments += saleLineItem.Comment;
                                    }
                                }

                                return comments;
                            }
                        case "DEPOSITDUE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return Printing.InternalApplication.Services.Rounding.Round(cot.PrepaymentAmountRequired, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "DEPOSITPAID":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                //When there is a possibility for override, a partial deposit amount can be paid
                                return Printing.InternalApplication.Services.Rounding.Round(cot.PrepaymentAmountPaid, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "DEPOSITREMAINING":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return Printing.InternalApplication.Services.Rounding.Round(cot.PrepaymentAmountPaid - cot.PrepaymentAmountApplied - cot.PrepaymentAmountInvoiced, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "DELIVERYTYPE":
                            if ((theTransaction as CustomerOrderTransaction) != null)
                            {
                                return string.Equals(theTransaction.DeliveryMode.Code, ApplicationSettings.Terminal.PickupDeliveryModeCode, StringComparison.Ordinal)
                                    ? LSRetailPosis.ApplicationLocalizer.Language.Translate(56349) // "Pick up all"
                                    : LSRetailPosis.ApplicationLocalizer.Language.Translate(56348); // "Ship all"
                            }
                            return string.Empty;

                        case "DELIVERYMETHOD":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return cot.DeliveryMode != null ? cot.DeliveryMode.DescriptionText : string.Empty;
                            }
                            return string.Empty;
                        case "DELIVERYDATE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return cot.RequestedDeliveryDate.ToShortDateString();
                            }
                            return string.Empty;
                        case "ORDERTYPE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return cot.OrderType.ToString();
                            }
                            return string.Empty;
                        case "ORDERSTATUS":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                switch (cot.OrderStatus)
                                {
                                    case LSRetailPosis.Transaction.SalesStatus.Canceled:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56379);
                                    case LSRetailPosis.Transaction.SalesStatus.Confirmed:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56404);
                                    case LSRetailPosis.Transaction.SalesStatus.Created:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(99709);
                                    case LSRetailPosis.Transaction.SalesStatus.Delivered:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56377);
                                    case LSRetailPosis.Transaction.SalesStatus.Invoiced:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56378);
                                    case LSRetailPosis.Transaction.SalesStatus.Lost:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56403);
                                    case LSRetailPosis.Transaction.SalesStatus.Processing:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56402);
                                    case LSRetailPosis.Transaction.SalesStatus.Sent:
                                        return LSRetailPosis.ApplicationLocalizer.Language.Translate(56405);
                                    case LSRetailPosis.Transaction.SalesStatus.Unknown:
                                        return string.Empty;
                                    default:
                                        return string.Empty;
                                }
                            }
                            return string.Empty;
                        case "REFERENCENO":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return cot.OrderId ?? string.Empty;
                            }
                            return string.Empty;
                        case "EXPIRYDATE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return cot.ExpirationDate.ToShortDateString();
                            }
                            return string.Empty;
                        case "ORDERID":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return cot.OrderId;
                            }
                            return string.Empty;
                            //add by yonathan 25/05/2023
                        case "LABELSALESORDERID" :
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                lblSalesId = "Sales Order";
                                return lblSalesId;
                            }
                            return string.Empty;
                            //end add
                        //add by yonathan 23/01/2024 for payment discount info
                        case "LABELPROMODISCOUNTPAYMENT":
                            string promoNameDisc = "";
                            if (theTransaction.Comment == "PAYMENTDISCOUNT")
                            {
                                RetailTransaction retailTransaction = theTransaction as RetailTransaction;
                                var discInfoCodeLines = retailTransaction.InfoCodeLines.Where(lines => lines != null && lines.InfocodeId == "DISCOUNT");
                                foreach (var discInfocode in discInfoCodeLines)
                                {

                                    promoNameDisc = checkDiscPayment(discInfocode.Information);
                                    if (!string.IsNullOrEmpty(promoNameDisc))
                                    {
                                        // Stop the loop if promoNameDisc is not empty
                                        break;
                                    }
                                }                                 
                                return promoNameDisc;
                            }
                            return string.Empty;
                            //end
                        //add by yonathan to invclude invoiceid 10092024
                        case "LABELINVOICEID":
                             
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                //string invoiceId = "";
                                
                                if (string.IsNullOrWhiteSpace(invoiceId))
                                {
                                    return string.Empty;
                                }
                                else
                                {
                                    return "Invoice Id";
                                    
                                }
                            }
                            return string.Empty;

                        case "TEXTINVOICEID":

                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                 
                                if (string.IsNullOrWhiteSpace(invoiceId))
                                {
                                    return string.Empty;
                                }
                                else
                                {
                                    return invoiceId; 
                                }
                            }
                            return string.Empty;
                        case "TEXTINFO1":

                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {

                                if (string.IsNullOrWhiteSpace(invoiceId))
                                {
                                    return "BUKAN STRUK INVOICE";
                                }
                                else
                                {
                                    return string.Empty;
                                }
                            }
                            return string.Empty;
                            //end
                        case "TOTALSHIPPIINGCHARGES":
                        case "SHIPPINGCHARGE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                string shippingCode = ApplicationSettings.Terminal.ShippingChargeCode;
                                decimal lineCharges = cot.SaleItems.Where(i => !i.Voided).Sum(c => c.MiscellaneousCharges.Sum(m => (m.ChargeCode == shippingCode ? m.Amount : decimal.Zero)));
                                decimal headerCharges = cot.MiscellaneousCharges.Sum(m => (m.ChargeCode == shippingCode ? m.Amount : decimal.Zero));
                                return Printing.InternalApplication.Services.Rounding.Round(lineCharges + headerCharges, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "CANCELLATIONCHARGE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                string code = ApplicationSettings.Terminal.CancellationChargeCode;
                                decimal sum = decimal.Zero;

                                if (Functions.CountryRegion == SupportedCountryRegion.IN)
                                {
                                    sum = cot.MiscellaneousCharges.Sum(c => (c.ChargeCode == code ? c.Amount - c.TaxAmountInclusive : decimal.Zero));
                                }
                                else
                                {
                                    sum = cot.MiscellaneousCharges.Sum(c => (c.ChargeCode == code ? c.Amount : decimal.Zero));
                                }

                                return Printing.InternalApplication.Services.Rounding.Round(sum, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;

                        case "TAXONCANCELLATIONCHARGE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && Functions.CountryRegion == SupportedCountryRegion.IN)
                            {
                                string code = ApplicationSettings.Terminal.CancellationChargeCode;
                                decimal sum = cot.MiscellaneousCharges.Sum(c => (c.ChargeCode == code ? c.TaxAmount : decimal.Zero));
                                return Printing.InternalApplication.Services.Rounding.Round(sum, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "TOTALCANCELLATIONCHARGE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && Functions.CountryRegion == SupportedCountryRegion.IN)
                            {
                                string code = ApplicationSettings.Terminal.CancellationChargeCode;
                                decimal sum = cot.MiscellaneousCharges.Sum(c => (c.ChargeCode == code ? c.Amount + c.TaxAmountExclusive : decimal.Zero));
                                return Printing.InternalApplication.Services.Rounding.Round(sum, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "TAXONSHIPPING":
                            //Select all charge lines containing chargecode as shipping code. Sum all tax items and return
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                string shippingCode = ApplicationSettings.Terminal.ShippingChargeCode;
                                var sum = decimal.Zero;
                                foreach (var charge in cot.MiscellaneousCharges)
                                {
                                    if (charge.ChargeCode == shippingCode)
                                    {
                                        sum += charge.TaxAmount;
                                    }
                                }
                                return Printing.InternalApplication.Services.Rounding.Round(sum, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "MISCCHARGETOTAL":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                decimal lineCharges = cot.SaleItems.Where(i => !i.Voided).Sum(c => c.MiscellaneousCharges.Sum(m => m.Amount));
                                decimal headerCharges = cot.MiscellaneousCharges.Sum(m => m.Amount);
                                return Printing.InternalApplication.Services.Rounding.Round(lineCharges + headerCharges, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;

                        case "DEPOSITAPPLIED":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                return Printing.InternalApplication.Services.Rounding.Round(cot.PrepaymentAmountApplied, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "TOTALLINEITEMSHIPPINGCHARGES":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                string shippingCode = ApplicationSettings.Terminal.ShippingChargeCode;
                                decimal lineCharges = cot.SaleItems.Where(i => !i.Voided).Sum(c => c.MiscellaneousCharges.Sum(m => (m.ChargeCode == shippingCode ? m.Amount : decimal.Zero)));
                                return Printing.InternalApplication.Services.Rounding.Round(lineCharges, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;

                        case "ORDERSHIPPINGCHARGE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                string shippingCode = ApplicationSettings.Terminal.ShippingChargeCode;
                                decimal headerCharges = cot.MiscellaneousCharges.Sum(m => (m.ChargeCode == shippingCode ? m.Amount : decimal.Zero));
                                return Printing.InternalApplication.Services.Rounding.Round(headerCharges, theTransaction.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "TOTALPAYMENTS":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                var totalPayments = TotalPayments(cot);
                                return Printing.InternalApplication.Services.Rounding.Round(totalPayments, cot.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "BALANCE":
                            if ((cot = theTransaction as CustomerOrderTransaction) != null)
                            {
                                var balance = cot.NetAmountWithTaxAndCharges - TotalPayments(cot);
                                return Printing.InternalApplication.Services.Rounding.Round(balance, cot.StoreCurrencyCode, true);
                            }
                            return string.Empty;
                        case "INVOICEACCOUNTNUMBER":
                            if (!string.IsNullOrEmpty(theTransaction.Customer.InvoiceAccount))
                            {
                                return theTransaction.Customer.InvoiceAccount;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        case "INVOICECUSTOMERADDRESS":
                            // POS sets the invoiced cusotmer to the primary customer if invoice accoutn is not set up,
                            // in which case we don't want to print out the primary customer address again as the invoice cusotmer address
                            if (!string.IsNullOrEmpty(theTransaction.Customer.InvoiceAccount))
                            {
                                return (theTransaction.InvoicedCustomer != null) ? theTransaction.InvoicedCustomer.Address : string.Empty;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        case "INVOICECUSTOMERNAME":
                            // POS sets the invoiced cusotmer to the primary customer if invoice accoutn is not set up
                            // in which case we don't want to print out the primary customer name again as the invoice cusotmer name
                            if (!string.IsNullOrEmpty(theTransaction.Customer.InvoiceAccount))
                            {
                                return (theTransaction.InvoicedCustomer != null) ? theTransaction.InvoicedCustomer.Name : string.Empty;
                            }
                            else
                            {
                                return string.Empty;
                            }
                            
                        //add NEC-Hmz to cutomize Receipt on POS


                        //Edit By Erwin 23 July 2019
                        //case "DPP": return GetTotalNetAmount(theTransaction);
                        case "DPP": 
                             
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                            {
                                lineAmountIncTaxDecimal = 0;
                                lineAmountIncTax = "";
                                subtotal = 0;
                                lineTaxAmountMST = "";
                                lineTaxAmountMSTDecimal = 0;
                                // Lookup the matching XML node for the current salesLine.ItemId

                                if (ApplicationSettings.Terminal.StoreTaxGroup == "TAX")
                                {
                                    foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                    {
                                        var matchingNode = xml.Elements("CustInvoiceTrans")
                                                            .FirstOrDefault(x => x.Attribute("ItemLines")
                                                            .Value.Split(';')[0] == saleLine.ItemId);

                                        if (matchingNode != null)
                                        {
                                            // Split the ItemLines attribute by semicolons
                                            string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                            lineAmountIncTax = itemDetails[3];
                                            lineTaxAmountMST = itemDetails[6];
                                            taxItemGroup = itemDetails[7];

                                            if (taxItemGroup == "PPN")
                                            if (string.Equals(taxItemGroup, "PPN", StringComparison.OrdinalIgnoreCase))

                                            {
                                                lineAmountIncTaxDecimal += decimal.Parse(lineAmountIncTax, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                                lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                            }

                                            //    lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));

                                            


                                        }
                                        //else
                                        //{
                                        //    subtotal = 0;
                                        //}
                                    }
                                    subtotal = lineAmountIncTaxDecimal - lineTaxAmountMSTDecimal; //#PPN12

                                    DPPPKP = subtotal;
                                    return String.Format("{0:#,0}", DPPPKP);
                                }
                                else //if (ApplicationSettings.Terminal.StoreTaxGroup == "NonPKP")
                                {
                                    foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                    {
                                        var matchingNode = xml.Elements("CustInvoiceTrans")
                                                            .FirstOrDefault(x => x.Attribute("ItemLines")
                                                            .Value.Split(';')[0] == saleLine.ItemId);

                                        if (matchingNode != null)
                                        {
                                            // Split the ItemLines attribute by semicolons
                                            string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                            lineAmountIncTax = itemDetails[3];
                                            lineTaxAmountMST = itemDetails[6];
                                            
                                            lineAmountIncTaxDecimal += decimal.Parse(lineAmountIncTax, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                            lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));


                                        }
                                        //else
                                        //{
                                        //    subtotal = 0;
                                        //}
                                    }
                                    subtotal = lineAmountIncTaxDecimal;// -lineTaxAmountMSTDecimal; //#PPN12
                                    DPPPKP = subtotal;
                                    return String.Format("{0:#,0}", DPPPKP);
                                }

                               
                                //return String.Format("{0:#,0}", subtotal * 11 / 12); //03012025 #PPN12


                            }
                            else
                            {
                                DPPPKP = GetTotalNetAmount(theTransaction);
                                return String.Format("{0:#,0}",
                                DPPPKP); //- GetTotalPB1(theTransaction));
                            }
                        //additional info receipt for TAX - Yonathan 21012024
                        case "LABELITEMBKP":
                            return "Kena Pajak";
                        case "LABELDPPOTHER":
                            if (statusDPP2025Print == "True")
                            {
                                return "DPP Nilai Lain :";
                            }
                            else
                            {
                                return string.Empty;
                            }
                           
                            
                        case "LABELITEMBEBAS":
                            return "Pajak Dibebaskan";
                        case "LABELDPPBEBAS":
                            return "DPP :";
                        case "LABELDPPLAINBEBAS":
                            if (statusDPP2025Print == "True")
                            {
                                return "DPP Nilai Lain :";
                            }
                            else
                            { 
                                return string.Empty;
                            }
                        case "LABELPPNBEBAS":
                            return "PPN :";
                        case "LABELITEMNON":
                            return "Non Pajak";
                        case "LABELDPPNON":
                            return "DPP :";
                        case "LABELDPPLAINNON":
                            if (statusDPP2025Print == "True")
                            {
                                return "DPP Nilai Lain :";
                            }
                            else
                            {
                                return string.Empty;
                            } 
                        case "LABELPPNNON":
                            return "PPN :";

                        //case "TEXTITEMBKP":
                            //return String.Format("{0:#,0}", getTotalBKP(theTransaction));
                        //case "TEXTDPPOTHER":
                        //    return "DPP Nilai Lain :";
                        //case "TEXTITEMBEBAS":
                            //return String.Format("{0:#,0}", getTotalBebas(theTransaction));
                        case "TEXTDPPBEBAS":
                            //check DPP dibebaskan
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                            {
                                lineAmountIncTaxDecimal = 0;
                                lineAmountIncTax = "";
                                subtotal = 0;
                                lineTaxAmountMST = "";
                                lineTaxAmountMSTDecimal = 0;
                                // Lookup the matching XML node for the current salesLine.ItemId

                                foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                {
                                    var matchingNode = xml.Elements("CustInvoiceTrans")
                                                        .FirstOrDefault(x => x.Attribute("ItemLines")
                                                        .Value.Split(';')[0] == saleLine.ItemId);

                                    if (matchingNode != null)
                                    {
                                        // Split the ItemLines attribute by semicolons
                                        string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');
                                        // if (string.Equals(taxItemGroup, 
                                        // , StringComparison.OrdinalIgnoreCase))
                                        lineAmountIncTax = itemDetails[3];
                                        lineTaxAmountMST = itemDetails[6];
                                        taxItemGroup = itemDetails[7];

                                        if (string.Equals(taxItemGroup, "Dibebaskan", StringComparison.OrdinalIgnoreCase))

                                        //if (taxItemGroup == "Dibebaskan")
                                        {
                                            lineAmountIncTaxDecimal += decimal.Parse(lineAmountIncTax, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                            lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                        }

                                        


                                    }
                                    //else
                                    //{
                                    //    subtotal = 0;
                                    //}
                                }
                                subtotal = lineAmountIncTaxDecimal - lineTaxAmountMSTDecimal; //#PPN12

                                DPPBebas = subtotal;
                                return String.Format("{0:#,0}", DPPBebas);
                                //return String.Format("{0:#,0}", subtotal * 11 / 12); //03012025 #PPN12


                            }
                            else
                            {
                                DPPBebas = getDPPBebas(theTransaction);
                                return String.Format("{0:#,0}", DPPBebas);
                            }
                            
                            //}
                             
                        case "TEXTDPPLAINBEBAS":
                            if (statusDPP2025Print == "True")
                            {
                                //validate status
                                if (DPPNilaiLainStatus == true)
                                {
                                    return String.Format("{0:#,0}", DPPBebas * 11 / 12);
                                }
                                else
                                {
                                    return "0";
                                }
                            }
                            else
                            {
                                return string.Empty;
                            }
                           
                             
                        case "TEXTPPNBEBAS":
                            return String.Format("{0:#,0}", getPPNBebas(theTransaction)); ;
                            
                        case "TEXTITEMNON":
                            return "0";
                        case "TEXTDPPNON":
                            //check DPP dibebaskan
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                            {
                                lineAmountIncTaxDecimal = 0;
                                lineAmountIncTax = "";
                                subtotal = 0;
                                lineTaxAmountMST = "";
                                lineTaxAmountMSTDecimal = 0;
                                // Lookup the matching XML node for the current salesLine.ItemId
                                foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                {
                                    var matchingNode = xml.Elements("CustInvoiceTrans")
                                                        .FirstOrDefault(x => x.Attribute("ItemLines")
                                                        .Value.Split(';')[0] == saleLine.ItemId);

                                    if (matchingNode != null)
                                    {
                                        // Split the ItemLines attribute by semicolons
                                        string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                        lineAmountIncTax = itemDetails[3];
                                        lineTaxAmountMST = itemDetails[6];
                                        taxItemGroup = itemDetails[7];
                                        if (string.Equals(taxItemGroup, "PPN", StringComparison.OrdinalIgnoreCase))

                                        //if (taxItemGroup == "PPN")
                                        {
                                            lineAmountIncTaxDecimal += decimal.Parse(lineAmountIncTax, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                            lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                        }

                                    }
                                    else
                                    {
                                        subtotal = 0;
                                    }
                                }
                                subtotal = lineAmountIncTaxDecimal - lineTaxAmountMSTDecimal; //#PPN12
                                DPPNonPajak = subtotal;
                                return String.Format("{0:#,0}", DPPNonPajak);
                                //return String.Format("{0:#,0}", subtotal * 11 / 12); //03012025 #PPN12


                            }
                            else
                            {
                                DPPNonPajak = getDPPNonPajak(theTransaction);
                                return String.Format("{0:#,0}", DPPNonPajak);
                            }
                            
                            //}
                        case "TEXTDPPLAINNON":
                            if (statusDPP2025Print == "True")
                            {
                                return "0";
                            }
                            else
                            {
                                return string.Empty;
                            }
                            //return "0";
                        case "TEXTPPNNON":
                            return "0";
                        
                        case "TEXTDPPOTHER":
                            if (statusDPP2025Print == "True")
                            {

                                return String.Format("{0:#,0}", DPPPKP *11/12);
                               //GetTotalNetAmountDPPLain(theTransaction) - GetTotalPB1(theTransaction));
                            }
                            else
                            {
                                return string.Empty;
                            }
                           
                        //End Edit By Erwin 23 July 2019
                        //case "DPP": return String.Format("{0:0,0}", totalCustom);
                        case "PPN": 
                             
                            if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                            {
                                lineAmountIncTaxDecimal = 0;
                                lineAmountIncTax = "";
                                subtotal = 0;
                                lineTaxAmountMST = "";
                                lineTaxAmountMSTDecimal = 0;
                                // Lookup the matching XML node for the current salesLine.ItemId
                                foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                {
                                    var matchingNode = xml.Elements("CustInvoiceTrans")
                                                        .FirstOrDefault(x => x.Attribute("ItemLines")
                                                        .Value.Split(';')[0] == saleLine.ItemId);

                                    if (matchingNode != null)
                                    {
                                        // Split the ItemLines attribute by semicolons
                                        string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                       
                                        lineTaxAmountMST = itemDetails[6];
                                        taxItemGroup = itemDetails[7];
                                        if (string.Equals(taxItemGroup, "PPN", StringComparison.OrdinalIgnoreCase) || string.Equals(taxItemGroup, "PKP", StringComparison.OrdinalIgnoreCase))
                                            lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                        //if (taxItemGroup == "PPN" || taxItemGroup == "PKP")
                                        


                                    }
                                    
                                }
                                return String.Format("{0:#,0}", subtotal =  lineTaxAmountMSTDecimal);
                            }
                            else
                            { 
                                return GetTotalPPN(theTransaction);
                            }

                        case "PB1": if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                            {
                                // Lookup the matching XML node for the current salesLine.ItemId
                                foreach (SaleLineItem saleLine in theTransaction.SaleItems)
                                {
                                    var matchingNode = xml.Elements("CustInvoiceTrans")
                                                        .FirstOrDefault(x => x.Attribute("ItemLines")
                                                        .Value.Split(';')[0] == saleLine.ItemId);

                                    if (matchingNode != null)
                                    {
                                        // Split the ItemLines attribute by semicolons
                                        string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                       
                                        lineTaxAmountMST = itemDetails[6];
                                        taxItemGroup = itemDetails[7];
                                        if (string.Equals(taxItemGroup, "PB1", StringComparison.OrdinalIgnoreCase))
                                        //if (taxItemGroup == "PB1")
                                        lineTaxAmountMSTDecimal += decimal.Parse(lineTaxAmountMST, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));


                                    }
                                    
                                }
                                return String.Format("{0:#,0}", subtotal = lineTaxAmountMSTDecimal);
                            }
                            else
                            {
                                return String.Format("{0:#,0}", GetTotalPB1(theTransaction));
                            }

                        // India receipt tax summary
                        case "COMPANYPANNO_IN":
                            return ApplicationSettings.Terminal.IndiaCompanyPanNumber;
                        case "VATTINNO_IN":
                            return ApplicationSettings.Terminal.IndiaVatTinNumber;
                        case "CSTTINNO_IN":
                            return ApplicationSettings.Terminal.IndiaCstTinNumber;
                        case "STCNUMBER_IN":
                            return ApplicationSettings.Terminal.IndiaSTCNumber;
                        case "ECCNUMBER_IN":
                            return ApplicationSettings.Terminal.IndiaECCNumber;
                        case "PROMOTIONALSERVICE":
                            {
                                var receiptMessage = Utility.CreateReceiptMessage();

                                Printing.InternalApplication.Services.Receipt.GetPromotionalMessage(theTransaction, receiptMessage);

                                return receiptMessage.MessageLines.Any() ? string.Join(Environment.NewLine, receiptMessage.MessageLines) : string.Empty;
                            }
                        case "FISCALREGISTERNUMBER":
                            return registerId;
                        case "FISCALCONTROLCODE":
                            return controlCode;
                        //ADD BY HERON

                        //add by yonathan 04/12/2023 to show no order id from Grabmart
                        case "LABELORDERNO":
                            if (OrderName == "GRABMART") //add by yonathan 04/12/2023 to show no order id from Grabmart
                            {
                                return "Grab Order No";
                            }
                            return string.Empty;
                        case "NOORDER":
                            if (OrderName == "GRABMART") //add by yonathan 04/12/2023 to show no order id from Grabmart
                            {
                                return OrderNo;
                            }
                            return string.Empty;
                        case "NAMEORDER ":

                            return OrderName;
                        //add by Yonathan 11/01/2024
                        case "EARNEDREWARDPOINTAMOUNTQUANTITY":
                            decimal qtyPointEarn = 0;
                            decimal amtPointEarn = 0;
                            string returnValueEarn ="";
                            var filteredRewardLinesEarn = theTransaction.LoyaltyItem.RewardPointLines
                                                    .Where(lines => lines.EntryType == LoyaltyRewardPointEntryType.Earn);

                            foreach (var line in filteredRewardLinesEarn)
                            {

                                qtyPointEarn += line.RewardPointAmountQuantity;

                                switch (line.RewardPointType)
                                {
                                    case LoyaltyRewardPointType.Quantity:
                                        returnValueEarn = Printing.InternalApplication.Services.Rounding.Round(qtyPointEarn, 0, false);
                                        break;
                                    case LoyaltyRewardPointType.Amount:
                                        returnValueEarn = Printing.InternalApplication.Services.Rounding.Round(qtyPointEarn, line.RewardPointCurrency, true);
                                        break;
                                }
                            }
                            return returnValueEarn;

                        case "REDEEMEDREWARDPOINTAMOUNTQUANTITY":

                            decimal qtyPointRedeem = 0;
                            decimal amtPointRedeem = 0;
                            string returnValueRedeem = "";
                             var filteredRewardLinesRedeem = theTransaction.LoyaltyItem.RewardPointLines
                                                     .Where(lines => lines.EntryType == LoyaltyRewardPointEntryType.Redeem);

                             foreach (var line in filteredRewardLinesRedeem)
                             {

                                 qtyPointRedeem += line.RewardPointAmountQuantity;
                                 switch (line.RewardPointType)
                                 {
                                     case LoyaltyRewardPointType.Quantity:
                                         returnValueRedeem = Printing.InternalApplication.Services.Rounding.Round(qtyPointRedeem, 0, false);
                                         break;
                                     case LoyaltyRewardPointType.Amount:
                                         returnValueRedeem = Printing.InternalApplication.Services.Rounding.Round(qtyPointRedeem, line.RewardPointCurrency, true);
                                         break;
                                 }
                             }
                             return returnValueRedeem;
                            break;
                    /*    case "NPWPADDRESS":

                            return npwpAddress;
                        case "NPWPAREA":

                            return npwpArea;

                        //ADD BY HERON
                     */
                        //ADD BY ERWIN
                        case "PURCHASERNAME":
                            return purchaserName;
                   //         return loyaltyFullName;
                    //        return "purchaser 1";
                       //     return eftInfo.CardName + " " + eftInfo.IssuerName;// "Testing Purchaser Name";
                        case "PURCHASERPHONE":
                        //    return loyaltyPhone;
                      
                            return purchaserPhone;
                        case "LOYALTYNAME":                           
                            return loyaltyFullName;

                        //    return theTransaction.Customer.MobilePhone +" "+ eftInfo.IssuerNumber;
  /*                      case "LOYALTYEZEELINK":
                            if (lblCardNumber != string.Empty)
                            {
                                string maskCardNumber = "";
                                string returnNumber = "";

                                if (cardNumber.Length > 4)
                                {
                                    maskCardNumber = cardNumber.Substring(cardNumber.Length - 4);
                                }

                                for (int i = 0; i < cardNumber.Length - 4; i++)
                                {
                                    returnNumber += "*";
                                }
                                return returnNumber + maskCardNumber;
                            }
                            else 
                                return string.Empty; 
                        case "LOYALTYEZEELINKBALANCE":
                            if (lblLastBalance != string.Empty)
                                return lastBalance.ToString();
                            else if (lblTotalBalance != string.Empty)
                                return totalBalance.ToString();
                            else
                                return string.Empty;
                        case "LOYALTYEZEELINKREDEEM":
                            if (lblRedeemPoints != string.Empty)
                                return redeemPoints.ToString();
                            else if (lblEarnPoints != string.Empty)
                                return earnPoints.ToString();
                            else
                                return string.Empty;
                        case "LABELEZEELINK":
                            return lblCardNumber;
                        case "LABELEZEELINKTRANSID":
                            return lblTransID;
                        case "LOYALTYEZEELINKTRANSID":
                            return transID;
                        case "LABELEZEELINKBALANCE":
                            if (lblLastBalance != string.Empty)
                                return lblLastBalance;
                            else if (lblTotalBalance != string.Empty)
                                return lblTotalBalance;
                            else
                                return string.Empty;
                        case "LABELEZEELINKREDEEM":
                            if (lblRedeemPoints != string.Empty)
                                return lblRedeemPoints;
                            else if (lblEarnPoints != string.Empty)
                                return lblEarnPoints;
                            else
                                return string.Empty;
                            */
                        //END ADD BY ERWIN



                        #region CPECRBCA
                        //set amount to variable based on settings from receipt format AX
                        /*case "DIBEBASKAN":
                            return String.Format("{0:0,0}", totalDibebaskan);*/
                        case "LABELAMBILTUNAI":
                            return amountAmbilTunai <= 0 ? "" : "Ambil Tunai";
                            //return "Ambil Tunai";
                        case "AMOUNTAMBILTUNAI":
                            return amountAmbilTunai <= 0 ? "" : Printing.InternalApplication.Services.Rounding.Round(amountAmbilTunai, false); //String.Format("{0,0}", amountAmbilTunai);
                        case "LABELTUNAIFEE": // add by Yonathan 10/06/2024 //CPIADMFEE
                            return amountAmbilTunai <= 0 ? "" : "Biaya Admin";
                        //return "Ambil Tunai";
                        case "AMOUNTTUNAIFEE":
                            return amountAmbilTunai <= 0 ? "" : Printing.InternalApplication.Services.Rounding.Round(amountAdmFee, false); //CPIADMFEE

                            //add by Yonathan 06112024
                        case "LABELORDERID":
                            if (APIAccessClass.xmlString1.ToString() != "")
                            {
                               

                                return "Order Id :";


                            }
                            else
                            {
                                return "";
                            }
                        case "TEXTORDERID":
                            //APIAccessClass.xmlString1 = "";
                            if (APIAccessClass.xmlString1.ToString() != "")
                            {
                                XDocument doc = XDocument.Parse(APIAccessClass.xmlString1);
                                // Get the value of <CPOrderNumber>
                                string CPOrderNumber = "";
                                return CPOrderNumber = doc.Root.Element("CPOrderNumber") != null ? doc.Root.Element("CPOrderNumber").Value : "";
                            }
                            else
                            {
                                return "";
                            }
                        case "LABELDELIVERYORDER":
                            if (APIAccessClass.xmlString1.ToString() != "")
                            {
                                

                                return "Deliver To :";    

                              
                            }
                            else
                            {
                                return "";
                            }
                                                   

                        case "TEXTDELIVERYORDER":
                            //APIAccessClass.xmlString1 = "";
                            if (APIAccessClass.xmlString1.ToString() != "")
                            {
                                XDocument doc = XDocument.Parse(APIAccessClass.xmlString1);
                                 // Get the value of <CPOrderNumber>
                                string CPBuyerName = doc.Root.Element("CPBuyerName") != null ? doc.Root.Element("CPBuyerName").Value : "";
                                string CPPhoneNumber = doc.Root.Element("CPPhone") != null ? doc.Root.Element("CPPhone").Value : "";
                                string CustomDeliveryAdd = doc.Root.Element("CustomerDeliveryAddress") != null ? doc.Root.Element("CustomerDeliveryAddress").Value : "";


                                return CPBuyerName + "(" + CPPhoneNumber + ")" + "\n" + CustomDeliveryAdd;
                            }
                            else
                            {
                                return "";
                            }
                            
                        #endregion
                    }


                    // Brazil NFC-e and CF-e fields
                    /*if (theTransaction.SubType == RetailTransactionSubType.ConsumerEFDocument || theTransaction.SubType == RetailTransactionSubType.ElectronicFiscalReceipt || theTransaction.SubType == RetailTransactionSubType.ElectronicFiscalReceiptCancelling)
                    {
                        var value = formModulationEfd.GetInfoFromFiscalDocument(itemInfo, theTransaction);

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            return value;
                        }

                    }*/
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                NetTracer.Warning("Printing [FormModulation] - Invalid Argument when trying to get information from transcation");
                return string.Empty;
            }
            catch (NullReferenceException)
            {
                NetTracer.Warning("Printing [FormModulation] - Null reference when trying to get information from transcation");
                return string.Empty;
            }

            return string.Empty;
        }

        private decimal getPPNBebas(RetailTransaction theTransaction)
        {
            //custom by Yonathan 29102024 for invoice
            CustomerOrderTransaction cot;
            decimal subtotal = 0;
            decimal ppnPct = 0;
            if (ApplicationSettings.Terminal.StoreTaxGroup == "TAX")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        //if (s.TaxGroupId == "Dibebaskan" && !s.Voided)
                        if (s.TaxGroupId.Equals("Dibebaskan", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                        {
                            ppnPct = getTaxPct("PPN");
                            subtotal += Math.Round(s.NetAmount * (ppnPct / 100));
                        }/* ppnPct = Math.Round(((s.NetAmountWithTax - s.NetAmount) / s.NetAmount) * 100);

                        subtotal += Math.Round((ppnPct/100) * s.NetAmount);*/

                    }
                    //Edit By Erwin 23 July 2019
                    //return String.Format("{0:0,0}", subtotal);

                    return subtotal; //03012025 #PPN12;
                    //End Edit By Erwin 23 July 2019
                }
                catch
                {
                    //Edit By Erwin 23 July 2019
                    //return string.Empty;
                    return 0;
                    //End Edit By Erwin 23 July 2019
                }
            }
            else //if (ApplicationSettings.Terminal.StoreTaxGroup == "NON")
            {
                return 0;
            }
           
        }

        private decimal getTaxPct(string p)
        {
            decimal taxPct = 0;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            //string invoiceId = "";
            try
            {
                string queryString = @"SELECT TAXVALUE
                                        FROM [ax].[TAXDATA]
                                        WHERE TAXCODE = 'PPN'
                                          AND GETDATE() BETWEEN TAXFROMDATE AND TAXTODATE";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    //command.Parameters.AddWithValue("@SALESID", _salesId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                taxPct = reader.GetDecimal(0);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }
            return taxPct;
        }

        private decimal getDPPNonPajak(RetailTransaction theTransaction)
        {
            //custom by Yonathan 29102024 for invoice
            CustomerOrderTransaction cot;
            decimal subtotal = 0;

            try
            {

                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                    //if (s.TaxGroupId == "NonPPN" && !s.Voided)
                    if (s.TaxGroupId.Equals("NonPPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                        subtotal += s.NetAmountWithNoTax;

                }
                //Edit By Erwin 23 July 2019
                //return String.Format("{0:0,0}", subtotal);

                return subtotal; //03012025 #PPN12;
                //End Edit By Erwin 23 July 2019
            }
            catch
            {
                //Edit By Erwin 23 July 2019
                //return string.Empty;
                return 0;
                //End Edit By Erwin 23 July 2019
            }
        }

        private decimal getDPPBebas(RetailTransaction theTransaction)
        {
            //custom by Yonathan 29102024 for invoice
            CustomerOrderTransaction cot;
            decimal subtotal = 0;

            try
            {

                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                    //if (s.TaxGroupId == "Dibebaskan" && !s.Voided)
                    if (s.TaxGroupId.Equals("Dibebaskan", StringComparison.OrdinalIgnoreCase) && !s.Voided)

                        subtotal += s.NetAmountWithNoTax;
                     
                }
                //Edit By Erwin 23 July 2019
                //return String.Format("{0:0,0}", subtotal);

                return subtotal; //03012025 #PPN12;
                //End Edit By Erwin 23 July 2019
            }
            catch
            {
                //Edit By Erwin 23 July 2019
                //return string.Empty;
                return 0;
                //End Edit By Erwin 23 July 2019
            }
        }

        private string getTotalBebas(RetailTransaction theTransaction)
        {
            throw new NotImplementedException();
        }

        private decimal getTotalBKP(RetailTransaction theTransaction)
        {
            try
            {
                decimal subTotalBKP = 0;
                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                    //if (s.TaxGroupId != "PPN" && !s.Voided)
                    if (s.TaxGroupId.Equals("PPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)

                    {
                        subTotalBKP = s.NetAmountWithTax;
                    }
                }
                //Edit By Erwin 23 July 2019
                //return String.Format("{0:0,0}", subTotalPB1);  
                return subTotalBKP;
                //return "12";
                //End Edit By Erwin 23 July 2019
            }
            catch
            {
                //Edit By Erwin 23 July 2019                
                //return string.Empty;
                return 0;
                //End Edit By Erwin 23 July 2019
            }
        }

        private void getInvoiceId(string _salesId)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            //string invoiceId = "";
            try
            {
                string queryString = @"SELECT INVOICECOMMENT,SALESORDERID,RECEIPTID FROM [ax].[RETAILTRANSACTIONTABLE]  where SALESORDERID = '"+_salesId+"'";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    //command.Parameters.AddWithValue("@SALESID", _salesId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                invoiceId = reader.GetString(0);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }


            //get info invoice item from AX - Yonathan 07102024
            containerArray = Printing.InternalApplication.TransactionServices.InvokeExtension("getInvoiceSalesOrder", _salesId); 
            //end
            //return invoiceId;
        }

        private string checkDiscPayment(string infocodeId)
        {

            
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string promoName = "";
            try
            {
                string queryString = @"SELECT [PROMOID], [PROMONAME]  FROM [ax].[CPPROMOCASHBACK] WHERE PROMOID = @PromoId";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@PromoId", infocodeId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                promoName =  reader.GetString(1);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }

            return promoName;
         
        }

        //<summary>
        //Added by NEC-Hmz to custom print receipt @ POS
        //</summary>
        private decimal GetTotalNetAmount(RetailTransaction theTransaction)
        {
            //custom by Yonathan 29102024 for invoice
            CustomerOrderTransaction cot;
            decimal subtotal = 0;

            if (ApplicationSettings.Terminal.StoreTaxGroup == "TAX")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        //if (s.TaxGroupId == "PPN" && !s.Voided)
                            if (s.TaxGroupId.Equals("PPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                            subtotal += s.NetAmountWithNoTax;

                        //disable for PPN only, other tax group will be splitted - 22012025
                        //else if (s.TaxGroupId != "PPN" && !s.Voided) //NECI_YNWA start modified because code change in CU10 regarding the column NetAmountWithNoTax is zero if the item type is BKTP
                        //subtotal += s.NetAmount; //NECI_YNWA end modified
                    }
                    //Edit By Erwin 23 July 2019
                    //return String.Format("{0:0,0}", subtotal);

                    //return subtotal; //03012025 #PPN12;
                    //End Edit By Erwin 23 July 2019
                }
                catch
                {
                    //Edit By Erwin 23 July 2019
                    //return string.Empty;
                    subtotal =  0;
                    //End Edit By Erwin 23 July 2019
                }
            
            }
            else if (ApplicationSettings.Terminal.StoreTaxGroup == "NonPKP")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        //if (s.TaxGroupId == "PPN" && !s.Voided)
                        if (s.TaxGroupId.Equals("PPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                            subtotal += s.NetAmount;

                        //disable for PPN only, other tax group will be splitted - 22012025
                        //else if (s.TaxGroupId != "PPN" && !s.Voided) //NECI_YNWA start modified because code change in CU10 regarding the column NetAmountWithNoTax is zero if the item type is BKTP
                        //subtotal += s.NetAmount; //NECI_YNWA end modified
                    }
                    //Edit By Erwin 23 July 2019
                    //return String.Format("{0:0,0}", subtotal);

                    //return subtotal; //03012025 #PPN12;
                    //End Edit By Erwin 23 July 2019
                }
                catch
                {
                    //Edit By Erwin 23 July 2019
                    //return string.Empty;
                    subtotal = 0;
                    //End Edit By Erwin 23 July 2019
                }
            }


            return subtotal;
        }

        private decimal GetTotalNetAmountDPPLain(RetailTransaction theTransaction)
        {
            //custom by Yonathan 29102024 for invoice
            CustomerOrderTransaction cot;
            decimal subtotal = 0;
            if (ApplicationSettings.Terminal.StoreTaxGroup == "TAX")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        //if (s.TaxGroupId == "PPN" && !s.Voided)
                            if (s.TaxGroupId.Equals("PPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                            subtotal += s.NetAmountWithNoTax;
                        /*else if (s.TaxGroupId != "PPN" && !s.Voided) //NECI_YNWA start modified because code change in CU10 regarding the column NetAmountWithNoTax is zero if the item type is BKTP
                            subtotal += s.NetAmount; //NECI_YNWA end modified*/
                    }
                    //Edit By Erwin 23 July 2019
                    //return String.Format("{0:0,0}", subtotal);
                    subtotal = subtotal * 11 / 12;
                    return subtotal; //03012025 #PPN12;
                    //End Edit By Erwin 23 July 2019
                }
                catch
                {
                    //Edit By Erwin 23 July 2019
                    //return string.Empty;
                    return 0;
                    //End Edit By Erwin 23 July 2019 
                }

            } 
            else //if (ApplicationSettings.Terminal.StoreTaxGroup == "NON")
            {
                try
                {

                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        //if (s.TaxGroupId == "PPN" && !s.Voided)
                            if (s.TaxGroupId.Equals("PPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                            subtotal += s.NetAmount;

                        //disable for PPN only, other tax group will be splitted - 22012025
                        //else if (s.TaxGroupId != "PPN" && !s.Voided) //NECI_YNWA start modified because code change in CU10 regarding the column NetAmountWithNoTax is zero if the item type is BKTP
                        //subtotal += s.NetAmount; //NECI_YNWA end modified
                    }
                    subtotal = subtotal * 11 / 12;
                    return subtotal; 
                    //Edit By Erwin 23 July 2019
                    //return String.Format("{0:0,0}", subtotal);

                    //return subtotal; //03012025 #PPN12;
                    //End Edit By Erwin 23 July 2019
                }
                catch
                {
                    //Edit By Erwin 23 July 2019
                    //return string.Empty;
                    return subtotal = 0;
                    //End Edit By Erwin 23 July 2019
                }
            }
               
            

            
        }

        private string GetTotalPPN(RetailTransaction theTransaction)
        {
            if (ApplicationSettings.Terminal.StoreTaxGroup == "TAX")
            {
                try
                {
                    decimal subTotalPPN = 0;
                    foreach (SaleLineItem s in theTransaction.SaleItems)
                    {
                        //if (s.TaxGroupId == "PPN" && !s.Voided)
                            if (s.TaxGroupId.Equals("PPN", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                            subTotalPPN += s.TaxAmount;
                    }

                    return String.Format("{0:#,0}", subTotalPPN);
                }
                catch
                {
                    return String.Format("{0:#,0}", 0); // string.Empty;
                }
            }
            else if (ApplicationSettings.Terminal.StoreTaxGroup == "NonPKP")
            {
                return String.Format("{0:#,0}", 0);
            }
            else
            {
                return String.Format("{0:#,0}", 0);
            }
            
        }
        private decimal GetTotalPB1(RetailTransaction theTransaction)
        {
            try
            {
                decimal subTotalPB1 = 0;
                foreach (SaleLineItem s in theTransaction.SaleItems)
                {
                   // if (s.TaxGroupId == "PB1")
                    if (s.TaxGroupId.Equals("PB1", StringComparison.OrdinalIgnoreCase) && !s.Voided)
                        subTotalPB1 += s.TaxAmount;
                }
                //Edit By Erwin 23 July 2019
                //return String.Format("{0:0,0}", subTotalPB1);  
                return subTotalPB1;
                //return "12";
                //End Edit By Erwin 23 July 2019
            }
            catch
            {
                //Edit By Erwin 23 July 2019                
                //return string.Empty;
                return 0;
                //End Edit By Erwin 23 July 2019
            }
        }
        // End Add NEC-Hmz


        /// <summary>
        /// All payments paid in any form (deposit, pickup etc.) 
        /// </summary>
        /// <param name="cot"></param>
        /// <returns></returns>
        private static decimal TotalPayments(CustomerOrderTransaction cot)
        {
            decimal totalPayments = 0.0M;
            foreach (var payment in cot.PaymentHistory)
            {
                // sum up payments
                decimal amount = (string.IsNullOrWhiteSpace(payment.Currency))
                    ? payment.Amount
                    : (Printing.InternalApplication.Services.Currency.CurrencyToCurrency(
                        payment.Currency,
                        ApplicationSettings.Terminal.StoreCurrency,
                        payment.Amount));

                totalPayments += amount;
            }
            return totalPayments;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode"), SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfather")]
        private string GetInfoFromSaleLineItem(FormItemInfo itemInfo, SaleLineItem saleLine, IPosTransaction theTransaction)
        {

            string returnValue = string.Empty;
            CustomerOrderTransaction cot;
            if (saleLine == null)
            {
                return returnValue;
            }
            try
            {
                PharmacySalesLineItem asPharmacySalesLineItem = saleLine as PharmacySalesLineItem;

                switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty))
                {
                    case "TAXID":
                        returnValue = saleLine.TaxGroupId;
                        break;
                    case "TAXPERCENT":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.TaxRatePct, false);
                        break;

                    case "TAXFLAG":
                        returnValue = GetTaxFlag(saleLine);
                        break;
                    case "ITEMNAME":
                        returnValue = saleLine.Description;
                        break;
                    case "ITEMNAMEALIAS":
                        returnValue = saleLine.DescriptionAlias;
                        break;
                    case "ITEMID":
                        returnValue = saleLine.ItemId;
                        break;
                    case "ITEMBARCODE":
                        returnValue = saleLine.BarcodeId;
                        break;
                    case "QTY":
                        if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                        {
                              
                                // Lookup the matching XML node for the current salesLine.ItemId
                            var matchingNode = xml.Elements("CustInvoiceTrans")  
                                                    .FirstOrDefault(x => x.Attribute("ItemLines")
                                                    .Value.Split(';')[0] == saleLine.ItemId);

                            if (matchingNode != null)
                            {
                                // Split the ItemLines attribute by semicolons
                                string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                // Retrieve ItemName and Qty
                                string itemName = itemDetails[1];
                                string qtyString = itemDetails[2];
                                decimal qty = decimal.Parse(qtyString, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));

                                returnValue = Printing.InternalApplication.Services.Rounding.RoundQuantity(qty, saleLine.SalesOrderUnitOfMeasure);
                                qtyPerLine = qty;
                            }
                            else
                            {
                                returnValue = Printing.InternalApplication.Services.Rounding.RoundQuantity(saleLine.Quantity, saleLine.SalesOrderUnitOfMeasure);
                            }
                                
                            
                            
                            //var groupedData = xdoc.Descendants("CustInvoiceTrans")
                            //   .Where(e => e.Attribute("ItemLines") != null)
                            //   .Select(e => e.Attribute("ItemLines").Value.Split(';'))
                            //   .GroupBy(
                            //       fields => new { ItemId = fields[0], ItemName = fields[1] }, // Group by ItemId and ItemName
                            //       fields => new
                            //       {
                            //           Quantity = decimal.Parse(fields[2], NumberStyles.Number, cultureInfo),
                            //           LineAmount = decimal.Parse(fields[3], NumberStyles.Number, cultureInfo),
                            //           SalesId = fields[4]
                            //       }
                            //   )
                            //   .Select(group => new
                            //   {
                            //       group.Key.ItemId,
                            //       group.Key.ItemName,

                            //       //SalesIdCount = group.Select(x => x.SalesId).Distinct().Count()
                            //   });

                            //// Output the results
                            //foreach (var data in groupedData)
                            //{

                            //    reportLayout.AppendLine(string.Format("{0} - {1}", data.ItemId.ToString(), data.ItemName.ToString()));

                            //    reportLayout.AppendLine(string.Format("{0} - {1}", data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));
                            //    totalAmount += data.TotalLineAmount;
                            //    //totalSales = data.SalesIdCount;
                            //}
                        }
                        else
                        {
                            returnValue = Printing.InternalApplication.Services.Rounding.RoundQuantity(saleLine.Quantity, saleLine.SalesOrderUnitOfMeasure);
                        }
                        
                        break;
                    case "UNITPRICE":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.Price, false);
                        break;
                    case "UNITPRICEWITHTAX":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.NetAmountWithTax / saleLine.Quantity, false);
                        break;
                    case "TOTALPRICE":
                        if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                        {
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(qtyPerLine * saleLine.Price, false);
                        }
                        else
                        {
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.GrossAmount, false);
                        }
                        
                        break;
                    case "TOTALPRICEWITHTAX":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.NetAmountWithTax, false);
                        break;
                    case "ITEMUNITID":
                        returnValue = saleLine.SalesOrderUnitOfMeasure;
                        break;
                    case "ITEMUNITIDNAME":
                        returnValue = saleLine.SalesOrderUnitOfMeasureName;
                        break;
                    case "LINEDISCOUNTAMOUNT":

                        //for Customer order - Yonathan 22102024  
                        if ((cot = theTransaction as CustomerOrderTransaction) != null && invoiceId != "")
                        {

                            // Lookup the matching XML node for the current salesLine.ItemId
                            var matchingNode = xml.Elements("CustInvoiceTrans")  
                                                    .FirstOrDefault(x => x.Attribute("ItemLines")
                                                    .Value.Split(';')[0] == saleLine.ItemId);

                            if (matchingNode != null)
                            {
                                // Split the ItemLines attribute by semicolons
                                string[] itemDetails = matchingNode.Attribute("ItemLines").Value.Split(';');

                                // Retrieve ItemName and Qty
                                string itemName = itemDetails[1];
                                string qtyString = itemDetails[2];
                                decimal discAmount = decimal.Parse(itemDetails[4], System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                decimal qty = decimal.Parse(qtyString, System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
                                returnValue = Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(discAmount) * qty, false);
                                
                            }
                            else
                            {
                                returnValue = Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(saleLine.LineDiscount), false);
                            }

                            //returnValue = ""; //lineamountinctax //Printing.InternalApplication.Services.Rounding.Round(qtyPerLine * decimal.Negate(saleLine.LineDiscount), false);
                        }
                        else
                        {
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(saleLine.LineDiscount), false);
                        }
                        //returnValue = Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(saleLine.LineDiscount), false); //original
                        break;
                    case "LINEDISCOUNTPERCENT":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.LinePctDiscount, false);
                        break;
                    case "FISCALDOCUMENTLINEORIGINALLINEDISCOUNT_BR":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.LoyaltyDiscount + saleLine.PeriodicDiscount + saleLine.LineDiscount, false);
                        break;
                    case "DISTRIBUTEDDISCOUNTPERLINE":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.TotalDiscount, false);
                        break;
                    case "PERIODICDISCOUNTAMOUNT":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(saleLine.PeriodicDiscount), false);
                        break;
                    case "PERIODICDISCOUNTNAME":
                        if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.AggregateItemsForPrinting == true)
                        {
                            if (saleLine.PeriodicDiscountPossibilities.Count == 1)
                            {
                                returnValue = (saleLine.PeriodicDiscountPossibilities.First() as LSRetailPosis.Transaction.Line.Discount.PeriodicDiscountItem).OfferId;
                            }
                            else if (saleLine.PeriodicDiscountPossibilities.Count > 1)
                            {
                                returnValue = "Offers";
                            }
                        }
                        else
                        {
                            returnValue = saleLine.TotalPeriodicOfferId;
                        }

                        break;
                    case "PERIODICDISCOUNTPERCENT":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.PeriodicPctDiscount, false);
                        break;
                    case "TOTALDISCOUNTAMOUNT":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(decimal.Negate(saleLine.TotalDiscount), false);
                        break;
                    case "TOTALDISCOUNTPERCENT":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.TotalPctDiscount, false);
                        break;
                    case "PHARMACYDOSAGESTRENGTH":
                        returnValue = Utility.ToString(asPharmacySalesLineItem.DosageStrength);
                        break;
                    case "PHARMACYPRESCRIPTIONNUMBER":
                        returnValue = Utility.ToString(asPharmacySalesLineItem.PrescriptionId);
                        break;
                    case "PHARMACYDOSAGESTRENGTHUNIT":
                        returnValue = asPharmacySalesLineItem.DosageStrengthUnit;
                        break;
                    case "PHARMACYDOSAGEUNITQTY":
                        returnValue = Utility.ToString(asPharmacySalesLineItem.DosageUnitQuantity);
                        break;
                    case "PHARMACYDOSAGETYPE":
                        returnValue = asPharmacySalesLineItem.DosageType;
                        break;
                    case "BATCHID":
                        returnValue = saleLine.BatchId;
                        break;
                    case "BATCHEXPDATE":
                        returnValue = saleLine.BatchExpDate.ToShortDateString();
                        break;
                    case "ITEMGROUP":
                        returnValue = saleLine.ItemGroupId;
                        break;
                    case "DIMENSIONCOLORID":
                        returnValue = saleLine.Dimension.ColorId;
                        break;
                    case "DIMENSIONCOLORVALUE":
                        returnValue = saleLine.Dimension.ColorName;
                        break;
                    case "DIMENSIONSIZEID":
                        returnValue = saleLine.Dimension.SizeId;
                        break;
                    case "DIMENSIONSIZEVALUE":
                        returnValue = saleLine.Dimension.SizeName;
                        break;
                    case "DIMENSIONSTYLEID":
                        returnValue = saleLine.Dimension.StyleId;
                        break;
                    case "DIMENSIONSTYLEVALUE":
                        returnValue = saleLine.Dimension.StyleName;
                        break;
                    case "DIMENSIONCONFIGID":
                        returnValue = saleLine.Dimension.ConfigId;
                        break;
                    case "DIMENSIONCONFIGVALUE":
                        returnValue = saleLine.Dimension.ConfigName;
                        break;
                    case "ITEMCOMMENT":
                        returnValue = saleLine.Comment;
                        break;
                    case "SERIALID":
                        returnValue = saleLine.SerialId;
                        break;
                    case "RFID":
                        returnValue = saleLine.RFIDTagId;
                        break;
                    case "UNITOFMEASURE":
                        // (e.g. 1.5Lb @ $1.99/Lb)
                        returnValue = string.Format("{0}{2} @ {1}/{2}", Printing.InternalApplication.Services.Rounding.RoundQuantity(saleLine.Quantity, saleLine.SalesOrderUnitOfMeasure),
                            Printing.InternalApplication.Services.Rounding.Round(saleLine.Price, theTransaction.StoreCurrencyCode, true), saleLine.SalesOrderUnitOfMeasure);
                        break;
                    case "LINEDELIVERYTYPE":
                        returnValue = string.Empty;
                        break;
                    case "LINEDELIVERYMETHOD":
                        returnValue = saleLine.DeliveryMode != null ? saleLine.DeliveryMode.DescriptionText : string.Empty;
                        break;
                    case "LINEDELIVERYDATE":
                        returnValue = (saleLine.DeliveryDate.HasValue ? saleLine.DeliveryDate.Value.ToShortDateString() : string.Empty);
                        break;
                    case "PICKUPQTY":
                        returnValue = Printing.InternalApplication.Services.Rounding.RoundQuantity(saleLine.Quantity, saleLine.SalesOrderUnitOfMeasure);
                        break;
                    case "LINEITEMSHIPPINGCHARGE":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(GetShippingCharge(saleLine), theTransaction.StoreCurrencyCode, true);
                        break;
                    case "LINENUMBER":
                        returnValue = string.Format("{0}", saleLine.LineId);
                        break;
                    case "ITEMTAX":
                        returnValue = Printing.InternalApplication.Services.Rounding.Round(saleLine.TaxAmount, theTransaction.StoreCurrencyCode, true);
                        break;
                    case "APPROXIMATETAXAMOUNT":
                        //returnValue = formModulationEfd.GetApproximateTaxAmountLine(saleLine, (RetailTransaction) theTransaction);
                        break;
                    case "KITCOMPONENTNAME":
                        returnValue = saleLine.Description;
                        break;
                    case "KITCOMPONENTQTY":
                        returnValue = Printing.InternalApplication.Services.Rounding.RoundQuantity(saleLine.Quantity, saleLine.SalesOrderUnitOfMeasure);
                        break;
                    case "KITCOMPONENTUNIT":
                        returnValue = saleLine.SalesOrderUnitOfMeasure;
                        break;
                    case "RETURNREASON":
                        returnValue = saleLine.ReturnReasonText;
                        break;
                    case "RETURNLOCATION":
                        returnValue = saleLine.ReturnLocationText;
                        break;
                    case "RETURNWAREHOUSE":
                        returnValue = saleLine.ReturnWarehouseText;
                        break;
                    case "RETURNPALLETE":
                        returnValue = saleLine.ReturnPalleteText;
                        break;
                    //add by Yonathan to show the Retail Variant ID 24/11/2022
                    case "RETAILVARIANTID":
                        returnValue = "      " + saleLine.Dimension.VariantId;
                        break;
                    //end
                    


                }

                if (returnValue == null)
                {
                    returnValue = string.Empty;
                }
                else
                {
                    if (itemInfo.Prefix.Length > 0)
                    {
                        returnValue = itemInfo.Prefix + returnValue;
                    }
                }
            }
            catch (DivideByZeroException)
            {
                NetTracer.Warning("Printing [FormModulation] - Sale line quantity must be greater than zero");

                returnValue = string.Empty;
            }
            catch (NullReferenceException)
            {
                NetTracer.Warning("Printing [FormModulation] - Null reference when trying to get information from sales line item");

                returnValue = string.Empty;
            }

            return returnValue;
        }

        private static string GetTaxFlag(SaleLineItem saleLine)
        {
            var returnValue = string.Empty;
            var hasExempt = false;
            var hasZero = (saleLine.TaxLines.Count == 0); // Init with false if any tax lines; otherwise, set to true if no tax lines

            foreach (ITaxItem taxLine in saleLine.TaxLines)
            {
                if (taxLine.Exempt)
                {
                    hasExempt = true;
                }
                if (taxLine.Percentage == decimal.Zero)
                {
                    hasZero = true;
                }
            }

            if (hasZero && hasExempt)
            {
                returnValue = ApplicationLocalizer.Language.Translate(13041) + " " + ApplicationLocalizer.Language.Translate(13040); //Zero Tax & Exempt
            }
            else if (hasZero)
            {
                returnValue = ApplicationLocalizer.Language.Translate(13041); //Zero tax
            }
            else if (hasExempt)
            {
                returnValue = ApplicationLocalizer.Language.Translate(13040); //Exempt
            }
            return returnValue;
        }

        private static bool IsKit(SaleLineItem saleLine)
        {
            ItemData itemData = new ItemData(ApplicationSettings.Database.LocalConnection,
                ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);

            return itemData.DoesKitExist(saleLine.ItemId);
        }

        private static decimal GetShippingCharge(SaleLineItem saleLine)
        {
            var shippingCode = ApplicationSettings.Terminal.ShippingChargeCode;
            return saleLine.MiscellaneousCharges.Sum(m => (m.ChargeCode == shippingCode ? m.Amount : decimal.Zero));
        }

        private static string GetInfoFromTenderLineItem(FormItemInfo itemInfo, ITenderLineItem tenderLine, IPosTransaction theTransaction)
        {
            string returnValue = string.Empty;
            ITender tenderInfo;
            try
            {
                if (tenderLine != null)
                {
                    switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty))
                    {
                        case "TENDERNAME":
                            if (tenderLine.Amount < 0)
                            {
                                TenderData tenderData = new TenderData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                                tenderInfo = tenderData.GetTender(tenderLine.TenderTypeId, theTransaction.StoreId);
                                if (tenderInfo.FunctionType == 1 || tenderInfo.FunctionType == 3) // FunctionType = 1 (cards), FunctionType = 3 (customer account)
                                {
                                    returnValue = ApplicationLocalizer.Language.Translate(13033); // "charge back"
                                }
                                else
                                {
                                    returnValue = ApplicationLocalizer.Language.Translate(13031); // "change back"
                                }
                                returnValue += " (" + tenderLine.Description + ")";
                            }
                            else
                            {
                                //Edit By Erwin Siswanto - 5 October 2018
                                TenderData tenderData = new TenderData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                                tenderInfo = tenderData.GetTender(tenderLine.TenderTypeId, theTransaction.StoreId);

                                if ((PosisOperations)tenderInfo.PosisOperation == PosisOperations.PayCustomerAccount)
                                {
                                    returnValue = tenderInfo.TenderName;
                                }
                                else
                                {
                                    returnValue = tenderLine.Description;
                                }
                                //END Edit By Erwin Siswanto - 5 October 2018
                            }

                            if (tenderLine.ForeignCurrencyAmount != 0)
                            {
                                returnValue += " (" + Printing.InternalApplication.Services.Rounding.Round(tenderLine.ForeignCurrencyAmount, tenderLine.CurrencyCode, false) + " " + tenderLine.CurrencyCode + ")";
                            }
                            break;
                        case "TENDERAMOUNT":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(Math.Abs(tenderLine.Amount), theTransaction.StoreCurrencyCode, true);
                            break;
                        case "TENDERCOMMENT":
                            returnValue = tenderLine.Comment;
                            break;
                    }

                    if (returnValue == null)
                    {
                        returnValue = string.Empty;
                    }
                }
            }
            catch (NullReferenceException)
            {
                NetTracer.Warning("Printing [FormModulation] - Null reference when trying to get information from tender line item");

                returnValue = string.Empty;
            }

            if (returnValue.Length > itemInfo.Length)
            {
                returnValue = Wrap(returnValue, itemInfo);
            }

            return returnValue;
        }

        private static string GetInfoFromLoyaltyRewardPointSummary(FormItemInfo itemInfo, LoyaltyRewardPointEntryType entryType, LoyaltyRewardPointSummary rewardLine)
        {
            string returnValue = string.Empty;

            try
            {
                if (rewardLine != null)
                {
                    switch (itemInfo.Variable.ToUpperInvariant())
                    {
                        case "EARNEDREWARDPOINTID":
                        case "REDEEMEDREWARDPOINTID":
                            returnValue = rewardLine.RewardPointId;
                            break;
                        case "EARNEDREWARDPOINTAMOUNTQUANTITY":
                        case "REDEEMEDREWARDPOINTAMOUNTQUANTITY":
                            decimal rewardPoints = rewardLine.RewardPointAmountQuantity;
                            if (entryType == LoyaltyRewardPointEntryType.Redeem || entryType == LoyaltyRewardPointEntryType.Refund)
                            {
                                rewardPoints = rewardPoints * -1;
                            }

                            switch (rewardLine.RewardPointType)
                            {
                                case LoyaltyRewardPointType.Quantity:
                                    returnValue = Printing.InternalApplication.Services.Rounding.Round(rewardPoints, 0, false);
                                    break;
                                case LoyaltyRewardPointType.Amount:
                                    returnValue = Printing.InternalApplication.Services.Rounding.Round(rewardPoints, rewardLine.RewardPointCurrency, true);
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (NullReferenceException)
            {
                NetTracer.Warning("Printing [FormModulation] - Null reference when trying to get information from loyalty line item");

                returnValue = string.Empty;
            }

            if (returnValue.Length > itemInfo.Length)
            {
                returnValue = Wrap(returnValue, itemInfo);
            }

            return returnValue;
        }

        private static string GetInfoFromTaxItem(FormItemInfo itemInfo, ITaxItem taxLine)
        {
            string returnValue = string.Empty;
            try
            {
                if (taxLine != null)
                {
                    switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty))
                    {
                        case "TAXID":
                            returnValue = taxLine.TaxCode;
                            break;
                        case "TAXGROUP":
                            returnValue = taxLine.TaxGroup;
                            break;
                        case "TAXBASIS":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.TaxBasis * Math.Sign(taxLine.Amount), false);
                            break;
                        case "TAXPERCENTAGE":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.Percentage, 2, false);
                            break;
                        case "TOTAL":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.Price, false);
                            break;
                        case "TAXAMOUNT":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.Amount, false);
                            break;
                        case "PRICE":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.Price - taxLine.Amount, false);
                            break;
                        case "TAXFLAG":
                            if (taxLine.Exempt)
                            {
                                returnValue = ApplicationLocalizer.Language.Translate(13040); //Exempt
                            }
                            else if (taxLine.Percentage == decimal.Zero)
                            {
                                returnValue = ApplicationLocalizer.Language.Translate(13041); //Zero tax
                            }
                            break;

                        case "BASICAMOUNT_IN":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.TaxBasis, false);
                            break;
                        case "TOTALAMOUNT_IN":
                            returnValue = Printing.InternalApplication.Services.Rounding.Round(taxLine.TaxBasis + taxLine.Amount, false);
                            break;
                        case "TAXCOMPONENT_IN":
                            TaxItemIndia taxLineIN = taxLine as TaxItemIndia;
                            if (taxLineIN != null)
                            {
                                returnValue = taxLineIN.TaxComponent;
                            }
                            break;
                    }

                    if (returnValue == null)
                    {
                        returnValue = string.Empty;
                    }
                }
            }
            catch (NullReferenceException)
            {
                NetTracer.Warning("Printing [FormModulation] - Null reference when trying to get information from tax item");

                returnValue = string.Empty;
            }

            if (returnValue.Length > itemInfo.Length)
            {
                returnValue = Wrap(returnValue, itemInfo);
            }

            return returnValue;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemInfo", Justification = "Grandfather")]
        private static int CountWordLength(string wordString, FormItemInfo itemInfo)
        {
            int count = 0;
            foreach (char ch in wordString)
            {
                if ((ch != '\r') && (ch != '\n'))
                {
                    count += 1;
                }
            }

            return count;
        }

        private static string GetAlignmentSettings(string parsedString, FormItemInfo itemInfo)
        {
            // the indication of a logo, barcode or qrcode, which are passed on unchanged.
            if (string.Compare(parsedString, LOGO_MESSAGE, StringComparison.OrdinalIgnoreCase) == 0 ||
                Regex.IsMatch(parsedString, logoRegEx, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(parsedString, barCodeRegEx, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(parsedString, qrCodeRegEx, RegexOptions.IgnoreCase))
            {
                return parsedString;
            }

            if (parsedString.Length > itemInfo.Length)
            {
                // don't trim strings that contain carrage return, they are considered to be previously handled
                if (!parsedString.Contains(Environment.NewLine))
                {
                    // The value seems to need to be trimmed
                    switch (itemInfo.VertAlign)
                    {
                        case valign.right:
                            parsedString = parsedString.Substring(parsedString.Length - itemInfo.Length, itemInfo.Length);
                            break;
                        default:
                            parsedString = parsedString.Substring(0, itemInfo.Length);
                            break;
                    }
                }
                else
                {
                    // If it contains carriage returns, don't forget to insert indents
                    string indent = Environment.NewLine + CreateWhitespace(itemInfo.CharIndex - 1, ' ');
                    parsedString = parsedString.Replace(Environment.NewLine, indent);
                }
            }
            else if (parsedString.Length < itemInfo.Length)
            {
                // The value seems to need to be filled
                int charCountUsableForSpace = itemInfo.Length - CountWordLength(parsedString, itemInfo);
                switch (itemInfo.VertAlign)
                {
                    case valign.left:
                        parsedString = parsedString.PadRight(itemInfo.Length, itemInfo.Fill);
                        break;
                    case valign.center:
                        int spaceOnLeftSide = (int)charCountUsableForSpace / 2;
                        int spaceOnRightSide = charCountUsableForSpace - spaceOnLeftSide;
                        parsedString = parsedString.PadLeft(spaceOnLeftSide + parsedString.Length, itemInfo.Fill);
                        parsedString = parsedString.PadRight(spaceOnRightSide + parsedString.Length, itemInfo.Fill);
                        break;
                    case valign.right:
                        parsedString = parsedString.PadLeft(charCountUsableForSpace + parsedString.Length, itemInfo.Fill);
                        break;
                }
            }

            return parsedString;
        }

        private static string ParseTenderLineVariable(FormItemInfo itemInfo, ITenderLineItem tenderLineItem, IPosTransaction theTransaction)
        {
            string variableString = GetInfoFromTenderLineItem(itemInfo, tenderLineItem, theTransaction);

            // Setting the align if neccessary
            return GetAlignmentSettings(variableString, itemInfo);
        }

        private string ParseItemVariable(FormItemInfo itemInfo, SaleLineItem saleLineItem, IPosTransaction theTransaction)
        {
            string parsedString = string.Empty;

            if (itemInfo.IsVariable)
            {
                parsedString = GetInfoFromSaleLineItem(itemInfo, saleLineItem, theTransaction);
            }
            else
            {
                parsedString = itemInfo.ValueString;
            }

            // Setting the align if neccessary
            parsedString = GetAlignmentSettings(parsedString, itemInfo);

            return parsedString;
        }

        private string ParseCardTenderVariable(FormItemInfo itemInfo, IEFTInfo eftInfo, RetailTransaction theTransaction)
        {
            string tmpString = string.Empty;

            if (itemInfo.IsVariable)
            {
                tmpString = GetInfoFromTransaction(itemInfo, eftInfo, null, theTransaction);
            }
            else
            {
                tmpString = itemInfo.ValueString;
            }

            // Setting the align if neccessary
            tmpString = GetAlignmentSettings(tmpString, itemInfo);

            return tmpString;
        }

        private string ParseTenderVariable(FormItemInfo itemInfo, ITenderLineItem tenderLineItem, RetailTransaction theTransaction)
        {
            string tmpString = string.Empty;

            if (itemInfo.IsVariable)
            {
                tmpString = GetInfoFromTransaction(itemInfo, null, tenderLineItem, theTransaction);
            }
            else
            {
                tmpString = itemInfo.ValueString;
            }

            // Setting the align if neccessary
            tmpString = GetAlignmentSettings(tmpString, itemInfo);

            return tmpString;
        }

        private static string ParseTaxVariable(FormItemInfo itemInfo, ITaxItem taxItem)
        {
            string tmpString = string.Empty;

            if (itemInfo.IsVariable)
            {
                tmpString = GetInfoFromTaxItem(itemInfo, taxItem);
            }
            else
            {
                tmpString = itemInfo.ValueString;
            }

            // Setting the align if neccessary
            tmpString = GetAlignmentSettings(tmpString, itemInfo);

            return tmpString;
        }

        private string ParseVariable(FormItemInfo itemInfo, ITenderLineItem tenderItem, RetailTransaction theTransaction)
        {
            string tmpString = string.Empty;

            if (itemInfo.IsVariable)
            {
                tmpString = GetInfoFromTransaction(itemInfo, null, tenderItem, theTransaction); 
            }
            else
            {
                tmpString = itemInfo.ValueString;
            }

            if (tmpString == null)
            {
                tmpString = string.Empty;
            }

            if (tmpString.IndexOf("\n") != -1)
            {
                string[] tmpNewLines = tmpString.Split('\n');
                for (int i = 0; i <= tmpNewLines.Length - 1; i++)
                {
                    //When the current line has length more than the max characters, wrap the text.
                    if (tmpNewLines[i].Length > itemInfo.Length)
                    {
                        //Wrap the current line text and assign back to the tmpNewLines[i]
                        tmpNewLines[i] = ParseLengthyVariable(tmpNewLines[i], itemInfo);
                    }
                    else
                    {
                        tmpNewLines[i] = GetAlignmentSettings(tmpNewLines[i], itemInfo);
                    }
                    if (i == 0)
                    {
                        tmpString = tmpNewLines[i];
                    }
                    else
                    {
                        tmpNewLines[i] = CreateWhitespace(itemInfo.CharIndex - 1, ' ') + tmpNewLines[i];
                        tmpString += Environment.NewLine + tmpNewLines[i];
                    }
                }
            }
            else
            {
                // Setting the align if neccessary
                tmpString = GetAlignmentSettings(tmpString, itemInfo);
            }

            return tmpString;
        }

        /// <summary>
        /// When the tempLine has a lengthy text value than the maximum length of the line, 
        /// ParseLengthyVariable wraps the text and sets alignment as required.
        /// </summary>
        /// <param name="tempLine">Line</param>
        /// <param name="itemInfo">FormItemInfo</param>
        /// <returns>Wrapped lengthy string</returns>
        private static string ParseLengthyVariable(string tempLine, FormItemInfo itemInfo)
        {
            string tmpString = string.Empty;
            //Iterate till the tempLine becomes 0
            while (tempLine.Length != 0)
            {
                if (tempLine.Length > itemInfo.Length)
                {
                    tmpString += string.Concat(tempLine.Substring(0, itemInfo.Length), Environment.NewLine);
                    tempLine = tempLine.Substring(itemInfo.Length, tempLine.Length - itemInfo.Length);
                }
                else
                {
                    tempLine = GetAlignmentSettings(tempLine, itemInfo);
                    tmpString += tempLine;
                    tempLine = string.Empty;
                }
            }

            //Set the alignment finally and return the string.
            tmpString = GetAlignmentSettings(tmpString, itemInfo);
            return tmpString;
        }

        private string ReadCardTenderDataSet(DataSet ds, IEFTInfo eftInfo, RetailTransaction theTransaction)
        {
            String returnString = string.Empty;

            // Note: Receipt templates are persisted as searilzed DataTable objects. Don't change the case
            // of these tables/fields for backward compatibility.

            DataTable lineTable = ds.Tables["line"];
            DataTable charPosTable;
            FormItemInfo itemInfo = null;

            if (lineTable != null)
            {
                foreach (DataRow dr in lineTable.Select(string.Empty, "nr asc"))
                {
                    String lineString = string.Empty;
                    string idVariable = (string)dr["ID"];

                    if (idVariable == "CRLF")
                    {
                        lineString += Environment.NewLine;
                    }
                    else if ((idVariable == "CardHolderSignature") && (eftInfo.CardType != EFTCardTypes.CREDIT_CARD))
                    {
                        // Skip card holder signature line for other than Credit Cards.
                    }
                    else
                    {
                        string drLineId = Utility.ToString(dr["line_id"]);
                        charPosTable = ds.Tables["charpos"];
                        if (charPosTable != null)
                        {
                            int nextCharNr = 1;
                            foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                            {
                                try
                                {
                                    itemInfo = new FormItemInfo(row);

                                    // Adding possible whitespace at the beginning of line
                                    lineString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                    // Adding marker for text formatting
                                    lineString += GetFontFormatStyle(itemInfo);

                                    // Parsing the itemInfo
                                    lineString += ParseCardTenderVariable(itemInfo, eftInfo, theTransaction);

                                    // Closing the string with a single space command to make sure spaces are always single spaced
                                    lineString += esc + NORMAL_TEXT_MARKER;

                                    // Specifing the position of the next char in the current line - bold take twice as much space
                                    nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                }
                                catch (Exception ex)
                                {
                                    ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                                }
                            }
                        }

                        lineString += Environment.NewLine;
                    }

                    returnString += lineString;
                }
            }

            return returnString.ToString();
        }

        private string ReadTenderDataSet(DataSet ds, ITenderLineItem tenderLineItem, RetailTransaction theTransaction)
        {
            String returnString = string.Empty;

            DataTable lineTable = ds.Tables["line"];
            DataTable charPosTable;
            FormItemInfo itemInfo = null;

            if (lineTable != null)
            {
                foreach (DataRow dr in lineTable.Select(string.Empty, "nr asc"))
                {
                    String lineString = string.Empty;
                    string idVariable = (string)dr["ID"];

                    switch (idVariable)
                    {
                        case "CRLF":
                            lineString += Environment.NewLine;
                            break;
                        case "Text":
                            string drLineId = Utility.ToString(dr["line_id"]);
                            charPosTable = ds.Tables["charpos"];
                            if (charPosTable != null)
                            {
                                int nextCharNr = 1;
                                foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                                {
                                    try
                                    {
                                        itemInfo = new FormItemInfo(row);

                                        // Adding possible whitespace at the beginning of line
                                        lineString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                        // Adding marker for text formatting
                                        lineString += GetFontFormatStyle(itemInfo);

                                        // Parsing the itemInfo
                                        lineString += ParseTenderVariable(itemInfo, tenderLineItem, theTransaction);

                                        // Closing the string with a single space command to make sure spaces are always single spaced
                                        lineString += esc + NORMAL_TEXT_MARKER;

                                        // Specifing the position of the next char in the current line - bold take twice as much space
                                        nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            lineString += Environment.NewLine;
                            break;
                    }

                    returnString += lineString;
                }
            }

            return returnString.ToString();
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfather")]
        private void ParseSaleItem(DataSet ds, ref FormItemInfo itemInfo, StringBuilder lIneStringBuilder, SaleLineItem saleLineItem, IPosTransaction theTransaction)
        {
            string idVariable = string.Empty;
            string drLineId = string.Empty;
            saleLineItem.GetType();
            // Only non-voided items will be printed
            if (!saleLineItem.Voided)
            {
                DataTable lineTable = ds.Tables["line"];
                DataTable charPosTable;
                if (lineTable != null)
                {
                    foreach (DataRow dr in lineTable.Select(string.Empty, "nr asc"))
                    {
                        idVariable = Utility.ToString(dr["ID"]);
                        switch (idVariable)
                        {
                            case "CRLF":
                                {
                                    lIneStringBuilder.Append(Environment.NewLine);
                                    break;
                                }
                            default:
                                {
                                    drLineId = Utility.ToString(dr["line_id"]);

                                    if ((idVariable == "PharmacyLine") && (!(saleLineItem is PharmacySalesLineItem))) { /*donothing*/ }
                                    else if ((idVariable == "TotalDiscount") && (saleLineItem.TotalDiscount == 0)) { /*donothing*/ }
                                    else if ((idVariable == "LineDiscount") && (saleLineItem.LineDiscount == 0)) { /*donothing*/ }
                                    else if ((idVariable == "AnyLineDiscount") && (saleLineItem.LineDiscount == 0 && saleLineItem.PeriodicDiscount == 0 && saleLineItem.LoyaltyDiscount == 0)) { /*donothing*/ }
                                    else if ((idVariable == "PeriodicDiscount") && (saleLineItem.PeriodicDiscount == 0)) { /*donothing*/ }
                                    else if ((idVariable == "Batch") && (string.IsNullOrEmpty(saleLineItem.BatchId))) { /*donothing*/ }
                                    else if (idVariable == "UnitOfMeasure" && !saleLineItem.ScaleItem) { /*donothing*/ }
                                    else if (idVariable == "ManualWeight" && !(saleLineItem.ScaleItem && saleLineItem.WeightManuallyEntered)) { /*donothing*/ }
                                    else if ((idVariable == "Dimension")
                                             && ((string.IsNullOrEmpty(saleLineItem.Dimension.ColorName))
                                                 && (string.IsNullOrEmpty(saleLineItem.Dimension.SizeName))
                                                 && (string.IsNullOrEmpty(saleLineItem.Dimension.StyleName))
                                                 && (string.IsNullOrEmpty(saleLineItem.Dimension.ConfigName)))) { /*donothing*/ }
                                    else if ((idVariable == "Comment") && (string.IsNullOrEmpty(saleLineItem.Comment))) { /*donothing*/ }
                                    else if ((idVariable == "KitComponentName")) //handle kit components separately from other line items to repeat line for each component
                                    {
                                        //parse kit variables if item is a kit and store parameters indicate that components should be printed
                                        if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.IncludeKitComponents && IsKit(saleLineItem))
                                        {
                                            ItemData itemData = new ItemData(ApplicationSettings.Database.LocalConnection,
                                                    ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);

                                            //get list of components in kit
                                            using (DataTable dt = itemData.GetRetailKitVariantComponents(saleLineItem.Dimension.VariantId))
                                            {
                                                //for each component in the kit, print the same line config
                                                foreach (DataRow component in dt.Rows)
                                                {
                                                    charPosTable = ds.Tables["charpos"];
                                                    if (charPosTable != null)
                                                    {
                                                        int nextCharNr = 1;
                                                        //format and print each field in the line being printed
                                                        foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                                                        {
                                                            try
                                                            {
                                                                itemInfo = new FormItemInfo(row);
                                                                // Adding possible whitespace at the beginning of line
                                                                lIneStringBuilder.Append(CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

                                                                //formatting for different font styles
                                                                lIneStringBuilder.Append(GetFontFormatStyle(itemInfo));

                                                                //creating a sale item to store kit component information that may be printed
                                                                //this item gets passed to ParseItemVariable to parse component fields in the same manner as other line fields
                                                                SaleLineItem componentItem = new SaleLineItem();
                                                                componentItem.Description = component["NAME"].ToString();
                                                                componentItem.Quantity = (decimal)component["COMPONENTQTY"];
                                                                componentItem.SalesOrderUnitOfMeasure = component["UNIT"].ToString();

                                                                //parsing itemInfo for kit component field information
                                                                lIneStringBuilder.Append(ParseItemVariable(itemInfo, componentItem, theTransaction));

                                                                // Closing the string with a single space command to make sure spaces are always single spaced
                                                                lIneStringBuilder.Append(esc + NORMAL_TEXT_MARKER);

                                                                // Specifing the position of the next char in the current line - bold takes twice as much space
                                                                nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                //Catching possible exception that may occur from appending to the string builder so we don't block the sale at POS
                                                                LSRetailPosis.ApplicationExceptionHandler.HandleException(ex.Message, ex);
                                                            }
                                                        }
                                                    }

                                                    lIneStringBuilder.Append(Environment.NewLine);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // options for idVariable:
                                        // Itemlines
                                        // TotalDiscount
                                        // LineDiscount
                                        charPosTable = ds.Tables["charpos"];
                                        if (charPosTable != null)
                                        {
                                            int nextCharNr = 1;
                                            foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                                            {
                                                try
                                                {
                                                    itemInfo = new FormItemInfo(row);
                                                    // Adding possible whitespace at the beginning of line
                                                    lIneStringBuilder.Append(CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

                                                    lIneStringBuilder.Append(GetFontFormatStyle(itemInfo));

                                                    // Parsing the itemInfo
                                                    lIneStringBuilder.Append(ParseItemVariable(itemInfo, saleLineItem, theTransaction));

                                                    // Closing the string with a single space command to make sure spaces are always single spaced
                                                    lIneStringBuilder.Append(esc + NORMAL_TEXT_MARKER);

                                                    // Specifing the position of the next char in the current line - bold take twice as much space
                                                    nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }
                                        }

                                        lIneStringBuilder.Append(Environment.NewLine);
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
        }

        //Creates a string to print out for each of the items in the transaction. If 
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "GrandFather PS6015")]
        private string ReadItemDataSet(DataSet ds, RetailTransaction theTransaction)
        {
            StringBuilder lIneStringBuilder = new StringBuilder(string.Empty);
            System.Collections.ArrayList itemArray = null;
            ISaleLineItem tempItem = null;
            bool found = false;

            if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.AggregateItemsForPrinting == true)
            {
                itemArray = new System.Collections.ArrayList();

                foreach (ISaleLineItem item in theTransaction.SaleItems)
                {
                    if (item.Voided)
                        continue;

                    if (!string.IsNullOrEmpty(item.ItemId))
                    {
                        foreach (ISaleLineItem itemInArray in itemArray)
                        {
                            if (itemInArray.ItemId == item.ItemId)
                            {
                                if (ShouldAggregate(itemInArray, item))
                                {
                                    itemInArray.Quantity += item.Quantity;
                                    itemInArray.GrossAmount += item.GrossAmount;
                                    itemInArray.LineDiscount += item.LineDiscount;
                                    itemInArray.TotalDiscount += item.TotalDiscount;
                                    itemInArray.PeriodicDiscount += item.PeriodicDiscount;
                                    itemInArray.NetAmount += item.NetAmount;
                                    for (int i = 0; i < item.TaxLines.Count(); i++)
                                    {
                                        itemInArray.Add(item.TaxLines.ElementAt(i));
                                    }

                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!found)
                    {
                        tempItem = Printing.InternalApplication.BusinessLogic.Utility.CreateSaleLineItem(
                            item, Printing.InternalApplication.Services.Rounding, theTransaction);
                        itemArray.Add(tempItem);
                    }

                    found = false;
                }
            }

            if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.AggregateItemsForPrinting)
            {
                //Go through the sale items and parse each line
                foreach (SaleLineItem saleLineItem in itemArray)
                {
                    lIneStringBuilder.Append(ReadItemDataSet(ds, saleLineItem, theTransaction));
                }
            }
            else
            {
                //Go through the sale items and parse each line
                if (theTransaction is IRetailTransaction)
                {
                    foreach (SaleLineItem saleLineItem in theTransaction.SaleItems)
                    {
                        lIneStringBuilder.Append(ReadItemDataSet(ds, saleLineItem, theTransaction));
                    }
                }
            }

            return lIneStringBuilder.ToString();
        }

        private string ReadItemDataSet(DataSet ds, SaleLineItem saleLineItem, RetailTransaction retailTransaction)
        {
            FormItemInfo itemInfo = null;
            StringBuilder resultText = new StringBuilder(string.Empty);

            ParseSaleItem(ds, ref itemInfo, resultText, saleLineItem, retailTransaction);

            return resultText.ToString();
        }

        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "GrandFather PS6015")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private string ReadDataset(DataSet ds, ITenderLineItem tenderItem, RetailTransaction theTransaction)
        {
            String tempString = string.Empty;
            DataTable lineTable = ds.Tables["line"];
            DataTable charPosTable = ds.Tables["charpos"];

            FormItemInfo itemInfo = null;

            if (lineTable != null)
            {
                if (charPosTable != null)
                {
                    charPosTable = ChangeDataTableColumnType(ds.Tables["charpos"], "nr");
                }
                //foreach (DataRow dr in lineTable.Rows)
                foreach (DataRow dr in lineTable.Select(string.Empty, "nr asc"))
                {
                    string idVariable = Utility.ToString(dr["ID"]);
                    string nrVariable = Utility.ToString(dr["nr"]);
                    string drLineId = string.Empty;
                    switch (idVariable)
                    {
                        case "CRLF":
                            tempString += Environment.NewLine;
                            break;
                        case "Text":
                            drLineId = Utility.ToString(dr["line_id"]);
                            if (charPosTable != null)
                            {
                                int nextCharNr = 1;


                                foreach (DataRow row in charPosTable.Select("line_id='" + drLineId + "'", "nr asc"))
                                {
                                    try
                                    {
                                        itemInfo = new FormItemInfo(row);
                                        // Adding possible whitespace at the beginning of line
                                        tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                        // Adding marker for text formatting
                                        tempString += GetFontFormatStyle(itemInfo);


                                        // Parsing the itemInfo
                                        tempString += ParseVariable(itemInfo, tenderItem, theTransaction);

                                        // Closing the string with a single space command to make sure spaces are always single spaced
                                        tempString += esc + NORMAL_TEXT_MARKER;

                                        // Specifing the position of the next char in the current line - bold take twice as much space
                                        nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }

                            tempString += Environment.NewLine;
                            break;

                        case "Taxes":
                            switch (Functions.CountryRegion)
                            {
                                case SupportedCountryRegion.IN:
                                    PopulateTaxSummaryForIndia(theTransaction);
                                    break;
                                default:
                                    break;
                            }

                            drLineId = Utility.ToString(dr["line_id"]);
                            if (charPosTable != null)
                            {
                                foreach (ITaxItem taxItem in theTransaction.TaxLines)
                                {
                                    int nextCharNr = 1;
                                    foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                                    {
                                        try
                                        {
                                            itemInfo = new FormItemInfo(row);
                                            // Adding possible whitespace at the beginning of line
                                            tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                            // Adding marker for text formatting
                                            tempString += GetFontFormatStyle(itemInfo);

                                            // Parsing the itemInfo
                                            tempString += ParseTaxVariable(itemInfo, taxItem);

                                            // Closing the string with a single space command to make sure spaces are always single spaced
                                            tempString += esc + NORMAL_TEXT_MARKER;

                                            // Specifing the position of the next char in the current line - bold take twice as much space
                                            nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                    tempString += Environment.NewLine;
                                }
                            }
                            break;

                        case "LoyaltyEarnLines":
                            charPosTable = ds.Tables["charpos"];
                            if (charPosTable != null)
                            {
                                tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.ReturnEarned);
                                tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.Earn);
                            }
                            break;

                        case "LoyaltyRedeemLines":
                            charPosTable = ds.Tables["charpos"];
                            if (charPosTable != null)
                            {
                                tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.Refund);
                                tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.Redeem);
                            }

                            break;

                        case "LoyaltyItem":
                            charPosTable = ds.Tables["charpos"];
                            if (charPosTable != null && theTransaction.LoyaltyItem != null && !string.IsNullOrEmpty(theTransaction.LoyaltyItem.LoyaltyCardNumber))
                            {
                                tempString += GetLoyaltyText(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), tenderItem);
                            }
                            break;
                        case "LoyaltyEarnText":
                            charPosTable = ds.Tables["charpos"];
                            // check if there are loyalty card and earned reward lines with redeemable points 
                            if (charPosTable != null
                                && theTransaction.LoyaltyItem != null
                                && theTransaction.LoyaltyItem.RewardPointLines != null
                                && theTransaction.LoyaltyItem.RewardPointLines.Any(l => (l.EntryType == LoyaltyRewardPointEntryType.Earn || l.EntryType == LoyaltyRewardPointEntryType.ReturnEarned) && l.RewardPointRedeemable))
                            {
                                tempString += GetLoyaltyText(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), tenderItem);
                            }
                            //changed by Yonathan 11/01/2024 -> because the point not show
                            //if (charPosTable != null)
                            //{
                            //    tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.ReturnEarned);
                            //    tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.Earn);
                            //}
                            break;
                        case "LoyaltyRedeemText":
                            charPosTable = ds.Tables["charpos"];
                            // check if there are loyalty card and redeemed reward lines
                            //changed by Yonathan 11/01/2024 -> because the point not show

                            
                            if (charPosTable != null
                                && theTransaction.LoyaltyItem != null
                                && theTransaction.LoyaltyItem.RewardPointLines != null
                                && theTransaction.LoyaltyItem.RewardPointLines.Any(l => l.EntryType == LoyaltyRewardPointEntryType.Redeem || l.EntryType == LoyaltyRewardPointEntryType.Refund))
                            {
                                tempString += GetLoyaltyText(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), tenderItem);
                            }

                            //if (charPosTable != null)
                            //{
                            //    tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.Refund);
                            //    tempString += ProcessLoyaltyLines(theTransaction, ds.Tables["charpos"], lineTable, Utility.ToString(dr["line_id"]), LoyaltyRewardPointEntryType.Redeem);
                            //}
                            break;
                        case "Tenders":
                            drLineId = Utility.ToString(dr["line_id"]);

                            if (charPosTable != null)
                            {
                                System.Collections.Generic.LinkedList<TenderLineItem> tenderLines;
                                if (theTransaction is SalesInvoiceTransaction)
                                {
                                    tenderLines = ((SalesInvoiceTransaction)theTransaction).TenderLines;
                                }
                                else if (theTransaction is SalesOrderTransaction)
                                {
                                    tenderLines = ((SalesOrderTransaction)theTransaction).TenderLines;
                                }
                                else
                                {
                                    tenderLines = theTransaction.TenderLines;
                                }

                                foreach (ITenderLineItem tenderLineItem in tenderLines)
                                {
                                    if (tenderLineItem.Voided == false)
                                    {
                                        int nextCharNr = 1;
                                        foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                                        {
                                            try
                                            {
                                                itemInfo = new FormItemInfo(row);
                                                // If tender is a Change Back tender, then a carrage return is put in front of the next line
                                                if ((tenderLineItem.Amount < 0) && (nextCharNr == 1))
                                                {
                                                    tempString += Environment.NewLine;
                                                }

                                                // Adding possible whitespace at the beginning of line
                                                tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                                // Adding marker for text formatting
                                                tempString += GetFontFormatStyle(itemInfo);

                                                // Parsing the itemInfo
                                                tempString += ParseTenderLineVariable(itemInfo, tenderLineItem, theTransaction);

                                                // Closing the string with a single space command to make sure spaces are always single spaced
                                                tempString += esc + NORMAL_TEXT_MARKER;

                                                // Specifing the position of the next char in the current line - bold take twice as much space
                                                nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }

                                        tempString += Environment.NewLine;
                                    }
                                }
                            }
                            break;

                        case "Tender":
                            drLineId = Utility.ToString(dr["line_id"]);
                            if (charPosTable != null)
                            {
                                int nextCharNr = 1;
                                foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                                {
                                    try
                                    {
                                        itemInfo = new FormItemInfo(row);
                                        // If tender is a Change Back tender, then a carrage return is put in front of the next line
                                        if ((tenderItem.Amount < 0) && (nextCharNr == 1))
                                        {
                                            tempString += Environment.NewLine;
                                        }

                                        // Adding possible whitespace at the beginning of line
                                        tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                        // Adding marker for text formatting
                                        tempString += GetFontFormatStyle(itemInfo);

                                        // Parsing the itemInfo
                                        tempString += ParseTenderLineVariable(itemInfo, tenderItem, theTransaction);

                                        // Closing the string with a single space command to make sure spaces are always single spaced
                                        tempString += esc + NORMAL_TEXT_MARKER;

                                        // Specifing the position of the next char in the current line - bold take twice as much space
                                        nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                tempString += Environment.NewLine;
                            }
                            break;

                        case "TenderRounding":
                            if (theTransaction.TransSalePmtDiff == 0)
                            {
                                break;
                            }

                            drLineId = Utility.ToString(dr["line_id"]);
                            if (charPosTable != null)
                            {
                                int nextCharNr = 1;

                                foreach (DataRow row in charPosTable.Select("line_id='" + drLineId + "'", "nr asc"))
                                {
                                    try
                                    {
                                        itemInfo = new FormItemInfo(row);
                                        // Adding possible whitespace at the beginning of line
                                        tempString += CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' ');

                                        // Adding marker for text formatting
                                        tempString += GetFontFormatStyle(itemInfo);

                                        // Parsing the itemInfo
                                        tempString += ParseVariable(itemInfo, tenderItem, theTransaction);

                                        // Closing the string with a single space command to make sure spaces are always single spaced
                                        tempString += esc + NORMAL_TEXT_MARKER;

                                        // Specifing the position of the next char in the current line - bold take twice as much space
                                        nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }

                            tempString += Environment.NewLine;
                            break;
                    }
                }
            }

            return tempString.ToString();

        }

        /// <summary>
        /// Gets loyalty text value based on transaction.
        /// </summary>
        /// <param name="theTransaction">Retail transaction.</param>
        /// <param name="charPosTable">Table with receipt layout data.</param>
        /// <param name="lineTable">Current processing receipt line data.</param>
        /// <param name="drLineId">Current processing receipt line id.</param>
        /// <param name="tenderItem">Tender line.</param>
        /// <returns>Text value.</returns>
        private string GetLoyaltyText(RetailTransaction theTransaction, DataTable charPosTable, DataTable lineTable, string drLineId, ITenderLineItem tenderItem)
        {
            StringBuilder tempString = new StringBuilder();

            int nextCharNr = 1;
            foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
            {
                FormItemInfo itemInfo = new FormItemInfo(row);
                // Adding possible whitespace at the beginning of line
                tempString.Append(FormModulation.CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

                // Adding marker for text formatting
                tempString.Append(GetFontFormatStyle(itemInfo));

                // Parsing the itemInfo
                tempString.Append(ParseVariable(itemInfo, tenderItem, theTransaction));

                // Closing the string with a single space command to make sure spaces are always single spaced
                tempString.Append(esc + NORMAL_TEXT_MARKER);

                // Specifing the position of the next char in the current line - bold take twice as much space
                nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
            }

            tempString.Append(Environment.NewLine);

            return tempString.ToString();
        }

        private string ProcessLoyaltyLines(RetailTransaction theTransaction, DataTable charPosTable, DataTable lineTable, string drLineId, LoyaltyRewardPointEntryType entryType)
        {
            StringBuilder tempString = new StringBuilder();

            // check if loyalty card and reward lines are present
            if (theTransaction.LoyaltyItem != null
                && theTransaction.LoyaltyItem.RewardPointLines != null
                && theTransaction.LoyaltyItem.RewardPointLines.Any(l => l.EntryType == entryType && l.RewardPointRedeemable))
            {
                IEnumerable<LoyaltyRewardPointSummary> rewardLines =
                    from l in theTransaction.LoyaltyItem.RewardPointLines
                    where l.EntryType == entryType && l.RewardPointRedeemable
                    group l by new
                    {
                        l.RewardPointId,
                        l.RewardPointType,
                        l.RewardPointCurrency
                    } into g
                    select new LoyaltyRewardPointSummary
                    {
                        RewardPointId = g.Key.RewardPointId,
                        RewardPointType = g.Key.RewardPointType,
                        RewardPointCurrency = g.Key.RewardPointCurrency,
                        RewardPointAmountQuantity = g.Sum(a => a.RewardPointAmountQuantity),
                    };

                // iterate over loyalty reward points
                foreach (LoyaltyRewardPointSummary rewardPoint in rewardLines)
                {
                    int nextCharNr = 1;
                    foreach (DataRow row in charPosTable.Select(lineTable.TableName + "_Id='" + drLineId + "'", "nr asc"))
                    {
                        FormItemInfo itemInfo = new FormItemInfo(row);
                        // Adding possible whitespace at the beginning of line
                        tempString.Append(FormModulation.CreateWhitespace(itemInfo.CharIndex - nextCharNr, ' '));

                        // Adding marker for text formatting
                        tempString.Append(GetFontFormatStyle(itemInfo));

                        // Parsing the itemInfo
                        tempString.Append(FormModulation.ParseLoyaltyRewardPointSummary(itemInfo, entryType, rewardPoint));

                        // Closing the string with a single space command to make sure spaces are always single spaced
                        tempString.Append(esc + NORMAL_TEXT_MARKER);

                        // Specifing the position of the next char in the current line - bold take twice as much space
                        nextCharNr = itemInfo.CharIndex + itemInfo.Length * itemInfo.SizeFactor;
                    }

                    tempString.Append(Environment.NewLine);
                }
            }

            return tempString.ToString();
        }

        private static string ParseLoyaltyRewardPointSummary(FormItemInfo itemInfo, LoyaltyRewardPointEntryType entryType, LoyaltyRewardPointSummary rewardPoint)
        {
            string variableString = FormModulation.GetInfoFromLoyaltyRewardPointSummary(itemInfo, entryType, rewardPoint);

            return GetAlignmentSettings(variableString, itemInfo);
        }

        /// <summary>
        /// Method converts the given DataTable String column type to Integer column type without data loss
        /// </summary>
        /// <param name="charPosTable">Data Table.</param>
        /// <param name="columnName">Column to be convert to integer type.</param>
        /// <returns>Returns Data Table with converted integer type column.</returns>
        private static DataTable ChangeDataTableColumnType(DataTable charPosTable, string columnName)
        {
            //Convert the string field to int, only if it string.
            if (charPosTable.Columns[columnName].DataType != typeof(int))
            {
                //A new column : tempColumn is created to hold the char positions and added to the data table charPosTable. 
                var newColumn = new DataColumn("tempColumn", typeof(int));
                charPosTable.Columns.Add(newColumn);

                //Dump the data from columnName to tempColumn.
                foreach (DataRow row in charPosTable.Rows)
                {
                    row[newColumn] = Convert.ChangeType(row[columnName], typeof(int));
                }

                //Remove columnName from the data table
                charPosTable.Columns.Remove(columnName);
                //rename tempColumn to newColumn.
                newColumn.ColumnName = columnName;
            }

            //Change the data view of the new charPosTable so that the column is sorted to ascending order.
            var dataView = charPosTable.DefaultView;
            dataView.Sort = columnName + " ASC";

            return dataView.ToTable();
        }

        private static void PopulateTaxSummaryForIndia(RetailTransaction theTransaction)
        {
            if (ApplicationSettings.Terminal.IndiaTaxDetailsType == Terminal.IndiaReceiptTaxDetailsType.PerTaxComponent)
            {
                IList<TaxItemIndia> indiaTaxItems = new List<TaxItemIndia>();
                foreach (ISaleLineItem item in theTransaction.SaleItems)
                {
                    foreach (TaxItem taxItem in item.TaxLines)
                    {
                        indiaTaxItems.Add(taxItem as TaxItemIndia);
                    }
                }

                if (ApplicationSettings.Terminal.ShowIndiaTaxOnTax)
                {
                    theTransaction.TaxLines = BuildIndiaTaxSummaryPerComponent1(indiaTaxItems);
                }
                else
                {
                    theTransaction.TaxLines = BuildIndiaTaxSummaryPerComponent2(indiaTaxItems);
                }
            }
            else if (ApplicationSettings.Terminal.IndiaTaxDetailsType == Terminal.IndiaReceiptTaxDetailsType.PerLine)
            {
                theTransaction.TaxLines = BuildIndiaTaxSummaryPerLine(theTransaction);
            }
        }

        /// <summary>
        /// Build tax summary lines of the India receipt, with tax amounts be aggregated by sale line items.
        /// </summary>
        /// <param name="theTransaction">The retail transaction.</param>
        /// <returns>The tax summary lines of the India receipt.</returns>
        /// <remarks>
        /// In this case, the settings of "RetailStoreTable > Misc > Receipts" is as follows,
        /// 1) The "Tax details" option is set as "Per line"
        /// 2) The "Show tax on tax" option is set as "N/A", as it is disabled in this case
        /// 
        /// For example, the retail transaction has four sale line items, as follows,
        /// Item ID | Price | Tax code | Formula       | Tax basis | Tax rate | Tax amount
        /// 0001    | 100   | SERV5    | Line amount   | 100.00    |  5%      |  5.00
        ///         |       | E-CSS5   | Excl.+[SERV5] |   5.00    |  5%      |  0.25
        /// 0002    | 100   | VAT10    | Line amount   | 100.00    | 10%      | 10.00
        ///         |       | Surchg2  | Excl.+[VAT10] |  10.00    |  2%      |  0.20
        /// 0003    | 100   | SERV4    | Line amount   | 100.00    |  4%      |  4.00
        ///         |       | E-CSS5   | Excl.+[SERV4] |   4.00    |  5%      |  0.20
        /// 0004    | 100   | VAT12    | Line amount   | 100.00    | 12%      | 12.00
        ///         |       | Surchg2  | Excl.+[VAT12] |  12.00    |  2%      |  0.24
        /// 
        /// And the tax summary lines will be as follows,
        /// Tax code | Tax basis | Tax rate | Tax amount
        /// AA       | 100.00    |  5.25%   |  5.25
        /// AB       | 100.00    | 10.20%   | 10.20
        /// AC       | 100.00    |  4.20%   |  4.20
        /// AD       | 100.00    | 12.24%   | 12.24
        /// 
        /// Tax codes are automatically named from "AA" to "AZ", ...
        /// </remarks>
        private static LinkedList<TaxItem> BuildIndiaTaxSummaryPerLine(RetailTransaction theTransaction)
        {
            LinkedList<TaxItem> lines = new LinkedList<TaxItem>();

            char code1 = 'A', code2 = 'A';
            foreach (ISaleLineItem saleItem in theTransaction.SaleItems)
            {
                TaxItemIndia t = new TaxItemIndia();
                t.TaxCode = "" + code1 + code2;
                t.TaxBasis = saleItem.TaxLines.First(x => !(x as TaxItemIndia).IsTaxOnTax).TaxBasis;
                t.Amount = saleItem.TaxLines.Sum(x => x.Amount);
                t.Percentage = t.TaxBasis != decimal.Zero ? 100 * t.Amount / t.TaxBasis : decimal.Zero;

                lines.AddLast(t);

                // Generate tax code of the next line
                code2++;
                if (code2 > 'Z')
                {
                    code2 = 'A';
                    code1++;

                    if (code1 > 'Z')
                    {
                        code1 = 'A';
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// Build tax summary line of the India receipt, with tax amounts be aggregated by "main" tax codes (which
        /// are not India tax on tax codes).
        /// </summary>
        /// <param name="indiaTaxItems">All tax items of the India retail transaction.</param>
        /// <returns>The tax summary lines of the India receipt.</returns>
        /// <remarks>
        /// In this case, the settings of "RetailStoreTable > Misc > Receipts" is as follows,
        /// 1) The "Tax details" option is set as "Per tax component"
        /// 2) The "Show tax on tax" option is turned OFF
        /// 
        /// For example, the retail transaction has four sale line items, as follows,
        /// Item ID | Price | Tax code | Formula       | Tax basis | Tax rate | Tax amount
        /// 0001    | 100   | SERV5    | Line amount   | 100.00    |  5%      |  5.00
        ///         |       | E-CSS5   | Excl.+[SERV5] |   5.00    |  5%      |  0.25
        /// 0002    | 100   | VAT10    | Line amount   | 100.00    | 10%      | 10.00
        ///         |       | Surchg2  | Excl.+[VAT10] |  10.00    |  2%      |  0.20
        /// 0003    | 100   | SERV4    | Line amount   | 100.00    |  4%      |  4.00
        ///         |       | E-CSS5   | Excl.+[SERV4] |   4.00    |  5%      |  0.20
        /// 0004    | 100   | VAT12    | Line amount   | 100.00    | 12%      | 12.00
        ///         |       | Surchg2  | Excl.+[VAT12] |  12.00    |  2%      |  0.24
        /// 
        /// And the tax summary lines will be as follows,
        /// Tax code | Tax basis | Tax rate | Tax amount
        /// SERV5    | 100.00    |  5.25%   |  5.25
        /// SERV4    | 100.00    |  4.20%   |  4.20
        /// VAT10    | 100.00    | 10.20%   | 10.20
        /// VAT12    | 100.00    | 12.24%   | 12.24
        /// </remarks>
        private static LinkedList<TaxItem> BuildIndiaTaxSummaryPerComponent2(IList<TaxItemIndia> indiaTaxItems)
        {
            LinkedList<TaxItem> lines = new LinkedList<TaxItem>();

            var groups = indiaTaxItems.GroupBy(x =>
            {
                string taxCode = !x.IsTaxOnTax ? x.TaxCode : x.TaxCodesInFormula.First();

                return new { x.TaxGroup, taxCode };
            });
            foreach (var group in groups)
            {
                TaxItemIndia t = new TaxItemIndia();
                t.TaxGroup = group.Key.TaxGroup;
                t.TaxCode = group.Key.taxCode;
                t.TaxBasis = group.First(x => !x.IsTaxOnTax).TaxBasis;
                t.Amount = group.Sum(x => x.Amount);
                t.Percentage = t.TaxBasis != decimal.Zero ? 100 * t.Amount / t.TaxBasis : decimal.Zero;

                lines.AddLast(t);
            }

            // Order by tax component
            lines = new LinkedList<TaxItem>(lines.OrderBy(x => (x as TaxItemIndia).TaxComponent));

            return lines;
        }

        /// <summary>
        /// Build tax summary line of the India receipt, with tax amounts be aggregated by tax codes.
        /// </summary>
        /// <param name="indiaTaxItems">All tax items of the India retail transaction.</param>
        /// <returns>The tax summary lines of the India receipt.</returns>
        /// <remarks>
        /// In this case, the settings of "RetailStoreTable > Misc > Receipts" is as follows,
        /// 1) The "Tax details" option is set as "Per tax component"
        /// 2) The "Show tax on tax" option is turned ON
        /// 
        /// For example, the retail transaction has four sale line items, as follows,
        /// Item ID | Price | Tax code | Formula       | Tax basis | Tax rate | Tax amount
        /// 0001    | 100   | SERV5    | Line amount   | 100.00    |  5%      |  5.00
        ///         |       | E-CSS5   | Excl.+[SERV5] |   5.00    |  5%      |  0.25
        /// 0002    | 100   | VAT10    | Line amount   | 100.00    | 10%      | 10.00
        ///         |       | Surchg2  | Excl.+[VAT10] |  10.00    |  2%      |  0.20
        /// 0003    | 100   | SERV4    | Line amount   | 100.00    |  4%      |  4.00
        ///         |       | E-CSS5   | Excl.+[SERV4] |   4.00    |  5%      |  0.20
        /// 0004    | 100   | VAT12    | Line amount   | 100.00    | 12%      | 12.00
        ///         |       | Surchg2  | Excl.+[VAT12] |  12.00    |  2%      |  0.24
        /// 
        /// And the tax summary lines will be as follows,
        /// Tax component | Tax code | Tax basis | Tax rate | Tax amount
        /// Service       | SERV5    | 100.00    |  5%      |  5.00
        /// Service       | SERV4    | 100.00    |  4%      |  4.00
        /// E-CSS         | E-CSS5   |   9.00    |  5%      |  0.45
        /// VAT           | VAT10    | 100.00    | 10%      | 10.00
        /// VAT           | VAT12    | 100.00    | 12%      | 12.00
        /// Surcharge     | Surchg2  |  22.00    |  2%      |  0.44
        /// </remarks>
        private static LinkedList<TaxItem> BuildIndiaTaxSummaryPerComponent1(IList<TaxItemIndia> indiaTaxItems)
        {
            LinkedList<TaxItem> lines = new LinkedList<TaxItem>();

            var groups = indiaTaxItems.GroupBy(x => new { x.TaxComponent, x.TaxCode });
            foreach (var group in groups)
            {
                TaxItemIndia t = new TaxItemIndia();
                t.TaxComponent = group.Key.TaxComponent;
                t.TaxCode = group.Key.TaxCode;
                t.Amount = group.Sum(x => x.Amount);
                t.Percentage = group.First().Percentage;
                t.TaxBasis = group.Sum(x => x.TaxBasis);
                t.TaxGroup = group.First().TaxGroup;

                lines.AddLast(t);
            }

            // Order by tax component
            lines = new LinkedList<TaxItem>(lines.OrderBy(x => (x as TaxItemIndia).TaxComponent));

            return lines;
        }

        private static string Wrap(string text, FormItemInfo itemInfo)
        {
            // Define our regex
            string pattern = @"(?<Line>.{1," + itemInfo.Length + @"})(?:\W)";

            // Split the string based on the pattern
            string[] lines = Regex.Split(text, pattern, RegexOptions.IgnoreCase
                                                        | RegexOptions.IgnorePatternWhitespace
                                                        | RegexOptions.Multiline
                                                        | RegexOptions.ExplicitCapture
                                                        | RegexOptions.CultureInvariant
                                                        | RegexOptions.Compiled);

            // Empty string to return value
            string returnValue = string.Empty;

            // Concacitante all lines adding a hard return on each line
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    returnValue += GetAlignmentSettings(line.Trim(), itemInfo) + Environment.NewLine;
                }
            }

            // No NewLine at the end of the string.
            return returnValue.TrimEnd(Environment.NewLine.ToCharArray());
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Grandfather")]
        private static DataSet ConvertToDataSet(string templateXml, DataRow dataRow)
        {
            DataSet template = null;

            if (templateXml.Length > 0)
            {
                template = new DataSet("New DataSet");
                byte[] buffer = null;
                int discarded;
                buffer = LSRetailPosis.DataAccess.DataUtil.HexEncoding.GetBytes(templateXml, out discarded);

                if (buffer != null)
                {
                    using (System.IO.MemoryStream myStream = new System.IO.MemoryStream())
                    {
                        myStream.Write(buffer, 0, buffer.Length);
                        myStream.Position = 0;

                        template.ReadXml(myStream);
                    }
                }

                // Adding detail table to the dataset
                DataTable formDetails = new DataTable();
                formDetails.TableName = "FORMDETAILS";

                // Adding columns to items data
                DataColumn col = new DataColumn("ID", Type.GetType("System.String"));
                formDetails.Columns.Add(col);
                col = new DataColumn("TITLE", Type.GetType("System.String"));
                formDetails.Columns.Add(col);
                col = new DataColumn("DESCRIPTION", Type.GetType("System.String"));
                formDetails.Columns.Add(col);
                col = new DataColumn("UPPERCASE", Type.GetType("System.Boolean"));
                formDetails.Columns.Add(col);

                Object[] row = new Object[4];

                row[0] = dataRow["FORMLAYOUTID"];
                row[1] = dataRow["TITLE"].ToString();
                row[2] = dataRow["DESCRIPTION"].ToString();
                row[3] = dataRow["UPPERCASE"].ToString() == "1";

                formDetails.Rows.Add(row);
                template.Tables.Add(formDetails);
            }

            return template;
        }

        /// <summary>
        /// Returns transformed card tender as string.
        /// </summary>
        /// <param name="formNumber"></param>
        /// <param name="EFTInfo"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public string GetTransformedCardTender(FormInfo formInfo, IEFTInfo EFTInfo, RetailTransaction theTransaction)
        {
            if (formInfo == null)
            {
                throw new ArgumentNullException("formInfo");
            }

            StringBuilder returnString = new StringBuilder();
            DataSet ds = null;

            if (LSRetailPosis.Settings.ApplicationSettings.Terminal.TrainingMode == true)
            {
                returnString.Append(ApplicationLocalizer.Language.Translate(true, null, 13042)
                    + Environment.NewLine + "***********************************" + Environment.NewLine);
            }

            // Getting a dataset containing the headerpart of the current form
            ds = formInfo.HeaderTemplate;
            returnString.Append(ReadCardTenderDataSet(ds, EFTInfo, theTransaction));

            // Getting a dataset containing the footerpart of the current form
            ds = formInfo.FooterTemplate;
            returnString.Append(ReadCardTenderDataSet(ds, EFTInfo, theTransaction));

            if (LSRetailPosis.Settings.ApplicationSettings.Terminal.TrainingMode == true)
            {
                returnString.Append(Environment.NewLine + "***********************************"
                    + Environment.NewLine + ApplicationLocalizer.Language.Translate(true, null, 13042));
            }

            // further modification of the string
            DataTable formDetails = ds.Tables["FORMDETAILS"];
            if (formDetails != null)
            {
                DataRow detailRow = formDetails.Rows[0];
                {
                    if (Convert.ToBoolean(detailRow["UPPERCASE"]) == true)
                    {
                        string tempstring = returnString.ToString();
                        return tempstring.ToUpper();
                    }
                }
            }

            return returnString.ToString();
        }

        /// <summary>
        /// Returns transformed tender data as string.
        /// </summary>
        /// <param name="formNumber"></param>
        /// <param name="tenderLineItem"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Changing public API will break backwards compatibility.")]
        public string GetTransformedTender(FormInfo formInfo, TenderLineItem tenderLineItem, RetailTransaction theTransaction)
        {
            if (formInfo == null)
            {
                throw new ArgumentNullException("formInfo");
            }

            StringBuilder returnString = new StringBuilder();
            DataSet ds = null;

            if (LSRetailPosis.Settings.ApplicationSettings.Terminal.TrainingMode == true)
            {
                returnString.Append(ApplicationLocalizer.Language.Translate(true, null, 13042)
                    + Environment.NewLine + "***********************************" + Environment.NewLine);
            }

            // Getting a dataset containing the headerpart of the current form
            ds = formInfo.HeaderTemplate;
            returnString.Append(ReadTenderDataSet(ds, tenderLineItem, theTransaction));

            // Getting a dataset containing the footerpart of the current form
            ds = formInfo.FooterTemplate;
            returnString.Append(ReadTenderDataSet(ds, tenderLineItem, theTransaction));

            if (LSRetailPosis.Settings.ApplicationSettings.Terminal.TrainingMode == true)
            {
                returnString.Append(Environment.NewLine + "***********************************"
                    + Environment.NewLine + ApplicationLocalizer.Language.Translate(true, null, 13042));
            }

            // further modification of the string
            DataTable formDetails = ds.Tables["FORMDETAILS"];
            if (formDetails != null)
            {
                DataRow detailRow = formDetails.Rows[0];
                {
                    if (Convert.ToBoolean(detailRow["UPPERCASE"]) == true)
                    {
                        string tempstring = returnString.ToString();
                        return tempstring.ToUpper();
                    }
                }
            }

            return returnString.ToString();
        }

        /// <summary>
        /// Gets transformed transaction details as per given form number.
        /// </summary>
        /// <param name="formInfo"></param>
        /// <param name="trans"></param>
        public void GetTransformedTransaction(FormInfo formInfo, RetailTransaction theTransaction)
        {
            if (formInfo == null)
            {
                throw new ArgumentNullException("formInfo");
            }

            try
            {
                DataSet ds = null;

                if (LSRetailPosis.Settings.ApplicationSettings.Terminal.TrainingMode == true)
                {
                    var sbHeaderText = new StringBuilder();
                    if (Functions.CountryRegion == SupportedCountryRegion.SE)
                    {
                        sbHeaderText.Append(esc + DOUBLESIZE_TEXT_MARKER);
                    }

                    sbHeaderText.Append((ApplicationLocalizer.Language.Translate(true, null, 13042)));

                    if (Functions.CountryRegion == SupportedCountryRegion.SE)
                    {
                        sbHeaderText.Append(esc + NORMAL_TEXT_MARKER);
                    }

                    sbHeaderText.Append(Environment.NewLine + "***********************************" + Environment.NewLine);
                    formInfo.Header = sbHeaderText.ToString();
                }


                if (formInfo.Reprint)
                {
                    copyText = ApplicationLocalizer.Language.Translate(true, null, 13039);
                }
                else
                {
                    copyText = string.Empty;
                }

                // Getting a dataset containing the headerpart of the current form
                ds = formInfo.HeaderTemplate;
                formInfo.Header = ReadDataset(ds, null, theTransaction);
                formInfo.Header = RemoveEmptyLines(formInfo.Header); //add by yonathan 18102024 #THERMAL
                formInfo.HeaderLines = ds.Tables[0].Rows.Count;

                

                // Getting a dataset containing the linepart of the current form
                ds = formInfo.DetailsTemplate;
                formInfo.Details = ReadItemDataSet(ds, theTransaction);
                formInfo.Details = RemoveEmptyLines(formInfo.Details); //add by yonathan 18102024 #THERMAL
                formInfo.DetailLines = ds.Tables[0].Rows.Count;

                // Getting a dataset containing the footerpart of the current form
                //get DPP2025 printing status 23012025 getDPP2025Status
                //ReadOnlyCollection<object> containerArray = Printing.InternalApplication.TransactionServices.InvokeExtension("getDPP2025Status", ApplicationSettings.Database.DATAAREAID);
                statusDPP2025Print = "True";//containerArray[3].ToString(); 
                ds = formInfo.FooterTemplate;
                formInfo.Footer = ReadDataset(ds, null, theTransaction);
                formInfo.Footer = RemoveEmptyLines(formInfo.Footer); //add by yonathan 18102024 #THERMAL
                formInfo.FooterLines = ds.Tables[0].Rows.Count;




                if (LSRetailPosis.Settings.ApplicationSettings.Terminal.TrainingMode == true)
                {
                    StringBuilder sbFooterText = new StringBuilder(Environment.NewLine + "***********************************"
                        + Environment.NewLine);
                    if (Functions.CountryRegion == SupportedCountryRegion.SE)
                    {
                        sbFooterText.Append(esc + DOUBLESIZE_TEXT_MARKER);
                    }

                    sbFooterText.Append(ApplicationLocalizer.Language.Translate(true, null, 13042));

                    if (Functions.CountryRegion == SupportedCountryRegion.SE)
                    {
                        sbFooterText.Append(esc + NORMAL_TEXT_MARKER);
                    }

                    formInfo.Footer = sbFooterText.ToString();
                }

                // further modification of the string
                DataTable formDetails = ds.Tables["FORMDETAILS"];
                if (formDetails != null)
                {
                    DataRow detailRow = formDetails.Rows[0];
                    {
                        if (Convert.ToBoolean(detailRow["UPPERCASE"]) == true)
                        {
                            formInfo.Header = formInfo.Header.ToUpper();
                            formInfo.Details = formInfo.Details.ToUpper();
                            formInfo.Footer = formInfo.Footer.ToUpper();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NetTracer.Warning("Printing [FormModulation] - Exception when trying to get transformed transcation");

                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }


        //test for removing white space -- yonathan 18102024 #THERMAL
        public static string RemoveEmptyLines(string receipt)
        {
            // Split the receipt into lines
            var lines = receipt.Split(new[] { '\n' }, StringSplitOptions.None);
            var formattedReceipt = new StringBuilder();

            foreach (var line in lines)
            {
                // Initialize variables to track whether the line is considered empty
                bool isEmptyLine = true;

                // Split the line by the control characters |1C, |2C, and |4C
                string[] parts = line.Split(new[] { "|1C", "|2C", "|4C" }, StringSplitOptions.None);
                // Iterate through parts to check for meaningful content
                foreach (var part in parts)
                {
                    // Trim whitespace from each part
                    string content = part.Trim();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        isEmptyLine = false; // Found meaningful content
                        break; // No need to check further
                    }
                }

                // If the line is not empty, format and append it to the result
                if (!isEmptyLine)
                {
                    // Append the original line to retain formatting, and then add a new line
                    formattedReceipt.AppendLine(line.TrimEnd());
                }
            }

            // Return the final formatted receipt string
            return formattedReceipt.ToString();
        }
        //end

        /// <summary>
        /// Returns transformed tender data as string.
        /// </summary>
        /// <param name="formNumber"></param>
        /// <param name="tenderLineItem"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public string GetTransformedSaleItem(FormInfo formInfo, ISaleLineItem saleItem, IPosTransaction posTransaction)
        {
            if (formInfo == null)
            {
                throw new ArgumentNullException("formInfo");
            }

            var retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                throw new ArgumentException("posTransaction");
            }

            var saleLineItem = saleItem as SaleLineItem;

            if (retailTransaction == null)
            {
                throw new ArgumentException("iSaleItem");
            }

            StringBuilder returnString = new StringBuilder();
            DataSet ds = null;

            // Getting a dataset containing the headerpart of the current form
            ds = formInfo.HeaderTemplate;
            if (ds != null)
            {
                formInfo.HeaderLines = ds.Tables[0].Rows.Count;
                formInfo.Header = ReadDataset(ds, null, retailTransaction);
                returnString.Append(formInfo.Header);
            }

            // Getting a dataset containing the linepart of the current form
            ds = formInfo.DetailsTemplate;
            if (ds != null)
            {
                formInfo.DetailLines = ds.Tables[0].Rows.Count;
                formInfo.Details = ReadItemDataSet(ds, saleLineItem, retailTransaction);
                returnString.Append(formInfo.Details);
            }

            // Getting a dataset containing the footerpart of the current form
            ds = formInfo.FooterTemplate;
            if (ds != null)
            {
                formInfo.FooterLines = ds.Tables[0].Rows.Count;
                formInfo.Footer = ReadDataset(ds, null, retailTransaction);
                returnString.Append(formInfo.Footer);

                // further modification of the string
                DataTable formDetails = ds.Tables["FORMDETAILS"];
                if (formDetails != null)
                {
                    DataRow detailRow = formDetails.Rows[0];
                    {
                        if (Convert.ToBoolean(detailRow["UPPERCASE"]) == true)
                        {
                            formInfo.Header = formInfo.Header.ToUpper();
                            formInfo.Details = formInfo.Details.ToUpper();
                            formInfo.Footer = formInfo.Footer.ToUpper();
                            string tempstring = returnString.ToString();
                            return tempstring.ToUpper();
                        }
                    }
                }
            }

            return returnString.ToString();
        }

        /// <summary>
        /// Gets font style marker.
        /// </summary>
        /// <param name="itemInfo">Information about an item</param>
        /// <returns>Font style marker</returns>
        private string GetFontFormatStyle(FormItemInfo itemInfo)
        {
            // Calculation of the marker value:
            // Normal text  = |1C   Double size text      = |3C
            // Bold text    = |2C   Double size bold text = |4C
            int markerValue = 1 + (itemInfo.FontStyle == System.Drawing.FontStyle.Bold ? 1 : 0) + (itemInfo.FontSize == FormFontSize.Large ? 2 : 0);
            return string.Format("{0}|{1}C", esc, markerValue); ;
        }

        /// <summary>
        /// Gets form information as per given form id.
        /// </summary>
        /// <param name="formId">The form id.</param>
        /// <param name="copyReceipt">if set to <c>true</c> [copy receipt].</param>
        /// <param name="receiptProfileId">The receipt profile id.</param>
        /// <returns></returns>
        public FormInfo GetInfoForForm(FormType formId, bool copyReceipt, string receiptProfileId)
        {
            FormInfo formInfo = new FormInfo();
            string layoutId = receiptData.GetFormLayoutId(formId, receiptProfileId);

            formInfo.Reprint = copyReceipt;
            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM dbo.RETAILFORMLAYOUT WHERE FORMLAYOUTID = @FORMID";
                    command.Parameters.Add("@FORMID", SqlDbType.NVarChar).Value = layoutId;

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(command))
                    {
                        using (System.Data.DataTable table = new System.Data.DataTable())
                        {
                            adapter.Fill(table);
                            if (table.Rows.Count > 0)
                            {
                                if (table.Rows[0]["PRINTASSLIP"] != DBNull.Value)
                                {
                                    formInfo.PrintAsSlip = (1 == Convert.ToInt32(table.Rows[0]["PRINTASSLIP"].ToString()));
                                }
                                //int isSlip = Convert.ToInt32(table.Rows[0]["PrintAsSlip"].ToString());
                                if (table.Rows[0]["PRINTBEHAVIOUR"] != DBNull.Value)
                                {
                                    formInfo.PrintBehaviour = Convert.ToInt32(table.Rows[0]["PRINTBEHAVIOUR"].ToString());
                                }

                                if (table.Rows[0]["HEADERXML"] != DBNull.Value)
                                {
                                    formInfo.HeaderTemplate = ConvertToDataSet(table.Rows[0]["HEADERXML"].ToString(), table.Rows[0]);
                                }

                                if (table.Rows[0]["LINESXML"] != DBNull.Value)
                                {
                                    formInfo.DetailsTemplate = ConvertToDataSet(table.Rows[0]["LINESXML"].ToString(), table.Rows[0]);
                                }

                                if (table.Rows[0]["FOOTERXML"] != DBNull.Value)
                                {
                                    formInfo.FooterTemplate = ConvertToDataSet(table.Rows[0]["FOOTERXML"].ToString(), table.Rows[0]);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return formInfo;
        }

        private static bool IsOposPrinterActive(FormType formType, bool copyReceipt)
        {
            bool retVal = false;
            var activePrinter = PrintingActions.GetActivePrinters(new FormModulation(ApplicationSettings.Database.LocalConnection), formType, copyReceipt);
            retVal = activePrinter.Count(l => l.Type == DeviceTypes.OPOS) == 0 ? true : false;
            return retVal;
        }

        /// <summary>
        /// Checks if an item can be aggregated.
        /// </summary>
        /// <param name="itemInArray">Aggregated item.</param>
        /// <param name="itemToAggregate">Item to be aggregated.</param>
        /// <returns>bool</returns>
        private static bool ShouldAggregate(ISaleLineItem itemInArray, ISaleLineItem itemToAggregate)
        {
            Dimensions itemInArrayDimension = ((LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem)(itemInArray)).Dimension;
            Dimensions itemToAggregateDimension = ((LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem)(itemToAggregate)).Dimension;

            return ((itemInArray.ItemId == itemToAggregate.ItemId)
                    && (itemInArray.LineDiscount == itemToAggregate.LineDiscount)
                    && (itemInArray.LinePctDiscount == itemToAggregate.LinePctDiscount)
                    && (itemInArray.TotalDiscount == itemToAggregate.TotalDiscount)
                    && (itemInArray.TotalPctDiscount == itemToAggregate.TotalPctDiscount)
                    && ((itemInArray.Quantity > 0 && itemToAggregate.Quantity > 0) || (itemInArray.Quantity < 0 && itemToAggregate.Quantity < 0))
                    && (itemInArray.PeriodicPctDiscount == itemToAggregate.PeriodicPctDiscount)
                    && (itemInArray.Price == itemToAggregate.Price)
                    && (itemInArrayDimension != null && itemInArrayDimension.IsEquivalent(itemToAggregateDimension))
                    && (itemInArray.BatchId == itemToAggregate.BatchId)
                    && (itemInArray.SerialId == itemToAggregate.SerialId)
                    && (itemInArray.SalesOrderUnitOfMeasure == itemToAggregate.SalesOrderUnitOfMeasure)
                    && (itemInArray.Comment == itemToAggregate.Comment));
        }
        //Add by Erwin
        private void getPurchaser(RetailTransaction theTransaction)
        {
            SqlConnection con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            int flag = 0;

            try
            {

                /*string queryString = @" SELECT PurchaserName, NAMEORDER FROM [CPOrderTable]
                                        WHERE TRANSACTIONID =  @TRANSACTIONID  ";*/

                string queryString = @"SELECT PurchaserName, PurchaserPhone 
                                        FROM AX.CPNEWPAYMENTPOS
                                        WHERE TransactionID = @TRANSACTIONID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            purchaserName = reader.GetString(0);
                            purchaserPhone = reader.GetString(1);

                            flag = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            if (flag == 0)
            {
                con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

                try
                {

                    /*string queryString = @" SELECT PurchaserName, NAMEORDER FROM [CPOrderTable]
                                            WHERE TRANSACTIONID =  @TRANSACTIONID  ";*/

                    string queryString = @"SELECT PurchaserName, PurchaserPhone 
                                        FROM AX.CPNEWPAYMENTPOSTMP
                                        WHERE TransactionID = @TRANSACTIONID";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                purchaserName = reader.GetString(0);
                                purchaserPhone = reader.GetString(1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    throw;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
        }

        #region CPECRBCA
        /*private void getDibebaskan(RetailTransaction theTransaction)
        {
            SqlConnection con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"select 
	                                        COALESCE(SUM(NETAMOUNT), 0) AS totalDibebaskan
                                        from ax.RETAILTRANSACTIONSALESTRANS
                                        WHERE 
	                                        TAXITEMGROUP = 'dibebaskan'
	                                        AND TRANSACTIONID = @TRANSACTIONID
	                                        AND DATAAREAID = @DATAAREAID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);
                    command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalDibebaskan = ((decimal) reader["totalDibebaskan"]) * -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }*/

        /*
         * get Ambil Tunai Amount from AX.CPAMBILTUNAI
         * if not found, there is a possibility data still in AX.CPAMBILTUNAITEMP
         * if not found, there is no ambil tunai transaction
         */
        private void getAmbilTunai(RetailTransaction theTransaction)
        {
            int amountFound = 0;
            SqlConnection con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"select 
	                                        CAST(AMOUNT AS DECIMAL(18,0)) AS amountTunai,
                                            CAST(ADMFEE AS DECIMAL(18,0)) AS amountAdmFee
                                        from ax.CPAMBILTUNAI
                                        WHERE 
	                                        TRANSACTIONID = @TRANSACTIONID
	                                        AND DATAAREAID = @DATAAREAID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);
                    command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            amountAmbilTunai = (decimal)reader["amountTunai"];
                            amountAdmFee = (decimal)reader["amountAdmFee"];
                            amountFound = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            if (amountFound == 0)
            {
                con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

                try
                {

                    /*string queryString = @" SELECT PurchaserName, NAMEORDER FROM [CPOrderTable]
                                            WHERE TRANSACTIONID =  @TRANSACTIONID  ";*/

                    string queryString = @"select 
	                                        CAST(AMOUNT AS DECIMAL(18,0)) AS amountTunai,
                                            CAST(ADMFEE AS DECIMAL(18,0)) AS amountAdmFee
                                        from ax.CPAMBILTUNAITEMP
                                        WHERE 
	                                        TRANSACTIONID = @TRANSACTIONID
	                                        AND DATAAREAID = @DATAAREAID";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);
                        command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                amountAmbilTunai = (decimal)reader["amountTunai"];
                                amountAdmFee = (decimal)reader["amountAdmFee"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    throw;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }


            }
        }
        #endregion
        private void getAdmTunai(RetailTransaction theTransaction)
        {
            int amountFound = 0;
            SqlConnection con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"SELECT 	
	                                    TENDERTYPEID,
	                                    FROMDATE,
	                                    TODATE,
	                                    CAST(ADMFEE AS DECIMAL(18,0)) AS amountFee
                                    from ax.CPBANKADM
                                    WHERE 
	                                    [TENDERTYPEID] = @TENDERTYPEID
                                     AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date)";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    //command.Parameters.AddWithValue("@TENDERTYPEID", theTransaction.);
                     

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            amountAmbilTunai = (decimal)reader["amountTunai"];
                            amountFound = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

//            if (amountFound == 0)
//            {
//                con = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

//                try
//                {

//                    /*string queryString = @" SELECT PurchaserName, NAMEORDER FROM [CPOrderTable]
//                                            WHERE TRANSACTIONID =  @TRANSACTIONID  ";*/

//                    string queryString = @"select 
//	                                        CAST(AMOUNT AS DECIMAL(18,0)) AS amountTunai
//                                        from ax.CPAMBILTUNAITEMP
//                                        WHERE 
//	                                        TRANSACTIONID = @TRANSACTIONID
//	                                        AND DATAAREAID = @DATAAREAID";

//                    using (SqlCommand command = new SqlCommand(queryString, connection))
//                    {
//                        command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);
//                        command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

//                        if (connection.State != ConnectionState.Open)
//                        {
//                            connection.Open();
//                        }
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            if (reader.Read())
//                            {
//                                amountAmbilTunai = (decimal)reader["amountTunai"];
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
//                    throw;
//                }
//                finally
//                {
//                    if (connection.State != ConnectionState.Closed)
//                    {
//                        connection.Close();
//                    }
//                }
//            }
        }

        //Customize Ezeelink
  /*      private void getEzeelink(RetailTransaction theTransaction)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"SELECT CARDNUMBER, REDEEMPOINTS, LASTBALANCE, CUSTOMERNAME, M_TRAN_ID
                                        FROM AX.CPEZREDEEM
                                        WHERE TRANSACTIONID = @TRANSACTIONID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cardNumber = reader.GetString(0);
                            redeemPoints = reader.GetFloat(1);
                            lastBalance = reader.GetFloat(2);
                            ezeelinkCustomerName = reader.GetString(3);
                            transID = reader.GetString(4);

                            lblCardNumber = "Ezeelink Card Number: ";
                            lblRedeemPoints = "Ezeelink Redeem Points: ";
                            lblLastBalance = "Ezeelink Balance: ";
                            lblTransID = "Ezeelink Transaction ID: ";

                            command.Dispose();
                            reader.Dispose();
                            reader.Close();

                            string queryStringEarn = @"SELECT CARDNUMBER, EARNPOINTS, LASTBALANCE, CUSTOMERNAME, M_TRAN_ID
                                        FROM AX.CPEZEARN
                                        WHERE TRANSACTIONID = @TRANSACTIONID";

                            using (SqlCommand commandEarn = new SqlCommand(queryStringEarn, connection))
                            {
                                commandEarn.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);

                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }
                                using (SqlDataReader readerEarn = commandEarn.ExecuteReader())
                                {
                                    if (readerEarn.Read())
                                    {
                                        cardNumber = readerEarn.GetString(0);
                                        redeemPoints = readerEarn.GetFloat(1);
                                        lastBalance = readerEarn.GetFloat(2);
                                        ezeelinkCustomerName = readerEarn.GetString(3);
                                        transID = readerEarn.GetString(4);

                                        lblCardNumber = "Ezeelink Card Number: ";
                                        lblRedeemPoints = "Ezeelink Earn Points: ";
                                        lblLastBalance = "Ezeelink Balance: ";
                                        lblTransID = "Ezeelink Transaction ID: ";
                                    }
                                }
                            }
                        }
                        else
                        {
                            command.Dispose();
                            reader.Dispose();
                            reader.Close();

                            string queryStringEarn = @"SELECT CARDNUMBER, EARNPOINTS, LASTBALANCE, CUSTOMERNAME, M_TRAN_ID
                                        FROM AX.CPEZEARN
                                        WHERE TRANSACTIONID = @TRANSACTIONID";

                            using (SqlCommand commandEarn = new SqlCommand(queryStringEarn, connection))
                            {
                                commandEarn.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);

                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }
                                using (SqlDataReader readerEarn = commandEarn.ExecuteReader())
                                {
                                    if (readerEarn.Read())
                                    {
                                        cardNumber = readerEarn.GetString(0);
                                        earnPoints = readerEarn.GetFloat(1);
                                        totalBalance = readerEarn.GetFloat(2);
                                        ezeelinkCustomerName = readerEarn.GetString(3);
                                        transID = readerEarn.GetString(4);

                                        lblCardNumber = "Ezeelink Card Number: ";
                                        lblEarnPoints = "Ezeelink Earn Points: ";
                                        lblTotalBalance = "Ezeelink Balance: ";
                                        lblTransID = "Ezeelink Transaction ID: ";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        */
        //End Add by Erwin

        // add by julius S 
        private void getMemberName(RetailTransaction theTransaction)
        {
            //modified by Yonathan 11/01/2024 to get member detail from RTS
            ReadOnlyCollection<object> containerArray;
            containerArray = Printing.InternalApplication.TransactionServices.InvokeExtension("getLoyaltyDetail", theTransaction.LoyaltyItem.LoyaltyCardNumber);
            
                
            loyaltyFullName = containerArray[4].ToString();
            loyaltyPhone = containerArray[6].ToString();
        //    loyaltyFullName = "test";
            //    loyaltyPhone = theTransaction.LoyaltyItem.LoyaltyCardNumber; [DevDynamicsAX].[dbo].[CP_LOYALTYCARDSSIRCLO]


         //   SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

//            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;

//            SqlConnection connection = new SqlConnection(connectionString);

//            try
//            {
//                string queryString = @" SELECT FULLNAME , HANDPHONE FROM [dbo].[CP_LOYALTYCARDSSIRCLO]
//                                         WHERE CARDNUMBER = @CARDNUMBER ";

//                using (SqlCommand command = new SqlCommand(queryString, connection))
//                {
//                    command.Parameters.AddWithValue("@CARDNUMBER", theTransaction.LoyaltyItem.LoyaltyCardNumber);

//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            loyaltyFullName = reader.GetString(0);
//                            loyaltyPhone = reader.GetString(1);

//                        }
//                    }
//                }


//            }
//            catch (Exception ex)
//            {
//                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
//                throw;
//            }
//            finally
//            {
//                if (connection.State != ConnectionState.Closed)
//                {
//                    connection.Close();
//                }
//            }
            
        }

        //Add by Heron
        private void getNoOrder(RetailTransaction theTransaction)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string queryString = @" SELECT NOORDER, NAMEORDER FROM [CPOrderTable]
                                        WHERE TRANSACTIONID =  @TRANSACTIONID  ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", theTransaction.TransactionId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            OrderNo = reader.GetString(0);
                            OrderName = reader.GetString(1);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        private void getNPWPDetail(RetailTransaction theTransaction)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @" SELECT NPWPAddress , NPWPArea FROM ax.[RETAILSTORETABLE]
                                         WHERE STORENUMBER = @STOREID ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STOREID", theTransaction.StoreId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            npwpAddress = reader.GetString(0);
                            npwpArea = reader.GetString(1);

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        //end added by Heron
        /// <summary>
        /// Loyalty reward point summary class.
        /// </summary>
        private class LoyaltyRewardPointSummary
        {
            public string RewardPointId { get; set; }
            public LoyaltyRewardPointType RewardPointType { get; set; }
            public decimal RewardPointAmountQuantity { get; set; }
            public string RewardPointCurrency { get; set; }
        }
    }
}
