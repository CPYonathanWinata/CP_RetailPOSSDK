/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// View model class for Extended LogOn Dialog
    /// </summary>
    internal sealed class ExtendedLogOnViewModel : INotifyPropertyChanged
    {

        #region Fields

        // SQL exception error numbers
        private const int SQL_ERROR_DUPLICATE_RECORD = 2627;
        private const int SQL_ERROR_NULL_DATA = 515;

        // String message Ids.
        private const int STRING_ALREADY_EXISTS = 99410;
        private const int STRING_STAFF_NOT_FOUND = 99411;
        private const int STRING_SAVE_ERROR = 99412;

        /// Error code returned by TS method call
        /// Linked to AX::RetailTransactionServies::CreateExtendedLogOn Method
        private const int ERROR_STAFF_NOTFOUND = 1;
        private const int ERROR_ALREADY_EXISTS = 2;

        public const int PAGE_SIZE = 50;

        private readonly LogonData logonData = new LogonData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
        private readonly List<OperatorViewModel> results = new List<OperatorViewModel>();
        private bool isLastRowLoaded;
        private int? selectedResult;
        private string sortColumn = "STAFFID";
        private bool sortAsc = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the results.
        /// </summary>
        public ReadOnlyCollection<OperatorViewModel> Results
        {
            get
            {
                return this.results.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the selected result.
        /// </summary>
        /// <remarks>Selected result if available, null otherwise.</remarks>
        public OperatorViewModel SelectedResult
        {
            get
            {
                return (selectedResult.HasValue && selectedResult.Value >= 0)
                    ? this.Results[selectedResult.Value]
                    : null;
            }
        }

        /// <summary>
        /// Gets or sets the operator filter.
        /// </summary>
        public string SearchFilter { get; set; }

        #endregion

        #region Construction

        public ExtendedLogOnViewModel()
        {
            this.SearchFilter = string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the result list.
        /// </summary>
        /// <param name="fromRow">From row.</param>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No user input")]
        private void UpdateResultList(int fromRow)
        {
            NetTracer.Information("ExtendedLogOnViewModel : UpdateResultList : Start");

            if (fromRow == 0)
            {
                this.results.Clear();
                ExecuteSelect(null);
            }

            if (isLastRowLoaded)
            {
                return;
            }

            SqlConnection connection = ApplicationSettings.Database.LocalConnection;
            string query = string.Format(
                @"SELECT DISTINCT STAFFID, NAME, ASSIGNED FROM
                    (SELECT STAFFID, P.NAME, ASSIGNED, ROW_NUMBER() OVER (ORDER BY {0} {1}) AS ROW
                        FROM RETAILSTAFFTABLE AS S
                        INNER JOIN HCMWORKER W ON W.PERSONNELNUMBER = S.STAFFID
                        INNER JOIN DIRPARTYTABLE P ON W.PERSON = P.RECID
                        INNER JOIN DIRADDRESSBOOKPARTY ABP ON P.RECID = ABP.PARTY AND ABP.VALIDFROM <= GETUTCDATE() AND GETUTCDATE() <= ABP.VALIDTO
                        INNER JOIN DIRADDRESSBOOK AB ON ABP.ADDRESSBOOK = AB.RECID
                        INNER JOIN RETAILSTOREADDRESSBOOK SAB ON AB.RECID = SAB.ADDRESSBOOK AND SAB.ADDRESSBOOKTYPE = 1
                        LEFT JOIN (SELECT STAFF, MIN(LOGONTYPE) AS ASSIGNED FROM RETAILSTAFFEXTENDEDLOGON GROUP BY STAFF) E ON E.STAFF = S.RECID
                        WHERE SAB.STORERECID = @STORERECID AND (STAFFID LIKE @SEARCHFILTER OR P.NAME LIKE @SEARCHFILTER)
                    ) RN
                    WHERE RN.ROW >= @FROMROW AND RN.ROW <= @TOROW",
                    sortColumn,
                    sortAsc ? "ASC" : "DESC");

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@STORERECID", ApplicationSettings.Terminal.StorePrimaryId);
                    command.Parameters.AddWithValue("@SEARCHFILTER", String.Format("%{0}%", this.SearchFilter));
                    command.Parameters.AddWithValue("@FROMROW", fromRow);
                    command.Parameters.AddWithValue("@TOROW", (fromRow + PAGE_SIZE));

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.results.Add(new OperatorViewModel(reader));
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // If we didn't get back a full page of results then we loaded everything
            if (this.Results.Count % PAGE_SIZE > 0)
                isLastRowLoaded = true;

            OnPropertyChanged("Results");

            NetTracer.Information("SearchViewModel : UpdateResultList : End");
        }

        /// <summary>
        /// Creates extended log on in AX.
        /// </summary>
        /// <param name="extendedLogOnInfo">The extended log on info.</param>
        /// <returns>The record identifier of newly created extended logon in AX.</returns>
        /// <exception cref="PosisException">Thrown if save failed.</exception>
        private long AxCreateExtendedLogOn(IExtendedLogOnInfo extendedLogOnInfo)
        {
            ReadOnlyCollection<object> containerArray = PosApplication.Instance.TransactionServices.Invoke(
                "CreateSecureExtendedLogon",
                this.SelectedResult.OperatorID,
                extendedLogOnInfo.LogOnKey,
                (int)extendedLogOnInfo.LogOnType,
                extendedLogOnInfo.ExtraData == null ?  string.Empty : Convert.ToBase64String(extendedLogOnInfo.ExtraData));

            // containerArray[0] = boolean: success or failure.
            // containerArray[1] = int: error code if failed.
            // containerArray[2] = RecId of newly created extended log on record if succeeded.

            bool result = Convert.ToBoolean(containerArray[1]);

            if ((!result) || containerArray.Count < 3)
            {
                switch (Convert.ToInt32(containerArray[2]))
                {
                    case ERROR_ALREADY_EXISTS:
                        throw new PosisException() { ErrorMessageNumber = STRING_ALREADY_EXISTS };

                    case ERROR_STAFF_NOTFOUND:
                        throw new PosisException() { ErrorMessageNumber = STRING_STAFF_NOT_FOUND };

                    default:
                        throw new PosisException() { ErrorMessageNumber = STRING_SAVE_ERROR };
                }
            }

            return Convert.ToInt64(containerArray[3]);
        }

        /// <summary>
        /// Delete extended log on in AX.
        /// </summary>
        /// <exception cref="PosisException">Thrown if save failed.</exception>
        private void AxDeleteExtendedLogOn()
        {
            ReadOnlyCollection<object> containerArray = PosApplication.Instance.TransactionServices.Invoke(
                "DeleteExtendedLogon",
                this.SelectedResult.OperatorID,
                0);

            bool result = Convert.ToBoolean(containerArray[1]);

            if (!result)
            {
                throw new PosisException() { ErrorMessageNumber = STRING_SAVE_ERROR };
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Executes the search.
        /// </summary>
        public void ExecuteSearch()
        {
            // Reset last row loaded flag
            isLastRowLoaded = false;

            UpdateResultList(0);
        }

        /// <summary>
        /// Executes the next page.
        /// </summary>
        public void ExecuteNextPage()
        {
            UpdateResultList(results.Count);
        }

        /// <summary>
        /// Executes the clear.
        /// </summary>
        public void ExecuteClear()
        {
            this.SearchFilter = string.Empty;

            this.ExecuteSearch();
        }

        /// <summary>
        /// Executes the select.
        /// </summary>
        /// <param name="selectedIndex">Index of the selected.</param>
        public void ExecuteSelect(int? selectedIndex)
        {
            if (this.selectedResult != selectedIndex)
            {
                this.selectedResult = selectedIndex;

                OnPropertyChanged("SelectedResult");
            }
        }


        /// <summary>
        /// Executes the assign on currently selected operator.
        /// </summary>
        /// <param name="extendedLogOnInfo">The extended log on info.</param>
        /// <exception cref="PosisException">Thrown if save failed.</exception>
        public void ExecuteAssign(IExtendedLogOnInfo extendedLogOnInfo)
        {
            NetTracer.Information("ExtendedLogOnViewModel::ExecuteAssign: Start.");
            bool saved = false;

            try
            {
                logonData.DbUtil.BeginTransaction();
                
                // Save in HQ
                extendedLogOnInfo.RecordId = this.AxCreateExtendedLogOn(extendedLogOnInfo);

                // Create the hash for the logon key to save it locally.
                extendedLogOnInfo.LogOnData = LSRetailPosis.DataAccess.LogonData.ComputePasswordHash(((int)extendedLogOnInfo.LogOnType).ToString(), extendedLogOnInfo.LogOnKey, ApplicationSettings.Terminal.StaffPasswordHashName);                

                // Save a local copy for immediate availability at store.
                logonData.CreateExtendedLogOn(this.SelectedResult.OperatorID, extendedLogOnInfo);

                saved = true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == SQL_ERROR_DUPLICATE_RECORD)
                {
                    throw new PosisException(ex.Number, ex) { ErrorMessageNumber = STRING_ALREADY_EXISTS };
                }
                else if (ex.Number == SQL_ERROR_NULL_DATA)
                {
                    throw new PosisException(ex.Number, ex) { ErrorMessageNumber = STRING_STAFF_NOT_FOUND };
                }

                // Any other Sql error will be thrown as it is.
                throw;
            }
            finally
            {
                if (saved)
                {
                    logonData.DbUtil.EndTransaction();

                    SelectedResult.ExtendedLogOnAssigned = true;
                    OnPropertyChanged("Results");

                    NetTracer.Information("ExtendedLogOnViewModel::ExecuteAssign: Successful.");
                }
                else
                {
                    logonData.DbUtil.CancelTransaction();

                    NetTracer.Error("ExtendedLogOnViewModel::ExecuteAssign: Failed.");
                }
            }
        }

        /// <summary>
        /// Executes the unassign on currently selected operator.
        /// </summary>
        public void ExecuteUnassign()
        {
            NetTracer.Information("ExtendedLogOnViewModel::ExecuteUnassign: Start.");
            bool saved = false;

            try
            {
                logonData.DbUtil.BeginTransaction();

                // Delete in HQ
                this.AxDeleteExtendedLogOn();

                // Delete from local db for immediate availability at store.
                logonData.DeleteExtendedLogOn(this.SelectedResult.OperatorID);

                saved = true;
            }
            finally
            {
                if (saved)
                {
                    logonData.DbUtil.EndTransaction();

                    SelectedResult.ExtendedLogOnAssigned = false;
                    OnPropertyChanged("Results");

                    NetTracer.Information("ExtendedLogOnViewModel::ExecuteUnassign: Successful.");
                }
                else
                {
                    logonData.DbUtil.CancelTransaction();

                    NetTracer.Error("ExtendedLogOnViewModel::ExecuteUnassign: Failed.");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
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

        #endregion

    }

    /// <summary>
    /// Operator view model.
    /// </summary>
    internal sealed class OperatorViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        public OperatorViewModel(IDataReader dataRow)
        {
            this.OperatorID = DBUtil.ToStr(dataRow["STAFFID"]);
            this.OperatorName = DBUtil.ToStr(dataRow["NAME"]);
            this.ExtendedLogOnAssigned = DBUtil.ToBool(dataRow["ASSIGNED"]);
        }

        /// <summary>
        /// Gets or sets the operator ID.
        /// </summary>
        public string OperatorID { get; set; }

        /// <summary>
        /// Gets or sets the name of the operator.
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether extended log on assigned.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification="Used for data binding.")]
        public bool ExtendedLogOnAssigned { get; set; }
    }
}
