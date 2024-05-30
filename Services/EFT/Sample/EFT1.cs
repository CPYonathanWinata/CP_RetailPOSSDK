/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.ComponentModel.Composition;
using System.Data.SqlClient;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

namespace EFT
{
    /// <summary>
    /// EFT service provider for processing card payments.
    /// </summary>
    [Export(typeof(IEFTV2))]
    public class EFT1 : IEFTV2
    {
        #region Member variables

        protected SqlConnection Connection { get; set; }
        protected string DataAreaId { get; set; }
        protected string ServerName { get; set; }
        protected string ServerPort { get; set; }
        protected string CompanyId { get; set; }
        protected string TerminalId { get; set; }
        protected string Password { get; set; }
        protected string StoreId { get; set; }
        protected string UserId { get; set; }
        protected int Configuration { get; set; }
        protected int Environment { get; set; }
        protected string ProfileId { get; set; }
        protected int TestMode { get; set; }

        [Import]
        public IApplication Application { get; set; }

        #endregion

        #region IEFT Members

        /// <summary>
        /// Generate Credit card token is created by a payment SDK processor and represents the credit card number
        /// and is used within AX to process customer order securely
        /// Needs to internally set the posTransaction.CreditCardToken
        /// </summary>
        /// <param name="amount">Amount to display.</param>
        /// <param name="posTransaction">Reference to an IPosTransaction object.</param>
        public void GenerateCardToken(decimal amount, IPosTransaction posTransaction)
        {
            posTransaction.CreditCardToken = "dummy token";
            System.Windows.Forms.MessageBox.Show("Generate Card Token");
        }

        /// <summary>
        /// Gets the card information and amount for the specific card.
        /// </summary>
        /// <param name="cardInfo">Card information</param>
        /// <returns>True if the system should continue with the authorization process and false otherwise.</returns>
        public bool GetCardInfoAndAmount(ref ICardInfo cardInfo)
        {
            // Show UI. Input Card number, Amount, etc.

            cardInfo.CardType = CardTypes.InternationalCreditCard;
            cardInfo.CardNumber = "1234-5678-9012-3456";
            cardInfo.ExpDate = "05/10";
            cardInfo.CardReady = true;

            //testing only
            System.Windows.Forms.MessageBox.Show("Get Card Info and Amount");
            //end testing only

            return cardInfo.CardReady;
        }

        /// <summary>
        /// Processes the card payment by establishing a connection with the configured payment processor.
        /// </summary>
        /// <param name="eftInfo">Reference to an EftInfo object.</param>
        /// <param name="posTransaction">Current transaction.</param>
        /// <returns>True if the processing succeeded.</returns>
        public bool ProcessCardPayment(ref IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            eftInfo.Authorized = true;
            eftInfo.IsPendingCapture = true;

            System.Windows.Forms.MessageBox.Show("Process Card Payment");

            return eftInfo.Authorized;
        }

        /// <summary>
        /// Voids the card payment by establishing a connection with the configured payment processor.
        /// </summary>
        /// <param name="eftInfo">Reference to an EftInfo object.</param>
        /// <param name="posTransaction">Current transaction.</param>
        /// <returns>True if the processing succeeded.</returns>
        public bool VoidTransaction(ref IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            eftInfo.CardNumberHidden = true;
            eftInfo.Authorized = true;

            System.Windows.Forms.MessageBox.Show("Void Transaction");

            return eftInfo.Authorized;
        }

        /// <summary>
        /// Identifies the card if a match with pre-configured card types in not found by the application.
        /// </summary>
        /// <param name="cardInfo">Reference to an EftInfo object.</param>
        /// <param name="eftInfo">Reference to an eftInfo object.</param>
        public void IdentifyCard(ref ICardInfo cardInfo, ref IEFTInfo eftInfo)
        {
            if (cardInfo != null)
            {
                cardInfo.CardType = CardTypes.Unknown;
            }
            System.Windows.Forms.MessageBox.Show("Identify Card");
        }

        /// <summary>
        /// Captures the card payment by establishing a connection with the configured payment processor.
        /// </summary>
        /// <param name="eftInfo">Reference to an EftInfo object.</param>
        /// <param name="posTransaction">Current transaction.</param>
        public void CapturePayment(IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
        }

        /// <summary>
        /// Get transaction token for authorized transaction (Secure card storage).
        /// </summary>
        /// <param name="eftInfo">EftInfo object.</param>
        /// <param name="posTransaction">Reference to an IPosTransaction object.</param>
        public void GetTransactionToken(IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
        }

        /// <summary>
        /// Return a SigCap service (if availbale)
        /// </summary>
        /// <returns>null if the service is not provided</returns>
        public ISignatureCapture GetSignatureCapture()
        {
            return null;
        }

        /// <summary>
        /// Return a PIN Pad service (if available)
        /// </summary>
        /// <returns>null if service is not provided</returns>
        public IPinPad GetPinPad()
        {
            return null;
        }

        #endregion
    }
}
