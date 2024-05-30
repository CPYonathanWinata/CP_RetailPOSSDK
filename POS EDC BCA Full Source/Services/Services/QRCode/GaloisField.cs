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

    /// <summary>
    /// A Galois Field of 2 to the 8th power.
    /// </summary>
    internal class GaloisField
    {
        #region fields

        private readonly int m;
        private readonly byte mask;
        private readonly byte[] values;
        private readonly int[] table;

        #endregion fields

        #region constructors

        /// <summary>
        /// Creates a new instance of the <c>GaloisField</c>.
        /// </summary>
        public GaloisField()
        {
            this.m = 8;
            this.mask = 0x1d;   // Last 8 bits of 100011101 which represents field's prime modulus polynomial
            this.values = new byte[(int)Math.Pow(2, m)];
            this.table = new int[values.Length];

            for (var i = 0; i < m; i++)
            {
                values[i] = (byte)Math.Pow(2, i);
                table[values[i]] = i;
            }
            for (var i = m; i < values.Length - 1; i++)
            {
                if (values[i - 1] >= values.Length / 2)
                {
                    values[i] = (byte)((values[i - 1] * 2) ^ mask);
                }
                else
                {
                    values[i] = (byte)(values[i - 1] * 2);
                }
                table[values[i]] = i;
            }
        }

        #endregion

        #region public methods

        public byte this[int index]
        {
            get
            {
                if (index < 0) return 0;
                return values[index % (values.Length - 1)];
            }
        }

        /// <summary>
        /// Gets the table value by index.
        /// </summary>
        /// <param name="value">The value index.</param>
        /// <returns></returns>
        public int GetByValue(byte value)
        {
            if (value != 0)
            {
                return table[value];
            }
            else
            {
                return -1;  //undefined
            }
        }

        #endregion
    }
}
