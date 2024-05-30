/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Collections.Generic;
using System.Data;
using LSRetailPosis.DataAccess;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    internal sealed class KitComponentSubstituteViewModel
    {
        /// <summary>
        /// Gets or sets the ItemData for Products and it's properties
        /// </summary>
        private ItemData _itemData;

        /// <summary>
        /// Gets or sets the item number.
        /// </summary>
        public string ItemNumber { get; set; }

        /// <summary>
        /// Gets or sets the item number.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the Kit Component Product RecId.
        /// </summary>
        public long KitComponentProductRecId { get; set; }

        /// <summary>
        /// Gets or sets the Product RecId.
        /// </summary>
        public decimal KitComponentQty { get; set; }
        
        /// <summary>
        /// Gets or sets the Product RecId.
        /// </summary>
        public string KitComponentUOM { get; set; }

        /// <summary>
        ///  DataTable containing a list of Kit Component Substitutes
        /// </summary>
        public DataTable ComponentSubstitutes { get; private set; }

        /// <summary>
        /// Gets or sets the confirmation object.
        /// </summary>
        public KitComponentSubstituteConfirmation ConfirmationResult { get; set; }

        /// <summary>
        /// Gets or sets the ItemData for Products and it's properties
        /// </summary>
        private ItemData ItemData
        {
            get
            {
                if (_itemData == null)
                {
                    _itemData = new ItemData(ApplicationSettings.Database.LocalConnection,
                        ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Terminal.StorePrimaryId);
                }
                return _itemData;
            }
        }

        #region Constructor

        /// <summary>
        /// Constructor for KitComponentSubstituteConfirmation class, which retrieves Substitute list 
        /// </summary>
        /// <param name="obj"></param>
        public KitComponentSubstituteViewModel(KitComponentSubstituteConfirmation obj)
        {
            ConfirmationResult = obj;
            ComponentSubstitutes = new DataTable();
            
            if (ConfirmationResult != null)
            {
                if (ConfirmationResult.SelectedComponentLineRecId > 0)
                {
                    using (DataTable dt = ItemData.GetKitSubstituteComponentList(ConfirmationResult.SelectedComponentLineRecId))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            int RowIndexToDelete = -1;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if ((long)dr["COMPONENTPRODUCTID"] == (long)ConfirmationResult.IncludedComponents[ConfirmationResult.SelectedComponentLineRecId])
                                {
                                    KitComponentProductRecId = ConfirmationResult.SelectedComponentLineRecId;
                                    ItemNumber = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["Number"]);
                                    ItemName = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["Name"]);
                                    KitComponentQty = PosApplication.Instance.BusinessLogic.Utility.ToDecimal(dr["QTY"]);
                                    KitComponentUOM = PosApplication.Instance.BusinessLogic.Utility.ToString(dr["UnitOfMeasure"]);
                                    RowIndexToDelete = dt.Rows.IndexOf(dr);
                                }
                            }
                            if (RowIndexToDelete >= 0)
                            {
                                dt.Rows.RemoveAt(RowIndexToDelete);
                            }
                            ComponentSubstitutes.Clear();
                            ComponentSubstitutes.Load(dt.CreateDataReader());
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Calculates the price.
        /// </summary>
        public void CalculatePrice()
        {
            foreach (DataRow row in this.ComponentSubstitutes.Rows)
            {
                decimal price = PosApplication.Instance.Services.Price.GetItemPrice((string)row["ITEMID"], (string)row["UNIT"]);
                row["ITEMPRICE"] = PosApplication.Instance.Services.Rounding.RoundForDisplay(price, true, false);
            }
        }

        /// <summary>
        /// GetSubstituteComponentForMasterDimension
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns> Return DataRow object</returns>
        public string GetDisplayProductNumberForMasterItemDimensionSelection(long variantId)
        {
            return ItemData.GetDisplayProductNumber(variantId);
        }

        /// <summary>
        /// Determines if the current selected set of components represents a valid kit configuration
        /// </summary>
        /// <param name="components">Collection of components included in Kit configuration. Key: represents the component line recordid and  Value: represents the component productId.</param>
        /// <returns> Returns true if kit configuration is valid; otherwise returns false.</returns>
        public bool IsValidKitConfiguration(Dictionary<long, long> components)
        {
            using (DataTable dt = ItemData.GetKitConfigBasedVariantId(this.ConfirmationResult.KitItemId, components))
            {
                return (dt.Rows.Count == 1);
            }
        }
    }
}
