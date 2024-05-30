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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class ItemDetailsViewModel : PageViewModel
    {
        private ReadOnlyCollection<LineItemViewModel> lineItems;
        private List<LineItemViewModel> viewModels;

        public ItemDetailsViewModel(CustomerOrderTransaction customerOrderTransaction)
        {
            this.SetTransaction(customerOrderTransaction);

            // Create a collection of LineItemViewModels from each SaleLineItem
            viewModels = (from lineItem in this.Transaction.SaleItems.Where(i => !i.Voided)
                          select new LineItemViewModel(lineItem)).ToList<LineItemViewModel>();

            this.lineItems = new ReadOnlyCollection<LineItemViewModel>(viewModels);
        }

        public ReadOnlyCollection<LineItemViewModel> Items
        {
            get { return lineItems; }
        }

        /// <summary>
        /// Indicates whether any of the line items are to be picked up from the current store
        /// </summary>
        public bool AnyItemsForCurrentStore
        {
            get
            {
                return 
                    (this.Transaction.Mode == CustomerOrderMode.Pickup)
                    && this.Items.Any(vm => vm.IsPickupAtStore(ApplicationSettings.Terminal.StoreId));
            }
        }

        public void ExecutePickupAllCommand()
        {
            foreach (LineItemViewModel vm in this.Items)
            {
                vm.ExecutePickupAllCommand();
            }

            OnPropertyChanged("Items");
        }

        public void SetPickupQuantity(string itemId, int lineId, decimal qty)
        {
            LineItemViewModel item = this.Items.SingleOrDefault(i => (i.ItemId == itemId) && (i.LineId == lineId));
            if (item != null && item.IsPickupAtStore(ApplicationSettings.Terminal.StoreId))
            {
                item.PickupQuantity = qty;
            }
        }

        public void CommitChanges()
        {
            this.viewModels.ForEach(vm => vm.CommitPickupQuantity());

            // If we commit here, force into Pickup mode regardless of the actual quantities specified
            // (this means that we can be in Pickup mode even though the 'pick up quantity' is zero
            this.Transaction.Mode = CustomerOrderMode.Pickup;
        }
    }

    /// <summary>
    /// View Model encapsulating an individual Line Item on the Customer Order
    /// </summary>
    class LineItemViewModel
    {
        private const string variantSeparator = "{0} - {1}";
        private string moreInfo = null;

        private SaleLineItem lineItem;

        public LineItemViewModel(SaleLineItem item)
        {
            lineItem = item;

            // Pre-fill the PickupQuantity for items that can be picked up at the current store.
            if (IsPickupAtStore(ApplicationSettings.Terminal.StoreId))
            {
                //calculating the remaining items
                this.PickupQuantity = this.QuantityRemaining;
            }

            if (((CustomerOrderTransaction)this.lineItem.Transaction).Mode == CustomerOrderMode.Pickup)
            {
                //Initialize the line item quantity to the default pickup quantity (only if in Pickup mode)
                lineItem.Quantity = this.PickupQuantity;
            }
        }

        public int LineId
        {
            get { return lineItem.LineId; }
        }

        public string ItemId
        {
            get { return lineItem.ItemId; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by databinding")]
        public string Description
        {
            get { return lineItem.Description; }
        }

        public decimal QuantityOrdered
        {
            get { return lineItem.QuantityOrdered; }
        }

        public decimal QuantityPreviouslyPickedUp
        {
            get { return lineItem.QuantityPickedUp; }
        }

        public decimal QuantityRemaining
        {
            get
            {
                decimal remaining = this.QuantityOrdered - this.QuantityPreviouslyPickedUp;
                if (remaining < 0)
                    remaining = 0;

                return remaining;
            }
        }

        /// <summary>
        /// Line item quantity
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by data binding")]
        public decimal Quantity
        {
            get { return lineItem.Quantity; }
        }

        /// <summary>
        /// Quantity being picked up
        /// </summary>
        public decimal PickupQuantity
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by data binding")]
        public string FormattedDeliveryMethod
        {
            get
            {
                string displayText = null;

                if (lineItem.DeliveryMode != null)
                {
                    string pickupCode = ApplicationSettings.Terminal.PickupDeliveryModeCode;

                    // If pick up...
                    if (string.Equals(lineItem.DeliveryMode.Code, pickupCode, StringComparison.Ordinal))
                    {
                        if (!string.IsNullOrWhiteSpace(lineItem.DeliveryStoreNumber))
                        {
                            displayText = string.Format(
                                LSRetailPosis.ApplicationLocalizer.Language.Translate(56306), // "{0} : {1}"
                                lineItem.DeliveryMode.Code,
                                lineItem.DeliveryStoreNumber);
                        }
                    }
                    else
                    {
                        // Else shipping...

                        string shippingChargeCode = LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode;

                        // Look for a shipping charge
                        var miscCharge = lineItem.MiscellaneousCharges.FirstOrDefault(m => string.Equals(m.ChargeCode, shippingChargeCode, StringComparison.Ordinal));
                        if (miscCharge != null)
                        {
                            displayText = string.Format(
                                LSRetailPosis.ApplicationLocalizer.Language.Translate(56305), // "{0} : {1}"
                                lineItem.DeliveryMode.Code,
                                SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(miscCharge.Amount, true, false));
                        }
                        else
                        {
                            displayText = lineItem.DeliveryMode.Code;
                        }
                    }
                }

                return displayText;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by data binding")]
        public string DeliveryDate
        {
            get
            {
                string formattedDate = null;

                if (lineItem.DeliveryDate.HasValue && lineItem.DeliveryDate.Value > DateTime.MinValue)
                {
                    formattedDate = lineItem.DeliveryDate.Value.ToShortDateString();
                }

                return formattedDate;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by data binding")]
        public string MoreInfo
        {
            get
            {
                // If null then this is first time.
                // Else if empty then this was done before.
                if (moreInfo == null)
                {
                    List<string> dims = new List<string>(4);
                    string colorName = lineItem.Dimension.ColorName ?? lineItem.Dimension.ColorId;
                    string sizeName = lineItem.Dimension.SizeName ?? lineItem.Dimension.SizeId;
                    string styleName = lineItem.Dimension.StyleName ?? lineItem.Dimension.StyleId;
                    string configName = lineItem.Dimension.ConfigName ?? lineItem.Dimension.ConfigId;

                    if (!string.IsNullOrWhiteSpace(colorName))
                    {
                        dims.Add(colorName);
                    }

                    if (!string.IsNullOrWhiteSpace(sizeName))
                    {
                        dims.Add(sizeName);
                    }

                    if (!string.IsNullOrWhiteSpace(styleName))
                    {
                        dims.Add(styleName);
                    }

                    if (!string.IsNullOrWhiteSpace(configName))
                    {
                        dims.Add(configName);
                    }

                    if (dims.Count > 0)
                    {
                        moreInfo = dims.Aggregate(((output, next) => string.Format(variantSeparator, output, next)));
                    }
                    else
                    {
                        // Set to epty to avoid repeat.
                        moreInfo = string.Empty;
                    }
                }

                return moreInfo;
            }
        }

        /// <summary>
        /// Verifies if this item is supposed to be picked up at the given store.
        /// </summary>
        /// <param name="storeNumber">The store number.</param>
        /// <returns>True if the item is supposed to be picked at the given store, false, otherwise.</returns>
        public bool IsPickupAtStore(string storeNumber)
        {
            string deliveryModeCode = null;
            string deliveryStoreNumber = null;
            string pickupDeliveryModeCode = ApplicationSettings.Terminal.PickupDeliveryModeCode;

            if (!string.IsNullOrEmpty(storeNumber))
            {
                if (this.lineItem.DeliveryMode != null)
                {
                    deliveryModeCode = this.lineItem.DeliveryMode.Code;
                    deliveryStoreNumber = this.lineItem.DeliveryStoreNumber;
                }
                else if (this.lineItem.Transaction != null && this.lineItem.Transaction.DeliveryMode != null)
                {
                    deliveryModeCode = this.lineItem.Transaction.DeliveryMode.Code;
                    deliveryStoreNumber = this.lineItem.Transaction.StoreId;
                }

                return 
                    ((CustomerOrderTransaction)this.lineItem.Transaction).Mode == CustomerOrderMode.Pickup
                    && string.Equals(deliveryModeCode, ApplicationSettings.Terminal.PickupDeliveryModeCode, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(deliveryStoreNumber, storeNumber, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the quantity is allowed for the specific unitOfMeasure.
        /// E.g. it is illegal to sell 4,6 units of coke but allright for metric goods.
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool IsQtyAllowed(decimal qty)
        {
            bool retVal = true;

            try
            {
                // If the unit of measure is not defined then we cannot determine if the qty is allowed.
                if (string.IsNullOrEmpty(this.lineItem.SalesOrderUnitOfMeasure))
                {
                    return true;
                }

                decimal modResult = qty % 1m;
                if (modResult == 0)
                {
                    // Qty is a whole unit, e.g. 2,00
                    // We can always sell whole units, so this is allowed
                    retVal = true;
                    return retVal;
                }
                else
                {
                    // Qty is a broken unit, e.g. 5,70

                    UnitOfMeasureData unitOfMeasureData = new UnitOfMeasureData(
                        SalesOrder.InternalApplication.Settings.Database.Connection,
                        SalesOrder.InternalApplication.Settings.Database.DataAreaID,
                        LSRetailPosis.Settings.ApplicationSettings.Terminal.StorePrimaryId,
                        SalesOrder.InternalApplication);
                    int numberOfUnitDecimals = unitOfMeasureData.GetUnitDecimals(this.lineItem.SalesOrderUnitOfMeasure);

                    // Find the correct comma char for the location.
                    string commaChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                    string tempStr = Convert.ToString(qty);
                    int commaIndex = (tempStr.IndexOf(commaChar) + 1); // the first numbers plus decimal point

                    int qtyDecimals = 0;
                    if (commaIndex > 0)
                    {
                        qtyDecimals = ((tempStr.Length) - commaIndex);     //total length minus beginning numbers and decimal = number of decimal points 
                    }

                    if (numberOfUnitDecimals < qtyDecimals)
                    {
                        retVal = false;
                    }
                }
            }
            catch (Exception)
            {
                LSRetailPosis.ApplicationLog.Log(this.ToString(), "Could not determine if the qty is allowed for the item.", LSRetailPosis.LogTraceLevel.Trace);

                // We cannot retrieve the correct number of decimals from the database, for this unit of measure
                // Therefore we cannot determine if the qty is legal and have to allow it.
                retVal = true;
            }

            return retVal;
        }


        /// <summary>
        /// Currency formatted net amount
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by data binding")]
        public string FormattedNetAmount
        {
            get { return SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(lineItem.NetAmount, true, false); }
        }

        public void ExecutePickupAllCommand()
        {
            if (this.IsPickupAtStore(ApplicationSettings.Terminal.StoreId))
            {
                this.PickupQuantity = this.QuantityRemaining;
            }
        }

        public void CommitPickupQuantity()
        {
            lineItem.Quantity = this.PickupQuantity;

            // During recall/pickup, QuantityDiscounted is directly linked to Quantity.  See SalesOrder\SerializationHelper.GetTransactionFromInfo(...)
            if (lineItem.DiscountLines.Count > 0)
            {
                lineItem.QuantityDiscounted = this.PickupQuantity;
            }
        }
    }
}