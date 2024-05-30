/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Class that encapsulate report parameter.
    /// </summary>
    [Serializable]
    public class POSReportParameter
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>        
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Label for the parameter.
        /// </summary>
        public string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Data type of the parameter.
        /// </summary>
        public POSReportParameterDataType DataType
        {
            get;
            set;
        }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        public object Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Enumerator to represent type of report parameter datatypes.
    /// </summary>
    public enum POSReportParameterDataType
    {
        String = 1,
        DateTime = 2,
        Decimal = 3,
        Integer = 4
    }
}