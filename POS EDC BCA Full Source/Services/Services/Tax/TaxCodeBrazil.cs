/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.Tax
{
    /// <summary>
    /// Provides specific logic for the Brazilian tax engine
    /// </summary>
    public sealed class TaxCodeBrazil : TaxCode
    {
        private TaxFiscalValue _fiscalValue = TaxFiscalValue.Blank;
        private TaxDataBrazil taxData;

        private const int IcmsTaxType = 2;

        /// <summary>
        /// Flags if this specific tax code is included in the sales price
        /// </summary>
        public bool IncludedTax { get; private set; }

        /// <summary>
        /// The Brazilian tax type
        /// </summary>
        public int TaxType { get; set; }

        /// <summary>
        /// The Brazilian fiscal value
        /// </summary>
        public TaxFiscalValue FiscalValue
        {
            get
            {
                if (_fiscalValue == TaxFiscalValue.Blank)
                {
                    _fiscalValue = taxData.GetFiscalValue(LineItem.SalesTaxGroupId, LineItem.TaxGroupId, Code);
                }

                return _fiscalValue;
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">TaxCode</param>
        /// <param name="lineItem">The line item which the tax is being applied to</param>
        /// <param name="taxGroup">TaxGroup that this Tax Code belongs to</param>
        /// <param name="currency">Currency that this tax is calculated in</param>
        /// <param name="value">Value/Rate of the tax</param>
        /// <param name="limitMin">Minimum amount required to calculate this tax.</param>
        /// <param name="limitMax">Maximum amount required to calculate this tax</param>
        /// <param name="exempt">Whether or not this tax is exempt</param>
        /// <param name="taxBase">Origin from which sales tax is calculated</param>
        /// <param name="limitBase">Basis of sales tax limits</param>
        /// <param name="method">Whether tax is calculated for entire amounts or for intervals</param>
        /// <param name="taxOnTax">TaxCode of the other sales tax that this tax is based on.</param>
        /// <param name="unit">Unit for calculating per-unit amounts</param>
        /// <param name="collectMin">Collection limits, the minimum tax that can be collected</param>
        /// <param name="collectMax">Collection limits, the maximum tax that can be collected</param>
        /// <param name="groupRounding">Whether or not this code should be rounded at the Tax Group scope.</param>
        /// <param name="includedTax">Whether or not this tax is included in sales price</param>
        /// <param name="taxType">The Brazilian tax type</param>
        /// <param name="provider">The tax code provider that created this instance.</param>
        public TaxCodeBrazil(
            string code,
            ITaxableItem lineItem,
            string taxGroup,
            string currency,
            decimal value,
            decimal limitMin,
            decimal limitMax,
            bool exempt,
            TaxBase taxBase,
            TaxLimitBase limitBase,
            TaxCalculationMode method,
            string taxOnTax,
            string unit,
            decimal collectMin,
            decimal collectMax,
            bool groupRounding,
            bool includedTax,
            int taxType,
            TaxCodeProvider provider)
            : base(
                code, lineItem, taxGroup, currency, value, limitMin, limitMax, exempt, taxBase, limitBase, method,
                taxOnTax, unit, false, collectMin, collectMax, groupRounding, provider)
        {
            IncludedTax = includedTax;
            TaxType = taxType;

            this.taxData = (provider as TaxCodeProviderBrazil).TaxData;
        }

        /// <summary>
        /// Flags if the tax amount should be included in the sales price
        /// </summary>
        public override bool TaxIncludedInPrice
        {
            get { return IncludedTax; }
        }

        /// <summary>
        /// Sets the tax line amount to 0 when its type is ICMS excluded
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public override decimal CalculateTaxExcluded(ReadOnlyCollection<TaxCode> codes)
        {
            return ZeroTaxAmount() ? decimal.Zero : base.CalculateTaxExcluded(codes);
        }

        /// <summary>
        /// Sets the tax line amount to 0 when its type is ICMS included
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public override decimal CalculateTaxIncluded(ReadOnlyCollection<TaxCode> codes)
        {
            return ZeroTaxAmount() ? decimal.Zero : base.CalculateTaxIncluded(codes);
        }

        private bool ZeroTaxAmount()
        {
            var taxCodeProviderBrazil = Provider as TaxCodeProviderBrazil;
            var shouldZeroTaxAmount = false;

            if (taxCodeProviderBrazil != null)
            {
                if (FiscalValue != TaxFiscalValue.WithCreditDebit)
                {
                    shouldZeroTaxAmount = true;
                }

                if (TaxType == IcmsTaxType && LineItem.RetailTransaction.SubType == RetailTransactionSubType.FiscalReceipt)
                {
                    shouldZeroTaxAmount = true;
                }
            }

            return shouldZeroTaxAmount;
        }
    }
}
