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
    using LSRetailPosis;

    /// <summary>
    /// Responsible for applying Mask Patterns to a Symbol.
    /// </summary>
    internal class MaskPatternApplier
    {
        #region fields

        private readonly ISymbol symbol;
        private readonly IEnumerable bitstream;

        #endregion

        #region constructors

        /// <summary>
        /// Create a Mask Pattern applier for a symbol and codewords.
        /// The Symbol must have Function Patterns, Format Information,
        /// and Version Information filled so that it's IsInitial method
        /// returns true only for modules in the Encoding Region.
        /// </summary>
        /// <param name="symbol">Symbol with empty Encoding Region.</param>
        /// <param name="codewords">Codewords to which Mask Patterns will be applied.</param>
        public MaskPatternApplier(ISymbol symbol, IEnumerable codewords)
        {
            this.symbol = symbol;
            this.bitstream = codewords;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Apply a Mask Pattern and create a Symbol.
        /// </summary>
        /// <param name="pattern">Pattern to apply.</param>
        /// <returns>Symbol with the Mask Pattern applied.</returns>
        public void Apply(MaskPattern.ConditionDelegate pattern)
        {
            using (var encodingRegion = new EncodingRegion(this.symbol))
            {
                FillEncodingRegion(encodingRegion, pattern);
            }
        }

        /// <summary>
        /// Fill the Encoding Region by applying the given Mask Pattern.
        /// The delegate parameter interferes with both _Accessor and PrivateObjects,
        /// so this method is kept public as a work around for testing purposes.
        /// </summary>
        /// <param name="encodingRegion">Encoding Region to fill.</param>
        /// <param name="pattern">Pattern to apply.</param>
        public void FillEncodingRegion(IEnumerator<Module> encodingRegion, MaskPattern.ConditionDelegate pattern)
        {
            foreach (bool bit in this.bitstream)
            {
                if (encodingRegion.MoveNext())
                {
                    // Get the mask bit for the current module, do the masking, and place
                    // the resuting bit into the current module.
                    encodingRegion.Current.Set(bit ^ encodingRegion.Current.GetMask(pattern));
                }
                else
                {
                    throw new ArgumentException(ApplicationLocalizer.Language.Translate(2805), "encodingRegion");
                }
            }

            // Fill Remainder Bits
            while (encodingRegion.MoveNext())
            {
                encodingRegion.Current.Set(false);
            }
        }

        #endregion
    }
}
