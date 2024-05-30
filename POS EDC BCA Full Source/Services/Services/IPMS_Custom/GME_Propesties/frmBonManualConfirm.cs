using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;

namespace GME_Custom.GME_Propesties
{
    public class frmBonManualConfirm : Confirmation
    {
        public const string bonManualNumber = "Bon Manual";
        public const string reason = "Reason";

        public IApplication apps { get; set; }
        public string storeId { get; set; }
    }
}
