/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch
{
    using System;
    using System.Windows.Forms;

    using DataEntity;
    using LSRetailPosis.POSProcesses;
    
    public partial class frmJournalSearchBrazil : frmTouchBase, ISalesOrderSearchCriteriaHolder
    {
        /// <summary>
        /// Gets or sets the search criteria to be used.
        /// </summary>
        public SalesOrderSearchCriteria SearchCriteria { get; set; }

        /// <summary>
        /// Gives all details from database for search.
        /// </summary>
        public frmJournalSearchBrazil()
        {
            InitializeComponent();
        }

        private void TranslateLabels()
        {
            btnClose.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.Cancel);
            btnClear.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.Clear);
            lblHeader.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.SearchJournal);

            lblFiscalReceiptNumber.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.FiscalReceiptNumber);
            lblReceiptNumber.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.ReceiptNumber);
            lblCPFCNPJ.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.CNPJCPF);
            lblForeignerId.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.ForeignerId);
            lblFiscalDocumentSeries.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.FiscalDocumentSeries);
            lblFiscalDocumentNumber.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.FiscalDocumentNumber);
            btnMoreOptions.Text = LSRetailPosis.ApplicationLocalizer.Language.Translate(Resources.MoreOptions);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.PopulateSearchCriteriaFromFields();
        }

        private void PopulateSearchCriteriaFromFields()
        {
            if (!string.IsNullOrWhiteSpace(this.txtFiscalReceiptNumber.Text))
            {
                this.SearchCriteria.FiscalReceiptNumber = int.Parse(this.txtFiscalReceiptNumber.Text.Trim());   
            }

            if (!string.IsNullOrWhiteSpace(this.txtReceiptNumber.Text))
            {
                this.SearchCriteria.ReceiptNumber = int.Parse(this.txtReceiptNumber.Text);   
            }

            if (!string.IsNullOrWhiteSpace(this.txtCNPJCPF.Text))
            {
                this.SearchCriteria.CnpjCpf = this.txtCNPJCPF.Text.Trim();
            }

            if (!string.IsNullOrWhiteSpace(this.txtForeignerId.Text))
            {
                this.SearchCriteria.ForeignerId = this.txtForeignerId.Text.Trim();
            }

            if (!string.IsNullOrWhiteSpace(this.txtFiscalDocumentSeries.Text))
            {
                this.SearchCriteria.FiscalDocumentSeries = this.txtFiscalDocumentSeries.Text.Trim();
            }

            if (!string.IsNullOrWhiteSpace(this.txtFiscalDocumentNumber.Text))
            {
                this.SearchCriteria.FiscalDocumentNumber = this.txtFiscalDocumentNumber.Text.Trim();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void SetFormFocus()
        {
            txtFiscalReceiptNumber.Select();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtFiscalReceiptNumber.Text = string.Empty;
            this.txtReceiptNumber.Text = string.Empty;
            this.txtCNPJCPF.Text = string.Empty;
            this.txtForeignerId.Text = string.Empty;
            this.txtFiscalDocumentSeries.Text = string.Empty;
            this.txtFiscalDocumentNumber.Text = string.Empty;
            
            SetFormFocus();
        }

        private void btnMoreOptions_Click(object sender, EventArgs e)
        {
            using (var searchJournal = new frmJournalSearch())
            {
                this.PopulateSearchCriteriaFromFields();

                // using the same criteria
                searchJournal.SearchCriteria = this.SearchCriteria;

                POSFormsManager.ShowPOSForm(searchJournal);

                if (searchJournal.DialogResult == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
