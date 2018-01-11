using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IReassignClaimRepository : IDisposable
    {
        /// <summary>
        /// Gets the reassign grid data.
        /// </summary>
        /// <param name="claimSearchCriteria">The claim search criteria.</param>
        /// <returns></returns>
        ReassignClaimContainer GetReassignGridData(ClaimSearchCriteria claimSearchCriteria);

        /// <summary>
        /// Adds the reassigned claim job.
        /// </summary>
        /// <param name="reassignedClaimJob">The reassigned claim job.</param>
        /// <returns></returns>
        bool AddReassignedClaimJob(ReassignedClaimJob reassignedClaimJob);

        /// <summary>
        /// Gets the contracts by node identifier.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        List<Contract> GetContractsByNodeId(ContractHierarchy contractHierarchy);

        /// <summary>
        /// Gets the claim linked count.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        int GetClaimLinkedCount(ContractHierarchy contractHierarchy);
    }
}
