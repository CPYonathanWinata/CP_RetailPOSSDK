/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    partial class formDeliveryMethod
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnPickup = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnShip = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.btnPickup, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnShip, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 15, 10, 6);
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(587, 322);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // btnPickup
            // 
            this.btnPickup.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPickup.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPickup.Appearance.Options.UseFont = true;
            this.btnPickup.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPickup.Location = new System.Drawing.Point(230, 109);
            this.btnPickup.Margin = new System.Windows.Forms.Padding(4);
            this.btnPickup.Name = "btnPickup";
            this.btnPickup.Size = new System.Drawing.Size(127, 57);
            this.btnPickup.TabIndex = 0;
            this.btnPickup.Text = "Pickup at a store";
            this.btnPickup.Click += new System.EventHandler(this.OnPickClick);
            // 
            // btnShip
            // 
            this.btnShip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShip.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShip.Appearance.Options.UseFont = true;
            this.btnShip.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnShip.Location = new System.Drawing.Point(230, 228);
            this.btnShip.Margin = new System.Windows.Forms.Padding(4);
            this.btnShip.Name = "btnShip";
            this.btnShip.Size = new System.Drawing.Size(127, 57);
            this.btnShip.TabIndex = 2;
            this.btnShip.Text = "Ship";
            this.btnShip.Click += new System.EventHandler(this.OnShipClick);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(78, 15);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblTitle.Size = new System.Drawing.Size(431, 64);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Select a delivery method";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(229, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 34);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Ship";
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.tableLayoutPanel);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(591, 326);
            this.panelControl.TabIndex = 1;
            // 
            // formDeliveryMethod
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(591, 326);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formDeliveryMethod";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnPickup;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnShip;
        private System.Windows.Forms.Label lblTitle;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
    }
}