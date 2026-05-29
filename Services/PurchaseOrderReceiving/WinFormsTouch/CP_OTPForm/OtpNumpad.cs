using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.PurchaseOrderReceiving.WinFormsTouch.CP_OTPForm
{
    public partial class OtpNumpad : UserControl
    {
        public TextBox TargetTextBox { get; set; }

        public int MaxLength { get; set; }

        public OtpNumpad()
        {
            InitializeComponent();
            BuildButtons(); 
            MaxLength = 6;
        }

        private void BuildButtons()
        {
            int btnW = 70;
            int btnH = 70;
            int margin = 5;

            string[] keys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "C", "0", "←" };

            for (int i = 0; i < keys.Length; i++)
            {
                Button btn = new Button();
                btn.Text = keys[i];
                btn.Width = btnW;
                btn.Height = btnH;

                int row = i / 3;
                int col = i % 3;

                btn.Left = col * (btnW + margin);
                btn.Top = row * (btnH + margin);

                btn.Click += (s, e) => KeyPressed(btn.Text);

                this.Controls.Add(btn);
            }
        }

        private void KeyPressed(string key)
        {
            if (TargetTextBox == null)
                return;

            if (key == "←")
            {
                if (TargetTextBox.Text.Length > 0)
                {
                    TargetTextBox.Text = TargetTextBox.Text.Substring(0, TargetTextBox.Text.Length - 1);
                }
                return;
            }

            if (key == "C")
            {
                TargetTextBox.Clear();
                return;
            }

            // limit panjang OTP
            if (TargetTextBox.Text.Length >= MaxLength)
                return;

            TargetTextBox.Text += key;
        }
    }
}
