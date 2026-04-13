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

namespace Microsoft.Dynamics.Retail.Pos.PurchaseOrderReceiving.WinFormsTouch.CP_OTPForm
{
    public partial class CP_OTPForm : Form
    {
        public bool IsValidOTP { get; set; }
        string poNumber = "";
        public CP_OTPForm(string _poNumber)
        {
            InitializeComponent();
            poNumber = _poNumber;
            lblError.Text = "";
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(APIAccess.APIAccessClass.codeOTP) && txtCodeOTP.Text == APIAccess.APIAccessClass.codeOTP)
            //get OTP from AX
            ReadOnlyCollection<object> containerArray;
            containerArray = PurchaseOrderReceiving.InternalApplication.TransactionServices.InvokeExtension("getOtpPO", poNumber);
            string otpPO = containerArray[3].ToString();;
            if (txtCodeOTP.Text == otpPO) 
            {
                IsValidOTP = true;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                IsValidOTP = false;
                lblError.Text = "OTP salah";
            }
        }
    }
}
