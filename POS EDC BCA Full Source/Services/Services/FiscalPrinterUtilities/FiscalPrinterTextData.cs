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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;

namespace Microsoft.Dynamics.Retail.FiscalPrinter
{
    /// <summary>
    /// Class implements IFiscalPrinterTextWithFormatting interface.
    /// </summary>
    [Export(typeof(IFiscalPrinterTextData))]
    public class FiscalPrinterTextData : IFiscalPrinterTextData
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public FiscalPrinterTextData()
        {
        }
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="fontStyle">Style of the text.</param>
        public FiscalPrinterTextData(string text, IFiscalPrinterFontStyle fontStyle)
        {
            this.Text = text;
            this.FontStyle = fontStyle;
        }
        /// <summary>
        /// Text to print.
        /// </summary>
        public string Text  { get; set; }

        /// <summary>
        /// Font style of the text.
        /// </summary>
        public IFiscalPrinterFontStyle FontStyle { get; set; }        
    }
}
