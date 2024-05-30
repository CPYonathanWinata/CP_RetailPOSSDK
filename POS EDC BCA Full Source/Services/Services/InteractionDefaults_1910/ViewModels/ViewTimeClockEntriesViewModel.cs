/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/



namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using LSRetailPosis.DataAccess.DataUtil;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Settings;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.DataManager;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// View model class for view time clock entries form, which is use to know the the status.
    /// </summary>
    internal sealed class ViewTimeClockEntriesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Returned from transaction service and this value is used to find result state, like true/false.
        /// </summary>
        private const int ResultIndex = 1;
        private DataTable timeClockEntries;

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor ViewTimeClockEntriesViewModel.
        /// </summary>
        /// <param name="storeID">Selected store.</param>
        /// <param name="registrationTypeFilters">CSV of TimeClockType filters</param>
        public ViewTimeClockEntriesViewModel(string storeId, TimeClockType[] registrationTypeFilters)
        {
            ViewTimeClock(storeId, registrationTypeFilters, null); //We will pass empty string other then break activity
        }

        /// <summary>
        /// This method used to get all user details for manager logins.
        /// </summary>
        /// <param name="storeId">Selected storeID (may be empty string)</param>
        /// <param name="registrationType">CSV of TimeClockType filters</param>
        /// <param name="breakActivity">Break activity like, Break for lunch/break from work.</param>
        public void ViewTimeClock(string storeId, TimeClockType [] registrationTypeFilters, string breakActivity)
        {
            if (storeId == null)
            {
                throw new ArgumentNullException("storeId");
            }

            if (registrationTypeFilters == null)
            {
                throw new ArgumentNullException("registrationTypeFilters");
            }

            var employeeDataManager = new EmployeeDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            var employee = employeeDataManager.GetEmployee(ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalOperator.OperatorId);

            // if current employee (store manager) is not assigned to the store specified, throw an exception
            if (!string.IsNullOrEmpty(storeId) && !employeeDataManager.IsStoreAssignedToEmployee(storeId, employee.Worker.PersonId))
            {
                // The store is unavailable for the current user.
                using (frmMessage dialog = new frmMessage(99872, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }

            // if empty store number is passed, get the list of all stores that store manager is assigned to
            if (string.IsNullOrEmpty(storeId))
            {
                storeId = string.Join(",", employeeDataManager.GetStores(employee.Worker.PersonId).Select(e => e.Number));
            }

            string methodName = "getManagerHistoryUtc";
            string regTypes = String.Join(",", Array.ConvertAll(registrationTypeFilters, value => (int)value)); // Cast as int CSV;
            int skip = 0;   // skip 0 record
            int top = 0;    // top of 0 is to select all records

            ReadOnlyCollection<object> containerArray = null;

            containerArray = PosApplication.Instance.TransactionServices.Invoke(
                methodName,
                string.Empty, //We will pass operator ID(2nd parameter) as blank as managers can view all other users activities
                storeId, 
                regTypes, 
                DateTime.Today.AddDays(-90).ToUniversalTime(), // Previous 90 days history
                DateTime.Today.AddDays(1).ToUniversalTime(),   // Up to and including the end of of the current day
                string.IsNullOrWhiteSpace(breakActivity) ? string.Empty : GetJobID(breakActivity), // Get JobID when breakActivity
                skip,
                top
                ); 

            bool retValue = Convert.ToBoolean(containerArray[ResultIndex]);

            if (retValue)
            {
                this.TimeClockEntries = FillTimeClockEntryTable(containerArray);
            }
        }

        /// <summary>
        /// Get the time clock entries for managers.
        /// </summary>
        public DataTable TimeClockEntries
        {
            get { return timeClockEntries; }
            set
            {
                if (timeClockEntries != value)
                {
                    timeClockEntries = value;
                    OnPropertyChanged("TimeClockEntries");
                }
            }
        }

        #region private methods
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private static DataTable FillTimeClockEntryTable(ReadOnlyCollection<object> containerArray)
        {
            int registrationType;
            DataTable timeClockEntries = new DataTable();
            timeClockEntries.Columns.Add("WORKER", typeof(string));
            timeClockEntries.Columns.Add("PERSONNEL", typeof(string));
            timeClockEntries.Columns.Add("PROFILE", typeof(string));
            timeClockEntries.Columns.Add("CLOCKINTIME", typeof(string));
            timeClockEntries.Columns.Add("CLOCKOUTTIME", typeof(string));
            timeClockEntries.Columns.Add("ACTIVITY", typeof(string));
            timeClockEntries.Columns.Add("STORE", typeof(string));

            for (int i = 3; i < containerArray.Count; i++)
            {
                IList timeClockEntryRecord = (IList)containerArray[i];

                DataRow row = timeClockEntries.NewRow();

                row["WORKER"] = ConvertToStringAtIndex(timeClockEntryRecord, 0);
                row["PERSONNEL"] = ConvertToStringAtIndex(timeClockEntryRecord, 1);
                row["PROFILE"] = ConvertToStringAtIndex(timeClockEntryRecord, 2);
                row["CLOCKINTIME"] = ConvertToDateTimeAtIndex(timeClockEntryRecord, 3);
                row["CLOCKOUTTIME"] = ConvertToDateTimeAtIndex(timeClockEntryRecord, 4);
                registrationType = Convert.ToInt32(ConvertToStringAtIndex(timeClockEntryRecord, 5));
                row["ACTIVITY"] = ConvertToTimeClockType(registrationType);
                row["STORE"] = ConvertToStringAtIndex(timeClockEntryRecord, 6);

                timeClockEntries.Rows.Add(row);
            }
            return timeClockEntries;
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

        private static TimeClockType ConvertToTimeClockType(int intValue)
        {
            TimeClockType enumValue = TimeClockType.None;
            if (Enum.IsDefined(typeof(TimeClockType), enumValue))
            {
                enumValue = (TimeClockType)(intValue);
            }

            return enumValue;
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by data binding")]
        private void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new ArgumentException(msg);
            }
        }

        /// <summary>
        /// Gets the jobID for break activity.
        /// </summary>
        /// <param name="jobActivity">Job activity like, BreakForLunch\BreakFromWork.</param>
        /// <returns>JobID for a specific break activity.</returns>
        private string GetJobID(string jobActivity)
        {
            string jobID = string.Empty;
            SqlConnection connection = ApplicationSettings.Database.LocalConnection;

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    GetQuery(sqlCommand);
                    sqlCommand.Parameters.AddWithValue("@ACTIVITY", jobActivity);
                    sqlCommand.Connection = connection;
                    sqlCommand.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                        jobID = DBUtil.ToStr(sqlCommand.ExecuteScalar());
                    }
                }
                return jobID;
            }

            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Grandfather")]
        private void GetQuery(SqlCommand command)
        {
            string query = string.Format(
                @"SELECT JOBID
                        FROM JMGIPCACTIVITY 
                        WHERE DATAAREAID = @DATAAREAID
                            AND ACTIVITY = @ACTIVITY");
            command.CommandText = query;
        }
        #endregion

    }
}
