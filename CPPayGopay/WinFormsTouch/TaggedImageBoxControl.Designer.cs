/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    partial class TaggedImageBoxControl
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
            EndImageRefresh();

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
            this.TaggedImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.TaggedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // TaggedImage
            // 
            this.TaggedImage.Location = new System.Drawing.Point(0, 0);
            this.TaggedImage.Name = "TaggedImage";
            this.TaggedImage.Size = new System.Drawing.Size(120, 90);
            this.TaggedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.TaggedImage.TabIndex = 0;
            this.TaggedImage.TabStop = false;
            this.TaggedImage.Click += new System.EventHandler(this.TaggedImage_Click);
            // 
            // TaggedImageBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TaggedImage);
            this.Name = "TaggedImageBoxControl";
            this.Size = new System.Drawing.Size(120, 90);
            ((System.ComponentModel.ISupportInitialize)(this.TaggedImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox TaggedImage;
    }
}
