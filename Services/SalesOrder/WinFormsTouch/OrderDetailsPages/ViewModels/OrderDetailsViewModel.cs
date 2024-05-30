/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LSRetailPosis;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.SaleItem;
using LSRetailPosis.Transaction.Line.Tax;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch;
using CustomerOrderMode = LSRetailPosis.Transaction.CustomerOrderMode;
using CustomerOrderType = LSRetailPosis.Transaction.CustomerOrderType;
using DM = Microsoft.Dynamics.Retail.Pos.DataManager;
using SalesStatus = LSRetailPosis.Transaction.SalesStatus;
using System.Data.SqlClient;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{

    sealed class OrderDetailsViewModel : PageViewModel
    {
        private int selectedPageIndex = -1;
        private string selectedPageTitle;
        private DateTime? requestedDeliveryDate;
        private ReadOnlyCollection<DataEntity.Employee> employees;
        private OrderDetailsSelection mode;

        ICustomCustOrderTransaction customCustOrderTrans;


        //add Yonathan 07/02/2024
        string custSalesTakerId { get; set; }
        string custSalesTakerName { get; set; }
        string custSalesTakerNameOnReceipt { get; set; }
        //end

        // In OrderDetailsViewModel class
        public string GetCustSalesTakerId()
        {


            return custSalesTakerId;

        }


         public List<DataEntity.Employee> GetEmployeeListFromDatabase(string connectionString)
        {
            List<DataEntity.Employee> employeeList = new List<DataEntity.Employee>();
            //string connectionString = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT PERSONNELNUMBER, FIRSTNAME, HCMWORKER.RECID HCMREC FROM HCMWORKER
                                        JOIN DIRPERSONNAME ON HCMWORKER.PERSON = DIRPERSONNAME.PERSON

                                        ORDER BY FIRSTNAME"; // Adjust query as needed

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataEntity.Employee employee = new DataEntity.Employee
                                {
                                    // Map properties from the database columns
                                    WorkerId = (long)reader["HCMREC"],
                                    StaffId = reader["PERSONNELNUMBER"].ToString(),
                                    NameOnReceipt = reader["FIRSTNAME"].ToString()
                                    // Map other properties...
                                };

                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, throw, etc.)
                //Console.WriteLine($"Error retrieving employee list: {ex.Message}");
            }

            return employeeList;
        }

        public OrderDetailsViewModel(
            CustomerOrderTransaction customerOrderTransaction,
            OrderDetailsSelection selectedMode)
        {
            this.SetTransaction(customerOrderTransaction);

            this.mode = selectedMode;

            DM.StoreDataManager storeDataManager = new DM.StoreDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);

            DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                SalesOrder.InternalApplication.Settings.Database.Connection,
                SalesOrder.InternalApplication.Settings.Database.DataAreaID);



            // Collection of all employees
            //GetEmployeeListFromDatabase(employeeDataManager.Connection.ConnectionString);
            // custom by Yonathan to change the lookup 28/02/2024
            //employees = new ReadOnlyCollection<DataEntity.Employee>(GetEmployeeListFromDatabase(employeeDataManager.Connection.ConnectionString));
            employees = new ReadOnlyCollection<DataEntity.Employee>(employeeDataManager.GetEmployees(ApplicationSettings.Terminal.StoreId));

            // Set the default minimum expiration date to tomorrow (but do not overwrite the current expiration date already set on the transaction)
            this.MinimumOrderExpirationDate = DateTime.Today.AddDays(1);

            // Use the actual, recalled, expiration date if it is older.
            // This is needed to prevent the UI from forcing the older date to snap to the minimum.
            if (this.MinimumOrderExpirationDate > this.OrderExpirationDate)
            {
                this.MinimumOrderExpirationDate = this.OrderExpirationDate;
            }

            // Set the default sales person to the currently logged in operator
            if (string.IsNullOrWhiteSpace(this.SalesPersonId))
                this.SalesPersonId = LSRetailPosis.Settings.ApplicationSettings.Terminal.TerminalOperator.OperatorId;

            if (string.IsNullOrWhiteSpace(this.SalesTakerId))
                this.SalesTakerId = LSRetailPosis.Settings.ApplicationSettings.Terminal.TerminalOperator.OperatorId;


        }




        /// <summary>
        /// Gets or sets the order id
        /// </summary>
        public string OrderId
        {
            get { return this.Transaction.OrderId; }
            set
            {
                if (this.Transaction.OrderId != value)
                {
                    this.Transaction.OrderId = value;
                    OnPropertyChanged("OrderId");
                }
            }
        }

        /// <summary>
        /// Gets the display string for order type
        /// </summary>
        public string FormattedOrderType
        {
            get
            {
                return this.OrderTypeFormatter.Format(null, this.Transaction.OrderType, null);
            }
        }

        /// <summary>
        /// Gets or sets the type of the order
        /// </summary>
        public CustomerOrderType OrderType
        {
            get { return this.Transaction.OrderType; }
            set
            {
                if (this.Transaction.OrderType != value)
                {
                    this.Transaction.OrderType = value;
                    this.Transaction.CalcTotals();
                    OnPropertyChanged("OrderType");
                    OnPropertyChanged("FormattedOrderType");
                }
            }
        }

        /// <summary>
        /// Gets or sets selected page
        /// </summary>
        public int SelectedPageIndex
        {
            get { return this.selectedPageIndex; }
            set
            {
                if (this.selectedPageIndex != value)
                {
                    this.selectedPageIndex = value;
                    OnPropertyChanged("SelectedPageIndex");
                    OnPropertyChanged("SelectedPageTitle");
                }
            }
        }

        /// <summary>
        /// Gets or sets the displayed title of the selected page
        /// </summary>
        public string SelectedPageTitle
        {
            get { return this.selectedPageTitle; }
            set
            {
                if (this.selectedPageTitle != value)
                {
                    this.selectedPageTitle = value;
                    OnPropertyChanged("SelectedPageTitle");
                }
            }
        }

        /// <summary>
        /// Gets or sets the comment for the order
        /// </summary>
        public string OrderComment
        {
            get { return ((IPosTransactionV1)this.Transaction).Comment; }
            set
            {
                if (((IPosTransactionV1)this.Transaction).Comment != value)
                {
                    ((IPosTransactionV2)this.Transaction).Comment = value != null ? value.Replace(Environment.NewLine, string.Empty) : string.Empty;
                    OnPropertyChanged("OrderComment");
                }
            }
        }

        /// <summary>
        /// Gets the loyalty card id
        /// </summary>
        public string LoyaltyCardId
        {
            get { return this.Transaction.LoyaltyItem != null ? this.Transaction.LoyaltyItem.LoyaltyCardNumber : string.Empty; }
        }

        /// Gets the channel card id
        /// </summary>
        public string ChannelCardId
        {
            get { return this.Transaction.ChannelReferenceId; }
        }

        /// <summary>
        /// Gets the formatted status of the order
        /// </summary>
        public string FormattedOrderStatus
        {
            get
            {
                string displayString = null;

                switch (this.Transaction.OrderStatus)
                {
                    case SalesStatus.Unknown:
                    case SalesStatus.Confirmed:
                    case SalesStatus.Created:
                    case SalesStatus.Processing:
                    case SalesStatus.Lost:
                    case SalesStatus.Canceled:
                    case SalesStatus.Sent:
                    case SalesStatus.Delivered:
                    case SalesStatus.Invoiced:
                        displayString = this.OrderStatusFormatter.Format(null, this.Transaction.OrderStatus, null);
                        break;
                    default:
                        throw new InvalidEnumArgumentException(this.Transaction.OrderStatus.ToString());
                }

                return displayString;
            }
        }

        /// <summary>
        /// Gets the sales person for the order
        /// </summary>
        public string SalesPerson
        {
            get
            {
                string[] parts;
                if (this.Transaction.SalesPersonName.Contains('|') == true)
                {
                    parts = this.Transaction.SalesPersonName.Split('|');
                    return parts[0];
                }
                else
                {
                    return this.Transaction.SalesPersonName;
                }

            }

        }

        /// <summary>
        /// Gets or sets the sales person for the order
        /// </summary>
        public string SalesPersonId
        {

            get
            {
                string[] parts;
                if (this.Transaction.SalesPersonId != null && this.Transaction.SalesPersonId.Contains('|') == true)
                {
                    parts = this.Transaction.SalesPersonId.Split('|');
                    return parts[0];
                }
                else
                {
                    return this.Transaction.SalesPersonId;
                }
                //parts = this.Transaction.SalesPersonId.Contains('|') == true ? this.Transaction.SalesPersonId.Split('|') : this.Transaction.SalesPersonId ;

            }
            set
            {
                string[] parts;
                if (this.Transaction.SalesPersonId != null &&  this.Transaction.SalesPersonId.Contains('|') == true)
                {
                    parts = this.Transaction.SalesPersonId.Split('|');
                    if (!string.Equals(parts[0], value, StringComparison.Ordinal))
                    {
                        this.Transaction.SalesPersonId = changeSalesPerson(this.Transaction.SalesPersonId, value);

                        DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                            SalesOrder.InternalApplication.Settings.Database.Connection,
                            SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                        // Set the associated name properties
                        DataEntity.Employee employee = employeeDataManager.GetEmployee(ApplicationSettings.Terminal.StoreId, value);
                        
                        this.Transaction.SalesPersonName = (employee != null) ? changeSalesPerson(this.Transaction.SalesPersonName, employee.Name) : null;
                        this.Transaction.SalesPersonNameOnReceipt = (employee != null) ? changeSalesPerson(this.Transaction.SalesPersonNameOnReceipt, employee.NameOnReceipt) : null;

                        OnPropertyChanged("SalesPersonId");
                        OnPropertyChanged("SalesPerson");

                    }
                }
                else
                {
                    if (!string.Equals(this.Transaction.SalesPersonId, value, StringComparison.Ordinal))
                    {
                        this.Transaction.SalesPersonId = value;

                        DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                            SalesOrder.InternalApplication.Settings.Database.Connection,
                            SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                        // Set the associated name properties
                        DataEntity.Employee employee = employeeDataManager.GetEmployee(ApplicationSettings.Terminal.StoreId, value);

                        this.Transaction.SalesPersonName = (employee != null) ? employee.Name : null;
                        this.Transaction.SalesPersonNameOnReceipt = (employee != null) ? employee.NameOnReceipt : null;

                        OnPropertyChanged("SalesPersonId");
                        OnPropertyChanged("SalesPerson");

                    }
                }



            }
        }


        // Add by Yonathan 07/02/2024
        public string changeSalesPerson(string oldPerson, string newPerson)
        {
            string[] parts;
            string joinedPerson = "";
            // Split the string using the pipe character '|'

            if (oldPerson == null)
            {
                oldPerson = "|";
            }
            parts = oldPerson.Split('|');
            parts[0] = newPerson;

            joinedPerson = string.Join("|", parts);

            return joinedPerson;
        }

        public string changeSalesTaker(string oldTaker, string newTaker)
        {
            string[] parts;
            string joinedTaker = "";
            // Split the string using the pipe character '|'
            if (oldTaker == null)
            {
                oldTaker = "|";
            }
            parts = oldTaker.Split('|');
            parts[1] = newTaker;

            joinedTaker = string.Join("|", parts);

            return joinedTaker;
        }

        public string SalesTaker
        {
            get
            {
                string[] parts;
                if (this.Transaction.SalesPersonName.Contains('|') == true)
                {
                    parts = this.Transaction.SalesPersonName.Split('|');
                    return parts[1];
                }
                else
                {
                    return "";
                }

            }
        }


        /// <summary>
        /// Gets or sets the sales person for the order
        /// </summary>
        public string SalesTakerId
        {
            //customCustOrderTrans = new CustomerOrderTransaction();
            get { return this.Transaction.SalesPersonId; }
            set
            {
                string[] parts;
                if (this.Transaction.SalesPersonId.Contains('|') == true)
                {
                    parts = this.Transaction.SalesPersonId.Split('|');
                    if (!string.Equals(parts[1], value, StringComparison.Ordinal))
                    {
                        this.Transaction.SalesPersonId = changeSalesTaker(this.Transaction.SalesPersonId, value);

                        DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                            SalesOrder.InternalApplication.Settings.Database.Connection,
                            SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                        // Set the associated name properties
                        DataEntity.Employee employee = employeeDataManager.GetEmployee(ApplicationSettings.Terminal.StoreId, value);

                        this.Transaction.SalesPersonName = (employee != null) ? changeSalesTaker(this.Transaction.SalesPersonName, employee.Name) : null;
                        this.Transaction.SalesPersonNameOnReceipt = (employee != null) ? changeSalesTaker(this.Transaction.SalesPersonNameOnReceipt, employee.NameOnReceipt) : null;

                        OnPropertyChanged("SalesTakerId");
                        OnPropertyChanged("SalesTaker");

                    }
                }
                else
                {
                    if (!string.Equals(this.Transaction.SalesPersonId, value, StringComparison.Ordinal))
                    {
                        this.Transaction.SalesPersonId = this.Transaction.SalesPersonId + "|" + value; //change to this

                        DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                            SalesOrder.InternalApplication.Settings.Database.Connection,
                            SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                        // Set the associated name properties
                        DataEntity.Employee employee = employeeDataManager.GetEmployee(ApplicationSettings.Terminal.StoreId, value);

                        this.Transaction.SalesPersonName = (employee != null) ? this.Transaction.SalesPersonName + "|" + employee.Name : this.Transaction.SalesPersonName + "|" + null;
                        this.Transaction.SalesPersonNameOnReceipt = (employee != null) ? this.Transaction.SalesPersonNameOnReceipt + "|" + employee.NameOnReceipt : this.Transaction.SalesPersonNameOnReceipt + "|" + null;

                        OnPropertyChanged("SalesTakerId");
                        OnPropertyChanged("SalesTaker");

                    }
                }


                
            }
        }


        /// <summary>
        /// Gets minimum valid expiration date.
        /// </summary>
        public DateTime MinimumOrderExpirationDate { get; private set; }

        /// <summary>
        /// Gets or sets the expiration date of the order
        /// </summary>
        public DateTime OrderExpirationDate
        {
            get
            {
                return this.Transaction.ExpirationDate;
            }
            set
            {
                if (this.Transaction.ExpirationDate != value)
                {
                    this.Transaction.ExpirationDate = value;
                    OnPropertyChanged("OrderExpirationDate");
                }
            }
        }

        /// <summary>
        /// Indicates that we have delivery information set on this transaction.
        /// </summary>
        public bool HasDeliveryInformation
        {
            get
            {
                return this.IsRetrieveMode
                    || this.Transaction.DeliveryMode != null
                    || this.Transaction.SaleItems.Any(item => item.DeliveryMode != null);
            }
        }

        /// <summary>
        /// Indicates if we are in create or retrieve mode
        /// </summary>
        /// <value>True for retrieve mode, false for create</value>
        public bool IsRetrieveMode
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Transaction.OrderId);
            }
        }

        /// <summary>
        /// Indicates if we are in cancel mode
        /// </summary>
        public bool IsCancellationChargeVisible
        {
            get
            {
                // if we have a cancellation charge or if we are on cancel mode, it should be visible
                return this.CancellationCharge.HasValue
                    || this.mode == OrderDetailsSelection.CancelOrder
                    || this.Transaction.Mode == CustomerOrderMode.Cancel
                    && !string.IsNullOrWhiteSpace(ApplicationSettings.Terminal.CancellationChargeCode);
            }
        }

        /// <summary>
        /// Indicates if changes to Delivery options are allowed
        /// </summary>
        public bool IsDeliveryChangeAllowed
        {
            get
            {
                return (this.Transaction.Mode == CustomerOrderMode.Create)
                    || ((this.Transaction.Mode == CustomerOrderMode.Edit) && (this.Transaction.OrderStatus == SalesStatus.Created))
                    || (this.Transaction.Mode == CustomerOrderMode.Convert);
            }
        }

        /// <summary>
        /// Indicates if changes to order type are allowed
        /// </summary>
        public bool IsOrderTypeChangeAllowed
        {
            get
            {
                return (this.Transaction.Mode == CustomerOrderMode.Create)
                    || ((this.Transaction.Mode == CustomerOrderMode.Edit || this.Transaction.Mode == CustomerOrderMode.Convert)
                        && (this.Transaction.OriginalOrderType != CustomerOrderType.SalesOrder));
            }
        }

        /// <summary>
        /// Indicates if changes to the order comments are blocked
        /// </summary>
        public bool IsCommentChangeAllowed
        {
            get { return this.Transaction.Mode != CustomerOrderMode.Pickup; }
        }

        /// <summary>
        /// Gets the shipping address
        /// </summary>
        public IAddress ShippingAddress
        {
            get { return this.Transaction.ShippingAddress; }
        }

        /// <summary>
        /// Gets the formatted shipping charge.
        /// </summary>
        public string FormattedOrderShippingCharge
        {
            get
            {
                return SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(
                    GetOrderShippingCharge(), true, false);
            }
        }

        /// <summary>
        /// Gets the formatted shipping charge.
        /// </summary>
        public string FormattedLinesShippingCharge
        {
            get
            {
                return SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(
                    GetLinesShippingCharge(), true, false);
            }
        }

        /// <summary>
        /// Gets the formatted shipping charge.
        /// </summary>
        public string FormattedTotalShippingCharge
        {
            get
            {
                return SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(
                    GetOrderShippingCharge() + GetLinesShippingCharge(), true, false);
            }
        }

        private decimal GetOrderShippingCharge()
        {
            string shippingChargeCode = ApplicationSettings.Terminal.ShippingChargeCode;
            MiscellaneousCharge mc = this.Transaction.MiscellaneousCharges.FirstOrDefault(
                m => string.Equals(m.ChargeCode, shippingChargeCode, StringComparison.Ordinal));

            return (mc != null) ? mc.Amount : 0m;
        }

        private decimal GetLinesShippingCharge()
        {
            string shippingChargeCode = LSRetailPosis.Settings.ApplicationSettings.Terminal.ShippingChargeCode;

            IEnumerable<SaleLineItem> validItems = this.Transaction.SaleItems.Where(i => !i.Voided && i.Quantity != 0);
            decimal charge = validItems.Select(i => i.MiscellaneousCharges
                .Where(m => string.Equals(m.ChargeCode, shippingChargeCode, StringComparison.Ordinal)))
                .Sum(charges => charges.Sum(c => c.Amount));

            return charge;
        }

        /// <summary>
        /// Gets the formatted cancellation charge.
        /// </summary>
        public string FormattedCancellationCharge
        {
            get
            {
                return (this.CancellationCharge.HasValue)
                    ? SalesOrder.InternalApplication.Services.Rounding.RoundForDisplay(this.CancellationCharge.Value, true, false)
                    : string.Empty;
            }
        }

        public decimal? CancellationCharge
        {
            get
            {

                MiscellaneousCharge mc = this.Transaction.MiscellaneousCharges.SingleOrDefault(
                    m => string.Equals(m.ChargeCode, ApplicationSettings.Terminal.CancellationChargeCode, StringComparison.Ordinal));
                decimal? charge = (mc == null) ? null : (decimal?)mc.Amount;

                return charge;
            }
            set
            {
                if (value.HasValue && value.Value < decimal.Zero)
                    throw new ArgumentOutOfRangeException("Cancellation charge cannot be negative");

                // Track whether we make a change so we raised property changed events
                bool valueChanged = false;

                // See if there is an existing cancellation charge on the transaction
                MiscellaneousCharge mc = this.Transaction.MiscellaneousCharges.SingleOrDefault(
                    m => string.Equals(m.ChargeCode, ApplicationSettings.Terminal.CancellationChargeCode, StringComparison.Ordinal));
                if (mc == null)
                {
                    if (value.HasValue)
                    {
                        // No charge exists, so we add it

                        // Get Cancellation charge properties from DB
                        DM.StoreDataManager store = new DM.StoreDataManager(
                            SalesOrder.InternalApplication.Settings.Database.Connection,
                            SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                        DataEntity.MiscellaneousCharge chargeProperties = store.GetMiscellaneousCharge(ApplicationSettings.Terminal.CancellationChargeCode);

                        if (chargeProperties != null)
                        {
                            mc = (MiscellaneousCharge)SalesOrder.InternalApplication.BusinessLogic.Utility.CreateMiscellaneousCharge(
                                value.Value,
                                this.Transaction.Customer.SalesTaxGroup,
                                chargeProperties.TaxItemGroup,
                                chargeProperties.MarkupCode,
                                string.Empty,
                                this.Transaction);

                            // Set the proper SalesTaxGroup based on the store/customer settings
                            SalesOrder.InternalApplication.BusinessLogic.CustomerSystem.ResetCustomerTaxGroup(mc);

                            this.Transaction.MiscellaneousCharges.Add(mc);

                            valueChanged = true;
                        }
                        else
                        {
                            LSRetailPosis.ApplicationLog.Log(
                                this.ToString(),
                                string.Format("Cancellation charge properties for code:'{0}' not found. Check HQ settings", ApplicationSettings.Terminal.CancellationChargeCode),
                                LogTraceLevel.Error);

                            //"Cancellation charge could not be added because the charge was not found, or charges have not been configured for the store."
                            SalesOrder.InternalApplication.Services.Dialog.ShowMessage(56227, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }
                }
                else // Charge exists
                {
                    if (value.HasValue)
                    {
                        if (mc.Amount != value.Value)
                        {
                            // Update amount
                            mc.Amount = value.Value;

                            valueChanged = true;
                        }
                    }
                    else
                    {
                        // No charge, so remove
                        this.Transaction.MiscellaneousCharges.Remove(mc);

                        valueChanged = true;
                    }
                }

                // Fire changed events if the value was changed
                if (valueChanged)
                {
                    OnPropertyChanged("CancellationCharge");
                    OnPropertyChanged("FormattedCancellationCharge");
                }
            }
        }

        /// <summary>
        /// Requested delivery date
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return requestedDeliveryDate; }
            set
            {
                if (value.HasValue && requestedDeliveryDate != value.Value)
                {
                    requestedDeliveryDate = this.Transaction.RequestedDeliveryDate = value.Value;
                    OnPropertyChanged("DeliveryDate");
                }
            }
        }

        /// <summary>
        /// Collection of employees
        /// </summary>
        public ReadOnlyCollection<DataEntity.Employee> Employees
        {
            get { return this.employees; }
        }

        public void ExecuteClearHeaderDelivery()
        {
            this.Transaction.WarehouseId = ApplicationSettings.Terminal.InventLocationIdForCustomerOrder;
            this.Transaction.ShippingAddress = null;
            this.Transaction.DeliveryMode = null;
        }
    }


    //old
    /*
      /// <summary>
        /// Gets the sales person for the order
        /// </summary>
        public string SalesPerson
        {
            get { return this.Transaction.SalesPersonName; }
        }

        /// <summary>
        /// Gets or sets the sales person for the order
        /// </summary>
        public string SalesPersonId
        {
            get { return this.Transaction.SalesPersonId; }
            set
            {
                if (!string.Equals(this.Transaction.SalesPersonId, value, StringComparison.Ordinal))
                {
                    this.Transaction.SalesPersonId = value;

                    DM.EmployeeDataManager employeeDataManager = new DM.EmployeeDataManager(
                        SalesOrder.InternalApplication.Settings.Database.Connection,
                        SalesOrder.InternalApplication.Settings.Database.DataAreaID);

                    // Set the associated name properties
                    DataEntity.Employee employee = employeeDataManager.GetEmployee(ApplicationSettings.Terminal.StoreId, value);
                    
                    this.Transaction.SalesPersonName = (employee != null) ? employee.Name : null;
                    this.Transaction.SalesPersonNameOnReceipt = (employee != null) ? employee.NameOnReceipt : null;

                    OnPropertyChanged("SalesPersonId");
                    OnPropertyChanged("SalesPerson");

                }
            }
        }
     */
}