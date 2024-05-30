/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////// This file is ported from SysTaskRecorderVideoRecording.xpo and changed to c# syntax ///////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Properties;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.DocGeneration
{
    /// <summary>
    /// Controller to record video
    /// </summary>
    public class VideoRecordingGenerator
    {
        #region private fields
        private Process recorderProcess;
        private ProcessStartInfo processStartInfo;
        private RecorderStatus recordingStatus;
        private const string BackgroundWorkerFile = @"TaskRecorderBackgroundWorker.exe";
        private const string RecordingWav = "recording.wmv";
        private const int VideoFrameWidth = 1024;
        private const int VideoFrameHeight = 768;
        private const short AxSessionNumber = 0;
        private const short FramesPerSecond = 4;
        string formattedRecordingPath;
        string originalRecordingPath;

        private string primaryMutex;
        private Mutex startStopSwitch;
        private Mutex pauseSignal;
        private Mutex resumeSignal;
        private const string PauseMutexSuffix = "_1";
        private const string ResumeMutexSuffix = "_2";
        #endregion

        #region public methods
        /// <summary>
        /// Starts the video recording.
        /// </summary>
        /// <param name="recordingPath">The recording path.</param>
        /// <returns>Status of whether it is successful in starting.</returns>
        public bool Start(string recordingPath)
        {
            if (!File.Exists(BackgroundWorkerFile))
            {
                throw new InvalidOperationException(ApplicationLocalizer.Language.Translate(58421)); // TaskRecorderBackgroundWorker.exe does not exist in POS folder.
            }
            originalRecordingPath = recordingPath;
            formattedRecordingPath = string.Format("\"{0}\"", recordingPath);

            // 1024 x 768 is the framesize for the video, evnen though it records fullscreen regardless of screen resolution
            RecorderController(BackgroundWorkerFile,
                AxSessionNumber,
                Guid.NewGuid().ToString(),
                formattedRecordingPath,
                FramesPerSecond,
                VideoFrameWidth,
                VideoFrameHeight,
                true,
                false,
                false,
                formattedRecordingPath,
                false);

            startStopSwitch = new Mutex(true, primaryMutex);
            pauseSignal = new Mutex(true, this.GetNameForPauseMutex());
            resumeSignal = new Mutex(false, this.GetNameForResumeMutex()); //important!

            try
            {
                recorderProcess.Start();
                Console.WriteLine(recorderProcess.Id);
                this.parmRecordingStatus(RecorderStatus.Recording);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Pauses video recording.
        /// </summary>
        public void Pause()
        {
            if (recordingStatus != RecorderStatus.Recording)
            {
                return;
            }

            pauseSignal.ReleaseMutex();
            resumeSignal.WaitOne();
            this.parmRecordingStatus(RecorderStatus.Paused);
        }

        /// <summary>
        /// Stops video recording.
        /// </summary>
        /// <returns>File path of the recording.</returns>
        public string Stop()
        {
            string recordedFilePath = null;
            if (recordingStatus != RecorderStatus.Recording &&
                recordingStatus != RecorderStatus.Paused)
            {
                return recordedFilePath;
            }

            startStopSwitch.ReleaseMutex();
            recorderProcess.WaitForExit();
            this.parmRecordingStatus(RecorderStatus.Stopped);

            this.Cleanup();
            startStopSwitch = null;
            pauseSignal = null;
            resumeSignal = null;

            if (string.IsNullOrWhiteSpace(this.formattedRecordingPath))
            {
                return recordedFilePath;
            }
            else
            {
                return Path.Combine(originalRecordingPath, RecordingWav);
            }
        }

        /// <summary>
        /// Resumes video recording.
        /// </summary>
        public void Resume()
        {
            if (recordingStatus != RecorderStatus.Paused)
            {
                return;
            }
            resumeSignal.ReleaseMutex();
            pauseSignal.WaitOne();
            this.parmRecordingStatus(RecorderStatus.Recording);
        }
        #endregion

        #region private methods
        private void Cleanup()
        {
            if (startStopSwitch != null)
            {
                startStopSwitch.Close();
            }
            if (pauseSignal != null)
            {
                pauseSignal.Close();
            }
            if (resumeSignal != null)
            {
                resumeSignal.Close();
            }
        }

        private RecorderStatus parmRecordingStatus(RecorderStatus status)
        {
            recordingStatus = status;

            return recordingStatus;
        }

        private void RecorderController(string exePath,
            int sessionNo,
            string primaryMutexName,
            string outputFolder,
            int fps,
            int width,
            int height,
            bool
            trackMouse,
            bool collectTrace,
            bool collectRawTrace,
            string subFolder,
            bool compressTrace)
        {
            string arguments = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}",
                sessionNo,
                primaryMutexName,
                outputFolder,
                fps,
                width,
                height,
                trackMouse,
                collectTrace,
                collectRawTrace,
                subFolder,
                compressTrace);
            recorderProcess = new System.Diagnostics.Process();

            processStartInfo = new ProcessStartInfo(exePath, arguments) { CreateNoWindow = true, UseShellExecute = false, };
            recorderProcess.StartInfo = processStartInfo;
            primaryMutex = primaryMutexName;
        }

        private string GetNameForPauseMutex()
        {
            return primaryMutex + PauseMutexSuffix;
        }

        private string GetNameForResumeMutex()
        {
            return primaryMutex + ResumeMutexSuffix;
        }
        #endregion
    }
}
