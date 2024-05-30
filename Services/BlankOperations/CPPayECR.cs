using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LSRetailPosis.POSProcesses;

//using System.Threading.Tasks;
using LSRetailPosis.DataAccess;
using System.Data.SqlClient;
using LSRetailPosis.Transaction;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using DE = Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using LSRetailPosis.Settings;

//using LSRetailPosis.POSProcesses;


using GME_Custom;
using GME_Custom.GME_Data;
using GME_Custom.GME_Propesties;
using GME_Custom.GME_EngageFALWSServices;
using System.Threading;
using Microsoft.Dynamics.Retail.Pos.Contracts;
//using System.Threading.Tasks;


namespace Microsoft.Dynamics.Retail.Pos.BlankOperations
{
    public partial class CPPayECR : Form
    {
        IPosTransaction posTransaction;
        private System.Data.DataTable transactions;

        static AutoResetEvent autoEventBCA = new AutoResetEvent(false);
        private bool _isReceived { get; set; }
        static string _LastResponBCA { get; set; }
        static bool isEDCSettlementBCA { get; set; }
        public IApplication Application { get; set; }

        public CPPayECR(IPosTransaction _posTransaction)
        {
            InitializeComponent();
            posTransaction = _posTransaction;     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GME_Var.transTypeBCA = "01";
            GME_Var.isEDCBCA = true;

            GME_Var.panBCA = "1688700627201892".PadRight(19, ' ');
            GME_Var.expiryBCA = "2510";

            BlankOperations.ShowMsgBoxInformation("PAN: " + GME_Var.panBCA);

            ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
            GME_Var.approvalCodeBCA = string.Empty;

            

            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

            int totalAmount = decimal.ToInt32(retailTransaction.TransSalePmtDiff);

            BlankOperations.ShowMsgBoxInformation("total: " + totalAmount);

            try
            {
                BCAOnline.openPort("COM3");
            }
            catch (Exception ex)
            {

            }

            if (GME_Var.serialBCA.IsOpen)
            {
                //autoEventBCA = new AutoResetEvent(false);
                BCAOnline.sendData(totalAmount.ToString(), "01", Connection.applicationLoc, posTransaction);
                _isReceived = false;
                ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
                autoEventBCA.WaitOne();

                if (GME_Var.transTypeBCA == "01")
                {
                    if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeBCA == "*0")
                    {
                        BlankOperations.ShowMsgBoxInformation("Pembayaran EDC BCA berhasil");

                    }
                    else if (GME_Var.respCodeBCA == "54")
                    {
                        BlankOperations.ShowMsgBox("Decline Expired Card");
                        GME_Var.isEDCBCA = false;

                    }
                    else if (GME_Var.respCodeBCA == "55")
                    {
                        BlankOperations.ShowMsgBox("Decline Incorrect PIN");
                        GME_Var.isEDCBCA = false;

                    }
                    else if (GME_Var.respCodeBCA == "P2")
                    {
                        BlankOperations.ShowMsgBox("Read Card Error");
                        GME_Var.isEDCBCA = false;

                    }
                    else if (GME_Var.respCodeBCA == "P3")
                    {
                        BlankOperations.ShowMsgBox("User press Cancel on EDC");
                        GME_Var.isEDCBCA = false;

                    }
                    else if (GME_Var.respCodeBCA == "Z3")
                    {
                        BlankOperations.ShowMsgBox("EMV Card Decline");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "CE")
                    {
                        BlankOperations.ShowMsgBox("Connection Error/Line Busy");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "TO")
                    {
                        BlankOperations.ShowMsgBox("Connection Timeout");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "PT")
                    {
                        BlankOperations.ShowMsgBox("EDC Problem / EDC perlu di settlement");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "PS")
                    {
                        BlankOperations.ShowMsgBox("Settlement Failed");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "aa")
                    {
                        BlankOperations.ShowMsgBox("Decline (aa represent two digit alphanumeric value from EDC)");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "S2")
                    {
                        BlankOperations.ShowMsgBox("TRANSAKSI GAGAL");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "S3")
                    {
                        BlankOperations.ShowMsgBox("TXN BLM DIPROSES MINTA SCAN QR");
                        GME_Var.isEDCBCA = false;


                    }
                    else if (GME_Var.respCodeBCA == "S4")
                    {
                        BlankOperations.ShowMsgBox("TXN EXPIRED ULANGI TRANSAKSI");
                        GME_Var.isEDCBCA = false;

                    }
                    else if (GME_Var.respCodeBCA == "TN")
                    {
                        BlankOperations.ShowMsgBox("Topup Tunai Not Ready");
                        GME_Var.isEDCBCA = false;

                    }
                }

                autoEventBCA.Reset();


                //ThreadPool.QueueUserWorkItem(new WaitCallback(DataReceived), autoEventBCA);
                //autoEventBCA.WaitOne();
                // ThreadPool.QueueUserWorkItem(_ => receiveThreadFunc());
                // ThreadPool.QueueUserWorkItem(_ => DataReceived());
           //     if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeBCA == "*0")
   //                 Application.RunOperation(PosisOperations.PayCard, string.Empty);
            }
            else
            {
                BlankOperations.ShowMsgBoxInformation("Koneksi ke EDC BCA terputus. Silahkan Hubungi Help Desk dan lakukan pembayaran dengan EDC offline");
            }

       // outtoendmethod: ;
            GME_Var.isPoint = false;
        }

        public void receiveThreadBCA(object stateInfo)
        {
            using (GME_Var.serialBCA)
            {
                // get start time
                DateTime start = DateTime.Now;
                // buffer for pushing received string data into
                StringBuilder indata = new StringBuilder();

                // loop until at most 10 seconds have passed 
                while (!_isReceived)
                {
                    if (GME_Var.serialBCA.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[GME_Var.serialBCA.BytesToRead];
                        int bytesRead = GME_Var.serialBCA.Read(buffer, 0, buffer.Length);
                        if (bytesRead <= 0) return;
                        //serialBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        buffer.Last();
                        indata.Clear();
                        foreach (byte b in buffer)
                        {
                            indata.Append(string.Format("{0:x2}", b));
                        }
                    }
                    else
                        System.Threading.Thread.Sleep(25);

                    if (indata.Length > 0)
                    {
                        if (indata.ToString() != _LastResponBCA)
                        {
                            string hex = indata.ToString();
                            if (hex.Length > 2 && hex != "06")
                            {
                                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                                BCAOnline.MappingText(hex);
                                GME_Var.serialBCA.Dispose();
                                _LastResponBCA = hex;
                                isEDCSettlementBCA = false;
                                ((AutoResetEvent)stateInfo).Set();
                                break;
                                // isReceived = true;
                            }
                        }
                        else if (GME_Var.transTypeBCA == "10")
                        {
                            string hex = indata.ToString();
                            if (hex.Length > 2 && hex != "06")
                            {
                                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                                BCAOnline.MappingText(hex);
                                GME_Var.serialBCA.Dispose();
                                _LastResponBCA = hex;
                                isEDCSettlementBCA = false;
                                ((AutoResetEvent)stateInfo).Set();
                                break;
                                // isReceived = true;
                            }
                        }
                    }
                }

                /*
                    GME_Var.transTypeBCA = "01";
                            GME_Var.isEDCBCA = true;
                            ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();
                            GME_Var.approvalCodeBCA = string.Empty;
                            RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                            int totalAmount = decimal.ToInt32(retailTransaction.TransSalePmtDiff);

                            BlankOperations.ShowMsgBoxInformation("total: "+totalAmount);

                            try
                            {
                                BCAOnline.openPort("COM3");
                            }
                            catch (Exception e)
                            {

                            }

                            if (GME_Var.serialBCA.IsOpen)
                            {
                                //autoEventBCA = new AutoResetEvent(false);
                                BCAOnline.sendData(totalAmount.ToString(), "01", Connection.applicationLoc, posTransaction);
                                _isReceived = false;
                                ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
                                autoEventBCA.WaitOne();

                                if (GME_Var.transTypeBCA == "01")
                                {
                                    if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeBCA == "*0")
                                    {
                                        BlankOperations.ShowMsgBoxInformation("Pembayaran EDC BCA berhasil");

                                    }
                                    else if (GME_Var.respCodeBCA == "54")
                                    {
                                        BlankOperations.ShowMsgBox("Decline Expired Card");
                                        GME_Var.isEDCBCA = false;

                                    }
                                    else if (GME_Var.respCodeBCA == "55")
                                    {
                                        BlankOperations.ShowMsgBox("Decline Incorrect PIN");
                                        GME_Var.isEDCBCA = false;

                                    }
                                    else if (GME_Var.respCodeBCA == "P2")
                                    {
                                        BlankOperations.ShowMsgBox("Read Card Error");
                                        GME_Var.isEDCBCA = false;

                                    }
                                    else if (GME_Var.respCodeBCA == "P3")
                                    {
                                        BlankOperations.ShowMsgBox("User press Cancel on EDC");
                                        GME_Var.isEDCBCA = false;

                                    }
                                    else if (GME_Var.respCodeBCA == "Z3")
                                    {
                                        BlankOperations.ShowMsgBox("EMV Card Decline");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "CE")
                                    {
                                        BlankOperations.ShowMsgBox("Connection Error/Line Busy");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "TO")
                                    {
                                        BlankOperations.ShowMsgBox("Connection Timeout");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "PT")
                                    {
                                        BlankOperations.ShowMsgBox("EDC Problem / EDC perlu di settlement");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "PS")
                                    {
                                        BlankOperations.ShowMsgBox("Settlement Failed");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "aa")
                                    {
                                        BlankOperations.ShowMsgBox("Decline (aa represent two digit alphanumeric value from EDC)");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "S2")
                                    {
                                        BlankOperations.ShowMsgBox("TRANSAKSI GAGAL");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "S3")
                                    {
                                        BlankOperations.ShowMsgBox("TXN BLM DIPROSES MINTA SCAN QR");
                                        GME_Var.isEDCBCA = false;


                                    }
                                    else if (GME_Var.respCodeBCA == "S4")
                                    {
                                        BlankOperations.ShowMsgBox("TXN EXPIRED ULANGI TRANSAKSI");
                                        GME_Var.isEDCBCA = false;

                                    }
                                    else if (GME_Var.respCodeBCA == "TN")
                                    {
                                        BlankOperations.ShowMsgBox("Topup Tunai Not Ready");
                                        GME_Var.isEDCBCA = false;

                                    }
                                }

                                autoEventBCA.Reset();


                                //ThreadPool.QueueUserWorkItem(new WaitCallback(DataReceived), autoEventBCA);
                                //autoEventBCA.WaitOne();
                                // ThreadPool.QueueUserWorkItem(_ => receiveThreadFunc());
                                // ThreadPool.QueueUserWorkItem(_ => DataReceived());
                                if (GME_Var.respCodeBCA == "00" || GME_Var.respCodeBCA == "*0")
                                    Application.RunOperation(PosisOperations.PayCard, string.Empty);
                            }
                            else
                            {
                                BlankOperations.ShowMsgBoxInformation("Koneksi ke EDC BCA terputus. Silahkan Hubungi Help Desk dan lakukan pembayaran dengan EDC offline");
                            }

                        outtoendmethod: ;
                            GME_Var.isPoint = false;
                        }
                        break;
                    }
                case "14":
                    {
                        if (isEDCSettlementBCA) { BlankOperations.ShowMsgBox("Settlement menunggu respon dari EDC BCA"); goto outthisfunc; }

                        if (BlankOperations.ShowMsgDialog("Apakah anda mau melakukan Settlement EDC BCA ?") == "OK")
                        {
                            isEDCSettlementBCA = true;
                            ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                            GME_Var.transTypeBCA = "10";
                            GME_Var.settlementBCAOnline = true;
                            try
                            {
                                BCAOnline.openPort("COM3");
                            }
                            catch (Exception e)
                            {

                            }

                            if (GME_Var.serialBCA.IsOpen)
                            {
                                //autoEventBCA = new AutoResetEvent(false);
                                BCAOnline.sendData("000000000000", "10", this.Application, posTransaction);

                                _isReceived = false;
                                ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
                                autoEventBCA.WaitOne();

                                if (GME_Var.transTypeBCA == "10")
                                {
                                    if (GME_Var.respCodeBCA == "00")
                                    {
                                        BlankOperations.ShowMsgBoxInformation("Settlement berhasil");

                                    }
                                    else if (GME_Var.respCodeBCA == "54")
                                    {
                                        BlankOperations.ShowMsgBox("Decline Expired Card");

                                    }
                                    else if (GME_Var.respCodeBCA == "55")
                                    {
                                        BlankOperations.ShowMsgBox("Decline Incorrect PIN");

                                    }
                                    else if (GME_Var.respCodeBCA == "P2")
                                    {
                                        BlankOperations.ShowMsgBox("Read Card Error");

                                    }
                                    else if (GME_Var.respCodeBCA == "P3")
                                    {
                                        BlankOperations.ShowMsgBox("User press Cancel on EDC");

                                    }
                                    else if (GME_Var.respCodeBCA == "Z3")
                                    {
                                        BlankOperations.ShowMsgBox("EMV Card Decline");

                                    }
                                    else if (GME_Var.respCodeBCA == "CE")
                                    {
                                        BlankOperations.ShowMsgBox("Connection Error/Line Busy");

                                    }
                                    else if (GME_Var.respCodeBCA == "TO")
                                    {
                                        BlankOperations.ShowMsgBox("Connection Timeout");

                                    }
                                    else if (GME_Var.respCodeBCA == "PT")
                                    {
                                        BlankOperations.ShowMsgBox("TIdak ada transaksi yang harus di settlement");

                                    }
                                    else if (GME_Var.respCodeBCA == "PS")
                                    {
                                        BlankOperations.ShowMsgBox("Settlement Failed");

                                    }
                                    else if (GME_Var.respCodeBCA == "aa")
                                    {
                                        BlankOperations.ShowMsgBox("Decline (aa represent two digit alphanumeric value from EDC)");

                                    }
                                    else if (GME_Var.respCodeBCA == "S2")
                                    {
                                        BlankOperations.ShowMsgBox("TRANSAKSI GAGAL");

                                    }
                                    else if (GME_Var.respCodeBCA == "S3")
                                    {
                                        BlankOperations.ShowMsgBox("TXN BLM DIPROSES MINTA SCAN QR");

                                    }
                                    else if (GME_Var.respCodeBCA == "S4")
                                    {
                                        BlankOperations.ShowMsgBox("TXN EXPIRED ULANGI TRANSAKSI");

                                    }
                                    else if (GME_Var.respCodeBCA == "TN")
                                    {
                                        BlankOperations.ShowMsgBox("Topup Tunai Not Ready");

                                    }
                                }

                                autoEventBCA.Reset();

                            }
                            else
                            {
                                ShowMsgBoxInformation("Koneksi port EDC BCA terputus, Silahkan Hubungi Help Desk " + System.Environment.NewLine +
                                        "Lakukan settlement di EDC BCA secara manual");
                                isEDCSettlementBCA = false;
                            }
                        }
                    outthisfunc: ;

                        break;
                    }
                case "15":
                    {//cek connection
                        ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                        BCAOnline.checkConnectionPortBCA("COM3");

                        break;
                    }
                        */

             

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BlankOperations.ShowMsgBox("Tes Koneksi BCA");
            ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

            BCAOnline.checkConnectionPortBCA("COM3");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (BlankOperations.ShowMsgDialog("Apakah anda mau melakukan Settlement EDC BCA ?") == "OK")
            {
                isEDCSettlementBCA = true;
                ElectronicDataCaptureBCA BCAOnline = new ElectronicDataCaptureBCA();

                GME_Var.transTypeBCA = "10";
                GME_Var.settlementBCAOnline = true;
                try
                {
                    BCAOnline.openPort("COM3");
                }
                catch (Exception ex)
                {

                }

                if (GME_Var.serialBCA.IsOpen)
                {
                    //autoEventBCA = new AutoResetEvent(false);
                    BCAOnline.sendData("000000000000", "10", this.Application, posTransaction);

                    _isReceived = false;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(receiveThreadBCA), autoEventBCA);
                    autoEventBCA.WaitOne();

                    if (GME_Var.transTypeBCA == "10")
                    {
                        if (GME_Var.respCodeBCA == "00")
                        {
                            BlankOperations.ShowMsgBoxInformation("Settlement berhasil");

                        }
                        else if (GME_Var.respCodeBCA == "54")
                        {
                            BlankOperations.ShowMsgBox("Decline Expired Card");

                        }
                        else if (GME_Var.respCodeBCA == "55")
                        {
                            BlankOperations.ShowMsgBox("Decline Incorrect PIN");

                        }
                        else if (GME_Var.respCodeBCA == "P2")
                        {
                            BlankOperations.ShowMsgBox("Read Card Error");

                        }
                        else if (GME_Var.respCodeBCA == "P3")
                        {
                            BlankOperations.ShowMsgBox("User press Cancel on EDC");

                        }
                        else if (GME_Var.respCodeBCA == "Z3")
                        {
                            BlankOperations.ShowMsgBox("EMV Card Decline");

                        }
                        else if (GME_Var.respCodeBCA == "CE")
                        {
                            BlankOperations.ShowMsgBox("Connection Error/Line Busy");

                        }
                        else if (GME_Var.respCodeBCA == "TO")
                        {
                            BlankOperations.ShowMsgBox("Connection Timeout");

                        }
                        else if (GME_Var.respCodeBCA == "PT")
                        {
                            BlankOperations.ShowMsgBox("TIdak ada transaksi yang harus di settlement");

                        }
                        else if (GME_Var.respCodeBCA == "PS")
                        {
                            BlankOperations.ShowMsgBox("Settlement Failed");

                        }
                        else if (GME_Var.respCodeBCA == "aa")
                        {
                            BlankOperations.ShowMsgBox("Decline (aa represent two digit alphanumeric value from EDC)");

                        }
                        else if (GME_Var.respCodeBCA == "S2")
                        {
                            BlankOperations.ShowMsgBox("TRANSAKSI GAGAL");

                        }
                        else if (GME_Var.respCodeBCA == "S3")
                        {
                            BlankOperations.ShowMsgBox("TXN BLM DIPROSES MINTA SCAN QR");

                        }
                        else if (GME_Var.respCodeBCA == "S4")
                        {
                            BlankOperations.ShowMsgBox("TXN EXPIRED ULANGI TRANSAKSI");

                        }
                        else if (GME_Var.respCodeBCA == "TN")
                        {
                            BlankOperations.ShowMsgBox("Topup Tunai Not Ready");

                        }
                    }

                    autoEventBCA.Reset();

                }
                else
                {
                    BlankOperations.ShowMsgBoxInformation("Koneksi port EDC BCA terputus, Silahkan Hubungi Help Desk " + System.Environment.NewLine +
                            "Lakukan settlement di EDC BCA secara manual");
                    isEDCSettlementBCA = false;
                }
            }
        outthisfunc: ;
        }
    }
}
