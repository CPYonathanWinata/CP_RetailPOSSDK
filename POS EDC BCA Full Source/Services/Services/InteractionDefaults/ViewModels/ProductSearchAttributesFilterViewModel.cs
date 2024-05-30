/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Interaction.ProductSearchAttributes;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// Model for frmProductSearchAttributes.
    /// </summary>
    internal sealed class ProductSearchAttributesFilterViewModel
    {

        #region Enums
        enum InventDimFieldId
        {
            Configuration = 7,
            Size = 8,
            Color = 9,
            Style = 16002
        };
        
        #endregion

        #region Constants
        private const int PRODUCTDIMENSIONSCOUNT = 4;
        private const int MINIMUM_SEARCH_TERM_LENGTH = 2;  // Minimum two characters are reqruied for search.
        #endregion

        #region Properties
        private IProductSearchFilter searchFilter;
        private IProductSearchFilter attributesCache;
        private long categoryId;
        
        /// <summary>
        /// List of all search attributes with filters
        /// </summary>
        public List<AttributeElement> AttributesElementList { get; private set; }

        #endregion

        #region Main Functions
        /// <summary>
        /// Constructor for the <c>ProductSearchAttributesFilterViewModel</c>.
        /// </summary>
        /// <param name="productAttributeSearchFilter">Search filter.</param>
        /// <param name="productAttributesValuesCache">Saved list of attributes and value list for attributes.</param>
        /// <param name="selectedCategoryId">Id of selected category.</param>
        public ProductSearchAttributesFilterViewModel(IProductSearchFilter productAttributeSearchFilter, IProductSearchFilter productAttributesValuesCache, long selectedCategoryId)
        {
            if (productAttributeSearchFilter == null)
            {
                throw new ArgumentNullException("productAttributeSearchFilter");
            }
            searchFilter = productAttributeSearchFilter;
            attributesCache = productAttributesValuesCache;
            categoryId = selectedCategoryId;
        }

        /// <summary>
        /// Initializes view model.
        /// </summary>
        public void Initialize()
        {
            LoadAllAtributesElements();
            InitAttributesFilter();
            LoadAttributesFilterValuesListFromCache();
            ApplySearchFilter();
        }
        #endregion

        #region Attributes Element

        /// <summary>
        /// This methods validates that range is set correctly and if not ArgumentOutOfRangeException would be thrown.
        /// </summary>
        /// <param name="rangeFromValue">Range from AttributeElementValue object.</param>
        /// <param name="newRangeFromValue">new value to check.</param>       
        /// <param name="errMessage">text of the error message.</param>
        /// <returns></returns>
        public bool ValidateRangeFromValue(AttributeElementValue rangeFromValue, object newRangeFromValue, out string errMessage)
        {
            bool retVal = true;
            errMessage = string.Empty;            
            var attributeElement = this.AttributesElementList.Where(atrFilter => atrFilter.Filter.RangeFrom == rangeFromValue).FirstOrDefault();
            if (!string.IsNullOrEmpty(newRangeFromValue.ToString()) && attributeElement != null && attributeElement.Filter.RangeTo != null && attributeElement.Filter.RangeTo.Value != null && !string.IsNullOrEmpty(attributeElement.Filter.RangeTo.Value.ToString()))
            {
                if (!CompareRangeValues(newRangeFromValue, attributeElement.Filter.RangeTo.Value, attributeElement.DataType, out errMessage))
                {
                    retVal = false;
                }
            }
            return retVal;
        }

        /// <summary>
        /// This methods validates that range is set correctly and if not ArgumentOutOfRangeException would be thrown.
        /// </summary>
        /// <param name="rangeToValue">Range to AttributeElementValue object.</param>
        /// <param name="newRangeToValue">new value to check.</param>
        /// <param name="errMessage">text of the error message.</param>
        /// <returns></returns>
        public bool ValidateRangeToValue(AttributeElementValue rangeToValue, object newRangeToValue, out string errMessage)
        {
            bool retVal = true;
            errMessage = string.Empty;
            var attributeElement = this.AttributesElementList.Where(atrFilter => atrFilter.Filter.RangeTo == rangeToValue).FirstOrDefault();
            if (!string.IsNullOrEmpty(newRangeToValue.ToString()) && attributeElement != null && attributeElement.Filter.RangeFrom != null && attributeElement.Filter.RangeFrom.Value != null && !string.IsNullOrEmpty(attributeElement.Filter.RangeFrom.Value.ToString()))
            {
                if (!CompareRangeValues(attributeElement.Filter.RangeFrom.Value, newRangeToValue, attributeElement.DataType, out errMessage))
                {
                    retVal = false;
                }
            }
            return retVal;
        }

        /// <summary>
        /// This methods validates that range is set correctly and if not ArgumentException would be thrown.
        /// </summary>
        /// <param name="textFilter">Value to check.</param>
        /// <param name="errMessage">text of the error message.</param>
        public bool ValidTextFilter(string textFilter, out string errMessage)
        {
            bool retVal = true;
            errMessage = ApplicationLocalizer.Language.Translate(61519); // Search requires a minimum of two characters.
            if (textFilter.Length > 0 && textFilter.Length < MINIMUM_SEARCH_TERM_LENGTH)
            {
                retVal = false;
            }
            return retVal;
        }

        private bool CompareRangeValues(object valueFrom, object valueTo, AttributeDataType attributeDataType, out string errMessage)
        {
            bool retVal = true;
            errMessage = string.Empty;
            switch (attributeDataType)
            {
                case AttributeDataType.Integer:
                    retVal = !((int)Convert.ToDecimal(valueFrom) > (int)Convert.ToDecimal(valueTo));
                    break;
                case AttributeDataType.Currency:
                case AttributeDataType.Decimal:
                    retVal = !(Convert.ToDecimal(valueFrom) > Convert.ToDecimal(valueTo));
                    break;
                case AttributeDataType.Text:
                    retVal = !(valueFrom.ToString().CompareTo(valueTo) > 0);
                    break;
                case AttributeDataType.DateTime:
                    retVal = !(Convert.ToDateTime(valueFrom) > Convert.ToDateTime(valueTo));
                    break;
                default:
                    break;
            }
            if (!retVal)
            {
                //The ending attribute value of the search range is less than the starting attribute value. 
                //Specify an ending attribute value that is greater than or equal to the starting attribute value.
                errMessage = ApplicationLocalizer.Language.Translate(107104);
            }
            return retVal;
        }

        private void LoadAllAtributesElements()
        {
            AttributesElementList = GetAllAttributesElements();
            InitAttributesElementsIds(AttributesElementList);
            SortAttributesElements(AttributesElementList);
        }

        private List<AttributeElement> GetAllAttributesElements()
        {
            var retVal = new List<AttributeElement>();
            retVal.AddRange(this.GetProductDimensionAttributes());
            retVal.AddRange(this.GetProductAttributes());
            retVal.AddRange(this.GetPartnersAttributes());
            return retVal;
        }

        private bool IsDimensionRefinable(InventDimFieldId dimensionFieldId, DataTable inventDimMetadata)
        {
            bool isRefinable = false;

            var rows = inventDimMetadata.Select(string.Format("DIMENSIONFIELDID = {0}", (int)dimensionFieldId));
            if (rows != null && rows.Count() > 0)
            {
                isRefinable = Convert.ToBoolean(rows[0]["ISREFINABLE"]);
            }
            
            return isRefinable;
        }


        private List<AttributeElement> GetProductDimensionAttributes()
        {

            var table = AttributeSearcher.GetSearchInventDimensionsList(searchFilter.ChannelId);

            var retVal = new List<AttributeElement>(PRODUCTDIMENSIONSCOUNT);

            if (IsDimensionRefinable(InventDimFieldId.Color, table))
            { 
                retVal.Add(
                    new AttributeElement()
                    {
                        Name = ApplicationLocalizer.Language.Translate(1861), // "Color"   
                        Type = AttributeType.DimensionColor,
                        ValueChecked = false,
                    });
            }

            if (IsDimensionRefinable(InventDimFieldId.Size, table))
            { 
                retVal.Add(
                    new AttributeElement()
                    {
                        Name = ApplicationLocalizer.Language.Translate(1862), // "Size"
                        Type = AttributeType.DimensionSize,
                        ValueChecked = false,
                    });

            }

            if (IsDimensionRefinable(InventDimFieldId.Style, table))
            { 
                retVal.Add(
                    new AttributeElement()
                        {
                            Name = ApplicationLocalizer.Language.Translate(1863), // "Style"
                            Type = AttributeType.DimensionStyle,
                            ValueChecked = false,
                        });
            }
 
            if (IsDimensionRefinable(InventDimFieldId.Configuration, table))
            {
                retVal.Add(
                     new AttributeElement()
                        {
                            Name = ApplicationLocalizer.Language.Translate(1867), // "Configuration"
                            Type = AttributeType.DimensionConfiguration,
                            ValueChecked = false,
                        });
            }
            
            return retVal;
        }

        private List<AttributeElement> GetProductAttributes()
        {
            var retVal = new List<AttributeElement>();

            if (this.attributesCache != null && this.attributesCache.AttributesFilter != null && this.attributesCache.AttributesFilter.Count != 0)
            {
                retVal.AddRange(LoadProductAttributesFromCache());
            }
            else
            {
                retVal.AddRange(LoadProductAttributesFromDB());
            }

            return retVal;
        }

        private List<AttributeElement> LoadProductAttributesFromDB()
        {

            var retVal = new List<AttributeElement>();
            var dt = AttributeSearcher.GetSearchProductAttributes(searchFilter.ChannelId, this.categoryId);
            foreach (var row in dt.Select())
            {
                retVal.Add(new AttributeElement()
                {
                    Name = row["NAME"].ToString(),
                    ExternalId = Convert.ToInt64(row["ATTRIBUTE"]),
                    IsEnumerable = Convert.ToBoolean(row["ISENUMERATION"]),
                    DataType = (AttributeDataType)Convert.ToInt32(row["DATATYPE"]),
                    Type = AttributeType.ProductAttribute,
                    ValueChecked = false
                });
            }
            return retVal;
        }

        private List<AttributeElement> LoadProductAttributesFromCache()
        {
            var retVal = new List<AttributeElement>();
            foreach (var attribute in this.attributesCache.AttributesFilter.Select(a => a.ProductAttributeInfo))
            {
                var attributeElement = new AttributeElement();
                attributeElement.Type = AttributeType.ProductAttribute;
                attributeElement.ExternalId = attribute.Id;
                attributeElement.Name = attribute.Name.ToString();
                attributeElement.DataType = attribute.DataType;
                attributeElement.IsEnumerable = attribute.IsEnumeration;
                attributeElement.ValueChecked = false;
                retVal.Add(attributeElement);
            }
            return retVal;
        }

        private List<AttributeElement> GetPartnersAttributes()
        {
            var retVal = new List<AttributeElement>();
            // Implement customization here
            return retVal;
        }

        private void InitAttributesElementsIds(List<AttributeElement> attributeElementsList)
        {
            int idx = 0;
            foreach (var attributeElement in attributeElementsList)
            {
                attributeElement.Id = idx;
                idx++;
            }
        }

        private void SortAttributesElements(List<AttributeElement> attributeElementsList)
        {
            attributeElementsList = attributeElementsList.OrderBy(l => l.Type).ThenBy(l => l.Name).ToList();
        }
        #endregion

        /// <summary>
        /// Load all attributes for specified attribute element.
        /// </summary>
        /// <param name="attributeElement">Attribute element</param>
        public void LoadAttributeFilter(AttributeElement attributeElement)
        {
            if (AttributesElementList.Contains(attributeElement) && attributeElement.Filter.AttributeFilterType == AttributeFilterType.ValueList)
            {
                LoadAttributeFilterValueList(attributeElement);
            }
        }

        private void LoadAttributeFilterValueList(AttributeElement attributeElement)
        {
            if (attributeElement.Filter.AttributeElementValueList.Count != 0)
            {
                return;
            }

            switch (attributeElement.Type)
            {
                case AttributeType.DimensionColor:
                case AttributeType.DimensionConfiguration:
                case AttributeType.DimensionSize:
                case AttributeType.DimensionStyle:
                    LoadFilterValuesListForProductDimension(attributeElement);
                    break;
                case AttributeType.ProductAttribute:
                    LoadFilterValuesListForProductAttribute(attributeElement);
                    break;
                case AttributeType.PartnerData:
                    // Implement customization here
                    break;
                default:
                    break;
            }
        }

        private void LoadFilterValuesListForProductAttribute(AttributeElement attributeElement)
        {
            if (attributeElement.Filter.AttributeElementValueList.Count == 0)
            {
                LoadProductAttributesValuesListFromDB(attributeElement);
            }
        }

        private void LoadFilterValuesListForProductDimension(AttributeElement attributeElement)
        {
            if (attributeElement.Filter.AttributeElementValueList.Count == 0)
            {
                LoadProductDimensionValuesFromDB(attributeElement);
            }
        }

        private void LoadProductDimensionValuesFromDB(AttributeElement attributeElement)
        {
            DataTable dt = null;
            try
            {
                switch (attributeElement.Type)
                {
                    case AttributeType.DimensionColor:
                        dt = AttributeSearcher.GetSearchProductColors(searchFilter.ChannelId);
                        break;
                    case AttributeType.DimensionConfiguration:
                        dt = AttributeSearcher.GetSearchProductConfigurations(searchFilter.ChannelId);
                        break;
                    case AttributeType.DimensionSize:
                        dt = AttributeSearcher.GetSearchProductSizes(searchFilter.ChannelId);
                        break;
                    case AttributeType.DimensionStyle:
                        dt = AttributeSearcher.GetSearchProductStyles(searchFilter.ChannelId);
                        break;
                    default:
                        dt = new DataTable();
                        break;
                }

                foreach (var row in dt.Select())
                {
                    attributeElement.Filter.AttributeElementValueList.Add(new AttributeElementValue()
                    {
                        Value = row["NAME"].ToString(),
                        ExternalId = Convert.ToInt64(row["RECID"]),
                        ValueChecked = false
                    });
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        private void LoadProductAttributesValuesListFromDB(AttributeElement attributeElement)
        {
            var dt = AttributeSearcher.GetAttributeListOfValues(searchFilter.ChannelId, attributeElement.ExternalId);
            foreach (var row in dt.Select())
            {
                attributeElement.Filter.AttributeElementValueList.Add(new AttributeElementValue()
                {
                    Value = row["VALUE"].ToString(),
                    ExternalId = Convert.ToInt64(row["ATTRIBUTE"]),
                    ValueChecked = false
                });
            }
        }

        private void InitAttributesFilter()
        {
            foreach (var attributeElenent in AttributesElementList)
            {
                InitFilter(attributeElenent);
            }
        }

        private void InitFilter(AttributeElement attributeElement)
        {
            if (attributeElement.Filter == null)
            {
                attributeElement.Filter = new AttributeFilter(GetAttributeFilterType(attributeElement));
                switch (attributeElement.Filter.AttributeFilterType)
                {
                    case AttributeFilterType.Range:
                        InitAttributeFilterRange(attributeElement.Filter, attributeElement.DataType);
                        break;
                    case AttributeFilterType.Text:
                        InitAttributeFilterText(attributeElement.Filter);
                        break;
                    case AttributeFilterType.YesNoOptions:
                        InitAttributeFilterYesNo(attributeElement.Filter);
                        break;
                    case AttributeFilterType.ValueList:
                        InitAttributeFilterValueList(attributeElement.Filter);
                        break;
                    default:
                        break;
                }
            }
        }

        private void LoadAttributesFilterValuesListFromCache()
        {
            if (attributesCache != null)
            {
                LoadProductDimensionAttributesFromCache(AttributeType.DimensionColor, attributesCache.ColorDimensionFilter);
                LoadProductDimensionAttributesFromCache(AttributeType.DimensionConfiguration, attributesCache.ConfigurationDimensionFilter);
                LoadProductDimensionAttributesFromCache(AttributeType.DimensionStyle, attributesCache.StyleDimensionFilter);
                LoadProductDimensionAttributesFromCache(AttributeType.DimensionSize, attributesCache.SizeDimensionFilter);
                LoadProductAttributesValuesListFromCache();
            }
        }

        private void LoadProductDimensionAttributesFromCache(AttributeType attributeType, IList<IProductSearchAttributeFilterValue> dimensionValues)
        {
            if (dimensionValues != null && dimensionValues.Count > 0)
            {
                var attributeElement = this.AttributesElementList.Where(attr => attr.Type == attributeType).First();


                foreach (var searchAttributeFilterValue in dimensionValues)
                {
                    var attributeElementValue = new AttributeElementValue();
                    attributeElementValue.Value = searchAttributeFilterValue.Value;
                    attributeElementValue.ExternalId = searchAttributeFilterValue.Id;
                    attributeElementValue.ValueChecked = false;
                    attributeElement.Filter.AttributeElementValueList.Add(attributeElementValue);
                }
            }
        }

        private void LoadProductAttributesValuesListFromCache()
        {
            foreach (var attributeElement in this.AttributesElementList.Where(attr => attr.Type == AttributeType.ProductAttribute && attr.Filter.AttributeFilterType == AttributeFilterType.ValueList).ToList())
            {
                var attrCache = attributesCache.AttributesFilter.Where(attr => attr.ProductAttributeInfo.Id == attributeElement.ExternalId).FirstOrDefault();
                if (attrCache.ProductSearchAttributeFilter != null && attrCache.ProductSearchAttributeFilter.FilterValuesList != null && attrCache.ProductSearchAttributeFilter.FilterValuesList.Count != 0)
                {
                    foreach (var attrVal in attrCache.ProductSearchAttributeFilter.FilterValuesList)
                    {
                        var attributeElementValue = new AttributeElementValue();
                        attributeElementValue.Value = attrVal.Value;
                        attributeElementValue.ExternalId = attrVal.Id;
                        attributeElementValue.ValueChecked = false;
                        attributeElement.Filter.AttributeElementValueList.Add(attributeElementValue);
                    }
                }
            }
        }

        private void ApplySearchFilter()
        {
            if (searchFilter != null)
            {
                ApplySearchFilterForDimension(AttributeType.DimensionColor);
                ApplySearchFilterForDimension(AttributeType.DimensionConfiguration);
                ApplySearchFilterForDimension(AttributeType.DimensionSize);
                ApplySearchFilterForDimension(AttributeType.DimensionStyle);
                ApplySearchFilterToProductSearchAttributes();
            }
        }

        private void ApplySearchFilterToProductSearchAttributes()
        {
            if (searchFilter != null && searchFilter.AttributesFilter != null && searchFilter.AttributesFilter.Count != 0)
            {
                foreach (var productSearchAttribute in searchFilter.AttributesFilter)
                {
                    var attributeElement = this.AttributesElementList.Where(attr => attr.ExternalId == productSearchAttribute.ProductAttributeInfo.Id).First();
                    switch (attributeElement.Filter.AttributeFilterType)
                    {
                        case AttributeFilterType.Range:
                            if (productSearchAttribute.ProductSearchAttributeFilter.RangeFrom != null)
                            {
                                attributeElement.Filter.RangeFrom.Value = productSearchAttribute.ProductSearchAttributeFilter.RangeFrom.Value;
                            }
                            if (productSearchAttribute.ProductSearchAttributeFilter.RangeTo != null)
                            {
                                attributeElement.Filter.RangeTo.Value = productSearchAttribute.ProductSearchAttributeFilter.RangeTo.Value;
                            }
                            break;
                        case AttributeFilterType.Text:
                            attributeElement.Filter.FilterValue.Value = productSearchAttribute.ProductSearchAttributeFilter.TextFilter;
                            break;
                        case AttributeFilterType.ValueList:
                            ApplySearchFilterToProductAttributesValuesList(attributeElement, productSearchAttribute.ProductSearchAttributeFilter.FilterValuesList);
                            break;
                        case AttributeFilterType.YesNoOptions:
                            attributeElement.Filter.TrueValue.ValueChecked = productSearchAttribute.ProductSearchAttributeFilter.FilterValuesList[0].Value == null ? true : Convert.ToBoolean(productSearchAttribute.ProductSearchAttributeFilter.FilterValuesList[0].Value);
                            attributeElement.Filter.FalseValue.ValueChecked = productSearchAttribute.ProductSearchAttributeFilter.FilterValuesList[0].Value == null ? true : !Convert.ToBoolean(productSearchAttribute.ProductSearchAttributeFilter.FilterValuesList[0].Value);
                            break;
                    }
                }
            }
        }

        private void ApplySearchFilterForDimension(AttributeType attributeType)
        {
            if (searchFilter != null)
            {
                IList<IProductSearchAttributeFilterValue> searchFilterForDimension = null;

                switch (attributeType)
                {
                    case AttributeType.DimensionColor:
                        searchFilterForDimension = searchFilter.ColorDimensionFilter;
                        break;
                    case AttributeType.DimensionSize:
                        searchFilterForDimension = searchFilter.SizeDimensionFilter;
                        break;
                    case AttributeType.DimensionConfiguration:
                        searchFilterForDimension = searchFilter.ConfigurationDimensionFilter;
                        break;
                    case AttributeType.DimensionStyle:
                        searchFilterForDimension = searchFilter.StyleDimensionFilter;
                        break;
                }

                if (searchFilterForDimension != null && searchFilterForDimension.Count != 0)
                {
                    foreach (var attributeValue in searchFilterForDimension)
                    {
                        AttributesElementList.Where(attr => attr.Type == attributeType).First().Filter.AttributeElementValueList.Where(attrValue => attrValue.ExternalId == attributeValue.Id).First().ValueChecked = true;
                    }
                }
            }
        }

        private void ApplySearchFilterToProductAttributesValuesList(AttributeElement attributeElement, IList<IProductSearchAttributeFilterValue> searchFilterValues)
        {
            foreach (var attributeFilter in searchFilterValues)
            {
                attributeElement.Filter.AttributeElementValueList.Where(attrValue => attrValue.Value == attributeFilter.Value).First().ValueChecked = true;
            }
        }

        private void InitAttributeFilterValueList(AttributeFilter attributeFilter)
        {
            attributeFilter.AttributeElementValueList = new List<AttributeElementValue>();
        }

        private void InitAttributeFilterYesNo(AttributeFilter attributeFilter)
        {
            attributeFilter.TrueValue = new AttributeElementValue();
            attributeFilter.TrueValue.Id = 0.ToString();
            attributeFilter.TrueValue.Value = ApplicationLocalizer.Language.Translate(1346); // Yes
            attributeFilter.TrueValue.ValueChecked = false;

            attributeFilter.FalseValue = new AttributeElementValue();
            attributeFilter.FalseValue.Id = 1.ToString();
            attributeFilter.FalseValue.Value = ApplicationLocalizer.Language.Translate(1345); // No
            attributeFilter.FalseValue.ValueChecked = false;
        }

        private void InitAttributeFilterText(AttributeFilter attributeFilter)
        {
            attributeFilter.FilterValue = new AttributeElementValue();
            attributeFilter.FilterValue.Id = 0.ToString();
            attributeFilter.FilterValue.Value = string.Empty;
        }

        private void InitAttributeFilterRange(AttributeFilter attributeFilter, AttributeDataType dataType)
        {
            attributeFilter.RangeFrom = new AttributeElementValue();
            attributeFilter.RangeFrom.Id = 0.ToString();
            if (dataType == AttributeDataType.DateTime)
            {
                attributeFilter.RangeFrom.Value = new DateTime();
            }

            attributeFilter.RangeTo = new AttributeElementValue();
            attributeFilter.RangeTo.Id = 1.ToString();
            if (dataType == AttributeDataType.DateTime)
            {
                attributeFilter.RangeFrom.Value = new DateTime();
            }
        }

        private AttributeFilterType GetAttributeFilterType(AttributeElement attributeElement)
        {
            AttributeFilterType retVal = AttributeFilterType.None;
            switch (attributeElement.Type)
            {
                case AttributeType.DimensionColor:
                case AttributeType.DimensionConfiguration:
                case AttributeType.DimensionSize:
                case AttributeType.DimensionStyle:
                    retVal = AttributeFilterType.ValueList;
                    break;
                case AttributeType.ProductAttribute:
                    retVal = GetProductAttributeFilterType(attributeElement.DataType, attributeElement.IsEnumerable);
                    break;
                default:
                    retVal = AttributeFilterType.None;
                    break;
            }
            return retVal;
        }

        private AttributeFilterType GetProductAttributeFilterType(AttributeDataType dataType, bool isEnumerable)
        {
            AttributeFilterType retVal;
            switch (dataType)
            {
                case AttributeDataType.Currency:
                case AttributeDataType.DateTime:
                case AttributeDataType.Decimal:
                case AttributeDataType.Integer:
                    retVal = AttributeFilterType.Range;
                    break;
                case AttributeDataType.Text:
                    retVal = isEnumerable ? AttributeFilterType.ValueList : AttributeFilterType.Text;
                    break;
                case AttributeDataType.Boolean:
                    retVal = AttributeFilterType.YesNoOptions;
                    break;
                default:
                    retVal = AttributeFilterType.None;
                    break;
            }
            return retVal;
        }

        /// <summary>
        /// Creates list of filter values from specififed attribute filter.
        /// </summary>
        /// <param name="attributeFilter">Attribute filter.</param>
        /// <returns>List of filter values.</returns>
        internal List<object> GetAllFiltersObjects(AttributeFilter attributeFilter)
        {
            var retVal = new List<object>();
            retVal.Add(attributeFilter);
            retVal.Add(attributeFilter.RangeFrom);
            retVal.Add(attributeFilter.RangeTo);
            retVal.Add(attributeFilter.FalseValue);
            retVal.Add(attributeFilter.TrueValue);
            retVal.Add(attributeFilter.FilterValue);
            if (attributeFilter.AttributeElementValueList != null)
            {
                retVal.AddRange(attributeFilter.AttributeElementValueList);
            }
            return retVal;
        }

        /// <summary>
        /// Save attributes and loaded list of values for filters with type ValueList.
        /// </summary>
        /// <returns></returns>
        public IProductSearchFilter SaveSearchAttrbutesCache()
        {
            var productSearchAttributesCache = searchFilter.CreateProductSearchFilter();
            foreach (var attributeElement in this.AttributesElementList)
            {
                switch (attributeElement.Type)
                {
                    case AttributeType.DimensionColor:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList, productSearchAttributesCache.ColorDimensionFilter, searchFilter);
                        break;
                    case AttributeType.DimensionConfiguration:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList, productSearchAttributesCache.ConfigurationDimensionFilter, searchFilter);
                        break;
                    case AttributeType.DimensionSize:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList, productSearchAttributesCache.SizeDimensionFilter, searchFilter);
                        break;
                    case AttributeType.DimensionStyle:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList, productSearchAttributesCache.StyleDimensionFilter, searchFilter);
                        break;
                    case AttributeType.ProductAttribute:
                            var productAttribute = CreateProductAttributeInfo(attributeElement, searchFilter);
                            var productSearchAttributeFilter = searchFilter.CreateProductSearchAttributeFilter();
                            if (attributeElement.Filter.AttributeFilterType == AttributeFilterType.ValueList)
                            {
                                SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList, productSearchAttributeFilter.FilterValuesList, searchFilter);
                            }
                            productSearchAttributesCache.AttributesFilter.Add(productSearchAttributesCache.CreateAttributeFilterEntry(productAttribute, productSearchAttributeFilter));
                        break;
                    default:
                        break;
                }
            }
            return productSearchAttributesCache;
        }

        private IProductAttributeInfo CreateProductAttributeInfo(AttributeElement attributeElement, IProductSearchFilter productSearchFilter)
        {
            var attributeInfo = productSearchFilter.CreateProductAttributeInfo();
            attributeInfo.DataType = attributeElement.DataType;
            attributeInfo.IsEnumeration = attributeElement.IsEnumerable;
            attributeInfo.Name = attributeElement.Name;
            attributeInfo.Id = attributeElement.ExternalId;
            return attributeInfo;
        }

        private void SaveAttributesElementValuesList(IEnumerable<AttributeElementValue> attributeElementValueList, IList<IProductSearchAttributeFilterValue> productSearchAttributeFilterValuesList, IProductSearchFilter searchFilterCreator)
        {
            foreach (var attributeElement in attributeElementValueList)
            {
                var searchFilterValue = searchFilterCreator.CreateProductSearchAttributeFilterValue();
                searchFilterValue.Id = attributeElement.ExternalId;
                searchFilterValue.Value = attributeElement.Value;
                productSearchAttributeFilterValuesList.Add(searchFilterValue);
            }
        }

        /// <summary>
        /// Save search filter.
        /// </summary>
        /// <returns></returns>
        public IProductSearchFilter SaveSearchFilter()
        {
            var productSearchFilter = searchFilter.CreateProductSearchFilter();
            productSearchFilter.CategoryId = this.categoryId;
            productSearchFilter.OrderBy = searchFilter.OrderBy;

            foreach (var attributeElement in this.AttributesElementList)
            {
                switch (attributeElement.Type)
                {
                    case AttributeType.DimensionColor:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList.Where(attr => attr.ValueChecked), 
                            productSearchFilter.ColorDimensionFilter, searchFilter);
                        break;
                    case AttributeType.DimensionConfiguration:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList.Where(attr => attr.ValueChecked), 
                            productSearchFilter.ConfigurationDimensionFilter, searchFilter);
                        break;
                    case AttributeType.DimensionSize:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList.Where(attr => attr.ValueChecked),
                            productSearchFilter.SizeDimensionFilter, searchFilter);
                        break;
                    case AttributeType.DimensionStyle:
                        SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList.Where(attr => attr.ValueChecked), 
                            productSearchFilter.StyleDimensionFilter, searchFilter);
                        break;
                    case AttributeType.ProductAttribute:
                        var productAttribute = CreateProductAttributeInfo(attributeElement, searchFilter);
                        var productSearchAttributeFilter = searchFilter.CreateProductSearchAttributeFilter();
                        switch (attributeElement.Filter.AttributeFilterType)
                        {
                            case AttributeFilterType.ValueList:
                                SaveAttributesElementValuesList(attributeElement.Filter.AttributeElementValueList.Where(attr => attr.ValueChecked), 
                                    productSearchAttributeFilter.FilterValuesList, searchFilter);
                                if (productSearchAttributeFilter.FilterValuesList.Count > 0)
                                {
                                    productSearchFilter.AttributesFilter.Add(productSearchFilter.CreateAttributeFilterEntry(productAttribute, productSearchAttributeFilter));
                                }
                                break;
                            case AttributeFilterType.Range:
                                if (!IsAttributeValueEmpty(attributeElement.Filter.RangeFrom) || !IsAttributeValueEmpty(attributeElement.Filter.RangeTo))
                                {
                                    SaveAttributesElementValuesRange(attributeElement.Filter, productSearchAttributeFilter, searchFilter);
                                    productSearchFilter.AttributesFilter.Add(productSearchFilter.CreateAttributeFilterEntry(productAttribute, productSearchAttributeFilter));
                                }
                                break;
                            case AttributeFilterType.Text:
                                if (!string.IsNullOrEmpty(attributeElement.Filter.FilterValue.Value.ToString()))
                                {
                                    productSearchAttributeFilter.TextFilter = attributeElement.Filter.FilterValue.Value.ToString();
                                    productSearchFilter.AttributesFilter.Add(productSearchFilter.CreateAttributeFilterEntry(productAttribute, productSearchAttributeFilter));
                                }
                                break;
                            case AttributeFilterType.YesNoOptions:
                                if (attributeElement.Filter.TrueValue.ValueChecked || attributeElement.Filter.FalseValue.ValueChecked)
                                {
                                    SaveAttributesElementValuesYesNo(attributeElement.Filter, productSearchAttributeFilter, searchFilter);
                                    productSearchFilter.AttributesFilter.Add(productSearchFilter.CreateAttributeFilterEntry(productAttribute, productSearchAttributeFilter));
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            return productSearchFilter;
        }

        private bool IsAttributeValueEmpty(AttributeElementValue attributeValue)
        {
            bool result = attributeValue == null || attributeValue.Value == null || string.IsNullOrEmpty(attributeValue.Value.ToString());
            if (!result)
            {
                if (attributeValue.Value.GetType() == typeof(DateTime))
                {
                    DateTime dtValue = (DateTime) attributeValue.Value;
                    result = dtValue == LSRetailPosis.DevUtilities.Utility.POSNODATE || dtValue == DateTime.MinValue;
                }
            }
            return result;
                    
        }
        private void SaveAttributesElementValuesRange(AttributeFilter attributeFilter, IProductSearchAttributeFilter productSearchAttributeFilter, IProductSearchFilter searchFilterCreator)
        {
            if (!IsAttributeValueEmpty(attributeFilter.RangeFrom))
            {
                var filterFromValue = searchFilterCreator.CreateProductSearchAttributeFilterValue();
                filterFromValue.Value = attributeFilter.RangeFrom.Value;
                productSearchAttributeFilter.RangeFrom = filterFromValue;
            }

            if (!IsAttributeValueEmpty(attributeFilter.RangeTo))
            {
                var filterToValue = searchFilterCreator.CreateProductSearchAttributeFilterValue();
                filterToValue.Value = attributeFilter.RangeTo.Value;
                productSearchAttributeFilter.RangeTo = filterToValue;
            }

        }

        private void SaveAttributesElementValuesYesNo(AttributeFilter attributeFilter, IProductSearchAttributeFilter productSearchAttributeFilter, IProductSearchFilter searchFilterCreator)
        {
            var filterFromValue = searchFilterCreator.CreateProductSearchAttributeFilterValue();
            if (attributeFilter.TrueValue.ValueChecked && attributeFilter.FalseValue.ValueChecked)
            {
                filterFromValue.Value = null;
            }
            else
            {
                filterFromValue.Value = attributeFilter.TrueValue.ValueChecked;
            }
            productSearchAttributeFilter.FilterValuesList.Add(filterFromValue);
        }
    }

    /// <summary>
    /// Search attribute filter.
    /// </summary>
    internal class AttributeFilter
    {
        /// <summary>
        /// Type of the filter.
        /// </summary>
        public AttributeFilterType AttributeFilterType { get; private set; }

        /// <summary>
        /// List of values.
        /// </summary>
        public List<AttributeElementValue> AttributeElementValueList { get; set; }

        /// <summary>
        /// Constructor for attribute filter.
        /// </summary>
        /// <param name="attributeElementType">Type of the filter.</param>
        public AttributeFilter(AttributeFilterType filterType)
        {
            this.AttributeFilterType = filterType;
        }

        /// <summary>
        /// Range from value.
        /// </summary>
        public AttributeElementValue RangeFrom { get; set; }

        /// <summary>
        /// Range to value.
        /// </summary>
        public AttributeElementValue RangeTo { get; set; }

        /// <summary>
        // Text to search.
        /// </summary>
        public AttributeElementValue FilterValue { get; set; }

        /// <summary>
        /// True value.
        /// </summary>
        public AttributeElementValue TrueValue { get; set; }

        /// <summary>
        /// False value.
        /// </summary>
        public AttributeElementValue FalseValue { get; set; }
    }
    /// <summary>
    /// Search attribute element value.
    /// </summary>
    internal class AttributeElementValue
    {
        /// <summary>
        /// Id of the search attribute element value.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// External id of the search attribute element value.
        /// </summary>
        public long ExternalId { get; set; }

        /// <summary>
        /// Value of the search attribute element value. This value is displaying to user
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Indicated if this attribute value was check, is used for check buttons.
        /// </summary>
        public bool ValueChecked { get; set; }
    }

    /// <summary>
    /// Type of the search attributes filter
    /// </summary>
    internal enum AttributeFilterType
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Text filter.
        /// </summary>
        Text = 1,

        /// <summary>
        /// Range.
        /// </summary>
        Range = 2,

        /// <summary>
        /// List of values to select.
        /// </summary>
        ValueList = 3,

        /// <summary>
        /// Yes/No options to select.
        /// </summary>
        YesNoOptions = 4
    }

    /// <summary>
    /// Type of the search attribute.
    /// </summary>
    internal enum AttributeType
    {
        /// <summary>
        /// Product color dimension.
        /// </summary>
        DimensionColor = 0,

        /// <summary>
        /// Product size dimension.
        /// </summary>
        DimensionSize = 1,

        /// <summary>
        /// Product style dimension.
        /// </summary>
        DimensionStyle = 2,

        /// <summary>
        /// Product configuration dimension.
        /// </summary>
        DimensionConfiguration = 3,

        /// <summary>
        /// Product attribute.
        /// </summary>
        ProductAttribute = 4,

        /// <summary>
        /// For clients/partners to store theirs custom data
        /// </summary>
        PartnerData = 5
    }

    /// <summary>
    /// Search attribute element
    /// </summary>
    internal class AttributeElement
    {
        /// <summary>
        /// Id of the search attribute element value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// External id of the search attribute element value.
        /// </summary>
        public long ExternalId { get; set; }

        /// <summary>
        /// Name of  the attribute to display.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the attribute.
        /// </summary>
        public AttributeType Type { get; set; }

        /// <summary>
        /// Type of value
        /// </summary>
        public AttributeDataType DataType { get; set; }

        /// <summary>
        /// Indicated if this attribute value was check, is used for check buttons.
        /// </summary>
        public bool ValueChecked { get; set; }


        /// <summary>
        /// Filter of the search attribute
        /// </summary>
        public AttributeFilter Filter { get; set; }

        /// <summary>
        /// Indicated that attributes values are predifined list.
        /// </summary>
        public bool IsEnumerable { get; set; }
    }
}