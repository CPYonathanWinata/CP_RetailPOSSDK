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
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using LSRetailPosis.POSControls;
using LSRetailPosis;
using LSRetailPosis.Transaction.Line;
using System.Collections.ObjectModel;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.IZone.PLN
{
    public partial class CPPLNPrabayar : frmTouchBase
    {
        int currentPanelIndex;
        IPosTransaction posTransaction;
        IApplication application;
        IBlankOperationInfo operationInfo;

        private void SetCurrentTab(int currentIndex)
        {
            if (currentPanelIndex > -1) //remove all old controls back to tabcontrol
            {
                int count = parentPanel.Controls.Count;

                for (int i = count - 1; i >= 0; i--)
                {
                    Control control = parentPanel.Controls[i];
                    parentPanel.Controls.Remove(control);
                    tabControl.TabPages[currentPanelIndex].Controls.Add(control);
                }
            }

            {
                int count = tabControl.TabPages[currentIndex].Controls.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    Control control = tabControl.TabPages[currentIndex].Controls[i];
                    tabControl.TabPages[currentIndex].Controls.Remove(control);
                    parentPanel.Controls.Add(control);
                }

                currentPanelIndex = currentIndex;

                // Call the appropriate load method based on the current index
                switch (currentIndex)
                {
                    case 0:
                        LoadTabOneData();
                        break;
                    case 1:
                        LoadTabTwoData();
                        break;
                    // Add more cases for additional tabs
                    default:
                        break;
                }
            }
        }

        private void LoadData()
        {
            btnBack.Enabled = false;
            btnBack.BackColor = Color.DarkGray;
            btnFinish.Enabled = false;
            btnFinish.BackColor = Color.DarkGray;
            //i want this on main and production too
            //i want this on main and production too ver 2
        }

        private void LoadTabOneData()
        {
            // Add code to load data for tab one
        }

        private void LoadTabTwoData()
        {
            ReadOnlyCollection<object> containerArray = application.TransactionServices.InvokeExtension("getItemList", "APLN");
        }

        public CPPLNPrabayar(IBlankOperationInfo _operationInfo, IPosTransaction _posTransaction, IApplication _application)
        {
            posTransaction = _posTransaction;
            application = _application;
            operationInfo = _operationInfo;

            InitializeComponent();
            parentPanel.Controls.Remove(tabControl);
            currentPanelIndex = -1;
            SetCurrentTab(0);

            LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string reasonMessage = string.Empty;
            if (NextButtonClicked(out reasonMessage))
            {
                int currentIndex = currentPanelIndex;
                currentIndex++;
                if (!(currentIndex >= tabControl.TabCount))
                {
                    SetCurrentTab(currentIndex);
                    btnBack.Enabled = true;
                    btnBack.BackColor = Color.FromArgb(171, 194, 215);

                    if (currentIndex == tabControl.TabCount - 1)
                    {
                        btnNext.Enabled = false;
                        btnNext.BackColor = Color.DarkGray;

                        btnFinish.Enabled = true;
                        btnFinish.BackColor = Color.FromArgb(171, 194, 215);
                    }
                }
            }
            else
            {
                if (reasonMessage.Length > 0)
                    MessageBox.Show(reasonMessage, "Error", MessageBoxButtons.OK);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (BackButtonClicked())
            {
                int currentIndex = currentPanelIndex;
                currentIndex--;
                if (currentIndex >= 0)
                {
                    SetCurrentTab(currentIndex);

                    btnNext.Enabled = true;
                    btnNext.BackColor = Color.FromArgb(171, 194, 215);

                    btnFinish.Enabled = false;
                    btnFinish.BackColor = Color.DarkGray;
                    if (currentIndex == 0)
                    {
                        btnBack.Enabled = false;
                        btnBack.BackColor = Color.DarkGray;
                    }
                }
            }
        }

        bool NextButtonClicked(out string reasonMessage)
        {
            reasonMessage = "OK";
            return true;
        }

        bool BackButtonClicked()
        {
            return true;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            CPIZone cpIzone = new CPIZone(operationInfo, posTransaction, application);
            cpIzone.ShowDialog();
            this.Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            SaleLineItem saleLineItem;

            decimal priceToOverride = 213000;
            string itemID = "80000012";

            if (itemID != "")
            {
                application.RunOperation(PosisOperations.ItemSale, itemID);
            }

            RetailTransaction transaction = BlankOperations.globalposTransaction as RetailTransaction;
            this.Close();
        }

        private void CPPLNPrabayar_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
                this.Owner.Close();
        }

        private void btn_enabledChanged(object sender, EventArgs e)
        {

        }
    }
}
