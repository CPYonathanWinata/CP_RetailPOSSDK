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
using System.Data.SqlClient;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPSearchCustomerEZ : Form
    {
        /*
        const string USER_AGENT = BlankOperations.USER_AGENT;

        const int DEVICE_ID = BlankOperations.EZ_DEVICE_ID;
        const string DEVICE_KEY = BlankOperations.EZ_DEVICE_KEY;
        const string MERCHANT_ID = BlankOperations.EZ_MERCHANT_ID;
        const string MERCHANT_KEY = BlankOperations.EZ_MERCHANT_KEY;

        const string EZ_GET_TOKEN = BlankOperations.EZ_GET_TOKEN_PFM_URL;
        const string EZ_GET_PROFILE = BlankOperations.EZ_GET_CARD_PROFILE_URL;

        string token = "";
        IPosTransaction posTransaction;

        string name = "";
        string cardNo = "";
        string mobilePhone = "";
        string address = "";
        string city = "";
        string province = "";
        string accountStatus = "";
        bool membership = false;

        public CPSearchCustomerEZ(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
        }

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

        private int getCardProfile()
        {
            if (getToken() == 1)
            {
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
                            name = item["response_data"]["results"]["cardholder_name"];
                            cardNo = item["response_data"]["results"]["card_no"];
                            mobilePhone = item["response_data"]["results"]["mobilephone"];
                            address = item["response_data"]["results"]["address"];
                            city = item["response_data"]["results"]["city"];
                            province = item["response_data"]["results"]["province"];
                            accountStatus = item["response_data"]["results"]["account_status"];
                            membership = item["response_data"]["results"]["membership"];

                            lblInfo.Text = "Card Number".PadRight(20, ' ') + ": " + cardNo + Environment.NewLine;
                            lblInfo.Text += "Cardholder Name".PadRight(20, ' ') + ": " + name + Environment.NewLine;
                            lblInfo.Text += "Mobile Phone".PadRight(20, ' ') + ": " + mobilePhone + Environment.NewLine;
                            lblInfo.Text += "Address".PadRight(20, ' ') + ": " + address + Environment.NewLine;
                            lblInfo.Text += "City".PadRight(20, ' ') + ": " + city + Environment.NewLine;
                            lblInfo.Text += "Province".PadRight(20, ' ') + ": " + province + Environment.NewLine;
                            lblInfo.Text += "Account Status".PadRight(20, ' ') + ": " + accountStatus + Environment.NewLine;
                            lblInfo.Text += "Membership".PadRight(20, ' ') + ": " + (membership ? "Member" : "Non Member") + Environment.NewLine;

                            return 1;
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

            return 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(getCardProfile() == 1)
            {
                btnSelect.Visible = true;
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtSearch.Text != "")
                {
                    if (getCardProfile() == 1)
                    {
                        btnSelect.Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please Scan Barcode", "Error Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"INSERT INTO CPEZSELECTEDCUSTOMER 
                                        (
                                            TRANSACTIONID,
                                            CARDNUMBER,
                                            NAME,
                                            MOBILEPHONE,
                                            ADDRESS,
                                            CITY,
                                            PROVINCE,
                                            ACCOUNTSTATUS,
                                            MEMBERSHIP
                                        )
                                        VALUES 
                                        (
                                            @TRANSACTIONID,
                                            @CARDNUMBER,
                                            @NAME,
                                            @MOBILEPHONE,
                                            @ADDRESS,
                                            @CITY,
                                            @PROVINCE,
                                            @ACCOUNTSTATUS,
                                            @MEMBERSHIP
                                        )";

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
                    cmd.Parameters.AddWithValue("@CARDNUMBER", cardNo);
                    cmd.Parameters.AddWithValue("@NAME", name);
                    cmd.Parameters.AddWithValue("@MOBILEPHONE", mobilePhone);
                    cmd.Parameters.AddWithValue("@ADDRESS", address);
                    cmd.Parameters.AddWithValue("@CITY", city);
                    cmd.Parameters.AddWithValue("@PROVINCE", province);
                    cmd.Parameters.AddWithValue("@ACCOUNTSTATUS", accountStatus);
                    cmd.Parameters.AddWithValue("@MEMBERSHIP", membership);

                    if(connection.State != ConnectionState.Open)
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
                connection.Close();
                this.Close();
            }
        }
         * */
    }
}
