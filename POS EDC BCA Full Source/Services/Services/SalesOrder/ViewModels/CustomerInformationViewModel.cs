/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class CustomerInformationViewModel : PageViewModel
    {
        public CustomerInformationViewModel(CustomerOrderTransaction customerTransaction)
        {
            this.SetTransaction(customerTransaction);
        }

        ///<summary>
        ///Gets the Name of the customer
        ///</summary>
        public string Name
        {
            get
            {
                return (this.Transaction.Customer == null)
                       ? string.Empty 
                       : this.Transaction.Customer.Name;
            }
        }

        ///<summary>
        ///Gets the Phone Number of the customer
        ///</summary>
        public string Phone
        {
            get
            {
                return (this.Transaction.Customer == null)
                       ? string.Empty 
                       : this.Transaction.Customer.Telephone;
            }
        }

        ///<summary>
        ///Gets the Email id of the customer
        ///</summary>
        public string Email
        {
            get
            {
                return (this.Transaction.Customer == null)
                       ? string.Empty 
                       : this.Transaction.Customer.Email;
            }
        }
        #region Commands

        /// <summary>
        /// Command to clear this.Transaction.Customer and billing info
        /// </summary>
        public void ExecuteClearCustomerCommand()
        {            
            ExecuteCommandAndUpdateTransaction(Contracts.PosisOperations.CustomerClear);
            this.OnPropertyChanged("Transaction");
        }

        public void ExecuteCustomerSearchCommand()
        {
            ExecuteCommandAndUpdateTransaction(Contracts.PosisOperations.CustomerSearch);
            OnPropertyChanged("Transaction");
        }

        public void ExecuteCustomerAddCommand()
        {
            ExecuteCommandAndUpdateTransaction(Contracts.PosisOperations.CustomerAdd);
            OnPropertyChanged("Transaction");
        }

        private void ExecuteCommandAndUpdateTransaction(Contracts.PosisOperations command)
        {
            IPosTransaction ipt = SalesOrder.InternalApplication.RunOperation(command, string.Empty, this.Transaction);
            if((ipt != null) && (!ipt.OperationCancelled))
            {
                this.Transaction = (CustomerOrderTransaction)ipt;                
            }
            OnPropertyChanged("Transaction");
        }
        #endregion 
    }
}