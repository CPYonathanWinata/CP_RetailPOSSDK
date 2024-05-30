using Microsoft.Dynamics.Retail.Pos.Contracts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class frmCheckLoyaltyDetail : Form
    {
        IApplication application;

        public frmCheckLoyaltyDetail(IApplication _application)
        {
            InitializeComponent();
            application = _application;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool status = false;
            string message = string.Empty;

            try
            {
                ReadOnlyCollection<object> containerArray;
         
                containerArray = application.TransactionServices.InvokeExtension("getLoyaltyDetail",tbCard.Text);

                status = (bool)containerArray[1];
                message = containerArray[2].ToString();
                tbName.Text = containerArray[4].ToString();
                tbPoin.Text = containerArray[5].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            if (!status)
            {
                MessageBox.Show(message);
            }
        }
    }
}
