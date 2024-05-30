/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModel;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    /// <summary>
    /// Report form class.
    /// </summary>
    [Export("ReportForm", typeof(IInteractionView))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "The report form contains runtime generated charts and grids controls. This class is maintanable."), Export("ReportForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ReportForm : frmTouchBase, IInteractionView
    { 
        private readonly Size DEFAULTLABELSIZE = new Size(200, 15);
        private readonly Size DEFAULTTEXTBOXSIZE = new Size(222, 12);
        private ReportViewModel viewmodel;
        private Label label;
        private TextBox txtBox;
        private DateTimePicker dtPicker;
        private Chart reportChart1 = new Chart();
        private string invalidArgumentException = ApplicationLocalizer.Language.Translate(100607);
        private string[] colorPalette = new string[] { "#6EAC1C", "#E3F6C9", "#37560D", "#528114", "#C7ED94", "#ABE45F", "#37560D", "#3EBBF0", "#D8F1FC", "#1098D2", "#B1E3F9", "#0A658C", "#8BD6F6", "#074156" };

        /// <summary>
        /// Constructor for this form.
        /// </summary>
        public ReportForm()
        {
            InitializeComponent();            
            this.viewmodel = new ReportViewModel();
            this.bindingSource1.Add(this.viewmodel);
            reportChart1.Width = 715;
            reportChart1.Height = 560;
            reportChart1.Palette = ChartColorPalette.None;                
            this.AssignColorPaletteToChart();
        }

        /// <summary>
        /// OnLoad method for this form.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnLoad(EventArgs e)
        {		
            if (this.viewmodel.GetReports().Count == 0)
            {
                using (frmMessage message = new frmMessage(100605, MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    POSFormsManager.ShowPOSForm(message);
                    this.Close();
                    return;
                }
            }

            this.SetReportList();
            this.TranslateLabels();
            this.reportChart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(this.OnReportChart1_GetToolTipText);
            this.OnGridButtonClick(this.btnGrid, null);

            // Set the current selected report as the first one
            this.PerformReportClick(this.viewmodel.GetReports()[0].ID);
            base.OnLoad(e);
        }

        /// <summary>
        /// OnClosed method for this form.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnClosed(EventArgs e)
        {
            if (this.reportChart1 != null)
            {
                this.reportChart1.GetToolTipText -= new EventHandler<ToolTipEventArgs>(this.OnReportChart1_GetToolTipText);
            }

            base.OnClosed(e);
        }

        /// <summary>
        /// Initialize method for this form.
        /// </summary>
        /// <typeparam name="TArgs">Arguments.</typeparam>
        /// <param name="args">Arguments.</param>
        public void Initialize<TArgs>(TArgs args) where TArgs : Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            return;
        }

        /// <summary>
        /// Get results method for this form.
        /// </summary>
        /// <typeparam name="TResults">Results.</typeparam>
        /// <returns>Results.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new ReportConfirmation
            {
                Success = true,
                Confirmed = true
            } as TResults;
        }

        #region EventHandlers

        /// <summary>
        /// Row click event for report list grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridReportListView_RowClick(object sender, RowClickEventArgs e)
        {
            UpdateNavigationButtons();
            ColumnView view = (ColumnView)this.gridReportList.MainView;
            if (view != null)
            {
                this.PerformReportClick(view.GetFocusedDataRow()[0].ToString());
            }
        }      

        /// <summary>
        /// Click handler for report run button.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event argument.</param>
        private void runReport_Click(object sender, EventArgs e)
        {
            string viewCaptionTxt = string.Empty;
            DataTable outputTable = this.GetCurrentOutputDataTable(out viewCaptionTxt);
            if (outputTable == null)
            {
                return;
            }

            this.gridView1.ViewCaption = viewCaptionTxt;
            this.gridView1.OptionsView.ShowViewCaption = true;
            this.gridControl1.DataSource = outputTable;           
            this.gridControl1.RefreshDataSource();
            foreach (GridColumn column in gridView1.Columns)
            {
                column.SummaryItem.SummaryType = SummaryItemType.None;
                column.BestFit();
                column.Caption = this.viewmodel.GetLocalizedLabel(column.FieldName);                
            }
            
            // Create charts for this report.
            this.CreateReportCharts(outputTable, viewCaptionTxt, this.viewmodel.GetCurrentReportCharts(outputTable));            
        }       

        /// <summary>
        /// Close button event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event argument.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Save click event handler.
        /// </summary>
        /// <param name="sender">Save button.</param>
        /// <param name="e">Event argument.</param>
        public void save_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = null;
            string currentView = this.viewmodel.GetCurrentView();
            try
            {
                saveFileDialog1 = new SaveFileDialog();               
                saveFileDialog1.Title = btnSave.Text;
                if (currentView.Equals("Grid", StringComparison.InvariantCultureIgnoreCase))
                {
                    saveFileDialog1.Filter = "Text files (*.txt)|*.txt|Excel File (*.xlsx)|*.xlsx|PDF (*.pdf)|*.pdf";
                    saveFileDialog1.ShowDialog();
                    if (!string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
                    {
                        using (System.IO.FileStream fs =
                            (System.IO.FileStream)saveFileDialog1.OpenFile())
                        {
                            switch (saveFileDialog1.FilterIndex)
                            {
                                case 1:
                                    this.gridView1.ExportToText(fs);
                                    break;

                                case 2:
                                    this.gridView1.OptionsPrint.AutoWidth = false;
                                    this.gridView1.ExportToXlsx(fs);
                                    break;

                                case 3:
                                    this.gridView1.ExportToPdf(fs);
                                    break;
                            }
                        }
                    }
                }
                else if (currentView.Equals("Chart", StringComparison.InvariantCultureIgnoreCase))
                {
                    saveFileDialog1.Filter = "Png Image (.png)|*.png";
                    saveFileDialog1.ShowDialog();
                    if (!string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
                    {
                        using (System.IO.FileStream fs =
                            (System.IO.FileStream)saveFileDialog1.OpenFile())
                        {
                            this.reportChart1.SaveImage(fs, ChartImageFormat.Png);
                        }
                    }
                }                
            }
            finally
            {
                if (saveFileDialog1 != null)
                {
                    saveFileDialog1.Dispose();
                }
            }
        }

        /// <summary>
        /// Grid btn click event handler.
        /// </summary>
        /// <param name="sender">Grid button.</param>
        /// <param name="e">Event Arguments.</param>
        public void OnGridButtonClick(object sender, System.EventArgs e)
        {
            this.btnGrid.Enabled = false;
            this.btnChart.Enabled = true;
            this.chartContainer.Visible = false;
            this.gridControl1.Visible = true;
            this.viewmodel.SetCurrentView("Grid");
        }

        /// <summary>
        /// Chart btn click event handler.
        /// </summary>
        /// <param name="sender">Chart button.</param>
        /// <param name="e">Event Arguments.</param>
        public void OnChartButtonClick(object sender, System.EventArgs e)
        {
            this.btnGrid.Enabled = true;
            this.btnChart.Enabled = false;
            this.chartContainer.Visible = true;
            this.gridControl1.Visible = false;
            this.viewmodel.SetCurrentView("Chart");
        }

        /// <summary>
        /// Handles custom display text for the columns.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>        
        private void gridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (ReportViewModel.IsNumeric(e.Column.ColumnType))
            {
                decimal decimalValue = Convert.ToDecimal(e.Value);
                e.DisplayText = PosApplication.Instance.Services.Rounding.RoundForDisplay(decimalValue, false, false);
            }
        }        

        /// <summary>
        /// Button up click event.
        /// </summary>
        /// <param name="sender">Up button.</param>
        /// <param name="e">Event arguments.</param>
        private void OnBtnUp_Click(object sender, EventArgs e)
        {
            this.gridReportListView.MovePrev();
        }

        /// <summary>
        /// Button down click event.
        /// </summary>
        /// <param name="sender">Down button.</param>
        /// <param name="e">Event arguments.</param>
        private void OnBtnDown_Click(object sender, EventArgs e)
        {
            this.gridReportListView.MoveNext();
        }

        /// <summary>
        /// When focus row changes on grid view.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnGridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            UpdateNavigationButtons();
            ColumnView view = (ColumnView)this.gridReportList.MainView;
            if (view != null)
            {
                this.PerformReportClick(view.GetFocusedDataRow()[0].ToString());
            }
        }

        /// <summary>
        /// Update navigation buttons.
        /// </summary>
        private void UpdateNavigationButtons()
        {
            btnDown.Enabled = !this.gridReportListView.IsLastRow;
            btnUp.Enabled = !this.gridReportListView.IsFirstRow;
        }

        /// <summary>
        /// Get tooltip text for chart bars.
        /// </summary>
        /// <param name="sender">Report chart.</param>
        /// <param name="e">Tooltip event argument.</param>
        private void OnReportChart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            string tooltipText = ApplicationLocalizer.Language.Translate(100610);

            // Check selected chart element and set tooltip text
            if (e.HitTestResult != null)
            {
                if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
                {
                    int i = e.HitTestResult.PointIndex;
                    if (e.HitTestResult.Series != null && e.HitTestResult.Series.Points != null && e.HitTestResult.Series.Points.Count > 0)
                    {
                        DataPoint dp = e.HitTestResult.Series.Points[i];
                        if (ReportViewModel.IsNumeric(this.gridView1.Columns[e.HitTestResult.Series.Name].ColumnType))
                        {
                            decimal decimalValue = Convert.ToDecimal(dp.YValues[0]);
                            string value = PosApplication.Instance.Services.Rounding.RoundForDisplay(decimalValue, false, false);
                            e.Text = string.Format(tooltipText, dp.AxisLabel, e.HitTestResult.Series.LegendText, value);
                        }
                        else
                        {
                            e.Text = string.Format(tooltipText, dp.AxisLabel, e.HitTestResult.Series.LegendText, dp.YValues[0]);
                        }
                    }
                }
            }
        }        

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Translate labels in this form.
        /// </summary>
        private void TranslateLabels()
        {
            this.btnSave.Text = ApplicationLocalizer.Language.Translate(100602);  //Save button.
            this.btnClose.Text = ApplicationLocalizer.Language.Translate(100604); //Close button.
            this.btnRunReport.Text = ApplicationLocalizer.Language.Translate(100608); //Run button.
            this.lblReportHeader.Text = ApplicationLocalizer.Language.Translate(100601); //Report header label button. 
            this.lblParametersHeader.Text = ApplicationLocalizer.Language.Translate(100609); //Report header label button. 
        }

        /// <summary>
        /// Set column grid for this form.
        /// </summary>
        private void SetColumnsGrid()
        {        
            this.tblReportParameters.Controls.Clear();
            foreach (POSReportParameter parameter in this.viewmodel.GetCurrentReport().Parameters)
            {
                this.label = new Label();
                this.label.Name = parameter.Label;
                this.label.Text = parameter.Label;
                this.label.Size = DEFAULTLABELSIZE;
                this.label.Font = new System.Drawing.Font("Segoe UI", 8F);
                this.label.Margin = new System.Windows.Forms.Padding(1, 5, 1, 5);
                this.tblReportParameters.Controls.Add(this.label);
                switch (parameter.DataType)
                {
                    case POSReportParameterDataType.DateTime:
                        this.dtPicker = new DateTimePicker();
                        this.dtPicker.Name = parameter.Name;
                        this.dtPicker.Size = DEFAULTTEXTBOXSIZE;
                        this.dtPicker.Font = new System.Drawing.Font("Segoe UI", 10F);
                        this.dtPicker.Margin = new System.Windows.Forms.Padding(4, 0, 1, 12);
                        this.dtPicker.Value = parameter.Value == null ? DateTime.Today : System.Convert.ToDateTime(parameter.Value);
                        this.tblReportParameters.Controls.Add(this.dtPicker);
                        break;
                    case POSReportParameterDataType.Integer:
                        int paramInt = 0;
                        this.txtBox = new TextBox();
                        this.txtBox.Name = parameter.Name;
                        this.txtBox.Size = DEFAULTTEXTBOXSIZE;
                        this.txtBox.Font = new System.Drawing.Font("Segoe UI", 10F);
                        this.txtBox.Margin = new System.Windows.Forms.Padding(4, 0, 1, 12);
                        this.txtBox.Text = parameter.Value == null ? paramInt.ToString() : parameter.Value.ToString();
                        this.tblReportParameters.Controls.Add(this.txtBox);
                        break;
                    case POSReportParameterDataType.Decimal:
                        decimal paramDecimal = 0.0m;
                        this.txtBox = new TextBox();
                        this.txtBox.Name = parameter.Name;
                        this.txtBox.Size = DEFAULTTEXTBOXSIZE;
                        this.txtBox.Font = new System.Drawing.Font("Segoe UI", 10F);
                        this.txtBox.Margin = new System.Windows.Forms.Padding(4, 0, 1, 12);
                        this.txtBox.Text = parameter.Value == null ? paramDecimal.ToString() : parameter.Value.ToString();
                        this.tblReportParameters.Controls.Add(this.txtBox);
                        break;
                    default:
                        this.txtBox = new TextBox();
                        this.txtBox.Name = parameter.Name;
                        this.txtBox.Size = DEFAULTTEXTBOXSIZE;
                        this.txtBox.Font = new System.Drawing.Font("Segoe UI", 10F);
                        this.txtBox.Margin = new System.Windows.Forms.Padding(4, 0, 1, 12);
                        this.txtBox.Text = parameter.Value == null ? string.Empty : parameter.Value.ToString();
                        this.tblReportParameters.Controls.Add(this.txtBox);
                        break;
                }
            }
        }

        /// <summary>
        /// Get output set for the current parameters and report.
        /// </summary>
        /// <param name="viewCaptionTxt">Table caption text.</param>
        /// <returns>Output datatable.</returns>
        private DataTable GetCurrentOutputDataTable(out string viewCaptionTxt)
        {
            viewCaptionTxt = this.viewmodel.GetCurrentReport().Title + " ";
            string parameterText = ApplicationLocalizer.Language.Translate(100611);

            foreach (POSReportParameter parameter in this.viewmodel.GetCurrentReport().Parameters)
            {
                bool parsed = false;
                switch (parameter.DataType)
                {
                    case POSReportParameterDataType.DateTime:
                        dtPicker = (DateTimePicker)this.tblReportParameters.Controls.Find(parameter.Name, true)[0];

                        // There is no need to perform parameter check here since dtpicker default value is a valid datetime.
                        parameter.Value = dtPicker.Value;

                        // Display the value according to current culture.
                        viewCaptionTxt += string.Format(parameterText, parameter.Label, dtPicker.Value.ToString("d"));                        
                        break;
                    case POSReportParameterDataType.Decimal:
                        txtBox = (TextBox)this.tblReportParameters.Controls.Find(parameter.Name, true)[0];
                        parameter.Value = txtBox.Text;                     
                        decimal decimalvalue = 0.0m;
                        parsed = decimal.TryParse(parameter.Value.ToString(), out decimalvalue);
                        if (!parsed)
                        {
                            POSFormsManager.ShowPOSErrorDialog(new LSRetailPosis.PosisException(string.Format(invalidArgumentException, parameter.Label)));
                            return null;
                        }

                        parameter.Value = decimalvalue;
                        viewCaptionTxt += string.Format(parameterText, parameter.Label, parameter.Value.ToString());
                        break;
                    case POSReportParameterDataType.Integer:
                        txtBox = (TextBox)this.tblReportParameters.Controls.Find(parameter.Name, true)[0];
                        parameter.Value = txtBox.Text;                     
                        int intValue = 0;
                        parsed = int.TryParse(parameter.Value.ToString(), out intValue);
                        if (!parsed)
                        {
                            POSFormsManager.ShowPOSErrorDialog(new LSRetailPosis.PosisException(string.Format(invalidArgumentException, parameter.Label)));
                            return null;
                        }

                        parameter.Value = intValue;
                        viewCaptionTxt += string.Format(parameterText, parameter.Label, parameter.Value.ToString());
                        break;
                    case POSReportParameterDataType.String:
                        txtBox = (TextBox)this.tblReportParameters.Controls.Find(parameter.Name, true)[0];                        
                        parameter.Value = txtBox.Text;
                        viewCaptionTxt += string.Format(parameterText, parameter.Label, parameter.Value.ToString());
                        break; 
                }
            }
            return this.viewmodel.GetCurrentReport().GetReportDataTable();
        }

        /// <summary>
        /// Add link labels for reports.
        /// </summary>               
        private void SetReportList()
        {
            DataTable reportList = this.viewmodel.GetReportList();

            try
            {
                this.gridReportList.DataSource = reportList;
                this.gridReportList.RefreshDataSource();
                this.gridReportListView.Columns[0].Visible = false;
            }
            finally 
            {
                if (reportList != null)
                {
                    reportList.Dispose();
                }
            }
        }

        /// <summary>
        /// Create charts.
        /// </summary>
        /// <param name="outputdataSet">Output dataset.</param>
        /// <param name="chartCaptionText">Chart caption text.</param>        
        private void CreateReportCharts(DataTable outputTable, string chartCaptionText, Collection<POSReportChart> charts)
        {
            string localColName, firstChartName = string.Empty;
            // Clear all chartareas from this chart.
            this.reportChart1.Titles.Clear();
            this.reportChart1.ChartAreas.Clear();
            this.reportChart1.Legends.Clear();
            this.reportChart1.Series.Clear();
            this.reportChart1.Titles.Add(chartCaptionText);
            this.reportChart1.Titles[0].Font = new System.Drawing.Font("Seoge UI", 8, FontStyle.Bold);            
            int index = 0;            

            if (outputTable != null && outputTable.Rows.Count > 0)
            {
                // Create legends for this chart object.
                this.reportChart1.Legends.Add(new Legend("legend"));                
                this.reportChart1.Legends["legend"].IsDockedInsideChartArea = false;
                this.reportChart1.Legends["legend"].TableStyle = LegendTableStyle.Auto;
                this.reportChart1.Legends["legend"].Docking = Docking.Bottom;
                this.reportChart1.Legends["legend"].Alignment = StringAlignment.Center;
                this.reportChart1.Legends["legend"].Font = new System.Drawing.Font("Seoge UI", 7, System.Drawing.FontStyle.Regular);
                
                // Create chart area for each configured chart.
                foreach (POSReportChart chart in charts)
                {
                    // Create chart area for each chart.
                    string chartName = "Chart" + index.ToString();
                    if (index == 0)
                    {
                        firstChartName = chartName;
                    }

                    this.reportChart1.ChartAreas.Add(chartName);
                    this.reportChart1.ChartAreas[chartName].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;
                    this.reportChart1.ChartAreas[chartName].AxisX.LabelStyle.Font = new System.Drawing.Font("Seoge UI", 7, System.Drawing.FontStyle.Regular);
                    this.reportChart1.ChartAreas[chartName].AxisY.LabelStyle.Font = new System.Drawing.Font("Seoge UI", 7, System.Drawing.FontStyle.Regular);
                    this.reportChart1.ChartAreas[chartName].AxisX.MajorGrid.LineColor = Color.LightGray;                    
                    this.reportChart1.ChartAreas[chartName].AxisY.MajorGrid.LineColor = Color.LightGray;                    
                    this.reportChart1.ChartAreas[chartName].BackColor = System.Drawing.Color.LightGray;
                    this.reportChart1.ChartAreas[chartName].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
                    this.reportChart1.ChartAreas[chartName].BackSecondaryColor = System.Drawing.Color.White;
                    this.reportChart1.ChartAreas[chartName].ShadowColor = System.Drawing.Color.Transparent;                    
                    this.reportChart1.ChartAreas[chartName].AxisY.LabelStyle.Format = "#";
                    if(index != 0)
                    {
                        this.reportChart1.ChartAreas[chartName].AlignWithChartArea = firstChartName;
                        this.reportChart1.ChartAreas[chartName].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                        this.reportChart1.ChartAreas[chartName].AlignmentStyle = AreaAlignmentStyles.All;
                    }

                    // Create series for each chart series.
                    foreach (string seriesName in chart.Series)
                    {
                        localColName = this.viewmodel.GetLocalizedLabel(outputTable.Columns[seriesName].ColumnName);
                        this.reportChart1.Series.Add(seriesName);
                        this.reportChart1.Series[seriesName].IsVisibleInLegend = true;                        
                        this.reportChart1.Series[seriesName].Points
                            .DataBindXY(outputTable.DefaultView, outputTable.Columns[chart.Categories].ColumnName,
                                        outputTable.DefaultView, outputTable.Columns[seriesName].ColumnName);
                        this.reportChart1.Series[seriesName].LegendText = localColName;
                        this.reportChart1.Series[seriesName].ChartArea = chartName;
                        this.reportChart1.Series[seriesName].BorderColor = System.Drawing.Color.Black;
                    }
                    
                    index++;
                }
            }
            
            this.chartContainer.Controls.Clear();
            this.chartContainer.Controls.Add(this.reportChart1);            
        }

        /// <summary>
        /// Set report form according to current selected reportId.
        /// </summary>
        /// <param name="reportId">Current report id.</param>
        private void PerformReportClick(string reportId)
        {
            this.viewmodel.SetCurrentReport(reportId);
            this.gridControl1.DataSource = null;
            this.gridView1.Columns.Clear();
            this.gridView1.OptionsView.ShowViewCaption = false;
            this.gridControl1.RefreshDataSource();
            this.SetColumnsGrid();
            this.btnRunReport.PerformClick();
        }

        /// <summary>
        /// Assign custom color palette to chart.
        /// </summary>
        private void AssignColorPaletteToChart()
        {
            Color[] customColors = new Color[this.colorPalette.Length];
            int index = 0;
            Color customColor;
            foreach (var colorCode in this.colorPalette)
            {
                customColor = ColorTranslator.FromHtml(colorCode);
                customColors[index++] = customColor;
            }

            this.reportChart1.PaletteCustomColors = customColors;
        }
        
        #endregion
    }
}
