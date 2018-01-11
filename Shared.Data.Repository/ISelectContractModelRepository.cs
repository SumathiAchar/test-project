using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface ISelectContractModelRepository : IDisposable
    {


        /// <summary>
        /// Gets all contract models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<ContractModel> GetAllContractModels(ContractModel data);

        /// <summary>
        /// Gets all contract facilities.
        /// </summary>
        /// <returns></returns>
        List<FacilityDetail> GetAllContractFacilities(ContractHierarchy data);
    }
}
