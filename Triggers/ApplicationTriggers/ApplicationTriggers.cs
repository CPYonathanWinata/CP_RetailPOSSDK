/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

//Microsoft Dynamics AX for Retail POS Plug-ins 
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using System.Data.SqlClient;
using System.Data;
using LSRetailPosis.POSControls.Touch;
using System.Xml;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

using APIAccess;
using MySql.Data.MySqlClient;
using System.ServiceProcess;
using System.Threading;
using System.Collections.ObjectModel;



namespace Microsoft.Dynamics.Retail.Pos.ApplicationTriggers
{
	[Export(typeof(IApplicationTrigger))]
	public class ApplicationTriggers : IApplicationTrigger
	{

		/// <summary>
		/// IApplication instance.
		/// </summary>
		private IApplication application;
		public List<APIAccess.API> APIList { get; set; }


		/// <summary>
		/// Gets or sets the IApplication instance.
		/// </summary>
		[Import]
		public IApplication Application
		{
			get
			{
				return this.application;
			}
			set
			{
				this.application = value;
				InternalApplication = value;
			}
		}

		/// <summary>
		/// Gets or sets the static IApplication instance.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal static IApplication InternalApplication { get; private set; }

		public ApplicationTriggers()
		{
		}


		#region custom Yonathan
		class SecureStringManager
		{
			readonly Encoding _encoding = Encoding.Unicode;

			public string Unprotect(string encryptedString)
			{
				byte[] protectedData = Convert.FromBase64String(encryptedString);
				byte[] unprotectedData = ProtectedData.Unprotect(protectedData,
					null, DataProtectionScope.CurrentUser);

				return _encoding.GetString(unprotectedData);
			}

			public string Protect(string unprotectedString)
			{
				byte[] unprotectedData = _encoding.GetBytes(unprotectedString);
				byte[] protectedData = ProtectedData.Protect(unprotectedData,
					null, DataProtectionScope.CurrentUser);

				return Convert.ToBase64String(protectedData);
			}
		}


		#region custom class for en/decrypt pass
		public class TripleDes
		{
			readonly byte[] _key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
			readonly byte[] _iv = { 8, 7, 6, 5, 4, 3, 2, 1 };

			// define the triple des provider
			private readonly TripleDESCryptoServiceProvider _mDes = new TripleDESCryptoServiceProvider();

			// define the string handler
			private readonly UTF8Encoding _mUtf8 = new UTF8Encoding();

			// define the local property arrays
			private readonly byte[] _mKey;
			private readonly byte[] _mIv;

			/// <summary>
			/// Default constructor
			/// </summary>
			public TripleDes()
			{
				_mKey = _key;
				_mIv = _iv;
			}

			/// <summary>
			/// Parameterized constructor
			/// </summary>
			/// <param name="key"></param>
			/// <param name="iv"></param>
			public TripleDes(byte[] key, byte[] iv)
			{
				_mKey = key;
				_mIv = iv;
			}

			/// <summary>
			/// Encrypts the given byte array input
			/// </summary>
			/// <param name="input">Input value</param>
			/// <returns>Encrypted result</returns>
			public byte[] Encrypt(byte[] input)
			{
				return Transform(input, _mDes.CreateEncryptor(_mKey, _mIv));
			}

			/// <summary>
			/// Decrypts the given encrypted byte array input
			/// </summary>
			/// <param name="input">Encrypted byte array input</param>
			/// <returns>Decrypted result</returns>
			public byte[] Decrypt(byte[] input)
			{
				return Transform(input, _mDes.CreateDecryptor(_mKey, _mIv));
			}

			/// <summary>
			/// Encrypts the given string input
			/// </summary>
			/// <param name="text">Input value</param>
			/// <returns>Encrypted result</returns>
			public string Encrypt(string text)
			{
				byte[] input = _mUtf8.GetBytes(text);
				byte[] output = Transform(input, _mDes.CreateEncryptor(_mKey, _mIv));
				return Convert.ToBase64String(output);
			}

			/// <summary>
			/// Decrypts the given encrypted string input
			/// </summary>
			/// <param name="text">Encrypted string input</param>
			/// <returns>Decrypted result</returns>
			public string Decrypt(string text)
			{
				byte[] input = Convert.FromBase64String(text);
				byte[] output = Transform(input, _mDes.CreateDecryptor(_mKey, _mIv));
				return _mUtf8.GetString(output);
			}

			private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
			{
				// create the necessary streams
				using (MemoryStream memStream = new MemoryStream())
				{
					using (CryptoStream cryptStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write))
					{
						// transform the bytes as requested
						cryptStream.Write(input, 0, input.Length);
						cryptStream.FlushFinalBlock();
						// Read the memory stream andconvert it back into byte array
						memStream.Position = 0;
						byte[] result = memStream.ToArray();
						// close and release the streams
						memStream.Close();
						cryptStream.Close();
						// hand back the encrypted buffer
						return result;
					}
				}
			}
		}
		#endregion
		public void connectToDB(string _storeId, string _dataAreaId)
		{
			//string result = "";
			string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
			string connectionString = getFolderPathConfig(PathDirectory, "connectionString");
			string passWord = getFolderPathConfig(PathDirectory, "passSQL");
			string hasil = "";
			string pass = "";
			string secret = "prima2022";
			string funcname = "";
			string dataAreaId = _dataAreaId;

			string storeId = _storeId;
			MySqlConnection connection;
			//SecureStringManager stringManager = new SecureStringManager();
			//hasil = stringManager.Unprotect(passWord);
			//pass = stringManager.Unprotect(hasil);
			//hasil = pass.Substring(4);

			hasil = passWord;
			TripleDes tripleDesClass = new TripleDes();
			//hasil = tripleDesClass.Encrypt(passWord);

			hasil = tripleDesClass.Decrypt(passWord);


			hasil = hasil.Replace(secret, "").Replace("  ", " ");

			connectionString += "pwd=" + hasil + ";";
			//connectionString = "Server=10.204.3.174;Port=3306;Database=api_dev;CharSet=latin1;Allow User Variables=True;uid=pfi;pwd=freshmart01";
			connection = new MySqlConnection(connectionString);
			try
			{

				string queryString = @" SELECT DATAAREAID, STOREID, FUNCNAME, URL, ISACTIVE, ISUSINGSERVICEREFERENCE, SERVICEREFERENCENAME FROM CPURLCONFIG WHERE STOREID = @STOREID && ISACTIVE = '1' && DATAAREAID =  @DATAAREAID";


				using (MySqlCommand command = new MySqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@STOREID", storeId);
					command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);

					if (connection.State != System.Data.ConnectionState.Open)
					{
						connection.Open();

					}
					APIAccess.API resultData = new APIAccess.API();
					using (MySqlDataReader reader = command.ExecuteReader())
					//MySqlDataReader reader = command.ExecuteReader();
					{
						if (reader.HasRows)
						{
							while (reader.Read())
							{
								resultData.URL = reader["URL"].ToString();
								resultData.FuncName = reader["FUNCNAME"].ToString();

								APIAccess.APIAccessClass.result.Add(resultData);
								//result
								//APIList.Add(result);

								APIAccess.APIAccessClass.resultURL.Add(reader["URL"].ToString());
								APIAccess.APIAccessClass.resultFuncName.Add(reader["FUNCNAME"].ToString());
								APIAccess.APIAccessClass.resultIsSvcRef.Add(reader["ISUSINGSERVICEREFERENCE"].ToString());
								APIAccess.APIAccessClass.resultSvcRefName.Add(reader["SERVICEREFERENCENAME"].ToString());

							}

							//connection.Dispose();

						}
						else
						{
							using (frmMessage dialog = new frmMessage(string.Format("Tidak ada data toko {0} pada konfigurasi API Table.\nHubungi Tim IT untuk lebih lanjut", storeId), MessageBoxButtons.OK, MessageBoxIcon.Error))
							{
								LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
								application.RunOperation(PosisOperations.ApplicationExit, "");
								

							}
						}
					}
					//connection.Close();
				}


			}
			catch (Exception ex)
			{
				//LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
			finally
			{
				if (connection.State != System.Data.ConnectionState.Closed)
				{
					connection.Dispose();
					connection.Close();

				}
			}
			//APIAccessClass.resultData
			//return result;

		}
		public string getFolderPath(string ProcessingDirectory, string typeFolder)
		{
			string Folder = "";

			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(ProcessingDirectory);
			XmlNode xnodes = xdoc.SelectSingleNode("configuration");
			XmlNodeList xmlList = xnodes.SelectNodes("AxRetailPOS");

			foreach (XmlNode xmlNodeS in xmlList)
			{
				Folder += "," + xmlNodeS.Attributes.GetNamedItem(typeFolder).Value;
			}
			return Folder.Substring(1);
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


		public void getDataSql()
		{
			string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pos.exe.config");
			string storeId = getFolderPath(PathDirectory, "StoreId");
			string dataAreaId = Application.Settings.Database.DataAreaID.ToString();
			bool result = true;
			//APIAccess.MySQLConnect connectSQL = new APIAccess.MySQLConnect();

			//connectSQL.connectToDB(storeID, dataAreaId);

			connectToDB(storeId, dataAreaId);

			result = checkTaxTable(storeId, dataAreaId);
			

            result = checkCustomTables(storeId, dataAreaId);
            if (result == false)
            {
                application.RunOperation(PosisOperations.ApplicationExit, "Close");
            }
		}

        private bool checkCustomTables(string storeId, string dataAreaId)
        {
            // Flag to indicate if all tables exist
            bool allExist = true;

            // List of semicolon-separated table names
            ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getListTablePOS");
           
            string tableNames = containerArray[3].ToString(); //"ax.CPRETAILPOSBATCHTABLEEXTENDS;ax.CPPROMOCASHBACSK;ax.CPPOSONLINEORDER";
            string[] tables = tableNames.Split(';'); // Split the string into individual table names

            APIAccess.APIFunction.DatabaseHelper dbHelper = new APIAccess.APIFunction.DatabaseHelper(ApplicationSettings.Database.LocalConnectionString);

            // List to collect missing table names
            List<string> missingTables = new List<string>();

            foreach (string tableName in tables)
            {
                bool exists = dbHelper.CheckExistTable(tableName);

                if (!exists)
                {
                    // Remove the 'ax.' prefix before adding to the list of missing tables
                    string displayTableName = tableName.StartsWith("ax.") ? tableName.Substring(3) : tableName;
                    missingTables.Add(displayTableName);
                    allExist = false; // Set the flag to false
                }
            }

            // If there are missing tables, show a dialog with their names
            if (missingTables.Count > 0)
            {
                string missingTablesMessage = "Tidak bisa masuk aplikasi POS. Table di bawah ini tidak exist di POS, silakan kontak ke IT Support:\n";
                for (int i = 0; i < missingTables.Count; i++)
                {
                    missingTablesMessage += string.Format("{0}. {1}\n", i + 1, missingTables[i]);

                }

                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(missingTablesMessage, MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                }
            }

            return allExist;
        }


		private bool checkTaxTable(string storeId, string dataAreaId)
		{
			SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{               
				string queryString = @"SELECT * FROM TAXTABLE JOIN TAXDATA
							ON TAXTABLE.TAXCODE = TAXDATA.TAXCODE
							WHERE TAXTABLE.DATAAREAID = @DATAAREAID";

				using (SqlCommand command = new SqlCommand(queryString, connectionStore))
					{
					 
						command.Parameters.AddWithValue("@DATAAREAID", dataAreaId );
				   

						if (connectionStore.State != ConnectionState.Open)
						{
							connectionStore.Open();
						}

						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (!reader.HasRows)
							{

								using (frmMessage dialog = new frmMessage(string.Format("Tidak ada data TAXTABLE/TAXDATA\npada toko {0}.\nHubungi Tim IT untuk melakukan Sync Data.", storeId), MessageBoxButtons.OK, MessageBoxIcon.Error))
								{
									LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
									return false;

								}
							}
							else
							{
								return true;
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
				if (connectionStore.State != ConnectionState.Closed)
				{
					connectionStore.Close();
				}
			}


		}
		#endregion

		#region IApplicationTriggers Members

		public void ApplicationStart()
		{
			string source = "IApplicationTriggers.ApplicationStart";
			string value = "Application has started";
			LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Debug);
			LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);

			// If the store is in Brazil, we should only allow to run the POS if the Functionality profile's ISO locale is Brazil
			if (ApplicationSettings.Terminal.StoreCountry.Equals(SupportedCountryRegion.BR.ToString(), StringComparison.OrdinalIgnoreCase)
				&& Functions.CountryRegion != SupportedCountryRegion.BR)
			{
				var message = ApplicationLocalizer.Language.Translate(85082, // The locale must be Brazil. In the POS functionality profile form, on the General tab, in the Locale field, select Brazil.
																	  ApplicationSettings.Terminal.StoreCountry);

				using (var form = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error))
				{
					LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(form);
				}

				throw new LSRetailPosis.PosStartupException(message);
			}

			//add by Yonathan for connecting to SQL
			getDataSql();
			//end add
			APIFunction.getMysqlConn(application);//add by Yonathan to lookup for API URL 08/08/2023


			//getAPIURL();
			//end

			//addEndPointBinding();
			//addNodeTcpBinding();
		}



		public void ApplicationStop()
		{
			string source = "IApplicationTriggers.ApplicationStop";
			string value = "Application has stopped";
			LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Debug);
			LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);
		}

		public void PostLogon(bool loginSuccessful, string operatorId, string name)
		{
			string source = "IApplicationTriggers.PostLogon";
			loginSuccessful = false;

			string value = loginSuccessful ? "User has successfully logged in. OperatorID: " + operatorId : "Failed user login attempt. OperatorID: " + operatorId;
			LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Debug);
			LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);
		}


		public void PreLogon(IPreTriggerResult preTriggerResult, string operatorId, string name)
		{
			LSRetailPosis.ApplicationLog.Log("IApplicationTriggers.PreLogon", "Before the user has been logged on...", LSRetailPosis.LogTraceLevel.Trace);
			//added customization by Yonathan 21-09-2022 for validate whether the warehouse is already sync
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			string storeId = "";
			string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pos.exe.config");
			storeId = getFolderPath(PathDirectory, "StoreId");
			string warehouseId = "";
			string siteId = "";
			string warehouseName = "";
			string dataAreaId = getFolderPath(PathDirectory, "DATAAREAID");
			//var retailTransaction = posTransaction as RetailTransaction;
			try
			{

				string queryString = @" SELECT A.INVENTLOCATION, C.NAME, A.INVENTLOCATIONDATAAREAID, C.INVENTSITEID 
							FROM ax.RETAILCHANNELTABLE A, ax.RETAILSTORETABLE B, ax.INVENTLOCATION C
							WHERE A.RECID=B.RECID AND C.INVENTLOCATIONID=A.INVENTLOCATION AND B.STORENUMBER=@STOREID AND A.INVENTLOCATIONDATAAREAID =@DATAAREAID";
				string queryString2 = @"SELECT A.INVENTLOCATION, A.INVENTLOCATIONDATAAREAID
							FROM ax.RETAILCHANNELTABLE A, ax.RETAILSTORETABLE B
							WHERE A.RECID=B.RECID AND  B.STORENUMBER=@STOREID  AND A.INVENTLOCATIONDATAAREAID =@DATAAREAID";
				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@STOREID", storeId);
					command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();

					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							//siteId = reader["INVENTSITEID"].ToString();
							//warehouseId = reader["A.INVENTLOCATION"].ToString();
							warehouseName = reader["NAME"].ToString();
						}

					}
				}
				if (warehouseName == "")
				{
					using (SqlCommand command = new SqlCommand(queryString2, connection))
					{
						command.Parameters.AddWithValue("@STOREID", storeId);
						command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);
						if (connection.State != ConnectionState.Open)
						{
							connection.Open();

						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								//siteId = reader["INVENTSITEID"].ToString();
								warehouseId = reader["INVENTLOCATION"].ToString();
								//warehouseName = reader["C.NAME"].ToString();
							}

						}

					}
					preTriggerResult.Message = warehouseId.ToString() + " Belum terdaftar, silakan kontak IT Support terkait";
					preTriggerResult.ContinueOperation = false;

					/*
					LogOnStatus logonStatus = LogOnStatus.None;
					LogOnConfirmation logOnConfirmation = new LogOnConfirmation();

					InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(logOnConfirmation, () =>
					{
						if (logOnConfirmation.Confirmed)
						{
							logonStatus = (LogOnStatus)logOnConfirmation.LogOnStatus;
						}
					}
					);

					Application.Services.Interaction.InteractionRequest(request);
					*/

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
			//warehouseId = "";

			//end customization
		}

		public void Logoff(string operatorId, string name)
		{
			string source = "IApplicationTriggers.Logoff";
			string serviceName = "CommerceDataExchangeAsyncClientService";
			string value = "User has successfully logged off. OperatorID: " + operatorId;
			LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Debug);
			LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);

			//StopService(serviceName);
			//Thread.Sleep(2000);
			//StartService(serviceName);
			//Thread.Sleep(2000);
		}

		public void LoginWindowVisible()
		{
			LSRetailPosis.ApplicationLog.Log("IApplicationTriggers.LoginWindowVisible", "When the login window is visible", LSRetailPosis.LogTraceLevel.Trace);
		}

		#endregion

		private void ShowPopupMessage(string title, string message)
		{
			// Display a MessageBox with the specified title and message
			//MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



			// Show the popup message
			DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			// Check the result and restart the timer accordingly

		}

		private void StopService(string serviceName)
		{
			using (ServiceController serviceController = new ServiceController(serviceName))
			{
				try
				{
					if (serviceController.Status == ServiceControllerStatus.Running)
					{
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
						ShowPopupMessage("Infolog", string.Format("Success stopping service '{0}'", serviceName));
					}
					else
					{
						ShowPopupMessage("Infolog", string.Format("Service '{0}' is already stopped.", serviceName));
					}
				}
				catch (Exception ex)
				{
					ShowPopupMessage("Infolog", string.Format("Error stopping service '{0}': {1}", serviceName, ex.Message));
				}
			}
		}

		private void StartService(string serviceName)
		{
			using (ServiceController serviceController = new ServiceController(serviceName))
			{
				try
				{
					if (serviceController.Status == ServiceControllerStatus.Stopped)
					{
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
						ShowPopupMessage("Infolog", string.Format("Success starting service '{0}'", serviceName));
					}
					else
					{
						ShowPopupMessage("Infolog", string.Format("Service '{0}' is already running.", serviceName));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error starting service '{0}': {1}", serviceName, ex.Message);
				}
			}
		}


		#region method customization by Yonathan 20/09/2022
		private void addEndPointBinding()
		{
			// Get the configuration file path
			string configFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pos.exe.config");

			// Load the configuration file
			XmlDocument configXml = new XmlDocument();
			configXml.Load(configFilePath);

			// Get the client node
			XmlNode clientNode = configXml.SelectSingleNode("//system.serviceModel/client");

			// Create a new endpoint element with the required attributes
			XmlElement endpointElement = configXml.CreateElement("endpoint");
			endpointElement.SetAttribute("address", "net.tcp://dynamics02:8201/DynamicsAx/Services/CPGetStockOnHandGroupPFMJKT12");
			endpointElement.SetAttribute("binding", "netTcpBinding");
			endpointElement.SetAttribute("bindingConfiguration", "NetTcpBinding_CPGetStockOnHandSvcPFMJKT12");
			endpointElement.SetAttribute("contract", "CPGetStockOnHandGroupPFMJKT12.CPGetStockOnHandSvcPFMJKT12");
			endpointElement.SetAttribute("name", "NetTcpBinding_CPGetStockOnHandSvcPFMJKT12");

			// Create a new identity element with the required attribute
			XmlElement identityElement = configXml.CreateElement("identity");
			XmlElement userPrincipalNameElement = configXml.CreateElement("userPrincipalName");
			userPrincipalNameElement.SetAttribute("value", "AOSServiceAcc@primarti.com");

			// Append the userPrincipalName element to the identity element
			identityElement.AppendChild(userPrincipalNameElement);

			// Append the identity and endpoint elements to the client node
			endpointElement.AppendChild(identityElement);
			clientNode.AppendChild(endpointElement);

			// Save the modified configuration file
			configXml.Save(configFilePath);
		}

		private void addNodeTcpBinding()
		{
			// Get the configuration file path
			string configFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pos.exe.config");




			// Load the configuration file
			XmlDocument configXml = new XmlDocument();
			configXml.Load(configFilePath);

			// Get the netTcpBinding node
			XmlNode netTcpBindingNode = configXml.SelectSingleNode("//system.serviceModel/bindings/netTcpBinding");

			// Create a new binding element with the name attribute set to "NetTcpBinding_CPGetStockOnHandSvcPFMJKT12"
			XmlElement bindingElement = configXml.CreateElement("binding");
			bindingElement.SetAttribute("name", "NetTcpBinding_CPGetStockOnHandSvcPFMJKT12");

			// Append the binding element to the netTcpBinding node
			netTcpBindingNode.AppendChild(bindingElement);

			// Save the modified configuration file
			configXml.Save(configFilePath);
		}
		#endregion

	}
}
