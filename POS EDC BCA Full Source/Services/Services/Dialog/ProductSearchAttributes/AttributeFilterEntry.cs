/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.ProductSearchAttributes
{
    internal class AttributeFilterEntry : IAttributeFilterEntry
    {
        public AttributeFilterEntry(IProductAttributeInfo productAttributeInfo, IProductSearchAttributeFilter productSearchAttributeFilter)
        {
            if (productAttributeInfo == null)
            {
                throw new ArgumentNullException("productAttributeInfo");
            }
            if (productSearchAttributeFilter == null)
            {
                throw new ArgumentNullException("productSearchAttributeFilter ");
            }

            this.ProductAttributeInfo = productAttributeInfo;
            this.ProductSearchAttributeFilter = productSearchAttributeFilter;
        }
        public IProductAttributeInfo ProductAttributeInfo { get; set; }

        public IProductSearchAttributeFilter ProductSearchAttributeFilter { get; set; }
    }
}