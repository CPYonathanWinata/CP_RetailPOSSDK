/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.WinControls;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Summary description for ManagerAccessForm.
    /// </summary>
    [Export("ManagerAccessForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class ManagerAccessForm : frmTouchBase, IInteractionView
    {
        private LogonData logonData;
        private string operatorId;
        private PosisOperations operationId;

        private NumPad numUserId;
        private SimpleButtonEx btnOk;
        private SimpleButtonEx btnCancel;
        private Label lblMsg;
        private TableLayoutPanel tableLayoutPanel1;
        private PanelControl panelControl1;

        private System.ComponentModel.Container components = null;
        private WinFormsTouch.TaggedImageBoxControl TaggedImage;
        
        private ManagerAccessForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerAccessForm"/> class.
        /// </summary>
        /// <param name="managerAccessConfirmation">The manager access confirmation.</param>
        [ImportingConstructor]
        public ManagerAccessForm(ManagerAccessConfirmation managerAccessConfirmation)
            : this()
        {
            if (managerAccessConfirmation == null)
            {
                throw new ArgumentNullException("managerAccessConfirmation");
            }

            this.operationId = (PosisOperations)managerAccessConfirmation.Operation;
        }

        internal ManagerAccessForm(PosisOperations operationId)
            : this()
        {
            this.operationId = operationId;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                if (!string.IsNullOrEmpty(this.operatorId))
                {
                    PromptForPassword();
                }

                PosApplication.Instance.Services.Peripherals.KeyLock.KeyLockSupervisorPositionEvent
                    += new KeyLockSupervisorPositionEventHandler(Keylock_KeylockSupervisorPositionEvent);

                PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived += new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
                PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginVerifyCapture();
            }

            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (TaggedImage.IsActive)
            {
                TaggedImage.EndImageRefresh();
            }

            PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();
            PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived -= new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
            PosApplication.Instance.Services.Peripherals.KeyLock.KeyLockSupervisorPositionEvent 
                -= new KeyLockSupervisorPositionEventHandler(Keylock_KeylockSupervisorPositionEvent);

            base.OnClosed(e);
        }

        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmCustomerSearch are reserved at 1320 - 1339
            // In use now are ID's 1320 - 1330
            //
            btnCancel.Text = ApplicationLocalizer.Language.Translate(1320); //Cancel
            btnOk.Text = ApplicationLocalizer.Language.Translate(1321); //OK
            numUserId.PromptText = ApplicationLocalizer.Language.Translate(28);   //Operator ID:
            lblMsg.Text = ApplicationLocalizer.Language.Translate(35);   //Enter an Operator ID

            if (this.operationId == PosisOperations.ApplicationExit)
            {
                this.Text = ApplicationLocalizer.Language.Translate(22); //Exit
            }
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numUserId = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.TaggedImage = new Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.TaggedImageBoxControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMsg.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMsg, 4);
            this.lblMsg.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblMsg.Location = new System.Drawing.Point(285, 40);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblMsg.Size = new System.Drawing.Size(449, 95);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Enter an Operator ID";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Location = new System.Drawing.Point(379, 688);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Windows.Forms.Padding(0);
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.OnBtnOkClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(514, 688);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(0);
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblMsg, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.numUserId, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.TaggedImage, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 15);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numUserId
            // 
            this.numUserId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numUserId.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUserId.Appearance.Options.UseFont = true;
            this.numUserId.AutoSize = true;
            this.numUserId.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.numUserId, 2);
            this.numUserId.CurrencyCode = null;
            this.numUserId.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numUserId.EnteredValue = "";
            this.numUserId.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Barcode;
            this.numUserId.Location = new System.Drawing.Point(315, 193);
            this.numUserId.Margin = new System.Windows.Forms.Padding(0);
            this.numUserId.MaskChar = "";
            this.numUserId.MaskInterval = 0;
            this.numUserId.MaxNumberOfDigits = 80;
            this.numUserId.MinimumSize = new System.Drawing.Size(390, 432);
            this.numUserId.Name = "numUserId";
            this.numUserId.NegativeMode = false;
            this.numUserId.NoOfTries = 0;
            this.numUserId.PromptText = "Operator ID";
            this.numUserId.ShortcutKeysActive = false;
            this.numUserId.Size = new System.Drawing.Size(390, 432);
            this.numUserId.TabIndex = 1;
            this.numUserId.TimerEnabled = true;
            this.numUserId.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.OnNumPadEnterButtonPressed);
            this.numUserId.CardSwept += new LSRetailPosis.POSProcesses.WinControls.NumPad.cardSwipedDelegate(this.numUserId_CardSwept);
            // 
            // TaggedImage
            // 
            this.TaggedImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.SetColumnSpan(this.TaggedImage, 2);
            this.TaggedImage.Location = new System.Drawing.Point(449, 138);
            this.TaggedImage.Name = "TaggedImage";
            this.TaggedImage.Size = new System.Drawing.Size(120, 90);
            this.TaggedImage.TabIndex = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1024, 768);
            this.panelControl1.TabIndex = 0;
            // 
            // ManagerAccessForm
            // 
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.MinimumSize = new System.Drawing.Size(420, 539);
            this.Name = "ManagerAccessForm";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private LogonData LogonSystem
        {
            get
            {
                if (this.logonData == null)
                {
                    this.logonData = new LogonData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                }

                return this.logonData;
            }
        }

        private bool IsPasswordRequired(string userId)
        {
            try
            {
                if (LogonSystem.UserIdExists(ApplicationSettings.Terminal.StoreId, userId))
                {
                    return LogonSystem.HasPassword(ApplicationSettings.Terminal.StoreId, userId, ApplicationSettings.Terminal.StaffPasswordHashName);
                }

                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private void OnNumPadEnterButtonPressed()
        {
            if (string.IsNullOrEmpty(this.operatorId))
            {
                //
                // Read operator ID
                //
                if (string.IsNullOrEmpty(this.numUserId.EnteredValue))
                {
                    // Invalid credentials
                    using (frmMessage dialog = new frmMessage(1323, MessageBoxButtons.OK, MessageBoxIcon.Information))
                    {
                        POSFormsManager.ShowPOSForm(dialog);
                    }
                }
                else
                {
                    bool usePassword = true;

                    if (Functions.StaffBarcodeLogOn)
                    {
                        IExtendedLogOnInfo extendedLogOnInfo = new ExtendedLogOnInfo()
                        {
                            LogOnKey = this.numUserId.EnteredValue,
                            LogOnType = ExtendedLogOnType.Barcode,
                            PasswordRequired = Functions.StaffBarcodeLogOnRequiresPassword
                        };

                        // First see if this is a extended logon key
                        this.operatorId = PosApplication.Instance.Services.Peripherals.LogOnDevice.Identify(extendedLogOnInfo);

                        // If not found, then give a try to legacy barcode mask approch.
                        if (string.IsNullOrWhiteSpace(operatorId))
                        {
                            IBarcodeInfo barcodeInfo = PosApplication.Instance.Services.Barcode.ProcessBarcode(BarcodeEntryType.ManuallyEntered, this.numUserId.EnteredValue);

                            if (barcodeInfo.InternalType == BarcodeInternalType.Employee)
                            {
                                this.operatorId = barcodeInfo.EmployeeId;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(operatorId))
                        {
                            usePassword = extendedLogOnInfo.PasswordRequired;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(operatorId))
                    {
                        this.operatorId = this.numUserId.EnteredValue;    //Standard employee id
                    }

                    if (usePassword)
                       PromptForPassword();
                    else
                        ValidateCredentials(ApplicationSettings.Terminal.StoreId, this.operatorId, null);
                }
            }
            else
            {
                //
                // Read password
                //
                using (SecureString ss = new SecureString())
                {
                    foreach (char c in this.numUserId.EnteredValue)
                    {
                        ss.AppendChar(c);
                    }

                    ss.MakeReadOnly();

                    ValidateCredentials(ApplicationSettings.Terminal.StoreId, this.operatorId, LogonData.ComputePasswordHash(this.operatorId, ss, ApplicationSettings.Terminal.StaffPasswordHashName));
                }
            }
        }

        private void OnBtnOkClicked(object sender, EventArgs e)
        {
            OnNumPadEnterButtonPressed();

            this.numUserId.Focus();
        }

        private void ValidateCredentials(string storeId, string userId, string passwordHash)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            bool isAuthenticated = false;
            int? errorId = null;

            // First, see if a password is required
            if (LogonSystem.UserIdExists(storeId, userId))
            {
                if (passwordHash != null && IsPasswordRequired(userId))
                {
                    if (LogonSystem.ValidatePasswordHash(storeId, userId, passwordHash))
                    {
                        // Password is good, authentication passed
                        isAuthenticated = true;
                    }
                    else
                    {
                        // Authentication failed
                        errorId = 1325; // The password is not valid. Enter a valid password
                    }
                }
                else
                {
                    // Password not required, authentication passed
                    isAuthenticated = true;
                }
            }
            else
            {
                // Authentication failed
                errorId = 3214; // The Operator ID is not valid.
            }

            // If we're authenticated, check authorization for requested operation
            if (isAuthenticated)
            {
                IUserAccessSystem userAccess = PosApplication.Instance.BusinessLogic.UserAccessSystem;
                if (userAccess.UserHasAccess(userId, operationId))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                {
                    // Unauthorized
                    errorId = 1322;
                }
            }

            // If we get to this point, an error occured
            if (errorId.HasValue)
            {
                // We shut down the capture device
                PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived -= new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
                PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();

                // Invalid credentials
                using (frmMessage dialog = new frmMessage(errorId.Value, MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                   POSFormsManager.ShowPOSForm(dialog);

                    // we restart the capture before showing the new form
                   PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived += new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
                   PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginVerifyCapture();

                }

            }

            PromptForOperatorId();
        }

        private void PromptForPassword()
        {
            this.numUserId.ClearValue();
            this.numUserId.EntryType = NumpadEntryTypes.Password;
            this.numUserId.PromptText = ApplicationLocalizer.Language.Translate(2379); // Password
            this.lblMsg.Text = ApplicationLocalizer.Language.Translate(1328); //Enter a password
        }

        private void PromptForOperatorId()
        {
            this.operatorId = null;

            this.numUserId.ClearValue();
            this.numUserId.EntryType = NumpadEntryTypes.Barcode;
            this.numUserId.PromptText = ApplicationLocalizer.Language.Translate(28); // Operator ID
            this.lblMsg.Text = ApplicationLocalizer.Language.Translate(35);
        }

        // This event is triggered when the manager key has been turned to supervisor mode
        private void Keylock_KeylockSupervisorPositionEvent()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public void CheckForManagerPrivileges(ICardInfo cardInfo, string userId)
        {
            this.operatorId = userId;

            numUserId_CardSwept(cardInfo);
        }

        /// <summary>
        /// Called when [log on device_ data received].
        /// </summary>
        /// <param name="extendedLogOnInfo">The extended log on info.</param>
        private void OnLogOnDevice_DataReceived(IExtendedLogOnInfo extendedLogOnInfo)
        {
            if (!string.IsNullOrWhiteSpace(extendedLogOnInfo.LogOnKey))
            {
                ProcessExtendedLogOnKey(extendedLogOnInfo);
            }
            // Status update
            else if (extendedLogOnInfo.DataStream != null)
            {
                TaggedImage.Initialize(extendedLogOnInfo.DataStream);
            }
        }

        /// <summary>
        /// Processes the extended log on key.
        /// </summary>
        /// <param name="extendedLogOnInfo">The extended log on info.</param>
        private void ProcessExtendedLogOnKey(IExtendedLogOnInfo extendedLogOnInfo)
        {
            try
            {
                // Check if the operator ID was assigned earlier via entering on keypad or via extended logon. Extended logon 
                // should not clear out whatever has been entered on the keypad
                if (string.IsNullOrEmpty(operatorId))
                {
                    operatorId = PosApplication.Instance.Services.Peripherals.LogOnDevice.Identify(extendedLogOnInfo);

                    if (!string.IsNullOrEmpty(this.operatorId))
                    {
                        if (extendedLogOnInfo.PasswordRequired)
                        {
                            PromptForPassword();
                        }
                        else
                        {
                            ValidateCredentials(ApplicationSettings.Terminal.StoreId, this.operatorId, null);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                NetTracer.Error(ex, "Unabled to process extended log on type: {0}", extendedLogOnInfo.LogOnType);
            }
        }

        private void numUserId_CardSwept(ICardInfo cardInfo)
        {
            if (Functions.StaffCardLogOn)
            {
                IExtendedLogOnInfo extendedLogOnInfo = new ExtendedLogOnInfo()
                {
                    LogOnKey = cardInfo.CardNumber,
                    LogOnType = ExtendedLogOnType.MagneticStripeReader,
                    PasswordRequired = Functions.StaffCardLogOnRequiresPassword
                };

                ProcessExtendedLogOnKey(extendedLogOnInfo);
            }
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
            return new ManagerAccessConfirmation
            {
                Confirmed = this.DialogResult == DialogResult.OK,
                OperatorId = this.operatorId
            } as TResults;
        }

        #endregion

    }
}
