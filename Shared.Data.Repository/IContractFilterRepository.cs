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


using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractFilterRepository : IDisposable
    {

        /// <summary>
        /// Gets the contract filters data .
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<ContractFilter> GetContractFilters(ContractServiceType data);
    }
}
