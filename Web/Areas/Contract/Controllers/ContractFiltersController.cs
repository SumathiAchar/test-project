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

using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractFiltersController : CommonController
    {
        //
        // GET: /ContractFilters/

        public ActionResult ContractFilters()
        {
            return View();
        }

        /// <summary>
        /// Gets the contract filters data based on contract id.
        /// </summary>
        /// <param name="contractId">The contract id.</param>
        /// <param name="contractServiceTypeId">The contract service type id.</param>
        /// <returns></returns>
        public ActionResult GetContractFiltersDataBasedOnContractId(long contractId, long contractServiceTypeId)
        {
            // When Contract Service Type ID is not null, send Contract Service ID and Contract ID = 0 Else send Contract ID and Contract Service Type ID = 0
            // Send two parameter
            ContractServiceType data = new ContractServiceType
            {
                ContractId = contractId,
                ContractServiceTypeId = contractServiceTypeId,
                UserName = GetCurrentUserName()
            };

            //Get the Name of User logged in

            List<ContractFilter> response = PostApiResponse<List<ContractFilter>>("ContractFilter",
                                                                                    "GetContractFiltersDataBasedOnContractId",
                                                                                    data);
            List<ContractFiltersViewModel> contractFiltersList =
                AutoMapper.Mapper.Map<List<ContractFilter>, List<ContractFiltersViewModel>>(response);

            return View("ContractServiceFilterList", contractFiltersList);

        }

        /// <summary>
        /// Delete Filters at either contract level or service type level
        /// </summary>
        /// <returns>Json result as true/false</returns>
        public JsonResult DeleteContractServiceTypeFilter(long contractserviceId, long contractId, long serviceLineTypeId, long paymentTypeId)
        {
            ContractServiceLinePaymentType contractServiceLinePaymentTypes = new ContractServiceLinePaymentType
            {
                ContractServiceTypeId =
                    contractserviceId,
                ContractId = contractId,
                ServiceLineTypeId =
                    serviceLineTypeId,
                PaymentTypeId = paymentTypeId,
                UserName = GetCurrentUserName()
            };
            //Get the Name of User logged in

            bool isSuccess = PostApiResponse<bool>("ContractServiceLinePaymentTypes",
                                              "DeleteContractServiceLInesandPaymentTypesById",
                                              contractServiceLinePaymentTypes);

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
    }
}
