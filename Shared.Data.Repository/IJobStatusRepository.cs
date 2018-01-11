using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IJobStatusRepository : IDisposable
    {
        /// <summary>
        /// Gets all jobs.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<TrackTask> GetAllJobs(TrackTask data);

        /// <summary>
        /// Updates the job status.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        int UpdateJobStatus(TrackTask job);

        /// <summary>
        /// Gets count for job alerts.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        int JobCountAlert(TrackTask job);

        /// <summary>
        /// Updates the job verified status.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        bool UpdateJobVerifiedStatus(TrackTask job);

        /// <summary>
        /// Gets all jobs for adjudication.
        /// </summary>
        /// <param name="jobStatusCodesIncluded"></param>
        /// <returns></returns>
        List<TrackTask> GetAllJobsForAdjudication(string jobStatusCodesIncluded);
        

        /// <summary>
        /// Determines whether [is manual adjudication running].
        /// </summary>
        /// <returns></returns>
        bool IsManualAdjudicationRunning();

        /// <summary>
        /// Cleanups the cancelled tasks.
        /// </summary>
        /// <returns></returns>
        bool CleanupCancelledTasks();

        /// <summary>
        /// Determines whether [is model exist] [the specified job].
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        bool IsModelExist(TrackTask job);

        /// <summary>
        /// Res the adjudicate.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        long ReAdjudicate(TrackTask job);

        /// <summary>
        /// Gets Open Claim Columns By UserId
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ClaimColumnOptions GetOpenClaimColumnOptionByUserId(ClaimColumnOptions data);

        /// <summary>
        /// Save Claim Column Options.
        /// </summary>
        /// <param name="claimColumnsInfo"></param>
        /// <returns></returns>
        bool SaveClaimColumnOptions(ClaimColumnOptions claimColumnsInfo);
    }
}
