/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Localization.Sweden.Triggers
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Settings.FunctionalityProfiles;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.FiscalItem;    
    using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// Sweden transaction trigger.
    /// </summary>
    [Export(typeof(ITransactionTrigger))]
    public class SwedenTransactionTrigger : ITransactionTrigger, IGlobalization
    {
        #region Globalization
        private readonly ReadOnlyCollection<string> supportedCountryRegions = new ReadOnlyCollection<string>(new string[] { SupportedCountryRegion.SE.ToString() });

        /// <summary>
        /// Defines ISO country region codes this functionality is applicable for.
        /// </summary>
        public ReadOnlyCollection<string> SupportedCountryRegions
        {
            get { return supportedCountryRegions; }
        }
        #endregion

        public void BeginTransaction(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void SaveTransaction(IPosTransaction posTransaction, System.Data.SqlClient.SqlTransaction sqlTransaction)
        {
            // Left empty on purpose.
        }

        public void PreEndTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            const int ReceiptTypeSales = 0;
            const int ReceiptTypeTraining = 2;

            var retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction != null
                && retailTransaction.TransactionType == PosTransaction.TypeOfTransaction.Sales
                && retailTransaction.EntryStatus == PosTransaction.TransactionStatus.Normal)
            {
                try
                {
                    int receiptType = ReceiptTypeSales;
                    if (retailTransaction.Training)
                    {
                        receiptType = ReceiptTypeTraining;
                    }
                    DateTime registrationDate = retailTransaction.BeginDateTime;
                    PosApplication.Instance.Services.FiscalRegister.ClearReceiptId();
                    string receiptId = PosApplication.Instance.Services.FiscalRegister.GetNextReceiptId(retailTransaction);
                    string organizationTaxIdNumber = ApplicationSettings.Terminal.TaxIdNumber;
                    var xDoc = DataConverter.SerializeRetailTransactionToXml(retailTransaction, registrationDate, receiptId, receiptType, organizationTaxIdNumber);

                    string response = PosApplication.Instance.Services.FiscalRegister.RegisterFiscalData(xDoc.ToString());
                    if (!string.IsNullOrEmpty(response))
                    {
                        string unitName = DataConverter.GetUnitNameFromAnswer(response);
                        string controlCode = DataConverter.GetControlCodeFromAnswer(response);
                        int lineId = 1;
                        if (retailTransaction.FiscalLineItems.Count > 0)
                        {
                            lineId = retailTransaction.FiscalLineItems.Max(fl => fl.LineId) + 1;
                        }
                        retailTransaction.FiscalLineItems.Add(new FiscalLineItem()
                        {
                            RegisterId = unitName,
                            ControlCode = controlCode,
                            RegistrationDate = registrationDate,
                            LineId = lineId
                        });
                    }
                }
                catch (Exception ex)
                {
                    LSRetailPosis.ApplicationLog.Log("SwedishFiscalRegister.SaveTransaction", ex.Message, LSRetailPosis.LogTraceLevel.Error);

                    if (preTriggerResult != null) {
                        preTriggerResult.ContinueOperation = false;
                        preTriggerResult.Message = ex.Message;
                    }
                }
            }
        }

        public void PostEndTransaction(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreVoidTransaction(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PostVoidTransaction(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction, IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PostReturnTransaction(IPosTransaction posTransaction)
        {
            // Left empty on purpose.
        }

        public void PreConfirmReturnTransaction(IPreTriggerResult preTriggerResult, IRetailTransaction originalTransaction)
        {
            // Left empty on purpose.
        }
    }
}