/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;

namespace Microsoft.Dynamics.Retail.FiscalRegistrationServices
{
    /// <summary>
    /// Helper for exception raising activities.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ThrowException(int resourceId, params object[] args)
        {
            ThrowException(true, resourceId, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="promptUser"></param>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ThrowException(bool promptUser, int resourceId, params object[] args)
        {
            ThrowException(promptUser, resourceId, null, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="promptUser"></param>
        /// <param name="resourceId"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public static void ThrowException(bool promptUser, int resourceId, Exception innerException, params object[] args)
        {
            var message = Resources.Translate(resourceId, args);

            if (promptUser)
            {
                //Make sure the user can click on OK
                SafeNativeMethodsHelper.BlockMouseAndKeyboard(false);
                ShowException(message);
            }
            LogHelper.LogError("RaiseException", message);
            if (innerException == null)
            {
                throw new FiscalRegisterException(resourceId, args);
            }
            else
            {
                throw new FiscalRegisterException(resourceId, innerException, args);
            }
        }

        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="errorNumber"> </param>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ThrowException(int errorNumber, int resourceId, params object[] args)
        {
            ThrowException(true, errorNumber, resourceId, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="promptUser"></param>
        /// <param name="errorNumber"> </param>
        /// <param name="resourceId"></param>
        /// <summary>
        /// <param name="args"></param>
        public static void ThrowException(bool promptUser, int errorNumber, int resourceId, params object[] args)
        {
            ThrowException(promptUser, errorNumber, resourceId, null, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="promptUser"></param>
        /// <param name="errorNumber"></param>
        /// <param name="resourceId"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public static void ThrowException(bool promptUser, int errorNumber, int resourceId, Exception innerException, params object[] args)
        {
            var message = Resources.Translate(resourceId, args);

            if (promptUser)
            {
                //Make sure the user can click on OK
                SafeNativeMethodsHelper.BlockMouseAndKeyboard(false);
                ShowException(message);
            }
            LogHelper.LogError("RaiseException", message);
            if (innerException == null)
            {
                throw new FiscalRegisterException(errorNumber, resourceId, args);
            }
            else
            {
                throw new FiscalRegisterException(errorNumber, resourceId, innerException, args);
            }
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        public static void ThrowException(string message)
        {
            ThrowException(true, message);
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        public static void ThrowException(bool promptUser, string message)
        {
            ThrowException(promptUser, message, null);
        }

        /// <summary>
        /// Show the exception message to the user, logs it and raises the fiscal register exception.
        /// </summary>
        /// <param name="promptUser"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public static void ThrowException(bool promptUser, string message, Exception innerException)
        {
            if (promptUser)
            {
                //Make sure the user can click on OK
                SafeNativeMethodsHelper.BlockMouseAndKeyboard(false);
                ShowException(message);
            }
            LogHelper.LogError("RaiseException", message);

            if (innerException == null)
            {
                throw new FiscalRegisterException(message);
            }
            else
            {
                throw new FiscalRegisterException(message, innerException);
            }
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="message"></param>
        public static void ShowAndLogException(string message)
        {
            ShowAndLogException("RaiseException", message, null);
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void ShowAndLogException(string source, string message)
        {
            ShowAndLogException(source, message, null);
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public static void ShowAndLogException(string source, string message, Exception innerException)
        {
            //Make sure the user can click on OK
            SafeNativeMethodsHelper.BlockMouseAndKeyboard(false);
            ShowException(message);

            if (innerException == null)
            {
                LogHelper.LogException(new FiscalRegisterException(message), source, message);
            }
            else
            {
                LogHelper.LogException(new FiscalRegisterException(message, innerException), source, message);
            }
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ShowAndLogException(int resourceId, params object[] args)
        {
            ShowAndLogException("RaiseException", resourceId, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ShowAndLogException(string source, int resourceId, params object[] args)
        {
            ShowAndLogException(source, resourceId, null, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="resourceId"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public static void ShowAndLogException(string source, int resourceId, Exception innerException, params object[] args)
        {
            var message = Resources.Translate(resourceId, args);

            //Make sure the user can click on OK
            SafeNativeMethodsHelper.BlockMouseAndKeyboard(false);
            ShowException(message);
            LogHelper.LogError("RaiseException", message);
            if (innerException == null)
            {
                LogHelper.LogException(new FiscalRegisterException(resourceId, args), source, message);
            }
            else
            {
                LogHelper.LogException(new FiscalRegisterException(resourceId, innerException, args), source, message);
            }
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ShowAndLogException(int errorNumber, int resourceId, params object[] args)
        {
            ShowAndLogException("RaiseException", resourceId, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorNumber"></param>
        /// <param name="resourceId"></param>
        /// <param name="args"></param>
        public static void ShowAndLogException(string source, int errorNumber, int resourceId, params object[] args)
        {
            ShowAndLogException(source, errorNumber, resourceId, null, args);
        }

        /// <summary>
        /// Show the exception message to the user, logs it but does not raise the fiscal register exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorNumber"></param>
        /// <param name="resourceId"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public static void ShowAndLogException(string source, int errorNumber, int resourceId, Exception innerException, params object[] args)
        {
            var message = Resources.Translate(resourceId, args);

            //Make sure the user can click on OK
            SafeNativeMethodsHelper.BlockMouseAndKeyboard(false);
            ShowException(message);
            LogHelper.LogError("RaiseException", message);
            if (innerException == null)
            {
                LogHelper.LogException(new FiscalRegisterException(errorNumber, resourceId, args), source, message);
            }
            else
            {
                LogHelper.LogException(new FiscalRegisterException(errorNumber, resourceId, innerException, args), source, message);
            }
        }

        /// <summary>
        /// Display a window with a exception error message.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public static void ShowException(string message)
        {
            using (var form = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                ShowPosForm(form);
            }
        }

        /// <summary>
        /// Shows main POS window if it is not Null and handle created, otherwise invokes posForm.ShowDialog()
        /// </summary>
        /// <param name="posForm">POS form</param>
        private static void ShowPosForm(Form posForm)
        {
            var posMainWindow = LSRetailPosis.ApplicationFramework.POSMainWindow as Control;

            if ((posMainWindow != null) && posMainWindow.IsHandleCreated)
            {
                LSRetailPosis.POSControls.POSFormsManager.ShowPOSForm(posForm);
            }
            else
            {
                posForm.ShowDialog();
            }
        }
    }
}