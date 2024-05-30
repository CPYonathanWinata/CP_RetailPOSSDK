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
    public class LocalizedSysModule : ILocalizedEnumObject<SysModule>
    {
        public SysModule EnumValue { get; set; }
        public string DisplayName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public void BuildLocalizedDisplayName()
        {
            switch (EnumValue)
            {
                case SysModule.None:
                    DisplayName = ApplicationLocalizer.Language.Translate(58100);
                    break;
                case SysModule.Ledger:
                    DisplayName = ApplicationLocalizer.Language.Translate(58101);
                    break;
                case SysModule.Cust:
                    DisplayName = ApplicationLocalizer.Language.Translate(58102);
                    break;
                case SysModule.Vend:
                    DisplayName = ApplicationLocalizer.Language.Translate(58103);
                    break;
                case SysModule.Sales:
                    DisplayName = ApplicationLocalizer.Language.Translate(58104);
                    break;
                case SysModule.Purch:
                    DisplayName = ApplicationLocalizer.Language.Translate(58105);
                    break;
                case SysModule.Invent:
                    DisplayName = ApplicationLocalizer.Language.Translate(58106);
                    break;
                case SysModule.Prod:
                    DisplayName = ApplicationLocalizer.Language.Translate(58107);
                    break;
                case SysModule.Project:
                    DisplayName = ApplicationLocalizer.Language.Translate(58108);
                    break;
                case SysModule.Payroll:
                    DisplayName = ApplicationLocalizer.Language.Translate(58109);
                    break;
                case SysModule.SalesMarketing:
                    DisplayName = ApplicationLocalizer.Language.Translate(58110);
                    break;
                case SysModule.FixedAssets:
                    DisplayName = ApplicationLocalizer.Language.Translate(58111);
                    break;
                case SysModule.Tax:
                    DisplayName = ApplicationLocalizer.Language.Translate(58112);
                    break;
                case SysModule.Bank:
                    DisplayName = ApplicationLocalizer.Language.Translate(58113);
                    break;
                case SysModule.System:
                    DisplayName = ApplicationLocalizer.Language.Translate(58114);
                    break;
                case SysModule.WMS:
                    DisplayName = ApplicationLocalizer.Language.Translate(58115);
                    break;
                case SysModule.FixedAssets_RU:
                    DisplayName = ApplicationLocalizer.Language.Translate(58116);
                    break;
                case SysModule.KMQuestionnaire:
                    DisplayName = ApplicationLocalizer.Language.Translate(58117);
                    break;
                case SysModule.HRM:
                    DisplayName = ApplicationLocalizer.Language.Translate(58118);
                    break;
                case SysModule.Costing:
                    DisplayName = ApplicationLocalizer.Language.Translate(58119);
                    break;
                case SysModule.Expense:
                    DisplayName = ApplicationLocalizer.Language.Translate(58120);
                    break;
                case SysModule.Employee_RU:
                    DisplayName = ApplicationLocalizer.Language.Translate(58121);
                    break;
                case SysModule.RCash:
                    DisplayName = ApplicationLocalizer.Language.Translate(58122);
                    break;
                case SysModule.Retail:
                    DisplayName = ApplicationLocalizer.Language.Translate(58123);
                    break;
                case SysModule.RDeferrals:
                    DisplayName = ApplicationLocalizer.Language.Translate(58124);
                    break;   
                default :
                    DisplayName = EnumValue.ToString();
                    break;
            }
        }
    }
}
