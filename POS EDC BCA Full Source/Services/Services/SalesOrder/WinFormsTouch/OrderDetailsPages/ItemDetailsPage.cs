/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraGrid;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class ItemDetailsPage : OrderDetailsPage
    {
        private const string COLQTY = "PickupQuantity";
        private int qtyPressedRowHandle = GridControl.InvalidRowHandle;
        private int qtyHighlightedRowHandle = GridControl.InvalidRowHandle;
        private const int BUTTONMARGIN = 3;

        public ItemDetailsPage()
        {
            InitializeComponent();

            // Set grid datasource as binding explicity as designer won't recognize a collection property
            gridItems.DataBindings.Add("DataSource", this.bindingSource, "Items");
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
            }
            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            this.Text              = LSRetailPosis.ApplicationLocalizer.Language.Translate(56330); // "Item details"
            btnPickAll.Text        = LSRetailPosis.ApplicationLocalizer.Language.Translate(56391); // "Pick up all items";
            colItemId.Caption      = LSRetailPosis.ApplicationLocalizer.Language.Translate(56392); // "Item number"
            colDescription.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(56393); // "Item name"
            colQtyOrdered.Caption  = LSRetailPosis.ApplicationLocalizer.Language.Translate(56394); // "Quantity ordered"
            colPrev.Caption        = LSRetailPosis.ApplicationLocalizer.Language.Translate(56395); // "Picked up quantity"
            colPickupQty.Caption   = LSRetailPosis.ApplicationLocalizer.Language.Translate(56396); // "Pickup quantity"
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(703, 501);
            }
        }

        private ItemDetailsViewModel ViewModel
        {
            get
            {
                return base.GetViewModel<ItemDetailsViewModel>();
            }
        }

        private void OnQtyButtonClicked(int rowHandle)
        {
            LineItemViewModel lineItem = this.ViewModel.Items[rowHandle];

            // If it is not for pickup at the current store, the button should be disabled and thus no form should be shown.
            // If there's no remaining items to pick up, the button should be disable
            if (!lineItem.IsPickupAtStore(ApplicationSettings.Terminal.StoreId) || lineItem.QuantityRemaining == 0)
            {
                return;
            }

            bool inputNeeded = true;
            decimal quantity = 0;
            do
            {
                // Get the quantity
                string inputText;

                using (frmInputNumpad inputDialog = new frmInputNumpad())
                {
                    inputDialog.EntryTypes = NumpadEntryTypes.Quantity;
                    inputDialog.PromptText = LSRetailPosis.ApplicationLocalizer.Language.Translate(56396); // "Pickup quantity"
                    inputDialog.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(99656); // Item details page
                    POSFormsManager.ShowPOSForm(inputDialog);
                    // Quit if cancel is pressed...
                    if (inputDialog.DialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    inputText = inputDialog.InputText;
                }

                // Set the quantity
                if (string.IsNullOrEmpty(inputText))
                {
                    quantity = 0;
                }
                else
                {
                    quantity = Convert.ToDecimal(inputText);
                }

                inputNeeded = false;

                // Can we sell this specific quantity of the item?
                // E.g. are we trying to sell 4,6 bottles of Coca Cola?
                if (lineItem.IsQtyAllowed(quantity) == false)
                {
                    // Invalid quantity for a non-scale item
                    using (frmMessage dialog = new frmMessage(4164, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error))
                    {
                        POSFormsManager.ShowPOSForm(dialog);
                    }

                    inputNeeded = true;
                }
            } while (inputNeeded);

            // Update the row source and repaint
            if (quantity <= lineItem.QuantityRemaining)
            {
                this.ViewModel.SetPickupQuantity(lineItem.ItemId, lineItem.LineId, quantity);
                this.ViewModel.CommitChanges();
                this.gridView.RefreshRow(rowHandle);
            }

        }

        private void OnPickAll_Click(object sender, EventArgs e)
        {
            this.ViewModel.ExecutePickupAllCommand();
            this.ViewModel.CommitChanges();
        }

        #region Navigation overrides

        public override bool PageRequiresNavigationButtons()
        {
            return true;
        }

        public override void OnUpButtonClicked()
        {
            gridView.MovePrev();
        }

        public override void OnPageUpButtonClicked()
        {
            gridView.MovePrevPage();
        }

        public override void OnDownButtonClicked()
        {
            gridView.MoveNext();
        }

        public override void OnPageDownButtonClicked()
        {
            gridView.MoveNextPage();
        }

        public override bool IsUpButtonEnabled()
        {
            return !gridView.IsFirstRow;
        }

        public override bool IsDownButtonEnabled()
        {
            return !gridView.IsLastRow;
        }

        public override bool IsClearEnabled()
        {
            return false;
        }

        #endregion

        #region Custom grid button code

        private void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string column = e.Column.FieldName;

            if (column == COLQTY)
            {
                e.Appearance.FillRectangle(e.Cache, e.Bounds);
                DrawButton(e.Cache, e.Bounds, gridItems.LookAndFeel.ActiveLookAndFeel.ActiveStyle, e.Appearance, GetButtonState(e.RowHandle, column), e.DisplayText);

                e.Handled = true;
            }
        }

        private void DrawButton(GraphicsCache cache, Rectangle bounds, ActiveLookAndFeelStyle lookAndFeel, AppearanceObject appearance, ObjectState state, string caption)
        {
            EditorButtonObjectInfoArgs args = new EditorButtonObjectInfoArgs(cache, Button, appearance);
            BaseLookAndFeelPainters painters = LookAndFeelPainterHelper.GetPainter(lookAndFeel);

            // Create some margin
            bounds.Inflate(-BUTTONMARGIN, -BUTTONMARGIN);

            args.Bounds = bounds;
            args.State = state;

            painters.Button.DrawObject(args);

            // Draw the text
            // DO NOT dispose of Brush, as it is owned by the cache.
            Brush brush = gridView.Appearance.HeaderPanel.GetForeBrush(cache);
            StringFormat sf = appearance.GetStringFormat(appearance.GetTextOptions());
            painters.Button.DrawCaption(args, caption, appearance.Font, brush, args.Bounds, sf);
        }

        private ObjectState GetButtonState(int rowHandle, string column)
        {
            int pressedRowHandle = GridControl.InvalidRowHandle;
            int highlightedRowHandle = GridControl.InvalidRowHandle;
            bool shouldDisable = false;
            LineItemViewModel row = this.ViewModel.Items[rowHandle];

            switch (column)
            {
                case COLQTY:
                    pressedRowHandle = QtyPressedRowHandle;
                    highlightedRowHandle = QtyHighlightedRowHandle;
                    shouldDisable = (row.QuantityRemaining == 0) || !row.IsPickupAtStore(ApplicationSettings.Terminal.StoreId);
                    break;
                default:
                    break;
            }

            if (shouldDisable)
            {
                return ObjectState.Disabled;
            }
            else if (rowHandle == pressedRowHandle)
            {
                return ObjectState.Pressed;
            }
            else
            {
                // Show hot if row is highlighted or cell is focused
                if (rowHandle == highlightedRowHandle
                    || (gridItems.IsFocused && (gridView.FocusedColumn.FieldName == column)
                    && (gridView.FocusedRowHandle == rowHandle)))
                {
                    return ObjectState.Hot;
                }
                else
                {
                    return ObjectState.Normal;
                }
            }
        }

        private DevExpress.XtraEditors.Controls.EditorButton button;

        private DevExpress.XtraEditors.Controls.EditorButton Button
        {
            get
            {
                if (button == null)
                {
                    button = new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph);
                }

                return button;
            }
        }

        /// <summary>
        /// Tracks the currently pressed row handle for the quantity column
        /// </summary>
        private int QtyPressedRowHandle
        {
            get { return qtyPressedRowHandle; }
            set
            {
                if (qtyPressedRowHandle != value)
                {
                    SetPressedRowHandle(ref qtyPressedRowHandle, COLQTY, value);

                    gridView.InvalidateRowCell(qtyPressedRowHandle, gridView.Columns[COLQTY]);
                }
            }
        }

        /// <summary>
        /// Tracks the currently highlighted row handle for the quantity column
        /// </summary>
        private int QtyHighlightedRowHandle
        {
            get { return qtyHighlightedRowHandle; }
            set
            {
                if (qtyHighlightedRowHandle != value)
                {
                    SetHighlightedRowHandle(ref qtyHighlightedRowHandle, COLQTY, value);

                    gridView.InvalidateRowCell(qtyHighlightedRowHandle, gridView.Columns[COLQTY]);
                }
            }
        }

        private void SetPressedRowHandle(ref int rowHandle, string column, int value)
        {
            if (rowHandle != GridControl.InvalidRowHandle)
            {
                int tempRowHandle = rowHandle;
                rowHandle = GridControl.InvalidRowHandle;
                gridView.InvalidateRowCell(tempRowHandle, gridView.Columns[column]);
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
                gridView.InvalidateRowCell(tempRowHandle, gridView.Columns[column]);
            }
            else
            {
                rowHandle = value;
                QtyPressedRowHandle = GridControl.InvalidRowHandle;
            }
        }

        /// <summary>
        /// Mouse down event handler for grid view
        /// </summary>
        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (QtyHighlightedRowHandle != GridControl.InvalidRowHandle)
            {
                QtyPressedRowHandle = QtyHighlightedRowHandle;
            }
        }

        /// <summary>
        /// Mouse up event handler for grid view
        /// </summary>
        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (QtyPressedRowHandle != GridControl.InvalidRowHandle)
            {
                OnQtyButtonClicked(QtyPressedRowHandle);
                QtyPressedRowHandle = GridControl.InvalidRowHandle;
            }
        }

        /// <summary>
        /// Mouse move event handler for grid view
        /// </summary>
        private void gridView_MouseMove(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view != null)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = view.CalcHitInfo(e.X, e.Y);
                if (info.InRowCell && (info.Column.FieldName == COLQTY) && IsMouseOverButton(info.RowHandle, COLQTY, new Point(e.X, e.Y)))
                {
                    QtyHighlightedRowHandle = info.RowHandle;
                }
                else
                {
                    QtyHighlightedRowHandle = GridControl.InvalidRowHandle;
                }
            }
        }

        private bool IsMouseOverButton(int rowHandle, string column, Point point)
        {
            bool result = false;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo = gridView.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
            if (viewInfo != null)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cellInfo = viewInfo.GetGridCellInfo(rowHandle, gridView.Columns[column]);
                result = cellInfo.Bounds.Contains(point);
            }

            return result;
        }

        /// <summary>
        /// Handles the display text for custom grid columns
        /// </summary>
        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case COLQTY:
                    e.DisplayText = this.ViewModel.Items[e.ListSourceRowIndex].PickupQuantity.ToString();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Key up handler for grid view
        /// </summary>
        /// <remarks>Invokes the  clicked handler for a column button when the space bar is pressed</remarks>
        private void gridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                switch (gridView.FocusedColumn.FieldName)
                {
                    case COLQTY:
                        OnQtyButtonClicked(gridView.FocusedRowHandle);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}