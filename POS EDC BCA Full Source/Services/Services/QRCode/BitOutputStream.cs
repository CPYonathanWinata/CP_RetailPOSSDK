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
    using LSRetailPosis;

    /// <summary>
    /// A bit output stream backed by a byte array.
    /// </summary>
    internal class BitOutputStream
    {
        #region fields

        private const int NumberOfBitsInByte = 8;

        private int byteIndex;
        private int bitIndex; //offset from Data[byteIndex]

        #endregion

        #region properties

        /// <summary>
        /// The Bit output stream data
        /// </summary>
        public byte[] Data
        {
            get; private set;
        }

        /// <summary>
        /// Gets the number of remaining bits.
        /// </summary>
        public int NumberOfRemainingBits
        {
            get
            {
                return (this.Data.Length - byteIndex) * NumberOfBitsInByte - bitIndex;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new instance of the <c>BitOutputStream</c>.
        /// </summary>
        /// <param name="byteSize">The size of the byte array</param>
        public BitOutputStream(int byteSize)
        {
            this.Data = new byte[byteSize];
        }

        #endregion

        #region public methods

        /// <summary>
        /// Write bits until the byte boundary.
        /// Does nothing if already at a boundary.
        /// </summary>
        public void WriteBitsUntilByteBoundary()
        {
            if (0 < bitIndex)
            {
                byteIndex++;
                bitIndex = 0;
            }
        }

        /// <summary>
        /// Write the first (from the left) numOfBits bits in b to the stream.
        /// </summary>
        /// <param name="b">Container for holding the bits.</param>
        public void Write(byte b)
        {
            Write(b, NumberOfBitsInByte);
        }

        /// <summary>
        /// Write the first (from the left) numOfBits bits in b to the stream.
        /// </summary>
        /// <param name="b">Container for holding the bits.</param>
        /// <param name="numOfBits">Number of bits to write.</param>
        public void Write(byte b, int numOfBits)
        {
            if (NumberOfBitsInByte < numOfBits)
            {
                throw new ArgumentOutOfRangeException("numOfBits", numOfBits, ApplicationLocalizer.Language.Translate(2800));
            }
            CleanseByte(ref b, numOfBits);

            var b1 = (byte)((uint)b >> bitIndex);  //scoot to the right if some bits are already in line
            this.Data[byteIndex] |= b1;

            var bitsWritten = Math.Min(numOfBits, NumberOfBitsInByte - bitIndex);
            bitIndex += bitsWritten;

            if (bitIndex == NumberOfBitsInByte)
            {
                byteIndex++;
                bitIndex = 0;
                if (bitsWritten < numOfBits)
                {
                    this.Data[byteIndex] = (byte)((uint)b << bitsWritten);
                    bitIndex = numOfBits - bitsWritten;
                }
            }
        }

        /// <summary>
        /// Write an integer in the specified number of bits.
        /// </summary>
        /// <param name="integer">Integer to be written.</param>
        /// <param name="numOfBits">Number of bits to use to represent the integer.</param>
        public void Write(uint integer, int numOfBits)
        {
            if (sizeof(uint) * NumberOfBitsInByte < numOfBits)
            {
                throw new ArgumentOutOfRangeException("numOfBits");
            }
            for (var i = 0; i < numOfBits; i += NumberOfBitsInByte)
            {
                var b = (byte)(integer >> 24 - i);
                Write(b, Math.Min(numOfBits - i, NumberOfBitsInByte));
            }
        }

        /// <summary>
        /// Write bits in the bit array into the stream.
        /// </summary>
        /// <param name="array">The array to write.</param>
        public void Write(BitArray array)
        {
            foreach (bool bit in array)
            {
                Write(bit);
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Write a bit into the stream.
        /// </summary>
        /// <param name="bit">The bit to write.</param>
        private void Write(bool bit)
        {
            if (bit)
            {
                // Use uint as a container with data in the Least Significant Byte
                var mask = (uint)0x00000080 >> bitIndex;
                var integer = this.Data[byteIndex];
                this.Data[byteIndex] = (byte) (integer | mask);
            }
            // Update index
            bitIndex++;
            if (bitIndex == NumberOfBitsInByte)
            {
                byteIndex++;
                bitIndex = 0;
            }
        }

        /// <summary>
        /// Make sure ineffective bits are 0.
        /// </summary>
        /// <param name="b">Container for bits.</param>
        /// <param name="numOfBits">Number of bits effective from the left.</param>
        static private void CleanseByte(ref byte b, int numOfBits)
        {
            byte mask = 0xFF;
            mask <<= NumberOfBitsInByte - numOfBits;
            b &= mask;
        }

        #endregion
    }
}
