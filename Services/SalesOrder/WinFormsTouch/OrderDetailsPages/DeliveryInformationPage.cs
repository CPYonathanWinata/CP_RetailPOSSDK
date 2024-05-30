/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    sealed partial class DeliveryInformationPage : OrderDetailsPage
    {
        private const int ButtonMargin = 3;
        private readonly Font WingdingsFont = new Font("Wingdings", 12.0f);
        private const string WingdingsTickCharachter = "ü";

        private const string ColStatus = "FormattedLineStatus";
        private const string ColDescription = "Description";
        private const string ColItemNumber = "ItemNumber";
        private const string ColItemOverview = "ItemOverview";
        private const string ColPickup = "Pickup";
        private const string ColQuantity = "Quantity";
        private const string ColShip = "Ship";

        private Dictionary<string, ButtonCellGridController> buttonCells;
        private List<LineLevelInformationViewModel> gridSource;

        public DeliveryInformationPage()
        {
            InitializeComponent();

            this.buttonCells = new Dictionary<string, ButtonCellGridController>();

            this.buttonCells[ColShip] = new ButtonCellGridController(ColShip, this.gridView);
            //this.buttonCells[ColPickup] = new ButtonCellGridController(ColPickup, this.gridView);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
				this.gridView.Appearance.HeaderPanel.ForeColor = this.ForeColor;
				this.gridView.Appearance.Row.ForeColor = this.ForeColor;
                TranslateLabels();
            }

            disableElement();//custom by Yonathan 3/Jan/2023
            base.OnLoad(e);
        }

        public void disableElement() //custom by Yonathan 3/Jan/2023
        {
            this.btnPickupAll.Visible = false;            

        }

        public override void OnActivate()
        {
            if (this.gridSource == null)
            {
                this.gridSource = this.ViewModel.Transaction.SaleItems.Where(i => !i.Voided).Select(
                        lineItem => new LineLevelInformationViewModel(lineItem, this.ViewModel.Transaction))
                        .ToList();

                this.gridItems.DataSource = this.gridSource;
            }

            UpdateButtons();
        }

        private void TranslateLabels()
        {
            this.Text               = LSRetailPosis.ApplicationLocalizer.Language.Translate(56331); // "Shipping and delivery"
            this.btnShipAll.Text    = LSRetailPosis.ApplicationLocalizer.Language.Translate(56348); // "Ship all"
            this.btnPickupAll.Text  = LSRetailPosis.ApplicationLocalizer.Language.Translate(56349); // "Pick up all"

            this.lblLinesShippingCharge.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56410); // "Shipping lines total"
            this.lblOrderShippingCharge.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56411); // "Order level shipping"
            this.lblTotalShippingCharge.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56412); // "Total shipping"

            this.colItemOverview.Caption     = LSRetailPosis.ApplicationLocalizer.Language.Translate(56415); // "Item overview"
            this.colQuantity.Caption         = LSRetailPosis.ApplicationLocalizer.Language.Translate(5323);  // "Quantity"
            this.colLineStatus.Caption       = LSRetailPosis.ApplicationLocalizer.Language.Translate(56217); // "Order status"
            this.colShipAll.Caption          = LSRetailPosis.ApplicationLocalizer.Language.Translate(56416); // "Ship"
            this.colPickupAll.Caption        = LSRetailPosis.ApplicationLocalizer.Language.Translate(56417); // "Pickup"
        }

        private OrderDetailsViewModel ViewModel
        {
            get { return GetViewModel<OrderDetailsViewModel>(); }
        }

        private void OnShipAll_Click(object sender, EventArgs e)
        {
            ShippingInformationViewModel vm = ShowShippingForm();
            if (vm != null)
            {
                try
                {
                    this.SetShipAllValues(vm);
                    vm.CommitHeaderChanges();

                    this.ResetControlBindings();
                }
                catch (InvalidOperationException ex)
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ShippingInformationViewModel ShowShippingForm()
        {
            return ShowShippingForm(-1);
        }

        private ShippingInformationViewModel ShowShippingForm(int rowIndex)
        {
            LineLevelInformationViewModel viewModel = null;
            if (rowIndex >= 0 && rowIndex < this.gridSource.Count)
            {
                viewModel = this.gridSource[rowIndex];
            }

            ShippingInformationViewModel vm = null;

            // Load any existing values
            if (viewModel != null)
            {
                vm = new ShippingInformationViewModel(this.ViewModel.Transaction, viewModel.LineItem);
                vm.DeliveryDate = viewModel.DeliveryDate;
                vm.ShippingCharge = viewModel.ShippingCharge;
                vm.ShippingMethod = vm.ShippingMethods.FirstOrDefault(m => m.Code == viewModel.ShippingMethodCode);
                
                vm.ShippingAddress = viewModel.ShippingAddress;
            }
            else
            {
                vm = new ShippingInformationViewModel(this.ViewModel.Transaction, null);
                vm.ShippingMethod = vm.ShippingMethods.FirstOrDefault(m => m.Code == "Cust");
            }
            vm.IsDeliveryChangeAllowed = this.ViewModel.IsDeliveryChangeAllowed;

            using (formShippingInformation frmShip = new formShippingInformation(vm))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmShip);

                if (frmShip.DialogResult == DialogResult.OK)
                {
                    return frmShip.ViewModel;
                }
            }

            return null;
        }

        private void SetShipAllValues(ShippingInformationViewModel shippingViewModel)
        {
            foreach (var lineViewModel in this.gridSource)
            {
                SetShipValues(shippingViewModel, lineViewModel);

                // don't copy the shipping charge
                lineViewModel.ShippingCharge = null;
                lineViewModel.Commit();
            }
        }

        private void OnPickupAll_Click(object sender, EventArgs e)
        {
            PickupInformationViewModel viewModel = ShowPickupForm();
            if (viewModel != null)
            {
                try
                {
                    this.SetPickupAllValues(viewModel);
                    viewModel.CommitHeaderChanges();

                    this.ResetControlBindings();
                }
                catch (InvalidOperationException ex)
                {
                    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private PickupInformationViewModel ShowPickupForm()
        {
            return ShowPickupForm(-1);
        }

        private PickupInformationViewModel ShowPickupForm(int rowIndex)
        {
            // Before we show the pickup form, check that a pickup code exists
            if (string.IsNullOrWhiteSpace(ApplicationSettings.Terminal.PickupDeliveryModeCode))
            {
                // "Pickup cannot be used for delivery because a pickup delivery code was not found."
                string errorMessage = LSRetailPosis.ApplicationLocalizer.Language.Translate(56382);
                SalesOrder.LogMessage(errorMessage, LSRetailPosis.LogTraceLevel.Error);
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }

            PickupInformationViewModel vm = new PickupInformationViewModel(this.ViewModel.Transaction);
            vm.IsDeliveryChangeAllowed = this.ViewModel.IsDeliveryChangeAllowed;

            // Load any existing values
            if (rowIndex >= 0 && rowIndex < this.gridSource.Count)
            {
                LineLevelInformationViewModel viewModel = this.gridSource[rowIndex];

                vm.PickupDate = viewModel.DeliveryDate;
                vm.SelectedStore = viewModel.DeliveryStore;
            }

            using (formPickupInformation pickup = new formPickupInformation(vm))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(pickup);

                if (pickup.DialogResult == DialogResult.OK)
                {
                    return pickup.ViewModel;
                }
            }

            return null;
        }

        private void SetPickupAllValues(PickupInformationViewModel pickupViewModel)
        {
            foreach (var lineViewModel in this.gridSource)
            {
                SetPickupValues(pickupViewModel, lineViewModel);
                lineViewModel.Commit();
            }
        }

        private static void SetPickupValues(
            PickupInformationViewModel fromPickupViewModel,
            LineLevelInformationViewModel toLineViewModel)
        {
            toLineViewModel.IsPickup = true;
            toLineViewModel.DeliveryDate = fromPickupViewModel.PickupDate;
            toLineViewModel.DeliveryStore = fromPickupViewModel.SelectedStore;

            //Set the the shippingAddress to the selected store's address
            toLineViewModel.ShippingAddress = fromPickupViewModel.GetStoreAddress(fromPickupViewModel.SelectedStore);

            // Remove any shipping related data
            toLineViewModel.ShippingCharge = null;
            toLineViewModel.ShippingMethodCode = null;
        }

        private static void SetShipValues(
            ShippingInformationViewModel fromShippingViewModel,
            LineLevelInformationViewModel toLineViewModel)
        {
            toLineViewModel.IsShipping = true;
            toLineViewModel.DeliveryDate = fromShippingViewModel.DeliveryDate;
            toLineViewModel.ShippingCharge = fromShippingViewModel.ShippingCharge;
            toLineViewModel.ShippingMethodCode = fromShippingViewModel.ShippingMethod.Code;
            toLineViewModel.ShippingAddress = fromShippingViewModel.ShippingAddress;

            // Remove pickup related data
            toLineViewModel.DeliveryStore = null;
        }

        public override bool IsClearEnabled()
        {
            return false;
        }

        #region Custom grid code
        /// <summary>
        /// Handles the display text for custom grid columns
        /// </summary>
        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (!this.gridSource.Any())
            {
                return;
            }

            LineLevelInformationViewModel viewModel = gridSource[e.ListSourceRowIndex];

            switch (e.Column.FieldName)
            {
                case ColShip:
                    e.DisplayText = viewModel.IsShipping
                        ? DeliveryInformationPage.WingdingsTickCharachter
                        : string.Empty;
                    break;

                case ColPickup:
                    e.DisplayText = viewModel.IsPickup
                        ? DeliveryInformationPage.WingdingsTickCharachter
                        : string.Empty;
                    break;
            }
        }

        private void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (this.buttonCells.ContainsKey(e.Column.FieldName))
            {
                e.Appearance.FillRectangle(e.Cache, e.Bounds);
                DrawButton(
                    e.Cache,
                    e.Bounds,
                    gridItems.LookAndFeel.ActiveLookAndFeel.ActiveStyle,
                    e.Appearance,
                    GetButtonState(e.RowHandle, e.Column.FieldName),
                    e.DisplayText);

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
            // DO NOT dispose of the brush, as it is owned by the cache
            Brush brush = GetButtonForeBrush(state, cache);
            StringFormat sf = appearance.GetStringFormat(appearance.GetTextOptions());
            painters.Button.DrawCaption(args, caption, WingdingsFont, brush, args.Bounds, sf);
        }

        private Brush GetButtonForeBrush(ObjectState state, GraphicsCache cache)
        {
            if (state == ObjectState.Disabled)
            {
                return SystemBrushes.GrayText;
            }

            return gridView.Appearance.HeaderPanel.GetForeBrush(cache);
        }

        private ObjectState GetButtonState(int rowHandle, string column)
        {
            int pressedRowHandle = GridControl.InvalidRowHandle;
            int highlightedRowHandle = GridControl.InvalidRowHandle;

            ButtonCellGridController controller;
            if (this.buttonCells.TryGetValue(column, out controller))
            {
                pressedRowHandle = controller.PressedRowHandle;
                highlightedRowHandle = controller.HighlightedRowHandle;
            }

            if (rowHandle == pressedRowHandle)
            {
                return ObjectState.Pressed;
            }
            else
            {
                // Show hot if row is highlighted or cell is focused
                if (rowHandle == highlightedRowHandle
                    || (gridItems.IsFocused && gridView.FocusedColumn.FieldName == column && gridView.FocusedRowHandle == rowHandle))
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
                    button = new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK);
                }

                return button;
            }
        }

        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var controller in this.buttonCells.Values)
            {
                if (controller.HighlightedRowHandle != GridControl.InvalidRowHandle)
                {
                    controller.PressedRowHandle = controller.HighlightedRowHandle;
                }
            }
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var controller in this.buttonCells.Values)
            {
                if (controller.PressedRowHandle != GridControl.InvalidRowHandle)
                {
                    OnDeliveryMethodButtonClick(controller.PressedRowHandle);
                    controller.PressedRowHandle = GridControl.InvalidRowHandle;
                }
            }
        }

        private void gridView_MouseMove(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view != null)
            {
                Point point = new Point(e.X, e.Y);
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = view.CalcHitInfo(point);
                foreach (var controller in this.buttonCells.Values)
                {
                    if (info.InRowCell && IsMouseOverButton(info.RowHandle, controller.ColumnName, point))
                    {
                        controller.HighlightedRowHandle = info.RowHandle;
                    }
                    else
                    {
                        controller.HighlightedRowHandle = GridControl.InvalidRowHandle;
                    }
                }
            }
        }

        private bool IsMouseOverButton(int rowHandle, string column, Point point)
        {
            bool result = false;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo vInfo = gridView.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
            if (vInfo != null)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cellInfo = vInfo.GetGridCellInfo(rowHandle, gridView.Columns[column]);
                result = cellInfo.Bounds.Contains(point);
            }

            return result;
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrevPage();
            UpdateButtons();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrev();
            UpdateButtons();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNext();
            UpdateButtons();
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNextPage();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnPageDown.Enabled = !gridView.IsLastRow;
            btnDown.Enabled = btnPageDown.Enabled;
            btnPageUp.Enabled = !gridView.IsFirstRow;
            btnUp.Enabled = btnPageUp.Enabled;

            btnPickupAll.Enabled = gridView.RowCount > 0;
            btnShipAll.Enabled = gridView.RowCount > 0;
        }

        private void gridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && this.gridSource.Any())
            {
                OnDeliveryMethodButtonClick(gridView.FocusedRowHandle);
            }
        }

        private void OnDeliveryMethodButtonClick(int rowHandle)
        {
            if (rowHandle < 0 || rowHandle > this.gridSource.Count)
            {
                return;
            }

            LineLevelInformationViewModel viewModel = this.gridSource[rowHandle];

            if (string.Equals(gridView.FocusedColumn.FieldName, ColShip, StringComparison.OrdinalIgnoreCase))
            {
                ShippingInformationViewModel shippingViewModel = ShowShippingForm(rowHandle);

                if (shippingViewModel != null)
                {
                    // we clear the header...
                    this.ViewModel.ExecuteClearHeaderDelivery();

                    // ... set the values for the selected line
                    SetShipValues(shippingViewModel, viewModel);

                    // ...and switch everything to line level
                    foreach (LineLevelInformationViewModel lineViewModel in this.gridSource)
                    {
                        lineViewModel.Commit();
                    }

                    this.ResetControlBindings();
                }
            }

            if (string.Equals(gridView.FocusedColumn.FieldName, ColPickup, StringComparison.OrdinalIgnoreCase))
            {
                PickupInformationViewModel pickupViewModel = ShowPickupForm(rowHandle);

                if (pickupViewModel != null)
                {
                    // we clear the header...
                    this.ViewModel.ExecuteClearHeaderDelivery();

                    // ... set the values for the selected line
                    SetPickupValues(pickupViewModel, viewModel);

                    // ...and switch everything to line level
                    foreach (LineLevelInformationViewModel lineViewModel in this.gridSource)
                    {
                        lineViewModel.Commit();
                    }

                    this.ResetControlBindings();
                }
            }
        }

        private void ResetControlBindings()
        {
            this.bindingSource.ResetBindings(false);
            this.gridView.RefreshData();
        }

        internal class ButtonCellGridController
        {
            private int pressedRowHandle = GridControl.InvalidRowHandle;
            private int highlightedRowHandle = GridControl.InvalidRowHandle;

            public ButtonCellGridController(string columnName, GridView parentGridView)
            {
                this.ColumnName = columnName;
                this.ParentGridView = parentGridView;
            }

            public string ColumnName { get; private set; }

            public GridView ParentGridView { get; private set; }

            public int PressedRowHandle
            {
                get { return this.pressedRowHandle; }
                set
                {
                    if (this.pressedRowHandle != value)
                    {
                        this.pressedRowHandle = GetPressedRowHandle(this.pressedRowHandle, value);
                        this.ParentGridView.InvalidateRowCell(this.pressedRowHandle, this.ParentGridView.Columns[this.ColumnName]);
                    }
                }
            }

            public int HighlightedRowHandle
            {
                get { return this.highlightedRowHandle; }
                set
                {
                    if (this.highlightedRowHandle != value)
                    {
                        this.highlightedRowHandle = GetHighlightedRowHandle(this.highlightedRowHandle, value);
                    }

                    this.ParentGridView.InvalidateRowCell(this.highlightedRowHandle, this.ParentGridView.Columns[this.ColumnName]);
                }
            }

            private int GetPressedRowHandle(int rowHandle, int value)
            {
                int pressedRow = GridControl.InvalidRowHandle;

                if (rowHandle != GridControl.InvalidRowHandle)
                {
                    this.ParentGridView.InvalidateRowCell(rowHandle, this.ParentGridView.Columns[this.ColumnName]);
                }
                else
                {
                    pressedRow = value;
                }

                return pressedRow;
            }

            private int GetHighlightedRowHandle(int rowHandle, int value)
            {
                int highlightedRow = GridControl.InvalidRowHandle;

                if (rowHandle != GridControl.InvalidRowHandle)
                {
                    this.ParentGridView.InvalidateRowCell(rowHandle, this.ParentGridView.Columns[this.ColumnName]);
                }
                else
                {
                    highlightedRow = value;
                    this.PressedRowHandle = GridControl.InvalidRowHandle;
                }

                return highlightedRow;
            }
        }

        # endregion
    }
}