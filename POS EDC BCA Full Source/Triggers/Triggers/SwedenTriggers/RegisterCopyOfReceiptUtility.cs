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
    using System.Linq;
    using LSRetailPosis;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Transaction;
    using LSRetailPosis.Transaction.Line.FiscalItem;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;
    using LSRetailPosis.DataAccess;
    using LSRetailPosis.DataAccess.DataUtil;
    
    /// <summary>
    /// Helper class for regestering copy of receipt in a control unit and POS.
    /// </summary>
    public static class RegisterCopyOfReceiptUtility
    {
        private const int operationTypeCopyOfReceipt = 1;

        private enum TransactionLocationType
        {
            Unknown,
            LocalDBSerialized,
            LocalDBTable,
            Remote
        }

        /// <summary>
        /// Register Copy of receipt in control unit and POS.
        /// </summary>
        /// <param name="retailTransaction">Retail Transaction</param>
        [CLSCompliant(false)]
        public static void RegisterCopyOfReceipt(RetailTransaction retailTransaction)
        {
            if (retailTransaction != null)
            {
                DateTime registrationDate = retailTransaction.BeginDateTime;
                var transactionLocationType = GetOrderLocationType(retailTransaction);
                UpdateFiscalLines(transactionLocationType, retailTransaction);
                ValidateRegisterReceiptCopyAllowed(transactionLocationType, retailTransaction);
                var xDoc = DataConverter.SerializeRetailTransactionToXml(retailTransaction, registrationDate, retailTransaction.ReceiptId,
                    operationTypeCopyOfReceipt, ApplicationSettings.Terminal.TaxIdNumber);
                string response = PosApplication.Instance.Services.FiscalRegister.RegisterFiscalData(xDoc.ToString());
                if (!string.IsNullOrEmpty(response))
                {
                    AddFiscalDataToRetailTransaction(retailTransaction, DataConverter.GetUnitNameFromAnswer(response),
                        DataConverter.GetControlCodeFromAnswer(response), registrationDate);
                    SaveFiscalDataForReceiptCopy(transactionLocationType, retailTransaction);
                }
            }
        }

        private static void ValidateRegisterReceiptCopyAllowed(TransactionLocationType orderLocationType, IRetailTransaction retailTransaction)
        {           
            if (retailTransaction.FiscalLineItems.Count(l => l.ReceiptCopy == true) != 0)
            {
                // You cannot print a copy of the receipt. A copy of the receipt is already registered.
                int errorCode = 107200;
                var ex = new PosisException(ApplicationLocalizer.Language.Translate(errorCode));
                ex.ErrorNumber = errorCode;
                throw ex;
            }

            if (orderLocationType != TransactionLocationType.LocalDBSerialized && orderLocationType != TransactionLocationType.LocalDBTable)
            {
                // A copy of the receipt can only be registered for transactions available in the store database.
                int errorCode = 107201;
                var ex = new PosisException(ApplicationLocalizer.Language.Translate(errorCode));
                ex.ErrorNumber = errorCode;
                throw ex;
            }
        }

        private static void SaveFiscalDataForReceiptCopy(TransactionLocationType orderLocationType, IRetailTransaction retailTransaction)
        {
           var fiscalLineItem = retailTransaction.FiscalLineItems.Last(l => l.ReceiptCopy == true);
           switch (orderLocationType)
           {
               case TransactionLocationType.LocalDBTable:
               case TransactionLocationType.LocalDBSerialized:
                   SaveFiscalDataToDB(retailTransaction, fiscalLineItem);
                   break;             
               default:
                   // A copy of the receipt can only be registered for transactions available in the store database.
                   int errorCode = 107201;
                   var ex = new PosisException(ApplicationLocalizer.Language.Translate(errorCode));
                   ex.ErrorNumber = errorCode;
                   throw ex;
           }
        }
    
        private static void AddFiscalDataToRetailTransaction(IRetailTransaction retailTransaction, string unitName, string controlCode, DateTime registrationDate)
        {
 	        int lineId = 1;            
            lineId = retailTransaction.FiscalLineItems.Max(fl => fl.LineId) + 1;

            var fiscalLineItem = new FiscalLineItem() 
            { 
                RegisterId = unitName, 
                ControlCode = controlCode, 
                RegistrationDate = registrationDate,
                ReceiptCopy = true,
                LineId = lineId                        
            };

            retailTransaction.FiscalLineItems.Add(fiscalLineItem);
        }

        private static void SaveFiscalDataToDB(IRetailTransaction retailTransaction, IFiscalLineItem fiscalLineItem)
        {           
            var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
            var transactionFiscalData = new TransactionFiscalData(dbUtil, ApplicationSettings.Database.DATAAREAID);
            var transData = new TransactionData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, PosApplication.Instance);
            dbUtil.BeginTransaction();
            transactionFiscalData.SaveSingleFiscalLine(retailTransaction, fiscalLineItem);            
            transData.SaveSerializedTransaction(retailTransaction as RetailTransaction);
            dbUtil.EndTransaction();            
        }

        private static TransactionLocationType GetOrderLocationType(IRetailTransaction retailTransaction)
        {
            return OrderExistsInLocalDBAsSerialized(retailTransaction) ? TransactionLocationType.LocalDBSerialized :
                OrderExistsInLocalDBInTable(retailTransaction) ? TransactionLocationType.LocalDBTable : TransactionLocationType.Remote;
        }

        private static bool OrderExistsInLocalDBAsSerialized(IRetailTransaction retailTransaction)
        {
            bool retVal = true;
            
            // Search transactions in serialized retail transaction table (RETAILTRANSACTIONTABLEEX5)
            TransactionData transData = new TransactionData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID, PosApplication.Instance);
            var localTransaction = transData.LoadSerializedTransaction(retailTransaction.TransactionId, retailTransaction.StoreId, retailTransaction.TerminalId);

            if (localTransaction == null)
            {
                retVal = false;
            }

            return retVal;
        }
         private static bool OrderExistsInLocalDBInTable(IRetailTransaction retailTransaction)
         {
            bool retVal = true;
            
             // Ttrying to find transaction in RetailTransactionTable. 
            CRTTransactionData crtData = new CRTTransactionData(PosApplication.Instance,
                ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

            var localTransaction = crtData.LoadRetailTransactionTable(retailTransaction.TransactionId, ApplicationSettings.Terminal.StorePrimaryId, retailTransaction.StoreId, retailTransaction.TerminalId, ApplicationSettings.Terminal.CompanyCurrency, ApplicationSettings.Terminal.CultureName);

            if (localTransaction == null)
            {
                retVal = false;
            }
            
            return retVal;
        }

        private static void UpdateFiscalLines(TransactionLocationType locationType, IRetailTransaction retailTransaction)
        {
            if (locationType == TransactionLocationType.LocalDBTable)
            {
                var dbUtil = new DBUtil(ApplicationSettings.Database.LocalConnection);
                var transactionFiscalData = new TransactionFiscalData(dbUtil, ApplicationSettings.Database.DATAAREAID);
                transactionFiscalData.LoadFiscalLines(retailTransaction);
            }
        }
    }
}
