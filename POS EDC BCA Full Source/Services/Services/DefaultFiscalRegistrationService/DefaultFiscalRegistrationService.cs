/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.SystemCore;
using Microsoft.Dynamics.Retail.FiscalRegistrationServices;

namespace Microsoft.Dynamics.Retail.POS.FiscalRegistrationServices.DefaultFiscalRegistrationService
{
    /// <summary>
    /// Default fiscal registration service. Transfers calls to loaded fiscal registration service.
    /// </summary>
    [Export(typeof(IFiscalRegister))]
    public class DefaultFiscalRegistrationService : IFiscalRegister
    {
        private IFiscalRegisterDriver customFiscalRegister = null;
        private bool registrationSuccessful = true;

        public string RegisterFiscalData(string serializedFiscalTransaction)
        {
            string response = null;

            if (customFiscalRegister != null)
            {
                registrationSuccessful = false;
                response = customFiscalRegister.RegisterFiscalData(serializedFiscalTransaction);
                registrationSuccessful = true;
            }

            return response;
        }

        public void Initialize()
        {
            customFiscalRegister = FiscalRegisterFactory.Instance.CreateFiscalRegister();

            if (customFiscalRegister != null)
                customFiscalRegister.Initialize(ConfigLoader.GetConfigFilename());
        }

        public void Uninitialize()
        {
            if (customFiscalRegister != null)
                customFiscalRegister.Uninitialize();
        }

        public bool FiscalRegisterEnabled()
        {
            return customFiscalRegister != null;
        }

        public bool HasReceiptId()
        {
            return customFiscalRegister != null && customFiscalRegister.CurrentReceiptId != null;
        }

        public string GetNextReceiptId(IPosTransaction transaction)
        {
            if (customFiscalRegister != null)
            {
                if (customFiscalRegister.HasReceiptNumbering())
                    return customFiscalRegister.CurrentReceiptId ?? (customFiscalRegister.CurrentReceiptId = customFiscalRegister.GetNextReceiptId());
                else
                    return customFiscalRegister.CurrentReceiptId ?? (customFiscalRegister.CurrentReceiptId = PosApplication.Instance.Services.ApplicationService.GetNextReceiptId(transaction));

            }
            return string.Empty;
        }

        public void ClearReceiptId()
        {
            if ((customFiscalRegister != null) && (registrationSuccessful))
                customFiscalRegister.CurrentReceiptId = null;
        }
    }
}