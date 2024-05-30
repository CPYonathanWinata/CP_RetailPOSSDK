/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/
﻿﻿
namespace Microsoft.Dynamics.Retail.Localization.Sweden.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using LSRetailPosis.Transaction;

    /// <summary>
    /// Used for interaction with sweden control unit.
    /// </summary>
    [Serializable]
    public static class DataConverter
    {
        /// <summary>
        /// Serialize retail transaction to XML doc.
        /// </summary>
        /// <param name="retailTransaction">Transaction</param>
        /// <param name="registrationDate">Fiscal registration date</param>
        /// <param name="receiptId">Id of the receipt</param>
        /// <param name="transactionType">Type of the transaction</param>
        /// <param name="storeId">Organization number</param>
        /// <returns>Serialized retail transaction in XML format</returns>
        public static XDocument SerializeRetailTransactionToXml(IRetailTransaction transaction, DateTime registrationDate, string receiptId, int transactionType, string storeId)
        {
            var retailTransaction = (RetailTransaction)transaction;
            var firstValidlSaleItem = retailTransaction.SaleItems.Where(i => i != null && !i.Voided).FirstOrDefault();
            if (firstValidlSaleItem == null)
            {
                throw new InvalidOperationException("retailTransaction.SaleItems doesn't contain valid items"); 
            }
            
            var rootElmt = new XElement("Transaction",
                new XElement("StoreId", storeId),
                new XElement("TerminalId", retailTransaction.TerminalId),
                new XElement("TransactionDate", registrationDate.ToString("s")), //ISO 8601
                new XElement("ReceiptId", String.IsNullOrWhiteSpace(receiptId) ? retailTransaction.ReceiptId : receiptId),
                new XElement("TotalAmount", retailTransaction.NetAmountWithTax),
                new XElement("QtyOrdered", firstValidlSaleItem.Quantity),
                new XElement("TransactionType", transactionType),
                new XElement("TaxLines",
                    retailTransaction.TaxLines.Select(t => 
                        new XElement("TaxLine",
                            new XElement("TaxGroup", t.TaxGroup),
                            new XElement("TaxCode", t.TaxCode),
                            new XElement("TaxAmount", t.Amount),
                            new XElement("TaxPercentage", t.Percentage)
                        )
                    )
                )
            );

            var xDoc = new XDocument(rootElmt);
            return xDoc;
        }
    
        /// <summary>
        /// Gets control code value from received answer from a control unit.
        /// </summary>
        /// <param name="response">Response from control unit in xml format</param>
        /// <returns>Control unit code</returns>
        public static string GetControlCodeFromAnswer(string response)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(response))
            {
                XDocument xResponse = XDocument.Parse(response);
                retVal = xResponse.Root.Descendants("ControlCode").First().Value;
            }

            return retVal;
        }

        /// <summary>
        /// Gets control unit name value from received answer from a control unit.
        /// </summary>
        /// <param name="response">Response from control unit in xml format</param>
        /// <returns>Control unit name</returns>
        public static string GetUnitNameFromAnswer(string response)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(response))
            {
                XDocument xResponse = XDocument.Parse(response);
                retVal = xResponse.Root.Descendants("UnitName").First().Value;                
            }

            return retVal;
        }
    }
}
