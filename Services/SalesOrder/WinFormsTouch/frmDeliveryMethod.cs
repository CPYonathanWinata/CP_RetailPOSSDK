/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using LSRetailPosis.POSProcesses;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
	partial class formDeliveryMethod : frmTouchBase
    {
        public DeliveryMethod DeliveryMethod { get; private set; }

        public formDeliveryMethod()
        {
            InitializeComponent();
            this.DeliveryMethod = WinFormsTouch.DeliveryMethod.None;
        }

        private void OnPickClick(object sender, EventArgs e)
        {
            this.DeliveryMethod = WinFormsTouch.DeliveryMethod.Pickup;
            this.Close();
        }

        private void OnShipClick(object sender, EventArgs e)
        {
            this.DeliveryMethod = WinFormsTouch.DeliveryMethod.Ship;
            this.Close();
        }
    }

    enum DeliveryMethod
    {
        /// <summary>
        /// No delivery method selected.
        /// </summary>
        None,

        /// <summary>
        /// Item is meant to be picked up.
        /// </summary>
        Pickup,

        /// <summary>
        /// Item is meant to be shipped.
        /// </summary>
        Ship
    }
}
