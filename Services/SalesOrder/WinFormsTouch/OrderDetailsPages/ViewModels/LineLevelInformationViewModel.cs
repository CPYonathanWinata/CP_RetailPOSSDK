/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LSRetailPosis;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Tax = LSRetailPosis.Transaction.Line.Tax;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class LineLevelInformationViewModel : INotifyPropertyChanged
    {
        #region Members

        private CustomerOrderTransaction transaction;
        private SaleLineItem saleLineItem;
        private bool isShipping;
        private bool isPickup;
        private string lineItemId;
        private string description;
        private decimal quantity;
        private IAddress shippingAddress;
        private string shippingMethodCode;
        private decimal? shippingCharge;
        private Store deliveryStore;
        private DateTime? deliveryDate;
        private CustomerOrderStatusFormatter orderStatusFormatter = new CustomerOrderStatusFormatter();

        #endregion

        /// <summary>
        /// Creates a <see cref="LineLevelInformationViewModel" object/>
        /// </summary>
        /// <param name="saleLineItem">Reference to the line item. It will be modified on Commit.</param>
        public LineLevelInformationViewModel(SaleLineItem saleLineItem, CustomerOrderTransaction custTransaction)
        {
            this.transaction = custTransaction;

            StoreDataManager storeDataManager = new StoreDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            this.deliveryStore = storeDataManager.GetStore(saleLineItem.DeliveryStoreNumber);

            this.saleLineItem = saleLineItem;
            this.LineItemId = saleLineItem.ItemId;
            this.Description = saleLineItem.Description;
            this.Quantity = saleLineItem.Quantity;
            this.DeliveryDate = saleLineItem.DeliveryDate;
            this.ShippingAddress = saleLineItem.ShippingAddress;
            this.ShippingMethodCode = saleLineItem.DeliveryMode != null ? saleLineItem.DeliveryMode.Code : string.Empty;

            // Since there is a difference between no shipping charge and a shipping charge of zero, we need to see if we have any shipping charges before summing them
            IEnumerable<Tax.MiscellaneousCharge> shippingCharges = saleLineItem.MiscellaneousCharges.Where(c => c.ChargeCode == ApplicationSettings.Terminal.ShippingChargeCode);
            if (shippingCharges.Any())
            {
                this.ShippingCharge = shippingCharges.Sum(c => c.Price);
            }

            // Check for delivery mode on line first, then header
            IDeliveryMode deliveryMode = saleLineItem.DeliveryMode ?? custTransaction.DeliveryMode;
            bool hasPickupCode = deliveryMode != null && string.Equals(deliveryMode.Code, ApplicationSettings.Terminal.PickupDeliveryModeCode, StringComparison.OrdinalIgnoreCase);
            bool hasDeliveryDate = saleLineItem.DeliveryDate.HasValue;

            // we have to do this because if hasPickupCode == false, that does not mean that IsShipping will be set to true
            this.IsPickup = hasPickupCode && hasDeliveryDate;
            this.IsShipping = !hasPickupCode && hasDeliveryDate;
        }

        #region Properties

        /// <summary>
        /// SaleLineItem associated with this view model
        /// </summary>
        public SaleLineItem LineItem
        {
            get { return this.saleLineItem; }
        }

        /// <summary>
        /// Gets the line item id.
        /// </summary>
        public string LineItemId
        {
            get { return this.lineItemId; }
            private set { this.lineItemId = value; }
        }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    OnPropertyChanged("Description");
                    OnPropertyChanged("LineItemOverview");
                }
            }
        }

        /// <summary>
        /// Gets the line item overview.
        /// </summary>
        public string ItemOverview
        {
            get
            {
                return ApplicationLocalizer.Language.Translate(56407, this.LineItemId, this.Description); // "{0} - {1}"
            }
        }

        /// <summary>
        /// Gets or sets the item quantity.
        /// </summary>
        public decimal Quantity
        {
            get { return this.quantity; }
            set
            {
                if (value != this.quantity)
                {
                    this.quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        /// <summary>
        /// Gets the formatted status of the line.
        /// </summary>
        public string FormattedLineStatus
        {
            get
            {
                return this.orderStatusFormatter.Format(null, this.LineItem.LineStatus, null);
            }
        }

        /// <summary>
        /// Gets the formatted status of the line.
        /// </summary>
        public string FormattedDeliveryMethod
        {
            get
            {
                string displayString = null;

                if (this.IsPickup)
                {
                    displayString = string.Format(
                        ApplicationLocalizer.Language.Translate(56408), // "{0}: {1}"
                        this.StoreNumber,
                        this.StoreName);
                }
                else if (this.IsShipping)
                {
                    if (this.LineItem.DeliveryMode != null)
                    {
                        if ((this.ShippingCharge.HasValue && this.ShippingCharge > 0))
                        {
                            displayString = string.Format(
                                  ApplicationLocalizer.Language.Translate(56408), // "{0} - {1}"
                                  this.LineItem.DeliveryMode.DescriptionText,
                                  SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(this.ShippingCharge.Value, true, false).ToString());
                        }
                        else
                        {
                            displayString = this.LineItem.DeliveryMode.DescriptionText;
                        }
                    }

                    if (this.ShippingAddress != null)
                    {
                        displayString = string.Format(
                            ApplicationLocalizer.Language.Translate(56408), // "{0} - {1}"
                            displayString,
                            this.shippingAddress.LineAddress);
                    }

                    if (this.DeliveryDate.HasValue)
                    {
                        displayString = string.Format(
                            ApplicationLocalizer.Language.Translate(56408), // "{0} - {1}"
                            displayString,
                            this.DeliveryDate.Value.ToShortDateString());
                    }
                }

                return displayString;
            }
        }

        /// <summary>
        /// Gets ir sets the value whether this is an item to be shipped.
        /// </summary>
        public bool IsShipping
        {
            get { return isShipping; }
            set
            {
                if (value != this.isShipping)
                {
                    if (value)
                    {
                        this.IsPickup = false;
                    }

                    this.isShipping = value;
                    OnPropertyChanged("IsShipping");
                    OnPropertyChanged("FormattedDeliveryMethod");
                }
            }
        }

        /// <summary>
        /// Gets or sets the shipping address.
        /// </summary>
        public IAddress ShippingAddress
        {
            get { return this.shippingAddress; }
            set
            {
                if (value != this.shippingAddress)
                {
                    this.shippingAddress = value;
                    OnPropertyChanged("ShippingAddress");
                }
            }
        }

        /// <summary>
        /// Gets or sets the shipping method.
        /// </summary>
        public string ShippingMethodCode
        {
            get { return this.shippingMethodCode; }
            set
            {
                if (value != this.shippingMethodCode)
                {
                    this.shippingMethodCode = value;
                    OnPropertyChanged("ShippingMethod");
                    OnPropertyChanged("FormattedDeliveryMethod");
                }
            }
        }

        /// <summary>
        /// Gets or sets the shipping charge.
        /// </summary>
        public decimal? ShippingCharge
        {
            get { return this.shippingCharge; }
            set
            {
                if (value != this.shippingCharge)
                {
                    this.shippingCharge = value;
                    OnPropertyChanged("ShippingCharge");
                    OnPropertyChanged("FormattedDeliveryMethod");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value whether this is an item to be picked up.
        /// </summary>
        public bool IsPickup
        {
            get { return isPickup; }
            set
            {
                if (value != this.isPickup)
                {
                    if (value)
                    {
                        this.IsShipping = false;
                    }

                    this.isPickup = value;
                    OnPropertyChanged("IsPickup");
                    OnPropertyChanged("FormattedDeliveryMethod");
                }
            }
        }

        /// <summary>
        /// Indicates the selected store, null if nothing is selected.
        /// </summary>
        public DataEntity.Store DeliveryStore
        {
            get { return deliveryStore; }
            set
            {
                if (deliveryStore != value)
                {
                    deliveryStore = value;
                    OnPropertyChanged("DeliveryStore");
                    OnPropertyChanged("StoreNumber");
                    OnPropertyChanged("StoreName");
                    OnPropertyChanged("FormattedDeliveryMethod");
                }
            }
        }

        /// <summary>
        /// Gets the store number of the store
        /// </summary>
        public string StoreNumber
        {
            get
            {
                return (this.DeliveryStore == null) ? null : this.DeliveryStore.Number;
            }
        }

        /// <summary>
        /// Gets the store name of the store
        /// </summary>
        public string StoreName
        {
            get
            {
                return (this.DeliveryStore == null) ? null : this.DeliveryStore.Name;
            }
        }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.deliveryDate; }
            set
            {
                if (value != this.deliveryDate)
                {
                    this.deliveryDate = value;
                    OnPropertyChanged("DeliveryDate");
                }
            }
        }

        #endregion

        /// <summary>
        /// Commits all changes to the line item.
        /// </summary>
        public void Commit()
        {
            string storeTaxGroup = string.Empty;
            
            StoreDataManager storeDataManager = new StoreDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            //In case of mixed delivery mode, set transaction RequestedDeliveryDate to the earliest date of all line items in customer order instead of an arbitrary value
            if (this.DeliveryDate.HasValue && DateTime.Compare(this.transaction.RequestedDeliveryDate, this.DeliveryDate.Value) > 0)
            {
                this.transaction.RequestedDeliveryDate = this.DeliveryDate.Value;
            }
            else
            {
                if (this.transaction.RequestedDeliveryDate == DateTime.MinValue)
                {
                    this.transaction.RequestedDeliveryDate = DateTime.Now.Date;
                }
            }

            this.saleLineItem.DeliveryDate = this.DeliveryDate;
            this.saleLineItem.Quantity = this.Quantity;

            this.saleLineItem.DeliveryStoreNumber = this.StoreNumber;

            if (this.IsShipping)
            {
                // if this line is shipping, then use the stores shipping warehouse setting
                this.saleLineItem.DeliveryWarehouse = ApplicationSettings.Terminal.InventLocationIdForCustomerOrder;
                this.saleLineItem.DeliveryStoreNumber = ApplicationSettings.Terminal.StoreId;

                this.saleLineItem.ShippingAddress = this.ShippingAddress;

                this.saleLineItem.DeliveryMode = storeDataManager.GetDeliveryMode(this.ShippingMethodCode);
            }
            else if (this.IsPickup)
            {
                if (string.IsNullOrWhiteSpace(ApplicationSettings.Terminal.PickupDeliveryModeCode))
                {
                    // "Pickup cannot be used for delivery because a pickup delivery code was not found."
                    string errorMessage = LSRetailPosis.ApplicationLocalizer.Language.Translate(56382);
                    SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);

                    throw new InvalidOperationException(errorMessage);
                }

                // if the delivery warehouse is not set, default to the store's warehouse
                this.saleLineItem.DeliveryWarehouse = (this.deliveryStore != null) ? this.deliveryStore.Warehouse : ApplicationSettings.Terminal.InventLocationId;
                this.saleLineItem.DeliveryMode = storeDataManager.GetDeliveryMode(ApplicationSettings.Terminal.PickupDeliveryModeCode);
                this.saleLineItem.ShippingAddress = this.shippingAddress;
            }

            // Set the proper SalesTaxGroup based on the store/customer settings
            SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(this.saleLineItem);

            // Adds or removes shipping charge
            CommitShippingCharge(this.saleLineItem, this.transaction, this.ShippingCharge);
        }

        private static void CommitShippingCharge(SaleLineItem lineItem, CustomerOrderTransaction trans, decimal? charge)
        {
            // First check if a shipping charge code was configured on the back end and exists in the DB
            string shippingChargeCode = LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode;

            if (charge.HasValue && string.IsNullOrWhiteSpace(shippingChargeCode))
            {
                // "A shipping charge cannot be added for item: {0} because a shipping charge code was not found."
                string errorMessage = string.Format(LSRetailPosis.ApplicationLocalizer.Language.Translate(56301), lineItem.ItemId);
                SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);

                throw new InvalidOperationException(errorMessage);
            }

            // Shipping charge
            Tax.MiscellaneousCharge mc = lineItem.MiscellaneousCharges.FirstOrDefault(m => m.ChargeCode == shippingChargeCode);

            // See if there's an existing charge
            if (mc == null)
            {
                if (charge.HasValue)
                {
                    // No charge exists, so we add it

                    // Get Cancellation charge properties from DB
                    StoreDataManager store = new StoreDataManager(
                        SalesOrder.InternalApplication.Settings.Database.Connection,
                        SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                    DataEntity.MiscellaneousCharge chargeProperties = store.GetMiscellaneousCharge(shippingChargeCode);

                    if (chargeProperties != null)
                    {
                        mc = new Tax.MiscellaneousCharge(
                            charge.Value,
                            trans.Customer.SalesTaxGroup,
                            chargeProperties.TaxItemGroup,
                            chargeProperties.MarkupCode,
                            string.Empty,
                            trans);

                        lineItem.MiscellaneousCharges.Add(mc);

                        // Set the proper SalesTaxGroup based on the store/customer settings
                        // Updating sales line will update charge line
                        SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(lineItem);
                    }
                    else
                    {
                        // "A shipping charge cannot be added for item: {0} because no information was found for shipping charge code: {1}."
                        string errorMessage = string.Format(LSRetailPosis.ApplicationLocalizer.Language.Translate(56303), lineItem.ItemId, shippingChargeCode);
                        SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);

                        throw new InvalidOperationException(errorMessage);
                    }
                }
            }
            else // Charge exists
            {
                if (charge.HasValue)
                {
                    if (mc.Amount != charge.Value)
                    {
                        // Update amount
                        mc.Amount = charge.Value;
                    }
                }
                else
                {
                    // No charge, so remove
                    lineItem.MiscellaneousCharges.Remove(mc);
                }
            }

            // Remove any header level charge
            var headerCharge = trans.MiscellaneousCharges.FirstOrDefault(m => m.ChargeCode == shippingChargeCode);
            if (headerCharge != null)
            {
                trans.MiscellaneousCharges.Remove(headerCharge);
            }

            // Trigger new price,tax and discount for the customer (do NOT reset item prices because we should preserve any existing Price Overrides)
            //SalesOrder.InternalApplication.BusinessLogic.ItemSystem.RecalcPriceTaxDiscount(trans, false); //disable test yonathan 22112024 #duplicate
            trans.CalcTotals();
        }

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
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
