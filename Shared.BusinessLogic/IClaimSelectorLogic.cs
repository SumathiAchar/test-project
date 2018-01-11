using System.Collections.Generic;
using System.Threading.Tasks;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface IClaimSelectorLogic
    {
        /// <summary>
        /// Gets the background adjudication task.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        long GetBackgroundAdjudicationTask(long? facilityId, int batchSize, int timeout);

        /// <summary>
        /// Gets the adjudicated facilities.
        /// </summary>
        /// <param name="facilityIds">The facility ids.</param>
        /// <returns></returns>
        IEnumerable<TrackTask> GetAdjudicatedTasks(string facilityIds);

        /// <summary>
        /// Adjudicates the claim data thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="isBackgroundTask">if set to <c>true</c> [is background task].</param>
        /// <returns></returns>
        Task<Dictionary<long, List<PaymentResult>>> AdjudicateClaimDataThread(long? taskId, int facilityId,
            int noOfRecords, string connectionString, bool isBackgroundTask);
    }
}
