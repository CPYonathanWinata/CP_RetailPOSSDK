/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.SalesOrder.CustomerOrderParameters;
using CustomerOrderMode = LSRetailPosis.Transaction.CustomerOrderMode;
using CustomerOrderType = LSRetailPosis.Transaction.CustomerOrderType;
using SalesStatus = LSRetailPosis.Transaction.SalesStatus;
using Tax = LSRetailPosis.Transaction.Line.Tax;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Xml;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    internal static class SalesOrderActions
    {
        private static string LogSource = typeof(SalesOrderActions).ToString();

        //public string npwpAddress,npwpArea;

        /// <summary>
        /// Create a pack slip for the given order
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <param name="orderId"></param>
        internal static void TryCreatePackSlip(SalesStatus orderStatus, string orderId)
        {
            //to call pack Slip Method
            try
            {
                bool retVal;
                string comment;

                switch (orderStatus)
                {
                    case SalesStatus.Created:
                    case SalesStatus.Processing:
                        // These statuses are allowed for Packslip creation
                        break;

                    case SalesStatus.Delivered:
                    case SalesStatus.Canceled:
                    case SalesStatus.Confirmed:
                    case SalesStatus.Invoiced:
                    case SalesStatus.Lost:
                    case SalesStatus.Sent:
                    case SalesStatus.Unknown:
                    default:
                        // Please select an open order
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56132, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                if (string.IsNullOrEmpty(orderId))
                {
                    // Please select a sales order
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56116, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Prevent Packing slip creation if cashier doesn't have view/edit access
                if (SalesOrder.InternalApplication.Services.LogOn.VerifyOperationAccess(
                        SalesOrder.InternalApplication.Shift.StaffId,
                        PosisOperations.CustomerOrderDetails))
                {
                    //create Packing slip operation
                    SalesOrder.InternalApplication.Services.SalesOrder.CreatePackingSlip(out retVal, out comment, orderId);
                    if (retVal)
                    {
                        //A packing slip has been created.
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56120, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ApplicationLog.Log(SalesOrderActions.LogSource, comment, LogTraceLevel.Error);

                        if (comment.Contains(SalesOrder.PackingSlipHasSerialItemsActiveInSalesProcess))
                        {
                            // "An error occurred while printing the creating slip. You must create the packing slip in Microsoft Dynamics AX."
                            SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56266, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // "Pack list could not be created as this time."
                            SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56231, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, x);
                // "Error creating the packing slip."
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56220, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Print a pack slip for the given order
        /// </summary>
        /// <param name="documentStatus"></param>
        /// <param name="orderId"></param>
        internal static void TryPrintPackSlip(SalesStatus documentStatus, string orderId)
        {
            try
            {

                //if (!selectedOrderStatus.Equals("Delivered"))
                if (documentStatus != SalesStatus.Delivered && documentStatus != SalesStatus.Invoiced)
                {
                    // Please select an delivered order
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56133, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(orderId))
                {
                    // Please select a sales order
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56116, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Prevent Packing slip printing if cashier doesn't have view/edit access
                if (SalesOrder.InternalApplication.Services.LogOn.VerifyOperationAccess(
                        SalesOrder.InternalApplication.Shift.StaffId,
                        PosisOperations.CustomerOrderDetails))
                {
                    IRetailTransaction transaction;
                    bool retValue = false;
                    string comment;
                                        
                    SalesOrder.InternalApplication.Services.SalesOrder.GetCustomerOrder(
                        ref retValue, orderId, out comment, out transaction);
                    
                    if (retValue)
                    {
                        //Add by Erwin 21 March 2019
                        //Add print date to CPPACKINGSLIPFLAG
                        SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                        int flag_new = 0;

                        //Check if SalesID already exists
                        try
                        {
                            string querySearch = "SELECT TOP 1 SALESID FROM CPPACKINGSLIPFLAG ";
                            querySearch += "WHERE SALESID = '" + orderId + "' AND INVENTLOCATIONID = '" + ApplicationSettings.Database.StoreID + "' AND DATAAREAID = '" + ApplicationSettings.Database.DATAAREAID + "' ";

                            using (SqlCommand commandSearch = new SqlCommand(querySearch, connection))
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                using (SqlDataReader reader = commandSearch.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        flag_new = 1;
                                    }
                                    reader.Close();
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Format Error", ex);
                        }

                        if (flag_new == 1)
                        {
                            CPInsertTable(orderId);
                        }


                        //End Add by Erwin 21 March 2019

                        //Edit by Erwin 22 March 2019
                        //Change default printing method to custom method

                        //SalesOrder.InternalApplication.Services.Printing.PrintPackSlip(transaction);
                        CPPrintPackSlip(orderId, "Original");

                        if(MessageBox.Show("Print Copy Receipt?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            CPPrintPackSlip(orderId, "Copy");
                        }

                        //End Edit by Erwin 22 March 2019
                    }
                    else
                    {
                        // The sales order was not found in AX
                        ApplicationLog.Log("frmGetSalesOrder.btnPrintPackSlip_Click()",
                            string.Format("{0}/n{1}", ApplicationLocalizer.Language.Translate(56124), comment),
                            LogTraceLevel.Error);
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56124, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception x)
            {
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, x);
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56220, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Create pick list for the given order
        /// </summary>
        /// <param name="transaction"></param>
        internal static void TryCreatePickListForOrder(SalesStatus salesStatus, string orderId)
        {
            try
            {
                switch (salesStatus)
                {
                    case SalesStatus.Created:
                        // These statuses are allowed for Picking list creation
                        break;

                    case SalesStatus.Processing:
                    case SalesStatus.Delivered:
                    case SalesStatus.Canceled:
                    case SalesStatus.Confirmed:
                    case SalesStatus.Invoiced:
                    case SalesStatus.Lost:
                    case SalesStatus.Sent:
                    case SalesStatus.Unknown:
                    default:
                        // Please select an open order
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56132, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                // Prevent Picking list creation if cashier doesn't have view/edit access
                if (SalesOrder.InternalApplication.Services.LogOn.VerifyOperationAccess(
                        SalesOrder.InternalApplication.Shift.StaffId,
                        PosisOperations.CustomerOrderDetails))
                {
                    bool retValue;
                    string comment;
                    SalesOrder.CreatePickingList(orderId, out retValue, out comment);

                    if (retValue)
                    {
                        // "The pick list was created"
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56233);
                    }
                    else
                    {
                        // "Pick list could not be created as this time."
                        ApplicationLog.Log(SalesOrderActions.LogSource, comment, LogTraceLevel.Error);
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56230, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, ex);
                throw;
            }
        }

        /// <summary>
        /// Attempt to return invoices for the given order
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        internal static CustomerOrderTransaction ReturnOrderInvoices(CustomerOrderTransaction transaction)
        {
            CustomerOrderTransaction result = null;
            try
            {
                if (transaction == null)
                {
                    // Operation not valid for this type of transaction
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(3175, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }

                if (transaction.OrderType != CustomerOrderType.SalesOrder)
                {
                    // Operation not valid for this type of transaction
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(3175, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }

                bool retValue = false;
                string comment = string.Empty;

                using (DataTable invoices = SalesOrder.GetSalesInvoiceList(ref retValue, ref comment, transaction.OrderId))
                {
                    if ((!retValue) || (invoices == null) || (invoices.Rows.Count == 0))
                    {
                        if (!retValue)
                        {
                            ApplicationLog.Log(SalesOrderActions.LogSource, comment, LogTraceLevel.Error);
                        }

                        //The return can't be processed because the sales order hasn't been invoiced.
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56140, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return null;
                    }

                    // Show the available sales orders for selection...
                    using (frmGetSalesInvoices dlg = new frmGetSalesInvoices(invoices, transaction))
                    {
                        SalesOrder.InternalApplication.ApplicationFramework.POSShowForm(dlg);

                        if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            // Copy the transaction back from the 'return invoices' form
                            result = dlg.Transaction;

                            SalesOrderActions.ProcessReturnReasonCodes(result);
                        }
                    }
                }
            }
            catch (PosisException px)
            {
                POSFormsManager.ShowPOSErrorDialog(px);
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, px);
            }
            catch (Exception x)
            {
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, x);
                throw;
            }

            return result;
        }

        internal static CustomerOrderTransaction GetTransactionFromInvoice(InvoiceJournal invoice)
        {
            CustomerOrderTransaction transaction = (CustomerOrderTransaction)
                SalesOrder.InternalApplication.BusinessLogic.Utility.CreateCustomerOrderTransaction(
                    ApplicationSettings.Terminal.StoreId,
                    ApplicationSettings.Terminal.StoreCurrency,
                    ApplicationSettings.Terminal.TaxIncludedInPrice,
                    SalesOrder.InternalApplication.Services.Rounding,
                    SalesOrder.InternalApplication.Services.SalesOrder);

            // Get all the defaults
            SalesOrder.InternalApplication.BusinessLogic.TransactionSystem.LoadTransactionStatus(transaction);

            //General header properties
            transaction.OrderId = invoice.SalesId;
            transaction.OrderType = CustomerOrderType.SalesOrder;
            transaction.QuotationId = invoice.SalesId;
            transaction.OriginalOrderType = CustomerOrderType.SalesOrder;

            transaction.OrderStatus = SalesStatus.Created;
            transaction.LockPrices = true;

            transaction.ExpirationDate = DateTime.Today;
            transaction.RequestedDeliveryDate = DateTime.Today;
            transaction.BeginDateTime = DateTime.Now;
            transaction.Comment = string.Empty;

            transaction.TotalManualDiscountAmount = invoice.TotalManualDiscountAmount;
            transaction.TotalManualPctDiscount = invoice.TotalManualDiscountPercentage;

            StoreDataManager storeDataManager = new StoreDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            // Customer info
            ICustomer customer = SalesOrder.InternalApplication.BusinessLogic.CustomerSystem
                .GetCustomerInfo(invoice.InvoiceAccount);

            SalesOrder.InternalApplication.BusinessLogic.CustomerSystem
                .SetCustomer(transaction, customer, customer);

            // Items
            foreach (InvoiceItem item in invoice.Items)
            {
                AddReturnItemToTransaction(
                    invoice.InvoiceId,
                    transaction,
                    storeDataManager,
                    item);
            }

            SalesOrder.InternalApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);
            transaction.CalculateAmountDue();

            return transaction;
        }

        private static void AddReturnItemToTransaction(string invoiceId, CustomerOrderTransaction transaction, StoreDataManager storeDataManager, InvoiceItem item)
        {
            // add item
            SaleLineItem lineItem = (SaleLineItem)
                SalesOrder.InternalApplication.BusinessLogic.Utility.CreateSaleLineItem(
                    ApplicationSettings.Terminal.StoreCurrency,
                    SalesOrder.InternalApplication.Services.Rounding,
                    transaction);

            lineItem.Found = true;
            lineItem.ItemId = item.ItemId;
            lineItem.Description = item.ProductName;
            lineItem.Quantity = item.Quantity;
            lineItem.ReturnQtyAllowed = item.Quantity;
            lineItem.SalesOrderUnitOfMeasure = item.Unit;
            lineItem.Price = item.Price;
            lineItem.NetAmount = item.NetAmount;
            lineItem.SalesTaxGroupId = item.SalesTaxGroup;
            lineItem.TaxGroupId = item.ItemTaxGroup;
            lineItem.SalesMarkup = item.SalesMarkup;
            lineItem.QuantityOrdered = item.Quantity;
            lineItem.DeliveryMode = storeDataManager.GetDeliveryMode(item.DeliveryMode);
            lineItem.DeliveryDate = DateTime.Today;
            lineItem.DeliveryStoreNumber = transaction.StoreId;
            lineItem.DeliveryWarehouse = item.Warehouse;
            lineItem.SerialId = item.SerialId;
            lineItem.BatchId = item.BatchId;
            lineItem.ReturnInvoiceInventTransId = item.InventTransId;
            lineItem.ReturnInvoiceId = invoiceId;
            // When we get price from a sales invoice in AX; this is THE price that we will use
            lineItem.ReceiptReturnItem = true;

            lineItem.Dimension.ColorId = item.ColorId;
            lineItem.Dimension.SizeId = item.SizeId;
            lineItem.Dimension.StyleId = item.StyleId;
            lineItem.Dimension.ConfigId = item.ConfigId;
            lineItem.Dimension.ColorName = item.ColorName;
            lineItem.Dimension.SizeName = item.SizeName;
            lineItem.Dimension.StyleName = item.StyleName;
            lineItem.Dimension.ConfigName = item.ConfigName;

            lineItem.LineManualDiscountAmount = item.LineManualDiscountAmount;
            lineItem.LineManualDiscountPercentage = item.LineManualDiscountPercentage;
            lineItem.PeriodicDiscount = item.PeriodicDiscount;
            lineItem.PeriodicPctDiscount = item.PeriodicPercentageDiscount;
            lineItem.LineDiscount = item.LineDscAmount;
            lineItem.TotalDiscount = item.TotalDiscount;
            lineItem.TotalPctDiscount = item.TotalPctDiscount;

            foreach (InvoiceItemDiscount invoiceItemDiscount in item.Discounts)
            {
                LSRetailPosis.Transaction.Line.Discount.DiscountItem discountItem = SerializationHelper.ConvertToDiscountItem(
                    invoiceItemDiscount.DiscountOriginType,
                    invoiceItemDiscount.ManualDiscountType,
                    invoiceItemDiscount.CustomerDiscountType,
                    invoiceItemDiscount.EffectiveAmount,
                    invoiceItemDiscount.DealPrice,
                    invoiceItemDiscount.DiscountAmount,
                    invoiceItemDiscount.Percentage,
                    invoiceItemDiscount.PeriodicDiscountOfferId,
                    invoiceItemDiscount.OfferName,
                    invoiceItemDiscount.DiscountCode);

                SalesOrder.InternalApplication.Services.Discount.AddDiscountLine(lineItem, discountItem);
            }

            SalesOrder.InternalApplication.Services.Item.ProcessItem(lineItem, bypassSerialNumberEntry: true);
            transaction.Add(lineItem);
        }

        /// <summary>
        /// Prompt for return reason code and add to transaction.
        /// </summary>
        /// <param name="customerOrderTransaction">Transaction to update.</param>
        private static void ProcessReturnReasonCodes(CustomerOrderTransaction customerOrderTransaction)
        {
            if (customerOrderTransaction == null)
            {
                NetTracer.Warning("customerOrderTransaction parameter is null");
                throw new ArgumentNullException("customerOrderTransaction");
            }

            // Process codes only if it is a return order and has items selected.
            if (customerOrderTransaction.Mode == CustomerOrderMode.Return &&
                customerOrderTransaction.SaleItems != null &&
                customerOrderTransaction.SaleItems.Count > 0)
            {
                string selectedValue;
                var returnReasonCodes = SalesOrder.GetReturnReasonCodes();
                DialogResult dialogResult = SalesOrder.InternalApplication.Services.Dialog.GenericLookup(
                    (IList)returnReasonCodes,
                    "Description",
                    ApplicationLocalizer.Language.Translate(99524), // Return Reason
                    "ReasonCodeId",
                    out selectedValue, null);

                if (dialogResult == DialogResult.OK)
                {
                    customerOrderTransaction.ReturnReasonCodeId = selectedValue;
                    // Search return reason code description in returnReasonCode collection by Id. 
                    customerOrderTransaction.ReturnReasonCodeDescription = returnReasonCodes.Where(c => c.ReasonCodeId == selectedValue).Select(c => c.Description).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Copy items from one transaction, and add them as returns to another
        /// </summary>
        /// <param name="returnedItems"></param>
        /// <param name="transToReturn"></param>
        /// <param name="retailTransaction"></param>
        internal static void InsertReturnedItemsIntoTransaction(IEnumerable<int> returnedItems, RetailTransaction transToReturn, RetailTransaction retailTransaction)
        {
            SaleLineItem returnedItem;

            foreach (int lineNum in returnedItems)
            {
                returnedItem = transToReturn.GetItem(lineNum);

                // Transfer the lineId from the returned transaction to the proper property in the new transaction.
                returnedItem.ReturnLineId = returnedItem.LineId;

                // Transfer the transactionId from the returned transacton to the proper property in the new transaction.
                returnedItem.ReturnTransId = returnedItem.Transaction.TransactionId;
                returnedItem.ReturnStoreId = returnedItem.Transaction.StoreId;
                returnedItem.ReturnTerminalId = returnedItem.Transaction.TerminalId;

                returnedItem.Quantity = returnedItem.ReturnQtyAllowed * -1;
                returnedItem.QuantityDiscounted = returnedItem.QuantityDiscounted * -1;
                returnedItem.IsReturnItem = true;

                retailTransaction.Add(returnedItem);
            }

            retailTransaction.Customer.ReturnCustomer = true;

            SalesOrder.InternalApplication.Services.Tax.CalculateTax(retailTransaction);
            retailTransaction.CalcTotals();
        }

        /// <summary>
        /// Get a sales order or quote by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        internal static CustomerOrderTransaction GetCustomerOrder(string orderId, CustomerOrderType orderType, CustomerOrderMode forMode)
        {
            CustomerOrderTransaction result = null;
            IRetailTransaction order;
            bool retValue = false;
            string comment;

            // Verify that the user has rights to EDIT an order, and prompt for access.
            if (!SalesOrder.InternalApplication.Services.LogOn.VerifyOperationAccess(SalesOrder.InternalApplication.Shift.StaffId, PosisOperations.CustomerOrderDetails))
            {
                return null;
            }

            switch (orderType)
            {
                case CustomerOrderType.SalesOrder:
                    SalesOrder.InternalApplication.Services.SalesOrder.GetCustomerOrder(
                        ref retValue, orderId, out comment, out order);
                    break;
                case CustomerOrderType.Quote:
                    SalesOrder.InternalApplication.Services.SalesOrder.GetCustomerQuote(
                        ref retValue, orderId, out comment, out order);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported CustomerOrderType");
            }

            if (retValue)
            {
                CustomerOrderTransaction customerOrder = (CustomerOrderTransaction)order;

                if (customerOrder.CurrencyCode.Equals(customerOrder.StoreCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    customerOrder.Mode = forMode;

                    if (forMode == CustomerOrderMode.Cancel)
                    {
                        AddDefaultCancellationCharge(customerOrder);
                    }

                    customerOrder.CalcTotals();

                    result = customerOrder;
                }
                else
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56142, MessageBoxButtons.OK, MessageBoxIcon.Error); // The sales order can't be recalled. The sales order uses a different currency than the currency used by the store.
                }
            }
            else
            {
                // The sales order was not found in AX
                ApplicationLog.Log(SalesOrderActions.LogSource,
                    string.Format("{0}\n{1}", ApplicationLocalizer.Language.Translate(56124), comment),
                    LogTraceLevel.Error);
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56124, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// Get a list of sales order for a specific customer...
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing of data table")]
        internal static DataTable GetOrdersList(string customerSearchTerm, string orderSearchTerm, DateTime? startDate, DateTime? endDate, int? resultMaxCount)
        {
            ApplicationLog.Log(
                SalesOrderActions.LogSource,
                "Getting the list of sales orders for a customer",
                LogTraceLevel.Trace);

            DataTable salesOrders;
            bool retVal = false;
            string comment = string.Empty;

            try
            {
                salesOrders = SalesOrder.GetCustomerOrdersList(ref retVal, ref comment, customerSearchTerm, orderSearchTerm, startDate, endDate, resultMaxCount);
            }
            catch (PosisException px)
            {
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, px);
                throw;
            }
            catch (Exception x)
            {
                ApplicationExceptionHandler.HandleException(SalesOrderActions.LogSource, x);
                throw new PosisException(52300, x);
            }
            return salesOrders;
        }

        internal static ICustomerOrderTransaction ShowOrderDetailsOptions(ICustomerOrderTransaction transaction)
        {
            using (formOrderDetailsSelection frm = new formOrderDetailsSelection())
            {
                ICustomerOrderTransaction result = transaction;
                POSFormsManager.ShowPOSForm(frm);
                if (frm.DialogResult == DialogResult.OK)
                {
                    // when in cancel mode, we only allow to enter view details, cancel or close order modes.
                    bool allowedOnCancelMode = IsSelectionAllowedOnCancelOrderMode(frm.Selection);
                    CustomerOrderTransaction cot = transaction as CustomerOrderTransaction;

                    if (cot != null && cot.Mode == CustomerOrderMode.Cancel && !allowedOnCancelMode)
                    {
                        SalesOrder.InternalApplication.Services.Dialog.ShowMessage(4543);
                        return result;
                    }

                    switch (frm.Selection)
                    {
                        case OrderDetailsSelection.ViewDetails:
                            SalesOrderActions.ShowOrderDetails(transaction, frm.Selection);
                            break;

                        case OrderDetailsSelection.CloseOrder:
                            CloseOrder(transaction);
                            break;

                        case OrderDetailsSelection.Recalculate:
                            RecalculatePrices(transaction);
                            break;

                        default:
                            break;
                    }
                }

                return result;
            }
        }

        internal static void SetCustomerOrderDefaults(ICustomerOrderTransaction trans)
        {
            trans.SiteId = string.Empty;
            trans.WarehouseId = ApplicationSettings.Terminal.InventLocationId;
            trans.ChannelId = ApplicationSettings.Terminal.StorePrimaryId;
            trans.ExpirationDate = DateTime.Now.Date.AddDays(ApplicationSettings.Terminal.ExpirationDate);

            trans.CalcTotals();
        }

        /// <summary>
        /// Sets default deposit override on customer order pickup.
        /// </summary>
        /// <remarks>At the customer order pickup if deposit was overridden, zero deposit applied,
        ///  if this is not full pickup, otherwise remaining deposit is applied. </remarks>
        /// <param name="trans">The Customer order transaction.</param>
        internal static void SetDefaultDepositOverride(CustomerOrderTransaction trans)
        {
            if (trans.Mode == CustomerOrderMode.Pickup && trans.PrepaymentAmountOverridden)
            {
                bool areAllItemsInvoiced = trans.SaleItems
                   .All(salesLine => salesLine.Quantity + salesLine.QuantityPickedUp == salesLine.QuantityOrdered);

                if (areAllItemsInvoiced)
                {
                    decimal availableDeposit = (trans.PrepaymentAmountInvoiced > trans.PrepaymentAmountPaid)
                         ? decimal.Zero
                         : (trans.PrepaymentAmountPaid - trans.PrepaymentAmountInvoiced);

                    // If everything has been picked up then apply all the remaining available deposit.
                    trans.PrepaymentAmountApplied = availableDeposit;
                }
                else
                {
                    trans.PrepaymentAmountApplied = decimal.Zero;
                }
            }
        }

        internal static void WarnAndFlagForRecalculation(CustomerOrderTransaction trans)
        {
            if (trans.ExpirationDate.Date < DateTime.Now.Date)
            {
                // show warning
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56300, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // unlock prices so that they will recalculate
                RecalculatePrices(trans);
            }
        }

        private static void RecalculatePrices(ICustomerOrderTransaction transaction)
        {
            CustomerOrderTransaction cot = transaction as CustomerOrderTransaction;

            if (cot != null)
            {
                if (cot.OrderStatus == SalesStatus.Created || cot.OrderStatus == SalesStatus.Unknown)
                {
                    cot.LockPrices = false;
                    // discounts which already exist on the transaction could have been recalled from AX, 
                    //  but we don't know which ones, so to avoid over-discounting, we remove all before recalculating the order
                    cot.ClearAllDiscounts();

                    // Reset tax groups before tax calculation
                    SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(cot);

                    SalesOrder.InternalApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);
                }
                else
                {
                    //Operation is not allowed for the current order status.
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56246);
                }
            }
        }

        private static void CloseOrder(ICustomerOrderTransaction transaction)
        {
            CustomerOrderTransaction cot = transaction as CustomerOrderTransaction;
            if (cot == null)
            {
                NetTracer.Warning("CustomerOrderTransaction is null");
                throw new ArgumentNullException("CustomerOrderTransaction");
            }

            cot.EntryStatus = PosTransaction.TransactionStatus.Cancelled;
        }

        /// <summary>
        /// Shows order details dialog.
        /// </summary>
        /// <param name="transaction">The customer order transaction to be shown the details of.</param>
        /// <param name="selectionMode">The detail selection mode.</param>
        /// <returns>Whether the user has chosen to go to the transaction page or cancel and return to the previous view.
        /// False means that the user cancelled the operation and returned to the previous view.</returns>
        internal static bool ShowOrderDetails(ICustomerOrderTransaction transaction, OrderDetailsSelection selectionMode)
        {
            CustomerOrderTransaction cot = (CustomerOrderTransaction)transaction;
            
            bool hasSO = false;
            hasSO = cot.OrderId == null || cot.OrderId == "" ? false : true;
            using (formOrderDetails frm = new formOrderDetails(cot, selectionMode))
            {
                POSFormsManager.ShowPOSForm(frm);
                DialogResult result = frm.DialogResult;

                // Get updated transaction since nested operations might have been run
                transaction = frm.Transaction;
                //add here for validation b2b or regular customer
                if (result == DialogResult.OK)
                {
                    //CHECK B2B by Yonathan 06/05/2024

                    string isB2bCust = "0";
                    ReadOnlyCollection<object> containerArray;
                    if (SalesOrder.InternalApplication.TransactionServices.CheckConnection())
                    {
                        try
                        {
                            containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getB2bRetailParam", cot.Customer.CustomerId.ToString());
                            isB2bCust = containerArray[6].ToString();


                        }
                        catch (Exception ex)
                        {
                            //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                            throw;
                        }
                    }

                    //Check to validate B2B or Regular customer
                    if ((isB2bCust == "1" || isB2bCust == "2") && hasSO == false) //if B2B but no SO yet
                    {
                        // Update the editing mode of the order.
                        UpdateCustomerOrderMode(cot, selectionMode);

                        // Refresh prices/totals
                        SalesOrder.InternalApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);

                        // Apply deposit override, if it was overriden
                        SalesOrderActions.SetDefaultDepositOverride(cot);

                        // call CalcTotal to roll up misc charge taxes
                        transaction.CalcTotals();

                        // Reminder prompt for Deposit Override w/ Zero deposit applied and has some deposit remaining.
                        if (selectionMode == OrderDetailsSelection.PickupOrder
                            && cot.PrepaymentAmountOverridden
                            && cot.PrepaymentAmountApplied == decimal.Zero
                            && cot.PrepaymentAmountPaid > cot.PrepaymentAmountInvoiced)
                        {
                            SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56139, MessageBoxButtons.OK, MessageBoxIcon.Information);    //"No deposit has been applied to this pickup. To apply a deposit, use the ""Deposit override"" operation."
                        }
                    }
                    else if ((isB2bCust == "1" || isB2bCust == "2") && hasSO == true) //if B2B but no SO yet
                    {
                        //if not, then only save the changes
                        CustomerOrderInfo parameters = SerializationHelper.GetInfoFromTransaction(cot);

                        // Serialize the Customer Order info into an XML doc
                        string xmlString = parameters.ToXml();

                        containerArray = SalesOrder.InternalApplication.TransactionServices.Invoke("updateCustomerOrder", xmlString);

                        if ((bool)containerArray[1] == true)
                        {
                            SalesOrder.InternalApplication.Services.Dialog.ShowMessage("Sales order updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        transaction.OperationCancelled = true;

                    }
                    else
                    {
                        //if not, then only save the changes
                        CustomerOrderInfo parameters = SerializationHelper.GetInfoFromTransaction(cot);

                        // Serialize the Customer Order info into an XML doc
                        string xmlString = parameters.ToXml();

                        containerArray = SalesOrder.InternalApplication.TransactionServices.Invoke("updateCustomerOrder", xmlString);

                        if ((bool)containerArray[1] == true)
                        {
                            SalesOrder.InternalApplication.Services.Dialog.ShowMessage("Sales order updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        transaction.OperationCancelled = true;
                    }

                    //// Update the editing mode of the order.
                    //UpdateCustomerOrderMode(cot, selectionMode);

                    //// Refresh prices/totals
                    //SalesOrder.InternalApplication.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);

                    //// Apply deposit override, if it was overriden
                    //SalesOrderActions.SetDefaultDepositOverride(cot);

                    //// call CalcTotal to roll up misc charge taxes
                    //transaction.CalcTotals();

                    //// Reminder prompt for Deposit Override w/ Zero deposit applied and has some deposit remaining.
                    //if (selectionMode == OrderDetailsSelection.PickupOrder
                    //    && cot.PrepaymentAmountOverridden
                    //    && cot.PrepaymentAmountApplied == decimal.Zero
                    //    && cot.PrepaymentAmountPaid > cot.PrepaymentAmountInvoiced)
                    //{
                    //    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56139, MessageBoxButtons.OK, MessageBoxIcon.Information);    //"No deposit has been applied to this pickup. To apply a deposit, use the ""Deposit override"" operation."
                    //}
                }
                else
                {
                    // Set cancel on the transaction so the original is not updated
                    transaction.OperationCancelled = true;
                }
            }

            return !transaction.OperationCancelled;
        }

        private static bool IsSelectionAllowedOnCancelOrderMode(OrderDetailsSelection selectionMode)
        {
            switch (selectionMode)
            {
                case OrderDetailsSelection.CancelOrder:
                case OrderDetailsSelection.CloseOrder:
                case OrderDetailsSelection.None:
                case OrderDetailsSelection.ViewDetails:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Add the default cancellation charge configured for this store
        /// </summary>
        /// <param name="transaction"></param>
        private static void AddDefaultCancellationCharge(CustomerOrderTransaction transaction)
        {
            Contracts.IApplication app = SalesOrder.InternalApplication;

            Tax.MiscellaneousCharge charge = transaction.MiscellaneousCharges.SingleOrDefault(
                m => string.Equals(m.ChargeCode, ApplicationSettings.Terminal.CancellationChargeCode, StringComparison.OrdinalIgnoreCase));

            // if there is not already a charge on this order, then attempt to add the default charge
            if (charge == null && transaction.OrderType == CustomerOrderType.SalesOrder)
            {
                // Get Cancellation charge properties from DB
                StoreDataManager store = new StoreDataManager(
                    app.Settings.Database.Connection,
                    app.Settings.Database.DataAreaID);

                DataEntity.MiscellaneousCharge chargeProperties = store.GetMiscellaneousCharge(
                    ApplicationSettings.Terminal.CancellationChargeCode);

                if (chargeProperties != null)
                {
                    // Compute the default charge rate
                    decimal chargePercent = ApplicationSettings.Terminal.CancellationCharge;
                    decimal chargeAmount = decimal.Zero;

                    if (chargePercent >= decimal.Zero)
                    {
                        chargeAmount = (transaction.NetAmountWithTaxAndCharges * chargePercent) / 100m;
                        chargeAmount = app.Services.Rounding.Round(chargeAmount, transaction.StoreCurrencyCode);
                    }

                    // construct and add the new charge
                    charge = (Tax.MiscellaneousCharge)app.BusinessLogic.Utility.CreateMiscellaneousCharge(
                        chargeAmount,
                        transaction.Customer.SalesTaxGroup,
                        chargeProperties.TaxItemGroup,
                        chargeProperties.MarkupCode,
                        string.Empty,
                        transaction);

                    // Set the proper SalesTaxGroup based on the store/customer settings
                    SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(charge);
                    transaction.MiscellaneousCharges.Add(charge);
                }
                else
                {
                    NetTracer.Information("chargeProperties is null");
                }
            }
        }

        private static void UpdateCustomerOrderMode(CustomerOrderTransaction cot, OrderDetailsSelection selectionMode)
        {
            switch (selectionMode)
            {
                case OrderDetailsSelection.CancelOrder:
                    {
                        cot.Mode = CustomerOrderMode.Cancel;
                    }
                    break;

                case OrderDetailsSelection.ViewDetails:
                    {
                        // if the order was cancelled before, we shouldn't change this mode when just viewing the order.
                        if (cot.Mode != CustomerOrderMode.Cancel)
                        {
                            if (cot.OriginalOrderType == CustomerOrderType.Quote
                                && cot.OrderType == CustomerOrderType.SalesOrder
                                && !string.IsNullOrWhiteSpace(cot.QuotationId))
                            {
                                // Change mode to 'Convert', if user is converting a Quote to a SalesOrder
                                cot.Mode = CustomerOrderMode.Convert;
                            }
                            else if (cot.Mode == CustomerOrderMode.Convert)
                            {
                                // Change mode to 'Edit', if user converted from Quote to SalesOrder and then back to Quote.
                                cot.Mode = CustomerOrderMode.Edit;
                            }
                        }
                    }
                    break;

                case OrderDetailsSelection.PickupOrder: //Pickup mode has already been handled by the ItemDetailsPage on frmOrderDetails
                default:
                    break;
            }
        }

        /*
         * By Erwin 25 March 2019
         * Create Method for Insert Data to local DB
         * */
        internal static void CPInsertTable(string orderId)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            
            try
            {
                // Insert new Row
                string queryString = @"INSERT INTO CPPACKINGSLIPFLAG (
                                                            SALESID,
                                                            INVENTLOCATIONID,
                                                            DATAAREAID,
                                                            PRINTDATE
                                                            )
                                                    VALUES (
                                                            @SALESID,
                                                            @INVENTLOCATIONID,
                                                            @DATAAREAID,
                                                            @PRINTDATE
                                                            )";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@SALESID", orderId);
                    command.Parameters.AddWithValue("@INVENTLOCATIONID", ApplicationSettings.Database.StoreID);
                    command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);
                    command.Parameters.AddWithValue("@PRINTDATE", DateTime.Now);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }

        }
        /*try
                {
                    CreatePackingSlip(out retValue, out comment, salesID, updatedXmlString);
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56120, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    //application.Services.Printing.PrintReceipt(Microsoft.Dynamics.Retail.Pos.Contracts.Services.FormType.SalesOrderReceipt, posTransaction, true);
                    SalesOrderActions.TryPrintPackSlip(LSRetailPosis.Transaction.SalesStatus.Delivered, salesID);
                    this.Close();
                }
                catch (Exception x)
                {
                    ApplicationExceptionHandler.HandleException(CP_SalesOrderDetail.LogSource, x);
                    // "Error creating the packing slip."
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56220, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/

        internal static int CPGenerateDetails(ref int offset, string orderId, ref string format, string blank_space)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            //SqlConnection connection = new SqlConnection(connectionString);
            int totalQty = 0;
            string itemDetails;//add by Yonathan 31/05/2023
            string xmlResponse = "";
            try
            {
                object[] parameterList = new object[] 
							{
								orderId.ToString(),                               
								ApplicationSettings.Database.DATAAREAID.ToString()
								
							};

                
                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getPackingSlipInfoDetails", parameterList);
                xmlResponse = containerArray[3].ToString();
            }
            catch (Exception x)
            {
                ApplicationExceptionHandler.HandleException(LogSource, x);
                // "Error creating the packing slip."
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56220, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

             //"<PackingSlip><CustConfirmTrans ItemId=\"10150014\" Name=\"LE MINERALE 600 ml\" Qty=\"4\" SalesPrice=\"3500.000\" DiscPercent=\"0\" DiscAmount=\"0.000\" LineAmount=\"12613.000\" /><CustConfirmTrans ItemId=\"12020002\" Name=\"INDOMIE GORENG\" Qty=\"3\" SalesPrice=\"2200.000\" DiscPercent=\"0\" DiscAmount=\"0.000\" LineAmount=\"5946.000\" /></PackingSlip>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlResponse);

            XmlNodeList custConfirmTransNodes = xmlDoc.SelectNodes("//CustConfirmTrans");

            foreach (XmlNode node in custConfirmTransNodes)
            {
                string itemId = node.Attributes["ItemId"].Value;
                string name = node.Attributes["Name"].Value.PadRight(16).Substring(0, 15); 
                string qty = decimal.Parse(node.Attributes["Qty"].Value).ToString("0");
                string price = node.Attributes["SalesPrice"].Value.Replace(".000",""); // decimal.Parse(node.Attributes["SalesPrice"].Value).ToString("0");
                string discPercent = node.Attributes["DiscPercent"].Value.Replace(".000", ""); ;
                string discAmount = node.Attributes["DiscAmount"].Value.Replace(".000", ""); ;
                string subTotalDB = node.Attributes["LineAmount"].Value.Replace(".000", ""); ;

                //string itemId = readerDetails["ITEMID"].ToString();
                //string name = readerDetails["NAME"].ToString().PadRight(16).Substring(0, 15);
                //string qty = readerDetails["QTY"].ToString();
                //string price = readerDetails["SALESPRICE"].ToString();
                //string discPercent = readerDetails["DISCPERCENT"].ToString();
                //string discAmount = readerDetails["DISCAMOUNT"].ToString();
                //string subTotalDB = readerDetails["LINEAMOUNT"].ToString();

                /*int subTotalWithoutDisc = int.Parse(price) * int.Parse(qty);
                int lineDiscAmount = int.Parse(discAmount) * int.Parse(qty);
                int subTotalwithDiscAmount = subTotalWithoutDisc - lineDiscAmount;
                decimal discPercentAmount = subTotalwithDiscAmount * (decimal.Parse(discPercent) / 100);
                int subTotalReal = (int)(subTotalwithDiscAmount - discPercentAmount);*/

                //modified by Yonathan /31/05/2023 to provide multiline item name
                itemDetails = blank_space +
                            itemId.PadRight(9) +
                            name.PadRight(15) +
                            qty.PadLeft(4) +
                            price.PadLeft(6) +
                            discPercent.PadLeft(5) +
                            discAmount.PadLeft(7) +
                            subTotalDB.PadLeft(8) +
                            Environment.NewLine;



                string item = node.Attributes["Name"].Value;
                int maxLength = 15;

                string[] lines = Enumerable.Range(0, (int)Math.Ceiling((double)item.Length / maxLength))
                    .Select(i => item.Substring(i * maxLength, Math.Min(maxLength, item.Length - i * maxLength)))
                    .ToArray();
                if (lines.Count() > 1)
                {
                    //foreach (string line in lines)
                    //{
                    for (int i = 1; i < lines.Length; i++)
                    {
                        
                        lines[i] = lines[i].StartsWith(" ") ? lines[i].TrimStart() : lines[i];
                        itemDetails += blank_space +
                                    string.Empty.PadRight(9) +
                                    lines[i].PadRight(15) +
                                    string.Empty.PadLeft(4) +
                                    string.Empty.PadLeft(6) +
                                    string.Empty.PadLeft(5) +
                                    string.Empty.PadLeft(7) +
                                    string.Empty.PadLeft(8) +
                                    Environment.NewLine;
                    }

                }
                format += itemDetails;
                //end mod

                /*
                format += blank_space +
                    itemId.PadRight(9) +
                    name.PadRight(15) +
                    qty.PadLeft(4) +
                    price.PadLeft(6) +
                    discPercent.PadLeft(5) +
                    discAmount.PadLeft(7) +
                    subTotalDB.PadLeft(8) +
                    Environment.NewLine;
                */
                totalQty += int.Parse(qty);

                offset += 13;
            }


//            try
//            {
//                string queryDetails = @"SELECT 
//                                            C.ITEMID,
//	                                        C.NAME,
//	                                        CAST(C.QTY AS INT) AS QTY,
//	                                        CAST(C.SALESPRICE AS INT) AS SALESPRICE,
//	                                        CAST(C.DISCPERCENT AS decimal(18,1)) AS DISCPERCENT,
//	                                        CAST(C.DISCAMOUNT AS INT) AS DISCAMOUNT,
//                                            CAST(C.LINEAMOUNT AS INT) AS LINEAMOUNT
//                                        FROM
//	                                        CUSTCONFIRMTRANS C
//                                        WHERE
//	                                        C.SALESID = @SALESID
//	                                        AND C.DATAAREAID = @DATAAREAID
//                                            AND C.CONFIRMID = (
//                                                SELECT TOP 1 CONFIRMID 
//                                                FROM CUSTCONFIRMJOUR J 
//                                                WHERE J.SALESID = @SALESID AND DATAAREAID = @DATAAREAID 
//                                                ORDER BY CONFIRMID DESC)";

//                using (SqlCommand commandDetails = new SqlCommand(queryDetails, connection))
//                {
//                    commandDetails.Parameters.AddWithValue("@SALESID", orderId);
//                    commandDetails.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);

//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }

//                    using (SqlDataReader readerDetails = commandDetails.ExecuteReader())
//                    {
//                        while (readerDetails.Read())
//                        {
//                            string itemId = readerDetails["ITEMID"].ToString();
//                            string name = readerDetails["NAME"].ToString().PadRight(16).Substring(0, 15);
//                            string qty = readerDetails["QTY"].ToString();
//                            string price = readerDetails["SALESPRICE"].ToString();
//                            string discPercent = readerDetails["DISCPERCENT"].ToString();
//                            string discAmount = readerDetails["DISCAMOUNT"].ToString();
//                            string subTotalDB = readerDetails["LINEAMOUNT"].ToString();

//                            /*int subTotalWithoutDisc = int.Parse(price) * int.Parse(qty);
//                            int lineDiscAmount = int.Parse(discAmount) * int.Parse(qty);
//                            int subTotalwithDiscAmount = subTotalWithoutDisc - lineDiscAmount;
//                            decimal discPercentAmount = subTotalwithDiscAmount * (decimal.Parse(discPercent) / 100);
//                            int subTotalReal = (int)(subTotalwithDiscAmount - discPercentAmount);*/

//                            format += blank_space +
//                                itemId.PadRight(9) +
//                                name.PadRight(15) +
//                                qty.PadLeft(4) +
//                                price.PadLeft(6) +
//                                discPercent.PadLeft(5) +
//                                discAmount.PadLeft(7) +
//                                subTotalDB.PadLeft(8) +
//                                Environment.NewLine;

//                            totalQty += int.Parse(qty);

//                            offset += 13;
//                        }
//                    }
//                }
//            }
//            catch (SqlException ex)
//            {
//                throw new Exception("Format Error", ex);
//            }

            return totalQty;
        }

        /*
         * By Erwin 25 March 2019
         * Create method for Generate Printing Details format
         * */
        internal static int CPGenerateDetailsOld(ref int offset, string orderId, ref string format, string blank_space)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            int totalQty = 0;

            try
            {
                string queryDetails = @"SELECT 
                                            C.ITEMID,
	                                        C.NAME,
	                                        CAST(C.QTY AS INT) AS QTY,
	                                        CAST(C.SALESPRICE AS INT) AS SALESPRICE,
	                                        CAST(C.DISCPERCENT AS decimal(18,1)) AS DISCPERCENT,
	                                        CAST(C.DISCAMOUNT AS INT) AS DISCAMOUNT,
                                            CAST(C.LINEAMOUNT AS INT) AS LINEAMOUNT
                                        FROM
	                                        CUSTCONFIRMTRANS C
                                        WHERE
	                                        C.SALESID = @SALESID
	                                        AND C.DATAAREAID = @DATAAREAID
                                            AND C.CONFIRMID = (
                                                SELECT TOP 1 CONFIRMID 
                                                FROM CUSTCONFIRMJOUR J 
                                                WHERE J.SALESID = @SALESID AND DATAAREAID = @DATAAREAID 
                                                ORDER BY CONFIRMID DESC)";

                using (SqlCommand commandDetails = new SqlCommand(queryDetails, connection))
                {
                    commandDetails.Parameters.AddWithValue("@SALESID", orderId);
                    commandDetails.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader readerDetails = commandDetails.ExecuteReader())
                    {
                        while (readerDetails.Read())
                        {
                            string itemId = readerDetails["ITEMID"].ToString();
                            string name = readerDetails["NAME"].ToString().PadRight(16).Substring(0, 15);
                            string qty = readerDetails["QTY"].ToString();
                            string price = readerDetails["SALESPRICE"].ToString();
                            string discPercent = readerDetails["DISCPERCENT"].ToString();
                            string discAmount = readerDetails["DISCAMOUNT"].ToString();
                            string subTotalDB = readerDetails["LINEAMOUNT"].ToString();

                            /*int subTotalWithoutDisc = int.Parse(price) * int.Parse(qty);
                            int lineDiscAmount = int.Parse(discAmount) * int.Parse(qty);
                            int subTotalwithDiscAmount = subTotalWithoutDisc - lineDiscAmount;
                            decimal discPercentAmount = subTotalwithDiscAmount * (decimal.Parse(discPercent) / 100);
                            int subTotalReal = (int)(subTotalwithDiscAmount - discPercentAmount);*/

                            format += blank_space +
                                itemId.PadRight(9) +
                                name.PadRight(15) +
                                qty.PadLeft(4) +
                                price.PadLeft(6) +
                                discPercent.PadLeft(5) +
                                discAmount.PadLeft(7) +
                                subTotalDB.PadLeft(8) +
                                Environment.NewLine;

                            totalQty += int.Parse(qty);

                            offset += 13;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }

            return totalQty;
        }

        /* */

        
        //Mod by Yonathan 19/05/2023 change from CPCONNECTION to using RetailTransactionService class
        internal static void CPGenerateFooter(string orderId, ref string format, string blank_space, int TotalQty)
        {
            string confirmId = "";
            int confirmAmount = 0;
            int sumTax = 0;
            int sumLineDisc = 0;
            int sumMarkUp = 0;

            string xmlResponse = "";
            try
            {
                object[] parameterList = new object[] 
							{
								orderId.ToString(),                               
								ApplicationSettings.Database.DATAAREAID.ToString()
								
							};


                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getPackingSlipInfoFooter", parameterList);
                //xmlResponse = 
                confirmId = containerArray[3].ToString();
                confirmAmount = int.Parse(containerArray[4].ToString());
                sumTax = int.Parse(containerArray[5].ToString());
                sumLineDisc = int.Parse(containerArray[6].ToString());
                sumMarkUp = int.Parse(containerArray[7].ToString());

                format += blank_space + "Total Qty Delivered".PadRight(20) + " : " + TotalQty + Environment.NewLine;
                format += blank_space + "Total Discount".PadRight(20) + " : " + sumLineDisc + Environment.NewLine;
                format += blank_space + "Delivery Cost".PadRight(20) + " : " + sumMarkUp + Environment.NewLine;
                format += blank_space + "Total Tax".PadRight(20) + " : " + sumTax + Environment.NewLine;
                format += blank_space + "Grand Total".PadRight(20) + " : " + confirmAmount + Environment.NewLine;
            }
            catch (Exception x)
            {
                ApplicationExceptionHandler.HandleException(LogSource, x);
                // "Error creating the packing slip."
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56220, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * By Erwin 26 March 2019
         * Create method for Generate Printing Footer format
         * */
        internal static void CPGenerateFooterOld(string orderId, ref string format, string blank_space, int TotalQty)
        {
            string confirmId = "";
            int confirmAmount = 0;
            int sumTax = 0;
            int sumLineDisc = 0;
            int sumMarkUp = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            // Get CustConfirmJour data
            try
            {
                string queryJour = @"SELECT 
                                        CONFIRMID, 
                                        CAST(CONFIRMAMOUNT AS INT) AS CONFIRMAMOUNT, 
                                        CAST(SUMTAX AS INT) AS SUMTAX, 
                                        CAST(SUMLINEDISC AS INT) AS SUMLINEDISC,
                                        CAST(SUMMARKUP AS INT) AS SUMMARKUP 
                                    FROM CUSTCONFIRMJOUR 
                                    WHERE salesid = '" + orderId + @"'
                                    AND DATAAREAID = '" + ApplicationSettings.Database.DATAAREAID + @"' 
                                    ORDER BY CONFIRMDOCNUM DESC";

                using (SqlCommand commandJour = new SqlCommand(queryJour, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader readerJour = commandJour.ExecuteReader())
                    {
                        if (readerJour.Read())
                        {
                            confirmId = readerJour["CONFIRMID"].ToString();
                            confirmAmount = int.Parse(readerJour["CONFIRMAMOUNT"].ToString());
                            sumTax = int.Parse(readerJour["SUMTAX"].ToString());
                            sumLineDisc = int.Parse(readerJour["SUMLINEDISC"].ToString());
                            sumMarkUp = int.Parse(readerJour["SUMMARKUP"].ToString());

                            format += blank_space + "Total Qty Delivered".PadRight(20) + " : " + TotalQty + Environment.NewLine;
                            format += blank_space + "Total Discount".PadRight(20) + " : " + sumLineDisc + Environment.NewLine;
                            format += blank_space + "Delivery Cost".PadRight(20) + " : " + sumMarkUp + Environment.NewLine;
                            format += blank_space + "Total Tax".PadRight(20) + " : " + sumTax + Environment.NewLine;
                            format += blank_space + "Grand Total".PadRight(20) + " : " + confirmAmount + Environment.NewLine;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }

            
        }

        /*
         * By Erwin 21 March 2019
         * Create Custom method for printing
         * Main method for custom printing
         * */
        internal static void CPPrintPackSlip(string orderId, string title)
        {
            //Initialize printing properties
            int offset = 0;
            string text_format = CPPrintingFormat(ref offset, orderId, title);

            PrintDocument print_document = new PrintDocument();
            PrintDialog print_dialog = new PrintDialog();
            PaperSize paper_size = new PaperSize("Custom", 100, offset + 100);
            Margins margin = new Margins(0, 0, 0, 0);

            //Processing Print
            print_dialog.Document = print_document;
            print_dialog.Document.DefaultPageSettings.PaperSize = paper_size;
            print_dialog.Document.DefaultPageSettings.Margins = margin;
            print_document.DefaultPageSettings.PaperSize.Width = 600;

            print_document.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(
                    text_format,
                    new Font("Lucida Console", 6),
                    new SolidBrush(Color.Black),
                    new RectangleF(
                        print_document.DefaultPageSettings.PrintableArea.Left,
                        0,
                        print_document.DefaultPageSettings.PrintableArea.Width,
                        print_document.DefaultPageSettings.PrintableArea.Height
                        )
                    );
            };

            try
            {
                print_document.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        //add by Yonathan
//        private void getNPWPDetail()
//        {
//            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
//            try
//            {
//                string queryString = @" SELECT NPWPAddress , NPWPArea FROM ax.[RETAILSTORETABLE]
//                                         WHERE STORENUMBER = @STOREID ";

//                using (SqlCommand command = new SqlCommand(queryString, connection))
//                {
//                    command.Parameters.AddWithValue("@STOREID", ApplicationSettings.Database.StoreID);

//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            npwpAddress = reader.GetString(0);
//                            npwpArea = reader.GetString(1);

//                        }
//                    }
//                }


//            }
//            catch (Exception ex)
//            {
//                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
//                throw;
//            }
//            finally
//            {
//                if (connection.State != ConnectionState.Closed)
//                {
//                    connection.Close();
//                }
//            }

//        }

        /*
         * Mod by Yonathan 19/05/2023 change from CPCONNECTION to using RetailTransactionService class
         * Create Custom method for printing
         * Create String format for receipt's content
         * */
        internal static string CPPrintingFormat(ref int offset, string orderId, string title)
        {
            string format = "";
            string blank_space = "\t     ";
            string single_line_break = blank_space;
            string double_line_break = blank_space;
            int is_data = 0;

            for (int i = 0; i < 55; i++)
            {
                single_line_break += "-";
                double_line_break += "=";
            }

            //string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
           // SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                object[] parameterList = new object[] 
							{
								orderId.ToString(),
                                ApplicationSettings.Database.StoreID.ToString(),
								ApplicationSettings.Database.DATAAREAID.ToString()
								
							};


                ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("getPackingSlipInfoHeader", parameterList);

                if (containerArray[2].ToString() == "Success")
                {
                    //each line use offset = 13
                    is_data = 1;

                    //Header

                    //format += Environment.NewLine;
                    //format += Environment.NewLine;
                    format += blank_space + "PT PRIMAFOOD INTERNATIONAL" + Environment.NewLine;
                    format += blank_space + "CABANG : " + ApplicationSettings.Terminal.StoreName + Environment.NewLine;
                    format += blank_space + "NPWP : " + ApplicationSettings.Terminal.TaxIdNumber + Environment.NewLine;
                    format += blank_space + "=======================================================" + Environment.NewLine;
                    format += blank_space + "Receipt Packing Slip" + Environment.NewLine;
                    format += blank_space + "Delivery Order " + title + Environment.NewLine;
                    format += blank_space + "=======================================================" + Environment.NewLine;
                    format += Environment.NewLine;
                    format += blank_space + "Sales ID".PadRight(20) + " : " + orderId + Environment.NewLine;
                    format += blank_space + "Created Date".PadRight(20) + " : " + DateTime.Parse(containerArray[17].ToString()).ToString("dd/MM/yyyy") + Environment.NewLine;
                    format += blank_space + "Requested Ship Date".PadRight(20) + " : " + DateTime.Parse(containerArray[8].ToString()).ToString("dd/MM/yyyy") + Environment.NewLine;
                    format += blank_space + "Delivery Date".PadRight(20) + " : " + DateTime.Parse(containerArray[6].ToString()).ToString("dd/MM/yyyy") + Environment.NewLine + Environment.NewLine;

                    format += blank_space + "Ship To " + Environment.NewLine;
                    format += blank_space + "-------" + Environment.NewLine;

                    string full_address = "";
                    int address_length = 0;

                    if (containerArray[13].ToString() != "") //13
                    {
                        format += blank_space + "Account : " + containerArray[9].ToString() + Environment.NewLine; //8
                        format += blank_space + "Nama Pembeli : " + containerArray[11].ToString(); //10
                        format += "(" + containerArray[13].ToString() + ")" + Environment.NewLine; //12
                        format += blank_space + "Info : " + containerArray[10].ToString() + Environment.NewLine; //9
                        //format += blank_space + "Alamat Kirim : " + reader.GetString(reader.GetOrdinal("CPDELIVERYADDR")) + Environment.NewLine;
                        full_address = containerArray[12].ToString(); //11
                        address_length = containerArray[12].ToString().Length ;
                    }
                    else
                    {
                        full_address = containerArray[19].ToString(); //18
                        address_length = containerArray[19].ToString().Length;
                        format += blank_space + "Nama Pembeli : " + containerArray[20].ToString() + Environment.NewLine; //19
                    }

                    int start_index = 0;
                    full_address = Regex.Replace(full_address, @"\r\n?|\n", " ");

                    if (address_length <= 41)
                    {
                        format += blank_space + "Alamat Kirim : " + full_address + Environment.NewLine;
                    }
                    else
                    {
                        format += blank_space + "Alamat Kirim : " + full_address.Substring(start_index, 41) + Environment.NewLine;
                        address_length -= 41;
                        start_index += 41;

                        while (address_length > 0)
                        {
                            if (address_length <= 54)
                            {
                                format += blank_space + full_address.Substring(start_index) + Environment.NewLine;
                            }
                            else
                            {
                                format += blank_space + full_address.Substring(start_index, 54) + Environment.NewLine;
                            }

                            address_length -= 54;
                            start_index += 54;
                            offset += 13;
                        }
                    }

                    format += Environment.NewLine;

                    offset += 182;
                }
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.HandleException(LogSource, ex);
                throw;
            }


            

            int TotalQty = 0;

            if (is_data != 0)
            {
                // Data Exists, proceed to generate details
                // Generate Details Label
                string labelItemID = "ID";
                string labelItemName = "Name";
                string labelQty = "Qty";
                string labelPrice = "Prc";
                string labelDiscPercent = "Dsc%";
                string labelDiscAmount = "DscAmt";
                string labelSubTotal = "SubTtl";

                format += blank_space +
                    labelItemID.PadRight(9) +
                    labelItemName.PadRight(15) +
                    labelQty.PadLeft(4) +
                    labelPrice.PadLeft(6) +
                    labelDiscPercent.PadLeft(5) +
                    labelDiscAmount.PadLeft(7) +
                    labelSubTotal.PadLeft(8) +
                    Environment.NewLine;

                format += single_line_break + Environment.NewLine;

                // Generate Details Data
                TotalQty = CPGenerateDetails(ref offset, orderId, ref format, blank_space);
            }

            //Footer
            format += Environment.NewLine + double_line_break + Environment.NewLine;

            CPGenerateFooter(orderId, ref format, blank_space, TotalQty);

            format += double_line_break + Environment.NewLine;

            format += Environment.NewLine;
            format += blank_space + "Created,".PadRight(40) + "Receive," + Environment.NewLine;
            for (int i = 0; i < 6; i++)
            {
                format += Environment.NewLine;
            }

            format += blank_space + "PrimaFreshmart".PadRight(40) + "Customer" + Environment.NewLine;

            offset += 143;

            //offset plus 120 at the end of document
            offset += 120;

            return format;
        }
        
        internal static string CPPrintingFormatOld(ref int offset, string orderId, string title)
        {
            string format = "";
            string blank_space = "\t     ";
            string single_line_break = blank_space;
            string double_line_break = blank_space;
            int is_data = 0;

            for (int i = 0; i < 55; i++)
            {
                single_line_break += "-";
                double_line_break += "=";
            }

            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                string queryString = @"
                                        SELECT
	                                        ST.SALESID, 
	                                        ST.CUSTACCOUNT,
	                                        ST.INVOICEACCOUNT,
	                                        CONVERT(VARCHAR, CP.DELIVERYDATE, 101) AS DELIVERYDATE,
	                                        ST.DOCUMENTSTATUS,
	                                        ST.INVENTLOCATIONID,
	                                        CONVERT(VARCHAR, ST.SHIPPINGDATEREQUESTED, 101) AS SHIPPINGDATEREQUESTED,
	                                        ST.CPONLINESTOREID,
	                                        ST.CPORDERNUMBER,
	                                        ST.CPBUYERNAME,
	                                        ST.CPDELIVERYADDR,
	                                        ST.CPPHONE,
	                                        ST.CPONLINESTORE,
	                                        ST.CPBUYERADDR,
	                                        ST.CPISONLINESTORE,
	                                        CONVERT(VARCHAR, ST.CREATEDDATETIME, 101) AS CREATEDDATE,
	                                        ST.DATAAREAID,
	                                        (SELECT TOP 1 ADDRESS FROM LOGISTICSPOSTALADDRESS LOG WHERE LOG.LOCATION = DIR.PRIMARYADDRESSLOCATION ORDER BY VALIDFROM DESC) AS ADDRESS,
                                            DIR.NAME
                                        FROM
	                                        SALESTABLE ST
                                            INNER JOIN CUSTPACKINGSLIPJOUR CP ON ST.SALESID = CP.SALESID AND ST.DATAAREAID = CP.DATAAREAID AND ST.INVENTLOCATIONID = CP.INVENTLOCATIONID
	                                        INNER JOIN CUSTTABLE CT ON ST.CUSTACCOUNT = CT.ACCOUNTNUM
	                                        LEFT JOIN DIRPARTYTABLE DIR ON CT.PARTY = DIR.RECID
                                        WHERE
	                                        ST.SALESID = @SALESID
	                                        AND ST.INVENTLOCATIONID LIKE '%" + ApplicationSettings.Database.StoreID + @"%'
	                                        AND ST.DATAAREAID = @DATAAREAID
                                        ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@SALESID", orderId);
                    command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //each line use offset = 13
                            is_data = 1;

                            //Header

                            //format += Environment.NewLine;
                            //format += Environment.NewLine;
                            format += blank_space + "PT PRIMAFOOD INTERNATIONAL" + Environment.NewLine;
                            format += blank_space + "CABANG : " + ApplicationSettings.Terminal.StoreName + Environment.NewLine;
                            format += blank_space + "NPWP : " + "" + Environment.NewLine;
                            format += blank_space + "=======================================================" + Environment.NewLine;
                            format += blank_space + "Receipt Packing Slip" + Environment.NewLine;
                            format += blank_space + "Delivery Order " + title + Environment.NewLine;
                            format += blank_space + "=======================================================" + Environment.NewLine;
                            format += Environment.NewLine;

                            format += blank_space + "Sales ID".PadRight(20) + " : " + orderId + Environment.NewLine;
                            format += blank_space + "Created Date".PadRight(20) + " : " + reader.GetString(reader.GetOrdinal("CREATEDDATE")) + Environment.NewLine;
                            format += blank_space + "Requested Ship Date".PadRight(20) + " : " + reader.GetString(reader.GetOrdinal("SHIPPINGDATEREQUESTED")) + Environment.NewLine;
                            format += blank_space + "Delivery Date".PadRight(20) + " : " + reader.GetString(reader.GetOrdinal("DELIVERYDATE")) + Environment.NewLine + Environment.NewLine;

                            format += blank_space + "Ship To " + Environment.NewLine;
                            format += blank_space + "-------" + Environment.NewLine;

                            string full_address = "";
                            int address_length = 0;

                            if (reader["CPONLINESTORE"].ToString() != "0")
                            {
                                format += blank_space + "Account : " + reader.GetString(reader.GetOrdinal("CPONLINESTOREID")) + Environment.NewLine;
                                format += blank_space + "Nama Pembeli : " + reader.GetString(reader.GetOrdinal("CPBUYERNAME"));
                                format += "(" + reader.GetString(reader.GetOrdinal("CPPHONE")) + ")" + Environment.NewLine;
                                format += blank_space + "Info : " + reader.GetString(reader.GetOrdinal("CPORDERNUMBER")) + Environment.NewLine;
                                //format += blank_space + "Alamat Kirim : " + reader.GetString(reader.GetOrdinal("CPDELIVERYADDR")) + Environment.NewLine;
                                full_address = reader.GetString(reader.GetOrdinal("CPDELIVERYADDR"));
                                address_length = reader.GetString(reader.GetOrdinal("CPDELIVERYADDR")).Length;
                            }
                            else
                            {
                                full_address = reader.GetString(reader.GetOrdinal("ADDRESS"));
                                address_length = reader.GetString(reader.GetOrdinal("ADDRESS")).Length;
                                format += blank_space + "Nama Pembeli : " + reader.GetString(reader.GetOrdinal("NAME")) + Environment.NewLine;
                            }

                            int start_index = 0;
                            full_address = Regex.Replace(full_address, @"\r\n?|\n", " ");

                            if (address_length <= 41)
                            {
                                format += blank_space + "Alamat Kirim : " + full_address + Environment.NewLine;
                            }
                            else
                            {
                                format += blank_space + "Alamat Kirim : " + full_address.Substring(start_index, 41) + Environment.NewLine;
                                address_length -= 41;
                                start_index += 41;

                                while (address_length > 0)
                                {
                                    if (address_length <= 54)
                                    {
                                        format += blank_space + full_address.Substring(start_index) + Environment.NewLine;
                                    }
                                    else
                                    {
                                        format += blank_space + full_address.Substring(start_index, 54) + Environment.NewLine;
                                    }

                                    address_length -= 54;
                                    start_index += 54;
                                    offset += 13;
                                }
                            }

                            format += Environment.NewLine;

                            offset += 182;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Format Error", ex);
            }

            int TotalQty = 0;

            if (is_data != 0)
            {
                // Data Exists, proceed to generate details
                // Generate Details Label
                string labelItemID = "ID";
                string labelItemName = "Name";
                string labelQty = "Qty";
                string labelPrice = "Prc";
                string labelDiscPercent = "Dsc%";
                string labelDiscAmount = "DscAmt";
                string labelSubTotal = "SubTtl";

                format += blank_space +
                    labelItemID.PadRight(9) +
                    labelItemName.PadRight(15) +
                    labelQty.PadLeft(4) +
                    labelPrice.PadLeft(6) +
                    labelDiscPercent.PadLeft(5) +
                    labelDiscAmount.PadLeft(7) +
                    labelSubTotal.PadLeft(8) +
                    Environment.NewLine;

                format += single_line_break + Environment.NewLine;

                // Generate Details Data
                TotalQty = CPGenerateDetails(ref offset, orderId, ref format, blank_space);
            }

            //Footer
            format += Environment.NewLine + double_line_break + Environment.NewLine;

            CPGenerateFooter(orderId, ref format, blank_space, TotalQty);

            format += double_line_break + Environment.NewLine;

            format += Environment.NewLine;
            format += blank_space + "Created,".PadRight(40) + "Receive," + Environment.NewLine;
            for (int i = 0; i < 6; i++)
            {
                format += Environment.NewLine;
            }

            format += blank_space + "PrimaFreshmart".PadRight(40) + "Customer" + Environment.NewLine;

            offset += 143;

            //offset plus 120 at the end of document
            offset += 120;

            return format;
        }
    }
}
