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
    /// String formatting class for Customer Order SalesStatus enum
    /// </summary>
    internal class CustomerOrderStatusFormatter : IFormatProvider, ICustomFormatter
    {
        private readonly string Unknown;
        private readonly string Confirmed;
        private readonly string Created;
        private readonly string Processing;
        private readonly string Lost;
        private readonly string Canceled;
        private readonly string Sent;
        private readonly string Delivered;
        private readonly string Invoiced;

        /// <summary>
        /// Formats status settings.
        /// </summary>
        public CustomerOrderStatusFormatter()
        {
            Unknown     = ApplicationLocalizer.Language.Translate(56375); // "None";
            Confirmed   = ApplicationLocalizer.Language.Translate(56404); // "Confirmed";
            Created     = ApplicationLocalizer.Language.Translate(56376); // "Created";
            Processing  = ApplicationLocalizer.Language.Translate(56402); // "Processing";
            Lost        = ApplicationLocalizer.Language.Translate(56403); // "Lost";
            Canceled    = ApplicationLocalizer.Language.Translate(56379); // "Canceled";
            Sent        = ApplicationLocalizer.Language.Translate(56405); // "Sent";
            Delivered   = ApplicationLocalizer.Language.Translate(56377); // "Delivered";
            Invoiced    = ApplicationLocalizer.Language.Translate(56378); // "Invoiced";
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
            if (arg is int || arg is SalesStatus)
            {
                switch ((int)arg)
                {
                    case (int)SalesStatus.Unknown:      return this.Unknown;
                    case (int)SalesStatus.Confirmed:    return this.Confirmed;
                    case (int)SalesStatus.Created:      return this.Created;
                    case (int)SalesStatus.Processing:   return this.Processing;
                    case (int)SalesStatus.Lost:         return this.Lost;
                    case (int)SalesStatus.Canceled:     return this.Canceled;
                    case (int)SalesStatus.Sent:         return this.Sent;
                    case (int)SalesStatus.Delivered:    return this.Delivered;
                    case (int)SalesStatus.Invoiced:     return this.Invoiced;
                    default:                            return string.Empty;
                }
            }
            return (arg == null) ? string.Empty : arg.ToString();
        }
    }
}
