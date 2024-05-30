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
using LSRetailPosis.Settings;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class frmShowNotification : Form
    {
        public frmShowNotification()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmShowNotification_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source= DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=DevDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";//Persist Security Info=False;User ID=USER_NAME;Password=USER_PASS;
            //string connectionString = @"Data Source= DYNAMICS16\SQLAXDB1 ;Initial Catalog=PRDDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";

            string queryString = "SELECT * FROM SALESTABLE ";
            queryString += "WHERE INVENTLOCATIONID LIKE '%" + ApplicationSettings.Database.StoreID + "%'";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                using(SqlCommand command = new SqlCommand(queryString, connection))
                {
                    if(connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        MessageBox.Show("Need to filter more");
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new Exception("SQL Connection Failed", ex);
            }
        }
    }
}
