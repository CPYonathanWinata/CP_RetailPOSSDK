/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.FiscalRegistrationServices;

namespace Microsoft.Dynamics.Retail.POS.FiscalRegistrationServices.DefaultFiscalRegistrationService
{
    internal static class ConfigLoader
    {
        /// <summary>
        /// Exports fiscal register service configuration file from database to temporary file and returns its path.
        /// </summary>
        /// <returns>Path to the temporary configuration file.</returns>
        public static string GetConfigFilename()
        {
            string tmpFileName = string.Empty;
            string configurationText = string.Empty;

            var queryString = new StringBuilder(@"
                                                    SELECT RFPC.XMLCONTENT FROM ax.RETAILFISCALPRINTERCONFIGTABLE AS RFPC
                                                    INNER JOIN ax.RETAILHARDWAREPROFILE AS RHP ON RFPC.CONFIGID = RHP.FISCALREGISTERCONFIGID
                                                    WHERE RHP.PROFILEID = @PROFILEID
                                                ");

            var parameterList = new List<SqlParameter>
                                                {
                                                    new SqlParameter("@PROFILEID", ApplicationSettings.Terminal.HardwareProfile),
                                                };

            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var dataTable = dbUtil.GetTable(queryString.ToString(), parameterList.ToArray());

            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["XMLCONTENT"] != DBNull.Value)
            {
                configurationText = dataTable.Rows[0]["XMLCONTENT"].ToString();
            }
            else
            {
                ExceptionHelper.ShowAndLogException(Resources.ConfigFileDoesNotExistInDB);
                return tmpFileName;
            }

            tmpFileName = Path.GetTempFileName();

            XmlDocument xdoc = new XmlDocument();

            xdoc.LoadXml(configurationText);
            xdoc.Save(tmpFileName);

            return tmpFileName;
        }
    }
}