/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Dialog
{
    using DataEntity;

    /// <summary>
    /// ISalesOrderSearchCriteriaHolder should be implemented by forms that
    /// can be used to input search criteria that is going to be used to filter the Journal.
    /// </summary>
    interface ISalesOrderSearchCriteriaHolder
    {
       /// <summary>
       /// Gets or sets the search criteria to be used.
       /// </summary>
       SalesOrderSearchCriteria SearchCriteria { get; set; }
    }
}
