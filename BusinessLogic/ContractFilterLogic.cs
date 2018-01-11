/************************************************************************************************************/
/**  Author         : Vishesh Bhawsar
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Contract Filters info
/**  User Story Id  : 5.User Story Add a new contract Figure 15
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractFilterLogic
    {
        /// <summary>
        /// Initializes repository
        /// </summary>
        private readonly IContractFilterRepository _contractFiltersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractFilterLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractFilterLogic(string connectionString)
        {
            _contractFiltersRepository = Factory.CreateInstance<IContractFilterRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractFilterLogic"/> class.
        /// </summary>
        /// <param name="contractRepository">The contract repository.</param>
        public ContractFilterLogic(IContractFilterRepository contractRepository)
        {
            if (contractRepository != null)
            {
                _contractFiltersRepository = contractRepository;
            }
        }


        /// <summary>
        /// Gets the contract filters data based on contract id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractFilter> GetContractFiltersData(ContractServiceType data)
        {
            List<ContractFilter> contractFiltersData =
                _contractFiltersRepository.GetContractFilters(data);
            return contractFiltersData;
        }
    }
}
