using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IClaimFieldValueRepository
    {
        /// <summary>
        /// Adds the claim field values.
        /// </summary>
        /// <param name="claimFieldValues">The claim field values.</param>
        /// <returns></returns>
        long AddClaimFieldValues(ClaimFieldValue claimFieldValues);
    }
}
