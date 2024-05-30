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
    using System.Collections;
    using System.Text;
    using LSRetailPosis;

    /// <summary>
    /// The "8-bit Byte" encoding Mode
    /// </summary>
    internal class EightBitByteMode : Mode
    {
        #region fields

        private const int NumberOfBitsPerCharacter = 8;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new instance of the <c>EightBitByteMode</c>.
        /// </summary>
        /// <param name="encoding">The character set encoding</param>
        public EightBitByteMode(Encoding encoding)
            : base(new BitArray(new bool[] {false, true, false, false}), new int[Mode.NumberOfVersionRanges] { 8, 16, 16 }, encoding)
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Encodes an input string into shift jis character encoding.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="stream">The output <c>BitOutputStream</c> </param>
        public override void Encode(string input, BitOutputStream stream)
        {
            //var sjis = Encoding.GetEncoding("shift_jis", new EncoderExceptionFallback(), new DecoderExceptionFallback());

            try
            {
                var sjisInput = Encoding.GetBytes(input);

                foreach (var b in sjisInput)
                {
                    if (IsJisX0201(b))
                    {
                        stream.Write(b);
                    }
                    else
                    {
                        throw new QrcodeException(ApplicationLocalizer.Language.Translate(2802));
                    }
                }
            }
            catch (EncoderFallbackException e)
            {
                throw new QrcodeException(ApplicationLocalizer.Language.Translate(2802), e);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new QrcodeException(ApplicationLocalizer.Language.Translate(2803), e);
            }
        }

        #endregion

        #region protected methods

        protected override int GetNumberOfBitsRequiredToEncode(int numberOfChars)
        {
            return numberOfChars * NumberOfBitsPerCharacter;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Check if a byte is in the JIS X 0201 character set.
        /// Control characters are passed through (returns true).
        /// </summary>
        /// <param name="b">Byte to check.</param>
        /// <returns>True if in the charset, false otherwise.</returns>
        private static bool IsJisX0201(byte b)
        {
            return (b < 0x80 || (0xA0 < b && b < 0xE0)) ? true : false;
        }

        #endregion
    }
}
