/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.Common;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts;

using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_EngageFALWSServices;
using System.Net.Http;

using System.Timers;
using System.Text.RegularExpressions;
using QRCoder;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;
using System.Drawing.Printing;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
	/// <summary>
	/// Summary description for frmPayCustomerAccount.
	/// </summary>
	[Export("PayCustomerAccountForm", typeof(IInteractionView))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class frmPayCustomerAccount : frmTouchBase, IInteractionView
	{
		#region Class variables
		private decimal registeredAmount;
		private decimal amount;
		private string customerID;
		private decimal balanceAmount;
		private PosTransaction posTransaction;
		private int controlIndex;
		private decimal? proposedPaymentAmount;
		private string approvalCode;
		#endregion

		#region Form Components

		private Tender tenderInfo;
		private PanelControl panelControl1;
		private TableLayoutPanel tableLayoutPanel1;
		private Label labelAmountDue;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
		private FlowLayoutPanel flowLayoutPanel1;
		private Label labelAmountDueValue;
		private PanelControl panelCustomer;
		private LSRetailPosis.POSProcesses.WinControls.NumPad numPad1;
		private PanelControl panelControl2;
		private LSRetailPosis.POSProcesses.WinControls.AmountViewer amtCustAmounts;
		private Label labelPaymentAmount;
		private TableLayoutPanel tableLayoutPanel3;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
		private TableLayoutPanel tableLayoutPanel4;
		//custom by Yonathan
		private TableLayoutPanel tableLayoutPanelQRIS = new TableLayoutPanel();
		//end
		#endregion
		//private StyleController styleController;
		private TableLayoutPanel tableLayoutPanel2;
		private Label labelAmount;
		private Label lblReff;
		private TextBox txtReff;
		private Label lblPhone;
		private TextBox txtPhone;
		private Label lblCustName;
		private TextBox txtCustName;
		//private StyleController styleController;
		private Label lblCustomerId;
		private Label lblCustomerName;
		private TextBox labelAmountValue;
		private System.Windows.Forms.ComboBox cmbPilihBank;
		private Button btnRequest;
		//private StyleController styleController;
		private System.ComponentModel.IContainer components;
		//public Button btnQRIS;
		private Button btnResponse;
		private StyleController styleController;
		private Button btnInquiry;
		private Label txtNMID;
		//add by Yonathan custom QRIS
		public string refNumber;
		public int countInquiryQR;
		public int timeOutRequest;
		private readonly System.Windows.Forms.Timer timerCount = new System.Windows.Forms.Timer();
		private Label lblWaitingRespond;
		public string transType;
		public int tenderQRIS = 30; //prod 30, dev 31
        public int tenderGrabMartDev = 19;//for Grabmart DEV by Yonathan 10/11/2023
		public int tenderGrabMart = 16;//for Grabmart PROD by Yonathan 10/11/2023 
		public int tenderShopee = 15;
		public int tenderShopeeDev = 15;
		private PictureBox placeHolderQR;
		public bool isIntegrated = false;
		public Bitmap croppedQRCodeImage;
		bool timesUp = false;
        public int retryCountGrab = 0;


		public string urlCreate, urlInvalidate, urlNotify;

		private DateTime targetTime;

		APIAccess.APIParameter.parmResponseShopeePay responseShopeePay;
		APIAccess.APIParameter.ListResultDataInquiry listData;
		//

		#region Properties

		static AutoResetEvent autoEventBCA = new AutoResetEvent(false);
		private bool _isReceived { get; set; }
		private bool _paymentSuccess { get; set; }
		static string _LastResponBCA { get; set; }
		static bool isEDCSettlementBCA { get; set; }

		public string CustomerID
		{
			get
			{
				return customerID;
			}
			set
			{
				customerID = (value == null) ? string.Empty : value;
			}
		}

		public decimal Amount
		{
			get
			{
				return amount;
			}
			set
			{
				try
				{
					amount = value;
					labelAmountValue.Text = PosApplication.Instance.Services.Rounding.Round(amount, false);
					#region CPECRBCA
					//set button request enabled when amount is filled
					btnRequest.Enabled = true;
					#endregion
				}
				catch (Exception)
				{
				}
			}
		}

		public decimal RegisteredAmount
		{
			get
			{
				return registeredAmount;
			}
			set
			{
				if (registeredAmount == value)
				{
					return;
				}

				registeredAmount = value;
			}
		}

		public PosTransaction PosTransaction
		{
			set
			{
				posTransaction = value;
				RetailTransaction retailPosTransaction = (RetailTransaction)posTransaction;

				if (!retailPosTransaction.InvoicedCustomer.IsEmptyCustomer())
				{
					this.CustomerID = retailPosTransaction.InvoicedCustomer.CustomerId;
				}
				else if (!retailPosTransaction.Customer.IsEmptyCustomer())
				{
					this.CustomerID = retailPosTransaction.Customer.CustomerId;
				}

				SetCustomerDisplayText(retailPosTransaction);
			}
		}

		#endregion

		#region Constructor and Destructor

		protected frmPayCustomerAccount()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			  
		}

		private void timerCount_Tick(object sender, EventArgs e)
		{
			timerCount.Stop();
			btnCancel.Enabled = true;
			//throw new NotImplementedException();
		}

		[ImportingConstructor]
		public frmPayCustomerAccount(PayCustomerAccountConfirmation payCustomerAccountConfirmation)
			: this()
		{
			if (payCustomerAccountConfirmation == null)
			{
				throw new ArgumentNullException("payCustomerAccountConfirmation");
			}
			this.tenderInfo = (Tender)payCustomerAccountConfirmation.TenderInfo;
			this.proposedPaymentAmount = payCustomerAccountConfirmation.ProposedPaymentAmount;

			lblCustomerId.Text = "";
			lblCustomerName.Text = "";

			amtCustAmounts.SoldLocalAmount =
				PosApplication.Instance.Services.Rounding.RoundAmount(
				payCustomerAccountConfirmation.ProposedPaymentAmount != null
				? (decimal)payCustomerAccountConfirmation.ProposedPaymentAmount
				: payCustomerAccountConfirmation.BalanceAmount,
				ApplicationSettings.Terminal.StoreId, tenderInfo.TenderID);
			this.balanceAmount = payCustomerAccountConfirmation.BalanceAmount;
			this.PosTransaction = (PosTransaction)payCustomerAccountConfirmation.PosTransaction;

			//MessageBox.Show("Tender ID : " + this.tenderInfo.TenderID);
			//MessageBox.Show("Tender Name : " + this.tenderInfo.TenderName);
			//MessageBox.Show("Customer ID : " + this.customerID);

			//MessageBox.Show("Tender Info : " + this.tenderInfo.PosisOperation);
			//MessageBox.Show("Proposed Payment Amount : " + this.proposedPaymentAmount);
			//MessageBox.Show("Balance Amount : " + this.balanceAmount);
			//MessageBox.Show("Pos Transaction : " + this.posTransaction);
		}


		 
		protected override void OnLoad(EventArgs e)
		{

			if (!this.DesignMode)
			{
				isIntegrated = validateIntegration();
				amtCustAmounts.LocalCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;
				amtCustAmounts.UsedCurrencyCode = ApplicationSettings.Terminal.StoreCurrency;

				if (tenderInfo.OverTenderAllowed)
				{
					amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.HigherAmounts;
				}
				else
				{
					amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;
				}

				amtCustAmounts.HighestOptionAmount = tenderInfo.MaximumAmountAllowed;
				amtCustAmounts.LowesetOptionAmount = tenderInfo.MinimumAmountAllowed;
				amtCustAmounts.SetButtons();

				amount = 0;
				registeredAmount = 0;
				controlIndex = 1;
				SetButtonStatus();

				//
				// Get all text through the Translation function in the ApplicationLocalizer
				//
				// TextID's for frmPayCustomerAccount are reserved at 1440 - 1459
				// In use now are ID's 1440 - 1451
				//                

				btnCancel.Text = ApplicationLocalizer.Language.Translate(1440); //Cancel
				btnOk.Text = ApplicationLocalizer.Language.Translate(1201);//Ok
				//labelAmount.Text = ApplicationLocalizer.Language.Translate(1441); //Amount
				labelPaymentAmount.Text = ApplicationLocalizer.Language.Translate(1442); //Payment amount

				//this.Text = ApplicationLocalizer.Language.Translate(1445); //Customer account payment
				labelAmountDue.Text = ApplicationLocalizer.Language.Translate(1450); //Total amount due:
				labelAmountDueValue.Text = "IDR " + PosApplication.Instance.Services.Rounding.RoundAmount(this.balanceAmount, ApplicationSettings.Terminal.StoreId, this.tenderInfo.TenderID, true);
				//btnCustomerSearch.Text = ApplicationLocalizer.Language.Translate(1447); //Change customer
				//labelCustomerId.Text = ApplicationLocalizer.Language.Translate(1448); //Customer id
				//labelCustomerName.Text = ApplicationLocalizer.Language.Translate(1449); //Customer Name
				//labelAccountInfo.Text = ApplicationLocalizer.Language.Translate(1451); //Account information


				numPad1.SetEnteredValueFocus();


				if (labelAmountValue.Text.Length != 0)
				{
					controlIndex = 2;
				}
				/*else if ((labelCustomerIdValue.Text.Length != 0) && (labelCustomerNameValue.Text.Length != 0))
				{
					controlIndex = 1;
				}*/
				else
				{
					controlIndex = 0;
				}

				SetButtonStatus();
			}

			if (this.customerID == null)
			{
				using (frmMessage dialog = new frmMessage("Must Choose Customer to Continue this Operation", MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					POSFormsManager.ShowPOSForm(dialog);
					Close();
				}
			}
			else
			{
				#region CPLOCKCUSTOMER
				//validate customer assigned based on tender
				if (!validateCustomer())
				{
					using (frmMessage dialog = new frmMessage("Please Choose Customer for Tender " + this.tenderInfo.TenderName, MessageBoxButtons.OK, MessageBoxIcon.Stop))
					{
						POSFormsManager.ShowPOSForm(dialog);
						Close(); 
					}
				}
				#endregion
			}

			#region CPECRBCA
			/*
			 * validate section for ECR tender only
			 * 32 : Debit ECR
			 * 33 : Credit ECR
			 * Add in this if when there is a new tender
			 */
			if (
				int.Parse(this.tenderInfo.TenderID) == 32
				|| int.Parse(this.tenderInfo.TenderID) == 33
				|| int.Parse(this.tenderInfo.TenderID) == tenderQRIS) //add by Yonaathan 10/01/2023 31 for QR
			{
				/*
				 * Check for connection to AOS
				 * timeout: 75 seconds
				 * return error No Internet Connection when still cannot connect after reaching timeout
				 */
				try
				{
					if (!PosApplication.Instance.TransactionServices.CheckConnection())
					{
						using (frmMessage dialog = new frmMessage("No Internet Connection (1)", MessageBoxButtons.OK, MessageBoxIcon.Stop))
						{
							POSFormsManager.ShowPOSForm(dialog);
							Close();
						}
					}
				}
				catch (Exception ex)
				{
					using (frmMessage dialog = new frmMessage("No Internet Connection (2)", MessageBoxButtons.OK, MessageBoxIcon.Stop))
					{
						POSFormsManager.ShowPOSForm(dialog);
						Close();
					}
				}

				//Design Form to match ECR Template

				//set form ECR true
				cmbPilihBank.Visible = true;

				/*
				 * change ref number & phone number to field card number & ambil tunai
				 * txtReff will act as Card Number for payment ECR
				 * txtPhone will act as Ambil Tunai Amount for payment ECR
				 * Set field to ReadOnly (as data will be fetch from ECR Result)
				 */
				lblReff.Text = "Card Number";
				lblPhone.Text = "Ambil Tunai";
				txtReff.ReadOnly = true;
				txtPhone.ReadOnly = true;
				btnRequest.Visible = true;
				btnRequest.Enabled = false;
				btnOk.Enabled = false;

				//set Unused field to Invisible
				lblCustName.Visible = false;
                //lblCustName.Text = "Biaya Tarik Tunai";
                txtCustName.Visible = false; //set true for bank adm fee 10/06/2024 Yonathan //CPIADMFEE
                
				//custom by Yonathan 2 Dec 2022 custom QRIS 
				btnInquiry.Visible = false;
				//btnQRIS.Visible = false;
				//end

				//add by Yonathan 1/Dec/2022 custom QRIS
				if (int.Parse(this.tenderInfo.TenderID) == tenderQRIS)
				{
					btnRequest.Text = "Request to QRIS";
					btnInquiry.Visible = true;
					btnInquiry.Text = "Inquiry QRIS";
					lblReff.Visible = false;
					lblPhone.Visible = false;
					txtReff.Visible = false;
					lblReff.Text = "Reference Number";
					txtPhone.Visible = false;
					btnInquiry.Enabled = false;
					 
					
				}

				//if (int.Parse(this.tenderInfo.TenderID) == tenderShopee)
				//{
				//    btnRequest.Text = "Generate QR";
				//    btnInquiry.Visible = true;
				//    btnInquiry.Text = "Inquiry QR";
				//    lblReff.Visible = false;
				//    lblPhone.Visible = false;
				//    txtReff.Visible = false;
				//    lblReff.Text = "Reference Number";
				//    txtPhone.Visible = false;
				//    btnInquiry.Enabled = false;
					
				//}
				//end 
				/*
				 * Get Bank Data from Local Database
				 * Used for Dropdown when choosing Bank and COMPORT
				 */
				string queryStringStore = "";
				SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{
					if (int.Parse(this.tenderInfo.TenderID) == tenderQRIS) //add by Yonathan to differentiate query between QRIS and debit ECR
					{
						queryStringStore = @"SELECT BANK, PORT FROM AX.CPPILIHBANK WHERE BANK LIKE 'QRIS%' ";
					}
					else
					{
						queryStringStore = @"SELECT BANK, PORT FROM AX.CPPILIHBANK WHERE BANK NOT LIKE 'QRIS%'"; 
					} 

					using (SqlCommand command = new SqlCommand(queryStringStore, connectionStore))
					{
						if (connectionStore.State != ConnectionState.Open)
						{
							connectionStore.Open();
						}

						//Assigned Result to Dropdown
						SqlDataAdapter d = new SqlDataAdapter(queryStringStore, connectionStore);
						DataTable dt = new DataTable();
						d.Fill(dt);

						cmbPilihBank.DataSource = dt;
						cmbPilihBank.DisplayMember = "BANK";
						cmbPilihBank.ValueMember = "PORT";
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					throw;
				}
				finally
				{
					if (connectionStore.State != ConnectionState.Closed)
					{
						connectionStore.Close();
					}
				}
			}
			/*else if (int.Parse(this.tenderInfo.TenderID) == 31) //if this is QR //custom by Yonathan 2 Dec 2022 custom QRIS
			{
				//prepare form for other payment (default as is)
				cmbPilihBank.Visible = false;
				btnRequest.Visible = false;

				lblReff.Text = "Reference No.";
				lblPhone.Text = "Phone No.";
				txtReff.ReadOnly = false;
				txtPhone.ReadOnly = false;
				lblCustName.Visible = true;
				txtCustName.Visible = true;
				btnRequest.Visible = true;
				btnResponse.Visible = true;
				btnQRIS.Visible = true;
			}*/
			else if ((int.Parse(this.tenderInfo.TenderID) == tenderShopee
				|| int.Parse(this.tenderInfo.TenderID) == tenderShopeeDev) && isIntegrated == true)
			{
				getURLAPI(); //disable temporarily for testing JOFFICE 11092024

                //temp code to define URL for SHOPEE
                //urlCreate = "https://partnerpfm.cp.co.id/api/shopeePay/snap/createDynamicQR";//reader["URLCREATE"].ToString();
                //urlInvalidate = "https://partnerpfm.cp.co.id/api/shopeePay/snap/QRInvalidate"; //reader["URLINVALIDATE"].ToString();
                //urlNotify = "https://partnerpfm.cp.co.id/api/shopeePay/snap/manualPaymentStatus";//reader["URLNOTIFY"].ToString();   
                //end
				//prepare form for other payment (default as is)
				cmbPilihBank.Visible = false;
				btnRequest.Visible = false;
				btnInquiry.Enabled = false;
				lblReff.Text = "Reference No.";                 
				txtReff.ReadOnly = false;
				tableLayoutPanelQRIS.Visible = true;
				numPad1.Visible = false;
				btnRequest.Visible = true;
				btnRequest.Text = "Generate QRIS";
				btnInquiry.Visible = true;

				lblPhone.Visible = false;
				txtPhone.Visible = false; 


				lblCustName.Visible = false;
				txtCustName.Visible = false;

				placeHolderQR.Visible = true;

				// Create the default white image
				Bitmap defaultImage = new Bitmap(310, 290); //310
				//using (Graphics g = Graphics.FromImage(defaultImage))
				//{ // Set the desired color
				//    //Color fillColor = Color.FromArgb(213, 222, 229);

				//    // Get the Graphics object
				//    g.Clear(Color.Gray);
				//    //g.FillRectangle(new SolidBrush(fillColor), 0, 0, 335, 335);
				//}
				txtReff.Text = this.posTransaction.TransactionId;
				//txtReff.Multiline = true;
				txtReff.ReadOnly = true;
				// Set the default white image to the PictureBox
				placeHolderQR.Image = defaultImage;
				//btnQRIS.Visible = false;
				// Get the text string from the TextBox
				//string textToEncode = "https://shopee.co.id/" 

				//// Create the QR code generator
				//QRCodeGenerator qrGenerator = new QRCodeGenerator();
				//QRCodeData qrCodeData = qrGenerator.CreateQrCode(textToEncode, QRCodeGenerator.ECCLevel.Q);
				//QRCode qrCode = new QRCode(qrCodeData);

				//// Convert the QR code into a bitmap image
				//Bitmap qrCodeImage = qrCode.GetGraphic(20);

				//// Display the QR code in the PictureBox
				//pictureBoxQR.Image = qrCodeImage;
				//txtNMID.Text = "NMID: ID123456789012345";
				//txtNMID.ReadOnly = true;
				txtNMID.Visible = true;
				btnOk.Enabled = false;
			}
			else if (int.Parse(this.tenderInfo.TenderID) == tenderGrabMart
				|| int.Parse(this.tenderInfo.TenderID) == tenderGrabMartDev)
			{


				if (isIntegrated == true && APIAccess.APIAccessClass.grabOrderState != "" )
				{
					if (APIAccess.APIAccessClass.grabCustPhone != "")
					{
						txtPhone.Text = APIAccess.APIAccessClass.grabCustPhone.ToString();//"";
						txtPhone.ReadOnly = true; //false; //tadinya false karena bisa diketik manual
					}


					if (APIAccess.APIAccessClass.grabCustName != "")
					{ 
						txtCustName.Text = APIAccess.APIAccessClass.grabCustName.ToString();// "";
						txtCustName.ReadOnly = true; //false; //tadinya false karena bisa diketik manual
					}

				}
				else if (isIntegrated == true && APIAccess.APIAccessClass.grabOrderState == "" )
				{
					using (frmMessage dialog = new frmMessage("Toko ini sudah terintegrasi dengan Grabmart.\nSilakan lakukan proses order dari menu Grabmart Order", MessageBoxButtons.OK, MessageBoxIcon.Stop))
					{
						POSFormsManager.ShowPOSForm(dialog);
						Close();
						pressCancel = "1";
						this.Close();
					}
				}
				string noOrder = getNoOrder(this.posTransaction.TransactionId);
				lblReff.Text = "Order Id.";
				txtReff.ReadOnly = true;
				txtReff.Text = noOrder;

				//prepare form for other payment (default as is)
				cmbPilihBank.Visible = false;
				btnRequest.Visible = false;                 
				lblPhone.Text = "Phone No.";
				
				
				lblCustName.Visible = true;
				txtCustName.Visible = true; 
				btnRequest.Visible = false;
				btnInquiry.Visible = false;
				numPad1.Enabled = false;
			}
			else
			{
				//prepare form for other payment (default as is)
				cmbPilihBank.Visible = false;
				btnRequest.Visible = false;

				lblReff.Text = "Reference No.";
				lblPhone.Text = "Phone No.";
				txtReff.ReadOnly = false;
				txtPhone.ReadOnly = false;
				lblCustName.Visible = true;
				txtCustName.Visible = true;
				btnRequest.Visible = false;
				btnInquiry.Visible = false;
				//btnQRIS.Visible = false;
			}

			#endregion

			base.OnLoad(e);
		}

		private string getNoOrder(string transactionId)
		{
			string NoOrder = "";
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{
				string queryString = @" SELECT NOORDER FROM [dbo].[CPOrderTable]
												WHERE TRANSACTIONID =  @TRANSACTIONID";

				using (SqlCommand command = new SqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@TRANSACTIONID", transactionId);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							NoOrder = reader.GetString(0);
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

			return NoOrder;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panelCustomer = new DevExpress.XtraEditors.PanelControl();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.txtCustName = new System.Windows.Forms.TextBox();
			this.lblCustName = new System.Windows.Forms.Label();
			this.txtPhone = new System.Windows.Forms.TextBox();
			this.labelAmountValue = new System.Windows.Forms.TextBox();
			this.lblPhone = new System.Windows.Forms.Label();
			this.txtReff = new System.Windows.Forms.TextBox();
			this.lblReff = new System.Windows.Forms.Label();
			this.labelAmount = new System.Windows.Forms.Label();
			this.lblCustomerId = new System.Windows.Forms.Label();
			this.lblCustomerName = new System.Windows.Forms.Label();
			this.numPad1 = new LSRetailPosis.POSProcesses.WinControls.NumPad();
			this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
			this.btnInquiry = new System.Windows.Forms.Button();
			this.btnRequest = new System.Windows.Forms.Button();
			this.amtCustAmounts = new LSRetailPosis.POSProcesses.WinControls.AmountViewer();
			this.labelPaymentAmount = new System.Windows.Forms.Label();
			this.cmbPilihBank = new System.Windows.Forms.ComboBox();
			this.lblWaitingRespond = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.labelAmountDue = new System.Windows.Forms.Label();
			this.labelAmountDueValue = new System.Windows.Forms.Label();
			this.placeHolderQR = new System.Windows.Forms.PictureBox();
			//add custom
			this.txtNMID = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelCustomer)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.placeHolderQR)).BeginInit();
			this.SuspendLayout();
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.tableLayoutPanel4);
			this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelControl1.Location = new System.Drawing.Point(0, 0);
			this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(1024, 732);
			this.panelControl1.TabIndex = 0;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(1020, 728);
			this.tableLayoutPanel4.TabIndex = 3;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnOk, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(30, 631);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(960, 66);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Appearance.Options.UseFont = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(484, 4);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(127, 57);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOk.Appearance.Options.UseFont = true;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(349, 4);
			this.btnOk.Margin = new System.Windows.Forms.Padding(4);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(127, 57);
			this.btnOk.TabIndex = 2;
			this.btnOk.Tag = "";
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click_1);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.panelCustomer, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.numPad1, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.panelControl2, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.cmbPilihBank, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblWaitingRespond, 2, 3);
			//this.tableLayoutPanel1.Controls.Add(this.placeHolderQR, 2, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(108, 166);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 500);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panelCustomer
			// 
			this.panelCustomer.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.panelCustomer.Controls.Add(this.tableLayoutPanel2);
			this.panelCustomer.Location = new System.Drawing.Point(44, 30);
			this.panelCustomer.Margin = new System.Windows.Forms.Padding(0);
			this.panelCustomer.Name = "panelCustomer";
			this.panelCustomer.Size = new System.Drawing.Size(200, 328);
			this.panelCustomer.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.txtCustName, 0, 10);
			this.tableLayoutPanel2.Controls.Add(this.lblCustName, 0, 9);
			this.tableLayoutPanel2.Controls.Add(this.txtPhone, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.labelAmountValue, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.lblPhone, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.txtReff, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.lblReff, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.labelAmount, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblCustomerId, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblCustomerName, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 11;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.33333F)); //Percent 33.33333F //customer id
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F)); //20 c
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F)); //43
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F)); 
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F)); //41F Absolute //reference textbox
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(196, 350); //324
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// txtCustName
			// 
			this.txtCustName.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtCustName.Location = new System.Drawing.Point(3, 290);
			this.txtCustName.Name = "txtCustName";
			this.txtCustName.Size = new System.Drawing.Size(190, 27);
			this.txtCustName.TabIndex = 12;
			// 
			// lblCustName
			// 
			this.lblCustName.AutoSize = true;
			this.lblCustName.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCustName.Location = new System.Drawing.Point(3, 260);
			this.lblCustName.Name = "lblCustName";
			this.lblCustName.Size = new System.Drawing.Size(119, 20);
			this.lblCustName.TabIndex = 11;
			this.lblCustName.Text = "Customer Name";
			// 
			// txtPhone
			// 
			this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtPhone.Location = new System.Drawing.Point(3, 225);
			this.txtPhone.Name = "txtPhone";
			this.txtPhone.Size = new System.Drawing.Size(137, 27);
			this.txtPhone.TabIndex = 10;
			this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
			// 
			// labelAmountValue
			// 
			this.labelAmountValue.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.labelAmountValue.Location = new System.Drawing.Point(3, 92);
			this.labelAmountValue.Name = "labelAmountValue";
			this.labelAmountValue.ReadOnly = true;
			this.labelAmountValue.Size = new System.Drawing.Size(139, 27);
			this.labelAmountValue.TabIndex = 4;
			// 
			// lblPhone
			// 
			this.lblPhone.AutoSize = true;
			this.lblPhone.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPhone.Location = new System.Drawing.Point(3, 198);
			this.lblPhone.Name = "lblPhone";
			this.lblPhone.Size = new System.Drawing.Size(82, 20);
			this.lblPhone.TabIndex = 9;
			this.lblPhone.Text = "Phone No.";
			// 
			// txtReff
			// 
			this.txtReff.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtReff.Location = new System.Drawing.Point(3, 160);
			this.txtReff.Name = "txtReff";
			this.txtReff.Size = new System.Drawing.Size(190, 50);
			this.txtReff.TabIndex = 8;
			this.txtReff.Multiline = true;
			this.txtReff.ScrollBars = ScrollBars.Vertical;
			this.txtReff.Height = 50;
			//this.txtReff.TextChanged += new System.EventHandler(txtReff_TextChanged);

			this.txtReff.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtReff_KeyPress);
			// 
			// lblReff
			// 
			this.lblReff.AutoSize = true;
			this.lblReff.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReff.Location = new System.Drawing.Point(3, 132);
			this.lblReff.Name = "lblReff";
			this.lblReff.Size = new System.Drawing.Size(106, 20);
			this.lblReff.TabIndex = 7;
			this.lblReff.Text = "Reference No.";
			// 
			// labelAmount
			// 
			this.labelAmount.AutoSize = true;
			this.labelAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
			this.labelAmount.Location = new System.Drawing.Point(3, 69);
			this.labelAmount.Name = "labelAmount";
			this.labelAmount.Size = new System.Drawing.Size(64, 20);
			this.labelAmount.TabIndex = 5;
			this.labelAmount.Text = "Amount";
			// 
			// lblCustomerId
			// 
			this.lblCustomerId.AutoSize = true;
			this.lblCustomerId.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
			this.lblCustomerId.Location = new System.Drawing.Point(3, 0);
			this.lblCustomerId.Name = "lblCustomerId";
			this.lblCustomerId.Size = new System.Drawing.Size(139, 18);
			this.lblCustomerId.TabIndex = 13;
			this.lblCustomerId.Text = "Label Customer ID";
			// 
			// lblCustomerName
			// 
			this.lblCustomerName.AutoSize = true;
			this.lblCustomerName.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
			this.lblCustomerName.Location = new System.Drawing.Point(3, 18);
			this.lblCustomerName.Name = "lblCustomerName";
			this.lblCustomerName.Size = new System.Drawing.Size(164, 50); //18
			this.lblCustomerName.TabIndex = 14;
			this.lblCustomerName.Text = "Label Customer Name";
			// 
			// numPad1
			// 
			this.numPad1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.numPad1.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numPad1.Appearance.Options.UseFont = true;
			this.numPad1.AutoSize = true;
			this.numPad1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.numPad1.CurrencyCode = null;
			this.numPad1.EnteredQuantity = new decimal(new int[] {
			0,
			0,
			0,
			0});
			this.numPad1.EnteredValue = "";
			this.numPad1.Location = new System.Drawing.Point(249, 30);
			this.numPad1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.numPad1.MaskChar = "";
			this.numPad1.MaskInterval = 0;
			this.numPad1.MaxNumberOfDigits = 20;
			this.numPad1.MinimumSize = new System.Drawing.Size(300, 330);
			this.numPad1.Name = "numPad1";
			this.numPad1.NegativeMode = false;
			this.numPad1.NoOfTries = 0;
			this.numPad1.NumberOfDecimals = 2;
			this.numPad1.PromptText = "Enter Amount";
			this.numPad1.ShortcutKeysActive = false;
			this.numPad1.Size = new System.Drawing.Size(300, 330);
			this.numPad1.TabIndex = 2;
			this.numPad1.TimerEnabled = true;
			this.numPad1.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad1_EnterButtonPressed);
			// 
			// panelControl2
			// 
			this.panelControl2.Controls.Add(this.btnInquiry);
			this.panelControl2.Controls.Add(this.btnRequest);
			this.panelControl2.Controls.Add(this.amtCustAmounts);
			this.panelControl2.Controls.Add(this.labelPaymentAmount);
			this.panelControl2.Location = new System.Drawing.Point(559, 30);
			this.panelControl2.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(200, 330);
			this.panelControl2.TabIndex = 3;
			// 
			// btnInquiry
			// 
			this.btnInquiry.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.btnInquiry.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnInquiry.FlatAppearance.BorderSize = 0;
			this.btnInquiry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInquiry.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F);
			this.btnInquiry.Location = new System.Drawing.Point(5, 182);
			this.btnInquiry.Name = "btnInquiry";
			this.btnInquiry.Size = new System.Drawing.Size(189, 62);
			this.btnInquiry.TabIndex = 3;
			this.btnInquiry.Text = "Inquiry QRIS";
			this.btnInquiry.UseVisualStyleBackColor = true;
			this.btnInquiry.Click += new System.EventHandler(this.btnInquiry_Click);
			// 
			// btnRequest
			// 
			this.btnRequest.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.btnRequest.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.btnRequest.FlatAppearance.BorderSize = 0;
			this.btnRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRequest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F);
			this.btnRequest.Location = new System.Drawing.Point(5, 107);
			this.btnRequest.Name = "btnRequest";
			this.btnRequest.Size = new System.Drawing.Size(188, 62);
			this.btnRequest.TabIndex = 2;
			this.btnRequest.Text = "Request to EDC";
			this.btnRequest.UseVisualStyleBackColor = false;
			this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
			// 
			// amtCustAmounts
			// 
			this.amtCustAmounts.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.amtCustAmounts.Appearance.Options.UseFont = true;
			this.amtCustAmounts.Appearance.Options.UseForeColor = true;
			this.amtCustAmounts.CurrencyRate = new decimal(new int[] {
			0,
			0,
			0,
			0});
			this.amtCustAmounts.ForeignCurrencyMode = false;
			this.amtCustAmounts.HighestOptionAmount = new decimal(new int[] {
			0,
			0,
			0,
			0});
			this.amtCustAmounts.IncludeRefAmount = true;
			this.amtCustAmounts.LocalCurrencyCode = "";
			this.amtCustAmounts.Location = new System.Drawing.Point(2, 29);
			this.amtCustAmounts.LowesetOptionAmount = new decimal(new int[] {
			0,
			0,
			0,
			0});
			this.amtCustAmounts.Name = "amtCustAmounts";
			this.amtCustAmounts.OptionsLimit = 5;
			this.amtCustAmounts.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.amtCustAmounts.ShowBorder = false;
			this.amtCustAmounts.Size = new System.Drawing.Size(196, 72);
			this.amtCustAmounts.TabIndex = 1;
			this.amtCustAmounts.UsedCurrencyCode = "";
			this.amtCustAmounts.ViewOption = LSRetailPosis.POSProcesses.WinControls.AmountViewer.ViewOptions.ExcactAmountOnly;
			this.amtCustAmounts.AmountChanged += new LSRetailPosis.POSProcesses.WinControls.AmountViewer.OutputChanged(this.amtCustAmounts_AmountChanged);
			// 
			// labelPaymentAmount
			// 
			this.labelPaymentAmount.Dock = System.Windows.Forms.DockStyle.Top;
			this.labelPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 14.25F);
			this.labelPaymentAmount.Location = new System.Drawing.Point(2, 2);
			this.labelPaymentAmount.Name = "labelPaymentAmount";
			this.labelPaymentAmount.Size = new System.Drawing.Size(196, 27);
			this.labelPaymentAmount.TabIndex = 0;
			this.labelPaymentAmount.Text = "Payment amount";
			this.labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cmbPilihBank
			// 
			this.cmbPilihBank.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cmbPilihBank.FormattingEnabled = true;
			this.cmbPilihBank.Location = new System.Drawing.Point(47, 3);
			this.cmbPilihBank.Name = "cmbPilihBank";
			this.cmbPilihBank.Size = new System.Drawing.Size(192, 28);
			this.cmbPilihBank.TabIndex = 4;
			this.cmbPilihBank.Text = "Pilih Bank";
			// 
			// lblWaitingRespond
			// 
			this.lblWaitingRespond.AutoSize = true;
			this.lblWaitingRespond.Font = new System.Drawing.Font("Arial", 11F);
			this.lblWaitingRespond.ForeColor = System.Drawing.Color.Red;
			this.lblWaitingRespond.Location = new System.Drawing.Point(255, 389);
			this.lblWaitingRespond.Margin = new System.Windows.Forms.Padding(11, 0, 3, 0);
			this.lblWaitingRespond.Name = "lblWaitingRespond";
			this.lblWaitingRespond.Size = new System.Drawing.Size(296, 18);
			this.lblWaitingRespond.TabIndex = 4;
			this.lblWaitingRespond.Text = "Waiting for response...Please check EDC";
			this.lblWaitingRespond.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblWaitingRespond.Visible = false;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.labelAmountDue);
			this.flowLayoutPanel1.Controls.Add(this.labelAmountDueValue);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(245, 43);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(529, 95);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// labelAmountDue
			// 
			this.labelAmountDue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelAmountDue.AutoSize = true;
			this.labelAmountDue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
			this.labelAmountDue.Location = new System.Drawing.Point(0, 0);
			this.labelAmountDue.Margin = new System.Windows.Forms.Padding(0);
			this.labelAmountDue.Name = "labelAmountDue";
			this.labelAmountDue.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
			this.labelAmountDue.Size = new System.Drawing.Size(390, 95);
			this.labelAmountDue.TabIndex = 0;
			this.labelAmountDue.Tag = "";
			this.labelAmountDue.Text = "Total amount due:";
			// 
			// labelAmountDueValue
			// 
			this.labelAmountDueValue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelAmountDueValue.AutoSize = true;
			this.labelAmountDueValue.Font = new System.Drawing.Font("Segoe UI Light", 36F);
			this.labelAmountDueValue.Location = new System.Drawing.Point(390, 0);
			this.labelAmountDueValue.Margin = new System.Windows.Forms.Padding(0);
			this.labelAmountDueValue.Name = "labelAmountDueValue";
			this.labelAmountDueValue.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
			this.labelAmountDueValue.Size = new System.Drawing.Size(139, 95);
			this.labelAmountDueValue.TabIndex = 1;
			this.labelAmountDueValue.Text = "$0.00";
			// 
			// placeHolderQR
			// 
			this.placeHolderQR.Location = new System.Drawing.Point(247, 0);
			this.placeHolderQR.Name = "placeHolderQR";
			this.placeHolderQR.Size = new System.Drawing.Size(300, 300); //310
			this.placeHolderQR.TabIndex = 5;
			//this.placeHolderQR.BorderStyle = BorderStyle.FixedSingle;
			//this.placeHolderQR.Image = Color.White; 
			this.placeHolderQR.TabStop = false;
			this.placeHolderQR.Visible = false;
			//this.placeHolderQR.Margin = new Padding(0, -10, 0, 0);
			///pictureQRIS 
			/// 
			PictureBox pictureQRIS = new PictureBox();                
			pictureQRIS.Size = new Size(120, 70); // Set the size of pictureBox2                 
			// Create a new Bitmap with white color
			int imageWidth = pictureQRIS.Width;
			int imageHeight = pictureQRIS.Height; 
			Bitmap whiteBitmap = null;
			Graphics g = null;

			whiteBitmap = new Bitmap(imageWidth, imageHeight);

			// Create a Graphics object from the Bitmap
			g = Graphics.FromImage(whiteBitmap);

			// Fill the Bitmap with white color
			g.Clear(Color.Black);

			// Assign the white Bitmap to the PictureBox's Image property
			//pictureQRIS.BackColor = Color.Black;
			//pictureQRIS.Image = whiteBitmap;
			
			pictureQRIS.Image = Image.FromFile(System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extensions", "img"), "QRIS-logo-1.png")); // Load the image for pictureBox1
			
			pictureQRIS.SizeMode = PictureBoxSizeMode.StretchImage;
			//pictureQRIS.Location = new System.Drawing.Point(110, 0);
			//placeHolderQR.Image = black;
			//textBOx                        
			this.txtNMID.ForeColor = Color.Black;
			this.txtNMID.Font = new System.Drawing.Font("Segoe UI", 11F,FontStyle.Bold);
			//this.txtNMID.Location = new System.Drawing.Point(3, 160);
			this.txtNMID.Name = "txtNMId";
			this.txtNMID.Visible = false;
			this.txtNMID.Size = new System.Drawing.Size(280, 20); //20 //250
			 
			//flow
			//
			/*
			FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
			flowLayoutPanel.Dock = DockStyle.Fill;
			flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
			flowLayoutPanel.Controls.Add(pictureQRIS);            
			flowLayoutPanel.Controls.Add(placeHolderQR);
			flowLayoutPanel.Controls.Add(this.txtNMID);
			flowLayoutPanel.BackColor = Color.White;
			flowLayoutPanel.Size = new System.Drawing.Size(340, 350);
			this.tableLayoutPanel1.Controls.Add(flowLayoutPanel, 2, 2);
			pictureQRIS.Margin = new Padding((flowLayoutPanel.Width - pictureQRIS.Width) / 2, 0, 0, 0);
			placeHolderQR.Margin = new Padding((flowLayoutPanel.Width - placeHolderQR.Width) / 2, 0, 0, 0);
			// Move txtNMID position to overlap the pictureQRIS control
			//int txtNMIDLeftMargin = (flowLayoutPanel.Width - txtNMID.Width) / 2;
			//int txtNMIDTopMargin = pictureQRIS.Height - txtNMID.Height; // Adjust this value as needed
			//txtNMID.Margin = new Padding(txtNMIDLeftMargin, txtNMIDTopMargin, 0, 0);
			
			txtNMID.Margin = new Padding(50, -30, 0, 0); 
			*/





			//test tablelayout\
			// Assuming you have created the TableLayoutPanel and other controls like pictureQRIS and placeHolderQR
			
			tableLayoutPanelQRIS.Size = new System.Drawing.Size(340, 350);
			tableLayoutPanelQRIS.Dock = DockStyle.Fill;
			tableLayoutPanelQRIS.BackColor = Color.White;
			tableLayoutPanelQRIS.ColumnCount = 1;
			tableLayoutPanelQRIS.RowCount = 3;
			tableLayoutPanelQRIS.RowStyles.Add(new RowStyle(SizeType.Absolute, pictureQRIS.Height));
			tableLayoutPanelQRIS.RowStyles.Add(new RowStyle(SizeType.Absolute, placeHolderQR.Height));
			tableLayoutPanelQRIS.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100)); // Adjust the height as needed for txtNMID
			//tableLayoutPanelQRIS.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			tableLayoutPanelQRIS.Controls.Add(pictureQRIS, 0, 0);
			tableLayoutPanelQRIS.Controls.Add(placeHolderQR, 0, 1);
			tableLayoutPanelQRIS.Controls.Add(this.txtNMID, 0, 2);
			tableLayoutPanelQRIS.Visible = false;
			// Set margins for pictureQRIS and placeHolderQR if needed
			pictureQRIS.Margin = new Padding((tableLayoutPanelQRIS.Width - pictureQRIS.Width) / 2, 0, 0, 0);
			placeHolderQR.Margin = new Padding(((tableLayoutPanelQRIS.Width - placeHolderQR.Width) / 2) + 2, 0, 0, 0); //(tableLayoutPanelQRIS.Width - placeHolderQR.Width) / 2

			// Move txtNMID position to overlap the pictureQRIS control
			int txtNMIDLeftMargin = ((tableLayoutPanelQRIS.Width - txtNMID.Width) / 2 ) ;
			//int txtNMIDTopMargin = 0; // Set this value to control the overlap position
			txtNMID.Margin = new Padding(txtNMIDLeftMargin, 0, 0, 0);//txtNMIDLeftMargin

			// Add the TableLayoutPanel to your parent control (e.g., a Form or another container)
			//this.Controls.Add(tableLayoutPanelQRIS);
			this.tableLayoutPanel1.Controls.Add(tableLayoutPanelQRIS, 2, 2);
			//end 

			// 
			// frmPayCustomerAccount
			// 
			this.Appearance.Options.UseFont = true;
			this.Appearance.Options.UseForeColor = true;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(1024, 732);
			this.Controls.Add(this.panelControl1);
			this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LookAndFeel.SkinName = "Money Twins";
			this.Name = "frmPayCustomerAccount";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			this.Controls.SetChildIndex(this.panelControl1, 0);
			((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelCustomer)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.placeHolderQR)).EndInit();
			this.ResumeLayout(false);

		}

		private void txtReff_TextChanged(object sender, EventArgs e)
		{

			txtReff.Height = 100;
		}

		void btnRequest_EnabledChanged(object sender, EventArgs e)
		{
			if (((Button)sender).Enabled)
			{
				this.btnRequest.BackColor = System.Drawing.SystemColors.ActiveCaption;
				this.btnRequest.FlatAppearance.BorderColor = System.Drawing.Color.White;
				btnRequest.ForeColor = System.Drawing.Color.Black;
				//btnRequest.Text = "Button";
			}
			else
			{
				btnRequest.BackColor = Color.FromArgb(255, 136, 176, 214);
				btnRequest.ForeColor = Color.FromArgb(255,146, 161, 177);
				btnRequest.FlatAppearance.BorderColor = Color.FromArgb(255, 223, 229, 235);
				/*back rgba(136,176,214,255)
				border rgba(223,229,235,255)
				font color rgba(146,161,177,255)
				*/
				//btnRequest.Text = "";
			}
		}

		
		#endregion

		//custom by Yonathan to prevent esc 07/02/2023 custom QRIS
		string pressCancel;
		void btnCancel_Click(object sender, EventArgs e)
		{
			
			if (
			   int.Parse(this.tenderInfo.TenderID) == 32
			   || int.Parse(this.tenderInfo.TenderID) == 33
			   || int.Parse(this.tenderInfo.TenderID) == tenderQRIS) //add by Yonaathan 10/01/2023 31 for QR
			{
				pressCancel = "1";
			}
			else if ((int.Parse(this.tenderInfo.TenderID) == tenderShopee
				|| int.Parse(this.tenderInfo.TenderID) == tenderShopeeDev) && isIntegrated == true)
			{
				string url = urlInvalidate; //"https://apiqrisdev.cp.co.id/api/shopeePay/createDynamicQR";
				// create QR generator object

				if (croppedQRCodeImage !=null)
				{
					using (frmMessage dialog = new frmMessage("Batalkan transaksi? Apabila transaksi batal,\nmaka kode QR akan dibatalkan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
					{
						 
						POSFormsManager.ShowPOSForm(dialog);
						if (dialog.DialogResult == System.Windows.Forms.DialogResult.Yes)
						{
							//APIAccess.APIParameter.parmResponseShopeePay responseShopeePay;
							string urlInq = urlNotify; //"https://apiqrisdev.cp.co.id/api/shopeePay/manualPaymentStatus"; 
							APIAccess.APIFunction.inquiryStatusPaymentShopeePay(urlInq, this.registeredAmount, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId, out responseShopeePay, out listData);
							if (responseShopeePay.message.ToString() == "success")
							{
								using (frmMessage dialog2 = new frmMessage("Kode QRIS Shopee ini sudah terbayarkan, silakan klik OK", MessageBoxButtons.OK, MessageBoxIcon.Information))
								{
									POSFormsManager.ShowPOSForm(dialog2);
								}
								lblWaitingRespond.Text = "";
								btnOk.Enabled = true;
								btnInquiry.Enabled = false;
								btnCancel.Enabled = false;
								amtCustAmounts.Enabled = false;
							}

							else
							{
								if(timesUp != true)
								{
									APIAccess.APIFunction.invalidateQRShopeePay(url, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId, out responseShopeePay);
									using (frmMessage dialog3 = new frmMessage(responseShopeePay.message, MessageBoxButtons.OK, MessageBoxIcon.Error))
									{

										POSFormsManager.ShowPOSForm(dialog3);

									}
								}
								
								pressCancel = "1";
								timerCount.Stop();
							}

						}
					}

				}
				else
				{
					pressCancel = "1";
				}

				
			}
			//add by Yonathan to accomodate GRABMART 10/11/2023 tenderID 19
			else if (int.Parse(this.tenderInfo.TenderID) == tenderGrabMart
				|| int.Parse(this.tenderInfo.TenderID) == tenderGrabMartDev)// && isIntegrated == true)
			{
				pressCancel = "1";
				//using (frmMessage dialog = new frmMessage("Batalkan p?\n", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
				//{
					 
				//    POSFormsManager.ShowPOSForm(dialog);
					 
				//}
				
				timerCount.Stop();
				if (isIntegrated == true)
				{
					//empty the graborder variable
					APIAccess.APIAccessClass.grabOrderState = "";
					APIAccess.APIAccessClass.grabCustName = "";
					APIAccess.APIAccessClass.grabCustPhone = "";
					APIAccess.APIAccessClass.grabOrderIdLong = "";
					PosApplication.Instance.RunOperation(PosisOperations.VoidTransaction, this.posTransaction);

				}



				//check if transaction already void
				//if (isIntegrated == true)
				//{
				//    while (((RetailTransaction)posTransaction).CalculableSalesLines != null)
				//    {
				//        PosApplication.Instance.RunOperation(PosisOperations.VoidTransaction, this.posTransaction);

				//        if (((RetailTransaction)BlankOperations.BlankOperations.globalposTransaction).CalculableSalesLines != null)
				//        {
				//            using (frmMessage dialog2 = new frmMessage("Harus kosongkan keranjang", MessageBoxButtons.OK, MessageBoxIcon.Information))
				//            {
				//                POSFormsManager.ShowPOSForm(dialog2);
				//            }
				//        }
				//    }
					
				//}
				

			}
			else
			{
				pressCancel = "1";
				timerCount.Stop();
			}
		}

		

		void OnKeyDown(object sender, KeyEventArgs e)
		{
			pressCancel = "0";
			if (e.KeyData == Keys.Escape)
			{
				pressCancel = "1";
				return;
			}
			//base.OnKeyDown(e);
		}
		private void FormIsClosing(object sender, FormClosingEventArgs e)
		{

			if (pressCancel == "1")
			{
				e.Cancel = false;
			}
			else
			{
				e.Cancel = true;
			}

			//e.Cancel = true; //prevent user from closing the form
			/*
			 * switch (e.CloseReason)
			{
				case CloseReason.UserClosing:
					if (MessageBox.Show("Are you sure you want to exit?",
										"Exit?",
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Question) == DialogResult.No)
					{
						e.Cancel = true;
					}
					break;
			}
			*/
		}
		//
		#region Events
		private void numPad1_EnterButtonPressed()
		{
			TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, numPad1.EnteredDecimalValue, true, balanceAmount);
			if (string.IsNullOrEmpty(tenderReq.ErrorText))
			{
				controlIndex = 1;
				Action(numPad1.EnteredValue);
			}
			else
			{
				//MessageBox.Show(tenderReq.ErrorText);
				using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					// The amount entered is higher than the maximum amount allowed
					POSFormsManager.ShowPOSForm(dialog);
					numPad1.TryAgain();
				}
			}
		}

		private void amtCustAmounts_AmountChanged(decimal outAmount, string currCode)
		{
			TenderRequirement tenderReq = new TenderRequirement(this.tenderInfo, outAmount, true, balanceAmount);
			if (string.IsNullOrEmpty(tenderReq.ErrorText))
			{
				controlIndex = 1;
				Action(outAmount.ToString());
			}
			else
			{
				using (frmMessage dialog = new frmMessage(tenderReq.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					// The amount entered is higher than the maximum amount allowed
					POSFormsManager.ShowPOSForm(dialog);
				}
			}
		}

		private void btnCustomerSearch_Click(object sender, EventArgs e)
		{
			CustomerSearch custSearch = new CustomerSearch();
			custSearch.OperationID = PosisOperations.CustomerSearch;
			custSearch.POSTransaction = this.posTransaction;
			custSearch.RunOperation();

			RetailTransaction retailPosTransaction = (RetailTransaction)posTransaction;

			if (!(retailPosTransaction.Customer.IsEmptyCustomer())
				&& retailPosTransaction.Customer.Blocked == BlockedEnum.No)
			{
				//Get the customer information
				//if (!retailPosTransaction.InvoicedCustomer.IsEmptyCustomer())
				//{
				//    this.CustomerID = retailPosTransaction.InvoicedCustomer.CustomerId;
				//}
				//else if (!retailPosTransaction.Customer.IsEmptyCustomer())
				//{
				//    this.CustomerID = retailPosTransaction.Customer.CustomerId;
				//}

				//SetCustomerDisplayText(retailPosTransaction);

				//The customer might have a discount so the lines need to be calculated again
				retailPosTransaction.CalcTotals();

				//Change the Amountviewer if needed
				if (retailPosTransaction.TransSalePmtDiff != balanceAmount)
				{
					balanceAmount = retailPosTransaction.TransSalePmtDiff;
					if (proposedPaymentAmount == null || Math.Abs(balanceAmount) < Math.Abs((decimal)proposedPaymentAmount))
					{
						amtCustAmounts.SoldLocalAmount = balanceAmount;
						amtCustAmounts.SetButtons();
					}

					//If an amount has already been selected and it's higher than the new balance of the transaction (with the new discount)
					//then we need to adjust it to reflect the new balance.
					if (this.Amount > balanceAmount)
					{
						this.Amount = balanceAmount;
					}

				}

				controlIndex = 2;
				Action(string.Empty);
			}
			/*The control itself displays information about weather a customer is found
			 *else
			{
				frmMessage dialog = new frmMessage(1446, MessageBoxButtons.OK, MessageBoxIcon.Information);
				POSFormsManager.ShowPOSForm(dialog);
			}*/
		}

		private void SetCustomerDisplayText(RetailTransaction transaction)
		{
			Customer cust;

			if (transaction.InvoicedCustomer != null
				&& !string.IsNullOrWhiteSpace(transaction.InvoicedCustomer.CustomerId))
			{
				cust = transaction.InvoicedCustomer;
			}
			else
			{
				cust = transaction.Customer;
			}

			lblCustomerId.Text = cust.CustomerId;
			lblCustomerName.Text = cust.Name;

			//labelCustomerIdValue.Text = cust.CustomerId;
			//labelCustomerNameValue.Text = cust.Name;
			//txtReff.Text = cust.CustomerId;
			//this.customerID = cust.CustomerId;
		}

		#endregion

		#region Private Procedures

		private void resetAllColors()
		{
			Color color = Color.Transparent;
			//labelCustomerIdValue.BackColor = color;
			//labelCustomerNameValue.BackColor = color;
			//labelAmountValue.BackColor = color;
		}

		private void SetButtonStatus()
		{
			Color selectColor = Color.White;

			resetAllColors();

			switch (controlIndex)
			{
				case 1:
					//labelAmountValue.BackColor = selectColor;
					labelAmountValue.Text = string.Empty;
					numPad1.Enabled = true;
					numPad1.EntryType = NumpadEntryTypes.Price;
					numPad1.PromptText = ApplicationLocalizer.Language.Translate(1443); //Enter Amount in
					numPad1.NegativeMode = this.balanceAmount < 0;
					break;

				case 2:
					//labelCustomerIdValue.BackColor = selectColor;
					//labelCustomerIdValue.Text = string.Empty;
					//labelCustomerNameValue.BackColor = selectColor;
					//labelCustomerNameValue.Text = string.Empty;
					amtCustAmounts.SoldLocalAmount = proposedPaymentAmount != null ? (decimal)proposedPaymentAmount : balanceAmount;
					amtCustAmounts.SetButtons();
					numPad1.NegativeMode = false;
					numPad1.PromptText = ApplicationLocalizer.Language.Translate(1444); //Choose currency code
					numPad1.Enabled = false;
					break;
			}
		}

		private bool SetValue(string value)
		{
			System.Diagnostics.Debug.Assert(value != null, "value may not be null.");

			Color selectColor = Color.White;
			bool valueOK = false;

			try
			{
				switch (controlIndex)
				{
					case 1:
						if (string.IsNullOrEmpty(value))
						{
							value = "0";
						}

						Amount = Convert.ToDecimal(value);
						valueOK = Math.Abs(Amount) > 0;
						break;
					case 2:
						/*valueOK = !string.IsNullOrWhiteSpace(labelCustomerIdValue.Text)
							&& !string.IsNullOrWhiteSpace(labelCustomerNameValue.Text);*/
						break;
				}
			}
			catch (FormatException)
			{
			}

			return valueOK;
		}

		private void Action(string value)
		{
			// Check if the amount is valid

			if (SetValue(value))
			{
				registeredAmount = amount;

				// if an amount and customer id have both been entered then close the dlg
				//if (!string.IsNullOrWhiteSpace(labelAmountValue.Text) && !string.IsNullOrWhiteSpace(labelCustomerIdValue.Text) && !string.IsNullOrWhiteSpace(labelCustomerNameValue.Text))
				//{
				//    registeredAmount  = amount;                    
				//    //this.DialogResult = DialogResult.OK;
				//    //Close();
				//}
				//else
				//{
				//    numPad1.TryAgain();
				//}
			}
			else
			{
				numPad1.TryAgain();
			}

			controlIndex++;
			if (controlIndex > 2)
			{
				controlIndex = 1;
			}

			numPad1.Clear();
			SetButtonStatus();
		}
		#endregion

		//private void btnOk_Click(object sender, EventArgs e)
		//{
		//    //this.numPad1_EnterButtonPressed();
		//    MessageBox.Show("Click OK");
		//}

		#region IInteractionView implementation

		/// <summary>
		/// Initialize the form
		/// </summary>
		/// <typeparam name="TArgs">Prism Notification type</typeparam>
		/// <param name="args">Notification</param>
		public void Initialize<TArgs>(TArgs args)
			where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
		{
			if (args == null)
				throw new ArgumentNullException("args");
		}

		/// <summary>
		/// Return the results of the interation call
		/// </summary>
		/// <typeparam name="TResults"></typeparam>
		/// <returns>Returns the TResults object</returns>
		public TResults GetResults<TResults>() where TResults : class, new()
		{
			return new PayCustomerAccountConfirmation
			{
				Confirmed = this.DialogResult == DialogResult.OK,
				RegisteredAmount = this.RegisteredAmount,
				CustomerId = this.customerID,
				//OperationDone = true,
			} as TResults;
		}

		#endregion
		public void command()
		{
			btnOk.PerformClick();
		}

		private void btnOk_Click_1(object sender, EventArgs e)
		{
            
			if (this.customerID == null)
			{
				using (frmMessage dialog = new frmMessage("Must Choose Customer to Continue this Operation", MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					POSFormsManager.ShowPOSForm(dialog);
					controlIndex = 3;
				}
				return;
			}

			if (labelAmountValue.Text == "")
			{
				using (frmMessage dialog = new frmMessage("Amount Must be Filled", MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					POSFormsManager.ShowPOSForm(dialog);
					this.DialogResult = DialogResult.None;
				}
				return;
			}
			else if (txtReff.Text == "")
			{
				string varError = "Reference No. Must be Filled";

				#region CPECRBCA
				//Adjust different Error message for ECR Response
				if (int.Parse(this.tenderInfo.TenderID) == 32 || int.Parse(this.tenderInfo.TenderID) == 33)
				{
					varError = "Please click request to EDC";
				}
				#endregion

				using (frmMessage dialog = new frmMessage(varError, MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					POSFormsManager.ShowPOSForm(dialog);
					this.DialogResult = DialogResult.None;
				}
				return;
			}
			else if (txtPhone.Text == "" && int.Parse(this.tenderInfo.TenderID) != tenderQRIS && int.Parse(this.tenderInfo.TenderID) != tenderShopeeDev && int.Parse(this.tenderInfo.TenderID) != tenderShopee) //add validation to differentiate transaction for QRIS 
			{
				string varError = "Phone No. Must be Filled";

				#region CPECRBCA
				//Adjust different Error message for ECR Response
				if (int.Parse(this.tenderInfo.TenderID) == 32 || int.Parse(this.tenderInfo.TenderID) == 33)
				{
					varError = "Please click request to EDC";
				}
				#endregion
				 
				using (frmMessage dialog = new frmMessage(varError, MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					POSFormsManager.ShowPOSForm(dialog);
					this.DialogResult = DialogResult.None;
				}
				return;
			}
			//Purchaser Name field is only validated when transaction is not ECR
			else if (txtCustName.Text == "" && int.Parse(this.tenderInfo.TenderID) != 32 && int.Parse(this.tenderInfo.TenderID) != 33 && int.Parse(this.tenderInfo.TenderID) != tenderQRIS && int.Parse(this.tenderInfo.TenderID) != tenderShopeeDev && int.Parse(this.tenderInfo.TenderID) != tenderShopee) //add tender 31 for QR by Yonathan 10/01/2023
			{
				using (frmMessage dialog = new frmMessage("Customer Name Must be Filled", MessageBoxButtons.OK, MessageBoxIcon.Stop))
				{
					POSFormsManager.ShowPOSForm(dialog);
					this.DialogResult = DialogResult.None;
				}
				return;
			}
			else
			{
				timerCount.Stop();
				this.DialogResult = DialogResult.OK;
				int id = 0;
				string dataArea = "";

				SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{
					string queryStringStore = @"
											SELECT TOP 1 INVENTLOCATIONDATAAREAID FROM AX.[RETAILCHANNELTABLE] WHERE INVENTLOCATION LIKE @Store";

					RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

					using (SqlCommand command = new SqlCommand(queryStringStore, connectionStore))
					{
						command.Parameters.AddWithValue("@store", "%" + this.posTransaction.StoreId + "%");

						if (connectionStore.State != ConnectionState.Open)
						{
							connectionStore.Open();
						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								dataArea = reader[0] + "";
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
					if (connectionStore.State != ConnectionState.Closed)
					{
						connectionStore.Close();
					}
				}

				SqlConnection connectionID = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{
					string queryStringID = @"
											SELECT TOP 1 REPLICATIONCOUNTERFROMORIGIN FROM AX.[CPNEWPAYMENTPOS] ORDER BY REPLICATIONCOUNTERFROMORIGIN DESC";

					RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

					using (SqlCommand command = new SqlCommand(queryStringID, connectionID))
					{

						if (connectionID.State != ConnectionState.Open)
						{
							connectionID.Open();
						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								id = (int)reader[0];
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
					if (connectionID.State != ConnectionState.Closed)
					{
						connectionID.Close();
					}
				}

				SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

				try
				{
					string queryStringDelete = @"DELETE FROM AX.[CPNEWPAYMENTPOSTMP]";

					SqlCommand commandDelete = new SqlCommand(queryStringDelete, connection);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					commandDelete.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					this.DialogResult = DialogResult.None;
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}

				try
				{
					string queryString = @"
											INSERT INTO AX.CPNEWPAYMENTPOSTMP (
																			ID,
																			TransactionID, 
																			PaymentAmount, 
																			NoReffTransaction, 
																			TenderID, 
																			TenderName, 
																			PurchaserName, 
																			PurchaserPhone, 
																			Store, 
																			TransDate,
																			[DATAAREAID],
																			[PARTITION]
																			)
											VALUES (
													@ID,
													@TransactionID,
													@PaymentAmount, 
													@NoReffTransaction, 
													@TenderID, 
													@TenderName, 
													@PurchaserName, 
													@PurchaserPhone, 
													@Store, 
													@TransDate,
													@DataAreaID,
													@Partition
													)";

					RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

					#region CPECRBCA
					//Add default value for ECR Transaction
					string refNumber = this.txtReff.Text;
					string purchaserName = this.txtCustName.Text;
					string purchaserPhone = this.txtPhone.Text;

					if (int.Parse(this.tenderInfo.TenderID) == 32 || int.Parse(this.tenderInfo.TenderID) == 33)
					{
						refNumber = approvalCode;
						purchaserName = this.tenderInfo.TenderName;
						purchaserPhone = "";
					}
					#endregion

					using (SqlCommand command = new SqlCommand(queryString, connection))
					{
						command.Parameters.AddWithValue("@ID", ++id);
						command.Parameters.AddWithValue("@TransactionID", this.posTransaction.TransactionId);
						command.Parameters.AddWithValue("@PaymentAmount", this.registeredAmount);
						command.Parameters.AddWithValue("@NoReffTransaction", refNumber);
						command.Parameters.AddWithValue("@TenderID", this.tenderInfo.TenderID);
						command.Parameters.AddWithValue("@TenderName", this.tenderInfo.TenderName);
						command.Parameters.AddWithValue("@PurchaserName", purchaserName);
						command.Parameters.AddWithValue("@PurchaserPhone", purchaserPhone);
						command.Parameters.AddWithValue("@Store", this.posTransaction.StoreId);
						command.Parameters.AddWithValue("@TransDate", DateTime.Now);
						command.Parameters.AddWithValue("@DataAreaID", dataArea);
						command.Parameters.AddWithValue("@Partition", 1);

						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						command.ExecuteNonQuery();
					}


					//Custom by Yonathan 20/11/2023 for Grabmart to update status to API
					if (int.Parse(this.tenderInfo.TenderID) == tenderGrabMart
				|| int.Parse(this.tenderInfo.TenderID) == tenderGrabMartDev)
					{

                        //check connection first - yonathan 06122024
                        bool status = false;
                        string functionName = "GetGRABMARTAPI";
                        string url = "";
                        APIAccess.APIFunction apiFunction = new APIAccess.APIFunction();

                        APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
                        url = APIClass.getURLAPIByFuncName(functionName);
                        //bool isApiAvailable = apiFunction.CheckApiAvailability(url).Result;
                        status = apiFunction.CheckForInternetConnection(PosApplication.Instance, url);
                        if (status == true)
                        {

                            string result = "";
                            result = updateStatusOrderGrabMart(APIAccess.APIAccessClass.merchantId, APIAccess.APIAccessClass.grabOrderIdLong.ToString(), this.posTransaction.TransactionId); 

                            APIAccess.APIParameter.ApiResponseGrabmart responseData = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(result); //MyJsonConverter.Deserialize<parmResponse>(result);

                            //Data order = MyJsonConverter.Deserialize<Data>(responseData.data);
                            if (responseData.error != false)
                            {

                            }
                            //empty the graborder variable
                            APIAccess.APIAccessClass.grabOrderState = "";
                            APIAccess.APIAccessClass.grabCustName = "";
                            APIAccess.APIAccessClass.grabCustPhone = "";
                            APIAccess.APIAccessClass.grabOrderIdLong = "";
                        }
                        else
                        {
                            string errText = "Tidak bisa terhubung dengan jaringan API.\nSilakan cek koneksi jaringan\ndan coba secara berkala.";
                            if (retryCountGrab == 2)
                            {
                                errText = "Tidak bisa terhubung dengan jaringan.\nSilakan batalkan transaksi ini dan coba proses ulang dari menu Grab Order.";
                            }
                            using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(errText, MessageBoxButtons.OK, MessageBoxIcon.Error))
                            {
                                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
                                retryCountGrab++;

                                if (retryCountGrab >= 3)
                                {
                                    btnCancel.Enabled = true;
                                    btnOk.Enabled = false;
                                }
                                else
                                {
                                    btnCancel.Enabled = false;
                                    btnOk.Enabled = true;
                                }

                                return;


                                //btnOk.Enabled = false;
                                
                            }

                        }

                        //original
                        //string result = "";
                        //result = updateStatusOrderGrabMart(APIAccess.APIAccessClass.merchantId, APIAccess.APIAccessClass.grabOrderIdLong.ToString(), this.posTransaction.TransactionId);

                        //APIAccess.APIParameter.ApiResponseGrabmart responseData = APIAccess.APIFunction.MyJsonConverter.Deserialize<APIAccess.APIParameter.ApiResponseGrabmart>(result); //MyJsonConverter.Deserialize<parmResponse>(result);

                        ////Data order = MyJsonConverter.Deserialize<Data>(responseData.data);
                        //if (responseData.error != false)
                        //{
						
                        //}
                        ////empty the graborder variable
                        //APIAccess.APIAccessClass.grabOrderState = "";
                        //APIAccess.APIAccessClass.grabCustName = "";
                        //APIAccess.APIAccessClass.grabCustPhone = "";
                        //APIAccess.APIAccessClass.grabOrderIdLong = "";
					}
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					this.DialogResult = DialogResult.None;
					throw;
				}
				finally
				{
					if (connection.State != ConnectionState.Closed)
					{
						connection.Close();
					}
				}

				pressCancel = "1";
			}
			 
		}

		
		private string updateStatusOrderGrabMart(string _merchantId, string _orderId, string _receiptId)
		{
			string result = "";
			//int rowIndex;
			//string amountCashout = "";
			string storeId = ApplicationSettings.Terminal.InventLocationId.ToString();//ApplicationSettings.Terminal.StoreId.ToString();
			//string PathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Extensions\\", "APIConfig.xml");
			var url = "";// "https://devpfm.cp.co.id/api/grab/updateStatusReceipt";
			 
			string functionName = "UpdateOrderGrabAPI";
			APIAccess.APIAccessClass APIClass = new APIAccess.APIAccessClass();
			url = APIClass.getURLAPIByFuncName(functionName);

			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
										   SecurityProtocolType.Tls11 |
										   SecurityProtocolType.Tls12;

			System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

			if (url == "")
			{
				throw new Exception(string.Format("Function not found : {0},\nPlease contact your ItSupport", functionName));
			}
			else
			{
				var httpRequest = (HttpWebRequest)WebRequest.Create(url);
				
				httpRequest.Method = "POST";
				httpRequest.ContentType = "application/json";
				httpRequest.Headers.Add("Authorization", "PFM");

				//initialize the required parameter before post to API

				//parmRequestCashOut.DATAAREAID = EOD.InternalApplication.Settings.Database.DataAreaID;
				//parmRequestCashOut.STOREID = "JCIBBR1"; //ApplicationSettings.Terminal.StoreId.ToString();
				//parmRequestCashOut.MODULETYPE = "CO";
				//parmRequestCashOut.TRANSDATE = DateTime.Now.ToString("yyyy-MM-'dd");
				var pack = new APIAccess.APIParameter.parmUpdateStatusListGrabmart()
				{
					merchantID = _merchantId,
					orderID = _orderId,
					receiptID = _receiptId
				};
				try
				{
					var data = APIAccess.APIFunction.MyJsonConverter.Serialize(pack);
					using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
					{
						streamWriter.Write(data);
					}

					var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
					{
						result = streamReader.ReadToEnd();
					}
				}
				catch (Exception ex)
				{
					//LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					//return "0";
					throw;

				}
			}

			return result;
		}

		private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && e.KeyChar != '+')
			{
				e.Handled = true;
			}
			else if (txtPhone.Text.Length >= 20 && e.KeyChar != (char)Keys.Back)
			{
				e.Handled = true;
			}
		}

		private void txtReff_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (txtReff.Text.Length >= 30 && e.KeyChar != (char)Keys.Back)
			{
				e.Handled = true;
			}
		}

		#region CPLOCKPAYMENT
		/*
		 * function to validate customer based on tender
		 * return true when customer is found and match with Tender ID
		 * else return false
		 */
		private bool validateCustomer()
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{
				string queryStringID = @"SELECT 
											CUSTACCOUNT 
										FROM ax.CPCUSTTENDERMAPTABLE
										WHERE 
											CUSTACCOUNT = @CUSTOMERID
											AND TENDERTYPEID = @TENDERTYPEID
											AND DATAAREAID = @DATAAREAID";

				RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

				using (SqlCommand command = new SqlCommand(queryStringID, connection))
				{
					command.Parameters.AddWithValue("@CUSTOMERID", this.customerID);
					command.Parameters.AddWithValue("@TENDERTYPEID", this.tenderInfo.TenderID);
					command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							return true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
			}
			finally
			{
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}

			return false;
		}

		#endregion

		#region check integration
		private bool validateIntegration()
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			try
			{
				string queryStringID = @"SELECT 
											ISINTEGRATION 
										FROM ax.CPEPAYMAPPING
										WHERE 
											 
											 TENDERTYPEID = @TENDERTYPEID
											AND STORENUMBER = @STORENUMBER";

				RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

				using (SqlCommand command = new SqlCommand(queryStringID, connection))
				{
					//command.Parameters.AddWithValue("@CUSTOMERID", this.customerID);
					command.Parameters.AddWithValue("@TENDERTYPEID", this.tenderInfo.TenderID);
					command.Parameters.AddWithValue("@STORENUMBER", LSRetailPosis.Settings.ApplicationSettings.Database.StoreID);

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							if(Convert.ToString(reader["ISINTEGRATION"]) == "1")
							{
								return true;
							}
							else
							{
								return false;
							}

							//if (timeClockType == TimeClockType.BreakFlowStart)
							//{
							//    this.BreakActivity = Convert.ToString(reader["ACTIVITY"]);
							//}
							//else
							//{
							//    this.JobId = Convert.ToString(reader["JOBID"]);
							//}
						}
					}

					
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
			}
			finally
			{
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}

			return false;
		}
		#endregion  

		#region CPECRBCA
		/*
		 * this function is called when user click Request to ECR button
		 * handle connection to ECR and return response
		 */
		private void btnRequest_Click(object sender, EventArgs e)
		{
			if (
				int.Parse(this.tenderInfo.TenderID) == 32
				|| int.Parse(this.tenderInfo.TenderID) == 33
				|| int.Parse(this.tenderInfo.TenderID) == tenderQRIS) //add by Yonaathan 10/01/2023 31 for QR
			{
				//validate amount must be filled before connect to ECR
				if (labelAmountValue.Text == "")
				{
					showError("Amount must be filled");
					return;
				}


				//get selected Bank Name
				string bankName = cmbPilihBank.GetItemText(cmbPilihBank.SelectedItem);

				//validate min amount
				SqlConnection connectionStore = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
				try
				{
					string queryStringStore = @"SELECT MINTRXAMOUNT FROM ax.CPPILIHBANK WHERE BANK = @SELECTEDBANK
											ORDER BY RECID DESC";

					RetailTransaction retailTransaction = (RetailTransaction)this.posTransaction;

					using (SqlCommand command = new SqlCommand(queryStringStore, connectionStore))
					{
						command.Parameters.AddWithValue("@SELECTEDBANK", bankName);
						if (connectionStore.State != ConnectionState.Open)
						{
							connectionStore.Open();
						}
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (!reader.Read() || decimal.Parse(reader[0] + "") > registeredAmount)
							{
								showError("Min Transaction Amount for this transaction is " + double.Parse(reader[0] + ""));
								return;
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
					if (connectionStore.State != ConnectionState.Closed)
					{
						connectionStore.Close();
					}
				}
				//end validate min amount
				/*
				 * Separate function for readability
				 * add new condition when there is a new ECR integration
				 * or use switch case when there is more than 4 Bank for readability
				 */
				if (bankName == "BCA")
				{
					requestECRBCA(bankName);
					//add by yonathan
					//txtCustName.Text = "QRIS TEST";
				}
				else if (bankName == "QRISBCA")
				{
					requestECRBCA(bankName);
					//add by yonathan
					txtCustName.Text = bankName.ToString();
				}
				else
				{
					showError("This bank does not support ECR");
				}

				//btnInquiry.Enabled = true;
				//btnRequest.Enabled = false;
				amtCustAmounts.Enabled = false;
			}
			else if (int.Parse(this.tenderInfo.TenderID) == tenderShopee
				|| int.Parse(this.tenderInfo.TenderID) == tenderShopeeDev)
			{
				if (labelAmountValue.Text == "")
				{
					showError("Amount must be filled");
					return;
				}
				if (txtReff.Text == "")
				{
					showError("Reference must be filled");
					return;
				}
				amtCustAmounts.Enabled = false;
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string generatedQR = ""; // "00020101021226610016ID.CO.SHOPEE.WWW01189360091800200345640208200345640303UME52041234530336054071000.005802ID5916PFM DUKUH KUPANG6015KOTA JAKARTA TI61051381062420524JDELIMA-R1-230726-1631240710JDELIMA-R16304C4C1";

				string url = urlCreate; //"https://apiqrisdev.cp.co.id/api/shopeePay/createDynamicQR";
				// create QR generator object
				//QRCodeGenerator qrGenerator = new QRCodeGenerator();

				APIAccess.APIParameter.parmResponseShopeePay responseShopeePay;
				APIAccess.APIParameter.ListResultData listData;
                //MessageBox.Show(string.Format("{0},{1},{2},{3},{4}", url, this.registeredAmount, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId));
				APIAccess.APIFunction.generateQRShopeePay(url, this.registeredAmount, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId,out responseShopeePay, out listData);


				//APIAccess.APIFunction.generateQRShopeePay(url, this.registeredAmount, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId);

				//APIAccess.APIParameter paramAPI = new APIAccess.APIParameter();
				  //dataResult = new APIAccess.APIParameter.ListResultData();
				if (listData == null)
				{
					using (frmMessage dialog = new frmMessage(responseShopeePay.message, MessageBoxButtons.OK, MessageBoxIcon.Error))
					{
						// The amount entered is higher than the maximum amount allowed
						POSFormsManager.ShowPOSForm(dialog);
						btnCancel.Enabled = true;
						btnInquiry.Enabled = false;
					}
				}
				else
				{
					downloadQR(listData.qr_url, listData.qr_content);
                    txtNMID.Text = "NMID: " + listData.nmid.ToString();
					//generatedQR = listData.qr_content;
					// create QR code data

					//QRCodeData qrCodeData = qrGenerator.CreateQrCode(generatedQR, QRCoder.QRCodeGenerator.ECCLevel.Q);

					//if (generatedQR != "")
					//{
					// create QR code object
					//QRCode qrCode = new QRCode(qrCodeData);

					// convert QR code to bitmap image
					//Bitmap qrCodeImage = qrCode.GetGraphic(20);

					// create new picture box
					//PictureBox qrCodePictureBox = new PictureBox();
					//PictureBox placeHolderQR
					//qrCodePictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

					//qrCodePictureBox.Size = new Size(300, 350);
					//qrCodePictureBox.Image = qrCodeImage;
					//placeHolderQR.Image = qrCodeImage;

					//Bitmap resizedQRCodeImage = new Bitmap(qrCodeImage, new Size(330, 340));
					//placeHolderQR.Image = resizedQRCodeImage;
					//Bitmap croppedQRCodeImage = CropImage(resizedQRCodeImage);



					//placeHolderQR.Image = croppedQRCodeImage;
					placeHolderQR.SizeMode = PictureBoxSizeMode.AutoSize;
					placeHolderQR.Visible = true;


                    Image imageToPrint = placeHolderQR.Image;

                    //Printer printer = new Printer();
                    //printer.PrintQRIS(imageToPrint);

					targetTime = DateTime.Now.AddSeconds(listData.qrValidityPeriod);

					// Initialize and start the timer.
					timerCount.Tick += Timer_Tick;
					timerCount.Interval = 1000;// listData.qrValidityPeriod;
					timerCount.Start();
					//countInquiryQR = countInquiryQR + 1;
					//Arahkan kamera ponsel customer ke QR code\nvia aplikasi Shopee untuk pembayaran\nQR code berlaku {0} detik
					btnRequest.Enabled = false;
					btnCancel.Enabled = false;
					btnInquiry.Enabled = true;
					btnOk.Enabled = false;
					//}
					//else
					//{
					//    lblWaitingRespond.Visible = true;
					//    lblWaitingRespond.Text = "QR gagal generate,\nklik tombol Generate QRIS lagi";

					//    btnRequest.Enabled = true;
					//    btnCancel.Enabled = true;
					//    btnInquiry.Enabled = false;
					//    btnOk.Enabled = false;
					//}
					////// Combine the base directory with the "/extension/img" folder path 
					//string imgFolderPath = System.IO.Path.Combine(baseDirectory, "Extensions", "img");    


					//PictureBox pictureQRIS = new PictureBox();                
					//pictureQRIS.Size = new Size(75, 35); // Set the size of pictureBox2                 
					//pictureQRIS.Image = Image.FromFile(System.IO.Path.Combine(imgFolderPath, "QRIS-logo-1.png")); // Load the image for pictureBox1
					//pictureQRIS.SizeMode = PictureBoxSizeMode.Zoom; // Adjust the image size to fit the PictureBox



					//this.Controls.Add(qrCodePictureBox);

					// Create FlowLayoutPanel
					//FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
					//flowLayoutPanel.Dock = DockStyle.Fill;
					//flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
					//flowLayoutPanel.Controls.Add(pictureQRIS);
					//flowLayoutPanel.Controls.Add(placeHolderQR);
					//this.tableLayoutPanel1.Controls.Add(flowLayoutPanel, 2, 2);
					// Add FlowLayoutPanel to the form
					//this.Controls.Add(flowLayoutPanel);
					////// Add pictureBox2 as a child control to pictureBox1
					////placeHolderQR.Controls.Add(pictureQRIS);

					//// Add pictureBox1 to the form's controls

					//this.tableLayoutPanel1.Controls.Add(pictureQRIS, 2, 1);
					//tableLayoutPanel1.CellPaint += TableLayoutPanel1_CellPaint;
				}
				

			}

			
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			 // Calculate the time remaining.
			TimeSpan remainingTime = targetTime - DateTime.Now; 

			// Update the textbox with the countdown value.
			if (remainingTime.TotalSeconds <= 0)
			{
				lblWaitingRespond.Text = "";
				timerCount.Stop();
				//lblWaitingRespond.Text = "Waktu habis, silakan generate QR\napabila ingin melanjutkan transaksi!"; "Masa berlaku kode QR sudah habis.\nAabila ingin generate ulang kode QR, silakan klik Cancel dan pilih Payment Shopee lagi" "Waktu habis, silakan klik cancel,\nkemudian masuk ke payment shopee lagi apabila ingin generate QR ulang"
				using (frmMessage dialog = new frmMessage("Masa berlaku kode QRIS sudah habis.\nApabila ingin generate ulang kode QRIS, silakan klik Cancel dan pilih Payment Shopee lagi", MessageBoxButtons.OK, MessageBoxIcon.Warning))
				{
					// The amount entered is higher than the maximum amount allowed
					POSFormsManager.ShowPOSForm(dialog);
					btnCancel.Enabled = true;
					btnInquiry.Enabled = true ;
					timesUp = true;
				}
				
				 // Stop the timer when the countdown reaches zero.
				//string url = "https://apiqrisdev.cp.co.id/api/shopeePay/createDynamicQR";
				
				//APIAccess.APIParameter.parmResponseShopeePay responseShopeePay;
				//APIAccess.APIFunction.invalidateQRShopeePay(url, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId, out responseShopeePay);
			}
			else
			{
				lblWaitingRespond.Visible = true;
				//lblWaitingRespond.Text = string.Format("Arahkan kamera ponsel customer ke QR code\nQR code berlaku {0:D2} menit : {1:D2} detik", remainingTime.Minutes, remainingTime.Seconds);
				lblWaitingRespond.Text = string.Format("Arahkan kamera ponsel cust ke QRIS\nQRIS berlaku {0:D2} menit : {1:D2} detik", remainingTime.Minutes, remainingTime.Seconds);
			} 
		}
		 private Bitmap CreateBitmapWithWhiteBackground(Image originalImage)
		{
			// Create a new Bitmap with the same size as the original image
			Bitmap bitmapWithWhiteBackground = new Bitmap(originalImage.Width, originalImage.Height);

			// Draw the original image on the new Bitmap with white background
			using (Graphics g = Graphics.FromImage(bitmapWithWhiteBackground))
			{
				g.Clear(Color.White); // Fill with white background
				g.DrawImage(originalImage, Point.Empty); // Draw the original image on top
			}

			return bitmapWithWhiteBackground;
		}
		//private void TableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		//{
		//    // Specify the column and row you want to fill with white
		//    int targetColumn = 2; // Column 2 (zero-based index)
		//    int targetRow = 1;    // Row 1 (zero-based index)

		//    // Check if the current cell is the target cell
		//    if (e.Column == targetColumn && e.Row == targetRow)
		//    {
		//        // Set the desired color
		//        Color fillColor = Color.White;

		//        // Get the Graphics object
		//        Graphics g = e.Graphics;

		//        // Fill the cell with the specified color
		//        g.FillRectangle(new SolidBrush(fillColor), e.CellBounds);
		//    }
		//}

		//ECR for BCA
		private void requestECRBCA(string bankName)
		{
			WinFormsTouch.CP_frmWaitingRespond waitingForm = new WinFormsTouch.CP_frmWaitingRespond();
			//Declare Transtype here, refer to Bank Documentation
			//string transType = "01";
			transType = "01";
			btnRequest.Enabled = false;

			if (int.Parse(this.tenderInfo.TenderID) == tenderQRIS)
			{
				transType = "31";
			}


			try
			{
				/*
				 * Set cancel button disabled during ECR Process
				 * to prevent user click Cancel during transaction in EDC
				 */
				btnCancel.Enabled = false;

				GME_Var.transTypeBCA = transType;
				//Create BCA Object using GMECustom library
				ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
				GME_Var.approvalCodeBCA = string.Empty;
				GME_Var.respCodeBCA = string.Empty;
				RetailTransaction retailTransaction = posTransaction as RetailTransaction;

				//Set totalamount based on registered Amount
				int totalAmount = decimal.ToInt32(this.registeredAmount);

				//Try Open Port based on CPPILIHBANK data
				try
				{
					BCAOnline.openPort(cmbPilihBank.SelectedValue.ToString());
				}
				catch (Exception ex)
				{
					MessageBox.Show("error open port");
				}

				if (GME_Var.serialBCA.IsOpen)
				{
					flushCOMPort();
					//Connection open
					_isReceived = false;
					autoEventBCA.Reset();
					autoEventBCA = new AutoResetEvent(false);
					
					/*
					 * Send Data to ECR in here
					 * Noticed function WaitCallBank(receiveThreadBCA) is threading
					 * Means that it will wait for receiveThreadBCA to return value for continue process
					 */

					try
					{
						BCAOnline.sendData(totalAmount.ToString(), transType, Connection.applicationLoc, posTransaction, refNumber);
						Thread.Sleep(500);
						lblWaitingRespond.Visible = true;
						//waitingForm.Show();
						ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
						autoEventBCA.WaitOne();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
					/*BCAOnline.sendData(totalAmount.ToString(), transType, Connection.applicationLoc, posTransaction);
					ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);*/
					
					/*if (!autoEventBCA.WaitOne(60000,false))
					{
						GME_Var.respCodeBCA = "TO";
					}*/
					
					if (GME_Var.transTypeBCA == transType)
					{

						btnCancel.Enabled = true;
						btnInquiry.Enabled = false ;
						lblWaitingRespond.Visible = false ;
						//validate response from BCA here
						switch (GME_Var.respCodeBCA)
						{
							
							case "00":
							case "*0":
								#region ECRSuccess
								//ECR & Transaction success, continue action to insert to DB here
								refNumber = GME_Var.refNumberBCA;
								string invoiceNumber = GME_Var.invoiceNumberBCA;
								int transAmount = int.Parse(GME_Var.amountBCA.Substring(0, GME_Var.amountBCA.Length - 2));
								int otherAmount = int.Parse(GME_Var.otherAmountBCA.Substring(0, GME_Var.otherAmountBCA.Length - 2));
                                decimal admFee = 0;
                                decimal admFeeText = 0;
                                //add Adm fee by Yonathan 10/06/2024 //CPIADMFEE
                                //get ADM fee
                                if (otherAmount != 0)
                                {
                                    admFee = GetAdmFee(int.Parse(this.tenderInfo.TenderID));
                                    
                                    otherAmount = admFee != 0 ? otherAmount - (int)admFee : otherAmount;

                                    admFeeText = admFee;
                                    lblCustName.Visible = true;
                                    lblCustName.Text = "Biaya Admin";
                                    txtCustName.Visible = true;
                                    txtCustName.ReadOnly = true;
                                    txtCustName.Text = PosApplication.Instance.Services.Rounding.Round(admFeeText, false); //admFee.ToString();
                                }
                                else
                                {
                                    lblCustName.Visible = true;
                                    lblCustName.Text = "Biaya Admin";
                                    txtCustName.Visible = true;
                                    txtCustName.ReadOnly = true;
                                    txtCustName.Text = PosApplication.Instance.Services.Rounding.Round(admFeeText, false);
                                    //lblCustName.Visible = false;
                                    //txtCustName.ReadOnly = false;
                                    //txtCustName.Visible = false; 
                                }
                                
                                //end

								string pan = GME_Var.panBCA;
								pan = pan.Replace(" ", "");
								//if (pan != "" && pan != null)
								if(string.IsNullOrEmpty(pan) == false)
								{
									pan = pan.Replace(" ", "");
									var lastDigits = pan.Substring(pan.Length - 4, 4);

									var requiredMask = new String('X', pan.Length - lastDigits.Length);

									var maskedString = string.Concat(requiredMask, lastDigits);
									pan = maskedString;
								}
								
								approvalCode = GME_Var.approvalCodeBCA;


								/*
								 * Adjust form design
								 * add value to field
								 * set button enabled/disabled
								 */

								txtPhone.Text = PosApplication.Instance.Services.Rounding.Round(otherAmount, false); // otherAmount + "";
								txtReff.Text = pan; //must be changed to masking
							   
								
								btnRequest.Enabled = false;
								btnOk.Enabled = true;
								cmbPilihBank.Enabled = false;
								amtCustAmounts.Enabled = false;

								if (int.Parse(this.tenderInfo.TenderID) == tenderQRIS)
								{
									using (frmMessage dialog = new frmMessage("Print QR Success", MessageBoxButtons.OK, MessageBoxIcon.Information))
									{
										POSFormsManager.ShowPOSForm(dialog);
									}
									txtReff.Text = GME_Var.refNumberBCA;
									btnInquiry.Enabled = true;
									btnCancel.Enabled = false;
									btnOk.Enabled = false;
									amtCustAmounts.Enabled = false;
									/*timerCount.Tick += timerCount_Tick;
									timerCount.Interval = 45000;
									timerCount.Start(); */ 
									//countInquiryQR = countInquiryQR + 1;
								}
								else
								{

									//insert into AX.CPAMBILTUNAI
									SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
                                    //add ADM FEE //CPIADMFEE
									try
									{
										string queryString = @"
											INSERT INTO AX.CPAMBILTUNAITEMP (
																			TRANSACTIONID, 
																			TRXIDEDC,
																			CARDNUMBER, 
																			AMOUNT, 
																			TRANSACTIONTYPE, 
																			BANK, 
																			TENDER, 
																			STORE,
																			TRANSDATE,
																			TRANSACTIONSTATUS,
                                                                            ADMFEE,
																			[DATAAREAID],
																			[PARTITION]
																			)
											VALUES (
													@TRANSACTIONID, 
													@TRXIDEDC,
													@CARDNUMBER, 
													@AMOUNT, 
													@TRANSACTIONTYPE, 
													@BANK, 
													@TENDER, 
													@STORE, 
													@TRANSDATE,
													@TRANSACTIONSTATUS,
                                                    @ADMFEE,
													@DATAAREAID,
													@PARTITION
													)";

										using (SqlCommand command = new SqlCommand(queryString, connection))
										{
											command.Parameters.AddWithValue("@TRANSACTIONID", this.posTransaction.TransactionId);
											command.Parameters.AddWithValue("@TRXIDEDC", GME_Var.approvalCodeBCA);
											command.Parameters.AddWithValue("@CARDNUMBER", pan);
											command.Parameters.AddWithValue("@AMOUNT", otherAmount);
											command.Parameters.AddWithValue("@TRANSACTIONTYPE", GME_Var.transTypeBCA);
											command.Parameters.AddWithValue("@BANK", bankName);
											command.Parameters.AddWithValue("@TENDER", this.tenderInfo.TenderID);
											command.Parameters.AddWithValue("@Store", this.posTransaction.StoreId);
											command.Parameters.AddWithValue("@TRANSDATE", DateTime.Now);
											command.Parameters.AddWithValue("@TRANSACTIONSTATUS", 0);
                                            command.Parameters.AddWithValue("@ADMFEE", admFee);
											command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);
											command.Parameters.AddWithValue("@PARTITION", 1);

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
										MessageBox.Show(ex.ToString());
										throw;
									}
									finally
									{
										if (connection.State != ConnectionState.Closed)
										{
											connection.Close();
										}
									}
									//end insert
									/*pan = pan.Replace(" ", "");
									var lastDigits = pan.Substring(pan.Length - 4, 4);

									var requiredMask = new String('X', pan.Length - lastDigits.Length);

									var maskedString = string.Concat(requiredMask, lastDigits);
									var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");*/
									//
									txtReff.Text = pan;// maskedString;
									//end masking
									using (frmMessage dialog = new frmMessage("Payment EDC Success", MessageBoxButtons.OK, MessageBoxIcon.Information))
									{
										POSFormsManager.ShowPOSForm(dialog);
									}
									btnCancel.Enabled = false;
									btnInquiry.Enabled = false;
								}
								
								
								break;
								#endregion
							case "54":
								showError("Decline Expired Card");
								break;
							case "55":
								showError("Decline Incorrect PIN");
								break;
							case "P2":
								showError("Read Card Error");
								break;
							case "P3":
								showError("User press Cancel on EDC");
								btnRequest.Enabled = true;
								break;
							case "Z3":
								showError("EMV Card Decline");
								break;
							case "CE":
								showError("Connection Error/Line Busy");
								break;
							case "TO":
								showError("Connection Timeout");
								btnRequest.Enabled = true;
								break;
							case "PT":
								showError("EDC Problem / EDC perlu di settlement");
								btnRequest.Enabled = true;
								break;
							case "PS":
								showError("Settlement Failed");
								break;
							case "aa":
								showError("Decline (aa represent two digit alphanumeric value from EDC)");
								break;
							case "S2":
								showError("TRANSAKSI GAGAL");
								break;
							case "S3":
								showError("TXN BLM DIPROSES MINTA SCAN QR");
								break;
							case "S4":
								showError("TXN EXPIRED ULANGI TRANSAKSI");
								break;
							case "TN":
								showError("Topup Tunai Not Ready");
								break;
							case "NAK":
								showError("NAK");
								btnCancel.Enabled = true;
								break;
							default:
								showError("Unknown Error: " + GME_Var.respCodeBCA);
								break;
						}
					}
					_isReceived = true;
					autoEventBCA.Reset();
				}
				else
				{
					btnCancel.Enabled = true;
					showError("EDC Not Connected");
				}

				GME_Var.isPoint = false;
				//Close port when transaction done
				BCAOnline.closePort();
			}
			catch (Exception ex)
			{
				showError("Failed to send data");
				btnCancel.Enabled = true;
			}
		}

        private void downloadQR(string _url, string _qrContent)
        {
            string imageUrl = _url; // Replace with the actual image URL
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    // Download the image as a byte array
                    byte[] imageBytes = webClient.DownloadData(imageUrl); 

                    // Convert the byte array to a Bitmap image
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        Bitmap image = new Bitmap(ms);
                        Bitmap resizedQRCodeImage = new Bitmap(image, new Size(295, 300));
                        //image.si = new Size(330, 340);
                        // Display the downloaded image in the PictureBox
                        //placeHolderQR.Image = image;
                        croppedQRCodeImage = CropImage(resizedQRCodeImage);
                        placeHolderQR.Image = croppedQRCodeImage;
                        /*
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);
                        Bitmap resizedQRCodeImage = new Bitmap(qrCodeImage, new Size(330, 340));
                        placeHolderQR.Image = resizedQRCodeImage;
                        Bitmap croppedQRCodeImage = CropImage(resizedQRCodeImage);
                        placeHolderQR.Image = croppedQRCodeImage;
                    */
                    }
                }
                catch (Exception ex)
                {
                    //if not success download QR from the uRL, just create it from string.
                    //MessageBox.Show("Error downloading the image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //create QR from String
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(_qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);
                    // create QR code object
                    QRCode qrCode = new QRCode(qrCodeData);
                    // convert QR code to bitmap image
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    Bitmap resizedQRCodeImage = new Bitmap(qrCodeImage, new Size(330, 340));
                    croppedQRCodeImage = CropImage(resizedQRCodeImage);
                    placeHolderQR.Image = croppedQRCodeImage;
                }
            }
        }


        private async void downloadQROld(string _url, string _qrContent)
        {
            string imageUrl = _url; // Replace with the actual image URL
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Download the image as a byte array
                    byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    // Convert the byte array to a Bitmap image
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        Bitmap image = new Bitmap(ms);
                        Bitmap resizedQRCodeImage = new Bitmap(image, new Size(295, 300));
                        //image.si = new Size(330, 340);
                        // Display the downloaded image in the PictureBox
                        //placeHolderQR.Image = image;
                        croppedQRCodeImage = CropImage(resizedQRCodeImage);
                        placeHolderQR.Image = croppedQRCodeImage;
                        /*
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);
                        Bitmap resizedQRCodeImage = new Bitmap(qrCodeImage, new Size(330, 340));
                        placeHolderQR.Image = resizedQRCodeImage;
                        Bitmap croppedQRCodeImage = CropImage(resizedQRCodeImage);
                        placeHolderQR.Image = croppedQRCodeImage;
                    */
                    }
                }
                catch (Exception ex)
                {
                    //if not success download QR from the uRL, just create it from string.
                    //MessageBox.Show("Error downloading the image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //create QR from String
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(_qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);
                    // create QR code object
                    QRCode qrCode = new QRCode(qrCodeData);
                    // convert QR code to bitmap image
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    Bitmap resizedQRCodeImage = new Bitmap(qrCodeImage, new Size(330, 340));
                    croppedQRCodeImage = CropImage(resizedQRCodeImage);
                    placeHolderQR.Image = croppedQRCodeImage;
                }
            }
        }

		/*
		 * Threading function
		 * call and wait for response from EDC
		 */
		public void receiveThreadBCA(object stateInfo)
		{
			using (GME_Var.serialBCA)
			{
				// get start time
				DateTime start = DateTime.Now;
				// buffer for pushing received string data into
				StringBuilder indata = new StringBuilder();
				_LastResponBCA = "";
				//looptime is used to determined how many seconds has passed
				int loopTime = 0;
				GME_Var.serialBCA.DiscardInBuffer();
				GME_Var.serialBCA.DiscardOutBuffer();
				GME_Var.serialBCA.BaseStream.Flush();
				while (!_isReceived)
				{
					//set timeout when there is no response from EDC after some time
					if (loopTime > 60)
					{						
						if (transType == "32")
						{
							//btnCancel.Enabled = false;
							GME_Var.serialBCA.Dispose();
							GME_Var.transTypeBCA = transType;
							GME_Var.respCodeBCA = "TOI";//Silakan cek koneksi kabel EDC, kemudian klik button Inquiry kembali
							_LastResponBCA = "";
							isEDCSettlementBCA = false;
							((AutoResetEvent)stateInfo).Set();
							_isReceived = true;
							break;
						}
						
					}
					
					//add loopTime after each loop
					loopTime++;
					//mod by Yonathan to check whether port is open -  Custom QRIS
					if (GME_Var.serialBCA.IsOpen)
					{
						if (GME_Var.serialBCA.BytesToRead > 0)
						{
							//Add data to string when there is a response from EDC
							byte[] buffer = new byte[GME_Var.serialBCA.BytesToRead];
							int bytesRead = GME_Var.serialBCA.Read(buffer, 0, buffer.Length);
							if (bytesRead <= 0) return;

							buffer.Last();
							indata.Clear();
							foreach (byte b in buffer)
							{
								indata.Append(string.Format("{0:x2}", b));
							}
						}
						else
							//else wait for 1 second
							System.Threading.Thread.Sleep(1000);

						//if response found (indata string > 0 character)
						if (indata.Length > 0)
						{
							/*
							if (GME_Var.serialBCA.IsOpen) //send ACK that informs EDC that message is received.
							{
								byte[] _hexval = new byte[1]; // need to convert byte to byte[] to write
								_hexval[0] = 0x06;
								GME_Var.serialBCA.Write(_hexval, 0, 1);
								//Thread.Sleep(1);
							}*/
							//check if current indata string is the same with last response
							if (indata.ToString() != _LastResponBCA)
							{
								string hex = indata.ToString();
								ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
								//if length > 2 and not 06 (ACK)
								if (hex.Length > 2 && hex != "06")
								{
									//mapping response to BCADataCapture (GMECustom library)
									//ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
									if (transType == "32")
									{
										BCAOnline.MappingTextInquiryQR(hex);
									}
									else
									{
										BCAOnline.MappingText(hex);
									}
									

									
									GME_Var.serialBCA.Dispose();
									_LastResponBCA = hex;
									isEDCSettlementBCA = false;
									((AutoResetEvent)stateInfo).Set();
									_isReceived = true;

									//send ACK
									ElectronicDataCaptureBCA BCAOnlineAck = new ElectronicDataCaptureBCA();
									try
									{
										BCAOnlineAck.openPort(cmbPilihBank.SelectedValue.ToString());
									}
									catch (Exception ex) { }

									if (GME_Var.serialBCA.IsOpen)
									{
										byte[] _hexval = new byte[1]; // need to convert byte to byte[] to write
										_hexval[0] = 0x06;
										GME_Var.serialBCA.Write(_hexval, 0, 1);
										//Thread.Sleep(1);
									}
									else
									{
										showError("EDC Not Connected");
									}
									//end send ACK
									break;
								}
								else if (hex == "15") //if NAK, then re-send the command
								{
									
									//btnCancel.Enabled = true;
									GME_Var.serialBCA.Dispose();
									GME_Var.transTypeBCA = "01";
									GME_Var.respCodeBCA = "NAK";
									_LastResponBCA = "";
									isEDCSettlementBCA = false;
									//((AutoResetEvent)stateInfo).Set();
									_isReceived = true;
									break;
									//string textReq = "P01000000000100000000000000                       000000000000002020N                                                                                   ";
									//byte[] hexstring = Encoding.ASCII.GetBytes(textReq);
									//ElectronicDataCaptureBCA BCAOnlineNAK = new ElectronicDataCaptureBCA();

									
									/*try
									{
										BCAOnlineNAK.openPort(cmbPilihBank.SelectedValue.ToString());
									}
									catch (Exception ex) { }
									if (GME_Var.serialBCA.IsOpen)
									{
									   

										foreach (byte hexval in hexstring)
										{
											byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
											GME_Var.serialBCA.Write(_hexval, 0, 1);
											//Thread.Sleep(1);
										}
									}
									else
									{
										showError("EDC Not Connected");
									}
									*/

									//ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
									//BCAOnline.sendData("1", "01", Connection.applicationLoc, posTransaction, refNumber);

									//send ACK
									/*
									ElectronicDataCaptureBCA BCAOnlineNAK = new ElectronicDataCaptureBCA();

									string textReq = "P01000000000100000000000000                       000000000000002020N                                                                                   "; 
									byte[] hexstring = Encoding.ASCII.GetBytes(textReq);
									try
									{
										BCAOnlineNAK.openPort(cmbPilihBank.SelectedValue.ToString());
									}
									catch (Exception ex) { }

									if (GME_Var.serialBCA.IsOpen)
									{
									   

										foreach (byte hexval in hexstring)
										{
											byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
											GME_Var.serialBCA.Write(_hexval, 0, 1);
											//Thread.Sleep(1);
										}
									}
									else
									{
										showError("EDC Not Connected");
									}*/
									//break;
								}
							}
							else
							{
								_isReceived = true;
								//send ACK
								ElectronicDataCaptureBCA BCAOnlineAck = new ElectronicDataCaptureBCA();
								try
								{
									BCAOnlineAck.openPort(cmbPilihBank.SelectedValue.ToString());
								}
								catch (Exception ex) { }

								if (GME_Var.serialBCA.IsOpen)
								{
									byte[] _hexval = new byte[1]; // need to convert byte to byte[] to write
									_hexval[0] = 0x06;
									GME_Var.serialBCA.Write(_hexval, 0, 1);
									//Thread.Sleep(1);
								}
								else
								{
									showError("EDC Not Connected");
								}
								//end send ACK
								break;
							}
						}
					}
					else
					{
						btnCancel.Enabled = true;
						GME_Var.serialBCA.Dispose();
						GME_Var.transTypeBCA = "01";
						GME_Var.respCodeBCA = "TO";
						_LastResponBCA = "";
						isEDCSettlementBCA = false;
						((AutoResetEvent)stateInfo).Set();
						_isReceived = true;
						break;
					}
					//
					
				}
			}
		}

		private void showError(string message)
		{
			using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Stop))
			{
				POSFormsManager.ShowPOSForm(dialog);
				this.DialogResult = DialogResult.None;
			}

		}
		#endregion

		private void btnInquiry_Click(object sender, EventArgs e) //custom by Yonathan 10/01/2023 custom QRIS
		{
			if (int.Parse(this.tenderInfo.TenderID) == 32
				|| int.Parse(this.tenderInfo.TenderID) == 33
				|| int.Parse(this.tenderInfo.TenderID) == tenderQRIS) //add by Yonaathan 10/01/2023 31 for QR
			{
				string bankName = "QRISBCA";

				//add validation
				if (validateReffNumber(txtReff.Text) == false)
				{
					requestInquiryQRISBCA(bankName);

					if (_paymentSuccess == true)
					{
						btnOk.Enabled = true;
						btnInquiry.Enabled = false;
						btnCancel.Enabled = false;
					}
					else if (_paymentSuccess == false)
					{
						countInquiryQR++;



					}
				}
				else
				{
					showError("NoReff tidak sesuai dengan transaksi QRIS saat ini. \nSilakan input no reff yang sesuai");
					countInquiryQR++;
				}
				//end

				if (countInquiryQR >= 5 && _paymentSuccess == false)
				{
					btnCancel.Enabled = true;
				}
				else if (countInquiryQR == 1)
				{
					txtReff.Visible = true;
					txtReff.Enabled = false;
					//txtReff.ReadOnly = false;
					lblReff.Visible = true;
					lblReff.Enabled = true;
					btnCancel.Enabled = false;
				}
				else
				{
					btnCancel.Enabled = false;
				}
			}
			else if (int.Parse(this.tenderInfo.TenderID) == tenderShopee
					|| int.Parse(this.tenderInfo.TenderID) == tenderShopeeDev)
			{
				string url = urlNotify; //"https://apiqrisdev.cp.co.id/api/shopeePay/manualPaymentStatus";
				// create QR generator object




				amtCustAmounts.Enabled = false;
				APIAccess.APIFunction.inquiryStatusPaymentShopeePay(url, this.registeredAmount, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId, out responseShopeePay, out listData);


				//APIAccess.APIFunction.generateQRShopeePay(url, this.registeredAmount, this.posTransaction.StoreId, this.posTransaction.TerminalId, this.posTransaction.TransactionId);

				//APIAccess.APIParameter paramAPI = new APIAccess.APIParameter();
				//dataResult = new APIAccess.APIParameter.ListResultData();

				//generatedQR = listData.qr_content;
				lblWaitingRespond.Visible = true;
				//lblWaitingRespond.Text = responseShopeePay.message.ToString();


				//add validation
				
				if (responseShopeePay.message.ToString() == "success")
				{
					using (frmMessage dialog = new frmMessage("Payment QRIS Shopee sukses", MessageBoxButtons.OK, MessageBoxIcon.Information))
					{
						POSFormsManager.ShowPOSForm(dialog);
					}
					lblWaitingRespond.Text = "";                         
					btnOk.Enabled = true;
					btnInquiry.Enabled = false;
					btnCancel.Enabled = false;
					amtCustAmounts.Enabled = false;
				}
				else  
				{
					using (frmMessage dialog = new frmMessage(responseShopeePay.message, MessageBoxButtons.OK, MessageBoxIcon.Error))
					{
						// The amount entered is higher than the maximum amount allowed
						POSFormsManager.ShowPOSForm(dialog);
						
					}
					btnOk.Enabled = false;
					btnCancel.Enabled = false;
					countInquiryQR++;
				}
				
				//end

				if (countInquiryQR >= 3 && responseShopeePay.message.ToString() != "success")
				{
					btnCancel.Enabled = true;
				}
				else if (countInquiryQR == 1)
				{
					txtReff.Visible = true;
					txtReff.Enabled = false;
					//txtReff.ReadOnly = false;
					lblReff.Visible = true;
					lblReff.Enabled = true;
					btnCancel.Enabled = false;
					amtCustAmounts.Enabled = false;
				}
				else
				{
					btnCancel.Enabled = false;
				}


				//if (responseShopeePay.message.ToString() == "success")
				//{
				//    lblWaitingRespond.Text = "Payment sucess";
				//    btnOk.Enabled = true;
				//    btnCancel.Enabled = false;

				//}
				//else
				//{
				//    lblWaitingRespond.Text = "Transaction not found";
				//    btnOk.Enabled = false;
				//    btnCancel.Enabled = false;
				//}
			}
			
		}

		private bool validateReffNumber(string reffNumber)
		{
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

			try
			{//todo benerin DB to filter the BANK
				string queryString = @"SELECT BANK, CARDNUMBER, TRANSDATE
										  FROM AX.CPAMBILTUNAI
										  WHERE
											convert(varchar(10), TRANSDATE, 102) = convert(varchar(10), getdate(), 102) AND
											BANK = 'QRISBCA' AND 
											CARDNUMBER = @cardnumber";
			using (SqlCommand command = new SqlCommand(queryString, connection))
			{
				command.Parameters.AddWithValue("@cardnumber", reffNumber);
				if (connection.State != ConnectionState.Open)
				{
					connection.Open();
				}

				command.ExecuteNonQuery();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						return true;
					}
					else { return false; }
				}
			}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				MessageBox.Show(ex.ToString());
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
		//custom by Yonathan 6 Dec 2022 for QRIS

		private void flushCOMPort()
		{
			GME_Var.serialBCA.DiscardInBuffer();
			GME_Var.serialBCA.DiscardOutBuffer();
			GME_Var.serialBCA.BaseStream.Flush();
			
		}
	  // This is the method to run when the timer is raised.
	  private  void TimerEventProcessor(object myObject, EventArgs myEventArgs)
	  {
		
	  }

	
		//custom by Yonathan 5 Dec 2022 custom QRIS
		private void requestInquiryQRISBCA(string bankName)
		{
			//Declare Transtype here, refer to Bank Documentation
			transType = "32";

			_paymentSuccess = false;
			refNumber = txtReff.Text;

			try
			{
				/*
				 * Set cancel button disabled during ECR Process
				 * to prevent user click Cancel during transaction in EDC
				 */
				btnCancel.Enabled = false;

				GME_Var.transTypeBCA = transType;

				//Create BCA Object using GMECustom library
				ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
				GME_Var.approvalCodeBCA = string.Empty;
				GME_Var.respCodeBCA = string.Empty;
				RetailTransaction retailTransaction = posTransaction as RetailTransaction;

				//Set totalamount based on registered Amount
				int totalAmount = decimal.ToInt32(this.registeredAmount);

				//Try Open Port based on CPPILIHBANK data
				if (GME_Var.serialBCA.IsOpen == false)
				{
					try
					{
						BCAOnline.openPort(cmbPilihBank.SelectedValue.ToString());
					}
					catch (Exception ex)
					{
						MessageBox.Show("error open port");
					}
				}


				if (GME_Var.serialBCA.IsOpen)
				{
					flushCOMPort();
					//Connection open
					_isReceived = false;
					autoEventBCA = new AutoResetEvent(false);

					/*
					 * Send Data to ECR in here
					 * Noticed function WaitCallBank(receiveThreadBCA) is threading
					 * Means that it will wait for receiveThreadBCA to return value for continue process
					 */
						
					try
					{ 
						BCAOnline.sendData(totalAmount.ToString(), transType, Connection.applicationLoc, posTransaction, refNumber);
						Thread.Sleep(500);
						lblWaitingRespond.Visible = true;
						ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
						autoEventBCA.WaitOne();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}

					/*if (autoEventBCA.WaitOne(60000) == false)
						{
							GME_Var.respCodeBCA = "TO";
						}*/
						
						

						if (GME_Var.transTypeBCA == transType)
						{
							btnCancel.Enabled = true;
							lblWaitingRespond.Visible = false;

							//validate response from BCA here
							switch (GME_Var.respCodeBCA)
							{
								case "00":
								case "*0":
									#region ECRSuccess
									//ECR & Transaction success, continue action to insert to DB here
									//refNumber = GME_Var.refNumberBCA;
									int transAmount = int.Parse(GME_Var.amountBCA.Substring(0, GME_Var.amountBCA.Length - 2));
									if (transAmount != retailTransaction.NetAmountWithTaxAndCharges)
									{
										showError("NOMINAL PEMBAYARAN TIDAK SESUAI DENGAN \nYANG HARUS DIBAYARKAN");

										break;
									}
									//int transAmount = int.Parse(GME_Var.amountBCA.Substring(0, GME_Var.amountBCA.Length - 2));
									int otherAmount = int.Parse(GME_Var.otherAmountBCA.Substring(0, GME_Var.otherAmountBCA.Length - 2));
									string pan = GME_Var.refNumberBCA;
									pan = pan.Replace(" ", "");
									/*if (pan.Length >= 16)
									{
										pan = pan.Remove(0, 4);
									}*/
									approvalCode = GME_Var.approvalCodeBCA;

									
									//insert into AX.CPAMBILTUNAI
									SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

									try
									{
										string queryString = @"
											INSERT INTO AX.CPAMBILTUNAITEMP (
																			TRANSACTIONID, 
																			TRXIDEDC,
																			CARDNUMBER, 
																			AMOUNT, 
																			TRANSACTIONTYPE, 
																			BANK, 
																			TENDER, 
																			STORE,
																			TRANSDATE,
																			TRANSACTIONSTATUS,
																			[DATAAREAID],
																			[PARTITION]
																			)
											VALUES (
													@TRANSACTIONID, 
													@TRXIDEDC,
													@CARDNUMBER, 
													@AMOUNT, 
													@TRANSACTIONTYPE, 
													@BANK, 
													@TENDER, 
													@STORE, 
													@TRANSDATE,
													@TRANSACTIONSTATUS,
													@DATAAREAID,
													@PARTITION
													)";

										using (SqlCommand command = new SqlCommand(queryString, connection))
										{
											command.Parameters.AddWithValue("@TRANSACTIONID", this.posTransaction.TransactionId);
											command.Parameters.AddWithValue("@TRXIDEDC", GME_Var.approvalCodeBCA);
											command.Parameters.AddWithValue("@CARDNUMBER", pan);
											command.Parameters.AddWithValue("@AMOUNT", otherAmount);
											command.Parameters.AddWithValue("@TRANSACTIONTYPE", GME_Var.transTypeBCA);
											command.Parameters.AddWithValue("@BANK", bankName);
											command.Parameters.AddWithValue("@TENDER", this.tenderInfo.TenderID);
											command.Parameters.AddWithValue("@Store", this.posTransaction.StoreId);
											command.Parameters.AddWithValue("@TRANSDATE", DateTime.Now);
											command.Parameters.AddWithValue("@TRANSACTIONSTATUS", 0);
											command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);
											command.Parameters.AddWithValue("@PARTITION", 1);

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
										MessageBox.Show(ex.ToString());
										throw;
									}
									finally
									{
										if (connection.State != ConnectionState.Closed)
										{
											connection.Close();
										}
									}
									
									//end insert
									/*
									 * Adjust form design
									 * add value to field
									 * set button enabled/disabled
									 */
									txtPhone.Text = "";// otherAmount + "";
									txtReff.Text = refNumber;//pan;
									btnRequest.Enabled = false;
									btnOk.Enabled = true;
									cmbPilihBank.Enabled = false;
									amtCustAmounts.Enabled = false;

									using (frmMessage dialog = new frmMessage("Payment EDC Success", MessageBoxButtons.OK, MessageBoxIcon.Information))
									{
										POSFormsManager.ShowPOSForm(dialog);
									}
									btnCancel.Enabled = false;
									txtReff.Enabled = false;
									_paymentSuccess = true;
									break;
									#endregion
								case "54":
									showError("Decline Expired Card");
									break;
								case "55":
									showError("Decline Incorrect PIN");
									break;
								case "P2":
									showError("Read Card Error");
									break;
								case "P3":
									showError("User press Cancel on EDC");
									
									break;
								case "Z3":
									showError("EMV Card Decline");
									break;
								case "CE":
									showError("Connection Error/Line Busy");
									break;
								case "TO":
									showError("Connection Timeout");
									break;
								case "TOI":
									showError("Connection Timeout\nSilakan cek koneksi kabel EDC,\nkemudian klik button Inquiry kembali");
									break;
								case "PT":
									showError("EDC Problem / EDC perlu di settlement");
									break;
								case "PS":
									showError("Settlement Failed");
									break;
								case "aa":
									showError("Decline (aa represent two digit alphanumeric value from EDC)");
									break;
								case "S2":
									showError("TRANSAKSI GAGAL");
									break;
								case "S3":
									showError("TXN BLM DIPROSES MINTA SCAN QR");
									break;
								case "S4":
									showError("TXN EXPIRED ULANGI TRANSAKSI");
									break;
								case "TN":
									showError("Topup Tunai Not Ready");
									break;
								case "Q2":
									showError("TRANSAKSI TIDAK DITEMUKAN");
									btnCancel.Enabled = false;
									break;
								case "NAK":
									showError("NAK");
									//btnCancel.Enabled = true;
									break;
								default:
									showError("Unknown Error: " + GME_Var.respCodeBCA);
									break;
							}
						}
						_isReceived = true;
						autoEventBCA.Reset();

					   
					
					
				}
				else
				{
					btnCancel.Enabled = true;
					showError("EDC Not Connected");
				}

				GME_Var.isPoint = false;
				//Close port when transaction done
				BCAOnline.closePort();
			}
			catch (Exception ex)
			{
				showError("Failed to send data");
				btnCancel.Enabled = true;
			}
		}


		#region QRIS custom Code
		private void getURLAPI()
		{

			//APIAccess.APIParameter.mySqlConnString;

			MySqlConnection connection = APIAccess.APIParameter.mySqlConnString;
			try
			{

				string queryString = @"SELECT * FROM CPAPIURLQRIS WHERE PAYMENTMETHOD = @TENDERID";

				using (MySqlCommand command = new MySqlCommand(queryString, connection))
				{
					command.Parameters.AddWithValue("@TENDERID", tenderShopee);
					 

					if (connection.State != System.Data.ConnectionState.Open)
					{
						connection.Open();

					}
					APIAccess.API resultData = new APIAccess.API();
					using (MySqlDataReader reader = command.ExecuteReader())
					//MySqlDataReader reader = command.ExecuteReader();
					{
						if (reader.HasRows)
						{
							while (reader.Read())
							{
								urlCreate = reader["URLCREATE"].ToString();
								urlInvalidate = reader["URLINVALIDATE"].ToString();
								urlNotify = reader["URLNOTIFY"].ToString();                               

							}

                           // MessageBox.Show(string.Format("{0},{1},{2}", urlCreate, urlInvalidate, urlNotify));
							//connection.Dispose();

						}
						else
						{
							using (frmMessage dialog = new frmMessage("Please check internet connection or \nAPI table CPAPIURLQRIS config!!!", MessageBoxButtons.OK, MessageBoxIcon.Error))
							{
								LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
								

							}
						}
					}
					//connection.Close();
				}


			}
			catch (Exception ex)
			{
				//LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				throw;
			}
			finally
			{
				if (connection.State != System.Data.ConnectionState.Closed)
				{
					connection.Dispose();
					connection.Close();

				}
			}
		}



		

		private Bitmap CropImage(Bitmap originalImage)
		{
			int x, y = 0;
			int width, height = 0;
			bool foundPixel = false;

			// Find the top-left corner coordinates of the QR code in the image
			for (x = 0; x < originalImage.Width; x++)
			{
				for (y = 0; y < originalImage.Height; y++)
				{
					if (originalImage.GetPixel(x, y).ToArgb() != Color.White.ToArgb())
					{
						foundPixel = true;
						break;
					}
				}
				if (foundPixel) break;
			}

			// Find the bottom-right corner coordinates of the QR code in the image
			foundPixel = false;
			for (width = originalImage.Width - 1; width >= 0; width--)
			{
				for (height = originalImage.Height - 1; height >= 0; height--)
				{
					if (originalImage.GetPixel(width, height).ToArgb() != Color.White.ToArgb())
					{
						foundPixel = true;
						break;
					}
				}
				if (foundPixel) break;
			}

			// Calculate the dimensions of the QR code
			width = width - x + 1;
			height = height - y + 1;

			// Create a new cropped bitmap
			System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(x, y, width, height);
			Bitmap croppedImage = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(croppedImage))
			{
				g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, width, height), cropRect, GraphicsUnit.Pixel);
			}

			return croppedImage;
		}

		#endregion

        //additional for adm fee by Yonathan 10/06/2024 //CPIADMFEE
        public decimal GetAdmFee(int _tenderId)
        {
            string admFee = "0";
            decimal amountAdmFee = 0;
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            try
            {
                string queryString = @"SELECT ADMFEE 
                                        FROM ax.CPBANKADM 
                                        WHERE TENDERTYPEID = @TENDERID 
                                        AND DATAAREAID = @DATAAREAID
                                        AND FROMDATE <= CAST(GETDATE() AS date) AND TODATE >= CAST(GETDATE() AS date)";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@TENDERID", _tenderId);
                    command.Parameters.AddWithValue("@DATAAREAID", LSRetailPosis.Settings.ApplicationSettings.Database.DATAAREAID);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            admFee = reader[0].ToString();

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

            amountAdmFee = (Math.Truncate(Convert.ToDecimal(admFee) * 1000m) / 1000m);
            return amountAdmFee;
        }
	}

    class Printer
    {
        private PrintDocument printDocument;
        private Image imageToPrint;

        public Printer()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        }

        public void PrintImage(Image image)
        {
            this.imageToPrint = image;
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        public void printQR(Image image)
        {
            // print here
             
            int Offset = 0;
            string s = "QRIS";
            int offset = 0;
            string text_format = "QRIS";
            s += "             " + text_format + Environment.NewLine;
            s += "             Keterangan   : " + Environment.NewLine +
                 "             No. Terima   : " + Environment.NewLine +
                 "             Tgl Terima   : " + DateTime.Now.ToString() + Environment.NewLine +
                 "             No. PO/TO    : " + Environment.NewLine +
                //   "Tgl PO       :" + "Tanggal PO" + Environment.NewLine + //Custom field
                //  "Supplier     :" + "Supplier" + Environment.NewLine + //Custom Field
                 "             DO No        : "  + Environment.NewLine; //delivery note number
            PrintDocument print_document = new PrintDocument();
            PrintDocument p = new PrintDocument();
            PrintDialog pd = new PrintDialog();
            PaperSize psize = new PaperSize("Custom", 100, Offset + 236);
            Margins margins = new Margins(0, 0, 0, 0);

            pd.Document = p;
            pd.Document.DefaultPageSettings.PaperSize = psize;
            pd.Document.DefaultPageSettings.Margins = margins;
            //p.DefaultPageSettings.PaperSize.Width = 600;
            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                //e1.Graphics.DrawString(s, new Font("Courier New", 9), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left + 100, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                //modif by Julius 14 07 2017
                e1.Graphics.DrawString(s, new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(p.DefaultPageSettings.PrintableArea.Left, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

            };
            try
            {
                //Edit by Erwin 23 October 2019
                //for (int i = 1; i <= 2; i++)
                //{
                //    p.Print();
                //}

                p.Print();

                //End Edit by Erwin 23 October 2019
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }

        }

        public void PrintQRIS(Image image)
        {
            // Initialize printing properties
            int offset = 0;
            string text_format = "QRIS";

            PrintDocument print_document = new PrintDocument();
            PrintDialog print_dialog = new PrintDialog();
            //PaperSize paper_size = new PaperSize("Custom", 100, offset + 100);
            PaperSize paper_size = new PaperSize("Custom", 350, 350);//offset + 100);
            Margins margin = new Margins(0, 0, 0, 0);

            // Processing Print
            print_dialog.Document = print_document;
            print_dialog.Document.DefaultPageSettings.PaperSize = paper_size;
            print_dialog.Document.DefaultPageSettings.Margins = margin;
            print_document.DefaultPageSettings.PaperSize.Width = 600;

            print_document.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                // Print text
                e1.Graphics.DrawString(
                    text_format,
                    new Font("Lucida Console", 6),
                    new SolidBrush(Color.Black),
                    new RectangleF(
                        print_document.DefaultPageSettings.PrintableArea.Left,
                        0,
                        print_document.DefaultPageSettings.PrintableArea.Width,
                        print_document.DefaultPageSettings.PrintableArea.Height
                    )
                );

                // Print image
                if (image != null)
                {
                    float textHeight = e1.Graphics.MeasureString(text_format, new Font("Lucida Console", 6)).Height;
                    float imageTop = textHeight + 10; // Add some spacing between text and image
                    RectangleF imageRect = new RectangleF(
                        print_document.DefaultPageSettings.PrintableArea.Left,
                        imageTop,
                        print_document.DefaultPageSettings.PrintableArea.Width,
                        print_document.DefaultPageSettings.PrintableArea.Height - imageTop
                    );
                    //e1.Graphics.DrawImage(image, imageRect);
                    e1.Graphics.DrawImage(image, 100, 50, image.Width, image.Height);

                }
            };

            try
            {
                print_document.Print();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occurred While Printing", ex);
            }
        }

       

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Print the image at the top-left corner of the page
            e.Graphics.DrawImage(imageToPrint, 0, 0, e.PageBounds.Width, e.PageBounds.Height);
        }
    }

}