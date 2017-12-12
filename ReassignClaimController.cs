using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Adjudication
{
    public class ReassignClaimController : BaseController
    {

        private readonly ReassignClaimLogic _reassignClaimLogic;

        /// <summary>
        /// Default Constructor
        /// </summary>
        ReassignClaimController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _reassignClaimLogic = new ReassignClaimLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the reassign grid data.
        /// </summary>
        /// <param name="claimSearchCriteria">The claim search criteria.</param>
        /// <returns></returns>
        [HttpPost]
        public ReassignClaimContainer GetReassignGridData(ClaimSearchCriteria claimSearchCriteria)
        {
            return _reassignClaimLogic.GetReassignGridData(claimSearchCriteria);
        }

        /// <summary>
        /// Gets the contracts by node identifier.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        [HttpPost]
        public List<Shared.Models.Contract> GetContractsByNodeId(ContractHierarchy contractHierarchy)
        {
            return _reassignClaimLogic.GetContractsByNodeId(contractHierarchy);
        }

        /// <summary>
        /// Adds the reassigned claim job.
        /// </summary>
        /// <param name="reassignedClaimJob">The reassigned claim job.</param>
        /// <returns></returns>
        [HttpPost]
        public bool AddReassignedClaimJob(ReassignedClaimJob reassignedClaimJob)
        {
            return _reassignClaimLogic.AddReassignedClaimJob(reassignedClaimJob);
        }

        /// <summary>
        /// Gets the claim linked count.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        [HttpPost]
        public int GetClaimLinkedCount(ContractHierarchy contractHierarchy)
        {
            return _reassignClaimLogic.GetClaimLinkedCount(contractHierarchy);
        }
        

    }
}
