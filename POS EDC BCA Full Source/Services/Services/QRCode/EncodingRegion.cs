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
    using System.Collections.Generic;

    /// <summary>
    /// In the encoding region of the QR Code symbol, symbol characters are
    /// positioned in two-module wide columns commencing at the lower right
    /// corner of the symbol and running alternately upwards and downwards
    /// from the right to the left.
    /// </summary>
    internal class EncodingRegion : IEnumerator<Module>
    {
        #region fields

        private readonly ISymbol symbol;
        private int x;
        private int y;
        private bool movingUp;
        private bool atRight;

        #endregion

        #region properties

        Object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public Module Current
        {
            get
            {
                return new Module(this.symbol, x, y);
            }
        }

        #endregion

        #region public mehtods

        /// <summary>
        /// Create and Encoding Region enumerator for the specified Symbol
        /// size.  All modules in the Encoding Region of the Symbol must
        /// be initial, and all other modules must not be initial.
        /// </summary>
        /// <param name="symbol">Size of the Symbol.</param>
        public EncodingRegion(ISymbol symbol)
        {
            this.symbol = symbol;
            this.Reset();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            while (MoveToNextModule())
            {
                if (this.symbol.IsInitial(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            this.x = this.symbol.SideLength - 2;   // Left in last column
            this.y = this.symbol.SideLength;       // Below last row
            movingUp = true;
            atRight = false;
        }

        #endregion

        #region private methods

        private bool MoveToNextModule()
        {
            if (atRight)
            {
                MoveLeft();
            }
            else
            {
                if (movingUp)
                {
                    if (y == 0)     // At the top edge of the Symbol
                    {
                        SwitchDirection();
                    }
                    else            // Still moving up
                    {
                        MoveRight();
                        y--;        // Move up
                    }
                }
                else                // Moving down
                {
                    if (this.symbol.SideLength - 1 <= y)    // At the bottom edge of Symbol
                    {
                        SwitchDirection();
                    }
                    else            // Still moving down
                    {
                        MoveRight();
                        y++;        // Move down
                    }
                }
            }
            if (x < 0)
            {
                x = -1; //make sure underflow doesn't occur with possible repeated calls to a finished iterator
                return false;
            }
            else
            {
                return true;
            }
        }

        private void MoveRight()
        {
            x++;
            atRight = true;
        }

        private void MoveLeft()
        {
            x--;
            atRight = false;
        }

        private void SwitchDirection()
        {
            movingUp = !movingUp; // Switch direction
            x--;                   // Move one module to the left

            // Special case at the Vertical Timing Pattern position.
            // Skip the entire x == 6 column and start at x == 5, at right.
            if (x == FunctionPatternWriter.TimingPatternPosition)
            {
                x--;
            }

            atRight = true;        // Now at the right side of the next column
        }

        #endregion
    }
}
