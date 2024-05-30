using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.ComponentModel.Composition;
using GME_Custom;
using System.Threading;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.ComponentModel;

namespace GME_Custom.GME_Propesties
{
    public class Connection : Confirmation
    {
        public IApplication apps { get; set; }
        public static IApplication applicationLoc;

        
    }
}
