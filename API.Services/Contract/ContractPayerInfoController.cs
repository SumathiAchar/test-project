/************************************************************************************************************/
/**  Author         : 
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract payers info , it add/modify multiple instances of payer contact information
/**  User Story Id  : 5.User Story Add a new contract Figure 11
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractPayerInfoController : BaseController
    {
        /// <summary>
        /// The _contract logic
        /// </summary>
        private readonly ContractPayerInfoLogic _contractLogic;
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractPayerInfoController"/> class.
        /// </summary>
        public ContractPayerInfoController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractLogic = new ContractPayerInfoLogic(bubbleDataSource);
        }
        /// <summary>
        /// This method will add/edit payerinfo in database and return inserted/updated id
        /// </summary>
        /// <param name="contractPayerInfo">Contract Product Info object</param>
        /// <returns>
        /// inserted/updated id
        /// </returns>
        [HttpPost]
        public long AddEditContractPayerInfo(ContractPayerInfo contractPayerInfo)
        {
            return _contractLogic.AddContractPayerInfo(contractPayerInfo);
        }

        /// <summary>
        /// Delete Contract payer info by Id
        /// </summary>
        /// <param name="contractPayerInfo">contractPayerInfoId</param>
        /// <returns>true/false</returns>
        [HttpPost]
        public bool DeleteContractPayerInfoById(ContractPayerInfo contractPayerInfo)
        {
            return _contractLogic.DeleteContractPayerInfo(contractPayerInfo);
        }

        /// <summary>
        /// Get Contract Payer Info based on contractPayerId
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// ContractPayerInfo object
        /// </returns>
        public ContractPayerInfo GetContractPayerInfo(long id)
        {
            return _contractLogic.GetContractPayerInfo(id);
        }
    }
}
