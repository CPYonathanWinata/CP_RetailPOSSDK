/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSRetailPosis;
using LSRetailPosis.DataAccess;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction.Line.SaleItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// Print label report view model.
    /// </summary>
    public sealed class PrintLabelReportViewModel : INotifyPropertyChanged
    {
        #region Properties

        private SaleLineItem saleLineItem = null;
        public frmMessage processingDialog = null;

        /// <summary>
        /// Gets or sets date of label report.
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Gets or sets type of label report.
        /// </summary>
        public RetailLabelTypeBase LabelType { get; set; }

        private List<LabelReportItem> items = new List<LabelReportItem>();
        /// <summary>
        /// Gets list of Items for printing.
        /// </summary>
        public ReadOnlyCollection<LabelReportItem> Items
        {
            get
            {
                return this.items.AsReadOnly();
            }
        }

        #endregion

        #region Construction

        internal PrintLabelReportViewModel()
        {
            // Hook up Scanner
            HookUpPeripherals();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Hooks up peripheral devices.
        /// </summary>
        public void HookUpPeripherals()
        {
            // Hook up Scanner
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(ProcessScannedItem);
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent += new ScannerMessageEventHandler(ProcessScannedItem);
        }

        /// <summary>
        /// Unhooks peripheral devices.
        /// </summary>
        public void UnHookPeripherals()
        {
            PosApplication.Instance.Services.Peripherals.Scanner.ScannerMessageEvent -= new ScannerMessageEventHandler(ProcessScannedItem);
        }

        /// <summary>
        /// Adds or updates item to the list.
        /// </summary>
        /// <param name="barcode">Barcode or item id</param>
        public void AddOrUpdateItem(string barcode)
        {
            if (this.GetItemInfo(barcode))
            {
                var currentItem = this.items.SingleOrDefault(i =>
                    this.saleLineItem.ItemId.Equals(i.ItemId, StringComparison.OrdinalIgnoreCase)
                    && (string.IsNullOrEmpty(this.saleLineItem.Dimension.VariantId) || this.saleLineItem.Dimension.VariantId.Equals(i.VariantId, StringComparison.OrdinalIgnoreCase)));

                if (currentItem != null)
                {
                    currentItem.Quantity++;
                }
                else
                {
                    this.items.Add(new LabelReportItem()
                    {
                        ItemId = this.saleLineItem.ItemId,
                        ItemName = this.saleLineItem.Description,
                        VariantId = this.saleLineItem.Dimension.VariantId,
                        Comment = GetColorSizeStyleConfig(),
                        Quantity = 1
                    });

                    this.saleLineItem = null;
                }

                OnPropertyChanged("Items");
            }
        }

        /// <summary>
        /// Enters item quantity.
        /// </summary>
        /// <param name="labelReportItem">Label report item</param>
        /// <param name="quantity">Quantity</param>
        public void EnterItemQuantity(LabelReportItem labelReportItem, int quantity)
        {
            if (labelReportItem != null)
            {
                labelReportItem.Quantity = quantity;
                OnPropertyChanged("Items");
            }
        }

        /// <summary>
        /// Removes selected item from the list.
        /// </summary>
        /// <param name="labelReportItem">Label report item</param>
        public void RemoveItem(LabelReportItem labelReportItem)
        {
            if (labelReportItem != null)
            {
                this.items.Remove(labelReportItem);
                OnPropertyChanged("Items");
            }
        }

        /// <summary>
        /// Opens Item search dialog.
        /// </summary>
        public void OpenItemSearchDialog()
        {
            string selectedItemId = null;

            // Show the search dialog through the item service
            if (PosApplication.Instance.Services.Item.ItemSearch(ref selectedItemId, 500))
            {
                this.AddOrUpdateItem(selectedItemId);
            }
        }

        /// <summary>
        /// Prints label reports.
        /// </summary>
        public void PrintLabelReport()
        {
            if (this.Items.Any()
                && PosApplication.Instance.Services.AxReporting.SetupPrinterSettings(true))
            {
                this.ShowProcessingDialogAsync();
                PosApplication.Instance.Services.AxReporting.PrintLabelReportCompleted += AxReporting_PrintLabelReportCompleted;
                PosApplication.Instance.Services.AxReporting.CallerData = new List<LabelReportItem>(this.Items);
                var itemsToPrint = this.Items.Select(i => PosApplication.Instance.Services.AxReporting.CreateLabelReportItem(i.ItemId, i.VariantId, i.Quantity)).ToList<ILabelReportItem>();
                PosApplication.Instance.Services.AxReporting.PrintLabelReportAsync(this.ReportDate, this.LabelType, SRSReportFileFormat.MHTML, itemsToPrint); 
            }
        }

        /// <summary>
        /// Sets up printer settings.
        /// </summary>
        public void SetupPrinterSettings()
        {
            PosApplication.Instance.Services.AxReporting.SetupPrinterSettings(false);
        }

        /// <summary>
        /// Shows processing dialog.
        /// </summary>
        public void ShowProcessingDialogAsync()
        {
            if (this.processingDialog != null)
            {
                Task.Factory.StartNew(() =>
                {
                    POSFormsManager.ShowPOSForm(processingDialog);
                });
            }
        }

        /// <summary>
        /// Checks print label report task state and result if available.
        /// </summary>
        public void CheckPrintLabelReportTaskResult()
        {
            var isPrintLabelReportTaskStarted = PosApplication.Instance.Services.AxReporting.IsPrintLabelReportTaskStarted();
            if (isPrintLabelReportTaskStarted.HasValue)
            {
                if (isPrintLabelReportTaskStarted == true)
                {
                    this.ShowProcessingDialogAsync();
                    PosApplication.Instance.Services.AxReporting.PrintLabelReportCompleted += AxReporting_PrintLabelReportCompleted;
                }
                else
                {
                    this.ParsePrintLabelReportResponse();
                } 
            }
        }

        /// <summary>
        /// Defines if user has permission to setup label printer.
        /// </summary>
        /// <returns>True if user has permission to setup label printer; false otherwise.</returns>
        public bool IsUserAllowedToSetupLabelPrinter()
        {
            return ApplicationSettings.Terminal.TerminalOperator.AllowSetupLabelPrinter;
        }

        #endregion

        #region Private members

        void AxReporting_PrintLabelReportCompleted(object sender, EventArgs e)
        {
            PosApplication.Instance.Services.AxReporting.PrintLabelReportCompleted -= AxReporting_PrintLabelReportCompleted;
            if ((processingDialog != null) && processingDialog.Visible)
            {
                processingDialog.Close();
                this.ParsePrintLabelReportResponse();
            }
        }

        private void ParsePrintLabelReportResponse()
        {
            var response = PosApplication.Instance.Services.AxReporting.GetPrintLabelReportResult();
            if (response != null)
            {
                this.items = new List<LabelReportItem>(PosApplication.Instance.Services.AxReporting.CallerData as List<LabelReportItem>);
                PosApplication.Instance.Services.AxReporting.CallerData = null;

                this.UpdateItemsReason(response);
                if (!response.Result)
                {
                    if (!string.IsNullOrEmpty(response.InfologContent))
                    {
                        NetTracer.Error(response.InfologContent); 
                    }
                    POSFormsManager.ShowPOSErrorDialog(new PosisException(response.ErrorMessage));
                }

                OnPropertyChanged("Items");
            }
        }

        private bool GetItemInfo(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return false;
            }

            this.saleLineItem = (SaleLineItem)PosApplication.Instance.BusinessLogic.Utility.CreateSaleLineItem();

            IScanInfo scanInfo = PosApplication.Instance.BusinessLogic.Utility.CreateScanInfo();
            scanInfo.ScanDataLabel = barcode;
            scanInfo.EntryType = BarcodeEntryType.ManuallyEntered;

            IBarcodeInfo barcodeInfo = PosApplication.Instance.Services.Barcode.ProcessBarcode(scanInfo);

            if ((barcodeInfo.InternalType == BarcodeInternalType.Item) && (barcodeInfo.ItemId != null))
            {
                // The entry was a barcode which was found and now we have the item id...
                this.saleLineItem.ItemId = barcodeInfo.ItemId;
                this.saleLineItem.BarcodeId = barcodeInfo.BarcodeId;
                this.saleLineItem.SalesOrderUnitOfMeasure = barcodeInfo.UnitId;
                this.saleLineItem.EntryType = barcodeInfo.EntryType;
                this.saleLineItem.Dimension.ColorId = barcodeInfo.InventColorId;
                this.saleLineItem.Dimension.SizeId = barcodeInfo.InventSizeId;
                this.saleLineItem.Dimension.StyleId = barcodeInfo.InventStyleId;
                this.saleLineItem.Dimension.VariantId = barcodeInfo.VariantId;
            }
            else
            {
                // It could be an ItemId
                this.saleLineItem.ItemId = barcodeInfo.BarcodeId;
                this.saleLineItem.EntryType = barcodeInfo.EntryType;
            }

            // fetch all the addtional item properties
            PosApplication.Instance.Services.Item.ProcessItem(saleLineItem, true);

            if (saleLineItem.Found == false)
            {
                POSFormsManager.ShowPOSMessageDialog(2611); // Item not found.
                return false;
            }
            else if ((saleLineItem.Dimension != null)
                && (saleLineItem.Dimension.EnterDimensions || !string.IsNullOrEmpty(saleLineItem.Dimension.VariantId)))
            {
                if (!barcodeInfo.Found)
                {
                    return OpenItemDimensionsDialog(barcodeInfo);
                }
            }

            return true;
        }

        private bool OpenItemDimensionsDialog(IBarcodeInfo barcodeInfo)
        {
            bool returnValue = false;
            // Get the dimensions
            DataTable inventDimCombination = PosApplication.Instance.Services.Dimension.GetDimensions(saleLineItem.ItemId);

            // Get the dimensions
            DimensionConfirmation dimensionConfirmation = new DimensionConfirmation()
            {
                InventDimCombination = inventDimCombination,
                DimensionData = saleLineItem.Dimension,
                DisplayDialog = !barcodeInfo.Found
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(dimensionConfirmation, () =>
            {
                if (dimensionConfirmation.Confirmed)
                {
                    if (dimensionConfirmation.SelectDimCombination != null)
                    {
                        DataRow dr = dimensionConfirmation.SelectDimCombination;
                        saleLineItem.Dimension.VariantId = dr.Field<string>("VARIANTID");
                        saleLineItem.Dimension.ColorId = dr.Field<string>("COLORID");
                        saleLineItem.Dimension.ColorName = dr.Field<string>("COLOR");
                        saleLineItem.Dimension.SizeId = dr.Field<string>("SIZEID");
                        saleLineItem.Dimension.SizeName = dr.Field<string>("SIZE");
                        saleLineItem.Dimension.StyleId = dr.Field<string>("STYLEID");
                        saleLineItem.Dimension.StyleName = dr.Field<string>("STYLE");
                        saleLineItem.Dimension.ConfigId = dr.Field<string>(DataAccessConstants.ConfigId);
                        saleLineItem.Dimension.ConfigName = dr.Field<string>("CONFIG");
                        saleLineItem.Dimension.DistinctProductVariantId = dr.Field<long>("DISTINCTPRODUCTVARIANT");

                        if (string.IsNullOrEmpty(saleLineItem.BarcodeId))
                        {   // Pick up if not previously set
                            saleLineItem.BarcodeId = dr.Field<string>("ITEMBARCODE");
                        }

                        string unitId = dr.Field<string>("UNITID");
                        if (!string.IsNullOrEmpty(unitId))
                        {
                            saleLineItem.SalesOrderUnitOfMeasure = unitId;
                        }
                    }
                    returnValue = true;
                }
            }
            );

            PosApplication.Instance.Services.Interaction.InteractionRequest(request);

            return returnValue;
        }

        private string GetColorSizeStyleConfig()
        {
            string dash = " - ";
            string color = Dimensions.DimensionValue(this.saleLineItem.Dimension.ColorName, this.saleLineItem.Dimension.ColorId);
            string size = Dimensions.DimensionValue(this.saleLineItem.Dimension.SizeName, this.saleLineItem.Dimension.SizeId);
            string style = Dimensions.DimensionValue(this.saleLineItem.Dimension.StyleName, this.saleLineItem.Dimension.StyleId);
            string config = Dimensions.DimensionValue(this.saleLineItem.Dimension.ConfigName, this.saleLineItem.Dimension.ConfigId);
            string[] dimensionValues = { color, size, style, config };

            StringBuilder colorSizeStyleConfig = new StringBuilder();
            for (int i = 0; i < dimensionValues.Length; i++)
            {
                if (dimensionValues[i].Length > 0)
                {
                    if (colorSizeStyleConfig.Length > 0)
                    {
                        colorSizeStyleConfig.Append(dash);
                    }
                    colorSizeStyleConfig.Append(dimensionValues[i]);
                }
            }

            return colorSizeStyleConfig.ToString();
        }

        private void ProcessScannedItem(IScanInfo scanInfo)
        {
            this.AddOrUpdateItem(scanInfo.ScanDataLabel);
        }

        private void UpdateItemsReason(ILabelReportResponse response)
        {
            this.items.ForEach(i =>
            {
                ILabelReportNotPrintedItem notPrintedItem = null;
                if (response.NotPrintedItems != null)
                {
                    notPrintedItem = response.NotPrintedItems.SingleOrDefault(npi =>
                        npi.ItemId.Equals(i.ItemId, StringComparison.OrdinalIgnoreCase)
                        && (string.IsNullOrEmpty(npi.VariantId) || npi.VariantId.Equals(i.VariantId, StringComparison.OrdinalIgnoreCase)));
                }

                if (notPrintedItem != null)
                {
                    i.Reason = notPrintedItem.Reason;
                }
                else if (response.Result)
                {
                    i.Reason = ApplicationLocalizer.Language.Translate(1201); // OK
                }
                else
                {
                    i.Reason = string.Empty;
                }
            });
        }

        #endregion

        #region INotifyPropertyChanged interface

        /// <summary>
        /// Raised when a property on this view model changes it's value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property which value was changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }

    /// <summary>
    /// Label report item class.
    /// </summary>
    public class LabelReportItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string VariantId { get; set; }
        public string Comment { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }
}
