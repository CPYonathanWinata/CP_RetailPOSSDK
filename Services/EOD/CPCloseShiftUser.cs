using LSRetailPosis;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
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
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;

namespace Microsoft.Dynamics.Retail.Pos.EOD
{
    public partial class CPCloseShiftUser : Form
    {
        private ReadOnlyCollection<DataEntity.Employee> employees;
        string closedBy = "";
        string storedBy = "";

            
        public string StoredBy
        {
            get { return storedBy; }
        }

        public CPCloseShiftUser()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;


            loadData();
         
        }

        private void loadData()
        {
            bool foundStaff = false;

            DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                EOD.InternalApplication.Settings.Database.Connection,
                EOD.InternalApplication.Settings.Database.DataAreaID);
            employees = new ReadOnlyCollection<DataEntity.Employee>(employeeDataManager.GetEmployees(ApplicationSettings.Terminal.StoreId));
            const string staffName = "NameOnReceipt";
            const string staffId = "StaffId";

            closedBox.Items.Clear();
            storedBox.Items.Clear();
            // Populate ComboBox with employees
            
            

            foreach (Employee employee in employees)
            {
                string displayText = string.Format("{0} | {1}", employee.StaffId, employee.NameOnReceipt); // Format: ID | Name
                //closedBox.Items.Add(new KeyValuePair<string, string>(employee.StaffId.ToString(), displayText));
                storedBox.Items.Add(new KeyValuePair<string, string>(employee.StaffId.ToString(), displayText));

                if (employee.StaffId == ApplicationSettings.Terminal.TerminalOperator.OperatorId.ToString() && foundStaff == false) // Compare names
                {
                     

                    closedBox.Items.Add(new KeyValuePair<string, string>(employee.StaffId.ToString(), displayText));
                    foundStaff = true;
                    closedBox.Enabled = false; // Disable selection change

                }
            }

            
            //closedBox.Items.Add(new KeyValuePair<string, string>(ApplicationSettings.Terminal.TerminalOperator.OperatorId.ToString(), ApplicationSettings.Terminal.TerminalOperator.OperatorId));

            // Set display and value members
            closedBox.DisplayMember = "Value"; // Shows "ID | Name" in the dropdown
            closedBox.ValueMember = "Key";     // Stores "ID" as the selected value

             // Set display and value members
            storedBox.DisplayMember = "Value"; // Shows "ID | Name" in the dropdown
            storedBox.ValueMember = "Key";     

            // Select first item by default (optional)
            if (closedBox.Items.Count > 0)
            {
                closedBox.SelectedIndex = 0;
            } 

            // Select first item by default (optional)
            if (storedBox.Items.Count > 0)
            {
                storedBox.SelectedIndex = 0;
            }
        }

        private void storedBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null && cb.SelectedItem is KeyValuePair<string, string>)
            {
                KeyValuePair<string, string> selectedEmployee = (KeyValuePair<string, string>)cb.SelectedItem;
                storedBy = selectedEmployee.Key;
                 
            }
        }


        // Event method for handling selection change
        private void ClosedBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null && cb.SelectedItem is KeyValuePair<string, string>)
            {
                KeyValuePair<string, string> selectedEmployee = (KeyValuePair<string, string>)cb.SelectedItem;
                closedBy = selectedEmployee.Key;
               
            }
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
