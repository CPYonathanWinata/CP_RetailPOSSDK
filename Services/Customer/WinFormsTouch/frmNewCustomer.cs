/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSRetailPosis.DevUtilities;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Customer.ViewModels;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Microsoft.Dynamics.Retail.Pos.Customer.WinFormsTouch
{ 
    partial class frmNewCustomer : frmTouchBase
    {
        private CustomerViewModel viewModel;
        private AddressViewModel addressViewModel;

        private const string FilterFormat =
            @"[OrderDate] LIKE '%{0}%' 
            OR [OrderNumber] LIKE '%{0}%' 
            OR [StoreName] LIKE '%{0}%' 
            OR [OrderStatus] LIKE '%{0}%' 
            OR [ItemName] LIKE '%{0}%' 
            OR [ItemQuantity] LIKE '%{0}%' 
            OR [ItemAmount] LIKE '%{0}%'";

        // Default constructor for the designer
        public frmNewCustomer()
        {
            InitializeComponent();
            
        }

        public frmNewCustomer(ICustomer newCustomer, IAddress newAddress)
            : this(newCustomer, null, newAddress)
        { 
        }

        public frmNewCustomer(ICustomer newCustomer, ICustomer invoiceCustomer, IAddress newAddress) : this()
        {
            // set and bind the VM for the customer fields
            this.viewModel = new CustomerViewModel(newCustomer, invoiceCustomer);
            this.bindingSource.Add(this.viewModel);

            // set the VM for the address control
            addressViewModel = new AddressViewModel(newAddress, newCustomer);
            this.viewAddressUserControl1.SetProperties(newCustomer, addressViewModel);
            this.viewAddressUserControl1.SetEditable(true);

            this.gridOrders.DataBindings.Add("DataSource", this.bindingSource, "Items");

            if (Functions.CountryRegion != SupportedCountryRegion.BR)
            {
                labelCpfCnpjNumber.Visible = false;
                textBoxCpfCnpjNumber.Visible = false;
            }

            // Set formatter to convert sales status enum to text
            this.columnOrderStatus.DisplayFormat.Format = new SalesOrderStatusFormatter();
            
        }

        public ICustomer Customer
        {
            get { return this.viewModel.Customer; }
        }
        public IAddress Address
        {
            get { return addressViewModel.Address; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
                SetTextBoxFocus();
                SetRadioButtonsEnabledState();
            }
            
            base.OnLoad(e);
            
        }

        private void SetRadioButtonsEnabledState()
        {
            // enable the customer type radio buttons only if a new customer 
            this.radioOrg.Enabled = this.viewModel.Customer.IsEmptyCustomer();
            this.radioPerson.Enabled = this.viewModel.Customer.IsEmptyCustomer();
        }

        private void TranslateLabels()
        {
            lblHeader.Text         = LSRetailPosis.ApplicationLocalizer.Language.Translate(56329); // customer information:
            labelName.Text         = LSRetailPosis.ApplicationLocalizer.Language.Translate(51129); // Name:
            labelFirstName.Text    = LSRetailPosis.ApplicationLocalizer.Language.Translate(51166); // First Name:
            labelMiddleName.Text   = LSRetailPosis.ApplicationLocalizer.Language.Translate(51167); // Middle Name:
            labelLastName.Text     = LSRetailPosis.ApplicationLocalizer.Language.Translate(51168); // Last Name:
            labelGroup.Text        = LSRetailPosis.ApplicationLocalizer.Language.Translate(51124); // Customer group:
            labelCurrency.Text     = LSRetailPosis.ApplicationLocalizer.Language.Translate(51125); // Currency:
            labelLanguage.Text     = LSRetailPosis.ApplicationLocalizer.Language.Translate(51137); // Language:
            labelAffiliations.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(103376); // Affiliations

            labelPhone.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51134); // Phone number:
            labelEmail.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51138); // Email:
            labelReceiptEmail.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51128); // Receipt email:
            labelWebSite.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51127); // Website:
            labelCpfCnpjNumber.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51207); // CPF / CNPJ:

            labelDateCreated.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51214); // Date created:
            labelTotalVisits.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51213); // Total visits:
            labelBalance.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(1566); // Balance:

            labelInvAccountName.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51225); // Invoice account name:
            labelInvAccountId.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51226); // Invoice account ID:
            labelInvBalance.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51227); // Invoice acount balance:

            labelSearch.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51209); // Sales orders:
            labelStoreLastVisited.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51211); // Store last visited:
            labelTotalSales.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51210); // Total sales:
            labelDateLastVisit.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51212); // Date of last visit:

            tabPageContact.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51216); // Customer details
            tabPageHistory.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51215); // Transaction history

            columnAmount.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51217); // Amount
            columnDate.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51223); // Date
            columnItem.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51220); // Product
            columnOrderNumber.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51222); // Order number
            columnOrderStatus.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51224); // Order status
            columnQuantity.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51219); // Quantity
            columnStore.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(51221); // Store

            labelType.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51169); // Type:
            radioOrg.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51165); // Organization
            radioPerson.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51164); // Person

            btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56319); // Cancel
            btnSave.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(56318); // Save
            btnClear.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51149); // Clear

            this.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51041); // Customer
        }

        #region events
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            if (hevent == null)
                throw new ArgumentNullException("hevent");

            LSRetailPosis.POSControls.POSFormsManager.ShowHelp(this);

            hevent.Handled = true;
            base.OnHelpRequested(hevent);
        }

        private void OnCustomerGroup_Click(object sender, EventArgs e)
        {
            this.viewModel.ExecuteSelectGroup();
        }

        private void OnCurrency_Click(object sender, EventArgs e)
        {
            this.viewModel.ExecuteSelectCurrency();
        }

        private void OnLanguage_Click(object sender, EventArgs e)
        {
            this.viewModel.ExecuteSelectLanguage();
        }

        private void OnAffiliations_Click(object sender, EventArgs e)
        {
            this.viewModel.ExecuteSelectAffiliation();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.viewModel.ExecuteClear();
            this.addressViewModel.ExecuteClear();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (!this.SaveCustomer())
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }

        #endregion

        private void SetSearchFilter(string p)
        {
            string filter = string.Empty;

            if (!string.IsNullOrWhiteSpace(p))
            {
                filter = string.Format(FilterFormat, p);
            }
            this.gridView.ActiveFilterString = filter;
        }

        private void SetTextBoxFocus()
        {
            if (this.viewModel.IsPerson)
            {
                //Person
                textBoxFirstName.Select();
            }
            else
            {
                //Organization
                textBoxName.Select();
            }
        }
        private bool saveOtherField()
        {
            string gender;
            string custType;

            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            

            try
            {
                
                if(this.cmbBoxGender.SelectedItem == null)
                {
                    gender = "";
                }
                else
                {
                    gender = this.cmbBoxGender.SelectedItem.ToString();
                }
                if(this.cmbBoxCustType.SelectedItem == null)
                {
                    custType = "";
                }
                else
                {
                    custType = this.cmbBoxCustType.SelectedItem.ToString();
                }
                
                
                string queryString = @"exec CP_InsertNewCustomer @CUSTACCOUNT  , @GENDER , @BIRTHDATE , @CUSTTYPE";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@CUSTACCOUNT", Customer.CustomerId);
                    command.Parameters.AddWithValue("@GENDER", gender);
                    command.Parameters.AddWithValue("@BIRTHDATE", this.dateBirth.Value);
                    command.Parameters.AddWithValue("@CUSTTYPE", custType);

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

            return true;
        }
        


        private bool validateCustomer()
        {
            bool validateOK = true;

            if (cmbBoxCustType.SelectedItem == "Loyalty")
            {
                if (String.IsNullOrEmpty(textIDNumber.Text))
                {
                    Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("ID Number must be filled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    Int64 number2;
                    if (Int64.TryParse(textIDNumber.Text, out number2) == false)
                    {
                        Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("ID Number must be numeric", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    { 
                        if(textIDNumber.Text.Length != 16)
                        {
                            Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("ID Number character should be 16 Digit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        if (this.checkDuplicateIdentificationNo())
                        {
                            //Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Identification Number is already exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    
                    }
                }
                if (dateBirth.Text.Length == 0)
                {
                    Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Birth date must be filled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (cmbBoxGender.SelectedItem == "" || cmbBoxGender.SelectedItem == null)
                {
                    Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Gender must be selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (String.IsNullOrEmpty(textBoxPhone.Text))
                {
                    Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Phone number must be filled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    Int64 number3;
                    if (Int64.TryParse(textBoxPhone.Text, out number3) == false)
                    {
                        Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Phone number must be numeric", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        if (this.checkDuplicatePhoneNumber())
                        {
                            //Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Phone number is already exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }
            return validateOK;
        }
        private bool checkDuplicatePhoneNumber()
        {
            string locator = "";
            //string connectionString = @"Data Source= DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=DevDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";
            //string connectionString = @"Data Source= DYNAMICS16\SQLAXDB1 ;Initial Catalog=PRDDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            
            SqlConnection connection = new SqlConnection(connectionString); 
            //SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
             

            // Update Validate Partition by Erwin 12 December 2019
            try
            {
                string queryString = @" SELECT LOCATOR
                                        FROM LOGISTICSELECTRONICADDRESS A
                                        INNER JOIN DIRPARTYLOCATION B ON A.LOCATION = B.LOCATION
                                        INNER JOIN CUSTTABLE C ON B.PARTY = C.PARTY
                                        INNER JOIN DATAAREA D ON C.PARTITION = D.PARTITION
                                        WHERE A.LOCATOR = @PHONENO and A.[TYPE] = 1 AND D.ID = @DATAAREAID ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@PHONENO", textBoxPhone.Text);
                    command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            locator = reader.GetString(0);
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
            if (locator != "")
            {
                Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Phone number :"+locator+" is already exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                           
                return true;

            }
            else
            {
                return false;
            }
        }
        
        private bool checkDuplicateIdentificationNo()
        {
            string IdNumber = "";

            //string connectionString = @"Data Source= DYNAMICS01\DEVPRISQLSVR ;Initial Catalog=DevDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";//Persist Security Info=False;User ID=USER_NAME;Password=USER_PASS;
            //string connectionString = @"Data Source= DYNAMICS16\SQLAXDB1 ;Initial Catalog=PRDDynamicsAX; Integrated Security=False;User ID=AXPOS;Password=P@ssw0rd;";
            string connectionString = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            
            SqlConnection connection = new SqlConnection(connectionString); //LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;


            // Update Validate Partition by Erwin 12 December 2019
            try
            {
                string queryString = @" SELECT IDENTIFICATIONNUMBER
                                        FROM CUSTTABLE A
                                        INNER JOIN DATAAREA B ON A.PARTITION = B.PARTITION
                                        WHERE IDENTIFICATIONNUMBER = @IDNO AND B.ID = @DATAAREAID ";
                //  AND CUSTGROUP = @CUSTGROUP ";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@IDNO", textIDNumber.Text);
                    command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);
                    //command.Parameters.AddWithValue("@CUSTGROUP", textBoxGroup.Text);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IdNumber = reader.GetString(0);
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
            if (IdNumber != "")
            {
                Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage("Identification Number : "+IdNumber+" is already exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            
                return true;

            }
            else
            {
                return false;
            }
        }

        private string cpGetOtherData()
        {
            string strGetData  = "";
            string strGender = "";
            string strCustType = "";

            if (this.cmbBoxGender.SelectedItem != null)
            {
                strGender = this.cmbBoxGender.SelectedItem.ToString();
            }
            if (this.cmbBoxCustType.SelectedItem != null)
            {
                strCustType = this.cmbBoxCustType.SelectedItem.ToString();    
            }
            
            
            strGetData = this.dateBirth.Value.ToString("MMddyyyy");

            if (strCustType == "Loyalty")
            {
                strGetData = strGetData + "1";
            }
            else
            {
                strGetData = strGetData + "0";
            }

            switch (strGender)
            {
                case "Male":
                    strGetData = strGetData + "1";
                    break;
                case "Female":
                    strGetData = strGetData + "2";
                    break;
                case "Non-Specific":
                    strGetData = strGetData + "3";
                    break;
                default:
                    strGetData = strGetData + "0";
                    break;
            }

            return strGetData;
        
        }

        private bool SaveCustomer()
        {
            try
            {
                bool createdLocal = false;
                bool createdAx = false;
                string comment = null;
                
                if (!this.validateCustomer())
                {
                    return false;
                }    
                
                DialogResult prompt = Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage(51148, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (prompt == System.Windows.Forms.DialogResult.Yes)
                {
                    IList<Int64> entityKeys = new List<Int64>();
                    ICustomer tempCustomer = MergeAddress(this.viewModel.Customer, this.addressViewModel.Address);
                    bool isEmptyCustomer = this.viewModel.Customer.IsEmptyCustomer();
                    tempCustomer.CCMNumber =this.cpGetOtherData();
                    // this.isEmptyCustomer is initialized at form load and uses the incoming customer object
                   // Pos.Customer.Customer.InternalApplication.TransactionServices.cgetin
                    if (isEmptyCustomer)
                    {
                        // Attempt to save in AX
                        Pos.Customer.Customer.InternalApplication.TransactionServices.NewCustomer(ref createdAx, ref comment, ref tempCustomer, ApplicationSettings.Terminal.StorePrimaryId, ref entityKeys);
                    }
                    else
                    {
                        Pos.Customer.Customer.UpdateCustomer(ref createdAx, ref comment, ref tempCustomer, ref entityKeys);
                    }
                    // Was the customer created in AX
                    if (createdAx)
                    {
                        // Was the customer created locally
                        DM.CustomerDataManager customerDataManager = new DM.CustomerDataManager(
                            ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

                        LSRetailPosis.Transaction.Customer transactionalCustomer = tempCustomer as LSRetailPosis.Transaction.Customer;

                        if (isEmptyCustomer)
                        {
                            createdLocal = customerDataManager.SaveTransactionalCustomer(transactionalCustomer, entityKeys);
                        }
                        else
                        {
                            createdLocal = customerDataManager.UpdateTransactionalCustomer(transactionalCustomer, entityKeys);
                        }

                        //Update the VM
                        this.viewModel = new CustomerViewModel(tempCustomer);
                        //if (cmbBoxCustType.SelectedItem == "Loyalty")
                        //{
                            this.saveOtherField();
                        //}       
                    }

                    if (!createdAx)
                    {
                        Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage(51159, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!createdLocal)
                    {
                        Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage(51156, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                return createdAx && createdLocal;
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                Pos.Customer.Customer.InternalApplication.Services.Dialog.ShowMessage(51158, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static ICustomer MergeAddress(ICustomer iCustomer, IAddress iAddress)
        {
            if (iCustomer != null && iAddress != null)
            {
                iCustomer.AddressComplement = iAddress.BuildingComplement;
                iCustomer.City = iAddress.City;
                
                iCustomer.Country = iAddress.Country;
                iCustomer.County = iAddress.County;
                iCustomer.DistrictName = iAddress.DistrictName;
                
                iCustomer.Extension = iAddress.Extension;
                iCustomer.OrgId = iAddress.OrgId;
                iCustomer.PostalCode = iAddress.PostalCode;
                iCustomer.State = iAddress.State;
                iCustomer.StreetName = iAddress.StreetName;
                iCustomer.AddressNumber = iAddress.StreetNumber;
                iCustomer.Address = iAddress.LineAddress;
                iCustomer.PrimaryAddress.AddressType = iAddress.AddressType;
                iCustomer.PrimaryAddress.Name = iAddress.Name;
                iCustomer.PrimaryAddress.Email = iAddress.Email;
                iCustomer.PrimaryAddress.Telephone = iAddress.Telephone;
                iCustomer.PrimaryAddress.URL = iAddress.URL;
                iCustomer.PrimaryAddress.SalesTaxGroup = iAddress.SalesTaxGroup;

                iCustomer.PrimaryAddress.CountryISOCode = iAddress.CountryISOCode;
                iCustomer.PrimaryAddress.StateDescription = iAddress.StateDescription;
                iCustomer.PrimaryAddress.CountyDescription = iAddress.CountyDescription;
                iCustomer.PrimaryAddress.CityRecId = iAddress.CityRecId;
                iCustomer.PrimaryAddress.CityDescription = iAddress.CityDescription;
                iCustomer.PrimaryAddress.DistrictRecId = iAddress.DistrictRecId;
                iCustomer.PrimaryAddress.DistrictDescription = iAddress.DistrictDescription;

                switch (iCustomer.RelationType)
                {
                    case RelationType.Person:
                        // send party name in fixed format
                        string name = Utility.JoinStrings(" ", iCustomer.FirstName, iCustomer.MiddleName, iCustomer.LastName);

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            iCustomer.Name = name;
                        }
                        break;
                    default:
                        // No change
                        break;
                }
            }
            return iCustomer;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SetSearchFilter(this.textSearch.Text.Trim());
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            SetSearchFilter(string.Empty);
            this.textSearch.Text = string.Empty;
        }

        private void tabControlParent_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == this.tabPageHistory)
            {
                this.BeginInvoke(new Action(this.viewModel.ExecuteLoadHistory));
                this.BeginInvoke(new Action(this.viewModel.ExecuteLoadBalances));
            }
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrevPage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            gridView.MovePrev();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNext();
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            gridView.MoveNextPage();
        }

        private void textSearch_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = buttonSearch;
        }

        private void textSearch_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void tableLayoutPanelLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void viewAddressUserControl1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanelRight_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            /*if (String.IsNullOrEmpty(textIDNumber.Text) ) //&& cmbBoxCustType.SelectedText == "Loyalty")
            {
                MessageBox.Show("Please fill ID number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {*/
                /*SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                try
                {
                    string queryString = @"exec CP_UpdateInsertCPOrder @TRANSACTIONID  , @NOORDER , @DATEORDER,@NAMEORDER";

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
                this.Close();*/
            //}



        }

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void textBoxFirstName_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateBirth_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}