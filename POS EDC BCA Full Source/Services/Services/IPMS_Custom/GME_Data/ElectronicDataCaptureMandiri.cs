using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.BlankOperations;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using GME_Custom.GME_Propesties;

namespace GME_Custom.GME_Data
{
    public class ElectronicDataCaptureMandiri
    {
        queryData data = new queryData();
        IApplication application = null;
        string recieved_data;
        char stx = (char)0x02;
        char etx = (char)0x03;
        char sho = (char)0x1;
        bool isShowMessage = false;

        public string checkConnectionPortMandiri(string port)
        {
            string result;

            GME_Var.serialMandiri.PortName = port;
            GME_Var.serialMandiri.BaudRate = 19200;
            GME_Var.serialMandiri.Handshake = System.IO.Ports.Handshake.None;
            GME_Var.serialMandiri.Parity = Parity.None;
            GME_Var.serialMandiri.DataBits = 8;
            GME_Var.serialMandiri.StopBits = StopBits.One;
            GME_Var.serialMandiri.ReadTimeout = 200;

            try
            {
                GME_Var.serialMandiri.Open();
                result = "Koneksi port EDC Mandiri tersambung";
            }
            catch (Exception ex)
            {
                result = "Koneksi port EDC Mandiri terputus, Silahkan Hubungi Help Desk " + System.Environment.NewLine +
                            "Lakukan penginputan di EDC Mandiri secara manual";
            }

            return result;
        }

        public void openPort(string port)
        {

            GME_Var.serialMandiri.PortName = port;
            GME_Var.serialMandiri.BaudRate = 19200;
            GME_Var.serialMandiri.Handshake = System.IO.Ports.Handshake.None;
            GME_Var.serialMandiri.Parity = Parity.None;
            GME_Var.serialMandiri.DataBits = 8;
            GME_Var.serialMandiri.StopBits = StopBits.One;
            GME_Var.serialMandiri.ReadTimeout = 200;
            GME_Var.serialMandiri.WriteTimeout = 50;
            try
            {
                GME_Var.serialMandiri.Open();
            }
            catch (Exception ex)
            {

            }
        }

        public string closePort()
        {
            string result;

            try
            {
                GME_Var.serialMandiri.Close();
                result = "success";
            }
            catch (Exception ex)
            {
                result = "failed " + ex;
            }

            return result;
        }

        #region Sending data        
        public void Send_Data(string totalAmount, string transType, IApplication applicationParm)
        {
            if (totalAmount.Length < 12)
            {
                string result = totalAmount;
                int length = 12 - result.Length;
                String frontPad = "";
                for (int j = 0; j < length; j++)
                {

                    frontPad += "0";

                }

                totalAmount = frontPad + result;
            }

            isShowMessage = false;

            SerialCmdSend(totalAmount, transType);
        }

        private static String dataIso()
        {
            string x;

            string otheramount = "000000000000";
            string PAN = "                   ";
            string ExpiryDate = "    ";
            string CancelReason = "00";
            string InvoiceNumber = "000000";
            string AuthIDResponse = "      ";
            string InstallmentFlag = " ";
            string RedeemFlag = " ";
            string DCCFlag = "N";
            string InstallmentPlan = "   ";
            string InstallmentTenor = "  ";
            string GenericData = "            ";
            string Refnumber = "            ";
            string Filler = "                                                      ";

            x = otheramount + PAN +
               ExpiryDate + CancelReason + InvoiceNumber + AuthIDResponse +
               InstallmentFlag + RedeemFlag + DCCFlag + InstallmentPlan +
               InstallmentTenor + GenericData + Refnumber + Filler;

            return x;
        }

        private static Char Calculatelrc(string lrcstr)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(lrcstr);
            byte lrc = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                lrc ^= bytes[i];
            }

            return Convert.ToChar(lrc);
        }

        private void SerialCmdSend(string data, string transType)
        {
            if (GME_Propesties.GME_Var.serialMandiri.IsOpen)
            {
                try
                {
                    //header
                    char Ver = (char)0x1;

                    string otheramount = "000000000000";
                    //10 transaction sale
                    //30 settlement
                    string type = transType;

                    char lrc = Calculatelrc(Ver.ToString() + type + data + otheramount + etx.ToString());
                    // char lrc = (char)0x2;
                    string final = string.Format(stx.ToString() + Ver + type + data + otheramount + etx.ToString() + lrc.ToString());
                    // Send the binary data out the port
                    byte[] hexstring = Encoding.ASCII.GetBytes(final);
                    //There is a intermitant problem that I came across
                    //If I write more than one byte in succesion without a 
                    //delay the PIC i'm communicating with will Crash
                    //I expect this id due to PC timing issues ad they are
                    //not directley connected to the COM port the solution
                    //Is a ver small 1 millisecound delay between chracters
                    foreach (byte hexval in hexstring)
                    {
                        byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                        GME_Propesties.GME_Var.serialMandiri.Write(_hexval, 0, 1);
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    BlankOperations.ShowMsgBox("Failed to SEND" + data + "\n" + ex + "\n");
                }
            }
            else
            {
                BlankOperations.ShowMsgBoxInformation("Koneksi port ke EDC Mandiri terputus, Silahkan Hubungi Help Desk");
            }
        }
        #endregion

        #region Recieving Data

        public void MappingTextMandiri(String hex)
        {
            //front and bottom
            hex = ConvertHextoAscii(hex);
            int[] arrFrontLength = { 1, 1, 2, 2 };
            string[] arrName = { "stx", "ver", "type", "responseCode" };
            string[] strKeyIndex = { "TerminalId", "MerchantId", "CardType", "PAN", "EntryMode", "TransactionType", "BatchNo", "TraceNo", "date", "time", "ReffNo", "ApprovalCode", "ETXCRC" };

            string[] temp = hex.Split('|');
            Dictionary<string, string> hash = new Dictionary<string, string>();
            int index = 0;

            if (hex.Contains("SETTLEMENT") && hex.Contains("UNSUCCESSFUL"))
            {
                GME_Var.respCodeMandiri = "ER";
            }

            if (hex.Contains("ER") && hex.Contains("SALE"))
            {
                GME_Var.respCodeMandiri = "ER";
            }

            if (temp.Length >= arrName.Length + strKeyIndex.Length)
            {
                for (int i = 0; i < strKeyIndex.Length; i++)
                {
                    //loop pertama
                    if (i == 0)
                    {
                        //ada 4 stx, ver, type, response
                        for (int j = 0; j < arrFrontLength.Length; j++)
                        {
                            hash.Add(arrName[j], temp[i].Substring(index, (arrFrontLength[j])));
                            index = index + (arrFrontLength[j]);
                        }
                    }
                    else
                    {
                        hash.Add(strKeyIndex[i], temp[i]);
                    }
                }

                foreach (KeyValuePair<string, string> pair in hash)
                {
                    if (pair.Key == "responseCode")
                        GME_Var.respCodeMandiri = pair.Value;
                    else if (pair.Key == "TerminalId")
                        GME_Var.terminalIdMandiri = pair.Value;
                    else if (pair.Key == "MerchantId")
                        GME_Var.merchantIdMandiri = pair.Value;
                    else if (pair.Key == "CardType")
                        GME_Var.cardTypeMandiri = pair.Value;
                    else if (pair.Key == "PAN")
                        GME_Var.panMandiri = pair.Value;
                    else if (pair.Key == "TransactionType")
                        GME_Var.transTypeMandiri = pair.Value;
                    else if (pair.Key == "TraceNo")
                        GME_Var.invoiceNumberMandiri = pair.Value;
                    else if (pair.Key == "date")
                        GME_Var.dateMandiri = pair.Value;
                    else if (pair.Key == "time")
                        GME_Var.timeMandiri = pair.Value;
                    else if (pair.Key == "ApprovalCode")
                        GME_Var.approvalCodeMandiri = pair.Value;
                }

                GME_Var.RespEDCMandiri = hash;

            }
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static string ConvertHextoAscii(string HexString)
        {
            string asciiString = "";

            int i = 0;
            int maxTranslationIndex = (HexString.Length);

            while (i < maxTranslationIndex)
            {
                //Convert the 2 characters at the current index in the index in our input string……
                string hs = System.Convert.ToChar(System.Convert.ToUInt32(HexString.Substring(i, 2), 16)).ToString();
                asciiString = asciiString + hs;
                i += 2;
            }

            return asciiString;
        }

        private string HextoStr(string hex)
        {
            string result = "";
            byte[] bytes = Encoding.ASCII.GetBytes(hex);


            UnicodeEncoding encode = new UnicodeEncoding();

            result = encode.GetString(bytes);

            return result;
        }

        private string ASCIITOHex(string ascii)

        {

            StringBuilder sb = new StringBuilder();

            byte[] inputBytes = Encoding.UTF8.GetBytes(ascii);

            inputBytes.Last();

            foreach (byte b in inputBytes)

            {
                sb.Append(string.Format("{0:x2}", b));
            }

            return sb.ToString();

        }
        #endregion
    }

}
