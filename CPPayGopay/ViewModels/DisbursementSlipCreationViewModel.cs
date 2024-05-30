/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using LSRetailPosis.POSProcesses.ViewModels;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity.Transaction.MemoryTables;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.ViewModels
{
    /// <summary>
    /// Disbursement slip order creation view model.
    /// </summary>
    internal sealed class DisbursementSlipCreationViewModel : IDisbursementSlipInfo, INotifyPropertyChanged
    {
        #region Ctor
        /// <summary>
        /// Default Ctor.
        /// </summary>
        public DisbursementSlipCreationViewModel()
        {
            Clear();
        }
        #endregion

        #region IDisbursementSlipInfo

        /// <summary>
        /// Person name
        /// </summary>
        public string PersonName
        {
            get { return personName; }
            set
            {
                personName = value;
                OnPropertyChanged("PersonName");
                Validate();
            }
        }

        /// <summary>
        /// Customer identity card information
        /// </summary>
        public string CardInfo
        {
            get { return cardInfo; }
            set
            {
                cardInfo = value;
                OnPropertyChanged("CardInfo");
                Validate();
            }
        }

        /// <summary>
        /// Reason of cash return
        /// </summary>
        public string ReasonOfReturn
        {
            get { return reasonOfReturn; }
            set
            {
                reasonOfReturn = value;
                OnPropertyChanged("ReasonOfReturn");
            }
        }

        /// <summary>
        /// Document number
        /// </summary>
        public string DocumentNum
        {
            get { return documentNum; }
            set
            {
                documentNum = value;
                OnPropertyChanged("DocumentNum");
            }
        }

        /// <summary>
        /// Document date
        /// </summary>
        public DateTime DocumentDate
        {
            get {return documentDate;}
            set
            {
                documentDate = value;
                OnPropertyChanged("DocumentDate");
            }
        }

        #endregion

        private bool isValid;

        /// <summary>
        /// Validates the disbursement slip information entered.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sets the <see cref="IsValid"/> property.
        /// </remarks>
        internal bool Validate()
        { 
            isValid =
                !string.IsNullOrWhiteSpace(PersonName) &&
                !string.IsNullOrWhiteSpace(CardInfo);

            OnPropertyChanged("IsValid");

            return isValid;
        }

        /// <summary>
        /// Determines whether current state of the entered information is valid.
        /// </summary>
        public bool IsValid
        {
            get { return isValid; }
        }

        /// <summary>
        /// Clears all teh fields in teh view model.
        /// </summary>
        internal void Clear()
        {
            PersonName = null;
            CardInfo = null;
            ReasonOfReturn = null;
            DocumentNum = null;
            DocumentDate = DateTime.Today;
        }

        private void NotifyAllPropertiesChanged()
        {
            OnPropertyChanged("PersonName");
            OnPropertyChanged("CardInfo");
            OnPropertyChanged("ReasonOfReturn");
            OnPropertyChanged("DocumentNum");
            OnPropertyChanged("DocumentDate");
            OnPropertyChanged("IsValid");
        }

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

        #region Private members
        private string personName;
        private string cardInfo;
        private string reasonOfReturn;
        private string documentNum;
        private DateTime documentDate;
        #endregion
    }
}
