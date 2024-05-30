/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
	partial class formOrderDetails : frmTouchBase
    {        
        private List<OrderDetailsPage> pages;
        private List<PageViewModel> pageViewModels;
        private OrderDetailsPage currentPage;
        private CustomerOrderTransaction transaction;
        private OrderDetailsSelection mode;
        private OrderDetailsViewModel orderDetailsViewModel;
        private OrderSummaryViewModel orderSummaryViewModel;
        private CustomerInformationViewModel customerInformationViewModel;
        private PaymentHistoryViewModel paymentHistoryViewModel;
        private ItemDetailsViewModel itemDetailsViewModel;        

        protected formOrderDetails()
        {
            InitializeComponent();
        }

        public formOrderDetails(CustomerOrderTransaction customerOrderTransaction, OrderDetailsSelection selectionMode):
            this()
        {
            this.transaction = customerOrderTransaction;
            this.mode        = selectionMode;
        }
                
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            if (hevent == null)
                throw new ArgumentNullException("hevent");
            else
            {
                LSRetailPosis.POSControls.POSFormsManager.ShowHelp(System.Windows.Forms.Form.ActiveForm);
                hevent.Handled = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();

                // Create a collection of pages
                this.pages = new List<OrderDetailsPage>(this.panelControl.Controls.Count);
                this.pages.Add(this.orderInformationPage);
                this.pages.Add(this.customerInformationPage);
                this.pages.Add(this.itemDetailsPage);
                this.pages.Add(this.deliveryInformationPage);
                this.pages.Add(this.paymentHistoryPage);
                this.pages.Add(this.cancellationChargePage);
                this.pages.Add(this.summaryPage);

                // Create a collection of view models
                pageViewModels               = new List<PageViewModel>();
                orderDetailsViewModel        = new OrderDetailsViewModel(this.transaction, this.mode);
                orderSummaryViewModel        = new OrderSummaryViewModel(this.transaction);
                customerInformationViewModel = new CustomerInformationViewModel(this.transaction);
                paymentHistoryViewModel      = new PaymentHistoryViewModel(this.transaction);
                itemDetailsViewModel         = new ItemDetailsViewModel(this.transaction);

                this.pageViewModels.Add(orderDetailsViewModel);
                this.pageViewModels.Add(orderSummaryViewModel);
                this.pageViewModels.Add(customerInformationViewModel);
                this.pageViewModels.Add(paymentHistoryViewModel);
                this.pageViewModels.Add(itemDetailsViewModel);

                // Listen to the property changed event on each view model
                this.pageViewModels.ForEach(vm => vm.PropertyChanged += new PropertyChangedEventHandler(OnPageViewModel_PropertyChanged));

                // Set on each page
                this.bindingSource.Add(orderDetailsViewModel);
                this.orderDetailsNavBar.SetViewModel(this.ViewModel);
                this.orderInformationPage.SetViewModel(this.ViewModel);
                this.itemDetailsPage.SetViewModel(itemDetailsViewModel);
                this.deliveryInformationPage.SetViewModel(this.ViewModel);
                this.customerInformationPage.SetViewModel(customerInformationViewModel);
                this.paymentHistoryPage.SetViewModel(paymentHistoryViewModel);
                this.cancellationChargePage.SetViewModel(this.ViewModel);
                this.summaryPage.SetViewModel(orderSummaryViewModel);
                
                this.ViewModel.SelectedPageIndex = GetStartPageIndex();
                this.ViewModel.SelectedPageTitle = this.pages[this.ViewModel.SelectedPageIndex].Text;

                // for a customer order that is not on Create state (i.e. some operation has been performed on it, e.g. processing, packingslip, invoicing, etc)
                // then we cannot let the user click on the save button and edit this order
                bool saveButtonDisabled = this.transaction.Mode == CustomerOrderMode.Edit && this.transaction.OrderStatus != SalesStatus.Created;
                this.btnSave.Enabled = !saveButtonDisabled;
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            this.Text      = LSRetailPosis.ApplicationLocalizer.Language.Translate(1571);  // "Order details"
            btnClear.Text  = LSRetailPosis.ApplicationLocalizer.Language.Translate(56327); // "Clear"
            btnSave.Text   = LSRetailPosis.ApplicationLocalizer.Language.Translate(56318); // "Save"
            btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56319); // "Cancel"
        }

        public CustomerOrderTransaction Transaction
        {
            get { return this.ViewModel.Transaction; }
        }

        private void OnPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PageViewModel pvm = (PageViewModel)sender;

            switch (e.PropertyName)
            {
                case "SelectedPageIndex":   // From OrderDetailsViewModel
                    LoadPage(this.ViewModel.SelectedPageIndex);
                    break;
                case "Transaction":         // From page view models
                    this.pageViewModels.ForEach(a => a.Transaction = pvm.Transaction);
                    this.currentPage.OnActivate();
                    break;
            }
        }

        private int GetStartPageIndex()
        {
            int selectedPageIndex;

            switch (this.mode)
            {
                case OrderDetailsSelection.PickupOrder:
                    selectedPageIndex = pages.IndexOf(itemDetailsPage);
                    break;
                case OrderDetailsSelection.CancelOrder:
                    selectedPageIndex = pages.IndexOf(cancellationChargePage);
                    break;
                default:
                    selectedPageIndex = pages.IndexOf(orderInformationPage);
                    break;
            }

            return selectedPageIndex;
        }

        private OrderDetailsViewModel ViewModel
        {
            get { return (OrderDetailsViewModel)bindingSource.Current; }
        }

        private void LoadPage(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < pages.Count)
            {
                this.tableLayoutPanel.SuspendLayout();

                // Disable the current page
                if (currentPage != null)
                    currentPage.Enabled = false;

                // Set the new current page
                currentPage = pages[pageIndex];
                
                // Update the view model for the current page's title
                this.ViewModel.SelectedPageTitle = currentPage.Text;

                btnUp.Visible    = btnPageUp.Visible = btnDown.Visible = btnPageDown.Visible = currentPage.PageRequiresNavigationButtons();
                btnClear.Enabled = currentPage.IsClearEnabled();
                currentPage.BringToFront();                

                // Give focus to the new page and select the first control in tab order
                currentPage.Enabled = true;
                currentPage.OnActivate();
                currentPage.Focus();
                currentPage.SelectNextControl(null, true, true, true, false);

                // Work around for apparent TableLayout bug that isn't collapsing the AutoSize row containing the arrow buttons
                // when they are invisible. We want the page to fill up the space used by the arrow buttons.
                tableLayoutPanel.SetRowSpan(this.panel, btnUp.Visible ? 1 : 2);

                // Update state of arrow buttons
                UpdateButtons();

                this.tableLayoutPanel.ResumeLayout();
            }
        }

        private void OnClear_Click(object sender, EventArgs e)
        {
            pages[this.ViewModel.SelectedPageIndex].OnClear();
        }

        private void OnUp_Click(object sender, EventArgs e)
        {
            pages[this.ViewModel.SelectedPageIndex].OnUpButtonClicked();

            UpdateButtons();
        }

        private void OnPageUp_Click(object sender, EventArgs e)
        {
            pages[this.ViewModel.SelectedPageIndex].OnPageUpButtonClicked();

            UpdateButtons();
        }

        private void OnDown_Click(object sender, EventArgs e)
        {
            pages[this.ViewModel.SelectedPageIndex].OnDownButtonClicked();

            UpdateButtons();
        }

        private void OnPageDown_Click(object sender, EventArgs e)
        {
            pages[this.ViewModel.SelectedPageIndex].OnPageDownButtonClicked();

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnUp.Enabled       = currentPage.IsUpButtonEnabled();
            btnPageUp.Enabled   = btnUp.Enabled;
            btnDown.Enabled     = currentPage.IsDownButtonEnabled();
            btnPageDown.Enabled = btnDown.Enabled;            
        }        
    }
}