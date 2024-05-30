/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    /// <summary>
    /// Summary description for frmInput.
    /// </summary>
    [Export("InputForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class frmInput : frmTouchBase, IInteractionView
    {
        private InputType currentInputType;
        private string inputText;
        private string promptText;
        private string contentText;
        private PanelControl panelBase;
        private Label lblPrompt;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnOk;
        private LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx btnCancel;
        private DevExpress.XtraEditors.MemoEdit memoKeyboardInput;
        private Label labelTextBox;
        private TextEdit textKeyboardInput;
        private const int SINGLE_LIMIT = 30;
        private Boolean useSingleLine;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public InputType CurrentInputType
        {
            get { return currentInputType; }
            set { currentInputType = value; }
        }

        public string InputText
        {
            get { return inputText; }
            set 
            { 
                memoKeyboardInput.Text = value;
                textKeyboardInput.Text = value;
            }
        }

        public string PromptText
        {
            set { promptText = value; }
            get { return promptText; }
        }

        public frmInput()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
            currentInputType = InputType.Normal;            
        }

        [ImportingConstructor]
        public frmInput(InputConfirmation inputConfirmation)
            : this()
        {
            if (inputConfirmation == null)
            {
                throw new ArgumentNullException("inputConfirmation");
            }

            if (useSingleLine = (inputConfirmation.MaxLength <= SINGLE_LIMIT && inputConfirmation.MaxLength != 0))
            {
                textKeyboardInput.Properties.MaxLength = inputConfirmation.MaxLength;
            }
            else
            {
                memoKeyboardInput.Properties.MaxLength = inputConfirmation.MaxLength;
            }
            
            this.PromptText = inputConfirmation.PromptText;
            this.Text = inputConfirmation.Text;
            this.CurrentInputType = inputConfirmation.InputType;
            this.contentText = inputConfirmation.Title;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (useSingleLine)
                {
                    tableLayoutPanel1.Controls.Add(this.textKeyboardInput, 1, 3);
                    textKeyboardInput.Visible = true;
                    textKeyboardInput.Text = this.Text;
                    textKeyboardInput.Select();
                }
                else
                {
                    tableLayoutPanel1.Controls.Add(this.memoKeyboardInput, 1, 3);
                    memoKeyboardInput.Visible = true;
                    memoKeyboardInput.Text = this.Text;
                    memoKeyboardInput.Select();
                }

                // Get all text through the Translation function in the ApplicationLocalizer
                //
                // TextID's for frmInput are reserved at 1240 - 1259
                // In use now are ID's 1240 - 1243       
                btnOk.Text        = LSRetailPosis.ApplicationLocalizer.Language.Translate(1240); // OK            
                btnCancel.Text    = LSRetailPosis.ApplicationLocalizer.Language.Translate(1241); // Cancel
                labelTextBox.Text = string.IsNullOrEmpty(this.contentText) ?
                                        LSRetailPosis.ApplicationLocalizer.Language.Translate(1243) : // Enter
                                        this.contentText; 

                lblPrompt.Text = this.PromptText;
            }

            base.OnLoad(e);
        }

   
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.panelBase = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.btnCancel = new LSRetailPosis.POSProcesses.WinControls.SimpleButtonEx();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.labelTextBox = new System.Windows.Forms.Label();
            this.memoKeyboardInput = new DevExpress.XtraEditors.MemoEdit();
            this.textKeyboardInput = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).BeginInit();
            this.panelBase.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoKeyboardInput.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textKeyboardInput.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.tableLayoutPanel1);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Size = new System.Drawing.Size(1024, 768);
            this.panelBase.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblPrompt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.memoKeyboardInput, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textKeyboardInput, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(30, 40, 30, 11);
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 764);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel, 3);
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel.Location = new System.Drawing.Point(33, 685);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(954, 65);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOk.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Location = new System.Drawing.Point(346, 4);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 57);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(481, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 57);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            // 
            // lblPrompt
            // 
            this.lblPrompt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPrompt.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPrompt, 3);
            this.lblPrompt.Font = new System.Drawing.Font("Segoe UI Light", 36F);
            this.lblPrompt.Location = new System.Drawing.Point(510, 40);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lblPrompt.Size = new System.Drawing.Size(0, 95);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Tag = "";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTextBox
            // 
            this.labelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTextBox.AutoSize = true;
            this.labelTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelTextBox.Location = new System.Drawing.Point(258, 346);
            this.labelTextBox.Margin = new System.Windows.Forms.Padding(3);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(78, 20);
            this.labelTextBox.TabIndex = 4;
            this.labelTextBox.Text = "Comment";
            // 
            // memoKeyboardInput
            // 
            this.memoKeyboardInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.memoKeyboardInput.Location = new System.Drawing.Point(258, 471);
            this.memoKeyboardInput.Name = "memoKeyboardInput";
            this.memoKeyboardInput.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.memoKeyboardInput.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.memoKeyboardInput.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.memoKeyboardInput.Properties.Appearance.Options.UseBackColor = true;
            this.memoKeyboardInput.Properties.Appearance.Options.UseFont = true;
            this.memoKeyboardInput.Properties.Appearance.Options.UseForeColor = true;
            this.memoKeyboardInput.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.memoKeyboardInput.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoKeyboardInput.Size = new System.Drawing.Size(504, 107);
            this.memoKeyboardInput.TabIndex = 1;
            this.memoKeyboardInput.Visible = false;
            this.memoKeyboardInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyboardInput_KeyDown);
            // 
            // textKeyboardInput
            // 
            this.textKeyboardInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textKeyboardInput.Location = new System.Drawing.Point(258, 223);
            this.textKeyboardInput.Name = "textKeyboardInput";
            this.textKeyboardInput.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.textKeyboardInput.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.textKeyboardInput.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.textKeyboardInput.Properties.Appearance.Options.UseBackColor = true;
            this.textKeyboardInput.Properties.Appearance.Options.UseFont = true;
            this.textKeyboardInput.Properties.Appearance.Options.UseForeColor = true;
            this.textKeyboardInput.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.textKeyboardInput.Size = new System.Drawing.Size(504, 32);
            this.textKeyboardInput.TabIndex = 5;
            this.textKeyboardInput.Visible = false;
            this.textKeyboardInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyboardInput_KeyDown);
            // 
            // frmInput
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelBase);
            this.DoubleBuffered = true;
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "frmInput";
            this.Controls.SetChildIndex(this.panelBase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBase)).EndInit();
            this.panelBase.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoKeyboardInput.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textKeyboardInput.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            String keyboardInput = useSingleLine ? textKeyboardInput.Text : memoKeyboardInput.Text;

            if ((currentInputType == InputType.Email) && (!IsEmail(keyboardInput)))
            {
                this.DialogResult = DialogResult.None;
                using (frmMessage dialog = new frmMessage(ApplicationLocalizer.Language.Translate(1242), MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                }
            }
            else
            {                
                this.DialogResult = DialogResult.OK;
                inputText = keyboardInput;
                Close();                
            }
        }

        private void KeyboardInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnOk_Click(sender, e);
            }
        }

        private static bool IsEmail(string inputEmail)
        {
            // Regex from: http://msdn.microsoft.com/en-us/library/01escwtf.aspx
            string regex = @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";

            return Regex.IsMatch(inputEmail, regex);
        }

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type</typeparam>
        /// <param name="args">Notification</param>
        public void Initialize<TArgs>(TArgs args)
            where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call
        /// </summary>
        /// <typeparam name="TResults"></typeparam>
        /// <returns>Returns the TResults object</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new InputConfirmation
            {
                EnteredText = this.InputText,
                Confirmed = this.DialogResult == DialogResult.OK
            } as TResults;
        }

        #endregion
    }
}