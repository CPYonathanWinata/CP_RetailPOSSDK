/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Item
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Linq;

    using LSRetailPosis.DataAccess;
    using LSRetailPosis.DataAccess.DataUtil;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction.Line.SaleItem;

    using Microsoft.Dynamics.Retail.Diagnostics;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.Item.WinFormsTouch;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    using Services = Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.POSProcesses.Common;

    [Export(typeof(Services.IItem))]
    public class Item : Services.IItem
    {
        // Get all text through the Translation function in the ApplicationLocalizer
        //
        // TextID's for the Item service are reserved at 61000 - 61999
        // TextID's for the following modules are as follows:
        //
        // Item.cs:                                      x - x.  The last in use: x
        // WinFormsTouch/frmItemSearch.cs:               61500 - 61599
        // winformsKeyboard/frmItemSearch.cs:            61600 - 61699
        // WinformsTouch/frmSerialIdSearch.cs:           61700 - 61799
        // WinformsKeyboard/frmSerialIdSearch.cs         61800 - 61899

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
       [Import]
       public IApplication Application { get; set; }

        private IUtility Utility
        {
            get { return this.Application.BusinessLogic.Utility; }
        }

        #region IItem Members

        /// <summary>
        /// Add all information about the item.
        /// </summary>
        /// <param name="saleLineItem">The sale line item.</param>
        public void ProcessItem(ISaleLineItem saleLineItem)
        {
            ProcessItem(saleLineItem, false);
        }

        /// <summary>
        /// Add all information about the item..
        /// </summary>
        /// <param name="saleLineItem">The sale line item.</param>
        /// <param name="bypassSerialNumberEntry">if set to <c>true</c> [bypass serial number entry].</param>
        public void ProcessItem(ISaleLineItem saleLineItem, bool bypassSerialNumberEntry)
        {
            SaleLineItem lineItem = (SaleLineItem)saleLineItem;
            if (lineItem == null)
            {
                NetTracer.Warning("saleLineItem parameter is null");
                throw new ArgumentNullException("saleLineItem");
            }

            try
            {
                if (!string.IsNullOrEmpty(lineItem.ItemId))
                {
                    GetInventTableInfo(ref lineItem);

                    // Set the Virtual Catalog Product flag ttrue if the product is Virtual catalog 
                    GetVirtualCatalogProductInfo(lineItem); 

                    GetInventTableModuleInfo(ref lineItem);
                    GetRBOInventTableInfo(ref lineItem);
                    GetInventDimInfo(ref lineItem);
                    if (!bypassSerialNumberEntry)
                    {
                        this.UpdateSerialNumberInfo(lineItem);
                    }                    
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }
        
        public bool ItemSearch(ref string selectedItemId, int numberOfDisplayedRows)
        {
            return ItemSearch(ref selectedItemId, numberOfDisplayedRows, null);
        }

        /// <summary>
        /// Displays the Item Search dialog
        /// </summary>
        /// <param name="selectedItemId">Selected item Id.</param>
        /// <param name="numberOfDisplayedRows">Maximum number of rows displayed.</param>
        /// <param name="categoryId">The product category Id.</param>
        /// <returns>Returns false if the user pressed cancel. Returns true if the user did choose to sell a selected item.  In this case the selecedItemId contains the item id of the item being sold.</returns>
        public bool ItemSearch(ref string selectedItemId, int numberOfDisplayedRows, string categoryId)
        {
            try
            {
                DialogResult dialogResult = this.Application.Services.Dialog.ItemSearch(numberOfDisplayedRows, ref selectedItemId, categoryId);

                // Quit if cancel is pressed...
                if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Displays the Item Search dialog.
        /// Returns false if the user pressed cancel.
        /// Returns true if the user did choose to sell a selected item.  
        /// In this case the selecedItemId contains the item id of the item being sold.
        /// </summary>
        /// <param name="selectedItemId"></param>
        /// <param name="numberOfDisplayedRows"></param>
        /// <returns></returns>
        //public bool ItemSearch(ref string selectedItemId, int numberOfDisplayedRows)
        //{
        //    try
        //    {
        //        DialogResult dialogResult = this.Application.Services.Dialog.ItemSearch(numberOfDisplayedRows, ref selectedItemId);

        //        // Quit if cancel is pressed...
        //        if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get Product data from AX searched through Transaction Service
        /// </summary>
        /// <param name="itemIdRange">If blank, retrieves list for ALL products</param>
        /// <returns>Returns downloaded Product Data in XML format.</returns>
        public XDocument GetProductData(string itemIdRange)
        {
            Dictionary<string, string> idDescription = new Dictionary<string, string>();
            ReadOnlyCollection<object> containerArray = null;
            IList xmlIdDescription = null;
            string productDataXmlString = null;
            XDocument tmpXDocument = null;

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (this.Application.TransactionServices.CheckConnection())
                {
                    // Search for item in Head Office through the Transaction Services...
                    containerArray = this.Application.TransactionServices.Invoke("GetProductData", itemIdRange, true);
                    if (containerArray != null)
                    {
                        bool retValue = (bool)containerArray[1];
                        string comment = containerArray[2].ToString();
                        if (retValue)
                        {
                            productDataXmlString = containerArray[3].ToString();
                            xmlIdDescription = (IList)containerArray[4];
                            if (xmlIdDescription != null)
                            {
                                for (int i = 4; i < xmlIdDescription.Count; i++)
                                {
                                    idDescription.Add(Convert.ToString(xmlIdDescription[i]), Convert.ToString(xmlIdDescription[i + 1]));
                                    i++;
                                }
                            }
                            if (productDataXmlString != null)
                            {
                                tmpXDocument = UpdateTableNamesWithDataSourceMap(productDataXmlString, idDescription);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw new LSRetailPosis.PosisException(13010, ex);
            }
            return tmpXDocument;
       }
        /// <summary>
        /// Gets list of Products from Product search in AX
        /// </summary>
        /// <param name="retailChannelRecId">Channel record id.</param>
        /// <param name="keyword">The product search string.</param>
        /// <param name="_startPosition">start of the result set.</param>
        /// <param name="_pageSize">The page size of the results.</param>
        /// <param name="_orderByField">The order by field name.</param>
        /// <param name="_sortOrder">The sort order of the results.</param>
        /// <param name="_includeTotal">The Total Count of the results.</param>
        /// <param name="_languageId">The language Id of results.</param>
        /// <param name="_otherChannelRecId">The record ID of other channel.</param>
        /// <param name="_catalogRecId">The record ID of the catalog to search products from.</param>
        /// <param name="_attributeRecIdRangeValue">The attribute record ID range value to filter product attribute values by.</param>
        /// <param name="itemsTable">The items table to store the results.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "6"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Argument is sufficiently validated"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Argument is sufficiently validated")]
        public void GetProductsByKeyword(
            Int64 retailChannelRecId,
            string keyword,
            Int64 _startPosition,
            Int64 _pageSize,
            string _orderByField,
            string _sortOrder,
            bool _includeTotal,
            string _languageId,
            Int64 _otherChannelRecId,
            Int64 _catalogRecId,
            string _attributeRecIdRangeValue,
            ref DataTable itemsTable)
        {
            ReadOnlyCollection<object> containerArray;
            string productDataXmlString = null;
            itemsTable = new DataTable();

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (this.Application.TransactionServices.CheckConnection())
                {
                    itemsTable.Columns.Add("ITEMID", typeof(string));
                    itemsTable.Columns.Add("ITEMNAME", typeof(string));
                    itemsTable.Columns.Add("ITEMPRICE", typeof(string));
                    itemsTable.Columns.Add("UNITOFMEASURE", typeof(string));
                    itemsTable.Columns.Add("INVENTPRODUCTTYPE_BR", typeof(string));
                    itemsTable.Columns.Add("ROWNUMBER", typeof(int));

                    // Search for item in Head Office through the Transaction Services...
                    containerArray = this.Application.TransactionServices.Invoke(
                        "GetProductsByKeyword",
                        retailChannelRecId,     //_currentChannelRecId
                        keyword,                // Search keyword
                        _startPosition,         // _startPosition
                        _pageSize,
                        _orderByField,
                        _sortOrder,
                        _includeTotal,                  // _includeTotal
                        Thread.CurrentThread.CurrentUICulture.Name,     // _languageId
                        _otherChannelRecId,                      // _otherChannelRecId
                        _catalogRecId,                      // _catalogRecId
                        _attributeRecIdRangeValue                    // _attributeRecIdRangeValue
                        );

                    if (containerArray != null)
                    {
                        bool retValue = (bool)containerArray[1];

                        if (retValue)
                        {
                            productDataXmlString = containerArray[3].ToString();
                            if (productDataXmlString != null)
                            {
                                RemoveXmlDeclaration(ref productDataXmlString);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(productDataXmlString))
                    {
                        XDocument Xdoc = XDocument.Parse(productDataXmlString);
                        IEnumerable<XElement> itemsResult = Xdoc.Elements("Products").Descendants();

                        foreach (XElement el in itemsResult)
                        {

                            if (itemsTable != null)
                            {
                                DataRow row = itemsTable.NewRow();
                                row["ITEMID"] = el.Attribute("ItemId").AttributeValueNull();
                                row["ITEMNAME"] = el.Attribute("Name").AttributeValueNull();
                                row["ITEMPRICE"] = el.Attribute("Price").AttributeValueNull();
                                row["ROWNUMBER"] = itemsTable.Rows.Count + 1;
                                row["UNITOFMEASURE"] = el.Attribute("UnitOfMeasure").AttributeValueNull();
                                row["INVENTPRODUCTTYPE_BR"] = string.Empty;
                                itemsTable.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw new LSRetailPosis.PosisException(13010, ex);
            }
        }
        /// <summary>
        /// Gets list of Products from Product search in AX
        /// </summary>
        /// <param name="retailChannelRecId">Retail channel record id.</param>
        /// <param name="keyword">keyword string to search in Ax.</param>
        /// <param name="_startPosition">The starting record position.</param>
        /// <param name="_pageSize">The page size of results.</param>
        /// <param name="_orderByField">The order by field name.</param>
        /// <param name="_sortOrder">The sort order of the results.</param>
        /// <param name="itemsTable">The items table to store the results.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "6"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Argument is sufficiently validated"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Argument is sufficiently validated")]
        public void GetProductsByKeyword(
            Int64 retailChannelRecId,
            string keyword,
            Int64 _startPosition,
            Int64 _pageSize,
            string _orderByField,
            string _sortOrder,
            ref DataTable itemsTable)
        {
            ReadOnlyCollection<object> containerArray;
            string productDataXmlString = null;
            itemsTable = new DataTable();

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (this.Application.TransactionServices.CheckConnection())
                {
                    itemsTable.Columns.Add("ITEMID", typeof(string));
                    itemsTable.Columns.Add("ITEMNAME", typeof(string));
                    itemsTable.Columns.Add("ITEMPRICE", typeof(string));
                    itemsTable.Columns.Add("UNITOFMEASURE", typeof(string));
                    itemsTable.Columns.Add("INVENTPRODUCTTYPE_BR", typeof(string));
                    itemsTable.Columns.Add("ROWNUMBER", typeof(int));

                    // Search for item in Head Office through the Transaction Services...
                    containerArray = this.Application.TransactionServices.Invoke(
                        "GetProductsByKeyword",
                        retailChannelRecId,
                        keyword,
                        _startPosition,
                        _pageSize,
                        _orderByField,
                        _sortOrder,
                        Thread.CurrentThread.CurrentUICulture.Name);

                    if (containerArray != null)
                    {
                        bool retValue = (bool)containerArray[1];

                        if (retValue)
                        {
                            productDataXmlString = containerArray[3].ToString();
                            if (productDataXmlString != null)
                            {
                                RemoveXmlDeclaration(ref productDataXmlString);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(productDataXmlString))
                    {
                        XDocument Xdoc = XDocument.Parse(productDataXmlString);
                        IEnumerable<XElement> itemsResult = Xdoc.Elements("Products").Descendants();

                        foreach (XElement el in itemsResult)
                        {
                            DataRow row = itemsTable.NewRow();
                            row["ITEMID"] = el.Attribute("ItemId").AttributeValueNull();
                            row["ITEMNAME"] = el.Attribute("Name").AttributeValueNull();
                            row["ITEMPRICE"] = el.Attribute("Price").AttributeValueNull();
                            row["ROWNUMBER"] = itemsTable.Rows.Count + 1;
                            row["UNITOFMEASURE"] = el.Attribute("UnitOfMeasure").AttributeValueNull();
                            row["INVENTPRODUCTTYPE_BR"] = string.Empty;
                            itemsTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw new LSRetailPosis.PosisException(13010, ex);
            }
        }

        /// <summary>
        /// Gets the product name from AX
        /// </summary>
        /// <param name="itemID">ItemID of the product to search in AX.</param>
        /// <returns>Returns the product name string for matched item ID in AX.</returns>
        public string GetProductNameByItemId(string itemID)
        {
            ReadOnlyCollection<object> containerArray;
            string productDataXmlString = null;
            string itemName = string.Empty;
            const int startPosition = 1;
            const int pageSize = 100;
            const string orderByField = "Name";
            const string sortOrder = "Ascending";

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                if (this.Application.TransactionServices.CheckConnection())
                {
                    // Search for item in Head Office through the Transaction Services...
                    containerArray = this.Application.TransactionServices.Invoke(
                        "GetProductsByKeyword",
                        ApplicationSettings.Terminal.StorePrimaryId,
                        itemID,
                        startPosition,
                        pageSize,
                        orderByField,
                        sortOrder,
                        Thread.CurrentThread.CurrentUICulture.Name);

                    if (containerArray != null)
                    {
                        bool retValue = (bool)containerArray[1];

                        if (retValue)
                        {
                            productDataXmlString = containerArray[3].ToString();
                            if (productDataXmlString != null)
                            {
                                RemoveXmlDeclaration(ref productDataXmlString);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(productDataXmlString))
                    {
                        XDocument Xdoc = XDocument.Parse(productDataXmlString);
                        IEnumerable<XElement> itemsResult = Xdoc.Elements("Products").Descendants();

                        foreach (XElement el in itemsResult)
                        {
                            if (el.Attribute("ItemId").AttributeValueNull() == itemID)
                            {
                                itemName = el.Attribute("Name").AttributeValueNull();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Do Nothing and return empty string for item Name
            }

            return itemName;
        }

        /// <summary>
        /// Updates the serial number for sales line.
        /// </summary>
        /// <remarks>This function asks the user to confirm serial numbers for sales line, if required.</remarks>
        /// <param name="saleLineItem">The sales line.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when saleLineItem is null</exception>
        public void UpdateSerialNumberInfo(ISaleLineItem saleLineItem)
        {
            if (saleLineItem == null)
            {
                throw new ArgumentNullException("saleLineItem");
            }
            
            SqlConnection connection = this.Application.Settings.Database.Connection;

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                bool hasTrackingDimensionWithSerialAlive = false;
                bool hasTrackingDimensionWithSerialSales = false;
                bool allowBlank = true;

                // Find out whether the item's tracking dimension group has active serial number setup. Serial number is one of field in tracking dimension group
                // and the value is 5. The rest are invent dimensions are listed below (including product dimension/storage dimension/tracking dimension. Ideally 
                // we should define ENUM for this, but we don't use most of these.

                // 1. Dimension No.
                // 2. Batch number
                // 3. Location
                // 4. Pallet ID
                // 5. Serial number
                // 6. Warehouse
                // 7. Configuration
                // 8. Size
                // 9. Color

                // Table ECORESTRACKINGDIMENSIONGROUPITEM will always have an entry if Tracking dimension group is assigned either on the product/ release product. 
                string queryString = @"SELECT I.ITEMID, DGP.TRACKINGDIMENSIONGROUP, DGF.DIMENSIONFIELDID, DGF.ISACTIVE, DGF.ISALLOWBLANKISSUEENABLED, DGF.ISSALESPROCESSACTIVATED
                                        FROM crt.GETASSORTEDINVENTITEM(@STORERECID, GETUTCDATE(), @ITEMID) I 
		                                    INNER JOIN ax.ECORESTRACKINGDIMENSIONGROUPITEM DGP ON I.ITEMID = DGP.ITEMID AND I.DATAAREAID = DGP.ITEMDATAAREAID                               
		                                    INNER JOIN ax.ECORESTRACKINGDIMENSIONGROUPFLDSETUP DGF ON DGP.TRACKINGDIMENSIONGROUP = DGF.TRACKINGDIMENSIONGROUP
                                        WHERE DGF.DIMENSIONFIELDID = 5 AND (DGF.ISACTIVE = 1 OR DGF.ISSALESPROCESSACTIVATED = 1)";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;
                    command.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar, 4).Value = Application.Settings.Database.DataAreaID;
                    command.Parameters.Add("@STORERECID", SqlDbType.BigInt).Value = LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId;

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        reader.Read();

                        if (reader.HasRows)
                        {
                            hasTrackingDimensionWithSerialAlive = Utility.ToBool(reader.GetInt32(reader.GetOrdinal("ISACTIVE")));
                            hasTrackingDimensionWithSerialSales = Utility.ToBool(reader.GetInt32(reader.GetOrdinal("ISSALESPROCESSACTIVATED")));
                            allowBlank = Utility.ToBool(reader.GetInt32(reader.GetOrdinal("ISALLOWBLANKISSUEENABLED")));
                        }
                    }
                }

                if (hasTrackingDimensionWithSerialSales)
                {
                    // With SalesProcessActivated flag (aka "light-weight S/N support"), we ONLY allow manual input of S/N.
                    InputSerialNumber(saleLineItem, allowBlank, this.Application);
                }
                else if (hasTrackingDimensionWithSerialAlive)
                {
                    // Otherwise, use the full S/N prompt/select workflow.
                    string querySerial = " SELECT ISNULL(INVENTSERIALID, '') AS INVENTSERIALID, ISNULL(RFIDTAGID, '') AS RFIDTAGID FROM INVENTSERIAL WHERE DATAAREAID = @DATAAREAID2 ";

                    if (!string.IsNullOrEmpty(saleLineItem.RFIDTagId))
                    {
                        querySerial += " AND RFIDTAGID = @ITEMID ";
                    }
                    else
                    {
                        querySerial += " AND ITEMID = @ITEMID ";
                    }

                    using (SqlCommand serialCommand = new SqlCommand(querySerial, connection))
                    {
                        if (!string.IsNullOrEmpty(saleLineItem.RFIDTagId))
                        {
                            serialCommand.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 24).Value = saleLineItem.RFIDTagId;
                        }
                        else
                        {
                            serialCommand.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;
                        }

                        serialCommand.Parameters.Add("@DATAAREAID2", SqlDbType.NVarChar, 4).Value = Application.Settings.Database.DataAreaID;

                        using (SqlDataReader serialReader = serialCommand.ExecuteReader())
                        {
                            using (DataTable serialNo = new DataTable())
                            {
                                serialNo.Load(serialReader);

                                if (serialNo.Rows.Count == 0)
                                {
                                    InputSerialNumber(saleLineItem, allowBlank, this.Application);
                                }
                                else if (serialNo.Rows.Count == 1)
                                {
                                    saleLineItem.SerialId = (string)serialNo.Rows[0]["INVENTSERIALID"];
                                    saleLineItem.RFIDTagId = (string)serialNo.Rows[0]["RFIDTAGID"];
                                }
                                else
                                {
                                    SelectSerialNumber(saleLineItem, allowBlank, this.Application);
                                }
                            }
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
            }
        }

        #endregion

        # region Private methods

        private static string RemoveXmlDeclaration(ref string xml)
        {
            // Remove Xml declaration
            Match xmlDeclaration = Regex.Match(xml, @"<\?xml.*\?>");
            if (xmlDeclaration.Success)
            {
                xml = xml.Replace(xmlDeclaration.Value, string.Empty);
            }

            return xml;
        }

        private static XDocument UpdateTableNamesWithDataSourceMap(string productsXml, IDictionary<string, string> map)
        {
            // Remove the namespace from the XML document to allow us to parse it in SQL.
            System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
            dom.LoadXml(productsXml);
            dom.LoadXml(dom.OuterXml.Replace(dom.DocumentElement.NamespaceURI, string.Empty));
            dom.DocumentElement.RemoveAllAttributes();

            XDocument xmlDocument = XDocument.Parse(dom.OuterXml);
            if (xmlDocument != null)
            {
                var elements = xmlDocument.Root.Descendants();
                foreach (XElement element in elements)
                {
                    string elementName = element.Name.LocalName;
                    if (map.ContainsKey(elementName))
                    {
                        string tableName = map[elementName];
                        element.Name = XName.Get(tableName, element.Name.NamespaceName);
                    }
                }
            }

            return xmlDocument;
        }

        private void GetInventTableInfo(ref SaleLineItem saleLineItem)
        {
            SqlConnection connection = Application.Settings.Database.Connection;

            try
            {
                string queryString = @"SELECT I.[ITEMTYPE], ISNULL(TR.NAME, I.ITEMNAME) AS ITEMNAME, I.[NAMEALIAS], I.[ITEMGROUPID], I.[DIMGROUPID], I.[PRODUCT] 
	                                    FROM crt.GETASSORTEDINVENTITEM(@STORERECID, GETUTCDATE(), @ITEMID) I 
		                                    INNER JOIN ax.ECORESPRODUCT AS PR ON PR.RECID = I.PRODUCT 
		                                    LEFT OUTER JOIN ax.ECORESPRODUCTTRANSLATION AS TR ON PR.RECID = TR.PRODUCT AND TR.LANGUAGEID = @CULTUREID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@STORERECID", SqlDbType.BigInt).Value = LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId;
                    command.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;
                    command.Parameters.Add("@CULTUREID", SqlDbType.NVarChar, 7).Value = ApplicationSettings.Terminal.CultureName;

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        reader.Read();

                        if (reader.HasRows)
                        {
                            saleLineItem.ItemType = (BaseSaleItem.ItemTypes)reader.GetInt32(reader.GetOrdinal("ITEMTYPE"));
                            saleLineItem.ItemGroupId = reader.GetString(reader.GetOrdinal("ITEMGROUPID"));
                            saleLineItem.DimensionGroupId = reader.GetString(reader.GetOrdinal("DIMGROUPID"));
                            saleLineItem.DescriptionAlias = reader.GetString(reader.GetOrdinal("NAMEALIAS"));
                            saleLineItem.ProductId = reader.GetInt64(reader.GetOrdinal("PRODUCT"));

                            if (string.IsNullOrEmpty(saleLineItem.Description))
                            {
                                saleLineItem.Description = reader.GetString(reader.GetOrdinal("ITEMNAME"));
                            }

                            saleLineItem.Found = true;
                        }
                        else
                        {
                            saleLineItem.Found = false;

                            // Retrieve product information from AX if saleLineItem is not available locally.
                            saleLineItem.Description = GetProductNameByItemId(saleLineItem.ItemId);
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
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Grandfather as this causes early loop termination")]
        private void GetInventTableModuleInfo(ref SaleLineItem saleLineItem)
        {
            SqlConnection connection = Application.Settings.Database.Connection;

            try
            {
                string queryString = "SELECT M.[LINEDISC],M.[MULTILINEDISC],M.[ENDDISC],ISNULL(M.[UNITID], '') AS UNITID, M.MODULETYPE, M.TAXITEMGROUPID, M.MARKUP, M.ALLOCATEMARKUP, M.MARKUPGROUPID, M.PRICEQTY, M.PRICE "
                    + "FROM INVENTTABLEMODULE M "
                    + "WHERE M.ITEMID=@ITEMID AND M.DATAAREAID=@DATAAREAID ORDER BY M.MODULETYPE ASC";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;
                    command.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar, 4).Value = Application.Settings.Database.DataAreaID;

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        saleLineItem.LineDiscountGroup = string.Empty;
                        saleLineItem.MultiLineDiscountGroup = string.Empty;
                        saleLineItem.IncludedInTotalDiscount = false;

                        while (reader.Read())
                        {
                            if ((int)reader["MODULETYPE"] == 2)
                            {
                                saleLineItem.BackofficeSalesOrderUnitOfMeasure = reader.GetString(reader.GetOrdinal("UNITID"));
                                saleLineItem.LineDiscountGroup = Utility.ToString(reader["LINEDISC"]);
                                saleLineItem.MultiLineDiscountGroup = Utility.ToString(reader["MULTILINEDISC"]);
                                saleLineItem.IncludedInTotalDiscount = Utility.ToBool(reader["ENDDISC"]);

                                saleLineItem.TaxGroupId = Utility.ToString(reader["TAXITEMGROUPID"]);
                                saleLineItem.TaxGroupIdOriginal = saleLineItem.TaxGroupId;
                                saleLineItem.Markup = reader.GetDecimal(reader.GetOrdinal("MARKUP"));
                                saleLineItem.MarkupGroupId = Utility.ToString(reader["MARKUPGROUPID"]);

                                saleLineItem.PriceQty = reader.GetDecimal(reader.GetOrdinal("PRICEQTY"));

                                // if value is 1, it is per unit Price charge  - for some reason, for a boolean flag, AX uses an entire int.
                                saleLineItem.AllocateMarkup = reader.GetInt32(reader.GetOrdinal("ALLOCATEMARKUP")) == 1; 

                                reader.Close();
                                UnitOfMeasureData uomData = new UnitOfMeasureData(
                                    connection,
                                    this.Application.Settings.Database.DataAreaID,
                                    LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId,
                                    this.Application);
                                saleLineItem.BackofficeSalesOrderUnitOfMeasureName = uomData.GetUnitName(saleLineItem.BackofficeSalesOrderUnitOfMeasure);

                                if (!string.IsNullOrEmpty(saleLineItem.BarcodeId) || !string.IsNullOrEmpty(saleLineItem.SalesOrderUnitOfMeasure))
                                {   // The unit of measure on the barcode always takes presedense over the unit of measure on the item itself.

                                    saleLineItem.SalesOrderUnitOfMeasureName = uomData.GetUnitName(saleLineItem.SalesOrderUnitOfMeasure);
                                    saleLineItem.UnitQtyConversion = uomData.GetUOMFactor(saleLineItem.SalesOrderUnitOfMeasure, saleLineItem.BackofficeSalesOrderUnitOfMeasure, saleLineItem);
                                }
                                else
                                {   // initialize the active unit of measure for the item to this as the default
                                    saleLineItem.SalesOrderUnitOfMeasure = saleLineItem.BackofficeSalesOrderUnitOfMeasure;

                                    // Default is SalesOrderUnitOfMeasureName == BackofficeSalesOrderUnitOfMeasureName
                                    saleLineItem.SalesOrderUnitOfMeasureName = saleLineItem.BackofficeSalesOrderUnitOfMeasureName;
                                    saleLineItem.UnitQtyConversion = (UnitQtyConversion)this.Application.BusinessLogic.Utility.CreateUnitQtyConversion();
                                }

                                // Must break out because at this point the reader has been closed
                                break;
                            }
                            else if ((int)reader["MODULETYPE"] == 0)
                            {
                                saleLineItem.InventOrderUnitOfMeasure = (string)reader["UNITID"] ?? string.Empty;
                                saleLineItem.CostPrice = (decimal)reader["PRICE"];
                            }
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
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfather")]
        private void GetRBOInventTableInfo(ref SaleLineItem saleLineItem)
        {
            SqlConnection connection = Application.Settings.Database.Connection;

            try
            {
                string queryString = @" SELECT QTYBECOMESNEGATIVE,    ZEROPRICEVALID,    NODISCOUNTALLOWED, SCALEITEM, BLOCKEDONPOS, DATETOBEBLOCKED, 
                                    DATETOACTIVATEITEM, 
                                    KEYINGINQTY, KEYINGINPRICE,    MUSTKEYINCOMMENT" +
                                    ", PROHIBITRETURN_RU " +
                                    @"FROM RETAILINVENTTABLE 
                                    WHERE ITEMID = @ITEMID AND DATAAREAID=@DATAAREAID ";

                using (SqlCommand command = new SqlCommand(queryString.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@ITEMID", saleLineItem.ItemId);
                    command.Parameters.AddWithValue("@DATAAREAID", Application.Settings.Database.DataAreaID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        reader.Read();

                        if (reader.HasRows)
                        {
                            saleLineItem.QtyBecomesNegative = ((int)reader["QTYBECOMESNEGATIVE"] != 0);
                            saleLineItem.ZeroPriceValid = ((int)reader["ZEROPRICEVALID"] != 0);
                            saleLineItem.NoDiscountAllowed = ((int)reader["NODISCOUNTALLOWED"] != 0);
                            saleLineItem.ScaleItem = ((int)reader["SCALEITEM"] != 0);
                            saleLineItem.Blocked |= ((int)reader["BLOCKEDONPOS"] == 1);
                            saleLineItem.DateToBeBlocked = (DateTime)reader["DATETOBEBLOCKED"];
                            saleLineItem.DateToActivateItem = (DateTime)reader["DATETOACTIVATEITEM"];
                            int keyingInQty = (int)reader["KEYINGINQTY"];
                            int keyingInPrice = (int)reader["KEYINGINPRICE"];

                            switch (keyingInQty)
                            {
                                case 0: saleLineItem.KeyInQuantity = KeyInQuantities.NotMandatory; break;
                                case 1: saleLineItem.KeyInQuantity = KeyInQuantities.MustKeyInQuantity; break;
                                case 2: saleLineItem.KeyInQuantity = KeyInQuantities.EnteringQuantityNotAllowed; break;
                            }

                            switch (keyingInPrice)
                            {
                                case 0: saleLineItem.KeyInPrice = KeyInPrices.NotMandatory; break;
                                case 1: saleLineItem.KeyInPrice = KeyInPrices.MustKeyInNewPrice; break;
                                case 2: saleLineItem.KeyInPrice = KeyInPrices.MustKeyInEqualHigherPrice; break;
                                case 3: saleLineItem.KeyInPrice = KeyInPrices.MustKeyInEqualLowerPrice; break;
                                case 4: saleLineItem.KeyInPrice = KeyInPrices.EnteringPriceNotAllowed; break;
                            }

                            if (reader["MUSTKEYINCOMMENT"] != System.DBNull.Value)
                            {
                                saleLineItem.MustKeyInComment = ((int)reader["MUSTKEYINCOMMENT"] == 1);
                            }
                            else
                            {
                                saleLineItem.MustKeyInComment = false;
                            }

                            saleLineItem.ProhibitReturn = ((int)reader["PROHIBITRETURN_RU"] != 0);
                        }
                        else
                        {
                            saleLineItem.QtyBecomesNegative = false;
                            saleLineItem.ZeroPriceValid = false;
                            saleLineItem.NoDiscountAllowed = false;
                            saleLineItem.ScaleItem = false;
                            saleLineItem.MustKeyInComment = false;
                            saleLineItem.ProhibitReturn = false;
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
            }
        }

        private void GetInventDimInfo(ref SaleLineItem saleLineItem)
        {
            SqlConnection connection = Application.Settings.Database.Connection;

            try
            {
                if (string.IsNullOrEmpty(saleLineItem.Dimension.VariantId))
                {
                    //If variant is not set for the item but dimensions are found the enter them manually
                    string queryString = @"SELECT ITEMID FROM crt.GETASSORTEDINVENTDIMCOMBINATIONFORITEM(@STORERECID, GETDATE(), @ITEMID)";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.Add("@STORERECID", SqlDbType.BigInt).Value = LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId;
                        command.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            reader.Read();
                            saleLineItem.Dimension.EnterDimensions = reader.HasRows;
                        }
                    }
                }
                else
                {
                    string queryString = @"
                            SELECT I.INVENTSIZEID AS SIZEID, I.INVENTCOLORID AS COLORID, I.INVENTSTYLEID AS STYLEID, I.CONFIGID AS CONFIGID, 
                    ISNULL(DVC.NAME, '') AS COLOR, ISNULL(DVSZ.NAME, '') AS SIZE, ISNULL(DVST.NAME, '') AS STYLE, 
                    ISNULL(DVCFG.NAME, '') AS CONFIG, 
                    IDC.DISTINCTPRODUCTVARIANT, IDC.INVENTDIMID
                    FROM crt.GETASSORTEDINVENTDIMCOMBINATIONFORITEM(@STORERECID, GETDATE(), @ITEMID) AS IDC
                    INNER JOIN INVENTDIM I ON I.INVENTDIMID = IDC.INVENTDIMID AND I.DATAAREAID = IDC.DATAAREAID
                    INNER JOIN crt.GETASSORTEDINVENTITEM(@STORERECID, GETUTCDATE(), @ITEMID) AIT ON AIT.ITEMID = IDC.ITEMID 

                                        LEFT OUTER JOIN ECORESCOLOR ON ECORESCOLOR.NAME = I.INVENTCOLORID
                                        LEFT OUTER JOIN ECORESPRODUCTMASTERCOLOR ON (ECORESPRODUCTMASTERCOLOR.COLOR = ECORESCOLOR.RECID)
                                        AND (ECORESPRODUCTMASTERCOLOR.COLORPRODUCTMASTER = AIT.PRODUCT)
                                        LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVC ON DVC.PRODUCTMASTERDIMENSIONVALUE = ECORESPRODUCTMASTERCOLOR.RECID AND DVC.LANGUAGEID = @CULTUREID

                                        LEFT OUTER JOIN ECORESSIZE ON ECORESSIZE.NAME = I.INVENTSIZEID
                                        LEFT OUTER JOIN ECORESPRODUCTMASTERSIZE ON (ECORESPRODUCTMASTERSIZE.SIZE_ = ECORESSIZE.RECID)
                                        AND (ECORESPRODUCTMASTERSIZE.SIZEPRODUCTMASTER = AIT.PRODUCT)
                                        LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVSZ ON DVSZ.PRODUCTMASTERDIMENSIONVALUE = ECORESPRODUCTMASTERSIZE.RECID AND DVSZ.LANGUAGEID = @CULTUREID

                                        LEFT OUTER JOIN ECORESSTYLE ON ECORESSTYLE.NAME = I.INVENTSTYLEID
                    LEFT OUTER JOIN ECORESPRODUCTMASTERSTYLE ON (ECORESPRODUCTMASTERSTYLE.STYLE = ECORESSTYLE.RECID)
                    AND (ECORESPRODUCTMASTERSTYLE.STYLEPRODUCTMASTER = AIT.PRODUCT)
                    LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVST ON DVST.PRODUCTMASTERDIMENSIONVALUE = ECORESPRODUCTMASTERSTYLE.RECID AND DVST.LANGUAGEID = @CULTUREID

                                        LEFT OUTER JOIN ECORESCONFIGURATION ON ECORESCONFIGURATION.NAME = I.CONFIGID
                                        LEFT OUTER JOIN ECORESPRODUCTMASTERCONFIGURATION ON (ECORESPRODUCTMASTERCONFIGURATION.CONFIGURATION = ECORESCONFIGURATION.RECID)
                                        AND (ECORESPRODUCTMASTERCONFIGURATION.CONFIGPRODUCTMASTER = AIT.PRODUCT)
                                        LEFT OUTER JOIN ECORESPRODUCTMASTERDIMVALUETRANSLATION DVCFG ON DVCFG.PRODUCTMASTERDIMENSIONVALUE = ECORESPRODUCTMASTERCONFIGURATION.RECID AND DVCFG.LANGUAGEID = @CULTUREID

                                        WHERE (IDC.RETAILVARIANTID = @RetailVariantId) AND (IDC.DATAAREAID = @DATAAREAID) AND (IDC.STORERECID = @STORERECID) ";

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.Add("@STORERECID", SqlDbType.BigInt).Value = LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId;
                        command.Parameters.Add("@RETAILVARIANTID", SqlDbType.NVarChar, 20).Value = saleLineItem.Dimension.VariantId;
                        command.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar, 4).Value = Application.Settings.Database.DataAreaID;
                        command.Parameters.Add("@CULTUREID", SqlDbType.NVarChar, 7).Value = Thread.CurrentThread.CurrentUICulture.Name;
                        command.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            reader.Read();

                            if (reader.HasRows)
                            {
                                saleLineItem.Dimension.ColorId = Utility.ToString(reader["COLORID"]);
                                saleLineItem.Dimension.SizeId = Utility.ToString(reader["SIZEID"]);
                                saleLineItem.Dimension.StyleId = Utility.ToString(reader["STYLEID"]);
                                saleLineItem.Dimension.ConfigId = Utility.ToString(reader["CONFIGID"]);

                                saleLineItem.Dimension.ColorName = Utility.ToString(reader["COLOR"]);
                                saleLineItem.Dimension.SizeName = Utility.ToString(reader["SIZE"]);
                                saleLineItem.Dimension.StyleName = Utility.ToString(reader["STYLE"]);
                                saleLineItem.Dimension.ConfigName = Utility.ToString(reader["CONFIG"]);
                                saleLineItem.Dimension.DistinctProductVariantId = (long)reader["DISTINCTPRODUCTVARIANT"];

                                saleLineItem.Dimension.InventDimId = Utility.ToString(reader["INVENTDIMID"]);

                                saleLineItem.Dimension.EnterDimensions = false;
                            }
                            else
                            {
                                saleLineItem.Dimension.EnterDimensions = true;
                            }
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
            }
        }

        /// <summary>
        /// Checks the temporary assortments table for virtual catalog product
        /// Sets the property of virtual catalog product on sales line item
        /// </summary>
        /// <param name="saleLineItem">The sales line item</param>
        private void GetVirtualCatalogProductInfo(SaleLineItem saleLineItem)
        {
            SqlConnection connection = Application.Settings.Database.Connection;

            try
            {
                // Search TmpAssortments table if the product is virtual catalog product
                string queryString = @"SELECT TOP 1 ITEMID FROM [crt].TMPASSORTEDPRODUCTS WHERE ITEMID=@ITEMID 
                        AND ( (ValidFrom <= GetUtcDate()) AND (GetUtcDate() <= ValidTo) )";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@ITEMID", SqlDbType.NVarChar, 20).Value = saleLineItem.ItemId;

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        reader.Read();
                        saleLineItem.IsVirtualProduct = reader.HasRows;
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
            }
        }

        /// <summary>
        /// Shows the dialog for selecting serial number and populates it to sale line item.
        /// </summary>
        /// <param name="saleLineItem">The sale line item.</param>
        /// <param name="allowBlank">True, if allow blank input.</param>
        /// <param name="application">The instance of the application object.</param>
        private static void SelectSerialNumber(ISaleLineItem saleLineItem, bool allowBlank, IApplication application)
        {
            using (frmSerialIdSearch searchDialog = new WinFormsTouch.frmSerialIdSearch(100))
            {
                searchDialog.ItemId = saleLineItem.ItemId;
                searchDialog.SelectedSerialId = saleLineItem.SerialId;

                // Show the search dialog
                bool inputValid;
                do
                {
                    inputValid = true;

                    application.ApplicationFramework.POSShowForm(searchDialog);

                    // Quit if cancel is pressed...
                    if (searchDialog.DialogResult == DialogResult.Cancel && allowBlank)
                    {
                        return;
                    }
                    else if (searchDialog.DialogResult == DialogResult.Cancel && !allowBlank)
                    {
                        inputValid = false;
                    }
                }
                while (!inputValid);

                saleLineItem.SerialId = searchDialog.SelectedSerialId;
                saleLineItem.RFIDTagId = searchDialog.SelectedRFIDTagId;
            }
        }

        private static void InputSerialNumber(ISaleLineItem saleLineItem, bool allowBlank, IApplication application)
        {
            bool inputValid = true;
            do
            {
                InputConfirmation inputConfirmation = new InputConfirmation()
                {
                    MaxLength = 20,
                    PromptText = LSRetailPosis.ApplicationLocalizer.Language.Translate(61000), // Enter serial no.
                    Text = saleLineItem.SerialId,
                    Title = saleLineItem.LineId == 0 ? 
                                saleLineItem.Description : 
                                string.Format("{0}: {1}", saleLineItem.LineId, saleLineItem.Description)
                };

                InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                    inputConfirmation,
                    () =>
                        {
                            if (inputConfirmation.Confirmed && !string.IsNullOrEmpty(inputConfirmation.EnteredText))
                            {
                                saleLineItem.SerialId = inputConfirmation.EnteredText;
                                inputValid = true;
                            }
                            else
                            {
                                inputValid = allowBlank;
                            }
                        });

                application.Services.Interaction.InteractionRequest(request);

                if (!inputValid)
                {   // Inform the user that they must enter a valid serial number
                    string message = LSRetailPosis.ApplicationLocalizer.Language.Translate(200007);
                    using (frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    }
                }
            }
            while (!inputValid);
        }
        #endregion

        public void RemoveNonReturnableEntities(IRetailTransaction returnTransaction)
        {
            // Here we need to go through the items in the transaction and remove those that cannot be selected for return.
            // E.g. gift cards, voided items, returned items, unrecognized items, etc....

            // We do not need to worry about sales order,invoices & credit memos since they are not stored in the item table and 
            // therefore not present.

            // Here we need to check if we recognize the item in question.
            // It might be an item only available in selective stores and not in this one, for example.
            // So if we do not recognize the item in the local POS database we cannot allow it to be returned in this store and 
            // therefore we need to remove it from the item list that can be selected for return.

            RetailTransaction retailTransaction = returnTransaction as RetailTransaction;
            if (retailTransaction == null)
            {
                NetTracer.Warning("transaction parameter is not of type RetailTranasction");
                throw new ArgumentOutOfRangeException("transaction");
            }

            LinkedList<SaleLineItem> itemsToRemove = new LinkedList<SaleLineItem>();
            bool remove = false;

            foreach (SaleLineItem item in retailTransaction.SaleItems)
            {
                remove = false;

                // We cannot return items that were voided in the original transaction
                if (item.Voided)
                {
                    remove = true;
                }

                // We cannot return items that were returned in the original transaction
                if (item.Quantity < 0)
                {
                    remove = true;
                }

                // We cannot return items that are not available in current store.
                // We cannot return gift cards, which are stored neither with an item id nor a barcode.
                if (string.IsNullOrEmpty(item.ItemId) || (item.ItemId == ApplicationSettings.Terminal.ItemToGiftCard) && string.IsNullOrEmpty(item.BarcodeId))
                {
                    remove = true;
                }

                if (item is LSRetailPosis.Transaction.Line.GiftCertificateItem.GiftCertificateItem && false /* IsReturnable as it was only Issued but never used... */)
                {
                    remove = true;
                }


                // We cannot return items that have already been returned.  
                if (item.ReturnQtyAllowed <= 0)
                {
                    remove = true;
                }

                if (LSRetailPosis.Settings.ApplicationSettings.Terminal.ProposeRefundPaymentAmount_RU && item.Quantity == decimal.Zero)
                {
                    remove = true;
                }

                // We cannot return non-returnable items
                if (!ReturnOperationHelper.CanReturnNonReturnableItem(item))
                {
                    remove = true;
                }

                if (remove)
                {
                    itemsToRemove.AddLast(item);
                }
            }

            foreach (SaleLineItem item in itemsToRemove)
            {
                retailTransaction.SaleItems.Remove(item);
            }
        }
    }
}
