using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    public class frmPublicConfirm : Confirmation
    {
        public string custName { get; set; }
        public string custEmail { get; set; }
        public string custPhoneNum { get; set; }
        public string custGroup { get; set; }
        public IPosTransaction posTransaction { get; set; }
        public IApplication application { get; set; }
    }
}
