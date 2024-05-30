/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class CancellationChargePage : OrderDetailsPage
    {
        private const string DATABINDING_TEXTPROPERTY = "Text";

        public CancellationChargePage()
        {
            InitializeComponent();

            // Toggle the charge button's text based on whether or not there is a charge
            btnAdd.DataBindings[DATABINDING_TEXTPROPERTY].Format += new ConvertEventHandler(OnChargeButtonText_Format);
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
            this.Text       = LSRetailPosis.ApplicationLocalizer.Language.Translate(56333); // "Cancellation charge"
            this.label.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56226); //"Cancellation charge:"
        }

        private void OnChargeButtonText_Format(object sender, ConvertEventArgs e)
        {
            // We're converting from decimal? to string
            if (e.DesiredType == typeof(string))
            {
                e.Value = (e.Value == null)
                    ? LSRetailPosis.ApplicationLocalizer.Language.Translate(56221)  // Add
                    : LSRetailPosis.ApplicationLocalizer.Language.Translate(56223); // Remove
            }
        }

        private OrderDetailsViewModel ViewModel
        {
            get
            {
                return base.GetViewModel<OrderDetailsViewModel>();
            }
        }

        protected override void OnViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CancellationCharge":
                    TranslateLabels();
                    break;
            }
        }

        private void OnChargeAddChange_Click(object sender, EventArgs e)
        {
            if (!this.ViewModel.CancellationCharge.HasValue)
            {
                using (frmInputNumpad inputDialog = new frmInputNumpad())
                {
                    inputDialog.PromptText = LSRetailPosis.ApplicationLocalizer.Language.Translate(56333); // Cancellation charge
					inputDialog.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56333); // Cancellation charge
                    inputDialog.EntryTypes = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Price;

                    SalesOrder.InternalApplication.ApplicationFramework.POSShowForm(inputDialog);
                    if (inputDialog.DialogResult == DialogResult.OK)
                    {
                        decimal cancellationCharge;
                        if (decimal.TryParse(inputDialog.InputText, out cancellationCharge))
                        {
                            this.ViewModel.CancellationCharge = cancellationCharge;
                        }
                    }
                }
            }
            else
            {
                // Remove charge
                this.ViewModel.CancellationCharge = null;
            }
        }
    }
}