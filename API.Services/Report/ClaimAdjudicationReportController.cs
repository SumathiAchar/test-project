using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class ClaimAdjudicationReportController : BaseController
    {

        private readonly ClaimAdjudicationReportLogic _adjudicationReportLogic;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimAdjudicationReportController"/> class.
        /// </summary>
        public ClaimAdjudicationReportController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _adjudicationReportLogic = new ClaimAdjudicationReportLogic(bubbleDataSource);
        }
        

        /// <summary>
        /// Gets all claim adjudication.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimAdjudicationReport GetClaimAdjudicationReport(ClaimAdjudicationReport claimAdjudicationReport)
        {
            return _adjudicationReportLogic.GetClaimAdjudicationReport(claimAdjudicationReport);

        }

        /// <summary>
        /// Gets all claim adjudication.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimAdjudicationReport GetSelectedClaim(ClaimAdjudicationReport claimAdjudicationReport)
        {
            return _adjudicationReportLogic.GetSelectedClaim(claimAdjudicationReport);

        }
        /// <summary>
        /// Gets Open Claim Columns Names By UserId
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimAdjudicationReport GetOpenClaimColumnNamesBasedOnUserId(ClaimAdjudicationReport data)
        {
            return _adjudicationReportLogic.GetOpenClaimColumnNamesBasedOnUserId(data);
        }
    }
}
