using System;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using System.Threading;
using System.Threading.Tasks;
using SSI.ContractManagement.Shared.Helpers.Unity;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// JOb class to create threads for each adjudication job
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class Job : IDisposable
    {
        private readonly ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        private readonly object _statusLock = new object();
        private Task _task;

        public long TaskId { get; private set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private Enums.JobStatus Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "jobid"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Job GetJobObject(long jobid)
        {
            return new Job(jobid);
        }

        /// <summary>
        /// Initializes the job
        /// </summary>
        public Job(long idJob)
        {
            TaskId = idJob;

            // Set the status
            lock (_statusLock)
            {
                Status = Enums.JobStatus.Requested;
            }
        }

        /// <summary>
        /// Initializes the job
        /// </summary>
        public Job(long idJob, Task task)
        {
            TaskId = idJob;
            _task = task;
            // Set the status
            lock (_statusLock)
            {
                Status = Enums.JobStatus.Requested;
            }
        }

        /// <summary>
        /// Runs the job.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="connectionString">The connection string.</param>
        public void RunJob(long taskId, int facilityId, string connectionString)
        {
            var selectClaimsLogic = Factory.CreateInstance<IClaimSelectorLogic>(connectionString, true);
            // Create a new task to start adjudicating claims as per the conditions in the given idJob and facilityid
            _task =
                Task.Factory.StartNew(
                    () =>
                      selectClaimsLogic.AdjudicateClaimDataThread(taskId, facilityId, GlobalConfigVariable.NoOfRecordsForAdjudicate,
                            connectionString, false));

            lock (_statusLock)
            {
                Status = Enums.JobStatus.Running;
            }
        }

        /// <summary>
        /// Pause the job
        /// </summary>
        public void Pause()
        {
            // Set the status
            lock (_statusLock)
            {
                Status = Enums.JobStatus.Paused;
            }

            // Reset the event handler to make the thread wait
            _pauseEvent.Reset();
        }

        /// <summary>
        /// Resumes the job
        /// </summary>
        public void Resume()
        {
            // Set the status
            lock (_statusLock)
            {
                Status = Enums.JobStatus.Running;
            }

            // Set the event handler to make the thread resume execution within the loop
            _pauseEvent.Set();
        }

        /// <summary>
        /// Stop the job
        /// </summary>
        public void Stop()
        {
            // Set the status
            lock (_statusLock)
            {
                Status = Enums.JobStatus.Cancelled;
            }

            //Signal the shutdown event. Set the event handler to make the thread exit gracefully
            _shutdownEvent.Set();

            // Make sure to resume any paused threads
            _pauseEvent.Set();
            // Wait for a minute for the task to complete - OPTIONAL
            _task.Wait(60000);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _shutdownEvent.Close();
            _pauseEvent.Close();
        }
    }
}

