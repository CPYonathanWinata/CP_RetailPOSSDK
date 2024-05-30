/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System.Drawing.Printing;
using System.Drawing;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export("LoyaltyCardForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LoyaltyCardForm : frmTouchBase, IInteractionView
    {
        int Offset = 0;
        #region Members

        /// <summary>
        /// Gets or sets the view model of this form.
        /// </summary>
        private LoyaltyCardFormViewModel ViewModel { get; set; }

        private readonly LoyaltyCardFormContext context;

        #endregion

        #region Construction

        /// <summary>
        /// Default ctor.
        /// </summary>
        protected LoyaltyCardForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        [ImportingConstructor]
        public LoyaltyCardForm(LoyaltyCardConfirmation loyaltyCardConfirmation) : this()
        {
            if (loyaltyCardConfirmation == null)
            {
                throw new ArgumentNullException("loyaltyCardConfirmation");
            }

            context = loyaltyCardConfirmation.Context;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                ViewModel = new LoyaltyCardFormViewModel(context);

                bindingSource.Add(ViewModel);

                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmLoyaltyCard are reserved at 55500 - 55599
                //
                TranslateAndSetupControls();

                // Hook up MSR/Scanner
                ViewModel.HookUpPeripherals();
                LoyaltyCardFormViewModel.EnablePosDevices();
                ViewModel.ClearViewModelProperties();
            }

            base.OnLoad(e);
             
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            LoyaltyCardFormViewModel.DisablePosDevices();
             

        }

        private void TranslateAndSetupControls()
        {
            this.Text = lblHeading.Text = ViewModel.GetFormTitle();            
            padLoyaltyCardNumber.PromptText = GetLoyaltyCardNumberLabelText(); //modified by rs 15 June 2022
            lblLoyaltyCardBalanceTitle.Text = GetLoyaltyCardBalancePointsLabelText();
            lblLoyaltyCardCustomerNameTitle.Text = GetLoyaltyCardCustomerNameLabelText();
            lblLoyaltyCardStatusTitle.Text = GetLoyaltyCardStatusLabelText();
            lblLoyaltyCardTypeTitle.Text = GetLoyaltyCardTypeLabelText();
            lblLoyaltyCardIssuedPointsTitle.Text = GetLoyaltyCardIssuedPointsLabelText();
            lblLoyaltyCardUsedPointsTitle.Text = GetLoyaltyCardUsedPointsLabelText();
            lblLoyaltyCardExpiredPointsTitle.Text = GetLoyaltyCardExpiredPointsLabelText();

            this.btnOk.Text = GetPrintReceiptButtonLabelText();
            btnCancel.Text = GetCancelButtonLabelText();
            btnGet.Text = ViewModel.GetGetButtonLabelText();
        }

        #endregion

        #region Methods

        private void PreExecute()
        {
            ViewModel.CardNumber = padLoyaltyCardNumber.EnteredValue;

            LoyaltyCardFormViewModel.DisablePosDevices();

            try
            {
                ViewModel.PreExecute();
            }
            catch (PosisException px)
            {
                HandlePosIsException(px);
            }
            catch (LoyaltyCardException gex)
            {
                HandleLoyaltyCardException(gex);
            }

            LoyaltyCardFormViewModel.EnablePosDevices();
        }

        private bool Execute()
        {
            bool result = false;

            LoyaltyCardFormViewModel.DisablePosDevices();

            try
            {
                result = ViewModel.Execute();
                padLoyaltyCardNumber.SelectAll();
            }
            catch (PosisException px)
            {
                HandlePosIsException(px);
            }
            catch (LoyaltyCardException lex)
            {
                HandleLoyaltyCardException(lex);
            }

            LoyaltyCardFormViewModel.EnablePosDevices();

            return result;
        }

        private void HandleLoyaltyCardException(LoyaltyCardException ex)
        {
            ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            ShowErrorMessage(ex.Message);
        }

        private void HandlePosIsException(PosisException ex)
        {
            string message = string.Format(ApplicationLocalizer.Language.Translate(55512),
                    LSRetailPosis.Settings.ApplicationSettings.ShortApplicationTitle);

            ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            ShowErrorMessage(message);
        }

        private void ShowErrorMessage(string message)
        {
            LoyaltyCardFormViewModel.ShowErrorMessage(message);
            padLoyaltyCardNumber.SelectAll();
        }

        private static string GetLoyaltyCardNumberLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardNumber);
        }

        private static string GetLoyaltyCardCustomerNameLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardCustomerName);
        }

        private static string GetLoyaltyCardStatusLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardStatus);
        }

        private static string GetLoyaltyCardTypeLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardType);
        }

        private static string GetLoyaltyCardIssuedPointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardIssuedPoints);
        }

        private static string GetLoyaltyCardUsedPointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardUsedPoints);
        }

        private static string GetLoyaltyCardExpiredPointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardExpiredPoints);
        }

        private static string GetLoyaltyCardBalancePointsLabelText()
        {
            return ApplicationLocalizer.Language.Translate(LoyaltyCardFormViewModel.Resources.LoyaltyCardBalance);
        }

        private static string GetPrintReceiptButtonLabelText()
        {
            return ApplicationLocalizer.Language.Translate(55410); // Print receipt
        }

        private static string GetCancelButtonLabelText()
        {
            return ApplicationLocalizer.Language.Translate(55404); // Cancel
        }

        #endregion

        #region Event handlers

        private void padLoyaltyCardId_EnterButtonPressed()
        {
            if (padLoyaltyCardNumber.EnteredValue.Length > 0)
            {
                PreExecute();
            }
        }

        private void padLoyaltyCardId_CardSwept(string[] trackParts)
        {
            if ((trackParts != null) && (trackParts.Length > 0))
            {
                padLoyaltyCardNumber.EnteredValue = trackParts[0];
                padLoyaltyCardNumber.Refresh();
                PreExecute();
            }
        }

        private void btnValidateLoyaltyCard_Click(object sender, EventArgs e)
        {
            PreExecute();
        }

        private void ProcessScannedItem(IScanInfo scanInfo)
        {
            if (scanInfo.ScanData.Length > 0)
            {
                padLoyaltyCardNumber.EnteredValue = scanInfo.ScanData;
                padLoyaltyCardNumber.Refresh();
                PreExecute();
            }
        }

        private void ProcessSwipedCard(ICardInfo cardInfo)
        {
            padLoyaltyCardId_CardSwept(cardInfo.Track2Parts);
        }

        private void numPad_EnterButtonPressed()
        {
            btnOk_Click(this, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            if (padLoyaltyCardNumber.EnteredValue.Length > 0
                //&& Execute()
                )
            {

                this.DialogResult = DialogResult.OK;
               
                // print here
                string s = this.cpPrintReceipt();
                
                PrintDocument p = new PrintDocument();
                PrintDialog pd = new PrintDialog();
                PaperSize psize = new PaperSize("Custom", 100, Offset + 236);                 
                Margins margins = new Margins(0, 0, 0, 0); 

                pd.Document = p;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.Document.DefaultPageSettings.Margins = margins;
                p.DefaultPageSettings.PaperSize.Width = 400;
                p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                {
                    e1.Graphics.DrawString(s, new Font("Calibri", 8, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

                };
                try
                {
                    //for (int i = 1; i <= 2; i++)
                    //{
                        p.Print();
                   // }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception Occured While Printing", ex);
                }
                // End add NEC Hmz
                
                this.Close();
            }
             
        }

        #endregion

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type.</typeparam>
        /// <param name="args">Notification object.</param>
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Returns the results of the interaction call.
        /// </summary>
        /// <typeparam name="TResults">Type of result confirmation object.</typeparam>
        /// <returns>Result confirmation object.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new LoyaltyCardConfirmation
            {
                Confirmed = this.DialogResult == System.Windows.Forms.DialogResult.OK
            } as TResults;
        }
        
        public string cpPrintReceipt()
        {
            ILoyaltyCardData cd;
            string comment = "";

            cd = PosApplication.Instance.Services.Loyalty.GetLoyaltyBalanceInfo(padLoyaltyCardNumber.EnteredValue);
            for (int i = 1; i <= 10; i++)
            {
                comment += Environment.NewLine;
            }

            comment += "                                                Loyalty Card" + Environment.NewLine;
            comment += "                                                    ********************************************************" + Environment.NewLine;
            comment += "                                                Loyalty Card    :" + "****" + cd.CardNumber.Substring(cd.CardNumber.Length -4,4) + Environment.NewLine;
            comment += "                                                Customer        :" + cd.CustomerName + Environment.NewLine;
            comment += "                                                Card Type       :" + cd.CardTypeString + Environment.NewLine;

            Offset = Offset + 312;
            comment += Environment.NewLine + "                                                Issued Points   :" + cd.IssuedPoints.ToString("#,##0.00") + Environment.NewLine;
            comment += "                                                Used Points     :" + cd.UsedPoints.ToString("#,##0.00") + Environment.NewLine;
            comment += "                                                Expired Points :" + cd.ExpiredPoints.ToString("#,##0.00") + Environment.NewLine;
            comment += Environment.NewLine + "                                                Balance Points :" + cd.BalancePoints.ToString("#,##0.00") + Environment.NewLine;
            comment += "                                                        ********************************************************" + Environment.NewLine;

            return comment;
        
        }

    }
}
