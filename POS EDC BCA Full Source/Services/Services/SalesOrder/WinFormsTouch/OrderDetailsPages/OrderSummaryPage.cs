/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using LSRetailPosis;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class OrderSummaryPage : OrderDetailsPage
    {
        private const string DATABINDING_VISIBLEPROPERTY = "Visible";

        public OrderSummaryPage()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
                this.memoEdit1.Properties.ReadOnly = true;
                this.memoEdit1.BackColor = System.Drawing.SystemColors.Window;
                this.memoEdit1.ForeColor = System.Drawing.SystemColors.WindowText;

                //Workaround for issue where memoEdit control forces highlighting of text the first time it gains focus
                this.memoEdit1.SelectionLength = 0;
                this.memoEdit1.TabStop = false;
            }
            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            this.Text = ApplicationLocalizer.Language.Translate(56334); // "Order summary"
        }

        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                return new System.Drawing.Size(703, 501);
            }
        }

        private void OrderSummaryPage_Leave(object sender, EventArgs e)
        {
            //Workaround for issue where memoEdit control forces highlighting of text the first time it gains focus
            memoEdit1.SelectionLength = 0;
            memoEdit1.TabStop = true;
        }
    }
}