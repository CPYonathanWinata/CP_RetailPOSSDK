//Added by Adhi 
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    //Added by Adhi 
    public class frmFamilyPurchaseConfirm : Confirmation
    {
        public string CustId { get; set; }
        public string CustFirstName { get; set; }

        public IApplication application { get; set; }
        //Added by Adhi (fungsi form)
        public IPosTransaction posTransaction { get; set; }
    }
}
