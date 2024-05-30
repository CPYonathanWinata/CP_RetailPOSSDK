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
using LSRetailPosis.Settings;
using System.Text.RegularExpressions;

//Begin Add NEC-Hamzah
using System.Configuration;

using System.Drawing.Printing;
//End Add NEC-Hamzah
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

using LSRetailPosis.DataAccess;
//using Microsoft.Dynamics.Retail.Diagnostics;
//using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using LSRetailPosis.POSProcesses;

//using Microsoft.Dynamics.Retail.Pos.Customer;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPLoveRemittPayment : Form
    {
        IPosTransaction posTransaction;
        Timer timer = new System.Windows.Forms.Timer();
        Timer timerCountdown = new System.Windows.Forms.Timer();

        const string USER_AGENT = BlankOperations.USER_AGENT;

        const int DEVICE_ID = BlankOperations.LV_DEVICE_ID;
        const string DEVICE_KEY = BlankOperations.LV_DEVICE_KEY;
        const string MERCHANT_ID = BlankOperations.LV_MERCHANT_ID;
        const string MERCHANT_KEY = BlankOperations.LV_MERCHANT_KEY;

        const string LV_PAYMENT_INQUIRY_URL = BlankOperations.LV_PAYMENT_INQUIRY_URL;
        const string LV_PAYMENT_PROCESS_URL = BlankOperations.LV_PAYMENT_PROCESS_URL;

    //    int pendingTransID;
     //   int convert = 0;
        int isCheckClick = 0;
        int countDownCounter = 60;
        int isFromAnotherFunction = 0;
        int isSuccessPay = 0;
    //    decimal poinBal = 0;
    //    string token;
        int Offset = 0;

        string upLine1 = "";
        string upLine2 = "";
        string upLine3 = "";
        string upLine4 = "";
        string downLine1 = "";
        string downLine2 = "";
        string downLine3 = "";
        string trans_date = "";

        private static readonly Regex regex = new Regex(@"^\d+$");
     
        public CPLoveRemittPayment(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;     
        }

        private void CPLoveRemittPayment_Load(object sender, EventArgs e)
        {
            lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", (""));
            lblResponse.Text = "";
  
            btnCancel.Enabled = true;
            btnCancel.Visible = true;
   
            btnPayment.Visible = false;
            btnPayment.Enabled = false;
    
            isCheckClick = 0;
            lblCountdown.Text = "";
            isSuccessPay = 0;

        }
              
        private void InitializeComponent()
        {
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblPaymentCode = new System.Windows.Forms.Label();
            this.txtPaymentCode = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.lblFee = new System.Windows.Forms.Label();
            this.txtFee = new System.Windows.Forms.TextBox();
            this.lblCustName = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.lblTransId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.btnCopyReceipt = new System.Windows.Forms.Button();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtAmount
            // 
            this.txtAmount.Enabled = false;
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(309, 231);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new System.Drawing.Size(231, 26);
            this.txtAmount.TabIndex = 2;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(310, 215);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(102, 13);
            this.lblAmount.TabIndex = 3;
            this.lblAmount.Text = "Amount Transaction";
            // 
            // lblPaymentCode
            // 
            this.lblPaymentCode.AutoSize = true;
            this.lblPaymentCode.Location = new System.Drawing.Point(25, 165);
            this.lblPaymentCode.Name = "lblPaymentCode";
            this.lblPaymentCode.Size = new System.Drawing.Size(59, 13);
            this.lblPaymentCode.TabIndex = 4;
            this.lblPaymentCode.Text = "KodeBayar";
            // 
            // txtPaymentCode
            // 
            this.txtPaymentCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaymentCode.Location = new System.Drawing.Point(23, 181);
            this.txtPaymentCode.MaxLength = 20;
            this.txtPaymentCode.Name = "txtPaymentCode";
            this.txtPaymentCode.Size = new System.Drawing.Size(231, 26);
            this.txtPaymentCode.TabIndex = 5;
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCheck.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.Location = new System.Drawing.Point(636, 189);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(131, 53);
            this.btnCheck.TabIndex = 7;
            this.btnCheck.Text = "Check Payment";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(23, 51);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(359, 29);
            this.lblTotalAmount.TabIndex = 10;
            this.lblTotalAmount.Text = "Total Amount : Rp 100.000.000, -";
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResponse.Location = new System.Drawing.Point(24, 80);
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
            // btnPayment
            // 
            this.btnPayment.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnPayment.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayment.Location = new System.Drawing.Point(636, 267);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(131, 53);
            this.btnPayment.TabIndex = 13;
            this.btnPayment.Text = "Payment";
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Visible = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
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
            // lblFee
            // 
            this.lblFee.AutoSize = true;
            this.lblFee.Location = new System.Drawing.Point(310, 272);
            this.lblFee.Name = "lblFee";
            this.lblFee.Size = new System.Drawing.Size(64, 13);
            this.lblFee.TabIndex = 17;
            this.lblFee.Text = "Amount Fee";
            // 
            // txtFee
            // 
            this.txtFee.Enabled = false;
            this.txtFee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFee.Location = new System.Drawing.Point(309, 288);
            this.txtFee.Name = "txtFee";
            this.txtFee.ReadOnly = true;
            this.txtFee.Size = new System.Drawing.Size(231, 26);
            this.txtFee.TabIndex = 16;
            // 
            // lblCustName
            // 
            this.lblCustName.AutoSize = true;
            this.lblCustName.Location = new System.Drawing.Point(310, 164);
            this.lblCustName.Name = "lblCustName";
            this.lblCustName.Size = new System.Drawing.Size(82, 13);
            this.lblCustName.TabIndex = 19;
            this.lblCustName.Text = "Customer Name";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Enabled = false;
            this.txtCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.Location = new System.Drawing.Point(309, 180);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.ReadOnly = true;
            this.txtCustomerName.Size = new System.Drawing.Size(231, 26);
            this.txtCustomerName.TabIndex = 18;
            this.txtCustomerName.TextChanged += new System.EventHandler(this.txtCustomerName_TextChanged);
            // 
            // lblTransId
            // 
            this.lblTransId.AutoSize = true;
            this.lblTransId.Location = new System.Drawing.Point(25, 120);
            this.lblTransId.Name = "lblTransId";
            this.lblTransId.Size = new System.Drawing.Size(46, 13);
            this.lblTransId.TabIndex = 20;
            this.lblTransId.Text = "Trans Id";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(329, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 29);
            this.label1.TabIndex = 21;
            this.label1.Text = "PAYMENT LOVE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(310, 330);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Total Amount";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Enabled = false;
            this.txtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalAmount.Location = new System.Drawing.Point(309, 346);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(231, 26);
            this.txtTotalAmount.TabIndex = 22;
            // 
            // btnCopyReceipt
            // 
            this.btnCopyReceipt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCopyReceipt.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCopyReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyReceipt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyReceipt.Location = new System.Drawing.Point(636, 339);
            this.btnCopyReceipt.Name = "btnCopyReceipt";
            this.btnCopyReceipt.Size = new System.Drawing.Size(131, 53);
            this.btnCopyReceipt.TabIndex = 24;
            this.btnCopyReceipt.Text = "Copy Receipt";
            this.btnCopyReceipt.UseVisualStyleBackColor = false;
            this.btnCopyReceipt.Visible = false;
            this.btnCopyReceipt.Click += new System.EventHandler(this.btnCopyReceipt_Click);
            // 
            // txtRefNo
            // 
            this.txtRefNo.Enabled = false;
            this.txtRefNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefNo.Location = new System.Drawing.Point(309, 402);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.ReadOnly = true;
            this.txtRefNo.Size = new System.Drawing.Size(231, 26);
            this.txtRefNo.TabIndex = 25;
            this.txtRefNo.Visible = false;
            // 
            // lblRefNo
            // 
            this.lblRefNo.AutoSize = true;
            this.lblRefNo.Location = new System.Drawing.Point(312, 386);
            this.lblRefNo.Name = "lblRefNo";
            this.lblRefNo.Size = new System.Drawing.Size(41, 13);
            this.lblRefNo.TabIndex = 26;
            this.lblRefNo.Text = "Ref No";
            this.lblRefNo.Visible = false;
            // 
            // CPLoveRemittPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(804, 528);
            this.Controls.Add(this.lblRefNo);
            this.Controls.Add(this.txtRefNo);
            this.Controls.Add(this.btnCopyReceipt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTransId);
            this.Controls.Add(this.lblCustName);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblFee);
            this.Controls.Add(this.txtFee);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.btnPayment);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.txtPaymentCode);
            this.Controls.Add(this.lblPaymentCode);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.txtAmount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "CPLoveRemittPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payment LoveRemit";
            this.Load += new System.EventHandler(this.CPLoveRemittPayment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            btnPayment.Visible = true;
            btnPayment.Enabled = true;
            timer.Stop();
        }
        
        private void timerCountdown_Tick(object sender, System.EventArgs e)
        {
            countDownCounter--;
            if(countDownCounter == 0)
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

            txtAmount.Text = "";
            txtCustomerName.Text = "";
            txtFee.Text = "";
            lblTransId.Text = "";
            txtTotalAmount.Text = "";
            txtRefNo.Text = "";

            string responseCode = "0";     
            string message = "";    
            string response_time = "";
            string amount = "";
            string fee = "";
            string total_amount = "";
            string expired = "";
            string customer_name = "";
            string transId = "";
            string ref_no = "";

            string PAYMENT_CODE = txtPaymentCode.Text;

            if (regex.IsMatch(PAYMENT_CODE))
            {
                int pc_length = PAYMENT_CODE.Length;

                if (pc_length >= 10)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                    string reqCheckUrl = LV_PAYMENT_INQUIRY_URL;

                    ASCIIEncoding encoder = new ASCIIEncoding();
                    byte[] data = encoder.GetBytes("");

                    HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                    reqCheck.Method = "POST";
                    reqCheck.Credentials = CredentialCache.DefaultCredentials;
                    reqCheck.Accept = "text/json";
        
                    reqCheck.Headers["Authorization"]   = "PFM";
                
                    reqCheck.UserAgent = USER_AGENT;
                    reqCheck.ContentType = "application/json";
               
                    string requestBody = string.Format("{{\"store_code\":\"{0}\",\"data_area\":\"{1}\",\"terminal_id\":\"{2}\",\"payment_code\":\"{3}\",\"operator_id\":\"{4}\"}}", ApplicationSettings.Database.StoreID, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Database.TerminalID, PAYMENT_CODE, ApplicationSettings.Terminal.TerminalOperator.OperatorId);
                    byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

                    Stream requestStream = reqCheck.GetRequestStream();
                    requestStream.Write(byteArray, 0, byteArray.Length);
                    requestStream.Close();

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

                        responseCode    = item["response_code"].ToString();
                        transId         = item["trans_id"].ToString();
                        response_time   = item["response_time"].ToString();
                        amount          = item["amount"].ToString();
                        fee             = item["fee"].ToString();
                        total_amount    = item["total_amount"].ToString();
                        expired         = item["expired"].ToString();
                        customer_name   = item["customer_name"].ToString();
                        message         = item["message"].ToString();
                        trans_date      = item["trans_date"].ToString();
                        ref_no          = item["ref_no"].ToString();    

                        upLine1 = item["upLine1"].ToString();
                        upLine2 = item["upLine2"].ToString();
                        upLine3 = item["upLine3"].ToString();
                        upLine4 = item["upLine4"].ToString();
                        downLine1 = item["downLine1"].ToString();
                        downLine2 = item["downLine2"].ToString();
                        downLine3 = item["downLine3"].ToString();

                        if (responseCode == "00")
                        {
                            txtAmount.Text          = amount;
                            txtCustomerName.Text    = customer_name;
                            txtFee.Text             = fee;
                            txtTotalAmount.Text     = total_amount;

                            lblTransId.Text         = transId.ToString();
                            lblTotalAmount.Text     = "Total Amount : Rp " + string.Format("{0:#,0.00}", total_amount);

                            lblResponse.Text = "Inquiry Sucess";

                            btnPayment.Visible = true;
                            btnPayment.Enabled = true;

                            txtPaymentCode.Enabled = false;

                        }
                        else if (responseCode == "1004") // already paid
                        {
                            txtAmount.Text = amount;
                            txtCustomerName.Text = customer_name;
                            txtFee.Text = fee;
                            txtTotalAmount.Text = total_amount;
                            txtRefNo.Text = ref_no;

                            lblTransId.Text = transId.ToString();
                            lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", total_amount);

                            btnCopyReceipt.Visible = true;
                            btnCopyReceipt.Enabled = true;
                            txtPaymentCode.Enabled = false;
                            lblResponse.Text = item["message"];
                        }
                        else
                        {
                            MessageBox.Show(item["message"], "Error Check PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            btnPayment.Enabled = false;
                            btnPayment.Visible = false;
                            lblResponse.Text = item["message"];
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Payment Code must be 10 digit", "Error Check PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {                
                MessageBox.Show("Payment Code must be numeric", "Error Check PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            lblResponse.Text = "Processing...";
      
         //   Random rand = new Random();
            string responseCode  = "0";       
            string message       = "";      
            string response_time = "";
            string amount        = "";
            string fee           = "";
            string total_amount  = "";
         //   string expired = "";
            string customer_name = "";
            string transId       = "";
            string reference_no  = "";
            
            string PAYMENT_CODE     = txtPaymentCode.Text;
            string TRANSACTION_ID   = lblTransId.Text;
            string OPERATOR_ID      = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
           

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;

            string reqCheckUrl = LV_PAYMENT_PROCESS_URL;

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes("");

            HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
            reqCheck.Method         = "POST";
            reqCheck.Credentials    = CredentialCache.DefaultCredentials;
            reqCheck.Accept         = "text/json";
           
            reqCheck.Headers["Authorization"]   = "PFM";
        /*  reqCheck.Headers["store_code"]      = ApplicationSettings.Database.StoreID;
            reqCheck.Headers["data_area"]       = ApplicationSettings.Database.DATAAREAID;
            reqCheck.Headers["terminal_id"]     = ApplicationSettings.Database.TerminalID;
            reqCheck.Headers["transaction_Id"]  = TRANSACTION_ID;
            reqCheck.Headers["payment_code"]    = PAYMENT_CODE;
            reqCheck.Headers["operator_id"]     = OPERATOR_ID;

          */ 

            reqCheck.UserAgent      = USER_AGENT;
            reqCheck.ContentType    = "application/json";
        //    reqCheck.ContentLength  = data.Length;

            string requestBody = string.Format("{{\"store_code\":\"{0}\",\"data_area\":\"{1}\",\"terminal_id\":\"{2}\",\"transaction_id\":\"{3}\",\"payment_code\":\"{4}\",\"operator_id\":\"{5}\"}}", ApplicationSettings.Database.StoreID, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Database.TerminalID, TRANSACTION_ID, PAYMENT_CODE, OPERATOR_ID);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            Stream requestStream = reqCheck.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();


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

                responseCode    = item["response_code"].ToString();
                transId         = item["trans_id"].ToString();
                response_time   = item["response_time"].ToString();
                amount          = item["amount"].ToString();
                fee             = item["fee"].ToString();
                total_amount    = item["total_amount"].ToString();               
                customer_name   = item["customer_name"].ToString();
                reference_no    = item["reference_no"].ToString();
                message         = item["message"].ToString();
                trans_date      = item["trans_date"].ToString();
              

                if (responseCode == "00")
                {
                    txtAmount.Text = amount;
                    txtCustomerName.Text = customer_name;
                    txtFee.Text = fee;
                    txtTotalAmount.Text = total_amount;
                    lblTransId.Text = transId;

                    txtRefNo.Visible = true;
                    lblRefNo.Visible = true;

                    lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", total_amount);

                    MessageBox.Show("Payment Sucess", "Alert Payment", MessageBoxButtons.OK);

                    btnPayment.Enabled = true;
                    btnPayment.Visible = true;
                    lblResponse.Text = "Payment Sucess";

                    txtRefNo.Text = reference_no;

                    string s = this.ReceiptDocumentFormat(upLine1, upLine2, upLine3, upLine4, downLine1, downLine2, downLine3, customer_name, PAYMENT_CODE, total_amount, amount, fee, reference_no, transId, trans_date, "ori");

                    PrintDocument p = new PrintDocument();
                    PrintDialog pd  = new PrintDialog();
                    PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
                    Margins margins = new Margins(0, 0, 0, 0);

                    pd.Document = p;
                    pd.Document.DefaultPageSettings.PaperSize = psize;
                    pd.Document.DefaultPageSettings.Margins = margins;
                    p.DefaultPageSettings.PaperSize.Width = 600;
                    p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                    {   
                        e1.Graphics.DrawString(s, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    };

                    try
                    {  
                        p.Print();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception Occured While Printing", ex);
                    }

                    this.Close();
              
                }
                else
                {
                    MessageBox.Show(item["message"], "Error Process PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnPayment.Enabled = false;
                    btnPayment.Visible = false;
                                        
                    lblResponse.Text = "";
                }

                resetForm();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numCurrNumpad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '.')
            {
                e.Handled = false;
            }
        }

        private void CPLoveRemittPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            timerCountdown.Stop();
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void resetForm()
        {
            btnPayment.Enabled = false;
            btnPayment.Visible = false;

            btnCopyReceipt.Visible = false;
            btnCopyReceipt.Enabled = false;
            txtPaymentCode.Text = "";

            txtAmount.Text = "";
            txtCustomerName.Text = "";
            txtFee.Text = "";
            txtTotalAmount.Text = "";

            lblTransId.Text = "";
            lblTotalAmount.Text = "Total Amount : Rp ";

            txtRefNo.Text = "";
            lblResponse.Text = "";
        }

        private void btnCopyReceipt_Click(object sender, EventArgs e)
        {
           
            string customer_name    = txtCustomerName.Text;
            string total_amount     = txtTotalAmount.Text;
            string amount           = txtAmount.Text;
            string fee              = txtFee.Text;
            string ref_no           = txtRefNo.Text;
            string transId          = lblTransId.Text;
            string payment_code     = txtPaymentCode.Text;


            string s = this.ReceiptDocumentFormat(upLine1, upLine2, upLine3, upLine4, downLine1, downLine2, downLine3,customer_name, payment_code, total_amount, amount, fee, ref_no, transId, trans_date,"copy");

            PrintDocument p = new PrintDocument();
            PrintDialog pd  = new PrintDialog();
            PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
            Margins margins = new Margins(0, 0, 0, 0);

            pd.Document = p;
            pd.Document.DefaultPageSettings.PaperSize = psize;
            pd.Document.DefaultPageSettings.Margins = margins;
            p.DefaultPageSettings.PaperSize.Width = 600;
            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(s, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };

            try
            {
                p.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private string ReceiptDocumentFormat(string upLine1, string upLine2, string upLine3, string upLine4, string downLine1, string downLine2,string downLine3, string customer_name,string payment_code, string total_amount, string amount, string fee, string ref_no, string trans_id, string trans_date, string type)
        {
            string s = "";
            int x = 0;
            int y = 0;
            string spc = "";
            string storeId=  ApplicationSettings.Database.StoreID;
            string operator_name = ApplicationSettings.Terminal.TerminalOperator.Name;

            try
            {
                s += " " + upLine1 + Environment.NewLine +
                     " " + upLine2 + Environment.NewLine +
                     " " + upLine3 + Environment.NewLine +
                     " " + upLine4 + Environment.NewLine +                    
                     "                      CABANG:" + ApplicationSettings.Terminal.StoreName + Environment.NewLine ;
                
                s += "             -----------------------------------" + Environment.NewLine ;
                s += "                                " +trans_date + Environment.NewLine;
                if (type == "copy")
                {
                s += "             Copy Receipt                 "+ Environment.NewLine;
                }
                else
                {
                s += "                                      " + Environment.NewLine;
                }

                s += "             TransactionID: " + trans_id + Environment.NewLine +
                     "             staff:" + ApplicationSettings.Terminal.TerminalOperator.OperatorId +"-"+operator_name+Environment.NewLine + Environment.NewLine;

                s += "             Kode Bayar: " + payment_code + Environment.NewLine;
                s += "             Ref No: " + ref_no + Environment.NewLine + Environment.NewLine;
                s += "             Details" + Environment.NewLine;
                s += "             Customer Name: " + customer_name + Environment.NewLine;


                x = 15 - amount.Length;

                spc = "";

                for (y = 0; y < x ;y++)
                {
                    spc += " ";
                }

                s += "             Amount Transaction: " + spc + amount + Environment.NewLine;

                x = 35 - 12 - fee.Length;
                spc = "";
                for (y = 0; y < x; y++)
                {
                    spc += " ";
                }

                s += "             Amount Fee: " + spc + fee + Environment.NewLine;

                x = 35 - 14 - total_amount.Length;
                spc = "";
                for (y = 0; y < x; y++)
                {
                    spc += " ";
                }

                s += "             Total Amount: " + spc + total_amount + Environment.NewLine;

                s += "             -----------------------------------" + Environment.NewLine;

                s += " "+downLine1 + Environment.NewLine +
                     " "+downLine2 + Environment.NewLine +
                     " "+downLine3 + Environment.NewLine;


                Offset = Offset + 260;
            }
            catch (Exception ex)
            {
                throw new Exception("Format Error", ex);
            }

            return s;
        }

        

        
    }
}
