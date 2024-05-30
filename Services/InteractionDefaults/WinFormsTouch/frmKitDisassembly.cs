/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraGrid;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Classes needed for custom grid button workaround")]
    [Export("KitDisassemblyView", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmKitDisassembly : frmTouchBase, IInteractionView
    {
        private const string ColQtyToCart = "QTYTOCART";
        private const int ButtonMargin = 3;

        #region Fields

        /// <summary>
        /// Gets or sets the kit disassembly view model.
        /// </summary>
        private KitDisassemblyViewModel kitDisassemblyViewModel;

        /// <summary>
        /// set flag to true to surpress the dimension picker form when specific 
        /// variant is already identified from a barcode
        /// </summary>
        private bool surpressVariantPickerForm = false;

        // Variables for the Add to cart quantity custom button
        private int qtyToCartPressedRowHandle = GridControl.InvalidRowHandle;
        private int qtyToCartHighlightedRowHandle = GridControl.InvalidRowHandle;

        #endregion

        #region Ctr
        /// <summary>
        /// Creates the kit disassembly form and 
        /// initializes the kit disassembly view model object
        /// </summary>
        public frmKitDisassembly()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                kitDisassemblyViewModel = new KitDisassemblyViewModel();
                kitDisassemblyViewModel.PropertyChanged += new PropertyChangedEventHandler(OnViewModel_PropertyChanged);
                txtKitQtyInput.Validating += txtKitQtyInput_Validating;
                gridKitComponents.DataSource = kitDisassemblyViewModel.Components;
                this.btnOk.Enabled = false;
                txtKitIdInput.Select();
            }
            base.OnLoad(e);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearKit();
        }

        /// <summary>
        /// Clear Kit Control values.. 
        /// </summary>
        private void ClearKit()
        {
            this.kitDisassemblyViewModel.Clear();
            txtKitIdInput.Text = string.Empty;
            txtKitQtyInput.Text = string.Empty;
            txtKitIdInput.Select();
            this.btnOk.Enabled = false;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MoveNext();
        }

        private void btnGetKit_Click(object sender, EventArgs e)
        {
            this.txtKitIdInput_EnterButtonPressed();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(kitDisassemblyViewModel.KitID)
                && !string.IsNullOrWhiteSpace(kitDisassemblyViewModel.KitVariantID))
            {
                if (!kitDisassemblyViewModel.DisassemblAtRegister)
                {
                    POSFormsManager.ShowPOSMessageDialog(99868); // This Kit can't be disassembled at a register
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                POSFormsManager.ShowPOSMessageDialog(99866); //Please enter a valid Kit ID to perform a kit disassembly operation
            }
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MoveNextPage();
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MovePrevPage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gridKitComponentsView.MovePrev();
        }

        private void txtKitIdInput_EnterButtonPressed()
        {
            if (string.IsNullOrWhiteSpace(txtKitIdInput.Text))
            {
                POSFormsManager.ShowPOSMessageDialog(99866); //Please enter a valid Kit ID to perform a disassembly operation.
                txtKitIdInput.Text = kitDisassemblyViewModel.KitID;
            }
            else
            {
                // Process barcode
                IScanInfo scanInfo = PosApplication.Instance.BusinessLogic.Utility.CreateScanInfo();
                scanInfo.ScanDataLabel = txtKitIdInput.Text;

                IBarcodeInfo barcodeInfo = PosApplication.Instance.Services.Barcode.ProcessBarcode(scanInfo);
                if (barcodeInfo.Found && barcodeInfo.InternalType == BarcodeInternalType.Item)
                {
                    if (kitDisassemblyViewModel.DoesKitExist(barcodeInfo.ItemId))
                    {
                        if (!string.IsNullOrWhiteSpace(barcodeInfo.VariantId))
                        {
                            surpressVariantPickerForm = true; // Surpress dimension picker because barcode has distinct variant of a kit
                            kitDisassemblyViewModel.KitID = barcodeInfo.ItemId;
                            kitDisassemblyViewModel.KitVariantID = barcodeInfo.VariantId;
                            surpressVariantPickerForm = false; // Reset the surpress dimension picker flag
                            this.btnOk.Enabled = true; // Enable Disassemble button when config is already selected using barcode or variant ID. 
                        }
                        else
                        {
                            kitDisassemblyViewModel.KitID = barcodeInfo.ItemId;
                        }
                    }
                    else
                    {
                        POSFormsManager.ShowPOSMessageDialog(99866); //Please enter a valid Kit ID to perform a disassembly operation.
                        txtKitIdInput.Text = kitDisassemblyViewModel.KitID;
                    }
                }
                // if barcode is not found check if text represents kitid 
                else if (kitDisassemblyViewModel.DoesKitExist(barcodeInfo.BarcodeId))
                {
                    kitDisassemblyViewModel.KitID = txtKitIdInput.Text;
                }
                // if text is not barcode or kitid show error message
                else
                {
                    POSFormsManager.ShowPOSMessageDialog(99866); //Please enter a valid Kit ID to perform a disassembly operation.
                    txtKitIdInput.Text = kitDisassemblyViewModel.KitID;
                }
            }
        }

        void txtKitQtyInput_Validating(object sender, CancelEventArgs e)
        {
            UpdateKitQuantity();
        }

        /// <summary>
        /// Update the kit quantity to disassemble.
        /// </summary>
        private void UpdateKitQuantity()
        {
            int qtyVal;
            if (int.TryParse(txtKitQtyInput.Text, out qtyVal) && (qtyVal > 0))
            {
                kitDisassemblyViewModel.KitQuantity = qtyVal;
            }
            else
            {
                POSFormsManager.ShowPOSMessageDialog(99865); //The kit quantity is not valid. Enter a valid quantity, and then try again.
                txtKitQtyInput.Text = kitDisassemblyViewModel.KitQuantity.ToString();
            }
        }

        /// <summary>
        /// Listens for changes on the view model
        /// </summary>        
        private void OnViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "KitID":
                    // This will set the KitVariantID
                    SelectKitConfiguration();
                    break;
                case "KitVariantID":
                    // Change in KitVarianId mean the grid datasource is repopulated in the view model,
                    // hence the grid needs to be refreshed
                    gridKitComponents.Refresh();
                    break;
                case "Description":
                    lblKitDescriptionVal.Text = kitDisassemblyViewModel.Description;
                    break;
                case "KitName":
                    lblKitNameVal.Text = kitDisassemblyViewModel.KitName;
                    break;
                case "KitQuantity":
                    txtKitQtyInput.Text = kitDisassemblyViewModel.KitQuantity == 0 ? string.Empty : kitDisassemblyViewModel.KitQuantity.ToString();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Shows a UI form to select the configuration of the kit
        /// </summary>
        private void SelectKitConfiguration()
        {
            if (!string.IsNullOrWhiteSpace(kitDisassemblyViewModel.KitID) && !surpressVariantPickerForm)
            {
                InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                    new ProductDetailsConfirmation() { ItemNumber = kitDisassemblyViewModel.KitID, SourceContext = ProductDetailsSourceContext.DisassembleKit }, () => { });
                PosApplication.Instance.Services.Interaction.InteractionRequest(request);
                ProductDetailsConfirmation confirmation = request.Context as ProductDetailsConfirmation;
                if (confirmation != null && confirmation.Confirmed && confirmation.AddToSale && (confirmation.ResultData != null))
                {
                    Dimensions dimensionResultData = (Dimensions)confirmation.ResultData;
                    kitDisassemblyViewModel.KitVariantID = dimensionResultData.VariantId;
                    this.btnOk.Enabled = true;
                    txtKitQtyInput.Select();
                }
                else
                {
                    // Configuration selection was canceled - clear the kit information and retain the KitId to allow searching again
                    String kitId = txtKitIdInput.Text;
                    ClearKit();
                    txtKitIdInput.Text = kitId;
                    txtKitIdInput.Select();
                }
            }
        }

        private void TranslateLabels()
        {
            this.lblHeader.Text = ApplicationLocalizer.Language.Translate(99850);  //"Disassemble kit"
            this.lblKitIdQuery.Text = ApplicationLocalizer.Language.Translate(99851);  //"Kit ID"
            this.lblKitName.Text = ApplicationLocalizer.Language.Translate(99852);  //"Kit name"
            this.lblDescription.Text = ApplicationLocalizer.Language.Translate(99853);  //"Description"
            this.lblKitQty.Text = ApplicationLocalizer.Language.Translate(99854);  //"Quantity"

            this.componentIdCol.Caption = ApplicationLocalizer.Language.Translate(99862);  //"Product number"
            this.nameCol.Caption = ApplicationLocalizer.Language.Translate(99861);  //"Name"
            this.descriptionCol.Caption = ApplicationLocalizer.Language.Translate(99860);  //"Description"
            this.quantityCol.Caption = ApplicationLocalizer.Language.Translate(99855);  //"Total quantity"
            this.addToCartQtyCol.Caption = ApplicationLocalizer.Language.Translate(99856);  //"Add to cart"
            this.qtyToInventoryCol.Caption = ApplicationLocalizer.Language.Translate(99857);  //"Return to inventory"
            this.unitCol.Caption = ApplicationLocalizer.Language.Translate(99869);  //"Unit"

            this.btnSearchKit.Text = ApplicationLocalizer.Language.Translate(99864);   //"Search"
            this.btnOk.Text = ApplicationLocalizer.Language.Translate(99897);   //"Disassemble"
            this.btnClose.Text = ApplicationLocalizer.Language.Translate(99859);   // "Cancel"
        }

        /// <summary>
        /// Shows a numpad for entering quantity value when "Add to cart" field button is selected
        /// </summary>
        private void OnQtyToCartButtonClick()
        {
            bool inputNeeded;
            do
            {
                inputNeeded = false;
                using (frmInputNumpad inputDialog = new frmInputNumpad())
                {
                    inputDialog.EntryTypes = NumpadEntryTypes.Quantity;
                    inputDialog.PromptText = LSRetailPosis.ApplicationLocalizer.Language.Translate(103111);   // "Enter quantity"

                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(inputDialog);

                    if (inputDialog.DialogResult == DialogResult.OK)
                    {
                        // Set "Add to cart" field value to that of the numpad input
                        decimal subTotal;
                        if (decimal.TryParse(inputDialog.InputText, out subTotal))
                        {
                            int rowIndex = gridKitComponentsView.GetFocusedDataSourceRowIndex();

                            // Update quantity for row
                            if (!this.kitDisassemblyViewModel.ExecuteUpdateAddToCartQuantity(rowIndex, subTotal))
                            {
                                POSFormsManager.ShowPOSMessageDialog(99849); //The quantity is not valid. Enter a valid quantity, and then try again.
                                inputNeeded = true;
                            }
                        }
                    }
                }
            }
            while (inputNeeded);
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
            KitDisassemblyConfirmation result = new KitDisassemblyConfirmation
            {
                KitId = kitDisassemblyViewModel.KitID,
                KitVariantId = kitDisassemblyViewModel.KitVariantID,
                KitQuantity = kitDisassemblyViewModel.KitQuantity,
                Confirmed = this.DialogResult == DialogResult.OK
            };
            result.CartItems = kitDisassemblyViewModel.GetCartItems();
            return result as TResults;
        }

        #endregion

        private void txtKitIdInput_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    txtKitIdInput_EnterButtonPressed();
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void txtKitQtyInput_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    UpdateKitQuantity();
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        #region Custom grid button code

        /// <summary>
        /// Shows a custom cell button for the "Add to cart" field
        /// </summary>
        private void gridKitComponentsView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string column = e.Column.FieldName;

            if (column == ColQtyToCart)
            {
                e.Appearance.FillRectangle(e.Cache, e.Bounds);
                DrawButton(e.Cache, e.Bounds, gridKitComponents.LookAndFeel.ActiveLookAndFeel.ActiveStyle, e.Appearance, GetButtonState(e.RowHandle, column), e.DisplayText);

                e.Handled = true;
            }
        }

        private void DrawButton(GraphicsCache cache, Rectangle bounds, ActiveLookAndFeelStyle lookAndFeel, AppearanceObject appearance, ObjectState state, string caption)
        {
            EditorButtonObjectInfoArgs args = new EditorButtonObjectInfoArgs(cache, Button, appearance);
            BaseLookAndFeelPainters painters = LookAndFeelPainterHelper.GetPainter(lookAndFeel);

            // Create some margin
            bounds.Inflate(-ButtonMargin, -ButtonMargin);

            args.Bounds = bounds;
            args.State = state;

            painters.Button.DrawObject(args);

            // Draw the text 
            painters.Button.DrawCaption(args, caption, appearance.Font, gridKitComponentsView.Appearance.HeaderPanel.GetForeBrush(cache), args.Bounds, appearance.GetStringFormat(appearance.GetTextOptions()));
        }

        protected ObjectState GetButtonState(int rowHandle, string column)
        {
            int pressedRowHandle = QtyToCartPressedRowHandle;
            int highlightedRowHandle = QtyToCartHighlightedRowHandle;

            ObjectState buttonState = ObjectState.Normal;

            if (rowHandle == pressedRowHandle)
            {
                buttonState = ObjectState.Pressed;
            }
            else if (rowHandle == highlightedRowHandle
                    || (gridKitComponents.IsFocused 
                        && (gridKitComponentsView.FocusedColumn.FieldName == column) 
                        && (gridKitComponentsView.FocusedRowHandle == rowHandle)))
            {
                // Show hot if row is highlighted or cell is focused
                buttonState = ObjectState.Hot;
            }

            return buttonState;
        }

        private DevExpress.XtraEditors.Controls.EditorButton button;

        private DevExpress.XtraEditors.Controls.EditorButton Button
        {
            get
            {
                if (button == null)
                {
                    button = new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK);
                }

                return button;
            }
        }

        private int QtyToCartPressedRowHandle
        {
            get { return qtyToCartPressedRowHandle; }
            set
            {
                if (qtyToCartPressedRowHandle != value)
                {
                    SetPressedRowHandle(ref qtyToCartPressedRowHandle, ColQtyToCart, value);

                    gridKitComponentsView.InvalidateRowCell(qtyToCartPressedRowHandle, gridKitComponentsView.Columns[ColQtyToCart]);
                }
            }
        }

        private int QtyToCartHighlightedRowHandle
        {
            get { return qtyToCartHighlightedRowHandle; }
            set
            {
                if (qtyToCartHighlightedRowHandle != value)
                {
                    SetHighlightedRowHandle(ref qtyToCartHighlightedRowHandle, ColQtyToCart, value);

                    gridKitComponentsView.InvalidateRowCell(qtyToCartHighlightedRowHandle, gridKitComponentsView.Columns[ColQtyToCart]);
                }
            }
        }

        private void SetPressedRowHandle(ref int rowHandle, string column, int value)
        {
            if (rowHandle != GridControl.InvalidRowHandle)
            {
                int tempRowHandle = rowHandle;
                rowHandle = GridControl.InvalidRowHandle;
                gridKitComponentsView.InvalidateRowCell(tempRowHandle, gridKitComponentsView.Columns[column]);
            }

            rowHandle = value;
        }

        private void SetHighlightedRowHandle(ref int rowHandle, string column, int value)
        {
            if (rowHandle == value)
            {
                return;
            }

            if (rowHandle != GridControl.InvalidRowHandle)
            {
                int tempRowHandle = rowHandle;
                rowHandle = GridControl.InvalidRowHandle;
                gridKitComponentsView.InvalidateRowCell(tempRowHandle, gridKitComponentsView.Columns[column]);
            }
            else
            {
                rowHandle = value;
                QtyToCartPressedRowHandle = GridControl.InvalidRowHandle;
            }
        }

        private void gridKitComponentsView_MouseUp(object sender, MouseEventArgs e)
        {
            if (QtyToCartPressedRowHandle != GridControl.InvalidRowHandle)
            {
                OnQtyToCartButtonClick();
                QtyToCartPressedRowHandle = GridControl.InvalidRowHandle;
            }
        }

        private void gridKitComponentsView_MouseMove(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view != null)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = view.CalcHitInfo(e.X, e.Y);
                if (info.InRowCell && info.Column.FieldName == ColQtyToCart && IsMouseOverButton(info.RowHandle, ColQtyToCart, new Point(e.X, e.Y)))
                {
                    QtyToCartHighlightedRowHandle = info.RowHandle;
                }
                else
                {
                    QtyToCartHighlightedRowHandle = GridControl.InvalidRowHandle;
                }
            }
        }

        private bool IsMouseOverButton(int rowHandle, string column, Point point)
        {
            bool result = false;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo vInfo = gridKitComponentsView.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
            if (vInfo != null)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cellInfo = vInfo.GetGridCellInfo(rowHandle, gridKitComponentsView.Columns[column]);
                result = cellInfo.Bounds.Contains(point);
            }

            return result;
        }

        private void gridKitComponentsView_MouseDown(object sender, MouseEventArgs e)
        {
            if (QtyToCartHighlightedRowHandle != GridControl.InvalidRowHandle)
            {
                QtyToCartPressedRowHandle = QtyToCartHighlightedRowHandle;
            }
        }

        #endregion
    }
}
