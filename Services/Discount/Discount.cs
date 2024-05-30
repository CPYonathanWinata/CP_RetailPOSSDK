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
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.Discount;
using LSRetailPosis.Transaction.Line.SaleItem;
using LSRetailPosis.Transaction.MemoryTables;
using Microsoft.Dynamics.Commerce.Runtime;
using Microsoft.Dynamics.Commerce.Runtime.Data;
using Microsoft.Dynamics.Commerce.Runtime.Services;
using Microsoft.Dynamics.Commerce.Runtime.Services.Messages;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using CRTDM = Microsoft.Dynamics.Commerce.Runtime.DataModel;
using CustomerDiscountTypes = LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem.CustomerDiscountTypes;
using DE = Microsoft.Dynamics.Retail.Pos.DataEntity;
using DiscountTypes = LSRetailPosis.Transaction.Line.Discount.LineDiscountItem.DiscountTypes;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using PeriodicDiscOfferType = LSRetailPosis.Transaction.Line.Discount.PeriodicDiscountItem.PeriodicDiscOfferType;
using PeriodStatus = LSRetailPosis.Transaction.MemoryTables.Period.PeriodStatus;

using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
namespace Microsoft.Dynamics.Retail.Pos.DiscountService
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Revisit this class for refactoring")]
    [Export(typeof(IDiscount))]
    public class Discount : IDiscount, IInitializeable
    {
        #region Structs and enums
        // Sentinal value for 'no date specified' Duplicated throughout software
        private readonly DateTime NoDate = new DateTime(1900, 1, 1);

        // See Price.cs for duplicate definition.  Clean up if refactoring common code between services ever occurs
        private enum DateValidationTypes
        {
            Advanced = 0,
            Standard = 1
        }

        #endregion

        #region Member variables

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

        private IUtility Utility
        {
            get
            {
                return this.Application.BusinessLogic.Utility;
            }
        }

        // Internal Cache
        private List<PriceDiscData> priceDiscDataCache;
        private string priceDiscDataCacheToken; // Token to indicate cache source (maps to transcationId)

        private DataTable activeOffers = new DataTable("ACTIVEPERIODICOFFERS");
        private DataTable activeOfferLines = new DataTable("ACTIVEPERIODICOFFERLINES");
        private DataTable tmpMMOffer = new DataTable("TMPMMOFFER");
        private DataTable tmpDiscountCode = new DataTable("TMPDISCOUNTCODE");

        private DM.DiscountDataManager discountDataManager;
        private DiscountParameters DiscountParameters { get; set; }

        private ReadOnlyCollection<Int64> storePriceGroups = new List<Int64>().AsReadOnly();
        #endregion

        #region IInitializeable Members

        public void Initialize(DM.DiscountDataManager dataManagerForDiscounts, DiscountParameters discountParametersActive)
        {
            if (dataManagerForDiscounts == null)
            {
                NetTracer.Warning("dataManagerForDiscounts parameter is null");
                throw new ArgumentNullException("dataManagerForDiscounts");
            }

            if (discountParametersActive == null)
            {
                NetTracer.Warning("discountParametersActive parameter is null");
                throw new ArgumentNullException("discountParametersActive");
            }

            priceDiscDataCache = new List<PriceDiscData>();

            this.DiscountParameters = discountParametersActive;
            this.discountDataManager = dataManagerForDiscounts;
            this.storePriceGroups = this.discountDataManager.PriceGroupIdsFromStore(ApplicationSettings.Terminal.StorePrimaryId);

            if (ApplicationSettings.LogTraceLevel == LogTraceLevel.Trace)
            {
                StringBuilder priceGroupString =
                    this.storePriceGroups.Aggregate(new StringBuilder(), (pgString, pgId) => pgString.AppendFormat(" '{0}'", pgId.ToString()));
                LSRetailPosis.ApplicationLog.Log("Discount.Initialize()",
                    String.Format("Initializing discount. Store '{0}' belongs to price groups:{1}.",
                    ApplicationSettings.Terminal.StorePrimaryId, priceGroupString.ToString()),
                    LogTraceLevel.Trace);
            }

            MakeActiveOfferTables();        //Tables for periodic discount calculation
            MakeTmpOfferTable();            //Temporary table used to for mix match calculation
            MakeTmpDiscountCodeTable();
        }

        public void Initialize()
        {
            DiscountParameters parameters = new DiscountParameters();
            parameters.InitializeParameters();
            Initialize(
                new DM.DiscountDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID),
                parameters);
        }

        public void Uninitialize()
        {
            activeOffers.Dispose();
            activeOfferLines.Dispose();
            tmpMMOffer.Dispose();
            tmpDiscountCode.Dispose();
        }

        #endregion

        private static void CalculateTotalDiscount(IRetailTransaction rt)
        {
            rt.ClearTotalDiscountLines();

            if (rt.TotalManualPctDiscount != 0)
            {
                rt.AddTotalDiscPctLines();
            }
            if (rt.TotalManualDiscountAmount != 0)
            {
                rt.AddTotalDiscAmountLines(typeof(TotalDiscountItem), rt.TotalManualDiscountAmount);
            }
        }

        private static void CalculateLoyaltyDiscount(IRetailTransaction rt)
        {
            rt.ClearLoyaltyDiscountLines();

            if (rt.LoyaltyManualDiscountAmount.HasValue)
            {
                rt.AddLoyaltyDiscAmountLines(rt.LoyaltyManualDiscountAmount.Value);
            }
        }

        /// <summary>
        /// Calculates all of the discounts for the transactions.
        /// </summary>
        /// <param name="retailTransaction"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Existing API, temporary debug code has coupling.")]
        public void CalculateDiscount(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;
            if (transaction == null)
            {
                NetTracer.Error("retailTransaction parameter is null");
                throw new ArgumentNullException("retailTransaction");
            }

            ICustomerOrderTransaction orderTransaction = retailTransaction as ICustomerOrderTransaction;
            bool priceLock = (orderTransaction == null) ? false : (orderTransaction.LockPrices);

            if (!priceLock)
            {
#if DEBUG
                string compareDiscounts = Environment.GetEnvironmentVariable("COMPAREDISCOUNTS");
                string alwaysShowComparisonResults = Environment.GetEnvironmentVariable("ALWAYSSHOWCOMPARISONRESULTS");

                RetailTransaction rt = null;
                TimeSpan oldTimeElapsed = new TimeSpan();
                TimeSpan newTimeElapsed = new TimeSpan();

                if (!string.IsNullOrEmpty(compareDiscounts) && compareDiscounts.ToUpperInvariant().StartsWith("Y"))
                {
                    rt = transaction.CloneTransaction() as RetailTransaction;

                    DateTime startOldTime = DateTime.Now;

                    CalcPeriodicDisc(rt);  //Calculation of periodic offers

                    if (!string.IsNullOrEmpty(rt.Customer.CustomerId))
                    {
                        CalcCustomerDiscount(rt); //Calculation of customer discount
                    }

                    // this is manual total discount, it should always be calculated
                    CalculateTotalDiscount(rt);

                    CalculateLoyaltyDiscount(rt);

                    oldTimeElapsed = DateTime.Now.Subtract(startOldTime);
                }

                DateTime startNewTime = DateTime.Now;

#endif
                var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
                using (CommerceRuntime commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal))
                {
                    //RequestContext requestContext = commerceRuntime.CreateRequestContext(null);

                    //CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                    //Microsoft.Dynamics.Commerce.Runtime.Services.IService svc = commerceRuntime.GetService(ServiceTypes.PricingService);
                    //GetPriceServiceResponse response = svc.Execute<GetPriceServiceResponse>(new CalculateDiscountsServiceRequest(requestContext, crtTransaction));
                    //CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                    CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                    GetPriceServiceResponse response = commerceRuntime.Execute<GetPriceServiceResponse>(new CalculateDiscountsServiceRequest(crtTransaction), null);
                    CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                }



                 //custom by Yonathan 11/07/2023 to remove discount if it isn't B2B
                bool isEmptyCustomer = transaction.Customer.IsEmptyCustomer();
                //if (APIAccess.APIAccessClass.isB2b == null)
                //{
                //    try
                //    {
                //        PosApplication.Instance.Triggers.Invoke<ICustomerTrigger>((Action<ICustomerTrigger>)(t => t.PostCustomer((IPosTransaction)transaction)));

                //        //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 
                //        //LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(string.Format("{0} {1}. Changed quantity", currentLine.Description, currentLine.ItemId));
                //    }
                //    catch (Exception ex)
                //    {
                //        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                //    }
                //}
                

                if (isEmptyCustomer == false)
                {




                    int indexLines = 0;
                    
                    string isB2bCust = APIAccess.APIAccessClass.isB2b;
                    string priceGroup = APIAccess.APIAccessClass.priceGroup;//.ToString();
                    string lineDiscGroup = APIAccess.APIAccessClass.lineDiscGroup;//.ToString();
                    if (isB2bCust == "0")
                        foreach (var salesLine in transaction.CalculableSalesLines)
                        {

                            foreach (var lineDiscount in salesLine.DiscountLines.ToList())
                            {

                                if (lineDiscount.ToString() == "LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem")
                                {

                                    salesLine.DiscountLines.Remove(lineDiscount);


                                }
                                indexLines++;
                            }

                        }//end custom
                }
                
                 
                retailTransaction.IsDiscountFullyCalculated = true;
#if DEBUG
                newTimeElapsed = DateTime.Now.Subtract(startNewTime);

                // Compare the old and new discount engine values
                if (rt != null)
                {
                    string oldDiscountResult = string.Empty;
                    string newDiscountResult = string.Empty;
                    string resultStatus = "Equal";
                    decimal oldDiscountAmount = 0M;
                    decimal newDiscountAmount = 0M;

                    // Build old discount result and amount
                    foreach (var line in rt.SaleItems)
                    {
                        if (line.DiscountLines != null)
                        {
                            foreach (var discountLine in line.DiscountLines)
                            {
                                PeriodicDiscountItem pd = discountLine as PeriodicDiscountItem;
                                string offerId = pd != null ? pd.OfferId : string.Empty;
                                string offerName = pd != null ? pd.OfferName : string.Empty;
                                oldDiscountAmount += line.Quantity * (discountLine.Amount + (discountLine.Percentage / 100.0M * line.Price));
                                oldDiscountResult += GetDescriptionForDiscountOnItem(line.Quantity, line.Description, line.Price, discountLine.Percentage, discountLine.Amount, offerId, offerName);
                            }
                        }
                    }

                    // Now build the new discount result and amount
                    foreach (var line in transaction.SaleItems)
                    {
                        if (line.DiscountLines != null)
                        {
                            foreach (var discountLine in line.DiscountLines)
                            {
                                PeriodicDiscountItem pd = discountLine as PeriodicDiscountItem;
                                string offerId = pd != null ? pd.OfferId : string.Empty;
                                string offerName = pd != null ? pd.OfferName : string.Empty;
                                newDiscountAmount += line.Quantity * (discountLine.Amount + (discountLine.Percentage / 100.0M * line.Price));
                                newDiscountResult += GetDescriptionForDiscountOnItem(line.Quantity, line.Description, line.Price, discountLine.Percentage, discountLine.Amount, offerId, offerName);
                            }
                        }
                    }

                    if (newDiscountAmount != oldDiscountAmount)
                    {
                        resultStatus = "Different";
                    }

                    if ((!string.IsNullOrEmpty(alwaysShowComparisonResults) && alwaysShowComparisonResults.ToUpperInvariant().StartsWith("Y")) || resultStatus != "Equal")
                    {
                        MessageBox.Show(string.Format("{0}\nOld Discount: {1}\nNew Discount: {2}", resultStatus, oldDiscountResult, newDiscountResult), string.Format("Discount comparison - Old: {0}ms, New: {1}ms", oldTimeElapsed.TotalMilliseconds, newTimeElapsed.TotalMilliseconds));
                    }
                }
#endif
            }

            retailTransaction.IsDiscountFullyCalculated = true;
        }

        /// <summary>
        /// Calculates all of the discounts for the transactions.
        /// </summary>
        /// <param name="retailTransaction"></param>
        public void CalculateSimpleDiscount(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;
            if (transaction == null)
            {
                NetTracer.Error("retailTransaction parameter is null");
                throw new ArgumentNullException("retailTransaction");
            }

            ICustomerOrderTransaction orderTransaction = retailTransaction as ICustomerOrderTransaction;
            bool priceLock = (orderTransaction == null) ? false : (orderTransaction.LockPrices);

            if (!priceLock)
            {
                var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
                using (CommerceRuntime commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal))
                {
                    //RequestContext requestContext = commerceRuntime.CreateRequestContext(null);

                    //CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                    //Microsoft.Dynamics.Commerce.Runtime.Services.IService svc = commerceRuntime.GetService(ServiceTypes.PricingService);
                    //GetPriceServiceResponse response = svc.Execute<GetPriceServiceResponse>(new CalculateDiscountsServiceRequest(requestContext, crtTransaction, CRTDM.DiscountCalculationMode.CalculateOffer));
                    //CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                    CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                    GetPriceServiceResponse response = commerceRuntime.Execute<GetPriceServiceResponse>(new CalculateDiscountsServiceRequest(crtTransaction, CRTDM.DiscountCalculationMode.CalculateOffer), null);
                    CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                }

                retailTransaction.IsDiscountFullyCalculated = false;
            }
        }

        #region CustomerDiscount
        /// <summary>
        /// Add total discount amount to the value amount.
        /// </summary>
        /// <param name="retailTransaction"></param>
        /// <param name="amountValue"></param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public void AddTotalDiscountAmount(IRetailTransaction retailTransaction, decimal amountValue)
        {
            retailTransaction.SetTotalDiscAmount(amountValue);
        }

        /// <summary>
        /// Add loyalty points discount amount to the value amount
        /// </summary>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <param name="amountValue">The amount to be set</param>
        public void AddLoyaltyDiscountAmount(IRetailTransaction retailTransaction, decimal amountValue)
        {
            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            retailTransaction.SetLoyaltyDiscAmount(amountValue);
        }

        /// <summary>
        /// Returns true if total discount amount is authorized.
        /// </summary>
        /// <param name="retailTransaction"></param>
        /// <param name="amountValue"></param>
        /// <param name="maxAmountValue"></param>
        /// <returns></returns>
        public bool AuthorizeTotalDiscountAmount(IRetailTransaction retailTransaction, decimal amountValue, decimal maxAmountValue)
        {
            if (retailTransaction == null)
            {
                NetTracer.Warning("retailTransaction parameter is null");
                throw new ArgumentNullException("retailTransaction");
            }

            bool returnValue = true;
            decimal tempGrossAmount = retailTransaction.GrossAmount - retailTransaction.LineDiscount - retailTransaction.PeriodicDiscountAmount;

            if (Discount.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled()
                && !Discount.InternalApplication.Services.Peripherals.FiscalPrinter.AuthorizeTotalDiscountAmount(retailTransaction, amountValue, maxAmountValue))
            {
                returnValue = false;
            }
            else if (amountValue > Math.Abs(tempGrossAmount))
            {
                returnValue = false;
                string message = ApplicationLocalizer.Language.Translate(3178); //The discount amount is to high. The discount amount cannot exceed the balance due.
                using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }
            else if (amountValue > Math.Abs(maxAmountValue))
            {
                returnValue = false;
                string maximumAmountRounded = this.Application.Services.Rounding.Round(maxAmountValue, true);
                decimal maximumDiscountPct = (tempGrossAmount == decimal.Zero) ? decimal.Zero : (100m * maxAmountValue / tempGrossAmount);

                string message = ApplicationLocalizer.Language.Translate(3173, maximumAmountRounded, maximumDiscountPct.ToString("n2")); //The discount amount is to high. The discount percentage limit is set to xxxx %.
                using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Sets total discount percentage as per the given discount percentage.
        /// </summary>
        /// <param name="retailTransaction"></param>
        /// <param name="percentValue"></param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public void AddTotalDiscountPercent(IRetailTransaction retailTransaction, decimal percentValue)
        {
            retailTransaction.SetTotalDiscPercent(percentValue);
        }

        /// <summary>
        /// Returns true if total discount percent is authorized.
        /// </summary>
        /// <param name="retailTransaction"></param>
        /// <param name="percentValue"></param>
        /// <param name="maxPercentValue"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public bool AuthorizeTotalDiscountPercent(IRetailTransaction retailTransaction, decimal percentValue, decimal maxPercentValue)
        {
            bool returnValue = true;
            decimal tempGrossAmount = retailTransaction.GrossAmount - retailTransaction.LineDiscount - retailTransaction.PeriodicDiscountAmount;

            if (Discount.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled()
                && !Discount.InternalApplication.Services.Peripherals.FiscalPrinter.AuthorizeTotalDiscountPercent(retailTransaction, percentValue, maxPercentValue))
            {
                returnValue = false;
            }
            else if (percentValue > 100m || percentValue > maxPercentValue)
            {
                string message = string.Format(LSRetailPosis.ApplicationLocalizer.Language.Translate(3511), maxPercentValue.ToString("n2")); //The discount percentage is to high. The maximum limit is set to xxx %.
                using (frmMessage dialog = new frmMessage(message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information))
                {
                    this.Application.ApplicationFramework.POSShowForm(dialog);
                }

                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Adds disount item to the given discount item.
        /// </summary>
        /// <param name="lineItem"></param>
        /// <param name="discountItem"></param>
        public void AddLineDiscountAmount(ISaleLineItem lineItem, ILineDiscountItem discountItem)
        {
            SaleLineItem salesLineIem = lineItem as SaleLineItem;
            if (salesLineIem != null && discountItem != null)
            {
                salesLineIem.ResetManualLineDiscounts();
                salesLineIem.LineManualDiscountAmount = this.Application.Services.Rounding.Round(discountItem.Amount * salesLineIem.Quantity);
            }
        }

        /// <summary>
        /// Add discount line
        /// </summary>
        /// <param name="lineItem">Line to which discount would be added</param>
        /// <param name="discountItem">Discount item to add</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void AddDiscountLine(ISaleLineItem lineItem, IDiscountItem discountItem)
        {
            ((SaleLineItem)lineItem).Add((DiscountItem)discountItem);
        }

        /// <summary>
        /// Returns true if discount amount is authorized.
        /// </summary>
        /// <param name="saleLineItem"></param>
        /// <param name="lineDiscountItem"></param>
        /// <param name="maximumDiscountAmt"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1"), SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public bool AuthorizeLineDiscountAmount(ISaleLineItem saleLineItem, ILineDiscountItem lineDiscountItem, decimal maximumDiscountAmt)
        {
            bool returnValue = true;
            decimal itemPriceWithoutDiscount = saleLineItem.Price * saleLineItem.Quantity;
            decimal tempItemAmount = saleLineItem.NetAmount - lineDiscountItem.Amount; 
            maximumDiscountAmt *= saleLineItem.Quantity;

            if (Discount.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled()
                && !Discount.InternalApplication.Services.Peripherals.FiscalPrinter.AuthorizeLineDiscountAmount(saleLineItem, lineDiscountItem, maximumDiscountAmt))
            {
                returnValue = false;
            }
            else if (lineDiscountItem.Amount > Math.Abs(itemPriceWithoutDiscount) || tempItemAmount < decimal.Zero)
            {
                returnValue = false;
                string message = ApplicationLocalizer.Language.Translate(3177); //The discount amount is to high. The discount amount cannot exceed the item price.
                using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }
            else if (lineDiscountItem.Amount > Math.Abs(maximumDiscountAmt))
            {
                returnValue = false;
                string maximumAmountRounded = this.Application.Services.Rounding.Round(maximumDiscountAmt, true);
                decimal maximumDiscountPct = (itemPriceWithoutDiscount == decimal.Zero) ? decimal.Zero : (100m * maximumDiscountAmt / itemPriceWithoutDiscount);
                string message = ApplicationLocalizer.Language.Translate(3173, maximumAmountRounded, maximumDiscountPct.ToString("n2")); //The discount amount is to high. The discount percentage limit is set to xxxx %.

                using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Add disount percent to the given discount item.
        /// </summary>
        /// <param name="lineItem"></param>
        /// <param name="discountItem"></param>
        public void AddLineDiscountPercent(ISaleLineItem lineItem, ILineDiscountItem discountItem)
        {
            SaleLineItem salesLineIem = lineItem as SaleLineItem;
            if (salesLineIem != null && discountItem != null)
            {
                salesLineIem.ResetManualLineDiscounts();
                salesLineIem.LineManualDiscountPercentage = discountItem.Percentage;
            }
        }

        /// <summary>
        /// Returns true if discount percent is correct.
        /// </summary>
        /// <param name="saleLineItem"></param>
        /// <param name="lineDiscountItem"></param>
        /// <param name="maximumDiscountPct"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1"), SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public bool AuthorizeLineDiscountPercent(ISaleLineItem saleLineItem, ILineDiscountItem lineDiscountItem, decimal maximumDiscountPct)
        {
            bool returnValue = true;
            decimal itemPriceWithoutDiscount = saleLineItem.Price * saleLineItem.Quantity;

            if (Discount.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled()
                && !Discount.InternalApplication.Services.Peripherals.FiscalPrinter.AuthorizeLineDiscountPercent(saleLineItem, lineDiscountItem, maximumDiscountPct))
            {
                returnValue = false;
            }
            else if (lineDiscountItem.Percentage > 100m || lineDiscountItem.Percentage > maximumDiscountPct)
            {
                string message = string.Format(LSRetailPosis.ApplicationLocalizer.Language.Translate(3183), maximumDiscountPct.ToString("n2")); //The discount percentage is to high. The limit is xxx %.
                using (frmMessage dialog = new frmMessage(message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information))
                {
                    this.Application.ApplicationFramework.POSShowForm(dialog);
                }

                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Calculate the customer discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Calc", Justification = "Cannot change public API.")]
        public void CalcCustomerDiscount(RetailTransaction retailTransaction)
        {
            //Calc line discount
            retailTransaction = (RetailTransaction)CalcLineDiscount(retailTransaction);

            //Calc multiline discount
            retailTransaction = (RetailTransaction)CalcMultiLineDiscount(retailTransaction);

            //Calc total discount
            retailTransaction = (RetailTransaction)CalcTotalCustomerDiscount(retailTransaction);
        }
    
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification="Revisit. Don't want to risk refactoring right now.")]
        private void GetLineDiscountLines(RetailTransaction retailTransaction, SaleLineItem saleItem, string currencyCode, ref decimal absQty, ref decimal discountAmount, ref decimal percent1, ref decimal percent2, ref decimal minQty)
        {
            int idx = 0;
            while (idx < 9)
            {
                int itemCode = idx % 3;    //Mod divsion
                int accountCode = idx / 3;

                string accountRelation = (accountCode == (int)PriceDiscAccountCode.Table) ? retailTransaction.Customer.CustomerId : (accountCode == (int)PriceDiscAccountCode.GroupId) ? retailTransaction.Customer.LineDiscountGroup : string.Empty;
                string itemRelation = (itemCode == (int)PriceDiscItemCode.Table) ? saleItem.ItemId : saleItem.LineDiscountGroup;

                if (accountRelation == null)
                {
                    accountRelation = string.Empty;
                }

                if (itemRelation == null)
                {
                    itemRelation = string.Empty;
                }

                PriceDiscType relation = PriceDiscType.LineDiscSales; //Sales line discount - 5

                if (DiscountParameters.Activation(relation, (PriceDiscAccountCode)accountCode, (PriceDiscItemCode)itemCode))
                {
                    if ((ValidRelation((PriceDiscAccountCode)accountCode, accountRelation)) &&
                        (ValidRelation((PriceDiscItemCode)itemCode, itemRelation)))
                    {
                        try
                        {
                            bool dimensionDiscountFound = false;

                            if (!string.IsNullOrEmpty(saleItem.Dimension.VariantId))
                            {
                                var dimensionPriceDiscTable = GetPriceDiscDataCached(retailTransaction.TransactionId, relation, itemRelation, accountRelation, itemCode, accountCode, absQty, currencyCode, saleItem.Dimension, true);

                                foreach (DiscountAgreementArgs row in dimensionPriceDiscTable)
                                {
                                    bool unitsAreUndefinedOrEqual =
                                        (String.IsNullOrEmpty(row.UnitId) ||
                                         String.Equals(row.UnitId, saleItem.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase));

                                    if (unitsAreUndefinedOrEqual)
                                    {
                                        percent1 += row.Percent1;
                                        percent2 += row.Percent2;
                                        discountAmount += row.Amount;
                                        minQty += row.QuantityAmountFrom;
                                    }

                                    if (percent1 > 0M || percent2 > 0M || discountAmount > 0M)
                                    {
                                        dimensionDiscountFound = true;
                                    }

                                    if (!row.SearchAgain)
                                    {
                                        idx = 9;
                                    }
                                }
                            }

                            if (!dimensionDiscountFound)
                            {
                                var priceDiscTable = GetPriceDiscDataCached(retailTransaction.TransactionId, relation, itemRelation, accountRelation, itemCode, accountCode, absQty, currencyCode, saleItem.Dimension, false);

                                foreach (DiscountAgreementArgs row in priceDiscTable)
                                {
                                    bool unitsAreUndefinedOrEqual =
                                        (String.IsNullOrEmpty(row.UnitId) ||
                                         String.Equals(row.UnitId, saleItem.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase));

                                    if (unitsAreUndefinedOrEqual)
                                    {
                                        percent1 += row.Percent1;
                                        percent2 += row.Percent2;
                                        discountAmount += row.Amount;
                                        minQty += row.QuantityAmountFrom;
                                    }

                                    if (!row.SearchAgain)
                                    {
                                        idx = 9;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            NetTracer.Error(ex, "Discount::GetLineDiscountLines failed for retailTransaction {0} saleItem {1} currencyCode {2}", retailTransaction, saleItem, currencyCode);
                            LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                            throw;
                        }
                    }
                }

                idx++;
            }
        }

        /// <summary>
        /// The calculation of a customer line discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction.</returns>
        private IRetailTransaction CalcLineDiscount(RetailTransaction retailTransaction)
        {
            //Loop trough all items all calc line discount
            foreach (SaleLineItem saleItem in retailTransaction.CalculableSalesLines)
            {
                decimal absQty = Math.Abs(saleItem.Quantity);
                decimal discountAmount = 0m;
                decimal percent1 = 0m;
                decimal percent2 = 0m;
                decimal minQty = 0m;

                GetLineDiscountLines(retailTransaction, saleItem, retailTransaction.StoreCurrencyCode, ref absQty, ref discountAmount, ref percent1, ref percent2, ref minQty);

                if (percent1 == 0M
                    && percent2 == 0M
                    && discountAmount == 0M
                    && (ApplicationSettings.Terminal.StoreCurrency != ApplicationSettings.Terminal.CompanyCurrency))
                {
                    GetLineDiscountLines(retailTransaction, saleItem, ApplicationSettings.Terminal.CompanyCurrency, ref absQty, ref discountAmount, ref percent1, ref percent2, ref minQty);
                    discountAmount = this.Application.Services.Currency.CurrencyToCurrency(ApplicationSettings.Terminal.CompanyCurrency, ApplicationSettings.Terminal.StoreCurrency, discountAmount);
                }

                decimal totalPercentage = (1 - (1 - (percent1 / 100)) * (1 - (percent2 / 100))) * 100;

                if (((totalPercentage != 0m) || (discountAmount != 0m)) && (!saleItem.DiscountsWereRemoved))
                {

                    CustomerDiscountItem discountItem = (CustomerDiscountItem)this.Utility.CreateCustomerDiscountItem();
                    discountItem.LineDiscountType = DiscountTypes.Customer;
                    discountItem.CustomerDiscountType = CustomerDiscountTypes.LineDiscount;
                    discountItem.Percentage = totalPercentage;
                    discountItem.Amount = discountAmount;

                    UpdateDiscountLines(saleItem, discountItem);
                }
            }

            return retailTransaction;
        }

        /// <summary>
        /// The calculation of a customer multiline discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction.</returns>
        private IRetailTransaction CalcMultiLineDiscount(RetailTransaction retailTransaction)
        {
            string storeCurrency = ApplicationSettings.Terminal.StoreCurrency;
            string companyCurrency = ApplicationSettings.Terminal.CompanyCurrency;
            var multilineDiscountCalculator = new MultilineDiscountCalculator(this.DiscountParameters, this.Application, this, storeCurrency, companyCurrency);

            return multilineDiscountCalculator.CalcMultiLineDiscount(retailTransaction);
        }

        /// <summary>
        /// The calculation of the total customer discount.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>The retail transaction.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfather")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Needs refactoring")]
        private IRetailTransaction CalcTotalCustomerDiscount(RetailTransaction retailTransaction)
        {
            decimal totalAmount = 0;

           /** 
             * This is added for performance improvement when transaction have multiple lines. Earlier, this code used to clone each line item.
             * Clone method is using BinaryFormatter::Serialize and BinaryFormatter::Deserialize of memory stream as transaction line.
             * BinaryFormatter::Serialize and BinaryFormatter::Deserialize were taking 90% CPU on POS Client Code while adding an item apart from 
             * few of SQL Queries. This is the reason to CloneTransaction and it's line in one call insteading of cloning lineItems individually. 
             * This has shown 60% Performance improvement in larger dataset and at least 10-20% improvement on smaller data set. 
             * Don't see any memory pressure due to this change. 
             * */

            // Duplicate the transaction
            RetailTransaction rt = (RetailTransaction)retailTransaction.CloneTransaction();

            //Find the total amount as a basis for the total discount
            foreach (ISaleLineItem saleItem in rt.CalculableSalesLines)
            {
                if ((saleItem.IncludedInTotalDiscount) && (!saleItem.DiscountsWereRemoved) && (!saleItem.Voided))
                {
                    saleItem.CalculateLine();
                    totalAmount += saleItem.NetAmountWithAllInclusiveTax;
                }
            }

            decimal absTotalAmount = Math.Abs(totalAmount);

            //Find the total discounts.
            PriceDiscType relation = PriceDiscType.EndDiscSales; //Total sales discount - 7
            int itemCode = (int)PriceDiscItemCode.All; //All items - 2
            int accountCode = 0;
            string itemRelation = string.Empty;
            decimal percent1 = 0m;
            decimal percent2 = 0m;
            decimal discountAmount = 0m;
            Dimensions dimension = (Dimensions)this.Utility.CreateDimension();

            int idx = 0;
            while (idx < /* Max(PriceDiscAccountCode) */ 3)
            {   // Check discounts for Store Currency 
                accountCode = idx;

                string accountRelation = (accountCode == (int)PriceDiscAccountCode.Table) ? retailTransaction.Customer.CustomerId : (accountCode == (int)PriceDiscAccountCode.GroupId) ? retailTransaction.Customer.TotalDiscountGroup : string.Empty;
                accountRelation = accountRelation ?? String.Empty;

                // Only get Active discount combinations
                if (DiscountParameters.Activation(relation, (PriceDiscAccountCode)accountCode, (PriceDiscItemCode)itemCode))
                {
                    var priceDiscTable = GetPriceDiscDataCached(retailTransaction.TransactionId, relation, itemRelation, accountRelation, itemCode, accountCode, absTotalAmount, retailTransaction.StoreCurrencyCode, dimension, false);

                    foreach (DiscountAgreementArgs row in priceDiscTable)
                    {
                        percent1 += row.Percent1;
                        percent2 += row.Percent2;
                        discountAmount += row.Amount;

                        if (!row.SearchAgain)
                        {
                            idx = 3;
                        }
                    }
                }
                idx++;
            }

            if (percent1 == 0M && percent2 == 0M && discountAmount == 0M && (ApplicationSettings.Terminal.CompanyCurrency != ApplicationSettings.Terminal.StoreCurrency))
            {
                idx = 0;
                while (idx < /* Max(PriceDiscAccountCode) */ 3)
                {   // Check discounts for Company Currency 
                    accountCode = idx;

                    string accountRelation = (accountCode == (int)PriceDiscAccountCode.Table) ? retailTransaction.Customer.CustomerId : (accountCode == (int)PriceDiscAccountCode.GroupId) ? retailTransaction.Customer.TotalDiscountGroup : string.Empty;

                    // Only get Active discount combinations
                    if (DiscountParameters.Activation(relation, (PriceDiscAccountCode)accountCode, (PriceDiscItemCode)itemCode))
                    {
                        var priceDiscTable = GetPriceDiscDataCached(retailTransaction.TransactionId, relation, itemRelation, accountRelation, itemCode, accountCode, absTotalAmount, ApplicationSettings.Terminal.CompanyCurrency, dimension, false);

                        foreach (DiscountAgreementArgs row in priceDiscTable)
                        {
                            percent1 += row.Percent1;
                            percent2 += row.Percent2;
                            discountAmount += row.Amount;

                            if (!row.SearchAgain)
                            {
                                idx = 3;
                            }
                        }
                    }

                    idx++;
                }

                discountAmount = this.Application.Services.Currency.CurrencyToCurrency(ApplicationSettings.Terminal.CompanyCurrency, ApplicationSettings.Terminal.StoreCurrency, discountAmount);
            }

            decimal totalPercentage = (1 - (1 - (percent1 / 100)) * (1 - (percent2 / 100))) * 100;

            if (discountAmount != decimal.Zero)
            {
                retailTransaction.AddTotalDiscAmountLines(typeof(CustomerDiscountItem), discountAmount);
            }

            if (totalPercentage != 0)
            {
                //Update the sale items.
                foreach (SaleLineItem saleItem in retailTransaction.CalculableSalesLines)
                {
                    if (saleItem.IncludedInTotalDiscount)
                    {
                        CustomerDiscountItem discountItem = saleItem.GetCustomerDiscountItem(CustomerDiscountTypes.TotalDiscount, DiscountTypes.Customer);
                        discountItem.Percentage = totalPercentage;
                    }
                }
            }

            return retailTransaction;
        }

        internal ReadOnlyCollection<DiscountAgreementArgs> GetPriceDiscDataCached(string cacheToken, PriceDiscType relation,
            string itemRelation, string accountRelation, int itemCode, int accountCode,
            decimal quantityAmount, string targetCurrencyCode, Dimensions itemDimensions, bool includeDimensions)
        {
            if (!cacheToken.Equals(priceDiscDataCacheToken, StringComparison.OrdinalIgnoreCase))
            {   // Clear the cache and update the token
                priceDiscDataCache.Clear();
                priceDiscDataCacheToken = cacheToken;
            }
            else
            {   // Check the cache...

                // Linq Solution (best measured over hash, for-each)
                var tmp = (from item in priceDiscDataCache
                           where
                               (item.Relation == relation) &&
                               (item.ItemRelation == itemRelation) &&
                               (item.AccountRelation == accountRelation) &&
                               (item.ItemCode == itemCode) &&
                               (item.AccountCode == accountCode) &&
                               (item.QuantityAmount == quantityAmount) &&
                               (item.TargetCurrencyCode == targetCurrencyCode) &&
                               (item.ItemDimensions.IsEquivalent(itemDimensions)) &&
                               (item.IncludeDimensions == includeDimensions)
                           select item).FirstOrDefault();

                if (tmp != null)
                {
                    return tmp.SqlResults;
                }
            }

            // Item not found in cache - Get from SQL and add to the cache
            var result = GetPriceDiscData(relation, itemRelation, accountRelation, itemCode, accountCode, quantityAmount, targetCurrencyCode, itemDimensions, includeDimensions);
            priceDiscDataCache.Add(new PriceDiscData(result, relation, itemRelation, accountRelation, itemCode, accountCode, quantityAmount, targetCurrencyCode, itemDimensions, includeDimensions));

            return result;
        }

        internal struct DiscountAgreementArgs
        {
            public Decimal Percent1 { get; set; }
            public Decimal Percent2 { get; set; }
            public Decimal Amount { get; set; }
            public Decimal QuantityAmountFrom { get; set; }
            public String UnitId { get; set; }
            //            public Decimal QuantityAmountTo { get; set; }
            public bool SearchAgain { get; set; }
            //public string InventDimId { get; set; }
            //public string InventStyleId { get; set; }
            //public string InventColorId { get; set; }
            //public string InventSizeid { get; set; }
        }

        /// <summary>
        /// Gets the discount data.
        /// </summary>
        /// <remarks>Caller is responsible for disposing returned object</remarks>
        /// <param name="relation">The relation (line,mulitline,total)</param>
        /// <param name="itemRelation">The item relation</param>
        /// <param name="accountRelation">The account relation</param>
        /// <param name="itemCode">The item code (table,group,all)</param>
        /// <param name="accountCode">The account code(table,group,all)</param>
        /// <param name="quantityAmount">The quantity or amount that sets the minimum quantity or amount needed</param>
        /// <param name="targetCurrencyCode">The store or company currency</param>
        /// <returns></returns>
        private ReadOnlyCollection<DiscountAgreementArgs> GetPriceDiscData(
            PriceDiscType relation,
            string itemRelation,
            string accountRelation,
            int itemCode,
            int accountCode,
            decimal quantityAmount,
            string targetCurrencyCode,
            Contracts.DataEntity.IDimension itemDimensions,
            bool includeDimensions)
        {
            accountRelation = accountRelation ?? string.Empty;
            itemRelation = itemRelation ?? string.Empty;
            targetCurrencyCode = targetCurrencyCode ?? string.Empty;
            string inventColorId = ((itemDimensions.ColorId != null && includeDimensions) ? itemDimensions.ColorId : string.Empty);
            string inventSizeId = ((itemDimensions.SizeId != null && includeDimensions) ? itemDimensions.SizeId : string.Empty);
            string inventStyleId = ((itemDimensions.StyleId != null && includeDimensions) ? itemDimensions.StyleId : string.Empty);

            var discountAgreements = this.discountDataManager.GetPriceDiscData((int)relation,
                itemRelation, accountRelation, itemCode, accountCode,
                quantityAmount, targetCurrencyCode, inventColorId, inventSizeId, inventStyleId, this.NoDate);

            // can't use initializer in select of Linq-to-Entities query
            return discountAgreements.Select(t => new DiscountAgreementArgs
            {
                Percent1 = t.Percent1,
                Percent2 = t.Percent2,
                Amount = t.Amount,
                UnitId = t.UnitId,
                QuantityAmountFrom = t.QuantityAmountFrom,
                //                QuantityAmountTo = t.QuantityAmountTo,
                SearchAgain = t.ShouldSearchAgain == 0 ? false : true
            }).ToList().AsReadOnly();
        }

        /// <summary>
        /// Update the discount items.
        /// </summary>
        /// <param name="saleItem">The item line that the discount line is added to.</param>
        /// <param name="discountItem">The new discount item</param>
        internal static void UpdateDiscountLines(SaleLineItem saleItem, CustomerDiscountItem discountItem)
        {
            //Check if line discount is found, if so then update
            bool discountLineFound = false;
            foreach (DiscountItem discLine in saleItem.DiscountLines)
            {
                CustomerDiscountItem customerDiscLine = discLine as CustomerDiscountItem;
                if (customerDiscLine != null)
                {
                    //If found then update
                    if ((customerDiscLine.LineDiscountType == discountItem.LineDiscountType) &&
                        (customerDiscLine.CustomerDiscountType == discountItem.CustomerDiscountType))
                    {
                        customerDiscLine.Percentage = discountItem.Percentage;
                        customerDiscLine.Amount = discountItem.Amount;
                        discountLineFound = true;
                    }
                }
            }
            //If line discount is not found then add it.
            if (!discountLineFound)
            {
                saleItem.Add(discountItem);
            }

            saleItem.WasChanged = true;
        }

        /// <summary>
        /// Is there a valid relation between the itemcode and relation?
        /// </summary>
        /// <param name="itemCode">The item code (table,group,all)</param>
        /// <param name="relation">The item relation</param>
        /// <returns>Returns true if the relation ok, else false.</returns>
        internal static bool ValidRelation(PriceDiscItemCode itemCode, string relation)
        {
            System.Diagnostics.Debug.Assert(relation != null, "relation should not be null.");

            bool ok = true;

            if (!string.IsNullOrEmpty(relation) && (itemCode == PriceDiscItemCode.All))
            {
                ok = false;
            }

            if (string.IsNullOrEmpty(relation) && (itemCode != PriceDiscItemCode.All))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Is there a valid relation between the accountcode and relation?
        /// </summary>
        /// <param name="accountCode">The account code (table,group,all).</param>
        /// <param name="relation">The account relation.</param>
        /// <returns></returns>
        internal static bool ValidRelation(PriceDiscAccountCode accountCode, string relation)
        {
            System.Diagnostics.Debug.Assert(relation != null, "relation should not be null.");

            bool ok = true;

            if (!string.IsNullOrEmpty(relation) && (accountCode == PriceDiscAccountCode.All))
            {
                ok = false;
            }

            if (string.IsNullOrEmpty(relation) && (accountCode != PriceDiscAccountCode.All))
            {
                ok = false;
            }

            return ok;
        }

        #endregion CustomerDiscount

        #region PeriodicDiscount

        /// <summary>
        /// Find the discount validation period and determines if it is active for the given date and time
        /// </summary>
        /// <param name="validationPeriod">Period Id of the validation period to check</param>
        /// <param name="transDateTime">Date/time to validate</param>
        /// <returns>Boolean indicating whether the validation period is active for the given date and time</returns>
        public bool IsValidationPeriodActive(string validationPeriodId, DateTime transDateTime)
        {
            if (string.IsNullOrEmpty(validationPeriodId))  //then it is always a valid period
            {
                NetTracer.Information("validationPeriodId parameter is null");
                return true;
            }

            DE.RetailDiscountValidationPeriod validationPeriod = discountDataManager.GetDiscValidationPeriod(validationPeriodId);
            DateTime transDate = transDateTime.Date;
            TimeSpan transTime = transDateTime.TimeOfDay;

            if (validationPeriod == null) //If validation period is not found
            {
                NetTracer.Information("validationPeriod is null");
                return true;
            }

            //Is the discount valid within the start and end date period?
            if (IsDateWithinStartEndDate(transDate, validationPeriod.ValidFrom.Date, validationPeriod.ValidTo.Date))
            {
                bool answerFound = false;
                bool isActive = false;

                // does today's configuration tell if period is active?
                if (IsRangeDefinedForDay(validationPeriod, transDate.DayOfWeek))
                {
                    isActive = IsPeriodActiveForDayAndTime(validationPeriod, transDate.DayOfWeek, transTime, false);
                    answerFound = true;
                }

                // if we don't know or got negative result, see if yesterday will activate it (if its range ends after midnight)
                DayOfWeek yesterday = transDate.AddDays(-1).DayOfWeek;
                bool lastRangeDefinedAfterMidnight =
                IsRangeDefinedForDay(validationPeriod, yesterday) && validationPeriod.IsEndTimeAfterMidnightForDay(yesterday);

                if ((!answerFound || isActive == false) && lastRangeDefinedAfterMidnight)
                {
                    // if yesterday makes it active, set isActive = true
                    isActive = IsPeriodActiveForDayAndTime(validationPeriod, yesterday, transTime, true);
                    answerFound = true;
                }

                // if we still don't know, try using general configuration
                if (!answerFound)
                {
                    var configuration = new PeriodRangeConfiguration
                    {
                        StartTime = validationPeriod.StartingTime,
                        EndTime = validationPeriod.EndingTime,
                        IsActiveOnlyWithinBounds = validationPeriod.IsTimeBounded != 0,
                        EndsTomorrow = validationPeriod.IsEndTimeAfterMidnight != 0
                    };

                    if ((validationPeriod.StartingTime != 0) && (validationPeriod.EndingTime != 0))
                    {
                        int currentTime = Convert.ToInt32(transTime.TotalSeconds);
                        isActive = IsTimeActiveForConfiguration(currentTime, configuration, false);
                        answerFound = true;
                    }
                }

                return answerFound ? isActive : (validationPeriod.IsTimeBounded == 1);
            }

            // not within date range, so active if not set to be within date range
            return validationPeriod.IsTimeBounded != 1;
        }

        private static bool IsRangeDefinedForDay(DE.RetailDiscountValidationPeriod period, DayOfWeek day)
        {
            return (period.StartingTimeForDay(day) != 0) && (period.EndingTimeForDay(day) != 0);
        }

        private static bool IsPeriodActiveForDayAndTime(DE.RetailDiscountValidationPeriod period, DayOfWeek day, TimeSpan time, bool testOnlyAfterMidnight)
        {
            var configuration = new PeriodRangeConfiguration
            {
                StartTime = period.StartingTimeForDay(day),
                EndTime = period.EndingTimeForDay(day),
                EndsTomorrow = period.IsEndTimeAfterMidnightForDay(day),
                IsActiveOnlyWithinBounds = period.IsTimeBoundedForDay(day),
            };

            Int32 currentTime = Convert.ToInt32(time.TotalSeconds);
            return IsTimeActiveForConfiguration(currentTime, configuration, testOnlyAfterMidnight);
        }

        /// <summary>
        /// For a given time, and period time-range setup, and whether to restrict our search to after midnight,
        ///  this method tells if the given time is active or inactive within the context of the range
        /// </summary>
        /// <param name="currentTime">Current time in seconds past midnight</param>
        /// <param name="configuration">Period time range setup parameters</param>
        /// <param name="testOnlyAfterMidnight">Whether we only check for activity after midnight</param>
        /// <returns>Result telling if given time is active in the configuration</returns>
        private static bool IsTimeActiveForConfiguration(Int32 currentTime, PeriodRangeConfiguration configuration, bool testOnlyAfterMidnight)
        {
            // if time falls between start and end times, return true if set to be active in range
            bool rangeAppliesBeforeMidnight = (configuration.StartTime <= currentTime &&
                ((configuration.EndTime >= currentTime) || configuration.EndsTomorrow));

            if (!testOnlyAfterMidnight && rangeAppliesBeforeMidnight)
            {
                return configuration.IsActiveOnlyWithinBounds;
            }

            // if time is before end time for ending times past midnight, return true if set to be active in range
            bool rangeAppliesAfterMidnight = (configuration.EndsTomorrow && configuration.EndTime >= currentTime);

            if (rangeAppliesAfterMidnight)
            {
                return configuration.IsActiveOnlyWithinBounds;
            }

            return !configuration.IsActiveOnlyWithinBounds;
        }

        /// <summary>
        /// Represent a time-range configuration for discount validation period
        ///  These ranges have a start and end time, indicator for ending past midnight, and 
        ///  flag indicated what finding a time in this range means (i.e. whether being in the range validates/invalidates the time)
        /// </summary>
        private struct PeriodRangeConfiguration
        {
            public Int32 StartTime;
            public Int32 EndTime;
            public bool EndsTomorrow;
            public bool IsActiveOnlyWithinBounds;
        }

        /// <summary>
        /// //Finds if a discount period is valid.
        /// </summary>
        /// <param name="periodId">The unique period id.</param>
        /// <param name="transDateTime">The date and time for the transaction.</param>
        /// <returns></returns>
        private PeriodStatus DiscountPeriodValid(string periodId, DateTime transDateTime)
        {
            return IsValidationPeriodActive(periodId, transDateTime) ? PeriodStatus.IsValid : PeriodStatus.IsInvalid;
        }

        private void MakeActiveOfferTables()
        {
            //Adding colums to activeOffers
            activeOffers.Columns.Add("OFFERID", typeof(string));
            activeOffers.Columns.Add("DESCRIPTION", typeof(string));
            activeOffers.Columns.Add("STATUS", typeof(int));
            activeOffers.Columns.Add("PDTYPE", typeof(int));
            activeOffers.Columns.Add("CONCURRENCYMODE", typeof(int));
            activeOffers.Columns.Add("DISCVALIDPERIODID", typeof(string));
            activeOffers.Columns.Add("DATEVALIDATIONTYPE", typeof(DateValidationTypes));
            activeOffers.Columns.Add("VALIDFROM", typeof(DateTime));
            activeOffers.Columns.Add("VALIDTO", typeof(DateTime));
            activeOffers.Columns.Add("DISCOUNTTYPE", typeof(int));
            activeOffers.Columns.Add("SAMEDIFFMMLINES", typeof(int));
            activeOffers.Columns.Add("NOOFLINESTOTRIGGER", typeof(int));
            activeOffers.Columns.Add("DEALPRICEVALUE", typeof(decimal));
            activeOffers.Columns.Add("DISCOUNTPCTVALUE", typeof(decimal));
            activeOffers.Columns.Add("DISCOUNTAMOUNTVALUE", typeof(decimal));
            activeOffers.Columns.Add("NOOFLEASTEXPENSIVEITEMS", typeof(int));
            activeOffers.Columns.Add("NOOFTIMESAPPLICABLE", typeof(int));
            activeOffers.Columns.Add("NOLINESTRIGGERED", typeof(int));      // The number of lines that have been triggerd in an mix and match offer. Shoulb be equal or less than NoOfLinesToTrigger.
            activeOffers.Columns.Add("NOOFTIMESACTIVATED", typeof(int));    // The number times the offer has been activated. Should be equal or less than NoOfTimesApplicable
            activeOffers.Columns.Add("ISDISCOUNTCODEREQUIRED", typeof(int));
            DataColumn[] primaryKey = new DataColumn[2];
            primaryKey[0] = activeOffers.Columns["OFFERID"];
            primaryKey[1] = activeOffers.Columns["PDTYPE"];
            activeOffers.PrimaryKey = primaryKey;

            //Adding columns to activeOfferLines
            activeOfferLines.Columns.Add("OFFERID", typeof(string));            // The offer id
            activeOfferLines.Columns.Add("LINEID", typeof(int));                // The offer line id
            activeOfferLines.Columns.Add("PRODUCTID", typeof(Int64));
            activeOfferLines.Columns.Add("DISTINCTPRODUCTVARIANTID", typeof(Int64));
            activeOfferLines.Columns.Add("SALELINEID", typeof(int));            // The retailtransaction sale line id
            activeOfferLines.Columns.Add("QUANTITY", typeof(decimal));          // The item quantity
            activeOfferLines.Columns.Add("UNIT", typeof(string));               // The item unit (UOM)
            activeOfferLines.Columns.Add("DEALPRICEORDISCPCT", typeof(decimal));// The deal price or discount percentage
            activeOfferLines.Columns.Add("LINEGROUP", typeof(string));
            activeOfferLines.Columns.Add("DISCTYPE", typeof(int));
            activeOfferLines.Columns.Add("STATUS", typeof(int));
            activeOfferLines.Columns.Add("NOOFITEMSNEEDED", typeof(int));
            activeOfferLines.Columns.Add("MIXMATCHPRIORITY", typeof(decimal)); // The order priority for the mix and match lines, higher values give higher priority

            // columns for Discount Offer specific details
            activeOfferLines.Columns.Add("DISCOUNTMETHOD", typeof(int));    // Dicount Offer method
            activeOfferLines.Columns.Add("DISCAMOUNT", typeof(decimal));    // Discount amount
            activeOfferLines.Columns.Add("DISCPCT", typeof(decimal));       // Discount percent
            activeOfferLines.Columns.Add("OFFERPRICE", typeof(decimal));    // Offer price
            activeOfferLines.Columns.Add("OFFERPRICEINCLTAX", typeof(decimal)); // Offer Price (w/ tax)

            // column for Threshold specific details
            activeOfferLines.Columns.Add("COUNTNONDISCOUNTITEMS", typeof(bool));

            // Primary Key
            DataColumn[] primaryLineKey = new DataColumn[5];
            primaryLineKey[0] = activeOfferLines.Columns["OFFERID"];
            primaryLineKey[1] = activeOfferLines.Columns["PRODUCTID"];
            primaryLineKey[2] = activeOfferLines.Columns["DISTINCTPRODUCTVARIANTID"];
            primaryLineKey[3] = activeOfferLines.Columns["SALELINEID"];
            primaryLineKey[4] = activeOfferLines.Columns["DEALPRICEORDISCPCT"];
            activeOfferLines.PrimaryKey = primaryLineKey;
        }

        private void MakeTmpOfferTable()
        {
            tmpMMOffer.Columns.Add("SALELINEID", typeof(int));
            tmpMMOffer.Columns.Add("ITEMSTRIGGERED", typeof(int));
            tmpMMOffer.Columns.Add("DISCTYPE", typeof(int));
            tmpMMOffer.Columns.Add("DEALPRICEORDISCPCT", typeof(decimal));
            tmpMMOffer.Columns.Add("PRICE", typeof(decimal));
            DataColumn[] pk = new DataColumn[1];
            pk[0] = tmpMMOffer.Columns["SALELINEID"];
            tmpMMOffer.PrimaryKey = pk;
        }

        private void MakeTmpDiscountCodeTable()
        {
            tmpDiscountCode.Columns.Add("DISCOUNTCODE", typeof(String));
            tmpDiscountCode.Columns.Add("BARCODE", typeof(String));
            tmpDiscountCode.Columns.Add("DISCOUNTOFFERID", typeof(String));
        }

        // Duplicated in Price.cs.  Refactor whenever change in architecture allows.
        private bool IsDateWithinStartEndDate(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return (((dateToCheck >= startDate) || (startDate == NoDate))
                && ((dateToCheck <= endDate) || (endDate == NoDate)));
        }

        /// <summary>
        /// Loops through the transaction to find offers that the items are in.
        /// </summary>
        /// <param name="retailTransaction"></param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfather")]
        private void FindPeriodicOffers(RetailTransaction retailTransaction)
        {
            activeOffers.Clear();
            activeOfferLines.Clear();

            LSRetailPosis.Transaction.MemoryTables.PeriodicDiscount cache = retailTransaction.PeriodicDiscount;

            //Find all the active periodic offers 
            foreach (SaleLineItem saleItem in retailTransaction.CalculableSalesLines)
            {
                // Update the cache for this item if needed.
                UpdateCache(cache, saleItem);

                //Get the data for the item
                DataTable periodicDiscountData = cache.Get(
                    saleItem.ProductId,
                    saleItem.Dimension.DistinctProductVariantId);

                bool addPeriodicDiscounts = true;

                if (saleItem.NoDiscountAllowed)
                {
                    DataRow[] rows = periodicDiscountData.Select("PDTYPE = 4");
                    if (rows != null && rows.Length > 0)
                    {
                        periodicDiscountData = rows.CopyToDataTable();
                    }
                    else
                    {
                        addPeriodicDiscounts = false;
                    }
                }

                if (addPeriodicDiscounts)
                {
                    AddPeriodicDiscountsToCalculation(retailTransaction, saleItem, periodicDiscountData);
                }
            }
        }

        private PeriodStatus GetPeriodicDiscountPeriodStatus(RetailTransaction retailTransaction, BaseSaleItem saleItem, DataRow periodicDiscount)
        {
            string discValidPeriodId = (string)periodicDiscount["DISCVALIDPERIODID"];
            DateValidationTypes dateValidationType = (DateValidationTypes)periodicDiscount["DATEVALIDATIONTYPE"];

            PeriodStatus periodStatus = PeriodStatus.IsInvalid;
            switch (dateValidationType)
            {
                case DateValidationTypes.Advanced:
                    periodStatus = retailTransaction.Period.IsValid(discValidPeriodId);
                    if (periodStatus == PeriodStatus.NotFoundInMemoryTable)
                    {
                        periodStatus = DiscountPeriodValid(discValidPeriodId, DateTime.Now);
                        retailTransaction.Period.Add(discValidPeriodId, (periodStatus == PeriodStatus.IsValid));
                    }
                    break;
                case DateValidationTypes.Standard:
                    periodStatus = IsDateWithinStartEndDate(DateTime.Now.Date, (DateTime)periodicDiscount["VALIDFROM"], (DateTime)periodicDiscount["VALIDTO"])
                        ? PeriodStatus.IsValid : PeriodStatus.IsInvalid;
                    break;

                default:
                    NetTracer.Warning("Discount::GetPeriodicDiscountPeriodStatus: Invalid Discount Validation Type (retailTransaction {0} saleItem {1}", retailTransaction.TransactionId, saleItem.ItemId);
                    System.Diagnostics.Debug.Fail("Invalid Discount Validation Type");
                    break;
            }
            return periodStatus;
        }

        private bool IsPeriodicDiscountApplicable(RetailTransaction retailTransaction, BaseSaleItem saleItem, DataRow periodicDiscount)
        {
            return GetPeriodicDiscountPeriodStatus(retailTransaction, saleItem, periodicDiscount) == PeriodStatus.IsValid;
            
        }
        
        /// <summary>
        /// Adds given discount data to transaction to be computed for the given sales item
        /// </summary>
        /// <param name="retailTransaction">Transaction with the discount and item</param>
        /// <param name="saleItem">The sale item on the transaction which the discount data applies to</param>
        /// <param name="periodicDiscountData">The discount data, which should be considered for calculation.</param>
        public void AddPeriodicDiscountsToCalculation(RetailTransaction retailTransaction, BaseSaleItem saleItem, DataTable periodicDiscountData)
        {
            if (retailTransaction == null)
            {
                NetTracer.Warning("retailTransaction parameter is null");
                throw new ArgumentNullException("retailTransaction");
            }
            if (saleItem == null)
            {
                NetTracer.Warning("saleItem parameter is null");
                throw new ArgumentNullException("saleItem");
            }
            if (periodicDiscountData == null)
            {
                NetTracer.Warning("periodicDiscountData parameter is null");
                throw new ArgumentNullException("periodicDiscountData");
            }
            //Loop through the offers found for the item
            foreach (DataRow row in periodicDiscountData.Rows)
            {
                if (IsPeriodicDiscountApplicable(retailTransaction, saleItem, row))
                {
                    try
                    {
                        string filterExpr = string.Format("OFFERID='{0}'", row["OFFERID"]);
                        DataRow[] dr = activeOffers.Select(filterExpr);
                        // If has not been added yet to active offers
                        if (dr.Length == 0)
                        {
                            DataRow offerRow;
                            offerRow = activeOffers.NewRow();
                            offerRow["OFFERID"] = row["OFFERID"];
                            offerRow["DESCRIPTION"] = row["DESCRIPTION"];
                            offerRow["PDTYPE"] = row["PDTYPE"];
                            offerRow["CONCURRENCYMODE"] = row["CONCURRENCYMODE"];
                            offerRow["DISCVALIDPERIODID"] = row["DISCVALIDPERIODID"];
                            offerRow["DATEVALIDATIONTYPE"] = row["DATEVALIDATIONTYPE"];
                            offerRow["VALIDFROM"] = row["VALIDFROM"];
                            offerRow["VALIDTO"] = row["VALIDTO"];
                            offerRow["DISCOUNTTYPE"] = row["DISCOUNTTYPE"];
                            offerRow["SAMEDIFFMMLINES"] = row["SAMEDIFFMMLINES"];
                            offerRow["NOOFLINESTOTRIGGER"] = row["NOOFLINESTOTRIGGER"];
                            offerRow["DEALPRICEVALUE"] = row["DEALPRICEVALUE"];
                            offerRow["DISCOUNTPCTVALUE"] = row["DISCOUNTPCTVALUE"];
                            offerRow["DISCOUNTAMOUNTVALUE"] = row["DISCOUNTAMOUNTVALUE"];
                            offerRow["NOOFLEASTEXPENSIVEITEMS"] = row["NOOFLEASTEXPITEMS"];
                            offerRow["NOOFTIMESAPPLICABLE"] = row["NOOFTIMESAPPLICABLE"];
                            offerRow["NOLINESTRIGGERED"] = 0;
                            offerRow["NOOFTIMESACTIVATED"] = 0;
                            offerRow["ISDISCOUNTCODEREQUIRED"] = row["ISDISCOUNTCODEREQUIRED"];
                            activeOffers.Rows.Add(offerRow);
                        }

                        filterExpr = string.Format(
                            "OFFERID='{0}' AND PRODUCTID='{1}' AND DISTINCTPRODUCTVARIANTID = '{2}' AND SALELINEID='{3}' AND DEALPRICEORDISCPCT='{4}'",
                            row["OFFERID"], row["PRODUCTID"], row["DISTINCTPRODUCTVARIANTID"], saleItem.LineId.ToString(), row["DEALPRICEORDISCPCT"]);

                        DataRow[] aol = activeOfferLines.Select(filterExpr);
                        if (aol.Length == 0)
                        {
                            DataRow offerLineRow;
                            offerLineRow = activeOfferLines.NewRow();
                            offerLineRow["OFFERID"] = row["OFFERID"];
                            offerLineRow["LINEID"] = row["LINEID"];
                            offerLineRow["PRODUCTID"] = row["PRODUCTID"];
                            offerLineRow["DISTINCTPRODUCTVARIANTID"] = row["DISTINCTPRODUCTVARIANTID"];
                            offerLineRow["SALELINEID"] = saleItem.LineId;
                            offerLineRow["QUANTITY"] = saleItem.Quantity;
                            offerLineRow["UNIT"] = row["UNIT"];
                            offerLineRow["DEALPRICEORDISCPCT"] = row["DEALPRICEORDISCPCT"];
                            offerLineRow["LINEGROUP"] = row["LINEGROUP"];
                            offerLineRow["DISCTYPE"] = row["DISCTYPE"];
                            offerLineRow["STATUS"] = row["STATUS"];

                            int noOfItemsNeeded = (row["NOOFITEMSNEEDED"] == System.DBNull.Value) ? (0) : (int)row["NOOFITEMSNEEDED"];

                            // MixMatch priority should be based ONLY on the individual unit price, this prevents
                            // groups of cheaper items getting rated 'higher/more-expensive' than a single higher priced item.
                            // For example 3x$10 > 1x$20.  The $10 item should be selected for 'least expensive', even though the "extended" price is higher.
                            decimal priority = saleItem.Price;

                            offerLineRow["NOOFITEMSNEEDED"] = noOfItemsNeeded;
                            offerLineRow["MIXMATCHPRIORITY"] = priority;

                            // Discount Offer line details
                            offerLineRow["DISCOUNTMETHOD"] = row["DISCOUNTMETHOD"];
                            offerLineRow["DISCAMOUNT"] = row["DISCAMOUNT"];
                            offerLineRow["DISCPCT"] = row["DISCPCT"];
                            offerLineRow["OFFERPRICE"] = row["OFFERPRICE"];
                            offerLineRow["OFFERPRICEINCLTAX"] = row["OFFERPRICEINCLTAX"];

                            // Threshold Offer line details
                            offerLineRow["COUNTNONDISCOUNTITEMS"] = Convert.ToBoolean(row["COUNTNONDISCOUNTITEMS"]);

                            activeOfferLines.Rows.Add(offerLineRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        NetTracer.Error(ex, "Discount::AddPeriodicDiscountsToCalculation failed for retailTransaction {0} saleItem {1}", retailTransaction.TransactionId, saleItem.ItemId);
                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Check whether or not an item's offers are already in the cache, and add them from the DB if necessary.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="saleItem"></param>
        private void UpdateCache(LSRetailPosis.Transaction.MemoryTables.PeriodicDiscount cache, SaleLineItem saleItem)
        {
            if (!cache.HasItem(saleItem.ProductId, saleItem.Dimension.DistinctProductVariantId))
            {
                // If not in cache retrieve from DB and add to cache
                ReadOnlyCollection<PeriodicDiscountTemporaryData> periodicDiscountData = GetPeriodicDiscountData(saleItem.ProductId, saleItem.Dimension.DistinctProductVariantId);
                List<string> offerIds = new List<string>();
                foreach (var discount in periodicDiscountData)
                {
                    offerIds.Add(discount.OfferId);
                }
                ReadOnlyCollection<DiscountCodeDataTemporaryData> discountCodeData = GetDiscountCodesForOffer(offerIds);
                cache.Add(periodicDiscountData, saleItem.ProductId, saleItem.Dimension.DistinctProductVariantId, discountCodeData);
            }
        }

        /// <summary>
        /// Loops through the active offers in priority order.  
        /// Starting with offers with the highest order(lowest number) first. 
        /// </summary>
        /// <param name="retailtransaction">The retailtransaction.</param>
        public RetailTransaction RegisterPeriodicDisc(RetailTransaction retailTransaction)
        {
            bool isDiscountValid = true;
            foreach (DataRow row in activeOffers.Select(string.Empty, "CONCURRENCYMODE ASC, OFFERID ASC"))
            {
                isDiscountValid = true;
                string offerId = (string)row["OFFERID"];
                string offerName = (string)row["DESCRIPTION"];
                bool isDiscountCodeRequired = Convert.ToBoolean((int)row["ISDISCOUNTCODEREQUIRED"]);
                string discountCode = string.Empty;
                DiscountMethodType discType = (DiscountMethodType)row["DISCOUNTTYPE"];
                PeriodicDiscOfferType discOfferType = (PeriodicDiscOfferType)row["PDTYPE"];
                ConcurrencyMode concurrencyMode = (ConcurrencyMode)row["CONCURRENCYMODE"];
                //Check if offer id need a discount code in order to get applied
                if (isDiscountCodeRequired)
                {
                    //if discount code is reqired, then get the required code from discountcode table against offerid and match up with the list of discount coupons available in the current transaction
                    //this region will only get executed when ISDISCOUNTCODEREQUIRED flag is set to true else will be skipped                    
                    tmpDiscountCode.Clear();
                    PeriodicDiscount cache = retailTransaction.PeriodicDiscount;
                    tmpDiscountCode = cache.GetDiscountOfferDetails(offerId);

                    //Match the discount codes avialble with the codes scanned at POS(stored in a List)
                    isDiscountValid = false;
                    foreach (DataRow dr in tmpDiscountCode.Select())
                    {
                        string candidateDiscountCode = dr["DISCOUNTCODE"].ToString();
                        if (retailTransaction.DiscountCodes.Contains(candidateDiscountCode))
                        {
                            isDiscountValid = true;
                            discountCode = candidateDiscountCode;
                            break;
                        }
                    }
                }
                if (isDiscountValid)
                {
                    switch (discOfferType)
                    {
                        case PeriodicDiscOfferType.MixAndMatch:
                            retailTransaction = CalcMixMatch(offerId, offerName, discountCode, row, retailTransaction, concurrencyMode);
                            break;

                        case PeriodicDiscOfferType.Multibuy:
                            retailTransaction = CalcMultiBuy(offerId, offerName, discountCode, retailTransaction, discType, concurrencyMode);
                            break;

                        case PeriodicDiscOfferType.Offer:
                            retailTransaction = CalcDiscountOffer(offerId, offerName, discountCode, retailTransaction, concurrencyMode);
                            break;

                        case PeriodicDiscOfferType.Threshold:
                            CalculateThresholdDiscount(retailTransaction, offerId, offerName, discountCode, concurrencyMode, activeOfferLines);
                            break;
                    }
                }
            }
            return retailTransaction;
        }

        /// <summary>
        /// Calculate the periodic discounts for the transation.
        /// </summary>
        /// <param name="retailtransaction"></param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Calc", Justification = "Cannot change public API.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public void CalcPeriodicDisc(RetailTransaction retailTransaction)
        {
            //Clear all the periodic discounts 
            retailTransaction.ClearPeriodicDiscounts();

            //Clear Customer discounts
            retailTransaction.ClearCustomerDiscounts();

            //Find all possible offfers
            FindPeriodicOffers(retailTransaction);

            //Calculate the periodic offers
            retailTransaction = RegisterPeriodicDisc(retailTransaction);

            //Apply concurrency rules to valid offers
            DiscountConcurrencyRules.ApplyConcurrencyRules(retailTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Caller is responsible for disposing returned object</remarks>
        /// <param name="productId"></param>
        /// <param name="variantId"></param>
        /// <returns></returns>
        private ReadOnlyCollection<PeriodicDiscountTemporaryData> GetPeriodicDiscountData(Int64 productId, Int64 variantId)
        {
            List<PeriodicDiscountTemporaryData> periodicDiscounts = new List<PeriodicDiscountTemporaryData>();
            DateTime today = DateTime.Now.Date;

            SqlConnection connection = Discount.InternalApplication.Settings.Database.Connection;
            string dataAreaId = Discount.InternalApplication.Settings.Database.DataAreaID;

            try
            {
                string queryString = @"
                    SELECT
                        pd.OFFERID,
                        pd.NAME,
                        pd.PERIODICDISCOUNTTYPE,
                        pd.CONCURRENCYMODE,
                        pd.ISDISCOUNTCODEREQUIRED,                        
                        pd.VALIDATIONPERIODID,
                        pd.DATEVALIDATIONTYPE,
                        pd.VALIDFROM,
                        pd.VALIDTO,
                        pd.DISCOUNTTYPE,
                        pd.NOOFLINESTOTRIGGER,
                        pd.DEALPRICEVALUE,
                        pd.DISCOUNTPERCENTVALUE,
                        pd.DISCOUNTAMOUNTVALUE,
                        pd.NOOFLEASTEXPENSIVELINES,
                        pd.NUMBEROFTIMESAPPLICABLE,
                        pd.LINENUM,
                        pd.DISCOUNTPERCENTORVALUE,    
                        ISNULL(mmol.LINEGROUP,'') AS LINEGROUP,
                        ISNULL(mmol.DISCOUNTTYPE,'') AS MIXANDMATCHDISCOUNTTYPE, 
                        ISNULL(mmol.NUMBEROFITEMSNEEDED,'') AS NUMBEROFITEMSNEEDED,
    
                        ISNULL(dol.DISCOUNTMETHOD,0) AS DISCOUNTMETHOD,
                        ISNULL(dol.DISCAMOUNT,0) AS DISCAMOUNT, 
                        ISNULL(dol.DISCPCT, 0) AS DISCPCT, 
                        ISNULL(dol.OFFERPRICE, 0) AS OFFERPRICE, 
                        ISNULL(dol.OFFERPRICEINCLTAX, 0) AS OFFERPRICEINCLTAX,
    
                        ISNULL(uom.SYMBOL,'') AS SYMBOL,
                        ISNULL(pd.COUNTNONDISCOUNTITEMS, 0) AS COUNTNONDISCOUNTITEMS
                    FROM
                        RETAILPERIODICDISCOUNTSFLATTENED pd
                    LEFT JOIN UNITOFMEASURE uom
                        ON uom.RECID = pd.UNITOFMEASURE
                    JOIN RETAILGROUPMEMBERLINE rgl
                        ON pd.RETAILGROUPMEMBERLINE = rgl.RECID
                    LEFT JOIN RETAILPRODUCTORVARIANTCATEGORYANCESTORS rpca
                        ON rgl.CATEGORY = rpca.CATEGORY
                    LEFT JOIN RETAILDISCOUNTLINEMIXANDMATCH mmol
                        ON pd.DISCOUNTLINEID = mmol.RECID
                    LEFT JOIN RETAILDISCOUNTLINEOFFER dol 
                        ON pd.DISCOUNTLINEID = dol.RECID
                    WHERE
                      ((rgl.VARIANT != 0 AND rgl.VARIANT = @VARIANTID) OR
                       (rgl.VARIANT = 0 AND rgl.PRODUCT != 0 AND rgl.PRODUCT = @PRODUCTID) OR
                       (rgl.VARIANT = 0 AND rgl.PRODUCT = 0 AND rpca.PRODUCT IN (@PRODUCTID, @VARIANTID)))

                    AND (pd.STATUS = 1)
                    AND (pd.PERIODICDISCOUNTTYPE != 3) -- Don't fetch promotions
                    AND (pd.CURRENCYCODE IN (@STORECURRENCY, ''))
                    AND pd.OFFERID in (SELECT OFFERID FROM RETAILDISCOUNTPRICEGROUP rdpg JOIN @STOREPRICEGROUPS spg ON rdpg.PRICEDISCGROUP = spg.PRICEGROUPID) 

                    AND ((pd.VALIDFROM <= @TODAY OR pd.VALIDFROM <= @NODATE)
                                AND (pd.VALIDTO >= @TODAY OR pd.VALIDTO <= @NODATE))

                    ORDER BY pd.OFFERID, pd.LINENUM";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STORECURRENCY", ApplicationSettings.Terminal.StoreCurrency);
                    command.Parameters.AddWithValue("@VARIANTID", variantId);
                    command.Parameters.AddWithValue("@PRODUCTID", productId);
                    command.Parameters.AddWithValue("@TODAY", today);
                    command.Parameters.AddWithValue("@NODATE", DateTime.Parse("1900-01-01"));

                    // convert store price group list to data-table for use as TVP in the query.
                    using (DataTable priceGroupTable = new DataTable())
                    {
                        priceGroupTable.Columns.Add("PRICEGROUPID", typeof(long));
                        foreach (long priceGroupId in this.storePriceGroups)
                        {
                            priceGroupTable.Rows.Add(priceGroupId);
                        }

                        // Fill out TVP for store price group list
                        SqlParameter param = command.Parameters.Add("@STOREPRICEGROUPS", SqlDbType.Structured);
                        param.Direction = ParameterDirection.Input;
                        param.TypeName = "GETPRICEDISCOUNTDATA_PRICEGROUPS_TABLETYPE";
                        param.Value = priceGroupTable;

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            PeriodicDiscountTemporaryData pdt = new PeriodicDiscountTemporaryData()
                            {
                                OfferId = (string)reader["OFFERID"],
                                Description = (string)reader["NAME"],
                                PeriodicDiscountType = (int)reader["PERIODICDISCOUNTTYPE"],
                                ConcurrencyMode = (int)reader["CONCURRENCYMODE"],
                                IsDiscountCodeRequired = (int)reader["ISDISCOUNTCODEREQUIRED"],
                                ValidationPeriodId = (string)reader["VALIDATIONPERIODID"],
                                DateValidationType = (int)reader["DATEVALIDATIONTYPE"],
                                ValidFrom = (DateTime)reader["VALIDFROM"],
                                ValidTo = (DateTime)reader["VALIDTO"],
                                DiscountType = (int)reader["DISCOUNTTYPE"],
                                NumberOfLinesToTrigger = (int)reader["NOOFLINESTOTRIGGER"],
                                DealPriceValue = (decimal)reader["DEALPRICEVALUE"],
                                DiscountPercentValue = (decimal)reader["DISCOUNTPERCENTVALUE"],
                                DiscountAmountValue = (decimal)reader["DISCOUNTAMOUNTVALUE"],
                                NumberOfLeastExpensiveLines = (int)reader["NOOFLEASTEXPENSIVELINES"],
                                NumberOfTimesApplicable = (int)reader["NUMBEROFTIMESAPPLICABLE"],
                                DiscountLineNumber = (decimal)reader["LINENUM"],
                                DiscountPercentOrValue = (decimal)reader["DISCOUNTPERCENTORVALUE"],

                                LineGroup = (string)(reader["LINEGROUP"] ?? String.Empty),
                                DiscType = (int)(reader["MIXANDMATCHDISCOUNTTYPE"] ?? 0),
                                NumberOfItemsNeeded = (int)(reader["NUMBEROFITEMSNEEDED"] ?? 0),

                                DiscountMethod = (int)(reader["DISCOUNTMETHOD"] ?? 0),
                                DiscountAmount = (decimal)(reader["DISCAMOUNT"] ?? 0),
                                DiscountPercent = (decimal)(reader["DISCPCT"] ?? 0),
                                OfferPrice = (decimal)(reader["OFFERPRICE"] ?? 0),
                                OfferPriceIncludingTax = (decimal)(reader["OFFERPRICEINCLTAX"] ?? 0),

                                UnitOfMeasureSymbol = (string)(reader["SYMBOL"] ?? string.Empty),
                                CountNonDiscountItems = (int)(reader["COUNTNONDISCOUNTITEMS"] ?? 0),
                            };
                            periodicDiscounts.Add(pdt);
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

            if (ApplicationSettings.LogTraceLevel == LogTraceLevel.Trace)
            {
                var distinctOfferIds = periodicDiscounts.Select(p => p.OfferId).Distinct();
                LSRetailPosis.ApplicationLog.Log("Discount.GetPeriodicDiscountData()",
                    String.Format("Found {0} periodic discounts for product '{1}' (variant '{2}'):{3}",
                    distinctOfferIds.Count(), productId, variantId,
                    distinctOfferIds.Aggregate(new StringBuilder(), (ids, id) => ids.AppendFormat(" '{0}'", id)).ToString()),
                    LogTraceLevel.Trace);
            }

            return periodicDiscounts.AsReadOnly();
        }

        private ReadOnlyCollection<DiscountCodeDataTemporaryData> GetDiscountCodesForOffer(List<string> offerIds)
        {
            ReadOnlyCollection<DiscountCodeDataTemporaryData> discountCodeData = new List<DiscountCodeDataTemporaryData>().AsReadOnly();
            try
            {
                SqlConnection connection = Application.Settings.Database.Connection;
                string dataAreaId = Application.Settings.Database.DataAreaID;
                using (DM.PosDbContext posDb = new DM.PosDbContext(connection))
                {
                    var discountCodes = (from discountCode in posDb.RetailDiscountCodes
                                         where offerIds.Contains(discountCode.DiscountOfferId) && discountCode.DataAreaId == dataAreaId
                                         select discountCode).ToList();
                    discountCodeData = discountCodes.Select(r => new DiscountCodeDataTemporaryData
                    {
                        DiscountOfferId = r.DiscountOfferId,
                        Barcode = r.Barcode,
                        DiscountCode = r.DiscountCode
                    }).ToList().AsReadOnly();
                }

            }
            catch (Exception ex)
            {
                NetTracer.Error(ex, "Discount::GetDiscountCodesForOffer failed.");
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            return discountCodeData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Caller is responsible for disposing returned object</remarks>
        /// <param name="offerId"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private DataTable GetMMLineGroups(string offerId)
        {
            DataTable mmLineGroups = new DataTable();

            var lineGroups = this.discountDataManager.GetMMLineGroups(offerId);

            mmLineGroups.Columns.Add("LINEGROUP", typeof(string));
            mmLineGroups.Columns.Add("NOOFITEMSNEEDED", typeof(Int32));
            mmLineGroups.Columns.Add("ITEMSTRIGGERED", typeof(Int32));

            foreach (var lg in lineGroups)
            {
                DataRow row = mmLineGroups.NewRow();
                row["LINEGROUP"] = lg.MixAndMatchLineGroup;
                row["NOOFITEMSNEEDED"] = lg.NumberOfItemsNeeded;
                row["ITEMSTRIGGERED"] = lg.NumberOfItemsNeeded;
                mmLineGroups.Rows.Add(row);
            }

            return mmLineGroups;
        }

        /// <summary>
        /// Calculate Discount Offers
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="offerName"></param>
        /// <param name="retailTransaction"></param>
        /// <param name="discType"></param>
        /// <returns></returns>
        private RetailTransaction CalcDiscountOffer(string offerId, string offerName, string discountCode, RetailTransaction retailTransaction, ConcurrencyMode concurrencyMode)
        {
            //Loop through all the lines in a specific offer
            foreach (DataRow row in activeOfferLines.Select("OFFERID='" + offerId + "'", "OFFERID ASC, DEALPRICEORDISCPCT DESC"))
            {
                SaleLineItem saleItem = retailTransaction.CalculableSalesLines.Where(cl => cl.LineId == (int)row["SALELINEID"]).First();

                bool continueWithDiscount = false;
                string offerUOM = Utility.ToString(row["UNIT"]);

                if (!string.IsNullOrEmpty(offerUOM) && !string.Equals(offerUOM, saleItem.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                {
                    // If the UOM is specified (non-empty) and the line item doesn't match, then skip this discount
                    continue;
                }

                // If items haven't already been nabbed by other discounts
                decimal leftToDiscount = (Math.Abs(saleItem.Quantity) - Math.Abs(saleItem.QuantityDiscounted));
                if (leftToDiscount > 0M)
                {
                    if (Math.Abs(saleItem.QuantityDiscounted) > 0M)
                    {
                        // Split off a new row for the partial quantity that is not covered by the previous discount.
                        DataRow newRow = SplitLine(ref retailTransaction, saleItem.LineId, leftToDiscount);

                        // Continue with the discount using the newly split row.
                        saleItem = retailTransaction.CalculableSalesLines.Where(cl => cl.LineId == retailTransaction.SaleItems.Count).First();
                    }
                    continueWithDiscount = true;
                }

                // Apply the Discount Offer
                if (continueWithDiscount && (saleItem.Quantity != 0m))
                {
                    RegisterDiscountOffer(offerId, offerName, discountCode, row, saleItem, concurrencyMode);
                }
            }

            return retailTransaction;
        }

        private void RegisterDiscountOffer(string offerId, string offerName, string discountCode, DataRow row, SaleLineItem saleItem, ConcurrencyMode concurrencyMode)
        {
            PeriodicDiscountItem discountItem = (PeriodicDiscountItem)this.Utility.CreatePeriodicDiscountItem();
            discountItem.LineDiscountType = DiscountTypes.Periodic;
            discountItem.PeriodicDiscountType = PeriodicDiscOfferType.Offer;
            discountItem.OfferId = offerId;
            discountItem.OfferName = offerName;
            discountItem.DiscountCode = discountCode;
            discountItem.ConcurrencyMode = concurrencyMode;
            discountItem.IsCompoundable = false;
            // discount offers should be grouped for consideration by item, allowing application of a single discount line in an offer
            discountItem.PeriodicDiscGroupId = saleItem.ItemId;

            decimal offerPrice;
            DiscountMethod discountMethod = (DiscountMethod)Utility.ToInt(row["DISCOUNTMETHOD"]);
            switch (discountMethod)
            {
                case DiscountMethod.DiscountAmount: // amount
                    discountItem.Percentage = 0m;
                    discountItem.Amount = Utility.ToDecimal(row["DISCAMOUNT"]);
                    discountItem.IsCompoundable = true;
                    break;

                case DiscountMethod.OfferPrice: // price
                    offerPrice = Utility.ToDecimal(row["OFFERPRICE"]);
                    offerPrice = AmountInStoreCurrency(offerPrice);

                    discountItem.Percentage = 0m;
                    discountItem.Amount = saleItem.Price - offerPrice;
                    break;

                case DiscountMethod.OfferPriceInclTax: // price w/ tax
                    offerPrice = Utility.ToDecimal(row["OFFERPRICEINCLTAX"]);
                    offerPrice = AmountInStoreCurrency(offerPrice);

                    discountItem.Percentage = 0m;
                    discountItem.Amount = saleItem.Price - offerPrice;
                    break;

                case DiscountMethod.DiscountPercent: // percent
                default:
                    // percentage should not be more than 100%
                    discountItem.Percentage = Math.Min(100, Utility.ToDecimal(row["DISCPCT"]));
                    discountItem.Amount = 0m;
                    discountItem.IsCompoundable = true;
                    break;
            }

            saleItem.WasChanged = true;

            UpdatePeriodicDiscountLines(saleItem, discountItem);
        }

        /// <summary>
        /// Sums up the quantity for the item in different lines in a certain multibuy offer. 
        /// </summary>
        /// <param name="offerId">The id of the offer.</param>
        /// <param name="productId">The id of the product to find quantity for.</param>
        /// <param name="distinctProductVariantId">The variant id to find quantity for.</param>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <returns>The total quantity of the item in the transaction.</returns>
        private decimal MultibuyLineQty(string offerId, Int64 productId, Int64 distinctProductVariantId, RetailTransaction retailTransaction)
        {
            Decimal result = decimal.Zero;

            //Loop through all the lines in a specific offer to find the totals for that item.
            foreach (DataRow row in activeOfferLines.Select("OFFERID='" + offerId + "'", "OFFERID ASC, LINEID ASC"))
            {
                SaleLineItem saleItem = retailTransaction.CalculableSalesLines.Single(cl => cl.LineId == (int)row["SALELINEID"]);

                // If the Offer's UOM is specified (non-empty) then only count lines with a matching UOM.
                string offerUOM = Utility.ToString(row["UNIT"]);

                if (!saleItem.NoDiscountAllowed
                    && !saleItem.Voided
                    && saleItem.ProductId == productId
                    && ((distinctProductVariantId == 0) || (saleItem.Dimension.DistinctProductVariantId == distinctProductVariantId))
                    && ((offerUOM.Length == 0) || String.Equals(offerUOM, saleItem.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase)))
                {
                    //If the item doesn't have a periodic discount or has been added to the same offer we are looking at
                    //add the total quantity of the item to the result
                    if ((saleItem.PeriodicDiscountPossibilities.Count == 0) || (saleItem.PeriodicDiscountPossibilities.Select(p => p.OfferId).Contains(offerId)))
                    {
                        result += saleItem.Quantity;
                    }
                    //If the item is in another offer then get the possible number of items that have not been discounted yet
                    else if ((saleItem.PeriodicDiscountPossibilities.Count != 0) && (!saleItem.PeriodicDiscountPossibilities.Select(p => p.OfferId).Contains(offerId)))
                    {
                        result += (saleItem.Quantity - saleItem.QuantityDiscounted);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate the periodic multibuy discount.
        /// </summary>
        /// <param name="offerId">The id of the offer</param>
        /// <param name="discountCode">The discount code.</param>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <param name="discType"></param>
        /// <returns>The retail transaction</returns>
        private RetailTransaction CalcMultiBuy(string offerId, string offerName, string discountCode, RetailTransaction retailTransaction, DiscountMethodType discType, ConcurrencyMode concurrencyMode)
        {
            //Loop through all the lines in a specific offer to calculate the discount
            foreach (DataRow row in activeOfferLines.Select("OFFERID='" + offerId + "'", "OFFERID ASC, LINEID ASC"))
            {
                SaleLineItem saleItem = retailTransaction.CalculableSalesLines.Where(cl => cl.LineId == (int)row["SALELINEID"]).First();
                Int64 distinctProductVariantId = (Int64)row["DISTINCTPRODUCTVARIANTID"];
                if (distinctProductVariantId == 0 || distinctProductVariantId == saleItem.Dimension.DistinctProductVariantId)
                {
                    // continue to next offer line if the unit of measure doesn't match
                    string offerUOM = Utility.ToString(row["UNIT"]);
                    if (!string.IsNullOrEmpty(offerUOM) && !string.Equals(offerUOM, saleItem.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // get total number of times item is in the transaction
                    decimal totQtyForMultiBuyLine = MultibuyLineQty(offerId, saleItem.ProductId, saleItem.Dimension.DistinctProductVariantId, retailTransaction);

                    // find multibuy line whose required items is closest to totQtyForMultiBuyLine without going over
                    MultibuyLine multiBuyLine = MultibuyLine.FetchForOfferAndQuantity(offerId, Math.Abs(totQtyForMultiBuyLine));

                    // continue to next offer if no valid multibuy line was found
                    if (multiBuyLine.MinQuantity <= 0m)
                    {
                        continue;
                    }

                    decimal leftToDiscount = (Math.Abs(saleItem.Quantity) - Math.Abs(saleItem.QuantityDiscounted));
                    if (leftToDiscount > 0m)
                    {
                        if (Math.Abs(saleItem.QuantityDiscounted) > 0M)
                        {
                            // If some of the quantity is already 'claimed' split off a new sales line with the unclaimed items and use it
                            DataRow newRow = SplitLine(ref retailTransaction, saleItem.LineId, leftToDiscount);
                            saleItem = retailTransaction.CalculableSalesLines.Single(cl => cl.LineId == retailTransaction.SaleItems.Count);
                        }
                        multiBuyLine = RegisterMultibuyDiscount(offerId, offerName, discountCode, discType, saleItem, multiBuyLine, concurrencyMode);
                    }
                }
            }

            return retailTransaction;
        }

        private MultibuyLine RegisterMultibuyDiscount(string offerId, string offerName, string discountCode, DiscountMethodType discType, SaleLineItem saleItem, MultibuyLine multiBuyLine, ConcurrencyMode concurrencyMode)
        {
            if (saleItem.Price != 0m && saleItem.Quantity != 0m)
            {
                saleItem.WasChanged = true;

                PeriodicDiscountItem discountItem = (PeriodicDiscountItem)this.Utility.CreatePeriodicDiscountItem();
                discountItem.LineDiscountType = DiscountTypes.Periodic;
                discountItem.PeriodicDiscountType = PeriodicDiscOfferType.Multibuy;
                discountItem.OfferId = offerId;
                discountItem.OfferName = offerName;
                discountItem.DiscountCode = discountCode;
                discountItem.ConcurrencyMode = concurrencyMode;
                discountItem.IsCompoundable = false;
                // quantity discounts should be grouped for consideration by item for whole transaction.
                //  if we only apply it to some of the items, can't be sure we stay above the quantity activation threshold
                discountItem.PeriodicDiscGroupId = saleItem.ItemId;

                if ((discType == DiscountMethodType.DealPrice) || (discType == DiscountMethodType.MultiplyDealPrice))
                {
                    discountItem.Percentage = 0m;
                    discountItem.Amount = saleItem.Price - AmountInStoreCurrency(multiBuyLine.UnitPriceOrDiscPct);
                }
                else // discount percent
                {
                    discountItem.Percentage = multiBuyLine.UnitPriceOrDiscPct;
                    discountItem.Amount = 0m;
                    discountItem.IsCompoundable = true;
                }

                UpdatePeriodicDiscountLines(saleItem, discountItem);
            }
            else
            {
                NetTracer.Information("Discount::RegisterMultibuyDiscount: saleItem.Price and/or saleItem.Quantity is zero for offerId {0} offerName {1} discountCode {2}", offerId, offerName, discountCode);
            }
            return multiBuyLine;
        }

        private static DataTable CompressActiveOfferLines(DataTable offerLines)
        {
            bool negQtyExists = false;
            bool posQtyExists = false;

            foreach (DataRow row in offerLines.Select())
            {
                if ((decimal)row["QUANTITY"] > 0m)
                {
                    posQtyExists = true;
                }

                if ((decimal)row["QUANTITY"] < 0m)
                {
                    negQtyExists = true;
                }
            }

            // if transaction has mix and match lines which are negative and positive
            if (posQtyExists && negQtyExists)
            {
                // get lines in mix and match offer, and sort in order of transaction
                foreach (DataRow row in offerLines.Select(string.Empty, "SALELINEID ASC"))
                {
                    // for each offer line, get all offer lines with the same product/variant, sorted reverse order of transaction
                    foreach (DataRow row2 in offerLines.Select(String.Format("PRODUCTID={0} AND DISTINCTPRODUCTVARIANTID={1}",
                        (Int64)row["PRODUCTID"], (Int64)row["DISTINCTPRODUCTVARIANTID"]), "SALELINEID DESC"))
                    {
                        // only do anything if inner line is below outer line in transaction order
                        if ((int)row2["SALELINEID"] > (int)row["SALELINEID"])
                        {
                            // if outer line is positive and inner line is negative
                            if ((decimal)row["QUANTITY"] > 0m && (decimal)row2["QUANTITY"] < 0m)
                            {
                                // if the sum of inner and outer line is positive or zero, move all quantity to the outer line
                                if ((decimal)row["QUANTITY"] + (decimal)row2["QUANTITY"] >= 0m)
                                {
                                    row["QUANTITY"] = (decimal)row["QUANTITY"] + (decimal)row2["QUANTITY"];
                                    row2["QUANTITY"] = 0m;
                                }
                                // if sum of inner and outer line is negative, move all quantity to inner line
                                else
                                {
                                    row2["QUANTITY"] = (decimal)row["QUANTITY"] + (decimal)row2["QUANTITY"];
                                    row["QUANTITY"] = 0m;
                                }
                            }
                            // if outer line is negative and inner line is positive
                            else if ((decimal)row["QUANTITY"] < 0m && (decimal)row2["QUANTITY"] > 0m)
                            {
                                // if sum of inner and outer lines are negative or zero, move all quantity to outer line
                                if ((decimal)row["QUANTITY"] + (decimal)row2["QUANTITY"] <= 0m)
                                {
                                    row["QUANTITY"] = (decimal)row["QUANTITY"] + (decimal)row2["QUANTITY"];
                                    row2["QUANTITY"] = 0m;
                                }
                                // if sume of inner and outer lines are positive, move all quantity to inner line
                                else
                                {
                                    row2["QUANTITY"] = (decimal)row["QUANTITY"] + (decimal)row2["QUANTITY"];
                                    row["QUANTITY"] = 0m;
                                }
                            }
                        }
                        // go to next outer line if there are no more matching inner lines below it
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return offerLines;
        }

        private DataTable GetMixMatchOfferLines(string offerId)
        {
            //Get a copy of active offerLines for this offer ordered by priority
            using (DataView tmpOffer = new DataView(activeOfferLines))
            {
                tmpOffer.RowFilter = "OFFERID='" + offerId + "'";
                tmpOffer.Sort = ("MIXMATCHPRIORITY ASC");

                //Create a new datatable with the Mix & Match information sorted by the Mix & Match priority
                DataTable offerLines = tmpOffer.ToTable();
                //Set the primary key as M&M priority + Sale line id + PRODUCT ID + VARIANT ID
                DataColumn[] pk = new DataColumn[5];
                pk[0] = offerLines.Columns["MIXMATCHPRIORITY"];
                pk[1] = offerLines.Columns["SALELINEID"];
                pk[2] = offerLines.Columns["PRODUCTID"];
                pk[3] = offerLines.Columns["DISTINCTPRODUCTVARIANTID"];
                pk[4] = offerLines.Columns["UNIT"];
                offerLines.PrimaryKey = pk;

                //Must compress check because of minus quantities
                offerLines = CompressActiveOfferLines(offerLines);
                return offerLines;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Grandfathered")]
        private RetailTransaction CalcMixMatch(string offerId, string offerName, string discountCode, DataRow activeOffer, RetailTransaction retailTransaction, ConcurrencyMode concurrencyMode)
        {
            DataTable offerLines = GetMixMatchOfferLines(offerId);

            DataTable mmLineGroups = GetMMLineGroups(offerId);
            if (mmLineGroups.Rows.Count == 0)
            {
                NetTracer.Information("Discount::CalcMixMatch: mmLineGroups.Rows.Count is zero for offerId {0} offerName {1} discountCode {2}", offerId, offerName, discountCode);
                return retailTransaction;
            }

            ResetMixAndMatchCalculation(mmLineGroups);
            UInt32 numberOfTimesOfferHasBeenApplied = 0;
            int qtyDiscounted = 0;
            try
            {
                do
                {
                    DiscountMethodType DiscountType = (DiscountMethodType)activeOffer["DISCOUNTTYPE"];

                    //split product for least expensive
                    if (DiscountType == DiscountMethodType.LeastExpensive)
                    {
                        int noOfLeastExpensiveItem = (int)activeOffer["NOOFLEASTEXPENSIVEITEMS"];
                        decimal leastExpensivePrice = GetMaxLeastExpensiveAmount(noOfLeastExpensiveItem, retailTransaction);
                        int qtyCount = 0;
                        int splitCount = 0;
                        foreach (DataRow offerline in this.tmpMMOffer.Select("PRICE <= " + leastExpensivePrice.ToString(), "PRICE ASC, SALELINEID DESC"))
                        {
                            ISaleLineItem saleItem = retailTransaction.CalculableSalesLines.Single(cl => cl.LineId == (int)offerline["SALELINEID"]);
                            qtyCount += Convert.ToInt32(saleItem.Quantity);
                            if (qtyCount > noOfLeastExpensiveItem)
                            {
                                DataRow discountedRow = SplitLineForLeastExpensive(ref retailTransaction, saleItem.LineId, noOfLeastExpensiveItem - splitCount);
                                break;
                            }
                            splitCount += Convert.ToInt32(saleItem.Quantity);
                        }
                    }

                    //If the criteria for the offer has been fulfilled then update the items.
                    if (AllGroupsTriggered(mmLineGroups))
                    {
                        foreach (DataRow tmpMMRow in tmpMMOffer.Select())
                        {
                            ISaleLineItem saleItem = retailTransaction.CalculableSalesLines.Single(cl => cl.LineId == (int)tmpMMRow["SALELINEID"]);

                            //When setting QuantityDiscounted, take the quantity sign into account so that the discount is correctly calculated for Return lines.
                            saleItem.QuantityDiscounted += (int)tmpMMRow["ITEMSTRIGGERED"] * Math.Sign(saleItem.Quantity);
                        }

                        //Calculate discount and update all saleitems
                        qtyDiscounted += RegisterMixMatch(offerId, offerName, discountCode, activeOffer, tmpMMOffer, retailTransaction, concurrencyMode, numberOfTimesOfferHasBeenApplied, qtyDiscounted);
                        numberOfTimesOfferHasBeenApplied += 1;

                        ResetMixAndMatchCalculation(mmLineGroups);
                    }

                    //Get all offerlines for the offer in question
                    offerLines = GetMixMatchOfferLines(offerId);

                    int totQuantityDiscounted = 0;

                    foreach (DataRow row in offerLines.Select(string.Empty, "MIXMATCHPRIORITY DESC"))
                    {
                        //Debug
                        decimal priority = (decimal)row["MIXMATCHPRIORITY"];

                        if (AllGroupsTriggered(mmLineGroups)) { break; }

                        ISaleLineItem saleItem = retailTransaction.CalculableSalesLines.Single(cl => cl.LineId == (int)row["SALELINEID"]);
                        DataRow discountedRow = row;
                        string offerUOM = Utility.ToString(row["UNIT"]);

                        if (!string.IsNullOrEmpty(offerUOM) && !string.Equals(offerUOM, saleItem.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if ((Math.Abs(saleItem.Quantity) - Math.Abs(saleItem.QuantityDiscounted)) > decimal.Zero)
                        {
                            if (Math.Abs(saleItem.QuantityDiscounted) > decimal.Zero)
                            {
                                // split the line, and continue with discount using the new line
                                discountedRow = SplitLine(ref retailTransaction, saleItem.LineId, (saleItem.Quantity - saleItem.QuantityDiscounted));
                                saleItem = retailTransaction.CalculableSalesLines.Single(cl => cl.LineId == retailTransaction.SaleItems.Count);
                            }
                        }
                        else
                        {
                            continue;
                        }

                        // calculate the discount using the selected row & line item
                        if (saleItem.Quantity != decimal.Zero)
                        {
                            decimal discountedRowQuantity = (decimal)discountedRow["QUANTITY"];
                            decimal leftToDiscount = Math.Abs(discountedRowQuantity) - saleItem.QuantityDiscounted;
                            totQuantityDiscounted += Convert.ToInt32(leftToDiscount);

                            // Mix and Match discount only currently works for integer quantities.  Therefore
                            // leftToDiscount must be at least 1
                            if ((leftToDiscount >= 1.0M) && (saleItem.NoDiscountAllowed == false))
                            {
                                FindTriggeredMixAndMatchItems(mmLineGroups, totQuantityDiscounted, saleItem, discountedRow, leftToDiscount);
                            }
                        }
                    }
                } while (AllGroupsTriggered(mmLineGroups));
            }
            catch (Exception ex)
            {
                NetTracer.Error(ex, "Discount::CalcMixMatch: failed while matching mix and match lines for offerId {0} offerName {1} discountCode {2}", offerId, offerName, discountCode);
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }

            return retailTransaction;
        }

        private void FindTriggeredMixAndMatchItems(DataTable mmLineGroups, int totQuantityDiscounted, ISaleLineItem saleItem, DataRow discountedRow, decimal leftToDiscount)
        {
            //Go through each instance of the sale item and check if it can be in the promotion.
            string lineGroup = (string)discountedRow["LINEGROUP"];
            for (int i = 0; i < leftToDiscount; i++)
            {
                foreach (DataRow mmRow in mmLineGroups.Select("LINEGROUP='" + lineGroup + "'"))
                {
                    int noOfItemsNeeded = (int)mmRow["NOOFITEMSNEEDED"];
                    int itemsTriggered = (int)mmRow["ITEMSTRIGGERED"];

                    if ((totQuantityDiscounted >= itemsTriggered) && (noOfItemsNeeded > itemsTriggered))
                    {
                        bool found = false;

                        foreach (DataRow mmOldRow in tmpMMOffer.Select("SALELINEID='" + saleItem.LineId.ToString() + "'"))
                        {
                            mmOldRow["ITEMSTRIGGERED"] = (int)mmOldRow["ITEMSTRIGGERED"] + 1;
                            found = true;
                        }

                        if (!found)
                        {
                            DataRow tmpMMRow;
                            tmpMMRow = tmpMMOffer.NewRow();
                            tmpMMRow["SALELINEID"] = saleItem.LineId;
                            tmpMMRow["ITEMSTRIGGERED"] = 1;
                            tmpMMRow["DISCTYPE"] = discountedRow["DISCTYPE"];
                            tmpMMRow["DEALPRICEORDISCPCT"] = discountedRow["DEALPRICEORDISCPCT"];
                            tmpMMRow["PRICE"] = saleItem.Price;
                            tmpMMOffer.Rows.Add(tmpMMRow);
                        }

                        mmRow["ITEMSTRIGGERED"] = itemsTriggered + 1;
                    }
                }
            }
        }

        private void ResetMixAndMatchCalculation(DataTable mmLineGroups)
        {
            //Initialize
            tmpMMOffer.Clear();

            //Set items triggerd to zero for all line groups
            foreach (DataRow row in mmLineGroups.Select())
            {
                row["ITEMSTRIGGERED"] = 0;
            }
        }

        /// <summary>
        /// Adjusts multi line discounts to make the amount come out to an exact target discount to account
        /// for rounding
        /// </summary>
        /// <param name="tmpMMOffer"></param>
        /// <param name="retailTransaction"></param>
        /// <param name="targetDiscountAmt"></param>
        /// <param name="application"></param>
        private static void AdjustMultiLineDiscount(DataTable tmpMMOffer, RetailTransaction retailTransaction, decimal targetDiscountAmt, IApplication application)
        {
            SaleLineItem highestDiscountLine = null;
            decimal actualDiscount = decimal.Zero;
            decimal highestDiscountAmount = decimal.Zero;

            // Sum up all of the rounded periodic mix and match discounts to find the actual discount
            // Also find the row with the highest discount for the one to adjust.
            foreach (DataRow row in tmpMMOffer.Rows)
            {
                SaleLineItem saleItem = retailTransaction.GetItem((int)row["SALELINEID"]);
                DiscountItem discountItem = saleItem.GetPossiblePeriodicDiscountItem(PeriodicDiscOfferType.MixAndMatch, DiscountTypes.Periodic);
                decimal extendedDiscountAmount = application.Services.Rounding.Round(discountItem.Amount * Math.Abs(saleItem.Quantity));
                if ((Math.Abs(extendedDiscountAmount) > highestDiscountAmount) || (highestDiscountLine == null))
                {
                    highestDiscountAmount = Math.Abs(extendedDiscountAmount);
                    highestDiscountLine = saleItem;
                }

                actualDiscount += extendedDiscountAmount;
            }

            // Adjust the line to make the discount come out exact.
            if ((targetDiscountAmt != actualDiscount) && (highestDiscountLine != null))
            {
                DiscountItem discountItem = highestDiscountLine.GetPossiblePeriodicDiscountItem(PeriodicDiscOfferType.MixAndMatch, DiscountTypes.Periodic);
                discountItem.Amount += (targetDiscountAmt - actualDiscount) / Math.Abs(highestDiscountLine.Quantity);
            }
        }

        private int RegisterMixMatch(string offerId, string offerName, string discountCode, DataRow activeOffer, DataTable tmpMixAndMatchOffer, RetailTransaction retailTransaction, ConcurrencyMode concurrencyMode, UInt32 groupNumber, int qtyDiscounted)
        {
            DiscountMethodType DiscountType = (DiscountMethodType)activeOffer["DISCOUNTTYPE"];
            decimal dealPrice = (decimal)activeOffer["DEALPRICEVALUE"];
            decimal discountAmount = (decimal)activeOffer["DISCOUNTAMOUNTVALUE"];
            decimal totalAmount = decimal.Zero;
            bool isCompoundable = false;

            dealPrice = AmountInStoreCurrency(dealPrice);
            discountAmount = AmountInStoreCurrency(discountAmount);
            int noOfLeastExpensiveItem = (int)activeOffer["NOOFLEASTEXPENSIVEITEMS"];
            try
            {
                foreach (DataRow row in tmpMixAndMatchOffer.Select())
                {
                    ISaleLineItem saleItem = retailTransaction.GetItem((int)row["SALELINEID"]);

                    totalAmount += saleItem.Price * (int)row["ITEMSTRIGGERED"];
                }

                foreach (DataRow row in tmpMixAndMatchOffer.Select())
                {
                    SaleLineItem saleItem = retailTransaction.GetItem((int)row["SALELINEID"]);
                    saleItem.WasChanged = true;

                    decimal percentage = 0m;
                    decimal amount = 0m;

                    // choose correct price field based on tax scheme
                    decimal saleprice = saleItem.Price;

                    if (saleItem.Quantity != 0m && totalAmount != 0m)
                    {
                        switch (DiscountType)
                        {
                            case DiscountMethodType.DealPrice:
                                {
                                    percentage = 0m;
                                    amount = this.Application.Services.Rounding.Round((totalAmount - dealPrice) * Math.Abs((saleprice * saleItem.QuantityDiscounted / totalAmount)));
                                    amount = amount / Math.Abs(saleItem.Quantity); //Discount amount per pcs.
                                }
                                break;
                            case DiscountMethodType.DiscountPercent:
                                {
                                    percentage = (decimal)activeOffer["DISCOUNTPCTVALUE"] * Math.Abs((saleItem.QuantityDiscounted / (saleItem.Quantity)));
                                    amount = 0m;
                                    isCompoundable = true;
                                }
                                break;
                            case DiscountMethodType.DiscountAmount:
                                {
                                    percentage = 0m;
                                    amount = discountAmount * Math.Abs((saleprice * saleItem.QuantityDiscounted / totalAmount));
                                    amount = amount / Math.Abs(saleItem.Quantity); //Discount amount per pcs.
                                    isCompoundable = true;
                                }
                                break;
                            case DiscountMethodType.LeastExpensive:
                                {
                                    percentage = 0m;
                                    /*Decimal LeastExpensiveDiscount = GetLeastExpensiveAmount((int)activeOffer["NOOFLEASTEXPENSIVEITEMS"], retailTransaction);
                                    amount = this.Application.Services.Rounding.Round(LeastExpensiveDiscount * Math.Abs((saleprice * saleItem.QuantityDiscounted / totalAmount)));
                                    amount = amount / Math.Abs(saleItem.Quantity); //Discount amount per pcs.
                                     * */
                                    Decimal LeastExpensiveDiscount = GetMaxLeastExpensiveAmount(noOfLeastExpensiveItem, retailTransaction);
                                    if (LeastExpensiveDiscount >= saleprice)
                                    {
                                        //find best match based on price and quantity
                                        bool perfectMatch = false;
                                        int bestMatchLineId = 0;
                                        foreach (DataRow offerline in this.tmpMMOffer.Select("PRICE <= " + saleprice.ToString(), "SALELINEID DESC"))
                                        {
                                            ISaleLineItem _saleItem = retailTransaction.GetItem((int)offerline["SALELINEID"]);
                                            if (Convert.ToInt32(_saleItem.Quantity) == noOfLeastExpensiveItem)
                                            {
                                                perfectMatch = true;
                                                bestMatchLineId = _saleItem.LineId;
                                                break;
                                            }
                                        }

                                        if (perfectMatch)
                                        {
                                            if (saleItem.LineId == bestMatchLineId)
                                            {
                                                percentage = 100m;
                                                amount = saleprice;
                                                qtyDiscounted = Convert.ToInt32(saleItem.Quantity);
                                            }
                                        }
                                        else
                                        {
                                            if (qtyDiscounted < noOfLeastExpensiveItem)
                                            {
                                                percentage = 100m;
                                                amount = saleprice;
                                                qtyDiscounted += Convert.ToInt32(saleItem.Quantity);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        amount = 0m;
                                        percentage = 0m;
                                    }
                                }
                                break;
                            case DiscountMethodType.LineSpecific:
                                {
                                    if ((int)row["DISCTYPE"] == (int)DiscountMethodType.DealPrice)
                                    {
                                        percentage = 0m;
                                        decimal lineSpecDealPrice = (decimal)row["DEALPRICEORDISCPCT"];
                                        lineSpecDealPrice = AmountInStoreCurrency(lineSpecDealPrice);

                                        amount = ((saleprice * Math.Abs(saleItem.QuantityDiscounted)) - lineSpecDealPrice);
                                        amount = amount / Math.Abs(saleItem.Quantity); //Discount amount per pcs.
                                    }
                                    else // discount percent
                                    {
                                        percentage = (decimal)row["DEALPRICEORDISCPCT"] * Math.Abs((saleItem.QuantityDiscounted / (saleItem.Quantity)));
                                        amount = 0;
                                        isCompoundable = true;
                                    }
                                }
                                break;
                        }

                        PeriodicDiscountItem discountItem = (PeriodicDiscountItem)this.Utility.CreatePeriodicDiscountItem();
                        discountItem.LineDiscountType = DiscountTypes.Periodic;
                        discountItem.PeriodicDiscountType = PeriodicDiscOfferType.MixAndMatch;
                        discountItem.Percentage = percentage;
                        discountItem.Amount = amount;
                        discountItem.OfferId = offerId;
                        discountItem.OfferName = offerName;
                        discountItem.DiscountCode = discountCode;
                        discountItem.ConcurrencyMode = concurrencyMode;
                        discountItem.IsCompoundable = isCompoundable;
                        // group Id represents this unique mix and match set which is being registered
                        discountItem.PeriodicDiscGroupId = discountItem.OfferId + "_" + groupNumber.ToString();

                        UpdatePeriodicDiscountLines(saleItem, discountItem);
                    }
                }

                // Adjust the multi-line discounts to make the amounts come out exact.
                switch (DiscountType)
                {
                    case DiscountMethodType.DealPrice:
                        AdjustMultiLineDiscount(
                            tmpMixAndMatchOffer,
                            retailTransaction,
                            (totalAmount - dealPrice),
                            this.Application);
                        break;

                    /*case DiscountMethodType.LeastExpensive:
                        AdjustMultiLineDiscount(
                            tmpMixAndMatchOffer,
                            retailTransaction,
                            GetLeastExpensiveAmount((int)activeOffer["NOOFLEASTEXPENSIVEITEMS"], retailTransaction),
                            this.Application);
                        break;*/
                }
                return qtyDiscounted;

            }
            catch (Exception ex)
            {
                NetTracer.Error(ex, "Discount::RegisterMixMatch failed for offerId {0} offerName {1} discountCode {2}", offerId, offerName, discountCode);
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }


        /// <summary>
        /// If the company currency differs from the store, convert the given
        ///   amount from the company currency to the store currency
        /// </summary>
        /// <param name="amount">Amount in company currency</param>
        /// <returns>Amount in store currency</returns>
        private decimal AmountInStoreCurrency(decimal amount)
        {
            if (ApplicationSettings.Terminal.CompanyCurrency != ApplicationSettings.Terminal.StoreCurrency)
            {
                return this.Application.Services.Currency.CurrencyToCurrency(
                    ApplicationSettings.Terminal.CompanyCurrency,
                    ApplicationSettings.Terminal.StoreCurrency,
                    amount);
            }
            else
            {
                return amount;
            }
        }

        /// <summary>
        /// Returns the max of the least discount amount for the least exepnsive items.
        /// For example(Buy 2 get 2 Free): one transatcion like below
        /// Product1    $10     1pcs
        /// Product2    $5      1pcs
        /// Product3    $7      1pcs
        /// Product4    $9      1pcs
        /// For above scenario the Max least discount amount should be 7
        /// <param name="retailTransaction">The retailtransaction</param>
        /// <param name="noLeastExpensiveItems">The number of least expensive items that are free</param>
        /// <returns></returns>
        private decimal GetMaxLeastExpensiveAmount(int noLeastExpensiveItems, RetailTransaction retailTransaction)
        {
            decimal discountAmount = 0m;
            decimal salePrice = 0m;
            int items = 0;
            decimal[] leastExpensive = new decimal[noLeastExpensiveItems];
            int index = 0;
            foreach (DataRow row in this.tmpMMOffer.Select(string.Empty, "PRICE ASC"))
            {
                ISaleLineItem saleItem = retailTransaction.GetItem((int)row["SALELINEID"]);
                salePrice = saleItem.Price;

                if (Math.Abs(saleItem.Quantity) + items <= noLeastExpensiveItems)
                {
                    items += Math.Abs((int)saleItem.Quantity);
                }
                else
                {
                    items = noLeastExpensiveItems;
                }
                leastExpensive[index] = salePrice;
                index++;

                if (items == noLeastExpensiveItems)
                {
                    break;
                }
            }

            discountAmount = leastExpensive.Max();

            return discountAmount;
        }

        private static bool AllGroupsTriggered(DataTable mmLineGroups)
        {
            foreach (DataRow row in mmLineGroups.Select())
            {
                if ((int)row["NOOFITEMSNEEDED"] > (int)row["ITEMSTRIGGERED"])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Used to split a item line if needed in a periodicdiscount.  A splitting of a line is needed if part of the 
        /// quantity has been used in another (mix&match)offer.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="lineId">The id of the line that will be splited.</param>
        /// <param name="qtyNewLine">The quantity that will be taken taken from one line and put into a new line</param>
        private DataRow SplitLine(ref RetailTransaction retailTransaction, int lineId, decimal qtyNewLine)
        {
            if (qtyNewLine == 0M)
            {
                NetTracer.Warning("qtyNewLine parameter is zero");
                return null;
            }

            bool newLineAdded = AddNewLine(ref retailTransaction, lineId, qtyNewLine);

            DataRow offerLineRow = null;

            foreach (SaleLineItem saleItem in retailTransaction.CalculableSalesLines)
            {
                AddNewActiveOfferLine(retailTransaction, lineId, qtyNewLine, ref offerLineRow);

                //Doing the same for the tmpMMOffer rows
                foreach (DataRow row in tmpMMOffer.Select(string.Empty, "SALELINEID DESC"))
                {
                    if ((int)row["SALELINEID"] == lineId)
                    {
                        AddNewOffer(retailTransaction, qtyNewLine, row);
                        break;
                    }
                }
            }

            return offerLineRow;
        }

        /// <summary>
        /// Used to split a item line if needed in a periodicdiscount.  A splitting of a line is needed if part of the 
        /// quantity has been used in another (mix&match)offer.
        /// </summary>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <param name="lineId">The id of the line that will be splited.</param>
        /// <param name="qtyNewLine">The quantity that will be taken taken from one line and put into a new line</param>
        private DataRow SplitLineForLeastExpensive(ref RetailTransaction retailTransaction, int lineId, decimal qtyNewLine)
        {
            if (qtyNewLine == 0M)
            {
                NetTracer.Warning("qtyNewLine parameter is zero");
                return null;
            }

            bool newLineAdded = AddNewLine(ref retailTransaction, lineId, qtyNewLine);

            DataRow offerLineRow = null;
            //Refresh the offer information after adding a sales line
            if (newLineAdded)
            {
                AddNewActiveOfferLine(retailTransaction, lineId, qtyNewLine, ref offerLineRow);

                //Doing the same for the tmpMMOffer rows
                foreach (DataRow row in tmpMMOffer.Select("SALELINEID ='" + lineId.ToString() + "'"))
                {
                    if (Convert.ToDecimal(row["ITEMSTRIGGERED"]) > qtyNewLine)
                    {
                        row["ITEMSTRIGGERED"] = Convert.ToDecimal(row["ITEMSTRIGGERED"]) - qtyNewLine;
                    }

                    AddNewOffer(retailTransaction, qtyNewLine, row);
                    break;
                }
            }

            return offerLineRow;
        }

        private bool AddNewLine(ref RetailTransaction retailTransaction, int lineId, decimal qtyNewLine)
        {
            if (qtyNewLine == 0M)
            {
                NetTracer.Warning("qtyNewLine parameter is zero");
                return false;
            }

            bool newLineAdded = false;

            //Create a list for item´s to be removed
            LinkedList<SaleLineItem> newSaleLinesList = new LinkedList<SaleLineItem>();

            foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(i => i.LineId == lineId))
            {
                //Create the dublicate sale line 
                SaleLineItem newSaleItem1 = new SaleLineItem(retailTransaction.StoreCurrencyCode, this.Application.Services.Rounding, retailTransaction);
                newSaleItem1 = (SaleLineItem)saleItem.CloneLineItem();
                newSaleItem1.Quantity = qtyNewLine;
                newSaleItem1.QuantityDiscounted = 0m;
                newSaleItem1.DiscountLines.Clear();
                newSaleItem1.PeriodicDiscountPossibilities.Clear();

                newSaleLinesList.AddLast(newSaleItem1);
                newLineAdded = true;

                //Update the discount line on the original sale item to reflect the new quantity
                foreach (ILineDiscountItem discLine in saleItem.DiscountLines.Concat(saleItem.PeriodicDiscountPossibilities))
                {
                    discLine.Percentage = discLine.Percentage * (saleItem.Quantity) / Math.Abs(saleItem.QuantityDiscounted);
                    discLine.Amount = discLine.Amount * Math.Abs(saleItem.Quantity) / Math.Abs(saleItem.QuantityDiscounted);
                }

                //Set the new quantity on the orgininal sale line item
                saleItem.Quantity += -qtyNewLine;
            }

            foreach (SaleLineItem item in newSaleLinesList)
            {
                retailTransaction.Add(item);
            }

            return newLineAdded;
        }

        private void AddNewOffer(RetailTransaction retailTransaction, decimal qtyNewLine, DataRow row)
        {
            DataRow tmpMMRow;
            tmpMMRow = this.tmpMMOffer.NewRow();
            tmpMMRow["SALELINEID"] = retailTransaction.SaleItems.Count;
            tmpMMRow["ITEMSTRIGGERED"] = qtyNewLine;
            tmpMMRow["DISCTYPE"] = row["DISCTYPE"];
            tmpMMRow["DEALPRICEORDISCPCT"] = row["DEALPRICEORDISCPCT"];
            tmpMMRow["PRICE"] = row["PRICE"];
            this.tmpMMOffer.Rows.Add(tmpMMRow);
        }

        private void AddNewActiveOfferLine(RetailTransaction retailTransaction, int lineId, decimal qtyNewLine, ref DataRow offerLineRow)
        {
            foreach (DataRow row in this.activeOfferLines.Select("SALELINEID ='" + lineId.ToString() + "'"))
            {
                if (Convert.ToDecimal(row["QUANTITY"]) - qtyNewLine > 0m)
                {
                    row["QUANTITY"] = Convert.ToDecimal(row["QUANTITY"]) - qtyNewLine;
                }
                offerLineRow = this.activeOfferLines.NewRow();
                offerLineRow.ItemArray = row.ItemArray;
                offerLineRow["SALELINEID"] = retailTransaction.SaleItems.Count;
                offerLineRow["QUANTITY"] = qtyNewLine;
                this.activeOfferLines.Rows.Add(offerLineRow);
            }
        }

        /// <summary>
        /// Tries to apply specified threshold discount offer to the sales transaction.
        /// </summary>
        /// <param name="transaction">Transaction which will get possible discount lines.</param>
        /// <param name="offerId">Id of the threshold discount.</param>
        /// <param name="offerName">Name of the threshold discount.</param>
        /// <param name="discountCode">Discount code (if any) which activated this offer.</param>
        /// <param name="concurrencyMode">The concurrency mode of the discount.</param>
        /// <param name="activeOfferLines">Reference to the table joining sales lines and periodic discounts.</param>
        private static void CalculateThresholdDiscount(RetailTransaction transaction, string offerId, string offerName, string discountCode, ConcurrencyMode concurrencyMode, DataTable activeOfferLines)
        {
            // Loop through all the offer lines to calculate the total amount applicable to this discount
            decimal totalAmountForOffer = 0M;
            List<SaleLineItem> linesToDiscount = new List<SaleLineItem>();
            foreach (DataRow row in activeOfferLines.Select("OFFERID='" + offerId + "'", "OFFERID ASC, LINEID ASC"))
            {
                // Consider valid lines only. Ignore voided lines.
                var salesLine = transaction.CalculableSalesLines.Where(sl => sl.LineId.Equals(row["SALELINEID"])).FirstOrDefault();
                if (salesLine == null)
                {
                    continue;
                }

                // Skip non-discount items unless the offer is configured to count them
                if (!row.Field<bool>("COUNTNONDISCOUNTITEMS") && salesLine.NoDiscountAllowed)
                {
                    continue;
                }

                long distinctProductVariantId = row.Field<long>("DISTINCTPRODUCTVARIANTID");
                if (distinctProductVariantId == 0 || distinctProductVariantId == salesLine.Dimension.DistinctProductVariantId)
                {
                    // continue to next offer line if the unit of measure doesn't match
                    string offerUom = row.Field<string>("UNIT");
                    if (!string.IsNullOrEmpty(offerUom) && !string.Equals(offerUom, salesLine.SalesOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // add total amount from line and store line for discounting later
                    totalAmountForOffer += salesLine.Price * salesLine.Quantity;
                    linesToDiscount.Add(salesLine);
                }
            }

            // Get the tiers for this threshold discount
            IEnumerable<ThresholdDiscountTier> tiers = GetThresholdTiersByOfferId(offerId);

            // get sorted tiers with trigger <= current amount
            IEnumerable<ThresholdDiscountTier> activeTiers = tiers
                .Where(t => t.AmountThreshold <= totalAmountForOffer)
                .OrderByDescending(t => t.AmountThreshold);

            // if non-compoundable, we'll never have to worry about the total dropping due to other
            //  discounts applying, so we can truncate the list to the highest threshold reachable.
            if (concurrencyMode != ConcurrencyMode.Compounded)
            {
                activeTiers = activeTiers.Take(1);
            }

            // apply the active tiers
            int numberOfApplications = 0;
            foreach (var activeTier in activeTiers)
            {
                numberOfApplications += 1;

                if (activeTier.DiscountMethod == ThresholdDiscountMethod.AmountOff)
                {
                    ApplyAmountDiscount(linesToDiscount, activeTier.DiscountValue, activeTier.AmountThreshold, numberOfApplications, offerId, offerName, discountCode, concurrencyMode);
                }
                else if (activeTier.DiscountMethod == ThresholdDiscountMethod.PercentOff)
                {
                    ApplyPercentDiscount(linesToDiscount, activeTier.DiscountValue, activeTier.AmountThreshold, numberOfApplications, offerId, offerName, discountCode, concurrencyMode);
                }
                else
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Invalid threshold tier method: {0}", activeTier.DiscountMethod));
                }
            }
        }

        /// <summary>
        /// For each given sales line, apply distributed possible amount-based threshold discount.
        /// </summary>
        /// <param name="salesLines">Sales lines receiving the discount.</param>
        /// <param name="amountValue">The amount to subtract across the sales lines.</param>
        /// <param name="amountRequired">The amount required to trigger the threshold discount.</param>
        /// <param name="applicationCount">The number of times this threshold offer has been applied. Compound discounts may need to try multiple tiers of the same offer.</param>
        /// <param name="offerId">Id of the threshold discount.</param>
        /// <param name="offerName">Name of the threshold discount.</param>
        /// <param name="discountCode">Code, if any, that triggered the discount.</param>
        /// <param name="concurrencyMode">The concurrency mode of the discount.</param>
        private static void ApplyAmountDiscount(
            IEnumerable<SaleLineItem> salesLines,
            decimal amountValue,
            decimal amountRequired,
            int applicationCount,
            string offerId,
            string offerName,
            string discountCode,
            ConcurrencyMode concurrencyMode)
        {
            if (!salesLines.Any())
            {
                return;
            }

            // calculate amount distribution
            var lineDiscountAmountDict = new Dictionary<int, decimal>(salesLines.Count());

            IEnumerable<SaleLineItem> discountableLines = salesLines.Where(l => !l.NoDiscountAllowed);

            decimal totalDiscountableAmount = discountableLines
                .Aggregate(0M, (a, l) => a + (l.Price * l.Quantity));

            foreach (var line in discountableLines)
            {
                if (line.Price != 0m && line.Quantity != 0m)
                {
                    var linePercent = (line.Price * line.Quantity) / totalDiscountableAmount;
                    var amountOffLine = Round(amountValue * linePercent / line.Quantity);
                    lineDiscountAmountDict.Add(line.LineId, amountOffLine);
                }
            }

            // correct discount amounts so the sum exactly matches target amount
            CorrectLineDiscountAmounts(discountableLines, lineDiscountAmountDict, amountValue);

            // now apply the computed discount amount to each discountable line
            foreach (var line in discountableLines)
            {
                decimal amountOff = lineDiscountAmountDict[line.LineId];
                AddPossibleDiscountLine(ThresholdDiscountMethod.AmountOff, amountOff, line, amountRequired, applicationCount, offerId, offerName, discountCode, concurrencyMode);
            }
        }

        /// <summary>
        /// Make sure that the given discount amount matches the target discount amount.
        /// </summary>
        /// <param name="salesLines">Sales lines with the discount.</param>
        /// <param name="lineDiscountAmounts">Map of sales lines (by line Id) to given unit discount amount.</param>
        /// <param name="targetAmount">Target discount amount.</param>
        private static void CorrectLineDiscountAmounts(
            IEnumerable<SaleLineItem> salesLines,
            Dictionary<int, decimal> lineDiscountAmounts,
            decimal targetAmount)
        {
            // find given discount amount across sales lines
            decimal totalDiscountGiven = 0M;
            foreach (var line in salesLines)
            {
                totalDiscountGiven += Round(line.Quantity * lineDiscountAmounts[line.LineId]);
            }

            // only adjust if given amount doesn't equal target amount
            if (targetAmount != totalDiscountGiven)
            {
                // find most discounted line and adjust it's discount so total matches the target
                decimal amountToAdjustBy = Round(targetAmount) - Round(totalDiscountGiven);
                var mostExpensiveLine = salesLines.OrderByDescending(sl => sl.Price * sl.Quantity).First();
                lineDiscountAmounts[mostExpensiveLine.LineId] += amountToAdjustBy / (mostExpensiveLine.Quantity == 0 ? 1M : mostExpensiveLine.Quantity);
            }
        }

        /// <summary>
        /// For each given sales line, apply a possible percentage-based threshold discount.
        /// </summary>
        /// <param name="salesLines">Sales lines recieving the discount.</param>
        /// <param name="percentValue">The percent off from the discount.</param>
        /// <param name="amountRequired">The amount required to trigger the threshold discount.</param>
        /// <param name="applicationCount">The number of times this threshold offer has been applied. Compound discounts may need to try multiple tiers of the same offer.</param>
        /// <param name="offerId">Id of the threshold discount.</param>
        /// <param name="offerName">Name of the threshold discount.</param>
        /// <param name="discountCode">Code, if any, that triggered the discount.</param>
        /// <param name="concurrencyMode">The concurrency mode of the discount.</param>
        private static void ApplyPercentDiscount(
            IEnumerable<SaleLineItem> salesLines,
            decimal percentValue,
            decimal amountRequired,
            int applicationCount,
            string offerId,
            string offerName,
            string discountCode,
            ConcurrencyMode concurrencyMode)
        {
            foreach (var line in salesLines)
            {
                AddPossibleDiscountLine(ThresholdDiscountMethod.PercentOff, percentValue, line, amountRequired, applicationCount, offerId, offerName, discountCode, concurrencyMode);
            }
        }

        /// <summary>
        /// Add the threshold discount line as a possible discount to the sales line.
        /// </summary>
        /// <param name="method">Type of threshold discount.</param>
        /// <param name="value">Value of the threshold discount.</param>
        /// <param name="salesLine">The sales line recieving the possible discount.</param>
        /// <param name="amountRequired">The amount required to trigger the threshold discount.</param>
        /// <param name="applicationCount">The number of times this threshold offer has been applied. Compound discounts may need to try multiple tiers of the same offer.</param>
        /// <param name="offerId">Id of the threshold discount.</param>
        /// <param name="offerName">Name of the threshold discount.</param>
        /// <param name="discountCode">Code, if any, that triggered the discount.</param>
        /// <param name="concurrencyMode">The concurrency mode of the discount.</param>
        private static void AddPossibleDiscountLine(ThresholdDiscountMethod method, decimal value, SaleLineItem salesLine, decimal amountRequired, int applicationCount, string offerId, string offerName, string discountCode, ConcurrencyMode concurrencyMode)
        {
            if (salesLine.Price != 0m && salesLine.Quantity != 0m)
            {
                salesLine.WasChanged = true;

                PeriodicDiscountItem discountItem = new PeriodicDiscountItem
                {
                    PeriodicDiscountType = PeriodicDiscOfferType.Threshold,
                    LineDiscountType = DiscountTypes.Periodic,
                    OfferId = offerId,
                    OfferName = offerName,
                    DiscountCode = discountCode,
                    ConcurrencyMode = concurrencyMode,
                    IsCompoundable = true,
                    Percentage = 0M,
                    Amount = 0M,
                    ThresholdAmountRequired = amountRequired,                    

                    // this application of the offer should apply as a whole unit or not at all
                    PeriodicDiscGroupId = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", offerId, applicationCount),
                };

                if (method == ThresholdDiscountMethod.AmountOff)
                {
                    discountItem.Amount = value;
                }
                else if (method == ThresholdDiscountMethod.PercentOff)
                {
                    discountItem.Percentage = value;
                }

                UpdatePeriodicDiscountLines(salesLine, discountItem);
            }
        }

        /// <summary>
        /// Gets the tiers configured for the given threshold discount offer Id from the database.
        /// </summary>
        /// <param name="offerId">Offer Id to search by.</param>
        /// <returns>Collection of threshold tiers.</returns>
        private static ReadOnlyCollection<ThresholdDiscountTier> GetThresholdTiersByOfferId(string offerId)
        {
            var tiers = new List<ThresholdDiscountTier>();

            SqlConnection connection = Discount.InternalApplication.Settings.Database.Connection;
            string dataAreaId = Discount.InternalApplication.Settings.Database.DataAreaID;

            try
            {
                string queryString = @"
                    SELECT
                        tiers.RECID,
                        tiers.AMOUNTTHRESHOLD,
                        tiers.DISCOUNTMETHOD,
                        tiers.DISCOUNTVALUE,
                        tiers.OFFERID
                    FROM [dbo].RETAILDISCOUNTTHRESHOLDTIERS tiers
                    WHERE tiers.OFFERID = @OFFERID
                      AND tiers.DATAAREAID = @DATAAREAID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@OFFERID", offerId ?? string.Empty);
                    command.Parameters.AddWithValue("@DATAAREAID", dataAreaId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tier = new ThresholdDiscountTier(
                                offerId :(string)reader["OFFERID"],
                                recordId : (long)reader["RECID"],
                                amountThreshold: (decimal)reader["AMOUNTTHRESHOLD"],
                                discountMethod: (ThresholdDiscountMethod)reader["DISCOUNTMETHOD"],
                                discountValue : (decimal)reader["DISCOUNTVALUE"]
                                );
                            tiers.Add(tier);
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

            return tiers.AsReadOnly();
        }

        /// <summary>
        /// Update the periodic discount items.
        /// </summary>
        /// <param name="saleItem">The item line that the discount line is added to.</param>
        /// <param name="discountItem">The new discount item</param>
        private static void UpdatePeriodicDiscountLines(SaleLineItem saleItem, PeriodicDiscountItem discountItem)
        {
            //Check if line discount is found, if so then update
            bool discountLineFound = false;
            foreach (PeriodicDiscountItem periodicDiscLine in saleItem.PeriodicDiscountPossibilities)
            {
                //If found then update
                if ((periodicDiscLine.LineDiscountType == discountItem.LineDiscountType) &&
                    (periodicDiscLine.PeriodicDiscountType == discountItem.PeriodicDiscountType) &&
                    (periodicDiscLine.OfferId == discountItem.OfferId) &&
                    (periodicDiscLine.PeriodicDiscGroupId == discountItem.PeriodicDiscGroupId))
                {
                    periodicDiscLine.Percentage = discountItem.Percentage;
                    periodicDiscLine.Amount = discountItem.Amount;
                    periodicDiscLine.IsCompoundable = discountItem.IsCompoundable;
                    periodicDiscLine.ConcurrencyMode = discountItem.ConcurrencyMode;
                    periodicDiscLine.PeriodicDiscGroupId = discountItem.PeriodicDiscGroupId;
                    periodicDiscLine.ThresholdAmountRequired = discountItem.ThresholdAmountRequired;
                    discountLineFound = true;
                }
            }
            //If line discount is not found then add it.
            if (discountLineFound == false)
            {
                saleItem.Add(discountItem);
                String message = String.Format("Adding possible periodic discount '{0}' to item '{1}' line '{2}'.", discountItem.OfferId, saleItem.ItemId, saleItem.LineId);
                LSRetailPosis.ApplicationLog.Log("Discount.UpdatePeriodicDiscountLine()", message, LogTraceLevel.Trace);
            }
        }

        /// <summary>
        /// Rounds with rounding service.
        /// </summary>
        /// <param name="value">Value to round.</param>
        /// <returns>Rounded value.</returns>
        private static decimal Round(decimal value)
        {
            return Discount.InternalApplication.Services.Rounding.Round(value);
        }

        #endregion PeriodicDiscount

#if DEBUG
        private static string GetDescriptionForDiscountOnItem(decimal quantity, string description, decimal price, decimal discountPercentage, decimal discountAmount, string offerId, string offerName)
        {
            return string.Format("{0:F3} @ {1:C3} - {2} : -{3:F3}/{4:F3}% ({5} : {6})\r\n", quantity, price, description, discountAmount, discountPercentage, offerId, offerName);
        }
#endif
    }

    internal class PriceDiscData
    {
        public PriceDiscType Relation { get; private set; }
        public string ItemRelation { get; private set; }
        public string AccountRelation { get; private set; }
        public int ItemCode { get; private set; }
        public int AccountCode { get; private set; }
        public decimal QuantityAmount { get; private set; }
        public string TargetCurrencyCode { get; private set; }
        public Dimensions ItemDimensions { get; private set; }
        public bool IncludeDimensions { get; private set; }
        public ReadOnlyCollection<Discount.DiscountAgreementArgs> SqlResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceDiscData"/> class.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="itemRelation">The item relation.</param>
        /// <param name="accountRelation">The account relation.</param>
        /// <param name="itemCode">The item code.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="quantityAmount">The quantity amount.</param>
        /// <param name="targetCurrencyCode">The target currency code.</param>
        /// <param name="itemDimensions">The item dimensions.</param>
        /// <param name="includeDimensions">if set to <c>true</c> [include dimensions].</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public PriceDiscData(PriceDiscType relation, string itemRelation, string accountRelation, int itemCode, int accountCode, decimal quantityAmount, string targetCurrencyCode, Dimensions itemDimensions, bool includeDimensions)
            : this(null, relation, itemRelation, accountRelation, itemCode, accountCode, quantityAmount, targetCurrencyCode, itemDimensions, includeDimensions)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PriceDiscData"/> class.
        /// </summary>
        /// <param name="sqlResults">The SQL results.</param>
        /// <param name="relation">The relation.</param>
        /// <param name="itemRelation">The item relation.</param>
        /// <param name="accountRelation">The account relation.</param>
        /// <param name="itemCode">The item code.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="quantityAmount">The quantity amount.</param>
        /// <param name="targetCurrencyCode">The target currency code.</param>
        /// <param name="itemDimensions">The item dimensions.</param>
        /// <param name="includeDimensions">if set to <c>true</c> [include dimensions].</param>
        public PriceDiscData(ReadOnlyCollection<Discount.DiscountAgreementArgs> sqlResults, PriceDiscType relation, string itemRelation, string accountRelation, int itemCode, int accountCode, decimal quantityAmount, string targetCurrencyCode, Dimensions itemDimensions, bool includeDimensions)
        {
            this.SqlResults = sqlResults;

            this.Relation = relation;
            this.ItemRelation = itemRelation;

            this.AccountRelation = accountRelation;
            this.ItemCode = itemCode;
            this.AccountCode = accountCode;
            this.QuantityAmount = quantityAmount;
            this.TargetCurrencyCode = targetCurrencyCode;
            this.ItemDimensions = itemDimensions;
            this.IncludeDimensions = includeDimensions;
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Rel {0}, IR {1}, AR {2}, IC {3}, AC {4}, QTY {5}, CUR {6}, ID {7}, IID {8}", Relation, ItemRelation, AccountRelation, ItemCode, AccountCode, QuantityAmount, TargetCurrencyCode, ItemDimensions, IncludeDimensions);
        }
    }
}
