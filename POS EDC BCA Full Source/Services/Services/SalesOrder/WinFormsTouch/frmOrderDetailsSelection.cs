/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    /// <summary>
    /// Customer Orders details selection form
    /// </summary>
	partial class formOrderDetailsSelection : frmTouchBase
    {
        private OrderDetailsSelection selection = OrderDetailsSelection.None;

        public formOrderDetailsSelection()
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
            lblHeader.Text = ApplicationLocalizer.Language.Translate(56228); //Customer order options
            btnViewDetails.Text = ApplicationLocalizer.Language.Translate(56212);   // View details
            btnCloseOrder.Text  = ApplicationLocalizer.Language.Translate(56214);   // Close order
            btnRecalculate.Text = ApplicationLocalizer.Language.Translate(103135);  //Recalculate
            btnClose.Text = ApplicationLocalizer.Language.Translate(56205); //Close
        }

        private void OnBtnViewDetails_Click(object sender, EventArgs e)
        {
            this.Selection = OrderDetailsSelection.ViewDetails;            
        }
        
        private void OnBtnCloseOrder_Click(object sender, EventArgs e)
        {
            this.Selection = OrderDetailsSelection.CloseOrder;
        }

        private void OnBtnReturnOrder_Click(object sender, EventArgs e)
        {
            this.Selection = OrderDetailsSelection.Recalculate;
        }

        /// <summary>
        /// Indicates the button selected
        /// </summary>
        public OrderDetailsSelection Selection
        {
            get { return selection; }
            private set
            {
                selection = value;
            }
        }
    }

    /// <summary>
    /// Selection choices for operating on an existing order
    /// </summary>
    enum OrderDetailsSelection
    {
        None,
        ViewDetails,
        PickupOrder,
        CloseOrder,
        CancelOrder,
        Recalculate,
        PickList
    }
}