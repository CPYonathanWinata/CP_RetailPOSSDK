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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.ProductSearchAttributes
{
    /// <summary>
    /// Static class containing method related to search product by different attributes
    /// </summary>
    internal static class ProductSearcher
    {
        #region Sql statement
        private const string searchProductSql = @"
DECLARE @FOUNDPRODUCTS TABLE (PRODUCT BIGINT)
DECLARE @PRODUCTS  [crt].[RECORDIDTABLETYPE]
IF LEN(@SEARCHVALUE) <> 0
BEGIN
	INSERT @FOUNDPRODUCTS
	EXEC dbo.GETPRODUCTSBYITEMID @SEARCHVALUE
	INSERT @FOUNDPRODUCTS
	EXEC dbo.GETPRODUCTSBYSEARCHNAME @SEARCHVALUE
	INSERT @FOUNDPRODUCTS
	EXEC dbo.GETPRODUCTSBYNAME @SEARCHVALUE, @CULTUREID
END

    DECLARE @tvp_ProductIds [crt].[RECORDIDTABLETYPE]

	INSERT INTO @tvp_ProductIds (RECID)
	SELECT DISTINCT it.PRODUCT
	FROM INVENTTABLE it 
		JOIN INVENTTABLEMODULE itm ON it.ITEMID = itm.ITEMID AND itm.MODULETYPE = 2 AND it.DATAAREAID = itm.DATAAREAID 
		JOIN ECORESPRODUCTCATEGORY erpc ON erpc.PRODUCT = it.PRODUCT 
		JOIN RETAILCATEGORYCONTAINMENTLOOKUP rccl ON rccl.CONTAINEDCATEGORY = erpc.CATEGORY  
	WHERE 
		(LEN(@SEARCHVALUE) = 0 OR
		EXISTS (SELECT P.PRODUCT FROM @FOUNDPRODUCTS p WHERE P.PRODUCT = it.PRODUCT)) 
		AND rccl.CATEGORY = @CATEGORY 
		AND it.DATAAREAID = @DATAAREAID

	INSERT INTO @PRODUCTS (RECID)
	SELECT DISTINCT PRODUCTID AS PRODUCT
	FROM [crt].GETASSORTEDPRODUCTS(@STORERECID, @STOREDATE, 0, 1, 1, @tvp_ProductIds) ap 
    {0}

EXEC [dbo].[GETPRODUCTSBYRECID] @PRODUCTS, @ORDERBY, @CULTUREID, @STORERECID, @DATAAREAID, @ASCENDING, @FROMROW, @PAGESIZE
                ";
        #endregion

        #region Private methods

        private static DataTable SearchProductsInCategory(long categoryId, int rowFrom, int rowsCount,  ProductSearchCriteria productCriteria)
        {
            if (productCriteria == null)
            {
                throw new ArgumentNullException("productCriteria");
            }

            if (categoryId == 0)
            {
                throw new ArgumentException("categoryId");
            }
            if (rowFrom < 0)
            {
                throw new ArgumentOutOfRangeException("rowFrom");
            }
            if (rowsCount <= 0)
            {
                throw new ArgumentOutOfRangeException("rowsCount");
            }

            List<SqlParameter> parameters = new List<SqlParameter>(
                new SqlParameter[]
                {
                    new SqlParameter("@SEARCHVALUE", productCriteria.ProductName),
                    new SqlParameter("@STORERECID", productCriteria.ChannelId),
                    new SqlParameter("@STOREDATE", DateTime.Today),
                    new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
                    new SqlParameter("@CATEGORY", categoryId),
                    new SqlParameter("@CULTUREID", productCriteria.LanguageId),
                    new SqlParameter("@ORDERBY", productCriteria.OrderBy),
                    new SqlParameter("@ASCENDING", productCriteria.SortAscending),
                    new SqlParameter("@FROMROW", rowFrom),
                    new SqlParameter("@PAGESIZE", rowsCount)
                }
            );

            var sqlCondition = productCriteria.generateProductCriteria();
            parameters.AddRange(sqlCondition.SqlParameters);

            var sqlBuilder = new StringBuilder(sqlCondition.SqlExpression);
            if (sqlBuilder.Length > 0)
            {
                sqlBuilder.Insert(0, " WHERE ");
            }
            string sql = string.Format(searchProductSql, sqlBuilder.ToString());

            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var dataTable = dbUtil.GetTable(sql, parameters.ToArray());

            return dataTable;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Searches for products satisfied the filter.
        /// </summary>
        /// <param name="productSearchFilter">Product search filter.</param>
        /// <returns>Table with results.</returns>
        public static DataTable SearchProducts(IProductSearchFilter productSearchFilter)
        {
            if (productSearchFilter == null)
            {
                throw new ArgumentNullException("productCriteria");
            }

            return SearchProductsInCategory(productSearchFilter.CategoryId, productSearchFilter.StartRow, productSearchFilter.RowsCount,
                            new ProductSearchCriteria(productSearchFilter));
        }

        #endregion

        #region Subsidiary classes

        /// <summary>
        /// Class for generation where clause  in SQL statement for searching products.
        /// </summary>
        internal class ProductSearchCriteria
        {
            #region Private fields and constants

            private const string productRecIDField = "ap.PRODUCTID";
            SqlParameterNameGenerator sqlParameterNameGenerator;
            private IList<Criterion> criteria;
            private InventDimCompositeCriterion inventDimCompositeCriterion;

            #endregion

            #region Constructor

            /// <summary>
            /// Constructs instance of the class using IProductSearchFilter to init values.
            /// </summary>
            /// <param name="productSearchFilter">Product search filter.</param>
            public ProductSearchCriteria(IProductSearchFilter productSearchFilter)
            {
                if (productSearchFilter == null)
                {
                    throw new ArgumentNullException("productSearchFilter");
                }

                sqlParameterNameGenerator = new SqlParameterNameGenerator();
                criteria = new List<Criterion>();
                inventDimCompositeCriterion = new InventDimCompositeCriterion(sqlParameterNameGenerator, productRecIDField);

                ProductName = productSearchFilter.SearchValue;
                ChannelId = productSearchFilter.ChannelId;
                OrderBy = productSearchFilter.OrderBy;
                SortAscending = productSearchFilter.SortAscending;
                LanguageId = productSearchFilter.LanguageId;

                fillFromProductSearchFilter(productSearchFilter);
            }

            #endregion

            #region Properties

            /// <summary>
            /// Product name. 
            /// </summary>
            public string ProductName { get; private set; }

            /// <summary>
            /// Order for returned records.
            /// </summary>
            public string OrderBy { get; private set; }

            /// <summary>
            /// Search ascending.
            /// </summary>
            public bool SortAscending { get; private set; }

            /// <summary>
            /// Store language.
            /// </summary>
            public string LanguageId { get; private set; }

            /// <summary>
            /// Channel record id.
            /// </summary>
            public long ChannelId { get; private set; }

            #endregion

            #region Private methods

            private void fillFromProductSearchFilter(IProductSearchFilter productSearchFilter)
            {
                if (productSearchFilter.ColorDimensionFilter != null && productSearchFilter.ColorDimensionFilter.Count > 0)
                {
                    this.AddColorCondition(productSearchFilter.ColorDimensionFilter.Select(color => color.Id));
                }

                if (productSearchFilter.ConfigurationDimensionFilter != null && productSearchFilter.ConfigurationDimensionFilter.Count > 0)
                {
                    this.AddConfigurationCondition(productSearchFilter.ConfigurationDimensionFilter.Select(config => config.Id));
                }

                if (productSearchFilter.SizeDimensionFilter != null && productSearchFilter.SizeDimensionFilter.Count > 0)
                {
                    this.AddSizeCondition(productSearchFilter.SizeDimensionFilter.Select(size => size.Id));
                }

                if (productSearchFilter.StyleDimensionFilter != null && productSearchFilter.StyleDimensionFilter.Count > 0)
                {
                    this.AddStyleCondition(productSearchFilter.StyleDimensionFilter.Select(style => style.Id));
                }

                foreach (var attributeItem in productSearchFilter.AttributesFilter)
                {
                    AddAttributeFilter(attributeItem);
                }
            }

            private T Convert<T>(object value) where T : struct
            {
                string stringVal = null;
                if (value.GetType() == typeof(string))
                {
                    stringVal = (string)value;
                    if (typeof(T) == typeof(decimal)) 
                    {
                        return (T)(object)decimal.Parse(stringVal);
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)(object)(int)decimal.Parse(stringVal);
                    }
                    else
                    {
                        throw new ArgumentException("value");
                    }
                }
                return (T)value;
            }
            
            private T? Val<T>(IProductSearchAttributeFilterValue filterValue) where T : struct
            {
                T? value = null;
                if (filterValue != null && filterValue.Value != null)
                {
                    value = Convert<T>(filterValue.Value);
                }
                return value;
            }

            private void AddAttributeFilter(IAttributeFilterEntry attributeItem)
            {
                var attribute = attributeItem.ProductAttributeInfo;
                var attributeFilter = attributeItem.ProductSearchAttributeFilter;
                switch (attribute.DataType)
                {
                    case AttributeDataType.Boolean:
                        if (attributeFilter.FilterValuesList.Count != 1)
                        {
                            throw new ArgumentException("attributeFilter.FilterValuesList should contain 1 element with bool? type");
                        }
                        this.AddBooleanAttributeValueCondition(attribute.Id, (bool?)attributeFilter.FilterValuesList[0].Value);
                        break;

                    case AttributeDataType.Currency:
                        this.AddCurrencyAttributeValueCondition(attribute.Id, Val<decimal>(attributeFilter.RangeFrom), Val<decimal>(attributeFilter.RangeTo));
                        break;

                    case AttributeDataType.DateTime:
                        this.AddDateTimeAttributeValueCondition(attribute.Id, Val<DateTime>(attributeFilter.RangeFrom), Val<DateTime>(attributeFilter.RangeTo));
                        break;

                    case AttributeDataType.Decimal:
                        this.AddFloatAttributeValueCondition(attribute.Id, Val<decimal>(attributeFilter.RangeFrom), Val<decimal>(attributeFilter.RangeTo));
                        break;

                    case AttributeDataType.Integer:
                        this.AddIntAttributeValueCondition(attribute.Id, Val<int>(attributeFilter.RangeFrom), Val<int>(attributeFilter.RangeTo));
                        break;

                    case AttributeDataType.Text:
                        if (attribute.IsEnumeration)
                        {
                            if (attributeFilter.FilterValuesList != null && attributeFilter.FilterValuesList.Count > 0)
                            {
                                this.AddTextListAttributeValueCondition(attribute.Id, attributeFilter.FilterValuesList.Select(attr => (string)attr.Value));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(attributeFilter.TextFilter))
                            {
                                this.AddTextLikeAttributeValueCondition(attribute.Id, attributeFilter.TextFilter);
                            }
                        }
                        break;

                    default:
                        throw new ArgumentException(string.Format("AttributeDataType '{0}' is not supported", attribute.DataType.ToString()));

                }
            }

            private void AddColorCondition(IEnumerable<long> colorRecIds)
            {
                inventDimCompositeCriterion.AddColors(colorRecIds);
            }

            private void AddConfigurationCondition(IEnumerable<long> configurationRecIds)
            {
                inventDimCompositeCriterion.AddConfigurations(configurationRecIds);
            }

            private void AddSizeCondition(IEnumerable<long> sizeRecIds)
            {
                inventDimCompositeCriterion.AddSizes(sizeRecIds);
            }

            private void AddStyleCondition(IEnumerable<long> styleRecIds)
            {
                inventDimCompositeCriterion.AddStyles(styleRecIds);
            }

            private void AddIntAttributeValueCondition(long attributeRecId, int? valueFrom, int? valueTo)
            {
                criteria.Add(new AtrributeIntValueCriterion(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIDField));
            }

            private void AddFloatAttributeValueCondition(long attributeRecId, decimal? valueFrom, decimal? valueTo)
            {
                criteria.Add(new AtrributeFloatValueCriterion(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIDField));
            }

            private void AddCurrencyAttributeValueCondition(long attributeRecId, decimal? valueFrom, decimal? valueTo)
            {
                criteria.Add(new AtrributeCurrencyValueCriterion(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIDField));
            }

            private void AddDateTimeAttributeValueCondition(long attributeRecId, DateTime? valueFrom, DateTime? valueTo)
            {
                criteria.Add(new AtrributeDateTimeValueCriterion(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIDField));
            }

            private void AddBooleanAttributeValueCondition(long attributeRecId, bool? value)
            {
                criteria.Add(new AtrributeBooleanValueCriterion(attributeRecId, value, sqlParameterNameGenerator, productRecIDField));
            }

            private void AddTextLikeAttributeValueCondition(long attributeRecId, string value)
            {
                criteria.Add(new AtrributeTextLikeValueCriterion(attributeRecId, value, LanguageId, sqlParameterNameGenerator, productRecIDField));
            }

            private void AddTextListAttributeValueCondition(long attributeRecId, IEnumerable<string> values)
            {
                criteria.Add(new AtrributeTextListValueCriterion(attributeRecId, values, LanguageId, sqlParameterNameGenerator, productRecIDField));
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Generates product search criteria and returns result/>
            /// </summary>
            /// <returns>product search criteria as <typeparamref name="SqlCondition"</returns>
            public SqlCondition generateProductCriteria()
            {
                bool hasCriteria = false;
                StringBuilder sb = new StringBuilder();
                string whereClause = string.Empty;
                List<SqlParameter> parameters = new List<SqlParameter>();

                sb.AppendLine();

                if (!inventDimCompositeCriterion.IsEmpty)
                {
                    criteria.Add(inventDimCompositeCriterion);
                }

                foreach (var criterion in criteria)
                {
                    var sqlCondition = criterion.generateSQLCondition();
                    if (hasCriteria)
                    {
                        sb.AppendLine();
                        sb.AppendLine(" AND");
                    }
                    sb.AppendLine();
                    sb.Append("( ");
                    sb.Append(sqlCondition.SqlExpression);
                    sb.Append(" ) ");
                    parameters.AddRange(sqlCondition.SqlParameters);
                    hasCriteria = true;
                }

                if (hasCriteria)
                {
                    whereClause = sb.ToString();
                }

                return new SqlCondition(whereClause, parameters);
            }

            #endregion
        }

        /// <summary>
        /// Class for storing SQL statement and its parameters.
        /// </summary>
        internal class SqlCondition
        {
            /// <summary>
            /// Costruct instance of the class.
            /// </summary>
            /// <param name="sql">Sql expression.</param>
            /// <param name="parameters">List of sql parameters.</param>
            public SqlCondition(string sqlExpression, IEnumerable<SqlParameter> parameters)
            {
                SqlExpression = sqlExpression;
                this.SqlParameters = parameters;
            }

            /// <summary>
            /// Costruct instance of the class with empty parameters list.
            /// </summary>
            /// <param name="sql">Sql expression.</param>
            public SqlCondition(string sql)
                : this(sql, emptyParamList)
            {

            }

            /// <summary>
            /// Costruct instance of the class with one parameter.
            /// </summary>
            /// <param name="sql">Sql expression.</param>
            /// <param name="parameters">Sql parameter.</param>
            public SqlCondition(string sql, SqlParameter parameter)
                : this(sql, new SqlParameter[] { parameter })
            {

            }

            /// <summary>
            /// Sql expression.
            /// </summary>
            public string SqlExpression { get; private set; }

            /// <summary>
            /// List of sql parameters.
            /// </summary>
            public IEnumerable<SqlParameter> SqlParameters { get; private set; }

            private static IEnumerable<SqlParameter> emptyParamList = new SqlParameter[] { };

            private static SqlCondition empty = new SqlCondition(string.Empty);

            /// <summary>
            /// Instance of the empty class.
            /// </summary>
            public static SqlCondition Empty { get { return empty; } }

            /// <summary>
            /// Validates if instance of the class is empty.
            /// </summary>
            public static bool IsEmpty(SqlCondition sqlCondition)
            {
                return sqlCondition == null || string.IsNullOrWhiteSpace(sqlCondition.SqlExpression);
            }
        }

        /// <summary>
        /// Class for generation unique name of sql parameter in sql statement.
        /// </summary>
        internal class SqlParameterNameGenerator
        {
            private int counter = 0;

            /// <summary>
            /// Generates unique parameter name.
            /// </summary>
            /// <returns></returns>
            public string generateParameterName()
            {
                return string.Format("@param{0}", ++counter);
            }
        }

        /// <summary>
        /// Abstract class which represents one criterion for searching products.
        /// </summary>
        internal abstract class Criterion
        {
            /// <summary>
            /// Sql parameters name generator.
            /// </summary>
            protected SqlParameterNameGenerator sqlParameterNameGenerator { get; set; }

            /// <summary>
            /// Product record id fiels name.
            /// </summary>
            protected string productRecIdField { get; set; }

            /// <summary>
            /// Constucts instance of the class.
            /// </summary>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            protected Criterion(SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
            {
                this.sqlParameterNameGenerator = sqlParameterNameGenerator;
                this.productRecIdField = productRecIdField;
            }

            /// <summary>
            /// Generates sql condition for the criterion.
            /// </summary>
            /// <returns></returns>
            abstract public SqlCondition generateSQLCondition();
        }

        
        /// <summary>
        /// Abstract class which represents an inventory dimension criterion (color, configuration, size and style) for searching products.
        /// </summary>
        internal abstract class InventDimCriterion : Criterion
        {
            /// <summary>
            /// Invent dimension record id.
            /// </summary>
            protected IEnumerable<long> inventDimRecIds;

            /// <summary>
            /// Constructs instance of the class.
            /// </summary>
            /// <param name="inventDimRecIds">List of inventory dimension.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            protected InventDimCriterion(IEnumerable<long> inventDimRecIds, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
                : base(sqlParameterNameGenerator, productRecIdField)
            {
                this.inventDimRecIds = inventDimRecIds;
            }

            public override SqlCondition generateSQLCondition()
            {
                const string sqlTemplate = @"
                INNER JOIN {0} er{2} ON er{2}.NAME = invdim.{1} AND er{2}.RECID in ({3})
	            INNER JOIN {4} erpm{2} ON erpm{2}.{5} = er{2}.RECID
                ";

                StringBuilder argListBuilder = new StringBuilder();
                var sqlParameters = new List<SqlParameter>();
                string sql = string.Empty;

                if (inventDimRecIds != null)
                {
                    bool hasCondition = false;
                    foreach (long inventDimRecId in inventDimRecIds)
                    {
                        string inventDimValueParam = sqlParameterNameGenerator.generateParameterName();
                        if (hasCondition)
                        {
                            argListBuilder.Append(" , ");
                        }
                        argListBuilder.Append(inventDimValueParam);
                        sqlParameters.Add(new SqlParameter(inventDimValueParam, inventDimRecId));
                        hasCondition = true;
                    }
                    if (hasCondition)
                    {
                        sql = string.Format(sqlTemplate, DimensionTable, InventDimDimensionField, TableAliasSuffix, argListBuilder.ToString(), ProductMasterTable, ProductMasterInventDimField);
                    }
                }

                return new SqlCondition(sql, sqlParameters.ToArray());
            }

            /// <summary>
            /// Product master invent dimension field.
            /// </summary>
            protected abstract string ProductMasterInventDimField { get; }
            /// <summary>
            /// Product master product field.
            /// </summary>
            protected abstract string ProductMasterProductField { get; }
            /// <summary>
            /// Product master table.
            /// </summary>
            protected abstract string ProductMasterTable { get; }
            /// <summary>
            /// Dimension table.
            /// </summary>
            protected abstract string DimensionTable { get; }
            /// <summary>
            /// InventDim dimension filed name {CONFIGID, INVENTCOLORID, INVENTSIZEID, INVENTSTYLEID}.
            /// </summary>
            protected abstract string InventDimDimensionField { get; }

            /// <summary>
            /// Table alias suffix.
            /// </summary>
            protected abstract string TableAliasSuffix { get; }
        }

        /// <summary>
        /// Сlass which represents a color criterion for searching products.
        /// </summary>
        internal class ColorCriterion : InventDimCriterion
        {
            /// <summary>
            /// Constructs instance of the class.
            /// </summary>
            /// <param name="colors">List of colors.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public ColorCriterion(IEnumerable<long> colors, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
                : base(colors, sqlParameterNameGenerator, productRecIdField)
            { }

            protected override string ProductMasterInventDimField
            {
                get { return "COLOR"; }
            }
            protected override string ProductMasterProductField
            {
                get { return "COLORPRODUCTMASTER"; }
            }
            protected override string ProductMasterTable
            {
                get { return "[AX].[ECORESPRODUCTMASTERCOLOR]"; }
            }
            protected override string DimensionTable
            {
                get { return "[AX].[ECORESCOLOR]"; }
            }
            protected override string InventDimDimensionField
            {
                get { return "INVENTCOLORID"; }
            }

            protected override string TableAliasSuffix
            {
                get { return "cl"; }
            }
        }

        /// <summary>
        /// Сlass which represents a configuration criterion for searching products.
        /// </summary>
        internal class ConfigurationCriterion : InventDimCriterion
        {
            /// <summary>
            /// Constructs instance of the class.
            /// </summary>
            /// <param name="configurations">List of configurations.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public ConfigurationCriterion(IEnumerable<long> configurations, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
                : base(configurations, sqlParameterNameGenerator, productRecIdField)
            { }

            protected override string ProductMasterInventDimField
            {
                get { return "CONFIGURATION"; }
            }
            protected override string ProductMasterProductField
            {
                get { return "CONFIGPRODUCTMASTER"; }
            }
            protected override string ProductMasterTable
            {
                get { return "[AX].[ECORESPRODUCTMASTERCONFIGURATION]"; }
            }
            protected override string DimensionTable
            {
                get { return "[AX].[ECORESCONFIGURATION]"; }
            }
            protected override string InventDimDimensionField
            {
                get { return "CONFIGID"; }
            }

            protected override string TableAliasSuffix
            {
                get { return "cfg"; }
            }
        }

        /// <summary>
        /// Сlass which represents a size criterion for searching products.
        /// </summary>
        internal class SizeCriterion : InventDimCriterion
        {
            /// <summary>
            /// Constructs instance of the class.
            /// </summary>
            /// <param name="sizes">List of sizes.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public SizeCriterion(IEnumerable<long> sizes, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
                : base(sizes, sqlParameterNameGenerator, productRecIdField)
            { }

            protected override string ProductMasterInventDimField
            {
                get { return "SIZE"; }
            }
            protected override string ProductMasterProductField
            {
                get { return "SIZEPRODUCTMASTER"; }
            }
            protected override string ProductMasterTable
            {
                get { return "[AX].[ECORESPRODUCTMASTERSIZE]"; }
            }
            protected override string DimensionTable
            {
                get { return "[AX].[ECORESSIZE]"; }
            }
            protected override string InventDimDimensionField
            {
                get { return "INVENTSIZEID"; }
            }
            protected override string TableAliasSuffix
            {
                get { return "sz"; }
            }
        }

        /// <summary>
        /// Сlass which represents a size criterion for searching products.
        /// </summary>
        internal class StyleCriterion : InventDimCriterion
        {
            /// <summary>
            /// Constructs instance of the class.
            /// </summary>
            /// <param name="styles">List of styles.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public StyleCriterion(IEnumerable<long> styles, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
                : base(styles, sqlParameterNameGenerator, productRecIdField)
            { }

            protected override string ProductMasterInventDimField
            {
                get { return "STYLE"; }
            }
            protected override string ProductMasterProductField
            {
                get { return "STYLEPRODUCTMASTER"; }
            }
            protected override string ProductMasterTable
            {
                get { return "[AX].[ECORESPRODUCTMASTERSTYLE]"; }
            }
            protected override string DimensionTable
            {
                get { return "[AX].[ECORESSTYLE]"; }
            }
            protected override string InventDimDimensionField
            {
                get { return "INVENTSTYLEID"; }
            }
            protected override string TableAliasSuffix
            {
                get { return "st"; }
            }
        }

        /// <summary>
        /// Сlass which represents a invent dimension composite criterion for searching products.
        /// </summary>
        internal class InventDimCompositeCriterion: Criterion
        {

            private const string sqlTemplate = @"
            EXISTS (
	            SELECT  idc.RECID FROM INVENTDIMCOMBINATION idc
	            INNER JOIN INVENTDIM invdim ON invdim.INVENTDIMID = idc.INVENTDIMID AND invdim.DATAAREAID = idc.DATAAREAID
                {0}
                WHERE idc.ITEMID = ap.ITEMID AND idc.DATAAREAID = @DATAAREAID 
		            AND (idc.DISTINCTPRODUCTVARIANT = ap.VARIANTID OR ap.VARIANTID = 0)            
            )
            ";

            private List<long> configList = new List<long>();
            private List<long> colorList = new List<long>();
            private List<long> sizeList = new List<long>();
            private List<long> styleList = new List<long>();

            /// <summary>
            /// Constructs instance of the class.
            /// </summary>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public InventDimCompositeCriterion(SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdField)
                : base(sqlParameterNameGenerator, productRecIdField)
            {

            }

            /// <summary>
            /// Determines if criterion empty.
            /// </summary>
            public bool IsEmpty
            {
                get 
                {   
                    return configList.Count == 0 && colorList.Count == 0 && sizeList.Count == 0 && styleList.Count == 0; 
                }
            }

            /// <summary>
            /// Adds list of colors to the class.
            /// </summary>
            /// <param name="colors">List of colors.</param>
            public void AddColors(IEnumerable<long> colors)
            {
                colorList.AddRange(colors);
            }

            /// <summary>
            /// Adds list of configurations to the class.
            /// </summary>
            /// <param name="configurations">List of configurations.</param>
            public void AddConfigurations(IEnumerable<long> configurations)
            {
                configList.AddRange(configurations);
            }

            /// <summary>
            /// Adds list of sizes to the class.
            /// </summary>
            /// <param name="sizes">List of sizes.</param>
            public void AddSizes(IEnumerable<long> sizes)
            {
                sizeList.AddRange(sizes);
            }

            /// <summary>
            /// Adds list of styles to the class.
            /// </summary>
            /// <param name="styles">List of styles.</param>
            public void AddStyles(IEnumerable<long> styles)
            {
                styleList.AddRange(styles);
            }

            public override SqlCondition generateSQLCondition()
            {
                StringBuilder builder = new StringBuilder();
                var sqlParameters = new List<SqlParameter>();

                if (configList.Count > 0)
                {
                    var configCriterion = new ConfigurationCriterion(configList, this.sqlParameterNameGenerator, this.productRecIdField);
                    AddDimensionCondition(configCriterion, sqlParameters, builder);
                }

                if (colorList.Count > 0)
                {
                    var colorCriterion = new ColorCriterion(colorList, this.sqlParameterNameGenerator, this.productRecIdField);
                    AddDimensionCondition(colorCriterion, sqlParameters, builder);
                }

                if (sizeList.Count > 0)
                {
                    var sizeCriterion = new SizeCriterion(sizeList, this.sqlParameterNameGenerator, this.productRecIdField);
                    AddDimensionCondition(sizeCriterion, sqlParameters, builder);
                }

                if (styleList.Count > 0)
                {
                    var styleCriterion = new StyleCriterion(styleList, this.sqlParameterNameGenerator, this.productRecIdField);
                    AddDimensionCondition(styleCriterion, sqlParameters, builder);
                }

                string sql = string.Empty;
                if (builder.Length > 0)
                {
                    sql = string.Format(sqlTemplate, builder.ToString());
                }
                
                return new SqlCondition(sql, sqlParameters.ToArray());
            }

            private void AddDimensionCondition(InventDimCriterion invDimCriterion, List<SqlParameter> sqlParameters, StringBuilder builder)
            {
                var invDimCondition = invDimCriterion.generateSQLCondition();
                sqlParameters.AddRange(invDimCondition.SqlParameters);
                builder.AppendLine();
                builder.Append(invDimCondition.SqlExpression);
            }
        }

        /// <summary>
        /// Abstract class which represents an attribute value criterion for searching products.
        /// </summary>
        internal abstract class AttributeValueCriterion : Criterion
        {
            /// <summary>
            /// Attribute value table alias.
            /// </summary>
            protected const string attributeValueTableAlias = "erv";
            /// <summary>
            /// Attribute record id.
            /// </summary>
            readonly protected long attributeRecId;
            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            protected AttributeValueCriterion(long attributeRecId, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(sqlParameterNameGenerator, productRecIdFields)
            {
                this.attributeRecId = attributeRecId;
            }

            public override SqlCondition generateSQLCondition()
            {

                string inventDimValueParam = sqlParameterNameGenerator.generateParameterName();
                string sql = @"
                EXISTS (SELECT erav.VALUE FROM [AX].[ECORESPRODUCTINSTANCEVALUE] erpiv
	                INNER JOIN [AX].[ECORESATTRIBUTEVALUE] erav ON erav.INSTANCEVALUE = erpiv.RECID AND erav.ATTRIBUTE = {0}
	                INNER JOIN {1} erv on erv.RECID = erav.VALUE
                WHERE erpiv.PRODUCT = {2} {3})
                ";            
                
                var condition = ValueCondition;
                var parameters = new List<SqlParameter>(condition.SqlParameters);
                string attributRecIdParam = sqlParameterNameGenerator.generateParameterName();
                parameters.Add(new SqlParameter(attributRecIdParam, attributeRecId));
                sql = string.Format(sql, attributeRecId, AttributeValueTable, this.productRecIdField, condition.SqlExpression);
                return new SqlCondition(sql, parameters.ToArray());
            }

            /// <summary>
            /// Attribute value table.
            /// </summary>
            protected abstract string AttributeValueTable { get; }
            /// <summary>
            /// Value condition.
            /// </summary>
            protected abstract SqlCondition ValueCondition { get; }
        }

        /// <summary>
        /// Abstract class which represents an attribute values range criterion for searching products.
        /// </summary>
        internal abstract class AtrributeValueRangeCriterion<T> : AttributeValueCriterion where T : struct
        {
            private SqlCondition condition;

            /// <summary>
            /// Range of values
            /// </summary>
            protected T? valueFrom, valueTo;

            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="valueFrom">Value from.</param>
            /// <param name="valueTo">Value to.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            protected AtrributeValueRangeCriterion(long attributeRecId, T? valueFrom, T? valueTo, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, sqlParameterNameGenerator, productRecIdFields)
            {
                this.valueFrom = valueFrom;
                this.valueTo = valueTo;
                condition = null;
            }

            private void processValues()
            {
                string parameterFrom = null, parameterTo = null;
                StringBuilder where = new StringBuilder();
                var parameters = new List<SqlParameter>();
                if (valueFrom.HasValue)
                {
                    parameterFrom = sqlParameterNameGenerator.generateParameterName();
                    where.AppendFormat("( {0}.{1} >= {2} ) ", attributeValueTableAlias, AttrributeValueField, parameterFrom);
                    parameters.Add(new SqlParameter(parameterFrom, valueFrom));
                }

                if (valueTo.HasValue)
                {
                    if (where.Length > 0)
                    {
                        where.Append(" AND ");
                    }
                    parameterTo = sqlParameterNameGenerator.generateParameterName();
                    where.AppendFormat("( {0}.{1} <= {2} ) ", attributeValueTableAlias, AttrributeValueField, parameterTo);
                    parameters.Add(new SqlParameter(parameterTo, valueTo));
                }

                if (where.Length > 0)
                {
                    where.Insert(0, " AND ( ");
                    where.Append(" ) ");
                    condition = new SqlCondition(where.ToString(), parameters.ToArray());
                }
                else
                {
                    condition = new SqlCondition(string.Empty);
                }
            }

            protected override SqlCondition ValueCondition
            {
                get
                {
                    if (condition == null)
                    {
                        processValues();
                    }
                    return condition;
                }
            }

            /// <summary>
            /// Attribute value field
            /// </summary>
            protected abstract string AttrributeValueField { get; }
        }

        /// <summary>
        /// Class which represents an attribute integer value range criterion for searching products.
        /// </summary>
        internal class AtrributeIntValueCriterion : AtrributeValueRangeCriterion<int>
        {
            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="valueFrom">Value from.</param>
            /// <param name="valueTo">Value to.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeIntValueCriterion(long attributeRecId, int? valueFrom, int? valueTo, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIdFields)
            {
            }

            protected override string AttrributeValueField
            {
                get { return "INTVALUE"; }
            }

            protected override string AttributeValueTable
            {
                get { return "[AX].[ECORESINTVALUE]"; }
            }

        }

        /// <summary>
        /// Class which represents an attribute float value range criterion for searching products.
        /// </summary>
        internal class AtrributeFloatValueCriterion : AtrributeValueRangeCriterion<decimal>
        {
            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="valueFrom">Value from.</param>
            /// <param name="valueTo">Value to.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeFloatValueCriterion(long attributeRecId, decimal? valueFrom, decimal? valueTo, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIdFields)
            {
            }

            protected override string AttrributeValueField
            {
                get { return "FLOATVALUE"; }
            }

            protected override string AttributeValueTable
            {
                get { return "[AX].[ECORESFLOATVALUE]"; }
            }

        }

        /// <summary>
        /// Class which represents an attribute currency value range criterion for searching products.
        /// </summary>
        internal class AtrributeCurrencyValueCriterion : AtrributeValueRangeCriterion<decimal>
        {
            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="valueFrom">Value from.</param>
            /// <param name="valueTo">Value to.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeCurrencyValueCriterion(long attributeRecId, decimal? valueFrom, decimal? valueTo, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIdFields)
            {
            }

            protected override string AttrributeValueField
            {
                get { return "CURRENCYVALUE"; }
            }

            protected override string AttributeValueTable
            {
                get { return "[AX].[ECORESCURRENCYVALUE]"; }
            }

        }

        /// <summary>
        /// Class which represents an attribute datetime value range criterion for searching products.
        /// </summary>
        internal class AtrributeDateTimeValueCriterion : AtrributeValueRangeCriterion<DateTime>
        {
            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="valueFrom">Value from.</param>
            /// <param name="valueTo">Value to.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeDateTimeValueCriterion(long attributeRecId, DateTime? valueFrom, DateTime? valueTo, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, valueFrom, valueTo, sqlParameterNameGenerator, productRecIdFields)
            {
                if (valueFrom.HasValue)
                {
                    this.valueFrom = (valueFrom.Value.Date).ToUniversalTime();
                }
                if (valueTo.HasValue)
                {
                    this.valueTo = (valueTo.Value.Date.AddDays(1).AddSeconds(-1)).ToUniversalTime();
                }
            }

            protected override string AttrributeValueField
            {
                get { return "DATETIMEVALUE"; }
            }

            protected override string AttributeValueTable
            {
                get { return "[AX].[ECORESDATETIMEVALUE]"; }
            }

        }

        /// <summary>
        /// Class which represents a attribute boolean value criterion for searching products.
        /// </summary>
        internal class AtrributeBooleanValueCriterion : AttributeValueCriterion
        {
            private bool? attributeValue;

            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="attributeValue">Attribute value.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeBooleanValueCriterion(long attributeRecId, bool? attributeValue, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, sqlParameterNameGenerator, productRecIdFields)
            {
                this.attributeValue = attributeValue;
            }

            protected override SqlCondition ValueCondition
            {
                get
                {
                    SqlCondition condition = SqlCondition.Empty;
                    if (this.attributeValue != null)
                    {
                        string parameterName = sqlParameterNameGenerator.generateParameterName();
                        string sql = string.Format(" AND ({0}.BOOLEANVALUE = {1}) ", attributeValueTableAlias, parameterName);
                        condition = new SqlCondition(sql, new SqlParameter(parameterName, attributeValue.Value ? 1 : 0));
                    }
                    return condition;
                }
            }

            protected override string AttributeValueTable
            {
                get { return "[AX].[ECORESBOOLEANVALUE]"; }
            }
        }

        /// <summary>
        /// Abstarct class which represents an attribute text value criterion for searching products.
        /// </summary>
        internal abstract class AtrributeTextBaseValueCriterion : AttributeValueCriterion
        {
            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="languageId">Language Id for translation.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            protected AtrributeTextBaseValueCriterion(long attributeRecId, string languageId, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, sqlParameterNameGenerator, productRecIdFields)
            {
                this.languageId = languageId;
            }

            protected override SqlCondition ValueCondition
            {
                get
                {
                    string sql = @" AND (({0}.TEXTVALUE {1}) OR ((SELECT TEXTVALUE FROM  [AX].[ECORESTEXTVALUETRANSLATION] ertvt WHERE ertvt.TEXTVALUETABLE = {2}.RECID AND ertvt.[LANGUAGE] = {3}) {1}))";
                    SqlCondition condition = TextValueExpression;
                    if (!SqlCondition.IsEmpty(condition))
                    {
                        var parameters = new List<SqlParameter>(condition.SqlParameters);
                        string parameterName = sqlParameterNameGenerator.generateParameterName();
                        parameters.Add(new SqlParameter(parameterName, languageId));

                        sql = string.Format(sql, attributeValueTableAlias, condition.SqlExpression, attributeValueTableAlias, parameterName);
                        condition = new SqlCondition(sql, parameters);
                    }
                    return condition;
                }
            }

            protected override string AttributeValueTable
            {
                get { return "[AX].[ECORESTEXTVALUE]"; }
            }

            protected string languageId { get; private set; }

            /// <summary>
            /// Attribute value field.
            /// </summary>
            protected string AttrributeValueField
            {
                get { return "TEXTVALUE"; }
            }

            /// <summary>
            /// Text value expression.
            /// </summary>
            protected abstract SqlCondition TextValueExpression { get; }
        }

        /// <summary>
        /// Class which represents an attribute text value criterion which is compares using 'like' sql operator for searching products.
        /// </summary>
        internal class AtrributeTextLikeValueCriterion : AtrributeTextBaseValueCriterion
        {
            private string attributeValue;

            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="attributeValue">Attribute value.</param>
            /// <param name="languageId">Language Id for translation.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeTextLikeValueCriterion(long attributeRecId, string attributeValue, string languageId, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, languageId, sqlParameterNameGenerator, productRecIdFields)
            {
                this.attributeValue = string.Format("%{0}%", attributeValue);
            }

            protected override SqlCondition TextValueExpression
            {
                get
                {
                    string parameterName = sqlParameterNameGenerator.generateParameterName();
                    return new SqlCondition(
                            string.Format("like {0} ", parameterName),
                            new SqlParameter(parameterName, attributeValue));
                }
            }
        }

        /// <summary>
        /// Сlass which represents an attribute text value list criterion for searching products.
        /// </summary>
        internal class AtrributeTextListValueCriterion : AtrributeTextBaseValueCriterion
        {
            private IEnumerable<string> attributeValues;

            /// <summary>
            /// Construct instance of the class.
            /// </summary>
            /// <param name="attributeRecId">Attribute record id.</param>
            /// <param name="attributeValue">List of text attribute values.</param>
            /// <param name="languageId">Language Id for translation.</param>
            /// <param name="sqlParameterNameGenerator">Sql parameters name generator.</param>
            /// <param name="productRecIdField">Product record id fiels name.</param>
            public AtrributeTextListValueCriterion(long attributeRecId, IEnumerable<string> attributeValues, string languageId, SqlParameterNameGenerator sqlParameterNameGenerator, string productRecIdFields)
                : base(attributeRecId, languageId, sqlParameterNameGenerator, productRecIdFields)
            {
                this.attributeValues = attributeValues;
            }

            protected override SqlCondition TextValueExpression
            {
                get
                {
                    StringBuilder stringList = new StringBuilder();
                    var parameters = new List<SqlParameter>();
                    foreach (string attributeValue in attributeValues)
                    {
                        string parameterName = sqlParameterNameGenerator.generateParameterName();
                        if (stringList.Length > 0)
                            stringList.Append(" , ");
                        stringList.Append(parameterName);
                        parameters.Add(new SqlParameter(parameterName, attributeValue));
                    }
                    stringList.Insert(0, " IN ( ");
                    stringList.Append(" ) ");

                    return new SqlCondition(stringList.ToString(), parameters);
                }
            }
        }

        #endregion
    }
}
