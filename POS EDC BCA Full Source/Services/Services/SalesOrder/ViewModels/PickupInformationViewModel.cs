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
using System.ComponentModel;
using System.Data;
using System.Linq;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    sealed class PickupInformationViewModel : INotifyPropertyChanged
    {
        private DM.StoreDataManager storeDataManager;
        private DM.CustomerDataManager customerDataManager;
        private bool validated;
        private DateTime? pickUpDate;
        private ReadOnlyCollection<DataEntity.Store> stores;
        private DataEntity.Store selectedStore;
        private bool isDeliveryChangeAllowed;
        private CustomerOrderTransaction transaction;

        public PickupInformationViewModel(CustomerOrderTransaction custTransaction)
        {
            this.transaction = custTransaction;

            // Get list of addresses
            storeDataManager = new DM.StoreDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            customerDataManager = new DM.CustomerDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            // Get list of nearby store identifiers
            HashSet<string> nearbyStores = this.GetStoresInStoreLocatorGroup();
            
            // Create a read-only collection
            stores = new ReadOnlyCollection<DataEntity.Store>(
                storeDataManager.GetStoresForPickup(SalesOrder.InternalApplication.Settings.Database.DataAreaID, nearbyStores, true));

            // Load date
            if (custTransaction.RequestedDeliveryDate > DateTime.MinValue)
                this.pickUpDate = custTransaction.RequestedDeliveryDate;

            // Load store
            if (!string.IsNullOrWhiteSpace(custTransaction.WarehouseId))
                this.selectedStore = storeDataManager.GetWarehouseStore(custTransaction.WarehouseId);
            else
                this.selectedStore = storeDataManager.GetStore(LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreId);
        }

        /// <summary>
        /// Customer pick up date
        /// </summary>
        public DateTime? PickupDate
        {
            get { return pickUpDate; }
            set
            {
                if (pickUpDate != value)
                {
                    pickUpDate = value;
                    OnPropertyChanged("PickupDate");

                    Validate();
                }
            }
        }

        /// <summary>
        /// Collection of stores
        /// </summary>
        public ReadOnlyCollection<DataEntity.Store> Stores
        {
            get { return stores; }
        }

        /// <summary>
        /// Indicates the selected store, null if nothing is selected.
        /// </summary>
        public DataEntity.Store SelectedStore
        {
            get { return selectedStore; }
            set
            {
                if (selectedStore != value)
                {
                    selectedStore = value;
                    OnPropertyChanged("SelectedStore");
                    OnPropertyChanged("StoreLocationDescription");

                    Validate();
                }
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


        public string StoreLocationDescription
        {
            get
            {
                string displayText = null;

                if (this.SelectedStore != null)
                {
                    if (!string.IsNullOrWhiteSpace(this.SelectedStore.Name))
                    {
                        displayText = string.Format(
                            LSRetailPosis.ApplicationLocalizer.Language.Translate(56306), // "{0} : {1}"
                            this.SelectedStore.Number,
                            this.SelectedStore.Name);
                    }
                    else
                    {
                        displayText = this.SelectedStore.Number;
                    }
                }

                return displayText;
            }
        }

        /// <summary>
        /// Gets or sets whether all the required data has been collection
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
            this.Validated = this.PickupDate.HasValue
                && (this.SelectedStore != null)
                && this.IsDeliveryChangeAllowed;
        }

        /// <summary>
        /// Save changes back to models
        /// </summary>
        public void CommitHeaderChanges()
        {
            if (this.Validated)
            {
                // First check if a pickup code was configured on the back end and exists in the DB
                string pickupCode = LSRetailPosis.Settings.ApplicationSettings.Terminal.PickupDeliveryModeCode;

                if (string.IsNullOrWhiteSpace(pickupCode))
                {
                    // "Pickup cannot be used for delivery because a pickup delivery code was not found."
                    string errorMessage = LSRetailPosis.ApplicationLocalizer.Language.Translate(56382);
                    SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);

                    throw new InvalidOperationException(errorMessage);
                }

                this.transaction.RequestedDeliveryDate = this.PickupDate.Value;
                this.transaction.StoreId = this.SelectedStore.Number;
                this.transaction.WarehouseId = this.SelectedStore.Warehouse;
                this.transaction.DeliveryMode = storeDataManager.GetDeliveryMode(ApplicationSettings.Terminal.PickupDeliveryModeCode);
                this.transaction.ShippingAddress = GetStoreAddress(this.selectedStore);

                // Remove any header level charge
                var headerCharge = transaction.MiscellaneousCharges.FirstOrDefault(m => string.Equals(m.ChargeCode, LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode, StringComparison.Ordinal));
                if (headerCharge != null)
                {
                    transaction.MiscellaneousCharges.Remove(headerCharge);
                }

                // Clear any line item delivery options and duplicate the pickup store on each line
                foreach (var item in this.transaction.SaleItems)
                {
                    ClearLineDeliveryOptions(item);

                    item.DeliveryStoreNumber = this.transaction.StoreId;
                    item.DeliveryWarehouse = this.transaction.WarehouseId;
                    item.ShippingAddress = this.transaction.ShippingAddress;

                    if (this.transaction.RequestedDeliveryDate != default(DateTime))
                    {
                        item.DeliveryDate = this.transaction.RequestedDeliveryDate;
                    }
                }
            }
        }

        /// <summary>
        /// Get the address for the currently selected store
        /// </summary>
        /// <returns></returns>
        public Address GetStoreAddress(DataEntity.Store store)
        {
            Address storeAddress = null;
            if (store != null && store.Party != null)
            {
                // Get the primary address (and ONLY the primary address)
                DataEntity.PostalAddress address = customerDataManager.GetPrimaryPostalAddress(store.Party.Id);

                if (address != null)
                {
                    storeAddress = customerDataManager.GetAddress(address.Id);
                }
            }

            return storeAddress;
        }

        /// <summary>
        /// Get list of nearby stores within current store locator group.
        /// </summary>
        /// <returns>List of nearby stores within current store locator group</returns>
        private HashSet<string> GetStoresInStoreLocatorGroup()
        {
            DataTable nearbyStoresTableResults = null;
            HashSet<string> storeIdentifiers = new HashSet<string>();

            try
            {
                StoreData storeData = new StoreData(
                    ApplicationSettings.Database.LocalConnection,
                    ApplicationSettings.Database.DATAAREAID);

                long storeRecordId = ApplicationSettings.Terminal.StorePrimaryId;
                nearbyStoresTableResults = storeData.GetNearbyStores(storeRecordId, 0, 0, float.MaxValue, 1.0f);
                DataRowCollection nearbyStores = nearbyStoresTableResults.Rows;

                int nearbyStoresLength = nearbyStores.Count;
                for (int i = 0; i < nearbyStoresLength; i++)
                {
                    string storeNumber = nearbyStores[i][StoreData.STORENUMBERCOLUMN].ToString();
                    if (!storeIdentifiers.Contains(storeNumber))
                    {
                        storeIdentifiers.Add(storeNumber);
                    }
                }
            }
            catch (Exception exception)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), exception);
                throw;
            }
            finally
            {
                if (nearbyStoresTableResults != null)
                {
                    nearbyStoresTableResults.Dispose();
                }
            }

            return storeIdentifiers;
        }

        private static void ClearLineDeliveryOptions(LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem item)
        {
            item.DeliveryDate = null;
            item.ShippingAddress = null;
            item.DeliveryMode = null;
            item.DeliveryStoreNumber = null;

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