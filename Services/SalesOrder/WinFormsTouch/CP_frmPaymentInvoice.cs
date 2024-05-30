using LSRetailPosis.DataAccess;
using LSRetailPosis.POSControls;
using LSRetailPosis.POSControls.Touch;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
	public partial class CP_frmPaymentInvoice : frmTouchBase
	{
		
		public IApplication application;
		string custId;
		string invoiceId;
		string salesId;
		string journalNum = "";
        decimal totalInvoice = 0;
		private LSRetailPosis.POSProcesses.frmMessage refreshDialog;
		private BackgroundWorker refreshWorker;
		bool iscardbalancechecked = false;
		RetailTransaction Transaction;
		public CP_frmPaymentInvoice(IApplication _application, string _salesId, string _invoiceId, string _custAcc, decimal _totalInvoice)
		{
			InitializeComponent();
			application = _application;
			salesId = _salesId;
			invoiceId = _invoiceId;
			custId = _custAcc;
            totalInvoice = _totalInvoice;
			//ReadOnlyCollection<object> containerArray = application.TransactionServices.Invoke("getSalesInvoicesBySalesId", salesId);
			//invoiceId = getInvoiceId(containerArray[3].ToString());
			lblInvNumber.Text = _invoiceId;
            txtInvTotal.Text =  _totalInvoice.ToString("N2");
            CustomerOrderTransaction cot = SalesOrderActions.GetCustomerOrder(salesId, LSRetailPosis.Transaction.CustomerOrderType.SalesOrder, LSRetailPosis.Transaction.CustomerOrderMode.Edit);
            Transaction = cot as RetailTransaction;
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			bool retValue = false;
			string comment = "";
			decimal amountToRemove = 0;
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Cancel Posting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
				if (dialog.DialogResult == DialogResult.Yes)
				{
					foreach (DataGridViewRow row in dataGridView1.Rows)
					{
						// DataGridViewRow currentRow = dataGridView1.CurrentRow;

						// Retrieve values from desired cells in the current row
						string currVoucherNum = row.Cells[0].Value.ToString();
						// Remove the corresponding row from the DataGridView
						decimal.TryParse(row.Cells[1].Value.ToString(), out amountToRemove);


                        //application.TransactionServices.AddToGiftCard(retValue, comment, -amountToRemove, currVoucherNum, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
                        //        Transaction.TransactionId, Transaction.ReceiptId, "IDR", -amountToRemove, DateTime.Now);
                        //application.TransactionServices.GiftCardRelease(retValue, comment, currVoucherNum);
						//application.TransactionServices.VoidGiftCard(retValue, comment, currVoucherNum);
						application.TransactionServices.VoidGiftCardPayment(retValue, comment, currVoucherNum, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId);

					}
					dataGridView1.Rows.Clear();

					voucherTxtBox.Clear();
					this.Close();
				}
			}
			

		}


		public void CheckGiftCardBalance(out decimal _balance, out bool iscardbalancechecked)
		{
			_balance = 0;
			iscardbalancechecked= false;
			decimal balance;
			if (application.TransactionServices.CheckConnection())
			{
				bool succeeded = false;
				string comment = string.Empty;
				string currencycode = "IDR";// string.Empty;
				decimal giftcardbalance = 0m;

				balance = 0;

				application.TransactionServices.GetGiftCardBalance(ref succeeded, ref comment, ref currencycode, ref giftcardbalance, voucherTxtBox.Text);

				if (succeeded)
				{
					//currency = currencycode;
					_balance = giftcardbalance;
					iscardbalancechecked = true;
					
				}
				else
				{
					iscardbalancechecked = false;
					throw new Exception(comment);
					
					//
				}
			}
		}

		 
		private void postBtn_Click(object sender, EventArgs e)
		{
			using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage("Create and Post Payment Journal?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
			{
				LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
				if (dialog.DialogResult == DialogResult.Yes)
				{
					postPayment();
					settlePayment();
				}
			}
			//using (refreshDialog = new LSRetailPosis.POSProcesses.frmMessage("Posting payment", MessageBoxButtons.OK, MessageBoxIcon.Information))  //"Searching for orders..."
			//using (refreshWorker = new BackgroundWorker())
			//{
			//    //Create a background worker to fetch the data
			//    refreshWorker.DoWork += postPayment;
			//    refreshWorker.RunWorkerCompleted += refreshWorker_RunWorkerCompleted;

			//    //listen to th OnShow event of the dialog so we can kick-off the thread AFTER the dialog is visible
			//    refreshDialog.Shown += refreshDialog_Shown;
			//    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(refreshDialog);

			//    //Worker thread terminates, which causes the dialog to close, and both can safely dispose at the end of the 'using' scope.
			//}

			
		}

		public void settlePayment()//(object sender, DoWorkEventArgs e)
		{
			if (journalNum != "")
			{
				SettlePayment(journalNum, invoiceId);
			}
		}

		public void postPayment()//(object sender, DoWorkEventArgs e)
		{
			
			PostPayment(invoiceId, out journalNum);
			
		}

		void refreshDialog_Shown(object sender, EventArgs e)
		{
			// Set the wait cursor and then kick-off the async worker thread.
			refreshDialog.UseWaitCursor = true;
			refreshWorker.RunWorkerAsync();
		}

		private void RefreshGrid()
		{
			// Pop a "loading..." dialog and refresh the list contents in the background.
			
		}
		void refreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				//If there were any exceptions during the Refresh work, then throw them now.
				if (e.Error != null)
				{
					throw e.Error;
				}

			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);

				// "An error occurred while refreshing the list."
				SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56232, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				// close the "Loading..." dilaog
				refreshDialog.UseWaitCursor = false;
				refreshDialog.Close();

			   
			}
		}

		private void PostPayment(string _invoiceAx, out string _journalNum)
		{
			bool retValue = false;
			string comment = "";
			decimal amountToRemove = 0;
			_journalNum = "";
            decimal totalVoucher = 0;
            decimal amountCash = 0;
            decimal.TryParse(txtVouchTotal.Text.ToString(), out totalVoucher);


            amountCash = totalInvoice - totalVoucher;
			//var custId = gridView1.GetRowCellValue(gridView1.GetFocusedDataSourceRowIndex(), "CUSTOMERACCOUNT");
			if (application.TransactionServices.CheckConnection())
			{ 
			    try
			    {
				    object[] parameterList = new object[] 
							    {
								    amountCash,0,0,totalVoucher,0, //cash,debit,credit,voucher,other not implemented
								    salesId,
								    custId,
								    _invoiceAx,
								    DateTime.Now,
								    "",
								    ApplicationSettings.Database.StoreID.ToString()
							    };


				    ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("postSalesPayment", parameterList);
				    _journalNum = containerArray[3].ToString();
				    if (containerArray[2].ToString() == "Success")
				    {
					    SalesOrder.InternalApplication.Services.Dialog.ShowMessage(String.Format("Payment journal {0} has been created", _journalNum), MessageBoxButtons.OK, MessageBoxIcon.Information);
					    
					    		
						    foreach (DataGridViewRow row in dataGridView1.Rows)
						    {
							    // DataGridViewRow currentRow = dataGridView1.CurrentRow;
							    // Retrieve values from desired cells in the current row
							    string currVoucherNum = row.Cells[0].Value.ToString();
							    // Remove the corresponding row from the DataGridView
							    decimal.TryParse(row.Cells[1].Value.ToString(), out amountToRemove);
							    application.TransactionServices.GiftCardPayment(retValue, comment, 0, currVoucherNum, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
									    Transaction.TransactionId, Transaction.ReceiptId, "IDR", amountToRemove, DateTime.Now);
						    }
								   

					    //
				    }
				    else
				    {
					    throw new Exception("Payment error, please post Payment on AX");
				    }
			    }
			    catch (Exception ex)
			    {
				    LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				    throw;
			    }
			}  

		}

		private void SettlePayment(string _journalNum, string _invoiceNum)
		{
			if (application.TransactionServices.CheckConnection())
			{
				try
				{
					object[] parameterList = new object[] 
							{
								custId,
								_journalNum,
								_invoiceNum,                                
							};
					ReadOnlyCollection<object> containerArray = SalesOrder.InternalApplication.TransactionServices.InvokeExtension("postSettlePayment", parameterList);

					if (containerArray[2].ToString() == "1")
					{
						SalesOrder.InternalApplication.Services.Dialog.ShowMessage(string.Format("Payment has been settled \n "), MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.Close();
					}
					else
					{
						throw new Exception("Settle error, please settle payment on AX");
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
			}
		}

		private void useBtn_Click(object sender, EventArgs e)
		{
			

			bool retValue = false;
			string comment = "";
			string value = voucherTxtBox.Text;
			bool iscardbalancechecked = false;

            decimal currentVouchTotal = 0;
            decimal.TryParse(txtVouchTotal.Text,out currentVouchTotal);
            decimal remainBalance = 0;
            decimal invoiceBalance = 0;
            decimal amountToBeUsed = 0;
			decimal.TryParse(txtInvTotal.Text, out invoiceBalance);
            //Random random = new Random();
			//int minAmount = 1000;
			//int maxAmount = 1000000;
			//int range = (maxAmount - minAmount + 1) / 1000;
			decimal amount = 0;// random.Next(0, range) * 1000 + minAmount;
			string cardNumber = "";
            string currCode = "";
            
            if (txtRemain.Text == (0).ToString("N2"))
            {
                MessageBox.Show("Cannot redeem more voucher\nRemaining amount is already zero", "Infolog", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                

                this.CheckGiftCardBalance(out amount, out iscardbalancechecked);

                currentVouchTotal += amount;

                remainBalance = currentVouchTotal - invoiceBalance;
                /*
                 * voucher 1 = 100.000
                 * voucher 2 = 300.000
                 * invoice = 278.000
                 * 
                 * voucher 2 usage = (vouch 1 + vouch 2 full) - invoice = 122.000
                 * if >= 0, then voucher to be used =  vouch 2 - 122.000
                 ex : 400.000 -  278.000 = 122.000  , so the voucher amount to be used is 78.000
                 ex : 200.000 - 278.000 = -78.000, so the voucher use is  full
                 */

                if (remainBalance >= 0)
                {
                    amountToBeUsed = amount - remainBalance;
                    //currentVouchTotal - remainBalance;
                    //amountToBeUsed = remainBalance;
                }
                else
                {
                    amountToBeUsed = amount;
                }

                if (iscardbalancechecked == true && amount != 0)
                {
                    MessageBox.Show("Voucher Valid", "Infolog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TenderData tenderData = new TenderData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                    //string tenderId = tenderData.GetTenderID(ApplicationSettings.Terminal.StoreId, 214.ToString());
                    //if (tenderId == "-1")
                    //{
                    //  POSFormsManager.ShowPOSMessageDialog(4301);
                    //}
                    //else
                    //{
                    //    application.Services.GiftCard.AddToGiftCard(retailTransaction, (ITender)tenderData.GetTender(tenderId, ApplicationSettings.Terminal.StoreId));
                    //    // ISSUE: reference to a compiler-generated method

                    //}
                    /*GiftCard.InternalApplication.TransactionServices.AddToGiftCard(ref succeeded, ref comment, ref currentBalance, cardNumber,
                            ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
                            Transaction.TransactionId, Transaction.ReceiptId, this.Currency, this.ToGiftCardCurrency(amount), DateTime.Now);*/
                    if (application.TransactionServices.CheckConnection())
                    {
                        application.TransactionServices.ValidateGiftCard(ref retValue, ref comment, ref currCode, ref amount, value, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId);
                        //application.TransactionServices.AddToGiftCard(retValue, comment, amount, value, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
                        //        Transaction.TransactionId, Transaction.ReceiptId, "IDR", amount, DateTime.Now);

                    }
                    //application.Services.GiftCard.
                    // Add the value to the DataGridView
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dataGridView1, value, amountToBeUsed);
                    dataGridView1.Rows.Add(newRow);
                    dataGridView1.Columns[1].DefaultCellStyle.Format = "N2";

                    // Alternatively, set the formatting for a specific cell
                    int rowIndex = 0;
                    dataGridView1.Rows[rowIndex].Cells[1].Style.Format = "N2";

                }
                else
                {
                    MessageBox.Show("Voucher already redeemed", "Infolog", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //TenderData tenderData = new TenderData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                }
            }
            
			
			// Clear the TextBox
			voucherTxtBox.Clear();
			CalculateColumnTotal();
			//useBtn.Enabled = false;
		}

		private void validateBtn_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Voucher Valid","Infolog",MessageBoxButtons.OK,MessageBoxIcon.Information);
			useBtn.Enabled = true;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			bool retValue = false;
			string comment = "";
			decimal amountToRemove = 0;

			
			if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["removeBtn"].Index)
			{
				DataGridViewRow currentRow = dataGridView1.CurrentRow;

				// Retrieve values from desired cells in the current row
				string currVoucherNum = currentRow.Cells[0].Value.ToString();
				// Remove the corresponding row from the DataGridView
				decimal.TryParse(currentRow.Cells[1].Value.ToString(), out amountToRemove);
				if (application.TransactionServices.CheckConnection())
				{

                    //application.TransactionServices.AddToGiftCard(retValue, comment, -amountToRemove, currVoucherNum, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId, Transaction.OperatorId,
                    //        Transaction.TransactionId, Transaction.ReceiptId, "IDR", -amountToRemove, DateTime.Now);
                    //application.TransactionServices.GiftCardRelease(retValue, comment, currVoucherNum);
					//application.TransactionServices.VoidGiftCard(retValue, comment, currVoucherNum);
					application.TransactionServices.VoidGiftCardPayment(retValue, comment, currVoucherNum, ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalId);
					dataGridView1.Rows.RemoveAt(e.RowIndex);
					voucherTxtBox.Clear();
				}
			}
			CalculateColumnTotal();
		}

		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (dataGridView1.RowCount > 0)
			{
				dataGridView1.Columns["removeBtn"].Visible = true;                 
			}
			else
			{ 
				dataGridView1.Columns["removeBtn"].Visible = false;                 
			}
		}

		private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			//// Calculate the line number (1-based index)
			//string lineNumber = (e.RowIndex + 1).ToString();

			//// Get the bounds of the row header cell
			//Rectangle rowHeaderBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dataGridView1.RowHeadersWidth, e.RowBounds.Height);

			//// Center the line number vertically in the cell
			//StringFormat format = new StringFormat();
			//format.Alignment = StringAlignment.Center;
			//format.LineAlignment = StringAlignment.Center;

			//// Draw the line number on the row header cell
			//e.Graphics.DrawString(lineNumber, dataGridView1.Font, Brushes.Black, rowHeaderBounds, format);
		}

		private void CalculateColumnTotal()
		{
			decimal vouchTotal = 0;

			// Specify the column index to calculate the total
			int columnIndex = 1; // Replace with the desired column index

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (row.Cells[columnIndex].Value != null)
				{
                    decimal value;
                    if (decimal.TryParse(row.Cells[columnIndex].Value.ToString(), out value))
					{
						vouchTotal += value;
					}
				}
			}

			txtVouchTotal.Text = vouchTotal.ToString();
            txtRemain.Text = (totalInvoice - vouchTotal).ToString("N2");
		}

		private void txtTotal_TextChanged(object sender, EventArgs e)
		{
			decimal amount;
			if (decimal.TryParse(txtVouchTotal.Text, out  amount))
			{
				// Format the value with thousand separators and 2 decimal places
				string formattedAmount = amount.ToString("N2");

				// Display the formatted value in the TextBox
				txtVouchTotal.Text = formattedAmount;
			}
		}
	}
}
