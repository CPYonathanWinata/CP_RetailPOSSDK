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
using APIAccess;

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
		private Timer timerOnlineOrder;
		private Timer timerBlibliOrder;
		private Timer timerCheckConnection;
        private Timer timerCheckTempTableAPI;

		private int notificationIntervalInMinutes = 1; // Change this value to set the interval
		public string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");

		public class CustomPopupForm : Form
		{
			private static CustomPopupForm currentPopup;
			private Timer timer;
			public IApplication thisApplication { get; set; }
			// This is now a constructor
			public CustomPopupForm(string message, string title, Timer timer, IApplication application)
			{
				thisApplication = application;
				this.timer = timer;

				// Set up the form's properties
				this.Text = title;
				this.Size = new System.Drawing.Size(400, 250); // Increase form size
				this.StartPosition = FormStartPosition.CenterScreen;

				// Label to display the message
				Label messageLabel = new Label();
				messageLabel.Text = message;
				messageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14); // Set font size
				messageLabel.Size = new System.Drawing.Size(360, 100); // Increase label size to fit form
				messageLabel.Location = new System.Drawing.Point(20, 20);
				this.Controls.Add(messageLabel);

				// Button to trigger the action
				Button actionButton = new Button();
				actionButton.Text = "BUKA ORDER";
				actionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14); // Set font size
				actionButton.Size = new System.Drawing.Size(240, 50); // Increase button size
				actionButton.Location = new System.Drawing.Point(80, 140); // Adjust button location
				actionButton.Click += ActionButton_Click;
				this.Controls.Add(actionButton);

			}

			// Method to show the form with a check for duplicates
			public static void ShowPopup(string message, string title, Timer timer, IApplication application)
			{
				// Check if the form is already open
				if (currentPopup == null || currentPopup.IsDisposed)
				{
					currentPopup = new CustomPopupForm(message, title, timer, application);
					currentPopup.Show();
				}
				else
				{
					// Bring the already open form to the front
					currentPopup.BringToFront();
				}
			}

			private void ActionButton_Click(object sender, EventArgs e)
			{
				// Close the current popup form
				this.Close();
				// Restart the timer
				timer.Start();
				// Perform the operation
				thisApplication.RunOperation(PosisOperations.SalesOrder, "ONLINE");
			}

			// Override OnClosed to reset the static reference when the form is closed
			protected override void OnClosed(EventArgs e)
			{
				base.OnClosed(e);
				currentPopup = null;  // Clear the reference when form is closed
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

	        //online Order - yonathan 08112024
	        int notifOnlineInterval = Convert.ToInt16(getFolderPathConfig(PathDirectory, "notifOnlineInterval"));
	        if (notifOnlineInterval != 0)
	        {
		        // Create a timer with the specified interval
		        timerOnlineOrder = new Timer();
		        timerOnlineOrder.Interval = notifOnlineInterval * 60 * 1000; // Convert minutes to milliseconds
		        timerOnlineOrder.Tick += Timer_TickOnlineOrder;

		        // Start the timer 
		        timerOnlineOrder.Start();
	        }
	        //end

	        //online Order - yonathan 08112024
	        int notifBlibliInterval = Convert.ToInt16(getFolderPathConfig(PathDirectory, "notifBlibliInterval"));
	        if (notifBlibliInterval != 0)
	        {
		        // Create a timer with the specified interval
		        timerBlibliOrder = new Timer();
		        timerBlibliOrder.Interval = notifBlibliInterval * 60 * 1000; // Convert minutes to milliseconds
		        timerBlibliOrder.Tick += Timer_TickBlibliOrder;

		        // Start the timer 
		        timerBlibliOrder.Start();
	        }
	        //end

	        //check temp table to send to API - Yonathan 22052026
            /*
            int checkTempTableAPI = Convert.ToInt16(getFolderPathConfig(PathDirectory, "checkTempTableAPI"));
            if (checkTempTableAPI != 0)
            {
                timerCheckTempTableAPI = new Timer();

                timerCheckTempTableAPI.Interval = checkTempTableAPI * 60 * 1000; // Convert minutes to milliseconds
                timerCheckTempTableAPI.Tick +=timerCheckTempTableAPI_Tick ;

                // Start the timer 
                timerCheckTempTableAPI.Start();
            }*/

	        //int notifCheckConnection = 1;
	        //if (notifCheckConnection != 0)
	        //{
	        //    // Create a timer with the specified interval
	        //    timerCheckConnection = new Timer();
	        //    timerCheckConnection.Interval = notifCheckConnection * 60 * 1000; // Convert minutes to milliseconds
	        //    timerCheckConnection.Tick += timerCheckConnection_Tick;

	        //    // Start the timer 
	        //    timerCheckConnection.Start(); 
	        //}

        }

        
        private void timerCheckTempTableAPI_Tick(object sender, EventArgs e)
        {
            string functionName = "AddItemMultipleAPI";
            string url = "";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            if (url == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your IT Admin", functionName));
            }

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                // STEP 1: Get distinct RECEIPTID list
                List<string> receiptIds = new List<string>();

                string queryReceiptIds = @"SELECT DISTINCT [RECEIPTID] FROM [ax].[CPINVENTORYONHAND_TEMP]";

                using (SqlCommand cmdReceipts = new SqlCommand(queryReceiptIds, connection))
                using (SqlDataReader readerReceipts = cmdReceipts.ExecuteReader())
                {
                    while (readerReceipts.Read())
                    {
                        receiptIds.Add(readerReceipts["RECEIPTID"].ToString());
                    }
                }

                // STEP 2-5: Loop each RECEIPTID
                foreach (string receiptId in receiptIds)
                {
                    var packList = new List<APIAccess.APIParameter.parmRequestAddItemMultiple>();

                    // STEP 2: Query records grouped by RECEIPTID
                    string queryLines = @"SELECT [RECEIPTID], [ITEMID], [QTY], [UNITID], [DATAAREA], [WAREHOUSE], [TRANSTYPE], [VARIANTID]
                                   FROM [ax].[CPINVENTORYONHAND_TEMP]
                                   WHERE [RECEIPTID] = @RECEIPTID";

                    using (SqlCommand cmdLines = new SqlCommand(queryLines, connection))
                    {
                        cmdLines.Parameters.AddWithValue("@RECEIPTID", receiptId);

                        using (SqlDataReader readerLines = cmdLines.ExecuteReader())
                        {
                            while (readerLines.Read())
                            {
                                var pack = new APIAccess.APIParameter.parmRequestAddItemMultiple
                                {
                                    ITEMID = readerLines["ITEMID"].ToString(),
                                    QTY = readerLines["QTY"].ToString().Replace(",", "."),
                                    UNITID = readerLines["UNITID"].ToString(),
                                    DATAAREAID = readerLines["DATAAREA"].ToString(),
                                    WAREHOUSE = readerLines["WAREHOUSE"].ToString(),
                                    TYPE = readerLines["TRANSTYPE"].ToString(),
                                    REFERENCESNUMBER = readerLines["RECEIPTID"].ToString(),
                                    RETAILVARIANTID = readerLines["VARIANTID"].ToString()
                                };

                                packList.Add(pack);
                            }
                        }
                    }

                    // STEP 3: Send to API with retry
                    int retryCount = 0;
                    bool isSuccess = false;
                    string result = "";

                    var multiData = APIAccess.APIFunction.MyJsonConverter.Serialize(packList);

                    while (retryCount < 3)
                    {
                        try
                        {
                            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                            httpRequest.Method = "POST";
                            httpRequest.ContentType = "application/json";
                            httpRequest.Headers.Add("Authorization", "PFM");

                            using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                            {
                                streamWriter.Write(multiData);
                                streamWriter.Flush();
                            }

                            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                            }

                            isSuccess = true;
                            break; // Success, exit retry loop
                        }
                        catch (Exception ex)
                        {
                            retryCount++;
                            LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        }
                    }

                    // STEP 4: Delete records if API succeeded
                    if (isSuccess)
                    {
                        string deleteQuery = @"DELETE FROM [ax].[CPINVENTORYONHAND_TEMP] WHERE [RECEIPTID] = @RECEIPTID";

                        using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, connection))
                        {
                            cmdDelete.Parameters.AddWithValue("@RECEIPTID", receiptId);
                            cmdDelete.ExecuteNonQuery();
                        }
                    }

                    // STEP 5: Continue to next RECEIPTID
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }

	   private void timerCheckConnection_Tick(object sender, EventArgs e)
	   {
		   // check RTS connection
		   bool statusTrans, statusAPI;
		   statusTrans = Application.TransactionServices.CheckConnection();

		   // check API Connection
		   string urlAPI = "https://apiqrisdev.cp.co.id";
		   try
		   {
			   var resp = ((HttpWebRequest)WebRequest.Create(urlAPI)).GetResponse();
			   statusAPI = true;
		   }
		   catch
		   {
			   statusAPI = false;
		   }

		   // update RTS label
		   if (statusTrans)
		   {
			   lblRTS.Text = "RTS Conn: ON";
			   lblRTS.ForeColor = System.Drawing.Color.Green; // hijau
		   }
		   else
		   {
			   lblRTS.Text = "RTS Conn: OFF";
			   lblRTS.ForeColor = System.Drawing.Color.Red; // merah
		   }

		   // update API label
		   if (statusAPI)
		   {
			   lblAPI.Text = "API Conn: ON";
			   lblAPI.ForeColor = System.Drawing.Color.Green; // hijau
		   }
		   else
		   {
			   lblAPI.Text = "API Conn: OFF";
			   lblAPI.ForeColor = System.Drawing.Color.Red; // merah
		   }
	   }

	   private void Timer_TickBlibliOrder(object sender, EventArgs e)
	   {
		   APIAccess.APIParameter.ApiResponseBliBliListOrder responseAPI;
		   bool detectDelivered = false;
		   
		   string url = "";
		   APIAccess.APIParameter.Receiver receiverParm;
		   string functionName = "GetBlibliOrderAPI";
		   APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
		   url = APIClass.getURLAPIByFuncName(functionName);

		   System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

		   responseAPI = APIAccess.APIFunction.BlibliOrderAPI.getBlibliOrderList(url, ApplicationSettings.Terminal.InventLocationId, DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
		   if (responseAPI.error == true)
		   {

			   if (responseAPI.message.IndexOf("Pickup Point", StringComparison.OrdinalIgnoreCase) < 0)
			   {

				   using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(responseAPI.message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
				   {
					   LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

					   timer.Stop();

					   return;
				   }
			   }
		   }
		   else
		   {
			   List<APIAccess.APIParameter.OrderData> order = responseAPI.data;

			   if (order != null && order.Count > 0)
			   {
				   PlayNotificationSound();

				   //DialogResult result = CustomMessageBox.Show("PESANAN BARU DITERIMA\nSILAKAN CEK GRABMART ORDER");

				   ShowPopupMessage("BLIBLI ORDER NOTIFICATION", string.Format("PESANAN BARU DITERIMA\nJANGAN LUPA CEK BLIBLI ORDER UNTUK MEMPROSES PESANAN", notificationIntervalInMinutes));

			   }
		   }
	   }

	   private void Timer_TickOnlineOrder(object sender, EventArgs e)
	   {
			APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
			APIAccess.APIFunction APIFunction = new APIAccess.APIFunction();
			string responseAPI;
			bool detectDelivered = false;
		  
			//"https://devpfm.cp.co.id/api/grab/listOrder"
			string url = "";//    https://apiqrisdev.cp.co.id/api/jbl/getTotalSalesOrder";
			APIAccess.APIParameter.Receiver receiverParm;
			string functionName = "GetOnlineOrderAPI";
		 
			url = APIClass.getURLAPIByFuncName(functionName);

			System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			//ServicePointManager.Expect100Continue = true;
			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			responseAPI = APIFunction.getOnlineOrder(Application.Settings.Database.DataAreaID, ApplicationSettings.Terminal.InventLocationId, url);
			APIParameter.parmResponseOnlineOrder responseOnlineOrder = APIFunction.MyJsonConverter.Deserialize<APIParameter.parmResponseOnlineOrder>(responseAPI);
			int resultData = responseOnlineOrder.data.total_order;

			if (resultData != 0 )
			{
				ShowPopupMessage("ONLINE ORDER NOTIFICATION", string.Format("Ada {0} Pesanan Online untuk toko ini.\nKlik tombol 'BUKA ORDER' untuk memproses Pesanan Online dan pilih Order Type 'Online Order'.", resultData));
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

				//disable untuk membuka status delivered agar masuk notif - yonathan 11112024
				//foreach (var orderList in order)
				//{
				//    detectDelivered = orderList.state == "DELIVERED" ? true : false;
				//}



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
		   


			if (title =="GRABMART ORDER NOTIFICATION" )
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
			else if (title == "ONLINE ORDER NOTIFICATION")
			{
				timer.Stop();
			  
				//CustomPopupForm popup = new CustomPopupForm(message, "ONLINE ORDER", timer, Application);

				CustomPopupForm.ShowPopup(message, "ONLINE ORDER", timer, Application);
				//popup.ShowDialog();
				
			}
			else if (title == "BLIBLI ORDER NOTIFICATION")
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
