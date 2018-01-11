/************************************************************************************************************/
/**  Author         : Manjunath
/**  Created        : 19-Sep-2013
/**  Summary        : Handles insertion of contractLogs infromation
/**  User Story Id  : 
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
    /// <summary>
    /// Repository from Contract Log
    /// </summary>
    public interface IContractLogRepository : IDisposable
    {
        /// <summary>
        /// Adds the contract log.
        /// </summary>
        /// <param name="contractLogList">The contract log list.</param>
        /// <returns></returns>
        bool AddContractLog(List<ContractLog> contractLogList);
    }
}
