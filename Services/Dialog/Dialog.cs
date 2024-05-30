/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Dialog.ViewModels;
using Microsoft.Dynamics.Retail.Pos.Dialog.WinFormsTouch;

namespace Microsoft.Dynamics.Retail.Pos.Dialog
{
    [Export(typeof(IDialog))]
    public class Dialog : IDialog
    {
        /// <summary>
        /// IApplication instance.
        /// </summary>
        private IApplication application;
        private IPosTransaction posTransaction;

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
        [Import]
        public IApplication Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
                InternalApplication = value;
            }
        }

        /// <summary>
        /// Gets or sets the static IApplication instance.
        /// </summary>
        internal static IApplication InternalApplication { get; private set; }

        #region IDialog Members

        /// <summary>
        /// Displays journal dialogue as for touch based or keyboard based.
        /// </summary>
        /// <param name="journalDialogResults"></param>
        /// <param name="dialogResultObject"></param>
        /// <param name="posTransaction"></param>
        public void ShowJournalDialog(ref JournalDialogResults journalDialogResults, ref object dialogResultObject, IPosTransaction posTransaction)
        {
            // Touch based hardware
            using (frmJournal journalDialog = new frmJournal(posTransaction, Application))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(journalDialog);

                journalDialogResults = journalDialog.JournalDialogResults;
                dialogResultObject = journalDialog.JournalDialogResultObject;
            }
        }

        /// <summary>
        /// Returns message as per given message id as DialogResult.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public DialogResult ShowMessage(int messageId)
        {
            using (frmMessage dialog = new frmMessage(messageId))
            {
				POSFormsManager.ShowPOSForm(dialog);

                return dialog.DialogResult;
            }
        }

        /// <summary>
        /// Displays message as per given message id.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="msgBoxButtons"></param>
        /// <returns></returns>
        public DialogResult ShowMessage(int messageId, MessageBoxButtons msgBoxButtons)
        {
            using (frmMessage dialog = new frmMessage(messageId, msgBoxButtons))
            {
				POSFormsManager.ShowPOSForm(dialog);
                return dialog.DialogResult;
            }
        }

        /// <summary>
        /// Displays message as per given message id.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="msgBoxButtons"></param>
        /// <param name="msgBoxIcon"></param>
        /// <returns></returns>
        public DialogResult ShowMessage(int messageId, MessageBoxButtons msgBoxButtons, MessageBoxIcon msgBoxIcon)
        {
            //if (messageId == 56138) // mod by Yonathan 30/05/2023 disable confirmation box for customer order additional shipping information
            //{
            //    return DialogResult.No;
            //}
            //else
            //{
                using (frmMessage dialog = new frmMessage(messageId, msgBoxButtons, msgBoxIcon))
                {
                    POSFormsManager.ShowPOSForm(dialog);
                    return dialog.DialogResult;
                }
            //}
            
        }

        /// <summary>
        /// Displays message as per given message id.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="msgBoxButtons"></param>
        /// <param name="msgBoxIcon"></param>
        /// <returns></returns>
        public DialogResult ShowMessage(string message, MessageBoxButtons msgBoxButtons, MessageBoxIcon msgBoxIcon)
        {
            using (frmMessage dialog = new frmMessage(message, msgBoxButtons, msgBoxIcon))
            {
				POSFormsManager.ShowPOSForm(dialog);
                return dialog.DialogResult;
            }
        }

        /// <summary>
        /// Displays items as per given item id as well as no of rows to be displayed.
        /// </summary>
        /// <param name="howManyRows"></param>
        /// <param name="selectedItemID"></param>
        /// <returns></returns>
        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID)
        {
            return ItemSearch(howManyRows, ref selectedItemID, null);
        }

        /// <summary>
        /// Displays items as per given item id as well as no of rows to be displayed.
        /// </summary>
        /// <param name="howManyRows">Maximum number of rows displayed.</param>
        /// <param name="selectedItemID">Selected item Id.</param>
        /// <param name="categoryId">The product category Id.</param>
        /// <returns></returns>
        public DialogResult ItemSearch(int howManyRows, ref string selectedItemID, string categoryId)
        {
            //using (frmItemSearch itemSearch = new frmItemSearch(howManyRows)) not used temporary - add by Yonathan 4/11/2022
            //{
            //    itemSearch.CategoryId = categoryId;
            //    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(itemSearch);
            //    selectedItemID = itemSearch.SelectedItemId;
            //    return itemSearch.DialogResult;
            //}
            using (frmItemSearch itemSearch = new frmItemSearch(howManyRows, posTransaction))
            {
                itemSearch.CategoryId = categoryId;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(itemSearch);
                selectedItemID = itemSearch.SelectedItemId;
                return itemSearch.DialogResult;
            }
        }

        //custom by Yonathan for Stock positive
        public string ItemSearchCustom(int howManyRows, ref string selectedItemID)//, IPosTransaction thisPosTrans)
        {
            using (frmItemSearch itemSearch = new frmItemSearch(howManyRows, posTransaction))
            {
                //itemSearch.CategoryId = categoryId;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(itemSearch);
                selectedItemID = itemSearch.SelectedItemId;
                return selectedItemID;
            }
        }
        //end

        /// <summary>
        /// Does generic search of the items.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="selectedRow"></param>      
        /// <returns></returns>
        public DialogResult GenericSearch(System.Data.DataTable dataTable, ref System.Data.DataRow selectedRow)
        {
                    return GenericSearch(dataTable,ref selectedRow,null);           
        }

        /// <summary>
        /// Does generic search of the items.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="selectedRow"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public DialogResult GenericSearch(System.Data.DataTable dataTable, ref System.Data.DataRow selectedRow, string title)
        {
            using (frmGenericSearch frmGenericSearch = new frmGenericSearch(dataTable, selectedRow, title))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmGenericSearch);
                selectedRow = frmGenericSearch.SelectedDataRow;
                return frmGenericSearch.DialogResult;
            }
        }

        /// <summary>
        /// Generic lookup.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="displayColumn"></param>
        /// <param name="selectedRow"></param>
        /// <param name="sizeFactor"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DialogResult GenericLookup(System.Data.DataTable dataTable, int displayColumn, ref System.Data.DataRow selectedRow, string defaultValue)
        {
            using (frmGenericLookup frmGenericLookup = new frmGenericLookup(dataTable, displayColumn, defaultValue))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmGenericLookup);
                selectedRow = frmGenericLookup.SelectedDataRow;
                return frmGenericLookup.DialogResult;
            }
        }

        /// <summary>
        /// Generic lookup which shows a list of class instances
        /// </summary>
        /// <param name="dataTable">List of classes whose properties are shown</param>
        /// <param name="displayPropertyName">Name of property which should be shown in the lookup form</param>
        /// <param name="displayColumnCaption">Text label for the column on the lookup</param>
        /// <param name="returnPropertyName">Name of property which should be returned from selected record</param>
        /// <param name="returnPropertyValue">out string of the value in the return property</param>
        /// <param name="sizeFactor"></param>
        /// <param name="defaultValue">default focused row when lookup form opens</param>
        /// <returns>Exit result of the lookup dialog</returns>
        public DialogResult GenericLookup(IList dataTable, string displayPropertyName, string displayColumnCaption, string returnPropertyName, out String returnPropertyValue, string defaultValue)
        {
            using (frmGenericLookup frmGenericLookup = new frmGenericLookup(dataTable, displayPropertyName, displayColumnCaption, returnPropertyName, defaultValue))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmGenericLookup);
                returnPropertyValue = frmGenericLookup.SelectedString;
                return frmGenericLookup.DialogResult;
            }
        }

        /// <summary>
        /// Generic lookup which shows a list of class instances
        /// </summary>
        /// <param name="dataTable">List of classes whose properties are shown</param>
        /// <param name="displayProperties">Display properties which should be shown in the lookup form</param>
        /// <param name="displayHeading">Display heading</param>
        /// <param name="returnPropertyName">Name of property which should be returned from selected record</param>
        /// <param name="returnPropertyValue">out string of the value in the return property</param>
        /// <param name="defaultValue">default focused row when lookup form opens</param>
        /// <returns>Exit result of the lookup dialog</returns> string returnPropertyName, string defaultValue)
        public DialogResult GenericLookup(IList dataTable, OrderedDictionary displayProperties, string displayHeading, string returnPropertyName, out String returnPropertyValue, string defaultValue)
        {
            using (frmGenericLookup frmGenericLookup = new frmGenericLookup(dataTable, displayProperties, displayHeading, returnPropertyName, defaultValue))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmGenericLookup);
                returnPropertyValue = frmGenericLookup.SelectedString;
                return frmGenericLookup.DialogResult;
            }
        }

        /// <summary>
        /// Displays price check in a dialog.
        /// </summary>
        /// <param name="useScanner"></param>
        /// <param name="posTransaction"></param>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public DialogResult PriceCheck(bool useScanner, IPosTransaction posTransaction, ref string inputText)
        {
            DialogResult result = DialogResult.Cancel;
            using (frmPriceCheck frmPriceCheck = new frmPriceCheck(posTransaction))
            {
                frmPriceCheck.UseScanner = useScanner;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmPriceCheck);
                result = frmPriceCheck.DialogResult;
                inputText = frmPriceCheck.Barcode;
            }

            return result;
        }

        /// <summary>
        /// Displays price check in a dialog.
        /// </summary>
        /// <param name="useScanner"></param>
        /// <param name="posTransaction"></param>
        /// <param name="saleItem"></param>
        /// <returns></returns>
        public DialogResult PriceCheck(bool useScanner, IPosTransaction posTransaction, out ISaleLineItem saleItem)
        {
            DialogResult result = DialogResult.Cancel;
            using (frmPriceCheck frmPriceCheck = new frmPriceCheck(posTransaction))
            {
                frmPriceCheck.UseScanner = useScanner;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmPriceCheck);
                result = frmPriceCheck.DialogResult;
                saleItem = frmPriceCheck.SaleItem;
            }

            return result;
        }

        /// <summary>
        /// Displays tender declaration in a dialog box.
        /// </summary>
        /// <param name="posTransaction"></param>
        /// <returns></returns>
        public DialogResult TenderDeclaration(IPosTransaction posTransaction)
        {
            DialogResult result = DialogResult.Cancel;
            using (frmTenderCount frmTenderCount = new frmTenderCount(posTransaction))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frmTenderCount);
                result = frmTenderCount.DialogResult;
            }
            return result;
        }

        /// <summary>
        /// Displays possible tax override codes in a list and returns the selected one
        /// </summary>
        /// <param name="howManyRows">Number of rows to display in list</param>
        /// <param name="overrideBy">Value of TaxOverrideBy enum, telling whether line or transaction overrides should be listed</param>
        /// <param name="selectedTaxGroupId">The selected tax override</param>
        /// <returns></returns>
        public DialogResult TaxOverrideSearch(int howManyRows, int overrideBy, ref string selectedTaxCodeId)
        {
            DialogResult result = DialogResult.Cancel;
            using (frmTaxOverrideSearch taxOverrideSearch = new frmTaxOverrideSearch((TaxOverrideBy)overrideBy, howManyRows))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(taxOverrideSearch);
                selectedTaxCodeId = taxOverrideSearch.SelectedTaxOverrideCode;
                result = taxOverrideSearch.DialogResult;
            }
            return result;
        }

        /// <summary>
        /// Displays inventory lookup form.
        /// </summary>
        /// <param name="posTransaction">Current POS transaction</param>
        /// <returns>Dialog result</returns>
        public DialogResult InventoryLookup(IPosTransaction posTransaction)
        {
            using (formInventoryLookup frm = new formInventoryLookup(posTransaction))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frm);

                return frm.DialogResult;
            }
        }

        /// <summary>
        /// Displays inventory lookup form.
        /// </summary>
        /// <param name="itemOrVariantId">item id or variant id</param>
        /// <returns>Dialog result</returns>
        public DialogResult InventoryLookup(string itemOrVariantId)
        {
            using (formInventoryLookup frm = new formInventoryLookup(itemOrVariantId))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(frm);

                return frm.DialogResult;
            }
        }

        /// <summary>
        /// Searches the specified search term.
        /// </summary>
        /// <param name="searchType">Startup type of the search.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="showUIForEmptyResult">if set to <c>true</c> then show UI even when result is empty.</param>
        /// <returns>
        /// Selected result if found, null otherwise.
        /// </returns>
        public ISearchResult Search(SearchType searchType, string searchTerm, bool showUIForEmptyResult)
        {
            SearchResult result = null;
            SearchViewModel viewModel = new SearchViewModel(searchType, searchTerm);

            viewModel.ExecuteSearch();

            // We will not show the UI for emtpy result if not requested.
            if (showUIForEmptyResult
                || viewModel.Results.Count > 0
                || viewModel.SearchTerms.Length < viewModel.MinimumSearchTermLengh)
            {
                result = new SearchResult();

                using (frmSmartSearch searchForm = new frmSmartSearch(viewModel))
                {
                    POSFormsManager.ShowPOSForm(searchForm);

                    if (searchForm.DialogResult == DialogResult.OK)
                    {
                        result.SearchType = (SearchResultType)viewModel.SelectedResult.SearchType;
                        result.Number = viewModel.SelectedResult.Number;

                        // the Tag field contains the partynumber in the case of customer search.
                        if (result.SearchType == SearchResultType.Customer)
                        {
                            result.Data = viewModel.SelectedResult.Tag.ToString();
                        }
                        else if (result.SearchType == SearchResultType.Item) // If ItemSearch then also include Data result (for Kit products, it will be barcodeInfo) 
                        {
                            result.Data = viewModel.Data;
                        }
                    }
                }
            }

            return result;
        }

        #endregion

    }
}
