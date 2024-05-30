/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.IO;
using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.DataEntity;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.SysTaskRecorder;
using System.Linq;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Services
{
    /// <summary>
    /// Service for task recorder
    /// </summary>
    public sealed class TaskRecorderService
    {
        #region private fields

        private TaskRecorderDataManager dataManager = new TaskRecorderDataManager(ApplicationSettings.Database.LocalConnection, ApplicationSettings.Database.DATAAREAID);

        #endregion

        #region singleton members

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TaskRecorderService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TaskRecorderService();
                }
                return instance;
            }
        }private static TaskRecorderService instance;

        private TaskRecorderService()
        {

        }

        #endregion

        #region public methods

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns>Parameters.</returns>
        public SysTaskRecorderParameterServiceContract GetParameters()
        {
            SysTaskRecorderParameterServiceContract contract = null;
            TaskRecorderParameters parameter = dataManager.GetParameters();
            if (parameter != null)
            {
                contract = Utils.Convert<TaskRecorderParameters, SysTaskRecorderParameterServiceContract>(parameter);
            }
            return contract;
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <param name="recordingPath">The recording path.</param>
        public void SetParameters(string templatePath, string recordingPath)
        {
            dataManager.SetParameters(templatePath, recordingPath);
        }

        /// <summary>
        /// Gets the framework list.
        /// </summary>
        /// <returns>Framework list.</returns>
        public SysTaskRecorderFrameworkServiceContract[] GetFrameworkList()
        {
            TaskRecorderFramework[] frameworks = dataManager.GetFrameworks();
            SysTaskRecorderFrameworkServiceContract[] frameworksConverted =
                Utils.Convert<TaskRecorderFramework[], SysTaskRecorderFrameworkServiceContract[]>(frameworks);
            return frameworksConverted;
        }

        /// <summary>
        /// Gets the industry list.
        /// </summary>
        /// <returns>Industry list.</returns>
        public SysTaskRecorderIndustryServiceContract[] GetIndustryList()
        {
            TaskRecorderIndustry[] industries = dataManager.GetIndustries();
            SysTaskRecorderIndustryServiceContract[] industriesConverted =
                Utils.Convert<TaskRecorderIndustry[], SysTaskRecorderIndustryServiceContract[]>(industries);
            return industriesConverted;
        }

        /// <summary>
        /// Retrieves the framework industry map information.
        /// </summary>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        /// <returns>Map info along with the corresponding framework lines.</returns>
        public SysTaskRecorderMapServiceContract RetrieveFrameworkIndustryMapInfo(string frameworkId, string industryCode)
        {
            var convertedLines = new List<SysTaskRecorderLineServiceContract>();
            ICollection<TaskRecorderFrameworkLevel> originalLevels = null;

            var originalMap = dataManager.GetFrameworkIndustryMap(frameworkId, industryCode);
            if (originalMap != null)
            {
                originalLevels = originalMap.Levels;
                if (originalLevels != null)
                {
                    // Get first level and then it's corresponding lines
                    var firstLevel = originalLevels.FirstOrDefault(level => level.Sequence == 1);
                    if (firstLevel != null)
                    {
                        var originalLines = dataManager.GetFrameworkLines(firstLevel.Id);
                        foreach (var originalLine in originalLines)
                        {
                            bool lineHasArtifacts = dataManager.FrameworkLineHasArtifacts(originalLine.Id);
                            var convertedLine = Utils.Convert<TaskRecorderFrameworkLine, SysTaskRecorderLineServiceContract>(originalLine);
                            convertedLine.HasRecording = lineHasArtifacts;

                            Guid parentId = Guid.Empty;
                            var parentLine = dataManager.GetFrameworkLine(originalLine.ParentFrameworkLineId);
                            if (parentLine != null)
                            {
                                parentId = parentLine.SyncId;
                            }
                            convertedLine.ParentId = parentId;
                            convertedLines.Add(convertedLine);
                        }
                    }
                }
            }

            SysTaskRecorderMapServiceContract contract = new SysTaskRecorderMapServiceContract()
            {
                FrameworkLines = convertedLines.ToArray(),
                GenerateProcessData = (originalMap != null) && (originalMap.GenerateProcessData == 1)
            };
            if (originalLevels != null)
            {
                contract.FrameworkIndustryLevels = Utils.Convert<ICollection<TaskRecorderFrameworkLevel>, SysTaskRecorderLevelServiceContract[]>(originalLevels);
            }
            return contract;
        }

        /// <summary>
        /// Gets the immediate child framework lines of the specified line. (Does not include nested children).
        /// </summary>
        /// <param name="frameworkLineSyncId">The framework line synchronize identifier.</param>
        /// <returns>
        /// Immediate chid framework lines of the specified line.
        /// </returns>
        /// <exception cref="System.ArgumentException">frameworkLineSyncId</exception>
        public SysTaskRecorderLineServiceContract[] GetImmediateChildFrameworkLines(Guid frameworkLineSyncId)
        {
            SysTaskRecorderLineServiceContract[] convertedChildLines = null;

            var originalChildLines = dataManager.GetImmediateChildFrameworkLines(frameworkLineSyncId);
            if (originalChildLines != null)
            {
                convertedChildLines = new SysTaskRecorderLineServiceContract[originalChildLines.Length];
                for (int i = 0; i < originalChildLines.Length; i++)
                {
                    bool lineHasArtifacts = dataManager.FrameworkLineHasArtifacts(originalChildLines[i].Id);
                    convertedChildLines[i] = Utils.Convert<TaskRecorderFrameworkLine, SysTaskRecorderLineServiceContract>(originalChildLines[i]);
                    convertedChildLines[i].HasRecording = lineHasArtifacts;
                }

            }
            return convertedChildLines;
        }

        /// <summary>
        /// Clears the framework line.
        /// </summary>
        /// <param name="frameworkLineSyncId">The framework line synchronize identifier.</param>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        public bool ClearFrameworkLine(Guid frameworkLineSyncId, string frameworkId, string industryCode)
        {
            bool deletedFolder = false;
            string recordingLineFolderPath = this.GetRecordingFilePath(frameworkId, industryCode, frameworkLineSyncId);

            if (!string.IsNullOrWhiteSpace(recordingLineFolderPath))
            {
                try
                {
                    if (Directory.Exists(recordingLineFolderPath))
                    {
                        Directory.Delete(recordingLineFolderPath, true);
                    }

                    // Sometimes, deleting directory is taking time and if we check whether directory is deleted immediately, then we are getting false positives
                    // So, directly assigning true instead of checking and assigning. If we get any exception, we make it false anyway.
                    deletedFolder = true;
                }
                catch (Exception)
                {
                    deletedFolder = false;
                }
            }

            if (deletedFolder)
            {
                dataManager.ClearFrameworkLine(frameworkLineSyncId);
            }

            return deletedFolder;
        }

        /// <summary>
        /// Deletes the framework line.
        /// </summary>
        /// <param name="frameworkLineSyncId">The framework line synchronize identifier.</param>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        public bool DeleteFrameworkLine(Guid frameworkLineSyncId, string frameworkId, string industryCode)
        {
            bool clearedAllLines = true;
            bool clearedCurrentLine = true;
            TaskRecorderFrameworkLine[] frameworkLineHierarchy = dataManager.GetFrameworkLineHierarchy(frameworkLineSyncId);

            if (frameworkLineHierarchy != null)
            {
                foreach (var line in frameworkLineHierarchy)
                {
                    clearedCurrentLine = this.ClearFrameworkLine(line.SyncId, frameworkId, industryCode);
                    clearedAllLines &= clearedCurrentLine;
                    dataManager.DeleteFrameworkLine(line.SyncId);
                }
            }

            return clearedAllLines;
        }

        /// <summary>
        /// Creates the framework line.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="module">The module.</param>
        /// <param name="operationGroup">The operation group.</param>
        /// <param name="parentFrameworkLineSyncId">The parent framework line synchronize identifier.</param>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        /// <returns>The created framework line.</returns>
        public SysTaskRecorderLineServiceContract CreateFrameworkLine(string name, string description, SysModule module, SysTaskRecorderOperationGroup operationGroup, Guid parentFrameworkLineSyncId, string frameworkId, string industryCode)
        {
            SysTaskRecorderLineServiceContract newLine = null;
            var createdLine = dataManager.CreateFrameworkLine(name, description, (int)module, (int)operationGroup, parentFrameworkLineSyncId, frameworkId, industryCode);

            if (createdLine != null)
            {
                newLine = Utils.Convert<TaskRecorderFrameworkLine, SysTaskRecorderLineServiceContract>(createdLine);
            }

            return newLine;
        }

        /// <summary>
        /// Gets the recording file path.
        /// </summary>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        /// <param name="frameworkLineSyncId">The framework line synchronize identifier.</param>
        /// <returns>Recording file path.</returns>
        public string GetRecordingFilePath(string frameworkId, string industryCode, Guid frameworkLineSyncId)
        {
            string recordingFilePath = null;
            string recordingFolderPath = null;
            string recordingLinePath = null;

            if (!string.IsNullOrWhiteSpace(recordingFolderPath = this.GetRecordingFolderPath(frameworkId, industryCode)) &&
                !string.IsNullOrWhiteSpace(recordingLinePath = this.GetRecordingLinePath(frameworkLineSyncId)))
            {
                recordingFilePath = Path.Combine(recordingFolderPath, recordingLinePath);
            }
            return recordingFilePath;
        }

        /// <summary>
        /// Sets the recording artifacts.
        /// </summary>
        /// <param name="docPaths">The document paths.</param>
        /// <param name="frameworkLineSyncId">The framework line synchronize identifier.</param>
        public void SetRecordingArtifacts(Dictionary<SysTaskRecorderType, FileContent> docPaths, Guid frameworkLineSyncId)
        {
            dataManager.SetRecordingArtifacts(docPaths, frameworkLineSyncId);
        }

        #endregion

        #region private methods

        private string GetRecordingFolderPath(string frameworkId, string industryCode)
        {
            string mainFolderPath = dataManager.GetRecordingPath();
            string frameworkIndustryFolderPath = string.Format("{0}{1}{2}_{3}", mainFolderPath, Path.DirectorySeparatorChar, frameworkId, industryCode);
            return frameworkIndustryFolderPath;
        }

        private string GetRecordingLinePath(Guid frameworkLineSyncId)
        {
            string backSlash = @"\";
            string forwardSlash = @"/";
            string outputFileName, fileNamestr1, fileNameStr2;
            TaskRecorderFrameworkLine frameworkLine = dataManager.GetFrameworkLine(frameworkLineSyncId);

            fileNamestr1 = frameworkLine.Name;
            fileNamestr1 = fileNamestr1.Substring(0, Math.Min(15, fileNamestr1.Length));
            fileNamestr1 = fileNamestr1.Replace(' ', '_');

            string lineSyncId = frameworkLineSyncId.ToString("D").ToUpperInvariant();
            fileNameStr2 = lineSyncId.Substring(0, lineSyncId.IndexOf('-') + 1);

            outputFileName = fileNamestr1 + '_' + fileNameStr2;
            outputFileName = outputFileName.Replace(backSlash, "_");
            outputFileName = outputFileName.Replace(forwardSlash, "_");

            return outputFileName;
        }
        #endregion
    }
}
