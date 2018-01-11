using System.Collections.Generic;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Shared.Data.Repository;

namespace SSI.ContractManagement.BusinessLogic
{

    //REVIEW-MAY This class should be called as ContractServiceLineLogic and there should be no separte model called ServiceLineCode. ServiceLineCode is redundant.
    public class ServiceLineCodeLogic
    {
        private readonly IServiceLineCodeRepository _serviceLineCodeDetailsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineCodeLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServiceLineCodeLogic(string connectionString)
        {
            _serviceLineCodeDetailsRepository = Factory.CreateInstance<IServiceLineCodeRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineCodeLogic"/> class.
        /// </summary>
        /// <param name="serviceLineCodeDetailsRepository">The service line code details repository.</param>
        public ServiceLineCodeLogic(IServiceLineCodeRepository serviceLineCodeDetailsRepository)
        {
            if (serviceLineCodeDetailsRepository != null)
            {
                _serviceLineCodeDetailsRepository = serviceLineCodeDetailsRepository;
            }
        }


        /// <summary>
        /// Adds the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        /// <returns></returns>
        public long AddEditServiceLineCodeDetails(ContractServiceLine contractServiceLine)
        {
            return _serviceLineCodeDetailsRepository.AddEditServiceLineCodeDetails(contractServiceLine);
        }

        /// <summary>
        /// Gets the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        /// <returns></returns>
        public ContractServiceLine GetServiceLineCodeDetails(ContractServiceLine contractServiceLine)
        {
            return _serviceLineCodeDetailsRepository.GetServiceLineCodeDetails(contractServiceLine);
        }

        /// <summary>
        /// Gets all service line code details by contract id.
        /// </summary>
        /// <param name="contractId">The contract id.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractServiceLine> GetAllServiceLineCodeDetailsByContractId(long contractId)
        {
            return _serviceLineCodeDetailsRepository.GetAllServiceLineCodeDetailsByContractId(contractId);
        }

        /// <summary>
        /// Gets all ServiceLineCodes.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ServiceLineCode> GetAllServiceLineCodes(long? serviceLineTypeId, int pageSize, int pageIndex)
        {
            List<ServiceLineCode> allServiceLineCodes =  new List<ServiceLineCode>();
            if (_serviceLineCodeDetailsRepository != null)
                if (serviceLineTypeId != null)
                    return _serviceLineCodeDetailsRepository.GetAllServiceLineCodes(serviceLineTypeId.Value, pageSize, pageIndex);
            return allServiceLineCodes;
        }
    }
}
