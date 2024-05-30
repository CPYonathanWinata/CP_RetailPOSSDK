/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using Contracts.Services;
    using LSRetailPosis;

    /// <summary>
    /// QRCode Version, from 1 - 40.
    /// Version determines the size of the QRCode, where larger Versions
    /// have more modules (cells).
    /// </summary>
    internal class Version
    {
        #region constants

        public const int Max = 40;
        private const int NumberOfModulesPerSideInVersion1 = 21;

        /// <summary>
        /// The number of modules per side increases by 4 with each
        /// higher Version.
        /// </summary>
        private const int DifferenceInNumberOfModulesPerSideBetweenVersions = 4;

        #endregion

        #region fields

        // Total number of codewords (data & error) in each Version
        private static readonly int[] NumCodewords = new int[Max] {
              26,   44,   70,  100,  134,  172,  196,  242,  292,  346,
             404,  466,  532,  581,  655,  733,  815,  901,  991, 1085,
            1156, 1258, 1364, 1474, 1588, 1706, 1828, 1921, 2051, 2185,
            2323, 2465, 2611, 2761, 2876, 3034, 3196, 3362, 3532, 3706
        };

        // Number of error correction codewords in each version and error correction level
        private static readonly int[,] NumErrorCorrectionCodewords = new int[Max, 4] {
            {7,10,13,17},{10,16,22,28},{15,26,36,44},{20,36,52,64},
            {26,48,72,88},{36,64,96,112},{40,72,108,130},{48,88,132,156},
            {60,110,160,192},{72,130,192,224},{80,150,224,264},{96,176,260,308},
            {104,198,288,352},{120,216,320,384},{132,240,360,432},{144,280,408,480},
            {168,308,448,532},{180,338,504,588},{196,364,546,650},{224,416,600,700},
            {224,442,644,750},{252,476,690,816},{270,504,750,900},{300,560,810,960},
            {312,588,870,1050},{336,644,952,1110},{360,700,1020,1200},{390,728,1050,1260},
            {420,784,1140,1350},{450,812,1200,1440},{480,868,1290,1530},{510,924,1350,1620},
            {540,980,1440,1710},{570,1036,1530,1800},{570,1064,1590,1890},{600,1120,1680,1980},
            {630,1204,1770,2100},{660,1260,1860,2220},{720,1316,1950,2310},{750,1372,2040,2430}
        };

        // Number of error correction blocks in each version and error correction level
        private static readonly int[,] NumErrorCorrectionBlocks = new int[Max, 4] {
            {1,1,1,1},{1,1,1,1},{1,1,2,2},{1,2,2,4},
            {1,2,4,4},{2,4,4,4},{2,4,6,5},{2,4,6,6},
            {2,5,8,8},{4,5,8,8},{4,5,8,11},{4,8,10,11},
            {4,9,12,16},{4, 9, 16, 16},{6, 10, 12, 18},{6, 10, 17, 16},
            {6, 11, 16, 19},{6, 13, 18, 21},{7, 14, 21, 25},{8, 16, 20, 25},
            {8, 17, 23, 25},{9, 17, 23, 34},{9, 18, 25 ,30},{10, 20, 27, 32},
            {12, 21, 29, 35},{12, 23, 34 ,37},{12, 25, 34, 40},{13, 26, 35, 42},
            {14, 28, 38, 45},{15, 29, 40, 48},{16, 31, 43, 51},{17, 33, 45, 54},
            {18, 35, 48, 57},{19, 37, 51, 60},{19, 38, 53, 63},{20, 40, 56, 66},
            {21, 43, 59, 70},{22, 45, 62, 74},{24, 47, 65, 77},{25, 49, 68, 81}
        };

        /// <summary>
        /// For each version, the row or column coordinates of the center
        /// module of each Alignment Pattern.
        /// </summary>
        private static readonly int[][] AlignmentPatternCoord = new int[Max][] {
            new int[] {},
            new int[] {6, 18},
            new int[] {6, 22},
            new int[] {6, 26},
            new int[] {6, 30},
            new int[] {6, 34},
            new int[] {6, 22, 38},
            new int[] {6, 24, 42},
            new int[] {6, 26, 46},
            new int[] {6, 28, 50},
            new int[] {6, 30, 54},
            new int[] {6, 32, 58},
            new int[] {6, 34, 62},
            new int[] {6, 26, 46, 66},
            new int[] {6, 26, 48, 70},
            new int[] {6, 26, 50, 74},
            new int[] {6, 30, 54, 78},
            new int[] {6, 30, 56, 82},
            new int[] {6, 30, 58, 86},
            new int[] {6, 34, 62, 90},
            new int[] {6, 28, 50, 72, 94},
            new int[] {6, 26, 50, 74, 98},
            new int[] {6, 30, 54, 78, 102},
            new int[] {6, 28, 54, 80, 106},
            new int[] {6, 32, 58, 84, 110},
            new int[] {6, 30, 58, 86, 114},
            new int[] {6, 34, 62, 90, 118},
            new int[] {6, 26, 50, 74, 98, 122},
            new int[] {6, 30, 54, 78, 102, 126},
            new int[] {6, 26, 52, 78, 104, 130},
            new int[] {6, 30, 56, 82, 108, 134},
            new int[] {6, 34, 60, 86, 112, 138},
            new int[] {6, 30, 58, 86, 114, 142},
            new int[] {6, 34, 62, 90, 118, 146},
            new int[] {6, 30, 54, 78, 102, 126, 150},
            new int[] {6, 24, 50, 76, 102, 128, 154},
            new int[] {6, 28, 54, 80, 106, 132, 158},
            new int[] {6, 32, 58, 84, 110, 136, 162},
            new int[] {6, 26, 54, 82, 110, 138, 166},
            new int[] {6, 30, 58, 86, 114, 142, 170},
        };

        /// <summary>
        /// 0-based version index for accessing the arrays
        /// </summary>
        private readonly int _index;

        #endregion

        #region properties

        /// <summary>
        /// Version number with range 1 - 40.
        /// </summary>
        public int Number
        {
            get { return this._index + 1; }
        }

        #endregion

        #region constructors

        public Version(int version)
        {
            ThrowIfInvalid(version);
            this._index = version - 1;
        }

        #endregion

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="errorCorrectionLevel"></param>
        /// <returns></returns>
        public static int GetNumberOfDataCodewords(int version, ErrorCorrectionLevel errorCorrectionLevel)
        {
            ThrowIfInvalid(version);
            return NumCodewords[version - 1] - NumErrorCorrectionCodewords[version - 1, (int)errorCorrectionLevel];
        }

        /// <summary>
        /// The number of modules (cells) per side in the QR Code symbol.
        /// </summary>
        public int NumberOfModulesPerSide
        {
            get { return NumberOfModulesPerSideInVersion1 + (this._index * DifferenceInNumberOfModulesPerSideBetweenVersions); }
        }

        /// <summary>
        /// Gets the total number of codewords.
        /// </summary>
        public int TotalNumberOfCodewords
        {
            get { return NumCodewords[this._index]; }
        }

        /// <summary>
        /// Gets the aligment pattern coordinates.
        /// </summary>
        public int[] AlignmentPatternCoordinates
        {
            get { return AlignmentPatternCoord[this._index]; }
        }

        /// <summary>
        /// Gets the number of data codewords.
        /// </summary>
        /// <param name="level">The error correction level</param>
        /// <returns>The number of data codewords</returns>
        public int GetNumberOfDataCodewords(ErrorCorrectionLevel level)
        {
            return this.TotalNumberOfCodewords - NumErrorCorrectionCodewords[this._index, (int)level];
        }

        /// <summary>
        /// Gets the number of error correction codewords by 
        /// error correction code block.
        /// </summary>
        /// <param name="level">The Error correction level</param>
        /// <returns>The number of the error correction codewords</returns>
        public int GetNumberOfErrorCorrectionCodewordsPerBlock(ErrorCorrectionLevel level)
        {
            return NumErrorCorrectionCodewords[this._index, (int)level] / GetNumberOfErrorCorrectionBlocks(level);
        }

        /// <summary>
        /// Gets the number of error correction blocks.
        /// </summary>
        /// <param name="level">The error correction level</param>
        /// <returns>The number of error correction blocks</returns>
        public int GetNumberOfErrorCorrectionBlocks(ErrorCorrectionLevel level)
        {
            return NumErrorCorrectionBlocks[this._index, (int)level];
        }

        #endregion

        #region private methods

        private static void ThrowIfInvalid(int version)
        {
            if (version < 1 || Max < version)
            {
                throw new QrcodeException(ApplicationLocalizer.Language.Translate(2810));
            }
        }

        #endregion
    }
}
