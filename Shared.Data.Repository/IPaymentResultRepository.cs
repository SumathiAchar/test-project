using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    /// <summary>
    /// Interface for Payment result 
    /// </summary>
    public interface IPaymentResultRepository : IDisposable
    {

        /// <summary>
        /// Updates the payment results.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <param name="earlyExitClaims">The early exit claims.</param>
        /// <returns></returns>
        bool UpdatePaymentResults(List<PaymentResult> paymentResults, int noOfRecords, long taskId, List<EvaluateableClaim> adjudicatedClaims, List<EvaluateableClaim> earlyExitClaims);
        
    }
}
