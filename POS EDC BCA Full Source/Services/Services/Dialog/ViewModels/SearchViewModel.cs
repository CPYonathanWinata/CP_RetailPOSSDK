/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using LSRetailPosis;
using LSRetailPosis.ButtonGrid;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.ViewModels
{
    /// <summary>
    /// Smart search view model
    /// </summary>
    internal sealed class SearchViewModel : INotifyPropertyChanged
    {

        #region Fields

        public const int PAGE_SIZE = 50;
        private const int MINIMUM_SEARCH_TERM_LENGTH = 2;  // Minimum two characters are reqruied for search.
        public const string PROPERTY_SEARCH_TERMS = "SearchTerms";
        public const string PROPERTY_SEARCH_TYPE = "SearchType";
        public const string PROPERTY_ADD_TO_SALE = "AddToSale";
        public const string PROPERTY_DATA = "Data";

        private bool addToSale;
        private object data;
        private string searchTerms;
        private readonly long searchCategoryHierarchyId;
        private long searchCategoryId;
        private SearchType searchType;
        private string sortColumn = "Name";
        private bool sortAsc = true;
        private bool isLastRowLoaded;
        private bool itemPriceVisible;
        private int? selectedResult;
        private Image selectedImage;
        private List<ResultRow> result = new List<ResultRow>();
        private readonly ItemData itemData = new ItemData(ApplicationSettings.Database.LocalConnection,
                                        ApplicationSettings.Database.DATAAREAID,
                                        ApplicationSettings.Terminal.StorePrimaryId);

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewModel"/> class.
        /// </summary>
        /// <param name="searchType">Startup type of the search.</param>
        /// <param name="searchTerm">The search term.</param>
        public SearchViewModel(SearchType searchType, string searchTerm)
        {
            this.MinimumSearchTermLengh = MINIMUM_SEARCH_TERM_LENGTH;
            this.SearchTerms = searchTerm ?? string.Empty;
            this.SearchType = searchType;

            // If search type is category then search term is the "Category RecId"
            if (searchType == SearchType.Category)
            {
                Tuple<string,long> categoryDetail;

                if ((long.TryParse(SearchTerms, out this.searchCategoryId))
                    && (categoryDetail = itemData.GetCategoryDetails(searchCategoryId)) != null)
                {
                    this.SearchTerms = categoryDetail.Item1;
                    this.searchCategoryHierarchyId = categoryDetail.Item2;
                }
                else
                {
                    NetTracer.Warning("SearchViewModel : Invalid category specified '{0}'", searchTerm);
                }
            }
            
            if (searchCategoryHierarchyId == 0)
            {
                this.searchCategoryHierarchyId = itemData.GetRetailProductHierarchy();
            }
        }

        #endregion

        #region Properties

        public bool AddToSale
        {
            get { return addToSale; }
            set
            {
                if (value != addToSale)
                {
                    addToSale = value;
                    OnPropertyChanged(PROPERTY_ADD_TO_SALE);
                }
            }
        }

        public object Data
        {
            get { return data; }
            set
            {
                if (value != null)
                {
                    data = value;
                    OnPropertyChanged(PROPERTY_DATA);
                }
            }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        public ReadOnlyCollection<ResultRow> Results
        {
            get
            {
                return this.result.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets or sets the search terms.
        /// </summary>
        public string SearchTerms
        {
            get { return searchTerms; }
            set
            {
                if (!string.Equals(searchTerms, value, StringComparison.OrdinalIgnoreCase))
                {
                    searchTerms = value;
                    OnPropertyChanged(PROPERTY_SEARCH_TERMS);
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected image.
        /// </summary>
        public Image SelectedImage
        {
            get { return selectedImage; }
            set
            {
                if (selectedImage != value)
                {
                    if (selectedImage != null)
                    {
                        selectedImage.Dispose();
                    }

                    selectedImage = value;
                    OnPropertyChanged("SelectedImage");
                }
            }
        }

        /// <summary>
        /// Gets the selected result.
        /// </summary>
        /// <remarks>Selected result if available, null otherwise.</remarks>
        public ResultRow SelectedResult
        {
            get
            {
                return (selectedResult.HasValue && selectedResult.Value >= 0)
                    ? this.result[selectedResult.Value]
                    : null;
            }
        }

        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        public SearchType SearchType
        {
            get { return searchType; }
            private set
            {
                if (searchType != value)
                {
                    searchType = value;

                    OnPropertyChanged(PROPERTY_SEARCH_TYPE);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether item price visible.
        /// </summary>
        /// <value> 
        ///   <c>true</c> if true then price is visible; otherwise not.</c>.
        /// </value>
        public bool ItemPriceVisible
        {
            get { return itemPriceVisible; }
            set
            {
                if (itemPriceVisible != value)
                {
                    if (value)
                    {
                        CalculatePrice();
                    }

                    itemPriceVisible = value;
                    OnPropertyChanged("ItemPriceVisible");
                }
            }
        }

        /// <summary>
        /// Gets the minimum search term lengh.
        /// </summary>
        public int MinimumSearchTermLengh { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether Products should be searched across AX.
        /// </summary>
        /// <value> 
        ///   <c>true</c> if true then price is visible; otherwise not.</c>.
        /// </value>
        public bool SearchAllAxProducts { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the result list.
        /// </summary>
        /// <param name="fromRow">From row.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",Justification = "object disposed")]
        private void UpdateResultList(int fromRow)
        {
            NetTracer.Information("SearchViewModel : UpdateResultList : Start");

            if (fromRow == 0)
            {
                this.result.Clear();
                ExecuteSelect(null);
            }

            if (isLastRowLoaded
                || this.SearchTerms.Length < MinimumSearchTermLengh)
            {
                return;
            }

            SqlConnection connection = ApplicationSettings.Database.LocalConnection;

            if (this.SearchAllAxProducts == false)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        if (SearchType == SearchType.Category)
                        {
                            GetCategoryQuery(command, fromRow);

                            // Category mode automatically switches to items when executed.
                            this.SearchType = SearchType.Item;
                        }
                        else
                        {
                            GetDefaultQuery(command, fromRow);
                        }
                        command.Connection = connection;

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.result.Add(new ResultRow(reader));
                            }
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                DataTable itemsWithPrices=new DataTable();
                const Int64 startPosition = 1;
                const Int64 pageSize = 100;
                const string orderByField = "Name";
                string sortOrder = "Ascending";
                bool includeTotal = true;
                int otherChannelRecId = 0;
                int catalogRecId = 0;
                string attributeRecIdRangeValue = string.Empty;

                sortOrder = sortAsc ? DevExpress.Data.ColumnSortOrder.Ascending.ToString() : DevExpress.Data.ColumnSortOrder.Descending.ToString();             

                Dialog.InternalApplication.Services.Item.GetProductsByKeyword(
                    ApplicationSettings.Terminal.StorePrimaryId, 
                    SearchTerms,
                    startPosition, 
                    pageSize,
                    orderByField,
                    sortOrder,
                    includeTotal,
                    Thread.CurrentThread.CurrentUICulture.Name,
                    otherChannelRecId,
                    catalogRecId,
                    attributeRecIdRangeValue,
                    ref itemsWithPrices);
                
                
                foreach (DataRow row in itemsWithPrices.Rows)
                {
                    this.result.Add(new ResultRow(row["ITEMID"].ToString(), row["ITEMNAME"].ToString(), row["ITEMPRICE"].ToString()));
                }
                if (itemsWithPrices != null)
                {
                    itemsWithPrices.Dispose();
                }
            }

            // If we didn't get back a full page of results then we loaded everything
            if (this.result.Count % PAGE_SIZE > 0)
                isLastRowLoaded = true;

            // Get the price of the items if enabled.
            if (ItemPriceVisible)
            {
                CalculatePrice();
            }

            OnPropertyChanged("Results");

            NetTracer.Information("SearchViewModel : UpdateResultList : End");
        }


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

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No user input")]
        private void GetDefaultQuery(SqlCommand command, int fromRow)
        {
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@SEARCHVALUE", SearchTerms),
                                                             new SqlParameter("@ORDERBY", sortColumn.ToUpper()),
                                                             new SqlParameter("@CULTUREID", ApplicationSettings.Terminal.CultureName),
                                                             new SqlParameter("@STORERECID", ApplicationSettings.Terminal.StorePrimaryId),
                                                             new SqlParameter("@STOREDATE", DateTime.Today),
                                                             new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
                                                             new SqlParameter("@CATEGORYHIERARCHY", searchCategoryHierarchyId),
                                                             new SqlParameter("@SEARCHTYPE", this.SearchType),
                                                             new SqlParameter("@ITEMTYPE", SearchType.Item),
                                                             new SqlParameter("@CUSTOMERTYPE", SearchType.Customer),
                                                             new SqlParameter("@CATEGORYTYPE", SearchType.Category),
                                                             new SqlParameter("@ASCENDING", sortAsc),
                                                             new SqlParameter("@FROMROW", fromRow),
                                                             new SqlParameter("@PAGESIZE", PAGE_SIZE)
                                                            };

            command.CommandText = String.Format("exec dbo.SMARTSEARCH {0}", String.Join(",", parameters.Select(x => x.ParameterName)));
            command.Parameters.AddRange(parameters);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No user input")]
        private void GetCategoryQuery(SqlCommand command, int fromRow)
        {
            string query = @"DECLARE @tvp_ProductIds [crt].[RECORDIDTABLETYPE] 

	                        INSERT INTO @tvp_ProductIds (RECID) 
	                        SELECT DISTINCT IT.PRODUCT 
                            FROM INVENTTABLE IT 
                            JOIN INVENTTABLEMODULE IM ON IT.ITEMID = IM.ITEMID AND IM.MODULETYPE = 2 AND IT.DATAAREAID = IM.DATAAREAID 
                            JOIN ECORESPRODUCTCATEGORY PC ON PC.PRODUCT = IT.PRODUCT 
                            JOIN RETAILCATEGORYCONTAINMENTLOOKUP CL ON CL.CONTAINEDCATEGORY = PC.CATEGORY
                            WHERE CL.CATEGORY = @CATEGORY AND IT.DATAAREAID = @DATAAREAID 
                
				            SELECT * FROM 
                                (SELECT *, ROW_NUMBER() OVER (ORDER BY {0} {1}) AS ROW 
                                    FROM 
                                    (SELECT IT.ITEMID AS NUMBER, COALESCE(TR.NAME, IT.ITEMID) AS NAME, @ITEMTYPE AS TYPE, ISNULL(IM.UNITID, '') AS TAG 
                                        FROM [crt].GETASSORTEDPRODUCTS(@STORERECID, @STOREDATE, 0, 1, 1, @tvp_ProductIds) IT 
                                        JOIN INVENTTABLEMODULE IM ON IT.ITEMID = IM.ITEMID AND IM.MODULETYPE = 2 AND IM.DATAAREAID = @DATAAREAID 
                                        JOIN ECORESPRODUCTCATEGORY PC ON PC.PRODUCT = IT.PRODUCTID 
                                        JOIN ECORESPRODUCT AS PR ON PR.RECID = IT.PRODUCTID 
                                        JOIN RETAILCATEGORYCONTAINMENTLOOKUP CL ON CL.CONTAINEDCATEGORY = PC.CATEGORY AND CL.CATEGORY = @CATEGORY 
                                        LEFT JOIN ECORESPRODUCTTRANSLATION AS TR ON PR.RECID = TR.PRODUCT AND TR.LANGUAGEID = @CULTUREID 
                                    ) UN 
                                ) RN 
                                WHERE RN.ROW >= @FROMROW 
					            AND RN.ROW <= @TOROW";

            string categoryQuery = string.Format(query, sortColumn, sortAsc ? "ASC" : "DESC");

            command.CommandText = categoryQuery;
            command.Parameters.AddWithValue("@CULTUREID", ApplicationSettings.Terminal.CultureName);
            command.Parameters.AddWithValue("@ITEMTYPE", SearchType.Item);
            command.Parameters.AddWithValue("@STORERECID", ApplicationSettings.Terminal.StorePrimaryId);
            command.Parameters.AddWithValue("@STOREDATE", DateTime.Today);
            command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);
            command.Parameters.AddWithValue("@CATEGORY", this.searchCategoryId);
            command.Parameters.AddWithValue("@FROMROW", fromRow);
            command.Parameters.AddWithValue("@TOROW", (fromRow + PAGE_SIZE));
        }

        /// <summary>
        /// Gets the item image.
        /// </summary>
        /// <param name="itemNumber">The item number.</param>
        /// <returns>The image if available, else null.</returns>
        private static Image GetItemImage(string itemNumber)
        {
            Image result = null;
            SqlConnection connection = ApplicationSettings.Database.LocalConnection;
            const string query = "SELECT TOP 1 MEDIUMSIZE FROM ECORESPRODUCTIMAGE IM " +
                                    "INNER JOIN INVENTTABLE IT ON IM.REFRECORD = IT.RECID " +
                                    "WHERE ITEMID = @ITEMNUMBER " +
                                    "ORDER BY DEFAULTIMAGE DESC ";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ITEMNUMBER", itemNumber);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    result = GUIHelper.GetBitmap(command.ExecuteScalar() as byte[]);
                }
            }
            catch (Exception ex)
            {
                // Image loading failure should not block operation.
                NetTracer.Warning(ex, "SearchViewModel : GetItemImage : Failed to load image for item {0}", itemNumber);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the price.
        /// </summary>
        private void CalculatePrice()
        {
            try
            {
                List<ISaleLineItem> items = new List<ISaleLineItem>();

                // Add the items to get prices for to the collection to send into the pricing engine.
                for (int x = 0; x < this.Results.Count; x++)
                {
                    if (this.Results[x].SearchType == SearchType.Item && this.Results[x].ItemPrice == null)
                    {
                        items.Add(new SaleLineItem() { LineId = x, ItemId = this.Results[x].Number, SalesOrderUnitOfMeasure = (string)this.Results[x].Tag });
                    }
                }

                if (items.Count > 0)
                {
                    // Obtain prices for the items and repopulate the results grid with those prices.
                    var pricedItems = Dialog.InternalApplication.Services.Price.GetItemPrices(items, null).ToDictionary(p => p.LineId);

                    for (int x = 0; x < this.Results.Count; x++)
                    {
                        if (this.Results[x].SearchType == SearchType.Item && this.Results[x].ItemPrice == null)
                        {
                            this.Results[x].ItemPrice = Dialog.InternalApplication.Services.Rounding.RoundForDisplay(pricedItems[x].Price, true, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Price calculation failure should not block operation.
                NetTracer.Warning(ex, "SearcViewModel : CalculatePrice : Failed to calculate prices for items");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Executes the search.
        /// </summary>
        public void ExecuteSearch()
        {
            // Reset last row loaded flag
            isLastRowLoaded = false;

            UpdateResultList(0);
        }

        /// <summary>
        /// Executes the next page.
        /// </summary>
        public void ExecuteNextPage()
        {
            UpdateResultList(result.Count);
        }

        /// <summary>
        /// Executes the clear.
        /// </summary>
        public void ExecuteClear()
        {
            this.SearchTerms = null;

            this.result.Clear();

            ExecuteSelect(null);
        }

        /// <summary>
        /// Executes the select.
        /// </summary>
        /// <param name="selectedIndex">Index of the selected.</param>
        public void ExecuteSelect(int? selectedIndex)
        {
            if (this.selectedResult != selectedIndex)
            {
                this.selectedResult = selectedIndex;

                // Clear the image if there is any.
                this.SelectedImage = null;

                OnPropertyChanged("SelectedResult");
            }
        }

        /// <summary>
        /// Executes the customer transactions.
        /// </summary>
        public void ExecuteCustomerTransactions()
        {
            Dialog.InternalApplication.Services.Customer.Transactions(this.SelectedResult.Number);
        }

        /// <summary>
        /// Shows the product details form.
        /// </summary>
        public void ExecuteProductDetails()
        {
            string itemNumber = (this.SelectedResult != null) ? this.SelectedResult.Number : string.Empty;

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                    new ProductDetailsConfirmation() { ItemNumber = itemNumber, SourceContext = ProductDetailsSourceContext.ItemSearch }, () => { });
            Dialog.InternalApplication.Services.Interaction.InteractionRequest(request);

            ProductDetailsConfirmation confirmation = request.Context as ProductDetailsConfirmation;
            if ((confirmation != null) && confirmation.Confirmed && confirmation.AddToSale)
            {
                this.AddToSale = confirmation.AddToSale;
                this.Data = confirmation.ResultData; // ResultData will be null for non-kit item and in that case itemnumber will be used to add to cart.
            }
            else
            {
                this.AddToSale = false;
            }
        }

        /// <summary>
        /// Executes the filter all.
        /// </summary>
        public void ExecuteFilterAll()
        {
            if (this.SearchType != SearchType.All)
            {
                this.SearchType = SearchType.All;
                ExecuteSearch();
            }
        }

        /// <summary>
        /// Executes the filter items only.
        /// </summary>
        public void ExecuteFilterItemsOnly()
        {
            if (this.SearchType != SearchType.Item)
            {
                this.SearchType = SearchType.Item;
                ExecuteSearch();
            }
        }

        /// <summary>
        /// Executes the filter customers only.
        /// </summary>
        public void ExecuteFilterCustomersOnly()
        {
            if (this.SearchType != SearchType.Customer)
            {
                this.SearchType = SearchType.Customer;
                ExecuteSearch();
            }
        }

        /// <summary>
        /// Executes the category search.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        public void ExecuteFilterCategory()
        {
            if (this.SearchType != SearchType.Category)
            {
                // For category search, the term is not user input rather it is category number.
                this.SearchType = SearchType.Category;
                this.searchCategoryId = long.Parse(this.SelectedResult.Number);
                this.SearchTerms = this.SelectedResult.Name;

                ExecuteSearch();
            }
        }

        /// <summary>
        /// Sets the Search Criteria to search across All Products.
        /// </summary>
        public void SetSearchAcrossAX(bool setSearch)
        {
            SearchAllAxProducts = setSearch;
        }

        /// <summary>
        /// Sets the Search Criteria to search across All Products.
        /// </summary>
        public void ResetSearchAcrossAX()
        {
            if (this.SearchAllAxProducts)
            {
                this.SearchAllAxProducts = false;
            }
        }

        /// <summary>
        /// Executes the load row detail.
        /// </summary>
        public void ExecuteLoadRowDetail()
        {
            ResultRow selectedRow = this.SelectedResult;

            if (selectedRow != null)
            {
                if ((selectedRow.SearchType == SearchType.Customer) && (selectedRow.cachedCustomerResult == null))
                {
                    selectedRow.cachedCustomerResult = Dialog.InternalApplication.BusinessLogic.CustomerSystem.GetCustomerInfo(selectedRow.Number);
                }
                else if (selectedRow.SearchType == SearchType.Item)
                {
                    this.SelectedImage = GetItemImage(selectedRow.Number);
                }
            }

            OnPropertyChanged("SelectedResult");
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        [System.Diagnostics.DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new ArgumentException(msg);
            }
        }

        #endregion

    }

    #region Types

    /// <summary>
    /// Search row data object.
    /// </summary>
    internal class ResultRow
    {
        internal ICustomer cachedCustomerResult = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultRow"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public ResultRow(IDataReader reader)
        {
            this.Number = DBUtil.ToStr(reader["NUMBER"]);
            this.Name = DBUtil.ToStr(reader["NAME"]);
            this.SearchType = (SearchType)DBUtil.ToInt32(reader["TYPE"]);
            this.Tag = reader["TAG"];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultRow"/> class.
        /// </summary>
        /// <param name="itemNumber">The item number</param>
        /// <param name="itemName">The item name</param>
        /// <param name="itemPrice">The item price</param>
        public ResultRow(string itemNumber, string itemName, string itemPrice)
        {
            this.Number = itemNumber;
            this.Name = itemName;
            this.SearchType = SearchType.Item;
            this.ItemPrice = itemPrice;
            this.Tag = "";
        }

        /// <summary>
        /// Gets the number.
        /// </summary>
        public string Number
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        public SearchType SearchType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets row type specific tag
        /// </summary>
        public object Tag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the item price.
        /// </summary>
        public string ItemPrice
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the type of the formatted result.
        /// </summary>
        /// <value>
        /// The type of the formatted result.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for data binding.")]
        public string FormattedResultType
        {
            get
            {
                switch (this.SearchType)
                {
                    case SearchType.Item:
                        return ApplicationLocalizer.Language.Translate(1741);
                    case SearchType.Customer:
                        return ApplicationLocalizer.Language.Translate(1742);
                    case SearchType.Category:
                        return ApplicationLocalizer.Language.Translate(1743);
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        public ICustomer GetCustomer()
        {
            return cachedCustomerResult;
        }
    }

    /// <summary>
    /// Serach result data object.
    /// </summary>
    internal class SearchResult : ISearchResult
    {
        /// <summary>
        /// Gets the type of the search result.
        /// </summary>
        public SearchResultType SearchType { get; set; }

        /// <summary>
        /// Gets the number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets the Data as barcodeinfo.
        /// </summary>
        public object Data { get; set; }
    }


    #endregion
}
