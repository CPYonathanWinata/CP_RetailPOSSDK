/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Drawing2D;
using LSRetailPosis;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch
{
    sealed partial class OrderDetailsNavigationBar : UserControl
    {        
        private readonly Color gradient1 = Color.FromArgb(214, 231, 247);
        private readonly Color gradient2 = Color.FromArgb(148, 182, 231);

        private const int BulletDiameter = 8;
        private int selectedIndex;

        public event EventHandler SelectedIndexChanged;

        public OrderDetailsNavigationBar()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
				SetLinkColor();
                TranslateLabels();
            }

            base.OnLoad(e);
        }

		private void SetLinkColor()
		{
			this.labelOrderInfo.LinkColor = this.ForeColor;
			this.labelCustomerInfo.LinkColor = this.ForeColor;
			this.labelItemDetails.LinkColor = this.ForeColor;
			this.labelShipping.LinkColor = this.ForeColor;
			this.labelPaymentInfo.LinkColor = this.ForeColor;
			this.labelCancel.LinkColor = this.ForeColor;
			this.labelSummary.LinkColor = this.ForeColor;
		}

        private void TranslateLabels()
        {
            labelOrderInfo.Text    = ApplicationLocalizer.Language.Translate(56328); // "Order information"
            labelCustomerInfo.Text = ApplicationLocalizer.Language.Translate(56329); // "Customer information"
            labelItemDetails.Text  = ApplicationLocalizer.Language.Translate(56330); // "Item details"
            labelShipping.Text     = ApplicationLocalizer.Language.Translate(56331); // "Shipping and delivery"
            labelPaymentInfo.Text  = ApplicationLocalizer.Language.Translate(56332); // "Payment history"
            labelCancel.Text       = ApplicationLocalizer.Language.Translate(56333); // "Cancellation charge"
            labelSummary.Text      = ApplicationLocalizer.Language.Translate(56334); // "Order summary"
        }

        public void SetViewModel(OrderDetailsViewModel vm)
        {
            this.bindingSource.Add(vm);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (e != null)
            {
                using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, gradient1, gradient2, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(lgb, this.ClientRectangle);
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected link index
        /// </summary>
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                if (this.selectedIndex != value)
                {
                    this.selectedIndex = value;
                    this.tableLayoutPanel.Invalidate(false);

                    OnSelectedIndexChanged();
                }
            }
        }

        private void OnSelectedIndexChanged()
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void OnTableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            // Draw the bullet
            if (e.Column == 0 && e.Row == this.SelectedIndex)
            {
                Rectangle bullet = new Rectangle(
                    e.CellBounds.X + ((e.CellBounds.Width - BulletDiameter) / 2),
                    e.CellBounds.Y + ((e.CellBounds.Height - BulletDiameter) / 2),
                    BulletDiameter,
                    BulletDiameter);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (SolidBrush b = new SolidBrush(this.ForeColor))
                {                    
                    e.Graphics.FillEllipse(b, bullet);
                }

                using (Pen p = new Pen(this.ForeColor))
                {
                    e.Graphics.DrawEllipse(p, bullet);
                }                
            }
        }

        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel label = sender as LinkLabel;
            if (label != null)
            {
                // Update the selected index so we can draw the bullet
                this.SelectedIndex = this.tableLayoutPanel.GetRow(label);
            }
        }        
    }    
}