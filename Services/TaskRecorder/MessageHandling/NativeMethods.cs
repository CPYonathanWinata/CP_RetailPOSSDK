/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.MessageHandling
{
    /// <summary>
    /// Provides native methods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Translates virtual-key messages into character messages. 
        /// The character messages are posted to the calling thread's message queue, to be read the next time the thread calls the GetMessage or PeekMessage function
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that contains message information retrieved from the calling thread's message queue by using the GetMessage or PeekMessage function.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref Message lpMsg);
    }

}
