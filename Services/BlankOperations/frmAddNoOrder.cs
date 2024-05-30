using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSRetailPosis.DataAccess;
using System.Data.SqlClient;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class frmAddNoOrder : Form
    {
        IPosTransaction posTransaction;

        public frmAddNoOrder(IPosTransaction _posTransaction)
        {
            
            InitializeComponent();
            posTransaction = _posTransaction;
            
        }

        private void frmAddNoOrder_Load(object sender, EventArgs e)
        {
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {

                string queryString = @" SELECT NOORDER,DATEORDER,NAMEORDER FROM [CPOrderTable]
                                        WHERE TRANSACTIONID =  @TRANSACTIONID  ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
                    
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            textBox1.Text = reader.GetString(0);
                            dateTimePicker1.Value = reader.GetDateTime(1);
                            textBox2.Text = reader.GetString(2);
                                
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

        private void button2_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please fill all the column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            else
            {
                SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                try
                {

                    //string queryString = @"  INSERT INTO CPOrderTable (TRANSACTIONID,NOORDER,DATEORDER,NAMEORDER)
                    //                        VALUES ( @TRANSACTIONID  , @NOORDER , @DATEORDER,@NAMEORDER )";
                    string queryString = @"exec CP_UpdateInsertCPOrder @TRANSACTIONID  , @NOORDER , @DATEORDER,@NAMEORDER" ;

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@TRANSACTIONID", posTransaction.TransactionId);
                        command.Parameters.AddWithValue("@NOORDER", this.textBox1.Text);
                        command.Parameters.AddWithValue("@DATEORDER", this.dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@NAMEORDER", this.textBox2.Text);

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        command.ExecuteNonQuery();
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
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
