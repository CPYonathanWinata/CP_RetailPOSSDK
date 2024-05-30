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
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Text;
using LSRetailPosis;
using LSRetailPosis.ButtonGrid;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.Interaction.Properties;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System.ComponentModel;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// View model class for the product details form.
    /// </summary>
    internal sealed class ProductDetailsViewModel : INotifyPropertyChanged
    {
        #region private members

        /// <summary>
        /// Gets or sets the product details information.
        /// </summary>
        private ProductDetails productDetails { get; set; }

        /// <summary>
        /// Gets DataManager object. 
        /// </summary>
        private ItemDataManager itemDataManager;

        /// <summary>
        /// Gets or sets Kit product master ID
        /// </summary>
        private long kitProductMaster { get; set; }

        /// <summary>
        /// Gets or sets Kit product master number
        /// </summary>
        private bool dimensionConfigNeeded { get; set; }

        /// <summary>
        /// Gets or sets Kit Rec ID
        /// </summary>
        private long kitRecID { get; set; }

        /// <summary>
        /// Gets or sets the ItemData for Products and it's properties
        /// </summary>
        private ItemData itemData;

        /// <summary>
        /// Gets or sets the ItemData for Products and it's properties
        /// </summary>
        private ItemData ItemData
        {
            get
            {
                if (itemData == null)
                {
                    itemData = new ItemData(ApplicationSettings.Database.LocalConnection,
                        ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);
                }
                return itemData;
            }
        }

        /// <summary>
        /// Gets or sets item number/product number
        /// </summary>
        private Dimensions dimension;

        /// <summary>
        /// private boolean to track of whether is ItemNumber a kit master or not. 
        /// </summary>
        private bool? isKit;

        private Image image;

        private string barCode;

        private string price;

        #endregion 
        
        #region public members

        /// <summary>
        ///  Returning Dimension for Kit product or return null 
        /// </summary>
        public Dimensions DimensionData
        {
            get { return dimension; }
        }

        /// <summary>
        /// Check if Kit exists, if yes then return true, else false. 
        /// </summary>
        public bool IsKit
        {
            get
            {
                if (isKit == null)
                {
                    if (string.IsNullOrWhiteSpace(this.ItemNumber))
                    {
                        throw new ArgumentException("Empty string or null value is not allowed for ItemNumber");
                    }

                    using (DataTable tblKitDetails = ItemData.GetRetailKitDetails(this.ItemNumber))
                    {
                        if (tblKitDetails.Rows.Count == 1)
                        {
                            DataRow row = tblKitDetails.Rows[0];
                            kitRecID = (Int64)row["KITRECID"];
                            kitProductMaster = (Int64)row["KITPRODUCTMASTER"];
                            isKit = true;
                        }
                        else
                            isKit = false;
                    }
                }
                return isKit ?? false;
            }
        }

        /// <summary>
        /// Gets or sets item number/product number
        /// </summary>
        public string ItemNumber { get; private set; }

        /// <summary>
        /// Represents the variant ID of an master item (including Kit product)
        /// </summary>
        public string VariantId { get; private set; }

        /// <summary>
        ///  DataTable containing a list of components 
        /// </summary>
        public DataTable KitConfigComponents { get; private set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string Description
        {
            get { return this.productDetails.Description; }
        }

        /// <summary>
        /// Gets the item image.
        /// </summary>
        public Image Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }

                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        /// <summary>
        /// Gets the date blocked value.
        /// </summary>
        public DateTime? DateBlocked
        {
            get
            {
                DateTime dateBlocked = this.productDetails.DateBlocked;

                return (dateBlocked == LSRetailPosis.DevUtilities.Utility.POSNODATE)
                    ? (DateTime?)null
                    : dateBlocked;
            }
        }

        /// <summary>
        /// Gets the date to be blocked value.
        /// </summary>
        public DateTime? DateToBeBlocked
        {
            get
            {
                DateTime dateToBeBlocked = this.productDetails.DateToBeBlocked;

                return (dateToBeBlocked == LSRetailPosis.DevUtilities.Utility.POSNODATE)
                    ? (DateTime?)null
                    : dateToBeBlocked;
            }
        }

        /// <summary>
        /// Gets the issue date.
        /// </summary>
        public DateTime? IssueDate
        {
            get
            {
                DateTime issueDate = this.productDetails.IssueDate;

                return (issueDate == LSRetailPosis.DevUtilities.Utility.POSNODATE)
                    ? (DateTime?)null
                    : issueDate;
            }
        }

        /// <summary>
        /// Gets the search name.
        /// </summary>
        public string SearchName
        {
            get { return this.productDetails.SearchName; }
        }


        /// <summary>
        /// Gets the barcode.
        /// </summary>
        public string Barcode
        {
            get { return barCode; }
            set
            {
                if (barCode != value)
                {
                    barCode = value;

                    OnPropertyChanged("Barcode");
                }
            }
        }

        /// <summary>
        /// Gets the price.
        /// </summary>
        public string Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;

                    OnPropertyChanged("Price");
                }
            }
        }

        /// <summary>
        /// Gets the unit of measure.
        /// </summary>
        public string UnitOfMeasure
        {
            get { return this.productDetails.UnitOfMeasure; }
        }

        /// <summary>
        /// Gets the product category.
        /// </summary>
        public string ProductCategory
        {
            get { return this.productDetails.ProductCategory; }
        }

        /// <summary>
        /// Gets the formatted product category hierarchy.
        /// </summary>
        public string FormattedProductCategoryHierarchy { get; private set; }

        /// <summary>
        /// Gets the product attributes collection.
        /// </summary>
        public Collection<NameValuePair> ProductAttributes
        {
            get { return this.productDetails.ProductAttributes; }
        }

        #endregion

        #region private functions

        /// <summary>
        /// Gets the formatted product category hierarchy string.
        /// </summary>
        /// <returns>The formatted product category hierarchy string.</returns>
        private string GetFormattedProductCategoryHierarchy()
        {
            StringBuilder builder = new StringBuilder();

            Collection<CategoryInformation> categoryHierarchy = this.productDetails.CategoryHierarchy;
            for (int i = 0; i < categoryHierarchy.Count; i++)
            {
                // only uses the format when collection has more than one and it's not the last one
                bool useFormat = (categoryHierarchy.Count > 1) && (i != (categoryHierarchy.Count - 1));

                builder.AppendFormat(useFormat ?
                    ApplicationLocalizer.Language.Translate(99817) // "{0} > "
                    : "{0}",
                    categoryHierarchy[i].CategoryName);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Sets KitName, Description and KitQuantity property for the current Kit Id
        /// </summary>
        /// <param name="updateDetailsPageValues">If true, then update Product Details Page controls like image, barcode and price through GetRetailKitVariantIDOnCurrentSelection function.</param>
        /// <exception cref="System.ArgumentException">Not a valid KitRecId</exception>
        private void LoadDefaultKitComponents(bool updateDetailsPageValues)
        {
            // Check if the KitId is a valid value before accessing data layer
            if (this.kitRecID > 0)
            {
                using (DataTable dt = ItemData.GetKitDefaultComponentList(this.kitRecID))
                {
                    KitConfigComponents.Clear();
                    KitConfigComponents.Load(dt.CreateDataReader());

                    if (dimension == null)
                    {
                        dimension = new Dimensions();
                    }

                    GetRetailKitVariantIDOnCurrentSelection(updateDetailsPageValues);
                }
            }
            else
            {
                throw new ArgumentException("Not a valid KitRecId");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailsViewModel"/> class.
        /// </summary>
        /// <param name="itemNumber">The item number.</param>
        /// <param name="variantId">Variant ID, pass null if none</param>
        /// <param name="isSourceSalesTransOrInventoryLookup">true if calling Product details form from Sales Transaction or Inventory Lookup</param>
        public ProductDetailsViewModel(string itemNumber, string variantId, bool isSourceSalesTransOrInventoryLookup)
        {
            if (string.IsNullOrWhiteSpace(itemNumber))
                throw new ArgumentNullException("itemNumber");
            
            this.ItemNumber = itemNumber;

            this.VariantId = variantId;

            this.KitConfigComponents = new DataTable();

            if (this.IsKit)
            {
                // If you want to add to Sale or look for Inventory then set dimensionConfigNeeded to true. 
                dimensionConfigNeeded = isSourceSalesTransOrInventoryLookup;

                // parameter false to LoadDefaultKitComponents will not load price, image, price and barcode inside when it will happen as part of GetProductDetails in below code.  
                this.LoadDefaultKitComponents(false);
            }

            itemDataManager = new ItemDataManager(
                    PosApplication.Instance.Settings.Database.Connection,
                    PosApplication.Instance.Settings.Database.DataAreaID);

            this.productDetails = itemDataManager.GetProductDetails(
                ApplicationSettings.Terminal.StorePrimaryId,
                this.ItemNumber,
                this.VariantId,
                ApplicationSettings.Terminal.CultureName);

            Image productImage = GUIHelper.GetBitmap(this.productDetails.ImageData);

            if (productImage != null)
                this.Image = productImage;

            this.Barcode = this.productDetails.Barcode;
            
            decimal CurrentPrice = 0;

            // If InvenDimId is not empty space then get variant specific price. 
            if (!string.IsNullOrWhiteSpace(this.productDetails.InventDimId))
            {
                CurrentPrice = PosApplication.Instance.Services.Price.GetItemPrice(ItemNumber, this.productDetails.InventDimId, this.productDetails.UnitOfMeasure);
                this.Price = PosApplication.Instance.Services.Rounding.RoundForDisplay(CurrentPrice, true, false);
            }

            // If current price is 0 (that means, didn't find any price for variant, in case of variant) or it is non-variant, so lets get the price for non-variant item. 
            if (CurrentPrice == 0)
            {
                decimal PriceInDecimal = PosApplication.Instance.Services.Price.GetItemPrice(this.ItemNumber, this.productDetails.UnitOfMeasure);
                this.Price = PosApplication.Instance.Services.Rounding.RoundForDisplay(PriceInDecimal, true, false);
            }

            this.FormattedProductCategoryHierarchy = GetFormattedProductCategoryHierarchy();
        }

        #endregion

        #region public functions

        /// <summary>
        /// Shows the inventory lookup form using ID.
        /// </summary>
        public void ShowInventoryLookup()
        {
            // If we know the variant ID pass that, otherwise pass the item ID
            string id = string.IsNullOrWhiteSpace(this.VariantId) ? this.productDetails.ID : this.VariantId;
            PosApplication.Instance.Services.Dialog.InventoryLookup(id);
        }

        /// <summary>
        /// Get Variant Id for Current Selected Kit components...
        /// </summary>
        /// <param name="updateDetailsPageValues">If true, then update Product Details Page controls like image, barcode and price.</param>
        /// <exception cref="System.ArgumentException">Not a valid KitRecId</exception>
        public void GetRetailKitVariantIDOnCurrentSelection(bool updateDetailsPageValues)
        {
            if (kitRecID > 0)
            {
                Dictionary<long, long> CurrentKitComponentSelectionList = new Dictionary<long, long>();
                foreach (DataRow row in KitConfigComponents.Rows)
                {
                    CurrentKitComponentSelectionList.Add((long)row["DEFAULTCOMPONENTRECID"], (long)row["COMPONENTPRODUCTID"]);
                }

                using (DataTable dt = ItemData.GetKitConfigBasedVariantId(this.ItemNumber, CurrentKitComponentSelectionList))
                {
                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];

                        dimension.VariantId = PosApplication.Instance.BusinessLogic.Utility.ToString(row["RETAILKITRETAILVARIANTID"]);
                        this.VariantId = dimension.VariantId;
                        dimension.DistinctProductVariantId = (long)row["RETAILKITDISTINCTPRODUCTVARIANT"];
                        this.Barcode = PosApplication.Instance.BusinessLogic.Utility.ToString(row["KITCONFIGBARCODE"]);

                        if (dimensionConfigNeeded)
                        {
                            dimension.ConfigId = PosApplication.Instance.BusinessLogic.Utility.ToString(row["CONFIGID"]);
                            dimension.ConfigName = PosApplication.Instance.BusinessLogic.Utility.ToString(row["CONFIGNAME"]);
                            dimension.InventDimId = PosApplication.Instance.BusinessLogic.Utility.ToString(row["RETAILKITINVENTDIMID"]);
                            dimension.EnterDimensions = false;
                            dimension.ColorId = string.Empty;
                            dimension.ColorName = string.Empty;
                            dimension.SizeId = string.Empty;
                            dimension.SizeName = string.Empty;
                            dimension.StyleId = string.Empty;
                            dimension.StyleName = string.Empty;
                        }

                        // If this call is made as part of constructor then don't need to find and update price, image and barcode. That should be part of constructor productDetails page loading. 
                        // Following part will be executed when substitute product is selected. 
                        if (updateDetailsPageValues)
                        {
                            string inventDimId = PosApplication.Instance.BusinessLogic.Utility.ToString(row["RETAILKITINVENTDIMID"]);

                            // Retrieve Price for currently select Kit configuration... 
                            decimal CurrentPrice = 0;

                            // If InvenDimId is not empty space then get Kit Configuration specific price. 
                            if (!string.IsNullOrWhiteSpace(inventDimId))
                            {
                                CurrentPrice = PosApplication.Instance.Services.Price.GetItemPrice(ItemNumber, inventDimId, this.productDetails.UnitOfMeasure);
                                this.Price = PosApplication.Instance.Services.Rounding.RoundForDisplay(CurrentPrice, true, false);
                            }

                            // If current price is 0 (that means, didn't find any price for Kit configuration, so lets get the price for master Kit item. 
                            if (CurrentPrice == 0)
                            {
                                decimal PriceInDecimal = PosApplication.Instance.Services.Price.GetItemPrice(ItemNumber, this.productDetails.UnitOfMeasure);
                                this.Price = PosApplication.Instance.Services.Rounding.RoundForDisplay(PriceInDecimal, true, false);
                            }

                            // Retrieve Image for currently select Kit configuration... 
                            byte[] imageData = null;

                            // If InventDimid is populated, then get image for variant
                            if (!string.IsNullOrWhiteSpace(inventDimId))
                            {
                                imageData = itemDataManager.GetItemImageData(ItemNumber, inventDimId);
                            }

                            // Either item is variant, but didn't get image for variant OR item is no-variant, then get image based on itemNumber. 
                            if (imageData == null)
                            {
                                imageData = itemDataManager.GetItemImageData(ItemNumber);
                            }

                            Image productImage = null;

                            if (imageData != null)
                            {
                                productImage = GUIHelper.GetBitmap(imageData);
                            }

                            this.Image = productImage ?? Resources.ProductUnavailable;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Can't Find VariantID as there is no matching config associated with this KITRECID");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Not a valid KitRecId");
            }
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
}