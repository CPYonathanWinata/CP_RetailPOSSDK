
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace APIAccess
{
    public class APIAccessClass
    {
        public static string userID;
        public static string resultAPI;
        public static List<API> result = new List<API>();
        public static List<string> resultURL = new List<string>();
        public static List<string> resultFuncName = new List<string>();
        public static List<string> resultIsSvcRef = new List<string>();
        public static List<string> resultSvcRefName = new List<string>();
        public static bool grabMartStatus = false;

        public static string custId { get; set; }
        public static string isB2b { get; set; }
        public static string priceGroup { get; set; }
        public static string lineDiscGroup { get; set; }

        public static List<string> itemToRemove { get; set; }
        public static List<string> itemToRemoveList = new List<string>();


        public static string grabCustName = "";
        public static string grabCustPhone = "";
        public static string grabOrderState = "";
        public static string grabOrderIdLong = "";
        //grabmart
        public static string merchantId { get; set; }

        /*private static string LikeToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("_", ".").Replace("%", ".*") + "$";
        }*/

        public string getURLAPIByFuncName(string _funcName)
        {
            string URL = "";
            int i = 0;
            string lowerCase = "";
            //check if there is wildcard
            
            string likeToRegular = "";
            if (_funcName.Contains("%"))
            {
                likeToRegular = "^" + Regex.Escape(_funcName).Replace("_", ".").Replace("%", ".*") + "$"; 
            }
            else
            {
                likeToRegular = _funcName;
                
            }
            foreach (var row in APIAccess.APIAccessClass.resultFuncName)
            {

               
                if (Regex.IsMatch(row.ToLower(), likeToRegular.ToLower()))

                
                //if (String.Equals(row, _funcName, StringComparison.OrdinalIgnoreCase))
                {
                    if (APIAccess.APIAccessClass.resultIsSvcRef[i].ToString() == "True")
                    {
                        URL = APIAccess.APIAccessClass.resultSvcRefName[i].ToString();
                    }
                    else
                    {
                        URL = APIAccess.APIAccessClass.resultURL[i].ToString();
                    }
                    
                    
                    //URL = row.ToString();
                    break;
                }
                i++;


            }

            if (URL == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", _funcName));
            }
            return URL;
        }

        public string getSvcRefByName(string _funcName)
        {
            string URL = "";
            int i = 0;
            string lowerCase = "";
            //check if there is wildcard

            string likeToRegular = "";
            if (_funcName.Contains("%"))
            {
                likeToRegular = "^" + Regex.Escape(_funcName).Replace("_", ".").Replace("%", ".*") + "$";
            }
            else
            {
                likeToRegular = _funcName;

            }
            foreach (var row in APIAccess.APIAccessClass.resultFuncName)
            {


                if (Regex.IsMatch(row.ToLower(), likeToRegular.ToLower()))

                //if (String.Equals(row, _funcName, StringComparison.OrdinalIgnoreCase))
                {
                    if (APIAccess.APIAccessClass.resultIsSvcRef[i].ToString() == "False")
                    {
                        URL = APIAccess.APIAccessClass.resultURL[i].ToString();
                    }
                    else
                    {
                        URL = this.getURLAPIByFuncName(_funcName);
                    }
                    URL = row.ToString();
                    break;
                }
                i++;


            }

            if (URL == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", _funcName));
            }
            return URL;
        }

    }
    

    

    public class parmRequest
    {
        public string DATAAREAID { get; set; }
        public string WAREHOUSE { get; set; }
    }
    public class responseData
    {
        public string INVENTLOCATIONID { get; set; }
        public string ITEMID { get; set; }
        public string ITEMNAME { get; set; }
        public string QUANTITYPO { get; set; }
        public string QUANTITYTO { get; set; }
        public string QUANTITYSALES { get; set; }
        public string TOTALDAILY { get; set; }
        public string STOCKCOUNTING { get; set; }
        public string STOCKCOUNTINGADJUST { get; set; }
    }

    public class parmResponse
    {
        public bool error { get; set; }
        public int message_code { get; set; }
        public string message_description { get; set; }
        public string response_data { get; set; }
    }

    public class API
    {
        public string URL {get;set;}
        public string FuncName {get;set;}
        public string IsServRef { get; set; }
        public string ServRefName { get; set; }

    }

    public class MySQLConnect
    {
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



        public void connectToDB(string _storeId, string _dataAreaId)
        {
            //string result = "";
            string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            string connectionString = getFolderPathConfig(PathDirectory, "connectionString");
            string passWord = getFolderPathConfig(PathDirectory, "passSQL");
            string hasil = "";
            string pass = "";
            string URL = "";
            string funcname = "";
            string dataAreaId = _dataAreaId;

            string storeId = _storeId;
            MySqlConnection connection;
            //SecureStringManager stringManager = new SecureStringManager();
            hasil = passWord;//stringManager.Unprotect(passWord);
            //pass = stringManager.Unprotect(hasil);
            //hasil = pass.Substring(4);
            connectionString += "pwd=" + hasil + ";";
            connectionString = "Server=10.204.3.174;Port=3306;Database=api_dev;CharSet=latin1;Allow User Variables=True;uid=pfi;pwd=freshmart01";
            connection = new MySqlConnection(connectionString);
            try
            {

                string queryString = @" SELECT DATAAREAID, STOREID, FUNCNAME, URL FROM api_dev.CPURLCONFIG WHERE STOREID = @STOREID && DATAAREAID =  @DATAAREAID";


                using (MySqlCommand command = new MySqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STOREID", storeId);
                    command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();

                    }
                    var result = new APIAccess.API();
                    //using (MySqlDataReader reader = command.ExecuteReader())
                    MySqlDataReader reader = command.ExecuteReader();
                    //{

                    if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.URL = reader["URL"].ToString();
                                result.FuncName = reader["FUNCNAME"].ToString();

                                APIAccess.APIAccessClass.result.Add(result);
                                //APIList.Add(result);

                                APIAccess.APIAccessClass.resultURL.Add(reader["URL"].ToString());
                                APIAccess.APIAccessClass.resultFuncName.Add(reader["FUNCNAME"].ToString());


                            }
                        }
                        
                        

                    //}
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
                    connection.Close();
                }
            }
            //APIAccessClass.resultData
            //return result;
            
        }

        #region custom class for en/decrypt pass
        public class TripleDes
        {
            readonly byte[] _key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            readonly byte[] _iv = { 8, 7, 6, 5, 4, 3, 2, 1 };

            // define the triple des provider
            private readonly System.Security.Cryptography.TripleDESCryptoServiceProvider _mDes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

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

    }
}
