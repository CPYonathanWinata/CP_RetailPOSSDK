using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;


namespace GME_Custom.GME_Propesties
{
    public class frmCustTypeConfirm : Confirmation
    {
        string guest;
        string familyMember;

        public IPosTransaction posTransaction { get; set; }
        public IApplication application { get; set; }
    }
}
