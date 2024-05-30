/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

//Microsoft Dynamics AX for Retail POS Plug-ins 
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using LSRetailPosis.Transaction.Line.SaleItem;
using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using LSRetailPosis.Transaction.Line.Discount;
using System.Linq;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;

namespace Microsoft.Dynamics.Retail.Pos.ItemTriggers
{
    /// <summary>
    /// <example><code>
    /// // In order to get a copy of the last item added to the transaction, use the following code:
    /// LinkedListNode<SaleLineItem> saleItem = ((RetailTransaction)posTransaction).SaleItems.Last;
    /// // To remove the last line use:
    /// ((RetailTransaction)posTransaction).SaleItems.RemoveLast();
    /// </code></example>
    /// </summary>
    [Export(typeof(IItemTrigger))]
    public class ItemTriggers : IItemTrigger
    {

        #region Constructor - Destructor

        public ItemTriggers()
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for ItemTriggers are reserved at 50350 - 50399
        }        

        #endregion

        #region IItemTriggersV1 Members

        public void PreSale(IPreTriggerResult  preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreSale", "Prior to the sale of an item...", LSRetailPosis.LogTraceLevel.Trace);

            queryData data = new queryData();            

            if (GME_Var.isVoidTransaction)
            {
                if (GME_Var.isChangeSalesPerson)
                {
                    if (GME_Var.salesPerson != data.getSalesPerson(posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                    {
                        GME_Var.salesPerson = data.getSalesPerson(posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                    }

                    if (saleLineItem.LineId == 0)
                    {
                        saleLineItem.Comment = GME_Var.salesPerson + System.Environment.NewLine;
                    }                    
                }
                else
                {
                    if (saleLineItem.LineId == 0)
                    {
                        saleLineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                    }
                }

               GME_Var.isVoidTransaction = false;
            }
            else
            {
                if (GME_Var.isChangeSalesPerson)
                {
                    if (GME_Var.salesPerson != data.getSalesPerson(posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                    {
                        GME_Var.salesPerson = data.getSalesPerson(posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                    }

                    if (saleLineItem.LineId == 0)
                    {
                        saleLineItem.Comment = GME_Var.salesPerson + System.Environment.NewLine;
                    }

                    if (saleLineItem.ItemId == GME_Var.itemDonasi)
                    {
                        saleLineItem.PriceOverridden = true;
                        saleLineItem.Price = GME_Var.amountDonasi;
                        saleLineItem.NetAmount = GME_Var.amountDonasi;
                    }
                }
                else
                {
                    if (saleLineItem.LineId == 0)
                    {
                        saleLineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;                        
                    }

                    if (saleLineItem.ItemId == GME_Var.itemDonasi)
                    {
                        saleLineItem.PriceOverridden = true;
                        saleLineItem.Price = GME_Var.amountDonasi;
                        saleLineItem.NetAmount = GME_Var.amountDonasi;
                    }
                }
            }
        }

        public void PostSale(IPosTransaction posTransaction)
        {
             LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostSale", "After the sale of an item...", LSRetailPosis.LogTraceLevel.Trace);
            
            RetailTransaction retailTrans = posTransaction as RetailTransaction;
            GME_Var.welcomePosTransaction = posTransaction;

            if (retailTrans.NumberOfLines() == 1)
            {
                if (GME_Var.isSkipCustomerType == false)
                {
                    if (retailTrans.Customer.CustomerId == null)
                    {
                        GME_Var.isRefreshCustomer = true;
                        Connection.applicationLoc.RunOperation(PosisOperations.CustomerAdd, string.Empty);                        
                    }
                }
            }

            queryData data = new GME_Custom.GME_Data.queryData();
            LinkedList<SaleLineItem> lsSaleLineItem = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
            foreach (SaleLineItem lineItem in lsSaleLineItem)
            {
                int itemtype = data.getItemType(lineItem.ItemId, Connection.applicationLoc.Settings.Database.DataAreaID, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                //ADD BY RIZKI for testing active voucher
                if (lineItem.LineId == lsSaleLineItem.Count)
                {
                    if (itemtype == 101)
                    {
                        GME_FormCaller.GME_FormItemVoucher(Connection.applicationLoc);
                        lineItem.Comment += GME_Var.payVoucherId;
                    }
                }
            }
            GME_Var.totalAmountEDC = retailTrans.TransSalePmtDiff;

            if (GME_Var.isChangeSalesPerson)
            {
                if (GME_Var.salesPerson != data.getSalesPerson(posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString))
                {
                    GME_Var.salesPerson = data.getSalesPerson(posTransaction.StoreId, posTransaction.TerminalId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                }

                LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                foreach (SaleLineItem lineItem in tmp)
                {

                    if (lineItem.LineId == tmp.Count)
                    {
                        lineItem.Comment = GME_Var.salesPerson + System.Environment.NewLine;
                                             
                    }

                   
                }                                
            }
            else
            {
                //DISKON & PROMO                
                GME_Var.isExclusive = false;
                LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
                DataTable dts = new DataTable();
                dts = data.getAllInfo(Connection.applicationLoc.Settings.Database.Connection.ConnectionString);
                decimal amountItem = 0;
                decimal tothargaakhir = 0;
                decimal hargaakhir = 0;
                decimal tothargaawal = 0;
                decimal percentage = 0;
                string[] temp = new string[3];
                decimal tmpTotal = 0;
                decimal bestAmount = 0;
                decimal compareAmount = 0;
                string oldItemID = "";
                DataTable tmpTable;
                GME_Var.iscekitemLine = false;
                GME_Var.isDiscountORApplied = false;
                DiscountItem disc;
                string bestOfferId = "";
                Boolean isGreater = true;
                foreach (SaleLineItem lineItem in tmp)
                {
                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                    int itemtype = data.getItemType(lineItem.ItemId, Connection.applicationLoc.Settings.Database.DataAreaID, Connection.applicationLoc.Settings.Database.Connection.ConnectionString);

                    if (lineItem.LineId == tmp.Count)
                    {
                        lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;

                    }

                    //IF VOUCHER WAS APPLIED (FOR BIRTHDAY VOUCHER)
                    if (GME_Var.isVoucherAppliedforBirthday == true && lineItem.LineId == tmp.Count)
                    {
                        posTransaction = GME_Var.welcomePosTransaction;
                        int j = 0;
                        var categoryAX = new long[data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
                        foreach (DataRow row in data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                        {
                            categoryAX[j] = row.Field<long>("PARENTCATEGORY");
                            j++;
                        }

                        int count = 0;
                        var itemVouchBirthday = new string[0];
                        var itemVouchBirthdayOld = new string[0];
                        foreach (SaleLineItem lineItems in tmp)
                        {
                            foreach (DataRow row in data.getCategoryItem(lineItems.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("CATEGORY", typeof(string));
                                dt.Rows.Add(row.Field<long>("LEVEL1"));
                                dt.Rows.Add(row.Field<long>("LEVEL2"));
                                dt.Rows.Add(row.Field<long>("LEVEL3"));
                                dt.Rows.Add(row.Field<long>("LEVEL4"));
                                dt.Rows.Add(row.Field<long>("LEVEL5"));
                                dt.Rows.Add(row.Field<long>("LEVEL6"));
                                string[] allcategoryItem = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                                for (j = 0; j < categoryAX.Length; j++)
                                {
                                    for (int i = 0; i < allcategoryItem.Length; i++)
                                    {
                                        if (categoryAX[j] == Convert.ToInt64(allcategoryItem[i]))
                                        {
                                            { goto outtoendmethod; }
                                        }

                                        if (i == allcategoryItem.Length - 1 && j == categoryAX.Length - 1)
                                        {
                                            count++;
                                            itemVouchBirthday = new string[count];
                                            itemVouchBirthday[count - 1] = lineItems.ItemId;

                                            if (itemVouchBirthday[0] == null)
                                            {
                                                for (int z = 0; z < itemVouchBirthdayOld.Length; z++)
                                                {
                                                    itemVouchBirthday[z] = itemVouchBirthdayOld[z];
                                                }
                                            }
                                            itemVouchBirthdayOld = itemVouchBirthday;
                                            { goto outtoendmethod; }
                                        }
                                    }
                                }
                            }
                            outtoendmethod:;
                        }
                        GME_Var.itemVouchTBSI = itemVouchBirthday;

                        //GET NET AMOUNT
                        decimal netamountBirthday = 0;
                        foreach (SaleLineItem lineItems in tmp)
                        {
                            for (int i = 0; i < itemVouchBirthdayOld.Length; i++)
                            {
                                if (lineItems.ItemId == itemVouchBirthday[i] && lineItems.Voided == false)
                                {
                                    netamountBirthday = netamountBirthday + lineItems.NetAmount;
                                }
                            }
                            GME_Var.netAmountBirthday = netamountBirthday;
                        }

                        //APPLY VOUCHER
                        if (GME_Var.isBirthdayNotProrate == true)
                        {
                            disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                            foreach (var val in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (val.ItemId == GME_Var.itemVouchTBSI[i] && val.Voided == false)
                                    {
                                        disc.Percentage = GME_Var.percentageDisc;

                                        val.DiscountLines.AddFirst(disc);
                                        val.Comment = posTransaction.OperatorId + System.Environment.NewLine;

                                        val.CalculateLine();
                                        retailTrans.CalcTotals();
                                        Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                    }
                                }
                            }
                        }

                        //check var global jika ada maka prorate?
                        if (GME_Var.isBirthdayProrate == true && GME_Var.birthdayAmountDisc != 0)
                        {
                            decimal potongan = 0;
                            disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                            foreach (var val in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (val.ItemId == GME_Var.itemVouchTBSI[i] && val.Voided == false)
                                    {
                                        potongan = (val.NetAmount * GME_Var.birthdayAmountDisc) / GME_Var.netAmountBirthday;
                                        disc.Amount = decimal.Round(potongan) / val.Quantity;

                                        val.DiscountLines.AddFirst(disc);
                                        val.Comment = posTransaction.OperatorId + System.Environment.NewLine;

                                        val.CalculateLine();
                                        retailTrans.CalcTotals();
                                        Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                    }
                                }
                            }
                        }
                    }

                    //IF VOUCHER WAS APPLIED (FOR VMS & WELCOME VOUCHER)
                    if (GME_Var.isVoucherApplied == true && lineItem.LineId == tmp.Count)
                    {
                        int j = 0;
                        var categoryAX = new long[data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
                        foreach (DataRow row in data.getItemVoucherexclude(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                        {
                            categoryAX[j] = row.Field<long>("PARENTCATEGORY");
                            j++;
                        }

                        int count = 0;
                        var itemVouchTBSI = new string[0];
                        var itemVouchTBSIold = new string[0];
                        foreach (SaleLineItem lineItems in tmp)
                        {
                            foreach (DataRow row in data.getCategoryItem(lineItems.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("CATEGORY", typeof(string));
                                dt.Rows.Add(row.Field<long>("LEVEL1"));
                                dt.Rows.Add(row.Field<long>("LEVEL2"));
                                dt.Rows.Add(row.Field<long>("LEVEL3"));
                                dt.Rows.Add(row.Field<long>("LEVEL4"));
                                dt.Rows.Add(row.Field<long>("LEVEL5"));
                                dt.Rows.Add(row.Field<long>("LEVEL6"));
                                string[] allcategoryItem = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                                for (j = 0; j < categoryAX.Length; j++)
                                {
                                    for (int i = 0; i < allcategoryItem.Length; i++)
                                    {
                                        if (categoryAX[j] == Convert.ToInt64(allcategoryItem[i]))
                                        {
                                            { goto outtoendmethod; }
                                        }

                                        if (i == allcategoryItem.Length - 1 && j == categoryAX.Length - 1)
                                        {
                                            count++;
                                            itemVouchTBSI = new string[count];
                                            itemVouchTBSI[count - 1] = lineItems.ItemId;

                                            if (itemVouchTBSI[0] == null)
                                            {
                                                for (int z = 0; z < itemVouchTBSIold.Length; z++)
                                                {
                                                    itemVouchTBSI[z] = itemVouchTBSIold[z];
                                                }
                                            }
                                            itemVouchTBSIold = itemVouchTBSI;
                                            { goto outtoendmethod; }
                                        }
                                    }
                                }
                            }
                            outtoendmethod:;
                        }
                        GME_Var.itemVouchTBSI = itemVouchTBSI;

                        //WELCOME VOUCHER 50
                        if (GME_Var.iswelcomevoucher50 == true)
                        {
                            //GET AMOUNTDUE WELCOME 50K
                            int counts = 0;
                            int countwlcm = 0;
                            var itemVouchWLCM50 = new string[0];
                            var itemVouchWLCM50old = new string[0];
                            decimal stash50k = 0;

                            var minbelanja = Convert.ToDecimal(data.getMinTransaksi50(Connection.applicationLoc.Settings.Database.Connection.ConnectionString));
                            var categoryWelcome50AX = new long[data.getkategori50(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
                            foreach (DataRow row in data.getkategori50(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                            {
                                categoryWelcome50AX[countwlcm] = row.Field<long>("RECID");
                                countwlcm++;
                            }

                            foreach (var lineItems in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (lineItems.ItemId == GME_Var.itemVouchTBSI[i] && lineItems.Voided == false)
                                    {
                                        foreach (DataRow row in data.getCategoryItem(lineItems.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                                        {
                                            DataTable dt = new DataTable();
                                            dt.Columns.Add("KATEGORI", typeof(string));
                                            dt.Rows.Add(row.Field<long>("LEVEL1"));
                                            dt.Rows.Add(row.Field<long>("LEVEL2"));
                                            dt.Rows.Add(row.Field<long>("LEVEL3"));
                                            dt.Rows.Add(row.Field<long>("LEVEL4"));
                                            dt.Rows.Add(row.Field<long>("LEVEL5"));
                                            dt.Rows.Add(row.Field<long>("LEVEL6"));
                                            string[] allcategory = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                                            for (int k = 0; k < categoryWelcome50AX.Length; k++)
                                            {
                                                for (int l = 0; l < allcategory.Length; l++)
                                                {
                                                    if (categoryWelcome50AX[k] == Convert.ToInt64(allcategory[l]))
                                                    {
                                                        counts++;
                                                        itemVouchWLCM50 = new string[counts];
                                                        itemVouchWLCM50[counts - 1] = lineItems.ItemId;

                                                        if (itemVouchWLCM50[0] == null)
                                                        {
                                                            for (int iterasi = 0; iterasi < itemVouchWLCM50old.Length; iterasi++)
                                                            {
                                                                itemVouchWLCM50[iterasi] = itemVouchWLCM50old[iterasi];
                                                            }
                                                        }
                                                        itemVouchWLCM50old = itemVouchWLCM50;

                                                        stash50k = stash50k + lineItems.NetAmount;
                                                        { goto outtoendmethod; }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                outtoendmethod:;
                                GME_Var.displaynumberWelcomeVouch = itemVouchWLCM50old;
                            }

                            //APPLY VOUCHER
                            foreach (var lineItems in retailTrans.SaleItems)
                            {
                                disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                                var displaynumberwlcmvouch = GME_Var.displaynumberWelcomeVouch;

                                if (GME_Var.amountVouchTBSI == 0)
                                {
                                    GME_Var.amountVouchTBSI = GME_Var.wlcmvouchamount;
                                }

                                for (int i = 0; i < displaynumberwlcmvouch.Length; i++)
                                {
                                    if (lineItems.ItemId == displaynumberwlcmvouch[i])
                                    {
                                        if (lineItems.NetAmount != 0 && lineItems.Voided == false && GME_Var.wlcmvouchamount != 0)
                                        {
                                            decimal prorate = 0;
                                            prorate = (lineItems.NetAmount / stash50k) * GME_Var.amountVouchTBSI;

                                            decimal potongan = prorate / lineItems.Quantity;
                                            disc.Amount = potongan;
                                            lineItems.DiscountLines.AddFirst(disc);
                                            lineItems.Comment = lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                        }
                                        lineItems.CalculateLine();
                                        retailTrans.CalcTotals();
                                        Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                    }
                                }
                            }
                        }

                        //WELCOME VOUCHER 100
                        else if (GME_Var.iswelcomevoucher100 == true)
                        {
                            //GET AMOUNTDUE WELCOME 100K
                            int counts = 0;
                            int countwlcm = 0;
                            var itemVouchWLCM100 = new string[0];
                            var itemVouchWLCM100old = new string[0];
                            decimal stash100k = 0;

                            var minbelanja = Convert.ToDecimal(data.getMinTransaksi100(GME_Custom.GME_Propesties.Connection.applicationLoc.Settings.Database.Connection.ConnectionString));
                            var categoryWelcome100AX = new long[data.getkategori100(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows.Count];
                            foreach (DataRow row in data.getkategori100(Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                            {
                                categoryWelcome100AX[countwlcm] = row.Field<long>("RECID");
                                countwlcm++;
                            }

                            foreach (var lineItems in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (lineItems.ItemId == GME_Var.itemVouchTBSI[i] && lineItems.Voided == false)
                                    {
                                        foreach (DataRow row in data.getCategoryItem(lineItems.ItemId, Connection.applicationLoc.Settings.Database.Connection.ConnectionString).Rows)
                                        {
                                            DataTable dt = new DataTable();
                                            dt.Columns.Add("KATEGORI", typeof(string));
                                            dt.Rows.Add(row.Field<long>("LEVEL1"));
                                            dt.Rows.Add(row.Field<long>("LEVEL2"));
                                            dt.Rows.Add(row.Field<long>("LEVEL3"));
                                            dt.Rows.Add(row.Field<long>("LEVEL4"));
                                            dt.Rows.Add(row.Field<long>("LEVEL5"));
                                            dt.Rows.Add(row.Field<long>("LEVEL6"));
                                            string[] allcategory = dt.Rows.OfType<DataRow>().Select(y => y[0].ToString()).ToArray();

                                            for (int k = 0; k < categoryWelcome100AX.Length; k++)
                                            {
                                                for (int l = 0; l < allcategory.Length; l++)
                                                {
                                                    if (categoryWelcome100AX[k] == Convert.ToInt64(allcategory[l]))
                                                    {
                                                        counts++;
                                                        itemVouchWLCM100 = new string[counts];
                                                        itemVouchWLCM100[counts - 1] = lineItems.ItemId;

                                                        if (itemVouchWLCM100[0] == null)
                                                        {
                                                            for (int iterasi = 0; iterasi < itemVouchWLCM100old.Length; iterasi++)
                                                            {
                                                                itemVouchWLCM100[iterasi] = itemVouchWLCM100old[iterasi];
                                                            }
                                                        }
                                                        itemVouchWLCM100old = itemVouchWLCM100;

                                                        stash100k = stash100k + lineItems.NetAmount;
                                                        { goto outtoendmethod; }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                outtoendmethod:;
                                GME_Var.displaynumberWelcomeVouch = itemVouchWLCM100old;
                            }

                            //APPLY VOUCHER
                            foreach (var lineItems in retailTrans.SaleItems)
                            {
                                disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                                var displaynumberwlcmvouch = GME_Var.displaynumberWelcomeVouch;

                                if (GME_Var.amountVouchTBSI == 0)
                                {
                                    GME_Var.amountVouchTBSI = GME_Var.wlcmvouchamount;
                                }

                                for (int i = 0; i < displaynumberwlcmvouch.Length; i++)
                                {
                                    if (lineItems.ItemId == displaynumberwlcmvouch[i])
                                    {
                                        if (lineItems.NetAmount != 0 && lineItems.Voided == false && GME_Var.wlcmvouchamount != 0)
                                        {
                                            decimal prorate = 0;
                                            prorate = (lineItems.NetAmount / stash100k) * GME_Var.amountVouchTBSI;

                                            decimal potongan = prorate / lineItems.Quantity;
                                            disc.Amount = potongan;
                                            lineItems.DiscountLines.AddFirst(disc);
                                            lineItems.Comment = lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                        }
                                        lineItems.CalculateLine();
                                        retailTrans.CalcTotals();
                                        Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                    }
                                }
                            }
                        }
                        //VMS
                        else
                        {
                            //GET AMOUNTDUE              
                            decimal amountdue = 0;
                            foreach (var lineItems in retailTrans.SaleItems)
                            {
                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (lineItems.ItemId == GME_Var.itemVouchTBSI[i] && lineItems.Voided == false)
                                    {
                                        amountdue = amountdue + lineItems.NetAmount;
                                    }
                                }
                            }

                            //APPLY VOUCHER
                            foreach (var lineItems in retailTrans.SaleItems)
                            {
                                disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                                if (GME_Var.amountVouchTBSI == 0)
                                {
                                    GME_Var.amountVouchTBSI = GME_Var.wlcmvouchamount;
                                }

                                for (int i = 0; i < GME_Var.itemVouchTBSI.Length; i++)
                                {
                                    if (lineItems.ItemId == GME_Var.itemVouchTBSI[i] && lineItems.Voided == false)
                                    {
                                        decimal prorate = 0;
                                        prorate = (lineItems.NetAmount / amountdue) * GME_Var.amountVouchTBSI;

                                        decimal potongan = prorate / lineItems.Quantity;
                                        disc.Amount = potongan;
                                        lineItems.DiscountLines.AddLast(disc);
                                        lineItems.Comment = posTransaction.OperatorId + System.Environment.NewLine;
                                    }
                                }
                                GME_Var.isSetQty = false;
                                lineItems.CalculateLine();
                                retailTrans.CalcTotals();
                                Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                            }
                        }
                    }

                    //GWP
                    if (!GME_Var.isExclusive)
                    {
                        var res = from row in dts.AsEnumerable()
                                  where row.Field<string>("TBS_FREEDISCITEMID") == lineItem.ItemId && !lineItem.Voided
                                  select row;

                        foreach (DataRow a in res)
                        {
                            tmpTotal = 0;
                            foreach (SaleLineItem linesItemTotal in tmp)
                            {
                                if (!linesItemTotal.Voided)
                                {
                                    tmpTable = data.getCategoryProduct(Connection.applicationLoc.Settings.Database.Connection.ConnectionString, linesItemTotal.ItemId, Convert.ToInt64(a["CATEGORY"]));
                                    if (tmpTable.Rows.Count > 0)
                                    {
                                        tmpTotal += linesItemTotal.NetAmount;
                                    }
                                }
                            }
                            if (Convert.ToDecimal(a["AMOUNTTHRESHOLD"]) <= tmpTotal)
                            {
                                GME_Var.itemDiscount = lineItem.ItemId;
                                GME_Var.getDiscountType = Convert.ToInt32(a["TBS_DiscountType"]);
                                GME_Var.getOperator = Convert.ToInt32(a["TBS_Operator"]);
                                GME_Var.itemDiscAmount = Convert.ToInt32(a["DISCOUNTVALUE"]);
                                GME_Var.exclusiveOfferid = a["OFFERID"].ToString();
                                GME_Var.itemDiscount = a["TBS_FREEDISCITEMID"].ToString();
                                GME_Var.iscekitemLine = true;
                                if (!GME_Var.isExclusive)
                                {
                                    switch (a["CONCURRENCYMODE"].ToString())
                                    {
                                        case "0": // exclusive
                                            GME_Var.isExclusive = true;
                                            bestOfferId = GME_Var.exclusiveOfferid;
                                            isGreater = true;
                                            break;
                                        case "1"://BestPrice
                                            isGreater = false;
                                            switch (GME_Var.getDiscountType)
                                            {
                                                case 0://percentage
                                                    compareAmount = lineItem.NetAmount * (GME_Var.itemDiscAmount / 100);
                                                    isGreater = compareAmount > bestAmount ? true : false;
                                                    bestAmount = compareAmount > bestAmount ? compareAmount : bestAmount;

                                                    if (GME_Var.getOperator == 2 && isGreater)
                                                    {
                                                        GME_Var.isDiscountORApplied = false;
                                                    }
                                                    break;
                                                case 1://amount
                                                    compareAmount = GME_Var.itemDiscAmount;
                                                    isGreater = compareAmount > bestAmount ? true : false;
                                                    bestAmount = compareAmount > bestAmount ? compareAmount : bestAmount;
                                                    break;
                                                case 2: // deal price
                                                    compareAmount = lineItem.NetAmount - GME_Var.itemDiscAmount;
                                                    isGreater = compareAmount > bestAmount ? true : false;
                                                    bestAmount = compareAmount > bestAmount ? compareAmount : bestAmount;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                //cek Best Price


                            }
                        }
                    }


                    //JIKA ADA DISKON YANG TERTANGKAP
                    if (GME_Var.iscekitemLine == true && isGreater)
                    {
                        //CEK ITEMS
                        //--------------Discount Type (Discount %)---------------//
                        if (GME_Var.getDiscountType == 0)
                        {

                            disc = new LineDiscountItem();

                            //-------------------------NULL----------------------//
                            if (GME_Var.getOperator == 0)
                            {
                                if (lineItem.Voided == false)
                                {
                                    disc.Percentage = GME_Var.itemDiscAmount / lineItem.Quantity;
                                    lineItem.DiscountLines.AddFirst(disc);
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r" + GME_Var.exclusiveOfferid + System.Environment.NewLine;

                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                }
                            }

                            //-------------------------AND-----------------------//
                            if (GME_Var.getOperator == 1)
                            {
                                if (lineItem.Voided == false)
                                {
                                    oldItemID = lineItem.ItemId;
                                    disc.Percentage = GME_Var.itemDiscAmount / lineItem.Quantity;
                                    lineItem.DiscountLines.AddFirst(disc);
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r" + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine;

                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                }
                            }

                            //-------------------------OR------------------------//
                            if (GME_Var.getOperator == 2)
                            {
                                if (lineItem.Voided == false && GME_Var.isDiscountORApplied == false)
                                {
                                    if (oldItemID != "")
                                    {
                                        RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                                        //retailTransaction.SaleItems.qua
                                        ////int tmpLine;
                                        LinkedList<SaleLineItem> lsSaleLineItemRemove = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;

                                        lsSaleLineItemRemove.ToList();

                                        var sel = lsSaleLineItemRemove.Where(os =>
                                               oldItemID == os.ItemId
                                            );

                                        foreach (var a in sel)
                                        {

                                            //LSRetailPosis.Transaction.Line.Discount.DiscountItem discs;
                                            //discs = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                                            //disc.Percentage = 0;
                                            a.DiscountLines.RemoveFirst();
                                            a.CustomerDiscount = 0;
                                            a.Comment = posTransaction.OperatorId + System.Environment.NewLine;// + "Diskon & Promo \r" + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine;
                                            //a.OriginalPrice = a.NetAmount * a.Quantity;
                                            //a.TradeAgreementPrice = a.OriginalPrice;
                                            a.CalculateLine();
                                            retailTrans.CalcTotals();
                                            Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                        }
                                    }
                                    oldItemID = lineItem.ItemId;
                                    disc.Percentage = GME_Var.itemDiscAmount / lineItem.Quantity;
                                    lineItem.DiscountLines.AddFirst(disc);
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r" + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine;
                                    GME_Var.isDiscountORApplied = true;

                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                }
                            }
                        }

                        //--------------Discount Type (Discount Amount)----------//
                        if (GME_Var.getDiscountType == 1)
                        {
                            disc = new LineDiscountItem();

                            //-------------------------NULL----------------------//
                            if (GME_Var.getOperator == 0)
                            {
                                if (lineItem.Voided == false)
                                {
                                    disc.Amount = GME_Var.itemDiscAmount / lineItem.Quantity;
                                    lineItem.DiscountLines.AddFirst(disc);
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine;

                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                }
                            }

                            //-------------------------AND-----------------------//
                            if (GME_Var.getOperator == 1)
                            {
                                decimal amountdue = 0;

                                //MENCARI TOTAL AMOUNT ITEM DISKON
                                foreach (var lineItems in retailTrans.SaleItems)
                                {
                                    for (int i = 0; i < GME_Var.itemdiscount.Length; i++)
                                    {
                                        if (lineItems.ItemId == GME_Var.itemdiscount[i] && lineItems.Voided == false)
                                        {
                                            amountdue = GME_Var.getamountdue;

                                            amountdue = amountdue + lineItems.NetAmount;
                                            GME_Var.getamountdue = amountdue;
                                        }
                                    }
                                }

                                //APPLY DISKON
                                foreach (var lineItems in retailTrans.SaleItems)
                                {
                                    for (int i = 0; i < GME_Var.itemdiscount.Length; i++)
                                    {
                                        if (lineItems.ItemId == GME_Var.itemdiscount[i] && lineItems.Voided == false)
                                        {
                                            if (lineItems.Quantity > 1)
                                            {
                                                decimal prorate = 0;
                                                prorate = (lineItems.NetAmount / amountdue) * GME_Var.itemDiscAmount;
                                                decimal potongan = prorate / lineItems.Quantity;

                                                disc.Amount = potongan;
                                                lineItems.DiscountLines.AddFirst(disc);
                                                lineItems.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine;
                                            }
                                            else
                                            {
                                                decimal potongan = 0;
                                                potongan = (lineItems.NetAmount / amountdue) * GME_Var.itemDiscAmount;

                                                disc.Amount = potongan / lineItems.Quantity;
                                                lineItems.DiscountLines.AddFirst(disc);
                                                lineItems.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r '" + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + "'" + System.Environment.NewLine;

                                                lineItem.CalculateLine();
                                                retailTrans.CalcTotals();
                                                Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                            }
                                        }
                                    }
                                }
                            }

                            //-------------------------OR------------------------//
                            if (GME_Var.getOperator == 2)
                            {
                                if (lineItem.Voided == false && GME_Var.isDiscountORApplied == false)
                                {
                                    disc.Amount = GME_Var.itemDiscAmount / lineItem.Quantity;
                                    lineItem.DiscountLines.AddFirst(disc);
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine;
                                    GME_Var.isDiscountORApplied = true;

                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                }
                            }
                        }

                        //-------------Discount Type(Deal Price)-----------------//
                        if (GME_Custom.GME_Propesties.GME_Var.getDiscountType == 2)
                        {
                            disc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();

                            //-------------------------NULL----------------------//
                            if (GME_Var.getOperator == 0)
                            {
                                if (lineItem.Voided == false)
                                {
                                    int iLastLine = lineItem.LineId;
                                    SaleLineItem sl = retailTrans.GetItem(iLastLine);
                                    sl.Price = (GME_Var.itemDiscAmount + ((lineItem.Quantity - 1) * lineItem.Price)) / lineItem.Quantity;
                                    lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Var.exclusiveOfferid + System.Environment.NewLine + "Price has overridden from " + lineItem.NetAmount;

                                    lineItem.CalculateLine();
                                    retailTrans.CalcTotals();
                                    Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                }
                            }

                            //-------------------------AND-----------------------//
                            if (GME_Var.getOperator == 1)
                            {
                                decimal amountdue = 0;
                                int count = 0;

                                //MENCARI TOTAL AMOUNT ITEM DISKON
                                foreach (var lineItems in retailTrans.SaleItems)
                                {
                                    for (int i = 0; i < GME_Var.itemdiscount.Length; i++)
                                    {
                                        if (lineItems.ItemId == GME_Var.itemdiscount[i] && lineItems.Voided == false)
                                        {
                                            amountdue = GME_Var.getamountdue;

                                            amountdue = amountdue + lineItems.NetAmount;
                                            GME_Var.getamountdue = amountdue;
                                            count++;
                                        }
                                    }
                                }

                                //APPLY DISKON
                                foreach (var lineItems in retailTrans.SaleItems)
                                {
                                    if (lineItems.NetAmount > GME_Var.itemDiscAmount)
                                    {
                                        for (int i = 0; i < GME_Var.itemdiscount.Length; i++)
                                        {
                                            if (lineItems.ItemId == GME_Var.itemdiscount[i] && count == 1 && lineItems.Voided == false)
                                            {
                                                int iLastLine = lineItems.LineId;
                                                SaleLineItem sl = retailTrans.GetItem(iLastLine);
                                                sl.Price = (GME_Var.itemDiscAmount + ((lineItems.Quantity - 1) * lineItems.Price)) / lineItems.Quantity;
                                                lineItems.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine + "Price has overridden from " + lineItems.NetAmount;

                                                lineItem.CalculateLine();
                                                retailTrans.CalcTotals();
                                                Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                            }
                                            else if (lineItems.ItemId == GME_Custom.GME_Propesties.GME_Var.itemdiscount[i] && count > 1 && lineItems.Voided == false)
                                            {
                                                int iLastLine = lineItems.LineId;
                                                SaleLineItem sl = retailTrans.GetItem(iLastLine);

                                                decimal a = 0;
                                                a = (lineItems.NetAmount / amountdue) * (amountdue - GME_Custom.GME_Propesties.GME_Var.itemDiscAmount);

                                                sl.Price = ((lineItems.NetAmount - a) + ((lineItems.Quantity - 1) * lineItems.Price)) / lineItems.Quantity;
                                                lineItems.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Custom.GME_Propesties.GME_Var.exclusiveOfferid + System.Environment.NewLine + "Price has overridden from " + lineItems.NetAmount;

                                                lineItem.CalculateLine();
                                                retailTrans.CalcTotals();
                                                Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                            }
                                        }
                                    }
                                }
                            }

                            //-------------------------OR------------------------//
                            if (GME_Var.getOperator == 2)
                            {
                                foreach (var lineItems in retailTrans.SaleItems)
                                {
                                    if (lineItems.NetAmount > GME_Var.itemDiscAmount)
                                    {
                                        for (int i = 0; i < GME_Var.itemdiscount.Length; i++)
                                        {
                                            if (lineItem.Voided == false && GME_Var.isDiscountORApplied == false)
                                            {
                                                int iLastLine = lineItem.LineId;
                                                SaleLineItem sl = retailTrans.GetItem(iLastLine);
                                                sl.Price = (GME_Var.itemDiscAmount + ((lineItem.Quantity - 1) * lineItem.Price)) / lineItem.Quantity;
                                                lineItem.Comment = posTransaction.OperatorId + System.Environment.NewLine + "Diskon & Promo \r " + GME_Var.exclusiveOfferid + System.Environment.NewLine + "Price has overridden from " + lineItem.NetAmount;
                                                GME_Var.isDiscountORApplied = true;

                                                lineItem.CalculateLine();
                                                retailTrans.CalcTotals();
                                                Connection.applicationLoc.Services.Tax.CalculateTax(retailTrans);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            GME_Var.doneCheckItemDiscount = true;
            GME_Var.retailTransGlobal = retailTrans;
        }

        public void PreReturnItem(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreReturnItem", "Prior to entering return state...", LSRetailPosis.LogTraceLevel.Trace);                
        }

        public void PostReturnItem(IPosTransaction posTransaction)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostReturnItem", "After entering return state", LSRetailPosis.LogTraceLevel.Trace);                    
        }

        public void PreVoidItem(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreVoidItem", "Before voiding an item", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostVoidItem(IPosTransaction posTransaction, int lineId)
        {
            string source = "IItemTriggersV1.PostVoidItem";
            string value = "After voiding an item";
            LSRetailPosis.ApplicationLog.Log(source, value, LSRetailPosis.LogTraceLevel.Trace);
            LSRetailPosis.ApplicationLog.WriteAuditEntry(source, value);

            RetailTransaction retailTrans = posTransaction as RetailTransaction;

            LinkedList<SaleLineItem> tmp = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;
            foreach (SaleLineItem lineItem in tmp)
            {
                if (lineItem.LineId == lineId)
                {
                    GME_Var.totalAmountEDC = GME_Var.totalAmountEDC - lineItem.NetAmount;
                }
            }
            PostSale(posTransaction);
            GME_Var.retailTransGlobal = retailTrans;
        }

        public void PreSetQty(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PreSetQty", "Before setting the qty for an item", LSRetailPosis.LogTraceLevel.Trace);
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            LinkedList<SaleLineItem> lsSaleLineItem = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;

            lsSaleLineItem.ToList();

            var sel = lsSaleLineItem.Where(os =>
                   lineId == os.LineId
                );

            foreach (var a in sel)
            {
                GME_Var.qtyBefore = a.Quantity;
                GME_Var.lineNumberGlobal = a.LineId;
            }
        }

        public void PostSetQty(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostSetQty", "After setting the qty from an item", LSRetailPosis.LogTraceLevel.Trace);

            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            decimal qtyafter = 0;
            LinkedList<SaleLineItem> lsSaleLineItem = ((LSRetailPosis.Transaction.RetailTransaction)(posTransaction)).SaleItems;

            lsSaleLineItem.ToList();

            var sel = lsSaleLineItem.Where(os =>
                   GME_Var.lineNumberGlobal == os.LineId
                );

            foreach (var a in sel)
            {
                qtyafter = a.Quantity;
            }

            if (qtyafter != GME_Var.qtyBefore)
                PostSale(posTransaction);
        }

        public void PrePriceOverride(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PrePriceOverride", "Before overriding the price on an item", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void  PostPriceOverride(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV1.PostPriceOverride", "After overriding the price of an item", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

        #region IItemTriggersV2 Members

        public void PreClearQty(IPreTriggerResult preTriggerResult, ISaleLineItem saleLineItem, IPosTransaction posTransaction, int lineId)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV2.PreClearQty", "Triggered before clear the quantity of an item.", LSRetailPosis.LogTraceLevel.Trace);
        }

        public void PostClearQty(IPosTransaction posTransaction, ISaleLineItem saleLineItem)
        {
            LSRetailPosis.ApplicationLog.Log("IItemTriggersV2.PostClearQty", "Triggered after clear the quantity of an item.", LSRetailPosis.LogTraceLevel.Trace);
        }

        #endregion

    }
}
