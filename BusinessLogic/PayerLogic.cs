using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class PayerLogic
    {
        private readonly IPayerRepository _payerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayerLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PayerLogic(string connectionString)
        {
            _payerRepository = Factory.CreateInstance<IPayerRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayerLogic"/> class.
        /// </summary>
        /// <param name="payerRepository">The payer repository.</param>
        public PayerLogic(IPayerRepository payerRepository)
        {
            if (payerRepository != null)
                _payerRepository = payerRepository;
        }

        /// <summary>
        /// Gets the payers.
        /// </summary>
        /// <param name="selectedFacility">The selected facility.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Payer> GetPayers(ContractServiceLineClaimFieldSelection selectedFacility)
        {
            return _payerRepository.GetPayers(selectedFacility);
        }
    }
}
