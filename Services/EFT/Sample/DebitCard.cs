using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardInterface;
using EFTInterface;
using LSRetailPosis.POSProcesses;
using System.Windows.Forms;
using System.ComponentModel;
using PeripheralsInterface;
using LSRetailPosis;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.EFT.Sample
{
    class DebitCard
    {
    }
}
/*
 * //Microsoft Dynamics AX for Retail POS Plug-ins 
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardInterface;
using EFTInterface;
using LSRetailPosis.POSProcesses;
using System.Windows.Forms;
using System.ComponentModel;
using PeripheralsInterface;
using LSRetailPosis;

namespace EFT
{
    /// <summary>
    /// Implements IPayment interface for debit card payments.
    /// </summary>
    internal class DebitPayment : IPayment
    {
        private CardInfo cardInfo;
        private EFTInfo eftInfo;
        private IPaymentProcessor processor;
        private frmMessage deviceDialog;
        private BackgroundWorker deviceWorker;
        private bool deviceEventReceived;

        /// <summary>
        /// Creates debit payment entity using card information.
        /// </summary>
        /// <param name="cardInfo">Card information</param>
        public DebitPayment(CardInfo cardInfo)
        {
            this.cardInfo = cardInfo;
        }

        /// <summary>
        /// Creates debit payment entity using eftinfo and payment processor.
        /// </summary>
        /// <param name="eftInfo">Eft information</param>
        /// <param name="posTransaction">posTransaction</param>
        /// <param name="processor">Payment processor</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "posTransaction", Justification = "Grandfather")]
        public DebitPayment(EFTInfo eftInfo, object posTransaction, IPaymentProcessor processor)
        {
            //posTransaction is not used but contains a snapshot of all data associated with the current
            //  transaction.  This information may be helpful with EFT processing
            this.eftInfo = eftInfo;
            this.processor = processor;
        }

        #region IPayment Members

         /// <summary>
        /// Get the card data associated with this payment type.
        /// Contains relevant information that has either been read via stripe reader or manually entered.
        /// </summary>
        /// <returns>Card information</returns>
        public CardInfo GetCardInfo()
        {
            return this.cardInfo;
        }

        /// <summary>
        /// Get the Eft data associated with this payment type.
        /// Contains relevant information gathered via payment processing. 
        /// </summary>
        /// <returns>Eft information</returns>
        public EFTInfo GetEftInfo()
        {
            return this.eftInfo;
        }

        /// <summary>
        /// Gets the card information and amount for the specific card.
        /// </summary>
        public void GetPaymentData()
        {
            using (Keyboard.frmDebitCardDialog cardDialog = new Keyboard.frmDebitCardDialog(this.cardInfo))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(cardDialog);
            }

            PinPadWorkflow();
        }

        /// <summary>
        /// Implements workflows supported by the card type.
        /// </summary>
        public void ProcessPayment()
        {
	        using (deviceDialog = new frmMessage(50035, MessageBoxIcon.Information, true))
	        {
		        using (deviceWorker = new BackgroundWorker())
		        {
			        // Create a background worker thread to interact with the payment device
			        deviceWorker.DoWork += new DoWorkEventHandler(OnWorkerDoWork);
			        deviceWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnRunWorkerCompleted);

			        //listen to the OnShow event, so we can kick-off the background thread AFTER the dialog is visible.
			        deviceDialog.Shown += new EventHandler(OnProcessPaymentDialogShown);
			        deviceDialog.ShowDialog();

			        //At this point the worker thread terminated, which caused the dialog to close, 
			        //and both can be safely disposed-of at the end of the 'using' scope.
		        }
	        }            
        }

        /// <summary>
        /// Sets eft information when on worker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
	        this.eftInfo.CardNumberHidden = true;
	        processor.Authorize();
	        this.eftInfo = processor.GetEftInfo();            
        }

        /// <summary>
        /// Event is generated when on run worker completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
	        deviceDialog.UseWaitCursor = false;
	        deviceDialog.Close();
        }

        private void OnProcessPaymentDialogShown(object sender, EventArgs e)
        {
	        deviceDialog.UseWaitCursor = true;
	        deviceWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Voids the card payment by establishing a connection with the configured payment processor.
        /// Once a connection to the broker is established, it attempts to void the card payment.
        /// </summary>
        public void VoidPayment()
        {
            this.eftInfo.Authorized = false;
        }

        /// <summary>
        /// Captures the card payment by establishing a connection with the configured payment processor.
        /// Once a connection to the broker is established, it attempts to capture the authorized card payment.
        /// </summary>
        public void CapturePayment()
        {
            processor.Capture();
        }

		/// <summary>
		/// Print the decline receipt.
		/// </summary>
		public void PrintDeclineReceipt()
		{
			Printing.PrintDeclineReceipt(this.eftInfo);
		}

        #endregion

        /// <summary>
        /// Implement pinpad workflow for debit card.
        /// </summary>
        private void PinPadWorkflow()
        {
            DialogResult result;

            if (cardInfo.CardReady)
            {
                // Init the cardInfo to indicate no data from device
                this.cardInfo.CardReady = false;
                this.cardInfo.EncryptedPIN = string.Empty;
                this.cardInfo.AdditionalSecurityInformation = string.Empty;

                result = DialogResult.Retry;
                this.deviceEventReceived = false;
                while ((result == DialogResult.Retry) && !deviceEventReceived)
                {
                    using (deviceDialog = new frmMessage(50001, MessageBoxButtons.RetryCancel, MessageBoxIcon.Information, true))
                    {   // Shoehorn an event model into the existing form

                        this.deviceEventReceived = false;

                        // Subscribe to the device and form events
                        ApplicationServices.IPeripherals.PinPad.EntryCompleteEvent += new PinPadEntryCompleteEventHandler(Pinpad_EntryCompleteEvent);
                        deviceDialog.Shown += new EventHandler(deviceDialog_Shown);

                        // Show the form
                        result = deviceDialog.ShowDialog();

                        // Unsubscribe from the device and form events
                        deviceDialog.Shown -= new EventHandler(deviceDialog_Shown);
                        ApplicationServices.IPeripherals.PinPad.EntryCompleteEvent -= new PinPadEntryCompleteEventHandler(Pinpad_EntryCompleteEvent);

                        try
                        {
                            if (!deviceEventReceived)
                            {
                                ApplicationServices.IPeripherals.PinPad.EndTransaction(false);
                            }
                        }
                        catch (Exception)
                        {   // Device failure
                            // Ignore...
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event will be fired when pinpad entry is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Pinpad_EntryCompleteEvent(object sender, PinPadEventArgs e)
        {
            this.deviceEventReceived = true;

            this.cardInfo.CardReady =
                e.DataEvent &&
                (e.Status == PinPadEntryStatus.Success);
            this.cardInfo.EncryptedPIN = e.EncryptedPIN;
            this.cardInfo.AdditionalSecurityInformation = e.AdditionalSecurityInformation;


            try
            {
                ApplicationServices.IPeripherals.PinPad.EndTransaction(this.cardInfo.CardReady);
            }
            catch (Exception)
            {   // Device failure
                // Ignore...
            }


            // Once the device is completed (or errors out), we can then close the dialog
            // dialogResult is not used if in this case.
            deviceDialog.Close();
        }

        /// <summary>
        /// Event will be fired to show device dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deviceDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                ApplicationServices.IPeripherals.PinPad.BeginTransaction(cardInfo.Amount, cardInfo.CardNumber);
            }
            catch
            {
                // Device Failure
                deviceDialog.DialogResult = DialogResult.Cancel;
                deviceDialog.Close();
            }
        }
    }
}
 */