using LSRetailPosis.Settings;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPSyncPriceForm : Form
    {
        IApplication application;
        private string JobId = "";
        private const string AsyncDatabase = "AsyncClientJTJDRN1Store";
        private const bool DateRequestedIsUtc = false;   // set true if DATEREQUESTED is stored in UTC
        private static readonly TimeSpan Cooldown = TimeSpan.FromHours(1);

        private readonly Func<ReadOnlyCollection<object>> invokeSync;
        private readonly Button btnSync = new Button();
        private readonly Button btnCheck = new Button();
        private readonly Label lblStatus = new Label();
        private const int MatchWindowMinutes = 5;
        private static DateTime? lastSyncClick;   // static: survives closing/reopening the form
        private const string ProcessingText = "Sedang sinkronisasi";
        private readonly Timer dotTimer = new Timer();
        private int dotCount;

        private const int PollIntervalMs = 5000;
        private readonly Timer pollTimer = new Timer();
        private bool isChecking;   // prevents overlapping DB queries

        public CPSyncPriceForm(IApplication _application, Func<ReadOnlyCollection<object>> invokeSync)
        {
            this.invokeSync = invokeSync;
            application = _application;
       

            Text = "Sinkronisasi Harga";
            ClientSize = new Size(360, 170);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            btnSync.Text = "Sinkronisasi harga";
            btnSync.SetBounds(20, 20, 150, 45);
            btnSync.Click += OnSyncClick;

            btnCheck.Text = "Check status";
            btnCheck.SetBounds(190, 20, 150, 45);
            btnCheck.Click += OnCheckClick;

            lblStatus.SetBounds(20, 85, 320, 60);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Font = new Font(Font.FontFamily, 12f, FontStyle.Bold);

            Controls.AddRange(new Control[] { btnSync, btnCheck, lblStatus });
            InitStatusAnimation();
        }

        private void InitStatusAnimation()
        {
            dotTimer.Interval = 500;
            dotTimer.Tick += delegate
            {
                dotCount = (dotCount % 3) + 1;
                lblStatus.Text = ProcessingText + new string('.', dotCount).PadRight(3, '\u00A0');
            };

            pollTimer.Interval = PollIntervalMs;
            pollTimer.Tick += OnPollTick;

            FormClosed += delegate
            {
                dotTimer.Stop(); dotTimer.Dispose();
                pollTimer.Stop(); pollTimer.Dispose();
            };
        }

        private void SetProcessing()
        {
            lblStatus.ForeColor = Color.FromArgb(0, 90, 200);   // blue

            if (!dotTimer.Enabled)   // don't restart the dots on every poll
            {
                dotCount = 1;
                lblStatus.Text = ProcessingText + ".".PadRight(3, '\u00A0');
                dotTimer.Start();
            }

            pollTimer.Start();   // check status every 5 seconds
        }

        private void SetSuccess()
        {
            dotTimer.Stop();
            pollTimer.Stop();
            lblStatus.ForeColor = Color.FromArgb(0, 150, 0);    // green
            lblStatus.Text = "Sinkronisasi berhasil";
        }

        private void SetFailed()
        {
            dotTimer.Stop();
            pollTimer.Stop();
            lblStatus.ForeColor = Color.FromArgb(200, 0, 0);    // red
            lblStatus.Text = "Sinkronisasi gagal";
        }

        private void SetInfo(string text)
        {
            dotTimer.Stop();
            pollTimer.Stop();
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.Text = text;
        }


        private void OnSyncClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                DateTime now = DateRequestedIsUtc ? DateTime.UtcNow : DateTime.Now;

                // Cooldown reference = newest of: latest DB row, last click (row may not exist yet)
                int status;
                ReadOnlyCollection<object> containerArray = application.TransactionServices.InvokeExtension("getRetailParamJobId", ApplicationSettings.Database.DATAAREAID);
                 JobId = containerArray[3].ToString(); 
                
                DateTime? reference = GetSession(null, null, out status);
                if (lastSyncClick.HasValue && (!reference.HasValue || lastSyncClick.Value > reference.Value))
                    reference = lastSyncClick;

                if (reference.HasValue)
                {
                    TimeSpan remaining = Cooldown - (now - reference.Value);
                    if (remaining > TimeSpan.Zero)
                    {
                        ShowMessage(string.Format("Silakan menunggu {0} menit {1} detik untuk sinkronisasi lagi",
                            (int)remaining.TotalMinutes, remaining.Seconds), MessageBoxIcon.Information);
                        return;
                    }
                }

                ReadOnlyCollection<object> result = invokeSync();
                bool retValue = (bool)result[1];
                string errorMessage = result[2].ToString();

                if (retValue)
                {
                    lastSyncClick = now;   // used by Check status to find the matching session row
                    lblStatus.Text = "Sedang diproses";
                    ShowMessage("Sinkronisasi dimulai", MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = "Sinkronisasi gagal";
                    ShowMessage("Call failed. Error Message: " + errorMessage, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Manual check (button)
        private void OnCheckClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                RefreshStatus();
            }
            catch (Exception ex)
            {
                LSRetailPosis.ApplicationExceptionHandler.HandleException(this.ToString(), ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        // Automatic check (every 5 seconds while processing)
        private void OnPollTick(object sender, EventArgs e)
        {
            try
            {
                RefreshStatus();
            }
            catch (Exception)
            {
                // Stop polling on a DB error instead of showing an error dialog every 5 seconds
                SetInfo("Gagal memeriksa status");
            }
        }

        // Shared check logic: reads DOWNLOADSESSION and updates the label
        private void RefreshStatus()
        {
            if (isChecking)
                return;

            isChecking = true;
            try
            {
                int status;
                DateTime? row;

                if (lastSyncClick.HasValue)
                {
                    DateTime from = lastSyncClick.Value.AddMinutes(-MatchWindowMinutes);
                    DateTime to = lastSyncClick.Value.AddMinutes(MatchWindowMinutes);
                    row = GetSession(from, to, out status);

                    if (!row.HasValue)
                    {
                        SetProcessing();   // row not created yet
                        return;
                    }
                }
                else
                {
                    row = GetSession(null, null, out status);
                    if (!row.HasValue)
                    {
                        SetInfo("Belum ada data sinkronisasi");
                        return;
                    }
                }

                if (status == 4)
                    SetSuccess();
                else if (status == 8)
                    SetFailed();
                else
                    SetProcessing();
            }
            finally
            {
                isChecking = false;
            }
        }

 

        private DateTime? GetSession(DateTime? from, DateTime? to, out int status)
        {
            status = 0;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                LSRetailPosis.Settings.ApplicationSettings.Database.LocalConnection.ConnectionString);
            builder.InitialCatalog = AsyncDatabase;

            string sql = "SELECT TOP 1 DATEREQUESTED, STATUS FROM dbo.DOWNLOADSESSION WHERE JOBID = @JobId";
            if (from.HasValue && to.HasValue)
                sql += " AND DATEREQUESTED BETWEEN @From AND @To";
            sql += " ORDER BY DATEREQUESTED DESC";

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@JobId", JobId);
                if (from.HasValue && to.HasValue)
                {
                    cmd.Parameters.AddWithValue("@From", from.Value);
                    cmd.Parameters.AddWithValue("@To", to.Value);
                }
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    status = Convert.ToInt32(reader["STATUS"]);
                    return Convert.ToDateTime(reader["DATEREQUESTED"]);
                }
            }
        }

        private static void ShowMessage(string text, MessageBoxIcon icon)
        {
            using (LSRetailPosis.POSProcesses.frmMessage dialog =
                new LSRetailPosis.POSProcesses.frmMessage(text, MessageBoxButtons.OK, icon))
            {
                LSRetailPosis.POSProcesses.POSFormsManager.ShowPOSForm(dialog);
            }
        }
    }
}
