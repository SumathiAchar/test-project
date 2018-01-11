using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IServiceLineCodeRepository : IDisposable
    {
        /// <summary>
        /// Adds the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        long AddEditServiceLineCodeDetails(ContractServiceLine contractServiceLine);
        
        /// <summary>
        /// Gets the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        ContractServiceLine GetServiceLineCodeDetails(ContractServiceLine contractServiceLine);

        /// <summary>
        /// Gets all service line code details by contract id.
        /// </summary>
        /// <param name="contractId">The contract id.</param>
        /// <returns></returns>
        List<ContractServiceLine> GetAllServiceLineCodeDetailsByContractId(long contractId);

        /// <summary>
        /// Gets all ServiceLine Codes.
        /// </summary>
        /// <returns></returns>
        List<ServiceLineCode> GetAllServiceLineCodes(long serviceLineTypeId, int pageSize, int pageIndex);



    }
}
