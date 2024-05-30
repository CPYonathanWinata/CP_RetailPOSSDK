/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class RegisterTimeForm
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
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.lblCurrentStatus = new System.Windows.Forms.Label();
            this.bindingSource = new System.Windows.Forms.BindingSource();
            this.lblLastActivity = new System.Windows.Forms.Label();
            this.btnClockIn = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClockOut = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnBreakFromWork = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnBreakForLunch = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnLogbook = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnClose = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tlpContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 7;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.Controls.Add(this.labelTitle, 0, 0);
            this.tlpContent.Controls.Add(this.lblCurrentStatus, 1, 3);
            this.tlpContent.Controls.Add(this.lblLastActivity, 3, 3);
            this.tlpContent.Controls.Add(this.btnClockIn, 1, 2);
            this.tlpContent.Controls.Add(this.btnClockOut, 2, 2);
            this.tlpContent.Controls.Add(this.btnBreakFromWork, 3, 2);
            this.tlpContent.Controls.Add(this.btnBreakForLunch, 4, 2);
            this.tlpContent.Controls.Add(this.btnLogbook, 5, 2);
            this.tlpContent.Controls.Add(this.btnClose, 2, 5);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tlpContent.Location = new System.Drawing.Point(0, 0);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tlpContent.RowCount = 6;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.Size = new System.Drawing.Size(1024, 768);
            this.tlpContent.TabIndex = 8;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.tlpContent.SetColumnSpan(this.labelTitle, 7);
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.labelTitle.Location = new System.Drawing.Point(367, 40);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.labelTitle.Size = new System.Drawing.Size(289, 95);
            this.labelTitle.TabIndex = 9;
            this.labelTitle.Text = "Register time";
            // 
            // lblCurrentStatus
            // 
            this.lblCurrentStatus.AutoSize = true;
            this.tlpContent.SetColumnSpan(this.lblCurrentStatus, 2);
            this.lblCurrentStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "RegisterTimeType", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.lblCurrentStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentStatus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCurrentStatus.Location = new System.Drawing.Point(168, 416);
            this.lblCurrentStatus.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCurrentStatus.Name = "lblCurrentStatus";
            this.lblCurrentStatus.Size = new System.Drawing.Size(268, 60);
            this.lblCurrentStatus.TabIndex = 6;
            this.lblCurrentStatus.Tag = "Normal";
            this.lblCurrentStatus.Text = "Current status:";
            this.lblCurrentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bindingSource
            // 
            this.bindingSource.DataSource = typeof(Microsoft.Dynamics.Retail.Pos.Interaction.ViewModel.RegisterTimeFormViewModel);
            // 
            // lblLastActivity
            // 
            this.tlpContent.SetColumnSpan(this.lblLastActivity, 3);
            this.lblLastActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "LastActivity", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.lblLastActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLastActivity.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblLastActivity.Location = new System.Drawing.Point(448, 416);
            this.lblLastActivity.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLastActivity.Name = "lblLastActivity";
            this.lblLastActivity.Size = new System.Drawing.Size(408, 60);
            this.lblLastActivity.TabIndex = 7;
            this.lblLastActivity.Tag = "Normal";
            this.lblLastActivity.Text = "Last activity:";
            this.lblLastActivity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClockIn
            // 
            this.btnClockIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClockIn.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClockIn.Appearance.Options.UseFont = true;
            this.btnClockIn.Appearance.Options.UseTextOptions = true;
            this.btnClockIn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnClockIn.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "RegisterTimeType", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.btnClockIn.Location = new System.Drawing.Point(168, 355);
            this.btnClockIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnClockIn.Name = "btnClockIn";
            this.btnClockIn.Size = new System.Drawing.Size(127, 57);
            this.btnClockIn.TabIndex = 0;
            this.btnClockIn.Tag = "BtnNormal";
            this.btnClockIn.Text = "Clock-in";
            this.btnClockIn.Click += new System.EventHandler(this.btnClockIn_Click);
            // 
            // btnClockOut
            // 
            this.btnClockOut.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClockOut.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClockOut.Appearance.Options.UseFont = true;
            this.btnClockOut.Appearance.Options.UseTextOptions = true;
            this.btnClockOut.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnClockOut.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "RegisterTimeType", true));
            this.btnClockOut.Location = new System.Drawing.Point(308, 355);
            this.btnClockOut.Margin = new System.Windows.Forms.Padding(4);
            this.btnClockOut.Name = "btnClockOut";
            this.btnClockOut.Size = new System.Drawing.Size(127, 57);
            this.btnClockOut.TabIndex = 1;
            this.btnClockOut.Tag = "BtnNormal";
            this.btnClockOut.Text = "Clock-out";
            this.btnClockOut.Click += new System.EventHandler(this.btnClockOut_Click);
            // 
            // btnBreakFromWork
            // 
            this.btnBreakFromWork.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBreakFromWork.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBreakFromWork.Appearance.Options.UseFont = true;
            this.btnBreakFromWork.Appearance.Options.UseTextOptions = true;
            this.btnBreakFromWork.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnBreakFromWork.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "RegisterBreakType", true));
            this.btnBreakFromWork.Location = new System.Drawing.Point(446, 355);
            this.btnBreakFromWork.Margin = new System.Windows.Forms.Padding(4);
            this.btnBreakFromWork.Name = "btnBreakFromWork";
            this.btnBreakFromWork.Size = new System.Drawing.Size(132, 57);
            this.btnBreakFromWork.TabIndex = 2;
            this.btnBreakFromWork.Tag = "BtnNormal";
            this.btnBreakFromWork.Text = "Break from work";
            this.btnBreakFromWork.Click += new System.EventHandler(this.btnBreakFromWork_Click);
            // 
            // btnBreakForLunch
            // 
            this.btnBreakForLunch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBreakForLunch.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBreakForLunch.Appearance.Options.UseFont = true;
            this.btnBreakForLunch.Appearance.Options.UseTextOptions = true;
            this.btnBreakForLunch.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnBreakForLunch.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSource, "RegisterBreakType", true));
            this.btnBreakForLunch.Location = new System.Drawing.Point(586, 355);
            this.btnBreakForLunch.Margin = new System.Windows.Forms.Padding(4);
            this.btnBreakForLunch.Name = "btnBreakForLunch";
            this.btnBreakForLunch.Size = new System.Drawing.Size(132, 57);
            this.btnBreakForLunch.TabIndex = 3;
            this.btnBreakForLunch.Tag = "BtnNormal";
            this.btnBreakForLunch.Text = "Break for lunch";
            this.btnBreakForLunch.Click += new System.EventHandler(this.btnBreakForLunch_Click);
            // 
            // btnLogbook
            // 
            this.btnLogbook.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogbook.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogbook.Appearance.Options.UseFont = true;
            this.btnLogbook.Appearance.Options.UseTextOptions = true;
            this.btnLogbook.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnLogbook.Location = new System.Drawing.Point(728, 355);
            this.btnLogbook.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogbook.Name = "btnLogbook";
            this.btnLogbook.Size = new System.Drawing.Size(127, 57);
            this.btnLogbook.TabIndex = 4;
            this.btnLogbook.Tag = "BtnNormal";
            this.btnLogbook.Text = "Logbook";
            this.btnLogbook.Click += new System.EventHandler(this.btnLogbook_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.CausesValidation = false;
            this.tlpContent.SetColumnSpan(this.btnClose, 3);
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(448, 696);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 57);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            // 
            // RegisterTimeForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tlpContent);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RegisterTimeForm";
            this.Text = "Time clock";
            this.Controls.SetChildIndex(this.tlpContent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClockIn;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClockOut;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnBreakFromWork;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnBreakForLunch;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnLogbook;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnClose;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Label lblCurrentStatus;
        private System.Windows.Forms.Label lblLastActivity;
        private System.Windows.Forms.Label labelTitle;
    }
}