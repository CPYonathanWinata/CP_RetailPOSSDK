/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Data.SqlClient;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.FiscalRegistrationServices
{
    /// <summary>
    /// Helper class for retrieving tax data from database.
    /// </summary>
    public static class TaxHelper
    {
        /// <summary>
        /// Get tax rate by tax code from database.
        /// </summary>
        /// <param name="taxCode">Tax code.</param>
        /// <returns>Tax rate.</returns>
        public static decimal? GetTaxRate(string taxCode)
        {
            SqlConnection localConnection = ApplicationSettings.Database.LocalConnection;
            string dataAreaId = ApplicationSettings.Database.DATAAREAID;
            long storePrimaryId = ApplicationSettings.Terminal.StorePrimaryId;

            TaxData taxData = new TaxData(localConnection, dataAreaId);

            var taxRate = taxData.GetTaxValueIfExists(taxCode);
            return taxRate;
        }
    }
}
