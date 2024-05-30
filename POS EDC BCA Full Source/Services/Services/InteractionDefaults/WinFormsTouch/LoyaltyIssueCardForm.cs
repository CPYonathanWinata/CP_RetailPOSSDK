/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Forms;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Transaction;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// Summary description for frmLoyaltyIssueCard.
    /// </summary>
    [Export("LoyaltyCardIssueForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LoyaltyIssueCardForm : frmTouchBase, IInteractionView
    {
        private PosTransaction PosTransaction { get; set; }

        #region Constructor and Destructor

        protected LoyaltyIssueCardForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Creates loyalty issue card form.
        /// </summary>
        /// <param name="loyaltyCardIssuedConfirmation"></param>
        /// <exception cref="ArgumentNullException">Throws exception if <paramref name="loyaltyCardIssuedConfirmation"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Throws exception if <paramref name="loyaltyCardIssuedConfirmation.PosTransaction"/> is null.</exception>
        [ImportingConstructor]
        public LoyaltyIssueCardForm(LoyaltyCardIssuedConfirmation loyaltyCardIssuedConfirmation)
            : this()
        {
            if (loyaltyCardIssuedConfirmation == null)
            {
                throw new ArgumentNullException("LoyaltyCardIssuedConfirmation");
            }

            this.PosTransaction = loyaltyCardIssuedConfirmation.PosTransaction as PosTransaction;

            if (this.PosTransaction == null)
            {
                throw new InvalidOperationException("LoyaltyCardIssuedConfirmation.PosTransaction");
            }

            this.UpdateCustomer();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Get all text through the Translation function in the ApplicationLocalizer
                // TextID's for Loyalty are reserved at 50075 - 50099 (in use )

                btnCancel.Text = ApplicationLocalizer.Language.Translate(1440); //Cancel
                btnOk.Text = ApplicationLocalizer.Language.Translate(1201);//Ok

                labelIssueCard.Text = ApplicationLocalizer.Language.Translate(50100); // Issue loyalty card
                btnCustomerSearch.Text = ApplicationLocalizer.Language.Translate(1447); // Change customer
                labelCustomerId.Text = ApplicationLocalizer.Language.Translate(1032369); // Account
                labelCustomerName.Text = ApplicationLocalizer.Language.Translate(1505); // Name
                labelAccountInfo.Text = ApplicationLocalizer.Language.Translate(1483); // Customer
                numPad1.PromptText = ApplicationLocalizer.Language.Translate(50101); // Card number

                this.EnablePeripherals();

                numPad1.Focus();
            }

            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            this.DisablePeripherals();

            base.OnFormClosed(e);
        }


        private void EnablePeripherals()
        {
            this.DisablePeripherals();

            Microsoft.Dynamics.Retail.Pos.SystemCore.PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent += this.ProcessScannedCard;
            PosApplication.Instance.Services.Peripherals.MSR.MSRMessageEvent += this.ProcessSweptCard;

            PosApplication.Instance.Services.Peripherals.Scanner.ReEnableForScan();
            PosApplication.Instance.Services.Peripherals.MSR.EnableForSwipe();
        }

        private void DisablePeripherals()
        {
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent -= this.ProcessScannedCard;
            PosApplication.Instance.Services.Peripherals.MSR.MSRMessageEvent -= this.ProcessSweptCard;
            PosApplication.Instance.Services.Peripherals.Scanner.DisableForScan();
            PosApplication.Instance.Services.Peripherals.MSR.DisableForSwipe();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Events

        private void numPad1_EnterButtonPressed()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            CustomerSearch custSearch = new CustomerSearch();
            custSearch.OperationID = PosisOperations.CustomerSearch;
            custSearch.POSTransaction = this.PosTransaction;
            custSearch.RunOperation();

            this.UpdateCustomer();
        }

        private void UpdateCustomer()
        {
            RetailTransaction retailPosTransaction = (RetailTransaction)PosTransaction;

            if (!(retailPosTransaction.Customer.IsEmptyCustomer())
                && retailPosTransaction.Customer.Blocked == BlockedEnum.No)
            {
                labelCustomerIdValue.Text = retailPosTransaction.Customer.CustomerId;
                labelCustomerNameValue.Text = retailPosTransaction.Customer.Name;

                retailPosTransaction.CalcTotals();
            }
        }

        private void ProcessScannedCard(IScanInfo scanInfo)
        {
            this.numPad1.EnteredValue = scanInfo.ScanData;
            PosApplication.Instance.Services.Peripherals.Scanner.ReEnableForScan();
        }

        private void ProcessSweptCard(ICardInfo cardInfo)
        {
            this.numPad1.EnteredValue = cardInfo.CardNumber;
            PosApplication.Instance.Services.Peripherals.MSR.EnableForSwipe();
        }

        #endregion

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type</typeparam>
        /// <param name="args">Notification</param>
        public void Initialize<TArgs>(TArgs args)
            where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call
        /// </summary>
        /// <typeparam name="TResults"></typeparam>
        /// <returns>Returns the TResults object</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new LoyaltyCardIssuedConfirmation
            {
                LoyaltyCardNumber = this.numPad1.EnteredValue,
                Confirmed = this.DialogResult == DialogResult.OK,
            } as TResults;
        }

        #endregion
    }
}