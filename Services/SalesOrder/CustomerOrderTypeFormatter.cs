/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSRetailPosis;
using LSRetailPosis.Transaction;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    /// <summary>
    /// String Formatting class for CustomerOrderType enum
    /// </summary>
    internal class CustomerOrderTypeFormatter : IFormatProvider, ICustomFormatter
    {
        private readonly string SalesOrder;
        private readonly string Quote;

        /// <summary>
        /// Formats status settings.
        /// </summary>
        public CustomerOrderTypeFormatter()
        {
            SalesOrder   = ApplicationLocalizer.Language.Translate(56373); // "Sales Order";
            Quote        = ApplicationLocalizer.Language.Translate(56374); // "Quote";
        }

        /// <summary>
        /// The GetFormat method of the IFormatProvider interface.
        /// This must return an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetFormat(System.Type type)
        {
            return this;
        }

        /// <summary>
        /// The Format method of the ICustomFormatter interface.
        /// This must format the specified value according to the specified format settings.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is int || arg is CustomerOrderType)
            {
                switch ((int)arg)
                {
                    case (int)CustomerOrderType.Quote:      return this.Quote;
                    case (int)CustomerOrderType.SalesOrder: return this.SalesOrder;
                    default:                            return string.Empty;
                }
            }
            return (arg == null) ? string.Empty : arg.ToString();
        }
    }
}
