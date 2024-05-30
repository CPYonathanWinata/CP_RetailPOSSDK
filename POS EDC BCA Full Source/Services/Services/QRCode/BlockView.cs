/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


/// NOTE: All classes made public to work around VS problem with generics & private accessors

namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System;
    using System.Collections.Generic;
    using LSRetailPosis;

    /// <summary>
    /// Blocks represent a set of data codeword blocks, for each of which
    /// error correction codewords need to be calculated.
    /// </summary>
    internal sealed class BlockView
    {
        #region properties

        internal EnumerableBlocks ShortBlocks { get; private set; }
        internal EnumerableBlocks LongBlocks { get; private set; }

        #endregion

        #region constructors

        /// <summary>
        /// Create blocks from data codewords and specified number of blocks.
        /// One or more blocks of size N (short blocks) are followed by 0 or more blocks
        /// of size N + 1 (long blocks).
        /// The size of the blocks and the number of long blocks are determined from
        /// the length of the data codeword sequence and the number of blocks.
        /// The remaining codewords R after dividing the sequence by the number of blocks
        /// are distributed to the last R blocks, forming R long blocks.
        /// </summary>
        /// <param name="dataCodewords">Data codewords from which to form blocks.</param>
        /// <param name="numOfBlocks">Number of blocks to create.</param>
        public BlockView(byte[] dataCodewords, int numOfBlocks)
        {
            int numOfLongBlocks;
            var shortBlockSize = Math.DivRem(dataCodewords.Length, numOfBlocks, out numOfLongBlocks);
            if (shortBlockSize < 1)
            {
                throw new ArgumentException(ApplicationLocalizer.Language.Translate(2801), "numOfBlocks");
            }
            var numOfShortBlocks = numOfBlocks - numOfLongBlocks;

            var startOfLongBlocks = numOfShortBlocks * shortBlockSize;

            this.ShortBlocks = new EnumerableBlocks(dataCodewords, 0, numOfShortBlocks, shortBlockSize);
            this.LongBlocks = new EnumerableBlocks(dataCodewords, startOfLongBlocks, numOfLongBlocks, shortBlockSize + 1);
        }

        #endregion

    }

    /// <summary>
    /// An enumerable set of fixed-size codeword blocks.
    /// Each block is represented by a segment of a byte array.
    /// </summary>
    internal sealed class EnumerableBlocks : IEnumerable<ArraySegment<byte>>
    {
        #region fields

        private readonly byte[] dataCodewords;
        private readonly int offset;
        private readonly int privNumberOfBlocks;
        private readonly int privBlockSize;

        #endregion

        #region properties

        // Need a name different from property name to keeping readonly-ness...
        public int NumberOfBlocks { get { return privNumberOfBlocks; } }
        public int BlockSize { get { return privBlockSize; } }

        #endregion

        #region constructors

        /// <summary>
        /// Create blocks from data codewords starting at specified offset,
        /// with specified number of blocks and block size.
        /// </summary>
        /// <param name="dataCodewords">Codewords from which to form blocks.</param>
        /// <param name="offset">Location in codewords from which to start creating blocks.</param>
        /// <param name="numOfBlocks">Number of blocks to create.</param>
        /// <param name="blockSize">Size of each block.</param>
        public EnumerableBlocks(byte[] dataCodewords, int offset, int numOfBlocks, int blockSize)
        {
            this.dataCodewords = dataCodewords;
            this.offset = offset;
            this.privNumberOfBlocks = numOfBlocks;
            this.privBlockSize = blockSize;
        }

        #endregion

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region public  methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<ArraySegment<byte>> GetEnumerator()
        {
            for (int i = 0; i < this.NumberOfBlocks; i++)
            {
                yield return BlockAt(i);
            }
        }

        #endregion

        #region private methods

        private ArraySegment<byte> BlockAt(int index)
        {
            var block = new ArraySegment<byte>(this.dataCodewords, this.offset + (index * this.BlockSize), this.BlockSize);
            return block;
        }

        #endregion
    }
}
