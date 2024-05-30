/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Dynamics.Retail.Pos.SystemCore;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    public partial class TaggedImageBoxControl : UserControl
    {

        private List<byte> imageStream;
        
        private Object lockObj = new Object();

        private MemoryStream oldImageStream = null;

        private bool initialized = false;

        /// <summary>
        /// A flag that checks if the form is closed. If it is, image update thread should terminate.
        /// </summary>
        private bool executionComplete = true;

        /// <summary>
        /// If Extended log on registration is active, this is set to true
        /// </summary>
        private bool enrollmentModeActive = false;
        
        public bool IsActive 
        {
            get 
            { 
                return !executionComplete; 
            }
        }

        public Image CurrentImage
        {
            get
            {
                return TaggedImage.Image;
            }
        }


        public TaggedImageBoxControl()
        {
            InitializeComponent();
        }

        public void Initialize(object stream)
        {
            Initialize(stream, false);
        }

        public void Initialize(object stream, bool enrollMode)
        {
            lock (lockObj)
            {
                executionComplete = false;
            }
            this.imageStream = stream as List<byte>;
            Task.Factory.StartNew(new Action(StartUpdate));
            this.enrollmentModeActive = enrollMode;
            this.initialized = true;
        }

        /// <summary>
        /// Updates the image control with the latest imageStream data
        /// </summary>
        /// <param name="parameter"></param>
        private void StartUpdate()
        {
            while (!executionComplete)
            {
               UpdateTaggedImage();
            }
        }

        /// <summary>
        /// Delegate invoked by the running thread. This updates the image control with the latest imageStream.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification="tempImageStream disposed via oldImageStream")]
        private void UpdateTaggedImage()
        {
            if (imageStream.Count > 0)
            {
                MemoryStream tempImageStream = new MemoryStream();
                try
                {
                    var imageStreamAsArray = imageStream.ToArray();
                    tempImageStream.Write(imageStreamAsArray, 0, imageStreamAsArray.Length);
                    tempImageStream.Seek(0, SeekOrigin.Begin);
                    lock (lockObj)
                    {
                        if (!executionComplete)
                        {
                            try
                            {
                                TaggedImage.Image = Image.FromStream(tempImageStream);
                            }
                            catch (ArgumentException e)
                            {
                                // we ignore this because plugin might not have refreshed the image totally
                                Debug.WriteLine("error: " + e.StackTrace);
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    LSRetailPosis.ApplicationExceptionHandler.HandleException("Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch.TaggedImageBoxControl", e);
                    // locked to prevent EndImageRefresh creating a race condition to close the image stream
                    lock (lockObj)
                    {
                        //shut down the tempImageStream
                        if (tempImageStream != null)
                        {
                            tempImageStream.Close();
                            tempImageStream = null;
                        }
                    }
                }
                finally
                {
                    // Dispose the previous stream only when a new stream's reference is with TaggedImage
                    if (oldImageStream != null)
                    {
                        oldImageStream.Close();
                        oldImageStream = null;
                    }
                    oldImageStream = tempImageStream;
                }
            }
        }
        

        private void TaggedImage_Click(object sender, EventArgs e)
        {
            // Do not try to disable when no capture device is plugged in or enrollment mode is on
            if (!this.enrollmentModeActive && this.initialized)
            {
                lock (lockObj)
                {
                    if (!executionComplete)
                    {
                        PosApplication.Instance.Services.Peripherals.LogOnDevice.EndCapture();
                        executionComplete = true;
                        TaggedImage.Image = Properties.Resources.disabled;
                    }
                    else
                    {
                        executionComplete = false;
                        PosApplication.Instance.Services.Peripherals.LogOnDevice.BeginVerifyCapture();
                    }
                }
            }
        }

        public void EndImageRefresh()
        {
            lock (lockObj)
            {
                if (oldImageStream != null)
                {
                    oldImageStream.Close();
                    oldImageStream = null;
                }
                executionComplete = true;
            }
        }

    }
}
