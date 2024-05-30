using LSRetailPosis.POSControls.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Xml;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CP_DailyOnHand : frmTouchBase
    {

        DataTable dataTableList;
        IPosTransaction posTransaction;
        IApplication application;
        public CP_DailyOnHand(IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
            application = _application;
            dateTimeBox.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000; //1 seconds
            timer.Start();


            storeBox.Text = ApplicationSettings.Terminal.StoreId + " - " + ApplicationSettings.Terminal.StoreName; 
            this.BringToFront();
        }


   
        void timer_Tick(object sender, EventArgs e)
        {
            dateTimeBox.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
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
            public string RETAILVARIANTID { get; set; }
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

        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
        }
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

        public string getFolderPath(string ProcessingDirectory, string typeFolder)
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

        public string getWarehouseName()
        {
            string resultWarehouse = "";
            //SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            resultWarehouse = ApplicationSettings.Terminal.InventLocationId;
            //no need to query inventLocation because we can get it through ApplicationSettings.Terminal.InventLocationId --by Yonathan 
            /*var retailTransaction = posTransaction as RetailTransaction;
            try
            {

                string queryString = @" SELECT A.INVENTLOCATION, A.INVENTLOCATIONDATAAREAID, C.INVENTSITEID 
                            FROM ax.RETAILCHANNELTABLE A, ax.RETAILSTORETABLE B, ax.INVENTLOCATION C
                            WHERE A.RECID=B.RECID AND C.INVENTLOCATIONID=A.INVENTLOCATION AND B.STORENUMBER=@STOREID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            resultWarehouse = reader["INVENTLOCATION"].ToString();
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
            }*/

            return resultWarehouse;


        }


        public string getItemName(string parmItemId)
        {
            string nameItem = "";
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
           
            try
            {
                string queryString = @"SELECT  ECO.DISPLAYPRODUCTNUMBER,ECO.SEARCHNAME, ECOTRANS.NAME from ECORESPRODUCT ECO inner join ECORESPRODUCTTRANSLATION ECOTRANS on ECO.RECID = ECOTRANS.PRODUCT where ECO.DISPLAYPRODUCTNUMBER  =@ITEMID";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";
                
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@ITEMID", parmItemId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nameItem = reader["NAME"].ToString();

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
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }
            return nameItem;
        }

        private void CP_DailyOnHand_Load(object sender, EventArgs e)
        {
            //string connectionString = "Server=10.204.3.174;Database=api_dev;Uid=pfi;Password=freshmart01";
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            //MySqlConnection connection = new MySqlConnection(connectionString);
            string companyCode = application.Settings.Database.DataAreaID;

            string url = "";// "http://10.204.3.174:80/api/stockOnHand/getDailyOnHand";
            //url = getFolderPath(PathDirectory, "urlAPI") + "api/stockOnHand/getDailyOnHand";
            string functionName = "GetDOHAPI";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            url = APIClass.getURLAPIByFuncName(functionName);

            if (url == "")
            {
                this.Close();
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
                
            }
            
            string result = "";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", "PFM");
            httpRequest.Method = "POST";

            httpRequest.ContentType = "application/json";

            searchBox.Text = "Search ItemId or ProductName";
            searchBox.ForeColor = Color.Gray;

            var pack = new parmRequest()
            {
                DATAAREAID = companyCode,
                WAREHOUSE = getWarehouseName()
            };


            var data = MyJsonConverter.Serialize(pack); //MyJsonConverter.Serialize(pack);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();


            }

            parmResponse responseData = MyJsonConverter.Deserialize<parmResponse>(result); //MyJsonConverter.Deserialize<parmResponse>(result);


            List<responseData> responseDataMulti = MyJsonConverter.Deserialize<List<responseData>>(responseData.response_data);
            foreach (var lines in responseDataMulti)
            {
                lines.ITEMNAME = getItemName(lines.ITEMID);
            }
            var list = new BindingList<responseData>(responseDataMulti);


            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            dataTableList = converter.ToDataTable(responseDataMulti);
            dataGridResult.DataSource = dataTableList;
            this.changeGridStyles();

        }

        private void changeGridStyles()
        {
            dataGridResult.ReadOnly = true;
            this.dataGridResult.Columns["INVENTLOCATIONID"].Visible = false;
            this.dataGridResult.Columns["ITEMID"].HeaderText = "ItemId";
            this.dataGridResult.Columns["ITEMNAME"].HeaderText = "ProductName";
            this.dataGridResult.Columns["ITEMNAME"].Width = 200;
            this.dataGridResult.Columns["RETAILVARIANTID"].HeaderText = "VariantId";
            this.dataGridResult.Columns["QUANTITYPO"].HeaderText = "Qty PO";
            this.dataGridResult.Columns["QUANTITYTO"].HeaderText = "Qty TO";
            this.dataGridResult.Columns["QUANTITYSALES"].HeaderText = "Qty Sales";
            this.dataGridResult.Columns["TOTALDAILY"].HeaderText = "Total Daily";
            this.dataGridResult.Columns["STOCKCOUNTING"].HeaderText = "Stock Counted";
            this.dataGridResult.Columns["STOCKCOUNTINGADJUST"].HeaderText = "Stock Adjusted (+/-)";
            this.dataGridResult.DefaultCellStyle.Font = new Font("Tahoma", 12);
            this.dataGridResult.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridResult.Columns[1].Width = 90;
            dataGridResult.Columns[2].Width = 280;
            dataGridResult.Columns[3].Width = 90;
            dataGridResult.Columns[4].Width = 80;
            dataGridResult.Columns[5].Width = 80;
            dataGridResult.Columns[6].Width = 80;
            dataGridResult.Columns[7].Width = 80;
            dataGridResult.Columns[8].Width = 80;
            dataGridResult.Columns[9].Width = 85;

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            string searchValue = searchBox.Text;
            if (searchBox.Text.ToString() != "Search ItemId or ProductName")
            {
                try
                {
                    var re = from row in dataTableList.AsEnumerable()
                             where (row.Field<String>("ITEMID").Contains(searchValue.ToUpper())
                               || row.Field<String>("ITEMNAME").Contains(searchValue.ToUpper()))
                             select row;
                    if (re.Count() == 0)
                    {
                        //MessageBox.Show("No Results");
                        dt.Columns.Add("ItemId");
                        dt.Columns.Add("ProductName");
                        dt.Columns.Add("VariantId");
                        dt.Columns.Add("Qty PO");
                        dt.Columns.Add("Qty TO");
                        dt.Columns.Add("Qty Sales");
                        dt.Columns.Add("Total Daily");
                        dt.Columns.Add("Stock Counted");
                        dt.Columns.Add("Stock Adjusted (+/-)");

                        dataGridResult.DataSource = dt;

                        dataGridResult.Columns[0].Width = 90;
                        dataGridResult.Columns[1].Width = 315;
                        dataGridResult.Columns[2].Width = 80;
                        dataGridResult.Columns[3].Width = 70;
                        dataGridResult.Columns[4].Width = 70;
                        dataGridResult.Columns[5].Width = 70;
                        dataGridResult.Columns[6].Width = 70;
                        dataGridResult.Columns[7].Width = 80;
                        dataGridResult.Columns[8].Width = 85;

                    }
                    else
                    {
                        dataGridResult.DataSource = re.CopyToDataTable();
                        this.changeGridStyles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void searchBox_Enter(object sender, EventArgs e)
        {
            if (searchBox.Text == "Search ItemId or ProductName")
            {
                searchBox.Text = "";
                searchBox.ForeColor = Color.Black;
            }
        }

        private void searchBox_Leave(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchBox.Text = "Search ItemId or ProductName";
                searchBox.ForeColor = Color.Gray;
            }
        }
    }
}
