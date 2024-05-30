using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceModel;
using System.Net.Http;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.ComponentModel.Composition;


namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
	public partial class CPStressTest : Form
	{
		//[Import]
        public IApplication Application;
		public CPStressTest(IApplication _application)
		{
			InitializeComponent();
            Application = _application;
			this.FormBorderStyle = FormBorderStyle.FixedSingle; // OR FormBorderStyle.FixedDialog OR FormBorderStyle.Fixed3D
			txtBox.ScrollBars = ScrollBars.Vertical;
			txtBox.WordWrap = false;
		}

		

		public string checkStockOnHand(string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "")
		{
			System.Diagnostics.Stopwatch timer = new Stopwatch();
			string itemId, siteId, wareHouse, maxQty, barCode, company = "";
			bool status = false;
			string message = "";


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

                timer.Start();
                ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getStockOnHand", parameterList);               
                
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                txtBox.AppendText("Time elapsed : " + timeTaken + "\r\n");
                
                //txtBox.AppendText("Service reference : " + url + "\r\n");
                txtBox.AppendText("Item ID : " + containerArray[3] + "\r\n");
                txtBox.AppendText("Barcode : " + containerArray[4] + "\r\n");
                txtBox.AppendText("Avail Phy QTY : " + containerArray[5] + "\r\n");
                txtBox.AppendText("Phy Invent Qty : " + containerArray[6] + "\r\n" + "\r\n");

                    //Console.ReadLine();
                
				//containerArray = Application.TransactionServices.InvokeExtension("getStockOnHandList", "SvcRefGetStockOnhandPFMPOC");
				/*returnValue = InternalApplication.TransactionServices.Invoke(
				 "GetCustomerBalance",
				 new object[] { customerId, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreCurrency, LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId }
				 );*/
				//status = (bool)containerArray[1];
				// message = containerArray[2].ToString();
				//tbName.Text = containerArray[4].ToString();
				//tbPoin.Text = containerArray[5].ToString();

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			
		    return message;
			/*
			switch (url)
			{
				case "SvcRefGetStockOnhandPFMPOC":
				   
					//HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUri);  

					var cc = new CPGetStockOnHandGroupPFMPOC.CallContext();// { Company = "6000", Language = "EN-US", LogonAsUser = @"MAHADASHA\business.system" };

					using (CPGetStockOnHandGroupPFMPOC.CP_GetStockOnHandClient client1 = new  CPGetStockOnHandGroupPFMPOC.CP_GetStockOnHandClient())
					{
						//callContext.Company
						//ServiceReference1.CallContext cc = new ServiceReference1.CallContext();
						//cc.Company = parmCompanyCode;
						cc.Company = parmCompanyCode;
						CP_GetStockOnHandParm GetStockOnHandParm_ = new CP_GetStockOnHandParm();
						GetStockOnHandParm_.parmItemId = parmItemId;
						GetStockOnHandParm_.parmSiteId = parmSiteId;
						GetStockOnHandParm_.parmWarehouse = parmWareHouse;
						GetStockOnHandParm_.parmMaxQtyCheck = "";
						GetStockOnHandParm_.parmBarcodeSetupId = "";
						// Process the result
						//Console.WriteLine(result);
						timer.Start();
						CP_GetStockOnHandResponse[] str = client1.getStockOnHandList(cc, GetStockOnHandParm_);
						timer.Stop();
						TimeSpan timeTaken = timer.Elapsed;
						txtBox.AppendText("Time elapsed : " + timeTaken + "\r\n");
						foreach (var a in str)
						{
							//txtBox.AppendText("Service reference : " + url + "\r\n");
							txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
							txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
							txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
							txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

							//Console.ReadLine();
						}
					}



					break;

				case "SvcRefGetStockOnhandPFM12":
					var cc2 = new CPGetStockOnHandGroupPFMJKT12.CallContext();
					using (CPGetStockOnHandSvcPFMJKT12Client client2 = new CPGetStockOnHandSvcPFMJKT12Client())
					//using (var client2 = new ServiceReference2.CPGetStockOnHandSvcPFMJKT12Client())
					{
						//ServiceReference2.CallContext cc2 = new ServiceReference2.CallContext();
						//cc2.Company = parmCompanyCode;
						cc2.Company = parmCompanyCode;

						CP_GetStockOnHandParmPFMJKT12 GetStockOnHandParm2_ = new CP_GetStockOnHandParmPFMJKT12();
						GetStockOnHandParm2_.parmItemId = parmItemId;
						GetStockOnHandParm2_.parmSiteId = parmSiteId;
						GetStockOnHandParm2_.parmWarehouse = parmWareHouse;
						GetStockOnHandParm2_.parmMaxQtyCheck = "";
						GetStockOnHandParm2_.parmBarcodeSetupId = "";


						timer.Start();
						CP_GetStockOnHandResponsePFMJKT12[] str2 = client2.getStockOnHandList(cc2, GetStockOnHandParm2_);
						timer.Stop();
						TimeSpan timeTaken2 = timer.Elapsed;
						txtBox.AppendText("Time elapsed : " + timeTaken2 + "\r\n");
						foreach (var a in str2)
						{

							txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
							txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
							txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
							txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

							//Console.ReadLine();
						}
					}

					break;*/


			
		}

        public void checkStockOnHandOld(string url, string parmCompanyCode = "", string parmSiteId = "", string parmWareHouse = "", string parmItemId = "", string parmMaxQty = "", string parmBarcodeSetupId = "", string parmConfigId = "")
        {
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            string itemId, siteId, wareHouse, maxQty, barCode, company = "";
            bool status = false;
            string message = "";


            
            switch (url)
            {
                case "SvcRefGetStockOnhandPFMPOC":
				   
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUri);  

                    //var cc = new CPGetStockOnHandGroupPFMPOC.CallContext();// { Company = "6000", Language = "EN-US", LogonAsUser = @"MAHADASHA\business.system" };

                    //using (CPGetStockOnHandGroupPFMPOC.CP_GetStockOnHandClient client1 = new  CPGetStockOnHandGroupPFMPOC.CP_GetStockOnHandClient())
                    //{
                    //    //callContext.Company
                    //    //ServiceReference1.CallContext cc = new ServiceReference1.CallContext();
                    //    //cc.Company = parmCompanyCode;
                    //    cc.Company = parmCompanyCode;
                    //    CP_GetStockOnHandParm GetStockOnHandParm_ = new CP_GetStockOnHandParm();
                    //    GetStockOnHandParm_.parmItemId = parmItemId;
                    //    GetStockOnHandParm_.parmSiteId = parmSiteId;
                    //    GetStockOnHandParm_.parmWarehouse = parmWareHouse;
                    //    GetStockOnHandParm_.parmMaxQtyCheck = "";
                    //    GetStockOnHandParm_.parmBarcodeSetupId = "";
                    //    // Process the result
                    //    //Console.WriteLine(result);
                    //    timer.Start();
                    //    CP_GetStockOnHandResponse[] str = client1.getStockOnHandList(cc, GetStockOnHandParm_);
                    //    timer.Stop();
                    //    TimeSpan timeTaken = timer.Elapsed;
                    //    txtBox.AppendText("Time elapsed : " + timeTaken + "\r\n");
                    //    foreach (var a in str)
                    //    {
                    //        //txtBox.AppendText("Service reference : " + url + "\r\n");
                    //        txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
                    //        txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
                    //        txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
                    //        txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

                    //        //Console.ReadLine();
                    //    }
                    //}



                    break;

                //case "SvcRefGetStockOnhandPFM12":
                //    var cc2 = new CPGetStockOnHandGroupPFMJKT12.CallContext();
                //    using (CPGetStockOnHandSvcPFMJKT12Client client2 = new CPGetStockOnHandSvcPFMJKT12Client())
                //    //using (var client2 = new ServiceReference2.CPGetStockOnHandSvcPFMJKT12Client())
                //    {
                //        //ServiceReference2.CallContext cc2 = new ServiceReference2.CallContext();
                //        //cc2.Company = parmCompanyCode;
                //        cc2.Company = parmCompanyCode;

                //        CP_GetStockOnHandParmPFMJKT12 GetStockOnHandParm2_ = new CP_GetStockOnHandParmPFMJKT12();
                //        GetStockOnHandParm2_.parmItemId = parmItemId;
                //        GetStockOnHandParm2_.parmSiteId = parmSiteId;
                //        GetStockOnHandParm2_.parmWarehouse = parmWareHouse;
                //        GetStockOnHandParm2_.parmMaxQtyCheck = "";
                //        GetStockOnHandParm2_.parmBarcodeSetupId = "";


                //        timer.Start();
                //        CP_GetStockOnHandResponsePFMJKT12[] str2 = client2.getStockOnHandList(cc2, GetStockOnHandParm2_);
                //        timer.Stop();
                //        TimeSpan timeTaken2 = timer.Elapsed;
                //        txtBox.AppendText("Time elapsed : " + timeTaken2 + "\r\n");
                //        foreach (var a in str2)
                //        {

                //            txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
                //            txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
                //            txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
                //            txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

                //            //Console.ReadLine();
                //        }
                //    }

                //    break;

                case "SvcRefGetStockOnhandPFM12":
                    //var cc2 = new CPGetStockOnHandEnhancedKUJKT12.CallContext(); //new CPGetStockOnHandGroupPFMJKT12.CallContext();
                    //using(var client2 = new CPGetStockOnHandEnhancedKUJKT12.CP_GetStockOnHandSvcKUJKT12Client())
                    ////using ( client2 = new cpget())
                    ////using (var client2 = new ServiceReference2.CPGetStockOnHandSvcPFMJKT12Client())
                    //{
                    //    //ServiceReference2.CallContext cc2 = new ServiceReference2.CallContext();
                    //    //cc2.Company = parmCompanyCode;
                    //    cc2.Company = parmCompanyCode;

                    //    CP_GetStockOnHandParmKUJKT12 GetStockOnHandParm2_ = new CP_GetStockOnHandParmKUJKT12();
                    //    GetStockOnHandParm2_.parmItemId = parmItemId;
                    //    GetStockOnHandParm2_.parmSiteId = parmSiteId;
                    //    GetStockOnHandParm2_.parmWarehouse = parmWareHouse;
                    //    GetStockOnHandParm2_.parmMaxQtyCheck = "";
                    //    GetStockOnHandParm2_.parmBarcodeSetupId = "";


                    //    timer.Start();
                    //    CP_GetStockOnHandResponseKUJKT12[] str2 = client2.getStockOnHandList(cc2, GetStockOnHandParm2_);
                    //    timer.Stop();
                    //    TimeSpan timeTaken2 = timer.Elapsed;
                    //    txtBox.AppendText("Time elapsed : " + timeTaken2 + "\r\n");
                    //    foreach (var a in str2)
                    //    {

                    //        txtBox.AppendText("Item ID : " + a.parmItemId + "\r\n");
                    //        txtBox.AppendText("Barcode : " + a.parmItemBarcode + "\r\n");
                    //        txtBox.AppendText("Avail Phy QTY : " + a.parmAvailPhyQty + "\r\n");
                    //        txtBox.AppendText("Phy Invent Qty : " + a.parmPhyInventQty + "\r\n" + "\r\n");

                    //        //Console.ReadLine();
                    //    }
                    //}

                    break;

            }

        }

		private void button1_Click(object sender, EventArgs e)
		{
			string[] whList = { "WH_JDELIMA", "WH_JCLDUK1", "WH_JCURUG1", "WH_JTJDRN1", "WH_JOFFICE" };
			string[] itemList = {"00000028","10000099","10001","10010002","10010008","10010009","10010010","10010011","10010015"
								,"10010016"
								,"10010041"
								,"10150017"
								,"11110001"
								,"11310014"
								,"11450150"
								,"11620150"
								,"11660150"
								,"18010000"
								,"18010001"
								,"18010002"
								,"18010003"
								,"18010004"
								,"18010005"
								,"19020013"
								,"30400110"
								,"30400111"
								,"31900020"
								,"41000001"
								,"42000001"
								};
			string itemId, siteId, wareHouse, maxQty, barCode, company = "";
			string returnValue = "";
			string servRef = comboBox1.SelectedItem.ToString();
			txtBox.Clear();
			// Create instances of the service references
		   

			int i = 0;
            for (int loop = 0; loop <= 10; loop++)
            {
                foreach (var a in whList)
                {
                    //string connectionString = @"Data Source = DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=JTJDRN1StoreDev; Integrated Security=True; ";
                    ////SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                    ////ServiceReference1.CP_GetStockOnHandResponse[] str = client1.getStockOnHandList(cc, GetStockOnHandParm_);
                    ////ServiceReference2.CPGetStockOnHandSvcPFMJKT12GetStockOnHandListResponse[] str2 = client2.getStockOnHandList(cc2, GetStockOnHandParm_)
                    //using (SqlConnection connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Open();

                    //    string query = @"SELECT ITEMID, DATAAREAID  FROM [ax].[CPITEMONHANDSTATUS]";

                    //    using (SqlCommand command = new SqlCommand(query, connection))
                    //    {
                    //        using (SqlDataReader reader = command.ExecuteReader())
                    //        {
                    txtBox.AppendText(servRef + " " + a.ToString() + "\r\n");
                    //while (reader.Read())
                    foreach (var x in itemList)
                    {
                        i++;
                        //txtBox.Text += "Test #" + i.ToString() + "\r\n";

                        txtBox.AppendText("Test #" + i.ToString() + "\r\n");
                        //txtBox.Text = txtBox.Text + "\n";
                        //Console.WriteLine("Test #" + i.ToString());
                        string itemid = x.ToString();//(string)reader["ITEMID"];
                        string dataAreaId = "5740"; //(string)reader["DATAAREAID"];
                        returnValue = checkStockOnHand(servRef, dataAreaId, "JKT", a.ToString(), itemid, "", "", "");

                    }
                    //}
                    //}
                    //}
                }
            }
                

			/*
			for (int i = 0; i <= 10; i++ )
			{
				Console.WriteLine(i.ToString() );
				returnValue = checkStockOnHand("ServiceReference2", "5740", "JKT", "WH_JDELIMA", "10000099", "", "", "");

			}*/
			//Console.ReadLine();
			//checkStockOnHand();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            string[] whList = { "WH_JDELIMA", "WH_JCLDUK1", "WH_JCURUG1", "WH_JTJDRN1", "WH_JOFFICE" };
            string[] itemList = {"00000028","10000099","10001","10010002","10010008","10010009","10010010","10010011","10010015"
                                ,"10010016"
                                ,"10010041"
                                ,"10150017"
                                ,"11110001"
                                ,"11310014"
                                ,"11450150"
                                ,"11620150"
                                ,"11660150"
                                ,"18010000"
                                ,"18010001"
                                ,"18010002"
                                ,"18010003"
                                ,"18010004"
                                ,"18010005"
                                ,"19020013"
                                ,"30400110"
                                ,"30400111"
                                ,"31900020"
                                ,"41000001"
                                ,"42000001"
                                };
            string itemId, siteId, wareHouse, maxQty, barCode, company = "";
            string returnValue = "";
            string servRef = comboBox1.SelectedItem.ToString();
            txtBox.Clear();

            int i = 0;
            for (int loop = 0; loop <= 10; loop++)
            {
                foreach (var a in whList)
                {
                    //string connectionString = @"Data Source = DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=JTJDRN1StoreDev; Integrated Security=True; ";
                    ////SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                    ////ServiceReference1.CP_GetStockOnHandResponse[] str = client1.getStockOnHandList(cc, GetStockOnHandParm_);
                    ////ServiceReference2.CPGetStockOnHandSvcPFMJKT12GetStockOnHandListResponse[] str2 = client2.getStockOnHandList(cc2, GetStockOnHandParm_)
                    //using (SqlConnection connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Open();

                    //    string query = @"SELECT ITEMID, DATAAREAID  FROM [ax].[CPITEMONHANDSTATUS]";

                    //    using (SqlCommand command = new SqlCommand(query, connection))
                    //    {
                    //        using (SqlDataReader reader = command.ExecuteReader())
                    //        {
                    txtBox.AppendText(servRef + " " + a.ToString() + "\r\n");
                    //while (reader.Read())
                    foreach (var x in itemList)
                    {
                        i++;
                        //txtBox.Text += "Test #" + i.ToString() + "\r\n";

                        txtBox.AppendText("Test #" + i.ToString() + "\r\n");
                        //txtBox.Text = txtBox.Text + "\n";
                        //Console.WriteLine("Test #" + i.ToString());
                        string itemid = x.ToString();//(string)reader["ITEMID"];
                        string dataAreaId = "5740"; //(string)reader["DATAAREAID"];
                        checkStockOnHandOld(servRef, dataAreaId, "JKT", a.ToString(), itemid, "", "", "");

                    }
                    //}
                    //}
                    //}
                }
            }

            

           
        }
	}
}
