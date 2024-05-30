using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Transaction;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using LSRetailPosis.Settings;
using System.Text.RegularExpressions;

//Begin Add NEC-Hamzah
using System.Configuration;

using System.Drawing.Printing;
//End Add NEC-Hamzah
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

using LSRetailPosis.DataAccess;
//using Microsoft.Dynamics.Retail.Diagnostics;
//using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using LSRetailPosis.POSProcesses;

//using Microsoft.Dynamics.Retail.Pos.Customer;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPLoveRemittTPT : Form
    {
        private System.Data.DataTable purposes;
        private System.Data.DataTable occupations;

        IPosTransaction posTransaction;
        Timer timer = new System.Windows.Forms.Timer();
        Timer timerCountdown = new System.Windows.Forms.Timer();

        const string USER_AGENT = BlankOperations.USER_AGENT;

        const int DEVICE_ID = BlankOperations.LV_DEVICE_ID;
        const string DEVICE_KEY = BlankOperations.LV_DEVICE_KEY;
        const string MERCHANT_ID = BlankOperations.LV_MERCHANT_ID;
        const string MERCHANT_KEY = BlankOperations.LV_MERCHANT_KEY;

        const string LV_TPT_INQUIRY_URL = BlankOperations.LV_TPT_INQUIRY_URL;
        const string LV_TPT_PROCESS_URL = BlankOperations.LV_TPT_PROCESS_URL;

        const string LV_GET_OCCUPATION_URL = BlankOperations.LV_GET_OCCUPATION_URL;
        const string LV_GET_PURPOSE_URL = BlankOperations.LV_GET_PURPOSE_URL;

    //    int pendingTransID;
     //   int convert = 0;
        int isCheckClick = 0;
        int countDownCounter = 60;
        int isFromAnotherFunction = 0;
        int isSuccessPay = 0;
    //    decimal poinBal = 0;
    //    string token;
        int Offset = 0;

        string upLine1 = "";
        string upLine2 = "";
        string upLine3 = "";
        string upLine4 = "";
        string downLine1 = "";
        string downLine2 = "";
        string downLine3 = "";
        string trans_date = "";

        string total_amount = "";
        string expired = "";
        string receiver_name = "";
        string transId = "";
        string ref_no = "";
        string city_name = "";
        string address = "";
        string occupation_name = "";
        string identity_type = "";
        string occupation_id = "";
        string phone_number = "";
        string identity_number = "";
        string trans_purpose_id = "";
        string trans_purpose_name = "";
        string city_id = "";
        string transaction_id = "";
        string tpt_code = "";

        string identity_no_print = "";
        string phone_number_print = "";

        string receiver_gender = "";
        string identity_exp = "";
        string postal_code = "";
        string dob = "";
        string pob = "";
        string district = "";
        string  subdistrict = "";

        string amount = "";
        string fee = "";
        string ext_ref = "";

        string s = "";
        int x = 0;
        int y = 0;
        string spc = "";

        private static readonly Regex regex = new Regex(@"^\d+$");
     
        public CPLoveRemittTPT(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;     
        }

        private void CPLoveRemittTPT_Load(object sender, EventArgs e)
        {
            lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", (""));
            lblResponse.Text = "";
  
            btnCancel.Enabled = true;
            btnCancel.Visible = true;
   
            btnProcess.Visible = false;
            btnProcess.Enabled = false;
    
            isCheckClick = 0;
         //   lblCountdown.Text = "";
            isSuccessPay = 0;

            occupations = this.getOccupation();

            cbOccupation.DataSource = occupations;
            cbOccupation.ValueMember = "Occupation_Id";
            cbOccupation.DisplayMember = "Occupation_Text";
      
            purposes = this.getPurpose();

            cbPurpose.DataSource = purposes;
            cbPurpose.ValueMember = "Purpose_Id";
            cbPurpose.DisplayMember = "Purpose_Text";
            
        }
              
        private void InitializeComponent()
        {
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblPaymentCode = new System.Windows.Forms.Label();
            this.txtTPTCode = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtFee = new System.Windows.Forms.TextBox();
            this.lblCustName = new System.Windows.Forms.Label();
            this.txtReceiverName = new System.Windows.Forms.TextBox();
            this.lblTransId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.btnCopyReceipt = new System.Windows.Forms.Button();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.lblRefNo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtIdentityType = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIdentityNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPoB = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOccupationName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPurposeName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtSubDistrict = new System.Windows.Forms.TextBox();
            this.txtDoB = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.txtCityName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtCityId = new System.Windows.Forms.TextBox();
            this.txtPurposeId = new System.Windows.Forms.TextBox();
            this.txtOccupationId = new System.Windows.Forms.TextBox();
            this.txtTransType = new System.Windows.Forms.TextBox();
            this.txtIdentityExp = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbGenderM = new System.Windows.Forms.RadioButton();
            this.rbGenderF = new System.Windows.Forms.RadioButton();
            this.cbOccupation = new System.Windows.Forms.ComboBox();
            this.cbPurpose = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtAddress
            // 
            this.txtAddress.Enabled = false;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(289, 255);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(231, 75);
            this.txtAddress.TabIndex = 2;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(292, 239);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(86, 13);
            this.lblAmount.TabIndex = 3;
            this.lblAmount.Text = "Alamat Penerima";
            // 
            // lblPaymentCode
            // 
            this.lblPaymentCode.AutoSize = true;
            this.lblPaymentCode.Location = new System.Drawing.Point(25, 146);
            this.lblPaymentCode.Name = "lblPaymentCode";
            this.lblPaymentCode.Size = new System.Drawing.Size(53, 13);
            this.lblPaymentCode.TabIndex = 4;
            this.lblPaymentCode.Text = "KodeTPT";
            // 
            // txtTPTCode
            // 
            this.txtTPTCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTPTCode.Location = new System.Drawing.Point(23, 162);
            this.txtTPTCode.MaxLength = 20;
            this.txtTPTCode.Name = "txtTPTCode";
            this.txtTPTCode.Size = new System.Drawing.Size(231, 26);
            this.txtTPTCode.TabIndex = 5;
            this.txtTPTCode.TextChanged += new System.EventHandler(this.txtTPTCode_TextChanged);
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCheck.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.Location = new System.Drawing.Point(636, 189);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(131, 53);
            this.btnCheck.TabIndex = 7;
            this.btnCheck.Text = "Check TPT";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(23, 51);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(359, 29);
            this.lblTotalAmount.TabIndex = 10;
            this.lblTotalAmount.Text = "Total Amount : Rp 100.000.000, -";
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResponse.Location = new System.Drawing.Point(24, 80);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(275, 24);
            this.lblResponse.TabIndex = 11;
            this.lblResponse.Text = "Response Message Goes Here";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(636, 421);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 53);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnProcess.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(636, 267);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(131, 53);
            this.btnProcess.TabIndex = 13;
            this.btnProcess.Text = "Process TPT";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Visible = false;
            this.btnProcess.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // txtFee
            // 
            this.txtFee.Enabled = false;
            this.txtFee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFee.Location = new System.Drawing.Point(23, 524);
            this.txtFee.Name = "txtFee";
            this.txtFee.ReadOnly = true;
            this.txtFee.Size = new System.Drawing.Size(231, 26);
            this.txtFee.TabIndex = 16;
            this.txtFee.Text = "fee";
            this.txtFee.Visible = false;
            // 
            // lblCustName
            // 
            this.lblCustName.AutoSize = true;
            this.lblCustName.Location = new System.Drawing.Point(24, 193);
            this.lblCustName.Name = "lblCustName";
            this.lblCustName.Size = new System.Drawing.Size(82, 13);
            this.lblCustName.TabIndex = 19;
            this.lblCustName.Text = "Nama Penerima";
            this.lblCustName.Click += new System.EventHandler(this.lblCustName_Click);
            // 
            // txtReceiverName
            // 
            this.txtReceiverName.Enabled = false;
            this.txtReceiverName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReceiverName.Location = new System.Drawing.Point(23, 209);
            this.txtReceiverName.Name = "txtReceiverName";
            this.txtReceiverName.ReadOnly = true;
            this.txtReceiverName.Size = new System.Drawing.Size(231, 26);
            this.txtReceiverName.TabIndex = 18;
            this.txtReceiverName.TextChanged += new System.EventHandler(this.txtCustomerName_TextChanged);
            // 
            // lblTransId
            // 
            this.lblTransId.AutoSize = true;
            this.lblTransId.Location = new System.Drawing.Point(25, 120);
            this.lblTransId.Name = "lblTransId";
            this.lblTransId.Size = new System.Drawing.Size(46, 13);
            this.lblTransId.TabIndex = 20;
            this.lblTransId.Text = "Trans Id";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(329, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 29);
            this.label1.TabIndex = 21;
            this.label1.Text = "TPT LOVE";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtAmount
            // 
            this.txtAmount.Enabled = false;
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(294, 524);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new System.Drawing.Size(231, 26);
            this.txtAmount.TabIndex = 22;
            this.txtAmount.Text = "amount";
            this.txtAmount.Visible = false;
            // 
            // btnCopyReceipt
            // 
            this.btnCopyReceipt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCopyReceipt.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCopyReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyReceipt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyReceipt.Location = new System.Drawing.Point(636, 339);
            this.btnCopyReceipt.Name = "btnCopyReceipt";
            this.btnCopyReceipt.Size = new System.Drawing.Size(131, 53);
            this.btnCopyReceipt.TabIndex = 24;
            this.btnCopyReceipt.Text = "Copy Receipt";
            this.btnCopyReceipt.UseVisualStyleBackColor = false;
            this.btnCopyReceipt.Visible = false;
            this.btnCopyReceipt.Click += new System.EventHandler(this.btnCopyReceipt_Click);
            // 
            // txtRefNo
            // 
            this.txtRefNo.Enabled = false;
            this.txtRefNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefNo.Location = new System.Drawing.Point(289, 487);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.ReadOnly = true;
            this.txtRefNo.Size = new System.Drawing.Size(231, 26);
            this.txtRefNo.TabIndex = 25;
            this.txtRefNo.Visible = false;
            // 
            // lblRefNo
            // 
            this.lblRefNo.AutoSize = true;
            this.lblRefNo.Location = new System.Drawing.Point(294, 471);
            this.lblRefNo.Name = "lblRefNo";
            this.lblRefNo.Size = new System.Drawing.Size(41, 13);
            this.lblRefNo.TabIndex = 26;
            this.lblRefNo.Text = "Ref No";
            this.lblRefNo.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 241);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Jenis Kelamin Penerima ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "No HP Penerima";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Enabled = false;
            this.txtPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(23, 305);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.ReadOnly = true;
            this.txtPhoneNumber.Size = new System.Drawing.Size(231, 26);
            this.txtPhoneNumber.TabIndex = 29;
            // 
            // txtIdentityType
            // 
            this.txtIdentityType.Enabled = false;
            this.txtIdentityType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdentityType.Location = new System.Drawing.Point(726, 529);
            this.txtIdentityType.Name = "txtIdentityType";
            this.txtIdentityType.ReadOnly = true;
            this.txtIdentityType.Size = new System.Drawing.Size(74, 26);
            this.txtIdentityType.TabIndex = 31;
            this.txtIdentityType.Text = "identy id";
            this.txtIdentityType.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 334);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "No KTP/SIM";
            // 
            // txtIdentityNo
            // 
            this.txtIdentityNo.Enabled = false;
            this.txtIdentityNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdentityNo.Location = new System.Drawing.Point(23, 351);
            this.txtIdentityNo.Name = "txtIdentityNo";
            this.txtIdentityNo.ReadOnly = true;
            this.txtIdentityNo.Size = new System.Drawing.Size(231, 26);
            this.txtIdentityNo.TabIndex = 33;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 380);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Masa Berlaku KTP/SIM";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 425);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "Tanggal Lahir Penerima";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 470);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Tempat Lahir Penerima ";
            // 
            // txtPoB
            // 
            this.txtPoB.Enabled = false;
            this.txtPoB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoB.Location = new System.Drawing.Point(23, 486);
            this.txtPoB.Name = "txtPoB";
            this.txtPoB.Size = new System.Drawing.Size(231, 26);
            this.txtPoB.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(292, 194);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "Pekerjaan Penerima";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // txtOccupationName
            // 
            this.txtOccupationName.Enabled = false;
            this.txtOccupationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOccupationName.Location = new System.Drawing.Point(297, 556);
            this.txtOccupationName.Name = "txtOccupationName";
            this.txtOccupationName.ReadOnly = true;
            this.txtOccupationName.Size = new System.Drawing.Size(231, 26);
            this.txtOccupationName.TabIndex = 43;
            this.txtOccupationName.Text = "pekerjaan";
            this.txtOccupationName.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(292, 146);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Tujuan Penerima";
            // 
            // txtPurposeName
            // 
            this.txtPurposeName.Enabled = false;
            this.txtPurposeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPurposeName.Location = new System.Drawing.Point(31, 557);
            this.txtPurposeName.Name = "txtPurposeName";
            this.txtPurposeName.ReadOnly = true;
            this.txtPurposeName.Size = new System.Drawing.Size(231, 26);
            this.txtPurposeName.TabIndex = 41;
            this.txtPurposeName.Text = "purpose";
            this.txtPurposeName.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(292, 426);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 46;
            this.label12.Text = "Kode Pos Penerima";
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Enabled = false;
            this.txtPostalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostalCode.Location = new System.Drawing.Point(289, 439);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(231, 26);
            this.txtPostalCode.TabIndex = 45;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(293, 378);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(161, 13);
            this.label14.TabIndex = 50;
            this.label14.Text = "Kecamatan/Kelurahan Penerima";
            // 
            // txtSubDistrict
            // 
            this.txtSubDistrict.Enabled = false;
            this.txtSubDistrict.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubDistrict.Location = new System.Drawing.Point(289, 396);
            this.txtSubDistrict.Name = "txtSubDistrict";
            this.txtSubDistrict.Size = new System.Drawing.Size(231, 26);
            this.txtSubDistrict.TabIndex = 49;
            this.txtSubDistrict.TextChanged += new System.EventHandler(this.textBox12_TextChanged);
            // 
            // txtDoB
            // 
            this.txtDoB.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDoB.Location = new System.Drawing.Point(23, 441);
            this.txtDoB.Name = "txtDoB";
            this.txtDoB.Size = new System.Drawing.Size(231, 23);
            this.txtDoB.TabIndex = 51;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(399, 80);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(354, 16);
            this.label15.TabIndex = 52;
            this.label15.Text = "* KASIR WAJIB MINTA FOTOCOPY KTP/SIM 1LBR";
            // 
            // txtCityName
            // 
            this.txtCityName.Enabled = false;
            this.txtCityName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCityName.Location = new System.Drawing.Point(289, 349);
            this.txtCityName.Name = "txtCityName";
            this.txtCityName.ReadOnly = true;
            this.txtCityName.Size = new System.Drawing.Size(231, 26);
            this.txtCityName.TabIndex = 53;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(292, 333);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 13);
            this.label16.TabIndex = 54;
            this.label16.Text = "Kota Penerima";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // txtCityId
            // 
            this.txtCityId.Enabled = false;
            this.txtCityId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCityId.Location = new System.Drawing.Point(644, 530);
            this.txtCityId.Name = "txtCityId";
            this.txtCityId.ReadOnly = true;
            this.txtCityId.Size = new System.Drawing.Size(76, 26);
            this.txtCityId.TabIndex = 55;
            this.txtCityId.Text = "city id";
            this.txtCityId.Visible = false;
            // 
            // txtPurposeId
            // 
            this.txtPurposeId.Enabled = false;
            this.txtPurposeId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPurposeId.Location = new System.Drawing.Point(670, 495);
            this.txtPurposeId.Name = "txtPurposeId";
            this.txtPurposeId.ReadOnly = true;
            this.txtPurposeId.Size = new System.Drawing.Size(85, 26);
            this.txtPurposeId.TabIndex = 57;
            this.txtPurposeId.Text = "purpose id";
            this.txtPurposeId.Visible = false;
            // 
            // txtOccupationId
            // 
            this.txtOccupationId.Enabled = false;
            this.txtOccupationId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOccupationId.Location = new System.Drawing.Point(560, 530);
            this.txtOccupationId.Name = "txtOccupationId";
            this.txtOccupationId.ReadOnly = true;
            this.txtOccupationId.Size = new System.Drawing.Size(67, 26);
            this.txtOccupationId.TabIndex = 59;
            this.txtOccupationId.Text = "job id";
            this.txtOccupationId.Visible = false;
            // 
            // txtTransType
            // 
            this.txtTransType.Enabled = false;
            this.txtTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransType.Location = new System.Drawing.Point(560, 497);
            this.txtTransType.Name = "txtTransType";
            this.txtTransType.ReadOnly = true;
            this.txtTransType.Size = new System.Drawing.Size(88, 26);
            this.txtTransType.TabIndex = 63;
            this.txtTransType.Text = "trans type";
            this.txtTransType.Visible = false;
            // 
            // txtIdentityExp
            // 
            this.txtIdentityExp.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdentityExp.Location = new System.Drawing.Point(23, 399);
            this.txtIdentityExp.Name = "txtIdentityExp";
            this.txtIdentityExp.Size = new System.Drawing.Size(231, 23);
            this.txtIdentityExp.TabIndex = 64;
            this.txtIdentityExp.Value = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(400, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(356, 16);
            this.label2.TabIndex = 65;
            this.label2.Text = "* KASIR WAJIB CEK DATA KTP dengan FORMTPT";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(400, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(378, 15);
            this.label5.TabIndex = 66;
            this.label5.Text = "* DATA DIRI FORMTPT dengan COPY KTP HARUS SESUAI";
            // 
            // rbGenderM
            // 
            this.rbGenderM.AutoSize = true;
            this.rbGenderM.Location = new System.Drawing.Point(23, 261);
            this.rbGenderM.Name = "rbGenderM";
            this.rbGenderM.Size = new System.Drawing.Size(43, 17);
            this.rbGenderM.TabIndex = 67;
            this.rbGenderM.TabStop = true;
            this.rbGenderM.Text = "Pria";
            this.rbGenderM.UseVisualStyleBackColor = true;
            this.rbGenderM.CheckedChanged += new System.EventHandler(this.rbGenderM_CheckedChanged);
            // 
            // rbGenderF
            // 
            this.rbGenderF.AutoSize = true;
            this.rbGenderF.Location = new System.Drawing.Point(76, 261);
            this.rbGenderF.Name = "rbGenderF";
            this.rbGenderF.Size = new System.Drawing.Size(59, 17);
            this.rbGenderF.TabIndex = 68;
            this.rbGenderF.TabStop = true;
            this.rbGenderF.Text = "Wanita";
            this.rbGenderF.UseVisualStyleBackColor = true;
            this.rbGenderF.CheckedChanged += new System.EventHandler(this.rbGenderF_CheckedChanged);
            // 
            // cbOccupation
            // 
            this.cbOccupation.Enabled = false;
            this.cbOccupation.FormattingEnabled = true;
            this.cbOccupation.Location = new System.Drawing.Point(289, 211);
            this.cbOccupation.Name = "cbOccupation";
            this.cbOccupation.Size = new System.Drawing.Size(231, 21);
            this.cbOccupation.TabIndex = 69;
            // 
            // cbPurpose
            // 
            this.cbPurpose.Enabled = false;
            this.cbPurpose.FormattingEnabled = true;
            this.cbPurpose.Location = new System.Drawing.Point(289, 163);
            this.cbPurpose.Name = "cbPurpose";
            this.cbPurpose.Size = new System.Drawing.Size(231, 21);
            this.cbPurpose.TabIndex = 70;
            // 
            // CPLoveRemittTPT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(804, 595);
            this.Controls.Add(this.cbPurpose);
            this.Controls.Add(this.cbOccupation);
            this.Controls.Add(this.rbGenderF);
            this.Controls.Add(this.rbGenderM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIdentityExp);
            this.Controls.Add(this.txtTransType);
            this.Controls.Add(this.txtOccupationId);
            this.Controls.Add(this.txtPurposeId);
            this.Controls.Add(this.txtCityId);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtCityName);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtDoB);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtSubDistrict);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtOccupationName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPurposeName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPoB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtIdentityNo);
            this.Controls.Add(this.txtIdentityType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblRefNo);
            this.Controls.Add(this.txtRefNo);
            this.Controls.Add(this.btnCopyReceipt);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTransId);
            this.Controls.Add(this.lblCustName);
            this.Controls.Add(this.txtReceiverName);
            this.Controls.Add(this.txtFee);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.txtTPTCode);
            this.Controls.Add(this.lblPaymentCode);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.txtAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "CPLoveRemittTPT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payment LoveRemit";
            this.Load += new System.EventHandler(this.CPLoveRemittTPT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            btnProcess.Visible = true;
            btnProcess.Enabled = true;
            timer.Stop();
        }
        
        private void timerCountdown_Tick(object sender, System.EventArgs e)
        {
            countDownCounter--;
            if(countDownCounter == 0)
            {
                isFromAnotherFunction = 1;
                btnCheck_Click(btnCheck, EventArgs.Empty);
                isFromAnotherFunction = 0;
                this.Close();
            }
         //   lblCountdown.Text = "This window will close automatically in " + countDownCounter.ToString() + " second(s)";
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            lblResponse.Text = "Processing...";
            
            txtReceiverName.Text    = "";
          //  txtReceiverGender.Text  = "";
            txtPhoneNumber.Text     = "";
            txtIdentityType.Text    = "";
            txtIdentityNo.Text      = "";
            txtIdentityExp.Text     = "";
            txtAddress.Text         = "";
            txtDoB.Text             = "";
            txtPoB.Text             = "";
            txtPurposeName.Text     = "";
            txtPurposeId.Text       = "";
            txtOccupationName.Text  = "";
            txtPostalCode.Text      = "";
           
            txtSubDistrict.Text     = "";
            txtCityId.Text          = "";
            txtCityName.Text        = "";

            txtFee.Text     = "";
            lblTransId.Text = "";
            txtAmount.Text  = "";
            txtRefNo.Text   = "";

            string responseCode = "0";     
            string message = "";    
            string response_time = "";

            tpt_code = txtTPTCode.Text;

            if (regex.IsMatch(tpt_code))
            {
                int pc_length = tpt_code.Length;

                if (pc_length >= 10)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                    string reqCheckUrl = LV_TPT_INQUIRY_URL;

                    ASCIIEncoding encoder = new ASCIIEncoding();
                    byte[] data = encoder.GetBytes("");

                    HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
                    reqCheck.Method = "POST";
                    reqCheck.Credentials = CredentialCache.DefaultCredentials;
                    reqCheck.Accept = "text/json";
        
                    reqCheck.Headers["Authorization"]   = "PFM";
                
                    reqCheck.UserAgent = USER_AGENT;
                    reqCheck.ContentType = "application/json";

                    string requestBody = string.Format("{{\"store_code\":\"{0}\",\"data_area\":\"{1}\",\"terminal_id\":\"{2}\",\"mtcn_code\":\"{3}\",\"operator_id\":\"{4}\"}}", ApplicationSettings.Database.StoreID, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Database.TerminalID, tpt_code, ApplicationSettings.Terminal.TerminalOperator.OperatorId);
                    byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

                    Stream requestStream = reqCheck.GetRequestStream();
                    requestStream.Write(byteArray, 0, byteArray.Length);
                    requestStream.Close();

                    using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                    {
                        string content = string.Empty;
                        using (var stream = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                content = sr.ReadToEnd();
                            }
                        }
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        dynamic item = serializer.Deserialize<object>(content);

                        responseCode        = item["response_code"].ToString();
                        transaction_id      = item["trans_id"].ToString();
                        response_time       = item["response_time"].ToString();
                        amount              = item["transaction_amount"].ToString();
                        amount              = amount.Replace(".0", "");
                        fee                 = item["transaction_fee"].ToString();
                        total_amount        = item["transaction_amount"].ToString();

                        city_name           = item["city_name"].ToString();
                        address             = item["address"].ToString();
                        trans_purpose_id    = item["trans_purpose_id"].ToString();
                        occupation_name     = item["occupation_name"].ToString();
                        identity_type       = item["identity_type_id"].ToString();
                        receiver_name       = item["name"].ToString();
                        occupation_id       = item["occupation_id"].ToString();
                        phone_number        = item["phone_number"].ToString();
                        identity_number     = item["identity_number"].ToString();
                        trans_purpose_name  = item["trans_purpose_name"].ToString();
                        city_id             = item["city_id"].ToString();

                        subdistrict         = item["subdistrict"].ToString();
                        pob                 = item["pob"].ToString();
                        postal_code         = item["postal_code"].ToString();
                        dob                 = item["dob"].ToString();
                        identity_exp        = item["identity_exp"].ToString();
                        receiver_gender     = item["gender"].ToString();

                        if (identity_exp == "")
                        {
                            identity_exp = DateTime.Now.ToString();
                        }

                        if (dob == "")
                        {
                            dob = DateTime.Now.ToString();
                        }

                        message          = item["message"].ToString();
                        ref_no           = item["ext_ref"].ToString();

                        trans_date = item["trans_date"].ToString();

                        upLine1   = item["upLine1"].ToString();
                        upLine2   = item["upLine2"].ToString();
                        upLine3   = item["upLine3"].ToString();
                        upLine4   = item["upLine4"].ToString();
                        downLine1 = item["downLine1"].ToString();
                        downLine2 = item["downLine2"].ToString();
                        downLine3 = item["downLine3"].ToString();

                        int phLength = phone_number.Length;
                        spc = "";
                        for (y = 0; y < phLength-6; y++)
                        {
                            spc += "x";
                        }

                        if (phone_number != "")
                        {
                            phone_number_print = spc + phone_number.Substring(phLength - 6, 6);
                        }

                        int idNoLength = identity_number.Length;

                        spc = "";
                        for (y = 0; y < idNoLength - 6; y++)
                        {
                            spc += "x";
                        }

                        if (identity_number != "")
                        {
                            identity_no_print = spc + identity_number.Substring(idNoLength - 6, 6);
                        }
                                         
                        if (responseCode == "00")
                        {
                            
                            lblResponse.Text = "Inquiry Sucess";

                            btnProcess.Visible = true;
                            btnProcess.Enabled = true;
                            txtTPTCode.Enabled = false;

                       //     txtReceiverGender.Enabled = true;
                            rbGenderF.Enabled = true;
                            rbGenderM.Enabled = true;
                            txtIdentityExp.Enabled = true;
                            txtPoB.Enabled = true;
                            txtDoB.Enabled = true;
                            txtSubDistrict.Enabled = true;
                            txtPostalCode.Enabled = true;                         
                            txtAddress.Enabled = true;

                            cbPurpose.Enabled = true;
                            cbOccupation.Enabled = true;
                           

                        }
                        else if (responseCode == "2007") // already process
                        {  
                            btnCopyReceipt.Visible = true;
                            btnCopyReceipt.Enabled = true;
                            txtTPTCode.Enabled = false;
                            lblResponse.Text = item["message"];

                      //      txtReceiverGender.Enabled = false;
                            rbGenderF.Enabled = false;
                            rbGenderM.Enabled = false;

                            identity_number = identity_no_print;
                            phone_number    = phone_number_print;

                            txtIdentityExp.Enabled = false;
                            txtPoB.Enabled = false;
                            txtDoB.Enabled = false;
                            txtSubDistrict.Enabled = false;
                            txtPostalCode.Enabled = false;
                            txtAddress.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show(item["message"], "Error Check PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            btnProcess.Enabled = false;
                            btnProcess.Visible = false;
                            lblResponse.Text = item["message"];
                        }

                        txtAmount.Text = amount;
                        txtReceiverName.Text = receiver_name;
                        txtFee.Text = fee;


                        lblTransId.Text = transaction_id;
                        lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", total_amount);

                        txtCityName.Text        = city_name;
                        txtAddress.Text         = address;
                        txtPurposeId.Text       = trans_purpose_id;
                        txtOccupationName.Text  = occupation_name;
                        txtReceiverName.Text    = receiver_name;
                        txtOccupationId.Text    = occupation_id;
                        txtPhoneNumber.Text     = phone_number;
                        txtIdentityNo.Text      = identity_number;
                        txtPurposeName.Text     = trans_purpose_name;
                        txtCityId.Text          = city_id;
                        txtIdentityType.Text    = identity_type;


                        txtSubDistrict.Text  = subdistrict;
                        txtPoB.Text          = pob;
                        txtPostalCode.Text   = postal_code;
                        txtDoB.Value         = Convert.ToDateTime(dob);
                        txtIdentityExp.Value = Convert.ToDateTime(identity_exp);

                        if (receiver_gender == "M")
                        {
                            rbGenderM.Checked = true;
                        }
                        else if (receiver_gender == "F")
                        {
                            rbGenderF.Checked = true;
                        }

                        cbOccupation.SelectedValue  = occupation_id;
                        cbPurpose.SelectedValue     = trans_purpose_id;

                    }
                }
                else
                {
                    MessageBox.Show("TPT Code must be 10 digit", "Error Check PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {                
                MessageBox.Show("TPT Code must be numeric", "Error Check PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            lblResponse.Text = "Processing...";
      
         //   Random rand = new Random();
            string responseCode  = "0";       
            string message       = "";      
            string response_time = "";
            string amount        = "";
            string fee           = "";
            string total_amount  = "";
         //   string expired = "";
            string customer_name = "";
            string transId       = "";
            string reference_no  = "";
            
            string tpt_code         = txtTPTCode.Text;
            transaction_id          = lblTransId.Text;
            string OPERATOR_ID      = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
           

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;

            string reqCheckUrl = LV_TPT_PROCESS_URL;

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes("");

            HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
            reqCheck.Method         = "POST";
            reqCheck.Credentials    = CredentialCache.DefaultCredentials;
            reqCheck.Accept         = "text/json";
           
            reqCheck.Headers["Authorization"]   = "PFM";
       
            reqCheck.UserAgent      = USER_AGENT;
            reqCheck.ContentType    = "application/json";
       
            if (rbGenderM.Checked)
            {
                receiver_gender = "M";
            }
            else if (rbGenderF.Checked)
            {
                receiver_gender = "F";
            }

            receiver_name       = txtReceiverName.Text;
          //  receiver_gender     = txtReceiverGender.Text;
            phone_number        = txtPhoneNumber.Text;
            identity_type       = txtIdentityType.Text;
            identity_number     = txtIdentityNo.Text;
            identity_exp        = txtIdentityExp.Value.ToString("yyyy-MM-dd");
            address             = txtAddress.Text;
            dob                 = txtDoB.Value.ToString("yyyy-MM-dd");
            pob                 = txtPoB.Text;
       //     trans_purpose_id    = txtPurposeId.Text;
       //     occupation_id       = txtOccupationId.Text;
            postal_code         = txtPostalCode.Text;
            district            = txtCityName.Text;
            subdistrict         = txtSubDistrict.Text;

          //  occupation_id       = ((KeyValuePair<string, string>)cbOccupation.SelectedItem).Key;
        //    trans_purpose_id    = ((KeyValuePair<string, string>)cbPurpose.SelectedItem).Key;

            occupation_id = ((DataRowView)cbOccupation.SelectedItem)["Occupation_Id"].ToString();
            trans_purpose_id = ((DataRowView)cbPurpose.SelectedItem)["Purpose_Id"].ToString();

            /*if (occupation_id == "")
            {
                occupation_id = "1";
            }
            */

            if (occupation_id == "0")
            {
                MessageBox.Show("Pekerjaan Penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (trans_purpose_id == "0")
            {
                MessageBox.Show("Tujuan Penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtDoB.Value.Date > txtIdentityExp.Value.Date)
            {
                MessageBox.Show("Tanggal Lahir tidak boleh lebih kecil dari masa berlaku KTP/SIM", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (receiver_name == "")
            {
                MessageBox.Show("Nama Penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if( identity_number == "")
            {
                MessageBox.Show("No KTP / SIM harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if( pob == "")
            {
                MessageBox.Show("Tempat lahir harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(postal_code == "")
            {
                MessageBox.Show("Kode Pos penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if( subdistrict == "")
            {
                MessageBox.Show("Kecamatan / Kelurahan penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(receiver_gender == "")
            {
                MessageBox.Show("Jenis Kelamin harus dipilih", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(address == "")
            {  
                MessageBox.Show("Alamat penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(district == "")
            {
                MessageBox.Show("Kota penerima harus diisi", "Error Process Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string requestBody = string.Format("{{\"store_code\":\"{0}\",\"data_area\":\"{1}\",\"terminal_id\":\"{2}\",\"transaction_id\":\"{3}\",\"mtcn_code\":\"{4}\",\"operator_id\":\"{5}\",", ApplicationSettings.Database.StoreID, ApplicationSettings.Database.DATAAREAID, ApplicationSettings.Database.TerminalID, transaction_id, tpt_code, OPERATOR_ID);

                    requestBody += string.Format("\"receiver_name\":\"{0}\",", receiver_name);
                    requestBody += string.Format("\"receiver_gender\":\"{0}\",", receiver_gender);
                    requestBody += string.Format("\"receiver_phone_number\":\"{0}\",", phone_number);
                    requestBody += string.Format("\"receiver_identity_type\":\"{0}\",", identity_type);
                    requestBody += string.Format("\"receiver_identity_number\":\"{0}\",", identity_number);
                    requestBody += string.Format("\"receiver_identity_exp\":\"{0}\",", identity_exp);
                    requestBody += string.Format("\"receiver_address\":\"{0}\",", address);
                    requestBody += string.Format("\"receiver_dob\":\"{0}\",", dob);
                    requestBody += string.Format("\"receiver_pob\":\"{0}\",", pob);
                    requestBody += string.Format("\"receiver_purpose\":\"{0}\",", trans_purpose_id);
                    requestBody += string.Format("\"receiver_occupation\":\"{0}\",", occupation_id);
                    requestBody += string.Format("\"receiver_postal_code\":\"{0}\",", postal_code);
                    requestBody += string.Format("\"receiver_district\":\"{0}\",", district);
                    requestBody += string.Format("\"receiver_sub_district\":\"{0}\"}}", subdistrict);

                    byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

                    Stream requestStream = reqCheck.GetRequestStream();
                    requestStream.Write(byteArray, 0, byteArray.Length);
                    requestStream.Close();

                    using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
                    {
                        string content = string.Empty;
                        using (var stream = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                content = sr.ReadToEnd();
                            }
                        }

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        dynamic item = serializer.Deserialize<object>(content);

                        responseCode = item["rc"].ToString();
                        transId = item["transaction_id"].ToString();
                        response_time = item["response_time"].ToString();
                        amount = item["transaction_amount"].ToString();
                        fee = item["transaction_fee"].ToString();
                        //   total_amount    = item["total_amount"].ToString();
                        total_amount = item["transaction_amount"].ToString();
                        customer_name = item["receiver_name"].ToString();
                        reference_no = item["ext_ref"].ToString();
                        message = item["message"].ToString();
                        trans_date = item["trans_date"].ToString();

                        if (responseCode == "00")
                        {
                            txtReceiverName.Text = customer_name;
                            txtFee.Text = fee;
                            txtAmount.Text = total_amount;
                            lblTransId.Text = transId;

                            txtRefNo.Visible = true;
                            lblRefNo.Visible = true;

                            lblTotalAmount.Text = "Total Amount : Rp " + string.Format("{0:#,0.00}", total_amount);

                            MessageBox.Show("TPT Sucess", "Alert TPT", MessageBoxButtons.OK);

                            btnProcess.Enabled = true;
                            btnProcess.Visible = true;
                            lblResponse.Text = "Cash Out Sucess";

                            txtRefNo.Text = reference_no;

                            string s = this.ReceiptDocumentFormat("ori");

                            PrintDocument p = new PrintDocument();
                            PrintDialog pd = new PrintDialog();
                            PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
                            Margins margins = new Margins(0, 0, 0, 0);

                            pd.Document = p;
                            pd.Document.DefaultPageSettings.PaperSize = psize;
                            pd.Document.DefaultPageSettings.Margins = margins;
                            p.DefaultPageSettings.PaperSize.Width = 600;
                            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                            {
                                e1.Graphics.DrawString(s, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                            };

                            try
                            {
                                p.Print();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Exception Occured While Printing", ex);
                            }

                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show(item["message"], "Error Process PaymentLove", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            btnProcess.Enabled = false;
                            btnProcess.Visible = false;

                            lblResponse.Text = "";
                        }


                        resetForm();
                    }
                }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numCurrNumpad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '.')
            {
                e.Handled = false;
            }
        }

        private void CPLoveRemittPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            timerCountdown.Stop();
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void resetForm()
        {
            btnProcess.Enabled = false;
            btnProcess.Visible = false;

            btnCopyReceipt.Visible = false;
            btnCopyReceipt.Enabled = false;
            txtTPTCode.Text = "";

            txtAddress.Text = "";
            txtReceiverName.Text = "";
            txtFee.Text = "";
            txtAmount.Text = "";

            lblTransId.Text = "";
            lblTotalAmount.Text = "Total Amount : Rp ";

            txtRefNo.Text = "";
            lblResponse.Text = "";
        }

        private void btnCopyReceipt_Click(object sender, EventArgs e)
        {
      
            string s = this.ReceiptDocumentFormat("copy");

            PrintDocument p = new PrintDocument();
            PrintDialog pd  = new PrintDialog();
            PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
            Margins margins = new Margins(0, 0, 0, 0);

            pd.Document = p;
            pd.Document.DefaultPageSettings.PaperSize = psize;
            pd.Document.DefaultPageSettings.Margins = margins;
            p.DefaultPageSettings.PaperSize.Width = 600;
            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(s, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };

            try
            {
                p.Print();

                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private string ReceiptDocumentFormat(string type)
        {
            
            string storeId=  ApplicationSettings.Database.StoreID;
            string operator_name = ApplicationSettings.Terminal.TerminalOperator.Name;

            try
            {
                s += " " + upLine1 + Environment.NewLine +
                     " " + upLine2 + Environment.NewLine +
                     " " + upLine3 + Environment.NewLine +
                     " " + upLine4 + Environment.NewLine +                    
                     "                      CABANG:" + ApplicationSettings.Terminal.StoreName + Environment.NewLine ;
                
                s += "             -----------------------------------" + Environment.NewLine ;
                s += "                                " +trans_date + Environment.NewLine;
                if (type == "copy")
                {
                s += "             Copy Receipt                 "+ Environment.NewLine;
                }
                else
                {
                s += "                                      " + Environment.NewLine;
                }

                s += "             TransactionID: " + transaction_id + Environment.NewLine +
                     "             staff:" + ApplicationSettings.Terminal.TerminalOperator.OperatorId +"-"+operator_name+Environment.NewLine + Environment.NewLine;

                s += "             Kode Pencairan: " + tpt_code + Environment.NewLine;
                s += "             Ref No: " + ref_no + Environment.NewLine + Environment.NewLine;
                s += "             Details" + Environment.NewLine;
                s += "             Receiver Name: " + receiver_name + Environment.NewLine;
                s += "             KTP/SIM: " + identity_no_print + Environment.NewLine;
                s += "             No HP: " + phone_number_print + Environment.NewLine;

                x = 15 - amount.Length;

                spc = "";

                for (y = 0; y < x ;y++)
                {
                    spc += " ";
                }

                s += "             Amount Transaction: " + spc + amount + Environment.NewLine;

                x = 35 - 12 - fee.Length;
                spc = "";
                for (y = 0; y < x; y++)
                {
                    spc += " ";
                }

                s += "             -----------------------------------" + Environment.NewLine;

                s += " "+downLine1 + Environment.NewLine +
                     " "+downLine2 + Environment.NewLine +
                     " "+downLine3 + Environment.NewLine;


                Offset = Offset + 260;
            }
            catch (Exception ex)
            {
                throw new Exception("Format Error", ex);
            }

            return s;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCustName_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void rbGenderM_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGenderM.Checked)
            {
                rbGenderF.Checked = false;
            }
        }

        private void rbGenderF_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGenderF.Checked)
            {
                rbGenderM.Checked = false;
            }
        }

        public class Occoupations
        {
            public string occupation_id { get; set; }            
            public string occupation_text { get; set; }
        }

        public class Purposes
        {
            public string purpose_id { get; set; }             
            public string purpose_text { get; set; }
        }

        public class Root
        {
            public List<Occoupations> occupation { get; set; }
            public List<Purposes> purpose { get; set; }
        }

        public DataTable getOccupation()
        {
            string terminalId = ApplicationSettings.Database.TerminalID;
            string store_code = ApplicationSettings.Database.StoreID;
           
            DataTable dt = new DataTable();


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                             SecurityProtocolType.Tls11 |
                                             SecurityProtocolType.Tls12;

            string reqCheckUrl = LV_GET_OCCUPATION_URL;

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes("");

            HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
            reqCheck.Method = "POST";
            reqCheck.Credentials = CredentialCache.DefaultCredentials;
            reqCheck.Accept = "text/json";

            reqCheck.Headers["Authorization"] = "PFM";

            reqCheck.UserAgent = USER_AGENT;
            reqCheck.ContentType = "application/json";

            string requestBody = string.Format("{{\"store_code\":\"{0}\",\"data_area\":\"{1}\",\"terminal_id\":\"{2}\",\"operator_id\":\"{3}\"}}", ApplicationSettings.Database.StoreID, ApplicationSettings.Database.DATAAREAID, terminalId, ApplicationSettings.Terminal.TerminalOperator.OperatorId);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            Stream requestStream = reqCheck.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
            {
                string content = string.Empty;
                using (var stream = resp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                Root myDeserializedClass = new JavaScriptSerializer().Deserialize<Root>(content);

                dt.Columns.Add("Occupation_Id");
                dt.Columns.Add("Occupation_Text");
                
                DataRow dr = null;

                dr = dt.NewRow(); // have new row on each iteration
                dr["Occupation_Id"] = "0";
                dr["Occupation_Text"] = "Please Select";
                dt.Rows.Add(dr);               
                
                if (myDeserializedClass.occupation != null)
                {
                    foreach (var occupations in myDeserializedClass.occupation)
                    {
                        dr = dt.NewRow(); // have new row on each iteration
                        dr["Occupation_Id"] = occupations.occupation_id;
                        dr["Occupation_Text"] = occupations.occupation_text;                       
                        dt.Rows.Add(dr);                      
                    }
                }

            }

            return dt;
        }

        public DataTable getPurpose()
        {
            string terminalId = ApplicationSettings.Database.TerminalID;
            string store_code = ApplicationSettings.Database.StoreID;

            DataTable dt_p = new DataTable();


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                             SecurityProtocolType.Tls11 |
                                             SecurityProtocolType.Tls12;

            string reqCheckUrl = LV_GET_PURPOSE_URL;

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes("");

            HttpWebRequest reqCheck = (HttpWebRequest)WebRequest.Create(reqCheckUrl);
            reqCheck.Method = "POST";
            reqCheck.Credentials = CredentialCache.DefaultCredentials;
            reqCheck.Accept = "text/json";

            reqCheck.Headers["Authorization"] = "PFM";

            reqCheck.UserAgent = USER_AGENT;
            reqCheck.ContentType = "application/json";

            string requestBody = string.Format("{{\"store_code\":\"{0}\",\"data_area\":\"{1}\",\"terminal_id\":\"{2}\",\"operator_id\":\"{3}\"}}", ApplicationSettings.Database.StoreID, ApplicationSettings.Database.DATAAREAID, terminalId, ApplicationSettings.Terminal.TerminalOperator.OperatorId);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            Stream requestStream = reqCheck.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            using (HttpWebResponse resp = (HttpWebResponse)reqCheck.GetResponse())
            {
                string content = string.Empty;
                using (var stream = resp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                Root myDeserializedClass = new JavaScriptSerializer().Deserialize<Root>(content);

                dt_p.Columns.Add("Purpose_Id");
                dt_p.Columns.Add("Purpose_Text");

                DataRow dr = null;

                dr = dt_p.NewRow(); // have new row on each iteration
                dr["Purpose_Id"] = "0";
                dr["Purpose_Text"] = "Please Select";
                dt_p.Rows.Add(dr);

                if (myDeserializedClass.purpose != null)
                {
                    foreach (var purposes in myDeserializedClass.purpose)
                    {
                        dr = dt_p.NewRow(); // have new row on each iteration
                        dr["Purpose_Id"] = purposes.purpose_id;
                        dr["Purpose_Text"] = purposes.purpose_text;
                        dt_p.Rows.Add(dr);
                    }
                }

            }

            return dt_p;
        }

        private void txtTPTCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
