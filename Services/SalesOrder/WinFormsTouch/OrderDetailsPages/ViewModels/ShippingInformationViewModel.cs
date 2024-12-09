/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using Tax = LSRetailPosis.Transaction.Line.Tax;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{    
    sealed class ShippingInformationViewModel : INotifyPropertyChanged
    {
        private DM.StoreDataManager storeDataManager;
        private CustomerOrderTransaction transaction;
        private SaleLineItem lineItem;
        private DateTime? deliveryDate;        
        private IAddress shippingAddress;
        private decimal? shippingCharge;
        private ReadOnlyCollection<DeliveryModeExploded> deliveryModes;
        private DeliveryModeExploded selectedShippingMethod;
        private bool isDeliveryChangeAllowed;
        private bool validated;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="custTransaction">Transaction for this view model</param>
        /// <param name="saleLineItem">Sale line item for this view model if applicable</param>
        /// <remarks>If saleLineItem is not null then view model wraps line not transaction</remarks>
        public ShippingInformationViewModel(CustomerOrderTransaction custTransaction, SaleLineItem saleLineItem)
        {
            this.transaction     = custTransaction;
            this.lineItem        = saleLineItem;
            this.ShippingAddress = (saleLineItem == null) ? custTransaction.ShippingAddress : saleLineItem.ShippingAddress;

            // Load delivery date and set DeliveryDate property so validation is checked
            if (saleLineItem == null)
            {
                if (custTransaction.RequestedDeliveryDate > DateTime.MinValue)
                    this.DeliveryDate = custTransaction.RequestedDeliveryDate;
            }
            else
            {
                if (saleLineItem.DeliveryDate.HasValue && saleLineItem.DeliveryDate.Value > DateTime.MinValue)
                    this.DeliveryDate = saleLineItem.DeliveryDate.Value;
            }

            // Load delivery charge            
            string shippingChargeCode  = LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode;            
            Tax.MiscellaneousCharge mc = (saleLineItem == null)
                ? custTransaction.MiscellaneousCharges.SingleOrDefault(m => m.ChargeCode == shippingChargeCode)
                : lineItem.MiscellaneousCharges.SingleOrDefault(m => m.ChargeCode == shippingChargeCode);

            if (mc != null)
                this.shippingCharge = mc.Price;

            // Get list of addresses
            storeDataManager = new DM.StoreDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            // Create a read-only collection
            GetValidDeliveryModes();
            //deliveryModes = new ReadOnlyCollection<DeliveryModeExploded>(storeDataManager.GetDeliveryModes());

            // Load delivery method and set ShippingMethod property so validation is checked
            //this.ShippingMethod = (DeliveryMode)((saleLineItem == null) ? custTransaction.DeliveryMode : saleLineItem.DeliveryMode);
            this.ShippingMethod = new DeliveryModeExploded((DeliveryMode)((saleLineItem == null) ? custTransaction.DeliveryMode : saleLineItem.DeliveryMode));
        }

        private void GetValidDeliveryModes()
        {
            if (this.lineItem != null)
            {
                //Delivery modes for a single line
                deliveryModes = DeliveryModesForItem(this.lineItem);
            }
            else
            {
                //Delivery modes for the whole transaction
                ReadOnlyCollection<DataEntity.DeliveryModeExploded> commonDeliveryModes = null;

                foreach (var line in this.transaction.SaleItems)
                {
                    if (commonDeliveryModes == null)
                    {
                        //Start with the first line's modes
                        commonDeliveryModes = DeliveryModesForItem(line);
                    }
                    else
                    {
                        //Only keep the intersection of each additional line's delivery modes
                        commonDeliveryModes.Intersect(DeliveryModesForItem(line));
                    }
                }

                // Return the common delivery modes, or an empty list.
                deliveryModes = commonDeliveryModes ?? new ReadOnlyCollection<DataEntity.DeliveryModeExploded>(new Collection<DataEntity.DeliveryModeExploded>());
            }            
        }

        private ReadOnlyCollection<DeliveryModeExploded> DeliveryModesForItem(SaleLineItem item)
        {
            // Set address fields if available
            string country = string.Empty;
            string state = string.Empty;

            if (this.ShippingAddress != null)
            {
                country = this.ShippingAddress.Country;
                state = this.ShippingAddress.State;
            }

            return new ReadOnlyCollection<DataEntity.DeliveryModeExploded>(
                storeDataManager.GetDeliveryModes(
                    ApplicationSettings.Terminal.StorePrimaryId,
                    country,
                    state,
                    item.ItemId,
                    item.DimensionGroupId));
        }

        /// <summary>
        /// Customer order transaction
        /// </summary>
        public CustomerOrderTransaction Transaction
        {
            get { return this.transaction; }            
        }

        /// <summary>
        /// Indicates the selected shipping method, null if nothing is selected.
        /// </summary>
        public DeliveryModeExploded ShippingMethod
        {
            get { return selectedShippingMethod; }
            set
            {
                if (selectedShippingMethod != value)
                {
                    selectedShippingMethod = value;
                    OnPropertyChanged("ShippingMethod");

                    Validate();
                }
            }
        }

        /// <summary>
        /// Collection of stores
        /// </summary>
        public ReadOnlyCollection<DeliveryModeExploded> ShippingMethods
        {
            get { return deliveryModes; }
        }        

        /// <summary>
        /// Formatted shipping charge
        /// </summary>
        /// <remarks>Formatted as string for display binding</remarks>
        public string FormattedShippingCharge
        {
            get 
            {                
                return (this.ShippingCharge.HasValue)
                    ? SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(this.ShippingCharge.Value, true, false)
                    : null;
            }
        }
        
        /// <summary>
        /// Shipping charge
        /// </summary>
        public decimal? ShippingCharge
        {
            get { return shippingCharge; }
            set
            {
                if (shippingCharge != value)
                {
                    shippingCharge = value;
                    OnPropertyChanged("ShippingCharge");
                    OnPropertyChanged("FormattedShippingCharge");
                    Validate();
                }
            }
        }

        /// <summary>
        /// Requested delivery date
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                if (deliveryDate != value)
                {
                    deliveryDate = value;
                    OnPropertyChanged("DeliveryDate");

                    Validate();
                }
            }
        }

        public IAddress ShippingAddress
        {
            get { return this.shippingAddress; }
            set
            {
                shippingAddress = value;

                OnPropertyChanged("Name");
                OnPropertyChanged("Street");
                OnPropertyChanged("State");
                OnPropertyChanged("PostalCode");
                OnPropertyChanged("Phone");
                OnPropertyChanged("MobilePhone");
                OnPropertyChanged("ShippingAddress");
                Validate();
            }
        }


        /// <summary>
        /// Indicates if changes to Delivery options are allowed
        /// </summary>
        public bool IsDeliveryChangeAllowed
        {
            get
            {
                return this.isDeliveryChangeAllowed;
            }
            set
            {
                if (this.isDeliveryChangeAllowed != value)
                {
                    this.isDeliveryChangeAllowed = value;
                    OnPropertyChanged("IsDeliveryChangeAllowed");
                }
            }
        }

        public void ExecuteClearCommand()
        {
            this.ShippingAddress = null;
            this.ShippingMethod  = null;
            this.ShippingCharge  = 0m;
            this.DeliveryDate    = null;
            this.GetValidDeliveryModes();
        }

        public void ExecuteShippingAddressSearchCommand()
        {
            this.ShippingAddress = SalesOrder.InternalApplication.Services.Customer.SearchShippingAddress(this.Transaction.Customer);

            ResetValidDeliveryModes();
        }

        private void ResetValidDeliveryModes()
        {
            this.GetValidDeliveryModes();
            if (this.ShippingMethod != null)
            {
                this.ShippingMethod = this.ShippingMethods.FirstOrDefault(s => s.Code == this.ShippingMethod.Code);
            }
        }        

        /// <summary>
        /// Gets or sets whether all the required data has been collected
        /// </summary>
        public bool Validated
        {
            get { return validated; }
            set
            {
                if (validated != value)
                {
                    validated = value;
                    OnPropertyChanged("Validated");
                }
            }
        }

        private void Validate()
        {
            this.Validated = this.DeliveryDate.HasValue 
                && (this.ShippingAddress != null) 
                && (this.ShippingMethod != null) 
                && this.IsDeliveryChangeAllowed;
        }

        /// <summary>
        /// Save changes back to models
        /// </summary>
        public void CommitHeaderChanges()
        {
            SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.SetShippingAddress(transaction, this.ShippingAddress);

            // Delivery date
            this.transaction.RequestedDeliveryDate = this.DeliveryDate.Value;

            // Shipping method
            this.transaction.DeliveryMode = storeDataManager.GetDeliveryMode(this.ShippingMethod.Code);
            
            // Shipping warehouse
            this.transaction.WarehouseId = LSRetailPosis.Settings.ApplicationSettings.Terminal.InventLocationIdForCustomerOrder;

            // Clear any line item delivery options
            foreach (var item in this.transaction.SaleItems)
            {
                ClearLineDeliveryOptions(item);

                item.ShippingAddress = this.transaction.ShippingAddress;
                item.DeliveryMode = this.transaction.DeliveryMode;

                if (this.transaction.RequestedDeliveryDate != default(DateTime))
                {
                    item.DeliveryDate = this.transaction.RequestedDeliveryDate;
                }

                SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(item);
            }

            //
            // Shipping charge
            //            

            // First check if a shipping charge code was configured on the back end and exists in the DB
            string shippingChargeCode = LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode;

            if (this.ShippingCharge.HasValue && string.IsNullOrWhiteSpace(shippingChargeCode))
            {
                // "A shipping charge cannot be added because a shipping charge code was not found."
                string errorMessage = LSRetailPosis.ApplicationLocalizer.Language.Translate(56302);
                SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);

                throw new InvalidOperationException(errorMessage);
            }

            Tax.MiscellaneousCharge mc = this.Transaction.MiscellaneousCharges.SingleOrDefault(m => m.ChargeCode == LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode);

            // See if there's an existing charge
            if (mc == null)
            {
                if (this.ShippingCharge.HasValue)
                {
                    // No charge exists, so we add it

                    // Get Cancellation charge properties from DB
                    DM.StoreDataManager store = new DM.StoreDataManager(
                        SalesOrder.InternalApplication.Settings.Database.Connection,
                        SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                    DataEntity.MiscellaneousCharge chargeProperties = store.GetMiscellaneousCharge(LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode);

                    if (chargeProperties != null)
                    {
                        mc = new Tax.MiscellaneousCharge(
                            this.ShippingCharge.Value,
                            this.transaction.Customer.SalesTaxGroup,
                            chargeProperties.TaxItemGroup,
                            chargeProperties.MarkupCode,
                            string.Empty,
                            this.Transaction);

                        // Set the proper SalesTaxGroup based on the store/customer settings
                        SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(mc);
                        
                        this.Transaction.MiscellaneousCharges.Add(mc);
                    }
                    else
                    {
                        // "A shipping charge cannot be added because no information was found for shipping charge code: {0}."
                        string errorMessage = string.Format(LSRetailPosis.ApplicationLocalizer.Language.Translate(56304), shippingChargeCode);
                        SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);

                        throw new InvalidOperationException(errorMessage);
                    }
                }
            }
            else // Charge exists
            {
                if (this.ShippingCharge.HasValue)
                {
                    if (mc.Amount != this.ShippingCharge.Value)
                    {
                        // Update amount
                        mc.Amount = this.ShippingCharge.Value;                        
                    }
                }
                else
                {
                    // No charge, so remove
                    this.Transaction.MiscellaneousCharges.Remove(mc);                    
                }
            }

            // Trigger new price,tax and discount for the customer
            // We should preserve any existing Price Overrides
            //SalesOrder.InternalApplication.BusinessLogic.ItemSystem.RecalcPriceTaxDiscount(this.Transaction, false);  //disable test yonathan 22112024 #duplicate
            this.Transaction.CalcTotals();   
        }

        private static void ClearLineDeliveryOptions(SaleLineItem item)
        {
            item.DeliveryDate      = null;            
            item.ShippingAddress   = null;
            item.DeliveryMode  = null;
            item.DeliveryStoreNumber = null;
            item.DeliveryWarehouse = null;            

            // Remove any charges
            var charge = item.MiscellaneousCharges.FirstOrDefault(m => string.Equals(m.ChargeCode, LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode, StringComparison.Ordinal));
            if (charge != null)
                item.MiscellaneousCharges.Remove(charge);            
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
            //this.VerifyPropertyName(propertyName);

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