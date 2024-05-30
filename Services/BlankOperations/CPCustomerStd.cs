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
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ICustomer;

using LSRetailPosis.Transaction;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    [Export(typeof(IPosCustomControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CPCustomerStd : UserControl, IPosCustomControl
    {
        public CPCustomerStd()
        {
            InitializeComponent();
        }

        public void LoadLayout(string layoutId)
        {
            //throw new NotImplementedException();
            lblCustomerStd.Text = "";
        }

        public void TransactionChanged(Contracts.DataEntity.IPosTransaction transaction)
        {
            RetailTransaction retailTransaction = transaction as RetailTransaction;

            if (retailTransaction != null)
            {
                DE customer = retailTransaction.Customer;

                if (!String.IsNullOrEmpty(customer.CustomerId))
                {
                    // Customer Number, Name
                    lblCustomerStd.Text = "CustAccount : ";
                    lblCustomerStd.Text += customer.CustomerId + Environment.NewLine;
                    lblCustomerStd.Text += "CustomerName : ";
                    lblCustomerStd.Text += customer.Name + Environment.NewLine;

                    // Address
                    lblCustomerStd.Text += "Address : " + Environment.NewLine;
                    lblCustomerStd.Text += customer.Address;
                }
                else
                {
                    lblCustomerStd.Text = "";
                }
            }
            else
            {
                lblCustomerStd.Text = "";
            }
            //throw new NotImplementedException();
        }
    }
}
