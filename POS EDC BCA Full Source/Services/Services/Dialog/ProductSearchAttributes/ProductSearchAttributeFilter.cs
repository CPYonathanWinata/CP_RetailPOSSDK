/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using System.Collections.Generic;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.ProductSearchAttributes
{
    internal class ProductSearchAttributeFilter : IProductSearchAttributeFilter
    {
        public ProductSearchAttributeFilter()
        {
            this.FilterValuesList = new List<IProductSearchAttributeFilterValue>();
        }

        public IList<IProductSearchAttributeFilterValue> FilterValuesList { get; set; }

        public IProductSearchAttributeFilterValue RangeFrom { get; set; }

        public IProductSearchAttributeFilterValue RangeTo { get; set; }

        public string TextFilter { get; set; }

        public dynamic PartnerData { get; set; }
    }
}
