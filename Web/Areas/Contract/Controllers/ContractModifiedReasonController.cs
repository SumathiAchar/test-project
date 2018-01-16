using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractModifiedReasonController : SessionStoreController
    {
        //
        // GET: /ContractModifiedReason/

        public ActionResult Index(long? contractId, long? nodeId, int facilityId)//TODO Janaki
        {
            LastExpandedNodeId = nodeId;
            LastHighlightedNodeId = nodeId;
            ContractModifiedReasonViewModel contractModifiedReasonViewModel = new ContractModifiedReasonViewModel
            {
                ContractId = contractId,
                NodeId = nodeId,
                FacilityId = facilityId
            };
            return View(contractModifiedReasonViewModel);
        }

        /// <summary>
        /// Adds the modified reason.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddModifiedReason(ContractModifiedReasonViewModel info)
        {
            ContractModifiedReason contractModifiedReason = AutoMapper.Mapper.Map<ContractModifiedReasonViewModel, ContractModifiedReason>(info);
            //Get the UserName logged in
            contractModifiedReason.UserName =GetLoggedInUserName();
            int result = PostApiResponse<int>("ContractModifiedReason", "AddModifiedReason", contractModifiedReason);
            LastExpandedNodeId = info.NodeId;
            LastHighlightedNodeId = info.NodeId;
            return result > 0 ? Json(new { sucess = true, Id = result }) : Json(new { sucess = false });
        }

        /// <summary>
        /// Copies the primary model.
        /// </summary>
        /// <returns></returns>
        public ActionResult CopyPrimaryModel()
        {
            return View();
        }
    }
}
