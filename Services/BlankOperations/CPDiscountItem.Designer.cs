using System.Drawing;
namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    partial class CPDiscountItem
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            
            this.lblHeader = new System.Windows.Forms.Label();
            this.dataGridResult = new System.Windows.Forms.DataGridView();
            this.closeBtn = new System.Windows.Forms.Button();
            this.totalAmountTextBox = new System.Windows.Forms.TextBox();
            this.dataGridHeader = new System.Windows.Forms.DataGridView();
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(172, 60);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(674, 65);
            this.lblHeader.TabIndex = 7;
            this.lblHeader.Text = "";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //this.lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            this.lblHeader.SizeChanged += Label_SizeChanged;
            // 
            // dataGridResult
            // 
            this.dataGridResult.AllowUserToAddRows = false;
            this.dataGridResult.AllowUserToDeleteRows = false;
            this.dataGridResult.AllowUserToResizeRows = false;
            this.dataGridResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResult.EnableHeadersVisualStyles = false;
            this.dataGridResult.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.dataGridResult.Location = new System.Drawing.Point(29, 413);
            this.dataGridResult.Name = "dataGridResult";
            this.dataGridResult.ReadOnly = true;
            this.dataGridResult.RowHeadersVisible = false;
            this.dataGridResult.RowTemplate.Height = 40;
            this.dataGridResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridResult.Size = new System.Drawing.Size(962, 260);
            this.dataGridResult.TabIndex = 6;
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(194)))), ((int)(((byte)(215)))));
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.closeBtn.Location = new System.Drawing.Point(448, 689);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(127, 57);
            this.closeBtn.TabIndex = 9;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // totalAmountTextBox
            // 
            this.totalAmountTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            this.totalAmountTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.totalAmountTextBox.Font = new System.Drawing.Font("Tahoma", 16F);
            this.totalAmountTextBox.Location = new System.Drawing.Point(339, 140);
            this.totalAmountTextBox.Name = "totalAmountTextBox";
            this.totalAmountTextBox.ReadOnly = true;
            this.totalAmountTextBox.Size = new System.Drawing.Size(345, 26);
            this.totalAmountTextBox.TabIndex = 11;
            this.totalAmountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dataGridHeader
            // 
            this.dataGridHeader.AllowUserToAddRows = false;
            this.dataGridHeader.AllowUserToDeleteRows = false;
            this.dataGridHeader.AllowUserToResizeRows = false;
            this.dataGridHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(222)))), ((int)(((byte)(229)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(145)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridHeader.EnableHeadersVisualStyles = false;
            this.dataGridHeader.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            this.dataGridHeader.Location = new System.Drawing.Point(29, 181);
            this.dataGridHeader.Name = "dataGridHeader";
            this.dataGridHeader.ReadOnly = true;
            this.dataGridHeader.RowHeadersVisible = false;
            this.dataGridHeader.RowTemplate.Height = 40;
            this.dataGridHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridHeader.Size = new System.Drawing.Size(962, 211);
            this.dataGridHeader.TabIndex = 12;
            this.dataGridHeader.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridHeader_DataBindingComplete);
            this.dataGridHeader.SelectionChanged += new System.EventHandler(this.dataGridHeader_SelectionChanged);
            // 
            // CPDiscountItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.dataGridHeader);
            this.Controls.Add(this.totalAmountTextBox);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.dataGridResult);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "CPDiscountItem";
            this.Text = "CPDiscountPayment";
            this.Controls.SetChildIndex(this.dataGridResult, 0);
            this.Controls.SetChildIndex(this.lblHeader, 0);
            this.Controls.SetChildIndex(this.closeBtn, 0);
            this.Controls.SetChildIndex(this.totalAmountTextBox, 0);
            this.Controls.SetChildIndex(this.dataGridHeader, 0);
            //((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
        internal System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView dataGridResult;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.TextBox totalAmountTextBox;
        private System.Windows.Forms.DataGridView dataGridHeader;
    }
}