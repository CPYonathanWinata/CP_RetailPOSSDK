/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using LSRetailPosis.Settings;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// View model class for time clock, which is use to know the current status\register status
    /// </summary>
    internal sealed class RegisterTimeFormViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Returned from transaction service and this value is used to find result state, like true/false.
        /// </summary>
        private const int ResultIndex = 1;

        /// <summary>
        /// Returned from transaction service and this value is used for error message if any.
        /// </summary>
        private const int ErrorMessageIndex = 2;

        /// <summary>
        /// Returned from transaction service and this value has lastactivity.
        /// </summary>
        private const int LastActivityIndex = 3;

        /// <summary>
        /// Returned from transaction service and this value has value like timeclock type.
        /// </summary>
        private const int RegisterTimeTypeIndex = 4;

        private const int RegisterBreakTypeIndex = 5;
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private TimeClockType clockRegisterType =   TimeClockType.None;
        private string lastActivity;

        private const string BreakForLunch = "LunchBrk";
        private const string BreakFromWork = "DailyBrks";

        private string JobId; 
        private string BreakActivity;
        private string breakRegisterType;

        /// <summary>
        /// Constructor to load current time registration state.
        /// </summary>
        public RegisterTimeFormViewModel()
        {
            ReadOnlyCollection<object> containerArray = PosApplication.Instance.TransactionServices.Invoke(
                "getWorkerCurrentTimeRegistrationState", ApplicationSettings.Terminal.TerminalOperator.OperatorId, ApplicationSettings.Terminal.TerminalId);

            bool result = Convert.ToBoolean(containerArray[ResultIndex]);
            if (result)
            {
                try
                {
                    this.LastActivity = Convert.ToDateTime(containerArray[LastActivityIndex]).ToLocalTime().ToString();
                }
                catch
                {
                    this.LastActivity = containerArray[LastActivityIndex].ToString();
                }
                this.RegisterTimeType = (TimeClockType)containerArray[RegisterTimeTypeIndex];
                
                if ((TimeClockType)containerArray[RegisterTimeTypeIndex] == TimeClockType.BreakFlowStart) 
                {   //This will execute on form load and when user allready registered only with BreakForLunch/BreakFromWork
                    this.JobId = Convert.ToString(containerArray[RegisterBreakTypeIndex]);
                    GetActivity(string.Empty, TimeClockType.BreakFlowStart); //To find the activity only job id required.

                    this.RegisterBreakType = BreakActivity;
                }
            }
        }

        /// <summary>
        /// Gets the type of the log on.
        /// </summary>
        public TimeClockType RegisterTimeType
        {
            get
            {
                return clockRegisterType;
            }
            private set
            {
                if (clockRegisterType != value)
                {
                    clockRegisterType = value;
                    OnPropertyChanged("RegisterTimeType");
                }
            }
        }
        /// <summary>
        /// Gets the type of break.
        /// </summary>
        public string RegisterBreakType
        {
            get
            {
                return breakRegisterType;
            }
            private set
            {
                if (!string.Equals(breakRegisterType, value, StringComparison.Ordinal))
                {
                    breakRegisterType = value;
                    OnPropertyChanged("RegisterBreakType");
                }
            }
        }

        /// <summary>
        /// Gets or sets the last activity description
        /// </summary>
        public string LastActivity
        {
            get { return lastActivity; }
            set
            {
                if (!string.Equals(lastActivity, value, StringComparison.Ordinal))
                {
                    lastActivity = value;
                    OnPropertyChanged("LastActivity");
                }
            }
        }

        /// <summary>
        /// Gets or sets the clockin\clockout
        /// </summary>
        /// <param name="registerTimeType">Time clock type.</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool RegisterTimeClock(TimeClockType registerTimeType)
        {
            string method = null;
            switch (registerTimeType)
            {
                case TimeClockType.ClockIn:
                    method = "clockIn";
                    break;
                case TimeClockType.ClockOut:
                    method = "clockOut";
                    break;
            }

            return RegisterTimeClockOperation(registerTimeType, method);
        }

        private bool RegisterTimeClockOperation(TimeClockType registerTimeType,  string method)
        {
            ReadOnlyCollection<object> containerArray = null;
            string workerId = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
            string terminal = Convert.ToString(ApplicationSettings.Terminal.TerminalId);


            if (registerTimeType == TimeClockType.BreakFlowStart)
            {
                containerArray = PosApplication.Instance.TransactionServices.Invoke(
                    method, workerId, terminal, this.JobId);
            }
            else
            {
                containerArray = PosApplication.Instance.TransactionServices.Invoke(
                    method, workerId, terminal);
            }

            bool result = Convert.ToBoolean(containerArray[ResultIndex]);

            //if result is false then container array will have error message in [ErrorMessageIndex]=2
            //else container array will have last activity in [LastActivityIndex]=3
            if (!result)
            {
                this.LastActivity = containerArray[ErrorMessageIndex].ToString();
            }
            else
            {
                try
                {
                    this.LastActivity = Convert.ToDateTime(containerArray[LastActivityIndex]).ToLocalTime().ToString();
                }
                catch
                {
                    this.LastActivity = containerArray[LastActivityIndex].ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the break types.
        /// </summary>
        /// <param name="registerTimeType">Break type.</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool RegisterTimeClock(BreakType breakType)
        {
            string method = "startBreak";
            bool result;
            switch (breakType)
            {
                case BreakType.BreakFromWork:
                    GetActivity(BreakFromWork, TimeClockType.None); //set the JobId. To get the JobID time clock type is not required.
                    result = RegisterTimeClockOperation(TimeClockType.BreakFlowStart, method);
                    return result;
                case BreakType.BreakForLunch:
                    GetActivity(BreakForLunch, TimeClockType.None); //set the JobId. To get the JobID time clock type is not required.
                    result = RegisterTimeClockOperation(TimeClockType.BreakFlowStart, method);
                    return result;
            }

            return true;
        }

        private void GetActivity(string activity, TimeClockType timeClockType)
        {
            SqlConnection connection = ApplicationSettings.Database.LocalConnection;

            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    if (timeClockType == TimeClockType.BreakFlowStart)
                    {//this is to get activity message from db (DailyBrks/LunchBrk)
                        GetActivityMessage(command);
                        command.Parameters.AddWithValue("@JOBID", this.JobId);
                    }
                    else
                    {//this is to get JobId
                        GetQuery(command);
                        command.Parameters.AddWithValue("@ACTIVITY", activity);
                    }
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (timeClockType == TimeClockType.BreakFlowStart)
                            {
                                this.BreakActivity = Convert.ToString(reader["ACTIVITY"]);
                            }
                            else
                            {
                                this.JobId = Convert.ToString(reader["JOBID"]);
                            }
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
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void GetQuery(SqlCommand command)
        {
            string query = string.Format(
                @"SELECT ACTIVITY, JOBID
                        FROM JMGIPCACTIVITY 
                        WHERE CATEGORY ='BREAK' 
                            AND JMGIPCACTIVITY.BREAKDROP = 0 
                            AND JMGIPCACTIVITY.SIGNIN = 1
                            AND DATAAREAID = @DATAAREAID
                            AND ACTIVITY = @ACTIVITY");
            command.CommandText = query;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void GetActivityMessage(SqlCommand command)
        {
            string query = string.Format(
                @"SELECT ACTIVITY, JOBID
                        FROM JMGIPCACTIVITY 
                        WHERE CATEGORY ='BREAK' 
                            AND JMGIPCACTIVITY.BREAKDROP = 0 
                            AND JMGIPCACTIVITY.SIGNIN = 1
                            AND DATAAREAID = @DATAAREAID
                            AND JOBID = @JOBID");
            command.CommandText = query;
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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

        #endregion
    }
}
