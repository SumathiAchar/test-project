using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// interface for the contract log
    /// </summary>
    public interface IContractLogLogic
    {
        /// <summary>
        /// Adds the contract log.
        /// </summary>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="contractList">The contract list.</param>
        /// <param name="?">The ?.</param>
        /// <returns></returns>
        bool AddContractLog(Dictionary<long, List<PaymentResult>> paymentResultDictionary, List<Contract> contractList
            );
    }
}
