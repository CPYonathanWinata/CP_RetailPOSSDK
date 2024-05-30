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
using System.Text;
using System.Threading.Tasks;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.SysTaskRecorder;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Localization
{
    public class LocalizedSysTaskRecorderUsageProfile : ILocalizedEnumObject<SysTaskRecorderUsageProfile>
    {
        public SysTaskRecorderUsageProfile EnumValue { get; set; }
        public string DisplayName { get; set; }

        public void BuildLocalizedDisplayName()
        {
            switch (EnumValue)
            {
                case SysTaskRecorderUsageProfile.None:
                    DisplayName = ApplicationLocalizer.Language.Translate(58300);
                    break;

                case SysTaskRecorderUsageProfile.Master:
                    DisplayName = ApplicationLocalizer.Language.Translate(58301);
                    break;

                case SysTaskRecorderUsageProfile.Setup:
                    DisplayName = ApplicationLocalizer.Language.Translate(58302);
                    break;

                case SysTaskRecorderUsageProfile.Transaction:
                    DisplayName = ApplicationLocalizer.Language.Translate(58303);
                    break;
                default:
                    DisplayName = EnumValue.ToString();
                    break;
            }
        }
    }
}
