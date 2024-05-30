using LSRetailPosis.POSControls.Touch;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.Discount;
using LSRetailPosis.Transaction.Line.InfocodeItem;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using CRTDM = Microsoft.Dynamics.Commerce.Runtime.DataModel;
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
using Microsoft.Dynamics.Commerce.Runtime.Services.Messages;
using Microsoft.Dynamics.Commerce.Runtime;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using Microsoft.Dynamics.Commerce.Runtime.Data;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
	
    //NOTES : Discount Item ED "23".
    //NOTES : Discount Item ED PER RECEIPT "25"
	public partial class CPDiscountItem :  LSRetailPosis.POSControls.Touch.frmTouchBase
	{
		IPosTransaction posTransaction;
		IApplication application;
		string promoId;
		string promoIdString;
        string operationId;
		private Contracts.Services.KeyInPrices keyPrices;
		private Commerce.Runtime.DataModel.SalesLine crtSalesLine;
        string promoCode;
        string commentCode;
        string lineCommentCode;
        bool isDev;
		//private string connectionString = "YourConnectionString";
		//SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
		public CPDiscountItem(IPosTransaction _posTransaction, IApplication _application, string _promoId, string _operationId)
		{
			InitializeComponent();
			posTransaction = _posTransaction;
			application = _application;
			promoId = _promoId;
            operationId = _operationId;
            isDev = true;

            if(isDev == true)
            {
                if (operationId == "23")
                {
                    LoadDataHeader();
                    LoadData();
                    promoCode = "ED";  //"PDI"; //ED
                    commentCode = "PROMOED";  //"PROMOPDI";
                    lineCommentCode = "AddItem-ED";  //"AddItem-PDI";
                    lblHeader.Text = "Promo Discount Item";
                }
                else if(operationId == "25")
                {
                    LoadDataHeaderReceipt();
                    LoadDataLineReceipt();
                    promoCode = "QS";  //"PDIS";
                    commentCode = "PROMORCPT"; // "PROMOPDIS";
                    lineCommentCode = "AddItem-QS";// "AddItem-PDIS"; 
                    lblHeader.Text = "Promo Discount Item per Struk";
                }
            }
            else
            {
                if (operationId == "23")
                {
                    LoadDataHeader();
                    LoadData();
                    promoCode = "PDI";  //"PDI"; //ED
                    commentCode = "PROMOPDI";  //"PROMOPDI";
                    lineCommentCode = "AddItem-PDI";  //"AddItem-PDI";
                    lblHeader.Text = "Promo Discount Item";
                }
                else if(operationId == "25")
                {
                    LoadDataHeaderReceipt();
                    LoadDataLineReceipt();
                    promoCode = "PDIS";  //"PDIS";
                    commentCode = "PROMOPDIS"; // "PROMOPDIS";
                    lineCommentCode = "AddItem-PDIS";// "AddItem-PDIS"; 
                    lblHeader.Text = "Promo Discount Item per Struk";
                }
            }
            


            //if (operationId == "23")
            //{
            //    LoadDataHeader();
            //    LoadData();
            //    promoCode = "ED";  //"PDI"; //ED
            //    commentCode = "PROMOED";  //"PROMOPDI";
            //    lineCommentCode = "AddItem-ED";  //"AddItem-PDI";
            //    lblHeader.Text = "Promo Discount Item";
            //}
            //else if(operationId == "25")
            //{
            //    LoadDataHeaderReceipt();
            //    LoadDataLineReceipt();
            //    promoCode = "QS";  //"PDIS";
            //    commentCode = "PROMORCPT"; // "PROMOPDIS";
            //    lineCommentCode = "AddItem-QS";// "AddItem-PDIS"; 
            //    lblHeader.Text = "Promo Discount Item per Struk";
            //}

            
			this.dataGridResult.CellContentClick += dataGridResult_CellContentClick;
		}
        private void Label_SizeChanged(object sender, EventArgs e)
        {
            CenterLabel();
        }

        private void CenterLabel()
        {
            int x = (this.ClientSize.Width - lblHeader.Width) / 2;
            //int y = (this.ClientSize.Height - lblHeader.Height) / 2;
            lblHeader.Location = new System.Drawing.Point(x, lblHeader.Location.Y);
        }
        private void LoadDataLineReceipt()
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            //totalAmountTextBox.Text = "TOTAL BELANJA : " + retailTransaction.NetAmountWithTax.ToString("N2");
            string additionalQuery = "";
            string additionalQuery2 = "";
            string promoIdQuery = "";
            string itemToExclude = "";
            string maxReceiptQuery = "";
            List<string> excludeList = new List<string>();

            //dataGridResult.Rows.Clear();
            //dataGridResult.Columns.Clear();

            //loop through the lines to get all item id
            foreach (var items in retailTransaction.CalculableSalesLines)
            {
                if (items.PeriodicDiscountLines.Count == 0)//&& items.PeriodicDiscountLines)
                {
                    excludeList.Add(items.ItemId);
                }
                else
                {
                    foreach (var discountLines in items.PeriodicDiscountLines)
                    {
                        PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;
                        if (!periodDiscItem.OfferId.StartsWith("QS"))  //if (!periodDiscItem.OfferId.StartsWith("PDIS"))
                        {
                            excludeList.Add(items.ItemId);
                            //itemToExclude += "'" + items.ItemId + "'";

                            //// Add comma only if there are more items following
                            //if (discountLines != items.PeriodicDiscountLines.Last())
                            //{
                            //    itemToExclude += ",";
                            //}
                        }
                    }
                }
                //foreach (var discountLines in items.PeriodicDiscountLines)
                //{
                //    PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;
                //    if (!periodDiscItem.OfferId.StartsWith("ED"))
                //    {

                //        itemToExclude += "'" + items.ItemId + "'";

                //        // Add comma only if there are more items following
                //        if (discountLines != items.PeriodicDiscountLines.Last())
                //        {
                //            itemToExclude += ",";
                //        }
                //    }
                //}
            }
            if (excludeList.Count != 0)
            {
                itemToExclude = "'" + string.Join("','", excludeList) + "'";
            }


            try
            {



                //                string queryString = @"SELECT CUSTACCOUNT as [Customer] 
                //                              
                //                                        , [DESCRIPTION] as [Promo Name]
                //                                        , [FROMDATE] as [From Date]
                //                                        , [TODATE] as [To Date]
                //                                        , [MINPAYMENTAMOUNT] as [Min. Payment]
                //                                        , [ITEMID] as [ItemId]
                //                                        , [PRODUCTNAME] as [Item Name]
                //                                        , [DISCAMOUNT] as [Disc. Amount]
                //                                        , [DISCPERCENTAGE] as [Disc. Percentage] 
                //                                        , [MAXQTY] as [Max Qty]
                //                                        , CPPROMOEDQTYDETAIL.PROMOID AS [Promo ID]    
                //                                        FROM AX.CPPROMOEDQTY JOIN AX.CPPROMOEDQTYDETAIL ON CPPROMOEDQTYDETAIL.PROMOID = CPPROMOEDQTY.PROMOID
                //                                        WHERE ISACTIVE = 1   
                //                                        ORDER BY MINPAYMENTAMOUNT ASC
                //                                    ";
                //if (promoId != "")
                //{
                //    additionalQuery = "AND CPPROMOEDQTY.PROMOID = '" + promoId + "'";
                //}
                //if (itemToExclude != "")
                //{
                //    additionalQuery2 = "AND CPPROMOEDQTYDETAIL.ITEMID NOT IN (" + itemToExclude + ")";
                //}
                //if (promoIdString != "")
                //{
                //    promoIdQuery = "AND AX.CPPROMOEDQTYDETAIL.PROMOID = '" + promoIdString + "'";
                //}

                if (promoId != "")
                {
                    additionalQuery = "AND CPPROMOEDQTYPERSTRUK.PROMOID = '" + promoId + "'";
                }
                if (itemToExclude != "")
                {
                    additionalQuery2 = "AND CPPROMOEDQTYPERSTRUKDETAIL.ITEMID NOT IN (" + itemToExclude + ")";
                }
                if (promoIdString != "")
                {
                    promoIdQuery = "AND AX.CPPROMOEDQTYPERSTRUKDETAIL.PROMOID = '" + promoIdString + "'";
                }

                string queryString = @"SELECT 											 
	                                        [DESCRIPTION] as [Promo Name], 
	                                        [FROMDATE] as [From Date], 
	                                        [TODATE] as [To Date], 
	                                        [MINPAYMENTAMOUNT] as [Min. Payment], 
	                                        [ITEMID] as [ItemId], 
	                                        [PRODUCTNAME] as [Item Name], 
	                                        [DISCAMOUNT] as [Disc. Amount], 
	                                        [DISCPERCENTAGE] as [Disc. Percentage], 
	                                        [MAXQTY] as [Max Qty], 
	                                        CPPROMOEDQTYPERSTRUKDETAIL.PROMOID AS [Promo ID],
	                                        CPPROMOEDQTYPERSTRUKDETAIL.MAXQTYPERRECEIPT AS [Max Qty per Rcpt],
	                                        (

                                                SELECT  ISNULL(ABS(SUM(S.QTY)),0) QTY 
		                                        FROM RETAILTRANSACTIONSALESTRANS S
		 
			 
		                                        WHERE S.RECEIPTID != ''
		 
		                                        AND S.COMMENT = CPPROMOEDQTYPERSTRUKDETAIL.PROMOID
		                                        AND S.STORE = @STOREID
		                                        AND S.ITEMID = CPPROMOEDQTYPERSTRUKDETAIL.ITEMID
		                                        AND S.TRANSDATE BETWEEN CPPROMOEDQTYPERSTRUK.FROMDATE AND CPPROMOEDQTYPERSTRUK.TODATE
		                                        AND S.TRANSACTIONSTATUS != 1

		                                       
	                                        ) AS TERJUAL
                                        FROM  
	                                        AX.CPPROMOEDQTYPERSTRUK 
                                        JOIN 
	                                        AX.CPPROMOEDQTYPERSTRUKDETAIL ON CPPROMOEDQTYPERSTRUKDETAIL.PROMOID = CPPROMOEDQTYPERSTRUK.PROMOID
                                        WHERE 
											
	                                        CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date) AND AX.CPPROMOEDQTYPERSTRUKDETAIL.RETAILSTOREID  = @STOREID "
                                        + promoIdQuery + ""
                                        + additionalQuery + ""
                                        + additionalQuery2 +
                                        "ORDER BY MINPAYMENTAMOUNT ASC";
                 //SELECT ISNULL(ABS(SUM(S.QTY)),0) 
                 //                               FROM RETAILTRANSACTIONSALESTRANS S
                 //                               JOIN RETAILTRANSACTIONDISCOUNTTRANS D ON
                 //                                   S.DATAAREAID = D.DATAAREAID AND
                 //                                   S.STORE = D.STOREID AND
                 //                                   S.TERMINALID = D.TERMINALID AND
                 //                                   S.TRANSACTIONID = D.TRANSACTIONID AND
                 //                                   S.LINENUM = D.SALELINENUM AND
                 //                                   S.CHANNEL = D.CHANNEL
                 //                               WHERE S.RECEIPTID != ''
                 //                               AND D.PERIODICDISCOUNTOFFERID = CPPROMOEDQTYPERSTRUKDETAIL.PROMOID
                 //                               AND S.STORE = @STOREID
                 //                               AND S.ITEMID = CPPROMOEDQTYPERSTRUKDETAIL.ITEMID
                 //                               AND S.TRANSDATE BETWEEN CPPROMOEDQTYPERSTRUK.FROMDATE AND CPPROMOEDQTYPERSTRUK.TODATE
                string queryStringOld = @"SELECT 
											 
											[DESCRIPTION] as [Promo Name], 
											[FROMDATE] as [From Date], 
											[TODATE] as [To Date], 
											[MINPAYMENTAMOUNT] as [Min. Payment], 
											[ITEMID] as [ItemId], 
											[PRODUCTNAME] as [Item Name], 
											[DISCAMOUNT] as [Disc. Amount], 
											[DISCPERCENTAGE] as [Disc. Percentage], 
											[MAXQTY] as [Max Qty], 
											CPPROMOEDQTYDETAIL.PROMOID AS [Promo ID],
											(
												SELECT ISNULL(ABS(SUM(S.QTY)),0) 
												FROM RETAILTRANSACTIONSALESTRANS S
												JOIN RETAILTRANSACTIONDISCOUNTTRANS D ON
													S.DATAAREAID = D.DATAAREAID AND
													S.STORE = D.STOREID AND
													S.TERMINALID = D.TERMINALID AND
													S.TRANSACTIONID = D.TRANSACTIONID AND
													S.LINENUM = D.SALELINENUM AND
													S.CHANNEL = D.CHANNEL
												WHERE S.RECEIPTID != ''
												AND D.PERIODICDISCOUNTOFFERID = CPPROMOEDQTYDETAIL.PROMOID
												AND S.STORE = @STOREID
												AND S.ITEMID = CPPROMOEDQTYDETAIL.ITEMID
												AND S.TRANSDATE BETWEEN CPPROMOEDQTY.FROMDATE AND CPPROMOEDQTY.TODATE
											) AS TERJUAL
										FROM  
											AX.CPPROMOEDQTY 
										JOIN 
											AX.CPPROMOEDQTYDETAIL ON CPPROMOEDQTYDETAIL.PROMOID = CPPROMOEDQTY.PROMOID
										WHERE 
											
											CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date) AND AX.CPPROMOEDQTYDETAIL.RETAILSTOREID  = @STOREID "
                                        + promoIdQuery + ""
                                        + additionalQuery + ""
                                        + additionalQuery2 +
                                        "ORDER BY MINPAYMENTAMOUNT ASC";
                /*AX.CPPROMOEDQTYDETAIL.PROMOID = @PROMOID
                                              AND CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= SYSDATETIME() AND TODATE >= SYSDATETIME() AND AX.CPPROMOEDQTYDETAIL.RETAILSTOREID  = @STOREID "*/
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    //command.Parameters.AddWithValue("@PROMOID", promoIdString);

                    command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
                    command.Parameters.AddWithValue("@CUSTACC", retailTransaction.Customer.CustomerId);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the DataGridView
                    dataGridResult.DataSource = dataTable;


                    bool buttonColumnExists = false;
                    foreach (DataGridViewColumn col in dataGridResult.Columns)
                    {
                        if (col.Name == "btnSelect")
                        {
                            buttonColumnExists = true;
                            break;
                        }
                    }

                    if (!buttonColumnExists)
                    {
                        DataGridViewButtonColumn btnSelectCol = new DataGridViewButtonColumn();
                        btnSelectCol.HeaderText = "";
                        btnSelectCol.Name = "btnSelect";
                        btnSelectCol.ReadOnly = true;
                        btnSelectCol.Text = "Pilih";
                        btnSelectCol.UseColumnTextForButtonValue = true;
                        dataGridResult.Columns.Add(btnSelectCol);
                    }
                    //// Add a ButtonField to the GridView
                    //DataGridViewButtonColumn btnSelectCol = new DataGridViewButtonColumn();
                    //btnSelectCol.HeaderText = "";
                    //btnSelectCol.Name = "btnSelect";
                    //btnSelectCol.ReadOnly = true;
                    //btnSelectCol.Text = "Pilih";
                    //btnSelectCol.UseColumnTextForButtonValue = true;
                    //dataGridResult.Columns.Add(btnSelectCol);
                    // Bind the GridView data after adding the column

                    gridStyle();


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
        }

        private void LoadDataHeaderReceipt()
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            totalAmountTextBox.Text = "TOTAL BELANJA : " + retailTransaction.NetAmountWithTax.ToString("N2");
            string additionalQuery = "";
            if (promoId != "")
            {
                additionalQuery = "AND HEADER.PROMOID = '" + promoId + "'";
            }

            string queryStringHeader = @" SELECT 
	                                            HEADER.PROMOID AS [Promo ID],                                          
	                                            [DESCRIPTION] as [Promo Name], 
	                                            [FROMDATE] as [From Date], 
	                                            [TODATE] as [To Date], 
	                                            [MINPAYMENTAMOUNT] as [Min. Payment] ,
                                                DETAIL.RETAILSTOREID AS [Store]

                                            FROM  
	                                            AX.CPPROMOEDQTYPERSTRUK HEADER
	                                            LEFT JOIN  (
	                                            SELECT 
		                                            *,
		                                            ROW_NUMBER() OVER (PARTITION BY PromoID ORDER BY PROMOID) AS RowNum
	                                            FROM 
		                                            AX.CPPROMOEDQTYPERSTRUKDETAIL) DETAIL
	                                            ON HEADER.PROMOID = DETAIL.PROMOID AND DETAIL.RowNum=1
											WHERE 
												CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date) 
                                                AND DETAIL.RETAILSTOREID = @STOREID "
                                            + additionalQuery + " " +
                                            "ORDER BY MINPAYMENTAMOUNT ASC";
            //AND RETAILSTOREID =@STOREID 
            using (SqlCommand command = new SqlCommand(queryStringHeader, connection))
            {

                command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
                command.Parameters.AddWithValue("@CUSTACC", retailTransaction.Customer.CustomerId);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();

                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Bind the DataTable to the DataGridView
                dataGridHeader.DataSource = dataTable;
                dataGridHeader.CellClick += dataGridHeader_CellClick;

                // Bind the GridView data after adding the column


            }
        }

		private void LoadDataHeader()
		{
			RetailTransaction retailTransaction = posTransaction as RetailTransaction;
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			totalAmountTextBox.Text = "TOTAL BELANJA : " + retailTransaction.NetAmountWithTax.ToString("N2");
            string additionalQuery = "";
            if (promoId != "")
            {
                additionalQuery = "AND HEADER.PROMOID = '" + promoId + "'";
            }

			string queryStringHeader = @"SELECT 
												HEADER.PROMOID AS [Promo ID],                                          
												[DESCRIPTION] as [Promo Name], 
												[FROMDATE] as [From Date], 
												[TODATE] as [To Date], 
												[MINPAYMENTAMOUNT] as [Min. Payment] ,
                                                DETAIL.RETAILSTOREID AS [Store]

											FROM  
												AX.CPPROMOEDQTY HEADER
												LEFT JOIN  (
												SELECT 
													*,
													ROW_NUMBER() OVER (PARTITION BY PromoID ORDER BY PROMOID) AS RowNum
												FROM 
													AX.CPPROMOEDQTYDETAIL) DETAIL
												ON HEADER.PROMOID = DETAIL.PROMOID AND DETAIL.RowNum=1
											WHERE 
												CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date) 
                                                AND DETAIL.RETAILSTOREID = @STOREID "
                                            + additionalQuery  + " " +
											"ORDER BY MINPAYMENTAMOUNT ASC";
            //AND RETAILSTOREID =@STOREID 
			using (SqlCommand command = new SqlCommand(queryStringHeader, connection))
			{

				command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
				command.Parameters.AddWithValue("@CUSTACC", retailTransaction.Customer.CustomerId);
				if (connection.State != ConnectionState.Open)
				{
					connection.Open();

				}

				SqlDataAdapter adapter = new SqlDataAdapter(command);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);

				// Bind the DataTable to the DataGridView
				dataGridHeader.DataSource = dataTable;
				dataGridHeader.CellClick += dataGridHeader_CellClick;

				// Bind the GridView data after adding the column


			}
		}

		private void dataGridHeader_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Retrieve the selected PromoId from the clicked row
			int rowIndex = e.RowIndex;
			if (rowIndex >= 0 && rowIndex < dataGridHeader.Rows.Count)
			{
				DataGridViewRow selectedRow = dataGridHeader.Rows[rowIndex];
			    promoIdString= selectedRow.Cells["Promo ID"].Value.ToString();

				// Populate the detail DataGridView with data based on the selected PromoId
                PopulateDetailDataGridView(promoIdString);
			}
		}

		private void PopulateDetailDataGridView(string selectedPromoId)
		{
            // Remove all columns
            while (dataGridResult.Columns.Count > 0)
            {
                dataGridResult.Columns.RemoveAt(0); // Remove the first column repeatedly until there are no more columns left
            }

            if (operationId == "23")
            {
                LoadData();
            }
            else if (operationId == "25")
            {
                LoadDataLineReceipt();
            }
		}

		//public class InfoCodeLineItemV1 : IInfoCodeLineItemV1
		//{
		//    public int AdditionalCheck { get; set; }
		//    public decimal Amount { get; set; }
		//    public string InfocodeId { get; set; }
		//    public string Information { get; set; }
		//    public bool InputRequired { get; set; }
		//    // ... other properties and methods from the interface
		//}

		// Usage

		private void LoadData()
		{
			RetailTransaction retailTransaction = posTransaction as RetailTransaction;
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			//totalAmountTextBox.Text = "TOTAL BELANJA : " + retailTransaction.NetAmountWithTax.ToString("N2");
			string additionalQuery="";
			string additionalQuery2 = "";
            string promoIdQuery="";
			string itemToExclude = "";
            string maxReceiptQuery = "";
			List<string> excludeList = new List<string>();

			//dataGridResult.Rows.Clear();
			//dataGridResult.Columns.Clear();

			//loop through the lines to get all item id
			foreach(var items in retailTransaction.CalculableSalesLines) 
			{
				if (items.PeriodicDiscountLines.Count == 0)//&& items.PeriodicDiscountLines)
				{
					excludeList.Add(items.ItemId);
				}
				else
				{
					foreach (var discountLines in items.PeriodicDiscountLines)
					{
						PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;
                        if (!periodDiscItem.OfferId.StartsWith("ED")) //if (!periodDiscItem.OfferId.StartsWith("PDI"))
						{
							excludeList.Add(items.ItemId);
							//itemToExclude += "'" + items.ItemId + "'";

							//// Add comma only if there are more items following
							//if (discountLines != items.PeriodicDiscountLines.Last())
							//{
							//    itemToExclude += ",";
							//}
						}
					}
				}
				//foreach (var discountLines in items.PeriodicDiscountLines)
				//{
				//    PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;
				//    if (!periodDiscItem.OfferId.StartsWith("ED"))
				//    {

				//        itemToExclude += "'" + items.ItemId + "'";

				//        // Add comma only if there are more items following
				//        if (discountLines != items.PeriodicDiscountLines.Last())
				//        {
				//            itemToExclude += ",";
				//        }
				//    }
				//}
			}
			if(excludeList.Count!=0)
			{
				itemToExclude = "'" + string.Join("','", excludeList) + "'";
			}
				

			try
			{

					
			
//                string queryString = @"SELECT CUSTACCOUNT as [Customer] 
//                              
//                                        , [DESCRIPTION] as [Promo Name]
//                                        , [FROMDATE] as [From Date]
//                                        , [TODATE] as [To Date]
//                                        , [MINPAYMENTAMOUNT] as [Min. Payment]
//                                        , [ITEMID] as [ItemId]
//                                        , [PRODUCTNAME] as [Item Name]
//                                        , [DISCAMOUNT] as [Disc. Amount]
//                                        , [DISCPERCENTAGE] as [Disc. Percentage] 
//                                        , [MAXQTY] as [Max Qty]
//                                        , CPPROMOEDQTYDETAIL.PROMOID AS [Promo ID]    
//                                        FROM AX.CPPROMOEDQTY JOIN AX.CPPROMOEDQTYDETAIL ON CPPROMOEDQTYDETAIL.PROMOID = CPPROMOEDQTY.PROMOID
//                                        WHERE ISACTIVE = 1   
//                                        ORDER BY MINPAYMENTAMOUNT ASC
//                                    ";
				if (promoId != "")
				{
					additionalQuery = "AND CPPROMOEDQTY.PROMOID = '"+promoId+"'";
				}
				if (itemToExclude != "")
				{
					additionalQuery2 = "AND CPPROMOEDQTYDETAIL.ITEMID NOT IN (" + itemToExclude +")";
				}
                if(promoIdString != "")
                {
                    promoIdQuery = "AND AX.CPPROMOEDQTYDETAIL.PROMOID = '" + promoIdString + "'";
                }
				 
				string queryString = @"SELECT 
											 
											[DESCRIPTION] as [Promo Name], 
											[FROMDATE] as [From Date], 
											[TODATE] as [To Date], 
											[MINPAYMENTAMOUNT] as [Min. Payment], 
											[ITEMID] as [ItemId], 
											[PRODUCTNAME] as [Item Name], 
											[DISCAMOUNT] as [Disc. Amount], 
											[DISCPERCENTAGE] as [Disc. Percentage], 
											[MAXQTY] as [Max Qty], 
											CPPROMOEDQTYDETAIL.PROMOID AS [Promo ID],
											(
												SELECT  ISNULL(ABS(SUM(S.QTY)),0) QTY 
		                                        FROM RETAILTRANSACTIONSALESTRANS S
		 
			 
		                                        WHERE S.RECEIPTID != ''
		 
		                                        AND S.COMMENT = CPPROMOEDQTYDETAIL.PROMOID
		                                        AND S.STORE = @STOREID
		                                        AND S.ITEMID = CPPROMOEDQTYDETAIL.ITEMID
		                                        AND S.TRANSDATE BETWEEN CPPROMOEDQTY.FROMDATE AND CPPROMOEDQTY.TODATE
		                                        AND S.TRANSACTIONSTATUS != 1
											) AS TERJUAL
										FROM  
											AX.CPPROMOEDQTY 
										JOIN 
											AX.CPPROMOEDQTYDETAIL ON CPPROMOEDQTYDETAIL.PROMOID = CPPROMOEDQTY.PROMOID
										WHERE 
											
											CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date) AND AX.CPPROMOEDQTYDETAIL.RETAILSTOREID  = @STOREID "
                                        + promoIdQuery + ""    
										+ additionalQuery  + ""
										+ additionalQuery2 +
										"ORDER BY MINPAYMENTAMOUNT ASC";
                /*AX.CPPROMOEDQTYDETAIL.PROMOID = @PROMOID
                                              AND CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= SYSDATETIME() AND TODATE >= SYSDATETIME() AND AX.CPPROMOEDQTYDETAIL.RETAILSTOREID  = @STOREID "*/
				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					//command.Parameters.AddWithValue("@PROMOID", promoIdString);
					
					command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
					command.Parameters.AddWithValue("@CUSTACC", retailTransaction.Customer.CustomerId);
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();

					}

					SqlDataAdapter adapter = new SqlDataAdapter(command);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);

					// Bind the DataTable to the DataGridView
					dataGridResult.DataSource = dataTable;


                    bool buttonColumnExists = false;
                    foreach (DataGridViewColumn col in dataGridResult.Columns)
                    {
                        if (col.Name == "btnSelect")
                        {
                            buttonColumnExists = true;
                            break;
                        }
                    }

                    if (!buttonColumnExists)
                    {
                        DataGridViewButtonColumn btnSelectCol = new DataGridViewButtonColumn();
                        btnSelectCol.HeaderText = "";
                        btnSelectCol.Name = "btnSelect";
                        btnSelectCol.ReadOnly = true;
                        btnSelectCol.Text = "Pilih";
                        btnSelectCol.UseColumnTextForButtonValue = true;
                        dataGridResult.Columns.Add(btnSelectCol);
                    }
                    //// Add a ButtonField to the GridView
                    //DataGridViewButtonColumn btnSelectCol = new DataGridViewButtonColumn();
                    //btnSelectCol.HeaderText = "";
                    //btnSelectCol.Name = "btnSelect";
                    //btnSelectCol.ReadOnly = true;
                    //btnSelectCol.Text = "Pilih";
                    //btnSelectCol.UseColumnTextForButtonValue = true;
                    //dataGridResult.Columns.Add(btnSelectCol);
					// Bind the GridView data after adding the column

					gridStyle();


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
		}

	   

		private void dataGridResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			decimal maxDisc = 0;
			decimal pctDisc = 0;
			decimal amtDisc = 0;
			decimal minPaymAmt = 0;
			decimal amountPct = 0;
			string message = "";
			string itemId = "";
			decimal qtySelected = 0;
			decimal qtyMax = 0;
            decimal qtyMaxRcpt = 0;
			decimal qtySold = 0;
			decimal qtyCurrent=0;
			string promoId = "";
			string promoName = "";
            decimal qtyOnCart = 0;
			RetailTransaction transaction = posTransaction as RetailTransaction;
			//maxDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Max Disc"].Value);
			promoId = dataGridResult.Rows[e.RowIndex].Cells["Promo ID"].Value.ToString();
			promoName = dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString();
			pctDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Disc. Percentage"].Value);
			amtDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Disc. Amount"].Value); 
			minPaymAmt = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Min. Payment"].Value);
			itemId = dataGridResult.Rows[e.RowIndex].Cells["ItemId"].Value.ToString();
			qtyMax = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Max Qty"].Value);
            int rowIndex = e.RowIndex;
            if (dataGridResult.Columns.Contains("Max Qty per Rcpt") && dataGridResult.Rows[rowIndex].Cells["Max Qty per Rcpt"].Value != null)
            {
                qtyMaxRcpt = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Max Qty per Rcpt"].Value);
            }
			qtySold = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["TERJUAL"].Value);

			if (e.ColumnIndex == dataGridResult.Columns["btnSelect"].Index && e.RowIndex >= 0)
			{               

				if (transaction.NetAmountWithTax >= minPaymAmt)
				{
					for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
					{
						//string thisItemId = "";
						LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = transaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);
						int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;

						if (currentLine.ItemId == itemId && currentLine.Voided == false)
						{
							qtyCurrent = currentLine.QuantityOrdered;
						}
					}
				

					//if (itemId != "")
					//{

						//show dialog for selecting Qty
                    qtySelected = ShowQuantitySelectionForm(qtyCurrent);
                     
					if (qtySelected == 0)
					{
						return;
					} 
					else
					{
                        //check if qty selected is more than qty max per receipt
                        //if (qtySelected + qtyCurrent > qtyMaxRcpt && operationId == "25")
                        if (qtySelected > qtyMaxRcpt && operationId == "25")
                        {
                            message = "Jumlah barang melebihi quantity maksimum\nper struk pada promo ini. Jumlah barang per struk = " + qtyMaxRcpt.ToString("N2");
                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                return;
                            }
                        }
                        else
                        {


                            //check  if qty added no more than qty sold
                            if (qtySelected > (qtyMax - qtySold) ) //if (qtySelected > (qtyMax - qtySold) - qtyCurrent) // 8 - 5
                            {
                                message = "Jumlah barang melebihi quantity maksimum pada promo ini.\nSisa barang yang mendapat promo ini = " + (qtyMax - qtySold - qtyCurrent).ToString("N2");
                                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                {
                                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                    return;
                                }
                            }

                            else
                            {
                                //check if the cart has already same item that selects. if yes then add the qty, if not then treat is as add new line
                                var matchingLines = transaction.SaleItems.Where(line => line.ItemId == itemId).ToList(); 

                                if (matchingLines.Count != 0)
                                {
                                    for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
                                    {
                                        LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = transaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);
                                        int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;
                                        if (currentLine.ItemId == itemId)
                                        {
                                            currentLine.QuantityOrdered =  qtySelected; //currentLine.QuantityOrdered + qtySelected;
                                            currentLine.Quantity = currentLine.QuantityOrdered;// +qtySelected;
                                        }

                                    }
                                    calculateAllLinesPromo(transaction, promoId, promoName);
                                    /*promoCode = "QS";
                    commentCode = "PROMORCPT";
                    lineCommentCode = "AddItem-QS";*/
                                    //transaction.Comment = "PROMOED";
                                    transaction.Comment = commentCode;
                                    transaction.CalcTotals();
                                    POSFormsManager.ShowPOSStatusPanelText("Added item discount " + dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString());
                                    closeBtn.PerformClick();

                                    application.RunOperation(PosisOperations.DisplayTotal, "");
                                }
                                else
                                {

                                    //addItem(transaction);

                                    //check whether current line has 

                                    ItemSale iSale = new ItemSale();
                                    iSale.OperationID = PosisOperations.ItemSale;
                                    iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                                    iSale.Barcode = itemId;


                                    iSale.IsInfocodeItem = true;
                                    iSale.OperationInfo.NumpadQuantity = qtySelected;
                                    iSale.POSTransaction = (PosTransaction)posTransaction;
                                    //iSale.POSTransaction.Comment = "AddItem-ED";
                                    iSale.POSTransaction.Comment = lineCommentCode;
                                    iSale.RunOperation();

                                    //PosApplication.Instance.RunOperation(PosisOperations.PayCustomerAccount, "", (PosTransaction)transaction);

                                    //LSRetailPosis.Transaction.Line.Discount.LineDiscountItem lineDisc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                                    //DiscountItem discItem = (DiscountItem)new PeriodicDiscountItem();
                                    //if (pctDisc == 0)
                                    //{
                                    //    discItem.Amount = amtDisc;
                                    //}
                                    //else if (amtDisc == 0)
                                    //{
                                    //    discItem.Percentage = pctDisc;
                                    //}


                                    //

                                    /*if (transaction.SaleItems.Last.Value.CustomerDiscount != 0 || transaction.SaleItems.Last.Value.LineDiscount != 0 || transaction.SaleItems.Last.Value.DiscountLines.Count !=0)//period discount !=ED
                                    */
                                    // Assuming transaction is your LinkedList<Transaction>



                                    //var discountLinesWithNonEmptyOfferId = transaction.SaleItems.Last.Value.di

                                    //    .Where(discountLine => !string.IsNullOrEmpty(discountLine.OfferId))
                                    //    .ToList(); // Materialize the query into a list to avoid modifying it while iterating

                                    if (transaction.SaleItems.Last.Value.CustomerDiscount != 0 || transaction.SaleItems.Last.Value.LineDiscount != 0 || transaction.SaleItems.Last.Value.DiscountLines.Count != 0)
                                    {

                                        var lastSaleItem = transaction.SaleItems.Last.Value;

                                        //if it has a discount that has an offer ID
                                        //foreach (var discountLine in lastSaleItem.DiscountLines.ToList())
                                        //{
                                        //    if (discountLine is PeriodicDiscountItem)
                                        //    {
                                        //        // Convert discountLine to PeriodicDiscountItem
                                        //        PeriodicDiscountItem periodicLines = (PeriodicDiscountItem)discountLine;

                                        //        // Check if the OfferId is not null or empty
                                        //        if (!string.IsNullOrEmpty(periodicLines.OfferId))
                                        //        {
                                        //            // Remove the discountLine from the DiscountLines list
                                        //            lastSaleItem.DiscountLines.Remove(discountLine);
                                        //        }
                                        //    }
                                        //}

                                        foreach (var discountLine in lastSaleItem.DiscountLines.ToList())
                                        {
                                            lastSaleItem.DiscountLines.Remove(discountLine);
                                        }

                                        transaction.CurrentSaleLineItem.Comment = dataGridResult.Rows[e.RowIndex].Cells["Promo ID"].Value.ToString();
                                        calculateAllLinesPromo(transaction, promoId, promoName);
                                        transaction.Comment = commentCode; //"PROMOED";
                                        transaction.CalcTotals();
                                        POSFormsManager.ShowPOSStatusPanelText("Added item discount " + dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString());
                                        closeBtn.PerformClick();

                                        application.RunOperation(PosisOperations.DisplayTotal, "");
                                        //transaction.SaleItems.RemoveLast();

                                        //message = "Tidak bisa memilih barang ini dikarenakan terdapat promo regular yang sedang berlaku.";
                                        //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                        //{
                                        //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                        //    return;
                                        //}
                                        //foreach (var discountLine in discountLinesWithNonEmptyOfferId)
                                        //{

                                        //    lastSaleItem.DiscountLines.Remove(discountLine);
                                        //}

                                        //if (lastSaleItem.DiscountLines.Count == 0)
                                        //{
                                        //    transaction.CurrentSaleLineItem.Comment = dataGridResult.Rows[e.RowIndex].Cells["Promo ID"].Value.ToString();
                                        //    calculateAllLinesPromo(transaction, promoId, promoName);
                                        //    transaction.Comment = "PROMOED";
                                        //    transaction.CalcTotals();
                                        //    POSFormsManager.ShowPOSStatusPanelText("Added item discount " + dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString());
                                        //    closeBtn.PerformClick();

                                        //    application.RunOperation(PosisOperations.DisplayTotal, "");
                                        //}



                                    }
                                    else
                                    {
                                        transaction.CurrentSaleLineItem.Comment = dataGridResult.Rows[e.RowIndex].Cells["Promo ID"].Value.ToString();
                                        calculateAllLinesPromo(transaction, promoId, promoName);
                                        transaction.Comment = commentCode;// "PROMOED";

                                        transaction.CalcTotals();
                                        POSFormsManager.ShowPOSStatusPanelText("Added item discount " + dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString());
                                        closeBtn.PerformClick();

                                        application.RunOperation(PosisOperations.DisplayTotal, "");
                                    }
                                }

                            }
                        }
					}

					
						
				}
				else
				{
					message = "Total minimal belanja belum memenuhi syarat untuk menggunakan promo ini.\nTambah lagi " + (minPaymAmt - transaction.NetAmountWithTax).ToString("N2");
					using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
					{
						LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
						return;
					}
				}
			}
			

			 
		}

		private void calculateAllLinesPromo(RetailTransaction _transaction, string _promoId, string _promoName)
		{
			var matchingLines = _transaction.SaleItems.Where(line => line.Comment == _promoId).ToList();
			decimal pctDisc = 0;
			decimal amtDisc = 0;
			DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue; 
			foreach (var line in matchingLines)
			{
				line.IsInfoCodeItem = false;
				LSRetailPosis.Transaction.Line.Discount.LineDiscountItem lineDisc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                if (operationId == "23")
                {
                    selectPromoItem(line.ItemId, _promoId, out pctDisc, out amtDisc, out fromDate, out toDate);
                }
                else if (operationId == "25")
                {
                    selectPromoItemReceipt(line.ItemId, _promoId, out pctDisc, out amtDisc, out fromDate, out toDate);
                }
				
                //check if this line already has a discount with the same promo ID

                foreach (var discountLine in line.DiscountLines.ToList())
                {
                    line.DiscountLines.Remove(discountLine);
                }
                               
                var existingDiscount = line.DiscountLines
                                        .OfType<PeriodicDiscountItem>()
                                        .FirstOrDefault(d => d.OfferId == _promoId);

                if (existingDiscount == null)
                {
                    DiscountItem discItem = new PeriodicDiscountItem();
                    if (pctDisc == 0)
                    {
                        discItem.Amount = amtDisc;
                    }
                    else if (amtDisc == 0)
                    {
                        discItem.Percentage = pctDisc;
                    }

                    PeriodicDiscountItem periodDiscItem = discItem as PeriodicDiscountItem;
                    periodDiscItem.OfferId = _promoId;
                    periodDiscItem.OfferName = _promoName;
                    periodDiscItem.QuantityDiscounted = 1;
                    periodDiscItem.BeginDateTime = fromDate;
                    periodDiscItem.EndDateTime = toDate;

                    line.DiscountLines.AddFirst(discItem);
                }


                //DiscountItem discItem = (DiscountItem)new PeriodicDiscountItem();
                //if (pctDisc == 0)
                //{
                //    discItem.Amount = amtDisc;
                //}
                //else if (amtDisc == 0)
                //{
                //    discItem.Percentage = pctDisc;
                //}

				
                //PeriodicDiscountItem periodDiscItem = discItem as PeriodicDiscountItem;
                //periodDiscItem.OfferId = _promoId;
                //periodDiscItem.OfferName = _promoName;
                //periodDiscItem.QuantityDiscounted = 1;
                //periodDiscItem.BeginDateTime = fromDate; //Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["From Date"].Value.ToString());
                //periodDiscItem.EndDateTime = toDate; //Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["To Date"].Value.ToString());


                //line.DiscountLines.AddFirst(discItem);
			}

			//PeriodicDiscountItem periodDiscItem = discItem as PeriodicDiscountItem;
			//periodDiscItem.OfferId = dataGridResult.Rows[e.RowIndex].Cells["Promo ID"].Value.ToString();
			//periodDiscItem.OfferName = dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString();
			//periodDiscItem.QuantityDiscounted = 1;
			//periodDiscItem.BeginDateTime = Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["From Date"].Value.ToString());
			//periodDiscItem.EndDateTime = Convert.ToDateTime(dataGridResult.Rows[e.RowIndex].Cells["To Date"].Value.ToString());

			//transaction.CurrentSaleLineItem.DiscountLines.AddFirst(discItem);
		}

		private void selectPromoItem(string itemId, string promoId, out decimal pctDisc, out decimal amtDisc, out DateTime fromDate, out DateTime toDate)
		{
			amtDisc = 0;
			pctDisc = 0;
			fromDate = DateTime.Now;
			toDate = DateTime.Now;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
			    string queryString = "";
			    queryString = @"SELECT LINES.[PROMOID]       
							      ,[ITEMID]     
							      ,[DISCAMOUNT]
							      ,[DISCPERCENTAGE]      
							      ,[RETAILSTOREID]
							      ,[FROMDATE]
							      ,[TODATE]
						      FROM [ax].[CPPROMOEDQTYDETAIL] LINES
						      LEFT JOIN [AX].[CPPROMOEDQTY] HEADER
						      ON LINES.PROMOID =  HEADER.PROMOID
						      WHERE RETAILSTOREID = @STOREID
						      AND LINES.PROMOID = @PROMOID
						      AND LINES.ITEMID = @ITEMID";

			    //SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			    using (SqlCommand command = new SqlCommand(queryString, connection))
			    {

				    command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
				    command.Parameters.AddWithValue("@PROMOID", promoId);
				    command.Parameters.AddWithValue("@ITEMID", itemId);
				    if (connection.State != ConnectionState.Open)
				    {
					    connection.Open();

					    using (SqlDataReader reader = command.ExecuteReader())
					    {
						    if (reader.Read())
						    {

							    pctDisc = Convert.ToDecimal(reader["DISCPERCENTAGE"]);
							    amtDisc = Convert.ToDecimal(reader["DISCAMOUNT"]);
							    fromDate = Convert.ToDateTime(reader["FROMDATE"].ToString());
							    toDate = Convert.ToDateTime(reader["TODATE"].ToString());
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
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }
		}
        private void selectPromoItemReceipt(string itemId, string promoId, out decimal pctDisc, out decimal amtDisc, out DateTime fromDate, out DateTime toDate)
        {
            amtDisc = 0;
            pctDisc = 0;
            fromDate = DateTime.Now;
            toDate = DateTime.Now;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = "";
                queryString = @"SELECT LINES.[PROMOID]       
							  ,[ITEMID]     
							  ,[DISCAMOUNT]
							  ,[DISCPERCENTAGE]      
							  ,[RETAILSTOREID]
							  ,[FROMDATE]
							  ,[TODATE]
						  FROM [ax].[CPPROMOEDQTYPERSTRUKDETAIL] LINES
						  LEFT JOIN [AX].[CPPROMOEDQTYPERSTRUK] HEADER
						  ON LINES.PROMOID =  HEADER.PROMOID
						  WHERE RETAILSTOREID = @STOREID
						  AND LINES.PROMOID = @PROMOID
						  AND LINES.ITEMID = @ITEMID";

                
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@STOREID", posTransaction.StoreId);
                    command.Parameters.AddWithValue("@PROMOID", promoId);
                    command.Parameters.AddWithValue("@ITEMID", itemId);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                pctDisc = Convert.ToDecimal(reader["DISCPERCENTAGE"]);
                                amtDisc = Convert.ToDecimal(reader["DISCAMOUNT"]);
                                fromDate = Convert.ToDateTime(reader["FROMDATE"].ToString());
                                toDate = Convert.ToDateTime(reader["TODATE"].ToString());
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
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }
        }
		/*
		//Custom By Timothy November 2022 -- Promo Item
		public  Void AddSalesItemPromoItem(InputParameter parm, string axconstring = "", string custAccount = "")
		{
			CommerceRuntime commerceRuntime = null;
			//TerminalModel.Terminal storeInformation = new TerminalModel.Terminal();
			string dataAreaid = null;
			LSRetailPosis.Settings.ConfigFile.AppConfiguration config = new LSRetailPosis.Settings.ConfigFile.AppConfiguration();

		  


			//general.getTerminalFunction(con, config.DATAAREAID, config.StoreId, config.TerminalId, ref storeInformation);


			var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));

			commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal);
			


		   

			 
			try
			{
				PosTransaction pos = saveTransaction.LoadSerializedTransaction(config.StoreId, config.TerminalId, config);
				RetailTransaction retailTrans = (RetailTransaction)pos;

				////Delete Existing Good Product with same item id and redefined line number
				//LinkedList<SaleLineItem> backup = new LinkedList<SaleLineItem>(retailTrans.SaleItems);
				//foreach (SaleLineItem item in backup.Where(x => x.Comment != parm.comment))
				//{
				//    if (item.ItemId == parm.itemid) retailTrans.SaleItems.Remove(item);
				//}

				////redefined line number after delete good product with same item id
				//if (backup.Count != retailTrans.SaleItems.Count)
				//{
				//    int id = 1;
				//    foreach (SaleLineItem item in retailTrans.SaleItems) item.LineId = id++;
				//}

				decimal quantityNow = 0;
				foreach (SaleLineItem item in retailTrans.SaleItems.Where(x => x.Comment == parm.comment))
				{
					if (item.ItemId == parm.itemid) quantityNow += item.Quantity;
				}

				if (quantityNow + parm.quantity > 5000)
				{
					OutputModel.LookupItem itemTrans = new OutputModel.LookupItem();
					//retailTrans.SaleItems = backup;
					itemTrans = this.generateLookupModel(retailTrans);

					OutputModel.outputMessage outMsgDisc = new OutputModel.outputMessage();
					outMsgDisc.status = 200;
					outMsgDisc.error = true;
					outMsgDisc.messsage = "Maximum quantity of items is 5000, please change quantity below 5000";
					outMsgDisc.data = itemTrans;
					return outMsgDisc;
				}

				string transid = retailTrans.TransactionId;
				CRTDM.SalesLine crtSalesLine = new CRTDM.SalesLine();
				Collection<SalesLine> salesLinesColl = new Collection<SalesLine>();

				int linetmp = 0;
				Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices keyPrices = Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.NotMandatory;

				Microsoft.Dynamics.Retail.Pos.Item.Item itm = new Microsoft.Dynamics.Retail.Pos.Item.Item();
				itm.setDataExtend(config.StoreDatabaseConnectionString, parm.terminal.storePrimary, parm.terminal.CultureName, config.DATAAREAID);
				if (parm.quantity <= 0.0M)
				{
					LinkedList<SaleLineItem> listSalesCur = retailTrans.SaleItems;
					SaleLineItem lineDums = new SaleLineItem();
					lineDums = listSalesCur.FirstOrDefault(x => x.ItemId == parm.itemid && x.Comment == parm.comment);
					if (lineDums != null)
					{
						listSalesCur.Remove(lineDums);
					}
					retailTrans.SaleItems = listSalesCur;
				}
				else
				{
					SaleLineItem saleitem = new SaleLineItem(parm.terminal.storecurrency, round, retailTrans);
					saleitem.ItemId = parm.itemid;
					saleitem.Quantity = parm.quantity;

					SaleLineItem lineExist = new SaleLineItem();
					lineExist = retailTrans.SaleItems.FirstOrDefault(x => x.ItemId == parm.itemid && x.Comment == parm.comment);
					if (lineExist != null && lineExist.KeyInPrice != Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.MustKeyInNewPrice)
					{
						saleitem.LineId = lineExist.LineId;
						saleitem.Quantity = parm.quantity + lineExist.Quantity;
					}
					else
					{
						saleitem.LineId = retailTrans.SaleItems.Count + 1;
					}

					saleitem.LineManualDiscountPercentage = parm.lineDiscount;
					saleitem.LineManualDiscountAmount = parm.lineDiscountAmount;
					saleitem.SalesTaxGroupId = parm.terminal.StoreTaxGroup;
					saleitem.Comment = parm.comment;
					getVariantId(ref saleitem, config, axconstring);
					itm.ProcessItemExtend(saleitem);
					if (saleitem.Blocked == true)
					{
						OutputModel.LookupItem outputNotFound = new OutputModel.LookupItem();
						//retailTrans.SaleItems = backup;
						outputNotFound = this.generateLookupModel(retailTrans);

						OutputModel.outputMessage outMsgNotFound = new OutputModel.outputMessage();
						outMsgNotFound.status = 200;
						outMsgNotFound.error = true;
						outMsgNotFound.messsage = "The selected item is blocked and can't be sold";
						outMsgNotFound.data = outputNotFound;
						return outMsgNotFound;
					}

					linetmp = saleitem.LineId;
					keyPrices = saleitem.KeyInPrice;
					saleitem.TaxGroupIdOriginal = saleitem.TaxGroupId;
					saleitem.IncludedInTotalDiscount = true;
					saleitem.SalesOrderUnitOfMeasureName = saleitem.SalesOrderUnitOfMeasure;
					saleitem.InventOrderUnitOfMeasureName = saleitem.InventOrderUnitOfMeasure;
					saleitem.BackofficeSalesOrderUnitOfMeasureName = saleitem.BackofficeSalesOrderUnitOfMeasure;

					crtSalesLine = CommerceRuntimeTransactionConverter.ConvertPosSalesLineItemToCrtSalesLine(saleitem);
					string lineIdString = lineIdString = System.Guid.NewGuid().ToString();
					crtSalesLine.Barcode = parm.barcodeid;
					crtSalesLine.LineId = lineIdString;
					salesLinesColl.Add(crtSalesLine);

					CRTDM.SalesTransaction crtSalesTransaction = new CRTDM.SalesTransaction();
					crtSalesTransaction.IsTaxIncludedInPrice = parm.terminal.TaxIncludedInPrice;

					crtSalesTransaction.SalesLines = salesLinesColl;
					RequestContext context = commerceRuntime.CreateRequestContext(new CalculateTaxServiceRequest());
					context.SetSalesTransaction(crtSalesTransaction);
					CalculateTaxServiceResponse tax = commerceRuntime.Execute<CalculateTaxServiceResponse>(new CalculateTaxServiceRequest(), context);
					crtSalesTransaction = tax.Transaction;

					RequestContext contextTot = commerceRuntime.CreateRequestContext(new CalculateTaxServiceRequest());
					contextTot.SetSalesTransaction(crtSalesTransaction);
					CalculateTotalsServiceResponse tot = commerceRuntime.Execute<CalculateTotalsServiceResponse>(new CalculateTotalsServiceRequest(crtSalesTransaction), contextTot);
					crtSalesTransaction = tot.Transaction;


					RetailTransaction retDum = new RetailTransaction(config.StoreId, parm.terminal.storecurrency, parm.terminal.TaxIncludedInPrice, round);

					SaleLineItem possale = new SaleLineItem(parm.terminal.storecurrency, round, retDum);

					CommerceRuntimeTransactionConverter.PopulatePosLineFromCrtLine(retailTrans, possale, crtSalesTransaction.SalesLines[0]);
					CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(crtSalesTransaction, retDum);


					LinkedList<SaleLineItem> listSales = retailTrans.SaleItems;
					SaleLineItem lineDum = new SaleLineItem();
					lineDum = listSales.FirstOrDefault(x => x.ItemId == parm.itemid && x.Comment == parm.comment);
					if (lineDum != null && keyPrices != Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.MustKeyInNewPrice)
					{
						listSales.Find(lineDum).Value = possale;
					}
					else
					{
						retailTrans.Add(possale);
					}
				}
				retailTrans.TransactionId = "";
				CRTDM.SalesTransaction crtsales = new CRTDM.SalesTransaction();
				crtsales = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(retailTrans);
				GetPriceServiceResponse responseEnd = commerceRuntime.Execute<GetPriceServiceResponse>(new UpdatePriceServiceRequest(crtsales, false), null);
				crtsales = responseEnd.Transaction;

				CalculateTotalsServiceResponse total = commerceRuntime.Execute<CalculateTotalsServiceResponse>(new CalculateTotalsServiceRequest(crtsales), null);
				crtsales = total.Transaction;

				CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(crtsales, retailTrans);

				retailTrans.SaleItems.FirstOrDefault(x => x.LineId == linetmp).KeyInPrice = keyPrices;

				retailTrans.TransactionId = transid;

				foreach (var saleitem in retailTrans.SaleItems)
				{
					saleitem.TaxGroupIdOriginal = saleitem.TaxGroupId;
					saleitem.IncludedInTotalDiscount = true;
					saleitem.SalesOrderUnitOfMeasureName = saleitem.SalesOrderUnitOfMeasure;
					saleitem.InventOrderUnitOfMeasureName = saleitem.InventOrderUnitOfMeasure;
					saleitem.BackofficeSalesOrderUnitOfMeasureName = saleitem.BackofficeSalesOrderUnitOfMeasure;
					saleitem.TaxLines.Clear();
					//TaxLine tx = saleitem.TaxLines.FirstOrDefault();

				}

				bool overridePrice = keyPrices == Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.MustKeyInNewPrice ? true : false;

				if (retailTrans.SaleItems.Count(x => x.Price == 0 && x.KeyInPrice != Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.MustKeyInNewPrice) > 0)
				{
					OutputModel.LookupItem outputNotFound = new OutputModel.LookupItem();
					//retailTrans.SaleItems = backup;
					outputNotFound = this.generateLookupModel(retailTrans);

					OutputModel.outputMessage outMsgNotFound = new OutputModel.outputMessage();
					outMsgNotFound.status = 200;
					outMsgNotFound.error = true;
					outMsgNotFound.messsage = "The selected item can't be sold";
					outMsgNotFound.data = outputNotFound;
					return outMsgNotFound;
				}

				//Add Discount
				PromoItem promo = JsonConvert.DeserializeObject<PromoItem>(parm.promoBadProduct);

				//Validation between NetAmountWithTaxWithoutPromo + overallTotalDiscountWithoutPromo > MinPayment
				decimal netAmountWithTaxWithoutPromo = 0;
				decimal overallTotalDiscountWithoutPromo = 0;
				foreach (SaleLineItem item in retailTrans.SaleItems.Where(x => x.Comment != parm.comment))
				{
					netAmountWithTaxWithoutPromo += item.NetAmountWithTax;
					overallTotalDiscountWithoutPromo += item.TotalDiscount;
				}

				if (netAmountWithTaxWithoutPromo + overallTotalDiscountWithoutPromo >= promo.MinPayment)
				{
					//AddCustomer
					LSRetailPosis.Transaction.Customer customer = new LSRetailPosis.Transaction.Customer();
					customer = GetCustomerInfo(custAccount, config);
					SalesOrderTransaction soTransaction = retailTrans as SalesOrderTransaction;
					if (soTransaction != null)
					{
						// Must check for ISalesOrderTransaction before IRetailTransaction because it derives from IRetailTransaction
						soTransaction.Customer = customer as LSRetailPosis.Transaction.Customer;
					}
					else
					{
						if (retailTrans != null)
						{
							if (!retailTrans.IsCustomerAllowed())
							{
								OutputModel.outputMessage outMsg = new OutputModel.outputMessage();
								outMsg.status = 200;
								outMsg.error = true;
								outMsg.messsage = "This transaction not allow for add customer";
								return outMsg;
							}

							//retailTrans.AffiliationLines = new LinkedList<AffiliationItem>();
							//retailTrans.LoyaltyItem = new LoyaltyItem();

							DE.ICustomer invoicedCustomer = customer;
							string invoiceAccount = customer.InvoiceAccount;

							//If the customer has another account as invoice account
							if (!string.IsNullOrWhiteSpace(invoiceAccount))
							{
								//invoicedCustomer = CustomerSystem.GetCustomerInfo(invoiceAccount);
							}

							if (retailTrans.Customer == null || !retailTrans.Customer.IsEmptyCustomer())
							{
								//this.ResetCustomer(retailTransaction, false, CustomerSystem.ShouldResetLoyaltyItem(customer, retailTransaction1));
							}
							retailTrans.Customer = null;

							retailTrans.Customer = customer as LSRetailPosis.Transaction.Customer;
						}
					}

					//Calculate Discount
					//List<SaleLineItem> tempAdd = new List<SaleLineItem>();
					foreach (SaleLineItem item in retailTrans.SaleItems)
					{
						if (item.ItemId == parm.itemid && item.Comment == parm.comment)
						{
							//decimal maxQuantityPerReceipt = promo.Items.FirstOrDefault(x => x.ItemId == item.ItemId).MaxQtyPerReceipt;
							if (item.DiscountLines.Count > 0)
							{
								//There is discount Applied in sales lines
								foreach (DiscountItem discExist in item.DiscountLines)
								{
									PeriodicDiscountItem periodicDiscountItemExist = discExist as PeriodicDiscountItem;
									if (periodicDiscountItemExist.OfferId == promo.PromoId)
										periodicDiscountItemExist.QuantityDiscounted = item.Quantity;
									//periodicDiscountItemExist.QuantityDiscounted = item.Quantity > maxQuantityPerReceipt ? maxQuantityPerReceipt : item.Quantity;
								}
							}
							else
							{
								DiscountItem discountItem = (DiscountItem)new PeriodicDiscountItem();
								if (promo.Items.FirstOrDefault(x => x.ItemId == item.ItemId).DiscountPercentage == 0)
								{
									discountItem.Amount = Decimal.Round(promo.Items.FirstOrDefault(x => x.ItemId == item.ItemId).DiscountAmount);
								}
								else
								{
									discountItem.Percentage = Convert.ToDecimal(promo.Items.FirstOrDefault(x => x.ItemId == item.ItemId).DiscountPercentage);
								}

								PeriodicDiscountItem periodicDiscountItem = discountItem as PeriodicDiscountItem;
								periodicDiscountItem.OfferId = promo.PromoId;
								periodicDiscountItem.OfferName = promo.PromoName;
								periodicDiscountItem.QuantityDiscounted = item.Quantity;
								//periodicDiscountItem.QuantityDiscounted = item.Quantity > maxQuantityPerReceipt ? maxQuantityPerReceipt : item.Quantity;
								periodicDiscountItem.BeginDateTime = Convert.ToDateTime(promo.FromDate);
								periodicDiscountItem.EndDateTime = Convert.ToDateTime(promo.ToDate).AddHours(23).AddMinutes(59).AddSeconds(59);

								item.DiscountLines.AddFirst(periodicDiscountItem);
							}

							////split quantity bad when bad product is greater than max quantity per receipt, create a new salesline without discount
							//if (item.Quantity > maxQuantityPerReceipt)
							//{
							//    decimal newQuantityWithoutDisc = item.Quantity - maxQuantityPerReceipt;
							//    item.Quantity = maxQuantityPerReceipt;

							//    //create a new sales lines or update exist quantity item non bad prodcut
							//    if (retailTrans.SaleItems.Count(x => x.ItemId == item.ItemId && x.Comment != item.Comment) > 0)
							//    {
							//        //update exist qty
							//        retailTrans.SaleItems.FirstOrDefault(x => x.ItemId == item.ItemId && x.Comment != item.Comment).Quantity += newQuantityWithoutDisc;
							//        retailTrans.SaleItems.FirstOrDefault(x => x.ItemId == item.ItemId && x.Comment != item.Comment).TaxLines.Clear();
							//        retailTrans.SaleItems.FirstOrDefault(x => x.ItemId == item.ItemId && x.Comment != item.Comment).DiscountLines.Clear();
							//    }
							//    else
							//    {
							//        //insert a new salesitem
							//        SaleLineItem newItem = new SaleLineItem(parm.terminal.storecurrency, round, retailTrans);
							//        newItem = item.CloneLineItem() as SaleLineItem;
							//        newItem.Quantity = newQuantityWithoutDisc;
							//        newItem.LineId = retailTrans.SaleItems.Count + tempAdd.Count + 1;
							//        newItem.Comment = "";
							//        newItem.TaxLines.Clear();
							//        newItem.DiscountLines.Clear();

							//        tempAdd.Add(newItem);
							//    }

							//}
						}
					}
					//foreach (SaleLineItem item in tempAdd)
					//{
					//    retailTrans.SaleItems.AddLast(item);
					//}
					retailTrans.CalcTotals();
				}
				else
				{
					retailTrans.SaleItems.FirstOrDefault(x => x.ItemId == parm.itemid && x.Comment == parm.comment).Comment = "";
				}
				//End Add Discount

				//Calculate Tax
				TaxCodeProvider provide = new TaxCodeProvider();
				provide = new TaxCodeProvider();
				provide.setExtend(new SqlConnection(config.StoreDatabaseConnectionString), config.DATAAREAID);
				provide.CalculateTaxExtend(ref retailTrans);

				retailTrans.CalcTotals();

				cekTrxId(ref retailTrans, config);

				saveTransaction.SaveSerializedTransaction(retailTrans, config.StoreId, config.TerminalId, dbUtil, true, config.DATAAREAID);

				OutputModel.LookupItem output = new OutputModel.LookupItem();
				output = this.generateLookupModel(retailTrans, overridePrice);

				OutputModel.outputMessage outMsg1 = new OutputModel.outputMessage();
				outMsg1.status = 200;
				outMsg1.error = false;
				outMsg1.messsage = parm.promoBadProduct;
				outMsg1.data = output;
				return outMsg1;
			}

			catch (Exception ee)
			{
				dbUtil.EndTransaction();
				EventLogWrite.writeEvent("Add Item Promo Item", ee.ToString());
				OutputModel.outputMessage outMsg1 = new OutputModel.outputMessage();
				outMsg1.status = 200;
				outMsg1.error = true;
				outMsg1.messsage = ee.Message;
				return outMsg1;
			}
		}
		*/

		private void addItem(RetailTransaction retailTrans)
		{
			string itemid = "11320006";
			decimal quantity = 1;
			SaleLineItem saleitem = new SaleLineItem(retailTrans.StoreCurrencyCode, retailTrans.IRounding, (PosTransaction)retailTrans);
			saleitem.ItemId = itemid;
			saleitem.Quantity = quantity;
			CommerceRuntime commerceRuntime = null;
			Collection<SalesLine> salesLinesColl = new Collection<SalesLine>();
			LSRetailPosis.Settings.ConfigFile.AppConfiguration config = new LSRetailPosis.Settings.ConfigFile.AppConfiguration();

			 
			//general.getTerminalFunction(con, config.DATAAREAID, config.StoreId, config.TerminalId, ref storeInformation);


			var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
						
			 commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal);
					   
			SaleLineItem lineExist = new SaleLineItem();
			lineExist = retailTrans.SaleItems.FirstOrDefault(x => x.ItemId == saleitem.ItemId);//&& x.Comment == parm.comment);
			if (lineExist != null && lineExist.KeyInPrice != Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.MustKeyInNewPrice)
			{
				saleitem.LineId = lineExist.LineId;
				saleitem.Quantity = 1 + lineExist.Quantity;
			}
			else
			{
				saleitem.LineId = retailTrans.SaleItems.Count + 1;
			}

			saleitem.LineManualDiscountPercentage = 0;
			saleitem.LineManualDiscountAmount = 0;
			saleitem.SalesTaxGroupId = ApplicationSettings.Terminal.StoreTaxGroup;
			saleitem.Comment = "";
			//getVariantId(ref saleitem, config, axconstring);
			 
			//if (saleitem.Blocked == true)
			//{
			//    OutputModel.LookupItem outputNotFound = new OutputModel.LookupItem();
			//    //retailTrans.SaleItems = backup;
			//    outputNotFound = this.generateLookupModel(retailTrans);

			//    OutputModel.outputMessage outMsgNotFound = new OutputModel.outputMessage();
			//    outMsgNotFound.status = 200;
			//    outMsgNotFound.error = true;
			//    outMsgNotFound.messsage = "The selected item is blocked and can't be sold";
			//    outMsgNotFound.data = outputNotFound;
			//    return outMsgNotFound;
			//}

			int linetmp = saleitem.LineId;
			keyPrices = saleitem.KeyInPrice;
			saleitem.TaxGroupIdOriginal = saleitem.TaxGroupId;
			saleitem.IncludedInTotalDiscount = true;
			saleitem.SalesOrderUnitOfMeasureName = saleitem.SalesOrderUnitOfMeasure;
			saleitem.InventOrderUnitOfMeasureName = saleitem.InventOrderUnitOfMeasure;
			saleitem.BackofficeSalesOrderUnitOfMeasureName = saleitem.BackofficeSalesOrderUnitOfMeasure;

			crtSalesLine = CommerceRuntimeTransactionConverter.ConvertPosSalesLineItemToCrtSalesLine(saleitem);
			string lineIdString = lineIdString = System.Guid.NewGuid().ToString();
			crtSalesLine.Barcode = "11320006";
			crtSalesLine.LineId = lineIdString;
			salesLinesColl.Add(crtSalesLine);

			CRTDM.SalesTransaction crtSalesTransaction = new CRTDM.SalesTransaction();
			crtSalesTransaction.IsTaxIncludedInPrice = true;//parm.terminal.TaxIncludedInPrice;

			crtSalesTransaction.SalesLines = salesLinesColl;
			RequestContext context = commerceRuntime.CreateRequestContext(new CalculateTaxServiceRequest());
			context.SetSalesTransaction(crtSalesTransaction);
			CalculateTaxServiceResponse tax = commerceRuntime.Execute<CalculateTaxServiceResponse>(new CalculateTaxServiceRequest(), context);
			crtSalesTransaction = tax.Transaction;

			RequestContext contextTot = commerceRuntime.CreateRequestContext(new CalculateTaxServiceRequest());
			contextTot.SetSalesTransaction(crtSalesTransaction);
			CalculateTotalsServiceResponse tot = commerceRuntime.Execute<CalculateTotalsServiceResponse>(new CalculateTotalsServiceRequest(crtSalesTransaction), contextTot);
			crtSalesTransaction = tot.Transaction;


			RetailTransaction retDum = new RetailTransaction(retailTrans.StoreId, retailTrans.StoreCurrencyCode , retailTrans.TaxIncludedInPrice, retailTrans.IRounding);

			SaleLineItem possale = new SaleLineItem(retailTrans.StoreCurrencyCode, retailTrans.IRounding, retDum);

			CommerceRuntimeTransactionConverter.PopulatePosLineFromCrtLine(retailTrans, possale, crtSalesTransaction.SalesLines[0]);
			CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(crtSalesTransaction, retDum);


			LinkedList<SaleLineItem> listSales = retailTrans.SaleItems;
			SaleLineItem lineDum = new SaleLineItem();
			lineDum = listSales.FirstOrDefault(x => x.ItemId == itemid);//&& x.Comment == parm.comment);
			if (lineDum != null && keyPrices != Microsoft.Dynamics.Retail.Pos.Contracts.Services.KeyInPrices.MustKeyInNewPrice)
			{
				listSales.Find(lineDum).Value = possale;
			}
			else
			{
				retailTrans.Add(possale);
			}
		}


		private decimal ShowQuantitySelectionForm(decimal startingQty)
		{
			// Instantiate the quantity selection form
            QuantitySelectionForm quantityForm = new QuantitySelectionForm(startingQty);


			// Display the form as a dialog
			if (quantityForm.ShowDialog() == DialogResult.OK)
			{
				// Quantity selected, do something if needed
				decimal selectedQuantity = quantityForm.SelectedQuantity;
				return selectedQuantity;
				// You can use the selectedQuantity here
			}
			else
			{

				return 0;
			}
			   
		}
		

		private void gridStyle()
		{
			
			dataGridResult.Columns[0].Width = 70;
			dataGridResult.Columns[0].Visible = false;
			dataGridResult.Columns[1].Width = 70;
			dataGridResult.Columns[2].Width = 70;
			dataGridResult.Columns[3].Width = 80;
			dataGridResult.Columns[4].Width = 70;
			dataGridResult.Columns[5].Width = 180;
			dataGridResult.Columns[6].Width = 80;
			dataGridResult.Columns[7].Width = 70;
			dataGridResult.Columns[7].DefaultCellStyle.Format = "N2";
			dataGridResult.Columns[8].Width = 70;
			dataGridResult.Columns[8].DefaultCellStyle.Format = "N2";
			dataGridResult.Columns[9].Visible = false;
			dataGridResult.Columns[10].Width = 80;
			dataGridResult.Columns[10].DefaultCellStyle.Format = "N2";

            if (dataGridResult.Columns[11] != null)
            {
                dataGridResult.Columns[11].Width = 80;
                dataGridResult.Columns[11].DefaultCellStyle.Format = "N2";
            }


            /*[DESCRIPTION] as [Promo Name], 
			[FROMDATE] as [From Date], 
			[TODATE] as [To Date], 
			[MINPAYMENTAMOUNT] as [Min. Payment], 
			[ITEMID] as [ItemId], 
			[PRODUCTNAME] as [Item Name], 
			[DISCAMOUNT] as [Disc. Amount], 
			[DISCPERCENTAGE] as [Disc. Percentage], 
			[MAXQTY] as [Max Qty], 
			CPPROMOEDQTYDETAIL.PROMOID AS [Promo ID],*/

            /*[DESCRIPTION] as [Promo Name], 
	        [FROMDATE] as [From Date], 
	        [TODATE] as [To Date], 
	        [MINPAYMENTAMOUNT] as [Min. Payment], 
	        [ITEMID] as [ItemId], 
	        [PRODUCTNAME] as [Item Name], 
	        [DISCAMOUNT] as [Disc. Amount], 
	        [DISCPERCENTAGE] as [Disc. Percentage], 
	        [MAXQTY] as [Max Qty], 
	        CPPROMOEDQTYPERSTRUKDETAIL.PROMOID AS [Promo ID],
	        CPPROMOEDQTYPERSTRUKDETAIL.MAXQTYPERRECEIPT AS [Max Qty per Rcpt],
             
             */
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void dataGridHeader_SelectionChanged(object sender, EventArgs e)
		{
			//if (dataGridHeader.SelectedRows.Count > 0)
			//{
			//    // Get the selected row
			//    DataGridViewRow selectedRow = dataGridHeader.SelectedRows[0];

			//    // Assuming columnIndex is the index of the cell containing PROMO ID
			//    int promoIdCellIndex = 0; // You need to specify the correct index
			//    // Get the value of the cell containing PROMO ID
			//    string promoIdValue = selectedRow.Cells[promoIdCellIndex].Value.ToString();

			//    // Update the string variable with the PROMO ID value
			//    promoIdString = promoIdValue;
				
			//    LoadData();
			//}
		}

		private void dataGridHeader_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			// Check if there are any rows in the DataGridView
			if (dataGridHeader.Rows.Count > 0)
			{
				// Get the value of the first row, first column cell
				string promoIdValue = dataGridHeader.Rows[0].Cells[0].Value.ToString();

				// Update the string variable with the value
				promoIdString = promoIdValue;
			}
		}
	}

	public class QuantitySelectionForm : Form
	{
		// Define controls for quantity selection form
		private TextBox quantityTextBox;
		private Button confirmButton;
        private decimal startingQty;
		// Property to access selected quantity
		public decimal SelectedQuantity { get; private set; }

		public QuantitySelectionForm(decimal startQty)
		{
            startingQty = startQty;
			InitializeComponent();
            quantityTextBox.Text = startingQty.ToString();
		}

		private void InitializeComponent()
		{
			// Initialize form and controls
			this.Text = "Jumlah Barang";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.FixedDialog; // Set the form border style to FixedDialog
			this.MaximizeBox = false; // Disable maximize button
			this.MinimizeBox = false; // Disable minimize button
			this.Size = new System.Drawing.Size(200, 150);

			// Label for "Select Quantity"
			Label selectQuantityLabel = new Label();
			selectQuantityLabel.Text = "Pilih Jumlah Barang:";
			selectQuantityLabel.Location = new System.Drawing.Point(10, 10);
			selectQuantityLabel.AutoSize = true;
			this.Controls.Add(selectQuantityLabel);

			// Textbox for quantity
			quantityTextBox = new TextBox();
			quantityTextBox.Text = "1"; // Set default value to 1
			quantityTextBox.Location = new System.Drawing.Point(50, 30);
			quantityTextBox.Size = new System.Drawing.Size(50, 20);
			quantityTextBox.KeyPress += QuantityTextBox_KeyPress; // Add KeyPress event handler
			this.Controls.Add(quantityTextBox);


			// Minus button
			Button minusButton = new Button();
			minusButton.Text = "-";
			minusButton.Location = new System.Drawing.Point(10, 30);
			minusButton.Size = new System.Drawing.Size(30, 20);
			minusButton.Click += MinusButton_Click;
			this.Controls.Add(minusButton);

			// Plus button
			Button plusButton = new Button();
			plusButton.Text = "+";
			plusButton.Location = new System.Drawing.Point(110, 30);
			plusButton.Size = new System.Drawing.Size(30, 20);
			plusButton.Click += PlusButton_Click;
			this.Controls.Add(plusButton);

			// Confirm button
			confirmButton = new Button();
			confirmButton.Text = "Konfirmasi";
			confirmButton.Location = new System.Drawing.Point(50, 70);
			confirmButton.Size = new System.Drawing.Size(100, 30);
			confirmButton.Click += ConfirmButton_Click;
			this.Controls.Add(confirmButton);
		}

		// Event handler for KeyPress event of quantityTextBox
		private void QuantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Check if the entered character is a digit or control character
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				// If not a digit or control character, set handled to true to suppress the key press
				e.Handled = true;
			}
		}

		// Event handler for Minus button click
		private void MinusButton_Click(object sender, EventArgs e)
		{
			// Get the current value
			decimal currentValue;
			if (decimal.TryParse(quantityTextBox.Text, out currentValue))
			{
				// Check if the value is greater than 1 before decrementing
				if (currentValue > 1)
					quantityTextBox.Text = (currentValue - 1).ToString();
			}
		}

		// Event handler for Plus button click
		private void PlusButton_Click(object sender, EventArgs e)
		{
			// Get the current value
			decimal currentValue;
			if (decimal.TryParse(quantityTextBox.Text, out currentValue))
			{
				// Increment the value
				quantityTextBox.Text = (currentValue + 1).ToString();
			}
		}

		// Event handler for Confirm button click
		private void ConfirmButton_Click(object sender, EventArgs e)
		{
			decimal selectedQuantity;
			if (decimal.TryParse(quantityTextBox.Text, out selectedQuantity) && selectedQuantity > 0)
			{
				SelectedQuantity = selectedQuantity; // Parse the decimal value
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				MessageBox.Show("Please enter a valid positive number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		// Event handler for form closing
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			// Check if the form is closing without selecting a quantity intentionally
			if (this.DialogResult != DialogResult.OK)
			{
				// Set the DialogResult property to DialogResult.Cancel
				this.DialogResult = DialogResult.Cancel;
			}
			base.OnFormClosing(e);
		}
	}
}
