/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Dynamics.Retail.FiscalRegistrationServices
{
    /// <summary>
    /// Fiscal register that is created by FiscalRegisterFactory if intialization of correct fiscal register instance was failed
    /// Shows and/or throws FiscalRegisterException for every call 
    /// </summary>
    public class ExceptionalFiscalRegister : IFiscalRegisterDriver
    {
        public string RegisterFiscalData(string serializedFiscalTransaction)
        {
            ExceptionHelper.ThrowException("Fiscal register is not initialized");
            return null;
        }

        public void Initialize(string configFileName)
        {
            ExceptionHelper.ShowAndLogException("Fiscal register is not initialized");
        }

        public void Uninitialize()
        {
            ExceptionHelper.ShowAndLogException("Fiscal register is not initialized");
        }

        public bool HasReceiptNumbering()
        {
            ExceptionHelper.ThrowException("Fiscal register is not initialized");
            return false;
        }

        public string GetNextReceiptId()
        {
            ExceptionHelper.ThrowException("Fiscal register is not initialized");
            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "Grandfather")]
        public string CurrentReceiptId
        {
            get
            {
                ExceptionHelper.ThrowException("Fiscal register is not initialized");
                return null;
            }
            set
            {
                ExceptionHelper.ThrowException("Fiscal register is not initialized");
            }
        }
    }
}
