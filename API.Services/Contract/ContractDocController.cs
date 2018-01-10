/************************************************************************************************************/
/**  Author         : 
/**  Created        : 12-Aug-2013
/**  Summary        : Handles documents added , delete or view related to Contract
/**  User Story Id  : 5.User Story Add a new contract Figure 13
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
    public class ContractDocController : BaseController
    {
        private readonly ContractDocumentLogic _contractLogic;
        ContractDocController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractLogic = new ContractDocumentLogic(bubbleDataSource);
        }

        /// <summary>
        /// This method will add/edit ContractDocs in database and return inserted/updated id
        /// </summary>
        /// <returns>inserted/updated id</returns>
        [HttpPost]
        public ContractDoc AddEditContractDocs(ContractDoc contractDocs)
        {
            return _contractLogic.AddEditContractDocs(contractDocs);
        }

        /// <summary>
        /// Delete Contract doc by Id
        /// </summary>
        /// <param name="contractDocs">contractDocId</param>
        /// <returns>true/false</returns>
        [HttpPost]
        public bool DeleteContractDoc(ContractDoc contractDocs)
        {
            return _contractLogic.DeleteContractDoc(contractDocs);
        }

        /// <summary>
        /// Gets the contract document by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ContractDoc GetContractDocById(long id)
        {
            return _contractLogic.GetContractDocById(id);
        }
    }
}
