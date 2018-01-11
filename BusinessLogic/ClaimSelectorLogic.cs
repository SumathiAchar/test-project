
/************************************************************************************************************/
/**  Author         : Prasad Dintakurti
/**  Created        : 04-Sep-2013
/**  Summary        : Handles Add/Modify Select Claims Logic Details functionalities

/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ClaimSelectorLogic : IClaimSelectorLogic
    {
        private readonly IClaimSelectorRepository _claimSelectorRepository;

        private readonly IAdjudicationEngine _adjudicationEngine;
        private List<Task> _tasks;
        private Queue<AdjudicatedClaimsResult> _paymentResultQueue;
        private bool _isJobRunning;
        private int _noOfRecordsPerJob;
        private long _taskIdforPaymentResult;
        private List<Contract> _contractsforPaymentResult;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimSelectorLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimSelectorLogic(string connectionString)
        {
            _connectionString = connectionString;
            _adjudicationEngine = Factory.CreateInstance<IAdjudicationEngine>(connectionString, true);
            _claimSelectorRepository = Factory.CreateInstance<IClaimSelectorRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimSelectorLogic"/> class.
        /// </summary>
        /// <param name="selectClaimsRepository">The select claims repository.</param>
        /// <param name="adjudicationEngine">The adjudication engine.</param>
        public ClaimSelectorLogic(IClaimSelectorRepository selectClaimsRepository, IAdjudicationEngine adjudicationEngine)
        {
            if (selectClaimsRepository != null)
                _claimSelectorRepository = selectClaimsRepository;

            if (adjudicationEngine != null)
                _adjudicationEngine = adjudicationEngine;
        }

        /// <summary>
        /// Adds the new Add Select Claims.
        /// </summary>
        public long AddEditSelectClaims(ClaimSelector claimSelector)
        {

            if (claimSelector != null)
            {
                if (claimSelector.DateFrom == DateTime.MinValue ||
                    claimSelector.DateTo == DateTime.MinValue || claimSelector.DateFrom == null ||
                    claimSelector.DateTo == null)
                {
                    claimSelector.DateTo = DateTime.Now;
                    claimSelector.DateFrom =
                        DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                }
                if (claimSelector.DateType == 0 || claimSelector.DateType == null)
                {
                    claimSelector.DateType = 1;
                }


                claimSelector.RunningStatus = GlobalConfigVariable.IsAdjudicationUsingWindowsService
                    ? Convert.ToInt16(Enums.JobStatus.Requested, CultureInfo.InvariantCulture)
                    : Convert.ToInt16(Enums.JobStatus.Debug, CultureInfo.InvariantCulture);
                long taskId = _claimSelectorRepository.AddEditSelectClaims(claimSelector);

                //TODO: Right now we configured calling Adjudication type(windows job/normal adjudication) in web.config for testing purpose
                if (!GlobalConfigVariable.IsAdjudicationUsingWindowsService)
                {
                    //Adjudicate ClaimData based on the taskid and facilityId
                    AdjudicateClaimDataThread(taskId, claimSelector.FacilityId, GlobalConfigVariable.NoOfRecordsForAdjudicate, _connectionString, false);
                }
                return taskId;

            }
            return 0;
        }

        /// <summary>
        /// Gets the Claims count based on given search criteria
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        public long GetSelectedClaimList(ClaimSelector claimSelector)
        {
            if (claimSelector != null)
            {
                if (claimSelector.DateFrom == DateTime.MinValue ||
                     claimSelector.DateTo == DateTime.MinValue || claimSelector.DateFrom == null ||
                                                                  claimSelector.DateTo == null)
                {
                    claimSelector.DateTo = DateTime.Now;
                    claimSelector.DateFrom =
                        DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                }

                if (claimSelector.DateType == 0 || claimSelector.DateType == null)
                {
                    claimSelector.DateType = (byte)Enums.DateTypeFilter.DateofserviceandAdmissiondate;
                }

                return _claimSelectorRepository.GetClaimsCountForAdjudication(claimSelector);
            }
            return 0;
        }

        #region Multithreading

        /// <summary>
        /// Adjudicates the claim data thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="isBackgroundTask">if set to <c>true</c> [is background task].</param>
        /// <returns></returns>
        public Task<Dictionary<long, List<PaymentResult>>> AdjudicateClaimDataThread(long? taskId, int facilityId,
            int noOfRecords, string connectionString, bool isBackgroundTask)
        {
            if (taskId.HasValue)
            {
                try
                {
                    long claimsCount = AddClaimsForATask(Convert.ToInt64(taskId));

                    // Get Contracts for Adjudication
                    List<Contract> contracts = GetContracts(Convert.ToInt64(taskId),
                        facilityId, claimsCount);

                    // Adjudication with multithreading
                    AdjudicateClaimTasks(Convert.ToInt64(taskId), noOfRecords, claimsCount, contracts, connectionString, isBackgroundTask);
                }
                catch (Exception ex)
                {
                    TrackTask task = new TrackTask
                    {
                        TaskId = Convert.ToString(taskId, CultureInfo.InvariantCulture),
                        Status = Convert.ToString((byte)Enums.JobStatus.Failed, CultureInfo.InvariantCulture),
                        UserName = Constants.BackgroundServiceUser
                    };
                    Log.LogError(Constants.ClaimSelectionExceptionLog + taskId, Constants.BackgroundServiceUser, ex);
                    UpdateJobStatus(task);
                }
            }
            return Task.FromResult(new Dictionary<long, List<PaymentResult>>());
        }

        /// <summary>
        /// Adjudicates the claim tasks.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="claimsCount">The claims count.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="isBackgroundTask">if set to <c>true</c> [is background task].</param>
        private void AdjudicateClaimTasks(long taskId, int noOfRecords, long claimsCount, List<Contract> contracts, string connectionString, bool isBackgroundTask)
        {
            long startRow = Constants.One;
            _tasks = new List<Task>();
            _paymentResultQueue = new Queue<AdjudicatedClaimsResult>();
            _taskIdforPaymentResult = taskId;
            _noOfRecordsPerJob = noOfRecords;
            _contractsforPaymentResult = contracts;
            int threadcount = isBackgroundTask? Constants.One :((claimsCount < Constants.Four) ? Constants.One : GlobalConfigVariable.ThreadCount);
            for (int threadIndex = 1; threadIndex <= threadcount; threadIndex++)
            {
                long endRow = GetClaimRange(claimsCount, threadcount, threadIndex);
                long claimsCountForThread = ClaimsCountForThread(endRow, startRow);
                //Adding threads to queue
                _tasks.Add(AdjudicateClaimsDataTaskThread(taskId, noOfRecords, claimsCountForThread, contracts,
                    startRow, endRow, connectionString));
                startRow = endRow + 1;
            }
            // Update payment result using timer while thread doing adjudication. Once adjudication is done from threads, then timer will stop to update payment result.
            Timer updatePaymentResultTimer = new Timer();
            updatePaymentResultTimer.Elapsed += UpdatePaymentResultTimer;
            updatePaymentResultTimer.Interval = Constants.PaymentResultInterval;
            updatePaymentResultTimer.Start();
            Task.WaitAll(_tasks.ToArray());
            updatePaymentResultTimer.Stop();
            if (_paymentResultQueue != null && _paymentResultQueue.Count > 0)
            {
                bool isPaymentQueueEmpty = true;
                while (isPaymentQueueEmpty)
                {
                    if (!_isJobRunning && _paymentResultQueue.Count > 0)
                    {
                        var paymentResultDictionary = _paymentResultQueue.Dequeue();
                        if (_paymentResultQueue.Count == 0)
                        {
                            isPaymentQueueEmpty = false;
                        }
                        if (paymentResultDictionary != null)
                        {
                            //Update Payment result and earlyexit claims
                            if (!UpdatePaymentResultTask(taskId, noOfRecords, contracts, paymentResultDictionary))
                            {
                                isPaymentQueueEmpty = false;
                            }
                        }
                    }

                }
            }
            bool isPaymentResultUpdated = true;
            while (isPaymentResultUpdated)
            {
                if (!_isJobRunning)
                {
                    UpdateJob(Convert.ToInt64(taskId));
                    isPaymentResultUpdated = false;
                }
            }
        }

        /// <summary>
        /// Adjudicates the claims data task thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="claimsCountForThread">The claims count for thread.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private Task AdjudicateClaimsDataTaskThread(long taskId, int noOfRecords, long claimsCountForThread, List<Contract> contracts, long startRow, long endRow, string connectionString)
        {
            return
               Task.Run(
                   () => AdjudicateClaimsDataTask(taskId, noOfRecords, claimsCountForThread, contracts,
                       startRow, endRow, connectionString));
        }

        /// <summary>
        /// Updates the payment result timer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void UpdatePaymentResultTimer(object sender, ElapsedEventArgs e)
        {
            if (_paymentResultQueue != null && _paymentResultQueue.Count > 0 && !_isJobRunning)
            {
                lock (_paymentResultQueue)
                {
                    var paymentResultDictionary = _paymentResultQueue.Dequeue();
                    if (paymentResultDictionary != null)
                    {
                        _isJobRunning = true;
                        //Update Payment result and earlyexit claims
                        UpdatePaymentResultTask(_taskIdforPaymentResult, _noOfRecordsPerJob, _contractsforPaymentResult,
                            paymentResultDictionary);
                        _isJobRunning = false;
                    }
                }
            }
        }

        /// <summary>
        /// Claims Count for Thread
        /// </summary>
        /// <param name="endRow"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        private static long ClaimsCountForThread(long endRow, long startRow)
        {
            return ((endRow + Convert.ToInt64(1)) - startRow);
        }

        /// <summary>
        /// Get Claims Range
        /// </summary>
        /// <param name="claimsCount"></param>
        /// <param name="threadsCount"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static long GetClaimRange(long claimsCount, long threadsCount, int count)
        {
            return (claimsCount / threadsCount) * count + (claimsCount % threadsCount);
        }

        /// <summary>
        /// Adjudicates the claims data task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="claimsCount">The claims count for thread.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private void AdjudicateClaimsDataTask(long taskId, int noOfRecords, long claimsCount, List<Contract> contracts,
            long startRow, long endRow, string connectionString)
        {
            IAdjudicationEngine adjudicationEngine = Factory.CreateInstance<IAdjudicationEngine>(connectionString, true);
            try
            {
                while (claimsCount > 0)
                {
                    // Adjudication Starts
                    AdjudicatedClaimsResult paymentResultDictionary = adjudicationEngine.AdjudicateClaimsDataThread(taskId, noOfRecords, contracts, startRow, endRow);
                    lock (_paymentResultQueue)
                    {
                        // Adding payment result and claim information into Queue. If Job is paused, then have to stop adjudication by setting 'claimsCountForThread=0'
                        if (!paymentResultDictionary.IsPaused)
                        {
                            _paymentResultQueue.Enqueue(paymentResultDictionary);
                        }
                        else
                        {
                            claimsCount = 0;
                        }
                    }

                    claimsCount = claimsCount - noOfRecords;
                }
            }
            catch (Exception ex)
            {
                Log.LogError(Constants.AdjudicationExceptionLog + taskId, Constants.BackgroundServiceUser, ex);
            }
        }

        /// <summary>
        /// Updates the payment result task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="adjudicatedClaimsResult">The payment result.</param>
        /// <returns></returns>
        private bool UpdatePaymentResultTask(long taskId, int noOfRecords, List<Contract> contracts, AdjudicatedClaimsResult adjudicatedClaimsResult)
        {
            return _adjudicationEngine.UpdatePaymentResultsThread(taskId, noOfRecords, contracts, adjudicatedClaimsResult.PaymentResult,
               adjudicatedClaimsResult.AdjudicateClaims, adjudicatedClaimsResult.EarlyExitClaims);
        }

        /// <summary>
        /// UpdateJob
        /// </summary>
        /// <param name="taskId"></param>
        private void UpdateJob(long? taskId)
        {
            UpdateRunningTasksThread(Convert.ToInt64(taskId));
            TrackTask task = new TrackTask
            {
                TaskId = Convert.ToString(taskId, CultureInfo.InvariantCulture),
                Status = Convert.ToString((byte)Enums.JobStatus.Completed, CultureInfo.InvariantCulture),
                UserName = Constants.BackgroundServiceUser
            };
            UpdateJobStatus(task);
        }

        /// <summary>
        /// AddClaimsForATask
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        private long AddClaimsForATask(long taskId)
        {
            return _adjudicationEngine.AddClaimsForATask(taskId);
        }

        /// <summary>
        /// Adjudicates the task claims thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="totalClaimCount">The total claim count.</param>
        /// <returns></returns>
        private List<Contract> GetContracts(long taskId, int facilityId, long totalClaimCount)
        {
            return _adjudicationEngine.GetContracts(taskId, facilityId, totalClaimCount);
        }

        /// <summary>
        /// Updates the running tasks thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        private void UpdateRunningTasksThread(long taskId)
        {
            _adjudicationEngine.UpdateRunningTasksThread(taskId);
        }

        /// <summary>
        /// UpdateJobStatus
        /// </summary>
        /// <param name="task"></param>
        private void UpdateJobStatus(TrackTask task)
        {
            _claimSelectorRepository.UpdateJobStatus(task);
        }

        #endregion Multithreading

        /// <summary>
        /// Gets the ssi number for background ajudication.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ajudication"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ssi"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<int> GetSsiNumberForBackgroundAjudication()
        {
            return _claimSelectorRepository.GetSsiNumberForBackgroundAjudication();
        }

        /// <summary>
        /// Checks the adjudication request name exist.
        /// </summary>
        /// <param name="claimSelector">The select claims.</param>
        /// <returns></returns>
        public bool CheckAdjudicationRequestNameExist(ClaimSelector claimSelector)
        {
            return _claimSelectorRepository.CheckAdjudicationRequestNameExist(claimSelector);
        }

        /// <summary>
        /// Selects the claimfor background adjudication.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="batchSize">The batch size for background adjudication.</param>
        /// <param name="timeout">command timeout</param>
        /// <returns></returns>
        public long GetBackgroundAdjudicationTask(long? facilityId, int batchSize, int timeout)
        {
            return _claimSelectorRepository.GetBackgroundAdjudicationTask(facilityId, batchSize, timeout);
        }

        /// <summary>
        /// Gets the adjudicated facilities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TrackTask> GetAdjudicatedTasks(string facilityIds)
        {
            return _claimSelectorRepository.GetAdjudicatedTasks(facilityIds);
        }

        /// <summary>
        /// Reviews the claim.
        /// </summary>
        /// <param name="claimsReviewed">The claims reviewed.</param>
        /// <returns></returns>
        public bool ReviewClaim(IEnumerable<ClaimsReviewed> claimsReviewed)
        {
            return _claimSelectorRepository.ReviewClaim(claimsReviewed);
        }

        /// <summary>
        /// Reviewed all claims.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <returns></returns>
        public bool ReviewedAllClaims(SelectionCriteria selectionCriteria)
        {
            return _claimSelectorRepository.ReviewedAllClaims(selectionCriteria);
        }

        /// <summary>
        /// Adds the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        public ClaimNote AddClaimNote(ClaimNote claimNote)
        {
            return _claimSelectorRepository.AddClaimNote(claimNote);
        }

        /// <summary>
        /// Deletes the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        public bool DeleteClaimNote(ClaimNote claimNote)
        {
            return _claimSelectorRepository.DeleteClaimNote(claimNote);
        }

        /// <summary>
        /// Gets the claim notes.
        /// </summary>
        /// <param name="claimNotesContainer">The claim notes container.</param>
        /// <returns></returns>
        public ClaimNotesContainer GetClaimNotes(ClaimNotesContainer claimNotesContainer)
        {
            return _claimSelectorRepository.GetClaimNotes(claimNotesContainer);
        }
    }
}
