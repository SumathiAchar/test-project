using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Interface for the payment result
    /// </summary>
    public interface IPaymentResultLogic : IAdjudicationBaseLogic
    {
        /// <summary>
        /// Gets the payment result list.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        List<PaymentResult> GetPaymentResults(EvaluateableClaim claim);

        /// <summary>
        /// Updates the payment results.
        /// </summary>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="adjudicatedClaimsGuid">The adjudicated claims unique identifier.</param>
        /// <param name="earlyExitClaimsGuid">The early exit claims unique identifier.</param>
        /// <returns></returns>
        bool UpdatePaymentResults(Dictionary<long, List<PaymentResult>> paymentResultDictionary,
             int noOfRecords, long taskId, List<EvaluateableClaim> adjudicatedClaimsGuid, List<EvaluateableClaim> earlyExitClaimsGuid);

    }
}
