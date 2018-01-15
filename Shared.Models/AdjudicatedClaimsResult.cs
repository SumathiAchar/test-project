using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class AdjudicatedClaimsResult
    {
        /// <summary>
        /// Gets or sets the payment result.
        /// </summary>
        /// <value>
        /// The payment result.
        /// </value>
        public Dictionary<long, List<PaymentResult>> PaymentResult { get; set; }

        /// <summary>
        /// Gets or sets the adjudicate claims.
        /// </summary>
        /// <value>
        /// The adjudicate claims.
        /// </value>
        public List<EvaluateableClaim> AdjudicateClaims { get; set; }

        /// <summary>
        /// Gets or sets the early exit claims.
        /// </summary>
        /// <value>
        /// The early exit claims.
        /// </value>
        public List<EvaluateableClaim> EarlyExitClaims { get; set; }

        /// <summary>
        /// Gets or sets IsPaused
        /// </summary>
        /// <value>
        /// The IsPaused
        /// </value>
        public bool IsPaused { get; set; }

    }
}
