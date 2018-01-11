using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Interface for evaluate able claim logic class
    /// </summary>
    public interface IEvaluateableClaimLogic : IAdjudicationBaseLogic
    {
        /// <summary>
        /// Updates the evaluate able claims.
        /// </summary>
        /// <param name="evaluateableClaims">The evaluate able claims.</param>
        /// <returns></returns>
        List<EvaluateableClaim> UpdateEvaluateableClaims(List<EvaluateableClaim> evaluateableClaims);
    }
}
