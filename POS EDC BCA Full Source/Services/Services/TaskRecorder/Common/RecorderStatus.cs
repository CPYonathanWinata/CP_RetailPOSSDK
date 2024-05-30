/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common
{
    /// <summary>
    /// Recorder status
    /// </summary>
    public enum RecorderStatus
    {
        /// <summary>
        /// The stopped status
        /// </summary>
        Stopped,

        /// <summary>
        /// The recording status
        /// </summary>
        Recording,

        /// <summary>
        /// The paused status
        /// </summary>
        Paused,

        /// <summary>
        /// Status while stopping / generating documents
        /// </summary>
        Stopping
    }
}
