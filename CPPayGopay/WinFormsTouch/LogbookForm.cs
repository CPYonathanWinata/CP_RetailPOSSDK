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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Settings;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.DataManager;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    public partial class LogbookForm : frmTouchBase
    {
        private const int LastDay = 1;
        private const int LastWeek = 7;
        private const int LastMonth = 30;

        // Tolerance for time filter.  For example, check-in on one machine, go to another and see log book entry, 
        // if the machines have a small time difference we still want to show receint time clock operations.
        private const int TimeToleranceInMinutes = 30;         

        private StoreDataManager storeDataManager;
        private IList<Store> stores;
        private Dictionary<string, int> storeIndexByNumber;
        private DataTable storeTable;
        private Store selectedStore;
        private int lastDaysCount;

        public LogbookForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.lastDaysCount = LogbookForm.LastDay;

                LoadStores();
                LoadLogbook();
                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void LoadStores()
        {
            this.storeDataManager = new StoreDataManager(
                PosApplication.Instance.Settings.Database.Connection,
                PosApplication.Instance.Settings.Database.DataAreaID);

            this.stores = this.storeDataManager.GetStores();

            this.storeIndexByNumber = new Dictionary<string, int>();
            for (int i = 0; i < this.stores.Count; i++)
            {
                Store store = stores[i];
                this.storeIndexByNumber[store.Number] = i;
            }

            this.selectedStore = this.storeDataManager.GetStore(
                ApplicationSettings.Terminal.StoreId);
           
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

        /// <summary>
        /// This method loads the logbook and disposes the old datasource on the grid.
        /// </summary>
        private void LoadLogbook()
        {
            this.gridLogbook.SuspendLayout();

            IDisposable logbook = this.gridLogbook.DataSource as IDisposable;
            if (logbook != null)
            {
                logbook.Dispose();
            }

            string storeId = (this.selectedStore != null)
                ? this.selectedStore.Number
                : PosApplication.Instance.Shift.StoreId;

            this.gridLogbook.DataSource = Logbook.LoadLogbook(
                ApplicationSettings.Terminal.TerminalOperator.OperatorId,
                storeId,
                DateTime.UtcNow.AddDays(-this.lastDaysCount).AddMinutes(-TimeToleranceInMinutes),  // previous lastDaysCount (by 24-hour days not calendar days) with a small tolerance
                DateTime.UtcNow.AddMinutes(TimeToleranceInMinutes)); // until current time (with a small tolerance into the future to account for small time differences on differnt machines

            this.gridLogbook.ResumeLayout();
        }

        private void TranslateLabels()
        {
            this.btnClose.Text = ApplicationLocalizer.Language.Translate(51359); // "Close"
            this.btnLastDay.Text = ApplicationLocalizer.Language.Translate(51360); // "Last 24 hours"
            this.btnLastMonth.Text = ApplicationLocalizer.Language.Translate(51362); // "Last 30 days"
            this.btnLastWeek.Text = ApplicationLocalizer.Language.Translate(51361); // "Last 7 days"
            this.btnSelectStore.Text = ApplicationLocalizer.Language.Translate(51363); // "Select store"
            this.colActivityType.Caption = ApplicationLocalizer.Language.Translate(51368); // Activty type
            this.colActivity.Caption    =ApplicationLocalizer.Language.Translate(51364); // Activity
            this.colDateTime.Caption = ApplicationLocalizer.Language.Translate(51369); //Start Date
            this.colStore.Caption = ApplicationLocalizer.Language.Translate(51370); // "Store"

            this.labelTitle.Text = ApplicationLocalizer.Language.Translate(51371); // "Logbook"
        }

        private void btnLastDay_Click(object sender, EventArgs e)
        {
            this.lastDaysCount = LogbookForm.LastDay;
            this.LoadLogbook();
        }

        private void btnLastWeek_Click(object sender, EventArgs e)
        {
            this.lastDaysCount = LogbookForm.LastWeek;
            this.LoadLogbook();
        }

        private void btnLastMonth_Click(object sender, EventArgs e)
        {
            this.lastDaysCount = LogbookForm.LastMonth;
            this.LoadLogbook();
        }

        private void btnSelectStore_Click(object sender, EventArgs e)
        {
            DataRow dataRow = this.storeTable.NewRow();
            DialogResult result = PosApplication.Instance.Services.Dialog.GenericSearch(storeTable, ref dataRow);

            if (result == DialogResult.OK)
            {
                this.selectedStore = this.stores[this.storeIndexByNumber[dataRow["NUMBER"].ToString()]];

                LoadLogbook();
            }
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            this.gridView.MoveNextPage();
            this.CheckRowPosition();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            this.gridView.MoveNext();
            this.CheckRowPosition();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            this.gridView.MovePrev();
            this.CheckRowPosition();
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            this.gridView.MovePrevPage();
            this.CheckRowPosition();
        }

        private void CheckRowPosition()
        {
            this.btnPgDown.Enabled = !gridView.IsLastRow;
            this.btnDown.Enabled = btnPgDown.Enabled;
            this.btnPgUp.Enabled = !gridView.IsFirstRow;
            this.btnUp.Enabled = btnPgUp.Enabled;
        }

        /// <summary>
        /// This class deals with Transaction Service calls
        /// </summary>
        internal static class Logbook
        {
            [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "It's the caller responsibility to dispose the object.")]
            internal static DataTable LoadLogbook(string workerID, string storeID, DateTime fromDate, DateTime toDate)
            {
                string methodName = "getWorkerHistoryUtc";
                int skip = 0;   // skip 0 record
                int top = 0;    // top of 0 is to select all records
                ReadOnlyCollection<object> containerArray = null;

                containerArray = PosApplication.Instance.TransactionServices.Invoke(
                    methodName, workerID, storeID, fromDate, toDate, skip, top);

                bool retValue = Convert.ToBoolean(containerArray[1]);

                DataTable logbook = new DataTable();
                FillLogbookDataTable(containerArray, logbook);

                return logbook;
            }

            private static void FillLogbookDataTable(ReadOnlyCollection<object> containerArray, DataTable logbook)
            {
                int registrationType;
                logbook.Columns.Add("ACTIVITYTYPE", typeof(string));
                logbook.Columns.Add("ACTIVITY", typeof(string));
                logbook.Columns.Add("DATETIME", typeof(string));
                logbook.Columns.Add("STORE", typeof(string));

                for (int i = 3; i < containerArray.Count; i++)
                {
                    IList logBookRecord = (IList)containerArray[i];

                    DataRow row = logbook.NewRow();
                   
                    registrationType = Convert.ToInt32(ConvertToStringAtIndex(logBookRecord, 0));

                    row["ACTIVITYTYPE"] = ConvertToStringAtIndex(logBookRecord, 1);
                    row["ACTIVITY"]     = ConvertToStringAtIndex(logBookRecord, 2);
                    row["DATETIME"]     = ConvertToDateTimeAtIndex(logBookRecord, 3);
                    row["STORE"]        = ConvertToStringAtIndex(logBookRecord, 4);

                    logbook.Rows.Add(row);
                }
            }

            private static string ConvertToStringAtIndex(IList list, int index)
            {
                try
                {
                    return Convert.ToString(list[index]);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            private static DateTime ConvertToDateTimeAtIndex(IList list, int index)
            {
                try
                {

                    return Convert.ToDateTime(list[index]).ToLocalTime();
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }
    }
}
