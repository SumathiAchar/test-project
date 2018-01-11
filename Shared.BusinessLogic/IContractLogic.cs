using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// interface for the Contract Logic class
    /// </summary>
    public interface IContractLogic : IAdjudicationBaseLogic
    {
        /// <summary>
        /// Gets or sets the contract.
        /// </summary>
        /// <value>
        /// The contract.
        /// </value>
        Contract Contract { get; set; }

        /// <summary>
        /// Updates the contract conditions.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        Contract UpdateContractCondition(Contract contract);

        /// <summary>
        /// Gets or sets the adjudicate claims.
        /// </summary>
        /// <value>
        /// The adjudicate claims.
        /// </value>
        List<EvaluateableClaim> AdjudicateClaims { get; set; }

        /// <summary>
        /// Gets or sets the early exit claims.
        /// </summary>
        /// <value>
        /// The early exit claims.
        /// </value>
        List<EvaluateableClaim> EarlyExitClaims { get; set; }
    }
}
