/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Runtime.Serialization;
using LSRetailPosis;
using System.Security.Permissions;

namespace Microsoft.Dynamics.Retail.FiscalRegistrationServices
{
    /// <summary>
    /// Class that extends PosisException to provide an specific Fiscal register exception.
    /// </summary>
    [Serializable]
    public class FiscalRegisterException : PosisException
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public FiscalRegisterException()
        { }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="message"></param>
        public FiscalRegisterException(string message)
            : base(message)
        { }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="errorNumber"></param>
        public FiscalRegisterException(int errorNumber)
            : this(Resources.Translate(errorNumber))
        {
            ErrorNumber = errorNumber;
            ErrorMessageNumber = errorNumber;
        }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <param name="args"></param>
        public FiscalRegisterException(int errorNumber, params object[] args)
            : this(Resources.Translate(errorNumber, args))
        {
            ErrorNumber = errorNumber;
            ErrorMessageNumber = errorNumber;
        }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public FiscalRegisterException(int errorNumber, Exception innerException, params object[] args)
            : this(Resources.Translate(errorNumber, args), innerException)
        {
            ErrorNumber = errorNumber;
            ErrorMessageNumber = errorNumber;
        }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <param name="errorMessageNumber"></param>
        /// <param name="args"></param>
        public FiscalRegisterException(int errorNumber, int errorMessageNumber, params object[] args)
            : base(Resources.Translate(errorMessageNumber, args))
        {
            ErrorNumber = errorNumber;
            ErrorMessageNumber = errorMessageNumber;
        }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <param name="errorMessageNumber"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public FiscalRegisterException(int errorNumber, int errorMessageNumber, Exception innerException, params object[] args)
            : base(Resources.Translate(errorMessageNumber, args), innerException)
        {
            ErrorNumber = errorNumber;
            ErrorMessageNumber = errorMessageNumber;
        }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FiscalRegisterException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// FiscalRegisterException constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FiscalRegisterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
        }

        /// <summary>
        /// Gets or sets a generic object that partners can create to extend the RetailTransaction.
        /// </summary>
        /// <value>Any object created by the partner that is marked as Serializable.</value>
        public object PartnerObject { get; set; }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("PartnerObject", PartnerObject);
        }
    }
}