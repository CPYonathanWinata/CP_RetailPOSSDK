/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using LSRetailPosis.Transaction.Line.SaleItem;

namespace Microsoft.Dynamics.Retail.Pos.CustomField
{
    /// <summary>
    /// Implementation of ICustomField service
    /// Provides run-time values for custom UI layout fields in the POS
    /// </summary>
    [Export(typeof(ICustomField))]
    public class CustomField : ICustomField
    {

        #region ICustomFieldtV1 Members

        /// <summary>
        /// Return values for custom ItemReceipt fields
        /// </summary>
        /// <param name="fields">collection of the custom fields that exist</param>
        /// <param name="saleLine">current sale line</param>
        /// <param name="posTransaction">current transaction</param>
        /// <returns>dictionary of [Field Name, Field Value] pairs</returns>
        public IDictionary<string, string> PopulateItemReceiptFields(IEnumerable<CustomFieldValue> fields, ISaleLineItem saleLineItem, IPosTransaction posTransaction)
        {
#if DEBUG
            if (saleLineItem != null)
            {
                Dictionary<string, string> fieldResults = new Dictionary<string, string>();
                fieldResults.Add("ITEM_1", saleLineItem.Price.ToString());
                fieldResults.Add("ITEM_2", saleLineItem.LineId.ToString());
                fieldResults.Add("ITEM_3", saleLineItem.TaxRatePct.ToString());
                return fieldResults;
            }
#endif
            return null;
        }

        /// <summary>
        /// Return values for custom Payment fields
        /// </summary>
        /// <param name="fields">collection of the custom fields that exist</param>
        /// <param name="saleLine">current sale line</param>
        /// <param name="posTransaction">current transaction</param>
        /// <returns>dictionary of [Field Name, Field Value] pairs</returns>
        public IDictionary<string, string> PopulatePaymentFields(IEnumerable<CustomFieldValue> fields, ITenderLineItem tenderLineItem, IPosTransaction posTransaction)
        {
#if DEBUG
            if (tenderLineItem != null)
            {
                Dictionary<string, string> fieldResults = new Dictionary<string, string>();
                fieldResults.Add("PAY_1", tenderLineItem.ExchangeRate.ToString());
                fieldResults.Add("PAY_2", tenderLineItem.LineId.ToString());
                return fieldResults;
            }
#endif
            return null;
        }

        /// <summary>
        /// Return values for custom Total fields
        /// </summary>
        /// <param name="fields">collection of the custom fields that exist</param>
        /// <param name="posTransaction">current transaction</param>
        /// <returns>dictionary of [Field Name, Field Value] pairs</returns>
        public IDictionary<string, string> PopulateTotalFields(IEnumerable<CustomFieldValue> fields, IPosTransaction posTransaction)
        {
#if DEBUG
            if (posTransaction != null)
            {
                Dictionary<string, string> fieldResults = new Dictionary<string, string>();
                fieldResults.Add("TOTALS_1", posTransaction.TransactionId.ToString());
                return fieldResults;
            }
#endif
            return null;
        }

        #endregion

        #region ICustomFieldtV2 Members

        const string itemPreviewTextDiscountField = "Discount";
        const string itemPreviewTextQuantityField = "Quantity";
        const string itemPreviewTextPriceOverriddenField = "PriceOverridden";
        const string itemPreviewTextDimensionField = "Dimension";
        const string itemPreviewTextSalesPersonField = "SalesPerson";
        const string itemPreviewTextSerialIdField = "SerialId";

        /// <summary>
        /// Returns the ItemReceipt Preview Text.
        /// </summary>
        /// <param name="previewComments">The preview Comments.</param>
        /// <param name="posTransaction">The pos transaction.</param>
        /// <param name="saleLineItem">The sale line item.</param>
        /// <returns>List[comments]</returns>
        public IDictionary<string, string> PopulateItemReceiptPreviewText(IDictionary<string, string> previewComments, IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
#if DEBUG
            if (previewComments != null && posTransaction != null)
            {
                SaleLineItem saleLine = (SaleLineItem)saleLineItem;

                // Customization of the comments can be done as below.            
                //previewComments[itemPreviewTextDiscountField] = "Your discount is " + saleLine.PartnerData.CustomPercentDiscountField.Tostring() + " Percent.";               

                // previewComments[itemPreviewTextQuantityField] = "User customized quantity";
                // if(saleLine.PriceOverridden)
                // {
                // previewComments[itemPreviewTextPriceOverriddenField] = "User customized text";
                // }
                // previewComments[itemPreviewTextDimensionField] = "User customized Dimension";                 
                // if (saleLine.SalesPersonId)
                // {                     
                // previewComments[itemPreviewTextSalesPersonField] = "User Custom Text";
                // }
                // if (saleLine.SerialId)
                // {
                // previewComments[itemPreviewTextSerialIdField] = "User Custom Text";
                // }   
            }
#endif
            return previewComments;
        }

        #endregion

    }
}
