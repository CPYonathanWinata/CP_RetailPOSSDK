/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch
{
    partial class formInventoryLookup : frmTouchBase
    {
        private const string dimensionSeparator = " - ";
        private IPosTransaction posTransaction;
        private DataTable inventoryTable;
        private SaleLineItem saleLineItem;
        private string itemId;

        private const string ITEMIDCOLUMN = "ITEMID";
        private const string INVENTLOCATIONIDCOLUMN = "INVENTLOCATIONID";
        private const string STORENAMECOLUMN = "STORENAME";
        private const string INVENTORYCOLUMN = "INVENTORY";

        public IBarcodeInfo BarcodeInfo { get; set; }

        private Lazy<Dimensions> dimension = new Lazy<Dimensions>();

        /// <summary>
        /// Property to hide Lazy wrapper
        /// </summary>
        private Dimensions Dimension
        {
            get { return this.dimension.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="formInventoryLookup"/> class.
        /// </summary>
        /// <param name="posTransaction">The pos transaction.</param>
        public formInventoryLookup(IPosTransaction posTransaction)
            : this()
        {
            this.posTransaction = posTransaction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="formInventoryLookup"/> class.
        /// </summary>
        /// <param name="itemOrVariantId">item id or variant id</param>
        public formInventoryLookup(string itemOrVariantId)
            : this()
        {
            // Store itemOrVariantId in dimension object as a variant to be determined later
            this.Dimension.VariantId = itemOrVariantId;
        }

        protected formInventoryLookup()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                numPad1.EntryType = Contracts.UI.NumpadEntryTypes.Barcode;
                numPad1.EnteredValue = string.Empty;

                TranslateLabels();

                Dialog.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(Scanner_ScannerMessageEvent);
                Dialog.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent += new ScannerMessageEventHandler(Scanner_ScannerMessageEvent);
                Dialog.InternalApplication.Services.Peripherals.Scanner.ReEnableForScan();

                // Create the inventory table....
                CreateInventoryTable();

                ClearForm();

                if (posTransaction == null)
                {
                    btnSearch.Enabled = btnAddToTransaction.Enabled = false;

                    LSRetailPosis.DataAccess.ItemData itemData = new LSRetailPosis.DataAccess.ItemData(
                    ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);

                    // Determine if ID passed in is an item ID or variant ID, update itemId var accordingly
                    string foundItemId = itemData.GetItemForVariantId(this.Dimension.VariantId);

                    if (foundItemId == null)
                    {
                        // We didn't find an item based on the variant ID passed in which means the ID was already an item ID; assign and clear
                        this.itemId = this.Dimension.VariantId;
                        this.Dimension.VariantId = null;
                    }
                    else
                    {
                        this.itemId = foundItemId;
                    }

                    numPad1.EnteredValue = itemId;
                    BarcodeInfo = null;
                    InventoryLookup(numPad1.EnteredValue);
                }
            }

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Ideally after OnShown(), UI should have be been rendered, but it is not.
            // Adding explicit update call to render UI and then perform IO operation.
            Update();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!this.DesignMode)
            {
                Dialog.InternalApplication.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(Scanner_ScannerMessageEvent);
                Dialog.InternalApplication.Services.Peripherals.Scanner.DisableForScan();
            }

            base.OnClosed(e);
        }

        void Scanner_ScannerMessageEvent(IScanInfo scanInfo)
        {
            InventoryLookup(scanInfo.ScanDataLabel);
        }

        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmInventoryLookup are reserved at 2600 - 2649
            // In use now are ID's: 2600 - 2611
            //

            this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(2600);     // Inventory Lookup
            numPad1.PromptText = ApplicationLocalizer.Language.Translate(2601);     // Scan or enter barcode
            lblItemHeading.Text = ApplicationLocalizer.Language.Translate(2602);     // Product name
            lblInventoryHeading.Text = ApplicationLocalizer.Language.Translate(2603);     // Inventory
            lblItemIdHeading.Text = ApplicationLocalizer.Language.Translate(2604);     // Product id
            btnClose.Text = ApplicationLocalizer.Language.Translate(2605);     // Close
            btnSearch.Text = ApplicationLocalizer.Language.Translate(2607);     // Search
            colInventory.Caption = string.Format(ApplicationLocalizer.Language.Translate(2613), string.Empty); // Inventory ({0}), {0} = unit
            colStore.Caption = ApplicationLocalizer.Language.Translate(2608);     // Store
            btnAddToTransaction.Text = ApplicationLocalizer.Language.Translate(2615);     // Add to transaction
        }

        private void CreateInventoryTable()
        {
            inventoryTable = new DataTable();
            inventoryTable.Columns.Add(new DataColumn(formInventoryLookup.ITEMIDCOLUMN, typeof(string)));
            inventoryTable.Columns.Add(new DataColumn(formInventoryLookup.INVENTLOCATIONIDCOLUMN, typeof(string)));
            inventoryTable.Columns.Add(new DataColumn(formInventoryLookup.STORENAMECOLUMN, typeof(string)));
            inventoryTable.Columns.Add(new DataColumn(formInventoryLookup.INVENTORYCOLUMN, typeof(string)));
        }

        private void ClearForm()
        {
            saleLineItem = null;

            lblItem.Text = string.Empty;
            lblInventory.Text = string.Empty;
            lblItemId.Text = string.Empty;
            lblItemDimensions.Text = string.Empty;

            inventoryTable.Clear();
            grInventory.DataSource = this.inventoryTable;

            colInventory.Caption = string.Format(ApplicationLocalizer.Language.Translate(2613), string.Empty); // Inventory ({0}), {0} = unit

            numPad1.ClearValue();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string barcodeId;
            string selectedItemId = string.Empty;

            // Show the search dialog through the item service
            if (!Dialog.InternalApplication.Services.Item.ItemSearch(ref selectedItemId, 500))
            {
                return;
            }

            LSRetailPosis.DataAccess.ItemData itemData = new LSRetailPosis.DataAccess.ItemData(
                ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);
            System.Data.DataTable barcodeTable = itemData.GetBarcodesForItem(selectedItemId);

            barcodeId = string.Empty;
            if (barcodeTable != null)
            {
                switch (barcodeTable.Rows.Count)
                {

                    case 0:
                        //If no barcode is found then use the item id
                        barcodeId = string.Empty;
                        break;

                    case 1:
                        //If one barcode is found then use it.
                        barcodeId = barcodeTable.Rows[0]["ITEMBARCODE"].ToString();
                        break;

                    default:
                        //If more than one barcode is found then select a barcode
                        BarcodeConfirmation barCodeConfirmation = new BarcodeConfirmation()
                        {
                            BarcodeTable = barcodeTable
                        };

                        InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(barCodeConfirmation, () =>
                        {
                            if (barCodeConfirmation.Confirmed)
                            {
                                barcodeId = barCodeConfirmation.SelectedBarcodeId;  // Use the selected barcode

                            }
                        }
                        );

                        Dialog.InternalApplication.Services.Interaction.InteractionRequest(request);
                        break;
                }
            }

            if (barcodeId.Length != 0)
            {
                numPad1.EnteredValue = barcodeId;
            }
            else
            {
                numPad1.EnteredValue = selectedItemId;
            }

            InventoryLookup(numPad1.EnteredValue);
        }

        private void numPad1_EnterButtonPressed()
        {
            // Perform new search. Assume anything entered from this NumPad is an item ID
            this.itemId = numPad1.EnteredValue;
            if (this.dimension.IsValueCreated)
                this.dimension = new Lazy<Dimensions>();

            InventoryLookup(this.itemId);
        }

        /// <summary>
        /// Filters the results from transaction service inventory lookup with get nearby stores function.
        /// </summary>
        private void FilterWithNearbyStores()
        {
            DataTable nearbyStoresTableResults = null;

            try
            {
                StoreData storeData = new StoreData(
                    ApplicationSettings.Database.LocalConnection,
                    ApplicationSettings.Database.DATAAREAID);

                long storeRecordId = ApplicationSettings.Terminal.StorePrimaryId;
                nearbyStoresTableResults = storeData.GetNearbyStores(storeRecordId, 0, 0, float.MaxValue, 1.0f);
                DataRowCollection nearbyStores = nearbyStoresTableResults.Rows;
                HashSet<string> inventLocationNearbyStores = new HashSet<string>();

                //create a collection invent location id nearby stores
                int nearbyStoresLength = nearbyStores.Count;
                for (int i = 0; i < nearbyStoresLength; i++)
                {
                    DataRow nearbyStore = nearbyStores[i];
                    string inventLocationId = nearbyStore[StoreData.INVENTLOCATIONCOLUMN].ToString();

                    if (!inventLocationNearbyStores.Contains(inventLocationId))
                    {
                        inventLocationNearbyStores.Add(inventLocationId);
                    }
                }

                //mark rows for removal if Store invent location id is not included on invent location id get nearby stores
                IList<DataRow> deletedStores = new List<DataRow>();
                foreach (DataRow inventoryRow in this.inventoryTable.Rows)
                {
                    string inventLocationId = inventoryRow[formInventoryLookup.INVENTLOCATIONIDCOLUMN].ToString();

                    if (!inventLocationNearbyStores.Contains(inventLocationId))
                    {
                        deletedStores.Add(inventoryRow);
                    }
                }

                //remove rows
                DataRowCollection inventoryRows = this.inventoryTable.Rows;
                foreach (DataRow storeDeletionRow in deletedStores)
                {
                    inventoryRows.Remove(storeDeletionRow);
                }
            }
            catch (PosisException px)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), px);
                throw;
            }
            catch (Exception x)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                throw;
            }
            finally
            {
                if (nearbyStoresTableResults != null)
                {
                    nearbyStoresTableResults.Dispose();
                }
            }
        }

        private void InventoryLookup(string barcode)
        {
            ClearForm();

            if (GetItemInfo(barcode) == false)
            {
                ClearForm();
                return;
            }

            // Get the inventory through the Transaction Services....
            bool retVal = false;
            string comment = string.Empty;

            try
            {
                // Begin by checking if there is a connection to the Transaction Service
                Dialog.InternalApplication.TransactionServices.CheckConnection();

                // Get the inventory status from the Transaction Services...
                Dialog.InternalApplication.TransactionServices.InventoryLookup(ref retVal, ref comment, ref inventoryTable, saleLineItem.ItemId, saleLineItem.Dimension.VariantId);
            }
            catch (PosisException px)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), px);
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSErrorDialog(px);

                ClearForm();
                return;
            }
            catch (Exception x)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSErrorDialog(new PosisException(13010, x));      // Could not connect to the transaction services.

                ClearForm();
                return;
            }

            // Populate the foreground of the dialog => The local store and remove it from the list of other stores....
            LSRetailPosis.DataAccess.SettingsData settingsData = new LSRetailPosis.DataAccess.SettingsData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            int tableIndex = -1;

            lblItem.Text = saleLineItem.Description;
            lblItemId.Text = saleLineItem.ItemId;
            colInventory.Caption = string.Format(ApplicationLocalizer.Language.Translate(2613), saleLineItem.SalesOrderUnitOfMeasure);

            // Display dimensions if it is a variant item {Color - Size - Style - Config}
            if (!string.IsNullOrEmpty(saleLineItem.Dimension.VariantId))
            {
                StringBuilder dimensions = new StringBuilder();

                if (!string.IsNullOrEmpty(saleLineItem.Dimension.ColorName))
                {
                    dimensions.Append(saleLineItem.Dimension.ColorName);
                }

                if (!string.IsNullOrEmpty(saleLineItem.Dimension.SizeName))
                {
                    if (dimensions.Length > 0)
                    {
                        dimensions.Append(dimensionSeparator);
                    }

                    dimensions.Append(saleLineItem.Dimension.SizeName);
                }

                if (!string.IsNullOrEmpty(saleLineItem.Dimension.StyleName))
                {
                    if (dimensions.Length > 0)
                    {
                        dimensions.Append(dimensionSeparator);
                    }

                    dimensions.Append(saleLineItem.Dimension.StyleName);
                }

                if (!string.IsNullOrEmpty(saleLineItem.Dimension.ConfigName))
                {
                    if (dimensions.Length > 0)
                    {
                        dimensions.Append(dimensionSeparator);
                    }

                    dimensions.Append(saleLineItem.Dimension.ConfigName);
                }

                lblItemDimensions.Text = dimensions.ToString();

                if (!lblItemDimensions.Visible)
                {
                    lblItemDimensions.Visible = true;
                }
            }
            else
            {
                lblItemDimensions.Text = string.Empty;

                if (lblItemDimensions.Visible)
                {
                    lblItemDimensions.Visible = false;
                }
            }

            if (inventoryTable.Rows.Count > 0)
            {
                this.FilterWithNearbyStores();

                foreach (DataRow row in inventoryTable.Rows)
                {
                    // Convert the quantity from Inventory units to Sales units.
                    decimal inventory = saleLineItem.UnitQtyConversion.Convert(DBUtil.ToDecimal(row[formInventoryLookup.INVENTORYCOLUMN]));
                    row[formInventoryLookup.INVENTORYCOLUMN] = Dialog.InternalApplication.Services.Rounding.RoundQuantity(inventory, saleLineItem.SalesOrderUnitOfMeasure);

                    // Inventory for the local store will be shown on the right panel of dialog (and not in grid)
                    string itemInventLocation = DBUtil.ToStr(row[formInventoryLookup.INVENTLOCATIONIDCOLUMN]);

                    if (ApplicationSettings.Terminal.InventLocationId.Equals(itemInventLocation, StringComparison.OrdinalIgnoreCase))
                    {
                        lblInventory.Text = string.Format(ApplicationLocalizer.Language.Translate(2614), row[formInventoryLookup.INVENTORYCOLUMN], saleLineItem.SalesOrderUnitOfMeasure);
                        tableIndex = inventoryTable.Rows.IndexOf(row);
                    }
                }

                //if table index is 0, then it's already on the top of the list.
                if (tableIndex > 0)
                {
                    //table index is pointing the the store that user is logging on.
                    //with this help, we can move the data to the top of the list (move it to index 0)
                    DataRowCollection storesList = inventoryTable.Rows;
                    DataRow currentStore = storesList[tableIndex];

                    //we need to copy the data on current Store and insert to index 0 since
                    //removing and adding the data with the same row reference will lose the data
                    object[] sourceData = currentStore.ItemArray;
                    object[] dataCopy = new object[sourceData.Length];
                    sourceData.CopyTo(dataCopy, 0);
                    storesList.RemoveAt(tableIndex);
                    currentStore.ItemArray = dataCopy;
                    storesList.InsertAt(currentStore, 0);
                }

                grInventory.DataSource = this.inventoryTable;
            }
            else
            {
                using (LSRetailPosis.POSProcesses.frmMessage msgBox = new LSRetailPosis.POSProcesses.frmMessage(2609, MessageBoxButtons.OK, MessageBoxIcon.Error))  // Unable to retrieve the inventory status.
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(msgBox);
                }

                ClearForm();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private bool GetItemInfo(string barcode)
        {
            System.Diagnostics.Debug.Assert(barcode != null, "barcode should not be null");

            if (string.IsNullOrEmpty(barcode))
            {
                return false;
            }

            saleLineItem = (SaleLineItem)Dialog.InternalApplication.BusinessLogic.Utility.CreateSaleLineItem(
                ApplicationSettings.Terminal.StoreCurrency,
                Dialog.InternalApplication.Services.Rounding,
                posTransaction);

            IScanInfo scanInfo = Dialog.InternalApplication.BusinessLogic.Utility.CreateScanInfo();
            scanInfo.ScanDataLabel = barcode;
            scanInfo.EntryType = BarcodeEntryType.ManuallyEntered;

            //IBarcodeInfo barcodeInfo = Dialog.InternalApplication.BusinessLogic.Utility.CreateBarcodeInfo();
            BarcodeInfo = Dialog.InternalApplication.Services.Barcode.ProcessBarcode(scanInfo);

            if ((BarcodeInfo.InternalType == BarcodeInternalType.Item) && (BarcodeInfo.ItemId != null))
            {
                // The entry was a barcode which was found and now we have the item id...
                saleLineItem.ItemId = BarcodeInfo.ItemId;
                saleLineItem.BarcodeId = BarcodeInfo.BarcodeId;
                saleLineItem.SalesOrderUnitOfMeasure = BarcodeInfo.UnitId;
                if (BarcodeInfo.BarcodeQuantity > 0)
                {
                    saleLineItem.Quantity = BarcodeInfo.BarcodeQuantity;
                }

                if (BarcodeInfo.BarcodePrice > 0)
                {
                    saleLineItem.Price = BarcodeInfo.BarcodePrice;
                }

                saleLineItem.EntryType = BarcodeInfo.EntryType;

                saleLineItem.Dimension.ColorId = BarcodeInfo.InventColorId;
                saleLineItem.Dimension.SizeId = BarcodeInfo.InventSizeId;
                saleLineItem.Dimension.StyleId = BarcodeInfo.InventStyleId;
                saleLineItem.Dimension.ConfigId = BarcodeInfo.ConfigId;
                saleLineItem.Dimension.VariantId = BarcodeInfo.VariantId;
            }
            else
            {
                // It could be an ItemId
                saleLineItem.ItemId = BarcodeInfo.BarcodeId;
                saleLineItem.EntryType = BarcodeInfo.EntryType;
            }

            // Please assign empty string for BarcodeId if it is null. 
            if (BarcodeInfo.BarcodeId == null)
            {
                BarcodeInfo.BarcodeId = string.Empty;
            }

            BarcodeInfo.InternalType = BarcodeInternalType.Item;
            BarcodeInfo.EntryType = BarcodeEntryType.Selected;

            Dialog.InternalApplication.Services.Item.ProcessItem(saleLineItem, true);

            if (saleLineItem.Found == false)
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611);             // Item not found.
                return false;
            }

            //Color,Size,Style,config
            if (saleLineItem.Dimension.VariantId == null)
            {
                saleLineItem.Dimension.VariantId = string.Empty;
            }

            if (saleLineItem.Dimension.EnterDimensions)
            {
                if (!string.IsNullOrWhiteSpace(this.Dimension.VariantId))
                {
                    // If we know the variant ID, load the dimensions
                    Dialog.InternalApplication.Services.Dimension.GetDimensionForVariant(this.Dimension);
                    if (this.Dimension.DistinctProductVariantId != 0)
                    {
                        this.Dimension.EnterDimensions = false;
                        saleLineItem.Dimension = this.Dimension;
                    }
                }
                else // Here we are now dealing with master item (that may include Kit Master ID) 
                {
                    // See if we have a kit and select configuration if we do
                    ItemData itemData = new ItemData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);
                    if (itemData.DoesKitExist(saleLineItem.ItemId))
                    {
                        // Invoke Product Details Form to get Kit Configuration (return specific variantId)
                        InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                            new ProductDetailsConfirmation() { ItemNumber = saleLineItem.ItemId, SourceContext = ProductDetailsSourceContext.InventoryLookup }, () => { });
                        PosApplication.Instance.Services.Interaction.InteractionRequest(request);
                        ProductDetailsConfirmation confirmation = request.Context as ProductDetailsConfirmation;
                        if (confirmation != null && confirmation.Confirmed && confirmation.AddToSale)
                        {
                            saleLineItem.Dimension = (Dimensions)confirmation.ResultData;
                        }
                        else
                        {
                            NetTracer.Information("Operation [ItemSale]:Valid Kit dimension is not defined.");
                            return false;
                        }
                    }
                    else
                    {
                        /* 
                         * When Product is Non-Kit Master Item, to check inventory, we need to get specific variant using Dimension picker. 
                         * Reason Not to use:  RunOperation => SetDimensions because RunOperation creates additional blank Cart, 
                         * this was causing Item Add from Product details Page to add item in above blank cart (from SetDimension ) 
                         * Fix: Call Dimension picker through GetDimensions and DimensionConfirmation to avoid creating additional blank cart. 
                         * This fixes Bug # 1033269: Bugbash [EPOS] : Adding Item fails without any error or warning from product details when added, after inventory check.
                         */

                        bool isDimensionConfirm = false;

                        DataTable inventDimCombination = PosApplication.Instance.Services.Dimension.GetDimensions(saleLineItem.ItemId);

                        DimensionConfirmation dimensionConfirmation = new DimensionConfirmation()
                        {
                            InventDimCombination = inventDimCombination,
                            DimensionData = new Dimensions(),
                            DisplayDialog = true
                        };

                        InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(dimensionConfirmation, () =>
                        {
                            if (dimensionConfirmation != null && dimensionConfirmation.Confirmed && dimensionConfirmation.SelectDimCombination != null)
                            {
                                DataRow dr = dimensionConfirmation.SelectDimCombination;
                                saleLineItem.Dimension.VariantId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["VARIANTID"]);
                                saleLineItem.Dimension.ColorId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["COLORID"]);
                                saleLineItem.Dimension.ColorName = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["COLOR"]);
                                saleLineItem.Dimension.SizeId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["SIZEID"]);
                                saleLineItem.Dimension.SizeName = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["SIZE"]);
                                saleLineItem.Dimension.StyleId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["STYLEID"]);
                                saleLineItem.Dimension.StyleName = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["STYLE"]);
                                saleLineItem.Dimension.ConfigId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["CONFIGID"]);
                                saleLineItem.Dimension.ConfigName = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["CONFIG"]);
                                saleLineItem.Dimension.InventDimId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["INVENTDIMID"]);
                                saleLineItem.Dimension.DistinctProductVariantId = (Int64)dr["DISTINCTPRODUCTVARIANT"];

                                if (string.IsNullOrEmpty(saleLineItem.BarcodeId))
                                {
                                    saleLineItem.BarcodeId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["ITEMBARCODE"]);
                                }

                                string unitId = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["UNITID"]);

                                if (!string.IsNullOrEmpty(unitId))
                                {
                                    saleLineItem.SalesOrderUnitOfMeasure = unitId;
                                }

                                isDimensionConfirm = true;
                            }
                            else
                            {
                                NetTracer.Information("Inventory Lookup Product dimension is not selected.");
                            }
                        } );

                        PosApplication.Instance.Services.Interaction.InteractionRequest(request);

                        if (!isDimensionConfirm)
                        {
                            return false;
                        }
                    }
                }

                // Fill up BarcodeInfo with master dimension info, 

                BarcodeInfo.VariantId = saleLineItem.Dimension.VariantId;
                BarcodeInfo.ConfigId = saleLineItem.Dimension.ConfigId;
                BarcodeInfo.ConfigName = saleLineItem.Dimension.ConfigName;
                BarcodeInfo.InventColorId = saleLineItem.Dimension.ColorId;
                BarcodeInfo.ColorName = saleLineItem.Dimension.ColorName;
                BarcodeInfo.InventSizeId = saleLineItem.Dimension.SizeId;
                BarcodeInfo.SizeName = saleLineItem.Dimension.SizeName;
                BarcodeInfo.InventStyleId = saleLineItem.Dimension.StyleId;
                BarcodeInfo.StyleName = saleLineItem.Dimension.StyleName;
                BarcodeInfo.EnterDimensions = false;
            }

            if (!saleLineItem.SalesOrderUnitOfMeasure.Equals(saleLineItem.InventOrderUnitOfMeasure, StringComparison.OrdinalIgnoreCase))
            {
                UnitOfMeasureData uomData = new UnitOfMeasureData(
                    ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID,
                    ApplicationSettings.Terminal.StorePrimaryId, Dialog.InternalApplication);

                saleLineItem.UnitQtyConversion = uomData.GetUOMFactor(saleLineItem.InventOrderUnitOfMeasure, saleLineItem.SalesOrderUnitOfMeasure, saleLineItem);
            }

            return true;
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            gvInventory.MovePrevPage();
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            gvInventory.MoveNextPage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gvInventory.MovePrev();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gvInventory.MoveNext();
        }

        private void btnAddToTransaction_Click(object sender, EventArgs e)
        {
            if (BarcodeInfo != null && BarcodeInfo.InternalType == BarcodeInternalType.Item)
            {
                Dialog.InternalApplication.RunOperation(PosisOperations.ItemSale, BarcodeInfo);
            }
            else if (!string.IsNullOrEmpty(lblItemId.Text))
            {
                if (saleLineItem != null && saleLineItem.Dimension.EnterDimensions)
                {
                    Dialog.InternalApplication.RunOperation(PosisOperations.ItemSale, saleLineItem);
                }
                else
                {
                    Dialog.InternalApplication.RunOperation(PosisOperations.ItemSale, lblItemId.Text);
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}