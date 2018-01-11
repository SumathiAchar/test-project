using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Base class contains IsMatch & Evaluate method
    /// </summary>
    public interface IAdjudicationBaseLogic
    {
        /// <summary>
        /// Determines whether the specified claim is match.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        bool IsMatch(IEvaluateableClaim claim);
        
        /// <summary>
        /// Evaluates the specified claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        List<PaymentResult> Evaluate(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter);
    }
}
