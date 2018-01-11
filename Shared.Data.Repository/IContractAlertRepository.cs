using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractAlertRepository : IDisposable
    {

        /// <summary>
        /// Gets the contract alerts.
        /// </summary>
        /// <param name="contractAlertList">The user unique identifier.</param>
        /// <returns></returns>
        List<ContractAlert> GetContractAlerts(ContractAlert contractAlertList);


        /// <summary>
        /// Updates the contract alerts.
        /// </summary>
        /// <param name="data">ContractId and UserName.</param>
        /// <returns></returns>
        bool UpdateContractAlerts(ContractAlert data);

        /// <summary>
        /// Gets count for ContractAlerts
        /// </summary>
        /// <param name="getAlertCount">The get alert count.</param>
        /// <returns></returns>
        int ContractAlertCount(ContractAlert getAlertCount);

        /// <summary>
        /// Updates the alert verified status.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        bool UpdateAlertVerifiedStatus(ContractAlert contract);
        
        
    }
}
