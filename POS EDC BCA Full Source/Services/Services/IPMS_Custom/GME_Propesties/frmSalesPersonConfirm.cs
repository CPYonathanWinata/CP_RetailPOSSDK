using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    public class frmSalesPersonConfirm : Confirmation
    {
        public string salesPersonId { get; set; }
        public string salesPersonName { get; set; }
        public IApplication application { get; set; }
        //public IPosTransaction posTransaction { get; set; }
    }
}
