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
    using System.Drawing;
    using Contracts.Services;

    /// <summary>
    /// A Symbol backed by a bitmap.
    /// </summary>
    internal class Symbol : ISymbol, IDisposable
    {
        #region fields

        private readonly Color darkColor;
        private readonly Pen darkPen;
        private readonly Brush darkBrush;

        private readonly Color lightColor;
        private readonly Pen lightPen;
        private readonly Brush lightBrush;

        // Color used to detect if a dark/light has been set or not
        private Color initialColor;

        private bool disposed;

        private Bitmap bitmap;
        private Graphics graphics;

        #endregion

        #region properties

        public Version Version { get; private set; }
        public ErrorCorrectionLevel ErrorCorrectionLevel { get; private set; }

        private Bitmap Bitmap
        {
            get
            {
                return bitmap;
            }
            set
            {
                if (this.bitmap != null)
                {
                    this.bitmap.Dispose();
                }
                this.bitmap = value;
            }
        }

        private Graphics Graphics
        {
            get
            {
                return this.graphics;
            }
            set
            {
                if (this.graphics != null)
                {
                    this.graphics.Dispose();
                }
                this.graphics = value;
            }
        }

        #endregion

        #region constuctors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Design")]
        public Symbol(Version version, ErrorCorrectionLevel errorCorrectionLevel, Color darkColor, Color lightColor)
            : this(
                version, errorCorrectionLevel, darkColor, lightColor,
                GetInitialColor(darkColor, lightColor),
                new Bitmap(version.NumberOfModulesPerSide, version.NumberOfModulesPerSide)
            )
        {
            this.Graphics.Clear(initialColor);
        }

        /// <summary>
        /// Create a deep copy of a Symbol.
        /// The underlying bitmap is deep-copied.
        /// </summary>
        /// <param name="symbol">Symbol to copy.</param>
        private Symbol(Symbol symbol)
            : this(
                symbol.Version, symbol.ErrorCorrectionLevel, symbol.darkColor, symbol.lightColor,
                symbol.initialColor, (Bitmap)symbol.Bitmap.Clone()
            )
        {
        }

        private Symbol(Version version, ErrorCorrectionLevel errorCorrectionLevel, Color darkColor, Color lightColor,
            Color initialColor, Bitmap bitmap)
        {
            this.Version = version;
            this.ErrorCorrectionLevel = errorCorrectionLevel;

            this.Bitmap = bitmap;
            this.Graphics = Graphics.FromImage(this.Bitmap);

            this.darkColor = darkColor;
            this.darkPen = new Pen(darkColor);
            this.darkBrush = new SolidBrush(darkColor);

            this.lightColor = lightColor;
            this.lightPen = new Pen(lightColor);
            this.lightBrush = new SolidBrush(lightColor);

            this.initialColor = initialColor;
        }

        #endregion

        #region public methods

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public int SideLength
        {
            get { return this.Version.NumberOfModulesPerSide; }
        }

        /// <summary>
        /// Create a deep copy of a Symbol.
        /// The underlying bitmap is deep-copied.
        /// </summary>
        public Object Clone()
        {
            return new Symbol(this);
        }

        public void SetModule(int x, int y, bool dark)
        {
            this.Bitmap.SetPixel(x, y, dark ? this.darkColor : this.lightColor);
        }

        public void DrawLines(Point[] points, bool dark)
        {
            this.Graphics.DrawLines(dark ? this.darkPen : this.lightPen, points);
        }

        public void FillRectangle(int x, int y, int width, int height, bool dark)
        {
            this.Graphics.FillRectangle(dark ? this.darkBrush : this.lightBrush,
                x, y, width, height);
        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void DrawImage(Image image, int x, int y)
        {
            this.Graphics.DrawImage(image, x, y);
        }

        /// <summary>
        /// Create a bitmap of two concentric squares.
        /// The outermost square is with the dark color, and the next outermost square
        /// is with the light color.  The remaining inner square is filled with the dark color.
        /// </summary>
        /// <param name="sideLength">Side length of the bitmap in number of pixels.</param>
        /// <returns>Bitmap of two concentric squares.</returns>
        public Bitmap CreateTwoConcentricSquares(int sideLength)
        {
            var bitmap = new Bitmap(sideLength, sideLength);

            try
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    // First, fill the complete rectangle with dark color
                    graphics.FillRectangle(this.darkBrush, 0, 0, bitmap.Width, bitmap.Height);

                    // Then draw a rectangle with 1px width with the light color
                    graphics.DrawRectangle(this.lightPen, 1, 1, bitmap.Width - 3, bitmap.Height - 3);
                }
            }
            catch (Exception)
            {
                bitmap.Dispose();
            }

            return bitmap;
        }

        /// <summary>
        /// Load a codeword bit stream.
        /// </summary>
        /// <param name="bitstream">Bit stream to load.</param>
        /// <param name="maskPattern">Pattern evaluation is not implemented yet, so specify pattern here.</param>
        public void Load(IEnumerable bitstream, MaskPattern maskPattern)
        {
            new FunctionPatternWriter(this).Write();

            /// Block out the Format Information and Version Information areas
            /// so that they are not interpreted as the Encoding Region.
            var versionInformationWriter = new VersionInformationWriter(this);
            var formatInformationWriter = new FormatInformationWriter(this);

            versionInformationWriter.Fill();
            formatInformationWriter.Fill();

            // Pattern evaluation is not yet implemented.
            // Take only one pattern, apply masking, and use the result.
            new MaskPatternApplier(this, bitstream).Apply(maskPattern.Condition);

            // Write Format and Version Information
            formatInformationWriter.Write(maskPattern.Ref);
            versionInformationWriter.Write();
        }

        public void Load(BitArray codewords)
        {
            Load(codewords, new MaskPattern.P000());
        }


        /// <summary>
        /// The bitmap in this Symbol contains a pixel for each module.
        /// Get a bitmap of this Symbol scaled with the cell size, so that each module
        /// is represented by a square of cell size side length.
        /// The Quiet Zone, which is a 4-module-wide margin of light color surrounding the QR Code,
        /// is added in the returned bitmap.
        /// </summary>
        /// <param name="cellSize">Side length of each cell in pixels.</param>
        /// <returns>Scaled Symbol with Quiet Zone.</returns>
        public Bitmap GetScaledBitmap(int cellSize)
        {
            const int quietZoneWidth = 4;

            var scaledSideLength = this.SideLength * cellSize;
            var scaledSideLengthWithQuietZone = scaledSideLength + (quietZoneWidth * cellSize * 2);

            var scaledBitmap = new Bitmap(scaledSideLengthWithQuietZone, scaledSideLengthWithQuietZone);

            try
            {
                using (var graphics = Graphics.FromImage(scaledBitmap))
                {
                    using (var darkBrush = new SolidBrush(this.darkColor))
                    {
                        // Fill with light color so only dark cells need to be drawn
                        graphics.Clear(this.lightColor);

                        // Convert each dark pixel to a square of the specified cell size
                        for (var y = 0; y < this.Bitmap.Height; y++)
                        {
                            for (var x = 0; x < this.Bitmap.Width; x++)
                            {
                                // If the pixel in the bitmap is dark, draw a square of cell size
                                // which represents a module in the scaled bitmap.
                                if (this.Bitmap.GetPixel(x, y).ToArgb() == this.darkColor.ToArgb())
                                {
                                    // Draw a square of the specified cell size
                                    graphics.FillRectangle(darkBrush,
                                        (quietZoneWidth + x) * cellSize,    // x of top-left corner in the scaled coordinate
                                        (quietZoneWidth + y) * cellSize,    // y of top-left corner in the scaled coordinate
                                        cellSize, cellSize);                // the square
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                scaledBitmap.Dispose();
            }

            return scaledBitmap;
        }

        public virtual bool IsInitial(int x, int y)
        {
            return this.Bitmap.GetPixel(x, y).ToArgb() == initialColor.ToArgb();
        }


        #endregion

        #region protected methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.Graphics.Dispose();
                    this.Bitmap.Dispose();
                    this.darkBrush.Dispose();
                    this.darkPen.Dispose();
                    this.lightBrush.Dispose();
                    this.lightPen.Dispose();
                }

                disposed = true;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Initialize the bitmap by setting an appropriate initial color.
        /// This initial color will represent the "not set" state of a module.
        /// </summary>
        private static Color GetInitialColor(Color dark, Color light)
        {
            if (CanBeUsedAsInitialColor(Color.Gray, dark, light))
            {
                return Color.Gray;
            }
            else if (CanBeUsedAsInitialColor(Color.LightGray, dark, light))
            {
                return Color.LightGray;
            }
            else
            {
                return Color.DimGray;
            }
        }

        /// <summary>
        /// A color can be used as an initial color if it is neither the dark nor the light color.
        /// </summary>
        /// <param name="color">Color to check.</param>
        /// <param name="dark">The color to be used for the dark</param>
        /// <param name="light">The color to be used for the light</param>
        /// <returns>True if it can be used as an initial color, false otherwise.</returns>
        private static bool CanBeUsedAsInitialColor(Color color, Color dark, Color light)
        {
            return color.ToArgb() != dark.ToArgb() && color.ToArgb() != light.ToArgb();
        }

        #endregion
    }
}
