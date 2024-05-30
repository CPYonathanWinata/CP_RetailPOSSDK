using System.Text;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;

using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.Composition;
using System;
using System.Collections.Generic;

namespace GME_Custom.GME_Data
{
    public class queryData
    {
        public DataTable getCustomerInfo(string connString)
        {
            string queryString = @"SELECT a.AccountNum, b.Name, a.CustGroup, a.Currency FROM [ax].[CUSTTABLE] a INNER JOIN [ax].[DIRPARTYTABLE] b on b.RECID = a.PARTY";
            return getData(queryString, connString);
        }

        public string getWorkerStatus(string storeId, string operatorId, string connString)
        {            
            string queryString = @"SELECT e.POSPERMISSIONGROUPID FROM [ax].[HCMWORKER] a
                                    INNER JOIN [ax].[HCMPOSITIONWORKERASSIGNMENT] b on b.WORKER = a.RECID
                                    INNER JOIN [ax].[HCMPOSITIONDETAIL] c on c.POSITION = b.POSITION
                                    INNER JOIN [ax].[RETAILJOBPOSPERMISSIONGROUP] d on d.JOB = c.JOB
                                    INNER JOIN [ax].[RETAILPOSPERMISSIONGROUP] e on e.RECID = d.RETAILPOSPERMISSIONGROUP
                                    where a.PERSONNELNUMBER = '" + operatorId + "' AND c.VALIDTO = (select MAX(VALIDTO) from HCMPOSITIONDETAIL where HCMPOSITIONDETAIL.POSITION = b.POSITION) AND b.VALIDTO = '2154-12-31 23:59:59.000'";


            return getString("POSPERMISSIONGROUPID", queryString, connString); 
        }

        public string getStoreManagerOperatorId(string storeId, string terminalId, string connString)
        {
            string queryString = @"SELECT a.OPERATORID FROM [ax].[TBS_MANAGEROPERATOR] a
                                    WHERE a.STOREID = '"+ storeId +"' AND a.TERMINALID = '"+ terminalId + "'";

            return getString("OPERATORID", queryString, connString);
        }

        public string getStoreManagerOperatorName(string operatorId, string connString)
        {
            string queryString = @"SELECT b.Name FROM [ax].[HCMWORKER] a 
                                    INNER JOIN [ax].[DIRPARTYTABLE] b on b.RECID = a.PERSON
                                    WHERE a.PERSONNELNUMBER = '"+ operatorId +"'";

            return getString("NAME", queryString, connString);
        }

        public string isBonManualNumber(string storeId, string bonManualNum, string connString)
        {
            string queryString = @"SELECT a.TBS_INUSE FROM [ax].[TBS_BONMANUALDETAILS] a
                                    WHERE a.TBS_STOREID = '" + storeId + "' AND a.TBS_BONMANUALNUMSEQ = '" + bonManualNum + "'";
            
            return getString("TBS_INUSE", queryString, connString);
        }

        public List<string> checkBonManualLongkap(string storeId, string bonManualNum, string connString)
        {
            string queryString = @"SELECT TBS_BONMANUALNUMSEQ FROM AX.TBS_BONMANUALDETAILS
                                    WHERE RECID < (SELECT RECID FROM AX.TBS_BONMANUALDETAILS
                                    WHERE TBS_BONMANUALNUMSEQ = '" + bonManualNum + "' AND TBS_STOREID = '" + storeId + "') AND TBS_STOREID = '"+ storeId +"' AND TBS_INUSE = 0 ORDER BY RECID ASC";

            DataTable result = getData(queryString, connString);
            List<string> strResult = new List<string>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                strResult.Add(result.Rows[i]["TBS_BONMANUALNUMSEQ"].ToString());
            }

            return strResult;
        }

        public bool updateBonManualDetails(string bonManualNum, string storeId, string connString)
        {
            string queryString = @"UPDATE [ax].[TBS_BONMANUALDETAILS] set TBS_INUSE = 1 WHERE TBS_BONMANUALNUMSEQ = '" + bonManualNum + "' AND TBS_STOREID = '"+ storeId +"'";
            return executeNonQuery(queryString, connString);
        }

        public bool insertBonManualDataPull(string bonManualNum, string storeId, string reason, string channel, string terminalId, string transactionId, string receiptNum, decimal amount, string dataAreaId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_BONMANUALDETAILSDATAPULL] ([TBS_STOREID], --1
                                                                                        [TBS_BONMANUALNUMSEQ], --2
                                                                                        [TBS_REASON], --3
                                                                                        [TBS_INUSE], --4
                                                                                        [DATAAREAID], --5
                                                                                        [CHANNEL], --6
                                                                                        [STORE], --7
                                                                                        [TERMINAL], --8
                                                                                        [TRANSACTIONID], --9
                                                                                        [TBS_RECEIPTNUMBER], --10
                                                                                        [TBS_TOTALAMOUNTTRANSACTION] --11
                                                                                        )                                                                                        
                                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10,@PARM11)";
            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", bonManualNum));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", reason));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", "1"));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM5", dataAreaId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM6", channel));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM7", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM8", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM9", transactionId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM10", receiptNum));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM11", amount));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;                
            }
        }

        public bool insertManagerOperator(string operatorId, string storeId, string terminalId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_MANAGEROPERATOR] ([OPERATORID],
                                                                                [STOREID],
                                                                                [TERMINALID]
                                                                                )
                                                                                VALUES (@PARM1,@PARM2,@PARM3)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", operatorId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", storeId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", terminalId));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {

                return false;
            }
        }

        public bool insertNewCustomerDataExtend(string customerId, string gender, string skinType, DateTime dateOfBirth, string storeId, string channel, string transactionId, string terminalId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_CUSTOMERDATAPULL] ([CUSTACCOUNT], --1
                                                                                [GENDER], --2
                                                                                [SKINTYPE], --3
                                                                                [DATEOFBIRTH], --4
                                                                                [STORE], --5
                                                                                [CHANNEL], --6
                                                                                [TRANSACTIONID], --7
                                                                                [TERMINAL], --8
                                                                                [DATAAREAID], --9
                                                                                [ISMEMBER] --10
                                                                                )
                                                                                VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", customerId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", gender));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", skinType));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", dateOfBirth));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM5", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM6", channel));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM7", transactionId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM8", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM9", GME_Propesties.GME_Var.dataAreaId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM10", "1"));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;                
            }
        }

        public bool insertSalesPerson(string salesPersonId, string storeId, string terminalId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_SALESPERSON] ([SALESPERSONID], --1
                                                                            [SALESPERSONNAME], --2
                                                                            [STOREID], --3
                                                                            [TERMINALID] --4
                                                                            )
                                                                            VALUES(@PARM1,@PARM2,@PARM3,@PARM4)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", salesPersonId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", getStoreManagerOperatorName(salesPersonId, connString)));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", terminalId));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public string getSalesPerson(string storeId, string terminalId, string connString)
        {
            string queryString = @"SELECT a.SALESPERSONID FROM [ax].[TBS_SALESPERSON] a
                                    WHERE a.STOREID = '" + storeId + "' AND a.TERMINALID = '" + terminalId + "'";

            return getString("SALESPERSONID", queryString, connString);
        }

        public bool updateSalesPerson(string salesPersonId, string storeId, string terminalId, string connString)
        {
            string commandString = @"UPDATE [ax].[TBS_SALESPERSON] SET SALESPERSONID = '" + salesPersonId + "', SALESPERSONNAME = '"+ getStoreManagerOperatorName(salesPersonId, connString) + "' WHERE STOREID = '" + storeId + "' AND TERMINALID = '" + terminalId + "'";

            return executeNonQuery(commandString, connString);
        }

        public bool isStoreTerminalExists(string storeId, string terminalId, string connString)
        {
            string queryString = @"SELECT a.TERMINALID FROM [ax].[TBS_MANAGEROPERATOR] a
                                    WHERE a.STOREID = '" + storeId + "' and a.TERMINALID = '"+ terminalId +"'";

            if (getString("TERMINALID", queryString, connString) != "")
                return true;
            else
                return false;
        }

        public bool updateManagerOperator(string storeId, string operatorId, string terminalId, string connString)
        {
            string queryString = @"UPDATE [ax].[TBS_MANAGEROPERATOR] set OPERATORID = '" + operatorId + "' WHERE STOREID = '" + storeId + "' AND TERMINALID = '"+ terminalId +"'";
            return executeNonQuery(queryString, connString);
        }

        public string getChannelStore(string storeId, string connString)
        {
            string queryString = @"SELECT a.RECID FROM [ax].[RETAILCHANNELTABLE] a
                                    WHERE a.INVENTLOCATION = '" + storeId + "'";

            return getString("RECID", queryString, connString);
        }

        public string getCustomerId(string custId, string connString)
        {
            string queryString = @"SELECT a.ACCOUNTNUM FROM [ax].[CUSTTABLE] a
                                    WHERE a.ACCOUNTNUM = '" + custId + "'";

            return getString("ACCOUNTNUM", queryString, connString);
        }

        public string getCustomerName(string custId, string connString)
        {
            string queryString = @"select a.NAME from ax.DIRPARTYTABLE a
                                    inner join ax.CUSTTABLE b on b.PARTY = a.RECID
                                    where b.ACCOUNTNUM = '" + custId + "'";

            return getString("NAME", queryString, connString);
        }

        public bool isStorePointPay(string storeId, string connString)
        {
            string queryString = @"SELECT a.TBS_POINTPAY FROM [ax].[RETAILSTORETABLE] a
                                    WHERE a.STORENUMBER = '" + storeId + "'";

            if (getString("TBS_POINTPAY", queryString, connString) == "0")            
                return false;            
            else
                return true;
        }

        public bool isStoreDonasi(string storeId, string connString)
        {
            string queryString = @"SELECT a.TBS_DONASI FROM [ax].[RETAILSTORETABLE] a
                                    WHERE a.STORENUMBER = '" + storeId + "'";

            if (getString("TBS_DONASI", queryString, connString) == "0")
                return false;
            
                return true;
        }

        public string getFamilyMember(string familyCardMember, string connString)
        {
            string queryString = @"SELECT a.TBS_CARDNUMBER FROM [ax].[TBS_CUSTOMER_FAMILYCARD] a
                                    WHERE a.TBS_CARDNUMBER = '" + familyCardMember + "'";

            return getString("TBS_CARDNUMBER", queryString, connString);
        }

        public string getFamilyCustomerId(string familyCardMember, string connString)
        {
            string queryString1 = @"SELECT a.ACCOUNTNUM FROM [ax].[TBS_CUSTOMER_FAMILYCARD] a
                                    WHERE a.TBS_CARDNUMBER = '" + familyCardMember + "'";

            return getString("ACCOUNTNUM", queryString1, connString);
        }

        public string getFamilyMemberName(string familyCardMember, string connString)
        {
            string customerId = string.Empty;

            string queryString1 = @"SELECT a.ACCOUNTNUM FROM [ax].[TBS_CUSTOMER_FAMILYCARD] a
                                    WHERE a.TBS_CARDNUMBER = '" + familyCardMember + "'";

            customerId = getString("ACCOUNTNUM", queryString1, connString);

            return getCustomerName(customerId, connString);
        }

        //added by rizki (pay point)
        public string getFamilyCodeItem(string itemid, string connString)
        {
            string result;

            string queryString = "select TBS_FAMILYCODE from ax.ECORESPRODUCT where DISPLAYPRODUCTNUMBER = '" + itemid + "' ";

            result = getString("TBS_FAMILYCODE", queryString, connString);

            return result;
        }

        //add by rizki - test birthdayvoucher
        public Dictionary<string,string> getBirthdayVoucherData(string areaId, string connString)
        {
      
            string query1 = @"SELECT a.MINPEMBELANJAANTIER FROM ax.TBS_BirthdayVoucher a where a.DATAAREAID = '" + areaId + "'";
            string query2 = @"SELECT a.MAXPEMBELANJAANTIER FROM ax.TBS_BirthdayVoucher a where a.DATAAREAID = '" + areaId + "'";
            string query3 = @"SELECT a.MINPEMBELANJAANFAN FROM ax.TBS_BirthdayVoucher a where a.DATAAREAID = '" + areaId + "'";
            string query4 = @"SELECT a.MAXPEMBELANJAANFAN FROM ax.TBS_BirthdayVoucher a where a.DATAAREAID = '" + areaId + "'";
            string query5 = @"SELECT a.PercentageTierClub FROM ax.TBS_BirthdayVoucher a where a.DATAAREAID = '" + areaId + "'";
            string query6 = @"SELECT a.PercentageFanClub FROM ax.TBS_BirthdayVoucher a where a.DATAAREAID = '" + areaId + "'";

            string minclub = getString("MINPEMBELANJAANTIER", query1, connString);
            string maxclub = getString("MAXPEMBELANJAANTIER", query2, connString);
            string minfan = getString("MINPEMBELANJAANFAN", query3, connString);
            string maxfan = getString("MAXPEMBELANJAANFAN", query4, connString);
            string DiscTierClub = getString("PercentageTierClub", query5, connString);
            string DiscTierFan = getString("PercentageFanClub", query6, connString);


            Dictionary<string,string> dt = new Dictionary<string, string>();
            dt.Add("minclub", minclub);
            dt.Add("maxclub", maxclub);
            dt.Add("minfan", minfan);
            dt.Add("maxfan", maxfan);
            dt.Add("DiscTierClub", DiscTierClub);
            dt.Add("DiscTierFan", DiscTierFan);

            return dt;
        }

        //public bool updateReceiptIdTransTable(string receiptId, string transactionId, string connString)
        //{
        //    string queryString = @"UPDATE [ax].[RETAILTRANSACTIONTABLE] SET RECEIPTID = '" + receiptId + "' WHERE TRANSACTIONID = '" + transactionId + "'";

        //    return executeNonQuery(queryString, connString);
        //}

        //public bool updateReceiptIdSalesTrans(string receiptId, string transactionId, string connString)
        //{
        //    string queryString = @"UPDATE [ax].[RETAILTRANSACTIONSALESTRANS] SET RECEIPTID = '" + receiptId + "' WHERE TRANSACTIONID = '" + transactionId + "'";

        //    return executeNonQuery(queryString, connString);
        //}

        public string selectTransTable(string transactionId, string connString)
        {
            string queryString = @"SELECT a.RECEIPTID FROM [ax].[RETAILTRANSACTIONTABLE] a 
                                    WHERE a.TRANSACTIONID = '" + transactionId + "'";

            return getString("RECEIPTID", queryString, connString);
        }

        public bool insertReceiptNumberSequence(string storeId, string terminalId, string typeId, string transType, int year, int sequence, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_RECEIPTNUMBERSEQUENCE] ([TYPEID], --1
                                                                                    [TRANSTYPE], --2
                                                                                    [STOREID], --3
                                                                                    [TERMINALID], --4
                                                                                    [YEAR], --5
                                                                                    [SEQUENCE] --6
                                                                                    )
                                                                                    VALUES(@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6)";


            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", typeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", transType));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM5", year));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM6", sequence));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public string getNextReceiptNumberSequence(string storeId, string terminalId, string typeId, string year, string connString)
        {
            string queryString = @"SELECT a.SEQUENCE FROM [ax].[TBS_RECEIPTNUMBERSEQUENCE] a
                WHERE a.STOREID = '" + storeId + "' AND a.TERMINALID = '" + terminalId + "' AND a.TYPEID = '" + typeId + "' AND a.YEAR = '"+ year +"' ORDER BY a.SEQUENCE DESC";

            return getString("SEQUENCE", queryString, connString);
        }

        public string checkValidReceiptYear(string storeId, string terminalId, string typeId, string connString)
        {
            string queryString = @"SELECT a.YEAR FROM [ax].[TBS_RECEIPTNUMBERSEQUENCE] a
                WHERE a.STOREID = '" + storeId + "' AND a.TERMINALID = '" + terminalId + "' AND a.TYPEID = '" + typeId + "' ORDER BY a.YEAR DESC";

            return getString("YEAR", queryString, connString);
        }

        public DataTable getSkinType(string connString)
        {
            string queryString = @"SELECT a.NOMOR, a.DESKRIPSI FROM [ax].TBS_JENISKULIT a";

            return getData(queryString, connString);
        }

        ///////// ADD BY MARIA - CARD REPLACEMENT 
        public bool insertCardReplacement(string newCardNum, string oldCardNum, string oldCardType, string newCardType,
                                                   string storeId, string channel, string transactionId, string terminalId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_CARDREPLACEMENTDATAPULL] ([TBS_NEWCARDNUMBER], --1
                                                                             [TBS_OLDCARDNUMBER], --2
                                                                             [TBS_OLDCARDTYPE], --- 3
                                                                             [TBS_NEWCARDTYPE], --- 4
                                                                             [STORE], --5
                                                                             [CHANNEL], --6   
                                                                             [TRANSACTIONID], --7  
                                                                             [TERMINAL], --8
                                                                             [DATAAREAID] --9
                                                                            )
                                                                            VALUES(@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", newCardNum));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", oldCardNum));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", oldCardType));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", newCardType));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM5", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM6", channel));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM7", transactionId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM8", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM9", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public DataTable getCustomerUpdateInfo(string memberCardNumber, string connString)
        {
            string queryString = @"SELECT a.CARDNUMBER, b.ACCOUNTNUM, d.TBS_GENDER, d.TBS_DATEOFBIRTH, d.TBS_SKINTYPE, d.FIRSTNAME, d.LASTNAME, d.EMAIL, d.PHONENUMBER 
                                    FROM [AX].[RETAILLOYALTYCARD] a 
                                    JOIN [AX].[CUSTTABLE] b ON b.PARTY = a.PARTY 
                                    JOIN [AX].[TBS_CUSTOMEREXTENDSINFO] d ON d.ACCOUNTNUM = b.ACCOUNTNUM
                                    WHERE a.CARDNUMBER = '"+ memberCardNumber + "'";

            return getData(queryString, connString);
        }

        public bool insertUpdateCustomerDataPull(string firstName, string lastName, string gender, string email, string dateofBirth, string phoneNumber, string skinType, string accountNum, string connString,
            string channel, string terminalId, string storeId, string transactionId)
        {
            string commandString = @"INSERT INTO [ax].[TBS_UPDATECUSTOMERDATAPULL] ([CHANNEL], --1
                                                                                    [STORE], --2
                                                                                    [TERMINAL], -- 3
                                                                                    [TRANSACTIONID], --4
                                                                                    [TBS_SKINTYPE], --5
                                                                                    [TBS_GENDER], --6
                                                                                    [TBS_DATEOFBIRTH], --7
                                                                                    [PHONENUMBER], --8
                                                                                    [LASTNAME], --9
                                                                                    [FIRSTNAME], --10
                                                                                    [EMAIL], --11
                                                                                    [ACCOUNTNUM], --12
                                                                                    [DATAAREAID] --13
                                                                                    )
                                                                                    VALUES (@PARM1, @PARM2, @PARM3, @PARM4, @PARM5, @PARM6, @PARM7, @PARM8, @PARM9, @PARM10, @PARM11, @PARM12, @PARM13)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", channel));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", transactionId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM5", skinType));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM6", gender));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM7", dateofBirth.Substring(0, 10)));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM8", phoneNumber));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM9", lastName));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM10", firstName));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM11", email));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM12", accountNum));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM13", GME_Propesties.GME_Var.dataAreaId));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public string checkEmployeeIsMember(string NIK, string connString)
        {
            string queryString = @"select b.CARDNUMBER from [ax].[CUSTTABLE] a 
                                    join [ax].[RETAILLOYALTYCARD] b on b.PARTY = a.PARTY                                   
                                    where a.ACCOUNTNUM = '" + NIK + "'";

            return getString("CARDNUMBER", queryString, connString);
        }

        public string checkFamilyIsMember(string familyNumber, string connString)
        {
            string queryString = @"select b.CARDNUMBER from [ax].[TBS_CUSTOMER_FAMILYCARD] x
                                    join [ax].[CUSTTABLE] a on a.ACCOUNTNUM = x.ACCOUNTNUM
                                    join [ax].[RETAILLOYALTYCARD] b on b.PARTY = a.PARTY                                    
                                    where x.ACCOUNTNUM = '" + familyNumber + "'";

            return getString("CARDNUMBER", queryString, connString);
        }

        public string checkSearchCustomerIsMember(string NIK, string partyNumber, string connString)
        {
            string queryString = @"select b.CARDNUMBER from [ax].[CUSTTABLE] a 
                                    join [ax].[RETAILLOYALTYCARD] b on b.PARTY = a.PARTY                                   
                                    where a.ACCOUNTNUM = '" + NIK + "' and b.PARTY = '" + partyNumber + "'";

            return getString("CARDNUMBER", queryString, connString);
        }

        public string checkSearchCustomerIsMember(string NIK, string connString)
        {
            string queryString = @"select b.CARDNUMBER from [ax].[CUSTTABLE] a 
                                    join [ax].[RETAILLOYALTYCARD] b on b.PARTY = a.PARTY                                   
                                    where a.ACCOUNTNUM = '" + NIK + "'";

            return getString("CARDNUMBER", queryString, connString);
        }

        public string getCustomerIdByCardNumber(string cardNumber, string connString)
        {
            string queryString = @"Select a.ACCOUNTNUM from ax.CUSTTABLE a
                                    join ax.RETAILLOYALTYCARD b on b.PARTY = a.PARTY where b.CARDNUMBER = '" + cardNumber + "'";

            return getString("ACCOUNTNUM", queryString, connString);
        }

        public bool updatePaymentTypeCardBCA(string invoiceNumber, string approvalCode, string transactionId, string storeId, string terminalId, string cardTypeId, string connString)
        {
            string commandString = @"update ax.RETAILTRANSACTIONPAYMENTTRANS set TBS_INVOICENUMBER = '"+ invoiceNumber + "', TBS_APPROVALCODE = '"+ approvalCode + "', CARDTYPEID = '" + cardTypeId + "' where TRANSACTIONID = '" + transactionId + "' and STORE = '"+ storeId + "' and TERMINAL = '"+ terminalId + "' and TENDERTYPE = 16";

            return executeNonQuery(commandString, connString);
        }

        public bool updatePaymentTypeCardMandiri(string invoiceNumber, string approvalCode, string transactionId, string storeId, string terminalId, string cardTypeId, string connString)
        {
            string commandString = @"update ax.RETAILTRANSACTIONPAYMENTTRANS set TBS_INVOICENUMBER = '" + invoiceNumber + "', TBS_APPROVALCODE = '" + approvalCode + "', CARDTYPEID = '" + cardTypeId + "' where TRANSACTIONID = '" + transactionId + "' and STORE = '" + storeId + "' and TERMINAL = '" + terminalId + "' and TENDERTYPE = 17";

            return executeNonQuery(commandString, connString);
        }        

        public bool isCardNumberInAX(string cardNumber, string connString)
        {
            string queryString = @"SELECT CARDNUMBER FROM [AX].[RETAILLOYALTYCARD] 
                                    WHERE CARDNUMBER = '" + cardNumber + "'";

            if (getString("CARDNUMBER", queryString, connString) != string.Empty)
            {
                GME_Propesties.GME_Var.identifierCode = getDataString("CARDNUMBER", queryString, connString, string.Empty);
                return true;
            }
            else
            {
                return false;
            }
        }

        //ADD BY RIZKI for testing active voucher
        public bool insertToActiveVoucherTemp(string storeId, string terminalId, string voucherId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_ACTIVEVOUCHERTEMP] ([STORE], --1
                                                                                    [TERMINAL], --2
                                                                                    [VOUCHERID] --3
                                                                                    )
                                                                                    VALUES(@PARM1,@PARM2,@PARM3)";


            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", voucherId));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        //ADD BY RIZKI for get PKP Date
        public string getPKPDate(string storeid, string connString)
        {
            
            string queryString = "select TBS_PKPDate from ax.RETAILSTORETABLE where STORENUMBER = '" + storeid + " ' ";

            return getString("TBS_PKPDate", queryString, connString);
            
        }

        //ADD BY RIZKI for get Nama PTPKP
        public string getNamaPTPKP(string storeid, string connString)
        {

            string queryString = "select TBS_NAMAPTPKP from ax.RETAILSTORETABLE where STORENUMBER = '" + storeid + " ' ";

            return getString("TBS_NAMAPTPKP", queryString, connString);

        }

        //ADD BY RIZKI for get PKP Date
        public string getAlamatPKP(string storeid, string connString)
        {

            string queryString = "select TBS_ALAMATPKP from ax.RETAILSTORETABLE where STORENUMBER = '" + storeid + " ' ";

            return getString("TBS_ALAMATPKP", queryString, connString);

        }

        //ADD BY RIZKI for testing active voucher
        public int getItemType(string itemid, string dataareaid, string connString)
        {
            string type;

            string queryString = "select ITEMTYPE from inventtable where itemid = '" + itemid + " ' and DATAAREAID = '" + dataareaid + "' ";

            type = getString("ITEMTYPE", queryString, connString);

            return Convert.ToInt16(type);
        }

        //ADD BY RIZKI for testing active voucher
        public DataTable getVoucherDataTemp(string storeId, string terminalId, string connString)
        {
            string queryString = @"SELECT VOUCHERID FROM [ax].TBS_ACTIVEVOUCHERTEMP where STORE = '" + storeId + "' and TERMINAL = '" + terminalId + "'";

            return getData(queryString, connString);
        }

        //ADD BY RIZKI for testing active voucher
        public bool deleteVoucherDataTemp(string voucherId, string terminalId, string storeId, string connString)
        {
            string queryString = "Delete from [ax].[TBS_ACTIVEVOUCHERTEMP] where VOUCHERID = '" + voucherId + "' and TERMINAL = '" + terminalId + "' and STORE = '" + storeId + "'";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                } 

                using (SqlCommand sqlCmd = new SqlCommand(queryString, connection))
                {

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Added by Adhi (TBSI Voucher Baru)
        public bool insertRedeemVoucher(string storeId, string terminalId, string voucherId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_REDEEMVOUCHERTEMP] ([STORE], --1
                                                                                [TERMINAL], --2
                                                                                [VOUCHERID] --3
                                                                                )
                                                                                VALUES(@PARM1,@PARM2,@PARM3)";
            SqlConnection connection = new SqlConnection(connString);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", voucherId));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        //Added by Adhi (TBSI Voucher Baru)
        public DataTable getRedeemVoucher(string storeId, string terminalId, string connString)
        {
            string querryString = @"SELECT a.VOUCHERID FROM [ax].[TBS_REDEEMVOUCHERTEMP] a
                                    WHERE a.STORE = '" + storeId + "' AND a.TERMINAL = '" + terminalId + "'";

            return getData(querryString, connString);
        }

        //Added by Adhi(TBSI Voucher Baru)
        public bool deleteRedeemVoucher(string voucherId, string storeId, string terminalId, string connString)
        {
            string commandString = @"DELETE FROM [ax].[TBS_REDEEMVOUCHERTEMP]
                                    WHERE VOUCHERID = '" + voucherId + "' AND STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "'";

            return executeNonQuery(commandString, connString);
        }

        //Added by Adhi (TBSI Voucher Baru)
        public DataTable getvoucherExclude(string connString)
        {
            string queryString = @"SELECT CATEGORY FROM [ax].TBS_VOUCHEREXCLUDE";

            return getData(queryString, connString);
        }

        //Added by Adhi (TBSI Voucher Baru)
        public string getItemDonasi(string name, string connString)
        {
            string queryString = @"SELECT RECID from ax.ECORESCATEGORY WHERE NAME = '" + name + "'";
            return getString("RECID", queryString, connString);
        }

        //Added by Adhi(TBSI Voucher Baru)
        public bool updateVoucher(string voucherId, string storeId, string terminalId, string connString, string transactionId)
        {
            string commandString = @"UPDATE [ax].[RETAILTRANSACTIONTABLE] SET TBS_VOUCHERCODE = '" + voucherId + "' WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";

            return executeNonQuery(commandString, connString);
        }

        public bool updateRetailTransactionTable(Dictionary<string, string> value, string storeId, string terminalId, string transactionId, string connString)
        {

            string totalDPP = GME_Custom.GME_Propesties.GME_Var.totalDPP.ToString();
            if (totalDPP.Contains(","))
                totalDPP = totalDPP.Replace(",", ".");

            string queryString = @"UPDATE [ax].[RETAILTRANSACTIONTABLE] set TBS_POINT = '" + value["POINT"] + "', TBS_DPP = " + totalDPP + ", TBS_ApprovalCode = '" + value["ApprovalCode"] + "', TBS_PAN = '" + value["PAN"] + "'  WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";
            return executeNonQuery(queryString, connString);
        }

        public Dictionary<string, string> getReprintVariable(string transactionId, string terminalId, string storeId, string connString)
        {
            string query1 = @"SELECT TBS_POINT FROM ax.RETAILTRANSACTIONTABLE   WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";
            string query2 = @"SELECT TBS_DPP FROM ax.RETAILTRANSACTIONTABLE   WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";
            string query3 = @"SELECT TBS_ApprovalCode FROM ax.RETAILTRANSACTIONTABLE   WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";
            string query4 = @"SELECT TBS_PAN FROM ax.RETAILTRANSACTIONTABLE  WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";
            string query5 = @"SELECT TBS_ReceiptId FROM ax.RETAILTRANSACTIONTABLE  WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";


            string TBS_POINT = getString("TBS_POINT", query1, connString);
            string TBS_DPP = getString("TBS_DPP", query2, connString);
            string TBS_ApprovalCode = getString("TBS_ApprovalCode", query3, connString);
            string TBS_PAN = getString("TBS_PAN", query4, connString);
            string receiptId = getString("TBS_ReceiptId", query5, connString);

            Dictionary<string, string> dt = new Dictionary<string, string>();
            dt.Add("TBS_POINT", TBS_POINT);
            dt.Add("TBS_DPP", TBS_DPP);
            dt.Add("TBS_ApprovalCode", TBS_ApprovalCode);
            dt.Add("TBS_PAN", TBS_PAN);
            dt.Add("TBS_ReceiptId", receiptId);


            return dt;
        }

        //Added by Adhi (TBSI Voucher Baru)
        public string getItemUndiscount(string name, string connString)
        {
            string queryString = @"SELECT RECID from ax.ECORESCATEGORY WHERE NAME = '" + name + "'";
            return getString("RECID", queryString, connString);
        }

        //Approval Code
        public bool updateApprovalCode(string approvalCode, string storeId, string terminalId, string connString)
        {
            string commandString = @"UPDATE [ax].[RETAILTRANSACTIONPAYMENTTRANS]  SET TBS_APPROVALCODE = '" + approvalCode + "' WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "'";

            return executeNonQuery(commandString, connString);
        }

        public bool checkExistPublicByPhone(string phoneNumber, string connString)
        {
            bool result = false;

            string queryString = @"SELECT b.ACCOUNTNUM FROM ax.LOGISTICSELECTRONICADDRESS c
                                    JOIN ax.DIRPARTYLOCATION d ON d.LOCATION = c.LOCATION
                                    JOIN ax.DIRPARTYTABLE a ON a.RECID = d.PARTY
                                    JOIN ax.CUSTTABLE b ON b.PARTY = a.RECID
                                    WHERE c.LOCATOR = '" + phoneNumber + "' and TYPE = 1";

            if (getDataString("ACCOUNTNUM", queryString, connString, string.Empty) != string.Empty)
            {
                GME_Propesties.GME_Var.customerId = getDataString("ACCOUNTNUM", queryString, connString, string.Empty);
                result = true;                
            }

            return result;
        }

        public bool checkExistPublicByEmail(string email, string connString)
        {
            bool result = false;

            string queryString = @"SELECT b.ACCOUNTNUM FROM ax.LOGISTICSELECTRONICADDRESS c
                                    JOIN ax.DIRPARTYLOCATION d ON d.LOCATION = c.LOCATION
                                    JOIN ax.DIRPARTYTABLE a ON a.RECID = d.PARTY
                                    JOIN ax.CUSTTABLE b ON b.PARTY = a.RECID
                                    WHERE c.LOCATOR = '" + email + "' and TYPE = 2";

            if (getDataString("ACCOUNTNUM", queryString, connString, string.Empty) != string.Empty)
            {
                GME_Propesties.GME_Var.customerId = getDataString("ACCOUNTNUM", queryString, connString, string.Empty);
                result = true;
            }

            return result;
        }

        public bool checkExistPublicByCardNum(string cardNumber, string connString)
        {
            bool result = false;

            string queryString = @"SELECT CARDNUMBER FROM ax.RETAILLOYALTYCARD WHERE CARDNUMBER = '" + cardNumber + "'";

            if (getDataString("CARDNUMBER", queryString, connString, string.Empty) != string.Empty)
            {
                result = true;
            }

            return result;
        }

        public void getCardNumberFromCustId(string CustID, string connString)
        {
            string queryString = "SELECT a.ACCOUNTNUM, b.CARDNUMBER AS CARDNUM FROM ax.CUSTTABLE a join ax.RETAILLOYALTYCARD b on b.PARTY = a.PARTY where a.ACCOUNTNUM = '" + CustID + "'";

            if (getDataString("CARDNUM", queryString, connString, string.Empty) != string.Empty)
            {
                GME_Propesties.GME_Var.identifierCode = getDataString("CARDNUM", queryString, connString, string.Empty);
            }
        }

        public DataTable getEnrollmentType(string connString)
        {
            string queryString = @"SELECT TYPE FROM ax.TBS_ENROLLMENTTYPE";

            return getData(queryString, connString);
        }

        public bool updateReceiptCustomerTransTable(string receiptId,  string customerIdOffline, string transactionId, string connString)
        {
            string commandString = @"UPDATE ax.RETAILTRANSACTIONTABLE set TBS_RECEIPTID = '"+ receiptId +"', TBS_ENROLLMENTTYPE = '"+ GME_Propesties.GME_Var.custEnrollType +"', CUSTACCOUNT = '"+ customerIdOffline + "'  WHERE TRANSACTIONID = '" + transactionId + "'";

            return executeNonQuery(commandString, connString);
        }

        public bool updateReceiptIdSalesTrans(string receiptId, string transactionId, string connString)
        {
            string commandString = @"UPDATE ax.RETAILTRANSACTIONSALESTRANS set TBS_RECEIPTID = '" + receiptId + "' WHERE TRANSACTIONID = '" + transactionId + "'";

            return executeNonQuery(commandString, connString);
        }        
                
        //ADHI GET LINES DISCOUNT (IF NOT NULL) (DISKON & PROMO)
        public DataTable getProductLinesNotNull(string offerid, string connString)
        {
            string queryString = @"SELECT a.OFFERID, b.PRODUCT, c.DISPLAYPRODUCTNUMBER from ax.RETAILPERIODICDISCOUNTLINE a
                                    join ax.RETAILGROUPMEMBERLINE b on b.RECID = a.RETAILGROUPMEMBERLINE
                                    join ax.ECORESPRODUCT c on c.RECID = b.PRODUCT
                                    where a.OFFERID = '" + offerid + "'";
            return getData(queryString, connString);
        }
        
        //ADHI GET AMOUNT ITEM THRESHOLD (DISKON & PROMO)
        public string getAmountItemThreshold(string itemId, string connString)
        {
            string queryString = @"SELECT * from ax.PRICEDISCTABLE 
                                    WHERE ITEMRELATION = '" + itemId + "' and FROMDATE < GETDATE() and TODATE > GETDATE()";
            return getString("AMOUNT", queryString, connString);
        }

        //NEW
        public string getParentCategory(long category, string connString)
        {
            string queryString = @"SELECT PARENTCATEGORY FROM AX.ECORESCATEGORY	
		                            WHERE RECID = '" + category + "'";
            return getString("PARENTCATEGORY", queryString, connString);
        }

        //NEW
        public string getCategory(string itemId, string connString)
        {
            string queryString = @"SELECT PARENTCATEGORY FROM INVENTTABLE A
                                    INNER JOIN INVENTITEMGROUPITEM B ON B.ITEMID = A.ITEMID AND B.ITEMDATAAREAID = A.DATAAREAID
                                    INNER JOIN ECORESPRODUCTCATEGORY C on C.PRODUCT = A.PRODUCT
                                    INNER JOIN ECORESCATEGORY D on D.RECID = C.CATEGORY
                                    WHERE A.ITEMID = '" + itemId + "'";
            return getString("PARENTCATEGORY", queryString, connString);
        }

        //NEW
        public DataTable getCategoryItem(string itemID, string connString)
        {
            string queryString = @"SELECT DISTINCT
                                    (SELECT ERCLV1.PARENTCATEGORY FROM ECORESCATEGORY ERCLV1
	                                    WHERE ERCLV1.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV2
			                            WHERE ERCLV2.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV3
				                        WHERE ERCLV3.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV4
				                        WHERE ERCLV4.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV5
					                    WHERE ERCLV5.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV6
						                WHERE ERCLV6.RECID = C.CATEGORY)))))) 
                                    LEVEL1,
                                    (SELECT ERCLV2.PARENTCATEGORY FROM ECORESCATEGORY ERCLV2
	                                    WHERE ERCLV2.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV3
			                            WHERE ERCLV3.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV4
			                            WHERE ERCLV4.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV5
				                        WHERE ERCLV5.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV6
					                    WHERE ERCLV6.RECID = C.CATEGORY)))))
                                    LEVEL2,
                                    (SELECT ERCLV3.PARENTCATEGORY FROM ECORESCATEGORY ERCLV3
	                                    WHERE ERCLV3.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV4
			                            WHERE ERCLV4.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV5
			                            WHERE ERCLV5.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV6
				                        WHERE ERCLV6.RECID = C.CATEGORY)))) 
                                    LEVEL3,
                                    (SELECT ERCLV4.PARENTCATEGORY FROM ECORESCATEGORY ERCLV4
	                                    WHERE ERCLV4.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV5
		                                WHERE ERCLV5.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV6
			                            WHERE ERCLV6.RECID = C.CATEGORY))) 
                                    LEVEL4,
                                    (SELECT ERCLV5.PARENTCATEGORY FROM ECORESCATEGORY ERCLV5
	                                    WHERE ERCLV5.RECID = (SELECT PARENTCATEGORY FROM ECORESCATEGORY ERCLV6
		                                WHERE ERCLV6.RECID = C.CATEGORY)) 
                                    LEVEL5,
                                    D.PARENTCATEGORY AS LEVEL6,
                                    A.ITEMID
                                    FROM INVENTTABLE A
                                    INNER JOIN INVENTITEMGROUPITEM B ON B.ITEMID = A.ITEMID AND B.ITEMDATAAREAID = A.DATAAREAID
                                    INNER JOIN ECORESPRODUCTCATEGORY C on C.PRODUCT = A.PRODUCT
                                    INNER JOIN ECORESCATEGORY D on D.RECID = C.CATEGORY
                                    WHERE A.ITEMID = '" + itemID + "'";
            return getData(queryString, connString);
        }

        //GWP
        public DataTable getAllInfo(string connString)
        {
            string queryString = @"WITH hirarkiCTE AS
                        ( 
                        SELECT recid,PARENTCATEGORY,NAME,LEVEL, b.CATEGORYHIERARCHY
                        FROM ax.ECORESCATEGORY  b                        
                        UNION ALL
                        SELECT c.recid, c.PARENTCATEGORY,c.NAME, c.LEVEL, c.CATEGORYHIERARCHY
                        FROM ax.ECORESCATEGORY c 
                        INNER JOIN hirarkiCTE s ON s.PARENTCATEGORY = c.RECID 
                        )
                        SELECT DISTINCT * 
                        FROM hirarkiCTE  a
                        join (SELECT  A.OFFERID, D.AMOUNTTHRESHOLD, C.PRODUCT, C.CATEGORY ,D.TBS_FREEDISCITEMID
                        ,D.TBS_DISCOUNTTYPE, D.DISCOUNTVALUE , d.TBS_OPERATOR,a.CONCURRENCYMODE
                        from ax.RETAILPERIODICDISCOUNT  A
                        INNER JOIN AX.RETAILPERIODICDISCOUNTLINE B ON B.OFFERID = A.OFFERID
                        INNER JOIN AX.RETAILGROUPMEMBERLINE C ON C.RECID = B.RETAILGROUPMEMBERLINE
                        INNER JOIN AX.RETAILDISCOUNTTHRESHOLDTIERS D ON D.OFFERID = A.OFFERID
                        where A.PERIODICDISCOUNTTYPE = '4' and A.STATUS = '1' 
                        ) c on a.RECID = c.CATEGORY;
                        ";
            return getData(queryString, connString);
        }

        //GWP
        public DataTable getCategoryProduct(string connString, string productId, Int64 category)
        {
            string quer = @"DECLARE @RefRecId as BIGINT
                            DECLARE @CategoryHierarchy as BIGINT
                            DECLARE @RecIdECoreCategory as BIGINT
                            DECLARE @productID as varchar(50)
                            set @productID = '" + productId + "' ";
            quer += @"set @RefRecId = (select top(1) RECID from ax.ECORESPRODUCT
                            where DISPLAYPRODUCTNUMBER = @productID);

                            select top(1) @CategoryHierarchy = c.CATEGORYHIERARCHY,@RecIdECoreCategory = c.CATEGORY from ax.ECORESPRODUCTCATEGORY c where PRODUCT = @RefRecId;

                            WITH hirarkiCTE AS
                            ( 
                            SELECT recid,PARENTCATEGORY,NAME,LEVEL, b.CATEGORYHIERARCHY
                            FROM ax.ECORESCATEGORY  b
                            WHERE 
                            b.CATEGORYHIERARCHY = @CategoryHierarchy
                            and b.RECID = @RecIdECoreCategory
                            UNION ALL
                            SELECT c.recid, c.PARENTCATEGORY,c.NAME, c.LEVEL, c.CATEGORYHIERARCHY
                            FROM ax.ECORESCATEGORY c 
                            INNER JOIN hirarkiCTE s ON s.PARENTCATEGORY = c.RECID 
                            WHERE
                            c.CATEGORYHIERARCHY = @CategoryHierarchy
                            )

                            SELECT DISTINCT * 
                            FROM hirarkiCTE  a
                            where RECID = '" + category + "';";

            return getData(quer, connString);
        }

        //VOUCHER EXCLUDE
        public DataTable getItemVoucherexclude(string connString)
        {
            string queryString = @"SELECT b.PARENTCATEGORY FROM ax.TBS_VOUCHEREXCLUDE a
	                                JOIN ax.ECORESCATEGORY b on b.NAME = a.CATEGORY";

            return getData(queryString, connString);
        }

        //Added by Adhi(Welcome Voucher 50Rb)
        public string getVerification50Rb(string connString)
        {
            string queryString = @"SELECT WELCOMEVOUCHER50RB FROM ax.TBS_WELCOMEVOUCHER";

            return getString("WELCOMEVOUCHER50RB", queryString, connString);
        }

        //Added by Adhi(Welcome Voucher 50Rb)
        public string getMinTransaksi50(string connString)
        {
            string queryString = @"SELECT MINTRANS50RB FROM ax.TBS_WELCOMEVOUCHER";

            return getString("MINTRANS50RB", queryString, connString);
        }

        //Added by Adhi(Welcome Voucher 50Rb)
        public DataTable getkategori50(string connString)
        {
            string queryString = @"SELECT b.RECID FROM ax.TBS_WLCMVOUCHTHRESHOLD50 a
                                        JOIN ax.ECORESCATEGORY b on b.NAME = a.KATEGORI50";
            return getData(queryString, connString);
        }

        //Added by Adhi(Welcome Voucher 100Rb)
        public string getVerification100Rb(string connString)
        {
            string queryString = @"SELECT WELCOMEVOUCHER100RB FROM ax.TBS_WELCOMEVOUCHER";

            return getString("WELCOMEVOUCHER100RB", queryString, connString);
        }

        //Added by Adhi(Welcome Voucher 100Rb)
        public string getMinTransaksi100(string connString)
        {
            string queryString = @"SELECT MINTRANS100RB FROM ax.TBS_WELCOMEVOUCHER";

            return getString("MINTRANS100RB", queryString, connString);
        }

        //Added by Adhi(Welcome Voucher 100Rb)
        public DataTable getkategori100(string connString)
        {
            string queryString = @"SELECT b.RECID FROM ax.TBS_WLCMVOUCHTHRESHOLD100 a
                                        JOIN ax.ECORESCATEGORY b on b.NAME = a.KATEGORI100";

            return getData(queryString, connString);
        }

        //Added by Adhi(Welcome Voucher)
        public string getKodePromoIDWelcome(string connString)
        {
            string queryString = @"SELECT KODEPROMOID FROM ax.TBS_WELCOMEVOUCHER";

            return getString("KODEPROMOID", queryString, connString);
        }

        //Added by Adhi(Welcome Voucher)
        public bool updateKodePromoID(string kodepromoId, string storeId, string terminalId, string connString, string transactionId, int discountOrginType)
        {
            string commandString = @"UPDATE [ax].[RETAILTRANSACTIONDISCOUNTTRANS]  SET PERIODICDISCOUNTOFFERID = '" + kodepromoId + "' WHERE STOREID = '" + storeId + "' AND TERMINALID = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "' AND DISCOUNTORIGINTYPE = '" + discountOrginType + "'";

            return executeNonQuery(commandString, connString);
        }
        
        //Added by Adhi(Birthday Voucher)
        public string getKodePromoIDBirthdayCLUB(string connString)
        {
            string queryString = @"SELECT KODEPROMOIDCLUB FROM ax.TBS_BIRTHDAYVOUCHER";

            return getString("KODEPROMOIDCLUB", queryString, connString);
        }

        //Added by Adhi(Birthday Voucher)
        public string getKodePromoIDBirthdayFAN(string connString)
        {
            string queryString = @"SELECT KODEPROMOIDFAN FROM ax.TBS_BIRTHDAYVOUCHER";

            return getString("KODEPROMOIDFAN", queryString, connString);
        }

        //Added by Adhi (Disc Voucher)
        public bool insertVoucherTransactionPull(int cashVoucher, int discVoucher, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_VOUCHERTRANSACTIONPULL] ([CASHVOUCHER], --1
                                                                                    [DISCOUNTVOUCHER], --2  
                                                                                    [CHANNEL], --3
                                                                                    [DATAAREAID], --4
                                                                                    [STORE], --5
                                                                                    [TERMINAL], --6
                                                                                    [TRANSACTIONID] --7                                                                                 
                                                                                    )
                                                                                    VALUES(@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", cashVoucher));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", discVoucher));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", channel));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM5", posTransaction.StoreId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM6", posTransaction.TerminalId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM7", posTransaction.TransactionId));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        //Approval Code
        public bool updateApprovalCode(string approvalCode, string storeId, string terminalId, string connString, string transactionId)
        {
            string commandString = @"UPDATE [ax].[RETAILTRANSACTIONPAYMENTTRANS]  SET TBS_APPROVALCODE = '" + approvalCode + "' WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";

            return executeNonQuery(commandString, connString);
        }

        //DiscVoucherinsertCustomerTransOfflineGuestinsertCustomerTransOfflineGuest
        public bool updateDiscVoucher(int discVoucher, string storeId, string terminalId, string connString, string transactionId)
        {
            string commandString = @"UPDATE [ax].[RETAILTRANSACTIONTABLE]  SET TBS_DISCVOUCHER = '" + discVoucher + "' WHERE STORE = '" + storeId + "' AND TERMINAL = '" + terminalId + "' AND TRANSACTIONID = '" + transactionId + "'";

            return executeNonQuery(commandString, connString);
        }

        public string getKodePengikatRedeem(string connString)
        {
            string queryString = @"SELECT a.KODEPENGIKAT FROM [ax].[TBS_KODEPENGIKATREDEEM] a";

            return getString("KODEPENGIKAT", queryString, connString);
        }

        public string getCustomerByCardNumber(string cardNumber, string connString)
        {
            string queryString = @"SELECT b.ACCOUNTNUM from ax.RETAILLOYALTYCARD a
                                    join ax.CUSTTABLE b on b.PARTY = a.PARTY
                                    where a.CARDNUMBER = '" + cardNumber + "'";

            return getString("ACCOUNTNUM", queryString, connString);
        }

        public string getCustomerByTransaction(string receiptId, string connString)
        {
            string queryString = @"select CUSTACCOUNT from RETAILTRANSACTIONTABLE where RECEIPTID = '" + receiptId + "'";

            return getString("CUSTACCOUNT", queryString, connString);
        }

        #region offline
        public bool insertGuestCustomerOfflineSNF(string firstName, string phoneNumber, string email, int status, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_ENGAGESNF] ([FIRSTNAME], --1
                                                                        [PHONENUMBER], --2
                                                                        [EMAIL], --3
                                                                        [STATUS], --4
                                                                        [CHANNEL], --5
                                                                        [DATAAREAID], --6
                                                                        [STORE], --7
                                                                        [TERMINAL], --8
                                                                        [TRANSACTIONID] --9
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", firstName));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", phoneNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", email));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", channel));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM5", status));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM6", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM7", posTransaction.StoreId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM8", posTransaction.TerminalId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM9", posTransaction.TransactionId));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public bool insertOfflineSNF(IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_ENGAGESNF] ([BIRTHDAY],  --1
                                                                        [CASHIERID],  --2
                                                                        [CHANNEL],  --3
                                                                        [CURRENCYCODE],  --4
                                                                        [CUSTOMERID],  --5
                                                                        [EMAIL],  --6
                                                                        [FAMILYCODE],  --7
                                                                        [FIRSTNAME],  --8
                                                                        [GENDER],  --9
                                                                        [HOUSEHOLDID],  --10
                                                                        [IDENTIFIERID],  --11
                                                                        [LASTNAME],  --12
                                                                        [PERSONID],  --13
                                                                        [PHONENUMBER],  --14
                                                                        [PROCESSSTOPPED],  --15
                                                                        [PRODUCTCODE],  --16
                                                                        [QTY],  --17
                                                                        [DATAAREAID],  --18
                                                                        [SKINTYPE],  --19
                                                                        [STORE],  --20
                                                                        [TERMINAL],  --21
                                                                        [TRANSACTIONDATE],  --22
                                                                        [TRANSACTIONID],  --23
                                                                        [TRANSACTIONTIME],  --24
                                                                        [UNITPRICE],  --25
                                                                        [VOUCHERNUMBER],  --26                                                                        
                                                                        [STATUS],  --27
                                                                        [ENROLLTYPE], --28
                                                                        [IDENTIFIERIDREPLACEMENT] --29
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10,@PARM11,@PARM12,@PARM13,@PARM14,@PARM15,
                                                                                @PARM16,@PARM17,@PARM18,@PARM19,@PARM20,@PARM21,@PARM22,@PARM23,@PARM24,@PARM25,@PARM26,@PARM27,@PARM28,@PARM29)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    for (int i = 0; i < GME_Propesties.GME_Var.fieldSNF.Length; i++)
                    {
                        int j = i + 1;
                        if (GME_Propesties.GME_Var.GetSNF(GME_Propesties.GME_Var.fieldSNF[i]) != null)
                        {
                            sqlcmd.Parameters.Add(new SqlParameter("@PARM" + j.ToString(), GME_Propesties.GME_Var.GetSNF(GME_Propesties.GME_Var.fieldSNF[i])));
                        }
                        else
                        {
                            switch (GME_Propesties.GME_Var.fieldSNF[i])
                            {
                                case "DATAAREAID":
                                    sqlcmd.Parameters.Add(new SqlParameter("@PARM" + j.ToString(), GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                                    break;
                                case "STORE":
                                    sqlcmd.Parameters.Add(new SqlParameter("@PARM" + j.ToString(), posTransaction.StoreId));
                                    break;
                                case "TERMINAL":
                                    sqlcmd.Parameters.Add(new SqlParameter("@PARM" + j.ToString(), posTransaction.TerminalId));
                                    break;
                                case "TRANSACTIONID":
                                    sqlcmd.Parameters.Add(new SqlParameter("@PARM" + j.ToString(), posTransaction.TransactionId));
                                    break;
                                default:
                                    sqlcmd.Parameters.Add(new SqlParameter("@PARM" + j.ToString(), ""));
                                    break;
                            }
                        }
                    }

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public bool insertOfflineSNFLines(string productCode, string familyCode, decimal qty, decimal productPrice, string voucherNumber, string ticketNumber, string ticketDate, int ticketIndex, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_ENGAGESNFLINE] ([CHANNEL],  --1
                                                                        [CURRENCYCODE],  --2                                                                      
                                                                        [FAMILYCODE],  --3                                                                                                                                                
                                                                        [PRODUCTCODE],  --4
                                                                        [QTY],  --5
                                                                        [DATAAREAID],  --6                                                                      
                                                                        [STORE],  --7
                                                                        [TERMINAL],  --8
                                                                        [TRANSACTIONDATE],  --9
                                                                        [TRANSACTIONID],  --10
                                                                        [TRANSACTIONTIME],  --11
                                                                        [UNITPRICE],  --12
                                                                        [VOUCHERNUMBER],  --13                                                                                                                                                
                                                                        [TICKETNUMBER], -- 14
                                                                        [TICKETDATE], --15
                                                                        [TICKETINDEX] -- 16
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10,@PARM11,@PARM12,@PARM13,@PARM14,@PARM15,
                                                                                @PARM16)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", channel));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", "IDR"));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", familyCode));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", productCode));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM5", qty));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM6", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM7", posTransaction.StoreId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM8", posTransaction.TerminalId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM9", DateTime.Now.ToString("yyyyMMdd")));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM10", posTransaction.TransactionId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM11", DateTime.Now.ToString("HHmmss")));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM12", productPrice));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM13", voucherNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM14", ticketNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM15", ticketDate));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM16", ticketIndex));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }                
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public bool insertOfflineSNFVoucher( string voucherNumber, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_ENGAGESNFLINE] ([CHANNEL],  --1                                                                        
                                                                        [DATAAREAID],  --2                                                                      
                                                                        [STORE],  --3
                                                                        [TERMINAL],  --4                                                                       
                                                                        [TRANSACTIONID],  --5                                                                                                                                               
                                                                        [VOUCHERNUMBER],  --6                                                                                                                                                                                                                       
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", channel));;
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", posTransaction.StoreId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", posTransaction.TerminalId));                    
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM5", posTransaction.TransactionId));                                        
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM6", voucherNumber));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public bool insertCustomerTransOfflineEnroll(string customerId, string firstName, string LastName, string gender, string cardNumber, DateTime birthDate,
                                                int skinType, int enrollType, string email, string phoneNumber, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;
            int newGender = 0;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            if (gender == "M")
                newGender = 1;
            else
                newGender = 2;

            string commandString = @"INSERT INTO [ax].[TBS_CUSTOMERTRANSACTIONOFFLINE] ([CUSTOMERID], --1
                                                                        [FIRSTNAME], --2
                                                                        [LASTNAME], --3
                                                                        [GENDER], --4
                                                                        [CARDNUMBER], --5
                                                                        [BIRTHDATE], --6
                                                                        [SKINTYPE], --7
                                                                        [ENROLLTYPE], --8
                                                                        [EMAIL], --9
                                                                        [PHONENUMBER], --10
                                                                        [CHANNEL], --11
                                                                        [DATAAREAID], --12
                                                                        [STORE], --13
                                                                        [TERMINAL], --14
                                                                        [TRANSACTIONID] --15
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10,
                                                                        @PARM11,@PARM12,@PARM13,@PARM14,@PARM15)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", customerId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", firstName));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", LastName));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", newGender));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM5", cardNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM6", birthDate));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM7", skinType));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM8", enrollType));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM9", email));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM10", phoneNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM11", channel));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM12", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM13", posTransaction.StoreId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM14", posTransaction.TerminalId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM15", posTransaction.TransactionId));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public string getNextCustomerIdSequence(string storeId, string terminalId, string connString)
        {
            string queryString = @"SELECT a.SEQUENCE FROM [ax].[TBS_CUSTOMEROFFLINENUMSEQ] a
                WHERE a.STORE = '" + storeId + "' AND a.TERMINAL = '" + terminalId + "' ORDER BY a.SEQUENCE DESC";

            return getString("SEQUENCE", queryString, connString);
        }

        public bool insertCustomerOfflineSequence(string storeId, string terminalId, int sequence, string customerId, string connString)
        {
            string commandString = @"INSERT INTO [ax].[TBS_CUSTOMEROFFLINENUMSEQ] ([STORE], --1
                                                                                    [TERMINAL], --2                                                                                    
                                                                                    [SEQUENCE], --3
                                                                                    [CUSTOMERID] --4                                                                                   
                                                                                    )
                                                                                    VALUES(@PARM1,@PARM2,@PARM3,@PARM4)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlCmd = new SqlCommand(commandString, connection))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM1", storeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM2", terminalId));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM3", sequence));
                    sqlCmd.Parameters.Add(new SqlParameter("@PARM4", customerId));

                    int result = sqlCmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public bool insertCustomerTransOfflineGuest(string customerId, string firstName, string LastName, string cardNumber, string phoneNumber, string email,
                                                    IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = this.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_CUSTOMERTRANSACTIONOFFLINE] ([CUSTOMERID], --1
                                                                        [FIRSTNAME], --2
                                                                        [LASTNAME], --3                                                                       
                                                                        [CARDNUMBER], --4                                                                      
                                                                        [EMAIL], --5
                                                                        [PHONENUMBER], --6
                                                                        [CHANNEL], --7
                                                                        [DATAAREAID], --8
                                                                        [STORE], --9
                                                                        [TERMINAL], --10
                                                                        [TRANSACTIONID] --11
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10,
                                                                        @PARM11)";

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sqlcmd = new SqlCommand(commandString, connection))
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM1", customerId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM2", firstName));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM3", LastName));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", cardNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM5", email));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM6", phoneNumber));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM7", channel));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM8", GME_Propesties.Connection.applicationLoc.Settings.Database.DataAreaID));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM9", posTransaction.StoreId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM10", posTransaction.TerminalId));
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM11", posTransaction.TransactionId));

                    int result = sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public string getRedeemAmountDefault(string connString)
        {
            string queryString = @"SELECT TOREWARDAMOUNTQTY FROM ax.RETAILLOYALTYREDEEMSCHEMELINE
                                    WHERE TOREWARDTYPE = 3";

            return getString("TOREWARDAMOUNTQTY", queryString, connString);
        }

        public DataTable getLoyaltyItems(string connString)
        {
            string querystring = "select DISPLAYPRODUCTNUMBER from ax.RETAILLOYALTYREDEEMSCHEMELINE a join ax.RETAILGROUPMEMBERLINE b on b.RECID = a.TORETAILGROUPMEMBERLINE join ax.ECORESPRODUCT c on c.RECID = b.PRODUCT where a.TOREWARDTYPE = '2'";

            return getData(querystring, connString);
        }

        public string getItemPointPrice(string productnumber, string connString)
        {
            string queryString = "select FROMREWARDPOINTAMOUNTQTY from ax.RETAILLOYALTYREDEEMSCHEMELINE a join ax.RETAILGROUPMEMBERLINE b on b.RECID = a.TORETAILGROUPMEMBERLINE join ax.ECORESPRODUCT c on c.RECID = b.PRODUCT where a.TOREWARDTYPE = '2' and c.DISPLAYPRODUCTNUMBER = '" + productnumber + "'";

            return getString("FROMREWARDPOINTAMOUNTQTY", queryString, connString);
        }
        #endregion  

        #region Private Base
        private DataTable getData(string queryStr, string connString)
        {
            DataTable result = new DataTable();

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string queryString = queryStr;

                using (SqlCommand command = new SqlCommand(queryStr, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        result.Load(reader);
                    }
                }
            }
            catch (SqlException)
            {
                return result;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return result;
        }

        private void getDataSP(string queryStr, string connString, ref DataTable dt)
        {
            DataTable result = new DataTable();

            SqlConnection connection = new SqlConnection(connString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string queryString = queryStr;

                using (SqlCommand command = new SqlCommand(queryStr, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adap = new SqlDataAdapter(command);
                    adap.Fill(dt);
                    /*using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        result.Load(reader);
                    }*/
                }
            }
            catch (SqlException)
            {
                //return result;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            //return result;
        }

        private string getString(string ColumnName, string queryStr, string ConnString)
        {
            return getDataString(ColumnName, queryStr, ConnString, "");
        }

        private string getDataString(string ColumnName, string queryStr, string ConnString, string DefaultItem)
        {
            string strResult = DefaultItem;
            DataTable result = getData(queryStr, ConnString);
            if (result.Rows.Count > 0)
                strResult = result.Rows[0][ColumnName].ToString();
            return strResult;
        }

        bool executeNonQuery(string queryStr, string ConnString)
        {
            SqlConnection connection = new SqlConnection(ConnString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                string queryString = queryStr;

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    int result = command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
        #endregion
    }
}
