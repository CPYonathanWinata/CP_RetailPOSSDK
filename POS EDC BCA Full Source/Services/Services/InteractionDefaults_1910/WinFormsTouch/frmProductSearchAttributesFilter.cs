/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.WinControls;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels;
using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
using DevExpress.Utils;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export("ProductSearchAttributesFilter", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class frmProductSearchAttributesFilter : frmTouchBase, IInteractionView
    {
        private static int SCROLL_SIZE = SystemInformation.VerticalScrollBarWidth;

        private readonly Size DEFAULTATTRIBUTEVALUEBUTTONSIZE = new Size(130, 70);
        private readonly Size DEFAULTATTRIBUTEBUTTONSIZE = new Size(140, 70);
        private readonly Size DEFAULTINPUTSIZE = new Size(400, 70);

        private List<CheckButton> attributesButtons;
        private List<Control> attributeFiltersControls;

        private ProductSearchAttributesFilterViewModel viewModel;

        [ImportingConstructor]
        public frmProductSearchAttributesFilter(ProductSearchAttributesFilterConfirmation args)
            : this()
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (args.SearchFilter == null || !(args.SearchFilter is IProductSearchFilter))
            {
                throw new ArgumentNullException("SearchFilter");
            }

            var searchFilter = args.SearchFilter as IProductSearchFilter;
            var searchFilterCache = args.SearchFilterCache == null ? null : args.SearchFilterCache as IProductSearchFilter;
            viewModel = new ProductSearchAttributesFilterViewModel(searchFilter, searchFilterCache, args.CategoryId);
            attributeFiltersControls = new List<Control>();
        }

        private frmProductSearchAttributesFilter()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            viewModel.Initialize();
            
            if (!LoadAttributeButtons())
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSMessageDialog(107109);
                this.Close();
            }

            if (!this.DesignMode)
            {
                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {
            //            
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmDimensions are reserved at 107101 - 107150
            // In use now are ID's 107108
            //
            this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(107101);  //Select search options
            btnOK.Text = ApplicationLocalizer.Language.Translate(1201);  // Ok
            btnCancel.Text = ApplicationLocalizer.Language.Translate(56134); //Cancel            
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private CheckButton CreateCheckButton(string text, object tag)
        {
            CheckButton button = new CheckButton();
            button.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.Appearance.Options.UseFont = true;
            button.Appearance.TextOptions.WordWrap = WordWrap.Wrap;            
            button.Text = text; // If the description field is empty, use the ID field
            button.Tag = tag;
            button.Visible = false;
            return button;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private CheckButton CreateAttributeButton(string text, object tag)
        {
            var button = CreateCheckButton(text, tag);
            button.Size = DEFAULTATTRIBUTEBUTTONSIZE;
            button.Click += new EventHandler(OnAttributeButton_Click);
            return button;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private CheckButton CreateAttributeValueButton(string text, object tag)
        {
            var button = CreateCheckButton(text, tag);
            button.Size = DEFAULTATTRIBUTEVALUEBUTTONSIZE;
            button.DataBindings.Add(new Binding("Checked", tag, "ValueChecked", false, DataSourceUpdateMode.OnPropertyChanged));            
            button.CheckedChanged += new EventHandler(OnAttributeValueButtonCheck_CheckedChanged);
            return button;
        }

        private void OnAttributeValueButtonCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton clickedButton = sender as CheckButton;
            if (clickedButton != null)
            {
                if (clickedButton.Checked)
                {
                    clickedButton.ForeColor = SystemColors.GrayText;
                }
                else
                {
                    clickedButton.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void UpdateAttributesButtonsPropertiesOnClick(CheckButton clickedButton)
        {
            if (clickedButton != null)
            {
                // Disable the selected button and enable the rest                
                this.attributesButtons.Where(btn => btn.Enabled == false && btn != clickedButton).ToList().ForEach(btn => { btn.Enabled = true; btn.Checked = false; });
                clickedButton.Enabled = false;
            }
        }

        private void OnAttributeButton_Click(object sender, EventArgs e)
        {
            CheckButton clickedButton = sender as CheckButton;
            if (clickedButton != null)
            {
                this.SuspendLayout();
                UpdateAttributesButtonsPropertiesOnClick(clickedButton);
                this.attributesValuesPanel.Focus();
                ShowAttributesValues(clickedButton.Tag);
                this.ResumeLayout();
            }
        }

        private void ShowAttributesValues(object tag)
        {
            var attributeElement = tag as AttributeElement;
            this.viewModel.LoadAttributeFilter(attributeElement);
            if (!IsFilterAddedToForm(attributeElement.Filter))
            {
                CreateControlsForAttributeFilter(attributeElement);
            }
            HideAttributesFilters();
            ShowAttributeFilter(attributeElement.Filter);
            SetFocusOnAttributeFilterValue(attributeElement.Filter);
        }

        private void SetFocusOnAttributeFilterValue(AttributeFilter attributeFilter)
        {
            object objectToSetFocus = null;
            switch(attributeFilter.AttributeFilterType)
            {
                case AttributeFilterType.Range:
                    objectToSetFocus = attributeFilter.RangeFrom;
                    break;
                case AttributeFilterType.Text:
                    objectToSetFocus = attributeFilter.FilterValue;
                    break;
                case AttributeFilterType.ValueList:
                    objectToSetFocus = attributeFilter.AttributeElementValueList.FirstOrDefault();
                    break;
                case AttributeFilterType.YesNoOptions:
                    objectToSetFocus = attributeFilter.TrueValue;
                    break;
                default:
                    break;
            }
            if (objectToSetFocus != null)
            {
                attributeFiltersControls.Where(ctrl => ctrl.Tag == objectToSetFocus).First().Focus();
            }
            else
            {
                this.attributesValuesPanel.Focus();
            }
        }

        private void HideAttributesFilters()
        {
            attributeFiltersControls.Where(ctrl => ctrl.Visible == true).ToList().ForEach(ctrl => ctrl.Visible = false);
        }
        private void ShowAttributeFilter(AttributeFilter attributeFilter)
        {
            List<object> objectListToShow = this.viewModel.GetAllFiltersObjects(attributeFilter);
            foreach (var objectToShow in objectListToShow)
            {
                attributeFiltersControls.Where(ctrl => ctrl.Tag == objectToShow).ToList().ForEach(ctrl => ctrl.Visible = true);
            }
        }

        private void CreateControlsForAttributeFilter(AttributeElement attributeElement)
        {
            switch (attributeElement.Filter.AttributeFilterType)
            {
                case AttributeFilterType.Range:
                    CreateRangeControls(attributeElement);
                    break;
                case AttributeFilterType.Text:
                    CreateInputTextFilter(attributeElement.Filter);
                    break;
                case AttributeFilterType.YesNoOptions:
                    CreateYesNoControls(attributeElement);
                    break;
                case AttributeFilterType.ValueList:
                    CreateAttributeFilterValueList(attributeElement);
                    break;
                default:
                    break;
            }
        }

        private void CreateAttributeFilterValueList(AttributeElement attributeElement)
        {
            var addedControls = new List<Control>();
            attributeElement.Filter.AttributeElementValueList.ForEach(attributeValue => addedControls.Add(CreateAttributeValueButton((string)attributeValue.Value, attributeValue)));
            attributeFiltersControls.AddRange(addedControls);
            attributesValuesPanel.Controls.AddRange(addedControls.ToArray());
        }

        private void CreateYesNoControls(AttributeElement attributeElement)
        {
            var addedControls = new List<Control>();
            addedControls.Add(CreateAttributeValueButton((string)attributeElement.Filter.TrueValue.Value, attributeElement.Filter.TrueValue));
            addedControls.Add(CreateAttributeValueButton((string)attributeElement.Filter.FalseValue.Value, attributeElement.Filter.FalseValue));
            attributeFiltersControls.AddRange(addedControls);
            attributesValuesPanel.Controls.AddRange(addedControls.ToArray());
        }

        private void CreateRangeControls(AttributeElement attributeElement)
        {
            TableLayoutPanel layoutControl = null;
            switch (attributeElement.DataType)
            {
                case AttributeDataType.DateTime:
                    layoutControl = CreateDateRangeLayoutWithControls(attributeElement.Filter);
                    break;
                case AttributeDataType.Integer:
                    layoutControl = CreateInputRangeLayoutWithControls(attributeElement.Filter, StringPadEntryTypes.Integer);
                    break;
                case AttributeDataType.Currency:
                    layoutControl = CreateInputRangeLayoutWithControls(attributeElement.Filter, StringPadEntryTypes.Price);
                    break;
                case AttributeDataType.Decimal:
                    layoutControl = CreateInputRangeLayoutWithControls(attributeElement.Filter, StringPadEntryTypes.Quantity);
                    break;
                default:
                    break;
            }
            attributeFiltersControls.Add(layoutControl);
            foreach (Control control in layoutControl.Controls)
            {
                attributeFiltersControls.Add(control);
            }
            this.attributesValuesPanel.Controls.Add(layoutControl);
        }

        private bool IsFilterAddedToForm(AttributeFilter attributeFilter)
        {
            bool retVal = false;
            List<object> objectListToCheck = this.viewModel.GetAllFiltersObjects(attributeFilter);
            foreach (var objectToCheck in objectListToCheck)
            {
                if (attributeFiltersControls.Count(ctrl => ctrl.Tag == objectToCheck) != 0)
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }

        private void CreateAttributeButtonList()
        {
            attributesButtons = new List<CheckButton>(viewModel.AttributesElementList.Count);

            foreach (var attribute in viewModel.AttributesElementList)
            {
                var button = CreateAttributeButton(attribute.Name, attribute);
                button.Visible = true;
                attributesButtons.Add(button);
            }
        }

        private bool LoadAttributeButtons()
        {
            CreateAttributeButtonList();
            bool hasButtons = attributesButtons != null && attributesButtons.Count > 0;
            if (hasButtons)
            {
                MakeButtonWidthsUniform(attributesButtons);

                this.SuspendLayout();
                this.attributesPanel.Controls.AddRange(attributesButtons.ToArray());

                this.attributesPanel.VerticalScroll.SmallChange = this.attributesPanel.Controls[0].Height + this.attributesPanel.Margin.Top + this.attributesPanel.Margin.Bottom;

                this.ResumeLayout(true);
                this.attributesPanel.Controls[0].PerformLayout();
                attributesButtons[0].PerformClick();
            }

            return hasButtons;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private Control CreateDateInputControl(string text, object tag)
        {
            var attributevalue = tag as AttributeElementValue;
            var dateInput = new DateEdit();
            dateInput.Tag = tag;
            dateInput.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dateInput.Size = DEFAULTINPUTSIZE;
            dateInput.Visible = false;
            dateInput.DataBindings.Add(new Binding("EditValue", tag, "Value", true));
            dateInput.DataBindings["EditValue"].Format += OnDateTimeFormat;
            return dateInput;
        }

        private void OnDateTimeFormat(object sender, ConvertEventArgs e)
        {
            if (e.Value == null || (DateTime)e.Value == DateTime.MinValue)
            {
                e.Value = string.Empty;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private Control CreateInputControl(string text, object tag, Microsoft.Dynamics.Retail.Pos.Contracts.UI.StringPadEntryTypes enterType)
        {
            var attributeValue = tag as AttributeElementValue;
            var stringPadControl = new StringPad();
            stringPadControl.Tag = tag;
            stringPadControl.EnteredValue = (string)attributeValue.Value;
            stringPadControl.EntryType = enterType;
            stringPadControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            stringPadControl.Size = DEFAULTINPUTSIZE;
            stringPadControl.NegativeMode = false;
            stringPadControl.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            stringPadControl.PromptText = text;
            stringPadControl.Visible = false;
            stringPadControl.DataBindings.Add(new Binding("EnteredValue", tag, "Value", true));
            return stringPadControl;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private Control CreateLabel(string text, object tag)
        {
            var label = new Label();
            label.Tag = tag;
            label.Text = text;
            label.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.Visible = false;
            return label;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
        private TableLayoutPanel CreateLayoutForRange(object tag)
        {

            var layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            layoutPanel.RowCount = 2;
            layoutPanel.ColumnCount = 2;
            layoutPanel.Tag = tag;
            layoutPanel.AutoSize = true;
            return layoutPanel;
        }

        private TableLayoutPanel CreateDateRangeLayoutWithControls(AttributeFilter attributeFilter)
        {
            var textFrom = ApplicationLocalizer.Language.Translate(107106); //From
            var textTo = ApplicationLocalizer.Language.Translate(107107); //To
            var addedRangeFromControl = CreateDateInputControl(textFrom, attributeFilter.RangeFrom);
            addedRangeFromControl.DataBindings["EditValue"].Parse += OnRangeFromValuePass;
            var addedRangeToControl = CreateDateInputControl(textTo, attributeFilter.RangeTo);
            addedRangeToControl.DataBindings["EditValue"].Parse += OnRangeToValuePass;

            var layoutPanel = CreateLayoutForRange(attributeFilter);
            layoutPanel.RowCount = 5;
            layoutPanel.Controls.Add(CreateLabel(textFrom, attributeFilter), 0, 0);
            layoutPanel.Controls.Add(addedRangeFromControl, 0, 1);
            layoutPanel.Controls.Add(CreateLabel(string.Empty, attributeFilter), 0, 2);
            layoutPanel.Controls.Add(CreateLabel(textTo, attributeFilter), 0, 3);
            layoutPanel.Controls.Add(addedRangeToControl, 0, 4);
            return layoutPanel;
        }

        private TableLayoutPanel CreateInputRangeLayoutWithControls(AttributeFilter attributeFilter, Contracts.UI.StringPadEntryTypes stringPadEntryTypes)
        {
            var addedRangeFromControl = CreateInputControl(ApplicationLocalizer.Language.Translate(107106) /* From */, attributeFilter.RangeFrom, stringPadEntryTypes);
            addedRangeFromControl.DataBindings["EnteredValue"].Parse += OnRangeFromValuePass;
            var addedRangeToControl = CreateInputControl(ApplicationLocalizer.Language.Translate(107107) /* To */, attributeFilter.RangeTo, stringPadEntryTypes);
            addedRangeToControl.DataBindings["EnteredValue"].Parse += OnRangeToValuePass;

            var layoutPanel = CreateLayoutForRange(attributeFilter);
            layoutPanel.Controls.Add(addedRangeFromControl, 0, 0);
            layoutPanel.Controls.Add(addedRangeToControl, 0, 1);
            return layoutPanel;
        }

        private void OnRangeToValuePass(object sender, ConvertEventArgs e)
        {
            var errMessage = string.Empty;
            AttributeElementValue attributeValueTo = (AttributeElementValue)((Binding)sender).DataSource;
            if (!this.viewModel.ValidateRangeToValue(attributeValueTo, e.Value, out errMessage))
            {          
                e.Value = attributeValueTo.Value;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSErrorDialog(new PosisException(errMessage));
            }
        }

        private void OnRangeFromValuePass(object sender, ConvertEventArgs e)
        {
            var errMessage = string.Empty;
            AttributeElementValue attributeValueFrom = (AttributeElementValue)((Binding)sender).DataSource;
            if (!this.viewModel.ValidateRangeFromValue(attributeValueFrom, e.Value, out errMessage))
            {
                e.Value = attributeValueFrom.Value;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSErrorDialog(new PosisException(errMessage));
            }
        }

        private void CreateInputTextFilter(AttributeFilter attributeFilter)
        {
            var addedControl = CreateInputControl(ApplicationLocalizer.Language.Translate(107103) /* Search text */, attributeFilter.FilterValue, Contracts.UI.StringPadEntryTypes.AlphaNumeric);
            addedControl.DataBindings["EnteredValue"].Parse += OneSearchTextBindingParse;
            this.attributeFiltersControls.Add(addedControl);
            attributesValuesPanel.Controls.Add(addedControl);
        }

        private void OneSearchTextBindingParse(object sender, ConvertEventArgs e)
        {
            var errMessage = string.Empty;
            if (!this.viewModel.ValidTextFilter(e.Value.ToString(), out errMessage))
            {
                e.Value = ((AttributeElementValue)((Binding)sender).DataSource).Value;
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSErrorDialog(new PosisException(errMessage));
            }
        }

        private static void MakeButtonWidthsUniform(List<CheckButton> buttonCollection)
        {
            // Find the widest button in the collection
            int maxWidth = buttonCollection.Max(btn => btn.Width);

            // Update all buttons in the collection to match
            buttonCollection.ForEach(btn => btn.Width = maxWidth);
        }

        private void ParentPanel_Resize(object sender, EventArgs e)
        {
            this.ResetAttributesValuesPanelSize();
        }

        private void ResetAttributesPanelSize()
        {
            // Hide the horizontal scroll bar if necessary.
            if (this.attributesPanel.HorizontalScroll.Visible)
            {
                this.attributesPanel.Height = this.parentAttributesPanel.Height + SCROLL_SIZE;
            }
            else
            {
                this.attributesPanel.Height = this.parentAttributesPanel.Height;
            }

            // Hide the vertical scroll bar if necessary.
            if (this.attributesPanel.VerticalScroll.Visible)
            {
                this.attributesPanel.Width = this.parentAttributesPanel.Width + SCROLL_SIZE;
            }
            else
            {
                this.attributesPanel.Width = this.parentAttributesPanel.Width;
            }
            this.btnAttributesListDown.Enabled = this.attributesPanel.VerticalScroll.Visible;
            this.btnAttributesListUp.Enabled = this.attributesPanel.VerticalScroll.Visible;
        }

        private void ResetAttributesValuesPanelSize()
        {
            // Hide the horizontal scroll bar if necessary.
            if (this.attributesValuesPanel.HorizontalScroll.Visible)
            {
                this.attributesValuesPanel.Height = this.parentAttributesValuesPanel.Height + SCROLL_SIZE;
            }
            else
            {
                this.attributesValuesPanel.Height = this.parentAttributesValuesPanel.Height;
            }

            // Hide the vertical scroll bar if necessary.
            if (this.attributesValuesPanel.VerticalScroll.Visible)
            {
                this.attributesValuesPanel.Width = this.parentAttributesValuesPanel.Width + SCROLL_SIZE;
            }
            else
            {
                this.attributesValuesPanel.Width = this.parentAttributesValuesPanel.Width;
            }

            this.btnPageDown.Enabled = this.attributesValuesPanel.VerticalScroll.Visible;
            this.btnDown.Enabled = this.attributesValuesPanel.VerticalScroll.Visible;
            this.btnUp.Enabled = this.attributesValuesPanel.VerticalScroll.Visible; ;
            this.btnPageUp.Enabled = this.attributesValuesPanel.VerticalScroll.Visible;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.attributesValuesPanel.VerticalScroll;
            int step = v_prop.Value;
            step += v_prop.SmallChange;

            if (step >= v_prop.Maximum)
            {
                step = v_prop.Maximum;             
            }
            
            this.attributesValuesPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.attributesValuesPanel.VerticalScroll;
            int step = v_prop.Value;
            step += v_prop.LargeChange;

            if (step >= v_prop.Maximum)
            {
                step = v_prop.Maximum;                
            }            

            this.attributesValuesPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.attributesValuesPanel.VerticalScroll;
            int step = v_prop.Value;
            step -= v_prop.SmallChange;

            if (step <= v_prop.Minimum)
            {
                step = v_prop.Minimum;
            }

            this.attributesValuesPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.attributesValuesPanel.VerticalScroll;
            int step = v_prop.Value;
            step -= v_prop.LargeChange;

            if (step <= v_prop.Minimum)
            {
                step = v_prop.Minimum;
            }

            this.attributesValuesPanel.AutoScrollPosition = new Point(0, step);
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
            return new ProductSearchAttributesFilterConfirmation
            {
                Confirmed = this.DialogResult == DialogResult.OK,
                SearchFilter = this.viewModel.SaveSearchFilter(),
                SearchFilterCache = this.viewModel.SaveSearchAttrbutesCache()
            } as TResults;
        }

        #endregion

        private void btnAttributesListDown_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.attributesPanel.VerticalScroll;
            int step = v_prop.Value;
            step += v_prop.SmallChange;

            if (step >= v_prop.Maximum)
            {
                step = v_prop.Maximum;
            }

            this.attributesPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnAttributesListUp_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.attributesPanel.VerticalScroll;
            int step = v_prop.Value;
            step -= v_prop.SmallChange;

            if (step <= v_prop.Minimum)
            {
                step = v_prop.Minimum;
            }

            this.attributesPanel.AutoScrollPosition = new Point(0, step);
        }

        private void parentAttributesPanel_Resize(object sender, EventArgs e)
        {
            this.ResetAttributesPanelSize();
        }

        private void attributesValuesPanel_Resize(object sender, EventArgs e)
        {
            this.ResetAttributesValuesPanelSize();
        }

        private void attributesPanel_Resize(object sender, EventArgs e)
        {
            this.ResetAttributesPanelSize();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}