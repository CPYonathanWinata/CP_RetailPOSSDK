using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using GME_Custom;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using GME_Custom.GME_Propesties;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.GME
{
    public partial class frmSplash : Form
    {
        static frmSplash ms_frmSplash = null;

        public frmSplash()
        {
            InitializeComponent();
        }        

        static public void ShowForm()
        {
            ms_frmSplash = new frmSplash();
            ms_frmSplash.ShowDialog();
            ms_frmSplash.Hide();
        }
        // A static method to close the SplashScreen
        static public void CloseForm()
        {
            ms_frmSplash = new frmSplash();
            ms_frmSplash.Close();
        }


        private void frmSplash_Load(object sender, EventArgs e)
        {
            
        }

      
    }
}
