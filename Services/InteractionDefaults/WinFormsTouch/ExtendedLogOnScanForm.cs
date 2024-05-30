/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings.HardwareProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using DE = Microsoft.Dynamics.Retail.Pos.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Extended log on scan
    /// </summary>
    internal partial class ExtendedLogOnScanForm : frmTouchBase
    {

        #region Properties

        private StringBuilder keyboardScanBuffer = new StringBuilder();

        /// <summary>
        /// Gets the extended log on info.
        /// </summary>
        public IExtendedLogOnInfo ExtendedLogOnInfo { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLogOnScanForm"/> class.
        /// </summary>
        public ExtendedLogOnScanForm()
        {
            InitializeComponent();
            base.HideHelpIcon();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.TranslateLabels();

                // Hook up Logon device
                PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived += new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
                PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginEnrollCapture();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Overloads <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!DesignMode)
            {
                PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();
                PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived -= new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
            }

            if (TaggedImageBox.IsActive)
            {
                TaggedImageBox.EndImageRefresh();
            }

            base.OnClosing(e);
        }

        private void TranslateLabels()
        {
            this.Text = ApplicationLocalizer.Language.Translate(99400);
            this.lblHeader.Text = ApplicationLocalizer.Language.Translate(99401);
            this.btnOk.Text = ApplicationLocalizer.Language.Translate(1344);
            this.btnCancel.Text = ApplicationLocalizer.Language.Translate(1340);
        }

        #endregion

        #region Methods

        private void HandleExtendedLogOnInfo(IExtendedLogOnInfo extendedLogOnInfo)
        {
            this.ExtendedLogOnInfo = extendedLogOnInfo;
            this.lblMessage.Text = ExtendedLogOnInfo.Message;

            if (string.IsNullOrWhiteSpace(ExtendedLogOnInfo.LogOnKey))
            {
                // Display the image
                if (ExtendedLogOnInfo.DataStream != null)
                {
                    TaggedImageBox.Visible = true;
                    CapturedImageBox.Visible = true;
                    BtnCaptureImage.Visible = true;
                    TaggedImageBox.Initialize(extendedLogOnInfo.DataStream, true);
                }

                this.btnOk.Enabled = false;
            }
            else
            {
                if (TaggedImageBox.IsActive)
                {
                    TaggedImageBox.EndImageRefresh();
                }
                if (BtnCaptureImage.Visible)
                {
                    BtnCaptureImage.Visible = false;
                }

                this.btnOk.Enabled = true;
                this.btnOk.Select();
            }
        }

        private void OnLogOnDevice_DataReceived(IExtendedLogOnInfo extendedLogOnInfo)
        {
            HandleExtendedLogOnInfo(extendedLogOnInfo);
        }

        private void OnButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (keyboardScanBuffer.Length > 0)
            {
                e.IsInputKey = true;
            }
        }
        
        private void OnButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyboardScanBuffer.Length > 0)
            {
                e.Handled = true;
            }
        }

        private void OnForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:

                    string keyboardScan = keyboardScanBuffer.ToString();
                    keyboardScanBuffer.Clear();

                    // IF scan is a MSR
                    if ((keyboardScan.StartsWith(MSR.StartTrack, StringComparison.OrdinalIgnoreCase) 
                        || keyboardScan.StartsWith(MSR.StartTrack2, StringComparison.OrdinalIgnoreCase))
                        && (keyboardScan.EndsWith(MSR.EndTrack, StringComparison.OrdinalIgnoreCase)))
                    {
                        string[] tracks = MSR.SplitTrackData(keyboardScan);

                        if (!string.IsNullOrWhiteSpace(tracks[1]))
                        {
                            ICardInfo cardInfo = PosApplication.Instance.BusinessLogic.Utility.CreateCardInfo();

                            // We just need card number from track 2
                            cardInfo.Track2 = tracks[1];

                            if (cardInfo.CardNumber.Length > 0)
                            {
                                IExtendedLogOnInfo extendedLogOnInfo = new DE.ExtendedLogOnInfo() 
                                { 
                                    LogOnKey = cardInfo.CardNumber,
                                    LogOnType = ExtendedLogOnType.MagneticStripeReader,
                                    Message = ApplicationLocalizer.Language.Translate(99409) /*Card swipe accepted*/ 
                                };

                                HandleExtendedLogOnInfo(extendedLogOnInfo);
                            }
                        }
                    }
                    else // If not, treat it as barcode scan.
                    {
                        IExtendedLogOnInfo extendedLogOnInfo = new DE.ExtendedLogOnInfo()
                        {
                            LogOnKey = keyboardScan,
                            LogOnType = ExtendedLogOnType.Barcode,
                            Message = ApplicationLocalizer.Language.Translate(99408) /*Bar code accepted*/
                        };

                        HandleExtendedLogOnInfo(extendedLogOnInfo);
                    }
                    break;

                default:
                    keyboardScanBuffer.Append(e.KeyChar);
                    break;
            }

            e.Handled = true;

        }

        private void OnBtnCaptureImage_KeyDown(object sender, EventArgs e)
        {
            PosApplication.Instance.Services.Peripherals.LogOnDevice.CaptureSample();
            CapturedImageBox.Image = TaggedImageBox.CurrentImage;
        }

        #endregion

    }
}
