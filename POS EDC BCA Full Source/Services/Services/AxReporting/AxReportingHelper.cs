/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Printing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

namespace Microsoft.Dynamics.Retail.Pos.AxReporting
{
    /// <summary>
    /// Helper class for AxReporting service.
    /// </summary>
    internal sealed class AxReportingHelper
    {
        #region Private fields
        
        private PrintTicket currentDefaultPrintTicket = null;
        private string currentDefaultPrinterName = null;

        private readonly string userPrintQueueFileName = null,
            userPrintTicketFileName = null;

        private string archiveLocalFileName = null,
            extractionDirectory = null;

        private static AxReportingHelper instance = new AxReportingHelper();

        private int filesToBePrintedCount = 0;

        #endregion

        #region Public properties

        /// <summary>
        /// Asynchronous task for the print label report operation.
        /// </summary>
        public Task<ILabelReportResponse> TaskPrintLabelReport { get; set; }
        
        /// <summary>
        /// Caller class data.
        /// </summary>
        public object CallerData { get; set; }

        /// <summary>
        /// Gets AxReportingHelper instance.
        /// </summary>
        public static AxReportingHelper Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets or sets the IApplication instance.
        /// </summary>
        public IApplication Application { get; set; }

        /// <summary>
        /// Used to track files that are currently in the process of printing.
        /// </summary>
        private volatile ConcurrentDictionary<string, WebBrowser> filesPrintingQueue;

        #endregion

        private AxReportingHelper() 
        {
            userPrintQueueFileName = string.Format(@"{0}\Services\{1}.PrintQueue.xml", Directory.GetCurrentDirectory(), Assembly.GetExecutingAssembly().GetName().Name);
            userPrintTicketFileName = string.Format(@"{0}\Services\{1}.PrintTicket.xml", Directory.GetCurrentDirectory(), Assembly.GetExecutingAssembly().GetName().Name);
        }

        #region Public methods

        /// <summary>
        /// Sets up printer settings retrieved from either files or print dialog
        /// </summary>
        /// <param name="restoreSavedSettings">Denotes that printer settings should be restored from files.</param>
        /// <returns>True if printer settings were successfully restored, otherwise false.</returns>
        public bool SetupPrinterSettings(bool restoreSavedSettings)
        {
            PrintTicket configuredPrintTicket = null;
            PrintQueue configuredPrintQueue = null;

            currentDefaultPrinterName = null;
            currentDefaultPrintTicket = null;

            using (var localPrintServer = new LocalPrintServer())
            {
                if (restoreSavedSettings)
                {
                    ReadPrintQueueTicketFromFile(out configuredPrintTicket, out configuredPrintQueue, localPrintServer);
                }

                if ((configuredPrintQueue == null) || (configuredPrintTicket == null))
                {
                    if (ApplicationSettings.Terminal.TerminalOperator.AllowSetupLabelPrinter)
                    {
                        if (ShowPrintDialog(out configuredPrintQueue, out configuredPrintTicket))
                        {
                            SavePrintQueueTicketToFile(configuredPrintTicket, configuredPrintQueue);
                        }
                    }
                    else
                    {
                        int messageId = restoreSavedSettings
                            ? 100037 // Printer settings are not available and you are not allowed to set up the printer. Please contact your system administrator.
                            : 100036; // You are not allowed to set up the label printer. Please contact your system administrator.
                        POSFormsManager.ShowPOSErrorDialog(new PosisException(ApplicationLocalizer.Language.Translate(messageId)));
                    }
                }

                if (restoreSavedSettings)
                {
                    if ((configuredPrintQueue != null)
                        && !string.Equals(configuredPrintQueue.FullName, localPrintServer.DefaultPrintQueue.FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        bool defaultPrinterWasSet = SafeNativeMethods.SetDefaultPrinter(configuredPrintQueue.FullName);
                        if (!defaultPrinterWasSet)
                            throw new InvalidOperationException(string.Format(ApplicationLocalizer.Language.Translate(100027), configuredPrintQueue.FullName)); // Unable to set the printer '{0}' as default.

                        currentDefaultPrinterName = localPrintServer.DefaultPrintQueue.FullName;
                        localPrintServer.DefaultPrintQueue = configuredPrintQueue;
                    }

                    if (configuredPrintTicket != null)
                    {
                        using (var defaultPrintQueue = localPrintServer.DefaultPrintQueue)
                        {
                            currentDefaultPrintTicket = defaultPrintQueue.UserPrintTicket;
                            SaveUserPrintQueueTicket(defaultPrintQueue, configuredPrintTicket);
                        }
                    }
                }
            }

            return (configuredPrintQueue != null) && (configuredPrintTicket != null);
        }

        /// <summary>
        /// Runs label report generation and prints them. 
        /// </summary>
        /// <param name="reportDate">Report date.</param>
        /// <param name="labelType">Label type.</param>
        /// <param name="reportFileFormat">Report file format.</param>
        /// <param name="labelReportItemList">List of <c>ILabelReportItem</c></param>
        /// <returns>Label report response.</returns>
        public ILabelReportResponse PrintLabelReport(DateTime reportDate, RetailLabelTypeBase labelType, SRSReportFileFormat reportFileFormat, IEnumerable<ILabelReportItem> labelReportItemList)
        {
            LabelReportResponse response = null;

            try
            {
                response = AxReportingTransactionService.RunLabelReport(reportDate, labelType, reportFileFormat, labelReportItemList);
                if (response.Result && !string.IsNullOrWhiteSpace(response.ReportFile))
                {
                    PrintArchivedFiles(ref response);
                }
            }
            finally
            {
                string archiveServerFileName = null;
                if (response != null)
                {
                    archiveServerFileName = response.ReportFile;
                }
                Task.Run(() => WaitForCompletionOfPrintingAndCleanup(archiveServerFileName))
                    .ContinueWith(t => HandleTaskException(t), TaskContinuationOptions.OnlyOnFaulted);
            }

            return response;
        }

        #endregion

        #region Private methods

        private void PrintArchivedFiles(ref LabelReportResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            if (string.IsNullOrWhiteSpace(response.ReportFile))
            {
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100011), "archiveServerFileName"); // The archive file name on the server is not specified.
            }

            const int defaultChunkSize = 786432; // 3/4 of 1 MB default value - in case parameter is not set up
            int fileChunkSize = LSRetailPosis.Settings.TransactionServicesProfiles.TransactionServiceProfile.TransferFileChunkSize != 0
                ? LSRetailPosis.Settings.TransactionServicesProfiles.TransactionServiceProfile.TransferFileChunkSize
                : defaultChunkSize;

            try
            {
                archiveLocalFileName = GetArchiveLocalFileName();
                extractionDirectory = GetExtractDirectoryName();

                try
                {
                    CopyFile(response.ReportFile, archiveLocalFileName, fileChunkSize);
                }
                catch (Exception ex)
                {
                    UpdateLabelReportResponse(response, false,
                        ApplicationLocalizer.Language.Translate(100005), // An error occurred during the file transfer.
                        string.Format("{0}: {1}: {2}", ApplicationLocalizer.Language.Translate(100002), ApplicationLocalizer.Language.Translate(100005), ex)); // Print label report: An error occurred during the file transfer.:
                    return;
                }

                try
                {
                    ExtractFilesToDirectory(archiveLocalFileName, extractionDirectory);
                }
                catch (Exception ex)
                {
                    UpdateLabelReportResponse(response, false,
                        ApplicationLocalizer.Language.Translate(100006), // An error occurred while extracting files from the archive.
                        string.Format("{0}: {1}: {2}", ApplicationLocalizer.Language.Translate(100002), ApplicationLocalizer.Language.Translate(100006), ex)); // Print label report: An error occurred while extracting files from the archive.:
                    return;
                }

                // Select files with the given extention in all subfolders of the extracted folder.
                var filesToBePrinted = Directory.GetFiles(extractionDirectory, "*.*", SearchOption.AllDirectories);
                if (filesToBePrinted.Length == 0)
                {
                    UpdateLabelReportResponse(response, false,
                        ApplicationLocalizer.Language.Translate(100009), null); // The labels that were generated on the Dynamics AX server and saved in the archive do not contain the files {0} with the specified extension for printing.
                    return;
                }

                InitializeFilesPrintingQueue(filesToBePrinted.Length);
                try
                {
                    LoadFilesForPrinting(filesToBePrinted);
                }
                catch (Exception ex)
                {
                    UpdateLabelReportResponse(response, false,
                        ApplicationLocalizer.Language.Translate(100007), // An error occurred while sending the file to the printer.
                        string.Format("{0}: {1}: {2}", ApplicationLocalizer.Language.Translate(100002), ApplicationLocalizer.Language.Translate(100007), ex)); // Print label report: An error occurred while sending the file to the printer.:
                }
            }
            catch (OutOfMemoryException ex)
            {
                UpdateLabelReportResponse(response, false,
                    ApplicationLocalizer.Language.Translate(100010), // There is not sufficient space available on the hard disk to complete the transaction.
                    string.Format("{0}: {1}", ApplicationLocalizer.Language.Translate(100002), ex.ToString())); // Print label report
            }
            catch (Exception ex)
            {
                UpdateLabelReportResponse(response, false,
                    string.Format("{0} {1}", ApplicationLocalizer.Language.Translate(100004), ApplicationLocalizer.Language.Translate(100008)), // An error occurred during the operation. Contact your administrator for further assistance.
                    string.Format("{0}: {1}: {2}", ApplicationLocalizer.Language.Translate(100002), ApplicationLocalizer.Language.Translate(100004), ex)); // Print label report: An error occurred during the operation.:
            }
        }

        private static void UpdateLabelReportResponse(LabelReportResponse response, bool result, string errorMessage, string logMessage)
        {
            response.Result = result;
            response.InfologContent = logMessage;
            response.ErrorMessage = errorMessage;
        }

        private void WaitForCompletionOfPrintingAndCleanup(string archiveServerFileName)
        {
            try
            {
                WaitForCompletionOfPrinting();
            }
            catch (Exception ex)
            {
                ApplicationLog.Log(ApplicationLocalizer.Language.Translate(100002), string.Format("{0}: {1}", ApplicationLocalizer.Language.Translate(100004), ex), LogTraceLevel.Error); // Print label report: An error occurred during the operation.:
            }
            finally
            {
                CleanupAndRestoreSettings(archiveServerFileName);
            }
        }

        private void CleanupAndRestoreSettings(string archiveServerFileName)
        {
            // Restore original printer settings, i.e., default printer name and a corresponding user print ticket.
            SetPrinterSettings();

            if (!string.IsNullOrWhiteSpace(extractionDirectory) && Directory.Exists(extractionDirectory))
            {
                Directory.Delete(extractionDirectory, true);
            }
            extractionDirectory = null;

            if (!string.IsNullOrWhiteSpace(archiveLocalFileName) && File.Exists(archiveLocalFileName))
            {
                File.Delete(archiveLocalFileName);
            }
            archiveLocalFileName = null;

            AxReportingTransactionService.DeleteFileOnServer(archiveServerFileName);
        }

        private static void HandleTaskException(Task t)
        {
            try
            {
                foreach (var ex in t.Exception.Flatten().InnerExceptions)
                {
                    ApplicationLog.Log(ApplicationLocalizer.Language.Translate(100002), ex.ToString(), LogTraceLevel.Error); // Print label report
                }
            }
            catch (Exception)
            {
                // swallowing exceptions so app code won't fail
            }
        }

        private void LoadFilesForPrinting(string[] filesToBePrinted)
        {
            foreach (var file in filesToBePrinted)
            {
                LoadFileForPrinting(file);
            }
        }

        private void InitializeFilesPrintingQueue(int filesToBePrinted)
        {
            filesToBePrintedCount = filesToBePrinted;
            filesPrintingQueue = new ConcurrentDictionary<string, WebBrowser>();
        }

        private void AddFileToPrintingQueue(string fileName, WebBrowser wb)
        {
            filesPrintingQueue.TryAdd(fileName, wb);
        }

        private void RemoveFileFromPrintingQueue(string fileName)
        {
            WebBrowser wb = null;
            filesPrintingQueue.TryRemove(fileName, out wb);
            filesToBePrintedCount--;
            if (wb != null && !wb.IsDisposed)
                wb.Dispose();
        }

        private bool PrintQueueIsEmpty()
        {
            return filesToBePrintedCount == 0 && (filesPrintingQueue != null ? filesPrintingQueue.IsEmpty : true);
        }

        /// <summary>
        /// Suspends current thread till we consider the printing process done.
        /// </summary>
        /// <remarks>
        /// Printing process via WebBrowser component does not provide us with the information on whether the document is in process of printing or it has already been printed in a synchronous manner.
        /// We do need to understand whether a particular file has been sent to the printer or not as we're going to purge the whole source folder afterwards and we do not want to do it too early.
        /// So we use ConcurrencyDictionary in order to keep track of files that started loading and have not yet been sent to the printer.
        /// This methods implements synchronous poll of the ConncurrencyDictionary at the specified intervals within the specified max idle time.
        /// </remarks>
        private void WaitForCompletionOfPrinting()
        {
            // This should remain a built-in constant.
            const int fileLoadTimeout = 10000, // 10 sec 
                sleepTime = 1000; // 1 sec
            var sw = new Stopwatch();
            while (!PrintQueueIsEmpty())
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }

                if (sw.ElapsedMilliseconds > fileLoadTimeout)
                {
                    throw new TimeoutException(string.Format(ApplicationLocalizer.Language.Translate(100014), fileLoadTimeout)); // Printing  could not be completed within the timeout value of {0} ms.
                }

                if (!sw.IsRunning)
                {
                    sw.Start();
                }

                Thread.Sleep(sleepTime);
            }
        }

        private static string GetArchiveLocalFileName()
        {
            return Path.GetTempFileName();
        }

        private static string GetExtractDirectoryName()
        {
            int tryCount = 10;
            bool directoryExists = false;
            string directoryName = null;
            string tempPath = Path.GetTempPath();

            do
            {
                directoryName = Path.Combine(tempPath, Guid.NewGuid().ToString("N"));
                directoryExists = Directory.Exists(directoryName);
            } while (directoryExists && --tryCount > 0);

            if (directoryExists)
                throw new InvalidOperationException(ApplicationLocalizer.Language.Translate(100015)); // The extraction directory name is not available.

            return directoryName;
        }

        private void CopyFile(string sourceFileName, string destinationFileName, int chunkSize)
        {
            if (string.IsNullOrWhiteSpace(sourceFileName))
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100016), "sourceFileName"); // The source file name is not specified.

            if (string.IsNullOrWhiteSpace(destinationFileName))
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100017), "destinationFileName"); // The destination file name is not specified.

            if (chunkSize <= 0)
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100018), "chunkSize"); // The packet size must be a positive value.

            bool read = true;
            int bytesRead = 0;

            var givenSha256HashBase64 = AxReportingTransactionService.GetFileSha256HashBase64(sourceFileName);
            using (var fs = new FileStream(destinationFileName, FileMode.Create, FileAccess.Write))
            {
                for (long position = 0L; read; position += bytesRead)
                {
                    byte[] bytes;
                    read = AxReportingTransactionService.ReadFilePart(sourceFileName, position, chunkSize, out bytes);
                    bytesRead = bytes.Length;
                    fs.Write(bytes, 0, bytesRead);
                    fs.Flush(true);
                }
            }

            string computedSha256HashBase64 = null;
            using (var fs = new FileStream(destinationFileName, FileMode.Open, FileAccess.Read))
            {
                computedSha256HashBase64 = ComputeSha256HashBase64(fs);
            }

            bool hashesAreEqual = string.Equals(givenSha256HashBase64, computedSha256HashBase64, StringComparison.Ordinal);

            if (!hashesAreEqual)
                throw new InvalidOperationException(ApplicationLocalizer.Language.Translate(100019)); // The file hashes did not match.
        }

        private static void ExtractFilesToDirectory(string zipFileName, string outputDirectory)
        {
            if (string.IsNullOrWhiteSpace(zipFileName))
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100020), "zipFileName"); // The zip file name is not specified.

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100021), "outputDirectory"); // The output directory name is not specified.

            if (!File.Exists(zipFileName))
                throw new FileNotFoundException(string.Format(ApplicationLocalizer.Language.Translate(100025), zipFileName)); // The file {0} does not exist.

            ZipFile.ExtractToDirectory(zipFileName, outputDirectory);
        }

        private static string ComputeSha256HashBase64(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            byte[] hash = null;

            using (var sha256 = new SHA256Managed())
            {
                hash = sha256.ComputeHash(stream);
            }

            string base64Hash = Convert.ToBase64String(hash);

            return base64Hash;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "We need this object alive to correctly handle the event which gets fired on another thread. There we take care of disposing it.")]
        private void LoadFileForPrinting(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(100016), "fileName"); // The source file name is not specified.
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format(ApplicationLocalizer.Language.Translate(100025), fileName), fileName); // The file {0} does not exist.
            }

            var thread = new Thread(() =>
            {
                var wb = new WebBrowser();
                wb.DocumentCompleted += wb_DocumentCompleted;
                // Add file to 'printing queue' before starting to load it in the browser component.
                AddFileToPrintingQueue(fileName, wb);
                wb.Url = new Uri(fileName);
                System.Windows.Forms.Application.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                ((WebBrowser)sender).Print();
                RemoveFileFromPrintingQueue(e.Url.LocalPath);
            }
            catch (Exception ex)
            {
                throw new PosisException(ApplicationLocalizer.Language.Translate(100007), ex); // An error occurred while sending the file to the printer.
            }
        }

        private void SetPrinterSettings()
        {
            // Set user print ticket first as it corresponds to the current default print queue.
            SaveUserPrintQueueTicket();

            if (!string.IsNullOrEmpty(currentDefaultPrinterName))
            {
                using (var localPrintServer = new LocalPrintServer())
                {
                    if (!string.Equals(localPrintServer.DefaultPrintQueue.FullName, currentDefaultPrinterName, StringComparison.OrdinalIgnoreCase))
                    {
                        bool defaultPrinterWasSet = SafeNativeMethods.SetDefaultPrinter(currentDefaultPrinterName);
                        if (!defaultPrinterWasSet)
                            throw new InvalidOperationException(string.Format(ApplicationLocalizer.Language.Translate(100027), currentDefaultPrinterName)); // Unable to set the printer '{0}' as default.
                    }
                }
            }
        }

        private void SavePrintQueueTicketToFile(PrintTicket configuredPrintTicket, PrintQueue configuredPrintQueue)
        {
            try
            {
                if (configuredPrintQueue != null)
                {
                    using (var sw = new StreamWriter(userPrintQueueFileName))
                    {
                        sw.Write(configuredPrintQueue.FullName);
                    }
                }

                if (configuredPrintTicket != null)
                {
                    if (File.Exists(userPrintTicketFileName))
                    {
                        File.Delete(userPrintTicketFileName);
                    }
                    using (var fs = new FileStream(userPrintTicketFileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        configuredPrintTicket.SaveTo(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new PosisException(ApplicationLocalizer.Language.Translate(100032), ex); // An error occurred while saving the printer settings to the file.
            }
        }

        private void ReadPrintQueueTicketFromFile(out PrintTicket printTicket, out PrintQueue printQueue, LocalPrintServer localPrintServer)
        {
            printQueue = null;
            printTicket = null;

            try
            {
                if (File.Exists(userPrintQueueFileName))
                {
                    using (var sr = new StreamReader(userPrintQueueFileName))
                    {
                        var printQueueFullName = sr.ReadToEnd();
                        foreach (var queue in localPrintServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections }))
                        {
                            if (queue.FullName.Equals(printQueueFullName))
                            {
                                printQueue = queue;
                                break;
                            }
                        }
                    }
                }

                if (File.Exists(userPrintTicketFileName))
                {
                    using (var fs = new FileStream(userPrintTicketFileName, FileMode.Open, FileAccess.Read))
                    {
                        printTicket = new PrintTicket(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new PosisException(ApplicationLocalizer.Language.Translate(100033), ex); // An error occurred while restoring the printer settings from the file.
            }
        }

        private void SaveUserPrintQueueTicket()
        {
            if (currentDefaultPrintTicket != null)
            {
                using (var localPrintServer = new LocalPrintServer())
                {
                    SaveUserPrintQueueTicket(localPrintServer.DefaultPrintQueue, currentDefaultPrintTicket);
                }
            }
        }

        private static void SaveUserPrintQueueTicket(PrintQueue printQueue, PrintTicket userPrintTicket)
        {
            if (printQueue == null)
                throw new ArgumentNullException("printQueue");

            if (userPrintTicket == null)
                throw new ArgumentNullException("userPrintTicket");

            var validationResult = printQueue.MergeAndValidatePrintTicket(printQueue.UserPrintTicket, userPrintTicket);
            if (validationResult.ConflictStatus == ConflictStatus.NoConflict)
            {
                printQueue.UserPrintTicket = validationResult.ValidatedPrintTicket;
                printQueue.Commit();
            }
        }

        private static bool ShowPrintDialog(out PrintQueue printQueue, out PrintTicket printTicket)
        {
            printQueue = null;
            printTicket = null;

            var printDialog = new System.Windows.Controls.PrintDialog();
            printDialog.MaxPage = 1;
            var dialogResult = printDialog.ShowDialog();
            if (dialogResult == null || !(bool)dialogResult)
            {
                return false;
            }

            printTicket = printDialog.PrintTicket;
            printQueue = printDialog.PrintQueue;

            return true;
        }

        #endregion

        #region Nested classes
        private static class SafeNativeMethods
        {
            #region P/Invoke
            [DllImport("Winspool.drv")]
            public static extern bool SetDefaultPrinter(string printerName);
            #endregion
        }
        #endregion
    }
}
