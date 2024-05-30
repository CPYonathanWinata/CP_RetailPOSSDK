using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

using PaymentTriggers;

namespace PaymentTriggers
{
    partial class CP_FormPayment
    {
        /// <summary>
        /// Required designer variable.
          static SerialPort mySerialPort;
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
            this.debitBCA_btn = new System.Windows.Forms.Button();
            this.error_btn = new System.Windows.Forms.Button();
            this.creditBCA_btn = new System.Windows.Forms.Button();
            this.flazzBCA_btn = new System.Windows.Forms.Button();
            this.other_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // debitBCA_btn
            // 
            this.debitBCA_btn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.debitBCA_btn.Location = new System.Drawing.Point(89, 12);
            this.debitBCA_btn.Name = "debitBCA_btn";
            this.debitBCA_btn.Size = new System.Drawing.Size(92, 29);
            this.debitBCA_btn.TabIndex = 11;
            this.debitBCA_btn.Text = "Debit BCA";
            this.debitBCA_btn.UseVisualStyleBackColor = true;
            this.debitBCA_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // error_btn
            // 
            this.error_btn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error_btn.Location = new System.Drawing.Point(76, 135);
            this.error_btn.Name = "error_btn";
            this.error_btn.Size = new System.Drawing.Size(105, 29);
            this.error_btn.TabIndex = 12;
            this.error_btn.Text = "false Debit BCA";
            this.error_btn.UseVisualStyleBackColor = true;
            this.error_btn.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // creditBCA_btn
            // 
            this.creditBCA_btn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creditBCA_btn.Location = new System.Drawing.Point(89, 47);
            this.creditBCA_btn.Name = "creditBCA_btn";
            this.creditBCA_btn.Size = new System.Drawing.Size(92, 29);
            this.creditBCA_btn.TabIndex = 13;
            this.creditBCA_btn.Text = "Kredit BCA";
            this.creditBCA_btn.UseVisualStyleBackColor = true;
            this.creditBCA_btn.Click += new System.EventHandler(this.button3_Click);
            // 
            // flazzBCA_btn
            // 
            this.flazzBCA_btn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flazzBCA_btn.Location = new System.Drawing.Point(89, 82);
            this.flazzBCA_btn.Name = "flazzBCA_btn";
            this.flazzBCA_btn.Size = new System.Drawing.Size(92, 29);
            this.flazzBCA_btn.TabIndex = 14;
            this.flazzBCA_btn.Text = "Flazz BCA";
            this.flazzBCA_btn.UseVisualStyleBackColor = true;
            this.flazzBCA_btn.Click += new System.EventHandler(this.button4_Click);
            // 
            // other_btn
            // 
            this.other_btn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.other_btn.Location = new System.Drawing.Point(76, 186);
            this.other_btn.Name = "other_btn";
            this.other_btn.Size = new System.Drawing.Size(105, 29);
            this.other_btn.TabIndex = 15;
            this.other_btn.Text = "Other Card";
            this.other_btn.UseVisualStyleBackColor = true;
            this.other_btn.Click += new System.EventHandler(this.button5_Click);
            // 
            // CP_FormPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.other_btn);
            this.Controls.Add(this.flazzBCA_btn);
            this.Controls.Add(this.creditBCA_btn);
            this.Controls.Add(this.error_btn);
            this.Controls.Add(this.debitBCA_btn);
            this.Name = "CP_FormPayment";
            this.Text = "Card Payment";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button debitBCA_btn;
        private System.Windows.Forms.Button error_btn;
        private System.Windows.Forms.Button creditBCA_btn;
        private System.Windows.Forms.Button flazzBCA_btn;
        private System.Windows.Forms.Button other_btn;

    }
}