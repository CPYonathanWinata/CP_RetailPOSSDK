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
    /// Class implements IFiscalPrinterFontStyle interface.
    /// </summary>
    [Export(typeof(IFiscalPrinterFontStyle))]
    public class FiscalPrinterFontStyle : IFiscalPrinterFontStyle
    {    
        /// <summary>
        /// The style of the text.
        /// </summary>
        public FontStyleType FontStyleType { get; set; }

        /// <summary>
        /// Get dynamic object that hold partner's data.
        /// </summary>
        /// <example>
        /// fontStyle.PartnerData.MS_NewProperty = "value";
        /// </example>
        public dynamic PartnerData { get; set; }
    }
}
