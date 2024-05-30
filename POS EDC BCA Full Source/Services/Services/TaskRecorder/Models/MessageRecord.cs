/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models
{
    /// <summary>
    /// Record for storing windows message info
    /// </summary>
    public class MessageRecord
    {
        #region constructors
        public MessageRecord(
            Guid recordId,
            int handle,
            int windowHandle,
            string windowTitle,
            MessageType messageType,
            EventType eventType,
            string controlName,
            string controlDisplayName,
            string controlText,
            AccessibleRole controlRole)
        {
            this.RecordId = recordId;
            this.Handle = handle;
            this.WindowHandle = windowHandle;
            this.WindowTitle = windowTitle;
            this.MessageType = messageType;
            this.EventType = eventType;
            this.ControlName = controlName;
            this.ControlDisplayName = controlDisplayName;
            this.ControlValue = controlText;
            this.ControlRole = controlRole;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the record identifier.
        /// </summary>
        /// <value>
        /// The record identifier.
        /// </value>
        public Guid RecordId { get; private set; }

        /// <summary>
        /// Gets the control's handle.
        /// </summary>
        /// <value>
        /// The control's handle.
        /// </value>
        public int Handle { get; private set; }

        /// <summary>
        /// Gets or sets the window handle.
        /// </summary>
        /// <value>
        /// The window handle.
        /// </value>
        public int WindowHandle { get; set; }

        /// <summary>
        /// Gets the window title.
        /// </summary>
        /// <value>
        /// The window title.
        /// </value>
        public string WindowTitle { get; private set; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public MessageType MessageType { get; private set; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public EventType EventType { get; private set; }

        /// <summary>
        /// Gets the name of the control.
        /// </summary>
        /// <value>
        /// The name of the control.
        /// </value>
        public string ControlName { get; private set; }

        /// <summary>
        /// Gets the display name of the control.
        /// </summary>
        /// <value>
        /// The display name of the control.
        /// </value>
        public string ControlDisplayName { get; private set; }

        /// <summary>
        /// Gets or sets the control value.
        /// </summary>
        /// <value>
        /// The control value.
        /// </value>
        public string ControlValue { get; set; }

        /// <summary>
        /// Gets the control role.
        /// </summary>
        /// <value>
        /// The control role.
        /// </value>
        public AccessibleRole ControlRole { get; private set; }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        public string HelpText
        {
            get
            {
                return MessageTextUtil.GetMessageText(this);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, '{3}', '{4}', {5}, {6}, {7}", this.Handle, this.MessageType, this.EventType, this.ControlName, this.ControlDisplayName, this.ControlValue, this.ControlRole, this.WindowTitle);
        }
        #endregion
    }
}
