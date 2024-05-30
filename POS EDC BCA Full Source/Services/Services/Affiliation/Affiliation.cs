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
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.Settings;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.AffiliationItem;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace Microsoft.Dynamics.Retail.Pos.Affiliation
{
    /// <summary>
    /// Operations for affiliations
    /// </summary>
    [Export(typeof(IAffiliation))]
    public class Affiliation : IAffiliation
    {
        //transaction context
        private RetailTransaction transaction;

        private IApplication application;

        [Import]
        public IApplication Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
            }
        }

        /// <summary>
        /// Set customer affiliations and navigate to Customer Affiliations page.
        /// </summary>
        /// <param name="customer">Customer to be edited</param>
        /// <returns>Customer affiliations results</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Needs refactoring")]
        public void SetCustomerAffiliations(DE.ICustomer customer)
        {
            AffiliationConfirmation affiliationConfirmation = new AffiliationConfirmation() { FormType = Notification.Contracts.FormType.Customer };

            if (customer != null && customer.CustomerAffiliations != null)
            {
                AffiliationDataManager affiliationDataManager = new AffiliationDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

                var affiliaitons = affiliationDataManager.GetAffiliations(customer.CustomerAffiliations, ApplicationSettings.Terminal.CultureName);

                if (affiliaitons != null)
                {
                    affiliationConfirmation.SelectedAffiliations = new ReadOnlyCollection<object>(affiliaitons.ToList<object>());
                }
            }

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(affiliationConfirmation, () =>
            {
                if (affiliationConfirmation.Confirmed)
                {
                    if (affiliationConfirmation.Confirmed && affiliationConfirmation.AffiliationResults != null)
                    {
                        // Keep id for updateing customer affiliations.
                        var oldCustAffiliations = customer.CustomerAffiliations.ToList();
                        var affiliationResults = affiliationConfirmation.AffiliationResults.Cast<DE.IAffiliation>();

                        // Clear exist affiliations and add newly updated affiliations.
                        customer.CustomerAffiliations.Clear();

                        foreach (var affiliation in affiliationResults)
                        {
                            // Get old affiliation so that the customer affiliation id would not lost.
                            DE.ICustomerAffiliation custAffiliation = oldCustAffiliations.FirstOrDefault(c => c.AffiliationId == affiliation.RecId);

                            if (custAffiliation == null)
                            {
                                custAffiliation = new CustomerAffiliation();
                                custAffiliation.AffiliationId = affiliation.RecId;
                            }

                            custAffiliation.CustomerId = customer.CustomerId;

                            customer.CustomerAffiliations.Add(custAffiliation);
                        }
                    }
                }
            });

            Application.Services.Interaction.InteractionRequest(request);
        }

        /// <summary>
        /// Show affiliation page and return selected affiliations.
        /// </summary>
        /// <param name="retailTransaction">Current retail transacion.</param>
        /// <param name="nameOfSelectedAffiliations">A list of selected affiliation name.</param>
        /// <returns>Select result of affiliation</returns>
        public IList<DE.IAffiliation> SelectAffiliations(IList<string> nameOfSelectedAffiliations)
        {
            IList<DE.IAffiliation> selectedAffiliations = null;

            if (nameOfSelectedAffiliations != null && nameOfSelectedAffiliations.Count > 0)
            {
                AffiliationDataManager affiliationDataManager = new AffiliationDataManager(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);
                selectedAffiliations = affiliationDataManager.GetAffiliations(nameOfSelectedAffiliations, ApplicationSettings.Terminal.CultureName).ToList<DE.IAffiliation>();
            }

            return this.SelectAffiliations(selectedAffiliations);
        }

        /// <summary>
        /// Show affiliation page and return selected affiliations.
        /// </summary>
        /// <param name="selectedAffiliations">A list of selected affiliation.</param>
        /// <returns>Select result of affiliation.</returns>
        public IList<DE.IAffiliation> SelectAffiliations(IList<DE.IAffiliation> selectedAffiliations)
        {
            ReadOnlyCollection<DE.IAffiliation> affiliations = null;

            AffiliationConfirmation affiliationConfirmation = new AffiliationConfirmation();

            if (selectedAffiliations != null)
            {
                affiliationConfirmation.SelectedAffiliations = new ReadOnlyCollection<object>(selectedAffiliations.Cast<object>().ToList());
            }

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(affiliationConfirmation, () =>
            {
                if (affiliationConfirmation.Confirmed && affiliationConfirmation.AffiliationResults != null)
                {
                    affiliations = new ReadOnlyCollection<DE.IAffiliation>(affiliationConfirmation.AffiliationResults.Cast<DE.IAffiliation>().ToList());
                }
            });

            Application.Services.Interaction.InteractionRequest(request);

            return affiliations;
        }

        /// <summary>
        /// Return true if add affiliations to transaction successfully.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public bool AddAffiliationsRequest(DE.IRetailTransaction retailTransaction, IList<DE.IAffiliation> affiliations)
        {
            try
            {
                LogMessage("Adding a Affiliation record to the transaction...",
                    LSRetailPosis.LogTraceLevel.Trace,
                    "Affiliation.AddAffiliationItem");

                this.transaction = (RetailTransaction)retailTransaction;

                if (this.transaction == null || affiliations == null)
                {
                    throw new ArgumentNullException();
                }

                foreach (DE.IAffiliation affiliation in affiliations)
                {
                    //The affiliation will be skipped if it is already in the transaction.
                    if (!this.transaction.AffiliationLines.Any(a => a.RecId == affiliation.RecId))
                    {
                        AffiliationItem affiliationItem = (AffiliationItem)affiliation;
                        affiliationItem.transaction = this.transaction;
                        transaction.AffiliationLines.AddLast(affiliationItem);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                NetTracer.Error(ex, "Affiliation::AddAffiliationRequest failed for retailTransaction {0}", retailTransaction.TransactionId);
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Log a message to the file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="traceLevel"></param>
        /// <param name="args"></param>
        private void LogMessage(string message, LogTraceLevel traceLevel, params object[] args)
        {
            ApplicationLog.Log(this.GetType().Name, string.Format(message, args), traceLevel);
        }
    }
}
