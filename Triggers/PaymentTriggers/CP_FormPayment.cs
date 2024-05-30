using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace PaymentTriggers
{
    public partial class CP_FormPayment : Form
    {
        decimal amounts;
        static bool _continue;
        static SerialPort _serialPort;  

        public CP_FormPayment(decimal _amount)
        {
            InitializeComponent();
            amounts = _amount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  MessageBox.Show( amounts.ToString() );
            CP_ECRBCA(amounts,"debit");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CP_ECRBCA_false(amounts);
        }

        public static string ConvertStringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

        private void CP_ECRBCA(decimal amount, string type)
        {
            //  int amount = (int)((RetailTransaction)posTransaction).NetAmountWithTax;
            //    MessageBox.Show("masuk ke CPECRBCA"); 

            SerialPort port = new SerialPort();
            port.PortName = "COM3";
            //     port.PortName = "COM7";
            port.Parity = Parity.None;
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            // port.Handshake = Handshake.RequestToSend;
            // port.ReceivedBytesThreshold = 8;
            if (port.IsOpen)
            {
                MessageBox.Show("port is open");
                port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
                port.Close();
                port.Dispose();
            }
            else
            {
                //       MessageBox.Show("port is not open");
            }
            //   string hexString = this.CPgetHexData(amount);

            byte[] bytesToSend = StringToByteArray(CPgetHexData(amount, type));

            int lrcValue = bytesToSend[1] ^ bytesToSend[2];

            for (int i = 3; i < bytesToSend.Length; i++)
            {
                lrcValue ^= bytesToSend[i];
            }

            //hexString = ConvertStringToHex(paddingValue);
            int tmp = lrcValue;
            string hexString = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            //Array.Clear(valueByte, 0, valueByte.Length);
            byte[] valueByte = StringToByteArray(hexString);

            for (int i = 0; i < valueByte.Length; i++)
            {
                Array.Resize(ref bytesToSend, bytesToSend.Length + 1);
                bytesToSend[bytesToSend.GetUpperBound(0)] = valueByte[i];
            }

            Console.WriteLine("Request Hex : ");
            Console.WriteLine(bytesToSend);

            try
            {
                port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
                port.Open();
                //         MessageBox.Show("open port");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            //         MessageBox.Show("before write port ");
            port.Write(bytesToSend, 0, bytesToSend.Length);
            //       MessageBox.Show("write port");
            //     port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);

            //   Console.ReadKey();

            //    cardInfo.AuthCode = "12345";
            //   cardInfo.CardNumber = "012345678";

            port.Close();
            port.Dispose();

            this.Close();
        }

        private void CP_ECRBCA_false(decimal amount)
        {
            //  int amount = (int)((RetailTransaction)posTransaction).NetAmountWithTax;
            //    MessageBox.Show("masuk ke CPECRBCA"); 

            SerialPort port = new SerialPort();
            port.PortName = "COM3";
            //     port.PortName = "COM7";
            port.Parity = Parity.None;
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            // port.Handshake = Handshake.RequestToSend;
            // port.ReceivedBytesThreshold = 8;
            if (port.IsOpen)
            {
                MessageBox.Show("port is open");
                port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
                port.Close();
                port.Dispose();
            }
            else
            {
                //       MessageBox.Show("port is not open");
            }
            //   string hexString = this.CPgetHexData(amount);

            byte[] bytesToSend = StringToByteArray(CPgetHexData_false(amount));

            int lrcValue = bytesToSend[1] ^ bytesToSend[2];

            for (int i = 3; i < bytesToSend.Length; i++)
            {
                lrcValue ^= bytesToSend[i];
            }

            //hexString = ConvertStringToHex(paddingValue);
            int tmp = lrcValue;
            string hexString = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            //Array.Clear(valueByte, 0, valueByte.Length);
            byte[] valueByte = StringToByteArray(hexString);

            for (int i = 0; i < valueByte.Length; i++)
            {
                Array.Resize(ref bytesToSend, bytesToSend.Length + 1);
                bytesToSend[bytesToSend.GetUpperBound(0)] = valueByte[i];
            }

            Console.WriteLine("Request Hex : ");
            Console.WriteLine(bytesToSend);

            try
            {
                port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
                port.Open();
                //         MessageBox.Show("open port");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            //         MessageBox.Show("before write port ");
            port.Write(bytesToSend, 0, bytesToSend.Length);
            //       MessageBox.Show("write port");
            //     port.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);

            //   Console.ReadKey();

            //    cardInfo.AuthCode = "12345";
            //   cardInfo.CardNumber = "012345678";

            port.Close();
            port.Dispose();

            this.Close();
        }


        public string CPgetHexData(decimal amount, string type)
        {
            string transType; // = ConvertStringToHex("01"); // transtype

            switch(type)
            {
                case "debit":
                    transType = ConvertStringToHex("01"); // transtype
                    break;
                case "kredit":
                    transType = ConvertStringToHex("01"); // transtype
                    break;

                default:
                     transType = ConvertStringToHex("00"); // transtype
                    break;
            }

            string stx = "02";
            string msgLength = "0150";
            string version = "01";
        //    string transType = ConvertStringToHex("01"); // transtype
            string transAmount = ConvertStringToHex(((amount + "") + "00").PadLeft(12, '0'));
            string otherAmount = ConvertStringToHex(("").PadLeft(12, '0'));
         //   string PAN = ConvertStringToHex(("").PadRight(19, ' ')); // testing
            string PAN = ConvertStringToHex(("1688700627201892").PadRight(19, ' ')); // testing
            //  string expiryDate = ConvertStringToHex(("").PadLeft(4, ' ')); // testing
            string expiryDate = ConvertStringToHex(("2510").PadLeft(4, ' ')); // testing YYMM
           
        //    string expiryDate = ConvertStringToHex("0000"); // testing YYMM
            string cancelReason = ConvertStringToHex("00");
            string invoiceNumber = ConvertStringToHex("000000");
            string authCode = ConvertStringToHex(("000000").PadLeft(6, ' '));
       //     string authCode = ConvertStringToHex(("").PadLeft(6, ' '));
            string instalmentFlag = "20";
            string redeemFlag = "20";
            string DccFlag = ConvertStringToHex("N");
            string instalmentPlan = ConvertStringToHex(("").PadLeft(3, ' '));
            string instalmentTenor = ConvertStringToHex(("").PadLeft(2, ' '));
            string GenericData = ConvertStringToHex(("").PadLeft(12, ' '));
            string ReffNumber = ConvertStringToHex(("").PadLeft(12, ' '));
            string Filler = ConvertStringToHex(("").PadLeft(54, ' '));
            string STX = "03";

            string bodyMessage = version + transType + transAmount + otherAmount + PAN + expiryDate + cancelReason + invoiceNumber + authCode +
              instalmentFlag + redeemFlag + DccFlag + instalmentPlan + instalmentTenor + GenericData + ReffNumber + Filler;


            //  string bodyMessage = "";


            //string LRC = ReturnLRC(bodyMessage).ToString();

            //     string sendData = stx + msgLength + version + transType + transAmount + otherAmount + PAN + expiryDate + cancelReason + invoiceNumber + authCode +
            //        instalmentFlag + redeemFlag + DccFlag + instalmentPlan + instalmentTenor + GenericData + ReffNumber + Filler + STX + LRC;
            string sendData = stx + msgLength + bodyMessage + STX;// + LRC;

            //      MessageBox.Show(transAmount);
            //      MessageBox.Show(sendData);

            Console.WriteLine(sendData + transType + transAmount + otherAmount);

            return sendData;
        }

        public string CPgetHexData_false(decimal amount)
        {
            string stx = "02";
            string msgLength = "0150";
            string version = "01";
            string transType = ConvertStringToHex("01"); // transtype
            string transAmount = ConvertStringToHex(((amount + "") + "00").PadLeft(12, '0'));
            string otherAmount = ConvertStringToHex(("").PadLeft(12, '0'));
            //   string PAN = ConvertStringToHex(("").PadRight(19, ' ')); // testing
        //    string PAN = ConvertStringToHex(("1688700627201892").PadRight(19, ' ')); // testing
            string PAN = ConvertStringToHex(("1688700627201234").PadRight(19, ' ')); // testing false no
            //  string expiryDate = ConvertStringToHex(("").PadLeft(4, ' ')); // testing
            string expiryDate = ConvertStringToHex(("1110").PadLeft(4, ' ')); // testing YYMM false

            //    string expiryDate = ConvertStringToHex("0000"); // testing YYMM
            string cancelReason = ConvertStringToHex("00");
            string invoiceNumber = ConvertStringToHex("000000");
            string authCode = ConvertStringToHex(("000000").PadLeft(6, ' '));
            //     string authCode = ConvertStringToHex(("").PadLeft(6, ' '));
            string instalmentFlag = "20";
            string redeemFlag = "20";
            string DccFlag = ConvertStringToHex("N");
            string instalmentPlan = ConvertStringToHex(("").PadLeft(3, ' '));
            string instalmentTenor = ConvertStringToHex(("").PadLeft(2, ' '));
            string GenericData = ConvertStringToHex(("").PadLeft(12, ' '));
            string ReffNumber = ConvertStringToHex(("").PadLeft(12, ' '));
            string Filler = ConvertStringToHex(("").PadLeft(54, ' '));
            string STX = "03";

            string bodyMessage = version + transType + transAmount + otherAmount + PAN + expiryDate + cancelReason + invoiceNumber + authCode +
              instalmentFlag + redeemFlag + DccFlag + instalmentPlan + instalmentTenor + GenericData + ReffNumber + Filler;


            //  string bodyMessage = "";


            //string LRC = ReturnLRC(bodyMessage).ToString();

            //     string sendData = stx + msgLength + version + transType + transAmount + otherAmount + PAN + expiryDate + cancelReason + invoiceNumber + authCode +
            //        instalmentFlag + redeemFlag + DccFlag + instalmentPlan + instalmentTenor + GenericData + ReffNumber + Filler + STX + LRC;
            string sendData = stx + msgLength + bodyMessage + STX;// + LRC;

            //      MessageBox.Show(transAmount);
            //      MessageBox.Show(sendData);

            Console.WriteLine(sendData + transType + transAmount + otherAmount);

            return sendData;
        }

        private void mySerialPort_DataReceived(
                   object sender,
                   SerialDataReceivedEventArgs e)
        {

            MessageBox.Show("Received data:");

            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //data received on serial port is asssigned to "indata" string
            //    Console.WriteLine("Received data:");
            MessageBox.Show("Received data:");
            Console.WriteLine(indata.Length);
            MessageBox.Show("" + indata.Length);

            //   int count = 0;
            //   string response = "";

            //   string hex = BitConverter.ToString(data);

            if (indata.Length > 1)
            {
                MessageBox.Show(""+indata.Length);
                //    while ((indata[149] != 0x30 && indata[150] != 0x30) && (indata[149] != 0x47 && indata[150] != 0x45))
                //   {
                //           mySerialPort.Write(bytestosend, 0, bytestosend.Length);
                //    MessageBox.Show("a");
                //  }
                //    port.Close();
                //   port.Dispose();
            }
            //       port.Close();
            //      port.Dispose();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            //    MessageBox.Show("string to byte ");
            return bytes;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            CP_ECRBCA_false(amounts);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
        private void mySerialPort_DataReceived(
                    object sender,
                    SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //data received on serial port is asssigned to "indata" string

            if (indata.Length > 1)
            {
                if (indata[149] != 0x30 && indata[150] != 0x30)
                {
                    if (countRetry < 3)
                    {
                        countRetry++;
                        mySerialPort.Write(bytestosend, 0, bytestosend.Length);
                    }
                    else
                    {
                        MessageBox.Show(topupName + " failed");

                        mySerialPort.Close();
                        bytestosend = new byte[4];
                        bytestosend[0] = 0x02;
                        bytestosend[1] = 0x42;
                        bytestosend[2] = 0x4E;
                        bytestosend[3] = 0x49;
                    }
                }
                else
                {
                    MessageBox.Show(topupName + " success");

                    mySerialPort.Close();
                    bytestosend = new byte[4];
                    bytestosend[0] = 0x02;
                    bytestosend[1] = 0x42;
                    bytestosend[2] = 0x4E;
                    bytestosend[3] = 0x49;
                }
            }

        }*/
        
    }
}
