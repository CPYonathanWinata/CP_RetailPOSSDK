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
using LSRetailPosis.DataAccess;
using LSRetailPosis.DataAccess.DataUtil;
using LSRetailPosis.POSControls;
using LSRetailPosis.POSControls.Touch;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Diagnostics;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using System.Data.SqlClient;
using System.Data;

namespace Microsoft.Dynamics.Retail.Pos.EOD
{
    [Export(typeof(IEOD))]
    public class EOD : IEOD
    {
        // Get all text through the Translation function in the ApplicationLocalizer
        //
        // TextID's for the EOD service are reserved at 51300 - 52299
        // In use now are ID's: 51315

        #region Fields

        private const int AllowMultipleShiftLogOnPermission = 1019;
        private const string CleanUpRetailLabelChangeJournalTransProcedure = "[dbo].[CLEANUP_RETAILLABELCHANGEJOURNALTRANS]";
        private const string StoreIDParameterCRT = "@STOREID";
        private const string DataareaIDParameterCRT = "@DATAAREAID";
        private const string ExpirationDaysParameterCRT = "@EXPIRATIONDAYS";

        //add by Yonathan 16102024
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        string selectedBatchId ;
        string selectedTerminalId ;
        long batchId = 0;
        //end

        #endregion

        #region Properties

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
                InternalApplication = value;
            }
        }

        /// <summary>
        /// Gets or sets the static IApplication instance.
        /// </summary>
        internal static IApplication InternalApplication { get; private set; }

        #endregion

        #region IEOD Members

        /// <summary>
        /// Open a shift.
        /// </summary>
        /// <param name="shift">Opened shift reference. null if shift was not opened (Non-drawer mode).</param>
        /// <returns>True if user selected any drawer or non-drawer mode, false if user canceled the operation</returns>
        public bool OpenShift(ref IPosBatchStaging shift)
        {
            string operatorId = ApplicationSettings.Terminal.TerminalOperator.OperatorId;

            // If already opened shift in memory belongs to logged on user (or assigned to user), then just return back.
            if (shift != null && (operatorId.Equals(shift.StaffId, StringComparison.OrdinalIgnoreCase) || ShiftUsersCache.Contains(shift, operatorId)))
            {
                return true; // return without any interaction.
            }

            BatchData batchData = new BatchData(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);
            IList<IPosBatchStaging> openedShifts = batchData.GetOpenedPosBatchesForTerminal(ApplicationSettings.Terminal.TerminalId);
            int maxSupporedOpenedShifts = Math.Max(1, GetAvailableDrawers().Count()); // Minimum 1 shift is supported even no cash drawer available
            int currentOpenedShifts = openedShifts != null ? openedShifts.Count : 0;
            bool canOpenMoreShifts = (currentOpenedShifts < maxSupporedOpenedShifts);
            bool allowMultipleShiftLogons = false; // Allow access to the shifts from other users.
            bool allowMultipleLogons = false; // Allow multiple logons (hence multiple open shifts)
            ShiftActionResult shiftActionResult = ShiftActionResult.None;
            IList<IPosBatchStaging> suspendedShifts = null;

            // Try finding opened batch from database.
            if (currentOpenedShifts > 0
                && (shift = openedShifts.FirstOrDefault(s => operatorId.Equals(s.StaffId, StringComparison.OrdinalIgnoreCase))) != null)
            {
                return true; // Open shift found, return without any interaction.
            }

            GetPermissions(ref allowMultipleLogons, ref allowMultipleShiftLogons);

            if (canOpenMoreShifts) // Have vacant drawers on current terminal
            {
                IPosBatchStaging shiftOnAnotherTerminal = batchData.GetPosBatchesWithStatus(PosBatchStatus.Open, operatorId).FirstOrDefault();

                if ((!allowMultipleLogons) && (shiftOnAnotherTerminal != null))
                {
                    // User is not allowed for multiple logons and
                    // owns a open shift on another terminal. Ask for action (Non-Drawer or cancel)
                    shiftActionResult = ShowShiftActionForm(51305, false, false, false, shiftOnAnotherTerminal.OpenedAtTerminal);
                }
                else
                {
                    bool canUseOpenedShift = currentOpenedShifts > 0 && allowMultipleShiftLogons;

                    suspendedShifts = batchData.GetPosBatchesWithStatus(PosBatchStatus.Suspended, allowMultipleShiftLogons ? null : operatorId);

                    // If there are suspended shifts?
                    if ((suspendedShifts.Count > 0) && (!Application.Settings.Database.IsOffline))
                    {
                        // And user with multiple shift permission is logging in then prompt for action.
                        if (allowMultipleShiftLogons)
                        {
                            shiftActionResult = ShowShiftActionForm(51306, true, true, canUseOpenedShift);
                        }
                        else
                        {
                            // and user without multiple shift permssions is logging in
                            shiftActionResult = ShowShiftActionForm(51307, false, true, canUseOpenedShift);
                        }
                    }
                    else
                    {
                        // A shift is not currently open on this register.
                        shiftActionResult = ShowShiftActionForm(51306, true, false, canUseOpenedShift);
                    }
                }
            }
            else // No more shifts can be opened on this register
            { 
                if (allowMultipleShiftLogons)
                {
                    // User allowed for multiple shifts (Use existing, Non-Drawer or cancel)
                    shiftActionResult = ShowShiftActionForm(51309, false, false, true);
                }
                else
                {
                    // Ask for action (Non-Drawer or cancel)
                    shiftActionResult = ShowShiftActionForm(51304, false, false, false);
                }
            }

            return ProcessShiftAction(shiftActionResult, openedShifts, suspendedShifts, ref shift);
        }

        /// <summary>
        /// Closes the current shift and print it as Z-Report.
        /// </summary>
        /// <param name="transaction">The current transaction instance.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification="Grandfathered")]
        public void CloseShift(IPosTransaction transaction)
        {
            //add by Yonathan CP_MDFCLOSESHIFT 04032025
            string storedByValue = "";
            if (transaction == null)
            {
                NetTracer.Warning("transaction parameter is null");
                throw new ArgumentNullException("transaction");
            }


            
            Batch batch = null;
             
            // Are you sure you want to close the shift ?
            if (this.Application.Services.Dialog.ShowMessage(51302, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                batch = new Batch(transaction.Shift);

                // Verify if all offline transacitons has been uploaded.
                if (!batch.VerifyOfflineTransactions())
                {
                    batch = null;
                }
            }

            //add a new form to choose closed by and stored by when closing shift
            if (batch != null)
            {
                //add by Yonathan CP_MDFCLOSESHIFT 04032025
                CPCloseShiftUser closeShift = new CPCloseShiftUser();
                if (closeShift.ShowDialog() == DialogResult.OK)  
                {
                    storedByValue = closeShift.StoredBy;
                    //MessageBox.Show("Stored By: " + storedByValue);  // Example usage
                }
            }


            // Calculate and verify amounts.
            if (batch != null)
            {
                // Calculate batch in background
                POSFormsManager.ShowPOSMessageWithBackgroundWorker(51303, delegate { batch.Calculate(); });

                Action<decimal, int, int> verifyAmount = delegate(decimal amount, int errorMsg, int warningMsg)
                {
                    if (amount == 0)
                    {
                        // Warning or error based on configration in HQ.
                        if ((Functions.RequireAmountDeclaration
                            && this.Application.Services.Dialog.ShowMessage(errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
                            || (this.Application.Services.Dialog.ShowMessage(warningMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No))
                        {
                            batch = null;
                        }
                    }
                };

                // Verify starting amounts.
                if (batch != null)
                    verifyAmount(batch.StartingAmountTotal, 51344, 51343);

                // Verify tender delcartion.
                if (batch != null)
                    verifyAmount(batch.DeclareTenderAmountTotal, 51346, 51345);
            }

            if (batch != null &&
                EOD.InternalApplication.Services.EFTServiceOperations.IsSupported())
            {
                bool runBankTotalsVerification = Functions.EODBankTotalsVerification;

                if (!runBankTotalsVerification)
                {
                    // Dialog is shown: Do you want to generate bank total verification?
                    runBankTotalsVerification = this.Application.Services.Dialog.ShowMessage(105011, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                }

                if (runBankTotalsVerification)
                {
                    try
                    {
                        EOD.InternalApplication.Services.EFTServiceOperations.RunTotalsVerification(transaction);
                    }
                    catch (EFTServiceOperationException ex)
                    {
                        NetTracer.Error(ex, "An error occured during the Bank Totals Verification operation");

                        // Bank total verification has not been performed. Do you want to close this shift anyway?
                        if (this.Application.Services.Dialog.ShowMessage(105010, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            batch = null;
                        }
                    }
                }
            }

            if (batch != null &&
                EOD.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled() &&
                Functions.CountryRegion == SupportedCountryRegion.RU)
            {
                try
                {
                    //print z report
                    EOD.InternalApplication.Services.Peripherals.FiscalPrinter.PrintZReport(transaction);
                }
                catch
                {
                    if (!EOD.InternalApplication.Services.Peripherals.FiscalPrinter.IsPrintingCommandSent)
                    {
                        batch = null;
                    }
                }
            }

            // Close the batch and Print Z report if everything is ok.
            if (batch != null)
            {
                batch.Status = PosBatchStatus.Closed;
                batch.CloseDateTime = DateTime.Now;
                batch.ClosedAtTerminal = ApplicationSettings.Terminal.TerminalId;

                BatchData batchData = new BatchData(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);
                batchData.CloseBatch(batch);
                transaction.Shift.Status = PosBatchStatus.Closed;

                //close shift
                updateCustomBatchTable(batch, storedByValue);
                //

                ShiftUsersCache.Remove(transaction.Shift);

                // Check the Print X/Z report on POS function whether is available. 
                if (Functions.PrintXZreportOnPos)
                {
                    // Print Z report if user has permissions.
                    IUserAccessSystem userAccessSystem = Application.BusinessLogic.UserAccessSystem;

                    if (userAccessSystem.UserHasAccess(ApplicationSettings.Terminal.TerminalOperator.OperatorId, PosisOperations.PrintZ))
                    {
                        POSFormsManager.ShowPOSMessageWithBackgroundWorker(99, delegate { batch.Print(ReportType.ZReport); });
                    }
                }

                int daysToPurge = Functions.DaysTransactionExists;
                
                if (daysToPurge > 0)
                {   
                    SqlStoredProcedure sqlStoredProcedure = new SqlStoredProcedure(EOD.CleanUpRetailLabelChangeJournalTransProcedure);
                    sqlStoredProcedure.Add(EOD.StoreIDParameterCRT, ApplicationSettings.Terminal.StoreId);
                    sqlStoredProcedure.Add(EOD.DataareaIDParameterCRT, ApplicationSettings.Database.DATAAREAID);
                    sqlStoredProcedure.Add(EOD.ExpirationDaysParameterCRT, daysToPurge);
                    new DBUtil(ApplicationSettings.Database.LocalConnection).Execute(sqlStoredProcedure);
                }

                //restart async client service to trigger the upload RPF
            }
            else
            {
                NetTracer.Information("Setting status of the transaction to 'cancelled'");
                ((PosTransaction)transaction).EntryStatus = PosTransaction.TransactionStatus.Cancelled;
            }
        }

        private void updateCustomBatchTable(Batch _batch  , string _setorBy)
        {

            //ADD BY ERWIN
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            var tenderID = new List<string>();


            try
            {
                //to do change to RETAILTRANSACTIONPAYMENTTRANS FOR TENDERID DISTINCT YONATHAN
                string queryString = @"
                INSERT INTO [ax].[CPRETAILPOSBATCHTABLEEXTEND]
                    ([BATCHID], [CLOSEBY], [SETORBY], [OPENBY], [STOREID], [TERMINALID], [DATAAREAID],[PARTITION])
                VALUES
                    (@BatchId, @CloseBy, @SetorBy, @OpenBy, @StoreId, @TerminalId, @DataAreaId, @PARTITION)";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@BatchId", _batch.BatchId);
                    command.Parameters.AddWithValue("@CloseBy", ApplicationSettings.Terminal.TerminalOperator.OperatorId);
                    command.Parameters.AddWithValue("@SetorBy", _setorBy);
                    command.Parameters.AddWithValue("@OpenBy", _batch.StaffId);
                    command.Parameters.AddWithValue("@StoreId", _batch.StoreId);
                    command.Parameters.AddWithValue("@TerminalId", _batch.TerminalId);
                    command.Parameters.AddWithValue("@DataAreaId", Application.Settings.Database.DataAreaID);
                    command.Parameters.AddWithValue("@PARTITION", 1);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
//             // Establish the database connection
//            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;

//            try
//            {
//                // Define the SQL UPDATE query
//                string queryString = @"
//                    UPDATE [ax].[CPRETAILPOSBATCHTABLEEXTEND]
//                    SET [CLOSEBY] = @CloseBy,
//                        [SETORBY] = @SetorBy
//                    WHERE [BATCHID] = @BatchId";
//                //
//                using (SqlCommand command = new SqlCommand(queryString, connection))
//                {
//                    // Set parameter values for the UPDATE statement
//                    command.Parameters.AddWithValue("@BatchId", _batch.BatchId);
//                    command.Parameters.AddWithValue("@CloseBy", ApplicationSettings.Terminal.TerminalOperator.OperatorId);
//                    command.Parameters.AddWithValue("@SetorBy", _setorBy);
//                    // Open the connection if it�s not already open
//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }

//                    // Execute the UPDATE command
//                    int rowsAffected = command.ExecuteNonQuery();
                     
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions if necessary
//                throw;
//            }
//            finally
//            {
//                // Ensure the connection is closed
//                if (connection.State != ConnectionState.Closed)
//                {
//                    connection.Close();
//                }
//            }
        }

        /// <summary>
        /// Suspend the current batch.
        /// </summary>
        /// <param name="transaction">The current transaction instance.</param>
        public void SuspendShift(IPosTransaction transaction)
        {
            if (transaction == null)
            {
                NetTracer.Warning("transaction parameter is null");
                throw new ArgumentNullException("transaction");
            }

            BatchData batchData = new BatchData(Application.Settings.Database.Connection,
                Application.Settings.Database.DataAreaID);

            transaction.Shift.OpenedAtTerminal = string.Empty;
            transaction.Shift.CashDrawer = string.Empty;
            transaction.Shift.Status = PosBatchStatus.Suspended;
            transaction.Shift.StatusDateTime = DateTime.Now;

            batchData.UpdateBatch(transaction.Shift);
            ShiftUsersCache.Remove(transaction.Shift);
            transaction.Shift.Print();
        }

        /// <summary>
        /// Blind closes the current shift.
        /// </summary>
        /// <param name="transaction">The current transaction instance.</param>
        public void BlindCloseShift(IPosTransaction transaction)
        {
            if (transaction == null)
            {
                NetTracer.Warning("transaction parameter is null");
                throw new ArgumentNullException("transaction");
            }

            // Are you sure you want to blind close the batch?
            DialogResult dialogResult = this.Application.Services.Dialog.ShowMessage(51308, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            {
                if (dialogResult == DialogResult.Yes)
                {
                    BatchData batchData = new BatchData(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);

                    transaction.Shift.Status = PosBatchStatus.BlindClosed;
                    transaction.Shift.StatusDateTime = DateTime.Now;
                    transaction.Shift.OpenedAtTerminal = string.Empty;
                    transaction.Shift.CashDrawer = string.Empty;

                    batchData.UpdateBatch(transaction.Shift);
                    ShiftUsersCache.Remove(transaction.Shift);
                    transaction.Shift.Print();
                }
                else
                {
                    ((PosTransaction)transaction).EntryStatus = PosTransaction.TransactionStatus.Cancelled;
                }
            }
        }

        /// <summary>
        /// Show blind closed shifts.
        /// </summary>
        /// <param name="transaction">The current transaction instance.</param>
        public void ShowBlindClosedShifts(IPosTransaction transaction)
        {
            if (transaction == null)
            {
                NetTracer.Warning("transaction parameter is null");
                throw new ArgumentNullException("transaction");
            }

            BatchData batchData = new BatchData(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);
            string operatorId = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
            bool multipleShiftLogOn = Application.BusinessLogic.UserAccessSystem.UserHasPermission(operatorId, AllowMultipleShiftLogOnPermission);
            IList<IPosBatchStaging> blindClosedShifts = batchData.GetPosBatchesWithStatus(PosBatchStatus.BlindClosed, multipleShiftLogOn ? null : operatorId);

            using (BlindClosedShiftsForm blindClosedShiftsForm = new BlindClosedShiftsForm(blindClosedShifts))
            {
                POSFormsManager.ShowPOSForm(blindClosedShiftsForm);
            }
        }

        /// <summary>
        /// Print Report for currently opend batch (X-Report)
        /// </summary>
        /// <param name="transaction"></param>
        public void PrintXReport(IPosTransaction transaction)
        {
            ApplicationLog.Log("EOD.PrintXReport", "Printing X report.", LogTraceLevel.Trace);

            if (transaction == null)
            {
                NetTracer.Warning("transaction parameter is null");
                throw new ArgumentNullException("transaction");
            }

            if (EOD.InternalApplication.Services.Peripherals.FiscalPrinter.FiscalPrinterEnabled() &&
                Functions.CountryRegion == SupportedCountryRegion.RU)
            {
                try
                {
                    //print X report
                    EOD.InternalApplication.Services.Peripherals.FiscalPrinter.PrintXReport(transaction);
                }
                catch (Exception x)
                {
                    if (!EOD.InternalApplication.Services.Peripherals.FiscalPrinter.IsPrintingCommandSent)
                    {
                        ApplicationExceptionHandler.HandleException(ToString(), x);
                    }
                }
            }

            // Check the Print X/Z report on POS function whether is available. 
            if (Functions.PrintXZreportOnPos)
            {
                Batch batch = new Batch(transaction.Shift);

                POSFormsManager.ShowPOSMessageWithBackgroundWorker(51303, delegate { batch.Calculate(); }); // Calculating transactions...
                POSFormsManager.ShowPOSMessageWithBackgroundWorker(99, delegate { batch.Print(ReportType.XReport); }); // Printing in progress...
            }
        }

        /// <summary>
        /// Print recently closed batch report (Z-Report)
        /// </summary>
        /// <param name="transaction"></param>
        public void PrintZReport(IPosTransaction transaction)
        {
            selectedBatchId = "";
            batchId = 0;
            ApplicationLog.Log("EOD.PrintZReport", "Printing Z report.", LogTraceLevel.Trace);
            
            
            OpenList(); 
            BatchData batchData = new BatchData(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);

            batchId = selectedBatchId != "" ? Convert.ToInt16(selectedBatchId) : 0;
            //Batch batch = batchData.ReadRecentlyClosedBatch(ApplicationSettings.Terminal.TerminalId);
            if(batchId!=0)
            {
                Batch batch = batchData.ReadBatch(ApplicationSettings.Terminal.TerminalId, batchId);
                if (batch != null)
                {
                    // Print batch.
                    POSFormsManager.ShowPOSMessageWithBackgroundWorker(99, delegate { batch.Print(ReportType.ZReport); });
                }
                else
                {
                    NetTracer.Information("EDO::PrintZReport: batch is null");
                }
            }

            
        }

        private void OpenList()
        {
            string storeId = ApplicationSettings.Database.StoreID; // Pass the store ID dynamically if needed
            
            APIAccess.APIFunction.DatabaseHelper dbHelper = new APIAccess.APIFunction.DatabaseHelper(ApplicationSettings.Database.LocalConnectionString);
            //check additional table first - yonathan 18112024
            string tableName = "ax.CPRETAILPOSBATCHTABLEEXTEND";
            bool exists = dbHelper.CheckExistTable(tableName);

            if (exists)
            {
                using (CPBatchForm batchForm = new CPBatchForm(storeId)) // Open as a temporary form
                {

                    if (batchForm.ShowDialog() == DialogResult.OK)
                    {
                        // Retrieve the selected values from the BatchForm
                        selectedBatchId = batchForm.SelectedBatchId;
                        selectedTerminalId = batchForm.SelectedTerminalId;

                        // Show the selected values (you can use them however you need)
                        //MessageBox.Show("Selected Batch ID: " + selectedBatchId + "\nSelected Terminal ID: " + selectedTerminalId);
                    }


                }
            }
            else
            {
                using (LSRetailPosis.POSProcesses.frmMessage dialog = new LSRetailPosis.POSProcesses.frmMessage(string.Format("Gagal memuat data karena table {0} tidak ditemukan.\nSilakan hubungi IT Support.", tableName), MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);

                }
                

            }

           

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the permissions for the current logged on user.
        /// </summary>
        /// <param name="allowMultipleLogons"></param>
        /// <param name="allowMultipleShiftLogons"></param>
        private static void GetPermissions(ref bool allowMultipleLogons, ref bool allowMultipleShiftLogons)
        {
            const int CHECKED = 1;
            LogonData logonData = new LogonData(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);
            PosPermission userPermission = logonData.GetUserPosPermission(ApplicationSettings.Terminal.StoreId, ApplicationSettings.Terminal.TerminalOperator.OperatorId);

            if (userPermission != null)
            {
                if ((userPermission.ManagerPrivileges == CHECKED)
                    || (userPermission.AllowMultipleLogins == CHECKED))
                {
                    allowMultipleLogons = true;
                }

                if ((userPermission.ManagerPrivileges == CHECKED)
                    || (userPermission.AllowMultipleShiftLogOn == CHECKED))
                {
                    allowMultipleShiftLogons = true;
                }
            }
        }

        /// <summary>
        /// Show the shift action form
        /// </summary>
        /// <param name="message">Message string id</param>
        /// <param name="newButton">Show new button</param>
        /// <param name="resumeButton">Show resume button</param>
        /// <param name="useButton">Show use button</param>
        /// <param name="args">Arguments for message string</param>
        /// <returns></returns>
        private static ShiftActionResult ShowShiftActionForm(int message, bool newButton, bool resumeButton, bool useButton, params object[] args)
        {
            using (ShiftActionForm shiftActionForm = new ShiftActionForm(ApplicationLocalizer.Language.Translate(message, args), newButton, resumeButton, useButton))
            {
                // Form is shown on top of Login form, so should not use POSFormsManager
                shiftActionForm.ShowDialog();
                return shiftActionForm.FormResult;
            }
        }

        /// <summary>
        /// Shows the shift selection form.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="shifts">The shifts.</param>
        /// <returns></returns>
        private static IPosBatchStaging ShowShiftSelectionForm(ShiftSelectionMode mode, IList<IPosBatchStaging> shifts)
        {
            IPosBatchStaging shift = null;

            using (ResumeShiftForm dialog = new ResumeShiftForm(mode, shifts))
            {
                // Form is shown on top of Login form, so should not use POSFormsManager
                dialog.ShowDialog();

                if (dialog.DialogResult == DialogResult.OK)
                {
                    shift = dialog.SelectedShift;
                }
            }

            return shift;
        }

        /// <summary>
        /// Get the cash drawer for shift if available.
        /// </summary>
        /// <param name="openedShifts">The opened shifts.</param>
        /// <param name="cashDrawerName">Name of the cash drawer.</param>
        /// <returns>
        /// True if cash drawer selected or available, false other.
        /// </returns>
        private static bool TryGetCashDrawerForShift(IList<IPosBatchStaging> openedShifts, ref string cashDrawerName)
        {
            bool result = true;
            ICollection<Tuple<string, string>> cashDrawers = GetAvailableDrawers();

            // IF more than one drawer available, then only show the cash drawer selection UI
            if (cashDrawers.Count > 1)
            {
                using (CashDrawerSelectionForm drawerSelectionForm = new CashDrawerSelectionForm(openedShifts))
                {
                    // Form is shown on top of Login form, so should not use POSFormsManager
                    drawerSelectionForm.ShowDialog();

                    if (drawerSelectionForm.DialogResult == DialogResult.OK)
                    {
                        cashDrawerName = drawerSelectionForm.SelectedCashDrawer;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else // Otherwise, get the first one if available.
            {
                Tuple<string, string> drawerInfo = cashDrawers.FirstOrDefault();

                if (drawerInfo != null)
                    cashDrawerName = drawerInfo.Item1; // Drawer name
            }

            return result;
        }

        /// <summary>
        /// Gets the available drawers if CashDrawer V2 service is loaded.
        /// </summary>
        /// <returns>Cash drawer collection.</returns>
        private static ICollection<Tuple<string, string>> GetAvailableDrawers()
        {
            ICollection<Tuple<string, string>> cashDrawers = new Collection<Tuple<string, string>>();

            // If CashDrawer V2 available then only get available drawers.
            if (((object)EOD.InternalApplication.Services.Peripherals.CashDrawer) is ICashDrawerV2)
            {
                cashDrawers = EOD.InternalApplication.Services.Peripherals.CashDrawer.GetAvailableDrawers();
            }

            return cashDrawers;
        }

        /// <summary>
        /// Processes the shift action.
        /// </summary>
        /// <param name="shiftActionResult">The shift action result.</param>
        /// <param name="openedShifts">The opened shifts.</param>
        /// <param name="suspendedShifts">The suspended shifts.</param>
        /// <param name="shift">The shift.</param>
        /// <returns>
        /// True if processed, false if canceled.
        /// </returns>
        private bool ProcessShiftAction(ShiftActionResult shiftActionResult, IList<IPosBatchStaging> openedShifts, IList<IPosBatchStaging> suspendedShifts, ref IPosBatchStaging shift)
        {
            BatchData batchData = new BatchData(Application.Settings.Database.Connection, Application.Settings.Database.DataAreaID);
            bool result = false;
            string cashDrawer = null;

            switch (shiftActionResult)
            {
                case ShiftActionResult.New:

                    if (TryGetCashDrawerForShift(openedShifts, ref cashDrawer))
                    {
                        PosBatchStaging newPosBatch = new PosBatchStaging();

                        newPosBatch.ChannelId = ApplicationSettings.Terminal.StorePrimaryId;
                        newPosBatch.StoreId = ApplicationSettings.Terminal.StoreId;
                        newPosBatch.TerminalId = newPosBatch.OpenedAtTerminal = ApplicationSettings.Terminal.TerminalId;
                        newPosBatch.CashDrawer = cashDrawer;
                        newPosBatch.StaffId = ApplicationSettings.Terminal.TerminalOperator.OperatorId;
                        newPosBatch.StartDateTime = newPosBatch.StatusDateTime = DateTime.Now;
                        newPosBatch.Status = PosBatchStatus.Open;
                        if (!ApplicationSettings.Terminal.TrainingMode) // Don't create shift in traning mode.
                        {
                            newPosBatch.BatchId = Application.Services.ApplicationService.GetAndIncrementTerminalSeed(NumberSequenceSeedType.BatchId);
                            batchData.CreateBatch(newPosBatch);
                        }
                        shift = newPosBatch;
                        //ADD SHIFT OPEN //disable and move to insert when close shift
                        //writeBatchTableExt(newPosBatch);
                        //
                        result = true;
                    }
                    break;

                case ShiftActionResult.Resume:
                    // Let user select the shift to resume.
                    shift = ShowShiftSelectionForm(ShiftSelectionMode.Resume, suspendedShifts);

                    if (shift != null && TryGetCashDrawerForShift(openedShifts, ref cashDrawer))
                    {
                        shift.Status = PosBatchStatus.Open;
                        shift.StatusDateTime = DateTime.Now;
                        shift.OpenedAtTerminal = ApplicationSettings.Terminal.TerminalId;
                        shift.CashDrawer = cashDrawer;
                        if (!ApplicationSettings.Terminal.TrainingMode) // Don't update batch in traning mode.
                        {
                            batchData.UpdateBatch(shift);
                        }

                        result = true;
                    }
                    break;

                case ShiftActionResult.UseOpened:

                    if (openedShifts.Count == 1)
                    {
                        shift = openedShifts.First();
                    }
                    else
                    {
                        // Let user select the opened shift to use.
                        shift = ShowShiftSelectionForm(ShiftSelectionMode.UseOpened, openedShifts);
                    }

                    if (shift != null)
                    {
                        ShiftUsersCache.Add(shift, ApplicationSettings.Terminal.TerminalOperator.OperatorId);
                        result = true;
                    }
                    break;

                case ShiftActionResult.NonDrawer:
                    shift = null;
                    result = true;
                    break;
            }

            return result;
        }

        private void writeBatchTableExt(PosBatchStaging _batch)
        {
            //ADD BY ERWIN
            SqlConnection connection = LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection;
            var tenderID = new List<string>();
 
           
            try
            {
                //to do change to RETAILTRANSACTIONPAYMENTTRANS FOR TENDERID DISTINCT YONATHAN
                string queryString = @"
                INSERT INTO [ax].[CPRETAILPOSBATCHTABLEEXTEND]
                    ([BATCHID], [CLOSEBY], [OPENBY], [STOREID], [TERMINALID], [DATAAREAID],[PARTITION])
                VALUES
                    (@BatchId, @CloseBy, @OpenBy, @StoreId, @TerminalId, @DataAreaId, @PARTITION)";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@BatchId", _batch.BatchId);
                    command.Parameters.AddWithValue("@CloseBy", "");
                    command.Parameters.AddWithValue("@OpenBy", _batch.StaffId);
                    command.Parameters.AddWithValue("@StoreId", _batch.StoreId);
                    command.Parameters.AddWithValue("@TerminalId", _batch.TerminalId);
                    command.Parameters.AddWithValue("@DataAreaId", Application.Settings.Database.DataAreaID);
                    command.Parameters.AddWithValue("@PARTITION", 1);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //LSRetailPosis.ApplicationExceptionHandler.HandleException();
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// Enum to specifies the report type.
    /// </summary>
    internal enum ReportType
    {
        /// <summary>
        /// X-Report
        /// </summary>
        XReport = 1,

        /// <summary>
        /// Z-Report
        /// </summary>
        ZReport = 2
    }
}
