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
    using System.Globalization;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Settings.FunctionalityProfiles;
    using LSRetailPosis.Transaction;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.DataManager;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// Form used to execute issue linked efdocument action.
    /// </summary>
    [Export("IssueLinkedEFDocumentForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class IssueLinkedEFDocumentForm : frmTouchBase, IInteractionView
    {
        private INoSaleFiscalDocumentTransaction NoSaleFiscalDocumentTransaction { get; set; }
        private IssueLinkedEFDocumentConfirmation Confirmation { get; set; }

        protected IssueLinkedEFDocumentForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the form.
        /// </summary>
        /// <param name="issueLinkedEFDocumentConfirmation">Confirmation instance with the transaction to be used by the issue linked efdocument action</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        [ImportingConstructor]
        public IssueLinkedEFDocumentForm(IssueLinkedEFDocumentConfirmation issueLinkedEFDocumentConfirmation)
            : this()
        {
            this.NoSaleFiscalDocumentTransaction = issueLinkedEFDocumentConfirmation.NoSaleFiscalDocumentTransaction;
            this.Confirmation = issueLinkedEFDocumentConfirmation;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            if (!DesignMode)
            {
                InitializeLabels();
                FillCustomerInfo();
                FillFiscalReceiptInfo();

                EnableOrDisableOkButton();
            }

            base.OnLoad(e);
        }

        private void FillFiscalReceiptInfo()
        {
            var originalRetailTransaction = NoSaleFiscalDocumentTransaction.OriginalTransaction as RetailTransaction;

            if (originalRetailTransaction == null)
            {
                return;
            }

            var fiscalReceiptData = new FiscalReceiptDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

            var fiscalReceipt = fiscalReceiptData.GetFiscalReceipt(originalRetailTransaction.TransactionId, NoSaleFiscalDocumentTransaction.StoreId, NoSaleFiscalDocumentTransaction.TerminalId);

            var fiscalPrinterDataManager = new FiscalPrinterDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

            var fiscalPrinter = fiscalPrinterDataManager.GetFiscalPrinter(fiscalReceipt.StoreId, fiscalReceipt.TerminalId, fiscalReceipt.FiscalPrinterSerialNumber);

            lblReceiptNumber.Text = fiscalReceipt.ReceiptNumber.ToString();
            lblFiscalReceiptNumber.Text = fiscalReceipt.FiscalReceiptNumber.ToString();
            lblFiscalPrinterTerminalNumber.Text = PadZerosToLeftOfTerminalNumber(fiscalPrinter.TerminalNumber);
        }

        private string PadZerosToLeftOfTerminalNumber(int terminalNumber)
        {
            const int MaxNumberOfCharacters = 3;

            return terminalNumber.ToString(CultureInfo.InvariantCulture).PadLeft(MaxNumberOfCharacters, '0');
        }

        private void FillCustomerInfo()
        {
            if (NoSaleFiscalDocumentTransaction.Customer == null || NoSaleFiscalDocumentTransaction.Customer.IsEmptyCustomer())
            {
                lblCustomerName.Text = ApplicationLocalizer.Language.Translate(3941); //No customer was selected
                lblCustomerCNPJCPF.Text = ApplicationLocalizer.Language.Translate(3941); //No customer was selected
                lblCustomerAddress.Text = ApplicationLocalizer.Language.Translate(3941); //No customer was selected
                lblCustomerReceiptEmail.Text = ApplicationLocalizer.Language.Translate(3941); //No customer was selected
                FormatSendByEmail(false);
            }
            else
            {
                lblCustomerName.Text = NoSaleFiscalDocumentTransaction.Customer.Name;
                lblCustomerCNPJCPF.Text = NoSaleFiscalDocumentTransaction.Customer.CNPJCPFNumber;
                lblCustomerAddress.Text = FormatAddress();
                lblCustomerReceiptEmail.Text = NoSaleFiscalDocumentTransaction.ReceiptEmailAddress;
                FormatSendByEmail(!string.IsNullOrEmpty(NoSaleFiscalDocumentTransaction.ReceiptEmailAddress));
            }
        }

        private void FormatSendByEmail(bool value)
        {
            checkSendByEmail.Checked = value;
            checkSendByEmail.Enabled = value;
        }

        private string FormatAddress()
        {
            var address = NoSaleFiscalDocumentTransaction.Customer.PrimaryAddress;

            return string.Format("{0}, {1} - {2}/{3} - CEP: {4}",
                address.StreetName,
                address.StreetNumber,
                address.City,
                address.State,
                address.PostalCode);
        }

        private void InitializeLabels()
        {
            lblTitle.Text = ApplicationLocalizer.Language.Translate(FormResources.IssueReferencedNFe);
            lblReceiptNumberLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.ReceiptNumber));
            lblFiscalReceiptNumberLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.FiscalReceiptNumber));
            lblFiscalPrinterTerminalNumberLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.FiscalPrinterTerminalNumber));
            lblCustomerNameLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.Name));
            lblCustomerCNPJCPFLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.CNPJ_CPF));
            lblCustomerAddressLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.CustomerAddress));
            lblCustomerReceiptEmailLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.ReceiptEmail));
            checkSendByEmailLabel.Text = string.Format("{0}:", ApplicationLocalizer.Language.Translate(FormResources.SendDanfeNfeByEmail));
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (NoSaleFiscalDocumentTransaction.Customer == null || NoSaleFiscalDocumentTransaction.Customer.IsEmptyCustomer())
                {
                    throw new PosisException(ApplicationLocalizer.Language.Translate(FormResources.CustomerMustBeSelected));
                }

                if (NoSaleFiscalDocumentTransaction.Customer.PrimaryAddress.CountryISOCode != Functions.CountryRegion.ToString())
                {
                    throw new PosisException(ApplicationLocalizer.Language.Translate(FormResources.CannotIssueLinkedEFDocumentForForeigner));
                }

                NoSaleFiscalDocumentTransaction.SendByEmail = checkSendByEmail.Checked;

                Confirmation.Confirmed = true;
            }
            catch (PosisException pex)
            {
                POSFormsManager.ShowPOSErrorDialog(pex);
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                POSFormsManager.ShowPOSErrorDialog(new PosisException(FormResources.AnErrorOccurredPerformingTheAction, ex));
            }
        }

        private void btnSelectCustomer_Click(object sender, System.EventArgs e)
        {
            PosApplication.Instance.Services.Customer.Search(NoSaleFiscalDocumentTransaction);

            if (NoSaleFiscalDocumentTransaction.Customer != null && 
                !NoSaleFiscalDocumentTransaction.Customer.IsEmptyCustomer() && 
                NoSaleFiscalDocumentTransaction.Customer.PrimaryAddress.CountryISOCode != Functions.CountryRegion.ToString())
            {
                NoSaleFiscalDocumentTransaction.Customer = null;

                POSFormsManager.ShowPOSErrorDialog(new PosisException(ApplicationLocalizer.Language.Translate(FormResources.CannotIssueLinkedEFDocumentForForeigner)));
            }

            FillCustomerInfo();

            EnableOrDisableOkButton();
        }

        private void EnableOrDisableOkButton()
        {
            btnOk.Enabled = NoSaleFiscalDocumentTransaction.Customer != null && !NoSaleFiscalDocumentTransaction.Customer.IsEmptyCustomer();
        }

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
            return Confirmation as TResults;
        }

        #endregion
    }
}
