using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractServiceLinePaymentTypeLogic
    {

        /// <summary>
        /// The _contract service line payment types repository
        /// </summary>
        private readonly IContractServiceLinePaymentTypeRepository _contractServiceLinePaymentTypesRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContractServiceLinePaymentTypeLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractServiceLinePaymentTypeLogic(string connectionString)
        {
            _contractServiceLinePaymentTypesRepository = Factory.CreateInstance<IContractServiceLinePaymentTypeRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractServiceLinePaymentTypeLogic"/> class.
        /// </summary>
        /// <param name="contractServiceLinePaymentTypesRepository">The contract service line payment types repository.</param>
        public ContractServiceLinePaymentTypeLogic(IContractServiceLinePaymentTypeRepository contractServiceLinePaymentTypesRepository)
        {
            if (contractServiceLinePaymentTypesRepository != null)
            {
                _contractServiceLinePaymentTypesRepository = contractServiceLinePaymentTypesRepository;
            }
        }


        /// <summary>
        /// Deletes the contract service calculate inesand payment types by unique identifier.
        /// </summary>
        /// <param name="contractServiceLinePaymentTypes">The contract service line payment types.</param>
        /// <returns></returns>
        public bool DeleteContractServiceLinesAndPaymentTypes(ContractServiceLinePaymentType contractServiceLinePaymentTypes)
        {
            return _contractServiceLinePaymentTypesRepository.DeleteContractServiceLinesAndPaymentTypes(contractServiceLinePaymentTypes);
        }
    }
}
