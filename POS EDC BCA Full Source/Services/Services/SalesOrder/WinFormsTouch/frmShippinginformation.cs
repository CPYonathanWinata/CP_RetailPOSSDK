/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
	sealed partial class formShippingInformation : frmTouchBase
	{
		private const string DATABINDING_VALUEPROPERTY = "Value";
		private const string DATABINDING_TEXTPROPERTY = "Text";

		private formShippingInformation()
		{
			InitializeComponent();
			viewAddressUserControl1.SetEditable(false);
			viewAddressUserControl1.SetCaptionVisibility(false);
		}

		public formShippingInformation(ShippingInformationViewModel viewModel)
			: this()
		{
			this.dateTimePicker.MinDate = DateTime.Today;
            this.labelShipMethodValue.Text = string.Empty;

			// Handle the formatting and parsing for binding to the DateTimePicker so it knows how to handle nulls from a DateTime? value.
			this.dateTimePicker.DataBindings[DATABINDING_VALUEPROPERTY].Format += new ConvertEventHandler(OnDateTimePicker_Format);
			this.dateTimePicker.DataBindings[DATABINDING_VALUEPROPERTY].Parse += new ConvertEventHandler(OnDateTimePicker_Parse);

			// Toggle the charge button's text based on whether or not there is a charge
			this.btnChargeChange.DataBindings[DATABINDING_TEXTPROPERTY].Format += new ConvertEventHandler(OnShippingChargeButtonText_Format);

			// Bind to view model
			this.bindingSource.Add(viewModel);

			if (viewModel != null && viewModel.Transaction != null)
			{
				this.viewAddressUserControl1.SetProperties(viewModel.Transaction.Customer, viewModel.ShippingAddress);
			}

			viewModel.PropertyChanged += new PropertyChangedEventHandler(OnViewModel_PropertyChanged);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			ViewModel.PropertyChanged -= OnViewModel_PropertyChanged;

			base.OnFormClosed(e);
		}

		protected override void OnLoad(System.EventArgs e)
		{
			if (!this.DesignMode)
			{
				TranslateLabels();
			}

			base.OnLoad(e);
		}

		private void TranslateLabels()
		{
			labelTitle.Text = ApplicationLocalizer.Language.Translate(56380); // Shipping information
			labelContact.Text = ApplicationLocalizer.Language.Translate(56321); // SHIPPING ADDRESS
			labelShippingMethod.Text = ApplicationLocalizer.Language.Translate(56323); // SHIPPING METHOD
			labelCharge.Text = ApplicationLocalizer.Language.Translate(56324); // Shipping charge:
			btnEditMethod.Text = ApplicationLocalizer.Language.Translate(56340); // Change...           

			labelDeliveryDate.Text = ApplicationLocalizer.Language.Translate(56325); // DELIVERY DATE
			btnSearch.Text = ApplicationLocalizer.Language.Translate(56317); // Search
			btnClear.Text = ApplicationLocalizer.Language.Translate(56327); // Clear
			btnSave.Text = ApplicationLocalizer.Language.Translate(56318); // Save
			btnCancel.Text = ApplicationLocalizer.Language.Translate(56319); // Cancel
		}

		private void OnDateTimePicker_Format(object sender, ConvertEventArgs e)
		{
			// Object => Control
			// e.Value is the object's value, if that is null we want to show the checkbox unchecked            

			if (e.Value == null)
			{
				dateTimePicker.ShowCheckBox = true;
				dateTimePicker.Checked = false;
			}
			else
			{
				if ((DateTime)e.Value >= this.dateTimePicker.MinDate)
				{
					dateTimePicker.ShowCheckBox = true;
					dateTimePicker.Checked = true;
				}
			}
		}

		private void OnDateTimePicker_Parse(object sender, ConvertEventArgs e)
		{
			// CONTROL => OBJECT
			// e.Value is the formatted value coming from the control, we change it to want we want to set on the object

			if (this.dateTimePicker.Checked)
			{
				e.Value = new Nullable<DateTime>(Convert.ToDateTime(e.Value));
			}
			else
			{
				e.Value = null;
			}
		}

		private void OnShippingChargeButtonText_Format(object sender, ConvertEventArgs e)
		{
			// We're converting from decimal? to string
			if (e.DesiredType == typeof(string))
			{
				e.Value = (e.Value == null)
					? ApplicationLocalizer.Language.Translate(56221)  // Add
					: ApplicationLocalizer.Language.Translate(56223); // Remove
			}
		}

		[Browsable(false)]
		public ShippingInformationViewModel ViewModel
		{
			get
			{
				ShippingInformationViewModel viewModel = null;

				if (bindingSource != null && bindingSource.Current != null)
					viewModel = (ShippingInformationViewModel)bindingSource.Current;

				return viewModel;
			}
		}

		private void OnClear_Click(object sender, System.EventArgs e)
		{
			this.ViewModel.ExecuteClearCommand();
		}

		private void OnSearch_Click(object sender, EventArgs e)
		{
			this.ViewModel.ExecuteShippingAddressSearchCommand();
		}

		private void OnBtnShippingMethod_Click(object sender, EventArgs e)
		{
			string code;
			string defaultValue = this.ViewModel.ShippingMethod == null ? null : this.ViewModel.ShippingMethod.Code;
			DialogResult result = SalesOrder.InternalApplication.Services.Dialog.GenericLookup(this.ViewModel.ShippingMethods, "DescriptionText", ApplicationLocalizer.Language.Translate(56381), "Code", out code, defaultValue);
			if (result == DialogResult.OK)
			{
				this.ViewModel.ShippingMethod = this.ViewModel.ShippingMethods.First(s => s.Code == code);
			}
		}

		private void OnChargeChange_Click(object sender, EventArgs e)
		{
			if (this.ViewModel.ShippingCharge.HasValue)
			{
				// Remove charge
				this.ViewModel.ShippingCharge = null;
			}
			else
			{
				// Add charge
				using (frmInputNumpad inputDialog = new frmInputNumpad())
				{
					inputDialog.EntryTypes = NumpadEntryTypes.Price;
					LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(inputDialog);

					if (inputDialog.DialogResult == DialogResult.OK)
					{
						decimal shippingCharge;
						if (decimal.TryParse(inputDialog.InputText, out shippingCharge))
						{
							this.ViewModel.ShippingCharge = shippingCharge;
						}
					}
				}
			}
		}

		private void OnViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{

			if (e.PropertyName.Equals("ShippingAddress"))
			{
				var viewModel = ViewModel; //Prevent unnecessary execution of the ViewModel property multiple times
				if (viewModel != null && viewModel.Transaction != null && viewModel.Transaction.Customer != null)
				{
					this.viewAddressUserControl1.SetProperties(viewModel.Transaction.Customer, viewModel.ShippingAddress);
				}
			}
		}
	}
}