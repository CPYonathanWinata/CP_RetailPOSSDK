/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.ProductSearchAttributes
{
    internal class ProductSearchFilter : IProductSearchFilter
    {
        public ProductSearchFilter()
        {
            this.AttributesFilter = new List<IAttributeFilterEntry>();
            this.ColorDimensionFilter = new List<IProductSearchAttributeFilterValue>();
            this.SizeDimensionFilter = new List<IProductSearchAttributeFilterValue>();
            this.StyleDimensionFilter = new List<IProductSearchAttributeFilterValue>();
            this.ConfigurationDimensionFilter = new List<IProductSearchAttributeFilterValue>();

            this.ChannelId = LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId;
            this.LanguageId = LSRetailPosis.Settings.ApplicationSettings.Terminal.CultureName;
        }

        public IList<IAttributeFilterEntry> AttributesFilter { get; set; }

        public IList<IProductSearchAttributeFilterValue> ColorDimensionFilter { get; set; }

        public IList<IProductSearchAttributeFilterValue> SizeDimensionFilter { get; set; }

        public IList<IProductSearchAttributeFilterValue> StyleDimensionFilter { get; set; }

        public IList<IProductSearchAttributeFilterValue> ConfigurationDimensionFilter { get; set; }

        public string SearchValue { get; set; }

        public string OrderBy { get; set; }

        public int StartRow { get; set; }

        public int RowsCount { get; set; }

        public bool SortAscending { get; set; }

        public string LanguageId { get; set; }

        public long ChannelId { get; set; }

        public long CategoryId { get; set; }

        public dynamic PartnerData { get; set; }

        public override string ToString()
        {
            string sNone = LSRetailPosis.ApplicationLocalizer.Language.Translate(107108); // None
            if (this.AttributesFilter == null && this.ColorDimensionFilter == null && this.SizeDimensionFilter == null && 
                this.StyleDimensionFilter == null && this.ConfigurationDimensionFilter == null)
                return sNone;

            List<string> filterStrings = new List<string>();

            foreach (var filter in this.AttributesFilter)
            {
                string valuesList = null;
                string valuesFrom = null;
                string valuesTo = null;
                string valuesText = null;
                string filterString = null;

                if (filter.ProductSearchAttributeFilter.FilterValuesList != null && filter.ProductSearchAttributeFilter.FilterValuesList.Count > 0)
                {
                    if (filter.ProductAttributeInfo.DataType == AttributeDataType.Boolean)
                    {
                        var filterValue = filter.ProductSearchAttributeFilter.FilterValuesList.FirstOrDefault();
                        if (filterValue.Value == null)
                        {
                            valuesList = string.Format("{0}, {1}", 
                                LSRetailPosis.ApplicationLocalizer.Language.Translate(50154),   //No
                                LSRetailPosis.ApplicationLocalizer.Language.Translate(50155));  //Yes
                        }
                        else
                        {
                            valuesList = (bool)filterValue.Value ? 
                                LSRetailPosis.ApplicationLocalizer.Language.Translate(50155) :  //Yes
                                LSRetailPosis.ApplicationLocalizer.Language.Translate(50154);   //No
                        }
                    }
                    else
                    {
                        valuesList = string.Join(", ", filter.ProductSearchAttributeFilter.FilterValuesList.Select(f => f.Value));
                    }
                }
                if (filter.ProductSearchAttributeFilter.RangeFrom != null)
                {
                    if (filter.ProductSearchAttributeFilter.RangeFrom.Value is DateTime)
                    {
                        valuesFrom = ((DateTime)filter.ProductSearchAttributeFilter.RangeFrom.Value).ToString("d", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        valuesFrom = filter.ProductSearchAttributeFilter.RangeFrom.Value.ToString();
                    }
                }
                if (filter.ProductSearchAttributeFilter.RangeTo != null)
                {
                    if (filter.ProductSearchAttributeFilter.RangeTo.Value is DateTime)
                    {
                        valuesTo = ((DateTime)filter.ProductSearchAttributeFilter.RangeTo.Value).ToString("d", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        valuesTo = filter.ProductSearchAttributeFilter.RangeTo.Value.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(filter.ProductSearchAttributeFilter.TextFilter))
                {
                    valuesText = filter.ProductSearchAttributeFilter.TextFilter;
                }

                if (!string.IsNullOrEmpty(valuesList))
                {
                    filterString = string.Format("{0}: {1}", filter.ProductAttributeInfo.Name, valuesList);
                }
                else if (!string.IsNullOrEmpty(valuesFrom) || !string.IsNullOrEmpty(valuesTo))
                {
                    List<string> fromToRange = new List<string>(2);
                    if (!string.IsNullOrEmpty(valuesFrom))
                    {
                        fromToRange.Add(string.Format("{0} {1}", LSRetailPosis.ApplicationLocalizer.Language.Translate(107106), valuesFrom)); // From
                    }
                    if (!string.IsNullOrEmpty(valuesTo))
                    {
                        fromToRange.Add(string.Format("{0} {1}", LSRetailPosis.ApplicationLocalizer.Language.Translate(107107), valuesTo)); // To
                    }

                    filterString = string.Format("{0}: {1}", filter.ProductAttributeInfo.Name, string.Join(", ", fromToRange));
                }
                else if (!string.IsNullOrEmpty(valuesText))
                {
                    filterString = string.Format("{0}: {1}", filter.ProductAttributeInfo.Name, valuesText);
                }
                filterStrings.Add(filterString);
            }

            if (this.ColorDimensionFilter.Count > 0)
            {
                filterStrings.Add(string.Format("{0}: {1}", LSRetailPosis.ApplicationLocalizer.Language.Translate(1861), //Color
                    string.Join(", ", this.ColorDimensionFilter.Select(c => c.Value))));
            }
            if (this.SizeDimensionFilter.Count > 0)
            {
                filterStrings.Add(string.Format("{0}: {1}", LSRetailPosis.ApplicationLocalizer.Language.Translate(1862), //Size
                    string.Join(", ", this.SizeDimensionFilter.Select(c => c.Value))));
            }
            if (this.StyleDimensionFilter.Count > 0)
            {
                filterStrings.Add(string.Format("{0}: {1}", LSRetailPosis.ApplicationLocalizer.Language.Translate(1863), //Style
                    string.Join(", ", this.StyleDimensionFilter.Select(c => c.Value))));
            }
            if (this.ConfigurationDimensionFilter.Count > 0)
            {
                filterStrings.Add(string.Format("{0}: {1}", LSRetailPosis.ApplicationLocalizer.Language.Translate(1867), //Configuration
                    string.Join(", ", this.ConfigurationDimensionFilter.Select(c => c.Value))));
            }

            string returnVal = sNone;

            if (filterStrings.Count > 0)
                returnVal = string.Join("; ", filterStrings);

            return returnVal;
        }

        public IProductSearchFilter CreateProductSearchFilter()
        {
            return new ProductSearchFilter();
        }

        public IProductAttributeInfo CreateProductAttributeInfo()
        {
            return new ProductAttributeInfo();
        }

        public IProductSearchAttributeFilter CreateProductSearchAttributeFilter()
        {
            return new ProductSearchAttributeFilter();
        }

        public IProductSearchAttributeFilterValue CreateProductSearchAttributeFilterValue()
        {
            return new ProductSearchAttributeFilterValue();
        }

        public IAttributeFilterEntry CreateAttributeFilterEntry(IProductAttributeInfo productAttributeInfo, IProductSearchAttributeFilter productSearchAttributeFilter)
        {
            return new AttributeFilterEntry(productAttributeInfo, productSearchAttributeFilter);
        }
    }
}