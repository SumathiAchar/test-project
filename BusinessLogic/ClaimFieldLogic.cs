using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ClaimFieldLogic
    {
        private readonly IClaimFieldRepository _claimFieldRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimFieldLogic(string connectionString)
        {
            _claimFieldRepository = Factory.CreateInstance<IClaimFieldRepository>(connectionString, true);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldLogic"/> class.
        /// </summary>
        /// <param name="claimFieldRepository">The appeal letter repository.</param>
        public ClaimFieldLogic(IClaimFieldRepository claimFieldRepository)
        {
            if (claimFieldRepository != null)
            {
                _claimFieldRepository = claimFieldRepository;
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ClaimField> GetClaimFieldsByModule(int moduleId)
        {
            return _claimFieldRepository.GetClaimFieldsByModule(moduleId);
        }
    }
}