/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System.Collections;

    /// <summary>
    /// From the ISO specification:
    /// "The Version Information is an 18 bit sequence containing 6 data
    /// bits, with 12 error correction bits calculated using the (18, 6) BCH
    /// code.
    /// No Version Information will result in an all-zero data string since
    /// only Versions 7 to 40 symbols contain the Version Information. Masking
    /// is not therefore applied to the Version Information.
    /// The Version Information areas are the 6 x 3 module block above the
    /// Timing Pattern and immediately to the left of the top right Position
    /// Detection Pattern Separator, and the 3 x 6 module block to the left of
    /// the Timing Pattern and immediately above the lower left Position
    /// Detection Pattern separator."
    /// </summary>
    internal class VersionInformationWriter
    {
        #region constants

        /// <summary>
        /// Number of bits in Version Information.
        /// </summary>
        public const int NumberOfBits = 18;

        /// <summary>
        /// The length of the short side of Version Information area.
        /// </summary>
        private const int ShortSideLength = 3;

        /// <summary>
        /// The length of the long side of Version Information area.
        /// </summary>
        private const int LongSideLength = 6;

        /// <summary>
        /// Version from which Version Information is required.
        /// </summary>
        private const int MinimumVersion = 7;

        /// <summary>
        /// Length of the corner square to which the Version Information area is adjacent.
        /// </summary>
        private const int CornerLength = FunctionPatternWriter.PositionDetectionPatternSideLength + 1;

        #endregion

        #region fields

        private readonly ISymbol symbol;

        /// <summary>
        /// Pre-calculated BCH code for each Version.
        /// </summary>
        private static readonly int[,] IntBCH = {
            {0,0,0,1,1,1,1,1,0,0,1,0,0,1,0,1,0,0},  // Version 7
            {0,0,1,0,0,0,0,1,0,1,1,0,1,1,1,1,0,0},
            {0,0,1,0,0,1,1,0,1,0,1,0,0,1,1,0,0,1},
            {0,0,1,0,1,0,0,1,0,0,1,1,0,1,0,0,1,1},

            {0,0,1,0,1,1,1,0,1,1,1,1,1,1,0,1,1,0},  // Version 11
            {0,0,1,1,0,0,0,1,1,1,0,1,1,0,0,0,1,0},
            {0,0,1,1,0,1,1,0,0,0,0,1,0,0,0,1,1,1},
            {0,0,1,1,1,0,0,1,1,0,0,0,0,0,1,1,0,1},
            {0,0,1,1,1,1,1,0,0,1,0,0,1,0,1,0,0,0},
            {0,1,0,0,0,0,1,0,1,1,0,1,1,1,1,0,0,0},
            {0,1,0,0,0,1,0,1,0,0,0,1,0,1,1,1,0,1},
            {0,1,0,0,1,0,1,0,1,0,0,0,0,1,0,1,1,1},
            {0,1,0,0,1,1,0,1,0,1,0,0,1,1,0,0,1,0},
            {0,1,0,1,0,0,1,0,0,1,1,0,1,0,0,1,1,0},

            {0,1,0,1,0,1,0,1,1,0,1,0,0,0,0,0,1,1},  // Version 21
            {0,1,0,1,1,0,1,0,0,0,1,1,0,0,1,0,0,1},
            {0,1,0,1,1,1,0,1,1,1,1,1,1,0,1,1,0,0},
            {0,1,1,0,0,0,1,1,1,0,1,1,0,0,0,1,0,0},
            {0,1,1,0,0,1,0,0,0,1,1,1,1,0,0,0,0,1},
            {0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1},
            {0,1,1,0,1,1,0,0,0,0,1,0,0,0,1,1,1,0},
            {0,1,1,1,0,0,1,1,0,0,0,0,0,1,1,0,1,0},
            {0,1,1,1,0,1,0,0,1,1,0,0,1,1,1,1,1,1},
            {0,1,1,1,1,0,1,1,0,1,0,1,1,1,0,1,0,1},

            {0,1,1,1,1,1,0,0,1,0,0,1,0,1,0,0,0,0},  // Version 31
            {1,0,0,0,0,0,1,0,0,1,1,1,0,1,0,1,0,1},
            {1,0,0,0,0,1,0,1,1,0,1,1,1,1,0,0,0,0},
            {1,0,0,0,1,0,1,0,0,0,1,0,1,1,1,0,1,0},
            {1,0,0,0,1,1,0,1,1,1,1,0,0,1,1,1,1,1},
            {1,0,0,1,0,0,1,0,1,1,0,0,0,0,1,0,1,1},
            {1,0,0,1,0,1,0,1,0,0,0,0,1,0,1,1,1,0},
            {1,0,0,1,1,0,1,0,1,0,0,1,1,0,0,1,0,0},
            {1,0,0,1,1,1,0,1,0,1,0,1,0,0,0,0,0,1},
            {1,0,1,0,0,0,1,1,0,0,0,1,1,0,1,0,0,1}   // Version 40
        };

        /// <summary>
        /// Pre-calculate BCH code for each Version, in BitArrays.
        /// </summary>
        private static readonly BitArray[] BCH = CreateBCH(IntBCH);

        #endregion

        #region properties

        private bool NeedsVersionInformation
        {
            get { return MinimumVersion <= this.symbol.Version.Number; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Create a Version Information writer for a Symbol.
        /// </summary>
        /// <param name="symbol">Symbol to write to.</param>
        public VersionInformationWriter(ISymbol symbol)
        {
            this.symbol = symbol;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Writer Version Information into the Symbol.
        /// </summary>
        public void Write()
        {
            if (this.NeedsVersionInformation)
            {
                var versionInformation = GetVersionInformation(this.symbol.Version.Number);
                Write(versionInformation);
            }
        }

        /// <summary>
        /// Fill the Version Information areas with light modules.
        /// </summary>
        public void Fill()
        {
            if (NeedsVersionInformation)
            {
                // To the left of top-right Position Detection Pattern
                this.symbol.FillRectangle(
                    this.symbol.SideLength - CornerLength - ShortSideLength,
                    0,
                    ShortSideLength,
                    LongSideLength,
                    false);

                // Above bottom-left Position Detection Pattern
                this.symbol.FillRectangle(
                    0,
                    this.symbol.SideLength - CornerLength - ShortSideLength,
                    LongSideLength,
                    ShortSideLength,
                    false);
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Create a BitArray representation of BCH code by converting from an integer representation.
        /// </summary>
        /// <param name="intBCH">BCH code represented in integers.</param>
        /// <returns>BitArray[] representation of BCH code.</returns>
        private static BitArray[] CreateBCH(int[,] intBCH)
        {
            var versionInfo = new BitArray[Version.Max - (MinimumVersion - 1)];

            for (var i = 0; i < versionInfo.Length; i++)
            {
                var bits = new BitArray(NumberOfBits);

                for (var j = 0; j < bits.Length; j++)
                {
                    bits.Set(j, intBCH[i, j] == 0 ? false : true);
                }
                versionInfo[i] = bits;
            }
            return versionInfo;
        }

        private static BitArray GetVersionInformation(int version)
        {
            return BCH[version - MinimumVersion]; // Convert to 0-based index
        }

        private void Write(BitArray versionInformation)
        {
            var enumerator = versionInformation.GetEnumerator();

            WriteTopRight(enumerator);

            // Placed twice.  Here goes the second round.
            enumerator.Reset();

            WriteBottomLeft(enumerator);
        }

        private void WriteTopRight(IEnumerator bits)
        {
            var startIndex = this.symbol.SideLength - CornerLength - 1;

            for (var y = LongSideLength - 1; 0 <= y; y--)
            {
                for (var xRel = 0; xRel < ShortSideLength; xRel++)
                {
                    bits.MoveNext();
                    this.symbol.SetModule(startIndex - xRel, y, (bool)bits.Current);
                }
            }
        }

        private void WriteBottomLeft(IEnumerator bits)
        {
            var startIndex = this.symbol.SideLength - CornerLength - 1;

            for (var x = LongSideLength - 1; 0 <= x; x--)
            {
                for (var yRel = 0; yRel < ShortSideLength; yRel++)
                {
                    bits.MoveNext();
                    this.symbol.SetModule(x, startIndex - yRel, (bool)bits.Current);
                }
            }
        }

        #endregion
    }
}
