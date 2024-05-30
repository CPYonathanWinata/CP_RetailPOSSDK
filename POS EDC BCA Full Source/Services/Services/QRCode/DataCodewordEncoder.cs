/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System;

    /// <summary>
    /// Encoder for encoding the input string into data codewords.
    /// </summary>
    internal class DataCodewordEncoder
    {
        #region fields

        private readonly BitOutputStream stream;
        private readonly Mode mode;
        private readonly Version version;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new instance of the <c>DataCodewordEncoder</c>.
        /// </summary>
        /// <param name="stream">The bit output stream</param>
        /// <param name="mode">The QRCode mode</param>
        /// <param name="version">The QRCode version</param>
        public DataCodewordEncoder(BitOutputStream stream, Mode mode, Version version)
        {
            this.stream = stream;
            this.mode = mode;
            this.version = version;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Encode the input string based on mode and return data codewords.
        /// </summary>
        /// <param name="input">The input string to be encoded.</param>
        /// <returns>Data codewords for the given input.</returns>
        public byte[] Encode(string input)
        {
            this.stream.Write(this.mode.Indicator);

            WriteCharCountIndicator((uint)input.Length, this.mode.GetNumberOfCharCountIndicatorBits(this.version));

            this.mode.Encode(input, this.stream);

            this.AddTerminator();
            this.AddPadCodewords();

            return this.stream.Data;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Write the character-count indicator.
        /// </summary>
        /// <param name="charCount">The character count.</param>
        /// <param name="numBits">Number of bits to use to represent the character count.</param>
        private void WriteCharCountIndicator(uint charCount, int numBits)
        {
            charCount <<= 32 - numBits;
            stream.Write(charCount, numBits);
        }

        private void AddTerminator()
        {
            var remainingBits = stream.NumberOfRemainingBits;
            if (0 < remainingBits)
            {
                stream.Write(0x00, Math.Min(4, remainingBits)); //max 4 zeros
                stream.WriteBitsUntilByteBoundary();
            }
        }
        private void AddPadCodewords()
        {
            const byte padCodeword1 = 0xEC;
            const byte padCodeword2 = 0x11;

            var remainingBytes = stream.NumberOfRemainingBits / 8;
            for (var i = 0; i < remainingBytes; i++)
            {
                if (i % 2 == 0)
                {
                    stream.Write(padCodeword1);
                }
                else
                {
                    stream.Write(padCodeword2);
                }
            }
        }

        #endregion
    }
}
