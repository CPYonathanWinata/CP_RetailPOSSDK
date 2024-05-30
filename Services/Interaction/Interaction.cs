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
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Dynamics.Retail.Notification.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    [Export(typeof(IInteraction))]
    public class Interaction : IInteraction
    {
        /// <summary>
        /// IApplication instance.
        /// </summary>
        private IApplication application;

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
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
        /// Used to make updates thread safe.
        /// </summary>
        static readonly object padlock = new object();

        /// <summary>
        /// Catalog that holds the MEF parts that support the interation interface
        /// </summary>
        private static AggregateCatalog catalog;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "We must not dispose of the object as the static container has a dependency on them.")]
        public void InteractionRequest(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }
            lock (padlock)
            {
                if (catalog == null)
                {
                    // Create the catalog of views
                    catalog = new AggregateCatalog();

                    const string servicesFolder = "Services";
                    const string extensionsFolder = "Extensions";
                    const string extensionFolder = "Extension";

                    string appPath = LSRetailPosis.Settings.ApplicationSettings.GetAppPath();
                    string interactionDefaultPath = Path.Combine(appPath, servicesFolder, "InteractionDefaults");
                    string defaultAssembly = Path.Combine(interactionDefaultPath, "InteractionDefaults.dll");

                    AddDirectoryToCatalog(catalog, Path.Combine(appPath, extensionsFolder));
                    AddDirectoryToCatalog(catalog, Path.Combine(appPath, servicesFolder, extensionsFolder));
                    AddDirectoryToCatalog(catalog, Path.Combine(interactionDefaultPath, extensionFolder));

                    catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(defaultAssembly)));
                }
            }

            string name = e.Context.GetType().Name;

            switch (name)
            {
                case "BarcodeConfirmation":
                    HandleBarcodeInteraction(e);
                    break;
                case "ExtendedLogOnConfirmation":
                    HandleExtendedLogOnInteraction(e);
                    break;
                case "DimensionConfirmation":
                    HandleDimensionInteraction(e);
                    break;
                case "InputConfirmation":
                    HandleInputInteraction(e);
                    break;
                case "LogOnConfirmation":
                    HandleLogOnInteraction(e);
                    break;
                case "ManagerAccessConfirmation":
                    HandleManagerAccessInteraction(e);
                    break;
                case "PayCashConfirmation":
                    HandlePayCashConfirmationInteraction(e);
                    break;
                case "PayCurrencyConfirmation":
                    HandleInputConfirmationInteraction(e);
                    break;
                case "PayCustomerAccountConfirmation":
                    HandlePayCustomerAccountConfirmationInteraction(e);
                    break;
                case "ProductInformationConfirmation":
                    HandleProductInformationInteraction(e);
                    break;
                case "ReturnTransactionConfirmation":
                    HandleReturnTransactionConfirmationInteraction(e);
                    break;
                case "RegisterTimeNotification":
                    HandleRegisterTimeNotificationInteraction(e);
                    break;
                case "ViewTimeClockEntriesNotification":
                    HandleViewTimeClockEntriesNotificationInteraction(e);
                    break;
                case "ProductDetailsConfirmation":
                    HandleProductDetailsConfirmation(e);
                    break;
                case "LoyaltyCardConfirmation":
                    HandleLoyaltyCardConfirmationInteraction(e);
                    break;
                case "RedeemLoyaltyPointsConfirmation":
                    HandleRedeemLoyaltyPointsConfirmationInteraction(e);
                    break;

                case "ReportConfirmation":
                    HandleReportConfirmationInteraction(e);
                    break;
                case "KitDisassemblyConfirmation":
                    HandleKitDisassemblyConfirmation(e);
                    break;
                case "LoyaltyCardIssuedConfirmation":
                    HandleLoyaltyIssueCardConfirmation(e);
                    break;
                case "LoyaltyCardPayConfirmation":
                    HandleLoyaltyCardPayConfirmation(e);
                    break;
                case "KitComponentSubstituteConfirmation":
                    HandleKitComponentSubstituteConfirmation(e);
                    break;
                case "AffiliationConfirmation":
                    HandleAffiliationConfirmation(e);
                    break;
                case "DisbursementSlipCreationConfirmation":
                    HandleDisbursementSlipCreationConfirmation(e);
                    break;
                case "SaleRefundDisplayNotification":
                    HandleSaleRefundDisplayNotification(e);
                    break;
                default:
                    throw new InvalidDataException(string.Format("Invalid confirmation name '{0}'.", name));
            }

            e.Callback();
        }

        /// <summary>
        /// Loads and shows the view then returns results.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter passed into view's constructor</typeparam>
        /// <typeparam name="TResults">Type of results returned</typeparam>
        /// <param name="viewName">Name of view to look up</param>
        /// <param name="context">The value of the parameter to pass to the view's constructor</param>
        /// <param name="showDialog">Show dialog when value true otherwise hide dialog</param>
        /// <returns></returns>
        private TResults InvokeInteraction<TParam, TResults>(string viewName, TParam context, bool showDialog)
            where TParam : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
            where TResults : class, new()
        {
            IInteractionView view = null;

            using (CompositionContainer container = new CompositionContainer(catalog))
            {
                // Add context to container for satisfying ImportingConstructor
                container.ComposeExportedValue<TParam>(context);

                // Load the view. If no or more than one view satisfying the condition is found, throws InvalidOperationException
                view = container.GetExportedValues<IInteractionView>(viewName).First();

                // Args to create the form are currently passed to single param ctor. If default ctor is used (without params) uncomment following line:
                // view.Initialize(e.Context); 

                // Show form
                System.Windows.Forms.Form form = (System.Windows.Forms.Form)view;
                if (showDialog)
                {
                    this.Application.ApplicationFramework.POSShowForm(form);
                }

                // Get results
                return view.GetResults<TResults>();
            }
        }

        /// <summary>
        /// Adds a directory as a catalog if it exists.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <param name="directoryPath">The directory path to add to the catalog.</param>
        private static void AddDirectoryToCatalog(AggregateCatalog catalog, string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                catalog.Catalogs.Add(new DirectoryCatalog(directoryPath));
            }
        }

        #region Interaction handlers

        // Dimension form
        private void HandleDimensionInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            DimensionConfirmation context = (DimensionConfirmation)e.Context;
            DimensionConfirmation results = InvokeInteraction<DimensionConfirmation, DimensionConfirmation>("DimensionView", context, context.DisplayDialog);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.SelectDimCombination = results.SelectDimCombination;
            }
        }

        // Barcode form
        private void HandleBarcodeInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            BarcodeConfirmation context = (BarcodeConfirmation)e.Context;
            BarcodeConfirmation results = InvokeInteraction<BarcodeConfirmation, BarcodeConfirmation>("BarcodeForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.SelectedBarcodeId = results.SelectedBarcodeId;
            }
        }

        private void HandleExtendedLogOnInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ExtendedLogOnConfirmation context = (ExtendedLogOnConfirmation)e.Context;
            ExtendedLogOnConfirmation results = InvokeInteraction<ExtendedLogOnConfirmation, ExtendedLogOnConfirmation>("ExtendedLogOnForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
            }
        }

        // Input form
        private void HandleInputInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            InputConfirmation context = (InputConfirmation)e.Context;
            InputConfirmation results = InvokeInteraction<InputConfirmation, InputConfirmation>("InputForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.EnteredText = results.EnteredText;
            }
        }

        private void HandleLogOnInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            LogOnConfirmation context = (LogOnConfirmation)e.Context;
            LogOnConfirmation results = InvokeInteraction<LogOnConfirmation, LogOnConfirmation>("LogOnForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.LogOnStatus = results.LogOnStatus;
            }
        }

        private void HandleManagerAccessInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ManagerAccessConfirmation context = (ManagerAccessConfirmation)e.Context;
            ManagerAccessConfirmation results = InvokeInteraction<ManagerAccessConfirmation, ManagerAccessConfirmation>("ManagerAccessForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.OperatorId = results.OperatorId;
            }
        }

        private void HandleInputConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            PayCurrencyConfirmation context = (PayCurrencyConfirmation)e.Context;
            PayCurrencyConfirmation results = InvokeInteraction<PayCurrencyConfirmation, PayCurrencyConfirmation>("PayCurrencyView", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.RegisteredAmount = results.RegisteredAmount;
                context.ExchangeRate = results.ExchangeRate;
                context.CurrentCurrencyCode = results.CurrentCurrencyCode;
            }
        }

        private void HandlePayCashConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            PayCashConfirmation context = (PayCashConfirmation)e.Context;
            PayCashConfirmation results = InvokeInteraction<PayCashConfirmation, PayCashConfirmation>("PayCashForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.RegisteredAmount = results.RegisteredAmount;
                context.OperationDone = results.OperationDone;
            }
        }

        private void HandleReportConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ReportConfirmation context = (ReportConfirmation)e.Context;
            ReportConfirmation results = InvokeInteraction<ReportConfirmation, ReportConfirmation>("ReportForm", context, true);
            if (results != null)
            {
                context.Success = results.Success;
                context.Confirmed = results.Confirmed;
            }
        }

        private void HandlePayCustomerAccountConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            PayCustomerAccountConfirmation context = (PayCustomerAccountConfirmation)e.Context;
            PayCustomerAccountConfirmation results = InvokeInteraction<PayCustomerAccountConfirmation, PayCustomerAccountConfirmation>("PayCustomerAccountForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.RegisteredAmount = results.RegisteredAmount;
                context.CustomerId = results.CustomerId;
            }
        }

        private void HandleProductInformationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ProductInformationConfirmation context = (ProductInformationConfirmation)e.Context;
            ProductInformationConfirmation results = InvokeInteraction<ProductInformationConfirmation, ProductInformationConfirmation>("ProductInformationForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
            }
        }

        private void HandleReturnTransactionConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ReturnTransactionConfirmation context = (ReturnTransactionConfirmation)e.Context;
            ReturnTransactionConfirmation results = InvokeInteraction<ReturnTransactionConfirmation, ReturnTransactionConfirmation>("ReturnTransactionForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.ReturnedLineNumbers = results.ReturnedLineNumbers;
            }
        }

        private void HandleRegisterTimeNotificationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            RegisterTimeNotification context = (RegisterTimeNotification)e.Context;
            RegisterTimeNotification results = InvokeInteraction<RegisterTimeNotification, RegisterTimeNotification>("RegisterTimeForm", context, true);
        }

        private void HandleViewTimeClockEntriesNotificationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ViewTimeClockEntriesNotification context = (ViewTimeClockEntriesNotification)e.Context;
            ViewTimeClockEntriesNotification results = InvokeInteraction<ViewTimeClockEntriesNotification, ViewTimeClockEntriesNotification>("ViewTimeClockEntriesForm", context, true);
        }



        private void HandleProductDetailsConfirmation(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            ProductDetailsConfirmation context = (ProductDetailsConfirmation)e.Context;
            ProductDetailsConfirmation results = InvokeInteraction<ProductDetailsConfirmation, ProductDetailsConfirmation>("ProductDetailsForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.AddToSale = results.AddToSale;
                context.ResultData = results.ResultData;
            }
        }

        private void HandleLoyaltyCardConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            LoyaltyCardConfirmation context = (LoyaltyCardConfirmation)e.Context;
            LoyaltyCardConfirmation results = InvokeInteraction<LoyaltyCardConfirmation, LoyaltyCardConfirmation>("LoyaltyCardForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
            }
        }

        private void HandleRedeemLoyaltyPointsConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            RedeemLoyaltyPointsConfirmation context = (RedeemLoyaltyPointsConfirmation)e.Context;
            RedeemLoyaltyPointsConfirmation results = InvokeInteraction<RedeemLoyaltyPointsConfirmation, RedeemLoyaltyPointsConfirmation>("RedeemLoyaltyPointsForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.CardNumber = results.CardNumber;
                context.DiscountAmount = results.DiscountAmount;
            }
        }

        private void HandleKitDisassemblyConfirmation(InteractionRequestedEventArgs e)
        {
            KitDisassemblyConfirmation context = (KitDisassemblyConfirmation)e.Context;
            KitDisassemblyConfirmation results = InvokeInteraction<KitDisassemblyConfirmation, KitDisassemblyConfirmation>("KitDisassemblyView", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.KitId = results.KitId;
                context.KitVariantId = results.KitVariantId;
                context.KitQuantity = results.KitQuantity;
                context.CartItems = results.CartItems;
            }
        }

        private void HandleLoyaltyIssueCardConfirmation(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            LoyaltyCardIssuedConfirmation context = (LoyaltyCardIssuedConfirmation)e.Context;
            LoyaltyCardIssuedConfirmation results = InvokeInteraction<LoyaltyCardIssuedConfirmation, LoyaltyCardIssuedConfirmation>("LoyaltyCardIssueForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.LoyaltyCardNumber = results.LoyaltyCardNumber;
            }
        }

        private void HandleLoyaltyCardPayConfirmation(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            LoyaltyCardPayConfirmation context = (LoyaltyCardPayConfirmation)e.Context;
            LoyaltyCardPayConfirmation results = InvokeInteraction<LoyaltyCardPayConfirmation, LoyaltyCardPayConfirmation>("LoyaltyPayForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.LoyaltyCardNumber = results.LoyaltyCardNumber;
                context.RegisteredAmount = results.RegisteredAmount;
            }
        }

        private void HandleKitComponentSubstituteConfirmation(InteractionRequestedEventArgs e)
        {
            KitComponentSubstituteConfirmation context = (KitComponentSubstituteConfirmation)e.Context;
            KitComponentSubstituteConfirmation results = InvokeInteraction<KitComponentSubstituteConfirmation, KitComponentSubstituteConfirmation>("KitComponentSubstituteView", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.SelectedComponentLineRecId = results.SelectedComponentLineRecId;
                context.ResultData = results.ResultData;
            }
        }

        private void HandleAffiliationConfirmation(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            AffiliationConfirmation context = (AffiliationConfirmation)e.Context;
            AffiliationConfirmation results = InvokeInteraction<AffiliationConfirmation, AffiliationConfirmation>("AffiliationForm", context, true);
            
            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.FormType = results.FormType;
                context.AffiliationResults = results.AffiliationResults;
            }
        }

        private void HandleDisbursementSlipCreationConfirmation(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            DisbursementSlipCreationConfirmation context = (DisbursementSlipCreationConfirmation)e.Context;
            DisbursementSlipCreationConfirmation results = InvokeInteraction<DisbursementSlipCreationConfirmation, DisbursementSlipCreationConfirmation>("DisbursementSlipCreationForm", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.DisbursementSlipInfo = results.DisbursementSlipInfo;
            }
        }

        private void HandleSaleRefundDisplayNotification(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            SaleRefundDisplayNotification context = (SaleRefundDisplayNotification)e.Context;
            InvokeInteraction<SaleRefundDisplayNotification, SaleRefundDisplayNotification>("SaleRefundDetailsForm", context, true);
        }
        #endregion
    }
}