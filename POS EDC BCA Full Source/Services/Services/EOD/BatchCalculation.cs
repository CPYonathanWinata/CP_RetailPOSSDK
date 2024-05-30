/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using TransactionStatus = LSRetailPosis.Transaction.PosTransaction.TransactionStatus;
using TypeOfTransaction = LSRetailPosis.Transaction.PosTransaction.TypeOfTransaction;
using LSRetailPosis.Transaction;

namespace Microsoft.Dynamics.Retail.Pos.EOD
{
    /// <summary>
    /// Class warps Batch calculation functions.
    /// </summary>
    static class BatchCalculation
    {

        #region Fields

        private static TenderDeclarationCalculationType tenderDeclCalculationType;

        #endregion

        #region Enums
        /// <summary>
        /// Specifies the type of Tender Declaration Calculation method to use.
        /// </summary>
        /// <remarks>Linked to AX Base enum: RetailTenderDeclarationCalculationBas.</remarks>
        public enum TenderDeclarationCalculationType
        {
            /// <summary>
            /// Last = 0
            /// </summary>
            Last = 0,

            /// <summary>
            /// Sum = 1
            /// </summary>
            Sum = 1
        }

        #endregion

        #region Queries

        private const string sqlHeaderJoinClause = "INNER JOIN RETAILTRANSACTIONTABLE AS H " +
            "ON H.TRANSACTIONID = L.TRANSACTIONID " +
            "AND H.STORE = L.STORE " +
            "AND H.TERMINAL = L.TERMINALID " +
            "AND H.DATAAREAID = L.DATAAREAID ";

        private const string sqlHeaderJoinClause1 = "INNER JOIN RETAILTRANSACTIONTABLE AS H " +
            "ON H.TRANSACTIONID = L.TRANSACTIONID " +
            "AND H.STORE = L.STORE " +
            "AND H.TERMINAL = L.TERMINAL " +
            "AND H.DATAAREAID = L.DATAAREAID ";

        private const string sqlHeaderJoinClause2 = "INNER JOIN RETAILTRANSACTIONTABLE AS H " +
            "ON H.TRANSACTIONID = L.TRANSACTIONID " +
            "AND H.STORE = L.STOREID " +
            "AND H.TERMINAL = L.TERMINALID " +
            "AND H.DATAAREAID = L.DATAAREAID ";

        private const string sqlStoreTenderJoinClause = "INNER JOIN RETAILSTORETENDERTYPETABLE AS T ON T.TENDERTYPEID = L.TENDERTYPE " +
            "AND T.DATAAREAID = L.DATAAREAID " +
            "JOIN RETAILSTORETABLE AS ST " +
            "ON ST.RECID = T.CHANNEL " +
            "AND ST.STORENUMBER = L.STORE ";

        private const string sqlWhereBatchClause = "WHERE H.STORE = @STOREID " +
            "AND H.BATCHTERMINALID = @BATCHTERMINALID " +
            "AND H.DATAAREAID = @DATAAREAID " +
            "AND H.BATCHID = @BATCHID ";

        private const string sqlSalesTotal = "SELECT -(SUM(L.NETAMOUNT) + CASE WHEN @TAXINCLUSIVE=1 THEN SUM(L.TAXAMOUNT) ELSE 0 END) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND QTY < 0 " +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS ";

        private const string sqlReturnsTotal = "SELECT SUM(L.NETAMOUNT) + CASE WHEN @TAXINCLUSIVE=1 THEN SUM(L.TAXAMOUNT) ELSE 0 END " +
            "FROM RETAILTRANSACTIONSALESTRANS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND QTY > 0 " +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS ";

        private const string sqlTaxTotal = "SELECT SUM(TAXAMOUNT) AS TAXTOTAL " +
            "FROM RETAILTRANSACTIONSALESTRANS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS ";

        private const string sqlDiscountTotal = "SELECT SUM(L.DISCAMOUNT) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS";

        private const string sqlCustomersCount = "SELECT COUNT(CUSTACCOUNT) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            "INNER JOIN RETAILSTORETABLE AS S ON H.STORE = S.STORENUMBER " +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND CUSTACCOUNT <> ''";

        private const string sqlPaidToAccountTotal = "SELECT SUM(AMOUNTTENDERED) " +
            "FROM RETAILTRANSACTIONPAYMENTTRANS AS L " +
            sqlHeaderJoinClause1 +
            sqlWhereBatchClause +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
            "AND [TYPE] = @TRANSACTIONTYPE";

        private const string sqlSuspendedTransactonCount = "SELECT COUNT(*) " +
            "FROM SALESTRANSACTION " +
            "WHERE CHANNELID = @CHANNELID " +
            "AND TERMINALID = @TERMINALID " +
            "AND ISSUSPENDED = 1 " +
            "AND DELETEDDATETIME IS NULL ";

        private const string sqlRoundingAmountTotal = "SELECT SUM(SALESPAYMENTDIFFERENCE) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause;

        private const string sqlTransactonsCount = "SELECT COUNT(*) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause;

        private const string sqlSalesAndReturnsCount = "select COUNT(DISTINCT L.TRANSACTIONID) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) ";

        private const string sqlSalesCount = "select COUNT(DISTINCT L.TRANSACTIONID) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND QTY < 0 " +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) ";

        private const string sqlVoidsCount = "SELECT COUNT(*) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause +
            "AND ENTRYSTATUS = @TRANSACTIONSTATUS ";

        private const string sqlTransactionTypesCount = "SELECT COUNT(*) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause +
            "AND [TYPE] = @TRANSACTIONTYPE";

        private const string sqlBatchStartDateTime = "SELECT MIN(CREATEDDATE) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause;

        private const string sqlIncomeExpense = "SELECT INCOMEEXEPENSEACCOUNT, ACCOUNTTYPE, SUM(AMOUNT) AS AMOUNT " +
            "FROM RETAILTRANSACTIONINCOMEEXPENSETRANS AS L " +
            sqlHeaderJoinClause1 +
            sqlWhereBatchClause +
            "AND ENTRYSTATUS = @TRANSACTIONSTATUS " +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
            "AND [TYPE] = @TRANSACTIONTYPE " +
            "GROUP BY ACCOUNTTYPE, INCOMEEXEPENSEACCOUNT " +
            "ORDER BY INCOMEEXEPENSEACCOUNT";

        private const string sqlTenderLines = "SELECT TENDERTYPE, T.NAME, L.CURRENCY, COUNTINGREQUIRED, " +
            "SUM(AMOUNTTENDERED) AS AMOUNT, SUM(AMOUNTCUR) AS AMOUNTCUR " +
            "FROM {0} AS L " + // tableName from tenderLinesQueries Dictionary
            sqlHeaderJoinClause1 +
            sqlStoreTenderJoinClause +
            sqlWhereBatchClause +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
            "AND (COUNTINGREQUIRED = 1 OR TAKENTOBANK = 1 OR TAKENTOSAFE = 1) " +
            "AND [TYPE] = (@TRANSACTIONTYPE) " +
            "GROUP BY TENDERTYPE, T.NAME, L.CURRENCY, COUNTINGREQUIRED";

        private const string sqlLastTenderDeclarationLine = "SELECT L.CURRENCY, L.TRANSDATE, L.TRANSTIME, TENDERTYPE, T.NAME, " +
           "COUNTINGREQUIRED, AMOUNTTENDERED AS AMOUNT, AMOUNTCUR AS AMOUNTCUR " +
           "FROM {0} AS L " + // tableName from tenderLinesQueries Dictionary
           sqlHeaderJoinClause1 +
           sqlStoreTenderJoinClause +
           sqlWhereBatchClause +
           "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
           "AND (COUNTINGREQUIRED = 1 OR TAKENTOBANK = 1 OR TAKENTOSAFE = 1) " +
           "AND [TYPE] = (@TRANSACTIONTYPE) " +
           "AND L.TRANSACTIONID IN (SELECT TOP 1 tRTT.TRANSACTIONID FROM RETAILTRANSACTIONTABLE AS tRTT WHERE tRTT.DATAAREAID = @DATAAREAID " +
                                                                    "AND tRTT.STORE = @STOREID " +
                                                                    "AND tRTT.[TYPE] = @TRANSACTIONTYPE " +
                                                                    "AND tRTT.BATCHTERMINALID = @BATCHTERMINALID " +
                                                                    "AND tRTT.BATCHID = @BATCHID " +
                                                                    "ORDER BY tRTT.TRANSDATE, tRTT.TRANSTIME DESC) ";

        private const string sqlTenderDeclarationCalculationType = "SELECT TENDERDECLARATIONCALCULATION " +
            "FROM RETAILSTORETABLE WHERE STORENUMBER = @STORE";

        private static Dictionary<TypeOfTransaction, string> tenderLinesQueries
                = new Dictionary<TypeOfTransaction, string>()
                {
                    {TypeOfTransaction.StartingAmount, "RETAILTRANSACTIONPAYMENTTRANS" },
                    {TypeOfTransaction.FloatEntry, "RETAILTRANSACTIONPAYMENTTRANS" },
                    {TypeOfTransaction.RemoveTender, "RETAILTRANSACTIONPAYMENTTRANS" },
                    {TypeOfTransaction.SafeDrop, "RETAILTRANSACTIONSAFETENDERTRANS" },
                    {TypeOfTransaction.BankDrop, "RETAILTRANSACTIONBANKEDTENDERTRANS"},
                    {TypeOfTransaction.TenderDeclaration, "RETAILTRANSACTIONTENDERDECLARATIONTRANS" }
                };

        private const string sqlTenderCalculatedLines = "SELECT TENDERTYPE, T.NAME, L.CURRENCY, COUNTINGREQUIRED, " +
            "CHANGELINE, SUM(AMOUNTTENDERED) AS AMOUNT, SUM(AMOUNTCUR) AS AMOUNTCUR, COUNT(*) AS COUNT " +
            "FROM RETAILTRANSACTIONPAYMENTTRANS AS L " +
            sqlHeaderJoinClause1 +
            sqlStoreTenderJoinClause +
            sqlWhereBatchClause +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
            "AND [TYPE] IN (@TYPE1, @TYPE2, @TYPE3, @TYPE4, @TYPE5, @TYPE6) " +
            "GROUP BY TENDERTYPE, T.NAME, L.CURRENCY, CHANGELINE, COUNTINGREQUIRED";

        private const string sqlVerifyTransactionTable = "SELECT TRANSACTIONID " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause +
            "AND TERMINAL = @TERMINALID ";

        private const string sqlReturnsCount = "select COUNT(DISTINCT L.TRANSACTIONID) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND QTY > 0";

        private const string sqlTrainingTransactonsCount = "SELECT COUNT(*) " +
            "FROM RETAILTRANSACTIONTABLE AS H " +
            sqlWhereBatchClause +
            "AND ENTRYSTATUS = @TRANSACTIONSTATUS " +
            "AND [TYPE] = @TRANSACTIONTYPE";

        private const string sqlSalesInTrainingModeTotal = "SELECT -(SUM(L.NETAMOUNT) + CASE WHEN @TAXINCLUSIVE=1 THEN SUM(L.TAXAMOUNT) ELSE 0 END) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ENTRYSTATUS = @TRANSACTIONSTATUS";

        private const string sqlProductJoinClause = "INNER JOIN [ax].[INVENTTABLE] AS IT " +
            "ON IT.ITEMID = L.ITEMID AND IT.ITEMTYPE = @ITEMTYPE ";

        private const string sqlSalesItemTypeQty = "SELECT SUM(L.QTY) FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlProductJoinClause +
            sqlWhereBatchClause +
            "AND H.ENTRYSTATUS = @TRANSACTIONSTATUS " +
            "AND L.TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
            "AND L.QTY < 0";

        private const string sqlTaxAmount = "SELECT SUM(L.AMOUNT) AS AMOUNT, L.TAXCODE AS TAXCODE FROM RETAILTRANSACTIONTAXTRANS AS L " +
            sqlHeaderJoinClause2 +
            sqlWhereBatchClause +
            "AND ENTRYSTATUS = @TRANSACTIONSTATUS " +
            "AND EXISTS (SELECT * FROM RETAILTRANSACTIONSALESTRANS AS ST WHERE ST.DATAAREAID = @DATAAREAID AND ST.STORE = H.STORE AND ST.TERMINALID = H.TERMINAL AND ST.TRANSACTIONID = H.TRANSACTIONID AND ST.LINENUM = L.SALELINENUM AND ST.TRANSACTIONSTATUS = @TRANSACTIONSTATUS) " +
            "GROUP BY TAXCODE " +
            "ORDER BY TAXCODE ASC";

        private const string sqlSuspendedTransactionsTotal = "SELECT SUM(AMOUNT) FROM crt.SALESTRANSACTION " +
            "WHERE TERMINALID = @TERMINALID " +
            "AND ISSUSPENDED = 1 AND DELETEDDATETIME IS NULL";

        private const string sqlCounterValue = "SELECT COUNTERVALUE FROM [dbo].[RETAILTERMINALCOUNTER] " +
            "WHERE STORE = @STOREID AND TERMINAL = @TERMINALID AND COUNTERTYPE = @COUNTERTYPE";

        private const string sqlReceiptCopiesCountAndSumAmount = "SELECT COUNT(*) AS COPIESCOUNT, CASE WHEN @TAXINCLUSIVE=1 THEN -SUM(GROSSAMOUNT) ELSE -SUM(NETAMOUNT) END AS COPIESAMOUNT " +
                "FROM RETAILTRANSACTIONTABLE AS H " +
                sqlWhereBatchClause +
                "AND EXISTS (SELECT * FROM ax.RETAILTRANSACTIONFISCALTRANS AS FT WHERE FT.TRANSACTIONID = H.TRANSACTIONID AND FT.STORE = H.STORE AND FT.TERMINAL = H.TERMINAL AND FT.DATAAREAID = @DATAAREAID AND FT.RECEIPTCOPY = 1)";

        private const string sqlReceiptsCount = "select COUNT(DISTINCT L.TRANSACTIONID) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) ";

        private const string sqlPriceOverrideTotal = "SELECT SUM((PRICE - ORIGINALPRICE) * QTY) " +
            "FROM RETAILTRANSACTIONSALESTRANS AS L " +
            sqlHeaderJoinClause +
            sqlWhereBatchClause +
            "AND ((ENTRYSTATUS = @TRANSACTIONSTATUS) OR (TYPE = @TYPECUSTOMERORDER AND ENTRYSTATUS = @TRANSACTIONSTATUSFORCUSTOMERORDER)) " +
            "AND TRANSACTIONSTATUS = @TRANSACTIONSTATUS " +
            "AND PRICECHANGE = 1";

        #endregion

        #region Methods

        /// <summary>
        /// Calculate a batch using Transactions.
        /// </summary>
        /// <param name="batch"></param>
        public static void Calculate(this Batch batch)
        {
            DbConnection connection = ApplicationSettings.Database.LocalConnection;

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (DbCommand dBCommand = new SqlCommand())
                {
                    dBCommand.Connection = connection;

                    CalculateHeader(dBCommand, batch);
                    CalculateIncomeExpenseAccounts(dBCommand, batch);
                    CalculateTender(dBCommand, batch);
                    if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
                    {
                        // Swedish legislation requires tax distribution information to be outputted in X and Z reports
                        CalculateTax(dBCommand, batch);
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Verify if offline transactions has been uploaded to Store Db.
        /// </summary>
        /// <param name="posBatch"></param>
        /// <returns>True if all transactions has been uploaded, false otherwise.</returns>
        public static bool VerifyOfflineTransactions(this Batch batch)
        {
            bool result = true; // optimistic approach
            SqlConnection storeDbConnection = EOD.InternalApplication.Settings.Database.Connection; // This method is only called when we are online, so this will be store db.
            SqlConnection offlineConnection = EOD.InternalApplication.Settings.Database.OfflineConnection;

            // If offline db is not configured then no need to verify.
            if (offlineConnection != null)
            {
                try
                {
                    List<string> offlineTransactions = ReadTransactions(batch, offlineConnection);

                    if (offlineTransactions.Count > 0)
                    {
                        List<string> storeDbTransactions = ReadTransactions(batch, storeDbConnection);

                        if (offlineTransactions.Except(storeDbTransactions).Count() > 0)
                        {
                            // We have some missing transactions in store db.
                            EOD.InternalApplication.Services.Dialog.ShowMessage(51341);
                            result = false;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    NetTracer.Error(ex, "Offline db is not available or configured properly.");
                    EOD.InternalApplication.Services.Dialog.ShowMessage(51377);
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculate Header
        /// </summary>
        /// <param name="dBCommand"></param>
        /// <param name="batch"></param>
        private static void CalculateHeader(DbCommand dBCommand, Batch batch)
        {
            // Shift start date should be batch start date and should not be recalculated. 
            // dBCommand.Initialize(sqlBatchStartDateTime, batch);
            // batch.StartDateTime = DBUtil.ToDateTime(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlSalesTotal, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
            dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
            dBCommand.AddParameter("@TAXINCLUSIVE", ApplicationSettings.Terminal.TaxIncludedInPrice);
            batch.SalesTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlReturnsTotal, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
            dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
            dBCommand.AddParameter("@TAXINCLUSIVE", ApplicationSettings.Terminal.TaxIncludedInPrice);
            batch.ReturnsTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlTaxTotal, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
            dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
            batch.TaxTotal = Decimal.Negate(DBUtil.ToDecimal(dBCommand.ExecuteScalar()));

            dBCommand.Initialize(sqlDiscountTotal, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
            dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
            batch.DiscountTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlPaidToAccountTotal, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONTYPE", TypeOfTransaction.Payment);
            batch.PaidToAccountTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlRoundingAmountTotal, batch);
            batch.RoundedAmountTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

            if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
            {
                dBCommand.Initialize(sqlSalesCount, batch);
            }
            else
            {
                dBCommand.Initialize(sqlSalesAndReturnsCount, batch);
            }
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
            dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
            batch.SalesCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlVoidsCount, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Voided);
            batch.VoidsCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlCustomersCount, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
            dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
            batch.CustomersCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlTransactionTypesCount, batch);
            dBCommand.AddParameter("@TRANSACTIONTYPE", TypeOfTransaction.OpenDrawer);
            batch.NoSaleCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlTransactionTypesCount, batch);
            dBCommand.AddParameter("@TRANSACTIONTYPE", TypeOfTransaction.LogOn);
            batch.LogOnCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

            dBCommand.Initialize(sqlTransactonsCount, batch);
            batch.TransactionsCount = DBUtil.ToInt32(dBCommand.ExecuteScalar()) + 1;

            dBCommand.CommandText = sqlSuspendedTransactonCount;
            dBCommand.Parameters.Clear();
            dBCommand.AddParameter("@CHANNELID", ApplicationSettings.Terminal.StorePrimaryId);
            dBCommand.AddParameter("@TERMINALID", ApplicationSettings.Terminal.TerminalId);
            batch.SuspendedTransactionsCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

            #region Swedish specific fields
            if (LSRetailPosis.Settings.FunctionalityProfiles.Functions.CountryRegion == LSRetailPosis.Settings.FunctionalityProfiles.SupportedCountryRegion.SE)
            {
                if (ApplicationSettings.Terminal.TaxIncludedInPrice)
                {
                    batch.SalesTotalIncludingTax = batch.SalesTotal;
                    dBCommand.Initialize(sqlSalesTotal, batch);
                    dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                    dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
                    dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
                    dBCommand.AddParameter("@TAXINCLUSIVE", false);
                    batch.SalesTotalExlcudingTax = DBUtil.ToDecimal(dBCommand.ExecuteScalar());
                }
                else 
                {
                    batch.SalesTotalExlcudingTax = batch.SalesTotal;
                    dBCommand.Initialize(sqlSalesTotal, batch);
                    dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                    dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
                    dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
                    dBCommand.AddParameter("@TAXINCLUSIVE", true);
                    batch.SalesTotalIncludingTax = DBUtil.ToDecimal(dBCommand.ExecuteScalar());
                }

                dBCommand.Initialize(sqlReceiptsCount, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
                dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
                batch.ReceiptsCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

                dBCommand.Initialize(sqlReturnsCount, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
                dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
                batch.ReturnsCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

                dBCommand.Initialize(sqlSalesItemTypeQty, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                dBCommand.AddParameter("@ITEMTYPE", ItemTypes.Item);
                batch.GoodsSoldQty = -DBUtil.ToDecimal(dBCommand.ExecuteScalar());

                dBCommand.Initialize(sqlSalesItemTypeQty, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                dBCommand.AddParameter("@ITEMTYPE", ItemTypes.Service);
                batch.ServicesSoldQty = -DBUtil.ToDecimal(dBCommand.ExecuteScalar());

                dBCommand.Initialize(sqlReceiptCopiesCountAndSumAmount, batch);
                dBCommand.AddParameter("@TAXINCLUSIVE", ApplicationSettings.Terminal.TaxIncludedInPrice);
                using (var reader = dBCommand.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        batch.ReceiptCopiesCount = DBUtil.ToInt32(reader[0]);
                        batch.ReceiptCopiesTotal = DBUtil.ToDecimal(reader[1]);
                    }
                }

                dBCommand.Initialize(sqlSalesInTrainingModeTotal, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Training);
                dBCommand.AddParameter("@TAXINCLUSIVE", ApplicationSettings.Terminal.TaxIncludedInPrice);
                batch.TrainingTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

                dBCommand.Initialize(sqlTrainingTransactonsCount, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Training);
                dBCommand.AddParameter("@TRANSACTIONTYPE", TypeOfTransaction.Sales);
                batch.TrainingCount = DBUtil.ToInt32(dBCommand.ExecuteScalar());

                dBCommand.CommandText = sqlSuspendedTransactionsTotal;
                dBCommand.Parameters.Clear();
                dBCommand.AddParameter("@TERMINALID", batch.TerminalId);
                batch.SuspendedTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

                dBCommand.Initialize(sqlPriceOverrideTotal, batch);
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                dBCommand.AddParameter("@TRANSACTIONSTATUSFORCUSTOMERORDER", TransactionStatus.Posted);
                dBCommand.AddParameter("@TYPECUSTOMERORDER", TypeOfTransaction.CustomerOrder);
                batch.PriceOverrideTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

                #region Grand totals
                dBCommand.CommandText = sqlCounterValue;
                dBCommand.Parameters.Clear();
                dBCommand.AddParameter("@STOREID", batch.StoreId);
                dBCommand.AddParameter("@TERMINALID", batch.TerminalId);
                dBCommand.AddParameter("@COUNTERTYPE", (int)CounterType.TotalSalesNet);
                batch.SalesGrandTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());

                dBCommand.CommandText = sqlCounterValue;
                dBCommand.Parameters.Clear();
                dBCommand.AddParameter("@STOREID", batch.StoreId);
                dBCommand.AddParameter("@TERMINALID", batch.TerminalId);
                dBCommand.AddParameter("@COUNTERTYPE", (int)CounterType.TotalReturnsNet);
                batch.ReturnsGrandTotal = DBUtil.ToDecimal(dBCommand.ExecuteScalar());
                #endregion
            } 
            #endregion
        }

        /// <summary>
        /// Income/Expense Accounts
        /// </summary>
        /// <param name="dBCommand"></param>
        /// <param name="batch"></param>
        private static void CalculateIncomeExpenseAccounts(DbCommand dBCommand, Batch batch)
        {
            dBCommand.Initialize(sqlIncomeExpense, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TRANSACTIONTYPE", TypeOfTransaction.IncomeExpense);
            using (DbDataReader reader = dBCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    BatchAccountLine batchAccountLine = new BatchAccountLine();

                    batchAccountLine.AccountNumber = (string)reader["INCOMEEXEPENSEACCOUNT"];
                    batchAccountLine.AccountType = DBUtil.ToEnum<IncomeExpenseAccountType>(reader["ACCOUNTTYPE"]);
                    batchAccountLine.Amount = DBUtil.ToDecimal(reader["AMOUNT"]);

                    if (batchAccountLine.AccountType == IncomeExpenseAccountType.Income)
                        batchAccountLine.Amount = Decimal.Negate(batchAccountLine.Amount);

                    batch.AccountLines.Add(batchAccountLine);
                }
            }
        }

        /// <summary>
        /// Tender Lines
        /// </summary>
        /// <param name="dBCommand"></param>
        /// <param name="batch"></param>
        private static void CalculateTender(DbCommand dBCommand, Batch batch)
        {
            dBCommand.Initialize(sqlTenderDeclarationCalculationType);
            dBCommand.AddParameter("@STORE", ApplicationSettings.Terminal.StoreId);
            tenderDeclCalculationType = DBUtil.ToEnum<TenderDeclarationCalculationType>(dBCommand.ExecuteScalar());

            // Calculate Tender addition/removal
            foreach (KeyValuePair<TypeOfTransaction, string> query in tenderLinesQueries)
            {
                // Checking store level setting whether Tender Declaration Calculation Type is Last or Sum.
                if (query.Key.Equals(PosTransaction.TypeOfTransaction.TenderDeclaration) && tenderDeclCalculationType.Equals(TenderDeclarationCalculationType.Last))
                {
                    dBCommand.Initialize(string.Format(sqlLastTenderDeclarationLine, query.Value), batch);
                }
                else
                {
                    dBCommand.Initialize(string.Format(sqlTenderLines, query.Value), batch);
                }
                dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
                dBCommand.AddParameter("@TRANSACTIONTYPE", (int)query.Key);
                using (DbDataReader reader = dBCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BatchTenderLine batchTenderLine = batch.TenderLines.FindOrCreate(reader);
                        decimal amount = DBUtil.ToDecimal(reader["AMOUNT"]);
                        decimal amountCur = DBUtil.ToDecimal(reader["AMOUNTCUR"]);

                        batchTenderLine.CountingRequired = DBUtil.ToBool(reader["COUNTINGREQUIRED"]);

                        switch (query.Key)
                        {
                            case TypeOfTransaction.StartingAmount:
                                batchTenderLine.StartingAmount = amount;
                                batchTenderLine.StartingAmountCur = amountCur;
                                break;

                            case TypeOfTransaction.FloatEntry:
                                batchTenderLine.FloatEntryAmount = amount;
                                batchTenderLine.FloatEntryAmountCur = amountCur;
                                break;

                            case TypeOfTransaction.RemoveTender:
                                batchTenderLine.RemoveTenderAmount = decimal.Negate(amount);
                                batchTenderLine.RemoveTenderAmountCur = decimal.Negate(amountCur);
                                break;

                            case TypeOfTransaction.SafeDrop:
                                batchTenderLine.SafeDropAmount = amount;
                                batchTenderLine.SafeDropAmountCur = amountCur;
                                break;

                            case TypeOfTransaction.BankDrop:
                                batchTenderLine.BankDropAmount = amount;
                                batchTenderLine.BankDropAmountCur = amountCur;
                                break;

                            case TypeOfTransaction.TenderDeclaration:
                                batchTenderLine.DeclareTenderAmount = amount;
                                batchTenderLine.DeclareTenderAmountCur = amountCur;
                                break;

                            default:
                                String message = "Unsupported transaction type";
                                NetTracer.Error(message);
                                throw new NotSupportedException(message);
                        }
                    }
                }
            }

            // Calcualte tendered and change.
            dBCommand.Initialize(sqlTenderCalculatedLines, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            dBCommand.AddParameter("@TYPE1", TypeOfTransaction.Sales);
            dBCommand.AddParameter("@TYPE2", TypeOfTransaction.Payment);
            dBCommand.AddParameter("@TYPE3", TypeOfTransaction.SalesInvoice);
            dBCommand.AddParameter("@TYPE4", TypeOfTransaction.SalesOrder);
            dBCommand.AddParameter("@TYPE5", TypeOfTransaction.CustomerOrder);
            dBCommand.AddParameter("@TYPE6", TypeOfTransaction.IncomeExpense);
            using (DbDataReader reader = dBCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    BatchTenderLine batchTenderLine = batch.TenderLines.FindOrCreate(reader);
                    decimal amount = DBUtil.ToDecimal(reader["AMOUNT"]);
                    decimal amountCur = DBUtil.ToDecimal(reader["AMOUNTCUR"]);

                    if (DBUtil.ToBool(reader["CHANGELINE"]))
                    {
                        batchTenderLine.ChangeAmount = decimal.Negate(amount);
                        batchTenderLine.ChangeAmountCur = decimal.Negate(amountCur);
                    }
                    else
                    {
                        batchTenderLine.TenderedAmount = amount;
                        batchTenderLine.TenderedAmountCur = amountCur;
                        batchTenderLine.Count = DBUtil.ToInt32(reader["COUNT"]);
                    }

                    batchTenderLine.CountingRequired = DBUtil.ToBool(reader["COUNTINGREQUIRED"]);
                }
            }
        }

        /// <summary>
        /// Calculates tax for the batch.
        /// </summary>
        /// <param name="dBCommand">A <see cref="DbCommand"/> instance that is utilized.</param>
        /// <param name="batch">Current batch instance.</param>
        private static void CalculateTax(DbCommand dBCommand, Batch batch)
        {
            dBCommand.Initialize(sqlTaxAmount, batch);
            dBCommand.AddParameter("@TRANSACTIONSTATUS", TransactionStatus.Normal);
            using (var reader = dBCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var batchTaxLine = batch.TaxLines.FindOrCreate(reader);
                    decimal amount = (decimal)reader["AMOUNT"];
                    batchTaxLine.Amount += amount;
                }
            }
        }

        /// <summary>
        /// Initialize dBCommand with query and required parameters.
        /// </summary>
        /// <remarks>CA2100 The queries are already parametrized. No sql injection threat.</remarks>
        /// <param name="dBCommand"></param>
        /// <param name="query"></param>
        /// <param name="posBatch"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "The queries are already parametrized. No sql injection threat.")]
        private static void Initialize(this DbCommand dBCommand, string query, Batch posBatch = null)
        {
            dBCommand.CommandText = query;
            dBCommand.Parameters.Clear();

            if (posBatch != null)
            {
                dBCommand.Parameters.Add(new SqlParameter("@STOREID", posBatch.StoreId));
                dBCommand.Parameters.Add(new SqlParameter("@BATCHTERMINALID", posBatch.TerminalId));
                dBCommand.Parameters.Add(new SqlParameter("@BATCHID", posBatch.BatchId));
            }
            else
            {
                dBCommand.Parameters.Add(new SqlParameter("@STOREID", ApplicationSettings.Terminal.StoreId));
                dBCommand.Parameters.Add(new SqlParameter("@TERMINALID", ApplicationSettings.Terminal.TerminalId));
            }

            dBCommand.Parameters.Add(new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID));
        }

        /// <summary>
        /// Add parameter to dbCommand
        /// </summary>
        /// <param name="dBCommand"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        private static void AddParameter(this DbCommand dBCommand, string parameterName, object value)
        {
            dBCommand.Parameters.Add(new SqlParameter(parameterName, value));
        }

        /// <summary>
        /// Find or create a batch tender line.
        /// </summary>
        /// <param name="batchTenderLines"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static BatchTenderLine FindOrCreate(this Collection<BatchTenderLine> batchTenderLines, DbDataReader reader)
        {
            string tenderTypeId = (string)reader["TENDERTYPE"];
            string currency = (string)reader["CURRENCY"];
            string name = (string)reader["NAME"];
            BatchTenderLine batchTenderLine = batchTenderLines.FirstOrDefault(p => p.TenderTypeId == tenderTypeId && p.Currency == currency);

            if (batchTenderLine == null)
            {
                batchTenderLine = new BatchTenderLine();
                batchTenderLine.TenderTypeId = tenderTypeId;
                batchTenderLine.CardTypeId = string.Empty;
                batchTenderLine.Currency = currency;
                batchTenderLine.TenderName = name;
                batchTenderLines.Add(batchTenderLine);
            }

            return batchTenderLine;
        }

        /// <summary>
        /// Finds or creates a batch tax line.
        /// </summary>
        /// <param name="batchTaxLines">Current collection of batch tax lines.</param>
        /// <param name="reader">Current <see cref="DbDataReader"/> instance</param>
        /// <returns>An instance of <see cref="BatchTaxLine"/> found or created.</returns>
        private static BatchTaxLine FindOrCreate(this Collection<BatchTaxLine> batchTaxLines, DbDataReader reader)
        {
            string taxCode = (string)reader["TAXCODE"];
            BatchTaxLine batchTaxLine = batchTaxLines.FirstOrDefault(t => string.Equals(t.TaxCode, taxCode, StringComparison.OrdinalIgnoreCase));

            if (batchTaxLine == null)
            {
                batchTaxLine = new BatchTaxLine();
                batchTaxLine.TaxCode = taxCode;
                batchTaxLines.Add(batchTaxLine);
            }

            return batchTaxLine;
        }

        /// <summary>
        /// Read transaction for a given batch.
        /// </summary>
        /// <param name="batch">Batch object</param>
        /// <param name="connection">Connection to use</param>
        /// <returns>List of transaction ids.</returns>
        private static List<string> ReadTransactions(Batch batch, SqlConnection connection)
        {
            List<string> transactionIds = new List<string>();

            try
            {
                using (SqlCommand command = new SqlCommand(sqlVerifyTransactionTable, connection))
                {
                    command.Parameters.Add(new SqlParameter("@BATCHTERMINALID", batch.TerminalId));
                    command.Parameters.Add(new SqlParameter("@BATCHID", batch.BatchId));
                    command.Parameters.Add(new SqlParameter("@STOREID", ApplicationSettings.Terminal.StoreId));
                    command.Parameters.Add(new SqlParameter("@TERMINALID", ApplicationSettings.Terminal.TerminalId));
                    command.Parameters.Add(new SqlParameter("@DATAAREAID", EOD.InternalApplication.Settings.Database.DataAreaID));

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactionIds.Add(DBUtil.ToStr(reader["TRANSACTIONID"]));
                        }
                    }
                }
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return transactionIds;
        }

        #endregion

    }
}
