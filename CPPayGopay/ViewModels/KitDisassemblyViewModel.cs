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
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    internal sealed class KitDisassemblyViewModel : INotifyPropertyChanged
    {
        private const string ColComment = "COMMENT";
        private const string ColUnit = "UNIT";
        private const string ColQtyToCart = "QTYTOCART";
        private const string ColComponentQty = "COMPONENTQTY";
        private const string ColQtyToInventory = "QTYTOINVENTORY";
        private const string ComputedQtyToInventory = "COMPONENTQTY - QTYTOCART"; // ColComponentQty - ColQtyToCart

        private const string CommentSeparator = " - ";
        private const int SqlTrue = 1;

        #region Fields

        private ItemData itemData;
        private decimal kitQuantity;
        private string kitVariantID;
        private string kitName;
        private string kitID;
        private string description;

        #endregion

        #region Ctr

        /// <summary>
        /// Initializes a new instance of the <see cref="KitDisassemblyViewModel"/> class.
        /// </summary>
        internal KitDisassemblyViewModel()
        {
            InitDatasource();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The kit's item Id.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// throws an exception if non existing kitid or null value is assigned.
        /// </exception>
        public string KitID
        {
            get
            {
                return kitID;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("KitID");
                }

                if (!DoesKitExist(value))
                {
                    throw new ArgumentOutOfRangeException("KitID", string.Format("The KitId {0} does not represent a valid or existing kit", value));
                }

                if (!string.Equals(kitID, value, StringComparison.Ordinal))
                {
                    // Reset the view model
                    this.Clear();

                    kitID = value;
                    this.PopulateData();

                    OnPropertyChanged("KitID");
                }
            }
        }

        /// <summary>
        /// Gets or sets the current kit variant id
        /// also updates the components if the KitVariantId is changed.
        /// </summary>
        /// <exception cref="ArgumentException">throws exception on null or empty value</exception>
        public string KitVariantID
        {
            get
            {
                return kitVariantID;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("KitVariantID");
                }

                if (!string.Equals(kitVariantID, value, StringComparison.Ordinal))
                {
                    this.Components.Clear();

                    kitVariantID = value;

                    using (DataTable dt = itemData.GetRetailKitVariantComponents(kitVariantID))
                    {
                        Components.Load(dt.CreateDataReader());
                        InsertCommentLineForVariant();
                    }

                    OnPropertyChanged("KitVariantID");

                    this.KitQuantity = 1;
                }
            }
        }

        /// <summary>
        /// Description of the kit.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            private set
            {
                if (!string.Equals(description, value, StringComparison.Ordinal))
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Item name of the kit.
        /// </summary>
        public string KitName
        {
            get
            {
                return kitName;
            }
            private set
            {
                if (!string.Equals(kitName, value, StringComparison.Ordinal))
                {
                    kitName = value;
                    OnPropertyChanged("KitName");
                }
            }
        }

        /// <summary>
        ///  DataTable containing a list of constituent components of the kit.
        /// </summary>
        public DataTable Components { get; private set; }

        /// <summary>
        /// Executes the update add to cart quantity.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="subTotal">The sub-total.</param>
        /// <returns>true if update is successful, false otherwise.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">row</exception>
        public bool ExecuteUpdateAddToCartQuantity(int row, decimal subTotal)
        {
            if ((row < 0) || (row >= this.Components.Rows.Count))
            {
                throw new ArgumentOutOfRangeException("row");
            }

            // Check if quantity is valid for current uom and Update the data source that is bound to the data grid.
            if (PosApplication.Instance.BusinessLogic.Utility.IsQuantityAllowed(subTotal, (string)this.Components.Rows[row][ColUnit]))
            {
                this.Components.Rows[row][ColQtyToCart] = subTotal;
                return true;
            }
            else
            {
                // return false if quantity is invalid and update fails.
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the quantity of the kit and
        /// also updates the total quantity of each components 
        /// when the quantity of the kit changes.
        /// </summary>
        public decimal KitQuantity
        {
            get
            {
                return kitQuantity;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("KitQuantity can not be zero or negative");
                }

                if (kitQuantity != value)
                {
                    foreach (DataRow row in Components.Rows)
                    {
                        row[ColQtyToCart] = 0; //reset add to cart quantity if the kit quantity changes
                        row[ColComponentQty] = (((decimal)row[ColComponentQty]) / kitQuantity) * value;
                    }

                    kitQuantity = value;
                    OnPropertyChanged("KitQuantity");
                }
            }
        }

        /// <summary>
        /// Gets the flag which determines if a kit can be disassembled at POS or not.
        /// </summary>
        public bool DisassemblAtRegister { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the list of components in the kit that will be added to cart.
        /// </summary>
        public Collection<CartItem> GetCartItems()
        {
            Collection<CartItem> cartItems = new Collection<CartItem>();

            foreach (DataRow dr in this.Components.Rows)
            {
                if ((decimal)dr[ColQtyToCart] > 0)
                {
                    cartItems.Add(new CartItem { 
                        ItemId = (string)dr["ITEMID"],
                        VariantId = (string)dr["VARIANTID"],
                        Unit = (string)dr[ColUnit],
                        Quantity = (decimal)dr[ColQtyToCart] 
                    });
                }
            }

            return cartItems;
        }

        /// <summary>
        /// Doeses the kit exist.
        /// </summary>
        /// <param name="kitId">The kit id.</param>
        /// <returns>true if the kit exists.</returns>
        /// <exception cref="System.ArgumentException">kitId</exception>
        public bool DoesKitExist(string kitId)
        {
            if (string.IsNullOrWhiteSpace(kitId))
            {
                throw new ArgumentException("kitId");
            }

            return itemData.DoesKitExist(kitId);
        }

        private void InitDatasource()
        {
            itemData = new ItemData(ApplicationSettings.Database.LocalConnection,
                ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);

            Components = new DataTable();

            Components.Columns.Add(ColComment, typeof(string));
            Components.Columns.Add(ColComponentQty, typeof(decimal));
            Components.Columns.Add(ColQtyToCart, typeof(decimal)).DefaultValue = 0;
            Components.Columns.Add(ColQtyToInventory, typeof(decimal), ComputedQtyToInventory);

            Components.ColumnChanging += new DataColumnChangeEventHandler(ValidateCartQtyValue);
        }

        private void ValidateCartQtyValue(object obj, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName.Equals(ColQtyToCart, StringComparison.Ordinal))
            {
                decimal newQtyToCartValue;
                if (!decimal.TryParse(e.ProposedValue.ToString(), out newQtyToCartValue)
                    || (newQtyToCartValue < 0)
                    || (newQtyToCartValue > (decimal)e.Row[ColComponentQty]))
                {
                    e.ProposedValue = e.Row[ColQtyToCart];

                    // "The quantity is not valid. Enter a quantity that is greater than zero and less than or equal {0}."
                    // {0} is the maximum quantity of that component available for disassembly
                    string message = string.Format(LSRetailPosis.ApplicationLocalizer.Language.Translate(99899), String.Format("{0:0}", (decimal)e.Row[ColComponentQty]));
                    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    }
                }
            }
        }

        private void InsertCommentLineForVariant()
        {
            foreach (DataRow row in Components.Rows)
            {
                // Size, Color, Style and Config dimensions will be added to 'dimensions' list 
                // the comment line is then built by concatenating these values
                List<string> dimensions = new List<string>();

                AddDimensions(dimensions, row, "COLOR");
                AddDimensions(dimensions, row, "SIZE");
                AddDimensions(dimensions, row, "STYLE");
                AddDimensions(dimensions, row, "CONFIG");
                row[ColComment] = string.Join(CommentSeparator, dimensions);
            }
        }

        private static void AddDimensions(List<string> dimensions, DataRow row, string dimensionName)
        {
            string dimentionVal = (string)row[dimensionName];
            if (!string.IsNullOrWhiteSpace(dimentionVal))
            {
                dimensions.Add(dimentionVal);
            }
        }

        /// <summary>
        /// Sets KitName, Description and KitQuantity property for the current Kit ID.
        /// </summary>
        private void PopulateData()
        {
            // Check if the KitId is a valid value before accessing data layer
            if (!string.IsNullOrWhiteSpace(this.KitID))
            {
                using (DataTable tblKitDetails = itemData.GetRetailKitDetails(this.KitID))
                {
                    if (tblKitDetails.Rows.Count == 1)
                    {
                        DataRow row = tblKitDetails.Rows[0];
                        KitName = (string)row["NAME"];
                        Description = (string)row["DESCRIPTION"];
                        KitQuantity = 1;
                        DisassemblAtRegister = ((int)row["DISASSEMBLEATREGISTERFLAG"] == SqlTrue);
                    }
                }
            }
        }

        /// <summary>
        /// Resets the view model to its initial state.
        /// </summary>
        public void Clear()
        {
            Components.Clear();
            kitVariantID = null;
            OnPropertyChanged("KitVariantID");
            description = null;
            OnPropertyChanged("Description");
            kitName = null;
            OnPropertyChanged("KitName");
            kitQuantity = 0;
            OnPropertyChanged("KitQuantity");
            kitID = null;
            OnPropertyChanged("KitID");
        }

        #endregion

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
    }
}
