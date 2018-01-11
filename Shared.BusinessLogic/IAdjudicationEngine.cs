using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Adjudication Engine class
    /// </summary>
    public interface IAdjudicationEngine
    {
        /// <summary>
        /// Adjudicates the claim.
        /// </summary>
        /// <param name="evaluateableClaims">The evaluate able claims.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        Dictionary<long, List<PaymentResult>> AdjudicateClaim(List<EvaluateableClaim> evaluateableClaims,
            List<Contract> contracts, long taskId);


        /// <summary>
        /// Adds the claims for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        long AddClaimsForATask(long taskId);


        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="totalClaimCount">The total claim count.</param>
        /// <returns></returns>
        List<Contract> GetContracts(long taskId, int facilityId, long totalClaimCount);

        /// <summary>
        /// Adjudicates the claims data thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <returns></returns>
        AdjudicatedClaimsResult AdjudicateClaimsDataThread(long taskId, int noOfRecords, List<Contract> contracts, long startRow, long endRow);

        /// <summary>
        /// Updates the payment results thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <param name="earlyExitClaims">The early exit claims.</param>
        /// <returns></returns>
        bool UpdatePaymentResultsThread(long taskId, int noOfRecords, List<Contract> contracts,
             Dictionary<long, List<PaymentResult>> paymentResultDictionary, List<EvaluateableClaim> adjudicatedClaims, List<EvaluateableClaim> earlyExitClaims);

        /// <summary>
        /// Updates the running tasks thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        void UpdateRunningTasksThread(long taskId);
    }
}
