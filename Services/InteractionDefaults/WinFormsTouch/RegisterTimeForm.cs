/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Interaction
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Forms;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.Interaction.ViewModel;
    using Microsoft.Dynamics.Retail.Pos.SystemCore;

    /// <summary>
    /// Time registration form
    /// </summary>
    [Export("RegisterTimeForm", typeof(IInteractionView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RegisterTimeForm : frmTouchBase, IInteractionView
    {
        private RegisterTimeFormViewModel               viewModel;

        private const int ClockInTextResourceId         = 51355;    //Clock-in
        private const int ClockOutTextResourceId        = 51354;    //Clock-out
        private const int BreakForLunchTextResourceId   = 51352;    //Break for lunch
        private const int BreakFromWorkTextResourceId   = 51353;    //Break from work
        private const int LogbookTextResourceId         = 51351;    //Logbook
        private const int RegisterTimeTitleResourceId   = 51358;    //Register time
        private const int RegisterTimeFormCloseId       = 51359;    //Close
        private const string BreakForLunch = "LunchBrk";
        private const string BreakFromWork = "DailyBrks";
        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterTimeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterTimeForm"/> class.
        /// </summary>
        /// <param name="RegisterTimeNotification">Notification object.</param>
        [ImportingConstructor]
        public RegisterTimeForm(RegisterTimeNotification RegisterTimeNotification)
            : this()
        {
            if (RegisterTimeNotification == null)
            {
                throw new ArgumentNullException("RegisterTimeNotification");
            }
        }

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <typeparam name="TArgs">Prism Notification type.</typeparam>
        /// <param name="args">Notification.</param>
        public void Initialize<TArgs>(TArgs args)
            where TArgs : Microsoft.Practices.Prism.Interactivity.InteractionRequest.Notification
        {
            if (args == null)
                throw new ArgumentNullException("args");
        }

        /// <summary>
        /// Return the results of the interation call.
        /// </summary>
        /// <typeparam name="TResults">The notification result.</typeparam>
        /// <returns>Returns the TResults object.</returns>
        public TResults GetResults<TResults>() where TResults : class, new()
        {
            return new RegisterTimeNotification() as TResults;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                // Data bindings
                btnClockOut.DataBindings["Enabled"].Format      += new ConvertEventHandler(OnConvertTimeClockStateClockOutToBool);
                btnClockIn.DataBindings["Enabled"].Format       += new ConvertEventHandler(OnConvertTimeClockStateClockInToBool);
                btnBreakForLunch.DataBindings["Enabled"].Format += new ConvertEventHandler(OnConvertTimeClockStateBreakFromWorkToBool);
                btnBreakFromWork.DataBindings["Enabled"].Format += new ConvertEventHandler(OnConvertTimeClockStateBreakFromWorkToBool);
                lblCurrentStatus.DataBindings["Text"].Format    += new ConvertEventHandler(OnConvertTimeClockStateToStatus);
                lblLastActivity.DataBindings["Text"].Format     += new ConvertEventHandler(OnConvertLastActivityToFormattedText);
                
                // Create view model
                viewModel = new RegisterTimeFormViewModel();
                bindingSource.Add(viewModel);

                TranslateLabels();
            }

            base.OnLoad(e);
        }

        private void OnConvertLastActivityToFormattedText(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string))
            {
                e.Value = ApplicationLocalizer.Language.Translate(51357, e.Value); //51357 Last activity at: {0}
            }
        }

        private void OnConvertTimeClockStateToStatus(object sender, ConvertEventArgs e)
        {
            string translatedText;
            if (e.DesiredType == typeof(string))
            {
                switch (viewModel.RegisterBreakType)
                {
                    case BreakFromWork:
                        translatedText = ApplicationLocalizer.Language.Translate(BreakFromWorkTextResourceId);
                        e.Value = ApplicationLocalizer.Language.Translate(translatedText, BreakFromWorkTextResourceId);
                        break;
                    case BreakForLunch:
                        translatedText = ApplicationLocalizer.Language.Translate(BreakForLunchTextResourceId);
                        e.Value = ApplicationLocalizer.Language.Translate(translatedText, BreakForLunchTextResourceId);
                        break;
                    default:
                        e.Value = ApplicationLocalizer.Language.Translate(51356, TranslateTimeClockState((TimeClockType)e.Value)); //51356 Current status at: {0}
                        break;
                }
            }
        }
        
        private void OnConvertTimeClockStateClockInToBool(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = !((TimeClockType)e.Value == TimeClockType.ClockIn);
            }
        }

        private void OnConvertTimeClockStateClockOutToBool(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                TimeClockType status = (TimeClockType)e.Value;
                e.Value = !(status == TimeClockType.BreakFlowStart || status == TimeClockType.ClockOut || status == TimeClockType.None);
            }
        }

        private void OnConvertTimeClockStateBreakFromWorkToBool(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(bool))
            {
                e.Value = !(viewModel.RegisterBreakType == BreakFromWork 
                                || viewModel.RegisterBreakType == BreakForLunch 
                                || ((viewModel.RegisterTimeType == TimeClockType.ClockOut) || (viewModel.RegisterTimeType == TimeClockType.None))); //Disable Clock-out, BreakFromWork and BreakForLunch buttons.
            }
        }

        /// <summary>
        /// Translate labels
        /// </summary>
        private void TranslateLabels()
        {
            btnClockIn.Text         = ApplicationLocalizer.Language.Translate(ClockInTextResourceId);
            btnClockOut.Text        = ApplicationLocalizer.Language.Translate(ClockOutTextResourceId);
            btnBreakFromWork.Text   = ApplicationLocalizer.Language.Translate(BreakFromWorkTextResourceId);
            btnBreakForLunch.Text   = ApplicationLocalizer.Language.Translate(BreakForLunchTextResourceId);
            btnLogbook.Text         = ApplicationLocalizer.Language.Translate(LogbookTextResourceId);
            btnClose.Text           = ApplicationLocalizer.Language.Translate(RegisterTimeFormCloseId);

            labelTitle.Text = ApplicationLocalizer.Language.Translate(RegisterTimeTitleResourceId);
        }

        private static string TranslateTimeClockState(TimeClockType state)
        {
            string translatedText = null;

            switch (state)
            {
                case TimeClockType.ClockIn:
                    translatedText = ApplicationLocalizer.Language.Translate(ClockInTextResourceId);
                    break;
                case TimeClockType.ClockOut:
                    translatedText = ApplicationLocalizer.Language.Translate(ClockOutTextResourceId);
                    break;
                case TimeClockType.BreakFlowStart:
                    translatedText = ApplicationLocalizer.Language.Translate(BreakForLunchTextResourceId);
                    break;
                default:
                    break;
            }

            return translatedText ?? string.Empty;
        }

        private void btnClockOut_Click(object sender, EventArgs e)
        {
            TimeClockAccess(51348, TimeClockType.ClockOut); //You successfully clocked-out at: {0}
        }

        private void btnClockIn_Click(object sender, EventArgs e)
        {
            TimeClockAccess(51347, TimeClockType.ClockIn); // You successfully clocked-in at: {0}
        }

        private void btnBreakFromWork_Click(object sender, EventArgs e)
        {
            TimeClockAccess(51376, BreakType.BreakFromWork); // You are on a break from work
        }

        private void btnBreakForLunch_Click(object sender, EventArgs e)
        {
            TimeClockAccess(51375, BreakType.BreakForLunch); // You are on a break for lunch
        }

        private void TimeClockAccess(int message, TimeClockType operation)
        {
            bool result = viewModel.RegisterTimeClock(operation);
            ShowMessage(message, result);
            Close();
        }

        private void TimeClockAccess(int message, BreakType breakType)
        {
            bool result = viewModel.RegisterTimeClock(breakType);
            ShowMessage(message, result);
            Close();
        }

        private void btnLogbook_Click(object sender, EventArgs e)
        {
            using (LogbookForm logbookForm = new LogbookForm())
            {
                PosApplication.Instance.ApplicationFramework.POSShowForm(logbookForm);
            }
        }

        private void ShowMessage(int messageId, bool result)
        {
            string messageText = result
                ? ApplicationLocalizer.Language.Translate(messageId, viewModel.LastActivity)
                : viewModel.LastActivity;

            PosApplication.Instance.Services.Dialog.ShowMessage(
                messageText, MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}