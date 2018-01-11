using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractAlertLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IContractAlertRepository _contractAlertsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAlertLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractAlertLogic(string connectionString)
        {
            _contractAlertsRepository = Factory.CreateInstance<IContractAlertRepository>(connectionString, true);
        }


        public ContractAlertLogic(IContractAlertRepository contractAlertsRepository)
        {
            if (contractAlertsRepository != null)
            {
                _contractAlertsRepository = contractAlertsRepository;
            }
        }

        /// <summary>
        /// Gets the contract alerts.
        /// </summary>
        /// <param name="contractAlertList">The user unique identifier.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractAlert> GetContractAlerts(ContractAlert contractAlertList)
        {
            if (contractAlertList != null)
            {
                return _contractAlertsRepository.GetContractAlerts(contractAlertList);
            }
            return null;
        }



        /// <summary>
        /// Updates the contract alerts.
        /// </summary>
        /// <param name="data">ContractId and UserName</param>
        /// <returns></returns>
        public bool UpdateContractAlerts(ContractAlert data)
        {
            if (data != null)
            {
                return _contractAlertsRepository.UpdateContractAlerts(data);
            }
            return false;
        }
        
        /// <summary>
        /// Gets the count of contract alerts.
        /// </summary>
        /// <param name="getAlertCount">The get alert count.</param>
        /// <returns></returns>
        public int ContractAlertCount(ContractAlert getAlertCount)
        {
            return _contractAlertsRepository.ContractAlertCount(getAlertCount);
        }

        /// <summary>
        /// Updates the alert verified status.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        // FIXED-JAN-AA Can't we handle this update thru the above method UpdateContractAlerts itself? Is this not redundant?
        // It is different thing from above methods.
        public bool UpdateAlertVerifiedStatus(ContractAlert contract)
        {
            return _contractAlertsRepository.UpdateAlertVerifiedStatus(contract);
        }
       
    }
}
