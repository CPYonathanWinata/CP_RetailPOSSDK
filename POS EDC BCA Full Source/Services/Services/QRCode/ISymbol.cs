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
    using System.Drawing;
    using Contracts.Services;

    /// <summary>
    /// Symbol interface.
    /// </summary>
    internal interface ISymbol : ICloneable
    {
        /// <summary>
        /// The Version of the Symbol
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// The Error Correction Level of the Symbol
        /// </summary>
        ErrorCorrectionLevel ErrorCorrectionLevel { get; }

        /// <summary>
        /// Get the side length of the Symbol in number of modules.
        /// </summary>
        int SideLength { get; }

        /// <summary>
        /// Check if the specified module of the Symbol is initial.
        /// </summary>
        /// <param name="x">X coordinate of the module to check.</param>
        /// <param name="y">Y coordiante of the module to check.</param>
        /// <returns>True if still initial, false otherwise.</returns>
        bool IsInitial(int x, int y);

        /// <summary>
        /// Set the specified module to dark or light.
        /// </summary>
        /// <param name="x">X coordinate of the module to set.</param>
        /// <param name="y">Y coordinate of the module to set.</param>
        /// <param name="dark">True to set to dark, false to set to light.</param>
        void SetModule(int x, int y, bool dark);

        /// <summary>
        /// Set modules to dark or light by drawing a line between points.
        /// </summary>
        /// <param name="points">Points indicating module coordinates.</param>
        /// <param name="dark">True to set to dark, false to set to light.</param>
        void DrawLines(Point[] points, bool dark);

        /// <summary>
        /// Sed modules in the specified rectangular area to dark or light.
        /// </summary>
        /// <param name="x">X coordinate of the upper-left point of the rectangle.</param>
        /// <param name="y">Y coordinate of the upper-left point of the rectangle.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="dark">True to set to dark, false to set to light.</param>
        void FillRectangle(int x, int y, int width, int height, bool dark);
    }
}
