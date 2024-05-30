/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Data;
    using System.Windows.Forms;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.DataManager;
    using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    [Export("ViewTimeClockEntriesForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ViewTimeClockEntriesForm : frmTouchBase, IInteractionView
    {
        /// <summary>
        /// Returned from transaction service and this value is used to find result state, like true/false.
        /// </summary>
        private const int ResultIndex = 1;

        /// <summary>
        /// Returned from transaction service and this value is used to find error message.
        /// </summary>
        private const int ErrorMessageIndex = 2;

        private ViewTimeClockEntriesViewModel viewModel;
        private StoreDataManager storeDataManager;
        private IList<Store> stores;
        private Dictionary<string, int> storeIndexByNumber;
        private DataTable storeTable;
        private Store selectedStore;
        private TimeClockType[] registrationTypeFilter;
        private const string BreakForLunch = "LunchBrk"; // This value is from JmgIpcActivity table, this is "Break for lunch" 
        private const string BreakFromWork = "DailyBrks"; // This value is from JmgIpcActivity table, this is "Break from work"
        private string selectedBreakActivity = string.Empty;       // This holds value of currently selected break activity only.        
        private bool isSelectAllStoreButtonSelected = false; //This is to make sure, the last button pressed was Selectallstore button.

        public ViewTimeClockEntriesForm()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewTimeClockEntriesForm"/> class.
        /// </summary>
        /// <param name="ViewTimeClockEntriesNotification">View time clock entries notification.</param>
        [ImportingConstructor]
        public ViewTimeClockEntriesForm(ViewTimeClockEntriesNotification ViewTimeClockEntriesNotification)
            : this()
        {
            if (ViewTimeClockEntriesNotification == null)
            {
                throw new ArgumentNullException("ViewTimeClockEntriesNotification");
            }
        }

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type.</typeparam>
        /// <param name="args">Notification.</param>
        public void Initialize<TArgs>(TArgs args)
            where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call.
        /// </summary>
        /// <typeparam name="TResults">The notification result.</typeparam>
        /// <returns>Returns the TResults object.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new ViewTimeClockEntriesNotification() as TResults;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                LoadStores();

                string storeId = (this.selectedStore != null)
                   ? this.selectedStore.Number
                   : string.Empty;

                // Initilize filter
                this.registrationTypeFilter = new TimeClockType[] { TimeClockType.ClockIn, TimeClockType.BreakFlowStop };

                // Create view model
                viewModel = new ViewTimeClockEntriesViewModel(storeId, this.registrationTypeFilter);
                bindingSource.Add(viewModel);

                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void LoadStores()
        {
            this.storeDataManager = new StoreDataManager(
                    PosApplication.Instance.Settings.Database.Connection,
                    PosApplication.Instance.Settings.Database.DataAreaID);

            var employeeDataManager = new EmployeeDataManager(
                    PosApplication.Instance.Settings.Database.Connection,
                    PosApplication.Instance.Settings.Database.DataAreaID);

            var employee = employeeDataManager.GetEmployee(PosApplication.Instance.Shift.StoreId, PosApplication.Instance.Shift.StaffId);
            this.stores = employeeDataManager.GetStores(employee.Worker.PersonId);

            this.storeIndexByNumber = new Dictionary<string, int>();
            for (int i = 0; i < this.stores.Count; i++)
            {
                Store store = stores[i];
                this.storeIndexByNumber[store.Number] = i;
            }

            this.selectedStore = this.storeDataManager.GetStore(
                PosApplication.Instance.Shift.StoreId);

            CreateStoreTable();
        }

        private void CreateStoreTable()
        {
            if (this.storeTable != null)
            {
                this.storeTable.Dispose();
            }

            this.storeTable = new DataTable();
            storeTable.Columns.Add("NUMBER").Caption = ApplicationLocalizer.Language.Translate(51374); // "Number"
            storeTable.Columns.Add("NAME").Caption = ApplicationLocalizer.Language.Translate(51373); // "Name"
            storeTable.Columns.Add("ADDRESS").Caption = ApplicationLocalizer.Language.Translate(51372); // "Address"

            foreach (Store store in this.stores)
            {
                DataRow row = storeTable.NewRow();
                row["NUMBER"] = store.Number;
                row["NAME"] = store.Name;
                row["ADDRESS"] = store.Address;

                storeTable.Rows.Add(row);
            }
        }

        private void SelectByRegisterType(string breakActivity)
        {
            string storeId = (this.selectedStore != null)
                  ? this.selectedStore.Number
                  : string.Empty;

            viewModel.ViewTimeClock(storeId, this.registrationTypeFilter, breakActivity);
        }

        private void CheckRowPosition()
        {
            this.btnPgDown.Enabled = !gridView.IsLastRow;
            this.btnDown.Enabled = btnPgDown.Enabled;
            this.btnPgUp.Enabled = !gridView.IsFirstRow;
            this.btnUp.Enabled = btnPgUp.Enabled;
        }

        private void OnButtonClockIn_Click(object sender, EventArgs e)
        {
            this.registrationTypeFilter = new TimeClockType[] { TimeClockType.ClockIn, TimeClockType.BreakFlowStop };
            this.selectedBreakActivity = string.Empty;
            SelectByRegisterType(null); //We will pass null for other then break activity.
        }

        private void OnButtonClockOut_Click(object sender, EventArgs e)
        {
            this.registrationTypeFilter = new TimeClockType[] { TimeClockType.ClockOut };
            this.selectedBreakActivity = string.Empty;
            SelectByRegisterType(null); //We will pass null for other then break activity.
        }

        private void OnButtonBreakForLunch_Click(object sender, EventArgs e)
        {
            this.registrationTypeFilter = new TimeClockType[] { TimeClockType.BreakFlowStart } ;
            this.selectedBreakActivity = BreakForLunch;
            SelectByRegisterType(BreakForLunch);
        }

        private void OnButtonBreakFromWork_Click(object sender, EventArgs e)
        {
            this.registrationTypeFilter = new TimeClockType[] { TimeClockType.BreakFlowStart };
            this.selectedBreakActivity = BreakFromWork;
            SelectByRegisterType(BreakFromWork);
        }

        private void OnButtonSelectStore_Click(object sender, EventArgs e)
        {
            DataRow dataRow = this.storeTable.NewRow();
            DialogResult result = PosApplication.Instance.Services.Dialog.GenericSearch(storeTable, ref dataRow, ApplicationLocalizer.Language.Translate(99711));

            if (result == DialogResult.OK)
            {
                this.selectedStore = this.stores[this.storeIndexByNumber[dataRow["NUMBER"].ToString()]];
                isSelectAllStoreButtonSelected = false;
            }
            else
            {   // If user press cancel on "Select Store" button, then button checked state should be for button "btnSelectAllStore".                
                this.BeginInvoke(new MethodInvoker(SetStoreButtonCheckedUnchecked));
            }
            string storeId = (this.selectedStore != null)
                 ? this.selectedStore.Number
                 : string.Empty;

            viewModel.ViewTimeClock(storeId, this.registrationTypeFilter, this.selectedBreakActivity);
        }

        private void SetStoreButtonCheckedUnchecked()
        {
            if (isSelectAllStoreButtonSelected) //this will make sure, the last button press was Selectallstore button
            {
                btnSelectAllStore.Checked = true;
                btnSelectAllStore.Focus();
            }
            else
            {
                btnSelectStore.Checked = true;
                btnSelectStore.Focus();
            }
        }

        private void OnButtonSelectAllStore_Click(object sender, EventArgs e)
        {
            this.selectedStore = null;

            string storeId = (this.selectedStore != null)
                ? this.selectedStore.Number
                : string.Empty;

            viewModel.ViewTimeClock(storeId, this.registrationTypeFilter , selectedBreakActivity);
            isSelectAllStoreButtonSelected = true;
        }

        private void OnButtonPgUp_Click(object sender, EventArgs e)
        {
            this.gridView.MovePrevPage();
            this.CheckRowPosition();
        }

        private void OnButtonUp_Click(object sender, EventArgs e)
        {
            this.gridView.MovePrev();
            this.CheckRowPosition();
        }

        private void OnButtonDown_Click(object sender, EventArgs e)
        {
            this.gridView.MoveNext();
            this.CheckRowPosition();
        }

        private void OnButtonPgDown_Click(object sender, EventArgs e)
        {
            this.gridView.MoveNextPage();
            this.CheckRowPosition();
        }

        private void TranslateLabels()
        {

            this.btnClockIn.Text = ApplicationLocalizer.Language.Translate(99701); //Clock in
            this.btnClockOut.Text = ApplicationLocalizer.Language.Translate(99702); //Clock out
            this.btnBreakFromWork.Text = ApplicationLocalizer.Language.Translate(99703); //Break from work
            this.btnBreakForLunch.Text = ApplicationLocalizer.Language.Translate(99704); //Break for lunch
            this.btnSelectStore.Text = ApplicationLocalizer.Language.Translate(99705); //Select store
            this.btnSelectAllStore.Text = ApplicationLocalizer.Language.Translate(99706); //Select all store
            this.btnClose.Text = ApplicationLocalizer.Language.Translate(99707); // Close
            this.labelTitle.Text = ApplicationLocalizer.Language.Translate(99708); // View time clock entries

            this.colClockInTime.Caption = ApplicationLocalizer.Language.Translate(10076); // Time
            this.colClockOutTime.Visible = false; // Hide the column
        }
    }
}
