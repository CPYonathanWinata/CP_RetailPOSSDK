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

    public class ElectronicDataCaptureBCA
    {
        public IApplication application { get; set; }
        IPosTransaction posTransaction = null;
        string recieved_data;
        char stx = (char)0x02;
        char etx = (char)0x03;
        char sho = (char)0x1;
        bool isShowMessage = false;
        static string serialBuffer = "";
        static string expectedEcho = null;
        static object expectedEchoLock = new object();
        static ManualResetEvent expectedEchoReceived = new ManualResetEvent(false);
        bool isReceiveMessage = false;

        queryData data = new queryData();

        public string checkConnectionPortBCA(string port)
        {
            string result;

            GME_Var.serialBCA.PortName = port;
            GME_Var.serialBCA.BaudRate = 19200;
            GME_Var.serialBCA.Handshake = System.IO.Ports.Handshake.None;
            GME_Var.serialBCA.Parity = Parity.None;
            GME_Var.serialBCA.DataBits = 8;
            GME_Var.serialBCA.StopBits = StopBits.One;
            GME_Var.serialBCA.ReadTimeout = 200;
            GME_Var.serialBCA.WriteTimeout = 50;
            try
            {
                GME_Var.serialBCA.Open();
                result = "Koneksi port EDC BCA tersambung";
            }
            catch (Exception ex)
            {
                result = "Koneksi port EDC BCA terputus, Silahkan Hubungi Help Desk " + System.Environment.NewLine + 
                            "Lakukan penginputan di EDC BCA secara manual";
            }

            return result;
        }

        public void openPort(string port)
        {

            GME_Var.serialBCA.PortName = port;
            GME_Var.serialBCA.BaudRate = 115200;
            GME_Var.serialBCA.Handshake = System.IO.Ports.Handshake.None;
            GME_Var.serialBCA.Parity = Parity.None;
            GME_Var.serialBCA.DataBits = 8;
            GME_Var.serialBCA.StopBits = StopBits.One;
            GME_Var.serialBCA.ReadTimeout = 200;
            GME_Var.serialBCA.WriteTimeout = 50;
            try
            {
                GME_Var.serialBCA.Open();
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
                GME_Var.serialBCA.Close();
                result = "success";
            }
            catch (Exception ex)
            {
                result = "failed " + ex;
            }

            return result;
        }

        #region sending data        
        public void sendData(string totalAmount, string transType, IApplication applicationParm, IPosTransaction posTransactionParm, string refNumber = "")
        {
            if (totalAmount.Length < 12)
            {
                //back padding
                String result = totalAmount.Insert(totalAmount.Length, "00");

                //count needed padding to front
                int length = 12 - result.Length;
                String frontPad = "";
                for (int j = 0; j < length; j++)
                {
                    frontPad += "0";
                }

                totalAmount = frontPad + result;
            }

            isShowMessage = false;

            this.application = applicationParm;
            this.posTransaction = posTransactionParm;
            SerialCmdSend(totalAmount, transType, refNumber);
        }

        private static String dataIso()
        {
            string x;

            string otheramount = "000000000000";
            string PAN = "                   ";
            string ExpiryDate = "    ";
            //string PAN = "5409120012345684".PadRight(19, ' ');
            //string ExpiryDate = "2510";
            string CancelReason = "00";
            string InvoiceNumber = "000000";
      //      string AuthIDResponse = "      ";
            string AuthIDResponse =  "000000"; //if Debit, the auth id is 000000

       //     string InstallmentFlag = " ";
        //    string RedeemFlag = " ";
            string InstallmentFlag = " ";// "20";
            string RedeemFlag = " "; //"20";
            string DCCFlag = "N";
            string InstallmentPlan = "000";//"   ";
            string InstallmentTenor = "00";//"  ";
            string GenericData = "            ";
            string Refnumber = "            ";
            string Filler = "                                                      ";

            x = otheramount + PAN +
               ExpiryDate + CancelReason + InvoiceNumber + AuthIDResponse +
               InstallmentFlag + RedeemFlag + DCCFlag + InstallmentPlan +
               InstallmentTenor + GenericData + Refnumber + Filler;

            return x;
        }

        private static String dataIsoQRIS()
        {
            string x;

            string otheramount = "000000000000";
            string PAN = "                   ";
            string ExpiryDate = "    ";
            //string PAN = "5409120012345684".PadRight(19, ' ');
            //string ExpiryDate = "2510";
            string CancelReason = "00";
            string InvoiceNumber = "000000";
            //      string AuthIDResponse = "      ";
            string AuthIDResponse = "      "; // "000000"; //if QR, authid is "      "

            //     string InstallmentFlag = " ";
            //    string RedeemFlag = " ";
            string InstallmentFlag = " ";// "20";
            string RedeemFlag = " "; //"20";
            string DCCFlag = "N";
            string InstallmentPlan = "000";//"   ";
            string InstallmentTenor = "00";//"  ";
            string GenericData = "            ";
            string Refnumber = "            ";
            string Filler = "                                                      ";

            x = otheramount + PAN +
               ExpiryDate + CancelReason + InvoiceNumber + AuthIDResponse +
               InstallmentFlag + RedeemFlag + DCCFlag + InstallmentPlan +
               InstallmentTenor + GenericData + Refnumber + Filler;

            return x;
        }

        private static String dataIsoInquiry(string refNumber)
        {
            string x;

            string otheramount = "000000000000";
            string PAN = "                   ";
            string ExpiryDate = "    ";
            //string PAN = "5409120012345684".PadRight(19, ' ');
            //string ExpiryDate = "2510";
            string CancelReason = "00";
            string InvoiceNumber = "000000";
            //      string AuthIDResponse = "      ";
            string AuthIDResponse = "      ";
            //     string InstallmentFlag = " ";
            //    string RedeemFlag = " ";
            string InstallmentFlag = " ";
            string RedeemFlag = " ";
            string DCCFlag = "N";
            string InstallmentPlan = "000";//"   ";
            string InstallmentTenor = "00";//"  ";

            string GenericData = "            ";
            string Refnumber = refNumber; //"            ";
            string Filler = "                                                      ";

            x = otheramount + PAN +
               ExpiryDate + CancelReason + InvoiceNumber + AuthIDResponse +
               InstallmentFlag + RedeemFlag + DCCFlag + InstallmentPlan +
               InstallmentTenor + GenericData + Refnumber + Filler;

            return x;
        }


        private static String dataIsoReprint(string invoiceNum)
        {
            string x;

            string otheramount = "000000000000";
            string PAN = "                   ";
            string ExpiryDate = "    ";
            //string PAN = "5409120012345684".PadRight(19, ' ');
            //string ExpiryDate = "2510";
            string CancelReason = "00";
            string InvoiceNumber = invoiceNum;
            //      string AuthIDResponse = "      ";
            string AuthIDResponse = "000000"; //if Debit, the auth id is 000000

            //     string InstallmentFlag = " ";
            //    string RedeemFlag = " ";
            string InstallmentFlag = " ";// "20";
            string RedeemFlag = " "; //"20";
            string DCCFlag = "N";
            string InstallmentPlan = "000";//"   ";
            string InstallmentTenor = "00";//"  ";
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

        private void SerialCmdSend(string data, string transType, string refNumber)
        {
            if (GME_Var.serialBCA.IsOpen)
            {
                try
                {
                    //header
                    //char Ver = (char)0x1;

                    char Ver = (char)0x3; //change to support QRIS by Yonathan 2 Dec 2022
                    isReceiveMessage = false;
                    string Transtype = transType;
                    string contains;
                    if (Transtype == "10")
                    {
                        //settlements set amount data to zero
                        data = "000000000000";
                        contains = sho.ToString() + "P" + Ver + Transtype + data + dataIso();
                    }
                    else if (transType == "32")
                    {
                        data = "000000000000";
                        contains = sho.ToString() + "P" + Ver + Transtype + data + dataIsoInquiry(refNumber); 
                    }
                    else if (transType == "31") //if payment QRIS
                    {
                        contains = sho.ToString() + "P" + Ver + Transtype + data + dataIsoQRIS();
                    }
                    else if (transType == "19")
                    {
                        data = "000000000000";
                        contains = sho.ToString() + "P" + Ver + Transtype + data + dataIsoReprint(refNumber);
                    }
                    else
                    {
                        contains = sho.ToString() + "P" + Ver + Transtype + data + dataIso();
                    }

                    char lrc = Calculatelrc(contains + etx.ToString());
                    string final = string.Format(stx.ToString() + contains + etx.ToString() + lrc.ToString());
                    // Send the binary data out the port
                    byte[] hexstring = Encoding.ASCII.GetBytes(final);

                    foreach (byte hexval in hexstring)
                    {
                        byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                        GME_Var.serialBCA.Write(_hexval, 0, 1);
                        //Thread.Sleep(1); //disable (not respond after write function) disable by Yonathan 22/12/2022 
                        //end test 123
                    }
                }
                catch (Exception ex)
                {
                    BlankOperations.ShowMsgBox("Failed to SEND" + data + "\n" + ex + "\n");

                }

            }

        }
        #endregion

        #region recieving data

        public void MappingText(String hex)
        {//hex sometimes 1515
            int countNum = 8;
            if (hex.Length == 404)
            {
                countNum = 5;
            }
            int[] arrLength = { 1, 2, 1, 2, 12, 12, 19, 4, 2, 12, 6, 8, 6, 15, 8, 1, 26, 16, 6, 6, 2, 1, 1, 1, 12, 1, 3, 8, 1, countNum, 1, 1 }; //8 to 6 
            string[] strKeyIndex = { "STX", "ML", "VER", "TYPE", "AMOUNT", "OTHER AMOUNT", "PAN", "Expiry", "RespCode", "RRN", "ApprovalCode", "date", "time", "merchantId", "terminalId", "offlineFlag", "cardholderName", "PANCashierCard", "invoiceNumber", "batchNumber", "issuerId", "installmentFlag", "dccFlag", "reedemFlag", "infoAmount", "dccDecimalPlace", "dccCurrencyName", "dccExRate", "couponFlag", "filler", "ETX", "LRC" };
            int index = 0;

            List<string> temp = new List<string>();
            foreach (int i in arrLength)
            {
                temp.Add(hex.Substring(index, (i * 2)));
                index = index + (i * 2);
            }

            string[] arrTemp = temp.ToArray();
            Dictionary<string, string> hash = new Dictionary<string, string>();
            for (int j = 0; j < strKeyIndex.Length; j++)
            {
                hash.Add(strKeyIndex[j], arrTemp[j]);
            }

            foreach (KeyValuePair<string, string> pair in hash)
            {
                if (pair.Key == "AMOUNT")
                    GME_Var.amountBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "OTHER AMOUNT")
                    GME_Var.otherAmountBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "PAN")
                    GME_Var.panBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "Expiry")
                    GME_Var.expiryBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "RespCode")
                    GME_Var.respCodeBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "ApprovalCode")
                    GME_Var.approvalCodeBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "date")
                    GME_Var.dateBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "time")
                    GME_Var.timeBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "merchantId")
                    GME_Var.merchantIdBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "terminalId")
                    GME_Var.terminalIdBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "invoiceNumber")
                    GME_Var.invoiceNumberBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "RRN") //add reff number response by Yonathan 2 Dec 2022
                    GME_Var.refNumberBCA = ConvertHextoAscii(pair.Value);
            }

            GME_Var.RespEDCBCA = hash;


        }

        public void MappingTextInquiryQR(String hex)
        {
           string ascii = hex; //"0202000333323030303030303030303130303030303030303030303030303933363030302a2a2a2a2a2a2a2a2a323134342a2a2a2a3030333037353135303031343730313533333138202020323032333033313631353333303930303038383530303030313539393944484d504e3030334e496b6120507574726920202020202020202020202020202020202020202020202020202020202020202030303032303730303031303630384e4e4e3030303030303030303030302020202020202020202020204e2020202020033e";
            
            byte[] bytes = new byte[ascii.Length / 2];
            for (int i = 0; i < ascii.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(ascii.Substring(i, 2), 16);
            }

            string inputString = System.Text.Encoding.ASCII.GetString(bytes);
            
            //had to check the digit after 73rd, due to the ingenico move bug approval code only 6 digit, not 9, and remove spaces to make it easy to map 
            string subs2 = inputString.Substring(73, 4);
            if (inputString.Substring(73, 2) == "20")
            {
                inputString = inputString.Insert(73, "   "); //adding 3 spaces to make it 9 digits

                int lengthStr = inputString.Length;
                string substr = inputString.Substring(lengthStr - 10, 8); 
                // if(input)
                if (substr == "        ") //remove 3 spaces at the end to make it 200 chars
                {
                    inputString = inputString.Remove(lengthStr - 5, 3);

                }
            }

            
            int[] arrLength = { 1, 2, 1, 2, 12, 12, 19, 4, 2, 12, 9, 8, 6, 15, 8, 1, 26, 16, 6, 6, 2, 1, 1, 1, 12, 1, 3, 8, 1, 5, 1, 1 };
            string[] strKeyIndex = { "STX", "ML", "VER", "TYPE", "AMOUNT", "OTHER AMOUNT", "PAN", "Expiry", "RespCode", "RRN", "ApprovalCode", "date", "time", "merchantId", "terminalId", "offlineFlag", "cardholderName", "PANCashierCard", "invoiceNumber", "batchNumber", "issuerId", "installmentFlag", "dccFlag", "reedemFlag", "infoAmount", "dccDecimalPlace", "dccCurrencyName", "dccExRate", "couponFlag", "filler", "ETX", "LRC" };

            string[] substrings = new string[arrLength.Length];

            List<string> temp = new List<string>();
            int startIndex = 0;
            for (int i = 0; i < arrLength.Length; i++)
            {
                int length = arrLength[i];
                substrings[i] = inputString.Substring(startIndex, length);
                temp.Add(inputString.Substring(startIndex, length));
                startIndex += length;
            }

            string[] arrTemp = temp.ToArray();
            Dictionary<string, string> hash = new Dictionary<string, string>();
            for (int j = 0; j < strKeyIndex.Length; j++)
            {
                hash.Add(strKeyIndex[j], arrTemp[j]);
            }
            foreach (KeyValuePair<string, string> pair in hash)
            {
                if (pair.Key == "AMOUNT")
                    GME_Var.amountBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "OTHER AMOUNT")
                    GME_Var.otherAmountBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "PAN")
                    GME_Var.panBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "Expiry")
                    GME_Var.expiryBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "RespCode")
                    GME_Var.respCodeBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "ApprovalCode")
                    GME_Var.approvalCodeBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "date")
                    GME_Var.dateBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "time")
                    GME_Var.timeBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "merchantId")
                    GME_Var.merchantIdBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "terminalId")
                    GME_Var.terminalIdBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "invoiceNumber")
                    GME_Var.invoiceNumberBCA = pair.Value; //ConvertHextoAscii(pair.Value);
                else if (pair.Key == "RRN") //add reff number response by Yonathan 2 Dec 2022
                    GME_Var.refNumberBCA = pair.Value; //ConvertHextoAscii(pair.Value);
            }

            GME_Var.RespEDCBCA = hash;

        }
        /*public void MappingTextInquiryQR(String hex)
        {//hex sometimes 1515
            //int countNum = 7;
            //if (hex.Length == 404)
            //{
                int countNum = 5;
            //}
            int[] arrLength = { 1, 2, 1, 2, 12, 12, 19, 4, 2, 12, 9, 8, 6, 15, 8, 1, 26, 16, 6, 6, 2, 1, 1, 1, 12, 1, 3, 8, 1, countNum, 1, 1 }; // 6 to 8
            string[] strKeyIndex = { "STX", "ML", "VER", "TYPE", "AMOUNT", "OTHER AMOUNT", "PAN", "Expiry", "RespCode", "RRN", "ApprovalCode", "date", "time", "merchantId", "terminalId", "offlineFlag", "cardholderName", "PANCashierCard", "invoiceNumber", "batchNumber", "issuerId", "installmentFlag", "dccFlag", "reedemFlag", "infoAmount", "dccDecimalPlace", "dccCurrencyName", "dccExRate", "couponFlag", "filler", "ETX", "LRC" };
            int index = 0;

            List<string> temp = new List<string>();
            foreach (int i in arrLength)
            {
                temp.Add(hex.Substring(index, (i * 2)));
                index = index + (i * 2);
            }

            string[] arrTemp = temp.ToArray();
            Dictionary<string, string> hash = new Dictionary<string, string>();
            for (int j = 0; j < strKeyIndex.Length; j++)
            {
                hash.Add(strKeyIndex[j], arrTemp[j]);
            }

            foreach (KeyValuePair<string, string> pair in hash)
            {
                if (pair.Key == "AMOUNT")
                    GME_Var.amountBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "OTHER AMOUNT")
                    GME_Var.otherAmountBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "PAN")
                    GME_Var.panBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "Expiry")
                    GME_Var.expiryBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "RespCode")
                    GME_Var.respCodeBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "ApprovalCode")
                    GME_Var.approvalCodeBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "date")
                    GME_Var.dateBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "time")
                    GME_Var.timeBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "merchantId")
                    GME_Var.merchantIdBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "terminalId")
                    GME_Var.terminalIdBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "invoiceNumber")
                    GME_Var.invoiceNumberBCA = ConvertHextoAscii(pair.Value);
                else if (pair.Key == "RRN") //add reff number response by Yonathan 2 Dec 2022
                    GME_Var.refNumberBCA = ConvertHextoAscii(pair.Value);
            }

            GME_Var.RespEDCBCA = hash;


        }
         * 
         */
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