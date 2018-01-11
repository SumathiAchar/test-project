using System.Collections.Generic;
using System.Globalization;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{

    public class JobStatusLogic : IJobStatusLogic
    {
        /// <summary>
        /// The jobsRunning details repository
        /// </summary>
        private readonly IJobStatusRepository _jobsStatusRepository;
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<Job> _jobsRunning = new List<Job>();

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public JobStatusLogic(string connectionString)
        {
            _jobsStatusRepository = Factory.CreateInstance<IJobStatusRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusLogic"/> class.
        /// </summary>
        /// <param name="iJobStatusRepository">The i job status repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i")]
        public JobStatusLogic(IJobStatusRepository iJobStatusRepository)
        {
            _jobsStatusRepository = iJobStatusRepository;
        }

        /// <summary>
        /// Creates a job
        /// </summary>
        /// <param name="idJob"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public Job CreateJob(long idJob)
        {
            Job job = new Job(idJob).GetJobObject(idJob);
            _jobsRunning.Add(job);
            return job;
        }
        
        /// <summary>
        /// FEtches all not completed jobs
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<TrackTask> GetAllJobs(TrackTask data)
        {
            return _jobsStatusRepository.GetAllJobs(data);
        }

        /// <summary>
        /// Gets all jobs for adjudication.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<TrackTask> GetAllJobsForAdjudication()
        {
            string jobStatusCodesIncluded = string.Format(CultureInfo.InvariantCulture, "{0},{1}", (byte)Enums.JobStatus.Requested, (byte)Enums.JobStatus.Resumed);
            return _jobsStatusRepository.GetAllJobsForAdjudication(jobStatusCodesIncluded);
        }
        

        /// <summary>
        /// Updates the job status.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public int UpdateJobStatus(TrackTask job)
        {
            int jobStatus = _jobsStatusRepository.UpdateJobStatus(job);
            return jobStatus;
        }

        /// <summary>
        /// Gets count of Job alerts.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public int JobCountAlert(TrackTask job)
        {
            return _jobsStatusRepository.JobCountAlert(job);
        }
        /// <summary>
        /// Updates the job verified status.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public bool UpdateJobVerifiedStatus(TrackTask job)
        {
            return _jobsStatusRepository.UpdateJobVerifiedStatus(job);
        }

        /// <summary>
        /// Determines whether [is manual adjudication running].
        /// </summary>
        /// <returns></returns>
        public bool IsManualAdjudicationRunning()
        {
            return _jobsStatusRepository.IsManualAdjudicationRunning();
        }

        /// <summary>
        /// Cleanups the cancelled tasks.
        /// </summary>
        /// <returns></returns>
        public bool CleanupCancelledTasks()
        {
            return _jobsStatusRepository.CleanupCancelledTasks();
        }

        /// <summary>
        /// Determines whether [is model exist] [the specified job].
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public bool IsModelExist(TrackTask job)
        {
            return _jobsStatusRepository.IsModelExist(job);
        }

        /// <summary>
        /// Res the adjudicate.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re")]
        public long ReAdjudicate(TrackTask job)
        {
            return _jobsStatusRepository.ReAdjudicate(job);
        }
        /// <summary>
        /// Gets Adjudication Columns By UserId
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ClaimColumnOptions GetOpenClaimColumnOptionByUserId(ClaimColumnOptions data)
        {
            return _jobsStatusRepository.GetOpenClaimColumnOptionByUserId(data);
        }

        /// <summary>
        /// Save Claim Column Options.
        /// </summary>
        /// <param name="claimColumnsInfo"></param>
        /// <returns></returns>
        public bool SaveClaimColumnOptions(ClaimColumnOptions claimColumnsInfo)
        {
            return _jobsStatusRepository.SaveClaimColumnOptions(claimColumnsInfo);
        }
    }
}
