/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

namespace Microsoft.Dynamics.Retail.Pos.AxReporting
{
    /// <summary>
    /// Class implementing the interface IAxReporting
    /// </summary>
    [Export(typeof(IAxReporting))]
    public class AxReporting : IAxReporting
    {
        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
        [Import]
        public IApplication Application
        {
            set
            {
                AxReportingHelper.Instance.Application = value;
            }
        }

        public ILabelReportResponse PrintLabelReport(DateTime reportDate, RetailLabelTypeBase labelType, SRSReportFileFormat reportFileFormat, IEnumerable<ILabelReportItem> labelReportItemList)
        {
            throw new NotSupportedException("This method is no longer supported. Use AxReporting.PrintLabelReportAsync().");
        }

        public ILabelReportItem CreateLabelReportItem(string itemId, string variantId, decimal quantity)
        {
            return new LabelReportItem()
            {
                ItemId = itemId,
                VariantId = variantId,
                Quantity = quantity
            };
        }

        public object CallerData 
        { 
            get
            {
                return AxReportingHelper.Instance.CallerData;
            }
            set
            {
                AxReportingHelper.Instance.CallerData = value;
            }
        }

        public event EventHandler PrintLabelReportCompleted;

        public async void PrintLabelReportAsync(DateTime reportDate, RetailLabelTypeBase labelType, SRSReportFileFormat reportFileFormat, IEnumerable<ILabelReportItem> labelReportItemList)
        {
            if (labelReportItemList == null)
            {
                throw new ArgumentNullException("labelReportItemList");
            }

            ILabelReportResponse response = null;
            string message = null;

            try
            {
                AxReportingHelper.Instance.TaskPrintLabelReport = Task<ILabelReportResponse>.Factory.StartNew(() =>
                {
                    return AxReportingHelper.Instance.PrintLabelReport(reportDate, labelType, reportFileFormat, labelReportItemList);
                });

                response = await AxReportingHelper.Instance.TaskPrintLabelReport;

                if (response != null)
                {
                    message = response.ErrorMessage;
                    if (response.Result)
                    {
                        message = ApplicationLocalizer.Language.Translate(100030); // Printing of labels is complete.
                    }
                }
            }
            catch (Exception ex)
            {
                message = ApplicationLocalizer.Language.Translate(100034); // An error occurred during the asynchronous printing of labels.
                ApplicationLog.Log(message, ex.Message, LogTraceLevel.Error);
            }
            finally
            {
                POSFormsManager.ShowPOSStatusPanelText(message);
                OnPrintLabelReportCompleted(new EventArgs());
            }
        }

        private void OnPrintLabelReportCompleted(EventArgs e)
        {
            EventHandler handler = PrintLabelReportCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public bool SetupPrinterSettings(bool restoreSavedSettings)
        {
            return AxReportingHelper.Instance.SetupPrinterSettings(restoreSavedSettings);
        }

        public ILabelReportResponse GetPrintLabelReportResult()
        {
            ILabelReportResponse response = null;

            if ((AxReportingHelper.Instance.TaskPrintLabelReport != null)
                && (AxReportingHelper.Instance.TaskPrintLabelReport.IsCompleted || AxReportingHelper.Instance.TaskPrintLabelReport.IsFaulted))
            {
                try
                {
                    response = AxReportingHelper.Instance.TaskPrintLabelReport.Result;
                }
                catch (AggregateException ae)
                {
                    StringBuilder exMessage = new StringBuilder(ApplicationLocalizer.Language.Translate(100034)); // An error occurred during the asynchronous printing of labels.
                    foreach (var ex in ae.Flatten().InnerExceptions)
                    {
                        exMessage.Append(ex.Message);
                    }
                    response = new LabelReportResponse() { 
                        Result = false,
                        ErrorMessage = ApplicationLocalizer.Language.Translate(100034), // An error occurred during the asynchronous printing of labels.
                        InfologContent = exMessage.ToString()
                    };
                }
                finally
                {
                    AxReportingHelper.Instance.TaskPrintLabelReport = null;
                }
            }

            return response;
        }

        public bool? IsPrintLabelReportTaskStarted()
        {
            bool? result = null;

            if (AxReportingHelper.Instance.TaskPrintLabelReport != null)
            {
                if (!AxReportingHelper.Instance.TaskPrintLabelReport.IsCompleted && !AxReportingHelper.Instance.TaskPrintLabelReport.IsFaulted)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }

    #region Supporting classes

    /// <summary>
    /// Label report item class
    /// </summary>
    internal class LabelReportItem : ILabelReportItem
    {
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// Label report not printed item class
    /// </summary>
    internal class LabelReportNotPrintedItem : ILabelReportNotPrintedItem
    {
        public string ItemId { get; set; }
        public string VariantId { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// Label report response class
    /// </summary>
    internal class LabelReportResponse : ILabelReportResponse
    {
        #region ILabelReportResponse
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
        public string InfologContent { get; set; }
        public IEnumerable<ILabelReportNotPrintedItem> NotPrintedItems { get; set; }
        #endregion

        public string ReportFile { get; set; }
    }

    #endregion
}
