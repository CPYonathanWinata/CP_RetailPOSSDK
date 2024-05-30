using LSRetailPosis.POSControls.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System.Xml;
using Microsoft.Dynamics.Retail.Pos.BlankOperations.IZone.PLN;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_IZone.PLN;
//using Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_IZone.PLN;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPIZone : frmTouchBase
    {
        IPosTransaction posTransaction;
        IApplication application;
        IBlankOperationInfo operationInfo;
        public CPIZone(IBlankOperationInfo _operationInfo, IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
            application = _application;
            operationInfo = _operationInfo;
        }

        
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPLNPra_Click(object sender, EventArgs e)
        {
            CPPLNPrabayar plnPrabayar = new CPPLNPrabayar(operationInfo, posTransaction, application);
            plnPrabayar.Owner = this;
            plnPrabayar.Show();
            //this.Hide();
 
            //plnPrabayar.ShowDialog();
        }

        private void btnPLNPasca_Click(object sender, EventArgs e)
        {
            CPPLNPascabayar plnPascabayar = new CPPLNPascabayar(operationInfo, posTransaction, application);
            plnPascabayar.Owner = this;
            plnPascabayar.Show();
        }

        
    }
}
