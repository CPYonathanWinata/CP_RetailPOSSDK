using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    public class frmTestConmfirm : Confirmation
    {
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string CustPhone { get; set; }

        public IApplication application;        
    }
}
