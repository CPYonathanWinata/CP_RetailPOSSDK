/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;

using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Pos.Dialog;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ICustomer;
using DT = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.ITender;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using System.Net;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using CRTDM = Microsoft.Dynamics.Commerce.Runtime.DataModel;

using CRSPE = Microsoft.Dynamics.Commerce.Runtime.Services.PricingEngine;
//for BCA

using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_EngageFALWSServices;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Commerce.Runtime;
using Microsoft.Dynamics.Commerce.Runtime.Data;
using Microsoft.Dynamics.Commerce.Runtime.Services.Messages;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using LSRetailPosis.Transaction.Line;
using LSRetailPosis.Transaction.Line.Discount;



//using System.Threading.Tasks;


//using Microsoft.Dynamics.Retail.Pos.Interaction;
//using Microsoft.Practices.Prism.Interactivity.InteractionRequest;


 /* using GME_Custom;

//using Microsoft.Dynamics.Retail.Pos.Interaction;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using LSRetailPosis.Transaction;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Printing;
//using OnBarcode.Barcode;
using System;
using System.Text.RegularExpressions;
using LSRetailPosis.POSProcesses;
using GME_Custom.GME_EngageFALWSServices;
using System.Collections.Generic;
//using Jacksonsoft;
using System.Threading.Tasks;
using LSRetailPosis.Transaction.Line.SaleItem;
using System.Threading;
using LSRetailPosis.DataAccess;
using System.Linq;
 */

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{

	[Export(typeof(IBlankOperations))]
	public sealed class BlankOperations : IBlankOperations
	{
		int custNoOrder;
		public static IBlankOperationInfo globaloperationInfo;
		public static IPosTransaction globalposTransaction;
        public static IPosTransaction grabPosTransaction;
        public static string itemIdToAdd;
        public static decimal quantityToAdd;
        public static int exponent;
		//CHANGE BETWEEN DEVELOPMENT AND PRODUCTION EZEELINK
		/*
		 * ---- search for keyword production
		 * BLANKOPERATION.cs
		 * CPCHECKBALANCEEZ.cs
		 * CPRedeemEZ.cs
		 * 
		 * -- transactiontrigger.dll
		 * 
		 */

		//public const string USER_AGENT = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)" +
		//        " (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

		public const string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";


		//DEVELOPMENT CREDENTIAL
		//public const int EZ_DEVICE_ID = 57;
		//public const string EZ_DEVICE_KEY = "b41c4c2da562a283fbf0dc94184ad5792a865be1";
		//public const string EZ_MERCHANT_ID = "MD190805";
		//public const string EZ_MERCHANT_KEY = "9df983ej9fgu0w3rm89ug08r9ty8390m5nar8ytm";

		//public const string EZ_GET_TOKEN_PFM_URL = "https://pfm.cp.co.id/api/ezeelink/token/get";
		//public const string EZ_CHECK_BALANCE_URL = "https://api.ezeelink.co.id/v2/staging/EZ_GetCardPointBalance";
		//public const string EZ_GET_CARD_PROFILE_URL = "https://api.ezeelink.co.id/v2/staging/EZ_GetCardProfile";
		//public const string EZ_REQUEST_REDEEM_URL = "https://api.ezeelink.co.id/v2/staging/EZ_RequestRedeemPoint";
		//public const string EZ_REQUEST_PENDING_TRANS_STATUS_URL = "https://api.ezeelink.co.id/v2/staging/EZ_RequestPendingTransStatus";
		//public const string EZ_GET_MERCHANT_SETTING_URL = "https://api.ezeelink.co.id/v2/staging/EZ_GetMerchantSettings";
		//public const string EZ_RESENT_PUSH_NOTIF_URL = "https://api.ezeelink.co.id/v2/staging/EZ_ResentPushNotif";

		//PRODUCTION CREDENTIAL
 /*       public const int EZ_DEVICE_ID = 143;
		public const string EZ_DEVICE_KEY = "12905b460f97465880de6e7da58c20b8";
		public const string EZ_MERCHANT_ID = "65496501";
		public const string EZ_MERCHANT_KEY = "10F926CB69ED4D0893A218EBD92E4576";

 /*       public const string EZ_GET_TOKEN_PFM_URL = "https://pfm.cp.co.id/api/ezeelink/token/production/get";
		public const string EZ_CHECK_BALANCE_URL = "https://api.ezeelink.co.id/v2/apiezee/EZ_GetCardPointBalance";
		public const string EZ_GET_CARD_PROFILE_URL = "https://api.ezeelink.co.id/v2/apiezee/EZ_GetCardProfile";
		public const string EZ_REQUEST_REDEEM_URL = "https://api.ezeelink.co.id/v2/apiezee/EZ_RequestRedeemPoint";
		public const string EZ_REQUEST_PENDING_TRANS_STATUS_URL = "https://api.ezeelink.co.id/v2/apiezee/EZ_RequestPendingTransStatus";
		public const string EZ_GET_MERCHANT_SETTING_URL = "https://api.ezeelink.co.id/v2/apiezee/EZ_GetMerchantSettings";
		public const string EZ_RESENT_PUSH_NOTIF_URL = "https://api.ezeelink.co.id/v2/apiezee/EZ_ResentPushNotif";
*/
		// for Loveremit TT

		public const int LV_DEVICE_ID = 001;
		public const string LV_DEVICE_KEY = "pfm";
		public const string LV_MERCHANT_ID = "pfm";
		public const string LV_MERCHANT_KEY = "pfm";

		public const string LV_PARTNER_ID   = "PFMUSR";
		public const string LV_API_KEY      = "PFMKEY";
		public const string LV_MERCHANT_CODE = "pfm";

  /*        //DEV 
		public const string LV_PAYMENT_INQUIRY_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/DevPayment_Inquiry";
		public const string LV_PAYMENT_PROCESS_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/DevPayment_Process";

		public const string LV_TPT_INQUIRY_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/DevTPT_Inquiry";
		public const string LV_TPT_PROCESS_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/DevTPT_Process";

		public const string LV_ZREPORT_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/DevZReport";
		public const string LV_REPORT_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/DevReport";
	//  */   

		  //PROD
		public const string LV_PAYMENT_INQUIRY_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/Payment_Inquiry";
		public const string LV_PAYMENT_PROCESS_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/Payment_Process";

		public const string LV_TPT_INQUIRY_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/TPT_Inquiry";
		public const string LV_TPT_PROCESS_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/TPT_Process";

		public const string LV_ZREPORT_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/ZReport";
		public const string LV_REPORT_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/Report";
//    


		public const string LV_GET_OCCUPATION_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/Get_Occupation";
		public const string LV_GET_PURPOSE_URL = "https://partnerpfm.cp.co.id/api/LoveRemit/Get_Purpose";

	   
		[Import]

		public IApplication Application { get; set; }
		// Get all text through the Translation function in the ApplicationLocalizer
		// TextID's for BlankOperations are reserved at 50700 - 50999

		static AutoResetEvent autoEventBCA = new AutoResetEvent(false);
		private bool itemExist;
		private bool _isReceived { get; set; }
		static string _LastResponBCA { get; set; }        
		static bool isEDCSettlementBCA { get; set; }
	  

		#region IBlankOperations Members
		/// <summary>
		/// Displays an alert message according operation id passed.
		/// </summary>
		/// <param name="operationInfo"></param>
		/// <param name="posTransaction"></param>        
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
		public void BlankOperation(IBlankOperationInfo operationInfo, IPosTransaction posTransaction)
		{
			switch (operationInfo.OperationId)
			{
				case "1":
					{
						getNoOrder(posTransaction);
						break;
					}

				case "2":
					{
						frmShowRetailTrx cpShowJr = new frmShowRetailTrx(posTransaction);
						cpShowJr.ShowDialog();
						break;                        
					}

		 /*       case "3":
					{
						CPCheckBalanceEZ cpCheckBalance = new CPCheckBalanceEZ(posTransaction);
						cpCheckBalance.ShowDialog();
						break;
					}
					*/
				case "4":
					{
						CPRequestBNI cpShowReq = new CPRequestBNI(posTransaction);
						cpShowReq.ShowDialog();
						break;
					}

/*                case "5":
					{
						selectCustomerEzeelink(posTransaction);
						break;
					}

				case "6":
					{
						clearCustomerEzeelink(posTransaction);
						break;
					}

				case "7":
					{
						redeemEzeelink(posTransaction);
						break;
					} */
                case "7":
                    {//cek connection
                        #region CPLOCKTENDER
                        //need to hardcode tender id since this is custom made
                        string tenderId = "14";
                        
                        if (!validateCustomer(posTransaction, tenderId))
                        {
                            using (frmMessage dialog = new frmMessage("Please Choose Correct Customer for this Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            {
                                POSFormsManager.ShowPOSForm(dialog);
                            }
                        }
                        else
                        {
                            //get your code here
                            if (BlankOperations.ShowMsgDialog("Apakah anda mau melakukan cek koneksi BCA ?") == "OK")
                            {
                                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                                BCAOnline.checkConnectionPortBCA("COM3");
                            }
                            else
                            {
                                ShowMsgBoxInformation("batal");
                            }
                        }
                        #endregion

                        break;
                    }
				case "8":
					{
						getVoucherCode(posTransaction);
						break;
					}
				case "9":
					{
						CPLoveRemittPayment cpLoveRemittPayment = new CPLoveRemittPayment(posTransaction);
						cpLoveRemittPayment.ShowDialog();
						break;
					}
				case "10":
					{
						CPLoveRemittTPT cpLoveRemittTPT = new CPLoveRemittTPT(posTransaction);
						cpLoveRemittTPT.ShowDialog();
						break;
					}
				case "11":
					{
						CPLoveInquiry cpLoveInquiry = new CPLoveInquiry(posTransaction);
						cpLoveInquiry.ShowDialog();
						break;
					}
				case "13":
					{
				   //     Application.RunOperation(PosisOperations.PayCard, string.Empty);
					//    Application.RunOperation(PosisOperations.PayCustomerAccount, CustomerPaymentTransaction);

				   //     Application.RunOperation(PosisOperations.PayCustomerAccount,"debitBCA");

						//Add by Erwin 14 Mar 2022
						//CPWeightBarcode cpWeightBarcode = new CPWeightBarcode(posTransaction);
						//cpWeightBarcode.ShowDialog();
						cpScanBarcode(posTransaction);
						//End Add by Erwin 14 Mar 2022


						break;
					}
				case "14":
					{
						CP_DailyOnHand cpDailyOnHand = new CP_DailyOnHand(posTransaction, Application);
						cpDailyOnHand.ShowDialog();
						break;
					}
				case "15":
					{
						//add by Yonathan 26/20/2022 inherit the Set Quantity form 
						string itemId = "";
						//string input = "";
						decimal qtyInput = 0;
						string statusPanelText = "";
                        RetailTransaction retailTransaction = posTransaction as RetailTransaction;
						using (LSRetailPosis.POSProcesses.frmInputNumpad formInputQty = new LSRetailPosis.POSProcesses.frmInputNumpad())
						{

                            //check if already apply custom discount, so that the user can't edit Qty from here.
                            if (retailTransaction.Comment == "PAYMENTDISCOUNT" || retailTransaction.Comment == "PROMOED" || retailTransaction.Comment == "PROMORCPT")
                            {
                                
                                ShowMsgBox("Fungsi ini dibatasi ketika sudah apply discount PROMO. Silakan lanjut ke menu pembayaran atau batalkan sepenuhnya (void) transaksi ini");
                            }
                            else
                            {
                                //check if line selected
                                if (retailTransaction == null) //|| retailTransaction.CalculableSalesLines.Count == 0)
                                {
                                    ShowMsgBoxInformation("Set quantity - No item selected");
                                    /*using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Set quantity - No item selected", MessageBoxButtons.OK, MessageBoxIcon.Information))
                                    {
                                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

                                    }*/

                                }
                                else
                                {
                                    for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
                                    {
                                        //string thisItemId = "";
                                        LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = retailTransaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);
                                        int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;
                                        if (currentLine.LineId == operationInfo.ItemLineId && currentLine.Voided == true)
                                        {

                                            ShowMsgBoxInformation("Set quantity - No item selected");
                                            return;
                                            //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Set quantity - No item selected", MessageBoxButtons.OK, MessageBoxIcon.Information))
                                            //{
                                            //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                            //    return;
                                            //}                                       
                                        }
                                        else if (currentLine.LineId == operationInfo.ItemLineId && currentLine.Voided == false)
                                        {
                                            itemId = retailTransaction.CurrentSaleLineItem.ItemId == "" ? "" : retailTransaction.CurrentSaleLineItem.ItemId;
                                            formInputQty.Text = "Set quantity";
                                            formInputQty.NumberOfDecimals = 3;
                                            formInputQty.EntryTypes = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.Quantity;
                                            formInputQty.PromptText = "Enter the quantity";
                                            //preTrigger

                                            //LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem sl = retailTransaction.CurrentSaleLineItem;
                                            /*
                                            try
                                            {
                                                PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                                                PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PreSetQty(preTriggerResult, sl, posTransaction, 1)));


                                            }
                                            catch (Exception ex)
                                            {
                                                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                                            }
                                            */
                                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(formInputQty);
                                            if (formInputQty.InputText != "")
                                            {
                                                qtyInput = Convert.ToDecimal(formInputQty.InputText);

                                                if (qtyInput > LSRetailPosis.Settings.FunctionalityProfiles.Functions.MaximumQty)
                                                {
                                                    ShowMsgBox(string.Format("Jumlah barang yang di-input terlalu besar!!!\nJumlah maksimum {0}", LSRetailPosis.Settings.FunctionalityProfiles.Functions.MaximumQty.ToString("N2")));
                                                }
                                                else
                                                {
                                                    //sl.Quantity += qty;
                                                    currentLine.QuantityOrdered = qtyInput;
                                                    currentLine.Quantity = qtyInput;

                                                    try
                                                    {
                                                        PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                                                        PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PostSetQty(posTransaction, currentLine)));
                                                        //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 
                                                        LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusPanelText(string.Format("{0} {1}. Changed quantity", currentLine.Description, currentLine.ItemId));
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                                                    }
                                                    Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(retailTransaction);
                                                    //Application.BusinessLogic.ItemSystem.cal(retailTransaction);
                                                    //retailTransaction.calc
                                                    retailTransaction.CalcTotals();
                                                    //LSRetailPosis.POSControls.POSFormsManager.ShowPOSStatusBarText("This is statusbar text");
                                                    statusPanelText = currentLine.Description + " " + currentLine.ItemId + ". Changed quantity";
                                                }
                                                
                                            }
                                        }


                                    }
                                    /*
                                    if (retailTransaction.SaleItems.)// retailTransaction.CurrentSaleLineItem.Voided == true)
                                    {
                                        using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Set quantity - No item selected", MessageBoxButtons.OK, MessageBoxIcon.Information))
                                        {
                                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

                                        }
                                    }
                                    else
                                    {*/

                                    //}

                                }

                            }
							
						}

                        //add to check discount payment from transaction.comment
                        //if (checkDiscPayment(posTransaction) == true)
                        //{
                        //    Application.Services.Discount.AddTotalDiscountAmount(retailTransaction, 0);
                        //    Application.Services.Discount.AddTotalDiscountPercent(retailTransaction, 0);
                        //    //application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction);
                        //    Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);

                        //    //globalTransaction.CalcTotals();
                        //    retailTransaction.Comment = "";
                        //    retailTransaction.CalcTotals(); 
                        //    POSFormsManager.ShowPOSStatusPanelText("Discount payment removed");

                        //    Application.RunOperation(PosisOperations.DisplayTotal, "");
                        //    retailTransaction.CalcTotals();
                        //}

						break;
						//end add
					}
				case "16":
					{
						int row = 500;
						string stringTest = "";
						string selectedItemID = "";
						Dialog.Dialog itemDialog = new Dialog.Dialog();
						string dialogResult = itemDialog.ItemSearchCustom(row, ref stringTest);
						selectedItemID = dialogResult;
						//Application.RunOperation(PosisOperations.ItemSale, "18010000");
						APIAccess.APIAccessClass.userID = "primarti\\it.application";
						
						itemExist = false;
						//add by Yonathan to prevent error when adding the same item more than once 3 Nov 2022
						RetailTransaction retailTransaction = posTransaction as RetailTransaction;
						decimal qtyInput = 0;

						//check if line selected
						if (retailTransaction == null && selectedItemID != null) //|| retailTransaction.CalculableSalesLines.Count == 0)
						{
							Application.RunOperation(PosisOperations.ItemSale, selectedItemID);


							//this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
							////Application.RunOperation(PosisOperations.ItemSale, "123");
							//LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
							//iSale.OperationID = PosisOperations.ItemSale;
							//iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
							////iSale.Barcode = skuId; disable by Yonathan 21/10/2022
							//iSale.Barcode = itemId;//txtSKU.Text;  // change to this by yonathan 21/10/2022
							////iSale.BarcodeInfo.ItemId = txtSKU.Text;
							//iSale.OperationInfo.NumpadQuantity = 1;
							//iSale.POSTransaction = (PosTransaction)posTransaction;

							//iSale.RunOperation();
						}
						else if(retailTransaction != null && selectedItemID != null) 
						{
							for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
							{
								//string thisItemId = "";
								LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = retailTransaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);
								int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;

								if (currentLine.ItemId == selectedItemID && currentLine.Voided == false)
								{

									qtyInput = 1;
									try
									{
										PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
										PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PreSetQty(preTriggerResult, currentLine, posTransaction, i)));


									}
									catch (Exception ex)
									{
										LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
									}

									//sl.Quantity += qty;
									//sl.QuantityOrdered = qty;
									currentLine.QuantityOrdered = qtyInput;


									try
									{
										PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
										PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PostSetQty(posTransaction, currentLine)));
										currentLine.Quantity += currentLine.QuantityOrdered;

									}
									catch (Exception ex)
									{
										LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
									}
									//this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

									//sl.CalculateLine();
									itemExist = true;

									retailTransaction.CalcTotals();
								}
								
                                 

							}

							if (!itemExist)
							{
								LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
								iSale.OperationID = PosisOperations.ItemSale;
								iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
								//iSale.Barcode = skuId; disable by Yonathan 21/10/2022
								iSale.Barcode = selectedItemID; // change to this by yonathan 21/10/2022
								//iSale.BarcodeInfo.ItemId = txtSKU.Text;
								iSale.OperationInfo.NumpadQuantity = 1;
								iSale.POSTransaction = (PosTransaction)posTransaction;

								iSale.RunOperation();
							}


						}
						//end
		
						break;

					}
				case "17": //for settlement BCA 30/03/2023 Yonathan
					{
						//MessageBox.Show("Trigger 17 reprint last transaction");
						CPSettlementEDC cpSettlementEDC = new CPSettlementEDC(posTransaction, Application, "19");
						cpSettlementEDC.ShowDialog();
						break;
						
					}

				case "18": //for reprint last transaction BCA 30/03/2023 Yonathan
					{ 
						//MessageBox.Show("Trigger 18 settlement");
						CPSettlementEDC cpSettlementEDC = new CPSettlementEDC(posTransaction, Application, "10");  
						cpSettlementEDC.ShowDialog();
						break;
					}
				case "19":
					{

                        bool isCartEmpty = true;
                        bool isIntegrated = false; 
						//Validate must choose customer before proceed
						if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
						{
							DE customer = ((RetailTransaction)posTransaction).Customer;
                            RetailTransaction transaction = posTransaction as RetailTransaction;

                             
                            //string tenderId = "19"; //for DEV 
                            string tenderId = "16"; //for PROD
                            if (!validateCustomer(posTransaction, tenderId))
                            {
                                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Correct Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
								{
									LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
									return;
								}
                            }
                            else
                            {
                                isCartEmpty = checkCart(transaction);
                                if (isCartEmpty == true)
                                {
                                    isIntegrated = validateIntegrationGrabMart();
                                    if (isIntegrated == true) 
                                    {
                                        CPGrabOrder CPGrabOrder = new CPGrabOrder(posTransaction, Application);
                                        CPGrabOrder.ShowDialog();
                                    }
                                    else
                                    {
                                        using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Belum terintegrasi dengan Grabmart.\nSilakan add item order Grabmart secara manual", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                        {
                                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                            return;
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Kosongkan keranjang terlebih dahulu", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                    {
                                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                        return;
                                    }
                                }
                                
                            }
                            
						}
						else
						{
							using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
							{
								LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
								return;
							}
						}
						
						//Application.RunOperation(PosisOperations.ItemSale, "10130000");
						/*RetailTransaction transaction = posTransaction as RetailTransaction;
						LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
						iSale.OperationID = PosisOperations.ItemSale;
						iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
						//iSale.Barcode = skuId; disable by Yonathan 21/10/2022
						iSale.Barcode = "10130000";  // change to this by yonathan 21/10/2022
						//iSale.BarcodeInfo.ItemId = txtSKU.Text;
						iSale.OperationInfo.NumpadQuantity = 1;
						iSale.POSTransaction = (PosTransaction)posTransaction;

						iSale.RunOperation(); */
						/*
						PayCash pay = new PayCash(false, "1");  // 1 is the number of Payement method

						pay.OperationID = PosisOperations.PayCash; // choose ure payment method
						pay.OperationInfo = new OperationInfo();
						pay.POSTransaction = (PosTransaction)posTransaction;
						pay.Amount = transaction.NetAmountWithTax;

						pay.RunOperation();*/
						break;
					}
                case "22": //For custom discount payment. Yonathan 17/01/2024
                    {
                        RetailTransaction transaction = posTransaction as RetailTransaction;
                         
                        if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
						{
							DE customer = ((RetailTransaction)posTransaction).Customer;
                            

                            if ((customer.CustomerId != null || customer.CustomerId == "") && transaction.SaleItems.Count != 0)
                            {
                                if (customer.CustomerId != "")
                                {
                                    checkB2bCust(customer.CustomerId);
                                    string isB2bCust = "";
                                    

                                    isB2bCust = APIAccess.APIAccessClass.isB2b;

                                    if (isB2bCust == "1" || isB2bCust == "2")
                                    {
                                        using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Customer B2B atau Canvas tidak bisa mengakses menu ini", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                        {
                                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                            return;
                                        }
                                    }
                                }

                                foreach (var lineItem in transaction.SaleItems)
                                {

                                    foreach (var discountLines in lineItem.PeriodicDiscountLines)
                                    {
                                        PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;
                                        if (periodDiscItem.OfferId.StartsWith("ED") || periodDiscItem.OfferId.StartsWith("QS")) //if (periodDiscItem.OfferId.StartsWith("PDI") || periodDiscItem.OfferId.StartsWith("PDIS"))  //
                                        {
                                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Tidak bisa akses ke menu ini karena sudah mendapat diskon", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                            {
                                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                                return;
                                            }
                                        }
                                        //else
                                        //{
                                        //    promoID = periodDiscItem.OfferId;
                                        //}
                                        
                                    }

                                }

                                CPDiscountPayment cpDiscPayment = new CPDiscountPayment(posTransaction, Application);
                                cpDiscPayment.ShowDialog();
                                //Application.RunOperation(PosisOperations.DisplayTotal, "");
                                 
                            }
                            else
                            {
                                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please add item first or choose customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                {
                                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                return;
                            }
                        }
                        Application.RunOperation(PosisOperations.DisplayTotal, "");
                         
                        transaction.CalcTotals();
                        
                        transaction.Save();
                        
                        break;
                    }
                case "23": //For custom discount item. Yonathan 06/02/2024
                    {
                        RetailTransaction transaction = posTransaction as RetailTransaction;
                        string promoID = "";
                        if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
                        {
                            DE customer = ((RetailTransaction)posTransaction).Customer;

                           

                            if ((customer.CustomerId != null || customer.CustomerId == "") )//&& transaction.SaleItems.Count != 0)                             
                            //if ( transaction.SaleItems.Count != 0)
                            {
                                 //check 
                                if (customer.CustomerId != "")
                                {
                                    checkB2bCust(customer.CustomerId);
                                    string isB2bCust = "";


                                    isB2bCust = APIAccess.APIAccessClass.isB2b;

                                    if (isB2bCust == "1" || isB2bCust == "2")
                                    {
                                        using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Customer B2B atau Canvas tidak bisa mengakses menu ini", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                        {
                                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                            return;
                                        }
                                    }
                                }

                                foreach(var lineItem in transaction.SaleItems)
                                {
                                    
                                    foreach (var discountLines in lineItem.PeriodicDiscountLines)
                                    {
                                        PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;

                                        if (periodDiscItem.OfferId.StartsWith("ED")) //if (periodDiscItem.OfferId.StartsWith("PDI")) //
                                        {
                                             promoID = periodDiscItem.OfferId;
                                        }
                                        else
                                        {
                                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Tidak bisa akses ke menu ini karena sudah mendapat diskon", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                            {
                                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                                return;
                                            }
                                        }
                                       
                                    } 
                                    
                                }

                                CPDiscountItem cpDiscItem = new CPDiscountItem(posTransaction, Application, promoID, operationInfo.OperationId);


                                cpDiscItem.ShowDialog();
                                //Application.RunOperation(PosisOperations.DisplayTotal, "");

                            }
                            else
                            {
                                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please choose customer", MessageBoxButtons.OK, MessageBoxIcon.Stop)) //
                                {
                                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                    return;
                                }
                                //add item first or 
                            }
                        }
                        else
                        {
                            //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please add item first", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            //{
                            //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                            //    return;
                            //}
                        }
                        
                        Application.RunOperation(PosisOperations.DisplayTotal, "");

                        transaction.CalcTotals();

                        transaction.Save();

                        break;
                    }
                case "24": //for check stock yonathan 22/02/2024
                    {

                       
                        CheckStockForm.MainFormCheckStock checkStock = new CheckStockForm.MainFormCheckStock(posTransaction, Application);


                        checkStock.ShowDialog();
                        break;
                    }

                case "25":
                    {
                        RetailTransaction transaction = posTransaction as RetailTransaction;
                        string promoID = "";
                        if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
                        {
                            DE customer = ((RetailTransaction)posTransaction).Customer;



                            if ((customer.CustomerId != null || customer.CustomerId == "") )//&& transaction.SaleItems.Count != 0)
                            //if ( transaction.SaleItems.Count != 0)
                            {
                                //check 
                                if (customer.CustomerId != "")
                                {
                                    checkB2bCust(customer.CustomerId);
                                    string isB2bCust = "";


                                    isB2bCust = APIAccess.APIAccessClass.isB2b;

                                    if (isB2bCust == "1" || isB2bCust == "2")
                                    {
                                        using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Customer B2B atau Canvas tidak bisa mengakses menu ini", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                        {
                                            LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                            return;
                                        }
                                    }
                                }
                                foreach (var lineItem in transaction.SaleItems)
                                {

                                    foreach (var discountLines in lineItem.PeriodicDiscountLines)
                                    {
                                        PeriodicDiscountItem periodDiscItem = discountLines as PeriodicDiscountItem;
                                        //if (!periodDiscItem.OfferId.StartsWith("ED") && !periodDiscItem.OfferId.StartsWith("QS"))
                                        //{
                                        //    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Tidak bisa akses ke menu ini karena sudah mendapat diskon", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                        //    {
                                        //        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                        //        return;
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    promoID = periodDiscItem.OfferId;
                                        //}
                                        if (periodDiscItem.OfferId.StartsWith("QS")) //if (periodDiscItem.OfferId.StartsWith("PDIS")) //
                                        {
                                            promoID = periodDiscItem.OfferId;
                                        }
                                        else
                                        {
                                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Tidak bisa akses ke menu ini karena sudah mendapat diskon", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                            {
                                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                                return;
                                            }
                                        }
                                    }

                                }

                                CPDiscountItem cpDiscItem = new CPDiscountItem(posTransaction, Application, promoID, operationInfo.OperationId);


                                cpDiscItem.ShowDialog();
                                //Application.RunOperation(PosisOperations.DisplayTotal, "");

                            }
                            else
                            {
                                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please choose customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                                {
                                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            //using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please add item first", MessageBoxButtons.OK, MessageBoxIcon.Stop))
                            //{
                            //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                            //    return;
                            //}
                        }

                        Application.RunOperation(PosisOperations.DisplayTotal, "");

                        transaction.CalcTotals();

                        transaction.Save();

                        break;
                    }
                case "88":
                    {
                        break;
                    }
                case "89":
                    {
                        Application.RunOperation(PosisOperations.LoyaltyCardBalance, posTransaction);
                        break;
                    }
				case "90":
					{
                        //for testing purpose

						//addNodeTcpBinding();
						//addEndPointBinding();
						CPStressTest cpTest = new CPStressTest(Application);
						cpTest.ShowDialog();
						break;
					}
                
                case "91":
                    {
                        //CPIZone cpIzone = new CPIZone(operationInfo, posTransaction, Application); 
                        //cpIzone.ShowDialog();
                        
                        //RetailTransaction transaction = globalposTransaction as RetailTransaction;
                        ////transaction.CalcTotals();

                        ////to refresh the not updated price in the main display POS
                        ////Application.BusinessLogic.TransactionSystem.LoadTransactionStatus(transaction);
                        //Application.RunOperation(PosisOperations.DisplayTotal, "");
                        ////Application.RunOperation(PosisOperations.NoOperation, "");
                        ////posTransaction.CalcTotals();
                        ////transaction.
                        //break;

                        List<string> listitemtoremove = APIAccess.APIAccessClass.itemToRemove;
                        //list<string> itemids = transaction.calculablesaleslines.select(salesline => salesline.itemid).tolist();
                        //listitemtoremove.add("10010001");
                        //listitemtoremove.add("10010004");
                        //listitemtoremove.add("10010005");
                        int index = 0;
                        RetailTransaction transaction = posTransaction as RetailTransaction;     
                        if (listitemtoremove != null)
                        {
                            foreach (var salesline in transaction.CalculableSalesLines)
                            {
                                //list<int> numbers = new list<int> { 1, 2, 3, 4, 5 };

                                for (int i = listitemtoremove.Count - 1; i >= 0; i--)
                                {
                                    if (listitemtoremove[i] == salesline.ItemId)
                                    {

                                        salesline.Voided = true;
                                        listitemtoremove.RemoveAt(i);
                                        break;// remove element at index i.
                                    }
                                }




                            }
                        }
                            
                    
                        break;
                        
                       
                        
                        //PriceOverride priceOverride = new PriceOverride();
                        //OperationInfo opInfo = new OperationInfo();
                        //string priceToOverride = "525000";

                        //opInfo.NumpadValue = priceToOverride;
                        //opInfo.ItemLineId = transaction.SaleItems.Last.Value.LineId;

                        //priceOverride.OperationInfo = opInfo;
                        //priceOverride.OperationID = PosisOperations.PriceOverride;
                        //priceOverride.POSTransaction = transaction;
                        //priceOverride.LineId = transaction.SaleItems.Last.Value.LineId;
                        //priceOverride.RunOperation();
                    }
                case "92":
                    {
                        //Indosmart 
                        CPIZone cpIzone = new CPIZone(operationInfo, posTransaction, Application);
                        cpIzone.ShowDialog();

                        RetailTransaction transaction = globalposTransaction as RetailTransaction;
                        //transaction.CalcTotals();

                        //to refresh the not updated price in the main display POS
                        //Application.BusinessLogic.TransactionSystem.LoadTransactionStatus(transaction);
                        Application.RunOperation(PosisOperations.DisplayTotal, "");
                        //Application.RunOperation(PosisOperations.NoOperation, "");
                        //posTransaction.CalcTotals();
                        //transaction.
                        break;
                    }



                //for development purpose
                case "93":
                    {
                        int num = 0;
                        RetailTransaction retailTransaction = posTransaction as RetailTransaction;
                        foreach (var salesline in retailTransaction.CalculableSalesLines)
                        {
                            num++;
                            if( num % 2 == 0)
                            {
                                salesline.ShouldBeManuallyRemoved = true;
                                
                            }
                            
                        }
                    }
                    break;
                case "94":
                    {
                        //int exponent = 0;
                        //decimal priceAfterExponent = 0;
                        //string priceAfterExponentString = "";


                        /*
                         priceAfterExponentString = orderItem.price.ToString().Substring(0, orderItem.price.ToString().Length - exponent);

                        decimal.TryParse(priceAfterExponentString, out priceAfterExponent);
                         */
                        LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();

                        iSale = new LSRetailPosis.POSProcesses.ItemSale();
                        iSale.OperationID = PosisOperations.ItemSale;
                        iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                        iSale.Barcode = itemIdToAdd;  // change to this by yonathan 21/10/2022
                        //iSale.BarcodeInfo.ItemId = txtSKU.Text;
                        iSale.OperationInfo.NumpadQuantity = quantityToAdd;//orderItem.id
                        iSale.POSTransaction = (LSRetailPosis.Transaction.PosTransaction)posTransaction;

                        iSale.RunOperation();
                        grabPosTransaction = iSale.POSTransaction;
                    }
                    break;
                case "95":
                    {
                        PosApplication.Instance.RunOperation(PosisOperations.ItemSale, itemIdToAdd);
                        //RetailTransaction transaction = globalposTransaction as RetailTransaction;
                    }
                    break;

                case "96":
                    {
                        RetailTransaction transaction;
                        transaction = posTransaction as RetailTransaction;
                        string itemID = "10150018";


                        LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
                        iSale.OperationID = PosisOperations.ItemSale;
                        iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                        //iSale.Barcode = skuId; disable by Yonathan 21/10/2022
                        iSale.Barcode = itemID;//txtSKU.Text;  // change to this by yonathan 21/10/2022
                        //iSale.BarcodeInfo.ItemId = txtSKU.Text;
                        iSale.OperationInfo.NumpadQuantity = 1;
                        iSale.POSTransaction = (PosTransaction)posTransaction;

                        iSale.RunOperation();                        

                        //PosApplication.Instance.RunOperation(PosisOperations.PayCustomerAccount, "", (PosTransaction)transaction);

                        LSRetailPosis.Transaction.Line.Discount.LineDiscountItem lineDisc = new LSRetailPosis.Transaction.Line.Discount.LineDiscountItem();
                        lineDisc.Amount = 1000;
                        Application.Services.Discount.AddLineDiscountAmount(transaction.CurrentSaleLineItem, lineDisc);

                        Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);

                        //globalTransaction.CalcTotals();

                        transaction.CalcTotals();
                        transaction.Save();
                        POSFormsManager.ShowPOSStatusPanelText("Added item discount " + itemID);
                        

                        Application.RunOperation(PosisOperations.DisplayTotal, "");
                    }
                    break;
                case "97":
                    {
                        SaleLineItem saleLineItem;
                        RetailTransaction transaction = posTransaction as RetailTransaction;

                        decimal priceToOverride = 21300;
                        saleLineItem = RetailTransaction.SetCostPrice(transaction.SaleItems.Last.Value, priceToOverride);
                        Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);
                        transaction.CalcTotals();
                    }
                    break;
                case "98":
                    {
                        //Price Override current line without checking permission group
                        SaleLineItem saleLineItem;
                        RetailTransaction transaction = posTransaction as RetailTransaction;

                        decimal priceToOverride = 21300;


                        for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
                        {
                            //string thisItemId = "";
                            LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = transaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);
                            int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;

                            if (currentLine.LineId == operationInfo.ItemLineId && currentLine.Voided == false)
                            {
                                saleLineItem = RetailTransaction.PriceOverride(currentLine, priceToOverride);//transaction.SaleItems.Last.Value, priceToOverride);
                                Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);
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

                        //saleLineItem = RetailTransaction.PriceOverride(transaction.CurrentSaleLineItem,priceToOverride);//transaction.SaleItems.Last.Value, priceToOverride);
                        //Application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(posTransaction);
                        //transaction.CalcTotals();
                        //string str = ((IServicesV1)PosApplication.Instance.Services).Rounding.Round(((BaseSaleItem)saleLineItem).OriginalPrice, true);
                        //POSFormsManager.ShowPOSStatusPanelText(ApplicationLocalizer.Language.Translate(3352, new object[3]
                        //{
                        //    (object) ((LineItem) saleLineItem).Description,
                        //    (object) ((BaseSaleItem) saleLineItem).BarcodeId,
                        //    (object) str
                        //}));
                    }
                    break;

                case "99":
                    {
                        //Price Override with checking permission group
                        RetailTransaction transaction = posTransaction as RetailTransaction;
                        PriceOverride priceOverride = new PriceOverride();
                        OperationInfo opInfo = new OperationInfo();
                        string priceToOverride = "525000";

                        opInfo.NumpadValue = priceToOverride;
                        opInfo.ItemLineId = transaction.SaleItems.Last.Value.LineId;

                        priceOverride.OperationInfo = opInfo;
                        priceOverride.OperationID = PosisOperations.PriceOverride;
                        priceOverride.POSTransaction = transaction;
                        priceOverride.LineId = transaction.SaleItems.Last.Value.LineId;
                        priceOverride.RunOperation();




                        //  if (Math.Abs(((BaseSaleItem)saleLineItem).NetAmountWithTax) >= 1000000000000M || Math.Abs(transaction.NetAmountWithTax) >= 1000000000000M)
                        //{
                        //    using (frmMessage oForm = new frmMessage(3033, MessageBoxButtons.OK, MessageBoxIcon.Hand))
                        //        POSFormsManager.ShowPOSForm((Form)oForm);
                        //    throw new PosOperationException(3033);
                        //}
                        //  ((BaseSaleItem)saleLineItem).StandardRetailPrice = priceToOverride;
                        //// ISSUE: reference to a compiler-generated method
                        //string str = ((IServicesV1)PosApplication.Instance.Services).Rounding.Round(((BaseSaleItem)saleLineItem).OriginalPrice, true);
                        //POSFormsManager.ShowPOSStatusPanelText(ApplicationLocalizer.Language.Translate(3352, new object[3]
                        //{
                        //    (object) ((LineItem) saleLineItem).Description,
                        //    (object) ((BaseSaleItem) saleLineItem).BarcodeId,
                        //    (object) str
                        //}));

                        /*
                        foreach (var salesLine in transaction.CalculableSalesLines)
                        {
                            salesLine.CustomerPrice = 32000;
                            salesLine.GrossAmount = 32000;
                            salesLine.OriginalPrice = 32000;
                            salesLine.Price = 32000;
                            //salesLine.PriceOverridden = true;
                            salesLine.TradeAgreementPriceGroup = "HUBRETAIL";
                            salesLine.TradeAgreementPrice = 32000;
                            

                        }
                        */
                        //transaction.CalcTotals();

                        //Application.BusinessLogic.ItemSystem.CalculatePriceDiscount(posTransaction);

                        //CommerceRuntime commerceRunTime 

                        var principal = new CommercePrincipal(new CommerceIdentity(ApplicationSettings.Terminal.StorePrimaryId, new List<string>()));
                        IEnumerable<string> accountRelation = new List<string>
                        {
                            "HUBRETAIL"
                        };
                        using (CommerceRuntime commerceRuntime = CommerceRuntime.Create(ApplicationSettings.CommerceRuntimeConfiguration, principal))
                        {
                            RequestContext requestContext = commerceRuntime.CreateRequestContext(null);
                            CRTDM.ProductVariant variant = null;
                            CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                            PricingDataManager pricingManager = new PricingDataManager(requestContext);
                            ISet<string> itemIds = new HashSet<string>();

                            // Adding elements to the set
                            itemIds.Add("11310014");

                            //ReadOnlyCollection<CRTDM.TradeAgreement> TA = pricingManager.FindTradeAgreements(CRTDM.PriceDiscountType.PriceSales, CRTDM.PriceDiscountItemCode.ItemGroup, "11310014", CRTDM.PriceDiscountAccountCode.CustomerGroup, accountRelation, "PC", "IDR", 0, variant, DateTime.Now);
                            ReadOnlyCollection<CRTDM.TradeAgreement> TA = pricingManager.ReadDiscountTradeAgreements(itemIds, "C000004332", DateTimeOffset.Now, DateTimeOffset.Now, "IDR");
                            //pricingManager.FindTradeAgreements(CRTDM.PriceDiscountType.PriceSales,PriceDiscItemCode.All,"11310014",transaction.Customer,"","",0,"",DateTime.Now) ;
                            //Microsoft.Dynamics.Commerce.Runtime.Services.IService svc = commerceRuntime.GetService(ServiceTypes.PricingService);
                            //GetPriceServiceResponse response = svc.Execute<GetPriceServiceResponse>(new CalculateDiscountsServiceRequest(requestContext, crtTransaction));
                            //CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                            //CRTDM.SalesTransaction crtTransaction = CommerceRuntimeTransactionConverter.ConvertRetailTransactionToSalesTransaction(transaction);
                            //CRSPE.PricingEngine.CalculatePricesForTransaction(crtTransaction, pricingManager,)
                            //GetPriceServiceResponse response = commerceRuntime.Execute<GetPriceServiceResponse>(new CalculateDiscountsServiceRequest(crtTransaction), null);
                            //CommerceRuntimeTransactionConverter.PopulateRetailTransactionFromSalesTransaction(response.Transaction, transaction);
                        }
                        break;
                    }
                case "100":

                    bool statusTrans;
                    statusTrans = Application.TransactionServices.CheckConnection();
                    if (statusTrans == true)
                    {

                        ShowMsgBoxInformation("Koneksi lancar");
                    }
                    else
                    {
                        ShowMsgBoxInformation(statusTrans.ToString());
                    }
                    
                    break;
                case "101":
                    {

                        ReadOnlyCollection<object> containerArray;
                        string fromDate = "21/06/2024 00:00:00";
                        string toDate = "21/06/2024 16:00:00";
                        containerArray = Application.TransactionServices.InvokeExtension("getSalesOrderSummary", "JKT", "WH_JDELIMA", fromDate,toDate);
                        //MessageBox.Show(string.Format("{0} - {1}",DateTime.Now.ToString(), containerArray[3].ToString()));
                    }
                    break;
                //Application.RunOperation(PosisOperations.PayCard, string.Empty, posTransaction);
                //CP_SalesOrderDetail cpSalesDetail = new CP_SalesOrderDetail(Application);
                //cpSalesDetail.ShowDialog();
                //break;
                /*
                 try
                {
                    ReadOnlyCollection<object> containerArray;
                    containerArray = Application.TransactionServices.InvokeExtension("myTestMethod", "ThisTest");

                    bool retValue = (bool)containerArray[1];
                    string errorMessage = containerArray[2].ToString();
                    string payout = containerArray[3].ToString();
                    string comment = string.Empty;

                    if (retValue)
                    {
                        comment = string.Format("Call succeeded.  Payout:   {0}", payout);
                    }
                    else
                    {
                        comment = string.Format("Call failed.  Error Message:     {0}", errorMessage);
                    }

                    using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(comment.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    }

                }
                catch (System.Exception ex)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    throw;
                }*/
                //break;
					   

			}

		}

        private void checkB2bCust(string _custId)
        {
             
                string isB2bCust = "";
                if (Application.TransactionServices.CheckConnection())
                {
                    try
                    {
                        ReadOnlyCollection<object> containerArray = Application.TransactionServices.InvokeExtension("getB2bRetailParam", _custId);

                        APIAccess.APIAccessClass.isB2b = containerArray[6].ToString();

                    }
                    catch (Exception ex)
                    {
                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        throw;
                    }
                }

                 

           
        }

        private bool checkDiscPayment(IPosTransaction posTransaction)
        {
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            bool isPaymDisc = false;
            try
            {
                string queryString = @"SELECT [PROMOID]  FROM [ax].[CPPROMOCASHBACK] WHERE PROMOID = @PromoId";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@PromoId", retailTransaction.Comment);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                isPaymDisc = true;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }

            return isPaymDisc;
        }

        private bool checkCart(RetailTransaction _transaction)
        {
            if(_transaction.CalculableSalesLines.Count != 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

		

		
		#endregion

		#region CPLOCKTENDER
        //function to validate integration
        private bool validateIntegrationGrabMart()
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            string tenderName = "%GRABMART%";
            try
            {
                string queryStringID = @"SELECT 
	                                        TENDERTYPENAME,
	                                        ISINTEGRATION 
                                        FROM ax.CPEPAYMAPPING
                                        WHERE 
											 
		                                TENDERTYPENAME LIKE @TENDERNAME
										AND STORENUMBER = @STORENUMBER";

                
                using (SqlCommand command = new SqlCommand(queryStringID, connection))
                {
                    //command.Parameters.AddWithValue("@CUSTOMERID", this.customerID);
                    command.Parameters.AddWithValue("@TENDERNAME", tenderName);
                    command.Parameters.AddWithValue("@STORENUMBER", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);
                    
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Convert.ToString(reader["ISINTEGRATION"]) == "1")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                            //if (timeClockType == TimeClockType.BreakFlowStart)
                            //{
                            //    this.BreakActivity = Convert.ToString(reader["ACTIVITY"]);
                            //}
                            //else
                            //{
                            //    this.JobId = Convert.ToString(reader["JOBID"]);
                            //}
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return false;
        }
		//function to validate customer based on tender
		private bool validateCustomer(IPosTransaction posTransaction, string tenderId)
		{
			//Validate must choose customer before proceed
			if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{
				DE customer = ((RetailTransaction)posTransaction).Customer;

				if (!String.IsNullOrEmpty(customer.CustomerId))
				{
					SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
					try
					{
						string queryStringID = @"SELECT 
											CUSTACCOUNT 
										FROM ax.CPCUSTTENDERMAPTABLE
										WHERE 
											CUSTACCOUNT = @CUSTOMERID
											AND TENDERTYPEID = @TENDERTYPEID
											AND DATAAREAID = @DATAAREAID";

						RetailTransaction retailTransaction = (RetailTransaction)posTransaction;

						using (SqlCommand command = new SqlCommand(queryStringID, connection))
						{
							command.Parameters.AddWithValue("@CUSTOMERID", customer.CustomerId);
							command.Parameters.AddWithValue("@TENDERTYPEID", tenderId);
							command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

							if (connection.State != ConnectionState.Open)
							{
								connection.Open();
							}
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									return true;
								}
							}
						}
					}
					catch (Exception ex)
					{
						LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					}
					finally
					{
						if (connection.State != ConnectionState.Closed)
						{
							connection.Close();
						}
					}
				}
			}
			
			return false;
		}

		#endregion
		

		private void getCustAddNoOrder(string custId)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{

				string queryString = @" SELECT CPNoOrder FROM [ax].[CUSTTABLE]
										WHERE ACCOUNTNUM =  @CUSTID
										AND CPNoOrder = 1";

				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@CUSTID", custId);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							custNoOrder = reader.GetInt32(0);
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
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}

		}
		/****/
/****/

		private void getNoOrder(IPosTransaction posTransaction)
		{
			if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{
				DE customer = ((RetailTransaction)posTransaction).Customer;
				custNoOrder = 0;

				if (!String.IsNullOrEmpty(customer.CustomerId))
				{
					this.getCustAddNoOrder(customer.CustomerId);
					if (custNoOrder.Equals(1))
					{
						frmAddNoOrder addNoOrderForm = new frmAddNoOrder(posTransaction);
						addNoOrderForm.ShowDialog();
					}
					else
					{
						MessageBox.Show("For this customer no need to Add No Order");
					}
				}
				else
				{
					MessageBox.Show("For this customer no need to Add No Order");
				}
			}
			else
			{
				MessageBox.Show("For this customer no need to Add No Order");
			}
		}

   /*     private void selectCustomerEzeelink(IPosTransaction posTransaction)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

			try
			{
				string queryString = "SELECT TOP 1 TRANSACTIONID FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

				using (SqlCommand cmd = new SqlCommand(queryString, connection))
				{
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							MessageBox.Show("Please clear current customer");
						}
						else
						{
							reader.Close();
							reader.Dispose();

							CPSearchCustomerEZ cpSearchCustEZ = new CPSearchCustomerEZ(posTransaction);
							cpSearchCustEZ.ShowDialog();
						}
					}
				}
			}
			catch (SqlException ex)
			{
				throw new Exception("Format Error", ex);
			}
			finally
			{
				connection.Close();
			}
		} */

	/*    private void clearCustomerEzeelink(IPosTransaction posTransaction)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

			try
			{
				string queryString = "SELECT TOP 1 TRANSACTIONID FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

				using (SqlCommand cmd = new SqlCommand(queryString, connection))
				{
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (!reader.Read())
						{
							MessageBox.Show("There is no customer");
						}
						else
						{
							if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
							{
								if(((RetailTransaction)posTransaction).TenderLines.Count > 0)
								{
									MessageBox.Show("Cannot clear customer in this transaction");
									return;
								}
							}
							

							if (MessageBox.Show("Are you sure you want to clear the customer from the transaction?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
							{
								reader.Close();
								reader.Dispose();
								cmd.Dispose();

								string queryDelete = "DELETE FROM CPEZSELECTEDCUSTOMER";
								using (SqlCommand cmdDelete = new SqlCommand(queryDelete, connection))
								{
									if (connection.State != ConnectionState.Open)
									{
										connection.Open();
									}

									cmdDelete.ExecuteNonQuery();
								}

							}
						}
					}
				}
			}
			catch (SqlException ex)
			{
				throw new Exception("Format Error", ex);
			}
			finally
			{
				connection.Close();
			}
		}
	 * */
		public class grabMartProperties
		{
			public static bool grabMartStatus = false;
		}


		public static bool CheckForInternetConnection()
		{
			try
			{
				using (var client = new WebClient())
				using (client.OpenRead("https://pfm.cp.co.id/api/connection/check"))
					return true;
			}
			catch
			{
				return false;
			}
		}
/*
		private int duplicateRedeem(IPosTransaction posTransaction)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{
				string queryString = "SELECT TRANSACTIONID FROM AX.CPEZREDEEM WHERE TRANSACTIONID = @TRANSACTIONID ";

				using (SqlCommand cmd = new SqlCommand(queryString, connection))
				{
					cmd.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Cannot redeem more than 1 time per transaction", MessageBoxButtons.OK, MessageBoxIcon.Error))
							{
								LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
								return 1;
							}
						}
					}
				}
			}
			catch (SqlException ex)
			{
				MessageBox.Show("No Internet Connection, Current transaction will not earn point. (10)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				//LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
			}
			finally
			{
				connection.Close();
			}

			return 0;
		}

		private void redeemEzeelink(IPosTransaction posTransaction)
		{
			if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{
				int totalItem = ((RetailTransaction)posTransaction).SaleItems.Count;

				if (totalItem < 1)
				{
					using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("There are no items to pay for", MessageBoxButtons.OK, MessageBoxIcon.Error))
					{
						LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
						return;
					}
				}
				else
				{
					//validate ITEM TOP UP BNI
					if (!validateTopUpBNI(posTransaction))
					{
						using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("PAYMENT METHOD MUST BE CASH FOR TOP UP", MessageBoxButtons.OK, MessageBoxIcon.Error))
						{
							LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
							return;
						}
					}
					//end validate
					SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

					try
					{
						string queryString = "SELECT TOP 1 TRANSACTIONID, CARDNUMBER FROM CPEZSELECTEDCUSTOMER ORDER BY TRANSACTIONID DESC";

						using (SqlCommand cmd = new SqlCommand(queryString, connection))
						{
							if (connection.State != ConnectionState.Open)
							{
								connection.Open();
							}

							using (SqlDataReader reader = cmd.ExecuteReader())
							{
								if (reader.Read())
								{
									if (!CheckForInternetConnection())
									{
										using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Cannot redeem, no internet connection", MessageBoxButtons.OK, MessageBoxIcon.Error))
										{
											LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
											return;
										}
									}
									else
									{
										if (reader["TRANSACTIONID"].ToString() == posTransaction.TransactionId)
										{
											string tempCardNumber = reader["CARDNUMBER"].ToString();
											reader.Close();
											reader.Dispose();
											cmd.Dispose();
											connection.Close();
											connection.Dispose();

											if(duplicateRedeem(posTransaction) != 1)
											{
												CPRedeemEZ cpRedeem = new CPRedeemEZ(posTransaction, tempCardNumber);
												cpRedeem.ShowDialog();
											}
										}
										else
										{
											using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Something went wrong, Please void transaction and try again", MessageBoxButtons.OK, MessageBoxIcon.Error))
											{
												LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
												return;
											}
										}
									}
								}
								else
								{
									using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please select customer ezeelink to continue this operation", MessageBoxButtons.OK, MessageBoxIcon.Error))
									{
										LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
										return;
									}
								}
							}
						}
					}
					catch (SqlException ex)
					{
						throw new Exception("Format Error", ex);
					}
					finally
					{
						if (connection != null)
							connection.Dispose();
					}
					
				}
			}
			else
			{
				using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("There are no items to pay for", MessageBoxButtons.OK, MessageBoxIcon.Error))
				{
					LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
					return;
				}
			}
		}
 * */

		private bool validateTopUpBNI(IPosTransaction posTransaction)
		{
			string listItem = "";
			int totalItem = 0;
			var dict = new Dictionary<string, int>();

			listItem += "(";

			for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
			{
				if (totalItem != 0 && !((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Voided && ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId != "")
					listItem += ", ";

				if (!((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Voided && ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId != "")
				{
					listItem += "'" + ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId + "'";
					dict[((RetailTransaction)posTransaction).SaleItems.ElementAt(i).ItemId] = (int)((RetailTransaction)posTransaction).SaleItems.ElementAt(i).Quantity;
					totalItem++;
				}
			}

			listItem += ")";

			if (totalItem != 0)
			{
				SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{

					string queryString = @" SELECT MAX(ID1) AS ID1, MAX(ID2) AS ID2, SUM(TOTAL_ITEM1) AS TOTAL_ITEM1, SUM(TOTAL_ITEM2) AS TOTAL_ITEM2, SUM(EDC1) AS EDC1, SUM(EDC2) AS EDC2
										FROM
										(
											SELECT ITEMID ID1, '' ID2, SUM(1) TOTAL_ITEM1, 0 TOTAL_ITEM2, SUM(EDCTYPE) EDC1, 0 EDC2
											FROM [AX].[CPITEMTOPUP] WHERE ITEMID IN " + listItem + @"
											GROUP BY ITEMID
	
											UNION

											SELECT '' ID1, ITEMID ID2, 0 TOTAL_ITEM1, SUM(1) TOTAL_ITEM2, 0 EDC1, SUM(EDCTYPE) EDC2
											FROM [AX].[CPITEMTOPUPFEE] WHERE ITEMID IN " + listItem + @"
											GROUP BY ITEMID
										) AS TOPUP
										";

					using (SqlCommand command = new SqlCommand(queryString, connection))
					{
						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								return false;
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
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}

				}
			}
			return true;
		}

		private int getCustVoucherCode(string custID)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			int returnValue = 0;

			try
			{

				string queryString = @" SELECT ISVOUCHERCODEREQUIRED FROM [ax].[CPCUSTTABLEVOUCHER]
										WHERE ACCOUNTNUM =  @CUSTID
										AND ISVOUCHERCODEREQUIRED = 1";

				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@CUSTID", custID);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							returnValue = 1;
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
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}

			return returnValue;
		}

	  

		public static void ShowMsgBox(string text)
		{
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
			}

		}

		public static void ShowMsgBoxInformation(string text)
		{
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
			}
		}

		public static string ShowMsgDialog(string message)
		{
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

				if (dialog.DialogResult == DialogResult.Yes)
				{
					return "OK";
				}
				else
				{
					return "NO";
				}
			}
		}

		private void getVoucherCode(IPosTransaction posTransaction)
		{
			if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{
				//VALIDATE IF ROW EXISTS IN CPEXTVOUCHERCODETMP
				SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

				try
				{
					string queryString = @"SELECT TOP 1 VOUCHERCODE FROM CPEXTVOUCHERCODETMP";

					using (SqlCommand cmd = new SqlCommand(queryString, connection))
					{
						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Voucher Code already Applied, finish current transaction to add new Voucher", MessageBoxButtons.OK, MessageBoxIcon.Stop))
								{
									LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
									reader.Close();
									reader.Dispose();
									cmd.Dispose();
									connection.Close();
									connection.Dispose();

									return;
								}
							}
						}
					}
				}
				catch (SqlException ex)
				{
					throw new Exception("Format Error", ex);
				}
				finally
				{
					connection.Close();
				}

				DE customer = ((RetailTransaction)posTransaction).Customer;
				custNoOrder = 0;

				if (!String.IsNullOrEmpty(customer.CustomerId))
				{
					if (this.getCustVoucherCode(customer.CustomerId) == 1)
					{
                        CPVoucherCode cpVoucher = new CPVoucherCode(posTransaction, Application);
						cpVoucher.ShowDialog();
					}
					else
					{
						using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("For this customer no need to Add Voucher Code", MessageBoxButtons.OK, MessageBoxIcon.Stop))
						{
							LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
							return;
						}
					}
				}
				else
				{
					using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
					{
						LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
						return;
					}
				}
			}
			else
			{
				using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
					return;
				}
			}
		}

		private void cpScanBarcode(IPosTransaction posTransaction)
		{
			//Validate must choose customer before proceed
			if (posTransaction.ToString() == "LSRetailPosis.Transaction.RetailTransaction")
			{
				DE customer = ((RetailTransaction)posTransaction).Customer;

				if (!String.IsNullOrEmpty(customer.CustomerId))
				{
					//allowed to proceed
					CPWeightBarcode cpWeightBarcode = new CPWeightBarcode(posTransaction);
					cpWeightBarcode.ShowDialog();
				}
				else
				{
					using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
					{
						LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
						return;
					}
				}
			}
			else
			{
				using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Please Choose Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
					return;
				}
			}
		}
	}
}
