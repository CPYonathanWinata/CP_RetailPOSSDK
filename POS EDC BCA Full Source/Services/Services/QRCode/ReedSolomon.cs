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
    /// Reed Solomon error correction code generator.
    /// </summary>
    internal class ReedSolomon
    {
        private readonly GaloisField _galoisField;

        public ReedSolomon()
        {
            this._galoisField = new GaloisField();
        }

        /// <summary>
        /// Generate error correction code for the given input.
        /// Generation depends on the size of the output array.
        /// </summary>
        /// <param name="input">Data for which error correction code needs to be calculated.</param>
        /// <param name="output">Calculated error correction code.</param>
        public void Generate(ArraySegment<byte> input, byte[] output)
        {
            var generator = GeneratorPolynomials.Get(output.Length);

            var rs = new RSCode(generator, this._galoisField);

            rs.GetErrorCode(input).CopyTo(output, 0);
        }
    }

    internal class RSCode
    {
        private int[] g;
        private byte[] b;
        private int k;
        private GaloisField gf;
        public RSCode(int[] polynomial, GaloisField gf)
        {
            this.g = polynomial;
            this.k = polynomial.Length;
            this.gf = gf;
            this.b = new byte[k];
        }
        public RSCode()
        {
            this.g = new int[15] { 105, 99, 5, 124, 140, 237, 58, 58, 51, 37, 202, 91, 61, 183, 8 };
            this.k = g.Length;
            this.gf = new GaloisField();
            this.b = new byte[k];
        }
        private void FlushInput(int input)
        {
            input = gf.GetByValue((byte)(gf[input] ^ b[k - 1]));
            for (int i = k - 1; i > 0; i--)
            {
                b[i] = (input != -1) ? (byte)(gf[input + g[i]] ^ b[i - 1]) : b[i - 1];
            }
            b[0] = (input != -1) ? gf[input + g[0]] : (byte)0;
        }
        /// <summary>
        /// Input the exponent and get the error code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] GetErrorCode(int[] input)
        {
            foreach (int i in input)
                this.FlushInput(i);
            //reverse the order
            for (var i = 0; i < b.Length / 2; i++)
            {
                var temp = b[i];
                b[i] = b[b.Length - 1 - i];
                b[b.Length - 1 - i] = temp;
            }
            return b;
        }
        /// <summary>
        /// Input the binary data and get the error code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] GetErrorCode(ArraySegment<byte> input)
        {
            for (var index = 0; index < input.Count; index++)
            {
                var i = gf.GetByValue(input.Array[input.Offset + index]);
                this.FlushInput(i);
            }
            //reverse the order
            for (var i = 0; i < b.Length / 2; i++)
            {
                var temp = b[i];
                b[i] = b[b.Length - 1 - i];
                b[b.Length - 1 - i] = temp;
            }
            return b;
        }
    }
}
