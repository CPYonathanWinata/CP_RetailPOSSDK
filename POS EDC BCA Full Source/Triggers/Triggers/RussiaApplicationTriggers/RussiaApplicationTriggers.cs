/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Localization.Russia.ApplicationTriggers
{
    [Export(typeof(IApplicationTrigger))]
    public sealed class RussiaApplicationTriggers : IApplicationTrigger, IGlobalization
    {
        #region Globalization
        private readonly ReadOnlyCollection<string> supportedCountryRegions = new ReadOnlyCollection<string>(new string[] { SupportedCountryRegion.RU.ToString() });

        public System.Collections.ObjectModel.ReadOnlyCollection<string> SupportedCountryRegions
        {
            get { return supportedCountryRegions; }
        }
        #endregion

        #region IApplicationTriggers Members
        public void Logoff(string operatorId, string name)
        {
            var result = PosApplication.Instance.Services.AxReporting.IsPrintLabelReportTaskStarted();
            if (result.HasValue && result.Value)
            {
                using (var dialog = new LSRetailPosis.POSProcesses.frmMessage(100031, MessageBoxButtons.OK, MessageBoxIcon.Warning, true)) // Printing of labels is in progress and will continue after you log off.
                {
                    POSFormsManager.ShowPOSForm(dialog);
                } 
            }
        }

        public void ApplicationStart()
        {
            // Left empty on purpose.
        }

        public void ApplicationStop()
        {
            // Left empty on purpose.
        }

        public void PostLogon(bool loginSuccessful, string operatorId, string name)
        {
            // Left empty on purpose.
        }

        public void PreLogon(IPreTriggerResult preTriggerResult, string operatorId, string name)
        {
            // Left empty on purpose.
        }

        public void LoginWindowVisible()
        {
            // Left empty on purpose.
        }
        #endregion
    }
}
