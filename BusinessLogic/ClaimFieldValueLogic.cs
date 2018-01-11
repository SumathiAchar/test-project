using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ClaimFieldValueLogic
    {
        /// <summary>
        /// The _claim field values repository
        /// </summary>
        private readonly IClaimFieldValueRepository _claimFieldValuesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldValueLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimFieldValueLogic(string connectionString)
        {
            _claimFieldValuesRepository = Factory.CreateInstance<IClaimFieldValueRepository>(connectionString, true);
        }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldValueLogic"/> class.
        /// </summary>
        /// <param name="claimFieldValueRepository">The claim field value repository.</param>
        public ClaimFieldValueLogic(IClaimFieldValueRepository claimFieldValueRepository)
        {
            if (claimFieldValueRepository != null)
            {
                _claimFieldValuesRepository = claimFieldValueRepository;
            }
        }
        /// <summary>
        /// Adds the claim field values.
        /// </summary>
        /// <param name="claimFieldValues">The claim field values.</param>
        /// <returns></returns>
        public long AddClaimFieldValues(ClaimFieldValue claimFieldValues)
        {
            return _claimFieldValuesRepository.AddClaimFieldValues(claimFieldValues);
        }
    }
}
