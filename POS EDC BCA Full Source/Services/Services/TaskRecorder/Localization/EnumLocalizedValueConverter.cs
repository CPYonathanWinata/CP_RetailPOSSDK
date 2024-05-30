/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.SysTaskRecorder;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Localization
{
    public static class EnumLocalizedValueConverter
    {
        /// <summary>
        /// GetLocalizedEnumValues
        /// </summary>
        /// <typeparam name="TLocalized">Localized Type</typeparam>
        /// <typeparam name="TEnum">Enum Type</typeparam>
        /// <param name="originalColumn"></param>
        /// <returns></returns>
        public static Collection<TLocalized> GetLocalizedEnumValues<TLocalized, TEnum>(ReadOnlyCollection<TEnum> originalColumn) where TLocalized : ILocalizedEnumObject<TEnum>, new()
        {
            if (originalColumn != null)
            {
                Collection<TLocalized> localized = new Collection<TLocalized>();
                TLocalized localizedObject = default(TLocalized);
                foreach (TEnum profile in originalColumn)
                {
                    localizedObject = new TLocalized();
                    localizedObject.EnumValue = profile;
                    localizedObject.BuildLocalizedDisplayName();
                    localized.Add(localizedObject);
                }

                return localized;
            }
            return null;
        }
    }   
}
