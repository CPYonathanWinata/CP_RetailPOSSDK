/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTab;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling
{
    /// <summary>
    /// Extension methods for Control class
    /// </summary>
    internal static class ControlExtensions
    {
        #region public methods

        /// <summary>
        /// Gets the value of the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="onValue">The action to execute when value is available.</param>
        public static void GetValue(this Control control, Action<string> onValue)
        {
            if ((control != null) && (onValue != null))
            {
                AccessibleObject accessibleObject = control.AccessibilityObject;

                if (accessibleObject != null)
                {
                    switch (accessibleObject.Role)
                    {
                        case AccessibleRole.Table:
                            {
                                GetValueFromGrid(control, onValue);
                                break;
                            }
                        case AccessibleRole.PageTabList:
                            {
                                GetValueFromTabControl(control, onValue);
                                break;
                            }
                        default:
                            {
                                string controlText = string.IsNullOrWhiteSpace(accessibleObject.Name) ? accessibleObject.Value : accessibleObject.Name;
                                onValue(controlText);
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the display name of the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The display name of the control.</returns>
        public static string GetDisplayName(this Control control)
        {
            string displayName = null;

            if ((control == null) || (control.AccessibilityObject == null))
            {
                return displayName;
            }

            AccessibleObject accessibleObject = control.AccessibilityObject;

            switch (accessibleObject.Role)
            {
                case AccessibleRole.Text:
                    {
                        // The real display name exists in accessible object only when name is not empty and not equal to value
                        if ((!string.IsNullOrWhiteSpace(accessibleObject.Name)) &&
                            (!string.Equals(accessibleObject.Name, accessibleObject.Value)))
                        {
                            displayName = accessibleObject.Name;
                        }
                        break;
                    }
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                // If the name is empty, atleast get the parent's name, which in most cases will be appropriate
                displayName = !string.IsNullOrWhiteSpace(accessibleObject.Name) ?
                                accessibleObject.Name :
                                accessibleObject.Parent.Name;
            }

            return displayName;
        }

        #endregion

        #region private methods

        private static void GetValueFromGrid(Control control, Action<string> onValue)
        {
            GridControl gridControl = control as GridControl;

            if (gridControl != null)
            {
                // Get the main view from grid control, so that we can get the visible columns
                GridView gridView = gridControl.MainView as GridView;

                if (gridView != null)
                {
                    // Subscribe for the focus changed event to get the selected row (selection changed event works only when multiple selection is enabled)
                    FocusedRowChangedEventHandler focusedRowChangedEventHandler = null;
                    focusedRowChangedEventHandler = (sender, args) =>
                        {
                            gridView.FocusedRowChanged -= focusedRowChangedEventHandler;

                            if (args.FocusedRowHandle >= 0)
                            {
                                StringBuilder controlValueBuilder = new StringBuilder();
                                GridViewInfo gridViewInfo = (GridViewInfo)gridView.GetViewInfo();

                                // Get values from the visible columns/cells and append them (as comma separated values)
                                foreach (GridColumnInfoArgs columnInfoArgs in gridViewInfo.ColumnsInfo)
                                {
                                    controlValueBuilder.Append(gridView.GetRowCellValue(args.FocusedRowHandle, columnInfoArgs.Column) + ", ");
                                }

                                // Trim any excess spaces and commas to remove any empty cell values and any trailing commas
                                onValue(controlValueBuilder.ToString().Trim(',', ' '));
                            }
                        };

                    // Remove any previous handlers before adding new ones 
                    gridView.FocusedRowChanged -= focusedRowChangedEventHandler;
                    gridView.FocusedRowChanged += focusedRowChangedEventHandler;

                }
            }
        }

        private static void GetValueFromTabControl(Control control, Action<string> onValue)
        {
            XtraTabControl tabControl = control as XtraTabControl;
            if (tabControl != null)
            {
                TabPageChangedEventHandler tabChangedEventHandler = null;
                tabChangedEventHandler = (sender, args) =>
                {
                    tabControl.SelectedPageChanged -= tabChangedEventHandler;
                    if (tabControl.SelectedTabPage != null)
                    {
                        onValue(tabControl.SelectedTabPage.Text);
                    }
                };

                // Remove any previous handlers before adding new ones 
                tabControl.SelectedPageChanged -= tabChangedEventHandler;
                tabControl.SelectedPageChanged += tabChangedEventHandler;
            }
        }

        #endregion

    }
}
