/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Xml;
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{    
    /// <summary>
    /// Class that encapsulate report object.
    /// </summary>
    [Serializable]
    public class POSReport
    {
        private const string ParamChannelId = "bi_ChannelId";
        private const string ParamUserId = "nvc_UserId";
        private const string XmlParameterLabel = "Label";
        private const string XmlParameterName = "Name";
        private const string XmlParameterType = "DataType";
        private const string XmlParameterDefaultValue = "DefaultValue";
        private const string XmlDatasourceType = "DataSourceType";
        private const string XmlParameter = "ReportParameter";
        private const string XmlCharts = "ReportCharts";
        private const string XmlChart = "ReportXYChart";
        private const string XmlCategories = "Categories";
        private const string XmlSeries = "Series";
        private const string XmlQuery = "Query";
        private const string XmlTitle = "Title";
        private const string XmlDataset = "DataSet";
        private const string XmlParameters = "ReportParameters";
        private const string XmlReport = "RetailReport";
        private const string XmlIsUserBasedReport = "IsUserBasedReport";

        /// <summary>
        /// Report ID.
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// Report Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Report Title.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Report Parameters.
        /// </summary>
        public Collection<POSReportParameter> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Report Charts.
        /// </summary>
        public Collection<POSReportChart> Charts
        {
            get;
            private set;
        }

        /// <summary>
        /// Report DataSource.
        /// </summary>
        public string DataSourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Report Query.
        /// </summary>
        public string Query
        {
            get;
            set;
        }

        /// <summary>
        /// Report roles allowed.
        /// </summary>        
        public ReadOnlyCollection<Int64> RolesAllowed
        {
            get;
            private set;
        }

        /// <summary>
        /// Report is user based.
        /// </summary>        
        public bool IsUserBasedReport
        {
            get;
            set;
        }

        /// <summary>
        /// Set Parameters for this report.
        /// </summary>
        /// <param name="parameters">List of parameters.</param>
        public void SetParameters(Collection<POSReportParameter> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters", "Parameters collection is null.");
            }

            bool validDefaultValue = false;

            // Add appropriate values for each parameter
            foreach(POSReportParameter parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    validDefaultValue = ParseValueForReportParameter(parameter);
                    if (!validDefaultValue)
                    {
                        parameter.Value = null;
                    }
                }
            }

            this.Parameters = parameters;
        }

        /// <summary>
        /// Set charts for this report.
        /// </summary>
        /// <param name="charts">List of charts.</param>
        public void SetCharts(Collection<POSReportChart> charts)
        {
            this.Charts = charts;
        }

        /// <summary>
        /// Set roles allowed for this report.
        /// </summary>
        /// <param name="rolesAllowed">ReadOnlyCollection of roles allowed.</param>
        public void SetRolesAllowed(ReadOnlyCollection<Int64> rolesAllowed)
        {
            this.RolesAllowed = rolesAllowed;
        }

        /// <summary>
        /// Parse report definition xml and create POS report.
        /// </summary>
        /// <param name="reportDefinitionXml">Report definition xml.</param>
        /// <returns>POS report.</returns>     
        public static POSReport ParseReportDefinitionXml(string reportDefinitionXml)
        {
            if (string.IsNullOrEmpty(reportDefinitionXml))
            {
                throw new ArgumentNullException("ReportDefinitionXml", "Report definition xml is empty");
            }

            Collection<POSReportParameter> parameters = new Collection<POSReportParameter>();
            Collection<POSReportChart> charts = new Collection<POSReportChart>();
            POSReport report = new POSReport();
            StringReader stringReader = null;

            try
            {
                stringReader  = new StringReader(reportDefinitionXml);
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    stringReader = null;
                    POSReportParameter parameter = null;
                    POSReportChart chart = null;
                    Collection<string> series = null;
                    string attribute;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case XmlReport:
                                    break;
                                case XmlTitle:
                                    reader.Read();
                                    report.Title = reader.Value;
                                    break;
                                case XmlDatasourceType:
                                    reader.Read();
                                    report.DataSourceType = reader.Value;
                                    break;
                                case XmlQuery:
                                    reader.Read();
                                    report.Query = reader.Value;
                                    break;
                                case XmlDataset:
                                    break;
                                case XmlIsUserBasedReport:
                                    reader.Read();
                                    report.IsUserBasedReport = System.Convert.ToBoolean(reader.Value);
                                    break;
                                case XmlParameters:
                                    break;
                                case XmlParameter:
                                    parameter = new POSReportParameter();
                                    attribute = reader[XmlParameterName];
                                    parameter.Name = attribute.Trim();
                                    attribute = reader[XmlParameterType];
                                    parameter.DataType = (POSReportParameterDataType)Enum.Parse(typeof(POSReportParameterDataType), attribute.Trim(), true);
                                    attribute = reader[XmlParameterLabel];
                                    parameter.Label = attribute.Trim();
                                    attribute = reader[XmlParameterDefaultValue];
                                    if (attribute != null)
                                    {
                                        parameter.Value = attribute.Trim();
                                    }

                                    parameters.Add(parameter);
                                    break;
                                case XmlCharts:
                                    break;
                                case XmlChart:
                                    if (chart != null)
                                    {
                                        chart.SetSeries(series);
                                        charts.Add(chart);
                                    }

                                    chart = new POSReportChart();
                                    series = new Collection<string>();
                                    attribute = reader[XmlCategories];
                                    chart.Categories = attribute.Trim();
                                    break;
                                case XmlSeries:
                                    reader.Read();
                                    series.Add(reader.Value);
                                    break;
                                default:
                                    throw new System.NotImplementedException("This element is not implemented in report definition xml.");
                            }
                        }
                    }
                    if (chart != null)
                    {
                        chart.SetSeries(series);
                        charts.Add(chart);
                    }
                }
                report.SetParameters(parameters);
                report.SetCharts(charts);                
            }
            finally
            {
                if (stringReader != null)
                {
                    stringReader.Dispose();
                }
            }
            return report;
        }

        /// <summary>
        /// Parse values for report input parameters.
        /// </summary>
        /// <param name="parameter">POSReport parameter.</param>
        /// <returns>If value passed was valid or not.</returns>
        public static bool ParseValueForReportParameter(POSReportParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter", "Parameter is null.");
            }

            bool parsed;
            NumberStyles numStyle = NumberStyles.Any;

            switch (parameter.DataType)
            {
                case POSReportParameterDataType.DateTime:
                    DateTime datetimevalue = new DateTime();
                    DateTimeStyles dtStyles = DateTimeStyles.None;
                    parsed = DateTime.TryParse(parameter.Value.ToString(), CultureInfo.InvariantCulture, dtStyles, out datetimevalue);
                    if (parsed)
                    {
                        parameter.Value = datetimevalue;
                    }
                    else
                    {
                        return false;
                    }

                    break;
                case POSReportParameterDataType.Decimal:                    
                    decimal decimalvalue = 0.0m;
                    parsed = decimal.TryParse(parameter.Value.ToString(), numStyle, CultureInfo.InvariantCulture, out decimalvalue);
                    if (parsed)
                    {
                        parameter.Value = decimalvalue;
                    }
                    else
                    {
                        return false;
                    }  
                      
                    break;
                case POSReportParameterDataType.Integer:
                    int intValue = 0;
                    parsed = int.TryParse(parameter.Value.ToString(), numStyle, CultureInfo.InvariantCulture, out intValue);
                    if (parsed)
                    {
                        parameter.Value = intValue;
                    }
                    else
                    {
                        return false;
                    }   
                     
                    break;
                default:
                    return true;                    
            }
            
            return true;
        }

        /// <summary>
        /// Run the SQLs for reports and populates the dataset based on it.
        /// </summary>
        /// <returns>Output dataset for reports.</returns>                
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "False positive: This is a parameterized stored proc call.")]
        public DataTable GetReportDataTable()
        {
            using (DataSet dataSet = new DataSet())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = ApplicationSettings.Database.LocalConnection;
                    command.CommandText = this.Query;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue(ParamChannelId, ApplicationSettings.Terminal.StorePrimaryId);
                    if (this.IsUserBasedReport)
                    {
                        command.Parameters.AddWithValue(ParamUserId, ApplicationSettings.Terminal.TerminalOperator.OperatorId);
                    }

                    foreach (POSReportParameter parameter in this.Parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }
                    
                    using (SqlDataAdapter dataAdaptor = new SqlDataAdapter())
                    {
                        dataAdaptor.SelectCommand = command;
                        dataAdaptor.Fill(dataSet);
                    }
                }

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    return dataSet.Tables[0];
                }
            }

            return null;
        }
    }

}
