/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using DevExpress.XtraEditors;
using LSRetailPosis.POSProcesses.WinControls;
using System.Windows.Forms;
using LSRetailPosis;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    sealed partial class OrderTypePopup : UserControl
    {
        public OrderTypePopup()
        {
            InitializeComponent();
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
            btnSalesOrder.Text = ApplicationLocalizer.Language.Translate(56373); // "Sales order"
            btnQuote.Text      = ApplicationLocalizer.Language.Translate(56374); // "Quote"
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            if (btn != null)
            {
                this.SelectedIndex = btn.TabIndex;                
            }

            ToolStripDropDown parent = this.Parent as ToolStripDropDown;
            if (parent != null)
                parent.Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        /// <summary>
        /// Index of selected button
        /// </summary>
        public int SelectedIndex
        {
            get;
            private set;
        }
    }
}