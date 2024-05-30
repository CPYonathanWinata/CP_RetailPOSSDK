/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


namespace Microsoft.Dynamics.Retail.Pos.Qrcode
{
    /// <summary>
    /// In the encoding region of the QR Code symbol, symbol characters are
    /// positioned in two-module wide columns commencing at the lower right
    /// corner of the symbol and running alternately upwards and downwards
    /// from the right to the left.
    /// </summary>
    internal struct Module
    {
        #region fields

        private readonly ISymbol symbol;

        #endregion

        #region properties

        public int X { get; private set; }
        public int Y { get; private set; }

        #endregion

        #region constructors

        public Module(ISymbol symbol, int x, int y)
            : this()
        {
            this.symbol = symbol;
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region public methods

        public void Set(bool dark)
        {
            this.symbol.SetModule(this.X, this.Y, dark);
        }

        public bool GetMask(MaskPattern.ConditionDelegate condition)
        {
            return condition(this.X, this.Y);
        }

        #endregion
    }
}
