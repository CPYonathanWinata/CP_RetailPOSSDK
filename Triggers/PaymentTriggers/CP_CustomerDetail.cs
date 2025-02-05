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

namespace PaymentTriggers
{

    public partial class CP_CustomerDetail : Form
    {
        //APIAccess.APIParameter.RetailTransactionExtended retailExtended = new APIAccess.APIParameter.RetailTransactionExtended();
        public CP_CustomerDetail()
        {
            InitializeComponent();
            LoadComboBox(natComboBox);
            errLbl1.Text = "";
            errLbl1.ForeColor = Color.Red;
            errLbl2.Text = "";
            errLbl2.ForeColor = Color.Red;
            errLbl3.Text = "";
            errLbl3.ForeColor = Color.Red;
            errLbl4.Text = "";
            errLbl4.ForeColor = Color.Red;
            errLbl5.Text = "";
            errLbl5.ForeColor = Color.Red;
            errLbl6.Text = "";
            errLbl6.ForeColor = Color.Red;

            if (APIAccess.APIAccessClass.idTypeBox != "" && APIAccess.APIAccessClass.idTypeBox != null)
            {
                idTypeBox.SelectedIndex = Convert.ToInt16(APIAccess.APIAccessClass.idTypeBox);
                idText.Text = APIAccess.APIAccessClass.idText;
                custText.Text = APIAccess.APIAccessClass.custText;
                genderBox.SelectedIndex = Convert.ToInt16(APIAccess.APIAccessClass.genderBox);
                ageText.Text = APIAccess.APIAccessClass.ageText.ToString();
                natComboBox.SelectedIndex =  APIAccess.APIAccessClass.nationalityIndex;

            }
            else
            {
                idTypeBox.SelectedIndex = -1;
                idText.Text = "";
                custText.Text = "";
                genderBox.SelectedIndex = -1;
                ageText.Text = "";
                natComboBox.SelectedIndex = -1;
            }
            
            //check the existing transactionID data on table, if exists, load the data based on transactionID

        }

        public void LoadComboBox(ComboBox comboBox)
        {
            // Your connection string
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
        
            // SQL query
            string query = @"
                SELECT DISTINCT LANGUAGEID, LONGNAME
                FROM AX.[LOGISTICSADDRESSCOUNTRYREGIONTRANSLATION]
                WHERE LANGUAGEID = 'EN-US'";

            try
            {
                  
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                       
                        connection.Open();
 
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            natComboBox.Items.Clear();

                            
                            while (reader.Read())
                            {
                                // Add the LONGNAME column value to the ComboBox
                                natComboBox.Items.Add(reader["LONGNAME"].ToString());
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

        private void okBtn_Click(object sender, EventArgs e)
        {
            //check all the texbox            
            bool checkAll = false;
            bool foundEmpty = false;
            if (idTypeBox.SelectedIndex == -1)
            {
                errLbl1.Text = "Harus diisi";
                foundEmpty = true;
            }
            else
            {
                errLbl1.Text = "";
            }

            //idTypeBox
            if(idText.Text == "")
            {
                errLbl2.Text = "Harus diisi";
                foundEmpty = true;
            }
            else
            {
                errLbl2.Text = "";
            }

            if(custText.Text =="")
            {
                errLbl3.Text = "Harus diisi";
                foundEmpty = true;
            }
            else
            {
                errLbl3.Text = "";
            }

            if (genderBox.SelectedIndex == -1)
            {
                errLbl4.Text = "Harus diisi";
                foundEmpty = true;
                //checkAll == true;
            }
            else
            {
                errLbl4.Text = "";
            }
            //genderBox
            if(ageText.Text =="")
            {
                errLbl5.Text = "Harus diisi";
                foundEmpty = true;
            }
            else
            {
                errLbl5.Text = "";
            }

            //genderBox
            if (natComboBox.SelectedIndex == -1)
            {
                errLbl6.Text = "Harus diisi";
                foundEmpty = true;
            }
            else
            {
                errLbl6.Text = "";
            }

            if (foundEmpty  == false)
            {
                //input to CPRETAILTRANSACTIONTABLEEXT

                writeToTemp(idTypeBox.SelectedIndex.ToString(), idText.Text, custText.Text, genderBox.SelectedIndex.ToString(), ageText.Text, natComboBox.SelectedIndex, natComboBox.SelectedItem.ToString());
                //

                this.Close();
        
            }
        }

        private void writeToTemp(string idType, string idText, string custText, string gender, string age, int nationalityIndex, string nationality)
        {
            try
            {

                APIAccess.APIAccessClass.idTypeBox = idType;
                APIAccess.APIAccessClass.idText = idText;
                APIAccess.APIAccessClass.custText = custText;
                APIAccess.APIAccessClass.genderBox = gender;
                APIAccess.APIAccessClass.ageText = Convert.ToInt16(age);
                APIAccess.APIAccessClass.nationalityIndex = nationalityIndex;
                APIAccess.APIAccessClass.nationality = nationality;
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
            //finally
            //{
               
            //        //if (connection.State != ConnectionState.Closed)
            //        //{
            //        //    connection.Close();
            //        //}
                
            //}
        }

        private void ageText_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the key pressed is a control key (e.g., Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the key is not a number or control key, suppress the input
                e.Handled = true;
            }
        }


        // Handle KeyPress event to allow only alphanumeric characters
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Block non-alphanumeric characters
                e.Handled = true;
            }
        }

        // Handle TextChanged event to ensure no invalid characters (extra validation)
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var currentText = idText.Text;
            foreach (char c in currentText)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    // Remove any invalid character that slipped through
                    idText.Text = currentText.Replace(c.ToString(), string.Empty);
                    idText.SelectionStart = idText.Text.Length; // Set cursor to the end
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
