/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.PriceService
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.SaleItem;
    using Microsoft.Dynamics.Commerce.Runtime;
    using Microsoft.Dynamics.Commerce.Runtime.Data;
    using Microsoft.Dynamics.Commerce.Runtime.Services.Messages;
    using Microsoft.Dynamics.Retail.Diagnostics;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using CRTDM = Microsoft.Dynamics.Commerce.Runtime.DataModel;
    using DM = Microsoft.Dynamics.Retail.Pos.DataManager;

    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification="EF pushed this over the limit. This class needs refactoring.")]
    [Export(typeof(IPrice))]
    public class Price : IPrice, IInitializeable
    {

        #region Member variables

        private PriceParameters salesPriceParameters;
        private ReadOnlyCollection<string> storePriceGroups;
        ////private ReadOnlyCollection<long> storePriceGroupIds;

        /////// <summary>
        /////// Sentinal value for 'no date specified'
        /////// </summary>
        ////internal static readonly DateTime NoDate = new DateTime(1900, 1, 1);

        // See Discount.cs for duplicate definition.  Clean up if refactoring common code between services ever occurs
        internal enum DateValidationTypes
        {
            Advanced = 0,
            Standard = 1
        }

        private DM.DiscountDataManager discountDataManager =
            new DM.DiscountDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

        #endregion

        #region Properties
        /// <summary>
        /// IApplication instance.
        /// </summary>
        private IApplication application;

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
        [Import]
        public IApplication Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
                InternalApplication = value;
            }
        }

        /// <summary>
        /// Gets or sets the static IApplication instance.
        /// </summary>
        internal static IApplication InternalApplication { get; private set; }

        #endregion

        #region IInitializeableV1 Members

        /// <summary>
        /// Initialize price service.
        /// </summary>
        public void Initialize()
        {
            this.salesPriceParameters = GetPriceParameters();
            this.storePriceGroups = this.discountDataManager.PriceGroupsFromStore(ApplicationSettings.Terminal.StorePrimaryId);
            ////this.storePriceGroupIds = this.discountDataManager.PriceGroupIdsFromStore(ApplicationSettings.Terminal.StorePrimaryId);
        }

        /// <summary>
        /// Uninitialize price service.
        /// </summary>
        public void Uninitialize()
        {
        }

        #endregion

        #region IPrice

        /// <summary>
        /// <remarks></remarks>
        /// </summary>
        /// <param name="retailTransaction"></param>
        public void GetPrice(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = (RetailTransaction)retailTransaction;
            ICustomerOrderTransaction orderTransaction = retailTransaction as ICustomerOrderTransaction;
            bool priceLock = (orderTransaction == null) ? false : (orderTransaction.LockPrices);

            try
            {
                if (transaction != null
                    && transaction.CalculableSalesLines != null
                    && transaction.CalculableSalesLines.Any())
                {
                    var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
                    using (CommerceRuntime commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal))
                    {
                        CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                        GetPriceServiceResponse response = commerceRuntime.Execute<GetPriceServiceResponse>(new GetPriceServiceRequest(crtTransaction), null);
                        CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                    }
                }
            }
            catch (Exception  ex)
            {
                throw new LSRetailPosis.PosisException(LSRetailPosis.ApplicationLocalizer.Language.Translate(99995), ex.InnerException);
            }
        }

        /// <summary>
        /// Updates all prices as per included tax.
        /// </summary>
        /// <param name="retailTransaction"></param>
        /// <param name="restoreItemPrices"></param>
        public void UpdateAllPrices(IRetailTransaction retailTransaction, bool restoreItemPrices)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;
            if (transaction == null)
            {
                NetTracer.Warning("retailTransaction parameter is null");
                throw new ArgumentNullException("retailTransaction");
            }

            try
            {
                var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
                using (CommerceRuntime commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal))
                {
                    CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                    GetPriceServiceResponse response = commerceRuntime.Execute<GetPriceServiceResponse>(new UpdatePriceServiceRequest(crtTransaction, restoreItemPrices), null);
                    CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                }
            }
            catch (Exception ex)
            {
                throw new LSRetailPosis.PosisException(LSRetailPosis.ApplicationLocalizer.Language.Translate(99995), ex.InnerException);
            }
        }

        private PriceParameters GetPriceParameters()
        {
            string queryString = "SELECT SALESPRICEACCOUNTITEM, SALESPRICEGROUPITEM, SALESPRICEALLITEM FROM PRICEPARAMETERS WHERE "
                + "DATAAREAID = @DATAAREAID ";
            SqlConnection connection = Application.Settings.Database.Connection;
            string dataAreaId = Application.Settings.Database.DataAreaID;

            bool salesPriceAccountItem = false;
            bool salesPriceGroupItem = false;
            bool salesPriceAllItem = false;

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    SqlParameter dataAreaIdParm = command.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar, 4);
                    dataAreaIdParm.Value = dataAreaId;

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    reader.Read();
                    if (reader.HasRows)
                    {
                        if (reader.GetInt32(reader.GetOrdinal("SALESPRICEACCOUNTITEM")) == 1) { salesPriceAccountItem = true; } else { salesPriceAccountItem = false; }
                        if (reader.GetInt32(reader.GetOrdinal("SALESPRICEGROUPITEM")) == 1) { salesPriceGroupItem = true; } else { salesPriceGroupItem = false; }
                        if (reader.GetInt32(reader.GetOrdinal("SALESPRICEALLITEM")) == 1) { salesPriceAllItem = true; } else { salesPriceAllItem = false; }
                    }
                    else
                    {
                        salesPriceAccountItem = false;
                        salesPriceGroupItem = false;
                        salesPriceAllItem = false;
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

            return new PriceParameters(salesPriceAccountItem, salesPriceGroupItem, salesPriceAllItem);
        }

        private PriceResult FindPriceAgreement(RetailPriceArgs args)
        {
            // salesUOM is the unit of measure being set at the moment
            // inventUOM is the base UOM for the item

            // First we get the price according to the base UOM
            PriceAgreementArgs p = args.AgreementArgsForInventory();
            PriceResult result = TradeAgreementPricing.priceAgr(p, this.salesPriceParameters); // eg: Pcs

            // Is the current UOM something different than the base UOM?
            if (args.SalesUOM != args.InventUOM)
            {
                // Then let's see if we find some price agreement for that UOM
                p = args.ArgreementArgsForSale();
                PriceResult salesUOMResult = TradeAgreementPricing.priceAgr(p, this.salesPriceParameters); // eg: Box

                // If there is a price found then we return that as the price
                if (salesUOMResult.Price > decimal.Zero)
                {
                    return salesUOMResult;
                }
                else
                {
                    return new PriceResult(result.Price * args.UnitQtyConversion.GetFactorForQty(args.Quantity), result.IncludesTax);
                }
            }

            // else we return baseUOM price mulitplied with the unit qty factor.
            return result;
        }

        /// <summary>
        /// Returns the basic price found in the inventory table. Used if no price is found in the PriceDiscountTable
        /// </summary>
        /// <param name="itemId">The unique item id as stored in the inventory table</param>
        /// <returns>The basic sales price</returns>
        [Obsolete("This method is not used anymore, please use GetItemPrice method.")]
        public decimal GetBasicPrice(string itemId)
        {
            throw new NotSupportedException("The method GetBasicPrice is not supported anymore, please use GetItemPrice method.");
        }

        private PriceResult GetRetailPrice(RetailPriceArgs args)
        {
            PriceResult result;

            //Store and trade agreement price
            result = FindPriceAgreement(args);

            //If the store currency is not the same as the HO currency then check if there is a price for the HO currency in the trade agreements
            if ((result.Price == 0) && (args.CurrencyCode != ApplicationSettings.Terminal.CompanyCurrency))
            {
                string originalCurrencyCode = args.CurrencyCode;
                args.CurrencyCode = ApplicationSettings.Terminal.CompanyCurrency;
                result = FindPriceAgreement(args);
                result = new PriceResult(result, ApplicationSettings.Terminal.CompanyCurrency, originalCurrencyCode);
            }

            return result;
        }

        internal PriceResult GetActiveTradeAgreement(RetailTransaction retailTransaction, SaleLineItem saleItem, Decimal quantity)
        {
            PriceResult result;

            // Get basic arguments for Price evaluation
            RetailPriceArgs args = new RetailPriceArgs()
            {
                Barcode = saleItem.BarcodeId,
                CurrencyCode = retailTransaction.StoreCurrencyCode, // store currency
                CustomerId = retailTransaction.Customer.CustomerId,
                Dimensions = saleItem.Dimension,
                InventUOM = saleItem.InventOrderUnitOfMeasure,
                ItemId = saleItem.ItemId,
                PriceGroups = this.storePriceGroups,
                Quantity = quantity,
                SalesUOM = saleItem.SalesOrderUnitOfMeasure,
                UnitQtyConversion = saleItem.UnitQtyConversion,
                SerialId = saleItem.SerialId
            };

            //Get the active retail price - checks following prices brackets in order: Customer TAs, Store price group TAs, 'All' TAs.
            // First bracket to return a price 'wins'. Each bracket returns the lowest price it can find.
            result = GetRetailPrice(args);

            //Direct customer TA price would have been caught above. 
            // Compare against customer price group TAs now and override if lower than previously found price (or if previously found price was 0).
            if (!string.IsNullOrEmpty(retailTransaction.Customer.CustomerId)
                && !string.IsNullOrEmpty(retailTransaction.Customer.PriceGroup)
                && !this.storePriceGroups.Contains(retailTransaction.Customer.PriceGroup))
            {
                //Customer price group
                args.PriceGroups = new ReadOnlyCollection<string>(new[] { retailTransaction.Customer.PriceGroup });
                PriceResult customerResult = FindPriceAgreement(args);

                //If the store currency is not the same as the HO currency then check if there is a price for the HO currency in the trade agreements
                if ((customerResult.Price == 0) && (retailTransaction.StoreCurrencyCode != ApplicationSettings.Terminal.CompanyCurrency))
                {
                    // convert the TA from company currency to store currency
                    args.CurrencyCode = ApplicationSettings.Terminal.CompanyCurrency;
                    customerResult = FindPriceAgreement(args);
                    customerResult = new PriceResult(customerResult, ApplicationSettings.Terminal.CompanyCurrency, retailTransaction.StoreCurrencyCode);
                }

                // Pick the Customer price if either the Retail price is ZERO, or the Customer Price is non-zero AND lower
                if ((result.Price == decimal.Zero)
                    || ((customerResult.Price > decimal.Zero) && (customerResult.Price <= result.Price)))
                {
                    result = customerResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns price of the item as per given item id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="unitOfMeasure"></param>
        /// <returns></returns>
        public decimal GetItemPrice(string itemId, string unitOfMeasure)
        {
            SaleLineItem line = new SaleLineItem() { ItemId = itemId, SalesOrderUnitOfMeasure = unitOfMeasure };
            return GetItemPrices(new List<ISaleLineItem>(1) { line }, null).First().Price;
        }

        /// <summary>
        /// Checks if the item with variant is a part of a price agreement and if it is returns that price. If it's not then the function returns the basic item price.
        /// </summary>
        /// <param name="itemId">The ID of the item to be checked.</param>
        /// <param name="inventDimId">Inventory Dimension Id.</param>
        /// <param name="unitOfMeasure">The unit of measure to be checked.</param>
        /// <returns>The price of variant item.</returns>
        public decimal GetItemPrice(string itemId, string inventDimId, string unitOfMeasure)
        {
            if (string.IsNullOrWhiteSpace(inventDimId))
            {
                throw new ArgumentNullException("inventDimId");
            }

            SaleLineItem line = new SaleLineItem() { ItemId = itemId, SalesOrderUnitOfMeasure = unitOfMeasure };
            return GetItemPrices(new List<ISaleLineItem>(1) { line }, inventDimId).First().Price;
        }

        /// <summary>
        /// Checks if the items are part of a price agreement and returns either that price or the basic item price for each specified item.
        /// </summary>
        /// <param name="items">The collection of items to check.</param>
        /// <param name="inventDimId">Inventory Dimension Id.</param>
        /// <returns>The collection of items with prices populated.</returns>
        public IEnumerable<ISaleLineItem> GetItemPrices(IEnumerable<ISaleLineItem> items, string inventDimId)
        {
            List<ISaleLineItem> result = new List<ISaleLineItem>(items);

            try
            {
                var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
                using (CommerceRuntime commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal))
                {
                    List<CRTDM.SalesLine> salesLines = new List<CRTDM.SalesLine>(result.Count);

                    // Populate lines to send to CRT pricing engine.
                    foreach (var item in result)
                    {
                        salesLines.Add(new CRTDM.SalesLine() { LineId = item.LineId.ToString(), ItemId = item.ItemId, Quantity = (item.Quantity == 0 ? 1 : item.Quantity), SalesOrderUnitOfMeasure = item.SalesOrderUnitOfMeasure, InventoryDimensionId = inventDimId });
                    }

                    GetPricesServiceRequest priceRequest = new GetPricesServiceRequest(salesLines, DateTimeOffset.Now, string.Empty, string.Empty, CRTDM.PricingCalculationMode.Independent);
                    GetPricesServiceResponse response = commerceRuntime.Execute<GetPricesServiceResponse>(priceRequest, null);

                    var itemDictionary = response.SalesLines.ToDictionary(p => p.LineId);

                    foreach (var item in result)
                    {
                        item.Price = itemDictionary[item.LineId.ToString()].AdjustedPrice;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new LSRetailPosis.PosisException(LSRetailPosis.ApplicationLocalizer.Language.Translate(99995), ex.InnerException);
            }
        }
        #endregion
    }
}
