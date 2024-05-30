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
    /// Delegate for calculating Reed-Solomon code.
    /// (Made public due to problems with unit testing...)
    /// </summary>
    /// <param name="input">Data for which to calculate the code.</param>
    /// <param name="output">The calculated code.</param>
    delegate void ReedSolomonDelegate(ArraySegment<byte> input, byte[] output);

    /// <summary>
    /// Error Correction Codewords
    /// </summary>
    internal class ErrorCorrectionCodewords : IEnumerable<byte>
    {
        #region fields

        private readonly byte[][] _blocks;
        private readonly int _blockSize;

        #endregion

        #region constructors

        /// <summary>
        /// Create an empty container for storing Error Correction Codewords
        /// of the specified number of blocks and block size.
        /// </summary>
        /// <param name="numOfBlocks">Number of error correction codeword blocks to create.</param>
        /// <param name="blockSize">Size of each error correction block.</param>
        public ErrorCorrectionCodewords(int numOfBlocks, int blockSize)
        {
            _blocks = new byte[numOfBlocks][];
            for (int i = 0; i < numOfBlocks; i++)
            {
                _blocks[i] = new byte[blockSize];
            }

            this._blockSize = blockSize;
        }

        #endregion

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region public methods

        /// <summary>
        /// Fill this instance by calculating the error correction codewords
        /// for each data block using the delegate for Reed Solomon calculation.
        /// </summary>
        /// <param name="dataBlocks">Data blocks for which to calculate error
        /// correction blocks.</param>
        /// <param name="reedSolomon">Delegate for calculating the Reed-Solomon code.</param>
        public void Fill(BlockView dataBlocks, ReedSolomonDelegate reedSolomon)
        {
            int errorBlockIndex = 0;
            foreach (ArraySegment<byte> dataBlock in dataBlocks.ShortBlocks)
            {
                reedSolomon(dataBlock, _blocks[errorBlockIndex++]);
            }
            foreach (ArraySegment<byte> dataBlock in dataBlocks.LongBlocks)
            {
                reedSolomon(dataBlock, _blocks[errorBlockIndex++]);
            }
        }

        /// <summary>
        /// Enumerate codewords column-based, where each block is interpreted as a row.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<byte> GetEnumerator()
        {
            for (var col = 0; col < this._blockSize; col++)
            {
                foreach (var block in _blocks)
                {
                    yield return block[col];
                }
            }
        }

        #endregion
    }
}
