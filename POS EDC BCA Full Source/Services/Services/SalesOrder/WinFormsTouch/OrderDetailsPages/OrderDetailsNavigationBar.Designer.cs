/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class OrderDetailsNavigationBar
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelCustomerInfo = new System.Windows.Forms.LinkLabel();
            this.labelOrderInfo = new System.Windows.Forms.LinkLabel();
            this.labelItemDetails = new System.Windows.Forms.LinkLabel();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelShipping = new System.Windows.Forms.LinkLabel();
            this.labelSummary = new System.Windows.Forms.LinkLabel();
            this.labelPaymentInfo = new System.Windows.Forms.LinkLabel();
            this.labelCancel = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelCustomerInfo, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelOrderInfo, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelItemDetails, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelShipping, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.labelSummary, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.labelPaymentInfo, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelCancel, 1, 5);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(316, 328);
            this.tableLayoutPanel.TabIndex = 0;
            this.tableLayoutPanel.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.OnTableLayoutPanel_CellPaint);
            // 
            // labelCustomerInfo
            // 
            this.labelCustomerInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomerInfo.AutoSize = true;
            this.labelCustomerInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelCustomerInfo.Location = new System.Drawing.Point(33, 51);
            this.labelCustomerInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelCustomerInfo.Name = "labelCustomerInfo";
            this.labelCustomerInfo.Size = new System.Drawing.Size(164, 21);
            this.labelCustomerInfo.TabIndex = 1;
            this.labelCustomerInfo.TabStop = true;
            this.labelCustomerInfo.Text = "Customer information";
            this.labelCustomerInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // labelOrderInfo
            // 
            this.labelOrderInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelOrderInfo.AutoSize = true;
            this.labelOrderInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelOrderInfo.Location = new System.Drawing.Point(33, 10);
            this.labelOrderInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelOrderInfo.Name = "labelOrderInfo";
            this.labelOrderInfo.Size = new System.Drawing.Size(137, 21);
            this.labelOrderInfo.TabIndex = 0;
            this.labelOrderInfo.TabStop = true;
            this.labelOrderInfo.Text = "Order information";
            this.labelOrderInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // labelItemDetails
            // 
            this.labelItemDetails.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelItemDetails.AutoSize = true;
            this.labelItemDetails.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "IsRetrieveMode", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelItemDetails.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelItemDetails.Location = new System.Drawing.Point(33, 92);
            this.labelItemDetails.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelItemDetails.Name = "labelItemDetails";
            this.labelItemDetails.Size = new System.Drawing.Size(90, 21);
            this.labelItemDetails.TabIndex = 2;
            this.labelItemDetails.TabStop = true;
            this.labelItemDetails.Text = "Item details";
            this.labelItemDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.SalesOrder.OrderDetailsViewModel);
            // 
            // labelShipping
            // 
            this.labelShipping.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelShipping.AutoSize = true;
            this.labelShipping.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelShipping.Location = new System.Drawing.Point(33, 133);
            this.labelShipping.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelShipping.Name = "labelShipping";
            this.labelShipping.Size = new System.Drawing.Size(161, 21);
            this.labelShipping.TabIndex = 3;
            this.labelShipping.TabStop = true;
            this.labelShipping.Text = "Shipping and delivery";
            this.labelShipping.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // labelSummary
            // 
            this.labelSummary.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSummary.AutoSize = true;
            this.labelSummary.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelSummary.Location = new System.Drawing.Point(33, 256);
            this.labelSummary.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(121, 21);
            this.labelSummary.TabIndex = 6;
            this.labelSummary.TabStop = true;
            this.labelSummary.Text = "Order summary";
            this.labelSummary.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // labelPaymentInfo
            // 
            this.labelPaymentInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelPaymentInfo.AutoSize = true;
            this.labelPaymentInfo.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "IsRetrieveMode", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.labelPaymentInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelPaymentInfo.Location = new System.Drawing.Point(33, 174);
            this.labelPaymentInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelPaymentInfo.Name = "labelPaymentInfo";
            this.labelPaymentInfo.Size = new System.Drawing.Size(123, 21);
            this.labelPaymentInfo.TabIndex = 4;
            this.labelPaymentInfo.TabStop = true;
            this.labelPaymentInfo.Text = "Payment history";
            this.labelPaymentInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // labelCancel
            // 
            this.labelCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCancel.AutoSize = true;
            this.labelCancel.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bindingSource, "IsCancellationChargeVisible", true));
            this.labelCancel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labelCancel.Location = new System.Drawing.Point(33, 215);
            this.labelCancel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(146, 21);
            this.labelCancel.TabIndex = 5;
            this.labelCancel.TabStop = true;
            this.labelCancel.Text = "Cancellation charge";
            this.labelCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // OrderDetailsNavigationBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OrderDetailsNavigationBar";
            this.Size = new System.Drawing.Size(316, 328);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.LinkLabel labelCustomerInfo;
        private System.Windows.Forms.LinkLabel labelSummary;
        private System.Windows.Forms.LinkLabel labelShipping;
        private System.Windows.Forms.LinkLabel labelPaymentInfo;
        private System.Windows.Forms.LinkLabel labelItemDetails;
        private System.Windows.Forms.LinkLabel labelOrderInfo;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.LinkLabel labelCancel;
    }
}
