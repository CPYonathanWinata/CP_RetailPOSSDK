using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPRedeemEZ : Form
    {/*
        IPosTransaction posTransaction;
        Timer timer = new System.Windows.Forms.Timer();
        Timer timerCountdown = new System.Windows.Forms.Timer();

        const string USER_AGENT = BlankOperations.USER_AGENT;

        const int DEVICE_ID = BlankOperations.EZ_DEVICE_ID;
        const string DEVICE_KEY = BlankOperations.EZ_DEVICE_KEY;
        const string MERCHANT_ID = BlankOperations.EZ_MERCHANT_ID;
        const string MERCHANT_KEY = BlankOperations.EZ_MERCHANT_KEY;

        const string EZ_GET_TOKEN = BlankOperations.EZ_GET_TOKEN_PFM_URL;
        const string EZ_REQUEST_REDEEM = BlankOperations.EZ_REQUEST_REDEEM_URL;
        const string EZ_REQUEST_STATUS = BlankOperations.EZ_REQUEST_PENDING_TRANS_STATUS_URL;
        const string EZ_GET_PROFILE = BlankOperations.EZ_GET_CARD_PROFILE_URL;
        const string EZ_GET_SETTING = BlankOperations.EZ_GET_MERCHANT_SETTING_URL;
        const string EZ_GET_BALANCE = BlankOperations.EZ_CHECK_BALANCE_URL;
        const string EZ_RESENT_PUSH_NOTIF = BlankOperations.EZ_RESENT_PUSH_NOTIF_URL;

        int pendingTransID;
        int convert = 0;
        int isCheckClick = 0;
        int countDownCounter = 60;
        int isFromAnotherFunction = 0;
        int isSuccessPay = 0;
        decimal poinBal = 0;
        string cardholderName = "";
        string M_TRANS_ID_PAYMENT = "";
        string token;
        string cardnumberMain = "";

        private int getToken()
        {
            string newToken = "";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
            string reqUrl = EZ_GET_TOKEN;
            int return_code = 0;
            string message = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqUrl);
            req.Method = "POST";
            req.Credentials = CredentialCache.DefaultCredentials;
            req.Accept = "text/json";
            req.Headers["Authorization"] = "retailstore01*";
            req.UserAgent = USER_AGENT;

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                string content = string.Empty;
                using (var stream = resp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic item = serializer.Deserialize<object>(content);

                return_code = item["return_code"];
                message = (string)item["message"];

                if (return_code == 200)
                {
                    newToken = (string)item["result"];

                    if (resp != null)
                        resp.Close();

                    token = newToken;
                    return 1;
                }
                else
                {
                    MessageBox.Show(message);
                    return 0;
                }
            }
        }

        public CPRedeemEZ(IPosTransaction _posTransaction, string _cardnumber)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
            cardnumberMain = _cardnumber;
        }

        private void numCurrNumpad_EnterButtonPressed()
        {
            txtAmount.Text = numCurrNumpad.EnteredValue;
        }

        private void CPRedeemEZ_Load(object sender, EventArgs e)
        {
            btnPayAll.Text = ((int)(((RetailTransaction)posTransaction).NetAmountWithTax - ((RetailTransaction)posTransaction).Payment)).ToString();
            lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", (((RetailTransaction)posTransaction).NetAmountWithTax - ((RetailTransaction)posTransaction).Payment));
            lblResponse.Text = "";
            numCurrNumpad.Enabled = true;
            btnPayAll.Enabled = true;
            btnCancel.Enabled = true;
            btnCancel.Visible = true;
            btnPauseCountdown.Visible = false;
            btnResend.Visible = false;
            txtCardNumber.Text = cardnumberMain;
            isCheckClick = 0;
            lblCountdown.Text = "";
            isSuccessPay = 0;

            checkBalance();
            checkMerchantSetting();

            lblTotalBalance.Text = "Total Point : " + ((int)poinBal) + " (* " + convert + ")" + Environment.NewLine;
            lblTotalBalance.Text += "Total Balance : Rp. " + string.Format("{0:#,0.00}", (poinBal * convert));
        }

        private void btnPayAll_Click(object sender, EventArgs e)
        {
            txtAmount.Text = btnPayAll.Text;
        }

        private void InitializeComponent()
        {
            this.numCurrNumpad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnPayAll = new System.Windows.Forms.Button();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnResend = new System.Windows.Forms.Button();
            this.lblTotalBalance = new System.Windows.Forms.Label();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.btnPauseCountdown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // numCurrNumpad
            // 
            this.numCurrNumpad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numCurrNumpad.Appearance.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.numCurrNumpad.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCurrNumpad.Appearance.ForeColor = System.Drawing.Color.Transparent;
            this.numCurrNumpad.Appearance.Options.UseBackColor = true;
            this.numCurrNumpad.Appearance.Options.UseFont = true;
            this.numCurrNumpad.Appearance.Options.UseForeColor = true;
            this.numCurrNumpad.AutoSize = true;
            this.numCurrNumpad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numCurrNumpad.CurrencyCode = null;
            this.numCurrNumpad.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCurrNumpad.EnteredValue = "";
            this.numCurrNumpad.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Integer;
            this.numCurrNumpad.Location = new System.Drawing.Point(294, 154);
            this.numCurrNumpad.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.numCurrNumpad.MaskChar = "";
            this.numCurrNumpad.MaskInterval = 0;
            this.numCurrNumpad.MaxNumberOfDigits = 8;
            this.numCurrNumpad.MinimumSize = new System.Drawing.Size(300, 330);
            this.numCurrNumpad.Name = "numCurrNumpad";
            this.numCurrNumpad.NegativeMode = false;
            this.numCurrNumpad.NoOfTries = 0;
            this.numCurrNumpad.NumberOfDecimals = 0;
            this.numCurrNumpad.PromptText = "Enter Amount";
            this.numCurrNumpad.ShortcutKeysActive = false;
            this.numCurrNumpad.Size = new System.Drawing.Size(300, 330);
            this.numCurrNumpad.TabIndex = 1;
            this.numCurrNumpad.TimerEnabled = true;
            this.numCurrNumpad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numCurrNumpad_EnterButtonPressed);
            this.numCurrNumpad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numCurrNumpad_KeyPress);
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(23, 189);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new System.Drawing.Size(231, 26);
            this.txtAmount.TabIndex = 2;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(23, 173);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(43, 13);
            this.lblAmount.TabIndex = 3;
            this.lblAmount.Text = "Amount";
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Location = new System.Drawing.Point(26, 234);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(69, 13);
            this.lblCardNumber.TabIndex = 4;
            this.lblCardNumber.Text = "Card Number";
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(23, 250);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(231, 26);
            this.txtCardNumber.TabIndex = 5;
            // 
            // btnPay
            // 
            this.btnPay.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnPay.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPay.Location = new System.Drawing.Point(636, 267);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(131, 53);
            this.btnPay.TabIndex = 6;
            this.btnPay.Text = "Send Payment";
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCheck.Enabled = false;
            this.btnCheck.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.Location = new System.Drawing.Point(636, 345);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(131, 53);
            this.btnCheck.TabIndex = 7;
            this.btnCheck.Text = "Check Payment";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnPayAll
            // 
            this.btnPayAll.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnPayAll.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnPayAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayAll.Location = new System.Drawing.Point(636, 189);
            this.btnPayAll.Name = "btnPayAll";
            this.btnPayAll.Size = new System.Drawing.Size(131, 53);
            this.btnPayAll.TabIndex = 9;
            this.btnPayAll.Text = "Amount";
            this.btnPayAll.UseVisualStyleBackColor = false;
            this.btnPayAll.Click += new System.EventHandler(this.btnPayAll_Click);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(18, 9);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(359, 29);
            this.lblTotalAmount.TabIndex = 10;
            this.lblTotalAmount.Text = "Total Amount : Rp 100.000.000, -";
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResponse.Location = new System.Drawing.Point(24, 38);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(275, 24);
            this.lblResponse.TabIndex = 11;
            this.lblResponse.Text = "Response Message Goes Here";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(636, 421);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 53);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnResend
            // 
            this.btnResend.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnResend.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnResend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResend.Location = new System.Drawing.Point(636, 267);
            this.btnResend.Name = "btnResend";
            this.btnResend.Size = new System.Drawing.Size(131, 53);
            this.btnResend.TabIndex = 13;
            this.btnResend.Text = "Resend Payment";
            this.btnResend.UseVisualStyleBackColor = false;
            this.btnResend.Visible = false;
            this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
            // 
            // lblTotalBalance
            // 
            this.lblTotalBalance.AutoSize = true;
            this.lblTotalBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalBalance.Location = new System.Drawing.Point(24, 62);
            this.lblTotalBalance.Name = "lblTotalBalance";
            this.lblTotalBalance.Size = new System.Drawing.Size(76, 17);
            this.lblTotalBalance.TabIndex = 14;
            this.lblTotalBalance.Text = "Total Point";
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountdown.ForeColor = System.Drawing.Color.Red;
            this.lblCountdown.Location = new System.Drawing.Point(20, 502);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(133, 20);
            this.lblCountdown.TabIndex = 15;
            this.lblCountdown.Text = "Countdown Label";
            // 
            // btnPauseCountdown
            // 
            this.btnPauseCountdown.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnPauseCountdown.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnPauseCountdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPauseCountdown.Location = new System.Drawing.Point(636, 421);
            this.btnPauseCountdown.Name = "btnPauseCountdown";
            this.btnPauseCountdown.Size = new System.Drawing.Size(131, 53);
            this.btnPauseCountdown.TabIndex = 16;
            this.btnPauseCountdown.Text = "Pause Timer";
            this.btnPauseCountdown.UseVisualStyleBackColor = false;
            this.btnPauseCountdown.Click += new System.EventHandler(this.btnPauseCountdown_Click);
            // 
            // CPRedeemEZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(804, 528);
            this.Controls.Add(this.btnPauseCountdown);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.lblTotalBalance);
            this.Controls.Add(this.btnResend);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.btnPayAll);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.numCurrNumpad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "CPRedeemEZ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pay Redeem Ezeelink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CPRedeemEZ_FormClosing);
            this.Load += new System.EventHandler(this.CPRedeemEZ_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (txtAmount.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Amount must be filled", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            if (txtCardNumber.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Card Number must be filled", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            if (int.Parse(txtAmount.Text) > (int)(((RetailTransaction)posTransaction).NetAmountWithTax - ((RetailTransaction)posTransaction).Payment))
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Amount must be less than total payment", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            lblResponse.Text = "Processing...";
            numCurrNumpad.Enabled = false;
            btnPayAll.Enabled = false;
            btnPay.Enabled = false;
            btnCancel.Enabled = false;
            btnCancel.Visible = false;
            btnResend.Enabled = false;

            try
            {
                if (sendPayment() == 1)
                {
                    timer.Interval = 30000; // here time in milliseconds
                    timer.Tick += timer_Tick;
                    timer.Start();

                    btnCheck.Enabled = true;

                    lblResponse.Text = "Requesting Customer Authentication... Press Check Payment to Confirm";
                }
                else
                {
                    numCurrNumpad.Enabled = true;
                    btnPayAll.Enabled = true;
                    btnPay.Enabled = true;
                    btnCheck.Enabled = false;
                    btnCancel.Enabled = true;
                    btnCancel.Visible = true;
                    btnPauseCountdown.Visible = false;
                    lblResponse.Text = "";
                }
            }
            catch
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("No Internet Connection to Redeem Point", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    numCurrNumpad.Enabled = true;
                    btnPayAll.Enabled = true;
                    btnPay.Enabled = true;
                    btnCheck.Enabled = false;
                    btnCancel.Enabled = true;
                    btnCancel.Visible = true;
                    btnPauseCountdown.Visible = false;
                    lblResponse.Text = "";

                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            btnPay.Enabled = false;
            btnPay.Visible = false;
            btnResend.Visible = true;
            btnResend.Enabled = true;
            timer.Stop();
        }

        private void checkBalance()
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string cardNumber = "";

            try
            {
                string queryString = "SELECT TOP 1 CARDNUMBER FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cardNumber = reader["CARDNUMBER"].ToString();
                        }
                        reader.Close();
                        reader.Dispose();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            getToken();

            Random rand = new Random();
            int responseCode = 0;
            string error_id = "";
            string M_TRANS_ID = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-" + rand.Next(1, 10000);
            string CARD_NUMBER = cardNumber;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
            string reqCheckUrl = EZ_GET_BALANCE;

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes("");

            HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
            reqCheck.Method = "POST";
            reqCheck.Credentials = CredentialCache.DefaultCredentials;
            reqCheck.Accept = "text/json";
            reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
            reqCheck.Headers["device_key"] = DEVICE_KEY;
            reqCheck.Headers["access_token"] = token;
            reqCheck.Headers["merchant_id"] = MERCHANT_ID;
            reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
            reqCheck.Headers["m_trans_id"] = M_TRANS_ID;
            reqCheck.Headers["card_number"] = CARD_NUMBER;
            reqCheck.UserAgent = USER_AGENT;
            reqCheck.ContentType = "application/json";
            reqCheck.ContentLength = data.Length;

            using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
            {
                string content = string.Empty;
                using (var stream = resp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic item = serializer.Deserialize<object>(content);

                responseCode = item["response_code"];

                if (responseCode == 200)
                {
                    error_id = item["response_data"]["error_id"];

                    if (error_id == "0000")
                    {
                        for (int i = 0; i < (item["response_data"]["results"]["pgm_info"]).Length; i++)
                        {
                            if (item["response_data"]["results"]["pgm_info"][i]["pgm_id"] == "10002") //Production
                            //if (item["response_data"]["results"]["pgm_info"][i]["pgm_id"] == "10000") //Development
                            {
                                Decimal.TryParse(item["response_data"]["results"]["pgm_info"][i]["point_bal"], out poinBal);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(item["response_data"]["error_text"], "Error Check Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(item["response_message"], "Error Check Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int sendPayment()
        {
            if (getToken() == 1)
            {
                Random rand = new Random();
                int responseCode = 0;
                string error_id = "";
                M_TRANS_ID_PAYMENT = "" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                string CARD_NUMBER = txtCardNumber.Text;

                if (convert == 0)
                {
                    MessageBox.Show("Send Payment Failed, try again");
                    return 0;
                }

                if ((int.Parse(txtAmount.Text) / convert) > poinBal)
                {
                    MessageBox.Show("Not Enough Balance");
                    return 0;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;
                string reqCheckUrl = EZ_REQUEST_REDEEM;

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");

                HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                reqCheck.Method = "POST";
                reqCheck.Credentials = CredentialCache.DefaultCredentials;
                reqCheck.Accept = "text/json";
                reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
                reqCheck.Headers["device_key"] = DEVICE_KEY;
                reqCheck.Headers["access_token"] = token;
                reqCheck.Headers["merchant_id"] = MERCHANT_ID;
                reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
                reqCheck.Headers["merchant_trans_id"] = M_TRANS_ID_PAYMENT;
                reqCheck.Headers["card_number"] = CARD_NUMBER;
                reqCheck.Headers["point"] = (int.Parse(txtAmount.Text) / convert).ToString();
                reqCheck.Headers["verify_type_id"] = "101";
                reqCheck.UserAgent = USER_AGENT;
                reqCheck.ContentType = "application/json";
                reqCheck.ContentLength = data.Length;

                using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                {
                    string content = string.Empty;
                    using (var stream = resp.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic item = serializer.Deserialize<object>(content);

                    responseCode = item["response_code"];

                    if (responseCode == 200)
                    {
                        error_id = item["response_data"]["error_id"];

                        if (error_id == "0000")
                        {
                            pendingTransID = item["response_data"]["results"]["pending_trans_id"];

                            return 1;
                        }
                        else
                        {
                            MessageBox.Show(item["response_data"]["error_text"], "Error Send Payment (1)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(item["response_message"], "Error Send Payment (2)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return 0;
        }

        private void checkMerchantSetting()
        {
            if (getToken() == 1)
            {
                Random rand = new Random();
                int responseCode = 0;
                string error_id = "";
                string M_TRANS_ID = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-" + rand.Next(1, 10000);
                string CARD_NUMBER = txtCardNumber.Text;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;
                string reqCheckUrl = EZ_GET_SETTING;

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");

                HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                reqCheck.Method = "POST";
                reqCheck.Credentials = CredentialCache.DefaultCredentials;
                reqCheck.Accept = "text/json";
                reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
                reqCheck.Headers["device_key"] = DEVICE_KEY;
                reqCheck.Headers["access_token"] = token;
                reqCheck.Headers["merchant_id"] = MERCHANT_ID;
                reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
                reqCheck.Headers["merchant_trans_id"] = M_TRANS_ID;
                reqCheck.UserAgent = USER_AGENT;
                reqCheck.ContentType = "application/json";
                reqCheck.ContentLength = data.Length;

                using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                {
                    string content = string.Empty;
                    using (var stream = resp.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic item = serializer.Deserialize<object>(content);

                    responseCode = item["response_code"];

                    if (responseCode == 200)
                    {
                        error_id = item["response_data"]["error_id"];

                        if (error_id == "0000")
                        {
                            for (int i = 0; i < (item["response_data"]["results"]["Point_Setting"]).Length; i++)
                            {
                                if ((item["response_data"]["results"]["Point_Setting"][i]["mmerchant_pgm_id"] + "") == "10002") // Production
                                //if (item["response_data"]["results"]["Point_Setting"][i]["mmerchant_pgm_id"] == 10000) // Development
                                {
                                    convert = (int)item["response_data"]["results"]["Point_Setting"][i]["Point_to_Cash"];
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(item["response_data"]["error_text"], "Error Check Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(item["response_message"], "Error Check Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private int check_status(ref int points, ref decimal balance, ref string transDateTime, ref string m_tran_id)
        {
            if (getToken() == 1)
            {
                Random rand = new Random();
                int responseCode = 0;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;
                string reqCheckUrl = EZ_REQUEST_STATUS;

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");

                HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                reqCheck.Method = "POST";
                reqCheck.Credentials = CredentialCache.DefaultCredentials;
                reqCheck.Accept = "text/json";
                reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
                reqCheck.Headers["device_key"] = DEVICE_KEY;
                reqCheck.Headers["access_token"] = token;
                reqCheck.Headers["merchant_id"] = MERCHANT_ID;
                reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
                reqCheck.Headers["pendingtrans_id"] = pendingTransID.ToString();
                reqCheck.UserAgent = USER_AGENT;
                reqCheck.ContentType = "application/json";
                reqCheck.ContentLength = data.Length;


                using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                {
                    string content = string.Empty;
                    using (var stream = resp.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic item = serializer.Deserialize<object>(content);

                    responseCode = item["response_code"];

                    if (responseCode == 200)
                    {
                        points = item["response_data"]["pendingtrans_point"];
                        balance = item["response_data"]["loyalty_balance"];
                        transDateTime = item["response_data"]["pendingtrans_update_date"];
                        m_tran_id = item["response_data"]["m_tran_id"];

                        int pendingTransStatusID = item["response_data"]["pendingtrans_transtatus_id"];

                        if (pendingTransStatusID == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            lblResponse.Text = "Payment pending, Please confirm customer authentication and check again...";
                        }
                    }
                    else
                    {
                        lblResponse.Text = "Failed";
                    }
                }
            }

            return 0;
        }

        private void getCardProfile()
        {
            if (getToken() == 1)
            {
                Random rand = new Random();
                int responseCode = 0;
                string error_id = "";
                string M_TRANS_ID = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-" + rand.Next(1, 10000);
                string CARD_NUMBER = txtCardNumber.Text;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;
                string reqCheckUrl = EZ_GET_PROFILE;

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");

                HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                reqCheck.Method = "POST";
                reqCheck.Credentials = CredentialCache.DefaultCredentials;
                reqCheck.Accept = "text/json";
                reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
                reqCheck.Headers["device_key"] = DEVICE_KEY;
                reqCheck.Headers["access_token"] = token;
                reqCheck.Headers["merchant_id"] = MERCHANT_ID;
                reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
                reqCheck.Headers["m_trans_id"] = M_TRANS_ID;
                reqCheck.Headers["card_number"] = CARD_NUMBER;
                reqCheck.UserAgent = USER_AGENT;
                reqCheck.ContentType = "application/json";
                reqCheck.ContentLength = data.Length;

                using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                {
                    string content = string.Empty;
                    using (var stream = resp.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic item = serializer.Deserialize<object>(content);

                    responseCode = item["response_code"];

                    if (responseCode == 200)
                    {
                        error_id = item["response_data"]["error_id"];

                        if (error_id == "0000")
                        {
                            string name = item["response_data"]["results"]["cardholder_name"];

                            cardholderName = name;
                        }
                        else
                        {
                            MessageBox.Show(item["response_data"]["error_text"], "Error Check Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(item["response_message"], "Error Check Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void timerCountdown_Tick(object sender, System.EventArgs e)
        {
            countDownCounter--;
            if (countDownCounter <= 0)
            {
                isFromAnotherFunction = 1;
                btnCheck_Click(btnCheck, EventArgs.Empty);
                isFromAnotherFunction = 0;
                this.Close();
            }
            lblCountdown.Text = "This window will close automatically in " + countDownCounter.ToString() + " second(s)";
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            lblResponse.Text = "Processing...";
            int points = 0;
            decimal balance = 0;
            string transDateTime = "";
            string m_tran_id = "";

            if (isCheckClick == 0 && isFromAnotherFunction == 0)
            {
                timerCountdown.Tick += new EventHandler(timerCountdown_Tick);
                timerCountdown.Interval = 1000;
                timerCountdown.Start();
                btnPauseCountdown.Visible = true;

                lblCountdown.Text = "This window will close automatically in " + countDownCounter.ToString() + " second(s)";
                isCheckClick = 1;
            }

            if (isSuccessPay == 1)
            {
                return;
            }


            if (check_status(ref points, ref balance, ref transDateTime, ref m_tran_id) == 1)
            {
                string customTenderType = "14";
                try
                {
                    //GET CUSTOMER NAME
                    getCardProfile();

                    //INSERT INTO AX.CPEZREDEEM
                    SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

                    try
                    {
                        string queryString = @"INSERT INTO AX.CPEZREDEEM(
                                                RECEIPTID,
                                                TRANSACTIONID,
                                                CARDNUMBER,
                                                AMOUNT,
                                                REDEEMPOINTS,
                                                CUSTOMERNAME,
                                                LASTBALANCE,
                                                TRANSDATETIMESTR,
                                                M_TRAN_ID,
                                                CHANNEL,
                                                STORE,
                                                TERMINAL,
                                                DATAAREAID,
                                                PARTITION,
                                                RECID
                                            ) 
                                            VALUES
                                            (
                                                @RECEIPTID,
                                                @TRANSACTIONID,
                                                @CARDNUMBER,
                                                @AMOUNT,
                                                @REDEEMPOINTS,
                                                @CUSTOMERNAME,
                                                @LASTBALANCE,
                                                @TRANSDATETIME,
                                                @M_TRAN_ID,
                                                @CHANNEL,
                                                @STORE,
                                                @TERMINAL,
                                                @DATAAREAID,
                                                @PARTITION,
                                                @RECID
                                            )";

                        using (SqlCommand cmd = new SqlCommand(queryString, connection))
                        {
                            cmd.Parameters.AddWithValue("@RECEIPTID", 1);
                            cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
                            cmd.Parameters.AddWithValue("@CARDNUMBER", txtCardNumber.Text);
                            cmd.Parameters.AddWithValue("@AMOUNT", decimal.Parse(txtAmount.Text));
                            cmd.Parameters.AddWithValue("@REDEEMPOINTS", points);
                            cmd.Parameters.AddWithValue("@CUSTOMERNAME", cardholderName);
                            cmd.Parameters.AddWithValue("@LASTBALANCE", balance);
                            cmd.Parameters.AddWithValue("@TRANSDATETIME", transDateTime);
                            cmd.Parameters.AddWithValue("@M_TRAN_ID", m_tran_id);
                            cmd.Parameters.AddWithValue("@CHANNEL", 1);
                            cmd.Parameters.AddWithValue("@STORE", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);
                            cmd.Parameters.AddWithValue("@TERMINAL", LSRetailPosis.Settings.ApplicationSettings.Database.TerminalID);
                            cmd.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);
                            cmd.Parameters.AddWithValue("@PARTITION", 1);
                            cmd.Parameters.AddWithValue("@RECID", 1);

                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Format Error", ex);
                    }
                    finally
                    {
                        if (connection != null)
                            connection.Dispose();
                    }

                    isSuccessPay = 1;

                    // cast the POSOperation object to RetailTransaction
                    RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                    // Create new tender line item object
                    LSRetailPosis.Transaction.Line.TenderItem.TenderLineItem tenderLineItem = new LSRetailPosis.Transaction.Line.TenderItem.TenderLineItem();

                    // your custom payment tender type
                    tenderLineItem.TenderTypeId = customTenderType;
                    ((ILineItem)tenderLineItem).Description = "Ezeelink Card";

                    // amount of transaction
                    tenderLineItem.Amount = decimal.Parse(txtAmount.Text);
                    tenderLineItem.CompanyCurrencyAmount = decimal.Parse(txtAmount.Text);

                    // add tender line object in current transaction
                    retailTransaction.Add(tenderLineItem);
                    ((PosTransaction)retailTransaction).LastRunOperationIsValidPayment = true;
                }
                catch (Exception ex)
                {
                    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Something went wrong, please try again. ", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                        return;
                    }
                }
                finally
                {
                    this.Close();
                }
            }
            /*else
            {
                btnCancel.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int resendPushNotif()
        {
            if (M_TRANS_ID_PAYMENT != "")
            {
                if (getToken() == 1)
                {
                    int responseCode = 0;
                    string error_id = "";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;
                    string reqCheckUrl = EZ_RESENT_PUSH_NOTIF;

                    ASCIIEncoding encoder = new ASCIIEncoding();
                    byte[] data = encoder.GetBytes("");

                    HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                    reqCheck.Method = "POST";
                    reqCheck.Credentials = CredentialCache.DefaultCredentials;
                    reqCheck.Accept = "text/json";
                    reqCheck.Headers["device_id"] = DEVICE_ID.ToString();
                    reqCheck.Headers["device_key"] = DEVICE_KEY;
                    reqCheck.Headers["access_token"] = token;
                    reqCheck.Headers["merchant_id"] = MERCHANT_ID;
                    reqCheck.Headers["merchant_key"] = MERCHANT_KEY;
                    reqCheck.Headers["merchant_trans_id"] = M_TRANS_ID_PAYMENT;
                    reqCheck.UserAgent = USER_AGENT;
                    reqCheck.ContentType = "application/json";
                    reqCheck.ContentLength = data.Length;

                    using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                    {
                        string content = string.Empty;
                        using (var stream = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                content = sr.ReadToEnd();
                            }
                        }
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        dynamic item = serializer.Deserialize<object>(content);

                        responseCode = item["response_code"];

                        if (responseCode == 200)
                        {
                            error_id = item["response_data"]["error_id"];

                            if (error_id == "0000")
                            {
                                return 1;
                            }
                            else
                            {
                                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(item["response_data"]["error_text"], MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                {
                                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                    return 0;
                                }
                            }
                        }
                        else
                        {
                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(item["response_message"], MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                return 0;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            isFromAnotherFunction = 1;
            btnCheck_Click(btnCheck, EventArgs.Empty);
            isFromAnotherFunction = 0;

            if (txtAmount.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Amount must be filled", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            if (txtCardNumber.Text == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Card Number must be filled", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            if (int.Parse(txtAmount.Text) > (int)(((RetailTransaction)posTransaction).NetAmountWithTax - ((RetailTransaction)posTransaction).Payment))
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Amount must be less than total payment", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            if (M_TRANS_ID_PAYMENT == "")
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Something went wrong, please try again", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    btnPay.Enabled = true;
                    btnPay.Visible = true;
                    btnResend.Visible = false;
                    btnResend.Enabled = false;
                    return;
                }
            }

            lblResponse.Text = "Processing...";
            numCurrNumpad.Enabled = false;
            btnPayAll.Enabled = false;
            btnPay.Enabled = false;
            btnCancel.Visible = false;
            btnResend.Enabled = false;

            if (resendPushNotif() == 1)
            {
                timer.Interval = 20000; // here time in milliseconds
                timer.Tick += timer_Tick;
                timer.Start();

                btnCheck.Enabled = true;

                lblResponse.Text = "Requesting Customer Authentication... Press Check Payment to Confirm";
            }
            else
            {
                numCurrNumpad.Enabled = true;
                btnPayAll.Enabled = true;
                btnPay.Enabled = true;
                btnCheck.Enabled = false;
                //btnCancel.Enabled = true;
                lblResponse.Text = "";
            }
        }

        private void numCurrNumpad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.Handled = false;
            }
        }

        private void btnPauseCountdown_Click(object sender, EventArgs e)
        {
            if (btnPauseCountdown.Text == "Pause Timer")
            {
                timerCountdown.Stop();
                lblCountdown.Text = "This window will close automatically in " + countDownCounter.ToString() + " second(s) - Paused";
                btnPauseCountdown.Text = "Resume Timer";
            }
            else if (btnPauseCountdown.Text == "Resume Timer")
            {
                timerCountdown.Start();
                lblCountdown.Text = "This window will close automatically in " + countDownCounter.ToString() + " second(s)";
                btnPauseCountdown.Text = "Pause Timer";
            }
        }

        private void CPRedeemEZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            timerCountdown.Stop();
        }
*/
    }
}
