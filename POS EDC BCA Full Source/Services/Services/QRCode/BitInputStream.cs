/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    using System.Collections.Generic;

    /// <summary>
    /// A bit input stream backed by a byte array.
    /// </summary>
    internal class BitInputStream : IEnumerable<bool>
    {
        #region fields

        private const int NumberOfBitsInByte = 8;
        private readonly byte[] bytes;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new instance of the <c>BitInputStream</c>.
        /// </summary>
        /// <param name="bytes">The bit input stream bytes</param>
        public BitInputStream(byte[] bytes)
        {
            this.bytes = bytes;
        }

        #endregion

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region public methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<bool> GetEnumerator()
        {
            for (var byteIndex = 0; byteIndex < this.bytes.Length; byteIndex++)
            {
                for (var shift = NumberOfBitsInByte - 1; 0 <= shift; shift--)
                {
                    yield return ((this.bytes[byteIndex] >> shift) & 1) != 0;
                }
            }
        }

        #endregion
    }
}
