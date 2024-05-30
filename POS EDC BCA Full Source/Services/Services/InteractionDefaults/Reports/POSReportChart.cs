/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Class that encapsulate report chart.
    /// </summary>
    [Serializable]
    public class POSReportChart
    {
        /// <summary>
        /// Name of the categories.
        /// </summary>        
        public string Categories
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of series.
        /// </summary>
        public Collection<string> Series
        {
            get;
            private set;
        }

        /// <summary>
        /// Set series for this chart.
        /// </summary>
        /// <param name="charts">List of series.</param>
        public void SetSeries(Collection<string> series)
        {
            this.Series = series;
        }
    }
}
