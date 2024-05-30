using LSRetailPosis.POSControls.Touch;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.InfocodeItem;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
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

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPDiscountPayment :  LSRetailPosis.POSControls.Touch.frmTouchBase
    {
        IPosTransaction posTransaction;
        IApplication application;
        //private string connectionString = "YourConnectionString";
        //SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
        public CPDiscountPayment(IPosTransaction _posTransaction, IApplication _application)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
            application = _application;
            LoadData();
            this.dataGridResult.CellContentClick += dataGridResult_CellContentClick;
        }

        //public class InfoCodeLineItemV1 : IInfoCodeLineItemV1
        //{
        //    public int AdditionalCheck { get; set; }
        //    public decimal Amount { get; set; }
        //    public string InfocodeId { get; set; }
        //    public string Information { get; set; }
        //    public bool InputRequired { get; set; }
        //    // ... other properties and methods from the interface
        //}

        // Usage
        

        private void dataGridResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            decimal maxDisc = 0;
            decimal pctDisc = 0;
            decimal amtDisc = 0;
            decimal minPaymAmt = 0;
            decimal amountPct = 0;
            string message = "";
            RetailTransaction globalTransaction;
            RetailTransaction transaction = posTransaction as RetailTransaction;
            maxDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Max Disc"].Value);
            pctDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Disc. Percentage"].Value);
            amtDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Disc. Amount"].Value);
            minPaymAmt = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["Min. Payment"].Value);

            if (transaction.NetAmountWithTax >= minPaymAmt)
            {
                //application.RunOperation(PosisOperations.TotalDiscountAmount, totalDisc);
                //totalDisc = Convert.ToDecimal(dataGridResult.Rows[e.RowIndex].Cells["MAXCASHBACKPCTAMOUNT"].Value);
                //application.RunOperation(PosisOperations.TotalDiscountAmount, totalDisc);

                //saleLineItem = RetailTransaction.`(currentLine, priceToOverride);//transaction.SaleItems.Last.Value, priceToOverride);

                //globalTransaction = BlankOperations.globalposTransaction as RetailTransaction;

               

                //add discount based on the scheme
                //check discount scheme
                if (pctDisc != 0)
                {

                    
                    amountPct = (pctDisc / 100) * transaction.NetAmountWithTax;
                    //check if not more than maxAmount

                    if (maxDisc == 0) // no limit amount for pct discount
                    {
                        transaction.SetTotalDiscPercent(pctDisc);
                        transaction.Comment = dataGridResult.Rows[e.RowIndex].Cells["Disc Code"].Value.ToString();
                        try
                        {
                            PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                            PosApplication.Instance.Triggers.Invoke<IDiscountTrigger>((Action<IDiscountTrigger>)(t => t.PostTotalDiscountPercent(posTransaction)));
                            //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 

                        }
                        catch (Exception ex)
                        {
                            LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                        }
                    }
                    else
                    {
                        if(amountPct >= maxDisc)
                        {
                            transaction.SetTotalDiscAmount(maxDisc);
                            transaction.Comment =  dataGridResult.Rows[e.RowIndex].Cells["Disc Code"].Value.ToString();
                            try
                            {
                                PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                                PosApplication.Instance.Triggers.Invoke<IDiscountTrigger>((Action<IDiscountTrigger>)(t => t.PostTotalDiscountAmount(posTransaction)));
                                //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 

                            }
                            catch (Exception ex)
                            {
                                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                            }

                        }
                        else
                        {
                            transaction.SetTotalDiscPercent(pctDisc);
                            transaction.Comment =  dataGridResult.Rows[e.RowIndex].Cells["Disc Code"].Value.ToString();
                            try
                            {
                                PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                                PosApplication.Instance.Triggers.Invoke<IDiscountTrigger>((Action<IDiscountTrigger>)(t => t.PostTotalDiscountPercent(posTransaction)));
                                //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 

                            }
                            catch (Exception ex)
                            {
                                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                            }
                        } 
                    }
                    
                }
                else if (amtDisc != 0)
                {
                    transaction.SetTotalDiscAmount(amtDisc);
                    transaction.Comment =   dataGridResult.Rows[e.RowIndex].Cells["Disc Code"].Value.ToString();
                    try
                    {
                        PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
                        PosApplication.Instance.Triggers.Invoke<IDiscountTrigger>((Action<IDiscountTrigger>)(t => t.PostTotalDiscountAmount(posTransaction)));
                        //LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(2611); 

                    }
                    catch (Exception ex)
                    {
                        LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    }
                }
                //application.Services.Discount.AddTotalDiscountAmount(transaction, totalDisc);
                //application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(BlankOperations.globalposTransaction);
                application.BusinessLogic.ItemSystem.CalculatePriceTaxDiscount(transaction);
                 
                //globalTransaction.CalcTotals();
                
                transaction.CalcTotals();
                POSFormsManager.ShowPOSStatusPanelText("Added payment discount " + dataGridResult.Rows[e.RowIndex].Cells["Promo Name"].Value.ToString());
                closeBtn.PerformClick();

                application.RunOperation(PosisOperations.DisplayTotal, "");


                
            }
            else
            {
                message = "Total minimal belanja belum memenuhi syarat untuk menggunakan promo ini.\nTambah lagi " + (minPaymAmt - transaction.NetAmountWithTax).ToString("N2");
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                    return;
                }
            }
        }

        private void LoadData()
        { 
            RetailTransaction retailTransaction = posTransaction as RetailTransaction;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

            try
            {
                string queryString = @"SELECT [PROMOID] as [Disc Code]
                                            , [PROMONAME] as [Promo Name]
                                            , [CUSTACCOUNT] as [Customer] 
                                            , [TENDERTYPENAME] as [Payment Method]
                                            , [MINPAYMENTAMOUNT] as [Min. Payment]
                                            , [PROMOPERCENTAGE] as [Disc. Percentage]                                            
                                            , [CASHBACKDISCOUNT] as [Disc. Amount]
                                            , [MAXCASHBACKPCTAMOUNT] as [Max Disc]
                                            , [FROMDATE] as [From Date]
                                            , [TODATE] as [To Date]
                                            FROM [ax].[CPPROMOCASHBACK] WHERE CUSTACCOUNT = @CUSTACC AND ISACTIVE = 1 AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date)";
                //string queryString = @"SELECT ITEMID,POSITIVESTATUS,DATAAREAID FROM ax.CPITEMONHANDSTATUS where ITEMID=@ITEMID";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {

                    command.Parameters.AddWithValue("@CUSTACC", retailTransaction.Customer.CustomerId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();

                    } 
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the DataGridView
                    dataGridResult.DataSource = dataTable;

                    // Add a ButtonField to the GridView
                    DataGridViewButtonColumn btnSelectCol = new DataGridViewButtonColumn();
                    btnSelectCol.HeaderText = "";
                    btnSelectCol.Name = "btnSelect";
                    btnSelectCol.ReadOnly = true;
                    btnSelectCol.Text = "Select";
                    btnSelectCol.UseColumnTextForButtonValue = true;
                    dataGridResult.Columns.Add(btnSelectCol);
                    // Bind the GridView data after adding the column

                    gridStyle();
                    
                    
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
        }

        private void gridStyle()
        {
            dataGridResult.Columns[0].Width = 80;
            dataGridResult.Columns[1].Width = 180;
            dataGridResult.Columns[2].Width = 70;
            dataGridResult.Columns[3].Width = 70;
            dataGridResult.Columns[4].Width = 80;
            dataGridResult.Columns[5].Width = 70;
            dataGridResult.Columns[6].Width = 80;
            dataGridResult.Columns[7].Width = 80;
            dataGridResult.Columns[8].Width = 70;
            dataGridResult.Columns[9].Width = 70;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
