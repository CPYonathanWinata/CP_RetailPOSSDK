/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

//Microsoft Dynamics AX for Retail POS Plug-ins
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.DevUtilities;
using System.Diagnostics;

namespace Microsoft.Dynamics.Retail.Pos.CreateDatabaseService
{

    internal class ImportInitialData
    {

        #region Fields

        private DBUtil dbUtil;
        private Action<string> importStatusCallback;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor.
        /// Import all available data into database.
        /// </summary>
        /// <param name="dbUtil"></param>
        /// <param name="dataAreaID"></param>
        /// <param name="statusCallback"></param>
        internal ImportInitialData(DBUtil dbUtil, Action<string> statusCallback)
        {
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for CorporateCard are reserved at 50600 - 50699
            //
            // These TextID's are in every class in the CreateDatabase project

            this.dbUtil = dbUtil;
            this.importStatusCallback = statusCallback;
        }

        /// <summary>
        /// Get table data from resource.
        /// </summary>
        /// <remarks>Caller is responsible for disposing returned object</remarks>
        /// <param name="tableName">Table resource name.</param>
        /// <returns>Caller owned datatable.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        public static DataTable GetData(string tableName)
        {
            DataTable data = new DataTable();
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (StreamReader streamReader = new StreamReader(assembly.GetManifestResourceStream("CreateDatabase." + tableName + ".xml")))
            {
                data.ReadXml(streamReader);
            }

            return data;
        }

        #endregion

        #region Private Methods

        private void RaiseTableImported(string tableName)
        {
            if (importStatusCallback != null && tableName != null)
                importStatusCallback(tableName);
        }

        #endregion

    }
}