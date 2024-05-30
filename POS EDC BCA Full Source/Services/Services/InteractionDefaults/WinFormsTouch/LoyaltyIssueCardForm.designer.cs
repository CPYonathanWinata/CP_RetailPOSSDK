/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    using System.Windows.Forms;
    using DevExpress.XtraEditors;

    partial class LoyaltyIssueCardForm
    {
        #region Form Components
        private PanelControl panelControl1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelIssueCard;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private Label labelAccountInfo;
        private Label labelCustomerId;
        private Label labelCustomerName;
        private Label labelCustomerIdValue;
        private Label labelCustomerNameValue;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCustomerSearch;
        private LSRetailPosis.POSProcesses.WinControls.NumPad numPad1;
        #endregion
        private System.ComponentModel.IContainer components;


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
            this.numPad1 = new LSRetailPosis.POSProcesses.WinControls.NumPad();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAccountInfo = new System.Windows.Forms.Label();
            this.labelCustomerId = new System.Windows.Forms.Label();
            this.labelCustomerName = new System.Windows.Forms.Label();
            this.btnCustomerSearch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.labelCustomerIdValue = new System.Windows.Forms.Label();
            this.labelCustomerNameValue = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelIssueCard = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1024, 768);
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
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(30, 687);
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
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
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
            this.btnOk.TabIndex = 1;
            this.btnOk.Tag = "";
            this.btnOk.Text = "OK";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.numPad1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(108, 226);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 365);
            this.tableLayoutPanel1.TabIndex = 1;
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
            this.numPad1.EntryType = Microsoft.Dynamics.Retail.Pos.Contracts.UI.NumpadEntryTypes.AlphaNumeric;
            this.numPad1.Location = new System.Drawing.Point(378, 33);
            this.numPad1.MaskChar = "";
            this.numPad1.MaskInterval = 0;
            this.numPad1.MaxNumberOfDigits = 30;
            this.numPad1.MinimumSize = new System.Drawing.Size(300, 330);
            this.numPad1.Name = "numPad1";
            this.numPad1.NegativeMode = false;
            this.numPad1.NoOfTries = 0;
            this.numPad1.NumberOfDecimals = 0;
            this.numPad1.PromptText = "";
            this.numPad1.ShortcutKeysActive = false;
            this.numPad1.Size = new System.Drawing.Size(300, 330);
            this.numPad1.TabIndex = 1;
            this.numPad1.TimerEnabled = true;
            this.numPad1.EnterButtonPressed += new LSRetailPosis.POSProcesses.WinControls.NumPad.enterbuttonDelegate(this.numPad1_EnterButtonPressed);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.labelAccountInfo, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.labelCustomerId, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.labelCustomerName, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.btnCustomerSearch, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.labelCustomerIdValue, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.labelCustomerNameValue, 0, 4);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(122, 30);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(0, 0, 120, 0);
            this.tableLayoutPanel5.RowCount = 6;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(253, 335);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // labelAccountInfo
            // 
            this.labelAccountInfo.AutoSize = true;
            this.labelAccountInfo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountInfo.Location = new System.Drawing.Point(0, 0);
            this.labelAccountInfo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelAccountInfo.Name = "labelAccountInfo";
            this.labelAccountInfo.Size = new System.Drawing.Size(93, 25);
            this.labelAccountInfo.TabIndex = 0;
            this.labelAccountInfo.Text = "Customer";
            // 
            // labelCustomerId
            // 
            this.labelCustomerId.AutoSize = true;
            this.labelCustomerId.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerId.Location = new System.Drawing.Point(0, 28);
            this.labelCustomerId.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelCustomerId.Name = "labelCustomerId";
            this.labelCustomerId.Size = new System.Drawing.Size(63, 20);
            this.labelCustomerId.TabIndex = 1;
            this.labelCustomerId.Text = "Account";
            // 
            // labelCustomerName
            // 
            this.labelCustomerName.AutoSize = true;
            this.labelCustomerName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerName.Location = new System.Drawing.Point(0, 155);
            this.labelCustomerName.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelCustomerName.Name = "labelCustomerName";
            this.labelCustomerName.Size = new System.Drawing.Size(55, 26);
            this.labelCustomerName.TabIndex = 3;
            this.labelCustomerName.Text = "Name";
            // 
            // btnCustomerSearch
            // 
            this.btnCustomerSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCustomerSearch.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerSearch.Appearance.Options.UseFont = true;
            this.btnCustomerSearch.Location = new System.Drawing.Point(0, 285);
            this.btnCustomerSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.Size = new System.Drawing.Size(133, 50);
            this.btnCustomerSearch.TabIndex = 7;
            this.btnCustomerSearch.Text = "Change customer";
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // labelCustomerIdValue
            // 
            this.labelCustomerIdValue.AutoSize = true;
            this.labelCustomerIdValue.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerIdValue.Location = new System.Drawing.Point(0, 51);
            this.labelCustomerIdValue.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelCustomerIdValue.Name = "labelCustomerIdValue";
            this.labelCustomerIdValue.Size = new System.Drawing.Size(0, 17);
            this.labelCustomerIdValue.TabIndex = 8;
            // 
            // labelCustomerNameValue
            // 
            this.labelCustomerNameValue.AutoSize = true;
            this.labelCustomerNameValue.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerNameValue.Location = new System.Drawing.Point(0, 181);
            this.labelCustomerNameValue.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelCustomerNameValue.Name = "labelCustomerNameValue";
            this.labelCustomerNameValue.Size = new System.Drawing.Size(0, 17);
            this.labelCustomerNameValue.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.labelIssueCard);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(325, 43);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(369, 95);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // labelIssueCard
            // 
            this.labelIssueCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelIssueCard.AutoSize = true;
            this.labelIssueCard.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelIssueCard.Location = new System.Drawing.Point(0, 0);
            this.labelIssueCard.Margin = new System.Windows.Forms.Padding(0);
            this.labelIssueCard.Name = "labelIssueCard";
            this.labelIssueCard.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelIssueCard.Size = new System.Drawing.Size(369, 95);
            this.labelIssueCard.TabIndex = 0;
            this.labelIssueCard.Tag = "";
            this.labelIssueCard.Text = "Issue loyalty card";
            // 
            // LoyaltyIssueCardForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "LoyaltyIssueCardForm";
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
