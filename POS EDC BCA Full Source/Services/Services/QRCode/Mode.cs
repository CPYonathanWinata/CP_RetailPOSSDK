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
    using System.Collections;
    using Contracts.Services;
    using LSRetailPosis;

    /// <summary>
    /// Base class for all encoding Modes.
    /// </summary>
    internal abstract class Mode
    {
        #region constants

        private const int IndicatorSize = 4;
        public const int NumberOfVersionRanges = 3;

        #endregion

        #region fields

        public readonly BitArray Indicator; //a 4-bit mode indicator

        /// <summary>
        /// The end-index of the three Version ranges 1-9, 10-26, 27-40.
        /// </summary>
        private readonly int[] versionRangeEnds = new int[NumberOfVersionRanges] { 9, 26, 40 };

        /// <summary>
        /// Number of Character-Count Indicator Bits for the three Version ranges.
        /// </summary>
        private readonly int[] numCharCountIndicatorBits;

        protected Encoding Encoding { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Initialize the mode with given parameters.
        /// </summary>
        /// <param name="indicator">A byte containing the Mode Indicator in the first four bits.</param>
        /// <param name="numCharCountIndicatorBits">A size 3 array containing number of
        /// character-count indicator bits for Version ranges 1-9, 10-26, and 27-40.</param>
        /// <param name="encoding">The character set encoding</param>
        protected Mode(BitArray indicator, int[] numCharCountIndicatorBits, Encoding encoding)
        {
            if (indicator.Length != IndicatorSize)
            {
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(2806), "indicator");
            }
            if (numCharCountIndicatorBits.Length != NumberOfVersionRanges)
            {
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(2807), "numCharCountIndicatorBits");
            }
            if (encoding == null)
            {
                throw  new ArgumentNullException("encoding");
            }

            this.Indicator = indicator;
            this.numCharCountIndicatorBits = numCharCountIndicatorBits;
            this.Encoding = encoding;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns the character count indicator, which is a string of bits that represents
        /// the number of characters that will be encoded. The character count lenght depends
        /// on the Qrcode version.
        /// </summary>
        /// <param name="version">The Qrcode version</param>
        /// <returns>Returns the character count indicator</returns>
        public int GetNumberOfCharCountIndicatorBits(Version version)
        {
            int range;
            if (version.Number <= versionRangeEnds[0])
            {
                range = 0;
            }
            else if (version.Number < versionRangeEnds[1])
            {
                range = 1;
            }
            else
            {
                range = 2;
            }

            return numCharCountIndicatorBits[range];
        }

        /// <summary>
        /// Create the smallest Version that will accomodate an input of given
        /// length with the given Error Correction Level.
        /// </summary>
        /// <param name="inputLength">Length of input data to accomodate.</param>
        /// <param name="errorCorrectionLevel">Error Correction Level to be used.</param>
        /// <returns>Smallest Version that will be able to accomodate the data.</returns>
        public Version CreateSmallestVersion(int inputLength, ErrorCorrectionLevel errorCorrectionLevel)
        {
            const int bitsPerCodeword = 8;

            // This is only for the data.  Mode Indicator and Character Count Indicator
            // need to be accounted for.
            var bitsForData = GetNumberOfBitsRequiredToEncode(inputLength);

            // Check each Version by range
            var version = 1;

            for (var range = 0; range < versionRangeEnds.Length; range++)
            {
                // Total bits required for current Version range
                var bitsRequired = IndicatorSize + numCharCountIndicatorBits[range] + bitsForData;

                // Check each Version in current range
                for (; version <= versionRangeEnds[range]; version++)
                {
                    if (bitsRequired <= Version.GetNumberOfDataCodewords(version, errorCorrectionLevel) * bitsPerCodeword)
                    {
                        // Current Version is enough
                        return new Version(version);
                    }
                }
            }

            throw new QrcodeException(ApplicationLocalizer.Language.Translate(2808));
        }

        public abstract void Encode(string input, BitOutputStream stream);

        #endregion

        #region protected methods

        protected abstract int GetNumberOfBitsRequiredToEncode(int numberOfChars);

        #endregion
    }
}
