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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    /// <summary>
    /// Form for display Kit Component Sustitute Lists
    /// </summary>
    [Export("KitComponentSubstituteView", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class KitComponentSubstituteForm : frmTouchBase, IInteractionView
    {
        /// <summary>
        /// Gets or sets the Kit Component Substitute view model.
        /// </summary>
        private KitComponentSubstituteViewModel ComponentSubstituteViewModel { get; set; }

        #region Constructor

        /// <summary>
        /// Creates the Kit Component Sustitute Form and 
        /// initializes the Kit Component Sustitute view model object
        /// </summary>
        public KitComponentSubstituteForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KitComponentSubstituteForm"/> class.
        /// </summary>
        /// <param name="kitComponentSubstituteConfirmation">Kit component substitute confirmation.</param>
        /// 
        [ImportingConstructor]
        public KitComponentSubstituteForm(KitComponentSubstituteConfirmation kitComponentSubstituteConfirmation)
            : this()
        {
            if (kitComponentSubstituteConfirmation == null)
            {
                throw new ArgumentNullException("KitComponentSubstituteConfirmation");
            }

            ComponentSubstituteViewModel = new KitComponentSubstituteViewModel(kitComponentSubstituteConfirmation);
            grKitProductSubstitute.DataSource = ComponentSubstituteViewModel.ComponentSubstitutes;
        }
        #endregion

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type</typeparam>
        /// <param name="args">Notification</param>
        public void Initialize<TArgs>(TArgs args)
            where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call
        /// </summary>
        /// <typeparam name="TResults"></typeparam>
        /// <returns>Returns the TResults object</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            this.ComponentSubstituteViewModel.ConfirmationResult.Confirmed = this.DialogResult == DialogResult.OK;
            return this.ComponentSubstituteViewModel.ConfirmationResult as TResults;
        }
        #endregion

        /// <summary>
        /// Executing OnLoad of Form KitComponentSubstituteForm 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (grdKitProductSubstituteView.DataRowCount > 0)
                {
                    btnSelect.Enabled = true;
                }   
                else
                {
                    btnSelect.Enabled = false;
                }
                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            // Labels
            this.lblHeading.Text = ApplicationLocalizer.Language.Translate(99877); // Change products
            this.lblProducts.Text = ApplicationLocalizer.Language.Translate(99878); // Products

            // Columns
            this.colItemId.Caption = ApplicationLocalizer.Language.Translate(99879); // Item number
            this.colItemName.Caption = ApplicationLocalizer.Language.Translate(99880); // Item name
            this.colQuantity.Caption = ApplicationLocalizer.Language.Translate(99892); // Quantity
            this.colItemUnitOfMeasure.Caption = ApplicationLocalizer.Language.Translate(99893); // Unit of Measure
            this.colItemPrice.Caption = ApplicationLocalizer.Language.Translate(99881); // Price

            // Buttons
            this.btnSelect.Text = ApplicationLocalizer.Language.Translate(99883); // OK
            this.btnGetprice.Text = ApplicationLocalizer.Language.Translate(99884); // Show price
            this.btnCancel.Text = ApplicationLocalizer.Language.Translate(99885); // Cancel
        }

        /// <summary>
        /// Show/Hide Price for a substitute product list. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetprice_Click(object sender, EventArgs e)
        {
            SetPriceVisibility(!colItemPrice.Visible);
        }

        /// <summary>
        /// Sets the price visibility.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        private void SetPriceVisibility(bool visible)
        {
            colItemPrice.Visible = visible;
            
            // Make price column next to name column.
            if (colItemPrice.Visible)
            {
                colItemPrice.VisibleIndex = colItemName.VisibleIndex + 1;
            }

            // Update button text
            btnGetprice.Text = ApplicationLocalizer.Language.Translate(colItemPrice.Visible
                                                                            ? 99896 /* "Hide price" */
                                                                            : 99884 /* "Show price" */);
        }

        /// <summary>
        /// Select Substitute list when user Click on Select button and return to Product Details Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            grdKitProductSubstituteView_DoubleClick(sender, e);
        }

        /// <summary>
        /// Select Substitute when user Double Click on selected substitue in the grid and return to ProductDetailsForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdKitProductSubstituteView_DoubleClick(object sender, EventArgs e)
        {
            Boolean IsDimensionPickerCancelled = false;
            System.Data.DataRow Row = grdKitProductSubstituteView.GetDataRow(grdKitProductSubstituteView.GetSelectedRows()[0]);
            if (PosApplication.Instance.BusinessLogic.Utility.ToBool(Row["ISMASTERPRODUCT"]))
            {
                string MasterId = PosApplication.Instance.BusinessLogic.Utility.ToString(Row["ItemID"]);
                using (DataTable inventDimCombination = PosApplication.Instance.Services.Dimension.GetDimensions(MasterId))
                {
                    if (inventDimCombination.Rows.Count > 0)
                    {
                        DimensionConfirmation dimensionConfirmation = new DimensionConfirmation()
                        {
                            InventDimCombination = inventDimCombination,
                            DimensionData = new Dimensions(),
                            DisplayDialog = true
                        };

                        InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(dimensionConfirmation, () =>
                        {
                            if (dimensionConfirmation.Confirmed)
                            {
                                //If a valid selection combination of config was made.
                                if (dimensionConfirmation.SelectDimCombination != null)
                                {
                                    DataRow dr = dimensionConfirmation.SelectDimCombination;
                                    long ProductVariant = (long)dr["DISTINCTPRODUCTVARIANT"];

                                    if (ProductVariant > 0) 
                                    {
                                        if (this.ComponentSubstituteViewModel.KitComponentProductRecId == ProductVariant)
                                        {
                                            Row["QTY"] = this.ComponentSubstituteViewModel.KitComponentQty;
                                            Row["UnitOfMeasure"] = this.ComponentSubstituteViewModel.KitComponentUOM;
                                        }
                                        else
                                        {
                                            DataRow[] matchingVariantSubstitute = this.ComponentSubstituteViewModel.ComponentSubstitutes.Select(string.Format("COMPONENTPRODUCTID = {0}", ProductVariant));

                                            // Check if substitute variant with same variant Id in the Kit component substitute list, if yes then copy Qty and Unit of measure. 
                                            if ((matchingVariantSubstitute != null) && (matchingVariantSubstitute.Length == 1) && (matchingVariantSubstitute[0]["QTY"] != Row["QTY"]))
                                            {
                                                Row["QTY"] = matchingVariantSubstitute[0]["QTY"];
                                                Row["UnitOfMeasure"] = matchingVariantSubstitute[0]["UnitOfMeasure"];
                                            }
                                        }

                                        Row["COMPONENTPRODUCTID"] = ProductVariant;
                                        Row["NUMBER"] = this.ComponentSubstituteViewModel.GetDisplayProductNumberForMasterItemDimensionSelection(ProductVariant);
                                        Row["ISMASTERPRODUCT"] = false;
                                        Row["ISDEFAULTCOMPONENT"] = false;

                                        this.ComponentSubstituteViewModel.ConfirmationResult.ResultData = Row;
                                    }
                                }
                            }
                            else
                            {
                                IsDimensionPickerCancelled = true;
                            }
                        }
                        );
                        PosApplication.Instance.Services.Interaction.InteractionRequest(request);
                    }
                }
            }
            else
            {
                this.ComponentSubstituteViewModel.ConfirmationResult.ResultData = Row;
            }

            // Validate that the selected list of components represent a kit variant and that all its component products are assigned to the current store
            // If user cancels the dimension picker, it means no substitute has been selected, hence skip validation and stay on the form
            if (!IsDimensionPickerCancelled)
            {
                // Create the current set of selected component ids, replacing the original component being substituted with the selected substitute
                long index = this.ComponentSubstituteViewModel.ConfirmationResult.SelectedComponentLineRecId;
                this.ComponentSubstituteViewModel.ConfirmationResult.IncludedComponents[index] = (long)Row["COMPONENTPRODUCTID"];

                // If the user's selection represent valid kit configuration whose component products are all assigned to the store then confirm the result is obtained and close the form
                if (this.ComponentSubstituteViewModel.IsValidKitConfiguration(this.ComponentSubstituteViewModel.ConfirmationResult.IncludedComponents))
                {
                    this.ComponentSubstituteViewModel.ConfirmationResult.Confirmed = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // One or more product substitutes are not available. Select a different product substitute.
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(99900);
                }
            }
        }

        /// <summary>
        /// Move to Previous Page when user clicks on Page Up button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPgUp_Click(object sender, EventArgs e)
        {
            grdKitProductSubstituteView.MovePrevPage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            grdKitProductSubstituteView.MovePrev();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            grdKitProductSubstituteView.MoveNext(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPgDown_Click(object sender, EventArgs e)
        {
            grdKitProductSubstituteView.MoveNextPage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdKitProductSubstituteView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            btnDown.Enabled = btnPgDown.Enabled = !grdKitProductSubstituteView.IsLastRow;
            btnUp.Enabled = btnPgUp.Enabled = !grdKitProductSubstituteView.IsFirstRow;
        }
    }
}
