/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Xml;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSRetailPosis.Settings;
using LSRetailPosis.DataAccess.DataUtil;

namespace Microsoft.Dynamics.Retail.FiscalPrinter.RussianFiscalPrinter
{
    /// <summary>
    /// Helper class for loading configuration data from database
    /// </summary>
    public static class ConfigurationHelper
    {
        private const string imageExt = ".bmp";

        /// <summary>
        /// Exports fiscal printer configuration file from database to temporary file and returns its path
        /// </summary>
        /// <returns>Path to the temporary configuration file</returns>
        public static string GetPrinterConfigFilename()
        {
            string tmpFileName = string.Empty;
            string configurationText = string.Empty;

            var queryString = new StringBuilder(@"
                                                    SELECT XMLCONTENT FROM ax.RETAILFISCALPRINTERCONFIGTABLE WHERE CONFIGID IN
                                                   (SELECT FISCALPRINTERCONFIGID FROM ax.RETAILHARDWAREPROFILE WHERE PROFILEID = @PROFILEID)   
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
                ExceptionHelper.ThrowException(true, Resources.ConfigFileDoesNotExistInDB);
            }

            tmpFileName = Path.GetTempFileName();

            XmlDocument xdoc = new XmlDocument();

            xdoc.LoadXml(configurationText);
            xdoc.Save(tmpFileName);

            //File.WriteAllText(tmpFileName, configurationText);

            return tmpFileName;
        }


        /// <summary>
        /// Exports image from database to temporary file and returns its path
        /// </summary>
        /// <param name="imageId">Image id in ax.RETAILIMAGES database</param>
        /// <returns>Path to the temporary file with resource image</returns>
        public static string GetImageFileName(int imageId)
        {
            string tmpFileName = string.Empty;
            byte[] imageData = new byte[0];

            var queryString = new StringBuilder(@"SELECT PICTURE FROM ax.RETAILIMAGES WHERE PICTUREID = @PICTUREID");

            var parameterList = new List<SqlParameter>
                                                {
                                                    new SqlParameter("@PICTUREID", imageId),
                                                };

            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var dataTable = dbUtil.GetTable(queryString.ToString(), parameterList.ToArray());

            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["PICTURE"] != DBNull.Value)
            {
                imageData = (byte[])dataTable.Rows[0]["PICTURE"];
            }
            else
            {
                ExceptionHelper.ThrowException(true, Resources.Translate(Resources.InvalidImageId, imageId));
            }

            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + imageExt;
            tmpFileName = Path.Combine(path, fileName);

            File.WriteAllBytes(tmpFileName, imageData);

            return tmpFileName;
        }
        
        /// <summary>
        /// Loads image from file
        /// </summary>
        /// <param name="filename">Path to image file</param>
        /// <returns>Bitmap object with image</returns>
        public static Bitmap LoadImageFromFile(string filename)
        {
            return (Bitmap)System.Drawing.Image.FromFile(filename, true);
        }
    }
}
