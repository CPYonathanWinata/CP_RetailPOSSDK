using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;

namespace GME_Custom.GME_Propesties
{
    public class frmCustInfoConfirm : Confirmation
    {
        public const string accountNum = "Account Number";
        public const string custGroup = "Customer Group";
        public const string currency = "Currency";
        public const string custName = "Customer Name";

        public static DataTable getCustInfoTempDT()
        {
            DataTable resultDT = new DataTable();

            resultDT.Columns.Add(accountNum);
            resultDT.Columns.Add(custGroup);
            resultDT.Columns.Add(currency);
            resultDT.Columns.Add(custName);

            return resultDT;
        }
    }
}
