using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Windows.Services.BackgroundAdjudicationService
{
    public partial class ComputeAdjudicationJob : ServiceBase
    {
        private const string JobStarted = "Compute BackgroundAdjudication Started.";
        private const string OnStartError = "OnStart encountered an error";
        private const string OnStartTrace = "OnStart Stack Trace : ";
        private const string EncounteredError = "Compute BackgroundAdjudication encountered an error";
        private const string JobStopped = "Background Adjudication is stopped and cleanup status is";
        private const string Succeed = "succeed";
        private const string Failed = "failed";
        private const string Minutes = "minutes.";
        private IJobStatusLogic _jobStatusLogic;
        private IClaimSelectorLogic _claimSelectorLogic;
        private readonly IFacilityLogic _facilityLogic;
        private readonly string _userName;

        public ComputeAdjudicationJob()
        {
            _facilityLogic = Factory.CreateInstance<IFacilityLogic>();
            _userName = GlobalConfigVariable.BackgroundUserNameForScheduleJob;
           //InitializeComponent();
           ComputeBackgroundAdjudication(GlobalConfigVariable.ThreadCountForBackground); //(1);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system 
        /// starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            try
            {
                Task adjudicationTask = new Task(() => ComputeBackgroundAdjudication(GlobalConfigVariable.ThreadCountForBackground));
                adjudicationTask.Start();
            }
            catch (Exception ex)
            {
                Log.LogError(string.Format("{0} '{1}'", OnStartError, ex.Message), _userName);
                Log.LogError(string.Format("{0} '{1}'", OnStartTrace, ex.StackTrace), _userName);
            }
        }

        /// <summary>
        /// CalculateVarianceMethodCall invokes the CalculateVarianceProcess method
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        private void ComputeBackgroundAdjudication(int threadCount)
        {
            try
            {
                // ReSharper disable once RedundantAssignment
                long taskId = 0;
                if (GlobalConfigVariable.IsLogEvent)
                {
                    Log.LogInfo(JobStarted, _userName);
                }
                List<Task> tasks = new List<Task>();

                //Actual Adjudication Method call
                //Fetches the connection string based on the database name  
                string bubbleConnectionString = _facilityLogic.GetBubbleConnectionString(GlobalConfigVariable.DbName);
                //FIXED-2016-R3-S3 - No need to fetch all facility data, fetch facilities based on bubbleConnectionString or DbName
                List<Facility> facilities = _facilityLogic.GetFacilitiesDataSource(bubbleConnectionString);
                List<TrackTask> adjudicatedTasks = new List<TrackTask>();
                //Get facilityIds for all the facilities
                string facilityIds = String.Join(Constants.Comma, facilities.Select(facility => facility.FacilityId).ToList());
                //Gets the FacilityIds of the particular bubble
                List<int> bubbleFacilities =
                    facilities.Select(facility => facility.FacilityId)
                        .ToList();
                   _claimSelectorLogic = Factory.CreateInstance<IClaimSelectorLogic>(bubbleConnectionString, true);
                    IEnumerable<TrackTask> adjudicatedTaskDetails = _claimSelectorLogic.GetAdjudicatedTasks(facilityIds);
                    adjudicatedTasks.AddRange(adjudicatedTaskDetails);
                //Last Adjudicated facilities should be shown at the first
                adjudicatedTasks = adjudicatedTasks.OrderBy(facility => facility.InsertDate).ToList();
                if (adjudicatedTasks.Count > 0)
                {
                    foreach (TrackTask adjudicatedTask in adjudicatedTasks)
                    {
                        bubbleFacilities.Remove(Convert.ToInt32(adjudicatedTask.FacilityId));
                        bubbleFacilities.Add(Convert.ToInt32(adjudicatedTask.FacilityId));
                    }
                }
                threadCount = facilities.Count > threadCount ? threadCount : facilities.Count;
                //Looping through each facilities to create the task
                for (int facilityIndex = 0; facilityIndex < bubbleFacilities.Count && threadCount > 0; facilityIndex++)
                {
                    if (facilities.FirstOrDefault(f => f.FacilityId == bubbleFacilities[facilityIndex]) != null)
                    {
                        //Creates the ClaimSelector instance based on the facility's connection string
                        _claimSelectorLogic = Factory.CreateInstance<IClaimSelectorLogic>(bubbleConnectionString,
                            true);
                        //Call claim pick SP by passing facility id. This will create a row in track task and insert claims to task claims table. It will return TaskId.
                        taskId = _claimSelectorLogic.GetBackgroundAdjudicationTask(bubbleFacilities[facilityIndex],
                            GlobalConfigVariable.BatchSizeForBackgroundAdjudication,
                            GlobalConfigVariable.TimeOutBackgroundAdjudication);
                        if (taskId > 0)
                        {
                            // ReSharper disable once AccessToModifiedClosure
                            if (bubbleFacilities.Count > facilityIndex)
                            {
                                int index = facilityIndex;
                                tasks.Add(
                                    Task.Run(
                                        () =>
                                            ComputeBackgroundAdjudicationTask(taskId, bubbleFacilities[index],
                                                bubbleConnectionString, true)));
                            }
                            threadCount--;
                        }
                        else if (taskId == -1)
                        {
                            PauseBackgroundAdjudicationProcess(GlobalConfigVariable.SleepIntervalWhenManualJobIsRunning, true);
                        }
                    }
                }
                Task.WaitAll(tasks.ToArray());
                if (tasks.Count == 0)
                {
                    PauseBackgroundAdjudicationProcess(GlobalConfigVariable.SleepIntervalWhenBackgroundJobCompletes, false);
                }
                else
                {
                    ComputeBackgroundAdjudication(GlobalConfigVariable.ThreadCountForBackground);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(
                    string.Format("{{{0} {1}.{2}}} - {3}", EncounteredError,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), _userName);
                ComputeBackgroundAdjudication(threadCount);
            }
        }

        /// <summary>
        /// Computes the background adjudication task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="bubbleConnectionString">The bubble connection string.</param>
        /// <param name="isBackgroundTask">if set to <c>true</c> [is background task].</param>
        private void ComputeBackgroundAdjudicationTask(long taskId, int facilityId, string bubbleConnectionString, bool isBackgroundTask)
        {
            _claimSelectorLogic.AdjudicateClaimDataThread(taskId, facilityId, GlobalConfigVariable.BatchSizeForBackgroundAdjudication, bubbleConnectionString, isBackgroundTask);
            //completedAdjudicationJobHandler(1);
        }


        /// <summary>
        /// Pauses the background adjudication process.
        /// </summary>
        /// <param name="sleepInterval">The sleep interval.</param>
        /// <param name="isManualAdjudicationRunning">if set to <c>true</c> [is manual adjudication running].</param>
        private void PauseBackgroundAdjudicationProcess(int sleepInterval, bool isManualAdjudicationRunning)
        {
            if (GlobalConfigVariable.IsLogEvent)
            {
                string eventLogComment = isManualAdjudicationRunning
                ? Constants.CommentTextWhenManualJobIsRunning
                : Constants.CommentTextWhenBackgroundJobCompleted;
                Log.LogInfo(string.Format("{0}{1} {2}", eventLogComment,
                        TimeSpan.FromMilliseconds(GlobalConfigVariable.SleepIntervalWhenManualJobIsRunning).TotalMinutes, Minutes), _userName);
            }
            //Thread will go in sleep mode for 2 minute if there any manual adjudication running 
            //or 1 hour if background adjudication has completed once full cycle for all available claim data
            Thread.Sleep(sleepInterval);
            //After 1 hour data will pick up for background adjudication has completed once full cycle
            if (!isManualAdjudicationRunning)
                ComputeBackgroundAdjudication(GlobalConfigVariable.ThreadCountForBackground);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            //Fetches the connection string based on the database name  
            string bubbleConnectionString = _facilityLogic.GetBubbleConnectionString(GlobalConfigVariable.DbName);
            _jobStatusLogic = Factory.CreateInstance<IJobStatusLogic>(bubbleConnectionString, true);
                //returns the result back as false why again try catch is used here.
                // CleanupCancelledTasks method is returning bool , either use this return type or make the method return type void
                bool cleanupStatus = _jobStatusLogic.CleanupCancelledTasks();

                if (GlobalConfigVariable.IsLogEvent)
                {
                    Log.LogInfo(string.Format("{0} {1}", JobStopped,
                            cleanupStatus ? Succeed : Failed), _userName);
                }
           
        }
    }
}
