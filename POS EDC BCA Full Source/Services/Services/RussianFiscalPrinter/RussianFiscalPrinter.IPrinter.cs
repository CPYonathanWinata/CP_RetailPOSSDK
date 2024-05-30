/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Dynamics.Retail.FiscalPrinter.RussianFiscalPrinter
{
    public sealed partial class RussianFiscalPrinter
    {
        public void Load()
        {
            LogHelper.LogTrace("FiscalPrinter.IPrinter.Load", "Load called");

            InitializePrinter();
        }

        /// <summary>
        /// Gets or sets a value indicating that fiscal printer is in the process of printing slip document.
        /// </summary>
        /// <remarks>
        /// This value is used to track the state of printing slip document on the fiscal printer.
        /// The slip document is a non-fiscal document and is printed as a sequence of print line commands.
        /// This flag is used to let the driver know that it is printing lines of a slip document to enforce proper error handling.
        /// </remarks>
        public bool IsPrintingSlipDocument { get; private set; }
        
        private bool ExecutePrintOperation(Action operation)
        {
            bool retry = true;

            while (retry)
            {
                try
                {
                    IsPrintingSlipDocument = true;
                    operation();
                    IsPrintingSlipDocument = false;
                    return true;
                }
                catch
                {
                    var dialogResult = UserMessages.ShowRetryDialog(Resources.Translate(Resources.SlipDocumentPrintError));
                    retry = dialogResult == System.Windows.Forms.DialogResult.Retry;
                }
            }

            return false;
        }

        public void Unload()
        {
            LogHelper.LogTrace("FiscalPrinter.IPrinter.Unload", "Unload called");

            // Put uninitialization logic here if needed
        }
        
        /// <summary>
        /// This method will print the text as management report in the fiscal printer
        /// </summary>
        /// <param name="text"></param>
        public void PrintReceipt(string text)
        {
            LogHelper.LogTrace("FiscalPrinter.IPrinter.PrintReceipt", "text = {0}", text);

            Action operation = () => { FiscalPrinterDriverFactory.FiscalPrinterDriver.ManagementReportPrintLine(text); };
            if (ExecutePrintOperation(operation))
            {
                RussianFiscalPrinterDriver.FiscalPrinterDriver.RibbonFeed();
                FiscalPrinterDriverFactory.FiscalPrinterDriver.ExecutePaperCut(false);
            }
        }

        /// <summary>
        /// Prints a slip containing the text in the textToPrint parameter
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="footer"></param>
        public void PrintSlip(string header, string details, string footer)
        {
            LogHelper.LogTrace("FiscalPrinter.IPrinter.PrintSlip", "header = {0}, details = {1}, footer = {2}", header, details, footer);

            Action operation = () => 
            {
                FiscalPrinterDriverFactory.FiscalPrinterDriver.ManagementReportPrintLine(header);
                FiscalPrinterDriverFactory.FiscalPrinterDriver.ManagementReportPrintLine(details);
                FiscalPrinterDriverFactory.FiscalPrinterDriver.ManagementReportPrintLine(footer);
            };

            if (ExecutePrintOperation(operation))
            {
                RussianFiscalPrinterDriver.FiscalPrinterDriver.RibbonFeed();
                FiscalPrinterDriverFactory.FiscalPrinterDriver.ExecutePaperCut(false);
            }
        }

        /// <summary>Determines whether the fiscal printer supports printing receipt in non fiscal mode.</summary>
        /// <param name="copyReceipt">Denotes that this is a copy of a receipt; optional, false by default.</param>
        /// <returns>True if the fiscal printer supports printing receipt in non fiscal mode; false otherwise.</returns>
        public bool SupportPrintingReceiptInNonFiscalMode(bool copyReceipt)
        {
            return copyReceipt;
        }

        /// <summary> Determines whether the fiscal printer prohibits printing receipt on non fiscal printers. </summary>
        /// <param name="copyReceipt">Denotes that this is a copy of a receipt; optional, false by default. </param>
        /// <returns>True if the fiscal printer prohibits printing receipt on non fiscal printers; false otherwise. </returns>
        public bool ProhibitPrintingReceiptOnNonFiscalPrinters(bool copyReceipt)
        {
            return false;
        }

        /// <summary>        
        /// Gets maximum number of symbols on paper for the specific <c>IFiscalPrinterFontStyle</c>.
        /// </summary>
        /// <param name="fontStyle">Font style.</param>        
        public int GetLineLegth(IFiscalPrinterFontStyle fontStyle)
        {
            if (fontStyle == null)
            {
                fontStyle = FiscalPrinterUtility.CreateDefaultFiscalPrinterFontStyle();
            }

            LogHelper.LogTrace("FiscalPrinter.IPrinting.GetMaxSymbolsOnPaperPerLine", "fontStyle = {0} ", fontStyle.FontStyleType);

            return RussianFiscalPrinterDriver.FiscalPrinterDriver.GetLineLength(fontStyle);
        }

        /// <summary>
        /// Creates text style based on fontStyle and partner data.
        /// </summary>
        /// <param name="fontStyleType">Style of the text.</param>
        /// <param name="partnerData">Dynamic object that hold partner's data.</param>
        /// <returns>return the object created</returns>
        public IFiscalPrinterFontStyle CreateFiscalPrinterFontStyle(FontStyleType fontStyleType, dynamic partnerData)
        {
            LogHelper.LogTrace("FiscalPrinter.CreateFiscalPrinterFontStyle", "FontStyleType = {0} ", fontStyleType);
            return FiscalPrinterUtility.CreateFiscalPrinterFontStyle(fontStyleType, partnerData);
        }

        /// <summary>
        /// Creates font style with text based on text, fontStyle and partner data.
        /// </summary>
        /// <param name="text">Text to print.</param>
        /// <param name="fontStyleType">Style of the text.</param>
        /// <param name="partnerData">Dynamic object that hold partner's data.</param>
        /// <returns>return the object created.</returns>
        public IFiscalPrinterTextData CreateFiscalPrinterTextData(string text, FontStyleType fontStyleType, dynamic partnerData)
        {
            LogHelper.LogTrace("FiscalPrinter.CreateFiscalPrinterTextWithFormatting", "text = {0} fontStyleType = {1}", text, fontStyleType);
            return FiscalPrinterUtility.CreateFiscalPrinterTextData(text, fontStyleType, partnerData);
        }

        /// <summary>
        /// Creates font style with text based on text and font style.
        /// </summary>
        /// <param name="text">Text to print.</param>
        /// <param name="fontStyle">Style of the font.</param>        
        /// <returns>return the object created.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods")]
        public IFiscalPrinterTextData CreateFiscalPrinterTextData(string text, IFiscalPrinterFontStyle fontStyle)
        {
            if (fontStyle == null)
            {
                throw new ArgumentException("fontStyle");
            }

            LogHelper.LogTrace("FiscalPrinter.CreateFiscalPrinterTextWithFormatting", "text = {0} fontStyle = {1}", text, fontStyle.FontStyleType);
            return FiscalPrinterUtility.CreateFiscalPrinterTextData(text, fontStyle);
        }

        /// <summary>
        /// Print custom document.
        /// </summary>
        /// <param name="layoutId">layoutid parameter value.</param>
        /// <param name="textToPrint">List of text blocks with format style to print.</param>
        /// <param name="posTransaction">POS transaction.</param>
        public void PrintCustomDocument(string layoutId, IList<IFiscalPrinterTextData> textToPrint, IPosTransaction posTransaction)
        {
            LogHelper.LogTrace("FiscalPrinter.IPrinting.PrintCustomDocument", "layoutId = {0} textToString = {1}", layoutId, textToPrint == null ? string.Empty: textToPrint.ToString());

            RussianFiscalPrinterDriver.FiscalPrinterDriver.PrintCustomDocument(layoutId, textToPrint, posTransaction);
        }
    }
}
