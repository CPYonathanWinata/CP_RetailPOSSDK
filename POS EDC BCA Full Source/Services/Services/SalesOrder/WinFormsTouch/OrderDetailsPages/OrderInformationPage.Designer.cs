/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class OrderInformationPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelLoyaltyCard = new System.Windows.Forms.Label();
            this.labelOrderType = new System.Windows.Forms.Label();
            this.labelSalesPerson = new System.Windows.Forms.Label();
            this.labelExpiration = new System.Windows.Forms.Label();
            this.labelComment = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxType = new DevExpress.XtraEditors.TextEdit();
            this.textBoxStatus = new DevExpress.XtraEditors.TextEdit();
            this.textBoxPerson = new DevExpress.XtraEditors.TextEdit();
            this.textBoxComment = new DevExpress.XtraEditors.MemoEdit();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.labelOrderId = new System.Windows.Forms.Label();
            this.textBoxOrderId = new DevExpress.XtraEditors.TextEdit();
            this.textBoxLoyaltyCard = new DevExpress.XtraEditors.TextEdit();
            this.labelChannelOrder = new System.Windows.Forms.Label();
            this.textBoxChannelOrder = new DevExpress.XtraEditors.TextEdit();
            this.btnPersonEdit = new LSRetailPosis.POSProcesses.WinControls.SimplePopupButton();
            this.btnEditType = new LSRetailPosis.POSProcesses.WinControls.SimplePopupButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPerson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxComment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxOrderId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxLoyaltyCard.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxChannelOrder.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelLoyaltyCard, 0, 12);
            this.tableLayoutPanel.Controls.Add(this.labelOrderType, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelSalesPerson, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.labelExpiration, 0, 8);
            this.tableLayoutPanel.Controls.Add(this.labelComment, 0, 10);
            this.tableLayoutPanel.Controls.Add(this.labelStatus, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxType, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxStatus, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.textBoxPerson, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.textBoxComment, 0, 11);
            this.tableLayoutPanel.Controls.Add(this.dateTimePicker, 0, 9);
            this.tableLayoutPanel.Controls.Add(this.labelOrderId, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxOrderId, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxLoyaltyCard, 0, 13);
            this.tableLayoutPanel.Controls.Add(this.labelChannelOrder, 0, 14);
            this.tableLayoutPanel.Controls.Add(this.textBoxChannelOrder, 0, 15);
            this.tableLayoutPanel.Controls.Add(this.btnPersonEdit, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.btnEditType, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 17;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(701, 680);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelLoyaltyCard
            // 
            this.labelLoyaltyCard.AutoSize = true;
            this.labelLoyaltyCard.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelLoyaltyCard.Location = new System.Drawing.Point(3, 408);
            this.labelLoyaltyCard.Name = "labelLoyaltyCard";
            this.labelLoyaltyCard.Size = new System.Drawing.Size(97, 21);
            this.labelLoyaltyCard.TabIndex = 14;
            this.labelLoyaltyCard.Text = "Loyalty card:";
            // 
            // labelOrderType
            // 
            this.labelOrderType.AutoSize = true;
            this.labelOrderType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelOrderType.Location = new System.Drawing.Point(3, 61);
            this.labelOrderType.Name = "labelOrderType";
            this.labelOrderType.Size = new System.Drawing.Size(88, 21);
            this.labelOrderType.TabIndex = 2;
            this.labelOrderType.Text = "Order type:";
            // 
            // labelSalesPerson
            // 
            this.labelSalesPerson.AutoSize = true;
            this.labelSalesPerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelSalesPerson.Location = new System.Drawing.Point(3, 187);
            this.labelSalesPerson.Name = "labelSalesPerson";
            this.labelSalesPerson.Size = new System.Drawing.Size(101, 21);
            this.labelSalesPerson.TabIndex = 7;
            this.labelSalesPerson.Text = "Sales person:";
            // 
            // labelExpiration
            // 
            this.labelExpiration.AutoSize = true;
            this.labelExpiration.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelExpiration.Location = new System.Drawing.Point(3, 252);
            this.labelExpiration.Name = "labelExpiration";
            this.labelExpiration.Size = new System.Drawing.Size(116, 21);
            this.labelExpiration.TabIndex = 10;
            this.labelExpiration.Text = "Expiration date:";
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelComment.Location = new System.Drawing.Point(3, 311);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(89, 21);
            this.labelComment.TabIndex = 12;
            this.labelComment.Text = "Comments:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelStatus.Location = new System.Drawing.Point(3, 126);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(99, 21);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "Order status:";
            // 
            // textBoxType
            // 
            this.textBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedOrderType", true));
            this.textBoxType.Location = new System.Drawing.Point(3, 94);
            this.textBoxType.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxType.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxType.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxType.Properties.Appearance.Options.UseFont = true;
            this.textBoxType.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxType.Properties.ReadOnly = true;
            this.textBoxType.Size = new System.Drawing.Size(344, 24);
            this.textBoxType.TabIndex = 3;
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "FormattedOrderStatus", true));
            this.textBoxStatus.Location = new System.Drawing.Point(3, 155);
            this.textBoxStatus.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxStatus.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatus.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxStatus.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxStatus.Properties.Appearance.Options.UseFont = true;
            this.textBoxStatus.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxStatus.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxStatus.Properties.ReadOnly = true;
            this.textBoxStatus.Size = new System.Drawing.Size(344, 24);
            this.textBoxStatus.TabIndex = 6;
            // 
            // textBoxPerson
            // 
            this.textBoxPerson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPerson.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SalesPerson", true));
            this.textBoxPerson.Location = new System.Drawing.Point(3, 220);
            this.textBoxPerson.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textBoxPerson.Name = "textBoxPerson";
            this.textBoxPerson.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxPerson.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPerson.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxPerson.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxPerson.Properties.Appearance.Options.UseFont = true;
            this.textBoxPerson.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxPerson.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxPerson.Properties.ReadOnly = true;
            this.textBoxPerson.Size = new System.Drawing.Size(344, 24);
            this.textBoxPerson.TabIndex = 8;
            // 
            // textBoxComment
            // 
            this.textBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "OrderComment", true));
            this.textBoxComment.Location = new System.Drawing.Point(3, 340);
            this.textBoxComment.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxComment.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxComment.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxComment.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxComment.Properties.Appearance.Options.UseFont = true;
            this.textBoxComment.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxComment.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxComment.Properties.MaxLength = 60;
            this.textBoxComment.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxComment.Size = new System.Drawing.Size(344, 60);
            this.textBoxComment.TabIndex = 13;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource, "OrderExpirationDate", true));
            this.dateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("MinDate", this.bindingSource, "MinimumOrderExpirationDate", true));
            this.dateTimePicker.Location = new System.Drawing.Point(3, 281);
            this.dateTimePicker.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.dateTimePicker.MaximumSize = new System.Drawing.Size(344, 22);
            this.dateTimePicker.MinimumSize = new System.Drawing.Size(344, 22);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(344, 22);
            this.dateTimePicker.TabIndex = 11;
            // 
            // labelOrderId
            // 
            this.labelOrderId.AutoSize = true;
            this.labelOrderId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelOrderId.Location = new System.Drawing.Point(3, 0);
            this.labelOrderId.Name = "labelOrderId";
            this.labelOrderId.Size = new System.Drawing.Size(113, 21);
            this.labelOrderId.TabIndex = 0;
            this.labelOrderId.Text = "Order number:";
            // 
            // textBoxOrderId
            // 
            this.textBoxOrderId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOrderId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "OrderId", true));
            this.textBoxOrderId.Location = new System.Drawing.Point(3, 29);
            this.textBoxOrderId.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textBoxOrderId.Name = "textBoxOrderId";
            this.textBoxOrderId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxOrderId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOrderId.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxOrderId.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxOrderId.Properties.Appearance.Options.UseFont = true;
            this.textBoxOrderId.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxOrderId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxOrderId.Properties.ReadOnly = true;
            this.textBoxOrderId.Size = new System.Drawing.Size(344, 24);
            this.textBoxOrderId.TabIndex = 1;
            // 
            // textBoxLoyaltyCard
            // 
            this.textBoxLoyaltyCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLoyaltyCard.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "LoyaltyCardId", true));
            this.textBoxLoyaltyCard.Location = new System.Drawing.Point(3, 437);
            this.textBoxLoyaltyCard.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textBoxLoyaltyCard.Name = "textBoxLoyaltyCard";
            this.textBoxLoyaltyCard.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxLoyaltyCard.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLoyaltyCard.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxLoyaltyCard.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxLoyaltyCard.Properties.Appearance.Options.UseFont = true;
            this.textBoxLoyaltyCard.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxLoyaltyCard.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxLoyaltyCard.Properties.ReadOnly = true;
            this.textBoxLoyaltyCard.Size = new System.Drawing.Size(344, 24);
            this.textBoxLoyaltyCard.TabIndex = 15;
            // 
            // labelChannelOrder
            // 
            this.labelChannelOrder.AutoSize = true;
            this.labelChannelOrder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelChannelOrder.Location = new System.Drawing.Point(3, 469);
            this.labelChannelOrder.Name = "labelChannelOrder";
            this.labelChannelOrder.Size = new System.Drawing.Size(115, 21);
            this.labelChannelOrder.TabIndex = 16;
            this.labelChannelOrder.Text = "Channel Order:";
            // 
            // textBoxChannelOrder
            // 
            this.textBoxChannelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChannelOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "ChannelCardId", true));
            this.textBoxChannelOrder.Location = new System.Drawing.Point(3, 498);
            this.textBoxChannelOrder.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.textBoxChannelOrder.Name = "textBoxChannelOrder";
            this.textBoxChannelOrder.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textBoxChannelOrder.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChannelOrder.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textBoxChannelOrder.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxChannelOrder.Properties.Appearance.Options.UseFont = true;
            this.textBoxChannelOrder.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxChannelOrder.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textBoxChannelOrder.Properties.ReadOnly = true;
            this.textBoxChannelOrder.Size = new System.Drawing.Size(344, 24);
            this.textBoxChannelOrder.TabIndex = 17;
            // 
            // btnPersonEdit
            // 
            this.btnPersonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPersonEdit.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPersonEdit.Appearance.Options.UseFont = true;
            this.btnPersonEdit.Location = new System.Drawing.Point(353, 187);
            this.btnPersonEdit.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.btnPersonEdit.MaximumSize = new System.Drawing.Size(127, 57);
            this.btnPersonEdit.Name = "btnPersonEdit";
            this.btnPersonEdit.Padding = new System.Windows.Forms.Padding(0);
            this.btnPersonEdit.PopupContent = null;
            this.tableLayoutPanel.SetRowSpan(this.btnPersonEdit, 2);
            this.btnPersonEdit.Size = new System.Drawing.Size(127, 57);
            this.btnPersonEdit.TabIndex = 9;
            this.btnPersonEdit.Text = "Change...";
            this.btnPersonEdit.Click += new System.EventHandler(this.OnBtnPersonEdit_Click);
            // 
            // btnEditType
            // 
            this.btnEditType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditType.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditType.Appearance.Options.UseFont = true;
            this.btnEditType.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "IsOrderTypeChangeAllowed", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnEditType.Location = new System.Drawing.Point(353, 61);
            this.btnEditType.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.btnEditType.MaximumSize = new System.Drawing.Size(127, 57);
            this.btnEditType.Name = "btnEditType";
            this.btnEditType.Padding = new System.Windows.Forms.Padding(0);
            this.btnEditType.PopupContent = null;
            this.tableLayoutPanel.SetRowSpan(this.btnEditType, 2);
            this.btnEditType.Size = new System.Drawing.Size(127, 57);
            this.btnEditType.TabIndex = 4;
            this.btnEditType.Text = "Change...";
            // 
            // OrderInformationPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OrderInformationPage";
            this.Size = new System.Drawing.Size(701, 680);
            this.Text = "Order information";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPerson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxComment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxOrderId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxLoyaltyCard.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxChannelOrder.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelOrderType;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.Label labelExpiration;
        private System.Windows.Forms.Label labelSalesPerson;
        private System.Windows.Forms.Label labelStatus;
        private DevExpress.XtraEditors.TextEdit textBoxType;
        private DevExpress.XtraEditors.TextEdit textBoxStatus;
        private DevExpress.XtraEditors.TextEdit textBoxPerson;
        private DevExpress.XtraEditors.MemoEdit textBoxComment;
        private LSRetailPosis.POSProcesses.WinControls.SimplePopupButton btnEditType;
        private LSRetailPosis.POSProcesses.WinControls.SimplePopupButton btnPersonEdit;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label labelOrderId;
        private DevExpress.XtraEditors.TextEdit textBoxOrderId;
        private System.Windows.Forms.Label labelLoyaltyCard;
        private DevExpress.XtraEditors.TextEdit textBoxLoyaltyCard;
        private System.Windows.Forms.Label labelChannelOrder;
        private DevExpress.XtraEditors.TextEdit textBoxChannelOrder;
    }
}
