/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using LSRetailPosis;
    using LSRetailPosis.DataAccess;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.POSProcesses.Common;
    using LSRetailPosis.Settings;
    using LSRetailPosis.Settings.FunctionalityProfiles;
    using LSRetailPosis.Settings.VisualProfiles;
    using Microsoft.Dynamics.Retail.Diagnostics;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
    using Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;
    using EF = Microsoft.Dynamics.Retail.Pos.DataEntity;

    [Export("LogOnForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class LogOnForm : frmTouchBase, IInteractionView
    {

        #region Types

        internal enum LogonModes
        {
            Numpad = 0,
            UserList = 1
        }

        #endregion

        #region Class Variables
        private string operatorId;          //selected user id
        private string password;            //selected password
        private string operatorName;        //operator's name
        private string operatorNameOnReceipt;//operator's name
        private LogOnStatus status;
        private DialogResult dlgResult = DialogResult.Ignore;     //Determines if the form can be closed or not 
        
        #endregion

        #region Components

        private IContainer components = null;
        private LogonModes logonMode;
        private Label lblUser;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numPad;
        private Microsoft.Dynamics.Retail.Pos.Interaction.Controls.SplitButton exitSplitButton;
        private TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraGrid.GridControl grdUsers;
        private DevExpress.XtraGrid.Views.Grid.GridView grvUserData;
        private DevExpress.XtraGrid.Columns.GridColumn colOperatorID;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem trainingToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem restartToolStripMenuItem;
        private ToolStripMenuItem shutdownToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem productInformationToolStripMenuItem;
        private Label labelTime;
        private Label lblTraining;
        private TableLayoutPanel headerTableLayoutPanel;
        private TaggedImageBoxControl TaggedImage;
        private Timer timer;

        #endregion

        #region Class Properties
        public string OperatorId
        {
            get { return operatorId; }
        }

        public string Password
        {
            get { return password; }
        }

        public string OperatorName
        {
            get { return operatorName; }
        }

        public string OperatorNameOnReceipt
        {
            get { return operatorNameOnReceipt; }
        }

        public LogOnStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        #endregion
        
        #region Constructor and Destructor

        public LogOnForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
				this.HideHelpIcon();

                this.Bounds = new Rectangle(
                    ApplicationSettings.MainWindowLeft,
                    ApplicationSettings.MainWindowTop,
                    ApplicationSettings.MainWindowWidth,
                    ApplicationSettings.MainWindowHeight);

                this.Text = ApplicationSettings.ApplicationTitle;

                LoadBackgroundImage();

                ActivateTraining(ApplicationSettings.Terminal.TrainingMode);

                status = LogOnStatus.None;

                if (Functions.ShowStaffListAtLogon)
                    logonMode = LogonModes.UserList;
                else
                    logonMode = LogonModes.Numpad;

                if (logonMode == LogonModes.UserList)
                {
                    GetUserInformation();
                    grdUsers.Visible = true;
                    tableLayoutPanel2.SetColumnSpan(grdUsers, 3);
                    lblUser.Visible = false;
                }
                else
                {
                    grdUsers.Visible = false;
                    lblUser.Visible = true;

                    this.numPad.SuspendLayout();
                    numPad.Visible = true;
                    numPad.EntryType = NumpadEntryTypes.Barcode;
                    this.numPad.ResumeLayout();

                    PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived += new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
                    PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginVerifyCapture();                   
                }

                // Set visibility of training mode
                trainingToolStripMenuItem.Visible = toolStripSeparator4.Visible = !ApplicationSettings.Terminal.HideTrainingMode;

                TranslateLabels();
                ClearVariables();

                UpdateTime();
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for LogOnForm are reserved at 20 - 49
            // In use now are ID's 20 - 48
            //

            trainingToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(20);  // Activate Training
            exitToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(22);  // Exit            
            restartToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(25); // Reboot
            shutdownToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(26); // Shutdown       
            productInformationToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(48); // Product information

            colName.Caption = ApplicationLocalizer.Language.Translate(27); //Name
            colOperatorID.Caption = ApplicationLocalizer.Language.Translate(28); //Operator Id

            lblTraining.Text = ApplicationLocalizer.Language.Translate(29); //Training           
            lblUser.Text = string.Format(ApplicationLocalizer.Language.Translate(44), string.Empty); // User Name  
            numPad.PromptText = ApplicationLocalizer.Language.Translate(28); //Operator Id 
        }

        private void LoadBackgroundImage()
        {
            try
            {
                if (PosApplication.Instance.Services.ApplicationService.LoginWindowImage != null)
                {
                    this.BackgroundImage = PosApplication.Instance.Services.ApplicationService.LoginWindowImage;                    
                }
                else
                {
                    int imageId = VisualProfile.ImageId;

                    // Only attempy load if we have an image ID
                    if (imageId > 0)
                    {
                        LayoutData layoutData = new LayoutData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                        using (DataTable pictureTable = layoutData.GetPicture(imageId))
                        {
                            byte[] pictureData = (byte[])pictureTable.Rows[0]["PICTURE"];

                            if (pictureData.Length > 0)
                            {
                                this.BackgroundImage = LSRetailPosis.ButtonGrid.GUIHelper.GetBitmap(pictureData);
                            }
                        }
                    }
                }

                // Set background image layout
                if (this.BackgroundImage != null)
                {
                    // Set to Center. Note if this is set to Tile (default) colors are rendered correctly e.g. White in images is rendered light gray
                    this.BackgroundImageLayout = ImageLayout.Center;
                }
            }
            catch (Exception ex)
            {
                NetTracer.Warning(ex, "LogOnForm.LoadBackgroundImage() exception loading background image");
            }
        }

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
            this.exitSplitButton = new Microsoft.Dynamics.Retail.Pos.Interaction.Controls.SplitButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trainingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.productInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numPad = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.lblUser = new System.Windows.Forms.Label();
            this.grdUsers = new DevExpress.XtraGrid.GridControl();
            this.grvUserData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colOperatorID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTraining = new System.Windows.Forms.Label();
            this.headerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelTime = new System.Windows.Forms.Label();
            this.TaggedImage = new Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.TaggedImageBoxControl();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvUserData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.headerTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // exitSplitButton
            // 
            this.exitSplitButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.exitSplitButton.AutoSize = true;
            this.exitSplitButton.ContextMenuStrip = this.contextMenuStrip;
            this.exitSplitButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.exitSplitButton.Location = new System.Drawing.Point(803, 689);
            this.exitSplitButton.Name = "exitSplitButton";
            this.exitSplitButton.Size = new System.Drawing.Size(127, 57);
            this.exitSplitButton.TabIndex = 5;
            this.exitSplitButton.Text = "Exit";
            this.exitSplitButton.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainingToolStripMenuItem,
            this.toolStripSeparator4,
            this.productInformationToolStripMenuItem,
            this.toolStripSeparator3,
            this.restartToolStripMenuItem,
            this.shutdownToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(255, 172);
            // 
            // trainingToolStripMenuItem
            // 
            this.trainingToolStripMenuItem.Name = "trainingToolStripMenuItem";
            this.trainingToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.trainingToolStripMenuItem.Text = "Activate training";
            this.trainingToolStripMenuItem.Visible = false;
            this.trainingToolStripMenuItem.Click += new System.EventHandler(this.trainingToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(251, 6);
            this.toolStripSeparator4.Visible = false;
            // 
            // productInformationToolStripMenuItem
            // 
            this.productInformationToolStripMenuItem.Name = "productInformationToolStripMenuItem";
            this.productInformationToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.productInformationToolStripMenuItem.Text = "Product Information";
            this.productInformationToolStripMenuItem.Click += new System.EventHandler(this.productInformationToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(251, 6);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.shutdownToolStripMenuItem.Text = "Shut down";
            this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.shutdownToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(251, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // numPad
            // 
            this.numPad.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.numPad.Appearance.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPad.Appearance.Options.UseFont = true;
            this.numPad.AutoSize = true;
            this.numPad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.numPad.CurrencyCode = null;
            this.numPad.EnteredQuantity = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPad.EnteredValue = "";
            this.numPad.Location = new System.Drawing.Point(317, 135);
            this.numPad.MaskChar = null;
            this.numPad.MaskInterval = 0;
            this.numPad.MaxNumberOfDigits = 13;
            this.numPad.MinimumSize = new System.Drawing.Size(390, 432);
            this.numPad.Name = "numPad";
            this.numPad.NegativeMode = false;
            this.numPad.NoOfTries = 0;
            this.numPad.NumberOfDecimals = 2;
            this.numPad.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.numPad.PromptText = null;
            this.numPad.ShortcutKeysActive = false;
            this.numPad.Size = new System.Drawing.Size(390, 432);
            this.numPad.TabIndex = 3;
            this.numPad.TimerEnabled = false;
            this.numPad.Visible = false;
            this.numPad.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad_EnterButtonPressed);
            this.numPad.CardSwept += new LSRetailPosis.POSProcesses.WinControls.NumPad.cardSwipedDelegate(this.numPad_CardSwept);
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(317, 100);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(138, 32);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "User Name:";
            // 
            // grdUsers
            // 
            this.grdUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdUsers.Location = new System.Drawing.Point(3, 135);
            this.grdUsers.LookAndFeel.SkinName = "Money Twins";
            this.grdUsers.MainView = this.grvUserData;
            this.grdUsers.MinimumSize = new System.Drawing.Size(500, 0);
            this.grdUsers.Name = "grdUsers";
            this.grdUsers.Size = new System.Drawing.Size(500, 432);
            this.grdUsers.TabIndex = 2;
            this.grdUsers.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvUserData,
            this.gridView1});
            this.grdUsers.Visible = false;
            this.grdUsers.Click += new System.EventHandler(this.grdUsers_Click);
            // 
            // grvUserData
            // 
            this.grvUserData.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grvUserData.Appearance.HeaderPanel.Options.UseFont = true;
            this.grvUserData.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grvUserData.Appearance.Row.Options.UseFont = true;
            this.grvUserData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOperatorID,
            this.colName});
            this.grvUserData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grvUserData.GridControl = this.grdUsers;
            this.grvUserData.Name = "grvUserData";
            this.grvUserData.OptionsBehavior.Editable = false;
            this.grvUserData.OptionsCustomization.AllowFilter = false;
            this.grvUserData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grvUserData.OptionsSelection.EnableAppearanceHideSelection = false;
            this.grvUserData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.grvUserData.OptionsView.ShowGroupPanel = false;
            this.grvUserData.OptionsView.ShowIndicator = false;
            this.grvUserData.PaintStyleName = "Skin";
            this.grvUserData.RowHeight = 31;
            this.grvUserData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grvUserData_KeyDown);
            // 
            // colOperatorID
            // 
            this.colOperatorID.Caption = "OperatorID";
            this.colOperatorID.FieldName = "STAFFID";
            this.colOperatorID.Name = "colOperatorID";
            this.colOperatorID.OptionsColumn.AllowEdit = false;
            this.colOperatorID.Visible = true;
            this.colOperatorID.VisibleIndex = 0;
            this.colOperatorID.Width = 125;
            // 
            // colName
            // 
            this.colName.Caption = "Name";
            this.colName.FieldName = "NAME";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            this.colName.Width = 371;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grdUsers;
            this.gridView1.Name = "gridView1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.numPad, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.exitSplitButton, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblUser, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.grdUsers, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblTraining, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.headerTableLayoutPanel, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.21476F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblTraining
            // 
            this.lblTraining.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTraining.AutoSize = true;
            this.lblTraining.Font = new System.Drawing.Font("Segoe UI Semibold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraining.Location = new System.Drawing.Point(415, 593);
            this.lblTraining.Name = "lblTraining";
            this.lblTraining.Size = new System.Drawing.Size(194, 50);
            this.lblTraining.TabIndex = 2;
            this.lblTraining.Text = "TRAINING";
            this.lblTraining.Visible = false;
            this.lblTraining.Click += new System.EventHandler(this.trainingToolStripMenuItem_Click);
            // 
            // headerTableLayoutPanel
            // 
            this.headerTableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel2.SetColumnSpan(this.headerTableLayoutPanel, 3);
            this.headerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.8446F));
            this.headerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.15541F));
            this.headerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 425F));
            this.headerTableLayoutPanel.Controls.Add(this.labelTime, 2, 0);
            this.headerTableLayoutPanel.Controls.Add(this.TaggedImage, 1, 0);
            this.headerTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.headerTableLayoutPanel.Name = "headerTableLayoutPanel";
            this.headerTableLayoutPanel.RowCount = 1;
            this.headerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.headerTableLayoutPanel.Size = new System.Drawing.Size(1018, 94);
            this.headerTableLayoutPanel.TabIndex = 6;
            // 
            // labelTime
            // 
            this.labelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTime.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.Location = new System.Drawing.Point(623, 10);
            this.labelTime.Margin = new System.Windows.Forms.Padding(10);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(385, 45);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "12:00";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelTime.Click += new System.EventHandler(this.labelTime_Click);
            // 
            // TaggedImage
            // 
            this.TaggedImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TaggedImage.Location = new System.Drawing.Point(460, 3);
            this.TaggedImage.Name = "TaggedImage";
            this.TaggedImage.Size = new System.Drawing.Size(120, 88);
            this.TaggedImage.TabIndex = 1;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // LogOnForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel2);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "LogOnForm";
            this.Text = "Log on";
            this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvUserData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.headerTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Events

        private void OnLogOnDevice_DataReceived(IExtendedLogOnInfo extendedLogOnInfo)
        {
            if (!string.IsNullOrWhiteSpace(extendedLogOnInfo.LogOnKey))
            {
                ProcessExtendedLogOnKey(extendedLogOnInfo);
            }
            // Status update - only invoked when a capture device is plugged in with FaceBiometricPlugin
            else if (extendedLogOnInfo.DataStream != null)
            {
                TaggedImage.Initialize(extendedLogOnInfo.DataStream);
            }
        }

        private void numPad_CardSwept(ICardInfo cardInfo)
        {
            if (!Functions.StaffCardLogOn)
                return;

            IExtendedLogOnInfo extendedLogOnInfo = new EF.ExtendedLogOnInfo()
            {
                LogOnKey = cardInfo.CardNumber,
                LogOnType = ExtendedLogOnType.MagneticStripeReader,
                PasswordRequired = Functions.StaffCardLogOnRequiresPassword
            };

            ProcessExtendedLogOnKey(extendedLogOnInfo);
        }

        /// <summary>
        /// Processes the extended log on key.
        /// </summary>
        /// <param name="extendedLogOnInfo">The extended log on info.</param>
        private void ProcessExtendedLogOnKey(IExtendedLogOnInfo extendedLogOnInfo)
        {
            try
            {
                if (status == LogOnStatus.None)
                {
                    // Check if the operator ID was assigned earlier via entering on keypad or via extended logon. Extended logon 
                    // should not clear out whatever has been entered on the keypad
                    if (string.IsNullOrEmpty(operatorId))
                    {
                        this.operatorId = PosApplication.Instance.Services.Peripherals.LogOnDevice.Identify(extendedLogOnInfo);

                        HandleStaffId(extendedLogOnInfo.PasswordRequired);
                    }
                }
            }
            catch (Exception ex)
            {
                NetTracer.Error(ex, "Unabled to process extended log on type: {0}", extendedLogOnInfo.LogOnType);
            }
        }

        private void LogonSuccessful()
        {
            try
            {
                //Set the status for the Main form to know what's going on
                status = LogOnStatus.LogOn;

                PosApplication.Instance.BusinessLogic.LogonLogoffSystem.ConcludeSuccessfulLogOn();

                lblUser.Text = string.Empty;
                operatorId = string.Empty;
                password = string.Empty;
                operatorName = string.Empty;
                operatorNameOnReceipt = string.Empty;

                dlgResult = DialogResult.OK;

                this.Close();

            }
            catch (Exception ex)
            {
                
                using (frmError errorForm = new LSRetailPosis.POSProcesses.frmError(ex.Message, ex))
                {
					POSFormsManager.ShowPOSForm(errorForm);
				}
            }
        }

        private void numPad_EnterButtonPressed()
        {
            HandleStaffId(true);
        }

        private void HandleStaffId(bool usePassword)
        {
            try
            {
                // remove these events so that the capture device is shut down when the logon is processed
                PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived -= OnLogOnDevice_DataReceived;
                PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();

                if ((!string.IsNullOrWhiteSpace(numPad.EnteredValue)) || (!string.IsNullOrWhiteSpace(this.operatorId)))
                {
                    string nextPromptText = numPad.PromptText;
                    NumpadEntryTypes currentEntryType = numPad.EntryType;

                    LSRetailPosis.POSProcesses.Common.Login loginProcess = new LSRetailPosis.POSProcesses.Common.Login();
                    bool okToContinue = loginProcess.LoginWindowEnterPressed(
                        ref currentEntryType, numPad.EnteredValue, ref operatorId, ref operatorName,
                        ref operatorNameOnReceipt, ref password, ref nextPromptText, ref usePassword);

                    lblUser.Text = string.IsNullOrEmpty(operatorName) ? string.Empty : string.Format(ApplicationLocalizer.Language.Translate(44), operatorName);
                    numPad.EntryType = currentEntryType;
                    numPad.PromptText = nextPromptText;

                    if (okToContinue)
                    {
                        if (!Login.LogOnUser(usePassword, operatorId, operatorName, operatorNameOnReceipt, password, numPad.EnteredValue))
                        {
                            ClearVariables();
                        }
                        else
                        {
                            LogonSuccessful();
                        }
                    }
                }
                else
                {
                    POSFormsManager.ShowPOSMessageDialog(3214);
                }
            }
            catch (Exception ex)
            {
                ApplicationExceptionHandler.HandleException(this.ToString(), ex);

                operatorName = string.Empty;
                lblUser.Text = string.Empty;

                if (ex.GetFirstInnerException<SqlException>(e => SqlErrorCodes.GetAllEventIdsByType(SqlErrorType.ConnectionError).Contains(e.Number)) != null)
                {
                    throw new LogOnException("LogOnException", ex);
                }
            }
            finally
            {
                // re-enable capture device. If the logon failed, capture device is again used, else closed during form close
                PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived += OnLogOnDevice_DataReceived;
                PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginVerifyCapture();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            //Default value for dlgResult is Ignore - before any Close() is called the
            //value is changed to OK.
            if (dlgResult == DialogResult.Ignore)
            {
                // Execute exit logic then check dlgResult again
                this.exitSplitButton.PerformClick();
                if (dlgResult != System.Windows.Forms.DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }

            base.OnClosing(e);
        }

        private void LogonGridUser()
        {
            try
            {
                //if nothing is selected display an error message
                if (grvUserData.SelectedRowsCount == 0)
                {
                    PosApplication.Instance.Services.Dialog.ShowMessage(31, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //Get the selected user id
                DataRow row = grvUserData.GetDataRow(grvUserData.GetSelectedRows()[0]);
                operatorId = row["STAFFID"].ToString();
                operatorName = row["NAME"].ToString();

                operatorNameOnReceipt = row["NAMEONRECEIPT"].ToString();

                LogonData logonData = new LogonData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
                EF.PosPermission posPermission = logonData.GetUserPosPermission(ApplicationSettings.Terminal.StoreId, operatorId);

                if (!string.IsNullOrWhiteSpace(operatorId))
                {
                    //This is to assign the operator culture ONCE such that it is available throughout all translation attempts
                    LSRetailPosis.DataAccess.Language lang = new Language(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

                    // Set operator culture if found, otherwise default to terminal culture
                    string opCultNam = lang.GetOperatorCultureName(ApplicationSettings.Terminal.StoreId, operatorId);
                    if (!string.IsNullOrEmpty(opCultNam))
                    {
                        ApplicationSettings.Terminal.OperatorCultureName = opCultNam;
                    }
                }


                if (posPermission == null)
                {
                    // No permisison or position
                    using (frmMessage message = new frmMessage(1004))
                    {
						POSFormsManager.ShowPOSForm(message);
					}

                    return;
                }

                //display the numpad to get the password
                using (LSRetailPosis.POSProcesses.frmInputNumpad frmNumPad = new LSRetailPosis.POSProcesses.frmInputNumpad())
                {
                    frmNumPad.EntryTypes = NumpadEntryTypes.Password;
                    frmNumPad.PromptText = ApplicationLocalizer.Language.Translate(2379); //"Password";

                    bool logonWorked = false;

                    while (logonWorked == false)
                    {
                        //if button OK / Enter was hit
						POSFormsManager.ShowPOSForm(frmNumPad);
                        if (frmNumPad.DialogResult == DialogResult.OK)
                        {
                            password = frmNumPad.InputText;

                            if (logonWorked = Login.LogOnUser(true, operatorId, operatorName, operatorNameOnReceipt, password))
                            {
                                //Set privilege 
                                ApplicationSettings.Terminal.TerminalOperator.MaxLineReturnAmount = PosApplication.Instance.BusinessLogic.Utility.ToDecimal(posPermission.MaxLineReturnAmount);
                                ApplicationSettings.Terminal.TerminalOperator.MaxTotalReturnAmount = PosApplication.Instance.BusinessLogic.Utility.ToDecimal(posPermission.MaxTotalReturnAmount);
                                ApplicationSettings.Terminal.TerminalOperator.UserIsManager = PosApplication.Instance.BusinessLogic.Utility.ToBool(posPermission.ManagerPrivileges);
                                ApplicationSettings.Terminal.TerminalOperator.UserIsInventoryUser = PosApplication.Instance.BusinessLogic.Utility.ToBool(posPermission.UseHandheld);

                                ApplicationSettings.Terminal.TerminalOperator.AllowReturnOfNonReturnableItem_RU = PosApplication.Instance.BusinessLogic.Utility.ToBool(posPermission.AllowReturnOfNonReturnableItem_RU);
                                ApplicationSettings.Terminal.TerminalOperator.AllowNotSameDateReturn_RU = (EmployeeNotSameDateReturn)posPermission.AllowNotSameDateReturn_RU;
                                ApplicationSettings.Terminal.TerminalOperator.AllowDifferentPaymentMethodRefunds_RU = PosApplication.Instance.BusinessLogic.Utility.ToBool(posPermission.AllowDifferentPaymentMethodRefunds_RU);
                                ApplicationSettings.Terminal.TerminalOperator.AllowSetupLabelPrinter = PosApplication.Instance.BusinessLogic.Utility.ToBool(posPermission.AllowSetupLabelPrinter);

                                LogonSuccessful();
                            }
                        }
                        else
                        {
                            logonWorked = true;
                            status = LogOnStatus.None;
                        }

                        frmNumPad.TryAgain();
                    }
                }

                if (status == LogOnStatus.LogOn)
                {
                    dlgResult = DialogResult.OK;
                    Close();
                }

            }
            catch (Exception x)
            {
                // The user hasn't entered all the information needed.
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), x);
                throw new PosisException(34, x);
            }

        }

        private void GetUserInformation()
        {
            //Get a list of all the users          
            Employee employees = new Employee(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            DataTable userData = employees.GetEmployees(ApplicationSettings.Terminal.StoreId);

            grdUsers.DataSource = userData;
        }

        private void grdUsers_Click(object sender, EventArgs e)
        {
            LogonGridUser();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logonMode == LogonModes.Numpad)
            {
                try
                {
                    status = LogOnStatus.Exit;
                    PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived -= OnLogOnDevice_DataReceived;
                    PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();

                    if (UserHasAccessRights(PosisOperations.ApplicationExit))
                    {
                        //status = LogonStatus.Exit;
                        dlgResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived += OnLogOnDevice_DataReceived;
                        PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginVerifyCapture();
                        status = LogOnStatus.None;
                    }
                }
                catch (Exception)
                {
                    status = LogOnStatus.None;
                }
            }
            else
            {
                if (UserHasAccessRights(PosisOperations.ApplicationExit))
                {
                    status = LogOnStatus.Exit;
                    dlgResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    status = LogOnStatus.None;
                }
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            status = LogOnStatus.Restart;
            if (logonMode == LogonModes.Numpad)
            {
                try
                {
                    if (UserHasAccessRights(PosisOperations.RestartComputer))
                    {
                        //status = LogonStatus.Restart;
                        dlgResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        status = LogOnStatus.None;
                    }
                }
                catch (Exception)
                {
                    status = LogOnStatus.None;
                }
            }
            else
            {
                if (UserHasAccessRights(PosisOperations.RestartComputer))
                {
                    status = LogOnStatus.Restart;
                    dlgResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    status = LogOnStatus.None;
                }
            }

        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            status = LogOnStatus.Shutdown;
            if (logonMode == LogonModes.Numpad)
            {
                try
                {
                    if (UserHasAccessRights(PosisOperations.ShutDownComputer))
                    {
                        //status = LogonStatus.Shutdown;
                        dlgResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        status = LogOnStatus.None;
                    }
                }
                catch (Exception)
                {
                    status = LogOnStatus.None;
                }
            }
            else
            {
                if (UserHasAccessRights(PosisOperations.ShutDownComputer))
                {
                    status = LogOnStatus.Shutdown;
                    dlgResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    status = LogOnStatus.None;
                }
            }
        }

        private void productInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductInformationConfirmation productInformationConfirmation = new ProductInformationConfirmation();
            Microsoft.Practices.Prism.Interactivity.InteractionRequest.InteractionRequestedEventArgs request = new Practices.Prism.Interactivity.InteractionRequest.InteractionRequestedEventArgs(productInformationConfirmation, () => { });

            PosApplication.Instance.Services.Interaction.InteractionRequest(request);
        }

        private bool UserHasAccessRights(PosisOperations OperationID)
        {
            bool userHasAccess = false;

            if (logonMode == LogonModes.UserList)
            {
                operatorId = grvUserData.GetDataRow(grvUserData.GetSelectedRows()[0])["STAFFID"].ToString();

                if (!PosApplication.Instance.BusinessLogic.UserAccessSystem.UserHasAccess(operatorId, OperationID))
                {
                    using (frmMessage dialog = new frmMessage(1322)) // Unauthorized
                    {
						POSFormsManager.ShowPOSForm(dialog);
                    }
                }
                else
                {
                    using (frmInputNumpad frmNumPad = new frmInputNumpad())
                    {
                        frmNumPad.EntryTypes = NumpadEntryTypes.Password;
                        frmNumPad.PromptText = ApplicationLocalizer.Language.Translate(2379); //"Password";

                        do
                        {
							POSFormsManager.ShowPOSForm(frmNumPad);
                            if (frmNumPad.DialogResult == DialogResult.OK)
                            {
                                LogonData logonData = new LogonData(PosApplication.Instance.Settings.Database.Connection, PosApplication.Instance.Settings.Database.DataAreaID);
                                password = frmNumPad.InputText;
                                userHasAccess = logonData.ValidatePasswordHash(ApplicationSettings.Terminal.StoreId, operatorId, LogonData.ComputePasswordHash(operatorId, password, ApplicationSettings.Terminal.StaffPasswordHashName));

                                if (!userHasAccess)
                                {
                                    using (frmMessage errorMessage = new frmMessage(1325))
                                    {
										POSFormsManager.ShowPOSForm(errorMessage); // Invalid password
                                    }

                                    frmNumPad.TryAgain();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (!userHasAccess);
                    }
                }
            }
            else
            {
                using (ManagerAccessForm frmManager = new ManagerAccessForm(OperationID))
                {
					POSFormsManager.ShowPOSForm(frmManager);
                    userHasAccess = DialogResult.OK == frmManager.DialogResult;
                }
            }

            return userHasAccess;
        }

        private void trainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle training mode
            ActivateTraining(!ApplicationSettings.Terminal.TrainingMode);
        }

        private void ActivateTraining(bool enable)
        {
            ApplicationSettings.Terminal.TrainingMode = enable;

            // Update menu text
            if (enable)
            {
                trainingToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(21); // "Deactivate training"
            }
            else
            {
                trainingToolStripMenuItem.Text = ApplicationLocalizer.Language.Translate(20); // "Activate training"
            }

            this.lblTraining.Visible = enable;
        }

        private void grvUserData_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:                
                    LogonGridUser();
                    break;
                default:
                    break;
            }
        }

        private void ClearVariables()
        {
            if (logonMode == LogonModes.Numpad)
            {
                lblUser.Text = string.Empty;
                operatorId = string.Empty;
                password = string.Empty;
                operatorName = string.Empty;
                numPad.EntryType = NumpadEntryTypes.Barcode;
                numPad.PromptText       = ApplicationLocalizer.Language.Translate(28); //Operator Id
                exitSplitButton.Text    = ApplicationLocalizer.Language.Translate(36); //Exit
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            if (logonMode == LogonModes.UserList)
            {
                grdUsers.Select();
            }
            else
            {
                numPad.SetEnteredValueFocus();
            }

            base.OnActivated(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            // If tagged image control is active, shut it down at this point
            if (TaggedImage.IsActive)
            {
                TaggedImage.EndImageRefresh();
            }

            PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();
            PosApplication.Instance.Services.Peripherals.LogOnDevice.DataReceived -= new LogOnDeviceEventHandler(OnLogOnDevice_DataReceived);
            
            base.OnClosed(e);
        }

        #endregion

        #region Clock Functionality

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            this.labelTime.Text = ShowFullDateTimePattern
                ? DateTime.Now.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.FullDateTimePattern)
                : DateTime.Now.ToLongTimeString();
        }

        private void labelTime_Click(object sender, EventArgs e)
        {
            ShowFullDateTimePattern = !ShowFullDateTimePattern;
        }

        private bool ShowFullDateTimePattern { get; set; }

        #endregion

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
            return new LogOnConfirmation
            {
                Confirmed = true,
                LogOnStatus = (int)this.Status
            } as TResults;
        }

        #endregion

    }
}
