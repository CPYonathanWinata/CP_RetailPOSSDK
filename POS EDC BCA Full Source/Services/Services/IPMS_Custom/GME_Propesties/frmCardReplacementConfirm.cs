/// ADD BY MARIA   -- FORM Card Replacement
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System.Threading.Tasks;


namespace GME_Custom.GME_Propesties
{
    public class FrmCardReplacementConfirm : Confirmation
    {
        public IApplication application { get; set; }
        public IPosTransaction posTransaction { get; set; }
        public string oldCardNumber { get; set; }
        public string oldcCardType { get; set; }
        public string newCardNumber { get; set; }
        public string newCardType { get; set; }
    }
}
