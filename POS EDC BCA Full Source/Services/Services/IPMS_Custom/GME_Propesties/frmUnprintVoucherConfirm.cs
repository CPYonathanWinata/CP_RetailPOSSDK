/// ADD BY MARIA   -- FORM Unprint Voucher
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Threading.Tasks;

namespace GME_Custom.GME_Propesties
{
    public class FrmUnprintVoucherConfirm : Confirmation
    {
        public IApplication application { get; set; }
        public string memberId { get; set; }
    }
}
