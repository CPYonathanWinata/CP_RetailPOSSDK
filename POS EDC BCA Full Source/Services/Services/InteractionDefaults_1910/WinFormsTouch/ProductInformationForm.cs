/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Settings;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Notification.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    /// <summary>
    /// Summary description for ProductInformationForm
    /// </summary>
    [Export("ProductInformationForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ProductInformationForm : frmTouchBase, IInteractionView 
    {
        public ProductInformationForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void TranslateLabels()
        {   
            lblVersion.Text = ApplicationLocalizer.Language.Translate(41); //Version: 
            lblVersion2.Text = ApplicationSettings.ApplicationVersion;
            lblLicenseTerms.Text = ApplicationLocalizer.Language.Translate(49); // View license agreement      
            lblCountry2.Text = Functions.CountryRegion.ToString();
            buttonOK.Text = ApplicationLocalizer.Language.Translate(50158); //OK
        }

        private void lblLicenseTerms_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(ApplicationSettings.EulaFile);
            }
            catch (System.IO.FileNotFoundException fnfe)
            {
                Diagnostics.NetTracer.Error(fnfe, fnfe.Source);
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                Diagnostics.NetTracer.Error(we, we.Source);
            }
        }

        #region IInteractionView implementation

        /// <summary>
        /// Initialize the form
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type</typeparam>
        /// <param name="args">Notification</param>
        public void Initialize<TArgs>(TArgs args)
             where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call
        /// </summary>
        /// <typeparam name="TResults"></typeparam>
        /// <returns>Returns the TResults object</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new ExtendedLogOnConfirmation
            {
                Confirmed = this.DialogResult == DialogResult.OK,
            } as TResults;
        }

        #endregion
    }
}
