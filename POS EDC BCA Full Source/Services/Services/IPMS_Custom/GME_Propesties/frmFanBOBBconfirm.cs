/// // ADD  BY MARIA- WCS BBOB FORM
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    public class frmFanBOBBconfirm : Confirmation
    {
        public IApplication application;
        public string item { get; set; }
        public string quantity { get; set; }
    }
}
