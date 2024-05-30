/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using LSRetailPosis.Settings.HardwareProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using FiscalRegisterProfile = LSRetailPosis.Settings.HardwareProfiles.FiscalRegister;

namespace Microsoft.Dynamics.Retail.FiscalRegistrationServices
{
    /// <summary>
    /// Factory that creates fiscal register instance.
    /// </summary>
    public class FiscalRegisterFactory
    {
        public const string FiscalRegistersFolder = "FiscalRegistrationServices";
        
        private FiscalRegisterFactory() { }

        static FiscalRegisterFactory()
        {
            Instance = new FiscalRegisterFactory();
        }

        /// <summary>
        /// Singleton instance of FiscalRegisterFactory.
        /// </summary>
        public static FiscalRegisterFactory Instance { get; private set; }

        /// <summary>
        /// Create fiscal register instance.
        /// </summary>
        /// <returns></returns>
        public IFiscalRegisterDriver CreateFiscalRegister()
        {
            IFiscalRegisterDriver fiscalRegister = null;

            if (FiscalRegisterProfile.DriverType == FiscalRegisterDriver.ManufacturerDriver)
            {
                string assemblyName = null;
                string className = null;
                string fullAssemblyPath = null;

                try
                {
                    var storeCountry = Functions.CountryRegion.ToString();
                    assemblyName = (string)Properties.Settings.Default["FiscalRegisterAssembly_" + storeCountry];
                    className = (string)Properties.Settings.Default["FiscalRegisterClass_" + storeCountry];

                    //Remove invalid chars
                    assemblyName = RemoveInvalidChars(assemblyName, Path.GetInvalidPathChars());
                    assemblyName = Environment.ExpandEnvironmentVariables(assemblyName);
                    className = RemoveInvalidChars(className, Path.GetInvalidFileNameChars());

                    fullAssemblyPath = Path.IsPathRooted(assemblyName)
                        ? assemblyName
                        : Path.Combine(ApplicationSettings.GetAppPath(), FiscalRegistersFolder, assemblyName);

                    Assembly driverAssembly = Assembly.LoadFrom(fullAssemblyPath);
                    var driverType = driverAssembly.GetType(className);
                    fiscalRegister = (IFiscalRegisterDriver)Activator.CreateInstance(driverType);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.ShowAndLogException("FiscalRegisterFactory.CreateFiscalRegister",
                        string.Format("Exception: {0}, AssemblyPath: {1}, ClassName: {2}",
                        ex.Message, fullAssemblyPath, className));
                    fiscalRegister = new ExceptionalFiscalRegister();
                }
            }

            return fiscalRegister; 
        }

        private static string RemoveInvalidChars(string name, char[] invalidChars)
        {
            var regexName = new Regex(string.Format("[{0}]", Regex.Escape(new string(invalidChars))));
            var cleanedName = regexName.Replace(name, string.Empty).Trim();

            return cleanedName;
        }
    }
}