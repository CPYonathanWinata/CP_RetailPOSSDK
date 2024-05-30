using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    public class FrmWelcomeVoucherConfirm : Confirmation
    {
        public string voucherId { get; set; }

        public IApplication application { get; set; }
    }
}
