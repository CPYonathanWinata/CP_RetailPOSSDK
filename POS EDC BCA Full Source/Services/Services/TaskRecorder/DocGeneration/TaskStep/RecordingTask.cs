/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.DocGeneration.TaskStep
{
    /// <summary>
    /// Represents task element
    /// </summary>
    [XmlRoot("Task")]
    public class RecordingTask
    {
        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingTask"/> class.
        /// </summary>
        public RecordingTask()
        {
            this.Steps = new Collection<RecordingStep>();
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        [XmlElement("Step")]
        public Collection<RecordingStep> Steps { get; private set; }
        #endregion

        #region public methods
        /// <summary>
        /// Adds the steps.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void AddSteps(IEnumerable<MessageRecord> messages)
        {
            if (messages!=null)
            {
                int i = 1;
                foreach (var message in messages)
                {
                    this.Steps.Add(new RecordingStep() { Id = i, Description = MessageTextUtil.FormatMessageTextWithSequenceNumber(i++, message.HelpText) });
                }
            }
        }
        #endregion
    }
}
