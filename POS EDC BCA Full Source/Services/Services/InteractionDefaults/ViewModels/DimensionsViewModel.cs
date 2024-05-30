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
using LSRetailPosis.POSProcesses.ViewModels;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    internal enum DimensionType
    {
        Color  = 0,
        Size   = 1,
        Style  = 2,
        Config = 3
    }

    /// <summary>
    /// Binding model for dimensions form that is independent of the UI
    /// </summary>
    internal sealed class DimensionsViewModel : INotifyPropertyChanged
    {
        private readonly int DIMENSIONTYPECOUNT = Enum.GetValues(typeof(DimensionType)).Length;

        // Tracks the current dimension
        private DimensionType? currentDimension;

        // Collection of all variants for all dimensions
        private Collection<DimensionValue> allVariants;

        // Collection of the active dimensions and the selected variant for each dimension (e.g. an item might have only color and size)
        private Dictionary<DimensionType, string> activeDimensions;

        // Event fires when selections have been made for all dimensions
        public event EventHandler Finished;

        public DimensionsViewModel(DataTable inventDimCombination)
        {
            allVariants      = new Collection<DimensionValue>();
            activeDimensions = new Dictionary<DimensionType, string>(DIMENSIONTYPECOUNT);

            // Colors
            LoadVariants(inventDimCombination, DimensionType.Color, "COLORS", "COLORID", "COLOR", "COLORDISPLAYORDER");

            // Size
            LoadVariants(inventDimCombination, DimensionType.Size, "SIZE", "SIZEID", "SIZE", "SIZEDISPLAYORDER");

            // Style
            LoadVariants(inventDimCombination, DimensionType.Style, "STYLE", "STYLEID", "STYLE", "STYLEDISPLAYORDER");            

            // Config
            LoadVariants(inventDimCombination, DimensionType.Config, "CONFIG", "CONFIGID", "CONFIG", "");

            // Set the next dimension
            NextDimension();
        }

        private void LoadVariants(DataTable dataTable, DimensionType dimType, string tableName, string dimensionId, string dimensionName, string dimensionDisplayOrder)
        {
            using (DataTable dt = SelectDistinct(tableName, dataTable, string.Empty, dimensionId, dimensionName, dimensionDisplayOrder))
            {
                DataRow[] rows = dt.Select(string.Empty);
                if (rows.Length > 0)
                {
                    activeDimensions.Add(dimType, null);

                    foreach (DataRow row in rows)
                    {
                        DimensionValue dv = new DimensionValue(row["ID"].ToString(), row["NAME"].ToString(), dimType,
                            string.IsNullOrWhiteSpace(dimensionDisplayOrder) ? 0 : Convert.ToDecimal(row["DISPLAYORDER"].ToString()));
                        allVariants.Add(dv);
                    }
                }
            }
        }

        /// <remarks>Caller is responsible for disposing returned object</remarks>
        /// <remarks>Unchanged legacy code</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private static DataTable SelectDistinct(string tableName, DataTable sourceTable, string selectString, string dimensiondId, string dimensionName, string dimensionDisplayOrder)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add("ID", sourceTable.Columns[dimensiondId].DataType);
            dt.Columns.Add("NAME", sourceTable.Columns[dimensionName].DataType);
            dt.Columns.Add("VARIANTID", sourceTable.Columns["VARIANTID"].DataType);
            if (!string.IsNullOrWhiteSpace(dimensionDisplayOrder))
            {
                dt.Columns.Add("DISPLAYORDER", sourceTable.Columns[dimensionDisplayOrder].DataType);
            }

            object lastValue   = null;
            object lastName    = null;
            object lastVariant = null;
            object lastDisplayOrder = null;
            foreach (DataRow dr in sourceTable.Select(selectString, dimensiondId))
            {
                string idValue = dr[dimensiondId] as string;
                if (!string.IsNullOrEmpty(idValue) && (lastValue == null || !(ColumnEqual(lastValue, dr[dimensiondId]))))
                {
                    lastValue        = dr[dimensiondId];
                    lastName         = dr[dimensionName];
                    lastVariant      = dr["VARIANTID"];
                    if (!string.IsNullOrWhiteSpace(dimensionDisplayOrder))
                    {
                        lastDisplayOrder = dr[dimensionDisplayOrder];

                        dt.Rows.Add(lastValue, lastName, lastVariant, lastDisplayOrder);
                    }
                    else
                    {
                        dt.Rows.Add(lastValue, lastName, lastVariant);
                    }
                }
            }

            return dt;
        }

        /// <remarks>Unchanged legacy code</remarks>
        private static bool ColumnEqual(object A, object B)
        {
            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }

        /// <summary>
        /// Indicates whether a dimension is active (enabled)
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public bool Exists(DimensionType dim)
        {
            return activeDimensions.ContainsKey(dim);
        }

        /// <summary>
        /// Returns the active color dimension Id if selected, otherwise null.
        /// </summary>
        public string ActiveColorId
        {
            get
            {
                return activeDimensions.ContainsKey(DimensionType.Color) ? activeDimensions[DimensionType.Color] : null;
            }
        }

        /// <summary>
        /// Returns the active size dimension Id if selected, otherwise null.
        /// </summary>
        public string ActiveSizeId
        {
            get
            {
                return activeDimensions.ContainsKey(DimensionType.Size) ? activeDimensions[DimensionType.Size] : null;
            }
        }

        /// <summary>
        /// Returns the active style dimension Id if selected, otherwise null.
        /// </summary>
        public string ActiveStyleId
        {
            get
            {
                return activeDimensions.ContainsKey(DimensionType.Style) ? activeDimensions[DimensionType.Style] : null;
            }
        }

        /// <summary>
        /// Returns the active config dimension Id if selected, otherwise null.
        /// </summary>
        public string ActiveConfigId
        {
            get
            {
                return activeDimensions.ContainsKey(DimensionType.Config) ? activeDimensions[DimensionType.Config] : null;
            }
        }

        /// <summary>
        /// Returns the all variants for all dimensions
        /// </summary>
        public Collection<DimensionValue> AllVariants
        {
            get
            {
                return this.allVariants;
            }
        }

        /// <summary>
        /// Gets or sets the current dimension
        /// </summary>
        public DimensionType CurrentDimension
        {
            get
            {
                return this.currentDimension.Value;
            }
            set
            {
                if (!currentDimension.HasValue || currentDimension.Value != value)
                {
                    currentDimension = value;
                    OnPropertyChanged("CurrentDimension");
                }
            }
        }

        private void NextDimension()
        {
            this.CurrentDimension = FindNextDimension();
        }

        /// <summary>
        /// Sets the selected variant for the current dimension e.g. "Red" was selected for Color.
        /// </summary>
        /// <param name="selectedId">ID of selected variant</param>
        public void SetSelectedId(string selectedId)
        {
            activeDimensions[this.CurrentDimension] = selectedId;

            if (IsFinished())
            {
                // Fire Finished event
                this.Finished(this, EventArgs.Empty);
            }
            else
            {
                // Switch to the next dimension
                NextDimension();
            }
        }

        /// <summary>
        /// Indicates whether all selections have been made for all active dimensions
        /// </summary>        
        public bool IsFinished()
        {
            bool finished = true;

            // Loop through each key and see if we have a value
            foreach (var key in activeDimensions.Keys)
            {
                if (activeDimensions[key] == null)
                {
                    finished = false;
                    break;
                }
            }

            return finished;
        }

        /// <summary>
        /// Determines the next active dimension
        /// </summary>        
        private DimensionType FindNextDimension()
        {
            // We use the order of values as defined in the enum
            DimensionType? next = null;

            // Start at the current index or -1 if there is no index yet
            int startIndex = this.currentDimension.HasValue ? (int)this.currentDimension.Value : -1;
            int maxDelta = this.currentDimension.HasValue ? DIMENSIONTYPECOUNT - 1 : DIMENSIONTYPECOUNT;
            
            // Find the next active dimension. i has to be less than (or equal) maxDelta, as we do not want to come back to the current dimension.
            for (int i = 1; i <= maxDelta; i++)
            {
                int newIndex = (startIndex + i) % DIMENSIONTYPECOUNT;
                DimensionType dim = (DimensionType)newIndex;

                // See if the dimension is active and unpopulated
                if (activeDimensions.ContainsKey(dim) && activeDimensions[dim] == null)
                {
                    next = dim;
                    break;
                }
            }

            if (!next.HasValue)
                throw new InvalidOperationException("No active dimensions found");

            return next.Value;
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
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("Dimension={Dimension}, Name={Name}, Id={Id}")]
    internal class DimensionValue
    {
        public DimensionValue(string id, string name, DimensionType dimension, decimal displayOrder)
        {
            this.Id             = id;
            this.Name           = name;
            this.Dimension      = dimension;
            this.DisplayOrder   = displayOrder;
        }

        public string Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public DimensionType Dimension
        {
            get;
            private set;
        }
 
        public decimal DisplayOrder
        {
            get;
            private set;
        }

        /// <summary>
        /// Compares dimension values for sorting purpose. Sorting is made by display order, and then by name.
        /// Values with zero display orders should be at the beginning of list.
        /// E.g. we have the following values:
        /// Name = Black, DisplayOrder = 3
        /// Name = Silver, DisplayOrder = 0
        /// Name = White, DisplayOrder = 1
        /// Name = Gold, DisplayOrder = 2
        /// Name = Red, DisplayOrder = 0
        /// Result sorted list should be: Red(0), Silver(0), White (1), Gold (2), Black (3).
        /// </summary>
        /// <returns></returns>
        internal static int CompareDisplayOrders(DimensionValue v1, DimensionValue v2)
        {
            // values with different dimension types (e.g. color vs style) should not be compared by this comparison
            if (v1.Dimension != v2.Dimension)
            {
                return 0;
            }
            
            // if display orders are equal, compare by name
            if (v1.DisplayOrder == v2.DisplayOrder)
            {
                return v1.Name.CompareTo(v2.Name);
            }

            // in all other cases common compare by display order values
            return v1.DisplayOrder.CompareTo(v2.DisplayOrder);
        }
   }
}
