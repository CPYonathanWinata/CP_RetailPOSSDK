using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction.Line.SaleItem;

namespace BlankOperationCashierSales
{
    public partial class frmCashierTrans : Form
    {
        DataTable dtSalesTransaction = new DataTable("SalesTable");
        IPosTransaction posTransaction;
        public frmCashierTrans(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;
        }

        private void frmCashierTrans_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getCashierTrans()
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @" SELECT * FROM RETAILTRANSACTIONTABLE
                                        WHERE STAFF = @STAFFID
                                        AND STORE = @STORE";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@STAFFID", tbStaffName.Text);
                    command.Parameters.AddWithValue("@STORE", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.Fill(dtSalesTransaction);
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

        private void button1_Click(object sender, EventArgs e)
        {
            getCashierTrans();
            dataGridView1.DataSource = dtSalesTransaction.DefaultView;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RetailTransaction retail = (RetailTransaction)posTransaction;
            MessageBox.Show(string.Format("Customer : {0} ,Total Amount {1}", retail.Customer.CustomerId, retail.NetAmountWithTax.ToString()));
        }

        private void btnSetQty_Click(object sender, EventArgs e)
        {
            RetailTransaction retail = (RetailTransaction)posTransaction;

            foreach (SaleLineItem lineItem in retail.SaleItems)
            {
                if (!lineItem.Voided)
                {
                    lineItem.Quantity = decimal.Parse(tbQty.Text);
                    lineItem.CalculateLine();
                }
            }
            retail.CalcTotals();
            retail.Save();
        }
    }
}
