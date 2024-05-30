/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Receipt
{
    using System.ComponentModel.Composition;
    using Contracts.BusinessObjects;
    using Contracts.DataEntity;
    using Contracts.Services;

    /// <summary>
    /// Receipt service allow partners to render custom messages during the sale and add them to the receipt layout.
    /// </summary>
    [Export(typeof(IReceipt))]
    public sealed class Receipt : IReceipt
    {
        /// <summary>
        /// Get the promotional message before end receipt
        /// </summary>
        /// <param name="posTransaction">The pos transaction.</param>
        /// <param name="receiptMessage">The receipt message class.</param>
        public void GetPromotionalMessage(IPosTransaction posTransaction, IReceiptMessage receiptMessage)
        {
            //
            // Left empty on purpose
            //
        }
    }
}
