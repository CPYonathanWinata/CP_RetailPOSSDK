/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using LSRetailPosis.DataAccess.DataUtil;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Dimension
{
    [Export(typeof(IDimension))]
    public class Dimension : IDimension
    {
        // Get all text through the Translation function in the ApplicationLocalizer
        // TextID's for Dimension are reserved at 50100 - 50149

        [Import]
        public IApplication Application { get; set; }

        #region IDimension Members

        const string itemIdSelectQuery = @"DECLARE @tvp_ProductIds [crt].[RECORDIDTABLETYPE]
            INSERT INTO @tvp_ProductIds (RECID)
            SELECT TOP 1 PRODUCT FROM [ax].INVENTTABLE WHERE ITEMID = @ITEMID AND DATAAREAID = @DATAAREAID";
        
        const string variantIdSelectQuery = @"DECLARE @tvp_ProductIds [crt].[RECORDIDTABLETYPE]
            INSERT INTO @tvp_ProductIds (RECID)
            SELECT TOP 1 INVENT.PRODUCT FROM [ax].INVENTTABLE INVENT
            JOIN [ax].INVENTDIMCOMBINATION INVNETDIMCOMB ON INVENT.ITEMID = INVNETDIMCOMB.ITEMID AND INVENT.DATAAREAID = INVNETDIMCOMB.DATAAREAID
            WHERE INVNETDIMCOMB.RETAILVARIANTID = @RETAILVARIANTID AND INVNETDIMCOMB.DATAAREAID = @DATAAREAID";

        const string querySelectClause = @" 
            SELECT TR.NAME AS DESCRIPTION, IDC.RETAILVARIANTID AS VARIANTID, IDC.DISTINCTPRODUCTVARIANT, IDC.INVENTDIMID,
            I.INVENTSIZEID AS SIZEID, I.INVENTCOLORID AS COLORID, I.INVENTSTYLEID AS STYLEID, I.CONFIGID AS CONFIGID,
            ISNULL(DVTC.NAME, '') AS COLOR, ISNULL(DVTSZ.NAME, '') AS SIZE, ISNULL(DVTST.NAME, '') AS STYLE, 
            ISNULL(DVTCFG.NAME, '') AS CONFIG, B.ITEMBARCODE, ISNULL(B.UNITID, '') AS UNITID,
            DVC.RETAILDISPLAYORDER AS COLORDISPLAYORDER, DVSZ.RETAILDISPLAYORDER AS SIZEDISPLAYORDER, DVST.RETAILDISPLAYORDER AS STYLEDISPLAYORDER
            FROM [crt].GETASSORTEDPRODUCTS(@STORERECID, @STOREDATE, 0, 0, 1, @tvp_ProductIds) AIT 
            INNER JOIN [ax].INVENTDIMCOMBINATION IDC ON IDC.ITEMID = AIT.ITEMID AND IDC.DATAAREAID = @DATAAREAID 
	            AND (IDC.DISTINCTPRODUCTVARIANT = AIT.VARIANTID OR AIT.VARIANTID = 0)
            INNER JOIN INVENTDIM I ON I.INVENTDIMID = IDC.INVENTDIMID AND I.DATAAREAID = IDC.DATAAREAID

            LEFT OUTER JOIN ECORESCOLOR ON ECORESCOLOR.NAME = I.INVENTCOLORID
            LEFT OUTER JOIN ECORESPRODUCTMASTERCOLOR ON (ECORESPRODUCTMASTERCOLOR.COLOR = ECORESCOLOR.RECID)
            AND (ECORESPRODUCTMASTERCOLOR.COLORPRODUCTMASTER = AIT.PRODUCTID)
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMENSIONVALUE DVC ON DVC.RECID = ECORESPRODUCTMASTERCOLOR.RECID
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVTC ON DVTC.PRODUCTMASTERDIMENSIONVALUE = DVC.RECID AND DVTC.LANGUAGEID = @CULTUREID

            LEFT OUTER JOIN ECORESSIZE ON ECORESSIZE.NAME = I.INVENTSIZEID
            LEFT OUTER JOIN ECORESPRODUCTMASTERSIZE ON (ECORESPRODUCTMASTERSIZE.SIZE_ = ECORESSIZE.RECID)
            AND (ECORESPRODUCTMASTERSIZE.SIZEPRODUCTMASTER = AIT.PRODUCTID)
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMENSIONVALUE DVSZ ON DVSZ.RECID = ECORESPRODUCTMASTERSIZE.RECID
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVTSZ ON DVTSZ.PRODUCTMASTERDIMENSIONVALUE = DVSZ.RECID AND DVTSZ.LANGUAGEID = @CULTUREID

            LEFT OUTER JOIN ECORESSTYLE ON ECORESSTYLE.NAME = I.INVENTSTYLEID
            LEFT OUTER JOIN ECORESPRODUCTMASTERSTYLE ON (ECORESPRODUCTMASTERSTYLE.STYLE = ECORESSTYLE.RECID)
            AND (ECORESPRODUCTMASTERSTYLE.STYLEPRODUCTMASTER = AIT.PRODUCTID)
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMENSIONVALUE DVST ON DVST.RECID = ECORESPRODUCTMASTERSTYLE.RECID
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVTST ON DVTST.PRODUCTMASTERDIMENSIONVALUE = DVST.RECID AND DVTST.LANGUAGEID = @CULTUREID

            LEFT OUTER JOIN ECORESCONFIGURATION ON ECORESCONFIGURATION.NAME = I.CONFIGID
            LEFT OUTER JOIN ECORESPRODUCTMASTERCONFIGURATION ON (ECORESPRODUCTMASTERCONFIGURATION.CONFIGURATION = ECORESCONFIGURATION.RECID)
            AND (ECORESPRODUCTMASTERCONFIGURATION.CONFIGPRODUCTMASTER = AIT.PRODUCTID)
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMENSIONVALUE DVCFG ON DVCFG.RECID = ECORESPRODUCTMASTERCONFIGURATION.RECID
            LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVTCFG ON DVTCFG.PRODUCTMASTERDIMENSIONVALUE = DVCFG.RECID AND DVTCFG.LANGUAGEID = @CULTUREID

            LEFT OUTER JOIN INVENTITEMBARCODE B ON IDC.RETAILVARIANTID = B.RBOVARIANTID AND IDC.DATAAREAID = B.DATAAREAID

            LEFT JOIN ECORESPRODUCTTRANSLATION AS TR ON IDC.DISTINCTPRODUCTVARIANT = TR.PRODUCT AND TR.LANGUAGEID = @CULTUREID";

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Caller is responsible for disposing returned object</remarks>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        public System.Data.DataTable GetDimensions(string itemId)
        {
            LSRetailPosis.ApplicationLog.Log("Dimension.GetDimensions", "Get dimensions", LSRetailPosis.LogTraceLevel.Trace);

            SqlConnection connection = Application.Settings.Database.Connection;

            try
            {
                string sql = itemIdSelectQuery + querySelectClause + @" WHERE IDC.ITEMID = @ITEMID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ITEMID", itemId);
                    command.Parameters.AddWithValue("@DATAAREAID", Application.Settings.Database.DataAreaID);
                    command.Parameters.AddWithValue("@STORERECID", LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId);
                    command.Parameters.AddWithValue("@STOREDATE", DateTime.Today);
                    command.Parameters.AddWithValue("@CULTUREID", Thread.CurrentThread.CurrentUICulture.Name);

                    if (connection.State != ConnectionState.Open) { connection.Open(); }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        System.Data.DataTable table = new System.Data.DataTable();
                        adapter.Fill(table);

                        return table;
                    }
                }
            }
            catch (Exception x)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        public void GetDimensionForVariant(DE.IDimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException("dimension");
            }

            LSRetailPosis.ApplicationLog.Log("Dimension.GetDimensionForVariant", "Get dimension for variant", LSRetailPosis.LogTraceLevel.Trace);

            try
            {
                string sql = variantIdSelectQuery + querySelectClause + @" WHERE IDC.RETAILVARIANTID = @RETAILVARIANTID";

                using (DataTable layoutTable = new DBUtil(Application.Settings.Database.Connection).GetTable(sql,
                    new SqlParameter("@RETAILVARIANTID", dimension.VariantId),
                    new SqlParameter("@DATAAREAID", Application.Settings.Database.DataAreaID),
                    new SqlParameter("@STORERECID", LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId),
                    new SqlParameter("@STOREDATE", DateTime.Today),
                    new SqlParameter("@CULTUREID", Thread.CurrentThread.CurrentUICulture.Name)))
                {
                    if (layoutTable.Rows.Count > 0)
                    {
                        dimension.SizeId        = layoutTable.Rows[0]["SIZEID"].ToString();
                        dimension.ColorId       = layoutTable.Rows[0]["COLORID"].ToString();
                        dimension.StyleId       = layoutTable.Rows[0]["STYLEID"].ToString();
                        dimension.ConfigId      = layoutTable.Rows[0]["CONFIGID"].ToString();
                        dimension.SizeName      = layoutTable.Rows[0]["SIZE"].ToString();
                        dimension.ColorName     = layoutTable.Rows[0]["COLOR"].ToString();
                        dimension.StyleName     = layoutTable.Rows[0]["STYLE"].ToString();
                        dimension.ConfigName    = layoutTable.Rows[0]["CONFIG"].ToString();
                        dimension.DistinctProductVariantId = (Int64)layoutTable.Rows[0]["DISTINCTPRODUCTVARIANT"];
                        dimension.InventDimId = layoutTable.Rows[0]["INVENTDIMID"].ToString();
                    }
                }
            }
            catch (Exception x)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                throw;
            }
        }

        #endregion
    }
}
