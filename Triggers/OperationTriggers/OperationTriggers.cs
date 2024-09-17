/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using System.Data.SqlClient;
using System;
using System.Data;

using System.Linq;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using LSRetailPosis.Settings.FunctionalityProfiles;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using LSRetailPosis.Settings;
using System.Collections.ObjectModel;
using LSRetailPosis.Transaction.Line.Discount;


namespace Microsoft.Dynamics.Retail.Pos.OperationTriggers
{
	[Export(typeof(IOperationTrigger))]
	public class OperationTriggers : IOperationTrigger
	{
        [Import]
        public IApplication Application { get; set; }
	   #region Constructor - Destructor
		
		public OperationTriggers()
		{
			
			// Get all text through the Translation function in the ApplicationLocalizer
			// TextID's for InfocodeTriggers are reserved at 59000 - 59999
		}

		#endregion
        bool foundOperationCancelled = false;
		#region IOperationTriggersV1 Members

		/// <summary>
		/// Before the operation is processed this trigger is called.
		/// </summary>
		/// <param name="preTriggerResult"></param>
		/// <param name="posTransaction"></param>
		/// <param name="posisOperation"></param>
		public void PreProcessOperation(IPreTriggerResult preTriggerResult, IPosTransaction posTransaction, PosisOperations posisOperation)
		{

            //if (posisOperation == PosisOperations.RecallUnconcludedTransaction)
            //{
            //    posTransaction.OperationCancelled = true;
            //    preTriggerResult.ContinueOperation = false;
            //}

            

            //add by Yonathan to disable any operation when promo discount payment applied - 23/01/2024 - CPPOS_PROMODISCPAYMENT
            if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction" )
            {
                RetailTransaction transaction = posTransaction as RetailTransaction;
                if(transaction.Customer != null && transaction.SaleItems.Count == 0 && (posisOperation == PosisOperations.ConvertCustomerOrder || posisOperation == PosisOperations.CustomerOrderDetails))
                {
                     using (frmMessage dialog = new frmMessage("Keranjang masih kosong, tidak bisa membuat customer order", MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                POSFormsManager.ShowPOSForm(dialog);
                                posTransaction.OperationCancelled = true;
                                preTriggerResult.ContinueOperation = false;
                                return;
                            }
 
                }



                if (transaction.Comment == "PAYMENTDISCOUNT" || transaction.Comment == "PROMOED" || transaction.Comment == "PROMORCPT") //if (transaction.Comment == "PAYMENTDISCOUNT"  || transaction.Comment == "PROMOPDI" || transaction.Comment == "PROMOPDIS")                                  
                {

                    /*
                     * yang boleh = 
                     * - Pay
                     * - Void
                     * - Display total
                     * - 
                     */

                     
                    if (posisOperation != PosisOperations.PayCustomerAccount && posisOperation != PosisOperations.PayCash && posisOperation != PosisOperations.PayCard && posisOperation != PosisOperations.VoidTransaction && posisOperation != PosisOperations.ChangeBack  && posisOperation  != PosisOperations.DisplayTotal && posisOperation  != PosisOperations.BlankOperation && posisOperation != PosisOperations.ItemSale )//bukan itemsale dan bukan PROMOED) )
                    {

                        if (posisOperation == PosisOperations.LinkedItemsAdd && transaction.SaleItems.Last().IsInfoCodeItem == true)
                        {
                            transaction.SaleItems.Last().IsInfoCodeItem = false;
                        }
                        else 
                        {
                            using (frmMessage dialog = new frmMessage("Fungsi ini dibatasi ketika sudah apply discount PROMO. Silakan lanjut ke menu pembayaran atau batalkan sepenuhnya (void) transaksi ini", MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                POSFormsManager.ShowPOSForm(dialog);
                                posTransaction.OperationCancelled = true;
                                preTriggerResult.ContinueOperation = false;
                                return;
                            }
                        }
                        
                    }

                    transaction.SaleItems.Last().IsInfoCodeItem = false;
                    
                    
                }
            }
           
           
            //end


            //add by Yonathan to validate if customer order only can void transaction & pay
            if (posTransaction.ToString() == "LSRetailPosis.Transaction.CustomerOrderTransaction" && (posisOperation != PosisOperations.PayCustomerAccount && posisOperation != PosisOperations.PayCash && posisOperation != PosisOperations.PayCard && posisOperation != PosisOperations.VoidTransaction && posisOperation != PosisOperations.ChangeBack && posisOperation != PosisOperations.DisplayTotal && posisOperation != PosisOperations.BlankOperation && posisOperation != PosisOperations.ItemSale && posisOperation != PosisOperations.ConvertCustomerOrder && posisOperation != PosisOperations.CustomerOrderDetails))
            {
                using (frmMessage dialog = new frmMessage("Fungsi ini dibatasi untuk transaksi Customer Order. Silakan lanjut ke menu pembayaran atau batalkan sepenuhnya (void) transaksi ini", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                    posTransaction.OperationCancelled = true;
                    preTriggerResult.ContinueOperation = false;
                    return;
                }
            }
            //

			int flag = 0;
			LSRetailPosis.ApplicationLog.Log("ICustomerTriggersV1.PreProcessOperation", "Before the operation is processed this trigger is called.", LSRetailPosis.LogTraceLevel.Trace);
			if (posisOperation == PosisOperations.CloseShift)
			{
				//custom by Yonathan to validate the settlement before closing the shift 13/03/2023
				SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection; 

				//before check settlement, check if transaction debit, credit, qr exist in transdate today
				try
				{
					//string queryStringPayment = @"SELECT TenderName,Store,TransDate FROM [ax].[CPNEWPAYMENTPOS] WHERE  STORE = @STORE AND TRANSDATE = @TRANSDATE AND (TENDERNAME LIKE 'QRIS%' OR TENDERNAME LIKE 'DEBIT%' OR TENDERNAME LIKE 'CREDIT%')";
					//string queryStringPayment = @"SELECT TenderName,Store,TransDate FROM [ax].[CPNEWPAYMENTPOS] WHERE  STORE = @STORE AND CAST(TRANSDATE as date) BETWEEN  @TRANSDATE AND @TRANSDATE  AND (TENDERNAME LIKE 'QRIS%' OR TENDERNAME LIKE 'DEBIT%' OR TENDERNAME LIKE 'CREDIT%')";
					string queryStringPayment = @"SELECT BANK,STORE,TRANSDATE,DATAAREAID      
												  FROM [ax].[CPAMBILTUNAI]
												  WHERE STORE = @STORE
													AND CAST(TRANSDATE as date) BETWEEN  @TRANSDATE AND @TRANSDATE";
					//TRANSDATE >= '2023-03-15 00:00:00.000' AND TRANSDATE <= '2023-03-15 23:59:59.999'
					using (SqlCommand command = new SqlCommand(queryStringPayment, connectionStore))
					{
						command.Parameters.AddWithValue("@STORE", posTransaction.StoreId);
						command.Parameters.AddWithValue("@TRANSDATE", DateTime.Now.ToString("yyyy-MM-dd"));

						if (connectionStore.State != ConnectionState.Open)
						{
							connectionStore.Open();
						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.HasRows == true)
							{
								flag = 1;
								
								//dataArea = reader[0] + "";
							}
						}
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connectionStore.State != ConnectionState.Closed)
					{
						connectionStore.Close();
					}
				}

				//if there is payment with debit,credit, or QRIS, then check settlement table
				if (flag == 1)
				{
					try
					{
						string queryStringStore = @"SELECT * FROM AX.CPSETTLEMENT WHERE STOREID = @STORE AND TRANSDATE = @TRANSDATE";

						//RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

						using (SqlCommand command = new SqlCommand(queryStringStore, connectionStore))
						{
							command.Parameters.AddWithValue("@STORE", posTransaction.StoreId);
							command.Parameters.AddWithValue("@TRANSDATE", DateTime.Now.ToString("yyyy-MM-dd"));

							if (connectionStore.State != ConnectionState.Open)
							{
								connectionStore.Open();
							}
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.HasRows == false)
								{
									using (frmMessage dialog = new frmMessage("Lakukan Settlement EDC terlebih dahulu!!\nKlik Additional Tasks - Settlement EDC", MessageBoxButtons.OK, MessageBoxIcon.Error))
									{
										POSFormsManager.ShowPOSForm(dialog);
										posTransaction.OperationCancelled = true;
										preTriggerResult.ContinueOperation = false;
										return;
									}
									//dataArea = reader[0] + "";
								}
							}
						}
					}
					catch (Exception ex)
					{
						LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
						throw;
					}
					finally
					{
						if (connectionStore.State != ConnectionState.Closed)
						{
							connectionStore.Close();
						}
					}
				}
				
				
				
				//end
			}
		}

		/// <summary>
		/// After the operation has been processed this trigger is called.
		/// </summary>
		/// <param name="posTransaction"></param>
		/// <param name="posisOperation"></param>
		public void PostProcessOperation(IPosTransaction posTransaction, PosisOperations posisOperation)
		{
			LSRetailPosis.ApplicationLog.Log("IOperationTriggersV1.PostProcessOperation", "After the operation has been processed this trigger is called.", LSRetailPosis.LogTraceLevel.Trace);
			decimal amountTA = 0;
			string isB2bCust = "0";
			string priceGroup = "";
            string lineDiscGroup = "";
            decimal amountDisc, pct1, pct2;
            decimal totalPct = 0;
            string custId = "";
            int indexString = 1;
            bool removeItems = false;
            string ppnValidate = "False";
            string taxGroup1="";
            string taxGroup2 ="";
            bool foundDifferentTaxGroup = false;
            foundOperationCancelled = false;

            //string[] listItemToRemove;
            List<string> listItemToRemove = new List<string>();
            APIAccess.APIAccessClass.itemToRemove = new List<string>();
			//string custId = APIAccess.APIAccessClass.custId.ToString();

            //let's rework the price adjustment


            //
            if (posTransaction.OperationCancelled == true)
            {
                foundOperationCancelled = true;
            }
            
            
            
            //add by Yonathan for Applying the Customer B2B Price Group (25/07/2023)

            if ((posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction" || posTransaction.ToString() == "LSRetailPosis.Transaction.CustomerOrderTransaction") && foundOperationCancelled == false)
            {
                RetailTransaction transaction = posTransaction as RetailTransaction;
                if (transaction.CalculableSalesLines.Count != 0)
                {
                    bool isEmptyCustomer = transaction.Customer.IsEmptyCustomer();

                    if (isEmptyCustomer == false)
                    {
                        isB2bCust = APIAccess.APIAccessClass.isB2b;
                        priceGroup = APIAccess.APIAccessClass.priceGroup;//.ToString();
                        lineDiscGroup = APIAccess.APIAccessClass.lineDiscGroup;//.ToString();

                        if (isB2bCust == null)
                        {
                            try
                            {
                                ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getB2bRetailParam", transaction.Customer.CustomerId.ToString());
                                //APIAccess.APIAccessClass.userID = "";
                                APIAccess.APIAccessClass.custId = transaction.Customer.CustomerId.ToString();
                                APIAccess.APIAccessClass.isB2b = containerArray[6].ToString();
                                APIAccess.APIAccessClass.priceGroup = containerArray[4].ToString();
                                APIAccess.APIAccessClass.lineDiscGroup = containerArray[5].ToString();
                            }
                            catch (Exception ex)
                            {
                                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                                throw;
                            }
                        }
                        //if yes, query the item                          

                        if (isB2bCust == "1" || isB2bCust == "2")
                        {
                            //get customer group ppnValidation
                            ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getPPNValidate", transaction.Customer.CustomerId.ToString());
                            ppnValidate = containerArray[3].ToString();

                            custId = transaction.Customer.CustomerId;
                            //List<string> itemIds = transaction.CalculableSalesLines.Select(salesLine => salesLine.ItemId).ToList();
                            //List<string> unitIds = transaction.CalculableSalesLines.Select(salesLine => salesLine.BackofficeSalesOrderUnitOfMeasure).ToList();
                            //List<string> result = findPriceAgreement(posTransaction, transaction.ChannelId, itemIds, priceGroup);//, salesLine.BackofficeSalesOrderUnitOfMeasure);
                            //calculatePriceDiscFromTA(itemIds,transaction,priceGroup);
                            //Dictionary<string, Tuple<decimal, decimal, decimal, decimal>> priceAndDiscountMap = CalculatePriceAndDiscount(transaction, Application.Settings.Database.DataAreaID, transaction.ChannelId, itemIds, priceGroup, lineDiscGroup, unitIds);

                            if (posisOperation == PosisOperations.ProcessInput || posisOperation == PosisOperations.BlankOperation || posisOperation == PosisOperations.Customer || posisOperation == PosisOperations.CustomerSearch || posisOperation == PosisOperations.SetQty || posisOperation == PosisOperations.ConvertCustomerOrder)
                            {





                                foreach (var salesLine in transaction.CalculableSalesLines)
                                {
                                    //find the pricegroup that specified for the applied customer first yonathan 12/07/2024

                                    List<string> result = findPriceAgreement(posTransaction, transaction.ChannelId, salesLine.ItemId, transaction.Customer.CustomerId, salesLine.BackofficeSalesOrderUnitOfMeasure, salesLine.Quantity);

                                    //if not found, check the pricegroup that exist in the customers master data
                                    //calculate the pricegroup 
                                    //List<string> 
                                    if (result.Count == 0)
                                    {
                                        result = findPriceAgreement(posTransaction, transaction.ChannelId, salesLine.ItemId, priceGroup, salesLine.BackofficeSalesOrderUnitOfMeasure, salesLine.Quantity);
                                    }
                                    //calculate the discount                             
                                    //List<string> resultDisc = findDiscountAgreement(posTransaction, Application.Settings.Database.DataAreaID, salesLine.ItemId, lineDiscGroup, salesLine.BackofficeSalesOrderUnitOfMeasure);

                                    List<string> resultDisc = findDiscountAgreement(posTransaction, transaction.ChannelId, salesLine.ItemId, transaction.Customer.CustomerId, salesLine.BackofficeSalesOrderUnitOfMeasure, salesLine.Quantity);
                                    if (resultDisc.Count == 0)
                                    {
                                        resultDisc = findDiscountAgreement(posTransaction, transaction.ChannelId, salesLine.ItemId, lineDiscGroup, salesLine.BackofficeSalesOrderUnitOfMeasure, salesLine.Quantity);
                                    }
                                    //query the B2B price from TA

                                    if (result != null && result.Count > 0)
                                    {
                                        amountTA = Convert.ToDecimal(result[0]);
                                        //do the price override based on the TA
                                        salesLine.CustomerPrice = amountTA;
                                        salesLine.GrossAmount = amountTA;
                                        salesLine.OriginalPrice = amountTA;
                                        salesLine.Price = amountTA;
                                        //salesLine.PriceOverridden = true;
                                        salesLine.TradeAgreementPriceGroup = result[1];
                                        salesLine.TradeAgreementPrice = amountTA;


                                        //check taxgroup
                                        if (ppnValidate == "True")
                                        {
                                            if (taxGroup1 == "")
                                            {
                                                foundDifferentTaxGroup = false;
                                            }
                                            else if (taxGroup1 == salesLine.TaxGroupId)
                                            {
                                                foundDifferentTaxGroup = false;
                                            }
                                            else
                                            {
                                                foundDifferentTaxGroup = true;


                                            }

                                        }
                                        taxGroup1 = salesLine.TaxGroupId;
                                        //remove all discount
                                        //var lastSaleItem = transaction.SaleItems.Last.Value;



                                        foreach (var discountLine in salesLine.DiscountLines.ToList())
                                        {
                                            salesLine.DiscountLines.Remove(discountLine);
                                        }

                                        //add discount Line based on customer master
                                        LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem custDiscountManual = new LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem();

                                        //check the resultDisc count
                                        if (resultDisc != null && resultDisc.Count > 0)
                                        {
                                            custDiscountManual.Amount = Convert.ToDecimal(resultDisc[1]);
                                            custDiscountManual.Percentage = Convert.ToDecimal(resultDisc[2]);
                                            //custDiscountManual.EffectiveAmount = 
                                            //SetEffectiveAmountForAmountOff
                                            Application.Services.Discount.AddDiscountLine(salesLine, custDiscountManual);
                                        }

                                        //add discount line

                                        //if (salesLine.DiscountLines.Count == 0)
                                        ////if (transaction.RecalledOrder == true && salesLine.DiscountLines.Count == 0)
                                        //{
                                        //    //Microsoft.Dynamics.Retail.Pos.DiscountService.Discount disc = new Microsoft.Dynamics.Retail.Pos.DiscountService.Discount();
                                        //    //disc.CalcCustomerDiscount(transaction);
                                        //    //Application.Services.Discount.AddDiscountLine(IDiscountItem//.GetLineDiscountLines()
                                        //    LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem custDiscountManual = new LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem();

                                        //    //check the resultDisc count
                                        //    if (resultDisc != null && resultDisc.Count > 0)
                                        //    {
                                        //        custDiscountManual.Amount = Convert.ToDecimal(resultDisc[1]);
                                        //        custDiscountManual.Percentage = Convert.ToDecimal(resultDisc[2]);
                                        //        //custDiscountManual.EffectiveAmount = 
                                        //        //SetEffectiveAmountForAmountOff
                                        //        Application.Services.Discount.AddDiscountLine(salesLine, custDiscountManual);
                                        //    }
                                        //}
                                        ////else if (transaction.RecalledOrder == true && salesLine.DiscountLines.Count > 0)
                                        //else if (salesLine.DiscountLines.Count > 0)
                                        //{
                                        //    //remove retail discount
                                        //    var lastSaleItem = transaction.SaleItems.Last.Value;



                                        //    foreach (var discountLine in lastSaleItem.DiscountLines.ToList())
                                        //    {
                                        //        lastSaleItem.DiscountLines.Remove(discountLine);
                                        //    }

                                        //    //Microsoft.Dynamics.Retail.Pos.DiscountService.Discount disc = new Microsoft.Dynamics.Retail.Pos.DiscountService.Discount();
                                        //    //disc.CalcCustomerDiscount(transaction);
                                        //    //Application.Services.Discount.AddDiscountLine(IDiscountItem//.GetLineDiscountLines()
                                        //    LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem custDiscountManual = new LSRetailPosis.Transaction.Line.Discount.CustomerDiscountItem();

                                        //    //check the resultDisc count
                                        //    if (resultDisc != null && resultDisc.Count > 0)
                                        //    {
                                        //        custDiscountManual.Amount = Convert.ToDecimal(resultDisc[1]);
                                        //        custDiscountManual.Percentage = Convert.ToDecimal(resultDisc[2]);
                                        //        //custDiscountManual.EffectiveAmount = 
                                        //        //SetEffectiveAmountForAmountOff
                                        //        Application.Services.Discount.AddDiscountLine(salesLine, custDiscountManual);
                                        //    }
                                        //}

                                        //if it has a discount that has an offer ID
                                        //foreach (var discountLine in salesLine.DiscountLines.ToList())
                                        //{
                                        //    if (discountLine is PeriodicDiscountItem)
                                        //    {
                                        //         Convert discountLine to PeriodicDiscountItem
                                        //        PeriodicDiscountItem periodicLines = (PeriodicDiscountItem)discountLine;

                                        //         Check if the OfferId is not null or empty
                                        //        if (!string.IsNullOrEmpty(periodicLines.OfferId))
                                        //        {
                                        //             Remove the discountLine from the DiscountLines list
                                        //            salesLine.DiscountLines.Remove(discountLine);
                                        //        }
                                        //    }
                                        //}


                                        Application.Services.Tax.CalculateTax(salesLine, transaction);

                                        //POSFormsManager.ShowPOSStatusPanelText("Change price to B2B Price" );


                                    }
                                    else
                                    {
                                        string custType = isB2bCust == "1" ? "Canvas" : "B2B";
                                        if (posisOperation == PosisOperations.ProcessInput || posisOperation == PosisOperations.BlankOperation)
                                        {

                                            using (frmMessage dialog = new frmMessage(string.Format("Item {0} - {1} ini tidak bisa dijual untuk cust {2}, karena tidak ada di setup harga cust {3}", salesLine.ItemId, salesLine.Description, transaction.Customer.CustomerId, custType), MessageBoxButtons.OK, MessageBoxIcon.Error))
                                            {
                                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                                posTransaction.OperationCancelled = true;
                                                LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(string.Empty);
                                                //Application.RunOperation(PosisOperations.VoidItem, salesLine.ItemId);
                                                return;
                                                //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                                                //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                                            }
                                            //cancel the process input item
                                        }
                                        //else if (posisOperation == PosisOperations.Customer || posisOperation == PosisOperations.CustomerSearch)
                                        //{
                                        //    //APIAccess.APIAccessClass.itemToRemove.Add(salesLine.ItemId);
                                        //    //removeItems = true;                                            

                                        //using (frmMessage dialog = new frmMessage("Untuk customer B2B/Canvas, silakan lakukan langkah di bawah ini :\n1. Void transaksi ini\n2. Add customer B2B/Canvas\n3. Kemudian scan ulang barang yang dibeli", MessageBoxButtons.OK, MessageBoxIcon.Error))
                                        //{
                                        //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                        //    posTransaction.OperationCancelled = true;
                                        //    LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(string.Empty);
                                        //    //Application.RunOperation(PosisOperations.VoidItem, salesLine.ItemId);
                                        //    return;
                                        //    //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                                        //    //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                                        //}
                                        //}
                                    }
                                    indexString++;
                                }
                            }



                            //Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);
                            //Application.BusinessLogic.ItemSystem.RecalcPriceTaxDiscount(transaction, true);
                            //Application.BusinessLogic.ItemSystem.RecalcPriceTaxDiscount(transaction, false);

                            transaction.CalcTotals();
                            transaction.Save();
                        }
                    }
                }

                ////Application.Services.Discount.CalculateDiscount(transaction);
                //if (foundDifferentTaxGroup == true)
                //{
                //    using (frmMessage dialog = new frmMessage(string.Format("Untuk customer B2B, tidak boleh ada item\ndengan TaxGroup berbeda. Silakan ganti barang yang lain"), MessageBoxButtons.OK, MessageBoxIcon.Error))
                //    {
                //        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                //        posTransaction.OperationCancelled = true;
                //        LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(string.Empty);
                //        //Application.RunOperation(PosisOperations.VoidItem, salesLine.ItemId);
                //        //return;
                //        //Application.RunOperation(PosisOperations.SetQty, itemIdRemove);
                //        //Application.RunOperation(PosisOperations.SetQty,itemIdRemove, posTransaction);

                //    }
                //}


                if (removeItems == true)
                {
                    using (frmMessage dialog = new frmMessage(string.Format("Untuk customer B2B, silakan scan ulang barang yang akan dibeli karena ada perbedaan pada harga dan barang"), MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                        //BlankOperationInfo operationInfo = new BlankOperationInfo();

                        //operationInfo.OperationId = "21";
                        //operationInfo.Parameter = "TEST";
                        //Application.Services.BlankOperations.BlankOperation((IBlankOperationInfo)operationInfo, (IPosTransaction)posTransaction);
                        Application.RunOperation(PosisOperations.BlankOperation, "91", posTransaction);

                        transaction.CalcTotals();

                        transaction.Save();
                    }

                    removeItems = false;

                }

            }
            else
            {
                LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(string.Empty);
            }
            
            
            switch (posisOperation)
            {
                case PosisOperations.VoidTransaction:
                    //case PosisOperations.con
                    //case PosisOperations.q
                    {
                        APIAccess.APIAccessClass.isB2b = null;
                        APIAccess.APIAccessClass.priceGroup = null;
                        APIAccess.APIAccessClass.lineDiscGroup = null;
                        APIAccess.APIAccessClass.ppnValidation = null;
                        APIAccess.APIAccessClass.custId = null;

                        break;
                    }
            }
			 //end add

		}

        private void findPriceAgreementForCustomer(IPosTransaction posTransaction)
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, Tuple<decimal, decimal, decimal, decimal>> CalculatePriceAndDiscount(RetailTransaction transaction, string _dataAreaId, long _channelId, List<string> itemIds, string _priceGroup, string _discGroup, List<string> _unitId)//(List<string> itemIds, RetailTransaction transaction)
        {
            Dictionary<string, Tuple<decimal, decimal, decimal, decimal>> priceAndDiscountMap = new Dictionary<string, Tuple<decimal, decimal, decimal, decimal>>();
            string joinedItems = "'" + string.Join("','", itemIds) + "'";
            string joinedUnits = "'" + string.Join("','", _unitId) + "'";

            // Create a parameterized SQL query to fetch price based on itemIds
            //string priceQuery = "SELECT ItemId, Price FROM PriceTable WHERE ItemId IN (@ItemIds)";
            string priceQuery = @"SELECT
                                ta.ACCOUNTRELATION,ta.AMOUNT,ta.ITEMRELATION
                                FROM [ax].PRICEDISCTABLE ta
                                INNER JOIN [ax].RETAILCHANNELTABLE AS c
                                ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
                                LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
                                WHERE
                                ta.ITEMRELATION in (@ItemRelation)
                                AND ta.ACCOUNTRELATION = @AccountRelations                            
                                AND ta.UNITID in (@UnitId)                       
                                AND ta.FROMDATE <= @ActiveDate
                                AND ta.TODATE >= @ActiveDate                           
                
                                ORDER BY ta.QUANTITYAMOUNTFROM, ta.RECID, ta.FROMDATE";
            // Create a parameterized SQL query to fetch discount based on itemIds
            

            // Create a SqlConnection and SqlCommand for price query
            using (SqlConnection priceConnection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection)
            using (SqlCommand priceCommand = new SqlCommand(priceQuery, priceConnection))
            {
                // Add the parameter for ItemIds in price query
                //priceCommand.Parameters.AddWithValue("@ItemIds", string.Join(",", itemIds));
                priceCommand.Parameters.AddWithValue("@CHANNELID", _channelId);
                //priceCommand.Parameters.AddWithValue("@ItemRelation", string.Join(",", itemIds));
                //priceCommand.Parameters.AddWithValue("@ItemRelation", string.Join(",", itemIds));
                priceCommand.Parameters.AddWithValue("@ItemRelation", joinedItems);
                priceCommand.Parameters.AddWithValue("@AccountRelations", _priceGroup);
                priceCommand.Parameters.AddWithValue("@UnitId", string.Join(",", _unitId));
                priceCommand.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));
                // Open the price connection
                priceConnection.Open();

                string finalQuery = priceCommand.CommandText;
                foreach (SqlParameter parameter in priceCommand.Parameters)
                {
                    finalQuery = finalQuery.Replace(parameter.ParameterName, parameter.Value.ToString());
                }

                // Execute the price query and retrieve the results
                using (SqlDataReader priceReader = priceCommand.ExecuteReader())
                {
                    while (priceReader.Read())
                    {
                        //stringList.Add(reader["AMOUNT"].ToString());
                        //stringList.Add(reader["ACCOUNTRELATION"].ToString());

                        string itemId = (string)priceReader["ITEMRELATION"];
                        decimal price = (decimal)priceReader["AMOUNT"];

                        // Store the price in the dictionary using the item ID as the key
                        if (!priceAndDiscountMap.ContainsKey(itemId))
                        {
                            priceAndDiscountMap[itemId] = Tuple.Create(price, 0m,0m,0m);
                        }
                    }
                }
            }

            // Create a SqlConnection and SqlCommand for discount query
            //string discountQuery = "SELECT ItemId, Discount FROM DiscountTable WHERE ItemId IN (@ItemIds)";
            string discountQuery = @"SELECT ta.ITEMRELATION,
	                                        ta.ACCOUNTRELATION,
	                                        ta.FROMDATE,
	                                        ta.TODATE,
	                                        ta.AMOUNT,
	                                        ta.PERCENT1,
	                                        ta.PERCENT2,
	                                        ta.RELATION,
	                                        ta.DATAAREAID,
	                                        ta.UNITID
 
                                        FROM [ax].PRICEDISCTABLE ta
                                        WHERE
                                        
                                        ta.RELATION IN (5, 6, 7)
                                        AND ta.CURRENCY = 'IDR'                                          
                                        AND ((ta.FROMDATE <= @ActiveDate OR ta.FROMDATE <= @NoDate)
                                                AND (ta.TODATE >= @ActiveDate OR ta.TODATE <= @NoDate))
                                        AND ta.DATAAREAID = @DataAreaId                                        
                                        AND ta.ITEMRELATION in (@ItemRelation)
                                        AND ta.ACCOUNTRELATION = @AccountRelations
                                        AND ta.UNITID in (@UnitId)";
            using (SqlConnection discountConnection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection)
            using (SqlCommand discountCommand = new SqlCommand(discountQuery, discountConnection))
            {
                // Add the parameter for ItemIds in discount query
                //discountCommand.Parameters.AddWithValue("@ItemIds", string.Join(",", itemIds));
                discountCommand.Parameters.AddWithValue("@DataAreaId", _dataAreaId);
                discountCommand.Parameters.AddWithValue("@ItemRelation", string.Join(",", itemIds));
                discountCommand.Parameters.AddWithValue("@AccountRelations", _discGroup);
                discountCommand.Parameters.AddWithValue("@UnitId", string.Join(",", _unitId));
                discountCommand.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));
                discountCommand.Parameters.AddWithValue("@NoDate", new DateTime(1900, 01, 01));
                // Open the discount connection
                discountConnection.Open();

                // Execute the discount query and retrieve the results
                using (SqlDataReader discountReader = discountCommand.ExecuteReader())
                {
                    while (discountReader.Read())
                    {
                        string itemId = (string)discountReader["ITEMRELATION"];
                        decimal discAmount = (decimal)discountReader["AMOUNT"];
                        decimal discPct1 = (decimal)discountReader["PERCENT1"];
                        decimal discPct2 = (decimal)discountReader["PERCENT2"];
                        // Add some sample strings to the list
                        //stringList.Add(reader["ITEMRELATION"].ToString());
                        //stringList.Add(reader["AMOUNT"].ToString());
                        //stringList.Add(reader["PERCENT1"].ToString());
                        //stringList.Add(reader["PERCENT2"].ToString());
                        // Update the discount in the dictionary using the item ID as the key
                        if (priceAndDiscountMap.ContainsKey(itemId))
                        {
                            var existingPrice = priceAndDiscountMap[itemId].Item1;
                            priceAndDiscountMap[itemId] = Tuple.Create(existingPrice, discAmount,discPct1,discPct2);
                            //priceAndDiscountMap[itemId] = Tuple.Create(existingPrice, discAmount);
                        }
                    }
                }
            }

            return priceAndDiscountMap;
        }


        public List<string> findDiscountAgreement(IPosTransaction posTransaction, long _channelId, string _itemRelation, string _accountRelations, string _unitId, decimal _qty = 0, bool found = true)
            //(IPosTransaction posTransaction, string _dataAreaId, string _itemRelation, string _accountRelations, string _unitId, bool found = true)
        {
            List<string> stringList = new List<string>();
            bool isFound = found;
            string additionalQuery = "";
             

            
            SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
//                string queryString = @"SELECT ta.ITEMRELATION,
//	                                        ta.ACCOUNTRELATION,
//	                                        ta.FROMDATE,
//	                                        ta.TODATE,
//	                                        ta.AMOUNT,
//	                                        ta.PERCENT1,
//	                                        ta.PERCENT2,
//	                                        ta.RELATION,
//	                                        ta.DATAAREAID,
//	                                        ta.UNITID
// 
//                                        FROM [ax].PRICEDISCTABLE ta
//                                        WHERE
//                                        
//                                        ta.RELATION IN (5, 6, 7)
//                                        AND ta.CURRENCY = 'IDR'                                          
//                                        AND ((ta.FROMDATE <= @ActiveDate OR ta.FROMDATE <= @NoDate)
//                                                AND (ta.TODATE >= @ActiveDate OR ta.TODATE <= @NoDate)) AND ta.DATAAREAID = @DataAreaId  AND ta.ITEMRELATION = @ItemRelation AND ta.ACCOUNTRELATION = @AccountRelations  AND ta.UNITID = @UnitId ";

                string queryString = @"SELECT TOP 1 ITEMRELATION, ACCOUNTRELATION, AMOUNT,PERCENT1,PERCENT2, QUANTITYAMOUNTFROM, QUANTITYAMOUNTTO, FROMDATE,TODATE,RELATION FROM PRICEDISCTABLE TA
                            INNER JOIN [ax].RETAILCHANNELTABLE AS c
                            ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
                            LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
                            WHERE ITEMRELATION = @ItemRelation
	                            AND TA.ACCOUNTRELATION = @AccountRelations
                                AND TA.RELATION = 5
                                AND TA.UNITID = @UnitId 
                                AND (
                                (@Quantity BETWEEN QUANTITYAMOUNTFROM AND QUANTITYAMOUNTTO) OR
                                (QUANTITYAMOUNTFROM = 0 AND QUANTITYAMOUNTTO = 0)
                                )
	                            AND(@ActiveDate BETWEEN TA.FROMDATE AND TA.TODATE )
                            ORDER BY QUANTITYAMOUNTFROM DESC";
                //RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

                using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                {
                   //NEW
                    command.Parameters.AddWithValue("@CHANNELID", _channelId);
                    command.Parameters.AddWithValue("@Quantity", _qty.ToString());
                    command.Parameters.AddWithValue("@ItemRelation", _itemRelation);
                    command.Parameters.AddWithValue("@AccountRelations", _accountRelations);
                    command.Parameters.AddWithValue("@UnitId", _unitId);
                    command.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));

                    //OLD

                    //command.Parameters.AddWithValue("@DataAreaId", _dataAreaId);
                    //command.Parameters.AddWithValue("@ItemRelation", _itemRelation);
                    //command.Parameters.AddWithValue("@AccountRelations", _accountRelations);
                    //command.Parameters.AddWithValue("@UnitId", _unitId);
                    //command.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    //command.Parameters.AddWithValue("@NoDate", new DateTime(1900, 01, 01));
                    if (connectionStore.State != ConnectionState.Open)
                    {
                        connectionStore.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {


                            // Add some sample strings to the list
                            stringList.Add(reader["ITEMRELATION"].ToString());
                            stringList.Add(reader["AMOUNT"].ToString());
                            stringList.Add(reader["PERCENT1"].ToString());
                            stringList.Add(reader["PERCENT2"].ToString());


                            //dataArea = reader[0] + "";
                        }

                        //else
                        //{
                        //    stringList.Add(_itemRelation.ToString());
                        //    stringList.Add("0");
                        //    stringList.Add("0");
                        //    stringList.Add("0");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connectionStore.State != ConnectionState.Closed)
                {
                    connectionStore.Close();
                }
            }

            //if (stringList == null)
            //{
            //    isFound = false;
            //    findDiscountAgreement(posTransaction, _dataAreaId, _itemRelation, _accountRelations, _unitId, isFound);
            //}
            //else
            //{
                return stringList;
            //}
            

        }


		#endregion

        public List<string> findPriceAgreementAlt(IPosTransaction posTransaction, long _channelId, List<string> _itemRelation, string _accountRelations)//, string _unitId)
        {
            List<string> stringList = new List<string>();
            SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string queryString = @"SELECT 
                						ta.ACCOUNTRELATION,ta.AMOUNT,ta.ITEMRELATION
                						FROM [ax].PRICEDISCTABLE ta
                						INNER JOIN [ax].RETAILCHANNELTABLE AS c
                						ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
                						LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
                						WHERE
                						ta.ITEMRELATION in (@ItemRelation)
                						AND ta.ACCOUNTRELATION = @AccountRelations                            
                							                   
                						AND ta.FROMDATE <= @ActiveDate
                						AND ta.TODATE >= @ActiveDate                           
                
                						ORDER BY ta.QUANTITYAMOUNTFROM, ta.RECID, ta.FROMDATE";

                 
                //RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

                using (SqlCommand command = new SqlCommand(queryString, connectionStore))
                {
                    
                    command.Parameters.AddWithValue("@CHANNELID", _channelId);
                    command.Parameters.AddWithValue("@ItemRelation", string.Join(",", _itemRelation));
                    //command.Parameters.AddWithValue("@ItemRelation", _itemRelation);
                    command.Parameters.AddWithValue("@AccountRelations", _accountRelations);
                    //command.Parameters.AddWithValue("@UnitId", _unitId);
                    command.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));

                    if (connectionStore.State != ConnectionState.Open)
                    {
                        connectionStore.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    stringList.Add(reader["AMOUNT"].ToString());
                        //}
                        if (reader.Read())
                        {


                            // Add some sample strings to the list
                            stringList.Add(reader["AMOUNT"].ToString());
                            stringList.Add(reader["ACCOUNTRELATION"].ToString());



                            //dataArea = reader[0] + "";
                        }
                        //else
                        //{
                        //    stringList.Add("0");
                        //    stringList.Add("");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connectionStore.State != ConnectionState.Closed)
                {
                    connectionStore.Close();
                }
            }

            return stringList;
        }

        //public List<string> findPriceAgreement(IPosTransaction posTransaction, long _channelId, List<string> _itemRelation, string _accountRelations)//, string _unitId)
		
		public List<string>  findPriceAgreement(IPosTransaction posTransaction, long _channelId, string _itemRelation, string _accountRelations, string _unitId, decimal _qty = 0)
		{
			List<string> stringList = new List<string>();
            string excludeRec = "";
            string additionalQuery = "";
			SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{

//                string queryString = @"SELECT 
//							ta.ACCOUNTRELATION,ta.AMOUNT,ta.ITEMRELATION
//							FROM [ax].PRICEDISCTABLE ta
//							INNER JOIN [ax].RETAILCHANNELTABLE AS c
//							ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
//							LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
//							WHERE
//							ta.ITEMRELATION in (@ItemRelation)
//							AND ta.ACCOUNTRELATION = @AccountRelations                            
//							                   
//							AND ta.FROMDATE <= @ActiveDate
//							AND ta.TODATE >= @ActiveDate                           
//
//							ORDER BY ta.QUANTITYAMOUNTFROM, ta.RECID, ta.FROMDATE";

//                string queryString = @"SELECT 
//							ta.ACCOUNTRELATION,ta.AMOUNT,ta.ITEMRELATION
//							FROM [ax].PRICEDISCTABLE ta
//							INNER JOIN [ax].RETAILCHANNELTABLE AS c
//							ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
//							LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
//							WHERE
//							ta.ITEMRELATION = @ItemRelation
//							AND ta.ACCOUNTRELATION = @AccountRelations                            
//							AND ta.UNITID = @UnitId                          
//							AND ta.FROMDATE <= @ActiveDate
//							AND ta.TODATE >= @ActiveDate                           
//
//							ORDER BY ta.QUANTITYAMOUNTFROM, ta.RECID, ta.FROMDATE";


                string queryString = @"SELECT TOP 1 ITEMRELATION, ACCOUNTRELATION, AMOUNT, QUANTITYAMOUNTFROM, QUANTITYAMOUNTTO, FROMDATE,TODATE FROM PRICEDISCTABLE TA
                            INNER JOIN [ax].RETAILCHANNELTABLE AS c
                            ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = @CHANNELID
                            LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
                            WHERE ITEMRELATION = @ItemRelation
	                            AND TA.ACCOUNTRELATION = @AccountRelations
                                AND TA.RELATION = 4
                                AND TA.UNITID = @UnitId 
                                AND (
                                (@Quantity BETWEEN QUANTITYAMOUNTFROM AND QUANTITYAMOUNTTO) OR
                                (QUANTITYAMOUNTFROM = 0 AND QUANTITYAMOUNTTO = 0)
                                )
	                            AND(@ActiveDate BETWEEN TA.FROMDATE AND TA.TODATE )
                            ORDER BY QUANTITYAMOUNTFROM DESC";

				using (SqlCommand command = new SqlCommand(queryString, connectionStore))
				{
					/*
					@Relation = 4,
					@ItemCode = 0,
					@ItemRelation = N'11310014',
					@AccountCode = 1,
					@AccountRelations = 'HUBRETAIL',
					@UnitId = N'PC',
					@CurrencyCode = N'IDR',
					@Quantity = 0
					 */
					command.Parameters.AddWithValue("@CHANNELID", _channelId );
                    command.Parameters.AddWithValue("@Quantity", _qty.ToString());
                    command.Parameters.AddWithValue("@ItemRelation", _itemRelation );                    
					command.Parameters.AddWithValue("@AccountRelations", _accountRelations );
					command.Parameters.AddWithValue("@UnitId", _unitId );                    
					command.Parameters.AddWithValue("@ActiveDate", DateTime.Now.ToString("yyyy-MM-dd"));

					if (connectionStore.State != ConnectionState.Open)
					{
						connectionStore.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
                        //while (reader.Read())
                        //{
                        //    stringList.Add(reader["AMOUNT"].ToString());
                        //}
						if (reader.Read())
						{


							// Add some sample strings to the list
							stringList.Add(reader["AMOUNT"].ToString());
							stringList.Add(reader["ACCOUNTRELATION"].ToString());



							//dataArea = reader[0] + "";
						}
						//else
						//{
						//    stringList.Add("0");
						//    stringList.Add("");
						//}

					}
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
			finally
			{
				if (connectionStore.State != ConnectionState.Closed)
				{
					connectionStore.Close();
				}
			}

			return stringList;
		}
			
			

			/*SELECT       
			ta.ITEMRELATION,
			ta.ACCOUNTRELATION,
			ta.AMOUNT,
			ta.PRICEUNIT,
			ta.RELATION,
			ta.UNITID
			FROM [ax].PRICEDISCTABLE ta
			INNER JOIN [ax].RETAILCHANNELTABLE AS c
			ON c.INVENTLOCATIONDATAAREAID = ta.DATAAREAID AND c.RECID = 5637170828
			LEFT JOIN [ax].INVENTDIM invdim ON ta.INVENTDIMID = invdim.INVENTDIMID AND ta.DATAAREAID = c.INVENTLOCATIONDATAAREAID
			WHERE
			ta.RELATION = 4
			AND ta.ITEMCODE = 0
			AND ta.ITEMRELATION = '11310014'
			AND ta.ACCOUNTCODE = 1

			-- USES Tvp: CREATE TYPE FINDPRICEAGREEMENT_ACCOUNTRELATIONS_TABLETYPE AS TABLE(ACCOUNTRELATION nvarchar(20) NOT NULL);
			AND ta.ACCOUNTRELATION = 'HUBRETAIL'

			AND ta.CURRENCY = 'IDR'
			AND ta.UNITID = 'PC'
			AND ta.QUANTITYAMOUNTFROM <= 0
			AND (ta.QUANTITYAMOUNTTO >= 0 OR ta.QUANTITYAMOUNTTO = 0.0)
			AND ((ta.FROMDATE <= '06/21/2023' OR ta.FROMDATE <= '06/21/2023')
			AND (ta.TODATE >= '06/21/2023' OR ta.TODATE <= '06/21/2025'))
			AND (invdim.INVENTCOLORID = '' OR invdim.INVENTCOLORID = '')
			AND (invdim.INVENTSIZEID = '' OR invdim.INVENTSIZEID = '')
			AND (invdim.INVENTSTYLEID = '' OR invdim.INVENTSTYLEID = '')
			AND (invdim.CONFIGID = '' OR invdim.INVENTSTYLEID = '')

			-- ORDERBY CLAUSE MUST MATCH THAT IN AX TO ENSURE COMPATIBLE PRICING BEHAVIOR.
			-- SEE THE CLASS PRICEDISC.FINDPRICEAGREEMENT() AND TABLE PRICEDISCTABLE.PRICEDISCIDX
			ORDER BY ta.QUANTITYAMOUNTFROM, ta.RECID, ta.FROMDATE

			*/
		}
	}


	

