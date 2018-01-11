using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    /// <summary>
    /// Repository Interface for the EvaluateableClaim
    /// </summary>
    public interface IEvaluateableClaimRepository : IDisposable
    {

        /// <summary>
        /// Gets the evaluateable claims.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecord">The no of record.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <returns></returns>
        List<EvaluateableClaim> GetEvaluateableClaims(long taskId, int noOfRecord, long startRow, long endRow);

        /// <summary>
        /// Adds the claims for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        long AddClaimsForATask(long taskId);

        /// <summary>
        /// Updates the running task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="isRunning">The is running.</param>
        /// <returns></returns>
        void UpdateRunningTask(long taskId, byte isRunning);
    }
}
