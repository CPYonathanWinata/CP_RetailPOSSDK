/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LSRetailPosis.DataAccess.DataUtil;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using LSRetailPosis.POSProcesses;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using System.ComponentModel.Composition;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    /// <summary>
    /// Income and Expense accounts form.
    /// </summary>
    [Export("frmIncomeExpenseAccounts", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmIncomeExpenseAccounts : frmTouchBase, IInteractionView
	{

		private string selectedAccountNum;
		private DataTable accounts;
		private int titleStringId;

		public string SelectedAccountNum
		{
			get { return selectedAccountNum; }
		}

		/// <summary>
		/// Income/expense account dialog.
		/// </summary>
		private frmIncomeExpenseAccounts()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Income/expense account dialog.
		/// </summary>
		/// <param name="titleStringId">String Id for dialog title.</param>
		/// <param name="accounts">Data set.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        [ImportingConstructor]
        public frmIncomeExpenseAccounts(IncomeExpenseAccountsConfirmation incomeExpenseAccountsConfirmation)
			: this()
		{
            this.titleStringId = incomeExpenseAccountsConfirmation.TitleStringId;
            this.accounts = incomeExpenseAccountsConfirmation.IncomeExpenseAccounts;
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!this.DesignMode)
			{
				this.Bounds = new Rectangle(
					LSRetailPosis.Settings.ApplicationSettings.MainWindowLeft,
                    LSRetailPosis.Settings.ApplicationSettings.MainWindowTop,
                    LSRetailPosis.Settings.ApplicationSettings.MainWindowWidth,
                    LSRetailPosis.Settings.ApplicationSettings.MainWindowHeight
					);

				TranslateLabels();
			}

			base.OnLoad(e);
		}

		private void TranslateLabels()
		{
			//
			// Get all text through the Translation function in the ApplicationLocalizer
			//
			// TextID's for frmIncomeExpenseAccounts are reserved at 2730 - 2779
			// In use now are ID's xxxx
			//

			// Translate the buttons...
			this.Text = ApplicationLocalizer.Language.Translate(titleStringId);
			btnSelect.Text = ApplicationLocalizer.Language.Translate(2737); //Select
			btnClose.Text = ApplicationLocalizer.Language.Translate(2738); //Close
			colAccountNum.Caption = ApplicationLocalizer.Language.Translate(2730); //Account number
			colName.Caption = ApplicationLocalizer.Language.Translate(2731); //Account name
			colNameAlias.Caption = ApplicationLocalizer.Language.Translate(2732); //Account alias
            lblHeading.Text = ApplicationLocalizer.Language.Translate(titleStringId); // Income expense accounts
		}


		private void frmIncomeExpenseAccounts_Load(object sender, EventArgs e)
		{
			grIncomeExpenseAccounts.DataSource = accounts;
			if (accounts.Rows.Count <= 0)
			{
				//No records
				using (frmMessage message = new frmMessage(ApplicationLocalizer.Language.Translate(2739), MessageBoxButtons.OK, MessageBoxIcon.Information))
				{
					POSFormsManager.ShowPOSForm(message);
				}
				this.Close();
			}
		}

		private void btnPgUp_Click(object sender, EventArgs e)
		{
			gridView1.MovePrevPage();
		}

		private void btnPgDown_Click(object sender, EventArgs e)
		{
			gridView1.MoveNextPage();
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			gridView1.MovePrev();
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			gridView1.MoveNext();
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			SelectItem();
		}

		private void SelectItem()
		{
			if (gridView1.RowCount > 0)
			{
				System.Data.DataRow Row = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
				selectedAccountNum = (string)Row["ACCOUNTNUM"];

				string line1 = DBUtil.ToStr(Row["MESSAGELINE1"]);
				string line2 = DBUtil.ToStr(Row["MESSAGELINE2"]);
				if ((!string.IsNullOrEmpty(line1)) || (!string.IsNullOrEmpty(line2)))
				{
					PosApplication.Instance.Services.Peripherals.LineDisplay.DisplayText(line1, line2);
				}

				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				Close();
			}
		}

		private void grIncomeExpenseAccounts_DoubleClick(object sender, EventArgs e)
		{
			SelectItem();
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
            return new IncomeExpenseAccountsConfirmation
            {
                SelectedAccountNumber = this.selectedAccountNum,
                Confirmed = this.DialogResult == DialogResult.OK
            } as TResults;
        }

        #endregion
	}
}