using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GME_Custom.GME_Propesties
{
    public class voucher
    {
        public decimal responseCode { get; set; }
        public string responseMessage { get; set; }
    }

    [Serializable]
    public class vouchersList
    {
        public List<voucher> VoucherItems { get; set; }
    }
}
