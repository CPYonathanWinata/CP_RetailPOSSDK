/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System.Drawing;

    /// <summary>
    /// Responsbile for writing Function Patterns to a Symbol.
    /// </summary>
    internal class FunctionPatternWriter
    {
        #region fields

        public const int PositionDetectionPatternSideLength = 7;
        public const int AlignmentPatternSideLength = 5;
        public const int TimingPatternPosition = 6; // From the left and top of Symbol

        private readonly Symbol _symbol;

        #endregion

        #region constructors

        /// <summary>
        /// Create a Function Pattern writer for a Symbol.
        /// </summary>
        /// <param name="symbol">Symbol to write to.</param>
        public FunctionPatternWriter(Symbol symbol)
        {
            this._symbol = symbol;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Place all Function Patterns into a Symbol.
        /// </summary>
        public void Write()
        {
            PlaceFinderPattern();
            PlaceSeparators();
            PlaceTimingPattern();
            PlaceAlignmentPatterns();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Position Detection Patterns are placed at the top-left, top-right, and bottom left corners
        /// of the Symbol.
        /// 
        /// An image is show below:
        /// xxxxxxx
        /// x     x
        /// x xxx x
        /// x xxx x
        /// x xxx x
        /// x     x
        /// xxxxxxx
        /// </summary>
        private void PlaceFinderPattern()
        {
            var positionDetectionPattern = this._symbol.CreateTwoConcentricSquares(PositionDetectionPatternSideLength);

            //top left
            this._symbol.DrawImage(positionDetectionPattern, 0, 0);

            //top right 
            this._symbol.DrawImage(positionDetectionPattern, this._symbol.SideLength - positionDetectionPattern.Width, 0);

            //bottom left
            this._symbol.DrawImage(positionDetectionPattern, 0, this._symbol.SideLength - positionDetectionPattern.Height);
        }

        /// <summary>
        /// Each Alignment Pattern may be viewed as three superimposed
        /// concentric squares and is constructed of dark 5 x 5 modules,
        /// light 3 x 3 modules and a single central dark module. The number
        /// of Alignment Patterns depends on the symbol version and they shall
        /// be placed in all Model 2 symbols of Version 2 or larger.
        /// 
        /// An image is shown below:
        /// 
        /// xxxxx
        /// *   *
        /// * * *
        /// *   *
        /// *****
        /// </summary>
        private void PlaceAlignmentPatterns()
        {
            var alignmentPattern = this._symbol.CreateTwoConcentricSquares(AlignmentPatternSideLength);

            var centerOffset = alignmentPattern.Width / 2;

            foreach (int x in this._symbol.Version.AlignmentPatternCoordinates)
            {
                foreach (int y in this._symbol.Version.AlignmentPatternCoordinates)
                {
                    if (!IsOccupiedByPositionDetectionPattern(x, y, this._symbol.Version))
                    {
                        this._symbol.DrawImage(alignmentPattern, x - centerOffset, y - centerOffset);
                    }
                }
            }
        }

        /// <summary>
        /// A one-module wide Separator is placed between each Position Detection Pattern and Encoding
        /// Region, and is constructed of all light modules.
        /// </summary>
        private void PlaceSeparators()
        {
            var cornerLength = PositionDetectionPatternSideLength;

            // top left
            this._symbol.DrawLines(new Point[] {
                    new Point(0, cornerLength),
                    new Point(cornerLength, cornerLength),
                    new Point(cornerLength, 0)
                },
                false
            );

            // top right
            this._symbol.DrawLines(new Point[] {
                    new Point(this._symbol.SideLength - cornerLength - 1, 0),
                    new Point(this._symbol.SideLength - cornerLength - 1, cornerLength),
                    new Point(this._symbol.SideLength - 1, cornerLength)
                },
                false
            );

            // bottom left
            this._symbol.DrawLines(new Point[] {
                    new Point(0, this._symbol.SideLength - cornerLength - 1),
                    new Point(cornerLength, this._symbol.SideLength - cornerLength - 1),
                    new Point(cornerLength, this._symbol.SideLength - 1)
                },
                false
            );
        }

        /// <summary>
        /// Horizontal and vertical Timing Patterns respectively consist of a one module wide row of
        /// alternating dar and light modules, commencing and ending with a dark module.
        /// </summary>
        private void PlaceTimingPattern()
        {
            var runStart = PositionDetectionPatternSideLength + 1;
            var runEnd = this._symbol.SideLength - (PositionDetectionPatternSideLength + 1);

            // Horizontal Timing Pattern, dark
            for (var x = runStart; x < runEnd; x += 2)
            {
                this._symbol.SetModule(x, TimingPatternPosition, true);
            }

            // Horizontal Timing Pattern, light
            for (var x = runStart + 1; x < runEnd - 1; x += 2)
            {
                this._symbol.SetModule(x, TimingPatternPosition, false);
            }

            // Vertical Timing Pattern, dark
            for (var y = runStart; y < runEnd; y += 2)
            {
                this._symbol.SetModule(TimingPatternPosition, y, true);
            }

            // Vertical Timing Pattern, light
            for (var y = runStart + 1; y < runEnd - 1; y += 2)
            {
                this._symbol.SetModule(TimingPatternPosition, y, false);
            }
        }

        /// <summary>
        /// Check if the given coordinate is in one of the Position Detection
        /// Pattern areas of a Symbol of the given Version.
        /// </summary>
        /// <param name="x">X coordinate of the point to check.</param>
        /// <param name="y">Y coordinate of the point to check.</param>
        /// <param name="version">Version of the Symbol to check against.</param>
        /// <returns></returns>
        private static bool IsOccupiedByPositionDetectionPattern(int x, int y, Version version)
        {
            return
                // top-left corner
                (x < PositionDetectionPatternSideLength && y < PositionDetectionPatternSideLength)
                ||
                // top-right corner
                (version.NumberOfModulesPerSide - PositionDetectionPatternSideLength <= x && y < PositionDetectionPatternSideLength)
                ||
                // bottom-left corner
                (x < PositionDetectionPatternSideLength && version.NumberOfModulesPerSide - PositionDetectionPatternSideLength <= y);
        }

        #endregion
    }
}
