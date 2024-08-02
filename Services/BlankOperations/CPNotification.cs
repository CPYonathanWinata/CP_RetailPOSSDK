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
using LSRetailPosis.Settings;
using System.Reflection;
using Microsoft.Dynamics.Retail.Pos.SalesOrder;
using System.Configuration;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System.Collections.ObjectModel;
using System.Net;
using System.Media;
using System.Xml;
using System.IO;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{

    [Export(typeof(IPosCustomControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CPNotification : UserControl, IPosCustomControl
    {
        [Import]

        public IApplication Application { get; set; }

        //test yonathan for timer 19/12/2023
        private Timer timer;
        private int notificationIntervalInMinutes = 1; // Change this value to set the interval
        public string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");


        public class CustomMessageBox : Form
        {
            private Button okButton;
            private Label messageLabel;

            public CustomMessageBox(string message)
            {
                // Set properties of the custom message box form
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                StartPosition = FormStartPosition.CenterScreen;
                Text = "GRABMART ORDER NOTIFICATION";

                // Create controls
                okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Dock = DockStyle.Bottom
                };

                messageLabel = new Label
                {
                    Text = message,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10)
                };

                // Add controls to the form
                Controls.Add(messageLabel);
                Controls.Add(okButton);

                // Event handler for the form load event
                Load += (sender, e) => CenterToScreen();
            }

            public static DialogResult Show(string message)
            {
                using (var customMessageBox = new CustomMessageBox(message))
                {
                    return customMessageBox.ShowDialog();
                }
            }
        }
        private bool validateIntegration()
        {
            bool integrationStatus = false;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string tenderName = "%GRABMART%";
            try
            {
                string queryStringID = @"SELECT 
	                                        TENDERTYPENAME,
	                                        ISINTEGRATION 
                                        FROM ax.CPEPAYMAPPING
                                        WHERE 
											 
		                                TENDERTYPENAME LIKE @TENDERNAME 
										AND STORENUMBER = @STORENUMBER";

                 

                using (SqlCommand command = new SqlCommand(queryStringID, connection))
                {
                    //command.Parameters.AddWithValue("@CUSTOMERID", this.customerID);
                    command.Parameters.AddWithValue("@TENDERNAME", tenderName);
                    command.Parameters.AddWithValue("@STORENUMBER", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Convert.ToString(reader["ISINTEGRATION"]) == "1")
                            {
                                integrationStatus =  true;
                            }
                            else
                            {
                                integrationStatus =  false;
                            }

                            //if (timeClockType == TimeClockType.BreakFlowStart)
                            //{
                            //    this.BreakActivity = Convert.ToString(reader["ACTIVITY"]);
                            //}
                            //else
                            //{
                            //    this.JobId = Convert.ToString(reader["JOBID"]);
                            //}
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return integrationStatus;
        }
       private void InitializeTimer()
        {
            if(validateIntegration() == true)
            {

                int notifInterval = Convert.ToInt16(getFolderPathConfig(PathDirectory, "notifInterval"));

                if (notifInterval != 0)
                {
                    // Create a timer with the specified interval
                    timer = new Timer();
                    timer.Interval = notifInterval * 60 * 1000; // Convert minutes to milliseconds
                    timer.Tick += Timer_Tick;

                    // Start the timer 
                    timer.Start();
                }
            }
            
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            APIAccess.APIParameter.ApiResponseGrabmart responseAPI;
            bool detectDelivered = false;
            // This method will be called every X minutes

            // Show a popup message
            // check order list
            //"https://devpfm.cp.co.id/api/grab/listOrder"
            string url = "";
            APIAccess.APIParameter.Receiver receiverParm;
            string functionName = "GetGRABMARTAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            responseAPI = APIAccess.APIFunction.GrabMartAPI.getOrderList(ApplicationSettings.Terminal.InventLocationId.ToString(), url);
            if (responseAPI.error == true)
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(responseAPI.message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    
                    timer.Stop();

                    return;
                }
            }
            else
            {
                APIAccess.APIParameter.Data[] order = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.Data[]>(responseAPI.data);

                foreach (var orderList in order)
                {
                    detectDelivered = orderList.state == "DELIVERED" ? true : false;
                }



                if (order.Length != 0 && detectDelivered == false)
                {
                    PlayNotificationSound();

                    //DialogResult result = CustomMessageBox.Show("PESANAN BARU DITERIMA\nSILAKAN CEK GRABMART ORDER");

                    ShowPopupMessage("GRABMART ORDER NOTIFICATION", string.Format("PESANAN BARU DITERIMA\nJANGAN LUPA CEK GRABMART ORDER UNTUK MEMPROSES PESANAN", notificationIntervalInMinutes));

                }
            }
            
            
            // You can perform other actions or show different messages here
        }

        public string getFolderPathConfig(string ProcessingDirectory, string typeFolder)
        {
            string Folder = "";

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(ProcessingDirectory);
            XmlNode xnodes = xdoc.SelectSingleNode("configuration");
            XmlNodeList xmlList = xnodes.SelectNodes("folderpath");

            foreach (XmlNode xmlNodeS in xmlList)
            {
                Folder += "," + xmlNodeS.Attributes.GetNamedItem(typeFolder).Value;
            }
            return Folder.Substring(1);
        }

        private void PlayNotificationSound()
        {

            string notifName = getFolderPathConfig(PathDirectory, "grabNotifSound");
            if (notifName != "")
            {
                try
                {
                    // Specify the path to the MP3 file
                    string mp3FilePath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extensions", "img"), notifName.ToString());
                
                    // Create a SoundPlayer instance and play the sound
                
                        using (SoundPlayer player = new SoundPlayer(mp3FilePath))
                        {
                            player.Play();
                        }
                     

                    //Console.WriteLine("Notification sound played!");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error playing notification sound: {ex.Message}");
                }
            }
        }

        private void ShowPopupMessage(string title, string message)
        {
            // Display a MessageBox with the specified title and message
            //MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            timer.Stop();

            // Show the popup message
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            // Check the result and restart the timer accordingly
            if (result == DialogResult.OK)
            {
                // User clicked OK, restart the timer
                timer.Start();
            }
            else if (result == DialogResult.Cancel)
            {
                timer.Start();  
            }
        }

        //end

        public CPNotification()
        {


            InitializeComponent();
            InitializeTimer();
        }

        public void getSalesData() //using realtime service
        {
            string sales_label = "SALES (0)";
            string param_local = "";
            
            //get packingslip data from local database
            SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryStringLocal = "SELECT SALESID FROM dbo.CPPACKINGSLIPFLAG ";
                queryStringLocal += "WHERE PRINTDATE >= GETDATE() - 30 ";

                using(SqlCommand commandLocal = new SqlCommand(queryStringLocal, connectionLocal))
                {
                    if(connectionLocal.State != ConnectionState.Open)
                    {
                        connectionLocal.Open();
                    }

                    using(SqlDataReader readerLocal = commandLocal.ExecuteReader())
                    {
                        while(readerLocal.Read())
                        {
                            if(param_local != "")
                            {
                                param_local += ", '" + readerLocal[0].ToString() + "'";
                            }
                            else
                            {
                                param_local = "'" + readerLocal[0].ToString() + "'";
                            }
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }

            //get salestable data from AX database
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;


            //change to using RTS to get the data from AX by Yonathan 20/12/2023
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                
                ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getSalesDataNotification", ApplicationSettings.Database.StoreID, param_local);

                sales_label = "Sales (" + containerArray[3].ToString() + ")";
                     
            }
            catch (SqlException ex)
            {
                btnSales.Text = sales_label;
                btnRefresh.Text = "Refresh";
                btnRefresh.Enabled = true;
                btnSales.Enabled = true;

                throw new Exception("Format Error", ex);                
            }

            btnSales.Text = sales_label;
        }

        public void getSalesDataOld() //using CPCONNECTION
        {
            string sales_label = "SALES (0)";
            string param_local = "";

            //get packingslip data from local database
            SqlConnection connectionLocal = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryStringLocal = "SELECT SALESID FROM dbo.CPPACKINGSLIPFLAG ";
                queryStringLocal += "WHERE PRINTDATE >= GETDATE() - 30 ";

                using (SqlCommand commandLocal = new SqlCommand(queryStringLocal, connectionLocal))
                {
                    if (connectionLocal.State != ConnectionState.Open)
                    {
                        connectionLocal.Open();
                    }

                    using (SqlDataReader readerLocal = commandLocal.ExecuteReader())
                    {
                        while (readerLocal.Read())
                        {
                            if (param_local != "")
                            {
                                param_local += ", '" + readerLocal[0].ToString() + "'";
                            }
                            else
                            {
                                param_local = "'" + readerLocal[0].ToString() + "'";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }

            //get salestable data from AX database
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                /*string queryDetail = @"SELECT DISTINCT SALESID
                                    FROM SALESLINE
                                    INNER JOIN INVENTDIM ON SALESLINE.INVENTDIMID = INVENTDIM.INVENTDIMID AND SALESLINE.DATAAREAID = INVENTDIM.DATAAREAID
                                    WHERE INVENTDIM.INVENTLOCATIONID NOT LIKE '%" + ApplicationSettings.Database.StoreID + "%'";

                string param_detail = "";

                using(SqlCommand commandDetail = new SqlCommand(queryDetail, connection))
                {
                    if(connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using(SqlDataReader readerDetail = commandDetail.ExecuteReader())
                    {
                        while(readerDetail.Read())
                        {
                            if(param_detail != "")
                            {
                                param_detail += ", '" + readerDetail[0].ToString() + "'";
                            }
                            else
                            {
                                param_detail = "'" + readerDetail[0].ToString() + "'";
                            }
                        }
                    }
                }*/
                //ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getSalesDataNotification", ApplicationSettings.Database.StoreID, param_local);

                string queryString = "SELECT COUNT(SALESID) AS COUNT_SALES FROM SALESTABLE ";
                queryString += "WHERE INVENTLOCATIONID LIKE '%" + ApplicationSettings.Database.StoreID + "%' ";
                queryString += "AND SHIPPINGDATEREQUESTED >= GETDATE() - 14 ";
                queryString += "AND SALESSTATUS = 2 ";
                /*queryString += @"AND SALESID NOT IN (
                                    SELECT DISTINCT SALESID
                                    FROM SALESLINE
                                    INNER JOIN INVENTDIM ON SALESLINE.INVENTDIMID = INVENTDIM.INVENTDIMID AND SALESLINE.DATAAREAID = INVENTDIM.DATAAREAID
                                    WHERE INVENTDIM.INVENTLOCATIONID NOT LIKE '%" + ApplicationSettings.Database.StoreID + "%')";*/

                /*if(param_detail != "")
                {
                    queryString += "AND SALESID NOT IN (" + param_detail + ") ";
                }*/

                if (param_local != "")
                {
                    queryString += "AND SALESID NOT IN (" + param_local + ") ";
                }

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sales_label = "Sales (" + reader[0].ToString() + ")";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                btnSales.Text = sales_label;
                btnRefresh.Text = "Refresh";
                btnRefresh.Enabled = true;
                btnSales.Enabled = true;

                throw new Exception("Format Error", ex);
            }

            btnSales.Text = sales_label;
        }
        public void LoadLayout(string layoutId)
        {
            //throw new NotImplementedException();
            //getSalesData();
        }

        public void TransactionChanged(Contracts.DataEntity.IPosTransaction transaction)
        {
            //test Yonathan 21/07/2023
            BlankOperations.globalposTransaction = transaction;
            //List<string> stringList = new List<string>();
            //string custId = "";
            //if (custId != "")
            //{
            //    RetailTransaction retailTransaction =  new RetailTransaction(ApplicationSettings.Database.StoreID, ApplicationSettings.Terminal.StoreCurrency, ApplicationSettings.Terminal.TaxIncludedInPrice, Application.Services.Rounding);
            //    RetailTransaction retailTransaction = transaction as RetailTransaction;
            //    PosApplication.Instance.RunOperation(PosisOperations.Customer, custId);
            //    PosApplication.Instance.RunOperation(PosisOperations.ItemSale, "10010001");
            //    retailTransaction.Save();
            //    retailTransaction.
            //}
            
            //throw new NotImplementedException();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Text = "Loading...";
            btnRefresh.Enabled = true;// false;
            btnSales.Enabled = false;
            getSalesData();
            btnRefresh.Text = "Refresh";
            btnRefresh.Enabled = true;
            btnSales.Enabled = true;
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            Application.RunOperation(PosisOperations.SalesOrder,"");
            //SalesOrder.WinFormsTouch.frmSalesOrder form = new SalesOrder.WinFormsTouch.frmSalesOrder();
            //form.ShowDialog();
        }
    }
}
