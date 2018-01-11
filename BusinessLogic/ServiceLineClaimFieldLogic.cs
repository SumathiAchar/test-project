using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ServiceLineClaimFieldLogic
    {
        private readonly IServiceLineClaimFieldRepository _serviceLineClaimFieldRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineClaimFieldLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServiceLineClaimFieldLogic(string connectionString)
        {
            _serviceLineClaimFieldRepository = Factory.CreateInstance<IServiceLineClaimFieldRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineClaimFieldLogic"/> class.
        /// </summary>
        /// <param name="serviceLineClaimFieldRepository">The service line claim field repository.</param>
        public ServiceLineClaimFieldLogic(IServiceLineClaimFieldRepository serviceLineClaimFieldRepository)
        {
            if (serviceLineClaimFieldRepository != null)
            {
                _serviceLineClaimFieldRepository = serviceLineClaimFieldRepository;
            }
        }


        /// <summary>
        /// Adds the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public long AddNewClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection)
        {
            return _serviceLineClaimFieldRepository.AddNewClaimFieldSelection(claimFieldSelection);
        }



        /// <summary>
        /// Edit the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public long EditClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection)
        {
            return _serviceLineClaimFieldRepository.EditClaimFieldSelection(claimFieldSelection);
        }


        /// <summary>
        /// Gets the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractServiceLineClaimFieldSelection> GetClaimFieldSelection(ContractServiceLineClaimFieldSelection claimFieldSelection)
        {
            return _serviceLineClaimFieldRepository.GetClaimFieldSelection(claimFieldSelection);
        }

    }
}
