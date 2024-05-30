using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using LSRetailPosis.Transaction;
using LSRetailPosis.POSControls;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ICustomer;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPVoucherCode : Form
    {
        [Import]

        public IApplication Application { get; set; }
        IPosTransaction posTransaction;
        string journalID    = "";
        string voucherCode  = "";
        string dataAreaID   = "";
        string description  = "";
        string itemID       = "";
        int is_exist_local  = 0;

        public CPVoucherCode(IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent();
            lblResponse.Text = "";

            posTransaction      = _posTransaction;
            Application = _application;
            btnValidate.Enabled = true;
            btnUse.Enabled      = false;
            txtVoucher.ReadOnly = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnValidate_Click(object sender, EventArgs e) //using RealTimeService by YNWA 25/09/2023
        {
            if (txtVoucher.Text == "")
            {
                errorMessage("Voucher Code must be filled");
                return;
            }

            is_exist_local = 0;

            //Validate Local DB for any duplicate voucher
            SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {

                string queryString = @"SELECT TOP 1 VOUCHERCODE FROM [AX].[CPRETAILTRANSACTIONVOUCHER] WHERE VOUCHERCODE = @VOUCHERCODE";

                using (SqlCommand cmd = new SqlCommand(queryString, connectionLocal))
                {
                    cmd.Parameters.AddWithValue("@VOUCHERCODE", txtVoucher.Text);

                    if (connectionLocal.State != ConnectionState.Open)
                    {
                        connectionLocal.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            is_exist_local = 1;
                            errorMessage("VOUCHER CODE HAS BEEN REDEEMED");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connectionLocal.State != ConnectionState.Closed)
                {
                    connectionLocal.Close();
                }
            }

            //Validate server DB for voucher
            //string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            DE customer = ((RetailTransaction)posTransaction).Customer;

            if (String.IsNullOrEmpty(customer.CustomerId))
            {
                errorMessage("Customer Not Found");
                return;
            }

            //SqlConnection connection = new SqlConnection(connectionString);
            
            try
            {

                ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("validateVoucherCode", txtVoucher.Text, customer.CustomerId);
                string format = "yyyy-MM-dd HH:mm:ss.fff";
                if (containerArray[2].ToString() == "true")
                {
                    DateTime fromDate = DateTime.ParseExact(containerArray[4].ToString(), format, System.Globalization.CultureInfo.InvariantCulture);  
                    DateTime toDate = DateTime.ParseExact(containerArray[5].ToString(), format, System.Globalization.CultureInfo.InvariantCulture);  
                    journalID = containerArray[3].ToString(); 
                    voucherCode = txtVoucher.Text;
                    dataAreaID = containerArray[7].ToString(); 
                    description = containerArray[6].ToString();
                    int redeemStatus = int.Parse(containerArray[8].ToString());  

                    if (DateTime.Compare(fromDate, DateTime.Now.Date) > 0)
                    {
                        errorMessage("VOUCHER CODE IS NOT ACTIVE");
                        return;
                    }

                    if (DateTime.Compare(toDate, DateTime.Now.Date) < 0)
                    {
                        string errormsg = string.Format("VOUCHER CODE IS EXPIRED {0}", toDate.ToShortDateString());
                        errorMessage(errormsg);
                        //errorMessage("VOUCHER CODE IS EXPIRED");
                        return;
                    }

                    if(redeemStatus != 0)
                    {
                        errorMessage("VOUCHER CODE HAS BEEN REDEEMED");
                        return;
                    }

                    if(is_exist_local == 1)
                    {
                        return;
                    }

                    lblResponse.Text = "VALID : " + description;
                    lblResponse.ForeColor = Color.Green;
                    btnValidate.Enabled = false;
                    btnUse.Enabled = true;
                    txtVoucher.ReadOnly = true;
                }
                else
                {
                    errorMessage("VOUCHER CODE IS NOT VALID");
                    return;
                }
                    
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
             
        }

        private void btnValidate_ClickOld(object sender, EventArgs e) //using CPCOnnection
        {
            if (txtVoucher.Text == "")
            {
                errorMessage("Voucher Code must be filled");
                return;
            }

            is_exist_local = 0;

            //Validate Local DB for any duplicate voucher
            SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {

                string queryString = @"SELECT TOP 1 VOUCHERCODE FROM [AX].[CPRETAILTRANSACTIONVOUCHER] WHERE VOUCHERCODE = @VOUCHERCODE";

                using (SqlCommand cmd = new SqlCommand(queryString, connectionLocal))
                {
                    cmd.Parameters.AddWithValue("@VOUCHERCODE", txtVoucher.Text);

                    if (connectionLocal.State != ConnectionState.Open)
                    {
                        connectionLocal.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            is_exist_local = 1;
                            errorMessage("VOUCHER CODE HAS BEEN REDEEMED");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connectionLocal.State != ConnectionState.Closed)
                {
                    connectionLocal.Close();
                }
            }

            //Validate server DB for voucher
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            DE customer = ((RetailTransaction)posTransaction).Customer;

            if (String.IsNullOrEmpty(customer.CustomerId))
            {
                errorMessage("Customer Not Found");
                return;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {

               

                //if(DateTime.Compare(Convert.ToDateTime(containerArray[5],),DateTime.Now.Date))
                string queryString = @" SELECT VC.PRICEDISCJOURNALID, VC.VOUCHERCODE, VC.FROMDATE, VC.TODATE, VC.DESCRIPTION, VC.DATAAREAID, VC.REDEEMSTATUS FROM CPEXTVOUCHERCODETABLE VC
                                        INNER JOIN PRICEDISCADMTRANS PDA ON VC.PRICEDISCJOURNALID = PDA.JOURNALNUM AND GETDATE() BETWEEN PDA.FROMDATE AND PDA.TODATE
                                        WHERE VC.VOUCHERCODE = @VOUCHERCODE AND PDA.ACCOUNTRELATION = @CUSTOMERID"; // AND VC.REDEEMSTATUS = 0 
                //                string queryString = @" SELECT VC.PRICEDISCJOURNALID, VC.VOUCHERCODE, VC.FROMDATE, VC.TODATE, VC.DESCRIPTION, VC.DATAAREAID, VC.REDEEMSTATUS FROM CPEXTVOUCHERCODETABLE VC
                //                                     INNER JOIN PRICEDISCADMTRANS PDA ON VC.PRICEDISCJOURNALID = PDA.JOURNALNUM WHERE VC.VOUCHERCODE = @VOUCHERCODE AND PDA.ACCOUNTRELATION = @CUSTOMERID"; // AND VC.REDEEMSTATUS = 0 

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@VOUCHERCODE", txtVoucher.Text);
                    command.Parameters.AddWithValue("@CUSTOMERID", customer.CustomerId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            DateTime fromDate = DateTime.Parse(reader["FROMDATE"].ToString());
                            DateTime toDate = DateTime.Parse(reader["TODATE"].ToString());
                            journalID = reader["PRICEDISCJOURNALID"].ToString();
                            voucherCode = txtVoucher.Text;
                            dataAreaID = reader["DATAAREAID"].ToString();
                            description = reader["DESCRIPTION"].ToString();
                            int redeemStatus = int.Parse(reader["REDEEMSTATUS"].ToString());
                            
                            if (DateTime.Compare(fromDate, DateTime.Now.Date) > 0)
                            {
                                errorMessage("VOUCHER CODE IS NOT ACTIVE");
                                return;
                            }

                            if (DateTime.Compare(toDate, DateTime.Now.Date) < 0)
                            {
                                string errormsg = string.Format("VOUCHER CODE IS EXPIRED {0}", toDate.ToShortDateString());
                                errorMessage(errormsg);
                                //errorMessage("VOUCHER CODE IS EXPIRED");
                                return;
                            }

                            if (redeemStatus != 0)
                            {
                                errorMessage("VOUCHER CODE HAS BEEN REDEEMED");
                                return;
                            }

                            if (is_exist_local == 1)
                            {
                                return;
                            }

                            lblResponse.Text = "VALID : " + description;
                            lblResponse.ForeColor = Color.Green;
                            btnValidate.Enabled = false;
                            btnUse.Enabled = true;
                            txtVoucher.ReadOnly = true;
                        }
                        else
                        {
                            errorMessage("VOUCHER CODE IS NOT VALID");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        private void errorMessage(string message)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                lblResponse.Text = message;
                lblResponse.ForeColor = Color.Red;
            }
        }
        private void btnUse_Click(object sender, EventArgs e) //using RealTimeService by YNWA 25/09/2023
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;

            //SqlConnection connection = new SqlConnection(connectionString);
            DE customer = ((RetailTransaction)posTransaction).Customer;

            try
            {
                ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("useVoucher", journalID, customer.CustomerId);               
                
                if (containerArray[2].ToString() == "true")

                {
                   
                    // disable by Yonathan to prevent adding the loop item to the voucher code TMP 20/03/2023
                    insertVoucherCodeTMP();
                }
                else
                {
                    errorMessage("ITEM NOT FOUND");
                }
                    
                
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
             
        }
        private void btnUse_ClickOld(object sender, EventArgs e) //using CPCOnnection
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            DE customer = ((RetailTransaction)posTransaction).Customer;

            try
            {
                //QUERY TO AX TABLE PRICEDISCADMTRANS TO CHECK THE VOUCHER JOURNAL NUMBER AND THE ITEM RELATED TO THE VOUCHER
                string queryString = @" SELECT ITEMRELATION, CAST(QUANTITYAMOUNTFROM AS INT) AS QUANTITYAMOUNTFROM, ACCOUNTRELATION
                                        FROM PRICEDISCADMTRANS
                                        WHERE JOURNALNUM = @JOURNALNUM AND ACCOUNTRELATION = @CUSTOMERID AND GETDATE() BETWEEN FROMDATE AND TODATE ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@JOURNALNUM", journalID);
                    command.Parameters.AddWithValue("@CUSTOMERID", customer.CustomerId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            /*while (reader.Read()) //LOOP THE ITEMS THAT RELATED TO THE VOUCHER
                            {
                                itemID = reader["ITEMRELATION"].ToString();
                                int qty = int.Parse(reader["QUANTITYAMOUNTFROM"].ToString());
                                string accountNum = reader["ACCOUNTRELATION"].ToString();
                                int flag = 0;
                                
                                if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
                                {
                                    for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
                                    {
                                        if (((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId == itemID)
                                        {
                                            flag = 1;
                                        }
                                    }
                                }

                                //ADD THE RELATED ITEM TO CART
                                if (flag == 0)
                                {
                                    PosApplication.Instance.RunOperation(PosisOperations.ItemSale, itemID);
                                }

                                POSFormsManager.ShowPOSStatusPanelText("Voucher Applied: " + description);

                                // INSERT INTO CPEXTVOUCHERCODETMP TABLE  TO FLAG THAT THIS  VOUCHER HAS BEEN USED                           

                                insertVoucherCodeTMP();


                                this.Close();
                            }*/// disable by Yonathan to prevent adding the loop item to the voucher code TMP 20/03/2023
                            insertVoucherCodeTMP(); 
                        }
                        else
                        {
                            errorMessage("ITEM NOT FOUND");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        

        private void insertVoucherCodeTMP()
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"INSERT INTO CPEXTVOUCHERCODETMP 
                                        (
                                            PRICEDISCJOURNALID,
                                            DATAAREAID,
                                            VOUCHERCODE,
                                            ITEMID
                                        )
                                        VALUES 
                                        (
                                            @PRICEDISCJOURNALID,
                                            @DATAAREAID,
                                            @VOUCHERCODE,
                                            @ITEMID
                                        )";

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@PRICEDISCJOURNALID", journalID);
                    cmd.Parameters.AddWithValue("@DATAAREAID", dataAreaID);
                    cmd.Parameters.AddWithValue("@VOUCHERCODE", voucherCode);
                    cmd.Parameters.AddWithValue("@ITEMID", itemID);

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
                this.Close();
            }
        }
    }
}
