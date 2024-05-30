/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    /// <summary>
    /// Form for display the product details.
    /// </summary>
    [Export("ProductDetailsForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ProductDetailsForm : frmTouchBase, IInteractionView
    {
        /// <summary>
        /// Gets or sets the confirmation object.
        /// </summary>
        private ProductDetailsConfirmation ConfirmationResult { get; set; }

        /// <summary>
        /// Gets or sets the product details view model.
        /// </summary>
        private ProductDetailsViewModel ViewModel { get; set; }

        private const string ItemNumberColumName = "Number";
        private const string ItemNameColumnName = "Name";
        private const string QuantityColumnName = "QTY";
        private const string UnitOfMeasureColumnName = "UnitOfMeasure";
        private const string ComponentProductIDColumnName = "COMPONENTPRODUCTID";
        private const string ComponentLineRecIdColumnName = "DEFAULTCOMPONENTRECID";
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailsForm"/> class.
        /// </summary>
        private ProductDetailsForm()
        {
            InitializeComponent();

            this.ConfirmationResult = new ProductDetailsConfirmation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailsForm"/> class.
        /// </summary>
        /// <param name="productDetailsConfirmation">Product details confirmation.</param>
        [ImportingConstructor]
        public ProductDetailsForm(ProductDetailsConfirmation productDetailsConfirmation)
            : this()
        {
            if (productDetailsConfirmation == null)
            {
                throw new ArgumentNullException("productDetailsConfirmation");
            }

            if (string.IsNullOrWhiteSpace(productDetailsConfirmation.ItemNumber))
            {
                throw new ArgumentNullException("productDetailsConfirmation.ItemNumber");
            }

            if (string.IsNullOrWhiteSpace(productDetailsConfirmation.SourceContext))
            {
                throw new ArgumentNullException("productDetailsConfirmation.SourceContext");
            }

            this.ConfirmationResult.ItemNumber = productDetailsConfirmation.ItemNumber;
            this.ConfirmationResult.ItemVariant = productDetailsConfirmation.ItemVariant;
            this.ConfirmationResult.SourceContext = productDetailsConfirmation.SourceContext;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                bool isSourceSalesTransOrInventoryLookup = (this.ConfirmationResult.SourceContext.Equals(ProductDetailsSourceContext.SalesTransaction, StringComparison.OrdinalIgnoreCase) || this.ConfirmationResult.SourceContext.Equals(ProductDetailsSourceContext.InventoryLookup, StringComparison.OrdinalIgnoreCase));

                this.ViewModel = new ProductDetailsViewModel(this.ConfirmationResult.ItemNumber, this.ConfirmationResult.ItemVariant, isSourceSalesTransOrInventoryLookup);

                this.bindingSource.Add(this.ViewModel);

                TranslateLabels();

                this.UpdateNavigationButtonsForProductAttributes();

                // Check if KitRecID has greater than 0 value indicate that ItemNumber is a Kit. This is set in ProductDetailsViewModel constructor if it is a Kit Item. 
                if (this.ViewModel.IsKit)
                {
                    this.EnableKitControls();
                }

                if (this.ConfirmationResult.SourceContext.Equals(ProductDetailsSourceContext.ViewProductDetailsOperation, StringComparison.OrdinalIgnoreCase)) // If Source is from View Product Details Operation from cart page, then make Add To sale button visible = false;
                {
                    this.btnAddToSale.Visible = false;
                }

            }
            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            // Header
            this.Text = ApplicationLocalizer.Language.Translate(99801, this.ViewModel.SearchName); // "{0}" - Item name
            this.lblHeader.Text = this.Text; // "{0}" - Item name

            // Labels
            this.lblBarCode.Text = ApplicationLocalizer.Language.Translate(99802); // Bar code:
            this.lblSearchName.Text = ApplicationLocalizer.Language.Translate(99803); // Search name:
            this.lblModelNumber.Text = ApplicationLocalizer.Language.Translate(99894); // Model number:
            this.lblModelNumberValue.Text = ApplicationLocalizer.Language.Translate(99801, this.ViewModel.ItemNumber); // "{0}" - Item number;
            this.lblCategory.Text = ApplicationLocalizer.Language.Translate(99804); // Category:
            this.lblDescription.Text = ApplicationLocalizer.Language.Translate(99805); // Description:
            this.lblPrice.Text = ApplicationLocalizer.Language.Translate(99806); // Price:
            this.lblProductAttributes.Text = ApplicationLocalizer.Language.Translate(99811); // Product attributes:

            // Columns
            this.colName.Caption = ApplicationLocalizer.Language.Translate(99812); // Name
            this.colValue.Caption = ApplicationLocalizer.Language.Translate(99813); // Value
            this.colKitComponentNumber.Caption = ApplicationLocalizer.Language.Translate(99874); // Number (Kit Specific)
            this.colKitComponentName.Caption = ApplicationLocalizer.Language.Translate(99875); // Name (Kit Specific)
            this.colKitComponentQTY.Caption = ApplicationLocalizer.Language.Translate(99876); // Qty. (Kit Specific)
            this.colKitComponentUOM.Caption = ApplicationLocalizer.Language.Translate(99882); // Unit (Kit Specific)

            // Buttons
            this.btnAddToSale.Text = ApplicationLocalizer.Language.Translate(99814); // Add to sale
            this.btnSubstitute.Text = ApplicationLocalizer.Language.Translate(99877); // Substitute product (Kit specific)
            this.btnInventoryLookup.Text = ApplicationLocalizer.Language.Translate(99815); // Inventory lookup
            this.btnCancel.Text = ApplicationLocalizer.Language.Translate(99816); // Cancel
        }

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type.</typeparam>
        /// <param name="args">Notification.</param>
        public void Initialize<TArgs>(TArgs args)
             where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
        }

        /// <summary>
        /// Return the results of the interation call.
        /// </summary>
        /// <typeparam name="TResults">The confirmation result of this interaction.</typeparam>
        /// <returns>Returns the TResults object.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            this.ConfirmationResult.Confirmed = true;
            return this.ConfirmationResult as TResults;
        }

        #endregion

        private void OnBtnAddToSale_Click(object sender, EventArgs e)
        {
            this.ConfirmationResult.AddToSale = true;
            this.ConfirmationResult.ItemNumber = this.ViewModel.ItemNumber;

            if (gridKitComponents.Visible)
            {
                switch (this.ConfirmationResult.SourceContext.ToUpper())
                {
                    case ProductDetailsSourceContext.ItemSearch:
                        // Set item info in barcodeinfo and pass object to Runoperation(). This allows ItemSale operation 
                        // to automatically set this information with out prompting user for selecting dimensions, also 
                        // enables using the  kit component's qty and UOM instead of the default UOM
                        IBarcodeInfo barcodeInfo = PosApplication.Instance.BusinessLogic.Utility.CreateBarcodeInfo();
                        barcodeInfo.ItemId = this.ViewModel.ItemNumber;
                        barcodeInfo.VariantId = this.ViewModel.VariantId;
                        barcodeInfo.UnitId = this.ViewModel.UnitOfMeasure;
                        barcodeInfo.InternalType = BarcodeInternalType.Item;
                        barcodeInfo.EntryType = BarcodeEntryType.Selected;
                        this.ConfirmationResult.ResultData = barcodeInfo;
                        break;
                    case ProductDetailsSourceContext.SalesTransaction:
                    case ProductDetailsSourceContext.DisassembleKit:
                    case ProductDetailsSourceContext.InventoryLookup:
                        this.ConfirmationResult.ResultData = this.ViewModel.DimensionData;
                        break;
                    default:
                        this.ConfirmationResult.ResultData = null;
                        break;
                }
            }
            else
            {
                this.ConfirmationResult.ResultData = null;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnBtnInventoryLookup_Click(object sender, System.EventArgs e)
        {
            ViewModel.ShowInventoryLookup();
        }

        private void OnBtnPgUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrevPage();
        }

        private void OnBtnUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrev();
        }

        private void OnBtnPgDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNextPage();
        }

        private void OnBtnDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNext();
        }

        private void OnGridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            UpdateNavigationButtonsForProductAttributes();
        }

        private void UpdateNavigationButtonsForProductAttributes()
        {
            btnDown.Enabled = btnPgDown.Enabled = !gridView.IsLastRow;
            btnUp.Enabled = btnPgUp.Enabled = !gridView.IsFirstRow;
        }

        private void UpdateNavigationButtonsForKitComponents()
        {
            this.btnDownKitComponents.Enabled = this.btnPgDownKitComponents.Enabled = !gridKitComponentsView.IsLastRow;
            this.btnUpKitComponents.Enabled = this.btnPgUpKitComponents.Enabled = !gridKitComponentsView.IsFirstRow;

            // Enable or disable Substitue Button based on component with Substitue or not. 
            if (gridKitComponentsView.GetSelectedRows()[0] >= 0)
            {
                System.Data.DataRow Row = gridKitComponentsView.GetDataRow(gridKitComponentsView.GetSelectedRows()[0]);
                if (PosApplication.Instance.BusinessLogic.Utility.ToBool(Row["HASSUBSTITUTE"]))
                {
                    this.btnSubstitute.Enabled = true;
                }
                else
                {
                    this.btnSubstitute.Enabled = false;
                }
            }
        }

        private void btnSubstitute_Click(object sender, EventArgs e)
        {
            if (gridKitComponentsView.GetSelectedRows()[0] >= 0)
            {
                // Get the row index of the component that is about to be substituted by another product.
                int selectedComponentRowIndex = gridKitComponentsView.GetSelectedRows()[0];

                System.Data.DataRow Row = gridKitComponentsView.GetDataRow(selectedComponentRowIndex);
                long CurrentSelectionDefaultComponentRecId = (long)Row[ComponentLineRecIdColumnName];

                // Get the currently included components before substitution is made.
                // The dictionary currentKitComponentSelectionList contains the component's productId keyed in by the component line RecId.
                Dictionary<long, long> currentKitComponentSelectionList = new Dictionary<long, long>();
                foreach (DataRow row in this.ViewModel.KitConfigComponents.Rows)
                {
                    currentKitComponentSelectionList.Add((long)row[ComponentLineRecIdColumnName], (long)row[ComponentProductIDColumnName]);
                }

                // open the product substitution form.
                KitComponentSubstituteConfirmation substituteConfirmation = new KitComponentSubstituteConfirmation()
                {
                    SelectedComponentLineRecId = CurrentSelectionDefaultComponentRecId,
                    IncludedComponents = currentKitComponentSelectionList,
                    KitItemId = this.ConfirmationResult.ItemNumber
                };

                InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(substituteConfirmation, () => { });
                PosApplication.Instance.Services.Interaction.InteractionRequest(request);
                KitComponentSubstituteConfirmation confirmation = request.Context as KitComponentSubstituteConfirmation;
                if (confirmation.Confirmed)
                {
                    var utility = PosApplication.Instance.BusinessLogic.Utility;
                    DataRow ResultReturned = (DataRow)confirmation.ResultData;

                    // Get the row which has the component the user just substituted and replace the existing product info 
                    // with the substitute product info
                    DataRow row = this.ViewModel.KitConfigComponents.Rows[selectedComponentRowIndex];

                    row[ItemNumberColumName] = utility.ToString(ResultReturned[ItemNumberColumName]);
                    row[ItemNameColumnName] = utility.ToString(ResultReturned[ItemNameColumnName]);
                    row[QuantityColumnName] = utility.ToDecimal(ResultReturned[QuantityColumnName]);
                    row[UnitOfMeasureColumnName] = utility.ToString(ResultReturned[UnitOfMeasureColumnName]);
                    row[ComponentProductIDColumnName] = ResultReturned[ComponentProductIDColumnName];

                    // Get the kitvariant and price for the current included components and update the viewmodel and also update image, barcode and price for kit variant. 
                    this.ViewModel.GetRetailKitVariantIDOnCurrentSelection(true);
                    lblPriceValue.Text = this.ViewModel.Price;
                    this.lblBarCodeValue.Text = this.ViewModel.Barcode;
                }
            }
        }

        /// <summary>
        /// DisplayKitControls
        /// </summary>
        private void EnableKitControls()
        {
            // Enable all Kit specific controls to display
            this.tblKitComponents.Visible = true;
            this.lblKitComponents.Visible = true;
            this.lblKitComponents.Text = ApplicationLocalizer.Language.Translate(99873); // Changed Text to "Included Products" for Kits.
            this.gridKitComponents.Visible = true;
            this.btnSubstitute.Visible = true;
            this.btnDownKitComponents.Visible = true;
            this.btnPgDownKitComponents.Visible = true;
            this.btnUpKitComponents.Visible = true;
            this.btnPgUpKitComponents.Visible = true;

            // By default very first time disable substitute button
            this.btnSubstitute.Enabled = false;

            // If Source form is Disassemble Kit then do not show "Check Inventory" button as well as change text of "Add to sale" button to "Disassemble". 
            if (this.ConfirmationResult.SourceContext.Equals(ProductDetailsSourceContext.DisassembleKit, StringComparison.OrdinalIgnoreCase) || this.ConfirmationResult.SourceContext.Equals(ProductDetailsSourceContext.InventoryLookup, StringComparison.OrdinalIgnoreCase))
            {
                this.btnInventoryLookup.Visible = false;
                this.btnAddToSale.Text = ApplicationLocalizer.Language.Translate(99898); // OK
            }

            gridKitComponents.DataSource = this.ViewModel.KitConfigComponents;

            this.UpdateNavigationButtonsForKitComponents();
        }

        private void btnPgUpKitComponents_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MovePrevPage();
        }

        private void btnUpKitComponents_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MovePrev();
        }

        private void btnDownKitComponents_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MoveNext();
        }

        private void btnPgDownKitComponents_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MoveNextPage();
        }

        private void gridKitComponentsView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            UpdateNavigationButtonsForKitComponents();
        }
    }
}
