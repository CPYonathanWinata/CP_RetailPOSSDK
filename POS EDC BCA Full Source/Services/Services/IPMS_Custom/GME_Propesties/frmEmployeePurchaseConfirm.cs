//Added by Adhi 
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    //Added by Adhi 
    public class frmEmployeePurchaseConfirm : Confirmation //Copas dari frmPublicConfirm nama file disesuaikan(8)
    {
        public string CustId { get; set; }//variabel yang dibuat (id nik) dapat bisa get dan set
        public string CustFirstName { get; set; }//variabel yang dibuat (first name) dapat bisa get dan set

        public IApplication application { get; set; }//koneksi,storeID,terminalID,shiftID
        public IPosTransaction posTransaction { get; set; }
    }
}
