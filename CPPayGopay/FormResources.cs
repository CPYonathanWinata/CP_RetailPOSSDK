/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// FormResources class is used to retrieve strings from the resource file.
    /// The current implementation is to retrieve them from the POS
    /// ApplicationLocalizer.Language.Translate and leverage the same features
    /// available for other POS modules.
    /// </summary>
    public static class FormResources
    {
        /// <summary>
        /// An error occurred performing the action.
        /// </summary>
        public const int AnErrorOccurredPerformingTheAction = 1000;

        /// <summary>
        /// CNPJ/CPF
        /// </summary>
        public const int CNPJ_CPF = 86206;

        /// <summary>
        /// Customer address
        /// </summary>
        public const int CustomerAddress = 103241;

        /// <summary>
        /// A customer must be selected.
        /// </summary>
        public const int CustomerMustBeSelected = 86612;

        /// <summary>
        /// Fiscal printer serial number
        /// </summary>
        public const int FiscalPrinterTerminalNumber = 86625;

        /// <summary>
        /// Fiscal receipt number
        /// </summary>
        public const int FiscalReceiptNumber = 89000;

        /// <summary>
        /// Issue referenced NF-e
        /// </summary>
        public const int IssueReferencedNFe = 86613;

        /// <summary>
        /// Name
        /// </summary>
        public const int Name = 86153;
        
        /// <summary>
        /// Receipt e-mail
        /// </summary>
        public const int ReceiptEmail = 85586;

        /// <summary>
        /// Send DANFE NF-e by e-mail
        /// </summary>
        public const int SendDanfeNfeByEmail = 85585;

        /// <summary>
        /// Receipt number
        /// </summary>
        public const int ReceiptNumber = 89001;

        /// <summary>
        /// The transaction could not be found.
        /// </summary>
        public const int TransactionCouldNotBeFound = 3681;

        /// <summary>
        /// Can not issue the linked electronic fiscal document to a foreign customer.
        /// </summary>
        public const int CannotIssueLinkedEFDocumentForForeigner = 85597;
    }
}
