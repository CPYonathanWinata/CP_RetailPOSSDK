/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling
{
    /// <summary>
    /// Message filter to observe windows messages sent to a windows form
    /// </summary>
    public class MessageFilter : IMessageFilter
    {
        #region private fields

        private bool isStarted = false;
        private bool ignoreNextKeyUp = false;
        private Control controlIgnored;
        private string currentRecordingFolderPath;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFilter" /> class.
        /// </summary>
        /// <param name="controlToIgnore">The control to ignore.</param>
        public MessageFilter(Control controlToIgnore)
        {
            this.controlIgnored = controlToIgnore;
            this.Messages = new Queue<MessageRecord>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public Queue<MessageRecord> Messages { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate screenshots (to be used in word doc creation).
        /// </summary>
        /// <value>
        ///   <c>true</c> if generates screenshots; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateScreenshots { get; set; }

        #endregion

        #region public methods

        /// <summary>
        /// Starts observing windows messages.
        /// </summary>
        /// <param name="recordingFolderPath">The recording folder path.</param>
        public void Start(string recordingFolderPath)
        {
            this.Messages.Clear();
            this.currentRecordingFolderPath = recordingFolderPath;
            this.isStarted = true;
        }

        /// <summary>
        /// Stops observing windows messages.
        /// </summary>
        public void Stop()
        {
            this.isStarted = false;
            this.currentRecordingFolderPath = null;
        }

        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        /// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
        /// </returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (this.isStarted)
            {
                Control control = GetControl(m.HWnd);

                if ((control != null) && (this.Filter(control)))
                {
                    bool shouldHandleMessage = false;
                    MessageType messageType = (MessageType)m.Msg;
                    EventType? eventType = RelayEventMap.GetEventType(m, control.AccessibilityObject.Role);
                    if (eventType == null)
                    {
                        return false;
                    }

                    switch (eventType.Value)
                    {
                        case EventType.Press:
                        case EventType.Click:
                            {
                                shouldHandleMessage = true;
                                break;
                            }
                        case EventType.Enter:
                            {
                                messageType = MessageType.VK_RETURN;
                                shouldHandleMessage = true;
                                ignoreNextKeyUp = true;
                                break;
                            }
                        case EventType.Type:
                            {
                                if (ignoreNextKeyUp)
                                {
                                    ignoreNextKeyUp = false;
                                }
                                else
                                {
                                    shouldHandleMessage = true;
                                }
                                break;
                            }
                    }

                    if (shouldHandleMessage)
                    {
                        string controlName = control.Name;
                        string controlDisplayName = control.GetDisplayName();

                        Control window = control.TopLevelControl;
                        int windowHwnd = (int)window.Handle;
                        string windowTitle = window.GetDisplayName();

                        if (!ignoreNextKeyUp)
                        {
                            shouldHandleMessage = !HandleTextBoxTyping(control, eventType.Value, controlDisplayName);
                        }

                        if (shouldHandleMessage)
                        {
                            MessageRecord newMessage = new MessageRecord(Guid.NewGuid(), (int)m.HWnd, windowHwnd, windowTitle, messageType, eventType.Value, controlName, controlDisplayName, null, control.AccessibilityObject.Role);
                            control.GetValue(value => newMessage.ControlValue = value);
                            this.AddScreenshotIfRequired(newMessage, control);
                            this.Messages.Enqueue(newMessage);
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region private fields

        private bool Filter(Control control)
        {
            if (object.ReferenceEquals(control.FindForm(), this.controlIgnored))
            {
                return false;
            }
            return true;
        }

        private void AddScreenshotIfRequired(MessageRecord newMessage, Control control)
        {
            if ((this.GenerateScreenshots) &&
                ((!this.Messages.Any()) || (int)control.TopLevelControl.Handle != this.Messages.Last().WindowHandle))
            {
                Utils.GetScreenshot(control, newMessage.RecordId, this.currentRecordingFolderPath);
            }
        }

        private bool HandleTextBoxTyping(Control control, EventType currentEventType, string controlDisplayName)
        {
            bool handled = false;
            AccessibleRole role = control.AccessibilityObject.Role;

            if ((role == AccessibleRole.Text))
            {
                if (currentEventType == EventType.Click)
                {
                    handled = true;
                }
                else if (this.Messages.Any())
                {
                    MessageRecord previousRecord = this.Messages.Last();

                    // While typing, if it is the same text box, then just replace the previous value with the new one
                    if ((int)control.Handle == previousRecord.Handle)
                    {
                        EventType? previousEventType = previousRecord.EventType;

                        // Check if previous event is same as the current event to make sure we are not updating the previous record for non related new event
                        if (previousEventType == EventType.Type)
                        {
                            control.GetValue(value =>
                            {
                                if (string.Equals(controlDisplayName, value))
                                {
                                    previousRecord.ControlValue = value;
                                }
                            });
                            handled = true;
                        }
                        else if (previousEventType == EventType.Enter)
                        {
                            // Pressing enter does not affect text, so just make it handled
                            handled = true;
                        }
                    }
                }
            }
            return handled;
        }

        private static Control GetControl(IntPtr HWnd)
        {
            Control control = Control.FromHandle(HWnd);
            if (control == null)
            {
                control = Control.FromChildHandle(HWnd);
            }
            return control;
        }

        #endregion
    }
}
