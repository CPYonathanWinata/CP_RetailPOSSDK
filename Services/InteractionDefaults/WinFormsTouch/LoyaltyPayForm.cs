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
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;
    using System.Collections.ObjectModel;
   
    /// <summary>
    /// Summary description for LoyaltyPayForm.
    /// </summary>
    /// <summary>
    /// Summary description for LoyaltyPayForm.
    /// </summary>
    [Export("LoyaltyPayForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LoyaltyPayForm : frmTouchBase, IInteractionView
    {
        Boolean ok = false;
        Boolean check_close = true;

        //add by Yonathan balance loyalty 26/01/2024
        decimal balanceLoyalty = 0;

        protected LoyaltyPayForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Creates loyalty pay card form.
        /// </summary>
        /// <param name="loyaltyCardPayConfirmation">Loyalty card payed confirmation.</param>
        [ImportingConstructor]
        public LoyaltyPayForm(LoyaltyCardPayConfirmation loyaltyCardPayConfirmation)
            : this()
        {
            if (loyaltyCardPayConfirmation == null)
            {
                throw new ArgumentNullException("loyaltyCardPayConfirmation");
            }

            if (loyaltyCardPayConfirmation.BalanceAmount == 0m || String.IsNullOrEmpty(loyaltyCardPayConfirmation.CurrencyCode))
            {
                throw new ArgumentException("loyaltyCardPayConfirmation.BalanceAmount or loyaltyCardPayConfirmation.CurrencyCode has invalid value.");
            }
            else
            {
            //    MessageBox.Show(loyaltyCardPayConfirmation.LoyaltyCardNumber);
                this.CardNumber = loyaltyCardPayConfirmation.LoyaltyCardNumber;
                this.BalanceAmount = loyaltyCardPayConfirmation.BalanceAmount;
                this.CurrencyCode = loyaltyCardPayConfirmation.CurrencyCode;
            }
        }

        private NumpadEntryTypes EntryType
        {
            set
            {
                if (value != this.numCurrNumpad.EntryType)
                {
                    switch (value)
                    {
                        case NumpadEntryTypes.AlphaNumeric:
                            this.numCurrNumpad.PromptText = ApplicationLocalizer.Language.Translate(50012);
                            this.numCurrNumpad.EntryType = NumpadEntryTypes.AlphaNumeric;
                            this.numCurrNumpad.NegativeMode = false;
                            break;
                        case NumpadEntryTypes.Price:
                            this.numCurrNumpad.PromptText = ApplicationLocalizer.Language.Translate(50017);
                            this.numCurrNumpad.EntryType = NumpadEntryTypes.Price;
                            this.numCurrNumpad.NegativeMode = this.BalanceAmount < 0;
                            break;
                    }
                }
            }
            get
            {
                return this.numCurrNumpad.EntryType;
            }
        }
         
        private string CardNumber { get; set; }
        private decimal BalanceAmount { get; set; }
        private string CurrencyCode { get; set; }
        

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.TranslateLabels();
                this.InitState();
                this.EnablePeripherals();
                //disable calculator by Yonathan 26/01/2024
                numCurrNumpad.Visible = false;
                btnPaymentAmount.Enabled = false;
                btnPaymentAmount.Text = "0";
                textBoxAmount.Text = "0";
                btnOk.Enabled = false;
                //
            }

            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            this.DisablePeripherals();

            base.OnFormClosed(e);
        }

        private void InitState()
        {
            this.EntryType = NumpadEntryTypes.Price;
         /*   if (!String.IsNullOrEmpty(this.CardNumber))
            {
                this.CPHideCharCardNumber(this.CardNumber);
            }
           */
            this.textBoxCardNumber.Text =  this.CardNumber;
            this.textBoxAmount.Text = PosApplication.Instance.Services.Rounding.Round(this.BalanceAmount, this.CurrencyCode, false);
            this.numCurrNumpad.EnteredValue = PosApplication.Instance.Services.Rounding.Round(this.BalanceAmount, this.CurrencyCode, false);
            this.btnPaymentAmount.Text = PosApplication.Instance.Services.Rounding.Round(this.BalanceAmount, this.CurrencyCode, true);
            this.labelAmountDueValue.Text = string.Format(ApplicationLocalizer.Language.Translate(1426), this.CurrencyCode, btnPaymentAmount.Text);
        }

        private void TranslateLabels()
        {
            this.labelAmountDue.Text = ApplicationLocalizer.Language.Translate(1450); // 85090
            this.labelCardNumber.Text = ApplicationLocalizer.Language.Translate(50005);
            this.labelAmount.Text = ApplicationLocalizer.Language.Translate(50008);
            this.Text = ApplicationLocalizer.Language.Translate(50042);
            this.labelPaymentAmount.Text = ApplicationLocalizer.Language.Translate(50083);//Payment amount
            this.btnOk.Text = ApplicationLocalizer.Language.Translate(50084);//OK
            this.btnCancel.Text = ApplicationLocalizer.Language.Translate(50085);//Cancel
        }

        private void EnablePeripherals()
        {
            this.DisablePeripherals();

            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent += this.ProcessScannedCard;
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

        private void AmountTextBox_Enter(Object sender, EventArgs e)
        {
            if (this.EntryType != NumpadEntryTypes.Price)
            {
                this.EntryType = NumpadEntryTypes.Price;
            }
        }

        private void CardNumberTextBox_Enter(Object sender, EventArgs e)
        {

        }

        private void numCurrNumpad_EnterButtonPressed()
        {
            switch (this.numCurrNumpad.EntryType)
            {
                case NumpadEntryTypes.Price:
                    this.textBoxAmount.Text = this.numCurrNumpad.EnteredValue;
                    break;
                case NumpadEntryTypes.AlphaNumeric:
                    this.textBoxCardNumber.Text = this.numCurrNumpad.EnteredValue;
                    break;
            }
        }

        private void btnPaymentAmount_Click(object sender, EventArgs e)
        {
            //modified by Yonathan 26/01/2024
            btnOk.Enabled = true;
            this.textBoxAmount.Text = PosApplication.Instance.Services.Rounding.Round(balanceLoyalty, this.CurrencyCode, false); //PosApplication.Instance.Services.Rounding.Round(this.BalanceAmount, this.CurrencyCode, false);
            if (this.EntryType == NumpadEntryTypes.Price)
            {
                this.numCurrNumpad.EnteredValue = PosApplication.Instance.Services.Rounding.Round(balanceLoyalty, this.CurrencyCode, false);  // PosApplication.Instance.Services.Rounding.Round(this.BalanceAmount, this.CurrencyCode, false);
                this.textBoxAmount.Select();
            }
        }

        private void ProcessScannedCard(IScanInfo scanInfo)
        {
            this.textBoxCardNumber.Text = scanInfo.ScanData;
            PosApplication.Instance.Services.Peripherals.Scanner.ReEnableForScan();
        }

        private void ProcessSweptCard(ICardInfo cardInfo)
        {
            this.textBoxCardNumber.Text = cardInfo.CardNumber;
            PosApplication.Instance.Services.Peripherals.MSR.EnableForSwipe();
        }

        private void CPHideCharCardNumber(string _strCard)
        {
            int lenCard = _strCard.Length;
            
            string stringSymbol = new string('*', lenCard - 4);
            this.textCardNumberHide.Text = stringSymbol + _strCard.Substring(lenCard - 4);

        }
        #region IInteractionView implementation

        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
        }

        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new LoyaltyCardPayConfirmation
            {
                Confirmed = this.DialogResult == DialogResult.OK,
                LoyaltyCardNumber = this.textBoxCardNumber.Text,
                RegisteredAmount = String.IsNullOrEmpty(this.textBoxAmount.Text) ? 0m : decimal.Parse(this.textBoxAmount.Text)
            } as TResults;
        }

        #endregion

      

        private void textCardNumberHide_TextChanged(object sender, EventArgs e)
        {
            string s = this.textCardNumberHide.Text;
            int lengthCard = this.textCardNumberHide.Text.Length;

       
            this.textBoxCardNumber.Text = this.textCardNumberHide.Text;
            if (lengthCard > 4)
            {
                this.textCardNumberHide.Text = s.Substring(s.Length - 4).PadLeft(s.Length, '*');
            }
           
        }

     
        private void textCardNumberHide_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            
        }

        private void textCardNumberHide_Enter(object sender, EventArgs e)
        {
            FirstInput = new DateTime(1900, 01, 01, 01, 00, 00);

        }
        private void textCardNumberHide_KeyUp(object sender, KeyEventArgs e)
        {
          
        }

        private void textCardNumberHide_Leave(object sender, EventArgs e)
        {
            FirstInput = new DateTime(1900, 01, 01, 01, 00, 00);
        }

        private void textCardNumberHide_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_close = false;
            ok = false;

            DateTime dateNull = new DateTime(1900, 01, 01, 01, 00, 00);
            DateTime start = DateTime.Now;
          //  int a = 0;

            if ( this.textCardNumberHide.Text.Length == 3 || this.textCardNumberHide.Text.Length == 8)
            {
                
                //MessageBox.Show(FirstInput.ToString());
                if (FirstInput == dateNull)
                {
                    FirstInput = DateTime.Now;
                }
                else
                {
                    TimeSpan timeDiff = DateTime.Now - FirstInput;
                                  
                    if (timeDiff.TotalMilliseconds < 300) // modify from 50 to 80
                    {
                        ok = true;
                       
                    }
                } 
            }
            else if(this.textCardNumberHide.Text.Length == 0)
            {
                FirstInput  = dateNull;
                check_close = false;
                ok          = false;
            }
            
        }

        private void textCardNumberHide_KeyDown(object sender, KeyEventArgs e)
        {
           // FirstInput = new DateTime(1900, 01, 01, 01, 00, 00);
            //MessageBox.Show("KeyDown");
        }
        private void btnCheckBalance_Click(object sender, EventArgs e)
        {
            //add by Yonathan 26/01/2024
            if (textCardNumberHide.Text != "")
            {
                ReadOnlyCollection<object> containerArray;
                ILoyaltyCardData loyaltyCardData;
                string loyaltyFullName = "";

                loyaltyCardData = PosApplication.Instance.Services.Loyalty.GetLoyaltyBalanceInfo(textBoxCardNumber.Text);

                
                btnPaymentAmount.Enabled = true;
                //if loyalty point bigger than the payment amount -> apply the point as the payment amount
                if (loyaltyCardData.BalancePoints >= this.BalanceAmount)
                {
                    balanceLoyalty = this.BalanceAmount;
                }
                else
                {
                    balanceLoyalty = loyaltyCardData.BalancePoints;                
                }
                btnPaymentAmount.Text = PosApplication.Instance.Services.Rounding.Round(balanceLoyalty, this.CurrencyCode, false);  // loyaltyCardData.BalancePoints.ToString("G");

                labelCheckBalanceAmount.Text = loyaltyCardData.BalancePoints.ToString("N");
                labelCheckBalanceTitle.Text = "Available Balance :";


                containerArray = PosApplication.Instance.TransactionServices.InvokeExtension("getLoyaltyDetail", textBoxCardNumber.Text.ToString());


                loyaltyFullName = containerArray[4].ToString();
                labelNameTitle.Text = loyaltyFullName;
                //GetLoyaltyBalanceInfo
            }
            
            
        }
        private void btnOk_Click(object sender, EventArgs e)
        {

        //    MessageBox.Show(this.textBoxCardNumber.Text);
        //    MessageBox.Show(this.CardNumber);

    /*        if (this.textBoxCardNumber.Text == this.CardNumber)
            {

                if (ok == false)
                {
                    MessageBox.Show("Please Scan Member Card!!!");
                    check_close = false;
                }
            }
            else
            {
                MessageBox.Show("Incorrect Member Card!!!");
                check_close = false;
            }
            */
        }

        private void LoyaltyPayForm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            if(check_close == false)
            {
                check_close = true;
                e.Cancel = true;
            }
        }

        private void textBoxCardNumber_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void numCurrNumpad_Load(object sender, EventArgs e)
        {
        //    check_close = false;
         //   ok = false;
        }

        private void textBoxAmount_EditValueChanged(object sender, EventArgs e)
        {
         //   check_close = false;
         //   ok = false;
        }

      
       
       
       

        

    }
}
