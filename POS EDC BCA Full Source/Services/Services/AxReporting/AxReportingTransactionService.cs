/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using LSRetailPosis;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Commerce.Runtime.TransactionService.Serialization;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

namespace Microsoft.Dynamics.Retail.Pos.AxReporting
{
    /// <summary>
    /// Internal class for working with transaction service
    /// </summary>
    internal static class AxReportingTransactionService
    {
        private static XDocument LabelReportRequestToXml(DateTime reportDate, RetailLabelTypeBase labelType, SRSReportFileFormat reportFileFormat, IEnumerable<ILabelReportItem> labelReportItemList)
        {
            var itemLines = new XElement("ItemsToPrint");
            XElement rootElmt = new XElement("PrintLabels",
                    new XElement("StoreId", ApplicationSettings.Terminal.StoreId),
                    new XElement("TerminalId", ApplicationSettings.Terminal.TerminalId),
                    new XElement("ReportDate", SerializationHelper.ConvertDateTimeToAXDateString(reportDate, 213)), // mm-dd-yyyy date sequence for str2date in AX
                    new XElement("LabelType", (int)labelType),
                    new XElement("FileFormat", (int)reportFileFormat),
                    itemLines
            );

            foreach (var item in labelReportItemList)
            {
                itemLines.Add(
                new XElement("Item",
                    new XElement("ItemId", item.ItemId),
                    new XElement("VariantId", item.VariantId),
                    new XElement("Quantity", item.Quantity)
                    )
                );
            }

            return new XDocument(rootElmt);
        }

        private static string GetReportFileNameFromXml(XDocument responseXml)
        {

            var xmlReportFile = responseXml.Descendants("ReportFile").SingleOrDefault();

            return xmlReportFile != null ? xmlReportFile.Attribute("Path").Value : string.Empty;
        }

        private static IEnumerable<LabelReportNotPrintedItem> GetNotPrintedItemsFromXml(XDocument responseXml)
        {
            IEnumerable<LabelReportNotPrintedItem> notPrintedItems = (from notPrintedItem in responseXml.Descendants("NotPrintedItem")
                                                                      select new LabelReportNotPrintedItem()
                                                                      {
                                                                          ItemId = notPrintedItem.Attribute("ItemId").Value,
                                                                          VariantId = notPrintedItem.Attribute("VariantId").Value,
                                                                          Reason = notPrintedItem.Attribute("Reason").Value,
                                                                      });

            return notPrintedItems;
        }

        /// <summary>
        /// Runs label report on AOS.
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <param name="labelType">Report type.</param>
        /// <param name="reportFileFormat">Report file format.</param>
        /// <param name="labelReportItemList">List of items to be printed.</param>
        /// <returns>LabelReportResponse class contains results and generated filename.</returns>
        public static LabelReportResponse RunLabelReport(DateTime reportDate, RetailLabelTypeBase labelType, SRSReportFileFormat reportFileFormat, IEnumerable<ILabelReportItem> labelReportItemList)
        {
            const int ResponseResult = 1;
            const int ResponseErrorMessage = 2;
            const int ResponseInfologContent = 3;
            const int ResponseReturnedXml = 4;

            if (labelReportItemList == null)
            {
                throw new ArgumentNullException("labelReportItemList");
            }

            LabelReportResponse response = new LabelReportResponse();

            var xmlDoc = LabelReportRequestToXml(reportDate, labelType, reportFileFormat, labelReportItemList);
            int timeOutInSec = LSRetailPosis.Settings.TransactionServicesProfiles.TransactionServiceProfile.ReportExecutionTimeout;
            var methodTimeout = timeOutInSec > 0 ? (TimeSpan?)new TimeSpan(0, 0, timeOutInSec) : null;
            System.Collections.ObjectModel.ReadOnlyCollection<object> serviceResponse = AxReportingHelper.Instance.Application.TransactionServices.Invoke(
                methodTimeout,
                "RunLabelReport",
                xmlDoc.ToString()
                );

            response.Result = (bool)serviceResponse[ResponseResult];

            if (response.Result)
            {
                xmlDoc = XDocument.Parse(serviceResponse[ResponseReturnedXml].ToString());
                response.ReportFile = GetReportFileNameFromXml(xmlDoc);
                response.NotPrintedItems = GetNotPrintedItemsFromXml(xmlDoc);
            }
            else
            {
                response.ErrorMessage = serviceResponse[ResponseErrorMessage].ToString();
                response.InfologContent = serviceResponse[ResponseInfologContent].ToString();
            }

            return response;
        }

        /// <summary>
        /// Reads label report file part.
        /// </summary>
        /// <param name="serverFileName">Label report file name on server.</param>
        /// <param name="startPosition">Starting position of a byte rto read.</param>
        /// <param name="length">Number of bytes to read.</param>
        /// <param name="bytesRead">Array of bytes where to read.</param>
        /// <returns>True if read file part is not the last one; false otherwise.</returns>
        public static bool ReadFilePart(string serverFileName, long startPosition, int length, out byte[] bytesRead)
        {
            if (string.IsNullOrWhiteSpace(serverFileName))
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100022), "serverFileName"); // The server file name is not specified.

            if (startPosition < 0L)
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100023), "startPosition"); // The starting position of the packet in the file should be a positive value.

            if (length <= 0)
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100018), "length"); // The packet size must be a positive value.

            var result = AxReportingHelper.Instance.Application.TransactionServices.Invoke("getFilePart", serverFileName, startPosition, length);
            bool success = (bool)result[1];
            if (!success)
            {
                string errorMessage = (string)result[2];
                throw new InvalidOperationException(string.Format("{0} {1}: {2}", ApplicationLocalizer.Language.Translate(100003), ApplicationLocalizer.Language.Translate(100008), errorMessage)); // An error occurred on the Dynamics AX server. Contact your administrator for further assistance.
            }

            string bytesBase64 = (string)result[3];
            bytesRead = Convert.FromBase64String(bytesBase64);

            return bytesRead.Length == length;
        }

        /// <summary>
        /// Gets label report file Sha256 hash.
        /// </summary>
        /// <param name="serverFileName">Label report file name on server.</param>
        /// <returns>Sha256 hash.</returns>
        public static string GetFileSha256HashBase64(string serverFileName)
        {
            if (string.IsNullOrWhiteSpace(serverFileName))
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100022), "serverFileName"); // The server file name is not specified.

            var result = AxReportingHelper.Instance.Application.TransactionServices.Invoke("getFileSha256Hash", serverFileName);
            bool success = (bool)result[1];
            if (!success)
            {
                string errorMessage = (string)result[2];
                throw new InvalidOperationException(string.Format("{0} {1}: {2}", ApplicationLocalizer.Language.Translate(100003), ApplicationLocalizer.Language.Translate(100008), errorMessage)); // An error occurred on the Dynamics AX server. Contact your administrator for further assistance.
            }

            string sha1HashBase64 = (string)result[3];

            return sha1HashBase64;
        }

        /// <summary>
        /// Deletes generated label report files on DAX server.
        /// </summary>
        /// <param name="serverFileName">Label report file name on server.</param>
        public static void DeleteFileOnServer(string serverFileName)
        {
            ReadOnlyCollection<object> result;
            result = AxReportingHelper.Instance.Application.TransactionServices.Invoke("deleteFile", serverFileName);
            bool success = (bool)result[1];
            if (success)
                return;

            string errorMessage = (string)result[2];
            ApplicationLog.Log(ApplicationLocalizer.Language.Translate(100002), string.Format("{0}: {1}", ApplicationLocalizer.Language.Translate(100003), errorMessage), LogTraceLevel.Error); // Print label report: An error occurred on the Dynamics AX server.: 
        }
    }
}
