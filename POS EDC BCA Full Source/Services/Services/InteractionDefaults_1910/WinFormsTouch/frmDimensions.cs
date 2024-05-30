/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.POSProcesses.WinControls;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
	[Export("DimensionView", typeof(IInteractionView))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class frmDimensions : frmTouchBase, IInteractionView
	{
        private static int SCROLL_SIZE = SystemInformation.VerticalScrollBarWidth;

        private DataTable inventDimCombination;
		private DataRow selectDimCombination;
		private string colorId = string.Empty;
		private string sizeId = string.Empty;
		private string styleId = string.Empty;
		private string configId = string.Empty;

		private const string CURRENTDIMENSION_PROPERTYNAME = "CurrentDimension";
		private const int ARROWCOLUMN = 1;
		private readonly Size DEFAULTVARIANTSIZE = new Size(130, 70);

		private List<Control> colorButtons;     // Collection of color variants
		private List<Control> sizeButtons;      // Collection of size variants
		private List<Control> styleButtons;     // Collection of style variants
		private List<Control> configButtons;    // Collection of config variants

        private bool colorSelected;
        private bool sizeSelected;
        private bool styleSelected;
        private bool configSelected;

		private int? CurrentArrowRow;
		private DimensionsViewModel viewModel;

		public DataRow SelectDimCombination
		{
			get { return selectDimCombination; }
		}

		[ImportingConstructor]
		public frmDimensions(DimensionConfirmation args)
			: this()
		{
			if (args == null)
				throw new ArgumentNullException("args");

			this.colorId = ((IDimension)args.DimensionData).ColorId;
			this.sizeId = ((IDimension)args.DimensionData).SizeId;
			this.styleId = ((IDimension)args.DimensionData).StyleId;
			this.configId = ((IDimension)args.DimensionData).ConfigId;

			this.inventDimCombination = args.InventDimCombination;
			viewModel = new DimensionsViewModel(this.inventDimCombination);
			viewModel.PropertyChanged += new PropertyChangedEventHandler(OnViewModel_PropertyChanged);
			viewModel.Finished += new EventHandler(OnViewModel_Finished);

			LoadButtons();
		}

		private frmDimensions()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!this.DesignMode)
			{
				TranslateLabels();

				// Load the first dimension's variants
				GetDimensionButton(viewModel.CurrentDimension).PerformClick();
			}

			base.OnLoad(e);
		}

		private void OnViewModel_Finished(object sender, EventArgs e)
		{
			Finished();
		}

		/// <summary>
		/// Handler that listens for properties on the view model to change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				// If the current dimension changes on the view model, click the corresponding button
				case CURRENTDIMENSION_PROPERTYNAME:
					GetDimensionButton(viewModel.CurrentDimension).PerformClick();
					break;
				default:
					break;
			}
		}

		public bool PendingOptions()
		{
			selectDimCombination = SetResult();
			return selectDimCombination == null;
		}

		private void TranslateLabels()
		{
			//
			// Get all text through the Translation function in the ApplicationLocalizer
			//
			// TextID's for frmDimensions are reserved at 1860 - 1879
			// In use now are ID's 1860 - 1863
			//
			this.Text = lblHeader.Text = ApplicationLocalizer.Language.Translate(1860); // Select product options  
			colorButton.Text = ApplicationLocalizer.Language.Translate(1861); // "Color"            
			sizeButton.Text = ApplicationLocalizer.Language.Translate(1862); // "Size"
			styleButton.Text = ApplicationLocalizer.Language.Translate(1863); // "Style"
			configButton.Text = ApplicationLocalizer.Language.Translate(1867); // "Configuration"            
			btnCancel.Text = ApplicationLocalizer.Language.Translate(56134); //Cancel            
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is responsible for disposing returned object")]
		private Control CreateVariantButton(string text, string tag)
		{
			SimpleButtonEx button = new SimpleButtonEx();
			button.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			button.Appearance.Options.UseFont = true;
			button.Click += new EventHandler(OnVariantButton_Click);
			button.Size = DEFAULTVARIANTSIZE;
			button.Text = string.IsNullOrWhiteSpace(text) ? tag : text; // If the description field is empty, use the ID field
			button.Tag = tag;
			button.Visible = false;
			button.Name = tag;   // Set for automation purposes

			return button;
		}

		/// <summary>
		/// Handler called when variant choice is made for a dimension
		/// </summary>        
		private void OnVariantButton_Click(object sender, EventArgs e)
		{
			Control clickedButton = sender as Control;
			if (clickedButton != null)
			{
				switch (viewModel.CurrentDimension)
				{
					case DimensionType.Color:
						// Disable the selected button and enable the rest
						colorButtons.ForEach(btn => btn.Enabled = (btn != clickedButton));

						// Change the dimension button text to the selected value
						colorButton.Text = clickedButton.Text;

						// Change the dimension button to checked state to indicate selected. We force the buttons to paint so the user gets the visual indication of
						// selection before the form closes (if this is the last dimension)
                        colorSelected = true;
                        colorButton.Checked = true;
						colorButton.Refresh();
						break;

					case DimensionType.Size:
						sizeButtons.ForEach(btn => btn.Enabled = (btn != clickedButton));

						sizeButton.Text = clickedButton.Text;
                        sizeSelected = true;
                        sizeButton.Checked = true;
						sizeButton.Refresh();
						break;

					case DimensionType.Style:
						styleButtons.ForEach(btn => btn.Enabled = (btn != clickedButton));

						styleButton.Text = clickedButton.Text;
                        styleSelected = true;
                        styleButton.Checked = true;
						styleButton.Refresh();
						break;

					case DimensionType.Config:
						configButtons.ForEach(btn => btn.Enabled = (btn != clickedButton));

						configButton.Text = clickedButton.Text;
                        configSelected = true;
                        configButton.Checked = true;
						configButton.Refresh();
						break;

					default:
						break;
				}

				// Update the view model with the selected ID. If this is the last one, the finished event will fire
				viewModel.SetSelectedId(clickedButton.Tag.ToString());
			}
		}

		private void LoadButtons()
		{
			int lastUsedDimensionButtonRow = 1;

			// Color
			if (colorButtons == null && viewModel.Exists(DimensionType.Color))
			{
				List<DimensionValue> colors = viewModel.AllVariants.Where(dim => dim.Dimension == DimensionType.Color).ToList();
				colors.Sort(DimensionValue.CompareDisplayOrders);
				colorButtons = new List<Control>(colors.Count);

				// Colors            
				foreach (var color in colors)
				{
					var button = CreateVariantButton(color.Name, color.Id);
					colorButtons.Add(button);
				}

				this.colorButton.Visible = true;

				// Update the row of the color dimension button so visible buttons will be top stacked
				this.tableLayoutPanel.SetRow(this.colorButton, lastUsedDimensionButtonRow++);

				// Add the color variants to the flow panel
				MakeButtonWidthsUniform(colorButtons);
                this.dimensionsPanel.Controls.AddRange(colorButtons.ToArray());
			}

			// Size
			if (sizeButtons == null && viewModel.Exists(DimensionType.Size))
			{
				List<DimensionValue> sizes = viewModel.AllVariants.Where(dim => dim.Dimension == DimensionType.Size).ToList();
				sizes.Sort(DimensionValue.CompareDisplayOrders);
				sizeButtons = new List<Control>(sizes.Count);

				// Sizes           
				foreach (var size in sizes)
				{
					var button = CreateVariantButton(size.Name, size.Id);
					sizeButtons.Add(button);
				}

				this.sizeButton.Visible = true;

				// Update the row of the size dimension button so visible buttons will be top stacked
				this.tableLayoutPanel.SetRow(this.sizeButton, lastUsedDimensionButtonRow++);

				// Add the size variants to the flow panel
				MakeButtonWidthsUniform(sizeButtons);
                this.dimensionsPanel.Controls.AddRange(sizeButtons.ToArray());
			}

			// Style
			if (styleButtons == null && viewModel.Exists(DimensionType.Style))
			{
				List<DimensionValue> styles = viewModel.AllVariants.Where(dim => dim.Dimension == DimensionType.Style).ToList();
				styles.Sort(DimensionValue.CompareDisplayOrders);
				styleButtons = new List<Control>(styles.Count);

				// Styles           
				foreach (var style in styles)
				{
					var button = CreateVariantButton(style.Name, style.Id);
					styleButtons.Add(button);
				}

				this.styleButton.Visible = true;

				// Update the row of the style dimension button so visible buttons will be top stacked
				this.tableLayoutPanel.SetRow(this.styleButton, lastUsedDimensionButtonRow++);

				// Add the style variants to the flow panel
				MakeButtonWidthsUniform(styleButtons);
                this.dimensionsPanel.Controls.AddRange(styleButtons.ToArray());
			}

			// Config
			if (configButtons == null && viewModel.Exists(DimensionType.Config))
			{
				List<DimensionValue> configs = viewModel.AllVariants.Where(dim => dim.Dimension == DimensionType.Config).ToList();
				configButtons = new List<Control>(configs.Count);

				// Configs
				foreach (var config in configs)
				{
					var button = CreateVariantButton(config.Name, config.Id);
					configButtons.Add(button);
				}

				this.configButton.Visible = true;

				// Update the row of the config dimension button so visible buttons will be top stacked
				this.tableLayoutPanel.SetRow(this.configButton, lastUsedDimensionButtonRow);

				// Add the config variants to the flow panel
				MakeButtonWidthsUniform(configButtons);
                this.dimensionsPanel.Controls.AddRange(configButtons.ToArray());
			}

            if (this.dimensionsPanel.Controls.Count > 0)
            {
                this.dimensionsPanel.VerticalScroll.SmallChange = this.dimensionsPanel.Controls[0].Height + this.dimensionsPanel.Margin.Top + this.dimensionsPanel.Margin.Bottom;
            }

			// Get the button associated with the current dimension
			SimpleButton dimBtn = GetDimensionButton(viewModel.CurrentDimension);

			// Update the row for rendering the arrow
			this.CurrentArrowRow = this.tableLayoutPanel.GetRow(dimBtn);

			// "Click" the next button to load its variants
			dimBtn.PerformClick();
		}

		private static void MakeButtonWidthsUniform(List<Control> buttonCollection)
		{
			// Find the widest button in the collection
			int maxWidth = buttonCollection.Max(btn => btn.Width);

			// Update all buttons in the collection to match
			buttonCollection.ForEach(btn => btn.Width = maxWidth);
		}

		/// <summary>
		/// Provides a map between the dimension and associated button
		/// </summary>        
		private SimpleButton GetDimensionButton(DimensionType dim)
		{
			switch (dim)
			{
				case DimensionType.Color:
					return this.colorButton;

				case DimensionType.Size:
					return this.sizeButton;

				case DimensionType.Style:
					return this.styleButton;

				case DimensionType.Config:
					return this.configButton;

				default:
					throw new InvalidEnumArgumentException(dim.ToString());
			}
		}

        /// <summary>
        /// Escape special character for DataTable.Select.
        /// There are no methods in .NET which would do this, so adding our own.
        /// </summary>
        /// <param name="str">String to escape.</param>
        /// <returns>Returns escaped string.</returns>
        private static string Escape(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return str.Replace("'", "''");
        }

        private string GetSelectString()
        {
            string result = "";

            //Color
            if (!string.IsNullOrEmpty(colorId))
            {
                result = "COLORID ='" + Escape(colorId) + "'";
            }

            //Size
            if (!string.IsNullOrEmpty(sizeId))
            {
                if (result.Length == 0)
                {
                    result += "SIZEID = '" + Escape(sizeId) + "'"; ;
                }
                else
                {
                    result += " AND SIZEID = '" + Escape(sizeId) + "'";
                }
            }

            //Style
            if (!string.IsNullOrEmpty(styleId))
            {
                if (result.Length == 0)
                {
                    result += "STYLEID = '" + Escape(styleId) + "'"; ;
                }
                else
                {
                    result += " AND STYLEID = '" + Escape(styleId) + "'";
                }
            }

            //Config
            if (!string.IsNullOrEmpty(configId))
            {
                if (result.Length == 0)
                {
                    result += "CONFIGID = '" + Escape(configId) + "'"; ;
                }
                else
                {
                    result += " AND CONFIGID = '" + Escape(configId) + "'";
                }
            }

            return result;
        }

		private void SaveSelectedDimensions()
		{
			this.colorId = viewModel.ActiveColorId ?? String.Empty;
			this.sizeId = viewModel.ActiveSizeId ?? String.Empty;
			this.styleId = viewModel.ActiveStyleId ?? String.Empty;
			this.configId = viewModel.ActiveConfigId ?? String.Empty;
		}

		private DataRow SetResult()
		{
			DataRow result = null;

			if (viewModel.IsFinished())
			{
				SaveSelectedDimensions();
				DataRow[] rows = (DataRow[])inventDimCombination.Select(GetSelectString());

				if (rows.Length > 0)
				{
					result = rows[0];
				}
			}

			return result;
		}

		private void Finished()
		{
			selectDimCombination = SetResult();
			if (selectDimCombination != null)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void OnColorButton_Click(object sender, EventArgs e)
		{
			// Show the color variants, hide the rest
			UpdateVariants(true, false, false, false);

			// Update the arrow position
			UpdateArrow(this.colorButton);

			// Update the current dimension
			this.viewModel.CurrentDimension = DimensionType.Color;

			// Move focus to the first enabled variant
            this.SelectNextControl(this.dimensionsPanel, true, true, true, false);

            this.ResetSize();
		}

        //Prevent the check button changing state (backColor) when clicked. Only change state when the category is selected. 
        private void OnColorButton_CheckedChanged(object sender, EventArgs e)
        {
            colorButton.Checked = colorSelected;
        }

		private void OnSizeButton_Click(object sender, EventArgs e)
		{
			// Show the size variants, hide the rest
			UpdateVariants(false, true, false, false);

			UpdateArrow(this.sizeButton);

			this.viewModel.CurrentDimension = DimensionType.Size;

			// Move focus to the first enabled variant
            this.SelectNextControl(this.dimensionsPanel, true, true, true, false);

            this.ResetSize();
		}

        private void OnSizeButton_CheckedChanged(object sender, EventArgs e)
        {
            sizeButton.Checked = sizeSelected;
        }

		private void OnStyleButton_Click(object sender, EventArgs e)
		{
			// Show the style variants, hide the rest
			UpdateVariants(false, false, true, false);

			UpdateArrow(this.styleButton);

			this.viewModel.CurrentDimension = DimensionType.Style;

			// Move focus to the first enabled variant
            this.SelectNextControl(this.dimensionsPanel, true, true, true, false);

            this.ResetSize();
		}

        private void OnStyleButton_CheckedChanged(object sender, EventArgs e)
        {
            styleButton.Checked = styleSelected;
        }

		private void OnConfigButton_Click(object sender, EventArgs e)
		{
			// Show the config variants, hide the rest
			UpdateVariants(false, false, false, true);

			UpdateArrow(this.configButton);

			this.viewModel.CurrentDimension = DimensionType.Config;

			// Move focus to the first enabled variant
            this.SelectNextControl(this.dimensionsPanel, true, true, true, false);

            this.ResetSize();
		}

        private void OnConfigButton_CheckedChanged(object sender, EventArgs e)
        {
            configButton.Checked = configSelected;
        }

		/// <summary>
		/// Updates the row to draw the arrow on then invalidates the painting surface
		/// </summary>
		/// <param name="buttonOfRow">Button the arrow is pointing to</param>
		private void UpdateArrow(Control buttonOfRow)
		{
			this.CurrentArrowRow = this.tableLayoutPanel.GetRow(buttonOfRow);
			this.tableLayoutPanel.Invalidate(false);
		}

		/// <summary>
		/// Updates the state of the choice buttons
		/// </summary>        
		/// <param name="colorVisibility">Visiblity of the color buttons</param>
		/// <param name="sizeVisibility">Visiblity of the size buttons</param>
		/// <param name="styleVisibility">Visiblity of the style buttons</param>
		/// <param name="configVisibility">Visiblity of the config buttons</param>
		private void UpdateVariants(bool colorVisibility, bool sizeVisibility, bool styleVisibility, bool configVisibility)
		{
            // this is done in order to increase the performance
            this.dimensionsPanel.SuspendLayout();

			if (colorButtons != null)
				colorButtons.ForEach(btn => btn.Visible = colorVisibility);

			if (sizeButtons != null)
				sizeButtons.ForEach(btn => btn.Visible = sizeVisibility);

			if (styleButtons != null)
				styleButtons.ForEach(btn => btn.Visible = styleVisibility);

			if (configButtons != null)
				configButtons.ForEach(btn => btn.Visible = configVisibility);

            this.dimensionsPanel.ResumeLayout();
		}

		private void OnTableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			// Draw the arrow
			if (e.Column == ARROWCOLUMN && e.Row == CurrentArrowRow)
			{
				using (Pen p = new Pen(this.ForeColor, 15))
				{
					p.StartCap = LineCap.Flat;
					p.EndCap = LineCap.ArrowAnchor;
					int y = e.CellBounds.Top + (e.CellBounds.Height / 2);
					Point p1 = new Point(e.CellBounds.Left, y);
					Point p2 = new Point(e.CellBounds.Right, y);
					e.Graphics.DrawLine(p, p1, p2);
				}
			}
		}

        private void ParentPanel_Resize(object sender, EventArgs e)
        {
            this.ResetSize();
        }

        private void ResetSize()
        {
            // Hide the horizontal scroll bar if necessary.
            if (this.dimensionsPanel.HorizontalScroll.Visible)
            {
                this.dimensionsPanel.Height = this.parentPanel.Height + SCROLL_SIZE;
            }
            else
            {
                this.dimensionsPanel.Height = this.parentPanel.Height;
            }

            // Hide the vertical scroll bar if necessary.
            if (this.dimensionsPanel.VerticalScroll.Visible)
            {
                this.dimensionsPanel.Width = this.parentPanel.Width + SCROLL_SIZE;
            }
            else
            {
                this.dimensionsPanel.Width = this.parentPanel.Width;
            }

            this.btnDown.Enabled = this.dimensionsPanel.VerticalScroll.Visible;
            this.btnPageDown.Enabled = this.dimensionsPanel.VerticalScroll.Visible;
            this.btnUp.Enabled = false;
            this.btnPageUp.Enabled = false;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.dimensionsPanel.VerticalScroll;
            int step = v_prop.Value;
            step += v_prop.SmallChange;

            if (step >= v_prop.Maximum)
            {
                step = v_prop.Maximum;
                this.btnDown.Enabled = false;
                this.btnPageDown.Enabled = false;
            }

            this.btnUp.Enabled = true;
            this.btnPageUp.Enabled = true;

            this.dimensionsPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.dimensionsPanel.VerticalScroll;
            int step = v_prop.Value;
            step += v_prop.LargeChange;
            
            if (step >= v_prop.Maximum)
            {
                step = v_prop.Maximum;
                this.btnDown.Enabled = false;
                this.btnPageDown.Enabled = false;
            }
            this.btnUp.Enabled = true;
            this.btnPageUp.Enabled = true;

            this.dimensionsPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.dimensionsPanel.VerticalScroll;
            int step = v_prop.Value;
            step -= v_prop.SmallChange;

            if (step <= v_prop.Minimum)
            {
                step = v_prop.Minimum;
                this.btnUp.Enabled = false;
                this.btnPageUp.Enabled = false;
            }

            this.btnDown.Enabled = true;
            this.btnPageDown.Enabled = true;

            this.dimensionsPanel.AutoScrollPosition = new Point(0, step);
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            VScrollProperties v_prop = this.dimensionsPanel.VerticalScroll;
            int step = v_prop.Value;
            step -= v_prop.LargeChange;

            if (step <= v_prop.Minimum)
            {
                step = v_prop.Minimum;
                this.btnUp.Enabled = false;
                this.btnPageUp.Enabled = false;
            }

            this.btnDown.Enabled = true;
            this.btnPageDown.Enabled = true;

            this.dimensionsPanel.AutoScrollPosition = new Point(0, step);
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
			return new DimensionConfirmation
			{
				SelectDimCombination = this.SelectDimCombination,
				Confirmed = this.DialogResult == DialogResult.OK
			} as TResults;
		}

		#endregion

	}
}