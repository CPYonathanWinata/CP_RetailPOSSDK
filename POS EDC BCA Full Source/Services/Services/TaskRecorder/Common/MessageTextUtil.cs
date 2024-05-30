/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Reflection;
using System.Windows.Forms;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Properties;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common
{
    /// <summary>
    /// Util to get message text/instruction based on message record
    /// </summary>
    internal static class MessageTextUtil
    {
        #region public methods
        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <param name="messageRecord">The message record.</param>
        /// <returns>Message text.</returns>
        public static string GetMessageText(MessageRecord messageRecord)
        {
            string helpText = string.Empty;

            if (messageRecord != null)
            {
                EventType? eventType = messageRecord.EventType;
                if (eventType != null)
                {
                    string helpTextFormat = GetLocalizedHelpTextFormat(eventType.Value, messageRecord.ControlRole);
                    switch (eventType.Value)
                    {
                        case EventType.Press:
                        case EventType.Click:
                            {
                                if ((messageRecord.ControlRole == AccessibleRole.Table) ||
                                    (messageRecord.ControlRole == AccessibleRole.PageTabList))
                                {
                                    helpText = Format(helpTextFormat, messageRecord.ControlValue);
                                }
                                else
                                {
                                    helpText = Format(helpTextFormat, messageRecord.ControlDisplayName);
                                }
                                break;
                            }
                        case EventType.Type:
                            {
                                helpText = Format(helpTextFormat, messageRecord.ControlDisplayName, messageRecord.ControlValue);
                                break;
                            }
                        case EventType.Enter:
                            {
                                helpText = Format(helpTextFormat, (58709).Translate()); // ENTER
                                break;
                            }
                    }
                }
            }

            return helpText;
        }

        /// <summary>
        /// Formats the message text with sequence number.
        /// </summary>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="helpText">The help text.</param>
        /// <returns>Formatted sequence number.</returns>
        public static string FormatMessageTextWithSequenceNumber(int sequenceNumber, string helpText)
        {
            return string.Format(ApplicationLocalizer.Language.Translate(58429), sequenceNumber, helpText); // {0}. {1}
        }
        #endregion

        #region private methods
        private static string Format(string format, string arg0)
        {
            return string.Format(format, arg0.Quote());
        }

        private static string Format(string format, string arg0, string arg1)
        {
            return string.Format(format, arg0.Quote(), arg1.Quote());
        }

        private static string Quote(this string stringToQuote)
        {
            return string.Format("'{0}'", stringToQuote);
        }

        private static string GetLocalizedHelpTextFormat(EventType eventType, AccessibleRole controlRole)
        {
            string propertyValue = null;
            string propertyName = string.Format("{0}_{1}", eventType.ToString(), controlRole.ToString());
            propertyValue = GetLocalizedMessage(propertyName);

            if (propertyValue == null)
            {
                propertyName = eventType.ToString();
                propertyValue = GetLocalizedMessage(propertyName);
            }

            return propertyValue;
        }

        private static string GetLocalizedMessage(string propertyName)
        {
            string propertyValue = null;

            switch (propertyName)
            {
                case "Click":
                    propertyValue = ApplicationLocalizer.Language.Translate(58700); // Click {0}
                    break;
                case "Click_PushButton":
                    propertyValue = ApplicationLocalizer.Language.Translate(58701); // Click {0} button
                    break;
                case "Click_RadioButton":
                case "Press_RadioButton":
                    propertyValue = ApplicationLocalizer.Language.Translate(58702); // Select {0} radio button
                    break;
                case "Click_Table":
                    propertyValue = ApplicationLocalizer.Language.Translate(58703); // Select {0} from table
                    break;
                case "Enter":
                    propertyValue = ApplicationLocalizer.Language.Translate(58704); // Press {0}
                    break;
                case "ErrorDialogTitleFormat":
                    propertyValue = ApplicationLocalizer.Language.Translate(58705); // Error - POS Task Recorder
                    break;
                case "ErrorWhileRetrievingData":
                    propertyValue = ApplicationLocalizer.Language.Translate(58706); // Error while retrieving initial data.
                    break;
                case "Type":
                    propertyValue = ApplicationLocalizer.Language.Translate(58707); // Change {0} to {1}
                    break;
                case "Type_Text":
                    propertyValue = ApplicationLocalizer.Language.Translate(58708); // Change {0} to {1}
                    break;
                case "Press_PushButton":
                    propertyValue = ApplicationLocalizer.Language.Translate(58453); // Press {0} button
                    break;
                case "Click_PageTabList":
                    propertyValue = ApplicationLocalizer.Language.Translate(58454); // Switch to {0} tab
                    break;
            }

            return propertyValue;
        }
        #endregion
    }
}
