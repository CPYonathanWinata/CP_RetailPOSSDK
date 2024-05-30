/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    partial class ExtendedLogOnScanForm
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.BtnCaptureImage = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.CapturedImageBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TaggedImageBox = new Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.TaggedImageBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CapturedImageBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.lblMessage, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lblHeader, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.btnCancel, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.btnOk, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.TaggedImageBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.BtnCaptureImage, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.CapturedImageBox, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 115);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1024, 537);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblMessage, 2);
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(10, 251);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(10, 20, 10, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1004, 21);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblHeader, 2);
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(10, 179);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(10, 10, 10, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1004, 32);
            this.lblHeader.TabIndex = 6;
            this.lblHeader.Text = "Swipe the card or scan the bar code, and then click Ok.";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(516, 306);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 14, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnButton_KeyDown);
            this.btnCancel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnButton_PreviewKeyDown);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(381, 306);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 14, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnButton_KeyDown);
            this.btnOk.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnButton_PreviewKeyDown);
            // 
            // BtnCaptureImage
            // 
            this.BtnCaptureImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnCaptureImage.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCaptureImage.Appearance.Options.UseFont = true;
            this.tableLayoutPanel.SetColumnSpan(this.BtnCaptureImage, 2);
            this.BtnCaptureImage.Location = new System.Drawing.Point(439, 149);
            this.BtnCaptureImage.Margin = new System.Windows.Forms.Padding(4, 14, 4, 4);
            this.BtnCaptureImage.Name = "BtnCaptureImage";
            this.BtnCaptureImage.Size = new System.Drawing.Size(145, 50);
            this.BtnCaptureImage.TabIndex = 9;
            this.BtnCaptureImage.Text = "Capture Image";
            this.BtnCaptureImage.Visible = false;
            this.BtnCaptureImage.Click += new System.EventHandler(this.OnBtnCaptureImage_KeyDown);
            // 
            // CapturedImageBox
            // 
            this.CapturedImageBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CapturedImageBox.Location = new System.Drawing.Point(688, 7);
            this.CapturedImageBox.Name = "CapturedImageBox";
            this.CapturedImageBox.Size = new System.Drawing.Size(160, 120);
            this.CapturedImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CapturedImageBox.TabIndex = 10;
            this.CapturedImageBox.TabStop = false;
            this.CapturedImageBox.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // TaggedImageBox
            // 
            this.TaggedImageBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TaggedImageBox.Location = new System.Drawing.Point(176, 7);
            this.TaggedImageBox.Name = "TaggedImageBox";
            this.TaggedImageBox.Size = new System.Drawing.Size(160, 120);
            this.TaggedImageBox.TabIndex = 8;
            this.TaggedImageBox.Visible = false;
            // 
            // ExtendedLogOnScanForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.KeyPreview = true;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "ExtendedLogOnScanForm";
            this.Text = "Extended log on";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnForm_KeyPress);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CapturedImageBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private WinFormsTouch.TaggedImageBoxControl TaggedImageBox;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx BtnCaptureImage;
        private System.Windows.Forms.PictureBox CapturedImageBox;

    }
}
