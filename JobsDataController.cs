using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Adjudication
{
    // ReSharper disable once UnusedMember.Global
    public class JobsDataController : BaseController
    {
        private readonly JobStatusLogic _jobStatusLogic;

        /// <summary>
        /// Default Constructor
        /// </summary>
        JobsDataController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _jobStatusLogic = new JobStatusLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the contract hierarchy.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<TrackTask> GetAllJobs(TrackTask data)
        {
            return _jobStatusLogic.GetAllJobs(data);
        }

        /// <summary>
        /// Updates job status
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int UpdateJobStatus(TrackTask job)
        {
            return _jobStatusLogic.UpdateJobStatus(job);
        }


        /// <summary>
        /// Gets count of jobs alert
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public int JobCountAlert(TrackTask job)
        {
            return _jobStatusLogic.JobCountAlert(job);
        }

        /// <summary>
        /// Updates the job verified status.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public bool UpdateJobVerifiedStatus(TrackTask job)
        {
            return _jobStatusLogic.UpdateJobVerifiedStatus(job);
        }

        /// <summary>
        /// Determines whether [is model exist] [the specified job].
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public bool IsModelExist(TrackTask job)
        {
            return _jobStatusLogic.IsModelExist(job);
        }

        /// <summary>
        /// Res the adjudicate.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public long ReAdjudicate(TrackTask job)
        {
            return _jobStatusLogic.ReAdjudicate(job);
        }

        /// <summary>
        /// Gets Open Claim Columns By UserId
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimColumnOptions GetOpenClaimColumnOptionByUserId(ClaimColumnOptions data)
        {
            return _jobStatusLogic.GetOpenClaimColumnOptionByUserId(data);
        }

        /// <summary>
        /// Save Claim Column Options.
        /// </summary>
        /// <param name="claimColumnsInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveClaimColumnOptions(ClaimColumnOptions claimColumnsInfo)
        {
            return _jobStatusLogic.SaveClaimColumnOptions(claimColumnsInfo);
        }
    }
}
