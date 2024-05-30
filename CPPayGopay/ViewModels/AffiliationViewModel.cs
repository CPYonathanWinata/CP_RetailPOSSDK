/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LSRetailPosis.POSProcesses.ViewModels;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// View model class for affiliation form
    /// </summary>
    internal sealed class AffiliationViewModel : INotifyPropertyChanged
    {
        #region Fields

        private Collection<AffilitionItemViewModel> displayAffiliations;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliationViewModel"/> class.
        /// </summary>
        /// <param name="selectedAffiliations">A list of selected affiliations.</param>
        public AffiliationViewModel(IList<object> selectedAffiliations)
        {
            this.Initialize(selectedAffiliations);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Affiliation list for display on AffiliationForm
        /// </summary>
        public Collection<AffilitionItemViewModel> DisplayAffiliations
        {
            get { return this.displayAffiliations; }
            private set
            {
                this.displayAffiliations = value;
                OnPropertyChanged("DisplayAffiliations");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize data.
        /// </summary>
        /// <param name="selectedAffiliations">Already selected affiliations.</param>
        private void Initialize(IList<object> selectedAffiliations)
        {
            DM.AffiliationDataManager affiliationDataManager = new DM.AffiliationDataManager(
                ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

            this.DisplayAffiliations = new Collection<AffilitionItemViewModel>();

            // Get affiliations that already selected. Mark them as selected in form.
            var affiliations = (selectedAffiliations == null ? null : selectedAffiliations.Cast<IAffiliation>());

            foreach (IAffiliation affiliation in affiliationDataManager.GetAffiliations(ApplicationSettings.Terminal.CultureName))
            {
                AffilitionItemViewModel itemViewModel = new AffilitionItemViewModel(affiliation);

                // Check if this affiliation exist in selection.
                // Mark it as selected if affiliation already exist.
                itemViewModel.IsSelected = (affiliations != null && affiliations.Any(a => a.RecId == affiliation.RecId));

                this.DisplayAffiliations.Add(itemViewModel);
            }
        }

        private void RefreshDataSouce()
        {
            OnPropertyChanged("DisplayAffiliations");
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
    }

    /// <summary>
    /// View Model encapsulating an individual affiliation Item on the affilaition form
    /// </summary>
    internal class AffilitionItemViewModel
    {
        /// <summary>
        /// Indicate if affiliation was selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Current affiliation.
        /// </summary>
        public IAffiliation Affiliation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffilitionItemViewModel"/> class.
        /// </summary>
        /// <param name="affiliation">Current affiliation.</param>
        public AffilitionItemViewModel(IAffiliation affiliation)
        {
            this.Affiliation = affiliation;
        }
    }
}
