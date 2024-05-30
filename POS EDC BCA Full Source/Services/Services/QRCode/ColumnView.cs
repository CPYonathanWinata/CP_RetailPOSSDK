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
    using System.Collections.Generic;

    /// <summary>
    /// Columns represent a set of data codeword columns of a fixed size,
    /// except for the last column which may be less than the fixed size,
    /// in which case it is aligned at the bottom of the matrix.
    /// Columns are formed by conceptually laying out Blocks of data
    /// codewords as rows.
    /// </summary>
    internal class ColumnView : IEnumerable<byte>
    {
        #region fields

        public readonly byte[] DataCodewords;
        private readonly int columnSize;
        private readonly int numOfFullColumns;
        private readonly int lastColumnSize;
        private readonly int lastColumnRowOffset;
        private readonly int numOfColumns;

        #endregion

        #region  constructors

        /// <summary>
        /// Create columns of given size from data codewords.
        /// The last column may be less than the specified column size.
        /// </summary>
        /// <param name="dataCodewords">Data codewords from which to form columns.</param>
        /// <param name="columnSize">Size of each column.</param>
        public ColumnView(byte[] dataCodewords, int columnSize)
        {
            this.DataCodewords = dataCodewords;
            this.columnSize = columnSize;

            int remainder;
            this.numOfFullColumns = Math.DivRem(dataCodewords.Length, columnSize, out remainder);
            if (remainder == 0)
            {
                this.lastColumnSize = this.columnSize;
                this.numOfColumns = this.numOfFullColumns;
            }
            else
            {
                this.lastColumnSize = remainder;
                this.numOfColumns = this.numOfFullColumns + 1;
            }
            // The row at which the last column starts
            this.lastColumnRowOffset = this.columnSize - this.lastColumnSize;
        }

        #endregion

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region public methods

        /// <summary>
        /// Generate each codeword in each column starting from the first codeword
        /// of the first column to the last codeword of the first column, and ending
        /// with the first codeword of the last column to the last codeword of the
        /// last column.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<byte> GetEnumerator()
        {
            // Generate codewords in the full columns
            for (var columnIndex = 0; columnIndex < this.numOfFullColumns; columnIndex++)
            {
                for (var rowIndex = 0; rowIndex < this.columnSize; rowIndex++)
                {
                    yield return GetCodeword(rowIndex, columnIndex);
                }
            }
            // Last column with remaining codewords, if any
            if (this.numOfFullColumns < this.numOfColumns)
            {
                for (var rowIndex = this.lastColumnRowOffset; rowIndex < this.columnSize; rowIndex++)
                {
                    yield return GetCodeword(rowIndex, this.numOfFullColumns);
                }
            }
        }

        #endregion

        #region private methods

        private byte GetCodeword(int rowIndex, int columnIndex)
        {
            return this.DataCodewords[GetAbsoluteOffset(rowIndex, columnIndex)];
        }

        /// <summary>
        /// Get the absolute offset (in the data codeword array) of the codeword
        /// at the specified row and column.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns>Offset in the data codeword array.</returns>
        private int GetAbsoluteOffset(int rowIndex, int columnIndex)
        {
            // Sample layout of codewords of length 18 with column size 4:
            //
            //  0  1  2  3
            //  4  5  6  7
            //  8  9 10 11 12
            // 13 14 15 16 17
            //
            // Offset of the codeword at row 3 (0-based) column 3 is 16.
            // This can be calculated as the area (number of codewords) above row 3 (= 13)
            // plus the column index 3.
            return this.GetAreaAboveRow(rowIndex) + columnIndex;
        }

        /// <summary>
        /// Return the number of cells above the specified row in the matrix represented by these columns.
        /// Empty cells, if any, in the last column are excluded.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int GetAreaAboveRow(int row)
        {
            var area = row * this.numOfFullColumns; //area above row, excluding remainder column if any

            if ((this.numOfFullColumns < this.numOfColumns) && (this.lastColumnRowOffset < row))
            {
                area += row - this.lastColumnRowOffset;  //add cells above row in remainder column
            }
            return area;
        }

        #endregion
    }
}
