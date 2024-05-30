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

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    internal static class SalesOrderActions
    {
        private static string LogSource = typeof(SalesOrderActions).ToString();

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
                if (SalesOrder.InternalApplication.Services.LogOn.VerifyOperationAccess(SalesOrder.InternalApplication.Shift.StaffId, PosisOperations.CustomerOrderDetails)
                    && SalesOrder.InternalApplication.Services.LogOn.VerifyOperationAccess(SalesOrder.InternalApplication.Shift.StaffId, PosisOperations.PackSlip))
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
                        SalesOrder.InternalApplication.Services.Printing.PrintPackSlip(transaction);
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

            using (formOrderDetails frm = new formOrderDetails(cot, selectionMode))
            {
                POSFormsManager.ShowPOSForm(frm);
                DialogResult result = frm.DialogResult;

                // Get updated transaction since nested operations might have been run
                transaction = frm.Transaction;

                if (result == DialogResult.OK)
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

    }
}
