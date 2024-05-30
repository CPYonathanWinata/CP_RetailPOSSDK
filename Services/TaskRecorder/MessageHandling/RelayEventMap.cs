/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling
{
    /// <summary>
    /// Maps raw windows message envents to events meaninfule for the recorder
    /// </summary>
    public static class RelayEventMap
    {
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="role">The role.</param>
        /// <returns>
        /// Event type.
        /// </returns>
        public static EventType? GetEventType(Message message, AccessibleRole role)
        {
            MessageType messageType = (MessageType)message.Msg;
            return GetEventType(messageType, message, role);
        }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <param name="messageRecord">The message record.</param>
        /// <param name="role">The role.</param>
        /// <returns>
        /// Event type.
        /// </returns>
        public static EventType? GetEventType(MessageRecord messageRecord, AccessibleRole role)
        {
            if (messageRecord == null)
            {
                return null;
            }
            MessageType messageType = messageRecord.MessageType;
            return GetEventType(messageType, null, role);
        }

        private static EventType? GetEventType(MessageType messageType, Message? message, AccessibleRole role)
        {
            EventType? actualEventType = null;
            switch (messageType)
            {
                case MessageType.WM_LBUTTONDOWN:
                case MessageType.WM_RBUTTONDOWN:
                    {
                        actualEventType = EventType.Click;
                        break;
                    }
                case MessageType.WM_KEYDOWN:
                    {
                        actualEventType = HandleKeyboardNavigation(message, role, actualEventType);

                        break;
                    }
                case MessageType.WM_KEYUP:
                    {
                        if ((role == AccessibleRole.PushButton) || (role == AccessibleRole.RadioButton))
                        {
                            Message msg = message.Value;
                            NativeMethods.TranslateMessage(ref msg);
                        }
                        else
                        {
                            actualEventType = EventType.Type;
                        }
                        break;
                    }
                case MessageType.WM_CHAR:
                    {
                        if (!((role == AccessibleRole.PushButton) || (role == AccessibleRole.RadioButton)))
                        {
                            actualEventType = EventType.Type;
                        }
                        break;
                    }
            }
            return actualEventType;
        }

        #region private methods

        private static EventType? HandleKeyboardNavigation(Message? message, AccessibleRole role, EventType? actualEventType)
        {
            if (message != null)
            {
                var key = (MessageType)message.Value.WParam;

                // Ignore all tabs, they are just navigation (because we do not have any text area which supports adding tabs as part of text)
                if (key == MessageType.VK_TAB)
                {
                    return null;
                }

                // If spacebar or enter is pressed on a button, then we say it is pressed
                if ((role == AccessibleRole.PushButton) || (role == AccessibleRole.RadioButton))
                {
                    if ((key == MessageType.VK_SPACE) ||
                        (key == MessageType.VK_RETURN))
                    {
                        actualEventType = EventType.Press;
                    }
                    else if (((key == MessageType.VK_LEFT) ||
                                (key == MessageType.VK_UP) ||
                                (key == MessageType.VK_RIGHT) ||
                                (key == MessageType.VK_DOWN)) &&
                            (role == AccessibleRole.RadioButton))
                    {
                        actualEventType = EventType.Press;
                    }
                }
                else if (key == MessageType.VK_RETURN)
                {
                    actualEventType = EventType.Enter;
                }
            }
            return actualEventType;
        }

        #endregion
    }
}
