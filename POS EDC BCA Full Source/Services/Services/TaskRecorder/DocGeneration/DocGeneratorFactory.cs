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
using System.Threading.Tasks;
using System.Xml.Serialization;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.DocGeneration.TaskStep;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.DocGeneration
{
    /// <summary>
    /// Generates documents after recording
    /// </summary>
    internal static class DocGeneratorFactory
    {
        #region public methods
        /// <summary>
        /// Creates the documents.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="recordingFolderPath">The recording folder path.</param>
        /// <param name="templatePath">The template path.</param>
        /// <param name="generateWordDocument">if set to <c>true</c> generates word document.</param>
        /// <returns>
        /// true if no errors in any created doc, else false
        /// </returns>
        public static async Task<Dictionary<SysTaskRecorderType, FileContent>> CreateDocsAsync(string taskName, IEnumerable<MessageRecord> messages, string recordingFolderPath, string templatePath, bool generateWordDocument)
        {
            var documentPaths = new Dictionary<SysTaskRecorderType, FileContent>();
            if (messages != null)
            {
                // Advanced mode feature needs to be implemented for this
                //CreateTaskStepXml(taskName, messages, recordingFolderPath);
                if (generateWordDocument)
                {
                    string docPath = await CreateRecordingWordDocAsync(taskName, messages, recordingFolderPath, templatePath);
                    documentPaths.Add(SysTaskRecorderType.HelpDocument, new FileContent { Path = docPath });
                }
            }
            return documentPaths;
        }
        #endregion

        #region private methods
        private static void CreateTaskStepXml(string taskName, IEnumerable<MessageRecord> messages, string recordingFolderPath)
        {
            if (messages != null)
            {
                RecordingTask task = GetRecordingTask(taskName, messages);
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                XmlSerializer serializer = new XmlSerializer(typeof(RecordingTask));
                using (StreamWriter writer = new StreamWriter(Path.Combine(recordingFolderPath, @"TaskStep.xml")))
                {
                    serializer.Serialize(writer, task, ns);
                }
            }
        }

        private static async Task<string> CreateRecordingWordDocAsync(string taskName, IEnumerable<MessageRecord> messages, string recordingFolderPath, string templatePath)
        {
            string filePath = await WordDocGenerator.CreateRecordingWordDocAsync(taskName, messages, recordingFolderPath, templatePath);
            return filePath;
        }

        private static RecordingTask GetRecordingTask(string taskName, IEnumerable<MessageRecord> messages)
        {
            RecordingTask task = new RecordingTask() { Name = taskName };
            task.AddSteps(messages);
            return task;
        }
        #endregion

    }
}
