using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractServiceTypeRepository : IDisposable
    {
        /// <summary>
        /// Gets the type of all contract service.
        /// </summary>
        /// <param name="contractId">The contract unique identifier.</param>
        /// <returns></returns>
        List<ContractServiceType> GetAllContractServiceType(long contractId);
        /// <summary>
        /// Adds the type of the edit contract service.
        /// </summary>
        /// <param name="contractServiceTypes">The contract service types.</param>
        /// <returns></returns>
        Int64 AddEditContractServiceType(ContractServiceType contractServiceTypes);


        /// <summary>
        /// Copies the Contract Service Type by Contract ServiceType id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        long CopyContractServiceType(ContractServiceType data);


        /// <summary>
        /// 
        /// Rename the Contract Service Type by Contract ServiceType id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        long RenameContractServiceType(ContractServiceType data);

        /// <summary>
        /// Gets the contract service type details.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ContractServiceType GetContractServiceTypeDetails(ContractServiceType data);


        /// <summary>
        /// Checks the contract service type name is unique.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        bool IsContractServiceTypeNameExit(ContractServiceType data);

        ///// <summary>
        ///// Edits the type of the contract service.
        ///// </summary>
        ///// <param name="contractServiceTypes">The contract service types.</param>
        ///// <returns></returns>
        //Int64 EditContractServiceType(ContractServiceType contractServiceTypes);
    }
}
