using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Drawing.Printing;


namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPCheckBalanceEZ : Form
    {/*
        const string USER_AGENT = BlankOperations.USER_AGENT;
        const int DEVICE_ID = BlankOperations.EZ_DEVICE_ID;
        const string DEVICE_KEY = BlankOperations.EZ_DEVICE_KEY;
        const string MERCHANT_ID = BlankOperations.EZ_MERCHANT_ID;
        const string MERCHANT_KEY = BlankOperations.EZ_MERCHANT_KEY;

        const string EZ_GET_TOKEN = BlankOperations.EZ_GET_TOKEN_PFM_URL;
        const string EZ_GET_BALANCE = BlankOperations.EZ_CHECK_BALANCE_URL;
        const string EZ_GET_PROFILE = BlankOperations.EZ_GET_CARD_PROFILE_URL;

        string token;
        string cardholderName = "";
        string cardNo = "";
        string poinBal = "0";
        int offset = 0;

        IPosTransaction posTransaction;
        public CPCheckBalanceEZ(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
        }

        private void checkBalance()
        {
            getToken();

            Random rand = new Random();
            int responseCode = 0;
            string error_id = "";
            string M_TRANS_ID = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-" + rand.Next(1, 10000);
            string CARD_NUMBER = txtSearch.Text;

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
                                poinBal = item["response_data"]["results"]["pgm_info"][i]["point_bal"] + "";
                                cardNo = item["response_data"]["results"]["pgm_info"][i]["card_no"];
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

        private void getToken()
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
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
        }

        private void getCardProfile()
        {
            getToken();

            Random rand = new Random();
            int responseCode = 0;
            string error_id = "";
            string M_TRANS_ID = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-" + rand.Next(1, 10000);
            string CARD_NUMBER = txtSearch.Text;

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                lblInfo.Text = "";
                lblInfo.Text = "Loading ... ";
                checkBalance();
                getCardProfile();

                lblInfo.Text = "Card Number : " + cardNo + Environment.NewLine;
                lblInfo.Text += "Customer Name : " + cardholderName + Environment.NewLine;
                lblInfo.Text += "Point Balance : " + poinBal;
            }
            else
            {
                MessageBox.Show("Please Scan Barcode", "Error Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtSearch.Text != "")
                {
                    lblInfo.Text = "";
                    lblInfo.Text = "Loading ... ";
                    checkBalance();
                    getCardProfile();

                    lblInfo.Text = "Card Number : " + cardNo + Environment.NewLine;
                    lblInfo.Text += "Customer Name : " + cardholderName + Environment.NewLine;
                    lblInfo.Text += "Point Balance : " + poinBal;
                }
                else
                {
                    MessageBox.Show("Please Scan Barcode", "Error Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                lblInfo.Text = "";
                lblInfo.Text = "Loading ... ";

                offset = 0;
                string s = this.PrintBalanceFormat();

                PrintDocument p = new PrintDocument();
                PrintDialog pd = new PrintDialog();
                //  PaperSize psize = new PaperSize("Custom", 100, Offset + 236);   
                PaperSize psize = new PaperSize("Custom", 100, offset + 100);
                Margins margins = new Margins(0, 0, 0, 0);

                pd.Document = p;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.Document.DefaultPageSettings.Margins = margins;
                p.DefaultPageSettings.PaperSize.Width = 600;
                p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                {
                    //e1.Graphics.DrawString(s, new Font("Courier New", 9), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left + 100, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                    e1.Graphics.DrawString(s, new Font("Lucida Console", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

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
            else
            {
                MessageBox.Show("Please Scan Barcode", "Error Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string PrintBalanceFormat()
        {
            checkBalance();
            getCardProfile();

            string maskCardNo = "";

            if(cardNo.Length > 4)
            {
                maskCardNo = cardNo.Substring(cardNo.Length - 4);
            }

            string receiptFormat = "";

            receiptFormat += Environment.NewLine;
            receiptFormat += Environment.NewLine;

            receiptFormat += "\t        Ezeelink Point Balance" + Environment.NewLine;

            receiptFormat += "\t        ";

            for (int i = 0; i < 42; i++)
            {
                receiptFormat += "*";
            }

            receiptFormat += Environment.NewLine;
            receiptFormat += "\t\t" + "Loyalty Card".PadLeft(20, ' ') + ": ";

            for (int i = 0; i < cardNo.Length - 4; i++)
            {
                receiptFormat += "*";
            }

            receiptFormat += maskCardNo + Environment.NewLine;
            receiptFormat += "\t\t" + "Customer".PadLeft(20, ' ') + ": " + cardholderName + Environment.NewLine;
            receiptFormat += "\t\t" + "Balance Point".PadLeft(20, ' ') + ": " + poinBal + Environment.NewLine;
            receiptFormat += Environment.NewLine;
            receiptFormat += Environment.NewLine;

            receiptFormat += "\t        ";
            for (int i = 0; i < 42; i++)
            {
                receiptFormat += "*";
            }

            offset = 240;

            return receiptFormat;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      * */
    }
}
