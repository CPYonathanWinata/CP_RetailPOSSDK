/// ADD BY MARIA   -- FORM DONASI
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Threading.Tasks;

namespace GME_Custom.GME_Propesties
{
    public class FrmDonasiConfirm : Confirmation
    {
        public string AmtDonasi { get; set; }


        public IApplication application { get; set; }
        public IPosTransaction posTransaction { get; set; }
    }
}
