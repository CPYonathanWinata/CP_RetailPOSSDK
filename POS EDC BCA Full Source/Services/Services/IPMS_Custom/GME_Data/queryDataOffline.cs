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

namespace GME_Custom.GME_Data
{
    public class queryDataOffline
    {
        queryData data = new queryData();

        public bool insertGuestCustomerOfflineSNF(string firstName, string phoneNumber, string email, int status, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = data.getChannelStore(posTransaction.StoreId, connString);

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

            channel = data.getChannelStore(posTransaction.StoreId, connString);

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
                                                                        [REPLICATIONCOUNTER],  --18
                                                                        [DATAAREAID],  --19
                                                                        [SKINTYPE],  --20
                                                                        [STORE],  --21
                                                                        [TERMINAL],  --22
                                                                        [TRANSACTIONDATE],  --23
                                                                        [TRANSACTIONID],  --24
                                                                        [TRANSACTIONTIME],  --25
                                                                        [UNITPRICE],  --26
                                                                        [VOUCHERNUMBER],  --27
                                                                        [ROWVERSION],  --28
                                                                        [STATUS],  --29
                                                                        [ENROLLTYPE], --30
                                                                        [IDENTIFIERIDREPLACEMENT] --31
                                                                        )
                                                                        VALUES (@PARM1,@PARM2,@PARM3,@PARM4,@PARM5,@PARM6,@PARM7,@PARM8,@PARM9,@PARM10,@PARM11,@PARM12,@PARM13,@PARM14,@PARM15,
                                                                                @PARM16,@PARM17,@PARM18,@PARM19,@PARM20,@PARM21,@PARM22,@PARM23,@PARM24,@PARM25,@PARM26,@PARM27,@PARM28,@PARM29,@PARM30,@PARM31)";

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
                        } else
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

        public bool insertCustomerTransOfflineEnroll(string customerId, string firstName, string LastName, string gender, string cardNumber, DateTime birthDate,
                                                int skinType, int enrollType, string email, string phoneNumber, IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = data.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_CUSTOMERTRANSACTIIONOFFLINE] ([CUSTOMERID], --1
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
                    sqlcmd.Parameters.Add(new SqlParameter("@PARM4", gender));
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
                WHERE a.STOREID = '" + storeId + "' AND a.TERMINALID = '" + terminalId + "' ORDER BY a.SEQUENCE DESC";

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

        public bool insertCustomerTransOfflineGuest(string customerId, string firstName, string LastName, string cardNumber, string email, string phoneNumber, 
                                                    IPosTransaction posTransaction, string connString)
        {
            string channel = string.Empty;

            channel = data.getChannelStore(posTransaction.StoreId, connString);

            string commandString = @"INSERT INTO [ax].[TBS_CUSTOMERTRANSACTIIONOFFLINE] ([CUSTOMERID], --1
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
            catch (SqlException ex)
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
            catch (SqlException ex)
            {
                return false;
            }
        }
        #endregion
    }
}
