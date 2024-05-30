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
using System.Linq;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModel
{
    /// <summary>
    /// View model for report form.
    /// </summary>
    internal sealed class ReportViewModel : INotifyPropertyChanged
    {
        #region Class Variables

        private Collection<POSReport> reports = new Collection<POSReport>();
        private POSReport currentReport = null;
        private string currentView = null;        
        private LogonData logonData = new LogonData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
        private DataManager.ReportsDataManager rdm = new DataManager.ReportsDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);        
        private IList<RetailReportLocalizedString> localizedStringsList;
        private static Type[] numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
        typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
        typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};

        #endregion

        #region Methods
        /// <summary>
        /// Constructor for this view model.
        /// </summary>
        public ReportViewModel()
        {
            Initialize();
        }        

        /// <summary>
        /// Initialize this view model.
        /// </summary>
        private void Initialize()
        {
            //Initialize Reports Collection from DB
            LoadReportsFromDB();           
        }

        /// <summary>
        /// Load POS reports from database.
        /// </summary>
        private void LoadReportsFromDB()
        {            
            ReadOnlyCollection<long> currentPOSPermissionGroups = this.GetPOSPermissionGroupID();
            foreach (DataEntity.RetailReport report in rdm.GetReports())
            {
                POSReport posReport = POSReport.ParseReportDefinitionXml(report.ReportDefinitionXml);
                posReport.Name = report.Name;
                posReport.ID = report.ReportID;          
                posReport.SetRolesAllowed(rdm.GetReportsRolesAllowed(report.Id));
                //Perform role based check.
                bool reportAllowed = false;
                if (posReport.RolesAllowed != null && posReport.RolesAllowed.Count > 0)
                {
                    foreach (Int64 currentGroup in currentPOSPermissionGroups)
                    {
                        foreach (Int64 allowedGroup in posReport.RolesAllowed)
                        {
                            if (allowedGroup == currentGroup)
                            {
                                reportAllowed = true;
                                break;
                            }
                        }
                        if (reportAllowed == true)
                            break;
                    }
                }

                if (posReport.DataSourceType.Equals("OLTP", StringComparison.InvariantCultureIgnoreCase) && reportAllowed)
                {
                    this.reports.Add(posReport);
                }
            }

            if (this.reports.Count > 0)
            {
                LoadLocalizedStringsFromDB(ApplicationSettings.Terminal.CultureName);
            }
        }

        /// <summary>
        /// Load localized strings from rpeort to DB.
        /// </summary>
        /// <param name="locale"></param>
        private void LoadLocalizedStringsFromDB(string locale)
        {
            localizedStringsList = rdm.GetLocalizedStrings(locale);
            foreach (var report in this.reports)
            {
                report.Title = this.GetLocalizedLabel(report.Title);
                foreach (var parameter in report.Parameters)
                {
                    parameter.Label = this.GetLocalizedLabel(parameter.Label);
                }
            }
        }

        /// <summary>
        /// Get permission group ID from database for this logged in user.
        /// </summary>
        /// <returns>Current POS permission group Id.</returns>
        public ReadOnlyCollection<Int64> GetPOSPermissionGroupID()
        {            
            string storeId = ApplicationSettings.Terminal.StoreId;
            string operatorId = ApplicationSettings.Terminal.TerminalOperator.OperatorId;                       
            return logonData.GetPOSPermissionGroupIds(storeId, operatorId);                  
        }



        /// <summary>
        /// Get report collection.
        /// </summary>
        /// <returns>Collection of POS reports.</returns>
        public Collection<POSReport> GetReports()
        {
            return reports;
        }

        /// <summary>
        /// Get current report.
        /// </summary>
        /// <returns>Get current selected POS report.</returns>
        public POSReport GetCurrentReport()
        {
            return currentReport;
        }

        /// <summary>
        /// Set the report selected by user as Current report.
        /// </summary>
        /// <param name="reportID"></param>
        public void SetCurrentReport(string reportID)
        {
            currentReport = reports.Where(p => p.ID == reportID).First();
        }

        /// <summary>
        /// Get current report's charts.
        /// </summary>
        /// <returns>Get current selected POS report's charts.</returns>
        public Collection<POSReportChart> GetCurrentReportCharts(DataTable outputDataTable)
        {
            if (outputDataTable == null)
            {
                return null;
            }

            return this.currentReport.Charts;
        }  
       

        /// <summary>
        /// Set the current view displayed.
        /// </summary>
        /// <param name="reportID"></param>
        public void SetCurrentView(string viewTag)
        {
            currentView = viewTag;
        }

        /// <summary>
        /// Get the current view displayed.
        /// </summary>
        public string GetCurrentView()
        {
            return currentView;
        }

        /// <summary>
        /// Get localized labels for the string.
        /// </summary>
        /// <param name="labelId">Label identifier.</param>
        /// <returns>Localized label</returns>
        public string GetLocalizedLabel(string labelId)
        {
                IEnumerable<string> localValues = from r in this.localizedStringsList
                                     where (r.StringId.Equals(labelId, StringComparison.InvariantCultureIgnoreCase))
                                     select r.StringValue;
                if (localValues.Count<string>() > 0)
                    return localValues.First<String>();
                else //If label is not localized, return the original value.
                    return labelId;
        }

        /// <summary>
        /// Check if data column is numeric.
        /// </summary>
        /// <param name="columnType">Data column type.</param>
        /// <returns>Is data column numeric.</returns>
        public static bool IsNumeric(Type columnType)
        {
            return numericTypes.Contains(columnType);
        }

        /// <summary>
        /// Return datatable to be bound to the report list grid view.
        /// </summary>
        /// <returns>Datatable containing report list.</returns>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification="It is caller's responsibility to dispose this datatable.")]
        public DataTable GetReportList()
        {
            DataTable reportList = new DataTable();            
            reportList.Columns.Add("ReportId");
            reportList.Columns.Add("ReportTitle");
            foreach (POSReport report in this.reports)
            {
                DataRow row = reportList.NewRow();
                row[0] = report.ID;
                row[1] = this.GetLocalizedLabel(report.Title);
                reportList.Rows.Add(row);
            }

            return reportList;            
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
}
