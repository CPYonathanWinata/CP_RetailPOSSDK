using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using LSRetailPosis;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
 
using System.Data.SqlClient;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
 
using System.Diagnostics;
namespace APIAccess
{
	public  class APIFunction
	{
		
		public IApplication Application { get; set; }
		public static class MyJsonConverter
		{
			/// <summary>
			/// Deserialize an from json string
			/// </summary>
			public static T Deserialize<T>(string body)
			{
				using (var stream = new MemoryStream())
				using (var writer = new StreamWriter(stream))
				{
					writer.Write(body);
					writer.Flush();
					stream.Position = 0;
					return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
				}
			}

			/// <summary>
			/// Serialize an object to json
			/// </summary>
			public static string Serialize<T>(T item)
			{
				using (MemoryStream ms = new MemoryStream())
				{
					new DataContractJsonSerializer(typeof(T)).WriteObject(ms, item);
					return Encoding.Default.GetString(ms.ToArray());
				}
			}
		}

		public ReadOnlyCollection<object> checkStockOnHand(IApplication _application, string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "")
		{
			//System.Diagnostics.Stopwatch timer = new Stopwatch();
			string itemId, siteId, wareHouse, maxQty, barCode, company = "";
			bool status = false;
			string message = "";
			//object[] array = Array.Empty<object>();

			Application = _application;
			ReadOnlyCollection<object> containerArray = new ReadOnlyCollection<object>(new object[0]);

			try
			{
				object[] parameterList = new object[] 
							{
								url,
								parmSiteId,
								parmWareHouse,
								parmItemId,
								"",
								"",
								
							};


				containerArray = Application.TransactionServices.InvokeExtension("getStockOnHand", parameterList);



				return containerArray;
		  

			}
			catch (Exception ex)
			{
				return containerArray;

			}

		}
        //public ReadOnlyCollection<object> checkStockOnHandMulti(IApplication _application, string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "")
            public ReadOnlyCollection<object> checkStockOnHandMultiNew(IApplication _application, string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "",  string parmQtyInput = "", string parmTransId = "")
        {
            //System.Diagnostics.Stopwatch timer = new Stopwatch();
            string itemId, siteId, wareHouse, maxQty, barCode, company = "";
            bool status = false;
            string message = "";
            //object[] array = Array.Empty<object>();

            Application = _application;
            ReadOnlyCollection<object> containerArray = new ReadOnlyCollection<object>(new object[0]);

            try
            {
                
                object[] parameterList = new object[] 
							{
								url,
                                parmCompanyCode,
								parmSiteId,
								parmWareHouse,
								parmItemId,
								"",
								"",                                 
                                parmQtyInput,
                                parmTransId								
							};


                containerArray = Application.TransactionServices.InvokeExtension("getStockOnHandMultiNew", parameterList);



                return containerArray;


            }
            catch (Exception ex)
            {
                return containerArray;

            }

        }

            public ReadOnlyCollection<object> checkStockOnHandMulti(IApplication _application, string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "")
            //public ReadOnlyCollection<object> checkStockOnHandMulti(IApplication _application, string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "", string parmQtyInput = "", string parmTransId = "")
            {
                //System.Diagnostics.Stopwatch timer = new Stopwatch();
                string itemId, siteId, wareHouse, maxQty, barCode, company = "";
                bool status = false;
                string message = "";
                //object[] array = Array.Empty<object>();

                Application = _application;
                ReadOnlyCollection<object> containerArray = new ReadOnlyCollection<object>(new object[0]);

                try
                {

                    object[] parameterList = new object[] 
							{
								url,
                                parmCompanyCode,
								parmSiteId,
								parmWareHouse,
								parmItemId,
								"",
								""						
							};


                    containerArray = Application.TransactionServices.InvokeExtension("getStockOnHandMulti", parameterList);



                    return containerArray;


                }
                catch (Exception ex)
                {
                    return containerArray;

                }

            }
		//itemid,qty,unitid,dataareaid,warehouse,type,reffnum,retailvarid
		public string addItemMultiple(string _url, List<APIParameter.parmRequestAddItemMultiple> _listData)
		{
			string result= "";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(_url);
			httpRequest.Method = "POST";
			httpRequest.ContentType = "application/json";
			httpRequest.Headers.Add("Authorization", "PFM");
			List<string> multiPack = new List<string>();
			var packList = new List<parmRequest>();

             

			//var multiData = _listData; //MyJsonConverter.Serialize(packList);
			var multiData = MyJsonConverter.Serialize(_listData);		 
			using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
			{
				streamWriter.Write(multiData);
				streamWriter.Flush();
			}

			try
			{
				HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
						result = streamReader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
			return result;
		}

		public string checkStockSO(string url, string parmItemId, string parmDataAreaId, string parmWareHouse, string parmTransId, string parmQuantityAx, string parmQuantityInput, string parmOrigin, string configId)
		{


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			var httpRequest = (HttpWebRequest)WebRequest.Create(url);
			string result = "";
			httpRequest.Method = "POST";
			httpRequest.ContentType = "application/json";
			httpRequest.Headers.Add("Authorization", "PFM");

             

			var pack = new APIParameter.parmRequestStockSO()
			{
				DATAAREAID = parmDataAreaId,
				ITEMID = parmItemId,
				WAREHOUSE = parmWareHouse,
				TRANSACTIONID = parmTransId,
				QUANTITYAX = parmQuantityAx,
				QUANTITYINPUT = parmQuantityInput,
				ORIGIN = parmOrigin,
				RETAILVARIANTID = configId
			};


			var data = MyJsonConverter.Serialize(pack);
			using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
			{
				streamWriter.Write(data);
			}

			try
			{
				var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}


			return result;
		}

		#region QRIS ShopeePay API
        public static void generateQRShopeePay(string url, decimal parmInput, string parmStoreId, string parmTerminalId, string parmTransId, out APIAccess.APIParameter.parmResponseShopeePay responseShopeePay, out APIAccess.APIParameter.ListResultData listData )
		{
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			var httpRequest = (HttpWebRequest)WebRequest.Create(url);
			string result = "";
			httpRequest.Method = "POST";
			httpRequest.ContentType = "application/json";
			httpRequest.Headers.Add("Authorization", "PFM");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			var pack = new APIAccess.APIParameter.parmRequestShopeePay()
			{
				storeId = parmStoreId,
				amount = Convert.ToDecimal(parmInput),
				terminalId = parmTerminalId,
				transactionId = parmTransId
			};


			var data = MyJsonConverter.Serialize(pack);
			using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
			{
				streamWriter.Write(data);
			}

			try
			{
				var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{

				throw;
			}

			responseShopeePay = MyJsonConverter.Deserialize<APIAccess.APIParameter.parmResponseShopeePay>(result);

            if (responseShopeePay.error == false)
            {
                listData = MyJsonConverter.Deserialize<APIAccess.APIParameter.ListResultData>(responseShopeePay.data);

            }
            else
            {
                listData = null;
            }
			
			//return result;

		}

        public static void inquiryStatusPaymentShopeePay(string url, decimal parmInput, string parmStoreId, string parmTerminalId, string parmTransId, out APIAccess.APIParameter.parmResponseShopeePay responseShopeePay, out APIAccess.APIParameter.ListResultDataInquiry listData)
		{
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
			
			var httpRequest = (HttpWebRequest)WebRequest.Create(url);
			string result = "";
			httpRequest.Method = "POST";
			httpRequest.ContentType = "application/json";
			httpRequest.Headers.Add("Authorization", "PFM");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			var pack = new APIAccess.APIParameter.parmRequestShopeePay()
			{
				storeId = parmStoreId,
				amount = Convert.ToDecimal(parmInput),
				terminalId = parmTerminalId,
				transactionId = parmTransId
			};


			var data = MyJsonConverter.Serialize(pack);
			using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
			{
				streamWriter.Write(data);
			}

			try
			{
				var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{

				throw;
			}

			responseShopeePay = MyJsonConverter.Deserialize<APIAccess.APIParameter.parmResponseShopeePay>(result);

            if (responseShopeePay.data != "")
            {
                listData = MyJsonConverter.Deserialize<APIAccess.APIParameter.ListResultDataInquiry>(responseShopeePay.data);

            }
            else
            {
                listData = null;
            }

			//return result;
		}

        //get sales order for online order store
        public string getOnlineOrder(string _legalEntity, string _warehouse, string url)
        {


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            string result = "";
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", "PFM");



            var pack = new APIParameter.parmRequestOnlineOrder()
            {
                legal =  _legalEntity,
                warehouse = _warehouse
            };


            var data = MyJsonConverter.Serialize(pack);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }


            return result;
        }

        //to convert string to decimal despite difference between regional format settings -> ex "1,000.00" or "1.000,00"
        public static decimal ParseDecimal(string input)
        {

            var regex = new Regex("^[^\\d-]*(-?(?:\\d|(?<=\\d)\\.(?=\\d{3}))+(?:,\\d+)?|-?(?:\\d|(?<=\\d),(?=\\d{3}))+(?:\\.\\d+)?)[^\\d]*$");

            char decimalChar;
            char thousandsChar;

            // Get the numeric part from the string
            var numberPart = regex.Match(input).Groups[1].Value;

            // Try to guess which character is used for decimals and which is used for thousands
            if (numberPart.LastIndexOf(',') > numberPart.LastIndexOf('.'))
            {
                decimalChar = ',';
                thousandsChar = '.';
            }
            else
            {
                decimalChar = '.';
                thousandsChar = ',';
            }

            // Remove thousands separators as they are not needed for parsing
            numberPart = numberPart.Replace(thousandsChar.ToString(), string.Empty);

            // Replace decimal separator with the one from InvariantCulture
            // This makes sure the decimal parses successfully using InvariantCulture
            numberPart = numberPart.Replace(decimalChar.ToString(),
                CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator);

            // Voilá
            var result = decimal.Parse(numberPart, NumberStyles.AllowDecimalPoint | NumberStyles.Number, CultureInfo.InvariantCulture);

            return result;
        }

        public static void invalidateQRShopeePay(string url, string parmStoreId, string parmTerminalId, string parmTransId, out APIAccess.APIParameter.parmResponseShopeePay responseShopeePay )
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            string result = "";
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", "PFM");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            var pack = new APIAccess.APIParameter.parmRequestShopeePay()
            {
                storeId = parmStoreId,
                
                terminalId = parmTerminalId,
                transactionId = parmTransId
            };


            var data = MyJsonConverter.Serialize(pack);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            responseShopeePay = MyJsonConverter.Deserialize<APIAccess.APIParameter.parmResponseShopeePay>(result);

             

            //return result;

        }
		#endregion


        public static string getFolderPathConfig(string ProcessingDirectory, string typeFolder)
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

        public static void getMysqlConn(IApplication _application) 
        {
            //string result = "";
            string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pos.exe.config");
            //string storeId = getFolderPathConfig(PathDirectory, "StoreId");
            //string dataAreaId = _application.Settings.Database.DataAreaID.ToString();


            string PathDirectoryAPI = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            string connectionString = getFolderPathConfig(PathDirectoryAPI, "connectionString");
            string passWord = getFolderPathConfig(PathDirectoryAPI, "passSQL");
            string hasil = "";
            string pass = "";
            string secret = "prima2022";


            MySql.Data.MySqlClient.MySqlConnection connection;
            //SecureStringManager stringManager = new SecureStringManager();
            //hasil = stringManager.Unprotect(passWord);
            //pass = stringManager.Unprotect(hasil);
            //hasil = pass.Substring(4);

            hasil = passWord;
            APIAccess.MySQLConnect.TripleDes tripleDesClass = new APIAccess.MySQLConnect.TripleDes();
            //hasil = tripleDesClass.Encrypt(passWord);

            hasil = tripleDesClass.Decrypt(passWord);


            hasil = hasil.Replace(secret, "").Replace("  ", " ");

            connectionString += "pwd=" + hasil + ";";
            //connectionString = "Server=10.204.3.174;Port=3306;Database=api_dev;CharSet=latin1;Allow User Variables=True;uid=pfi;pwd=freshmart01";
            //connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            APIAccess.APIParameter.mySqlConnString = new MySql.Data.MySqlClient.MySqlConnection(connectionString); ;
        }

        //find price for customer order 
        public  List<APIAccess.APIParameter.SaleLineItemData> findPriceAgreementCustom(
        IApplication _application,
        SqlConnection _connectionString,
        long _channelId,
        List<APIAccess.APIParameter.SaleLineItemData> _items,
        string _accountRelations,
        string _unitId,
        decimal _qty = 0)
        {
            List<APIAccess.APIParameter.SaleLineItemData> stringList = new List<APIAccess.APIParameter.SaleLineItemData>();
            SqlConnection connectionStore = _connectionString;

            try
            {
                string queryString = @"SELECT TOP 1 ITEMRELATION, ACCOUNTRELATION, AMOUNT, QUANTITYAMOUNTFROM, QUANTITYAMOUNTTO, FROMDATE, TODATE, UNITID
                               FROM PRICEDISCTABLE TA
                               INNER JOIN [ax].RETAILCHANNELTABLE AS c
                               ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
                               LEFT JOIN [ax].INVENTDIM invdim 
                               ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
                               WHERE ITEMRELATION = @ItemRelation
                               AND TA.ACCOUNTRELATION = @AccountRelations
                               AND TA.RELATION = 4
                               AND TA.UNITID = @UnitId
                               AND (
                                (1 BETWEEN QUANTITYAMOUNTFROM AND QUANTITYAMOUNTTO) OR
                                (QUANTITYAMOUNTFROM = 0 AND QUANTITYAMOUNTTO = 0)
                                )
                               AND (@ActiveDate BETWEEN TA.FROMDATE AND TA.TODATE)";

                if (connectionStore.State != ConnectionState.Open)
                {
                    connectionStore.Open();
                }

                foreach (var item in _items)
                {
                    using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                    {
                        // Bind parameters
                        command.Parameters.AddWithValue("@CHANNELID", _channelId);
                        command.Parameters.AddWithValue("@Quantity", _qty.ToString());
                        command.Parameters.AddWithValue("@ItemRelation", item.ItemId); // Dynamically bind ItemId
                        command.Parameters.AddWithValue("@AccountRelations", _accountRelations);
                        command.Parameters.AddWithValue("@UnitId", item.UnitId);
                        command.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                APIAccess.APIParameter.SaleLineItemData newItem = new APIAccess.APIParameter.SaleLineItemData
                                {
                                    ItemId = reader["ITEMRELATION"].ToString(),
                                    UnitId = reader["UNITID"].ToString(),
                                    Price = Convert.ToDecimal(reader["AMOUNT"].ToString()),
                                    LineId = item.LineId
                                };

                                stringList.Add(newItem);
                                // Add the price (AMOUNT) to the result list
                                //stringList.Add(reader["ITEMRELATION"].ToString() + ";"+reader["AMOUNT"].ToString());
                                
                            }
                            else
                            {
                                // Add a default value if no price is found

                                decimal price = _application.Services.Price.GetItemPrice(item.ItemId, item.UnitId);
                                //
                                APIAccess.APIParameter.SaleLineItemData newItem = new APIAccess.APIParameter.SaleLineItemData
                                {
                                    ItemId = item.ItemId,
                                    UnitId = item.UnitId,
                                    Price = price,
                                    LineId = item.LineId
                                };

                                stringList.Add(newItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connectionStore.State != ConnectionState.Closed)
                {
                    connectionStore.Close();
                }
            }

            return stringList;
        }

        //get B2B Parameter
        public void getB2BParameter(string _custId)
        {
            //APIAccess.APIAccessClass.custId = transaction.Customer.CustomerId.ToString();
            //APIAccess.APIAccessClass.isB2b = containerArray[6].ToString();
            //APIAccess.APIAccessClass.priceGroup = containerArray[4].ToString();
            //APIAccess.APIAccessClass.lineDiscGroup = containerArray[5].ToString();

            SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string queryString = @"SELECT CT.ACCOUNTNUM,CT.CUSTGROUP, CG.CPPPNVALIDATE, CG.CPPPNDIBEBASKAN, CT.PRICEGROUP,CT.LINEDISC, B2BSETUP.CP_CUSTCLASSIFICATION,B2BSETUP.AUTOMATICRESERVE
                                      FROM  [ax].[CUSTTABLE] CT
                                      LEFT JOIN AX.CP_CPCUSTORDERB2BSETUP B2BSETUP
                                      ON CT.ACCOUNTNUM = B2BSETUP.ACCOUNTNUM
                                      LEFT JOIN AX.CUSTGROUP CG
                                      ON CT.CUSTGROUP = CG.CUSTGROUP 
                                      WHERE CT.ACCOUNTNUM = @CUSTACCOUNT";


                //RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

                using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                {

                    command.Parameters.AddWithValue("@CUSTACCOUNT", _custId);


                    if (connectionStore.State != ConnectionState.Open)
                    {
                        connectionStore.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            APIAccess.APIAccessClass.isB2b = reader["CP_CUSTCLASSIFICATION"].ToString();
                            APIAccess.APIAccessClass.priceGroup = reader["PRICEGROUP"].ToString();
                            APIAccess.APIAccessClass.lineDiscGroup = reader["LINEDISC"].ToString();
                            APIAccess.APIAccessClass.ppnValidation = reader["CPPPNVALIDATE"].ToString();
                            APIAccess.APIAccessClass.custId = _custId;
                            //stringList.Add(reader["AMOUNT"].ToString());
                            //stringList.Add(reader["ACCOUNTRELATION"].ToString());


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

        public string getCustomerClass(string _custId)
        {
            //APIAccess.APIAccessClass.custId = transaction.Customer.CustomerId.ToString();
            //APIAccess.APIAccessClass.isB2b = containerArray[6].ToString();
            //APIAccess.APIAccessClass.priceGroup = containerArray[4].ToString();
            //APIAccess.APIAccessClass.lineDiscGroup = containerArray[5].ToString();

            string customerClass = "";
            SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string queryString = @"SELECT CT.ACCOUNTNUM,CT.CUSTGROUP, CG.CPPPNVALIDATE, CG.CPPPNDIBEBASKAN, CT.PRICEGROUP,CT.LINEDISC, B2BSETUP.CP_CUSTCLASSIFICATION,B2BSETUP.AUTOMATICRESERVE
                                      FROM  [ax].[CUSTTABLE] CT
                                      LEFT JOIN AX.CP_CPCUSTORDERB2BSETUP B2BSETUP
                                      ON CT.ACCOUNTNUM = B2BSETUP.ACCOUNTNUM
                                      LEFT JOIN AX.CUSTGROUP CG
                                      ON CT.CUSTGROUP = CG.CUSTGROUP 
                                      WHERE CT.ACCOUNTNUM = @CUSTACCOUNT";


                //RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

                using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                {

                    command.Parameters.AddWithValue("@CUSTACCOUNT", _custId);


                    if (connectionStore.State != ConnectionState.Open)
                    {
                        connectionStore.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerClass  = reader["CP_CUSTCLASSIFICATION"].ToString();
                             
                            //stringList.Add(reader["AMOUNT"].ToString());
                            //stringList.Add(reader["ACCOUNTRELATION"].ToString());


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
            return customerClass;

        }



        //public async Task<bool> CheckApiAvailability(string url)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.Timeout = TimeSpan.FromSeconds(5);
        //            HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        //            return response.IsSuccessStatusCode;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public   bool CheckForInternetConnection(IApplication _application, string _url)
		{
            string checkConn = "api/connection/check";

           
            Uri uri = new Uri(_url);
            string siteName = uri.GetLeftPart(UriPartial.Authority);

            _url = siteName + "/" + checkConn;

            //check RTS
            try
            {
                _application.TransactionServices.CheckConnection();
                
            }
            catch 
            {
                //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Tidak bisa terhubung dengan jaringan.\nCek koneksi RTS", MessageBoxButtons.OK, MessageBoxIcon.Error))
                //{
                //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                //}
                return false;
            }

            //check API
			try
			{
				using (var client = new WebClient())
                using (client.OpenRead(_url))//"https://pfm.cp.co.id/api/connection/check"))
					return true;
			}
			catch
			{
                //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Tidak bisa terhubung dengan jaringan.\nCek koneksi API", MessageBoxButtons.OK, MessageBoxIcon.Error))
                //{
                //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                //}
				return false;
			}


		}


        public void clearRetailTransExtended()
        {
            APIAccess.APIAccessClass.idTypeBox = "";
            APIAccess.APIAccessClass.idText = "";
            APIAccess.APIAccessClass.custText = "";
            APIAccess.APIAccessClass.genderBox = "";
            APIAccess.APIAccessClass.ageText = 0;
            APIAccess.APIAccessClass.nationality = "";
            APIAccess.APIAccessClass.nationalityIndex = -1;
            APIAccess.APIAccessClass.isB2b = "";
        }
        public void LogErrorToEventViewer(string errorDetails)
        {
            string source = "Microsoft Dynamics AX Retail : Retail POS";
            string logName = "Application";

            try
            {
                // Check if source exists; if not, create it
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }

                // Write the error details to Event Viewer
                using (EventLog eventLog = new EventLog(logName))
                {
                    eventLog.Source = source;
                    eventLog.WriteEntry(errorDetails, EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions when writing to the Event Viewer (e.g., insufficient permissions)
                Console.WriteLine("Failed to write to Event Viewer: " + ex.Message);
            }
        }


        public class DatabaseHelper
        {
            private string _connectionString;

            public DatabaseHelper(string connectionString)
            {
                _connectionString = connectionString;
            }
            // Method to check if a table exists
            public bool CheckExistTable(string tableName)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Split schema and table name if needed
                        string[] tableParts = tableName.Split('.');
                        string schema = tableParts.Length > 1 ? tableParts[0] : "dbo"; // Default to "dbo" if no schema provided
                        string table = tableParts.Length > 1 ? tableParts[1] : tableParts[0];

                        // Query to check if the table exists
                        string checkTableQuery = @"
                        SELECT COUNT(1) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName";

                        using (SqlCommand command = new SqlCommand(checkTableQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Schema", schema);
                            command.Parameters.AddWithValue("@TableName", table);

                            int result = (int)command.ExecuteScalar();
                            return result > 0;
                        }
                    }
                    catch
                    {
                        return false; // Return false in case of error
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }

            // Method to check if a field exists in a table
            public bool CheckExistField(string tableName, string fieldName)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Split schema and table name if needed
                        string[] tableParts = tableName.Split('.');
                        string schema = tableParts.Length > 1 ? tableParts[0] : "dbo"; // Default to "dbo" if no schema provided
                        string table = tableParts.Length > 1 ? tableParts[1] : tableParts[0];

                        // Query to check if the field exists
                        string checkFieldQuery = @"
                        SELECT COUNT(1)
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName";

                        using (SqlCommand command = new SqlCommand(checkFieldQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Schema", schema);
                            command.Parameters.AddWithValue("@TableName", table);
                            command.Parameters.AddWithValue("@ColumnName", fieldName);

                            int result = (int)command.ExecuteScalar();
                            return result > 0; // Returns true if the field exists
                        }
                    }
                    catch
                    {
                        return false; // Return false in case of error
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }
//            public bool CheckExistTable(string tableName)
//            {
//                using (SqlConnection connection = new SqlConnection(_connectionString))
//                {
//                    try
//                    {
//                        connection.Open();

//                        // Split schema and table name if needed
//                        string[] tableParts = tableName.Split('.');
//                        string schema = tableParts.Length > 1 ? tableParts[0] : "dbo"; // Default to "dbo" if no schema provided
//                        string table = tableParts.Length > 1 ? tableParts[1] : tableParts[0];

//                        // Query to check if the table exists
//                        string checkTableQuery = @"
//                            SELECT COUNT(1) 
//                            FROM INFORMATION_SCHEMA.TABLES 
//                            WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName";

//                        using (SqlCommand command = new SqlCommand(checkTableQuery, connection))
//                        {
//                            command.Parameters.AddWithValue("@Schema", schema);
//                            command.Parameters.AddWithValue("@TableName", table);

//                            int result = (int)command.ExecuteScalar();
//                            return result > 0;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        //Console.WriteLine($"Error checking table existence: {ex.Message}");
//                        return false; // You might handle this differently based on requirements
//                    }
//                    finally
//                    {
//                        if (connection.State != ConnectionState.Closed)
//                        {
//                            connection.Close();
//                        }
//                    }
//                }
//            }

        }

      


        public class GrabMartAPI
        {
            public static APIAccess.APIParameter.ApiResponseGrabmart getOrderList(string _storeId, string _url)
            {
                //int rowIndex;
                //string amountCashout = "";
                string storeId = _storeId; // ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
                //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
                //var url = "https://devpfm.cp.co.id/api/grab/listOrder"
                string url = "";
                APIAccess.APIParameter.Receiver receiverParm;
                string functionName = "GetGRABMARTAPI";
                APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
                url = _url; //APIClass.getURLAPIByFuncName(functionName);
                APIAccess.APIParameter.ApiResponseGrabmart responseData;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                string result = "";
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");

                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new APIAccess.APIParameter.parmOrderListGrabmart()
                {
                    warehouse = storeId,
                    dateFrom = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),//"2023-11-10 00:00:00", //DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),                    
                    dateTo = DateTime.Now.ToString("yyyy-MM-dd 23:59:59") //"2023-11-10 23:59:59"
                };


                try
                {
                    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        responseData = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(result); //

                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //return "0";
                    throw;

                }

                
                return responseData;
            }

            public static APIAccess.APIParameter.ApiResponseGrabmart requestCancelOrder(string _url, string _merchantId, string _orderIdLong)
            {
                string result = "";
                APIAccess.APIParameter.ApiResponseGrabmart responseData;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                var httpRequest = (HttpWebRequest)WebRequest.Create(_url);

                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");
                 
                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new APIAccess.APIParameter.parmGetCurrentOrderStateGrabmart()
                {
                    merchantID = _merchantId,
                    orderID = _orderIdLong

                };

                try
                {
                    var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        responseData = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(result); // 
                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    //return "0";
                    throw;

                }
                return responseData;
            }
            
        }
	}
}
