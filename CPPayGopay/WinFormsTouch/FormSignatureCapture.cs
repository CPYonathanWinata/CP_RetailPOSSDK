/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using LSRetailPosis.POSProcesses;
using LSRetailPosis;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Notification.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Signature capture control.
    /// </summary>
    [Export("SignatureCaptureForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FormSignatureCapture : frmTouchBase, IInteractionView
    {
        private ISignatureCapture signatureCapture;
        public ReadOnlyCollection<Point> Points { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected FormSignatureCapture()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args">Initialization args</param>
        [ImportingConstructor]
        public FormSignatureCapture(SignatureCaptureConfirmation args) 
            : this()
        {
            if (args == null)
                throw new ArgumentNullException("args");

            this.signatureCapture = args.Device;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (this.IsSignatureCaptureReady())
                {
                    this.signatureCapture.CaptureCompleteEvent += new SignatureCaptureCompleteEventHandler(signatureCapture_CaptureCompleteEvent);
                    this.signatureCapture.CaptureErrorEvent += new SignatureCaptureErrorMessageEventHandler(signatureCapture_CaptureErrorEvent);
                    this.lslNotReady.Visible = false;
                    this.signatureCapture.BeginCapture();
                }
                else
                {
                    this.lslNotReady.Visible = true;
                }

                this.TranslateLabels();
            }

            base.OnLoad(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.IsSignatureCaptureReady())
            {
                this.signatureCapture.CaptureCompleteEvent -= new SignatureCaptureCompleteEventHandler(signatureCapture_CaptureCompleteEvent);
                this.signatureCapture.CaptureErrorEvent -= new SignatureCaptureErrorMessageEventHandler(signatureCapture_CaptureErrorEvent);
                this.signatureCapture.EndCapture();
            }

            base.OnClosing(e);
        }

        private void TranslateLabels()
        {
            this.Text             = ApplicationLocalizer.Language.Translate(99622); // Signature capture.
            this.lslNotReady.Text = ApplicationLocalizer.Language.Translate(99621); // Device not ready.
            this.lblHeader.Text   = ApplicationLocalizer.Language.Translate(99620); // Ask the customer to provide their signature on the device.
            this.btnOk.Text       = ApplicationLocalizer.Language.Translate(55403); // Ok
            this.btnCancel.Text   = ApplicationLocalizer.Language.Translate(55404); // Cancel
        }

        private bool IsSignatureCaptureReady()
        {
            return this.signatureCapture != null && this.signatureCapture.IsActive;
        }

        private void signatureCapture_CaptureErrorEvent(string message)
        {
            NetTracer.Warning(message);
            using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation))
            {
                POSFormsManager.ShowPOSForm(dialog);
            }
        }

        private void signatureCapture_CaptureCompleteEvent(object sender, ISignatureCaptureInfo eventArgs)
        {
            this.signatureViewer.DrawSignature(eventArgs.Points, 50);
            this.Points        = eventArgs.Points;
            this.btnOk.Enabled = this.Points != null && this.Points.Count > 0;
        }

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type</typeparam>
        /// <param name="args">Notification</param>
        public void Initialize<TArgs>(TArgs args) where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
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
            SignatureCaptureConfirmation result = new SignatureCaptureConfirmation();
            result.Confirmed = this.DialogResult == DialogResult.OK;
            result.AddRange(this.Points);

            return result as TResults;
        }

        #endregion
    }
}
