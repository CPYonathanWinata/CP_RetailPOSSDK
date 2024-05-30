using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    [Export(typeof(IPosCustomControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CashierName : UserControl, IPosCustomControl
    {        

        public CashierName()
        {
            InitializeComponent();
        }

        public void LoadLayout(string layoutId)
        {
            //throw new NotImplementedException();
            CashierMngId.Text = GME_Var.cashierMngId;
            CashierMngName.Text = GME_Var.cashierMngName;
        }

        public void TransactionChanged(Contracts.DataEntity.IPosTransaction transaction)
        {
            //throw new NotImplementedException();                
        }        
    }
}
