using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IServiceLineTableSelectionRepository : IDisposable
    {
        /// <summary>
        /// Gets the claim fieldand tables.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        List<ClaimField> GetClaimFieldAndTables(ContractServiceLineTableSelection contract);

        /// <summary>
        /// Adds the service line claimand tables.
        /// </summary>
        /// <param name="serviceLineClaimandTableList">The service line claimand table list.</param>
        /// <returns></returns>
        long AddEditServiceLineClaimAndTables(List<ContractServiceLineTableSelection> serviceLineClaimandTableList);

        /// <summary>
        /// Gets the service line table selection.
        /// </summary>
        /// <param name="contractServiceLineTableSelection">The contract service line table selection.</param>
        /// <returns></returns>
        List<ContractServiceLineTableSelection> GetServiceLineTableSelection(ContractServiceLineTableSelection contractServiceLineTableSelection);

        /// <summary>
        /// Gets the table selection claim field operators.
        /// </summary>
        /// <returns></returns>
        List<ClaimFieldOperator> GetTableSelectionClaimFieldOperators();
    }
}
