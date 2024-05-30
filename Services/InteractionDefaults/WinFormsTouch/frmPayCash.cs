/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.Common;
using LSRetailPosis.POSProcesses.WinControls;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.SystemCore;


namespace LSRetailPosis.POSProcesses
{
    /// <summary>
    /// Summary description for frmPayCash.
    /// </summary>
    [Export("PayCashForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class frmPayCash : frmTouchBase, IInteractionView
    {
        #region Member variables;

        private decimal registeredAmount;
        private bool operationDone;
        #endregion
        private PanelControl panelBase;
        private Tender tenderInfo;
        private decimal balanceAmount;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private NumPad numCashNumpad;
        private AmountViewer amtCashAmounts;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Label lblPayAmt;
        private SimpleButtonEx btnOK;
        private SimpleButtonEx btnCancel;
        private FlowLayoutPanel flowLayoutPanel;
        private Label labelAmountDue;
        private Label labelAmountDueValue;
        private PanelControl panelAmount;

        #region Properties

        public decimal RegisteredAmount
        {
            get
            {
                return registeredAmount;
            }
            set
            {
                if (registeredAmount == value)
                {
                    return;
                }
                registeredAmount = value;
            }
        }
        public bool OperationDone
        {
            get
            {
                return operationDone;
            }
            set
            {
                operationDone = value;
            }
        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        protected frmPayCash()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmPayCash(PayCashConfirmation payCashConfirmation)
            : this()
        {
            this.operationDone = false;
            if (payCashConfirmation == null)
            {
                throw new ArgumentNullException("payCashConfirmation");
            }
            this.tenderInfo = (Tender)payCashConfirmation.TenderInfo;
            this.Text = payCashConfirmation.Title;

            amtCashAmounts.LocalCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
            amtCashAmounts.UsedCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
            this.balanceAmount = payCashConfirmation.BalanceAmount;
            amtCashAmounts.SoldLocalAmount = 
                PosApplication.Instance.Services.Rounding.RoundAmount(
                payCashConfirmation.ProposedPaymentAmount != null 
                ? (decimal)payCashConfirmation.ProposedPaymentAmount 
                : balanceAmount,
                ApplicationSettings.Terminal.StoreId, tenderInfo.TenderID.ToString());
            amtCashAmounts.ForeignCurrencyMode = false;
            amtCashAmounts.HighestOptionAmount = tenderInfo.MaximumAmountAllowed;
            amtCashAmounts.LowesetOptionAmount = tenderInfo.MinimumAmountAllowed;

            if (this.tenderInfo.OverTenderAllowed)
            {
                amtCashAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.HigherAmounts;
            }
            else
            {
                amtCashAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;
            }

            amtCashAmounts.SetButtons();

            numCashNumpad.NegativeMode = this.balanceAmount < 0;
            numCashNumpad.Select();

        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //
                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmPayCash are reserved at 1380 - 1399
                // In use now are ID's 1380 - 1386
                //

                btnCancel.Text           = ApplicationLocalizer.Language.Translate(1380);  // Cancel
                numCashNumpad.PromptText = ApplicationLocalizer.Language.Translate(1382);  // Enter amount
                btnOK.Text               = ApplicationLocalizer.Language.Translate(1201);  // Ok
                lblPayAmt.Text           = ApplicationLocalizer.Language.Translate(1384);  // Payment amount
                labelAmountDue.Text      = ApplicationLocalizer.Language.Translate(1450);  // Total amount due:
                labelAmountDueValue.Text = PosApplication.Instance.Services.Rounding.RoundAmount(balanceAmount, ApplicationSettings.Terminal.StoreId, tenderInfo.TenderID.ToString(), true);
            }

            base.OnLoad(e);
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
            this.panelBase = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelAmountDue = new System.Windows.Forms.Label();
            this.labelAmountDueValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.numCashNumpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.panelAmount = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPayAmt = new System.Windows.Forms.Label();
            this.amtCashAmounts = new LSRetailPosis.POSProcesses.WinControls.AmountViewer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOK = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.tableLayoutPanel1);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Size = new System.Drawing.Size(944, 767);
            this.panelBase.TabIndex = 10;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(940, 763);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel, 3);
            this.flowLayoutPanel.Controls.Add(this.labelAmountDue);
            this.flowLayoutPanel.Controls.Add(this.labelAmountDueValue);
            this.flowLayoutPanel.Location = new System.Drawing.Point(205, 43);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(529, 95);
            this.flowLayoutPanel.TabIndex = 22;
            // 
            // labelAmountDue
            // 
            this.labelAmountDue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAmountDue.AutoSize = true;
            this.labelAmountDue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelAmountDue.Location = new System.Drawing.Point(0, 0);
            this.labelAmountDue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAmountDue.Name = "labelAmountDue";
            this.labelAmountDue.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelAmountDue.Size = new System.Drawing.Size(390, 95);
            this.labelAmountDue.TabIndex = 0;
            this.labelAmountDue.Tag = "";
            this.labelAmountDue.Text = "Total amount due:";
            // 
            // labelAmountDueValue
            // 
            this.labelAmountDueValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAmountDueValue.AutoSize = true;
            this.labelAmountDueValue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelAmountDueValue.Location = new System.Drawing.Point(390, 0);
            this.labelAmountDueValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelAmountDueValue.Name = "labelAmountDueValue";
            this.labelAmountDueValue.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelAmountDueValue.Size = new System.Drawing.Size(139, 95);
            this.labelAmountDueValue.TabIndex = 1;
            this.labelAmountDueValue.Text = "$0.00";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.numCashNumpad, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelAmount, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(182, 205);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(576, 412);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // numCashNumpad
            // 
            this.numCashNumpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numCashNumpad.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCashNumpad.Appearance.Options.UseFont = true;
            this.numCashNumpad.AutoSize = true;
            this.numCashNumpad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numCashNumpad.CurrencyCode = null;
            this.numCashNumpad.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCashNumpad.EnteredValue = "";
            this.numCashNumpad.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Price;
            this.numCashNumpad.Location = new System.Drawing.Point(20, 53);
            this.numCashNumpad.MaskChar = "";
            this.numCashNumpad.MaskInterval = 0;
            this.numCashNumpad.MaxNumberOfDigits = 9;
            this.numCashNumpad.MinimumSize = new System.Drawing.Size(248, 314);
            this.numCashNumpad.Name = "numCashNumpad";
            this.numCashNumpad.NegativeMode = false;
            this.numCashNumpad.NoOfTries = 0;
            this.numCashNumpad.NumberOfDecimals = 2;
            this.numCashNumpad.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.numCashNumpad.PromptText = null;
            this.numCashNumpad.ShortcutKeysActive = false;
            this.numCashNumpad.Size = new System.Drawing.Size(248, 314);
            this.numCashNumpad.TabIndex = 8;
            this.numCashNumpad.TimerEnabled = true;
            this.numCashNumpad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad1_EnterButtonPressed);
            // 
            // panelAmount
            // 
            this.panelAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelAmount.Controls.Add(this.tableLayoutPanel4);
            this.panelAmount.Location = new System.Drawing.Point(344, 50);
            this.panelAmount.Margin = new System.Windows.Forms.Padding(0);
            this.panelAmount.Name = "panelAmount";
            this.panelAmount.Size = new System.Drawing.Size(176, 317);
            this.panelAmount.TabIndex = 16;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.lblPayAmt, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.amtCashAmounts, 0, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(172, 310);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // lblPayAmt
            // 
            this.lblPayAmt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPayAmt.AutoSize = true;
            this.lblPayAmt.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lblPayAmt.Location = new System.Drawing.Point(30, 0);
            this.lblPayAmt.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.lblPayAmt.Name = "lblPayAmt";
            this.lblPayAmt.Size = new System.Drawing.Size(112, 25);
            this.lblPayAmt.TabIndex = 10;
            this.lblPayAmt.Text = "Pay amount";
            // 
            // amtCashAmounts
            // 
            this.amtCashAmounts.CurrencyRate = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCashAmounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.amtCashAmounts.ForeignCurrencyMode = false;
            this.amtCashAmounts.HighestOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCashAmounts.IncludeRefAmount = true;
            this.amtCashAmounts.LocalCurrencyCode = "";
            this.amtCashAmounts.Location = new System.Drawing.Point(0, 30);
            this.amtCashAmounts.LowesetOptionAmount = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.amtCashAmounts.Margin = new System.Windows.Forms.Padding(0);
            this.amtCashAmounts.Name = "amtCashAmounts";
            this.amtCashAmounts.OptionsLimit = 5;
            this.amtCashAmounts.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.amtCashAmounts.ShowBorder = false;
            this.amtCashAmounts.Size = new System.Drawing.Size(172, 280);
            this.amtCashAmounts.TabIndex = 9;
            this.amtCashAmounts.UsedCurrencyCode = "";
            this.amtCashAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.HigherAmounts;
            this.amtCashAmounts.AmountChanged += new LSRetailPosis.POSProcesses.WinControls.AmountViewer.OutputChanged(this.amountViewer1_AmountChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(33, 684);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(874, 65);
            this.tableLayoutPanel3.TabIndex = 21;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(441, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOK.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(306, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(127, 57);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmPayCash
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(944, 767);
            this.Controls.Add(this.panelBase);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmPayCash";
            this.Controls.SetChildIndex(this.panelBase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelAmount)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {  // NOTE: Override keypreview on form to get this a first chance
            if (e != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Add:
                        registeredAmount = this.balanceAmount;
                        operationDone = true;
                        e.Handled = true;                                        // Indicate key is handled
                        this.DialogResult = DialogResult.OK;
                        Close();
                        break;
                }
            }

            base.OnKeyDown(e);
        }


        private void amountViewer1_AmountChanged(decimal outAmount, string currCode)
        {
            TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, outAmount, true, balanceAmount);
            if (string.IsNullOrEmpty(tenderReq.ErrorText))
            {
                registeredAmount = outAmount;
                operationDone = true;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop)) //The amount entered is higher than the maximum amount allowed
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }
        }

        private void numPad1_EnterButtonPressed()
        {
            TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, numCashNumpad.EnteredDecimalValue, true, balanceAmount);
            if (string.IsNullOrEmpty(tenderReq.ErrorText))
            {
                registeredAmount = numCashNumpad.EnteredDecimalValue;
                registeredAmount = PosApplication.Instance.Services.Rounding.RoundAmount(registeredAmount, ApplicationSettings.Terminal.StoreId, tenderInfo.TenderID.ToString());
                operationDone = true;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop)) //The amount entered is higher than the maximum amount allowed
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
                numCashNumpad.TryAgain();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            numPad1_EnterButtonPressed();
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
            return new PayCashConfirmation
            {
                Confirmed = this.DialogResult == DialogResult.OK,
                RegisteredAmount = this.RegisteredAmount,
                OperationDone = this.OperationDone,
            } as TResults;
        }

        #endregion
    }
}