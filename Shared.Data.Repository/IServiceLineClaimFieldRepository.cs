using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IServiceLineClaimFieldRepository : IDisposable
    {
        /// <summary>
        /// Adds the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        long AddNewClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection);

        /// <summary>
        /// Edit the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        long EditClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection);

        /// <summary>
        /// Gets the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        List<ContractServiceLineClaimFieldSelection> GetClaimFieldSelection(ContractServiceLineClaimFieldSelection claimFieldSelection);
    }
}
