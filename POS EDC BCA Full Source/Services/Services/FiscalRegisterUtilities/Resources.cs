/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using LSRetailPosis;

namespace Microsoft.Dynamics.Retail.FiscalRegistrationServices
{
    /// <summary>
    /// Resources class is use to retrieve strings from the resource file.
    /// The current implementation is to retrieve them from the POS
    /// ApplicationLocalizer.Language.Translate and leverage the same features
    /// available for other POS modules.
    /// 
    /// Resource string id ranges:
    ///     - Fiscal Register = 107200 to 107300
    /// </summary>

    public static class Resources
    {
        /// <summary>
        /// Tax mapping is not set up.
        /// </summary>
        public const int TaxMappingIsNotSet = 86432;
        /// <summary>
        /// Error in configuration file at line {0}. {1}
        /// </summary>
        public const int ConfigFileValidationError = 86425;
        /// <summary>
        /// Configuration file '{0}' has not been found.
        /// </summary>
        public const int ConfigFileNotFound = 86426;
        /// Specified configuration file does not exist in the database.
        /// </summary>
        public const int ConfigFileDoesNotExistInDB = 107206;
        /// <summary>
        /// Error occured while reading configuration file. {0} Contact system administrator.
        /// </summary>
        public const int FailedToLoadConfigFile = 86408;
        /// <summary>
        /// The control unit is not initialized.
        /// </summary>
        public const int ControlUnitNotInitialized = 107202;
        /// <summary>
        /// Failed to connect to the control unit. The error code is {0} and the connection string is {1}.
        /// </summary>
        public const int CannotConnectToControlUnit = 107203;
        /// <summary>
        /// Failed to close the connection to the control unit. The error code is {0}.
        /// </summary>
        public const int FailedToCloseConnection = 107205;
        /// <summary>
        /// A fiscal register configuration cannot be found for this POS register. Contact your system administrator.
        /// </summary>
        public const int FiscalRegisterConfigCannotBeFound = 107206;
        /// <summary>
        /// The tax code {0} is mapped to multiple control unit VAT groups. Contact your system administrator.
        /// </summary>
        public const int TaxCodeMappedToMultipleControlUnit = 107207;
        /// <summary>
        /// The tax code {0} is mapped to the control unit VAT group number {1}, which is not within a valid range. Contact your system administrator.
        /// </summary>
        public const int TaxCodeMappingIsNotInValidRange = 107208;
        /// <summary>
        /// Tax codes with unequal tax rates are mapped to the control unit VAT group {0}. Contact your system administrator.
        /// </summary>
        public const int TaxCodeWithUnequalTaxRatesMappedToOneControlUnit = 107209;
        /// <summary>
        /// The tax code {0} does not exist in the database. Contact your system administrator.
        /// </summary>
        public const int TaxCodeDoesNotExistInDatabase = 107210;
        /// <summary>
        /// The tax code {0} does not have a corresponding control unit VAT group. Contact your system administrator.
        /// </summary>
        public const int TaxCodeDoesNotHaveCorrespondingControlUnitVATGroup = 107211;
        /// <summary>
        /// Failed to check the status of the control unit. The error code is {0}. The extended error code is {1}.
        /// </summary>
        public const int FailedToCheckStatus = 107212;
        /// <summary>
        /// Failed to register POS in the control unit. The error code is {0}. The extended error code is {1}.
        /// </summary>
        public const int FailedToRegisterPOS = 107213;
        /// <summary>
        /// Failed to start a new receipt in the control unit. The error code is {0}. The extended error code is {1}.
        /// </summary>
        public const int FailedToStartNewReceipt = 107214;
        /// <summary>
        /// Failed to send the receipt to the control unit. The error code is {0}. The extended error code is {1}.
        /// </summary>
        public const int FailedToRegisterReceiptInControlUnit = 107215;
        /// <summary>
        /// Failed to parse the serialized transaction.
        /// </summary>
        public const int FailedToParseSerializedTransaction = 107216;
        /// <summary>
        /// Control code
        /// </summary>
        public const int ControlCode = 103386;
        /// <summary>
        /// Control unit serial number
        /// </summary>
        public const int ControlUnitSerialNumber = 103387;

        /// <summary>
        /// Retrieve the string identified by resourceId and format the final
        /// message based on the args based.
        /// </summary>
        /// <param name="resourceId">The resource identifier to look for.</param>
        /// <param name="args">The optional args to format message with.</param>
        /// <returns>The formated message.</returns>
        public static string Translate(int resourceId, params object[] args)
        {
            return ApplicationLocalizer.Language.Translate(resourceId, args);
        }
    }
}
