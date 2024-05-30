/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    partial class OrderDetailsPage : UserControl
    {
        public OrderDetailsPage()
        {
            InitializeComponent();
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(701, 565);
            }
        }

        public virtual void SetViewModel<T>(T viewModel)
        {
            bindingSource.Add(viewModel);
            this.GetViewModel<INotifyPropertyChanged>().PropertyChanged += new PropertyChangedEventHandler(OnViewModel_PropertyChanged);
        }

        /// <summary>
        /// Gets the ViewModel
        /// </summary>        
        protected T GetViewModel<T>()
        {
            return (T)bindingSource.Current;
        }

        /// <summary>
        /// Called when a property changes on the view model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Called when the page is activated and becomes the front page
        /// </summary>
        public virtual void OnActivate()
        {
            // Refresh any cached values
            this.GetViewModel<PageViewModel>().Refresh();

            // Refresh control bindings. 
            // We wouldn't need this if the model (CustomerOrderTransaction) also implemented INotifyPropertyChanged.
            this.bindingSource.ResetBindings(false);
        }

        // Called when the Clear button is clicked and this is the focus page
        public virtual void OnClear()
        {
        }

        public virtual bool IsClearEnabled()
        {
            return true;
        }

        #region Navigation methods

        /// <summary>
        /// Called when the up button is clicked
        /// </summary>
        public virtual void OnUpButtonClicked()
        {
        }

        /// <summary>
        /// Called when the page up button is clicked
        /// </summary>
        public virtual void OnPageUpButtonClicked()
        {
        }

        /// <summary>
        /// Called when the down button is clicked
        /// </summary>
        public virtual void OnDownButtonClicked()
        {
        }

        /// <summary>
        /// Called when the page down button is clicked
        /// </summary>
        public virtual void OnPageDownButtonClicked()
        {
        }

        /// <summary>
        /// Indicates whether an up button should be enabled.
        /// </summary>
        /// <example>It should be enabled if the current selected row of a grid is not the first</example>
        /// <returns>True if button should be enabled</returns>
        public virtual bool IsUpButtonEnabled()
        {
            return true;
        }

        /// <summary>
        /// Indicates whether a down button should be enabled.
        /// </summary>
        /// <example>It should be enabled if the current selected row of a grid is not the last</example>
        /// <returns>True if button should be enabled</returns>
        public virtual bool IsDownButtonEnabled()
        {
            return true;
        }

        /// <summary>
        /// Indicates whether navigatrion arrow buttons should be displayed.
        /// </summary>
        /// <remarks>Derived classes should override and return true if they contain a grid or similar control that requires navigation buttons</remarks>
        /// <returns>True if navigation buttons are required, false otherwise</returns>
        public virtual bool PageRequiresNavigationButtons()
        {
            return false;
        }

        #endregion
    }
}