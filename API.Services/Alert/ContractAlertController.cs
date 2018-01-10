using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Alert
{
    // ReSharper disable once UnusedMember.Global
    public class ContractAlertController : BaseController
    {
        /// <summary>
        /// The _contarct alerts logic
        /// </summary>
        private readonly ContractAlertLogic _contarctAlertsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAlertController"/> class.
        /// </summary>
        public ContractAlertController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _contarctAlertsLogic = new ContractAlertLogic(bubbleDataSource);
        }


        /// <summary>
        /// Gets the contract alerts.
        /// </summary>
        /// <param name="contractAlertList">The unique identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ContractAlert> GetContractAlerts(ContractAlert contractAlertList)
        {
            return _contarctAlertsLogic.GetContractAlerts(contractAlertList);
        }


        /// <summary>
        /// Updates the contract alerts.
        /// </summary>
        /// <param name="contractAlertList">The contract alert list.</param>
        /// <returns></returns>
        public bool UpdateContractAlerts(ContractAlert contractAlertList)
        {
            return _contarctAlertsLogic.UpdateContractAlerts(contractAlertList);
        }

        /// <summary>
        /// Gets the Contract alerts count.
        /// </summary>
        /// <param name="getAlertCount">The get alert count.</param>
        /// <returns></returns>
        public int ContractAlertCount(ContractAlert getAlertCount)
        {
            return _contarctAlertsLogic.ContractAlertCount(getAlertCount);
        }

        /// <summary>
        /// Updates the alert verified status.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public bool UpdateAlertVerifiedStatus(ContractAlert contract)
        {
            return _contarctAlertsLogic.UpdateAlertVerifiedStatus(contract);
        }
        
        
    }
}
