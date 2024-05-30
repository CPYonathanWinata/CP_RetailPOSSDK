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
    using Contracts.Services;

    /// <summary>
    /// The Format Information is a 15 bit sequence containing 5 data bits,
    /// with 10 error correction bits calculated using the (15, 5) BCH code.
    /// The first two data bits contain the Error Correction Level of the
    /// symbol. The third to fifth data bits of the Format Information contain
    /// the Mask Pattern Reference.
    /// </summary>
    internal class FormatInformationWriter
    {

        #region constants

        /// <summary>
        /// Number of bits in Format Information.
        /// </summary>
        public const int NumberOfBits = 15;

        /// <summary>
        /// Position of the Format Information in the Symbol.
        /// </summary>
        public const int Position = 8;

        public const int NumberOfErrorCorrectionLevels = 4;

        #endregion

        #region fields

        /// <summary>
        /// Mask to be applied on BCH-appended data, 101010000010010
        /// </summary>
        private static readonly BitArray Mask = new BitArray(new bool[NumberOfBits] {
            true, false, true, false, true, false, false, false, false, false, true, false, false, true, false
        });

        /// <summary>
        /// Pre-calculated (15, 5) BCH code for each Error Correction Level and Mask pattern,
        /// represented in integers.
        /// </summary>
        private static readonly int[, ,] IntBCH = new int[NumberOfErrorCorrectionLevels, MaskPattern.NumberOfPatterns, NumberOfBits] {
            { // Low: 01
                {0,1,0,0,0,1,1,1,1,0,1,0,1,1,0},
                {0,1,0,0,1,1,0,1,1,1,0,0,0,0,1},
                {0,1,0,1,0,0,1,1,0,1,1,1,0,0,0},
                {0,1,0,1,1,0,0,1,0,0,0,1,1,1,1},
                {0,1,1,0,0,1,0,0,0,1,1,1,1,0,1},
                {0,1,1,0,1,1,1,0,0,0,0,1,0,1,0},
                {0,1,1,1,0,0,0,0,1,0,1,0,0,1,1},
                {0,1,1,1,1,0,1,0,1,1,0,0,1,0,0}
            },
            { // Medium: 00
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,1,0,1,0,0,1,1,0,1,1,1},
                {0,0,0,1,0,1,0,0,1,1,0,1,1,1,0},
                {0,0,0,1,1,1,1,0,1,0,1,1,0,0,1},
                {0,0,1,0,0,0,1,1,1,1,0,1,0,1,1},
                {0,0,1,0,1,0,0,1,1,0,1,1,1,0,0},
                {0,0,1,1,0,1,1,1,0,0,0,0,1,0,1},
                {0,0,1,1,1,1,0,1,0,1,1,0,0,1,0}
            },
            { // Quartile: 11
                {1,1,0,0,0,0,1,0,1,0,0,1,1,0,1},
                {1,1,0,0,1,0,0,0,1,1,1,1,0,1,0},
                {1,1,0,1,0,1,1,0,0,1,0,0,0,1,1},
                {1,1,0,1,1,1,0,0,0,0,1,0,1,0,0},
                {1,1,1,0,0,0,0,1,0,1,0,0,1,1,0},
                {1,1,1,0,1,0,1,1,0,0,1,0,0,0,1},
                {1,1,1,1,0,1,0,1,1,0,0,1,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            },
            { // High: 10
                {1,0,0,0,0,1,0,1,0,0,1,1,0,1,1},
                {1,0,0,0,1,1,1,1,0,1,0,1,1,0,0},
                {1,0,0,1,0,0,0,1,1,1,1,0,1,0,1},
                {1,0,0,1,1,0,1,1,1,0,0,0,0,1,0},
                {1,0,1,0,0,1,1,0,1,1,1,0,0,0,0},
                {1,0,1,0,1,1,0,0,1,0,0,0,1,1,1},
                {1,0,1,1,0,0,1,0,0,0,1,1,1,1,0},
                {1,0,1,1,1,0,0,0,0,1,0,1,0,0,1}
            }
        };

        /// <summary>
        /// Pre-calculated (15, 5) BCH code for each Error Correction Level and Mask pattern,
        /// represented in a BitArray.
        /// </summary>
        private static readonly BitArray[,] BCH = CreateBCH(IntBCH);

        /// <summary>
        /// The Symbol to write the Format Information to.
        /// </summary>
        private readonly ISymbol symbol;

        #endregion

        #region constructors

        /// <summary>
        /// Create a Format Information writer for a Symbol.
        /// </summary>
        /// <param name="symbol">Symbol to write to.</param>
        public FormatInformationWriter(ISymbol symbol)
        {
            this.symbol = symbol;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Write Format Information for the given Mask Pattern Reference.
        /// </summary>
        /// <param name="pattern">Mask Pattern Reference for Format Infomation.</param>
        public void Write(MaskPattern.Reference pattern)
        {
            var formatInformation = GetFormatInformation(this.symbol.ErrorCorrectionLevel, pattern);
            Write(formatInformation);
        }

        /// <summary>
        /// Fill the Format Information area with light modules.
        /// </summary>
        public void Fill()
        {
            Write(new BitArray(NumberOfBits));
        }

        #endregion

        #region private methods

        /// <summary>
        /// Create a BitArray representation of BCH code by converting from an integer representation.
        /// </summary>
        /// <param name="intBCH">BCH code represented in integers.</param>
        /// <returns></returns>
        private static BitArray[,] CreateBCH(int[, ,] intBCH)
        {
            var formatInfo = new BitArray[NumberOfErrorCorrectionLevels, MaskPattern.Patterns.Length];

            for (var i = 0; i < NumberOfErrorCorrectionLevels; i++)
            {
                for (var j = 0; j < MaskPattern.Patterns.Length; j++)
                {
                    var info = new BitArray(NumberOfBits);

                    for (var k = 0; k <= intBCH.GetUpperBound(2); k++)
                    {
                        info.Set(k, intBCH[i, j, k] == 0 ? false : true);
                    }
                    formatInfo[i, j] = info;
                }
            }
            return formatInfo;
        }

        private static BitArray GetFormatInformation(ErrorCorrectionLevel errorCorrectionLevel, MaskPattern.Reference pattern)
        {
            // Make sure not to mutate BCH by calling Xor on it directly!
            return new BitArray(BCH[(int)errorCorrectionLevel, (int)pattern]).Xor(Mask);
        }

        /// <summary>
        /// Place Format Information on the Symbol.
        /// The information is placed twice in different locations for redundancy.
        /// </summary>
        /// <param name="formatInformation">Format Information to place.</param>
        private void Write(BitArray formatInformation)
        {
            var enumerator = formatInformation.GetEnumerator();

            WriteUpperLeft(enumerator);

            // Placed twice.  Here goes the second round.
            enumerator.Reset();

            WriteOther(enumerator);
        }

        private void WriteUpperLeft(IEnumerator enumerator)
        {
            for (var x = 0; x <= Position; x++) // Up to and including the corner
            {
                if (x != FunctionPatternWriter.TimingPatternPosition)
                {
                    enumerator.MoveNext();
                    this.symbol.SetModule(x, Position, (bool)enumerator.Current);
                }
            }
            for (var y = Position - 1; 0 <= y; y--) // From one above the corner
            {
                if (y != FunctionPatternWriter.TimingPatternPosition)
                {
                    enumerator.MoveNext();
                    this.symbol.SetModule(Position, y, (bool)enumerator.Current);
                }
            }
        }

        private void WriteOther(IEnumerator enumerator)
        {
            // Near bottom-left corner
            for (int i = 1; i < Position; i++)
            {
                enumerator.MoveNext();
                this.symbol.SetModule(Position, this.symbol.SideLength - i, (bool)enumerator.Current);
            }

            this.symbol.SetModule(Position, this.symbol.SideLength - Position, true); //always dark

            // Near top-right corner
            for (int x = this.symbol.SideLength - Position; x < this.symbol.SideLength; x++)
            {
                enumerator.MoveNext();
                this.symbol.SetModule(x, Position, (bool)enumerator.Current);
            }
        }

        #endregion

    }
}
