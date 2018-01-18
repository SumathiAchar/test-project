using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Windows.Services.OnDemandAdjudicationService
{
    public partial class OnDemandAdjudicationService : ServiceBase
    {
        private JobStatusLogic _jobManager;
        private Job _job;
        private FacilityLogic _facilityLogic;
        private Thread _processThread;
        private System.Timers.Timer _onDemandAdjudication;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnDemandAdjudicationService"/> class.
        /// </summary>
        public OnDemandAdjudicationService()
        {
            //InitializeComponent();
            OnDemandAdjudication();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            _onDemandAdjudication = new System.Timers.Timer();
            _onDemandAdjudication.Elapsed += onDemandAdjudication_Tick;
            _onDemandAdjudication.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["JobInterval"]);
            _onDemandAdjudication.Start();
        }

        /// <summary>
        /// Handles the Tick event of the onDemandAdjudication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void onDemandAdjudication_Tick(object sender, ElapsedEventArgs e)
        {
            _processThread = new Thread(OnDemandAdjudication);
            _processThread.Start();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            // Give thread a chance to stop
            _processThread.Join(Constants.OnDemandThreadWaitingTime);
            _onDemandAdjudication.Stop();
        }

        /// <summary>
        /// Called when [demand adjudication].
        /// </summary>
        private void OnDemandAdjudication()
        {
            try
            {
                //FIXED-MAR16 No need create object _jobManager with CMMembershipConnectionString. Create class called FacilityBubble and call CMMembershipConnectionString from there.
                _facilityLogic = new FacilityLogic();
                IEnumerable<FacilityBubble> facilityBubbles = _facilityLogic.GetBubbles();
                foreach (FacilityBubble facilityBubble in facilityBubbles)
                {
                    _jobManager = new JobStatusLogic(facilityBubble.ConnectionString);

                    //Get all jobs for adjudication which are not cancelled or completed
                    List<TrackTask> allJobs = _jobManager.GetAllJobsForAdjudication();

                    StartNewRequests(allJobs.FindAll(jobs => jobs.Status == Enums.JobStatus.Requested.ToString()),
                        facilityBubble.ConnectionString);
                    ResumePausedRequests(allJobs.FindAll(jobs => jobs.Status == Enums.JobStatus.Resumed.ToString()),
                        facilityBubble.ConnectionString);
                }

            }
            catch (Exception ex)
            {
                Log.LogError("Exception" + ex, string.Empty);
            }
        }

        /// <summary>
        /// Resumes the paused requests.
        /// </summary>
        /// <param name="allJobs">All jobs.</param>
        /// <param name="connectionString">The connection string.</param>
        private void ResumePausedRequests(IEnumerable<TrackTask> allJobs, string connectionString)
        {
            if (GlobalConfigVariable.IsLogEvent)
            {
                Log.LogInfo("Resume Request", ConfigurationManager.AppSettings["EventSource"]);
            }


            if (allJobs != null)
            {
                IEnumerable<TrackTask> resumedTasks =
                    allJobs.Where(
                        pauseJob =>
                            EnumHelperLibrary.GetEnumValueByFieldName<Enums.JobStatus>(pauseJob.Status) ==
                            Enums.JobStatus.Resumed);

                IEnumerable<TrackTask> trackTasks = resumedTasks as TrackTask[] ?? resumedTasks.ToArray();

                if (GlobalConfigVariable.IsLogEvent)
                {
                    Log.LogInfo("No of jobs :" + trackTasks.Count(), string.Empty);
                }
                foreach (TrackTask pauseJob in trackTasks)
                {
                    _job = new Job(Convert.ToInt64(pauseJob.TaskId));
                    _job.RunJob(Convert.ToInt64(pauseJob.TaskId), pauseJob.FacilityId, connectionString);
                }
            }
            else
            {
                if (GlobalConfigVariable.IsLogEvent)
                {
                    Log.LogInfo("No Jobs found", string.Empty);
                }
            }
        }

        /// <summary>
        /// Starts the new requests.
        /// </summary>
        /// <param name="allJobs">All jobs.</param>
        /// <param name="connectionString">The connection string.</param>
        private void StartNewRequests(IEnumerable<TrackTask> allJobs, string connectionString)
        {
            if (GlobalConfigVariable.IsLogEvent)
            {
                Log.LogInfo("New Job request", string.Empty);
            }

            if (allJobs != null)
                foreach (
                    TrackTask newJob in
                        allJobs.Where(
                            newJob => newJob.Status != null && newJob.Status == Enums.JobStatus.Requested.ToString()))
                {
                    if (GlobalConfigVariable.IsLogEvent)
                    {
                        Log.LogInfo("New Job status - " + newJob.Status + "........" + Enums.JobStatus.Requested,
                            string.Empty);
                        Log.LogInfo(
                            "Create Job - TaskId - JobStatus.Running.ToString()" + newJob.TaskId + ".. " +
                            Convert.ToInt16(Enums.JobStatus.Running), string.Empty);
                    }

                    _job = _jobManager.CreateJob(Convert.ToInt64(newJob.TaskId));
                    _job.RunJob(Convert.ToInt64(newJob.TaskId), newJob.FacilityId, connectionString);
                    TrackTask updatejob = new TrackTask
                    {
                        TaskId = newJob.TaskId,
                        Status =
                            Convert.ToString(
                                Convert.ToInt16(Enums.JobStatus.Running)),
                        UserName = "BackgroundServiceUser"
                    };

                    if (GlobalConfigVariable.IsLogEvent)
                    {
                        Log.LogInfo(
                            "Update Job request - TaskId - JobStatus.Running.ToString()" + newJob.TaskId + ".. " +
                            Convert.ToInt16(Enums.JobStatus.Running), string.Empty);
                    }

                    _jobManager.UpdateJobStatus(updatejob);

                }

        }
    }
}
