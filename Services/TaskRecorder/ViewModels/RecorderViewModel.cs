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
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.DocGeneration;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Localization;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MVVM;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Services;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.SysTaskRecorder;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels
{
    /// <summary>
    /// View model for recorder
    /// </summary>
    public sealed class RecorderViewModel : NotifyPropertyChangedBase, IDisposable
    {
        #region private fields

        private VideoRecordingGenerator videoController = new VideoRecordingGenerator();
        private TaskRecorderService serviceManager = TaskRecorderService.Instance;
        private FrameworkLineViewModel currentRecordingLine;
        private string currentRecordingFolderPath;
        private bool generateWordDoc;
        private const string WordTemplateExtension = ".dotx";
        private const string WordMacroEnabledTemplateExtension = ".dotm";
        private const string Word97_2003TemplateExtension = ".dot";
        private Dictionary<int, ObservableCollection<FrameworkLineViewModel>> frameworkLinesCache = new Dictionary<int, ObservableCollection<FrameworkLineViewModel>>();
        private Dictionary<int, SysTaskRecorderMapServiceContract> frameworkIndustryMapCache = new Dictionary<int, SysTaskRecorderMapServiceContract>();
        private bool disposed;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderViewModel"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public RecorderViewModel(MessageFilter filter)
        {
            this.MessageFilter = filter;
            this.RecorderStatus = RecorderStatus.Stopped;
            InitializeTemplateFilters();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the framework lines.
        /// </summary>
        /// <value>
        /// The framework lines.
        /// </value>
        public ObservableCollection<FrameworkLineViewModel> FrameworkLines
        {
            get
            {
                return frameworkLines;
            }
            private set
            {
                if (frameworkLines != value)
                {
                    frameworkLines = value;
                    OnPropertyChanged();
                }
            }
        }private ObservableCollection<FrameworkLineViewModel> frameworkLines;

        /// <summary>
        /// Gets or sets the selected framework line.
        /// </summary>
        /// <value>
        /// The selected framework line.
        /// </value>
        public FrameworkLineViewModel SelectedFrameworkLine
        {
            get
            {
                return selectedFrameworkLine;
            }
            set
            {
                if (selectedFrameworkLine != value)
                {
                    selectedFrameworkLine = value;
                    OnPropertyChanged();
                    TriggerCommandStates();
                }
            }
        }private FrameworkLineViewModel selectedFrameworkLine;

        /// <summary>
        /// Gets the framework list.
        /// </summary>
        /// <value>
        /// The framework list.
        /// </value>
        public ReadOnlyCollection<SysTaskRecorderFrameworkServiceContract> FrameworkList
        {
            get
            {
                return frameworkList;
            }
            private set
            {
                if (frameworkList != value)
                {
                    frameworkList = value;
                    OnPropertyChanged();
                    OnPropertyChanged("AreFrameworksIndustriesEmpty");
                }
            }
        }private ReadOnlyCollection<SysTaskRecorderFrameworkServiceContract> frameworkList;


        /// <summary>
        /// Gets or sets the selected framework.
        /// </summary>
        /// <value>
        /// The selected framework.
        /// </value>
        public SysTaskRecorderFrameworkServiceContract SelectedFramework
        {
            get
            {
                return selectedFramework;
            }
            set
            {
                if (selectedFramework != value)
                {
                    selectedFramework = value;
                    this.ResetFrameworkLines();
                    OnPropertyChanged();
                }
            }
        }private SysTaskRecorderFrameworkServiceContract selectedFramework;

        /// <summary>
        /// Gets the industry list.
        /// </summary>
        /// <value>
        /// The industry list.
        /// </value>
        public ReadOnlyCollection<SysTaskRecorderIndustryServiceContract> IndustryList
        {
            get
            {
                return industryList;
            }
            private set
            {
                if (industryList != value)
                {
                    industryList = value;
                    OnPropertyChanged();
                    OnPropertyChanged("AreFrameworksIndustriesEmpty");
                }
            }
        }private ReadOnlyCollection<SysTaskRecorderIndustryServiceContract> industryList;

        /// <summary>
        /// Gets or sets the selected industry.
        /// </summary>
        /// <value>
        /// The selected industry.
        /// </value>
        public SysTaskRecorderIndustryServiceContract SelectedIndustry
        {
            get
            {
                return selectedIndustry;
            }
            set
            {
                if (selectedIndustry != value)
                {
                    selectedIndustry = value;
                    this.ResetFrameworkLines();
                    OnPropertyChanged();
                }
            }
        }private SysTaskRecorderIndustryServiceContract selectedIndustry;

        /// <summary>
        /// Gets the available modules.
        /// </summary>
        /// <value>
        /// The available modules.
        /// </value>
        public ReadOnlyCollection<SysModule> AvailableModules
        {
            get
            {
                return availableModules;
            }
            private set
            {
                if (availableModules != value)
                {
                    availableModules = value;
                }
            }
        }private ReadOnlyCollection<SysModule> availableModules;

        /// <summary>
        /// Gets the localized available modules.
        /// </summary>
        /// <value>
        /// The localized available modules.
        /// </value>
        public Collection<LocalizedSysModule> LocalizedAvailableModules
        {
            get
            {
                if (localizedAvailableModules == null)
                {
                    localizedAvailableModules = EnumLocalizedValueConverter.GetLocalizedEnumValues<LocalizedSysModule, SysModule>(AvailableModules);
                }
                return localizedAvailableModules;
            }
        }private Collection<LocalizedSysModule> localizedAvailableModules;

        /// <summary>
        /// Gets the usage profiles.
        /// </summary>
        /// <value>
        /// The usage profiles.
        /// </value>
        public ReadOnlyCollection<SysTaskRecorderUsageProfile> UsageProfiles
        {
            get
            {
                return usageProfiles;
            }
            private set
            {
                if (usageProfiles != value)
                {
                    usageProfiles = value;
                }
            }
        }private ReadOnlyCollection<SysTaskRecorderUsageProfile> usageProfiles;

        /// <summary>
        /// Gets the localized usage profiles.
        /// </summary>
        /// <value>
        /// The localized usage profiles.
        /// </value>
        public Collection<LocalizedSysTaskRecorderUsageProfile> LocalizedUsageProfiles
        {
            get
            {
                if (localizedUsageProfiles == null)
                {
                    localizedUsageProfiles = EnumLocalizedValueConverter.GetLocalizedEnumValues<LocalizedSysTaskRecorderUsageProfile, SysTaskRecorderUsageProfile>(UsageProfiles);
                }
                return localizedUsageProfiles;
            }
        }private Collection<LocalizedSysTaskRecorderUsageProfile> localizedUsageProfiles;

        /// <summary>
        /// Gets or sets the recorder status.
        /// </summary>
        /// <value>
        /// The recorder status.
        /// </value>
        public RecorderStatus RecorderStatus
        {
            get
            {
                return recorderStatus;
            }
            set
            {
                if (this.recorderStatus != value)
                {
                    this.recorderStatus = value;
                    OnPropertyChanged();
                    this.TriggerCommandStates();
                }
            }
        }private RecorderStatus recorderStatus;

        /// <summary>
        /// Gets or sets the message filter.
        /// </summary>
        /// <value>
        /// The message filter.
        /// </value>
        public MessageFilter MessageFilter
        {
            get
            {
                return this.messageFilter;
            }
            set
            {
                if (this.messageFilter != value)
                {
                    this.messageFilter = value;
                }
            }
        }private MessageFilter messageFilter;

        /// <summary>
        /// Gets or sets the position recorder folder path.
        /// </summary>
        /// <value>
        /// The position recorder folder path.
        /// </value>
        public string PosRecorderFolderPath
        {
            get
            {
                return posRecorderFolderPath;
            }
            set
            {
                if (posRecorderFolderPath != value)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        posRecorderFolderPath = value.Trim('"');
                    }
                    else
                    {
                        posRecorderFolderPath = value;
                    }

                    OnPropertyChanged();
                    OnPropertyChanged("CanSetParameters");
                }
            }
        }private string posRecorderFolderPath;

        /// <summary>
        /// Gets or sets the position template file path.
        /// </summary>
        /// <value>
        /// The position template file path.
        /// </value>
        public string PosTemplateFilePath
        {
            get
            {
                return posTemplateFilePath;
            }
            set
            {
                if (posTemplateFilePath != value)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        posTemplateFilePath = value.Trim('"');
                    }
                    else
                    {
                        posTemplateFilePath = value;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged("CanSetParameters");
                }
            }
        }private string posTemplateFilePath;


        /// <summary>
        /// Gets or sets the template filter.
        /// </summary>
        /// <value>
        /// The template filter.
        /// </value>
        public string TemplateFilter
        {
            get
            {
                return templateFilter;
            }
            set
            {
                if (templateFilter != value)
                {
                    templateFilter = value;
                    OnPropertyChanged();
                }
            }
        }private string templateFilter;

        /// <summary>
        /// Gets a value indicating whether frameworks or industries are empty.
        /// </summary>
        /// <value>
        /// <c>true</c> if frameworks or industries are empty; otherwise, <c>false</c>.
        /// </value>
        public bool AreFrameworksIndustriesEmpty
        {
            get
            {
                return (this.FrameworkList != null) &&
                        (this.IndustryList != null) &&
                        ((!this.FrameworkList.Any()) ||
                        (!this.IndustryList.Any()));
            }
        }

        #region StartRecordingCommand

        /// <summary>
        /// Gets a value indicating whether recorder can start recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recorder can start recording; otherwise, <c>false</c>.
        /// </value>
        public bool CanStartRecording
        {
            get
            {
                return (this.RecorderStatus == RecorderStatus.Stopped) &&
                        (this.SelectedFrameworkLine != null) &&
                        (this.SelectedFrameworkLine.AllowArtifacts) &&
                        (!this.SelectedFrameworkLine.HasRecording) &&
                        this.IsBasicFrameworkLineSelected();
            }
        }

        /// <summary>
        /// Start recording.
        /// </summary>
        /// <param name="generateWordDocument">if set to <c>true</c> generate word document.</param>
        public void ExecuteStartRecording(bool generateWordDocument)
        {
            this.generateWordDoc = generateWordDocument;
            this.messageFilter.GenerateScreenshots = generateWordDocument;

            if (string.IsNullOrWhiteSpace(this.PosRecorderFolderPath))
            {
                if (this.OnError != null)
                {
                    OnError(this, new ViewModelErrorEventArgs() { Message = ApplicationLocalizer.Language.Translate(58430) }); // Please provide a valid folder path for the recording using Setup.

                    return;
                }
            }

            this.currentRecordingFolderPath = this.serviceManager.GetRecordingFilePath(this.SelectedFramework.FrameworkId, this.SelectedIndustry.IndustryCode, this.SelectedFrameworkLine.SyncId);
            if (!Directory.Exists(currentRecordingFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(currentRecordingFolderPath);
                }
                catch (Exception)
                {
                    OnError(this, new ViewModelErrorEventArgs() { Message = ApplicationLocalizer.Language.Translate(58431) });// Unable to create path specified in the Setup.

                    return;
                }
            }

            this.currentRecordingLine = this.SelectedFrameworkLine;
            Application.AddMessageFilter(messageFilter);
            this.MessageFilter.Start(currentRecordingFolderPath);
            try
            {
                this.videoController.Start(currentRecordingFolderPath);
            }
            catch (InvalidOperationException ex)
            {
                OnError(this, new ViewModelErrorEventArgs() { Message = ex.Message, Exception = ex });
                return;
            }

            this.RecorderStatus = RecorderStatus.Recording;
            OnPropertyChanged("SelectedFrameworkLine");
        }

        #endregion

        #region StopRecordingCommand

        /// <summary>
        /// Gets a value indicating whether recorder can stop recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [can stop recording]; otherwise, <c>false</c>.
        /// </value>
        public bool CanStopRecording
        {
            get
            {
                return ((this.RecorderStatus == RecorderStatus.Recording) || (this.RecorderStatus == RecorderStatus.Paused)) &&
                        (this.RecorderStatus != RecorderStatus.Stopping) &&
                                (this.SelectedFrameworkLine != null);
            }
        }

        /// <summary>
        /// Stop recording.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "No event handlers are attached for task, so do not have to dispose.")]
        public void ExecuteStopRecording()
        {
            Application.RemoveMessageFilter(messageFilter);

            this.MessageFilter.Stop();
            var wmvPath = this.videoController.Stop();

            var task = DocGeneratorFactory.CreateDocsAsync(this.currentRecordingLine.Name, this.MessageFilter.Messages, currentRecordingFolderPath, this.PosTemplateFilePath, this.generateWordDoc);

            task.ContinueWith((tsk, ctx) =>
                {
                    SynchronizationContext primaryContext = (SynchronizationContext)ctx;

                    // Handle if any exceptions occured during the previous task execution
                    Exception exception = tsk.Exception;
                    if (exception != null)
                    {
                        primaryContext.Post(exp =>
                        {
                            ApplicationExceptionHandler.HandleException(typeof(DocGeneratorFactory).ToString(), (Exception)exp);
                            if (OnError != null)
                            {
                                this.OnError(this, new ViewModelErrorEventArgs() { Message = (58400).Translate() }); // One or more errors occured while generating documents. Please check logs for details.
                            }
                        }, exception);
                    }
                    else
                    {
                        var docs = tsk.Result;

                        // If video file is generated, add it, else log message and inform user.
                        if (File.Exists(wmvPath))
                        {
                            docs.Add(SysTaskRecorderType.Video, new FileContent() { Path = wmvPath });
                        }
                        else
                        {
                            ApplicationLog.Log(this.ToString(), "Video not generated by TaskBackgroundWorker.exe", LogTraceLevel.Error);
                            if (OnError != null)
                            {
                                this.OnError(this, new ViewModelErrorEventArgs() { Message = (58400).Translate() }); // One or more errors occured while generating documents. Please check logs for details.
                            }
                        }

                        // If any docs are generated, then send send to database
                        if (docs.Any())
                        {
                            serviceManager.SetRecordingArtifacts(docs, this.SelectedFrameworkLine.SyncId);
                            this.currentRecordingLine = null;

                            primaryContext.Post(postState => this.SelectedFrameworkLine.HasRecording = true, null);
                        }
                    }

                    primaryContext.Post(postState => this.RecorderStatus = RecorderStatus.Stopped, null);
                }, SynchronizationContext.Current);

            this.RecorderStatus = RecorderStatus.Stopping;
        }

        #endregion

        #region CreateNewNodeCommand

        /// <summary>
        /// Gets a value indicating whether a node can be createed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a node can be create; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreateNewNode
        {
            get
            {
                return (this.SelectedFrameworkLine != null) &&
                    (this.SelectedFrameworkLine.IsBasic) &&
                    (this.SelectedFrameworkLine.AllowChildNodes) &&
                    (this.RecorderStatus != Common.RecorderStatus.Recording) &&
                    (this.RecorderStatus != Common.RecorderStatus.Stopping);
            }
        }

        /// <summary>
        /// Create new framework line from model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public void ExecuteCreateNewNode(SysTaskRecorderLineServiceContract model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            this.SelectedFrameworkLine.AddChild(model, this.frameworkIndustryMapCache[Utils.GetHashCode(this.SelectedFramework.FrameworkId, this.SelectedIndustry.IndustryCode)]);
        }

        #endregion

        #region DeleteNodeCommand

        /// <summary>
        /// Gets a value indicating whether it can delete node.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it can delete node; otherwise, <c>false</c>.
        /// </value>
        public bool CanDeleteNode
        {
            get
            {
                return (this.SelectedFrameworkLine != null) &&
                    (this.SelectedFrameworkLine.IsBasic) &&
                    (!this.SelectedFrameworkLine.IsFrameworkIndustryLine) &&
                    (this.SelectedFrameworkLine.Origin == RecorderLineOrigin.POS) &&
                    (this.RecorderStatus != Common.RecorderStatus.Recording) &&
                    (this.RecorderStatus != Common.RecorderStatus.Stopping);
            }
        }

        /// <summary>
        /// Deletes framework line / node.
        /// </summary>
        public void ExecuteDeleteNode()
        {
            var success = this.serviceManager.DeleteFrameworkLine(this.SelectedFrameworkLine.SyncId, this.SelectedFramework.FrameworkId, this.SelectedIndustry.IndustryCode);
            this.SelectedFrameworkLine = this.SelectedFrameworkLine.Delete();
            HandleFrameworkFolderDeletionResult(success);
        }

        private void HandleFrameworkFolderDeletionResult(bool success)
        {
            if ((!success) && (OnError != null))
            {
                OnError(this, new ViewModelErrorEventArgs() { Message = ApplicationLocalizer.Language.Translate(58440) });// Unable to delete one or more associated files/folders
            }
        }

        #endregion

        #region ClearNodeCommand

        /// <summary>
        /// Gets a value indicating whether a node's recordings can be cleared/deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a nodes' recordings can be cleared/deleted; otherwise, <c>false</c>.
        /// </value>
        public bool CanClearNode
        {
            get
            {
                return (this.SelectedFrameworkLine != null) &&
                    (this.SelectedFrameworkLine.IsBasic) &&
                    (!this.SelectedFrameworkLine.IsFrameworkIndustryLine) &&
                    (this.SelectedFrameworkLine.HasRecording) &&
                    (this.SelectedFrameworkLine.Origin == RecorderLineOrigin.POS) &&
                    (this.RecorderStatus != Common.RecorderStatus.Recording) &&
                    (this.RecorderStatus != Common.RecorderStatus.Stopping);
            }
        }

        /// <summary>
        /// Clears node out of its recordings.
        /// </summary>
        public void ExecuteClearNode()
        {
            bool success = this.serviceManager.ClearFrameworkLine(this.SelectedFrameworkLine.SyncId, this.SelectedFramework.FrameworkId, this.SelectedIndustry.IndustryCode);
            if (success)
            {
                this.SelectedFrameworkLine.HasRecording = false;
                this.TriggerCommandStates();
            }
            this.HandleFrameworkFolderDeletionResult(success);
        }

        #endregion

        #region SetParametersCommand

        /// <summary>
        /// Gets a value indicating whether it can set parameters.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it can set parameters; otherwise, <c>false</c>.
        /// </value>
        public bool CanSetParameters
        {
            get
            {
                try
                {
                    bool isRecorderFolderEmpty = string.IsNullOrWhiteSpace(this.PosRecorderFolderPath);
                    bool isTemplatePathEmpty = string.IsNullOrWhiteSpace(this.PosTemplateFilePath);

                    return (this.RecorderStatus != Common.RecorderStatus.Recording) &&
                            (this.RecorderStatus != Common.RecorderStatus.Stopping) &&
                            !(isRecorderFolderEmpty && isTemplatePathEmpty) &&
                            RecorderFolderExists(isRecorderFolderEmpty) &&
                            HasRightTemplateExtension(isTemplatePathEmpty);
                }
                catch (Exception ex)
                {
                    ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                }

                return false;
            }
        }

        /// <summary>
        /// Sets parameters.
        /// </summary>
        public void ExecuteSetParameters()
        {
            if (!string.IsNullOrWhiteSpace(this.PosTemplateFilePath) || !string.IsNullOrWhiteSpace(this.PosRecorderFolderPath))
            {
                this.serviceManager.SetParameters(this.PosTemplateFilePath, this.PosRecorderFolderPath);
            }
        }

        #endregion

        #endregion

        #region events

        /// <summary>
        /// Occurs when error occurs in view model.
        /// </summary>
        public event EventHandler<ViewModelErrorEventArgs> OnError;

        #endregion

        #region public methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            try
            {
                this.TriggerCommandStates();
                SysTaskRecorderParameterServiceContract parameter = serviceManager.GetParameters();
                if (parameter != null)
                {
                    this.PosRecorderFolderPath = parameter.FilePath;
                    this.PosTemplateFilePath = parameter.TemplatePath;
                }

                var originalFrameworkList = this.serviceManager.GetFrameworkList() ?? new SysTaskRecorderFrameworkServiceContract[0];
                this.FrameworkList = new ReadOnlyCollection<SysTaskRecorderFrameworkServiceContract>(originalFrameworkList);
                this.SelectedFramework = this.FrameworkList.FirstOrDefault();

                var originalIndustryList = this.serviceManager.GetIndustryList() ?? new SysTaskRecorderIndustryServiceContract[0];
                this.IndustryList = new ReadOnlyCollection<SysTaskRecorderIndustryServiceContract>(originalIndustryList);
                this.SelectedIndustry = this.IndustryList.FirstOrDefault();

                this.AvailableModules = new ReadOnlyCollection<SysModule>(Utils.GetEnumValues<SysModule>());
                this.UsageProfiles = new ReadOnlyCollection<SysTaskRecorderUsageProfile>(Utils.GetEnumValues<SysTaskRecorderUsageProfile>());
            }
            catch (Exception)
            {
                OnError(this, new ViewModelErrorEventArgs() { Message = ApplicationLocalizer.Language.Translate(58428) }); // Error while retrieving initial data.
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Resets the framework lines.
        /// </summary>
        public void ResetFrameworkLines()
        {
            if ((this.SelectedFramework != null) && (this.SelectedIndustry != null))
            {
                string frameworkId = this.SelectedFramework.FrameworkId;
                string industryCode = this.SelectedIndustry.IndustryCode;

                int cacheKey = Utils.GetHashCode(frameworkId, industryCode);
                ObservableCollection<FrameworkLineViewModel> lines;

                // If the lines are previously loaded, then they load from cache.
                if (this.frameworkLinesCache.TryGetValue(cacheKey, out lines))
                {
                    this.FrameworkLines = lines;
                }
                else
                {
                    // If not found in cache, get from service and cache the lines. 
                    SysTaskRecorderMapServiceContract frameworkIndustryMap = this.serviceManager.RetrieveFrameworkIndustryMapInfo(frameworkId, industryCode);
                    if (frameworkIndustryMap != null)
                    {
                        this.frameworkIndustryMapCache.Add(cacheKey, frameworkIndustryMap);
                        this.FrameworkLines = FrameworkLineViewModel.Create(frameworkId, industryCode, frameworkIndustryMap);

                        if (this.FrameworkLines != null)
                        {
                            this.frameworkLinesCache.Add(cacheKey, this.FrameworkLines);
                        }
                    }
                }

                if (this.FrameworkLines != null)
                {
                    this.SelectedFrameworkLine = this.FrameworkLines.FirstOrDefault();
                }
            }
        }

        #endregion

        #region private methods

        private void InitializeTemplateFilters()
        {
            string allTemplatesFilter = string.Format("*{0};*{1};*{2}", WordTemplateExtension, WordMacroEnabledTemplateExtension, Word97_2003TemplateExtension);
            this.TemplateFilter = string.Format("{0}|{1}|{2}|*{3}|{4}|*{5}|{6}|*{7}",
                (58449).Translate(), //All Templates
                allTemplatesFilter,
                (58450).Translate(), //Word Template (*.dotx)
                WordTemplateExtension,
                (58451).Translate(), //Word Macro-Enabled Template (*.dotm)
                WordMacroEnabledTemplateExtension,
                (58452).Translate(), //Word 97-2003 Template (*.dot)
                Word97_2003TemplateExtension);
        }

        private bool HasRightTemplateExtension(bool isTemplatePathEmpty)
        {
            return (isTemplatePathEmpty ||
                (File.Exists(this.PosTemplateFilePath) &&
                    (this.PosTemplateFilePath.EndsWith(WordTemplateExtension, StringComparison.OrdinalIgnoreCase) ||
                    this.PosTemplateFilePath.EndsWith(WordMacroEnabledTemplateExtension, StringComparison.OrdinalIgnoreCase) ||
                    this.PosTemplateFilePath.EndsWith(Word97_2003TemplateExtension, StringComparison.OrdinalIgnoreCase))));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // if disposing, free any managed objects....
                if (disposing)
                {
                    Application.RemoveMessageFilter(this.MessageFilter);
                    this.frameworkLinesCache.Clear();
                    this.frameworkIndustryMapCache.Clear();
                }

                this.disposed = true;
            }
        }

        private bool RecorderFolderExists(bool isRecorderFolderPathEmpty)
        {
            return (isRecorderFolderPathEmpty || Directory.Exists(this.PosRecorderFolderPath));
        }

        // This method should be removed once advanced mode recording feature is implemented
        private bool IsBasicFrameworkLineSelected()
        {
            return (this.SelectedFrameworkLine != null) && (this.SelectedFrameworkLine.IsBasic);
        }

        private void TriggerCommandStates()
        {
            OnPropertyChanged("CanStartRecording");
            OnPropertyChanged("CanStopRecording");
            OnPropertyChanged("CanCreateNewNode");
            OnPropertyChanged("CanDeleteNode");
            OnPropertyChanged("CanClearNode");
        }

        #endregion
    }

}
