/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Linq;
using System.Text;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Globalization;
namespace Microsoft.Dynamics.Retail.Pos.EOD
{
    /// <summary>
    /// Class warps Batch printing functions.
    /// </summary>
    static class BatchPrinting
    {

        #region Fields

        private static int paperWidth = 55;
        private static readonly string singleLine = string.Empty.PadLeft(paperWidth, '-');
        private static readonly string lineFormat = ApplicationLocalizer.Language.Translate(7060);
        private static readonly string currencyFormat = ApplicationLocalizer.Language.Translate(7061);
        private static readonly string typeFormat = ApplicationLocalizer.Language.Translate(7062);

        #endregion

        #region Methods

        /// <summary>
        /// Print a batch to the printer as X or Z report.
        /// </summary>
        /// <param name="batch">Calculated batch object</param>
        /// <param name="reportType">Report type</param>
        public static void Print(this Batch batch, ReportType reportType)
        {
            // TextID's for the Z/X Report are reserved at 7000 - 7099 

            if (EOD.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled()) 
            {
                paperWidth = EOD.InternalApplication.Services.Peripherals.FiscalPrinter.GetLineLegth(null);
            }

            StringBuilder reportLayout = new StringBuilder(2500);

            //Begin add NEC Hamzah
            decimal totalDebit, totalCredit, totalGiftCard, totalPoint;
            totalDebit = 0;
            totalCredit = 0;
            totalGiftCard = 0;
            totalPoint = 0;
            decimal salesTotalCustom = BatchCalculation.getSalesTotal(batch);
            decimal paymentCashCustom = BatchCalculation.getPaymentCash(batch);
 //           decimal paymentEzeelinkCustom = BatchCalculation.getPaymentEzeelink(batch);
            decimal giftCardTotal = BatchCalculation.getGiftCard(batch);
            decimal loyaltyCardTotal = BatchCalculation.getLoyaltyCard(batch);

            DataTable dtItemContainer = new DataTable();
            DataTable dtPaymentContainer = new DataTable();

            dtItemContainer = BatchCalculation.getItemContainer(batch);
            dtPaymentContainer = BatchCalculation.getPaymentDebit(batch);

            decimal AmountWalkInCustom = BatchCalculation.getAmountWalkIn(batch);
            decimal CountWalkInCustom = BatchCalculation.getCountWalkIn(batch);
            decimal AmountCanvasCustom = BatchCalculation.getAmountCanvas(batch);
            decimal CountCanvasCustom = BatchCalculation.getCountCanvas(batch);
            decimal AmountDeliveryOrderCustom = BatchCalculation.getAmountDeliveryOrder(batch);
            decimal CountDeliveryOrderCustom = BatchCalculation.getCountDeliveryOrder(batch);
            decimal custAmountTotal = BatchCalculation.getCustAmountTotal(batch);
            decimal custCountTotal = BatchCalculation.getCustCountTotal(batch);
            // End add NEC Hamzah
            // Header
            reportLayout.PrepareHeader(batch, reportType);

            // Total Amounts
            //Begin add and comment standard NEC
            reportLayout.AppendReportLine(7015);
            //reportLayout.AppendReportLine(7016, RoundDecimal(batch.SalesTotal));
            //reportLayout.AppendReportLine(7017, RoundDecimal(batch.ReturnsTotal));
            
            reportLayout.AppendLine();
            reportLayout.AppendReportLine(14000, RoundDecimal(salesTotalCustom));
            //reportLayout.AppendReportLine(7018, RoundDecimal(batch.TaxTotal));
            //reportLayout.AppendReportLine(7019, RoundDecimal(batch.DiscountTotal));
            //reportLayout.AppendReportLine(7020, RoundDecimal(batch.RoundedAmountTotal));
            //reportLayout.AppendReportLine(7021, RoundDecimal(batch.PaidToAccountTotal));
            //reportLayout.AppendReportLine(7022, RoundDecimal(batch.IncomeAccountTotal));
            //reportLayout.AppendReportLine(7023, RoundDecimal(batch.ExpenseAccountTotal));
            //if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
            //{
            //    reportLayout.AppendReportLine(7085, RoundDecimal(batch.SalesTotalExlcudingTax));
            //    reportLayout.AppendReportLine(7086, RoundDecimal(batch.SalesTotalIncludingTax));
            //    reportLayout.AppendReportLine(7084, RoundDecimal(batch.StartingAmountTotal));
            //    reportLayout.AppendReportLine(7071, RoundDecimal(batch.ReceiptCopiesTotal));
            //    reportLayout.AppendReportLine(7072, RoundDecimal(batch.TrainingTotal));
            //    reportLayout.AppendReportLine(7087, RoundDecimal(batch.PriceOverrideTotal));
            //    reportLayout.AppendReportLine(7067, RoundDecimal(batch.SuspendedTotal));
            //    reportLayout.AppendReportLine(7074, RoundDecimal(batch.SalesGrandTotal));
            //    reportLayout.AppendReportLine(7075, RoundDecimal(batch.ReturnsGrandTotal));
            //    reportLayout.AppendReportLine(7076, RoundDecimal(batch.SalesGrandTotal - batch.ReturnsGrandTotal));
            //    PrintTaxLines(batch, reportLayout);
            //}
            // End add and comment standard NEC
            reportLayout.AppendLine();
            //Begin add and comment standard NEC
            reportLayout.AppendReportLine(14020);
            // Statistics
            //reportLayout.AppendReportLine(7035);
            //reportLayout.AppendReportLine(7036, batch.SalesCount);
            //reportLayout.AppendReportLine(7038, batch.CustomersCount);
            //reportLayout.AppendReportLine(7039, batch.VoidsCount);
            //reportLayout.AppendReportLine(7040, batch.LogOnCount);
            //reportLayout.AppendReportLine(7041, batch.NoSaleCount);
            //if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
            //{
            //    reportLayout.AppendReportLine(7077, RoundDecimal(batch.GoodsSoldQty));
            //    reportLayout.AppendReportLine(7078, RoundDecimal(batch.ServicesSoldQty));
            //    reportLayout.AppendReportLine(7071, batch.ReceiptCopiesCount);
            //    reportLayout.AppendReportLine(7079, batch.ReceiptsCount);
            //    reportLayout.AppendReportLine(7080, batch.ReturnsCount);
            //    reportLayout.AppendReportLine(7072, batch.TrainingCount);
            //}

            //if (reportType == ReportType.XReport || LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
            //{
            //    reportLayout.AppendReportLine(7042, batch.SuspendedTransactionsCount);
            //}

            foreach (DataRow row in dtItemContainer.Rows)
            {
                reportLayout.AppendLine(string.Format("{0}        {1} - {2}", row[0].ToString(),row[2], RoundDecimal(Convert.ToDecimal(row[3]))));
                if (row[4].ToString() != "")
                {
                    reportLayout.AppendLine(string.Format("{0}", row[4].ToString())); //added by yonathan for displaying the variant id
                }
                reportLayout.AppendLine(string.Format("{0}", row[1].ToString()));

                    /*reportLayout.AppendLine(string.Format("{0} - {1}", row[0].ToString(), row[1].ToString()));
                if (row[4].ToString() != "")
                {
                    reportLayout.AppendLine(string.Format("{0}", row[4].ToString())); //added by yonathan for displaying the variant id
                }
                reportLayout.AppendLine(string.Format("{0} - {1}", row[2], RoundDecimal(Convert.ToDecimal(row[3]))));*/
                
            }

            reportLayout.AppendLine();

            reportLayout.AppendReportLine(14010);
            reportLayout.AppendReportLine(14011, RoundDecimal(paymentCashCustom));

  //          if (paymentEzeelinkCustom != 0)
  //              reportLayout.AppendReportLine("Ezeelink", RoundDecimal(paymentEzeelinkCustom));

            foreach (DataRow row in dtPaymentContainer.Rows)
            {
                Int32 cardIdUser = Convert.ToInt32(row[1]);
                decimal amountCard = Convert.ToDecimal(row[2]);

                switch (cardIdUser)
                {
                    //0 - Credit Card
                    case 0:
                        totalCredit += amountCard;
                        break;
                    //1 - Debit Card
                    case 1:
                        totalDebit += amountCard;
                        break;
                    //2 - Loyalty Card - Point
                    case 2:
                        totalPoint += amountCard;
                        break;
                    //7 - Gift Card
                    case 7:
                        totalGiftCard += amountCard;
                        break;
                }
            }
            if (totalDebit != 0)
                reportLayout.AppendReportLine(14012, RoundDecimal(totalDebit));
            if (totalCredit != 0)
                reportLayout.AppendReportLine(14013, RoundDecimal(totalCredit));
            if (totalGiftCard != 0)
                reportLayout.AppendReportLine(14014, RoundDecimal(totalGiftCard));
            if (totalPoint != 0)
                reportLayout.AppendReportLine(14015, RoundDecimal(totalPoint));

            //from payment method
            //if (giftCardTotal != 0)
            //    reportLayout.AppendReportLine(14014, RoundDecimal(giftCardTotal));
            if (loyaltyCardTotal != 0)
                reportLayout.AppendReportLine(14015, RoundDecimal(loyaltyCardTotal));

            //ADD BY ERWIN
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            var tenderID = new List<string>();

            try
            {
                //to do change to RETAILTRANSACTIONPAYMENTTRANS FOR TENDERID DISTINCT YONATHAN
                //string queryString = @" SELECT DISTINCT TenderID FROM ax.CPNewPaymentPOS";
                //CHANGE TO TENDERTYPE ID FROM TABLE MASTER - 18122024 YONATHAN
                string queryString = @"
                                        SELECT TENDERTYPEID FROM AX.RETAILSTORETENDERTYPETABLE
                                        WHERE CHANNEL = @channelId AND
	                                        TENDERTYPEID!=1
                                        ORDER BY CAST(TENDERTYPEID AS INT)";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@channelId", ApplicationSettings.Terminal.StorePrimaryId);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            /*string sqlCashTotal = "SELECT sum(L.AMOUNTCUR)" +
                                                    "FROM RETAILTRANSACTIONPAYMENTTRANS As L " +
                                                    "INNER JOIN RETAILTRANSACTIONTABLE AS H " +
                                                        "ON H.TRANSACTIONID = L.TRANSACTIONID " +
                                                        "AND H.STORE = L.STORE " +
                                                        "AND H.TERMINAL = L.TERMINAL " +
                                                        "AND H.DATAAREAID = L.DATAAREAID " +
                                                    "INNER JOIN CPNewPaymentPOS C ON L.TRANSACTIONID = C.TRANSACTIONID" +
                                                    "WHERE H.BATCHTERMINALID = @BATCHTERMINALID " +
                                                        "AND H.DATAAREAID = @DATAAREAID " +
                                                        "AND H.BATCHID = @BATCHID " +
                                                        "AND C.TenderID = @TenderID " +
                                                        "AND L.TRANSACTIONSTATUS = 0";*/

                            tenderID.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            //add to receipt
            foreach(string tender in tenderID)
            {
                SqlConnection conCustAccount = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

                try
                {
                  //to do change to LOOKUP TENDERNAME FROM MASTER - YONATHAN 
                    /*string queryCustAccount = @"
                                                SELECT  
                                                    C.TenderName, 
                                                    sum(L.AMOUNTCUR)
                                                FROM 
                                                    RETAILTRANSACTIONPAYMENTTRANS AS L 
                                                    INNER JOIN RETAILTRANSACTIONTABLE AS H 
                                                        ON H.TRANSACTIONID = L.TRANSACTIONID  
                                                        AND H.STORE = L.STORE 
                                                        AND H.TERMINAL = L.TERMINAL 
                                                        AND H.DATAAREAID = L.DATAAREAID 
                                                    INNER JOIN ax.CPNEWPAYMENTPOS AS C 
                                                        ON L.TRANSACTIONID = C.TRANSACTIONID 
                                                        and L.STORE = C.STORE
                                                WHERE 
                                                    H.BATCHTERMINALID = @BATCHTERMINALID 
                                                    AND H.DATAAREAID = @DATAAREAID 
                                                    AND H.BATCHID = @BATCHID 
                                                    AND C.TenderID = @TenderID 
                                                    AND L.TENDERTYPE = C.TenderID 
                                                    AND L.TRANSACTIONSTATUS = 0
                                                GROUP BY
                                                    C.TenderName";*/

                    string queryCustAccount = @"    SELECT  
                                                        C.NAME, 
                                                        sum(L.AMOUNTCUR)
                                                    FROM 
                                                        RETAILTRANSACTIONPAYMENTTRANS AS L 
                                                        INNER JOIN RETAILTRANSACTIONTABLE AS H 
                                                            ON H.TRANSACTIONID = L.TRANSACTIONID  
                                                            AND H.STORE = L.STORE 
                                                            AND H.TERMINAL = L.TERMINAL 
                                                            AND H.DATAAREAID = L.DATAAREAID 
	                                                    INNER JOIN AX.RETAILSTORETENDERTYPETABLE AS C
                                                        --INNER JOIN ax.CPNEWPAYMENTPOS AS C 
                                                            ON C.TENDERTYPEID = L.TENDERTYPE 
                                                            and C.CHANNEL = @channelId
                                                    WHERE 
                                                         H.BATCHTERMINALID = @BATCHTERMINALID  
                                                        AND H.DATAAREAID = @DATAAREAID 
                                                        AND H.BATCHID = @BATCHID 
                                                        AND C.TENDERTYPEID = @TenderID  
                                                        AND L.TENDERTYPE = C.TENDERTYPEID 
                                                        AND L.TRANSACTIONSTATUS = 0
                                                    GROUP BY
                                                        C.NAME
                                                    ";

                    using (SqlCommand cmdCustomerAcc = new SqlCommand(queryCustAccount, conCustAccount))
                    {
                        cmdCustomerAcc.Parameters.Add(new SqlParameter("@BATCHTERMINALID", batch.TerminalId));
                        cmdCustomerAcc.Parameters.Add(new SqlParameter("@BATCHID", batch.BatchId));
                        cmdCustomerAcc.Parameters.Add(new SqlParameter("@DATAAREAID", EOD.InternalApplication.Settings.Database.DataAreaID));
                        cmdCustomerAcc.Parameters.Add(new SqlParameter("@TenderID", tender));
                        cmdCustomerAcc.Parameters.Add(new SqlParameter("@channelId", ApplicationSettings.Terminal.StorePrimaryId));
                        if (conCustAccount.State != ConnectionState.Open)
                        {
                            conCustAccount.Open();
                        }
                        using (SqlDataReader readerCustomerAcc = cmdCustomerAcc.ExecuteReader())
                        {
                            if (readerCustomerAcc.Read())
                            {
                                reportLayout.AppendReportLine(readerCustomerAcc[0].ToString(), RoundDecimal(Convert.ToDecimal(readerCustomerAcc[1])));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                    throw;
                }
                finally
                {
                    if (conCustAccount.State != ConnectionState.Closed)
                    {
                        conCustAccount.Close();
                    }
                }
            }
            //END ADD BY ERWIN

            reportLayout.AppendLine();
            reportLayout.AppendLine();
            
            #region CPECRBCA
            /*
             * Add Ambil Tunai Amount to X & Z Reports
             */
            SqlConnection connectionECR = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            //add by Yonathan C.BANK != 'QRISBCA' for filtering the bank not QRIS
            decimal totalTunai = 0;
            try
            {
                string queryECR = @"
                                                SELECT  
                                                    C.BANK, 
                                                    sum(C.AMOUNT) as AMOUNT
                                                FROM 
                                                    RETAILTRANSACTIONPAYMENTTRANS AS L 
                                                    INNER JOIN RETAILTRANSACTIONTABLE AS H 
                                                        ON H.TRANSACTIONID = L.TRANSACTIONID  
                                                        AND H.STORE = L.STORE 
                                                        AND H.TERMINAL = L.TERMINAL 
                                                        AND H.DATAAREAID = L.DATAAREAID 
                                                    INNER JOIN ax.CPAMBILTUNAI AS C 
                                                        ON L.TRANSACTIONID = C.TRANSACTIONID 
                                                        and L.STORE = C.STORE
                                                WHERE 
                                                    H.BATCHTERMINALID = @BATCHTERMINALID 
                                                    AND H.DATAAREAID = @DATAAREAID 
                                                    AND H.BATCHID = @BATCHID 
                                                    AND L.TRANSACTIONSTATUS = 0
                                                    AND C.BANK != 'QRISBCA'
                                                GROUP BY
                                                    C.BANK";
                //C.BANK != 'QRISBCA'
                using (SqlCommand cmdAmbilTunai = new SqlCommand(queryECR, connectionECR))
                {
                    cmdAmbilTunai.Parameters.Add(new SqlParameter("@BATCHTERMINALID", batch.TerminalId));
                    cmdAmbilTunai.Parameters.Add(new SqlParameter("@BATCHID", batch.BatchId));
                    cmdAmbilTunai.Parameters.Add(new SqlParameter("@DATAAREAID", EOD.InternalApplication.Settings.Database.DataAreaID));

                    if (connectionECR.State != ConnectionState.Open)
                    {
                        connectionECR.Open();
                    }

                    //int flagFirstItem = 0;

                    using (SqlDataReader readerAmbilTunai = cmdAmbilTunai.ExecuteReader())
                    {
                        //Add header, setup in AX custom control
                        if (readerAmbilTunai.HasRows)
                        {
                            reportLayout.AppendReportLine(99122);
                        }

                        while (readerAmbilTunai.Read())
                        {
                            reportLayout.AppendReportLine(readerAmbilTunai[0].ToString(), RoundDecimal(Convert.ToDecimal(readerAmbilTunai[1])));
                            //Add totalTunai for each bank
                            totalTunai += Convert.ToDecimal(readerAmbilTunai[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (connectionECR.State != ConnectionState.Closed)
                {
                    connectionECR.Close();
                }
            }

            reportLayout.AppendLine();

            //add by Yonathan 20/20/2022
            Cashout cashOut = new Cashout();

            decimal amountCashOut = 0;
            string fromDateTime = "";
            string toDateTime = "";
            if (reportType == ReportType.ZReport)
            {
                amountCashOut = (Math.Truncate(Convert.ToDecimal(cashOut.getAmountCashout(batch.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"), batch.CloseDateTime.ToString("yyyy-MM-dd HH:mm:ss"))) * 1000m) / 1000m);

            }
            else
            {
                amountCashOut = (Math.Truncate(Convert.ToDecimal(cashOut.getAmountCashout(batch.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))) * 1000m) / 1000m);
            }
            
            //calcuate dataAmountCash out
            string stringCashout = amountCashOut.ToString();
            reportLayout.AppendLine(string.Format("Cash Out : {0}", RoundDecimal(amountCashOut)));
            //end add yonathan

            //setup available cash remaining
            reportLayout.AppendLine("-------------------------------------------------------");
            reportLayout.AppendLine(string.Format("Available Cash : {0}", RoundDecimal(paymentCashCustom - totalTunai - amountCashOut)));
            reportLayout.AppendLine("-------------------------------------------------------");
            #endregion
            //END ADD BY ERWIN

            

            reportLayout.AppendLine(string.Format("Walk In Cust : {0} = {1}", RoundDecimal(AmountWalkInCustom), CountWalkInCustom));
            //disable cust order calc because have details in below - Yonathan 02092024
            //reportLayout.AppendLine(string.Format("Customer Order : {0} = {1}", RoundDecimal(AmountCanvasCustom), CountCanvasCustom));
            //reportLayout.AppendLine(string.Format("Sales Order : {0} = {1}", RoundDecimal(AmountDeliveryOrderCustom), CountDeliveryOrderCustom));
            //end
            reportLayout.AppendLine(string.Format("Total Customer : {0} = {1}", RoundDecimal(custAmountTotal), custCountTotal));

            reportLayout.AppendLine();
            decimal avgBasket;
            if (custCountTotal != 0)
            {
                  avgBasket = Math.Round((decimal)custAmountTotal, 0, MidpointRounding.AwayFromZero) / custCountTotal;

            }
            else
            {
                  avgBasket = 0;
            }

            reportLayout.AppendLine(string.Format("Avg. Basket : {0} / Orang", RoundDecimal(avgBasket)));
            //End add and comment standard NEC

            // Tender totals
            // Begin add and comment standard NEC
            //reportLayout.AppendReportLine(7045);
            //reportLayout.AppendReportLine(7047, RoundDecimal(batch.TenderedTotal));
            //reportLayout.AppendReportLine(7048, RoundDecimal(batch.ChangeTotal));
            //reportLayout.AppendReportLine(7069, RoundDecimal(batch.StartingAmountTotal));
            //reportLayout.AppendReportLine(7049, RoundDecimal(batch.FloatEntryAmountTotal));
            //reportLayout.AppendReportLine(7050, RoundDecimal(batch.RemoveTenderAmountTotal));
            //reportLayout.AppendReportLine(7051, RoundDecimal(batch.BankDropTotal));
            //reportLayout.AppendReportLine(7052, RoundDecimal(batch.SafeDropTotal));
            //reportLayout.AppendReportLine(7053, RoundDecimal(batch.DeclareTenderAmountTotal));

            bool amountShort = batch.OverShortTotal < 0;

            //reportLayout.AppendReportLine(amountShort ? 7055 : 7054, RoundDecimal(amountShort ? decimal.Negate(batch.OverShortTotal) : batch.OverShortTotal));
            reportLayout.AppendLine();


            //add by Yonathan to include the Cust Order for today 30082024
            string returnString;
            ReadOnlyCollection<object> containerArray;
            
            string fromDate = batch.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"); // "29/08/2024 00:00:00";
            string fromDateUtc = "";
            string toDateUtc = "";
            string toDate = reportType == ReportType.ZReport ? batch.CloseDateTime.ToString("yyyy-MM-dd HH:mm:ss") :  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //"29/08/2024 16:00:00";
            string itemGroupLines = "";
            decimal totalAmount, totalSales;
            string returnValue = "false";
            //containerArray = EOD.InternalApplication.TransactionServices.InvokeExtension("getSalesOrderSummary", "JKT", "WH_JDELIMA", fromDate, toDate);
            
            string salesOrderParam = "";
            // Example datetime
            DateTime batchStartDateTime = batch.StartDateTime; // Use your batch.StartDateTime here
            DateTime batchToDateTime = reportType == ReportType.ZReport ? batch.CloseDateTime : DateTime.Now; //"29/08/2024 16:00:00";
            // Convert DateTime to DateTimeOffset to get the local timezone offset
            DateTimeOffset fromDatelocalDateTimeOffset = new DateTimeOffset(batchStartDateTime, TimeZoneInfo.Local.GetUtcOffset(batchStartDateTime));
            DateTimeOffset toDateLocalDateTimeOffset = new DateTimeOffset(batchToDateTime, TimeZoneInfo.Local.GetUtcOffset(batchToDateTime));
            // Subtract the offset to get the UTC time
            DateTime fromUtcDateTime = fromDatelocalDateTimeOffset.UtcDateTime;
            DateTime toUtcDateTime = toDateLocalDateTimeOffset.UtcDateTime;
            fromDateUtc = fromUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); //add InvariantCulture for global datetime format - yonathan 14102024 
            toDateUtc = toUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); //add InvariantCulture for global datetime format - yonathan 14102024
            // Format the UTC datetime to a string

            salesOrderParam = getCustOrderTransaction(fromDateUtc, toDateUtc);
            //"SO/24/0000000034;SO/24/0000000033;SO/24/0000000032;SO/24/0000000031";
            containerArray = EOD.InternalApplication.TransactionServices.InvokeExtension("getInvoiceSalesOrder", salesOrderParam);
            returnString = containerArray[3].ToString();
            returnValue = containerArray[1].ToString();


            totalAmount = 0;
            totalSales = 0;
            if (containerArray[1].ToString() != "False")
            {

                reportLayout.AppendLine("-------------------------------------------------------");
                reportLayout.AppendLine("POS Customer Order Summary");
                reportLayout.AppendLine("-------------------------------------------------------");

                
                returnString = containerArray[3].ToString();
                // Load XML into XDocument
                XDocument xdoc = XDocument.Parse(returnString);
                var cultureInfo = new CultureInfo("id-ID");
                // Parse the XML and group by ItemId and ItemName
                var groupedData = xdoc.Descendants("CustInvoiceTrans")
                    .Where(e => e.Attribute("ItemLines") != null)
                    .Select(e => e.Attribute("ItemLines").Value.Split(';'))
                    .GroupBy(
                        fields => new { ItemId = fields[0], ItemName = fields[1] }, // Group by ItemId and ItemName
                        fields => new
                        {
                            Quantity = decimal.Parse(fields[2], NumberStyles.Number, cultureInfo),
                            LineAmount = decimal.Parse(fields[3], NumberStyles.Number, cultureInfo),
                            SalesId = fields[4]
                        }
                    )
                    .Select(group => new
                    {
                        group.Key.ItemId,
                        group.Key.ItemName,
                        TotalQty = group.Sum(x => x.Quantity),
                        TotalLineAmount = group.Sum(x => x.LineAmount),
                        //SalesIdCount = group.Select(x => x.SalesId).Distinct().Count()
                    });

                // Output the results
                foreach (var data in groupedData)
                {

                    reportLayout.AppendLine(string.Format("{0}        {1} - {2}", data.ItemId.ToString(), data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));

                    reportLayout.AppendLine(string.Format("{0}", data.ItemName.ToString()));
                    /*reportLayout.AppendLine(string.Format("{0} - {1}", data.ItemId.ToString(), data.ItemName.ToString()));

                    reportLayout.AppendLine(string.Format("{0} - {1}", data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));*/
                    totalAmount += data.TotalLineAmount;
                    //totalSales = data.SalesIdCount;
                }
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml(containerArray[3].ToString());

                

                // Extract distinct SalesId values from the XML
                var distinctSalesIds = xdoc.Descendants("CustInvoiceTrans")
                    .Where(e => e.Attribute("ItemLines") != null)
                    .Select(e => e.Attribute("ItemLines").Value.Split(';').Last()) // Extract the last part (SalesId)
                    .Distinct() // Get distinct SalesId values
                    .Count(); // Count the number of distinct SalesId values

                reportLayout.AppendLine("-------------------------------------------------------");
                //foreach (XmlNode eachNode in totalNodes)
                //{

                //    totalAmount = eachNode.Attributes["TotalAmount"].Value;
                //    totalAmount.Replace(".", ",");
                reportLayout.AppendLine(string.Format("Total Amount Sales Order : {0}", RoundDecimal(Convert.ToDecimal(totalAmount))));
                //   totalSales = eachNode.Attributes["TotalSales"].Value;
                reportLayout.AppendLine(string.Format("Total Sales Order : {0}", RoundDecimal(distinctSalesIds)));
                    //purchid = node.Attributes["PURCHID"].Value;
                    //itemid = node.Attributes["ITEMID"].Value;
                //}
               
               


            }
            //section for online order - Yonathan 19112024
            salesOrderParam = getOnlineOrderTransaction(fromDateUtc, toDateUtc);
            containerArray = EOD.InternalApplication.TransactionServices.InvokeExtension("getInvoiceSalesOrder", salesOrderParam);
            returnString = containerArray[3].ToString();
            returnValue = containerArray[1].ToString();


            totalAmount = 0;
            totalSales = 0;
            if (containerArray[1].ToString() != "False")
            {
                reportLayout.AppendLine("");
                reportLayout.AppendLine("-------------------------------------------------------");
                reportLayout.AppendLine("Online Order Summary");
                reportLayout.AppendLine("-------------------------------------------------------");
                returnString = containerArray[3].ToString();
                // Load XML into XDocument
                XDocument xdoc = XDocument.Parse(returnString);
                var cultureInfo = new CultureInfo("id-ID");
                // Parse the XML and group by ItemId and ItemName
                var groupedData = xdoc.Descendants("CustInvoiceTrans")
                    .Where(e => e.Attribute("ItemLines") != null)
                    .Select(e => e.Attribute("ItemLines").Value.Split(';'))
                    .GroupBy(
                        fields => new { ItemId = fields[0], ItemName = fields[1] }, // Group by ItemId and ItemName
                        fields => new
                        {
                            Quantity = decimal.Parse(fields[2], NumberStyles.Number, cultureInfo),
                            LineAmount = decimal.Parse(fields[3], NumberStyles.Number, cultureInfo),
                            SalesId = fields[4]
                        }
                    )
                    .Select(group => new
                    {
                        group.Key.ItemId,
                        group.Key.ItemName,
                        TotalQty = group.Sum(x => x.Quantity),
                        TotalLineAmount = group.Sum(x => x.LineAmount),
                        //SalesIdCount = group.Select(x => x.SalesId).Distinct().Count()
                    });

                // Output the results
                foreach (var data in groupedData)
                {
                    reportLayout.AppendLine(string.Format("{0}        {1} - {2}", data.ItemId.ToString(), data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));

                    reportLayout.AppendLine(string.Format("{0}", data.ItemName.ToString()));
                    /*reportLayout.AppendLine(string.Format("{0} - {1}", data.ItemId.ToString(), data.ItemName.ToString()));

                    reportLayout.AppendLine(string.Format("{0} - {1}", data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));*/
                    /*reportLayout.AppendLine(string.Format("{0} - {1}", data.ItemId.ToString(), data.ItemName.ToString()));

                    reportLayout.AppendLine(string.Format("{0} - {1}", data.TotalQty, RoundDecimal(Convert.ToDecimal(data.TotalLineAmount))));*/
                    totalAmount += data.TotalLineAmount;
                    //totalSales = data.SalesIdCount;
                }
                XmlDocument xmlDoc = new XmlDocument();
              
                var distinctSalesIds = xdoc.Descendants("CustInvoiceTrans")
                    .Where(e => e.Attribute("ItemLines") != null)
                    .Select(e => e.Attribute("ItemLines").Value.Split(';').Last()) // Extract the last part (SalesId)
                    .Distinct() // Get distinct SalesId values
                    .Count(); // Count the number of distinct SalesId values

                reportLayout.AppendLine("-------------------------------------------------------");
             
                reportLayout.AppendLine(string.Format("Total Amount Sales Order : {0}", RoundDecimal(Convert.ToDecimal(totalAmount))));
              
                reportLayout.AppendLine(string.Format("Total Sales Order : {0}", RoundDecimal(distinctSalesIds)));
                 
            }
            

            

            //end

            // End add and comment standard NEC
            // Income/Expense
            // Begin comment standard NEC
           /* if (batch.AccountLines.Count > 0)
            {
                reportLayout.AppendReportLine(7030);
                foreach (BatchAccountLine accountLine in batch.AccountLines.OrderBy(a => a.AccountType))
                {
                    int typeResourceId = 0;

                    switch (accountLine.AccountType)
                    {
                        case IncomeExpenseAccountType.Income:
                            typeResourceId = 7031;
                            break;

                        case IncomeExpenseAccountType.Expense:
                            typeResourceId = 7032;
                            break;

                        default:
                            String message = string.Format("Unsupported account Type '{0}'.", accountLine.AccountType);
                            NetTracer.Error(message);
                            throw new NotSupportedException(message);
                    }

                    reportLayout.AppendReportLine(string.Format(typeFormat, accountLine.AccountNumber, ApplicationLocalizer.Language.Translate(typeResourceId)),
                            RoundDecimal(accountLine.Amount));
                }

                reportLayout.AppendLine();
            }
            */
            // NEC disable Tender
            // Tenders
            /*if (reportType == ReportType.ZReport && batch.TenderLines.Count > 0)
            {
                reportLayout.AppendReportLine(7046);
                foreach (BatchTenderLine tenderLine in batch.TenderLines.OrderBy(t => t.TenderName))
                {
                    string formatedTenderName = tenderLine.TenderName;

                    if (ApplicationSettings.Terminal.StoreCurrency != tenderLine.Currency)
                    {
                        formatedTenderName = string.Format(currencyFormat, tenderLine.TenderName, tenderLine.Currency);
                    }

                    reportLayout.AppendReportLine(string.Format(typeFormat, formatedTenderName, ApplicationLocalizer.Language.Translate(7049)),
                            RoundDecimal(tenderLine.AddToTenderAmountCur, tenderLine.Currency));
                    reportLayout.AppendReportLine(string.Format(typeFormat, formatedTenderName, ApplicationLocalizer.Language.Translate(7056)),
                            RoundDecimal(tenderLine.ShiftAmountCur, tenderLine.Currency));
                    reportLayout.AppendReportLine(string.Format(typeFormat, formatedTenderName, ApplicationLocalizer.Language.Translate(7050)),
                            RoundDecimal(tenderLine.RemoveFromTenderAmountCur, tenderLine.Currency));

                    if (tenderLine.CountingRequired)
                    {
                        reportLayout.AppendReportLine(string.Format(typeFormat, formatedTenderName, ApplicationLocalizer.Language.Translate(7053)),
                                RoundDecimal(tenderLine.DeclareTenderAmountCur, tenderLine.Currency));

                        amountShort = tenderLine.OverShortAmountCur < 0;

                        reportLayout.AppendReportLine(string.Format(typeFormat, formatedTenderName, ApplicationLocalizer.Language.Translate(amountShort ? 7055 : 7054)),
                            RoundDecimal(amountShort ? decimal.Negate(tenderLine.OverShortAmountCur) : tenderLine.OverShortAmountCur, tenderLine.Currency));
                    }

                    reportLayout.AppendReportLine(string.Format(typeFormat,
                        formatedTenderName, ApplicationLocalizer.Language.Translate(7057)), tenderLine.Count);

                    reportLayout.AppendLine();
                }
            }
            */
            if (((object) EOD.InternalApplication.Services.Printing) is IPrintingV2)
            {   // Print to the default printer
                EOD.InternalApplication.Services.Printing.PrintDefault(true, reportLayout.ToString());
            }
            else
            {   // Legacy support - direct print to printer #1
                NetTracer.Warning("BatchPrinting.Print - Printing service does not support default printer.  Using printer #1");
                EOD.InternalApplication.Services.Peripherals.Printer.PrintReceipt(reportLayout.ToString());
            }

            if (EOD.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled())
            {
                EOD.InternalApplication.Services.Peripherals.FiscalPrinter.PrintReceipt(reportLayout.ToString());
            }
        }

        private static string getCustOrderTransaction(string fromDate, string toDate)
        {
            SqlConnection localConnection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string salesIdMulti = "" ;
            string salesIds = "";
             
            decimal totalTunai = 0;
            try
            {
                string queryData = @"SELECT SALESORDERID, MODIFIEDDATETIME, TRANSACTIONID FROM ax.RETAILTRANSACTIONTABLE WHERE 
		                            SALESORDERID !='' 
		                            AND MODIFIEDDATETIME BETWEEN  '" + fromDate + "'  AND '" + toDate + "' ORDER BY SALESORDERID DESC"; 
                
                using (SqlCommand cmd = new SqlCommand(queryData, localConnection))
                {


                    if (localConnection.State != ConnectionState.Open)
                    {
                        localConnection.Open();
                    }

                    //int flagFirstItem = 0;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder salesIdBuilder = new StringBuilder();

                        while (reader.Read())
                        {
                            // Retrieve SALESORDERID from the reader
                              salesIdMulti = reader["SALESORDERID"].ToString();

                            // Append salesOrderId followed by a semicolon
                            if (salesIdBuilder.Length > 0)
                            {
                                salesIdBuilder.Append(";");
                            }
                            salesIdBuilder.Append(salesIdMulti);
                        }

                        // Convert the StringBuilder to a string
                          salesIds = salesIdBuilder.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (localConnection.State != ConnectionState.Closed)
                {
                    localConnection.Close();
                }
            }

            return salesIds;
            //throw new NotImplementedException();
        }

        private static string getOnlineOrderTransaction(string fromDate, string toDate)
        {
            SqlConnection localConnection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string salesIdMulti = "";
            string salesIds = "";
            
            decimal totalTunai = 0;
            try
            {
                string queryData = @"SELECT RETAILSTOREID,
                                            SALESID,
                                            STAFFID,
                                            TRANSDATETIME,
                                            DATAAREAID FROM ax.CPPOSONLINEORDER WHERE 
		                            SALESID !='' 
		                            AND TRANSDATETIME BETWEEN  '" + fromDate + "'  AND '" + toDate + "' ORDER BY SALESID DESC";
                //DATEADD(HOUR, -(DATEPART(TZOFFSET, SYSDATETIMEOFFSET()) / 60), SYSDATETIME()) 
                //C.BANK != 'QRISBCA'
                using (SqlCommand cmd = new SqlCommand(queryData, localConnection))
                {


                    if (localConnection.State != ConnectionState.Open)
                    {
                        localConnection.Open();
                    }

                    //int flagFirstItem = 0;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder salesIdBuilder = new StringBuilder();

                        while (reader.Read())
                        {
                            // Retrieve SALESORDERID from the reader
                            salesIdMulti = reader["SALESID"].ToString();

                            // Append salesOrderId followed by a semicolon
                            if (salesIdBuilder.Length > 0)
                            {
                                salesIdBuilder.Append(";");
                            }
                            salesIdBuilder.Append(salesIdMulti);
                        }

                        // Convert the StringBuilder to a string
                        salesIds = salesIdBuilder.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (localConnection.State != ConnectionState.Closed)
                {
                    localConnection.Close();
                }
            }

            return salesIds;
            //throw new NotImplementedException();
        }

        //private string getSalesOrderSummary()
        //{
            

        //    return containerArray[3].ToString();
        //}

        /// <summary>
        /// Prints tax lines to the X or Z report.
        /// </summary>
        /// <param name="batch">Calculated batch object</param>
        /// <param name="reportLayout">An instance of the <see cref="StringBuilder"/> with the report output</param>
        private static void PrintTaxLines(Batch batch, StringBuilder reportLayout)
        {
            if (batch.TaxLines.Count > 0)
            {
                reportLayout.AppendReportLine(7081);
                foreach (var taxLine in batch.TaxLines.OrderBy(t => t.TaxCode))
                {
                    reportLayout.AppendReportLine(ApplicationLocalizer.Language.Translate(7082, taxLine.TaxCode), RoundDecimal(taxLine.Amount));
                }
            }
        }

        /// <summary>
        /// Print a shift staging report.
        /// </summary>
        /// <param name="batchStaging"></param>
        public static void Print(this IPosBatchStaging batchStaging)
        {
            StringBuilder reportLayout = new StringBuilder(1000);
            int headerStringId = 0;
            int statusStringId = 0;

            switch (batchStaging.Status)
            {
                case PosBatchStatus.Suspended:
                    headerStringId = 7063;
                    statusStringId = 7067;
                    break;

                case PosBatchStatus.BlindClosed:
                    headerStringId = 7064;
                    statusStringId = 7068;
                    break;

                default:
                    NetTracer.Error("Unsupported batchStaging status {0}", batchStaging.Status);
                    throw new NotSupportedException();
            }

            // Header
            reportLayout.AppendLine(ApplicationLocalizer.Language.Translate(headerStringId));
            reportLayout.AppendLine();

            // Current information
            reportLayout.AppendReportLine(7006, DateTime.Now.ToShortDateString());
            reportLayout.AppendReportLine(7007, DateTime.Now.ToShortTimeString());
            reportLayout.AppendReportLine(7003, ApplicationSettings.Terminal.TerminalId);
            reportLayout.AppendLine();

            // Content
            reportLayout.AppendReportLine(7065, batchStaging.TerminalId);
            reportLayout.AppendReportLine(7005, batchStaging.BatchId);
            reportLayout.AppendReportLine(7066, ApplicationLocalizer.Language.Translate(statusStringId));
            reportLayout.AppendReportLine(7008, batchStaging.StartDateTime.ToShortDateString());
            reportLayout.AppendReportLine(7009, batchStaging.StartDateTime.ToShortTimeString());
            reportLayout.AppendReportLine(7004, batchStaging.StaffId);

            EOD.InternalApplication.Services.Peripherals.Printer.PrintReceipt(reportLayout.ToString());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Prepare report header.
        /// </summary>
        /// <param name="reportLayout"></param>
        /// <param name="reportType"></param>
        private static void PrepareHeader(this StringBuilder reportLayout, Batch batch, ReportType reportType)
        {
            string openBy = "";
            string closeBy = "";
            string setorBy = "";
            List<string> cashierOnDutyList = new List<string>();

            reportLayout.AppendLine(singleLine);
            switch (reportType)
            {
                case ReportType.XReport:
                    reportLayout.AppendReportLine(7000);
                    break;

                case ReportType.ZReport:
                    reportLayout.AppendReportLine(7001);
                    break;

                default:
                    String message = string.Format("Unsupported Report Type '{0}'.", reportType);
                    NetTracer.Error(message);
                    throw new NotSupportedException(message);
            }

            reportLayout.AppendReportHeaderLine(7002, ApplicationSettings.Terminal.StoreId, true);
            reportLayout.AppendLine();
            reportLayout.AppendReportHeaderLine(7006, DateTime.Now.ToShortDateString(), true);
            reportLayout.AppendLine();
            reportLayout.AppendReportHeaderLine(7003, ApplicationSettings.Terminal.TerminalId, true);
            reportLayout.AppendLine();
            reportLayout.AppendReportHeaderLine(7007, DateTime.Now.ToShortTimeString(), true);
            reportLayout.AppendLine();
            //reportLayout.AppendReportHeaderLine(7004, ApplicationSettings.Terminal.TerminalOperator.OperatorId, true);
            if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
            {
                reportLayout.AppendLine();
                reportLayout.AppendReportHeaderLine(103210, ApplicationSettings.Terminal.StoreName, true);
                reportLayout.AppendLine();
                reportLayout.AppendReportHeaderLine(7070, ApplicationSettings.Terminal.TaxIdNumber, true);
            }
            //reportLayout.AppendLine();
            //reportLayout.AppendLine();
            reportLayout.AppendReportHeaderLine(7005, ApplicationLocalizer.Language.Translate(206, batch.TerminalId, batch.BatchId), true);
            reportLayout.AppendLine();
            //custom by Yonathan 31102024
            if (reportType == ReportType.XReport)
            {
                openBy = batch.StaffId;
            }
            else
            {
                getOpenCloseBy(batch, out openBy, out closeBy, out setorBy);
            }
            
            getCashiersOnDuty(batch, reportType, out cashierOnDutyList);
           
            reportLayout.AppendLine("Printed By    : " + operatorName(ApplicationSettings.Terminal.TerminalOperator.OperatorId));
            reportLayout.AppendLine("Print Date    : " + DateTime.UtcNow.ToLocalTime().ToString());
            reportLayout.AppendLine("OpenShift By  : " );
            reportLayout.AppendLine( openBy + "-" + operatorName(openBy) );//, true);
            if (reportType == ReportType.ZReport)
            {
                
                reportLayout.AppendLine("CloseShift By : " );//, true);
                reportLayout.AppendLine(closeBy + "-" + operatorName(closeBy));//, true);

                reportLayout.AppendLine("Setor By : ");
                reportLayout.AppendLine(setorBy + "-" + operatorName(setorBy));

                
                

            }

            reportLayout.AppendLine("Cashiers on Duty : ");//, true);
            //foreach
            foreach (var cashier in cashierOnDutyList)
            {
                reportLayout.AppendLine(cashier + "-" + operatorName(cashier));
            }
            //end
            reportLayout.AppendLine();
            reportLayout.AppendReportHeaderLine(7008, batch.StartDateTime.ToShortDateString(), true);

            if (reportType == ReportType.ZReport)
            {
                reportLayout.AppendLine();
                reportLayout.AppendReportHeaderLine(7010, batch.CloseDateTime.ToShortDateString(), true);
                //reportLayout.AppendReportHeaderLine(7010, batch.CloseDateTime.ToShortDateString(), false);
            }
            else
            {
                reportLayout.AppendLine();
            }
            reportLayout.AppendLine();
            reportLayout.AppendReportHeaderLine(7009, batch.StartDateTime.ToShortTimeString(), true);
            if (reportType == ReportType.ZReport)
            {
                reportLayout.AppendLine();
                reportLayout.AppendReportHeaderLine(7011, batch.CloseDateTime.ToShortTimeString(), true);
                //reportLayout.AppendReportHeaderLine(7011, batch.CloseDateTime.ToShortTimeString(), false);
            }
            else
            {
                reportLayout.AppendLine();
            }

            reportLayout.AppendLine();
        }

        private static void getCashiersOnDuty(Batch batch, ReportType reportType, out List<string> cashierOnDutyList)
        {
            cashierOnDutyList = new List<string>();
            HashSet<string> uniqueCashiers = new HashSet<string>(); // To track unique cashiers
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string query = @"SELECT STAFF, CREATEDDATETIME, RECEIPTID, STORE 
                         FROM AX.RETAILTRANSACTIONTABLE 
                         WHERE RECEIPTID != '' 
                         AND  STORE = @StoreName
                         AND CREATEDDATETIME BETWEEN @StartDate AND @CloseDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@StartDate", batch.StartDateTime.AddHours(-7));
                    if (reportType == ReportType.ZReport)
                    {
                        cmd.Parameters.AddWithValue("@CloseDate", batch.CloseDateTime.AddHours(-7));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CloseDate", DateTime.Now);
                    }
                    cmd.Parameters.AddWithValue("@StoreName", ApplicationSettings.Terminal.StoreId.ToString());

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string staffId = reader["STAFF"].ToString();
                            if (uniqueCashiers.Add(staffId)) // Adds and checks for uniqueness
                            {
                                cashierOnDutyList.Add(staffId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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


        private static string operatorName(string openBy)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string opName = "";
            try
            {
            string queryString = @"SELECT WORKER.PERSONNELNUMBER, DIRPARTY.NAME FROM AX.HCMWORKER WORKER
                                  LEFT JOIN AX.DIRPARTYTABLE DIRPARTY
                                  ON WORKER.PERSON = DIRPARTY.RECID
                                  WHERE PERSONNELNUMBER = @STAFFID";
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.Add(new SqlParameter("@STAFFID", openBy));
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        opName = reader[1].ToString();
                       

                    }
                }
            }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return opName;

        }

        private static void getOpenCloseBy(Batch _batch, out string openBy, out string closeBy, out string setorBy)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            openBy = "";
            closeBy = "";
            setorBy = "";
            try
            {
               
                string queryString = @"SELECT BATCHID, OPENBY, CLOSEBY, SETORBY
                                      FROM  [ax].[CPRETAILPOSBATCHTABLEEXTEND]
                                      where BATCHID = @BATCHID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add(new SqlParameter("@BATCHID", _batch.BatchId));
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            openBy = reader[1].ToString();
                            closeBy = reader[2].ToString();
                            setorBy = reader[3].ToString();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
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

        /// <summary>
        /// Append Report header line.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="titleResourceId">Resouce Id of Title part of the line</param>
        /// <param name="value">Value part of the line</param>
        /// <param name="firstPart">True for first part of header, false for second.</param>
        private static void AppendReportHeaderLine(this StringBuilder stringBuilder, int titleResourceId, string value, bool firstPart)
        {
            int partWidth = (paperWidth / 2);
            int titleWidth = (int)(partWidth * 0.5);
            int valueWidth = (int)(partWidth * 0.4);
            string title = ApplicationLocalizer.Language.Translate(titleResourceId);
            string line = string.Format(lineFormat, title.PadRight(titleWidth), value.PadLeft(valueWidth));

            if (firstPart)
            {
                stringBuilder.Append(line.PadRight(partWidth));
            }
            else
            {
                stringBuilder.AppendLine(line.PadLeft(partWidth));
            }
        }

        /// <summary>
        /// Append report line.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="titleStringId">Resource id of title string</param>
        /// <param name="value">Value of tender item</param>
        private static void AppendReportLine(this StringBuilder stringBuilder, int titleStringId, object value)
        {
            string title = ApplicationLocalizer.Language.Translate(titleStringId);

            stringBuilder.AppendReportLine(title, value);
        }

        /// <summary>
        /// Append report line.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="title">Title string</param>
        /// <param name="value">Value of tender item</param>
        private static void AppendReportLine(this StringBuilder stringBuilder, string title, object value)
        {
            //EDIT BY ERWIN
            stringBuilder.AppendLine(string.Format(lineFormat, title, value.ToString().PadLeft(paperWidth - title.Length - 2)));
            //stringBuilder.AppendLine(string.Format("{0}{1}", title.PadRight(25, 'a'), value.ToString().PadLeft(10, 'b')));
            //END EDIT BY ERWIN
        }

        /// <summary>
        /// Append a report title.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="titleResourceId">Resource Id</param>
        private static void AppendReportLine(this StringBuilder stringBuilder, int titleResourceId)
        {
            stringBuilder.AppendLine(ApplicationLocalizer.Language.Translate(titleResourceId));
            stringBuilder.AppendLine(singleLine);
        }

        /// <summary>
        /// Round amount to decimal using store currency code.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string RoundDecimal(decimal value)
        {
            return RoundDecimal(value, ApplicationSettings.Terminal.StoreCurrency);
        }

        /// <summary>
        /// Rount amount to decimal using given curreny code.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        private static string RoundDecimal(decimal value, string currency)
        {
            return EOD.InternalApplication.Services.Rounding.RoundForDisplay(value, currency, true, false);
            
        }

        #endregion


        #region custom method
        

        

        

        #endregion  

    }

    

    public class Cashout
    {
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
        public static string getFolderPath(string ProcessingDirectory, string typeFolder)
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
        public class parmRequestCashOut
        {

            public string DATAAREAID { get; set; }
            public string STOREID { get; set; }
            public string MODULETYPE { get; set; }
            public string FROMDATE { get; set; }
            public string TODATE { get; set; }
        }

        public class parmResponseCashOut
        {
            public bool error { get; set; }
            public int message_code { get; set; }
            public string message_description { get; set; }
            public string response_data { get; set; }
        }

        public string getAmountCashout(string fromDateTime, string toDateTime)
        {
            //string amountCashout = "";
            string storeId = ApplicationSettings.Terminal.StoreId.ToString();
            //string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
            var url = "AXAIF";
            var url2 = "api";
            string functionName = "GetAmountCashOut";
            //url = getFolderPath(PathDirectory, "urlAXAIF") + "AXAIF/AIF/getStockOnHand";
            //url2 = getFolderPath(PathDirectory, "urlAPI") + "api/transaction/getAmount";
            APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
            //url = APIClass.getURLAPIByFuncName("GetStockAXPFMPOC");
            url2 = APIClass.getURLAPIByFuncName(functionName);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            if (url2 == "")
            {
                throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
            }
            else
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url2);
                string result = "";
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization", "PFM");

                //initialize the required parameter before post to API

                //parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
                //parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
                //parmRequestCashOut.MODULETYPE = "CO";
                //parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
                var pack = new parmRequestCashOut()
                {
                    DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID,
                    STOREID = ApplicationSettings.Terminal.StoreId.ToString(), //"JCIBBR1", //ApplicationSettings.Terminal.StoreId.ToString(),
                    MODULETYPE = "CO",
                    FROMDATE = fromDateTime, //DateTime.Now.ToString("yyyy-MM-dd"),
                    TODATE = toDateTime //DateTime.Now.ToString("yyyy-MM-dd")
                };




                try
                {
                    var data = MyJsonConverter.Serialize(pack);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        
                        
                    }
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    return "0";
                    throw;

                }

                parmResponseCashOut responseCashout = MyJsonConverter.Deserialize<parmResponseCashOut>(result);


                return responseCashout.response_data.ToString();
            }
        }
            
    }
    
}
