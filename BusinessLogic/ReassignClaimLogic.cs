using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ReassignClaimLogic
    {
        private readonly IReassignClaimRepository _reassignClaimRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReassignClaimLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ReassignClaimLogic(string connectionString)
        {
            _reassignClaimRepository = Factory.CreateInstance<IReassignClaimRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReassignClaimLogic"/> class.
        /// </summary>
        /// <param name="reassignClaimRepository">The reassign claim repository.</param>
        public ReassignClaimLogic(IReassignClaimRepository reassignClaimRepository)
        {
            if (reassignClaimRepository != null)
                _reassignClaimRepository = reassignClaimRepository;
        }

        /// <summary>
        /// Gets the reassign grid data.
        /// </summary>
        /// <param name="claimSearchCriteria">The claim search criteria.</param>
        /// <returns></returns>
        public ReassignClaimContainer GetReassignGridData(ClaimSearchCriteria claimSearchCriteria)
        {
            return _reassignClaimRepository.GetReassignGridData(claimSearchCriteria);
        }

        /// <summary>
        /// Adds the reassigned claim job.
        /// </summary>
        /// <param name="reassignedClaimJob">The reassigned claim job.</param>
        /// <returns></returns>
        public bool AddReassignedClaimJob(ReassignedClaimJob reassignedClaimJob)
        {
            return _reassignClaimRepository.AddReassignedClaimJob(reassignedClaimJob);
        }

        /// <summary>
        /// Gets the contracts by node identifier.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Contract> GetContractsByNodeId(ContractHierarchy contractHierarchy)
        {
            return _reassignClaimRepository.GetContractsByNodeId(contractHierarchy);
        }

        /// <summary>
        /// Gets the claim linked count.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        public int GetClaimLinkedCount(ContractHierarchy contractHierarchy)
        {
            return _reassignClaimRepository.GetClaimLinkedCount(contractHierarchy);
        }
    }
}
