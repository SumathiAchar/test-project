using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractPayerMapReportLogic
    {
        private readonly IContractPayerMapReportRepository _payerMappingReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractPayerMapReportLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractPayerMapReportLogic(string connectionString)
        {
            _payerMappingReportRepository = Factory.CreateInstance<IContractPayerMapReportRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractPayerMapReportLogic"/> class.
        /// </summary>
        /// <param name="payerMappingReportRepository">The payer mapping report repository.</param>
        public ContractPayerMapReportLogic(IContractPayerMapReportRepository payerMappingReportRepository)
        {
            if (payerMappingReportRepository != null)
            {
                _payerMappingReportRepository = payerMappingReportRepository;
            }
        }

        /// <summary>
        /// Gets all modeling details.
        /// </summary>
        /// <param name="contractPayerMapReport">payer mapping report report.</param>
        /// <returns></returns>
        public ContractPayerMapReport Get(ContractPayerMapReport contractPayerMapReport)
        {
            return _payerMappingReportRepository.Get(contractPayerMapReport);
        }
    }
}
