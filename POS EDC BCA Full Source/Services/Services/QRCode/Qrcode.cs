/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System.Text;

namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using Contracts;
    using Contracts.BusinessLogic;
    using Contracts.DataEntity;
    using Contracts.Services;
    using LSRetailPosis;

    /// <summary>
    /// Class implementing the interface IQrcode
    /// </summary>
    [Export(typeof(IQrcode))]        
    public class Qrcode : IQrcode
    {

        #region Member variables

        [Import]
        public IApplication Application { get; set; }

        private IUtility Utility
        {
            get { return this.Application.BusinessLogic.Utility; }
        }

        #endregion

        #region fields

        private Version version;
        private Mode mode;

        #endregion

        #region properties
    
        /// <summary>
        /// The QRCode error correction level.
        /// </summary>
        public ErrorCorrectionLevel ErrorCorrectionLevel { set; get; }

        /// <summary>
        /// The QRCode version. 1 to 40.
        /// </summary>
        public int Version
        {
            set { this.version = new Version(value); }
            get { return this.version.Number; }
        }

        /// <summary>
        /// A positive integer less than or equal to 10 that specifies the side
        /// length of each cell ("module") in pixels.
        /// </summary>
        public int CellSize { get; set; }

        /// <summary>
        /// Color to use for the dark color.
        /// </summary>
        public Color DarkColor { get; set; }

        /// <summary>
        /// Color to use for the light color.
        /// </summary>
        public Color LightColor { get; set; }

        private int NumberOfErrorCorrectionBlocks
        {
            get { return version.GetNumberOfErrorCorrectionBlocks(this.ErrorCorrectionLevel); }
        }

        #endregion


        #region public methods

        /// <summary>
        /// Encodes a string into a QRCode.
        /// Unless explicitly set in the property, the Version of the QRCode is
        /// automatically set to the minimum Version that can accomodate the
        /// input string length.
        /// The max string size is 2953 at ErrorCorrectionLevel Low and
        /// Version 40.
        /// Throws QrcodeException if the input string is too long or if any
        /// character is outside the JIS X 0201 character set.
        /// </summary>
        /// <param name="input">String to be encoded.</param>
        /// <param name="qrcodeInfo"></param>
        /// <returns>A bitmap representing the QRCode.</returns>
        public Image Encode(string input, IQrcodeInfo qrcodeInfo)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (qrcodeInfo == null)
            {
                throw new ArgumentNullException("qrcodeInfo");
            }

            this.ErrorCorrectionLevel = qrcodeInfo.ErrorCorrectionLevel;
            this.CellSize = qrcodeInfo.CellSize;
            this.DarkColor = Color.Black;
            this.LightColor = Color.White;

            var encoding = GetEncoding(qrcodeInfo.Encoding);
            this.mode = new EightBitByteMode(encoding);

            version = qrcodeInfo.Version == 0 ? this.mode.CreateSmallestVersion(input.Length, this.ErrorCorrectionLevel) : new Version(qrcodeInfo.Version);

            var dataCodewords = ConvertToDataCodewords(input);

            var finalSequence = ConvertToFinalCodewordSequence(dataCodewords);

            using (var symbol = new Symbol(version, this.ErrorCorrectionLevel, this.DarkColor, this.LightColor))
            {
                // Can't use BitArray(byte[]) because the bit-order in each byte is reversed.
                // Use BitInputStream to preserve big-endian bit-order.
                symbol.Load(new BitInputStream(finalSequence.ToArray()), new MaskPattern.P000());

                return symbol.GetScaledBitmap(this.CellSize);
            }
        }

        #endregion

        #region private methods

        private byte[] ConvertToDataCodewords(string input)
        {
            var encoder = CreateDataCodewordEncoder();

            return encoder.Encode(input);
        }

        /// <summary>
        /// Divide the given data codewords into blocks, and calculate the
        /// error correction codewords for each block.
        /// Divide the given data codewords into columns, and store the
        /// codewords in each column, followed by the error codewords,
        /// to form the final codeword sequence.
        /// </summary>
        /// <param name="dataCodewords">Data codewords from which to form the
        /// final codeword sequence.</param>
        /// <returns>The final codeword sequence.</returns>
        private List<byte> ConvertToFinalCodewordSequence(byte[] dataCodewords)
        {
            var dataBlocks = new BlockView(dataCodewords, this.NumberOfErrorCorrectionBlocks);
            var ecc = CreateErrorCorrectionCodewords();

            ecc.Fill(dataBlocks, new ReedSolomon().Generate);

            var dataColumns = new ColumnView(dataCodewords, this.NumberOfErrorCorrectionBlocks);
            var finalSequence = new List<byte>(version.TotalNumberOfCodewords);

            finalSequence.AddRange(dataColumns);
            finalSequence.AddRange(ecc);

            return finalSequence;
        }

        private DataCodewordEncoder CreateDataCodewordEncoder()
        {
            return new DataCodewordEncoder(
                new BitOutputStream(version.GetNumberOfDataCodewords(this.ErrorCorrectionLevel)),
                this.mode,
                version
            );
        }

        private ErrorCorrectionCodewords CreateErrorCorrectionCodewords()
        {
            return new ErrorCorrectionCodewords(
                this.NumberOfErrorCorrectionBlocks,
                version.GetNumberOfErrorCorrectionCodewordsPerBlock(this.ErrorCorrectionLevel)
            );
        }

        private static Encoding GetEncoding(QrcodeEncoding encoding)
        {
            switch (encoding)
            {
                case QrcodeEncoding.Iso88591:
                    return Encoding.GetEncoding("ISO-8859-1", new EncoderExceptionFallback(), new DecoderExceptionFallback());
                
                case QrcodeEncoding.ShiftJis:
                    return Encoding.GetEncoding("shift_jis", new EncoderExceptionFallback(), new DecoderExceptionFallback());
                
                case QrcodeEncoding.Utf8:
                    return Encoding.GetEncoding("UTF-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());

                default:
                    return null;
            }
        }

        #endregion
    }
}
