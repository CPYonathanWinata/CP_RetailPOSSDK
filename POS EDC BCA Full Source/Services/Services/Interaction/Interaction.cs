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
using GME_Custom;
using GME_Custom.GME_Propesties;

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
                case "frmTestConmfirm":
                    HandleFormTest1ConfirmationInteraction(e);
                    break;
                case "frmCustInfoConfirm":
                    HandleFormCustInfoConfirmationInteraction(e);
                    break;
                case "frmCustTypeConfirm":
                    HandleFormCustomerTypeConfirmationInteraction(e);
                    break;
                case "frmFamilyMemberConfirm":
                    HandleFormFamilyMemberConfirmationInteraction(e);
                    break;
                case "frmBonManualConfirm":
                    HandleFormBonManualConfirmationInteraction(e);                    
                    break;
                case "frmPublicConfirm":
                    HandleFormPublicConfirmationInteraction(e);
                    break;
                case "frmLYBCMemberConfirm":
                    HandleFormLYBCMemberConfirmationInteraction(e);
                    break;
                //Added by Adhi 
                case "frmFamilyPurchaseConfirm":
                    HandleFormTest3ConfirmationInteraction(e);
                    break;
                //Added by Adhi 
                case "frmEmployeePurchaseConfirm":
                    HandleFormTest2ConfirmationInteraction(e);
                    break;
                /// ADD  BY MARIA- WCS
                case "frmFanBOBBconfirm":
                    HandleFormFanBOBBInteraction(e);
                    break;
                case "frmEnrollmentConfirm":
                    HandleFormEnrollmentInteraction(e);
                    break;
                //Add by Adhi (Form Sales Person)
                case "frmSalesPersonConfirm":
                    HandleFormSalesPersonConfirmationInteraction(e);
                    break;
                /// ADD BY MARIA --- FORM DONASI
                case "FrmDonasiConfirm":
                    HandleFormDonasiConfirmationInteraction(e);
                    break;
                /// END FORM DONASI
                case "ProductSearchAttributesFilterConfirmation":
                    HandleProductSearchAttributesFilterInteraction(e);
                    break;
                /// ADD BY MARIA --- FORM Card Replacement
                case "FrmCardReplacementConfirm":
                    HandleFormCardReplacementConfirmationInteraction(e);
                    break;
                /// ADD BY MARIA --- FORM UnprintVoucher
                case "FrmUnprintVoucherConfirm":
                    HandleFormUnprintedVoucherConfirmationInteraction(e);
                    break;
                case "frmUpdateCustomerConfirm":
                    HandleFormUpdateCustomerConfirmationInteraction(e);
                    break;
                case "frmPayCardOfflineApprovalCode":
                    HandleFormPayCardOfflineApprovalConfirmationInteraction(e);
                    break;
                case "frmEnrollmentLYBCConfirm":
                    HandleFormEnrollmentLYBCConfirmationInteraction(e);
                    break;
                case "FrmBirthdayVoucherConfirm":
                    HandleFormBirthdayVoucherConfirmationInteraction(e);
                    break;
                //Add by Rizki
                case "frmItemVoucherConfirm":
                    HandleItemVoucherConfirmationInteraction(e);
                    break;
                //Add by Adhi (Form TBSI Voucher Baru)                
                case "frmTBSIVoucherBaruConfirm":
                    HandleFormTBSIVoucherConfirmationBaruInteraction(e);
                    break;
                //Add by Adhi (Form TBSI Voucher Lama)                
                case "frmTBSIVoucherLamaConfirm":
                    HandleFormTBSIVoucherLamaConfirmationInteraction(e);
                    break;
                //Add by Adhi (Form Welcome Voucher)                
                case "FrmWelcomeVoucherConfirm":
                    HandleFormWelcomeVoucherConfirmationInteraction(e);
                    break;
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
                case "PrintLabelReportNotification":
                    HandlePrintLabelReportNotification(e);
                    break;
                case "IncomeExpenseAccountsConfirmation":
                    HandleIncomeExpenseAccountsConfirmation(e);
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

        //private void HandlePayCashConfirmationInteraction(InteractionRequestedEventArgs e)
        //{
        //    if (e == null)
        //    {
        //        throw new ArgumentNullException("InteractionRequestedEventArgs");
        //    }

        //    PayCashConfirmation context = (PayCashConfirmation)e.Context;
        //    PayCashConfirmation results = InvokeInteraction<PayCashConfirmation, PayCashConfirmation>("PayCashForm", context, true);

        //    if (results != null)
        //    {
        //        context.Confirmed = results.Confirmed;
        //        context.RegisteredAmount = results.RegisteredAmount;
        //        context.OperationDone = results.OperationDone;
        //    }
        //}

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

        private void HandleFormBirthdayVoucherConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm context = (GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm)e.Context;
            GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm, GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm>("FrmBirthdayVoucher", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
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

        private void HandlePrintLabelReportNotification(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            PrintLabelReportNotification context = (PrintLabelReportNotification)e.Context;
            InvokeInteraction<PrintLabelReportNotification, PrintLabelReportNotification>("PrintLabelReportForm", context, true);
        }

        private void HandleProductSearchAttributesFilterInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            ProductSearchAttributesFilterConfirmation context = (ProductSearchAttributesFilterConfirmation)e.Context;
            ProductSearchAttributesFilterConfirmation results = InvokeInteraction<ProductSearchAttributesFilterConfirmation, ProductSearchAttributesFilterConfirmation>("ProductSearchAttributesFilter", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.SearchFilter = results.SearchFilter;
                context.SearchFilterCache = results.SearchFilterCache;
            }

        }

        private void HandleIncomeExpenseAccountsConfirmation(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            IncomeExpenseAccountsConfirmation context = (IncomeExpenseAccountsConfirmation)e.Context;
            IncomeExpenseAccountsConfirmation results = InvokeInteraction<IncomeExpenseAccountsConfirmation, IncomeExpenseAccountsConfirmation>("frmIncomeExpenseAccounts", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.SelectedAccountNumber = results.SelectedAccountNumber;
            }
        }

        //Add by Adhi (Welcome Voucher)
        private void HandleFormWelcomeVoucherConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }
            //frmSalesPersonConfirm ambil dari frmWelcomeVoucherConfirm.cs (9)
            //formSalesPerson ambil dari frmWelcomeVoucher.cs (22)
            GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm context = (GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm)e.Context;
            GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm, GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm>("FrmWelcomeVoucher", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }
        #endregion

        #region GME Interaction handlers
        private void HandleFormTestConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            IPMS_Custom.GME_Propesties.frmTestConfirm context = (IPMS_Custom.GME_Propesties.frmTestConfirm)e.Context;
            IPMS_Custom.GME_Propesties.frmTestConfirm result = InvokeInteraction<IPMS_Custom.GME_Propesties.frmTestConfirm, IPMS_Custom.GME_Propesties.frmTestConfirm>("FormTest", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandleFormCustInfoConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmCustInfoConfirm context = (GME_Custom.GME_Propesties.frmCustInfoConfirm)e.Context;
            GME_Custom.GME_Propesties.frmCustInfoConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmCustInfoConfirm, GME_Custom.GME_Propesties.frmCustInfoConfirm>("FormCustInfo", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandleFormCustomerTypeConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmCustTypeConfirm context = (GME_Custom.GME_Propesties.frmCustTypeConfirm)e.Context;
            GME_Custom.GME_Propesties.frmCustTypeConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmCustTypeConfirm, GME_Custom.GME_Propesties.frmCustTypeConfirm>("FormCustType", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandleFormFamilyMemberConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmFamilyMemberConfirm context = (GME_Custom.GME_Propesties.frmFamilyMemberConfirm)e.Context;
            GME_Custom.GME_Propesties.frmFamilyMemberConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmFamilyMemberConfirm, GME_Custom.GME_Propesties.frmFamilyMemberConfirm>("FormFamilyMember", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandleFormBonManualConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmBonManualConfirm context = (GME_Custom.GME_Propesties.frmBonManualConfirm)e.Context;
            GME_Custom.GME_Propesties.frmBonManualConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmBonManualConfirm, GME_Custom.GME_Propesties.frmBonManualConfirm>("FormBonManual", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandlePayCashConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            PayCashConfirmation context = (PayCashConfirmation)e.Context;
            PayCashConfirmation results = InvokeInteraction<PayCashConfirmation, PayCashConfirmation>("FormPaymentCash", context, true);

            if (results != null)
            {
                context.Confirmed = results.Confirmed;
                context.RegisteredAmount = results.RegisteredAmount;
                context.OperationDone = results.OperationDone;
            }
        }

        private void HandleFormPublicConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmPublicConfirm context = (GME_Custom.GME_Propesties.frmPublicConfirm)e.Context;
            GME_Custom.GME_Propesties.frmPublicConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmPublicConfirm, GME_Custom.GME_Propesties.frmPublicConfirm>("FormPublic", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
                context.custEmail = result.custEmail;
            }
        }

        private void HandleFormLYBCMemberConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmLYBCMemberConfirm context = (GME_Custom.GME_Propesties.frmLYBCMemberConfirm)e.Context;
            GME_Custom.GME_Propesties.frmLYBCMemberConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmLYBCMemberConfirm, GME_Custom.GME_Propesties.frmLYBCMemberConfirm>("FormLYBCMember", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandleFormTest1ConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmTestConmfirm context = (GME_Custom.GME_Propesties.frmTestConmfirm)e.Context;
            GME_Custom.GME_Propesties.frmTestConmfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmTestConmfirm, GME_Custom.GME_Propesties.frmTestConmfirm>("FormTest", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        //Added by Adhi 
        private void HandleFormTest2ConfirmationInteraction(InteractionRequestedEventArgs e)//copas dari (701)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm context = (GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm)e.Context;
            GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm, GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm>("formEmployeePurchase", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        //Added by Adhi 
        private void HandleFormTest3ConfirmationInteraction(InteractionRequestedEventArgs e)//copas dari (701)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm context = (GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm)e.Context;
            GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm, GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm>("formFamilyPurchase", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        /// ADD  BY MARIA- WCS   BBOB FORM
        private void HandleFormFanBOBBInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmFanBOBBconfirm context = (GME_Custom.GME_Propesties.frmFanBOBBconfirm)e.Context;
            GME_Custom.GME_Propesties.frmFanBOBBconfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmFanBOBBconfirm, GME_Custom.GME_Propesties.frmFanBOBBconfirm>("FrmFanBOBB", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }
        /// END
        
        private  void HandleFormEnrollmentInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmEnrollmentConfirm context = (GME_Custom.GME_Propesties.frmEnrollmentConfirm)e.Context;
            GME_Custom.GME_Propesties.frmEnrollmentConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmEnrollmentConfirm, GME_Custom.GME_Propesties.frmEnrollmentConfirm>("FormEnrollment", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        //Add by Adhi (Form Sales Person)
        private void HandleFormSalesPersonConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.frmSalesPersonConfirm context = (GME_Custom.GME_Propesties.frmSalesPersonConfirm)e.Context;
            GME_Custom.GME_Propesties.frmSalesPersonConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.frmSalesPersonConfirm, GME_Custom.GME_Propesties.frmSalesPersonConfirm>("formSalesPerson", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        /// ADD  BY MARIA- WCS  DONASI FORM
        private void HandleFormDonasiConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            FrmDonasiConfirm context = (FrmDonasiConfirm)e.Context;
            FrmDonasiConfirm result = InvokeInteraction<FrmDonasiConfirm, FrmDonasiConfirm>("FrmDonasi", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }
        /// END DONASI FORM

        /// ADD  BY MARIA- WCS  Card Replacemnt FORM
        private void HandleFormCardReplacementConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            GME_Custom.GME_Propesties.FrmCardReplacementConfirm context = (GME_Custom.GME_Propesties.FrmCardReplacementConfirm)e.Context;
            GME_Custom.GME_Propesties.FrmCardReplacementConfirm result = InvokeInteraction<GME_Custom.GME_Propesties.FrmCardReplacementConfirm, GME_Custom.GME_Propesties.FrmCardReplacementConfirm>("FrmCardReplacement", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }
        /// END  Card Replacemnt FORM
        
        /// ADD  BY MARIA- WCS  UNPRINTED VOUCHER FORM
        private void HandleFormUnprintedVoucherConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            FrmUnprintVoucherConfirm context = (FrmUnprintVoucherConfirm)e.Context;
            FrmUnprintVoucherConfirm result = InvokeInteraction<FrmUnprintVoucherConfirm, FrmUnprintVoucherConfirm>("FrmUnprintVoucher", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }
        /// END  UNPRINTED VOUCHER FORM
        
        private void HandleFormUpdateCustomerConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            frmUpdateCustomerConfirm context = (frmUpdateCustomerConfirm)e.Context;
            frmUpdateCustomerConfirm result = InvokeInteraction<frmUpdateCustomerConfirm, frmUpdateCustomerConfirm>("FormUpdateCustomer", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        private void HandleFormPayCardOfflineApprovalConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            frmPayCardOfflineApprovalCode context = (frmPayCardOfflineApprovalCode)e.Context;
            frmPayCardOfflineApprovalCode result = InvokeInteraction<frmPayCardOfflineApprovalCode, frmPayCardOfflineApprovalCode>("FormPayCardOfflineApproval", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
                context.approvalCode = result.approvalCode;
            }
        }

        private void HandleFormEnrollmentLYBCConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }

            frmEnrollmentLYBCConfirm context = (frmEnrollmentLYBCConfirm)e.Context;
            frmEnrollmentLYBCConfirm result = InvokeInteraction<frmEnrollmentLYBCConfirm, frmEnrollmentLYBCConfirm>("FormEnrollmentLYBC", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        //add by rizki
        private void HandleItemVoucherConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("InteractionRequestedEventArgs");
            }

            frmItemVoucherConfirm context = (frmItemVoucherConfirm)e.Context;
            frmItemVoucherConfirm result = InvokeInteraction<frmItemVoucherConfirm, frmItemVoucherConfirm>("FrmItemVoucher", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        //Add by Adhi (TBSI Voucher Baru)
        private void HandleFormTBSIVoucherConfirmationBaruInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }
            frmTBSIVoucherBaruConfirm context = (frmTBSIVoucherBaruConfirm)e.Context;
            frmTBSIVoucherBaruConfirm result = InvokeInteraction<frmTBSIVoucherBaruConfirm, frmTBSIVoucherBaruConfirm>("FrmTBSIVoucherBaru", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }

        //Add by Adhi (TBSI Voucher Lama)
        private void HandleFormTBSIVoucherLamaConfirmationInteraction(InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentException("InteractionRequestedEventArgs");
            }
            frmTBSIVoucherLamaConfirm context = (frmTBSIVoucherLamaConfirm)e.Context;
            frmTBSIVoucherLamaConfirm result = InvokeInteraction<frmTBSIVoucherLamaConfirm, frmTBSIVoucherLamaConfirm>("FrmTBSIVoucherLama", context, true);

            if (result != null)
            {
                context.Confirmed = result.Confirmed;
            }
        }       
        #endregion
    }
}