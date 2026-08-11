using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations.CPPricingSimulator
{
   
    public partial class NumericKeypadForm : Form
    {
        private TextBox txtDisplay;
        private Label lblPrompt;
        public string ResultValue { get; private set; }

        public NumericKeypadForm(string prompt, string title, string defaultValue)
        {
            this.Text = title;
            this.Width = 460;
            this.Height = 610;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblPrompt = new Label();
            lblPrompt.Text = prompt;
            lblPrompt.Left = 20;
            lblPrompt.Top = 20;
            lblPrompt.Width = 400;
            lblPrompt.Font = new Font("Segoe UI", 11F);
            this.Controls.Add(lblPrompt);

            txtDisplay = new TextBox();
            txtDisplay.Left = 20;
            txtDisplay.Top = 50;
            txtDisplay.Width = 400;
            txtDisplay.Height = 40;
            txtDisplay.Font = new Font("Segoe UI", 14F);
            txtDisplay.TextAlign = HorizontalAlignment.Right;
            txtDisplay.ReadOnly = true;
            txtDisplay.Text = defaultValue;
            this.Controls.Add(txtDisplay);

            string[,] keys = new string[4, 4]
        {
            { "7", "8", "9", "BACK" },
            { "4", "5", "6", "CLEAR" },
            { "1", "2", "3", "" },
            { "0", "00", ",", "ENTER" }
        };

            int btnWidth = 90;
            int btnHeight = 70;
            int startLeft = 20;
            int startTop = 110;
            int gap = 10;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    string keyText = keys[row, col];
                    if (string.IsNullOrEmpty(keyText)) continue;

                    Button btn = new Button();
                    btn.Text = keyText;
                    btn.Left = startLeft + col * (btnWidth + gap);
                    btn.Top = startTop + row * (btnHeight + gap);
                    btn.Width = btnWidth;
                    btn.Height = btnHeight;
                    btn.Font = new Font("Segoe UI", 14F);
                    btn.FlatStyle = FlatStyle.Flat;

                    if (keyText == "ENTER")
                    {
                        btn.Height = btnHeight * 2 + gap; // spans 2 rows like the mockup
                    }

                    btn.Click += KeyButton_Click;
                    this.Controls.Add(btn);

                    if (keyText == "ENTER")
                    {
                        row++; // skip the row it spans into
                    }
                }
            }

            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Left = 20;
            btnOk.Top = 500;
            btnOk.Width = 190;
            btnOk.Height = 45;
            btnOk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnOk.Click += BtnOk_Click;
            this.Controls.Add(btnOk);

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Left = 230;
            btnCancel.Top = 500;
            btnCancel.Width = 190;
            btnCancel.Height = 45;
            btnCancel.Font = new Font("Segoe UI", 11F);
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void KeyButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string key = btn.Text;

            if (key == "BACK")
            {
                if (txtDisplay.Text.Length > 0)
                {
                    txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
                }
            }
            else if (key == "CLEAR")
            {
                txtDisplay.Text = "";
            }
            else if (key == "ENTER")
            {
                BtnOk_Click(sender, e);
            }
            else
            {
                txtDisplay.Text += key;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            ResultValue = txtDisplay.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
