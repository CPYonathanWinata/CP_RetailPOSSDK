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
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CP_IZone.PLN
{
    public partial class CPPLNPascabayar : frmTouchBase
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
                //m_labelSubtitle.Text = (currentIndex + 1) + " : " + tabControl.TabPages[currentIndex].Text;
                for (int i = count - 1; i >= 0; i--)
                {
                    Control control = tabControl.TabPages[currentIndex].Controls[i];
                    tabControl.TabPages[currentIndex].Controls.Remove(control);
                    parentPanel.Controls.Add(control);
                }

                currentPanelIndex = currentIndex;
            }
        }

        private void LoadData()
        {
            btnBack.Enabled = false;
            btnBack.BackColor = Color.DarkGray;
            btnFinish.Enabled = false;
            btnFinish.BackColor = Color.DarkGray;
        }
        public CPPLNPascabayar(IBlankOperationInfo _operationInfo, IPosTransaction _posTransaction, IApplication _application)
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
            //RetailTransaction transaction = posTransaction as RetailTransaction;


            decimal priceToOverride = 673000;
            string itemID = "80000011";

            if (itemID != "")
            {
                application.RunOperation(PosisOperations.ItemSale, itemID);
                //Application.RunOperation(PosisOperations.ItemSale, selectedItemID);
            }

            RetailTransaction transaction = BlankOperations.globalposTransaction as RetailTransaction;
            //BlankOperations.globalposTransaction;
            //BlankOperations.

            //pascabayar to override the price
            if (transaction != null)
            {
                for (int i = 0; i < transaction.SaleItems.Count; i++)
                {
                    //string thisItemId = "";

                    LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = transaction.CurrentSaleLineItem; // transaction.GetItem(transaction.SaleItems.ElementAt(i).LineId);
                    //int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;

                    //if (currentLine.LineId == operationInfo.ItemLineId && currentLine.Voided == false)
                    if(currentLine.Voided == false)
                    {
                        saleLineItem = RetailTransaction.PriceOverride(currentLine, priceToOverride);//transaction.SaleItems.Last.Value, priceToOverride);
                        application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction);
                        transaction.CalcTotals();
                        string str = ((IServicesV1)PosApplication.Instance.Services).Rounding.Round(((BaseSaleItem)saleLineItem).OriginalPrice, true);
                        POSFormsManager.ShowPOSStatusPanelText(ApplicationLocalizer.Language.Translate(3352, new object[3]
                                    {
                                        (object) ((LineItem) saleLineItem).Description,
                                        (object) ((BaseSaleItem) saleLineItem).BarcodeId,
                                        (object) str
                                    }));

                    }
                }
            }
            this.Close();
        }

        private void CPPLNPascabayar_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {

            if (e.CloseReason != CloseReason.FormOwnerClosing)
                this.Owner.Close();

        }

        private void btn_enabledChanged(object sender, EventArgs e)
        {

        }
    }
}
 