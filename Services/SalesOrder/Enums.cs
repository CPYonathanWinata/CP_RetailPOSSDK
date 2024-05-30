/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    /// <summary>
    /// AX document status
    /// </summary>
    internal enum DocumentStatus
    {
        None = 0,
        Quotation,
        PurchaseOrder,
        Confirmation,
        PickingList,
        PackingSlip,
        ReceiptsList,
        Invoice,
        ApproveJournal,
        ProjectInvoice,
        ProjectPackingSlip,
        CRMQuotation,
        Lost,
        Canceled,
        FreeTextInvoice,
        RFQ,
        RFQAccept,
        RFQReject,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Purch")]
        PurchReq,
        RFQResend,
        Facture = 102
    }

    /// <summary>
    /// AX Sales Quotation Status
    /// </summary>
    internal enum SalesQuotationStatus
    {
        Created = 0,
        Sent,
        Confirmed,
        Lost,
        Canceled,
        Reset,
        Modified,
        Submitted,
        Approved
    }

    /// <summary>
    /// AX Sales Order Status
    /// </summary>
    internal enum SalesOrderStatus
    {
        None = 0,
        Backorder,
        Delivered,
        Invoiced,
        Canceled
    }
}
