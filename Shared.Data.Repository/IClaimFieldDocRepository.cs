using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IClaimFieldDocRepository : IDisposable
    {

        /// <summary>
        /// Adds the claim field docs.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        long AddClaimFieldDocs(ClaimFieldDoc data);


        /// <summary>
        /// Gets the table look up details by contract unique identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<ClaimFieldDoc> GetClaimFieldDocs(ClaimFieldDoc data);

        /// <summary>
        /// Get all claim fields
        /// </summary>
        /// <returns></returns>
        List<ClaimField> GetAllClaimFields();

        /// <summary>
        /// Deletes the specified claim field document.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        bool Delete(ClaimFieldDoc claimFieldDoc);


        /// <summary>
        /// Determines whether [is document in use] [the specified claim field document].
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        List<ContractLog> IsDocumentInUse(ClaimFieldDoc claimFieldDoc);

        /// <summary>
        /// Rename Payment Table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        ClaimFieldDoc RenamePaymentTable(ClaimFieldDoc claimFieldDoc);
    }
}
