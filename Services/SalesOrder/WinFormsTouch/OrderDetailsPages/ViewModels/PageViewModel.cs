/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class PageViewModel : INotifyPropertyChanged
    {
        private CustomerOrderTransaction transaction;
        private CustomerOrderTypeFormatter orderTypeFormatter = new CustomerOrderTypeFormatter();
        private CustomerOrderStatusFormatter orderStatusFormatter = new CustomerOrderStatusFormatter();

        public CustomerOrderTransaction Transaction
        {
            get { return this.transaction; }
            set
            {
                if (this.transaction != value)
                {
                    SetTransaction(value);
                    OnPropertyChanged("Transaction");
                }
            }
        }

        protected CustomerOrderTypeFormatter OrderTypeFormatter { get { return this.orderTypeFormatter; } }
        protected CustomerOrderStatusFormatter OrderStatusFormatter { get { return this.orderStatusFormatter; } }

        protected void SetTransaction(CustomerOrderTransaction custTransaction)
        {
            this.transaction = custTransaction;
        }

        /// <summary>
        /// Force the view model to throw out any cached values
        /// </summary>
        public virtual void Refresh()
        {
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        [System.Diagnostics.DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new Exception(msg);
            }
        }

        #endregion
    }
}
