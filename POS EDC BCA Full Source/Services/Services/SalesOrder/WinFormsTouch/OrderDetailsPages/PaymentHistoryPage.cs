/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class PaymentHistoryPage : OrderDetailsPage
    {
        public PaymentHistoryPage()
        {
            InitializeComponent();

            // Set grid datasource as binding explicity as designer won't recognize a collection property
            gridPayments.DataBindings.Add("DataSource", this.bindingSource, "Payments");
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
            this.Text          = LSRetailPosis.ApplicationLocalizer.Language.Translate(56332); // "Payment history"
            colDate.Caption    = LSRetailPosis.ApplicationLocalizer.Language.Translate(56351); // "Date"
            colName.Caption    = LSRetailPosis.ApplicationLocalizer.Language.Translate(56352); // "Order number"
            colAmount.Caption  = LSRetailPosis.ApplicationLocalizer.Language.Translate(56353); // "Amount"
        }

        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                return new System.Drawing.Size(703, 501);
            }
        }

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
    }
}