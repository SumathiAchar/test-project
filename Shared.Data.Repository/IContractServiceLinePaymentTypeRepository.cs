using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractServiceLinePaymentTypeRepository : IDisposable
    {

        /// <summary>
        /// Deletes the contract service calculate inesand payment types by unique identifier.
        /// </summary>
        /// <param name="contractServiceLinePaymentType">Type of the contract service line payment.</param>
        /// <returns></returns>
        bool DeleteContractServiceLinesAndPaymentTypes(
            ContractServiceLinePaymentType contractServiceLinePaymentType);

    }
}
