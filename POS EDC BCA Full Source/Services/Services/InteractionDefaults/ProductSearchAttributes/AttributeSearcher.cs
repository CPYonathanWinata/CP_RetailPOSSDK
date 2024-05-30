/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ProductSearchAttributes
{
    /// <summary>
    /// Static class containing method related to search product by different attributes
    /// </summary>
    internal static class AttributeSearcher
    {
        #region Private methods

        private static bool IsAttributeRefinable(string xml)
        {
            bool isRefinable = false;
            const string elementName = "ProductProperty";
            const string attributeName = "IsRefinable";

            if (!string.IsNullOrWhiteSpace(xml))
            {
                var xmlDoc = XDocument.Parse(xml);
                if (xmlDoc != null)
                {
                    var productProperty = xmlDoc.Descendants(elementName).SingleOrDefault();
                    if (productProperty != null)
                    {
                        var attribute = productProperty.Attribute(attributeName);
                        if (attribute != null)
                        {
                            string isSearchableAttributeValue = attribute.Value;
                            if (!string.IsNullOrWhiteSpace(isSearchableAttributeValue))
                            {
                                bool.TryParse(isSearchableAttributeValue, out isRefinable);
                            }
                        }
                    }
                }
            }

            return isRefinable;
        }

        private static DataTable GetSearchProductInventDim(long channelId, string sqlFunctionName)
        {
            const string channelIdParam = "@channelId";
            const string sqlString = @"SELECT * FROM {0}({1}) ORDER BY NAME";

            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter(channelIdParam, channelId) };
            string sql = string.Format(sqlString, sqlFunctionName, channelIdParam);


            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var dataTable = dbUtil.GetTable(sql, parameters.ToArray());

            return dataTable;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets list of inventory dimensions available for search product in given store.
        /// </summary>
        /// <param name="channelId">Channel record id.</param>
        /// <returns>Table with results.</returns>
        public static DataTable GetSearchInventDimensionsList(long channelId)
        {
            const string channelIdParam = "@channelId";
            const string sqlString = "SELECT * FROM [ax].[RETAILPUBINVENTDIMCHANNELMETADATA] WHERE HOSTCHANNEL = {0}";

            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter(channelIdParam, channelId)};
            string sql = string.Format(sqlString, channelIdParam);

            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var table = dbUtil.GetTable(sql, parameters.ToArray());

            return table;
        }

        /// <summary>
        /// Gets attributes available for search product in given store and category.
        /// </summary>
        /// <param name="channelId">Channel record id.</param>
        /// <param name="categoryId">Category record id.</param>
        /// <returns>Table with results.</returns>
        public static DataTable GetSearchProductAttributes(long channelId, long categoryId)
        {
            const string channelIdParam = "@channelId";
            const string categoryIdParam = "@categoryId";
            const string sqlString = "SELECT * FROM [dbo].[CHANNELSEARCHPRODUCTATTRIBUTEVIEW] WHERE CHANNEL = {0} AND CATEGORY = {1} ORDER BY NAME";

            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter(channelIdParam, channelId),
                                                                new SqlParameter(categoryIdParam, categoryId),
                                                        };
            string sql = string.Format(sqlString, channelIdParam, categoryIdParam);
            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var table = dbUtil.GetTable(sql, parameters.ToArray());

            var rowsToDelete = table.Rows.OfType<DataRow>().Where(row => !IsAttributeRefinable((string)row["METADATA"])).ToList();
            rowsToDelete.ForEach(row => table.Rows.Remove(row));

            return table;
        }

        /// <summary>
        /// Gets colors available for search product in given store.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns>Table with results.</returns>
        public static DataTable GetSearchProductColors(long channelId)
        {
            const string sqlFunctionName = @"[dbo].[GETCHANNELPRODUCTCOLORS]";

            return GetSearchProductInventDim(channelId, sqlFunctionName);
        }

        /// <summary>
        /// Gets configurations available for search product in given store.
        /// </summary>
        /// <param name="channelId">Channel record id.</param>
        /// <returns>Table with results.</returns>
        public static DataTable GetSearchProductConfigurations(long channelId)
        {
            const string sqlFunctionName = @"[dbo].[GETCHANNELPRODUCTCONFIGURATIONS]";

            return GetSearchProductInventDim(channelId, sqlFunctionName);
        }

        /// <summary>
        /// Gets sizes available for search product in given store.
        /// </summary>
        /// <param name="channelId">Channel record id.</param>
        /// <returns>Table with results.</returns>
        public static DataTable GetSearchProductSizes(long channelId)
        {
            const string sqlFunctionName = @"[dbo].[GETCHANNELPRODUCTSIZES]";

            return GetSearchProductInventDim(channelId, sqlFunctionName);
        }

        /// <summary>
        /// Gets styles available for search product in given store.
        /// </summary>
        /// <param name="channelId">Channel record id.</param>
        /// <returns>Table with results.</returns>
        public static DataTable GetSearchProductStyles(long channelId)
        {
            const string sqlFunctionName = @"[dbo].[GETCHANNELPRODUCTSTYLES]";

            return GetSearchProductInventDim(channelId, sqlFunctionName);
        }

        /// <summary>
        /// Gets get list of possible values for given attribute and channel.
        /// </summary>
        /// <param name="channelId">Channel record id.</param>
        /// <param name="attributeId">Attribute record id.</param>
        /// <returns>Table with results.</returns>
        public static DataTable GetAttributeListOfValues(long channelId, long attributeId)
        {
            const string channelIdParam = "@channelId";
            const string attributeIdParam = "@attributeId";
            const string sqlString = "SELECT * FROM [dbo].[GETATTRIBUTELISTOFVALUES]({0}, {1})";

            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter(channelIdParam, channelId),
                                                                new SqlParameter(attributeIdParam, attributeId),
                                                        };
            string sql = string.Format(sqlString, channelIdParam, attributeIdParam);

            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var dataTable = dbUtil.GetTable(sql, parameters.ToArray());

            return dataTable;
        }

        #endregion
    }
}