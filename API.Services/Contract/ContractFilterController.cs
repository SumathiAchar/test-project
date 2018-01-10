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
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractFilterController : BaseController
    {
        /// <summary>
        /// The contract filters logic
        /// </summary>
        private readonly ContractFilterLogic _contractFiltersLogic;

        /// <summary>
        /// Prevents a default instance of the <see cref="ContractFilterController"/> class from being created.
        /// </summary>
        private ContractFilterController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractFiltersLogic = new ContractFilterLogic(bubbleDataSource);
        }


        /// <summary>
        /// Gets the contract filters data based on contract id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ContractFilter> GetContractFiltersDataBasedOnContractId(ContractServiceType data)
        {
            if (data != null)
                return _contractFiltersLogic.GetContractFiltersData(data);
            return new List<ContractFilter>();
        }
    }
}
