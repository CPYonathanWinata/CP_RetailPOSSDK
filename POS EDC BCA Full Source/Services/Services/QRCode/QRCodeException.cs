/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Abstraction of all exceptions thrown from this namespace.
    /// </summary>
    [Serializable()]
    public class QrcodeException : Exception
    {
        public QrcodeException()
        {}

        public QrcodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public QrcodeException(string message)
            : base(message)
        {
        }
        protected QrcodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
