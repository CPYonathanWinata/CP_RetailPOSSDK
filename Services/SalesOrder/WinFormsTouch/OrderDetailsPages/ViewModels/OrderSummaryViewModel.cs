/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Pos.Printing;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class OrderSummaryViewModel : PageViewModel
    {        
        public OrderSummaryViewModel(CustomerOrderTransaction customerOrderTransaction)
        {
            this.SetTransaction(customerOrderTransaction);
        }

        public string OrderSummaryText
        {
            get
            {

                FormModulation formMod = new FormModulation(ApplicationSettings.Database.LocalConnection);
                

                FormInfo formInfo = formMod.GetInfoForForm(
                    formId: FormType.SalesOrderSummary, 
                    copyReceipt: false, 
                    receiptProfileId: LSRetailPosis.Settings.HardwareProfiles.Printer.ReceiptProfileId
                    );
                formMod.GetTransformedTransaction(formInfo, this.Transaction);

                string textForPreview = formInfo.Header;
                textForPreview += formInfo.Details;
                textForPreview += formInfo.Footer;
                textForPreview = textForPreview.Replace("|1C", string.Empty);
                textForPreview = textForPreview.Replace("|2C", string.Empty);

                return textForPreview;
            }
        }

        public override void Refresh()
        {
            // Ensure totals are up to date
            //SalesOrder.InternalApplication.BusinessLogic.ItemSystem.RecalcPriceTaxDiscount(this.Transaction, false); //test - disable - Yonathan 22112024 -#duplicate
            this.Transaction.CalcTotals();
            //add trigger Yonathan 17/07/2023
            try
            {
                PosApplication.Instance.Triggers.Invoke<IOperationTrigger>((Action<IOperationTrigger>)(t => t.PostProcessOperation((IPosTransaction)this.Transaction,Contracts.PosisOperations.ConvertCustomerQuote)));
                    
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            
            base.Refresh();
        }
    }
}