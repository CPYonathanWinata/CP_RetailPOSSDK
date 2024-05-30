using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using System.Data.SqlClient;
using LSRetailPosis.Transaction;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ICustomer;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    [Export(typeof(IPosCustomControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CPCustomerNumber : UserControl, IPosCustomControl
    {
        public CPCustomerNumber()
        {
            InitializeComponent();
        }

        public void LoadLayout(string layoutId)
        {
            //throw new NotImplementedException();
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = "DELETE FROM CPEZSELECTEDCUSTOMER";

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
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
                connection.Close();
            }

            lblCustomerNo.Text = "";
        }



        public void TransactionChanged(Contracts.DataEntity.IPosTransaction transaction)
        {
            //throw new NotImplementedException();
            //change to Loyalty by Yonathan 29/01/2024 CPLOYALTYRECEIPT
            ReadOnlyCollection<object> containerArray = null;
            string loyaltyFullName = "", loyaltyNumber = "", loyaltyPhone = "", loyaltyPoint = "";
             ILoyaltyCardData loyaltyCardData;
              
                
            RetailTransaction retailTransaction = transaction as RetailTransaction;
            if (retailTransaction != null)
            {
                
                DE customer = retailTransaction.Customer;

                if (!String.IsNullOrEmpty(customer.CustomerId) && !String.IsNullOrEmpty(retailTransaction.LoyaltyItem.LoyaltyCardNumber))
                {
                    containerArray = PosApplication.Instance.TransactionServices.InvokeExtension("getLoyaltyDetail", retailTransaction.LoyaltyItem.LoyaltyCardNumber);
                    loyaltyCardData = PosApplication.Instance.Services.Loyalty.GetLoyaltyBalanceInfo(retailTransaction.LoyaltyItem.LoyaltyCardNumber);
                    loyaltyNumber = containerArray[3].ToString();
                    loyaltyFullName = containerArray[4].ToString();
                    loyaltyPoint = loyaltyCardData.BalancePoints.ToString("N");
                    loyaltyPhone = containerArray[6].ToString();

                    // Customer Number, Name and Phone
                    lblCustomerNo.Text = "Loyalty Number : ";
                    lblCustomerNo.Text += CPHideCharCardNumber( loyaltyNumber) + Environment.NewLine;
                    lblCustomerNo.Text += "Loyalty Name : ";
                    lblCustomerNo.Text += loyaltyFullName + Environment.NewLine;
                    lblCustomerNo.Text += "Phone : " + loyaltyPhone + Environment.NewLine;

                    // Point
                    lblCustomerNo.Text += "Available Point : " + loyaltyPoint ;
                }
                else
                {
                    lblCustomerNo.Text = "";
                    containerArray = null;
                }

            }
            else
            {
                lblCustomerNo.Text = "";
                containerArray = null;
            }
            
            //lblCustomerNo.Text += reader["CITY"] + ", " + reader["PROVINCE"] + Environment.NewLine;


            //disable ezeelink as it's not used anymore (yonathan 29/01/2024)
            /*
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = "SELECT TOP 1 * FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

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
                            // Customer Number, Name and Phone
                            lblCustomerNo.Text = "EzeelinkCardNumber : ";
                            lblCustomerNo.Text += reader["CARDNUMBER"] + Environment.NewLine;
                            lblCustomerNo.Text += "EzeelinkCardName : ";
                            lblCustomerNo.Text += reader["NAME"] + Environment.NewLine;
                            lblCustomerNo.Text += "Phone : " + reader["MOBILEPHONE"] + Environment.NewLine;

                            // Address
                            lblCustomerNo.Text += "Address : " + Environment.NewLine;
                            lblCustomerNo.Text += reader["ADDRESS"] + Environment.NewLine;
                            lblCustomerNo.Text += reader["CITY"] + ", " + reader["PROVINCE"] + Environment.NewLine;
                        }
                        else
                        {
                            lblCustomerNo.Text = "";
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

            */
        }

        private string CPHideCharCardNumber(string _strCard)
        {
            int lenCard = _strCard.Length;
            if(lenCard <=4)
            {
                string stringSymbol = new string('*', lenCard);
                return stringSymbol + _strCard.Substring(lenCard);
            }
            else
            {
                string stringSymbol = new string('*', lenCard - 4);
                return stringSymbol + _strCard.Substring(lenCard - 4);
            }

            

        }
    }
}
