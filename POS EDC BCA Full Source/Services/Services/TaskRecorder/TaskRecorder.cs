/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.ComponentModel.Composition;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Views;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder
{
    // Get all text through the Translation function in the ApplicationLocalizer
    //
    // TextID's for the TaskRecorder service are reserved at 58000 - 58999
    // TextID's for the following modules are as follows:
    //
    // General                                      58400 - 58500. The last in use: 58456
    // frmNewNode.cs:                               58000 - 58050. The last in use: 58001
    // LocalizedSysModule.cs                        58100 - 58199. The last in use: 58124
    // LocalizedSysTaskRecorderOperationGroup.cs    58200 - 58299. The last in use: 58202
    // LocalizedSysTaskRecorderUsageProfile.cs      58300 - 58399. The last in use: 58303
    // SetupForm.cs:                                58600 - 58650. The last in use: 58605
    // MessageTextUtil.cs                           58700 - 58799. The last in use: 58708
    // MessageTextUtil.cs                           58800 - 58899. This is used for step sequence number.


    [Export(typeof(ITaskRecorder))]
    public sealed class TaskRecorder : ITaskRecorder
    {
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        [Import]
        public IApplication Application { get; set; }

        /// <summary>
        /// Starts task recorder.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Windows Forms automatically disposes modeless windows when they are closed.")]
        public void Record()
        {
            RecorderForm form = new RecorderForm();
            this.Application.ApplicationFramework.POSShowFormModeless(form);
        }
    }
}
