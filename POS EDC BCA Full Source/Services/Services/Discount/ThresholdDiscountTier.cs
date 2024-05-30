/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

namespace Microsoft.Dynamics.Retail.Pos.DiscountService
{
    /// <summary>
    /// This class represents a tier configured in a threshold discount.
    /// </summary>
    public sealed class ThresholdDiscountTier
    {
        /// <summary>
        /// Hidden private constructor.
        /// </summary>
        private ThresholdDiscountTier()
        {
        }

        /// <summary>
        /// Constructs a threshold tier.
        /// </summary>
        /// <param name="offerId">Id of the offer this belongs to.</param>
        /// <param name="amountThreshold">The amount which triggers this tier.</param>
        /// <param name="discountMethod">The method of discounting used by this tier.</param>
        /// <param name="discountValue">The value (amount/percent) for this tier's discount.</param>
        /// <param name="recordId">The unique record Id of this tier.</param>
        public ThresholdDiscountTier(
            string offerId, 
            decimal amountThreshold, 
            ThresholdDiscountMethod discountMethod,
            decimal discountValue,
            long recordId)
        {
            if (string.IsNullOrWhiteSpace(offerId))
            {
                throw new ArgumentNullException("offerId");
            }

            RecordId = recordId;
            OfferId = offerId;
            AmountThreshold = amountThreshold;
            DiscountMethod = discountMethod;
            DiscountValue = discountValue;
        }

        /// <summary>
        /// Gets the record identifier.
        /// </summary>
        public long RecordId
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the Id of the offer associated to this tier.
        /// </summary>
        public string OfferId
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the amount which triggers this tier.
        /// </summary>
        public decimal AmountThreshold
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of discount given by this tier.
        /// </summary>
        public ThresholdDiscountMethod DiscountMethod
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value of the the discount given by this tier.
        /// </summary>
        public decimal DiscountValue
        {
            get;
            private set;
        }
    }
}
