/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using DevExpress.XtraEditors;
using LSRetailPosis.DataAccess;
using LSRetailPosis.DevUtilities;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Dialog.ProductSearchAttributes;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch
{
	/// <summary>
	/// The types of Brazilian taxes.
	/// </summary>
	internal enum BrazilianTaxType
	{
		Blank = 0,
		Pis = 1,
		Icms = 2,
		Cofins = 3,
		Iss = 4,
		Irrf = 5,
		Inss = 6,
		Ipi = 8,
		ImportTax = 10,
		OtherTax = 11,
		InssRetained = 12
	}

	/// <summary>
	/// Summary description for frmItemSearch.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
	public class frmItemSearch : frmTouchBase
	{
		private DevExpress.XtraGrid.Blending.XtraGridBlending xtraGridBlending1;
		private String selectedItemId;
		private PanelControl basePanel;
		private System.Data.DataTable itemTable;
		private int getHowManyRows;
		private System.Windows.Forms.Timer clockTimer;
		private DateTime lastActiveDateTime;
		private int loadedCount; // = 0;
		private string sortBy;
		private DevExpress.XtraGrid.GridControl grItems;
		private DevExpress.XtraGrid.Views.Grid.GridView grdView;
		private DevExpress.XtraGrid.Columns.GridColumn colItemName;
		private DevExpress.XtraGrid.Columns.GridColumn colItemId;
		private DevExpress.XtraGrid.Columns.GridColumn colItemPrice;
		private DevExpress.XtraGrid.Columns.GridColumn colItemUnitOfMeasure;
		private DevExpress.XtraGrid.Columns.GridColumn colItemTaxRate;
		private DevExpress.XtraGrid.Columns.GridColumn colItemRoundTrunc;
		private DevExpress.XtraGrid.Columns.GridColumn colItemOwnThirdProduc;
		private bool sortAsc = true;
		private TableLayoutPanel tableLayoutPanel1;
		private SimpleButton btnSearch;
		private DevExpress.XtraEditors.TextEdit txtKeyboardInput;
		private SimpleButton btnClear;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgUp;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnUp;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPgDown;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnDown;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnSelect;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnGetprice;
		private TextEdit txtCategory;
		private SimpleButton btnSearchAttributes;
		private SimpleButton btnClearAttributes;
		private TextEdit txtAttributes;
		private LabelControl lblCategory;
		private LabelControl lblAttributes;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnProductDetails;
		private TableLayoutPanel tableLayoutPanel6;
		private TableLayoutPanel tableLayoutPanel2;
		private TableLayoutPanel tableLayoutPanel3;
		private Label lblHeading;
		private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
		private bool inputHasChanged = false;
		private TableLayoutPanel tableLayoutPanel4;
		private RadioButton radioCurrentStore;
		private RadioButton radioAllProducts;
		private bool itemSearchResultsFromAX = false;

		private IProductSearchFilter searchFilterRuntimeData = null;
		private IProductSearchFilter searchFilterRuntimeCache = null;

		private string categoryId;
		private string categoryName;
		private long categoryIdLong;
		private TableLayoutPanel tblLayoutPanelAttributesSearch;

		private readonly ItemData itemDataCategory = new ItemData(ApplicationSettings.Database.LocalConnection,
								ApplicationSettings.Database.DATAAREAID,
								ApplicationSettings.Terminal.StorePrimaryId);

		private DevExpress.XtraGrid.Columns.GridColumn colSearchString; //add by rs 19 Juni 2019
		[Import]
		public IPosTransaction posTransaction { get; set; }

		protected frmItemSearch()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CategoryId = null;
		}

		public frmItemSearch(int howManyRows, IPosTransaction thisPosTransaction)
			: this()
		{
			posTransaction = thisPosTransaction; //= PosTransaction;
			if (howManyRows == 0)
			{
				getHowManyRows = 1000;
			}
			else if (howManyRows == -1)
			{
				// Just to be safe we handle -1 since in the past -1 was used to represent that all of the
				// records should be fetched.
				getHowManyRows = 500;
			}
			else
			{
				getHowManyRows = howManyRows;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!this.DesignMode)
			{
				grdView.FocusedColumn = colItemName;

				//
				// Get all text through the Translation function in the ApplicationLocalizer
				//
				// TextID's for frmItemSearch are reserved at 61500 - 61599
				// In use now are ID's 61500 - 61513

				colItemId.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(61507); //Product number
				colItemName.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(61506); //Product name
				colItemPrice.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(61513); //Item price
				btnGetprice.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(61511); //Show price
				btnSelect.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(61514); //OK
				btnCancel.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(51114); //Cancel
				//title
				this.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(61612); //Product search
				lblHeading.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(61612); //Product search
				sortBy = colItemId.FieldName;

				colItemUnitOfMeasure.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(86168); //Unit of measure
				colItemRoundTrunc.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(86169); //Round or trunc
				colItemOwnThirdProduc.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(86170); //Own or third
				colItemTaxRate.Caption = LSRetailPosis.ApplicationLocalizer.Language.Translate(86171); //Trib Situation
				radioCurrentStore.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(99888); //Remote Search Checkbox
				radioAllProducts.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(99891); //Remote Search Checkbox

				lblCategory.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(99804); //Category
				lblAttributes.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(99811); //Product attributes
				btnProductDetails.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(99800); //Product details

				if (LSRetailPosis.Settings.ApplicationSettings.Terminal.AutoLogOffTimeOutInMin > 0)
				{
					clockTimer.Enabled = true;
				}

				CheckRowPosition();
				lastActiveDateTime = DateTime.Now;

				if (Functions.CountryRegion == SupportedCountryRegion.BR)
				{
					colItemPrice.Visible = colItemOwnThirdProduc.Visible = colItemRoundTrunc.Visible = colItemUnitOfMeasure.Visible = colItemTaxRate.Visible = colItemPrice.Visible = true;
					btnGetprice.Visible = false;
				}
				//this will show watermark text in the txtKeyboardInput textbox
				this.txtKeyboardInput.Properties.NullValuePrompt = LSRetailPosis.ApplicationLocalizer.Language.Translate(3092);//Enter your search term
				this.txtKeyboardInput.Properties.NullValuePromptShowForEmptyValue = true;
				this.txtKeyboardInput.Focus();

				if (this.AttributeSearchEnabled)
				{
					SwitchAttributeSearchControls(true);
					this.txtCategory.Text = this.CategoryName;
					this.txtAttributes.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(107108); // None
					InitProductSearchFilter();

					SearchItems();
				}
				else
				{
					SwitchAttributeSearchControls(false);
				}
			}

			base.OnLoad(e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() 
		{
			this.xtraGridBlending1 = new DevExpress.XtraGrid.Blending.XtraGridBlending();
			this.basePanel = new DevExpress.XtraEditors.PanelControl();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblHeading = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnPgDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnPgUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnGetprice = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnUp = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnDown = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnSelect = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.btnProductDetails = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtKeyboardInput = new DevExpress.XtraEditors.TextEdit();
			this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
			this.btnClear = new DevExpress.XtraEditors.SimpleButton();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.radioCurrentStore = new System.Windows.Forms.RadioButton();
			this.radioAllProducts = new System.Windows.Forms.RadioButton();
			this.grItems = new DevExpress.XtraGrid.GridControl();
			this.grdView = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colItemName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colItemId = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colItemPrice = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colItemUnitOfMeasure = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colItemTaxRate = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colItemOwnThirdProduc = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colItemRoundTrunc = new DevExpress.XtraGrid.Columns.GridColumn();
			this.tblLayoutPanelAttributesSearch = new System.Windows.Forms.TableLayoutPanel();
			this.btnClearAttributes = new DevExpress.XtraEditors.SimpleButton();
			this.lblAttributes = new DevExpress.XtraEditors.LabelControl();
			this.btnSearchAttributes = new DevExpress.XtraEditors.SimpleButton();
			this.lblCategory = new DevExpress.XtraEditors.LabelControl();
			this.txtAttributes = new DevExpress.XtraEditors.TextEdit();
			this.txtCategory = new DevExpress.XtraEditors.TextEdit();
			this.clockTimer = new System.Windows.Forms.Timer();
			((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.basePanel)).BeginInit();
			this.basePanel.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtKeyboardInput.Properties)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grItems)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdView)).BeginInit();
			this.tblLayoutPanelAttributesSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtAttributes.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCategory.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// xtraGridBlending1
			// 
			this.xtraGridBlending1.AlphaStyles.AddReplace("Row", 220);
			// 
			// basePanel
			// 
			this.basePanel.Controls.Add(this.tableLayoutPanel2);
			this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.basePanel.Location = new System.Drawing.Point(0, 0);
			this.basePanel.Name = "basePanel";
			this.basePanel.Size = new System.Drawing.Size(1024, 768);
			this.basePanel.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lblHeading, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(26, 25, 26, 11);
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1020, 764);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// lblHeading
			// 
			this.lblHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lblHeading.AutoSize = true;
			this.lblHeading.Font = new System.Drawing.Font("Segoe UI Light", 36F);
			this.lblHeading.Location = new System.Drawing.Point(411, 25);
			this.lblHeading.Margin = new System.Windows.Forms.Padding(0);
			this.lblHeading.Name = "lblHeading";
			this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
			this.lblHeading.Size = new System.Drawing.Size(198, 85);
			this.lblHeading.TabIndex = 0;
			this.lblHeading.Tag = "";
			this.lblHeading.Text = "Heading";
			this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 10;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.btnCancel, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnPgDown, 9, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnPgUp, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnGetprice, 5, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnUp, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnDown, 8, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnSelect, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnProductDetails, 6, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(26, 688);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 11, 0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(968, 65);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnCancel.Appearance.Options.UseFont = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnCancel.Location = new System.Drawing.Point(347, 4);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(130, 57);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Tag = "";
			this.btnCancel.Text = "Cancel";
			// 
			// btnPgDown
			// 
			this.btnPgDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPgDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnPgDown.Appearance.Options.UseFont = true;
			this.btnPgDown.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.bottom;
			this.btnPgDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnPgDown.Location = new System.Drawing.Point(897, 4);
			this.btnPgDown.Margin = new System.Windows.Forms.Padding(4);
			this.btnPgDown.Name = "btnPgDown";
			this.btnPgDown.Size = new System.Drawing.Size(67, 57);
			this.btnPgDown.TabIndex = 7;
			this.btnPgDown.Tag = "";
			this.btnPgDown.Text = "Ê";
			this.btnPgDown.Click += new System.EventHandler(this.btnPgDown_Click);
			// 
			// btnPgUp
			// 
			this.btnPgUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPgUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnPgUp.Appearance.Options.UseFont = true;
			this.btnPgUp.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.top;
			this.btnPgUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnPgUp.Location = new System.Drawing.Point(4, 4);
			this.btnPgUp.Margin = new System.Windows.Forms.Padding(4);
			this.btnPgUp.Name = "btnPgUp";
			this.btnPgUp.Size = new System.Drawing.Size(65, 57);
			this.btnPgUp.TabIndex = 0;
			this.btnPgUp.Tag = "";
			this.btnPgUp.Text = "Ç";
			this.btnPgUp.Click += new System.EventHandler(this.btnPgUp_Click);
			// 
			// btnGetprice
			// 
			this.btnGetprice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGetprice.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGetprice.Appearance.Options.UseFont = true;
			this.btnGetprice.Location = new System.Drawing.Point(485, 4);
			this.btnGetprice.Margin = new System.Windows.Forms.Padding(4);
			this.btnGetprice.Name = "btnGetprice";
			this.btnGetprice.Size = new System.Drawing.Size(127, 57);
			this.btnGetprice.TabIndex = 4;
			this.btnGetprice.Tag = "";
			this.btnGetprice.Text = "Show price";
			this.btnGetprice.Click += new System.EventHandler(this.btnGetprice_Click_1);
			// 
			// btnUp
			// 
			this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUp.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnUp.Appearance.Options.UseFont = true;
			this.btnUp.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.up;
			this.btnUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnUp.Location = new System.Drawing.Point(77, 4);
			this.btnUp.Margin = new System.Windows.Forms.Padding(4);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(65, 57);
			this.btnUp.TabIndex = 1;
			this.btnUp.Tag = "";
			this.btnUp.Text = "ñ";
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click_2);
			// 
			// btnDown
			// 
			this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDown.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnDown.Appearance.Options.UseFont = true;
			this.btnDown.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.down;
			this.btnDown.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnDown.Location = new System.Drawing.Point(824, 4);
			this.btnDown.Margin = new System.Windows.Forms.Padding(4);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(65, 57);
			this.btnDown.TabIndex = 6;
			this.btnDown.Tag = "";
			this.btnDown.Text = "ò";
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click_2);
			// 
			// btnSelect
			// 
			this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelect.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnSelect.Appearance.Options.UseFont = true;
			this.btnSelect.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnSelect.Location = new System.Drawing.Point(212, 4);
			this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(127, 57);
			this.btnSelect.TabIndex = 2;
			this.btnSelect.Tag = "";
			this.btnSelect.Text = "OK";
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click_1);
			// 
			// btnProductDetails
			// 
			this.btnProductDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnProductDetails.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnProductDetails.Appearance.Options.UseFont = true;
			this.btnProductDetails.Enabled = false;
			this.btnProductDetails.Location = new System.Drawing.Point(620, 4);
			this.btnProductDetails.Margin = new System.Windows.Forms.Padding(4);
			this.btnProductDetails.Name = "btnProductDetails";
			this.btnProductDetails.Size = new System.Drawing.Size(134, 57);
			this.btnProductDetails.TabIndex = 5;
			this.btnProductDetails.Tag = "";
			this.btnProductDetails.Text = "Product details";
			this.btnProductDetails.Click += new System.EventHandler(this.btnProductDetails_Click);
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.ColumnCount = 8;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.grItems, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.tblLayoutPanelAttributesSearch, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(30, 114);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.Padding = new System.Windows.Forms.Padding(10);
			this.tableLayoutPanel6.RowCount = 3;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(960, 559);
			this.tableLayoutPanel6.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel6.SetColumnSpan(this.tableLayoutPanel1, 8);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.txtKeyboardInput, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnSearch, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnClear, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(940, 68);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// txtKeyboardInput
			// 
			this.txtKeyboardInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtKeyboardInput.Location = new System.Drawing.Point(3, 3);
			this.txtKeyboardInput.Name = "txtKeyboardInput";
			this.txtKeyboardInput.Properties.Appearance.BackColor = System.Drawing.Color.White;
			this.txtKeyboardInput.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
			this.txtKeyboardInput.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
			this.txtKeyboardInput.Properties.Appearance.Options.UseBackColor = true;
			this.txtKeyboardInput.Properties.Appearance.Options.UseFont = true;
			this.txtKeyboardInput.Properties.Appearance.Options.UseForeColor = true;
			this.txtKeyboardInput.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.txtKeyboardInput.Size = new System.Drawing.Size(808, 32);
			this.txtKeyboardInput.TabIndex = 0;
			this.txtKeyboardInput.TextChanged += new System.EventHandler(this.txtKeyboardInput_TextChanged);
			this.txtKeyboardInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemSearch_KeyDown);
			this.txtKeyboardInput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtKeyboardInput_PreviewKeyDown);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnSearch.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.search;
			this.btnSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnSearch.Location = new System.Drawing.Point(817, 3);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(57, 32);
			this.btnSearch.TabIndex = 1;
			this.btnSearch.Text = "Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnClear.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
			this.btnClear.Appearance.Options.UseFont = true;
			this.btnClear.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.remove;
			this.btnClear.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnClear.Location = new System.Drawing.Point(880, 3);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(57, 32);
			this.btnClear.TabIndex = 2;
			this.btnClear.Text = "Clear";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.25387F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.74613F));
			this.tableLayoutPanel4.Controls.Add(this.radioCurrentStore, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.radioAllProducts, 1, 0);
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 41);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(302, 27);
			this.tableLayoutPanel4.TabIndex = 3;
			// 
			// radioCurrentStore
			// 
			this.radioCurrentStore.AutoSize = true;
			this.radioCurrentStore.Checked = true;
			this.radioCurrentStore.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this.radioCurrentStore.Location = new System.Drawing.Point(3, 3);
			this.radioCurrentStore.Name = "radioCurrentStore";
			this.radioCurrentStore.Size = new System.Drawing.Size(159, 21);
			this.radioCurrentStore.TabIndex = 0;
			this.radioCurrentStore.TabStop = true;
			this.radioCurrentStore.Text = "Current store products";
			this.radioCurrentStore.UseVisualStyleBackColor = true;
			this.radioCurrentStore.CheckedChanged += new System.EventHandler(this.radioCurrentStore_CheckedChanged);
			// 
			// radioAllProducts
			// 
			this.radioAllProducts.AutoSize = true;
			this.radioAllProducts.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this.radioAllProducts.Location = new System.Drawing.Point(203, 3);
			this.radioAllProducts.Name = "radioAllProducts";
			this.radioAllProducts.Size = new System.Drawing.Size(96, 21);
			this.radioAllProducts.TabIndex = 1;
			this.radioAllProducts.Text = "All products";
			this.radioAllProducts.UseVisualStyleBackColor = true;
			this.radioAllProducts.CheckedChanged += new System.EventHandler(this.radioAllProducts_CheckedChanged);
			// 
			// grItems
			// 
			this.grItems.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.tableLayoutPanel6.SetColumnSpan(this.grItems, 8);
			this.grItems.Cursor = System.Windows.Forms.Cursors.Default;
			this.grItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grItems.Location = new System.Drawing.Point(10, 154);
			this.grItems.MainView = this.grdView;
			this.grItems.Margin = new System.Windows.Forms.Padding(0);
			this.grItems.Name = "grItems";
			this.grItems.Size = new System.Drawing.Size(940, 395);
			this.grItems.TabIndex = 2;
			this.grItems.TabStop = false;
			this.grItems.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
			this.grdView});
			this.grItems.Click += new System.EventHandler(this.grItems_Click);
			this.grItems.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
			this.grItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grItems_KeyDown);
			// 
			// grdView
			// 
			this.grdView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.grdView.Appearance.HeaderPanel.Options.UseFont = true;
			this.grdView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.grdView.Appearance.Row.Options.UseFont = true;
			this.grdView.ColumnPanelRowHeight = 40;
			this.grdView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
			this.colItemName,
			this.colItemId,
			this.colItemPrice,
			this.colItemUnitOfMeasure,
			this.colItemTaxRate,
			this.colItemOwnThirdProduc,
			this.colItemRoundTrunc});
			this.grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
			this.grdView.GridControl = this.grItems;
			this.grdView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
			this.grdView.Name = "grdView";
			this.grdView.OptionsBehavior.Editable = false;
			this.grdView.OptionsCustomization.AllowFilter = false;
			this.grdView.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.grdView.OptionsView.ShowGroupPanel = false;
			this.grdView.OptionsView.ShowIndicator = false;
			this.grdView.RowHeight = 30;
			this.grdView.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
			this.grdView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
			this.grdView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
			this.grdView.Click += new System.EventHandler(this.grdView_Click);
			this.grdView.FocusedRowChanged += OnGridView_FocusedRowChanged;
			// 
			// colItemName
			// 
			this.colItemName.Caption = "Item Name";
			this.colItemName.FieldName = "ITEMNAME";
			this.colItemName.Name = "colItemName";
			this.colItemName.OptionsColumn.AllowEdit = false;
			this.colItemName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colItemName.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
			this.colItemName.Visible = true;
			this.colItemName.VisibleIndex = 1;
			this.colItemName.Width = 291;
			// 
			// colItemId
			// 
			this.colItemId.Caption = "Item number";
			this.colItemId.FieldName = "ITEMID";
			this.colItemId.Name = "colItemId";
			this.colItemId.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colItemId.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
			this.colItemId.Visible = true;
			this.colItemId.VisibleIndex = 0;
			this.colItemId.Width = 161;
			// 
			// colItemPrice
			// 
			this.colItemPrice.AppearanceCell.Options.UseTextOptions = true;
			this.colItemPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.colItemPrice.AppearanceHeader.Options.UseTextOptions = true;
			this.colItemPrice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.colItemPrice.Caption = "Price";
			this.colItemPrice.DisplayFormat.FormatString = "n:0";
			this.colItemPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.colItemPrice.FieldName = "ITEMPRICE";
			this.colItemPrice.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
			this.colItemPrice.Name = "colItemPrice";
			this.colItemPrice.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colItemPrice.Width = 130;
			// 
			// colItemUnitOfMeasure
			// 
			this.colItemUnitOfMeasure.Caption = "Unit of Measure";
			this.colItemUnitOfMeasure.FieldName = "UNITOFMEASURE";
			this.colItemUnitOfMeasure.Name = "colItemUnitOfMeasure";
			this.colItemUnitOfMeasure.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colItemUnitOfMeasure.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
			this.colItemUnitOfMeasure.Width = 57;
			// 
			// colItemTaxRate
			// 
			this.colItemTaxRate.Caption = "TAX";
			this.colItemTaxRate.FieldName = "TAX";
			this.colItemTaxRate.Name = "colItemTaxRate";
			this.colItemTaxRate.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colItemTaxRate.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
			this.colItemTaxRate.Width = 50;
			// 
			// colItemOwnThirdProduc
			// 
			this.colItemOwnThirdProduc.Caption = "IPPT";
			this.colItemOwnThirdProduc.FieldName = "OWNTHIRDPROD";
			this.colItemOwnThirdProduc.Name = "colItemOwnThirdProduc";
			this.colItemOwnThirdProduc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
			this.colItemOwnThirdProduc.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
			this.colItemOwnThirdProduc.Width = 57;
			// 
			// colItemRoundTrunc
			// 
			this.colItemRoundTrunc.Caption = "IAT";
			this.colItemRoundTrunc.FieldName = "ROUNDTRUNC";
			this.colItemRoundTrunc.Name = "colItemRoundTrunc";
			this.colItemRoundTrunc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.colItemRoundTrunc.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
			this.colItemRoundTrunc.Width = 50;
			// 
			// tblLayoutPanelAttributesSearch
			// 
			this.tblLayoutPanelAttributesSearch.AutoSize = true;
			this.tblLayoutPanelAttributesSearch.ColumnCount = 4;
			this.tableLayoutPanel6.SetColumnSpan(this.tblLayoutPanelAttributesSearch, 8);
			this.tblLayoutPanelAttributesSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutPanelAttributesSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutPanelAttributesSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutPanelAttributesSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutPanelAttributesSearch.Controls.Add(this.btnClearAttributes, 3, 1);
			this.tblLayoutPanelAttributesSearch.Controls.Add(this.lblAttributes, 0, 1);
			this.tblLayoutPanelAttributesSearch.Controls.Add(this.btnSearchAttributes, 2, 1);
			this.tblLayoutPanelAttributesSearch.Controls.Add(this.lblCategory, 0, 0);
			this.tblLayoutPanelAttributesSearch.Controls.Add(this.txtAttributes, 1, 1);
			this.tblLayoutPanelAttributesSearch.Controls.Add(this.txtCategory, 1, 0);
			this.tblLayoutPanelAttributesSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayoutPanelAttributesSearch.Location = new System.Drawing.Point(10, 78);
			this.tblLayoutPanelAttributesSearch.Margin = new System.Windows.Forms.Padding(0);
			this.tblLayoutPanelAttributesSearch.Name = "tblLayoutPanelAttributesSearch";
			this.tblLayoutPanelAttributesSearch.RowCount = 2;
			this.tblLayoutPanelAttributesSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutPanelAttributesSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutPanelAttributesSearch.Size = new System.Drawing.Size(940, 76);
			this.tblLayoutPanelAttributesSearch.TabIndex = 4;
			// 
			// btnClearAttributes
			// 
			this.btnClearAttributes.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnClearAttributes.Appearance.Font = new System.Drawing.Font("Wingdings", 21.75F);
			this.btnClearAttributes.Appearance.Options.UseFont = true;
			this.btnClearAttributes.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.remove;
			this.btnClearAttributes.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnClearAttributes.Location = new System.Drawing.Point(880, 41);
			this.btnClearAttributes.Name = "btnClearAttributes";
			this.btnClearAttributes.Size = new System.Drawing.Size(57, 32);
			this.btnClearAttributes.TabIndex = 5;
			this.btnClearAttributes.Text = "Clear";
			this.btnClearAttributes.Click += new System.EventHandler(this.btnClearAttributes_Click);
			// 
			// lblAttributes
			// 
			this.lblAttributes.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAttributes.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAttributes.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblAttributes.Location = new System.Drawing.Point(3, 49);
			this.lblAttributes.Name = "lblAttributes";
			this.lblAttributes.Size = new System.Drawing.Size(125, 16);
			this.lblAttributes.TabIndex = 2;
			this.lblAttributes.Text = "Product attributes:";
			// 
			// btnSearchAttributes
			// 
			this.btnSearchAttributes.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnSearchAttributes.Image = global::Microsoft.Dynamics.Retail.Pos.Dialog.Properties.Resources.search;
			this.btnSearchAttributes.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnSearchAttributes.Location = new System.Drawing.Point(817, 41);
			this.btnSearchAttributes.Name = "btnSearchAttributes";
			this.btnSearchAttributes.Size = new System.Drawing.Size(57, 32);
			this.btnSearchAttributes.TabIndex = 4;
			this.btnSearchAttributes.Text = "Search";
			this.btnSearchAttributes.Click += new System.EventHandler(this.btnSearchAttributes_Click);
			// 
			// lblCategory
			// 
			this.lblCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCategory.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCategory.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblCategory.Location = new System.Drawing.Point(3, 11);
			this.lblCategory.Name = "lblCategory";
			this.lblCategory.Size = new System.Drawing.Size(125, 16);
			this.lblCategory.TabIndex = 0;
			this.lblCategory.Text = "Category:";
			// 
			// txtAttributes
			// 
			this.txtAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAttributes.Location = new System.Drawing.Point(134, 41);
			this.txtAttributes.Name = "txtAttributes";
			this.txtAttributes.Properties.Appearance.BackColor = System.Drawing.Color.White;
			this.txtAttributes.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
			this.txtAttributes.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
			this.txtAttributes.Properties.Appearance.Options.UseBackColor = true;
			this.txtAttributes.Properties.Appearance.Options.UseFont = true;
			this.txtAttributes.Properties.Appearance.Options.UseForeColor = true;
			this.txtAttributes.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.txtAttributes.Size = new System.Drawing.Size(677, 32);
			this.txtAttributes.TabIndex = 3;
			this.txtAttributes.Properties.ReadOnly = true;
			// 
			// txtCategory
			// 
			this.txtCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCategory.Location = new System.Drawing.Point(134, 3);
			this.txtCategory.Name = "txtCategory";
			this.txtCategory.Properties.Appearance.BackColor = System.Drawing.Color.White;
			this.txtCategory.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
			this.txtCategory.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
			this.txtCategory.Properties.Appearance.Options.UseBackColor = true;
			this.txtCategory.Properties.Appearance.Options.UseFont = true;
			this.txtCategory.Properties.Appearance.Options.UseForeColor = true;
			this.txtCategory.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.txtCategory.Size = new System.Drawing.Size(677, 32);
			this.txtCategory.TabIndex = 1;
			this.txtCategory.Properties.ReadOnly = true;
			// 
			// clockTimer
			// 
			this.clockTimer.Interval = 1000;
			this.clockTimer.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// frmItemSearch
			// 
			this.Appearance.BackColor = System.Drawing.Color.White;
			this.Appearance.Options.UseBackColor = true;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.basePanel);
			this.LookAndFeel.SkinName = "Money Twins";
			this.Name = "frmItemSearch";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Item search";
			this.Controls.SetChildIndex(this.basePanel, 0);
			((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.basePanel)).EndInit();
			this.basePanel.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtKeyboardInput.Properties)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grItems)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdView)).EndInit();
			this.tblLayoutPanelAttributesSearch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtAttributes.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCategory.Properties)).EndInit();
			this.ResumeLayout(false);

			//add by rs 19 Juni 2019
			this.colSearchString = new DevExpress.XtraGrid.Columns.GridColumn(); 

			this.grdView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] 
			{             
				this.colItemName,             
				this.colItemId,             
				this.colSearchString,             
				this.colItemPrice,             
				this.colItemUnitOfMeasure,             
				this.colItemTaxRate,             
				this.colItemOwnThirdProduc,             
				this.colItemRoundTrunc
			}); 
 
			// colSearchString
			this.colSearchString.Caption = "Search name";             
			this.colSearchString.FieldName = "SEARCHNAME";             
			this.colSearchString.Name = "colSearchString";
			this.colSearchString.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;             
			this.colSearchString.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;             
			this.colSearchString.Visible = true;             
			this.colSearchString.VisibleIndex = 0;             
			this.colSearchString.Width = 161; 
			// end add

		}

		#endregion

		public void txtKeyboardInput_TextChanged(object sender, EventArgs e)
		{
			inputHasChanged = true;
		}

		private void ItemSearch_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Return:
					EnterPress();
					break;
				case Keys.Up:
					keyboard1_UpButtonPressed();
					break;
				case Keys.Down:
					keyboard1_DownButtonPressed();
					break;
				case Keys.PageUp:
					keyboard1_PgUpButtonPressed();
					break;
				case Keys.PageDown:
					keyboard1_PgDownButtonPressed();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Call this function when enter button has been pressed
		/// </summary>
		private void EnterPress()
		{
			if (!inputHasChanged)
			{
				keyboard1_EnterButtonPressedWithoutInputChange();
			}
			else
			{
				keyboard1_EnterButtonPressed();
			}

			inputHasChanged = false;
		}

		private void InitProductSearchFilter()
		{
			searchFilterRuntimeData = new ProductSearchFilter();
			searchFilterRuntimeData.CategoryId = CategoryIdLong;
			searchFilterRuntimeData.OrderBy = sortBy.ToUpper();
			searchFilterRuntimeData.SortAscending = sortAsc;
			searchFilterRuntimeData.RowsCount = getHowManyRows;
		}

		public String SelectedItemId
		{
			get { return selectedItemId; }
		}

		/// <summary>
		/// The product categoryId is using for filtering products.
		/// </summary>
		public string CategoryId
		{
			get { return categoryId; }
			set
			{
				try
				{
					if (string.IsNullOrEmpty(value) || !long.TryParse(value, out categoryIdLong))
					{
						categoryId = string.Empty;
						CategoryName = string.Empty;
						return;
					}
					categoryId = value;
					CategoryName = itemDataCategory.GetCategoryDetails(categoryIdLong).Item1;
				}
				catch (Exception ex)
				{
					categoryId = string.Empty;
					CategoryName = string.Empty;
					CategoryIdLong = 0;

					LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
					Dialog.InternalApplication.Services.Dialog.ShowMessage(99890, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private bool AttributeSearchEnabled
		{
			get { return !string.IsNullOrEmpty(this.CategoryId); }
		}

		private long CategoryIdLong
		{
			get { return categoryIdLong; }
			set { categoryIdLong = value; }
		}

		private string CategoryName
		{
			get { return categoryName; }
			set { categoryName = value; }
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			grdView.MoveNextPage();
			int topRowIndex = grdView.TopRowIndex;
			if ((grdView.IsLastRow) && (grdView.RowCount > 0))
			{
				using (DataTable newItemTable = GetItemList(loadedCount + 1, txtKeyboardInput.Text))
				{
					itemTable.Merge(newItemTable);
				}
				grdView.TopRowIndex = topRowIndex;
			}

			FillInPrices();
			CheckRowPosition();
			txtKeyboardInput.Select();
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			grdView.MovePrevPage();
			FillInPrices();
			CheckRowPosition();
			txtKeyboardInput.Select();
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			SelectItem();
		}

		private void keyboard1_EnterButtonPressed()
		{
			NetTracer.Information("frmItemSearch::keyboard1_EnterButtonPressed Started.");
			lastActiveDateTime = DateTime.Now;

			if (txtKeyboardInput.Text.Trim().Length == 0)
			{
				btnSelect_Click(null, null);
			}
			else
			{
				SearchItems();
			}

			CheckRowPosition();
			NetTracer.Information("frmItemSearch::keyboard1_EnterButtonPressed Completed.");
		}

		private void SearchItems()
		{
			DataTable newItemTable;
			if (!radioAllProducts.Checked)
			{
				newItemTable = GetItemList(0, txtKeyboardInput.Text);
			}
			// Search for items in head office if there are no search results in POS
			else
			{
				newItemTable = GetProductDataList(txtKeyboardInput.Text);

				if (newItemTable.Rows.Count == 0)
				{
					LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(99889);  // Item not found
				}
				else
				{
					itemSearchResultsFromAX = true;
				}
			}

			if (itemTable != null)
			{
				itemTable.Dispose();
			}

			if (Functions.CountryRegion == SupportedCountryRegion.BR)
			{
				AddBrazilianFieldsRequiredsByLaw(ref newItemTable);
			}

			grItems.DataSource = itemTable = newItemTable;

			if (!itemSearchResultsFromAX)
			{
				FillInPrices();
			}

			if (newItemTable != null)
			{
				newItemTable.Dispose();
			}
		}

		private void AddBrazilianFieldsRequiredsByLaw(ref DataTable itemsTable)
		{
			// Brazil localization
			if (itemsTable != null)
			{
				const string defaultRoundTruncate = "A";
				const string ownProduction = "P";
				const string thirdPartyProduction = "T";
				var thirdPartyProductTypeIds = new [] {"00", "07", "08", "10", "99"};

				//Brazilian law requires these fields
				itemsTable.Columns.Add(new DataColumn("OWNTHIRDPROD"));
				itemsTable.Columns.Add(new DataColumn("ROUNDTRUNC"));
				itemsTable.Columns.Add(new DataColumn("TAX"));

				foreach (DataRow row in itemsTable.Rows)
				{
					var productTypeId = Utility.ToStr(row["INVENTPRODUCTTYPE_BR"]);

					row["OWNTHIRDPROD"] = thirdPartyProductTypeIds.Contains(productTypeId) ? thirdPartyProduction : ownProduction;
					row["TAX"] = GetTaxLine(Utility.ToStr(row["ITEMID"]));
					row["ROUNDTRUNC"] = defaultRoundTruncate;
				}
			}
		}

		private void gridView1_DoubleClick(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;

			Point p = grdView.GridControl.PointToClient(MousePosition);
			DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = grdView.CalcHitInfo(p);

			if (info.HitTest != DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.Column)
			{
				SelectItem();
			}
		}

		private void SelectItem()
		{
			lastActiveDateTime = DateTime.Now;
			if (grdView.RowCount > 0)
			{
				System.Data.DataRow Row = grdView.GetDataRow(grdView.GetSelectedRows()[0]);
				selectedItemId = (string)Row[colItemId.FieldName];
				//add by Yonathan 4/Nov/2022 for stock positive
				RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                if (retailTransaction != null)
                {
                    if (((RetailTransaction)posTransaction).LastRunOperation == Microsoft.Dynamics.Retail.Pos.Contracts.PosisOperations.BlankOperation)
                    {
                        SelectItemCustom(selectedItemId, posTransaction);
                    }
                    else
                    {
                        SelectItem(selectedItemId);
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }


                //if (((InternalTransaction)posTransaction).LastRunOperation == Microsoft.Dynamics.Retail.Pos.Contracts.PosisOperations.BlankOperation)
                //{
                //    SelectItemCustom(selectedItemId, posTransaction);
                //}
                //else
                //{
                //    SelectItem(selectedItemId);
                //}
				//end add
				//SelectItem(selectedItemId); disable
				
			}
			else
			{
				txtKeyboardInput.Select();
			}
		}

		private void SelectItemCustom(string itemId,  IPosTransaction posTransaction)
		{
			//add by Yonathan to prevent error when adding the same item more than once 3 Nov 2022 stock positiuve
			RetailTransaction retailTransaction = posTransaction as RetailTransaction;
			decimal qtyInput = 0;

			//check if line selected
			if (retailTransaction == null) //|| retailTransaction.CalculableSalesLines.Count == 0)
			{
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                ////Application.RunOperation(PosisOperations.ItemSale, "123");
                //LSRetailPosis.POSProcesses.ItemSale iSale = new LSRetailPosis.POSProcesses.ItemSale();
                //iSale.OperationID = PosisOperations.ItemSale;
                //iSale.OperationInfo = new LSRetailPosis.POSProcesses.OperationInfo();
                ////iSale.Barcode = skuId; disable by Yonathan 21/10/2022
                //iSale.Barcode = itemId;//txtSKU.Text;  // change to this by yonathan 21/10/2022
                ////iSale.BarcodeInfo.ItemId = txtSKU.Text;
                //iSale.OperationInfo.NumpadQuantity = 1;
                //iSale.POSTransaction = (PosTransaction)posTransaction;

                //iSale.RunOperation();
			}
			else
			{
				for (int i = 0; i < ((RetailTransaction)posTransaction).SaleItems.Count; i++)
				{
					string thisItemId = "";
					LSRetailPosis.Transaction.Line.SaleItem.SaleLineItem currentLine = retailTransaction.GetItem(((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId);
					int lineId = ((RetailTransaction)posTransaction).SaleItems.ElementAt(i).LineId;

					if (currentLine.ItemId == itemId && currentLine.Voided == false)
					{

						qtyInput = 1;
						currentLine.QuantityOrdered = qtyInput;


						try
						{
							PreTriggerResult preTriggerResult = new LSRetailPosis.POSProcesses.PreTriggerResult();
							PosApplication.Instance.Triggers.Invoke<IItemTrigger>((Action<IItemTrigger>)(t => t.PostSetQty(posTransaction, currentLine)));
							currentLine.Quantity += currentLine.QuantityOrdered;

						}
						catch (Exception ex)
						{
							LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
						}
						this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

						//sl.CalculateLine();

						retailTransaction.CalcTotals();
					}
					else
					{
						this.DialogResult = System.Windows.Forms.DialogResult.OK;
					}


				}


			}
			//end
		}

		private void SelectItem(string itemId)
		{
			lastActiveDateTime = DateTime.Now;
			selectedItemId = itemId;
			//Create Data for the Selected Item
			if (itemSearchResultsFromAX)
			{
				CreateProductData(itemId);
			}


			
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			txtKeyboardInput.Text = string.Empty;

			SearchAfterClear();
		}

		private void SearchAfterClear()
		{
			DataTable newItemTable;

			newItemTable = GetItemList(0, txtKeyboardInput.Text);

			if (itemTable != null)
			{
				itemTable.Dispose();
			}

			grItems.DataSource = itemTable = newItemTable;
			FillInPrices();

			txtKeyboardInput.Select();
		}

		private void btnUp_Click_1(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			grdView.MovePrev();
			FillInPrices();
			CheckRowPosition();
			txtKeyboardInput.Select();
		}

		private void btnDown_Click_1(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			grdView.MoveNext();
			int topRowIndex = grdView.TopRowIndex;
			if ((grdView.IsLastRow) && (grdView.RowCount > 0))
			{
				using (DataTable newItemTable = GetItemList(loadedCount + 1, txtKeyboardInput.Text))
				{
					itemTable.Merge(newItemTable);
				}
				grdView.TopRowIndex = topRowIndex;
			}

			FillInPrices();
			CheckRowPosition();
			txtKeyboardInput.Select();
		}

		private void FillInPrices()
		{
			if (colItemPrice.Visible & !itemSearchResultsFromAX)
			{
				try
				{
					int mappedRow;

					decimal price = 0;
					int visibleItems = (grdView.ViewRect.Height / grdView.RowHeight);

					Debug.Assert(itemTable != null, "itemTable != null");

					itemTable.Columns[colItemPrice.FieldName].ReadOnly = false;
					itemTable.Columns[colItemPrice.FieldName].MaxLength = 50;

					// Find the prices for the rows that currently show on the screen
					List<ISaleLineItem> items = new List<ISaleLineItem>();

					for (int i = grdView.TopRowIndex; i <= grdView.TopRowIndex + visibleItems && i < itemTable.Rows.Count; i++)
					{
						mappedRow = grdView.GetDataSourceRowIndex(i);

						// Add the items to get prices for to the collection to send into the pricing engine.
						items.Add(new SaleLineItem() { LineId = i, ItemId = itemTable.Rows[mappedRow][colItemId.FieldName].ToString(), SalesOrderUnitOfMeasure = itemTable.Rows[mappedRow]["UNITOFMEASURE"].ToString() });

						if (itemTable.Rows[mappedRow][colItemPrice.FieldName].ToString().Length == 0)
						{
							price = Dialog.InternalApplication.Services.Price.GetItemPrice(itemTable.Rows[mappedRow][colItemId.FieldName].ToString(), itemTable.Rows[mappedRow]["UNITOFMEASURE"].ToString());
						}
					}

					if (items.Count > 0)
					{
						// Obtain prices for the items and repopulate the results grid with those prices.
						var pricedItems = Dialog.InternalApplication.Services.Price.GetItemPrices(items, null).ToDictionary(p => p.LineId);

						for (int i = grdView.TopRowIndex; i <= grdView.TopRowIndex + visibleItems && i < itemTable.Rows.Count; i++)
						{
							mappedRow = grdView.GetDataSourceRowIndex(i);
							itemTable.Rows[mappedRow][colItemPrice.FieldName] = Dialog.InternalApplication.Services.Rounding.RoundForDisplay(pricedItems[i].Price, true, false);
						}
					}

					grItems.DataSource = itemTable;

					itemTable.Columns[colItemPrice.FieldName].ReadOnly = true;
				}
				catch (Exception ex)
				{
					LSRetailPosis.ApplicationExceptionHandler.HandleException("frmItemSearch.FillInPrices", ex);
					throw;
				}
			}
		}

		private void btnGetPrice_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			try
			{
				colItemPrice.Visible = !colItemPrice.Visible;

				int messageId = 61511; // Show price

				if (colItemPrice.Visible)
				{
					FillInPrices();
					messageId = 61512; // Hide price
				}

				btnGetprice.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(messageId);
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException("frmItemSearch.btnGetPrice_Click", ex);
			}
		}

		private void SwitchAttributeSearchControls(bool showAttributeSearchMode)
		{
			tableLayoutPanel4.Visible = !showAttributeSearchMode;
			tblLayoutPanelAttributesSearch.Visible = showAttributeSearchMode;
			btnProductDetails.Visible = showAttributeSearchMode;
		}

		private void btnProductDetails_Click(object sender, EventArgs e)
		{
			string itemNumber = null;

			if (grdView.RowCount > 0)
			{
				System.Data.DataRow Row = grdView.GetDataRow(grdView.GetSelectedRows()[0]);
				itemNumber = (string)Row[colItemId.FieldName];
			}

			if (!string.IsNullOrEmpty(itemNumber))
			{
				InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
						new ProductDetailsConfirmation() { ItemNumber = itemNumber, SourceContext = ProductDetailsSourceContext.ItemSearch }, () => { });
				Dialog.InternalApplication.Services.Interaction.InteractionRequest(request);

				ProductDetailsConfirmation confirmation = request.Context as ProductDetailsConfirmation;
				if ((confirmation != null) && confirmation.Confirmed && confirmation.AddToSale)
				{
					SelectItem(itemNumber);
				}
			}
		}    

		/// <summary>
		/// Checks if the tax code is one of the pre-defined according to PAF-ECF legislation.
		/// </summary>
		/// <param name="taxCode">The tax code.</param>
		/// <returns>True if it is a pre-defined tax code; false, otherwise.</returns>
		private static bool IsPredefinedTaxCode(string taxCode)
		{
			if (taxCode == null)
			{
				throw new ArgumentNullException("taxCode");
			}

			switch (taxCode)
			{
				case "I1":
				case "I2":
				case "F1":
				case "F2":
				case "N1":
				case "N2":
				case "IS1":
				case "IS2":
				case "FS1":
				case "FS2":
				case "NS1":
				case "NS2":
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// Get the tax value formatted accordingly to the Brazilian PAF-ECF requirements.
		/// </summary>
		/// <param name="itemId">The given item ID.</param>
		/// <returns>The formatted tax value to be shown.</returns>
		private static string GetTaxLine(string itemId)
		{
			const string query = @"
							SELECT TT.TAXCODE, TD.TAXVALUE, TT.TAXTYPE_BR
							FROM INVENTTABLEMODULE ITM
							JOIN TAXONITEM AS TOI ON TOI.TAXITEMGROUP = ITM.TAXITEMGROUPID
									AND TOI.DATAAREAID = @DATAAREAID
							JOIN TAXTABLE AS TT ON TT.TAXCODE = TOI.TAXCODE AND (TT.TAXTYPE_BR = @ICMS OR TT.TAXTYPE_BR = @ISS) 
									AND TT.DATAAREAID = @DATAAREAID
							JOIN TAXDATA AS TD ON TD.TAXCODE = TOI.TAXCODE 
									AND TD.DATAAREAID = @DATAAREAID
							WHERE ITM.ITEMID = @ITEMID AND ITM.MODULETYPE = 2 AND ITM.DATAAREAID = @DATAAREAID 
								AND TT.TAXCODE IN
								(
									-- TAX CODE MUST BE ALLOWED IN THE STORE
									SELECT TGD.TAXCODE 
									FROM TAXGROUPDATA AS TGD
									JOIN RETAILSTORETABLE AS RST ON RST.STORENUMBER = @STOREID
									WHERE TGD.TAXGROUP = RST.TAXGROUP AND TGD.DATAAREAID = @DATAAREAID
								)";


			var result = LSRetailPosis.ApplicationLocalizer.Language.Translate(10); //Error 

			using (var taxLines = new DataTable())
			{
				try
				{
					using (var command = new SqlCommand())
					{
						command.Parameters.AddWithValue("@DATAAREAID", ApplicationSettings.Database.DATAAREAID);
						command.Parameters.AddWithValue("@STOREID", ApplicationSettings.Database.StoreID);
						command.Parameters.AddWithValue("@ITEMID", itemId);
						command.Parameters.AddWithValue("@ICMS", BrazilianTaxType.Icms);
						command.Parameters.AddWithValue("@ISS", BrazilianTaxType.Iss);
						command.CommandText = query;
						command.Connection = ApplicationSettings.Database.LocalConnection;

						if (command.Connection.State != ConnectionState.Open)
						{
							command.Connection.Open();
						}

						var reader = command.ExecuteReader();
						taxLines.Load(reader);
					}
				}
				finally
				{
					if (ApplicationSettings.Database.LocalConnection.State == ConnectionState.Open)
					{
						ApplicationSettings.Database.LocalConnection.Close();
					}
				}

				//The item must have only 1 ICMS or ISS tax
				if (taxLines.Rows.Count == 1)
				{
					var taxLine = taxLines.Rows[0];

					var taxCode = taxLine["TAXCODE"].ToString();
					var taxValue = PosApplication.Instance.BusinessLogic.Utility.ToDecimal(taxLine["TAXVALUE"]);
					var taxType = (BrazilianTaxType)taxLine["TAXTYPE_BR"];

					if (IsPredefinedTaxCode(taxCode))
					{
						result = taxCode.Substring(0, taxCode.Length - 1);
					}
					else if (taxType == BrazilianTaxType.Iss)
					{
						result = string.Format("S{0:0000}", taxValue * 100);
					}
					else if (taxType == BrazilianTaxType.Icms)
					{
						result = string.Format("T{0:0000}", taxValue * 100);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Search the items with specific term.
		/// </summary>
		/// <remarks>Caller is responsible for disposing returned object</remarks>
		/// <param name="fromRow"></param>
		/// <param name="searchString"></param>
		/// <returns>Datatable (Caller is responsible for disposing returned object)</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
		[SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "No user input")]
		private DataTable GetItemList(int fromRow, string searchString)
		{
			// Note: Query uses server side sorting using the SQL Server "over" operator to limit the rowset.
			SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
			string query = string.Format("SELECT ITEMID, ITEMNAME, '' AS ITEMPRICE,I.SEARCHNAME, I.UNITOFMEASURE, I.INVENTPRODUCTTYPE_BR   " + "FROM ( " + "    SELECT IT.INVENTPRODUCTTYPE_BR, IT.ITEMID, COALESCE(TR.NAME, IT.ITEMNAME, IT.ITEMID) AS ITEMNAME, IT.DATAAREAID, ISNULL(IM.UNITID, '') AS UNITOFMEASURE, ROW_NUMBER() " + "    OVER (ORDER BY IT.{0} {1}) AS ROW " + "    FROM ASSORTEDINVENTITEMS IT " + "    JOIN INVENTTABLEMODULE IM ON IT.ITEMID = IM.ITEMID AND IM.MODULETYPE = 2 " + "    JOIN ECORESPRODUCT AS PR ON PR.RECID = IT.PRODUCT " + "    LEFT JOIN ECORESPRODUCTTRANSLATION AS TR ON PR.RECID = TR.PRODUCT AND TR.LANGUAGEID = @CULTUREID " + "    WHERE IT.STORERECID = @STORERECID {2}) I " + "WHERE I.DATAAREAID=@DATAAREAID AND I.ROW > @FROMROW AND I.ROW <= @TOROW ", sortBy, sortAsc, searchString); //add by rs 19 Juni 2019
			
			NetTracer.Information("frmItemSearch::GetItemList Started with Search String: {0}, Number Of Items: {1}", searchString, getHowManyRows);

			foreach (DevExpress.XtraGrid.Columns.GridColumn col in grdView.Columns)
			{
				col.SortOrder = DevExpress.Data.ColumnSortOrder.None;
			}

			grdView.Columns[sortBy].SortOrder = sortAsc ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending;

			DataTable itemsTable;

			if (this.AttributeSearchEnabled)
			{
				searchFilterRuntimeData.SearchValue = searchString;
				searchFilterRuntimeData.CategoryId = CategoryIdLong;
				searchFilterRuntimeData.OrderBy = sortBy.ToUpper();
				searchFilterRuntimeData.SortAscending = sortAsc;
				searchFilterRuntimeData.StartRow = fromRow;
				searchFilterRuntimeData.RowsCount = getHowManyRows;

				itemsTable = ProductSearcher.SearchProducts(searchFilterRuntimeData);
			}
			else
			{
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					itemsTable = new DataTable();

					SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@SEARCHVALUE", searchString),
															 new SqlParameter("@ORDERBY", sortBy.ToUpper()),
															 new SqlParameter("@CULTUREID", ApplicationSettings.Terminal.CultureName),
															 new SqlParameter("@STORERECID", ApplicationSettings.Terminal.StorePrimaryId),
															 new SqlParameter("@STOREDATE", DateTime.Today),
															 new SqlParameter("@DATAAREAID", ApplicationSettings.Database.DATAAREAID),
															 new SqlParameter("@ASCENDING", sortAsc),
															 new SqlParameter("@FROMROW", fromRow),
															 new SqlParameter("@PAGESIZE", getHowManyRows)
															};

					using (SqlCommand command = new SqlCommand(String.Format("exec dbo.PRODUCTSEARCH {0}", String.Join(",", parameters.Select(x => x.ParameterName))), connection))
					{
						command.Parameters.AddRange(parameters);

						if (connection.State != ConnectionState.Open)
						{
							connection.Open();
						}

						using (SqlDataReader reader = command.ExecuteReader())
						{
							itemsTable.Load(reader);
						}
					}
				}
				finally
				{
					Cursor.Current = Cursors.Default;

					if (connection.State == ConnectionState.Open)
					{
						connection.Close();
					}
				}
			}

			loadedCount = fromRow + itemsTable.Rows.Count;

			NetTracer.Information("frmItemSearch::GetItemList completed with Search String: {0}, Number Of Items: {1}", searchString, getHowManyRows);

			return itemsTable;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fromRow")]
		private DataTable GetProductDataList(string searchString)
		{
			NetTracer.Information("frmItemSearch::GetProductDataList Started with Search String: {0}, Number Of Items: {1}", searchString, getHowManyRows);
			
			DataTable itemsWithPrices = new DataTable();
			const Int64 startPosition = 1;
			const Int64 pageSize = 100;
			const string orderByField = "Name";
			string sortOrder = "Ascending";
			bool includeTotal = false;
			int otherChannelRecId = 0;
			int catalogRecId=0;
			string attributeRecIdRangeValue = string.Empty;

			sortOrder = sortAsc ? DevExpress.Data.ColumnSortOrder.Ascending.ToString() : DevExpress.Data.ColumnSortOrder.Descending.ToString(); 

			Dialog.InternalApplication.Services.Item.GetProductsByKeyword(
				ApplicationSettings.Terminal.StorePrimaryId,
				searchString,
				startPosition,
				pageSize,
				orderByField,
				sortOrder,
				includeTotal,                  // _includeTotal
				Thread.CurrentThread.CurrentUICulture.Name,     // _languageId
				otherChannelRecId,                      // _otherChannelRecId
				catalogRecId,                      // _catalogRecId
				attributeRecIdRangeValue,          // _attributeRecIdRangeValue
				ref itemsWithPrices);

			NetTracer.Information("frmItemSearch::GetProductDataList completed with Search String: {0}, Number Of Items: {1}", searchString, getHowManyRows);
			return itemsWithPrices;
		}

		// Saves the product data in POS tables for the item selected
		private void CreateProductData(string selectedItem)
		{
			NetTracer.Information("frmItemSearch::CreateProductData Started with item selected: {0}", selectedItem);
			bool createdLocal = false;
			XDocument tmpstring;

			try
			{

				//Retrieve the Product
				tmpstring = Dialog.InternalApplication.Services.Item.GetProductData(selectedItem);

				DM.ItemDataManager itemdataManager = new DM.ItemDataManager(
					PosApplication.Instance.Settings.Database.Connection,
					PosApplication.Instance.Settings.Database.DataAreaID);

				ItemData itemData = new ItemData(
				ApplicationSettings.Database.LocalConnection,
				ApplicationSettings.Database.DATAAREAID,
				ApplicationSettings.Terminal.StorePrimaryId);

				createdLocal = itemData.SaveProductData(tmpstring);

				if (!createdLocal)
				{
					Dialog.InternalApplication.Services.Dialog.ShowMessage(99890, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
				Dialog.InternalApplication.Services.Dialog.ShowMessage(99890, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			NetTracer.Information("frmItemSearch::CreateProductData completed with Search String: {0}, Number Of Items: {1}", selectedItem, getHowManyRows);
		}

		private void keyboard1_DownButtonPressed()
		{
			btnDown_Click_1(this, new EventArgs());
		}

		private void keyboard1_UpButtonPressed()
		{
			btnUp_Click_1(this, new EventArgs());
		}

		private void keyboard1_PgDownButtonPressed()
		{
			btnDown_Click(this, new EventArgs());
		}

		private void keyboard1_PgUpButtonPressed()
		{
			btnUp_Click(this, new EventArgs());
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private void grdView_EndSorting(object sender, EventArgs e)
		{
			if (colItemPrice.Visible)
			{
				FillInPrices();
			}
			else
			{
				grItems.DataSource = itemTable;
			}
		}

		private void keyboard1_EnterButtonPressedWithoutInputChange()
		{
			SelectItem();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			// Done because of a suspicion that the database was blocking the data director when the form was open
			if (LSRetailPosis.Settings.ApplicationSettings.Terminal.AutoLogOffTimeOutInMin > 0)
			{
				TimeSpan timeSpan = DateTime.Now.Subtract(lastActiveDateTime);

				// Must ensure that we calculate the half-timeout as a double to avoid truncation issues
				if (timeSpan.TotalMinutes >= (double)(LSRetailPosis.Settings.ApplicationSettings.Terminal.AutoLogOffTimeOutInMin / 2.0))
				{
					clockTimer.Enabled = false;
					Close();
				}
			}
		}

		private void grItems_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private void grdView_TopRowChanged(object sender, EventArgs e)
		{
			FillInPrices();
		}

		private void grItems_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				btnSelect_Click(null, null);
			}
		}

		private void btnSearchAttributes_Click(object sender, EventArgs e)
		{
			InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
				new ProductSearchAttributesFilterConfirmation()
				{
					CategoryId = this.CategoryIdLong,
					SearchFilter = searchFilterRuntimeData,
					SearchFilterCache = searchFilterRuntimeCache
				}, () => { });
			Dialog.InternalApplication.Services.Interaction.InteractionRequest(request);

			ProductSearchAttributesFilterConfirmation confirmation = request.Context as ProductSearchAttributesFilterConfirmation;
			if ((confirmation != null) && confirmation.Confirmed)
			{
				searchFilterRuntimeData = (IProductSearchFilter)confirmation.SearchFilter;
				searchFilterRuntimeCache = (IProductSearchFilter)confirmation.SearchFilterCache;
				this.txtAttributes.Text = searchFilterRuntimeData.ToString();
				this.txtAttributes.Refresh();

				SearchItems();
			}
		}

		private void btnClearAttributes_Click(object sender, EventArgs e)
		{
			InitProductSearchFilter();
			this.txtAttributes.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(107108); // None
			this.txtAttributes.Refresh();

			SearchAfterClear();
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			inputHasChanged = true;
			if (!string.IsNullOrEmpty(txtKeyboardInput.Text))
			{
				EnterPress();
			}
			else
			{
				btnClear_Click(sender, e);
			}

			CheckRowPosition();
		}

		private void btnClear_Click_1(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnClear_Click(sender, e);
		}

		private void btnPgUp_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnUp_Click(sender, e);
		}

		private void btnUp_Click_2(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnUp_Click_1(sender, e);
		}

		private void btnGetprice_Click_1(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnGetPrice_Click(sender, e);
		}

		private void btnSelect_Click_1(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnSelect_Click(sender, e);
		}

		private void btnDown_Click_2(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnDown_Click_1(sender, e);
		}

		private void btnPgDown_Click(object sender, EventArgs e)
		{
			lastActiveDateTime = DateTime.Now;
			btnDown_Click(sender, e);
		}

		private void CheckRowPosition()
		{
			btnPgDown.Enabled = !grdView.IsLastRow;
			btnDown.Enabled = btnPgDown.Enabled;
			btnPgUp.Enabled = !grdView.IsFirstRow;
			btnUp.Enabled = btnPgUp.Enabled;
		}

		private void grdView_Click(object sender, EventArgs e)
		{
			Point p = grdView.GridControl.PointToClient(MousePosition);
			DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = grdView.CalcHitInfo(p);

			if (info.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.Column)
			{
				if (info.Column.FieldName != colItemPrice.FieldName)
				{
					if (Functions.CountryRegion == SupportedCountryRegion.BR
						&& (info.Column.FieldName == colItemOwnThirdProduc.FieldName
						|| info.Column.FieldName == colItemRoundTrunc.FieldName
						|| info.Column.FieldName == colItemUnitOfMeasure.FieldName
						|| info.Column.FieldName == colItemTaxRate.FieldName))
						return;

					if (sortBy == info.Column.FieldName)
					{
						sortAsc = !sortAsc;
					}
					else
					{
						sortAsc = true;
					}

					sortBy = info.Column.FieldName;

					loadedCount = 0;

					DataTable newItemTable = GetItemList(0, txtKeyboardInput.Text);

					if (itemTable != null)
					{
						itemTable.Dispose();
					}

					grItems.DataSource = itemTable = newItemTable;

					FillInPrices();
				}
			}

			CheckRowPosition();
			txtKeyboardInput.Select();
		}

		private void OnGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
		{
			// Enable OK button when we have results
			btnSelect.Enabled = e.FocusedRowHandle >= 0;
			// Enable ProductDetails button when we have results
			btnProductDetails.Enabled = e.FocusedRowHandle >= 0;
		}

		private void txtKeyboardInput_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void radioCurrentStore_CheckedChanged(object sender, EventArgs e)
		{
			txtKeyboardInput.Select();
		}

		private void radioAllProducts_CheckedChanged(object sender, EventArgs e)
		{
			txtKeyboardInput.Select();
		}
	}
}
