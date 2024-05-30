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
    public partial class CustomerInfo : UserControl, IPosCustomControl
    {
        public CustomerInfo()
        {
            InitializeComponent();
        }

        public void LoadLayout(string layoutId)
        {
            //throw new NotImplementedException();
            if (GME_Var.customerData != null)
            {
                IDLbl.Text = GME_Var.customerData.CustomerId;
                nameLbl.Text = GME_Var.customerData.Name;
                CardNumLbl.Text = GME_Var.identifierCode;
                cardTierLbl.Text = GME_Var.identifierCardType;
                cardPointLbl.Text = "XXXXX";
            }
            else
            {
                if (GME_Var.isShowInfoAfterEnroll)
                {
                    IDLbl.Text = string.Empty;
                    nameLbl.Text = GME_Var._custInfoName;
                    CardNumLbl.Text = GME_Var.identifierCode;
                    cardTierLbl.Text = GME_Var.identifierCardType == string.Empty ? "PRE-LYBC" : GME_Var.identifierCardType;
                    cardPointLbl.Text = "XXXXX";
                    GME_Var.isShowInfoAfterEnroll = false;
                }
                else
                {
                    IDLbl.Text = string.Empty;
                    nameLbl.Text = string.Empty;
                    CardNumLbl.Text = string.Empty;
                    cardTierLbl.Text = string.Empty;
                    cardPointLbl.Text = string.Empty;
                }

            }
        }

        public void TransactionChanged(Contracts.DataEntity.IPosTransaction transaction)
        {
            //throw new NotImplementedException(); 
            RetailTransaction retailTransaction = transaction as RetailTransaction;

            if (retailTransaction != null)
            {
                if (retailTransaction.LastRunOperation == PosisOperations.CustomerAdd)
                {
                    if (GME_Var.customerData != null)
                    {
                        IDLbl.Text = GME_Var.customerData.CustomerId;
                        nameLbl.Text = GME_Var.customerData.Name;
                        CardNumLbl.Text = GME_Var.identifierCode;
                        cardTierLbl.Text = GME_Var.identifierCardType;
                        cardPointLbl.Text = "XXXXX";
                    }
                    else
                    {
                        if (GME_Var.isShowInfoAfterEnroll)
                        {
                            IDLbl.Text = string.Empty;
                            nameLbl.Text = GME_Var._custInfoName;
                            CardNumLbl.Text = GME_Var.identifierCode;
                            cardTierLbl.Text = GME_Var.identifierCardType == string.Empty ? "PRE-LYBC" : GME_Var.identifierCardType;
                            cardPointLbl.Text = "XXXXX";
                            GME_Var.isShowInfoAfterEnroll = false;
                        }
                        else
                        {
                            IDLbl.Text = string.Empty;
                            nameLbl.Text = string.Empty;
                            CardNumLbl.Text = string.Empty;
                            cardTierLbl.Text = string.Empty;
                            cardPointLbl.Text = string.Empty;
                        }
                    }
                }
                else if (retailTransaction.LastRunOperation == PosisOperations.CustomerSearch)
                {
                    if (GME_Var.customerData != null)
                    {
                        IDLbl.Text = GME_Var.customerData.CustomerId;
                        nameLbl.Text = GME_Var.customerData.Name;
                        CardNumLbl.Text = GME_Var.identifierCode;
                        cardTierLbl.Text = GME_Var.identifierCardType;
                        cardPointLbl.Text = "XXXXX";
                    }
                    else
                    {
                        if (GME_Var.isShowInfoAfterEnroll)
                        {
                            IDLbl.Text = string.Empty;
                            nameLbl.Text = GME_Var._custInfoName;
                            CardNumLbl.Text = GME_Var.identifierCode;
                            cardTierLbl.Text = GME_Var.identifierCardType == string.Empty ? "PRE-LYBC" : GME_Var.identifierCardType;
                            cardPointLbl.Text = "XXXXX";
                            GME_Var.isShowInfoAfterEnroll = false;
                        }
                        else
                        {
                            IDLbl.Text = string.Empty;
                            nameLbl.Text = string.Empty;
                            CardNumLbl.Text = string.Empty;
                            cardTierLbl.Text = string.Empty;
                            cardPointLbl.Text = string.Empty;
                        }
                    }
                }
                else if (retailTransaction.LastRunOperation == PosisOperations.AffiliationAdd)
                {
                    if (GME_Var.customerData != null)
                    {
                        IDLbl.Text = GME_Var.customerData.CustomerId;
                        nameLbl.Text = GME_Var.customerData.Name;
                        CardNumLbl.Text = GME_Var.identifierCode;
                        cardTierLbl.Text = GME_Var.identifierCardType;
                        cardPointLbl.Text = "XXXXX";
                    }
                    else
                    {
                        IDLbl.Text = string.Empty;
                        nameLbl.Text = string.Empty;
                        CardNumLbl.Text = string.Empty;
                        cardTierLbl.Text = string.Empty;
                        cardPointLbl.Text = string.Empty;
                    }
                }
            }
            else
            {
                if (GME_Var.customerData == null)
                {
                    IDLbl.Text = string.Empty;
                    nameLbl.Text = string.Empty;
                    CardNumLbl.Text = string.Empty;
                    cardTierLbl.Text = string.Empty;
                    cardPointLbl.Text = string.Empty;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDLbl.Text = string.Empty;
            nameLbl.Text = string.Empty;
            CardNumLbl.Text = string.Empty;
            cardTierLbl.Text = string.Empty;
            cardPointLbl.Text = string.Empty;
            
            GME_Method.setAllVariableToDefault();

            Connection.applicationLoc.RunOperation(PosisOperations.CustomerAdd, string.Empty);
        }
    }
}
