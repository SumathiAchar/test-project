using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceLineTableSelectionLogic
    {
        /// <summary>
        /// The _service line table selection details repository
        /// </summary>
        private readonly IServiceLineTableSelectionRepository _serviceLineTableSelectionDetailsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineTableSelectionLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServiceLineTableSelectionLogic(string connectionString)
        {
            _serviceLineTableSelectionDetailsRepository = Factory.CreateInstance<IServiceLineTableSelectionRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineTableSelectionLogic"/> class.
        /// </summary>
        /// <param name="serviceLineTableSelectionDetailsRepository">The service line table selection details repository.</param>
        public ServiceLineTableSelectionLogic(IServiceLineTableSelectionRepository serviceLineTableSelectionDetailsRepository)
        {
            if (serviceLineTableSelectionDetailsRepository != null)
            {
                _serviceLineTableSelectionDetailsRepository = serviceLineTableSelectionDetailsRepository;
            }
        }

        /// <summary>
        /// Gets the claim fieldand tables.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ClaimField> GetClaimFieldAndTables(ContractServiceLineTableSelection contract)
        {
            return _serviceLineTableSelectionDetailsRepository.GetClaimFieldAndTables(contract);
        }

        /// <summary>
        /// Add & Edit the service line claimand tables.
        /// </summary>
        /// <param name="serviceLineClaimandTableList">The service line claimand table list.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Claimand")]
        public long AddEditServiceLineClaimAndTables(List<ContractServiceLineTableSelection> serviceLineClaimandTableList)
        {
            return _serviceLineTableSelectionDetailsRepository.AddEditServiceLineClaimAndTables(serviceLineClaimandTableList);
        }

        /// <summary>
        /// Gets the service line table selection.
        /// </summary>
        /// <param name="contractServiceLineTableSelection">The contract service line table selection.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractServiceLineTableSelection> GetServiceLineTableSelection(ContractServiceLineTableSelection contractServiceLineTableSelection)
        {
            return _serviceLineTableSelectionDetailsRepository.GetServiceLineTableSelection(contractServiceLineTableSelection);
        }

        /// <summary>
        /// Gets the table selection claim field operators.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<ClaimFieldOperator> GetTableSelectionClaimFieldOperators()
        {
            return _serviceLineTableSelectionDetailsRepository.GetTableSelectionClaimFieldOperators();
        }
    }
}
