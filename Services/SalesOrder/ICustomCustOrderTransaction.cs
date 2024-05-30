using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    // Your new interface that extends IRetailTransactionV1
    public interface ICustomCustOrderTransaction : ICustomerOrderTransaction
    {
        // New members specific to your customization
        string SalesTakerId { get; set; }

        string SalesTakerName { get; set; }

        string SalesTakerNameOnReceipt { get; set; }
    }

}
